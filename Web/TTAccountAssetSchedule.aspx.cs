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

public partial class TTAccountAssetSchedule : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);//Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","资产明细表", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadAccountFinancialSet();
            LoadAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim());
        }
    }

    protected void LoadAccountFinancialSet()
    {
        //string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode); 
        string strSuperDepartCode = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode.Trim());
        string strDepartString = GetUserDepartCode(strUserCode.Trim());
        //   string strHQL = "Select * From T_AccountFinancialSet Where DepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
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

    protected void LoadAccountGeneralLedgerList(string strFinancialID, string strIntervalID)
    {
        string strHQL = "Select A.*,B.AccountType from T_AccountGeneralLedger A,T_Account B where A.AccountCode=B.AccountCode ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and A.FinancialCode='" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and A.IntervalCode='" + strIntervalID + "' ";
        }
        strHQL += " Order By A.ID Asc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        string strFinancialID = DL_Financial.SelectedValue.Trim();
        string strIntervalID = DL_Interval.SelectedValue.Trim();
        LoadAccountGeneralLedgerList(strFinancialID, strIntervalID);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("ZiChanMingXiBiao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                CreateExcel(getAccountGeneralLedgerList(DL_Financial.SelectedValue.Trim(), DL_Interval.SelectedValue.Trim()), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected DataTable getAccountGeneralLedgerList(string strFinancialID, string strIntervalID)
    {
        string strHQL = "select E.AccountType '科目类型',A.AccountName '会计科目','性质'=case when ReceivablesRecordID>0 then '收款' when PayableRecordID>0 then '付款' else '上期结转' end," + 
            "FinancialName '财务帐套',IntervalName '财务区间',TotalMoney '发生金额',CurrencyType '币种',UserName '操作人',CreateTime '操作时间' from T_AccountGeneralLedger A," + 
            "T_AccountFinancialSet B,T_AccountingIntervalSet C,T_ProjectMember D,T_Account E where A.FinancialCode=B.FinancialCode and A.IntervalCode=C.IntervalCode and A.Creater=D.UserCode and A.AccountCode=E.AccountCode ";
        if (strFinancialID.Trim() != "")
        {
            strHQL += " and A.FinancialCode='" + strFinancialID + "' ";
        }
        if (strIntervalID.Trim() != "")
        {
            strHQL += " and A.IntervalCode='" + strIntervalID + "' ";
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

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
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
            return "上期结转"; 
        }
        else if (strReceivablesRecordID == "0" && strPayableRecordID != "0")
        {
            return "付款"; 
        }
        else if (strReceivablesRecordID != "0" && strPayableRecordID == "0")
        {
            return "收款"; 
        }
        else
            return "";
    }
}
