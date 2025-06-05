using System; using System.Resources;
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


public partial class TTUserPlanLogAnalyseReport : System.Web.UI.Page
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
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","查看所有成员计划", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}


        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB");

            strHQL = "from ProjectMember as projectMember ";
            strHQL += " Where projectMember.DepartCode in " + strDepartString;
            strHQL += " and projectMember.UserCode in (Select systemActiveUser.UserCode From SystemActiveUser as systemActiveUser)";
            strHQL += " Order by projectMember.SortNumber ASC";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD") + lst.Count.ToString() ;
            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
  
        string strDepartCode, strDepartName;
        string strLogOperator = DL_LogOperator.SelectedValue.Trim();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            string strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
            string strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");
            string strLeastPlanNumber = NB_LeastLogNubmer.Amount.ToString();

            LB_DepartCode.Text = strDepartCode;

            if (strLogOperator == "<")
            {
                strHQL = "Select * From T_ProjectMember Where DepartCode = " + "'" + strDepartCode + "'";
                strHQL += " and UserCode in (Select UserCode From T_SystemActiveUser)";
                strHQL += " and UserCode not in ";
                strHQL += " (Select UserCode From T_Plan_WorkLog Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " Group By UserCode Having Count(*) >= " + strLeastPlanNumber + ")";
                strHQL += " Order By SortNumber ASC";
            }
            else
            {
                strHQL = " Select * From T_Plan_WorkLog A, T_ProjectMember B Where A.UserCode = B.UserCode AND B.DepartCode = " + "'" + strDepartCode + "'"; 
                strHQL += " and B.UserCode in (Select UserCode From T_SystemActiveUser)";
                strHQL += " and B.UserCode not in ";
                strHQL += " (Select UserCode From T_Plan_WorkLog Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += " Group by UserCode Having Count(*) < " + strLeastPlanNumber + ")";
                strHQL += " and A.ID in (Select MAX(C.ID) From T_Plan_WorkLog C,T_Plan_WorkLog D Where C.UserCode =D.UserCode AND to_char(C.WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(C.WorkTime,'yyyymmdd')<= " + "'" + strEndTime + "'" + " Group By C.UserCode  ) ";

                strHQL += " Order By B.SortNumber ASC";
            }

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();

            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD") + ds.Tables[0].Rows.Count ;
            LB_Sql.Text = strHQL;
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB");

        string strDepartString = LB_DepartString.Text.Trim();
        string strLogOperator = DL_LogOperator.SelectedValue.Trim();

        string strUserCode = "%" + TB_UserCode.Text.Trim() + "%";
        string strUserName = "%" + TB_UserName.Text.Trim() + "%";
        string strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        string strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");
        string strLeastPlanNumber = NB_LeastLogNubmer.Amount.ToString();
       
        if (strLogOperator == "<")
        {
            strHQL = "Select * From T_ProjectMember Where UserCode Like " + "'" + strUserCode + "'";
            strHQL += " and UserName Like " + "'" + strUserName + "'";
            strHQL += " and DepartCode in " + strDepartString;
            strHQL += " and UserCode in (Select UserCode From T_SystemActiveUser)";
            strHQL += " and UserCode not in ";
            strHQL += " (Select UserCode From T_Plan_WorkLog Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
           strHQL += " Group By UserCode Having Count(*) >= " + strLeastPlanNumber + ")";

            strHQL += " Order By SortNumber ASC";
        }
        else
        {
            strHQL = " Select * From T_Plan_WorkLog A, T_ProjectMember B Where A.UserCode = B.UserCode ";
            strHQL += " and B.UserName Like " + "'" + strUserName + "'";
            strHQL += " and B.DepartCode in " + strDepartString;
            strHQL += " and B.UserCode in (Select UserCode From T_SystemActiveUser)";
            strHQL += " and B.UserCode not in ";
            strHQL += " (Select UserCode From T_Plan_WorkLog Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
            strHQL += " Group by UserCode Having Count(*) < " + strLeastPlanNumber + ")";
            strHQL += " and A.ID in (Select MAX(C.ID) From T_Plan_WorkLog C,T_Plan_WorkLog D Where C.UserCode =D.UserCode AND to_char(C.WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(C.WorkTime,'yyyymmdd')<= " + "'" + strEndTime + "'" + " Group By C.UserCode  ) ";

            strHQL += " Order By B.SortNumber ASC";
        }
       
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_UserNumber.Text = LanguageHandle.GetWord("GCXD") + ds.Tables[0].Rows.Count ;
        LB_Sql.Text = strHQL;

        LB_DepartCode.Text = "";
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("YongHuChengYuanXinXi") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                CreateExcel(getUserList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC")+"')", true);
            }
        }
    }

    private void CreateExcel(DataTable dt, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }

    protected DataTable getUserList()
    {
        string strHQL;
        string strDepartCode = LB_DepartCode.Text.Trim();
        string strLogOperator = DL_LogOperator.SelectedValue.Trim();

        string strStartTime = DateTime.Parse(DLC_StartTime.Text).ToString("yyyyMMdd");
        string strEndTime = DateTime.Parse(DLC_EndTime.Text).ToString("yyyyMMdd");
        string strLeastPlanNumber = NB_LeastLogNubmer.Amount.ToString();

        if (strDepartCode == "")//所有成员的情况
        {
            string strDepartString = LB_DepartString.Text.Trim();

          
            if (strLogOperator == "<")
            {
                strHQL = "Select UserCode 'Code',UserName 'Name',Gender 'Gender',Age 'Age',DepartCode 'DepartmentCode',DepartName 'DepartmentName'," +   
              "Duty 'Responsibility',OfficePhone 'OfficePhone',MobilePhone 'MobilePhone',EMail 'EMail',WorkScope 'ScopeOfWork',JoinDate 'JoinDate',Status 'Status'," +   
              LanguageHandle.GetWord("RefUserCodeCanKaoGongHaoIDCard") +
              "From T_ProjectMember Where DepartCode in " + strDepartString + " ";

                if (!string.IsNullOrEmpty(TB_UserCode.Text.Trim()))
                {
                    strHQL += " and UserCode like '%" + TB_UserCode.Text.Trim() + "%' ";
                }
                if (!string.IsNullOrEmpty(TB_UserName.Text.Trim()))
                {
                    strHQL += " and UserName like '%" + TB_UserName.Text.Trim() + "%' ";
                }

                strHQL += " and UserCode in (Select UserCode From T_SystemActiveUser)";
                strHQL += " and UserCode not in ";
                strHQL += " (Select UserCode From T_Plan_WorkLog  Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += "  Group By UserCode Having Count(*) >= " + strLeastPlanNumber + ")";
                strHQL += " Order by SortNumber ASC";
            }
            else
            {
                strHQL = "Select B.UserCode 'Code',B.UserName 'Name',B.Gender 'Gender',B.Age 'Age',B.DepartCode 'DepartmentCode',B.DepartName 'DepartmentName'," +   
                "B.Duty 'Responsibility',B.OfficePhone 'OfficePhone',B.MobilePhone 'MobilePhone',B.EMail 'EMail',B.WorkScope 'ScopeOfWork',B.JoinDate 'JoinDate',B.Status 'Status'," +   
                LanguageHandle.GetWord("BRefUserCodeCanKaoGongHaoBIDCa") +
                "From T_Plan_WorkLog A, T_ProjectMember B Where A.UserCode = B.UserCode and B.DepartCode in " + strDepartString + " ";

                if (!string.IsNullOrEmpty(TB_UserCode.Text.Trim()))
                {
                    strHQL += " and B.UserCode like '%" + TB_UserCode.Text.Trim() + "%' ";
                }
                if (!string.IsNullOrEmpty(TB_UserName.Text.Trim()))
                {
                    strHQL += " and B.UserName like '%" + TB_UserName.Text.Trim() + "%' ";
                }
                strHQL += " and B.UserCode in (Select UserCode From T_SystemActiveUser )";
                strHQL += " and B.UserCode not in ";
                strHQL += " (Select UserCode From T_Plan_WorkLog  Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += "  Group By UserCode Having Count(*) < " + strLeastPlanNumber + ")";
                strHQL += " and A.ID in (Select MAX(C.ID) From T_Plan_WorkLog C,T_Plan_WorkLog D Where C.UserCode =D.UserCode AND to_char(C.WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(C.WorkTime,'yyyymmdd')<= " + "'" + strEndTime + "'" + " Group By C.UserCode  ) ";

                strHQL += " Order by B.SortNumber ASC";
            }
        }
        else//按组织架构查询的
        {
            if (strLogOperator == "<")
            {
                strHQL = "Select UserCode 'Code',UserName 'Name',Gender 'Gender',Age 'Age',DepartCode 'DepartmentCode',DepartName 'DepartmentName'," +   
              "Duty 'Responsibility',OfficePhone 'OfficePhone',MobilePhone 'MobilePhone',EMail 'EMail',WorkScope 'ScopeOfWork',JoinDate 'JoinDate',Status 'Status'," +   
              LanguageHandle.GetWord("RefUserCodeCanKaoGongHaoIDCard") +
              "From T_ProjectMember Where DepartCode = '" + strDepartCode + "'";
                strHQL += " and UserCode in (Select UserCode From T_SystemActiveUser)";
                strHQL += " and UserCode not in ";
                strHQL += " (Select UserCode From T_Plan_WorkLog  Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += "  Group By UserCode Having Count(*) >= " + strLeastPlanNumber + ")";

                strHQL += " Order by SortNumber ASC";
            }
            else
            {
                strHQL = "Select B.UserCode 'Code',B.UserName 'Name',B.Gender 'Gender',B.Age 'Age',B.DepartCode 'DepartmentCode',B.DepartName 'DepartmentName'," +   
              "B.Duty 'Responsibility',B.OfficePhone 'OfficePhone',B.MobilePhone 'MobilePhone',B.EMail 'EMail',B.WorkScope 'ScopeOfWork',B.JoinDate 'JoinDate',B.Status 'Status'," +   
              LanguageHandle.GetWord("BRefUserCodeCanKaoGongHaoBIDCa") +
              "From T_Plan_WorkLog A, T_ProjectMember B Where A.UserCode = B.UserCode and B.DepartCode = '" + strDepartCode + "'";

                strHQL += " and B.UserCode in (Select UserCode From T_SystemActiveUser )";
                strHQL += " and B.UserCode not in ";
                strHQL += " (Select UserCode From T_Plan_WorkLog  Where  to_char(WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(WorkTime,'yyyymmdd') <= " + "'" + strEndTime + "'";
                strHQL += "  Group By UserCode Having Count(*) < " + strLeastPlanNumber + ")";
                strHQL += " and A.ID in (Select MAX(C.ID) From T_Plan_WorkLog C,T_Plan_WorkLog D Where C.UserCode =D.UserCode AND to_char(C.WorkTime,'yyyymmdd') >= " + "'" + strStartTime + "'" + " and to_char(C.WorkTime,'yyyymmdd')<= " + "'" + strEndTime + "'" + " Group By C.UserCode  ) ";

                strHQL += " Order by B.SortNumber ASC";
            }

        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        return ds.Tables[0];
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
