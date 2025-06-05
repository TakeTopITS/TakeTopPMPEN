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

using System.IO;


using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopCore;
using TakeTopSecurity;
using NHibernate.Util;

public partial class TTProModuleAuthority : System.Web.UI.Page
{
    string strCurrentUserCode, strCurrentUserDepartCode;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strCurrentUserCode = Session["UserCode"].ToString().Trim();
        strCurrentUserDepartCode = ShareClass.GetDepartCodeFromUserCode(strCurrentUserCode);

        strLangCode = Session["LangCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strCurrentUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "用户权限管理", strCurrentUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            string strSystemVersionType = Session["SystemVersionType"].ToString();
            if (strSystemVersionType == "GROUP")
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"), TreeView1, strCurrentUserCode);
            }
            else
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT"), TreeView1);
            }

            //从模板用户复制模组
            string strSampleUserCode = System.Configuration.ConfigurationManager.AppSettings["ModuleSampleUser"];
            ShareClass.InitialUserModules(strSampleUserCode, strCurrentUserCode);

            TakeTopAuthority.LoadParentModule(strCurrentUserCode, strCurrentUserCode, DataGrid3, strLangCode);
            TakeTopAuthority.LoadDepartParentRelatedModule(strCurrentUserDepartCode, strCurrentUserDepartCode, DataGrid1, strLangCode);
            TakeTopAuthority.LoadDepartChildModule(strCurrentUserDepartCode, strCurrentUserDepartCode, "0", "0", "", DataGrid5, strLangCode);


            try
            {
                SetMailBoxAuthority(strCurrentUserCode);
            }
            catch
            {
            }


            LB_AuthorityUserCode.Text = strCurrentUserCode;
            LB_AuthorityUserName.Text = ShareClass.GetUserName(strCurrentUserCode);

            LB_UserCode.Text = strCurrentUserCode;
            LB_UserName.Text = ShareClass.GetUserName(strCurrentUserCode);

            LB_DepartCode.Text = strCurrentUserDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strCurrentUserDepartCode);

            BT_UserAuthoritySave.Enabled = false;
            BT_DepartAuthoritySave.Enabled = false;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartCode, strSampleUserCode;


        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            TabContainer1.ActiveTabIndex = 0;

            //从模板用户复制模组
            strSampleUserCode = System.Configuration.ConfigurationManager.AppSettings["ModuleSampleUser"];
            TakeTopAuthority.CopyDepartmentAuthorityFromSampleUser(strDepartCode, strSampleUserCode, strCurrentUserCode);

            //装载模组到DATAGRID
            TakeTopAuthority.LoadDepartParentRelatedModule(strDepartCode, strCurrentUserCode, DataGrid1, strLangCode);
            TakeTopAuthority.LoadDepartChildModule(strDepartCode, strCurrentUserCode, "0", "0", "", DataGrid5, strLangCode);
            TakeTopAuthority.LoadDepartChildModule(strDepartCode, strCurrentUserCode, "0", "0", "", DataGrid8, strLangCode);

            LoadUserNotInDepartModule(strDepartCode);

            LB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);

            LB_SelectedDepartCode.Text = LB_DepartCode.Text;

            LB_ParentUserType.Text = "";
            LB_DepartUserType.Text = "";

            if (RB_OperationType.SelectedIndex == 0)
            {
                LB_RelatedDepartCode.Text = strDepartCode;
                LB_RelatedDepartName.Text = ShareClass.GetDepartName(strDepartCode);
            }

            if (strDepartCode == strCurrentUserDepartCode)
            {
                BT_DepartAuthoritySave.Enabled = false;
            }
            else
            {
                BT_DepartAuthoritySave.Enabled = true;
            }
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }

    protected void BT_FindUserName_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserName, strDepartCode, strDepartString;

        strDepartCode = LB_DepartCode.Text.Trim();
        strUserName = TB_FindUserName.Text.Trim();

        strDepartString = LB_DepartString.Text.Trim();

        if (TreeView1.SelectedNode != null)
        {
            strHQL = "Select * From T_ProjectMember Where UserName Like '%" + strUserName + "%'" + " and DepartCode = '" + strDepartCode + "'";
        }
        else
        {
            strHQL = "Select * From T_ProjectMember Where UserName Like '%" + strUserName + "%'" + " and DepartCode in " + strDepartString;
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode, strUserName, strDepartCode, strSampleUserCode;

        strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        LB_SelectedUserCode.Text = strUserCode;
        strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();
        strDepartCode = LB_RelatedDepartCode.Text.Trim();

        //从模板用户复制模组
        strSampleUserCode = System.Configuration.ConfigurationManager.AppSettings["ModuleSampleUser"];
        TakeTopAuthority.CopyUserAuthorityFromSampleUser(strDepartCode, strSampleUserCode, strUserCode);

        //装载模组到DATAGRID
        TakeTopAuthority.LoadParentModule(strUserCode, strCurrentUserCode, DataGrid3, strLangCode);
        TakeTopAuthority.LoadChildModule(strUserCode, strCurrentUserCode, "0", "0", "", DataGrid4, strLangCode);
        TakeTopAuthority.LoadChildModule(strUserCode, strCurrentUserCode, "0", "0", "", DataGrid7, strLangCode);

        try
        {
            //设置此用户邮件权限
            SetMailBoxAuthority(strUserCode);
        }
        catch
        {
        }

        if (RB_OperationType.SelectedIndex == 0)
        {
            TabContainer1.ActiveTabIndex = 0;
        }
        else
        {
            UserNotInDepartModuleBLL userNotInDepartModuleBLL = new UserNotInDepartModuleBLL();
            UserNotInDepartModule userNotInDepartModule = new UserNotInDepartModule();
            userNotInDepartModule.DepartCode = strDepartCode;
            userNotInDepartModule.UserCode = strUserCode;
            userNotInDepartModule.UserName = strUserName;

            try
            {
                userNotInDepartModuleBLL.AddUserNotInDepartModule(userNotInDepartModule);
                LoadUserNotInDepartModule(strDepartCode);
            }
            catch
            {
            }
        }

        TB_UserCode1.Text = strUserCode;

        LB_AuthorityUserCode.Text = strUserCode;
        LB_AuthorityUserName.Text = strUserName;

        LB_ParentUserType.Text = "";
        LB_DepartUserType.Text = "";

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        if (strUserCode == strCurrentUserCode)
        {
            BT_UserAuthoritySave.Enabled = false;
        }
        else
        {
            BT_UserAuthoritySave.Enabled = true;
        }
    }


    protected void BT_ModuleFind_Click(object sender, EventArgs e)
    {
        string strSelectedUserCode, strSelectedDepartCode;
        string strFindModuleName;

        strSelectedUserCode = LB_SelectedUserCode.Text;
        strSelectedDepartCode = LB_SelectedDepartCode.Text;

        if (strSelectedUserCode == "")
        {
            strSelectedUserCode = strCurrentUserCode;
        }
        if (strSelectedDepartCode == "")
        {
            strSelectedDepartCode = strCurrentUserDepartCode;
        }

        strFindModuleName = TB_ModuleFind.Text.Trim();

        try
        {
            LoadModule(strSelectedUserCode, strCurrentUserCode, DataGrid3, strLangCode, strFindModuleName);
            LoadDepartRelatedModule(strSelectedDepartCode, strCurrentUserCode, DataGrid1, strLangCode, strFindModuleName);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ShiBai") + "')", true);
        }
    }

    public static void LoadModule(string strUserCode, string strCurrentUserCode, DataGrid dataGrid, string strLangCode, string strModuleName)
    {
        string strHQL;
        string strVisible;

        strHQL = "Select Distinct A.ID,B.HomeModuleName as ModuleName,A.UserCode,A.Visible,B.ModuleType,B.UserType,B.SortNumber from T_ProModule A,T_ProModule C,T_ProModuleLevel B ";
        strHQL += " Where Rtrim(A.ModuleName) || Rtrim(A.ModuleType) || Rtrim(A.UserType) = Rtrim(C.ModuleName) || Rtrim(C.ModuleType) || Rtrim(C.UserType)";
        strHQL += " and Rtrim(C.ModuleName) || Rtrim(C.ModuleType) || Rtrim(C.UserType) = Rtrim(B.ModuleName) || Rtrim(B.ModuleType) || Rtrim(B.UserType)";
        strHQL += " and B.Visible = 'YES' AND B.IsDeleted = 'NO'";
        strHQL += " and A.UserCode =" + "'" + strUserCode + "'";

        //strHQL += " and length(rtrim(ltrim(B.ParentModule))) = 0 ";
        strHQL += " and A.ModuleName Like '%" + strModuleName + "%'";

        strHQL += " and C.UserCode = " + "'" + strCurrentUserCode + "'" + " and C.Visible = 'YES'";
        strHQL += " and B.LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order By B.ModuleType DESC, B.UserType ASC, B.SortNumber ASC";

        //
        DataSet ds = CoreShareClass.GetDataSetFromSql(strHQL, "T_ProModule");
        dataGrid.DataSource = ds;
        dataGrid.DataBind();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strVisible = ds.Tables[0].Rows[i][3].ToString().Trim();

            if (strVisible == "YES")
            {
                ((CheckBox)dataGrid.Items[i].FindControl("CB_ParentVisible")).Checked = true;
            }
            else
            {
                ((CheckBox)dataGrid.Items[i].FindControl("CB_ParentVisible")).Checked = false;
            }
            //((DropDownList)dataGrid.Items[i].FindControl("DL_ParentVisible")).SelectedValue = strVisible;
        }
    }

    public static void LoadDepartRelatedModule(string strDepartCode, string strCurrentUserCode, DataGrid dataGrid, string strLangCode, string strModuleName)
    {
        string strHQL;
        string strVisible;

        strHQL = "Select Distinct A.ID,B.HomeModuleName as ModuleName,A.DepartCode,A.Visible,B.ModuleType,B.UserType,B.SortNumber from T_DepartRelatedModule A,T_ProModule C,T_ProModuleLevel B";
        strHQL += " Where Rtrim(A.ModuleName) || Rtrim(A.ModuleType) || Rtrim(A.UserType) = Rtrim(C.ModuleName) || Rtrim(C.ModuleType) || Rtrim(C.UserType)";
        strHQL += " and Rtrim(C.ModuleName) || Rtrim(C.ModuleType) || Rtrim(C.UserType) = Rtrim(B.ModuleName) || Rtrim(B.ModuleType) || Rtrim(B.UserType)";
        strHQL += " and B.Visible = 'YES' AND B.IsDeleted = 'NO'";
        strHQL += " and A.DepartCode =" + "'" + strDepartCode + "'";

        //strHQL += " and length(rtrim(ltrim(B.ParentModule))) = 0 ";
        strHQL += " and C.ModuleName Like '%" + strModuleName + "%'";

        strHQL += " and C.UserCode = " + "'" + strCurrentUserCode + "'" + " and C.Visible = 'YES'";
        strHQL += " and B.LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order By B.ModuleType DESC, B.UserType ASC,B.SortNumber ASC";

        //
        DataSet ds = CoreShareClass.GetDataSetFromSql(strHQL, "T_DepartRelateModule");
        dataGrid.DataSource = ds;
        dataGrid.DataBind();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strVisible = ds.Tables[0].Rows[i][3].ToString().Trim();

            if (strVisible == "YES")
            {
                ((CheckBox)dataGrid.Items[i].FindControl("CB_ParentVisible")).Checked = true;
            }
            else
            {
                ((CheckBox)dataGrid.Items[i].FindControl("CB_ParentVisible")).Checked = false;
            }
            //((DropDownList)dataGrid.Items[i].FindControl("DL_ParentVisible")).SelectedValue = strVisible;
        }
    }


    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strModuleName, strModuleType;
        string strUserCode, strUserType;

        //保存权限数据
        if (BT_UserAuthoritySave.Enabled)
        {
            SaveUserAuthoritySave();
        }

        string strID = ((Button)e.Item.FindControl("BT_ParentID")).Text.Trim();
        for (int i = 0; i < DataGrid3.Items.Count; i++)
        {
            DataGrid3.Items[i].ForeColor = Color.Black;
            ((Button)DataGrid3.Items[i].FindControl("BT_ParentID")).ForeColor = Color.White;
        }
        e.Item.ForeColor = Color.Red;
        ((Button)e.Item.FindControl("BT_ParentID")).ForeColor = Color.Red;

        strHQL = "from ProModule as proModule where proModule.ID = " + strID;
        ProModuleBLL proModuleBLL = new ProModuleBLL();
        lst = proModuleBLL.GetAllProModules(strHQL);

        ProModule proModule = (ProModule)lst[0];

        strModuleType = proModule.ModuleType.Trim();
        LB_AuthorityUserCode.Text = proModule.UserCode;
        LB_AuthorityUserName.Text = ShareClass.GetUserName(proModule.UserCode.Trim());

        strModuleName = proModule.ModuleName.Trim();
        strUserCode = LB_AuthorityUserCode.Text.Trim();

        LB_ParentModuleID.Text = strID;
        strUserType = e.Item.Cells[5].Text.Trim();
        LB_ParentUserType.Text = strUserType;

        LB_ParentHomeModule.Text = ShareClass.GetHomeModuleName(strModuleName, strLangCode);

        TakeTopAuthority.LoadChildModule(strUserCode, strCurrentUserCode, strModuleName, strModuleType, strUserType, DataGrid4, strLangCode);
        LB_ParentModule.Text = strModuleName;
        LB_Index.Text = "3";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        try
        {
            SaveUserAuthoritySave();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            //设置缓存更改标志
            ChangePageCache();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = ((Button)e.Item.FindControl("BT_ChildID")).Text.Trim();
        string strHQL;
        IList lst;

        string strModuleName, strModuleType;
        string strUserCode, strUserType;

        //保存权限数据
        if (BT_UserAuthoritySave.Enabled)
        {
            SaveUserAuthoritySave();
        }

        for (int i = 0; i < DataGrid4.Items.Count; i++)
        {
            DataGrid4.Items[i].ForeColor = Color.Black;
            ((Button)DataGrid4.Items[i].FindControl("BT_ChildID")).ForeColor = Color.White;
        }
        e.Item.ForeColor = Color.Red;
        ((Button)e.Item.FindControl("BT_ChildID")).ForeColor = Color.Red;

        strHQL = "from ProModule as proModule where proModule.ID = " + strID;
        ProModuleBLL proModuleBLL = new ProModuleBLL();
        lst = proModuleBLL.GetAllProModules(strHQL);

        ProModule proModule = (ProModule)lst[0];

        strModuleName = proModule.ModuleName.Trim();
        strUserCode = LB_AuthorityUserCode.Text.Trim();

        strModuleType = proModule.ModuleType.Trim();
        strUserType = e.Item.Cells[5].Text.Trim();

        LB_ChildModuleID.Text = strID;
        LB_ChildHomeModule.Text = ShareClass.GetHomeModuleName(proModule.ModuleName.Trim(), strLangCode);
        LB_AuthorityUserCode.Text = proModule.UserCode;
        LB_AuthorityUserName.Text = ShareClass.GetUserName(proModule.UserCode.Trim());

        TakeTopAuthority.LoadChildModule(strUserCode, strCurrentUserCode, strModuleName, strModuleType, strUserType, DataGrid7, strLangCode);

        LB_Index.Text = "4";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        try
        {
            SaveUserAuthoritySave();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            //设置缓存更改标志
            ChangePageCache();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();

        for (int i = 0; i < DataGrid7.Items.Count; i++)
        {
            DataGrid7.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        //保存权限数据
        if (BT_UserAuthoritySave.Enabled)
        {
            SaveUserAuthoritySave();
        }
    }

    protected void BT_UserAuthoritySave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveUserAuthoritySave();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            //设置缓存更改标志
            ChangePageCache();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

    protected void SaveUserAuthoritySave()
    {
        string strHQL;

        string strParentID, strChildID, strGrandSonID;
        string strVisible;
        int i = 0, j = 0, k = 0;

        string strUserCode = LB_AuthorityUserCode.Text.Trim();
        string strModuleType = "0";
        string strUserType = LB_ParentUserType.Text.Trim();

        try
        {
            for (i = 0; i < DataGrid3.Items.Count; i++)
            {
                strParentID = ((Button)DataGrid3.Items[i].FindControl("BT_ParentID")).Text.Trim();

                if (((CheckBox)DataGrid3.Items[i].FindControl("CB_ParentVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }

                strHQL = "Update T_ProModule Set Visible = " + "'" + strVisible + "'" + " Where ID = " + strParentID;
                ShareClass.RunSqlCommand(strHQL);
            }

            for (j = 0; j < DataGrid4.Items.Count; j++)
            {
                strChildID = ((Button)DataGrid4.Items[j].FindControl("BT_ChildID")).Text.Trim();
                strModuleType = DataGrid4.Items[j].Cells[4].Text;

                if (((CheckBox)DataGrid4.Items[j].FindControl("CB_ChildVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }

                strHQL = "Update T_ProModule Set Visible = " + "'" + strVisible + "'" + " Where ID = " + strChildID;
                ShareClass.RunSqlCommand(strHQL);
            }

            for (k = 0; k < DataGrid7.Items.Count; k++)
            {
                strGrandSonID = DataGrid7.Items[k].Cells[0].Text;
                strModuleType = DataGrid7.Items[k].Cells[4].Text;

                if (((CheckBox)DataGrid7.Items[k].FindControl("CB_ChildVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }

                strHQL = "Update T_ProModule Set Visible = " + "'" + strVisible + "'" + " Where ID = " + strGrandSonID;
                ShareClass.RunSqlCommand(strHQL);
            }
        }
        catch
        {

        }
    }

    protected void BT_UserAuthorityCopy_Click(object sender, EventArgs e)
    {
        string strUserCode1 = TB_UserCode1.Text.Trim().ToUpper();
        string strUserCode2 = TB_UserCode2.Text.Trim().ToUpper();
        string strDepartString = LB_DepartString.Text.Trim();

        string strUserCode = LB_UserCode.Text.Trim();

        if (strUserCode != "ADMIN" & strUserCode1 == "ADMIN")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZYADMINYHCNBADMINXFZGTYHJC") + "')", true);
            return;
        }

        if (strUserCode2 == "ADMIN")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGADMINWGLYZHYZGXWXBFZXJC") + "')", true);
            return;
        }

        if (ShareClass.VerifyUserCode(strUserCode1, strDepartString) & ShareClass.VerifyUserCode(strUserCode2, strDepartString))
        {
            CopyUserModuleAuthority(strUserCode1, strUserCode2);

            //设置缓存更改标志
            ChangePageLeftColumnCache();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYHDMYWHYHCCNGLDFWJC") + "')", true);
        }
    }

    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageLeftColumnCache()
    {
        //设置缓存更改标志
        ShareClass.SetPageCacheMark("1");
        Session["CssDirectoryChangeNumber"] = "1";

        //设置缓存更改标志
        ShareClass.AddSpaceLineToLeftColumnForRefreshCache();
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strModuleName, strModuleType;
        string strDepartCode, strDepartUserType;

        string strID = ((Button)e.Item.FindControl("BT_ParentID")).Text;

        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            DataGrid1.Items[i].ForeColor = Color.Black;
            ((Button)DataGrid1.Items[i].FindControl("BT_ParentID")).ForeColor = Color.White;
        }
        e.Item.ForeColor = Color.Red;
        ((Button)e.Item.FindControl("BT_ParentID")).ForeColor = Color.Red;

        strHQL = "from DepartRelatedModule as departRelatedModule where departRelatedModule.ID = " + strID;
        DepartRelatedModuleBLL departRelatedModuleBLL = new DepartRelatedModuleBLL();
        lst = departRelatedModuleBLL.GetAllDepartRelatedModules(strHQL);
        DepartRelatedModule departRelatedModule = (DepartRelatedModule)lst[0];

        LB_DepartModuleID.Text = strID;
        LB_DepartModuleName.Text = departRelatedModule.ModuleName.Trim();
        strModuleType = departRelatedModule.ModuleType.Trim();
        LB_DepartCode.Text = departRelatedModule.DepartCode.Trim();
        LB_DepartName.Text = ShareClass.GetDepartName(departRelatedModule.DepartCode.Trim());

        strModuleName = LB_DepartModuleName.Text.Trim();

        strDepartCode = LB_DepartCode.Text.Trim();

        strDepartUserType = e.Item.Cells[5].Text;
        LB_DepartUserType.Text = strDepartUserType;

        TakeTopAuthority.LoadDepartChildModule(strDepartCode, strCurrentUserCode, strModuleName, strModuleType, strDepartUserType, DataGrid5, strLangCode);
        TakeTopAuthority.LoadDepartChildModule(strDepartCode, strCurrentUserCode, "0", "0", "", DataGrid8, strLangCode);

        LB_ParentDepartModule.Text = strModuleName;
        LB_ParentHomeDepartModule.Text = ShareClass.GetHomeModuleName(departRelatedModule.ModuleName.Trim(), strLangCode);
        LB_DepartModuleIndex.Text = "1";

        if (BT_DepartAuthoritySave.Enabled)
        {
            //保存权限数据
            SaveDepartAuthirity();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowDepartment','true') ", true);
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = ((Button)e.Item.FindControl("BT_ChildID")).Text;
        string strHQL;
        IList lst;

        string strModuleName, strModuleType;
        string strDepartCode, strDepartUserType;

        for (int i = 0; i < DataGrid5.Items.Count; i++)
        {
            DataGrid5.Items[i].ForeColor = Color.Black;
            ((Button)DataGrid5.Items[i].FindControl("BT_ChildID")).ForeColor = Color.White;
        }
        e.Item.ForeColor = Color.Red;
        ((Button)e.Item.FindControl("BT_ChildID")).ForeColor = Color.Red;

        if (BT_DepartAuthoritySave.Enabled)
        {
            //保存权限数据
            SaveDepartAuthirity();
        }


        strHQL = "from DepartRelatedModule as departRelatedModule where departRelatedModule.ID = " + strID;
        DepartRelatedModuleBLL departRelatedModuleBLL = new DepartRelatedModuleBLL();
        lst = departRelatedModuleBLL.GetAllDepartRelatedModules(strHQL);
        DepartRelatedModule departRelatedModule = (DepartRelatedModule)lst[0];


        LB_DepartModuleID.Text = strID;
        LB_DepartModuleName.Text = departRelatedModule.ModuleName.Trim();
        LB_DepartCode.Text = departRelatedModule.DepartCode.Trim();
        LB_DepartName.Text = ShareClass.GetDepartName(departRelatedModule.DepartCode.Trim());

        strModuleName = LB_DepartModuleName.Text.Trim();
        strDepartCode = LB_DepartCode.Text.Trim();
        strModuleType = departRelatedModule.ModuleType.Trim();
        strDepartUserType = e.Item.Cells[5].Text;
        LB_DepartUserType.Text = strDepartUserType;

        TakeTopAuthority.LoadDepartChildModule(strDepartCode, strCurrentUserCode, strModuleName, strModuleType, strDepartUserType, DataGrid8, strLangCode);

        LB_ChildDepartModule.Text = strModuleName;
        LB_ChildHomeDepartModule.Text = ShareClass.GetHomeModuleName(departRelatedModule.ModuleName.Trim(), strLangCode);
        LB_DepartModuleIndex.Text = "5";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowDepartment','true','popDetailWindowDepartment') ", true);

    }

    protected void BT_NewMainDepartment_Click(object sender, EventArgs e)
    {
        try
        {
            SaveDepartAuthirity();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();

        for (int i = 0; i < DataGrid8.Items.Count; i++)
        {
            DataGrid8.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        if (BT_DepartAuthoritySave.Enabled)
        {
            //保存权限数据
            SaveDepartAuthirity();
        }

    }

    protected void BT_NewDetailDepartment_Click(object sender, EventArgs e)
    {
        try
        {
            SaveDepartAuthirity();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowDepartment','true') ", true);
    }

    protected void BT_DepartAuthoritySave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveDepartAuthirity();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

    //保存权权限数据
    protected void SaveDepartAuthirity()
    {
        string strHQL;
        string strParentID, strChildID;
        string strVisible;
        int i = 0, j = 0, k = 0;
        string strChildModuleType = "0";


        string strDepartCode = LB_DepartCode.Text.Trim();
        string strParentDepartModule = LB_ParentDepartModule.Text.Trim();
        string strDepartModuleIndex = LB_DepartModuleIndex.Text.Trim();
        string strDepartUserType = LB_DepartUserType.Text.Trim();
        string strChildDepartModule = LB_ChildDepartModule.Text.Trim();

        try
        {
            for (i = 0; i < DataGrid1.Items.Count; i++)
            {
                strParentID = ((Button)DataGrid1.Items[i].FindControl("BT_ParentID")).Text.Trim();
                if (((CheckBox)DataGrid1.Items[i].FindControl("CB_ParentVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }
                //strVisible = ((DropDownList)DataGrid1.Items[i].FindControl("DL_ParentVisible")).SelectedValue;

                strHQL = "Update T_DepartRelatedModule Set Visible = " + "'" + strVisible + "'" + " Where ID = " + strParentID;
                ShareClass.RunSqlCommand(strHQL);
            }

            for (j = 0; j < DataGrid5.Items.Count; j++)
            {
                strChildID = ((Button)DataGrid5.Items[j].FindControl("BT_ChildID")).Text.Trim();

                if (((CheckBox)DataGrid5.Items[j].FindControl("CB_ChildVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }
                //strVisible = ((DropDownList)DataGrid5.Items[j].FindControl("DL_ChildVisible")).SelectedValue;

                strChildModuleType = DataGrid5.Items[j].Cells[4].Text;
                strHQL = "Update T_DepartRelatedModule Set Visible = " + "'" + strVisible + "'" + " Where ID = " + strChildID;
                ShareClass.RunSqlCommand(strHQL);
            }

            for (k = 0; k < DataGrid8.Items.Count; k++)
            {
                strChildID = DataGrid8.Items[k].Cells[0].Text;

                if (((CheckBox)DataGrid8.Items[k].FindControl("CB_ChildVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }
                //strVisible = ((DropDownList)DataGrid8.Items[k].FindControl("DL_ChildVisible")).SelectedValue;

                strHQL = "Update T_DepartRelatedModule Set Visible = " + "'" + strVisible + "'" + " Where ID = " + strChildID;
                ShareClass.RunSqlCommand(strHQL);
            }
        }
        catch
        {

        }
    }

    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strDepartCode;

        strID = e.Item.Cells[0].Text.Trim();
        strDepartCode = LB_RelatedDepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            strHQL = "From UserNotInDepartModule as userNotInDepartModule Where userNotInDepartModule.ID = " + strID;
            UserNotInDepartModuleBLL userNotInDepartModuleBLL = new UserNotInDepartModuleBLL();
            lst = userNotInDepartModuleBLL.GetAllUserNotInDepartModules(strHQL);
            UserNotInDepartModule userNotInDepartModule = (UserNotInDepartModule)lst[0];

            try
            {
                userNotInDepartModuleBLL.DeleteUserNotInDepartModule(userNotInDepartModule);
                LoadUserNotInDepartModule(strDepartCode);
            }
            catch
            {
            }
        }
    }


    protected void BT_DepartAuthorityCopy_Click(object sender, EventArgs e)
    {
        string strFromDepartCode = TB_FromDepartCode.Text.Trim().ToUpper();
        string strToDepartCode = TB_ToDepartCode.Text.Trim().ToUpper();
        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartString = LB_DepartString.Text.Trim();

        if (TakeTopAuthority.VerifyDepartCode(strFromDepartCode, strUserCode, strDepartString) & TakeTopAuthority.VerifyDepartCode(strToDepartCode, strUserCode, strDepartString))
        {
            CopyDepartModules(strFromDepartCode, strToDepartCode);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBMDMYWHBMCCNGLDFWJC") + "')", true);
        }
    }

    protected void BT_EffactSameDepartUser_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartCode, strChildDepartCode, strUserCode;

        strDepartCode = LB_RelatedDepartCode.Text.Trim();

        strHQL = "From ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
        strHQL += " and projectMember.UserCode not in (Select userNotInDepartModule.UserCode From UserNotInDepartModule as userNotInDepartModule where userNotInDepartModule.DepartCode = " + "'" + strDepartCode + "'" + ")";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = new ProjectMember();

        try
        {

            for (int i = 0; i < lst.Count; i++)
            {
                projectMember = (ProjectMember)lst[i];

                strUserCode = projectMember.UserCode.Trim();

                strHQL = "Delete From T_ProModule Where UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_ProModule (ModuleName,UserCode,Visible,ModuleType) ";
                strHQL += " Select ModuleName," + "'" + strUserCode + "'" + ",Visible,ModuleType From T_DepartRelatedModule Where DepartCode = " + "'" + strDepartCode + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            strHQL = "From Department as department where department.ParentCode = " + "'" + strDepartCode + "'";
            DepartmentBLL departmentBLL = new DepartmentBLL();
            lst = departmentBLL.GetAllDepartments(strHQL);
            Department department = new Department();

            for (int j = 0; j < lst.Count; j++)
            {
                department = (Department)lst[j];
                strChildDepartCode = department.DepartCode.Trim();

                strHQL = "Delete From T_DepartRelatedModule Where DepartCode = " + "'" + strChildDepartCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_DepartRelatedModule (ModuleName,DepartCode,Visible,ModuleType) ";
                strHQL += " Select ModuleName," + "'" + strChildDepartCode + "'" + ",Visible,ModuleType From T_DepartRelatedModule Where DepartCode = " + "'" + strDepartCode + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZXSJCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWFZXSJSBJC") + "')", true);
        }
    }

    protected void BT_MailBoxSet_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        int intID;

        strHQL = "From MailBoxAuthority as mailBoxAuthority where mailBoxAuthority.UserCode = " + "'" + strUserCode + "'";
        MailBoxAuthorityBLL mailBoxAuthorityBLL = new MailBoxAuthorityBLL();
        lst = mailBoxAuthorityBLL.GetAllMailBoxAuthoritys(strHQL);

        MailBoxAuthority mailBoxAuthority = (MailBoxAuthority)lst[0];

        intID = mailBoxAuthority.ID;

        if (CB_PasswordSet.Checked == true)
        {
            mailBoxAuthority.PasswordSet = "YES";
        }
        else
        {
            mailBoxAuthority.PasswordSet = "NO";
        }

        if (CB_DeleteOperate.Checked == true)
        {
            mailBoxAuthority.DeleteOperate = "YES";
        }
        else
        {
            mailBoxAuthority.DeleteOperate = "NO";
        }

        try
        {
            mailBoxAuthorityBLL.UpdateMailBoxAuthority(mailBoxAuthority, intID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void LoadUserNotInDepartModule(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserNotInDepartModule as userNotInDepartModule where userNotInDepartModule.DepartCode = " + "'" + strDepartCode + "'";
        UserNotInDepartModuleBLL userNotInDepartModuleBLL = new UserNotInDepartModuleBLL();
        lst = userNotInDepartModuleBLL.GetAllUserNotInDepartModules(strHQL);

        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();
    }

    protected void CopyUserModuleAuthority(string strUserCode1, string strUserCode2)
    {
        string strHQL;

        if (strUserCode1 != "" & strUserCode2 != "")
        {
            try
            {
                strHQL = "Delete from T_ProModule Where UserCode = " + "'" + strUserCode2 + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "insert into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType,ModuleDefinition,DiyFlow) ";
                strHQL += " select ModuleName," + "'" + strUserCode2 + "'" + ",Visible,ModuleType,UserType,ModuleDefinition,DiyFlow from T_ProModule ";
                strHQL += " where UserCode = " + "'" + strUserCode1 + "'";
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZXSJCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZXSJSBJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHDMBNWKFZSB") + "')", true);
        }
    }

    protected void CopyDepartModules(string strFromDepartCode, string strToDepartCode)
    {
        string strHQL;

        if (strFromDepartCode != "" & strToDepartCode != "")
        {
            try
            {
                strHQL = "Delete from T_DepartRelatedModule Where DepartCode = " + "'" + strToDepartCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "insert into T_DepartRelatedModule(ModuleName,DepartCode,Visible,ModuleType,UserType) ";
                strHQL += " select ModuleName," + "'" + strToDepartCode + "'" + ",Visible,ModuleType,UsreType from T_DepartRelatedModule ";
                strHQL += " where DepartCode = " + "'" + strFromDepartCode + "'";

                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZXSJCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZXSJSBJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBMDMBNWKFZSB") + "')", true);
        }
    }

    protected void SetMailBoxAuthority(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strPasswordSet, strDeleteOperate;

        strHQL = "From MailBoxAuthority as mailBoxAuthority where mailBoxAuthority.UserCode = " + "'" + strUserCode + "'";
        MailBoxAuthorityBLL mailBoxAuthorityBLL = new MailBoxAuthorityBLL();
        lst = mailBoxAuthorityBLL.GetAllMailBoxAuthoritys(strHQL);

        MailBoxAuthority mailBoxAuthority = (MailBoxAuthority)lst[0];

        strPasswordSet = mailBoxAuthority.PasswordSet.Trim();
        strDeleteOperate = mailBoxAuthority.DeleteOperate.Trim();

        if (strPasswordSet == "YES")
        {
            CB_PasswordSet.Checked = true;
        }
        else
        {
            CB_PasswordSet.Checked = false;
        }

        if (strDeleteOperate == "YES")
        {
            CB_DeleteOperate.Checked = true;
        }
        else
        {
            CB_DeleteOperate.Checked = false;
        }

        HL_MailProfile.NavigateUrl = "TTMailProfileSet.aspx?UserCode=" + strUserCode;
    }

    protected string GetMailBoxPassword(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strMailBoxPassword;

        strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        lst = mailProfileBLL.GetAllMailProfiles(strHQL);

        if (lst.Count > 0)
        {
            MailProfile mailProfile = (MailProfile)lst[0];

            try
            {
                strMailBoxPassword = mailProfile.Password.Trim();
            }
            catch
            {
                strMailBoxPassword = "";
            }
        }
        else
        {
            strMailBoxPassword = "";
        }

        return strMailBoxPassword;
    }


    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {
        //设置缓存更改标志
        ShareClass.SetPageCacheMark("1");
        Session["CssDirectoryChangeNumber"] = "1";

        //设置缓存更改标志
        ShareClass.AddSpaceLineToLeftColumnForRefreshCache();
    }


}
