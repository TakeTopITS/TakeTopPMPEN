using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using System.Security.Cryptography;
using System.Security.Permissions;
using System.Data.SqlClient;

using System.Globalization;
using System.Threading;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Events;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAPPMyMemProjectsRunStatus : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

     
        ShareClass.LoadSytemChart(strUserCode, "MemberProject", RP_ChartList);
        HL_SystemAnalystChartRelatedUserSet.NavigateUrl = "TTSystemAnalystChartRelatedUserSet.aspx?FormType=MemberProject";

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            int intIntervalTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerInterval"]);
            Timer_Refresh.Interval = intIntervalTime;

            strHQL = "from Project as project where  ";
            strHQL += " project.PMCode in ( select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ") ";
            strHQL += " and project.Status not in ('Deleted','Archived') ";
            strHQL += " Order by project.ProjectID DESC";
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
            LB_Sql.Text = strHQL;

            SetProRecordColor();
            //ShareClass.FinishPercentPicture(DataGrid3, 1);
            //ShareClass.FinChargePercentByRow(DataGrid3, 1);
            SetProjectStartAndEndTime(DataGrid3, 0);

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZLeader") + strUserCode + " " + strUserName;

            ShareClass.LoadMemberByUserCodeForDataGrid(strUserCode, "Project", DataGrid1);

            strHQL = string.Format(@"select distinct A.Status, rtrim(A.HomeName) as HomeName,A.SortNumber from T_ProjectStatus A 
                Where A.LangCode ='{0}' 
                and A.SortNumber in (Select min(B.SortNumber) From T_ProjectStatus B Where B.LangCode = '{0}' and A.HomeName = B.HomeName)
                Order By A.SortNumber ASC", strLangCode);
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectStatus");
            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();

        }
    }

      //设置项目时间和超期天数
    public static void SetProjectStartAndEndTime(DataGrid dataGrid, int intCellNumber)
    {
        int i;
        DateTime dtNowDate, dtBeginDate, dtEndDate;
        string strProjectID, strProjectStatus, strProjectStatusValue;

        for (i = 0; i < dataGrid.Items.Count; i++)
        {
            strProjectID = dataGrid.Items[i].Cells[intCellNumber].Text.Trim();

            Project Project = ShareClass.GetProject(strProjectID);

            strProjectStatus = Project.Status.Trim();
            strProjectStatusValue = Project.StatusValue.Trim();

            dtBeginDate = Project.BeginDate;
            dtEndDate = Project.EndDate;
            dtNowDate = DateTime.Now;

            TimeSpan sp = dtNowDate.Subtract(dtEndDate);
            int intDays = sp.Days;

            ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BeginDate")).Text = dtBeginDate.ToString("yyyy-MM-dd");
            ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_EndDate")).Text = dtEndDate.ToString("yyyy-MM-dd");

            if (intDays > 0)
            {
                if (strProjectStatus == "CaseClosed" | strProjectStatus == "Cancel")
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ChaoQi");
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = "0";
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).BackColor = Color.White;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DayUnit")).ForeColor = Color.Green;
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ChaoQi");
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = intDays.ToString();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BeginDate")).BackColor = Color.Red;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_EndDate")).BackColor = Color.Red;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BeginDate")).ForeColor = Color.Yellow;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_EndDate")).ForeColor = Color.Yellow;
                }
            }
            else
            {
                if (strProjectStatus == "CaseClosed" | strProjectStatus == "Cancel")
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ShengYu");
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = "0";
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).BackColor = Color.White;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DayUnit")).ForeColor = Color.Green;
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ShengYu");
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = (0 - intDays).ToString();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).BackColor = Color.White;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DayUnit")).ForeColor = Color.Green;
                }
            }
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        IList lst;

        string strUnderlingCode = ((Button)e.Item.FindControl("BT_UnderlingCode")).Text;

        string strUnderLingName = ShareClass.GetUserName(strUnderlingCode);

        string strUserCode = LB_UserCode.Text;

        string strUserName = ShareClass.GetUserName(strUserCode);

        string strHQL = "from Project as project where project.PMCode = " + "'" + strUnderlingCode + "'";
        strHQL += "  and project.Status not in ('Deleted','Archived') Order by project.ProjectID DESC";

        LB_Underling.Text = strUnderlingCode;

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLeader") + strUserCode + strUserName + LanguageHandle.GetWord("ZZXiaShu") + strUnderlingCode + strUnderLingName;

        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStatus = ((Button)e.Item.FindControl("BT_Status")).Text;

        string strHQL;
        string strUserCode = LB_UserCode.Text;
        string strUnderling = LB_Underling.Text;
        string strUserName = ShareClass.GetUserName(strUserCode);
        string strUnderLingName;

        if (LB_Underling.Text == "")
        {
            strHQL = "from Project as project where project.UserCode = project.PMCode and project.PMCode in ( select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " and " + "project.Status = " + "'" + strStatus + "'" + " order by project.ProjectID DESC";
            LB_QueryScope.Text = LanguageHandle.GetWord("ZZLeader") + strUserCode + strUserName + LanguageHandle.GetWord("ZZXMZT") + strStatus;

        }
        else
        {
            strUnderLingName = ShareClass.GetUserName(strUnderling);
            strHQL = "from Project as project where project.PMCode = " + "'" + LB_Underling.Text + "'";
            strHQL += " and " + "project.Status = " + "'" + strStatus + "'" + " order by project.ProjectID DESC";
            LB_QueryScope.Text = LanguageHandle.GetWord("ZZLeader") + strUserCode + strUserName + LanguageHandle.GetWord("ZZXiaShu") + strUnderling + strUnderLingName + LanguageHandle.GetWord("ZZXMZT") + strStatus;
        }

        ProjectBLL projectBLL = new ProjectBLL();

        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

    
        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);

        LB_Sql.Text = strHQL;
    }

    protected void BT_AllProject_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strUserName;
        string strHQL;
        IList lst;

        strUserName = ShareClass.GetUserName(strUserCode);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLeader") + strUserCode + strUserName;

        LB_Underling.Text = "";

        strHQL = "from Project as project where project.PMCode in ( select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";

        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql.Text = strHQL;

        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);
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
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);
    }

    protected void BT_DisplayStatus_Click(object sender, EventArgs e)
    {
        string strBTText = BT_DisplayStatus.Text;

        if (strBTText == LanguageHandle.GetWord("ShowStatus"))
        {
            DataGrid2.Visible = true;
            BT_DisplayStatus.Text = LanguageHandle.GetWord("YinQuXiangMuZhuangTai");
        }
        else
        {
            DataGrid2.Visible = false;
            BT_DisplayStatus.Text = LanguageHandle.GetWord("ShowStatus");
        }
    }

    protected void SetProRecordColor()
    {
        //int i;
        //DateTime dtNowDate, dtFinishedDate;
        //string strProjectID, strStatus;

        //for (i = 0; i < DataGrid3.Items.Count; i++)
        //{
        //    strProjectID = DataGrid3.Items[i].Cells[1].Text.Trim();

        //    Project Project = ShareClass.GetProject(strProjectID);

        //    dtFinishedDate = Project.EndDate;
        //    dtNowDate = DateTime.Now;
        //    strStatus = Project.Status.Trim();

        //    if (strStatus != "CaseClosed")
        //    {
        //        if (dtFinishedDate < dtNowDate)
        //        {
        //            DataGrid3.Items[i].ForeColor = Color.Red;
        //        }
        //    }
        //}
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
        IList lst;
        string strHQL, strFindCondition;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = "%" + TB_ProjectName.Text.Trim() + "%";

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectName like " + "'" + strFindCondition + "'" + " and project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMMBHZH") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;

    }

    protected void BT_ProjectIDFind_Click(object sender, EventArgs e)
    {
        string strUserCode;
        IList lst;
        string strHQL, strFindCondition;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = TB_ProjectID.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();

        if (strFindCondition == "")
        {
            strHQL = "from Project as project where project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";

            strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        }
        else
        {
            strHQL = "from Project as project where project.ProjectID = " + strFindCondition + " and project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and  memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";

            strHQL += " order by project.ProjectID DESC";
        }

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLiXiangRen") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;
    }

    protected void BT_MakeUserFind_Click(object sender, EventArgs e)
    {
        string strUserCode;
        IList lst;
        string strHQL, strFindCondition;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = "%" + TB_MakeUser.Text.Trim() + "%";

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.UserName like " + "'" + strFindCondition + "'" + " and project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";

        strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLiXiangRen") + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll");

        LB_Sql.Text = strHQL;
    }

    protected void BT_DateFind_Click(object sender, EventArgs e)
    {
        string strStartDate, strEndDate, strUserCode;
        string strHQL;
        IList lst;

        strStartDate = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyyMMdd");
        strEndDate = DateTime.Parse(DLC_EndDate.Text).ToString("yyyyMMdd");

        strUserCode = LB_UserCode.Text.Trim();

        strHQL = "from Project as project where to_char(project.BeginDate,'yyyymmdd') >= " + "'" + strStartDate + "'" + " and to_char(project.EndDate,'yyyymmdd') <= " + "'" + strEndDate + "'" + " and project.PMCode in ( select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";

        strHQL += " and project.Status not in ('Deleted','Archived') Order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);

        LB_QueryScope.Text = strStartDate + "----" + strEndDate;

        LB_Sql.Text = strHQL;

    }

    protected void Timer_Refresh_Tick(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = LB_Sql.Text;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        //ShareClass.FinishPercentPicture(DataGrid3, 1);
        //ShareClass.FinChargePercentByRow(DataGrid3, 1);
        SetProjectStartAndEndTime(DataGrid3, 0);
    }
}
