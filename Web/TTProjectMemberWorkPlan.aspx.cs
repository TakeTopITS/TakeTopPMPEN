using System; using System.Resources;
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
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProjectMemberWorkPlan : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strIsMobileDevice;

    string strProjectID, strProjectName;
 
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件2006－2012


        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_ReviewDetail);
HE_ReviewDetail.Language = Session["LangCode"].ToString();


        strProjectID = Request.QueryString["RelatedID"];
        strProjectName = ShareClass.GetProjectName(strProjectID);


        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","查看所有成员计划", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            ShareClass.InitialProjectMemberTree(TreeView1, strProjectID);

            if (strIsMobileDevice == "YES")
            {
                HT_ReviewDetail.Visible = true;
            }
            else
            {
                HE_ReviewDetail.Visible = true;
            }
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strMemberCode, strMemberName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        string strID = TreeView1.SelectedNode.Target.Trim();

        try
        {
            strID = int.Parse(strID).ToString();

            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ID = " + strID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ProRelatedUser proRelatedUser = (ProRelatedUser)lst[0];

                strMemberCode = proRelatedUser.UserCode.Trim();
                strMemberName = proRelatedUser.UserName.Trim();

                LB_OperatorCode.Text = strMemberCode;
                LB_OperatorName.Text = strMemberName;

                ShareClass.InitialPlanTreeByUserCode(TreeView2, strMemberCode, "PROJECT");
            }
        }
        catch
        {
        }

    }

  
    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanID, strPlanName, strPlanType, strChartTitle;
        DateTime dtStartTime, dtEndTime;
        string strDepartCode, strDepartName;
        string strOperatorCode, strOperatorName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target == "0")
        {
            strPlanID = treeNode.Target.Trim();
            strPlanName = LanguageHandle.GetWord("WoDeJiHua");
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

        BT_Add.Enabled = true;
        BT_Update.Enabled = true;
        BT_Delete.Enabled = true;

        HL_RelatedDoc.Enabled = true;
        HL_RelatedDoc.NavigateUrl = "TTPlanRelatedDoc.aspx?PlanID=" + strPlanID;

        strOperatorCode = LB_OperatorCode.Text.Trim();
        strOperatorName = LB_OperatorName.Text.Trim();

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strOperatorCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        Plan plan = GetPlanByPlanID(strPlanID);
        dtStartTime = plan.StartTime;
        dtEndTime = plan.EndTime;
        strPlanType = plan.PlanType.Trim();
        strChartTitle = plan.PlanName.Trim() + " " + strDepartName + LanguageHandle.GetWord("BuMenChengYuanJiHuaPingFenDuiB");

        strHQL = "Select CreatorCode,CreatorName,ScoringByLeader From T_Plan ";
        strHQL += " Where CreatorCode in (Select UserCode From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'" + ")";
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

        if (e.CommandName == "Update")
        {
            for (int i = 0; i < DataList4.Items.Count; i++)
            {
                DataList4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
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
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            else
            {
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
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

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;


            LoadPlanRelatedLeaderRecord(strPlanID);
            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

            UpdatePlanLeaderScoring(strPlanID);
            LoadPlan(strPlanID);
            AddLeader(strUserCode, strUserName);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC")+"')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strPlanID;
        string strReview;

        strID = LB_ID.Text.Trim();
        strPlanID = LB_PlanID.Text.Trim();

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.ID = " + strID;
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        PlanLeaderReview planLeaderReview = (PlanLeaderReview)lst[0];

        if (strIsMobileDevice == "YES")
        {
            strReview = HT_ReviewDetail.Text.Trim();
        }
        else
        {
            strReview = HE_ReviewDetail.Text.Trim();
        }

        planLeaderReview.Review = strReview;
        planLeaderReview.Scoring = NB_Scoring.Amount;

        try
        {
            planLeaderReviewBLL.UpdatePlanLeaderReview(planLeaderReview, int.Parse(strID));

            UpdatePlanLeaderScoring(strPlanID);
            LoadPlan(strPlanID);


            LoadPlanRelatedLeaderRecord(strPlanID);
            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC")+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strPlanID;

        strID = LB_ID.Text.Trim();
        strPlanID = LB_PlanID.Text.Trim();

        try
        {
            strHQL = "Delete From T_Plan_LeaderReview Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;


            LoadPlanRelatedLeaderRecord(strPlanID);
            LoadPlanRelatedLeaderHandleRecord(strPlanID, strUserCode);

            UpdatePlanLeaderScoring(strPlanID);
            LoadPlan(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC")+"')", true);
        }
    }

    protected void AddLeader(string strUserCode, string strUserName)
    {
        string strPlanID;
        string strLeaderCode, strLeaderName, strActor, strStatus;
        DateTime dtJoinTime;

        strPlanID = LB_PlanID.Text.Trim();
        strLeaderCode = strUserCode;
        strLeaderName = strUserName;
        strActor = LanguageHandle.GetWord("LingDao");
        dtJoinTime = DateTime.Now;
        strStatus = "Approved";


        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        PlanRelatedLeader planRelatedLeader = new PlanRelatedLeader();

        planRelatedLeader.PlanID = int.Parse(strPlanID);
        planRelatedLeader.LeaderCode = strLeaderCode;
        planRelatedLeader.LeaderName = strLeaderName;
        planRelatedLeader.Actor = strActor;
        planRelatedLeader.JoinTime = dtJoinTime;
        planRelatedLeader.Status = strStatus;


        try
        {
            planRelatedLeaderBLL.AddPlanRelatedLeader(planRelatedLeader);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC")+"')", true);
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

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName.Trim();

    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }
}
