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
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAppCollaborationDetail : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strCoID, strStatus;
        string strCreatorCode;

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor≥ı ºªØ
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/"; Session["PageName"] = "TakeTopSiteContentEdit";
        _FileBrowser.SetupCKEditor(CKEditor1);
CKEditor1.Language = Session["LangCode"].ToString();


        CKEditor1.Language = Session["LangCode"].ToString();

        strCoID = Request.QueryString["CoID"].Trim();
   

        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HTEditor1.Visible = true;
                HTEditor1.Toolbar = "";
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

            //if (strUserCode == strCreatorCode)
            //{
            //    BT_Close.Visible = true;
            //    BT_Active.Visible = true;
            //}

            if (strStatus != "Closed")
            {
                HL_CollaborationToTask.Enabled = true;
                HL_CollaborationToTask.NavigateUrl = "TTCollaborationToTask.aspx?ProjectID=1&RelatedType=Collaboration&CollaborationID=" + strCoID;
            }

            UpdateLastLoginTime(strUserCode, strCoID);

            LoadCollaborationMember(strCoID);

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



            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.parent.frames['rightFrame'].location.reload()", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSSBJC") + "')", true);
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

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBSBJC") + "')", true);
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

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHSBJC") + "')", true);
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
