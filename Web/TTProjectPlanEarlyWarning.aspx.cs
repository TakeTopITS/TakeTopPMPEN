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

using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProjectPlanEarlyWarning : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "项目的拖期计划", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            //string strLevel = DL_PlanLevel.SelectedValue.Trim();
            LoadWorkPlan("PM");

            LB_UserCode.Text = strUserCode;
        }
    }

    protected void DL_ActorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataGrid1.CurrentPageIndex = 0;

        string strActorType, strLevel;

        strActorType = DL_ActorType.SelectedValue.Trim();
        //strLevel = DL_PlanLevel.SelectedValue.Trim();

        LoadWorkPlan(strActorType);
    }

    protected void DL_PlanLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataGrid1.CurrentPageIndex = 0;

        string strActorType, strLevel;

        strActorType = DL_ActorType.SelectedValue.Trim();
        //strLevel = DL_PlanLevel.SelectedValue.Trim();

        LoadWorkPlan(strActorType);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectPlanList");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strPlanID, strPlanName, strHQL;
            IList lst;

            strPlanID = e.Item.Cells[2].Text.Trim();
            strPlanName = e.Item.Cells[3].Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            PlanMemberBLL planMemberBLL = new PlanMemberBLL();

            strPlanID = e.Item.Cells[0].Text.Trim();
            strHQL = "from PlanMember as planMember where planMember.PlanID = " + strPlanID + " order by planMember.ID ASC";
            lst = planMemberBLL.GetAllPlanMembers(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_Plan.Visible = true;
            LB_Plan.Text = Resources.lang.JiHua + ": " + strPlanID + " " + strPlanName + " "+LanguageHandle.GetWord("ChengYuan")+"：";

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName =  LanguageHandle.GetWord("TuoJiXiangMuJiHua") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

                string strHQL;
                string strActorType = DL_ActorType.SelectedValue.Trim();
                //string strLevel = DL_PlanLevel.SelectedValue.Trim();

                string strUserCode = Session["UserCode"].ToString();
                string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
                decimal deRelaydays = NB_RelayDays.Amount;

                strHQL = " select distinct ProjectID "+LanguageHandle.GetWord("XiangMuHao")+",ProjectName "+LanguageHandle.GetWord("XiangMuMing")+",PMCode "+LanguageHandle.GetWord("XiangMuJingLiDaiMa")+",PMName "+LanguageHandle.GetWord("XiangMuJingLiMingCheng")+", PlanID "+LanguageHandle.GetWord("JiHuaHao")+" ,PlanDetail "+LanguageHandle.GetWord("JiHuaMing")+",LeaderCode "+LanguageHandle.GetWord("FuZeRenDaiMa")+",Leader "+LanguageHandle.GetWord("FuZeRen")+",BeginTime "+LanguageHandle.GetWord("KaiShiShiJian")+",EndTime "+LanguageHandle.GetWord("JieShuShiJian")+",cast(ExpireDay as int) "+LanguageHandle.GetWord("TuoQiTianShu")+",";
                strHQL += " Percent_Done "+LanguageHandle.GetWord("JinDu")+",DefaultSchedule "+LanguageHandle.GetWord("BiaoZhunJinDu")+",Expense "+LanguageHandle.GetWord("FeiYong")+",DefaultCost "+LanguageHandle.GetWord("BiaoZhunChengBen")+",Budget "+LanguageHandle.GetWord("YuSuan")+" ";
                strHQL += " from V_ProjectPlanList";
                strHQL += " where PMCode = " + "'" + strUserCode + "'";

                if (strActorType == "LEADER")
                {
                    strHQL = " select distinct ProjectID "+LanguageHandle.GetWord("XiangMuHao")+",ProjectName "+LanguageHandle.GetWord("XiangMuMing")+",PMCode "+LanguageHandle.GetWord("XiangMuJingLiDaiMa")+",PMName "+LanguageHandle.GetWord("XiangMuJingLiMingCheng")+", PlanID "+LanguageHandle.GetWord("JiHuaHao")+" ,PlanDetail "+LanguageHandle.GetWord("JiHuaMing")+",LeaderCode "+LanguageHandle.GetWord("FuZeRenDaiMa")+",Leader "+LanguageHandle.GetWord("FuZeRen")+",BeginTime "+LanguageHandle.GetWord("KaiShiShiJian")+",EndTime "+LanguageHandle.GetWord("JieShuShiJian")+",cast(ExpireDay as int) "+LanguageHandle.GetWord("TuoQiTianShu")+",";
                    strHQL += " Percent_Done,DefaultSchedule "+LanguageHandle.GetWord("BiaoZhunJinDu")+",Expense,DefaultCost "+LanguageHandle.GetWord("BiaoZhunChengBen")+" ,Budget "+LanguageHandle.GetWord("YuSuan")+"";
                    strHQL += " from V_ProjectPlanList";
                    strHQL += " where PMCode in (Select UserCode From T_MemberLevel Where UserCode = " + "'" + strUserCode + "'" + " and ProjectVisible = 'YES' " + ")";            
                }

                if (strActorType == "SUPER")
                {
                    strHQL = " select distinct ProjectID "+LanguageHandle.GetWord("XiangMuHao")+",ProjectName "+LanguageHandle.GetWord("XiangMuMing")+",PMCode "+LanguageHandle.GetWord("XiangMuJingLiDaiMa")+",PMName "+LanguageHandle.GetWord("XiangMuJingLiMingCheng")+", PlanID "+LanguageHandle.GetWord("JiHuaHao")+" ,PlanDetail "+LanguageHandle.GetWord("JiHuaMing")+",LeaderCode "+LanguageHandle.GetWord("FuZeRenDaiMa")+",Leader "+LanguageHandle.GetWord("FuZeRen")+",BeginTime "+LanguageHandle.GetWord("KaiShiShiJian")+",EndTime "+LanguageHandle.GetWord("JieShuShiJian")+",cast(ExpireDay as int) "+LanguageHandle.GetWord("TuoQiTianShu")+",";
                    strHQL += " Percent_Done,DefaultSchedule "+LanguageHandle.GetWord("BiaoZhunJinDu")+",Expense,DefaultCost "+LanguageHandle.GetWord("BiaoZhunChengBen")+",Budget "+LanguageHandle.GetWord("YuSuan")+"";
                    strHQL += " from V_ProjectPlanList";
                    strHQL += " where PMCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
                }

                strHQL += " and Expireday > " + deRelaydays.ToString();
                strHQL += " and ParentIDGantt > 0";
                strHQL += " and Percent_Done < 100";
                strHQL += " and PlanID not In (Select Parent_ID From T_ImplePlan)";
                strHQL += " Order by ProjectID ASC,BeginTime ASC,EndTime ASC";
                MSExcelHandler.DataTableToExcel(strHQL, fileName);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGDCDSJYWJC + "')", true);
            }
        }
    }

    protected void LoadWorkPlan(string strActorType)
    {
        string strHQL;
        string strUserCode = Session["UserCode"].ToString();
        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        decimal deRelaydays = NB_RelayDays.Amount;

        strHQL = " select distinct PlanID,PlanDetail,BeginTime,EndTime,Budget,cast(ExpireDay as int) as ExpireDay,Status,ParentIDGantt,LeaderCode,Leader,";
        strHQL += " PriorID,Type,VerID,Percent_Done,DefaultSchedule,Expense,DefaultCost,ProjectID,ProjectName,PMCode,PMName  ";
        strHQL += " from V_ProjectPlanList";
        strHQL += " where PMCode = " + "'" + strUserCode + "'";

        if (strActorType == "LEADER")
        {
            strHQL = " select distinct PlanID,PlanDetail,BeginTime,EndTime,Budget,cast(ExpireDay as int) as ExpireDay,Status,ParentIDGantt,LeaderCode,Leader,";
            strHQL += " PriorID,Type,VerID,Percent_Done,DefaultSchedule,Expense,DefaultCost,ProjectID,ProjectName,PMCode,PMName  ";
            strHQL += " from V_ProjectPlanList";
            strHQL += " where PMCode in (Select UserCode From T_MemberLevel Where UserCode = " + "'" + strUserCode + "'" + " and ProjectVisible = 'YES' " + ")";     
        }

        if (strActorType == "SUPER")
        {
            strHQL = " select distinct PlanID,PlanDetail,BeginTime,EndTime,Budget,cast(ExpireDay as int) as ExpireDay,Status,ParentIDGantt,LeaderCode,Leader,";
            strHQL += " PriorID,Type,VerID,Percent_Done,DefaultSchedule,Expense,DefaultCost,ProjectID,ProjectName,PMCode,PMName  ";
            strHQL += " from V_ProjectPlanList";
            strHQL += " where PMCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
        }

       

        strHQL += " and Expireday > " + deRelaydays.ToString();
        strHQL += " and ParentIDGantt > 0";
        strHQL += " and Percent_Done < 100";
        strHQL += " and PlanID not In (Select Parent_ID From T_ImplePlan)";
        strHQL += " Order by ProjectID ASC,BeginTime ASC,EndTime ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_ProjectPlanList");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Number.Text = ds.Tables[0].Rows.Count.ToString();
        LB_Sql.Text = strHQL;
    }
}
