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
using System.Security.Permissions;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;



using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAllProjectsRunStatus : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ��jack.erp@gmail.com)
        //̩���ض�����2006��2026

        string strHQL;
        IList lst;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "���г�Ա��Ŀ״̬", strUserCode);
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

            int intIntervalTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerInterval"]);
            Timer_Refresh.Interval = intIntervalTime;

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "from Project as project";
            strHQL += " Where project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " and project.Status not in ('Deleted','Cancel')";
            strHQL += " order by project.ProjectID DESC ";
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            SetProRecordColor();
            ShareClass.FinishPercentPicture(DataGrid3, 1);
            ShareClass.FinChargePercentByRow(DataGrid3,1);
            ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

            ShareClass.SetRiskLabelColor(DataGrid3, 1);
            ShareClass.SetDefectLabelColor(DataGrid3, 1);

            LB_Sql.Text = strHQL;

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim();

            ShareClass.LoadProjectStatusForDataGrid(strLangCode, DataGrid2);
            LoadProjectType(DL_ProjectType);
            DL_ProjectType.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }


    protected void DL_ProjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project";
        strHQL += " Where project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + LB_DepartString.Text + ")";
        if (DL_ProjectType.SelectedValue.Trim() != "")
        {
            strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " Order by project.ProjectID DESC";

        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3, 1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

        BT_StatusVisible.Visible = true;
        BT_StatusVisible.Text = LanguageHandle.GetWord("ShowStatus").ToString().Trim();

        DataGrid2.Visible = false;

        LB_Sql.Text = strHQL;
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartCode, strDepartName, strUserCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);
            strUserCode = LB_UserCode.Text.Trim();

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen").ToString().Trim() + strDepartName;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            strHQL = "from Project as project where (project.PMCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") Or project.BelongDepartCode = '" + strDepartCode + "')";
            if (DL_ProjectType.SelectedValue.Trim() != "")
            {
                strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
            }
            strHQL += " and project.Status not in ('Deleted','Archived')";
            strHQL += " Order by project.ProjectID DESC";
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            SetProRecordColor();
            ShareClass.FinishPercentPicture(DataGrid3, 1);
            ShareClass.FinChargePercentByRow(DataGrid3,1);
            ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

            ShareClass.SetRiskLabelColor(DataGrid3, 1);
            ShareClass.SetDefectLabelColor(DataGrid3, 1);

            BT_StatusVisible.Visible = true;
            BT_StatusVisible.Text = LanguageHandle.GetWord("ShowStatus").ToString().Trim();

            DataGrid2.Visible = false;

            LB_Sql.Text = strHQL;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strOperatorName = ShareClass.GetUserName(strOperatorCode);
        string strUserCode = LB_UserCode.Text.Trim();

        string strHQL = "from Project as project where project.PMCode = " + "'" + strOperatorCode + "'";
        if (DL_ProjectType.SelectedValue.Trim() != "")
        {
            strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " order by project.ProjectID DESC";

        LB_OperatorCode.Text = strOperatorCode;
        LB_OperatorName.Text = strOperatorName;

        LB_DepartCode.Text = "";

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim() + strOperatorCode + strOperatorName;

        ProjectBLL projectBLL = new ProjectBLL();

        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStatus = ((Button)e.Item.FindControl("BT_Status")).Text;
        string strHQL;
        string strOperatorCode = LB_OperatorCode.Text;
        string strOperatorName;
        string strDepartCode, strDepartName;

        string strDepartString = LB_DepartString.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();


        if (LB_OperatorCode.Text == "")
        {
            strHQL = "from Project as project where project.Status = " + "'" + strStatus + "'";
            strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            if (DL_ProjectType.SelectedValue.Trim() != "")
            {
                strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
            }
            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim() + LanguageHandle.GetWord("ZZXMZT").ToString().Trim() + strStatus;

        }
        else
        {
            if (LB_DepartCode.Text == "")
            {
                strOperatorName = ShareClass.GetUserName(strOperatorCode);
                strHQL = "from Project as project where project.PMCode = " + "'" + strOperatorCode + "'" + " and " + "project.Status = " + "'" + strStatus + "'";
                strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";

                if (DL_ProjectType.SelectedValue.Trim() != "")
                {
                    strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
                }
                LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim() + strOperatorCode + strOperatorName + LanguageHandle.GetWord("ZZXMZT").ToString().Trim() + strStatus;
            }
            else
            {
                strDepartCode = LB_DepartCode.Text.Trim();
                strDepartName = LB_DepartName.Text.Trim();
                strHQL = "from Project as project where (project.PMCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") Or project.BelongDepartCode = '" + strDepartCode + "') and " + "project.Status = " + "'" + strStatus + "'";
                strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
                if (DL_ProjectType.SelectedValue.Trim() != "")
                {
                    strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
                }
                LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen").ToString().Trim() + strDepartName + LanguageHandle.GetWord("ZZXMZT").ToString().Trim() + strStatus;
            }
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " order by project.ProjectID DESC ";

        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

        LB_Sql.Text = strHQL;
    }

    protected void BT_AllProject_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strUserCode;

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZZhiXingZheAll").ToString().Trim();

        DataGrid1.Visible = false;

        LB_DepartCode.Text = "";
        LB_OperatorCode.Text = "";

        strUserCode = LB_UserCode.Text.Trim();

        string strDepartString = LB_DepartString.Text.Trim();
        strHQL = "from Project as project";
        strHQL += " Where project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        if (DL_ProjectType.SelectedValue.Trim() != "")
        {
            strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " order by project.ProjectID DESC ";
        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

        LB_Sql.Text = strHQL;
    }

    protected void BT_StatusVisible_Click(object sender, EventArgs e)
    {
        string strBTText = BT_StatusVisible.Text.Trim();

        if (strBTText == LanguageHandle.GetWord("ShowStatus").ToString().Trim())
        {
            DataGrid2.Visible = true;
            BT_StatusVisible.Text = LanguageHandle.GetWord("HideStatus").ToString().Trim();
        }
        else
        {
            DataGrid2.Visible = false;
            BT_StatusVisible.Text = LanguageHandle.GetWord("ShowStatus").ToString().Trim();
        }
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
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);
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

        string strDepartString = LB_DepartString.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectName like " + "'" + strFindCondition + "'";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        if (DL_ProjectType.SelectedValue.Trim() != "")
        {
            strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " order by project.ProjectID DESC ";
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZXMMBHZH").ToString().Trim() + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll").ToString().Trim();

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

        string strDepartString = LB_DepartString.Text.Trim();

        if (strFindCondition == "")
        {
            strHQL = "from Project as project ";
            strHQL += " where project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        }
        else
        {
            strHQL = "from Project as project where project.ProjectID = " + strFindCondition;
            strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        }
        if (DL_ProjectType.SelectedValue.Trim() != "")
        {
            strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " order by project.ProjectID DESC ";

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLiXiangRen").ToString().Trim() + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll").ToString().Trim();

        LB_Sql.Text = strHQL;
    }

    protected void BT_MakeUserFind_Click(object sender, EventArgs e)
    {
        string strUserCode;
        IList lst;
        string strHQL, strFindCondition;

        strUserCode = LB_UserCode.Text.Trim();
        strFindCondition = "%" + TB_MakeUser.Text.Trim() + "%";

        string strDepartString = LB_DepartString.Text.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.UserName like " + "'" + strFindCondition + "'";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        if (DL_ProjectType.SelectedValue.Trim() != "")
        {
            strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " order by project.ProjectID DESC";
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZLiXiangRen").ToString().Trim() + TB_ProjectName.Text.Trim() + LanguageHandle.GetWord("StatusAll").ToString().Trim();

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

        string strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from Project as project where to_char(project.BeginDate,'yyyymmdd') >= " + "'" + strStartDate + "'" + " and to_char(project.EndDate,'yyyymmdd') <= " + "'" + strEndDate + "'";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        if (DL_ProjectType.SelectedValue.Trim() != "")
        {
            strHQL += " and project.ProjectType = '" + DL_ProjectType.SelectedValue.Trim() + "'";
        }
        strHQL += " and project.Status not in ('Deleted','Archived')";
        strHQL += " order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        SetProRecordColor();
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);

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
        ShareClass.FinishPercentPicture(DataGrid3, 1);
        ShareClass.FinChargePercentByRow(DataGrid3,1);
        ShareClass.SetProjectStartAndEndTime(DataGrid3, 1);

        ShareClass.SetRiskLabelColor(DataGrid3, 1);
        ShareClass.SetDefectLabelColor(DataGrid3, 1);
    }

    public static void LoadProjectType(DropDownList DL_ProjectType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectType as projectType Order by projectType.SortNumber ASC";
        ProjectTypeBLL projectTypeBLL = new ProjectTypeBLL();
        lst = projectTypeBLL.GetAllProjectTypes(strHQL);
        DL_ProjectType.DataSource = lst;
        DL_ProjectType.DataBind();

        DL_ProjectType.Items.Insert(0, new ListItem("--Select--", ""));
    }
}
