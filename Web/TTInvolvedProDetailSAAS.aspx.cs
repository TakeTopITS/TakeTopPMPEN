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

public partial class TTInvolvedProDetailSAAS : System.Web.UI.Page
{
    string strIsMobileDevice;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strStatus1, strStatus2;

        strLangCode = Session["LangCode"].ToString();

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = ShareClass.GetUserName(strUserCode);
        string strUserType = ShareClass.GetUserType(strUserCode);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        string strProjectID = Request.QueryString["ProjectID"];
        if (strProjectID == null)
        {
            string strTaskID = Request.QueryString["TaskID"];
            strProjectID = GetProjectIDByTaskID(strTaskID);
        }
        LB_ProjectID.Text = strProjectID;


        //����û��Ƿ���Ŀ��Ա
        if (ShareClass.CheckUserIsProjectMember(strProjectID, strUserCode) == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }


        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        //2013-10-14 By LiuJianping ��Ŀ����
        string strPMUserCode = project.PMCode.Trim();//end
        string strProjectType = project.ProjectType.Trim();

        DataList1.DataSource = lst;
        DataList1.DataBind();

        LB_PMCode.Text = project.PMCode.Trim();

        //CKEditor��ʼ��
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

            //�����Ŀ������ϸ��Ӱ�죬��ֱ��ȡ��
            string strImpactByDetail = ShareClass.GetProjectTypeImpactByDetail(strProjectID);
            if (strImpactByDetail == "YES")
            {
                //NB_FinishPercent.Enabled = false;
                //NB_ManHour.Enabled = false;
            }

            HL_ProjectDetailView.NavigateUrl = "TTProjectDetailView.aspx?ProjectID=" + strProjectID;

            strHQL = "from DailyWork as dailyWork where dailyWork.Type = 'Participate' and dailyWork.ProjectID =" + strProjectID + " and " + " dailyWork.UserCode = " + "'" + strUserCode + "'" + " and " + "to_char(dailyWork.WorkDate,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'";  
            DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
            lst = dailyWorkBLL.GetAllDailyWorks(strHQL);

            if (lst.Count > 0)
            {
                DailyWork dailyWork = (DailyWork)lst[0];

                NB_FinishPercent.Amount = dailyWork.FinishPercent;

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
                NB_ManHour.Amount = dailyWork.ManHour;

                TB_WorkAddress.Text = dailyWork.Address;

                try
                {
                    DL_Authority.SelectedValue = dailyWork.Authority.Trim();
                }
                catch
                {
                }

                //�����Ŀ������ϸ��Ӱ�죬��ֱ��ȡ��
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
            }

            //����������ɼ�¼����
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

            DataSet ds;
            strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'ParticipateProjectFirstLineSaaS' and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
            Repeater1.DataSource = ds;
            Repeater1.DataBind();
            strHQL = "Select HomeModuleName, PageName || " + "'" + strProjectID + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'ParticipateProjectSecondLineSaaS' and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
            Repeater2.DataSource = ds;
            Repeater2.DataBind();

