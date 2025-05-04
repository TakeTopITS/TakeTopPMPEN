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

using System.Data.SqlClient;
using System.Data.OleDb;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;
using System.IO;
using jdk.nashorn.@internal.objects.annotations;
using Microsoft.Web.Management.Client.Win32;
using net.sf.mpxj.primavera.schema;
using static org.apache.poi.poifs.macros.Module;

public partial class TTSuperSystemModuleSet : System.Web.UI.Page
{
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        strLangCode = Session["LangCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility("TTSystemModuleSet.aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility("TTSystemModuleSet", "SystemModuleSettings", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            ShareClass.LoadLanguageForDropList(ddlLangSwitcher);
            ddlLangSwitcher.SelectedValue = strLangCode;

            InitialModuleTree("INNER", ddlLangSwitcher.SelectedValue.Trim());
        }
    }

    protected void DL_ForUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strUserType;

        strUserType = DL_ForUserType.SelectedValue.Trim();

        InitialModuleTree(strUserType, ddlLangSwitcher.SelectedValue.Trim());
    }

    protected void InitialModuleTree(string strUserType, string strLangCode)
    {
        string strHQL;

        string strModuleID, strModuleName, strModuleType, strHomeModuleName;

        //添加根节点
        TreeView1.Nodes.Clear();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();

        node1.Text = "<b>" + LanguageHandle.GetWord("XiTongMoZu").ToString().Trim() + "<b>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "Select ID,ModuleName,HomeModuleName,ModuleType From T_ProModuleLevel Where UserType = " + "'" + strUserType + "'" + " and char_length(ParentModule) = 0 ";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order By ModuleType DESC,SortNumber ASC";
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strModuleID = ds1.Tables[0].Rows[i][0].ToString();
            strModuleName = ds1.Tables[0].Rows[i][1].ToString();
            strHomeModuleName = ds1.Tables[0].Rows[i][2].ToString();
            strModuleType = ds1.Tables[0].Rows[i][3].ToString().Trim();

            node2 = new TreeNode();

            node2.Text = (i + 1).ToString() + "." + strHomeModuleName;
            node2.Target = strModuleID;
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);

            ModuleTreeShow(strUserType, strModuleType, strModuleName, node2, strLangCode);

            TreeView1.DataBind();
        }
    }

    public static void ModuleTreeShow(string strParentUserType, string strParentModuleType, string strParentModule, TreeNode treeNode, string strLangCode)
    {
        string strHQL;

        string strModuleID, strModuleName, strModuleType, strHomeModuleName;
        TreeNode node1 = new TreeNode();

        if (strParentModuleType == "APP" | strParentModuleType == "DIYAPP")
        {
            strHQL = "Select ID,ModuleName,HomeModuleName,ModuleType From T_ProModuleLevel Where UserType = " + "'" + strParentUserType + "'" + " and ParentModule = " + "'" + strParentModule + "' and ModuleType in ( 'APP','DIYAPP')";
        }
        else
        {
            strHQL = "Select ID,ModuleName,HomeModuleName,ModuleType From T_ProModuleLevel Where UserType = " + "'" + strParentUserType + "'" + " and ParentModule = " + "'" + strParentModule + "'";
        }
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order By SortNumber ASC";
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strModuleID = ds1.Tables[0].Rows[i]["ID"].ToString();
            strModuleName = ds1.Tables[0].Rows[i]["ModuleName"].ToString();
            strHomeModuleName = ds1.Tables[0].Rows[i]["HomeModuleName"].ToString();
            strModuleType = ds1.Tables[0].Rows[i]["ModuleType"].ToString().Trim();

            node1 = new TreeNode();
            node1.Text = (i + 1).ToString() + "." + strHomeModuleName;
            node1.Target = strModuleID;

            node1.Expanded = false;

            treeNode.ChildNodes.Add(node1);

            ModuleTreeShow(strParentUserType, strModuleType, strModuleName, node1, strLangCode);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strModuleID, strModuleName, strModuleType, strUserType;
        int intLevel;

        strUserType = DL_ForUserType.SelectedValue.Trim();
        strLangCode = ddlLangSwitcher.SelectedValue.Trim();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        intLevel = treeNode.Depth;
        LB_Level.Text = intLevel.ToString();

        strModuleID = treeNode.Target.Trim();
        strModuleType = GetLeftModuleType(strModuleID);
        strModuleName = GetLeftModuleName(strModuleID);

        if (strModuleID == "0")
        {
            LB_SelectedModuleName.Text = "";
            LB_HomeSelectedModuleName.Text = "";

            LoadChildModule("", strModuleType, strUserType, strLangCode);
        }
        else
        {
            strHQL = "Select ID,ParentModule,ModuleName,PageName,SortNumber,ModuleType,UserType,Visible,HomeModuleName From T_ProModuleLevel Where ID = " + strModuleID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

            strModuleType = ds.Tables[0].Rows[0][5].ToString().Trim();

            LB_SelectedModuleName.Text = strModuleName;
            LB_HomeSelectedModuleName.Text = ShareClass.GetHomeModuleName(strModuleName,strLangCode);
            LB_ModuleType.Text = strModuleType;

            LoadChildModule(strModuleName, strModuleType, strUserType, strLangCode);
        }
    }

    protected void BT_CheckPWD_Click(object sender, EventArgs e)
    {
        string strPassword = TB_Password.Text.Trim();

        if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
        {
            LB_Password.Text = strPassword;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
        }

        return;
    }

    protected void ddlLangSwitcher_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strModuleName, strModuleType, strUserType;

        strLangCode = ddlLangSwitcher.SelectedValue.Trim();

        strUserType = DL_ForUserType.SelectedValue.Trim();
        strModuleType = GetLeftModuleType("0");
        strModuleName = GetLeftModuleName("0");

        InitialModuleTree(strUserType, ddlLangSwitcher.SelectedValue.Trim());

        LoadChildModule("", strModuleType, strUserType, strLangCode);
    }

    protected void BT_CopyAllModuleForHomeLanguage_Click(object sender, EventArgs e)
    {
        string strPassword = LB_Password.Text.Trim();

        if (strPassword != "TAKETOPZXCZXJXYYZLY@#!")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            CopyAllPageModuleForHomeLanguage();
            CopyAllLeftModuleForHomeLanguage();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG").ToString().Trim() + "')", true);
        }
        catch (System.Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBJC").ToString().Trim() + "')", true);
        }

        return;
    }

    protected void BT_SynchronizeModuleDataFromExcel_Click(object sender, EventArgs e)
    {
        string strUserType, strModuleType, strModuleName;

        string strPassword = LB_Password.Text.Trim();

        if (strPassword != "TAKETOPZXCZXJXYYZLY@#!")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            UpdateLeftBarModules();
            UpdatePageBarModules();

            //刷新模组
            strLangCode = ddlLangSwitcher.SelectedValue.Trim();
            strUserType = DL_ForUserType.SelectedValue.Trim();
            strModuleType = GetLeftModuleType("0");
            strModuleName = GetLeftModuleName("0");

            InitialModuleTree(strUserType, ddlLangSwitcher.SelectedValue.Trim());

            LoadChildModule("", strModuleType, strUserType, strLangCode);

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click11", "alert('" + LanguageHandle.GetWord("ZZTBMZYYSJCG").ToString().Trim() + "')", true);
        }

        catch (System.Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("SBKNWJGSBDHLCDBGQJC").ToString().Trim() + "')", true);
        }

        return;
    }

    protected void BT_ExportToExcelForLeftModules_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Random a = new Random();

            string strPassword = LB_Password.Text.Trim();

            try
            {
                if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
                {
                    CreateLeftBarModuleExcel("Modules.xls");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }


            try
            {
                if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
                {
                    CreatePageBarModuleExcel("PageModules.xls");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
                }
            }
            catch (System.Exception err)
            {
                LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }

            return;

        }
    }

    protected void BT_ExportToExcelForPageModules_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Random a = new Random();

            string strPassword = LB_Password.Text.Trim();

            try
            {
                if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
                {
                    CreatePageBarModuleExcel("PageModules.xls");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
                }
            }
            catch (System.Exception err)
            {
                LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }

            return;
        }
    }

    protected void BT_DeleteUnVisibleModule_Click(object sender, EventArgs e)
    {
        string strHQL;

        if (Page.IsValid)
        {
            Random a = new Random();

            string strPassword = LB_Password.Text.Trim();


            if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
            {

                strHQL = "Update T_ProModuleLevel Set IsDeleted = 'YES' Where Visible = 'NO'";
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZYinCangChengGong").ToString().Trim()+"')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZMiMaBuDuiYinCangShiBaiQingJi").ToString().Trim()+"')", true);
            }

            return;
        }
    }


    protected void BT_ClearSystemBeginnerData_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Random a = new Random();

            string strPassword = LB_Password.Text.Trim();

            if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
            {
                string strHQL2 = "Update T_SystemDataManageForBeginer Set IsForbit = 'NO' Where OperationName = 'ClearData'";
                ShareClass.RunSqlCommand(strHQL2);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "top.frames[0].frames[2].parent.frames[\"rightTabFrame\"].popShowByURL('Outer/TTSystemDataManageForImplementationBeginner.aspx','SystemDataManage','99%','99%',window.location);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
            }

            return;
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strModuleID, strModuleName, strModuleType, strUserType, strParentModule;
            string strCommandName, strPageName, strOldPageName, strOldModuleType;

            strModuleID = e.Item.Cells[0].Text;
            strModuleName = e.Item.Cells[1].Text;
            strParentModule = e.Item.Cells[4].Text;

            strCommandName = e.CommandName.ToString();

            strOldPageName = GetLeftModulePageName(strModuleID);
            strPageName = ((TextBox)(e.Item.FindControl("TB_PageName"))).Text.Trim();

            strOldModuleType = GetLeftModuleType(strModuleID);
            strModuleType = ((DropDownList)(e.Item.FindControl("DL_ModuleType"))).SelectedValue.Trim();

            strUserType = e.Item.Cells[9].Text;

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            try
            {
                string strPassword = LB_Password.Text.Trim();

                if (strCommandName == "SAVE")
                {
                    if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
                    {
                        try
                        {
                            strHQL = "Update T_ProModuleLevel Set PageName = '" + strPageName + "',ModuleType = '" + strModuleType + "'";
                            strHQL += " Where PageName = '" + strOldPageName + "'" + " and ModuleType = '" + strOldModuleType + "'";
                            ShareClass.RunSqlCommand(strHQL);

                            strHQL = "Update T_ProModule Set ModuleType = '" + strModuleType + "'";
                            strHQL += " Where ModuleName = '" + strModuleName + "' and ModuleType = '" + strOldModuleType + "' and UserType = '" + strUserType + "'";
                            ShareClass.RunSqlCommand(strHQL);

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
                        return;
                    }
                }


                if (strCommandName == "DELETE")
                {
                    if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
                    {
                        strHQL = "Update T_ProModuleLevel Set IsDeleted = 'YES',Visible ='NO' ";
                        strHQL += " Where ModuleName = '" + strModuleName + "' and ParentModule = '" + strParentModule + "' and ModuleType = '" + strModuleType + "'";
                        ShareClass.RunSqlCommand(strHQL);

                        e.Item.Cells[8].Text = "YES";
                        e.Item.Cells[6].Text = "NO";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
                        return;
                    }
                }

                if (strCommandName == "CancelDelete")
                {
                    if (strPassword == "TAKETOPZXCZXJXYYZLY@#!")
                    {
                        strHQL = "Update T_ProModuleLevel Set IsDeleted = 'NO',Visible ='YES' ";
                        strHQL += " Where ModuleName = '" + strModuleName + "' and ParentModule = '" + strParentModule + "' and ModuleType = '" + strModuleType + "'";
                        ShareClass.RunSqlCommand(strHQL);

                        e.Item.Cells[8].Text = "NO";
                        e.Item.Cells[6].Text = "YES";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYCWJC").ToString().Trim() + "')", true);
                        return;
                    }
                }

                //设置缓存更改标志，并刷新页面缓存
                ChangePageCache();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    //更新左边样模组
    protected void UpdateLeftBarModules()
    {
        string strHQL1;
        string strModuleName, strParentModule, strLangCode, strHomeModuleName, strPageName, strModuleType, strUserType, strVisible, strIsDeleted, strSortNUmber, strIconURL;

        DataSet ds1;
        DataTable dt1;

        dt1 = new DataTable();

        string strpath = Server.MapPath("UpdateCode\\Modules.xls");
        dt1 = MSExcelHandler.ReadExcelToDataTable(strpath, "");

        DataRow[] dr = dt1.Select();  //定义一个DataRow数组
        int rowsnum = dt1.Rows.Count;
        if (rowsnum == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
        }
        else
        {
            for (int i = 0; i < dr.Length; i++)
            {
                strModuleName = dr[i]["ModuleName"].ToString().Trim();
                strParentModule = dr[i]["ParentModule"].ToString().Trim();
                strLangCode = dr[i]["LangCode"].ToString().Trim();
                strHomeModuleName = dr[i]["HomeModuleName"].ToString().Trim();
                strPageName = dr[i]["PageName"].ToString().Trim();
                strModuleType = dr[i]["ModuleType"].ToString().Trim();
                strUserType = dr[i]["UserType"].ToString().Trim();
                strVisible = dr[i]["Visible"].ToString().Trim();
                strIsDeleted = dr[i]["IsDeleted"].ToString().Trim();
                strSortNUmber = dr[i]["SortNumber"].ToString().Trim();
                strIconURL = dr[i]["IconURL"].ToString().Trim();
                strIconURL = strIconURL.Replace("//", "/");
                strIconURL = strIconURL.Replace("\\", "/");

                strHQL1 = "Select * From T_ProModuleLevel Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and ModuleType = '" + strModuleType + "' and LangCode='" + strLangCode + "' and UserType = '" + strUserType + "'";
                ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_ProModuleLevel");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strHQL1 = "Update T_ProModuleLevel Set HomeModuleName = '" + strHomeModuleName.Replace("'", "").Replace("\"", "").Replace("\\", "") + "'" + ",IconURL = " + "'" + strIconURL + "'";
                    strHQL1 += " Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and LangCode='" + strLangCode + "' and ModuleType = '" + strModuleType + "' and UserType = '" + strUserType + "'";

                    ShareClass.RunSqlCommand(strHQL1);
                }
            }

            string strDefaultLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            strHQL1 = string.Format(@"UPDATE T_ProModuleLevel B
                        SET SortNumber = A.SortNumber
                        FROM T_ProModuleLevel A
                        WHERE A.ModuleName = B.ModuleName 
                        AND A.ModuleType = B.ModuleType 
                        AND A.UserType = B.UserType 
                        AND A.LangCode <> B.LangCode 
                        AND A.LangCode = '{0}'", strDefaultLangCode);
            ShareClass.RunSqlCommand(strHQL1);
        }
    }

    //更新页面模组
    protected void UpdatePageBarModules()
    {
        string strHQL1;
        string strModuleName, strParentModule, strLangCode, strHomeModuleName, strPageName, strModuleType, strUserType, strVisible, strIsDeleted, strSortNUmber, strIconURL;

        DataSet ds1;

        string strpath = Server.MapPath("UpdateCode\\PageModules.xls");

        DataTable dt1;
        dt1 = new DataTable();
        dt1 = MSExcelHandler.ReadExcelToDataTable(strpath, "");
        DataRow[] dr = dt1.Select();  //定义一个DataRow数组
        int rowsnum = dt1.Rows.Count;

        if (rowsnum == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
        }
        else
        {
            for (int i = 0; i < dr.Length; i++)
            {
                strModuleName = dr[i]["ModuleName"].ToString().Trim();
                strParentModule = dr[i]["ParentModule"].ToString().Trim();
                strLangCode = dr[i]["LangCode"].ToString().Trim();
                strHomeModuleName = dr[i]["HomeModuleName"].ToString().Trim();
                strPageName = dr[i]["PageName"].ToString().Trim();
                strModuleType = dr[i]["ModuleType"].ToString().Trim();
                strUserType = dr[i]["UserType"].ToString().Trim();
                strVisible = dr[i]["Visible"].ToString().Trim();
                strIsDeleted = dr[i]["IsDeleted"].ToString().Trim();
                strSortNUmber = dr[i]["SortNumber"].ToString().Trim();
                strIconURL = dr[i]["IconURL"].ToString().Trim();
                strIconURL = strIconURL.Replace("//", "/");
                strIconURL = strIconURL.Replace("\\", "/");


                strHQL1 = "Select * From T_ProModuleLevelForPage  Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and ModuleType = '" + strModuleType + "' and LangCode='" + strLangCode + "' and UserType = '" + strUserType + "'";
                ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_ProModuleLevel");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strHQL1 = "Update T_ProModuleLevelForPage Set HomeModuleName = '" + strHomeModuleName.Replace("'", "").Replace("\"", "").Replace("\\", "") + "'" + ",IconURL = " + "'" + strIconURL + "'";
                    strHQL1 += " Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and LangCode='" + strLangCode + "' and ModuleType = '" + strModuleType + "' and UserType = " + "'" + strUserType + "'";

                    ShareClass.RunSqlCommand(strHQL1);
                }
            }

            string strDefaultLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            strHQL1 = string.Format(@"UPDATE T_ProModuleLevelForPage B
                        SET SortNumber = A.SortNumber
                        FROM T_ProModuleLevelForPage A
                        WHERE A.ModuleName = B.ModuleName 
                        AND A.ModuleType = B.ModuleType 
                        AND A.UserType = B.UserType 
                        AND A.LangCode <> B.LangCode 
                        AND A.LangCode = '{0}'", strDefaultLangCode);
            ShareClass.RunSqlCommand(strHQL1);
        }
    }


    //复制所有左边栏模组
    protected void CopyAllLeftModuleForHomeLanguage()
    {
        string strHQL, strLangHQL;

        string strModuleName, strModuleType, strUserType;
        string strFromLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];

        strUserType = DL_ForUserType.SelectedValue.Trim();
        strModuleType = GetLeftModuleType("0");
        strModuleName = GetLeftModuleName("0");

        strLangHQL = "Select LangCode From T_SystemLanguage Where LangCode <> " + "'" + strFromLangCode + "'";
        strLangHQL += " Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strLangHQL, "T_SystemLanguage");


        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strLangCode = ds.Tables[0].Rows[i][0].ToString().Trim();

            strHQL = "Insert Into T_ProModuleLevel(ModuleName,ParentModule,SortNumber,PageName ,ModuleType ,UserType ,Visible,LangCode,HomeModuleName,IsDeleted,IconURL,ModuleDefinition)";
            strHQL += " SELECT ModuleName,ParentModule,SortNumber,PageName ,ModuleType ,UserType ,Visible," + "'" + strLangCode + "'" + ",HomeModuleName,IsDeleted,IconURL,ModuleDefinition FROM T_ProModuleLevel";
            strHQL += " Where LangCode = '" + strFromLangCode + "' and trim(ModuleName) || trim(ParentModule) || trim(ModuleType) || trim(UserType)  Not in (Select rtrim(ModuleName) || trim(ParentModule) || trim(ModuleType) || trim(UserType) From T_ProModuleLevel Where LangCode = " + "'" + strLangCode + "'" + ")";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_ProModuleLevel B Set SortNumber = A.SortNumber From T_ProModuleLevel A Where A.ModuleName = B.ModuleName and A.LangCode = '" + strFromLangCode + "' AND B.LangCode =" + "'" + strLangCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ProModuleLevel Where LangCode = " + "'" + strLangCode + "'" + " and ModuleType in ('SYSTEM','APP')";
            strHQL += " and trim(ModuleName) || trim(ParentModule) || trim(ModuleType) || trim(UserType)  Not in (Select trim(ModuleName) || trim(ParentModule) || trim(ModuleType) || trim(UserType) From T_ProModuleLevel Where LangCode = '" + strFromLangCode + "')";
            ShareClass.RunSqlCommand(strHQL);
        }

        //设置缓存更改标志，并刷新页面缓存
        ChangePageCache();

        LoadChildModule("", strModuleType, strUserType, strLangCode);

        InitialModuleTree(strUserType, ddlLangSwitcher.SelectedValue.Trim());
    }


    //复制所有页面模组
    protected void CopyAllPageModuleForHomeLanguage()
    {
        string strHQL, strLangHQL;

        string strModuleName, strModuleType, strUserType;
        string strFromLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];

        strUserType = DL_ForUserType.SelectedValue.Trim();
        strModuleType = GetPageModuleType("0");
        strModuleName = GetPageModuleName("0");

        strLangHQL = "Select LangCode From T_SystemLanguage Where LangCode <> " + "'" + strFromLangCode + "'";
        strLangHQL += " Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strLangHQL, "T_SystemLanguage");

        try
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strLangCode = ds.Tables[0].Rows[i][0].ToString().Trim();

                strHQL = "Insert Into T_ProModuleLevelForPage(ModuleName,ParentModule,SortNumber,PageName ,ModuleType ,UserType ,Visible,LangCode,HomeModuleName,IsDeleted,IconURL)";
                strHQL += " SELECT ModuleName,ParentModule,SortNumber,PageName ,ModuleType ,UserType ,Visible," + "'" + strLangCode + "'" + ",HomeModuleName,IsDeleted,IconURL FROM T_ProModuleLevelForPage";
                strHQL += " Where LangCode = '" + strFromLangCode + "' and ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) || ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType))  Not in (Select ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) || ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType)) From T_ProModuleLevelForPage Where LangCode = " + "'" + strLangCode + "'" + ")";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Update T_ProModuleLevelForPage B Set SortNumber = A.SortNumber,Visible = A.Visible,IsDeleted = A.IsDeleted From T_ProModuleLevelForPage A Where A.ModuleName = B.ModuleName and A.ParentModule = B.ParentModule and A.LangCode = '" + strFromLangCode + "' AND B.LangCode =" + "'" + strLangCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_ProModuleLevelForPage Where LangCode = " + "'" + strLangCode + "'" + " and ModuleType in ('SYSTEM','APP')";
                strHQL += " and ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) || ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType))  Not in (Select ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) || ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType)) From T_ProModuleLevelForPage Where LangCode = '" + strFromLangCode + "')";
                ShareClass.RunSqlCommand(strHQL);
            }
        }
        catch (Exception ex)
        {
            LogClass.WriteLogFile(ex.Message.ToString());
        }
    }

    private void CreateLeftBarModuleExcel(string fileName)
    {
        string strHQL;

        strHQL = "Select ModuleName,ParentModule,PageName,ModuleType,LangCode,cast(HomeModuleName as varchar(2000)) as HomeModuleName,UserType,Visible,IsDeleted,SortNumber,IconURL,ModuleDefinition From T_ProModuleLevel";
        strHQL += " Order By LangCode ASC,HomeModuleName ASC";

        MSExcelHandler.DataTableToExcel(strHQL, fileName);
    }

    private void CreatePageBarModuleExcel(string fileName)
    {
        string strHQL;

        strHQL = "Select ModuleName,ParentModule,PageName,ModuleType,LangCode,cast(HomeModuleName as varchar(2000)) as HomeModuleName,UserType,Visible,IsDeleted,SortNumber,IconURL From T_ProModuleLevelForPage";
        strHQL += " Order By LangCode ASC,HomeModuleName ASC";

        MSExcelHandler.DataTableToExcel(strHQL, fileName);
    }

    protected void LoadChildModule(string strParentModuleName, string strModuleType, string strUserType, string strLangCode)
    {
        string strHQL;
        string strModuleName, strPageName;

        strLangCode = ddlLangSwitcher.SelectedValue.Trim();

        if (strModuleType != "APP")
        {
            strHQL = " select ID,ParentModule, ModuleName,PageName,SortNumber,ModuleType,UserType,Visible,HomeModuleName,LangCode,IsDeleted,IconURL from T_ProModuleLevel ";
            strHQL += " where UserType = " + "'" + strUserType + "'" + " and ltrim(rtrim(ParentModule)) = " + "'" + strParentModuleName + "'";
            strHQL += " and ModuleType <> 'APP'";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order By ModuleType DESC";
        }
        else
        {
            strHQL = " select ID,ParentModule, ModuleName,PageName,SortNumber,ModuleType,UserType,Visible,HomeModuleName,LangCode,IsDeleted,IconURL from T_ProModuleLevel ";
            strHQL += " where UserType = " + "'" + strUserType + "'" + " and ltrim(rtrim(ParentModule)) = " + "'" + strParentModuleName + "'";
            strHQL += " and ModuleType = 'APP'";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order By ModuleType DESC";
        }

        if (strParentModuleName == "")
        {
            strHQL = " select ID,ParentModule, ModuleName,PageName,SortNumber,ModuleType,UserType,Visible,HomeModuleName,LangCode,IsDeleted,IconURL from T_ProModuleLevel ";
            strHQL += " where UserType = " + "'" + strUserType + "'" + " and ltrim(rtrim(ParentModule)) = " + "'" + strParentModuleName + "'";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order By ModuleType DESC";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strModuleName = ds.Tables[0].Rows[i]["ModuleName"].ToString().Trim();
            strPageName = ds.Tables[0].Rows[i]["PageName"].ToString().Trim();
            strModuleType = ds.Tables[0].Rows[i]["ModuleType"].ToString().Trim();

            ((TextBox)DataGrid4.Items[i].FindControl("TB_PageName")).Text = strPageName;
            ((DropDownList)DataGrid4.Items[i].FindControl("DL_ModuleType")).SelectedValue = strModuleType.Trim();

            if (TakeTopSecurity.TakeTopLicense.IsCanNotDeletedModule(strModuleName))
            {
                ((Button)DataGrid4.Items[i].FindControl("BT_DeleteModule")).Enabled = false;
                ((Button)DataGrid4.Items[i].FindControl("BT_CancelDeleteModule")).Enabled = false;
            }
        }

        LB_ModuleNumber.Text = ds.Tables[0].Rows.Count.ToString();
    }

    protected string GetLeftModuleType(string strModuleID)
    {
        string strHQL = "Select ModuleType From T_ProModuleLevel Where ID = " + strModuleID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {

        ////设置缓存更改标志
        //ShareClass.SetPageCacheMark("1");
        //Session["CssDirectoryChangeNumber"] = "1";

        //更新页面缓存，刷新页面
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceModuleFlowView.aspx", "");
        ShareClass.AddSpaceLineToFile("TTModuleFlowChartViewJS.aspx", "");
        ShareClass.AddSpaceLineToFile("WFDesigner/TTTakeTopMFChartViewJS.aspx", "");
    }

    protected string GetLeftModuleUserType(string strModuleID)
    {
        string strHQL = "Select UserType From T_ProModuleLevel Where ID = " + strModuleID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    protected string GetLeftModulePageName(string strModuleID)
    {
        string strHQL;

        strHQL = "Select PageName From T_ProModuleLevel Where ID = " + strModuleID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    protected string GetLeftModuleName(string strModuleID)
    {
        string strHQL = "Select ModuleName From T_ProModuleLevel Where ID = " + strModuleID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    protected string GetPageModuleType(string strModuleID)
    {
        string strHQL = "Select ModuleType From T_ProModuleLevelForPage Where ID = " + strModuleID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    protected string GetPageModuleName(string strModuleID)
    {
        string strHQL = "Select ModuleName From T_ProModuleLevelForPage Where ID = " + strModuleID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }



}
