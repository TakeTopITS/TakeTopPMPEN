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

public partial class TTSystemExchangeDBServer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "数据交换服务器定义", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadSystemExchangeDB();
        }
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDBName, strDBServerName, strConnectstring, strLoginString, strStatus;
        int intSortNumber;

        strDBName = TB_DBName.Text.Trim();
        strDBServerName = TB_DBServerName.Text.Trim();
        strConnectstring = TB_ConnectString.Text.Trim();
        strLoginString = TB_LoginString.Text.Trim();
        strStatus = DL_Status.SelectedValue;
        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        strConnectstring = strConnectstring.Replace("'", "''");
        strLoginString = strLoginString.Replace("'", "''");

        strHQL = "Insert Into T_SystemExchangeDBServer(DBServerName,DBName,ConnectString,LoginString,Status,SortNumber) ";
        strHQL += " Values (" + "'" + strDBServerName + "'" + "," + "'" + strDBName + "'" + "," + "'" + strConnectstring + "'" + "," + "'" + strLoginString + "'" + "," + "'" + strStatus + "'" + "," + intSortNumber.ToString() + ")";

        //try
        //{
        ShareClass.RunSqlCommand(strHQL);

        LoadSystemExchangeDB();

        BT_Upate.Enabled = true;
        BT_Delete.Enabled = true;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJCG").ToString().Trim() + "')", true);
        //}
        //catch
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        //}
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDBName, strDBServerName, strConnectstring, strLoginString, strStatus;
        int intSortNumber;

        strDBName = TB_DBName.Text.Trim();
        strDBServerName = TB_DBServerName.Text.Trim();
        strConnectstring = TB_ConnectString.Text.Trim();
        strLoginString = TB_LoginString.Text.Trim();
        strStatus = DL_Status.SelectedValue;
        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        strConnectstring = strConnectstring.Replace("'", "''");
        strLoginString = strLoginString.Replace("'", "''");

        strHQL = "Update T_SystemExchangeDBServer Set ";
        strHQL += " DBName =" + "'" + strDBName + "'" + ", ConnectString = " + "'" + strConnectstring + "'" + ",LoginString = " + "'" + strLoginString + "'" + ",Status = " + "'" + strStatus + "'" + ",SortNumber = " + intSortNumber.ToString();
        strHQL += " Where DBServerName = " + "'" + strDBServerName + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSystemExchangeDB();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZXiuGaiLanguageHandleGetWordZ").ToString().Trim() + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true); 
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDBServerName, strConnectstring, strLoginString, strStatus;
        int intSortNumber;

        strDBServerName = TB_DBServerName.Text.Trim();
        strConnectstring = TB_ConnectString.Text.Trim();
        strLoginString = TB_LoginString.Text.Trim();
        strStatus = DL_Status.SelectedValue;
        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        strHQL = "Delete From T_SystemExchangeDBServer  Where DBServerName = " + "'" + strDBServerName + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSystemExchangeDB();

            BT_Upate.Enabled = false;
            BT_Delete.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuLanguageHandleGetWord").ToString().Trim()+"')", true); 
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strDBServerName;

        if (e.CommandName != "Page")
        {
            strDBServerName = ((Button)e.Item.FindControl("BT_DBServerName")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "Select DBServerName,DBName,ConnectString,LoginString,Status,SortNumber From T_SystemExchangeDBServer Where DBServerName = " + "'" + strDBServerName + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemExchangeDBServer");


            TB_DBServerName.Text = ds.Tables[0].Rows[0][0].ToString();
            TB_DBName.Text = ds.Tables[0].Rows[0][1].ToString();
            TB_ConnectString.Text = ds.Tables[0].Rows[0][2].ToString();
            TB_LoginString.Text = ds.Tables[0].Rows[0][3].ToString();
            DL_Status.SelectedValue = ds.Tables[0].Rows[0][4].ToString();
            NB_SortNumber.Amount = int.Parse(ds.Tables[0].Rows[0][5].ToString());

            BT_Upate.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void LoadSystemExchangeDB()
    {
        string strHQL;

        strHQL = "Select DBName,DBServerName,ConnectString,LoginString,Status,SortNumber From T_SystemEXchangeDBServer Order By SortNumber DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemExchangeDBServer");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }


}
