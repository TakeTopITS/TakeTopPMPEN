using System;
using System.Resources;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAllCustomerQuestions_YOUP : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL, strStatus;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strStatus = DL_ServiceStatus.SelectedValue.Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","查看所有客户服务", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;

            if (strStatus == "Warning")
            {
                strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Accepted' ";
                if (TextBox1.Text.Trim() != "")
                {
                    strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
                }
                if (TextBox2.Text.Trim() != "")
                {
                    strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
                }
                strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
                strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
                strHQL += " or ((customerQuestion.RecorderCode = '')";
                strHQL += " and (customerQuestion.OperatorCode = '')))";
                //strHQL += " and customerQuestion.ID in (Select customerQuestionHandleRecord.QuestionID From CustomerQuestionHandleRecord as customerQuestionHandleRecord Where to_char( customerQuestionHandleRecord.NextServiceTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') and customerQuestionHandleRecord.ID in (Select Max(customerQuestionHandleRecord1.ID) From CustomerQuestionHandleRecord as customerQuestionHandleRecord1 Group By customerQuestionHandleRecord1.QuestionID) ) ";
                strHQL += " order by customerQuestion.ID DESC";
                CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
                lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

                DataGrid1.DataSource = lst;
                DataGrid1.DataBind();
                LB_Sql1.Text = strHQL;

                LB_QueryScope.Text = strStatus;
            }
        }
    }

    protected void DL_ServiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartString = LB_DepartString.Text.Trim();
        string strStatus = DL_ServiceStatus.SelectedValue.Trim();

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Accepted' ";
        if (TextBox1.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
        }
        if (TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
        }
        strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " or ((customerQuestion.RecorderCode = '')";
        strHQL += " and (customerQuestion.OperatorCode = '')))";
        //strHQL += " and customerQuestion.ID in (Select customerQuestionHandleRecord.QuestionID From CustomerQuestionHandleRecord as customerQuestionHandleRecord Where  to_char( customerQuestionHandleRecord.NextServiceTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') and customerQuestionHandleRecord.ID in (Select Max(customerQuestionHandleRecord1.ID) From CustomerQuestionHandleRecord as customerQuestionHandleRecord1 Group By customerQuestionHandleRecord1.QuestionID) ) ";
        strHQL += " order by customerQuestion.ID DESC";

        if (strStatus == "Warning")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Accepted' ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " and customerQuestion.ID in (Select customerQuestionHandleRecord.QuestionID From CustomerQuestionHandleRecord as customerQuestionHandleRecord Where  to_char( customerQuestionHandleRecord.NextServiceTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') and customerQuestionHandleRecord.ID in (Select Max(customerQuestionHandleRecord1.ID) From CustomerQuestionHandleRecord as customerQuestionHandleRecord1 Group By customerQuestionHandleRecord1.QuestionID) ) ";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "ToHandle")
        {
            strHQL = "from CustomerQuestion as customerQuestion where 1=1 ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and (((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.RecorderCode = ''))";
            strHQL += " and (customerQuestion.OperatorCode = ''))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "InProgress")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Accepted' ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "Processed")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Completed'  ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "Deleted")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Deleted'  ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "All")
        {
            strHQL = "from CustomerQuestion as customerQuestion where 1=1 ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
        LB_Sql1.Text = strHQL;

        LB_QueryScope.Text = strStatus;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql1.Text;
        IList lst;

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void BT_Check_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartString = LB_DepartString.Text.Trim();
        string strStatus = DL_ServiceStatus.SelectedValue.Trim();

        if (TextBox1.Text.Trim() != "" && TextBox2.Text.Trim() != "")
        {
            DateTime dt1, dt2;
            dt1 = DateTime.Parse(TextBox1.Text.Trim());
            dt2 = DateTime.Parse(TextBox2.Text.Trim());
            if (dt1 > dt2)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSZSJYWJC").ToString().Trim() + "')", true);
                return;
            }
        }

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Accepted' ";
        if (TextBox1.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
        }
        if (TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
        }
        strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " or ((customerQuestion.RecorderCode = '')";
        strHQL += " and (customerQuestion.OperatorCode = '')))";
        //strHQL += " and customerQuestion.ID in (Select customerQuestionHandleRecord.QuestionID From CustomerQuestionHandleRecord as customerQuestionHandleRecord Where  to_char( customerQuestionHandleRecord.NextServiceTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') and customerQuestionHandleRecord.ID in (Select Max(customerQuestionHandleRecord1.ID) From CustomerQuestionHandleRecord as customerQuestionHandleRecord1 Group By customerQuestionHandleRecord1.QuestionID) ) ";
        strHQL += " order by customerQuestion.ID DESC";

        if (strStatus == "Warning")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Accepted' ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " and customerQuestion.ID in (Select customerQuestionHandleRecord.QuestionID From CustomerQuestionHandleRecord as customerQuestionHandleRecord Where  to_char( customerQuestionHandleRecord.NextServiceTime,'yyyymmdd') <= to_char(now()+PreDays*'1 day'::interval,'yyyymmdd') and customerQuestionHandleRecord.ID in (Select Max(customerQuestionHandleRecord1.ID) From CustomerQuestionHandleRecord as customerQuestionHandleRecord1 Group By customerQuestionHandleRecord1.QuestionID) ) ";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "ToHandle")
        {
            strHQL = "from CustomerQuestion as customerQuestion where 1=1 ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and (((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.RecorderCode = ''))";
            strHQL += " and (customerQuestion.OperatorCode = ''))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "InProgress")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Accepted' ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "Processed")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Completed'  ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "Deleted")
        {
            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorStatus = 'Deleted'  ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        if (strStatus == "All")
        {
            strHQL = "from CustomerQuestion as customerQuestion where 1=1 ";
            if (TextBox1.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox1.Text.Trim() + "'::date-customerQuestion.SummitTime::date<=0 ";
            }
            if (TextBox2.Text.Trim() != "")
            {
                strHQL += " and '" + TextBox2.Text.Trim() + "'::date-customerQuestion.SummitTime::date>=0 ";
            }
            strHQL += " and ((customerQuestion.RecorderCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or (customerQuestion.OperatorCode in (select projectMember.UserCode  From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
            strHQL += " or ((customerQuestion.RecorderCode = '')";
            strHQL += " and (customerQuestion.OperatorCode = '')))";
            strHQL += " order by customerQuestion.ID DESC";
        }

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
        LB_Sql1.Text = strHQL;

        LB_QueryScope.Text = strStatus;
    }
}