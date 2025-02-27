using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTConstractPayableRecord : System.Web.UI.Page
{
    string strPayableID, strConstractCode, strPayableCurrency;
    decimal dePayableAmount = 0;
    string strRelatedType;
    int intRelatedID = 0, intRelatedProjectID = 0;
    string strPayAccountCode, strPayAccount, strCurrencyType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strPayableID = Request.QueryString["PayableID"];

        strHQL = "from ConstractPayable as constractPayable where constractPayable.ID = " + strPayableID;
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ConstractPayable constractPayable = (ConstractPayable)lst[0];

        strConstractCode = constractPayable.ConstractCode.Trim();
        dePayableAmount = constractPayable.PayableAccount;
        strRelatedType = constractPayable.RelatedType.Trim();
        intRelatedID = constractPayable.RelatedID;
        strPayAccountCode = constractPayable.AccountCode.Trim();
        strPayAccount = constractPayable.Account.Trim();
        strPayableCurrency = constractPayable.CurrencyType.Trim();
        intRelatedProjectID = constractPayable.RelatedProjectID;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_OutOfPocketTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadConstractPayableRecord(strPayableID);
            LoadConstractPayableRecordReceiver(strUserCode);

            ShareClass.LoadCurrencyForDropDownList(DL_Currency);
            ShareClass.LoadReceivePayWayForDropDownList(DL_ReAndPayType);
            ShareClass.LoadBankForDropDownList(DL_Bank);

            CountAndUpdatePayableAmount(strPayableID);

            LB_PayableOperatorCode.Text = strUserCode;
            LB_PayableOperatorName.Text = strUserName;
        }
    }

    protected void DL_Currency_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCurrencyType = DL_Currency.SelectedValue.Trim();

        if (strCurrencyType != "")
        {
            NB_ExchangeRate.Amount = ShareClass.GetExchangeRateByCurrencyType(strCurrencyType);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_Bank_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strBankName;

        strBankName = DL_Bank.SelectedValue.Trim();

        TB_Bank.Text = strBankName;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_Receiver_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strReceiver = DL_Receiver.SelectedValue.Trim();

        TB_Receiver.Text = strReceiver;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Count_Click(object sender, EventArgs e)
    {
        NB_HomeCurrencyAmount.Amount = (NB_OutOfPocketAccount.Amount + NB_HandlingCharge.Amount) * NB_ExchangeRate.Amount;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_OutOfPocketTaxRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DL_OutOfPocketTaxRate.SelectedValue == "0")
        {
            NB_OutOfPocketTaxRate.Amount = 0;
        }
        if (DL_OutOfPocketTaxRate.SelectedValue == "0.13")
        {
            NB_OutOfPocketTaxRate.Amount = decimal.Parse("0.13");
        }
        if (DL_OutOfPocketTaxRate.SelectedValue == "0.09")
        {
            NB_OutOfPocketTaxRate.Amount = decimal.Parse("0.09");
        }
        if (DL_OutOfPocketTaxRate.SelectedValue == "0.06")
        {
            NB_OutOfPocketTaxRate.Amount = decimal.Parse("0.06");
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_PayableRecordID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_PayableRecordID.Text.Trim();

        if (strID == "")
        {
            AddPayableRecord();
        }
        else
        {
            UpdatePayableRecord();
        }
    }

    protected void AddPayableRecord()
    {
        string strPayableRecordID;
        string strOperatorCode, strOperatorName;
        string strRecorderCode, strRecorderName;
        string strReAndPayType, strCurrency, strBank;
        DateTime dtOutOfPocketTime;
        decimal deOutOfPocketAccount, DeInvoiceAccount, deExchangeRate;
        decimal deHandlingCharge, deHomeCurrencyAmount;
        string strReceiver, strReceiveComment;

        NB_HomeCurrencyAmount.Amount = (NB_OutOfPocketAccount.Amount + NB_HandlingCharge.Amount) * NB_ExchangeRate.Amount;

        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();

        strReAndPayType = DL_ReAndPayType.SelectedValue.Trim();
        strCurrency = DL_Currency.SelectedValue.Trim();
        strBank = TB_Bank.Text.Trim();
        deExchangeRate = NB_ExchangeRate.Amount;

        dtOutOfPocketTime = DateTime.Parse(DLC_OutOfPocketTime.Text);
        deOutOfPocketAccount = NB_OutOfPocketAccount.Amount;
        deHomeCurrencyAmount = NB_HomeCurrencyAmount.Amount;
        deHandlingCharge = NB_HandlingCharge.Amount;
        DeInvoiceAccount = NB_PayableInvoiceAccount.Amount;
        strOperatorCode = LB_PayableOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBDJRYDMCWCWCRJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return;
        }


        strReceiver = TB_Receiver.Text.Trim();
        strReceiveComment = TB_ReceiveComment.Text.Trim();

        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        ConstractPayableRecord constractPayableRecord = new ConstractPayableRecord();

        constractPayableRecord.PayableID = int.Parse(strPayableID);
        constractPayableRecord.ConstractCode = strConstractCode;
        constractPayableRecord.ReAndPayType = strReAndPayType;
        constractPayableRecord.Currency = strCurrency;
        constractPayableRecord.Bank = strBank;
        constractPayableRecord.OutOfPocketAccount = deOutOfPocketAccount;
        constractPayableRecord.OutOfPocketTime = dtOutOfPocketTime;
        constractPayableRecord.HomeCurrencyAmount = deHomeCurrencyAmount;
        constractPayableRecord.HandlingCharge = deHandlingCharge;
        constractPayableRecord.InvoiceAccount = DeInvoiceAccount;
        constractPayableRecord.ExchangeRate = NB_ExchangeRate.Amount;
        constractPayableRecord.Receiver = strReceiver;
        constractPayableRecord.OperatorCode = strOperatorCode;
        constractPayableRecord.OperatorName = strOperatorName;
        constractPayableRecord.OperateTime = DateTime.Now;

        constractPayableRecord.RelatedProjectID = intRelatedProjectID;

        constractPayableRecord.Comment = strReceiveComment;

        try
        {
            constractPayableRecordBLL.AddConstractPayableRecord(constractPayableRecord);
            strPayableRecordID = ShareClass.GetMyCreatedMaxConstractPayableRecordID(strPayableID);
            LB_PayableRecordID.Text = strPayableRecordID;

            //保存其它附属属性
            SaveConstractPayableRecordOtherField(strPayableRecordID);

            LoadConstractPayableRecord(strPayableID);

            CountAndUpdatePayableAmount(strPayableID);


            if (strRelatedType == "Project" & intRelatedID > 0)
            {
                ShareClass.AddConstractPayAmountToProExpense(intRelatedID.ToString(), strPayableID, strPayAccountCode, strPayAccount, LanguageHandle.GetWord("GeTongFuKuan").ToString().Trim(), deOutOfPocketAccount + deHandlingCharge, strCurrency, strRecorderCode, strRecorderName);
            }

            //关联商品采购单的项目的费用处理
            if (strRelatedType == "GoodsPO" & intRelatedID > 0)
            {
                string strRelatedFormType;
                int intRelatedFormID;
                string strHQL = "Select RelatedType,RelatedID From T_GoodsPurchaseOrder Where POID = " + intRelatedID.ToString();
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurchaseOrder");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRelatedFormType = ds.Tables[0].Rows[0][0].ToString().Trim();
                    intRelatedFormID = int.Parse(ds.Tables[0].Rows[0][1].ToString());

                    if (strRelatedFormType == "Project" & intRelatedFormID > 0)
                    {
                        ShareClass.AddConstractPayAmountToProExpense(intRelatedFormID.ToString(), strPayableID, strPayAccountCode, strPayAccount, LanguageHandle.GetWord("WuLiaoCaiGouFuKuan").ToString().Trim(), deOutOfPocketAccount + deHandlingCharge, strCurrency, strRecorderCode, strRecorderName);
                    }
                }
            }

            //关联资产采购单的项目的费用处理
            if (strRelatedType == "AssetPO" & intRelatedID > 0)
            {
                string strRelatedFormType;
                int intRelatedFormID;
                string strHQL = "Select RelatedType,RelatedID From T_AssetPurchaseOrder Where POID = " + intRelatedID.ToString();
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurchaseOrder");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRelatedFormType = ds.Tables[0].Rows[0][0].ToString().Trim();
                    intRelatedFormID = int.Parse(ds.Tables[0].Rows[0][1].ToString());

                    if (strRelatedFormType == "Project" & intRelatedFormID > 0)
                    {
                        ShareClass.AddConstractPayAmountToProExpense(intRelatedFormID.ToString(), strPayableID, strPayAccountCode, strPayAccount, LanguageHandle.GetWord("ZiChanCaiGouFuKuan").ToString().Trim(), deOutOfPocketAccount + deHandlingCharge, strCurrency, strRecorderCode, strRecorderName);
                    }
                }
            }

            //刷新应付款信息
            LoadConstractPayable(strPayableID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    //保存其它附属属性
    protected void SaveConstractPayableRecordOtherField(string strtPayableRecordID)
    {
        string strHQL;

        strHQL = string.Format(@"Update t_constractPayableRecord Set TaxRate = {0} Where ID = {1}",NB_OutOfPocketTaxRate.Amount, strtPayableRecordID);
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdatePayableRecord()
    {
        string strHQL;
        IList lst;

        string strID, strOperatorCode, strOperatorName;
        string strRecorderCode, strRecorderName;
        string strReAndPayType, strCurrency, strBank;
        DateTime dtOutOfPocketTime;
        decimal deOutOfPocketAccount, DeInvoiceAccount, deExchangeRate;
        decimal deHandlingCharge, deHomeCurrencyAmount;
        string strReceiver, strReceiveComment;
        
        NB_HomeCurrencyAmount.Amount = (NB_OutOfPocketAccount.Amount + NB_HandlingCharge.Amount) * NB_ExchangeRate.Amount;

        strID = LB_PayableRecordID.Text.Trim();

        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();

        strReAndPayType = DL_ReAndPayType.SelectedValue.Trim();
        strCurrency = DL_Currency.SelectedValue.Trim();
        strBank = TB_Bank.Text.Trim();

        DeInvoiceAccount = NB_PayableInvoiceAccount.Amount;
        deExchangeRate = NB_ExchangeRate.Amount;

        deOutOfPocketAccount = NB_OutOfPocketAccount.Amount;
        dtOutOfPocketTime = DateTime.Parse(DLC_OutOfPocketTime.Text);

        deHomeCurrencyAmount = NB_HomeCurrencyAmount.Amount;

        deHomeCurrencyAmount = NB_HomeCurrencyAmount.Amount;
        deHandlingCharge = NB_HandlingCharge.Amount;

        strOperatorCode = LB_PayableOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBDJRYDMCWCWCRJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }


        strReceiver = TB_Receiver.Text.Trim();
        strReceiveComment = TB_ReceiveComment.Text.Trim();

        strHQL = "from  ConstractPayableRecord as constractPayableRecord where constractPayableRecord.ID= " + strID;
        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);
        ConstractPayableRecord constractPayableRecord = (ConstractPayableRecord)lst[0];

        constractPayableRecord.ConstractCode = strConstractCode;

        constractPayableRecord.ReAndPayType = strReAndPayType;
        constractPayableRecord.Currency = strCurrency;
        constractPayableRecord.Bank = strBank;
        constractPayableRecord.ExchangeRate = NB_ExchangeRate.Amount;

        constractPayableRecord.OutOfPocketAccount = deOutOfPocketAccount;
        constractPayableRecord.OutOfPocketTime = dtOutOfPocketTime;

        constractPayableRecord.HomeCurrencyAmount = deHomeCurrencyAmount;
        constractPayableRecord.HandlingCharge = deHandlingCharge;

        constractPayableRecord.InvoiceAccount = DeInvoiceAccount;

        constractPayableRecord.Receiver = strReceiver;
        constractPayableRecord.OperatorCode = strOperatorCode;
        constractPayableRecord.OperatorName = strOperatorName;
        constractPayableRecord.OperateTime = DateTime.Now;

        constractPayableRecord.RelatedProjectID = intRelatedProjectID;

        constractPayableRecord.Comment = strReceiveComment;

        try
        {
            constractPayableRecordBLL.UpdateConstractPayableRecord(constractPayableRecord, int.Parse(strID));
            LoadConstractPayableRecord(strPayableID);

            //保存其它附属属性
            SaveConstractPayableRecordOtherField(strID);

            //计算和更新应付表数据
            CountAndUpdatePayableAmount(strPayableID);

            if (strRelatedType == "Project" & intRelatedID > 0)
            {
                ShareClass.UpdateConstractPayAmountToProExpense(intRelatedID.ToString(), strID, strPayAccountCode, strPayAccount, LanguageHandle.GetWord("GeTongFuKuan").ToString().Trim(), deOutOfPocketAccount + deHandlingCharge, strCurrency, strRecorderCode, strRecorderName);
            }

            //关联采购单的项目的费用处理
            if (strRelatedType == "GoodsPO" & intRelatedID > 0)
            {
                string strRelatedFormType;
                int intRelatedFormID;
                strHQL = "Select RelatedType,RelatedID From T_GoodsPurchaseOrder Where POID = " + intRelatedID.ToString();
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurchaseOrder");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRelatedFormType = ds.Tables[0].Rows[0][0].ToString().Trim();
                    intRelatedFormID = int.Parse(ds.Tables[0].Rows[0][1].ToString());

                    if (strRelatedFormType == "Project" & intRelatedFormID > 0)
                    {
                        ShareClass.UpdateConstractPayAmountToProExpense(intRelatedFormID.ToString(), strID, strPayAccountCode, strPayAccount, LanguageHandle.GetWord("WuLiaoCaiGouFuKuan").ToString().Trim(), deOutOfPocketAccount + deHandlingCharge, strCurrency, strRecorderCode, strRecorderName);
                    }
                }
            }

            //关联资产采购单的项目的费用处理
            if (strRelatedType == "AssetPO" & intRelatedID > 0)
            {
                string strRelatedFormType;
                int intRelatedFormID;
                strHQL = "Select RelatedType,RelatedID From T_AssetPurchaseOrder Where POID = " + intRelatedID.ToString();
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurchaseOrder");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRelatedFormType = ds.Tables[0].Rows[0][0].ToString().Trim();
                    intRelatedFormID = int.Parse(ds.Tables[0].Rows[0][1].ToString());

                    if (strRelatedFormType == "Project" & intRelatedFormID > 0)
                    {
                        ShareClass.UpdateConstractPayAmountToProExpense(intRelatedFormID.ToString(), strPayableID, strPayAccountCode, strPayAccount, LanguageHandle.GetWord("ZiChanCaiGouFuKuan").ToString().Trim(), deOutOfPocketAccount + deHandlingCharge, strCurrency, strRecorderCode, strRecorderName);
                    }
                }
            }

            //刷新应付款信息
            LoadConstractPayable(strPayableID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    //列出应付款
    protected void LoadConstractPayable(string strPayableID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayable as constractPayable where constractPayable.ID = " + strPayableID;
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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
                strHQL = "from  ConstractPayableRecord as constractPayableRecord where constractPayableRecord.ID= " + strID;

                ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();

                IList lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                ConstractPayableRecord constractPayableRecord = (ConstractPayableRecord)lst[0];

                LB_PayableRecordID.Text = constractPayableRecord.ID.ToString();

                try
                {
                    DL_ReAndPayType.SelectedValue = constractPayableRecord.ReAndPayType.Trim();
                }
                catch
                {
                }

                DL_Currency.SelectedValue = constractPayableRecord.Currency.Trim();
                TB_Bank.Text = constractPayableRecord.Bank.Trim();
                NB_ExchangeRate.Amount = constractPayableRecord.ExchangeRate;

                DLC_OutOfPocketTime.Text = constractPayableRecord.OutOfPocketTime.ToString("yyyy-MM-dd");

                NB_OutOfPocketAccount.Amount = constractPayableRecord.OutOfPocketAccount;
                NB_PayableInvoiceAccount.Amount = constractPayableRecord.InvoiceAccount;

                NB_HomeCurrencyAmount.Amount = constractPayableRecord.HomeCurrencyAmount;
                NB_HandlingCharge.Amount = constractPayableRecord.HandlingCharge;

                LB_PayableOperatorCode.Text = constractPayableRecord.OperatorCode.Trim();
                LB_PayableOperatorName.Text = constractPayableRecord.OperatorName.Trim();

                TB_Receiver.Text = constractPayableRecord.Receiver.Trim();
                TB_ReceiveComment.Text = constractPayableRecord.Comment.Trim();

                LoadConstractPayableRecordOtherField(strID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                IList lst;

                strHQL = "from  ConstractPayableRecord as constractPayableRecord where constractPayableRecord.ID= " + strID;
                ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
                lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);
                ConstractPayableRecord constractPayableRecord = (ConstractPayableRecord)lst[0];

                try
                {
                    constractPayableRecordBLL.DeleteConstractPayableRecord(constractPayableRecord);
                    LoadConstractPayableRecord(strPayableID);

                    CountAndUpdatePayableAmount(strPayableID);

                    if (strRelatedType == "Project" & intRelatedID > 0)
                    {
                        strHQL = "Delete From T_ProExpense Where ConstractPayID = " + strID;
                        ShareClass.RunSqlCommand(strHQL);
                    }

                    //关联采购单的项目的费用处理
                    if (strRelatedType == "GoodsPO" & intRelatedID > 0)
                    {
                        string strRelatedFormType;
                        int intRelatedFormID;
                        strHQL = "Select RelatedType,RelatedID From T_GoodsPurchaseOrder Where POID = " + intRelatedID.ToString();
                        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurchaseOrder");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strRelatedFormType = ds.Tables[0].Rows[0][0].ToString().Trim();
                            intRelatedFormID = int.Parse(ds.Tables[0].Rows[0][1].ToString());

                            if (strRelatedFormType == "Project" & intRelatedFormID > 0)
                            {
                                strHQL = "Delete From T_ProExpense Where ConstractPayID = " + strID;
                                ShareClass.RunSqlCommand(strHQL);
                            }
                        }
                    }


                    //关联资产采购单的项目的费用处理
                    if (strRelatedType == "AssetPO" & intRelatedID > 0)
                    {
                        string strRelatedFormType;
                        int intRelatedFormID;
                        strHQL = "Select RelatedType,RelatedID From T_AssetPurchaseOrder Where POID = " + intRelatedID.ToString();
                        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurchaseOrder");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strRelatedFormType = ds.Tables[0].Rows[0][0].ToString().Trim();
                            intRelatedFormID = int.Parse(ds.Tables[0].Rows[0][1].ToString());

                            if (strRelatedFormType == "Project" & intRelatedFormID > 0)
                            {
                                strHQL = "Delete From T_ProExpense Where ConstractPayID = " + strID;
                                ShareClass.RunSqlCommand(strHQL);
                            }
                        }
                    }

                    //刷新应付款信息
                    LoadConstractPayable(strPayableID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    //加载其它属性
    protected void LoadConstractPayableRecordOtherField(string strConstractPayableRecordID)
    {
        string strHQL;

        strHQL = "Select * From T_ConstractPayableRecord Where ID = " + strConstractPayableRecordID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_constractReceivables");

        NB_OutOfPocketTaxRate.Amount = decimal.Parse(ds.Tables[0].Rows[0]["TaxRate"].ToString());
    }

    protected void CountAndUpdatePayableAmount(string strPayableID)
    {
        string strHQL;
        IList lst;

        decimal deHomeOutOfPocketAmount = 0, deInvoiceAccount = 0, deExchangeRate = 1;

        strHQL = "from ConstractPayableRecord as constractPayableRecord where constractPayableRecord.PayableID = " + strPayableID;
        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);

        ConstractPayableRecord constractPayableRecord = new ConstractPayableRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            constractPayableRecord = (ConstractPayableRecord)lst[i];

            deExchangeRate = constractPayableRecord.ExchangeRate;
            deHomeOutOfPocketAmount += constractPayableRecord.HomeCurrencyAmount;
            deInvoiceAccount += constractPayableRecord.InvoiceAccount * deExchangeRate;
        }

        LB_PayableAmount.Text = dePayableAmount.ToString();
        LB_OutOfPocketAmount.Text = (deHomeOutOfPocketAmount / deExchangeRate).ToString("f2");
        LB_UNOutOfPocketAmount.Text = (dePayableAmount - deHomeOutOfPocketAmount / deExchangeRate).ToString("f2");

        strHQL = "Update T_ConstractPayable Set OutOfPocketAccount = " + LB_OutOfPocketAmount.Text + " where ID =" + strPayableID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractPayable Set UNPayAmount = " + (dePayableAmount - deHomeOutOfPocketAmount / deExchangeRate).ToString() + " where ID =" + strPayableID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractPayable Set InvoiceAccount = " + (deInvoiceAccount / deExchangeRate).ToString() + " where ID =" + strPayableID;
        ShareClass.RunSqlCommand(strHQL);
    }


    protected void LoadConstractPayableRecord(string strPayableID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayableRecord as constractPayableRecord where constractPayableRecord.PayableID = " + strPayableID;
        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadConstractPayableRecordReceiver(string strOperatorCode)
    {
        string strHQL;

        strHQL = "Select Distinct(Receiver) from T_ConstractPayableRecord where ";
        strHQL += " OperatorCode = " + "'" + strOperatorCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractPayableRecord");

        DL_Receiver.DataSource = ds;
        DL_Receiver.DataBind();

        DL_Receiver.Items.Insert(0, new ListItem("--Select--", ""));
    }

   
}
