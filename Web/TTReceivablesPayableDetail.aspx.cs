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

public partial class TTReceivablesPayableDetail : System.Web.UI.Page
{
    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_PayableTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ReceivablesTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadConstractReceivables(strRelatedType, strRelatedID);
            LoadConstractPayable(strRelatedType, strRelatedID);

            CountReceivablesAmount(strRelatedType, strRelatedID);
            CountPayableAmount(strRelatedType, strRelatedID);

            ShareClass.LoadAccountForDDL(DL_ReceiveAccount);
            ShareClass.LoadAccountForDDL(DL_PayAccount);

            TB_ReceivablesOperatorCode.Text = strUserCode;
            LB_ReceivablesOperatorName.Text = strUserName;
            TB_PayableOperatorCode.Text = strUserCode;
            LB_PayableOperatorName.Text = strUserName;

            ShareClass.LoadCurrencyType(DL_ReceiveCurrencyType);
            ShareClass.LoadCurrencyType(DL_PayableCurrencyType);
        }
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

        strOperatorCode = TB_ReceivablesOperatorCode.Text.Trim();
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
        constractReceivables.CurrencyType = DL_ReceiveCurrencyType.SelectedValue;

        constractReceivables.Payer = strPayer;
        constractReceivables.OperatorCode = strOperatorCode;
        constractReceivables.OperatorName = strOperatorName;
        constractReceivables.OperateTime = DateTime.Now;
        constractReceivables.PreDays = int.Parse(dePreDays.ToString());
        constractReceivables.Status = strStatus;
        constractReceivables.Comment = strReceiveComment;

        constractReceivables.RelatedType = strRelatedType;
        constractReceivables.RelatedID = int.Parse(strRelatedID);
        constractReceivables.RelatedPlanID = 0;

        if (strRelatedType == "Project")
        {
            constractReceivables.RelatedProjectID = int.Parse(strRelatedID);
        }
        else
        {
            constractReceivables.RelatedProjectID = 1;
        }

        constractReceivables.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        constractReceivables.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractReceivables.CurrencyType.Trim());

        try
        {
            constractReceivablesBLL.AddConstractReceivables(constractReceivables);
            strReceivableID = ShareClass.GetMyCreatedMaxConstractReceivablePlanID(strRelatedType, strRelatedID);
            LB_ReceivablesID.Text = strReceivableID;


            LoadConstractReceivables(strRelatedType, strRelatedID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
        }
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


        strOperatorCode = TB_ReceivablesOperatorCode.Text.Trim();
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

        constractReceivables.CurrencyType = DL_ReceiveCurrencyType.SelectedValue;

        constractReceivables.Payer = strPayer;
        constractReceivables.OperatorCode = strOperatorCode;
        constractReceivables.OperatorName = strOperatorName;
        constractReceivables.PreDays = int.Parse(dePreDays.ToString());
        constractReceivables.Status = strStatus;
        constractReceivables.Comment = strReceiveComment;

        constractReceivables.RelatedType = strRelatedType;
        constractReceivables.RelatedID = int.Parse(strRelatedID);

        if (strRelatedType == "Project")
        {
            constractReceivables.RelatedProjectID = int.Parse(strRelatedID);
        }
        else
        {
            constractReceivables.RelatedProjectID = 1;
        }


        constractReceivables.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        constractReceivables.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractReceivables.CurrencyType.Trim());

        try
        {
            constractReceivablesBLL.UpdateConstractReceivables(constractReceivables, int.Parse(strID));
            LoadConstractReceivables(strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
        }
    }


    protected void BT_CreatePayable_Click(object sender, EventArgs e)
    {
        LB_PayableID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);
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


        strOperatorCode = TB_PayableOperatorCode.Text.Trim();
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

        constractPayable.CurrencyType = DL_PayableCurrencyType.SelectedValue;

        constractPayable.OperatorCode = strOperatorCode;
        constractPayable.OperatorName = strOperatorName;
        constractPayable.OperateTime = DateTime.Now;
        constractPayable.PreDays = int.Parse(dePreDays.ToString());
        constractPayable.Status = strStatus;
        constractPayable.Receiver = strReceiver;
        constractPayable.Comment = strComment;
        constractPayable.RelatedType = strRelatedType;
        constractPayable.RelatedID = int.Parse(strRelatedID);
        constractPayable.RelatedPlanID = 0;

        if (strRelatedType == "Project")
        {
            constractPayable.RelatedProjectID = int.Parse(strRelatedID);
        }
        else
        {
            constractPayable.RelatedProjectID = 1;
        }

        constractPayable.AccountCode = string.IsNullOrEmpty(lbl_AccountCode1.Text) ? "" : lbl_AccountCode1.Text.Trim();
        constractPayable.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractPayable.CurrencyType.Trim());

        try
        {
            constractPayableBLL.AddConstractPayable(constractPayable);

            strPayableID = ShareClass.GetMyCreatedMaxConstractPayablePlanID(strRelatedType, strRelatedID);
            LB_PayableID.Text = strPayableID;

            LoadConstractPayable(strRelatedType, strRelatedID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);
        }
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

        strID = LB_PayableID.Text.Trim();
        strConstractCode = LB_ConstractCode.Text.Trim();
        strBillCode = TB_PayableBillCode.Text.Trim();
        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();


        strPayAccount = TB_PayAccount.Text.Trim();

        dtPayableTime = DateTime.Parse(DLC_PayableTime.Text);

        dePayableAccount = NB_PayableAccount.Amount;


        strOperatorCode = TB_PayableOperatorCode.Text.Trim();
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

        constractPayable.CurrencyType = DL_PayableCurrencyType.SelectedValue;

        constractPayable.OperatorCode = strOperatorCode;
        constractPayable.OperatorName = strOperatorName;
        constractPayable.PreDays = int.Parse(dePreDays.ToString());
        constractPayable.Status = strStatus;
        constractPayable.Receiver = strReceiver;
        constractPayable.Comment = strComment;

        constractPayable.RelatedType = strRelatedType;
        constractPayable.RelatedID = int.Parse(strRelatedID);

        constractPayable.AccountCode = string.IsNullOrEmpty(lbl_AccountCode1.Text) ? "" : lbl_AccountCode1.Text.Trim();
        constractPayable.ExchangeRate = ShareClass.GetCurrencyTypeExchangeRate(constractPayable.CurrencyType.Trim());

        try
        {
            constractPayableBLL.UpdateConstractPayable(constractPayable, int.Parse(strID));
            LoadConstractPayable(strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPayableWindow','true') ", true);
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

                DL_ReceiveCurrencyType.SelectedValue = constractReceivables.CurrencyType;

                DLC_ReceivablesTime.Text = constractReceivables.ReceivablesTime.ToString("yyyy-MM-dd");

                TB_ReceivablesOperatorCode.Text = constractReceivables.OperatorCode.Trim();
                LB_ReceivablesOperatorName.Text = constractReceivables.OperatorName.Trim();
                NB_ReceivablesPreDays.Amount = constractReceivables.PreDays;
                DL_ReceivablesStatus.SelectedValue = constractReceivables.Status.Trim();
                TB_Payer.Text = constractReceivables.Payer.Trim();
                TB_ReceiveComment.Text = constractReceivables.Comment.Trim();

                lbl_AccountCode.Text = constractReceivables.AccountCode.Trim();

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
                    LoadConstractReceivables(strRelatedType, strRelatedID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
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

                DLC_PayableTime.Text = constractPayable.PayableTime.ToString("yyyy-MM-dd");

                DL_PayableCurrencyType.SelectedValue = constractPayable.CurrencyType;

                TB_PayableOperatorCode.Text = constractPayable.OperatorCode.Trim();
                LB_PayableOperatorName.Text = constractPayable.OperatorName.Trim();

                DL_PayableStatus.SelectedValue = constractPayable.Status.Trim();
                NB_PayablePreDays.Amount = constractPayable.PreDays;

                TB_Receiver.Text = constractPayable.Receiver.Trim();
                TB_PayableComment.Text = constractPayable.Comment.Trim();

                lbl_AccountCode1.Text = constractPayable.AccountCode.Trim();

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
                    LoadConstractPayable(strRelatedType, strRelatedID);

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


    protected void DL_ReceiveAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccount = DL_ReceiveAccount.SelectedValue.Trim();
        lbl_AccountCode.Text = strAccount;
        TB_ReceiveAccount.Text = GetAccountName(strAccount);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popReceivablesWindow','true') ", true);
    }

    protected void DL_Account_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccount = DL_PayAccount.SelectedValue.Trim();
        lbl_AccountCode1.Text = strAccount;
        TB_PayAccount.Text = GetAccountName(strAccount);

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

    protected void CountReceivablesAmount(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        decimal deReceivablesAmount = 0, deReceiverAmount = 0;

        strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.RelatedType = " + "'" + strRelatedType + "'" + " and constractReceivables.RelatedID = " + strRelatedID;
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

        ConstractReceivables constractReceivables = new ConstractReceivables();

        for (int i = 0; i < lst.Count; i++)
        {
            constractReceivables = (ConstractReceivables)lst[i];

            deReceivablesAmount += constractReceivables.ReceivablesAccount;
            deReceiverAmount += constractReceivables.ReceiverAccount;
        }

        LB_ReceivablesAmount.Text = deReceivablesAmount.ToString();
        LB_ReceiverAmount.Text = deReceiverAmount.ToString();
        LB_UNReceiverAmount.Text = (deReceivablesAmount - deReceiverAmount).ToString();
    }

    protected void CountPayableAmount(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        decimal dePayableAmount = 0, dePayAmount = 0;

        strHQL = "from ConstractPayable as constractPayable where constractPayable.RelatedType = " + "'" + strRelatedType + "'" + " and constractPayable.RelatedID = " + strRelatedID;
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);

        ConstractPayable constractPayable = new ConstractPayable();

        for (int i = 0; i < lst.Count; i++)
        {
            constractPayable = (ConstractPayable)lst[i];

            dePayableAmount += constractPayable.PayableAccount;
            dePayAmount += constractPayable.OutOfPocketAccount;
        }

        LB_PayableAmount.Text = dePayableAmount.ToString();
        LB_PayAmount.Text = dePayAmount.ToString();
        LB_UNPayAmount.Text = (dePayableAmount - dePayAmount).ToString();
    }

    protected void LoadConstractReceivables(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.RelatedType = " + "'" + strRelatedType + "'" + " and constractReceivables.RelatedID = " + strRelatedID;
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        CountReceivablesAmount(strRelatedType, strRelatedID);
    }

    protected void LoadConstractPayable(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayable as constractPayable where constractPayable.RelatedType = " + "'" + strRelatedType + "'" + " and constractPayable.RelatedID = " + strRelatedID;
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        CountPayableAmount(strRelatedType, strRelatedID);
    }

    protected void LoadCurrencyType()
    {
        string strHQL;
        IList lst;

        strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        DL_ReceiveCurrencyType.DataSource = lst;
        DL_ReceiveCurrencyType.DataBind();

        DL_PayableCurrencyType.DataSource = lst;
        DL_PayableCurrencyType.DataBind();
    }

}
