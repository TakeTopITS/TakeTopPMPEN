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

public partial class TTAccountPayableRecord : System.Web.UI.Page
{
    string strConstractCode="", strPayableID = "0";
    decimal dePayableAmount = 0;
    string strRelatedType;
    int intRelatedID = 0;
    string strPayAccount;

    string strUserCode, strUserName;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产登记入库", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_OutOfPocketTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadConstractPayableRecord(strUserCode);
            CountPayableAmount(strUserCode);


            ShareClass.LoadCurrencyForDropDownList(DL_Currency);
            ShareClass.LoadReceivePayWayForDropDownList(DL_ReAndPayType);
            ShareClass.LoadBankForDropDownList(DL_Bank);


            TB_PayableOperatorCode.Text = strUserCode;
            LB_PayableOperatorName.Text = strUserName;
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
        LB_PayableRecordID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_PayableRecordID.Text.Trim();

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
        string strPayableRecordID;
        string strOperatorCode, strOperatorName;
        string strRecorderCode, strRecorderName;

        string strReAndPayType, strCurrency, strBank;

        DateTime dtOutOfPocketTime;
        decimal deOutOfPocketAccount, DeInvoiceAccount;

        string strReceiver, strReceiveComment;


        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();

        strReAndPayType = DL_ReAndPayType.SelectedValue.Trim();
        strCurrency = DL_Currency.SelectedValue.Trim();
        strBank = TB_Bank.Text.Trim();

        dtOutOfPocketTime = DateTime.Parse(DLC_OutOfPocketTime.Text);
        deOutOfPocketAccount = NB_OutOfPocketAccount.Amount;
        DeInvoiceAccount = NB_PayableInvoiceAccount.Amount;

        strOperatorCode = TB_PayableOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBDJRYDMCWCWCRJC")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

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
        constractPayableRecord.ExchangeRate = NB_ExchangeRate.Amount;

        constractPayableRecord.OutOfPocketAccount = deOutOfPocketAccount;
        constractPayableRecord.OutOfPocketTime = dtOutOfPocketTime;
        constractPayableRecord.InvoiceAccount = DeInvoiceAccount;

        constractPayableRecord.Receiver = strReceiver;
        constractPayableRecord.OperatorCode = strOperatorCode;
        constractPayableRecord.OperatorName = strOperatorName;
        constractPayableRecord.OperateTime = DateTime.Now;

        constractPayableRecord.Comment = strReceiveComment;


