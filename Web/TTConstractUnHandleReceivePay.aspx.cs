using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTConstractUnHandleReceivePay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "合同收付款预警", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "合同收付款预警";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadConstractReceivables(strUserCode);
            LoadConstractPayable(strUserCode);

            LB_ConstractReceivableCurrencyType.Text = ShareClass.GetHomeCurrencyType();
            LB_ConstractPayableCurrencyType.Text = ShareClass.GetHomeCurrencyType();
        }
    }

    protected void BT_FindReceiverFind_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strSalesName, strPayer, strPartBOperator, strConstractCode;
        decimal deReceivablesAccount = 0, deReceiverAccount = 0, deReceiverInvoiceAccount = 0, deUNReceiveAmount = 0;

        strUserCode = Session["UserCode"].ToString();

        strConstractCode = TB_ReceiverConstractCode.Text.Trim();

        strSalesName = TB_ReceiverSalesName.Text.Trim();
        strPayer = TB_Payer.Text.Trim();
        strPartBOperator = TB_PartBOperator.Text.Trim();

        strConstractCode = "%" + strConstractCode + "%";


        strSalesName = "%" + strSalesName + "%";
        strPayer = "%" + strPayer + "%";
        strPartBOperator = "%" + strPartBOperator + "%";

        strHQL = "Select * from T_ConstractReceivables as constractReceivables where constractReceivables.Status not in ('Completed','Cancel') and to_char( constractReceivables.ReceivablesTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') ";
        strHQL += " and constractReceivables.ConstractCode in ( Select constractRelatedUser.ConstractCode from T_ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and constractReceivables.Payer Like " + "'" + strPayer + "'";
        strHQL += " and constractReceivables.ConstractCode Like " + "'" + strConstractCode + "'";
        if (strSalesName != "%%")
        {
            strHQL += " and constractReceivables.ConstractCode in (Select constractSales.ConstractCode From T_ConstractSales as constractSales Where constractSales.SalesName Like " + "'" + strSalesName + "'" + ")";
        }
        strHQL += " and constractReceivables.ConstractCode in (Select constract.ConstractCode from T_Constract as constract where constract.PartBOperator Like " + "'" + strPartBOperator + "'" + ")";
        strHQL += " and constractReceivables.ConstractCode not in (Select constract.ConstractCode from T_Constract as constract where constract.Status in ('Archived','Cancel','Deleted'))";

        strHQL += " and constractReceivables.ConstractCode != ''";
        strHQL += " and constractReceivables.UNReceiveAmount > 0";
        strHQL += " Order by constractReceivables.ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "TconstractReceivables");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            deReceivablesAccount += decimal.Parse(ds.Tables[0].Rows[i]["ReceivablesAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            deReceiverAccount += decimal.Parse(ds.Tables[0].Rows[i]["ReceiverAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString()); 
            deReceiverInvoiceAccount += decimal.Parse(ds.Tables[0].Rows[i]["InvoiceAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString()); 
            deUNReceiveAmount += decimal.Parse(ds.Tables[0].Rows[i]["UNReceiveAmount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString()); 
        }

        LB_ReceivablesAccount.Text = deReceivablesAccount.ToString();
        LB_ReceiverAccount.Text = deReceiverAccount.ToString();
        LB_ReceiverInvoiceAccount.Text = deReceiverInvoiceAccount.ToString();
        LB_UNReceiveAmount.Text = deUNReceiveAmount.ToString();

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_FindPayerFind_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strSalesName, strReceiver, strPartAOperator, strConstractCode;
        decimal dePayableAccount = 0, deOutOfPocketAccount = 0, dePayerInvoiceAccount = 0, deUNPayAmount = 0;

        strUserCode = Session["UserCode"].ToString();

        strConstractCode = "%" + TB_PayerConstractCode.Text.Trim() + "%";
        strSalesName = "%" + TB_PayerSalesName.Text.Trim() + "%";
        strReceiver = "%" + TB_Receiver.Text.Trim() + "%";
        strPartAOperator = "%" + TB_PartAOperator.Text.Trim() + "%";

        strHQL = "Select * from T_ConstractPayable as constractPayable where constractPayable.Status not in ('Completed','Cancel') and to_char(constractPayable.PayableTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd')";
        strHQL += " and constractPayable.ConstractCode in ( Select constractRelatedUser.ConstractCode from T_ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and constractPayable.Receiver Like " + "'" + strReceiver + "'";
        strHQL += " and constractPayable.ConstractCode Like " + "'" + strConstractCode + "'";

        if (strSalesName != "%%")
        {
            strHQL += " and constractPayable.ConstractCode in (Select constractSales.ConstractCode From T_ConstractSales as constractSales Where constractSales.SalesName Like " + "'" + strSalesName + "'" + ")";
        }
        strHQL += " and constractPayable.ConstractCode in (Select constract.ConstractCode from T_Constract as constract where constract.PartAOperator Like " + "'" + strPartAOperator + "'" + ")";
        strHQL += " and constractPayable.ConstractCode not in (Select constract.ConstractCode from T_Constract as constract where constract.Status  in ('Archived','Cancel','Deleted'))";
        strHQL += " and constractPayable.ConstractCode != ''";
        strHQL += " and constractPayable.UNPayAmount > 0";
        strHQL += " Order by constractPayable.ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractPayable");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            dePayableAccount += decimal.Parse(ds.Tables[0].Rows[i]["PayableAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            deOutOfPocketAccount += decimal.Parse(ds.Tables[0].Rows[i]["OutOfPocketAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString()); 
            dePayerInvoiceAccount += decimal.Parse(ds.Tables[0].Rows[i]["InvoiceAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString()); 
            deUNPayAmount += decimal.Parse(ds.Tables[0].Rows[i]["UNPayAmount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString()); 
        }

        LB_PayableAccount.Text = dePayableAccount.ToString();
        LB_OutOfPocketAccount.Text = deOutOfPocketAccount.ToString();
        LB_PayerInvoiceAccount.Text = dePayerInvoiceAccount.ToString();
        LB_UNPayAmount.Text = deUNPayAmount.ToString();

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void LoadConstractReceivables(string strUserCode)
    {
        string strHQL;
        IList lst;

        decimal deReceivablesAccount = 0, deReceiverAccount = 0, deReceiverInvoiceAccount = 0, deUNReceiveAmount = 0;


        strHQL = "Select * from T_ConstractReceivables as constractReceivables where constractReceivables.Status not in ('Completed','Cancel') and to_char( constractReceivables.ReceivablesTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') ";
        strHQL += " and constractReceivables.ConstractCode in ( Select constractRelatedUser.ConstractCode from T_ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and constractReceivables.ConstractCode not in (Select constract.ConstractCode from T_Constract as constract where constract.Status in ('Archived','Cancel','Deleted'))";
        strHQL += " and constractReceivables.ConstractCode != ''";
        strHQL += " and constractReceivables.UNReceiveAmount > 0";
        strHQL += " Order by constractReceivables.ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "TconstractReceivables");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            deReceivablesAccount += decimal.Parse(ds.Tables[0].Rows[i]["ReceivablesAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            deReceiverAccount += decimal.Parse(ds.Tables[0].Rows[i]["ReceiverAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            deReceiverInvoiceAccount += decimal.Parse(ds.Tables[0].Rows[i]["InvoiceAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            deUNReceiveAmount += decimal.Parse(ds.Tables[0].Rows[i]["UNReceiveAmount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
        }

        LB_ReceivablesAccount.Text = deReceivablesAccount.ToString();
        LB_ReceiverAccount.Text = deReceiverAccount.ToString();
        LB_ReceiverInvoiceAccount.Text = deReceiverInvoiceAccount.ToString();
        LB_UNReceiveAmount.Text = deUNReceiveAmount.ToString();

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

    }

    protected void LoadConstractPayable(string strUserCode)
    {
        string strHQL;
        IList lst;

        decimal dePayableAccount = 0, deOutOfPocketAccount = 0, dePayerInvoiceAccount = 0, deUNPayAmount = 0;

        strHQL = "Select * from T_ConstractPayable as constractPayable where constractPayable.Status not in ('Completed','Cancel') and to_char(constractPayable.PayableTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd')";
        strHQL += " and constractPayable.ConstractCode in ( Select constractRelatedUser.ConstractCode from T_ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and constractPayable.ConstractCode not in (Select constract.ConstractCode from T_Constract as constract where constract.Status  in ('Archived','Cancel','Deleted'))";
        strHQL += " and constractPayable.ConstractCode != ''";
        strHQL += " and constractPayable.UNPayAmount > 0";
        strHQL += " Order by constractPayable.ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractPayable");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            dePayableAccount += decimal.Parse(ds.Tables[0].Rows[i]["PayableAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            deOutOfPocketAccount += decimal.Parse(ds.Tables[0].Rows[i]["OutOfPocketAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            dePayerInvoiceAccount += decimal.Parse(ds.Tables[0].Rows[i]["InvoiceAccount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
            deUNPayAmount += decimal.Parse(ds.Tables[0].Rows[i]["UNPayAmount"].ToString()) * decimal.Parse(ds.Tables[0].Rows[i]["ExchangeRate"].ToString());
        }

        LB_PayableAccount.Text = dePayableAccount.ToString();
        LB_OutOfPocketAccount.Text = deOutOfPocketAccount.ToString();
        LB_PayerInvoiceAccount.Text = dePayerInvoiceAccount.ToString();
        LB_UNPayAmount.Text = deUNPayAmount.ToString();

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }


}
