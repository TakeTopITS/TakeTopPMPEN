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


using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakeTaskTestCase : System.Web.UI.Page
{
    string strUserCode;
    string strLangCode, strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strTaskID = Request.QueryString["TaskID"];
        string strProjectID = GetProjectIDByTaskID(strTaskID);


        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/"; Session["PageName"] = "TakeTopSiteContentEdit";
        _FileBrowser.SetupCKEditor(HE_Description);
HE_Description.Language = Session["LangCode"].ToString();
      

        //this.Title = LanguageHandle.GetWord("Project") + strProjectID + " " + GetProjectName(strProjectID) + "的任务：" + strTaskID + " " + GetTaskName(strTaskID) + " " + " 的测试用例！";
        LB_UserCode.Text = strUserCode;


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_Description.Visible = true;
            }
            else
            {
                HE_Description.Visible = true;
            }

            InitialTemplatePrjectTaskTestCaseTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZongXiangMu"), LanguageHandle.GetWord("ZZTSDSSFF"), "TemplateProject", LanguageHandle.GetWord("CommonProject"));

            strHQL = "from TestStatus as testStatus";
            strHQL += " Where testStatus.LangCode =" + "'" + strLangCode + "'";
            strHQL += " Order by testStatus.SortNumber ASC";
            TestStatusBLL testStatusBLL = new TestStatusBLL();
            lst = testStatusBLL.GetAllTestStatuss(strHQL);
            DL_Status.DataSource = lst;
            DL_Status.DataBind();


            strHQL = "from TaskTestCase as taskTestCase where taskTestCase.TaskID = " + strTaskID;
            TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
            lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_TaskID.Text = strTaskID;
            LB_ProjectID.Text = strProjectID;
            LB_Sql.Text = strHQL;
        }
    }

    //创建模板项目树（用于测试用例复制）
    public static void InitialTemplatePrjectTaskTestCaseTree(TreeView TemplateProjectTreeView, string strUserCode, string strTotalProject, string strPushImplementationMethod, string strTemplateProject, string strCommonProject)
    {
        string strHQL, strProjectID2, strProject;
        IList lst;

        //添加根节点
        TemplateProjectTreeView.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();
        TreeNode node5 = new TreeNode();
        TreeNode node6 = new TreeNode();
        TreeNode node7 = new TreeNode();

        node1.Text = "<B>" + strTotalProject + "</B>";
        node1.Target = "1";
        node1.Expanded = true;
        TemplateProjectTreeView.Nodes.Add(node1);

        node2.Text = strTemplateProject;
        node2.Target = strTemplateProject;
        node2.Expanded = true;
        node1.ChildNodes.Add(node2);

        node3.Text = strCommonProject;
        node3.Target = strCommonProject;
        node3.Expanded = true;
        node1.ChildNodes.Add(node3);

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ProjectClass = 'TemplateProject' ";   
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID2 = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node4 = new TreeNode();

            node4.Text = strProjectID2 + "." + strProject;
            node4.Target = "Project" + strProjectID2;
            node4.Expanded = false;

            node2.ChildNodes.Add(node4);

            InitialTaskTestCaseTree(node4, strProjectID2);

            TemplateProjectTreeView.DataBind();
        }

        strHQL = "from Project as project where project.ProjectClass = 'RegularProject' and  project.PMCode = " + "'" + strUserCode + "'";   
        strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID2 = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node5 = new TreeNode();

            node5.Text = strProjectID2 + "." + strProject;
            node5.Target = "Project" + strProjectID2;
            node5.Expanded = false;

            node3.ChildNodes.Add(node5);

            InitialTaskTestCaseTree(node5, strProjectID2);

            TemplateProjectTreeView.DataBind();
        }
    }

    public static void InitialTaskTestCaseTree(TreeNode node, string strProjectID)
    {
        string strHQL, strTestCaseID, strTestCaseName;
        IList lst1;

        TreeNode node1;

        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        TaskTestCase taskTestCase = new TaskTestCase();

        strHQL = "from TaskTestCase as taskTestCase where taskTestCase.TaskID in (select projectTask from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + ")";
        lst1 = taskTestCaseBLL.GetAllTaskTestCases(strHQL);
        for (int j = 0; j < lst1.Count; j++)
        {
            taskTestCase = (TaskTestCase)lst1[j];
            strTestCaseID = taskTestCase.ID.ToString();
            strTestCaseName = taskTestCase.CaseName.Trim();

            node1 = new TreeNode();
            node1.Target = strTestCaseID;
            node1.Text = strTestCaseID + "." + strTestCaseName;
            node1.Expanded = true;

            node.ChildNodes.Add(node1);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strCaseID, strHQL;
        IList lst;

        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        TaskTestCase taskTestCase = new TaskTestCase();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strCaseID = treeNode.Target.Trim();

        if (strCaseID.IndexOf("Project") == -1)
        {
            strHQL = "from TaskTestCase as taskTestCase where taskTestCase.ID = " + strCaseID;
            lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);
            for (int i = 0; i < lst.Count; i++)
            {
                taskTestCase = (TaskTestCase)lst[i];

                TB_CaseName.Text = taskTestCase.CaseName.Trim();

                if (strIsMobileDevice == "YES")
                {
                    HT_Description.Text = taskTestCase.Description.Trim();
                }
                else
                {
                    HE_Description.Text = taskTestCase.Description.Trim();
                }

                TB_Requisite.Text = taskTestCase.Requisite.Trim();
            }
        }

      
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_CaseID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strCaseID;

        strCaseID = LB_CaseID.Text.Trim();

        if (strCaseID == "")
        {
            AddCase();
        }
        else
        {
            UpdateCase();
        }
    }

    protected void AddCase()
    {
       string strCaseID, strProjectID, strTaskID, strCaseName, strDescription, strRequisite;

        strProjectID = LB_ProjectID.Text.Trim();
        strTaskID = LB_TaskID.Text.Trim();
        strCaseName = TB_CaseName.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strDescription = HT_Description.Text;
        }
        else
        {
            strDescription = HE_Description.Text;
        }
        
        strRequisite = TB_Requisite.Text.Trim();

        if (strTaskID != "" & strCaseName != "" & strDescription != "" & strRequisite != "")
        {
            TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
            TaskTestCase taskTestCase = new TaskTestCase();

            taskTestCase.ProjectID = int.Parse(strProjectID);
            taskTestCase.TaskID = int.Parse(strTaskID);
            taskTestCase.CaseName = strCaseName;
            taskTestCase.Description = strDescription;
            taskTestCase.Requisite = strRequisite;
            taskTestCase.Status = "Plan";

            try
            {
                taskTestCaseBLL.AddTaskTestCase(taskTestCase);
                strCaseID = ShareClass.GetMyCreatedMaxTaskTestCaseID();

                LB_CaseID.Text = strCaseID;

                LoadTaskTestCase(strTaskID);
                InitialTemplatePrjectTaskTestCaseTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZongXiangMu"), LanguageHandle.GetWord("ZZTSDSSFF"), "TemplateProject", LanguageHandle.GetWord("CommonProject"));
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateCase()
    {
        string strID, strTaskID, strCaseName, strDescription, strRequisite, strStatus;
        string strHQL;
        IList lst;

        strTaskID = LB_TaskID.Text.Trim();

        strID = LB_CaseID.Text.Trim();
        strCaseName = TB_CaseName.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strDescription = HT_Description.Text;
        }
        else
        {
            strDescription = HE_Description.Text;
        }
        
        strRequisite = TB_Requisite.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        strHQL = "from TaskTestCase as taskTestCase where taskTestCase.ID = " + strID;
        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

        TaskTestCase taskTestCase = (TaskTestCase)lst[0];

        taskTestCase.CaseName = strCaseName;
        taskTestCase.Description = strDescription;
        taskTestCase.Requisite = strRequisite;
        taskTestCase.Status = strStatus;


        try
        {
            taskTestCaseBLL.UpdateTaskTestCase(taskTestCase, int.Parse(strID));
            LoadTaskTestCase(strTaskID);

            InitialTemplatePrjectTaskTestCaseTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZongXiangMu"), LanguageHandle.GetWord("ZZTSDSSFF"), "TemplateProject", LanguageHandle.GetWord("CommonProject"));
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }
    

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;
           
            string strTaskID = LB_TaskID.Text.Trim();

            string strID = e.Item.Cells[2].Text;

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from TaskTestCase as taskTestCase where taskTestCase.ID = " + strID;
                TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
                lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

                TaskTestCase taskTestCase = (TaskTestCase)lst[0];

                LB_CaseID.Text = taskTestCase.ID.ToString();
                TB_CaseName.Text = taskTestCase.CaseName.Trim();
                if (strIsMobileDevice == "YES")
                {
                    HT_Description.Text = taskTestCase.Description.Trim();
                }
                else
                {
                    HE_Description.Text = taskTestCase.Description.Trim();
                }
                TB_Requisite.Text = taskTestCase.Requisite.Trim();

                DL_Status.SelectedValue = taskTestCase.Status;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                strHQL = "from TaskTestCase as taskTestCase where taskTestCase.ID = " + strID;
                TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
                lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

                TaskTestCase taskTestCase = (TaskTestCase)lst[0];

                try
                {
                    taskTestCaseBLL.DeleteTaskTestCase(taskTestCase);
                    LoadTaskTestCase(strTaskID);

                    InitialTemplatePrjectTaskTestCaseTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZongXiangMu"), LanguageHandle.GetWord("ZZTSDSSFF"), "TemplateProject", LanguageHandle.GetWord("CommonProject"));

                    LB_CaseID.Text = "";
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void LoadTaskTestCase(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskTestCase as taskTestCase where taskTestCase.TaskID = " + strTaskID + " Order by taskTestCase.TaskID ASC";
        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        IList lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected string GetProjectName(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

    protected string GetTaskName(string strTaskID)
    {
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.Task.Trim();
    }

    protected string GetProjectIDByTaskID(string strTaskID)
    {
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.ProjectID.ToString();
    }

}
