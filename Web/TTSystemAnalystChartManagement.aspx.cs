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
using System.Web.Mail;
using System.Data.OleDb;
using System.Data.SqlClient;



using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using Npgsql;

public partial class TTSystemAnalystChartManagement : System.Web.UI.Page
{
    string strUserCode;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "分析图形设计", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadSystemAnalystChart();

            lbl_Departstring.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From SystemAnalystChartManagement as systemAnalystChartManagement Where systemAnalystChartManagement.ID = " + strID;
            SystemAnalystChartManagementBLL systemAnalystChartManagementBLL = new SystemAnalystChartManagementBLL();
            SystemAnalystChartManagement systemAnalystChartManagement = new SystemAnalystChartManagement();

            lst = systemAnalystChartManagementBLL.GetAllSystemAnalystChartManagements(strHQL);
            systemAnalystChartManagement = (SystemAnalystChartManagement)lst[0];

            LB_ChartID.Text = strID;
            TB_ChartName.Text = systemAnalystChartManagement.ChartName;
            DL_ChartType.SelectedValue = systemAnalystChartManagement.ChartType.Trim();
            TB_LinkAddress.Text = systemAnalystChartManagement.LinkURL;
            TB_SQLCode.Text = systemAnalystChartManagement.SqlCode;
            DL_IsStartup.SelectedValue = systemAnalystChartManagement.Status.Trim();


            BT_UpdateChart.Enabled = true;
            BT_DeleteChart.Enabled = true;

            string strChartType, strChartName;
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

            strChartType = DL_ChartType.SelectedValue.Trim();
            strChartName = TB_ChartName.Text.Trim();

