using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTAccountReceiptEvidence : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","财务收款入账", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        string IsFlag = GetIsFinancialIntervalName();
        if (IsFlag == "0")
        {
            Response.Redirect("TTAccountFinancialIntervalSet.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadAccountFinancialSet();
            ShareClass.LoadAccountForDDL(DL_AccountID);
            LoadConstractReceivablesRecordList(txt_ReceivInfo.Text.Trim(), strDepartString);
        }
    }

    /// <summary>
    /// 判断是否已选定财务帐套及区间
    /// </summary>
    /// <returns></returns>
    protected string GetIsFinancialIntervalName()
    {
        string flag1 = "0", flag2 = "0";
        string strHQL = "From AccountFinancialSet as accountFinancialSet Where accountFinancialSet.Status='OPEN' Order By accountFinancialSet.ID ASC ";
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst != null && lst.Count == 1)
        {
            AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];
            //lbl_FinancialID.Text = accountFinancialSet.ID.ToString();
            lbl_FinancialID.Text = accountFinancialSet.FinancialCode.Trim();
            lbl_FinancialName.Text = accountFinancialSet.FinancialName.Trim();
            lbl_CurrencyType.Text = accountFinancialSet.CurrencyType.Trim();
            lbl_ExchangeRate.Text = accountFinancialSet.ExchangeRate.ToString();

            flag1 = "1";
        }

        strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.Status='OPEN' Order By accountingIntervalSet.ID ASC ";
        AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
        lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
        if (lst != null && lst.Count == 1)
        {
            AccountingIntervalSet accountingIntervalSet = (AccountingIntervalSet)lst[0];
            //lbl_IntervalID.Text = accountingIntervalSet.ID.ToString();
            lbl_IntervalID.Text = accountingIntervalSet.IntervalCode.Trim();
            lbl_IntervalName.Text = accountingIntervalSet.IntervalName.Trim();

            flag2 = "1";
        }

        if (flag1 == "1" && flag2 == "1")
            return "1";
        else
            return "0";
    }

    protected void LoadAccountFinancialSet()
    {
        string strHQL = "From AccountFinancialSet as accountFinancialSet Order By accountFinancialSet.ID ASC ";
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        DL_FinancialID.DataSource = lst;
        DL_FinancialID.DataBind();
        DL_FinancialID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadConstractReceivablesRecordList(string strReceivInfo, string strDepartCode)
    {
        string strHQL = "Select A.* From T_ConstractReceivablesRecord A,T_ProjectMember B Where A.OperatorCode=B.UserCode ";
        strHQL += " and B.DepartCode in " + strDepartCode;
        if (!string.IsNullOrEmpty(strReceivInfo))
        {
            strHQL += " and (A.ConstractCode like '%" + strReceivInfo + "%' or A.ReceiverAccount like '%" + strReceivInfo + "%' or A.OperatorName like '%" + strReceivInfo + "%' " +
                "or A.Payer like '%" + strReceivInfo + "%' or A.Comment like '%" + strReceivInfo + "%' or A.OperatorCode like '%" + strReceivInfo + "%') ";
        }
        strHQL += " Order By A.ID DESC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadConstractReceivablesRecordList(txt_ReceivInfo.Text.Trim(), LB_DepartString.Text.Trim());
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            Button strID = (Button)e.Item.FindControl("BT_ID");

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            string strHQL = "Select * From T_ConstractReceivablesRecord Where ID='" + strID.Text + "' ";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                lbl_ReceivablesRecordID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                DL_AccountID.SelectedValue = GetReceiveAccountID(ds.Tables[0].Rows[0]["ID"].ToString());
                lbl_ReceiverAccount.Text = ds.Tables[0].Rows[0]["ReceiverAccount"].ToString();

                NB_TotalMoney.Amount = GetAccountValue(lbl_ReceivablesRecordID.Text.Trim(), decimal.Parse(lbl_ReceiverAccount.Text.Trim()), decimal.Parse(lbl_ExchangeRate.Text.Trim()));
                lbl_TotalMoney.Text = GetAccountValue(lbl_ReceivablesRecordID.Text.Trim(), decimal.Parse(lbl_ReceiverAccount.Text.Trim()), decimal.Parse(lbl_ExchangeRate.Text.Trim())).ToString();

                if (NB_TotalMoney.Amount > 0)
                {
                    BT_Add.Visible = true;
                    BT_Add.Enabled = true;
                }
                else
                {
                    BT_Add.Visible = false;
                }
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void DL_FinancialID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string FinancialID = DL_FinancialID.SelectedValue.Trim(); 
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        string strHQL = "from AccountFinancialSet as accountFinancialSet where accountFinancialSet.ID = '" + FinancialID + "' ";
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];
            lbl_CurrencyType.Text = accountFinancialSet.CurrencyType.Trim();

            NB_TotalMoney.Amount = GetAccountValue(lbl_ReceivablesRecordID.Text.Trim(), decimal.Parse(lbl_ReceiverAccount.Text.Trim()), accountFinancialSet.ExchangeRate);
            lbl_TotalMoney.Text = GetAccountValue(lbl_ReceivablesRecordID.Text.Trim(), decimal.Parse(lbl_ReceiverAccount.Text.Trim()), accountFinancialSet.ExchangeRate).ToString();

            if (NB_TotalMoney.Amount > 0)
            {
                BT_Add.Visible = true;
                BT_Add.Enabled = true;
            }
            else
            {
                BT_Add.Visible = false;
            }

            strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.FinancialID='" + accountFinancialSet.ID.ToString() + "' Order By accountingIntervalSet.ID ASC ";
            AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
            lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
            DL_IntervalID.DataSource = lst;
            DL_IntervalID.DataBind();
            DL_IntervalID.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        //if (DL_FinancialID.SelectedValue.Trim() == "0")
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGHJZTWBXJC")+"')", true);
        //    DL_FinancialID.Focus();
        //    return;
        //}
        //string FinancialStatus = GetFinancialStatus(DL_FinancialID.SelectedValue.Trim());
        //if (FinancialStatus != "OPEN")
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGHJZTYGBJC")+"')", true);
        //    DL_FinancialID.Focus();
        //    return;
        //}
        //if (DL_IntervalID.SelectedValue.Trim() == "0")
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGHJJWBXJC")+"')", true);
        //    DL_IntervalID.Focus();
        //    return;
        //}
        //string IntervalStatus = GetIntervalStatus(DL_IntervalID.SelectedValue.Trim());
        //if (IntervalStatus != "OPEN")
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGHJJYGBJC")+"')", true);
        //    DL_IntervalID.Focus();
        //    return;
        //}
        if (DL_AccountID.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGHJKMWBXJC")+"')", true);
            DL_AccountID.Focus();
            return;
        }
        if (NB_TotalMoney.Amount < 0 || NB_TotalMoney.Amount > decimal.Parse(lbl_TotalMoney.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGRZJEYWJC")+"')", true);
            NB_TotalMoney.Focus();
            return;
        }
        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        AccountGeneralLedger accountGeneralLedger = new AccountGeneralLedger();
        accountGeneralLedger.AccountCode = DL_AccountID.SelectedValue.Trim();
        accountGeneralLedger.AccountName = GetAccountName(DL_AccountID.SelectedValue.Trim());
        accountGeneralLedger.Creater = strUserCode.Trim();
        accountGeneralLedger.CreateTime = DateTime.Now;
        //accountGeneralLedger.FinancialID = int.Parse(DL_FinancialID.SelectedValue.Trim());
        //accountGeneralLedger.IntervalID = int.Parse(DL_IntervalID.SelectedValue.Trim());
        accountGeneralLedger.FinancialCode = lbl_FinancialID.Text.Trim();
        accountGeneralLedger.IntervalCode = lbl_IntervalID.Text.Trim();
        accountGeneralLedger.PayableRecordID = 0;
        accountGeneralLedger.ReceivablesRecordID = int.Parse(lbl_ReceivablesRecordID.Text.Trim());
        accountGeneralLedger.TotalMoney = NB_TotalMoney.Amount;
        try
        {
            accountGeneralLedgerBLL.AddAccountGeneralLedger(accountGeneralLedger);

            LoadConstractReceivablesRecordList(txt_ReceivInfo.Text.Trim(), LB_DepartString.Text.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRZCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRZSB")+"')", true);
        }
    }

    protected string GetAccountName(string strAccountID)
    {
        string flag = "";
        string strHQL = "Select AccountName From T_Account where AccountCode='" + strAccountID + "' ";
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

    protected decimal GetFinancialExchangeRate(string strFinancialCode)
    {
        decimal flag = 0;
        string strHQL = "Select ExchangeRate From T_AccountFinancialSet where FinancialCode='" + strFinancialCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = decimal.Parse(dt.Rows[0]["ExchangeRate"].ToString());
        }
        else
        {
            flag = 0;
        }
        return flag;
    }

    protected decimal GetAccountValue(string strID, decimal strReceiverAccount, decimal strExchangeRateGold)
    {
        decimal flag = 0;
        decimal strTotalMoney = 0;
        decimal strReceiverAccountGold = strReceiverAccount * GetReceiverCurrencyExchangeRate(strID) / strExchangeRateGold;
        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        string strHQL = "from AccountGeneralLedger as accountGeneralLedger where accountGeneralLedger.ReceivablesRecordID = '" + strID.Trim() + "' ";
        IList lst = accountGeneralLedgerBLL.GetAllAccountGeneralLedgers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                AccountGeneralLedger accountGeneralLedger = (AccountGeneralLedger)lst[i];
                decimal ExchangeRateOld = GetFinancialExchangeRate(accountGeneralLedger.FinancialCode.Trim());
                strTotalMoney += accountGeneralLedger.TotalMoney * ExchangeRateOld / strExchangeRateGold;
            }
        }
        flag = strReceiverAccountGold - strTotalMoney;
        return flag;
    }

    protected string GetReceiveAccountID(string strID)
    {
        string flag = "";
        string strHQL = "Select * From T_ConstractReceivablesRecord Where ID='" + strID + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string strReceivablesID = ds.Tables[0].Rows[0]["ReceivablesID"].ToString();
            strHQL = "Select * From T_ConstractReceivables Where ID='" + strReceivablesID + "' ";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivables");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                flag = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["AccountCode"].ToString()) ? "" : ds.Tables[0].Rows[0]["AccountCode"].ToString();
            }
        }
        return flag;
    }

    protected decimal GetReceiverCurrencyExchangeRate(string strID)
    {
        decimal flag = 0;
        string strHQL = "Select * From T_ConstractReceivablesRecord Where ID='" + strID + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string strReceivablesID = ds.Tables[0].Rows[0]["ReceivablesID"].ToString();
            strHQL = "Select * From T_ConstractReceivables Where ID='" + strReceivablesID + "' ";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivables");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                flag = decimal.Parse(ds.Tables[0].Rows[0]["ExchangeRate"].ToString());
            }
        }
        return flag;
    }

    protected string GetReceiverCurrency(string strID)
    {
        string flag = "";
        string strHQL = "Select * From T_ConstractReceivablesRecord Where ID='" + strID + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string strReceivablesID = ds.Tables[0].Rows[0]["ReceivablesID"].ToString();
            strHQL = "Select * From T_ConstractReceivables Where ID='" + strReceivablesID + "' ";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivables");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                flag = ds.Tables[0].Rows[0]["CurrencyType"].ToString();
            }
        }
        return flag;
    }

    protected string GetAccountGeneralLedgerStatus(string strID)
    {
        string flag = LanguageHandle.GetWord("WeiRuZhang");
        string strHQL = "Select * From T_AccountGeneralLedger Where ReceivablesRecordID='" + strID + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = LanguageHandle.GetWord("YiRuZhang");
        }
        return flag;
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            string strDepartCode = treeNode.Target.Trim();
            LB_DepartString.Text = "('" + strDepartCode + "')";
            //LB_DepartString.Text = GetDepartCodeList(strDepartCode);
            LoadConstractReceivablesRecordList(txt_ReceivInfo.Text.Trim(), LB_DepartString.Text.Trim());
        }

        
    }

    private bool GetCheckDataList(DataGrid dg, ref string ConstractReceivablesRecordId)
    {
        bool flag = false;
        StringBuilder sbId = new StringBuilder();
        for (int i = 0; i < dg.Items.Count; i++)
        {
            CheckBox cbSelect = (CheckBox)dg.Items[i].Cells[0].FindControl("CbSelect");
            if (cbSelect != null)
            {
                if (cbSelect.Checked)
                {
                    flag = true;
                    string _Id = string.Empty;
                    HiddenField hfId = (HiddenField)dg.Items[i].FindControl("hfID");

                    if (hfId != null)
                    {
                        _Id = hfId.Value;
                        sbId.AppendFormat("{0},", _Id);
                    }
                }
            }
        }
        ConstractReceivablesRecordId = sbId.ToString();
        return flag;
    }

    protected void Btn_AllAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        AccountGeneralLedger accountGeneralLedger = new AccountGeneralLedger();
        string constractReceivablesRecordId = string.Empty;
        flag = GetCheckDataList(DataGrid1, ref constractReceivablesRecordId);
        if (!flag)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCZX")+"')", true);
            return;
        }
        else
        {
            if (!string.IsNullOrEmpty(constractReceivablesRecordId))
            {
                string[] stempConRecRecId = constractReceivablesRecordId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < stempConRecRecId.Length; i++)
                {
                    string strHQL = "Select * From T_ConstractReceivablesRecord Where ID='" + stempConRecRecId[i].ToString() + "' ";
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivablesRecord");

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        decimal TotalMoney = GetAccountValue(ds.Tables[0].Rows[0]["ID"].ToString().Trim(), decimal.Parse(ds.Tables[0].Rows[0]["ReceiverAccount"].ToString().Trim()), decimal.Parse(lbl_ExchangeRate.Text.Trim()));

                        accountGeneralLedger.AccountCode = GetReceiveAccountID(ds.Tables[0].Rows[0]["ID"].ToString());
                        accountGeneralLedger.AccountName = GetAccountName(accountGeneralLedger.AccountCode.Trim());
                        accountGeneralLedger.Creater = strUserCode.Trim();
                        accountGeneralLedger.CreateTime = DateTime.Now;
                        accountGeneralLedger.FinancialCode = lbl_FinancialID.Text.Trim();
                        accountGeneralLedger.IntervalCode = lbl_IntervalID.Text.Trim();
                        accountGeneralLedger.PayableRecordID = 0;
                        accountGeneralLedger.ReceivablesRecordID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString().Trim());
                        accountGeneralLedger.TotalMoney = TotalMoney;

                        if (TotalMoney > 0)
                        {
                            accountGeneralLedgerBLL.AddAccountGeneralLedger(accountGeneralLedger);
                        }
                    }
                }

                LoadConstractReceivablesRecordList(txt_ReceivInfo.Text.Trim(), LB_DepartString.Text.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZPiLiangLanguageHandleGetWord")+"')", true); 
            }
        }
    }
}
