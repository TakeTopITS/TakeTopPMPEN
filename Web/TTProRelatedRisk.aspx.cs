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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProRelatedRisk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strProjectName;
        string strUserCode, strUserName;
        string strProjectID;

        strUserCode = Session["UserCode"].ToString();

        strUserName = GetUserName(strUserCode);

        strProjectID = Request.QueryString["ProjectID"];
        LB_ProjectID.Text = strProjectID;

        strProjectName = GetProjectName(strProjectID);

        //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + strProjectName + "的风险列表";

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            DLC_EffectDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_FindDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            InitialTemplatePrjectRiskTree(TreeView1, strUserCode, LanguageHandle.GetWord("ZongXiangMu").ToString().Trim(), LanguageHandle.GetWord("ZZTSDSSFF").ToString().Trim(), LanguageHandle.GetWord("MuBanXiangMu").ToString().Trim(), LanguageHandle.GetWord("CommonProject").ToString().Trim());

            LoadProjectRiskList(strProjectID);
        }
    }

    //创建模板项目树（用于测试用例复制）
    public static void InitialTemplatePrjectRiskTree(TreeView TemplateProjectTreeView, string strUserCode, string strTotalProject, string strPushImplementationMethod, string strTemplateProject, string strCommonProject)
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


        strHQL = "from Project as project where project.ProjectClass = '模板项目' "; 
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

            InitialProjectRiskTree(node4, strProjectID2);

            TemplateProjectTreeView.DataBind();
        }

        strHQL = "from Project as project where project.ProjectClass = '常规项目' and  project.PMCode = " + "'" + strUserCode + "'"; 
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

            InitialProjectRiskTree(node3, strProjectID2);

            TemplateProjectTreeView.DataBind();
        }
    }

    public static void InitialProjectRiskTree(TreeNode node, string strProjectID)
    {
        string strHQL,strRiskID, strRiskName;
        IList lst1;

        TreeNode node1;

        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        ProjectRisk projectRisk = new ProjectRisk();

        strHQL = "from ProjectRisk as projectRisk where projectRisk.ProjectID  = " + strProjectID;
        lst1 = projectRiskBLL.GetAllProjectRisks(strHQL);
        for (int j = 0; j < lst1.Count; j++)
        {
            projectRisk = (ProjectRisk)lst1[j];
            strRiskID = projectRisk.ID.ToString();
            strRiskName = projectRisk.Risk.Trim();

            node1 = new TreeNode();
            node1.Target = strRiskID;
            node1.Text = strRiskID + "." + strRiskName;
            node1.Expanded = true;

            node.ChildNodes.Add(node1);
        }

    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strRiskID;
        string strProjectID = LB_ProjectID.Text.Trim();

        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        ProjectRisk projectRisk = new ProjectRisk();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strRiskID = treeNode.Target.Trim();

        if (strRiskID.IndexOf("Project") == -1)
        {
            strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
            lst = projectRiskBLL.GetAllProjectRisks(strHQL);

            projectRisk = (ProjectRisk)lst[0];

            TB_RiskName.Text = projectRisk.Risk.Trim();
            TB_RiskDetail.Text = projectRisk.Detail.Trim();
            DL_RiskLevel.SelectedValue = projectRisk.RiskLevel.Trim();
            DLC_EffectDate.Text = projectRisk.EffectDate.ToString("yyyy-MM-dd");
            DLC_FindDate.Text = projectRisk.FindDate.ToString("yyyy-MM-dd");
            LB_ID.Text = projectRisk.ID.ToString();
            DL_Status.SelectedValue = projectRisk.Status.Trim();

      

            HL_RiskToTask.Enabled = true;
            HL_RiskToTask.NavigateUrl = "TTRiskToTask.aspx?RiskID=" + strRiskID + "&ProjectID=" + strProjectID;
            HL_RiskRelatedDoc.Enabled = true;
            HL_RiskRelatedDoc.NavigateUrl = "TTRiskRelatedDoc.aspx?RelatedID=" + strRiskID;
            HL_RiskReviewWF.Enabled = true;
            HL_RiskReviewWF.NavigateUrl = "TTRiskReviewWF.aspx?RiskID=" + strRiskID;


            HL_RelatedWorkFlowTemplate.Enabled = true;
            HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=ProjectRisk&RelatedID=" + strRiskID;
            HL_ActorGroup.Enabled = true;
            HL_ActorGroup.NavigateUrl = "TTRelatedActorGroup.aspx?RelatedType=ProjectRisk&RelatedID=" + strRiskID;
            HL_WLTem.Enabled = true;
            HL_WLTem.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=ProjectRisk&RelatedID=" + strRiskID;

            HL_RunRiskByWF.Enabled = true;
            HL_RunRiskByWF.NavigateUrl = "TTRelatedDIYWorkFlowForm.aspx?RelatedType=ProjectRisk&RelatedID=" + strRiskID;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[2].Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
            string strHQL;

            if (e.CommandName == "Update" | e.CommandName == "Other")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strID;
                ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
                IList lst = projectRiskBLL.GetAllProjectRisks(strHQL);
                ProjectRisk projectRisk = (ProjectRisk)lst[0];

                TB_RiskName.Text = projectRisk.Risk.Trim();
                TB_RiskDetail.Text = projectRisk.Detail.Trim();
                DL_RiskLevel.SelectedValue = projectRisk.RiskLevel.Trim();
                DLC_EffectDate.Text = projectRisk.EffectDate.ToString("yyyy-MM-dd");
                DLC_FindDate.Text = projectRisk.FindDate.ToString("yyyy-MM-dd");
                LB_ID.Text = projectRisk.ID.ToString();
                DL_Status.SelectedValue = projectRisk.Status.Trim();

                HL_RiskToTask.Enabled = true;
                HL_RiskToTask.NavigateUrl = "TTRiskToTask.aspx?RiskID=" + strID + "&ProjectID=" + strProjectID;
                HL_RiskRelatedDoc.Enabled = true;
                HL_RiskRelatedDoc.NavigateUrl = "TTRiskRelatedDoc.aspx?RelatedID=" + strID;
                HL_RiskReviewWF.Enabled = true;
                HL_RiskReviewWF.NavigateUrl = "TTRiskReviewWF.aspx?RiskID=" + strID;

                HL_RelatedWorkFlowTemplate.Enabled = true;
                HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;
                HL_ActorGroup.Enabled = true;
                HL_ActorGroup.NavigateUrl = "TTRelatedActorGroup.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;
                HL_WLTem.Enabled = true;
                HL_WLTem.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;

                HL_RunRiskByWF.Enabled = true;
                HL_RunRiskByWF.NavigateUrl = "TTRelatedDIYWorkFlowForm.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;
                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                }

                if (e.CommandName == "Other")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popOtherWindow','true') ", true);

                }
            }

            if (e.CommandName == "Delete")
            {
                string  strRisk, strDetail, strLevel;
                DateTime dtEffectDate;

              
                strProjectID = LB_ProjectID.Text.Trim();
                strRisk = TB_RiskName.Text.Trim();
                strDetail = TB_RiskDetail.Text.Trim();
                dtEffectDate = DateTime.Parse(DLC_EffectDate.Text);
                strLevel = DL_RiskLevel.SelectedValue;

                ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
                ProjectRisk projectRisk = new ProjectRisk();
                projectRisk.ID = int.Parse(strID);

                try
                {
                    projectRiskBLL.DeleteProjectRisk(projectRisk);

                    LoadProjectRiskList(strProjectID);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        IList lst = projectRiskBLL.GetAllProjectRisks(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }


    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strRiskID;

        strRiskID = LB_ID.Text.Trim();

        if (strRiskID == "")
        {
            AddRisk();
        }
        else
        {
            UpdateRisk();
        }
    }

    protected void AddRisk()
    {
        string strID;
        string strRisk, strDetail, strLevel, strStatus;
        int intProjectID;
        DateTime dtEffectDate, dtFindDate;

        string strProjectID = LB_ProjectID.Text.Trim();

        strRisk = TB_RiskName.Text.Trim();
        strDetail = TB_RiskDetail.Text.Trim();
        strLevel = DL_RiskLevel.SelectedValue;
        dtEffectDate = DateTime.Parse(DLC_EffectDate.Text);
        dtFindDate = DateTime.Parse(DLC_FindDate.Text);
        strStatus = DL_Status.SelectedValue.Trim();

        intProjectID = int.Parse(LB_ProjectID.Text.Trim());


        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        ProjectRisk projectRisk = new ProjectRisk();

        projectRisk.ProjectID = intProjectID;
        projectRisk.Risk = strRisk;
        projectRisk.Detail = strDetail;
        projectRisk.RiskLevel = strLevel;
        projectRisk.EffectDate = dtEffectDate;
        projectRisk.FindDate = dtFindDate;
        projectRisk.Status = strStatus;

        try
        {
            projectRiskBLL.AddProjectRisk(projectRisk);
            strID = ShareClass.GetMyCreatedMaxRiskID(strProjectID);

      

            HL_RiskToTask.Enabled = true;
            HL_RiskToTask.NavigateUrl = "TTRiskToTask.aspx?RiskID=" + strID + "&ProjectID=" + strProjectID;
            HL_RiskRelatedDoc.Enabled = true;
            HL_RiskRelatedDoc.NavigateUrl = "TTRiskRelatedDoc.aspx?RelatedID=" + strID;
            HL_RiskReviewWF.Enabled = true;
            HL_RiskReviewWF.NavigateUrl = "TTRiskReviewWF.aspx?RiskID=" + strID;


            HL_RelatedWorkFlowTemplate.Enabled = true;
            HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;
            HL_ActorGroup.Enabled = true;
            HL_ActorGroup.NavigateUrl = "TTRelatedActorGroup.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;
            HL_WLTem.Enabled = true;
            HL_WLTem.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;

            HL_RunRiskByWF.Enabled = true;
            HL_RunRiskByWF.NavigateUrl = "TTRelatedDIYWorkFlowForm.aspx?RelatedType=ProjectRisk&RelatedID=" + strID;

            LoadProjectRiskList(strProjectID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateRisk()
    {
        string strID, strProjectID, strRisk, strDetail, strLevel, strStatus;
        DateTime dtEffectDate, dtFindDate;

        strID = LB_ID.Text.Trim();
        strProjectID = LB_ProjectID.Text.Trim();
        strRisk = TB_RiskName.Text.Trim();
        strDetail = TB_RiskDetail.Text.Trim();
        dtEffectDate = DateTime.Parse(DLC_EffectDate.Text);
        dtFindDate = DateTime.Parse(DLC_FindDate.Text);

        strLevel = DL_RiskLevel.SelectedValue;
        strStatus = DL_Status.SelectedValue.Trim();

        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        ProjectRisk projectRisk = new ProjectRisk();

        projectRisk.ID = int.Parse(strID);
        projectRisk.ProjectID = int.Parse(strProjectID);
        projectRisk.Risk = strRisk;
        projectRisk.Detail = strDetail;
        projectRisk.EffectDate = dtEffectDate;
        projectRisk.FindDate = dtFindDate;
        projectRisk.RiskLevel = strLevel;
        projectRisk.Status = strStatus;

        try
        {
            projectRiskBLL.UpdateProjectRisk(projectRisk, int.Parse(strID));

            LoadProjectRiskList(strProjectID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void LoadProjectRiskList(string strProjectID)
    {
        string strHQL = "from ProjectRisk as projectRisk where projectRisk.ProjectID = " + strProjectID + " order by projectRisk.ID DESC";
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        IList lst = projectRiskBLL.GetAllProjectRisks(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName;
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

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    protected string GetProjectStatus(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        string strStatus = project.Status.Trim();

        return strStatus;
    }
}