            strHQL = TB_SQLCode.Text.Trim().Replace("[TAKETOPUSERCODE]", strUserCode).Replace("[TAKETOPDEPARTSTRING]", strDepartString).Replace("[TAKETOPLANGCODE]", strLangCode);
            LoadSytemChart(RP_ChartList, strChartName);
        }
    }

    protected void BT_TestCode_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strChartType, strChartName;

        string strDepartString = lbl_Departstring.Text.Trim();

        strChartType = DL_ChartType.SelectedValue.Trim();
        strChartName = TB_ChartName.Text.Trim();


        strHQL = TB_SQLCode.Text.Trim().Replace("[TAKETOPUSERCODE]", strUserCode).Replace("[TAKETOPDEPARTSTRING]", strDepartString).Replace("[TAKETOPLANGCODE]", strLangCode);

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDMZ").ToString().Trim() + "')", true);

            LoadSytemChart(RP_ChartList, strChartName);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDMYCJC").ToString().Trim() + "')", true);
        }
    }

    public void LoadSytemChart(Repeater RP_ChartList,string strChartName)
    {
        string strLangCode = HttpContext.Current.Session["LangCode"].ToString();
        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        string strHQL = "Select * From T_SystemAnalystChartManagement Where ChartName = '" + strChartName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");
        RP_ChartList.DataSource = ds;
        RP_ChartList.DataBind();
    }

    protected void BT_AddChart_Click(object sender, EventArgs e)
    {
        string strChartName, strChartType, strLinkURL, strSqlCode, strStatus;

        strChartType = DL_ChartType.SelectedValue.Trim();
        strChartName = TB_ChartName.Text.Trim();
        strLinkURL = TB_LinkAddress.Text.Trim();
        strStatus = DL_IsStartup.SelectedValue.Trim();
        strSqlCode = string.Format(TB_SQLCode.Text.Trim());

        if (!string.IsNullOrEmpty(TB_ChartName.Text.Trim()) && TB_ChartName.Text.Trim().Contains("@"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTSGNBNBHZFJC").ToString().Trim() + "')", true);
            TB_ChartName.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TB_SQLCode.Text.Trim()) || TB_SQLCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLYJBNWKJC").ToString().Trim() + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (!(TB_SQLCode.Text.Trim().ToLower().Contains("select") && TB_SQLCode.Text.Trim().ToLower().Contains("from") && TB_SQLCode.Text.Trim().ToLower().Contains("t_") && TB_SQLCode.Text.Trim().ToUpper().Contains("[TAKETOPUSERCODE]")))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC").ToString().Trim() + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (TB_SQLCode.Text.Trim().ToLower().Contains("create ") || TB_SQLCode.Text.Trim().ToLower().Contains("execute ") || TB_SQLCode.Text.Trim().ToLower().Contains("delete ") || TB_SQLCode.Text.Trim().ToLower().Contains("update") || TB_SQLCode.Text.Trim().ToLower().Contains("drop ")
            || TB_SQLCode.Text.Trim().ToLower().Contains("insert ") || TB_SQLCode.Text.Trim().ToLower().Contains("alter "))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC").ToString().Trim() + "')", true);
            TB_SQLCode.Focus();
            return;
        }


        SystemAnalystChartManagementBLL systemAnalystChartManagementBLL = new SystemAnalystChartManagementBLL();
        SystemAnalystChartManagement systemAnalystChartManagement = new SystemAnalystChartManagement();
        systemAnalystChartManagement.ChartType = strChartType;
        systemAnalystChartManagement.ChartName = strChartName;
        systemAnalystChartManagement.SqlCode = strSqlCode;
        systemAnalystChartManagement.LinkURL = strLinkURL;


        systemAnalystChartManagement.Status = strStatus;

        try
        {
            systemAnalystChartManagementBLL.AddSystemAnalystChartManagement(systemAnalystChartManagement);

            LB_ChartID.Text = ShareClass.GetMyCreatedMaxSystemAnalystChartID();

            BT_UpdateChart.Enabled = true;
            BT_DeleteChart.Enabled = true;

            LoadSystemAnalystChart();

            string strDepartString = lbl_Departstring.Text.Trim();
            string strHQL = TB_SQLCode.Text.Trim().Replace("[TAKETOPUSERCODE]", strUserCode).Replace("[TAKETOPDEPARTSTRING]", strDepartString).Replace("[TAKETOPLANGCODE]", strLangCode);
            LoadSytemChart(RP_ChartList, strChartName);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UpdateChart_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strChartName, strChartType, strLinkURL, strSqlCode, strStatus;

        strID = LB_ChartID.Text.Trim();
        strChartType = DL_ChartType.SelectedValue.Trim();
        strChartName = TB_ChartName.Text.Trim();
        strLinkURL = TB_LinkAddress.Text.Trim();
        strStatus = DL_IsStartup.SelectedValue.Trim();
        strSqlCode = string.Format(TB_SQLCode.Text.Trim());


        if (!string.IsNullOrEmpty(TB_ChartName.Text.Trim()) && TB_ChartName.Text.Trim().Contains("@"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTSGNBNBHZFJC").ToString().Trim() + "')", true);
            TB_ChartName.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TB_SQLCode.Text.Trim()) || TB_SQLCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLYJBNWKJC").ToString().Trim() + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (!(TB_SQLCode.Text.Trim().ToLower().Contains("select") && TB_SQLCode.Text.Trim().ToLower().Contains("from") && TB_SQLCode.Text.Trim().ToLower().Contains("t_") && (TB_SQLCode.Text.Trim().ToUpper().Contains("[TAKETOPUSERCODE]") | TB_SQLCode.Text.Trim().ToUpper().Contains("[TAKETOPDEPARTSTRING]"))))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC").ToString().Trim() + LanguageHandle.GetWord("ZZYGMYBHYHDMHBMC").ToString().Trim() + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (TB_SQLCode.Text.Trim().ToLower().Contains("create ") || TB_SQLCode.Text.Trim().ToLower().Contains("execute ") || TB_SQLCode.Text.Trim().ToLower().Contains("delete ") || TB_SQLCode.Text.Trim().ToLower().Contains("update") || TB_SQLCode.Text.Trim().ToLower().Contains("drop ")
            || TB_SQLCode.Text.Trim().ToLower().Contains("insert ") || TB_SQLCode.Text.Trim().ToLower().Contains("alter "))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC").ToString().Trim() + "')", true);
            TB_SQLCode.Focus();
            return;
        }

        strHQL = "From SystemAnalystChartManagement as systemAnalystChartManagement Where systemAnalystChartManagement.ID = " + strID;
        SystemAnalystChartManagementBLL systemAnalystChartManagementBLL = new SystemAnalystChartManagementBLL();
        SystemAnalystChartManagement systemAnalystChartManagement = new SystemAnalystChartManagement();

        lst = systemAnalystChartManagementBLL.GetAllSystemAnalystChartManagements(strHQL);
        systemAnalystChartManagement = (SystemAnalystChartManagement)lst[0];

        systemAnalystChartManagement.ChartType = DL_ChartType.SelectedValue;
        systemAnalystChartManagement.ChartName = TB_ChartName.Text.Trim();
        systemAnalystChartManagement.SqlCode = TB_SQLCode.Text.Trim();
        systemAnalystChartManagement.Status = DL_IsStartup.SelectedValue;
        systemAnalystChartManagement.LinkURL = TB_LinkAddress.Text.Trim();


        //try
        //{
        systemAnalystChartManagementBLL.UpdateSystemAnalystChartManagement(systemAnalystChartManagement, int.Parse(strID));
        LoadSystemAnalystChart();


        string strDepartString = lbl_Departstring.Text.Trim();
        strHQL = TB_SQLCode.Text.Trim().Replace("[TAKETOPUSERCODE]", strUserCode).Replace("[TAKETOPDEPARTSTRING]", strDepartString).Replace("[TAKETOPLANGCODE]", strLangCode);
        LoadSytemChart(RP_ChartList, strChartName);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        //}
        //catch
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        //}
    }

    protected void BT_DeleteChart_Click(object sender, EventArgs e)
    {

        string strHQL;
        string strID, strChartName, strChartType, strSqlCode, strStatus;

        strID = LB_ChartID.Text.Trim();
        strChartType = DL_ChartType.SelectedValue.Trim();
        strChartName = TB_ChartName.Text.Trim();
        strStatus = DL_IsStartup.SelectedValue.Trim();
        strSqlCode = TB_SQLCode.Text.Trim();


        try
        {
            strHQL = "Delete From T_SystemAnalystChartManagement Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            BT_UpdateChart.Enabled = false;
            BT_DeleteChart.Enabled = false;

            LB_ChartID.Text = "";

            LoadSystemAnalystChart();
        }
        catch
        {
        }
    }

    protected void LoadSystemAnalystChart()
    {
        string strHQL;

        strHQL = "Select * From T_SystemAnalystChartManagement";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

}
