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

public partial class TTAccountIncomeStatement : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","ËðÒæ±í", strUserCode);
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
        string strHQL;
        if (strFinancialID.Trim() != "" && strIntervalID.Trim() != "")
        {
            strHQL = "select F.AccountType,D.AccountName,D.BeforeMoney,D.HappenMoney,D.BeforeMoney+D.HappenMoney BackMoney,E.CurrencyType from " +
                "(select A.*,COALESCE(B.BeforeMoney,0) BeforeMoney,COALESCE(C.HappenMoney,0) HappenMoney from (select distinct AccountCode,AccountName,FinancialCode from " +
                "T_AccountGeneralLedger where FinancialCode='" + strFinancialID + "' and IntervalCode='" + strIntervalID + "') A left join (select AccountCode,AccountName," +
                "SUM(TotalMoney) BeforeMoney from T_AccountGeneralLedger where FinancialCode='" + strFinancialID + "' and IntervalCode='" + strIntervalID + "' and " +
                "ReceivablesRecordID='0' and PayableRecordID='0' group by AccountCode,AccountName) B on A.AccountCode=B.AccountCode left join (select AccountCode,AccountName," +
                "SUM(TotalMoney) HappenMoney from T_AccountGeneralLedger where FinancialCode='" + strFinancialID + "' and IntervalCode='" + strIntervalID + "' and " +
                "ReceivablesRecordID+PayableRecordID>0 group by AccountCode,AccountName) C on A.AccountCode=C.AccountCode) D,T_AccountFinancialSet E,T_Account F where "+
                "D.FinancialCode=E.FinancialCode and D.AccountCode=F.AccountCode ";

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");

            DataGrid1.CurrentPageIndex = 0;
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();
            lbl_sql.Text = strHQL;
        }
        else
        {
            DataGrid1.CurrentPageIndex = 0;
            DataGrid1.DataSource = null;
            DataGrid1.DataBind();
            lbl_sql.Text = "";
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        string strFinancialID = DL_Financial.SelectedValue.Trim();
        string strIntervalID = DL_Interval.SelectedValue.Trim();
        if (strFinancialID.Trim() != "" && strIntervalID.Trim() != "")
        {
            LoadAccountGeneralLedgerList(strFinancialID, strIntervalID);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSCWZTJCWJWBXJC").ToString().Trim()+"')", true);
            DL_Financial.Focus();
            DL_Interval.Focus();
            return;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!(DL_Financial.SelectedValue.Trim() != "" && DL_Interval.SelectedValue.Trim() != ""))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSCWZTJCWJWBXJC").ToString().Trim()+"')", true);
            DL_Financial.Focus();
            DL_Interval.Focus();
            return;
        }

        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("ZiChanLiRunBiao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
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
        if (strFinancialID.Trim() != "" && strIntervalID.Trim() != "")
        {
            string strHQL = "select F.AccountType 'SubjectType',D.AccountName 'AccountingSubjects',D.BeforeMoney 'BeginningBalance',D.HappenMoney 'AmountOccurred',D.BeforeMoney+D.HappenMoney 'EndingBalance',E.CurrencyType 'Currency' from " +  //ChineseWord //ChineseWord
                "(select A.*,COALESCE(B.BeforeMoney,0) BeforeMoney,COALESCE(C.HappenMoney,0) HappenMoney from (select distinct AccountCode,AccountName,FinancialCode from " +
                "T_AccountGeneralLedger where FinancialCode='" + strFinancialID + "' and IntervalCode='" + strIntervalID + "') A left join (select AccountCode,AccountName," +
                "SUM(TotalMoney) BeforeMoney from T_AccountGeneralLedger where FinancialCode='" + strFinancialID + "' and IntervalCode='" + strIntervalID + "' and " +
                "ReceivablesRecordID='0' and PayableRecordID='0' group by AccountCode,AccountName) B on A.AccountCode=B.AccountCode left join (select AccountCode,AccountName," +
                "SUM(TotalMoney) HappenMoney from T_AccountGeneralLedger where FinancialCode='" + strFinancialID + "' and IntervalCode='" + strIntervalID + "' and " +
                "ReceivablesRecordID+PayableRecordID>0 group by AccountCode,AccountName) C on A.AccountCode=C.AccountCode) D,T_AccountFinancialSet E,T_Account F where " +
                "D.FinancialCode=E.FinancialCode and D.AccountCode=F.AccountCode ";

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountGeneralLedger");
            return ds.Tables[0];
        }
        else
        {
            return null;
        }
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
}
