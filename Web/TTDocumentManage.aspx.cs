using ProjectMgt.BLL;
using ProjectMgt.Model;

using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTDocumentManage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strParentDepartString, strUnderDepartString;

        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);


        LB_UserCode.Text = strUserCode;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "KnowledgeManagement", strUserCode);


        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.Year.ToString() + "-01-01";
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strParentDepartString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);
            strUnderDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

            ShareClass.InitialAllUserDocTypeTree(TreeView1, strUserCode);

            LB_ParentDepratString.Text = strParentDepartString;
            LB_UnderDepartStringg.Text = strUnderDepartString;

            ShareClass.LoadActorGroupDropDownList(DL_Authority, strUserCode);
            ShareClass.InitialPrjectTreeByAuthority(TreeView2, strUserCode);

            strHQL = "from Document as document where (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
            strHQL += " or document.Visible = 'Group'  or document.Visible = 'All')";  
            strHQL += " or ((document.DocID in (Select docRelatedUser.DocID From DocRelatedUser as docRelatedUser where docRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
            strHQL += "or (document.Visible = 'Company' and document.DocID in (Select docRelatedDepartment.DocID From DocRelatedDepartment as docRelatedDepartment where docRelatedDepartment.DepartCode in" + strParentDepartString + "))";  
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))";
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select collaborationMember.CoID from CollaborationMember as collaborationMember where collaborationMember.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " ))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Defect' and document.RelatedID in (select relatedDefect.DefectID from RelatedDefect as relatedDefect where relatedDefect.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID in (select proRelatedUser.ProjectID from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " )))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select reqAssignRecord.ReqID from ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'" + " or reqAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + "))";   
            strHQL += " or (document.Visible = 'Entire' and document.RelatedID in (select planRelatedLeader.PlanID from PlanRelatedLeader as planRelatedLeader where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + "))";   
            strHQL += " or (document.RelatedType = 'Contract' and document.RelatedID in (select constract.ConstractID from Constract as constract where constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")))";   
            strHQL += "or (document.Visible = 'Company' and (document.DepartCode in" + strParentDepartString + " or document.DepartCode in " + strUnderDepartString + "))))";  


            strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();

            LB_Sql.Text = strHQL;

            LB_TotalCount.Text = LanguageHandle.GetWord("ZongWenJianShu") + lst.Count.ToString();
        }
    }


    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID, strProjectName, strHQL;
        string strUserName;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        strProjectID = treeNode.Target.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        if (lst.Count > 0)
        {
            Project project = (Project)lst[0];

            strProjectName = project.ProjectName.Trim();

            LB_ParentProjectID.Text = project.ProjectID.ToString();
            TB_ParentProject.Text = project.ProjectName.Trim();
        }
        else
        {
            LB_ParentProjectID.Text = "0";
            TB_ParentProject.Text = LanguageHandle.GetWord("ZongXiangMu");
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode;
        IList lst;

        string strParentDepartString, strUnderDepartString;

        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strDocTypeID = treeNode.Target.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);
        DocumentBLL documentBLL = new DocumentBLL();

        try
        {
            if (lst.Count > 0)
            {
                DocType docType = (DocType)lst[0];

                LB_DocTypeID.Text = strDocTypeID;
                TB_DocType.Text = docType.Type.Trim();
                NB_DocTypeSort.Amount = docType.SortNumber;

                try
                {
                    DL_Authority.SelectedValue = docType.SaveType.Trim();
                }
                catch
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + docType.SaveType + " " + LanguageHandle.GetWord("ActorGroup") + LanguageHandle.GetWord("IsNotExist") + "')", true);
                }


                strHQL = "from Document as document where  document.DocTypeID = " + "'" + strDocTypeID + "'";

                strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
                strHQL += " or document.Visible = 'Group'  or document.Visible = 'All')";  

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
                strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
                lst = documentBLL.GetAllDocuments(strHQL);
                DataGrid1.DataSource = lst;
                DataGrid1.DataBind();

                LB_Sql.Text = strHQL;

                if (docType.UserCode.Trim() == strUserCode & DataGrid1.Items.Count == 0)
                {
                    BT_UpdateDocType.Enabled = true;
                    BT_DeleteDocType.Enabled = true;
                }
                else
                {
                    BT_UpdateDocType.Enabled = false;
                    BT_DeleteDocType.Enabled = false;
                }

                if (docType.UserCode.Trim() == strUserCode)
                {
                    BT_UpdateDocType.Enabled = true;
                }
            }
            else
            {
                strHQL = "from Document as document where ";
                strHQL += " (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
                strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
                strHQL += " or (document.Visible = 'Group')  or (document.Visible = 'All'))";  

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
                strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
                lst = documentBLL.GetAllDocuments(strHQL);
                DataGrid1.DataSource = lst;
                DataGrid1.DataBind();

                LB_Sql.Text = strHQL;

                LB_DocTypeID.Text = "0";
                TB_DocType.Text = "";
                NB_DocTypeSort.Amount = 0;
            }

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWNKNSCLJSZLGRBMGSZGSGJSZJC") + "')", true);
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

    protected void BT_NewDocType_Click(object sender, EventArgs e)
    {
        string strDocType = TB_DocType.Text.Trim();
        string strSortNumber = NB_DocTypeSort.Amount.ToString();
        string strUserCode = Session["UserCode"].ToString();
        string strParentID = LB_DocTypeID.Text.Trim();
        string strAuthority = DL_Authority.SelectedValue.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        try
        {
            docType.Type = strDocType;
            docType.SortNumber = int.Parse(strSortNumber);
            docType.ParentID = int.Parse(strParentID);
            docType.UserCode = strUserCode;
            docType.SaveType = strAuthority;

            docTypeBLL.AddDocType(docType);

            ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);
        }
        catch
        {
        }

    }
    protected void BT_DeleteDocType_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();

        string strDocTypeID = LB_DocTypeID.Text.Trim();
        string strDocType = TB_DocType.Text.Trim();
        string strSortNumber = NB_DocTypeSort.Amount.ToString();

        DocTypeBLL docTypeBLL = new DocTypeBLL();

        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];

            if (strUserCode != docType.UserCode.Trim() | DataGrid1.Items.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGWJLXBSNCJDHCZCLXWJNBNSC") + "')", true);
            }
            else
            {
                try
                {
                    docTypeBLL.DeleteDocType(docType);

                    ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB") + "')", true);
                }
            }
        }
    }

    protected void BT_UpdateDocType_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode;
        int intID;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();

        string strDocTypeID = LB_DocTypeID.Text.Trim();
        string strDocType = TB_DocType.Text.Trim();
        string strSortNumber = NB_DocTypeSort.Amount.ToString();
        string strAuthority = DL_Authority.SelectedValue.Trim();
        intID = int.Parse(strDocTypeID);

        DocTypeBLL docTypeBLL = new DocTypeBLL();

        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];

            if (strUserCode != docType.UserCode.Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGWJLXBSNCJDNBNGG") + "')", true);
            }
            else
            {
                try
                {
                    docType.Type = strDocType;
                    docType.SortNumber = int.Parse(strSortNumber);
                    docType.SaveType = strAuthority;

                    docTypeBLL.UpdateDocType(docType, intID);

                    ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
                }
            }
        }
    }

    protected void BT_HazyFind_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        string strDocName = TB_DocName.Text.Trim();
        string strUploadManName = TB_UploadManName.Text.Trim();

        string strProjectID = LB_ParentProjectID.Text;
        string strProjectName = "%" + TB_ParentProject.Text.Trim() + "%";

        string strParentDepartString, strUnderDepartString;

        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

        strDocName = "%" + strDocName + "%";
        strUploadManName = "%" + strUploadManName + "%";

        string strStartTime, strEndTime;
        strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");


        if (strProjectID == "")
        {
            strHQL = "from Document as document where document.DocName like " + "'" + strDocName + "'" + " and UploadManName Like " + "'" + strUploadManName + "'";
            strHQL += " and (to_char(document.UploadTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(document.UploadTime,'yyyymmdd') <= " + "'" + strEndTime + "')";


            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + ")";  
            strHQL += " or (document.Visible = 'Group')  or (document.Visible = 'All'))";  


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

            strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";

            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
        }
        else
        {
            LoadProjectDoc(strUserCode, "Project", LB_ParentProjectID.Text);
        }
    }

    protected void LoadProjectDoc(string strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL = "";
        IList lst;

        string strDepartCode;
        string strRelatedName;

        string strDocName = TB_DocName.Text.Trim();
        string strUploadManName = TB_UploadManName.Text.Trim();

        strDocName = "%" + strDocName + "%";
        strUploadManName = "%" + strUploadManName + "%";

        string strStartTime, strEndTime;
        strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");


        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";
            strRelatedName = ShareClass.GetProjectName(strRelatedID);
        }

        if (strRelatedType == "Project")
        {
            strHQL = "from Document as document where document.DocName like " + "'" + strDocName + "'" + " and UploadManName Like " + "'" + strUploadManName + "' and (((document.RelatedType = 'Project' and document.RelatedID = " + strRelatedID + ")";
            strHQL += " and (to_char(document.UploadTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(document.UploadTime,'yyyymmdd') <= " + "'" + strEndTime + "')";


            strHQL += " and (((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire')))";   
            strHQL += " or (document.Visible in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + " ))))";
            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strRelatedID + "))";  
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strRelatedID + "))";
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strRelatedID + "))";  
            strHQL += " and ((document.Visible in ('Meeting','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
            strHQL += " or (document.Visible = 'Entire' )))))";   
        }
        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

       
        LB_Sql.Text = strHQL;

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
    }


    protected string GetDocTypeCreator(string strDocTypeID)
    {
        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.UserCode.Trim();
    }


}
