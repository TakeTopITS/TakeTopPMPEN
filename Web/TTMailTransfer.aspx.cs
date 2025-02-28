using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;

using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using Npgsql;

public partial class TTMailTransfer : System.Web.UI.Page
{
    int nMailID = -1;
    int nFolderID = -1;
    int nFromMailID = -1;
    string strUserCode;

    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strIdentifyString;

        //CKEditor��ʼ��
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(CKEditor1);

        strUserCode = Session["UserCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        ///��ȡ����nFolderID��ֵ
        if (Request.Params["FolderID"] != null)
        {
            if (Int32.TryParse(Request.Params["FolderID"].ToString(), out nFolderID) == false)
            {
                return;
            }
        }
        ///��ȡ����nMailID��ֵ
        if (Request.Params["MailID"] != null)
        {
            if (Int32.TryParse(Request.Params["MailID"].ToString(), out nMailID) == false)
            {
                return;
            }
            else
            {
                nFromMailID = int.Parse(Request.Params["MailID"].ToString());
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HTEditor1.Visible = true;
            }
            else
            {
                CKEditor1.Visible = true;
            }
            ///��ʾ�ʼ�����
            if (nMailID > -1)
            {
                try
                {
                    BindMailData(nMailID);
                    UpdateReadStatus(nMailID.ToString());
                }
                catch
                {
                }
            }


            LB_UserCode.Text = strUserCode;
            LoadContactList(strUserCode);

            LB_IdentifyString.Text = DateTime.Now.ToString("yyyyMMddHHMMssff");
            strIdentifyString = LB_IdentifyString.Text.Trim();

            strHQL = "Insert into T_MailAttachment(Name,Url,Type,Contain,MailID,IdentifyString) ";
            strHQL += " Select Name,Url,Type,Contain,0," + "'" + strIdentifyString + "'" + " from T_MailAttachment Where MailID = " + nFromMailID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Select AttachmentID,Name,Url From T_MailAttachment Where IdentifyString = " + "'" + strIdentifyString + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailAttachment");
            DataGrid4.DataSource = ds;
            DataGrid4.DataBind();
        }
    }

    private void BindMailData(int nMailID)
    {

        string strBody;
        string stMailSignInfo;


        stMailSignInfo = GetMailSignInfo(strUserCode, "InProgress");

        IMail mail = new Mail();
        NpgsqlDataReader dr = mail.GetSingleMail(nMailID);
        if (dr.Read())
        {
            Title.Text = LanguageHandle.GetWord("ZhuaiFa").ToString().Trim() + dr["Title"].ToString();
            //CC.Text = dr["CCAddress"].ToString();
            //To.Text = dr["ToAddress"].ToString();

            strBody = "<BR/><BR/>" + stMailSignInfo + "<BR/>" + "-------On " + DateTime.Now.ToString() + ",the mail transfer from as belows:" + "</n>" + dr["Body"].ToString();

            if (strIsMobileDevice == "YES")
            {
                HTEditor1.Text = strBody;
            }
            else
            {
                CKEditor1.Text = strBody;
            }

            HtmlCB.Checked = bool.Parse(dr["HTMLFormat"].ToString().ToLower());

        }
        dr.Close();
    }

    protected void ReturnBtn_Click(object sender, EventArgs e)
    {   ///���ص��ʼ��б�ҳ��
        Response.Redirect("~/TTMailDesktop.aspx");
        //Response.Write("<script language=javascript>history.go(-2);</script>");
    }

