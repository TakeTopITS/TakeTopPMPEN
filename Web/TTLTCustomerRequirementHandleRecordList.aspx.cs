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

public partial class TTLTCustomerRequirementHandleRecordList : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strQuestionID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strRecorderCode, strStatus;
        string strOperatorCode, strOperatorStatus;


        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);
        strQuestionID = Request.QueryString["ID"];

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];
        strRecorderCode = customerQuestion.RecorderCode.Trim();
        strOperatorCode = customerQuestion.OperatorCode.Trim();
        strOperatorStatus = customerQuestion.OperatorStatus.Trim();
        strStatus = customerQuestion.Status.Trim();

        //this.Title = "�ͻ����⣺" + strQuestionID + "�����¼";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DataList2.DataSource = lst;
            DataList2.DataBind();

            LoadCustomerQuestionHandleRecord(strQuestionID);
            LoadRelatedDoc(strQuestionID);

            if (strStatus == "Deleted")
            {
                BT_CancelDelete.Visible = true;
            }


            HL_QuestionToCustomer.NavigateUrl = "TTQuestionToCustomer.aspx?QuestionID=" + strQuestionID;

            //�г�ֱ�ӳ�Ա
            ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_Operator);

            LoadCustomerQuestionRelatedCandidate(strQuestionID);
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

        customerQuestion.Status = "InProgress";
        customerQuestion.OperatorCode = strOperatorCode;
        customerQuestion.OperatorName = ShareClass.GetUserName(strOperatorCode);
        customerQuestion.OperatorStatus = "Accepted";

        try
        {
            customerQuestionBLL.UpdateCustomerQuestion(customerQuestion, int.Parse(strQuestionID));

            //������Ϣ��������
            Msg msg = new Msg();
            string strMsg = LanguageHandle.GetWord("FuWuXuQiu").ToString().Trim() + ":" + customerQuestion.Question.Trim() + "," + LanguageHandle.GetWord("ZZYaoNiChuLi").ToString().Trim();
            msg.SendMSM("Message", strOperatorCode, strMsg, strUserCode);

            LoadCustomerQuestion(strQuestionID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZDCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZDSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_CancelDelete_Click(object sender, EventArgs e)
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

            BT_CancelDelete.Visible = false;

            //������Ϣ��������
           
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
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


    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strCandidateName = ((Button)e.Item.FindControl("BT_UserName")).Text;

        LB_CandidateName.Text = strCandidateName;

        LoadCollaborationLogWithCustomerQuestionCandidate(strQuestionID, strCandidateName);
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


    protected void LoadCustomerQuestionRelatedCandidate(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestionRelatedCandidate as customerQuestionRelatedCandidate where customerQuestionRelatedCandidate.QuestionID = " + strQuestionID;
        CustomerQuestionRelatedCandidateBLL customerQuestionRelatedCandidateBLL = new CustomerQuestionRelatedCandidateBLL();
        lst = customerQuestionRelatedCandidateBLL.GetAllCustomerQuestionRelatedCandidates(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadCollaborationLogWithCustomerQuestionCandidate(string strQuestionID, string strCandidateName)
    {
        string strHQL;
        IList lst;

        strHQL = "Select * From T_CollaborationLogWithCustomerQuestionCandidate Where QuestionID = " + strQuestionID + " and UserName = " + "'" + strCandidateName + "'";
        strHQL += " Order By LogID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CollaborationLogWithCustomerQuestionCandidate");

        DataList1.DataSource = ds;
        DataList1.DataBind();
    }



    protected void LoadRelatedDoc(string strQuestionID)
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

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }


}
