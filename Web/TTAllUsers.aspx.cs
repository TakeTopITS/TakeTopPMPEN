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

public partial class TTAllUsers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //Taketop Software 2006－2012

        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "查看所有成员", strUserCode);
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
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB");

            strHQL = "from ProjectMember as projectMember ";
            strHQL += " Where projectMember.DepartCode in " + strDepartString;
            strHQL += " Order by projectMember.SortNumber ASC";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD") + lst.Count.ToString();
            LB_Sql.Text = strHQL;


            strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Male'";
            strHQL += " and projectMember.DepartCode in " + strDepartString;
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            LB_MaleUserNumber.Text = lst.Count.ToString();

            strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Female'";
            strHQL += " and projectMember.DepartCode in " + strDepartString;
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            LB_FemaleUserNumber.Text = lst.Count.ToString();

            //员工数据分析图
            CreateMemberAnalystChart(strDepartString);
        }
    }

    //员工数据分析图
    protected void CreateMemberAnalystChart(string strDepartString)
    {
        string strChartTitle, strCmdText;

        strChartTitle = LanguageHandle.GetWord("YGXBBLT");
        strCmdText = @"Select Gender as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode in " + strDepartString;
        strCmdText += " Group By Gender";
        IFrame_Chart_MemberGender.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("YGZTBLT");
        strCmdText = @"Select Status as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode in " + strDepartString;
        strCmdText += " Group By Status";
        IFrame_Chart_MemberStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("YGZCQST");
        strCmdText = @"Select SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,7)  as XName,COALESCE(Count(*),0) as YNumber
            From T_ProjectMember Where CAST(SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,5) as int) > extract(year from now()) - 4   
            And  DepartCode in " + strDepartString;
        strCmdText += " Group By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7)";
        IFrame_Chart_MemberNumberTendency.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Line&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartCode, strDepartName;
        int intCount;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            intCount = ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            LB_ProjectMemberOwner.Text = strDepartName + LanguageHandle.GetWord("Membes");
            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD") + intCount.ToString();

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Male'";
            strHQL += " and projectMember.DepartCode = " + "'" + strDepartCode + "'";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            LB_MaleUserNumber.Text = lst.Count.ToString();

            strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Female'";
            strHQL += " and projectMember.DepartCode = " + "'" + strDepartCode + "'";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            LB_FemaleUserNumber.Text = lst.Count.ToString();

            LB_DepartCode.Text = strDepartCode;

            string strChartTitle, strCmdText;

            strChartTitle = LanguageHandle.GetWord("YGXBBLT");
            strCmdText = @"Select Gender as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'";
            strCmdText += " Group By Gender";
            IFrame_Chart_MemberGender.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

            strChartTitle = LanguageHandle.GetWord("YGZTBLT");
            strCmdText = @"Select Status as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'";
            strCmdText += " Group By Status";
            IFrame_Chart_MemberStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

            strChartTitle = LanguageHandle.GetWord("YGZCQST");
            strCmdText = @"Select SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,7) as XName,COALESCE(Count(*),0) as YNumber
            From T_ProjectMember Where CAST(SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,5) as int) > extract(year from now()) - 4   
            And  DepartCode = " + "'" + strDepartCode + "'";
            strCmdText += " Group By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7)";
            IFrame_Chart_MemberNumberTendency.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Line&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB");

        string strDepartString = LB_DepartString.Text.Trim();

        string strStatus = "%" + DL_Status.SelectedValue + "%";
        string strUserCode = "%" + TB_UserCode.Text.Trim() + "%";
        string strUserName = "%" + TB_UserName.Text.Trim() + "%";

        strHQL = "from ProjectMember as projectMember where ";
        strHQL += " projectMember.UserCode Like " + "'" + strUserCode + "'";
        strHQL += " and projectMember.UserName Like " + "'" + strUserName + "'";
        strHQL += " and projectMember.Status Like " + "'" + strStatus + "'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        strHQL += " Order by projectMember.SortNumber ASC";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_UserNumber.Text = LanguageHandle.GetWord("GCXD") + lst.Count.ToString();
        LB_Sql.Text = strHQL;


        strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Male'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        LB_MaleUserNumber.Text = lst.Count.ToString();

        strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Female'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        LB_FemaleUserNumber.Text = lst.Count.ToString();

        LB_DepartCode.Text = "";

        string strChartTitle, strCmdText;

        strChartTitle = LanguageHandle.GetWord("YGXBBLT");
        strCmdText = @"Select Gender as XName, Count(*) as YNumber
            From T_ProjectMember Where ";
        strCmdText += " UserCode Like " + "'" + strUserCode + "'";
        strCmdText += " and UserName Like " + "'" + strUserName + "'";
        strCmdText += " and Status Like " + "'" + strStatus + "'";
        strCmdText += " and DepartCode in " + strDepartString;
        strCmdText += " Group By Gender";
        IFrame_Chart_MemberGender.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("YGZTBLT");
        strCmdText = @"Select Status as XName, Count(*) as YNumber
            From T_ProjectMember Where ";
        strCmdText += " UserCode Like " + "'" + strUserCode + "'";
        strCmdText += " and UserName Like " + "'" + strUserName + "'";
        strCmdText += " and Status Like " + "'" + strStatus + "'";
        strCmdText += " and DepartCode in " + strDepartString;
        strCmdText += " Group By Status";
        IFrame_Chart_MemberStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("YGZCQST");
        strCmdText = @"Select SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,7) as XName,COALESCE(Count(*),0) as YNumber
            From T_ProjectMember Where CAST(SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,5) as int) > extract(year from now()) - 4";
        strCmdText += " and UserCode Like " + "'" + strUserCode + "'";
        strCmdText += " and UserName Like " + "'" + strUserName + "'";
        strCmdText += " and Status Like " + "'" + strStatus + "'";
        strCmdText += " and DepartCode in " + strDepartString;
        strCmdText += " Group By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7)";
        IFrame_Chart_MemberNumberTendency.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Line&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        //员工数据分析图
        CreateMemberAnalystChart(LB_DepartString.Text.Trim());
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("YongHuChengYuanXinXi") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                CreateExcel(fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC") + "')", true);
            }
        }
    }

    private void CreateExcel(string fileName)
    {
        string strHQL;
        string strDepartCode = LB_DepartCode.Text.Trim();

        if (strDepartCode == "")//所有成员的情况
        {
            string strDepartString = LB_DepartString.Text.Trim();
            strHQL = string.Format(@"Select UserCode {0},UserName {1},Gender {2},Age {3},DepartCode {4},DepartName {5},ChildDepartment {6},
                Duty {7},OfficePhone {8},MobilePhone {9},EMail {10},WorkScope {11},JoinDate {12},Status {13},
                RefUserCode {14},IDCard {15},SortNumber {16},(case when UserCode in (select UserCode from T_SystemActiveUser) then 'Enabled'
								 else 'NotEnabled' end) {17} 
                From T_ProjectMember Where DepartCode in {18}",
                            LanguageHandle.GetWord("DaiMa"),
                            LanguageHandle.GetWord("XingMing"),
                            LanguageHandle.GetWord("XingBie"),
                            LanguageHandle.GetWord("NianLing"),
                            LanguageHandle.GetWord("BuMenDaiMa"),
                            LanguageHandle.GetWord("BuMen"),
                            LanguageHandle.GetWord("ZiBuMen"),
                            LanguageHandle.GetWord("ZhiZe"),
                            LanguageHandle.GetWord("BanGongDianHua"),
                            LanguageHandle.GetWord("YiDongDianHua"),
                            LanguageHandle.GetWord("EMail"),
                            LanguageHandle.GetWord("GongZuoFanWei"),
                            LanguageHandle.GetWord("JiaRuRiQi"),
                            LanguageHandle.GetWord("ZhuangTai"),
                            LanguageHandle.GetWord("CanKaoGongHao"),
                            LanguageHandle.GetWord("ShenFenZhengHao"),
                            LanguageHandle.GetWord("ShunXuHao"),
                            LanguageHandle.GetWord("QuanXian"),
                            strDepartString);

            if (!string.IsNullOrEmpty(DL_Status.SelectedValue.Trim()))
            {
                strHQL += " and Status like '%" + DL_Status.SelectedValue.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(TB_UserCode.Text.Trim()))
            {
                strHQL += " and UserCode like '%" + TB_UserCode.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(TB_UserName.Text.Trim()))
            {
                strHQL += " and UserName like '%" + TB_UserName.Text.Trim() + "%' ";
            }
            strHQL += " Order by SortNumber ASC";
        }
        else//按组织架构查询的
        {
            strHQL = string.Format(@"Select UserCode {0},UserName {1}, Gender {2},Age {3}, DepartCode {4},DepartName {5},ChildDepartment {6},
          Duty {7},OfficePhone {8}, MobilePhone {9},EMail {10}, WorkScope {11},JoinDate {12}, Status {13},
        RefUserCode {14}, IDCard {15},SortNumber {16},(case when UserCode in (select UserCode from T_SystemActiveUser) then 'Enabled'
                         else 'NotEnabled' end) {17}
        From T_ProjectMember Where DepartCode = '{18}') Order by SortNumber ASC ",
            LanguageHandle.GetWord("DaiMa"),
            LanguageHandle.GetWord("XingMing"),
            LanguageHandle.GetWord("XingBie"),
            LanguageHandle.GetWord("NianLing"),
            LanguageHandle.GetWord("BuMenDaiMa"),
            LanguageHandle.GetWord("BuMen"),
            LanguageHandle.GetWord("ZiBuMen"),
            LanguageHandle.GetWord("ZhiZe"),
            LanguageHandle.GetWord("BanGongDianHua"),
            LanguageHandle.GetWord("YiDongDianHua"),
            LanguageHandle.GetWord("EMail"),
            LanguageHandle.GetWord("GongZuoFanWei"),
            LanguageHandle.GetWord("JiaRuRiQi"),
            LanguageHandle.GetWord("ZhuangTai"),
            LanguageHandle.GetWord("CanKaoGongHao"),
            LanguageHandle.GetWord("ShenFenZhengHao"),
            LanguageHandle.GetWord("ShunXuHao"),
            LanguageHandle.GetWord("QuanXian"),
            strDepartCode);
        }

        MSExcelHandler.DataTableToExcel(strHQL, fileName);
    }
    

    protected string GetUserStatus(string strUserCode)
    {
        string strHQL = "From SystemActiveUser as systemActiveUser where systemActiveUser.UserCode = '" + strUserCode.Trim() + "'";
        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        IList lst = systemActiveUserBLL.GetAllSystemActiveUsers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            return "Enabled";   
        }
        else
        {
            return "NotEnabled";   
        }
    }
}
