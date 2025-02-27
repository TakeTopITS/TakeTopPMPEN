using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Security.Permissions;
using System.Data.SqlClient;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;
using net.sf.mpxj.primavera.schema;
using Microsoft.Ajax.Utilities;


public partial class TTSystemActiveUserSet : System.Web.UI.Page
{
    string strCurrentUserType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;
        string strDepartString;

        strUserCode = Session["UserCode"].ToString();
        strCurrentUserType = ShareClass.GetUserType(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "系统用户设置", strUserCode);
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
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadSystemActiveUser(strDepartString);
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);

            LoadSystemActiveUserForDepartCode(strDepartCode, LB_DepartString.Text.Trim());

            LB_SelectedDepartCode.Text = strDepartCode;
        }
        else
        {
            LoadSystemActiveUser(LB_DepartString.Text.Trim());
            LB_SelectedDepartCode.Text = "";
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }

    protected void BT_AddSystemUser_Click(object sender, EventArgs e)
    {
        int i;
        string strDepartCode;

        //按部门添加系统用户
        i = AddSystemUserForDepartment();

        if (LB_SelectedDepartCode.Text != "")
        {
            strDepartCode = TreeView2.SelectedNode.Target;
            LoadSystemActiveUserForDepartCode(strDepartCode, LB_DepartString.Text.Trim());
        }
        else
        {
            LoadSystemActiveUser(LB_DepartString.Text.Trim());
        }

        if (i == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click77", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }

        if (i == -1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click11", "alert('" + LanguageHandle.GetWord("ZZJGWEBYHSYJDYNDZCSAPPLICENSEGDDSMBNZXZXDWEBYHL").ToString().Trim() + "')", true);
        }

        if (i == -2)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZJGAPPYHSYJDYNDZCSAPPLICENSEGDDSMBNZXZXDAPPYHL").ToString().Trim() + "')", true);
        }

        if (i == -3 || i == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click88", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_AllSystemUser_Click(object sender, EventArgs e)
    {
        LoadSystemActiveUser(LB_DepartString.Text.Trim());
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        int intWEBLicenseNumber, intAPPLicenseNumer, intWEBUserNumber, intAPPUserNumber;
        string strLicenseType;

        string strUserCode;
        string strUserName;

        string strIsWebUser = DL_IsWEBUser.SelectedValue.Trim();
        string strIsAppUser = DL_IsAPPUser.SelectedValue.Trim();

        string strDepartString = LB_DepartString.Text.Trim();

        strUserCode = TB_UserCode.Text.Trim();

        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        SystemActiveUser systemActiveUser = new SystemActiveUser();

        string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];

        TakeTopLicense license = new TakeTopLicense();
        intWEBLicenseNumber = license.GetWEBLicenseNumber(strServerName);
        intAPPLicenseNumer = license.GetAPPLicenseNumber(strServerName);

        //取得已增加的WEB和APP用户数
        intWEBUserNumber = GetWEBSystemUserNumber();
        intAPPUserNumber = GetAPPSystemUserNumber();


        strLicenseType = license.GetLicenseType(strServerName);
        if (strLicenseType == "REGISTER")
        {
            if (strIsWebUser == "YES")
            {
                intWEBUserNumber += 1;
            }

            if (strIsAppUser == "YES")
            {
                intAPPUserNumber += 1;
            }

            if (intWEBUserNumber > intWEBLicenseNumber)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGWEBYHSYJDYNDZCSAPPLICENSEGDDSMBNZXZXDWEBYHL").ToString().Trim() + "')", true);
                return;
            }

            if (intAPPUserNumber > intAPPLicenseNumer)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGAPPYHSYJDYNDZCSAPPLICENSEGDDSMBNZXZXDAPPYHL").ToString().Trim() + "')", true);
                return;
            }

            if (intWEBUserNumber <= intWEBLicenseNumber & intAPPUserNumber <= intAPPLicenseNumer)
            {
                if (strUserCode != "")
                {
                    if (GetUserCount(strUserCode) == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCYHBCZJC").ToString().Trim() + "')", true);
                        return;
                    }

                    strUserName = ShareClass.GetUserName(strUserCode);
                    TB_UserName.Text = strUserName;

                    if (ShareClass.VerifyUserCode(strUserCode, strDepartString) == false | ShareClass.VerifyUserName(strUserName, strDepartString) == false)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNMYXCZCYHJC").ToString().Trim() + "')", true);
                        return;
                    }

                    systemActiveUser.UserCode = strUserCode;
                    systemActiveUser.UserName = strUserName;
                    systemActiveUser.JoinTime = DateTime.Now;
                    systemActiveUser.OperatorCode = Session["UserCode"].ToString().Trim();
                    systemActiveUser.WebUser = strIsWebUser;
                    systemActiveUser.AppUser = strIsAppUser;

                    try
                    {
                        systemActiveUserBLL.AddSystemActiveUser(systemActiveUser);

                        if (LB_SelectedDepartCode.Text == "")
                        {
                            LoadSystemActiveUser(LB_DepartString.Text.Trim());
                        }
                        else
                        {
                            LoadSystemActiveUserForDepartCode(LB_SelectedDepartCode.Text.Trim(),LB_DepartString.Text.Trim());
                        }

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCCYKNYCZBNZFTJJC").ToString().Trim() + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBYHDMBNWK").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXTYHSYJDYNDSLICENSEGDDSMBNZXZXDYHL").ToString().Trim() + "')", true);
            }
        }
        else
        {
            if (strUserCode != "")
            {
                if (GetUserCount(strUserCode) == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCYHBCZJC").ToString().Trim() + "')", true);
                    return;
                }

                if (strIsAppUser == "YES")
                {
                    intAPPUserNumber += 1;
                }

                if (intAPPUserNumber > intAPPLicenseNumer)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGAPPYHSYJDYNDSAPPLICENSEGDDSMBNZXZXDAPPYHL").ToString().Trim() + "')", true);
                    return;
                }

                strUserName = ShareClass.GetUserName(strUserCode);
                TB_UserName.Text = strUserName;

                systemActiveUser.UserCode = strUserCode;
                systemActiveUser.UserName = strUserName;
                systemActiveUser.JoinTime = DateTime.Now;
                systemActiveUser.OperatorCode = Session["UserCode"].ToString().Trim();
                systemActiveUser.WebUser = strIsWebUser;
                systemActiveUser.AppUser = strIsAppUser;

                try
                {
                    systemActiveUserBLL.AddSystemActiveUser(systemActiveUser);

                    if (LB_SelectedDepartCode.Text == "")
                    {
                        LoadSystemActiveUser(LB_DepartString.Text.Trim());
                    }
                    else
                    {
                        LoadSystemActiveUserForDepartCode(LB_SelectedDepartCode.Text.Trim(), LB_DepartString.Text.Trim());
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCCYKNYCZBNZFTJJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBYHDMBNWK").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_FindWebUser_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strUserName, strIsWebUser, strDepartString;

        strUserCode = TB_UserCodeFind.Text.Trim();
        strUserName = TB_UserNameFind.Text.Trim();
        strIsWebUser = DL_CanWEBUser.SelectedValue.Trim();

        strDepartString = LB_DepartString.Text.Trim();

        strUserCode = "%" + strUserCode + "%";
        strUserName = "%" + strUserName + "%";

        strHQL = "Select * From T_SystemActiveUser as systemActiveUser Where systemActiveUser.UserCode Like " + "'" + strUserCode + "'";
        strHQL += " and systemActiveUser.UserName Like " + "'" + strUserName + "'";
        strHQL += " and systemActiveUser.WebUser = " + "'" + strIsWebUser + "'";
        strHQL += " and systemActiveUser.UserCode in (Select projectMember.UserCode From T_ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order By systemActiveUser.JoinTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        LB_FindUserNumber.Text = "WEB: " + ds.Tables[0].Rows.Count.ToString();

        LB_Sql.Text = strHQL;
    }

    protected void BT_FindAppUser_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strUserName, strIsAppUser, strDepartString;

        strUserCode = TB_UserCodeFind.Text.Trim();
        strUserName = TB_UserNameFind.Text.Trim();

        strIsAppUser = DL_CanAPPUser.SelectedValue.Trim();
        strDepartString = LB_DepartString.Text.Trim();

        strUserCode = "%" + strUserCode + "%";
        strUserName = "%" + strUserName + "%";

        strHQL = "Select * From T_SystemActiveUser as systemActiveUser Where systemActiveUser.UserCode Like " + "'" + strUserCode + "'";
        strHQL += " and systemActiveUser.UserName Like " + "'" + strUserName + "'";
        strHQL += " and systemActiveUser.AppUser = " + "'" + strIsAppUser + "'";
        strHQL += " and systemActiveUser.UserCode in (Select projectMember.UserCode From T_ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order By systemActiveUser.JoinTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        LB_FindUserNumber.Text = "APP: " + ds.Tables[0].Rows.Count.ToString();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = ((Button)e.Item.FindControl("BT_SystemUserCode")).Text.Trim();
            string strUserName = ((Button)e.Item.FindControl("BT_SystemUserName")).Text.Trim();
            string strIsWebUser = ((DropDownList)e.Item.FindControl("DL_WebUser")).SelectedValue;
            string strIsAppUser = ((DropDownList)e.Item.FindControl("DL_AppUser")).SelectedValue;

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
                ((Button)DataGrid4.Items[i].FindControl("BT_SystemUserCode")).ForeColor = Color.White;
                ((Button)DataGrid4.Items[i].FindControl("BT_SystemUserName")).ForeColor = Color.White;
            }
            e.Item.ForeColor = Color.Red;
            ((Button)e.Item.FindControl("BT_SystemUserCode")).ForeColor = Color.Red;
            ((Button)e.Item.FindControl("BT_SystemUserName")).ForeColor = Color.Red;

            TB_UserCode.Text = strUserCode;
            TB_UserName.Text = strUserName;
            DL_IsWEBUser.SelectedValue = strIsWebUser;
            DL_IsAPPUser.SelectedValue = strIsAppUser;

            if (e.CommandName == "Save")
            {
                try
                {
                    if (UpdateWebUser(strUserCode, strIsWebUser) & UpdateAppUser(strUserCode, strIsAppUser))
                    {
                        if (LB_SelectedDepartCode.Text == "")
                        {
                            LoadSystemActiveUser(LB_DepartString.Text.Trim());
                        }
                        else
                        {
                            LoadSystemActiveUserForDepartCode(LB_SelectedDepartCode.Text.Trim(), LB_DepartString.Text.Trim());
                        }

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click333", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click444", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click444", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                string strHQL;

                strHQL = "Delete From T_SystemActiveUser Where UserCode = '" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                if (LB_SelectedDepartCode.Text == "")
                {
                    LoadSystemActiveUser(LB_DepartString.Text.Trim());
                }
                else
                {
                    LoadSystemActiveUserForDepartCode(LB_SelectedDepartCode.Text.Trim(), LB_DepartString.Text.Trim());
                }
            }
        }
    }

    protected bool UpdateWebUser(string strUserCode, string strIsWebUser)
    {
        string strHQL;

        int intWEBLicenseNumber, intWEBUserNumber;
        string strLicenseType;

        string strDepartString = LB_DepartString.Text.Trim();


        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        SystemActiveUser systemActiveUser = new SystemActiveUser();

        string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];

        TakeTopLicense license = new TakeTopLicense();
        intWEBLicenseNumber = license.GetWEBLicenseNumber(strServerName);

        //取得已增加的WEB和APP用户数
        intWEBUserNumber = GetWEBSystemUserNumber();

        strLicenseType = license.GetLicenseType(strServerName);
        if (strLicenseType == "REGISTER")
        {
            if (strIsWebUser == "YES")
            {
                intWEBUserNumber += 1;
            }

            if (intWEBUserNumber > intWEBLicenseNumber)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click000", "alert('" + LanguageHandle.GetWord("ZZJGWEBYHYJZGSQWEBYHSQJC").ToString().Trim() + "')", true);
                return false;
            }

            strHQL = "Update T_SystemActiveUser Set WebUser = " + "'" + strIsWebUser + "'" + " Where UserCode =" + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            return true;
        }
        else
        {
            strHQL = "Update T_SystemActiveUser Set WebUser = " + "'" + strIsWebUser + "'" + " Where UserCode =" + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            return true;
        }
    }

    protected bool UpdateAppUser(string strUserCode, string strIsAppUser)
    {
        string strHQL;

        int intAPPLicenseNumer, intAPPUserNumber;
        string strLicenseType;

        string strDepartString = LB_DepartString.Text.Trim();


        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        SystemActiveUser systemActiveUser = new SystemActiveUser();

        string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];

        TakeTopLicense license = new TakeTopLicense();
        intAPPLicenseNumer = license.GetAPPLicenseNumber(strServerName);

        //取得已增加的APP用户数
        intAPPUserNumber = GetAPPSystemUserNumber();

        strLicenseType = license.GetLicenseType(strServerName);

        if (strIsAppUser == "YES")
        {
            intAPPUserNumber += 1;
        }

        if (intAPPUserNumber > intAPPLicenseNumer)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click000", "alert('" + LanguageHandle.GetWord("ZZJGAPPYHSYJDYNDZCSAPPLICENSEGDDSMBNZXZXDAPPYHL").ToString().Trim() + "')", true);
            return false;
        }

        strHQL = "Update T_SystemActiveUser Set AppUser = " + "'" + strIsAppUser + "'" + " Where UserCode =" + "'" + strUserCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        return true;
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL;
        IList lst;

        strHQL = LB_Sql.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ShareClass.GetUserName(strUserCode);

        TB_UserCode.Text = strUserCode;
        TB_UserName.Text = strUserName;
    }

    protected void LoadSystemActiveUserForDepartCode(string strDepartCode,string strDepartString)
    {
        string strHQL;

        strHQL = "select * From T_SystemActiveUser as systemActiveUser Where ";
        strHQL += " systemActiveUser.UserCode in (Select projectMember.UserCode From T_ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and systemActiveUser.UserCode in (Select UserCode From T_ProjectMember Where DepartCode = '" + strDepartCode + "')";
        strHQL += " Order By systemActiveUser.JoinTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        LB_SystemUserNumber.Text = ds.Tables[0].Rows.Count.ToString();

        LB_Sql.Text = strHQL;

        try
        {
            string strID;
            string strUserCode;
            string strIsWebUser, strIsAppUser;
            int j;


            for (j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                strID = DataGrid4.Items[j].Cells[0].Text;
                strIsWebUser = ds.Tables[0].Rows[j]["WebUser"].ToString().Trim();
                strIsAppUser = ds.Tables[0].Rows[j]["AppUser"].ToString().Trim();

                if (strIsWebUser == "YES")
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_WebUser")).SelectedValue = "YES";
                }
                else
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_WebUser")).SelectedValue = "NO";
                }

                if (strIsAppUser == "YES")
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_AppUser")).SelectedValue = "YES";
                }
                else
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_AppUser")).SelectedValue = "NO";
                }

                strUserCode = ((Button)DataGrid4.Items[j].FindControl("BT_SystemUserCode")).Text.Trim();
                if (strUserCode == "ADMIN" || strUserCode == "SAMPLE")
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_WebUser")).Enabled = false;
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_AppUser")).Enabled = false;

                    ((LinkButton)DataGrid4.Items[j].FindControl("LBT_Delete")).Visible = false;
                    ((Button)DataGrid4.Items[j].FindControl("BT_Save")).Visible = false;
                }
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());
        }
    }

    protected void LoadSystemActiveUser(string strDepartString)
    {
        string strHQL;

        strHQL = "select * From T_SystemActiveUser as systemActiveUser Where ";
        strHQL += " systemActiveUser.UserCode in (Select projectMember.UserCode From T_ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order By systemActiveUser.JoinTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        LB_SystemUserNumber.Text = ds.Tables[0].Rows.Count.ToString();

        LB_Sql.Text = strHQL;

        try
        {
            string strID;
            string strUserCode;
            string strIsWebUser, strIsAppUser;
            int j;


            for (j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                strID = DataGrid4.Items[j].Cells[0].Text;
                strIsWebUser = ds.Tables[0].Rows[j]["WebUser"].ToString().Trim();
                strIsAppUser = ds.Tables[0].Rows[j]["AppUser"].ToString().Trim();

                if (strIsWebUser == "YES")
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_WebUser")).SelectedValue = "YES";
                }
                else
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_WebUser")).SelectedValue = "NO";
                }

                if (strIsAppUser == "YES")
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_AppUser")).SelectedValue = "YES";
                }
                else
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_AppUser")).SelectedValue = "NO";
                }

                strUserCode = ((Button)DataGrid4.Items[j].FindControl("BT_SystemUserCode")).Text.Trim();
                if (strUserCode == "ADMIN" || strUserCode == "SAMPLE")
                {
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_WebUser")).Enabled = false;
                    ((DropDownList)DataGrid4.Items[j].FindControl("DL_AppUser")).Enabled = false;

                    ((LinkButton)DataGrid4.Items[j].FindControl("LBT_Delete")).Visible = false;
                    ((Button)DataGrid4.Items[j].FindControl("BT_Save")).Visible = false;
                }
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());
        }
    }

    //按部门添加系统用户
    protected int AddSystemUserForDepartment()
    {
        string strHQL;
        string strDepartCode, strUserCode, strUserName;
        string strDepartString = LB_DepartString.Text.Trim();
        int i;

        if (LB_SelectedDepartCode.Text == "")
        {
            return 0;
        }

        string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
        int intWEBLicenseNumber, intAPPLicenseNumer, intWEBUserNumber, intAPPUserNumber;
        string strLicenseType;

        TakeTopLicense license = new TakeTopLicense();
        intWEBLicenseNumber = license.GetWEBLicenseNumber(strServerName);
        intAPPLicenseNumer = license.GetAPPLicenseNumber(strServerName);
        strLicenseType = license.GetLicenseType(strServerName);

        string strIsWebUser = DL_IsWEBUser.SelectedValue.Trim();
        string strIsAppUser = DL_IsAPPUser.SelectedValue.Trim();

        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        SystemActiveUser systemActiveUser = new SystemActiveUser();

        try
        {
            strDepartCode = TreeView2.SelectedNode.Target;
            strHQL = "Select UserCode,UserName From T_ProjectMember Where DepartCode ='" + strDepartCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strUserCode = ds.Tables[0].Rows[i]["UserCode"].ToString().Trim();
                strUserName = ds.Tables[0].Rows[i]["UserName"].ToString().Trim();

                //判断是否已经存在此系统用户
                if (!checkIsSystemUser(strUserCode))
                {
                    //取得已增加的WEB和APP用户数
                    intWEBUserNumber = GetWEBSystemUserNumber();
                    intAPPUserNumber = GetAPPSystemUserNumber();

                    //LogClass.WriteLogFile(i.ToString() + " - " + strUserCode + " WEB " + strUserName);

                    if (strLicenseType == "REGISTER")
                    {
                        if (strIsWebUser == "YES")
                        {
                            intWEBUserNumber += 1;
                        }

                        if (strIsAppUser == "YES")
                        {
                            intAPPUserNumber += 1;
                        }

                        if (intWEBUserNumber > intWEBLicenseNumber)
                        {
                            return -1;
                        }

                        if (intAPPUserNumber > intAPPLicenseNumer)
                        {
                            return -2;
                        }

                        systemActiveUser.UserCode = strUserCode;
                        systemActiveUser.UserName = strUserName;
                        systemActiveUser.JoinTime = DateTime.Now;
                        systemActiveUser.OperatorCode = Session["UserCode"].ToString().Trim();
                        systemActiveUser.WebUser = strIsWebUser;
                        systemActiveUser.AppUser = strIsAppUser;

                        try
                        {
                            systemActiveUserBLL.AddSystemActiveUser(systemActiveUser);

                            //LogClass.WriteLogFile("Web:" + intWEBUserNumber.ToString() + " - " + intWEBLicenseNumber.ToString());
                            //LogClass.WriteLogFile("APP:" + intAPPUserNumber.ToString() + " - " + intAPPLicenseNumer.ToString());
                        }
                        catch (Exception err)
                        {
                            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                        }
                    }
                    else
                    {
                        if (strIsAppUser == "YES")
                        {
                            intAPPUserNumber += 1;
                        }

                        if (intAPPUserNumber > intAPPLicenseNumer)
                        {
                            return -2;
                        }


                        systemActiveUser.UserCode = strUserCode;
                        systemActiveUser.UserName = strUserName;
                        systemActiveUser.JoinTime = DateTime.Now;
                        systemActiveUser.OperatorCode = Session["UserCode"].ToString().Trim();
                        systemActiveUser.WebUser = strIsWebUser;
                        systemActiveUser.AppUser = strIsAppUser;

                        try
                        {
                            systemActiveUserBLL.AddSystemActiveUser(systemActiveUser);
                        }
                        catch (Exception err)
                        {
                            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                        }
                    }
                }
            }

            return 1;
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

            return -3;
        }
    }

    //检查此用户是不是系统用户；
    protected bool checkIsSystemUser(string strUserCode)
    {
        string strHQL;

        strHQL = "select * From T_SystemActiveUser Where UserCode = '" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected int GetWEBSystemUserNumber()
    {
        string strHQL;
        IList lst;

        strHQL = "From SystemActiveUser as systemActiveUser Where systemActiveUser.WebUser = 'YES'";
        SystemActiveUserBLL systemAcitveUserBLL = new SystemActiveUserBLL();
        lst = systemAcitveUserBLL.GetAllSystemActiveUsers(strHQL);

        return lst.Count;
    }

    protected int GetAPPSystemUserNumber()
    {
        string strHQL;
        IList lst;

        strHQL = "From SystemActiveUser as systemActiveUser Where systemActiveUser.AppUser = 'YES'";
        SystemActiveUserBLL systemAcitveUserBLL = new SystemActiveUserBLL();
        lst = systemAcitveUserBLL.GetAllSystemActiveUsers(strHQL);

        return lst.Count;
    }

    protected int GetUserCount(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        return lst.Count;
    }

}
