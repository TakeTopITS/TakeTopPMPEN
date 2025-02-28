using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class TTProjectDetail : System.Web.UI.Page
{
    string strIsMobileDevice;
    string strProjectType;

    string strLangCode;
    string strUserCode, strUserName, strUserType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectID, strProjectName, strPMUserCode, strStatus;
        string strImpactByDetail;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strUserType = ShareClass.GetUserType(strUserCode);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化      
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_TodaySummary);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strProjectID = Request.QueryString["ProjectID"];
        LB_ProjectID.Text = strProjectID;

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTProjectDetailSAAS.aspx?ProjectID=" + strProjectID);
        }

        //检查用户是否项目经理
        if (ShareClass.CheckUserIsProjectManager(strProjectID, strUserCode) == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        Project project = ShareClass.GetProject(strProjectID);
        strProjectName = project.ProjectName.Trim();
        strProjectType = project.ProjectType.Trim();
        strPMUserCode = project.PMCode.Trim();//

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_TodaySummary.Visible = true;
            }
            else
            {
                HE_TodaySummary.Visible = true;
            }

            //如果项目进度受细节影响，则直接取得
            strImpactByDetail = ShareClass.GetProjectTypeImpactByDetail(strProjectID);
            if (strImpactByDetail == "YES")
            {
                NB_FinishPercent.Enabled = false;
                NB_ManHour.Enabled = false;
            }

            //依项目类型添加关联的工作流模板和文档模板
            ShareClass.AddRelatedWorkFlowTemplateByProjectType(strProjectType, strProjectID, "Project", "Project", "ProjectType");

            HL_ProjectDetailView.NavigateUrl = "TTProjectDetailView.aspx?ProjectID=" + strProjectID;
          
            HL_BusinessForm.NavigateUrl = "TTRelatedDIYBusinessForm.aspx?RelatedType=Project&RelatedID=" + strProjectID + "&IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(ShareClass.getBusinessFormTemName("Project", strProjectID));
            //BusinessForm，如果不含业务表单，就隐藏“相关表单按钮”
            if (ShareClass.getRelatedBusinessFormTemName("Project", strProjectID) == "")
            {
                HL_BusinessForm.Visible = false;
            }

            strHQL = "from ProjectStatus as projectStatus";
            strHQL += " Where projectStatus.ProjectType = " + "'" + strProjectType + "'";
            strHQL += " And projectStatus.LangCode =" + "'" + strLangCode + "'";
            strHQL += " Order by projectStatus.SortNumber ASC";
            ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
            lst = projectStatusBLL.GetAllProjectStatuss(strHQL);
            DL_Status.DataSource = lst;
            DL_Status.DataBind();
            DL_Status.SelectedValue = project.Status;
            DL_StatusValue.SelectedValue = project.StatusValue.Trim();

            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            DataList1.DataSource = lst;
            DataList1.DataBind();

            LB_Status.Text = project.Status.Trim();
            LB_CreatorCode.Text = project.UserCode.Trim();//项目创建者

            if (ShareClass.GetProjectAllowPMChangeStatus(strProjectID) == "YES")
            {
                DL_Status.Enabled = true;
                DL_StatusValue.Enabled = true;
            }
            else
            {
                DL_Status.Enabled = false;
                DL_StatusValue.Enabled = false;
            }

            strHQL = "from DailyWork as dailyWork where dailyWork.ProjectID = " + strProjectID + " and " + " dailyWork.UserCode = " + "'" + strUserCode + "'" + " and " + "to_char(dailyWork.WorkDate,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'";
            DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
            lst = dailyWorkBLL.GetAllDailyWorks(strHQL);

            if (lst.Count > 0)
            {
                DailyWork dailyWork = (DailyWork)lst[0];
                if (strIsMobileDevice == "NO")
                {
                    HE_TodaySummary.Visible = true;
                    HE_TodaySummary.Text = dailyWork.DailySummary;
                }
                else
                {
                    HT_TodaySummary.Visible = true;
                    HT_TodaySummary.Text = dailyWork.DailySummary;
                }

                LB_WorkID.Text = dailyWork.WorkID.ToString();
                TB_Charge.Amount = dailyWork.Charge;
                TB_WorkAddress.Text = dailyWork.Address;
                TB_Achievement.Text = dailyWork.Achievement;

                //如果项目进度受细节影响，则直接取得
                if (strImpactByDetail == "YES")
                {
                    NB_FinishPercent.Amount = decimal.Parse(ShareClass.getCurrentDateTotalProgressForMember(strProjectID, strUserCode));
                    NB_ManHour.Amount = decimal.Parse(ShareClass.getCurrentDateTotalManHourByOneOperator(strProjectID, strUserCode, DateTime.Now.ToString("yyyyMMdd")));
                }
                else
                {
                    NB_FinishPercent.Amount = dailyWork.FinishPercent;
                    NB_ManHour.Amount = dailyWork.ManHour;
                }

                try
                {
                    DL_Authority.SelectedValue = dailyWork.Authority.Trim();
                }
                catch
                {
                }
            }

            //当天任务分派记录汇总
            HL_CurrentDailyWorkTask.NavigateUrl = "TTDailyWorkTaskView.aspx?ProjectID=" + strProjectID + "&UserCode=" + strUserCode + "&WorkDate=" + DateTime.Now;

            strStatus = ShareClass.GetProjectStatus(strProjectID);
            if (strStatus == "CaseClosed" || strStatus == "Suspended" || strStatus == "Cancel")
            {
                BT_Summit.Enabled = false;
                BT_Activity.Enabled = false;
                BT_Receive.Enabled = false;
                BT_Refuse.Enabled = false;
            }
            else
            {
                DataSet ds;
                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'PrincipalProjectFirstLine' and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater1.DataSource = ds;
                Repeater1.DataBind();

                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'PrincipalProjectSecondLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater2.DataSource = ds;
                Repeater2.DataBind();

                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'PrincipalProjectThirdLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater3.DataSource = ds;
                Repeater3.DataBind();

                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'PrincipalProjectFourthLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";   
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater4.DataSource = ds;
                Repeater4.DataBind();

                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'PrincipalProjectFifthLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater5.DataSource = ds;
                Repeater5.DataBind();
            }
        }
    }

    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strBTText;
        string strLBWorkID;
        string strUserCode = LB_UserCode.Text.Trim();
        string strProjectID = LB_ProjectID.Text.Trim();
        decimal deManHour = 0;
        decimal deUnitHourSalary = 0, deFinishPercent = 0, deBonus = 0;

        string strTodaySummary;

        if (strIsMobileDevice == "YES")
        {
            strTodaySummary = HT_TodaySummary.Text.Trim();
        }
        else
        {
            strTodaySummary = HE_TodaySummary.Text.Trim();
        }

        if (strTodaySummary == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJNRBNWKJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            //如果项目进度受细节影响，则直接取得
            if (ShareClass.GetProjectTypeImpactByDetail(strProjectID) == "YES")
            {
                NB_FinishPercent.Amount = decimal.Parse(ShareClass.getCurrentDateTotalProgressForMember(strProjectID, strUserCode));
                NB_ManHour.Amount = decimal.Parse(ShareClass.getCurrentDateTotalManHourByOneOperator(strProjectID, strUserCode, DateTime.Now.ToString("yyyyMMdd")));
            }

            if (NB_FinishPercent.Amount > 100)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCBFBBNCG100JC").ToString().Trim() + "')", true);
            }
            else
            {
                DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
                DailyWork dailyWork = new DailyWork();

                strBTText = BT_Summit.Text.Trim();
                strLBWorkID = LB_WorkID.Text.Trim();
                deUnitHourSalary = GetUnitHourSalary(strUserCode, strProjectID);
                deFinishPercent = NB_FinishPercent.Amount;
                deManHour = NB_ManHour.Amount;

                if (strLBWorkID == "-1")
                {
                    dailyWork.Type = "Lead";   
                    dailyWork.UserCode = strUserCode;
                    dailyWork.UserName = ShareClass.GetUserName(strUserCode);
                    dailyWork.WorkDate = DateTime.Now;
                    dailyWork.RecordTime = DateTime.Now;
                    dailyWork.Address = TB_WorkAddress.Text.Trim();
                    dailyWork.ProjectID = int.Parse(strProjectID);
                    dailyWork.ProjectName = ShareClass.GetProjectName(strProjectID);
                    dailyWork.DailySummary = strTodaySummary;
                    dailyWork.Achievement = TB_Achievement.Text;
                    dailyWork.Charge = 0;
                    dailyWork.ManHour = deManHour;
                    dailyWork.ConfirmManHour = deManHour;
                    dailyWork.Salary = deManHour * deUnitHourSalary;
                    dailyWork.FinishPercent = deFinishPercent;

                    deBonus = ShareClass.GetDailyWorkLogLength(dailyWork.DailySummary) * ShareClass.GetEveryCharPrice() + ShareClass.GetDailyUploadDocNumber(strUserCode, strProjectID) * ShareClass.GetEveryDocPrice();

                    dailyWork.Bonus = deBonus;
                    dailyWork.ConfirmBonus = deBonus;
                    dailyWork.Authority = DL_Authority.SelectedValue.Trim();

                    try
                    {
                        dailyWorkBLL.AddDailyWork(dailyWork);

                        ShareClass.UpdateProjectCompleteDegree(strProjectID, deFinishPercent);

                        //取得提交的WorkID
                        strHQL = "from DailyWork as dailyWork where dailyWork.Type = 'Lead' and dailyWork.ProjectID =" + strProjectID + " and " + " dailyWork.UserCode = " + "'" + strUserCode + "'" + " and " + "to_char(dailyWork.WorkDate,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'";   
                        lst = dailyWorkBLL.GetAllDailyWorks(strHQL);
                        dailyWork = (DailyWork)lst[0];
                        LB_WorkID.Text = dailyWork.WorkID.ToString();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCCJC").ToString().Trim() + "')", true);
                    }
                }
                else
                {
                    strHQL = "from DailyWork as dailyWork where dailyWork.WorkID = " + LB_WorkID.Text;

                    lst = dailyWorkBLL.GetAllDailyWorks(strHQL);
                    dailyWork = (DailyWork)lst[0];

                    strProjectID = dailyWork.ProjectID.ToString();
                    deFinishPercent = NB_FinishPercent.Amount;

                    dailyWork.RecordTime = DateTime.Now;
                    dailyWork.Address = TB_WorkAddress.Text.Trim();
                    dailyWork.FinishPercent = deFinishPercent;
                    dailyWork.DailySummary = strTodaySummary;
                    dailyWork.Achievement = TB_Achievement.Text;
                    dailyWork.ManHour = deManHour;
                    dailyWork.ConfirmManHour = deManHour;

                    dailyWork.Salary = deManHour * deUnitHourSalary;

                    deBonus = ShareClass.GetDailyWorkLogLength(dailyWork.DailySummary) * ShareClass.GetEveryCharPrice() + ShareClass.GetDailyUploadDocNumber(strUserCode, strProjectID) * ShareClass.GetEveryDocPrice();

                    dailyWork.Bonus = deBonus;
                    dailyWork.ConfirmBonus = deBonus;

                    dailyWork.Authority = DL_Authority.SelectedValue.Trim();

                    try
                    {
                        dailyWorkBLL.UpdateDailyWork(dailyWork, int.Parse(LB_WorkID.Text));

                        ShareClass.UpdateProjectCompleteDegree(strProjectID, deFinishPercent);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);

                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSBJCZ").ToString().Trim() + "')", true);
                    }
                }
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZDiJiaoShiBaiBaiFenBiBuNengWe").ToString().Trim()+"')", true);
        }
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strProjectID, strOldStatus, strOldStatusValue, strNewStatus, strNewStatusValue;

        strProjectID = LB_ProjectID.Text.Trim();

        Project project = ShareClass.GetProject(strProjectID);
        strOldStatus = project.Status.Trim();
        strOldStatusValue = project.StatusValue.Trim();

        strNewStatus = DL_Status.SelectedValue.Trim();

        strNewStatusValue = GetProjectStatusLatestValue(strProjectID, strNewStatus);
        DL_StatusValue.SelectedValue = strNewStatusValue;

        try
        {
            strHQL = "Update T_Project Set Status = " + "'" + strNewStatus + "'" + ",StatusValue = " + "'" + strNewStatusValue + "'" + " Where ProjectID = " + strProjectID;
            ShareClass.RunSqlCommand(strHQL);

            AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZTGBCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZTGBSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_StatusValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strWLID, strProjectID, strOldStatus, strOldStatusValue, strNewStatus, strNewStatusValue, strReviewControl;

        strProjectID = LB_ProjectID.Text.Trim();

        Project project = ShareClass.GetProject(strProjectID);
        strOldStatus = project.Status.Trim();
        strOldStatusValue = project.StatusValue.Trim();

        strNewStatus = DL_Status.SelectedValue.Trim();
        strNewStatusValue = DL_StatusValue.SelectedValue.Trim();

        strReviewControl = GetProjectStatusReviewControl(strProjectType, strNewStatus);

        if (strReviewControl == "YES")
        {
            if (strNewStatusValue == "Passed")
            {
                strHQL = "from StatusRelatedWF as statusRelatedWF where statusRelatedWF.Status = " + "'" + strNewStatus + "'" + " and  statusRelatedWF.RelatedType = 'Project' and statusRelatedWF.RelatedID = " + strProjectID + " Order by statusRelatedWF.ID DESC";
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

        strNewStatusValue = DL_StatusValue.SelectedValue.Trim();
        try
        {
            strHQL = "Update T_Project Set StatusValue = " + "'" + strNewStatusValue + "'" + " Where ProjectID = " + strProjectID;
            ShareClass.RunSqlCommand(strHQL);

            AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZTZGBCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZTZGBSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void BT_Receive_Click(object sender, EventArgs e)
    {
        string strProjectID = LB_ProjectID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strUserName = ShareClass.GetUserName(strUserCode);
        string strOldStatus, strOldStatusValue;
        string strNewStatus = "Accepted";
        string strNewStatusValue = "Passed";
        string strProjectName, strStatus;

        ProjectBLL projectBLL = new ProjectBLL();

        string strHQL = "from Project as project where project = " + strProjectID;
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strOldStatus = project.Status.Trim();
        strOldStatusValue = project.StatusValue.Trim();

        strStatus = project.Status.Trim();

        //推送实施路线
        HL_ImpleRoute.NavigateUrl = "TTSelectorPlan.aspx?projectID=" + strProjectID;

        if (strStatus == "New" | strStatus == "Review" | strStatus == "Plan" | strStatus == "Rejected")
        {
            strProjectName = project.ProjectName.Trim();

            project.Status = "Accepted";
            project.StatusValue = "Passed";

            try
            {
                projectBLL.UpdateProject(project, int.Parse(strProjectID));

                AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue, strUserCode);
                LB_Status.Text = strNewStatus;

                TB_Message.Text = strUserName + LanguageHandle.GetWord("ShouLiLeNiJianLiDeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName;

                LoadProject(strProjectID);


                if (System.Configuration.ConfigurationManager.AppSettings["SystemName"] == "SIMP")
                {
                    //弹出推送实施路线
                    HL_ImpleRoute.NavigateUrl = "TTSelectorPlan.aspx?projectID=" + strProjectID;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTSelectorPlan.aspx?projectID=" + strProjectID + "');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZNYSLGBNZFSL").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Refuse_Click(object sender, EventArgs e)
    {
        string strProjectID = LB_ProjectID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strUserName = ShareClass.GetUserName(strUserCode);
        string strOldStatus, strOldStatusValue;
        string strNewStatus = "Rejected";
        string strNewStatusValue = "Passed";
        string strProjectName, strStatus;
        string strHQL;
        IList lst;


        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strOldStatus = project.Status.Trim();
        strOldStatusValue = project.StatusValue.Trim();

        strStatus = project.Status.Trim();

        if (strStatus == "New" | strStatus == "Review" | strStatus == "Plan" | strStatus == "Accepted")
        {
            strProjectName = project.ProjectName.Trim();

            project.Status = "Rejected";
            project.StatusValue = "Passed";

            try
            {
                projectBLL.UpdateProject(project, int.Parse(strProjectID));

                AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue, strUserCode);
                LB_Status.Text = strNewStatus;

                TB_Message.Text = strUserName + LanguageHandle.GetWord("JuJueLeNiJianLiDeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName;
                LoadProject(strProjectID);
            }
            catch
            {

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJJSBJCZ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXMYZJXZBNJJL").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Activity_Click(object sender, EventArgs e)
    {
        string strProjectID = LB_ProjectID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strUserName = ShareClass.GetUserName(strUserCode);
        string strOldStatus, strOldStatusValue;
        string strNewStatus = "InProgress";
        string strNewStatusValue = "Passed";
        string strProjectName, strStatus;

        string strHQL;
        IList lst;

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strOldStatus = project.Status.Trim();
        strOldStatusValue = project.StatusValue.Trim();

        strStatus = project.Status.Trim();

        if (strStatus == "New" | strStatus == "Review" | strStatus == "Plan" | strStatus == "Rejected" | strStatus == "Accepted")
        {
            strProjectName = project.ProjectName.Trim();

            project.Status = "InProgress";
            project.StatusValue = "Passed";

            try
            {
                projectBLL.UpdateProject(project, int.Parse(strProjectID));

                AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue, strUserCode);
                LB_Status.Text = strNewStatus;

                TB_Message.Text = strUserName + LanguageHandle.GetWord("KaiShiChuLiNiJianLiDeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName;

                LoadProject(strProjectID);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHDSBJCZ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXMYZHDJXZBYJHL").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;
        string strUserCode = LB_UserCode.Text.Trim();
        string strCreatorCode = LB_CreatorCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_ReturnMsg.Checked == true | CB_ReturnMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("XiangMuChuLiQingKuangFanKui").ToString().Trim();
            strMsg = TB_Message.Text.Trim();

            if (CB_ReturnMsg.Checked == true)
            {
                msg.SendMSM("Message", strCreatorCode, strMsg, strUserCode);
            }

            if (CB_ReturnMail.Checked == true)
            {
                msg.SendMail(strCreatorCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB").ToString().Trim() + "')", true);
    }

    protected void AddStatusChangeRecord(string strProjectID, string strOldStatus, string strNewStatus, string strOldStatusValue, string strNewStatusValue, string strUserCode)
    {
        string strUserName;

        if ((strOldStatus != strNewStatus) | (strOldStatusValue != strNewStatusValue))
        {
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

    protected string GetProjectStatusReviewControl(string strProjectType, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectStatus as projectStatus where projectType = " + "'" + strProjectType + "'" + " and projectStatus.Status = " + "'" + strStatus + "'";
        ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
        lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

        ProjectStatus projectStatus = (ProjectStatus)lst[0];

        return projectStatus.ReviewControl.Trim();
    }

    /// <summary>
    /// 根据用户编码，角色名称，项目编号，判断该用户是否存在  By LiuJianping 2013-10-12
    /// </summary>
    /// <param name="strusercode">用户编码</param>
    /// <param name="strgroupname">角色名称</param>
    /// <param name="strprojectid">项目编号</param>
    /// <returns></returns>
    protected bool ISActorGroupByUserCode(string strusercode, string strgroupname, string strprojectid)
    {
        bool flag = true;
        string strHQL = "from RelatedUser as relatedUser where relatedUser.UserCode = '" + strusercode + "' and relatedUser.Actor = '" + strgroupname + "' and relatedUser.ProjectID='" + strprojectid + "' ";
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;

        return flag;
    }

    protected string GetProjectStatus(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.Status.Trim();
    }

    protected void LoadProject(string strProjectID)
    {
        string strHQL = "from Project as project where project = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();
    }

    protected decimal GetUnitHourSalary(string strUserCode, string strProjectID)
    {
        decimal deUnitHourSalary;
        string strHQL;
        IList lst;

        strHQL = "from RelatedUser as relatedUser where relatedUser.UserCode = " + "'" + strUserCode + "'" + " and relatedUser.ProjectID = " + strProjectID;
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

        if (lst.Count > 0)
        {
            RelatedUser relatedUser = (RelatedUser)lst[0];
            deUnitHourSalary = relatedUser.UnitHourSalary;
        }
        else
        {
            deUnitHourSalary = 0;
        }

        return deUnitHourSalary;
    }

}