    protected void UpdateReadStatus(string strMailID)
    {
        string strHQL;
        IList lst;
        int intFolderID, intNewFolderID, intReaderFlag;
        string strKeyWord;


        strHQL = "FROM Mails as mails where mails.MailID = " + strMailID;
        MailsBLL mailsBLL = new MailsBLL();
        lst = mailsBLL.GetAllMailss(strHQL);
        Mails mails = (Mails)lst[0];

        intReaderFlag = mails.ReaderFlag;
        intFolderID = mails.FolderID;

        strKeyWord = GetMailFolderKeyWord(strUserCode, intFolderID.ToString());

        if (intReaderFlag == 0)
        {
            mails.ReaderFlag = 1;

            //���ʼ����򿪴����Ķ����Զ�ת�롰�����ʼ���
            if (strKeyWord == "New")
            {
                intNewFolderID = GetMailFolderID(strUserCode, "Read");
                mails.FolderID = intNewFolderID;
            }

            mailsBLL.UpdateMails(mails, int.Parse(strMailID));

            if (strKeyWord == "New")
            {
                strHQL = "Update T_MailFolder Set NoReader = NoReader - 1 Where FolderID = " + intFolderID.ToString();
                ShareClass.RunSqlCommand(strHQL);
            }
        }
    }

    protected int GetMailFolderID(string strUserCode, string strKeyWord)
    {
        string strHQL;


        strHQL = "Select FolderID From T_MailFolder Where OwnerCode = " + "'" + strUserCode + "'" + " And KeyWord = " + "'" + strKeyWord + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailFolderID");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString().Trim());
    }

    protected string GetMailFolderKeyWord(string strUserCode, string strFoldID)
    {
        string strHQL;


        strHQL = "Select KeyWord From T_MailFolder Where FolderID = " + strFoldID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailFolderID");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }


    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strUserCode = LB_UserCode.Text.Trim();

            string strFileName1, strExtendName;
            string strIdentifyString;

            string strHQL;

            strIdentifyString = LB_IdentifyString.Text.Trim();


            strFileName1 = this.AttachFile.FileName;//��ȡ�ϴ��ļ����ļ���,������׺

            strExtendName = System.IO.Path.GetExtension(strFileName1);//��ȡ��չ��

            DateTime dtUploadNow = DateTime.Now; //��ȡϵͳʱ��

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


            string strAttachDocURL = "Doc/" + DateTime.Now.ToString("yyyyMM") + "/" + strUserCode + "/Doc/" + strFileName3;


            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
            }
            else
            {
                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);


