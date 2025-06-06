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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTAllProjectDocuments : System.Web.UI.Page
{
    string strRelatedName, strProjectID;

    string strProductLineRelated;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        LB_UserCode.Text = strUserCode;


        strProductLineRelated = ShareClass.GetDepartRelatedProductLineFromUserCode(strUserCode);

        string strTargetID = Request.QueryString["TargetID"];
        strProjectID = Request.QueryString["ProjectID"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strTargetID != null)
            {
                LoadProjectDoc(strUserCode, strTargetID);
            }
            else
            {
                LoadAllDocument(strUserCode);
            }
        }
    }

    protected void LoadProjectDoc(string strUserCode, string strTarget)
    {
        string strRelatedType, strRelatedID;
        int intIndex;

        TreeNode node = new TreeNode();
        TreeNode parentNode = new TreeNode();

        LB_FindCondition1.Text = "";

        intIndex = strTarget.IndexOf("_");

        strRelatedType = strTarget.Substring(0, intIndex);
        strRelatedID = strTarget.Substring(intIndex + 1, strTarget.Length - intIndex - 1);

        if (strRelatedID != "0")
        {
            LoadProjectDoc(strUserCode, strRelatedType, strRelatedID);
        }
        else
        {
            if (strRelatedID == "0")
            {
                LoadProjectDocByType(strUserCode, strRelatedType, strRelatedID, strProjectID);
            }

            if (strTarget == "Project_0")
            {
                LoadAllDocument(strUserCode);
            }
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUploadManName = TB_UploadManName.Text.Trim();
        string strDocName = TB_DocName.Text.Trim();
        string strDocType = DL_DocType.SelectedValue.Trim();

        strUploadManName = "%" + strUploadManName + "%";
        strDocName = "%" + strDocName + "%";
        strDocType = "%" + strDocType + "%";

        strHQL = LB_Sql.Text.Trim();

        strHQL += " and document.UploadManName like " + "'" + strUploadManName + "'";
        strHQL += " and document.DocName like " + "'" + strDocName + "'";
        strHQL += " and document.DocType Like " + "'" + strDocType + "'";
        strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";

        try
        {
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_FindCondition2.Text = LanguageHandle.GetWord("WenJianShangChuanZheBaoHan") + strUploadManName;
            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
        }
        catch
        {
        }
    }

    protected void DL_DocType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDocType;

        strDocType = DL_DocType.SelectedValue.Trim();

        strHQL = LB_Sql.Text.Trim();

        strHQL += " and document.DocType = " + "'" + strDocType + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_FindCondition2.Text = LanguageHandle.GetWord("WenJianLeiXing") + strDocType;
        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
    }

    protected void LoadProjectDoc(string strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL = "";
        IList lst;

        string strDepartCode, strDepartString;
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";
            strRelatedName = GetProjectName(strRelatedID);
        }

        if (strRelatedType == "ProjectPlan")
        {
            strRelatedType = "Plan";
            strRelatedName = GetProjectPlanName(strRelatedID);
        }

        if (strRelatedType == "ProjectTask")
        {
            strRelatedType = "Task";
            strRelatedName = GetProjectTaskName(strRelatedID);
        }

        if (strRelatedType == "Risk")
        {
            strRelatedType = "Risk";
            strRelatedName = GetRiskName(strRelatedID);
        }

        if (strRelatedType == "Req")
        {
            strRelatedType = "Requirement";
            strRelatedName = GetReqName(strRelatedID);
        }

        if (strRelatedType == "WorkFlow")
        {
            strRelatedType = "Workflow";
            strRelatedName = ShareClass.GetWorkFlowName(strRelatedID);
        }

        if (strRelatedType == "Collaboration")
        {
            strRelatedType = "Collaboration";  
            strRelatedName = GetCollaborationName(strRelatedID);
        }

        if (strRelatedType == "Meeting")
        {
            strRelatedType = "Meeting";  
            strRelatedName = GetMeetingName(strRelatedID);
        }

        if (strRelatedType == "CustomerQuestion")
        {
            strRelatedType = "CustomerQuestion";  
            strRelatedName = GetCustomerQuestionName(strRelatedID);
        }

        if (strRelatedType == "Project")
        {
            strHQL = "from Document as document where ((document.RelatedType = 'Project' and document.RelatedID = " + strRelatedID + ")";
            strHQL += " or (document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strRelatedID + "))";  
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strRelatedID + ")))";  
        }

        if (strRelatedType == "Plan")
        {
            strHQL = "from Document as document where ";
            strHQL += " document.RelatedType = 'Plan' and document.RelatedID = " + strRelatedID;
        }

        if (strRelatedType == "Task")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Task' and document.RelatedID = " + strRelatedID + " )";
            strHQL += " or ( document.RelatedType = 'Plan' and document.RelatedID in ( Select projectTask.PlanID from ProjectTask as projectTask where projectTask.TaskID = " + strRelatedID + ")))";
        }

        if (strRelatedType == "Risk")
        {
            strHQL = "from Document as document where ";
            strHQL += "((document.RelatedType = 'Risk' and document.RelatedID =" + strRelatedID + ")";  
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Risk' and meeting.RelatedID =" + strRelatedID + ")))";  
        }

        if (strRelatedType == "Requirement")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Requirement' and document.RelatedID = " + strRelatedID + ")";
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Requirement' and meeting.RelatedID =" + strRelatedID + ")))";  
        }

        if (strRelatedType == "Workflow")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Workflow' and document.RelatedID = " + strRelatedID + ")"; ;
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Workflow' and meeting.RelatedID =" + strRelatedID + ")))";  
        }

        if (strRelatedType == "Collaboration")  
        {
            strHQL = " from Document as document where document.RelatedType = 'Collaboration' and document.RelatedID = " + strRelatedID;  
        }

        if (strRelatedType == "Meeting")  
        {
            strHQL = " from Document as document where document.RelatedType = 'Meeting' and document.RelatedID = " + strRelatedID;  
        }

        if (strRelatedType == "CustomerQuestion")  
        {
            strHQL = " from Document as document where document.RelatedType = 'CustomerQuestion' and document.RelatedID = " + strRelatedID;  
        }

        strHQL += " and document.DepartCode in " + strDepartString;

        LB_Sql.Text = strHQL;

        LoadDocTypeList(strHQL);

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();

        LB_FindCondition1.Text = LanguageHandle.GetWord("ChaXunFanWeiShi") + ": " + strRelatedType + ":" + strRelatedID + " " + strRelatedName + " ";
        LB_FindCondition2.Text = "";
    }

    protected void LoadProjectDocByType(string strUserCode, string strRelatedType, string strRelatedID, string strProjectID)
    {
        string strHQL = "";
        IList lst;

        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";
        }

        if (strRelatedType == "ProjectPlan")
        {
            strRelatedType = "Plan";
        }

        if (strRelatedType == "ProjectTask")
        {
            strRelatedType = "Task";
        }

        if (strRelatedType == "Risk")
        {
            strRelatedType = "Risk";
        }

        if (strRelatedType == "Req")
        {
            strRelatedType = "Requirement";
        }

        if (strRelatedType == "WorkFlow")
        {
            strRelatedType = "Workflow";
        }

        if (strRelatedType == "Collaboration")
        {
            strRelatedType = "Collaboration";  
        }

        if (strRelatedType == "Meeting")
        {
            strRelatedType = "Meeting";  
        }

        if (strRelatedType == "CustomerQuestion")
        {
            strRelatedType = "CustomerQuestion";  
        }

        if (strRelatedType == "Project")
        {
            strHQL = "from Document as document where ((document.RelatedType = 'Project' and document.RelatedID = " + strRelatedID + ")";
            strHQL += " or (document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strRelatedID + "))";  
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strRelatedID + ")))";  

        }

        if (strRelatedType == "Plan")
        {
            strHQL = "from Document as document where ";
            strHQL += " document.RelatedType = 'Plan' and document.RelatedID in (Select workPlan.ID from WorkPlan as workPlan Where workPlan.ProjectID = " + strProjectID + ")";
        }

        if (strRelatedType == "Task")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Task' and document.RelatedID  in (Select projectTask.TaskID From ProjectTask as projectTask Where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or ( document.RelatedType = 'Plan' and document.RelatedID in ( Select projectTask.PlanID from ProjectTask as projectTask where projectTask.TaskID = " + strRelatedID + ")))";
        }

        if (strRelatedType == "Risk")
        {
            strHQL = "from Document as document where ";
            strHQL += "((document.RelatedType = 'Risk' and document.RelatedID  in (Select projectRisk.ID From ProjectRisk as projectRisk Where projectRisk.ProjectID =" + strProjectID + "))";  
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Risk' and meeting.RelatedID =" + strRelatedID + ")))";  
        }

        if (strRelatedType == "Requirement")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Requirement' and document.RelatedID  in (Select relatedReq.ReqID From RelatedReq as relatedReq Where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Requirement' and meeting.RelatedID =" + strRelatedID + ")))";  
        }

        if (strRelatedType == "Workflow")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID from WorkFlow as workFlow where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Workflow' and meeting.RelatedID =" + strRelatedID + ")))";  
        }

        if (strRelatedType == "Meeting")  
        {
            strHQL = "from Document as document where document.RelatedType = 'Meeting' and document.RelatedID  in (Select meeting.ID from Meeting as meeting where meeting.RelatedType = 'Project' and meeting.RelatedID =" + strProjectID + ")";  
        }

        if (strRelatedType == "Collaboration")  
        {
            strHQL = " from Document as document where document.RelatedType = 'Collaboration' and document.RelatedID = " + strRelatedID;  
        }

        if (strRelatedType == "CustomerQuestion")  
        {
            strHQL = " from Document as document where document.RelatedType = 'CustomerQuestion' and document.RelatedID = " + strRelatedID;  
        }

        strHQL += " and document.DepartCode in " + strDepartString;

        LB_Sql.Text = strHQL;

        LoadDocTypeList(strHQL);

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();

        LB_FindCondition1.Text = LanguageHandle.GetWord("ChaXunFanWeiShi") + ": " + strRelatedType + ":" + strRelatedID + " " + strRelatedName + " ";
        LB_FindCondition2.Text = "";
    }

    protected void LoadAllDocument(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        strHQL = "from Document as document where document.Status <> 'Deleted'";
        strHQL += " and document.DepartCode in " + strDepartString;

        LB_Sql.Text = strHQL;

        LoadDocTypeList(strHQL);

        strHQL += "  Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();

        LB_FindCondition1.Text = LanguageHandle.GetWord("CXFWXMSY");
        LB_FindCondition2.Text = "";
    }

    protected void LoadDocTypeList(string strHQL)
    {
        string strHQL1;
        IList lst;

        strHQL1 = "from DocType as docType where docType.Type in ( Select document.DocType  " + strHQL + ")" + " Order by docType.SortNumber ASC";
        DocTypeBLL docTypeBLL = new DocTypeBLL();
        lst = docTypeBLL.GetAllDocTypes(strHQL1);

        DL_DocType.DataSource = lst;
        DL_DocType.DataBind();

        DL_DocType.Items.Add(new ListItem("", ""));
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DocumentBLL documentBLL = new DocumentBLL();
        IList lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected string GetDocTypeCreator(string strDocTypeID)
    {
        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.UserCode.Trim();
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

    protected string GetProjectPlanName(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkPlan as workPlan where workPlan.ID = " + strPlanID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        WorkPlan workPlan = new WorkPlan();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);
        workPlan = (WorkPlan)lst[0];

        return workPlan.Name.Trim();
    }

    protected string GetProjectTaskName(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        ProjectTask projectTask = new ProjectTask();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        projectTask = (ProjectTask)lst[0];

        return projectTask.Task.Trim();
    }

    protected string GetRiskName(string strRiskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);
        ProjectRisk projectRisk = (ProjectRisk)lst[0];

        return projectRisk.Risk.Trim();
    }

    protected string GetReqName(string strReqID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();
        lst = requirementBLL.GetAllRequirements(strHQL);
        Requirement requirement = (Requirement)lst[0];

        return requirement.ReqName.Trim();
    }


    protected string GetCollaborationName(string strCoID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];

        return collaboration.CollaborationName.Trim();
    }

    protected string GetMeetingName(string strMeetingID)
    {
        string strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);
        Meeting meeting = (Meeting)lst[0];

        string strMeetingName = meeting.Name.Trim();

        return strMeetingName;
    }

    protected string GetCustomerQuestionName(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        return customerQuestion.Question.Trim();
    }

}
