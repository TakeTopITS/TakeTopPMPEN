using System;
using System.Resources;
using System.IO;
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

using TakeTopCore;
using PushSharp.Core;
using TakeTopGantt.models;
using DayPilot.Web.Ui;
using AjaxControlToolkit;


public partial class TTConstractDetail : System.Web.UI.Page
{
    string strAuthority;
    string strLangCode, strUserCode, strUserName;
    string strConstractCode, strConstractName, strConstractID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;


        string strCurrencyType;

        strLangCode = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        try
        {
            strConstractCode = Request.QueryString["ConstractCode"].Trim();
            if (strConstractCode == "")
            {
                Response.Redirect("TTDisplayErrors.aspx");
            }
        }
        catch
        {
            Response.Redirect("TTDisplayErrors.aspx");
        }


        strAuthority = GetConstractRelatedUserAuthority(strConstractCode, strUserCode);
        if (strAuthority == "NO")
        {
            Response.Redirect("TTConstractView.aspx?ConstractCode=" + strConstractCode);
        }

        if (strAuthority == "ALL")
        {
            DL_IsSecrecyPayable.Visible = true;
            DL_IsSecrecyReceiver.Visible = true;
        }
        else
        {
            DL_IsSecrecyPayable.Visible = false;
            DL_IsSecrecyReceiver.Visible = false;
        }

        strHQL = "from Constract as constract where constract.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractBLL constractBLL = new ConstractBLL();
        Constract constract = new Constract();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        constract = (Constract)lst[0];
        strConstractName = constract.ConstractName.Trim();
        strConstractID = constract.ConstractID.ToString();

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll2", "SaveScroll(GoodsListDivID);");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_GiveTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DLC_PayableTime.Text = GetWZCompactEffectDate(strConstractCode).ToString("yyyy-MM-dd");
            DLC_ReceivablesTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DLC_SaleTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_DeleveryTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DLC_DeliveryGoodsTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ReceiptGoodsTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_OpenDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DLC_EntryImportDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EntryReportDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            HL_BusinessForm.NavigateUrl = "TTRelatedDIYBusinessForm.aspx?RelatedType=Constract&RelatedID=" + strConstractID + "&IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(ShareClass.getBusinessFormTemName("Constract", strConstractID));
            //BusinessForm，如果不含业务表单，就隐藏“相关表单按钮”
            if (ShareClass.getRelatedBusinessFormTemName("Constract", strConstractID) == "")
            {
                HL_BusinessForm.Visible = false;
            }

            LoadConstractReceivables(strConstractCode, strAuthority);
            LoadConstractPayable(strConstractCode, strAuthority);

            LoadConstractReceivablesPayer(strUserCode);
            LoadConstractPayableReceiver(strUserCode);

            LoadConstractRelatedInvoice(strConstractCode);

            LoadRelatedProject(strConstractCode);
            LoadRelatedGoodsSaleOrder(strConstractCode);
            LoadRelatedGoodsPurchaseOrder(strConstractCode);
            LoadRelatedAssetPurchaseOrder(strConstractCode);

            LoadConstractSales(strConstractCode);

            LoadConstractRelatedGoodsList(strConstractCode);

            LoadConstractGoodsReceiptPlanList(strConstractCode);
            LoadConstractGoodsDeliveryPlanList(strConstractCode);

            CountReceivablesAmount(strConstractCode, strAuthority);
            CountPayableAmount(strConstractCode, strAuthority);
            CountInvoiceAmount(strConstractCode);

            LoadConstractRelatedEntry(strConstractCode);
            LoadConstractRelatedEntryForInner(strConstractCode);

            LoadConstractRadio();

            //LoadRelatedDocByDocType(strConstractCode, "合同内容", DataGrid18);

            LoadRelatedDocByDocType(strConstractCode, LanguageHandle.GetWord("BuChongXieYi"), DataGrid19);
            LoadContractBasisDocument(strConstractCode);

            //LoadRelatedDocByDocType(strConstractCode, "合同依据", DataGrid20);
            LoadRelatedDocByDocType(strConstractCode, LanguageHandle.GetWord("XiangMuJiTaXinXi"), DataGrid21);

            ShareClass.LoadCurrencyForDropDownList(DL_EncryCurrency);
            ShareClass.LoadCurrencyForDropDownList(DL_EntryCurrencyForInner);
            ShareClass.LoadWareHouseListByAuthorityForDropDownList(strUserCode, DL_WareHouse);

            LB_ConstractCode.Text = strConstractCode;

            ShareClass.LoadAccountForDDL(DL_ReceiveAccount);
            ShareClass.LoadAccountForDDL(DL_PayAccount);

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();
            DL_DeliveryGoodsUnitName.DataSource = lst;
            DL_DeliveryGoodsUnitName.DataBind();
            DL_ReceiptGoodsUnitName.DataSource = lst;
            DL_ReceiptGoodsUnitName.DataBind();
            DL_DeliveryGoodsUnitName.DataSource = lst;
            DL_DeliveryGoodsUnitName.DataBind();

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_GoodsType.DataSource = lst;
            DL_GoodsType.DataBind();
            DL_GoodsType.Items.Insert(0, new ListItem("--Select--", ""));

            DL_DeliveryGoodsType.DataSource = lst;
            DL_DeliveryGoodsType.DataBind();
            DL_DeliveryGoodsType.Items.Insert(0, new ListItem("--Select--", ""));

            DL_ReceiptGoodsType.DataSource = lst;
            DL_ReceiptGoodsType.DataBind();
            DL_ReceiptGoodsType.Items.Insert(0, new ListItem("--Select--", ""));

            DL_DeliveryGoodsType.DataSource = lst;
            DL_DeliveryGoodsType.DataBind();
            DL_DeliveryGoodsType.Items.Insert(0, new ListItem("--Select--", ""));

            LB_ReceivablesOperatorCode.Text = strUserCode;
            LB_ReceivablesOperatorName.Text = strUserName;
            LB_PayableOperatorCode.Text = strUserCode;
            LB_PayableOperatorName.Text = strUserName;

            strCurrencyType = GetConstractCurrencyType(strConstractCode);
            LB_PayableCurrency.Text = strCurrencyType;
            LB_ReceivablesCurrency.Text = strCurrencyType;

            DataSet ds;
            strHQL = "Select HomeModuleName, PageName || " + "'" + strConstractCode + "&RelatedID=" + constract.ConstractID.ToString() + "' as ModulePage  From T_ProModuleLevelForPage Where ParentModule = 'ContractExecution' and LangCode = '" + strLangCode + "' and Visible ='YES' Order By SortNumber ASC";   
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
            Repeater1.DataSource = ds;
            Repeater1.DataBind();

