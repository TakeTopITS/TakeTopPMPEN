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

public partial class TTDocumentTreeView : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strRelatedName, strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL = "";
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        LB_UserCode.Text = strUserCode;

        strRelatedType = Request.QueryString["RelatedType"].Trim();
        strRelatedID = Request.QueryString["RelatedID"];
        strProjectID = strRelatedID;

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";
            strRelatedName = ShareClass.GetProjectName(strRelatedID);

            //列出模板要求完成没见完成的文档
            LoadNotFinishedProjectDocument(ShareClass.GetProject(strProjectID).ProjectType.Trim());
        }

        if (strRelatedType == "ProjectPlan")
        {
            strRelatedType = "Plan";
            strRelatedName = GetProjectPlanName(strRelatedID);

            if (ShareClass.CheckUserCanViewProjectPlan(strRelatedID, strUserCode) == false)
            {
                Response.Redirect("TTDisplayCustomErrorMessage.aspx?ErrorMsg='" + LanguageHandle.GetWord("ZZJGZYXMJLJHYJHCJRHLXZJHFZRCNJXZCZQJC").ToString().Trim() + "'");
            }
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

        if (strRelatedType == "LargePlan")
        {
            strRelatedType = "MasterPlan";   //ChineseWord
            strRelatedName = GetPlanName(strRelatedID);
        }

        if (strRelatedType == "BOM")
        {
            strRelatedType = "Material";   //ChineseWord
            string strRelatedItemCode = Request.QueryString["RelatedItemCode"].Trim();
            strRelatedID = GetItemBomVersionBomID(strRelatedItemCode, strRelatedID);
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack != true)
        {
            if (strRelatedType == "Project")
            {
                string strPMCode = ShareClass.GetProject(strProjectID).PMCode.Trim();
                string strCreatorCode = ShareClass.GetProject(strProjectID).UserCode.Trim();

                if (strUserCode == strPMCode | strUserCode == strCreatorCode)
                {
                    strHQL = string.Format(@"from Document as document where ((document.RelatedType = 'Project' and document.RelatedID = {0} ))
                   or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = {0}))
                   or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = {0}))
                   or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID = {0}))
                   or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = {0}))
                   or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = {0}))
                   or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = {0})))
                   or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID = {0}))
                   ))
                   and rtrim(ltrim(document.Status)) <> 'Deleted'", strProjectID, strUserCode, strDepartCode);  
                }
                else
                {
                    strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
                    strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                    strHQL += " or (document.Visible in ( 'Department','Entire')))";   //ChineseWord
                    strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

                    strHQL += " or (document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";

                    strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";
                    strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlowBackup.WLID From WorkFlowBackup as workFlowBackup Where workFlowBackup.RelatedType = 'Project' and workFlowBackup.RelatedID = " + strProjectID + "))";

                    strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  
                    strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";

                    strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
                    strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";
                    strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlowBackup.WLID From WorkFlowBackup as workFlowBackup Where workFlowBackup.RelatedType = 'Plan' and workFlowBackup.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

                    strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))";  
                    strHQL += " and ((document.Visible in ('Meeting','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
                    strHQL += " or (document.Visible = 'Entire' )))";   //ChineseWord
                }
            }

            if (strRelatedType == "Plan")
            {
                strHQL = "from Document as document where ";
                strHQL += " (document.RelatedType = 'Plan' and document.RelatedID = " + strRelatedID;
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))";
                strHQL += " or (document.Visible in ( 'Department','Entire'))))";   //ChineseWord
            }

            if (strRelatedType == "Task")
            {
                strHQL = "from Document as document where ";
                strHQL += " (((document.RelatedType = 'Task' and document.RelatedID = " + strRelatedID + " )";
                strHQL += " or ( document.RelatedType = 'Plan' and document.RelatedID in ( Select projectTask.PlanID from ProjectTask as projectTask where projectTask.TaskID = " + strRelatedID + ")))";
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))";
                strHQL += " or (document.Visible in ( 'Department','Entire'))))";   //ChineseWord
            }

            if (strRelatedType == "Risk")
            {
                strHQL = "from Document as document where ";
                strHQL += " ((document.RelatedType = 'Risk' and document.RelatedID = " + strRelatedID;  
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + " )";  
                strHQL += " or ( document.Visible = 'Entire'))) ";   //ChineseWord
                strHQL += "or ((document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Risk' and meeting.RelatedID =" + strRelatedID + "))";  
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or ( document.Visible = 'Meeting'))))";  
            }

            if (strRelatedType == "Requirement")
            {
                strHQL = "from Document as document where ";
                strHQL += " ((document.RelatedType = 'Requirement' and document.RelatedID = " + strRelatedID;
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + " )";  
                strHQL += " or ( document.Visible = 'Entire'))) ";   //ChineseWord
                strHQL += "or ((document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Requirement' and meeting.RelatedID =" + strRelatedID + "))";  
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or ( document.Visible = 'Meeting'))))";  
            }

            if (strRelatedType == "Defect")
            {
                strHQL = "from Document as document where ";
                strHQL += " ((document.RelatedType = 'Defect' and document.RelatedID = " + strRelatedID;
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + " )";  
                strHQL += " or ( document.Visible = 'Entire'))) ";   //ChineseWord
                strHQL += "or ((document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Requirement' and meeting.RelatedID =" + strRelatedID + "))";  
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or ( document.Visible = 'Meeting'))))";  
            }

            if (strRelatedType == "Workflow")
            {
                strHQL = "from Document as document where document.Status <> 'Deleted' ";
                strHQL += " and (document.RelatedType = 'Workflow' and document.RelatedID = " + strRelatedID;
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + " )";  
                strHQL += " or ( document.Visible = 'Entire'))) ";   //ChineseWord
                strHQL += "or ((document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Workflow' and meeting.RelatedID =" + strRelatedID + "))";  
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or ( document.Visible = 'Meeting')))";  
            }

            if (strRelatedType == "Collaboration")  
            {
                strHQL = " from Document as document where document.RelatedType = 'Collaboration' and document.RelatedID = " + strRelatedID;  
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " )))";   //ChineseWord
            }

            if (strRelatedType == "Meeting")  
            {
                strHQL = " from Document as document where document.RelatedType = 'Meeting' and document.RelatedID = " + strRelatedID;  
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Meeting' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " )))";  
            }

            if (strRelatedType == "CustomerQuestion")  
            {
                strHQL = " from Document as document where document.RelatedType = 'CustomerQuestion' and document.RelatedID = " + strRelatedID;  
                strHQL += " and (document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";

            }

            if (strRelatedType == "MasterPlan")   //ChineseWord
            {
                strHQL = " from Document as document where document.RelatedType = 'MasterPlan' and document.RelatedID = " + strRelatedID;   //ChineseWord
                strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select planRelatedLeader.PlanID from PlanRelatedLeader as planRelatedLeader where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + ")))";   //ChineseWord
            }

            if (strRelatedType == "Material")   //ChineseWord
            {
                strHQL = " from Document as document where document.RelatedType = 'Material' and document.RelatedID = " + strRelatedID;   //ChineseWord
                strHQL += " and (document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";

            }
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' ";

            LB_Sql.Text = strHQL;

            strHQL += "  Order by document.DocID DESC";

            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();

            InitialProjectDocTypeTree();
        }
    }

    //列出没有完成的项目文档模板
    protected void LoadNotFinishedProjectDocument(string strProjectType)
    {
        string strHQL;

        //string strUserCode, strDepartCode;

        //strUserCode = HttpContext.Current.Session["UserCode"].ToString();
        //strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        //strHQL = string.Format(@"Select * From T_Document Where RelatedType = 'ProjectType' and RelatedName = '{3}' and rtrim(ltrim(Status)) <> 'Deleted' 
        //            and DocName NOT IN (Select D.DocName From (select distinct B.DocID, B.DocName From  (Select DocID,  DocName  from T_Document as document where ((document.RelatedType = 'Project' and document.RelatedID = {0}))
        //                or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID = {0}))
        //                or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From T_WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = {0}))
        //                or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from T_ProjectRisk as projectRisk where projectRisk.ProjectID = {0}))
        //                or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from T_ProjectTask as projectTask where projectTask.ProjectID = {0}))
        //                or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID From T_ImplePlan as workPlan where workPlan.ProjectID = {0}))
        //                or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From T_WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID From T_ImplePlan as workPlan where workPlan.ProjectID = {0})))
        //                or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from T_Meeting as meeting where meeting.RelatedID = {0}))
        //                ))
        //                and rtrim(ltrim(document.Status)) <> 'Deleted') A, (Select DocID, DocName From T_Document Where RelatedType = 'ProjectType' and RelatedName = '{3}' and rtrim(ltrim(Status)) <> 'Deleted') B
        //             Where A.DocName  Like '%' || B.DocName || '%') D)", strProjectID, strUserCode, strDepartCode, strProjectType);  

   
        strHQL = string.Format(@"select * FROM   
            t_documentForProjectPlanTemplate

         where DocName  in (

            SELECT   
                DocName
            FROM   
                t_documentForProjectPlanTemplate t2  
            WHERE   
                EXISTS (  
                    SELECT 1  
                    FROM T_Document t1  
                    WHERE  RelatedType='Plan' and RelatedID = {0} and Status<>  'Deleted' and find_longest_common_substring(t2.DocName, t1.DocName) IS NOT NULL  
                ) and RelatedType='Plan' and Status<> 'Deleted' and RelatedID in (Select ID From T_ImplePlan Where ProjectID = {0}))", strProjectID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Document");


        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void InitialProjectDocTypeTree()
    {
        string strHQL, strUserCode, strDocTypeID, strTotalDocType = "", strDocType, strDepartCode;
        IList lst;

        //添加根节点
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + strRelatedType + "：" + strRelatedID + " " + strRelatedName + LanguageHandle.GetWord("WenDangLieBiaoB").ToString().Trim();
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        if (strRelatedType == "Project")
        {
            strHQL = "from DocTypeFilter as docTypeFilter where docTypeFilter.DocTypeID In (Select document.DocTypeID from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire')))";   //ChineseWord
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";

            strHQL += " or (document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";

            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlowBackup.WLID From WorkFlowBackup as workFlowBackup Where workFlowBackup.RelatedType = 'Project' and workFlowBackup.RelatedID = " + strProjectID + "))";

            strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";

            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";
            strHQL += " or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlowBackup.WLID From WorkFlowBackup as workFlowBackup Where workFlowBackup.RelatedType = 'Plan' and workFlowBackup.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + ")))";

            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))";  
            strHQL += " and ((document.Visible in ('Meeting','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
            strHQL += " or (document.Visible = 'Entire' ))))";   //ChineseWord
        }
        else
        {
            strHQL = "from DocTypeFilter as docTypeFilter where docTypeFilter.RelatedType = " + "'" + strRelatedType + "'" + " and docTypeFilter.RelatedID = " + strRelatedID;
        }

        DocTypeFilterBLL docTypeFilterBLL = new DocTypeFilterBLL();
        DocTypeFilter docTypeFilter = new DocTypeFilter();
        lst = docTypeFilterBLL.GetAllDocTypeFilters(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            docTypeFilter = (DocTypeFilter)lst[i];

            strDocTypeID = docTypeFilter.DocTypeID.ToString();

            strDocType = docTypeFilter.DocType.Trim();

            if (strTotalDocType.IndexOf(strDocType) <= -1)
            {
                strTotalDocType += strDocType + ",";

                node3 = new TreeNode();

                node3.Text = docTypeFilter.DocTypeID.ToString().Trim() + "." + strDocType;
                node3.Target = strDocTypeID;
                node3.Expanded = false;

                node1.ChildNodes.Add(node3);
                TreeView1.DataBind();
            }
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode, strDocType;
        IList lst1, lst2;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strDocTypeID = treeNode.Target.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        DocumentBLL documentBLL = new DocumentBLL();
        strHQL = LB_Sql.Text.Trim();

        if (strDocTypeID != "0")
        {
            if (lst1.Count > 0)
            {
                DocType docType = (DocType)lst1[0];
                strDocType = docType.Type.Trim();

                strHQL += " and document.DocType = " + "'" + strDocType + "'" + " and document.Status <> 'Deleted' Order by document.DocID DESC";


                lst2 = documentBLL.GetAllDocuments(strHQL);
                DataGrid1.DataSource = lst2;
                DataGrid1.DataBind();

                LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX").ToString().Trim() + strDocType;

                LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst2.Count.ToString();

                ////设置缺省的文件类型
                //ShareClass.SetDefaultDocType(strDocType, LB_DocTypeID, TB_DocType);
                ////按文件类型设置缺省的工作流模板树
                //ShareClass.SetDefaultWorkflowTemplate(strDocType, DL_TemName);
            }
        }
        else
        {
            strHQL += "  and document.Status <> 'Deleted' Order by document.DocID DESC";

            lst2 = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst2;
            DataGrid1.DataBind();

            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst2.Count.ToString();
        }
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


    public string GetNotPlanTemplatedDocumentNotInDocumentList(string strPlanID)
    {
        string strHQL;
        string strDocNameListString = "";

        strHQL = string.Format(@"select * FROM   
            t_documentForProjectPlanTemplate
	
	        where DocName Not in (

            SELECT   
                DocName
            FROM   
                t_documentForProjectPlanTemplate t2  
            WHERE   
                EXISTS (  
                    SELECT 1  
                    FROM T_Document t1  
                    WHERE  RelatedType='Plan' and RelatedID = {0} and find_longest_common_substring(t2.DocName, t1.DocName) IS NOT NULL  
                ) and RelatedType='Plan' and RelatedID = {0})", strPlanID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Document");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strDocNameListString += ds.Tables[0].Rows[0]["DocName"].ToString() + "， ";
        }

        return strDocNameListString;
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

    protected string GetPlanName(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        Plan plan = (Plan)lst[0];

        return plan.PlanName.Trim();
    }

    protected string GetItemBomVersionBomID(string strItemCode, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " and itemBomVersion.VerID = " + strVerID;
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);
        if (lst.Count > 0)
        {
            ItemBomVersion itemBomVersion = (ItemBomVersion)lst[0];
            return itemBomVersion.ID.ToString();
        }
        else
        {
            return "0";
        }
    }
}
