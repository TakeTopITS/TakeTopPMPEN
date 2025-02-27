using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;

public partial class TTProjectHumanResourcesMonthlyReports : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","项目人力资源月报", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSNYBNWKCZSBJC").ToString().Trim()+"')", true);
                DLC_YearMonth.Focus();
                return;
            }
            else
            {
                strHQL = "select A.*,COALESCE(B.WeekTotal,0) WeekTotal from (select WorkType," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=1 then NumberUsed else 0 end) WeekTotal1 ," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=2 then NumberUsed else 0 end) WeekTotal2 ," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=3 then NumberUsed else 0 end) WeekTotal3 ," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=4 then NumberUsed else 0 end) WeekTotal4 from " +
                    "T_ProjectMemberSchedule group by WorkType) A " +
                    "left join " +
                    "(select WorkType, sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) WeekTotal from " +
                    "T_ProjectMemberScheduleBase group by WorkType) B on A.WorkType=B.WorkType";
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSNYBNWKCZSBJC").ToString().Trim()+"')", true);
                DLC_YearMonth.Focus();
                return;
            }
            else
            {
                strHQL = "select WorkType, sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=1 then NumberUsed else 0 end) WeekTotal1 ," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=2 then NumberUsed else 0 end) WeekTotal2 ," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=3 then NumberUsed else 0 end) WeekTotal3 ," +
                    " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=4 then NumberUsed else 0 end) WeekTotal4  from " +
                    "T_ProjectMemberSchedule where ProjectID='" + strProjectID + "' group by WorkType";
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
                    fileName = LanguageHandle.GetWord("QuanTiXiangMuHuiZongYueDuiZhao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else
                {
                    fileName = LanguageHandle.GetWord("XiangMuRenLiZiYuanYueBao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                CreateExcel(getExportBookList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim()+"')", true);
            }
        }
    }

    private void CreateExcel(DataTable dt, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }

    protected DataTable getExportBookList()
    {
        string strHQL;
        string strYearMonth = DLC_YearMonth.Text.Trim() == "" ? DateTime.Now.ToString("yyyy-MM") : DLC_YearMonth.Text.Trim();
        if (ddl_ProjectID.SelectedValue.Trim() == "0")
        {
            strHQL = "select A.WorkType '工种',A.WeekTotal1 '第一周',A.WeekTotal2 '第二周',A.WeekTotal3 '第三周',A.WeekTotal4 '第四周',COALESCE(B.WeekTotal,0) '储备量' from (select WorkType," + 
                " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=1 then NumberUsed else 0 end) WeekTotal1 ," +
                " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=2 then NumberUsed else 0 end) WeekTotal2 ," +
                " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=3 then NumberUsed else 0 end) WeekTotal3 ," +
                " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=4 then NumberUsed else 0 end) WeekTotal4 from " +
                "T_ProjectMemberSchedule group by WorkType) A " +
                "left join " +
                "(select WorkType, sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) then NumberAll else 0 end) WeekTotal  from " +
                "T_ProjectMemberScheduleBase group by WorkType) B on A.WorkType=B.WorkType";
        }
        else
        {
            strHQL = "select WorkType '工种', sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=1 then NumberUsed else 0 end) '第一周' ," + 
                " sum(case when '" + strYearMonth.Trim() + LanguageHandle.GetWord("SUBSTRINGtocharYearMonthyyyymm").ToString().Trim() +
                " sum(case when '" + strYearMonth.Trim() + LanguageHandle.GetWord("SUBSTRINGtocharYearMonthyyyymm").ToString().Trim() +
                " sum(case when '" + strYearMonth.Trim() + "' = SUBSTRING(to_char(YearMonth,'yyyy-mm-dd'),0,8) and WeekNum=4 then NumberUsed else 0 end) '第四周' from " + 
                "T_ProjectMemberSchedule where ProjectID='" + ddl_ProjectID.SelectedValue.Trim() + "' group by WorkType";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberSchedule");
        return ds.Tables[0];
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
