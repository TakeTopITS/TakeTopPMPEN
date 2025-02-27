using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTConfirmProjectManHour : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strProjectID, strProjectName;
        string strUserCode, strUserName;
        string strHQL;
        decimal deExpense = 0, deManHour = 0, deConfirmManHour = 0, deSalary = 0, deConfirmSalary = 0;
        IList lst;

        strProjectID = Request.QueryString["ProjectID"];
        strProjectName = ShareClass.GetProjectName(strProjectID);

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        //this.Title = "项目：" + strProjectName + "工时确认！";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;
            LB_ProjectID.Text = strProjectID;

            ShareClass.InitialProjectMemberTree(TreeView1, strProjectID);

            strHQL = "from ProjectDailyWork as projectDailyWork where projectDailyWork.ProjectID = " + strProjectID + " order by projectDailyWork.WorkDate DESC";

            ProjectDailyWorkBLL projectDailyWorkBLL = new ProjectDailyWorkBLL();
            lst = projectDailyWorkBLL.GetAllProjectDailyWorks(strHQL);
            DataList1.DataSource = lst;
            DataList1.DataBind();

            ProjectDailyWork projectDailyWork = new ProjectDailyWork();

            for (int i = 0; i < lst.Count; i++)
            {
                projectDailyWork = (ProjectDailyWork)lst[i];
                deExpense += projectDailyWork.Charge;
                deManHour += projectDailyWork.ManHour;
                deSalary += projectDailyWork.DailySalary;
                deConfirmManHour += projectDailyWork.ConfirmManHour;
                deConfirmSalary += projectDailyWork.ConfirmDailySalary;
            }

            LB_Amount.Text = deExpense.ToString();
            LB_ManHour.Text = deManHour.ToString();
            LB_Income.Text = deSalary.ToString();
            LB_ConfirmManHour.Text = deConfirmManHour.ToString();
            LB_ConfirmIncome.Text = deConfirmSalary.ToString();

            LB_Member1.Text = LanguageHandle.GetWord("SuoYouChenYuan").ToString().Trim();
            LB_Member2.Text = LanguageHandle.GetWord("SuoYouChenYuan").ToString().Trim();


            LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMJL").ToString().Trim() + strUserCode + " " + strUserName;
            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        decimal deExpense = 0, deManHour = 0, deConfirmManHour = 0, deSalary = 0, deConfirmSalary = 0;

        string strHQL;
        IList lst;

        string strProjectID, strID;
        string strUserCode, strUserName;


        strProjectID = LB_ProjectID.Text.Trim();
        strID = TreeView1.SelectedNode.Target.Trim();

        try
        {
            strID = int.Parse(strID).ToString();

            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ID = " + strID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ProRelatedUser proRelatedUser = (ProRelatedUser)lst[0];

                strUserCode = proRelatedUser.UserCode.Trim();
                strUserName = proRelatedUser.UserName.Trim();

                strHQL = "from ProjectDailyWork as projectDailyWork where projectDailyWork.ProjectID = " + strProjectID + " and projectDailyWork.UserCode = " + "'" + strUserCode + "'" + " order by projectDailyWork.WorkID DESC";
                ProjectDailyWorkBLL projectDailyWorkBLL = new ProjectDailyWorkBLL();
                lst = projectDailyWorkBLL.GetAllProjectDailyWorks(strHQL);

                DataList1.DataSource = lst;
                DataList1.DataBind();

                ProjectDailyWork projectDailyWork = new ProjectDailyWork();

                for (int i = 0; i < lst.Count; i++)
                {
                    projectDailyWork = (ProjectDailyWork)lst[i];
                    deExpense += projectDailyWork.Charge;
                    deManHour += projectDailyWork.ManHour;
                    deSalary += projectDailyWork.DailySalary;
                    deConfirmManHour += projectDailyWork.ConfirmManHour;
                    deConfirmSalary += projectDailyWork.ConfirmDailySalary;
                }

                LB_Amount.Text = deExpense.ToString();
                LB_ManHour.Text = deManHour.ToString();
                LB_Income.Text = deSalary.ToString();
                LB_ConfirmManHour.Text = deConfirmManHour.ToString();
                LB_ConfirmIncome.Text = deConfirmSalary.ToString();

                LB_Member1.Text = strUserCode + " " + strUserName;
                LB_Member2.Text = strUserCode + " " + strUserName;

                LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZhe").ToString().Trim() + strUserCode + strUserName;
                LB_Sql.Text = strHQL;
            }
        }
        catch
        {
        }
    }


    protected void BT_AllMember_Click(object sender, EventArgs e)
    {
        decimal deExpense = 0, deManHour = 0, deConfirmManHour = 0, deSalary = 0, deConfirmSalary = 0;
        string strProjectID = LB_ProjectID.Text.Trim();
        string strHQL = "from ProjectDailyWork as projectDailyWork where projectDailyWork.ProjectID = " + strProjectID + " order by projectDailyWork.WorkDate DESC";
        ProjectDailyWorkBLL projectDailyWorkBLL = new ProjectDailyWorkBLL();
        IList lst = projectDailyWorkBLL.GetAllProjectDailyWorks(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        ProjectDailyWork projectDailyWork = new ProjectDailyWork();

        for (int i = 0; i < lst.Count; i++)
        {
            projectDailyWork = (ProjectDailyWork)lst[i];
            deExpense += projectDailyWork.Charge;
            deManHour += projectDailyWork.ManHour;
            deSalary += projectDailyWork.DailySalary;
            deConfirmManHour += projectDailyWork.ConfirmManHour;
            deConfirmSalary += projectDailyWork.ConfirmDailySalary;
        }

        LB_Amount.Text = deExpense.ToString();
        LB_ManHour.Text = deManHour.ToString();
        LB_Income.Text = deSalary.ToString();
        LB_ConfirmManHour.Text = deConfirmManHour.ToString();
        LB_ConfirmIncome.Text = deConfirmSalary.ToString();

        LB_Member1.Text = LanguageHandle.GetWord("SuoYouChenYuan").ToString().Trim();
        LB_Member2.Text = LanguageHandle.GetWord("SuoYouChenYuan").ToString().Trim();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim();
    }

    protected void DataList1_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        string strWorkID;
        decimal deConfirmManHour, deUnitHourSalary;
        string strHQL;
        IList lst;

        if (e.CommandName == "Update")
        {
            strWorkID = DataList1.DataKeys[e.Item.ItemIndex].ToString();
            strHQL = "from DailyWork as dailyWork where dailyWork.WorkID = " + strWorkID;
            DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
            lst = dailyWorkBLL.GetAllDailyWorks(strHQL);

            DailyWork dailyWork = (DailyWork)lst[0];

            try
            {
                deConfirmManHour = decimal.Parse(((TextBox)e.Item.FindControl("TB_ConfirmManHour")).Text);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSRZDSZGS").ToString().Trim() + "')", true);
                return;
            }

            dailyWork.ConfirmManHour = deConfirmManHour;

            deUnitHourSalary = GetUnitHourSalary(dailyWork.UserCode.Trim(), dailyWork.ProjectID.ToString());
            dailyWork.Salary = deUnitHourSalary * deConfirmManHour;


            try
            {
                dailyWorkBLL.UpdateDailyWork(dailyWork, int.Parse(strWorkID));
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                LoadDailyWork();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
            }
        }
    }

    protected void LoadDailyWork()
    {
        string strHQL;
        IList lst;

        strHQL = LB_Sql.Text.Trim();
        ProjectDailyWorkBLL projectDailyWorkBLL = new ProjectDailyWorkBLL();
        lst = projectDailyWorkBLL.GetAllProjectDailyWorks(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();
    }


    protected decimal GetUnitHourSalary(string strUserCode, string strProjectID)
    {
        decimal deUnitHourSalary;
        string strHQL;
        IList lst;

        strHQL = "from RelatedUser as relatedUser where relatedUser.UserCode = " + "'" + strUserCode + "'" + " and relatedUser.ProjectID = " + strProjectID;
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
        RelatedUser relatedUser = (RelatedUser)lst[0];

        deUnitHourSalary = relatedUser.UnitHourSalary;

        return deUnitHourSalary;
    }

}
