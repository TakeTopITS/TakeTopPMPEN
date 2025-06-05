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

public partial class TTHumanResourcesTotalTable : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","项目人力资源表", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            BindDDLOther();
            LoadProjectMemberScheduleList(ddl_ProjectID.SelectedValue.Trim(), DropDownList1.SelectedValue.Trim());
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

    protected void LoadProjectMemberScheduleList(string strProjectID, string strType)
    {
        string strHQL;
        if (strType.Trim() == LanguageHandle.GetWord("AnGongChong"))
        {
            if (strProjectID.Trim() == "0")
            {
                strHQL = "select C.WorkType,C.NumberUsedTotal,COALESCE(D.NumberAllTotal,0) NumberAllTotal from (select A.WorkType,SUM(NumberUsed) NumberUsedTotal from " +
                    "T_ProjectMemberSchedule A group by A.WorkType) C left join (select B.WorkType,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B group by B.WorkType) D on C.WorkType=D.WorkType";
            }
            else
            {
                strHQL = "select C.WorkType,C.NumberUsedTotal,COALESCE(D.NumberAllTotal,0) NumberAllTotal from (select A.WorkType,SUM(NumberUsed) NumberUsedTotal from " +
                    "T_ProjectMemberSchedule A where A.ProjectID='" + strProjectID.Trim() + "' group by A.WorkType) C left join (select B.WorkType,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B where B.ProjectID='" + strProjectID.Trim() + "' group by B.WorkType) D on C.WorkType=D.WorkType";
            }

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberSchedule");

            DataGrid1.CurrentPageIndex = 0;
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();
            lbl_sql.Text = strHQL;

            Panel_1.Visible = true;
            Panel_2.Visible = false;
        }
        else
        {
            if (strProjectID.Trim() == "0")
            {
                strHQL = "select C.HumanSubgroups,C.NumberUsedTotal,COALESCE(D.NumberAllTotal,0) NumberAllTotal from (select A.HumanSubgroups,SUM(NumberUsed) NumberUsedTotal from " +
                    "T_ProjectMemberSchedule A group by A.HumanSubgroups) C left join (select B.HumanSubgroups,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B group by B.HumanSubgroups) D on C.HumanSubgroups=D.HumanSubgroups";
            }
            else
            {
                strHQL = "select C.HumanSubgroups,C.NumberUsedTotal,COALESCE(D.NumberAllTotal,0) NumberAllTotal from (select A.HumanSubgroups,SUM(NumberUsed) NumberUsedTotal from " +
                    "T_ProjectMemberSchedule A where A.ProjectID='" + strProjectID.Trim() + "' group by A.HumanSubgroups) C left join (select B.HumanSubgroups,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B where B.ProjectID='" + strProjectID.Trim() + "' group by B.HumanSubgroups) D on C.HumanSubgroups=D.HumanSubgroups";
            }

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberSchedule");

            DataGrid2.CurrentPageIndex = 0;
            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();
            lbl_sql.Text = strHQL;

            Panel_1.Visible = false;
            Panel_2.Visible = true;
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadProjectMemberScheduleList(ddl_ProjectID.SelectedValue.Trim(), DropDownList1.SelectedValue.Trim());
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = string.Empty;
                if (DropDownList1.SelectedValue.Trim().Equals(LanguageHandle.GetWord("AnGongChong")))
                {
                    fileName = LanguageHandle.GetWord("XiangMuRenLiZiYuanBiaoAnGongCh") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else
                {
                    fileName = LanguageHandle.GetWord("XiangMuRenLiZiYuanBiaoAnZiZu") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                CreateExcel(getExportBookList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC") + "')", true);
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
        if (DropDownList1.SelectedValue.Trim() == LanguageHandle.GetWord("AnGongChong"))
        {
            if (ddl_ProjectID.SelectedValue.Trim() == "0")
            {
                strHQL = "select C.WorkType '"+LanguageHandle.GetWord("ZhiCheng")+"/"+LanguageHandle.GetWord("GongZhong")+"',C.NumberUsedTotal 'PlannedLaborHours',COALESCE(D.NumberAllTotal,0) 'ReservedLaborHours' from (select A.WorkType,SUM(NumberUsed) NumberUsedTotal from " +   
                    "T_ProjectMemberSchedule A group by A.WorkType) C left join (select B.WorkType,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B group by B.WorkType) D on C.WorkType=D.WorkType";
            }
            else
            {
                strHQL = "select C.WorkType '"+LanguageHandle.GetWord("ZhiCheng")+"/"+LanguageHandle.GetWord("GongZhong")+"',C.NumberUsedTotal 'PlannedLaborHours',COALESCE(D.NumberAllTotal,0) 'ReservedLaborHours' from (select A.WorkType,SUM(NumberUsed) NumberUsedTotal from " +   
                    "T_ProjectMemberSchedule A where A.ProjectID='" + ddl_ProjectID.SelectedValue.Trim() + "' group by A.WorkType) C left join (select B.WorkType,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B where B.ProjectID='" + ddl_ProjectID.SelectedValue.Trim() + "' group by B.WorkType) D on C.WorkType=D.WorkType";
            }
        }
        else
        {
            if (ddl_ProjectID.SelectedValue.Trim() == "0")
            {
                strHQL = "select C.HumanSubgroups 'HumanResourceSubgroup',C.NumberUsedTotal 'PlannedLaborHours',COALESCE(D.NumberAllTotal,0) 'ReservedLaborHours' from (select A.HumanSubgroups,SUM(NumberUsed) NumberUsedTotal from " +   
                    "T_ProjectMemberSchedule A group by A.HumanSubgroups) C left join (select B.HumanSubgroups,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B group by B.HumanSubgroups) D on C.HumanSubgroups=D.HumanSubgroups";
            }
            else
            {
                strHQL = "select C.HumanSubgroups 'HumanResourceSubgroup',C.NumberUsedTotal 'PlannedLaborHours',COALESCE(D.NumberAllTotal,0) 'ReservedLaborHours' from (select A.HumanSubgroups,SUM(NumberUsed) NumberUsedTotal from " +   
                    "T_ProjectMemberSchedule A where A.ProjectID='" + ddl_ProjectID.SelectedValue.Trim() + "' group by A.HumanSubgroups) C left join (select B.HumanSubgroups,SUM(NumberAll) NumberAllTotal " +
                    "from T_ProjectMemberScheduleBase B where B.ProjectID='" + ddl_ProjectID.SelectedValue.Trim() + "' group by B.HumanSubgroups) D on C.HumanSubgroups=D.HumanSubgroups";
            }
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
