using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTConfirmProjectExpense : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strProjectID, strProjectName;

        string strHQL;
        IList lst;
        decimal deExpense = 0, deConfirmExpense = 0;

        strProjectID = Request.QueryString["ProjectID"];

        Project project = ShareClass.GetProject(strProjectID);
        strProjectName = project.ProjectName.Trim();

        LB_ProBudget.Text = project.Budget.ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", " aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;
            LB_ProjectID.Text = strProjectID;

            ShareClass.InitialProjectMemberTree(TreeView1, strProjectID);

            strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " Order by proExpense.ID DESC";
            ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
            lst = proExpenseBLL.GetAllProExpenses(strHQL);
            DataList1.DataSource = lst;
            DataList1.DataBind();

            ProExpense proExpense = new ProExpense();
            for (int i = 0; i < lst.Count; i++)
            {
                proExpense = (ProExpense)lst[i];
                deExpense += proExpense.Amount;
                deConfirmExpense += proExpense.ConfirmAmount;
            }

            LB_Member.Text = LanguageHandle.GetWord("SuoYouChenYuan");
            LB_Amount.Text = deExpense.ToString();
            LB_ConfirmAmount.Text = deConfirmExpense.ToString();

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll") + strUserCode + strUserName;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strOperatorCode, strOperatorName;
        string strProjectID = LB_ProjectID.Text.Trim();
        decimal deExpense = 0, deConfirmExpense = 0;

        string strID;

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

                strOperatorCode = proRelatedUser.UserCode.Trim();
                strOperatorName = proRelatedUser.UserName.Trim();

                strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strOperatorCode + "'" + " Order by proExpense.ID DESC";
                ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
                lst = proExpenseBLL.GetAllProExpenses(strHQL);
                DataList1.DataSource = lst;
                DataList1.DataBind();

                ProExpense proExpense = new ProExpense();
                for (int i = 0; i < lst.Count; i++)
                {
                    proExpense = (ProExpense)lst[i];
                    deExpense += proExpense.Amount;
                    deConfirmExpense += proExpense.ConfirmAmount;
                }

                LB_Member.Text = strOperatorCode + " " + strOperatorName;
                LB_Amount.Text = deExpense.ToString();
                LB_ConfirmAmount.Text = deConfirmExpense.ToString();

                LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll") + strUserCode + strUserName;
                LB_Sql.Text = strHQL;
            }
        }
        catch
        {
        }
    }


    protected void BT_AllMember_Click(object sender, EventArgs e)
    {
        decimal deExpense = 0, deConfirmExpense = 0;

        string strProjectID = LB_ProjectID.Text.Trim();
        string strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " Order by proExpense.ID DESC"; ;
        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        IList lst = proExpenseBLL.GetAllProExpenses(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        ProExpense proExpense = new ProExpense();
        for (int i = 0; i < lst.Count; i++)
        {
            proExpense = (ProExpense)lst[i];
            deExpense += proExpense.Amount;
            deConfirmExpense += proExpense.ConfirmAmount;
        }

        LB_Member.Text = LanguageHandle.GetWord("SuoYouChenYuan");
        LB_Amount.Text = deExpense.ToString();
        LB_ConfirmAmount.Text = deConfirmExpense.ToString();

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll");
    }

    protected void DataList1_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        string strID;
        decimal deConfirmExpense;
        string strHQL;
        string strOperatorCode, strProjectID;

        IList lst;

        if (e.CommandName == "Update")
        {
            strID = DataList1.DataKeys[e.Item.ItemIndex].ToString();


            strHQL = "from ProExpense as proExpense where proExpense.ID = " + strID;
            ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
            lst = proExpenseBLL.GetAllProExpenses(strHQL);
            ProExpense proExpense = (ProExpense)lst[0];

            try
            {
                deConfirmExpense = decimal.Parse(((TextBox)e.Item.FindControl("TB_ConfirmAmount")).Text);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSRZDJEGS") + "')", true);
                return;
            }

            try
            {
                proExpense.ConfirmAmount = deConfirmExpense;
                proExpenseBLL.UpdateProExpense(proExpense, int.Parse(strID));

                //更新项目日志中项目成员日工作费用
                strOperatorCode = proExpense.UserCode.Trim();
                strProjectID = proExpense.ProjectID.ToString();
                UpdateDailyProjectExpense(strUserCode, strProjectID, proExpense.RegisterDate);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXFYCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXFYSBJC") + "')", true);
            }
        }
    }


    //更新项目日志的项目成员日费用
    protected void UpdateDailyProjectExpense(string strOperatorCode, string strProjectID, DateTime dtWorkDate)
    {
        string strHQL;
        IList lst;
        decimal deAmount = 0, deConfirmAmount = 0;
        int intWorkID = 0;

        strHQL = "From ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strOperatorCode + "'" + " and to_char(proExpense.RegisterDate,'yyyymmdd') = " + dtWorkDate.ToString("yyyyMMdd");
        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        lst = proExpenseBLL.GetAllProExpenses(strHQL);

        ProExpense proExpense = new ProExpense();

        for (int i = 0; i < lst.Count; i++)
        {
            proExpense = (ProExpense)lst[i];
            deAmount += proExpense.ConfirmAmount;
            deConfirmAmount += proExpense.ConfirmAmount;
        }

        strHQL = "From DailyWork as dailyWork where dailyWork.ProjectID = " + strProjectID + " and dailyWork.UserCode = " + "'" + strOperatorCode + "'" + " and to_char(dailyWork.WorkDate,'yyyymmdd') = " + dtWorkDate.ToString("yyyyMMdd");
        DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
        lst = dailyWorkBLL.GetAllDailyWorks(strHQL);
        DailyWork dailyWork = new DailyWork();
        if (lst.Count > 0)
        {
            try
            {
                dailyWork = (DailyWork)lst[0];
                dailyWork.Charge = deAmount;
                dailyWork.ConfirmCharge = deConfirmAmount;
                intWorkID = dailyWork.WorkID;

                dailyWorkBLL.UpdateDailyWork(dailyWork, intWorkID);
            }
            catch
            {
            }
        }
        else
        {
            if (strOperatorCode == ShareClass.GetProjectPMCode(strProjectID))
            {
                dailyWork.Type = "Lead";   
            }
            else
            {
                dailyWork.Type = "Participate";  
            }
            dailyWork.UserCode = strOperatorCode;
            dailyWork.UserName = ShareClass.GetUserName(strOperatorCode);
            dailyWork.WorkDate = dtWorkDate;
            dailyWork.RecordTime = dtWorkDate;
            dailyWork.Address = "";
            dailyWork.ProjectID = int.Parse(strProjectID);
            dailyWork.ProjectName = ShareClass.GetProjectName(strProjectID);
            dailyWork.DailySummary = LanguageHandle.GetWord("ShuRuXiangMuXiangGuanFeiYong");
            dailyWork.Achievement = "";
            dailyWork.Charge = deAmount;
            dailyWork.ConfirmCharge = deConfirmAmount;
            dailyWork.ManHour = 0;
            dailyWork.ConfirmManHour = 0;
            dailyWork.Salary = 0;
            dailyWork.FinishPercent = 0;
            dailyWork.Bonus = 0;
            dailyWork.ConfirmBonus = 0;
            dailyWork.Authority = "NO";

            try
            {
                dailyWorkBLL.AddDailyWork(dailyWork);
            }
            catch
            {
            }
        }
    }

}
