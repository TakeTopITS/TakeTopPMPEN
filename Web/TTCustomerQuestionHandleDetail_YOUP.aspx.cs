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

public partial class TTCustomerQuestionHandleDetail_YOUP : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strQuestionID;
    string strProjectID;

    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strRecorderCode;
        string strOperatorCode, strOperatorStatus;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化      
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        //_FileBrowser.SetupCKEditor(HE_CustomerComment);
        _FileBrowser.SetupCKEditor(HE_HandleDetail);

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strQuestionID = Request.QueryString["ID"];

        if (strQuestionID == "0")
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

        //this.Title = "客户问题：" + strQuestionID + " " + customerQuestion.Question.Trim() + " 处理";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_HandleDetail.Visible = true;
            }
            else
            {
                HE_HandleDetail.Visible = true;
            }

            DataList2.DataSource = lst;
            DataList2.DataBind();

            DataList1.DataSource = lst;
            DataList1.DataBind();

            if (ISActorGroupDetailByUserCode(strUserCode.Trim(), LanguageHandle.GetWord("DianXiaoYuan").ToString().Trim()))
            {
                DataList1.Visible = true;
                DataList2.Visible = false;

                TB_NewNum.Visible = false;
                TB_Rebates.Visible = false;
                DLC_CollectionTime.Visible = false;
                DLC_RebatesTime.Visible = false;
            }
            else
            {
                DataList1.Visible = false;
                DataList2.Visible = true;

                TB_NewNum.Visible = true;
                TB_Rebates.Visible = true;
                DLC_CollectionTime.Visible = true;
                DLC_RebatesTime.Visible = true;
            }

            DLC_CollectionTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_NewTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_RebatesTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SumApplyTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_UpDoorDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_NextServiceTime.Text = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");
            NB_PreDays.Amount = 5;

            if (strOperatorCode == "")
            {
                BT_Add.Enabled = false;
                BT_Finish.Enabled = false;
            }
            else
            {
                BT_Add.Enabled = true;
                BT_Finish.Enabled = true;
            }

            if (strOperatorCode != "" & strOperatorCode != strUserCode)
            {
                BT_Accept.Enabled = false;
                BT_Exit.Enabled = false;
                BT_Finish.Enabled = false;

                BT_Add.Enabled = false;
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            else
            {
                BT_Exit.Enabled = true;
            }

            strProjectID = GetProjectIDByProExpenseQuestionID(strQuestionID);
            HL_Expense.NavigateUrl = "TTProExpense.aspx?ProjectID=" + strProjectID + "&TaskID=0&RecordID=0&QuestionID=" + strQuestionID;


            HL_RelatedDoc.NavigateUrl = "TTCustomerQuestionRelatedDoc.aspx?RelatedID=" + strQuestionID;
            HL_ResoveResultReview.NavigateUrl = "TTCustomerQuestionResultReviewWF_YOUP.aspx?RelatedID=" + strQuestionID;
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

    /// <summary>
    /// 根据用户编码，角色组名称，判断用户所属角色组
    /// </summary>
    /// <param name="strusercode"></param>
    /// <param name="strgroupname"></param>
    /// <returns></returns>
    protected bool ISActorGroupDetailByUserCode(string strusercode, string strgroupname)
    {
        bool flag = false;
        string strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = '" + strusercode + "' and actorGroupDetail.GroupName = '" + strgroupname + "' ";
        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }

        return flag;
    }

    protected void DataList4_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string strID, strHandleTime, strNow;
        string strHQL;
        IList lst;

        if (e.CommandName == "Update")
        {
            for (int i = 0; i < DataList4.Items.Count; i++)
            {
                DataList4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.ID = " + strID;
            CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
            lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);
            CustomerQuestionHandleRecord customerQuestionHandleRecord = (CustomerQuestionHandleRecord)lst[0];

            LB_ID.Text = strID;
            if (strIsMobileDevice == "YES")
            {
                HT_HandleDetail.Text = customerQuestionHandleRecord.HandleDetail.Trim();
            }
            else
            {
                HE_HandleDetail.Text = customerQuestionHandleRecord.HandleDetail.Trim();
            }

            TB_HandleWay.Text = customerQuestionHandleRecord.HandleWay.Trim();
            DL_HandleStatus.SelectedValue = customerQuestionHandleRecord.HandleStatus.Trim();
            NB_UsedTime.Amount = customerQuestionHandleRecord.UsedTime;
            DL_TimeUnit.SelectedValue = customerQuestionHandleRecord.TimeUnit;
            TB_CustomerAcceptor.Text = customerQuestionHandleRecord.CustomerAcceptor.Trim();
            TB_AcceptorContactWay.Text = customerQuestionHandleRecord.AcceptorContactWay.Trim();
            NB_PreDays.Amount = customerQuestionHandleRecord.PreDays;
            DLC_NextServiceTime.Text = customerQuestionHandleRecord.NextServiceTime.ToString("yyyy-MM-dd");

            HE_CustomerComment.Text = customerQuestionHandleRecord.CustomerComment.Trim();
            DL_DealSituation.SelectedValue = customerQuestionHandleRecord.DealSituation.Trim();
            DLC_CollectionTime.Text = customerQuestionHandleRecord.CollectionTime.ToString("yyyy-MM-dd");
            DLC_NewTime.Text = customerQuestionHandleRecord.NewTime.ToString("yyyy-MM-dd");
            DLC_RebatesTime.Text = customerQuestionHandleRecord.RebatesTime.ToString("yyyy-MM-dd");
            DLC_SumApplyTime.Text = customerQuestionHandleRecord.SumApplyTime.ToString("yyyy-MM-dd");
            DLC_UpDoorDate.Text = customerQuestionHandleRecord.UpDoorDate.ToString("yyyy-MM-dd");
            TB_CollectionPer.Text = customerQuestionHandleRecord.CollectionPer.Trim();
            TB_Lending.Text = customerQuestionHandleRecord.Lending.Trim();
            TB_NewNum.Text = customerQuestionHandleRecord.NewNum.Trim();
            TB_Rebates.Text = customerQuestionHandleRecord.Rebates.Trim();
            TB_Signing.Text = customerQuestionHandleRecord.Signing.Trim();
            TB_ToBank.Text = customerQuestionHandleRecord.ToBank.Trim();

            TB_NewNum.Visible = false;
            TB_Rebates.Visible = false;
            DLC_CollectionTime.Visible = false;
            DLC_RebatesTime.Visible = false;

            strHandleTime = customerQuestionHandleRecord.HandleTime.ToString("yyyyMMdd");

            strNow = DateTime.Now.ToString("yyyyMMdd");

            if (strNow != strHandleTime)
            {
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            else
            {
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
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
        customerQuestion.OperatorStatus = "Accepted";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));

            LoadCustomerQuestion(strQuestionID);

            BT_Add.Enabled = true;
            BT_Finish.Enabled = true;

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
        customerQuestion.OperatorStatus = "";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));
            LoadCustomerQuestion(strQuestionID);

            BT_Add.Enabled = false;
            BT_Finish.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZTuiChuLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZTuiChuLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
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
        customerQuestion.OperatorStatus = "";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));

            LoadCustomerQuestion(strQuestionID);

            BT_Add.Enabled = true;
            BT_Finish.Enabled = true;


            //推送消息给受理人
            Msg msg = new Msg();
            string strMsg = LanguageHandle.GetWord("FuWuXuQiu").ToString().Trim() + ":" + customerQuestion.Question.Trim() + "," + LanguageHandle.GetWord("ZZYaoNiChuLi").ToString().Trim();
            msg.SendMSM("Message", strOperatorCode, strMsg, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZDCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZDSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strRecordID, strHandleDetail, strStatus, strHandleWay, strTimeUnit, strCustomerComment, strCustomerAcceptor, strAcceptorContactWay;
        decimal deUsedTime;
        int intPreDays;
        DateTime dtNextServiceTime;

        if (strIsMobileDevice == "YES")
        {
            strHandleDetail = HT_HandleDetail.Text.Trim();
        }
        else
        {
            strHandleDetail = HE_HandleDetail.Text.Trim();
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
        customerQuestionHandleRecord.CustomerComment = HE_CustomerComment.Text.Trim();
        customerQuestionHandleRecord.CustomerAcceptor = strCustomerAcceptor;
        customerQuestionHandleRecord.AcceptorContactWay = strAcceptorContactWay;
        customerQuestionHandleRecord.UsedTime = deUsedTime;
        customerQuestionHandleRecord.HandleTime = DateTime.Now;
        customerQuestionHandleRecord.PreDays = intPreDays;
        customerQuestionHandleRecord.NextServiceTime = dtNextServiceTime;
        customerQuestionHandleRecord.OperatorCode = strUserCode;
        customerQuestionHandleRecord.OperatorName = ShareClass.GetUserName(strUserCode);

        customerQuestionHandleRecord.UpDoorDate = DLC_UpDoorDate.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_UpDoorDate.Text.Trim());
        customerQuestionHandleRecord.SumApplyTime = DLC_SumApplyTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_SumApplyTime.Text.Trim());
        customerQuestionHandleRecord.NewTime = DLC_NewTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_NewTime.Text.Trim());
        customerQuestionHandleRecord.CollectionTime = DLC_CollectionTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_CollectionTime.Text.Trim());
        customerQuestionHandleRecord.RebatesTime = DLC_RebatesTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_RebatesTime.Text.Trim());
        customerQuestionHandleRecord.ToBank = TB_ToBank.Text.Trim();
        customerQuestionHandleRecord.DealSituation = DL_DealSituation.SelectedValue.Trim();
        customerQuestionHandleRecord.NewNum = TB_NewNum.Text.Trim();
        customerQuestionHandleRecord.Signing = TB_Signing.Text.Trim();
        customerQuestionHandleRecord.Lending = TB_Lending.Text.Trim();
        customerQuestionHandleRecord.CollectionPer = TB_CollectionPer.Text.Trim();
        customerQuestionHandleRecord.Rebates = TB_Rebates.Text.Trim();

        try
        {
            customerQuestionHandleRecordBLL.AddCustomerQuestionHandleRecord(customerQuestionHandleRecord);

            strRecordID = ShareClass.GetMyCreatedMaxcustomerQuestionDetailID(strQuestionID);
            LB_ID.Text = strRecordID;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
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
        }
        else
        {
            strHandleDetail = HE_HandleDetail.Text.Trim();
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
        customerQuestionHandleRecord.CustomerComment = HE_CustomerComment.Text.Trim();
        customerQuestionHandleRecord.CustomerAcceptor = strCustomerAcceptor;
        customerQuestionHandleRecord.AcceptorContactWay = strAcceptorContactWay;
        customerQuestionHandleRecord.UsedTime = deUsedTime;
        customerQuestionHandleRecord.PreDays = intPreDays;
        customerQuestionHandleRecord.NextServiceTime = dtNextServiceTime;

        customerQuestionHandleRecord.UpDoorDate = DLC_UpDoorDate.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_UpDoorDate.Text.Trim());
        customerQuestionHandleRecord.SumApplyTime = DLC_SumApplyTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_SumApplyTime.Text.Trim());
        customerQuestionHandleRecord.NewTime = DLC_NewTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_NewTime.Text.Trim());
        customerQuestionHandleRecord.CollectionTime = DLC_CollectionTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_CollectionTime.Text.Trim());
        customerQuestionHandleRecord.RebatesTime = DLC_RebatesTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_RebatesTime.Text.Trim());
        customerQuestionHandleRecord.ToBank = TB_ToBank.Text.Trim();
        customerQuestionHandleRecord.DealSituation = DL_DealSituation.SelectedValue.Trim();
        customerQuestionHandleRecord.NewNum = TB_NewNum.Text.Trim();
        customerQuestionHandleRecord.Signing = TB_Signing.Text.Trim();
        customerQuestionHandleRecord.Lending = TB_Lending.Text.Trim();
        customerQuestionHandleRecord.CollectionPer = TB_CollectionPer.Text.Trim();
        customerQuestionHandleRecord.Rebates = TB_Rebates.Text.Trim();

        try
        {
            customerQuestionHandleRecordBLL.UpdateCustomerQuestionHandleRecord(customerQuestionHandleRecord, int.Parse(strID));

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID; ;

        strID = LB_ID.Text.Trim();

        strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.ID = " + strID;
        CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
        lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);
        CustomerQuestionHandleRecord customerQuestionHandleRecord = (CustomerQuestionHandleRecord)lst[0];

        try
        {
            customerQuestionHandleRecordBLL.DeleteCustomerQuestionHandleRecord(customerQuestionHandleRecord);

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }

        LoadCustomerQuestionHandleRecord(strQuestionID);
        LoadRelatedDoc(strQuestionID);
    }

    protected void DataList3_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        string strID, strHandleTime, strNow;
        string strHQL;
        IList lst;

        if (e.CommandName == "Update")
        {
            for (int i = 0; i < DataList3.Items.Count; i++)
            {
                DataList3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.ID = " + strID;
            CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
            lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);
            CustomerQuestionHandleRecord customerQuestionHandleRecord = (CustomerQuestionHandleRecord)lst[0];

            LB_ID.Text = strID;
            if (strIsMobileDevice == "YES")
            {
                HT_HandleDetail.Text = customerQuestionHandleRecord.HandleDetail.Trim();
            }
            else
            {
                HE_HandleDetail.Text = customerQuestionHandleRecord.HandleDetail.Trim();
            }

            TB_HandleWay.Text = customerQuestionHandleRecord.HandleWay.Trim();
            DL_HandleStatus.SelectedValue = customerQuestionHandleRecord.HandleStatus.Trim();
            NB_UsedTime.Amount = customerQuestionHandleRecord.UsedTime;
            DL_TimeUnit.SelectedValue = customerQuestionHandleRecord.TimeUnit;
            TB_CustomerAcceptor.Text = customerQuestionHandleRecord.CustomerAcceptor.Trim();
            TB_AcceptorContactWay.Text = customerQuestionHandleRecord.AcceptorContactWay.Trim();
            NB_PreDays.Amount = customerQuestionHandleRecord.PreDays;
            DLC_NextServiceTime.Text = customerQuestionHandleRecord.NextServiceTime.ToString("yyyy-MM-dd");

            HE_CustomerComment.Text = customerQuestionHandleRecord.CustomerComment.Trim();
            DL_DealSituation.SelectedValue = customerQuestionHandleRecord.DealSituation.Trim();
            DLC_CollectionTime.Text = customerQuestionHandleRecord.CollectionTime.ToString("yyyy-MM-dd");
            DLC_NewTime.Text = customerQuestionHandleRecord.NewTime.ToString("yyyy-MM-dd");
            DLC_RebatesTime.Text = customerQuestionHandleRecord.RebatesTime.ToString("yyyy-MM-dd");
            DLC_SumApplyTime.Text = customerQuestionHandleRecord.SumApplyTime.ToString("yyyy-MM-dd");
            DLC_UpDoorDate.Text = customerQuestionHandleRecord.UpDoorDate.ToString("yyyy-MM-dd");
            TB_CollectionPer.Text = customerQuestionHandleRecord.CollectionPer.Trim();
            TB_Lending.Text = customerQuestionHandleRecord.Lending.Trim();
            TB_NewNum.Text = customerQuestionHandleRecord.NewNum.Trim();
            TB_Rebates.Text = customerQuestionHandleRecord.Rebates.Trim();
            TB_Signing.Text = customerQuestionHandleRecord.Signing.Trim();
            TB_ToBank.Text = customerQuestionHandleRecord.ToBank.Trim();

            TB_NewNum.Visible = true;
            TB_Rebates.Visible = true;
            DLC_CollectionTime.Visible = true;
            DLC_RebatesTime.Visible = true;

            strHandleTime = customerQuestionHandleRecord.HandleTime.ToString("yyyyMMdd");

            strNow = DateTime.Now.ToString("yyyyMMdd");

            if (strNow != strHandleTime)
            {
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            else
            {
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
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

        DataList1.DataSource = lst;
        DataList1.DataBind();

        if (ISActorGroupDetailByUserCode(strUserCode.Trim(), LanguageHandle.GetWord("DianXiaoYuan").ToString().Trim()))
        {
            DataList1.Visible = true;
            DataList2.Visible = false;
        }
        else
        {
            DataList1.Visible = false;
            DataList2.Visible = true;
        }
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

        DataList4.DataSource = lst;
        DataList4.DataBind();

        if (ISActorGroupDetailByUserCode(strUserCode.Trim(), LanguageHandle.GetWord("DianXiaoYuan").ToString().Trim()))
        {
            DataList4.Visible = true;
            DataList3.Visible = false;
        }
        else
        {
            DataList4.Visible = false;
            DataList3.Visible = true;
        }
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


}
