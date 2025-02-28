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

public partial class TTAllThirdCustomerOperationRecord : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","第三方操作记录", strUserCode.Trim());
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "第三方操作记录--" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        LB_UserCode.Text = strUserCode.Trim();
        LB_UserName.Text = ShareClass.GetUserName(strUserCode).Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode.Trim());
            LB_DepartString.Text = strDepartString;
            
            LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("DiSanFangCaoZuoJiLuLieBiao").ToString().Trim();

            strHQL = "from CustomerOperationRecord as customerOperationRecord where customerOperationRecord.UserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " Order By customerOperationRecord.ID ";
            CustomerOperationRecordBLL customerOperationRecordBLL = new CustomerOperationRecordBLL();
            lst = customerOperationRecordBLL.GetAllCustomerOperationRecords(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_LeaveInfoNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + lst.Count.ToString() + LanguageHandle.GetWord("TiaoJiLu").ToString().Trim();

            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;
        int intCount;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            intCount = LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            LB_ProjectMemberOwner.Text = strDepartName + LanguageHandle.GetWord("DeDiSanFangCaoZuoJiLu").ToString().Trim();
            LB_LeaveInfoNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + intCount.ToString() + LanguageHandle.GetWord("Tiao").ToString().Trim();

            LB_DepartCode.Text = strDepartCode;
        }
    }

    protected int LoadUserByDepartCodeForDataGrid(string strDepartCode, DataGrid dataGrid)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerOperationRecord as customerOperationRecord where customerOperationRecord.UserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode = '" + strDepartCode.Trim() + "')";
        strHQL += " Order By customerOperationRecord.ID ";
        CustomerOperationRecordBLL customerOperationRecordBLL = new CustomerOperationRecordBLL();
        lst = customerOperationRecordBLL.GetAllCustomerOperationRecords(strHQL);
        dataGrid.DataSource = lst;
        dataGrid.DataBind();

        return lst.Count;
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("DiSanFangCaoZuoJiLuLieBiao").ToString().Trim();

        string strDepartString = LB_DepartString.Text.Trim();

        string strUserCode = "%" + TB_UserCode.Text.Trim() + "%";
        string strUserName = "%" + TB_UserName.Text.Trim() + "%";
        string strStartTime = DLC_StartTime.Text.Trim();
        string strEndTime = DLC_EndTime.Text.Trim();

        strHQL = "from CustomerOperationRecord as customerOperationRecord where ";
        strHQL += " (customerOperationRecord.UserCode Like '" + strUserCode + "' or customerOperationRecord.Creater Like '" + strUserCode + "')";
        strHQL += " and (customerOperationRecord.UserName Like '" + strUserName + "' or customerOperationRecord.CreaterName Like '" + strUserName + "')";
        if (strStartTime != "")
        {
            strHQL += " and '" + strStartTime + "'::date-customerOperationRecord.CreateTime::date<=0";
        }
        if (strEndTime != "")
        {
            strHQL += " and '" + strEndTime + "'::date-customerOperationRecord.CreateTime::date>=0";
        }
        strHQL += " and customerOperationRecord.UserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order By customerOperationRecord.ID ";
        CustomerOperationRecordBLL customerOperationRecordBLL = new CustomerOperationRecordBLL();
        lst = customerOperationRecordBLL.GetAllCustomerOperationRecords(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_LeaveInfoNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + lst.Count.ToString() + LanguageHandle.GetWord("Tiao").ToString().Trim();

        LB_Sql.Text = strHQL;

        LB_DepartCode.Text = "";
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        CustomerOperationRecordBLL customerOperationRecordBLL = new CustomerOperationRecordBLL();
        IList lst = customerOperationRecordBLL.GetAllCustomerOperationRecords(strHQL);
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
                string fileName = LanguageHandle.GetWord("DiSanFangCaoZuoJiLu").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                CreateExcel(getUserList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim()+"')", true);
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

        if (strDepartCode == "")//所有请假信息
        {
            string strDepartString = LB_DepartString.Text.Trim();

            strHQL = "Select ID 'Number',UserCode 'UserCode',UserName 'UserName',CreaterName 'Operator',CreateTime 'OperationTime',Remark 'OperationRemark' from T_CustomerOperationRecord where " +  //ChineseWord //ChineseWord
                "UserCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
        
            if (!string.IsNullOrEmpty(TB_UserCode.Text.Trim()))
            {
                strHQL += " and (UserCode like '%" + TB_UserCode.Text.Trim() + "%' or Creater like '%" + TB_UserCode.Text.Trim() + "%')";
            }
            if (!string.IsNullOrEmpty(TB_UserName.Text.Trim()))
            {
                strHQL += " and (UserName like '%" + TB_UserName.Text.Trim() + "%' or CreaterName like '%" + TB_UserName.Text.Trim() + "%')";
            }
            if (DLC_StartTime.Text.Trim() != "")
            {
                strHQL += " and '" + DLC_StartTime.Text.Trim() + "'::date-CreateTime::date<=0";
            }
            if (DLC_EndTime.Text.Trim() != "")
            {
                strHQL += " and '" + DLC_EndTime.Text.Trim() + "'::date-CreateTime::date>=0";
            }
            strHQL += " Order by ID ASC";
        }
        else//按组织架构查询的
        {
            strHQL = "Select ID 'Number',UserCode 'UserCode',UserName 'UserName',CreaterName 'Operator',CreateTime 'OperationTime',Remark 'OperationRemark' from T_CustomerOperationRecord where " +  //ChineseWord //ChineseWord
                "UserCode in (Select UserCode From T_ProjectMember Where DepartCode = '" + strDepartCode + "') Order by ID ASC ";

        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerOperationRecord");
        return ds.Tables[0];
    }
}
