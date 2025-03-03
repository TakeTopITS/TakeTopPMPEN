using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using TakeTopCore;
using Npgsql;

using Microsoft.Win32.SafeHandles;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTProjectPlanCopy : System.Web.UI.Page
{
    string strUserCode, strNewProjectID, strNewProjectName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strTemProjectID;

        strNewProjectID = Request.QueryString["ProjectID"];
        strNewProjectName = ShareClass.GetProjectName(strNewProjectID);

        strTemProjectID = Request.QueryString["TemProjectID"];
        strUserCode = Session["UserCode"].ToString();

        string strVerID, strID;
        int intPlanID;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            TakeTopPlan.InitialTemplatePrjectTreeForPlan(TreeView2, strUserCode, strTemProjectID, LanguageHandle.GetWord("ZongXiangMu").ToString().Trim(), LanguageHandle.GetWord("ZZTSDSSFF").ToString().Trim(), "TemplateProject", LanguageHandle.GetWord("CommonProject").ToString().Trim());

            LoadProjectPlanVersion(strNewProjectID);

            if (DL_VersionID.Items.Count > 0)
            {
                intPlanID = ShareClass.GetProjectPlanVersionID(strNewProjectID, "InUse");

                if (intPlanID > 0)
                {
                    DL_VersionID.SelectedValue = intPlanID.ToString();
                }

                strID = DL_VersionID.SelectedValue.Trim();
                DL_VersionType.SelectedValue = GetProjectPlanVersionType(strID);

                strVerID = DL_VersionID.SelectedItem.Text.Trim();
                TakeTopPlan.InitialProjectPlanTree(TreeView1, strNewProjectID, strVerID);

                LB_ProjectID.Text = strNewProjectID;

                HL_ProPlanGanttNew.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strNewProjectID + "&VerID=" + strVerID;

                BT_CopyVersion.Attributes.Add("onclick", "if(!confirm('" + LanguageHandle.GetWord("ZZNQDYFGZGJHM").ToString().Trim() + "')) return false;");
            }
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID2, strVerID;


        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        strProjectID2 = treeNode.Target.Trim();

        LB_OldProjectID.Text = strProjectID2;

        LoadOldProjectPlanVersion(strProjectID2);

        if (DL_OldVersionID.Items.Count > 0)
        {
            strVerID = DL_OldVersionID.SelectedItem.Text;
            TakeTopPlan.InitialProjectPlanTree(TreeView3, strProjectID2, strVerID);

            BT_CopyVersion.Enabled = true;
            HL_ProPlanGanttOld.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strProjectID2 + "&VerID=" + strVerID;
        }
        else
        {
            TakeTopPlan.InitialProjectPlanTree(TreeView3, strProjectID2, "0");

            BT_CopyVersion.Enabled = false;
        }
    }

    protected void BT_NewVersion_Click(object sender, EventArgs e)
    {
        string strID, strType;
        int intVerID;

        intVerID = int.Parse(NB_NewVerID.Amount.ToString());
        strType = DL_VersionType.SelectedValue.Trim();

        if (intVerID > 100 | intVerID < 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBBHZNS1100ZJDSZ").ToString().Trim() + "')", true);
            return;
        }

        if (GetProjectPlanVersionCount(strNewProjectID, intVerID.ToString()) == 0)
        {
            ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
            ProjectPlanVersion projectPlanVersion = new ProjectPlanVersion();
            projectPlanVersion.VerID = intVerID;
            projectPlanVersion.ProjectID = int.Parse(strNewProjectID);
            projectPlanVersion.Type = strType;
            projectPlanVersion.CreatorCode = strUserCode;
            projectPlanVersion.CreateTime = DateTime.Now;
            projectPlanVersion.FromProjectID = int.Parse(strNewProjectID);
            projectPlanVersion.FromProjectPlanVerID = intVerID;

            try
            {
                projectPlanVersionBLL.AddProjectPlanVersion(projectPlanVersion);
                LoadProjectPlanVersion(strNewProjectID);
                TakeTopPlan.InitialProjectPlanTree(TreeView1, strNewProjectID, intVerID.ToString());

                strID = DL_VersionID.SelectedValue.Trim();
                strType = DL_ChangeVersionType.SelectedValue.Trim();


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCXMZKNCZCBBHJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteVersion_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strType, strVerID;
        string strProjectCreatorCode;

        if (DL_VersionID.Items.Count == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBMXMBXBLYGJHBB").ToString().Trim() + "')", true);
            return;
        }

        strVerID = NB_NewVerID.Amount.ToString();
        strProjectCreatorCode = GetProjectCreatorCode(strNewProjectID);

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.VerID = " + strVerID + " and projectPlanVersion.ProjectID = " + strNewProjectID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);
        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
            strType = projectPlanVersion.Type.Trim();
            try
            {
                if (strType == "Baseline" & strType == "InUse")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBBZBBZNYLXZSCJC").ToString().Trim() + "')", true);
                    return;
                }
                else
                {
                    projectPlanVersionBLL.DeleteProjectPlanVersion(projectPlanVersion);

                    strHQL = "Delete From T_Document Where RelatedType = 'Plan' and RelatedID in (Select ID From T_ImplePlan Where ProjectID = " + strNewProjectID + " and VerID = " + strVerID + ")";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "delete from T_PlanMember where PlanID in (Select ID From T_ImplePlan where ProjectID = " + strNewProjectID + " and VerID = " + strVerID + ")";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "delete from dependency Where From_ID in (Select ID From T_ImplePlan where ProjectID = " + strNewProjectID + " and VerID = " + strVerID + ")";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From assignment Where task_id in (Select ID From T_ImplePlan where ProjectID = " + strNewProjectID + " and VerID = " + strVerID + ")";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "delete from T_ImplePlan where ProjectID = " + strNewProjectID + " and VerID = " + strVerID;
                    ShareClass.RunSqlCommand(strHQL);
                }

                LoadProjectPlanVersion(strNewProjectID);

                if (DL_VersionID.Items.Count > 0)
                {
                    strVerID = DL_VersionID.SelectedItem.Text.Trim();
                }

                TakeTopPlan.InitialProjectPlanTree(TreeView1, strNewProjectID, strVerID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBKNBCZCBBHJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DL_OldVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID, strOldProjectID;

        strID = DL_OldVersionID.SelectedValue.Trim();
        strOldProjectID = LB_OldProjectID.Text.Trim();
        strVerID = DL_OldVersionID.SelectedItem.Text.Trim();

        TakeTopPlan.InitialProjectPlanTree(TreeView3, strOldProjectID, strVerID);
        HL_ProPlanGanttOld.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strOldProjectID + "&VerID=" + strVerID;
    }

    protected void BT_CopyVersion_Click(object sender, EventArgs e)
    {
        string strOldVerID, strNewVerID, strOldProjectID, strOldGanttPID, strNewGanttPID;
        string strPlanDocCopy, strPlanVerType, strWFTemplateCopy;

        strOldVerID = DL_OldVersionID.SelectedItem.Text.Trim();
        strNewVerID = DL_NewVersionID.SelectedItem.Text.Trim();

        strPlanVerType = GetProjectPlanVersionTypeByVerID(strNewProjectID, strNewVerID);
        if (strPlanVerType == "Baseline" || strPlanVerType == "InUse")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBNFZBZBBDJHJC").ToString().Trim() + "')", true);
            return;
        }

        strOldProjectID = LB_OldProjectID.Text.Trim();
        strPlanDocCopy = DL_PlanDocCopy.SelectedValue.Trim();
        strWFTemplateCopy = DL_WFTemplateCopy.SelectedValue.Trim();

        strOldGanttPID = GetPIDForGantt(int.Parse(strOldProjectID), int.Parse(strOldVerID)).ToString();
        strNewGanttPID = GetPIDForGantt(int.Parse(strNewProjectID), int.Parse(strNewVerID)).ToString();

        try
        {
            //把模板项目的计划复制给新项目
            if (TakeTopPlan.CopyProjectPlanVersionToNewProject(strOldProjectID, strNewProjectID, strNewProjectName, strOldVerID, strNewVerID, strPlanDocCopy, strWFTemplateCopy, strUserCode) == true)
            {
                DL_VersionID.SelectedValue = DL_NewVersionID.SelectedValue;

                TakeTopPlan.InitialProjectPlanTree(TreeView1, strNewProjectID, strNewVerID);

                //改变基准时间段和单位的值
                string strHQL;
                strHQL = "update T_ImplePlan Set BaseLine_Start_Date = Start_Date,BaseLine_End_Date = End_Date Where ProjectID = " + strNewProjectID + " and VerID = " + strNewVerID;
                ShareClass.RunSqlCommand(strHQL);
                strHQL = "update T_ImplePlan Set Duration = F_WorkDay(Start_Date,End_Date), Duration_Unit = 'd' Where ProjectID = " + strNewProjectID + " and VerID = " + strNewVerID;
                ShareClass.RunSqlCommand(strHQL);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBJC").ToString().Trim() + "')", true);
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_Version_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID;
        string strHQL;
        IList lst;

        strID = DL_VersionID.SelectedValue.Trim();

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ID = " + strID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);
        ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];

        DL_ChangeVersionType.SelectedValue = projectPlanVersion.Type.Trim();

        strVerID = projectPlanVersion.VerID.ToString();
        TakeTopPlan.InitialProjectPlanTree(TreeView1, strNewProjectID, strVerID);

        HL_ProPlanGanttNew.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strNewProjectID + "&VerID=" + strVerID;
    }

    protected void DL_ChangeVersionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strType;
        string strHQL;
        IList lst;

        strID = DL_VersionID.SelectedValue.Trim();
        strType = DL_ChangeVersionType.SelectedValue.Trim();

        if (strType == "InUse")
        {
            strHQL = "update T_ProjectPlanVersion Set Type = 'Backup' where Type = 'InUse' and ProjectID = " + strNewProjectID;
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strType == "Baseline")
        {
            strHQL = "update T_ProjectPlanVersion Set Type = 'Backup' where Type = 'Baseline' and ProjectID = " + strNewProjectID;
            ShareClass.RunSqlCommand(strHQL);
        }

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ID = " + strID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);
        ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];

        projectPlanVersion.Type = strType;

        try
        {
            projectPlanVersionBLL.UpdateProjectPlanVersion(projectPlanVersion, int.Parse(strID));
        }
        catch
        {
        }
    }

    protected void LoadProjectPlanVersion(string strNewProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strNewProjectID + " Order by projectPlanVersion.VerID DESC";
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        DL_VersionID.DataSource = lst;
        DL_VersionID.DataBind();

        DL_NewVersionID.DataSource = lst;
        DL_NewVersionID.DataBind();
    }


    protected void LoadOldProjectPlanVersion(string strNewProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strNewProjectID + " Order by projectPlanVersion.VerID DESC";
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        DL_OldVersionID.DataSource = lst;
        DL_OldVersionID.DataBind();
    }


    protected int GetProjectPlanID(string strPriorID)
    {
        string strHQL, strVerID;
        IList lst;

        if (strPriorID == "0")
        {
            return 0;
        }

        strVerID = DL_VersionID.SelectedItem.Text.Trim();
        strHQL = "from WorkPlan as workPlan where workPlan.ProjectID = " + strNewProjectID + " and workPlan.VerID = " + strVerID + " and workPlan.ID = " + strPriorID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);

        WorkPlan workPlan = (WorkPlan)lst[0];
        return workPlan.ID;
    }

    protected string GetProjectPlanVersionType(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ID = " + strID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];

        return projectPlanVersion.Type.Trim();
    }

    protected int GetProjectPlanVersionCount(string strNewProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strNewProjectID + " and projectPlanVersion.VerID =" + strVerID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        return lst.Count;
    }

    protected string GetProjectCreatorCode(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.UserCode.Trim();
    }


    protected string GetProjectPlanVersionType(string strProjectID, string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.ID = " + strID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];

        return projectPlanVersion.Type.Trim();
    }

    protected string GetProjectPlanVersionTypeByVerID(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.VerID = " + strVerID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];

        return projectPlanVersion.Type.Trim();
    }

    public static string getProjectPlanMemberLeader(string strPlanID)
    {
        string strHQL;

        strHQL = "Select UserName From T_PlanMember Where PlanID = " + strPlanID + " and IsLeader = 'YES'";
        DataSet ds = CoreShareClass.GetDataSetFromSql(strHQL, "T_PlanMember");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    public static string getProjectPlanLeaderName(string strPlanID)
    {
        string strHQL;

        strHQL = "Select Leader From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = CoreShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return "";
        }
    }

    //取得GANTT图控件用的项目和计划版本号
    public static int GetPIDForGantt(int intProjectID, int intVerID)
    {
        string strVerID, strPID;

        if (intVerID < 10)
        {
            strVerID = "0" + intVerID.ToString();
        }
        else
        {
            strVerID = intVerID.ToString();
        }

        strPID = intProjectID.ToString() + strVerID;

        return int.Parse(strPID);
    }

    //取得GANTT图控件用的父计划号
    public static int GetParentIDGantt(string strProjectID, string strVerID, string strParentIDGantt)
    {
        string strHQL;

        strHQL = "Select  ID From T_ImplePlan Where ProjectID =" + strProjectID + "and VerID = " + strVerID + "  and Parent_ID = " + strParentIDGantt;
        DataSet ds = CoreShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

   
}
