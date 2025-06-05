using System; using System.Resources;
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
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTCollaborationDetailReadOnly : System.Web.UI.Page
{
    string strUserCode;
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strCoID, strStatus;
        string strCreatorCode;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(CKEditor1);
CKEditor1.Language = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strCoID = Request.QueryString["CoID"].Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HTEditor1.Visible = true;
            }
            else
            {
                CKEditor1.Visible = true;
            }


            strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
            CollaborationBLL collaborationBLL = new CollaborationBLL();
            lst = collaborationBLL.GetAllCollaborations(strHQL);
            Collaboration collaboration = (Collaboration)lst[0];

            DataList1.DataSource = lst;
            DataList1.DataBind();

            strCreatorCode = collaboration.CreatorCode.Trim();
            strStatus = collaboration.Status.Trim();

            LB_CollaborationName.Text = strCoID + " " + collaboration.CollaborationName.Trim();

            LB_Creator.Text = collaboration.CreatorCode.Trim() + " " + collaboration.CreatorName.Trim();
            LB_CreateTime.Text = collaboration.CreateTime.ToString();
            LB_Status.Text = collaboration.Status.Trim();

            if (strUserCode == strCreatorCode)
            {
                BT_Close.Visible = true;
                BT_Active.Visible = true;
            }

            if (strStatus != "Closed")
            {
                HL_CollaborationToTask.Enabled = true;
                HL_CollaborationToTask.NavigateUrl = "TTCollaborationToTask.aspx?ProjectID=1&RelatedType=Collaboration&CollaborationID=" + strCoID;
            }

            UpdateLastLoginTime(strUserCode, strCoID);

            LoadCollaborationMember(strCoID);

            LoadRelatedDoc(strCoID);

            lst = ShareClass.GetDocTypeList(strUserCode);
            DL_DocType.DataSource = lst;
            DL_DocType.DataBind();

            TB_Author.Text = ShareClass.GetUserName(strUserCode);


            LB_CoID.Text = strCoID;
        }
    }

    protected void BT_AddLog_Click(object sender, EventArgs e)
    {
        string strCoID;
        String strLog;

        strCoID = LB_CoID.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strLog = HTEditor1.Text.Trim();
        }
        else
        {
            strLog = CKEditor1.Text.Trim();
        }

        CollaborationLogBLL collaborationLogBLL = new CollaborationLogBLL();
        CollaborationLog collaborationLog = new CollaborationLog();

        collaborationLog.CoID = int.Parse(strCoID);

        collaborationLog.LogContent = strLog;
        collaborationLog.UserCode = strUserCode;
        collaborationLog.UserName = ShareClass.GetUserName(strUserCode);
        collaborationLog.CreateTime = DateTime.Now;

        try
        {
            collaborationLogBLL.AddCollaborationLog(collaborationLog);

            CKEditor1.Text = "";

            UpdateCollaborationStatus(strCoID);

            LoadRelatedDoc(strCoID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.parent.frames['rightFrame'].location.reload()", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFSSBJC")+"')", true);
        }
    }

    protected void BT_Close_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strCoID;

        strCoID = LB_CoID.Text.Trim();

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];

        collaboration.Status = "Closed";

        try
        {
            collaborationBLL.UpdateCollaboration(collaboration, int.Parse(strCoID));
            LB_Status.Text = "Closed";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGBCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGBSBJC")+"')", true);
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

            LB_Status.Text = "InProgress";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHSBJC")+"')", true);
        }
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
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
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC")+"');</script>");
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

                    LoadRelatedDoc(strCoID);

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZSCSBJC")+"');</script>");
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZZYSCDWJ")+"');</script>");
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

    protected void LoadCollaborationMember(string strCoID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CollaborationMember as collaborationMember where collaborationMember.CoID = " + strCoID;
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        lst = collaborationMemberBLL.GetAllCollaborationMembers(strHQL);

        RP_Attendant.DataSource = lst;
        RP_Attendant.DataBind();
    }

    protected void LoadRelatedDoc(string strCoID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.RelatedType = 'Collaboration' and document.RelatedID = " + strCoID;  
        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void UpdateLastLoginTime(string strUserCode, string strCoID)
    {
        string strHQL;
        IList lst;
        int intMemID;

        strHQL = "from CollaborationMember as collaborationMember where collaborationMember.CoID = " + strCoID + " and collaborationMember.UserCode = " + "'" + strUserCode + "'";
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        lst = collaborationMemberBLL.GetAllCollaborationMembers(strHQL);

        CollaborationMember collaborationMember = (CollaborationMember)lst[0];
        collaborationMember.LastLoginTime = DateTime.Now;
        intMemID = collaborationMember.MemID;

        try
        {
            collaborationMemberBLL.UpdateCollaborationMember(collaborationMember, intMemID);
        }
        catch
        {
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
}