            if (strStatus1 == "CaseClosed" || strStatus1 == "Suspended" || strStatus1 == "Cancel" || strStatus2 == "Pause" || strStatus2 == "Stop")
            {
                Repeater1.Visible = false;
                Repeater2.Visible = false;

                BT_Summit.Enabled = false;
                BT_Activity.Enabled = false;
                BT_Receive.Enabled = false;
                BT_Refuse.Enabled = false;

                HL_RelatedRisk.NavigateUrl = "TTProRelatedRiskView.aspx?ProjectID=" + strProjectID;
                HL_ExpenseApplyWL.NavigateUrl = "TTExpenseApplyWFView.aspx?RelatedType=Project&RelatedID=" + strProjectID;
                HL_AssignMeeting.NavigateUrl = "TTProMeetingView.aspx?ProjectID=" + strProjectID;
                HL_RelatedContactInfor.NavigateUrl = "TTContactListView.aspx?RelatedType=Project&RelatedID=" + strProjectID;
                HL_ProjectAssetPurchase.NavigateUrl = "TTProjectAssetPurchaseReport.aspx?RelatedType=Project&RelatedID=" + strProjectID;
                HL_ProjectAssetApplication.NavigateUrl = "TTAssetApplicationReport.aspx?RelatedType=Project&RelatedID=" + strProjectID;

                HL_ReceivablesPayableHandle.NavigateUrl = "TTReceivablesPayableHandleView.aspx?RelatedType=Project&RelatedID=" + strProjectID;

                //2013-10-14 LiuJianping ��Ŀ�Ŀ�ģ�� ֻ��
                HL_ProjectDocControl.NavigateUrl = "TTProjDocumentControlView.aspx?ProjectID=" + strProjectID;
                //��Ŀ����ſ������ʵ����Ŀ�ɱ���������ֻ�ܲ鿴 2013-11-27 LiuJianping 
                HL_ProjectCost.NavigateUrl = "TTProjectCostOperationView.aspx?ProjectID=" + strProjectID;
                //��Ŀ����ſ������������Դ����������ֻ�ܲ鿴 2014-01-21 LiuJianping 
                HL_ProjectHR.NavigateUrl = "TTProjectMemberScheduleView.aspx?ProjectID=" + strProjectID;
            }
            else
            {
                HL_RelatedRisk.NavigateUrl = "TTProRelatedRisk.aspx?ProjectID=" + strProjectID;
                HL_ExpenseApplyWL.NavigateUrl = "TTProjectExpenseApplyWF.aspx?RelatedType=Project&RelatedID=" + strProjectID;
                HL_AssignMeeting.NavigateUrl = "TTMakeProMeeting.aspx?ProjectID=" + strProjectID;

                HL_RunProjectByWF.Enabled = true;
                HL_RunProjectByWF.NavigateUrl = "TTRelatedDIYWorkflowForm.aspx?RelatedType=Project&RelatedID=" + strProjectID;

                HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Project&RelatedID=" + strProjectID;
                HL_ProjectAssetPurchase.NavigateUrl = "TTMakeProjectAssetPO.aspx?RelatedType=Project&RelatedID=" + strProjectID;
                HL_ProjectAssetApplication.NavigateUrl = "TTProjectAssetApplicationWL.aspx?RelatedType=Project&RelatedID=" + strProjectID;

                HL_ReceivablesPayableHandle.NavigateUrl = "TTReceivablesPayableHandleForPlan.aspx?RelatedType=Project&RelatedID=" + strProjectID;

                //2013-10-14 LiuJianping ��Ŀ�Ŀ�ģ�� �ɱ༭ �ж��û��Ƿ����Ŀ�רԱ�Ľ�ɫ����Ŀ����������ɱ༭������ֻ�ܲ鿴
                if (ISActorGroupByUserCode(strUserCode.Trim(), LanguageHandle.GetWord("WenKongZhuanYuan").ToString().Trim(), strProjectID) || strUserCode.Trim().Equals(strPMUserCode.ToString()))
                {
                    HL_ProjectDocControl.NavigateUrl = "TTProjDocumentControl.aspx?ProjectID=" + strProjectID;
                }
                else
                {
                    HL_ProjectDocControl.NavigateUrl = "TTProjDocumentControlView.aspx?ProjectID=" + strProjectID;
                }
                //��Ŀ����ſ������ʵ����Ŀ�ɱ���������ֻ�ܲ鿴 2013-11-27 LiuJianping 
                if (strUserCode.Trim().Equals(strPMUserCode.ToString()))
                {
                    HL_ProjectCost.NavigateUrl = "TTProjectCostOperationEdit.aspx?ProjectID=" + strProjectID;
                    HL_ProjectHR.NavigateUrl = "TTProjectMemberScheduleEdit.aspx?ProjectID=" + strProjectID;
                }
                else
                {
                    HL_ProjectCost.NavigateUrl = "TTProjectCostOperationView.aspx?ProjectID=" + strProjectID;
                    HL_ProjectHR.NavigateUrl = "TTProjectMemberScheduleView.aspx?ProjectID=" + strProjectID;
                }
            }


