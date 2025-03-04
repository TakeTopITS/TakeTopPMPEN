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


public partial class TTProjectMemberTaskDetailReport : System.Web.UI.Page
{
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strLangCode = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();

        LB_ReportName.Text = LanguageHandle.GetWord("XiangMuChengYuanRenWuBiao").ToString().Trim();


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_BeginDate.Text = DateTime.Now.Year.ToString() + "-01-01";
            DLC_EndDate.Text = DateTime.Now.Year.ToString() + "-12-31";

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;
        
            ShareClass.InitialAllProjectTree(TreeView2, strDepartString);

            ShareClass.LoadTaskStatus(DL_Status, strLangCode);
            DL_Status.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_BelongDepartCode.Text = strDepartCode;
            TB_BelongDepartName.Text = strDepartName;
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID, strProjectName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strProjectID = treeNode.Target;

            strProjectName = ShareClass.GetProjectName(strProjectID);

            LB_ProjectID.Text = strProjectID;
            TB_ProjectName.Text = strProjectName;
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDepartCode, strOpretorName,strProjectID, strProjectName, strTaskName, strStatus;
        string strTaskBeginTime, strTaskEndTime;

        strProjectID = LB_ProjectID.Text.Trim();
        strDepartCode = "%" + LB_BelongDepartCode.Text.Trim() + "%";
        strOpretorName = "%" + TB_TaskOperator.Text.Trim() + "%";
        strProjectName = "%" + TB_ProjectName.Text.Trim() + "%";
        strTaskName = "%" + TB_TaskName.Text.Trim() + "%";
        strTaskBeginTime = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyyMMdd");
        strTaskEndTime = DateTime.Parse(DLC_EndDate.Text).ToString("yyyyMMdd");

        try
        {
            strStatus = "%" + DL_Status.SelectedValue + "%";
        }
        catch
        {
            strStatus = "%%";
        }

        strHQL = "Select * From V_ProjectMemberTaskDetailReport  ";
        strHQL += " Where DepartCode Like " + "'" + strDepartCode + "'";
        strHQL += " and UserName Like " + "'" + strOpretorName + "'";
        if (strProjectID != "")
        {
            strHQL += " and ProjectID = " + strProjectID;
        }
        else
        {
            strHQL += " and ProjectName Like " + "'" + strProjectName + "'";
        }
        strHQL += " and TaskName Like " + "'" + strTaskName + "'";
        strHQL += " and to_char(TaskBeginDate,'yyyymmdd') >= " + "'" + strTaskBeginTime + "'";
        strHQL += " and to_char(TaskEndDate,'yyyymmdd') <= " + "'" + strTaskEndTime + "'";
        strHQL += " and Status Like " + "'" + strStatus + "'";
        strHQL += " Order By ProjectID ASC, DepartCode ASC,UserName ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_ProjectMemberTaskDetailReport");

        DataList1.DataSource = ds;
        DataList1.DataBind();

        LB_ResultNumber.Text = ds.Tables[0].Rows.Count.ToString();
    }

    protected void BT_Export_Click(object sender, EventArgs e)
    {
        string strHQL;


        string strDepartCode, strOpretorName,strProjectID, strProjectName, strTaskName, strStatus;
        string strTaskBeginTime, strTaskEndTime;

        strProjectID = LB_ProjectID.Text.Trim();
        strDepartCode = "%" + LB_BelongDepartCode.Text.Trim() + "%";
        strOpretorName = "%" + TB_TaskOperator.Text.Trim() + "%";
        strProjectName = "%" + TB_ProjectName.Text.Trim() + "%";
        strTaskName = "%" + TB_TaskName.Text.Trim() + "%";
        strTaskBeginTime = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyyMMdd");
        strTaskEndTime = DateTime.Parse(DLC_EndDate.Text).ToString("yyyyMMdd");

        strStatus = "%" + DL_Status.SelectedValue + "%";

        strHQL = @"Select DepartCode as Group,   
                   UserName as Name,   
                   ProjectName as Project,
                   PlanName as Plan,
                   TaskName as Task,
                   PlanBeginTime as PlannedStartTime,   
                   PlanEndTime as PlannedEndTime,   
                   TaskBeginDate as TaskEstimatedStartTime,   
                   TaskFirstOperateTime as TaskAcceptanceTime,   
                   TaskEndDate as TaskEstimatedEndTime,   
                   TaskLastestOperateTime as TaskLatestOperationTime,   
                   Status as TaskStatus,   
                   TaskLog as TaskLog   
                   From V_ProjectMemberTaskDetailReport";


        strHQL += " Where DepartCode Like " + "'" + strDepartCode + "'";
        strHQL += " and UserName Like " + "'" + strOpretorName + "'";
        if (strProjectID != "")
        {
            strHQL += " and ProjectID = " + strProjectID;
        }
        else
        {
            strHQL += " and ProjectName Like " + "'" + strProjectName + "'";
        }
        strHQL += " and TaskName Like " + "'" + strTaskName + "'";
        strHQL += " and to_char(TaskBeginDate,'yyyymmdd') >= " + "'" + strTaskBeginTime + "'";
        strHQL += " and to_char(TaskEndDate,'yyyymmdd') <= " + "'" + strTaskEndTime + "'";
        strHQL += " and Status Like " + "'" + strStatus + "'";
        strHQL += " Order By ProjectID ASC, DepartCode ASC,UserName ASC";

        DataTable dtProject = ShareClass.GetDataSetFromSql(strHQL, "project").Tables[0];

        LB_ResultNumber.Text = dtProject.Rows.Count.ToString();


        Export3Excel(dtProject, LanguageHandle.GetWord("XiangMuChengYuanRenWuxls").ToString().Trim());

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("DaoChuChengGong")+"£¡');", true);   
    }

    public void Export3Excel(DataTable dtData, string strFileName)
    {
        DataGrid dgControl = new DataGrid();
        dgControl.DataSource = dtData;
        dgControl.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dgControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }

}
