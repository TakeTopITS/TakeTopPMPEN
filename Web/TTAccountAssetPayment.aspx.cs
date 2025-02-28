using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTAccountAssetPayment : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","收支明细汇总表", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "", true);
        if (Page.IsPostBack != true)
        {
            LoadAccountFinancialSet();

            ShareClass.LoadAccountForDDL(ddl_Account);
            TakeTopCore.CoreShareClass.InitialAllDepartmentTree( LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1);

            LoadAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim(), ddl_Account.SelectedValue.Trim(), ddl_Type.SelectedValue.Trim(), LB_DepartString.Text);
        }
    }

    protected void LoadAccountFinancialSet()
    {
        string strSuperDepartCode = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode.Trim());
        string strDepartString = GetUserDepartCode(strUserCode.Trim());
        string strHQL = "Select * From T_AccountFinancialSet Where (DepartCode in " + strDepartString + " OR DepartCode in " + strSuperDepartCode + ") ";
        strHQL += " Order By ID ASC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet");

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

    protected void LoadAccountGeneralLedgerList(string strFinancialID, string strIntervalID, string strAccountCode, string strType, string strDepartCode)
    {
        string strHQL = "Select A.*,B.AccountType from T_AccountGeneralLedger A,T_Account B,T_AccountFinancialSet C where A.AccountCode=B.AccountCode and A.FinancialCode=C.FinancialCode ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and A.FinancialCode='" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and A.IntervalCode='" + strIntervalID + "' ";
        }
        if (strAccountCode.Trim() != "")
        {
            strHQL += " and A.AccountCode='" + strAccountCode + "' ";
        }
        if (strType == "0")
        {
        }
        else if (strType == "1")//收款
        {
            strHQL += " and A.TotalMoney>0 ";
        }
        else if (strType == "2")//付款
        {
            strHQL += " and A.TotalMoney<0 ";
        }
        if (strDepartCode.Trim() != "0" && strDepartCode.Trim() != "" && !string.IsNullOrEmpty(strDepartCode))
        {
            strHQL += " and C.DepartCode='" + strDepartCode + "' ";
        }
        strHQL += " Order By A.ID Asc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;

        GetAggregateAmount(strFinancialID, strIntervalID, strAccountCode, strType, strDepartCode);
    }

    protected void GetAggregateAmount(string strFinancialID, string strIntervalID, string strAccountCode, string strType, string strDepartCode)
    {
        string strHQL = "Select SUM(A.TotalMoney) allmoney from T_AccountGeneralLedger A,T_Account B,T_AccountFinancialSet C where A.AccountCode=B.AccountCode and A.FinancialCode=C.FinancialCode ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and A.FinancialCode='" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and A.IntervalCode='" + strIntervalID + "' ";
        }
        if (strAccountCode.Trim() != "")
        {
            strHQL += " and A.AccountCode='" + strAccountCode + "' ";
        }
        if (strDepartCode.Trim() != "0" && strDepartCode.Trim() != "" && !string.IsNullOrEmpty(strDepartCode))
        {
            strHQL += " and C.DepartCode='" + strDepartCode + "' ";
        }
        DataSet ds0 = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");
        if (ds0 != null && ds0.Tables[0].Rows.Count > 0)
        {
            lbl_Money.Text = string.IsNullOrEmpty(ds0.Tables[0].Rows[0]["allmoney"].ToString()) ? "0" : ds0.Tables[0].Rows[0]["allmoney"].ToString();
        }
        else
        {
            lbl_Money.Text = "0";
        }

        strHQL = "Select SUM(A.TotalMoney) recmoney from T_AccountGeneralLedger A,T_Account B,T_AccountFinancialSet C where A.AccountCode=B.AccountCode and A.FinancialCode=C.FinancialCode and A.TotalMoney>0 ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and A.FinancialCode='" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and A.IntervalCode='" + strIntervalID + "' ";
        }
        if (strAccountCode.Trim() != "")
        {
            strHQL += " and A.AccountCode='" + strAccountCode + "' ";
        }
        if (strDepartCode.Trim() != "0" && strDepartCode.Trim() != "" && !string.IsNullOrEmpty(strDepartCode))
        {
            strHQL += " and C.DepartCode='" + strDepartCode + "' ";
        }
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");
        if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
        {
            lbl_RecMoney.Text = string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["recmoney"].ToString()) ? "0" : ds1.Tables[0].Rows[0]["recmoney"].ToString();
        }
        else
        {
            lbl_RecMoney.Text = "0";
        }

        strHQL = "Select SUM(A.TotalMoney) paymoney from T_AccountGeneralLedger A,T_Account B,T_AccountFinancialSet C where A.AccountCode=B.AccountCode and A.FinancialCode=C.FinancialCode and A.TotalMoney<0 ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and A.FinancialCode='" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and A.IntervalCode='" + strIntervalID + "' ";
        }
        if (strAccountCode.Trim() != "")
        {
            strHQL += " and A.AccountCode='" + strAccountCode + "' ";
        }
        if (strDepartCode.Trim() != "0" && strDepartCode.Trim() != "" && !string.IsNullOrEmpty(strDepartCode))
        {
            strHQL += " and C.DepartCode='" + strDepartCode + "' ";
        }
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");
        if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
        {
            lbl_PayMoney.Text = string.IsNullOrEmpty(ds2.Tables[0].Rows[0]["paymoney"].ToString()) ? "0" : (decimal.Parse(ds2.Tables[0].Rows[0]["paymoney"].ToString()) * (-1)).ToString();
        }
        else
        {
            lbl_PayMoney.Text = "0";
        }

        if (strType == "0")
        {
            Label1.Visible = true;
            lbl_RecMoney.Visible = true;
            Label2.Visible = true;
            lbl_PayMoney.Visible = true;
            Label3.Visible = true;
            lbl_Money.Visible = true;
        }
        else if (strType == "1")//收款
        {
            Label1.Visible = true;
            lbl_RecMoney.Visible = true;
            Label2.Visible = false;
            lbl_PayMoney.Visible = false;
            Label3.Visible = false;
            lbl_Money.Visible = false;
        }
        else if (strType == "2")//付款
        {
            Label1.Visible = false;
            lbl_RecMoney.Visible = false;
            Label2.Visible = true;
            lbl_PayMoney.Visible = true;
            Label3.Visible = false;
            lbl_Money.Visible = false;
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        string strFinancialID = DL_Financial.SelectedValue.Trim();
        string strIntervalID = DL_Interval.SelectedValue.Trim();
        LoadAccountGeneralLedgerList(strFinancialID, strIntervalID, ddl_Account.SelectedValue.Trim(), ddl_Type.SelectedValue.Trim(), LB_DepartString.Text);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = "";
                if (ddl_Type.SelectedValue.Trim() == "0")
                {
                    fileName = LanguageHandle.GetWord("ShouZhiMingXiHuiZongBiao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (ddl_Type.SelectedValue.Trim() == "1")
                {
                    fileName = LanguageHandle.GetWord("ShouKuanMingXiHuiZongBiao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                else if (ddl_Type.SelectedValue.Trim() == "")
                {
                    fileName = LanguageHandle.GetWord("FuKuanMingXiHuiZongBiao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                }
                CreateExcel(getAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim(), ddl_Account.SelectedValue.Trim(), ddl_Type.SelectedValue.Trim(), LB_DepartString.Text), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected DataTable getAccountGeneralLedgerList(string strFinancialID, string strIntervalID, string strAccountCode, string strType, string strDepartCode)
    {
        string strHQL = "select E.AccountType 'SubjectType',A.AccountName 'AccountingSubjects','Nature'=case when ReceivablesRecordID>0 then 'Receipt' when PayableRecordID>0 then 'Payment' else 'CarryForwardFromPreviousPeriod' end," +  //ChineseWord //ChineseWord
            "FinancialName 'FinancialAccountingSet',IntervalName 'FinancialPeriod',TotalMoney 'AmountOccurred',CurrencyType 'Currency',UserName 'Operator',CreateTime 'OperationTime' from T_AccountGeneralLedger A," +  //ChineseWord //ChineseWord
            "T_AccountFinancialSet B,T_AccountingIntervalSet C,T_ProjectMember D,T_Account E where A.FinancialCode=B.FinancialCode and A.IntervalCode=C.IntervalCode and A.Creater=D.UserCode and A.AccountCode=E.AccountCode ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and A.FinancialCode='" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and A.IntervalCode='" + strIntervalID + "' ";
        }
        if (strAccountCode.Trim() != "")
        {
            strHQL += " and A.AccountCode='" + strAccountCode + "' ";
        }
        if (strType == "0")
        {
        }
        else if (strType == "1")//收款
        {
            strHQL += " and A.TotalMoney>0 ";
        }
        else if (strType == "2")//付款
        {
            strHQL += " and A.TotalMoney<0 ";
        }
        if (strDepartCode.Trim() != "0" && strDepartCode.Trim() != "" && !string.IsNullOrEmpty(strDepartCode))
        {
            strHQL += " and B.DepartCode='" + strDepartCode + "' ";
        }
        strHQL += " Order By A.ID Asc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");
        return ds.Tables[0];
    }

    private void CreateExcel(DataTable dt, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
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

            strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.FinancialCode='" + accountFinancialSet.FinancialCode.Trim() + "' Order By accountingIntervalSet.ID ASC ";
            AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
            lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
            DL_Interval.DataSource = lst;
            DL_Interval.DataBind();
            DL_Interval.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            string strDepartCode = treeNode.Target.Trim();
            LB_DepartString.Text = strDepartCode;
            LoadAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim(), ddl_Account.SelectedValue.Trim(), ddl_Type.SelectedValue.Trim(), LB_DepartString.Text);
        }

        
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
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

    protected string GetUserName(string strusercode)
    {
        return ShareClass.GetUserName(strusercode);
    }

    protected string GetAccountType(string strReceivablesRecordID, string strPayableRecordID)
    {
        if (strReceivablesRecordID == "0" && strPayableRecordID == "0")
        {
            return "CarryForwardFromPreviousPeriod";  //ChineseWord //ChineseWord
        }
        else if (strReceivablesRecordID == "0" && strPayableRecordID != "0")
        {
            return "Payment";  //ChineseWord //ChineseWord
        }
        else if (strReceivablesRecordID != "0" && strPayableRecordID == "0")
        {
            return "Receipt";  //ChineseWord //ChineseWord
        }
        else
            return "";
    }
}
