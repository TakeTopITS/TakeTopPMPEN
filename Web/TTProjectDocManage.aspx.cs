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

public partial class TTProjectDocManage : System.Web.UI.Page
{
    string strRelatedName, strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = GetDepartCode(strUserCode);
        LB_UserCode.Text = strUserCode;

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
                LoadAllProjectDocument(strUserCode);
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
                LoadAllProjectDocument(strUserCode);
            }
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDocType = DL_DocType.SelectedValue.Trim();
        strDocType = "%" + strDocType + "%";

        string strUploadManName = TB_UploadManName.Text.Trim();
        strUploadManName = "%" + strUploadManName + "%";

        string strDocName = TB_DocName.Text.Trim();
        strDocName = "%" + strDocName + "%";

        strHQL = LB_Sql.Text.Trim();

        strHQL += " and document.DocType Like " + "'" + strDocType + "'";
        strHQL += " and document.DocName Like " + "'" + strDocName + "'";
        strHQL += " and document.UploadManName like " + "'" + strUploadManName + "'";
        strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_FindCondition2.Text = LanguageHandle.GetWord("WenJianShangChuanZheBaoHan").ToString().Trim() + strUploadManName;
        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();
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

        LB_FindCondition2.Text = LanguageHandle.GetWord("WenJianLeiXing").ToString().Trim() + strDocType;
        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();
    }

    protected void LoadProjectDoc(string strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL = "";
        IList lst;

        string strDepartCode;

        strDepartCode = GetDepartCode(strUserCode);

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

        if (strRelatedType == "Defect")
        {
            strRelatedType = "Defect";
            strRelatedName = GetDefectName(strRelatedID);
        }

        if (strRelatedType == "WorkFlow")
        {
            strRelatedType = "Workflow";
            strRelatedName = ShareClass.GetWorkFlowName(strRelatedID);
        }

        if (strRelatedType == "Collaboration")
        {
            strRelatedType = "协作"; 
            strRelatedName = GetCollaborationName(strRelatedID);
        }

        if (strRelatedType == "Meeting")
        {
            strRelatedType = "会议"; 
            strRelatedName = GetMeetingName(strRelatedID);
        }

        if (strRelatedType == "CustomerQuestion")
        {
            strRelatedType = "客服"; 
            strRelatedName = GetCustomerQuestionName(strRelatedID);
        }

        if (strRelatedType == "Project")
        {
            strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strRelatedID + ")";

            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '部门','全体')))"; 
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = '风险' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strRelatedID + "))"; 
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.Visible in ('会议','部门') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) "; 
            strHQL += " or (document.Visible = '全体' )))))"; 
        }

        if (strRelatedType == "Plan")
        {
            strHQL = "from Document as document where ";
            strHQL += " (document.RelatedType = 'Plan' and document.RelatedID = " + strRelatedID;
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '部门','全体'))))"; 
        }

        if (strRelatedType == "Task")
        {
            strHQL = "from Document as document where ";
            strHQL += " (((document.RelatedType = 'Task' and document.RelatedID = " + strRelatedID + " )";
            strHQL += " or ( document.RelatedType = 'Plan' and document.RelatedID in ( Select projectTask.PlanID from ProjectTask as projectTask where projectTask.TaskID = " + strRelatedID + ")))";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '部门','全体'))))"; 
        }

        if (strRelatedType == "Risk")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = '风险' and document.RelatedID = " + strRelatedID; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='风险' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议'))))"; 
        }

        if (strRelatedType == "Requirement")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Requirement' and document.RelatedID = " + strRelatedID;
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Requirement' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议'))))"; 
        }


        if (strRelatedType == "Defect")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Defect' and document.RelatedID = " + strRelatedID;
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Defect' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议'))))"; 
        }

        if (strRelatedType == "Workflow")
        {
            strHQL = "from Document as document where document.Status <> 'Deleted' ";
            strHQL += " and (document.RelatedType = 'Workflow' and document.RelatedID = " + strRelatedID;
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Workflow' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议')))"; 
        }

        if (strRelatedType == "协作") 
        {
            strHQL = " from Document as document where document.RelatedType = '协作' and document.RelatedID = " + strRelatedID; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '全体' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " )))"; 
        }

        if (strRelatedType == "会议") 
        {
            strHQL = " from Document as document where document.RelatedType = '会议' and document.RelatedID = " + strRelatedID; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '会议' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " )))"; 
        }

        if (strRelatedType == "客服") 
        {
            strHQL = " from Document as document where document.RelatedType = '客服' and document.RelatedID = " + strRelatedID; 
            strHQL += " and (document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";

        }

        LB_Sql.Text = strHQL;

        LoadDocTypeList(strHQL);

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

        LB_FindCondition1.Text = LanguageHandle.GetWord("ChaXunFanWeiShi").ToString().Trim() + ": " + strRelatedType + ":" + strRelatedID + " " + strRelatedName + " ";
        LB_FindCondition2.Text = "";
    }


    protected void LoadProjectDocByType(string strUserCode, string strRelatedType, string strRelatedID, string strProjectID)
    {
        string strHQL = "";
        IList lst;

        string strDepartCode;


        strDepartCode = GetDepartCode(strUserCode);


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

        if (strRelatedType == "Defect")
        {
            strRelatedType = "Defect";

        }

        if (strRelatedType == "WorkFlow")
        {
            strRelatedType = "Workflow";

        }

        if (strRelatedType == "Collaboration")
        {
            strRelatedType = "协作"; 

        }

        if (strRelatedType == "Meeting")
        {
            strRelatedType = "会议"; 

        }

        if (strRelatedType == "CustomerQuestion")
        {
            strRelatedType = "客服"; 
        }

        if (strRelatedType == "Project")
        {
            strHQL = "  from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strRelatedID + ")";

            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '部门','全体')))"; 
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = '风险' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strRelatedID + "))"; 
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.Visible in ('会议','部门') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) "; 
            strHQL += " or (document.Visible = '全体' )))))"; 
        }

        if (strRelatedType == "Plan")
        {
            strHQL = "from Document as document where ";
            strHQL += " (document.RelatedType = 'Plan' and document.RelatedID in (Select workPlan.ID from WorkPlan as workPlan Where workPlan.ProjectID = " + strProjectID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '部门','全体'))))"; 
        }

        if (strRelatedType == "Task")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Task' and document.RelatedID in (Select projectTask.TaskID From ProjectTask as projectTask Where projectTask.ProjectID = " + strProjectID + ")";
            strHQL += " or ( document.RelatedType = 'Plan' and document.RelatedID in ( Select projectTask.PlanID from ProjectTask as projectTask where projectTask.TaskID = " + strRelatedID + ")))";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( '部门','全体'))))"; 
        }

        if (strRelatedType == "Risk")
        {
            strHQL = "from Document as document where ";
            strHQL += "((document.RelatedType = '风险' and document.RelatedID in (Select projectRisk.ID From ProjectRisk as projectRisk Where projectRisk.ProjectID =" + strProjectID + ")"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='风险' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议'))))"; 
        }

        if (strRelatedType == "Requirement")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Requirement' and document.RelatedID in (Select relatedReq.ReqID From RelatedReq as relatedReq Where relatedReq.ProjectID = " + strProjectID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Requirement' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议'))))"; 
        }

        if (strRelatedType == "Defect")
        {
            strHQL = "from Document as document where ";
            strHQL += " ((document.RelatedType = 'Defect' and document.RelatedID in (Select relatedReq.ReqID From RelatedReq as relatedReq Where relatedReq.ProjectID = " + strProjectID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Defect' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议'))))"; 
        }

        if (strRelatedType == "Workflow")
        {
            strHQL = "from Document as document where document.Status <> 'Deleted' ";
            strHQL += " and (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID from WorkFlow as workFlow where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '部门' and document.DepartCode = " + "'" + strDepartCode + "'" + " )"; 
            strHQL += " or ( document.Visible = '全体'))) "; 
            strHQL += "or ((document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Workflow' and meeting.RelatedID =" + strRelatedID + "))"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = '会议')))"; 
        }

        if (strRelatedType == "会议") 
        {
            strHQL = "from Document as document where document.RelatedType = '会议' and document.RelatedID in (Select meeting.ID from Meeting as meeting where meeting.RelatedType = 'Project' and meeting.RelatedID =" + strProjectID + ")"; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '会议' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " )))"; 
        }

        if (strRelatedType == "协作") 
        {
            strHQL = " from Document as document where document.RelatedType = '协作' and document.RelatedID = " + strRelatedID; 
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = '全体' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " )))"; 
        }

        if (strRelatedType == "客服") 
        {
            strHQL = " from Document as document where document.RelatedType = '客服' and document.RelatedID = " + strRelatedID; 
            strHQL += " and (document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";

        }

        LB_Sql.Text = strHQL;

        LoadDocTypeList(strHQL);

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

        LB_FindCondition1.Text = LanguageHandle.GetWord("CXFWXM").ToString().Trim() + ": " + strProjectID + " " + strRelatedType;
        LB_FindCondition2.Text = "";
    }


    protected void LoadAllProjectDocument(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartCode;

        strDepartCode = GetDepartCode(strUserCode);

        strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID in (select relatedUser.ProjectID from RelatedUser as relatedUser Where relatedUser.UserCode =  " + "'" + strUserCode + "'" + "))";
        strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or (document.Visible in ( '部门','全体'))))"; 

        strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID  in (select relatedUser.ProjectID from RelatedUser as relatedUser Where relatedUser.UserCode =  " + "'" + strUserCode + "'" + ")))";
        strHQL += " or (document.RelatedType = '风险' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID in (select relatedUser.ProjectID from RelatedUser as relatedUser Where relatedUser.UserCode =  " + "'" + strUserCode + "'" + ")))"; 
        strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID  in (select relatedUser.ProjectID from RelatedUser as relatedUser Where relatedUser.UserCode =  " + "'" + strUserCode + "'" + ")))";
        strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID  in (select relatedUser.ProjectID from RelatedUser as relatedUser Where relatedUser.UserCode =  " + "'" + strUserCode + "'" + ")))";
        strHQL += " or (document.RelatedType = '会议' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID  in (select relatedUser.ProjectID from RelatedUser as relatedUser Where relatedUser.UserCode =  " + "'" + strUserCode + "'" + ")))"; 
        strHQL += " and ((document.Visible in ('会议','部门') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) "; 
        strHQL += " or (document.Visible = '全体' )))))"; 

        LB_Sql.Text = strHQL;

        LoadDocTypeList(strHQL);

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();



        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

        LB_FindCondition1.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();
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

        DL_DocType.Items.Insert(0, new ListItem("--Select--", ""));
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

    protected string GetDefectName(string strDefectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Defectment as defectment where defectment.DefectID = " + strDefectID;
        DefectmentBLL defectmentBLL = new DefectmentBLL();
        lst = defectmentBLL.GetAllDefectments(strHQL);
        Defectment defectment = (Defectment)lst[0];

        return defectment.DefectName.Trim();
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

    protected string GetDepartCode(string strUserCode)
    {
        string strDepartCode, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strDepartCode = projectMember.DepartCode.Trim();
        return strDepartCode;
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


}
