using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Security.Permissions;
using System.Data.SqlClient;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;


using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using org.apache.commons.math3.geometry.spherical.oned;


public partial class TTProjectSummaryAnalystChart : System.Web.UI.Page
{
    string strProjectID, strProjectName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName;


        strProjectID = Request.QueryString["ProjectID"];

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);


        DisplayAnalystChart();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            string strVerID;
            strVerID = ShareClass.GetProjectPlanVerID(strProjectID, "InUse").ToString();
            ShareClass.DisplayRelatedMileStoneStepDump(strProjectID, strVerID, Repeater1);

            LoadProjectBudget(strProjectID);
        }
    }

    protected void DisplayAnalystChart()
    {
        string strHQL;
        string strCmdText, strChartTitle;

        strChartTitle = LanguageHandle.GetWord("JHZTFBT");
        strCmdText = @"Select (cast(percent_done as varchar) || cast('%' as varchar)) as XName,cast(count(*) as numeric) as YNumber
                  From T_ImplePlan  where ProjectID = " + strProjectID + " and VerID in (Select VerID From T_ProjectPlanVersion Where ProjectID = " + strProjectID + " and Type  not in ('Baseline','Backup'))  Group By percent_done";
        IFrame_Chart_PlanStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("XMFYYSFBT");
        strCmdText = @"Select A.Account as XName,SUM(A.ConfirmAmount) as YNumber 
                  From T_ProExpense A LEFT JOIN T_ProjectBudget B ON A.ProjectID = B.ProjectID AND A.Account = B.Account 
                      WHERE  A.ProjectID = " + strProjectID + " Group By A.Account ";
        IFrame_Chart_Expense.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("RWZTFBT");
        strCmdText = "select Status as XName,count(*) as YNumber from T_ProjectTask ";
        strCmdText += " where ProjectID = " + strProjectID + " Group By Status";
        IFrame_Chart_TaskStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("XQZTFBT");
        strCmdText = "select Status as XName,count(*) as YNumber from T_Requirement ";
        strCmdText += " where ReqID in (select ReqID from T_RelatedReq where ProjectID  = " + strProjectID + ")  Group By Status";
        IFrame_Chart_RequirementStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);


        strChartTitle = LanguageHandle.GetWord("GZLZTFBT");
        strHQL = "select Status as XName, Count(*) as YNumber from T_WorkFlow where ";
        strHQL += " ((RelatedType = 'Project' and RelatedID = " + strProjectID + ")";
        strHQL += " or (RelatedType = 'ExpenseApply' and RelatedID in (select ID from T_ExpenseApplyWL where RelatedID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'ExpenseClaim' and RelatedID in (select ECID from T_ExpenseClaim where RelatedID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'Requirement' and RelatedID in (select ReqID from T_RelatedReq where ProjectID = " + strProjectID + "))";
        strHQL += "or (RelatedType = 'Risk' and RelatedID in (select ID from T_ProjectRisk where ProjectID =" + strProjectID + "))";  
        strHQL += " or (RelatedType = 'Task' and RelatedID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'Plan' and RelatedID in (select ID From T_ImplePlan where ProjectID = " + strProjectID + "))";
        strHQL += "or (RelatedType = 'Meeting' and RelatedID in (select ID from T_Meeting where RelatedID =" + strProjectID + ")))";  
        strHQL += " Group By Status ";
        IFrame_Chart_WorkFlowStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);


        strChartTitle = LanguageHandle.GetWord("FXZTFBT");
        strCmdText = "select Status as XName,count(*) as YNumber from T_ProjectRisk ";
        strCmdText += " where ProjectID = " + strProjectID + " Group By Status";
        IFrame_Chart_RiskStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);
    }

    protected void LoadProjectBudget(string strProjectID)
    {
        string strHQL;
        IList lst;

        decimal deBudget = 0;

        strHQL = "From ProjectBudget as projectBudget Where projectBudget.ProjectID = " + strProjectID;
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);

        ProjectBudget projectBudget = new ProjectBudget();

        for (int i = 0; i < lst.Count; i++)
        {
            projectBudget = (ProjectBudget)lst[i];
            deBudget += projectBudget.Amount;
        }

        LB_ProBudget.Text = deBudget.ToString();
        LB_ConfirmProExpense.Text = GetProjectExpense(strProjectID);
    }


    protected decimal GetProjectAccountTotalAmount(string strProjectID, string strAccount)
    {
        string strHQL;

        strHQL = "Select Sum(ConfirmAmount) From T_ProExpense Where ProjectID = " + strProjectID + " and Account = " + "'" + strAccount + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");

        try
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        catch
        {
            return 0;
        }
    }
    protected string GetProjectExpense(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(ConfirmAmount),0) From T_ProExpense Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");


        try
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch
        {
            return "0";
        }
    }
}
