using System; using System.Resources;
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

public partial class TTCustomerQuestionManage_YOUP : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","�ͻ�����", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}
        

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;

            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();

            strHQL = "from CustomerQuestion as customerQuestion ";
            strHQL += " where customerQuestion.OperatorCode = " + "'" + strUserCode + "'" + "";
            strHQL += " and customerQuestion.Status = 'New' ";
            strHQL += " order by customerQuestion.ID DESC";
            customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();
            LB_Sql1.Text = strHQL;

            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorCode = " + "'" + strUserCode + "'" + " and customerQuestion.OperatorStatus = 'Accepted' order by customerQuestion.ID DESC";
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();
            LB_Sql4.Text = strHQL;

            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorCode = " + "'" + strUserCode + "'" + " and customerQuestion.OperatorStatus = 'Completed'  order by customerQuestion.ID DESC";
            customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
            LB_Sql2.Text = strHQL;

            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.RecorderCode = " + "'" + strUserCode + "'" + "  order by customerQuestion.ID DESC";
            customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
            LB_Sql3.Text = strHQL;
        }
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

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql2.Text;
        IList lst;

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;
        IList lst;

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void BT_AllQuestion_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartString = LB_DepartString.Text.Trim();

        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();

        strHQL = "from CustomerQuestion as customerQuestion ";
        strHQL += " where customerQuestion.OperatorCode = " + "'" + strUserCode + "'" + "";
        strHQL += " and customerQuestion.Status = 'New' ";
        strHQL += " order by customerQuestion.ID DESC";
        customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
        LB_Sql1.Text = strHQL;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorCode = " + "'" + strUserCode + "'" + " and customerQuestion.OperatorStatus = 'Accepted' order by customerQuestion.ID DESC";
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
        LB_Sql4.Text = strHQL;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.OperatorCode = " + "'" + strUserCode + "'" + " and customerQuestion.OperatorStatus = 'Completed'  order by customerQuestion.ID DESC";
        customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
        LB_Sql2.Text = strHQL;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.RecorderCode = " + "'" + strUserCode + "'" + "  order by customerQuestion.ID DESC";
        customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
        LB_Sql3.Text = strHQL;
    }
}