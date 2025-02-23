using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;

using ProjectMgt.BLL;
using ProjectMgt.Model;

using TakeTopWF;

public partial class TTWorkFlowTemplate : System.Web.UI.Page
{
    string strUserCode, strMakeUserCode;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserName;
        string strDepartString;
        IList lst;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx");
        bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickB", "autoheight();", true);
        if (Page.IsPostBack == false)
        {
            Session["SuperWFAdmin"] = "NO";

            strHQL = string.Format(@"from WLType as wlType 
                                     Where wlType.LangCode = '{0}' 
                                     order by wlType.SortNumber ASC", strLangCode);
            WLTypeBLL wlTypeBLL = new WLTypeBLL();
            lst = wlTypeBLL.GetAllWLTypes(strHQL);
            DL_WLType.DataSource = lst;
            DL_WLType.DataBind();
            DL_WLType.Items.Insert(0, new ListItem("--Select--", "0"));

            DL_NewWLType.DataSource = lst;
            DL_NewWLType.DataBind();
            DL_NewWLType.Items.Insert(0, new ListItem("--Select--", "0"));

            LoadCommonWorkflowRelatedPage();

            TakeTopCore.CoreShareClass.InitialUnderDepartmentTreeByAuthority(Resources.lang.ZZJGT, TreeView1, strUserCode);

            InitialWorkFlowTree(TreeView2, strUserCode);
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strTemName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        try
        {
            if (treeNode.Depth != 0 & treeNode.Depth != 1)
            {
                strTemName = treeNode.Text.Trim().Replace("Child:", "");

                HL_WorkFlowDesigner.Enabled = true;
                LB_DesignWorkflowTemplate.Text = strTemName + " " + Resources.lang.LiuChengMuBan + Resources.lang.SheJi;

                SelectWorkflowTemplateByTemName(strTemName);

                if (getWorkflowCountByTemNameAndCreatorCode(strTemName, strUserCode) == 0)
                {
                    BT_DeleteWFTemplate.Enabled = true;
                }
                else
                {
                    BT_DeleteWFTemplate.Enabled = false;
                }
            }
            else
            {
                HL_WorkFlowDesigner.NavigateUrl = "";
                HL_WorkFlowDesigner.Enabled = false;
                LB_DesignWorkflowTemplate.Text = Resources.lang.LiuChengMuBan + Resources.lang.SheJi;
            }
        }
        catch
        {
        }
    }

    protected void BT_CreateWorkFlowTemplate_Click(object sender, EventArgs e)
    {
        string strWLType, strWorkFlowTemName;
        string strUserCode, strDepartCode, strDepartName;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        strWLType = DL_WLType.SelectedValue.Trim();
        strWorkFlowTemName = TB_WorkFlow.Text.Trim();

        if (strWLType == "0")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZSBXZGZLMBLXJC + "')", true);
            return;
        }

