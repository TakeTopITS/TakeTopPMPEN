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

public partial class TTReceivablesPayableAlertSAAS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strDepartString;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ո���Ԥ��", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadConstractReceivables(strUserCode);
            LoadConstractPayable(strUserCode);
        }
    }

    protected void BT_FindReceiverFind_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode, strPayer;
        decimal deReceivablesAccount = 0, deReceiverAccount = 0, deReceiverInvoiceAccount = 0, deUNReceiveAmount = 0;

        strUserCode = Session["UserCode"].ToString();

        strPayer = TB_Payer.Text.Trim();
        strPayer = "%" + strPayer + "%";
      
        strHQL = "Select * from T_ConstractReceivables as constractReceivables where constractReceivables.Status not in ('Completed','Cancel') and to_char( constractReceivables.ReceivablesTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') ";
        strHQL += " and constractReceivables.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and constractReceivables.Payer Like " + "'" + strPayer + "'";
        strHQL += " and constractReceivables.ConstractCode = ''";
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

        string strUserCode, strReceiver;
        decimal dePayableAccount = 0, deOutOfPocketAccount = 0, dePayerInvoiceAccount = 0, deUNPayAmount = 0;

        strUserCode = Session["UserCode"].ToString();

        strReceiver = "%" + TB_Receiver.Text.Trim() + "%";

       
        strHQL = "Select * from T_ConstractPayable as constractPayable where constractPayable.Status not in ('Completed','Cancel') and to_char(constractPayable.PayableTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd')";
        strHQL += " and constractPayable.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and constractPayable.Receiver Like " + "'" + strReceiver + "'";
        strHQL += " and constractPayable.ConstractCode = ''";
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
        strHQL += " and constractReceivables.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and constractReceivables.ConstractCode = ''";
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
        strHQL += " and constractPayable.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and constractPayable.ConstractCode = ''";
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
