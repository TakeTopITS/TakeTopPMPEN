using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTDocumentManageForDepartment : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartCode, strUserName;

        string strParentDepartString, strUnderDepartString;

        strUserCode = Session["UserCode"].ToString();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        //this.Title = "部门人员文档管理---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "部门人员文档", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {

            strParentDepartString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);
            strUnderDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

            LB_ParentDepratString.Text = strParentDepartString;
            LB_UnderDepartStringg.Text = strUnderDepartString;

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            ShareClass.InitialAllUserDocTypeTree(TreeView3, strUserCode);

            LB_QueryScope.Text = LanguageHandle.GetWord("MyDocumentList");

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            strHQL = "from Document as document where ";
            strHQL += "  (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
            strHQL += " or document.Visible = 'Group' or document.Visible = 'All')";  

            strHQL += " or ((document.DocID in (Select docRelatedUser.DocID From DocRelatedUser as docRelatedUser where docRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
            strHQL += "or (document.Visible = 'Company' and document.DocID in (Select docRelatedDepartment.DocID From DocRelatedDepartment as docRelatedDepartment where docRelatedDepartment.DepartCode in" + strParentDepartString + "))";  
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))";
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select reqAssignRecord.ReqID from ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'" + " or reqAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + "))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select planRelatedLeader.PlanID from PlanRelatedLeader as planRelatedLeader where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + "))";   
            strHQL += " or (document.RelatedType = 'Contract' and document.RelatedID in (select constract.ConstractID from Constract as constract where constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")))";   
            strHQL += "or (document.Visible = 'Company' and (document.DepartCode in" + strParentDepartString + " or document.DepartCode in " + strUnderDepartString + "))))";  
        
            strHQL += " and document.Status <> 'Deleted'";
            strHQL += " Order by document.DocID DESC";
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID = TreeView3.SelectedNode.Target;
        string strDocType = GetDocTypeName(strDocTypeID);

        LB_DocTypeID.Text = strDocTypeID;
        TB_DocType.Text = strDocType;
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName, strHQL;
        IList lst;

        string strParentDepartString, strUnderDepartString;

        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen") + strDepartName + LanguageHandle.GetWord("ZZDWDLB");

            strHQL = "from Document as document where document.DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
            strHQL += " or document.Visible = 'Group' or document.Visible = 'All')";  

            strHQL += " or ((document.DocID in (Select docRelatedUser.DocID From DocRelatedUser as docRelatedUser where docRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
            strHQL += "or (document.Visible = 'Company' and document.DocID in (Select docRelatedDepartment.DocID From DocRelatedDepartment as docRelatedDepartment where docRelatedDepartment.DepartCode in" + strParentDepartString + "))";  
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))";
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select reqAssignRecord.ReqID from ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'" + " or reqAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + "))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select planRelatedLeader.PlanID from PlanRelatedLeader as planRelatedLeader where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + "))";   
            strHQL += " or (document.RelatedType = 'Contract' and document.RelatedID in (select constract.ConstractID from Constract as constract where constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")))";   
            strHQL += "or (document.Visible = 'Company' and (document.DepartCode in" + strParentDepartString + " or document.DepartCode in " + strUnderDepartString + "))))";  

            strHQL += " and document.Status <> 'Deleted'";
            strHQL += " Order by document.DocID DESC";
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUploadManCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUploadManName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        string strHQL;
        string strParentDepartString, strUnderDepartString;

        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

        LB_QueryScope.Text = LanguageHandle.GetWord("User") + ":" + strUploadManName + LanguageHandle.GetWord("ZZDWDLB");

        strHQL = "from Document as document where document.UploadManCode = " + "'" + strUploadManCode + "'";
        strHQL += " and (((document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
        strHQL += " or document.Visible = 'Group' or document.Visible = 'All')";  

        strHQL += " or ((document.DocID in (Select docRelatedUser.DocID From DocRelatedUser as docRelatedUser where docRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += "or (document.Visible = 'Company' and document.DocID in (Select docRelatedDepartment.DocID From DocRelatedDepartment as docRelatedDepartment where docRelatedDepartment.DepartCode in" + strParentDepartString + "))";  
        strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))";
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " ))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " ))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " ))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select reqAssignRecord.ReqID from ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'" + " or reqAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + "))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select planRelatedLeader.PlanID from PlanRelatedLeader as planRelatedLeader where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + "))";   
        strHQL += " or (document.RelatedType = 'Contract' and document.RelatedID in (select constract.ConstractID from Constract as constract where constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")))";   
        strHQL += "or (document.Visible = 'Company' and (document.DepartCode in" + strParentDepartString + " or document.DepartCode in " + strUnderDepartString + "))))";  

        strHQL += " and document.Status <> 'Deleted'";
        strHQL += " Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        IList lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strParentDepartString, strUnderDepartString;

        string strDocType = TB_DocType.Text.Trim();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        string strDocName = TB_DocName.Text;

        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZWJMBH") + strDocName;

        strDocType = "%" + strDocType + "%";
        strDocName = "%" + strDocName + "%";
        strHQL = "from Document as document where document.DocName like " + "'" + strDocName + "'" + " and document.DocType  Like  " + "'" + strDocType + "'"; ;
        strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
        strHQL += " or document.Visible = 'Group' or document.Visible = 'All')";  

        strHQL += " or ((document.DocID in (Select docRelatedUser.DocID From DocRelatedUser as docRelatedUser where docRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += "or (document.Visible = 'Company' and document.DocID in (Select docRelatedDepartment.DocID From DocRelatedDepartment as docRelatedDepartment where docRelatedDepartment.DepartCode in" + strParentDepartString + "))";  
        strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))";
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " ))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " ))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " ))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select reqAssignRecord.ReqID from ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'" + " or reqAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + "))";   
        strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select planRelatedLeader.PlanID from PlanRelatedLeader as planRelatedLeader where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + "))";   
        strHQL += " or (document.RelatedType = 'Contract' and document.RelatedID in (select constract.ConstractID from Constract as constract where constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")))";   
        strHQL += "or (document.Visible = 'Company' and (document.DepartCode in" + strParentDepartString + " or document.DepartCode in " + strUnderDepartString + "))))";  

        strHQL += " and document.Status <> 'Deleted'";
        strHQL += " Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
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

    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }

}
