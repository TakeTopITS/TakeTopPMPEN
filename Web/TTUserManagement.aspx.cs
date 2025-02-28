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
            LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("ChengYuanLieBiao").ToString().Trim();

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "Select * From T_ProjectMember Where DepartCode in " + strDepartString;
            strHQL += " Order By SortNumber ASC";
            DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            DataGrid1.DataSource = ds1;
            DataGrid1.DataBind();

            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + ds1.Tables[0].Rows.Count.ToString();
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

        strChartTitle = LanguageHandle.GetWord("YGXBBLT").ToString().Trim();
        strCmdText = @"Select Gender as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode in " + strDepartString;
        strCmdText += " Group By Gender";
        IFrame_Chart_MemberGender.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        //ShareClass.CreateAnalystPieChart(strCmdText, Chart_MemberGender, SeriesChartType.Pie, strChartTitle, "XName", "YNumber", "Default");

        strChartTitle = LanguageHandle.GetWord("YGZTBLT").ToString().Trim();
        strCmdText = @"Select Status as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode in " + strDepartString;
        strCmdText += " Group By Status";
        IFrame_Chart_MemberStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        //ShareClass.CreateAnalystPieChart(strCmdText, Chart_MemberStatus, SeriesChartType.Pie, strChartTitle, "XName", "YNumber", "Default");

        strChartTitle = LanguageHandle.GetWord("YGZCQST").ToString().Trim();
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

            LB_ProjectMemberOwner.Text = strDepartName + LanguageHandle.GetWord("DeChengYuan").ToString().Trim();
            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + intCount.ToString();

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

            strChartTitle = LanguageHandle.GetWord("YGXBBLT").ToString().Trim();
            strCmdText = @"Select Gender as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'";
            strCmdText += " Group By Gender";
            IFrame_Chart_MemberGender.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

            strChartTitle = LanguageHandle.GetWord("YGZTBLT").ToString().Trim();
            strCmdText = @"Select Status as XName, Count(*) as YNumber
            From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'";
            strCmdText += " Group By Status";
            IFrame_Chart_MemberStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

            strChartTitle = LanguageHandle.GetWord("YGZCQST").ToString().Trim();
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

        LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("ChengYuanLieBiao").ToString().Trim();


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

        LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + lst.Count.ToString();
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

        strChartTitle = LanguageHandle.GetWord("YGXBBLT").ToString().Trim();
        strCmdText = @"Select Gender as XName, Count(*) as YNumber
            From T_ProjectMember Where ";
        strCmdText += " UserCode Like " + "'" + strUserCode + "'";
        strCmdText += " and UserName Like " + "'" + strUserName + "'";
        strCmdText += " and Status Like " + "'" + strStatus + "'";
        strCmdText += " and DepartCode in " + strDepartString;
        strCmdText += " Group By Gender";
        IFrame_Chart_MemberGender.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("YGZTBLT").ToString().Trim();
        strCmdText = @"Select Status as XName, Count(*) as YNumber
            From T_ProjectMember Where ";
        strCmdText += " UserCode Like " + "'" + strUserCode + "'";
        strCmdText += " and UserName Like " + "'" + strUserName + "'";
        strCmdText += " and Status Like " + "'" + strStatus + "'";
        strCmdText += " and DepartCode in " + strDepartString;
        strCmdText += " Group By Status";
        IFrame_Chart_MemberStatus.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strCmdText);

        strChartTitle = LanguageHandle.GetWord("YGZCQST").ToString().Trim();
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
                string fileName = LanguageHandle.GetWord("YongHuChengYuanXinXi").ToString().Trim() + DateTime.Now.ToString("yyyyMMddHHmmss") + a.Next(100, 999) + ".xls";
                string strDepartCode = LB_DepartCode.Text.Trim();

                CreateExcel(strDepartCode, fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
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

            strHQL = @"Select A.UserCode 代码,A.UserName 姓名,A.Gender 性别,A.Age 年龄,A.DepartCode 部门代码,A.DepartName 部门名称,   
                A.Duty 职责,A.OfficePhone 办公电话,A.MobilePhone 移动电话,A.EMail EMail,A.WorkScope 工作范围,A.JoinDate 加入日期,A.Status 状态,   
                A.RefUserCode 参考工号,A.IDCard 身份证号,B.TopDepartName 一级部门,B.EntryTotalYearMonth 司龄,B.OfficeAddress 办公地址,   
                B.UserTypeExtend 员工类型,B.UserState 员工状态,B.ProbationPeriod 试用期,B.TurnOfficialDate 实际转正日期,B.HouseRegisterType 户籍类型,   
                B.PoliticalOutlook 政治面貌,B.UrgencyRelation 联系人关系,B.ContractType 合同类型,B.ContractCompany 合同公司,B.FirstContractStartTime 首次合同起始日,   
                B.FirstContractEndTime 首次合同到期日,B.FirstContractYears 首次合同期限,B.SecondContractStartTime 第二次合同起始日,B.SecondContractEndTime 第二次合同到期日,   
                B.SecondContractYears 	第二次合同期限,B.ThirdContractStartTime 第三次合同起始日,B.ThirdContractEndTime 第三次合同到期日,B.ThirdContractYears 第三次合同期限,   
                B.SignContractCount 已签次数,B.ContractStartTime 现合同起始日,B.ContractYears 现合同期限,A.SortNumber 顺序号    
                From T_ProjectMember A Left Join T_ProjectMemberExtend B On A.UserCode = B.UserCode  Where 1=1";
          
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
            strHQL = @"Select A.UserCode 代码,A.UserName 姓名,A.Gender 性别,A.Age 年龄,A.DepartCode 部门代码,A.DepartName 部门名称,   
                A.Duty 职责,A.OfficePhone 办公电话,A.MobilePhone 移动电话,A.EMail EMail,A.WorkScope 工作范围,A.JoinDate 加入日期,A.Status 状态,   
                A.RefUserCode 参考工号,A.IDCard 身份证号,B.TopDepartName 一级部门,B.EntryTotalYearMonth 司龄,B.OfficeAddress 办公地址,   
                B.UserTypeExtend 员工类型,B.UserState 员工状态,B.ProbationPeriod 试用期,B.TurnOfficialDate 实际转正日期,B.HouseRegisterType 户籍类型,   
                B.PoliticalOutlook 政治面貌,B.UrgencyRelation 联系人关系,B.ContractType 合同类型,B.ContractCompany 合同公司,B.FirstContractStartTime 首次合同起始日,   
                B.FirstContractEndTime 首次合同到期日,B.FirstContractYears 首次合同期限,B.SecondContractStartTime 第二次合同起始日,B.SecondContractEndTime 第二次合同到期日,   
                B.SecondContractYears 	第二次合同期限,B.ThirdContractStartTime 第三次合同起始日,B.ThirdContractEndTime 第三次合同到期日,B.ThirdContractYears 第三次合同期限,   
                B.SignContractCount 已签次数,B.ContractStartTime 现合同起始日,B.ContractYears 现合同期限,A.SortNumber 顺序号    
                From T_ProjectMember A Left Join T_ProjectMemberExtend B On A.UserCode = B.UserCode Where 1=1";
         
            if (!string.IsNullOrEmpty(strDepartCode))
            {
                strHQL += " and A.DepartCode = " + "'" + strDepartCode + "'";
            }
            strHQL += " Order by A.SortNumber ASC ";
        }
        MSExcelHandler.DataTableToExcel(strHQL, fileName);
    }

}
