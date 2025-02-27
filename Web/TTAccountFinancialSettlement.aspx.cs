using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTAccountFinancialSettlement : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","财务期末转账", strUserCode);
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
            LoadAccountFinancialSet();
            LoadAccountFinancialSet1();
            LoadAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim());
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
        string strHQL = "From AccountFinancialSet as accountFinancialSet Where accountFinancialSet.Status='OPEN' Order By accountFinancialSet.ID ASC ";
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        DL_FinancialID.DataSource = lst;
        DL_FinancialID.DataBind();
        DL_FinancialID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadAccountFinancialSet1()
    {
        string strDepartString = GetUserDepartCode(strUserCode.Trim());
        string strSuperDepartCode = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode.Trim());
        //string strHQL = "From AccountFinancialSet as accountFinancialSet Order By accountFinancialSet.ID ASC "; 
        string strHQL = "Select * From T_AccountFinancialSet Where (DepartCode in " + strDepartString + " OR DepartCode in " + strSuperDepartCode + ") ";
        strHQL += " Order By ID ASC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet");

        //AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        //IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        //DL_Financial.DataSource = lst;
        DL_Financial.DataSource = ds;
        DL_Financial.DataBind();
        DL_Financial.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected string GetUserDepartCode(string strusercode)
    {
        string strDepartString = "";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        string strHQL = "From ProjectMember as projectMember where projectMember.UserCode='" + strusercode + "' ";
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            if (!string.IsNullOrEmpty(projectMember.DepartCode) && projectMember.DepartCode.Trim() != "")
            {
                strDepartString += "('" + projectMember.DepartCode.Trim() + "',";
                strDepartString += "" + GetMaxHiDepartCode(projectMember.DepartCode.Trim()) + ")";
            }
        }

        if (strDepartString.Length > 0)
        {
            if (strDepartString.Contains(",)"))
            {
                strDepartString = strDepartString.Replace(",)", ")");
            }
        }

        return strDepartString;
    }

    protected string GetMaxHiDepartCode(string strdepartcode)
    {
        string strdepart = "";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        string strHQL = "from Department as department where department.ParentCode in ('" + strdepartcode + "') ";
        IList lst = departmentBLL.GetAllDepartments(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                Department department = (Department)lst[i];
                strdepart += "'" + department.DepartCode.Trim() + "'" + ",";
                GetMaxHiDepartCode(department.DepartCode.Trim());
            }
        }

        if (strdepart.Length > 0)
            return strdepart.Substring(0, strdepart.Length - 1);
        else
            return strdepart;
    }

    protected void LoadAccountGeneralLedgerList(string strFinancialID, string strIntervalID)
    {
        string strHQL = "Select FinancialCode,IntervalCode,AccountCode,AccountName,SUM(TotalMoney) TotalAllMoney from T_AccountGeneralLedger Where 1=1 ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and FinancialCode = '" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and IntervalCode = '" + strIntervalID + "' ";
        }
        strHQL += " Group by FinancialCode,IntervalCode,AccountCode,AccountName ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim());
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");

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

            strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.FinancialID='" + accountFinancialSet.ID.ToString() + "' and accountingIntervalSet.Status='OPEN' Order By accountingIntervalSet.ID ASC ";
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
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJZTWBXJC").ToString().Trim()+"')", true);
        //    DL_FinancialID.Focus();
        //    return;
        //}
        //if (DL_IntervalID.SelectedValue.Trim() == "0")
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJJWBXJC").ToString().Trim()+"')", true);
        //    DL_IntervalID.Focus();
        //    return;
        //}
        if (DL_Financial.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSCWZTWBXJC").ToString().Trim()+"')", true);
            DL_Financial.Focus();
            return;
        }
        if (DL_Interval.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSCWJWBXJC").ToString().Trim()+"')", true);
            DL_Interval.Focus();
            return;
        }

        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        AccountGeneralLedger accountGeneralLedger = new AccountGeneralLedger();
        string strHQL = "Select FinancialCode,IntervalCode,AccountCode,AccountName,SUM(TotalMoney) TotalAllMoney from T_AccountGeneralLedger Where 1=1 ";
        strHQL += " and FinancialCode = '" + DL_Financial.SelectedValue.Trim() + "' and IntervalCode = '" + DL_Interval.SelectedValue.Trim() + "' ";
        strHQL += " Group by FinancialCode,IntervalCode,AccountCode,AccountName ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                decimal strExchangeRate = GetFinancialExchangeRate(ds.Tables[0].Rows[i]["FinancialCode"].ToString());

                accountGeneralLedger.AccountCode = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                accountGeneralLedger.AccountName = ds.Tables[0].Rows[i]["AccountName"].ToString();
                accountGeneralLedger.Creater = strUserCode.Trim();
                accountGeneralLedger.CreateTime = DateTime.Now;
                //accountGeneralLedger.FinancialID = int.Parse(DL_FinancialID.SelectedValue.Trim());
                //accountGeneralLedger.IntervalID = int.Parse(DL_IntervalID.SelectedValue.Trim());
                accountGeneralLedger.FinancialCode = lbl_FinancialID.Text.Trim();
                accountGeneralLedger.IntervalCode = lbl_IntervalID.Text.Trim();
                accountGeneralLedger.PayableRecordID = 0;
                accountGeneralLedger.ReceivablesRecordID = 0;

                //   decimal strExchangeRateGold = GetFinancialExchangeRate(DL_FinancialID.SelectedValue.Trim());
                decimal strExchangeRateGold = GetFinancialExchangeRate(lbl_FinancialID.Text.Trim());

                accountGeneralLedger.TotalMoney = decimal.Parse(ds.Tables[0].Rows[i]["TotalAllMoney"].ToString()) * strExchangeRate / strExchangeRateGold;
                decimal strTotalMoney = accountGeneralLedger.TotalMoney;
                strHQL = "From AccountGeneralLedger as accountGeneralLedger Where accountGeneralLedger.FinancialCode='" + accountGeneralLedger.FinancialCode.ToString() + "' and " +
                    "accountGeneralLedger.IntervalCode='" + accountGeneralLedger.IntervalCode.ToString() + "' and accountGeneralLedger.PayableRecordID ='0' and " +
                    "accountGeneralLedger.ReceivablesRecordID ='0' and accountGeneralLedger.AccountCode='" + accountGeneralLedger.AccountCode.ToString() + "' ";
                    //"accountGeneralLedger.TotalMoney='" + accountGeneralLedger.TotalMoney.ToString() + "' ";
                IList lst = accountGeneralLedgerBLL.GetAllAccountGeneralLedgers(strHQL);
                if (lst != null && lst.Count > 0)
                {
                    accountGeneralLedger = (AccountGeneralLedger)lst[0];
                    accountGeneralLedger.TotalMoney = strTotalMoney;
                    accountGeneralLedgerBLL.UpdateAccountGeneralLedger(accountGeneralLedger, accountGeneralLedger.ID);
                }
                else
                {
                    accountGeneralLedgerBLL.AddAccountGeneralLedger(accountGeneralLedger);
                }
            }

            LoadAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZZCG").ToString().Trim()+"')", true);
        }
    }

    protected string GetFinancialName(string strFinancialCode)
    {
        string flag = string.Empty;
        string strHQL = "Select FinancialName From T_AccountFinancialSet where FinancialCode='" + strFinancialCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["FinancialName"].ToString().Trim();
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

    protected string GetIntervalName(string strIntervalCode)
    {
        string flag = string.Empty;
        string strHQL = "Select IntervalName From T_AccountingIntervalSet where IntervalCode='" + strIntervalCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountingIntervalSet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["IntervalName"].ToString().Trim();
        }
        else
        {
            flag = "";
        }
        return flag;
    }

    protected string GetFinancialCurrency(string strFinancialCode)
    {
        string flag = string.Empty;
        string strHQL = "Select CurrencyType From T_AccountFinancialSet where FinancialCode='" + strFinancialCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["CurrencyType"].ToString();
        }
        else
        {
            flag = "";
        }
        return flag;
    }

    protected void DL_Financial_SelectedIndexChanged(object sender, EventArgs e)
    {
        string FinancialID = DL_Financial.SelectedValue.Trim();
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        string strHQL = "from AccountFinancialSet as accountFinancialSet where accountFinancialSet.FinancialCode = '" + FinancialID + "' ";
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];

            strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.FinancialCode='" + accountFinancialSet.FinancialCode.Trim() + "' and accountingIntervalSet.Status='CLOSE' Order By accountingIntervalSet.ID ASC ";
            AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
            lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
            DL_Interval.DataSource = lst;
            DL_Interval.DataBind();
            DL_Interval.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }
}