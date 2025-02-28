using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using System.IO;
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTExpenseClaimToFinanceHandleForm : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        decimal deExpense = 0;

        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);


        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "费用报销记账", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "查看所有费用报销";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "from ExpenseClaim as expenseClaim where ";
            strHQL += " expenseClaim.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " and expenseClaim.Status In ('Passed','Completed','Recorded')";
            strHQL += " Order by expenseClaim.Status ASC";
            ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
            lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_Sql2.Text = strHQL;

            ExpenseClaim expenseClaim = new ExpenseClaim();
            for (int i = 0; i < lst.Count; i++)
            {
                expenseClaim = (ExpenseClaim)lst[i];
                deExpense += expenseClaim.Amount;
            }

            LB_Amount.Text = deExpense.ToString();

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZhe").ToString().Trim() + strUserCode + strUserName;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName, strHQL;
        IList lst;

        decimal deExpense = 0;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = GetDepartName(strDepartCode);

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen").ToString().Trim() + strDepartName;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ApplicantCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") ";
            strHQL += " and expenseClaim.Status In ('Passed','Completed','Recorded')";
            strHQL += " Order by expenseClaim.Status ASC";
            ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
            lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_Sql2.Text = strHQL;

            ExpenseClaim expenseClaim = new ExpenseClaim();
            for (int i = 0; i < lst.Count; i++)
            {
                expenseClaim = (ExpenseClaim)lst[i];
                deExpense += expenseClaim.Amount;
            }

            LB_Amount.Text = deExpense.ToString();


            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen").ToString().Trim() + strDepartName;
            LB_Sql.Text = strHQL;

            LB_OperatorCode.Text = "";
            LB_OperatorName.Text = "";
            LB_Status.Text = "";

            LoadRelatedWL("ExpenseReimbursement", -1);
            LoadRelatedExpenseClaimDetail("-1");
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strUserName = GetUserName(strUserCode);

        decimal deExpense = 0;

        strHQL = "from ExpenseClaim as expenseClaim where  expenseClaim.ApplicantCode = " + "'" + strUserCode + "'";
        strHQL += " and expenseClaim.Status In ('Passed','Completed','Recorded')";
        strHQL += " Order by expenseClaim.Status ASC";
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;

        ExpenseClaim expenseClaim = new ExpenseClaim();
        for (int i = 0; i < lst.Count; i++)
        {
            expenseClaim = (ExpenseClaim)lst[i];
            deExpense += expenseClaim.Amount;
        }

        LB_Amount.Text = deExpense.ToString();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZhe").ToString().Trim() + strUserCode + " " + strUserName;

        LoadRelatedWL("ExpenseReimbursement", -1);
        LoadRelatedExpenseClaimDetail("-1");
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strAccountCode, strAccountName;
        int intReAndPayalbeID;
        string strHQL;
        IList lst;


        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            if (e.CommandName == "ID")
            {
            }

            if (e.CommandName == "ACCOUNT")
            {
                //插入应收应付数据到应收应付表
                decimal deAmount = decimal.Parse(e.Item.Cells[4].Text);
                string strCurrencyType = e.Item.Cells[5].Text.Trim();
                string strReAndPayer = GetExpenseApplicantName(strID);
                string strStatus = GetExpenseClaimStatus(strID);

                int intProjectID = 1;

                Label23.Text = strStatus;

                if (strStatus != "Recorded")
                {
                    if (strStatus == "Passed" | strStatus == "Completed")
                    {
                        strHQL = "From ExpenseClaimDetail as expenseClaimDetail Where expenseClaimDetail.ECID = " + strID;
                        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
                        lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);
                        ExpenseClaimDetail expenseClaimDetail = new ExpenseClaimDetail();

                        for (int i = 0; i < lst.Count; i++)
                        {
                            expenseClaimDetail = (ExpenseClaimDetail)lst[i];
                            strAccountName = expenseClaimDetail.Account;
                            strAccountCode = expenseClaimDetail.AccountCode;

                            intReAndPayalbeID = ShareClass.InsertReceivablesOrPayableByAccount("Payables", "ExpenseCO", "ExpenseCO", strID, strID, strAccountCode, strAccountName, deAmount, strCurrencyType, strReAndPayer, strUserCode, intProjectID);
                            ShareClass.InsertReceivablesOrPayableRecord("Payables", intReAndPayalbeID, deAmount, "Transfer", strCurrencyType, strReAndPayer, strUserCode, intProjectID);   

                            strHQL = "Update T_ConstractPayable Set OutOfPocketAccount = " + deAmount.ToString() + ",UNPayAmount = 0 Where ID = " + intReAndPayalbeID.ToString();
                            ShareClass.RunSqlCommand(strHQL);

                            //把报销费用列入预算费用
                            string strApplicantCode = GetExpenseClaimApplicantCode(strID);
                            if (strAccountCode != "")
                            {
                                ShareClass.AddClaimExpenseToBudget(strAccountCode, strAccountName, 0, strUserCode, deAmount, DateTime.Now.Year, DateTime.Now.Month);
                            }
                        }

                        strHQL = "Update T_ExpenseClaim Set Status = 'Recorded' Where ECID = " + strID;
                        ShareClass.RunSqlCommand(strHQL);

                        e.Item.Cells[8].Text = "Recorded";

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJZCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZYZTWTGHWCDCNJZJC").ToString().Trim() + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBBNZFJZ").ToString().Trim() + "')", true);
                }

            }

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            LoadRelatedWL("ExpenseReimbursement", int.Parse(strID));
            LoadRelatedExpenseClaimDetail(strID);
        }
    }

    protected string GetExpenseClaimStatus(string strECID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        return ((ExpenseClaim)lst[0]).Status.Trim();
    }


    protected string GetExpenseApplicantName(string strECID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        return ((ExpenseClaim)lst[0]).ApplicantName.Trim();
    }

    //取得非具体业务对销报销款的申请人代码
    protected string GetExpenseClaimApplicantCode(string strECID)
    {
        string strHQL;

        strHQL = "Select ApplicantCode From T_ExpenseClaim Where ECID = " + strECID;
        strHQL += " and RelatedType = 'Other' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ExpenseClaim");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql2.Text;

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        IList lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void BT_AllMember_Click(object sender, EventArgs e)
    {
        decimal deExpense = 0;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        string strDepartString = LB_DepartString.Text.Trim();
        strHQL = "from ExpenseClaim as expenseClaim where  expenseClaim.ApplicantCode = " + "'" + strUserCode + "'";
        strHQL += " and expenseClaim.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and expenseClaim.Status In ('Passed','Completed','Recorded')";
        strHQL += " Order by expenseClaim.Status ASC";
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;

        ExpenseClaim expenseClaim = new ExpenseClaim();
        for (int i = 0; i < lst.Count; i++)
        {
            expenseClaim = (ExpenseClaim)lst[i];
            deExpense += expenseClaim.Amount;
        }

        LB_Amount.Text = deExpense.ToString();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim();

        LoadRelatedWL("ExpenseReimbursement", -1);
        LoadRelatedExpenseClaimDetail("-1");
    }

    protected void LoadRelatedWL(string strWLType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadRelatedExpenseClaimDetail(string strECID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaimDetail as expenseClaimDetail where  expenseClaimDetail.ECID = " + strECID;
        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
        lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName.Trim();
    }


    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }
}
