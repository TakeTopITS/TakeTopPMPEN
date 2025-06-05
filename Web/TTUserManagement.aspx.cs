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

public partial class TTUserManagement : System.Web.UI.Page
{
    string strCurrentUserType;

    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //Taketop Software 2006－2012

        string strHQL;

        string strUserCode, strUserName, strDepartString;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strCurrentUserType = ShareClass.GetUserType(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "HumanResourcesManagement", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "人事管理---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("ChengYuanLieBiao");

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "Select * From T_ProjectMember Where DepartCode in " + strDepartString;
            strHQL += " Order By SortNumber ASC";
            DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            DataGrid1.DataSource = ds1;
            DataGrid1.DataBind();

            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD") + ds1.Tables[0].Rows.Count.ToString();
            LB_Sql.Text = strHQL;

            strHQL = "Select * From T_ProjectMember Where DepartCode in " + strDepartString;
            strHQL += " and Gender = 'Male'";
            DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                LB_MaleUserNumber.Text = ds2.Tables[0].Rows.Count.ToString();
            }

            strHQL = "Select * From T_ProjectMember Where DepartCode in " + strDepartString;
            strHQL += " and Gender = 'Female'";
            DataSet ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            if (ds3.Tables[0].Rows.Count > 0)
            {
                LB_FemaleUserNumber.Text = ds3.Tables[0].Rows.Count.ToString();
            }

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

        //ShareClass.CreateAnalystPieChart(strCmdText, Chart_MemberGender, SeriesChartType.Pie, strChartTitle, "XName", "YNumber", "Default");

        strChartTitle = LanguageHandle.GetWord("YGZTBLT");
        strCmdText = @"Select Status as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode in " + strDepartString;
        strCmdText += " Group By Status";
        IFrame_Chart_MemberStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        //ShareClass.CreateAnalystPieChart(strCmdText, Chart_MemberStatus, SeriesChartType.Pie, strChartTitle, "XName", "YNumber", "Default");

