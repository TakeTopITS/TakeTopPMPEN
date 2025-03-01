using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTPlanLeaderReviewDetail : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strPlanID, strPlanName;

    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strCreatorCode, strPlanStatus;

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_ReviewDetail);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strPlanID = Request.QueryString["PlanID"];

        strHQL = "from Plan as plan where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        Plan plan = (Plan)lst[0];
        strPlanName = plan.PlanName.Trim();
        strCreatorCode = plan.UserCode.Trim();
        strPlanStatus = plan.Status.Trim();

        //this.Title = "下属计划：" + strPlanID + " " + strPlanName + " "+LanguageHandle.GetWord("ShenHe")+"";


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_ReviewDetail.Visible = true;
            }
            else
            {
                HE_ReviewDetail.Visible = true;
            }

            DataList2.DataSource = lst;
            DataList2.DataBind();

            LB_LeaderStatus.Text = GetPlanRelatedLeaderStatus(strPlanID, strUserCode);

            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);
            LoadPlanWorkLog(strPlanID);
            LoadPlanTarget(strPlanID);

            HL_RelatedDoc.Enabled = true;
            HL_RelatedDoc.NavigateUrl = "TTPlanRelatedDoc.aspx?PlanID=" + strPlanID;
        }
    }

    protected void BT_Agree_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "Update T_Plan_RelatedLeader Set Status = 'Approved' Where LeaderCode = " + "'" + strUserCode + "'";
        strHQL += " and PlanID = " + strPlanID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_New.Visible = true;
            BT_Finish.Enabled = true;

            LB_LeaderStatus.Text = "Approved";

            strHQL = "From PlanRelatedLeader as planRelatedLeader where planRelatedLeader.PlanID = " + strPlanID + " and planRelatedLeader.Status <> 'Approved'";
            PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
            lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

            if (lst.Count == 0)
            {
                strHQL = "Update T_Plan Set Status = 'Approved' Where PlanID = " + strPlanID;
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadPlan(strPlanID);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_CancelAgree_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "Update T_Plan_RelatedLeader Set Status = 'New' Where LeaderCode = " + "'" + strUserCode + "'";
        strHQL += " and PlanID = " + strPlanID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_New.Visible = true;
            BT_Finish.Enabled = true;

            LB_LeaderStatus.Text = "New";


            strHQL = "Update T_Plan Set Status = 'PendingReview' Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            LoadPlan(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQuXiaoLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQuXiaoLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
        }
    }

    protected void BT_Finish_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "Update T_Plan_RelatedLeader Set Status = 'Completed' Where LeaderCode = " + "'" + strUserCode + "'";
        strHQL += " and PlanID = " + strPlanID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_New.Visible = true;
            BT_Finish.Enabled = true;

            LB_LeaderStatus.Text = "Completed";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCJCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCJCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataList3_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        string strID, strReviewTime, strNow;
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

            strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.ID = " + strID;
            PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
            lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

            PlanLeaderReview planLeaderReview = (PlanLeaderReview)lst[0];


            LB_ID.Text = strID;

            if (strIsMobileDevice == "YES")
            {
                HT_ReviewDetail.Text = planLeaderReview.Review.Trim();
            }
            else
            {
                HE_ReviewDetail.Text = planLeaderReview.Review.Trim();
            }

            NB_Scoring.Amount = planLeaderReview.Scoring;
            strReviewTime = planLeaderReview.ReviewTime.ToString("yyyyMMdd");


            strNow = DateTime.Now.ToString("yyyyMMdd");

            if (strNow != strReviewTime)
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
            try
            {
                strHQL = "Delete From T_Plan_LeaderReview Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);
                

                LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

                UpdatePlanLeaderScoring(strPlanID);
                LoadPlan(strPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
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
        string strReview;
        decimal deScoring;

        if (strIsMobileDevice == "YES")
        {
            strReview = HT_ReviewDetail.Text.Trim();
        }
        else
        {
            strReview = HE_ReviewDetail.Text.Trim();
        }

        deScoring = NB_Scoring.Amount;

        try
        {
            PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
            PlanLeaderReview planLeaderReview = new PlanLeaderReview();

            planLeaderReview.PlanID = int.Parse(strPlanID);
            planLeaderReview.Review = strReview;
            planLeaderReview.Scoring = deScoring;
            planLeaderReview.LeaderCode = strUserCode;
            planLeaderReview.LeaderName = ShareClass.GetUserName(strUserCode);
            planLeaderReview.ReviewTime = DateTime.Now;

            planLeaderReviewBLL.AddPlanLeaderReview(planLeaderReview);

            LB_ID.Text = ShareClass.GetMyCreatedMaxPlanLeaderReviewID(strPlanID);

            BT_New.Visible = true;

            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

            UpdatePlanLeaderScoring(strPlanID);
            LoadPlan(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateDetail()
    {
        string strHQL;
        IList lst;

        string strID, strReview;

        if (strIsMobileDevice == "YES")
        {
            strReview = HT_ReviewDetail.Text.Trim();
        }
        else
        {
            strReview = HE_ReviewDetail.Text.Trim();
        }

        strID = LB_ID.Text.Trim();

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.ID = " + strID;
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        PlanLeaderReview planLeaderReview = (PlanLeaderReview)lst[0];

        planLeaderReview.Review = strReview;
        planLeaderReview.Scoring = NB_Scoring.Amount;

        try
        {
            planLeaderReviewBLL.UpdatePlanLeaderReview(planLeaderReview, int.Parse(strID));

            UpdatePlanLeaderScoring(strPlanID);
            LoadPlan(strPlanID);

            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }


    protected string GetPlanRelatedLeaderStatus(string strPlanID, string strLeaderCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanRelatedLeader as planRelatedLeader where planRelatedLeader.PlanID = " + strPlanID;
        strHQL += " and planRelatedLeader.LeaderCode = " + "'" + strLeaderCode + "'";
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        if (lst.Count > 0)
        {
            PlanRelatedLeader planRelatedLeader = (PlanRelatedLeader)lst[0];

            return planRelatedLeader.Status.Trim();
        }
        else
        {
            return "";
        }
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

            return true;
        }
        catch
        {
            return false;
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

    protected void LoadPlanTarget(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanTarget as planTarget Where planTarget.PlanID = " + strPlanID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadPlanWorkLog(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanWorkLog as planWorkLog Where PlanID = " + strPlanID;
        strHQL += " Order By planWorkLog.ID DESC";
        PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
        lst = planWorkLogBLL.GetAllPlanWorkLogs(strHQL);

        DataList1.DataSource = lst;
        DataList1.DataBind();
    }

    protected void LoadPlanRelatedLeaderHandleRecord(string strPlanID, string strLeaderCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.PlanID = " + strPlanID + " and planLeaderReview.LeaderCode = " + "'" + strLeaderCode + "'";
        strHQL += " Order By planLeaderReview.ID DESC";
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        DataList3.DataSource = lst;
        DataList3.DataBind();
    }

}
