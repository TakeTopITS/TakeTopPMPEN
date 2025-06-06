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

public partial class TTDWCustomTotal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();if (!IsPostBack)
        {
            DataYearMonthBinder();

            DataBindCusmtom();

            DataCustomTotalBinder();

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

        DDL_TYear.Items.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
        DDL_TYear.Items.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
        DDL_TYear.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
        DDL_TYear.Items.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
        DDL_TYear.Items.Add(new ListItem(DateTime.Now.AddYears(2).Year.ToString(), DateTime.Now.AddYears(2).Year.ToString()));
        DDL_TYear.SelectedValue = DateTime.Now.Year.ToString();

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

        DDL_TCustomeName.DataSource = dtCustom;
        DDL_TCustomeName.DataBind();

        DDL_TCustomeName.Items.Insert(0, new ListItem("", ""));

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


    private void DataCustomTotalBinder()
    {
        try
        {
            string strYear = DDL_TYear.SelectedValue;
            string strCustomName = TXT_TCustomeName.Text.Trim();
            string strAllCustomName = DDL_TCustomeName.SelectedValue;
            if (!ShareClass.CheckStringRight(strCustomName))
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('"+Resources.lang.ZZKHMCBNBHFFZFC+"');</script>");
                return;
            }

            //客户价值表
            string strDWCustomValueHQL = string.Empty;
            strDWCustomValueHQL = string.Format(@"select CustomName,SUBSTRING(YearMonth, 0, 5),
                            MonthTotal1 = sum(case when SUBSTRING(YearMonth, 5, 2) = '01' then  SurplusValue else 0 end),
                            MonthTotal2 = sum(case when SUBSTRING(YearMonth, 5, 2) = '02' then  SurplusValue else 0 end),
                            MonthTotal3 = sum(case when SUBSTRING(YearMonth, 5, 2) = '03' then  SurplusValue else 0 end),
                            MonthTotal4 = sum(case when SUBSTRING(YearMonth, 5, 2) = '04' then  SurplusValue else 0 end),
                            MonthTotal5 = sum(case when SUBSTRING(YearMonth, 5, 2) = '05' then  SurplusValue else 0 end),
                            MonthTotal6 = sum(case when SUBSTRING(YearMonth, 5, 2) = '06' then  SurplusValue else 0 end),
                            MonthTotal7 = sum(case when SUBSTRING(YearMonth, 5, 2) = '07' then  SurplusValue else 0 end),
                            MonthTotal8 = sum(case when SUBSTRING(YearMonth, 5, 2) = '08' then  SurplusValue else 0 end),
                            MonthTotal9 = sum(case when SUBSTRING(YearMonth, 5, 2) = '09' then  SurplusValue else 0 end),
                            MonthTotal10 = sum(case when SUBSTRING(YearMonth, 5, 2) = '10' then  SurplusValue else 0 end),
                            MonthTotal11 = sum(case when SUBSTRING(YearMonth, 5, 2) = '11' then  SurplusValue else 0 end),
                            MonthTotal12 = sum(case when SUBSTRING(YearMonth, 5, 2) = '12' then  SurplusValue else 0 end)
                            from T_DWCustomValue
                            where YearMonth like '{0}%'", strYear);
            if (!string.IsNullOrEmpty(strCustomName))
            {
                strDWCustomValueHQL += " and CustomName like '%" + strCustomName + "%'";
            }
            if (!string.IsNullOrEmpty(strAllCustomName))
            {
                strDWCustomValueHQL += " and CustomName = '" + strAllCustomName + "'";
            }
            strDWCustomValueHQL += "group by CustomName,SUBSTRING(YearMonth, 0, 5)";

            DataTable dtCustomValue = ShareClass.GetDataSetFromSql(strDWCustomValueHQL, "DWCustomValue").Tables[0];
            if (dtCustomValue != null && dtCustomValue.Rows.Count > 0)
            {
                //统计总的数据
                decimal decimalTotalMonth1 = 0;                        //1月份汇总
                decimal decimalTotalMonth2 = 0;                        //2月份汇总
                decimal decimalTotalMonth3 = 0;                        //3月份汇总
                decimal decimalTotalMonth4 = 0;                        //4月份汇总
                decimal decimalTotalMonth5 = 0;                        //5月份汇总
                decimal decimalTotalMonth6 = 0;                        //6月份汇总
                decimal decimalTotalMonth7 = 0;                        //7月份汇总
                decimal decimalTotalMonth8 = 0;                        //8月份汇总
                decimal decimalTotalMonth9 = 0;                        //9月份汇总
                decimal decimalTotalMonth10 = 0;                       //10月份汇总
                decimal decimalTotalMonth11 = 0;                       //11月份汇总
                decimal decimalTotalMonth12 = 0;                       //12月份汇总

                foreach (DataRow drCustomValue in dtCustomValue.Rows)
                {
                    decimal decimalMonth1 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal1"]), out decimalMonth1);
                    decimalTotalMonth1 += decimalMonth1;
                    decimal decimalMonth2 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal2"]), out decimalMonth2);
                    decimalTotalMonth2 += decimalMonth2;
                    decimal decimalMonth3 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal3"]), out decimalMonth3);
                    decimalTotalMonth3 += decimalMonth3;
                    decimal decimalMonth4 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal4"]), out decimalMonth4);
                    decimalTotalMonth4 += decimalMonth4;
                    decimal decimalMonth5 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal5"]), out decimalMonth5);
                    decimalTotalMonth5 += decimalMonth5;
                    decimal decimalMonth6 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal6"]), out decimalMonth6);
                    decimalTotalMonth6 += decimalMonth6;
                    decimal decimalMonth7 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal7"]), out decimalMonth7);
                    decimalTotalMonth7 += decimalMonth7;
                    decimal decimalMonth8 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal8"]), out decimalMonth8);
                    decimalTotalMonth8 += decimalMonth8;
                    decimal decimalMonth9 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal9"]), out decimalMonth9);
                    decimalTotalMonth9 += decimalMonth9;
                    decimal decimalMonth10 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal10"]), out decimalMonth10);
                    decimalTotalMonth10 += decimalMonth10;
                    decimal decimalMonth11 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal11"]), out decimalMonth11);
                    decimalTotalMonth11 += decimalMonth11;
                    decimal decimalMonth12 = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MonthTotal12"]), out decimalMonth12);
                    decimalTotalMonth12 += decimalMonth12;
                }

                DataRow newRow;
                newRow = dtCustomValue.NewRow();
                newRow["MonthTotal1"] = decimalTotalMonth1;
                newRow["MonthTotal2"] = decimalTotalMonth2;
                newRow["MonthTotal3"] = decimalTotalMonth3;
                newRow["MonthTotal4"] = decimalTotalMonth4;
                newRow["MonthTotal5"] = decimalTotalMonth5;
                newRow["MonthTotal6"] = decimalTotalMonth6;
                newRow["MonthTotal7"] = decimalTotalMonth7;
                newRow["MonthTotal8"] = decimalTotalMonth8;
                newRow["MonthTotal9"] = decimalTotalMonth9;
                newRow["MonthTotal10"] = decimalTotalMonth10;
                newRow["MonthTotal11"] = decimalTotalMonth11;
                newRow["MonthTotal12"] = decimalTotalMonth12;
                dtCustomValue.Rows.Add(newRow);
            }

            DG_Total.DataSource = dtCustomValue;
            DG_Total.DataBind();

            LB_TotalSql.Text = strDWCustomValueHQL;
        }
        catch (Exception ex) { }
    }

    protected void BT_Seach_Click(object sender, EventArgs e)
    {
        DataCustomValueBinder();

        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>clickDetail();</script>");
    }

    protected void BT_Export_Click(object sender, EventArgs e)
    {
        string strFileName = "客户价值表详细数据";
        Export3Excel(DG_CustomValue, strFileName);

        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>clickDetail();</script>");
    }


    protected void BT_TSeach_Click(object sender, EventArgs e)
    {
        DataCustomTotalBinder();

        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>clickTotal();</script>");
    }

    protected void BT_TExport_Click(object sender, EventArgs e)
    {
        string strFileName = "客户价值表汇总";
        Export3Excel(DG_Total, strFileName);

        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>clickTotal();</script>");
    }
    

    protected void DG_Total_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Total.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_TotalSql.Text.Trim(); ;
        DataTable dtZL = ShareClass.GetDataSetFromSql(strHQL, "total").Tables[0];

        DG_Total.DataSource = dtZL;
        DG_Total.DataBind();
    }

    protected void DG_CustomValue_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_CustomValue.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_CustomValueSql.Text.Trim();
        DataTable dtZL = ShareClass.GetDataSetFromSql(strHQL, "customValue").Tables[0];

        DG_CustomValue.DataSource = dtZL;
        DG_CustomValue.DataBind();

        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>clickDetail();</script>");
    }



    protected void DG_Total_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager && e.Item.ItemType != ListItemType.Footer && e.Item.ItemType != ListItemType.Header)
        {
            decimal decimalMonth1 = 0;
            decimal.TryParse(e.Item.Cells[1].Text, out decimalMonth1);
            decimal decimalMonth2 = 0;
            decimal.TryParse(e.Item.Cells[2].Text, out decimalMonth2);
            decimal decimalMonth3 = 0;
            decimal.TryParse(e.Item.Cells[3].Text, out decimalMonth3);
            decimal decimalMonth4 = 0;
            decimal.TryParse(e.Item.Cells[4].Text, out decimalMonth4);
            decimal decimalMonth5 = 0;
            decimal.TryParse(e.Item.Cells[5].Text, out decimalMonth5);
            decimal decimalMonth6 = 0;
            decimal.TryParse(e.Item.Cells[6].Text, out decimalMonth6);
            decimal decimalMonth7 = 0;
            decimal.TryParse(e.Item.Cells[7].Text, out decimalMonth7);
            decimal decimalMonth8 = 0;
            decimal.TryParse(e.Item.Cells[8].Text, out decimalMonth8);
            decimal decimalMonth9 = 0;
            decimal.TryParse(e.Item.Cells[9].Text, out decimalMonth9);
            decimal decimalMonth10 = 0;
            decimal.TryParse(e.Item.Cells[10].Text, out decimalMonth10);
            decimal decimalMonth11 = 0;
            decimal.TryParse(e.Item.Cells[11].Text, out decimalMonth11);
            decimal decimalMonth12 = 0;
            decimal.TryParse(e.Item.Cells[12].Text, out decimalMonth12);
            decimal decimalTotal12 = decimalMonth1 + decimalMonth2 + decimalMonth3 + decimalMonth4 + decimalMonth5 +
                decimalMonth6 + decimalMonth7 + decimalMonth8 + decimalMonth9 + decimalMonth10 + decimalMonth11 + decimalMonth12;

            Label lbShow = (Label)e.Item.FindControl("LB_ShowTotal");
            lbShow.Text = decimalTotal12.ToString();

        }
    }


    public void Export3Excel(System.Web.UI.Control objControl, string strFileName)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        objControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();

    }
}