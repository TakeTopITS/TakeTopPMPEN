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

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTUndertakeProject : System.Web.UI.Page
{
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件（TakeTop Software)2006－2012

        strLangCode = Session["LangCode"].ToString();

        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        string strUserName;

        ShareClass.LoadSytemChart(strUserCode, "UndertakeProject", RP_ChartList);
        HL_SystemAnalystChartRelatedUserSet.NavigateUrl = "TTSystemAnalystChartRelatedUserSet.aspx?FormType=UndertakeProject";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LB_UserCode.Text = strUserCode;
            strUserName = ShareClass.GetUserName(strUserCode);
            LB_UserName.Text = strUserName;

            strHQL = string.Format(@"WITH Cost AS (
                    SELECT ProjectID, SUM(CASE WHEN Type='Base' THEN Total ELSE 0 END) AS BaseCost,
                    SUM(CASE WHEN Type='Operation' THEN Total ELSE 0 END) AS OpCost
                    FROM T_ProjectCostManage
                    GROUP BY ProjectID
                    ), Rea AS (
                    SELECT ProjectID, COALESCE(OpCost, 0) / NULLIF(BaseCost, 0) AS TotalBL
                    FROM Cost
                    )
                    SELECT C.*, Rea.TotalBL AS PercentRea
                    FROM T_Project C
                    LEFT JOIN Rea ON C.ProjectID = Rea.ProjectID
                   
                    WHERE C.PMCode = '{0}'
                    AND ((C.ProjectStartupNeedSupperConfirm = 'YES' AND C.ConfirmedBySupper = 'YES')
                    OR C.ProjectStartupNeedSupperConfirm = 'NO')
                    AND C.Status NOT IN ('Hided', 'Deleted', 'Archived')
                    ORDER BY C.ProjectID DESC LIMIT 30;", strUserCode);

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMJL") + strUserName + LanguageHandle.GetWord("StatusAll");
            LB_Sql.Text = strHQL;

            ShareClass.LoadProjectStatusForDataGrid(strLangCode, DataGrid2);

            ShareClass.FinishPercentPicture(DataGrid1, 0);
            ShareClass.FinChargePercentByRow(DataGrid1, 0);
            ShareClass.SetProjectStartAndEndTime(DataGrid1, 0);

            ShareClass.SetRiskLabelColor(DataGrid1, 0);
            ShareClass.SetDefectLabelColor(DataGrid1, 0);

            RealChargeVisible();
            FinChargePercentByStep();
        }
    }
   
    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStatus = ((Button)e.Item.FindControl("BT_Status")).Text.Trim();
        string strUserName = LB_UserName.Text.Trim();
        string strUserCode = LB_UserCode.Text;


        string strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
            "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
            "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID " +
            " left join T_ProjectType E on C.ProjectType = E.Type " +
            " where C.PMCode='" + strUserCode + "' and " + "((E.ProjectStartupNeedSupperConfirm = 'YES' and C.ConfirmedBySupper = 'YES') or E.ProjectStartupNeedSupperConfirm = 'NO') " +
            " and C.Status = '" + strStatus + "' Order by C.ProjectID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        ShareClass.FinishPercentPicture(DataGrid1, 0);
        RealChargeVisible();
        FinChargePercentByStep();
        ShareClass.FinChargePercentByRow(DataGrid1, 0);
        ShareClass.SetProjectStartAndEndTime(DataGrid1, 0);

        ShareClass.SetRiskLabelColor(DataGrid1, 0);
        ShareClass.SetDefectLabelColor(DataGrid1, 0);

        LB_Sql.Text = strHQL;
        LB_QueryScope.Text = LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMJL") + strUserName + LanguageHandle.GetWord("Status") + ":" + strStatus;
    }

    protected void BT_AllProject_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strUserName = LB_UserName.Text.Trim();


        string strHQL ="select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
            "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
            "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID" +
            " left join T_ProjectType E on C.ProjectType = E.Type " +
            " where C.PMCode='" + strUserCode + "' and " + "((E.ProjectStartupNeedSupperConfirm = 'YES' and C.ConfirmedBySupper = 'YES') or E.ProjectStartupNeedSupperConfirm = 'NO') " +
            " and C.Status not in ('Hided','Deleted','Archived') Order by C.ProjectID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMJL") + strUserName + LanguageHandle.GetWord("StatusAll");
        LB_Sql.Text = strHQL;

        ShareClass.FinishPercentPicture(DataGrid1, 0);
        RealChargeVisible();
        FinChargePercentByStep();
        ShareClass.FinChargePercentByRow(DataGrid1, 0);

        ShareClass.SetRiskLabelColor(DataGrid1, 0);
        ShareClass.SetDefectLabelColor(DataGrid1, 0);

        ShareClass.SetProjectStartAndEndTime(DataGrid1, 0);

    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        ShareClass.FinishPercentPicture(DataGrid1, 0);
        RealChargeVisible();
        FinChargePercentByStep();
        ShareClass.FinChargePercentByRow(DataGrid1, 0);
        ShareClass.SetProjectStartAndEndTime(DataGrid1, 0);

        ShareClass.SetRiskLabelColor(DataGrid1, 0);
        ShareClass.SetDefectLabelColor(DataGrid1, 0);
    }

    protected void BT_DisplaySchedule_Click(object sender, EventArgs e)
    {
        ShareClass.FinishPercentPicture(DataGrid1, 0);
    }

    protected void BT_DisplayExpense_Click(object sender, EventArgs e)
    {
        RealChargeVisible();
        FinChargePercentByStep();
    }

    protected void RealChargeVisible()
    {
        LB_CostPer1.Visible = false;
        LB_CostPer2.Visible = false;
        LB_CostPer3.Visible = false;
        LB_CostPer4.Visible = false;
        LB_CostPer5.Visible = false;
        LB_CostPer6.Visible = false;
        LB_CostPer7.Visible = false;
        LB_CostPer8.Visible = false;
        LB_CostPer9.Visible = false;
        LB_CostPer10.Visible = false;

        LB_BdgPer1.Visible = false;
        LB_BdgPer2.Visible = false;
        LB_BdgPer3.Visible = false;
        LB_BdgPer4.Visible = false;
        LB_BdgPer5.Visible = false;
        LB_BdgPer6.Visible = false;
        LB_BdgPer7.Visible = false;
        LB_BdgPer8.Visible = false;
        LB_BdgPer9.Visible = false;
        LB_BdgPer10.Visible = false;

        IMB_ProCost1.Visible = false;
        IMB_ProCost2.Visible = false;
        IMB_ProCost3.Visible = false;
        IMB_ProCost4.Visible = false;
        IMB_ProCost5.Visible = false;
        IMB_ProCost6.Visible = false;
        IMB_ProCost7.Visible = false;
        IMB_ProCost8.Visible = false;
        IMB_ProCost9.Visible = false;
        IMB_ProCost10.Visible = false;

        IMB_ProBdg1.Visible = false;
        IMB_ProBdg2.Visible = false;
        IMB_ProBdg3.Visible = false;
        IMB_ProBdg4.Visible = false;
        IMB_ProBdg5.Visible = false;
        IMB_ProBdg6.Visible = false;
        IMB_ProBdg7.Visible = false;
        IMB_ProBdg8.Visible = false;
        IMB_ProBdg9.Visible = false;
        IMB_ProBdg10.Visible = false;


        LB_ProBdg1.Visible = false;
        LB_ProBdg2.Visible = false;
        LB_ProBdg3.Visible = false;
        LB_ProBdg4.Visible = false;
        LB_ProBdg5.Visible = false;
        LB_ProBdg6.Visible = false;
        LB_ProBdg7.Visible = false;
        LB_ProBdg8.Visible = false;
        LB_ProBdg9.Visible = false;
        LB_ProBdg10.Visible = false;
    }

    protected void FinChargePercentByStep()
    {
        string strProjectID, strBudgetProjectID;
        decimal deBudget, deRealCharge, deDefaultBudget;
        decimal deChargeHeight, deBudgetHeight;
        decimal deChargePercent, deBudgetPercent;
        string strChargePercent, strBudgetPercent;
        int i;
        string strHQL;
        IList lst;

        try
        {

            ProjectBLL projectBLL = new ProjectBLL();
            Project project = new Project();

            ProRealChargeBLL proRealChargeBLL = new ProRealChargeBLL();
            ProRealCharge proRealCharge = new ProRealCharge();

            for (i = 0; i < DataGrid1.Items.Count; i++)
            {
                strProjectID = DataGrid1.Items[i].Cells[0].Text.Trim();

                strBudgetProjectID = "<a href=TTProjectBudgetReport.aspx?ProjectID=" + strProjectID + " target=_blank>" + strProjectID + "</a>";

                strHQL = "from Project as project where project.ProjectID = " + strProjectID;
                lst = projectBLL.GetAllProjects(strHQL);
                project = (Project)lst[0];

                deBudget = project.Budget;

                //实际费用和预算对比
                strHQL = "from ProRealCharge as proRealCharge where proRealCharge.ProjectID = " + strProjectID;
                lst = proRealChargeBLL.GetAllProRealCharges(strHQL);
                if (lst.Count == 0)
                {
                    deRealCharge = 0;
                    deChargePercent = 0;
                    deChargeHeight = 0;
                    strChargePercent = "0" + "/" + deBudget.ToString();
                }
                else
                {
                    proRealCharge = (ProRealCharge)lst[0];
                    deRealCharge = proRealCharge.RealCharge;

                    if (deBudget == 0)
                    {
                        strChargePercent = deRealCharge.ToString();
                    }
                    else
                    {
                        strChargePercent = decimal.Round((deRealCharge / deBudget) * 100, 2).ToString() + "%";
                    }

                    if (deBudget == 0)
                    {
                        deChargePercent = deRealCharge;
                    }
                    else
                    {
                        deChargePercent = deRealCharge / deBudget;
                    }

                    deChargeHeight = deChargePercent * 150;
                    if (deChargeHeight > 150)
                    {
                        deChargeHeight = 150;
                    }
                }

                //标准成本和预算对比
                deDefaultBudget = ShareClass.GetProjectDefaultFinishCost(strProjectID);
                if (deBudget == 0)
                {
                    strBudgetPercent = deDefaultBudget.ToString();
                }
                else
                {
                    strBudgetPercent = decimal.Round((deDefaultBudget / deBudget) * 100, 2).ToString() + "%";
                }

                if (deBudget == 0)
                {
                    deBudgetPercent = deDefaultBudget;
                }
                else
                {
                    deBudgetPercent = deDefaultBudget / deBudget;
                }
                deBudgetHeight = deBudgetPercent * 150;
                if (deBudgetHeight > 150)
                    deBudgetHeight = 150;

                string strConstractAmontToolTip = LanguageHandle.GetWord("GeTongZongE") + GetProjectRelatedConstractTotalReceivablesAmount(strProjectID) + LanguageHandle.GetWord("GeTongYingShouZongE") + GetProjectRelatedConstractTotalReceivablesAmount(strProjectID) + LanguageHandle.GetWord("GeTongShiShouZongE") + GetProjectRelatedConstractTotalReceiverAmount(strProjectID);

                if (i == 0)
                {
                    LB_CostPer1.Visible = true;
                    IMB_ProCost1.Visible = true;

                    LB_BdgPer1.Visible = true;
                    IMB_ProBdg1.Visible = true;

                    LB_ProBdg1.Visible = true;

                    LB_CostPer1.Text = strChargePercent;
                    IMB_ProCost1.Height = (Unit)deChargeHeight;

                    LB_BdgPer1.Text = strBudgetPercent;
                    IMB_ProBdg1.Height = (Unit)deBudgetHeight;

                    LB_ProBdg1.Text = strBudgetProjectID;
                    LB_ProBdg1.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost1.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg1.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 1)
                {
                    LB_CostPer2.Visible = true;
                    IMB_ProCost2.Visible = true;

                    LB_BdgPer2.Visible = true;
                    IMB_ProBdg2.Visible = true;

                    LB_ProBdg2.Visible = true;

                    LB_CostPer2.Text = strChargePercent;
                    IMB_ProCost2.Height = (Unit)deChargeHeight;

                    LB_BdgPer2.Text = strBudgetPercent;
                    IMB_ProBdg2.Height = (Unit)deBudgetHeight;

                    LB_ProBdg2.Text = strBudgetProjectID;
                    LB_ProBdg2.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost2.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg2.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 2)
                {
                    LB_CostPer3.Visible = true;
                    IMB_ProCost3.Visible = true;

                    LB_BdgPer3.Visible = true;
                    IMB_ProBdg3.Visible = true;

                    LB_ProBdg3.Visible = true;

                    LB_CostPer3.Text = strChargePercent;
                    IMB_ProCost3.Height = (Unit)deChargeHeight;

                    LB_BdgPer3.Text = strBudgetPercent;
                    IMB_ProBdg3.Height = (Unit)deBudgetHeight;

                    LB_ProBdg3.Text = strBudgetProjectID;
                    LB_ProBdg3.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost3.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg3.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 3)
                {
                    LB_CostPer4.Visible = true;
                    IMB_ProCost4.Visible = true;

                    LB_BdgPer4.Visible = true;
                    IMB_ProBdg4.Visible = true;

                    LB_ProBdg4.Visible = true;

                    LB_CostPer4.Text = strChargePercent;
                    IMB_ProCost4.Height = (Unit)deChargeHeight;

                    LB_BdgPer4.Text = strBudgetPercent;
                    IMB_ProBdg4.Height = (Unit)deBudgetHeight;


                    LB_ProBdg4.Text = strBudgetProjectID;
                    LB_ProBdg4.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost4.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg4.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 4)
                {
                    LB_CostPer5.Visible = true;
                    IMB_ProCost5.Visible = true;

                    LB_BdgPer5.Visible = true;
                    IMB_ProBdg5.Visible = true;

                    LB_ProBdg5.Visible = true;

                    LB_CostPer5.Text = strChargePercent;
                    IMB_ProCost5.Height = (Unit)deChargeHeight;

                    LB_BdgPer5.Text = strBudgetPercent;
                    IMB_ProBdg5.Height = (Unit)deBudgetHeight;

                    LB_ProBdg5.Text = strBudgetProjectID;
                    LB_ProBdg5.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost5.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg5.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 5)
                {
                    LB_CostPer6.Visible = true;
                    IMB_ProCost6.Visible = true;

                    LB_BdgPer6.Visible = true;
                    IMB_ProBdg6.Visible = true;

                    LB_ProBdg6.Visible = true;

                    LB_CostPer6.Text = strChargePercent;
                    IMB_ProCost6.Height = (Unit)deChargeHeight;

                    LB_BdgPer6.Text = strBudgetPercent;
                    IMB_ProBdg6.Height = (Unit)deBudgetHeight;

                    LB_ProBdg6.Text = strBudgetProjectID;
                    LB_ProBdg6.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost6.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg6.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 6)
                {
                    LB_CostPer7.Visible = true;
                    IMB_ProCost7.Visible = true;

                    LB_BdgPer7.Visible = true;
                    IMB_ProBdg7.Visible = true;

                    LB_ProBdg7.Visible = true;

                    LB_CostPer7.Text = strChargePercent;
                    IMB_ProCost7.Height = (Unit)deChargeHeight;

                    LB_BdgPer7.Text = strBudgetPercent;
                    IMB_ProBdg7.Height = (Unit)deBudgetHeight;

                    LB_ProBdg7.Text = strBudgetProjectID;
                    LB_ProBdg7.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost7.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg7.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 7)
                {
                    LB_CostPer8.Visible = true;
                    IMB_ProCost8.Visible = true;

                    LB_BdgPer8.Visible = true;
                    IMB_ProBdg8.Visible = true;

                    LB_ProBdg8.Visible = true;

                    LB_CostPer8.Text = strChargePercent;
                    IMB_ProCost8.Height = (Unit)deChargeHeight;

                    LB_BdgPer8.Text = strBudgetPercent;
                    IMB_ProBdg8.Height = (Unit)deBudgetHeight;

                    LB_ProBdg8.Text = strBudgetProjectID;
                    LB_ProBdg8.ToolTip = strConstractAmontToolTip;

                    IMB_ProBdg8.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg8.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 8)
                {
                    LB_CostPer9.Visible = true;
                    IMB_ProCost9.Visible = true;

                    LB_BdgPer9.Visible = true;
                    IMB_ProBdg9.Visible = true;

                    LB_ProBdg9.Visible = true;

                    LB_CostPer9.Text = strChargePercent;
                    IMB_ProCost9.Height = (Unit)deChargeHeight;

                    LB_BdgPer9.Text = strBudgetPercent;
                    IMB_ProBdg9.Height = (Unit)deBudgetHeight;

                    LB_ProBdg9.Text = strBudgetProjectID;
                    LB_ProBdg9.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost9.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg9.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }

                if (i == 9)
                {
                    LB_CostPer10.Visible = true;
                    IMB_ProCost10.Visible = true;

                    LB_BdgPer10.Visible = true;
                    IMB_ProBdg10.Visible = true;

                    LB_ProBdg10.Visible = true;

                    LB_CostPer10.Text = strChargePercent;
                    IMB_ProCost10.Height = (Unit)deChargeHeight;

                    LB_BdgPer10.Text = strBudgetPercent;
                    IMB_ProBdg10.Height = (Unit)deBudgetHeight;

                    LB_ProBdg10.Text = strBudgetProjectID;
                    LB_ProBdg10.ToolTip = strConstractAmontToolTip;

                    IMB_ProCost10.ToolTip = LanguageHandle.GetWord("YiFaShengFeiYong") + deRealCharge.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                    IMB_ProBdg10.ToolTip = LanguageHandle.GetWord("DangQianRiJiYuSuan") + deDefaultBudget.ToString() + LanguageHandle.GetWord("ZongYuSuan") + deBudget.ToString() + "," + strConstractAmontToolTip;
                }
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + HttpContext.Current.Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected decimal GetProjectRelatedConstractTotalAmount(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(Amount),0) From T_Constract Where ConstractCode in (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected decimal GetProjectRelatedConstractTotalReceivablesAmount(string strProjectID)
    {
        string strHQL;

        strHQL = "Select  COALESCE(sum(ReceivablesAccount),0) From T_ConstractReceivables Where ConstractCode in (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivables");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected decimal GetProjectRelatedConstractTotalReceiverAmount(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(ReceiverAccount),0) From T_ConstractReceivables Where ConstractCode in (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractReceivables");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }


    protected void BT_HazyFind_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strHQL, strFindCondition;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = "%" + TB_ProjectName.Text.Trim() + "%";

        strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
            "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
            "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.PMCode='" + strUserCode + "' and " +
            "C.Status not in ('Hided','Deleted','Archived') and C.ProjectName like '" + strFindCondition + "' Order by C.ProjectID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        ShareClass.FinishPercentPicture(DataGrid1, 0);
        RealChargeVisible();
        FinChargePercentByStep();
        ShareClass.FinChargePercentByRow(DataGrid1, 0);
        ShareClass.SetProjectStartAndEndTime(DataGrid1, 0);

        ShareClass.SetRiskLabelColor(DataGrid1, 0);
        ShareClass.SetDefectLabelColor(DataGrid1, 0);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMMBHZH") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;
    }

    protected void BT_ProjectIDFind_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strHQL, strFindCondition;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = TB_ProjectID.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();

        if (strFindCondition == "")
        {
            strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
                "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
                "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.PMCode='" + strUserCode + "' and " +
                "C.Status not in ('Hided','Deleted','Archived') Order by C.ProjectID DESC";
        }
        else
        {
            strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
                "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
                "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.PMCode='" + strUserCode + "' and " +
                "C.Status not in ('Hided','Deleted','Archived') and C.ProjectID = " + strFindCondition + " Order by C.ProjectID DESC";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        ShareClass.FinishPercentPicture(DataGrid1, 0);
        RealChargeVisible();
        FinChargePercentByStep();
        ShareClass.FinChargePercentByRow(DataGrid1, 0);
        ShareClass.SetProjectStartAndEndTime(DataGrid1, 0);

        ShareClass.SetRiskLabelColor(DataGrid1, 0);
        ShareClass.SetDefectLabelColor(DataGrid1, 0);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLiXiangRen") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;
    }

    protected void BT_MakeUserFind_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strHQL, strFindCondition;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = "%" + TB_MakeUser.Text.Trim() + "%";


        strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
            "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
            "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.PMCode='" + strUserCode + "' and " +
            "C.Status not in ('Hided','Deleted','Archived') and C.UserName like '" + strFindCondition + "' Order by C.ProjectID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        ShareClass.FinishPercentPicture(DataGrid1, 0);
        RealChargeVisible();
        FinChargePercentByStep();
        ShareClass.FinChargePercentByRow(DataGrid1, 0);
        ShareClass.SetProjectStartAndEndTime(DataGrid1, 0);

        ShareClass.SetRiskLabelColor(DataGrid1, 0);
        ShareClass.SetDefectLabelColor(DataGrid1, 0);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLiXiangRen") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;
    }

    protected void BT_DateFind_Click(object sender, EventArgs e)
    {
        string strStartDate, strEndDate, strUserCode;
        string strHQL;

        strStartDate = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyyMMdd");
        strEndDate = DateTime.Parse(DLC_EndDate.Text).ToString("yyyyMMdd");

        strUserCode = LB_UserCode.Text.Trim();

        strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
            "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
            "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.PMCode='" + strUserCode + "' and " +
            "C.Status not in ('Hided','Deleted','Archived') and to_char(C.BeginDate,'yyyymmdd') >= '" + strStartDate + "' and to_char(C.EndDate,'yyyymmdd') <= '" + strEndDate + "' Order by C.ProjectID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }
}