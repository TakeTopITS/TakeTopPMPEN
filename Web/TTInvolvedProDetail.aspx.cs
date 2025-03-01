using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;

public partial class TTInvolvedProDetail : System.Web.UI.Page
{
    string strIsMobileDevice;
    string strLangCode;

    string strProjectType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strStatus1, strStatus2;
        string strImpactByDetail;

        strLangCode = Session["LangCode"].ToString();
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = ShareClass.GetUserName(strUserCode);
        string strUserType = ShareClass.GetUserType(strUserCode);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        string strProjectID = Request.QueryString["ProjectID"];
        if(strProjectID == null)
        {
            string strTaskID = Request.QueryString["TaskID"];
            strProjectID = GetProjectIDByTaskID(strTaskID);
        }
        LB_ProjectID.Text = strProjectID;

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTInvolvedProDetailSAAS.aspx?ProjectID=" + strProjectID);
        }

        //检查用户是否项目成员
        if (ShareClass.CheckUserIsProjectMember(strProjectID, strUserCode) == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        Project project = (Project)lst[0];
        //2013-10-14 By LiuJianping "+LanguageHandle.GetWord("XiangMuJingLi")+"
        strProjectType = project.ProjectType.Trim();
        string strPMUserCode = project.PMCode.Trim();//end
        LB_PMCode.Text = strPMUserCode;

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_TodaySummary);

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

            string strPMCode = ShareClass.GetProjectPMCode(strProjectID);
            if (strUserCode == strPMCode)
            {
                Response.Redirect("TTProjectDetail.aspx?ProjectID=" + strProjectID);
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

            strHQL = "from DailyWork as dailyWork where dailyWork.Type = 'Participate' and dailyWork.ProjectID =" + strProjectID + " and " + " dailyWork.UserCode = " + "'" + strUserCode + "'" + " and " + "to_char(dailyWork.WorkDate,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'";  
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

                LB_WorkID.Text = dailyWork.WorkID.ToString();
                TB_WorkAddress.Text = dailyWork.Address;
                TB_Charge.Amount = dailyWork.Charge;
                TB_Achievement.Text = dailyWork.Achievement;

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

            strHQL = "from RelatedUser as relatedUser where relatedUser.UserCode = " + "'" + strUserCode + "'" + " and relatedUser.ProjectID = " + strProjectID;
            strHQL += " and relatedUser.ID in (Select max(relatedUser.ID) From RelatedUser as relatedUser where relatedUser.UserCode = " + "'" + strUserCode + "'" + " and relatedUser.ProjectID = " + strProjectID + ")";
            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
            DataList2.DataSource = lst;
            DataList2.DataBind();

            RelatedUser relatedUser = (RelatedUser)lst[0];

            strStatus1 = ShareClass.GetProjectStatus(strProjectID);
            strStatus2 = relatedUser.Status.Trim();

            if (strStatus1 == "CaseClosed" || strStatus1 == "Suspended" || strStatus1 == "Cancel" || strStatus2 == "Pause" || strStatus2 == "Stop")
            {
                BT_Summit.Enabled = false;
                BT_Activity.Enabled = false;
                BT_Receive.Enabled = false;
                BT_Refuse.Enabled = false;
            }
            else
            {
                DataSet ds;
                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'ParticipateProjectFirstLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater1.DataSource = ds;
                Repeater1.DataBind();

                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'ParticipateProjectSecondLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";   
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater2.DataSource = ds;
                Repeater2.DataBind();

                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'ParticipateProjectThirdLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater3.DataSource = ds;
                Repeater3.DataBind();

                strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'ParticipateProjectFourthLine'  and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
                Repeater4.DataSource = ds;
                Repeater4.DataBind();
            }
        }
    }

    //依任务ID取得项目ID
    protected string GetProjectIDByTaskID(string strTaskID)
    {
        string strHQL;

        strHQL = "Select ProjectID From T_ProjectTask Where TaskID =" + strTaskID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        return ds.Tables[0].Rows[0][0].ToString();
    }


    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strProject;
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

        //如果项目进度受细节影响，则直接取得
        if (ShareClass.GetProjectTypeImpactByDetail(strProjectID) == "YES")
        {
            NB_FinishPercent.Amount = decimal.Parse(ShareClass.getCurrentDateTotalProgressForMember(strProjectID, strUserCode));
            NB_ManHour.Amount = decimal.Parse(ShareClass.getCurrentDateTotalManHourByOneOperator(strProjectID, strUserCode, DateTime.Now.ToString("yyyyMMdd")));
        }

        try
        {
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
                deManHour = NB_ManHour.Amount;
                deFinishPercent = NB_FinishPercent.Amount;

                if (strLBWorkID == "-1")
                {
                    strHQL = "from Project as project where project.ProjectID = " + strProjectID;
                    ProjectBLL projectBLL = new ProjectBLL();
                    lst = projectBLL.GetAllProjects(strHQL);
                    Project project = (Project)lst[0];

                    strProject = project.ProjectName.Trim();

                    dailyWork.Type = "Participate";  
                    dailyWork.UserCode = strUserCode;
                    dailyWork.UserName = ShareClass.GetUserName(strUserCode);
                    dailyWork.WorkDate = DateTime.Now;
                    dailyWork.RecordTime = DateTime.Now;
                    dailyWork.Address = TB_WorkAddress.Text;
                    dailyWork.ProjectID = int.Parse(strProjectID);
                    dailyWork.ProjectName = strProject;
                    dailyWork.DailySummary = strTodaySummary;
                    dailyWork.Charge = 0;
                    dailyWork.FinishPercent = deFinishPercent;
                    dailyWork.ManHour = deManHour;
                    dailyWork.ConfirmManHour = deManHour;
                    dailyWork.Salary = deManHour * deUnitHourSalary;

                    deBonus = ShareClass.GetDailyWorkLogLength(dailyWork.DailySummary) * ShareClass.GetEveryCharPrice() + ShareClass.GetDailyUploadDocNumber(strUserCode, strProjectID) * ShareClass.GetEveryDocPrice();

                    dailyWork.Bonus = deBonus;
                    dailyWork.ConfirmBonus = deBonus;

                    dailyWork.Authority = DL_Authority.SelectedValue.Trim();

                    try
                    {
                        dailyWorkBLL.AddDailyWork(dailyWork);

                        //取得提交的WorkID
                        strHQL = "from DailyWork as dailyWork where dailyWork.ProjectID = " + strProjectID + " and " + " dailyWork.UserCode = " + "'" + strUserCode + "'" + " and " + "to_char(dailyWork.WorkDate,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'";
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

                    dailyWork.WorkDate = DateTime.Now;
                    dailyWork.RecordTime = DateTime.Now;
                    dailyWork.Address = TB_WorkAddress.Text;
                    dailyWork.FinishPercent = deFinishPercent;
                    dailyWork.DailySummary = strTodaySummary;
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

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
                    }
                }
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCJC").ToString().Trim() + "')", true);
        }
    }


    protected void BT_Receive_Click(object sender, EventArgs e)
    {
        string strProjectID = LB_ProjectID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus;

        string strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.UserCode = " + "'" + strUserCode + "'";
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
        RelatedUser relatedUser = (RelatedUser)lst[0];

        strStatus = relatedUser.Status.Trim();

        if (strStatus == "Plan" | strStatus == "Rejected")
        {
            relatedUser.Status = "Accepted";

            try
            {
                relatedUserBLL.UpdateRelatedUser(relatedUser, relatedUser.ID);

                strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.UserCode = " + "'" + strUserCode + "'";
                relatedUserBLL = new RelatedUserBLL();
                lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
                DataList2.DataSource = lst;
                DataList2.DataBind();

                TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("ShouLiNiDeXiangMu").ToString().Trim() + strProjectID + " " + ShareClass.GetProjectName(strProjectID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSL").ToString().Trim() + "')", true);
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
        string strStatus;

        string strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.UserCode = " + "'" + strUserCode + "'";
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
        RelatedUser relatedUser = (RelatedUser)lst[0];

        strStatus = relatedUser.Status.Trim();

        if (strStatus == "Plan" | strStatus == "Accepted")
        {
            relatedUser.Status = "Rejected";

            try
            {
                relatedUserBLL.UpdateRelatedUser(relatedUser, relatedUser.ID);

                strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.UserCode = " + "'" + strUserCode + "'";
                relatedUserBLL = new RelatedUserBLL();
                lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
                DataList2.DataSource = lst;
                DataList2.DataBind();

                TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("JuJueNiDeXiangMu").ToString().Trim() + strProjectID + " " + ShareClass.GetProjectName(strProjectID);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJJ").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJJSBJC").ToString().Trim() + "')", true);
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
        string strStatus;

        string strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.UserCode = " + "'" + strUserCode + "'";
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
        RelatedUser relatedUser = (RelatedUser)lst[0];

        strStatus = relatedUser.Status.Trim();

        if (strStatus == "Plan" | strStatus == "Rejected" | strStatus == "Accepted")
        {
            relatedUser.Status = "InProgress";

            try
            {
                relatedUserBLL.UpdateRelatedUser(relatedUser, relatedUser.ID);

                strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.UserCode = " + "'" + strUserCode + "'";
                relatedUserBLL = new RelatedUserBLL();
                lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
                DataList2.DataSource = lst;
                DataList2.DataBind();

                TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("KaiShiChuLiNiDeXiangMu").ToString().Trim() + strProjectID + " " + ShareClass.GetProjectName(strProjectID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHD").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHDSBJC").ToString().Trim() + "')", true);
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
        string strPMCode = LB_PMCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_ReturnMsg.Checked == true | CB_ReturnMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("XiangMuChuLiQingKuangFanKui").ToString().Trim();
            strMsg = TB_Message.Text.Trim();

            if (CB_ReturnMsg.Checked == true)
            {
                msg.SendMSM("Message",strPMCode, strMsg, strUserCode);
            }

            if (CB_ReturnMail.Checked == true)
            {
                msg.SendMail(strPMCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB").ToString().Trim() + "')", true);
    }

    /// <summary>
    /// 根据用户编码，角色名称，项目编号，判断该用户是否存在  By LiuJianping 2013-10-14
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

    protected decimal GetUnitHourSalary(string strUserCode, string strProjectID)
    {
        decimal deUnitHourSalary;
        string strHQL;
        IList lst;

        strHQL = "from RelatedUser as relatedUser where relatedUser.UserCode = " + "'" + strUserCode + "'" + " and relatedUser.ProjectID = " + strProjectID;
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
        RelatedUser relatedUser = (RelatedUser)lst[0];

        deUnitHourSalary = relatedUser.UnitHourSalary;

        return deUnitHourSalary;
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

}
