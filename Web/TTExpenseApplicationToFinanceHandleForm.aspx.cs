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

public partial class TTExpenseApplicationToFinanceHandleForm : System.Web.UI.Page
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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "费用申请记账", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "查看所有费用申请";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "from ExpenseApplyWL as expenseApplyWL where ";
            strHQL += " expenseApplyWL.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " and expenseApplyWL.Status  In ('Passed','Completed','Recorded')";
            strHQL += " Order by expenseApplyWL.Status ASC";
            ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
            lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_Sql2.Text = strHQL;

            ExpenseApplyWL expenseApplyWL = new ExpenseApplyWL();
            for (int i = 0; i < lst.Count; i++)
            {
                expenseApplyWL = (ExpenseApplyWL)lst[i];
                deExpense += expenseApplyWL.Amount;
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

            strHQL = "from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.ApplicantCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") ";
            strHQL += " and expenseApplyWL.Status  In ('Passed','Completed','Recorded')";
            strHQL += " Order by expenseApplyWL.Status ASC";
            ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
            lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
            LB_Sql2.Text = strHQL;

            ExpenseApplyWL expenseApplyWL = new ExpenseApplyWL();
            for (int i = 0; i < lst.Count; i++)
            {
                expenseApplyWL = (ExpenseApplyWL)lst[i];
                deExpense += expenseApplyWL.Amount;
            }

            LB_Amount.Text = deExpense.ToString();


            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen").ToString().Trim() + strDepartName;
            LB_Sql.Text = strHQL;

            LB_OperatorCode.Text = "";
            LB_OperatorName.Text = "";
            LB_Status.Text = "";

            LoadRelatedWL("ExpenseRequest", -1);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strUserName = GetUserName(strUserCode);

        decimal deExpense = 0;

        strHQL = "from ExpenseApplyWL as expenseApplyWL where  expenseApplyWL.ApplicantCode = " + "'" + strUserCode + "'";
        strHQL += " and expenseApplyWL.Status  In ('Passed','Completed','Recorded')";
        strHQL += " Order by expenseApplyWL.Status ASC";
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;

        ExpenseApplyWL expenseApplyWL = new ExpenseApplyWL();
        for (int i = 0; i < lst.Count; i++)
        {
            expenseApplyWL = (ExpenseApplyWL)lst[i];
            deExpense += expenseApplyWL.Amount;
        }

        LB_Amount.Text = deExpense.ToString();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZhe").ToString().Trim() + strUserCode + " " + strUserName;
        LB_Sql.Text = strHQL;

        LoadRelatedWL("ExpenseRequest", -1);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strAccountCode, strAccount;
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
                string strReAndPayer = GetExpenseApplicationStatus(strID);
                string strStatus = GetExpenseApplicationStatus(strID);

                int intProjectID = 1;

                if (strStatus != "Recorded")
                {
                    if (strStatus == "Passed" | strStatus == "Completed")
                    {
                        strHQL = "From ExpenseApplyWL as expenseApplyWL where expenseApplyWL.ID = " + strID;
                        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
                        lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

                        ExpenseApplyWL expenseApplyWL = (ExpenseApplyWL)lst[0];

                        strAccountCode = expenseApplyWL.AccountCode.Trim();
                        strAccount = expenseApplyWL.Account.Trim();

                        intReAndPayalbeID = ShareClass.InsertReceivablesOrPayableByAccount("Payables", "ExpenseAO", "ExpenseAO", strID, strID, strAccountCode, strAccount, deAmount, strCurrencyType, strReAndPayer, strUserCode, intProjectID);
                        ShareClass.InsertReceivablesOrPayableRecord("Payables", intReAndPayalbeID, deAmount, "Transfer", strCurrencyType, strReAndPayer, strUserCode, intProjectID);   //ChineseWord

                        strHQL = "Update T_ConstractPayable Set OutOfPocketAccount = " + deAmount.ToString() + ",UNPayAmount = 0 Where ID = " + intReAndPayalbeID.ToString();
                        ShareClass.RunSqlCommand(strHQL);

                        strHQL = "Update T_ExpenseApplyWL Set Status = 'Recorded' Where ID = " + strID;
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

            LoadRelatedWL("ExpenseRequest", int.Parse(strID));
        }
    }

    protected string GetExpenseApplicationStatus(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseApplyWL as expenseApplyWL where  expenseApplyWL.ID = " + strID;
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        return ((ExpenseApplyWL)lst[0]).Status.Trim();
    }

    protected string GetExpenseApplicationName(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseApplyWL as expenseApplyWL where  expenseApplyWL.ID = " + strID;
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        return ((ExpenseApplyWL)lst[0]).ApplicantName.Trim();
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql2.Text;

        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        IList lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void BT_AllMember_Click(object sender, EventArgs e)
    {
        decimal deExpense = 0;
        string strHQL;
        IList lst;

        string strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from ExpenseApplyWL as expenseApplyWL ";
        strHQL += " where expenseApplyWL.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and expenseApplyWL.Status  In ('Passed','Completed','Recorded')";
        strHQL += " Order by expenseApplyWL.Status ASC";
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;

        ExpenseApplyWL expenseApplyWL = new ExpenseApplyWL();
        for (int i = 0; i < lst.Count; i++)
        {
            expenseApplyWL = (ExpenseApplyWL)lst[i];
            deExpense += expenseApplyWL.Amount;
        }

        LB_Amount.Text = deExpense.ToString();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim();

        LoadRelatedWL("ExpenseRequest", -1);
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
