using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Drawing;

public partial class TTDWCustomImport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataYearMonthBinder();

            DataBindCusmtom();

            DataCustomValueBinder();
        }
    }



    private void DataYearMonthBinder()
    {
        DDL_Year.Items.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
        DDL_Year.Items.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
        DDL_Year.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
        DDL_Year.Items.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
        DDL_Year.Items.Add(new ListItem(DateTime.Now.AddYears(2).Year.ToString(), DateTime.Now.AddYears(2).Year.ToString()));
        DDL_Year.SelectedValue = DateTime.Now.Year.ToString();

        
        for (int i = 1; i <= 12; i++)
        {
            if (i < 10)
            {
                DDL_Month.Items.Add(new ListItem("0" + i, "0" + i));
            }
            else
            {
                DDL_Month.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        string strCurrentMonth = DateTime.Now.Month.ToString();
        if (strCurrentMonth.Length > 1)
        {
            DDL_Month.SelectedValue = strCurrentMonth;
        }
        else
        {
            DDL_Month.SelectedValue = "0" + strCurrentMonth;
        }
    }


    private void DataBindCusmtom()
    {
        string strCustomHQL = "select CustomName from T_DWCustomValue group by CustomName";
        DataTable dtCustom = ShareClass.GetDataSetFromSql(strCustomHQL, "Custom").Tables[0];

        DDL_CustomName.DataSource = dtCustom;
        DDL_CustomName.DataBind();

        DDL_CustomName.Items.Insert(0, new ListItem("", ""));
    }





    private void DataCustomValueBinder()
    {
        string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;
        string strCustomName = TXT_CustomName.Text.Trim();
        string strAllCustomName = DDL_CustomName.SelectedValue;

        if (!ShareClass.CheckStringRight(strCustomName))
        {
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('"+Resources.lang.ZZKHMCBNBHFFZFC+"');</script>");
            return;
        }

        //客户价值表
        string strDWCustomValueHQL = string.Empty;
        strDWCustomValueHQL = string.Format(@"select * from
                        (
                        select * from T_DWCustomValue t
                        union all
                        select 
                        0 as ID,
                        CustomName,
                        NULL as ProductName,
                        NULL as ProductCode,
                        NULL as ProductType,
                        NULL as SaleTime,
                        SUM(SaleNumber) as SaleNumber,
                        SUM(SalePrice) as SalePrice,
                        SUM(SaleMoney) as SaleMoney,
                        SUM(ProductCost) as ProductCost,
                        SUM(MakeCost) as MakeCost,
                        SUM(TonCost) as TonCost,
                        SUM(PickCost) as PickCost,
                        SUM(QualityCost) as QualityCost,
                        SUM(TransportCost) as TransportCost,
                        SUM(AccountCost) as AccountCost,
                        SUM(ServeCost) as ServeCost,
                        SUM(TravelCost) as TravelCost,
                        NULL as Applyer,
                        SUM(SalesmanWages) as SalesmanWages,
                        SUM(SurplusValue) as SurplusValue,
                        YearMonth
                        from T_DWCustomValue t
                        group by CustomName,YearMonth
                        ) a
                         where  a.YearMonth = '{0}'", strYearMonth);

        if (!string.IsNullOrEmpty(strCustomName))
        {
            //strDWCustomValueHQL = string.Format("select * from T_DWCustomValue t where  YearMonth = '{0}' and CustomName like '%{1}%'", strYearMonth, strCustomName);
            strDWCustomValueHQL += " and a.CustomName like '%" + strCustomName + "%'";
        }
        if (!string.IsNullOrEmpty(strAllCustomName))
        {
            strDWCustomValueHQL += " and a.CustomName = '" + strAllCustomName + "'";
        }

        strDWCustomValueHQL += " order by a.CustomName,a.ID desc";

        DataTable dtCustomValue = ShareClass.GetDataSetFromSql(strDWCustomValueHQL, "DWCustomValue").Tables[0];

        DG_CustomValue.DataSource = dtCustomValue;
        DG_CustomValue.DataBind();

        LB_CustomValueSql.Text = strDWCustomValueHQL;
    }


    

    protected void BT_Seach_Click(object sender, EventArgs e)
    {
        DataCustomValueBinder();

    }



    protected void DG_CustomValue_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_CustomValue.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_CustomValueSql.Text.Trim();
        DataTable dtZL = ShareClass.GetDataSetFromSql(strHQL, "customValue").Tables[0];

        DG_CustomValue.DataSource = dtZL;
        DG_CustomValue.DataBind();

    }



}