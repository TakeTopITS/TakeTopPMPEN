using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTCreatedProjectDetail : System.Web.UI.Page
{
    string strProjectID, strProjectName;
    string strIsMobileDevice;
    string strLangCode;
    string strUserCode, strUserName, strUserType, strProjectType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName;

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        strUserType = Session["UserType"].ToString();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strLangCode = Session["LangCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_AcceptStandard);
        _FileBrowser.SetupCKEditor(HE_ProjectDetail);

        strProjectID = Request.QueryString["ProjectID"];
        strProjectName = ShareClass.GetProjectName(strProjectID);

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTCreatedProjectDetailSAAS.aspx?ProjectID=" + strProjectID);
        }

        //检查用户是否项目成员
        if (ShareClass.CheckUserIsProjectCreator(strProjectID, strUserCode) == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            try
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

                LB_PMCode.Enabled = false;

                string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityProjectLeader(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
                ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_NewPMName);

                ShareClass.LoadProjectType(DL_ProjectType);
                ShareClass.LoadCurrencyType(DL_CurrencyType);

                //列出可用的工作流模板
                ShareClass.LoadProjectPlanStartupRelatedWorkflowTemplate(strUserCode, DL_PlanStartupRelatedWorkflowTemplate);

                LoadProjectDetail(strProjectID);

                try
                {
                    DataSet ds;
                    strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'BuildProjectFirstLine' and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                    Repeater1.DataSource = ds;
                    Repeater1.DataBind();

                    strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'BuildProjectSecondLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                    Repeater2.DataSource = ds;
                    Repeater2.DataBind();

                    if(strProjectID == "1")
                    {
                        DL_Status.Enabled = false;
                        DL_StatusValue.Enabled = false;
                    }

                    //依项目类型添加关联的工作流模板和文档模板
                    ShareClass.AddRelatedWorkFlowTemplateByProjectType(strProjectType, strProjectID, "Project", "Project", "ProjectType");

                    HL_BusinessForm.NavigateUrl = "TTRelatedDIYBusinessForm.aspx?RelatedType=Project&RelatedID=" + strProjectID + "&IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(ShareClass.getBusinessFormTemName("Project", strProjectID));
                    //BusinessForm，如果不含业务表单，就隐藏“相关表单按钮”
                    if (ShareClass.getRelatedBusinessFormTemName("Project", strProjectID) == "")
                    {
                        HL_BusinessForm.Visible = false;
                    }


                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strUserCode, strCustomerPMName, strUserName, strStatus, strStatusValue, strBeginDate;
        string strEndDate, strProject, strDetail, strAcceptStandard, strParentID;
        string strBudget;
        string strHQL;
        string strProjectID;
        string strProjectType;
        IList lst;
        string strPMCode;
        decimal deProjectAmount, deManHour, deManNumber;
        string strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue;

        try
        {

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

                project.ProjectCode = TB_ProjectCode.Text.Trim();
                project.UserCode = strUserCode;
                project.UserName = ShareClass.GetUserName(strUserCode);
                project.PMCode = strPMCode;
                project.PMName = ShareClass.GetUserName(strPMCode);
                project.CustomerPMName = strCustomerPMName;
                project.ProjectName = strProject;
                project.ProjectType = strProjectType;
                project.Budget = decimal.Parse(strBudget);
                project.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
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

                project.ParentID = int.Parse(strParentID);

                if (strProjectID == "")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXMBNXGJC").ToString().Trim() + "')", true);
                }
                else
                {
                    projectBLL.UpdateProject(project, int.Parse(strProjectID));

                    AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue);
                    LB_Status.Text = strNewStatus;

                    //保存项目的其它属性
                    UpdateProjectOtherFieldValue(strProjectID);

                    TB_Message.Text = strUserName + LanguageHandle.GetWord("GengXinLeXiangMu").ToString().Trim() + strProjectID + " " + strProject + LanguageHandle.GetWord("DeNeiRongQingGuanZhuTeCiTongZh").ToString().Trim();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
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

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strProjectID, strProjectName, strHQL;
        string strPMCode, strUserCode, strUserName; ;
        IList lst;

        strProjectID = LB_ProjectID.Text.Trim();

        strUserCode = LB_UserCode.Text;
        strUserName = LB_UserName.Text;
        strPMCode = LB_PMCode.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        project.Status = "Deleted";

        strProjectName = project.ProjectName.Trim();

        try
        {
            projectBLL.UpdateProject(project, int.Parse(strProjectID));


            TB_Message.Text = strUserName + LanguageHandle.GetWord("ShanChuLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("DeNeiRongQingGuanZhuTeCiTongZh").ToString().Trim();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_ProjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType;

        try
        {
            strType = DL_ProjectType.SelectedValue.Trim();
            ShareClass.LoadProjectForPMStatus(strType,strLangCode,DL_Status);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
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
                        msg.SendMSM("Message", strRelatedUserCode, strMsg, strUserCode);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXFSSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectID, strStatus;

        try
        {
            strProjectID = LB_ProjectID.Text.Trim();
            strStatus = DL_Status.SelectedValue.Trim();

            DL_StatusValue.SelectedValue = GetProjectStatusLatestValue(strProjectID, strStatus);
            HL_StatusReview.NavigateUrl = "TTProjectReviewWL.aspx?ProjectID=" + LB_ProjectID.Text.Trim() + "&ProjectStatus=" + GetProjectStatusIdentityString(strStatus);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void DL_StatusValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL, strWLID, strProjectID, strProjectType, strStatus, strStatusValues, strReviewControl;
        IList lst;

        try
        {
            strProjectType = DL_ProjectType.SelectedValue.Trim();
            strStatus = DL_Status.SelectedValue.Trim();
            strStatusValues = DL_StatusValue.SelectedValue.Trim();
            strProjectID = LB_ProjectID.Text.Trim();

            if (strProjectID != "")
            {
                strReviewControl = GetProjectStatusReviewControl(strProjectType, strStatus);

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
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void BT_MyMember_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();

        ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_NewPMName);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

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
        string strActor, strNewPMCode, strNewPMName, strOldPMCode, strOldPMName;
        string strProjectID, strUserCode, strUserName, strProjectName;
        string strHQL;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = ShareClass.GetUserName(strUserCode);

        strActor = DL_Aactor.SelectedValue.Trim();
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

            if (strActor == "ProjectManager")
            {
                strOldPMCode = project.PMCode;
                strOldPMName = project.PMName;

                project.PMCode = strNewPMCode;
                project.PMName = strNewPMName;

                ShareClass.AddProjectMember(strProjectID, strNewPMCode, "项目经理", "项目经理", DL_Status.SelectedValue.Trim()); 
            }
            else
            {
                strOldPMCode = project.UserCode;
                strOldPMName = project.UserName;

                project.UserCode = strNewPMCode;
                project.UserName = strNewPMName;

                ShareClass.AddProjectMember(strProjectID, strNewPMCode, "立项者", "立项者", DL_Status.SelectedValue.Trim()); 
            }

            try
            {
                projectBLL.UpdateProject(project, int.Parse(strProjectID));

                TransferProjectBLL transferProjectBLL = new TransferProjectBLL();
                TransferProject transferProject = new TransferProject();

                transferProject.ProjectID = int.Parse(strProjectID);
                transferProject.Actor = strActor;
                transferProject.OldPMCode = strOldPMCode;
                transferProject.OldPMName = strOldPMName;
                transferProject.NewPMCode = strNewPMCode;
                transferProject.NewPMName = strNewPMName;
                transferProject.ChangeTime = DateTime.Now;

                transferProjectBLL.AddTransferProject(transferProject);

                LB_PMCode.Text = strNewPMCode;
                LB_PMName.Text = strNewPMName;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMZSCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZSSBJC").ToString().Trim() + "')", true);
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

    protected void LoadProjectDetail(string strProjectID)
    {
        string strProjectName, strHQL, strStatus;
        string strUserName;
        IList lst;

        try
        {
            ProjectBLL projectBLL = new ProjectBLL();
            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            lst = projectBLL.GetAllProjects(strHQL);
            Project project = (Project)lst[0];

            strUserName = LB_UserName.Text.Trim();
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
            try
            {
                DL_ProjectType.SelectedValue = project.ProjectType.Trim();
                ShareClass.LoadProjectForPMStatus(project.ProjectType.Trim(), strLangCode, DL_Status);
            }
            catch
            {
            }
            try
            {
                DL_Status.SelectedValue = project.Status;
                DL_StatusValue.SelectedValue = project.StatusValue.Trim();
            }
            catch
            {
            }
            try
            {
                DL_Status.SelectedValue = project.Status.Trim();
                DL_StatusValue.SelectedValue = project.StatusValue.Trim();
            }
            catch
            {
            }
            strStatus = project.Status.Trim();

            DL_CurrencyType.SelectedValue = project.CurrencyType;
            TB_ProjectCode.Text = project.ProjectCode.Trim();
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
                HT_ProjectDetail.Text = project.ProjectDetail;
                HT_AcceptStandard.Text = project.AcceptStandard;
            }
            else
            {
                HE_ProjectDetail.Text = project.ProjectDetail;
                HE_AcceptStandard.Text = project.AcceptStandard;
            }

            NB_Budget.Amount = project.Budget;
            DLC_BeginDate.Text = project.BeginDate.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = project.EndDate.ToString("yyyy-MM-dd");
            NB_ProjectAmount.Amount = project.ProjectAmount;
            NB_ManHour.Amount = project.ManHour;
            NB_ManNubmer.Amount = project.ManNumber;

            HL_ProjectBudget.NavigateUrl = "TTMakeProjectBudget.aspx?ProjectID=" + strProjectID;

            HL_StatusReview.Enabled = true;
            HL_StatusReview.NavigateUrl = "TTProjectReviewWL.aspx?ProjectID=" + strProjectID + "&Type=Status&ProjectStatus=" + GetProjectStatusIdentityString(strStatus);

            //设置项目其它属性的值 
            SetProjectOtherFieldValue(strProjectID);

            //会出错，这个错误是防止修改总项目
            try
            {
                LB_ParentProjectID.Text = project.ParentID.ToString();
                TB_ParentProject.Text = ShareClass.GetProjectName(project.ParentID.ToString());
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZXMBNXGJC").ToString().Trim() + "')", true);
                return;
            }

            BT_Upate.Enabled = true;
            BT_Delete.Enabled = true;

            BT_TransferProject.Enabled = true;
            CB_SMS.Enabled = true;
            CB_Mail.Enabled = true;
            BT_Send.Enabled = true;

            TB_Message.Text = strUserName + LanguageHandle.GetWord("GeiNiJianLiLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

   

    protected string GetProjectStatusReviewControl(string strProjectType, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectStatus as projectStatus where projectStatus.ProjectType = " + "'" + strProjectType + "'" + " and projectStatus.Status = " + "'" + strStatus + "'";
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

        strHQL = "from ProjectStatus as projectStatus where projectStatus.Status = " + "'" + strStatus + "'";
        ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
        lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

        ProjectStatus projectStatus = (ProjectStatus)lst[0];

        return projectStatus.IdentityString.Trim();
    }

}