        strChartTitle = LanguageHandle.GetWord("YGZCQST");
        strCmdText = @"Select SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,7) as XName,COALESCE(Count(*),0) as YNumber
            From T_ProjectMember Where CAST(SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,5) as int) > extract(year from now()) - 4   
            And  DepartCode in " + strDepartString;
        strCmdText += " Group By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7) Order By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7) ASC";
        IFrame_Chart_MemberNumberTendency.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Line&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        //ShareClass.CreateAnalystLineChart(strCmdText, Chart_MemberNumberTendency, SeriesChartType.Line, strChartTitle, "JoinMonth", "MonthNumber", "", "Default");
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

            LB_ProjectMemberOwner.Text = strDepartName + LanguageHandle.GetWord("DeChengYuan");
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
            strCmdText = @"Select SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,7) as JoinMonth,COALESCE(Count(*),0) as MonthNumber
            From T_ProjectMember Where CAST(SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,5) as int) > extract(year from now()) - 4   
            And  DepartCode = " + "'" + strDepartCode + "'";
            strCmdText += " Group By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7) Order By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7) ASC";
            IFrame_Chart_MemberNumberTendency.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Line&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("ChengYuanLieBiao");


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
        strCmdText = @"Select SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,7) as JoinMonth,COALESCE(Count(*),0) as MonthNumber
            From T_ProjectMember Where CAST(SUBSTRING(to_char(JoinDate,'yyyymmdd'),0,5) as int) > extract(year from now()) - 4";
        strCmdText += " and UserCode Like " + "'" + strUserCode + "'";
        strCmdText += " and UserName Like " + "'" + strUserName + "'";
        strCmdText += " and Status Like " + "'" + strStatus + "'";
        strCmdText += " and DepartCode in " + strDepartString;
        strCmdText += " Group By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7) Order By SUBSTRING (to_char(JoinDate,'yyyymmdd'),0,7) ASC";
        IFrame_Chart_MemberNumberTendency.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Line&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");

        DataGrid1.DataSource = ds;
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
                string fileName = LanguageHandle.GetWord("YongHuChengYuanXinXi") + DateTime.Now.ToString("yyyyMMddHHmmss") + a.Next(100, 999) + ".xls";
                string strDepartCode = LB_DepartCode.Text.Trim();

                CreateExcel(strDepartCode, fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC") + "')", true);
            }
        }
    }

    private void CreateExcel(string strDepartCode, string fileName)
    {
        string strHQL;
        string strDepartString;

        if (strDepartCode == "")//所有成员的情况
        {
            strDepartString = LB_DepartString.Text.Trim();
            strHQL = string.Format(@"Select A.UserCode {0},A.UserName {1},A.Gender {2},A.Age {3},A.DepartCode {4},A.DepartName {5},   
                A.Duty {6},A.OfficePhone {7},A.MobilePhone {8},A.EMail {9},A.WorkScope {10},A.JoinDate {11},A.Status {12},   
                A.RefUserCode {13},A.IDCard {14},B.TopDepartName {15},B.EntryTotalYearMonth {16},B.OfficeAddress {17},   
                B.UserTypeExtend {18},B.UserState {19},B.ProbationPeriod {20},B.TurnOfficialDate {21},B.HouseRegisterType {22},   
                B.PoliticalOutlook {23},B.UrgencyRelation {24},B.ContractType {25},B.ContractCompany {26},B.FirstContractStartTime {27},   
                B.FirstContractEndTime {28},B.FirstContractYears {29},B.SecondContractStartTime {30},B.SecondContractEndTime {31},   
                B.SecondContractYears {32},B.ThirdContractStartTime {33},B.ThirdContractEndTime {34},B.ThirdContractYears {35},   
                B.SignContractCount {36},B.ContractStartTime {37},B.ContractYears {38},A.SortNumber {39}    
                From T_ProjectMember A Left Join T_ProjectMemberExtend B On A.UserCode = B.UserCode  Where 1=1", 
                LanguageHandle.GetWord("DaiMa"), 
                LanguageHandle.GetWord("XingMing"), 
                LanguageHandle.GetWord("XingBie"), 
                LanguageHandle.GetWord("NianLing"), 
                LanguageHandle.GetWord("BuMenDaiMa"), 
                LanguageHandle.GetWord("BuMenMingCheng"), 
                LanguageHandle.GetWord("ZhiZe"), 
                LanguageHandle.GetWord("BanGongDianHua"), 
                LanguageHandle.GetWord("YiDongDianHua"), 
                LanguageHandle.GetWord("EMail"), 
                LanguageHandle.GetWord("GongZuoFanWei"), 
                LanguageHandle.GetWord("JiaRuRiQi"), 
                LanguageHandle.GetWord("ZhuangTai"), 
                LanguageHandle.GetWord("CanKaoGongHao"), 
                LanguageHandle.GetWord("ShenFenZhengHao"), 
                LanguageHandle.GetWord("YiJiBuMen"), 
                LanguageHandle.GetWord("SiLing"), 
                LanguageHandle.GetWord("BanGongDiZhi"), 
                LanguageHandle.GetWord("YuanGongLeiXing"), 
                LanguageHandle.GetWord("YuanGongZhuangTai"), 
                LanguageHandle.GetWord("ShiYongQi"), 
                LanguageHandle.GetWord("ShiJiZhuanZhengRiQi"), 
                LanguageHandle.GetWord("HuJiLeiXing"), 
                LanguageHandle.GetWord("ZhengZhiMianMao"), 
                LanguageHandle.GetWord("LianXiRenGuanXi"), 
                LanguageHandle.GetWord("HeTongLeiXing"), 
                LanguageHandle.GetWord("HeTongGongSi"), 
                LanguageHandle.GetWord("ShouCiHeTongQiShiRi"), 
                LanguageHandle.GetWord("ShouCiHeTongDaoQiRi"), 
                LanguageHandle.GetWord("ShouCiHeTongQiXian"), 
                LanguageHandle.GetWord("DiErCiHeTongQiShiRi"), 
                LanguageHandle.GetWord("DiErCiHeTongDaoQiRi"), 
                LanguageHandle.GetWord("DiErCiHeTongQiXian"), 
                LanguageHandle.GetWord("DiSanCiHeTongQiShiRi"), 
                LanguageHandle.GetWord("DiSanCiHeTongDaoQiRi"), 
                LanguageHandle.GetWord("DiSanCiHeTongQiXian"), 
                LanguageHandle.GetWord("YiQianCiShu"), 
                LanguageHandle.GetWord("XianHeTongQiShiRi"), 
                LanguageHandle.GetWord("XianHeTongQiXian"), 
                LanguageHandle.GetWord("ShunXuHao"));

            if (!string.IsNullOrEmpty(strDepartString))
            {
                strHQL += " and A.DepartCode in " + strDepartString + " ";
            }
            if (!string.IsNullOrEmpty(DL_Status.SelectedValue.Trim()))
            {
                strHQL += " and A.Status like '%" + DL_Status.SelectedValue.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(TB_UserCode.Text.Trim()))
            {
                strHQL += " and A.UserCode like '%" + TB_UserCode.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(TB_UserName.Text.Trim()))
            {
                strHQL += " and A.UserName like '%" + TB_UserName.Text.Trim() + "%' ";
            }
            strHQL += " Order by A.SortNumber ASC";
        }
        else//按组织架构查询的
        {
            strHQL = string.Format(@"Select A.UserCode {0},A.UserName {1},A.Gender {2},A.Age {3},A.DepartCode {4},A.DepartName {5},   
        A.Duty {6},A.OfficePhone {7},A.MobilePhone {8},A.EMail {9},A.WorkScope {10},A.JoinDate {11},A.Status {12},   
        A.RefUserCode {13},A.IDCard {14},B.TopDepartName {15},B.EntryTotalYearMonth {16},B.OfficeAddress {17},   
        B.UserTypeExtend {18},B.UserState {19},B.ProbationPeriod {20},B.TurnOfficialDate {21},B.HouseRegisterType {22},   
        B.PoliticalOutlook {23},B.UrgencyRelation {24},B.ContractType {25},B.ContractCompany {26},B.FirstContractStartTime {27},   
        B.FirstContractEndTime {28},B.FirstContractYears {29},B.SecondContractStartTime {30},B.SecondContractEndTime {31},   
        B.SecondContractYears {32},B.ThirdContractStartTime {33},B.ThirdContractEndTime {34},B.ThirdContractYears {35},   
        B.SignContractCount {36},B.ContractStartTime {37},B.ContractYears {38},A.SortNumber {39}    
        From T_ProjectMember A Left Join T_ProjectMemberExtend B On A.UserCode = B.UserCode Where 1=1",
        LanguageHandle.GetWord("DaiMa"), 
        LanguageHandle.GetWord("XingMing"), 
        LanguageHandle.GetWord("XingBie"), 
        LanguageHandle.GetWord("NianLing"), 
        LanguageHandle.GetWord("BuMenDaiMa"), 
        LanguageHandle.GetWord("BuMenMingCheng"), 
        LanguageHandle.GetWord("ZhiZe"), 
        LanguageHandle.GetWord("BanGongDianHua"), 
        LanguageHandle.GetWord("YiDongDianHua"), 
        LanguageHandle.GetWord("EMail"), 
        LanguageHandle.GetWord("GongZuoFanWei"), 
        LanguageHandle.GetWord("JiaRuRiQi"), 
        LanguageHandle.GetWord("ZhuangTai"), 
        LanguageHandle.GetWord("CanKaoGongHao"), 
        LanguageHandle.GetWord("ShenFenZhengHao"), 
        LanguageHandle.GetWord("YiJiBuMen"), 
        LanguageHandle.GetWord("SiLing"), 
        LanguageHandle.GetWord("BanGongDiZhi"), 
        LanguageHandle.GetWord("YuanGongLeiXing"), 
        LanguageHandle.GetWord("YuanGongZhuangTai"), 
        LanguageHandle.GetWord("ShiYongQi"), 
        LanguageHandle.GetWord("ShiJiZhuanZhengRiQi"), 
        LanguageHandle.GetWord("HuJiLeiXing"), 
        LanguageHandle.GetWord("ZhengZhiMianMao"), 
        LanguageHandle.GetWord("LianXiRenGuanXi"), 
        LanguageHandle.GetWord("HeTongLeiXing"), 
        LanguageHandle.GetWord("HeTongGongSi"), 
        LanguageHandle.GetWord("ShouCiHeTongQiShiRi"), 
        LanguageHandle.GetWord("ShouCiHeTongDaoQiRi"), 
        LanguageHandle.GetWord("ShouCiHeTongQiXian"), 
        LanguageHandle.GetWord("DiErCiHeTongQiShiRi"), 
        LanguageHandle.GetWord("DiErCiHeTongDaoQiRi"), 
        LanguageHandle.GetWord("DiErCiHeTongQiXian"), 
        LanguageHandle.GetWord("DiSanCiHeTongQiShiRi"), 
        LanguageHandle.GetWord("DiSanCiHeTongDaoQiRi"), 
        LanguageHandle.GetWord("DiSanCiHeTongQiXian"), 
        LanguageHandle.GetWord("YiQianCiShu"), 
        LanguageHandle.GetWord("XianHeTongQiShiRi"), 
        LanguageHandle.GetWord("XianHeTongQiXian"), 
        LanguageHandle.GetWord("ShunXuHao"));

            if (!string.IsNullOrEmpty(strDepartCode))
            {
                strHQL += " and A.DepartCode = " + "'" + strDepartCode + "'";
            }
            strHQL += " Order by A.SortNumber ASC ";
        }
        MSExcelHandler.DataTableToExcel(strHQL, fileName);
    }

}
