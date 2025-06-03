using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTSuperMakeProject : System.Web.UI.Page
{
    string strIsMobileDevice;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName, strProjectType;
        IList lst;

        strLangCode = Session["LangCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_AcceptStandard);
HE_AcceptStandard.Language = Session["LangCode"].ToString();
        _FileBrowser.SetupCKEditor(HE_ProjectDetail);
HE_ProjectDetail.Language = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "查看所有项目", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        LB_PMCode.Enabled = false;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_ProjectDetail.Visible = true;
                HT_AcceptStandard.Visible = true;
            }
            else
            {
                HE_ProjectDetail.Visible = true;
                HE_AcceptStandard.Visible = true;
            }

            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strSystemVersionType = Session["SystemVersionType"].ToString();
            if (strSystemVersionType != "GROUP" & strSystemVersionType != "ENTERPRISE")
            {
                BT_DirectDepartment.Visible = false;
            }

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityProjectLeader(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3, strUserCode);
            ShareClass.InitialPrjectTreeByAuthority(TreeView4, strUserCode);

            try
            {
                ShareClass.InitialPrjectTreeWithDeleteLine(TreeView1);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCCZFXMWBSXMHZXMDXMDJLYFPXMYMGZ").ToString().Trim() + "')", true);
            }

            LB_ProjectList.Text = LanguageHandle.GetWord("SuoYouXiangMuLieBiao").ToString().Trim();

            TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);

            strHQL = "from ProjectMember as projectMember where projectMember.UserCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ") Order By projectMember.SortNumber ASC";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            DL_NewPMName.DataSource = lst;
            DL_NewPMName.DataBind();

            ShareClass.LoadProjectType(DL_ProjectType);

            strProjectType = DL_ProjectType.SelectedItem.Text.Trim();
            ShareClass.LoadProjectForPMStatus(strProjectType, strLangCode,DL_Status);

            strHQL = "from Project as project order by project.ProjectID DESC";
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_Sql.Text = strHQL;

            //列出可用的工作流模板
            ShareClass.LoadProjectPlanStartupRelatedWorkflowTemplate(strUserCode, DL_PlanStartupRelatedWorkflowTemplate);
        }
    }

    protected void TreeView4_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID, strProjectName, strHQL;
        string strUserName;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView4.SelectedNode;

        strProjectID = treeNode.Target.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strUserName = LB_UserName.Text.Trim();
        strProjectName = project.ProjectName.Trim();

        LB_ParentProjectID.Text = project.ProjectID.ToString();
        TB_ParentProject.Text = project.ProjectName.Trim();
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID, strProjectName, strHQL, strStatus;
        string strUserName;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strProjectID = treeNode.Target.Trim();

        if (strProjectID == "0" | strProjectID == "1")
        {
            BT_Upate.Enabled = false;
            BT_Delete.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCKNSSTXGZXMJC").ToString().Trim() + "')", true);
            return;
        }

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strUserName = LB_UserName.Text.Trim();
        strProjectName = project.ProjectName.Trim();

        try
        {
            BT_Upate.Enabled = true;
            BT_Delete.Enabled = true;
            BT_SetTemProject.Enabled = true;
            BT_SetCommonProject.Enabled = true;

            BT_TransferProject.Enabled = true;
            CB_SMS.Enabled = true;
            CB_Mail.Enabled = true;
            BT_Send.Enabled = true;

            LB_ProjectID.Text = project.ProjectID.ToString();
            try
            {
                try
                {
                    DL_ProjectType.SelectedValue = project.ProjectType;
                }
                catch
                {
                    DL_ProjectType.SelectedValue = project.ProjectType.Trim();
                }
                ShareClass.LoadProjectForPMStatus(project.ProjectType.Trim(), strLangCode, DL_Status);
            }
            catch
            {
            }

            LB_ProjectClass.Text = project.ProjectClass.Trim();

            LB_PMCode.Text = project.PMCode;
            LB_PMName.Visible = true;
            LB_PMName.Text = ShareClass.GetUserName(project.PMCode.Trim());
            TB_CustomerPMName.Text = project.CustomerPMName;
            LB_Status.Text = project.Status.Trim();
            DLC_BeginDate.Text = project.BeginDate.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = project.EndDate.ToString("yyyy-MM-dd");
            TB_ProjectName.Text = project.ProjectName;

            if (strIsMobileDevice == "YES")
            {
                HT_ProjectDetail.Text = project.ProjectDetail.Trim();
                HT_AcceptStandard.Text = project.AcceptStandard.Trim();
            }
            else
            {
                HE_ProjectDetail.Text = project.ProjectDetail.Trim();
                HE_AcceptStandard.Text = project.AcceptStandard.Trim();
            }

            NB_Budget.Amount = project.Budget;

            NB_ProjectAmount.Amount = project.ProjectAmount;
            NB_ManHour.Amount = project.ManHour;
            NB_ManNubmer.Amount = project.ManNumber;

            try
            {
                try
                {
                    DL_Status.SelectedValue = project.Status;
                }
                catch
                {
                    DL_Status.SelectedValue = project.Status.Trim();
                }
                DL_StatusValue.SelectedValue = project.StatusValue.Trim();
            }
            catch
            {
            }

            DL_Priority.SelectedValue = project.Priority.Trim();

            //取得项目的其它属性
            SetProjectOtherFieldValue(strProjectID);

            strStatus = project.Status.Trim();

            HL_ProjectRelatedDoc.Enabled = true;
            HL_ProjectRelatedDoc.NavigateUrl = "TTProjectRelatedDoc.aspx";
            HL_ProjectRelatedDoc.NavigateUrl = HL_ProjectRelatedDoc.NavigateUrl + "?ProjectID=" + project.ProjectID.ToString();
            HL_RelatedConstract.Enabled = true;
            HL_RelatedConstract.NavigateUrl = "TTProjectRelatedConstract.aspx?ProjectID=" + project.ProjectID;
            HL_ProjectRelatedReq.Enabled = true;
            HL_ProjectRelatedReq.NavigateUrl = "TTProjectRelatedReq.aspx";
            HL_ProjectRelatedReq.NavigateUrl = HL_ProjectRelatedReq.NavigateUrl + "?ProjectID=" + project.ProjectID.ToString();
            HL_ProjectRelatedUser.Enabled = true;
            HL_ProjectRelatedUser.NavigateUrl = "TTProjectRelatedUser.aspx";
            HL_ProjectRelatedUser.NavigateUrl = HL_ProjectRelatedUser.NavigateUrl + "?ProjectID=" + project.ProjectID.ToString();
            HL_ProjectRelatedRisk.Enabled = true;
            HL_ProjectRelatedRisk.NavigateUrl = "TTProRelatedRisk.aspx?ProjectID=" + strProjectID;
            HL_StatusChangeRecord.Enabled = true;
            HL_StatusChangeRecord.NavigateUrl = "TTProStatusChangeRecord.aspx?ProjectID=" + strProjectID;
            HL_TransferProject.Enabled = true;
            HL_TransferProject.NavigateUrl = "TTTransferProjectRecord.aspx?ProjectID=" + strProjectID;
            HL_DailyWork.Enabled = true;
            HL_DailyWork.NavigateUrl = "TTProjectSummaryAnalystChart.aspx?ProjectID=" + strProjectID;
            HL_Expense.Enabled = true;
            HL_Expense.NavigateUrl = "TTProjectExpenseReport.aspx?ProjectID=" + strProjectID;

            HL_CustomerInfo.Enabled = true;
            HL_CustomerInfo.NavigateUrl = "TTProjectCustomerInfo.aspx?ProjectID=" + strProjectID;
            HL_VendorInfo.Enabled = true;
            HL_VendorInfo.NavigateUrl = "TTProjectVendorInfo.aspx?ProjectID=" + strProjectID;
            HL_MakePlan.Enabled = true;
            HL_MakePlan.NavigateUrl = "TTSuperWorkPlanMain.aspx?ProjectID=" + strProjectID;
            HL_ProjectReviewWL.Enabled = true;
            HL_ProjectReviewWL.NavigateUrl = "TTProjectReviewWL.aspx?ProjectID=" + strProjectID + "&Type=Project&ProjectStatus=NONE";
            HL_StatusReview.Enabled = true;
            HL_StatusReview.NavigateUrl = "TTProjectReviewWL.aspx?ProjectID=" + strProjectID + "&Type=Status&ProjectStatus=" + GetProjectStatusIdentityString(strStatus);
            HL_RelatedWorkFlowTemplate.Enabled = true;
            HL_RelatedWorkFlowTemplate.NavigateUrl = "TTProRelatedWorkFlowTemplate.aspx?ProjectID=" + strProjectID;

            //会出错，这个错误是防止修改总项目
            LB_ParentProjectID.Text = project.ParentID.ToString();
            TB_ParentProject.Text = ShareClass.GetProjectName(project.ParentID.ToString());

            TB_Message.Text = strUserName + LanguageHandle.GetWord("GeiNiJianLiLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();

            LB_ErrorMsg.Text = "";
        }
        catch (Exception err)
        {
            LB_ErrorMsg.Text = LanguageHandle.GetWord("JingGao").ToString().Trim() + "," + LanguageHandle.GetWord("XTFSCWYYHCSRX").ToString().Trim() + ": " + err.Message.ToString();
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            strHQL = "from Project as project where (project.PMCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") Or project.BelongDepartCode = '" + strDepartCode + "') Order by project.ProjectID DESC";
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strUserCode, strCustomerPMName, strUserName, strStatus, strStatusValue, strBeginDate;
        string strEndDate, strProject, strDetail, strAcceptStandard, strParentID;
        string strBudget;
        string strHQL;
        string strProjectID;
        string strProjectType, strPriority;
        IList lst;
        string strPMCode;
        decimal deProjectAmount, deManHour, deManNumber;

        string strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue;

        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        RelatedUser relatedUser = new RelatedUser();

        strUserCode = LB_UserCode.Text;
        strUserName = ShareClass.GetUserName(strUserCode);
        strPMCode = LB_PMCode.Text.Trim();
        strCustomerPMName = TB_CustomerPMName.Text.Trim();

        strBeginDate = DLC_BeginDate.Text;
        strEndDate = DLC_EndDate.Text;
        strProject = TB_ProjectName.Text.Trim();
        strProjectType = DL_ProjectType.SelectedValue.Trim();

        if (strIsMobileDevice == "YES")
        {
            strDetail = HT_ProjectDetail.Text.Trim();
            strAcceptStandard = HT_AcceptStandard.Text.Trim();
        }
        else
        {
            strDetail = HE_ProjectDetail.Text.Trim();
            strAcceptStandard = HE_AcceptStandard.Text.Trim();
        }

        deProjectAmount = NB_ProjectAmount.Amount;
        deManHour = NB_ManHour.Amount;
        deManNumber = NB_ManNubmer.Amount;
        strStatus = DL_Status.SelectedValue.Trim();
        strStatusValue = DL_StatusValue.SelectedValue.Trim();

        strParentID = LB_ParentProjectID.Text.Trim();
        strProjectID = LB_ProjectID.Text;
        strBudget = NB_Budget.Amount.ToString();

        strNewStatus = DL_Status.SelectedValue.Trim();
        strNewStatusValue = DL_StatusValue.SelectedValue.Trim();

        strPriority = DL_Priority.SelectedValue.Trim();

        if (strParentID == strProjectID)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWBNZSHZXMZWFXMJC").ToString().Trim() + "')", true);
            return;
        }

        if (strPMCode == "" | strStatus == "" | strBeginDate == "" | strEndDate == "" | strProject == "" | strDetail == "" | strAcceptStandard == "" | strParentID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            Project project = (Project)lst[0];

            strOldStatus = project.Status.Trim();
            strOldStatusValue = project.StatusValue.Trim();

            project.UserCode = strUserCode;
            project.UserName = ShareClass.GetUserName(strUserCode);
            project.PMCode = strPMCode;
            project.PMName = ShareClass.GetUserName(strPMCode);
            project.CustomerPMName = strCustomerPMName;
            project.ProjectName = strProject;
            project.ProjectType = strProjectType;
            project.Budget = decimal.Parse(strBudget);
            project.ProjectDetail = strDetail;
            project.AcceptStandard = strAcceptStandard;
            project.BeginDate = DateTime.Parse(strBeginDate);
            project.EndDate = DateTime.Parse(strEndDate);
            project.MakeDate = DateTime.Now;
            project.Status = strStatus;
            project.StatusValue = strStatusValue;
            project.ProjectAmount = deProjectAmount;
            project.ManHour = deManHour;
            project.ManNumber = deManNumber;

            project.Priority = strPriority;

            project.ParentID = int.Parse(strParentID);

            if (strProjectID == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXWLBNXGJC").ToString().Trim() + "')", true);
            }
            else
            {
                try
                {
                    projectBLL.UpdateProject(project, int.Parse(strProjectID));

                    //保存项目的其它属性
                    UpdateProjectOtherFieldValue(strProjectID);

                    AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue);
                    LB_Status.Text = strNewStatus;

                    LoadProject(strPMCode, strUserCode);

                    ShareClass.InitialPrjectTreeWithDeleteLine(TreeView1);

                    TB_Message.Text = strUserName + LanguageHandle.GetWord("GengXinLeXiangMu").ToString().Trim() + strProjectID + " " + strProject + LanguageHandle.GetWord("DeNeiRongQingGuanZhuTeCiTongZh").ToString().Trim();

                    LB_Sql.Text = strHQL;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    //更改项目其它字段值 
    public void UpdateProjectOtherFieldValue(string strProjectID)
    {
        string strHQL;

        string strLockStartupedPlan;
        strLockStartupedPlan = DL_LockStartupedPlan.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set LockStartupedPlan = '{0}' Where ProjectID = {1}", strLockStartupedPlan, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strAllowPMChangeStatus;
        strAllowPMChangeStatus = DL_AllowPMChangeStatus.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set AllowPMChangeStatus = '{0}' Where ProjectID = {1}", strAllowPMChangeStatus, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProgressByDetailImpact;
        strProgressByDetailImpact = DL_ProgressByDetailImpact.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set ProgressByDetailImpact = '{0}' Where ProjectID = {1}", strProgressByDetailImpact, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strPlanProgressNeedPlanerConfirm;
        strPlanProgressNeedPlanerConfirm = DL_PlanProgressNeedPlanerConfirm.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set PlanProgressNeedPlanerConfirm = '{0}' Where ProjectID = {1}", strPlanProgressNeedPlanerConfirm, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strAutoRunWFAfterMakeProject;
        strAutoRunWFAfterMakeProject = DL_AutoRunWFAfterMakeProject.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set AutoRunWFAfterMakeProject = '{0}' Where ProjectID = {1}", strAutoRunWFAfterMakeProject, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProjectStartupNeedSupperConfirm;
        strProjectStartupNeedSupperConfirm = DL_ProjectStartupNeedSupperConfirm.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set ProjectStartupNeedSupperConfirm = '{0}' Where ProjectID = {1}", strProjectStartupNeedSupperConfirm, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProjectPlanStartupStauts;
        strProjectPlanStartupStauts = DL_ProjectPlanStartupSatus.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set ProjectPlanStartupStatus = '{0}' Where ProjectID = {1}", strProjectPlanStartupStauts, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProjectPlanStartupRelatedWorkflowTemplate;
        strProjectPlanStartupRelatedWorkflowTemplate = DL_PlanStartupRelatedWorkflowTemplate.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set PlanStartupRelatedWorkflowTemplate = '{0}' Where ProjectID = {1}", strProjectPlanStartupRelatedWorkflowTemplate, strProjectID);
        ShareClass.RunSqlCommand(strHQL);
    }

    //设置项目其它属性的值 
    public void SetProjectOtherFieldValue(string strProjectID)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_Project Where ProjectID={0}", strProjectID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        DL_LockStartupedPlan.SelectedValue = ds.Tables[0].Rows[0]["LockStartupedPlan"].ToString().Trim();
        DL_AllowPMChangeStatus.SelectedValue = ds.Tables[0].Rows[0]["AllowPMChangeStatus"].ToString().Trim();

        DL_ProgressByDetailImpact.SelectedValue = ds.Tables[0].Rows[0]["ProgressByDetailImpact"].ToString().Trim();
        DL_PlanProgressNeedPlanerConfirm.SelectedValue = ds.Tables[0].Rows[0]["PlanProgressNeedPlanerConfirm"].ToString().Trim();

        DL_AutoRunWFAfterMakeProject.SelectedValue = ds.Tables[0].Rows[0]["AutoRunWFAfterMakeProject"].ToString().Trim();
        DL_ProjectStartupNeedSupperConfirm.SelectedValue = ds.Tables[0].Rows[0]["ProjectStartupNeedSupperConfirm"].ToString().Trim();

        DL_ProjectPlanStartupSatus.SelectedValue = ds.Tables[0].Rows[0]["ProjectPlanStartupStatus"].ToString().Trim();

        DL_PlanStartupRelatedWorkflowTemplate.SelectedValue = ds.Tables[0].Rows[0]["PlanStartupRelatedWorkflowTemplate"].ToString().Trim();
    }

    protected void BT_SetTemProject_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectID = LB_ProjectID.Text.Trim();

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        project.ProjectClass = "Template";

        try
        {
            projectBLL.UpdateProject(project, int.Parse(strProjectID));

            LB_ProjectClass.Text = LanguageHandle.GetWord("MoBanXiangMu").ToString().Trim();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGSWMBXM").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_SetCommonProject_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectID = LB_ProjectID.Text.Trim();

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        project.ProjectClass = "NormalProject";

        try
        {
            projectBLL.UpdateProject(project, int.Parse(strProjectID));

            LB_ProjectClass.Text = "Normal";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGSWCGXM").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectID, strProjectName, strProjectCode;
        string strPMCode, strUserCode, strUserName; ;
   
        strProjectID = LB_ProjectID.Text.Trim();

        strUserCode = LB_UserCode.Text;
        strUserName = LB_UserName.Text;
        strPMCode = LB_PMCode.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        project.Status = "Deleted";

        strProjectCode = project.ProjectCode.Trim();
        strProjectName = project.ProjectName.Trim();

        try
        {
            projectBLL.DeleteProject(project);

            try
            {
                strHQL = "Delete From T_WZProject Where ProjectCode = '" + strProjectCode + "'";
                ShareClass.RunSqlCommand(strHQL);
            }
            catch
            {
            }

            LoadProject(strPMCode, strUserCode);

            ShareClass.InitialPrjectTreeWithDeleteLine(TreeView1);

            BT_Upate.Enabled = false;
            BT_Delete.Enabled = false;
            BT_SetTemProject.Enabled = false;
            BT_SetCommonProject.Enabled = false;
            BT_TransferProject.Enabled = false;

            TB_Message.Text = strUserName + LanguageHandle.GetWord("ShanChuLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("DeNeiRongCiWeiZhenShanChuQingG").ToString().Trim();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuLanguageHandleGetWord").ToString().Trim()+"')", true); 
        }
    }

    protected void DL_ProjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectType;

        strProjectType = DL_ProjectType.SelectedValue.Trim();

        ShareClass.LoadProjectForPMStatus(strProjectType, strLangCode,DL_Status);
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strProjectID, strHQL;
        string strUserCode, strUserName, strRelatedUserCode;
        IList lst;
        string strMsg, strSubject;

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strProjectID = LB_ProjectID.Text.Trim();

        strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID;
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

        RelatedUser relatedUser = new RelatedUser();

        strMsg = TB_Message.Text.Trim();

        strSubject = LanguageHandle.GetWord("XiangMuTongZhi").ToString().Trim();

        Msg msg = new Msg();

        try
        {
            for (int i = 0; i < lst.Count; i++)
            {
                relatedUser = (RelatedUser)lst[i];
                strRelatedUserCode = relatedUser.UserCode.Trim();

                if (CB_SMS.Checked == true | CB_Mail.Checked == true)
                {
                    if (CB_SMS.Checked == true)
                    {
                        msg.SendMSM("Message",strRelatedUserCode, strMsg, strUserCode);
                    }

                    if (CB_Mail.Checked == true)
                    {
                        msg.SendMail(strRelatedUserCode, strSubject, strMsg, strUserCode);
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXFSCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZXinXiFaSongLanguageHandleGet").ToString().Trim() + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true); 
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUnderlingCode = ((Button)e.Item.FindControl("BT_UnderlingCode")).Text;
        string strUnderLingName = ((Button)e.Item.FindControl("BT_UnderlingName")).Text;
        string strUserCode = LB_UserCode.Text.Trim();
        IList lst;
        string strHQL;

        LB_PMName.Visible = true;
        LB_PMCode.Text = strUnderlingCode;
        LB_PMName.Text = strUnderLingName;

        DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        TB_ProjectName.Text = "";

        if (strIsMobileDevice == "YES")
        {
            HT_ProjectDetail.Text = "";
            HT_AcceptStandard.Text = "";
        }
        else
        {
            HE_ProjectDetail.Text = "";
            HE_AcceptStandard.Text = "";
        }


        LB_ProjectList.Visible = true;
        DataGrid2.Visible = true;

        LB_ProjectID.Text = "";

        LB_ProjectList.Text = strUnderlingCode + strUnderLingName + LanguageHandle.GetWord("DeXiangMuLieBiao").ToString().Trim();

        strHQL = "from Project as project where project.PMCode = " + "'" + strUnderlingCode + "'" + " and project.UserCode = " + "'" + strUserCode + "'" + " and project.Status <> 'Archived' order by project.ProjectID DESC";

        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strProjectID, strProjectName, strHQL, strStatus;
        string strUserName;

        if (e.CommandName != "Page")
        {

            strUserName = LB_UserName.Text.Trim();

            strProjectID = ((Button)e.Item.FindControl("BT_ProjectID")).Text;

            if (strProjectID == "0" | strProjectID == "1")
            {
                BT_Upate.Enabled = false;
                BT_Delete.Enabled = false;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCKNSSTXGZXMJC").ToString().Trim() + "')", true);
                return;
            }

            try
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from Project as project where project.ProjectID = " + strProjectID;

                ProjectBLL projectBLL = new ProjectBLL();
                IList lst = projectBLL.GetAllProjects(strHQL);

                Project project = (Project)lst[0];

                strProjectName = project.ProjectName.Trim();

                LB_ProjectID.Text = project.ProjectID.ToString();
                try
                {
                    DL_ProjectType.SelectedValue = project.ProjectType;
                    ShareClass.LoadProjectForPMStatus(project.ProjectType.Trim(), strLangCode, DL_Status);
                }
                catch
                {
                }

                LB_PMCode.Text = project.PMCode;
                LB_PMName.Visible = true;
                LB_PMName.Text = ShareClass.GetUserName(project.PMCode.Trim());
                TB_CustomerPMName.Text = project.CustomerPMName;
                LB_Status.Text = project.Status.Trim();
                DLC_BeginDate.Text = project.BeginDate.ToString("yyyy-MM-dd");
                DLC_EndDate.Text = project.EndDate.ToString("yyyy-MM-dd");
                TB_ProjectName.Text = project.ProjectName;

                if (strIsMobileDevice == "YES")
                {
                    HT_ProjectDetail.Text = project.ProjectDetail.Trim();
                    HT_AcceptStandard.Text = project.AcceptStandard.Trim();
                }
                else
                {
                    HE_ProjectDetail.Text = project.ProjectDetail.Trim();
                    HE_AcceptStandard.Text = project.AcceptStandard.Trim();
                }

                NB_Budget.Amount = project.Budget;

                NB_ProjectAmount.Amount = project.ProjectAmount;
                NB_ManHour.Amount = project.ManHour;
                NB_ManNubmer.Amount = project.ManNumber;
                try
                {
                    DL_Status.SelectedValue = project.Status.Trim();
                    DL_StatusValue.SelectedValue = project.StatusValue.Trim();
                }
                catch
                {
                }

                strStatus = project.Status.Trim();

                HL_ProjectRelatedDoc.Enabled = true;
                HL_ProjectRelatedDoc.NavigateUrl = "TTProjectRelatedDoc.aspx";
                HL_ProjectRelatedDoc.NavigateUrl = HL_ProjectRelatedDoc.NavigateUrl + "?ProjectID=" + project.ProjectID.ToString();
                HL_RelatedConstract.Enabled = true;
                HL_RelatedConstract.NavigateUrl = "TTProjectRelatedConstract.aspx?ProjectID=" + project.ProjectID;
                HL_ProjectRelatedReq.Enabled = true;
                HL_ProjectRelatedReq.NavigateUrl = "TTProjectRelatedReq.aspx";
                HL_ProjectRelatedReq.NavigateUrl = HL_ProjectRelatedReq.NavigateUrl + "?ProjectID=" + project.ProjectID.ToString();
                HL_ProjectRelatedUser.Enabled = true;
                HL_ProjectRelatedUser.NavigateUrl = "TTProjectRelatedUser.aspx";
                HL_ProjectRelatedUser.NavigateUrl = HL_ProjectRelatedUser.NavigateUrl + "?ProjectID=" + project.ProjectID.ToString();
                HL_ProjectRelatedRisk.Enabled = true;
                HL_ProjectRelatedRisk.NavigateUrl = "TTProRelatedRisk.aspx?ProjectID=" + strProjectID;
                HL_StatusChangeRecord.Enabled = true;
                HL_StatusChangeRecord.NavigateUrl = "TTProStatusChangeRecord.aspx?ProjectID=" + strProjectID;
                HL_TransferProject.Enabled = true;
                HL_TransferProject.NavigateUrl = "TTTransferProjectRecord.aspx?ProjectID=" + strProjectID;
                HL_DailyWork.Enabled = true;
                HL_DailyWork.NavigateUrl = "TTProjectSummaryAnalystChart.aspx?ProjectID=" + strProjectID;
                HL_Expense.Enabled = true;
                HL_Expense.NavigateUrl = "TTProjectExpenseReport.aspx?ProjectID=" + strProjectID;

                HL_CustomerInfo.Enabled = true;
                HL_CustomerInfo.NavigateUrl = "TTProjectCustomerInfo.aspx?ProjectID=" + strProjectID;
                HL_VendorInfo.Enabled = true;
                HL_VendorInfo.NavigateUrl = "TTProjectVendorInfo.aspx?ProjectID=" + strProjectID;
                HL_MakePlan.Enabled = true;
                HL_MakePlan.NavigateUrl = "TTWorkPlanMain.aspx?ProjectID=" + strProjectID;
                HL_ProjectReviewWL.Enabled = true;
                HL_ProjectReviewWL.NavigateUrl = "TTProjectReviewWL.aspx?ProjectID=" + strProjectID + "&Type=Project&ProjectStatus=NONE";
                HL_StatusReview.Enabled = true;
                HL_StatusReview.NavigateUrl = "TTProjectReviewWL.aspx?ProjectID=" + strProjectID + "&Type=Status&ProjectStatus=" + GetProjectStatusIdentityString(strStatus);
                HL_RelatedWorkFlowTemplate.Enabled = true;
                HL_RelatedWorkFlowTemplate.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=Project&RelatedID=" + strProjectID;

                //会出错，这个错误是防止修改总项目
                LB_ParentProjectID.Text = project.ParentID.ToString();
                TB_ParentProject.Text = ShareClass.GetProjectName(project.ParentID.ToString());

                BT_Upate.Enabled = true;
                BT_Delete.Enabled = true;
                BT_SetTemProject.Enabled = true;
                BT_SetCommonProject.Enabled = true;

                BT_TransferProject.Enabled = true;
                CB_SMS.Enabled = true;
                CB_Mail.Enabled = true;
                BT_Send.Enabled = true;

                TB_Message.Text = strUserName + LanguageHandle.GetWord("GeiNiJianLiLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();
            }
            catch(Exception err)
            {
                BT_Upate.Enabled = false;
                BT_Delete.Enabled = false;
                BT_TransferProject.Enabled = false;
                BT_Send.Enabled = false;

                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCKNSSTXGZXMHCXMDZTBZZTLBZJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_AllProject_Click(object sender, EventArgs e)
    {
        IList lst;
        string strUserCode = LB_UserCode.Text.Trim();
        string strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'" + " and  project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ") and project.Status <> 'Archived' order by project.ProjectID DESC";

        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_ProjectList.Visible = true;
        LB_ProjectList.Text = LanguageHandle.GetWord("WoChengYuanDeSuoYouXiangMuLieB").ToString().Trim();

        DataGrid2.Visible = true;

        LB_Sql.Text = strHQL;

        LB_PMCode.Text = "";
        DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        TB_ProjectName.Text = "";

        if (strIsMobileDevice == "YES")
        {
            HT_ProjectDetail.Text = "";
            HT_AcceptStandard.Text = "";
        }
        else
        {
            HE_ProjectDetail.Text = "";
            HE_AcceptStandard.Text = "";
        }

        LB_ProjectID.Text = "";

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectID, strStatus;

        strProjectID = LB_ProjectID.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        DL_StatusValue.SelectedValue = GetProjectStatusLatestValue(strProjectID, strStatus);
        HL_StatusReview.NavigateUrl = "TTProjectReviewWL.aspx?ProjectID=" + LB_ProjectID.Text.Trim() + "&ProjectStatus=" + GetProjectStatusIdentityString(strStatus);
    }

    protected void DL_StatusValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL, strWLID, strProjectID, strStatus, strStatusValues, strReviewControl;
        IList lst;

        strStatus = DL_Status.SelectedValue.Trim();
        strStatusValues = DL_StatusValue.SelectedValue.Trim();
        strProjectID = LB_ProjectID.Text.Trim();

        if (strProjectID != "")
        {
            strReviewControl = GetProjectStatusReviewControl(strStatus);

            if (strReviewControl == "YES")
            {
                if (strStatusValues == "Passed")
                {
                    strHQL = "from StatusRelatedWF as statusRelatedWF where statusRelatedWF.Status = " + "'" + strStatus + "'" + " and  statusRelatedWF.RelatedType = 'Project' and statusRelatedWF.RelatedID = " + strProjectID + " Order by statusRelatedWF.ID DESC";
                    StatusRelatedWFBLL statusRelatedWFBLL = new StatusRelatedWFBLL();
                    lst = statusRelatedWFBLL.GetAllStatusRelatedWFs(strHQL);
                    if (lst.Count > 0)
                    {
                        StatusRelatedWF statusRelatedWF = (StatusRelatedWF)lst[0];
                        strWLID = statusRelatedWF.WLID.ToString();

                        strHQL = "from WorkFlow as workFlow where workFlow.Status in ('Passed','CaseClosed') and workFlow.WLID = " + strWLID;
                        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
                        lst = workFlowBLL.GetAllWorkFlows(strHQL);

                        if (lst.Count == 0)
                        {
                            DL_StatusValue.SelectedValue = "InProgress";
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMYTJPSHPSMTGZTZBNGWTG").ToString().Trim() + "')", true);
                        }
                    }
                    else
                    {
                        DL_StatusValue.SelectedValue = "InProgress";
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMYTJPSHPSMTGZTZBNGWTG").ToString().Trim() + "')", true);
                    }
                }
            }
        }
        else
        {
            DL_StatusValue.SelectedValue = "InProgress";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCWKJLBNGBZTZXZXM").ToString().Trim() + "')", true);
        }
    }


    protected void BT_MyMember_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();

        ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_NewPMName);
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " Order By projectMember.SortNumber DESC";

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DL_NewPMName.DataSource = lst;
            DL_NewPMName.DataBind();
        }
    }

    protected void BT_TransferProject_Click(object sender, EventArgs e)
    {
        string strNewPMCode, strNewPMName, strOldPMCode, strOldPMName, strHQL;
        string strProjectID, strUserCode, strUserName, strProjectName;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = ShareClass.GetUserName(strUserCode);

        strNewPMCode = DL_NewPMName.SelectedValue.Trim();

        strProjectID = LB_ProjectID.Text.Trim();

        Msg msg = new Msg();

        if (strNewPMCode != "")
        {
            strNewPMName = ShareClass.GetUserName(strNewPMCode);

            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            Project project = new Project();
            project = (Project)lst[0];

            strProjectName = project.ProjectName.Trim();

            strOldPMCode = project.PMCode;
            strOldPMName = project.PMName;

            project.PMCode = strNewPMCode;
            project.PMName = strNewPMName;

            try
            {
                projectBLL.UpdateProject(project, int.Parse(strProjectID));

                TransferProjectBLL transferProjectBLL = new TransferProjectBLL();
                TransferProject transferProject = new TransferProject();

                transferProject.ProjectID = int.Parse(strProjectID);
                transferProject.OldPMCode = strOldPMCode;
                transferProject.OldPMName = strOldPMName;
                transferProject.NewPMCode = strNewPMCode;
                transferProject.NewPMName = strNewPMName;
                transferProject.ChangeTime = DateTime.Now;

                transferProjectBLL.AddTransferProject(transferProject);


                LB_PMCode.Text = strNewPMCode;
                LB_PMName.Text = strNewPMName;

                strHQL = "from Project as project where project.PMCode = " + "'" + strNewPMCode + "'" + " and project.UserCode = " + "'" + strUserCode + "'" + " and project.Status <> 'Archived' order by project.ProjectID DESC";
                lst = projectBLL.GetAllProjects(strHQL);
                DataGrid2.DataSource = lst;
                DataGrid2.DataBind();

                LB_Sql.Text = strHQL;
                LB_ProjectList.Text = strNewPMCode + strNewPMName + LanguageHandle.GetWord("DeXiangMuLieBiao").ToString().Trim();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZXCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZZhuaiXiangLanguageHandleGetW").ToString().Trim() + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true); 
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXMJLDDMBNWKJC").ToString().Trim() + "')", true);
        }
    }

    protected void AddStatusChangeRecord(string strProjectID, string strOldStatus, string strNewStatus, string strOldStatusValue, string strNewStatusValue)
    {
        string strUserCode, strUserName;

        if ((strOldStatus != strNewStatus) | (strOldStatusValue != strNewStatusValue))
        {
            strUserCode = LB_UserCode.Text.Trim();
            strUserName = ShareClass.GetUserName(strUserCode);

            ProStatusChangeBLL proStatusChangeBLL = new ProStatusChangeBLL();
            ProStatusChange proStatusChange = new ProStatusChange();

            proStatusChange.ProjectID = int.Parse(strProjectID);
            proStatusChange.UserCode = strUserCode;
            proStatusChange.UserName = strUserName;
            proStatusChange.OldStatus = strOldStatus;
            proStatusChange.NewStatus = strNewStatus;
            proStatusChange.OldStatusValue = strOldStatusValue;
            proStatusChange.NewStatusValue = strNewStatusValue;
            proStatusChange.ChangeTime = DateTime.Now;

            try
            {
                proStatusChangeBLL.AddProStatusChange(proStatusChange);
            }
            catch
            {
            }
        }
    }

    protected void LoadProjectStatus(string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectStatus as projectStatus where projectStatus.ProjectType = " + "'" + strType + "'" + " Order by projectStatus.SortNumber ASC";
        ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
        lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

        if (lst.Count == 0)
        {
            strHQL = "insert into T_ProjectStatus(Status,SortNumber,ReviewControl,IdentityString,ProjectType)";
            strHQL += " select Status,SortNumber,ReviewControl,IdentityString," + "'" + strType + "'";
            strHQL += " from T_ProjectStatus where ProjectType = 'OtherProject'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "from ProjectStatus as projectStatus where projectStatus.ProjectType = " + "'" + strType + "'" + " Order by projectStatus.SortNumber ASC";
            lst = projectStatusBLL.GetAllProjectStatuss(strHQL);
        }

        DL_Status.DataSource = lst;
        DL_Status.DataBind();
    }

    protected void LoadProject(string strPMCode, string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.PMCode = " + "'" + strPMCode + "'" + " and project.UserCode = " + "'" + strUserCode + "'" + " and project.Status <> 'Archived' order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


    protected string GetProjectStatusReviewControl(string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectStatus as projectStatus where projectStatus.Status = " + "'" + strStatus + "'";
        ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
        lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

        ProjectStatus projectStatus = (ProjectStatus)lst[0];

        return projectStatus.ReviewControl.Trim();
    }

    protected string GetProjectStatusLatestValue(string strProjectID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = " from ProStatusChange as proStatusChange where proStatusChange.ProjectID = " + strProjectID;
        strHQL += " and proStatusChange.NewStatus = " + "'" + strStatus + "'";
        strHQL += " Order by proStatusChange.ChangeTime DESC";
        ProStatusChangeBLL proStatusChangeBLL = new ProStatusChangeBLL();
        lst = proStatusChangeBLL.GetAllProStatusChanges(strHQL);

        if (lst.Count > 0)
        {
            ProStatusChange proStatusChange = (ProStatusChange)lst[0];
            return proStatusChange.NewStatusValue.Trim();
        }
        else
        {
            return "InProgress";
        }
    }

    protected string GetProjectStatusIdentityString(string strStatus)
    {
        string strHQL;
        IList lst;

        try
        {
            strHQL = "from ProjectStatus as projectStatus where projectStatus.Status = " + "'" + strStatus + "'";
            ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
            lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

            ProjectStatus projectStatus = (ProjectStatus)lst[0];

            return projectStatus.IdentityString.Trim();
        }
        catch
        {
            return "";
        }
    }

}
