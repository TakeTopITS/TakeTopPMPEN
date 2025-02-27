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
using Npgsql;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTKPILeaderReview : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件2006－2012

        string strKPICheckID;
        string strStatus;

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_LeaderSummary);

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        strKPICheckID = Request.QueryString["KPICheckID"];

        UserKPICheck userKPICheck = GetUserKPICheck(strKPICheckID);

        //this.Title = "员工: " + userKPICheck.UserName.Trim() + "绩效" + strKPICheckID + "评核";

        //生成同部门同职称员工绩效评分对比柱状图
        CreateSameDepartmentJobTitleKPIScoringChart(strUserCode, userKPICheck);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_LeaderSummary.Visible = true;
            }
            else
            {
                HE_LeaderSummary.Visible = true;
            }

            //计算KPI的系统评分
            LB_TotalSqlPoint.Text = ShareClass.CalculateSystemPoint(strKPICheckID).ToString();

            LoadKPI(strKPICheckID);

            LB_KPICheckID.Text = strKPICheckID;
            LB_KPICheckName.Text = userKPICheck.KPICheckName.Trim();

            strStatus = userKPICheck.Status.Trim();
            if (strStatus == "Closed")
            {
                BT_NewMain.Visible = false;
            }
            LB_Status.Text = strStatus;

            LB_TotalSelfPoint.Text = userKPICheck.TotalSelfPoint.ToString();
            LB_TotalLeaderPoint.Text = userKPICheck.TotalLeaderPoint.ToString();
            LB_TotalThirdPartPoint.Text = userKPICheck.TotalThirdPartPoint.ToString();
            LB_TotalHRPoint.Text = userKPICheck.TotalHRPoint.ToString();
            LB_TotalPoint.Text = userKPICheck.TotalPoint.ToString();

            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;
        }
    }

    protected void CreateSameDepartmentJobTitleKPIScoringChart(string strUserCode, UserKPICheck userKPICheck)
    {
        string strHQL;
        string strKPICheckID;
        string strChartTitle;
        string strDepartCode, strDepartName, strJobTitle;
        DateTime dtStartTime, dtEndTime;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strJobTitle = ShareClass.GetUserJobTitle(strUserCode);

        strKPICheckID = userKPICheck.KPICheckID.ToString();

        dtStartTime = userKPICheck.StartTime;
        dtEndTime = userKPICheck.EndTime;

        strChartTitle = userKPICheck.KPICheckName + LanguageHandle.GetWord("BuMen").ToString().Trim() + strDepartName + LanguageHandle.GetWord("ZhiChen").ToString().Trim() + strJobTitle + LanguageHandle.GetWord("ChengYuanJiXiaoZongPingFenDuiB").ToString().Trim();

        strHQL = "Select UserName as XName,TotalPoint as YNumber From T_UserKPICheck ";
        strHQL += " Where UserCode in (Select UserCode From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'" + " and JobTitle = " + "'" + strJobTitle + "'" + ")";
        strHQL += " and  to_char(StartTime,'yyyymmdd') >= " + "'" + dtStartTime.ToString("yyyyMMdd") + "'" + " and to_char(EndTime,'yyyymmdd') <= " + "'" + dtEndTime.ToString("yyyyMMdd") + "'";
        strHQL += " Order by TotalPoint ASC";

        IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Column&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strHQL);

    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strID, strStatus;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[1].Text;

            LB_KPIID.Text = strID;

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "Select Point,Comment From T_KPILeaderReview Where UserKPIID = " + strID + " and LeaderCode = " + "'" + strUserCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPILeaderReview");
            if (ds.Tables[0].Rows.Count > 0)
            {
                NB_Point.Amount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (strIsMobileDevice == "YES")
                {
                    HT_LeaderSummary.Text = ds.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    HE_LeaderSummary.Text = ds.Tables[0].Rows[0][1].ToString();
                }
            }
            else
            {
                NB_Point.Amount = 100;

                if (strIsMobileDevice == "YES")
                {
                    HT_LeaderSummary.Text = "";
                }
                else
                {
                    HE_LeaderSummary.Text = "";
                }
            }

            //取得KPI评论列表
            LoadKPIReviewList(strID);

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

        string strKPICheckID, strKPILeaderReviewID;
        string strKPIID, strLeaderComment;
        decimal deLeaderPoint;

        strKPICheckID = LB_KPICheckID.Text.Trim();
        strKPIID = LB_KPIID.Text.Trim();

        deLeaderPoint = NB_Point.Amount;
        if (deLeaderPoint > 100)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFSBNDY100JC").ToString().Trim() + "')", true);
            return;
        }

        if (strIsMobileDevice == "YES")
        {
            strLeaderComment = HT_LeaderSummary.Text;
        }
        else
        {
            strLeaderComment = HE_LeaderSummary.Text.Trim();
        }

        strHQL = "From KPILeaderReview as kpiLeaderReview Where kpiLeaderReview.LeaderCode = " + "'" + strUserCode + "'";
        strHQL += " and kpiLeaderReview.UserKPIID = " + strKPIID;
        KPILeaderReviewBLL kpiLeaderReviewBLL = new KPILeaderReviewBLL();
        lst = kpiLeaderReviewBLL.GetAllKPILeaderReviews(strHQL);

        KPILeaderReview kpiLeaderReview = new KPILeaderReview();

        if (lst.Count == 0)
        {
            kpiLeaderReview.UserKPIID = int.Parse(strKPIID);
            kpiLeaderReview.LeaderCode = strUserCode;
            kpiLeaderReview.LeaderName = strUserName;
            kpiLeaderReview.Point = deLeaderPoint;
            kpiLeaderReview.Comment = strLeaderComment;
            kpiLeaderReview.ReviewTime = DateTime.Now;

            try
            {
                kpiLeaderReviewBLL.AddKPILeaderReview(kpiLeaderReview);

                //更改领导评分总分
                UpdateUserKPICheckPoint(strKPICheckID, strKPIID);

                //更改KPI明细总分
                LB_TotalPoint.Text = ShareClass.UpdateKPICheckDetailTotalPoint(strKPICheckID).ToString();

                LoadKPI(strKPICheckID);

                //列出KPI评论列表
                LoadKPIReviewList(strKPIID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            kpiLeaderReview = (KPILeaderReview)lst[0];

            strKPILeaderReviewID = kpiLeaderReview.ID.ToString();

            kpiLeaderReview.UserKPIID = int.Parse(strKPIID);
            kpiLeaderReview.LeaderCode = strUserCode;
            kpiLeaderReview.LeaderName = strUserName;
            kpiLeaderReview.Point = deLeaderPoint;
            kpiLeaderReview.Comment = strLeaderComment;
            kpiLeaderReview.ReviewTime = DateTime.Now;

            try
            {
                kpiLeaderReviewBLL.UpdateKPILeaderReview(kpiLeaderReview, int.Parse(strKPILeaderReviewID));


                //更改领导评分总分
                UpdateUserKPICheckPoint(strKPICheckID, strKPIID);

                //更改KPI明细总分
                LB_TotalPoint.Text = ShareClass.UpdateKPICheckDetailTotalPoint(strKPICheckID).ToString();

                LoadKPI(strKPICheckID);

                //列出KPI评论列表
                LoadKPIReviewList(strKPIID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }

    }

    protected void UpdateUserKPICheckPoint(string strKPICheckID, string strKPICheckDetailID)
    {
        string strHQL;
        string strAverageLeaderPoint, strTotalLeaderPoint;

        DataSet ds;

        strHQL = "Select Avg(Point) From T_KPILeaderReview Where UserKPIID = " + strKPICheckDetailID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPILeaderReview");
        strAverageLeaderPoint = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_UserKPICheckDetail Set LeaderPoint = " + strAverageLeaderPoint + " Where ID = " + strKPICheckDetailID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Select Sum(LeaderPoint * Weight) From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheckDetail");
        strTotalLeaderPoint = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_UserKPICheck Set TotalLeaderPoint = " + strTotalLeaderPoint + " where KPICheckID = " + strKPICheckID;
        ShareClass.RunSqlCommand(strHQL);

        LB_TotalLeaderPoint.Text = strTotalLeaderPoint;
    }

    protected void LoadKPI(string strKPICheckID)
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
}
