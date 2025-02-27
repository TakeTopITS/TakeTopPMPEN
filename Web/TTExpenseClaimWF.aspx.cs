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
using System.Data.SqlClient;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTExpenseClaimWF : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strRelatedType, strRelatedID;

    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        //从流程中打开的业务单
        strToDoWLID = Request.QueryString["WLID"]; strToDoWLDetailID= Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        if (ShareClass.IsMobileDeviceCheckAgent() & Session["IsMobileDevice"].ToString() == "YES")
        {
            Response.Redirect("TTAPPExpenseClaimWF.aspx");
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_RegisterDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //取得会计科目列表
            ShareClass.LoadAccountForDDL(DL_Account);
            ShareClass.LoadCurrencyType(DL_CurrencyType);

            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;

            //费用报销
            ShareClass.LoadWFTemplate(strUserCode, "ExpenseReimbursement", DL_TemName);

            LoadExpenseClaimSheed(strUserCode);
        }
    }
    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        string strECID;
        IList lst;
        int intWLNumber = 0;

        if (e.CommandName != "Page")
        {
            strECID = e.Item.Cells[3].Text.Trim();

            intWLNumber = LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strECID));
            if (intWLNumber > 0)
            {
                BT_NewClaim.Visible = false;
                BT_New.Visible = false;

                BT_SubmitApply.Enabled = false;
            }
            else
            {
                BT_NewClaim.Visible = true;
                BT_New.Visible = true;

                BT_SubmitApply.Enabled = true;
            }

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("ExpenseReimbursement", "Other", strECID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                BT_NewClaim.Visible = true;
                BT_New.Visible = true;

            }

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
                ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
                lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);
                ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

                LB_ECID.Text = expenseClaim.ECID.ToString();
                TB_ExpenseName.Text = expenseClaim.ExpenseName.Trim();
                NB_ClaimAmount.Amount = expenseClaim.Amount;
                DL_CurrencyType.SelectedValue = expenseClaim.CurrencyType;
                TB_Purpose.Text = expenseClaim.Purpose.Trim();
                LB_Status.Text = expenseClaim.Status.Trim();

                LoadExpenseClaimDetail(strECID);

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
                string strUserCode = LB_UserCode.Text.Trim();

                if (intWLNumber > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCZGLDGZLJLBNSCJC").ToString().Trim() + "')", true);
                    return;
                }

                strHQL = " delete from T_ExpenseClaim where ECID = " + strECID;
                try
                {
                    ShareClass.RunSqlCommand(strHQL);
                    LoadExpenseClaimSheed(strUserCode);

                    LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strECID));

                    LoadExpenseClaimDetail(strECID);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_CreateClaim_Click(object sender, EventArgs e)
    {
        LB_ECID.Text = "";

        BT_NewClaim.Visible = true;
        BT_New.Visible = true;

        LoadExpenseClaimDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
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
    }

    protected void AddClaim()
    {
        string strExpenseID, strExpenseName, strPurpose;
        string strUserCode = LB_UserCode.Text.Trim();

        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        ExpenseClaim expenseClaim = new ExpenseClaim();

        expenseClaim.RelatedType = "Other";
        expenseClaim.RelatedID = 0;
        expenseClaim.ExpenseName = strExpenseName;
        expenseClaim.Purpose = strPurpose;
        expenseClaim.Amount = 0;
        expenseClaim.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        expenseClaim.ApplyTime = DateTime.Now;
        expenseClaim.ApplicantCode = strUserCode;
        expenseClaim.ApplicantName = LB_UserName.Text.Trim();
        expenseClaim.Status = "New";

        try
        {
            expenseClaimBLL.AddExpenseClaim(expenseClaim);
            strExpenseID = ShareClass.GetMyCreatedMaxExpenseClaimWLID(strUserCode);
            LB_ECID.Text = strExpenseID;

            LoadExpenseClaimSheed(strUserCode);
            LoadExpenseClaimDetail(strExpenseID);

            LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strExpenseID));

            LB_Status.Text = "New";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJSBKNFYMCHYTZSGDJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateClaim()
    {
        string strHQL, strStatus;
        string strExpenseName, strPurpose;
        decimal deAmount;

        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strECID = LB_ECID.Text.Trim();

        strStatus = LB_Status.Text;
        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();
        deAmount = NB_ClaimAmount.Amount;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strECID;
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);
        ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

        expenseClaim.ExpenseName = strExpenseName;
        expenseClaim.Purpose = strPurpose;
        expenseClaim.Amount = deAmount;
        expenseClaim.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        expenseClaim.ApplyTime = DateTime.Now;
        expenseClaim.Status = strStatus;

        try
        {
            expenseClaimBLL.UpdateExpenseClaim(expenseClaim, int.Parse(strECID));
            LoadExpenseClaimSheed(strUserCode);

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("ExpenseReimbursement", "Other", strECID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select ECID as DetailECID, * from T_ExpenseClaim where ECID = " + strECID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("ExpenseReimbursement", "Other", strECID);
                }

                if (strToDoWLID != null)
                {
                    if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }
            }

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
        string strUserCode;
        string strExpenseName, strPurpose, strCmdText;
        decimal deAmount;
        string strECID, strXMLFileName, strXMLFile2;
        string strWLID, strTemName;

        strWLID = "0";

        strUserCode = LB_UserCode.Text.Trim();
        strECID = LB_ECID.Text.Trim();

        strExpenseName = TB_ExpenseName.Text.Trim();
        strPurpose = TB_Purpose.Text.Trim();
        deAmount = NB_ClaimAmount.Amount;

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim() + "');</script>");

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

            return "0";
        }

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

            if (strRelatedType != "" & strRelatedType != null)
            {
                workFlow.RelatedType = strRelatedType;
                workFlow.RelatedID = int.Parse(strRelatedID);
            }
            else
            {
                workFlow.RelatedType = "Other";
                workFlow.RelatedID = expenseClaim.ECID;
            }
            workFlow.RelatedCode = "ExpenseCA" + strECID;

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

                strCmdText = "select ECID as DetailECID, * from T_ExpenseClaim where ECID = " + strECID;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_ExpenseClaim", strXMLFile2);

                LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strECID));

                LB_Status.Text = "InProgress";

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFYBXGZLSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                strWLID = "0";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFYBXGZLSCSBKNFYMCZSGDJC").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            strWLID = "0";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

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

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            int intWLNumber;
            string strStatus = LB_Status.Text;
            string strECID = LB_ECID.Text.Trim();

            intWLNumber = LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strECID));
            if (intWLNumber > 0)
            {
                BT_NewClaim.Visible = false;
                BT_New.Visible = false;
                BT_SubmitApply.Enabled = false;
            }
            else
            {
                BT_NewClaim.Visible = true;
                BT_New.Visible = true;
                BT_SubmitApply.Enabled = true;
            }

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("ExpenseReimbursement", "Other", strECID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                BT_NewClaim.Visible = true;
                BT_New.Visible = true;
            }


            string strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strHQL = " from ExpenseClaimDetail as expenseClaimDetail where expenseClaimDetail.ID = " + strID;

                ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
                IList lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);
                ExpenseClaimDetail expenseClaimDetail = (ExpenseClaimDetail)lst[0];

                LB_DetailID.Text = expenseClaimDetail.ID.ToString();
                lbl_AccountCode.Text = expenseClaimDetail.AccountCode.Trim();
                TB_Account.Text = expenseClaimDetail.Account.Trim();
                TB_Description.Text = expenseClaimDetail.Description.Trim();
                NB_Amount.Amount = expenseClaimDetail.Amount;
                DLC_RegisterDate.Text = expenseClaimDetail.RegisterDate.ToString("yyyy-MM-dd");

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCZGLDGZLJLBNSCJC").ToString().Trim() + "')", true);
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                    return;
                }

                Decimal deAmount = 0;

                string strHQL = " from ExpenseClaimDetail as expenseClaimDetail where expenseClaimDetail.ID = " + strID;
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

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("ExpenseRequest", "Other", strID, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText;

                        strCmdText = "select ECID as DetailECID, * from T_ExpenseClaim where ECID = " + strECID;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("ExpenseReimbursement", "Other", strECID);
                        }

                        if (strToDoWLID != null)
                        {
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }

                        if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                        {
                            strCmdText = "select * from T_ExpenseClaimDetail where ECID = " + strECID;
                            ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }
                    }
                }
                catch
                {
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_DetailID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
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

        strECID = LB_ECID.Text.Trim();
        int intWLNumber = LoadRelatedWL("ExpenseReimbursement", "Other", int.Parse(strECID));
        if (intWLNumber > 0 & strToDoWLID == null)
        {
            BT_SubmitApply.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCZGLDGZLJLBNSCJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        else
        {
            BT_SubmitApply.Enabled = true;
        }

        string strDetailID;

        strDetailID = LB_DetailID.Text.Trim();

        if (strDetailID == "")
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
        string strECID = LB_ECID.Text.Trim();
        string strStatus = LB_Status.Text;
        decimal deAmount = 0, deTotalAmount = 0, deBalanceAmount = 0;
        string strExpenseID;

        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        string strAccountName = TB_Account.Text.Trim();
        deAmount = NB_Amount.Amount;

        //存在部门预算的话，判断预算是否足够
        deBalanceAmount = ShareClass.GetBMBaseDataMoneyNum(strDepartCode, strAccountName, DateTime.Now.Year, DateTime.Now.Month, "Base") - deAmount;
        if (ShareClass.IsBMBaseDataExits("", strDepartCode, strAccountName, DateTime.Now.Year, DateTime.Now.Month, strUserCode))
        {
            if (deBalanceAmount < 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBMGNFYFYSBZQJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

                return;
            }
        }

        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
        ExpenseClaimDetail expenseClaimDetail = new ExpenseClaimDetail();

        expenseClaimDetail.ECID = int.Parse(LB_ECID.Text.Trim());
        expenseClaimDetail.RelatedExpenseID = 0;
        expenseClaimDetail.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        expenseClaimDetail.Account = TB_Account.Text.Trim();
        expenseClaimDetail.Description = TB_Description.Text.Trim();
        expenseClaimDetail.RelatedType = "Other";
        expenseClaimDetail.RelatedID = int.Parse(strECID);
        expenseClaimDetail.Amount = NB_Amount.Amount;
        expenseClaimDetail.UserCode = LB_UserCode.Text.Trim();
        expenseClaimDetail.UserName = LB_UserName.Text.Trim();
        expenseClaimDetail.RegisterDate = DateTime.Parse(DLC_RegisterDate.Text);

        try
        {
            expenseClaimDetailBLL.AddExpenseClaimDetail(expenseClaimDetail);
            strExpenseID = ShareClass.GetMyCreatedMaxExpenseClaimDetailID(LB_UserCode.Text.Trim());
            LoadExpenseClaimDetail(strECID);

            deTotalAmount = CountExpenseClaimAmount(strECID);
            NB_ClaimAmount.Amount = deTotalAmount;
            UPdateExpenseClaimAmount(strECID, deTotalAmount);

            LB_DetailID.Text = strExpenseID;

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("ExpenseReimbursement", "Other", strECID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText;

                strCmdText = "select ECID as DetailECID, * from T_ExpenseClaim where ECID = " + strECID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("ExpenseReimbursement", "Other", strECID);
                }

                if (strToDoWLID != null)
                {
                    if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }

                if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                {
                    strCmdText = "select * from T_ExpenseClaimDetail where ECID = " + strECID;
                    ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
    }

    protected void UpdateExpense()
    {
        string strStatus = LB_Status.Text;
        string strID = LB_DetailID.Text.Trim();
        string strECID = LB_ECID.Text.Trim();

        decimal deAmount = 0, deTotalAmount = 0, deBalanceAmount = 0;

        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        string strAccountName = TB_Account.Text.Trim();
        deAmount = NB_Amount.Amount;

        //存在部门预算的话，判断预算是否足够
        deBalanceAmount = ShareClass.GetBMBaseDataMoneyNum(strDepartCode, strAccountName, DateTime.Now.Year, DateTime.Now.Month, "Base") - deAmount;
        if (ShareClass.IsBMBaseDataExits("", strDepartCode, strAccountName, DateTime.Now.Year, DateTime.Now.Month, strUserCode))
        {
            if (deBalanceAmount < 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBMGNFYFYSBZQJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

                return;
            }
        }


        string strHQL = " from ExpenseClaimDetail as expenseClaimDetail where expenseClaimDetail.ID = " + strID;
        ExpenseClaimDetailBLL expenseClaimDetailBLL = new ExpenseClaimDetailBLL();
        IList lst = expenseClaimDetailBLL.GetAllExpenseClaimDetails(strHQL);
        ExpenseClaimDetail expenseClaimDetail = (ExpenseClaimDetail)lst[0];

        expenseClaimDetail.Amount = NB_Amount.Amount;
        expenseClaimDetail.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        expenseClaimDetail.Account = TB_Account.Text.Trim();
        expenseClaimDetail.Description = TB_Description.Text.Trim();

        try
        {
            expenseClaimDetailBLL.UpdateExpenseClaimDetail(expenseClaimDetail, int.Parse(strID));
            LoadExpenseClaimDetail(strECID);

            deTotalAmount = CountExpenseClaimAmount(strECID);
            NB_ClaimAmount.Amount = deTotalAmount;
            UPdateExpenseClaimAmount(strECID, deTotalAmount);

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("ExpenseRequest", "Other", strID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText;

                strCmdText = "select ECID as DetailECID, * from T_ExpenseClaim where ECID = " + strECID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("ExpenseReimbursement", "Other", strECID);
                }

                if (strToDoWLID != null)
                {
                    if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }

                if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                {
                    strCmdText = "select * from T_ExpenseClaimDetail where ECID = " + strECID;
                    ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }


    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'ExpenseReimbursement'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popAssignWindow') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        IList lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DL_Account_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccountCode = DL_Account.SelectedValue.Trim();
        lbl_AccountCode.Text = strAccountCode;
        TB_Account.Text = ShareClass.GetAccountName(strAccountCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void LoadExpenseClaimSheed(string strApplicantCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.RelatedType='Other'  and expenseClaim.ApplicantCode=" + "'" + strApplicantCode + "'" + " Order by expenseClaim.ECID DESC";

        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ECID = " + strWLBusinessID;
        }

        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadExpenseClaimDetail(string strECID)
    {
        string strHQL;

        try
        {
            strHQL = "Select * From T_ExpenseClaimDetail Where ECID = " + strECID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ExpenseClaimDetail");

            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
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

    protected int LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        return lst.Count;
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
}
