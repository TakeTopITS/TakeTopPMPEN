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

using System.Text;
using System.IO;
using System.Web.Mail;
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProjectReviewWL : System.Web.UI.Page
{
    string strUserCode, strUserName, strProjectID, strProjectStatus, strReviewType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strProject;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);

        strProjectID = Request.QueryString["ProjectID"];
        strReviewType = Request.QueryString["Type"];
        strProjectStatus = GetProjectStatus(Request.QueryString["ProjectStatus"].Trim());

        if (strReviewType == "Status")
        {
            strReviewType = "Status";   //ChineseWord
            TB_WLKey.Text = "StatusReview";
        }

        if (strReviewType == "Plan")
        {
            strReviewType = "Plan";
            TB_WLKey.Text = "PlanReview";
        }

        if (strReviewType == "Project")
        {
            strReviewType = "Project";
            TB_WLKey.Text = "ProjectReview";
        }

        strProject = GetProjectName(strProjectID); 
        //this.Title =LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + strProject + "ÆÀÉó¹¤×÷Á÷";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            TB_WLName.Text = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID  + strProject  + strReviewType + LanguageHandle.GetWord("PingShen").ToString().Trim();

            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            Project project = new Project();
            lst = projectBLL.GetAllProjects(strHQL);
            DataList1.DataSource = lst;
            DataList1.DataBind();

            strReviewType = "%" + strReviewType + "%";

            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            strHQL = "from WorkFlowTemplate as workFlowTemplate where (workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Project' and relatedWorkFlowTemplate.RelatedID = " + strProjectID + ")";
            strHQL += " and workFlowTemplate.Type like " + "'" + strReviewType + "'" + ")";
            strHQL += " Or  (workFlowTemplate.Type like " + "'" + strReviewType + "'" + " and workFlowTemplate.Authority = 'All')";
            strHQL += " and workFlowTemplate.Visible = 'YES'";
            strHQL += " order by workFlowTemplate.SortNumber ASC";

            //LogClass.WriteLogFile(strHQL);

            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();

            LoadRelatedWL("ProjectReview","Project", int.Parse(strProjectID));

            HL_RelatedWorkFlowTemplate.Enabled = true;
            HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=Project&RelatedID=" + strProjectID;
            HL_WLTem.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=Project&RelatedID=" + strProjectID;
        }
    }

    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName,strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText;
        DateTime dtCreateTime;

        string strWLID;

        strWLID = "0";

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = TB_WLKey.Text.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = strUserCode;
        strCreatorName = GetUserName(strCreatorCode);
        dtCreateTime = DateTime.Now;

        if (strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBGZLMBBNWKJC").ToString().Trim()+"')", true);
            return "0";
        }

        strXMLFileName = strWLType + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        WorkFlow workFlow = new WorkFlow();

        workFlow.WLName = strWLName;
        workFlow.WLType = "ProjectReview";
        workFlow.XMLFile = strXMLFile2;
        workFlow.TemName = strTemName;
        workFlow.Description = strDescription;
        workFlow.CreatorCode = strCreatorCode;
        workFlow.CreatorName = strCreatorName;
        workFlow.CreateTime = dtCreateTime;
        workFlow.Status = "New";
        workFlow.RelatedType = "Project";
        workFlow.RelatedID = int.Parse(strProjectID);
        workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

        if (CB_SMS.Checked == true)
        {
            workFlow.ReceiveSMS = "YES";
        }
        else
        {
            workFlow.ReceiveSMS = "NO";
        }

        if (CB_Mail.Checked == true)
        {
            workFlow.ReceiveEMail = "YES";
        }
        else
        {
            workFlow.ReceiveEMail = "NO";
        }

        try
        {
            workFlowBLL.AddWorkFlow(workFlow);

            strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

            strCmdText = "select * from T_Project where ProjectID = " + strProjectID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_Project", strXMLFile2);
            LoadRelatedWL("ProjectReview","Project", int.Parse(strProjectID));

            if (strProjectStatus != "NONE")
            {
                InsertStatusRelatedWL(strProjectID, strProjectStatus);
            }

           ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMPSSGZLSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            strWLID = "0";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMPSSGZLSB").ToString().Trim()+"')", true);
        }

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }


    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        string strHQL, strKeyWord;
        IList lst;
     
        strKeyWord = TB_KeyWord.Text.Trim();
        strKeyWord = "%" + strKeyWord + "%";

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Project' and relatedWorkFlowTemplate.RelatedID = " + strProjectID + ")";
        strHQL += " and workFlowTemplate.TemName like " + "'" + strKeyWord + "'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
     
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void InsertStatusRelatedWL(string strProjectID, string strProjectStatus)
    {
        string strHQL;
        IList lst;
        int intWLID;

        strHQL = "from WorkFlow as workFlow where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];
        intWLID = workFlow.WLID;

        StatusRelatedWFBLL statusRelatedWFBLL = new StatusRelatedWFBLL();
        StatusRelatedWF statusRelatedWF = new StatusRelatedWF();

        statusRelatedWF.RelatedType = "Project";
        statusRelatedWF.RelatedID = int.Parse(strProjectID);
        statusRelatedWF.Status = strProjectStatus;
        statusRelatedWF.WLID = intWLID;
        statusRelatedWF.CreateTime = DateTime.Now;

        try
        {
            statusRelatedWFBLL.AddStatusRelatedWF(statusRelatedWF);
        }
        catch
        {
        }
    }

    protected void LoadRelatedWorkFlowTemplate(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + ")";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
     
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }


    protected string GetProjectStatus(string strIdentityString)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectStatus as projectStatus where projectStatus.IdentityString = " + "'" +strIdentityString+ "'";
        ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
        lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

        if (lst.Count > 0)
        {
            ProjectStatus projectStatus = (ProjectStatus)lst[0];
            return projectStatus.Status.Trim();
        }
        else
        {
            return "NONE";
        }       
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName.Trim();
        return strUserName.Trim();
    }

    protected string GetDepartCode(string strUserCode)
    {
        string strDepartCode, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strDepartCode = projectMember.DepartCode;
        return strDepartCode;
    }

    protected string GetProjectName(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

 
  
}
