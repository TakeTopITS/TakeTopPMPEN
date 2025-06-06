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

public partial class TTDWMakeList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataYearMonthBinder();

            DataMakeBinder();

            DataProductTypeBinder();
        }
    }


    private void DataMakeBinder()
    {
        DG_Make.CurrentPageIndex = 0;

        string strSeachYear = DDL_SYear.SelectedValue;
        string strSeachMonth = DDL_SMonth.SelectedValue;

        DWMakeCostBLL dWMakeCostBLL = new DWMakeCostBLL();
        string strDWMakeCostHQL = string.Format("from DWMakeCost as dWMakeCost where YearMonth='{0}' order by dWMakeCost.ID desc", strSeachYear + "" + strSeachMonth);
        IList listDWMakeCost = dWMakeCostBLL.GetAllDWMakeCosts(strDWMakeCostHQL);

        DG_Make.DataSource = listDWMakeCost;
        DG_Make.DataBind();

        LB_MakeSql.Text = strDWMakeCostHQL;
    }

    private void DataProductTypeBinder()
    {
        DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
        string strDWProductTypeHQL = "from DWProductType as dWProductType order by dWProductType.ID desc";
        IList listDWProductType = dWProductTypeBLL.GetAllDWProductTypes(strDWProductTypeHQL);

        DDL_MakeType.DataSource = listDWProductType;
        DDL_MakeType.DataBind();

        DDL_MakeType.Items.Insert(0, new ListItem("", ""));
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


        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(2).Year.ToString(), DateTime.Now.AddYears(2).Year.ToString()));
        DDL_SYear.SelectedValue = DateTime.Now.Year.ToString();

        for (int i = 1; i <= 12; i++)
        {
            if (i < 10)
            {
                DDL_SMonth.Items.Add(new ListItem("0" + i, "0" + i));
            }
            else
            {
                DDL_SMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        string strSCurrentMonth = DateTime.Now.Month.ToString();
        if (strSCurrentMonth.Length > 1)
        {
            DDL_SMonth.SelectedValue = strSCurrentMonth;
        }
        else
        {
            DDL_SMonth.SelectedValue = "0" + strSCurrentMonth;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DWMakeCostBLL dWMakeCostBLL = new DWMakeCostBLL();
            DWMakeCost dWMakeCost = new DWMakeCost();
            string strMakeType = DDL_MakeType.SelectedValue;
            string strCost = TXT_Cost.Text.Trim();
            string strTonCost = TXT_TonCost.Text.Trim();
            string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;

            if (string.IsNullOrEmpty(strMakeType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXBYXWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZZCBBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZZCBBXWXSHZZS+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strTonCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDHBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strTonCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDHBXWXSHZZS+"')", true);
                return;
            }

            decimal decimalCost = 0;
            decimal.TryParse(strCost, out decimalCost);
            decimal decimalTonCost = 0;
            decimal.TryParse(strTonCost, out decimalTonCost);

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strDWMakeCostHQL = string.Format(@"from DWMakeCost as DWMakeCost where ID = " + strID);
                IList lstDWMakeCost = dWMakeCostBLL.GetAllDWMakeCosts(strDWMakeCostHQL);
                if (lstDWMakeCost != null && lstDWMakeCost.Count > 0)
                {
                    dWMakeCost = (DWMakeCost)lstDWMakeCost[0];

                    dWMakeCost.MakeType = strMakeType;
                    dWMakeCost.Cost = decimalCost;
                    dWMakeCost.TonCost = decimalTonCost;
                    dWMakeCost.YearMonth = strYearMonth;

                    dWMakeCostBLL.UpdateDWMakeCost(dWMakeCost, int.Parse(strID));
                }
            }
            else
            {
                dWMakeCost.MakeType = strMakeType;
                dWMakeCost.Cost = decimalCost;
                dWMakeCost.TonCost = decimalTonCost;
                dWMakeCost.YearMonth = strYearMonth;

                dWMakeCostBLL.AddDWMakeCost(dWMakeCost);

                string strSelectMaxHQL = "select ID from T_DWMakeCost order by ID desc limit 1";
                DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
                if (dtMaxID != null && dtMaxID.Rows.Count > 0)
                {
                    int intID = 0;
                    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                    TXT_ID.Text = intID.ToString();
                }
            }

            DataMakeBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex) { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Make.Items.Count; i++)
        {
            DG_Make.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";

        DDL_MakeType.SelectedValue = "";
        TXT_Cost.Text = "";
        TXT_TonCost.Text = "";
    }

    protected void BT_Seach_Click(object sender, EventArgs e)
    {
        DataMakeBinder();
    }


    protected void DG_Make_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Make.Items.Count; i++)
                {
                    DG_Make.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                DWMakeCostBLL dWMakeCostBLL = new DWMakeCostBLL();
                string strDWMakeCostHQL = string.Format(@"from DWMakeCost as dWMakeCost where ID = " + strCmdArgu);
                IList lstDWMakeCost = dWMakeCostBLL.GetAllDWMakeCosts(strDWMakeCostHQL);
                if (lstDWMakeCost != null && lstDWMakeCost.Count > 0)
                {
                    DWMakeCost dWMakeCost = (DWMakeCost)lstDWMakeCost[0];

                    HF_ID.Value = dWMakeCost.ID.ToString();
                    TXT_ID.Text = dWMakeCost.ID.ToString();
                    DDL_MakeType.SelectedValue = dWMakeCost.MakeType;
                    TXT_Cost.Text = dWMakeCost.Cost.ToString();
                    TXT_TonCost.Text = dWMakeCost.TonCost.ToString();
                    string strYearMonth = dWMakeCost.YearMonth;
                    DDL_Year.SelectedValue = strYearMonth.Substring(0, 4);
                    DDL_Month.SelectedValue = strYearMonth.Substring(4, 2);
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWMakeCostBLL dWMakeCostBLL = new DWMakeCostBLL();
                string strDWMakeCostHQL = string.Format(@"from DWMakeCost as dWMakeCost where ID = " + strCmdArgu);
                IList lstDWMakeCost = dWMakeCostBLL.GetAllDWMakeCosts(strDWMakeCostHQL);
                if (lstDWMakeCost != null && lstDWMakeCost.Count > 0)
                {
                    DWMakeCost dWMakeCost = (DWMakeCost)lstDWMakeCost[0];

                    dWMakeCostBLL.DeleteDWMakeCost(dWMakeCost);

                    DataMakeBinder();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void DG_Make_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Make.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_MakeSql.Text.Trim();
        DWMakeCostBLL dWMakeCostBLL = new DWMakeCostBLL();
        IList listDWMakeCost = dWMakeCostBLL.GetAllDWMakeCosts(strHQL);

        DG_Make.DataSource = listDWMakeCost;
        DG_Make.DataBind();
    }


    protected void btnImport_Click(object sender, EventArgs e)
    {
        string fileName = fileExcel.PostedFile.FileName.Substring(fileExcel.PostedFile.FileName.LastIndexOf(".")).ToLower();
        string url = string.Format("Doc/XML/{0}{1}", "DW"+DateTime.Now.ToString("yyyyMMddHHmmss"), fileName);
        string destFilePath = Server.MapPath(url);
        fileExcel.SaveAs(destFilePath);
        DataTable dtExcel = null;
        string resultMsg = string.Empty;
        try
        {
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
            lineNumber++;
            try
            {
                string strMakeType = ShareClass.ObjectToString(row["类型"]);
                if (string.IsNullOrEmpty(strMakeType))
                {
                    resultMsg += string.Format("第{0}行，类型不能为空<br/>", lineNumber);
                    continue;
                }


                //制造成本
                string strCost = ShareClass.ObjectToString(row["制造成本"]);
                if (string.IsNullOrEmpty(strCost))
                {
                    resultMsg += string.Format("第{0}行，制造成本不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsCost = ShareClass.CheckIsNumber(strCost);
                    if (!IsCost)
                    {
                        resultMsg += string.Format("第{0}行，制造成本只能是小数<br/>", lineNumber);
                        continue;
                    }
                }

                //吨耗
                string strTonCost = ShareClass.ObjectToString(row["吨耗"]);
                if (string.IsNullOrEmpty(strTonCost))
                {
                    resultMsg += string.Format("第{0}行，吨耗不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsTonCost = ShareClass.CheckIsNumber(strTonCost);
                    if (!IsTonCost)
                    {
                        resultMsg += string.Format("第{0}行，吨耗只能是小数<br/>", lineNumber);
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
        int successCount = 0;
        int lineNumber = 0;

        DWMakeCostBLL dWMakeCostBLL = new DWMakeCostBLL();

        foreach (DataRow row in dtExcel.Rows)
        {
            string strMakeType = string.Empty;
            string strCost = string.Empty;
            string strTonCost = string.Empty;
            string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;

            lineNumber++;
            strMakeType = ShareClass.ObjectToString(row["类型"]);
            strCost = ShareClass.ObjectToString(row["制造成本"]);
            decimal decimalCost = 0;
            decimal.TryParse(strCost, out decimalCost);
            strTonCost = ShareClass.ObjectToString(row["吨耗"]);
            decimal decimalTonCost = 0;
            decimal.TryParse(strTonCost, out decimalTonCost);

            DWMakeCost dWMakeCost = new DWMakeCost();
            dWMakeCost.MakeType = strMakeType;
            dWMakeCost.Cost = decimalCost;
            dWMakeCost.TonCost = decimalTonCost;
            dWMakeCost.YearMonth = strYearMonth;

            dWMakeCostBLL.AddDWMakeCost(dWMakeCost);

            successCount++;
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
            DataMakeBinder();

            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
        }

        return false;
    }
}