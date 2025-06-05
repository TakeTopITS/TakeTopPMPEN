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

using System.Text;
using System.IO;
using System.Web.Mail;

using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTSystemExchangeOrder : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "数据交换命令定义", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadSystemExchangeDB();
            LoadSystemExchangeOrder();
        }
    }

    protected void DL_Database_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSystemExchangeOrder();
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strHQL, strSqlCode;
        string strID, strDBServerName, strStatus;

        strDBServerName = DL_DBServerName.SelectedValue.Trim();
        strSqlCode = TB_SqlOrderString.Text.Trim();
        strStatus = DL_Status.SelectedValue;

        if (strSqlCode.ToLower().Contains("create") || strSqlCode.ToLower().Contains("execute") || strSqlCode.ToLower().Contains("delete") || strSqlCode.ToLower().Contains("update") || strSqlCode.ToLower().Contains("drop")
          || strSqlCode.ToLower().Contains("insert") || strSqlCode.ToLower().Contains("alter"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEUPDATEDROPINSERTALTERYJJC") + "')", true);
            return;
        }

        strHQL = "Insert Into T_SystemExchangeOrder(DBServerName,SqlOrderString,Status,CreatorCode,CreatorName) ";
        strHQL += " Values(" + "'" + strDBServerName + "'" + "," + "'" + strSqlCode + "'" + "," + "'" + strStatus + "'" + "," + "'" + strUserCode + "'" + "," + "'" + strUserName + "'" + ")";


        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strID = ShareClass.GetMyCreatedSystemExchangeDBSqlOrderID();

            LB_ID.Text = strID;

            LoadSystemExchangeOrder();

            BT_Upate.Enabled = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL, strSqlCode;
        string strID, strDBServerName, strStatus;

        strID = LB_ID.Text.Trim();

        strDBServerName = DL_DBServerName.SelectedValue.Trim();
        strSqlCode = TB_SqlOrderString.Text.Trim();
        strStatus = DL_Status.SelectedValue;


        if (strSqlCode.ToLower().Contains("create") || strSqlCode.ToLower().Contains("execute") || strSqlCode.ToLower().Contains("delete") || strSqlCode.ToLower().Contains("update") || strSqlCode.ToLower().Contains("drop")
          || strSqlCode.ToLower().Contains("insert") || strSqlCode.ToLower().Contains("alter"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEUPDATEDROPINSERTALTERYJJC") + "')", true);
            return;
        }

        strHQL = "Update T_SystemExchangeOrder Set ";
        strHQL += " DBServerName = " + "'" + strDBServerName + "'" + ",SqlOrderString = " + "'" + strSqlCode + "'" + ",Status = " + "'" + strStatus + "'";
        strHQL += " Where ID = " + strID;


        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSystemExchangeOrder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZXiuGaiLanguageHandleGetWordZ") + LanguageHandle.GetWord("ZZBCSB") + "')", true); 
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID;

        strID = LB_ID.Text.Trim();

        strHQL = "Delete From T_SystemExchangeOrder Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSystemExchangeOrder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuLanguageHandleGetWord")+"')", true); 
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strID;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "Select ID, SqlOrderString,Status From T_SystemExchangeOrder Where ID = " + strID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemExchangeOrder");

            LB_ID.Text = ds.Tables[0].Rows[0][0].ToString();
            TB_SqlOrderString.Text = ds.Tables[0].Rows[0][1].ToString();
            DL_Status.SelectedValue = ds.Tables[0].Rows[0][2].ToString();

            BT_Upate.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void LoadSystemExchangeDB()
    {
        string strHQL;

        strHQL = "Select DBServerName,DBName From T_SystemEXchangeDBServer Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemExchangeDBServer");

        DL_DBServerName.DataSource = ds;
        DL_DBServerName.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            LB_DBName.Text = ds.Tables[0].Rows[0][1].ToString();
        }
    }

    protected void LoadSystemExchangeOrder()
    {
        string strHQL;

        string strDBServerName = DL_DBServerName.SelectedValue;

        strHQL = "Select ID,DBServerName,SqlOrderString,CreatorCode,CreatorName,Status From T_SystemExchangeOrder Where DBServerName = " + "'" + strDBServerName + "'";
        strHQL += " Order By ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemExchangeOrder");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}