        if (strWorkFlowTemName != "")
        {
            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            WorkFlowTemplate workFlowTemplate = new WorkFlowTemplate();

            workFlowTemplate.TemName = strWorkFlowTemName;
            workFlowTemplate.Type = strWLType;
            workFlowTemplate.CreateTime = DateTime.Now;
            workFlowTemplate.CreatorCode = strUserCode;
            workFlowTemplate.CreatorName = ShareClass.GetUserName(strUserCode);
            workFlowTemplate.Status = "InUse";
            workFlowTemplate.Authority = "All";
            workFlowTemplate.IdentifyString = DateTime.Now.ToString("yyyyMMddHHMMssff");
            workFlowTemplate.WFDefinition = "";

            workFlowTemplate.EnableEdit = "NO";
            workFlowTemplate.BelongDepartCode = strDepartCode;
            workFlowTemplate.BelongDepartName = strDepartName;
            workFlowTemplate.SortNumber = 0;
            workFlowTemplate.Visible = "YES";
            workFlowTemplate.AutoActive = "NO";
            workFlowTemplate.DesignType = "JS";

            workFlowTemplate.OverTimeAutoAgree = "NO";
            workFlowTemplate.OverTimeHourNumber = 24;

            workFlowTemplate.XSNFile = @"Template\CommonBusinessForm.xsn";
            workFlowTemplate.PageFile = "";

            workFlowTemplate.WFDefinition = "{states:{rect2:{type:'start',text:{text:'开始'}, attr:{ x:209, y:72, width:50, height:50}, props:{guid:{value:'4af6bc4b-7ed9-0b0b-e3a0-91c9d8fd92d1'},text:{value:'开始'}}}},paths:{},props:{props:{name:{value:'新建流程'},key:{value:''},desc:{value:''}}}}";

            try
            {
                workFlowTemplateBLL.AddWorkFlowTemplate(workFlowTemplate);

                SelectWorkflowTemplateByTemName(strWorkFlowTemName);

                InitialWorkFlowTree(TreeView2, strUserCode);

                LB_DesignWorkflowTemplate.Text = strWorkFlowTemName + " " + Resources.lang.LiuChengMuBan + Resources.lang.SheJi;

                BT_DeleteWFTemplate.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ChengGong + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZSBKNCXTMCMBJC + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGZLMCBNWK + "')", true);
        }
    }

    protected void BT_DeleteWFTemplate_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strTemName, strWFType;

        strTemName = LB_WFTemplate.Text.Trim();
        strWFType = DL_WLType.SelectedValue.Trim();

        if (getWorkflowCountByTemNameAndCreatorCode(strTemName, strUserCode) == 0)
        {
            try
            {
                strHQL = string.Format(@"Delete From T_WorkFlowTemplate 
                                         Where TemName = '{0}'", strTemName);
                ShareClass.RunSqlCommand(strHQL);

                InitialWorkFlowTree(TreeView2, strUserCode);

                BT_DeleteWFTemplate.Enabled = false;
                DL_EnableEdit.Enabled = false;

                BT_CopyTem.Enabled = false;
                BT_ChangeType.Enabled = false;

                BT_UploadXSNFile.Enabled = false;
                BT_DeleteXSNFile.Enabled = false;

                BT_DeleteWFTemplate.Enabled = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCGZLMBCG + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGSCSBJC + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGSCSBJC + "')", true);
        }
    }

    protected int getWorkflowCountByTemNameAndCreatorCode(string strTemName, string strUserCode)
    {
        string strHQL = string.Format(@"Select * From T_WorkFlow 
                                       Where TemName = '{0}' 
                                       and CreatorCode = '{1}'", strTemName, strUserCode);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        return ds.Tables[0].Rows.Count;
    }

    public static void InitialUnderDepartmentTreeByAuthority(string strTreeName, TreeView TreeView, string strUserCode)
    {
        string strHQL;
        IList lst2;

        int i;

        string strParentDepartCode, strDepartCode, strDepartName;

        TreeView.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();

        node1.Text = "<B>" + strTreeName + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView.Nodes.Add(node1);

        DepartmentBLL departmentBLL = new DepartmentBLL();
        Department department = new Department();

        strParentDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = string.Format(@"from Department as department 
                                 where department.DepartCode = '{0}' 
                                 Order By department.DepartCode ASC", strParentDepartCode);
        lst2 = departmentBLL.GetAllDepartments(strHQL);

        for (i = 0; i < lst2.Count; i++)
        {
            department = (Department)lst2[i];
            strDepartCode = department.DepartCode.Trim();
            strDepartName = department.DepartName.Trim();

            node2 = new TreeNode();

            node2.Text = strDepartCode + " " + strDepartName;
            node2.Target = strDepartCode;
            node2.Expanded = true;

            node1.ChildNodes.Add(node2);
            UnderDepartmentTreeShowByAuthority(strDepartCode, node2, strUserCode);
            TreeView.DataBind();
        }
    }

    public static void UnderDepartmentTreeShowByAuthority(string strParentCode, TreeNode treeNode, string strUserCode)
    {
        string strHQL;
        IList lst1, lst2;

        string strDepartCode, strDepartName;

        strHQL = string.Format(@"from Department as department 
                                 where department.ParentCode = '{0}' 
                                 Order By department.DepartCode ASC", strParentCode);
        DepartmentBLL departmentBLL = new DepartmentBLL();
        Department department = new Department();
        lst1 = departmentBLL.GetAllDepartments(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            department = (Department)lst1[i];

            strDepartCode = department.DepartCode.Trim();
            strDepartName = department.DepartName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strDepartCode;
            node.Text = strDepartCode + " " + strDepartName;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = string.Format(@"from Department as department 
                                     where department.ParentCode = '{0}'", strDepartCode);
            lst2 = departmentBLL.GetAllDepartments(strHQL);

            if (lst2.Count > 0)
            {
                UnderDepartmentTreeShowByAuthority(strDepartCode, node, strUserCode);
            }
        }
    }

    public static void InitialActorGroupTree(TreeView TreeView, String strUserCode)
    {
        string strHQL, strActorGroupName;
        IList lst;

        TreeView.Nodes.Clear();

        TreeNode node0 = new TreeNode();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node0.Text = "<B>角色组</B>";
        node0.Target = "0";
        node0.Expanded = true;
        TreeView.Nodes.Add(node0);

        strHQL = string.Format(@"from ActorGroup as actorGroup 
                                 where actorGroup.Type = 'All' 
                                 Order by actorGroup.IdentifyString DESC");
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        ActorGroup actorGroup = new ActorGroup();

        for (int i = 0; i < lst.Count; i++)
        {
            actorGroup = (ActorGroup)lst[i];

            strActorGroupName = actorGroup.GroupName.Trim();

            node3 = new TreeNode();

            node3.Text = strActorGroupName;
            node3.Target = strActorGroupName;
            node3.Expanded = true;

            node0.ChildNodes.Add(node3);
        }

        node2 = new TreeNode();
        node2.Text = "<B>部分</B>";
        node2.Target = "1";
        node2.Expanded = false;
        node0.ChildNodes.Add(node2);

        strHQL = string.Format(@"from ActorGroup as actorGroup 
                                 where actorGroup.Type = '部分' 
                                 Order by actorGroup.IdentifyString DESC");
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            actorGroup = (ActorGroup)lst[i];

            strActorGroupName = actorGroup.GroupName.Trim();

            node4 = new TreeNode();

            node4.Text = strActorGroupName;
            node4.Target = strActorGroupName;
            node4.Expanded = true;

            node2.ChildNodes.Add(node4);
        }

        TreeView.DataBind();
    }

    protected void BT_SaveBelongDepartment_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strDepartCode, strDepartName;
        string strTemName, strWFType, strVisible, strAutoActive;
        int intSortNumber;

        strWFType = DL_WLType.SelectedValue.Trim();
        strTemName = LB_WFTemplate.Text.Trim();

        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());
        strDepartCode = LB_BelongDepartCode.Text.Trim();
        strDepartName = TB_BelongDepartName.Text.Trim();

        strVisible = DL_Visible.SelectedValue.Trim();
        strAutoActive = DL_AutoActive.SelectedValue.Trim();

        if (ShareClass.GetDepartName(strDepartCode) != strDepartName)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGBCSBBMMCHBMDMBDYBMZNBNSRJC + "')", true);
            return;
        }

        try
        {
            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set BelongDepartCode = '{0}' 
                                     Where TemName = '{1}'", strDepartCode, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set BelongDepartName = '{0}' 
                                     Where TemName = '{1}'", strDepartName, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set SortNumber = {0} 
                                     Where TemName = '{1}'", intSortNumber.ToString(), strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set Visible = '{0}' 
                                     Where TemName = '{1}'", strVisible, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set AutoActive = '{0}' 
                                     Where TemName = '{1}'", strAutoActive, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set OverTimeAutoAgree = '{0}' 
                                     Where TemName = '{1}'", DL_OverTimeAutoAgree.SelectedValue, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set OverTimeHourNumber = {0} 
                                     Where TemName = '{1}'", NB_OverTimeHourNumber.Amount.ToString(), strTemName);
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
        }
        catch
        {
        }
    }

    protected void BT_CopyTem_Click(object sender, EventArgs e)
    {
        string strOldTemName, strNewTemName, strWFType;

        strWFType = DL_WLType.SelectedValue.Trim();

        strOldTemName = LB_WFTemplate.Text.Trim();
        strNewTemName = TB_NewTemName.Text.Trim();

        if (strNewTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSBXMBMCBNKJC + "')", true);
            return;
        }

        try
        {
            string strNewIdentifyString = DateTime.Now.ToString("yyyyMMddHHMMssff");

            WFDataHandle.CopyWorkFlowTemplate(strWFType, strOldTemName, strNewTemName, strNewIdentifyString);

            InitialWorkFlowTree(TreeView2, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZFZCG + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCWFZSBJC + "')", true);
        }
    }

    public static string GetWFDefinition(string strTemName)
    {
        IList lst;
        string strHQL, strWFDefinition;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate 
                                 where workFlowTemplate.TemName = '{0}'", strTemName);
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strWFDefinition = workFlowTemplate.WFDefinition.Trim();
        }
        catch
        {
            strWFDefinition = "";
        }

        return strWFDefinition;
    }

    protected void BT_ChangeType_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strOldWLType, strNewWLType, strTemName;

        strOldWLType = LB_WFType.Text.Trim();
        strNewWLType = DL_NewWLType.SelectedValue.Trim();

        strTemName = LB_WFTemplate.Text.Trim();

        try
        {
            strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                     Set Type = '{0}' 
                                     Where TemName = '{1}'", strNewWLType, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlow 
                                     Set WLType = '{0}' 
                                     Where TemName = '{1}'", strNewWLType, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"Update T_WorkFlowBackup 
                                     Set WLType = '{0}' 
                                     Where TemName = '{1}'", strNewWLType, strTemName);
            ShareClass.RunSqlCommand(strHQL);

            LB_WFType.Text = strNewWLType;
            DL_WLType.SelectedValue = strNewWLType;

            InitialWorkFlowTree(TreeView2, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBianGenChengGong + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBianGenShiBai + "')", true);
        }
    }

    protected void BT_UploadXSNFile_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strTemName = LB_WFTemplate.Text.Trim();

            int i;
            string strHQL;

            if (strFileName1 != "")
            {
                i = strFileName1.LastIndexOf(".");
                string strNewExt = strFileName1.Substring(i);

                DateTime dtUploadNow = DateTime.Now;

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

                string strDocSavePath = Server.MapPath("Doc") + "\\WorkFlowTemplate\\";
                string strFileName3 = "Doc\\" + "WorkFlowTemplate\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);
                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_File.PostedFile.SaveAs(strFileName4);

                    strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                             Set XSNFile = '{0}' 
                                             Where TemName = '{1}'", strFileName3, strTemName);
                    ShareClass.RunSqlCommand(strHQL);

                    HL_XSNFile.Text = System.IO.Path.GetFileName(Server.MapPath(strFileName3));
                    HL_XSNFile.NavigateUrl = strFileName3;
                    HL_XSNFile.Target = "_blank";

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSHANGCCG + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYSCDWJ + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYSCDWJ + "')", true);
        }
    }

    protected void BT_DeleteXSNFile_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strTemName = LB_WFTemplate.Text.Trim();

        strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                 Set XSNFile = '' 
                                 Where TemName = '{0}'", strTemName);

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            HL_XSNFile.Text = "";
            HL_XSNFile.NavigateUrl = "";
            HL_XSNFile.Target = "_blank";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
        }
    }

    protected void DL_WFRelatedPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strTemName = LB_WFTemplate.Text.Trim();
        string strWFPageName = DL_WFRelatedPage.SelectedValue.Trim();

        if (strTemName == "")
        {
            return;
        }

        strHQL = string.Format(@"From WorkFlowTemplate as workFlowTemplate 
                                 Where workFlowTemplate.TemName = '{0}'", strTemName);
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        if (strWFPageName != "")
        {
            workFlowTemplate.PageFile = strWFPageName;
            workFlowTemplate.XSNFile = "";
        }
        else
        {
            workFlowTemplate.PageFile = "";
            workFlowTemplate.XSNFile = @"Template\CommonBusinessForm.xsn";
        }

        if (workFlowTemplate.XSNFile != "" & workFlowTemplate.XSNFile != null)
        {
            HL_XSNFile.Text = System.IO.Path.GetFileName(Server.MapPath(workFlowTemplate.XSNFile));
            HL_XSNFile.NavigateUrl = workFlowTemplate.XSNFile;
            HL_XSNFile.Target = "_blank";
        }
        else
        {
            HL_XSNFile.Text = "";
            HL_XSNFile.NavigateUrl = "";
            HL_XSNFile.Target = "_blank";
        }

        try
        {
            workFlowTemplateBLL.UpdateWorkFlowTemplate(workFlowTemplate, strTemName);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCSBJC + "')", true);
        }
    }

    protected void DL_EnableEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strTemName, strEnableEdit;

        strTemName = LB_WFTemplate.Text.Trim();
        strEnableEdit = DL_EnableEdit.SelectedValue.Trim();

        strHQL = string.Format(@"Update T_WorkFlowTemplate 
                                 Set EnableEdit = '{0}' 
                                 Where TemName = '{1}'", strEnableEdit, strTemName);

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGeiBianCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGeiBianSBQJC + "')", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID, strHQL;
            IList lst;

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            strHQL = string.Format(@"from WorkFlowTStepOperator as workFlowTStepOperator 
                                     where workFlowTStepOperator.ID = {0}", strID);
            WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
            lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
            WorkFlowTStepOperator workFlowTStepOperator = (WorkFlowTStepOperator)lst[0];

            LB_DetailID.Text = workFlowTStepOperator.ID.ToString();
            LBL_ActorGroup.Text = workFlowTStepOperator.ActorGroup;
            LBL_Requisite.Text = workFlowTStepOperator.Requisite.Trim();
            LBL_WorkDetail.Text = workFlowTStepOperator.WorkDetail;
            LBL_Actor.Text = workFlowTStepOperator.Actor.Trim();
            LBL_FinishedTime.Text = workFlowTStepOperator.LimitedTime.ToString();

            LBL_FieldList.Text = workFlowTStepOperator.FieldList.Trim();
            LBL_EditFieldList.Text = workFlowTStepOperator.EditFieldList.Trim();

            strMakeUserCode = LB_MakeUserCode.Text.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popwindowStepOperator') ", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strSendSMS, strSendEMail;

        if (e.CommandName != "Page")
        {
            string strStepID = ((Button)e.Item.FindControl("BT_StepID")).Text.ToString();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string strTemName = LB_WFTemplate.Text.Trim();

            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            string strHQL = string.Format(@"from WorkFlowTStep as workFlowTStep 
                                            where workFlowTStep.StepID = {0}", strStepID);
            IList lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

            WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

            LB_ID.Text = workFlowTStep.StepID.ToString();
            LB_SortNumber.Text = workFlowTStep.SortNumber.ToString();
            LBL_SortNumber.Text = workFlowTStep.SortNumber.ToString();
            LBL_StepName.Text = workFlowTStep.StepName;
            LBL_LimitedOperator.Text = workFlowTStep.LimitedOperator.ToString();
            LBL_LimitedTime.Text = workFlowTStep.LimitedTime.ToString();
            LBL_NextSortNumber.Text = workFlowTStep.NextSortNumber.ToString();

            LBL_NextStepMust.Text = workFlowTStep.NextStepMust.Trim();

            LBL_SelfReview.Text = workFlowTStep.SelfReview.Trim();
            LBL_IsPriorStepSelect.Text = workFlowTStep.IsPriorStepSelect.Trim();
            LBL_DepartRelated.Text = workFlowTStep.DepartRelated.Trim();
            LBL_PartTimeReview.Text = workFlowTStep.PartTimeReview.Trim();
            LBL_OperatorSelect.Text = workFlowTStep.OperatorSelect.Trim();
            LBL_ProjectRelated.Text = workFlowTStep.ProjectRelated.Trim();

            LBL_AllowSelfPass.Text = workFlowTStep.AllowSelfPass.Trim();
            LBL_AllowPriorOperatorPass.Text = workFlowTStep.AllowPriorOperatorPass.Trim();

            strSendSMS = workFlowTStep.SendSMS.Trim();
            strSendEMail = workFlowTStep.SendEMail.Trim();

            if (strSendSMS == "YES")
            {
                LBL_SendSMS.Text = "true";
            }
            else
            {
                LBL_SendSMS.Text = "false";
            }

            if (strSendEMail == "YES")
            {
                LBL_SendEMail.Text = "true";
            }
            else
            {
                LBL_SendEMail.Text = "false";
            }

            LB_StepID.Text = workFlowTStep.StepID.ToString().Trim();
            LB_StepName.Text = workFlowTStep.StepName.Trim();

            LoadWorkFlowTStepOperator(strStepID);

            strUserCode = LB_UserCode.Text.Trim();
            strMakeUserCode = LB_MakeUserCode.Text.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;
        string strTemName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);
            strTemName = LB_WFTemplate.Text.Trim();

            LB_BelongDepartCode.Text = strDepartCode;
            TB_BelongDepartName.Text = strDepartName;
        }
    }

    protected void UpdateWFDefinition(string strTemName, string strWFDefinition)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate 
                                 where workFlowTemplate.TemName = '{0}'", strTemName);
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        workFlowTemplate.WFDefinition = strWFDefinition;

        try
        {
            workFlowTemplateBLL.UpdateWorkFlowTemplate(workFlowTemplate, strTemName);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
        }
    }

    public void InitialWorkFlowTree(TreeView TreeView, String strUserCode)
    {
        string strHQL, strHQL2, strHQL3;
        DataSet ds, ds2, ds3;

        string strWFID, strWFType, strWFTypeHomeName, strTemName, strIdentifyString, strChildTemName, strChildIdentifyString;
        string strDepartCode, strUnderDepartString, strParentDepartString;

        strParentDepartString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);
        strUnderDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        TreeView.Nodes.Clear();

        TreeNode node0 = new TreeNode();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node1 = new TreeNode();
        node1.Text = "<B>" + Resources.lang.GongZuoLiuLeiXing + "&" + Resources.lang.MoBan + "</B>";
        node1.Target = "1";
        node1.Expanded = true;
        TreeView.Nodes.Add(node1);

        strHQL = string.Format(@"Select ID, Type, HomeName 
                                 From T_WLType 
                                 Where LangCode = '{0}' 
                                 and Type In (Select Type 
                                              From T_WorkFlowTemplate as workFlowTemplate 
                                              Where 1=1 
                                              and (BelongDepartCode in {1} 
                                              Or BelongDepartCode in {2} 
                                              Or TemName in (Select TemName 
                                                             From T_WorkFlowTemplateBusinessMember 
                                                             Where UserCode = '{3}') 
                                              Or TemName in (Select TemName 
                                                             From T_WorkFlowTemplateBusinessDepartment 
                                                             Where DepartCode in {1}))) 
                                 Order by SortNumber ASC", strLangCode, strParentDepartString, strUnderDepartString, strUserCode);
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strWFID = ds.Tables[0].Rows[i]["ID"].ToString().Trim();
            strWFType = ds.Tables[0].Rows[i]["Type"].ToString().Trim();
            strWFTypeHomeName = ds.Tables[0].Rows[i]["HomeName"].ToString().Trim();

            node2 = new TreeNode();
            node2.Text = "<B>" + strWFTypeHomeName + "</B>";
            node2.Target = strWFID;
            node2.Expanded = false;
            node1.ChildNodes.Add(node2);

            strHQL2 = string.Format(@"Select TemName, IdentifyString 
                                      From T_WorkFlowTemplate 
                                      Where Visible = 'YES' 
                                      and Authority = 'All' 
                                      and Type = '{0}' 
                                      and (BelongDepartCode in {1} 
                                      Or BelongDepartCode in {2}) 
                                      Order by SortNumber ASC", strWFType, strParentDepartString, strUnderDepartString);

            ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_WorkFlowTemplate");

            for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
            {
                strTemName = ds2.Tables[0].Rows[j]["TemName"].ToString().Trim();
                strIdentifyString = ds2.Tables[0].Rows[j]["IdentifyString"].ToString().Trim();

                node3 = new TreeNode();
                node3.Text = strTemName;
                node3.Target = strIdentifyString;
                node3.Expanded = false;
                node2.ChildNodes.Add(node3);

                strHQL3 = string.Format(@"Select distinct RelatedWFTemName 
                                          From T_WFTStepRelatedTem 
                                          Where RelatedStepID In (Select StepID 
                                                                  From T_WorkFlowTStep 
                                                                  Where TemName = '{0}')", strTemName);

                ds3 = ShareClass.GetDataSetFromSql(strHQL3, "T_WFTSteppRelatedTem");

                for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                {
                    strChildTemName = ds3.Tables[0].Rows[k]["RelatedWFTemName"].ToString().Trim();

                    node4 = new TreeNode();
                    node4.Text = "Child:" + strChildTemName;
                    node4.Target = strChildTemName + DateTime.Now.ToString("yyyyMMddHHMMssff");
                    node4.Expanded = false;
                    node3.ChildNodes.Add(node4);
                }
            }
        }

        TreeView.DataBind();
    }

    protected void SelectWorkflowTemplateByTemName(string strTemName)
    {
        string strHQL;
        IList lst;

        string strIdentifyString, strRelatedUserCode, strPageName, strEnableEdit, strVisible, strAutoActive, strDesignType;
        string strDepartCode, strBelongDepartCode, strBelongDepartName;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate 
                                 where workFlowTemplate.TemName = '{0}'", strTemName);

        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        strMakeUserCode = workFlowTemplate.CreatorCode.Trim();
        strIdentifyString = workFlowTemplate.IdentifyString.Trim();
        strEnableEdit = workFlowTemplate.EnableEdit.Trim();

        strBelongDepartCode = workFlowTemplate.BelongDepartCode.Trim();
        strBelongDepartName = workFlowTemplate.BelongDepartName.Trim();

        NB_SortNumber.Amount = workFlowTemplate.SortNumber;

        strVisible = workFlowTemplate.Visible.Trim();
        strAutoActive = workFlowTemplate.AutoActive.Trim();
        strDesignType = workFlowTemplate.DesignType.Trim();
        strPageName = workFlowTemplate.PageFile.Trim();

        if (workFlowTemplate.XSNFile != "" & workFlowTemplate.XSNFile != null)
        {
            HL_XSNFile.Text = System.IO.Path.GetFileName(Server.MapPath(workFlowTemplate.XSNFile));
            HL_XSNFile.NavigateUrl = workFlowTemplate.XSNFile;
            HL_XSNFile.Target = "_blank";
        }
        else
        {
            HL_XSNFile.Text = "";
            HL_XSNFile.NavigateUrl = "";
            HL_XSNFile.Target = "_blank";
        }

        HL_Creator.NavigateUrl = "TTUserInforSimple.aspx?UserCode=" + workFlowTemplate.CreatorCode.Trim();
        HL_Creator.Text = workFlowTemplate.CreatorName.Trim();

        HL_BusinessMember.NavigateUrl = "TTWorkFlowTemplateBusinessMember.aspx?IdentifyString=" + strIdentifyString;
        HL_BusinessMember.Target = "_blank";
        HL_BusinessMember.Enabled = true;

        HL_BusinessDepartment.NavigateUrl = "TTWorkFlowTemplateBusinessDepartment.aspx?IdentifyString=" + strIdentifyString;
        HL_BusinessDepartment.Target = "_blank";
        HL_BusinessDepartment.Enabled = true;

        LoadWorkFlowTStep(strTemName);

        LoadWorkFlowTStepOperator("0");

        LB_WFType.Text = workFlowTemplate.Type.Trim();
        LB_WFTemplate.Text = strTemName;

        LB_BelongDepartCode.Text = strBelongDepartCode;
        TB_BelongDepartName.Text = strBelongDepartName;

        NB_OverTimeHourNumber.Amount = workFlowTemplate.OverTimeHourNumber;
        DL_OverTimeAutoAgree.SelectedValue = workFlowTemplate.OverTimeAutoAgree.Trim();

        if (strDesignType == "JS")
        {
            HL_WorkFlowDesigner.NavigateUrl = "TTWorkFlowDesignerJS.aspx?IdentifyString=" + strIdentifyString;
            HL_WorkFlowDesigner.Target = "_blank";
            HL_WorkFlowDesigner.Enabled = true;

            HL_WorkFlowDesigner.ForeColor = Color.Red;
        }

        if (strDesignType == "SL")
        {
            HL_WorkFlowDesigner.NavigateUrl = "TTWorkFlowDesignerSL.aspx?IdentifyString=" + strIdentifyString;
            HL_WorkFlowDesigner.Target = "_blank";
            HL_WorkFlowDesigner.Enabled = true;

            HL_WorkFlowDesigner.ForeColor = Color.Red;
        }

        strRelatedUserCode = LB_RelatedUserCode.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        LB_MakeUserCode.Text = strMakeUserCode;

        DL_EnableEdit.SelectedValue = strEnableEdit;
        DL_Visible.SelectedValue = strVisible;
        DL_AutoActive.SelectedValue = strAutoActive;

        try
        {
            DL_WFRelatedPage.SelectedValue = strPageName;
        }
        catch
        {
        }

        string strUserParentDepartmentString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);

        strHQL = string.Format(@"SELECT ParentCode 
                                 FROM T_Department 
                                 where DepartCode in {0} 
                                 and ParentCode = '{1}'", strUserParentDepartmentString, strBelongDepartCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ParentDepartCode");
        if (ds.Tables[0].Rows.Count > 0)
        {
            TB_BelongDepartName.Enabled = false;
            BT_SaveBelongDepartment.Enabled = false;
        }
        else
        {
            TB_BelongDepartName.Enabled = true;
            BT_SaveBelongDepartment.Enabled = true;
        }

        BT_CopyTem.Enabled = true;
        BT_ChangeType.Enabled = true;

        if (strUserCode != strMakeUserCode)
        {
            BT_UploadXSNFile.Enabled = false;
            BT_DeleteXSNFile.Enabled = false;

            DL_EnableEdit.Enabled = false;
        }
        else
        {
            BT_UploadXSNFile.Enabled = true;
            BT_DeleteXSNFile.Enabled = true;

            DL_EnableEdit.Enabled = true;
        }
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL = string.Format(@"from Department as department 
                                       where department.DepartCode = '{0}'", strDepartCode);
        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName;
    }

    protected string GetMakeUserCode(string strTemName)
    {
        IList lst;
        string strHQL, strMakeUserCode;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate 
                                 where workFlowTemplate.TemName = '{0}'", strTemName);
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strMakeUserCode = workFlowTemplate.CreatorCode.Trim();

        return strMakeUserCode;
    }

    protected string GetWFEnableEdit(string strTemName)
    {
        IList lst;
        string strHQL;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate 
                                 where workFlowTemplate.TemName = '{0}'", strTemName);
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate.EnableEdit.Trim();
    }

    protected string GetIdentifyString(string strTemName)
    {
        IList lst;
        string strHQL, strIdentifyString;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate 
                                 where workFlowTemplate.TemName = '{0}'", strTemName);
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strIdentifyString = workFlowTemplate.IdentifyString.Trim();

        return strIdentifyString;
    }

    protected string GetWorkTemplateXSNFile(string strTemName)
    {
        IList lst;
        string strHQL, strXSNFile;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate 
                                 where workFlowTemplate.TemName = '{0}'", strTemName);
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strXSNFile = workFlowTemplate.XSNFile.Trim();
        }
        catch
        {
            strXSNFile = @"";
        }

        return strXSNFile;
    }

    protected string GetProjectName(string strProjectID)
    {
        string strHQL = string.Format(@"from Project as project 
                                       where project.ProjectID = '{0}'", strProjectID);
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

    protected void LoadWorkFlowTStep(string strTemName)
    {
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();

        string strHQL = string.Format(@"from WorkFlowTStep as workFlowTStep 
                                       where workFlowTStep.TemName = '{0}' 
                                       order by workFlowTStep.SortNumber ASC", strTemName);
        IList lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadWorkFlowTStepOperator(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = string.Format(@"from WorkFlowTStepOperator as workFlowTStepOperator 
                                 where workFlowTStepOperator.StepID = {0}", strStepID);
        WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();

        lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadCommonWorkflowRelatedPage()
    {
        string strHQL;

        strHQL = string.Format(@"Select * 
                                 From T_CommonWorkflowRelatedPage 
                                 Where LangCode = '{0}' 
                                 Order By ID ASC", strLangCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CommonWorkflowRelatedPage");

        DL_WFRelatedPage.DataSource = ds;
        DL_WFRelatedPage.DataBind();

        DL_WFRelatedPage.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected string GetActorGroupMakeUserCode(string strActorGroup)
    {
        string strHQL = string.Format(@"from ActorGroup as actorGroup 
                                       where actorGroup.GroupName = '{0}'", strActorGroup);
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        IList lst = actorGroupBLL.GetAllActorGroups(strHQL);

        ActorGroup actorGroup = (ActorGroup)lst[0];

        return actorGroup.MakeUserCode.Trim();
    }

    protected string GetActorGroupIdentityString(string strActorGroup)
    {
        string strHQL = string.Format(@"from ActorGroup as actorGroup 
                                       where actorGroup.GroupName = '{0}'", strActorGroup);
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        IList lst = actorGroupBLL.GetAllActorGroups(strHQL);

        ActorGroup actorGroup = (ActorGroup)lst[0];

        return actorGroup.IdentifyString.Trim();
    }
}