                    strHQL = "Insert into T_MailAttachment(Name,Url,Type,Contain,MailID,IdentifyString) ";
                    strHQL += " Values(" + "'" + strFileName2 + "'" + "," + "'" + strAttachDocURL + "'" + ",'Doc'," + "0,0," + "'" + strIdentifyString + "'" + ")";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Select AttachmentID,Name,Url From T_MailAttachment Where IdentifyString = " + "'" + strIdentifyString + "'";
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailAttachment");
                    DataGrid4.DataSource = ds;
                    DataGrid4.DataBind();

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "');</script>");
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "');</script>");
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[0].Text.Trim();
            string strIdentityString = LB_IdentifyString.Text.Trim();

            string strHQL;

            strHQL = "Delete From T_MailAttachment Where AttachmentID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Select AttachmentID,Name,Url From T_MailAttachment Where IdentifyString = " + "'" + strIdentityString + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailAttachment");
            DataGrid4.DataSource = ds;
            DataGrid4.DataBind();
        }
    }



    protected void NewBtn_Click(object sender, EventArgs e)
    {
        int nContain = 0;
        string strUserCode, strHQL;
        string from;

        IList lst;
        int nMailID, intAttachmentCount;
        string strDocSavePath, strAttachFileFullPath, strNewFullFileName, strAttachmentUrlList = "";
        string strAttachDocFullURL;
        string strNowTime;
        string strEnableSMTPSSL;

        strUserCode = Session["UserCode"].ToString();
        strDocSavePath = Server.MapPath("Doc");
        strNowTime = DateTime.Now.ToString("MMss");
        intAttachmentCount = 0;

        Folder folder = new Folder();
        FileInfo f;

        strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        lst = mailProfileBLL.GetAllMailProfiles(strHQL);
        MailProfile mailProfile = (MailProfile)lst[0];

        strEnableSMTPSSL = mailProfile.EnableSMTPSSL.Trim();

        ///��ӷ����˵�ַ
        from = mailProfile.Email.Trim();

        MailMessage mailMsg = new MailMessage();

        mailMsg.From = new MailAddress(from, mailProfile.UserName.Trim());
        mailMsg.To.Add(To.Text.Trim());
        nContain += To.Text.Length;

        if (CC.Text.Trim() != "")
        {
            mailMsg.CC.Add(CC.Text.Trim());
            nContain += CC.Text.Length;
        }
        else
        {
            //mailMsg.CC.Add(To.Text.Trim());
        }


        ///����ʼ�����
        mailMsg.Subject = Title.Text.Trim();
        nContain += Title.Text.Length;

        ///����ʼ�����
        ///

        if (strIsMobileDevice == "YES")
        {
            mailMsg.Body = HTEditor1.Text;
            mailMsg.BodyEncoding = Encoding.UTF8;
            mailMsg.IsBodyHtml = true;

            nContain += HTEditor1.Text.Length;
        }
        else
        {
            mailMsg.Body = CKEditor1.Text;
            mailMsg.BodyEncoding = Encoding.UTF8;
            mailMsg.IsBodyHtml = true;

            nContain += CKEditor1.Text.Length;
        }


        //���ʼ�����
        Attachment attachment;

        string strIdentifyString = LB_IdentifyString.Text.Trim();

        strHQL = "From Attachments as attachments where attachments.IdentifyString = " + "'" + strIdentifyString + "'";
        AttachmentsBLL attachmentsBLL = new AttachmentsBLL();
        lst = attachmentsBLL.GetAllAttachmentss(strHQL);

        Attachments attachments = new Attachments();

        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                try
                {
                    attachments = (Attachments)lst[i];
                    strAttachFileFullPath = Server.MapPath(attachments.Url.Trim());

                    f = new FileInfo(strAttachFileFullPath);

                    if (CB_BigAttachment.Checked != true)
                    {
                        attachment = new Attachment(strAttachFileFullPath);
                        mailMsg.Attachments.Add(attachment);
                    }
                    else
                    {
                        strAttachDocFullURL = Request.ServerVariables["HTTP_HOST"] + Request.ApplicationPath + "/" + attachments.Url.Trim();
                        strAttachmentUrlList += "</br>������<a href=http://" + strAttachDocFullURL + " target=_blank >" + attachments.Name.Trim() + "</a>";   //ChineseWord
                    }

                    nContain += int.Parse(f.Length.ToString());
                }
                catch
                {
                }
            }
        }

        if (strIsMobileDevice == "YES")
        {
            HTEditor1.Text += strAttachmentUrlList;
            mailMsg.Body = HTEditor1.Text;
        }
        else
        {
            CKEditor1.Text += strAttachmentUrlList;
            mailMsg.Body = CKEditor1.Text;
        }

        nContain += 100;

        try
        {
            IMail mail = new Mail();
            SmtpClient smtpClient = new SmtpClient(mailProfile.SmtpServerIP, mailProfile.SmtpServerPort);
            if (strEnableSMTPSSL == "YES")
            {
                //����SSL
                smtpClient.EnableSsl = true;
            }
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailProfile.AliasName.Trim(), mailProfile.Password.Trim());
            /*ָ����δ���������ʼ�*/
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;


            try
            {
                smtpClient.Send(mailMsg);

                if (mailMsg.Attachments.Count > 0)
                    intAttachmentCount = 1;

                //�����ʼ�
                nMailID = mail.SaveAsMail(mailMsg.Subject, mailMsg.Body, from,
                    To.Text.Trim(), CC.Text.Trim(), 1,
                    nContain, mailMsg.Attachments.Count > 0 ? 1 : 0, 1, folder.GetFolderID("Send", strUserCode), strUserCode);

            }
            catch
            {
                nMailID = mail.SaveAsMail(mailMsg.Subject, mailMsg.Body, from,
                 To.Text.Trim(), CC.Text.Trim(), 1,
                 nContain, mailMsg.Attachments.Count > 0 ? 1 : 0, 1, folder.GetFolderID("Waiting", strUserCode), strUserCode);
            }

            if (nMailID > 0)
            {
                ///���淢���ʼ��ĸ���
                ///

                strHQL = "From Attachments as attachments where attachments.IdentifyString = " + "'" + strIdentifyString + "'";
                lst = attachmentsBLL.GetAllAttachmentss(strHQL);

                for (int i = 0; i < lst.Count; i++)
                {
                    attachments = (Attachments)lst[i];

                    try
                    {
                        strAttachFileFullPath = Server.MapPath(attachments.Url.Trim());

                        f = new FileInfo(strAttachFileFullPath);

                        strNewFullFileName = System.IO.Path.GetFileName(f.FullName);


                        ///���淢���ʼ��ĸ���
                        mail.SaveAsMailAttachment(
                          strNewFullFileName,
                          attachments.Url.Trim(),
                          f.GetType().Name,
                          int.Parse(f.Length.ToString()),
                          nMailID);
                    }
                    catch
                    {
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //��ת���쳣������ҳ��
            Response.Redirect("TTErrorPage.aspx?ErrorMsg=" + ex.Message.Replace("<br>", "").Replace("\n", "")
                + "&ErrorUrl=" + Request.Url.ToString().Replace("<br>", "").Replace("\n", ""));
        }

        Response.Redirect("~/TTMailDesktop.aspx");
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();


        string strUserCode = LB_UserCode.Text.Trim();
        string strEmailAddress;

        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            DataGrid1.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        strEmailAddress = e.Item.Cells[1].Text.Trim();

        if (To.Text.Trim() == "")
        {
            To.Text = strEmailAddress;
        }
        else
        {
            To.Text = To.Text.Trim() + "," + strEmailAddress;
        }
    }

    protected void BT_HazyFind_Click(object sender, EventArgs e)
    {
        string strName, strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        strName = TB_HazyName.Text.Trim();
        ContactInforBLL contactInforBLL = new ContactInforBLL();


        strName = "%" + strName + "%";

        strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'" + " and contactInfor.FirstName like " + "'" + strName + "'" + " order by contactInfor.ID DESC";


        lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_FindCompany_Click(object sender, EventArgs e)
    {
        string strName, strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        strName = TB_HazyCompany.Text.Trim();
        ContactInforBLL contactInforBLL = new ContactInforBLL();


        strName = "%" + strName + "%";
        strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'" + " and contactInfor.Company like " + "'" + strName + "'" + " order by contactInfor.ID DESC";

        lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_ContactType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType, strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        strType = DL_ContactType.SelectedValue.Trim();


        strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode= " + "'" + strUserCode + "'" + " and contactInfor.Type = " + "'" + strType + "'" + " order by contactInfor.ID DESC";


        ContactInforBLL contactInforBLL = new ContactInforBLL();
        lst = contactInforBLL.GetAllContactInfors(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ContactInforBLL contactInforBLL = new ContactInforBLL();
        IList lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void LoadContactList(string strUserCode)
    {
        string strHQL;
        IList lst;


        strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'" + " order by contactInfor.ID DESC";

        ContactInforBLL contactInforBLL = new ContactInforBLL();
        lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected string GetMailSignInfo(string strUserCode, string strStatus)
    {
        string strHQL;
        string strSignInfo;

        strHQL = "Select SignInfo From T_MailSignInfo Where UserCode = " + "'" + strUserCode + "'" + " And Status = 'InProgress' Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailSignInfo");

        if (ds.Tables[0].Rows.Count > 0)
        {
            strSignInfo = ds.Tables[0].Rows[0][0].ToString().Trim();

            strSignInfo = strSignInfo.Replace("[DateTime]", DateTime.Now.ToString());

            return strSignInfo;
        }
        else
        {
            return "";
        }
    }

}
