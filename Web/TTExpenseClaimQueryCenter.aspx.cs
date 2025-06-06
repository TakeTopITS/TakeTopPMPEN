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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTExpenseClaimQueryCenter : System.Web.UI.Page
{
    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strRelatedTitle = "";

        string strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ʲ��Ǽ����", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        if (strRelatedType == null | strRelatedID == null)
        {
            strRelatedType = "Other";
            strRelatedID = "0";
        }


        if (strRelatedType == "Other")
        {
            strRelatedType = "Other";
            strRelatedID = "0";
        }

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";
            strRelatedTitle = GetProjectName(strRelatedID);
        }

        if (strRelatedType == "Requirement")
        {
            strRelatedType = "Requirement";
            strRelatedTitle = GetRequirementName(strRelatedID);
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[0].Text.Trim();

            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;


            LoadRelatedWL("ExpenseReimbursement", strRelatedType, int.Parse(strID));
            LoadRelatedExpenseClaimDetail(strID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void BT_FindAOCode_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        decimal deExpense = 0;

        strHQL = "from ExpenseClaim as expenseClaim where ";
        strHQL += " expenseClaim.ECID = " + TB_AOCode.Text.Trim();

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        ExpenseClaim expenseClaim = new ExpenseClaim();
        for (int i = 0; i < lst.Count; i++)
        {
            expenseClaim = (ExpenseClaim)lst[i];
            deExpense += expenseClaim.Amount;
        }

        LB_Amount.Text = deExpense.ToString();
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        decimal deExpense = 0;

        string strAOName, strApplicantName, strProjectName, strStatus;
        string strStartTime, strEndTime;

        strAOName = TB_ExpenseName.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        strApplicantName = TB_ApplicantName.Text.Trim();
        strProjectName = TB_ProjectName.Text.Trim();

        strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");

        strHQL = "from ExpenseClaim as expenseClaim where ";
        strHQL += "  expenseClaim.ApplicantName Like " + "'%" + strApplicantName + "%'";
        strHQL += " and expenseClaim.ExpenseName Like " + "'%" + strAOName + "%'";
        if (strProjectName != "")
        {
            strHQL += " And expenseClaim.RelatedType = 'Project' and expenseClaim.RelatedID in (Select project.ProjectID From Project as project Where project.ProjectName Like " + "'%" + strProjectName + "%')";
        }

        strHQL += " And to_char( expenseClaim.ApplyTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char( expenseClaim.ApplyTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
        strHQL += " and expenseClaim.Status Like " + "'%" + strStatus + "%'";
        strHQL += " Order by expenseClaim.ECID DESC";

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        ExpenseClaim expenseClaim = new ExpenseClaim();
        for (int i = 0; i < lst.Count; i++)
        {
            expenseClaim = (ExpenseClaim)lst[i];
            deExpense += expenseClaim.Amount;
        }

        LB_Amount.Text = deExpense.ToString();
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadExpenseClaim(string strApplicantCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;
        decimal deExpense = 0;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.RelatedType = " + "'" + strRelatedType + "'" + " and expenseClaim.RelatedID = " + strRelatedID + " and expenseClaim.ApplicantCode = " + "'" + strApplicantCode + "'" + " Order by expenseClaim.ECID DESC";
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        ExpenseClaim expenseClaim = new ExpenseClaim();
        for (int i = 0; i < lst.Count; i++)
        {
            expenseClaim = (ExpenseClaim)lst[i];
            deExpense += expenseClaim.Amount;
        }


        LB_Amount.Text = deExpense.ToString();
    }


    protected void LoadRelatedExpenseClaimDetail(string strECID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaimDetail as expenseClaimDetail where  expenseClaimDetail.ECID = " + strECID;
        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
        lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);

        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();
    }

    protected string GetProjectName(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

    protected string GetRequirementName(string strReqID)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();

        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement.ReqName.Trim();
    }

}
