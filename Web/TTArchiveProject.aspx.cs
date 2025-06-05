using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TTArchiveProject : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件2006－2012

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        string strHQL;


        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "项目归档", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ShareClass.LoadSytemChart(strUserCode, "AllProject", RP_ChartList);
        HL_SystemAnalystChartRelatedUserSet.NavigateUrl = "TTSystemAnalystChartRelatedUserSet.aspx?FormType=AllProject";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadAllProject(strDepartString);
            SetProRecordColor();

            ShareClass.FinishPercentPicture(DataGrid3, 0);

            RealChargeVisible();
            FinChargePercentByStep();

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll");

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);

            ShareClass.LoadProjectStatusForDataGrid(strLangCode, DataGrid2);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen") + strDepartName;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            strHQL = "from Project as project where (project.PMCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") Or project.BelongDepartCode = '" + strDepartCode + "') Order by project.ProjectID DESC";
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            SetProRecordColor();

            ShareClass.FinishPercentPicture(DataGrid3, 0);

            RealChargeVisible();
            FinChargePercentByStep();

            BT_StatusVisible.Visible = true;
            BT_StatusVisible.Text = LanguageHandle.GetWord("ShowStatus");

            DataGrid2.Visible = false;

            LB_Sql.Text = strHQL;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strOperatorName = ShareClass.GetUserName(strOperatorCode);

        string strHQL = "from Project as project where project.PMCode = " + "'" + strOperatorCode + "'" + " order by project.ProjectID DESC";

        LB_OperatorCode.Text = strOperatorCode;
        LB_OperatorName.Text = strOperatorName;

        LB_DepartCode.Text = "";

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll") + strOperatorCode + strOperatorName;

        ProjectBLL projectBLL = new ProjectBLL();

        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();


        ShareClass.FinishPercentPicture(DataGrid3, 0);

        RealChargeVisible();
        FinChargePercentByStep();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStatus = ((Button)e.Item.FindControl("BT_Status")).Text;
        string strHQL;
        string strOperatorCode = LB_OperatorCode.Text;
        string strOperatorName;
        string strDepartCode, strDepartName;

        if (LB_OperatorCode.Text == "")
        {
            strHQL = "from Project as project where project.Status = " + "'" + strStatus + "'" + " order by project.ProjectID DESC";

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll") + LanguageHandle.GetWord("ZZXMZT") + strStatus;

        }
        else
        {
            if (LB_DepartCode.Text == "")
            {
                strOperatorName = ShareClass.GetUserName(strOperatorCode);
                strHQL = "from Project as project where project.PMCode = " + "'" + strOperatorCode + "'" + " and " + "project.Status = " + "'" + strStatus + "'" + " order by project.ProjectID DESC";
                LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll") + strOperatorCode + strOperatorName + LanguageHandle.GetWord("ZZXMZT") + strStatus;
            }
            else
            {
                strDepartCode = LB_DepartCode.Text.Trim();
                strDepartName = LB_DepartName.Text.Trim();
                strHQL = "from Project as project where (project.PMCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") Or Or project.BelongDepartCode = '" + strDepartCode + "') and " + "project.Status = " + "'" + strStatus + "'" + " order by project.ProjectID DESC";
                LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen") + strDepartName + LanguageHandle.GetWord("ZZXMZT") + strStatus;
            }
        }

        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();


        ShareClass.FinishPercentPicture(DataGrid3, 0);

        RealChargeVisible();
        FinChargePercentByStep();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strProjectID, strHQL, strStatus;
        IList lst;

        if (e.CommandName != "Page")
        {
            strProjectID = e.Item.Cells[0].Text.Trim();
            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            Project project = (Project)lst[0];

            strStatus = project.Status.Trim();

            if (e.CommandName == "Archive")
            {
                if (strStatus == "Archived")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXMYGDBYZJXGDDZLJC") + "')", true);
                    return;
                }

                if (strStatus != "CaseClosed")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXMHMYJABNGDJC") + "')", true);
                    return;
                }

                project.Status = "Archived";

                try
                {
                    projectBLL.UpdateProject(project, int.Parse(strProjectID));

                    AddStatusChangeRecord(strProjectID, "CaseClosed", "Archived");

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG") + "')", true);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDSBJC") + "')", true);

                }
            }
            else
            {
                if (strStatus != "Archived")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXMZTBWGDBNCXJC") + "')", true);

                    return;
                }

                project.Status = "CaseClosed";

                try
                {
                    projectBLL.UpdateProject(project, int.Parse(strProjectID));
                    AddStatusChangeRecord(strProjectID, "Archived", "CaseClosed");

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCheXiaoLanguageHandleGetWord")+"')", true); 
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCheXiaoLanguageHandleGetWord")+"')", true); 
                }
            }

            LoadAllProject(LB_DepartString.Text.Trim());
            SetProRecordColor();
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strDepartCode = ((Button)e.Item.FindControl("BT_DepartCode")).Text.Trim();
            string strDepartName = ((Button)e.Item.FindControl("BT_DepartName")).Text.Trim();

            DataGrid1.Visible = true;

            LB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = strDepartName;

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen") + strDepartName;

            string strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            strHQL = "from Project as project where (project.PMCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") Or project.BelongDepartCode = '" + strDepartCode + "') Order by project.ProjectID DESC";
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            SetProRecordColor();


            ShareClass.FinishPercentPicture(DataGrid3, 0);

            RealChargeVisible();
            FinChargePercentByStep();

            BT_StatusVisible.Visible = true;
            BT_StatusVisible.Text = LanguageHandle.GetWord("ShowStatus");

            DataGrid2.Visible = false;
            LB_Sql.Text = strHQL;
        }
    }

    protected void BT_AllProject_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll");

        DataGrid1.Visible = false;

        LB_DepartCode.Text = "";
        LB_OperatorCode.Text = "";

        strHQL = "from Project as project order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();


        ShareClass.FinishPercentPicture(DataGrid3, 0);

        RealChargeVisible();
        FinChargePercentByStep();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();


        ShareClass.FinishPercentPicture(DataGrid3, 0);

        RealChargeVisible();
        FinChargePercentByStep();
    }

    protected void BT_StatusVisible_Click(object sender, EventArgs e)
    {
        string strBTText = BT_StatusVisible.Text.Trim();

        if (strBTText == LanguageHandle.GetWord("ShowStatus"))
        {
            DataGrid2.Visible = true;
            BT_StatusVisible.Text = LanguageHandle.GetWord("HideStatus");
        }
        else
        {
            DataGrid2.Visible = false;
            BT_StatusVisible.Text = LanguageHandle.GetWord("ShowStatus");
        }
    }

    protected void SetProRecordColor()
    {
        int i;
        DateTime dtNowDate, dtFinishedDate;
        string strStatus;

        for (i = 0; i < DataGrid3.Items.Count; i++)
        {
            dtFinishedDate = DateTime.Parse(DataGrid3.Items[i].Cells[5].Text.Trim());
            dtNowDate = DateTime.Now;
            strStatus = DataGrid3.Items[i].Cells[8].Text.Trim();

            if (strStatus != "CaseClosed" & strStatus != "Archived")
            {
                if (dtFinishedDate < dtNowDate)
                {
                    DataGrid3.Items[i].ForeColor = Color.Red;
                }
            }
        }
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

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        ProRealChargeBLL proRealChargeBLL = new ProRealChargeBLL();
        ProRealCharge proRealCharge = new ProRealCharge();

        for (i = 0; i < DataGrid3.Items.Count; i++)
        {
            strProjectID = DataGrid3.Items[i].Cells[0].Text.Trim();

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
        string strUserCode, strFindCondition, strDepartString;
        string strHQL;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = "%" + TB_ProjectName.Text.Trim() + "%";
        strDepartString = LB_DepartString.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectName like " + "'" + strFindCondition + "'";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " order by project.ProjectID DESC";
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();


        ShareClass.FinishPercentPicture(DataGrid3, 0);

        RealChargeVisible();
        FinChargePercentByStep();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMMBHZH") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;
    }

    protected void BT_ProjectIDFind_Click(object sender, EventArgs e)
    {
        string strUserCode, strDepartString, strFindCondition;
        IList lst;
        string strHQL;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = TB_ProjectID.Text.Trim();
        strDepartString = LB_DepartString.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();

        if (strFindCondition == "")
        {
            strHQL = "from Project as project";
        }
        else
        {
            strHQL = "from Project as project where project.ProjectID = " + strFindCondition;
        }

        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += "order by project.ProjectID DESC";

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 0);

        RealChargeVisible();
        FinChargePercentByStep();

        LB_QueryScope.Text = LanguageHandle.GetWord("Project") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;
    }

    protected void BT_MakeUserFind_Click(object sender, EventArgs e)
    {
        string strUserCode, strDepartString, strFindCondition;

        string strHQL;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = "%" + TB_MakeUser.Text.Trim() + "%";
        strDepartString = LB_DepartString.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.UserName like " + "'" + strFindCondition + "'";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " order by project.ProjectID DESC";
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 0);

        RealChargeVisible();
        FinChargePercentByStep();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLiXiangRen") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");
        LB_Sql.Text = strHQL;
    }

    protected void LoadAllProject(string strDepartString)
    {
        string strHQL;
        IList lst;

        ProjectBLL projectBLL = new ProjectBLL();

        strHQL = "from Project as project ";
        strHQL += " Where project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " order by project.ProjectID DESC ";
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void AddStatusChangeRecord(string strProjectID, string strOldSatus, string strNewStatus)
    {
        string strUserCode, strUserName;

        if (strOldSatus != strNewStatus)
        {
            strUserCode = LB_UserCode.Text.Trim();
            strUserName = ShareClass.GetUserName(strUserCode);

            ProStatusChangeBLL proStatusChangeBLL = new ProStatusChangeBLL();
            ProStatusChange proStatusChange = new ProStatusChange();

            proStatusChange.ProjectID = int.Parse(strProjectID);
            proStatusChange.UserCode = strUserCode;
            proStatusChange.UserName = strUserName;
            proStatusChange.OldStatus = strOldSatus;
            proStatusChange.NewStatus = strNewStatus;
            proStatusChange.ChangeTime = DateTime.Now;

            try
            {
                proStatusChangeBLL.AddProStatusChange(proStatusChange);
            }
            catch
            {

            }
        }
    }

    protected void BT_DateFind_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strStartDate, strEndDate, strUserCode, strDepartString;

        strStartDate = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyyMMdd");
        strEndDate = DateTime.Parse(DLC_EndDate.Text).ToString("yyyyMMdd");

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from Project as project where to_char(project.BeginDate,'yyyymmdd') >= " + "'" + strStartDate + "'" + " and to_char(project.EndDate,'yyyymmdd') <= " + "'" + strEndDate + "'";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

}

