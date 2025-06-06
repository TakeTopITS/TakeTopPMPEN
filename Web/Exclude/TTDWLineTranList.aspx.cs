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

public partial class TTDWLineTranList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataYearMonthBinder();

            DataLingTransportBinder();

            
        }
    }


    private void DataLingTransportBinder()
    {
        DG_LineTrans.CurrentPageIndex = 0;

        string strSeachYear = DDL_SYear.SelectedValue;
        string strSeachMonth = DDL_SMonth.SelectedValue;

        DWLineTransportBLL dWLineTransportBLL = new DWLineTransportBLL();
        string strDWLineTransportHQL = string.Format("from DWLineTransport as dWLineTransport where YearMonth='{0}' order by dWLineTransport.ID desc", strSeachYear + "" + strSeachMonth);
        IList listDWLineTransport = dWLineTransportBLL.GetAllDWLineTransports(strDWLineTransportHQL);

        DG_LineTrans.DataSource = listDWLineTransport;
        DG_LineTrans.DataBind();

        LB_LineTransSql.Text = strDWLineTransportHQL;
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
            DWLineTransportBLL dWLineTransportBLL = new DWLineTransportBLL();
            DWLineTransport dWLineTransport = new DWLineTransport();
            string strCustomName = TXT_CustomName.Text.Trim();
            string strAmount = TXT_Amount.Text.Trim();
            string strCost = TXT_Cost.Text.Trim();
            string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;

            if (string.IsNullOrEmpty(strCustomName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKHMCBYXWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strAmount))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSLBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strAmount))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSLBXWXSHZZS+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFYBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFYBXWXSHZZS+"')", true);
                return;
            }

            decimal decimalAmount = 0;
            decimal.TryParse(strAmount, out decimalAmount);
            decimal decimalCost = 0;
            decimal.TryParse(strCost, out decimalCost);

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strDWLineTransportHQL = string.Format(@"from DWLineTransport as dWLineTransport where ID = " + strID);
                IList lstDWLineTransport = dWLineTransportBLL.GetAllDWLineTransports(strDWLineTransportHQL);
                if (lstDWLineTransport != null && lstDWLineTransport.Count > 0)
                {
                    dWLineTransport = (DWLineTransport)lstDWLineTransport[0];

                    dWLineTransport.CustomName = strCustomName;
                    dWLineTransport.Amount = decimalAmount;
                    dWLineTransport.Cost = decimalCost;
                    dWLineTransport.YearMonth = strYearMonth;

                    dWLineTransportBLL.UpdateDWLineTransport(dWLineTransport, int.Parse(strID));
                }
            }
            else
            {
                dWLineTransport.CustomName = strCustomName;
                dWLineTransport.Amount = decimalAmount;
                dWLineTransport.Cost = decimalCost;
                dWLineTransport.YearMonth = strYearMonth;

                dWLineTransportBLL.AddDWLineTransport(dWLineTransport);

                string strSelectMaxHQL = "select ID from T_DWLineTransport order by ID desc limit 1";
                DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
                if (dtMaxID != null && dtMaxID.Rows.Count > 0)
                {
                    int intID = 0;
                    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                    TXT_ID.Text = intID.ToString();
                }
            }

            DataLingTransportBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex) { }
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_LineTrans.Items.Count; i++)
        {
            DG_LineTrans.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";

        TXT_CustomName.Text = "";
        TXT_Amount.Text = "";
        TXT_Cost.Text = "";
    }


    protected void BT_Seach_Click(object sender, EventArgs e)
    {
        DataLingTransportBinder();
    }

    protected void DG_LineTrans_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_LineTrans.Items.Count; i++)
                {
                    DG_LineTrans.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                DWLineTransportBLL dWLineTransportBLL = new DWLineTransportBLL();
                string strDWLineTransportHQL = string.Format(@"from DWLineTransport as dWLineTransport where ID = " + strCmdArgu);
                IList lstDWLineTransport = dWLineTransportBLL.GetAllDWLineTransports(strDWLineTransportHQL);
                if (lstDWLineTransport != null && lstDWLineTransport.Count > 0)
                {
                    DWLineTransport dWLineTransport = (DWLineTransport)lstDWLineTransport[0];

                    HF_ID.Value = dWLineTransport.ID.ToString();
                    TXT_ID.Text = dWLineTransport.ID.ToString();
                    TXT_CustomName.Text = dWLineTransport.CustomName;
                    TXT_Amount.Text = dWLineTransport.Amount.ToString();
                    TXT_Cost.Text = dWLineTransport.Cost.ToString();
                    string strYearMonth = dWLineTransport.YearMonth;
                    DDL_Year.SelectedValue = strYearMonth.Substring(0, 4);
                    DDL_Month.SelectedValue = strYearMonth.Substring(4, 2);
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWLineTransportBLL dWLineTransportBLL = new DWLineTransportBLL();
                string strDWLineTransportHQL = string.Format(@"from DWLineTransport as dWLineTransport where ID = " + strCmdArgu);
                IList lstDWLineTransport = dWLineTransportBLL.GetAllDWLineTransports(strDWLineTransportHQL);
                if (lstDWLineTransport != null && lstDWLineTransport.Count > 0)
                {
                    DWLineTransport dWLineTransport = (DWLineTransport)lstDWLineTransport[0];

                    dWLineTransportBLL.DeleteDWLineTransport(dWLineTransport);

                    //重新加载列表
                    DataLingTransportBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }
            }
        }
        catch (Exception ex) { }
    }


    protected void DG_LineTrans_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_LineTrans.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_LineTransSql.Text.Trim();
        DWLineTransportBLL dWLineTransportBLL = new DWLineTransportBLL();
        IList listDWLineTransport = dWLineTransportBLL.GetAllDWLineTransports(strHQL);

        DG_LineTrans.DataSource = listDWLineTransport;
        DG_LineTrans.DataBind();
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
            string strDLCode = string.Empty;
            string strDLName = string.Empty;
            lineNumber++;
            try
            {
                string strCustomName = ShareClass.ObjectToString(row["客户名称"]);
                if (string.IsNullOrEmpty(strCustomName))
                {
                    resultMsg += string.Format("第{0}行，客户名称不能为空<br/>", lineNumber);
                    continue;
                }
                
                //数量
                string strAmount = ShareClass.ObjectToString(row["数量"]);
                if (string.IsNullOrEmpty(strAmount))
                {
                    resultMsg += string.Format("第{0}行，数量不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsAmount = ShareClass.CheckIsNumber(strAmount);
                    if (!IsAmount)
                    {
                        resultMsg += string.Format("第{0}行，数量只能是小数<br/>", lineNumber);
                        continue;
                    }
                }

                //费用
                string strCost = ShareClass.ObjectToString(row["费用"]);
                if (string.IsNullOrEmpty(strCost))
                {
                    resultMsg += string.Format("第{0}行，费用不能为空<br/>", lineNumber);
                    continue;
                }
                else
                {
                    //必须为整形或者带小数点
                    bool IsCost = ShareClass.CheckIsNumber(strCost);
                    if (!IsCost)
                    {
                        resultMsg += string.Format("第{0}行，费用只能是小数<br/>", lineNumber);
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

        DWLineTransportBLL dWLineTransportBLL = new DWLineTransportBLL();

        foreach (DataRow row in dtExcel.Rows)
        {
            string strCustomName = string.Empty;
            string strAmount = string.Empty;
            string strCost = string.Empty;
            string strYearMonth = DDL_Year.SelectedValue + "" + DDL_Month.SelectedValue;

            lineNumber++;
            strCustomName = ShareClass.ObjectToString(row["客户名称"]);
            strAmount = ShareClass.ObjectToString(row["数量"]);
            decimal decimalAmount = 0;
            decimal.TryParse(strAmount, out decimalAmount);
            strCost = ShareClass.ObjectToString(row["费用"]);
            decimal decimalCost = 0;
            decimal.TryParse(strCost, out decimalCost);

            DWLineTransport dWLineTransport = new DWLineTransport();
            dWLineTransport.CustomName = strCustomName;
            dWLineTransport.Amount = decimalAmount;
            dWLineTransport.Cost = decimalCost;
            dWLineTransport.YearMonth = strYearMonth;


            dWLineTransportBLL.AddDWLineTransport(dWLineTransport);

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
            DataLingTransportBinder();
            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
        }

        return false;
    }
}