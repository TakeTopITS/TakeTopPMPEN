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

public partial class TTTaskTestCase : System.Web.UI.Page
{
    string  strIsMobileDevice;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        string strHQL;
        IList lst;

        string strTaskID = Request.QueryString["TaskID"];
        string strProjectID = Request.QueryString["ProjectID"];
        if (strProjectID == null)
        {
            strProjectID = GetProjectIDByTaskID(strTaskID);
        }

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_OperatorCommand);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_OperatorCommand.Visible = true;
            }
            else
            {
                HE_OperatorCommand.Visible = true;
            }

            strHQL = "from TaskTestCase as taskTestCase where taskTestCase.TaskID = " + strTaskID;
            TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
            lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_TaskID.Text = strTaskID;
            LB_ProjectID.Text = strProjectID;
            LB_UserCode.Text = strUserCode;
        }
    }

    //依任务ID取得项目ID
    protected string GetProjectIDByTaskID(string strTaskID)
    {
        string strHQL;

        strHQL = "Select ProjectID From T_ProjectTask Where TaskID =" + strTaskID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strID, strTaskID, strCaseName, strDescription, strRequisite, strStatus;
        string strOperatorCode;
        string strHQL;
        IList lst;

        strTaskID = LB_TaskID.Text.Trim();
        strOperatorCode = LB_UserCode.Text.Trim();


        strID = LB_CaseID.Text.Trim();
        strCaseName = LB_CaseName.Text.Trim();
        strDescription = LB_Description.Text.Trim();
        strRequisite = LB_Requisite.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        strHQL = "from TaskTestCase as taskTestCase where taskTestCase.ID = " + strID;
        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);
        TaskTestCase taskTestCase = (TaskTestCase)lst[0];
        taskTestCase.Status = strStatus;

        try
        {
            taskTestCaseBLL.UpdateTaskTestCase(taskTestCase, int.Parse(strID));

            TaskTestRecordBLL taskTestRecordBLL = new TaskTestRecordBLL();
            TaskTestRecord taskTestRecord = new TaskTestRecord();


            if (strIsMobileDevice == "YES")
            {
                taskTestRecord.Command = HT_OperatorCommand.Text;
            }
            else
            {
                taskTestRecord.Command = HE_OperatorCommand.Text;
              
            }

            taskTestRecord.TaskID = int.Parse(strTaskID);
            taskTestRecord.TestCaseID = int.Parse(strID);
            taskTestRecord.TesterCode = strOperatorCode;
            taskTestRecord.TesterName = ShareClass.GetUserName(strOperatorCode);
            taskTestRecord.TestTime = DateTime.Now;
            taskTestRecord.Status = DL_Status.SelectedValue;

            taskTestRecordBLL.AddTaskTestRecord(taskTestRecord);

            LoadTaskTestCase(strTaskID);
            LoadTaskTestRecord(strID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;
           
            string strTaskID = LB_TaskID.Text.Trim();

            string strID = e.Item.Cells[1].Text;

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
                LB_CaseName.Text = taskTestCase.CaseName.Trim();
                LB_Description.Text = taskTestCase.Description.Trim();
                LB_Requisite.Text = taskTestCase.Requisite.Trim();
                DL_Status.SelectedValue = taskTestCase.Status.Trim();

                LoadTaskTestRecord(strID);

                BT_Update.Enabled = true;

                HL_TaskRelatedDoc.Enabled = true;
                HL_TaskRelatedDoc.NavigateUrl = "TTProTaskRelatedDoc.aspx?TaskID=" + strTaskID;

                LB_TestCase.Text = taskTestCase.ID + " " + taskTestCase.CaseName;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
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

    protected void LoadTaskTestCase(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskTestCase as taskTestCase where taskTestCase.TaskID = " + strTaskID + " Order by taskTestCase.TaskID ASC";
        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadTaskTestRecord(string strTestCaseID)
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskTestRecord as taskTestRecord where taskTestRecord.TestCaseID = " + strTestCaseID + " Order by taskTestRecord.ID DESC";
        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }
}
