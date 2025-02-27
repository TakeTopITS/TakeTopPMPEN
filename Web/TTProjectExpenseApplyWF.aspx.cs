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

public partial class TTProjectExpenseApplyWF : System.Web.UI.Page
{
    string strUserCode;
    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strRelatedTitle = "";

        strUserCode = Session["UserCode"].ToString();
        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";
            strRelatedTitle = GetProjectName(strRelatedID);

        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_PayBackTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //取得会计科目列表
            ShareClass.LoadAccountForDDL(DL_Account);
            ShareClass.LoadCurrencyType(DL_CurrencyType);
            DL_CurrencyType.SelectedValue = GetProjectCurrencyType(strRelatedID);

            LoadExpenseApply(strUserCode, strRelatedType, strRelatedID);

            strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Visible = 'YES' and workFlowTemplate.Type = 'ExpenseRequest'";
            strHQL += " and ((workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Project' and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + "))";
            strHQL += " or ( workFlowTemplate.Authority = 'All' ))";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();

            LB_RelatedType.Text = strRelatedType;
            LB_RelatedID.Text = strRelatedID;

            BT_SubmitApply.Enabled = false;

            TB_ExpenseName.Text = LanguageHandle.GetWord("Project").ToString().Trim() + strRelatedID + " " + strRelatedTitle + LanguageHandle.GetWord("FeiYongShenQing").ToString().Trim();
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        BT_New.Enabled = true;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddEpense();
        }
        else
        {
            UpdateExpense();
        }
    }

    protected void AddEpense()
    {
        string strExpenseID, strExpenseName, strPurpose;
        DateTime dtPayBackTime;
        decimal deAmount;

        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();
        dtPayBackTime = DateTime.Parse(DLC_PayBackTime.Text);
        deAmount = NB_Amount.Amount;

        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        ExpenseApplyWL expenseApplyWL = new ExpenseApplyWL();

        expenseApplyWL.RelatedType = strRelatedType;
        expenseApplyWL.RelatedID = int.Parse(strRelatedID);
        expenseApplyWL.ExpenseName = strExpenseName;
        expenseApplyWL.Purpose = strPurpose;
        expenseApplyWL.Amount = deAmount;
        expenseApplyWL.AccountCode = lbl_AccountCode.Text.Trim();
        expenseApplyWL.Account = TB_Account.Text.Trim();
        expenseApplyWL.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        expenseApplyWL.ApplyTime = DateTime.Now;
        expenseApplyWL.PayBackTime = dtPayBackTime;
        expenseApplyWL.ApplicantCode = strUserCode;
        expenseApplyWL.ApplicantName = ShareClass.GetUserName(strUserCode);
        expenseApplyWL.Status = "New";

        try
        {
            expenseApplyWLBLL.AddExpenseApplyWL(expenseApplyWL);

            strExpenseID = ShareClass.GetMyCreatedMaxExpenseApplyWLID(strUserCode);
            LB_ID.Text = strExpenseID;

            BT_SubmitApply.Enabled = true;

            LoadExpenseApply(strUserCode, strRelatedType, strRelatedID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateExpense()
    {
        string strExpenseName, strPurpose;
        DateTime dtPayBackTime;
        decimal deAmount;
        string strID;

        strID = LB_ID.Text.Trim();

        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();
        dtPayBackTime = DateTime.Parse(DLC_PayBackTime.Text);
        deAmount = NB_Amount.Amount;

        string strHQL = "from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.ID = " + strID;
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        IList lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        ExpenseApplyWL expenseApplyWL = (ExpenseApplyWL)lst[0];

        expenseApplyWL.ExpenseName = strExpenseName;
        expenseApplyWL.Purpose = strPurpose;
        expenseApplyWL.Amount = deAmount;
        expenseApplyWL.AccountCode = lbl_AccountCode.Text.Trim();
        expenseApplyWL.Account = TB_Account.Text.Trim();
        expenseApplyWL.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        expenseApplyWL.PayBackTime = dtPayBackTime;

        try
        {
            expenseApplyWLBLL.UpdateExpenseApplyWL(expenseApplyWL, int.Parse(strID));
            LoadExpenseApply(strUserCode, strRelatedType, strRelatedID);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
       }
    }

    protected string SubmitApply()
    {
        string strExpenseName, strPurpose, strCmdText;
        DateTime dtPayBackTime;
        decimal deAmount;
        string strID, strXMLFileName, strXMLFile2;
        string strWLID;

        strWLID = "0";

        strID = LB_ID.Text.Trim();
        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();
        dtPayBackTime = DateTime.Parse(DLC_PayBackTime.Text);
        deAmount = NB_Amount.Amount;

        XMLProcess xmlProcess = new XMLProcess();

        string strHQL = "from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.ID = " + strID;
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        IList lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        ExpenseApplyWL expenseApplyWL = (ExpenseApplyWL)lst[0];
        expenseApplyWL.ExpenseName = strExpenseName;
        expenseApplyWL.Purpose = strPurpose;
        expenseApplyWL.Amount = deAmount;
        expenseApplyWL.PayBackTime = dtPayBackTime;
        expenseApplyWL.Status = "InProgress";

        try
        {
            expenseApplyWLBLL.UpdateExpenseApplyWL(expenseApplyWL, int.Parse(strID));

            strXMLFileName = "ExpenseRequest" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = strExpenseName;
            workFlow.WLType = "ExpenseRequest";
            workFlow.Status = "New";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = ShareClass.GetUserName(strUserCode);
            workFlow.Description = expenseApplyWL.Purpose;
            workFlow.XMLFile = strXMLFile2;
            workFlow.RelatedType = "Other";
            workFlow.RelatedID = int.Parse(strID);
            workFlow.DIYNextStep = "YES"; 
            workFlow.IsPlanMainWorkflow = "NO";

            if (CB_SMS.Checked == true)
            {
                workFlow.ReceiveSMS = "YES";
            }
            else
            {
                workFlow.ReceiveSMS = "NO";
            }

            if (CB_Mail.Checked == true)
            {
                workFlow.ReceiveEMail = "YES";
            }
            else
            {
                workFlow.ReceiveEMail = "NO";
            }

            try
            {
                workFlowBLL.AddWorkFlow(workFlow);
                strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

                strCmdText = "select * from T_ExpenseApplyWL where ID = " + strID;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_ExpenseApplyWL", strXMLFile2);

                LoadRelatedWL("ExpenseRequest", "Other", int.Parse(strID));

                DL_Status.SelectedValue = "InProgress";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFYSSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                strWLID = "0";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFYSSCSB").ToString().Trim() + "')", true);
            }

            LoadExpenseApply(strUserCode, strRelatedType, strRelatedID);
        }
        catch
        {
            strWLID = "0";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }

    protected void DL_Account_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccountCode = DL_Account.SelectedValue.Trim();
        lbl_AccountCode.Text = strAccountCode;
        TB_Account.Text = ShareClass.GetAccountName(strAccountCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = LB_ID.Text.Trim();
        string strStatus = DL_Status.SelectedValue.Trim();

        if (strID != "")
        {
            strHQL = "from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.ID = " + strID;
            ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
            lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

            ExpenseApplyWL expenseApplyWL = (ExpenseApplyWL)lst[0];

            expenseApplyWL.Status = strStatus;

            try
            {
                expenseApplyWLBLL.UpdateExpenseApplyWL(expenseApplyWL, int.Parse(strID));
                LoadExpenseApply(strUserCode, strRelatedType, strRelatedID);
            }
            catch
            {
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'ExpenseRequest'";
        strHQL += " and ((workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Project' and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + "))";
        strHQL += " or ( workFlowTemplate.Authority = 'All' ))";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[3].Text.Trim();

            int intWLNumber = GetRelatedWorkFlowNumber("ExpenseRequest", "Other", strID);
            if (intWLNumber > 0)
            {
                BT_New.Enabled = false;
                BT_SubmitApply.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
                BT_SubmitApply.Enabled = true;
            }


            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.ID = " + strID;
                ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
                lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);
                ExpenseApplyWL expenseApplyWL = (ExpenseApplyWL)lst[0];

                LB_ID.Text = strID;
                TB_ExpenseName.Text = expenseApplyWL.ExpenseName;
                TB_Purpose.Text = expenseApplyWL.Purpose;
                TB_Account.Text = expenseApplyWL.Account;
                lbl_AccountCode.Text = expenseApplyWL.AccountCode;
                NB_Amount.Amount = expenseApplyWL.Amount;
                DL_CurrencyType.SelectedValue = expenseApplyWL.CurrencyType;
                DLC_PayBackTime.Text = expenseApplyWL.PayBackTime.ToString("yyyy-MM-dd");
                DL_Status.SelectedValue = expenseApplyWL.Status.Trim();

                BT_New.Enabled = true;
                BT_SubmitApply.Enabled = true;

                LoadRelatedWL("ExpenseRequest", "Other", expenseApplyWL.ID);

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Assign")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {

                if (intWLNumber > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCZGLDGZLJLBNSCJC").ToString().Trim() + "')", true);
                    return;
                }

                strHQL = "from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.ID = " + strID;
                ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
                lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

                ExpenseApplyWL expenseApplyWL = (ExpenseApplyWL)lst[0];

                try
                {
                    expenseApplyWLBLL.DeleteExpenseApplyWL(expenseApplyWL);
                    LoadExpenseApply(strUserCode, strRelatedType, strRelatedID);

             
                    BT_SubmitApply.Enabled = false;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void LoadExpenseApply(string strApplicantCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.RelatedType = " + "'" + strRelatedType + "'" + " and expenseApplyWL.RelatedID = " + strRelatedID + " and expenseApplyWL.ApplicantCode = " + "'" + strApplicantCode + "'" + " Order by expenseApplyWL.ID DESC";
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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


    protected string GetProjectName(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

    protected string GetProjectCurrencyType(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.CurrencyType.Trim();
    }

    protected string GetRequirementName(string strReqID)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();

        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement.ReqName.Trim();
    }


    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

}
