using ProjectMgt.BLL;
using ProjectMgt.Model;

using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTConstractReceivablesRecord : System.Web.UI.Page
{
    string strReceivablesID, strConstractCode, strReceivablesCurrency, strRelatedProjectID;
    decimal deReceivablesAmount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strReceivablesID = Request.QueryString["ReceivablesID"];

        strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ID = " + strReceivablesID;
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ConstractReceivables constractReceivables = (ConstractReceivables)lst[0];

        strConstractCode = constractReceivables.ConstractCode.Trim();
        deReceivablesAmount = constractReceivables.ReceivablesAccount;
        strReceivablesCurrency = constractReceivables.CurrencyType.Trim();
        strRelatedProjectID = constractReceivables.RelatedProjectID.ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ReceiverTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadConstractReceivablesRecord(strReceivablesID);
            LoadConstractReceivablesRecordPayer(strUserCode);

            ShareClass.LoadCurrencyForDropDownList(DL_Currency);
            ShareClass.LoadReceivePayWayForDropDownList(DL_ReAndPayType);
            ShareClass.LoadBankForDropDownList(DL_Bank);

            CountAndUpdateReceivablesAmount(strReceivablesID);

            LB_ReceivablesOperatorCode.Text = strUserCode;
            LB_ReceivablesOperatorName.Text = strUserName;

            LB_ReceivablesCurrency.Text = strReceivablesCurrency;
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

    protected void DL_Payer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPayer = DL_Payer.SelectedValue.Trim();

        TB_Payer.Text = strPayer;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_ReceiverTaxRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DL_ReceiverTaxRate.SelectedValue == "0")
        {
            NB_ReceiverTaxRate.Amount = 0;
        }
        if (DL_ReceiverTaxRate.SelectedValue == "0.13")
        {
            NB_ReceiverTaxRate.Amount = decimal.Parse("0.13");
        }
        if (DL_ReceiverTaxRate.SelectedValue == "0.09")
        {
            NB_ReceiverTaxRate.Amount = decimal.Parse("0.09");
        }
        if (DL_ReceiverTaxRate.SelectedValue == "0.06")
        {
            NB_ReceiverTaxRate.Amount = decimal.Parse("0.06");
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ReceivablesRecordID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ReceivablesRecordID.Text.Trim();

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
        string strReceivableRecordID;
        string strOperatorCode, strOperatorName;
        string strRecorderCode, strRecorderName;
        string strReAndPayType, strCurrency, strBank;
        DateTime dtReceiverTime;
        decimal deReceiverAccount, DeInvoiceAccount;
        string strPayer, strReceiveComment;

        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();

        strReAndPayType = DL_ReAndPayType.SelectedValue.Trim();
        strCurrency = DL_Currency.SelectedValue.Trim();
        strBank = TB_Bank.Text.Trim();

        dtReceiverTime = DateTime.Parse(DLC_ReceiverTime.Text);
        deReceiverAccount = NB_ReceiverAccount.Amount;
        DeInvoiceAccount = NB_ReceivablesInvoiceAccount.Amount;

        strOperatorCode = LB_ReceivablesOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBDJRYDMCWCWCRJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }


        strPayer = TB_Payer.Text.Trim();
        strReceiveComment = TB_ReceiveComment.Text.Trim();

        ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
        ConstractReceivablesRecord constractReceivablesRecord = new ConstractReceivablesRecord();

        constractReceivablesRecord.ReceivablesID = int.Parse(strReceivablesID);
        constractReceivablesRecord.ConstractCode = strConstractCode;

        constractReceivablesRecord.ReAndPayType = strReAndPayType;
        constractReceivablesRecord.Currency = strCurrency;
        constractReceivablesRecord.Bank = strBank;
        constractReceivablesRecord.ExchangeRate = NB_ExchangeRate.Amount;

        constractReceivablesRecord.ReceiverAccount = deReceiverAccount;
        constractReceivablesRecord.ReceiverTime = dtReceiverTime;
        constractReceivablesRecord.InvoiceAccount = DeInvoiceAccount;

        constractReceivablesRecord.Payer = strPayer;
        constractReceivablesRecord.OperatorCode = strOperatorCode;
        constractReceivablesRecord.OperatorName = strOperatorName;
        constractReceivablesRecord.OperateTime = DateTime.Now;

        constractReceivablesRecord.RelatedProjectID = int.Parse(strRelatedProjectID);

        constractReceivablesRecord.Comment = strReceiveComment;

        try
        {
            constractReceivablesRecordBLL.AddConstractReceivablesRecord(constractReceivablesRecord);
            strReceivableRecordID = ShareClass.GetMyCreatedMaxConstractReceivableRecordID(strReceivablesID);
            LB_ReceivablesRecordID.Text = strReceivableRecordID;

            //保存其它附属属性
            SaveConstractReceivablesRecordOtherField(strReceivableRecordID);

            CountAndUpdateReceivablesAmount(strReceivablesID);
            LoadConstractReceivablesRecord(strReceivablesID);
            LoadConstractReceivables(strReceivablesID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    //保存其它附属属性
    protected void SaveConstractReceivablesRecordOtherField(string strReceivableRecordID)
    {
        string strHQL;

        strHQL = string.Format(@"Update t_constractReceivablesRecord Set TaxRate = {0} Where ID = {1}", NB_ReceiverTaxRate.Amount, strReceivableRecordID);
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateReceivables()
    {
        string strHQL;
        IList lst;

        string strID, strOperatorCode, strOperatorName;
        string strRecorderCode, strRecorderName;
        string strReAndPayType, strCurrency, strBank;
        DateTime dtReceiverTime;
        decimal deReceiverAccount, DeInvoiceAccount;
        string strPayer, strReceiveComment;

        strID = LB_ReceivablesRecordID.Text.Trim();

        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();

        strReAndPayType = DL_ReAndPayType.SelectedValue.Trim();
        strCurrency = DL_Currency.SelectedValue.Trim();
        strBank = TB_Bank.Text.Trim();

        dtReceiverTime = DateTime.Parse(DLC_ReceiverTime.Text);

        deReceiverAccount = NB_ReceiverAccount.Amount;
        DeInvoiceAccount = NB_ReceivablesInvoiceAccount.Amount;


        strOperatorCode = LB_ReceivablesOperatorCode.Text.Trim();
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


        strPayer = TB_Payer.Text.Trim();
        strReceiveComment = TB_ReceiveComment.Text.Trim();


        strHQL = "from  ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.ID= " + strID;
        ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
        lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);
        ConstractReceivablesRecord constractReceivablesRecord = (ConstractReceivablesRecord)lst[0];

        constractReceivablesRecord.ConstractCode = strConstractCode;

        constractReceivablesRecord.ReAndPayType = strReAndPayType;
        constractReceivablesRecord.Currency = strCurrency;
        constractReceivablesRecord.Bank = strBank;
        constractReceivablesRecord.ExchangeRate = NB_ExchangeRate.Amount;

        constractReceivablesRecord.ReceiverAccount = deReceiverAccount;
        constractReceivablesRecord.ReceiverTime = dtReceiverTime;
        constractReceivablesRecord.InvoiceAccount = DeInvoiceAccount;

        constractReceivablesRecord.Payer = strPayer;
        constractReceivablesRecord.OperatorCode = strOperatorCode;
        constractReceivablesRecord.OperatorName = strOperatorName;

        constractReceivablesRecord.RelatedProjectID = int.Parse(strRelatedProjectID);

        constractReceivablesRecord.Comment = strReceiveComment;

        try
        {
            constractReceivablesRecordBLL.UpdateConstractReceivablesRecord(constractReceivablesRecord, int.Parse(strID));

            //保存其它附属属性
            SaveConstractReceivablesRecordOtherField(strID);

            CountAndUpdateReceivablesAmount(strReceivablesID);
            LoadConstractReceivablesRecord(strReceivablesID);
            LoadConstractReceivables(strReceivablesID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
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
                strHQL = "from  ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.ID= " + strID;

                ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();

                IList lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                ConstractReceivablesRecord constractReceivablesRecord = (ConstractReceivablesRecord)lst[0];

                LB_ReceivablesRecordID.Text = constractReceivablesRecord.ID.ToString();
                DLC_ReceiverTime.Text = constractReceivablesRecord.ReceiverTime.ToString("yyyy-MM-dd");

                try
                {
                    DL_ReAndPayType.SelectedValue = constractReceivablesRecord.ReAndPayType.Trim();
                }
                catch
                {
                }

                DL_Currency.SelectedValue = constractReceivablesRecord.Currency.Trim();
                TB_Bank.Text = constractReceivablesRecord.Bank.Trim();
                NB_ExchangeRate.Amount = constractReceivablesRecord.ExchangeRate;

                NB_ReceiverAccount.Amount = constractReceivablesRecord.ReceiverAccount;
                NB_ReceivablesInvoiceAccount.Amount = constractReceivablesRecord.InvoiceAccount;
                LB_ReceivablesOperatorCode.Text = constractReceivablesRecord.OperatorCode.Trim();
                LB_ReceivablesOperatorName.Text = constractReceivablesRecord.OperatorName.Trim();

                TB_Payer.Text = constractReceivablesRecord.Payer.Trim();
                TB_ReceiveComment.Text = constractReceivablesRecord.Comment.Trim();

                LoadConstractReceivablesOtherField(strID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                IList lst;

                strHQL = "from  ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.ID= " + strID;
                ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
                lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);
                ConstractReceivablesRecord constractReceivablesRecord = (ConstractReceivablesRecord)lst[0];

                try
                {
                    constractReceivablesRecordBLL.DeleteConstractReceivablesRecord(constractReceivablesRecord);
                    LoadConstractReceivablesRecord(strReceivablesID);

                    CountAndUpdateReceivablesAmount(strReceivablesID);

                    LoadConstractReceivables(strReceivablesID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    //加载其它属性
    protected void LoadConstractReceivablesOtherField(string strConstractReceivablesRecordID)
    {
        string strHQL;

        strHQL = "Select * From T_ConstractReceivablesRecord Where ID = " + strConstractReceivablesRecordID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_constractReceivables");

        NB_ReceiverTaxRate.Amount = decimal.Parse(ds.Tables[0].Rows[0]["TaxRate"].ToString());
    }

    protected void CountAndUpdateReceivablesAmount(string strReceivablesID)
    {
        string strHQL;
        IList lst;

        decimal deReceiverAmount = 0, deInvoiceAccount = 0, deExchangeRate = 1;

        strHQL = "from ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.ReceivablesID = " + strReceivablesID;
        ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
        lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);

        ConstractReceivablesRecord constractReceivablesRecord = new ConstractReceivablesRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            constractReceivablesRecord = (ConstractReceivablesRecord)lst[i];

            deExchangeRate = constractReceivablesRecord.ExchangeRate;

            deReceiverAmount += constractReceivablesRecord.ReceiverAccount * deExchangeRate;
            deInvoiceAccount += constractReceivablesRecord.InvoiceAccount * deExchangeRate;
        }

        LB_ReceivablesAmount.Text = deReceivablesAmount.ToString("f2");
        LB_ReceiverAmount.Text = (deReceiverAmount / deExchangeRate).ToString("f2");
        LB_UNReceiverAmount.Text = (deReceivablesAmount - deReceiverAmount / deExchangeRate).ToString("f2");

        strHQL = "Update T_ConstractReceivables Set ReceiverAccount = " + (deReceiverAmount / deExchangeRate).ToString() + " where ID =" + strReceivablesID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractReceivables Set UNReceiveAmount = " + (deReceivablesAmount - deReceiverAmount / deExchangeRate).ToString() + " where ID =" + strReceivablesID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractReceivables Set InvoiceAccount = " + deInvoiceAccount.ToString() + " where ID =" + strReceivablesID;
        ShareClass.RunSqlCommand(strHQL);
    }

   

    protected void LoadConstractReceivables(string strReceivablesID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ID = " + strReceivablesID;
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadConstractReceivablesRecord(string strReceivablesID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.ReceivablesID = " + strReceivablesID;
        ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
        lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadConstractReceivablesRecordPayer(string strOperatorCode)
    {
        string strHQL;

        strHQL = "Select Distinct(Payer) from T_ConstractReceivablesRecord where ";
        strHQL += " OperatorCode = " + "'" + strOperatorCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");

        DL_Payer.DataSource = ds;
        DL_Payer.DataBind();

        DL_Payer.Items.Insert(0, new ListItem("--Select--", ""));
    }
}
