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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTSystemAnalystChartRelatedUserSet : System.Web.UI.Page
{
    string strUserCode, strFormType;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strFormType = Request.QueryString["FormType"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (Page.IsPostBack != true)
        {
            LoadAllSystemAnalystChart();
            LoadUserSystemAnalystChart(strUserCode, strFormType);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        string strChartName;

        strChartName = ((Button)e.Item.FindControl("BT_ChartName")).Text.Trim();

        strHQL = "Select * From T_SystemAnalystChartRelatedUser Where UserCode = '" + strUserCode + "' and ChartName = '" + strChartName + "'" + " and FormType='" + strFormType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYCZBNZFTJJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Insert Into T_SystemAnalystChartRelatedUser(UserCode,ChartName,FormType) Values('" + strUserCode + "','" + strChartName + "','" + strFormType + "')";
        ShareClass.RunSqlCommand(strHQL);

        LoadUserSystemAnalystChart(strUserCode, strFormType);
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strID, strChartName;
            string strCommandName;

            strID = e.Item.Cells[0].Text;
            strChartName = e.Item.Cells[1].Text;

            strCommandName = e.CommandName.ToString();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            try
            {
                if (strCommandName == "DELETE")
                {
                    strHQL = "Delete From T_SystemAnalystChartRelatedUser Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadUserSystemAnalystChart(strUserCode, strFormType);
                }

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        int j = 0, intSortNumber;

        try
        {
            for (j = 0; j < DataGrid4.Items.Count; j++)
            {
                strID = DataGrid4.Items[j].Cells[0].Text;

                intSortNumber = int.Parse(((TextBox)(DataGrid4.Items[j].FindControl("TB_SortNumber"))).Text.Trim());

                strHQL = "Update T_SystemAnalystChartRelatedUser Set SortNumber = " + intSortNumber.ToString() + " Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadUserSystemAnalystChart(strUserCode, strFormType);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click222", "reloadPrentPage();", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZBaoCunLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
        }
    }

    protected void BT_AssignToOtherMember_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = string.Format(@"Insert Into T_SystemAnalystChartRelatedUser('{0}',ChartName,FormType,SortNumber)
                 Select UserCode,ChartName,FormType,SortNumber From T_SystemAnalystChartRelatedUser
                 Where UserCode = 'ADMIN' 
	             and ChartName Not in (Select ChartName From T_SystemAnalystChartRelatedUser Where UserCode = '{0}')", strUserCode);

        ShareClass.RunSqlCommand(strHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZFenPaChengGong").ToString().Trim()+"')", true);
    }

    protected void LoadAllSystemAnalystChart()
    {
        string strHQL;

        strHQL = "Select * From T_SystemAnalystChartManagement Where Status = 'YES'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadUserSystemAnalystChart(string strUserCode, string strFormType)
    {
        string strHQL;
        string strSortNumber;

        strHQL = "Select ID,ChartName,SortNumber From T_SystemAnalystChartRelatedUser Where UserCode = " + "'" + strUserCode + "'" + " and FormType = " + "'" + strFormType + "'";
        strHQL += " Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strSortNumber = ds.Tables[0].Rows[i][2].ToString().Trim();
            ((TextBox)DataGrid4.Items[i].FindControl("TB_SortNumber")).Text = strSortNumber;
        }
    }
}
