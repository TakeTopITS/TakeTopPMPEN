using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class TTProExpense : System.Web.UI.Page
{
    string strProjectID, strTaskID, strRecordID, strQuestionID, strPlanID, strWorkflowStepDetailID, strTaskStatus, strProjectStatus;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strUserName, strProjectName;
        string strReviewType;
        decimal deExpense = 0, deConfirmExpense = 0;

        Project project = new Project();

        strProjectID = Request.QueryString["ProjectID"];
        if (strProjectID == null)
        {
            strProjectID = "1";
        }

        //strProjectID = "481";

        strTaskID = Request.QueryString["TaskID"];
        strRecordID = Request.QueryString["RecordID"];

        strQuestionID = Request.QueryString["QuestionID"];
        if (strQuestionID == null)
        {
            strQuestionID = "0";
        }

        strPlanID = Request.QueryString["PlanID"];
        if (strPlanID == null)
        {
            strPlanID = "0";
        }

        strWorkflowStepDetailID = Request.QueryString["WorkflowStepDetailID"];
        if (strWorkflowStepDetailID == null)
        {
            strWorkflowStepDetailID = "0";
        }

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTProExpenseSAAS.aspx?ProjectID=" + strProjectID + "&TaskID=" + strTaskID + "&PlanID=" + strPlanID + "&WorkflowStepDetailID=" + strWorkflowStepDetailID + "&RecordID=" + strRecordID + "&QuestionID=" + strQuestionID);
        }

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        project = GetProject(strProjectID);
        strProjectName = project.ProjectName.Trim();
        strProjectStatus = project.Status.Trim();
        LB_ProBudget.Text = project.Budget.ToString();
        LB_CurrencyType.Text = project.CurrencyType.Trim();
        LB_ExpenseCurrencyType.Text = project.CurrencyType.Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        if (Page.IsPostBack == false)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            DLC_EffectDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strReviewType = "ExpenseReimbursement";
            strReviewType = "%" + strReviewType + "%";


            if (strProjectID != "1")
            {
                strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Visible = 'YES' and workFlowTemplate.TemName in ";
                strHQL += "(Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Project' and relatedWorkFlowTemplate.RelatedID = " + strProjectID + ")";
                strHQL += " and workFlowTemplate.Type like " + "'" + strReviewType + "'";
                strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
            
                WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
                lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
                DL_TemName.DataSource = lst;
                DL_TemName.DataBind();
            }
            else
            {
                ShareClass.LoadWFTemplate(strUserCode, "ExpenseReimbursement", DL_TemName);
            }

            //取得会计科目列表
            ShareClass.LoadAccountForDDL(DL_Account);

            if (strTaskID != "0")
            {
                strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.TaskID = " + "'" + strTaskID + "'" + " and proExpense.UserCode = " + "'" + strUserCode + "'" + " order by proExpense.ID DESC";
            }
            else
            {
                if (strQuestionID != "0")
                {
                    strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strUserCode + "'" + " and QuestionID = " + strQuestionID + " order by proExpense.ID DESC";
                }
                else
                {
                    if (strWorkflowStepDetailID != "0")
                    {
                        strHQL = "from ProExpense as proExpense where  proExpense.UserCode = " + "'" + strUserCode + "'" + " and proExpense.WorkflowID = " + strWorkflowStepDetailID + " order by proExpense.ID DESC";
                    }
                    else
                    {
                        if (strPlanID != "0")
                        {
                            strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strUserCode + "'" + " and proExpense.PlanID = " + strPlanID + " order by proExpense.ID DESC";
                        }
                        else
                        {
                            strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strUserCode + "'" + " order by proExpense.ID DESC";
                        }
                    }
                }
            }


            ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
            lst = proExpenseBLL.GetAllProExpenses(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            ProExpense proExpense = new ProExpense();
            for (int i = 0; i < lst.Count; i++)
            {
                proExpense = (ProExpense)lst[i];
                deExpense += proExpense.Amount;
                deConfirmExpense += proExpense.ConfirmAmount;
            }

            LB_Amount.Text = deExpense.ToString();
            LB_ConfirmAmount.Text = deConfirmExpense.ToString();

            LB_ProjectID.Text = strProjectID;
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;

            //把已在报销清单的记录表现为蓝色
            ColorClaimedExpenseRecord(-1);

            if (strTaskID != "0" & strTaskID != null)
            {
                strTaskStatus = GetProjectTaskStatus(strTaskID);
                if (strTaskStatus == "Closed")
                {
                    BT_New.Enabled = false;
                }
            }

            if (strProjectStatus == "CaseClosed" || strProjectStatus == "Suspended" || strProjectStatus == "Cancel")
            {
                BT_New.Enabled = false;
            }

            LB_Sql.Text = strHQL;
            LoadProExpense(strHQL);

            //费用报销         
            LoadExpenseClaimSheed(strProjectID, strUserCode);
            if (strProjectID != "1")
            {
                TB_ExpenseName.Text = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("FeiYongBaoXiaoShenQing").ToString().Trim();
            }
        }
    }

    protected void CB_AllClaimed_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            if (((CheckBox)DataGrid1.Items[i].FindControl("CB_IsClaimed")).Enabled == true)
            {
                if (CB_AllClaimed.Checked == true)
                {
                    ((CheckBox)DataGrid1.Items[i].FindControl("CB_IsClaimed")).Checked = true;
                }
                else
                {
                    ((CheckBox)DataGrid1.Items[i].FindControl("CB_IsClaimed")).Checked = false;
                }
            }
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strRegisterDate;
            string strUserCode = LB_UserCode.Text.Trim();
            decimal deAmount;
            string strECID, strProjectTaskID, strFinancialStaffCode;
            string strStatus;
            int intWLNumber;

            string strID = e.Item.Cells[2].Text.Trim();

            strHQL = "from ProExpense as proExpense where proExpense.ID = " + strID;
            ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
            IList lst = proExpenseBLL.GetAllProExpenses(strHQL);
            ProExpense proExpense = (ProExpense)lst[0];

            if (e.CommandName == "Update")
            {
                LB_ID.Text = proExpense.ID.ToString();
                LB_ExpenseID.Text = proExpense.ID.ToString();
                lbl_AccountCode.Text = proExpense.AccountCode.Trim();
                TB_Account.Text = proExpense.Account.Trim();
                TB_Description.Text = proExpense.Description.Trim();
                DLC_EffectDate.Text = proExpense.EffectDate.ToString("yyyy-MM-dd");
                NB_Amount.Amount = proExpense.Amount;
                LB_CurrencyType.Text = proExpense.CurrencyType.Trim();
                strFinancialStaffCode = proExpense.FinancialStaffCode.Trim();

                //判断是否为任务费用     
                if (strTaskID != "0")
                {
                    strProjectTaskID = e.Item.Cells[8].Text.Trim();

                    try
                    {
                        strTaskStatus = GetProjectTaskStatus(strProjectTaskID);
                        if (strTaskStatus == "Closed")
                        {
                            //BT_New.Enabled = false;
                            //BT_Update.Enabled = false;
                            //BT_Delete.Enabled = false;
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    strProjectTaskID = e.Item.Cells[10].Text.Trim();
                    if (strProjectTaskID != "0")
                    {
                        strTaskStatus = GetProjectTaskStatus(strProjectTaskID);
                        if (strTaskStatus == "Closed")
                        {
                            BT_New.Enabled = false;

                            //BT_Update.Enabled = false;
                            //BT_Delete.Enabled = false;
                        }
                    }
                }

                if (strProjectStatus == "CaseClosed" || strProjectStatus == "Suspended" || strProjectStatus == "Cancel")
                {
                    BT_New.Enabled = false;
                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;

                    if (e.CommandName == "Claim")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCXMYJAHGHXBNBXFYL").ToString().Trim() + "')", true);
                    }
                }
                else
                {
                }

                if (GetExpenseClaimRecordCount(strID) > 0)
                {
                    //BT_Delete.Enabled = false;
                    //BT_Update.Enabled = false;
                }
                else
                {
                    //BT_Update.Enabled = true;
                    //BT_Delete.Enabled = true;
                }

                strRegisterDate = proExpense.RegisterDate.ToString("yyyyMMdd");
                if (strRegisterDate == DateTime.Now.ToString("yyyyMMdd") || strFinancialStaffCode == "")
                {
                    //BT_Update.Enabled = true;
                    //BT_Delete.Enabled = true;
                }
                else
                {
                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;
                }


                //把已在报销清单的记录表现为蓝色
                ColorClaimedExpenseRecord(e.Item.ItemIndex);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strProjectID;

                strUserCode = proExpense.UserCode;
                strProjectID = proExpense.ProjectID.ToString();

                try
                {
                    proExpenseBLL.DeleteProExpense(proExpense);

                    LoadAndUpdateProExpense(LB_Sql.Text.Trim());

                    //BT_Delete.Enabled = false;
                    //BT_Update.Enabled = false;

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }

            if (e.CommandName == "Claim")
            {
                strECID = LB_ECID.Text.Trim();
                strStatus = LB_Status.Text;

                if (strECID == "")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXZBXDCNBCJLTJDBXDZ").ToString().Trim() + "')", true);

                    //把已在报销清单的记录表现为蓝色
                    ColorClaimedExpenseRecord(e.Item.ItemIndex);

                    if (GetExpenseClaimRecordCount(strID) > 0)
                    {
                        //BT_Delete.Enabled = false;
                        //BT_Update.Enabled = false;
                    }
                    else
                    {
                        //BT_Update.Enabled = true;
                        //BT_Delete.Enabled = true;
                    }

                    return;
                }

                intWLNumber = GetRelatedWorkFlowNumber("ExpenseReimbursement", "Other", strECID);
                if (intWLNumber > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCBXDYZSPZBNZXZXDBXFYJC").ToString().Trim() + "')", true);

                    //把已在报销清单的记录表现为蓝色
                    ColorClaimedExpenseRecord(e.Item.ItemIndex);

                    if (GetExpenseClaimRecordCount(strID) > 0)
                    {
                        //BT_Delete.Enabled = false;
                        //BT_Update.Enabled = false;
                    }
                    else
                    {
                        //BT_Update.Enabled = true;
                        //BT_Delete.Enabled = true;
                    }

                    return;
                }

                if (GetExpenseClaimRecordCount(strID) == 0)
                {
                    ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
                    ExpenseClaimDetail expenseClaimDetail = new ExpenseClaimDetail();

                    expenseClaimDetail.ECID = int.Parse(LB_ECID.Text.Trim());
                    expenseClaimDetail.RelatedExpenseID = int.Parse(strID);
                    expenseClaimDetail.Account = proExpense.Account.Trim();
                    expenseClaimDetail.Description = proExpense.Description.Trim();
                    expenseClaimDetail.RelatedType = "Project";
                    expenseClaimDetail.RelatedID = int.Parse(strProjectID);
                    expenseClaimDetail.Amount = proExpense.ConfirmAmount;
                    expenseClaimDetail.UserCode = strUserCode;
                    expenseClaimDetail.UserName = LB_UserName.Text.Trim();
                    expenseClaimDetail.RegisterDate = proExpense.EffectDate;

                    try
                    {
                        expenseClaimDetailBLL.AddExpenseClaimDetail(expenseClaimDetail);
                        LoadExpenseClaimDetail(strECID);

                        deAmount = CountExpenseClaimAmount(strECID);
                        NB_ClaimAmount.Amount = deAmount;
                        UPdateExpenseClaimAmount(strECID, deAmount);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCBXDYZSPZBNZXZXDBXFYJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ExpenseID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strExpenseID;

        strExpenseID = LB_ExpenseID.Text.Trim();

        if (strExpenseID == "")
        {
            AddExpense();
        }
        else
        {
            UpdateExpense();
        }
    }


    protected void AddExpense()
    {
        string strID;
        string strProjectID = LB_ProjectID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strUserName = LB_UserName.Text.Trim();
        string strAccount = TB_Account.Text.Trim();
        string strDescription = TB_Description.Text.Trim();
        string strCurrencyType = LB_CurrencyType.Text.Trim();
        decimal deAmount = NB_Amount.Amount;

        DateTime dtEffectDate = DateTime.Parse(DLC_EffectDate.Text);

        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        ProExpense proExpense = new ProExpense();

        proExpense.ProjectID = int.Parse(strProjectID);
        proExpense.TaskID = int.Parse(strTaskID);
        proExpense.RecordID = int.Parse(strRecordID);
        proExpense.QuestionID = int.Parse(strQuestionID);
        proExpense.PlanID = int.Parse(strPlanID);
        proExpense.WorkflowID = int.Parse(strWorkflowStepDetailID);

        proExpense.UserCode = strUserCode;
        proExpense.UserName = strUserName;
        proExpense.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        proExpense.Account = strAccount;
        proExpense.Description = strDescription;
        proExpense.Amount = deAmount;

        proExpense.ConfirmAmount = deAmount;
        proExpense.CurrencyType = LB_CurrencyType.Text.Trim();
        proExpense.FinancialStaffCode = "";
        proExpense.FinancialStaffName = "";

        proExpense.EffectDate = dtEffectDate;
        proExpense.RegisterDate = DateTime.Now;

        if (strProjectID == "1" & strQuestionID == "0")
        {
            try
            {
                proExpenseBLL.AddProExpense(proExpense);
                strID = ShareClass.GetMyCreatedMaxProExpenseID(strProjectID, strUserCode);
                LB_ExpenseID.Text = strID;
                LB_ID.Text = strID;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                LoadAndUpdateProExpense(LB_Sql.Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            //检查相应科目项目预算有没有超支 //检查相应科目项目预算有没有超支
            if (ShareClass.CheckProjectExpenseBudget(strProjectID, strAccount, deAmount) == false)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZFYCGKMYSHXMZYSJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }

            try
            {
                proExpenseBLL.AddProExpense(proExpense);
                strID = ShareClass.GetMyCreatedMaxProExpenseID(strProjectID, strUserCode);
                LB_ExpenseID.Text = strID;
                LB_ID.Text = strID;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                LoadAndUpdateProExpense(LB_Sql.Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void UpdateExpense()
    {
        string strID, strHQL;
        IList lst;

        string strUserCode;
        string strProjectID, strAccount, strDescription;
        decimal deAmount;

        strID = LB_ID.Text;
        strAccount = TB_Account.Text.Trim();
        strDescription = TB_Description.Text.Trim();
        deAmount = NB_Amount.Amount;

        strHQL = "from ProExpense as proExpense where proExpense.ID = " + strID;
        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        lst = proExpenseBLL.GetAllProExpenses(strHQL);
        ProExpense proExpense = (ProExpense)lst[0];

        proExpense.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        proExpense.Account = strAccount;
        proExpense.Description = strDescription;
        proExpense.Amount = deAmount;

        proExpense.ConfirmAmount = deAmount;
        proExpense.CurrencyType = LB_CurrencyType.Text.Trim();

        proExpense.EffectDate = DateTime.Parse(DLC_EffectDate.Text);
        strUserCode = proExpense.UserCode;
        strProjectID = proExpense.ProjectID.ToString();

        if (strProjectID == "1" & strQuestionID == "0")
        {
            try
            {
                proExpenseBLL.UpdateProExpense(proExpense, int.Parse(strID));
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXCG").ToString().Trim() + "')", true);

                LoadAndUpdateProExpense(LB_Sql.Text.Trim());

                strUserCode = proExpense.UserCode;
                strProjectID = proExpense.ProjectID.ToString();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            //检查相应科目项目预算有没有超支 //检查相应科目项目预算有没有超支
            if (ShareClass.CheckProjectExpenseBudget(strProjectID, strAccount, deAmount) == false)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZFYCGKMYSHXMZYSJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }

            try
            {
                proExpenseBLL.UpdateProExpense(proExpense, int.Parse(strID));
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                LoadAndUpdateProExpense(LB_Sql.Text.Trim());

                strUserCode = proExpense.UserCode;
                strProjectID = proExpense.ProjectID.ToString();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        string strECID;
        IList lst;
        int intWLNumber = 0;
        string strWLStatus;

        if (e.CommandName != "Page")
        {
            TableClaimDetail.Visible = true;

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strECID = e.Item.Cells[3].Text.Trim();
            LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strECID));

            intWLNumber = GetRelatedWorkFlowNumber("ExpenseReimbursement", "Other", strECID);
            if (intWLNumber > 0)
            {
                BT_SubmitApply.Enabled = false;
                strWLStatus = GetWorkFlowStatus("ExpenseReimbursement", "Other", strECID);

                //BT_NewClaim.Enabled = true;
                //BT_UpdateClaim.Enabled = false;
                //BT_DeleteClaim.Enabled = false;
                BT_SubmitApply.Enabled = false;
            }
            else
            {
                //BT_NewClaim.Enabled = true;
                //BT_UpdateClaim.Enabled = true;
                //BT_DeleteClaim.Enabled = true;
                BT_SubmitApply.Enabled = true;
            }

            if (strProjectStatus == "CaseClosed" | strProjectStatus == "Suspended" | strProjectStatus == "Cancel")
            {
                //BT_NewClaim.Enabled = false;
                //BT_UpdateClaim.Enabled = false;
                //BT_DeleteClaim.Enabled = false;
                BT_SubmitApply.Enabled = false;
            }


            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
                ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
                lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);
                ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

                LB_ECID.Text = strECID;
                TB_ExpenseName.Text = expenseClaim.ExpenseName.Trim();
                NB_ClaimAmount.Amount = expenseClaim.Amount;
                LB_CurrencyType.Text = expenseClaim.CurrencyType.Trim();
                TB_Purpose.Text = expenseClaim.Purpose.Trim();
                LB_Status.Text = expenseClaim.Status.Trim();

                LB_Sql2.Text = strHQL;

                LoadExpenseClaimDetail(strECID);


                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popClaimWindow','true') ", true);
                }

                if (e.CommandName == "Assign")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                string strUserCode = LB_UserCode.Text.Trim();

                strHQL = " delete from T_ExpenseClaim where ECID = " + strECID;
                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadExpenseClaimSheed(strProjectID, strUserCode);

                    //BT_UpdateClaim.Enabled = false;
                    //BT_DeleteClaim.Enabled = false;
                    BT_SubmitApply.Enabled = false;

                    LB_ECID.Text = "";

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_Claim_Click(object sender, EventArgs e)
    {
        int j = 0;
        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            if (((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Checked == true & ((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Enabled == true)
            {
                j = 1;
            }
        }

        if (j == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("QingXianXuanZheYaoBaoXiaoDeFeiYong").ToString().Trim() + "')", true);
            return;
        }


        LB_ECID.Text = "";

        NB_ClaimAmount.Amount = getNewClaimAmount();

        LoadExpenseClaimDetail("0");

        TableClaimDetail.Visible = false;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popClaimWindow','false') ", true);
    }

    protected void BT_NewClaim_Click(object sender, EventArgs e)
    {
        string strECID;

        strECID = LB_ECID.Text.Trim();

        if (strECID == "")
        {
            AddClaim();
        }
        else
        {
            UpdateClaim();
        }


        LoadProExpense(LB_Sql.Text);
    }

    protected decimal getNewClaimAmount()
    {
        string strID;
        decimal deAmount = 0;
        ProExpense proExpense = new ProExpense();

        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            if (((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Checked == true & ((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Enabled == true)
            {
                strID = DataGrid1.Items[i].Cells[2].Text.Trim();

                proExpense = GetProExpense(strID);

                if (GetExpenseClaimRecordCount(strID) == 0)
                {
                    deAmount += proExpense.Amount;
                }
            }
        }

        return deAmount;
    }


    protected void BT_NewClaimAndSummitReview_Click(object sender, EventArgs e)
    {
        string strECID;

        strECID = LB_ECID.Text.Trim();

        if (strECID == "")
        {
            AddClaim();
        }
        else
        {
            UpdateClaim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop2222", "popShow('popAssignWindow','true') ", true);
    }

    protected void AddClaim()
    {
        string strECID, strExpenseName, strPurpose;
        string strUserCode = LB_UserCode.Text.Trim();

        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        ExpenseClaim expenseClaim = new ExpenseClaim();

        expenseClaim.RelatedType = "Project";
        expenseClaim.RelatedID = int.Parse(strProjectID);
        expenseClaim.ExpenseName = strExpenseName;
        expenseClaim.Purpose = strPurpose;
        expenseClaim.Amount = 0;
        expenseClaim.CurrencyType = LB_CurrencyType.Text.Trim();
        expenseClaim.ApplyTime = DateTime.Now;
        expenseClaim.ApplicantCode = strUserCode;
        expenseClaim.ApplicantName = LB_UserName.Text.Trim();
        expenseClaim.Status = "New";

        //string strHQL;
        //strHQL = string.Format(@"Insert Into T_ExpenseClaim(RelatedType,RelatedID,ExpenseName,Purpose,Amount,CurrencyType,ApplyTime,ApplicantCode,ApplicantName,Status)
        //                       Values('{0}','{1}','{2}','{3}',{4},'{5}',{6},'{7}','{8}','{9}')", "Project", int.Parse(strProjectID), strExpenseName, strPurpose, 0, LB_CurrencyType.Text.Trim(),
        //                       DateTime.Now, strUserCode, LB_UserName.Text.Trim(), "New");
        //LogClass.WriteLogFile(strHQL);

        //ShareClass.RunSqlCommand(strHQL);
                                


        try
        {
            expenseClaimBLL.AddExpenseClaim(expenseClaim);

            strECID = ShareClass.GetMyCreatedMaxExpenseClaimWLID(strUserCode);
            LB_ECID.Text = strECID;

            BT_SubmitApply.Enabled = true;

            LB_Status.Text = "New";

            string strID;
            decimal deAmount;
            ProExpense proExpense = new ProExpense();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                if (((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Checked == true & ((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Enabled == true)
                {
                    strID = DataGrid1.Items[i].Cells[2].Text.Trim();

                    proExpense = GetProExpense(strID);

                    if (GetExpenseClaimRecordCount(strID) == 0)
                    {
                        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
                        ExpenseClaimDetail expenseClaimDetail = new ExpenseClaimDetail();

                        expenseClaimDetail.ECID = int.Parse(strECID);
                        expenseClaimDetail.RelatedExpenseID = int.Parse(strID);
                        expenseClaimDetail.Account = proExpense.Account.Trim();
                        expenseClaimDetail.Description = proExpense.Description.Trim();
                        expenseClaimDetail.RelatedType = "Project";
                        expenseClaimDetail.RelatedID = int.Parse(strProjectID);
                        expenseClaimDetail.Amount = proExpense.ConfirmAmount;
                        expenseClaimDetail.UserCode = strUserCode;
                        expenseClaimDetail.UserName = LB_UserName.Text.Trim();
                        expenseClaimDetail.RegisterDate = proExpense.EffectDate;

                        try
                        {
                            expenseClaimDetailBLL.AddExpenseClaimDetail(expenseClaimDetail);

                        }
                        catch
                        {
                        }
                    }
                }
            }

            LoadExpenseClaimDetail(strECID);
            deAmount = CountExpenseClaimAmount(strECID);
            NB_ClaimAmount.Amount = deAmount;
            UPdateExpenseClaimAmount(strECID, deAmount);

            LoadExpenseClaimSheed(strProjectID, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateClaim()
    {
        string strHQL;
        string strExpenseName, strPurpose, strStatus;
        decimal deAmount;
        string strUserCode = LB_UserCode.Text.Trim();
        string strECID = LB_ECID.Text.Trim();
        IList lst;

        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();
        deAmount = NB_ClaimAmount.Amount;
        strStatus = LB_Status.Text;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);
        ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

        expenseClaim.ExpenseName = strExpenseName;
        expenseClaim.Purpose = strPurpose;
        expenseClaim.Amount = deAmount;
        expenseClaim.CurrencyType = LB_CurrencyType.Text.Trim();
        expenseClaim.ApplyTime = DateTime.Now;
        expenseClaim.Status = strStatus;

        try
        {
            expenseClaimBLL.UpdateExpenseClaim(expenseClaim, int.Parse(strECID));
            LoadExpenseClaimSheed(strProjectID, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }


    protected string SubmitApply()
    {
        string strUserCode;
        string strExpenseName, strPurpose, strCmdText;
        decimal deAmount;
        string strECID, strXMLFileName, strXMLFile2;
        string strWLID;

        strWLID = "0";

        strUserCode = LB_UserCode.Text.Trim();
        strECID = LB_ECID.Text.Trim();
        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();
        deAmount = NB_ClaimAmount.Amount;

        XMLProcess xmlProcess = new XMLProcess();

        string strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        IList lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];
        expenseClaim.ExpenseName = strExpenseName;
        expenseClaim.Purpose = strPurpose;
        expenseClaim.Amount = deAmount;
        expenseClaim.Status = "InProgress";

        try
        {
            expenseClaimBLL.UpdateExpenseClaim(expenseClaim, int.Parse(strECID));

            LB_Status.Text = "InProgress";

            strXMLFileName = "ExpenseReimbursement" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = strExpenseName;
            workFlow.WLType = "ExpenseReimbursement";
            workFlow.Status = "New";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = ShareClass.GetUserName(strUserCode);
            workFlow.Description = expenseClaim.Purpose;
            workFlow.XMLFile = strXMLFile2;
            workFlow.RelatedType = "Other";
            workFlow.RelatedID = int.Parse(strECID);
            workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

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

                strCmdText = "select ECID as DetailECID, * from T_ExpenseClaim where ECID = " + strECID;
                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_ExpenseClaim", strXMLFile2);

                LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strECID));

                //BT_UpdateClaim.Enabled = false;
                //BT_DeleteClaim.Enabled = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFYBXGZLSCCGDGZLGLYMC").ToString().Trim() + "')", true);
            }
            catch
            {
                strWLID = "0";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFYBXGZLSCSB").ToString().Trim() + "')", true);
            }

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
            string strURL = "TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID;
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop2222", "top.frames[0].frames[2].parent.frames['rightTabFrame'].popShowByURL('" + strURL + "','WorkFlow'title, 800, 600,window.location) ", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            Decimal deAmount = 0;
            string strStatus = LB_Status.Text;
            string strID;
            string strHQL;

            string strECID = LB_ECID.Text.Trim();
            int intWLNumber = GetRelatedWorkFlowNumber("ExpenseReimbursement", "Other", strECID);

            if (intWLNumber > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCBXDYZSPZBNSCFYTMJC").ToString().Trim() + "')", true);
                return;
            }

            strID = e.Item.Cells[0].Text.Trim();
            strHQL = " from ExpenseClaimDetail as expenseClaimDetail where expenseClaimDetail.ID = " + strID;
            ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
            IList lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);
            ExpenseClaimDetail expenseClaimDetail = (ExpenseClaimDetail)lst[0];

            try
            {
                expenseClaimDetailBLL.DeleteExpenseClaimDetail(expenseClaimDetail);
                LoadExpenseClaimDetail(strECID);

                deAmount = CountExpenseClaimAmount(strECID);
                NB_ClaimAmount.Amount = deAmount;
                UPdateExpenseClaimAmount(strECID, deAmount);
            }
            catch
            {
            }

            //把已在报销清单的记录表现为蓝色
            ColorClaimedExpenseRecord(e.Item.ItemIndex);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popClaimWindow','false') ", true);

        }
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        string strAccountCode;
        string strUserCode;
        strUserCode = Session["UserCode"].ToString();

        if (ExelToDBTestForItem() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim() ;
            return;
        }
        else
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() ;
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() ;
                return;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() ;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() ;
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strAccountCode = dr[i][LanguageHandle.GetWord("KeMuDaiMa").ToString().Trim()].ToString().Trim();

                        if (strAccountCode != "")
                        {
                            ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
                            ProExpense proExpense = new ProExpense();

                            try
                            {
                                proExpense.ProjectID = int.Parse(strProjectID);
                                proExpense.AccountCode = strAccountCode;
                                proExpense.Account = dr[i][LanguageHandle.GetWord("KeMuMingChen").ToString().Trim()].ToString().Trim();
                                proExpense.Description = dr[i][LanguageHandle.GetWord("YongTu").ToString().Trim()].ToString().Trim();
                                proExpense.Amount = decimal.Parse(dr[i][LanguageHandle.GetWord("JinE").ToString().Trim()].ToString().Trim());
                                proExpense.ConfirmAmount = decimal.Parse(dr[i][LanguageHandle.GetWord("JinE").ToString().Trim()].ToString().Trim());
                                proExpense.CurrencyType = dr[i]["Currency"].ToString().Trim();   
                                proExpense.EffectDate = DateTime.Parse(dr[i][LanguageHandle.GetWord("FaShengRiJi").ToString().Trim()].ToString().Trim());
                                proExpense.RegisterDate = DateTime.Now;

                                proExpense.UserCode = dr[i][LanguageHandle.GetWord("DangShiRenDaiMa").ToString().Trim()].ToString().Trim();
                                proExpense.UserName = dr[i][LanguageHandle.GetWord("DangShiRenXingMing").ToString().Trim()].ToString().Trim();

                                proExpense.FinancialStaffCode = "";
                                proExpense.FinancialStaffName = "";

                                proExpense.TaskID = 0;
                                proExpense.BMPUPayID = 0;
                                proExpense.ConstractPayID = 0;
                                proExpense.GoodsPOPayID = 0;
                                proExpense.PlanID = 0;
                                proExpense.QuestionID = 0;
                                proExpense.RecordID = 0;
                                proExpense.WorkflowID = 0;


                                proExpenseBLL.AddProExpense(proExpense);

                                continue;
                            }
                            catch (Exception err)
                            {
                                LB_ErrorText.Text += "Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace;
                            }

                        }
                    }

                    LoadProExpense(LB_Sql.Text.Trim());

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected int ExelToDBTestForItem()
    {
        int j = 0;

        string strAccountCode;
        string strUserCode;
        strUserCode = Session["UserCode"].ToString();

        try
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
                j = -1;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() ;
                j = -1;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() ;
                j = -1;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() ;
                    j = -1;
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strAccountCode = dr[i][LanguageHandle.GetWord("KeMuDaiMa").ToString().Trim()].ToString().Trim();

                        if (strAccountCode != "")
                        {
                            ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
                            ProExpense proExpense = new ProExpense();

                            try
                            {
                                proExpense.UserCode = dr[i][LanguageHandle.GetWord("DangShiRenDaiMa").ToString().Trim()].ToString().Trim();
                                proExpense.UserName = dr[i][LanguageHandle.GetWord("DangShiRenXingMing").ToString().Trim()].ToString().Trim();

                                if (proExpense.UserCode != strUserCode)
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCuoWuNiBuNengDaoRuBieRenDeFe").ToString().Trim()+"')", true);
                                    return -1;
                                }

                                proExpense.ProjectID = int.Parse(strProjectID);
                                proExpense.AccountCode = strAccountCode;
                                proExpense.Account = dr[i][LanguageHandle.GetWord("KeMuMingChen").ToString().Trim()].ToString().Trim();
                                proExpense.Description = dr[i][LanguageHandle.GetWord("YongTu").ToString().Trim()].ToString().Trim();
                                proExpense.Amount = decimal.Parse(dr[i][LanguageHandle.GetWord("JinE").ToString().Trim()].ToString().Trim());
                                proExpense.ConfirmAmount = decimal.Parse(dr[i][LanguageHandle.GetWord("JinE").ToString().Trim()].ToString().Trim());
                                proExpense.CurrencyType = dr[i]["Currency"].ToString().Trim();   
                                proExpense.EffectDate = DateTime.Parse(dr[i][LanguageHandle.GetWord("FaShengRiJi").ToString().Trim()].ToString().Trim());
                                proExpense.RegisterDate = DateTime.Now;

                                proExpense.FinancialStaffCode = "";
                                proExpense.FinancialStaffName = "";

                                proExpense.TaskID = 0;
                                proExpense.BMPUPayID = 0;
                                proExpense.ConstractPayID = 0;
                                proExpense.GoodsPOPayID = 0;
                                proExpense.PlanID = 0;
                                proExpense.QuestionID = 0;
                                proExpense.RecordID = 0;
                                proExpense.WorkflowID = 0;
                            }
                            catch (Exception err)
                            {
                                LB_ErrorText.Text += "Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace;

                                j = -1;

                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZDRSBSLBLSLYWSZ").ToString().Trim() + "')", true);
                                continue;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception err)
        {
            LB_ErrorText.Text += err.Message.ToString() + "<br/>"; ;

            j = -1;
        }

        return j;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        IList lst = proExpenseBLL.GetAllProExpenses(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        //把已在报销清单的记录表现为蓝色
        ColorClaimedExpenseRecord(-1);
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql2.Text;
        IList lst;

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.RelatedType='Project' and expenseClaim.RelatedID=" + strProjectID + " and expenseClaim.ApplicantCode=" + "'" + strUserCode + "'" + " Order by expenseClaim.ECID DESC";
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void LoadAndUpdateProExpense(string strHQL)
    {
        decimal deExpense = 0, deConfirmExpense = 0, deCurrentDayExpense = 0, deCurrentDayConfirmExpense = 0;
        decimal deTaskAssignRecordExpense = 0, deWorkFlowExpense = 0, dePlanExpense = 0;
        string strRegisterDate;
        string strUserCode = LB_UserCode.Text.Trim();

        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        IList lst = proExpenseBLL.GetAllProExpenses(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        ProExpense proExpense = new ProExpense();

        for (int i = 0; i < lst.Count; i++)
        {
            proExpense = (ProExpense)lst[i];
            deExpense += proExpense.Amount;
            deConfirmExpense += proExpense.ConfirmAmount;
            strRegisterDate = proExpense.RegisterDate.ToString("yyyy/MM/dd");

            if (strRegisterDate == DateTime.Now.ToString("yyyy/MM/dd"))
            {
                deCurrentDayExpense += proExpense.Amount;
                deCurrentDayConfirmExpense += proExpense.ConfirmAmount;
            }

            if (strRecordID != "0")
            {
                if (strRecordID == proExpense.RecordID.ToString())
                {
                    deTaskAssignRecordExpense += proExpense.ConfirmAmount;
                }
            }

            if (strPlanID != "0")
            {
                if (strPlanID == proExpense.PlanID.ToString())
                {
                    dePlanExpense += proExpense.ConfirmAmount;
                }
            }

            if (strWorkflowStepDetailID != "0")
            {
                if (strWorkflowStepDetailID == proExpense.WorkflowID.ToString())
                {
                    deWorkFlowExpense += proExpense.ConfirmAmount;
                }
            }
        }

        LB_CurrentDayAmount.Text = deCurrentDayExpense.ToString();
        LB_Amount.Text = deExpense.ToString();
        LB_ConfirmAmount.Text = deConfirmExpense.ToString();

        UpdateDailyExpense(strUserCode, strProjectID, deCurrentDayExpense, deCurrentDayConfirmExpense);

        if (strRecordID != "0")
        {
            UpdatedTaskAssignRecordCharge(strRecordID, deTaskAssignRecordExpense);
        }

        if (strTaskID != "0")
        {
            UpdateProjectTaskCharge(strTaskID);
        }

        if (strWorkflowStepDetailID != "0")
        {
            UpdateWorkFlowCharge(strWorkflowStepDetailID, deWorkFlowExpense);
        }

        if (strPlanID != "0")
        {
            strHQL = "Update T_ImplePlan Set Expense = " + (decimal.Parse(ShareClass.GetTotalRealExpenseByPlan(strPlanID)).ToString());
            strHQL += " Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);
        }

        //把已在报销清单的记录表现为蓝色
        ColorClaimedExpenseRecord(-1);
    }

    protected void UpdateDailyExpense(string strUserCode, string strProjectID, decimal DeCurrentDayAmount, decimal DeCurrentDayConfirmAmount)
    {
        string strHQL;
        IList lst;
        int intID;

        strHQL = "from DailyWork as dailyWork where dailyWork.UserCode = " + "'" + strUserCode + "'" + " and dailyWork.ProjectID = " + strProjectID + " and to_char(dailyWork.WorkDate,'yyyymmdd') = to_char(now(),'yyyymmdd')";
        DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
        lst = dailyWorkBLL.GetAllDailyWorks(strHQL);
        DailyWork dailyWork = new DailyWork();

        if (lst.Count == 0)
        {
            //if (strUserCode == ShareClass.GetProjectPMCode(strProjectID))
            //{
            //    dailyWork.Type = "Lead";
            //}
            //else
            //{
            //    dailyWork.Type = "Participate";
            //}
            //dailyWork.UserCode = strUserCode;
            //dailyWork.UserName = LB_UserName.Text.Trim();
            //dailyWork.WorkDate = DateTime.Now;
            //dailyWork.ProjectID = int.Parse(strProjectID);
            //dailyWork.ProjectName = ShareClass.GetProjectName(strProjectID);
            //dailyWork.DailySummary = LanguageHandle.GetWord("ShuRuXiangMuXiangGuanFeiYong").ToString().Trim();
            //dailyWork.Charge = DeCurrentDayAmount;
            //dailyWork.ConfirmCharge = DeCurrentDayConfirmAmount;
            //dailyWork.ManHour = 0;
            //dailyWork.FinishPercent = 0;
            //dailyWork.ConfirmManHour = 0;

            if (strUserCode == ShareClass.GetProjectPMCode(strProjectID))
            {
                dailyWork.Type = "Lead";   
            }
            else
            {
                dailyWork.Type = "Participate";  
            }
            dailyWork.UserCode = strUserCode;
            dailyWork.UserName = ShareClass.GetUserName(strUserCode);
            dailyWork.WorkDate = DateTime.Now;
            dailyWork.RecordTime = DateTime.Now;
            dailyWork.Address = "";
            dailyWork.ProjectID = int.Parse(strProjectID);
            dailyWork.ProjectName = ShareClass.GetProjectName(strProjectID);
            dailyWork.DailySummary = LanguageHandle.GetWord("ShuRuXiangMuXiangGuanFeiYong").ToString().Trim();
            dailyWork.Achievement = "";
            dailyWork.Charge = DeCurrentDayAmount;
            dailyWork.ConfirmCharge = DeCurrentDayConfirmAmount;
            dailyWork.ManHour = 0;
            dailyWork.ConfirmManHour = 0;
            dailyWork.Salary = 0;
            dailyWork.FinishPercent = 0;
            dailyWork.Bonus = 0;
            dailyWork.ConfirmBonus = 0;
            dailyWork.Authority = "NO";

            try
            {
                dailyWorkBLL.AddDailyWork(dailyWork);
            }
            catch (Exception err)
            {
            }
        }
        else
        {
            dailyWork = (DailyWork)lst[0];
            intID = dailyWork.WorkID;

            dailyWork.Charge = DeCurrentDayAmount;
            dailyWork.ConfirmCharge = DeCurrentDayConfirmAmount;

            try
            {
                dailyWorkBLL.UpdateDailyWork(dailyWork, intID);
            }
            catch
            {
            }
        }
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

        string strECID = LB_ECID.Text.Trim();
        string strStatus = LB_Status.Text;

        if (strECID != "")
        {
            strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
            ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
            lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

            ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

            expenseClaim.Status = strStatus;

            try
            {
                expenseClaimBLL.UpdateExpenseClaim(expenseClaim, int.Parse(strECID));
            }
            catch
            {
            }
        }
    }

    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        string strHQL, strKeyWord;
        IList lst;

        strKeyWord = TB_KeyWord.Text.Trim();
        strKeyWord = "%" + strKeyWord + "%";


        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Project' and relatedWorkFlowTemplate.RelatedID = " + strProjectID + ")";
        strHQL += " and workFlowTemplate.TemName like " + "'" + strKeyWord + "'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
     
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void UpdateProjectPlanCharge(string strPlanID, decimal deExpense)
    {
        string strHQL;

        strHQL = "Update T_ImplePlan Set Expense = " + deExpense;
        strHQL += " Where PlanID = " + strPlanID;

        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateWorkFlowCharge(string strWLDetailID, decimal deExpense)
    {
        string strHQL;

        strHQL = "Update T_WorkFlowStepDetail Set Expense = " + deExpense.ToString();
        strHQL += " Where ID = " + strWLDetailID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_WorkFlow Set Expense = " + getWorkFlowTotalCharge(strWLDetailID);
        strHQL += " Where WLID = " + getWorkFlowID(strWLDetailID);
        ShareClass.RunSqlCommand(strHQL);
    }

    protected string getWorkFlowTotalCharge(string strWLDetailID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(Expense),0) From T_WorkFlowStepDetail Where WLID = " + getWorkFlowID(strWLDetailID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepdetail");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    protected string getWorkFlowID(string strWLDetailID)
    {
        string strHQL;

        strHQL = "Select WLID From T_WorkFlowStepDetail Where ID = " + strWLDetailID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepdetail");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    protected void UpdatedTaskAssignRecordCharge(string strID, decimal deTaskAssignRecordExpense)
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];

        taskAssignRecord.Expense = deTaskAssignRecordExpense;

        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));

            UpdateProjectTaskCharge(taskAssignRecord.TaskID.ToString());
        }
        catch
        {
        }
    }

    protected void UpdateProjectTaskCharge(string strTaskID)
    {
        string strHQL;

        string strTotalTaskExpense;

        strHQL = "Select COALESCE(Sum(Expense),0) From T_TaskAssignRecord Where TaskID = " + strTaskID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

        strTotalTaskExpense = ds.Tables[0].Rows[0][0].ToString();

        strHQL = "Update T_ProjectTask Set Expense = " + strTotalTaskExpense + " Where TaskID = " + strTaskID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UPdateExpenseClaimStatus(string strECID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);
        ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

        expenseClaim.Status = strStatus;

        try
        {
            expenseClaimBLL.UpdateExpenseClaim(expenseClaim, int.Parse(strECID));
        }
        catch
        {
        }
    }


    protected void LoadProExpense(string strHQL)
    {
        decimal deExpense = 0, deCurrentDayExpense = 0, deTaskAssignRecordExpense = 0;
        string strRegisterDate, strID;
        string strUserCode = LB_UserCode.Text.Trim();

        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        IList lst = proExpenseBLL.GetAllProExpenses(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        ProExpense proExpense = new ProExpense();

        for (int i = 0; i < lst.Count; i++)
        {
            proExpense = (ProExpense)lst[i];
            deExpense += proExpense.Amount;
            strRegisterDate = proExpense.RegisterDate.ToString("yyyy/MM/dd");
            strID = proExpense.RecordID.ToString();

            if (strRegisterDate == DateTime.Now.ToString("yyyy/MM/dd"))
            {
                deCurrentDayExpense += proExpense.ConfirmAmount;
            }

            if (strRecordID != "0" & strRecordID != null)
            {
                if (strRecordID == strID)
                {
                    deTaskAssignRecordExpense += proExpense.ConfirmAmount;
                }
            }
        }

        LB_CurrentDayAmount.Text = deCurrentDayExpense.ToString();
        LB_Amount.Text = deExpense.ToString();

        //UpdateDailyExpense(strUserCode, strProjectID, deCurrentDayExpense);

        if (strRecordID != "0" & strRecordID != null)
        {
            UpdatedTaskAssignRecordCharge(strRecordID, deTaskAssignRecordExpense);
        }

        //把已在报销清单的记录表现为蓝色
        ColorClaimedExpenseRecord(-1);
    }

    protected void LoadExpenseClaimSheed(string strProjectID, string strApplicantCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.RelatedType='Project' and expenseClaim.RelatedID=" + strProjectID + " and expenseClaim.ApplicantCode=" + "'" + strApplicantCode + "'" + " Order by expenseClaim.ECID DESC";
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void LoadExpenseClaimDetail(string strECID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaimDetail as expenseClaimDetail where expenseClaimDetail.ECID = " + strECID;
        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
        lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected int GetExpenseClaimRecordCount(string strExpenseID)
    {
        string strHQL = "from ExpenseClaimDetail as expenseClaimDetail where expenseClaimDetail.RelatedType='Project' and expenseClaimDetail.RelatedExpenseID = " + strExpenseID;
        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
        IList lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);

        return lst.Count;
    }

    protected void UPdateExpenseClaimAmount(string strECID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);
        ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

        expenseClaim.Amount = deAmount;

        try
        {
            expenseClaimBLL.UpdateExpenseClaim(expenseClaim, int.Parse(strECID));
        }
        catch
        {
        }
    }

    protected decimal CountExpenseClaimAmount(string strECID)
    {
        string strHQL;
        IList lst;
        Decimal deAmount = 0, deTotalAmount = 0;

        strHQL = "from ExpenseClaimDetail as expenseClaimDetail where  expenseClaimDetail.ECID = " + strECID;
        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
        lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);
        ExpenseClaimDetail expenseClaimDetail = new ExpenseClaimDetail();

        for (int i = 0; i < lst.Count; i++)
        {
            expenseClaimDetail = (ExpenseClaimDetail)lst[i];
            deAmount = expenseClaimDetail.Amount;

            deTotalAmount += deAmount;
        }

        return deTotalAmount;
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

    protected void LoadWLType()
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'ExpenseReimbursement'";
        strHQL += " and ((workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Project' and relatedWorkFlowTemplate.RelatedID = " + strProjectID + "))";
        strHQL += " or ( workFlowTemplate.Authority = 'All' ))";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected void ColorClaimedExpenseRecord(int intItemIndex)
    {
        string strID;

        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            strID = DataGrid1.Items[i].Cells[2].Text.Trim();

            if (GetExpenseClaimRecordCount(strID) > 0)
            {
                DataGrid1.Items[i].ForeColor = Color.Green;

                ((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Checked = true;
                ((CheckBox)(DataGrid1.Items[i].FindControl("CB_IsClaimed"))).Enabled = false;
            }
            else
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
        }

        if (intItemIndex > -1)
        {
            DataGrid1.Items[intItemIndex].ForeColor = Color.Red;
        }
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

    protected string GetWorkFlowStatus(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow.Status.Trim();
    }

    protected Project GetProject(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project;
    }

    protected ProExpense GetProExpense(string strID)
    {
        string strHQL;

        strHQL = "from ProExpense as proExpense where proExpense.ID = " + strID;
        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        IList lst = proExpenseBLL.GetAllProExpenses(strHQL);
        ProExpense proExpense = (ProExpense)lst[0];

        return proExpense;
    }

    protected string GetProjectTaskStatus(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID= " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.Status.Trim();
    }

    protected string GetProjectStatus(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.Status.Trim();
    }

    protected string GetProjectTaskName(string strTaskId)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID= " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.Task.Trim();
    }

    protected string GetCustomerQuestionName(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        return customerQuestion.Question.Trim();
    }


}
