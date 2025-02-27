using System;
using System.Resources;
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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTCustomerQuestionRecord : System.Web.UI.Page
{
    string strIsMobileDevice;
    string strRelatedCustomerCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        strIsMobileDevice = Session["IsMobileDevice"].ToString();
        strRelatedCustomerCode = Request.QueryString["CustomerCode"];

        //CKEditor��ʼ��      
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(TB_Question);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "��¼�ͻ�����", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ShareClass.LoadSytemChart(strUserCode, "CustomerQuestionRecord", RP_ChartList);
        HL_SystemAnalystChartRelatedUserSet.NavigateUrl = "TTSystemAnalystChartRelatedUserSet.aspx?FormType=CustomerQuestionRecord";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_Question.Visible = true;
            }
            else
            {
                TB_Question.Visible = true;
            }

            DLC_ExpectedTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_AnswerTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TB_Company.Text = ShareClass.GetCustomerNameFromCustomerCode(strRelatedCustomerCode);

            LoadCustomerQuestionType();

            LoadCustomerQuestion(strUserCode, strRelatedCustomerCode);

            LoadCustomerQuestionCustomerStage();
            LoadCustomerQuestionStage();
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strNow, strHandleTime, strStatus;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strID = treeNode.Target.Trim();

        try
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strID;
            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

            try
            {
                DL_CustomerQuestionType.SelectedValue = customerQuestion.Type;
            }
            catch
            {
            }

            DL_IsImportant.SelectedValue = customerQuestion.IsImportant.Trim();

            if (strIsMobileDevice == "YES")
            {
                HT_Question.Text = customerQuestion.Question.Trim();
            }
            else
            {
                TB_Question.Text = customerQuestion.Question.Trim();
            }

            DLC_AnswerTime.Text = customerQuestion.AnswerTime.ToString("yyyy-MM-dd");
            TB_Company.Text = customerQuestion.Company.Trim();

            TB_ContactPerson.Text = customerQuestion.ContactPerson.Trim();
            TB_PhoneNumber.Text = customerQuestion.PhoneNumber.Trim();
            TB_EMail.Text = customerQuestion.EMail.Trim();
            TB_Address.Text = customerQuestion.Address.Trim();
            TB_PostCode.Text = customerQuestion.PostCode.Trim();


            LB_ID.Text = customerQuestion.ID.ToString();

            LoadCustomerQuestionHandleRecord(strID);

            strHandleTime = customerQuestion.SummitTime.ToString("yyyyMMdd");
            strNow = DateTime.Now.ToString("yyyyMMdd");

            strStatus = customerQuestion.Status.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        catch
        {
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_FindExistSame_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strCustomerName;

        strCustomerName = "%" + TB_Company.Text.Trim() + "%";

        if (strCustomerName != "%%")
        {
            strHQL = "From CustomerQuestion as customerQuestion Where customerQuestion.Company Like " + "'" + strCustomerName + "'";
            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            if (lst.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSCZXTMCDKH").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCZXTCCDKH").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDWMCBNWKJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        string strStatus;

        if (e.CommandName != "Page")
        {
            IList lst;

            string strID, strNow, strHandleTime;

            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update" | e.CommandName == "Detail")
            {
                try
                {
                    strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strID;
                    CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
                    lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

                    CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

                    try
                    {
                        DL_CustomerQuestionType.SelectedValue = customerQuestion.Type;
                    }
                    catch
                    {
                    }

                    DL_IsImportant.SelectedValue = customerQuestion.IsImportant.Trim();

                    if (strIsMobileDevice == "YES")
                    {
                        HT_Question.Text = customerQuestion.Question.Trim();
                    }
                    else
                    {
                        TB_Question.Text = customerQuestion.Question.Trim();
                    }

                    DLC_AnswerTime.Text = customerQuestion.AnswerTime.ToString("yyyy-MM-dd");
                    TB_Company.Text = customerQuestion.Company.Trim();

                    TB_ContactPerson.Text = customerQuestion.ContactPerson.Trim();
                    TB_PhoneNumber.Text = customerQuestion.PhoneNumber.Trim();
                    TB_EMail.Text = customerQuestion.EMail.Trim();
                    TB_Address.Text = customerQuestion.Address.Trim();
                    TB_PostCode.Text = customerQuestion.PostCode.Trim();

                    LB_ID.Text = customerQuestion.ID.ToString();

                    LoadCustomerQuestionHandleRecord(strID);

                    strHandleTime = customerQuestion.SummitTime.ToString("yyyyMMdd");
                    strNow = DateTime.Now.ToString("yyyyMMdd");

                    strStatus = customerQuestion.Status.Trim();

                    //�̻���Ϣ
                    TB_BusinessName.Text = customerQuestion.BusinessName.Trim();
                    DLC_ExpectedTime.Text = customerQuestion.ExpectedTime.ToString("yyyy-MM-dd");
                    TB_CustomerName.Text = customerQuestion.CustomerName.Trim();
                    TB_CustomerManager.Text = customerQuestion.CustomerManager.Trim();
                    NB_ExpectedEarnings.Amount = customerQuestion.ExpectedEarnings;

                    TB_BusinessSource.Text = customerQuestion.BusinessSource.Trim();
                    TB_SucessKeyReason.Text = customerQuestion.SucessKeyReason.Trim();
                    TB_FailedKeyReason.Text = customerQuestion.FailedKeyReason.Trim();
                    TB_AgencyName.Text = customerQuestion.AgencyName.Trim();

                    DL_Stage.SelectedValue = customerQuestion.Stage.Trim();
                    DL_CustomerStage.SelectedValue = customerQuestion.CustomerStage.Trim();
                    NB_Possibility.Amount = customerQuestion.Possibility;
                }
                catch
                {
                }

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Detail")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','true') ", true);
                }
            }
        }

        if (e.CommandName == "Delete")
        {
            IList lst;

            string strID, strHandleTime, strNow;

            strID = e.Item.Cells[2].Text.Trim();

            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strID;
            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

            try
            {
                strStatus = customerQuestion.Status.Trim();
                strHandleTime = customerQuestion.SummitTime.ToString("yyyyMMdd");
                strNow = DateTime.Now.ToString("yyyyMMdd");

                strStatus = customerQuestion.Status.Trim();

                if (strNow == strHandleTime & strStatus == "New")
                {
                    customerQuestionBLL.DeleteCustomerQuestion(customerQuestion);

                    LB_ID.Text = "";
                    TB_Question.Text = "";
                    HT_Question.Text = "";

                    DLC_AnswerTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TB_Company.Text = "";
                    TB_ContactPerson.Text = "";
                    TB_PhoneNumber.Text = "";
                    TB_EMail.Text = "";
                    TB_Address.Text = "";
                    TB_PostCode.Text = "";


                    if (strRelatedCustomerCode == null)
                    {
                        InitialCustomerQuestionTree("");
                    }
                    else
                    {
                        InitialCustomerQuestionTree(strRelatedCustomerCode);
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBWTJLDTHMYBSLCKYSCJC").ToString().Trim() + "')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSCSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text;
        IList lst;

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddCustomerRequirement();
        }
        else
        {
            UpdateCustomerRequirement();
        }
    }

    protected void AddCustomerRequirement()
    {
        string strID, strCompany, strContactPerson, strPhoneNumber, strEMail, strAddress, strPostCode, strType, strQuestion;
        string strUserCode, strIsImportant;
        DateTime dtAnswerTime;

        string strHQL;

        strUserCode = LB_UserCode.Text.Trim();

        strCompany = TB_Company.Text.Trim();

        strContactPerson = TB_ContactPerson.Text.Trim();
        strPhoneNumber = TB_PhoneNumber.Text.Trim();
        strEMail = TB_EMail.Text.Trim();
        strAddress = TB_Address.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strType = DL_CustomerQuestionType.SelectedValue.Trim();
        strIsImportant = DL_IsImportant.SelectedValue.Trim();

        if (strIsMobileDevice == "YES")
        {
            strQuestion = HT_Question.Text.Trim();
        }
        else
        {
            strQuestion = TB_Question.Text.Trim();
        }
        dtAnswerTime = DateTime.Parse(DLC_AnswerTime.Text);


        if (strCompany == "" | strContactPerson == "" | strPhoneNumber == "" | strQuestion == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDHXBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        else
        {
            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
            CustomerQuestion customerQuestion = new CustomerQuestion();

            customerQuestion.Question = strQuestion;

            customerQuestion.Type = strType;
            customerQuestion.IsImportant = strIsImportant;

            customerQuestion.Company = strCompany;

            customerQuestion.ContactPerson = strContactPerson;
            customerQuestion.PhoneNumber = strPhoneNumber;
            customerQuestion.EMail = strEMail;
            customerQuestion.Address = strAddress;
            customerQuestion.PostCode = strPostCode;
            customerQuestion.Type = strType;
            customerQuestion.AnswerTime = dtAnswerTime;
            customerQuestion.RecorderCode = strUserCode;
            customerQuestion.SummitTime = DateTime.Now;
            customerQuestion.Status = "New";

            customerQuestion.OperatorCode = "";
            customerQuestion.OperatorName = "";
            customerQuestion.OperatorStatus = "";
            customerQuestion.FromWebSite = "";

            //�̻���Ϣ
            TB_CustomerName.Text = strCompany;
            TB_CustomerManager.Text = strContactPerson;
            TB_BusinessName.Text = strQuestion;

            customerQuestion.BusinessName = TB_BusinessName.Text.Trim();
            customerQuestion.ExpectedTime = DateTime.Parse(DLC_ExpectedTime.Text);
            customerQuestion.ExpectedEarnings = NB_ExpectedEarnings.Amount;
            customerQuestion.CustomerManager = TB_CustomerManager.Text.Trim();
            customerQuestion.CustomerName = TB_CustomerName.Text.Trim();
            customerQuestion.SucessKeyReason = TB_SucessKeyReason.Text.Trim();
            customerQuestion.FailedKeyReason = TB_FailedKeyReason.Text.Trim();
            customerQuestion.AgencyName = TB_AgencyName.Text.Trim();
            customerQuestion.BusinessSource = TB_BusinessSource.Text.Trim();
            customerQuestion.Possibility = int.Parse(NB_Possibility.Amount.ToString());

            customerQuestion.Stage = DL_Stage.SelectedValue.Trim();
            customerQuestion.CustomerStage = DL_CustomerStage.SelectedValue.Trim();

            //try
            //{
            customerQuestionBLL.AddCustomerQuestion(customerQuestion);
            strID = ShareClass.GetMyCreatedMaxCustomerQuestionID(strUserCode);
            LB_ID.Text = strID;

            strHQL = "Insert Into T_CustomerRelatedQuestion(QuestionID,CustomerCode) Values (" + strID + "," + "'" + strRelatedCustomerCode + "'" + ")";
            ShareClass.RunSqlCommand(strHQL);

            if (strRelatedCustomerCode == null)
            {
                InitialCustomerQuestionTree("");
            }
            else
            {
                InitialCustomerQuestionTree(strRelatedCustomerCode);
            }

            LoadCustomerQuestion(strUserCode, strRelatedCustomerCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            //}
            //catch
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSBJC").ToString().Trim() + "')", true);

            //    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            //}
        }
    }

    protected void UpdateCustomerRequirement()
    {
        string strHQL;
        IList lst;
        string strID, strCompany, strContactPerson, strPhoneNumber, strEMail, strAddress, strPostCode, strType, strQuestion, strStatus, strUserCode, strHandleTime, strNow;
        string strIsImportant;

        DateTime dtAnswerTime;

        strUserCode = LB_UserCode.Text.Trim();

        strID = LB_ID.Text.Trim();
        strCompany = TB_Company.Text.Trim();
        strIsImportant = DL_IsImportant.SelectedValue.Trim();

        strContactPerson = TB_ContactPerson.Text.Trim();
        strPhoneNumber = TB_PhoneNumber.Text.Trim();
        strEMail = TB_EMail.Text.Trim();
        strAddress = TB_Address.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strType = DL_CustomerQuestionType.SelectedValue.Trim();
        if (strIsMobileDevice == "YES")
        {
            strQuestion = HT_Question.Text.Trim();
        }
        else
        {
            strQuestion = TB_Question.Text.Trim();
        }
        dtAnswerTime = DateTime.Parse(DLC_AnswerTime.Text);

        if (strCompany == "" | strContactPerson == "" | strPhoneNumber == "" | strQuestion == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDHXBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        else
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strID;
            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

            try
            {
                customerQuestion.Question = strQuestion;
                customerQuestion.Type = strType;
                customerQuestion.IsImportant = strIsImportant;
                customerQuestion.ContactPerson = strContactPerson;
                customerQuestion.Company = strCompany;

                customerQuestion.PhoneNumber = strPhoneNumber;
                customerQuestion.EMail = strEMail;
                customerQuestion.Address = strAddress;
                customerQuestion.PostCode = strPostCode;
                customerQuestion.Type = strType;
                customerQuestion.AnswerTime = dtAnswerTime;

                //�̻���Ϣ
                TB_CustomerName.Text = strCompany;
                TB_CustomerManager.Text = strContactPerson;
                TB_BusinessName.Text = strQuestion;

                customerQuestion.BusinessName = TB_BusinessName.Text.Trim();
                customerQuestion.ExpectedTime = DateTime.Parse(DLC_ExpectedTime.Text);
                customerQuestion.ExpectedEarnings = NB_ExpectedEarnings.Amount;
                customerQuestion.CustomerManager = TB_CustomerManager.Text.Trim();
                customerQuestion.CustomerName = TB_CustomerName.Text.Trim();
                customerQuestion.SucessKeyReason = TB_SucessKeyReason.Text.Trim();
                customerQuestion.FailedKeyReason = TB_FailedKeyReason.Text.Trim();
                customerQuestion.AgencyName = TB_AgencyName.Text.Trim();
                customerQuestion.BusinessSource = TB_BusinessSource.Text.Trim();
                customerQuestion.Possibility = int.Parse(NB_Possibility.Amount.ToString());

                customerQuestion.Stage = DL_Stage.SelectedValue.Trim();
                customerQuestion.CustomerStage = DL_CustomerStage.SelectedValue.Trim();

                strStatus = customerQuestion.Status.Trim();
                strHandleTime = customerQuestion.SummitTime.ToString("yyyyMMdd");
                strNow = DateTime.Now.ToString("yyyyMMdd");

                strStatus = customerQuestion.Status.Trim();

                if (strNow == strHandleTime & strStatus == "New")
                {
                    customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strID));

                    if (strRelatedCustomerCode == null)
                    {
                        InitialCustomerQuestionTree("");
                    }
                    else
                    {
                        InitialCustomerQuestionTree(strRelatedCustomerCode);
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBWTDTHMYSLZCKYXGJC").ToString().Trim() + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXGSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";
        TB_Question.Text = "";
        HT_Question.Text = "";

        DLC_AnswerTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        TB_Company.Text = "";

        TB_ContactPerson.Text = "";
        TB_PhoneNumber.Text = "";
        TB_EMail.Text = "";
        TB_Address.Text = "";
        TB_PostCode.Text = "";


        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Finish_Click(object sender, EventArgs e)
    {
        string strHQL, strQuestionID;
        IList lst;

        strQuestionID = LB_ID.Text.Trim();

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        customerQuestion.Status = "Completed";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void InitialCustomerQuestionTree(string strCustomerCode)
    {
        string strHQL, strUserCode, strID, strQuestion, strSummitTime, strType;

        DataSet ds2, ds3, ds4;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node1.Text = LanguageHandle.GetWord("BKeHuFuWuXuQiuLieBiaoB").ToString().Trim();
        node1.Target = LanguageHandle.GetWord("KeHuFuWuXuQiuLieBiao").ToString().Trim();
        node1.Expanded = false;
        TreeView1.Nodes.Add(node1);

        strUserCode = LB_UserCode.Text.Trim();

        if (strCustomerCode == "")
        {
            strHQL = "select  Distinct  to_char(SummitTime,'yyyy-mm-dd') as SummitTime from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
        }
        else
        {
            strHQL = "select  Distinct to_char(SummitTime,'yyyy-mm-dd') as SummitTime from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
            strHQL += " and ID in (Select QuestionID From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + ")";
        }
        strHQL += " Order by SummitTime DESC limit 30";

        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerQuestion");

        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
        {
            strSummitTime = ds2.Tables[0].Rows[i][0].ToString();

            node2 = new TreeNode();
            node2.Text = strSummitTime;
            node2.Target = strSummitTime;
            node2.Expanded = false;

            if (strCustomerCode == "")
            {
                strHQL = "select Distinct Type from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
            }
            else
            {
                strHQL = "select Distinct Type from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
                strHQL += " and ID in (Select QuestionID From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + ")";
            }
            strHQL += " and to_char(SummitTime,'yyyy-mm-dd') = " + "'" + strSummitTime + "'";
            strHQL += " Order by Type DESC";

            ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerQuestionRecord");
            for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
            {
                node3 = new TreeNode();

                strType = ds3.Tables[0].Rows[j][0].ToString();

                node3.Text = strType;
                node3.Target = strType;
                node3.Expanded = false;

                if (strCustomerCode == "")
                {
                    strHQL = "select ID,Question from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
                }
                else
                {
                    strHQL = "select ID,Question from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
                    strHQL += " and ID in (Select QuestionID From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + ")";
                }
                strHQL += " and Type = " + "'" + strType + "'" + " and to_char(SummitTime,'yyyy-mm-dd') = " + "'" + strSummitTime + "'";
                strHQL += " Order by SummitTime DESC";

                ds4 = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerQuestionRecord");
                for (int k = 0; k < ds4.Tables[0].Rows.Count; k++)
                {
                    node4 = new TreeNode();

                    strID = ds4.Tables[0].Rows[k][0].ToString();
                    strQuestion = ds4.Tables[0].Rows[k][1].ToString().Trim();

                    node4.Text = strID + " " + strQuestion;
                    node4.Target = strID;
                    node4.Expanded = false;

                    node3.ChildNodes.Add(node4);
                }

                node2.ChildNodes.Add(node3);
            }

            node1.ChildNodes.Add(node2);
        }

        TreeView1.DataBind();
        TreeView1.ExpandAll();
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

    protected void LoadCustomerQuestionType()
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestionType as customerQuestionType Order By customerQuestionType.SortNumber ASC";
        CustomerQuestionTypeBLL customerQuestionTypeBLL = new CustomerQuestionTypeBLL();
        lst = customerQuestionTypeBLL.GetAllCustomerQuestionTypes(strHQL);

        DL_CustomerQuestionType.DataSource = lst;
        DL_CustomerQuestionType.DataBind();
    }

    protected void LoadCustomerQuestion(string strUserCode, string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.RecorderCode = " + "'" + strUserCode + "'";
        if (strCustomerCode != "")
        {
            strHQL += " And customerQuestion.ID in (Select customerRelatedQuestion.QuestionID From CustomerRelatedQuestion as customerRelatedQuestion Where customerRelatedQuestion.CustomerCode = '" + strCustomerCode + "')";
        }
        strHQL += " order by customerQuestion.ID DESC";

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();

        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
        LB_Sql4.Text = strHQL;
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
}