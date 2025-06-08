using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;

public partial class TTTakeTopIMSend : System.Web.UI.Page
{
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;
        string strCoID, strCreatorCode, strChatterCode;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        ////CKEditor初始化
        //CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        //_FileBrowser.BasePath = "ckfinder/"; Session["PageName"] = "TakeTopSiteContentEdit";
        //_FileBrowser.SetupFCKeditor(HTEditor1);

        strCoID = Request.QueryString["CoID"].Trim();
        strChatterCode = Request.QueryString["ChatterCode"].Trim();

        strUserCode = Session["UserCode"].ToString();

        if (Page.IsPostBack != true)
        {
            //if (strIsMobileDevice == "YES")
            //{
            HTEditor1.Visible = true;
            //}
            //else
            //{
            //    CKEditor1.Visible = true;
            //}

            if (strCoID != "0")
            {
                Collaboration collaboration = GetCollaboration(strCoID);
                strCreatorCode = collaboration.CreatorCode.Trim();

                if (strUserCode == strCreatorCode)
                {
                    BT_Active.Visible = true;
                    BT_Close.Visible = true;
                }

                LB_CreatorCode.Text = strCreatorCode;
                TB_CollaborationName.Text = collaboration.CollaborationName.Trim();

                BtnUP.Enabled = true;

                Panel_CoName.Visible = true;
                TB_CollaborationName.Enabled = false;
            }
            else
            {
                Panel_CoName.Visible = true;
                TB_CollaborationName.Text = LanguageHandle.GetWord("ZZLinShiXiaoXi");

                BtnUP.Enabled = false;
            }


            IList lst = ShareClass.GetDocTypeList(strUserCode);


            DL_DocType.DataSource = lst;
            DL_DocType.DataBind();

            TB_Author.Text = ShareClass.GetUserName(strUserCode);

            LB_UserCode.Text = strUserCode;
            LB_ChatterCode.Text = strChatterCode;

            LB_CoID.Text = strCoID;
            TB_CoID.Text = strCoID;

            //自动插入一空值，作为即时通讯窗口打开阅读之记录
            AddCollaborationLog(strCoID);
        }
    }

    protected void AddCollaborationLog(string strCoID)
    {
        string strUserCode = Session["UserCode"].ToString();

        CollaborationLogBLL collaborationLogBLL = new CollaborationLogBLL();
        CollaborationLog collaborationLog = new CollaborationLog();

        collaborationLog.CoID = int.Parse(strCoID);


        collaborationLog.LogContent = "";

        collaborationLog.UserCode = strUserCode;
        collaborationLog.UserName = ShareClass.GetUserName(strUserCode);
        collaborationLog.CreateTime = DateTime.Now;

        try
        {
            collaborationLogBLL.AddCollaborationLog(collaborationLog);
        }
        catch
        {

        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strUserCode, strChatterCode;
        string strCoID;
        string strCreatorCode;

        strUserCode = LB_UserCode.Text.Trim();
        strChatterCode = LB_ChatterCode.Text.Trim();

        strCoID = LB_CoID.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();

        if (strCoID == "0")
        {
            strCoID = AddCollaboration(strUserCode, strChatterCode);
            strCreatorCode = strUserCode;

            BtnUP.Enabled = true;
        }
        else
        {
            strCreatorCode = LB_CreatorCode.Text.Trim();
        }

        LB_CoID.Text = strCoID;
        TB_CoID.Text = strCoID;

        CollaborationLogBLL collaborationLogBLL = new CollaborationLogBLL();
        CollaborationLog collaborationLog = new CollaborationLog();

        collaborationLog.CoID = int.Parse(strCoID);

        collaborationLog.LogContent = HTEditor1.Text;


        //if (strIsMobileDevice == "YES")
        //{
        //    collaborationLog.LogContent = HTEditor1.Text;
        //}
        //else
        //{
        //    collaborationLog.LogContent = CKEditor1.Text.Trim();
        //}
        collaborationLog.UserCode = strUserCode;
        collaborationLog.UserName = ShareClass.GetUserName(strUserCode);
        collaborationLog.CreateTime = DateTime.Now;

        try
        {
            collaborationLogBLL.AddCollaborationLog(collaborationLog);

            if (strUserCode == strCreatorCode)
            {
                BT_Active.Visible = true;
                BT_Close.Visible = true;
            }

            HTEditor1.Text = "";

            //if (strIsMobileDevice == "YES")
            //{
            //    HTEditor1.Text = "";
            //}
            //else
            //{
            //    CKEditor1.Text = "";
            //}


            strChatterCode = GetLastestChartterCode(strCoID, strUserCode, strChatterCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTTakeTopIMDetailView.aspx?CoID=" + strCoID + "&ChatterCode=" + strChatterCode + "','imTopFrame');", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSSBJC") + "')", true);
        }
    }

    protected void BT_Active_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strCoID;

        strCoID = LB_CoID.Text.Trim();

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);
        Collaboration collaboration = (Collaboration)lst[0];

        collaboration.Status = "InProgress";

        try
        {
            collaborationBLL.UpdateCollaboration(collaboration, int.Parse(strCoID));

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJH") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHSBJC") + "')", true);
        }
    }

    protected void BT_Close_Click(object sender, EventArgs e)
    {
        string strCoID;
        string strHQL;
        IList lst;

        strCoID = LB_CoID.Text.Trim();

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];

        collaboration.Status = "Closed";

        try
        {
            collaborationBLL.UpdateCollaboration(collaboration, int.Parse(strCoID));

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYGB") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBSBJC") + "')", true);
        }
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strUserCode = LB_UserCode.Text.Trim();

            string strCoID = LB_CoID.Text.Trim();
            string strAuthor = TB_Author.Text.Trim();
            string strDocTypeID = DL_DocType.SelectedValue.Trim();
            if (strDocTypeID == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGWDLXBNWKJC") + "');</script>");
                return;
            }
            string strDocType = GetDocTypeName(strDocTypeID);
            string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            string strVisible = DL_Visible.SelectedValue.Trim();

            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀

            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "');</script>");
            }
            else
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.RelatedType = "Collaboration";  
                document.DocType = strDocType;
                document.DocTypeID = int.Parse(strDocTypeID);
                document.RelatedID = int.Parse(strCoID);
                document.Author = strAuthor;
                document.DocName = strFileName2;
                document.Address = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                document.UploadManCode = strUserCode;
                document.UploadManName = ShareClass.GetUserName(strUserCode);
                document.UploadTime = DateTime.Now;
                document.Visible = strVisible;
                document.DepartCode = strDepartCode; document.DepartName = ShareClass.GetDepartName(strDepartCode);
                document.Status = "InProgress";
                document.RelatedName = "";


                try
                {
                    documentBLL.AddDocument(document);

                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "');</script>");
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZYSCDWJ") + "');</script>");
        }
    }

    protected void UpdateCollaborationStatus(string strCoID)
    {
        string strHQL, strStatus;
        IList lst;

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];

        strStatus = collaboration.Status.Trim();

        if (strStatus == "New")
        {
            collaboration.Status = "InProgress";

            try
            {
                collaborationBLL.UpdateCollaboration(collaboration, int.Parse(strCoID));
            }
            catch
            {
            }
        }
    }

    protected string AddCollaboration(string strUserCode, string strChatterCode)
    {
        string strHQL;

        string strCOID;
        string strCollaborationName, strContent;

        strCollaborationName = TB_CollaborationName.Text.Trim();

        strContent = HTEditor1.Text.Trim();

        //if (strIsMobileDevice == "YES")
        //{
        //    strContent = HTEditor1.Text.Trim();
        //}
        //else
        //{
        //    strContent = CKEditor1.Text.Trim();
        //}

        strUserCode = LB_UserCode.Text.Trim();


        CollaborationBLL collaborationBLL = new CollaborationBLL();
        Collaboration collaboration = new Collaboration();

        collaboration.CollaborationName = strCollaborationName;
        collaboration.Comment = strContent;
        collaboration.CreateTime = DateTime.Now;
        collaboration.CreatorCode = strUserCode;
        collaboration.CreatorName = ShareClass.GetUserName(strUserCode);
        collaboration.RelatedType = "OTHER";
        collaboration.RelatedID = 0;
        collaboration.Status = "InProgress";

        try
        {
            collaborationBLL.AddCollaboration(collaboration);

            strCOID = ShareClass.GetMyCreatedMaxColloaborationID(strUserCode);

            strHQL = "insert T_CollaborationMember(CoID,UserCode,UserName,CreateTime,LastLoginTime) ";
            strHQL += " Values(" + strCOID + "," + "'" + strChatterCode + "'" + "," + "'" + ShareClass.GetUserName(strChatterCode) + "'" + ",now(),now())";
            ShareClass.RunSqlCommand(strHQL);

            return strCOID;
        }
        catch
        {
            return "0";
        }
    }

    protected string GetLastestChartterCode(string strCoID, string strUserCode, string strChatterCode)
    {
        string strHQL;

        strHQL = "Select UserCode From T_CollaborationLog Where CoID = " + strCoID;
        strHQL += " and  UserCode <> " + "'" + strUserCode + "'";
        strHQL += " Order By LogID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CollaborationLog");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return strChatterCode;
        }
    }

    protected Collaboration GetCollaboration(string strCoID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;

        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = new Collaboration();

        if (lst.Count > 0)
        {
            collaboration = (Collaboration)lst[0];
            return collaboration;
        }
        else
        {
            return null;
        }
    }

    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }
}
