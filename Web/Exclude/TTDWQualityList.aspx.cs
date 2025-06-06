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

public partial class TTDWQualityList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataYearMonthBinder();

            DataQualityBinder();


        }
    }


    private void DataQualityBinder()
    {
        DG_Quality.CurrentPageIndex = 0;

        string strSeachYear = DDL_SYear.SelectedValue;
        string strSeachMonth = DDL_SMonth.SelectedValue;

        DWQualityCostBLL dWQualityCostBLL = new DWQualityCostBLL();
        string strDWQualityCostHQL = string.Format("from DWQualityCost as dWQualityCost where YearMonth='{0}' order by dWQualityCost.ID desc", strSeachYear + "" + strSeachMonth);
        IList listDWQualityCost = dWQualityCostBLL.GetAllDWQualityCosts(strDWQualityCostHQL);

        DG_Quality.DataSource = listDWQualityCost;
        DG_Quality.DataBind();

        LB_QualitySql.Text = strDWQualityCostHQL;
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
            DWQualityCostBLL dWQualityCostBLL = new DWQualityCostBLL();
            DWQualityCost dWQualityCost = new DWQualityCost();
            string strCustomName = TXT_CustomName.Text.Trim();
            string strPayMoney = TXT_PayMoney.Text.Trim();
            string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;

            string strWorkshop = TXT_Workshop.Text.Trim();


            if (string.IsNullOrEmpty(strCustomName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKHMCBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCustomName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKHMCBNBHFFZFC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPayMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPLZHCFYBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strPayMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZPLZHCFYBXWXSHZZS+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWorkshop))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCJBNBHFFZFC+"')", true);
                return;
            }

            decimal decimalPayMoney = 0;
            decimal.TryParse(strPayMoney, out decimalPayMoney);

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strDWQualityCostHQL = string.Format(@"from DWQualityCost as dWQualityCost where ID = " + strID);
                IList lstDWQualityCost = dWQualityCostBLL.GetAllDWQualityCosts(strDWQualityCostHQL);
                if (lstDWQualityCost != null && lstDWQualityCost.Count > 0)
                {
                    dWQualityCost = (DWQualityCost)lstDWQualityCost[0];

                    dWQualityCost.CustomName = strCustomName;
                    dWQualityCost.PayMoney = decimalPayMoney;
                    dWQualityCost.YearMonth = strYearMonth;

                    dWQualityCost.Workshop = strWorkshop;

                    dWQualityCostBLL.UpdateDWQualityCost(dWQualityCost, int.Parse(strID));
                }
            }
            else
            {
                dWQualityCost.CustomName = strCustomName;
                dWQualityCost.PayMoney = decimalPayMoney;
                dWQualityCost.YearMonth = strYearMonth;

                dWQualityCost.Workshop = strWorkshop;

                dWQualityCostBLL.AddDWQualityCost(dWQualityCost);

                string strSelectMaxHQL = "select ID from T_DWQualityCost order by ID desc limit 1";
                DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
                if (dtMaxID != null && dtMaxID.Rows.Count > 0)
                {
                    int intID = 0;
                    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                    TXT_ID.Text = intID.ToString();
                }
            }

            DataQualityBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex) { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Quality.Items.Count; i++)
        {
            DG_Quality.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";

        TXT_CustomName.Text = "";
        TXT_PayMoney.Text = "";
        TXT_Workshop.Text = "";
    }


    protected void BT_Seach_Click(object sender, EventArgs e)
    {
        DataQualityBinder();
    }

    protected void DG_Quality_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Quality.Items.Count; i++)
                {
                    DG_Quality.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                DWQualityCostBLL dWQualityCostBLL = new DWQualityCostBLL();
                string strDWQualityCostHQL = string.Format(@"from DWQualityCost as dWQualityCost where ID = " + strCmdArgu);
                IList lstDWQualityCost = dWQualityCostBLL.GetAllDWQualityCosts(strDWQualityCostHQL);
                if (lstDWQualityCost != null && lstDWQualityCost.Count > 0)
                {
                    DWQualityCost dWQualityCost = (DWQualityCost)lstDWQualityCost[0];

                    HF_ID.Value = dWQualityCost.ID.ToString();
                    TXT_ID.Text = dWQualityCost.ID.ToString();
                    TXT_CustomName.Text = dWQualityCost.CustomName;
                    TXT_PayMoney.Text = dWQualityCost.PayMoney.ToString();
                    string strYearMonth = dWQualityCost.YearMonth;
                    DDL_Year.SelectedValue = strYearMonth.Substring(0, 4);
                    DDL_Month.SelectedValue = strYearMonth.Substring(4, 2);

                    TXT_Workshop.Text = dWQualityCost.Workshop;
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWQualityCostBLL dWQualityCostBLL = new DWQualityCostBLL();
                string strDWQualityCostHQL = string.Format(@"from DWQualityCost as dWQualityCost where ID = " + strCmdArgu);
                IList lstDWQualityCost = dWQualityCostBLL.GetAllDWQualityCosts(strDWQualityCostHQL);
                if (lstDWQualityCost != null && lstDWQualityCost.Count > 0)
                {
                    DWQualityCost dWQualityCost = (DWQualityCost)lstDWQualityCost[0];

                    dWQualityCostBLL.DeleteDWQualityCost(dWQualityCost);

                    DataQualityBinder();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }
            }
        }
        catch (Exception ex) { }
    }



    protected void DG_Quality_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Quality.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_QualitySql.Text.Trim();
        DWQualityCostBLL dWQualityCostBLL = new DWQualityCostBLL();
        IList listDWQualityCost = dWQualityCostBLL.GetAllDWQualityCosts(strHQL);

        DG_Quality.DataSource = listDWQualityCost;
        DG_Quality.DataBind();
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
                string strMakeType = ShareClass.ObjectToString(row["客户名称"]);
                if (string.IsNullOrEmpty(strMakeType))
                {
                    resultMsg += string.Format("第{0}行，类型不能为空<br/>", lineNumber);
                    continue;
                }


                //赔料
                string strPayMoney = ShareClass.ObjectToString(row["赔料"]);
                if (string.IsNullOrEmpty(strPayMoney))
                {
                    resultMsg += string.Format("第{0}行，赔料不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsPayMoney = ShareClass.CheckIsNumber(strPayMoney);
                    if (!IsPayMoney)
                    {
                        resultMsg += string.Format("第{0}行，赔料只能是小数<br/>", lineNumber);
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

        DWQualityCostBLL dWQualityCostBLL = new DWQualityCostBLL();

        foreach (DataRow row in dtExcel.Rows)
        {
            string strCustomName = string.Empty;
            string strPayMoney = string.Empty;

            string strWorkshop = string.Empty;

            string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;

            lineNumber++;
            strCustomName = ShareClass.ObjectToString(row["客户名称"]);
            strPayMoney = ShareClass.ObjectToString(row["赔料"]);

            strWorkshop = ShareClass.ObjectToString(row["车间"]);

            decimal decimalPayMoney = 0;
            decimal.TryParse(strPayMoney, out decimalPayMoney);

            DWQualityCost dWQualityCost = new DWQualityCost();
            dWQualityCost.CustomName = strCustomName;
            dWQualityCost.PayMoney = decimalPayMoney;
            dWQualityCost.YearMonth = strYearMonth;

            dWQualityCost.Workshop = strWorkshop;

            dWQualityCostBLL.AddDWQualityCost(dWQualityCost);

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
            DataQualityBinder();

            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
        }

        return false;
    }
}