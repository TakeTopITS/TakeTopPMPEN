using System;
using System.Resources;
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
using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;
using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using System.IO;
using System.Data.OleDb;

public partial class TTCustomerAssetRecord : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode.Trim();
        LB_UserName.Text = ShareClass.GetUserName(strUserCode.Trim());

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from AssetType as assetType Order by assetType.SortNumber ASC";
            AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
            lst = assetTypeBLL.GetAllAssetTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            LoadCurrency();

            LoadPurchaseOrder(strUserCode.Trim());
        }
    }

    protected void LoadCurrency()
    {
        string strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC ";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        DL_CurrencyType.DataSource = lst;
        DL_CurrencyType.DataBind();
    }

    protected void ClearData()
    {
        TB_AssetCode.Text = "";
        TB_AssetName.Text = "";
        TB_ModelNumber.Text = "";
        NB_Number.Amount = 1;
        TB_Spec.Text = "";
        NB_Price.Amount = 0;
        LB_ID.Text = "";
        TB_PurReason.Text = "";
        TB_Supplier.Text = "";
        TB_SupplierPhone.Text = "";
        DL_Status.SelectedValue = "New";

    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;

            AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
            lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
            AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

            TB_AssetCode.Text = assetPurRecord.AssetCode;
            TB_AssetName.Text = assetPurRecord.AssetName;
            TB_ModelNumber.Text = assetPurRecord.ModelNumber;
            TB_Spec.Text = assetPurRecord.Spec;
            TB_PurReason.Text = assetPurRecord.PurReason;
            NB_Price.Amount = assetPurRecord.Price;
            TB_Supplier.Text = assetPurRecord.Supplier;
            TB_SupplierPhone.Text = assetPurRecord.SupplierPhone;
            DL_Type.SelectedValue = assetPurRecord.Type;
            DL_Unit.SelectedValue = assetPurRecord.Unit;
            NB_Number.Amount = assetPurRecord.Number;
            DL_Status.SelectedValue = assetPurRecord.Status.Trim();

            if (assetPurRecord.ApplicantCode.Trim() == strUserCode.Trim())
            {
                BT_New.Enabled = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Visible = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_New.Enabled = true;
                BT_Update.Visible = false;
                BT_Delete.Visible = false;
            }
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPOID;
            IList lst;

            strPOID = ((Button)e.Item.FindControl("BT_POID")).Text.Trim();

            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from AssetPurchaseOrder as assetpurchaseOrder where assetpurchaseOrder.POID = " + strPOID;
            AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
            lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
            AssetPurchaseOrder purchaseOrder = (AssetPurchaseOrder)lst[0];

            LB_POID.Text = purchaseOrder.POID.ToString();
            TB_POName.Text = purchaseOrder.POName.Trim();
            NB_Amount.Amount = purchaseOrder.Amount;
            TB_Comment.Text = purchaseOrder.Comment.Trim();
            DL_POStatus.SelectedValue = purchaseOrder.Status.Trim();
            DL_CurrencyType.Text = purchaseOrder.CurrencyType;

            LoadPurchaseOrderDetail(strPOID);

            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            if (purchaseOrder.OperatorCode.Trim() == strUserCode.Trim())
            {
                BT_UpdatePO.Visible = true;
                BT_UpdatePO.Enabled = true;
                BT_DeletePO.Visible = true;
                BT_DeletePO.Enabled = true;
                BT_New.Visible = true;
                BT_New.Enabled = true;
            }
            else
            {
                BT_UpdatePO.Visible = false;
                BT_DeletePO.Visible = false;

                BT_New.Visible = false;
            }

            ClearData();
        }
    }

    protected void BT_AddPO_Click(object sender, EventArgs e)
    {
        if (TB_POName.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSMCBNWKJC").ToString().Trim() + "')", true);
            TB_POName.Focus();
            return;
        }
        AssetPurchaseOrderBLL purchaseOrderBLL = new AssetPurchaseOrderBLL();
        AssetPurchaseOrder purchaseOrder = new AssetPurchaseOrder();

        purchaseOrder.POName = TB_POName.Text.Trim();
        purchaseOrder.PurManCode = strUserCode.Trim();
        purchaseOrder.PurManName = ShareClass.GetUserName(strUserCode.Trim());
        purchaseOrder.OperatorCode = strUserCode.Trim();
        purchaseOrder.OperatorName = purchaseOrder.PurManName.Trim();
        purchaseOrder.PurTime = DateTime.Now;
        purchaseOrder.ArrivalTime = DateTime.Now;
        purchaseOrder.Amount = 0;
        purchaseOrder.Comment = LanguageHandle.GetWord("DiSanFangSheBei").ToString().Trim() + TB_Comment.Text.Trim();
        purchaseOrder.Status = "New";
        purchaseOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        purchaseOrder.RelatedID = 0;
        purchaseOrder.RelatedType = "Other";

        try
        {
            purchaseOrderBLL.AddAssetPurchaseOrder(purchaseOrder);

            LB_POID.Text = GetMyCreatedMaxAssetPurchaseOrderID(purchaseOrder.OperatorCode.Trim());

            BT_UpdatePO.Visible = true;
            BT_UpdatePO.Enabled = true;
            BT_DeletePO.Visible = true;
            BT_DeletePO.Enabled = true;
            BT_New.Visible = true;
            BT_New.Enabled = true;

            NB_Amount.Amount = 0;

            LoadPurchaseOrder(strUserCode.Trim());

            LoadPurchaseOrderDetail(LB_POID.Text.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCCKNMCZD100GHZHBZZSZD200GHZGDJC").ToString().Trim() + "')", true);
        }
    }

    protected string GetMyCreatedMaxAssetPurchaseOrderID(string strusercode)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetpurchaseOrder where assetpurchaseOrder.OperatorCode = " + "'" + strusercode + "'" + " Order by assetpurchaseOrder.POID DESC";
        AssetPurchaseOrderBLL purchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = purchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AssetPurchaseOrder purchaseOrder = (AssetPurchaseOrder)lst[0];
            return purchaseOrder.POID.ToString();
        }
        else
            return "0";
    }

    protected void BT_UpdatePO_Click(object sender, EventArgs e)
    {
        if (TB_POName.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSMCBNWKJC").ToString().Trim() + "')", true);
            TB_POName.Focus();
            return;
        }
        string strPOID = LB_POID.Text.Trim();
        string strHQL = "from AssetPurchaseOrder as assetpurchaseOrder where assetpurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL purchaseOrderBLL = new AssetPurchaseOrderBLL();
        IList lst = purchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AssetPurchaseOrder purchaseOrder = (AssetPurchaseOrder)lst[0];

            purchaseOrder.POName = TB_POName.Text.Trim();
            purchaseOrder.PurManName = ShareClass.GetUserName(strUserCode.Trim());
            purchaseOrder.Amount = NB_Amount.Amount;
            purchaseOrder.Comment = LanguageHandle.GetWord("DiSanFangSheBei").ToString().Trim() + TB_Comment.Text.Trim();
            purchaseOrder.Status = DL_POStatus.SelectedValue.Trim();

            try
            {
                purchaseOrderBLL.UpdateAssetPurchaseOrder(purchaseOrder, purchaseOrder.POID);

                LoadPurchaseOrder(strUserCode.Trim());

                LoadPurchaseOrderDetail(LB_POID.Text.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_DeletePO_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPOID;

        strPOID = LB_POID.Text.Trim();

        strHQL = "Delete From T_AssetPurchaseOrder Where POID = " + strPOID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_AssetPurRecord Where POID = " + strPOID;
            ShareClass.RunSqlCommand(strHQL);

            BT_UpdatePO.Visible = false;
            BT_DeletePO.Visible = false;

            BT_New.Visible = false;
            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            LoadPurchaseOrder(strUserCode.Trim());
            LoadPurchaseOrderDetail(strPOID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        AssetPurchaseOrderBLL puchaseOrderBLL = new AssetPurchaseOrderBLL();
        IList lst = puchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        if (DL_Type.SelectedValue.Trim() == "" | TB_AssetName.Text.Trim() == "" | TB_Spec.Text.Trim() == "" | TB_AssetCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLXMCGGDMZRHYXDBNWKJC").ToString().Trim() + "')", true);

            return;
        }
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        AssetPurRecord assetPurRecord = new AssetPurRecord();

        assetPurRecord.POID = int.Parse(LB_POID.Text.Trim());
        assetPurRecord.Type = DL_Type.SelectedValue.Trim();
        assetPurRecord.AssetCode = TB_AssetCode.Text.Trim();
        assetPurRecord.AssetName = TB_AssetName.Text.Trim();
        assetPurRecord.Number = NB_Number.Amount;
        assetPurRecord.Unit = DL_Unit.SelectedValue.Trim();
        assetPurRecord.Price = NB_Price.Amount;
        assetPurRecord.ModelNumber = TB_ModelNumber.Text.Trim();
        assetPurRecord.Spec = TB_Spec.Text.Trim();
        assetPurRecord.PurReason = TB_PurReason.Text.Trim();
        assetPurRecord.PurTime = DateTime.Now;
        assetPurRecord.Status = DL_Status.SelectedValue.Trim();
        assetPurRecord.RelatedType = "Other";
        assetPurRecord.RelatedID = 0;
        assetPurRecord.ApplicantCode = strUserCode.Trim();
        assetPurRecord.ApplicantName = ShareClass.GetUserName(strUserCode.Trim());
        assetPurRecord.Supplier = TB_Supplier.Text.Trim();
        assetPurRecord.SupplierPhone = TB_SupplierPhone.Text.Trim();
        assetPurRecord.Amount = assetPurRecord.Price * assetPurRecord.Number;
        assetPurRecord.CurrencyType = DL_CurrencyType.SelectedValue.Trim();


        try
        {
            assetPurRecordBLL.AddAssetPurRecord(assetPurRecord);

            LB_ID.Text = ShareClass.GetMyCreatedMaxAssetPurRecordID(assetPurRecord.POID.ToString());

            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;

            LoadPurchaseOrderDetail(assetPurRecord.POID.ToString());

            NB_Amount.Amount = SumPurchaseOrderAmount(assetPurRecord.POID.ToString());
            UpdatePurchaseOrderAmount(assetPurRecord.POID.ToString(), NB_Amount.Amount);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strID;
        string strHQL;
        IList lst;
        strID = LB_ID.Text.Trim();
        if (DL_Type.SelectedValue.Trim() == "" | TB_AssetName.Text.Trim() == "" | TB_Spec.Text.Trim() == "" | TB_AssetCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLXMCGGDMZRHYXDBNWKJC").ToString().Trim() + "')", true);

            return;
        }
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

            assetPurRecord.Type = DL_Type.SelectedValue.Trim();
            assetPurRecord.AssetCode = TB_AssetCode.Text.Trim();
            assetPurRecord.AssetName = TB_AssetName.Text.Trim();
            assetPurRecord.Number = NB_Number.Amount;
            assetPurRecord.Unit = DL_Unit.SelectedValue.Trim();
            assetPurRecord.Price = NB_Price.Amount;
            assetPurRecord.ModelNumber = TB_ModelNumber.Text.Trim();
            assetPurRecord.Spec = TB_Spec.Text.Trim();
            assetPurRecord.PurReason = TB_PurReason.Text.Trim();
            assetPurRecord.Status = DL_Status.SelectedValue.Trim();
            assetPurRecord.ApplicantName = ShareClass.GetUserName(strUserCode.Trim());
            assetPurRecord.Supplier = TB_Supplier.Text.Trim();
            assetPurRecord.SupplierPhone = TB_SupplierPhone.Text.Trim();
            assetPurRecord.Amount = assetPurRecord.Number * assetPurRecord.Price;
            assetPurRecord.CurrencyType = DL_CurrencyType.SelectedValue.Trim();


            try
            {
                assetPurRecordBLL.UpdateAssetPurRecord(assetPurRecord, int.Parse(strID));

                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Visible = true;
                BT_Delete.Enabled = true;

                LoadPurchaseOrderDetail(assetPurRecord.POID.ToString());

                NB_Amount.Amount = SumPurchaseOrderAmount(assetPurRecord.POID.ToString());
                UpdatePurchaseOrderAmount(assetPurRecord.POID.ToString(), NB_Amount.Amount);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strPOID = LB_POID.Text.Trim();

        string strHQL;
        IList lst;

        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
        AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

        try
        {
            assetPurRecordBLL.DeleteAssetPurRecord(assetPurRecord);

            LoadPurchaseOrderDetail(strPOID);

            NB_Amount.Amount = SumPurchaseOrderAmount(strPOID);
            UpdatePurchaseOrderAmount(strPOID, NB_Amount.Amount);

            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_POStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPOID, strStatus, strUserCode;

        strPOID = LB_POID.Text.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strPOID != "")
        {
            UpdateAssetPurchaseStatus(strPOID, strStatus);
            LoadPurchaseOrder(strUserCode);
        }
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strStatus;

        strID = LB_ID.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        if (strID != "")
        {
            UpdateAssetPurchaseOrderDetailStatus(strID, strStatus);
        }
    }

    protected void UpdateAssetPurchaseStatus(string strPOID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetpurchaseOrder where assetpurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL purchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = purchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AssetPurchaseOrder purchaseOrder = (AssetPurchaseOrder)lst[0];

            purchaseOrder.Status = strStatus;

            try
            {
                purchaseOrderBLL.UpdateAssetPurchaseOrder(purchaseOrder, int.Parse(strPOID));
            }
            catch
            {
            }
        }
    }

    protected void UpdateAssetPurchaseOrderDetailStatus(string strID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

            assetPurRecord.Status = strStatus;

            try
            {
                assetPurRecordBLL.UpdateAssetPurRecord(assetPurRecord, int.Parse(strID));
            }
            catch
            {
            }
        }
    }

    protected void LoadPurchaseOrder(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetpurchaseOrder where assetpurchaseOrder.OperatorCode = " + "'" + strOperatorCode + "'" + " Order by assetpurchaseOrder.POID DESC";
        AssetPurchaseOrderBLL purchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = purchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DataGrid5.CurrentPageIndex = 0;
        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadPurchaseOrderDetail(string strPOID)
    {
        string strHQL = "Select * from T_AssetPurRecord where POID = " + strPOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected decimal SumPurchaseOrderAmount(string strPOID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.POID = " + strPOID;
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);

        AssetPurRecord assetPurRecord = new AssetPurRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            assetPurRecord = (AssetPurRecord)lst[i];
            deAmount += assetPurRecord.Number * assetPurRecord.Price;
        }

        return deAmount;
    }

    protected void UpdatePurchaseOrderAmount(string strPOID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetpurchaseOrder where assetpurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL purchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = purchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            AssetPurchaseOrder purchaseOrder = (AssetPurchaseOrder)lst[0];

            purchaseOrder.Amount = deAmount;
            purchaseOrderBLL.UpdateAssetPurchaseOrder(purchaseOrder, int.Parse(strPOID));
        }
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        if (FileUpload_Training.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZTSNZEXCELWJ").ToString().Trim() ;
            return;
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZTSZKYZEXCELWJ").ToString().Trim() ;
            return;
        }
        string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() ;
        }
        else
        {
            FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string strpath = strDocSavePath + newfilename;

            //DataSet ds = ExcelToDataSet(strpath, filename);
            //DataRow[] dr = ds.Tables[0].Select();
            //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
            //int rowsnum = ds.Tables[0].Rows.Count;

            DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
            DataRow[] dr = dt.Select();                        //定义一个DataRow数组
            int rowsnum = dt.Rows.Count;
            if (rowsnum == 0)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZTSEXCELBWKBWSJ").ToString().Trim() ;
            }
            else
            {
                string strPOID = "0";
                #region [添加主表数据]
                string strCurrencyType = dr[0]["币种"].ToString().Trim(); 
                AddAssetPurchaseOrderData(ref strPOID, strCurrencyType);
                #endregion

                AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
                AssetPurRecord assetPurRecord = new AssetPurRecord();

                for (int i = 0; i < dr.Length; i++)
                {
                    assetPurRecord.POID = int.Parse(strPOID.Trim());
                    assetPurRecord.Type = dr[i]["类型"].ToString().Trim(); 
                    assetPurRecord.AssetCode = dr[i]["代码"].ToString().Trim(); 
                    assetPurRecord.AssetName = dr[i]["名称"].ToString().Trim(); 
                    assetPurRecord.Number = decimal.Parse(dr[i]["数量"].ToString().Trim() == "" ? "0" : dr[i]["数量"].ToString().Trim()); 
                    assetPurRecord.Unit = dr[i]["单位"].ToString().Trim(); 
                    assetPurRecord.Price = decimal.Parse(dr[i]["单价"].ToString().Trim() == "" ? "0" : dr[i]["单价"].ToString().Trim()); 
                    assetPurRecord.ModelNumber = dr[i]["型号"].ToString().Trim(); 
                    assetPurRecord.Spec = dr[i]["规格"].ToString().Trim(); 
                    assetPurRecord.PurReason = dr[i]["登记理由"].ToString().Trim(); 
                    assetPurRecord.PurTime = DateTime.Now;
                    assetPurRecord.Status = dr[i]["状态"].ToString().Trim(); 
                    assetPurRecord.RelatedType = "Other";
                    assetPurRecord.RelatedID = 0;
                    assetPurRecord.ApplicantCode = strUserCode.Trim();
                    assetPurRecord.ApplicantName = ShareClass.GetUserName(strUserCode.Trim());
                    assetPurRecord.Supplier = dr[i]["Supplier"].ToString().Trim();
                    assetPurRecord.SupplierPhone = dr[i]["供应商电话"].ToString().Trim(); 
                    assetPurRecord.Amount = assetPurRecord.Price * assetPurRecord.Number;
                    assetPurRecord.CurrencyType = dr[i]["币种"].ToString().Trim(); 


                    assetPurRecordBLL.AddAssetPurRecord(assetPurRecord);

                    continue;
                }

                decimal strAmount = SumPurchaseOrderAmount(strPOID.Trim());
                UpdatePurchaseOrderAmount(strPOID.Trim(), strAmount);

                LoadPurchaseOrder(strUserCode.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRGXCG").ToString().Trim() + "')", true);
            }
        }
    }

    protected string AddAssetPurchaseOrderData(ref string strPOID, string strCurrencyType)
    {
        AssetPurchaseOrderBLL purchaseOrderBLL = new AssetPurchaseOrderBLL();
        AssetPurchaseOrder purchaseOrder = new AssetPurchaseOrder();

        purchaseOrder.POName = LanguageHandle.GetWord("DaoRuShuJu").ToString().Trim();
        purchaseOrder.PurManCode = strUserCode.Trim();
        purchaseOrder.PurManName = ShareClass.GetUserName(strUserCode.Trim());
        purchaseOrder.OperatorCode = strUserCode.Trim();
        purchaseOrder.OperatorName = purchaseOrder.PurManName.Trim();
        purchaseOrder.PurTime = DateTime.Now;
        purchaseOrder.ArrivalTime = DateTime.Now;
        purchaseOrder.Amount = 0;
        purchaseOrder.Comment = LanguageHandle.GetWord("DiSanFangSheBeiDaoRuShuJu").ToString().Trim();
        purchaseOrder.Status = "New";
        purchaseOrder.CurrencyType = strCurrencyType;
        purchaseOrder.RelatedID = 0;
        purchaseOrder.RelatedType = "Other";

        purchaseOrderBLL.AddAssetPurchaseOrder(purchaseOrder);

        return strPOID = GetMyCreatedMaxAssetPurchaseOrderID(purchaseOrder.OperatorCode.Trim());
    }
}
