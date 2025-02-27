using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTProjectPrimaveraTask : System.Web.UI.Page
{
    string strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strProjectID = Request.QueryString["ProjectID"];
        LB_ProjectID.Text = strProjectID.Trim();
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            LoadProjectPrimaveraTaskList(strProjectID.Trim());
        }
    }

    protected void LoadProjectPrimaveraTaskList(string strprojectId)
    {
        string strHQL;
        IList lst;
        strHQL = "from ProjectPrimaveraTask as projectPrimaveraTask where projectPrimaveraTask.ProjectID = '" + strprojectId.Trim() + "' order by projectPrimaveraTask.ID DESC";

        ProjectPrimaveraTaskBLL projectPrimaveraTaskBLL = new ProjectPrimaveraTaskBLL();

        lst = projectPrimaveraTaskBLL.GetAllProjectPrimaveraTasks(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From ProjectPrimaveraTask as projectPrimaveraTask where projectPrimaveraTask.ID = '" + strId + "'";
                ProjectPrimaveraTaskBLL projectPrimaveraTaskBLL = new ProjectPrimaveraTaskBLL();
                lst = projectPrimaveraTaskBLL.GetAllProjectPrimaveraTasks(strHQL);

                ProjectPrimaveraTask projectPrimaveraTask = (ProjectPrimaveraTask)lst[0];

                TB_TaskCode.Text = projectPrimaveraTask.TaskCode.Trim();
                TB_TaskName.Text = projectPrimaveraTask.TaskName.Trim();
                LB_ID.Text = projectPrimaveraTask.ID.ToString().Trim();
                DLC_BeginDate.Text = projectPrimaveraTask.BeginDate.ToString("yyyy-MM-dd");
                DLC_EndDate.Text = projectPrimaveraTask.EndDate.ToString("yyyy-MM-dd");

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strCode = strId;
                strHQL = "Delete From T_ProjectPrimaveraTask Where ID = '" + strCode + "' ";

                try
                {
                    ShareClass.RunSqlCommand(strHQL);
                    LoadProjectPrimaveraTaskList(strProjectID.Trim());

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    /// <summary>
    /// 新增或更新时，检查作业编码是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsProjectPrimaveraTask(string strCode, string strId)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strId))
        {
            strHQL = "Select ID From T_ProjectPrimaveraTask Where TaskCode='" + strCode + "'";
        }
        else
            strHQL = "Select ID From T_ProjectPrimaveraTask Where TaskCode='" + strCode + "' and ID<>'" + strId + "'";

        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectPrimaveraTask").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    /// <summary>
    /// 新增时，获取表T_ProjectPrimaveraTask中最大编号
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetProjectPrimaveraTaskID()
    {
        string flag = string.Empty;
        string strHQL = "Select ID From T_ProjectPrimaveraTask Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectPrimaveraTask").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["ID"].ToString().Trim();
        }
        else
        {
            return "0";
        }
    }


    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddTask();
        }
        else
        {
            UpdateTask();
        }
    }

    protected void AddTask()
    {
        if (TB_TaskCode.Text.Trim() == ""||TB_TaskName.Text.Trim()==""||DLC_EndDate.Text.Trim()==""||DLC_BeginDate.Text.Trim()=="")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYBMZYMCJZYYXDBNWKCZSBJC").ToString().Trim()+"')", true);
            TB_TaskCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (IsProjectPrimaveraTask(TB_TaskCode.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYBMYCZCZSBJC").ToString().Trim()+"')", true);
            TB_TaskCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        ProjectPrimaveraTaskBLL projectPrimaveraTaskBLL = new ProjectPrimaveraTaskBLL();
        ProjectPrimaveraTask projectPrimaveraTask = new ProjectPrimaveraTask();

        projectPrimaveraTask.BeginDate = DateTime.Parse(DLC_BeginDate.Text.Trim());
        projectPrimaveraTask.CreateDate = DateTime.Now;
        projectPrimaveraTask.EndDate = DateTime.Parse(DLC_EndDate.Text.Trim());
        projectPrimaveraTask.ProjectID = int.Parse(strProjectID.Trim());
        projectPrimaveraTask.ProjGuid = "";
        projectPrimaveraTask.TaskCode = TB_TaskCode.Text.Trim();
        projectPrimaveraTask.TaskGuid = "";
        projectPrimaveraTask.TaskName = TB_TaskName.Text.Trim();

        if (projectPrimaveraTask.BeginDate > projectPrimaveraTask.EndDate)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYYXDKSSJBNDYJSSJCZSBJC").ToString().Trim()+"')", true);
            DLC_BeginDate.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        try
        {
            projectPrimaveraTaskBLL.AddProjectPrimaveraTask(projectPrimaveraTask);
            LB_ID.Text = GetProjectPrimaveraTaskID();

            LoadProjectPrimaveraTaskList(strProjectID.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateTask()
    {
        if (TB_TaskCode.Text.Trim() == "" || TB_TaskName.Text.Trim() == "" || DLC_EndDate.Text.Trim() == "" || DLC_BeginDate.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYBMZYMCJZYYXDBNWKCZSBJC").ToString().Trim()+"')", true);
            TB_TaskCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (IsProjectPrimaveraTask(TB_TaskCode.Text.Trim(),LB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYBMYCZCZSBJC").ToString().Trim()+"')", true);
            TB_TaskCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        string strHQL = "From ProjectPrimaveraTask as projectPrimaveraTask where projectPrimaveraTask.ID = '" + LB_ID.Text.Trim() + "'";
        ProjectPrimaveraTaskBLL projectPrimaveraTaskBLL = new ProjectPrimaveraTaskBLL();
        IList lst = projectPrimaveraTaskBLL.GetAllProjectPrimaveraTasks(strHQL);

        ProjectPrimaveraTask projectPrimaveraTask = (ProjectPrimaveraTask)lst[0];

        projectPrimaveraTask.BeginDate = DateTime.Parse(DLC_BeginDate.Text.Trim());
        projectPrimaveraTask.EndDate = DateTime.Parse(DLC_EndDate.Text.Trim());
        projectPrimaveraTask.TaskCode = TB_TaskCode.Text.Trim();
        projectPrimaveraTask.TaskName = TB_TaskName.Text.Trim();

        if (projectPrimaveraTask.BeginDate > projectPrimaveraTask.EndDate)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYYXDKSSJBNDYJSSJCZSBJC").ToString().Trim()+"')", true);
            DLC_BeginDate.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        try
        {
            projectPrimaveraTaskBLL.UpdateProjectPrimaveraTask(projectPrimaveraTask, projectPrimaveraTask.ID);
            LoadProjectPrimaveraTaskList(strProjectID.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim() +"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

}