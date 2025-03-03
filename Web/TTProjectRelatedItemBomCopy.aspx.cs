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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopCore;

public partial class TTProjectRelatedItemBomCopy : System.Web.UI.Page
{
    string strUserCode, strNewProjectID, strNewProjectName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strNewProjectID = Request.QueryString["ProjectID"];
        strNewProjectName = ShareClass.GetProjectName(strNewProjectID);

        strUserCode = Session["UserCode"].ToString();

        string strVerID, strID;
        int intBomVerID;
 
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            try
            {
                TakeTopBOM.InitialTemplatePrjectTreeForBOM(TreeView2, strUserCode, LanguageHandle.GetWord("ZongXiangMu").ToString().Trim(), "TemplateProject", LanguageHandle.GetWord("CommonProject").ToString().Trim());
            }
            catch (Exception err)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
            }

            LoadProjectItemBomVersion(strNewProjectID);

            if (DL_VersionID.Items.Count > 0)
            {
                intBomVerID = GetProjectItemBomVersionID(strNewProjectID, "InUse");
                if (intBomVerID > 0)
                {
                    DL_VersionID.SelectedValue = intBomVerID.ToString();
                    strVerID = DL_VersionID.SelectedItem.Text.Trim();

                    try
                    {
                        TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, strVerID, TreeView1);
                    }
                    catch (Exception err)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
                    }
                }

                strID = DL_VersionID.SelectedValue.Trim();
                DL_VersionType.SelectedValue = GetProjectItemBomVersionType(strID);

                strVerID = DL_OldVersionID.SelectedItem.Text.Trim();

                try
                {
                    TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, strVerID, TreeView3);
                }
                catch (Exception err)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
                }

                LB_ProjectID.Text = strNewProjectID;
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

        LoadOldProjectItemBomVersion(strProjectID2);

        if (DL_OldVersionID.Items.Count > 0)
        {
            strVerID = DL_OldVersionID.SelectedItem.Text;

            try
            {
                TakeTopBOM.InitialProjectItemBomTree(strProjectID2, strVerID, TreeView3);
            }
            catch (Exception err)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
            }

            BT_CopyVersion.Enabled = true;

        }
        else
        {
            try
            {
                TakeTopBOM.InitialProjectItemBomTree(strProjectID2, "0", TreeView3);
            }
            catch (Exception err)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
            }

            BT_CopyVersion.Enabled = false;
        }
    }


    protected void BT_NewVersion_Click(object sender, EventArgs e)
    {
        int intVerID;

        intVerID = int.Parse(NB_NewVerID.Amount.ToString());

        if (GetProjectItemBomVersionCount(strNewProjectID, intVerID.ToString()) == 0)
        {
            ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
            ProjectItemBomVersion projectItemBomVersion = new ProjectItemBomVersion();

            projectItemBomVersion.ProjectID = int.Parse(strNewProjectID);
            projectItemBomVersion.VerID = intVerID;
            projectItemBomVersion.Type = "Backup";

            try
            {
                projectItemBomVersionBLL.AddProjectItemBomVersion(projectItemBomVersion);

                LoadProjectItemBomVersion(strNewProjectID);

                try
                {
                    TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, intVerID.ToString(), TreeView3);
                    TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, intVerID.ToString(), TreeView1);
                }
                catch (Exception err)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
                }

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
        string strVerID;

        if (DL_OldVersionID.Items.Count == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBMXMBXBLYGJHBB").ToString().Trim() + "')", true);
            return;
        }

        strVerID = NB_NewVerID.Amount.ToString();

        try
        {
            strHQL = "Delete From T_ProjectItemBomVersion Where ProjectID = " + strNewProjectID + " and VerID = " + strVerID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ProjectRelatedItemBom Where ProjectID = " + strNewProjectID + " and VerID = " + strVerID;
            ShareClass.RunSqlCommand(strHQL);

            LoadProjectItemBomVersion(strNewProjectID);

            if (DL_OldVersionID.Items.Count > 0)
            {
                strVerID = DL_OldVersionID.SelectedItem.Text.Trim();

                try
                {
                    TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, strVerID, TreeView3);
                    TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, strVerID, TreeView1);
                }
                catch (Exception err)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_OldVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID, strOldProjectID;

        strID = DL_OldVersionID.SelectedValue.Trim();
        strOldProjectID = LB_OldProjectID.Text.Trim();
        strVerID = DL_OldVersionID.SelectedItem.Text.Trim();

        try
        {
            TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, strVerID, TreeView3);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }

    }

    protected void BT_CopyVersion_Click(object sender, EventArgs e)
    {
        string strOldVerID, strNewVerID, strOldProjectID;

        strOldVerID = DL_OldVersionID.SelectedItem.Text.Trim();
        strNewVerID = DL_NewVersionID.SelectedItem.Text.Trim();

        strOldProjectID = LB_OldProjectID.Text.Trim();

        strOldVerID = DL_OldVersionID.SelectedItem.Text.Trim();
        strNewVerID = DL_NewVersionID.SelectedItem.Text.Trim();

        if (strNewProjectID == strOldProjectID & strOldVerID == strNewVerID)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTYXMDFZHBFZDBOMBBHBNXTJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            TakeTopBOM.CopyProjectBomVersion(strOldProjectID, strNewProjectID, strOldVerID, strNewVerID);

            TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, strNewVerID, TreeView1);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_Version_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID;

        strVerID = DL_VersionID.SelectedItem.Text.Trim();

        strID = DL_VersionID.SelectedValue.Trim();
        DL_ChangeVersionType.SelectedValue = GetProjectItemBomVersionType(strID);

        try
        {
            TakeTopBOM.InitialProjectItemBomTree(strNewProjectID, strVerID, TreeView1);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }
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
            strHQL = "update T_ProjectItemBomVersion Set Type = 'Backup' where Type = 'InUse' and ProjectID = " + strNewProjectID;
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strType == "Baseline")
        {
            strHQL = "update T_ProjectItemBomVersion Set Type = 'Backup' where Type = 'Baseline' and ProjectID = " + strNewProjectID;
            ShareClass.RunSqlCommand(strHQL);
        }

        strHQL = "from ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ID = " + strID;
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);
        ProjectItemBomVersion projectItemBomVersion = (ProjectItemBomVersion)lst[0];

        projectItemBomVersion.Type = strType;

        try
        {
            projectItemBomVersionBLL.UpdateProjectItemBomVersion(projectItemBomVersion, int.Parse(strID));
        }
        catch
        {
        }
    }


    protected int GetProjectItemBomVersionID(string strNewProjectID, string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strNewProjectID + " and projectItemBomVersion.Type = " + "'" + strType + "'";

        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);
        if (lst.Count > 0)
        {
            ProjectItemBomVersion projectItemBomVersion = (ProjectItemBomVersion)lst[0];
            return projectItemBomVersion.ID;
        }
        else
        {
            return 0;
        }
    }

    protected string GetProjectItemBomVersionType(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ID = " + strID;
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);

        ProjectItemBomVersion projectItemBomVersion = (ProjectItemBomVersion)lst[0];

        return projectItemBomVersion.Type.Trim();
    }

    protected int GetProjectItemBomVersionCount(string strNewProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strNewProjectID + " and projectItemBomVersion.VerID =" + strVerID;
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);

        return lst.Count;
    }

    protected void LoadOldProjectItemBomVersion(string strNewProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strNewProjectID + " Order by projectItemBomVersion.VerID DESC";
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);

        DL_OldVersionID.DataSource = lst;
        DL_OldVersionID.DataBind();
    }

    protected void LoadProjectItemBomVersion(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectItemBomVersion as projectItemBomVersion Where projectItemBomVersion.ProjectID = " + strProjectID + " Order by projectItemBomVersion.VerID DESC";
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);


        DL_OldVersionID.DataSource = lst;
        DL_OldVersionID.DataBind();

        DL_NewVersionID.DataSource = lst;
        DL_NewVersionID.DataBind();

        DL_VersionID.DataSource = lst;
        DL_VersionID.DataBind();
    }
}
