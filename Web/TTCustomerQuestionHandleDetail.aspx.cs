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

public partial class TTCustomerQuestionHandleDetail : System.Web.UI.Page
{
    string strUserCode, strUserName, strOperatorCode;
    string strQuestionID;
    string strProjectID;

    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strRecorderCode;
        string strOperatorStatus;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化      
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_CustomerComment);
        _FileBrowser.SetupCKEditor(HE_HandleDetail);

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strQuestionID = Request.QueryString["ID"];

        if(strQuestionID =="0")
        {
            return;
        }

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];
        strRecorderCode = customerQuestion.RecorderCode.Trim();
        strOperatorCode = customerQuestion.OperatorCode.Trim();
        strOperatorStatus = customerQuestion.OperatorStatus.Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DataList2.DataSource = lst;
            DataList2.DataBind();
            DataList1.DataSource = lst;
            DataList1.DataBind();

            LoadCustomerQuestionCustomerStage();
            LoadCustomerQuestionStage();

            if (strIsMobileDevice == "YES")
            {
                HT_CustomerComment.Visible = true;
                HT_HandleDetail.Visible = true;
            }
            else
            {
                HE_CustomerComment.Visible = true;
                HE_HandleDetail.Visible = true;
            }

            TB_CustomerAcceptor.Text = customerQuestion.ContactPerson.Trim();
            TB_AcceptorContactWay.Text = customerQuestion.PhoneNumber.Trim();
            DLC_NextServiceTime.Text = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
            NB_PreDays.Amount = 5;
            DL_IsImportant.SelectedValue = customerQuestion.IsImportant.Trim();

            DL_Stage.SelectedValue = customerQuestion.Stage.Trim();
            DL_CustomerStage.SelectedValue = customerQuestion.CustomerStage.Trim();
            NB_Possibility.Amount = customerQuestion.Possibility;

            if (strOperatorCode == "")
            {
                BT_New.Enabled = false;
                BT_Finish.Enabled = false;

                BT_DeleteQuestion.Enabled = false;
                DL_IsImportant.Enabled = false;
                BT_TransferOperator.Enabled = false;

                HL_QuestionToCustomer.Enabled = false;

                BT_Create.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
                BT_Finish.Enabled = true;

                DL_IsImportant.Enabled = true;
                BT_DeleteQuestion.Enabled = true;
                BT_TransferOperator.Enabled = true;
                HL_QuestionToCustomer.Enabled = true;

                BT_Create.Enabled = true;
            }

            if (strOperatorCode != "" & strOperatorCode != strUserCode)
            {
                BT_Accept.Enabled = false;
                BT_Exit.Enabled = false;
                BT_Finish.Enabled = false;

                BT_New.Enabled = false;

                BT_DeleteQuestion.Enabled = false;
                DL_IsImportant.Enabled = false;
                BT_TransferOperator.Enabled = false;

                HL_QuestionToCustomer.Enabled = false;

                BT_Create.Enabled = false;
            }
            else
            {
                BT_Exit.Enabled = true;
            }

            strProjectID = GetProjectIDByProExpenseQuestionID(strQuestionID);
            HL_Expense.NavigateUrl = "TTProExpense.aspx?ProjectID=" + strProjectID + "&TaskID=0&RecordID=0&QuestionID=" + strQuestionID;

            HL_RelatedDoc.NavigateUrl = "TTCustomerQuestionRelatedDoc.aspx?RelatedID=" + strQuestionID;
            HL_ResoveResultReview.NavigateUrl = "TTCustomerQuestionResultReviewWF.aspx?RelatedID=" + strQuestionID;
            HL_QuestionToCustomer.NavigateUrl = "TTQuestionToCustomer.aspx?QuestionID=" + strQuestionID;

            string strUserType = ShareClass.GetUserType(strUserCode);
            if (strUserType == "OUTER")
            {
                HL_QuestionToCustomer.Visible = false;
                HL_Expense.Visible = false;
                HL_ResoveResultReview.Visible = false;
            }

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);

            //列出直接成员
            ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_Operator);
        }
    }

    protected void DL_Stage_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strStage;

        strStage = DL_Stage.SelectedValue.Trim();

        try
        {
            strHQL = "From CustomerQuestionStage as customerQuestionStage Where customerQuestionStage.Stage = '" + strStage + "'";
            CustomerQuestionStageBLL customerQuestionStageBLL = new CustomerQuestionStageBLL();
            lst = customerQuestionStageBLL.GetAllCustomerQuestionStages(strHQL);

            CustomerQuestionStage customerQuestionStage = (CustomerQuestionStage)lst[0];

            NB_Possibility.Amount = customerQuestionStage.Possibility;
        }
        catch
        {
            NB_Possibility.Amount = 0;
        }
    }

    protected void BT_SaveBusinessStage_Click(object sender, EventArgs e)
    {
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        CustomerQuestion customerQuestion = new CustomerQuestion();

        try
        {
            string strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
            IList lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            customerQuestion = (CustomerQuestion)lst[0];

            customerQuestion.Possibility = int.Parse(NB_Possibility.Amount.ToString());
            customerQuestion.Stage = DL_Stage.SelectedValue.Trim();
            customerQuestion.CustomerStage = DL_CustomerStage.SelectedValue.Trim();

            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));

            DataList1.DataSource = lst;
            DataList1.DataBind();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString () + "')", true);
        }

    }

    protected void BT_Accept_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        customerQuestion.Status = "InProgress";
        customerQuestion.OperatorCode = strUserCode;
        customerQuestion.OperatorName = strUserName;
        customerQuestion.OperatorStatus = "Accepted";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));

            LoadCustomerQuestion(strQuestionID);

            BT_Create.Enabled = true;
            BT_New.Enabled = true;
            BT_Finish.Enabled = true;

            DL_IsImportant.Enabled = true;
            BT_TransferOperator.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Exit_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        customerQuestion.Status = "New";
        customerQuestion.OperatorCode = "";
        customerQuestion.OperatorName = "";
        customerQuestion.OperatorStatus = "";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));
            LoadCustomerQuestion(strQuestionID);

            BT_Create.Enabled = false;
            BT_New.Enabled = false;
            BT_Finish.Enabled = false;

            DL_IsImportant.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTCSLCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTCSLSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void DL_IsImportant_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        customerQuestion.IsImportant = DL_IsImportant.SelectedValue.Trim();

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));
            LoadCustomerQuestion(strQuestionID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ChengGong").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ChengGong").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Finish_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        customerQuestion.OperatorCode = strUserCode;
        customerQuestion.OperatorName = strUserName;
        customerQuestion.OperatorStatus = "Completed";
        customerQuestion.Status = "Completed";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));
            LoadCustomerQuestion(strQuestionID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteQuestion_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        customerQuestion.OperatorCode = strUserCode;
        customerQuestion.OperatorName = strUserName;
        customerQuestion.OperatorStatus = "Deleted";
        customerQuestion.Status = "Deleted";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));
            LoadCustomerQuestion(strQuestionID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_TransferOperator_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strOperatorCode = DL_Operator.SelectedValue.Trim();

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        customerQuestion.Status = "New";
        customerQuestion.OperatorCode = strOperatorCode;
        customerQuestion.OperatorName = ShareClass.GetUserName(strOperatorCode);
        customerQuestion.OperatorStatus = "";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));

            LoadCustomerQuestion(strQuestionID);

            BT_New.Enabled = true;
            BT_Finish.Enabled = true;

            //推送消息给受理人
            Msg msg = new Msg();
            string strMsg = LanguageHandle.GetWord("FuWuXuQiu").ToString().Trim() + ":" + customerQuestion.Question.Trim() + "," + LanguageHandle.GetWord("ZZYaoNiChuLi").ToString().Trim();
            msg.SendMSM("Message",strOperatorCode, strMsg, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZDCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZDSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_ContactWay_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strContactWay;

        strContactWay = DL_HandleWay.SelectedValue.Trim();

        TB_HandleWay.Text = strContactWay;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','true') ", true);
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        BT_New.Enabled = true;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddRecord();
        }
        else
        {
            UpdateRecord();
        }
    }

    protected void AddRecord()
    {
        string strRecordID, strHandleDetail, strStatus, strHandleWay, strTimeUnit, strCustomerComment, strCustomerAcceptor, strAcceptorContactWay;
        decimal deUsedTime;
        int intPreDays;
        DateTime dtNextServiceTime;

        if (strIsMobileDevice == "YES")
        {
            strHandleDetail = HT_HandleDetail.Text.Trim();
            strCustomerComment = HT_CustomerComment.Text.Trim();
        }
        else
        {
            strHandleDetail = HE_HandleDetail.Text.Trim();
            strCustomerComment = HE_CustomerComment.Text.Trim();
        }

        strStatus = DL_HandleStatus.SelectedValue.Trim();
        strHandleWay = TB_HandleWay.Text.Trim();
        strTimeUnit = DL_TimeUnit.SelectedValue.Trim();

        strCustomerAcceptor = TB_CustomerAcceptor.Text.Trim();
        strAcceptorContactWay = TB_AcceptorContactWay.Text.Trim();
        deUsedTime = NB_UsedTime.Amount;
        intPreDays = int.Parse(NB_PreDays.Amount.ToString());
        dtNextServiceTime = DateTime.Parse(DLC_NextServiceTime.Text);

        CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
        CustomerQuestionHandleRecord customerQuestionHandleRecord = new CustomerQuestionHandleRecord();

        customerQuestionHandleRecord.QuestionID = int.Parse(strQuestionID);
        customerQuestionHandleRecord.HandleDetail = strHandleDetail;
        customerQuestionHandleRecord.HandleStatus = strStatus;
        customerQuestionHandleRecord.HandleWay = strHandleWay;
        customerQuestionHandleRecord.TimeUnit = strTimeUnit;
        customerQuestionHandleRecord.CustomerComment = strCustomerComment;
        customerQuestionHandleRecord.CustomerAcceptor = strCustomerAcceptor;
        customerQuestionHandleRecord.AcceptorContactWay = strAcceptorContactWay;
        customerQuestionHandleRecord.UsedTime = deUsedTime;
        customerQuestionHandleRecord.HandleTime = DateTime.Now;
        customerQuestionHandleRecord.PreDays = intPreDays;
        customerQuestionHandleRecord.NextServiceTime = dtNextServiceTime;
        customerQuestionHandleRecord.OperatorCode = strUserCode;
        customerQuestionHandleRecord.OperatorName = ShareClass.GetUserName(strUserCode);

        customerQuestionHandleRecord.SumApplyTime = DateTime.Now;
        customerQuestionHandleRecord.CollectionTime = DateTime.Now;
        customerQuestionHandleRecord.NewTime = DateTime.Now;
        customerQuestionHandleRecord.RebatesTime = DateTime.Now;
        customerQuestionHandleRecord.UpDoorDate = DateTime.Now;


        try
        {
            customerQuestionHandleRecordBLL.AddCustomerQuestionHandleRecord(customerQuestionHandleRecord);
            strRecordID = ShareClass.GetMyCreatedMaxcustomerQuestionDetailID(strQuestionID);
            LB_ID.Text = strRecordID;


            BT_New.Enabled = true;

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateRecord()
    {
        string strHQL;
        IList lst;

        string strID, strHandleDetail, strStatus, strHandleWay, strTimeUnit, strCustomerComment, strCustomerAcceptor, strAcceptorContactWay;
        decimal deUsedTime;
        int intPreDays;
        DateTime dtNextServiceTime;

        strID = LB_ID.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strHandleDetail = HT_HandleDetail.Text.Trim();
            strCustomerComment = HT_CustomerComment.Text.Trim();
        }
        else
        {
            strHandleDetail = HE_HandleDetail.Text.Trim();
            strCustomerComment = HE_CustomerComment.Text.Trim();
        }

        strStatus = DL_HandleStatus.SelectedValue.Trim();
        strHandleWay = TB_HandleWay.Text.Trim();
        strTimeUnit = DL_TimeUnit.SelectedValue.Trim();
        strCustomerAcceptor = TB_CustomerAcceptor.Text.Trim();

        strAcceptorContactWay = TB_AcceptorContactWay.Text.Trim();
        deUsedTime = NB_UsedTime.Amount;
        intPreDays = int.Parse(NB_PreDays.Amount.ToString());
        dtNextServiceTime = DateTime.Parse(DLC_NextServiceTime.Text);

        strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.ID = " + strID;
        CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
        lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);

        CustomerQuestionHandleRecord customerQuestionHandleRecord = (CustomerQuestionHandleRecord)lst[0];

        customerQuestionHandleRecord.QuestionID = int.Parse(strQuestionID);
        customerQuestionHandleRecord.HandleDetail = strHandleDetail;
        customerQuestionHandleRecord.HandleStatus = strStatus;
        customerQuestionHandleRecord.HandleWay = strHandleWay;
        customerQuestionHandleRecord.TimeUnit = strTimeUnit;
        customerQuestionHandleRecord.CustomerComment = strCustomerComment;
        customerQuestionHandleRecord.CustomerAcceptor = strCustomerAcceptor;
        customerQuestionHandleRecord.AcceptorContactWay = strAcceptorContactWay;
        customerQuestionHandleRecord.UsedTime = deUsedTime;
        customerQuestionHandleRecord.PreDays = intPreDays;
        customerQuestionHandleRecord.NextServiceTime = dtNextServiceTime;

        try
        {
            customerQuestionHandleRecordBLL.UpdateCustomerQuestionHandleRecord(customerQuestionHandleRecord, int.Parse(strID));

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','true') ", true);
        }
    }


    protected void DataList3_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        string strID, strHandleTime, strNow;
        string strHQL;
        IList lst;

        strID = ((Label)e.Item.FindControl("BT_ID")).Text.Trim();

        if (e.CommandName == "Update")
        {
            for (int i = 0; i < DataList3.Items.Count; i++)
            {
                DataList3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.ID = " + strID;
            CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
            lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);
            CustomerQuestionHandleRecord customerQuestionHandleRecord = (CustomerQuestionHandleRecord)lst[0];

            LB_ID.Text = strID;
            if (strIsMobileDevice == "YES")
            {
                HT_HandleDetail.Text = customerQuestionHandleRecord.HandleDetail.Trim();
                HT_CustomerComment.Text = customerQuestionHandleRecord.CustomerComment.Trim();
            }
            else
            {
                HE_HandleDetail.Text = customerQuestionHandleRecord.HandleDetail.Trim();
                HE_CustomerComment.Text = customerQuestionHandleRecord.CustomerComment.Trim();
            }

            TB_HandleWay.Text = customerQuestionHandleRecord.HandleWay.Trim();
            HE_HandleDetail.Text = customerQuestionHandleRecord.HandleDetail.Trim();
            DL_HandleStatus.SelectedValue = customerQuestionHandleRecord.HandleStatus.Trim();
            NB_UsedTime.Amount = customerQuestionHandleRecord.UsedTime;
            DL_TimeUnit.SelectedValue = customerQuestionHandleRecord.TimeUnit;
            TB_CustomerAcceptor.Text = customerQuestionHandleRecord.CustomerAcceptor.Trim();
            TB_AcceptorContactWay.Text = customerQuestionHandleRecord.AcceptorContactWay.Trim();

            NB_PreDays.Amount = customerQuestionHandleRecord.PreDays;
            DLC_NextServiceTime.Text = customerQuestionHandleRecord.NextServiceTime.ToString("yyyy-MM-dd");

            strHandleTime = customerQuestionHandleRecord.HandleTime.ToString("yyyyMMdd");

            strNow = DateTime.Now.ToString("yyyyMMdd");

            if (strNow != strHandleTime | strOperatorCode != strUserCode)
            {
                BT_New.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','true') ", true);
        }

        if (e.CommandName == "Delete")
        {
            strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.ID = " + strID;
            CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
            lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);
            CustomerQuestionHandleRecord customerQuestionHandleRecord = (CustomerQuestionHandleRecord)lst[0];

            try
            {
                customerQuestionHandleRecordBLL.DeleteCustomerQuestionHandleRecord(customerQuestionHandleRecord);

                LoadCustomerQuestionHandleRecord(strQuestionID);
                LoadRelatedDoc(strQuestionID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            }

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);
        }
    }

    protected void LoadCustomerQuestion(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataList2.DataSource = lst;
        DataList2.DataBind();
    }

    protected void LoadCustomerQuestionHandleRecord(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.QuestionID = " + strQuestionID + " Order by customerQuestionHandleRecord.ID DESC";
        CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
        lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);

        DataList3.DataSource = lst;
        DataList3.DataBind();
    }

    protected void LoadRelatedDoc(string strCoID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.RelatedType = 'CustomerQuestion' and document.RelatedID = " + strQuestionID;  
        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected string GetProjectIDByProExpenseQuestionID(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProExpense as proExpense where proExpense.QuestionID = " + strQuestionID;
        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        lst = proExpenseBLL.GetAllProExpenses(strHQL);

        if (lst.Count > 0)
        {
            ProExpense proExpense = (ProExpense)lst[0];
            return proExpense.ProjectID.ToString();
        }
        else
        {
            return "1";
        }
    }

    protected void LoadCustomerQuestionStage()
    {
        string strHQL;

        strHQL = "Select * From T_CustomerQuestionStage Order By Possibility ASc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerQuestionStage");

        DL_Stage.DataSource = ds;
        DL_Stage.DataBind();

        DL_Stage.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadCustomerQuestionCustomerStage()
    {
        string strHQL;

        strHQL = "Select * From T_CustomerQuestionCustomerStage Order By SortNumber ASc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerQuestionCustomerStage");

        DL_CustomerStage.DataSource = ds;
        DL_CustomerStage.DataBind();

        DL_CustomerStage.Items.Insert(0, new ListItem("--Select--", ""));
    }
}
