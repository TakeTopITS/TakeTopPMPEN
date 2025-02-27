using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTViewMail : System.Web.UI.Page
{
    int nFolderID = -1;
    protected string AliasName = "AliasName";
    protected string Email = "Email";


    protected void Page_Load(object sender, EventArgs e)
    {
        string strDeleteOperate;
        string strUserCode = Session["UserCode"].ToString().Trim();

        ///��ȡ����nFolderID��ֵ
        if (Request.Params["FolderID"] != null)
        {
            if (Int32.TryParse(Request.Params["FolderID"].ToString(), out nFolderID) == false)
            {
                return;
            }
        }
        if (!Page.IsPostBack)
        {
            if (nFolderID > -1)
            {
                BindMailData(nFolderID);
                BindFolderData();

                LB_Folder.Text = GetFolderName(nFolderID.ToString());
            }

            strDeleteOperate = GetMailBoxAuthorityDelete(strUserCode);
            if (strDeleteOperate == "NO")
            {
                DeleteBtn.Enabled = false;
            }
        }
        DeleteBtn.Attributes.Add("onclick", "return confirm(" + LanguageHandle.GetWord("ZZNQDYSCSXYJM").ToString().Trim() + ");");
    }

    private void BindFolderData()
    {
        string strUserCode = Session["UserCode"].ToString();

        ///��ȡ����
        IFolder folder = new Folder();
        NpgsqlDataReader dr = folder.GetFolders(strUserCode);
        ///������
        FolderList.DataSource = dr;
        FolderList.DataTextField = "Name";
        FolderList.DataValueField = "FolderID";
        FolderList.DataBind();
        dr.Close();

        MoveBtn.Enabled = FolderList.Items.Count > 0 ? true : false;
    }
    private void BindMailData(int nFolderID)
    {	///��ȡ����
        IMail mail = new Mail();
        NpgsqlDataReader dr = mail.GetMailsByFloder(nFolderID);
        ///������
        MailView.DataSource = dr;
        MailView.DataBind();

        DeleteBtn.Enabled = MailView.Rows.Count > 0 ? true : false;

        string strUserCode = Session["UserCode"].ToString();
        string strDeleteOperate = GetMailBoxAuthorityDelete(strUserCode);
        if (strDeleteOperate == "NO")
        {
            DeleteBtn.Enabled = false;
        }

        dr.Close();
    }

    protected void CheckMail_CheckedChanged(object sender, EventArgs e)
    {
        int i = 0;
        if (CheckMail.Checked == true)
        {
            for (i = 0; i < MailView.Rows.Count; i++)
            {
                ((CheckBox)MailView.Rows[i].FindControl("CheckMail")).Checked = true;
            }
        }
        else
        {
            for (i = 0; i < MailView.Rows.Count; i++)
            {
                ((CheckBox)MailView.Rows[i].FindControl("CheckMail")).Checked = false;
            }
        }
    }

    protected void MoveBtn_Click(object sender, EventArgs e)
    {
        ///�������
        IMail mail = new Mail();
        try
        {
            foreach (GridViewRow row in MailView.Rows)
            {   ///��ȡ�ؼ�
                CheckBox checkMail = (CheckBox)row.FindControl("CheckMail");
                if (checkMail != null)
                {
                    if (checkMail.Checked == true)
                    {
                        ///ִ�����ݿ����
                        mail.MoveMail(Int32.Parse(MailView.DataKeys[row.RowIndex].Value.ToString()),
                            Int32.Parse(FolderList.SelectedValue));
                    }
                }
            }
            ///���°󶨿ؼ�������
            BindMailData(nFolderID);
        }
        catch (Exception ex)
        {   ///��ת���쳣������ҳ��
            Response.Redirect("TTTTErrorPage.aspx?ErrorMsg=" + ex.Message.Replace("<br>", "").Replace("\n", "")
                + "&ErrorUrl=" + Request.Url.ToString().Replace("<br>", "").Replace("\n", ""));
        }
    }
    protected void DeleteBtn_Click(object sender, EventArgs e)
    {
        ///�������
        IMail mail = new Mail();
        try
        {
            foreach (GridViewRow row in MailView.Rows)
            {   ///��ȡ�ؼ�
                CheckBox checkMail = (CheckBox)row.FindControl("CheckMail");
                if (checkMail != null)
                {
                    if (checkMail.Checked == true)
                    {
                        ///ִ�����ݿ����
                        mail.DeleteMail(Int32.Parse(MailView.DataKeys[row.RowIndex].Value.ToString()));
                    }
                }
            }
            ///���°󶨿ؼ�������
            BindMailData(nFolderID);
        }
        catch (Exception ex)
        {   ///��ת���쳣������ҳ��
            Response.Redirect("TTTTErrorPage.aspx?ErrorMsg=" + ex.Message.Replace("<br>", "").Replace("\n", "")
                + "&ErrorUrl=" + Request.Url.ToString().Replace("<br>", "").Replace("\n", ""));
        }
    }


    protected string GetFolderName(string strFolderID)
    {
        string strHQL;
        IList lst;

        strHQL = "FROM Folders as folders where folders.FolderID = " + strFolderID;
        FoldersBLL foldersBLL = new FoldersBLL();
        lst = foldersBLL.GetAllFolderss(strHQL);
        Folders folders = (Folders)lst[0];

        string strFolderName = folders.Name.Trim();

        string strFolderKeyWord = folders.KeyWord.Trim();
        if (strFolderKeyWord == "New")
        {
            strFolderName = LanguageHandle.GetWord("ZZNewMail").ToString().Trim();
        }
        if (strFolderKeyWord == "Read")
        {
            strFolderName = LanguageHandle.GetWord("ZZReadMail").ToString().Trim();
        }
        if (strFolderKeyWord == "Waiting")
        {
            strFolderName = LanguageHandle.GetWord("ZZWaitingMail").ToString().Trim();
        }
        if (strFolderKeyWord == "Send")
        {
            strFolderName = LanguageHandle.GetWord("ZZSendMail").ToString().Trim();
        }
        if (strFolderKeyWord == "Draft")
        {
            strFolderName = LanguageHandle.GetWord("ZZDraftBox").ToString().Trim();
        }
        if (strFolderKeyWord == "Rubbish")
        {
            strFolderName = LanguageHandle.GetWord("ZZRubbishBox").ToString().Trim();
        }

        return strFolderName;
    }

    protected string GetMailBoxAuthorityDelete(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From MailBoxAuthority as mailBoxAuthority where mailBoxAuthority.UserCode = " + "'" + strUserCode + "'";
        MailBoxAuthorityBLL mailBoxAuthorityBLL = new MailBoxAuthorityBLL();
        lst = mailBoxAuthorityBLL.GetAllMailBoxAuthoritys(strHQL);

        MailBoxAuthority mailBoxAuthority = (MailBoxAuthority)lst[0];

        return mailBoxAuthority.DeleteOperate.Trim();
    }
}
