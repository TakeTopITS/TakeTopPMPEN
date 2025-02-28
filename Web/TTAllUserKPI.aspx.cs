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


using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAllUserKPI : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //Taketop Software 2006－2012

        string strHQL;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工绩效导出处理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB").ToString().Trim();

            strHQL = "Select * From V_UserKPIList";
            strHQL += " Where DepartCode in " + strDepartString;
            strHQL += " Order By StartTime DESC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_UserKPIList");
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();


            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName, strDepartString;
        int intCount;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);
            strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthorityAndDepartCode(strDepartCode, strUserCode);

            intCount = ShareClass.LoadUserKPIByDepartCodeForDataGrid(strDepartString, DataGrid1);

            LB_ProjectMemberOwner.Text = strDepartName + LanguageHandle.GetWord("DeChengYuan").ToString().Trim();
            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + intCount.ToString();

            ShareClass.InitialKPICheckTreeByDepartCode(TreeView2, strDepartCode, strDepartString);

            LB_DepartCode.Text = strDepartCode;
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strKPICheckID, strKPICheckName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strKPICheckID = treeNode.Target.Trim();
            strKPICheckName = treeNode.Text.Trim();

            //计算KPI的系统评分
            ShareClass.CalculateSystemPoint(strKPICheckID);

            LoadUserKPIList(strKPICheckID);
        }
    }

    protected void LoadUserKPIList(string strKPICheckID)
    {
        string strHQL;

        strHQL = "Select * From V_UserKPIList Where KPICheckID = " + strKPICheckID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_UserKPIList");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;


        LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB").ToString().Trim();

        string strDepartString = LB_DepartString.Text.Trim();


        string strUserCode = "%" + TB_UserCode.Text.Trim() + "%";
        string strUserName = "%" + TB_UserName.Text.Trim() + "%";

        strHQL = "Select * From V_UserKPIList Where UserCode Like " + "'" + strUserCode + "'" + " and UserName Like " + "'" + strUserName + "'";
        strHQL += " and DepartCode in " + strDepartString;
        strHQL += " Order By StartTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_UserKPIList");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
        LB_Sql.Text = strHQL;

        LB_DepartCode.Text = "";
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_UserKPIList");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("YuanGongKPIKaoHeBiao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                CreateExcel(fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }
        }
    }

    private void CreateExcel(string fileName)
    {
        string strHQL;
        string strDepartCode = LB_DepartCode.Text.Trim();
        string strDepartString;

        if (strDepartCode == "")//所有成员的情况
        {
            strDepartString = LB_DepartString.Text.Trim();

            strHQL = string.Format(@"Select UserCode 代码,UserName 姓名,Gender 性别,DepartCode 部门代码,DepartName 部门名称,
                Duty 职责,KPICheckID 编号,KPICheckName KPI考核名称,TotalSelfPoint 自评分,TotalLeaderPoint 领导评分,TotalThirdPartPoint 第三方评分,TotalSqlPoint 系统评分,TotalHRPoint 人事评分,TotalPoint 总分
                From V_UserKPIList Where DepartCode in ") + strDepartString;


            if (!string.IsNullOrEmpty(TB_UserCode.Text.Trim()))
            {
                strHQL += " and UserCode like '%" + TB_UserCode.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(TB_UserName.Text.Trim()))
            {
                strHQL += " and UserName like '%" + TB_UserName.Text.Trim() + "%' ";
            }
            strHQL += " Order By StartTime DESC";
        }
        else//按组织架构查询的
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthorityAndDepartCode(strDepartCode, strUserCode);  //ChineseWord //ChineseWord

            strHQL = string.Format(@"Select UserCode 代码,UserName 姓名,Gender 性别,DepartCode 部门代码,DepartName 部门名称,
                Duty 职责,KPICheckID 编号,KPICheckName KPI考核名称,TotalSelfPoint 自评分,TotalLeaderPoint 领导评分,TotalThirdPartPoint 第三方评分,TotalSqlPoint 系统评分,TotalHRPoint 人事评分,TotalPoint 总分
                From V_UserKPIList Where DepartCode in ") + strDepartString   + " Order By StartTime DESC";
        }

        MSExcelHandler.DataTableToExcel(strHQL, fileName);  //ChineseWord //ChineseWord
    }

    protected string GetUserStatus(string strUserCode)
    {
        string strHQL = "From SystemActiveUser as systemActiveUser where systemActiveUser.UserCode = '" + strUserCode.Trim() + "'";
        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        IList lst = systemActiveUserBLL.GetAllSystemActiveUsers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            return "Enabled";  //ChineseWord //ChineseWord
        }
        else
        {
            return "NotEnabled";  //ChineseWord //ChineseWord
        }
    }
}
