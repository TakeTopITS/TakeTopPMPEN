using System; using System.Resources;
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
using System.Drawing;

public partial class TTCustomerQuestionRecord_YOUP : System.Web.UI.Page
{
    string strIsMobileDevice;
    string strRelatedCustomerCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        strIsMobileDevice = Session["IsMobileDevice"].ToString();
        strRelatedCustomerCode = Request.QueryString["CustomerCode"];

        //CKEditor初始化      
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(TB_Question);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "记录客户需求", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_Question.Visible = true;
            }
            else
            {
                TB_Question.Visible = true;
            }

            DLC_AnswerTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SummitTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (strRelatedCustomerCode == null)
            {
                InitialCustomerQuestionTree("");
            }
            else
            {
                InitialCustomerQuestionTree(strRelatedCustomerCode);
            }

            LoadProjectMember();
            //列出直接成员
            ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_Operator);
        }
    }

    protected void InitialCustomerQuestionTree(string strCustomerCode)
    {
        string strHQL, strUserCode, strID, strQuestion, strSummitTime, strType;

        DataSet ds2, ds3, ds4;

        //添加根节点
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
            strHQL = "select Distinct to_char(SummitTime,'yyyy-mm-dd') as SummitTime from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
        }
        else
        {
            strHQL = "select Distinct to_char(SummitTime,'yyyy-mm-dd') as SummitTime from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
            strHQL += " and ID in (Select QuestionID From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + ")";
        }
        strHQL += " Order by SummitTime DESC";

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
                strHQL = "select Distinct OperatorName from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
            }
            else
            {
                strHQL = "select Distinct OperatorName from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
                strHQL += " and ID in (Select QuestionID From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + ")";
            }
            strHQL += " and to_char(SummitTime,'yyyy-mm-dd') = " + "'" + strSummitTime + "'";
            strHQL += " Order by OperatorName ";

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
                    strHQL = "select ID,ContactPerson from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
                }
                else
                {
                    strHQL = "select ID,ContactPerson from T_CustomerQuestion where RecorderCode = " + "'" + strUserCode + "'";
                    strHQL += " and ID in (Select QuestionID From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + ")";
                }
                strHQL += " and OperatorName = " + "'" + strType + "'" + " and to_char(SummitTime,'yyyy-mm-dd') = " + "'" + strSummitTime + "'";
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
                DL_OperatorName.SelectedValue = customerQuestion.OperatorName.Trim();
            }
            catch
            {
            }

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

            DLC_SummitTime.Text = customerQuestion.SummitTime.ToString("yyyy-MM-dd");
            TB_ContactPerson.Text = customerQuestion.ContactPerson.Trim();
            TB_PhoneNumber.Text = customerQuestion.PhoneNumber.Trim();
            TB_EMail.Text = customerQuestion.EMail.Trim();
            TB_Address.Text = customerQuestion.Address.Trim();
            TB_PostCode.Text = customerQuestion.PostCode.Trim();
            LB_Status.Text = customerQuestion.Status.Trim();
            
            LB_OperatorCode.Text = customerQuestion.OperatorCode.Trim();
            TB_OperatorCode.Text = customerQuestion.OperatorCode.Trim();
            if (customerQuestion.OperatorCode.Trim() == "")
            {
                LB_OperatorName.Text = "";
            }
            else
            {
                LB_OperatorName.Text = ShareClass.GetUserName(customerQuestion.OperatorCode.Trim());
            }
            LB_OperatorStatus.Text = customerQuestion.OperatorStatus.Trim();

            LB_ID.Text = customerQuestion.ID.ToString();

            TB_CustomerArea.Text = customerQuestion.CustomerArea.Trim();
            TB_Type.Text = customerQuestion.Type.Trim();
            DL_OperatorName.SelectedValue = customerQuestion.OperatorName.Trim();
            TB_UserPosition.Text = customerQuestion.UserPosition.Trim();

            LoadCustomerQuestionHandleRecord(strID);
            LoadRelatedDoc(strID);

            strHandleTime = customerQuestion.SummitTime.ToString("yyyyMMdd");
            strNow = DateTime.Now.ToString("yyyyMMdd");

            strStatus = customerQuestion.Status.Trim();

            if (customerQuestion.RecorderCode.Trim().ToUpper() == "ADMIN")
            {
                TB_Company.Enabled = true;
                TB_PhoneNumber.Enabled = true;
                TB_EMail.Enabled = true;
            }
            else
            {
                TB_Company.Enabled = false;
                TB_PhoneNumber.Enabled = false;
                TB_EMail.Enabled = false;
            }

            if (strNow == strHandleTime | strStatus == "New")
            {
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
                BT_TransferOperator.Enabled = true;
            }
            else
            {
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                BT_TransferOperator.Enabled = true;
            }
        }
        catch
        {
        }
    }


    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strQuestion;
        DateTime dtAnswerTime;

        string strHQL;

        if (strIsMobileDevice == "YES")
        {
            strQuestion = HT_Question.Text.Trim();
        }
        else
        {
            strQuestion = TB_Question.Text.Trim();
        }
        dtAnswerTime = DateTime.Parse(DLC_AnswerTime.Text);


        if (TB_Company.Text.Trim() == "" | TB_ContactPerson.Text.Trim() == "" | TB_PhoneNumber.Text.Trim() == "" | strQuestion == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSDMXMDH1JBZLDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
            CustomerQuestion customerQuestion = new CustomerQuestion();

            customerQuestion.Question = strQuestion;
            customerQuestion.Type = TB_Type.Text.Trim();
            customerQuestion.IsImportant = "NO";
            customerQuestion.Company = TB_Company.Text.Trim();
            customerQuestion.ContactPerson = TB_ContactPerson.Text.Trim();
            customerQuestion.PhoneNumber = TB_PhoneNumber.Text.Trim();
            customerQuestion.EMail = TB_EMail.Text.Trim();
            customerQuestion.Address = TB_Address.Text.Trim();
            customerQuestion.PostCode = TB_PostCode.Text.Trim();
            customerQuestion.AnswerTime = dtAnswerTime;
            customerQuestion.RecorderCode = LB_UserCode.Text.Trim();
            customerQuestion.SummitTime = DLC_SummitTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_SummitTime.Text.Trim());
            customerQuestion.Status = "New";
            customerQuestion.OperatorCode = TB_OperatorCode.Text.Trim();
            customerQuestion.OperatorName = DL_OperatorName.SelectedValue.Trim();
            customerQuestion.OperatorStatus = "";
            customerQuestion.CustomerArea = TB_CustomerArea.Text.Trim();
            customerQuestion.UserIP = "";
            customerQuestion.UserPosition = TB_UserPosition.Text.Trim();
            customerQuestion.FromWebSite = "";

            try
            {
                customerQuestionBLL.AddCustomerQuestion(customerQuestion);
                LB_ID.Text = ShareClass.GetMyCreatedMaxCustomerQuestionID(customerQuestion.RecorderCode.Trim());

                if (LB_UserCode.Text.Trim().ToUpper() == "ADMIN")
                {
                    TB_Company.Enabled = true;
                    TB_PhoneNumber.Enabled = true;
                    TB_EMail.Enabled = true;
                }
                else
                {
                    TB_Company.Enabled = false;
                    TB_PhoneNumber.Enabled = false;
                    TB_EMail.Enabled = false;
                }

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
                BT_TransferOperator.Enabled = true;

                LB_Status.Text = "New";

                strHQL = "Insert Into T_CustomerRelatedQuestion(QuestionID,CustomerCode) Values (" + LB_ID.Text.Trim() + "," + "'" + strRelatedCustomerCode + "'" + ")";
                ShareClass.RunSqlCommand(strHQL);


                if (strRelatedCustomerCode == null)
                {
                    InitialCustomerQuestionTree("");
                }
                else
                {
                    InitialCustomerQuestionTree(strRelatedCustomerCode);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strID, strQuestion, strUserCode;
        string strHQL;
        IList lst;
        DateTime dtAnswerTime;

        strUserCode = LB_UserCode.Text.Trim();

        strID = LB_ID.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strQuestion = HT_Question.Text.Trim();
        }
        else
        {
            strQuestion = TB_Question.Text.Trim();
        }
        dtAnswerTime = DateTime.Parse(DLC_AnswerTime.Text);


        if (TB_Company.Text.Trim() == "" | TB_ContactPerson.Text.Trim() == "" | TB_PhoneNumber.Text.Trim() == "" | strQuestion == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSDMXMDH1JBZLDBNWKJC").ToString().Trim()+"')", true);
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
                customerQuestion.Type = TB_Type.Text.Trim();
                customerQuestion.ContactPerson = TB_ContactPerson.Text.Trim();
                customerQuestion.Company = TB_Company.Text.Trim();
                customerQuestion.PhoneNumber = TB_PhoneNumber.Text.Trim();
                customerQuestion.EMail = TB_EMail.Text.Trim();
                customerQuestion.Address = TB_Address.Text.Trim();
                customerQuestion.PostCode = TB_PostCode.Text.Trim();
                customerQuestion.OperatorCode = TB_OperatorCode.Text.Trim();
                customerQuestion.AnswerTime = dtAnswerTime;
                customerQuestion.OperatorName = DL_OperatorName.SelectedValue.Trim();
                customerQuestion.CustomerArea = TB_CustomerArea.Text.Trim();
                customerQuestion.SummitTime = DLC_SummitTime.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_SummitTime.Text.Trim());
                customerQuestion.UserPosition = TB_UserPosition.Text.Trim();

                string strStatus = customerQuestion.Status.Trim();
                string strHandleTime = customerQuestion.SummitTime.ToString("yyyyMMdd");
                string strNow = DateTime.Now.ToString("yyyyMMdd");

                if (strStatus == "New")
                {
                    customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strID));

                    if (LB_UserCode.Text.Trim().ToUpper() == "ADMIN")
                    {
                        TB_Company.Enabled = true;
                        TB_PhoneNumber.Enabled = true;
                        TB_EMail.Enabled = true;
                    }
                    else
                    {
                        TB_Company.Enabled = false;
                        TB_PhoneNumber.Enabled = false;
                        TB_EMail.Enabled = false;
                    }

                    if (strRelatedCustomerCode == null)
                    {
                        InitialCustomerQuestionTree("");
                    }
                    else
                    {
                        InitialCustomerQuestionTree(strRelatedCustomerCode);
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBWTMYSLZCKYXGJC").ToString().Trim()+"')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWXGSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strStatus, strHandleTime, strNow;

        strID = LB_ID.Text.Trim();

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        try
        {
            strStatus = customerQuestion.Status.Trim();
            strHandleTime = customerQuestion.SummitTime.ToString("yyyyMMdd");
            strNow = DateTime.Now.ToString("yyyyMMdd");

            if (strStatus == "New")
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

                DLC_SummitTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LB_Status.Text = "New";

                LB_OperatorCode.Text = "";
                LB_OperatorName.Text = "";
                LB_OperatorStatus.Text = "";

                TB_CustomerArea.Text = "";
                TB_UserPosition.Text = "";
                TB_OperatorCode.Text = "";

                TB_Company.Enabled = true;
                TB_PhoneNumber.Enabled = true;
                TB_EMail.Enabled = true;

                BT_Delete.Enabled = false;
                BT_Update.Enabled = false;
                BT_TransferOperator.Enabled = false;

                if (strRelatedCustomerCode == null)
                {
                    InitialCustomerQuestionTree("");
                }
                else
                {
                    InitialCustomerQuestionTree(strRelatedCustomerCode);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBWTMYBSLCKYSCJC").ToString().Trim()+"')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWSCSBJC").ToString().Trim()+"')", true);
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
        LB_Status.Text = "New";

        DLC_SummitTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        LB_OperatorCode.Text = "";
        LB_OperatorName.Text = "";
        LB_OperatorStatus.Text = "";

        TB_CustomerArea.Text = "";
        TB_UserPosition.Text = "";
        TB_OperatorCode.Text = "";

        TB_Company.Enabled = true;
        TB_PhoneNumber.Enabled = true;
        TB_EMail.Enabled = true;

        BT_Update.Enabled = false;
        BT_Delete.Enabled = false;
        BT_TransferOperator.Enabled = false;
    }

    protected void BT_TransferOperator_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strOperatorCode = DL_Operator.SelectedValue.Trim();

        string strQuestionID = LB_ID.Text.Trim();

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

            LB_OperatorCode.Text = strOperatorCode;
            LB_OperatorName.Text = ShareClass.GetUserName(strOperatorCode);
            LB_OperatorStatus.Text = "";
            TB_OperatorCode.Text = strOperatorCode;

            //推送消息给受理人
            Msg msg = new Msg();
            string strMsg = LanguageHandle.GetWord("FuWuXuQiu").ToString().Trim() + ":" + customerQuestion.Question.Trim() + "," + LanguageHandle.GetWord("ZZYaoNiChuLi").ToString().Trim();
            msg.SendMSM("Message",strOperatorCode, strMsg, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZDCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZDSBJC").ToString().Trim()+"')", true);
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
    }

    protected void LoadRelatedDoc(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.RelatedType = '客服' and document.RelatedID = " + strQuestionID; 
        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string struserCode;

        if (e.CommandName != "Page")
        {
            TabContainer1.ActiveTabIndex = 0;

            struserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            TB_OperatorCode.Text = struserCode;
            LB_OperatorCode.Text = struserCode;
            LB_OperatorName.Text = ShareClass.GetUserName(struserCode);
        }
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void LoadProjectMember()
    {
        string strHQL;
        strHQL = "Select * From T_ProjectMember Where 1=1 ";
        if (TB_UserInfo.Text.Trim() != "")
        {
            strHQL += "and (UserCode like '%" + TB_UserInfo.Text.Trim() + "%' or UserName like '%" + TB_UserInfo.Text.Trim() + "%' or Status like '%" + TB_UserInfo.Text.Trim() + "%')";
        }
        strHQL += " Order by UserCode";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        LB_Sql3.Text = strHQL;
    }

    protected void BT_Check_Click(object sender, EventArgs e)
    {
        LoadProjectMember();
    }
}
