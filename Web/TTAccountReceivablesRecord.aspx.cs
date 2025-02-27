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


public partial class TTAccountReceivablesRecord : System.Web.UI.Page
{
    string strReceivablesID = "0", strConstractCode = "";
    decimal deReceivablesAmount = 0;
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_ReceiverTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadConstractReceivablesRecord(strUserCode);
            CountReceivablesAmount(strUserCode);

            ShareClass.LoadCurrencyForDropDownList(DL_Currency);
            
            ShareClass.LoadReceivePayWayForDropDownList(DL_ReAndPayType);
            ShareClass.LoadBankForDropDownList(DL_Bank);

            TB_ReceivablesOperatorCode.Text = strUserCode;
            LB_ReceivablesOperatorName.Text = strUserName;
        }
    }

    protected void DL_Bank_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strBankName;

        strBankName = DL_Bank.SelectedValue.Trim();

        TB_Bank.Text = strBankName;

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

        strOperatorCode = TB_ReceivablesOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBDJRYDMCWCWCRJC").ToString().Trim()+"')", true);

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

        constractReceivablesRecord.Comment = strReceiveComment;

        try
        {
            constractReceivablesRecordBLL.AddConstractReceivablesRecord(constractReceivablesRecord);
            strReceivableRecordID = ShareClass.GetMyCreatedMaxConstractReceivableRecordID(strReceivablesID);
            LB_ReceivablesRecordID.Text = strReceivableRecordID;

            LoadConstractReceivablesRecord(strOperatorCode);
            CountReceivablesAmount(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
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


        strOperatorCode = TB_ReceivablesOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXGSBDJRYDMCWCWCRJC").ToString().Trim()+"')", true);

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

        constractReceivablesRecord.Comment = strReceiveComment;


        try
        {
            constractReceivablesRecordBLL.UpdateConstractReceivablesRecord(constractReceivablesRecord, int.Parse(strID));

            LoadConstractReceivablesRecord(strOperatorCode);
            CountReceivablesAmount(strOperatorCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
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

                DLC_ReceiverTime.Text = constractReceivablesRecord.ReceiverTime.ToString("yyyy-MM-dd");

                NB_ReceiverAccount.Amount = constractReceivablesRecord.ReceiverAccount;
                NB_ReceivablesInvoiceAccount.Amount = constractReceivablesRecord.InvoiceAccount;
                TB_ReceivablesOperatorCode.Text = constractReceivablesRecord.OperatorCode.Trim();
                LB_ReceivablesOperatorName.Text = constractReceivablesRecord.OperatorName.Trim();

                TB_Payer.Text = constractReceivablesRecord.Payer.Trim();
                TB_ReceiveComment.Text = constractReceivablesRecord.Comment.Trim();


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                IList lst;

                strID = LB_ReceivablesRecordID.Text.Trim();
                strHQL = "from  ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.ID= " + strID;
                ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
                lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);
                ConstractReceivablesRecord constractReceivablesRecord = (ConstractReceivablesRecord)lst[0];

                try
                {
                    constractReceivablesRecordBLL.DeleteConstractReceivablesRecord(constractReceivablesRecord);

                    LoadConstractReceivablesRecord(strUserCode);
                    CountReceivablesAmount(strUserCode);
                    
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and constractReceivablesRecord.ReceivablesID = 0 and constractReceivablesRecord.ConstractCode = ''";
        strHQL += " Order By constractReceivablesRecord.ID DESC";
        ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
        lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void CountReceivablesAmount(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        decimal deReceiverAmount = 0, deInvoiceAccount = 0, deExchangeRate = 1;

        strHQL = "from ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.OperatorCode = " + "'" + strOperatorCode + "'";
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

        LB_ReceivablesAmount.Text = deReceivablesAmount.ToString();
        LB_ReceiverAmount.Text = deReceiverAmount.ToString();
        LB_UNReceiverAmount.Text = (deReceivablesAmount - deReceiverAmount).ToString();

        strHQL = "Update T_ConstractReceivables Set ReceiverAccount = " + deReceiverAmount.ToString() + " where ID =" + strReceivablesID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractReceivables Set UNReceiveAmount = " + (deReceivablesAmount - deReceiverAmount).ToString() + " where ID =" + strReceivablesID;
        ShareClass.RunSqlCommand(strHQL);


        strHQL = "Update T_ConstractReceivables Set InvoiceAccount = " + deInvoiceAccount.ToString() + " where ID =" + strReceivablesID;
        ShareClass.RunSqlCommand(strHQL);
    }


    protected void LoadConstractReceivablesRecord(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.OperatorCode = " + "'" + strOperatorCode + "'";
        strHQL += " and constractReceivablesRecord.ReceivablesID = 0 and constractReceivablesRecord.ConstractCode = ''";
        strHQL += " Order By constractReceivablesRecord.ID DESC";
        ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
        lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

}