            try
            {
                string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
                if (strProductType != "ERP" & strProductType != "CRM" & strProductType != "SIMP" & strProductType != "EDPMP" & strProductType != "ECMP" & strProductType != "DEMO")
                {
                    TabPanel4.Visible = false;
                    TabPanel8.Visible = false;
                    TabPanel13.Visible = false;
                    TabPanel15.Visible = false;
                }

                if (strProductType != "CRM")
                {
                    TabPanel13.Visible = false;
                    TabPanel15.Visible = false;

                    TabPanel3.Visible = false;
                }
            }
            catch
            {
            }
        }
    }

    protected DateTime GetWZCompactEffectDate(string strConstractCode)
    {
        string strHQL;

        strHQL = "From WZCompact as wzCompact Where wzCompact.CompactCode = " + "'" + strConstractCode + "'";
        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        IList lst = wZCompactBLL.GetAllWZCompacts(strHQL);

        try
        {
            if (lst.Count > 0)
            {
                return DateTime.Parse(((WZCompact)lst[0]).EffectTime);
            }
            else
            {
                return DateTime.Now;
            }
        }
        catch
        {
            return DateTime.Now;
        }
    }

    protected void DL_RelatedType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strRelatedType;

        strRelatedType = DL_ReceivablesRelatedType.SelectedValue.Trim();

        if (strRelatedType == "Other")
        {
            NB_ReceivablesRelatedID.Amount = 0;
        }

        if (strRelatedType == "Project")
        {
            DL_ReceiveRelatedProjectID.Visible = true;
            DL_ReceiveRelatedGoodsSOID.Visible = false;
        }

        if (strRelatedType == "GoodsSO")
        {
            DL_ReceiveRelatedGoodsSOID.Visible = true;
            DL_ReceiveRelatedProjectID.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
    }

    protected void BT_ConatractReciveRefresh_Click(object sender, EventArgs e)
    {
        string strConstractCode = Request.QueryString["ConstractCode"];

        LoadConstractReceivables(strConstractCode, strAuthority);
    }

    protected void BT_ConstractPayRefresh_Click(object sender, EventArgs e)
    {
        string strConstractCode = Request.QueryString["ConstractCode"];

        LoadConstractPayable(strConstractCode, strAuthority);
    }

    protected void DL_ReceiveRelatedProjectID_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_ReceivablesRelatedID.Amount = int.Parse(DL_ReceiveRelatedProjectID.SelectedValue);


        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
    }

    protected void DL_ReceiveRelatedGoodsSOID_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_ReceivablesRelatedID.Amount = int.Parse(DL_ReceiveRelatedGoodsSOID.SelectedValue);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
    }

    protected void DL_ReceivaleTaxRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DL_ReceivaleTaxRate.SelectedValue == "0")
        {
            NB_ReceivaleTaxRate.Amount = 0;
        }
        if (DL_ReceivaleTaxRate.SelectedValue == "0.13")
        {
            NB_ReceivaleTaxRate.Amount = decimal.Parse("0.13");
        }
        if (DL_ReceivaleTaxRate.SelectedValue == "0.09")
        {
            NB_ReceivaleTaxRate.Amount = decimal.Parse("0.09");
        }
        if (DL_ReceivaleTaxRate.SelectedValue == "0.06")
        {
            NB_ReceivaleTaxRate.Amount = decimal.Parse("0.06");
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
    }


    protected void BT_CreateReceivables_Click(object sender, EventArgs e)
    {
        LB_ReceivablesID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','false') ", true);
    }

    protected void BT_Receivables_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ReceivablesID.Text.Trim();

        if (strID == "")
        {
            AddReceivables();
        }
        else
        {
            UpdateReceivables();
        }
    }

    protected void AddReceivables()
    {
        string strReceivableID;
        string strOperatorCode, strOperatorName;
        string strConstractCode, strBillCode, strRecorderCode, strRecorderName, strReceiveAccount;
        DateTime dtReceivablesTime, dtReceiverTime;
        decimal deReceivablesAccount, deReceiverAccount, DeInvoiceAccount;
        decimal dePreDays;
        string strStatus, strPayer, strReceiveComment;

        strConstractCode = LB_ConstractCode.Text.Trim();
        strBillCode = TB_ReceivablesBillCode.Text.Trim();
        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();
        dtReceiverTime = DateTime.Now;

        strReceiveAccount = TB_ReceiveAccount.Text.Trim();

        dtReceivablesTime = DateTime.Parse(DLC_ReceivablesTime.Text);

        deReceivablesAccount = NB_ReceivablesAccount.Amount;
        deReceiverAccount = 0;
        DeInvoiceAccount = 0;

        strOperatorCode = LB_ReceivablesOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBDJRYDMCWCWCRJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);

            return;
        }

        dePreDays = NB_ReceivablesPreDays.Amount;
        strStatus = DL_ReceivablesStatus.SelectedValue;
        strPayer = TB_Payer.Text.Trim();
        strReceiveComment = TB_ReceiveComment.Text.Trim();

        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        ConstractReceivables constractReceivables = new ConstractReceivables();

        constractReceivables.ConstractCode = strConstractCode;
        constractReceivables.BillCode = strBillCode;

        constractReceivables.ReceivablesAccount = deReceivablesAccount;
        constractReceivables.ReceivablesTime = dtReceivablesTime;
        constractReceivables.Account = strReceiveAccount;
        constractReceivables.ReceiverAccount = deReceiverAccount;
        constractReceivables.ReceiverTime = dtReceiverTime;
        constractReceivables.InvoiceAccount = DeInvoiceAccount;
        constractReceivables.UNReceiveAmount = deReceivablesAccount - deReceiverAccount;

        constractReceivables.CurrencyType = LB_ReceivablesCurrency.Text;

        constractReceivables.Payer = strPayer;
        constractReceivables.OperatorCode = strOperatorCode;
        constractReceivables.OperatorName = strOperatorName;
        constractReceivables.OperateTime = DateTime.Now;
        constractReceivables.PreDays = int.Parse(dePreDays.ToString());
        constractReceivables.Status = strStatus;
        constractReceivables.Comment = strReceiveComment;

        constractReceivables.RelatedType = DL_ReceivablesRelatedType.SelectedValue.Trim();
        constractReceivables.RelatedID = int.Parse(NB_ReceivablesRelatedID.Amount.ToString());

        if (DL_ReceivablesRelatedType.SelectedValue.Trim() == "Project")
        {
            constractReceivables.RelatedProjectID = int.Parse(NB_ReceivablesRelatedID.Amount.ToString());
            constractReceivables.RelatedPlanID = int.Parse(NB_ReceivablesRelatedPlanID.Amount.ToString());
        }
        else
        {
            constractReceivables.RelatedProjectID = 1;
            constractReceivables.RelatedPlanID = 0;
        }

        constractReceivables.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        constractReceivables.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractReceivables.CurrencyType.Trim());
        constractReceivables.IsSecrecy = DL_IsSecrecyReceiver.SelectedValue.Trim();

        try
        {
            constractReceivablesBLL.AddConstractReceivables(constractReceivables);
            strReceivableID = ShareClass.GetMyCreatedMaxConstractReceivableID(strConstractCode);
            LB_ReceivablesID.Text = strReceivableID;

            //保存其它附属属性
            SaveConstractReceivablesOtherField(strReceivableID);

            LoadConstractReceivables(strConstractCode, strAuthority);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
        }
    }

    //保存其它附属属性
    protected void SaveConstractReceivablesOtherField(string strReceivableID)
    {
        string strHQL;

        strHQL = string.Format(@"Update t_constractReceivables Set TaxRate = {0},IncomeRatio= '{1}' Where ID = {2}", NB_ReceivaleTaxRate.Amount, DL_ReceivablesIncomeRatio.SelectedValue.Trim(), strReceivableID);
        ShareClass.RunSqlCommand(strHQL);
    }


    protected void UpdateReceivables()
    {
        string strHQL;
        IList lst;

        string strID, strOperatorCode, strOperatorName;
        string strConstractCode, strBillCode, strRecorderCode, strRecorderName, strReceiveAccount;
        DateTime dtReceivablesTime;
        decimal deReceivablesAccount;
        decimal dePreDays;
        string strStatus, strPayer, strReceiveComment;


        strID = LB_ReceivablesID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strBillCode = TB_ReceivablesBillCode.Text.Trim();
        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();

        strReceiveAccount = TB_ReceiveAccount.Text.Trim();

        dtReceivablesTime = DateTime.Parse(DLC_ReceivablesTime.Text);

        deReceivablesAccount = NB_ReceivablesAccount.Amount;


        strOperatorCode = LB_ReceivablesOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBDJRYDMCWCWCRJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);

            return;
        }

        dePreDays = NB_ReceivablesPreDays.Amount;
        strStatus = DL_ReceivablesStatus.SelectedValue;
        strPayer = TB_Payer.Text.Trim();
        strReceiveComment = TB_ReceiveComment.Text.Trim();


        strHQL = "from  ConstractReceivables as constractReceivables where constractReceivables.ID= " + strID;
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);
        ConstractReceivables constractReceivables = (ConstractReceivables)lst[0];

        constractReceivables.ConstractCode = strConstractCode;
        constractReceivables.BillCode = strBillCode;

        constractReceivables.Account = strReceiveAccount;

        constractReceivables.ReceivablesAccount = deReceivablesAccount;
        constractReceivables.ReceivablesTime = dtReceivablesTime;

        constractReceivables.UNReceiveAmount = deReceivablesAccount - constractReceivables.ReceiverAccount;

        constractReceivables.CurrencyType = LB_ReceivablesCurrency.Text.Trim();

        constractReceivables.Payer = strPayer;
        constractReceivables.OperatorCode = strOperatorCode;
        constractReceivables.OperatorName = strOperatorName;
        constractReceivables.PreDays = int.Parse(dePreDays.ToString());
        constractReceivables.Status = strStatus;
        constractReceivables.Comment = strReceiveComment;

        constractReceivables.RelatedType = DL_ReceivablesRelatedType.SelectedValue.Trim();
        constractReceivables.RelatedID = int.Parse(NB_ReceivablesRelatedID.Amount.ToString());

        if (DL_ReceivablesRelatedType.SelectedValue.Trim() == "Project")
        {
            constractReceivables.RelatedProjectID = int.Parse(NB_ReceivablesRelatedID.Amount.ToString());
            constractReceivables.RelatedPlanID = int.Parse(NB_ReceivablesRelatedPlanID.Amount.ToString());
        }
        else
        {
            constractReceivables.RelatedProjectID = 1;
            constractReceivables.RelatedPlanID = 0;
        }

        constractReceivables.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        constractReceivables.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractReceivables.CurrencyType.Trim());

        constractReceivables.IsSecrecy = DL_IsSecrecyReceiver.SelectedValue.Trim();

        try
        {
            constractReceivablesBLL.UpdateConstractReceivables(constractReceivables, int.Parse(strID));

            //保存其它附属属性
            SaveConstractReceivablesOtherField(strID);

            LoadConstractReceivables(strConstractCode, strAuthority);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
        }
    }

    protected void DL_PayablesRelatedType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strRelatedType;

        strRelatedType = DL_PayablesRelatedType.SelectedValue.Trim();

        if (strRelatedType == "Other")
        {
            NB_PayableRelatedID.Amount = 0;
            DL_PayableRelatedProjectID.Visible = false;

            DL_PayableRelatedGoodsPurchaseID.Visible = false;
        }

        if (strRelatedType == "Project")
        {
            DL_PayableRelatedProjectID.Visible = true;

            DL_PayableRelatedGoodsPurchaseID.Visible = false;
            DL_PayableRelatedAssetPurchaseID.Visible = false;
        }

        if (strRelatedType == "GoodsPO")
        {
            DL_PayableRelatedGoodsPurchaseID.Visible = true;

            DL_PayableRelatedProjectID.Visible = false;
            DL_PayableRelatedAssetPurchaseID.Visible = false;
        }

        if (strRelatedType == "AssetPO")
        {
            DL_PayableRelatedAssetPurchaseID.Visible = true;

            DL_PayableRelatedGoodsPurchaseID.Visible = false;
            DL_PayableRelatedProjectID.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
    }

    protected void DL_PayableRelatedGoodsPurchaseID_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_PayableRelatedID.Amount = int.Parse(DL_PayableRelatedGoodsPurchaseID.SelectedValue);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);

    }

    protected void DL_PayableRelatedAssetPurchaseID_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_PayableRelatedID.Amount = int.Parse(DL_PayableRelatedAssetPurchaseID.SelectedValue);
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
    }

    protected void DL_PayableRelatedProjectID_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_PayableRelatedID.Amount = int.Parse(DL_PayableRelatedProjectID.SelectedValue);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
    }

    protected void DL_PayableTaxRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DL_PayableTaxRate.SelectedValue == "0")
        {
            NB_PayableTaxRate.Amount = 0;
        }
        if (DL_PayableTaxRate.SelectedValue == "0.13")
        {
            NB_PayableTaxRate.Amount = decimal.Parse("0.13");
        }
        if (DL_PayableTaxRate.SelectedValue == "0.09")
        {
            NB_PayableTaxRate.Amount = decimal.Parse("0.09");
        }
        if (DL_PayableTaxRate.SelectedValue == "0.06")
        {
            NB_PayableTaxRate.Amount = decimal.Parse("0.06");
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
    }


    protected void BT_CreatePayable_Click(object sender, EventArgs e)
    {
        LB_PayableID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
    }

    protected void BT_Payable_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_PayableID.Text.Trim();

        if (strID == "")
        {
            AddPayable();
        }
        else
        {
            UpdatePayable();
        }
    }

    protected void AddPayable()
    {
        string strPayableID;
        string strOperatorCode, strOperatorName;
        string strConstractCode, strBillCode, strRecorderCode, strRecorderName, strPayAccount;
        DateTime dtPayableTime, dtOutOfPocketTime;
        decimal dePayableAccount, deOutOfPocketAccount, DeInvoiceAccount;
        decimal dePreDays;
        string strStatus, strReceiver, strComment;

        strConstractCode = LB_ConstractCode.Text.Trim();
        strBillCode = TB_PayableBillCode.Text.Trim();
        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();
        dtOutOfPocketTime = DateTime.Now;

        strPayAccount = TB_PayAccount.Text.Trim();

        dtPayableTime = DateTime.Parse(DLC_PayableTime.Text);

        dePayableAccount = NB_PayableAccount.Amount;
        deOutOfPocketAccount = 0;
        DeInvoiceAccount = 0;

        decimal decimalPayOtherAccount = 0;
        decimal decimalOtherAccount = 0;

        strOperatorCode = LB_PayableOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWDJRYDMCWCWCRJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);

            return;
        }

        dePreDays = NB_PayablePreDays.Amount;
        strStatus = DL_PayableStatus.SelectedValue;
        strReceiver = TB_Receiver.Text.Trim();
        strComment = TB_PayableComment.Text.Trim();

        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        ConstractPayable constractPayable = new ConstractPayable();

        constractPayable.ConstractCode = strConstractCode;
        constractPayable.BillCode = strBillCode;

        constractPayable.Account = strPayAccount;
        constractPayable.PayableAccount = dePayableAccount;
        constractPayable.PayableTime = dtPayableTime;
        constractPayable.OutOfPocketAccount = deOutOfPocketAccount;
        constractPayable.OutOfPocketTime = dtOutOfPocketTime;
        constractPayable.InvoiceAccount = DeInvoiceAccount;
        constractPayable.UNPayAmount = dePayableAccount - deOutOfPocketAccount;

        constractPayable.CurrencyType = LB_PayableCurrency.Text;

        constractPayable.OperatorCode = strOperatorCode;
        constractPayable.OperatorName = strOperatorName;
        constractPayable.OperateTime = DateTime.Now;
        constractPayable.PreDays = int.Parse(dePreDays.ToString());
        constractPayable.Status = strStatus;
        constractPayable.Receiver = strReceiver;
        constractPayable.Comment = strComment;
        constractPayable.RelatedType = DL_PayablesRelatedType.SelectedValue;
        constractPayable.RelatedID = int.Parse(NB_PayableRelatedID.Amount.ToString());

        if (DL_PayablesRelatedType.SelectedValue.Trim() == "Project")
        {
            constractPayable.RelatedProjectID = int.Parse(NB_PayableRelatedID.Amount.ToString());
            constractPayable.RelatedPlanID = int.Parse(NB_PayableRelatedPlanID.Amount.ToString());
        }
        else
        {
            constractPayable.RelatedProjectID = 1;
            constractPayable.RelatedPlanID = 0;
        }

        constractPayable.AccountCode = string.IsNullOrEmpty(lbl_AccountCode1.Text) ? "" : lbl_AccountCode1.Text.Trim();
        constractPayable.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractPayable.CurrencyType.Trim());

        constractPayable.IsSecrecy = DL_IsSecrecyPayable.SelectedValue.Trim();

        constractPayable.PayOtherAccount = decimalPayOtherAccount;
        constractPayable.OtherAccount = decimalOtherAccount;

        try
        {
            constractPayable.RelatedPJBudgetID = int.Parse(LB_RelatedPJBudgetID.Text);
        }
        catch
        {
            constractPayable.RelatedPJBudgetID = 0;
        }
        constractPayable.RelatedPJBudgetAccountCode = LB_RelatedPJBudgetAccountCode.Text;
        constractPayable.RelatedPJBudgetAccount = LB_RelatedPJBudgetAccount.Text;


        try
        {
            constractPayableBLL.AddConstractPayable(constractPayable);

            strPayableID = ShareClass.GetMyCreatedMaxConstractPayableID(strConstractCode);
            LB_PayableID.Text = strPayableID;

            //保存其它附属属性
            SaveConstractPayableOtherField(strPayableID);


            LoadConstractPayable(strConstractCode, strAuthority);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);
        }
    }

    //保存其它附属属性
    protected void SaveConstractPayableOtherField(string strPayableID)
    {
        string strHQL;

        strHQL = string.Format(@"Update t_constractPayable Set TaxRate = {0},IncomeRatio ='{1}' Where ID = {2}", NB_PayableTaxRate.Amount, DL_PayableIncomeRatio.SelectedValue.Trim(), strPayableID);
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdatePayable()
    {
        string strHQL;
        IList lst;

        string strID, strOperatorCode, strOperatorName;
        string strConstractCode, strBillCode, strRecorderCode, strRecorderName, strPayAccount;
        DateTime dtPayableTime;
        decimal dePayableAccount;
        decimal dePreDays;
        string strStatus, strReceiver, strComment;
        int intProjectID;

        strID = LB_PayableID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strBillCode = TB_PayableBillCode.Text.Trim();
        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();


        strPayAccount = TB_PayAccount.Text.Trim();

        dtPayableTime = DateTime.Parse(DLC_PayableTime.Text);

        dePayableAccount = NB_PayableAccount.Amount;

        decimal decimalPayOtherAccount = 0;
        decimal decimalOtherAccount = 0;

        strOperatorCode = LB_PayableOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWDJRYDMCWCWCRJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);

            return;
        }
        dePreDays = NB_PayablePreDays.Amount;
        strStatus = DL_PayableStatus.SelectedValue;
        strReceiver = TB_Receiver.Text.Trim();
        strComment = TB_PayableComment.Text.Trim();

        if (DL_PayableRelatedProjectID.Items.Count > 0)
        {
            intProjectID = int.Parse(DL_PayableRelatedProjectID.SelectedValue);
        }
        else
        {
            intProjectID = 0;
        }

        strHQL = "from  ConstractPayable as constractPayable where constractPayable.ID= " + strID;
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);
        ConstractPayable constractPayable = (ConstractPayable)lst[0];

        constractPayable.ConstractCode = strConstractCode;
        constractPayable.BillCode = strBillCode;

        constractPayable.Account = strPayAccount;

        constractPayable.PayableAccount = dePayableAccount;
        constractPayable.PayableTime = dtPayableTime;

        constractPayable.UNPayAmount = dePayableAccount - constractPayable.OutOfPocketAccount;

        constractPayable.CurrencyType = LB_PayableCurrency.Text.Trim();
        constractPayable.OperatorCode = strOperatorCode;
        constractPayable.OperatorName = strOperatorName;
        constractPayable.PreDays = int.Parse(dePreDays.ToString());
        constractPayable.Status = strStatus;
        constractPayable.Receiver = strReceiver;
        constractPayable.Comment = strComment;

        constractPayable.RelatedType = DL_PayablesRelatedType.SelectedValue;
        constractPayable.RelatedID = int.Parse(NB_PayableRelatedID.Amount.ToString());

        if (DL_PayablesRelatedType.SelectedValue.Trim() == "Project")
        {
            constractPayable.RelatedProjectID = int.Parse(NB_PayableRelatedID.Amount.ToString());
            constractPayable.RelatedPlanID = int.Parse(NB_PayableRelatedPlanID.Amount.ToString());
        }
        else
        {
            constractPayable.RelatedProjectID = 1;
            constractPayable.RelatedPlanID = 0;
        }

        constractPayable.AccountCode = string.IsNullOrEmpty(lbl_AccountCode1.Text) ? "" : lbl_AccountCode1.Text.Trim();
        constractPayable.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractPayable.CurrencyType.Trim());

        constractPayable.IsSecrecy = DL_IsSecrecyPayable.SelectedValue.Trim();

        constractPayable.PayOtherAccount = decimalPayOtherAccount;
        constractPayable.OtherAccount = decimalOtherAccount;

        try
        {
            constractPayable.RelatedPJBudgetID = int.Parse(LB_RelatedPJBudgetID.Text);
        }
        catch
        {
            constractPayable.RelatedPJBudgetID = 0;
        }
        constractPayable.RelatedPJBudgetAccountCode = LB_RelatedPJBudgetAccountCode.Text;
        constractPayable.RelatedPJBudgetAccount = LB_RelatedPJBudgetAccount.Text;

        try
        {
            constractPayableBLL.UpdateConstractPayable(constractPayable, int.Parse(strID));

            //保存其它附属属性
            SaveConstractPayableOtherField(strID);

            LoadConstractPayable(strConstractCode, strAuthority);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);
        }
    }


    protected void DataGrid11_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = e.Item.Cells[2].Text;
            LB_ID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid11.Items.Count; i++)
            {
                DataGrid11.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "from ConstractRelatedGoods as constractRelatedGoods Where constractRelatedGoods.ID = " + strID;

                ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
                lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);
                ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];

                TB_FirstDirectory.Text = constractRelatedGoods.FirstDirectory;
                TB_SecondDirectory.Text = constractRelatedGoods.SecondDirectory;
                TB_ThirdDirectory.Text = constractRelatedGoods.ThirdDirectory;
                TB_FourthDirectory.Text = constractRelatedGoods.FourthDirectory;

                TB_GoodsCode.Text = constractRelatedGoods.GoodsCode;
                TB_GoodsName.Text = constractRelatedGoods.GoodsName;
                TB_ModelNumber.Text = constractRelatedGoods.ModelNumber;
                TB_Spec.Text = constractRelatedGoods.Spec;
                TB_Brand.Text = constractRelatedGoods.Brand;
                DL_GoodsType.SelectedValue = constractRelatedGoods.Type;
                DL_Unit.SelectedValue = constractRelatedGoods.Unit;
                NB_Number.Amount = constractRelatedGoods.Number;

                NB_Number.Amount = constractRelatedGoods.Number;
                NB_Price.Amount = constractRelatedGoods.Price;

                DL_Unit.SelectedValue = constractRelatedGoods.Unit;

                DLC_SaleTime.Text = constractRelatedGoods.SaleTime.ToString("yyyy-MM-dd");
                DLC_DeleveryTime.Text = constractRelatedGoods.DeleveryTime.ToString("yyyy-MM-dd");

                TB_SaleOrderNumber.Amount = constractRelatedGoods.SaleOrderNumber;
                TB_PurchaseOrderNubmer.Amount = constractRelatedGoods.PurchaseOrderNumber;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
                strHQL = "from ConstractRelatedGoods as constractRelatedGoods where constractRelatedGoods.ID = " + strID;
                lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);
                ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];

                try
                {
                    constractRelatedGoodsBLL.DeleteConstractRelatedGoods(constractRelatedGoods);

                    LoadConstractRelatedGoodsList(strConstractCode);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll(GoodsListDivIDEndRequestHandler);", true);

        }
    }

    protected void DataGrid14_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid14.Items.Count; i++)
            {
                DataGrid14.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_GoodsCode.Text = item.ItemCode;
                TB_GoodsName.Text = item.ItemName;
                try
                {
                    DL_GoodsType.SelectedValue = item.SmallType;
                }
                catch
                {

                }
                TB_ModelNumber.Text = item.ModelNumber;
                DL_Unit.SelectedValue = item.Unit;
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;
                NB_Price.Amount = item.PurchasePrice;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);

        }
    }

    protected void DataGrid13_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            for (int i = 0; i < DataGrid13.Items.Count; i++)
            {
                DataGrid13.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Goods as goods where goods.ID = " + strID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            if (lst.Count > 0)
            {
                Goods goods = (Goods)lst[0];

                TB_GoodsCode.Text = goods.GoodsCode;
                TB_GoodsName.Text = goods.GoodsName;
                TB_ModelNumber.Text = goods.ModelNumber;
                DL_Unit.SelectedValue = goods.UnitName;
                TB_Spec.Text = goods.Spec;
                TB_Brand.Text = goods.Manufacturer;
                DL_GoodsType.SelectedValue = goods.Type;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
        }
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;


        TabContainer2.ActiveTabIndex = 0;

        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strType = "%" + strType + "%";
        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strHQL = "Select * From T_Goods as goods Where goods.GoodsCode Like " + "'" + strGoodsCode + "'" + " and goods.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and goods.Type Like " + "'" + strType + "'" + " and goods.ModelNumber Like " + "'" + strModelNumber + "'" + " and goods.Spec Like " + "'" + strSpec + "'";
        strHQL += " Order by goods.Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid13.DataSource = ds;
        DataGrid13.DataBind();

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid14.DataSource = ds;
        DataGrid14.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
    }


    protected void BT_CreateGoodsList_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
    }


    protected void BT_SaveGoods_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddGoods();
        }
        else
        {
            UpdateGoods();
        }
    }

    protected void AddGoods()
    {
        string strRecordID, strConstractCode, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;
        decimal decNumber;
        DateTime dtSaleTime, dtDeleveryTime;
        decimal dePrice;


        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        decNumber = NB_Number.Amount;
        strSpec = TB_Spec.Text.Trim();
        dePrice = NB_Price.Amount;
        dtSaleTime = DateTime.Parse(DLC_SaleTime.Text);
        dtDeleveryTime = DateTime.Parse(DLC_DeleveryTime.Text);


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);

        }
        else
        {
            ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
            ConstractRelatedGoods constractRelatedGoods = new ConstractRelatedGoods();

            constractRelatedGoods.ConstractCode = strConstractCode;

            constractRelatedGoods.FirstDirectory = TB_FirstDirectory.Text;
            constractRelatedGoods.SecondDirectory = TB_SecondDirectory.Text;
            constractRelatedGoods.ThirdDirectory = TB_ThirdDirectory.Text;
            constractRelatedGoods.FourthDirectory = TB_FourthDirectory.Text;

            constractRelatedGoods.Type = strType;
            constractRelatedGoods.GoodsCode = strGoodsCode;
            constractRelatedGoods.GoodsName = strGoodsName;

            constractRelatedGoods.ModelNumber = strModelNumber;
            constractRelatedGoods.Spec = strSpec;
            constractRelatedGoods.Brand = TB_Brand.Text;

            constractRelatedGoods.Number = decNumber;
            constractRelatedGoods.Unit = strUnitName;
            constractRelatedGoods.Number = decNumber;
            constractRelatedGoods.Price = dePrice;

            constractRelatedGoods.Amount = decNumber * dePrice;

            constractRelatedGoods.SaleTime = dtSaleTime;
            constractRelatedGoods.DeleveryTime = dtDeleveryTime;

            constractRelatedGoods.SaleOrderNumber = TB_SaleOrderNumber.Amount;
            constractRelatedGoods.PurchaseOrderNumber = TB_PurchaseOrderNubmer.Amount;


            try
            {
                constractRelatedGoodsBLL.AddConstractRelatedGoods(constractRelatedGoods);

                strRecordID = ShareClass.GetMyCreatedMaxConstractRelatedGoodsID(strConstractCode);
                LB_ID.Text = strRecordID;


                LoadConstractRelatedGoodsList(strConstractCode);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
            }
        }
    }

    protected void UpdateGoods()
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strUnitName;
        DateTime dtSaleTime, dtDeleveryTime;
        decimal dePrice, deNumber;

        string strID, strConstractCode;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();

        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        dePrice = NB_Price.Amount;
        deNumber = NB_Number.Amount;

        dtSaleTime = DateTime.Parse(DLC_SaleTime.Text);
        dtDeleveryTime = DateTime.Parse(DLC_DeleveryTime.Text);

        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
        }
        else
        {
            ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
            strHQL = "from ConstractRelatedGoods as constractRelatedGoods where constractRelatedGoods.ID = " + strID;
            lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);
            ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];

            constractRelatedGoods.ConstractCode = strConstractCode;

            constractRelatedGoods.FirstDirectory = TB_FirstDirectory.Text;
            constractRelatedGoods.SecondDirectory = TB_SecondDirectory.Text;
            constractRelatedGoods.ThirdDirectory = TB_ThirdDirectory.Text;
            constractRelatedGoods.FourthDirectory = TB_FourthDirectory.Text;

            constractRelatedGoods.Type = strType;
            constractRelatedGoods.GoodsCode = strGoodsCode;
            constractRelatedGoods.GoodsName = strGoodsName;
            constractRelatedGoods.ModelNumber = strModelNumber;
            constractRelatedGoods.Spec = strSpec;
            constractRelatedGoods.Brand = TB_Brand.Text;

            constractRelatedGoods.Number = deNumber;
            constractRelatedGoods.Unit = strUnitName;
            constractRelatedGoods.Price = dePrice;

            constractRelatedGoods.Amount = deNumber * dePrice;

            constractRelatedGoods.SaleTime = dtSaleTime;
            constractRelatedGoods.DeleveryTime = dtDeleveryTime;

            constractRelatedGoods.SaleOrderNumber = TB_SaleOrderNumber.Amount;
            constractRelatedGoods.PurchaseOrderNumber = TB_PurchaseOrderNubmer.Amount;


            try
            {
                constractRelatedGoodsBLL.UpdateConstractRelatedGoods(constractRelatedGoods, int.Parse(strID));

                LoadConstractRelatedGoodsList(strConstractCode);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
            }
        }
    }


    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = e.Item.Cells[2].Text;
            LB_ReceiptPlanID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "from ConstractGoodsReceiptPlan as constractGoodsReceiptPlan Where constractGoodsReceiptPlan.ID = " + strID;

                ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
                lst = constractGoodsReceiptPlanBLL.GetAllConstractGoodsReceiptPlans(strHQL);
                ConstractGoodsReceiptPlan constractGoodsReceiptPlan = (ConstractGoodsReceiptPlan)lst[0];

                TB_ReceiptGoodsCode.Text = constractGoodsReceiptPlan.GoodsCode;
                TB_ReceiptGoodsName.Text = constractGoodsReceiptPlan.GoodsName;
                TB_ReceiptGoodsModelNumber.Text = constractGoodsReceiptPlan.ModelNumber;
                TB_ReceiptGoodsSpec.Text = constractGoodsReceiptPlan.Spec;
                TB_ReceiptGoodsBrand.Text = constractGoodsReceiptPlan.Brand;

                DL_ReceiptGoodsType.SelectedValue = constractGoodsReceiptPlan.Type;
                DL_ReceiptGoodsUnitName.SelectedValue = constractGoodsReceiptPlan.Unit;
                NB_ReceiptGoodsNumber.Amount = constractGoodsReceiptPlan.Number;
                NB_ReceiptGoodsPrice.Amount = constractGoodsReceiptPlan.Price;
                DLC_ReceiptGoodsTime.Text = constractGoodsReceiptPlan.ReceiptTime.ToString("yyyy-MM-dd");
                NB_ReceiptGoodsPreDay.Amount = constractGoodsReceiptPlan.PreDay;
                TB_ReceiptAddress.Text = constractGoodsReceiptPlan.ReceiptAddress;
                DL_ReceiptStatus.SelectedValue = constractGoodsReceiptPlan.Status.Trim();

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
                strHQL = "from ConstractGoodsReceiptPlan as constractGoodsReceiptPlan where constractGoodsReceiptPlan.ID = " + strID;
                lst = constractGoodsReceiptPlanBLL.GetAllConstractGoodsReceiptPlans(strHQL);
                ConstractGoodsReceiptPlan constractGoodsReceiptPlan = (ConstractGoodsReceiptPlan)lst[0];

                try
                {
                    constractGoodsReceiptPlanBLL.DeleteConstractGoodsReceiptPlan(constractGoodsReceiptPlan);

                    LoadConstractGoodsReceiptPlanList(strConstractCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }


    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid6.Items.Count; i++)
            {
                DataGrid6.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_ReceiptGoodsCode.Text = item.ItemCode;
                TB_ReceiptGoodsName.Text = item.ItemName;
                try
                {
                    DL_ReceiptGoodsType.SelectedValue = item.SmallType;
                }
                catch
                {

                }
                TB_ModelNumber.Text = item.ModelNumber;

                DL_ReceiptGoodsUnitName.SelectedValue = item.Unit;
                TB_ReceiptGoodsSpec.Text = item.Specification;
                TB_ReceiptGoodsBrand.Text = item.Brand;

                NB_ReceiptGoodsPrice.Amount = item.PurchasePrice;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
        }
    }

    protected void DataGrid9_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            for (int i = 0; i < DataGrid9.Items.Count; i++)
            {
                DataGrid9.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Goods as goods where goods.ID = " + strID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            if (lst.Count > 0)
            {
                Goods goods = (Goods)lst[0];

                TB_ReceiptGoodsCode.Text = goods.GoodsCode;
                TB_ReceiptGoodsName.Text = goods.GoodsName;
                TB_ReceiptGoodsModelNumber.Text = goods.ModelNumber;
                DL_ReceiptGoodsUnitName.SelectedValue = goods.UnitName;
                TB_ReceiptGoodsSpec.Text = goods.Spec;
                TB_ReceiptGoodsBrand.Text = goods.Manufacturer;

                DL_ReceiptGoodsType.SelectedValue = goods.Type;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
        }
    }


    protected void DataGrid25_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid25.Items.Count; i++)
            {
                DataGrid25.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from ConstractRelatedGoods as constractRelatedGoods Where constractRelatedGoods.ID = " + strID;

            ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
            lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);
            ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];

            TB_ReceiptGoodsCode.Text = constractRelatedGoods.GoodsCode;
            TB_ReceiptGoodsName.Text = constractRelatedGoods.GoodsName;
            TB_ReceiptGoodsModelNumber.Text = constractRelatedGoods.ModelNumber;
            DL_ReceiptGoodsUnitName.SelectedValue = constractRelatedGoods.Unit;
            TB_ReceiptGoodsSpec.Text = constractRelatedGoods.Spec;
            TB_ReceiptGoodsBrand.Text = constractRelatedGoods.Brand;

            DL_ReceiptGoodsType.SelectedValue = constractRelatedGoods.Type;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
        }
    }

    protected void BT_FindReceiptGoods_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;


        TabContainer3.ActiveTabIndex = 0;

        strType = DL_ReceiptGoodsType.SelectedValue.Trim();
        strGoodsCode = TB_ReceiptGoodsCode.Text.Trim();
        strGoodsName = TB_ReceiptGoodsName.Text.Trim();
        strModelNumber = TB_ReceiptGoodsModelNumber.Text.Trim();
        strSpec = TB_ReceiptGoodsSpec.Text.Trim();

        strType = "%" + strType + "%";
        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strHQL = "Select * From T_Goods as goods Where goods.GoodsCode Like " + "'" + strGoodsCode + "'" + " and goods.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and goods.Type Like " + "'" + strType + "'" + " and goods.ModelNumber Like " + "'" + strModelNumber + "'" + " and goods.Spec Like " + "'" + strSpec + "'";
        strHQL += " Order by goods.Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid9.DataSource = ds;
        DataGrid9.DataBind();

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid6.DataSource = ds;
        DataGrid6.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
    }

    protected void BT_ClearReceiptGoods_Click(object sender, EventArgs e)
    {
        TB_ReceiptGoodsCode.Text = "";
        TB_ReceiptGoodsName.Text = "";
        TB_ReceiptGoodsModelNumber.Text = "";
        TB_ReceiptGoodsSpec.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
    }

    protected void DL_WareHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_ReceiptAddress.Text = DL_WareHouse.SelectedValue.Trim();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
    }

    protected void BT_CreateReceivePlan_Click(object sender, EventArgs e)
    {
        LB_ReceiptPlanID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
    }

    protected void BT_ReceiveGoodsPlan_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ReceiptPlanID.Text.Trim();

        if (strID == "")
        {
            AddReceivePlan();
        }
        else
        {
            UpdateReceivePlan();
        }
    }

    protected void AddReceivePlan()
    {
        string strRecordID, strConstractCode, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;
        decimal decNumber;
        DateTime dtReceiptTime;
        decimal dePrice;
        int intPreDay;


        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = DL_ReceiptGoodsType.SelectedValue.Trim();
        strGoodsCode = TB_ReceiptGoodsCode.Text.Trim();
        strGoodsName = TB_ReceiptGoodsName.Text.Trim();
        strUnitName = DL_ReceiptGoodsUnitName.SelectedValue.Trim();
        strModelNumber = TB_ReceiptGoodsModelNumber.Text.Trim();
        decNumber = NB_ReceiptGoodsNumber.Amount;
        strSpec = TB_ReceiptGoodsSpec.Text.Trim();
        dePrice = NB_ReceiptGoodsPrice.Amount;
        dtReceiptTime = DateTime.Parse(DLC_ReceiptGoodsTime.Text);
        intPreDay = int.Parse(NB_ReceiptGoodsPreDay.Amount.ToString());

        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
        }
        else
        {
            ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
            ConstractGoodsReceiptPlan constractGoodsReceiptPlan = new ConstractGoodsReceiptPlan();

            constractGoodsReceiptPlan.ConstractCode = strConstractCode;
            constractGoodsReceiptPlan.Type = strType;
            constractGoodsReceiptPlan.GoodsCode = strGoodsCode;
            constractGoodsReceiptPlan.GoodsName = strGoodsName;

            constractGoodsReceiptPlan.ModelNumber = strModelNumber;
            constractGoodsReceiptPlan.Spec = strSpec;
            constractGoodsReceiptPlan.Brand = TB_ReceiptGoodsBrand.Text;

            constractGoodsReceiptPlan.Number = decNumber;
            constractGoodsReceiptPlan.Unit = strUnitName;
            constractGoodsReceiptPlan.Number = decNumber;
            constractGoodsReceiptPlan.Price = dePrice;
            constractGoodsReceiptPlan.Amount = decNumber * dePrice;

            constractGoodsReceiptPlan.UNReceiptedNumber = decNumber;

            constractGoodsReceiptPlan.ReceiptTime = dtReceiptTime;
            constractGoodsReceiptPlan.PreDay = intPreDay;
            constractGoodsReceiptPlan.ReceiptAddress = TB_ReceiptAddress.Text.Trim();
            constractGoodsReceiptPlan.Status = DL_ReceiptStatus.SelectedValue.Trim();

            try
            {
                constractGoodsReceiptPlanBLL.AddConstractGoodsReceiptPlan(constractGoodsReceiptPlan);

                strRecordID = ShareClass.GetMyCreatedMaxConstractGoodsReceiptPlanID(strConstractCode);
                LB_ID.Text = strRecordID;


                LoadConstractGoodsReceiptPlanList(strConstractCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
            }
        }
    }


    protected void UpdateReceivePlan()
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strUnitName;

        DateTime dtReceiptTime;
        decimal dePrice, deNumber;
        int intPreDay;

        string strID, strConstractCode;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ReceiptPlanID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = DL_ReceiptGoodsType.SelectedValue.Trim();
        strGoodsCode = TB_ReceiptGoodsCode.Text.Trim();
        strGoodsName = TB_ReceiptGoodsName.Text.Trim();
        strUnitName = DL_ReceiptGoodsUnitName.SelectedValue.Trim();
        strModelNumber = TB_ReceiptGoodsModelNumber.Text.Trim();
        deNumber = NB_ReceiptGoodsNumber.Amount;
        strSpec = TB_ReceiptGoodsSpec.Text.Trim();
        dePrice = NB_ReceiptGoodsPrice.Amount;
        dtReceiptTime = DateTime.Parse(DLC_ReceiptGoodsTime.Text);
        intPreDay = int.Parse(NB_ReceiptGoodsPreDay.Amount.ToString());


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
        }
        else
        {
            ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
            strHQL = "from ConstractGoodsReceiptPlan as constractGoodsReceiptPlan where constractGoodsReceiptPlan.ID = " + strID;
            lst = constractGoodsReceiptPlanBLL.GetAllConstractGoodsReceiptPlans(strHQL);
            ConstractGoodsReceiptPlan constractGoodsReceiptPlan = (ConstractGoodsReceiptPlan)lst[0];

            constractGoodsReceiptPlan.ConstractCode = strConstractCode;
            constractGoodsReceiptPlan.Type = strType;
            constractGoodsReceiptPlan.GoodsCode = strGoodsCode;
            constractGoodsReceiptPlan.GoodsName = strGoodsName;
            constractGoodsReceiptPlan.ModelNumber = strModelNumber;
            constractGoodsReceiptPlan.Spec = strSpec;
            constractGoodsReceiptPlan.Brand = TB_ReceiptGoodsBrand.Text;

            constractGoodsReceiptPlan.Number = deNumber;
            constractGoodsReceiptPlan.Unit = strUnitName;
            constractGoodsReceiptPlan.Price = dePrice;

            constractGoodsReceiptPlan.Amount = deNumber * dePrice;

            constractGoodsReceiptPlan.PreDay = intPreDay;
            constractGoodsReceiptPlan.ReceiptTime = dtReceiptTime;
            constractGoodsReceiptPlan.ReceiptAddress = TB_ReceiptAddress.Text.Trim();
            constractGoodsReceiptPlan.Status = DL_ReceiptStatus.SelectedValue.Trim();

            try
            {
                constractGoodsReceiptPlanBLL.UpdateConstractGoodsReceiptPlan(constractGoodsReceiptPlan, int.Parse(strID));

                LoadConstractGoodsReceiptPlanList(strConstractCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivePlanWindow','false') ", true);
            }
        }
    }


    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = e.Item.Cells[2].Text;
            LB_DeliveryPlanID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid7.Items.Count; i++)
            {
                DataGrid7.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "from ConstractGoodsDeliveryPlan as constractGoodsDeliveryPlan Where constractGoodsDeliveryPlan.ID = " + strID;

                ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
                lst = constractGoodsDeliveryPlanBLL.GetAllConstractGoodsDeliveryPlans(strHQL);
                ConstractGoodsDeliveryPlan constractGoodsDeliveryPlan = (ConstractGoodsDeliveryPlan)lst[0];

                TB_DeliveryGoodsCode.Text = constractGoodsDeliveryPlan.GoodsCode;
                TB_DeliveryGoodsName.Text = constractGoodsDeliveryPlan.GoodsName;
                TB_DeliveryGoodsModelNumber.Text = constractGoodsDeliveryPlan.ModelNumber;
                TB_DeliveryGoodsSpec.Text = constractGoodsDeliveryPlan.Spec;
                TB_DeliveryGoodsBrand.Text = constractGoodsDeliveryPlan.Brand;

                DL_DeliveryGoodsType.SelectedValue = constractGoodsDeliveryPlan.Type;
                DL_DeliveryGoodsUnitName.SelectedValue = constractGoodsDeliveryPlan.Unit;
                NB_DeliveryGoodsNumber.Amount = constractGoodsDeliveryPlan.Number;
                NB_DeliveryGoodsPrice.Amount = constractGoodsDeliveryPlan.Price;
                DLC_DeliveryGoodsTime.Text = constractGoodsDeliveryPlan.DeliveryTime.ToString("yyyy-MM-dd");

                NB_DeliveryGoodsPreDay.Amount = constractGoodsDeliveryPlan.PreDay;
                TB_DeliveryAddress.Text = constractGoodsDeliveryPlan.DeliveryAddress;
                DL_DeliveryStatus.SelectedValue = constractGoodsDeliveryPlan.Status.Trim();

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
                strHQL = "from ConstractGoodsDeliveryPlan as constractGoodsDeliveryPlan where constractGoodsDeliveryPlan.ID = " + strID;
                lst = constractGoodsDeliveryPlanBLL.GetAllConstractGoodsDeliveryPlans(strHQL);
                ConstractGoodsDeliveryPlan constractGoodsDeliveryPlan = (ConstractGoodsDeliveryPlan)lst[0];

                try
                {
                    constractGoodsDeliveryPlanBLL.DeleteConstractGoodsDeliveryPlan(constractGoodsDeliveryPlan);

                    LoadConstractGoodsDeliveryPlanList(strConstractCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid8.Items.Count; i++)
            {
                DataGrid8.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_DeliveryGoodsCode.Text = item.ItemCode;
                TB_DeliveryGoodsName.Text = item.ItemName;
                try
                {
                    DL_DeliveryGoodsType.SelectedValue = item.SmallType;
                }
                catch
                {
                }
                TB_ModelNumber.Text = item.ModelNumber;
                DL_DeliveryGoodsUnitName.SelectedValue = item.Unit;
                TB_DeliveryGoodsSpec.Text = item.Specification;
                TB_DeliveryGoodsBrand.Text = item.Brand;

                NB_DeliveryGoodsPrice.Amount = item.PurchasePrice;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Goods as goods where goods.ID = " + strID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            if (lst.Count > 0)
            {
                Goods goods = (Goods)lst[0];

                TB_DeliveryGoodsCode.Text = goods.GoodsCode;
                TB_DeliveryGoodsName.Text = goods.GoodsName;
                TB_DeliveryGoodsModelNumber.Text = goods.ModelNumber;
                DL_DeliveryGoodsUnitName.SelectedValue = goods.UnitName;
                TB_DeliveryGoodsSpec.Text = goods.Spec;
                TB_DeliveryGoodsBrand.Text = goods.Manufacturer;

                DL_DeliveryGoodsType.SelectedValue = goods.Type;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
        }
    }

    protected void DataGrid24_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid24.Items.Count; i++)
            {
                DataGrid24.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from ConstractRelatedGoods as constractRelatedGoods Where constractRelatedGoods.ID = " + strID;

            ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
            lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);
            ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];


            TB_DeliveryGoodsCode.Text = constractRelatedGoods.GoodsCode;
            TB_DeliveryGoodsName.Text = constractRelatedGoods.GoodsName;
            TB_DeliveryGoodsModelNumber.Text = constractRelatedGoods.ModelNumber;
            DL_DeliveryGoodsUnitName.SelectedValue = constractRelatedGoods.Unit;
            TB_DeliveryGoodsSpec.Text = constractRelatedGoods.Spec;
            TB_DeliveryGoodsBrand.Text = constractRelatedGoods.Brand;

            DL_DeliveryGoodsType.SelectedValue = constractRelatedGoods.Type;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
        }
    }

    protected void BT_FindDeliveryGoods_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;


        TabContainer3.ActiveTabIndex = 0;

        strType = DL_DeliveryGoodsType.SelectedValue.Trim();
        strGoodsCode = TB_DeliveryGoodsCode.Text.Trim();
        strGoodsName = TB_DeliveryGoodsName.Text.Trim();
        strModelNumber = TB_DeliveryGoodsModelNumber.Text.Trim();
        strSpec = TB_DeliveryGoodsSpec.Text.Trim();

        strType = "%" + strType + "%";
        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strHQL = "Select * From T_Goods as goods Where goods.GoodsCode Like " + "'" + strGoodsCode + "'" + " and goods.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and goods.Type Like " + "'" + strType + "'" + " and goods.ModelNumber Like " + "'" + strModelNumber + "'" + " and goods.Spec Like " + "'" + strSpec + "'";
        strHQL += " Order by goods.Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid8.DataSource = ds;
        DataGrid8.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
    }

    protected void BT_ClearDeliveryGoods_Click(object sender, EventArgs e)
    {
        TB_DeliveryGoodsCode.Text = "";
        TB_DeliveryGoodsName.Text = "";
        TB_DeliveryGoodsModelNumber.Text = "";
        TB_DeliveryGoodsSpec.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
    }

    protected void BT_CreateDeliveryPlan_Click(object sender, EventArgs e)
    {
        LB_DeliveryPlanID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
    }

    protected void BT_DeliverGoodsPlan_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_DeliveryPlanID.Text.Trim();

        if (strID == "")
        {
            AddDeliveryPlan();
        }
        else
        {
            UpdateDeliveryPlan();
        }
    }

    protected void AddDeliveryPlan()
    {
        string strRecordID, strConstractCode, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;
        decimal decNumber;
        DateTime dtDeliveryTime;
        decimal dePrice;
        int intPreDay;


        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = DL_DeliveryGoodsType.SelectedValue.Trim();
        strGoodsCode = TB_DeliveryGoodsCode.Text.Trim();
        strGoodsName = TB_DeliveryGoodsName.Text.Trim();
        strUnitName = DL_DeliveryGoodsUnitName.SelectedValue.Trim();
        strModelNumber = TB_DeliveryGoodsModelNumber.Text.Trim();
        decNumber = NB_DeliveryGoodsNumber.Amount;
        strSpec = TB_DeliveryGoodsSpec.Text.Trim();
        dePrice = NB_DeliveryGoodsPrice.Amount;
        dtDeliveryTime = DateTime.Parse(DLC_DeliveryGoodsTime.Text);
        intPreDay = int.Parse(NB_DeliveryGoodsPreDay.Amount.ToString());

        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
            ConstractGoodsDeliveryPlan constractGoodsDeliveryPlan = new ConstractGoodsDeliveryPlan();

            constractGoodsDeliveryPlan.ConstractCode = strConstractCode;
            constractGoodsDeliveryPlan.Type = strType;
            constractGoodsDeliveryPlan.GoodsCode = strGoodsCode;
            constractGoodsDeliveryPlan.GoodsName = strGoodsName;

            constractGoodsDeliveryPlan.ModelNumber = strModelNumber;
            constractGoodsDeliveryPlan.Spec = strSpec;
            constractGoodsDeliveryPlan.Brand = TB_DeliveryGoodsBrand.Text;


            constractGoodsDeliveryPlan.Number = decNumber;
            constractGoodsDeliveryPlan.Unit = strUnitName;
            constractGoodsDeliveryPlan.Number = decNumber;
            constractGoodsDeliveryPlan.Price = dePrice;

            constractGoodsDeliveryPlan.Amount = decNumber * dePrice;
            constractGoodsDeliveryPlan.UNDeliveredNumber = decNumber;

            constractGoodsDeliveryPlan.DeliveryTime = dtDeliveryTime;
            constractGoodsDeliveryPlan.PreDay = intPreDay;

            constractGoodsDeliveryPlan.DeliveryAddress = TB_DeliveryAddress.Text;
            constractGoodsDeliveryPlan.Status = DL_DeliveryStatus.SelectedValue.Trim();

            try
            {
                constractGoodsDeliveryPlanBLL.AddConstractGoodsDeliveryPlan(constractGoodsDeliveryPlan);
                strRecordID = ShareClass.GetMyCreatedMaxConstractGoodsDeliveryPlanID(strConstractCode);
                LB_DeliveryPlanID.Text = strRecordID;

                LoadConstractGoodsDeliveryPlanList(strConstractCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
            }
        }
    }


    protected void DataGrid20_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;


            string strID;

            strID = e.Item.Cells[0].Text;

            strHQL = "Delete From T_ContractBasisDocument Where DocID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            //合同依据文档
            LoadContractBasisDocument(strConstractCode);
        }
    }

    protected void BT_UPLoad_ContractBasisDocument_Click(object sender, EventArgs e)
    {
        //上传附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName, strAttachName;

            if (TB_ContractBasisDocumentType.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZDiShiWenDangLeiXingBuNengWei")+"')", true);
                return;
            }

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {

            }
            else
            {
                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                    strAttachName = Path.GetFileNameWithoutExtension(strFileName2);

                    HL_ContractBasisDocumentURL.Text = strFileName3;
                    HL_ContractBasisDocumentURL.NavigateUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //添加合同依据文档
                    AddContractBasisDocument(strConstractCode);
                }
                catch
                {
                }
            }
        }
        else
        {
        }

    }

    protected void BT_RefreshContractInfor_Click(object sender, EventArgs e)
    {
        LoadRelatedDocByDocType(strConstractCode, LanguageHandle.GetWord("BuChongXieYi"), DataGrid19);

        LoadRelatedDocByDocType(strConstractCode, LanguageHandle.GetWord("XiangMuJiTaXinXi"), DataGrid21);
    }



    protected void AddContractBasisDocument(string strConstractCode)
    {
        string strHQL;

        strHQL = string.Format(@"Insert Into t_ContractBasisDocument(ConstractCode,DocType,docname,address,uploadmancode,uploadmanname,uploadtime)
                 values('{0}','{1}','{2}','{3}','{4}','{5}',now())", strConstractCode, TB_ContractBasisDocumentType.Text, HL_ContractBasisDocumentURL.Text,
                 HL_ContractBasisDocumentURL.NavigateUrl, strUserCode, strUserName);

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadContractBasisDocument(strConstractCode);
        }
        catch
        {

        }
    }

    protected void LoadContractBasisDocument(string strConstractCode)
    {
        string strHQL;

        strHQL = "Select * From T_ContractBasisDocument Where ConstractCode = '" + strConstractCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ContractBasisDocument");

        DataGrid20.DataSource = ds;
        DataGrid20.DataBind();
    }


    protected void UpdateDeliveryPlan()
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strUnitName;

        DateTime dtDeliveryTime;
        decimal dePrice, deNumber;
        int intPreDay;

        string strID, strConstractCode;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_DeliveryPlanID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strType = DL_DeliveryGoodsType.SelectedValue.Trim();
        strGoodsCode = TB_DeliveryGoodsCode.Text.Trim();
        strGoodsName = TB_DeliveryGoodsName.Text.Trim();
        strUnitName = DL_DeliveryGoodsUnitName.SelectedValue.Trim();
        strModelNumber = TB_DeliveryGoodsModelNumber.Text.Trim();
        deNumber = NB_DeliveryGoodsNumber.Amount;
        strSpec = TB_DeliveryGoodsSpec.Text.Trim();
        dePrice = NB_DeliveryGoodsPrice.Amount;
        dtDeliveryTime = DateTime.Parse(DLC_DeliveryGoodsTime.Text);
        intPreDay = int.Parse(NB_DeliveryGoodsPreDay.Amount.ToString());

        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
            strHQL = "from ConstractGoodsDeliveryPlan as constractGoodsDeliveryPlan where constractGoodsDeliveryPlan.ID = " + strID;
            lst = constractGoodsDeliveryPlanBLL.GetAllConstractGoodsDeliveryPlans(strHQL);
            ConstractGoodsDeliveryPlan constractGoodsDeliveryPlan = (ConstractGoodsDeliveryPlan)lst[0];

            constractGoodsDeliveryPlan.ConstractCode = strConstractCode;
            constractGoodsDeliveryPlan.Type = strType;
            constractGoodsDeliveryPlan.GoodsCode = strGoodsCode;
            constractGoodsDeliveryPlan.GoodsName = strGoodsName;
            constractGoodsDeliveryPlan.ModelNumber = strModelNumber;
            constractGoodsDeliveryPlan.Spec = strSpec;
            constractGoodsDeliveryPlan.Brand = TB_DeliveryGoodsBrand.Text;

            constractGoodsDeliveryPlan.Number = deNumber;
            constractGoodsDeliveryPlan.Unit = strUnitName;
            constractGoodsDeliveryPlan.Price = dePrice;

            constractGoodsDeliveryPlan.Amount = deNumber * dePrice;

            constractGoodsDeliveryPlan.PreDay = intPreDay;
            constractGoodsDeliveryPlan.DeliveryTime = dtDeliveryTime;

            constractGoodsDeliveryPlan.DeliveryAddress = TB_DeliveryAddress.Text;
            constractGoodsDeliveryPlan.Status = DL_DeliveryStatus.SelectedValue.Trim();


            try
            {
                constractGoodsDeliveryPlanBLL.UpdateConstractGoodsDeliveryPlan(constractGoodsDeliveryPlan, int.Parse(strID));

                LoadConstractGoodsDeliveryPlanList(strConstractCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDeliveryPlanWindow','false') ", true);
            }
        }
    }

    protected void BT_CreateEntryOrder_Click(object sender, EventArgs e)
    {
        LB_EntryID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryOrderWindow','false') ", true);
    }

    protected void BT_EntryOrder_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_EntryID.Text.Trim();

        if (strID == "")
        {
            AddEntry();
        }
        else
        {
            UpdateEntry();
        }
    }

    protected void AddEntry()
    {
        string strEntryCode, strEntryName, strConstractCode, strCurrency, strCustoms, strPreEntryCode;
        decimal deAmount, deEntryTax, deAddedValueTax, deExchangeRate;
        DateTime dtImportDate, dtReportDate;

        strConstractCode = LB_ConstractCode.Text.Trim();

        strCustoms = TB_Customs.Text.Trim();
        strEntryCode = TB_EntryCode.Text.Trim();

        strEntryName = TB_EntryName.Text.Trim();
        strCurrency = DL_EncryCurrency.SelectedValue.Trim();

        deAmount = NB_EntryAmout.Amount;
        deEntryTax = NB_EntryTax.Amount;
        deAddedValueTax = NB_AddedValueTax.Amount;
        deExchangeRate = NB_ExchangeRate.Amount;

        strPreEntryCode = TB_PreEntryCode.Text.Trim();
        dtReportDate = DateTime.Parse(DLC_EntryReportDate.Text);
        dtImportDate = DateTime.Parse(DLC_EntryImportDate.Text);

        ConstractRelatedEntryOrderBLL constractRelatedEntryOrderBLL = new ConstractRelatedEntryOrderBLL();
        ConstractRelatedEntryOrder constractRelatedEntryOrder = new ConstractRelatedEntryOrder();

        constractRelatedEntryOrder.ConstractCode = strConstractCode;
        constractRelatedEntryOrder.EntryCode = strEntryCode;
        constractRelatedEntryOrder.PreEntryCode = strPreEntryCode;
        constractRelatedEntryOrder.EntryName = strEntryName;
        constractRelatedEntryOrder.Amount = deAmount;
        constractRelatedEntryOrder.Currency = strCurrency;
        constractRelatedEntryOrder.EntryTax = deEntryTax;
        constractRelatedEntryOrder.AddedValueTax = deAddedValueTax;
        constractRelatedEntryOrder.ExchangeRate = deExchangeRate;
        constractRelatedEntryOrder.Customs = strCustoms;
        constractRelatedEntryOrder.ImportDate = dtImportDate;
        constractRelatedEntryOrder.ReportDate = dtReportDate;

        try
        {
            constractRelatedEntryOrderBLL.AddConstractRelatedEntryOrder(constractRelatedEntryOrder);
            LB_EntryID.Text = ShareClass.GetMyCreatedMaxConstractEntryID(strConstractCode);

            LoadConstractRelatedEntry(strConstractCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCZXTDBGHJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryOrderWindow','true') ", true);
        }
    }

    protected void UpdateEntry()
    {
        string strID, strEntryCode, strEntryName, strConstractCode, strCurrency, strCustoms, strPreEntryCode;
        decimal deAmount, deEntryTax, deAddedValueTax, deExchangeRate;
        DateTime dtImportDate, dtReportDate;

        strID = LB_EntryID.Text.Trim();

        strConstractCode = LB_ConstractCode.Text.Trim();
        strCurrency = DL_EncryCurrency.SelectedValue.Trim();

        strEntryCode = TB_EntryCode.Text.Trim();
        strEntryName = TB_EntryName.Text.Trim();
        strCustoms = TB_Customs.Text.Trim();

        deAmount = NB_EntryAmout.Amount;
        deEntryTax = NB_EntryTax.Amount;
        deAddedValueTax = NB_AddedValueTax.Amount;
        deExchangeRate = NB_ExchangeRate.Amount;

        strPreEntryCode = TB_PreEntryCode.Text.Trim();
        dtReportDate = DateTime.Parse(DLC_EntryReportDate.Text);
        dtImportDate = DateTime.Parse(DLC_EntryImportDate.Text);

        string strHQL = "From ConstractRelatedEntryOrder as constractRelatedEntryOrder Where constractRelatedEntryOrder.ID = " + strID;
        ConstractRelatedEntryOrderBLL contractRelatedEntryOrderBLL = new ConstractRelatedEntryOrderBLL();
        IList lst = contractRelatedEntryOrderBLL.GetAllConstractRelatedEntryOrders(strHQL);
        ConstractRelatedEntryOrder constractRelatedEntryOrder = (ConstractRelatedEntryOrder)lst[0];

        constractRelatedEntryOrder.ConstractCode = strConstractCode;
        constractRelatedEntryOrder.EntryCode = strEntryCode;
        constractRelatedEntryOrder.PreEntryCode = strPreEntryCode;

        constractRelatedEntryOrder.EntryName = strEntryName;
        constractRelatedEntryOrder.Amount = deAmount;
        constractRelatedEntryOrder.Currency = strCurrency;
        constractRelatedEntryOrder.EntryTax = deEntryTax;
        constractRelatedEntryOrder.AddedValueTax = deAddedValueTax;
        constractRelatedEntryOrder.ExchangeRate = deExchangeRate;
        constractRelatedEntryOrder.Customs = strCustoms;

        constractRelatedEntryOrder.ImportDate = dtImportDate;
        constractRelatedEntryOrder.ReportDate = dtReportDate;

        try
        {
            contractRelatedEntryOrderBLL.UpdateConstractRelatedEntryOrder(constractRelatedEntryOrder, int.Parse(strID));
            LoadConstractRelatedEntry(strConstractCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZXTDBGHJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryOrderWindow','true') ", true);
        }
    }


    protected void DataGrid10_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strEntryID;

            strEntryID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                strHQL = "From ConstractRelatedEntryOrder as constractRelatedEntryOrder Where constractRelatedEntryOrder.ID = " + strEntryID;
                ConstractRelatedEntryOrderBLL contractRelatedEntryOrderBLL = new ConstractRelatedEntryOrderBLL();
                IList lst = contractRelatedEntryOrderBLL.GetAllConstractRelatedEntryOrders(strHQL);

                for (int i = 0; i < DataGrid10.Items.Count; i++)
                {
                    DataGrid10.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                ConstractRelatedEntryOrder constractRelatedEntryOrder = (ConstractRelatedEntryOrder)lst[0];

                LB_EntryID.Text = strEntryID;
                TB_EntryCode.Text = constractRelatedEntryOrder.EntryCode;
                TB_PreEntryCode.Text = constractRelatedEntryOrder.PreEntryCode;

                TB_EntryName.Text = constractRelatedEntryOrder.EntryName;
                NB_EntryAmout.Amount = constractRelatedEntryOrder.Amount;
                DL_EncryCurrency.SelectedValue = constractRelatedEntryOrder.Currency.Trim();
                NB_EntryTax.Amount = constractRelatedEntryOrder.EntryTax;
                NB_AddedValueTax.Amount = constractRelatedEntryOrder.AddedValueTax;
                NB_ExchangeRate.Amount = constractRelatedEntryOrder.ExchangeRate;
                TB_Customs.Text = constractRelatedEntryOrder.Customs;

                DLC_EntryImportDate.Text = constractRelatedEntryOrder.ReportDate.ToString("yyyy-MM-dd");
                DLC_EntryReportDate.Text = constractRelatedEntryOrder.ImportDate.ToString("yyyy-MM-dd");

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryOrderWindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strEntryCode, strConstractCode;

                strEntryCode = TB_EntryCode.Text.Trim();
                strConstractCode = LB_ConstractCode.Text.Trim();

                strHQL = "Delete From T_ConstractRelatedEntryOrder Where ID = " + strEntryID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadConstractRelatedEntry(strConstractCode);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
                }
            }
        }
    }


    protected void DataGrid15_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strEntryID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                strHQL = "From ConstractRelatedEntryOrderForInner as constractRelatedEntryOrderForInner Where constractRelatedEntryOrderForInner.ID = " + strEntryID;
                ConstractRelatedEntryOrderForInnerBLL contractRelatedEntryOrderBLL = new ConstractRelatedEntryOrderForInnerBLL();
                IList lst = contractRelatedEntryOrderBLL.GetAllConstractRelatedEntryOrderForInners(strHQL);

                for (int i = 0; i < DataGrid15.Items.Count; i++)
                {
                    DataGrid15.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                ConstractRelatedEntryOrderForInner constractRelatedEntryOrderForInner = (ConstractRelatedEntryOrderForInner)lst[0];

                LB_EntryIDForInner.Text = strEntryID;

                NB_EntryAmountForInner.Amount = constractRelatedEntryOrderForInner.Amount;
                DL_EntryCurrencyForInner.SelectedValue = constractRelatedEntryOrderForInner.Currency.Trim();
                NB_EntryTaxForInner.Amount = constractRelatedEntryOrderForInner.EntryTax;
                NB_EntryAddedValueTaxForInner.Amount = constractRelatedEntryOrderForInner.AddedValueTax;
                NB_EntryExchangeRateForInner.Amount = constractRelatedEntryOrderForInner.ExchangeRate;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryInnerWindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strConstractCode;

                strConstractCode = LB_ConstractCode.Text.Trim();

                strHQL = "Delete From T_ConstractRelatedEntryOrderForInner Where ID = " + strEntryID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadConstractRelatedEntryForInner(strConstractCode);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
                }
            }
        }
    }

    protected void BT_CreateEntryInner_Click(object sender, EventArgs e)
    {
        LB_EntryIDForInner.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryInnerWindow','false') ", true);
    }


    protected void BT_EntryInner_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_EntryIDForInner.Text.Trim();

        if (strID == "")
        {
            AddEntryInner();
        }
        else
        {
            UpdateEntryInner();
        }
    }


    protected void AddEntryInner()
    {
        string strConstractCode, strCurrency;
        decimal deAmount, deEntryTax, deAddedValueTax, deExchangeRate;

        strConstractCode = LB_ConstractCode.Text.Trim();
        strCurrency = DL_EntryCurrencyForInner.SelectedValue.Trim();
        deAmount = NB_EntryAmountForInner.Amount;
        deEntryTax = NB_EntryTaxForInner.Amount;
        deAddedValueTax = NB_EntryAddedValueTaxForInner.Amount;
        deExchangeRate = NB_EntryExchangeRateForInner.Amount;

        ConstractRelatedEntryOrderForInnerBLL constractRelatedEntryOrderForInnerBLL = new ConstractRelatedEntryOrderForInnerBLL();
        ConstractRelatedEntryOrderForInner constractRelatedEntryOrderForInner = new ConstractRelatedEntryOrderForInner();


        constractRelatedEntryOrderForInner.ConstractCode = strConstractCode;

        constractRelatedEntryOrderForInner.Amount = deAmount;
        constractRelatedEntryOrderForInner.Currency = strCurrency;
        constractRelatedEntryOrderForInner.EntryTax = deEntryTax;
        constractRelatedEntryOrderForInner.AddedValueTax = deAddedValueTax;
        constractRelatedEntryOrderForInner.ExchangeRate = deExchangeRate;


        try
        {
            constractRelatedEntryOrderForInnerBLL.AddConstractRelatedEntryOrderForInner(constractRelatedEntryOrderForInner);
            LB_EntryIDForInner.Text = ShareClass.GetMyCreatedMaxConstractEntryID(strConstractCode);


            LoadConstractRelatedEntryForInner(strConstractCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCZXTDBGHJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryInnerWindow','true') ", true);
        }
    }

    protected void UpdateEntryInner()
    {
        string strID, strConstractCode, strCurrency;
        decimal deAmount, deEntryTax, deAddedValueTax, deExchangeRate;

        strID = LB_EntryIDForInner.Text.Trim();

        strConstractCode = LB_ConstractCode.Text.Trim();
        strCurrency = DL_EntryCurrencyForInner.SelectedValue.Trim();
        deAmount = NB_EntryAmountForInner.Amount;
        deEntryTax = NB_EntryTaxForInner.Amount;
        deAddedValueTax = NB_EntryAddedValueTaxForInner.Amount;
        deExchangeRate = NB_EntryExchangeRateForInner.Amount;

        string strHQL = "From ConstractRelatedEntryOrderForInner as constractRelatedEntryOrderForInner Where constractRelatedEntryOrderForInner.ID = " + strID;
        ConstractRelatedEntryOrderForInnerBLL contractRelatedEntryOrderBLL = new ConstractRelatedEntryOrderForInnerBLL();
        IList lst = contractRelatedEntryOrderBLL.GetAllConstractRelatedEntryOrderForInners(strHQL);
        ConstractRelatedEntryOrderForInner constractRelatedEntryOrderForInner = (ConstractRelatedEntryOrderForInner)lst[0];

        constractRelatedEntryOrderForInner.ConstractCode = strConstractCode;

        constractRelatedEntryOrderForInner.Amount = deAmount;
        constractRelatedEntryOrderForInner.Currency = strCurrency;
        constractRelatedEntryOrderForInner.EntryTax = deEntryTax;
        constractRelatedEntryOrderForInner.AddedValueTax = deAddedValueTax;
        constractRelatedEntryOrderForInner.ExchangeRate = deExchangeRate;

        try
        {
            contractRelatedEntryOrderBLL.UpdateConstractRelatedEntryOrderForInner(constractRelatedEntryOrderForInner, int.Parse(strID));
            LoadConstractRelatedEntryForInner(strConstractCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZXTDBGHJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEntryInnerWindow','true') ", true);
        }
    }



    protected void LoadConstractRelatedEntry(string strConstractCode)
    {
        string strHQL;

        strHQL = "From ConstractRelatedEntryOrder as constractRelatedEntryOrder Where constractRelatedEntryOrder.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractRelatedEntryOrderBLL contractRelatedEntryOrderBLL = new ConstractRelatedEntryOrderBLL();
        IList lst = contractRelatedEntryOrderBLL.GetAllConstractRelatedEntryOrders(strHQL);

        DataGrid10.DataSource = lst;
        DataGrid10.DataBind();
    }

    protected void LoadConstractRadio()
    {
        string strHQL;

        strHQL = "Select * From T_ConstractRadio ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractRadio");

        DL_ReceivablesIncomeRatio.DataSource = ds;
        DL_ReceivablesIncomeRatio.DataBind();
    }

    protected void LoadConstractRelatedEntryForInner(string strConstractCode)
    {
        string strHQL;

        strHQL = "From ConstractRelatedEntryOrderForInner as constractRelatedEntryOrderForInner Where constractRelatedEntryOrderForInner.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractRelatedEntryOrderForInnerBLL contractRelatedEntryOrderBLL = new ConstractRelatedEntryOrderForInnerBLL();
        IList lst = contractRelatedEntryOrderBLL.GetAllConstractRelatedEntryOrderForInners(strHQL);

        DataGrid15.DataSource = lst;
        DataGrid15.DataBind();
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode, strHQL;

            string strID;

            strUserCode = LB_UserCode.Text.Trim();

            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ID= " + strID;

                ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();

                IList lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                ConstractReceivables constractReceivables = (ConstractReceivables)lst[0];

                LB_ReceivablesID.Text = constractReceivables.ID.ToString();

                TB_ReceivablesBillCode.Text = constractReceivables.BillCode.Trim();
                TB_ReceiveAccount.Text = constractReceivables.Account.Trim();
                NB_ReceivablesAccount.Amount = constractReceivables.ReceivablesAccount;

                DLC_ReceivablesTime.Text = constractReceivables.ReceivablesTime.ToString("yyyy-MM-dd");

                LB_ReceivablesCurrency.Text = constractReceivables.CurrencyType.Trim();

                LB_ReceivablesOperatorCode.Text = constractReceivables.OperatorCode.Trim();
                LB_ReceivablesOperatorName.Text = constractReceivables.OperatorName.Trim();
                NB_ReceivablesPreDays.Amount = constractReceivables.PreDays;
                DL_ReceivablesStatus.SelectedValue = constractReceivables.Status.Trim();
                TB_Payer.Text = constractReceivables.Payer.Trim();
                TB_ReceiveComment.Text = constractReceivables.Comment.Trim();

                DL_ReceivablesRelatedType.SelectedValue = constractReceivables.RelatedType.Trim();
                NB_ReceivablesRelatedID.Amount = constractReceivables.RelatedID;

                lbl_AccountCode.Text = constractReceivables.AccountCode.Trim();

                DL_IsSecrecyReceiver.SelectedValue = constractReceivables.IsSecrecy.Trim();

                NB_ReceivablesRelatedPlanID.Amount = constractReceivables.RelatedPlanID;

                LB_ReceivablesPlanName.Text = ShareClass.GetProjectPlanDetail(constractReceivables.RelatedPlanID.ToString());

                //加载其它属性
                LoadConstractReceivablesOtherField(strID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strConstractCode;
                IList lst;

                strConstractCode = LB_ConstractCode.Text.Trim();

                strHQL = "from  ConstractReceivables as constractReceivables where constractReceivables.ID= " + strID;
                ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
                lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);
                ConstractReceivables constractReceivables = (ConstractReceivables)lst[0];

                try
                {
                    constractReceivablesBLL.DeleteConstractReceivables(constractReceivables);
                    LoadConstractReceivables(strConstractCode, strAuthority);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    //加载其它属性
    protected void LoadConstractReceivablesOtherField(string strReceivablesID)
    {
        string strHQL;

        strHQL = "Select * From T_ConstractReceivables Where ID = " + strReceivablesID;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_constractReceivables");

        NB_ReceivaleTaxRate.Amount = decimal.Parse(ds.Tables[0].Rows[0]["TaxRate"].ToString());

        try
        {
            DL_ReceivablesIncomeRatio.SelectedValue = ds.Tables[0].Rows[0]["IncomeRatio"].ToString().Trim();
        }
        catch
        {
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode, strHQL;

            string strID;

            strUserCode = LB_UserCode.Text.Trim();

            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                strHQL = "from  ConstractPayable as constractPayable where constractPayable.ID= " + strID;

                ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();

                IList lst = constractPayableBLL.GetAllConstractPayables(strHQL);

                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                ConstractPayable constractPayable = (ConstractPayable)lst[0];

                LB_PayableID.Text = constractPayable.ID.ToString();

                TB_PayableBillCode.Text = constractPayable.BillCode.Trim();
                TB_PayAccount.Text = constractPayable.Account.Trim();
                NB_PayableAccount.Amount = constractPayable.PayableAccount;

                //NB_PayOtherAccount.Amount = constractPayable.PayOtherAccount;
                //NB_OtherAccount.Amount = constractPayable.OtherAccount;

                DLC_PayableTime.Text = constractPayable.PayableTime.ToString("yyyy-MM-dd");

                LB_PayableCurrency.Text = constractPayable.CurrencyType.Trim();

                LB_PayableOperatorCode.Text = constractPayable.OperatorCode.Trim();
                LB_PayableOperatorName.Text = constractPayable.OperatorName.Trim();

                DL_PayableStatus.SelectedValue = constractPayable.Status.Trim();
                NB_PayablePreDays.Amount = constractPayable.PreDays;

                TB_Receiver.Text = constractPayable.Receiver.Trim();
                TB_PayableComment.Text = constractPayable.Comment.Trim();

                DL_PayablesRelatedType.SelectedValue = constractPayable.RelatedType.Trim();
                NB_PayableRelatedID.Amount = constractPayable.RelatedID;

                LB_RelatedPJBudgetID.Text = constractPayable.RelatedPJBudgetID.ToString();
                LB_RelatedPJBudgetAccountCode.Text = constractPayable.RelatedPJBudgetAccountCode;
                LB_RelatedPJBudgetAccount.Text = constractPayable.RelatedPJBudgetAccount;

                if (DL_PayablesRelatedType.SelectedValue == "Project")
                {
                    BT_SelectPJBudget.Visible = true;
                }
                else
                {
                    BT_SelectPJBudget.Visible = false;
                }

                lbl_AccountCode1.Text = constractPayable.AccountCode.Trim();

                DL_IsSecrecyPayable.SelectedValue = constractPayable.IsSecrecy.Trim();

                NB_PayableRelatedPlanID.Amount = constractPayable.RelatedPlanID;

                LB_PayablePlanName.Text = ShareClass.GetProjectPlanDetail(constractPayable.RelatedPlanID.ToString());

                //加载其它属性
                LoadConstractPayableOtherField(strID);

                LoadConstractPayableVisa(strID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strConstractCode;
                IList lst;

                strConstractCode = LB_ConstractCode.Text.Trim();

                strHQL = "from  ConstractPayable as constractPayable where constractPayable.ID= " + strID;
                ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
                lst = constractPayableBLL.GetAllConstractPayables(strHQL);
                ConstractPayable constractPayable = (ConstractPayable)lst[0];

                try
                {
                    constractPayableBLL.DeleteConstractPayable(constractPayable);
                    LoadConstractPayable(strConstractCode, strAuthority);

                    strHQL = "Delete From T_ProExpense Where ConstractPayID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    //加载其它属性
    protected void LoadConstractPayableOtherField(string strConstractPayableID)
    {
        string strHQL;

        strHQL = "Select * From T_ConstractPayable Where ID =" + strConstractPayableID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractPayable");

        NB_PayableTaxRate.Amount = decimal.Parse(ds.Tables[0].Rows[0]["TaxRate"].ToString());
        DL_PayableIncomeRatio.SelectedValue = ds.Tables[0].Rows[0]["IncomeRatio"].ToString().Trim();
    }


    protected void BT_CreateSales_Click(object sender, EventArgs e)
    {
        LB_SalesID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popSalesWindow','false') ", true);
    }

    protected void BT_Sales_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_SalesID.Text.Trim();

        if (strID == "")
        {
            AddSales();
        }
        else
        {
            UpdateSales();
        }
    }

    protected void AddSales()
    {
        string strSalesID, strSalesName, strDuty, strComment, strStatus;
        decimal deCommission;
        DateTime dtGiveTime;

        string strUserCode = LB_UserCode.Text.Trim();

        string strConstractCode = LB_ConstractCode.Text.Trim();

        strSalesID = LB_SalesID.Text.Trim();
        strSalesName = TB_SalesName.Text.Trim();
        strDuty = TB_SalesDuty.Text.Trim();
        strComment = TB_SalesComment.Text.Trim();
        strStatus = DL_CommissionStatus.SelectedValue.Trim();

        deCommission = NB_Commission.Amount;
        dtGiveTime = DateTime.Parse(DLC_GiveTime.Text);

        ConstractSalesBLL constractSalesBLL = new ConstractSalesBLL();
        ConstractSales constractSales = new ConstractSales();

        constractSales.ConstractCode = strConstractCode;
        constractSales.SalesName = strSalesName;
        constractSales.Duty = strDuty;
        constractSales.Comment = strComment;
        constractSales.Commission = deCommission;
        constractSales.GiveTime = dtGiveTime;
        constractSales.CommissionStatus = strStatus;

        try
        {
            constractSalesBLL.AddConstractSales(constractSales);
            LB_SalesID.Text = ShareClass.GetMyCreatedMaxConstractSalesID(strConstractCode);

            LoadConstractSales(strConstractCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popSalesWindow','false') ", true);
        }
    }

    protected void UpdateSales()
    {
        string strHQL;
        IList lst;

        string strSalesID, strSalesName, strDuty, strComment, strStatus;
        decimal deCommission;
        DateTime dtGiveTime;

        string strUserCode = LB_UserCode.Text.Trim();
        string strConstractCode = LB_ConstractCode.Text.Trim();

        strSalesID = LB_SalesID.Text.Trim();
        strSalesName = TB_SalesName.Text.Trim();
        strDuty = TB_SalesDuty.Text.Trim();
        strComment = TB_SalesComment.Text.Trim();
        strStatus = DL_CommissionStatus.SelectedValue.Trim();

        deCommission = NB_Commission.Amount;
        dtGiveTime = DateTime.Parse(DLC_GiveTime.Text);

        strHQL = "From ConstractSales as constractSales Where constractSales.ID = " + strSalesID;
        ConstractSalesBLL constractSalesBLL = new ConstractSalesBLL();
        lst = constractSalesBLL.GetAllConstractSaless(strHQL);

        ConstractSales constractSales = (ConstractSales)lst[0];

        constractSales.SalesName = strSalesName;
        constractSales.Duty = strDuty;
        constractSales.Comment = strComment;
        constractSales.Commission = deCommission;
        constractSales.GiveTime = dtGiveTime;
        constractSales.CommissionStatus = strStatus;

        try
        {
            constractSalesBLL.UpdateConstractSales(constractSales, int.Parse(strSalesID));

            LoadConstractSales(strConstractCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popSalesWindows','false') ", true);
        }
    }


    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strUserCode;
            string strSalesID;

            strUserCode = LB_UserCode.Text.Trim();

            strSalesID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid3.Items.Count; i++)
                {
                    DataGrid3.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;


                strHQL = "From ConstractSales as constractSales Where constractSales.ID = " + strSalesID;
                ConstractSalesBLL constractSalesBLL = new ConstractSalesBLL();
                lst = constractSalesBLL.GetAllConstractSaless(strHQL);

                ConstractSales constractSales = (ConstractSales)lst[0];

                LB_SalesID.Text = constractSales.ID.ToString();
                TB_SalesName.Text = constractSales.SalesName.Trim();
                TB_SalesDuty.Text = constractSales.Duty.Trim();
                NB_Commission.Amount = constractSales.Commission;

                DLC_GiveTime.Text = constractSales.GiveTime.ToString("yyyy-MM-dd");

                DL_CommissionStatus.SelectedValue = constractSales.CommissionStatus.Trim();
                TB_SalesComment.Text = constractSales.Comment.Trim();

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popSalesWindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strSalesName, strDuty, strComment, strStatus;
                decimal deCommission;
                DateTime dtGiveTime;

                string strConstractCode = LB_ConstractCode.Text.Trim();


                strSalesName = TB_SalesName.Text.Trim();
                strDuty = TB_SalesDuty.Text.Trim();
                strComment = TB_SalesComment.Text.Trim();
                strStatus = DL_CommissionStatus.SelectedValue.Trim();

                deCommission = NB_Commission.Amount;
                dtGiveTime = DateTime.Parse(DLC_GiveTime.Text);

                ConstractSalesBLL constractSalesBLL = new ConstractSalesBLL();
                ConstractSales constractSales = new ConstractSales();

                constractSales.ID = int.Parse(strSalesID);


                try
                {
                    constractSalesBLL.DeleteConstractSales(constractSales);

                    LoadConstractSales(strConstractCode);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }


    protected void DataGrid12_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strUserCode;
            string strID;

            strUserCode = LB_UserCode.Text.Trim();

            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid12.Items.Count; i++)
                {
                    DataGrid12.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;


                strHQL = "From ConstractRelatedInvoice as constractRelatedInvoice Where constractRelatedInvoice.ID = " + strID;
                ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
                lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);

                ConstractRelatedInvoice constractRelatedInvoice = (ConstractRelatedInvoice)lst[0];


                LB_InvoiceID.Text = constractRelatedInvoice.ID.ToString();
                DL_InvoiceReceiveOPen.SelectedValue = constractRelatedInvoice.ReceiveOpen.Trim();
                DL_TaxType.SelectedValue = constractRelatedInvoice.TaxType.Trim();
                TB_InvoiceCode.Text = constractRelatedInvoice.InvoiceCode.Trim();
                NB_InvoiceAmount.Amount = constractRelatedInvoice.Amount;
                NB_InvoiceTaxRate.Amount = constractRelatedInvoice.TaxRate;

                DLC_OpenDate.Text = constractRelatedInvoice.OpenDate.ToString("yyyy-MM-dd");


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strInvoiceCode, strConstractCode;
                decimal deAmount, deTaxRate;

                strConstractCode = LB_ConstractCode.Text.Trim();
                strInvoiceCode = TB_InvoiceCode.Text.Trim();
                deAmount = NB_InvoiceAmount.Amount;
                deTaxRate = NB_InvoiceTaxRate.Amount;

                ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
                strHQL = "From ConstractRelatedInvoice as constractRelatedInvoice Where constractRelatedInvoice.ID = " + strID;
                lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);
                ConstractRelatedInvoice constractRelatedInvoice = (ConstractRelatedInvoice)lst[0];

                constractRelatedInvoice.ConstractCode = strConstractCode;
                constractRelatedInvoice.InvoiceCode = TB_InvoiceCode.Text.Trim();
                constractRelatedInvoice.Amount = deAmount;
                constractRelatedInvoice.TaxRate = deTaxRate;

                try
                {
                    constractRelatedInvoiceBLL.DeleteConstractRelatedInvoice(constractRelatedInvoice);
                    LoadConstractRelatedInvoice(strConstractCode);

                    CountInvoiceAmount(strConstractCode);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void BT_OpenInvoice_Click(object sender, EventArgs e)
    {
        LB_InvoiceID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','false') ", true);
    }

    protected void BT_Invoice_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_InvoiceID.Text.Trim();

        if (strID == "")
        {
            AddInvoice();
        }
        else
        {
            UpdateInvoice();
        }
    }

    protected void AddInvoice()
    {
        string strInvoiceCode, strConstractCode, strTaxType;
        decimal deAmount, deTaxRate;
        DateTime dtOpenDate;

        strConstractCode = LB_ConstractCode.Text.Trim();
        strInvoiceCode = TB_InvoiceCode.Text.Trim();
        strTaxType = DL_TaxType.SelectedValue.Trim();
        deAmount = NB_InvoiceAmount.Amount;
        deTaxRate = NB_InvoiceTaxRate.Amount;
        dtOpenDate = DateTime.Parse(DLC_OpenDate.Text);

        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        ConstractRelatedInvoice constractRelatedInvoice = new ConstractRelatedInvoice();

        constractRelatedInvoice.ReceiveOpen = DL_InvoiceReceiveOPen.SelectedValue.Trim();
        constractRelatedInvoice.ConstractCode = strConstractCode;
        constractRelatedInvoice.InvoiceCode = TB_InvoiceCode.Text.Trim();
        constractRelatedInvoice.TaxType = strTaxType;
        constractRelatedInvoice.Amount = deAmount;
        constractRelatedInvoice.TaxRate = deTaxRate;
        constractRelatedInvoice.OpenDate = dtOpenDate;
        constractRelatedInvoice.RelatedType = "OTHER";
        constractRelatedInvoice.RelatedID = "0";

        try
        {
            constractRelatedInvoiceBLL.AddConstractRelatedInvoice(constractRelatedInvoice);
            LoadConstractRelatedInvoice(strConstractCode);

            CountInvoiceAmount(strConstractCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true') ", true);
        }
    }

    protected void UpdateInvoice()
    {
        string strHQL;
        IList lst;

        string strID, strInvoiceCode, strConstractCode, strTaxType;
        decimal deAmount, deTaxRate;
        DateTime dtOpenDate;

        strID = LB_InvoiceID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strTaxType = DL_TaxType.SelectedValue.Trim();
        strInvoiceCode = TB_InvoiceCode.Text.Trim();
        deAmount = NB_InvoiceAmount.Amount;
        deTaxRate = NB_InvoiceTaxRate.Amount;
        dtOpenDate = DateTime.Parse(DLC_OpenDate.Text);

        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        strHQL = "From ConstractRelatedInvoice as constractRelatedInvoice Where constractRelatedInvoice.ID = " + strID;
        lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);
        ConstractRelatedInvoice constractRelatedInvoice = (ConstractRelatedInvoice)lst[0];

        constractRelatedInvoice.ReceiveOpen = DL_InvoiceReceiveOPen.SelectedValue.Trim();
        constractRelatedInvoice.ConstractCode = LB_ConstractCode.Text.Trim();
        constractRelatedInvoice.InvoiceCode = TB_InvoiceCode.Text.Trim();
        constractRelatedInvoice.TaxType = strTaxType;
        constractRelatedInvoice.Amount = deAmount;
        constractRelatedInvoice.TaxRate = deTaxRate;
        constractRelatedInvoice.OpenDate = dtOpenDate;
        constractRelatedInvoice.RelatedType = "OTHER";
        constractRelatedInvoice.RelatedID = "0";

        try
        {
            constractRelatedInvoiceBLL.UpdateConstractRelatedInvoice(constractRelatedInvoice, int.Parse(strID));

            LoadConstractRelatedInvoice(strConstractCode);

            CountInvoiceAmount(strConstractCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true') ", true);
        }
    }


    protected void DL_ReceiveAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccount = DL_ReceiveAccount.SelectedValue.Trim();
        lbl_AccountCode.Text = strAccount;
        TB_ReceiveAccount.Text = GetAccountName(strAccount);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
    }

    protected void DL_Expense_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccount = DL_PayAccount.SelectedValue.Trim();
        lbl_AccountCode1.Text = strAccount;
        TB_PayAccount.Text = GetAccountName(strAccount);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
    }

    protected void DL_Payer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPayer = DL_Payer.SelectedValue.Trim();

        TB_Payer.Text = strPayer;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
    }

    protected void DL_Receiver_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strReceiver = DL_Receiver.SelectedValue.Trim();

        TB_Receiver.Text = strReceiver;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);
    }

    protected string GetAccountName(string strAccountCode)
    {
        string flag = "";
        string strHQL = "Select AccountName From T_Account where AccountCode='" + strAccountCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_Account").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["AccountName"].ToString();
        }
        else
        {
            flag = "";
        }
        return flag;
    }

    protected void CountReceivablesAmount(string strConstractCode, string strAuthority)
    {
        string strHQL;
        IList lst;

        decimal deReceivablesAmount = 0, deReceiverAmount = 0, deUnReceiveAmount = 0;

        if (strAuthority == "ALL")
        {
            strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ConstractCode = " + "'" + strConstractCode + "'";
        }
        else
        {
            strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ConstractCode = " + "'" + strConstractCode + "'";
            strHQL += " and constractReceivables.IsSecrecy = 'NO'";
        }
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

        ConstractReceivables constractReceivables = new ConstractReceivables();

        for (int i = 0; i < lst.Count; i++)
        {
            constractReceivables = (ConstractReceivables)lst[i];

            deReceivablesAmount += constractReceivables.ReceivablesAccount;
            deReceiverAmount += constractReceivables.ReceiverAccount;
            deUnReceiveAmount += constractReceivables.UNReceiveAmount;
        }

        LB_ReceivablesAmount.Text = deReceivablesAmount.ToString();
        LB_ReceiverAmount.Text = deReceiverAmount.ToString();
        LB_UNReceiverAmount.Text = deUnReceiveAmount.ToString();
    }

    protected void CountPayableAmount(string strConstractCode, string strAuthority)
    {
        string strHQL;
        IList lst;

        decimal dePayableAmount = 0, dePayAmount = 0, deUnPayAmount = 0;

        if (strAuthority == "ALL")
        {
            strHQL = "from ConstractPayable as constractPayable where constractPayable.ConstractCode = " + "'" + strConstractCode + "'";
        }
        else
        {
            strHQL = "from ConstractPayable as constractPayable where constractPayable.ConstractCode = " + "'" + strConstractCode + "'";
            strHQL += " and constractPayable.IsSecrecy = 'NO'";
        }
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);

        ConstractPayable constractPayable = new ConstractPayable();

        for (int i = 0; i < lst.Count; i++)
        {
            constractPayable = (ConstractPayable)lst[i];

            dePayableAmount += constractPayable.PayableAccount;
            dePayAmount += constractPayable.OutOfPocketAccount;
            deUnPayAmount += constractPayable.UNPayAmount;
        }

        LB_PayableAmount.Text = dePayableAmount.ToString();
        LB_PayAmount.Text = dePayAmount.ToString();
        LB_UNPayAmount.Text = deUnPayAmount.ToString();
    }

    protected void CountInvoiceAmount(string strConstractCode)
    {
        string strHQL;
        IList lst;

        decimal deOpenInvoiceAmount = 0, deReceiveInvoiceAmount = 0;
        string strType;

        strHQL = "from ConstractRelatedInvoice as constractRelatedInvoice where constractRelatedInvoice.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);

        ConstractRelatedInvoice constractRelatedInvoice = new ConstractRelatedInvoice();

        for (int i = 0; i < lst.Count; i++)
        {
            constractRelatedInvoice = (ConstractRelatedInvoice)lst[i];

            strType = constractRelatedInvoice.ReceiveOpen.Trim();

            if (strType == "OPEN")
            {

                deOpenInvoiceAmount += constractRelatedInvoice.Amount;
            }

            if (strType == "RECEIVE")
            {
                deReceiveInvoiceAmount += constractRelatedInvoice.Amount;
            }
        }


        LB_TotalOpenInvoiceAmount.Text = deOpenInvoiceAmount.ToString();
        LB_TotalReceiveInvoiceAmount.Text = deReceiveInvoiceAmount.ToString();
    }

    protected string GetConstractRelatedUserAuthority(string strConstractCode, string strUserCode)
    {
        string strHQL;
        IList lst;

        string strRecorderCode;

        strHQL = "From Constract as constract where constract.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        Constract constract = (Constract)lst[0];

        strRecorderCode = constract.RecorderCode.Trim();

        if (strRecorderCode != strUserCode)
        {
            strHQL = "from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.ConstractCode = " + "'" + strConstractCode + "'" + " and constractRelatedUser.UserCode = " + "'" + strUserCode + "'";
            ConstractRelatedUserBLL constractRelatedUserBLL = new ConstractRelatedUserBLL();
            lst = constractRelatedUserBLL.GetAllConstractRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ConstractRelatedUser constractRelatedUser = (ConstractRelatedUser)lst[0];
                return constractRelatedUser.Authority.Trim();
            }
            else
            {
                return "NO";
            }
        }
        else
        {
            return "ALL";
        }
    }

    protected string GetConstractCurrencyType(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Constract as constract where constract.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        Constract constract = (Constract)lst[0];

        return constract.Currency.Trim();
    }



    protected void LoadConstractReceivables(string strConstractCode, string strAuthority)
    {
        string strHQL;
        IList lst;

        if (strAuthority == "ALL")
        {
            strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ConstractCode = " + "'" + strConstractCode + "'";
        }
        else
        {
            strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ConstractCode = " + "'" + strConstractCode + "'";
            strHQL += " and constractReceivables.IsSecrecy='NO'";
        }
        strHQL += " Order By constractReceivables.ID DESC";

        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        CountReceivablesAmount(strConstractCode, strAuthority);
    }

    protected void LoadConstractPayable(string strConstractCode, string strAuthority)
    {
        string strHQL;
        IList lst;

        if (strAuthority == "ALL")
        {
            strHQL = "from ConstractPayable as constractPayable where constractPayable.ConstractCode = " + "'" + strConstractCode + "'";
        }
        else
        {
            strHQL = "from ConstractPayable as constractPayable where constractPayable.ConstractCode = " + "'" + strConstractCode + "'";
            strHQL += " and constractPayable.IsSecrecy='NO'";
        }
        strHQL += " Order By constractPayable.ID DESC";
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        CountPayableAmount(strConstractCode, strAuthority);
    }

    protected void LoadConstractReceivablesPayer(string strOperatorCode)
    {
        string strHQL;

        strHQL = "Select Distinct(Payer) from T_ConstractReceivables where ";
        strHQL += " OperatorCode = " + "'" + strOperatorCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivables");

        DL_Payer.DataSource = ds;
        DL_Payer.DataBind();

        DL_Payer.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadConstractPayableReceiver(string strOperatorCode)
    {
        string strHQL;

        strHQL = "Select Distinct(Receiver) from T_ConstractPayable where ";
        strHQL += " OperatorCode = " + "'" + strOperatorCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractPayable");

        DL_Receiver.DataSource = ds;
        DL_Receiver.DataBind();

        DL_Receiver.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadConstractRelatedInvoice(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractRelatedInvoice as constractRelatedInvoice where constractRelatedInvoice.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);

        DataGrid12.DataSource = lst;
        DataGrid12.DataBind();
    }

    protected void LoadConstractSales(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From ConstractSales as constractSales Where constractSales.ConstractCode = " + "'" + strConstractCode + "'" + " Order By constractSales.ID ASC";
        ConstractSalesBLL constractSalesBLL = new ConstractSalesBLL();
        lst = constractSalesBLL.GetAllConstractSaless(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void LoadRelatedProject(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From ConstractRelatedProject as constractRelatedProject Where constractRelatedProject.ConstractCode = " + "'" + strConstractCode + "'" + " Order By constractRelatedProject.ID DESC";
        ConstractRelatedProjectBLL constractRelatedProjectBLL = new ConstractRelatedProjectBLL();
        lst = constractRelatedProjectBLL.GetAllConstractRelatedProjects(strHQL);

        DL_PayableRelatedProjectID.DataSource = lst;
        DL_PayableRelatedProjectID.DataBind();

        DL_ReceiveRelatedProjectID.DataSource = lst;
        DL_ReceiveRelatedProjectID.DataBind();

        DL_PayableRelatedProjectID.Items.Insert(0, new ListItem("0"));
        DL_ReceiveRelatedProjectID.Items.Insert(0, new ListItem("0"));
    }

    protected void LoadRelatedGoodsSaleOrder(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.SOID in ( select constractRelatedGoodsSaleOrder.SOID from  ConstractRelatedGoodsSaleOrder as constractRelatedGoodsSaleOrder where constractRelatedGoodsSaleOrder.ConstractCode = " + "'" + strConstractCode + "'" + ")";
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        DL_ReceiveRelatedGoodsSOID.DataSource = lst;
        DL_ReceiveRelatedGoodsSOID.DataBind();

        DL_ReceiveRelatedGoodsSOID.Items.Insert(0, new ListItem("0"));
    }

    protected void LoadRelatedGoodsPurchaseOrder(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsPurchaseOrder as goodsPurchaseOrder where goodsPurchaseOrder.POID in ( select constractRelatedGoodsPurchaseOrder.POID from  ConstractRelatedGoodsPurchaseOrder as constractRelatedGoodsPurchaseOrder where constractRelatedGoodsPurchaseOrder.ConstractCode = " + "'" + strConstractCode + "'" + ")";
        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        lst = goodsPurchaseOrderBLL.GetAllGoodsPurchaseOrders(strHQL);

        DL_PayableRelatedGoodsPurchaseID.DataSource = lst;
        DL_PayableRelatedGoodsPurchaseID.DataBind();

        DL_PayableRelatedGoodsPurchaseID.Items.Insert(0, new ListItem("0"));
    }

    protected void LoadRelatedAssetPurchaseOrder(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID in ( select constractRelatedAssetPurchaseOrder.POID from  ConstractRelatedAssetPurchaseOrder as constractRelatedAssetPurchaseOrder where constractRelatedAssetPurchaseOrder.ConstractCode = " + "'" + strConstractCode + "'" + ")";
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DL_PayableRelatedAssetPurchaseID.DataSource = lst;
        DL_PayableRelatedAssetPurchaseID.DataBind();

        DL_PayableRelatedAssetPurchaseID.Items.Insert(0, new ListItem("0"));
    }

    protected void LoadConstractRelatedGoodsList(string strConstractCode)
    {
        string strHQL;
        IList lst;

        ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
        strHQL = "from ConstractRelatedGoods as constractRelatedGoods where constractRelatedGoods.ConstractCode = " + "'" + strConstractCode + "'";
        lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);

        DataGrid11.DataSource = lst;
        DataGrid11.DataBind();

        DataGrid24.DataSource = lst;
        DataGrid24.DataBind();

        DataGrid25.DataSource = lst;
        DataGrid25.DataBind();

        strHQL = "Select Sum(Number * Price) From T_ConstractRelatedGoods Where ConstractCode = " + "'" + strConstractCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractRelatedGoods");

        LB_ConstractItemAmount.Text = ds.Tables[0].Rows[0][0].ToString();
    }

    protected void LoadConstractGoodsReceiptPlanList(string strConstractCode)
    {
        string strHQL;
        IList lst;

        ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
        strHQL = "from ConstractGoodsReceiptPlan as constractGoodsReceiptPlan where constractGoodsReceiptPlan.ConstractCode = " + "'" + strConstractCode + "'";
        lst = constractGoodsReceiptPlanBLL.GetAllConstractGoodsReceiptPlans(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadConstractGoodsDeliveryPlanList(string strConstractCode)
    {
        string strHQL;
        IList lst;

        ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
        strHQL = "from ConstractGoodsDeliveryPlan as constractGoodsDeliveryPlan where constractGoodsDeliveryPlan.ConstractCode = " + "'" + strConstractCode + "'";
        lst = constractGoodsDeliveryPlanBLL.GetAllConstractGoodsDeliveryPlans(strHQL);

        DataGrid7.DataSource = lst;
        DataGrid7.DataBind();
    }

    protected void DataGrid17_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strVisaID;
            IList lst;

            strVisaID = e.Item.Cells[1].Text.Trim();

            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from ConstractPayableVisa as constractPayableVisa where constractPayableVisa.ID = " + strVisaID;
            ConstractPayableVisaBLL constractPayableVisaBLL = new ConstractPayableVisaBLL();
            lst = constractPayableVisaBLL.GetAllConstractPayableVisas(strHQL);
            ConstractPayableVisa constractPayableVisa = (ConstractPayableVisa)lst[0];

            constractPayableVisa.ConstractPayableID = int.Parse(LB_PayableID.Text);

            try
            {
                constractPayableVisaBLL.UpdateConstractPayableVisa(constractPayableVisa, int.Parse(strVisaID));
            }
            catch
            {

            }

            LoadConstractPayableVisa(LB_PayableID.Text);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
        }
    }

    protected void RP_RelatedConstractPayableVisa_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strVisaID = ((Button)e.Item.FindControl("BT_VisaID")).Text;

            string strHQL;
            strHQL = "Update T_ConstractPayableVisa Set ConstractPayableID = 0 Where ID = " + strVisaID;
            ShareClass.RunSqlCommand(strHQL);

            LoadConstractPayableVisa(LB_PayableID.Text);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
        }
    }

    protected void BT_ReceivablesSelectPJPlan_Click(object sender, EventArgs e)
    {

        try
        {
            string strProjectID = NB_ReceivablesRelatedID.Amount.ToString();
            string strPlanVerID = ShareClass.GetProjectPlanVerID(strProjectID, "InUse").ToString();

            TakeTopPlan.InitialProjectPlanTree(TreeView_ReceivablesPJPlantTree, strProjectID, strPlanVerID);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true','popReceivablesSelectPJPlanWindow') ", true);
    }

    protected void BT_PayableSelectPJPlan_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectID = NB_PayableRelatedID.Amount.ToString();
            string strPlanVerID = ShareClass.GetProjectPlanVerID(strProjectID, "InUse").ToString();

            TakeTopPlan.InitialProjectPlanTree(TreeView_PayablePJPlantTree, strProjectID, strPlanVerID);
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true','popPayableSelectPJPlanWindow') ", true);
    }

    protected void TreeView_ReceivablesPJPlantTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strPlanID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView_ReceivablesPJPlantTree.SelectedNode;


        try
        {
            strPlanID = treeNode.Target.Trim();

            NB_ReceivablesRelatedPlanID.Amount = int.Parse(strPlanID);
            LB_ReceivablesPlanName.Text = ShareClass.GetProjectPlanDetail(strPlanID);
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','false') ", true);
    }

    protected void TreeView_PayablePJPlantTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strPlanID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView_PayablePJPlantTree.SelectedNode;


        try
        {
            strPlanID = treeNode.Target.Trim();
            NB_PayableRelatedPlanID.Amount = int.Parse(strPlanID);

            LB_PayablePlanName.Text = ShareClass.GetProjectPlanDetail(strPlanID);
        }
        catch
        {
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
    }

    protected void BT_SelectPJBudget_Click(object sender, EventArgs e)
    {
        string strProjectID;

        strProjectID = NB_PayableRelatedID.Amount.ToString();
        LoadProjectBudget(strProjectID);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true','popRelatedPJBudgetWindow') ", true);
    }

    protected void BT_SelectPJConstractVisa_Click(object sender, EventArgs e)
    {
        string strProjectID, strConstractPayableID;

        strProjectID = NB_PayableRelatedID.Amount.ToString();
        strConstractPayableID = LB_PayableID.Text;

        SelectConstractPayableVisa(strProjectID);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true','popConstractPayableVisaWindow') ", true);
    }


    protected void SelectConstractPayableVisa(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayableVisa as constractPayableVisa where ProjectID = " + strProjectID;
        strHQL += " Order by constractPayableVisa.ID DESC";
        ConstractPayableVisaBLL constractPayableVisaBLL = new ConstractPayableVisaBLL();
        lst = constractPayableVisaBLL.GetAllConstractPayableVisas(strHQL);

        DataGrid17.DataSource = lst;
        DataGrid17.DataBind();
    }

    protected void LoadConstractPayableVisa(string strConstractPayableID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayableVisa as constractPayableVisa where ConstractPayableID = " + strConstractPayableID;
        strHQL += " Order by constractPayableVisa.ID DESC";
        ConstractPayableVisaBLL constractPayableVisaBLL = new ConstractPayableVisaBLL();
        lst = constractPayableVisaBLL.GetAllConstractPayableVisas(strHQL);

        RP_RelatedConstractPayableVisa.DataSource = lst;
        RP_RelatedConstractPayableVisa.DataBind();
    }


    protected void DataGrid16_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strAccount = e.Item.Cells[0].Text.Trim();
            string strUserCode = LB_UserCode.Text.Trim();

            for (int i = 0; i < DataGrid16.Items.Count; i++)
            {
                DataGrid16.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Select")
            {
                string strHQL;
                IList lst;
                string strRelatedJBBudegtID;

                strRelatedJBBudegtID = e.Item.Cells[4].Text.Trim();
                strHQL = "From ProjectBudget as projectBudget Where projectBudget.ID = " + strRelatedJBBudegtID;
                ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
                lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);

                ProjectBudget projectBudget = (ProjectBudget)lst[0];

                LB_RelatedPJBudgetID.Text = e.Item.Cells[4].Text.Trim();
                LB_RelatedPJBudgetAccountCode.Text = projectBudget.AccountCode;
                LB_RelatedPJBudgetAccount.Text = projectBudget.Account;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','false') ", true);
            }
        }
    }

    protected void LoadProjectBudget(string strProjectID)
    {
        string strHQL;
        IList lst;

        decimal deBudget = 0;

        strHQL = "From ProjectBudget as projectBudget Where projectBudget.ProjectID = " + strProjectID;
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        DataGrid16.DataSource = lst;
        DataGrid16.DataBind();

        ProjectBudget projectBudget = new ProjectBudget();

        for (int i = 0; i < lst.Count; i++)
        {
            projectBudget = (ProjectBudget)lst[i];
            deBudget += projectBudget.Amount;
        }

        LB_RealBudget.Text = deBudget.ToString();

        LB_ProExpense.Text = GetProjectExpense(strProjectID);

        LB_ProjectBudget.Text = ShareClass.GetProject(strProjectID).Budget.ToString();

        FinishPercentPicture(strProjectID);
    }


    protected void FinishPercentPicture(string strProjectID)
    {
        string strAccount;

        decimal deAccountExpense, deAccountBudget;
        decimal deWidth;

        int intWidth;
        int i;

        for (i = 0; i < DataGrid16.Items.Count; i++)
        {
            strAccount = DataGrid16.Items[i].Cells[0].Text.Trim();

            deAccountBudget = decimal.Parse(DataGrid16.Items[i].Cells[1].Text.Trim());
            deAccountExpense = GetProjectAccountTotalAmount(strProjectID, strAccount);

            if (deAccountBudget == 0)
            {
                deAccountBudget = 1;
            }

            deWidth = (deAccountExpense / deAccountBudget) * 100;
            deWidth = System.Decimal.Round(deWidth, 0);

            intWidth = int.Parse(deWidth.ToString());

            if (intWidth > 200)
            {
                intWidth = 200;
            }

            if (intWidth < deAccountExpense.ToString().Length)
            {
                if ((deAccountExpense.ToString().Length + 50) < 200)
                {
                    ((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_FinishPercent")).Width = (Unit)(deAccountExpense.ToString().Length + 50);
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_FinishPercent")).Width = (Unit)200;
                }
            }
            else
            {
                ((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_FinishPercent")).Width = (Unit)(intWidth + 50);
            }

            if (decimal.Parse(((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_FinishPercent")).Width.ToString().Replace("px", "")) > 200)
            {
                ((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_FinishPercent")).Width = (Unit)200;
            }

            ((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_FinishPercent")).Text = deAccountExpense.ToString();

            ((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_DefaultPercent")).Width = (Unit)110;

            ((System.Web.UI.WebControls.Label)DataGrid16.Items[i].FindControl("LB_DefaultPercent")).Text = deAccountBudget.ToString();
        }
    }

    protected decimal GetProjectAccountTotalAmount(string strProjectID, string strAccount)
    {
        string strHQL;

        strHQL = "Select Sum(ConfirmAmount) From T_ProExpense Where ProjectID = " + strProjectID + " and Account = " + "'" + strAccount + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");

        try
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        catch
        {
            return 0;
        }
    }

    protected string GetProjectExpense(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(ConfirmAmount),0) From T_ProExpense Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");


        try
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch
        {
            return "0";
        }
    }

    public string GetAddMoney(decimal decimalPayableAccount, decimal decimalPayOtherAccount, decimal decimalOtherAccount)
    {
        decimal decimalResult = decimalPayableAccount + decimalPayOtherAccount + decimalOtherAccount;
        return decimalResult.ToString("#0.00");
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        string strItemCode;
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        //先导入物料主数据
        if (ExcelToDataTrainingForItem() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("DaoRuShiBaiShuJuYouWuQingJianC");
            return;
        }

        //导入项目关联物料数据
        if (ExelToDBTestForProjectItem() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC");
            return;
        }
        else
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ");
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ");
                return;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB");
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
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ");
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strItemCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();

                        if (strItemCode != "")
                        {
                            ItemBLL itemBLL = new ItemBLL();
                            Item item = new Item();
                            string strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
                            IList lst = itemBLL.GetAllItems(strHQL);
                            if (lst != null && lst.Count > 0)//存在，则不操作
                            {
                                //判断物料是否在项目预算中已存在
                                if (CheckConstractRelatedGoodsIsExisted(strItemCode, strConstractCode))
                                {
                                    LB_ErrorText.Text += LanguageHandle.GetWord("CuoWuDaiMa") + strItemCode + LanguageHandle.GetWord("YiCunZaiBuNengChongFuTianJiaQi");
                                    continue;
                                }

                                try
                                {
                                    item = (Item)lst[0];

                                    ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
                                    ConstractRelatedGoods constractRelatedGoods = new ConstractRelatedGoods();

                                    constractRelatedGoods.ConstractCode = strConstractCode;

                                    constractRelatedGoods.FirstDirectory = dr[i][LanguageHandle.GetWord("YiJiMuLu")].ToString().Trim();
                                    constractRelatedGoods.SecondDirectory = dr[i][LanguageHandle.GetWord("ErJiMuLu")].ToString().Trim();
                                    constractRelatedGoods.ThirdDirectory = dr[i][LanguageHandle.GetWord("SanJiMuLu")].ToString().Trim();
                                    constractRelatedGoods.FourthDirectory = dr[i][LanguageHandle.GetWord("SiJiMuLu")].ToString().Trim();

                                    constractRelatedGoods.GoodsCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();
                                    constractRelatedGoods.GoodsName = dr[i][LanguageHandle.GetWord("MingChen")].ToString().Trim();

                                    constractRelatedGoods.Number = decimal.Parse(dr[i][LanguageHandle.GetWord("ShuLiang")].ToString().Trim());
                                    constractRelatedGoods.Unit = dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim();
                                    constractRelatedGoods.Price = decimal.Parse(dr[i][LanguageHandle.GetWord("ChanJia")].ToString().Trim());
                                    constractRelatedGoods.Amount = decimal.Parse(dr[i][LanguageHandle.GetWord("JinE")].ToString().Trim());

                                    constractRelatedGoods.Comment = dr[i][LanguageHandle.GetWord("BeiZhu")].ToString().Trim();

                                    constractRelatedGoods.ModelNumber = item.ModelNumber;
                                    constractRelatedGoods.Spec = item.Specification;
                                    constractRelatedGoods.Brand = item.Brand;
                                    constractRelatedGoods.Type = item.SmallType;

                                    constractRelatedGoods.SaleTime = DateTime.Now;
                                    constractRelatedGoods.DeleveryTime = DateTime.Now;

                                    constractRelatedGoodsBLL.AddConstractRelatedGoods(constractRelatedGoods);
                                }
                                catch (Exception err)
                                {
                                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGDRSBJC") + " : " + LanguageHandle.GetWord("HangHao") + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa") + ": " + strItemCode + " : " + err.Message.ToString() + "<br/>"; ;
                                    LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC") + " : " + LanguageHandle.GetWord("HangHao") + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa") + ": " + strItemCode + " : " + err.Message.ToString());
                                }
                            }

                            continue;
                        }
                    }

                    LoadConstractRelatedGoodsList(strConstractCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG") + "')", true);
                }
            }
        }
    }

    protected int ExelToDBTestForProjectItem()
    {
        int j = 0;

        string strItemCode;
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        try
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ");
                j = -1;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ");
                j = -1;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB");
                j = -1;
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
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ");
                    j = -1;
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strItemCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();

                        if (strItemCode != "")
                        {
                            ItemBLL itemBLL = new ItemBLL();
                            string strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
                            IList lst = itemBLL.GetAllItems(strHQL);
                            if (lst != null && lst.Count > 0)//存在，则不操作
                            {
                                CheckAndAddUnit(dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim());

                                try
                                {
                                    decimal.Parse(dr[i][LanguageHandle.GetWord("ShuLiang")].ToString().Trim());
                                    decimal.Parse(dr[i][LanguageHandle.GetWord("BaoLiuShuLiang")].ToString().Trim());
                                    decimal.Parse(dr[i][LanguageHandle.GetWord("YiCaiGouLiang")].ToString().Trim());
                                    decimal.Parse(dr[i][LanguageHandle.GetWord("YiChuKuLiang")].ToString().Trim());
                                }
                                catch
                                {
                                    j = -1;

                                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZZDRSBSLBLSLYWSZ");
                                    continue;
                                }

                            }
                            else//新增
                            {
                                j = -1;

                                LB_ErrorText.Text += strItemCode + LanguageHandle.GetWord("ZZZWLDMBCZQJC");
                                continue;
                            }

                        }
                    }
                }
            }
        }
        catch (Exception err)
        {
            LB_ErrorText.Text += err.Message.ToString() + "<br/>"; ;

            j = -1;
        }

        return j;
    }



    //----------------------------------------导入物料主文件--------------------------------------
    protected int ExcelToDataTrainingForItem()
    {
        string strBigType;
        string strItemCode, strDefaultProcess, strCurrencyType;
        decimal deSafetyStock, deLossRate;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        strDefaultProcess = "";
        strCurrencyType = "Renminbi";   

        int intWarrantyPeriod;

        if (ExelToDBTestForItem() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC");
            return -1;
        }
        else
        {
            deSafetyStock = 0;
            deLossRate = 0;

            intWarrantyPeriod = 0;

            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ");
                return -1;

            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ");
                return -1;

            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click444", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRSB") + "')", true);
                return -1;
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
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ");
                    return -1;
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strItemCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();

                        if (strItemCode != "")
                        {
                            ItemBLL itemBLL = new ItemBLL();
                            string strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
                            IList lst = itemBLL.GetAllItems(strHQL);
                            if (lst != null && lst.Count > 0)//存在，则不操作
                            {
                            }
                            else//新增
                            {
                                Item item = new Item();

                                item.ItemCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();
                                item.ItemName = dr[i][LanguageHandle.GetWord("MingChen")].ToString().Trim();
                                item.Type = dr[i][""+LanguageHandle.GetWord("ShuXing")+"（"+LanguageHandle.GetWord("CaiGouJian")+"、"+LanguageHandle.GetWord("ZiZhiJian")+"、"+LanguageHandle.GetWord("WaiXieJian")+"、"+LanguageHandle.GetWord("JiaoFuJian")+"）"].ToString().Trim();   
                                strBigType = dr[i][""+LanguageHandle.GetWord("DaLei")+"（"+LanguageHandle.GetWord("WuLiao")+"、"+LanguageHandle.GetWord("ZiChan")+"）"].ToString().Trim();   

                                item.BigType = "";
                                if (strBigType == LanguageHandle.GetWord("WuLiao"))
                                {
                                    item.BigType = "Goods";
                                }
                                if (strBigType == "Assets")
                                {
                                    item.BigType = "Asset";
                                }

                                item.SmallType = dr[i][""+LanguageHandle.GetWord("XiaoLei")+"（"+LanguageHandle.GetWord("WuLiaoHuoZiChanLeiXing")+"）"].ToString().Trim();   
                                item.Specification = dr[i][LanguageHandle.GetWord("GuiGe")].ToString().Trim();
                                item.ModelNumber = dr[i][LanguageHandle.GetWord("XingHao")].ToString().Trim();
                                item.Brand = dr[i][LanguageHandle.GetWord("PinPai")].ToString().Trim();
                                item.Unit = dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim();

                                item.RegistrationNumber = dr[i][LanguageHandle.GetWord("ZhuCeZhengHao")].ToString().Trim();
                                item.PackingType = dr[i]["PackagingMethod"].ToString().Trim();   

                                item.PULeadTime = 0;
                                item.MFLeadTime = 0;

                                item.PurchasePrice = decimal.Parse(dr[i][LanguageHandle.GetWord("ChanJia")].ToString().Trim());
                                item.SalePrice = decimal.Parse(dr[i][LanguageHandle.GetWord("ChanJia")].ToString().Trim());
                                item.CurrencyType = strCurrencyType;

                                item.HRCost = 0;
                                item.MFCost = 0;
                                item.MTCost = 0;
                                item.MFLeadTime = 0;

                                item.SafetyStock = deSafetyStock;
                                item.LossRate = deLossRate;

                                item.DefaultProcess = strDefaultProcess;
                                item.WarrantyPeriod = intWarrantyPeriod;

                                item.PhotoURL = "";


                                item.RelatedType = "SYSTEM";
                                item.RelatedID = 0;

                                itemBLL.AddItem(item);

                                strHQL = "Insert Into T_ItemBomVersion(ItemCode,VerID,Type,RelatedType,RelatedID) Values(" + "'" + strItemCode + "'" + ",1,'Standard','SYSTEM',0)";   
                                ShareClass.RunSqlCommand(strHQL);

                                //strHQL = "Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,Number,Unit,DefaultProcess,ChildItemVerID,VerID)";
                                //strHQL += " Values(" + "'" + strItemCode + "'" + "," + "'" + strItemCode + "'" + "," + "'" + strItemCode + "'" + ",1," + "'" + dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim() + "'" + "," + "'" + strDefaultProcess + "'" + ",1,1)";
                                //ShareClass.RunSqlCommand(strHQL);


                                strHQL = @"Insert Into T_ItemBom(ItemCode,ParentItemCode,ChildItemCode,ChildItemName,ChildItemType,ChildItemSpecification,
                                       ChildItemModelNumber,ChildItemBrand,Number,ReservedNumber,LossRate,Unit,ChildItemPhotoURL,DefaultProcess,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,
                                       PurchasePrice,SalePrice,ChildItemVerID,VerID,BelongItemCode,BelongVerID,KeyWord,ParentKeyWord)";
                                strHQL += " Values('" + strItemCode + "','" + strItemCode + "','" + strItemCode + "','" + item.ItemName.Trim() + "','" + item.SmallType.Trim() + "','" + item.Specification.Trim() + "','" + item.ModelNumber.Trim() + "','" + item.Brand.Trim() + "',1,0,0," + "'" + item.Unit.Trim() + "','" + item.PhotoURL.Trim() + "','" + strDefaultProcess + "'," + item.PULeadTime.ToString() + "," + item.MFLeadTime.ToString() + "," + item.HRCost.ToString() + "," + item.MTCost.ToString() + "," + item.MFCost.ToString() + "," + item.PurchasePrice.ToString() + "," + item.SalePrice.ToString() + ",1,1,'" + strItemCode + "',1,'" + strItemCode + "1" + strItemCode + "1" + "','" + strItemCode + "1" + strItemCode + "1" + "')";
                                ShareClass.RunSqlCommand(strHQL);

                            }
                            continue;
                        }
                    }

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG") + "')", true);
                }

                return 1;
            }
        }
    }

    protected int ExelToDBTestForItem()
    {
        int j = 0;

        string strItemCode;
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        if (FileUpload_Training.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ");
            j = -1;
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ");
            j = -1;
        }
        string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB");
            j = -1;
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
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ");
                j = -1;
            }
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    strItemCode = dr[i][LanguageHandle.GetWord("DaiMa")].ToString().Trim();
                    if (strItemCode != "")
                    {
                        CheckAndAddUnit(dr[i][LanguageHandle.GetWord("ChanWei")].ToString().Trim());

                        string strBigType = dr[i][""+LanguageHandle.GetWord("DaLei")+"（"+LanguageHandle.GetWord("WuLiao")+"、"+LanguageHandle.GetWord("ZiChan")+"）"].ToString().Trim();   
                        if (strBigType != LanguageHandle.GetWord("WuLiao") & strBigType != "Assets")
                        {
                            LB_ErrorText.Text += LanguageHandle.GetWord("DaoRuShiBaiDaLeiZhiNengSheWeiW");
                            j = -1;
                        }

                        string strSmallType = dr[i][""+LanguageHandle.GetWord("XiaoLei")+"（"+LanguageHandle.GetWord("WuLiaoHuoZiChanLeiXing")+"）"].ToString().Trim();   
                        if (CheckSmallType(strSmallType, strBigType) == 0)
                        {
                            if (strBigType != LanguageHandle.GetWord("WuLiao"))
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaoRuShiBaiXiaoLei") + strSmallType + LanguageHandle.GetWord("BuCunZaiQingXianZaiJiChuShuJuW");
                            }
                            else
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("DaoRuShiBaiXiaoLei") + strSmallType + LanguageHandle.GetWord("BuCunZaiQingXianZaiWuLiaoLeiXi");
                            }
                            j = -1;
                        }
                    }
                }
            }
        }

        return j;
    }

    protected int CheckSmallType(string strType, string strBigType)
    {
        string strHQL;

        if (strBigType == LanguageHandle.GetWord("WuLiao"))
        {
            strHQL = "Select Type From T_GoodsType Where Type = '" + strType + "'";
        }
        else
        {
            strHQL = "Select Type From T_AssetType Where Type = '" + strType + "'";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsType");

        return ds.Tables[0].Rows.Count;
    }

    protected void BT_DeleteAllProjectItem_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "Delete From T_ConstractRelatedGoods Where ConstractCode = '" + strConstractCode + "'";

        ShareClass.RunSqlCommand(strHQL);

        LoadConstractRelatedGoodsList(strConstractCode);
    }


    //判断物料是否在合同物料清单中已存在
    protected bool CheckConstractRelatedGoodsIsExisted(string strGoodsCode, string strConstractCode)
    {
        string strHQL;

        strHQL = "Select * From T_ConstractRelatedGoods Where GoodsCode = '" + strGoodsCode + "' and ConstractCode = '" + strConstractCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractRelatedGoods");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void CheckAndAddUnit(string strUnitName)
    {
        string strHQL;
        IList lst;

        strHQL = "from JNUnit as jnUnit Where jnUnit.UnitName = " + "'" + strUnitName + "'";
        JNUnitBLL jnUnitBLL = new JNUnitBLL();
        lst = jnUnitBLL.GetAllJNUnits(strHQL);

        JNUnit jnUnit = new JNUnit();

        if (lst.Count == 0)
        {
            jnUnit.UnitName = strUnitName;
            jnUnit.SortNumber = 100;

            jnUnitBLL.AddJNUnit(jnUnit);
        }
    }

    //按类型列出文档
    protected void LoadRelatedDocByDocType(string strConstractCode, string strDocType, DataGrid dataGrid)
    {
        string strHQL, strUserCode;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();

        strHQL = "from Document as document where ";
        strHQL += " (document.RelatedType = 'Contract' and document.RelatedID in (select constract.ConstractID from Constract as constract where constract.ConstractCode =" + "'" + strConstractCode + "'" + ")";   
        strHQL += " or document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From WorkFlow as workFlow Where workFlow.RelatedType = 'Contract' and workFlow.RelatedID in ( select constract.ConstractID from Constract as constract where constract.ConstractCode =" + "'" + strConstractCode + "'" + "))";   
        strHQL += " or document.RelatedType = 'Collaboration' and document.RelatedID in (Select collaboration.CoID From Collaboration as collaboration Where collaboration.RelatedType = 'CONSTRACT' and collaboration.RelatedCode =" + "'" + strConstractCode + "'" + "))";  
        strHQL += " and document.DocType = '" + strDocType + "'";

        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);


        dataGrid.DataSource = lst;
        dataGrid.DataBind();
    }

}
