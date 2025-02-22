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

public partial class TTDWProductList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataProductBinder();

            DataProductTypeBinder();
        }
    }


    private void DataProductBinder()
    {
        DG_Product.CurrentPageIndex = 0;

        string strDWProductHQL = "select p.*,t.ProductType from T_DWProduct p left join T_DWProductType t on p.TypeID=t.ID order by p.ID desc";
        DataTable dtProduct = ShareClass.GetDataSetFromSql(strDWProductHQL, "DWProduct").Tables[0];

        DG_Product.DataSource = dtProduct;
        DG_Product.DataBind();

        LB_ProductSql.Text = strDWProductHQL;
    }

    private void DataProductTypeBinder()
    {
        DWProductTypeBLL dWProductTypeBLL = new DWProductTypeBLL();
        string strDWProductTypeHQL = "from DWProductType as dWProductType order by dWProductType.ID desc";
        IList listDWProductType = dWProductTypeBLL.GetAllDWProductTypes(strDWProductTypeHQL);

        DDL_Type.DataSource = listDWProductType;
        DDL_Type.DataBind();

        DDL_Type.Items.Insert(0, new ListItem("", ""));

        DDL_SProduct.DataSource = listDWProductType;
        DDL_SProduct.DataBind();

        DDL_SProduct.Items.Insert(0, new ListItem("全部", ""));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DWProductBLL dWProductBLL = new DWProductBLL();
            DWProduct dWProduct = new DWProduct();
            string strProductCode = TXT_ProductCode.Text.Trim();
            string strProductName = TXT_ProductName.Text.Trim();
            string strProductType = DDL_Type.SelectedValue;

            if (string.IsNullOrEmpty(strProductCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPBHBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strProductCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPBHBNBHFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strProductName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPMCBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strProductName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPMCBNBHFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strProductType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXBYXWKBC+"')", true);
                return;
            }

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;

                string strCheckProductHQL = string.Format(@"select * from T_DWProduct where ProductCode = '{0}'", strProductCode);
                DataTable dtCheckProduct = ShareClass.GetDataSetFromSql(strCheckProductHQL, "CheckProduct").Tables[0];
                if (dtCheckProduct != null && dtCheckProduct.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCheckProduct.Rows)
                    {
                        if (ShareClass.ObjectToString(dr["ID"]) != strID)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPBHYJCZZXTX+"')", true);
                            return;
                        }
                    }
                }

                string strProductHQL = string.Format(@"from DWProduct as dWProduct where ID = " + strID);
                IList lstProduct = dWProductBLL.GetAllDWProducts(strProductHQL);
                if (lstProduct != null && lstProduct.Count > 0)
                {
                    dWProduct = (DWProduct)lstProduct[0];

                    dWProduct.ProductCode = strProductCode;
                    dWProduct.ProductName = strProductName;
                    int intTypeID = 0;
                    int.TryParse(strProductType, out intTypeID);
                    dWProduct.TypeID = intTypeID;

                    dWProductBLL.UpdateDWProduct(dWProduct, int.Parse(strID));


                }
            }
            else
            {
                string strCheckProductHQL = string.Format(@"select * from T_DWProduct where ProductCode = '{0}'", strProductCode);
                DataTable dtCheckProduct = ShareClass.GetDataSetFromSql(strCheckProductHQL, "CheckProduct").Tables[0];
                if (dtCheckProduct != null && dtCheckProduct.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCPBHYJCZZXTX+"')", true);
                    return;
                }

                dWProduct.ProductCode = strProductCode;
                dWProduct.ProductName = strProductName;
                int intTypeID = 0;
                int.TryParse(strProductType, out intTypeID);
                dWProduct.TypeID = intTypeID;

                dWProductBLL.AddDWProduct(dWProduct);



                string strSelectMaxHQL = "select ID from T_DWProduct order by ID desc limit 1";
                DataTable dtMaxID = ShareClass.GetDataSetFromSql(strSelectMaxHQL, "strSelectMaxHQL").Tables[0];
                if (dtMaxID != null && dtMaxID.Rows.Count > 0)
                {
                    int intID = 0;
                    int.TryParse(dtMaxID.Rows[0]["ID"] == DBNull.Value ? "0" : dtMaxID.Rows[0]["ID"].ToString(), out intID);
                    TXT_ID.Text = intID.ToString();
                }
            }

            DataProductBinder();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch (Exception ex)
        { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Product.Items.Count; i++)
        {
            DG_Product.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";
        TXT_ProductCode.Text = "";
        TXT_ProductName.Text = "";
        DDL_Type.SelectedValue = "";

    }


    protected void DG_Product_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Product.Items.Count; i++)
                {
                    DG_Product.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                DWProductBLL dWProductBLL = new DWProductBLL();
                string strProductHQL = string.Format(@"from DWProduct as dWProduct where ID = " + strCmdArgu);
                IList lstProduct = dWProductBLL.GetAllDWProducts(strProductHQL);
                if (lstProduct != null && lstProduct.Count > 0)
                {
                    DWProduct dWProduct = (DWProduct)lstProduct[0];

                    HF_ID.Value = dWProduct.ID.ToString();
                    TXT_ID.Text = dWProduct.ID.ToString();
                    TXT_ProductCode.Text = dWProduct.ProductCode;
                    TXT_ProductName.Text = dWProduct.ProductName;
                    DDL_Type.SelectedValue = dWProduct.TypeID.ToString();
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                DWProductBLL dWProductBLL = new DWProductBLL();
                string strProductHQL = string.Format(@"from DWProduct as dWProduct where ID = " + strCmdArgu);
                IList lstProduct = dWProductBLL.GetAllDWProducts(strProductHQL);
                if (lstProduct != null && lstProduct.Count > 0)
                {
                    DWProduct dWProduct = (DWProduct)lstProduct[0];

                    dWProductBLL.DeleteDWProduct(dWProduct);

                    DataProductBinder();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                }
            }
        }
        catch (Exception ex) { }
    }


    protected void DG_Product_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Product.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_ProductSql.Text.Trim();
        DataTable dtProduct = ShareClass.GetDataSetFromSql(strHQL, "DWProduct").Tables[0];

        DG_Product.DataSource = dtProduct;
        DG_Product.DataBind();
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
                string strProductCode = ShareClass.ObjectToString(row["产品编号"]);
                if (string.IsNullOrEmpty(strProductCode))
                {
                    resultMsg += string.Format("第{0}行，产品编号不能为空<br/>", lineNumber);
                    continue;
                }

                string strProductName = ShareClass.ObjectToString(row["产品名称"]);
                if (string.IsNullOrEmpty(strProductName))
                {
                    resultMsg += string.Format("第{0}行，产品名称不能为空<br/>", lineNumber);
                    continue;
                }

                string strProductType = ShareClass.ObjectToString(row["产品类型"]);
                if (string.IsNullOrEmpty(strProductType))
                {
                    resultMsg += string.Format("第{0}行，产品类型不能为空<br/>", lineNumber);
                    continue;
                }
                else {
                    string strProductTypeHQL = string.Format("select * from T_DWProductType where ProductType = '{0}'", strProductType);
                    DataTable dtProductType = ShareClass.GetDataSetFromSql(strProductTypeHQL, "ProductType").Tables[0];
                    if (dtProductType == null || dtProductType.Rows.Count == 0)
                    {
                        resultMsg += string.Format("第{0}行，产品类型:{1}在产品类型表中不存在<br/>", lineNumber, strProductType);
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

        DWProductBLL dWProductBLL = new DWProductBLL();

        foreach (DataRow row in dtExcel.Rows)
        {
            string strProductCode = string.Empty;
            string strProductName = string.Empty;
            string strProductType = string.Empty;

            lineNumber++;
            strProductCode = ShareClass.ObjectToString(row["产品编号"]);
            strProductName = ShareClass.ObjectToString(row["产品名称"]);
            strProductType = ShareClass.ObjectToString(row["产品类型"]);

            string strProductTypeHQL = string.Format("select ID from T_DWProductType where ProductType = '{0}'", strProductType);
            DataTable dtProductType = ShareClass.GetDataSetFromSql(strProductTypeHQL, "ProductType").Tables[0];
            int intTypeID = 0;
            if (dtProductType != null && dtProductType.Rows.Count > 0)
            {
                int.TryParse(ShareClass.ObjectToString(dtProductType.Rows[0]["ID"]), out intTypeID);
            }

            DWProduct dWProduct = new DWProduct();
            dWProduct.ProductCode = strProductCode;
            dWProduct.ProductName = strProductName;
            dWProduct.TypeID = intTypeID;

            dWProductBLL.AddDWProduct(dWProduct);

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
            DataProductBinder();
            return true;
        }
        else
        {
            resultMsg += string.Format("<br/>未导入数据， 共有 {0} 条数据验证失败", dtExcel.Rows.Count - successCount);
        }

        return false;
    }

    protected void DDL_SProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        DG_Product.CurrentPageIndex = 0;

        string strDWProductHQL = "select p.*,t.ProductType from T_DWProduct p left join T_DWProductType t on p.TypeID=t.ID ";
        string strProductType = DDL_SProduct.SelectedValue;
        if (!string.IsNullOrEmpty(strProductType))
        {
            strDWProductHQL += " where p.TypeID = " + strProductType;
        }
        strDWProductHQL += " order by p.ID desc";
        DataTable dtProduct = ShareClass.GetDataSetFromSql(strDWProductHQL, "DWProduct").Tables[0];

        DG_Product.DataSource = dtProduct;
        DG_Product.DataBind();

        LB_ProductSql.Text = strDWProductHQL;
    }


    protected void BT_ProducteAsc_Click(object sender, EventArgs e)
    {
        DG_Product.CurrentPageIndex = 0;

        string strDWProductHQL = "select p.*,t.ProductType from T_DWProduct p left join T_DWProductType t on p.TypeID=t.ID order by p.ProductCode asc";
        DataTable dtProduct = ShareClass.GetDataSetFromSql(strDWProductHQL, "DWProduct").Tables[0];

        DG_Product.DataSource = dtProduct;
        DG_Product.DataBind();

        LB_ProductSql.Text = strDWProductHQL;
    }


    protected void BT_ProductDesc_Click(object sender, EventArgs e)
    {
        DG_Product.CurrentPageIndex = 0;

        string strDWProductHQL = "select p.*,t.ProductType from T_DWProduct p left join T_DWProductType t on p.TypeID=t.ID order by p.ProductCode desc";
        DataTable dtProduct = ShareClass.GetDataSetFromSql(strDWProductHQL, "DWProduct").Tables[0];

        DG_Product.DataSource = dtProduct;
        DG_Product.DataBind();

        LB_ProductSql.Text = strDWProductHQL;
    }
}