        try
        {
            constractPayableRecordBLL.AddConstractPayableRecord(constractPayableRecord);
            strPayableRecordID = ShareClass.GetMyCreatedMaxConstractPayableRecordID(strPayableID);
            LB_PayableRecordID.Text = strPayableRecordID;
            

            LoadConstractPayableRecord(strUserCode);

            CountPayableAmount(strUserCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdatePayable()
    {
        string strHQL;
        IList lst;

        string strID, strOperatorCode, strOperatorName;
        string strRecorderCode, strRecorderName;

        string strReAndPayType, strCurrency, strBank;

        DateTime dtOutOfPocketTime;
        decimal deOutOfPocketAccount, DeInvoiceAccount;

        string strReceiver, strReceiveComment;


        strID = LB_PayableRecordID.Text.Trim();

        strRecorderCode = LB_UserCode.Text.Trim();
        strRecorderName = LB_UserName.Text.Trim();

        strReAndPayType = DL_ReAndPayType.SelectedValue.Trim();
        strCurrency = DL_Currency.SelectedValue.Trim();
        strBank = TB_Bank.Text.Trim();

        dtOutOfPocketTime = DateTime.Parse(DLC_OutOfPocketTime.Text);

        deOutOfPocketAccount = NB_OutOfPocketAccount.Amount;
        DeInvoiceAccount = NB_PayableInvoiceAccount.Amount;


        strOperatorCode = TB_PayableOperatorCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXGSBDJRYDMCWCWCRJC")+"')", true);

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
        constractPayableRecord.InvoiceAccount = DeInvoiceAccount;

        constractPayableRecord.Receiver = strReceiver;
        constractPayableRecord.OperatorCode = strOperatorCode;
        constractPayableRecord.OperatorName = strOperatorName;
        constractPayableRecord.OperateTime = DateTime.Now;

        constractPayableRecord.Comment = strReceiveComment;


        try
        {
            constractPayableRecordBLL.UpdateConstractPayableRecord(constractPayableRecord, int.Parse(strID));
            LoadConstractPayableRecord(strUserCode);

            CountPayableAmount(strUserCode);
           
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);
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


                DLC_OutOfPocketTime.Text = constractPayableRecord.OutOfPocketTime.ToString("yyyy-MM-dd");

                NB_OutOfPocketAccount.Amount = constractPayableRecord.OutOfPocketAccount;
                NB_PayableInvoiceAccount.Amount = constractPayableRecord.InvoiceAccount;

                try
                {
                    DL_ReAndPayType.SelectedValue = constractPayableRecord.ReAndPayType.Trim();
                }
                catch
                {
                }
                DL_Currency.SelectedValue = constractPayableRecord.Currency.Trim();
                NB_ExchangeRate.Amount = constractPayableRecord.ExchangeRate;

                TB_PayableOperatorCode.Text = constractPayableRecord.OperatorCode.Trim();
                LB_PayableOperatorName.Text = constractPayableRecord.OperatorName.Trim();

                TB_Receiver.Text = constractPayableRecord.Receiver.Trim();
                TB_ReceiveComment.Text = constractPayableRecord.Comment.Trim();
                

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
                    LoadConstractPayableRecord(strUserCode);

                    CountPayableAmount(strUserCode);
                    
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL ;
        IList lst;


        strHQL = "from ConstractPayableRecord as constractPayableRecord where constractPayableRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and constractPayableRecord.PayableID = 0 and constractPayableRecord.ConstractCode = ''";
        strHQL += " Order By constractPayableRecord.ID DESC";
        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void CountPayableAmount(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        decimal deOutOfPocketAmount = 0, deInvoiceAccount = 0, deExchangeRate = 1;

        strHQL = "from ConstractPayableRecord as constractPayableRecord where constractPayableRecord.OperatorCode = " + "'"+strOperatorCode+ "'";
        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);

        ConstractPayableRecord constractPayableRecord = new ConstractPayableRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            constractPayableRecord = (ConstractPayableRecord)lst[i];

            deExchangeRate = constractPayableRecord.ExchangeRate;

            deOutOfPocketAmount += constractPayableRecord.OutOfPocketAccount * deExchangeRate;
            deInvoiceAccount += constractPayableRecord.InvoiceAccount * deExchangeRate;
        }

        LB_PayableAmount.Text = dePayableAmount.ToString();
        LB_OutOfPocketAmount.Text = deOutOfPocketAmount.ToString();
        LB_UNOutOfPocketAmount.Text = (dePayableAmount - deOutOfPocketAmount).ToString();

        strHQL = "Update T_ConstractPayable Set OutOfPocketAccount = " + deOutOfPocketAmount.ToString() + " where ID =" + strPayableID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractPayable Set UNPayAmount = " + (dePayableAmount - deOutOfPocketAmount).ToString() + " where ID =" + strPayableID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractPayable Set InvoiceAccount = " + deInvoiceAccount.ToString() + " where ID =" + strPayableID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void LoadConstractPayableRecord(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayableRecord as constractPayableRecord where constractPayableRecord.OperatorCode = " + "'" + strOperatorCode + "'";
        strHQL += " and constractPayableRecord.PayableID = 0 and constractPayableRecord.ConstractCode = ''";
        strHQL += " Order By constractPayableRecord.ID DESC";
        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

}
