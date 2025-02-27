using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTMyMemberPlans : System.Web.UI.Page
{
    string strUserCode, strUserName;

    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件2006－2012

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_ReviewDetail);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

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

            ShareClass.LoadMemberByUserCodeForDataGrid(strUserCode, "Plan", DataGrid1);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strOperatorName = ShareClass.GetUserName(strOperatorCode);

        LB_OperatorCode.Text = strOperatorCode;
        LB_OperatorName.Text = strOperatorName;


        ShareClass.InitialPlanTreeByUserCode(TreeView2, strOperatorCode, "OTHER");

        LoadPlan("0");
        LoadPlanWorkLog("0");
        LoadPlanTarget("0");
        LoadPlanRelatedLeaderRecord("0");
        LoadPlanRelatedLeaderHandleRecord("0", strUserCode);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanID, strPlanName, strPlanType, strChartTitle;
        DateTime dtStartTime, dtEndTime;


        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target == "0")
        {
            strPlanID = treeNode.Target.Trim();
            strPlanName = LanguageHandle.GetWord("WoDeJiHua").ToString().Trim();
        }
        else
        {
            strPlanID = treeNode.Target.Trim();
            strPlanName = treeNode.Text.Trim();
        }

        if (strPlanID == "0")
        {
            return;
        }


        LoadPlan(strPlanID);
        LoadPlanWorkLog(strPlanID);
        LoadPlanTarget(strPlanID);
        LoadPlanRelatedLeaderRecord(strPlanID);
        LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

        LB_PlanID.Text = strPlanID;

        BT_New.Visible = true;

        HL_RelatedDoc.Enabled = true;
        HL_RelatedDoc.NavigateUrl = "TTPlanRelatedDoc.aspx?PlanID=" + strPlanID;


        Plan plan = GetPlanByPlanID(strPlanID);
        dtStartTime = plan.StartTime;
        dtEndTime = plan.EndTime;
        strPlanType = plan.PlanType.Trim();
        strChartTitle = plan.PlanName.Trim() + " " + LanguageHandle.GetWord("ZhiJieChengYuanJiHuaPingFenDui").ToString().Trim();

        strHQL = "Select (CreatorCode || CreatorName) as XName,ScoringByLeader as YNumber From T_Plan ";
        strHQL += " Where CreatorCode in (Select UnderCode From T_MemberLevel Where UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and PlanType = " + "'" + strPlanType + "'";
        strHQL += " and  to_char(StartTime,'yyyymmdd') >= " + "'" + dtStartTime.ToString("yyyyMMdd") + "'" + " and to_char(EndTime,'yyyymmdd') <= " + "'" + dtEndTime.ToString("yyyyMMdd") + "'";
        strHQL += " Order by ScoringByLeader ASC";
        IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Column&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strHQL);

    }



    protected void DataList4_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        string strID, strReviewTime, strNow;
        string strHQL;
        IList lst;

        strID = ((Label)e.Item.FindControl("LB_ID")).Text;

        if (e.CommandName == "Update")
        {
            for (int i = 0; i < DataList4.Items.Count; i++)
            {
                DataList4.Items[i].ForeColor = Color.Black;
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
            string strPlanID;

            strID = LB_ID.Text.Trim();
            strPlanID = LB_PlanID.Text.Trim();

            try
            {
                strHQL = "Delete From T_Plan_LeaderReview Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                BT_New.Visible = false;

                LoadPlanRelatedLeaderRecord(strPlanID);
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
        string strPlanID, strReview;
        decimal deScoring;

        strPlanID = LB_PlanID.Text.Trim();

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

            LoadPlanRelatedLeaderRecord(strPlanID);
            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

            UpdatePlanLeaderScoring(strPlanID);
            LoadPlan(strPlanID);
            AddLeader(strUserCode, strUserName);

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

        string strID, strPlanID;

        strID = LB_ID.Text.Trim();
        strPlanID = LB_PlanID.Text.Trim();

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.ID = " + strID;
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        PlanLeaderReview planLeaderReview = (PlanLeaderReview)lst[0];

        if (strIsMobileDevice == "YES")
        {
            planLeaderReview.Review = HT_ReviewDetail.Text.Trim();
        }
        else
        {
            planLeaderReview.Review = HE_ReviewDetail.Text.Trim();
        }

        planLeaderReview.Scoring = NB_Scoring.Amount;

        try
        {
            planLeaderReviewBLL.UpdatePlanLeaderReview(planLeaderReview, int.Parse(strID));

            UpdatePlanLeaderScoring(strPlanID);
            LoadPlan(strPlanID);


            LoadPlanRelatedLeaderRecord(strPlanID);
            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void AddLeader(string strUserCode, string strUserName)
    {
        string strHQL;
        IList lst;

        string strPlanID;
        string strLeaderCode, strLeaderName, strActor, strStatus;
        DateTime dtJoinTime;

        strPlanID = LB_PlanID.Text.Trim();
        strLeaderCode = strUserCode;
        strLeaderName = strUserName;
        strActor = LanguageHandle.GetWord("LingDao").ToString().Trim();
        dtJoinTime = DateTime.Now;
        strStatus = "Approved";


        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        PlanRelatedLeader planRelatedLeader = new PlanRelatedLeader();

        strHQL = "From PlanRelatedLeader as planRelatedLeader where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'";
        strHQL += " and planRelatedLeader.PlanID = " + strPlanID;
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);
        if (lst.Count > 0)
        {
            return;
        }

        planRelatedLeader.PlanID = int.Parse(strPlanID);
        planRelatedLeader.LeaderCode = strLeaderCode;
        planRelatedLeader.LeaderName = strLeaderName;
        planRelatedLeader.Actor = strActor;
        planRelatedLeader.JoinTime = dtJoinTime;
        planRelatedLeader.Status = strStatus;


        try
        {
            planRelatedLeaderBLL.AddPlanRelatedLeader(planRelatedLeader);
        }
        catch
        {

        }
    }

    protected void LoadPlanRelatedLeaderHandleRecord(string strPlanID, string strLeaderCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.PlanID = " + strPlanID + " and planLeaderReview.LeaderCode = " + "'" + strLeaderCode + "'";
        strHQL += " Order By planLeaderReview.ID DESC";
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        DataList4.DataSource = lst;
        DataList4.DataBind();
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

    protected Plan GetPlanByPlanID(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        Plan plan = (Plan)lst[0];

        return plan;
    }

    protected void LoadPlanWorkLog(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanWorkLog as planWorkLog Where PlanID = " + strPlanID;
        strHQL += " Order By planWorkLog.ID DESC";
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

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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

    protected void LoadPlanMyReviewRecord(string strPlanID, string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.PlanID = " + strPlanID;
        strHQL += " and planLeaderReview.LeaderCode = " + "'" + strUserCode + "'";
        strHQL += " Order By planLeaderReview.ID DESC";
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        DataList4.DataSource = lst;
        DataList4.DataBind();
    }

}
