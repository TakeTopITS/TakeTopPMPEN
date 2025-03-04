using System; using System.Resources;
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

public partial class TTConstractDetailReport : System.Web.UI.Page
{
    string strLikeUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;

        string strDepartString;

        string strUserName;
        string strUserCode = Session["UserCode"].ToString();

        strLikeUserCode = "%" + strUserCode + "%";


        //this.Title = "合同利润报表";

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strStartTime, strEndTime;
        string strConstractName, strConstractCode;

        string strDepartString;

        strDepartString = LB_DepartString.Text.Trim();

        strStartTime = DLC_StartTime.Text;
        strEndTime = DLC_EndTime.Text;

        strConstractCode = TB_ConstractCode.Text.Trim();
        strConstractCode = "%" + strConstractCode + "%";
        strConstractName = "%" + TB_ConstractName.Text.Trim() + "%";


        strHQL = @"select c.PartA as Unit,   
                    c.ConstractCode as ContractNumber,   
                    COALESCE(i.InvoiceAmount,0) as ContractAmount,   
                    COALESCE(p.PayableAccount,0) as OrderAmount,   
                    COALESCE(r.PayableRecordAmount,0) as ForeignImportCost,   
                    COALESCE(p.PayableAccount,0) as DomesticImportCost,   
                    COALESCE(f.InnerEntryTax,0) as ImportDuty,   
                    COALESCE(f.InnerAddedValueTax,0) as ImportVAT,   
                    COALESCE(p.PayOtherAccount,0) as TotalOtherCharges,   
                    COALESCE(r.HandlingCharge,0) as BankHandlingFee,   
                    COALESCE(p.OtherAccount,0) as OtherPayments,   
                    (COALESCE(i.InvoiceAmount,0) / 117 * 17 -COALESCE(f.InnerAddedValueTax,0) - COALESCE(p.PayableAccount,0) / 117 * 17) as VATPayable,   
                    (COALESCE(i.InvoiceAmount,0) / 117 * 17 -COALESCE(f.InnerAddedValueTax,0) - COALESCE(p.PayableAccount,0) / 117 * 17) * 12 / 100 as LocalTaxPayable,   
                    COALESCE(i.InvoiceAmount,0) / 117 * 100 * 3 / 10000 as StampDuty,   
                    COALESCE(i.InvoiceAmount,0) / 117 * 100 * 3 / 10000 as FloodControlFee,   
                    (COALESCE(i.InvoiceAmount,0)-COALESCE(r.PayableRecordAmount,0)-COALESCE(p.PayableAccount,0)-COALESCE(f.InnerEntryTax,0)-
                    COALESCE(f.InnerAddedValueTax,0)-COALESCE(p.PayOtherAccount,0)-COALESCE(r.HandlingCharge,0)-COALESCE(p.OtherAccount,0)-0)-
                    (COALESCE(i.InvoiceAmount,0) / 117 * 17 -COALESCE(f.InnerAddedValueTax,0) - COALESCE(p.PayableAccount,0) / 117 * 17) -
                    (COALESCE(i.InvoiceAmount,0) / 117 * 17 -COALESCE(f.InnerAddedValueTax,0) - COALESCE(p.PayableAccount,0) / 117 * 17) * 12 / 100 -
                    COALESCE(i.InvoiceAmount,0) / 117 * 100 * 3 / 10000 - 
                    COALESCE(i.InvoiceAmount,0) / 117 * 100 * 3 / 10000
                    as ContractProfit   
                    from T_Constract c
                    left join 
                    (
                    select ConstractCode,SUM(Amount) as InvoiceAmount from T_ConstractRelatedInvoice
                    where ReceiveOpen = OPEN
                    group by ConstractCode
                    ) i on c.ConstractCode = i.ConstractCode
                    left join
                    (
                    select ConstractCode,SUM(PayableAccount) as PayableAccount,SUM(PayOtherAccount) as PayOtherAccount,
                    SUM(OtherAccount) as OtherAccount
                    from T_ConstractPayable
                    group by ConstractCode
                    ) p on c.ConstractCode = p.ConstractCode
                    left join
                    (
                    select ConstractCode,SUM(HomeCurrencyAmount*ExchangeRate) as PayableRecordAmount,SUM(HandlingCharge) as HandlingCharge 
                    from T_ConstractPayableRecord
                    group by ConstractCode
                    ) r on c.ConstractCode = r.ConstractCode
                    left join
                    (
                    select ConstractCode,SUM(EntryTax) as InnerEntryTax,SUM(AddedValueTax) as InnerAddedValueTax 
                    from T_ConstractRelatedEntryOrderForInner
                    group by ConstractCode
                    ) f on c.ConstractCode = f.ConstractCode
                        where 1=1 ";
        strHQL += " and (c.ConstractCode in (Select ConstractCode From T_Constract Where  DepartCode in " + strDepartString + ")";
        strHQL += " Or (c.ConstractCode in (Select ConstractCode From T_ConstractRelatedUser Where UserCode like  " + "'" + strLikeUserCode + "'" + ")))";
        strHQL += " and c.ConstractCode Like " + "'" + strConstractCode + "'";
        strHQL += " and c.ConstractName Like " + "'" + strConstractName + "'";
        if (!string.IsNullOrEmpty(strStartTime))
        {
            strHQL += " and to_char(StartDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
        }
        if (!string.IsNullOrEmpty(strEndTime))
        {
            strHQL += " and to_char(EndDate,'yyyymmdd') <=" + "'" + strEndTime + "'";
        }
        strHQL += " Order by c.ConstractID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractImportReport");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        CountAmount(ds);

        LB_Sql.Text = strHQL;
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        string strHQL = LB_Sql.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractBankReport");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void CountAmount(DataSet ds)
    {
        int i;
        decimal deTotalAmount = 0, deTotalEntryTaxAmount = 0, deTotalAddedValueTaxAmount = 0;

        //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        //{
        //    deTotalAmount += decimal.Parse(ds.Tables[0].Rows[i]["TotalAmount"].ToString());
        //    deTotalEntryTaxAmount += decimal.Parse(ds.Tables[0].Rows[i]["TotalEntryTax"].ToString());
        //    deTotalAddedValueTaxAmount += decimal.Parse(ds.Tables[0].Rows[i]["TotalAddedValueTax"].ToString());
        //}

        LB_TotalAmount.Text = deTotalAmount.ToString();
        LB_TotalEntryTaxAmount.Text = deTotalEntryTaxAmount.ToString();
        LB_TotalAddedTaxAmount.Text = deTotalAddedValueTaxAmount.ToString();

    }

}
