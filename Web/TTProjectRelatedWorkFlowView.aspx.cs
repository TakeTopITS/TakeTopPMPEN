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

public partial class TTProjectRelatedWorkFlowView : System.Web.UI.Page
{
    string strProjectID;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ��jack.erp@gmail.com)
        //̩�����(TakeTop Software)

        string strHQL;
        IList lst;

        strLangCode = Session["LangCode"].ToString();

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strProjectID = Request.QueryString["ProjectID"];
        string strProjectName = ShareClass.GetProjectName(strProjectID);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.LoadProjectMemberByProjectIDForDataGrid(strProjectID, DataGrid1);

            strHQL = "from WorkFlow as workFlow Where";
            strHQL += " ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
            strHQL += " or (workFlow.RelatedType = 'ExpenseApply' and workFlow.RelatedID in (select expenseApplyWL.ID from ExpenseApplyWL as expenseApplyWL where expenseApplyWL.RelatedID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'ExpenseClaim' and workFlow.RelatedID in (select expenseClaim.ECID from ExpenseClaim as expenseClaim where expenseClaim.RelatedID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
            strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
            strHQL += " Order by workFlow.WLID DESC";
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            lst = workFlowBLL.GetAllWorkFlows(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            LB_Sql.Text = strHQL;

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZGZLSQZSY").ToString().Trim();

            strHQL = "from WLType as wlType ";
            strHQL += " Where wlType.LangCode = " + "'" + strLangCode + "'";
            strHQL += " order by wlType.SortNumber ASC";
            WLTypeBLL wlTypeBLL = new WLTypeBLL();
            lst = wlTypeBLL.GetAllWLTypes(strHQL);
            DL_WLType.DataSource = lst;
            DL_WLType.DataBind();

            strHQL = "from WLStatus as wlStatus ";
            strHQL += " Where wlStatus.LangCode = " + "'" + strLangCode + "'";
            strHQL += " order by wlStatus.SortNumber ASC";
            WLStatusBLL wlStatusBLL = new WLStatusBLL();
            lst = wlStatusBLL.GetAllWLStatuss(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZGZLSQZSY").ToString().Trim();
            LB_ReportName.Text = LanguageHandle.GetWord("XiangMu").ToString().Trim() + ": " + strProjectID + " " + LanguageHandle.GetWord("GongZuoLiuBaoBiao").ToString().Trim();
            strHQL = "select Status as XName, Count(*) as YNumber from T_WorkFlow where ";
            strHQL += " ((RelatedType = 'Project' and RelatedID = " + strProjectID + ")";
            strHQL += " or (RelatedType = 'ExpenseApply' and RelatedID in (select ID from T_ExpenseApplyWL where RelatedID = " + strProjectID + "))";
            strHQL += " or (RelatedType = 'ExpenseClaim' and RelatedID in (select ECID from T_ExpenseClaim where RelatedID = " + strProjectID + "))";
            strHQL += " or (RelatedType = 'Requirement' and RelatedID in (select ReqID from T_RelatedReq where ProjectID = " + strProjectID + "))";
            strHQL += "or (RelatedType = 'Risk' and RelatedID in (select ID from T_ProjectRisk where ProjectID =" + strProjectID + "))";  //ChineseWord
            strHQL += " or (RelatedType = 'Task' and RelatedID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + "))";
            strHQL += " or (RelatedType = 'Plan' and RelatedID in (select ID From T_ImplePlan where ProjectID = " + strProjectID + "))";
            strHQL += "or (RelatedType = 'Meeting' and RelatedID in (select ID from T_Meeting where RelatedID =" + strProjectID + ")))";  //ChineseWord
            strHQL += " Group By Status ";
            IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Bar&ChartName=" + LB_ReportName.Text + "&SqlCode=" + ShareClass.Escape(strHQL);


            //ShareClass.CreateAnalystBarChart(strHQL, Chart1, SeriesChartType.Bar, "", "Status", "StatusCount", "Bar");
            //Chart1.Visible = true;
        }
    }

    protected void BT_AllWorkFlow_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        LB_QueryScope.Text = LanguageHandle.GetWord("ZZGZLSYCY").ToString().Trim();
        LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZGZLSQZSY").ToString().Trim();

        LB_DepartCode.Text = "";
        LB_OperatorCode.Text = "";
        LB_Status.Text = "";

        DataGrid3.CurrentPageIndex = 0;

        strHQL = "from WorkFlow as workFlow Where  ";
        strHQL += " ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
        strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
        strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
        strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
        strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
        strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql.Text = strHQL;

        strHQL = "select Status as XName, Count(*) as YNumber from T_WorkFlow where ";
        strHQL += " ((RelatedType = 'Project' and RelatedID = " + strProjectID + ")";
        strHQL += " or (RelatedType = 'ExpenseApply' and RelatedID in (select ID from T_ExpenseApplyWL where RelatedID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'ExpenseClaim' and RelatedID in (select ECID from T_ExpenseClaim where RelatedID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'Requirement' and RelatedID in (select ReqID from T_RelatedReq where ProjectID = " + strProjectID + "))";
        strHQL += "or (RelatedType = 'Risk' and RelatedID in (select ID from T_ProjectRisk where ProjectID =" + strProjectID + "))";  //ChineseWord
        strHQL += " or (RelatedType = 'Task' and RelatedID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'Plan' and RelatedID in (select ID From T_ImplePlan where ProjectID = " + strProjectID + "))";
        strHQL += "or (RelatedType = 'Meeting' and RelatedID in (select ID from T_Meeting where RelatedID =" + strProjectID + ")))";  //ChineseWord
        strHQL += " Group By Status ";
        IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Bar&ChartName=WorkFlow&SqlCode=" + ShareClass.Escape(strHQL);


        //ShareClass.CreateAnalystBarChart(strHQL, Chart1, SeriesChartType.Bar, "", "Status", "StatusCount", "Bar");
        //Chart1.Visible = true;
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strOperatorName = GetUserName(strOperatorCode);
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        LB_OperatorCode.Text = strOperatorCode;
        LB_OperatorName.Text = strOperatorName;

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + strOperatorName;
        LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + strOperatorName;

        DataGrid3.CurrentPageIndex = 0;

        strHQL = "from WorkFlow as workFlow where workFlow.CreatorCode = " + "'" + strOperatorCode + "'";
        strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
        strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
        strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
        strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
        strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
        strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Status.Text = "";
        LB_Sql.Text = strHQL;

        strHQL = "select Status as XName, Count(*) as YNumber from T_WorkFlow where CreatorCode = " + "'" + strOperatorCode + "'";
        strHQL += "  and ((RelatedType = 'Project' and RelatedID = " + strProjectID + ")";
        strHQL += " or (RelatedType = 'ExpenseApply' and RelatedID in (select ID from T_ExpenseApplyWL where RelatedID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'ExpenseClaim' and RelatedID in (select ECID from T_ExpenseClaim where RelatedID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'Requirement' and RelatedID in (select ReqID from T_RelatedReq where ProjectID = " + strProjectID + "))";
        strHQL += "or (RelatedType = 'Risk' and RelatedID in (select ID from T_ProjectRisk where ProjectID =" + strProjectID + "))";  //ChineseWord
        strHQL += " or (RelatedType = 'Task' and RelatedID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + "))";
        strHQL += " or (RelatedType = 'Plan' and RelatedID in (select ID From T_ImplePlan where ProjectID = " + strProjectID + "))";
        strHQL += "or (RelatedType = 'Meeting' and RelatedID in (select ID from T_Meeting where RelatedID =" + strProjectID + ")))";  //ChineseWord
        strHQL += " Group By Status ";
        IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Bar&ChartName=WorkFlow&SqlCode=" + ShareClass.Escape(strHQL);

    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStatus = ((Button)e.Item.FindControl("BT_Status")).Text;
        string strHQL;
        string strOperatorCode = LB_OperatorCode.Text;
        string strDepartCode, strDepartName;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        strDepartCode = LB_DepartCode.Text.Trim();
        strDepartName = LB_DepartName.Text.Trim();

        DataGrid3.CurrentPageIndex = 0;

        if (LB_OperatorCode.Text == "")
        {
            strHQL = "from WorkFlow as workFlow where workFlow.Status = " + "'" + strStatus + "'";
            strHQL += " and workFlow.CreatorCode = " + "'" + strOperatorCode + "'";
            strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
            strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
            strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
            strHQL += " Order by workFlow.WLID DESC";

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZApplicantAll").ToString().Trim() + " " + LanguageHandle.GetWord("ZhuangTai").ToString().Trim() + ":" + strStatus;
            LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZApplicantAll").ToString().Trim() + " " + LanguageHandle.GetWord("ZhuangTai").ToString().Trim() + ":" + strStatus;
        }
        else
        {
            string strOperatorName = LB_OperatorName.Text.Trim();
            strHQL = "from WorkFlow as workFlow where workFlow.CreatorCode = " + "'" + strOperatorCode + "'";
            strHQL += " and " + "workFlow.Status = " + "'" + strStatus + "'";
            strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
            strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
            strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
            strHQL += " Order by workFlow.WLID DESC";

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + strOperatorName + LanguageHandle.GetWord("ZhuagTai").ToString().Trim() + " ��" + strStatus;
            LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + strOperatorName + " ��" + strStatus;
        }


        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Status.Text = strStatus;
        LB_Sql.Text = strHQL;
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;
        IList lst;

        string strHQL = LB_Sql.Text;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strWLType = DL_WLType.SelectedValue.Trim();
        string strDepartCode, strOperatorCode, strStatus;
        string strDepartName, strOperatorName;

        string strUserCode = LB_UserCode.Text.Trim();

        string strHQL;
        IList lst;

        strOperatorCode = LB_OperatorCode.Text.Trim();
        strOperatorName = LB_OperatorName.Text.Trim();
        strDepartCode = LB_DepartCode.Text.Trim();
        strDepartName = LB_DepartName.Text.Trim();
        strStatus = LB_Status.Text.Trim();

        DataGrid3.CurrentPageIndex = 0;

        if (LB_OperatorCode.Text == "")
        {
            strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'";
            strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
            strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
            strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
            strHQL += " Order by workFlow.WLID DESC";

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZApplicantAll").ToString().Trim() + " " + LanguageHandle.GetWord("LeiXin").ToString().Trim() + ":" + strWLType;
            LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZApplicantAll").ToString().Trim() + " " + LanguageHandle.GetWord("LeiXin").ToString().Trim() + ":" + strWLType;
        }
        else
        {
            if (LB_Status.Text == "")
            {
                strHQL = "from WorkFlow as workFlow where workFlow.CreatorCode = " + "'" + strOperatorCode + "'" + " and " + "workFlow.WLType = " + "'" + strWLType + "'";
                strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
                strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
                strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
                strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
                strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
                strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
                strHQL += " Order by workFlow.WLID DESC";

                LB_QueryScope.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + " " + strOperatorName + LanguageHandle.GetWord("LeiXin").ToString().Trim() + ":" + strWLType;

                LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + " " + strOperatorName + LanguageHandle.GetWord("LeiXin").ToString().Trim() + ":" + strWLType;
            }

            else
            {
                strHQL = "from WorkFlow as workFlow where workFlow.CreatorCode = " + "'" + strOperatorCode + "'" + " and " + "workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.Status = " + "'" + strStatus + "'";
                strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
                strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
                strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
                strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
                strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
                strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
                strHQL += " Order by workFlow.WLID DESC";

                LB_QueryScope.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + " " + strOperatorName + LanguageHandle.GetWord("LeiXin").ToString().Trim() + ":" + strWLType + LanguageHandle.GetWord("ZhuangTai").ToString().Trim() + ":" + strStatus;

                LB_QueryScopeResult.Text = LanguageHandle.GetWord("ZZApplicant").ToString().Trim() + strOperatorCode + " " + strOperatorName + LanguageHandle.GetWord("LeiXin").ToString().Trim() + ":" + strWLType + LanguageHandle.GetWord("ZhuangTai").ToString().Trim() + ":" + strStatus;
            }
        }

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_FindWFID_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        string strWFID;

        strWFID = NB_WFID.Amount.ToString();

        LB_QueryScope.Text = LanguageHandle.GetWord("BianHao").ToString().Trim() + ":" + strWFID;

        LB_DepartCode.Text = "";
        LB_OperatorCode.Text = "";
        LB_Status.Text = "";

        DataGrid3.CurrentPageIndex = 0;

        try
        {
            strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWFID;
            strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
            strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
            strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
            strHQL += " Order by workFlow.WLID DESC";

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            lst = workFlowBLL.GetAllWorkFlows(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            LB_Sql.Text = strHQL;
        }
        catch
        {
        }
    }

    protected void BT_WFNameFind_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strWFName;

        strWFName = TB_WFName.Text.Trim();

        LB_QueryScope.Text = LanguageHandle.GetWord("MingCheng").ToString().Trim() + ":" + strWFName;

        LB_DepartCode.Text = "";
        LB_OperatorCode.Text = "";
        LB_Status.Text = "";

        DataGrid3.CurrentPageIndex = 0;

        strWFName = "%" + strWFName + "%";

        try
        {
            strHQL = "from WorkFlow as workFlow where workFlow.WLName Like " + "'" + strWFName + "'";
            strHQL += " and ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
            strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Risk' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  //ChineseWord
            strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += "or (workFlow.RelatedType = 'Meeting' and workFlow.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + ")))";  //ChineseWord
            strHQL += " Order by workFlow.WLID DESC";

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            lst = workFlowBLL.GetAllWorkFlows(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            LB_Sql.Text = strHQL;
        }
        catch
        {
        }
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName.Trim();
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
