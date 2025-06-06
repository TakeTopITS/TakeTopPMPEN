using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTDWCustomValueImport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataYearMonthBinder();

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


    private void DataCustomValueBinder()
    {
        try
        {
            DataTable dtCustomValue = GetCustomTable();

            //统计总的数据
            decimal decimalTotalTransportCost = 0;          //运输费用
            decimal decimalTotalAccountCost = 0;            //财务费用
            decimal decimalTotalServeCost = 0;              //招待费用
            decimal decimalTotalTravelCost = 0;             //差旅费用
            decimal decimalTotalSalesmanWages = 0;          //业务员工资
            decimal decimalTotalSurplusValue = 0;           //剩余价值

            foreach (DataRow drCustomValue in dtCustomValue.Rows)
            {
                decimal decimalTransportCost = 0;
                decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TransportCost"]), out decimalTransportCost);
                decimalTotalTransportCost += decimalTransportCost;
                decimal decimalAccountCost = 0;
                decimal.TryParse(ShareClass.ObjectToString(drCustomValue["AccountCost"]), out decimalAccountCost);
                decimalTotalAccountCost += decimalAccountCost;
                decimal decimalServeCost = 0;
                decimal.TryParse(ShareClass.ObjectToString(drCustomValue["ServeCost"]), out decimalServeCost);
                decimalTotalServeCost += decimalServeCost;
                decimal decimalTravelCost = 0;
                decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TravelCost"]), out decimalTravelCost);
                decimalTotalTravelCost += decimalTravelCost;
                decimal decimalSalesmanWages = 0;
                //decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SalesmanWages"]), out decimalSalesmanWages);
                decimalTotalSalesmanWages += decimalSalesmanWages;
                decimal decimalSurplusValue = 0;
                decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SurplusValue"]), out decimalSurplusValue);
                decimalTotalSurplusValue += decimalSurplusValue;
            }

            DataRow newRow;
            newRow = dtCustomValue.NewRow();
            newRow["TransportCost"] = decimalTotalTransportCost;
            newRow["AccountCost"] = decimalTotalAccountCost;
            newRow["ServeCost"] = decimalTotalServeCost;
            newRow["TravelCost"] = decimalTotalTravelCost;
            newRow["SurplusValue"] = decimalTotalSurplusValue;
            dtCustomValue.Rows.Add(newRow);

            DG_CustomValue.DataSource = dtCustomValue;
            DG_CustomValue.DataBind();
        }
        catch (Exception ex) { }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string fileName = fileExcel.PostedFile.FileName;
        if (!string.IsNullOrEmpty(fileName))
        {
            fileName = fileName.Substring(fileExcel.PostedFile.FileName.LastIndexOf(".")).ToLower();

            string url = string.Format("Doc/XML/{0}{1}", "DW" + DateTime.Now.ToString("yyyyMMddHHmmss"), fileName);
            string destFilePath = Server.MapPath(url);
            fileExcel.SaveAs(destFilePath);
            DataTable dtExcel = null;
            string resultMsg = string.Empty;
            try
            {
                //dtExcel = ExcelUtils.ExcelToDataSet(destFilePath, "Sheet1").Tables[0];
                dtExcel = ExcelUtils.ReadExcel(destFilePath, "Sheet1").Tables[0];
                bool isSuccess = ValidateData(dtExcel, ref resultMsg);
                if (isSuccess)
                {
                    Import(dtExcel, ref resultMsg);
                }

                lblMsg.Text = string.Format("<span style='color:red' >{0}</span>", resultMsg);
            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
            }
        }
    }


    /// <summary>
    /// 验证数据合法性.
    /// </summary>
    /// <param name="dtExcel"></param>
    /// <param name="resultMsg"></param>
    /// <returns></returns>
    private bool ValidateData(DataTable dtExcel, ref string resultMsg)
    {
        int lineNumber = 1;
        foreach (DataRow row in dtExcel.Rows)
        {
            string strDLCode = string.Empty;
            string strDLName = string.Empty;
            lineNumber++;
            try
            {
                string strSaleTime = ShareClass.ObjectToString(row["日期"]);
                string strCustomName = ShareClass.ObjectToString(row["购货单位"]);
                string strProductType = ShareClass.ObjectToString(row["产品种类"]);
                string strLongProductCode = ShareClass.ObjectToString(row["产品长代码"]);
                string strProductCode = ShareClass.ObjectToString(row["工艺卡编号"]);
                string strSaleNumber = ShareClass.ObjectToString(row["数量"]);
                string strSalePrice = ShareClass.ObjectToString(row["含税单价"]);
                string strSaleMoney = ShareClass.ObjectToString(row["金额"]);
                string strAccountCost = ShareClass.ObjectToString(row["财务费用"]);

                if (string.IsNullOrEmpty(strSaleTime) && string.IsNullOrEmpty(strCustomName) && string.IsNullOrEmpty(strProductType) &&
                    string.IsNullOrEmpty(strLongProductCode) && string.IsNullOrEmpty(strProductCode) && string.IsNullOrEmpty(strSaleNumber) &&
                    string.IsNullOrEmpty(strSalePrice) && string.IsNullOrEmpty(strSaleMoney) && string.IsNullOrEmpty(strAccountCost))
                {
                    break;
                }

                if (string.IsNullOrEmpty(strSaleTime))
                {
                    resultMsg += string.Format("第{0}行，日期不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    try
                    {
                        DateTime.Parse(strSaleTime);
                    }
                    catch (Exception ex)
                    {
                        resultMsg += string.Format("第{0}行，日期格式不正确<br/>", lineNumber);
                        continue;
                    }
                }

                //验证购货单位，购货单位是否在基础表中存在

                if (string.IsNullOrEmpty(strCustomName))
                {
                    resultMsg += string.Format("第{0}行，购货单位不能为空<br/>", lineNumber);
                    continue;
                }
                //string strCustomNameHQL = "select count(1) as RowNumber from T_DWQualityCost where CustomName = '" + strCustomName + "'";
                //DataTable dtCustomName = ShareClass.GetDataSetFromSql(strCustomNameHQL, "CustomName").Tables[0];
                //if (dtCustomName == null || dtCustomName.Rows.Count == 0)
                //{
                //    resultMsg += string.Format("第{0}行，购货单位在质量成本的基础表中不存在<br/>", lineNumber);
                //    continue;
                //}

                //产品种类

                if (string.IsNullOrEmpty(strProductType))
                {
                    resultMsg += string.Format("第{0}行，产品种类不能为空<br/>", lineNumber);
                    continue;
                }
                string strProductTypeHQL = "select count(1) as RowNumber from T_DWProductType where ProductType = '" + strProductType + "'";
                DataTable dtProductType = ShareClass.GetDataSetFromSql(strProductTypeHQL, "ProductType").Tables[0];
                if (dtProductType == null || dtProductType.Rows.Count == 0)
                {
                    resultMsg += string.Format("第{0}行，产品种类在类型的基础表中不存在<br/>", lineNumber);
                    continue;
                }
                //产品长代码

                if (string.IsNullOrEmpty(strLongProductCode))
                {
                    resultMsg += string.Format("第{0}行，产品长代码不能为空<br/>", lineNumber);
                    continue;
                }

                //工艺卡编号

                if (string.IsNullOrEmpty(strProductCode))
                {
                    resultMsg += string.Format("第{0}行，工艺卡编号不能为空<br/>", lineNumber);
                    continue;
                }

                //数量

                if (string.IsNullOrEmpty(strSaleNumber))
                {
                    resultMsg += string.Format("第{0}行，数量不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsSaleNumber = ShareClass.CheckIsNumber(strSaleNumber);
                    if (!IsSaleNumber)
                    {
                        resultMsg += string.Format("第{0}行，数量只能是小数<br/>", lineNumber);
                        continue;
                    }
                }

                //含税单价

                if (string.IsNullOrEmpty(strSalePrice))
                {
                    resultMsg += string.Format("第{0}行，含税单价不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsSalePrice = ShareClass.CheckIsNumber(strSalePrice);
                    if (!IsSalePrice)
                    {
                        resultMsg += string.Format("第{0}行，含税单价只能是小数<br/>", lineNumber);
                        continue;
                    }
                }

                //金额

                if (string.IsNullOrEmpty(strSaleMoney))
                {
                    resultMsg += string.Format("第{0}行，金额不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsSaleMoney = ShareClass.CheckIsNumber(strSaleMoney);
                    if (!IsSaleMoney)
                    {
                        resultMsg += string.Format("第{0}行，金额只能是小数<br/>", lineNumber);
                        continue;
                    }
                }

                //财务费用

                if (string.IsNullOrEmpty(strAccountCost))
                {
                    resultMsg += string.Format("第{0}行，财务费用不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsAccountCost = ShareClass.CheckIsNumber(strAccountCost);
                    if (!IsAccountCost)
                    {
                        resultMsg += string.Format("第{0}行，财务费用只能是小数<br/>", lineNumber);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = string.Format("<span style='color:red' >导入时出现以下错误: {0}!</span>", ex.Message);
            }

        }
        if (!string.IsNullOrEmpty(resultMsg)) return false;
        return true;
    }


    private bool Import(DataTable dtExcel, ref string resultMsg)
    {
        try
        {
            //先清空客户价值导入表
            string strDeleteCustomImportHQL = "truncate table T_DWCustomImport";
            ShareClass.RunSqlCommand(strDeleteCustomImportHQL);

            int successCount = 0;
            int lineNumber = 0;

            DWCustomImportBLL dWCustomImportBLL = new DWCustomImportBLL();

            foreach (DataRow row in dtExcel.Rows)
            {
                string strSaleTime = string.Empty;
                string strCustomName = string.Empty;
                string strProductType = string.Empty;
                string strProductName = string.Empty;
                string strProductCode = string.Empty;
                string strSaleNumber = string.Empty;
                string strSalePrice = string.Empty;
                string strSaleMoney = string.Empty;
                string strAccountCost = string.Empty;
                string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;

                try
                {
                    lineNumber++;
                    strSaleTime = ShareClass.ObjectToString(row["日期"]);
                    DateTime dtSaleTime = DateTime.Now;
                    DateTime.TryParse(strSaleTime, out dtSaleTime);
                    strCustomName = ShareClass.ObjectToString(row["购货单位"]);
                    strProductType = ShareClass.ObjectToString(row["产品种类"]);
                    strProductName = ShareClass.ObjectToString(row["产品长代码"]);
                    strProductCode = ShareClass.ObjectToString(row["工艺卡编号"]);
                    strSaleNumber = ShareClass.ObjectToString(row["数量"]);
                    decimal decimalSaleNumber = 0;
                    decimal.TryParse(strSaleNumber, out decimalSaleNumber);
                    strSalePrice = ShareClass.ObjectToString(row["含税单价"]);
                    decimal decimalSalePrice = 0;
                    decimal.TryParse(strSalePrice, out decimalSalePrice);
                    strSaleMoney = ShareClass.ObjectToString(row["金额"]);
                    decimal decimalSaleMoney = 0;
                    decimal.TryParse(strSaleMoney, out decimalSaleMoney);
                    strAccountCost = ShareClass.ObjectToString(row["财务费用"]);
                    decimal decimalAccountCost = 0;
                    decimal.TryParse(strAccountCost, out decimalAccountCost);

                    if (string.IsNullOrEmpty(strSaleTime) && string.IsNullOrEmpty(strCustomName) && string.IsNullOrEmpty(strProductType) &&
                       string.IsNullOrEmpty(strProductName) && string.IsNullOrEmpty(strProductCode) && string.IsNullOrEmpty(strSaleNumber) &&
                       string.IsNullOrEmpty(strSalePrice) && string.IsNullOrEmpty(strSaleMoney) && string.IsNullOrEmpty(strAccountCost))
                    {
                        break;
                    }

                    DWCustomImport dWCustomImport = new DWCustomImport();
                    dWCustomImport.SaleTime = dtSaleTime;
                    dWCustomImport.CustomName = strCustomName;
                    dWCustomImport.ProductType = strProductType;
                    dWCustomImport.ProductName = strProductName;
                    dWCustomImport.ProductCode = strProductCode;
                    dWCustomImport.SaleNumber = decimalSaleNumber;
                    dWCustomImport.SalePrice = decimalSalePrice;
                    dWCustomImport.SaleMoney = decimalSaleMoney;
                    dWCustomImport.AccountCost = decimalAccountCost;
                    dWCustomImport.YearMonth = strYearMonth;

                    dWCustomImportBLL.AddDWCustomImport(dWCustomImport);

                    successCount++;
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + Resources.lang.ZZJGDRSBJC + " : " + Resources.lang.HangHao + ": " + (lineNumber + 2).ToString() + " , " + Resources.lang.DaiMa + ": " + strCustomName + " : " + err.Message.ToString());
                }
            }

            if (successCount > 0)
            {
                if (successCount == dtExcel.Rows.Count)
                {
                    resultMsg += string.Format("<br/>已成功导入 {0} 条数据", successCount);
                }
                else
                {
                    resultMsg += string.Format("<br/>已成功导入 {0} 条数据， 共有 {1} 条数据验证失败", successCount, dtExcel.Rows.Count - successCount);
                }

                //重新加载列表
                DataCustomValueBinder();
                return true;
            }
            else
            {
                resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
            }

            return false;
        }
        catch (Exception ex)
        {

            resultMsg += ex.Message;
            return false;
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        try
        {
            string strYear = DDL_Year.SelectedValue;
            string strCurrentMonth = DDL_Month.SelectedValue;

            DataTable CustomValueTable = GetCustomTable();

            if (CustomValueTable != null && CustomValueTable.Rows.Count > 0)
            {
                //判断
                string strDWCustomValueHQL = string.Format("select * from T_DWCustomValue t where YearMonth = '{0}'", strYear + "" + strCurrentMonth);
                DataTable dtCustomValue = ShareClass.GetDataSetFromSql(strDWCustomValueHQL, "CustomValue").Tables[0];
                if (dtCustomValue != null && dtCustomValue.Rows.Count > 0)
                {
                    //已经存在，是删除再创建呢，还是提示，已经保存过了，不能再保存
                    string strDelCustomValueHQL = string.Format("delete T_DWCustomValue where YearMonth = '{0}'", strYear + "" + strCurrentMonth);
                    ShareClass.RunSqlCommand(strDelCustomValueHQL);

                    DWCustomValueBLL dWCustomValueBLL = new DWCustomValueBLL();

                    foreach (DataRow drCustomValue in CustomValueTable.Rows)
                    {
                        try
                        {
                            DWCustomValue dWCustomValue = new DWCustomValue();
                            dWCustomValue.CustomName = ShareClass.ObjectToString(drCustomValue["CustomName"]);
                            dWCustomValue.ProductName = ShareClass.ObjectToString(drCustomValue["ProductName"]);
                            dWCustomValue.ProductCode = ShareClass.ObjectToString(drCustomValue["ProductCode"]);
                            dWCustomValue.ProductType = ShareClass.ObjectToString(drCustomValue["ProductType"]);
                            DateTime dtSaleTime = DateTime.Now;
                            DateTime.TryParse(ShareClass.ObjectToString(drCustomValue["SaleTime"]), out dtSaleTime);
                            dWCustomValue.SaleTime = dtSaleTime;
                            decimal decimalSaleNumber = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SaleNumber"]), out decimalSaleNumber);
                            dWCustomValue.SaleNumber = decimalSaleNumber;
                            decimal decimalSalePrice = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SalePrice"]), out decimalSalePrice);
                            dWCustomValue.SalePrice = decimalSalePrice;
                            decimal decimalSaleMoney = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SaleMoney"]), out decimalSaleMoney);
                            dWCustomValue.SaleMoney = decimalSaleMoney;
                            decimal decimalProductCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["ProductCost"]), out decimalProductCost);
                            dWCustomValue.ProductCost = decimalProductCost;
                            decimal decimalMakeCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MakeCost"]), out decimalMakeCost);
                            dWCustomValue.MakeCost = decimalMakeCost;
                            decimal decimalTonCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TonCost"]), out decimalTonCost);
                            dWCustomValue.TonCost = decimalTonCost;
                            decimal decimalPickCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["PickCost"]), out decimalPickCost);
                            dWCustomValue.PickCost = decimalPickCost;
                            decimal decimalQualityCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["QualityCost"]), out decimalQualityCost);
                            dWCustomValue.QualityCost = decimalQualityCost;
                            decimal decimalTransportCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TransportCost"]), out decimalTransportCost);
                            dWCustomValue.TransportCost = decimalTransportCost;
                            decimal decimalAccountCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["AccountCost"]), out decimalAccountCost);
                            dWCustomValue.AccountCost = decimalAccountCost;
                            decimal decimalServeCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["ServeCost"]), out decimalServeCost);
                            dWCustomValue.ServeCost = decimalServeCost;
                            decimal decimalTravelCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TravelCost"]), out decimalTravelCost);
                            dWCustomValue.TravelCost = decimalTravelCost;
                            dWCustomValue.Applyer = ShareClass.ObjectToString(drCustomValue["Applyer"]);
                            decimal decimalSalesmanWages = 0;
                            //decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SalesmanWages"]), out decimalSalesmanWages);
                            dWCustomValue.SalesmanWages = decimalSalesmanWages;
                            decimal decimalSurplusValue = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SurplusValue"]), out decimalSurplusValue);
                            dWCustomValue.SurplusValue = decimalSurplusValue;
                            dWCustomValue.YearMonth = ShareClass.ObjectToString(drCustomValue["YearMonth"]);


                            dWCustomValueBLL.AddDWCustomValue(dWCustomValue);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    //弹出成功提示
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
                }
                else
                {
                    DWCustomValueBLL dWCustomValueBLL = new DWCustomValueBLL();

                    foreach (DataRow drCustomValue in CustomValueTable.Rows)
                    {
                        try
                        {
                            DWCustomValue dWCustomValue = new DWCustomValue();
                            dWCustomValue.CustomName = ShareClass.ObjectToString(drCustomValue["CustomName"]);
                            dWCustomValue.ProductName = ShareClass.ObjectToString(drCustomValue["ProductName"]);
                            dWCustomValue.ProductCode = ShareClass.ObjectToString(drCustomValue["ProductCode"]);
                            dWCustomValue.ProductType = ShareClass.ObjectToString(drCustomValue["ProductType"]);
                            DateTime dtSaleTime = DateTime.Now;
                            DateTime.TryParse(ShareClass.ObjectToString(drCustomValue["SaleTime"]), out dtSaleTime);
                            dWCustomValue.SaleTime = dtSaleTime;
                            decimal decimalSaleNumber = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SaleNumber"]), out decimalSaleNumber);
                            dWCustomValue.SaleNumber = decimalSaleNumber;
                            decimal decimalSalePrice = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SalePrice"]), out decimalSalePrice);
                            dWCustomValue.SalePrice = decimalSalePrice;
                            decimal decimalSaleMoney = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SaleMoney"]), out decimalSaleMoney);
                            dWCustomValue.SaleMoney = decimalSaleMoney;
                            decimal decimalProductCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["ProductCost"]), out decimalProductCost);
                            dWCustomValue.ProductCost = decimalProductCost;
                            decimal decimalMakeCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MakeCost"]), out decimalMakeCost);
                            dWCustomValue.MakeCost = decimalMakeCost;
                            decimal decimalTonCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TonCost"]), out decimalTonCost);
                            dWCustomValue.TonCost = decimalTonCost;
                            decimal decimalPickCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["PickCost"]), out decimalPickCost);
                            dWCustomValue.PickCost = decimalPickCost;
                            decimal decimalQualityCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["QualityCost"]), out decimalQualityCost);
                            dWCustomValue.QualityCost = decimalQualityCost;
                            decimal decimalTransportCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TransportCost"]), out decimalTransportCost);
                            dWCustomValue.TransportCost = decimalTransportCost;
                            decimal decimalAccountCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["AccountCost"]), out decimalAccountCost);
                            dWCustomValue.AccountCost = decimalAccountCost;
                            decimal decimalServeCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["ServeCost"]), out decimalServeCost);
                            dWCustomValue.ServeCost = decimalServeCost;
                            decimal decimalTravelCost = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TravelCost"]), out decimalTravelCost);
                            dWCustomValue.TravelCost = decimalTravelCost;
                            dWCustomValue.Applyer = ShareClass.ObjectToString(drCustomValue["Applyer"]);
                            decimal decimalSalesmanWages = 0;
                            //decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SalesmanWages"]), out decimalSalesmanWages);
                            dWCustomValue.SalesmanWages = decimalSalesmanWages;
                            decimal decimalSurplusValue = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SurplusValue"]), out decimalSurplusValue);
                            dWCustomValue.SurplusValue = decimalSurplusValue;
                            dWCustomValue.YearMonth = ShareClass.ObjectToString(drCustomValue["YearMonth"]);


                            dWCustomValueBLL.AddDWCustomValue(dWCustomValue);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    //弹出成功提示
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
                }
            }
            else
            {
                //弹出提示，客户剩余价值表为空
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKHSYJZBWKXDRSJ + "')", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKHSYJZBCWEXMESSAGE + "')", true);
        }
    }


    private DataTable GetCustomTable()
    {
        string strYear = DDL_Year.SelectedValue;
        string strCurrentMonth = DDL_Month.SelectedValue;

        DataTable dtMakeCost = new DataTable();
        DataTable dtQuality = new DataTable();
        DataTable dtLineTrans = new DataTable();
        DataTable dtTravel = new DataTable();

        try
        {
            //制造费用及吨耗
            string strDWMakeCostHQL = string.Format("select * from T_DWMakeCost where YearMonth = '{0}'", strYear + strCurrentMonth);
            dtMakeCost = ShareClass.GetDataSetFromSql(strDWMakeCostHQL, "Make").Tables[0];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZZFYJDHCWEXMESSAGE + "')", true);
        }

        try
        {
            //质量成本表
            string strDWQualityCostHQL = string.Format("select * from T_DWQualityCost where YearMonth = '{0}'", strYear + strCurrentMonth);
            dtQuality = ShareClass.GetDataSetFromSql(strDWQualityCostHQL, "Quality").Tables[0];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZLCBBCWEXMESSAGE + "')", true);
        }

        try
        {
            //运输费用月结
            string strDWLineTransportHQL = string.Format("select * from T_DWLineTransport where YearMonth = '{0}'", strYear + strCurrentMonth);
            dtLineTrans = ShareClass.GetDataSetFromSql(strDWLineTransportHQL, "LineTrans").Tables[0];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYSFYYJCWEXMESSAGE + "')", true);
        }

        try
        {
            //查询招待，差旅费
            string strTravelHQL = string.Format(@"
                select * from T_DWTravelexpenses 
                where SUBSTRING(to_char(tianbiaoriqi, 'yyyy-mm-dd'), 0, 8) = SUBSTRING(to_char('{0}', 'yyyy-mm-dd'), 0, 8)
                and liuchengzhuangtai = '1'", strYear + "-" + strCurrentMonth);
            dtTravel = ShareClass.GetDataSetFromSql(strTravelHQL, "Travel").Tables[0];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCZDCLFCWEXMESSAGE + "')", true);
        }

        //客户价值表
        string strDWCustomValueHQL = "select * from T_DWCustomImport t order by ID asc";
        DataTable dtCustomValue = ShareClass.GetDataSetFromSql(strDWCustomValueHQL, "DWCustomValue").Tables[0];
        try
        {
            if (dtCustomValue != null && dtCustomValue.Rows.Count > 0)
            {
                IDictionary<string, decimal> dictionSaleTotalNumber = new Dictionary<string, decimal>();//统计客户的销售总数量
                IDictionary<string, decimal> dictionSaleTotalMoney = new Dictionary<string, decimal>();//统计客户的销售总金额

                try
                {
                    foreach (DataRow drCustomValue in dtCustomValue.Rows)
                    {
                        string strCustomName = ShareClass.ObjectToString(drCustomValue["CustomName"]).Trim();                            //客户名称
                        string strSaleNumber = ShareClass.ObjectToString(drCustomValue["SaleNumber"]);                            //销售数量
                        string strSaleMoney = ShareClass.ObjectToString(drCustomValue["SaleMoney"]);                              //销售金额
                        decimal decimalSaleNumber = 0;
                        decimal.TryParse(strSaleNumber, out decimalSaleNumber);
                        decimal decimalSaleMoney = 0;
                        decimal.TryParse(strSaleMoney, out decimalSaleMoney);

                        if (!dictionSaleTotalNumber.Keys.Contains(strCustomName))
                        {
                            dictionSaleTotalNumber.Add(strCustomName, decimalSaleNumber);
                        }
                        else
                        {
                            dictionSaleTotalNumber[strCustomName] = dictionSaleTotalNumber[strCustomName] + decimalSaleNumber;
                        }

                        if (!dictionSaleTotalMoney.Keys.Contains(strCustomName))
                        {
                            dictionSaleTotalMoney.Add(strCustomName, decimalSaleMoney);
                        }
                        else
                        {
                            dictionSaleTotalMoney[strCustomName] = dictionSaleTotalMoney[strCustomName] + decimalSaleMoney;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKHMCXSSLXSJECWEXMESSAGE + "')", true);
                }

                Hashtable htMake = new Hashtable();                                     //制造成本和吨耗
                Hashtable htQuality = new Hashtable();                                  //质量成本
                Hashtable htLineTrans = new Hashtable();                                //运输费用
                Hashtable htTravel = new Hashtable();                                   //招待，差旅费

                foreach (DataRow drCustomValue in dtCustomValue.Rows)
                {
                    string strCustomName = ShareClass.ObjectToString(drCustomValue["CustomName"]).Trim();                            //客户名称
                    string strProductType = ShareClass.ObjectToString(drCustomValue["ProductType"]).Trim();                            //类别
                    decimal decimalTonCost = 0;                                                                                 //吨耗
                    decimal decimalSaleNumber = 0;                                                                              //销售数量
                    decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SaleNumber"]), out decimalSaleNumber);
                    decimal decimalTotalSaleMoney = dictionSaleTotalMoney[strCustomName];                                       //当前客户销售总金额



                    //销售数量为负数时，销售金额为0
                    if (decimalSaleNumber < 0)
                    {
                        drCustomValue["SaleMoney"] = 0;
                    }

                    try
                    {
                        //制造成本和吨耗
                        foreach (DataRow drMake in dtMakeCost.Rows)
                        {
                            if (strProductType.ToLower() == ShareClass.ObjectToString(drMake["MakeType"]).Trim().ToLower())
                            {
                                decimal decimalCost = 0;            //制造成本
                                decimal.TryParse(ShareClass.ObjectToString(drMake["Cost"]), out decimalCost);

                                decimal.TryParse(ShareClass.ObjectToString(drMake["TonCost"]), out decimalTonCost);             //吨耗

                                //更新客户价值Table中的制造费用和吨耗 制造费用及吨耗表单数据引入制造成本（包含水电）[制造成本*销售数量/1000]及吨耗
                                decimal decimalMakeCost = (decimalCost * decimalSaleNumber) / 1000;

                                if (decimalSaleNumber >= 0)
                                {
                                    drCustomValue["MakeCost"] = decimalMakeCost;
                                }
                                else
                                {
                                    drCustomValue["MakeCost"] = System.Math.Abs(decimalMakeCost);
                                }
                                drCustomValue["TonCost"] = decimalTonCost;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZZCBHDHCWEXMESSAGE + "')", true);
                    }

                    try
                    {
                        //产品成本X 
                        string strProductCode = ShareClass.ObjectToString(drCustomValue["ProductCode"]).Trim();                            //产品编号

                        //配方*数量 总价
                        string strMatchPriceHQL = string.Format(@"select COALESCE(SUM(t.TotalMatch),0) as TotalMatch from
                                        (
                                        select pm.ProductPrice*m.MaterialPrice as TotalMatch from T_DWProMatch pm
                                        left join T_DWProduct p on pm.ProductID = p.ID
                                        left join T_DWMatch m on pm.MatchID = m.ID
                                        where ProductCode = '{0}'
                                        ) t", strProductCode);
                        DataTable dtMatchPirce = ShareClass.GetDataSetFromSql(strMatchPriceHQL, "MatchPrice").Tables[0];
                        decimal decimalMatchPrice = 0;
                        decimal.TryParse(ShareClass.ObjectToString(dtMatchPirce.Rows[0]["TotalMatch"]), out decimalMatchPrice);

                        //数量 总数量
                        string strProductPriceHQL = string.Format(@"select SUM(COALESCE(pm.ProductPrice,0)) as ProductPrice from T_DWProMatch pm
                                        left join T_DWProduct p on pm.ProductID = p.ID
                                        where p.ProductCode = '{0}'", strProductCode);
                        DataTable dtProductPrice = ShareClass.GetDataSetFromSql(strProductPriceHQL, "ProductPrice").Tables[0];
                        decimal decimalProductPrice = 0;
                        if (dtProductPrice != null && dtProductPrice.Rows.Count > 0)
                        {
                            decimal.TryParse(ShareClass.ObjectToString(dtProductPrice.Rows[0]["ProductPrice"]), out decimalProductPrice);
                        }

                        //类型
                        string strProductTypeHQL = string.Format(@"select p.*,t.ProductType from T_DWProduct p
                                        left join T_DWProductType t on p.TypeID = t.ID
                                        where p.ProductCode = '{0}'", strProductCode);
                        DataTable dtProductType = ShareClass.GetDataSetFromSql(strProductTypeHQL, "ProductType").Tables[0];
                        string strCheckProductType = string.Empty;
                        if (dtProductType != null && dtProductType.Rows.Count > 0)
                        {
                            strCheckProductType = ShareClass.ObjectToString(dtProductType.Rows[0]["ProductType"]);
                        }


                        //计算配方价
                        decimal decimalMatchMoney = 0;
                        if (decimalProductPrice != 0)
                        {
                            decimalMatchMoney = decimalMatchPrice * 1000 / decimalProductPrice;
                        }
                        if (strCheckProductType.Trim().ToLower() == "pvc")
                        {
                            decimalMatchMoney += 100;
                        }

                        //更新客户价值表Table中的产品成本X                  配方价*销售数量/1000*耗吨
                        decimal decimalValue1 = 0;
                        decimalValue1 = decimal.Parse((((decimalMatchMoney * decimalSaleNumber) / 1000) * decimalTonCost).ToString("#0.000"));

                        if (decimalSaleNumber >= 0)
                        {
                            drCustomValue["ProductCost"] = decimalValue1;
                        }
                        else
                        {
                            drCustomValue["ProductCost"] = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCPCBXCWEXMESSAGE + "')", true);
                    }


                    try
                    {
                        //包装费用
                        if (strProductType.ToLower() == "pvc")
                        {
                            //PVC：100*销售数量/1000
                            decimal decimalValue2 = 0;
                            decimalValue2 = decimal.Parse(((100 * decimalSaleNumber) / 1000).ToString("#0.000"));
                            if (decimalSaleNumber >= 0)
                            {
                                drCustomValue["PickCost"] = decimalValue2;
                            }
                            else
                            {
                                drCustomValue["PickCost"] = System.Math.Abs(decimalValue2);
                            }
                        }
                        else
                        {
                            //PE/屏蔽/PE无卤：250*销售数量/1000
                            decimal decimalValue3 = 0;
                            decimalValue3 = decimal.Parse(((250 * decimalSaleNumber) / 1000).ToString("#0.000"));
                            if (decimalSaleNumber >= 0)
                            {
                                drCustomValue["PickCost"] = decimalValue3;
                            }
                            else
                            {
                                drCustomValue["PickCost"] = System.Math.Abs(decimalValue3);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBZFYCWEXMESSAGE + "')", true);
                    }

                    try
                    {
                        //质量损失
                        if (!htQuality.Contains(strCustomName))
                        {
                            foreach (DataRow drQuality in dtQuality.Rows)
                            {
                                if (strCustomName == ShareClass.ObjectToString(drQuality["CustomName"]).Trim())
                                {
                                    decimal decimalPayMoney = 0;       //质量损失
                                    decimal.TryParse(ShareClass.ObjectToString(drQuality["PayMoney"]), out decimalPayMoney);

                                    //更新客户价值Table中的质量损失
                                    if (decimalSaleNumber >= 0)
                                    {
                                        drCustomValue["QualityCost"] = decimalPayMoney;
                                    }
                                    else
                                    {
                                        drCustomValue["QualityCost"] = System.Math.Abs(decimalPayMoney);
                                    }

                                    htQuality.Add(strCustomName, "");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZLSSCWEXMESSAGE + "')", true);
                    }


                    try
                    {
                        //运输费用月结
                        if (!htLineTrans.Contains(strCustomName))
                        {
                            foreach (DataRow drLineTrans in dtLineTrans.Rows)
                            {
                                if (strCustomName == ShareClass.ObjectToString(drLineTrans["CustomName"]).Trim())
                                {
                                    decimal decimalCost = 0;                    //运输费用
                                    decimal.TryParse(ShareClass.ObjectToString(drLineTrans["Cost"]), out decimalCost);

                                    //更新客户价值Table中的运输费用
                                    if (decimalSaleNumber >= 0)
                                    {
                                        drCustomValue["TransportCost"] = decimalCost;
                                    }
                                    else
                                    {
                                        drCustomValue["TransportCost"] = System.Math.Abs(decimalCost);
                                    }

                                    htLineTrans.Add(strCustomName, "");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYSFYYJCWEXMESSAGE + "')", true);
                    }


                    try
                    {
                        //财务费用月结                                             数字*6.9%/12*销售总金额（本家客户）
                        decimal decimalAccountCost = 0;
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["AccountCost"]), out decimalAccountCost);

                        decimal decimalValue4 = 0;
                        decimalValue4 = decimal.Parse(((((decimalAccountCost * 69) / 1000) / 12) * decimalTotalSaleMoney).ToString("#0.000"));
                        if (decimalSaleNumber >= 0)
                        {
                            drCustomValue["AccountCost"] = decimalValue4;
                        }
                        else
                        {
                            drCustomValue["AccountCost"] = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCWFYYJCWEXMESSAGE + "')", true);
                    }


                    try
                    {
                        //招待费用，差旅费，报销人员 TODO待定
                        if (!htTravel.Contains(strCustomName))
                        {
                            foreach (DataRow drTravel in dtTravel.Rows)
                            {
                                if (strCustomName == ShareClass.ObjectToString(drTravel["chuchaikehuquancheng"]).Trim())
                                {
                                    if (ShareClass.ObjectToString(drTravel["zhiliangsunshi"]).Trim() == "1")
                                    {
                                        //质量损失选择是， 此时根据客户全称，总报销费用带过去到‘客户价值表’质量损失处，出差人姓名带过去‘报销人员’
                                        decimal decimalZongBaoXiaoFeiYong = 0;          //总报销费用
                                        decimal.TryParse(ShareClass.ObjectToString(drTravel["zongbaoxiaofeiyong"]).Trim(), out decimalZongBaoXiaoFeiYong);
                                        decimal decimalZDQualityCost = 0;
                                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["QualityCost"]), out decimalZDQualityCost);
                                        drCustomValue["QualityCost"] = decimalZDQualityCost + decimalZongBaoXiaoFeiYong;
                                        drCustomValue["Applyer"] = ShareClass.ObjectToString(drTravel["chuchairenxingming"]);

                                        htTravel.Add(strCustomName, "");

                                        break;
                                    }
                                    else
                                    {
                                        //质量损失选择否，此时根据客户全程，招待费用合计和差旅费用合计、出差人姓名带过去‘客户价值表’招待费用、差旅费用、报销人员
                                        decimal decimalHeJi2 = 0;          //招待费用合计
                                        decimal.TryParse(ShareClass.ObjectToString(drTravel["heji2"]), out decimalHeJi2);
                                        decimal decimalHeJi1 = 0;          //差旅费用合计
                                        decimal.TryParse(ShareClass.ObjectToString(drTravel["heji1"]), out decimalHeJi1);

                                        drCustomValue["ServeCost"] = decimalHeJi2;
                                        drCustomValue["TravelCost"] = decimalHeJi1;
                                        drCustomValue["Applyer"] = ShareClass.ObjectToString(drTravel["chuchairenxingming"]);

                                        htTravel.Add(strCustomName, "");

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZDFYCLFBXRYTODODDCWEXMESSAGE + "')", true);
                    }

                    //业务员工资    销售总额/100
                    //decimal decimalValue5 = 0;
                    //decimalValue5 = decimal.Parse((decimalTotalSaleMoney / 100).ToString("#0.000"));
                    //drCustomValue["SalesmanWages"] = decimalValue5;

                    try
                    {
                        //剩余价值
                        //销售总额/1.17-产品成本/1.17-制造费用-包装费用-质量损失-运输费用*0.93-财务费用-业务费用-差旅费-业务员工资
                        decimal decimalSaleMoney2 = 0;
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SaleMoney"]), out decimalSaleMoney2);
                        decimal decimalProductCost = 0;               //产品成本                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["ProductCost"]), out decimalProductCost);
                        decimal decimalMakeCost2 = 0;               //制造费用                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["MakeCost"]), out decimalMakeCost2);
                        decimal decimalPickCost = 0;               //包装费用                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["PickCost"]), out decimalPickCost);
                        decimal decimalQualityCost = 0;               //质量损失                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["QualityCost"]), out decimalQualityCost);
                        decimal decimalTransportCost = 0;               //运输费用                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TransportCost"]), out decimalTransportCost);
                        decimal decimalAccountCost2 = 0;               //财务费用                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["AccountCost"]), out decimalAccountCost2);
                        decimal decimalServeCost = 0;               //业务费用                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["ServeCost"]), out decimalServeCost);
                        decimal decimalTravelCost = 0;               //差旅费                                                          
                        decimal.TryParse(ShareClass.ObjectToString(drCustomValue["TravelCost"]), out decimalTravelCost);
                        decimal decimalSalesmanWages = 0;               //业务员工资                                                          
                        //decimal.TryParse(ShareClass.ObjectToString(drCustomValue["SalesmanWages"]), out decimalSalesmanWages);
                        decimal decimalValue6 = 0;
                        //decimalValue6 = decimal.Parse(((decimalTotalSaleMoney / 117) * 100 - (decimalProductCost / 117) * 100 - decimalMakeCost2 - decimalPickCost
                        //    - decimalQualityCost - (decimalTransportCost * 93) / 100 - decimalAccountCost2 - decimalServeCost - decimalTravelCost - decimalSalesmanWages).ToString("#0.000"));


                        //销售数量为正数时，剩余价值=销售总额/1.17-产品成本/1.17 -制造费用-包装费用-质量损失-运输费用*0.93-财务费用-业务费用-差旅费
                        //销售数量为负数时，剩余价值=0 -|制造费用|-|包装费用|-质量损失-|运输费用*0.93|-业务费用-差旅费

                        if (decimalSaleNumber >= 0)
                        {
                            decimalValue6 = decimal.Parse(((decimalSaleMoney2 / 117) * 100 - (decimalProductCost / 117) * 100 - decimalMakeCost2 - decimalPickCost
                                - decimalQualityCost - ((decimalTransportCost * 93) / 100) - decimalAccountCost2 - decimalServeCost - decimalTravelCost).ToString("#0.000"));
                        }
                        else
                        {
                            decimalValue6 = decimal.Parse((0 - System.Math.Abs(decimalMakeCost2) - System.Math.Abs(decimalPickCost) - System.Math.Abs(decimalQualityCost) - System.Math.Abs(((decimalTransportCost * 93) / 100)) - System.Math.Abs(decimalServeCost) - System.Math.Abs(decimalTravelCost)).ToString("#0.000"));
                        }
                        drCustomValue["SurplusValue"] = decimalValue6;
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSYJZCWEXMESSAGE + "')", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKHJZBCWEXMESSAGE + "')", true);
        }

        try
        {
            //查询有招待差旅费的客户，但客户不在导入的数据里面         TODO
            string strNotImportCustomHQL = string.Format(@"
                    select * from T_DWTravelexpenses 
                    where SUBSTRING(to_char(tianbiaoriqi, 'yyyy-mm-dd'), 0, 8) = SUBSTRING(to_char('{0}', 'yyyy-mm-dd'), 0, 8)
                    and chuchaikehuquancheng not in 
                    (
                    select CustomName from T_DWCustomImport
                    )
                    and liuchengzhuangtai = '1'", strYear + "-" + strCurrentMonth);
            DataTable dtNotImportCustom = ShareClass.GetDataSetFromSql(strNotImportCustomHQL, "Not").Tables[0];
            if (dtNotImportCustom != null && dtNotImportCustom.Rows.Count > 0)
            {
                foreach (DataRow drNotImportCustom in dtNotImportCustom.Rows)
                {
                    DataRow newRow;
                    newRow = dtCustomValue.NewRow();
                    newRow["CustomName"] = ShareClass.ObjectToString(drNotImportCustom["chuchaikehuquancheng"]);

                    if (ShareClass.ObjectToString(drNotImportCustom["zhiliangsunshi"]).Trim() == "1")
                    {
                        //质量损失选择是， 此时根据客户全称，总报销费用带过去到‘客户价值表’质量损失处，出差人姓名带过去‘报销人员’
                        decimal decimalZongBaoXiaoFeiYong = 0;          //总报销费用
                        decimal.TryParse(ShareClass.ObjectToString(drNotImportCustom["zongbaoxiaofeiyong"]), out decimalZongBaoXiaoFeiYong);

                        newRow["QualityCost"] = decimalZongBaoXiaoFeiYong;
                        newRow["Applyer"] = ShareClass.ObjectToString(drNotImportCustom["chuchairenxingming"]);
                    }
                    else
                    {
                        //质量损失选择否，此时根据客户全程，招待费用合计和差旅费用合计、出差人姓名带过去‘客户价值表’招待费用、差旅费用、报销人员
                        decimal decimalHeJi2 = 0;          //招待费用合计
                        decimal.TryParse(ShareClass.ObjectToString(drNotImportCustom["heji2"]), out decimalHeJi2);
                        decimal decimalHeJi1 = 0;          //差旅费用合计
                        decimal.TryParse(ShareClass.ObjectToString(drNotImportCustom["heji1"]), out decimalHeJi1);

                        newRow["ServeCost"] = decimalHeJi2;
                        newRow["TravelCost"] = decimalHeJi1;
                        newRow["Applyer"] = ShareClass.ObjectToString(drNotImportCustom["chuchairenxingming"]);
                    }

                    dtCustomValue.Rows.Add(newRow);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCYZDCLFDKHDKHBZDRDSJLMCWEXMESSAGE + "')", true);
        }

        return dtCustomValue;
    }


    protected void DG_CustomValue_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_CustomValue.CurrentPageIndex = e.NewPageIndex;
        DataCustomValueBinder();
    }
}