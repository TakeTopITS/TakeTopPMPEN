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

public partial class TTAllProjectList : System.Web.UI.Page
{
    string strUserCode, strRelatedType;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strRelatedType = Request.QueryString["RelatedType"].Trim();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            try
            {
                if (strRelatedType == "ProjectReq" | strRelatedType == "ProjectTask" | strRelatedType == "ProjectWorkFlow" | strRelatedType == "ProjectRisk" | strRelatedType == "ProjectBonus" | strRelatedType == "ProjectExpense" | strRelatedType == "ProjectIncomeAndExpense" | strRelatedType == "InAndOut" | strRelatedType == "MakeBudget" | strRelatedType == "MakeBudgetAll" | strRelatedType == "MakeItemBudget" | strRelatedType == "BudgetReport" | strRelatedType == "MaterialExpenseApply")
                {
                    LB_Scope.Text = ShareClass.InitialAllProjectRelatedPageTree(TreeView1, strUserCode, strRelatedType, "25", "All", "0", "0");
                }

                if (strRelatedType == "ProjectDoc")
                {
                    LB_Scope.Text = ShareClass.InitialAllProjectDocTree(TreeView1, strUserCode, "25", "All", "0", "0");
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCCZFXMWBSXMHZXMDXMDJLYFPXMYMGZ") + "')", true);
            }
        }
    }

    protected void BT_PirorProjectList_Click(object sender, EventArgs e)
    {
        string strQueryScope, strMaxProjectID, strMinProjectID;

        strQueryScope = LB_Scope.Text.Trim();

        if (strQueryScope != "")
        {
            strMinProjectID = strQueryScope.Substring(0, strQueryScope.IndexOf("-"));
            strMaxProjectID = strQueryScope.Replace(strMinProjectID + "-", "");

            if (strRelatedType == "ProjectReq" | strRelatedType == "ProjectTask" | strRelatedType == "ProjectWorkFlow" | strRelatedType == "ProjectRisk" | strRelatedType == "ProjectBonus" | strRelatedType == "ProjectExpense" | strRelatedType == "ProjectIncomeAndExpense" | strRelatedType == "InAndOut" | strRelatedType == "MakeBudget" | strRelatedType == "MakeBudgetAll" | strRelatedType == "MakeItemBudget" | strRelatedType == "BudgetReport" | strRelatedType == "MaterialExpenseApply")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectRelatedPageTree(TreeView1, strUserCode, strRelatedType, "25", "Piror", strMinProjectID, strMaxProjectID);
            }

            if (strRelatedType == "ProjectDoc")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectDocTree(TreeView1, strUserCode, "25", "Piror", strMinProjectID, strMaxProjectID);
            }
        }
        else
        {
            if (strRelatedType == "ProjectReq" | strRelatedType == "ProjectTask" | strRelatedType == "ProjectWorkFlow" | strRelatedType == "ProjectRisk" | strRelatedType == "ProjectBonus" | strRelatedType == "ProjectExpense" | strRelatedType == "ProjectIncomeAndExpense" | strRelatedType == "InAndOut" | strRelatedType == "MakeBudget" | strRelatedType == "MakeBudgetAll" | strRelatedType == "MakeItemBudget" | strRelatedType == "BudgetReport" | strRelatedType == "MaterialExpenseApply")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectRelatedPageTree(TreeView1, strUserCode, strRelatedType, "25", "All", "0", "0");
            }

            if (strRelatedType == "ProjectDoc")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectDocTree(TreeView1, strUserCode, "25", "All", "0", "0");
            }
        }
    }

    protected void BT_NextProjectList_Click(object sender, EventArgs e)
    {
        string strQueryScope, strMaxProjectID, strMinProjectID;

        strQueryScope = LB_Scope.Text.Trim();

        if (strQueryScope != "")
        {
            strMinProjectID = strQueryScope.Substring(0, strQueryScope.IndexOf("-"));
            strMaxProjectID = strQueryScope.Replace(strMinProjectID + "-", "");

            if (strRelatedType == "ProjectReq" | strRelatedType == "ProjectTask" | strRelatedType == "ProjectWorkFlow" | strRelatedType == "ProjectRisk" | strRelatedType == "ProjectBonus" | strRelatedType == "ProjectExpense" | strRelatedType == "ProjectIncomeAndExpense" | strRelatedType == "InAndOut" | strRelatedType == "MakeBudget" | strRelatedType == "MakeBudgetAll" | strRelatedType == "MakeItemBudget" | strRelatedType == "BudgetReport" | strRelatedType == "MaterialExpenseApply")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectRelatedPageTree(TreeView1, strUserCode, strRelatedType, "25", "Next", strMinProjectID, strMaxProjectID);
            }

            if (strRelatedType == "ProjectDoc")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectDocTree(TreeView1, strUserCode, "25", "Next", strMinProjectID, strMaxProjectID);
            }
        }
        else
        {
            if (strRelatedType == "ProjectReq" | strRelatedType == "ProjectTask" | strRelatedType == "ProjectWorkFlow" | strRelatedType == "ProjectRisk" | strRelatedType == "ProjectBonus" | strRelatedType == "ProjectExpense" | strRelatedType == "ProjectIncomeAndExpense" | strRelatedType == "InAndOut" | strRelatedType == "MakeBudget" | strRelatedType == "MakeBudgetAll" | strRelatedType == "MakeItemBudget" | strRelatedType == "BudgetReport" | strRelatedType == "MaterialExpenseApply")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectRelatedPageTree(TreeView1, strUserCode, strRelatedType, "25", "All", "0", "0");
            }

            if (strRelatedType == "ProjectDoc")
            {
                LB_Scope.Text = ShareClass.InitialAllProjectDocTree(TreeView1, strUserCode, "25", "All", "0", "0");
            }
        }

    }

}