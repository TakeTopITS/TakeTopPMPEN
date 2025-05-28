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

public partial class TTKPIThirdPartReview : System.Web.UI.Page
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
        _FileBrowser.SetupCKEditor(HE_UserSummary);
HE_UserSummary.Language = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strIsMobileDevice = Session["IsMobileDevice"].ToString();


        strKPICheckID = Request.QueryString["KPICheckID"];

        UserKPICheck userKPICheck = GetUserKPICheck(strKPICheckID);

        //this.Title = "员工: " + userKPICheck.UserName .Trim () +  "绩效" + strKPICheckID + "评核";

        //生成同部门同职称员工绩效评分对比柱状图
        CreateSameDepartmentJobTitleKPIScoringChart(strUserCode, userKPICheck);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_UserSummary.Visible = true;
            }
            else
            {
                HE_UserSummary.Visible = true;
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
        string strChartTitle;
        string strDepartCode, strDepartName, strJobTitle;
        DateTime dtStartTime, dtEndTime;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strJobTitle = ShareClass.GetUserJobTitle(strUserCode);

        dtStartTime = userKPICheck.StartTime;
        dtEndTime = userKPICheck.EndTime;

        strChartTitle = userKPICheck.KPICheckName + LanguageHandle.GetWord("BuMen").ToString().Trim() + strDepartName + LanguageHandle.GetWord("ZhiChen").ToString().Trim() + strJobTitle + LanguageHandle.GetWord("ChengYuanJiXiaoZongPingFenDuiB").ToString().Trim();

        strHQL = "Select UserName as XName,TotalPoint as YNumber From T_UserKPICheck ";
        strHQL += " Where UserCode in (Select UserCode From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'" + " and JobTitle = " + "'" + strJobTitle + "'" + ")";
        strHQL += " and  to_char(StartTime,'yyyymmdd') >= " + "'" + dtStartTime.ToString("yyyyMMdd") + "'" + " and to_char(EndTime,'yyyymmdd') <= " + "'" + dtEndTime.ToString("yyyyMMdd") + "'";
        strHQL += " Order by TotalPoint ASC";
        IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Column&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strHQL);

        //Chart1.Visible = true;
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

            strHQL = "Select Point,Comment From T_KPIThirdPartReview Where UserKPIID = " + strID + " and UserCode = " + "'" + strUserCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPIThirdPartReview");
            if (ds.Tables[0].Rows.Count > 0)
            {
                NB_Point.Amount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (strIsMobileDevice == "YES")
                {
                    HT_UserSummary.Text = ds.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    HE_UserSummary.Text = ds.Tables[0].Rows[0][1].ToString();
                }
            }
            else
            {
                NB_Point.Amount = 100;

                if (strIsMobileDevice == "YES")
                {
                    HT_UserSummary.Text = "";
                }
                else
                {
                    HE_UserSummary.Text = "";
                }
            }

            //列出KPI评论列表
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

        string strKPICheckID, strKPIThirdPartReviewID;
        string strKPIID, strUserComment;
        decimal deUserPoint;

        strKPICheckID = LB_KPICheckID.Text.Trim();
        strKPIID = LB_KPIID.Text.Trim();

        deUserPoint = NB_Point.Amount;
        if (deUserPoint > 100)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFSBNDY100JC").ToString().Trim() + "')", true);
            return;
        }

        if (strIsMobileDevice == "YES")
        {
            strUserComment = HT_UserSummary.Text.Trim();
        }
        else
        {
            strUserComment = HE_UserSummary.Text.Trim();
        }

        if (strUserComment == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGPHYJBNWKJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "From KPIThirdPartReview as kpiThirdPartReview Where kpiThirdPartReview.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and kpiThirdPartReview.UserKPIID = " + strKPIID;
        KPIThirdPartReviewBLL kpiThirdPartReviewBLL = new KPIThirdPartReviewBLL();
        lst = kpiThirdPartReviewBLL.GetAllKPIThirdPartReviews(strHQL);

        KPIThirdPartReview kpiThirdPartReview = new KPIThirdPartReview();

        if (lst.Count == 0)
        {
            kpiThirdPartReview.UserKPIID = int.Parse(strKPIID);
            kpiThirdPartReview.UserCode = strUserCode;
            kpiThirdPartReview.UserName = strUserName;
            kpiThirdPartReview.Point = deUserPoint;
            kpiThirdPartReview.Comment = strUserComment;
            kpiThirdPartReview.ReviewTime = DateTime.Now;

            try
            {
                kpiThirdPartReviewBLL.AddKPIThirdPartReview(kpiThirdPartReview);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            kpiThirdPartReview = (KPIThirdPartReview)lst[0];

            strKPIThirdPartReviewID = kpiThirdPartReview.ID.ToString();

            kpiThirdPartReview.UserKPIID = int.Parse(strKPIID);
            kpiThirdPartReview.UserCode = strUserCode;
            kpiThirdPartReview.UserName = strUserName;
            kpiThirdPartReview.Point = deUserPoint;
            kpiThirdPartReview.Comment = strUserComment;
            kpiThirdPartReview.ReviewTime = DateTime.Now;

            try
            {
                kpiThirdPartReviewBLL.UpdateKPIThirdPartReview(kpiThirdPartReview, int.Parse(strKPIThirdPartReviewID));

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }

        }

        //更改第三方评分总分
        UpdateThirdPartKPICheckPoint(strKPICheckID, strKPIID);

        //更改KPI明细总分
        LB_TotalPoint.Text = ShareClass.UpdateKPICheckDetailTotalPoint(strKPICheckID).ToString();

        LoadKPI(strKPICheckID);

        LoadKPIReviewList(strKPIID);
    }

    protected void UpdateThirdPartKPICheckPoint(string strKPICheckID, string strKPICheckDetailID)
    {
        string strHQL;
        string strAverageThirdPartPoint, strTotalThirdPartPoint;

        DataSet ds;

        strHQL = "Select Avg(Point) From T_KPIThirdPartReview Where UserKPIID = " + strKPICheckDetailID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPIThirdPartReview");
        strAverageThirdPartPoint = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_UserKPICheckDetail Set ThirdPartPoint = " + strAverageThirdPartPoint + " Where ID = " + strKPICheckDetailID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Select Sum(ThirdPartPoint * Weight) From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheckDetail");
        strTotalThirdPartPoint = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_UserKPICheck Set TotalThirdPartPoint = " + strTotalThirdPartPoint + " where KPICheckID = " + strKPICheckID;
        ShareClass.RunSqlCommand(strHQL);

        LB_TotalThirdPartPoint.Text = strTotalThirdPartPoint;
    }

    protected void LoadKPI(string strKPICheckID)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserKPICheckDetail as userKPICheckDetail where userKPICheckDetail.KPICheckID = " + strKPICheckID;
        strHQL += " and userKPICheckDetail.ID in (Select kpiThirdPartReview.UserKPIID From KPIThirdPartReview as kpiThirdPartReview Where kpiThirdPartReview.UserCode = " + "'" + strUserCode + "'" + ")";
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

        strHQL = "From UserKPICheckDetail as userKPICheckDetail Where userKPICheckDetail.ID = " + strKPIID;
        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();
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
