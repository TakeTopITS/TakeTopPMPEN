using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TTWorkPlanRelatedWorkflowListView : System.Web.UI.Page
{
    string strLangCode;
    string strPlanID;

    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ��jack.erp@gmail.com)
        //̩������(TakeTop Software)       
        string strHQL;
        IList lst;

        strPlanID = Request.QueryString["PlanID"];

        strLangCode = Session["LangCode"].ToString();
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = ShareClass.GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        if (ShareClass.CheckUserCanViewProjectPlan(strPlanID, strUserCode) == false)
        {
            Response.Redirect("TTDisplayCustomErrorMessage.aspx?ErrorMsg='" + LanguageHandle.GetWord("ZZJGZYXMJLJHYJHCJRHLXZJHFZRCNJXZCZQJC") + "'");
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            strHQL = "from WorkFlow as workFlow ";
            strHQL += " Where workFlow.RelatedType = 'Plan' And workFlow.RelatedID = " + strPlanID;
            strHQL += " and workFlow.Status <> 'New'";
            strHQL += " Order by workFlow.WLID DESC";
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            lst = workFlowBLL.GetAllWorkFlows(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            LB_Sql.Text = strHQL;

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZGZLSQZSY");
        }
    }

}