            HL_AssignTask.NavigateUrl = "TTInvolvedProAssignTask.aspx?ProjectID=" + strProjectID;
            HL_RelatedReq.NavigateUrl = "TTInvolvedProjectRelatedReqMain.aspx?ProjectID=" + strProjectID;

            HL_RelatedConstract.NavigateUrl = "TTProjectRelatedConstract.aspx?ProjectID=" + strProjectID;

            HL_LeadReview.NavigateUrl = "TTLeadReviewSummary.aspx?ProjectID=" + strProjectID;
            HL_StatusChangeRecord.NavigateUrl = "TTProStatusChangeRecord.aspx?ProjectID=" + strProjectID;
            HL_TransferProject.NavigateUrl = "TTTransferProjectRecord.aspx?ProjectID=" + strProjectID;
            HL_ProjectMeeting.NavigateUrl = "TTProMeetingView.aspx?ProjectID=" + strProjectID;


            HL_WorkPlan.NavigateUrl = "TTMakePlan.aspx?RelatedType=PROJECT&RelatedID=" + strProjectID;


            if (strUserType == "INNER")
            {
                HL_MakeCollaborationThirdPart.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=PROJECT&RelatedID=" + strProjectID;
            }
            else
            {
                HL_MakeCollaborationThirdPart.NavigateUrl = "TTMakeCollaborationThirdPart.aspx?RelatedType=PROJECT&RelatedID=" + strProjectID;
            }

            string strSystemVersionType = Session["SystemVersionType"].ToString();
            if (strSystemVersionType != "GROUP" & strSystemVersionType != "ENTERPRISE")
            {
                HL_ExpenseApplyWL.Visible = false;
                HL_MakeCollaborationThirdPart.Visible = false;
                HL_ProjectDocControl.Visible = false;
                HL_ProjectHR.Visible = false;
                HL_ProjectCost.Visible = false;
                HL_RelatedRisk.Visible = false;

                HL_ProjectAssetPurchase.Visible = false;
                HL_ProjectAssetApplication.Visible = false;

                HL_RelatedConstract.Visible = false;

            }

            if (strSystemVersionType == "SAAS")
            {
                HL_RelatedContactInfor.Visible = false;
                HL_RelatedReq.Visible = false;
                HL_AssignMeeting.Visible = false;
                HL_ReceivablesPayableHandle.Visible = false;
                HL_WorkPlan.Visible = false;
                HL_RunProjectByWF.Visible = false;
            }
        }
    }

    //������IDȡ����ĿID
    protected string GetProjectIDByTaskID(string strTaskID)
    {
        string strHQL;

        strHQL = "Select ProjectID From T_ProjectTask Where TaskID =" + strTaskID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    /// <summary>
    /// �����û����룬��ɫ���ƣ���Ŀ��ţ��жϸ��û��Ƿ����  By LiuJianping 2013-10-14
    /// </summary>
    /// <param name="strusercode">�û�����</param>
    /// <param name="strgroupname">��ɫ����</param>
    /// <param name="strprojectid">��Ŀ���</param>
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

    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strProject;
        string strHQL;
        IList lst;

        string strBTText;
        string strLBWorkID;
        string strUserCode = LB_UserCode.Text.Trim();
        string strProjectID = LB_ProjectID.Text.Trim();
        decimal deManHour = NB_ManHour.Amount;
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

                if (strLBWorkID == "-1")
                {
                    strHQL = "from Project as project where project.ProjectID = " + strProjectID;

                    ProjectBLL projectBLL = new ProjectBLL();

                    lst = projectBLL.GetAllProjects(strHQL);

                    Project project = (Project)lst[0];

                    strProject = project.ProjectName.Trim(); ;
                    deFinishPercent = NB_FinishPercent.Amount;

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
                    deFinishPercent = NB_FinishPercent.Amount;

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
