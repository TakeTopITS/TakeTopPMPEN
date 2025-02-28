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
using System.Data.OleDb;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;
using System.Runtime.CompilerServices;

public partial class TTSystemModuleSet : System.Web.UI.Page
{
    string strLangCode, strUserCode;
    string strForbitModule;

    protected  void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();

        strForbitModule = Session["ForbitModule"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "SystemModuleSettings", strUserCode);
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
            DL_WLType.Items.Insert(0, new ListItem("--Select--", "0"));
            LoadWorkFlowTemplate();

            LB_ParentModuleType.Text = "SYSTEM";

            ShareClass.LoadLanguageForDropList(ddlLangSwitcher);
            ddlLangSwitcher.SelectedValue = strLangCode;

            InitialModuleTree(TreeView1, "INNER", ddlLangSwitcher.SelectedValue.Trim());
            InitialModuleTree(TreeView2, "INNER", ddlLangSwitcher.SelectedValue.Trim());

            LB_UserAuthorizationRecordNumber.Text = GetUserAuthorizationRecordNumber().ToString();

            LoadReportType();
        }
    }

    protected void DL_ForUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strUserType;

        strUserType = DL_ForUserType.SelectedValue.Trim();

        InitialModuleTree(TreeView1, strUserType, ddlLangSwitcher.SelectedValue.Trim());
        InitialModuleTree(TreeView2, strUserType, ddlLangSwitcher.SelectedValue.Trim());
        InitialModuleTree(TreeView1, strUserType, ddlLangSwitcher.SelectedValue.Trim());
        InitialModuleTree(TreeView2, strUserType, ddlLangSwitcher.SelectedValue.Trim());
    }

 
    protected void BT_DeleteDoubleModule_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strModuleName, strModuleType, strUserType;

        strLangCode = ddlLangSwitcher.SelectedValue.Trim();
        strUserType = DL_ForUserType.SelectedValue.Trim();
        strModuleType = GetModuleType("0");
        strModuleName = GetModuleName("0");

        strHQL = "CALL Pro_DeleteDoubleModule()";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadChildModule("", strModuleType, strUserType, strLangCode);
            InitialModuleTree(TreeView1, strUserType, ddlLangSwitcher.SelectedValue.Trim());
            InitialModuleTree(TreeView2, strUserType, ddlLangSwitcher.SelectedValue.Trim());

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCWC").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteUserAuthorizationBySuperModule_Click(object sender, EventArgs e)
    {
        string strHQL;

        try
        {
            strHQL = "CALL Pro_DeleteUserAuthorizationBySuperModule()";
            ShareClass.RunSqlCommand(strHQL);

            LB_UserAuthorizationRecordNumber.Text = GetUserAuthorizationRecordNumber().ToString();

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCWC").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_ReOrderUserModuleAuthorization_Click(object sender, EventArgs e)
    {
        string strHQL;

        try
        {
            strHQL = "Call PR_DeleteNotActiveUserModuleAndReOrderUserModuleAuthorization()";
            ShareClass.RunSqlCommand(strHQL);

            LB_UserAuthorizationRecordNumber.Text = GetUserAuthorizationRecordNumber().ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZZWC").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_ModuleType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strFunType = DL_ModuleType.SelectedValue.Trim();

        if (strFunType == "DIYAPP" | strFunType == "DIYMO")
        {
            DL_WLType.Enabled = true;
            DL_DIYWFTemplate.Enabled = true;
        }
        else
        {
            DL_WLType.Enabled = false;
            DL_DIYWFTemplate.Enabled = false;
        }

        if (strFunType == "DIYAPP" | strFunType == "DIYMO")
        {
            DL_ReportType.Enabled = true;
            DL_Report.Enabled = true;
        }
        else
        {
            DL_ReportType.Enabled = false;
            DL_Report.Enabled = false;
        }
    }

    protected void DL_WLType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL, strWLType;


        strWLType = DL_WLType.SelectedValue.Trim();

        strHQL = "Select TemName,IdentifyString From T_WorkFlowTemplate Where type = " + "'" + strWLType + "'" + " and Authority = 'All'";
        strHQL += " Order by CreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_DIYWFTemplate.DataSource = ds;
        DL_DIYWFTemplate.DataBind();

        DL_DIYWFTemplate.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void DL_DIYWFTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strIdentifyString = DL_DIYWFTemplate.SelectedValue.Trim();
        string strModuleName = TB_ModuleName.Text.Trim();
        string strFunType = DL_ModuleType.SelectedValue.Trim();

        if (strFunType == "DIYAPP")
        {
            TB_PageName.Text = "TTAPPDIYSystemMain.aspx?TemIdentifyString=" + strIdentifyString;
        }
        if (strFunType == "DIYMO")
        {
            TB_PageName.Text = "TTDIYSystemMain.aspx?TemIdentifyString=" + strIdentifyString;
        }
    }

    protected void DL_ReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadReportByType(DL_ReportType.SelectedValue.Trim());
    }

    protected void DL_Report_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strFunType = DL_ModuleType.SelectedValue.Trim();

        if (strFunType == "DIYMO")
        {
            TB_PageName.Text = "TTReportView.aspx?ReportName=" + DL_Report.SelectedValue.Trim();
        }
    }

    protected void LoadReportType()
    {
        string strHQL;
        IList lst;

        strHQL = "from ReportType as reportType Where reportType.Type In (Select report.ReportType From Report as report)";
        strHQL += " Order By reportType.SortNumber ASC";
        ReportTypeBLL reportTypeBLL = new ReportTypeBLL();
        lst = reportTypeBLL.GetAllReportTypes(strHQL);

        DL_ReportType.DataSource = lst;
        DL_ReportType.DataBind();

        DL_ReportType.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadReportByType(string strReportType)
    {
        string strHQL;
        IList lst;

        strHQL = "From Report as report Where report.ReportType = " + "'" + strReportType + "'";
        strHQL += " Order By report.ID DESC";
        ReportBLL reportBLL = new ReportBLL();
        lst = reportBLL.GetAllReports(strHQL);

        DL_Report.DataSource = lst;
        DL_Report.DataBind();

        DL_Report.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strModuleID, strModuleName, strParentModuleName, strModuleType, strUserType;
        int intLevel;
        string strParentModuleID;

        strUserType = DL_ForUserType.SelectedValue.Trim();
        strLangCode = ddlLangSwitcher.SelectedValue.Trim();


        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        intLevel = treeNode.Depth;
        LB_Level.Text = intLevel.ToString();

        strModuleID = treeNode.Target.Trim();
        try
        {
            strParentModuleID = treeNode.Parent.Target.ToString();
        }
        catch
        {
            strParentModuleID = "0";
        }

        strModuleType = GetModuleType(strModuleID);
        strModuleName = GetModuleName(strModuleID);

        if (strModuleID == "0")
        {
            LB_ID.Text = "";
            TB_ParentModuleName.Text = "";

            TB_ModuleName.Text = "";
            TB_HomeModuleName.Text = "";

            TB_PageName.Text = "";
            NB_SortNumber.Amount = 1;

            IM_ModuleIcon.ImageUrl = "";

            TB_ModuleName.Enabled = true;

            BT_AddChildModule.Enabled = true;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            LB_SelectedModuleName.Text = "";

            LB_OldModuleName.Text = "";
            LB_OldModuleName.Text = "";
            LB_OldModuleType.Text = "";
            LB_OldParentModuleName.Text = "";

            DL_Visible.SelectedValue = "YES";

            LB_ParentModuleType.Text = "SYSTEM";

            HL_ImpleRute.Enabled = false;
            HL_ImpleRute.NavigateUrl = "";

            LoadChildModule("", strModuleType, strUserType, strLangCode);
        }
        else
        {
            strHQL = "Select ID,ParentModule,ModuleName,PageName,SortNumber,ModuleType,Visible,DIYFlow,HomeModuleName,IconURL,UserType From T_ProModuleLevel Where ID = " + strModuleID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

            LB_ID.Text = strModuleID;

            TB_ParentModuleName.Text = ds.Tables[0].Rows[0]["ParentModule"].ToString().Trim();
            strParentModuleName = ds.Tables[0].Rows[0]["ParentModule"].ToString().Trim();

            TB_ModuleName.Text = ds.Tables[0].Rows[0]["ModuleName"].ToString().Trim();
            strModuleName = ds.Tables[0].Rows[0]["ModuleName"].ToString().Trim();

            TB_HomeModuleName.Text = ds.Tables[0].Rows[0]["HomeModuleName"].ToString().Trim();

            TB_PageName.Text = ds.Tables[0].Rows[0]["PageName"].ToString();
            NB_SortNumber.Amount = int.Parse(ds.Tables[0].Rows[0]["SortNumber"].ToString());

            try
            {
                DL_ModuleType.SelectedValue = ds.Tables[0].Rows[0]["ModuleType"].ToString().Trim();
            }
            catch
            {
            }
            strModuleType = ds.Tables[0].Rows[0]["ModuleType"].ToString().Trim();

            BT_UploadModuleIcon.Enabled = true;
            BT_DeleteModuleIcon.Enabled = true;
            IM_ModuleIcon.ImageUrl = ds.Tables[0].Rows[0]["iconurl"].ToString().Trim();

            LB_SelectedModuleName.Text = strModuleName;
            LB_OldModuleName.Text = strModuleName;
            LB_OldModuleType.Text = strModuleType;
            LB_OldParentModuleName.Text = strParentModuleName;
            DL_Visible.SelectedValue = ds.Tables[0].Rows[0]["Visible"].ToString().Trim();
            DL_IsDIYFlow.SelectedValue = ds.Tables[0].Rows[0]["DIYFlow"].ToString().Trim();

            LB_ParentModuleType.Text = GetModuleType(strParentModuleID);

            HL_ImpleRute.Enabled = true;
            HL_ImpleRute.NavigateUrl = "TTModuleFlowDesignerJS.aspx?Type=SystemModule&IdentifyString=" + strModuleID;

            if (strModuleType == "SYSTEM" | strModuleType == "APP")
            {
                TB_ModuleName.Enabled = true;

                TB_PageName.Enabled = true;
                DL_ModuleType.Enabled = true;
                DL_WLType.Enabled = true;
                DL_DIYWFTemplate.Enabled = true;


                BT_Update.Enabled = true;
                BT_Delete.Enabled = false;
            }
            else
            {
                TB_ModuleName.Enabled = true;

                TB_PageName.Enabled = true;
                DL_ModuleType.Enabled = true;
                DL_WLType.Enabled = true;
                DL_DIYWFTemplate.Enabled = true;

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }

            if (strModuleType == "APP")
            {
                BT_AddChildModule.Enabled = true;
                //DL_ModuleType.Enabled = false;
                DL_ModuleType.SelectedValue = "APP";
            }

            if (strModuleType == "SITE")
            {
                BT_AddChildModule.Enabled = true;
                DL_ModuleType.Enabled = false;
                DL_ModuleType.SelectedValue = "SITE";
            }

            if (intLevel == 3 & strModuleType != "SITE")
            {
                BT_AddChildModule.Enabled = false;
            }
            else
            {
                BT_AddChildModule.Enabled = true;
            }

            if (TakeTopSecurity.TakeTopLicense.IsCanNotDeletedModule(strModuleName))
            {
                BT_Delete.Enabled = false;
            }

            LoadChildModule(strModuleName, strModuleType, strUserType, strLangCode);
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strModuleID, strModuleName, strNewModuleType, strSelectModuleType;
        int intLevel;


        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;
        intLevel = treeNode.Depth;

        LB_Level.Text = intLevel.ToString();
        strModuleID = treeNode.Target.Trim();

        strNewModuleType = DL_ModuleType.SelectedValue.Trim();
        strSelectModuleType = GetModuleType(strModuleID);

        if (intLevel > 2 & strNewModuleType != "SITE")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDSCBNZJZMZQJC").ToString().Trim() + "')", true);
            return;
        }

        if (strNewModuleType != strSelectModuleType & strNewModuleType == "SITE")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWZMZZNFZMZMZZXQJC").ToString().Trim() + "')", true);
            return;
        }

        strModuleName = GetModuleName(strModuleID);
        TB_ParentModuleName.Text = strModuleName;
        LB_ParentModuleType.Text = strSelectModuleType;

        HL_ImpleRute.Enabled = true;
        HL_ImpleRute.NavigateUrl = "TTModuleFlowDesignerJS.aspx?IdentifyString=" + strModuleID;
    }

    protected void ddlLangSwitcher_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strModuleName, strModuleType, strUserType;

        strLangCode = ddlLangSwitcher.SelectedValue.Trim();

        strUserType = DL_ForUserType.SelectedValue.Trim();
        strModuleType = GetModuleType("0");
        strModuleName = GetModuleName("0");

        InitialModuleTree(TreeView1, strUserType, ddlLangSwitcher.SelectedValue.Trim());
        InitialModuleTree(TreeView2, strUserType, ddlLangSwitcher.SelectedValue.Trim());

        LoadChildModule("", strModuleType, strUserType, strLangCode);
    }


    protected void BT_UploadModuleIcon_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strParentModule, strModuleName, strModuleType, strUserType;
            string strHQL;


            strUserType = DL_ForUserType.SelectedValue.Trim();
            try
            {
                strParentModule = TreeView1.SelectedNode.Text.Trim();
            }
            catch
            {
                strParentModule = TB_ParentModuleName.Text.Trim();
            }
            strModuleName = TB_ModuleName.Text.Trim();
            strModuleType = DL_ModuleType.SelectedValue.Trim();

            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strLoginUserCode = Session["UserCode"].ToString().Trim();

            int i;


            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + strExtName;


                string strDocSavePath = Server.MapPath("ImagesSkin") + "\\";
                string strFileName3 = "ImagesSkin/" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }
                try
                {
                    FUP_File.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_ProModuleLevel Set IconURL = " + "'" + strFileName3 + "'" + " Where ModuleName = " + "'" + strModuleName + "'";
                    strHQL += " and ModuleType = " + "'" + strModuleType + "'" + " and UserType = " + "'" + strUserType + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_ModuleIcon.ImageUrl = strFileName3;

                    LoadChildModule(strParentModule, strModuleType, strUserType, strLangCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCHCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteModuleIcon_Click(object sender, EventArgs e)
    {
        string strParentModule, strModuleName, strModuleType, strUserType;
        string strHQL;


        strUserType = DL_ForUserType.SelectedValue.Trim();
        try
        {
            strParentModule = TreeView1.SelectedNode.Text.Trim();
        }
        catch
        {
            strParentModule = TB_ParentModuleName.Text.Trim();
        }
        strModuleName = TB_ModuleName.Text.Trim();
        strModuleType = DL_ModuleType.SelectedValue.Trim();

        string strFileName1 = FUP_File.PostedFile.FileName.Trim();
        string strLoginUserCode = Session["UserCode"].ToString().Trim();

        try
        {
            strHQL = "Update T_ProModuleLevel Set IconURL = '' Where ModuleName = " + "'" + strModuleName + "'";
            strHQL += " and ModuleType = " + "'" + strModuleType + "'" + " and UserType = " + "'" + strUserType + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_ModuleIcon.ImageUrl = "";

            LoadChildModule(strParentModule, strModuleType, strUserType, strLangCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_AddChildModule_Click(object sender, EventArgs e)
    {
        string strID, strParentModuleName, strModuleName, strInnerModuleName, strHomeModuleName, strPageName, strSortNumber, strModuleType, strOldModuleName, strUserType, strVisible, strIsDIYFlow;
        string strHQL;

        strID = LB_ID.Text.Trim();

        strModuleName = TB_ModuleName.Text.Trim();
        strHomeModuleName = TB_HomeModuleName.Text.Trim();

        strLangCode = ddlLangSwitcher.SelectedValue.Trim();

        if (LB_ParentModuleType.Text == "APP" | LB_ParentModuleType.Text == "DIYAPP")
        {
            DL_ModuleType.SelectedValue = "DIYAPP";
        }

        strPageName = TB_PageName.Text.Trim();
        strSortNumber = NB_SortNumber.Amount.ToString();
        strModuleType = DL_ModuleType.SelectedValue.Trim();
        strUserType = DL_ForUserType.SelectedValue.Trim();
        strVisible = DL_Visible.SelectedValue.Trim();
        strIsDIYFlow = DL_IsDIYFlow.SelectedValue.Trim();

        strOldModuleName = LB_OldModuleName.Text.Trim();
        strParentModuleName = TB_ParentModuleName.Text.Trim();

        if (strModuleName.IndexOf("NONE") >= 0 | strForbitModule.IndexOf(strModuleName + ",") >= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJGMZMCBNWNONEBQBNDJYMZMCQDNQJC").ToString().Trim() + "')", true);
            return;
        }

        if (strModuleType == "SYSTEM" | strModuleType == "APP")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNZXZZDYLXDMZJC").ToString().Trim() + "')", true);
            return;
        }

        if (GetExistAllSameModuleNumber(strModuleName, strUserType, "") > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJGCZXTMCXTMZLXXTYHLXDMZBNCFZJ").ToString().Trim() + "')", true);
            return;
        }

        if (strModuleName == "DIYMO" | strModuleName == "DIYAPP")
        {
            strInnerModuleName = GetModuleNameByPageName(strPageName);
            if (strInnerModuleName != "")
            {
                strModuleName = strInnerModuleName;
                TB_ModuleName.Text = strModuleName;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCYMCZNZMZXMZZNKYSBMM").ToString().Trim() + "')", true);
            }
        }

        if (GetChildModuleNumber(strModuleName, strParentModuleName, strModuleType, strUserType, strLangCode) > 0 | strModuleName == strParentModuleName)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBNBZMZSWHMZDHMZQJC").ToString().Trim() + "')", true);
            return;
        }

        if (strModuleName == strOldModuleName)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNBNZJHFMZMCXTDMZJC").ToString().Trim() + "')", true);

            return;
        }

        try
        {
            if (GetTopProModuleCount(strModuleName, strModuleType) > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZXTMCYJMZZJSBJC").ToString().Trim() + "')", true);
                return;
            }

            strHQL = "Insert Into T_ProModuleLevel(ParentModule,ModuleName,HomeModuleName,PageName,SortNumber,ModuleType,UserType,Visible,DIYFlow,LangCode,IsDeleted,ModuleDefinition)";
            strHQL += " Values (" + "'" + strParentModuleName + "'" + "," + "'" + strModuleName + "'" + "," + "'" + strHomeModuleName + "'" + "," + "'" + strPageName + "'" + "," + strSortNumber + "," + "'" + strModuleType + "'" + ",'" + strUserType + "','" + strVisible + "','" + strIsDIYFlow + "','" + strLangCode + "','NO','')";
            ShareClass.RunSqlCommand(strHQL);

            LB_ID.Text = ShareClass.GetMyCreatedMaxModuleID();
            LB_OldModuleName.Text = strModuleName;

            if (ShareClass.IsExistModuleByUserCode("SAMPLE", strModuleName, strModuleType, strUserType) == false)
            {
                strHQL = "Insert Into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType) Select ModuleName,'SAMPLE','" + strVisible + "',ModuleType,UserType From T_ProModuleLevel Where ID = " + LB_ID.Text;
                ShareClass.RunSqlCommand(strHQL);
            }

            if (ShareClass.IsExistModuleByUserCode("ADMIN", strModuleName, strModuleType, strUserType) == false)
            {
                strHQL = "Insert Into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType) Select ModuleName,'ADMIN','YES',ModuleType,UserType From T_ProModuleLevel Where ID = " + LB_ID.Text;
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadChildModule(strParentModuleName, strModuleType, strUserType, strLangCode);

            BT_Delete.Enabled = false;
            BT_Update.Enabled = false;

            BT_UploadModuleIcon.Enabled = false;
            BT_DeleteModuleIcon.Enabled = false;


            InitialModuleTree(TreeView1, strUserType, ddlLangSwitcher.SelectedValue.Trim());
            InitialModuleTree(TreeView2, strUserType, ddlLangSwitcher.SelectedValue.Trim());

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strID, strParentModule, strModuleName, strInnerModuleName, strHomeModuleName, strPageName, strSortNumber, strOldModuleName, strModuleType, strOldModuleType, strUserType, strVisible, strIsDIYFlow;
        string strParentModuleName, strOldParentModuleName;
        string strHQL;

        strID = LB_ID.Text.Trim();

        strUserType = DL_ForUserType.SelectedValue.Trim();

        try
        {
            strParentModule = TreeView1.SelectedNode.Text.Trim();
        }
        catch
        {
            strParentModule = TB_ParentModuleName.Text.Trim();
        }

        if (LB_ParentModuleType.Text == "APP" | LB_ParentModuleType.Text == "DIYAPP")
        {
            DL_ModuleType.SelectedValue = "DIYAPP";
        }

        strParentModuleName = TB_ParentModuleName.Text.Trim();

        strModuleName = TB_ModuleName.Text.Trim();
        strHomeModuleName = TB_HomeModuleName.Text.Trim();

        strPageName = TB_PageName.Text.Trim();
        strSortNumber = NB_SortNumber.Amount.ToString();
        strModuleType = DL_ModuleType.SelectedValue.Trim();

        strOldModuleName = LB_OldModuleName.Text.Trim();
        strOldParentModuleName = LB_OldParentModuleName.Text.Trim();
        strOldModuleType = LB_OldModuleType.Text.Trim();
        strVisible = DL_Visible.SelectedValue.Trim();
        strIsDIYFlow = DL_IsDIYFlow.SelectedValue.Trim();

        string strForbitModule = Session["ForbitModule"].ToString();
        if (strModuleName.IndexOf("NONE") >= 0 | strForbitModule.IndexOf(strModuleName + ",") >= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJGMZMCBNWNONEBQBNDJYMZMCQDNQJC").ToString().Trim() + "')", true);
            return;
        }

        if (strModuleName != strOldModuleName & GetExistAllSameModuleNumber(strModuleName, strUserType, strID) > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJGCZXTMCXTMZLXXTYHLXDMZBNCFZJ").ToString().Trim() + "')", true);
            return;
        }

        if (strModuleName == "DIYMO" | strModuleName == "DIYAPP")
        {
            strInnerModuleName = GetModuleNameByPageName(strPageName);
            if (strInnerModuleName != "")
            {
                strModuleName = strInnerModuleName;
                TB_ModuleName.Text = strModuleName;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCYMCZNZMZXMZZNKYSBMM").ToString().Trim() + "')", true);
            }
        }

        if (GetChildModuleNumber(strModuleName, strParentModuleName, strModuleType, strUserType, strLangCode) > 0 | strModuleName == strParentModuleName)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBNBZMZSWHMZDHMZQJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            if (strOldModuleType == "SYSTEM" | strOldModuleType == "APP")
            {
                strHQL = "Update T_ProModuleLevel Set ParentModule = " + "'" + strParentModuleName + "'" + ",HomeModuleName=" + "'" + strHomeModuleName + "'" + ",PageName=" + "'" + strPageName + "'" + ", SortNumber=" + strSortNumber + ",Visible=" + "'" + strVisible + "',DIYFlow='" + strIsDIYFlow + "'";
                strHQL += " Where ModuleName = " + "'" + strOldModuleName + "'" + " and ParentModule = " + "'" + strOldParentModuleName + "'" + " and ModuleType = " + "'" + strOldModuleType + "'" + " and UserType = " + "'" + strUserType + "'";
                strHQL += " and ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Update T_ProModuleLevel Set DIYFlow='" + strIsDIYFlow + "'";
                strHQL += " Where ModuleName = " + "'" + strOldModuleName + "'" + " and ParentModule = " + "'" + strOldParentModuleName + "'" + " and ModuleType = " + "'" + strOldModuleType + "'" + " and UserType = " + "'" + strUserType + "'";
                ShareClass.RunSqlCommand(strHQL);
            }
            else
            {
                strHQL = "Update T_ProModuleLevel Set ParentModule = " + "'" + strParentModuleName + "'" + ", ModuleName = " + "'" + strModuleName + "'" + ",HomeModuleName=" + "'" + strHomeModuleName + "'" + ",PageName=" + "'" + strPageName + "'" + ",SortNumber=" + strSortNumber + ",Visible=" + "'" + strVisible + "',DIYFlow='" + strIsDIYFlow + "'";
                strHQL += " Where ModuleName = " + "'" + strOldModuleName + "'" + " and ParentModule = " + "'" + strOldParentModuleName + "'" + " and ModuleType = " + "'" + strOldModuleType + "'" + " and UserType = " + "'" + strUserType + "'";
                strHQL += " and ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Update T_ProModuleLevel Set DIYFlow='" + strIsDIYFlow + "'";
                strHQL += " Where ModuleName = " + "'" + strOldModuleName + "'" + " and ParentModule = " + "'" + strOldParentModuleName + "'" + " and ModuleType = " + "'" + strOldModuleType + "'" + " and UserType = " + "'" + strUserType + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Update T_ProModule Set ModuleName = " + "'" + strModuleName + "'";
                strHQL += " Where ModuleName = " + "'" + strOldModuleName + "'" + " and ModuleType = " + "'" + strOldModuleType + "'" + " and UserType = " + "'" + strUserType + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadChildModule(strParentModule, strModuleType, strUserType, strLangCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            InitialModuleTree(TreeView1, strUserType, ddlLangSwitcher.SelectedValue.Trim());
            InitialModuleTree(TreeView2, strUserType, ddlLangSwitcher.SelectedValue.Trim());

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID, strParentModule, strModuleName, strPageName, strSortNumber, strModuleType, strUserType;
        string strHQL;

        strID = LB_ID.Text.Trim();

        strParentModule = TB_ParentModuleName.Text.Trim();
        strModuleName = TB_ModuleName.Text.Trim();
        strPageName = TB_PageName.Text.Trim();
        strSortNumber = NB_SortNumber.Amount.ToString();
        strModuleType = DL_ModuleType.SelectedValue.Trim();
        strUserType = DL_ForUserType.SelectedValue.Trim();

        try
        {
            strHQL = "Delete From T_ProModuleLevel where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ProModule Where rtrim(ModuleName) || rtrim(ModuleType) || rtrim(UserType) = '" + strModuleName + strModuleType + strUserType + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadChildModule(strParentModule, strModuleType, strUserType, strLangCode);

            BT_AddChildModule.Enabled = false;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            BT_UploadModuleIcon.Enabled = false;
            BT_DeleteModuleIcon.Enabled = false;

            InitialModuleTree(TreeView1, strUserType, ddlLangSwitcher.SelectedValue.Trim());
            InitialModuleTree(TreeView2, strUserType, ddlLangSwitcher.SelectedValue.Trim());

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strModuleID, strModuleName, strModuleType, strUserType;
            string strCommandName;

            strModuleID = e.Item.Cells[0].Text.Trim();
            strModuleName = e.Item.Cells[1].Text.Trim();
            strModuleType = e.Item.Cells[5].Text.Trim();
            strUserType = e.Item.Cells[12].Text.Trim();
            strCommandName = e.CommandName.ToString().Trim();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            try
            {
                if (strCommandName == "ADMIN")
                {
                    if (ShareClass.IsExistModuleByUserCode("ADMIN", strModuleName, strModuleType, strUserType) == false)
                    {
                        strHQL = "Insert Into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType) Select ModuleName,'ADMIN','YES',ModuleType,UserType From T_ProModuleLevel Where ID = " + strModuleID;
                        ShareClass.RunSqlCommand(strHQL);
                    }
                }

                if (strCommandName == "SAMPLE")
                {
                    string strVisible = ((DropDownList)(e.Item.FindControl("DL_SetVisible"))).SelectedValue.Trim();

                    if (ShareClass.IsExistModuleByUserCode("SAMPLE", strModuleName, strModuleType, strUserType) == false)
                    {
                        strHQL = "Insert Into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType) Select ModuleName,'SAMPLE','" + strVisible + "',ModuleType,UserType From T_ProModuleLevel Where ID = " + strModuleID;
                        ShareClass.RunSqlCommand(strHQL);

                        strHQL = "Insert Into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType) Select ModuleName,'" + strUserCode + "','YES',ModuleType,UserType From T_ProModuleLevel Where ID = " + strModuleID;
                        ShareClass.RunSqlCommand(strHQL);
                    }
                    else
                    {
                        strHQL = String.Format(@"Update T_ProModule Set Visible = '{0}' Where ModuleName = '{1}'", strVisible, strModuleName);
                        ShareClass.RunSqlCommand(strHQL);
                    }
                }

                strHQL = "CALL pro_deletedoublemodule()";
                ShareClass.RunSqlCommand(strHQL);

                //设置缓存更改标志，并刷新页面缓存
                ChangePageCache();

                //更新页面缓存，刷新页面
                ShareClass.AddSpaceLineToFile("TTModuleTreeSelectPage.aspx", "<%--***--%>");

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSQCG").ToString().Trim() + "')", true);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_ModuleSave_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strModuleID, strUserType, strModuleType, strModuleName, strHomeModuleName, strVisible,strIsDIYFlow;

        int j = 0, intSortNumber;

        strModuleID = LB_ID.Text.Trim();
        strUserType = DL_ForUserType.SelectedValue.Trim();
        strModuleType = DL_ModuleType.SelectedValue.Trim();

        try
        {
            for (j = 0; j < DataGrid4.Items.Count; j++)
            {
                strID = DataGrid4.Items[j].Cells[0].Text;

                intSortNumber = int.Parse(((TextBox)(DataGrid4.Items[j].FindControl("TB_SortNumber"))).Text.Trim());

                strHQL = "Update T_ProModuleLevel Set SortNumber = " + intSortNumber.ToString() + " Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                strHomeModuleName = ((TextBox)DataGrid4.Items[j].FindControl("TB_HomeModuleName")).Text;
                strHQL = "Update T_ProModuleLevel Set HomeModuleName = " + "'" + strHomeModuleName + "'" + " Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                if (((CheckBox)DataGrid4.Items[j].FindControl("CB_ModuleVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }

                if (((CheckBox)DataGrid4.Items[j].FindControl("CB_IsDIYFlow")).Checked)
                {
                    strIsDIYFlow = "YES";
                }
                else
                {
                    strIsDIYFlow = "NO";
                }

                strHQL = "Update T_ProModuleLevel Set Visible = " + "'" + strVisible + "',DIYFlow ='" + strIsDIYFlow + "' Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);
            }

            if (strModuleID == "0")
            {
                LoadChildModule("", strModuleType, strUserType, strLangCode);
            }
            else
            {
                LoadChildModule(TB_ModuleName.Text.Trim(), strModuleType, strUserType, strLangCode);
            }

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void InitialModuleTree(TreeView treeView, string strUserType, string strLangCode)
    {
        string strHQL;
        string strModuleID, strModuleName, strModuleType, strHomeModuleName;

        //添加根节点
        treeView.Nodes.Clear();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();

        node1.Text = "<b>" + LanguageHandle.GetWord("XiTongMoZu").ToString().Trim() + "<b>";
        node1.Target = "0";
        node1.Expanded = true;
        treeView.Nodes.Add(node1);

        strHQL = "Select ID,ModuleName,HomeModuleName,ModuleType From T_ProModuleLevel Where UserType = " + "'" + strUserType + "'" + " and char_length(ParentModule) = 0 ";
        strHQL += " and position('" + strForbitModule + "' in rtrim(ModuleName)) = 0";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " and IsDeleted = 'NO'";
        strHQL += " Order By ModuleType DESC,SortNumber ASC";
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strModuleID = ds1.Tables[0].Rows[i][0].ToString();
            strModuleName = ds1.Tables[0].Rows[i][1].ToString();
            strHomeModuleName = ds1.Tables[0].Rows[i][2].ToString();
            strModuleType = ds1.Tables[0].Rows[i][3].ToString().Trim();

            if (strForbitModule.IndexOf(strModuleName + ",") >= 0)
            {
                continue;
            }

            node2 = new TreeNode();

            node2.Text = (i + 1).ToString() + "." + strHomeModuleName;
            node2.Target = strModuleID;
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);


            ModuleTreeShow(strUserType, strModuleType, strModuleName, node2, strLangCode);


            treeView.DataBind();
        }
    }

    public void ModuleTreeShow(string strParentUserType, string strParentModuleType, string strParentModule, TreeNode treeNode, string strLangCode)
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

        strHQL += " and position('" + strForbitModule + "' in rtrim(ModuleName)) = 0";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " and IsDeleted = 'NO'";
        strHQL += " Order By ModuleType DESC,SortNumber ASC";
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

    protected string GetModuleNameByPageName(string strPageName)
    {
        string strHQL;

        strHQL = "Select PageName From T_ProModuleLevel Where PageName = " + "'" + strPageName + "'";
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

    protected int GetChildModuleNumber(string strParentModule, string strModuleName, string strModuleType, string strUserType, string strLangCode)
    {
        string strHQL;

        strHQL = "Select * From T_ProModuleLevel Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and ModuleType = '" + strModuleType + "' and UserType = '" + strUserType + "' and LangCode = '" + strLangCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        return ds.Tables[0].Rows.Count;

    }

    protected void LoadChildModule(string strParentModuleName, string strModuleType, string strUserType, string strLangCode)
    {
        string strHQL;
        string strSortNumber, strVisible, strIsDIYFlow, strModuleName, strHomeModuleName;

        strLangCode = ddlLangSwitcher.SelectedValue.Trim();

        if (strModuleType != "APP" & strModuleType != "DIYAPP")
        {
            strHQL = " select ID,ParentModule,ModuleName,PageName,SortNumber,ModuleType,Visible,DIYFlow,HomeModuleName,LangCode,IconURL,UserType from T_ProModuleLevel ";
            strHQL += " where UserType = " + "'" + strUserType + "'" + " and ltrim(rtrim(ParentModule)) = " + "'" + strParentModuleName + "'";
            strHQL += " and ModuleType not in ('APP','DIYAPP')";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
            strHQL += " and IsDeleted = 'NO'";
            strHQL += " Order By ModuleType DESC,SortNumber ASC";
        }
        else
        {
            strHQL = " select ID,ParentModule,ModuleName,PageName,SortNumber,ModuleType,Visible,DIYFlow,HomeModuleName,LangCode,IconURL,UserType from T_ProModuleLevel ";
            strHQL += " where UserType = " + "'" + strUserType + "'" + " and ltrim(rtrim(ParentModule)) = " + "'" + strParentModuleName + "'";
            strHQL += " and ModuleType in ( 'APP','DIYAPP')";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
            strHQL += " and IsDeleted = 'NO'";
            strHQL += " Order By ModuleType DESC,SortNumber ASC";
        }

        if (strParentModuleName == "")
        {
            strHQL = " select ID,ParentModule,ModuleName,PageName,SortNumber,ModuleType,Visible,DIYFlow,HomeModuleName,LangCode,IconURL,UserType from T_ProModuleLevel ";
            strHQL += " where UserType = " + "'" + strUserType + "'" + " and ltrim(rtrim(ParentModule)) = " + "'" + strParentModuleName + "'";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
            strHQL += " and IsDeleted = 'NO'";
            strHQL += " Order By ModuleType DESC,SortNumber ASC";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        BT_ModuleSave.Enabled = true;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strSortNumber = ds.Tables[0].Rows[i][4].ToString().Trim();
            ((TextBox)DataGrid4.Items[i].FindControl("TB_SortNumber")).Text = strSortNumber;

            strModuleName = ds.Tables[0].Rows[i]["ModuleName"].ToString().Trim();
            strVisible = ds.Tables[0].Rows[i]["Visible"].ToString().Trim();
            strIsDIYFlow = ds.Tables[0].Rows[i]["DIYFlow"].ToString().Trim();

            if (strVisible == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_ModuleVisible")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_ModuleVisible")).Checked = false;
            }

            if (strIsDIYFlow == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsDIYFlow")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsDIYFlow")).Checked = false;
            }


            if (TakeTopSecurity.TakeTopLicense.IsCanNotDeletedModule(strModuleName))
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_ModuleVisible")).Checked = true;
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_ModuleVisible")).Enabled = false;
            }

            strHomeModuleName = ds.Tables[0].Rows[i]["HomeModuleName"].ToString().Trim();
            ((TextBox)DataGrid4.Items[i].FindControl("TB_HomeModuleName")).Text = strHomeModuleName;
        }

        LB_ModuleNumber.Text = ds.Tables[0].Rows.Count.ToString();
    }

    protected void LoadWorkFlowTemplate()
    {
        string strHQL;

        strHQL = "Select TemName,IdentifyString From T_WorkFlowTemplate Where Authority = 'All'";
        strHQL += " and COALESCE(XSNFile,'NULL') <> 'NULL'";
        strHQL += " Order by CreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_DIYWFTemplate.DataSource = ds;
        DL_DIYWFTemplate.DataBind();
    }

    protected int GetTopProModuleCount(string strModuleName, string strModuleType)
    {
        string strHQL;

        strHQL = " select ID,ParentModule,ModuleName,PageName,SortNumber,ModuleType from T_ProModuleLevel  ";
        strHQL += " where ModuleName = " + "'" + strModuleName + "'" + " and ModuleType = " + "'" + strModuleType + "'";
        strHQL += " and char_length(rtrim(ltrim(ParentModule))) = 0";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        return ds.Tables[0].Rows.Count;
    }

    protected int GetExistAllSameModuleNumber(string strModuleName, string strUserType, string strID)
    {
        string strHQL;

        if (strID == "")
        {
            strHQL = "Select * From T_ProModuleLevel Where ModuleName = '" + strModuleName + "' and UserType = '" + strUserType + "'";
        }
        else
        {
            strHQL = "Select * From T_ProModuleLevel Where ModuleName = '" + strModuleName + "' and UserType = '" + strUserType + "'";
            strHQL += " and ID <> " + strID;
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        return ds.Tables[0].Rows.Count;
    }

    protected int GetNewProModuleCount(string strNewModuleName, string strModuleType, string strID)
    {
        string strHQL;

        strHQL = " select ID,ParentModule, ModuleName,PageName,SortNumber,ModuleType from T_ProModuleLevel  ";
        strHQL += " where ModuleName = " + "'" + strNewModuleName + "'" + " and ModuleType = " + "'" + strModuleType + "'" + " and ID <> " + strID;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        return ds.Tables[0].Rows.Count;
    }

    protected string GetModuleType(string strModuleID)
    {
        string strHQL = "Select ModuleType From T_ProModuleLevel Where ID = " + strModuleID;

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

    protected string GetModuleName(string strModuleID)
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

    protected int GetUserAuthorizationRecordNumber()
    {
        string strHQL;

        strHQL = "Select count(*) From T_ProModule";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModule");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString().Trim());
    }


    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {

        ////设置缓存更改标志
        //ShareClass.SetPageCacheMark("1");
        //Session["CssDirectoryChangeNumber"] = "1";

        //更新页面缓存，刷新页面
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceModuleFlowView.aspx", "<%--***--%>");
        ShareClass.AddSpaceLineToFile("TTModuleFlowChartViewJS.aspx", "<%--***--%>");
        ShareClass.AddSpaceLineToFile("WFDesigner/TTTakeTopMFChartViewJS.aspx", "<%--***--%>");
    }



}
