using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;

public partial class TTProjectHumanResourcePlan : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","项目人力资源计划", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_YearMonth.Text = DateTime.Now.ToString("yyyy-MM");
            BindDDLOther();
            LoadProjectMemberScheduleList(ddl_ProjectID.SelectedValue.Trim(), DLC_YearMonth.Text.Trim());
        }
    }

    protected void BindDDLOther()
    {
        string strHQL = "Select ProjectID, (cast(ProjectID as char(10)) || '.' || ProjectName) as ProjectName from T_Project Order by ProjectID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        ddl_ProjectID.DataSource = ds;
        ddl_ProjectID.DataBind();
        ddl_ProjectID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadProjectMemberScheduleList(string strProjectID, string strYearMonth)
    {
        string strHQL;

        if (strProjectID.Trim() == "0")
        {
            if (strYearMonth.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSNYBNWKCZSBJC").ToString().Trim() + "')", true);
                DLC_YearMonth.Focus();
                return;
            }
            else
            {
                DateTime dt = DateTime.Parse(strYearMonth.Trim());
                Label14.Text = dt.ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label15.Text = dt.AddMonths(1).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label16.Text = dt.AddMonths(2).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label17.Text = dt.AddMonths(3).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label18.Text = dt.AddMonths(4).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label19.Text = dt.AddMonths(5).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label20.Text = dt.AddMonths(6).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label21.Text = dt.AddMonths(7).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label22.Text = dt.AddMonths(8).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label23.Text = dt.AddMonths(9).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label24.Text = dt.AddMonths(10).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label25.Text = dt.AddMonths(11).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label26.Text = dt.AddMonths(12).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                strHQL = "select * from (select WorkType," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'1 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal1 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'2 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal2 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'3 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal3 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'4 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal4 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'5 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal5 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'6 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal6 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'7 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal7 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'8 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal8 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'9 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal9 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'10 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal10 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'11 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal11 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'12 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal12  " +
                    "from T_ProjectMemberSchedule group by WorkType " +
                    "union all " +
                    "select WorkType ||'储备量' WorkType, sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal ," + 
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'1 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal1 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'2 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal2 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'3 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal3 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'4 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal4 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'5 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal5 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'6 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal6 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'7 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal7 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'8 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal8 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'9 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal9 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'10 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal10 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'11 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal11 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'12 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal12 " +
                    "from T_ProjectMemberScheduleBase group by WorkType) A order by WorkType";
            }

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberSchedule");

            DataGrid2.CurrentPageIndex = 0;
            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();
            lbl_sql.Text = strHQL;

            Panel_1.Visible = false;
            Panel_2.Visible = true;
        }
        else
        {
            if (strYearMonth.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSNYBNWKCZSBJC").ToString().Trim() + "')", true);
                DLC_YearMonth.Focus();
                return;
            }
            else
            {
                DateTime dt = DateTime.Parse(strYearMonth.Trim());
                Label1.Text = dt.ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label2.Text = dt.AddMonths(1).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label3.Text = dt.AddMonths(2).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label4.Text = dt.AddMonths(3).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label5.Text = dt.AddMonths(4).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label6.Text = dt.AddMonths(5).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label7.Text = dt.AddMonths(6).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label8.Text = dt.AddMonths(7).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label9.Text = dt.AddMonths(8).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label10.Text = dt.AddMonths(9).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label11.Text = dt.AddMonths(10).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label12.Text = dt.AddMonths(11).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                Label13.Text = dt.AddMonths(12).ToString(LanguageHandle.GetWord("yyyyNianMMYue").ToString().Trim());
                strHQL = "select WorkType," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'1 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal1 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'2 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal2 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'3 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal3 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'4 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal4 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'5 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal5 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'6 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal6 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'7 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal7 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'8 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal8 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'9 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal9 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'10 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal10 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'11 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal11 ," +
                    " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'12 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal12 = " +
                    "from T_ProjectMemberSchedule where ProjectID='" + strProjectID.Trim() + "' group by WorkType";
            }

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberSchedule");

            DataGrid1.CurrentPageIndex = 0;
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();
            lbl_sql.Text = strHQL;

            Panel_1.Visible = true;
            Panel_2.Visible = false;
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadProjectMemberScheduleList(ddl_ProjectID.SelectedValue.Trim(), DLC_YearMonth.Text.Trim());
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = string.Empty;
                if (ddl_ProjectID.SelectedValue.Trim() == "0")
                {
                    fileName = LanguageHandle.GetWord("QuanTiXiangMuHuiZongJiHuaDuiZh").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else
                {
                    fileName = LanguageHandle.GetWord("XiangMuRenLiZiYuanZongJiHua").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }

                string strHQL;
                string strYearMonth = DLC_YearMonth.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM") : DLC_YearMonth.Text.Trim();
                DateTime dt = DateTime.Parse(DLC_YearMonth.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM") : DLC_YearMonth.Text.Trim());
                if (ddl_ProjectID.SelectedValue.Trim() == "0")
                {
                    strHQL = "select * from (select WorkType," +
                        " sum(case when '" + strYearMonth + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'1 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal1 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'2 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal2 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'3 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal3 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'4 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal4 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'5 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal5 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'6 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal6 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'7 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal7 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'8 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal8 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'9 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal9 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'10 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal10 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'11 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal11 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'12 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal12 " +
                        "from T_ProjectMemberSchedule group by WorkType " +
                        "union all " +
                        "select WorkType, sum(case when '" + strYearMonth + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'1 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal1 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'2 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal2 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'3 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal3 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'4 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal4 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'5 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal5 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'6 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal6 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'7 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal7 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'8 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal8 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'9 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal9 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'10 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal10 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'11 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal11 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'12 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) MonthTotal12 " +
                        "from T_ProjectMemberScheduleBase group by WorkType) A order by WorkType";
                }
                else
                {
                    strHQL = "select WorkType ," +
                        " sum(case when '" + strYearMonth + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'1 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal1 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'2 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal2 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'3 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal3 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'4 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal4 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'5 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal5 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'6 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal6 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'7 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal7 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'8 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal8 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'9 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal9 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'10 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal10 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'11 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal11 ," +
                        " sum(case when SUBSTRING(to_char('" + dt + "'::timestamp+'12 month'::interval,'yyyy-mm-dd'),0,8) = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberUsed else 0 end) MonthTotal12 " +
                        "from T_ProjectMemberSchedule where ProjectID='" + ddl_ProjectID.SelectedValue.Trim() + "' group by WorkType";
                }

                MSExcelHandler.DataTableToExcel(strHQL, fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberSchedule");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberSchedule");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}
