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

public partial class TTPlanDetail : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strPlanID, strPlanName;
    string strPlanStartTime, strPlanEndTime;
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strCreatorCode, strPlanStatus;

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_LogDetail);
HE_LogDetail.Language = Session["LangCode"].ToString();


        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        strPlanID = Request.QueryString["PlanID"];

        strHQL = "from Plan as plan where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        Plan plan = (Plan)lst[0];
        strPlanName = plan.PlanName.Trim();
        strCreatorCode = plan.UserCode.Trim();
        strPlanStatus = plan.Status.Trim();

        strPlanStartTime = plan.StartTime.ToString("yyyyMMdd");
        strPlanEndTime = plan.EndTime.ToString("yyyyMMdd");

        NB_ScoringBySelf.Amount = plan.ScoringBySelf;

        //this.Title = LanguageHandle.GetWord("JiHua") + ": " + strPlanID + " " + strPlanName + " 处理";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_LogDetail.Visible = true;
            }
            else
            {
                HE_LogDetail.Visible = true;
            }

            DataList2.DataSource = lst;
            DataList2.DataBind();

            LoadPlanWorkLog(strPlanID);
            LoadPlanTarget(strPlanID);
            LoadPlanRelatedLeaderHandleRecord(strPlanID);

            HL_RelatedDoc.Enabled = true;
            HL_RelatedDoc.NavigateUrl = "TTPlanRelatedDoc.aspx?PlanID=" + strPlanID;

            //int intLeaderReviewCount = GetPlanRelatedLeaderCountByActiveFinish(strPlanID);
            //int intWorkLogCount = GetPlanWorkLogCountByActiveFinish(strPlanID);
      
            if (strPlanStatus != "Approved")
            {
                BT_SubmitApprove.Enabled = true;
            }
            else
            {
                BT_SubmitApprove.Enabled = false;
            }
        }
    }

    protected void DataList3_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        string strID, strWorkTime, strNow;
        string strHQL;
        IList lst;

        strID = ((Label)e.Item.FindControl("LB_ID")).Text;

        if (e.CommandName == "Update")
        {
            for (int i = 0; i < DataList3.Items.Count; i++)
            {
                DataList3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "From PlanWorkLog as planWorkLog where planWorkLog.ID = " + strID;

            PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
            lst = planWorkLogBLL.GetAllPlanWorkLogs(strHQL);

            PlanWorkLog planWorkLog = (PlanWorkLog)lst[0];

            LB_ID.Text = strID;

            if (strIsMobileDevice == "YES")
            {
                HT_LogDetail.Text = planWorkLog.LogDetail.Trim();
            }
            else
            {
                HE_LogDetail.Text = planWorkLog.LogDetail.Trim();
            }

            NB_PlanProgress.Amount = planWorkLog.Progress;
            strWorkTime = planWorkLog.WorkTime.ToString("yyyyMMdd");


            strNow = DateTime.Now.ToString("yyyyMMdd");

            if (strNow != strWorkTime)
            {
                BT_New.Visible = false;
            }
            else
            {
                BT_New.Visible = true;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }

        if (e.CommandName == "Delete")
        {
            string  strProgress;

            try
            {
                strHQL = "Delete From T_Plan_WorkLog Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                strProgress = GetMaxPlanWorkLogProgress(strPlanID);
                strHQL = "Update T_Plan Set Progress = " + strProgress + " Where PlanID = " + strPlanID;
                ShareClass.RunSqlCommand(strHQL);

                LoadPlanWorkLog(strPlanID);
                LoadPlan(strPlanID);
                

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
            }
        }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        BT_New.Visible = true;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddDetail();
        }
        else
        {
            UpdateDetail();
        }
    }

    protected void AddDetail()
    {
        string strLogDetail;
        int intProgress;

        if (strIsMobileDevice == "YES")
        {
            strLogDetail = HT_LogDetail.Text.Trim();
        }
        else
        {
            strLogDetail = HE_LogDetail.Text.Trim();
        }
        intProgress = int.Parse(NB_PlanProgress.Amount.ToString());

        PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
        PlanWorkLog planWorkLog = new PlanWorkLog();

        planWorkLog.PlanID = int.Parse(strPlanID);
        planWorkLog.LogDetail = strLogDetail;
        planWorkLog.Progress = intProgress;
        planWorkLog.WorkTime = DateTime.Now;
        planWorkLog.UserCode = strUserCode;
        planWorkLog.UserName = ShareClass.GetUserName(strUserCode);

        try
        {
            planWorkLogBLL.AddPlanWorkLog(planWorkLog);

            LB_ID.Text = ShareClass.GetMyCreatedMaxPlanWorkLogID(strPlanID);

            UpdateWholePlanProgress(strPlanID, intProgress.ToString());

            LoadPlanWorkLog(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void UpdateDetail()
    {
        string strHQL;
        IList lst;

        string strID, strLogDetail;
        int intProgress;

        strID = LB_ID.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strLogDetail = HT_LogDetail.Text.Trim();
        }
        else
        {
            strLogDetail = HE_LogDetail.Text.Trim();
        }

        intProgress = int.Parse(NB_PlanProgress.Amount.ToString());

        strHQL = "From PlanWorkLog as planWorkLog where planWorkLog.ID = " + strID;
        PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
        lst = planWorkLogBLL.GetAllPlanWorkLogs(strHQL);

        PlanWorkLog planWorkLog = (PlanWorkLog)lst[0];

        planWorkLog.LogDetail = strLogDetail;
        planWorkLog.Progress = intProgress;


        try
        {
            planWorkLogBLL.UpdatePlanWorkLog(planWorkLog, int.Parse(strID));

            UpdateWholePlanProgress(strPlanID, intProgress.ToString());

            LoadPlanWorkLog(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }
  

    protected void BT_UpdateScoringBySelf_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strScoringBySelf = NB_ScoringBySelf.Amount.ToString();

        try
        {
            strHQL = "Update T_Plan Set ScoringBySelf = " + strScoringBySelf + " where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            LoadPlan(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXZPFCG") + "')", true);
        }
        catch
        {
        }
    }

    protected void BT_Finish_Click(object sender, EventArgs e)
    {
        string strHQL;

        try
        {
            strHQL = "Update T_Plan Set Status = 'Completed' where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            LoadPlan(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCCG") + "')", true);
        }
        catch
        {
        }
    }

    protected void BT_SubmitApprove_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanRelatedLeader as planRelatedLeader where planRelatedLeader.PlanID = " + strPlanID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);
        if (lst.Count == 0)
        {
            strHQL = "Insert Into T_Plan_RelatedLeader (PlanID,LeaderCode,LeaderName ,JoinTime ,Actor,Status )";
            strHQL += " Select " + strPlanID + ",LeaderCode,LeaderName ,now() ,Actor,'New' From T_Plan_RelatedLeader ";
            strHQL += " Where PlanID in (Select MAX(PlanID) From T_Plan Where UserCode = '" + strUserCode + "' and PlanID <> " + strPlanID + ")";
            strHQL += " And LeaderCode Not In (Select LeaderCode From T_Plan_RelatedLeader Where PlanID = " + strPlanID + ")";
            ShareClass.RunSqlCommand(strHQL);
        }

        strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);
        if (lst.Count > 0)
        {
           Plan plan = (Plan)lst[0];
           
            plan.SubmitTime = DateTime.Now.ToString();
            plan.Status = "Pending Review";

            try
            {
                planBLL.UpdatePlan(plan, int.Parse(strPlanID));

                LoadPlanList(strPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSHCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSHSBJC") + "')", true);
            }
        }
    }

    protected void LoadPlanList(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataList2.DataSource = lst;
        DataList2.DataBind();
    }
  
    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        string strTargetID;
        decimal deTargetProgress;

        ShareClass.ColorDataGridSelectRow(DataGrid1, e);

        strTargetID = e.Item.Cells[0].Text;

        strHQL = "From PlanTarget as planTarget Where planTarget.ID = " + strTargetID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        PlanTarget planTarget = new PlanTarget();

        if (lst.Count > 0)
        {
            planTarget = (PlanTarget)lst[0];

            deTargetProgress = decimal.Parse(((TextBox)e.Item.FindControl("NB_TargetProgress")).Text);

            planTarget.Progress = int.Parse(deTargetProgress.ToString());

            try
            {
                planTargetBLL.UpdatePlanTarget(planTarget, int.Parse(strTargetID));

                LoadPlanTarget(strPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }
    }


    protected void UpdateTarget(string strTargetID,decimal deTargetProgress)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanTarget as planTarget Where planTarget.ID = " + strTargetID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        PlanTarget planTarget = new PlanTarget();

        if (lst.Count > 0)
        {
            planTarget = (PlanTarget)lst[0];

            planTarget.Progress = int.Parse(deTargetProgress.ToString());

            try
            {
                planTargetBLL.UpdatePlanTarget(planTarget, int.Parse(strTargetID));

                LoadPlanTarget(strPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }
    }

    protected void BT_ImportSamePeriodSchedule_Click(object sender, EventArgs e)
    {
        string strHQL1, strHQL2;

        strHQL1 = "Insert Into T_Plan_WorkLog(PlanID,LogDetail,Progress,WorkTime,UserCode,UserName,ScheduleEventID) ";
        strHQL1 += " Select " + strPlanID + ",Name + ':' + EventContent,0,EventStart,UserCode,UserName,ID From T_ScheduleEvent Where UserCode = " + "'" + strUserCode + "'";
        strHQL1 += " and to_char(EventStart,'yyyymmdd') >= " + "'" + strPlanStartTime + "'" + " and to_char(EventEnd,'yyyymmdd') <= " + "'" + strPlanEndTime + "'";
        strHQL1 += " and ID not in (Select ScheduleEventID From T_Plan_WorkLog Where PlanID = " + strPlanID + ")";
        strHQL1 += " Order By ID ASC";

        try
        {
            ShareClass.RunSqlCommand(strHQL1);

            strHQL2 = "Update T_Plan_WorkLog Set LogDetail = Name + ':' + EventContent  From T_ScheduleEvent A,T_Plan_WorkLog B Where A.ID = B.ScheduleEventID";
            strHQL2 += " and B.PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL2);

            LoadPlanWorkLog(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDRWC") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDRSBJC") + "')", true);
        }

    }

    protected void UpdateWholePlanProgress(string strPlanID, string strProgress)
    {
        string strHQL;

        strHQL = "Update T_Plan Set Progress = " + strProgress + " where PlanID = " + strPlanID;
        ShareClass.RunSqlCommand(strHQL);

        LoadPlan(strPlanID);
    }

    protected string GetMaxPlanWorkLogProgress(string strPlanID)
    {
        string strHQL;

        strHQL = "Select Progress From T_Plan_WorkLog Where PlanID = " + strPlanID + " Order By ID DESC limit 1";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Plan_WorkLog");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }


    protected void LoadPlan(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataList2.DataSource = lst;
        DataList2.DataBind();
    }

    protected void LoadPlanWorkLog(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanWorkLog as planWorkLog Where PlanID = " + strPlanID;
        strHQL += " Order By planWorkLog.WorkTime DESC";
        PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
        lst = planWorkLogBLL.GetAllPlanWorkLogs(strHQL);

        DataList3.DataSource = lst;
        DataList3.DataBind();
    }

    protected void LoadPlanTarget(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanTarget as planTarget Where planTarget.PlanID = " + strPlanID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadPlanRelatedLeaderRecord(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.PlanID = " + strPlanID;
        strHQL += " Order By planLeaderReview.ID DESC";
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        DataList1.DataSource = lst;
        DataList1.DataBind();
    }

    protected void LoadPlanRelatedLeaderHandleRecord(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.PlanID = " + strPlanID;
        strHQL += " Order By planLeaderReview.ID DESC";
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        DataList1.DataSource = lst;
        DataList1.DataBind();
    }

    protected bool UpdatePlanLeaderScoring(string strPlanID)
    {
        string strHQL;

        decimal deLeaderScoring;

        strHQL = "Select Avg(Scoring) From T_Plan_LeaderReview Where PlanID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AvgScoring");

        deLeaderScoring = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

        try
        {
            strHQL = "Update T_Plan Set ScoringByLeader = " + deLeaderScoring.ToString() + " Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return true;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return false;
        }

    }

    protected int GetPlanRelatedLeaderCountByActiveFinish(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.Status in ('Approved','Completed')";
        strHQL += " and planRelatedLeader.PlanID = " + strPlanID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        return lst.Count;
    }

    protected int GetPlanWorkLogCountByActiveFinish(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanWorkLog as planWorkLog Where PlanID = " + strPlanID;
        strHQL += " Order By planWorkLog.ID DESC";
        PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
        lst = planWorkLogBLL.GetAllPlanWorkLogs(strHQL);

        return lst.Count;
    }


}
