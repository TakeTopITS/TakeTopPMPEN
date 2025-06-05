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
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTUserLoginManage : System.Web.UI.Page
{
    string strOperatorCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["UserCode"] = "C7094";
        strOperatorCode = Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strOperatorCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","用户登录管理", strOperatorCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            if (strOperatorCode == "ADMIN")
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree( LanguageHandle.GetWord("ZZJGT"),TreeView1);
                DL_IsAllMember.Enabled = true;

                BT_AddMDIStyle.Enabled = true;
            }
            else
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"),TreeView1, strOperatorCode);
                DL_IsAllMember.Enabled = false;

                BT_AddMDIStyle.Enabled = false;
            }

            string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strOperatorCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            LoadSystemMDIStyle();

            LoadLoginUserList(strOperatorCode);
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

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode, strIP;

        strUserCode = TB_UserCode.Text.Trim();
        strIP = TB_IP.Text.Trim();

        strUserCode = "%" + strUserCode + "%";
        strIP = "%" + strIP + "%";

        strHQL = "Select ID,UserCode,UserName,IP,Message,isForbidLogin,Status From T_UserLoginManage ";
        strHQL += " Where UserCode Like " + "'" + strUserCode + "'" + " and IP Like " + "'" + strIP + "'";
        strHQL += " Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_AllLoginUser_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "Select ID,UserCode,UserName,IP,Message,isForbidLogin,Status From T_UserLoginManage ";
        strHQL += " OperatorCode in (Select UserCode From T_ProjectMember Where DepartCode in " + LB_DepartString.Text.Trim() + ")";
        strHQL += " Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DL_IsAllMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strIsAllMember;

        strIsAllMember = DL_IsAllMember.SelectedValue.Trim();

        if (strIsAllMember == "YES")
        {
            TB_UserCode.Enabled = false;
            TB_IP.Enabled = false;
        }
        else
        {
            TB_UserCode.Enabled = true;
            TB_IP.Enabled = true;
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strIsAllMember, strUserCode, strUserName, strIP, strIsForbidLogin, strStatus, strMessage;

        strUserCode = TB_UserCode.Text.Trim();
        try
        {
            strUserName = ShareClass.GetUserName(strUserCode);
            LB_UserName.Text = strUserName;
        }
        catch
        {
            strUserName = "";
            LB_UserName.Text = "";
        }

        strIsAllMember = DL_IsAllMember.SelectedValue.Trim();
        strIP = TB_IP.Text.Trim();
        strMessage = TB_Msg.Text.Trim();
        strIsForbidLogin = DL_IsForbidLogin.SelectedValue;
        strStatus = DL_Status.SelectedValue;

        try
        {
            strHQL = "Insert Into T_UserLoginManage(IsAllMember,UserCode,UserName,IP,Message,IsForbidLogin,Status,OperatorCode,OperateTime)";
            strHQL += " Values(" + "'" + strIsAllMember + "'" + "," + "'" + strUserCode + "'" + "," + "'" + strUserName + "'" + "," + "'" + strIP + "'" + "," + "'" + strMessage + "'" + "," + "'" + strIsForbidLogin + "'" + "," + "'" + strStatus + "'" + "," + "'" + strOperatorCode + "'" + ",now())";

            ShareClass.RunSqlCommand(strHQL);

            LB_ID.Text = ShareClass.GetMyCreatedMaxUserLoginManageID();

            LoadLoginUserList(strOperatorCode);

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC")+"')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID = LB_ID.Text.Trim();

        string strIsAllMember, strUserCode, strUserName, strIP, strIsForbidLogin, strStatus, strMessage;

        strUserCode = TB_UserCode.Text.Trim();
        try
        {
            strUserName = ShareClass.GetUserName(strUserCode);
            LB_UserName.Text = strUserName;
        }
        catch
        {
            strUserName = "";
            LB_UserName.Text = "";
        }

        strIsAllMember = DL_IsAllMember.SelectedValue.Trim();
        strIP = TB_IP.Text.Trim();
        strMessage = TB_Msg.Text.Trim();
        strIsForbidLogin = DL_IsForbidLogin.SelectedValue;
        strStatus = DL_Status.SelectedValue;

        try
        {
            strHQL = "Update T_UserLoginManage Set IsAllMember = " + "'" + strIsAllMember + "'" + ", UserCode = " + "'" + strUserCode + "'" + ",UserName = " + "'" + strUserName + "'" + ",IP = " + "'" + strIP + "'" + ",Message = " + "'" + strMessage + "'" + ",IsForbidLogin = " + "'" + strIsForbidLogin + "'" + ",Status = " + "'" + strStatus + "'";
            strHQL += " Where ID = " + strID;

            ShareClass.RunSqlCommand(strHQL);

            LoadLoginUserList(strOperatorCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGSBJC")+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID = LB_ID.Text.Trim();

        strHQL = "Delete From T_UserLoginManage Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadLoginUserList(strOperatorCode);

            BT_Delete.Enabled = false;
            BT_Update.Enabled = false;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC")+"')", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;


        TB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;


        strHQL = "Select ID,IsAllMember, UserCode,UserName,IP,Message,isForbidLogin,Status From T_UserLoginManage Where UserCode = " + "'" + strUserCode + "'";
        strHQL += " Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strID;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text;

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "Select IsAllMember,UserCode,UserName,IP,Message,isForbidLogin,Status From T_UserLoginManage Where ID = " + strID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");

            LB_ID.Text = strID;
            DL_IsAllMember.SelectedValue = ds.Tables[0].Rows[0][0].ToString().Trim();
            TB_UserCode.Text = ds.Tables[0].Rows[0][1].ToString();
            LB_UserName.Text = ds.Tables[0].Rows[0][2].ToString();
            TB_IP.Text = ds.Tables[0].Rows[0][3].ToString();
            TB_Msg.Text = ds.Tables[0].Rows[0][4].ToString();
            DL_IsForbidLogin.SelectedValue = ds.Tables[0].Rows[0][5].ToString().Trim();
            DL_Status.SelectedValue = ds.Tables[0].Rows[0][6].ToString().Trim();

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text.Trim(); ;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DataGrid35_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strMDIStyle = ((Button)e.Item.FindControl("BT_MDIStyle")).Text.Trim();
        string strPageName = e.Item.Cells[1].Text.Trim();
        string strMobilePageName = e.Item.Cells[2].Text.Trim();
        string strThirdPartPageName = e.Item.Cells[3].Text.Trim();
        string strThirdPartMobilePageName = e.Item.Cells[4].Text.Trim();
        string strSortNumber = e.Item.Cells[5].Text.Trim();

        for (int i = 0; i < DataGrid35.Items.Count; i++)
        {
            DataGrid35.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        TB_MDIStyle.Text = strMDIStyle;
        TB_MDIStypePageNamePC.Text = strPageName;
        TB_MDIStylePageNameMobile.Text = strMobilePageName;
        TB_ThirdPartPageName.Text = strThirdPartPageName;
        TB_ThirdPartMobilePageName.Text = strThirdPartMobilePageName;

        TB_MDIStyleSort.Text = strSortNumber;

        BT_AppleAllUser.Enabled = true;
        BT_UpdateMDIStyle.Enabled = true;
        BT_DeleteMDIStyle.Enabled = true;

        if (strMDIStyle == LanguageHandle.GetWord("ZuoYouXiaZhan"))
        {
            BT_DeleteMDIStyle.Enabled = false;
        }
        else
        {
            BT_DeleteMDIStyle.Enabled = true;
        }

        if (strOperatorCode != "ADMIN")
        {
            BT_AddMDIStyle.Enabled = false;
            BT_DeleteMDIStyle.Enabled = false;
            BT_UpdateMDIStyle.Enabled = false;
            BT_AppleAllUser.Enabled = false;
        }
    }

    protected void BT_AddMDIStyle_Click(object sender, EventArgs e)
    {
        string strMDIStyle, strPageName, strMobilePageName, strThirdPartPageName, strThirdPartMobilePageName, strSortNumber;

        strMDIStyle = TB_MDIStyle.Text;
        strPageName = TB_MDIStypePageNamePC.Text;
        strMobilePageName = TB_MDIStylePageNameMobile.Text;
        strThirdPartPageName = TB_ThirdPartPageName.Text;
        strThirdPartMobilePageName = TB_ThirdPartMobilePageName.Text;
        strSortNumber = TB_MDIStyleSort.Text;

        SystemMDIStyle systemMDIStyle = new SystemMDIStyle();
        systemMDIStyle.MDIStyle = strMDIStyle;
        systemMDIStyle.PageName = strPageName;
        systemMDIStyle.MobilePageName = strMobilePageName;
        systemMDIStyle.ThirdPartPageName = strThirdPartPageName;
        systemMDIStyle.ThirdPartMobilePageName = strThirdPartMobilePageName;
        systemMDIStyle.SortNumber = int.Parse(strSortNumber);

        try
        {
            SystemMDIStyleBLL systemMDIStyleBLL = new SystemMDIStyleBLL();

            systemMDIStyleBLL.AddSystemMDIStyle(systemMDIStyle);

            BT_DeleteMDIStyle.Enabled = true;
            BT_AppleAllUser.Enabled = true;

            LoadSystemMDIStyle();
        }
        catch
        {
        }
    }

    protected void BT_UpdateMDIStyle_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strMDIStyle, strPageName, strMobilePageName, strThirdPartPageName, strThirdPartMobilePageName, strSortNumber;

        strMDIStyle = TB_MDIStyle.Text;
        strPageName = TB_MDIStypePageNamePC.Text;
        strMobilePageName = TB_MDIStylePageNameMobile.Text;
        strThirdPartPageName = TB_ThirdPartPageName.Text;
        strThirdPartMobilePageName = TB_ThirdPartMobilePageName.Text;
        strSortNumber = TB_MDIStyleSort.Text;

        strHQL = "From SystemMDIStyle as systemMDIStyle Where systemMDIStyle.MDIStyle = " + "'" + strMDIStyle + "'";
        SystemMDIStyleBLL systemMDIStyleBLL = new SystemMDIStyleBLL();
        lst = systemMDIStyleBLL.GetAllSystemMDIStyles(strHQL);
        SystemMDIStyle systemMDIStyle  = (SystemMDIStyle)lst[0];
        
        systemMDIStyle.PageName = strPageName;
        systemMDIStyle.MobilePageName = strMobilePageName;
        systemMDIStyle.ThirdPartPageName = strThirdPartPageName;
        systemMDIStyle.ThirdPartMobilePageName = strThirdPartMobilePageName;
        systemMDIStyle.SortNumber = int.Parse(strSortNumber);

        try
        {

            systemMDIStyleBLL.UpdateSystemMDIStyle(systemMDIStyle, strMDIStyle);

            LoadSystemMDIStyle();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch

        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSB") + "')", true);
        }
    }

    protected void BT_DeleteMDIStyle_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strMDIStyle;

        strMDIStyle = TB_MDIStyle.Text;


        try
        {
            strHQL = "Delete From T_SystemMDIStyle Where MDIStyle = " + "'" + strMDIStyle + "'";
            ShareClass.RunSqlCommand(strHQL);

            BT_DeleteMDIStyle.Enabled = false;
            BT_AppleAllUser.Enabled = false;

            LoadSystemMDIStyle();
        }
       catch
        {
        }
    }

    protected void BT_AppleAllUser_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strMDIStyle = TB_MDIStyle.Text.Trim();

        if (strMDIStyle != "")
        {
            strHQL = "Update T_ProjectMember Set MDIStyle = " + "'" + strMDIStyle + "'";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGSYYHDYCYCZJMYS")+"')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC")+"')", true);
        }
    }

    protected void DL_MDIStylePagePC_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strMDIPage = DL_MDIStylePagePC.SelectedValue.Trim();

        TB_MDIStypePageNamePC.Text = strMDIPage;
    }

    protected void DL_MDIStylePageMobile_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strMDIPage = DL_MDIStylePageMobile.SelectedValue.Trim();

        TB_MDIStylePageNameMobile.Text = DL_MDIStylePageMobile.SelectedValue.Trim();
    }

    protected void DL_ThirdPartPageName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCSMDIPage = DL_ThirdPartPageName.SelectedValue.Trim();

        TB_ThirdPartPageName.Text = strCSMDIPage;
    }

    protected void DL_ThirdPartMobilePageName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCSMobileMDIPage = DL_ThirdPartMobilePageName.SelectedValue.Trim();

        TB_ThirdPartMobilePageName.Text = strCSMobileMDIPage;
    }

    protected void LoadLoginUserList(string strUserCode)
    {
        string strHQL;

        strHQL = "Select ID,IsAllMember,UserCode,UserName,IP,Message,isForbidLogin,Status From T_UserLoginManage ";
        strHQL += " Where OperatorCode in (Select UserCode From T_ProjectMember Where DepartCode in " + LB_DepartString.Text.Trim() + ")";
        strHQL += " Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadSystemMDIStyle()
    {
        string strHQL = "from SystemMDIStyle as systemMDIStyle Order By systemMDIStyle.SortNumber ASC";

        SystemMDIStyleBLL systemMDIStyleBLL = new SystemMDIStyleBLL();
        IList lst = systemMDIStyleBLL.GetAllSystemMDIStyles(strHQL);


        DataGrid35.DataSource = lst;
        DataGrid35.DataBind();
    }
}
