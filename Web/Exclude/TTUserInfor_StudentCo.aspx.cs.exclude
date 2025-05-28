using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTUserInfor_StudentCo : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserCode"] != null)
        {
            strUserCode = Session["UserCode"].ToString();

        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataYearMonthBinder();
            TXT_CollectTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(Request.QueryString["StudentCode"]))
            {
                string strStudentCode = Request.QueryString["StudentCode"].ToString();
                HF_StudentCode.Value = strStudentCode;
                string strStudentName = Request.QueryString["StudentName"].ToString();
                HF_StudentName.Value = strStudentName;
                DataBinder(strStudentCode);

                LB_StudentCode.Text = strStudentCode;
                LB_StudentName.Text = strStudentName;
            }


        }
    }


    private void DataYearMonthBinder()
    {
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(-2).Year.ToString(), DateTime.Now.AddYears(-2).Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(-1).Year.ToString(), DateTime.Now.AddYears(-1).Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(1).Year.ToString(), DateTime.Now.AddYears(1).Year.ToString()));
        DDL_SYear.Items.Add(new ListItem(DateTime.Now.AddYears(2).Year.ToString(), DateTime.Now.AddYears(2).Year.ToString()));
        DDL_SYear.SelectedValue = DateTime.Now.Year.ToString();

        for (int i = 1; i <= 12; i++)
        {

            if (i < 10)
            {
                DDL_SMonth.Items.Add(new ListItem("0" + i, "0" + i));
            }
            else
            {
                DDL_SMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        string strCurrentMonth = DateTime.Now.Month.ToString();
        if (strCurrentMonth.Length > 1)
        {
            DDL_SMonth.SelectedValue = strCurrentMonth;
        }
        else
        {
            DDL_SMonth.SelectedValue = "0" + strCurrentMonth;
        }
    }




    private void DataBinder(string strStudentCode)
    {
        DG_List.CurrentPageIndex = 0;


        string strSeachYear = DDL_SYear.SelectedValue;
        string strSeachMonth = DDL_SMonth.SelectedValue;

        ProjectMemberStudentCostBLL projectMemberStudentCostBLL = new ProjectMemberStudentCostBLL();
        string strProjectMemberStudentCostHQL = string.Format("from ProjectMemberStudentCost as projectMemberStudentCost where StudentCode = '{0}' and to_char( CollectTime, 'yyyy-mm-dd' ) like '%{1}%' order by projectMemberStudentCost.id desc", strStudentCode, strSeachYear + "-" + strSeachMonth);
        IList listProjectMemberStudentCost = projectMemberStudentCostBLL.GetAllProjectMemberStudentCosts(strProjectMemberStudentCostHQL);

        DG_List.DataSource = listProjectMemberStudentCost;
        DG_List.DataBind();

        LB_QualitySql.Text = strProjectMemberStudentCostHQL;
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectMemberStudentCostBLL projectMemberStudentCostBLL = new ProjectMemberStudentCostBLL();
            ProjectMemberStudentCost projectMemberStudentCost = new ProjectMemberStudentCost();

            string strEditID = TXT_ID.Text.Trim();
            string strCollectTime = TXT_CollectTime.Text.Trim();

            DateTime dtCollectTime = DateTime.Now;
            DateTime.TryParse(strCollectTime, out dtCollectTime);

            string strCostProject = TXT_CostProject.Text.Trim();
            string strCostDemial = TXT_CostDemial.Text.Trim();

            decimal decimalCostDemial = 0;
            decimal.TryParse(strCostDemial, out decimalCostDemial);

            string strWangFeePerSemester = TXT_WangFeePerSemester.Text.Trim();
            decimal decimalWangFeePerSemester = 0;
            decimal.TryParse(strWangFeePerSemester, out decimalWangFeePerSemester);


            string strMeals = TXT_Meals.Text.Trim();
            decimal decimalMeals = 0;
            decimal.TryParse(strMeals, out decimalMeals);


            string strActivityCost = TXT_ActivityCost.Text.Trim();
            decimal decimalActivityCost = 0;
            decimal.TryParse(strActivityCost, out decimalActivityCost);


            string strCustodyAfterClass = TXT_CustodyAfterClass.Text.Trim();
            decimal decimalCustodyAfterClass = 0;
            decimal.TryParse(strCustodyAfterClass, out decimalCustodyAfterClass);


            string strReplaceCosts = TXT_ReplaceCosts.Text.Trim();
            decimal decimalReplaceCosts = 0;
            decimal.TryParse(strReplaceCosts, out decimalReplaceCosts);


            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strProjectMemberStudentCostHQL = string.Format(@"from ProjectMemberStudentCost as projectMemberStudentCost where ID = " + strID);
                IList lstProjectMemberStudentCost = projectMemberStudentCostBLL.GetAllProjectMemberStudentCosts(strProjectMemberStudentCostHQL);
                if (lstProjectMemberStudentCost != null && lstProjectMemberStudentCost.Count > 0)
                {
                    projectMemberStudentCost = (ProjectMemberStudentCost)lstProjectMemberStudentCost[0];

                    projectMemberStudentCost.CollectTime = dtCollectTime;
                    projectMemberStudentCost.CostProject = strCostProject;
                    projectMemberStudentCost.CostDemial = decimalCostDemial;

                    projectMemberStudentCost.WangFeePerSemester = decimalWangFeePerSemester;
                    projectMemberStudentCost.Meals = decimalMeals;
                    projectMemberStudentCost.ActivityCost = decimalActivityCost;
                    projectMemberStudentCost.CustodyAfterClass = decimalCustodyAfterClass;
                    projectMemberStudentCost.ReplaceCosts = decimalReplaceCosts;

                    projectMemberStudentCostBLL.UpdateProjectMemberStudentCost(projectMemberStudentCost, int.Parse(strID));
                }
            }
            else
            {
                projectMemberStudentCost.CollectTime = dtCollectTime;
                projectMemberStudentCost.CostProject = strCostProject;
                projectMemberStudentCost.CostDemial = decimalCostDemial;

                projectMemberStudentCost.WangFeePerSemester = decimalWangFeePerSemester;
                projectMemberStudentCost.Meals = decimalMeals;
                projectMemberStudentCost.ActivityCost = decimalActivityCost;
                projectMemberStudentCost.CustodyAfterClass = decimalCustodyAfterClass;
                projectMemberStudentCost.ReplaceCosts = decimalReplaceCosts;

                projectMemberStudentCost.StudentCode = HF_StudentCode.Value;
                projectMemberStudentCost.StudentName = HF_StudentName.Value;
                projectMemberStudentCost.CreatUserCode = strUserCode;

                projectMemberStudentCostBLL.AddProjectMemberStudentCost(projectMemberStudentCost);


            }

            DataBinder(HF_StudentCode.Value);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception ex) { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";

        TXT_CollectTime.Text = DateTime.Now.ToString("yyyy-MM-dd");


        TXT_CostProject.Text = "";
        TXT_CostDemial.Text = "";
    }


    protected void BT_Seach_Click(object sender, EventArgs e)
    {
        DataBinder(HF_StudentCode.Value);
    }

    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                ProjectMemberStudentCostBLL projectMemberStudentCostBLL = new ProjectMemberStudentCostBLL();
                string strProjectMemberStudentCostHQL = string.Format(@"from ProjectMemberStudentCost as ProjectMemberStudentCost where ID = " + strCmdArgu);
                IList lstProjectMemberStudentCost = projectMemberStudentCostBLL.GetAllProjectMemberStudentCosts(strProjectMemberStudentCostHQL);
                if (lstProjectMemberStudentCost != null && lstProjectMemberStudentCost.Count > 0)
                {
                    ProjectMemberStudentCost projectMemberStudentCost = (ProjectMemberStudentCost)lstProjectMemberStudentCost[0];

                    HF_ID.Value = projectMemberStudentCost.id.ToString();
                    TXT_ID.Text = projectMemberStudentCost.id.ToString();
                    TXT_CollectTime.Text = projectMemberStudentCost.CollectTime.ToString();

                    TXT_CostProject.Text = projectMemberStudentCost.CostProject;
                    TXT_CostDemial.Text = projectMemberStudentCost.CostDemial.ToString();

                    TXT_WangFeePerSemester.Text = projectMemberStudentCost.WangFeePerSemester.ToString();

                    TXT_Meals.Text = projectMemberStudentCost.Meals.ToString();

                    TXT_ActivityCost.Text = projectMemberStudentCost.ActivityCost.ToString();

                    TXT_CustodyAfterClass.Text = projectMemberStudentCost.CustodyAfterClass.ToString();

                    TXT_ReplaceCosts.Text = projectMemberStudentCost.ReplaceCosts.ToString();


                }
            }

            else if (cmdName == "Finished")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                ProjectMemberStudentCostBLL projectMemberStudentCostBLL = new ProjectMemberStudentCostBLL();
                string strProjectMemberStudentCostHQL = string.Format(@"from ProjectMemberStudentCost as projectMemberStudentCost where ID = " + strCmdArgu);
                IList lstProjectMemberStudentCost = projectMemberStudentCostBLL.GetAllProjectMemberStudentCosts(strProjectMemberStudentCostHQL);
                if (lstProjectMemberStudentCost != null && lstProjectMemberStudentCost.Count > 0)
                {
                    ProjectMemberStudentCost projectMemberStudentCost = (ProjectMemberStudentCost)lstProjectMemberStudentCost[0];

                    projectMemberStudentCost.Status = "FINISHED";

                    projectMemberStudentCostBLL.UpdateProjectMemberStudentCost(projectMemberStudentCost, int.Parse(strCmdArgu));


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSFCG").ToString().Trim() + "')", true);
                }
            }

            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                ProjectMemberStudentCostBLL projectMemberStudentCostBLL = new ProjectMemberStudentCostBLL();
                string strProjectMemberStudentCostHQL = string.Format(@"from ProjectMemberStudentCost as projectMemberStudentCost where ID = " + strCmdArgu);
                IList lstProjectMemberStudentCost = projectMemberStudentCostBLL.GetAllProjectMemberStudentCosts(strProjectMemberStudentCostHQL);
                if (lstProjectMemberStudentCost != null && lstProjectMemberStudentCost.Count > 0)
                {
                    ProjectMemberStudentCost projectMemberStudentCost = (ProjectMemberStudentCost)lstProjectMemberStudentCost[0];

                    projectMemberStudentCostBLL.DeleteProjectMemberStudentCost(projectMemberStudentCost);

                    DataBinder(HF_StudentCode.Value);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
            }
        }
        catch (Exception ex) { }
    }




}