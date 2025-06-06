using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTProjectManHourAndExpenseReport : System.Web.UI.Page
{
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strLangCode = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();

        LB_ReportName.Text = LanguageHandle.GetWord("XiangMuGongShiHeFeiYongHuiZong");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            //LoadProjectManHourAndExpenseReportForJHKC();
            ShareClass.InitialAllProjectTree(TreeView2, strDepartString);

            ShareClass.LoadProjectStatusForDropDownList(strLangCode, DL_Status);
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

        string strDepartCode, strPMName, strProjectID, strProjectName, strStatus;
        string strBeginTime, strEndTime;

        strProjectID = LB_ProjectID.Text.Trim();
        strDepartCode = "%" + LB_BelongDepartCode.Text.Trim() + "%";
        strPMName = "%" + TB_PMName.Text.Trim() + "%";
        strProjectName = "%" + TB_ProjectName.Text.Trim() + "%";

        strBeginTime = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyy-MM-dd");
        strEndTime = DateTime.Parse(DLC_EndDate.Text).ToString("yyyy-MM-dd");

        strStatus = "%" + DL_Status.SelectedValue + "%";

        strHQL = "Exec Pro_GetProjectManHourAndExpenseReportForJHKC2";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "Pro_ProjectMemberTaskDetailReport");

        DataView dv = ds.Tables[0].DefaultView;

        if (strProjectID != "")
        {
            dv.RowFilter = "ProjectID = " + strProjectID;
        }
        else
        {
            dv.RowFilter = LanguageHandle.GetWord("XiangMuMingChenLike") + strProjectName + "%'";
        }
        dv.RowFilter += LanguageHandle.GetWord("AndXiangMuFuZeRenLike") + strPMName + "%'";
        dv.RowFilter += LanguageHandle.GetWord("AndJinChangRiJi") + strBeginTime + LanguageHandle.GetWord("AndChuChangRiJi") + strEndTime + "'";
        dv.Sort += " ProjectID DESC";

        GridView1.DataSource = dv;
        GridView1.DataBind();

        LB_ResultNumber.Text = GridView1.Rows.Count.ToString();
    }


    protected void BT_Export_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDepartCode, strPMName,strProjectID, strProjectName, strStatus;
        string strBeginTime, strEndTime;

        strProjectID = LB_ProjectID.Text.Trim();
        strDepartCode = "%" + LB_BelongDepartCode.Text.Trim() + "%";
        strPMName = "%" + TB_PMName.Text.Trim() + "%";
        strProjectName = "%" + TB_ProjectName.Text.Trim() + "%";

        strBeginTime = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyy-MM-dd");
        strEndTime = DateTime.Parse(DLC_EndDate.Text).ToString("yyyy-MM-dd");

        strStatus = "%" + DL_Status.SelectedValue + "%";

        strHQL = "Exec Pro_GetProjectManHourAndExpenseReportForJHKC2";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "Pro_ProjectMemberTaskDetailReport");

        DataView dv = ds.Tables[0].DefaultView;

        if (strProjectID != "")
        {
            dv.RowFilter = "ProjectID = " + strProjectID;
        }
        else
        {
            dv.RowFilter = LanguageHandle.GetWord("XiangMuMingChenLike") + strProjectName + "%'";
        }
        dv.RowFilter += LanguageHandle.GetWord("AndXiangMuFuZeRenLike") + strPMName + "%'"; dv.RowFilter += LanguageHandle.GetWord("AndJinChangRiJi") + strBeginTime + LanguageHandle.GetWord("AndChuChangRiJi") + strEndTime + "'";
        dv.Sort += " ProjectID DESC";

        GridView1.DataSource = dv;
        GridView1.DataBind();

        DataTable dtProject = dv.ToTable();

        Export3Excel(dtProject, LanguageHandle.GetWord("XiangMuGongShiHeFeiYongHuiZong"));

        LB_ResultNumber.Text = GridView1.Rows.Count.ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("DaoChuChengGong")+"��');", true);   
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


    protected void LoadProjectManHourAndExpenseReportForJHKC()
    {
        string strHQL;

        strHQL = "Exec Pro_GetProjectManHourAndExpenseReportForJHKC2";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "Pro_GetProjectManHourAndExpenseReportForJHKC");

        GridView1.DataSource = ds;
        GridView1.DataBind();
    }

}
