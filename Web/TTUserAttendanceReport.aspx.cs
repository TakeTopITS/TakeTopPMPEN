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

using System.Text;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTUserAttendanceReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName;
        string strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTUserAttendanceReportSAAS.aspx");
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = ShareClass.getCurrentMonthStartDay().ToString("yyyy-MM-dd");
            DLC_EndTime.Text = ShareClass.getCurrentMonthEndDay().ToString("yyyy-MM-dd");

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
        }
    }

   

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            TB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strDepartCode, strUserCode, strUserName, strDisplayType, strStartTime, strEndTime;
        string strDepartString;

        strDepartCode = TB_DepartCode.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strUserName = TB_UserName.Text.Trim();

        if(strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoQingXianShuaZeBuMenZa").ToString().Trim()+"')", true);
            return;
        }

        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthorityAndDepartCode(strDepartCode, strUserCode);

        strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");

        strDisplayType = DL_DisplayType.SelectedValue.Trim();

        strDepartCode = "%" + strDepartCode + "%";
        strUserName = "%" + strUserName + "%";

        if (CB_IncludeChildDepartment.Checked == true)
        {
            if (strDisplayType == "DisplayAbnormal")
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and (LateMinute <> 0 or EarlyMinute <> 0 or (MCheckInDistance > LargestDistance Or MCheckOutDistance > LargestDistance Or ACheckInDistance > LargestDistance Or ACheckOutDistance > LargestDistance Or NCheckInDistance > LargestDistance Or NCheckOutDistance > LargestDistance Or OCheckInDistance > LargestDistance Or OCheckOutDistance > LargestDistance))";
                strHQL += " and UserName Like " + "'" + strUserName + "'";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
                strHQL += " and UserCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
            }
            else
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and  UserName Like " + "'" + strUserName + "'";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
                strHQL += " and  UserCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
            }
        }
        else
        {
            if (strDisplayType == "DisplayAbnormal")
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and (LateMinute <> 0 or EarlyMinute <> 0 or (MCheckInDistance > LargestDistance Or MCheckOutDistance > LargestDistance Or ACheckInDistance > LargestDistance Or ACheckOutDistance > LargestDistance Or NCheckInDistance > LargestDistance Or NCheckOutDistance > LargestDistance Or OCheckInDistance > LargestDistance Or OCheckOutDistance > LargestDistance))";
                strHQL += " and UserName Like " + "'" + strUserName + "'";
                strHQL += " and  UserCode in (Select UserCode From T_ProjectMember Where DepartCode Like " + "'" + strDepartCode + "'" + ")";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
            }
            else
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and  UserName Like " + "'" + strUserName + "'";
                strHQL += " and  UserCode in (Select UserCode From T_ProjectMember Where DepartCode Like " + "'" + strDepartCode + "'" + ")";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
            }
        }
        strHQL += " Order by to_char(AttendanceDate,'yyyymmdd') DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserAttendanceRecord");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strDepartCode, strUserCode, strUserName, strDisplayType, strStartTime, strEndTime;
        string strDepartString;

        strDepartCode = TB_DepartCode.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strUserName = TB_UserName.Text.Trim();

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoQingXianShuaZeBuMenZa").ToString().Trim()+"')", true);
            return;
        }

        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthorityAndDepartCode(strDepartCode, strUserCode);

        strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");

        strDisplayType = DL_DisplayType.SelectedValue.Trim();

        strDepartCode = "%" + strDepartCode + "%";
        strUserName = "%" + strUserName + "%";

        if (CB_IncludeChildDepartment.Checked == true)
        {
            if (strDisplayType == "DisplayAbnormal")
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and (LateMinute <> 0 or EarlyMinute <> 0 or (MCheckInDistance > LargestDistance Or MCheckOutDistance > LargestDistance Or ACheckInDistance > LargestDistance Or ACheckOutDistance > LargestDistance Or NCheckInDistance > LargestDistance Or NCheckOutDistance > LargestDistance Or OCheckInDistance > LargestDistance Or OCheckOutDistance > LargestDistance))";
                strHQL += " and UserName Like " + "'" + strUserName + "'";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
                strHQL += " and UserCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
            }
            else
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and  UserName Like " + "'" + strUserName + "'";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
                strHQL += " and  UserCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
            }
        }
        else
        {
            if (strDisplayType == "DisplayAbnormal")
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and (LateMinute <> 0 or EarlyMinute <> 0 or (MCheckInDistance > LargestDistance Or MCheckOutDistance > LargestDistance Or ACheckInDistance > LargestDistance Or ACheckOutDistance > LargestDistance Or NCheckInDistance > LargestDistance Or NCheckOutDistance > LargestDistance Or OCheckInDistance > LargestDistance Or OCheckOutDistance > LargestDistance))";
                strHQL += " and UserName Like " + "'" + strUserName + "'";
                strHQL += " and UserCode in (Select UserCode From T_ProjectMember Where DepartCode Like " + "'" + strDepartCode + "'" + ")";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
            }
            else
            {
                strHQL = "Select * From T_UserAttendanceRecord  where to_char(AttendanceDate,'yyyymmdd') >= " + "'" + strStartTime + "'";
                strHQL += " and  to_char(AttendanceDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " and  UserName Like " + "'" + strUserName + "'";
                strHQL += " and  UserCode in (Select UserCode From T_ProjectMember Where DepartCode Like " + "'" + strDepartCode + "'" + ")";
                strHQL += " and COALESCE(LeaderCode,'') = '' ";
            }
        }
        strHQL += " Order by to_char(AttendanceDate,'yyyymmdd') DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserAttendanceRecord");

        Random a = new Random();
        string fileName = LanguageHandle.GetWord("YuanGongKaoQinBaoBiao").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";

        string strTableTitle = string.Format(@"<table cellspacing='0' cellpadding='4' id='DataGrid3' style='color:#333333;width:100%;border-collapse:collapse;'>
         <tr class='itemStyle'>
             <td class='itemBorder' align='center' style='width:4%;'> </td>
             <td class='itemBorder' align='center' style='width:8%;'></td>
             <td class='itemBorder' align='center' style='width:6%;'></td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
               
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                {0}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
               
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                
             </td>
             <td class='itemBorder' align='center' style='width:6%;'></td>
             <td class='itemBorder' align='center' style='width:4%;'></td>
             <td class='itemBorder' align='center' style='width:4%;'></td>
             <td class='itemBorder' align='center'></td>
         </tr>
         <tr class='itemStyle'>
             <td class='itemBorder' align='center' style='width:4%;'> ID</td>
             <td class='itemBorder' align='center' style='width:8%;'>{1}</td>   
             <td class='itemBorder' align='center' style='width:6%;'>{2}</td>   
             <td class='itemBorder' align='center' style='width:8%;'>
                 {3}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 {4}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 {5}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 {6}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 {7}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 {8}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 {9}   
             </td>
             <td class='itemBorder' align='center' style='width:8%;'>
                 {10}   
             </td>
             <td class='itemBorder' align='center' style='width:6%;'>{11} (Ã×)</td>   
             <td class='itemBorder' align='center' style='width:4%;'>{12} (·Ö)</td>   
             <td class='itemBorder' align='center' style='width:4%;'>{13} (·Ö)</td>   
             <td class='itemBorder' align='center'>{14}</td>   
         </tr>
     </table>",
      LanguageHandle.GetWord("YuanGongKaoQinBaoBiao"),
      LanguageHandle.GetWord("XingMing"),
      LanguageHandle.GetWord("ChuQinRiQi"),
      LanguageHandle.GetWord("ShangWuShangBanShiJian"),
      LanguageHandle.GetWord("ShangWuXiaBanShiJian"),
      LanguageHandle.GetWord("XiaWuShangBanShiJian"),
      LanguageHandle.GetWord("XiaWuXiaBanShiJian"),
      LanguageHandle.GetWord("WanBanShangBanShiJian"),
      LanguageHandle.GetWord("WanBanXiaBanShiJian"),
      LanguageHandle.GetWord("WuYeShangBanShiJian"),
      LanguageHandle.GetWord("WuYeXiaBanShiJian"),
      LanguageHandle.GetWord("YunXuZuiDaJuLi"),
      LanguageHandle.GetWord("ChiDao"),
      LanguageHandle.GetWord("ZaoTui"),
      LanguageHandle.GetWord("WeiZhi"));
        try
        {
            ShareClass.DataGridExportToExecl(DataGrid3, strTableTitle, fileName, ds);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }


    protected void BT_NewReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("TTUserAttendanceReportNew.aspx");
    }
}
