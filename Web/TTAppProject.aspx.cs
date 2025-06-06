using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TTAppProject : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ��jack.erp@gmail.com)
        //̩��������TakeTop Software)2006��2012

        string strHQL;
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();
       

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "Project", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType != "SAAS" | strProductType == "LOCALSAAS")
        {
            TBL_ProjectCode.Visible = false;
        }
      

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "SetDataGridTrClickLink();", true);
        if (Page.IsPostBack != true)
        {
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;

            strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
                "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
                "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.PMCode='" + strUserCode + "' and " +
                "C.Status not in ('New','Hided','Deleted','Archived') Order by C.ProjectID DESC";

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();
            LB_Sql1.Text = strHQL;


            strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from V_ProRelatedUser C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
               "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
               "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.UserCode='" + strUserCode + "' and " +
               "C.ProStatus not in ('New','Review','Hided','Deleted','Archived','Pause','Stop') and C.PMCode <> '" + strUserCode + "' Order by C.ProjectID DESC";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();
            LB_Sql2.Text = strHQL;


            strHQL = "select C.*,COALESCE(D.TotalBL,0) PercentRea from T_Project C left join (select A.ProjectID,COALESCE(B.TotalRea,0)/CASE WHEN A.Total = 0 Then 1 END as TotalBL from (select " +
                "ProjectID,SUM(Total) Total from T_ProjectCostManage Where Type='Base' group by ProjectID) A left join (select ProjectID,SUM(Total) TotalRea from " +
                "T_ProjectCostManage where Type='Operation' group by ProjectID) B on A.ProjectID=B.ProjectID) D on C.ProjectID=D.ProjectID where C.UserCode='" + strUserCode + "' and " +
                "C.Status not in ('New','Hided','Deleted','Archived') Order by C.ProjectID DESC";

            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");
            DataGrid3.DataSource = ds;
            DataGrid3.DataBind();
            LB_Sql3.Text = strHQL;
        }
    }


    protected void BT_AddProject_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode, strUserName, strPMCode;
        string strProjectID, strProjectCode, strProjectName;

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        strProjectCode = TB_ProjectCode.Text.Trim();

        if (strProjectCode == "")
        {
            return;
        }

        strHQL = "Select ProjectID,ProjectName,PMCode From T_Project Where ProjectCode = " + "'" + strProjectCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds.Tables[0].Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMDMBCZQJC") + "')", true);
            return;
        }
        strProjectID = ds.Tables[0].Rows[0][0].ToString();
        strProjectName = ds.Tables[0].Rows[0][1].ToString();
        strPMCode = ds.Tables[0].Rows[0][2].ToString();

        strHQL = "Select * From T_ContactInfor Where UserCode = " + "'" + strPMCode + "' and  MobilePhone = " + "'" + strUserCode + "'";
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ContactInfor");
        if (ds1.Tables[0].Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZNBZCXMYYCYZZQJC") + "')", true);
            return;
        }

        strHQL = "Select * From T_MemberLevel Where UserCode = '" + strPMCode + "' and UnderCode = '" + strUserCode + "'";
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");
        if (ds2.Tables[0].Rows.Count == 0)
        {
            strHQL = "Insert Into T_MemberLevel(UserCode  ,UnderCode ,AgencyStatus,ProjectVisible ,PlanVisible  ,KPIVisible ,WorkloadVisible ,ScheduleVisible ,WorkflowVisible,CustomerServiceVisible ,ConstractVisible,PositionVisible,SortNumber)";
            strHQL += " values('" + strPMCode + "','" + strUserCode + "',1,'YES','YES','YES','YES','YES','YES','YES','YES','YES',1)";
            ShareClass.RunSqlCommand(strHQL);
        }

        strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID + " and UserCode =" + "'" + strUserCode + "'";
        DataSet ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");
        if (ds3.Tables[0].Rows.Count == 0)
        {
            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            RelatedUser relatedUser = new RelatedUser();

            relatedUser.ProjectID = int.Parse(strProjectID);
            relatedUser.ProjectName = strProjectName;
            relatedUser.UserCode = strUserCode;
            relatedUser.UserName = strUserName;
            relatedUser.DepartCode = ShareClass.GetDepartCodeFromUserCode("ADMIN");
            relatedUser.DepartName = ShareClass.GetDepartName(ShareClass.GetDepartCodeFromUserCode("ADMIN"));
            relatedUser.Actor = "ProjectMember";
            relatedUser.JoinDate = DateTime.Now;
            relatedUser.LeaveDate = DateTime.Parse("2099-12-31");
            relatedUser.Status = "Plan";
            relatedUser.WorkDetail = "";
            relatedUser.SMSCount = 0;
            relatedUser.SalaryMethod = LanguageHandle.GetWord("GongShi");
            relatedUser.PromissionScale = 0;
            relatedUser.UnitHourSalary = 0;
            relatedUser.CanUpdatePlan = "NO";

            relatedUserBLL.AddRelatedUser(relatedUser);
        }

        Response.Redirect("TTAPPProject.aspx");
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql1.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql2.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCost");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }


}