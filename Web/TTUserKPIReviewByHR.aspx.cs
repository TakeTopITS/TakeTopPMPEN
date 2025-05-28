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
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using Npgsql;

public partial class TTUserKPIReviewByHR : System.Web.UI.Page
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
        _FileBrowser.SetupCKEditor(HE_HRSummary);
HE_HRSummary.Language = Session["LangCode"].ToString();

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工绩效评审", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_HRSummary.Visible = true;
            }
            else
            {
                HE_HRSummary.Visible = true;
            }

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            LB_DepartCode.Text = strDepartCode;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strOperatorName = ShareClass.GetUserName(strOperatorCode);

        LB_OperatorCode.Text = strOperatorCode;
        LB_OperatorName.Text = strOperatorName;

        ShareClass.InitialKPICheckTreeByUserCode(TreeView2, strOperatorCode);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strKPICheckID, strKPICheckName;
        string strStatus;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strKPICheckID = treeNode.Target.Trim();
            strKPICheckName = treeNode.Text.Trim();

            LB_KPICheckID.Text = strKPICheckID;
            LB_KPICheckName.Text = strKPICheckName;

            //计算KPI的系统评分
            LB_TotalSqlPoint.Text = ShareClass.CalculateSystemPoint(strKPICheckID).ToString();

            LoadUserKPIList(strKPICheckID);

            UserKPICheck userKPICheck = GetUserKPICheck(strKPICheckID);

            strStatus = userKPICheck.Status.Trim();
            if (strStatus == "Closed")
            {
                BT_NewMain.Visible = false;
            }
            LB_Status.Text = strStatus;

            LB_TotalSelfPoint.Text = userKPICheck.TotalSelfPoint.ToString();
            LB_TotalLeaderPoint.Text = userKPICheck.TotalLeaderPoint.ToString();
            LB_TotalThirdPartPoint.Text = userKPICheck.TotalThirdPartPoint.ToString();
            LB_TotalSqlPoint.Text = userKPICheck.TotalSqlPoint.ToString();
            LB_TotalHRPoint.Text = userKPICheck.TotalHRPoint.ToString();

            //更改KPI明细总分
            LB_TotalPoint.Text = ShareClass.UpdateKPICheckDetailTotalPoint(strKPICheckID).ToString();

            //生成同部门同职称员工绩效评分对比柱状图
            CreateSameDepartmentJobTitleKPIScoringChart(userKPICheck.UserCode.Trim(), userKPICheck);
        }
    }
    
    protected void CreateSameDepartmentJobTitleKPIScoringChart(string strUserCode, UserKPICheck userKPICheck)
    {
        string strHQL;
        string strChartTitle;
        string strDepartCode, strDepartName, strJobTitle;
        DateTime dtStartTime, dtEndTime;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strJobTitle = ShareClass.GetUserJobTitle(strUserCode);

        dtStartTime = userKPICheck.StartTime;
        dtEndTime = userKPICheck.EndTime;

        strChartTitle = LanguageHandle.GetWord("BuMen").ToString().Trim() + strDepartName + LanguageHandle.GetWord("ChengYuanJiXiaoZongPingFenDuiB").ToString().Trim();

        strHQL = "Select UserName as XName,TotalPoint as YNumber From T_UserKPICheck ";
        strHQL += " Where UserCode in (Select UserCode From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "')";
        strHQL += " and to_char(StartTime,'yyyymmdd') >= " + "'" + dtStartTime.ToString("yyyyMMdd") + "'" + " and to_char(EndTime,'yyyymmdd') <= " + "'" + dtEndTime.ToString("yyyyMMdd") + "'";
        strHQL += " Order by TotalPoint ASC";

        IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Column&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strHQL);

    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strKPIID, strStatus;

        if (e.CommandName != "Page")
        {
            strKPIID = e.Item.Cells[1].Text;

            LB_KPIID.Text = strKPIID;

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "Select Point,Comment From T_KPIHRReview Where UserKPIID = " + strKPIID + " and HRCode = " + "'" + strUserCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPIHRReview");
            if (ds.Tables[0].Rows.Count > 0)
            {
                NB_HRPoint.Amount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (strIsMobileDevice == "YES")
                {
                    HT_HRSummary.Text = ds.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    HE_HRSummary.Text = ds.Tables[0].Rows[0][1].ToString();
                }
            }
            else
            {
                NB_HRPoint.Amount = 100;

                if (strIsMobileDevice == "YES")
                {
                    HT_HRSummary.Text = "";
                }
                else
                {
                    HE_HRSummary.Text = "";
                }
            }

            //列出KPI评论表
            LoadKPIReviewList(strKPIID);

            strStatus = LB_Status.Text.Trim();
            if (strStatus == "Closed")
            {
                BT_NewMain.Visible = false;
            }
            else
            {
                BT_NewMain.Visible = true;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        IList lst;

        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strKPICheckID, strKPIHRReviewID;
        string strKPIID, strUserComment;
        decimal deHRPoint;

        strKPICheckID = LB_KPICheckID.Text.Trim();
        strKPIID = LB_KPIID.Text.Trim();

        deHRPoint = NB_HRPoint.Amount;
        if (deHRPoint > 100)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFSBNDY100JC").ToString().Trim() + "')", true);
            return;
        }

        if (strIsMobileDevice == "YES")
        {
            strUserComment = HT_HRSummary.Text.Trim();
        }
        else
        {
            strUserComment = HE_HRSummary.Text.Trim();
        }

        if (strUserComment == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGPHYJBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        strHQL = "From KPIHRReview as kpiHRReview Where kpiHRReview.HRCode = " + "'" + strUserCode + "'";
        strHQL += " and kpiHRReview.UserKPIID = " + strKPIID;
        KPIHRReviewBLL kpiHRReviewBLL = new KPIHRReviewBLL();
        lst = kpiHRReviewBLL.GetAllKPIHRReviews(strHQL);

        KPIHRReview kpiHRReview = new KPIHRReview();

        if (lst.Count == 0)
        {
            kpiHRReview.UserKPIID = int.Parse(strKPIID);
            kpiHRReview.HRCode = strUserCode;
            kpiHRReview.HRName = strUserName;
            kpiHRReview.Point = deHRPoint;
            kpiHRReview.Comment = strUserComment;
            kpiHRReview.ReviewTime = DateTime.Now;

            try
            {
                kpiHRReviewBLL.AddKPIHRReview(kpiHRReview);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            kpiHRReview = (KPIHRReview)lst[0];

            strKPIHRReviewID = kpiHRReview.ID.ToString();

            kpiHRReview.UserKPIID = int.Parse(strKPIID);
            kpiHRReview.HRCode = strUserCode;
            kpiHRReview.HRName = strUserName;
            kpiHRReview.Point = deHRPoint;
            kpiHRReview.Comment = strUserComment;
            kpiHRReview.ReviewTime = DateTime.Now;

            try
            {
                kpiHRReviewBLL.UpdateKPIHRReview(kpiHRReview, int.Parse(strKPIHRReviewID));

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }

        //更改人事评分总分
        UpdateHRKPICheckPoint(strKPICheckID, strKPIID);

        //更改KPI明细总分
        LB_TotalPoint.Text = ShareClass.UpdateKPICheckDetailTotalPoint(strKPICheckID).ToString();

        LoadUserKPIList(strKPICheckID);

        //列出KPI评论表
        LoadKPIReviewList(strKPIID);
    }

    protected void UpdateHRKPICheckPoint(string strKPICheckID, string strKPICheckDetailID)
    {
        string strHQL;
        string strAverageHRPoint, strTotalHRPoint;

        DataSet ds;

        strHQL = "Select Avg(Point) From T_KPIHRReview Where UserKPIID = " + strKPICheckDetailID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPIHRReview");
        strAverageHRPoint = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_UserKPICheckDetail Set HRPoint = " + strAverageHRPoint + " Where ID = " + strKPICheckDetailID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Select Sum(HRPoint * Weight) From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheckDetail");
        strTotalHRPoint = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_UserKPICheck Set TotalHRPoint = " + strTotalHRPoint + " where KPICheckID = " + strKPICheckID;
        ShareClass.RunSqlCommand(strHQL);

        LB_TotalHRPoint.Text = strTotalHRPoint;
    }

    protected void UpdateUserKPICheckPoint(string strKPICheckID)
    {
        string strHQL;
        string strTotalPoint;

        strHQL = "Select  Sum(Point * Weight) From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheckDetail");

        strTotalPoint = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_UserKPICheck Set TotalPoint = " + strTotalPoint + " where KPICheckID = " + strKPICheckID;
        ShareClass.RunSqlCommand(strHQL);

        LB_TotalHRPoint.Text = strTotalPoint;
    }

    protected UserKPICheck GetUserKPICheck(string strKPICheckID)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserKPICheck as userKPICheck Where userKPICheck.KPICheckID = " + strKPICheckID;
        UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
        lst = userKPICheckBLL.GetAllUserKPIChecks(strHQL);

        UserKPICheck userKPICheck = (UserKPICheck)lst[0];

        return userKPICheck;
    }

    //列出KPI评论表
    protected void LoadKPIReviewList(string strKPIID)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserKPICheckDetail as userKPICheckDetail Where userKPICheckDetail.ID = " + strKPIID;
        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        strHQL = "From KPIThirdPartReview as kpiThirdPartReview Where kpiThirdPartReview.UserKPIID = " + strKPIID;
        KPIThirdPartReviewBLL kpiThirdPartReviewBLL = new KPIThirdPartReviewBLL();
        lst = kpiThirdPartReviewBLL.GetAllKPIThirdPartReviews(strHQL);
        DataList3.DataSource = lst;
        DataList3.DataBind();

        strHQL = "From KPILeaderReview as kpiLeaderReview Where kpiLeaderReview.UserKPIID = " + strKPIID;
        KPILeaderReviewBLL kpiLeaderReviewBLL = new KPILeaderReviewBLL();
        lst = kpiLeaderReviewBLL.GetAllKPILeaderReviews(strHQL);
        DataList2.DataSource = lst;
        DataList2.DataBind();

        strHQL = "From KPIHRReview as kpiHRReview Where kpiHRReview.UserKPIID = " + strKPIID;
        KPIHRReviewBLL kpiHRReviewBLL = new KPIHRReviewBLL();
        lst = kpiHRReviewBLL.GetAllKPIHRReviews(strHQL);
        DataList4.DataSource = lst;
        DataList4.DataBind();
    }

    protected void LoadUserKPIList(string strKPICheckID)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserKPICheckDetail as userKPICheckDetail where userKPICheckDetail.KPICheckID = " + strKPICheckID;
        strHQL += " Order by userKPICheckDetail.ID ASC";

        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

}
