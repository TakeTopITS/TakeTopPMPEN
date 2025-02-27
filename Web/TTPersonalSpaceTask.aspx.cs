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
using System.Text;

using System.Security.Cryptography;
using System.Security.Permissions;
using Npgsql;//using System.Data.SqlClient;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;
using System.Timers;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTPersonalSpaceTask : System.Web.UI.Page
{
    int intRunNumber;
	
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            //���ҳ�滺�棬���ڸı�Ƥ��
            SetPageNoCache();

            intRunNumber = 0;

            AsyncWork();
        }
    }

    //���ҳ�滺�棬���ڸı�Ƥ��
    public void SetPageNoCache()
    {
        if (Session["CssDirectoryChangeNumber"].ToString() == "1")
        {
            //���ȫ������
            IDictionaryEnumerator allCaches = Page.Cache.GetEnumerator();
            while (allCaches.MoveNext())
            {
                Page.Cache.Remove(allCaches.Key.ToString());
            }

            Page.Response.Buffer = true;
            Page.Response.AddHeader("Pragma", "No-Cache");

            Page.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Page.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Page.Response.Expires = 0;
            Page.Response.CacheControl = "no-cache";
            Page.Response.Cache.SetNoStore();
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (intRunNumber == 0)
        {
            AsyncWork();

            Timer1.Interval = 3600000;

            intRunNumber = 1;
        }
    }

    private void AsyncWork()
    {
        string strUserInfo, strUserName;

        string strUserCode;
        String strLangCode;
        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();

        strUserName = Session["UserName"].ToString();
        strUserInfo = LanguageHandle.GetWord("YongHu").ToString().Trim() + ": " + strUserCode + "  " + strUserName;


        try
        {

            LoadProjectTaskList(strUserCode);

        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());
        }
    }

    protected void LoadProjectTaskList(string strUserCode)
    {
        string strHQL;
        IList lst;

        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and taskAssignRecord.Status in ('Plan','Accepted','InProgress','ToHandle','InProgress') and taskAssignRecord.ID not in (select taskAssignRecord.PriorID from TaskAssignRecord as taskAssignRecord) ";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.MoveTime DESC";
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and taskAssignRecord.Status in ('�ܾ�','Suspended','Cancel','Completed','�����','�ѷ���') and taskAssignRecord.ID not in (select taskAssignRecord.PriorID from TaskAssignRecord as taskAssignRecord) "; 
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask  where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.MoveTime DESC";
        taskAssignRecordBLL = new TaskAssignRecordBLL();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }


    protected string GetTaskPriority(string strTaskID)
    {
        string strHQL;

        strHQL = "Select Priority From T_ProjectTask Where TaskID = " + strTaskID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL;

        string strUserCode;

        strUserCode = Session["UserCode"].ToString();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and taskAssignRecord.Status in ('Plan','Accepted','InProgress') and taskAssignRecord.ID not in (select taskAssignRecord.PriorID from TaskAssignRecord as taskAssignRecord) ";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.ID DESC";

        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;

        string strHQL;

        string strUserCode;

        strUserCode = Session["UserCode"].ToString();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and (taskAssignRecord.Status in ('�ܾ�','Completed','Suspended','Cancel') and taskAssignRecord.ID not in (select taskAssignRecord.PriorID from TaskAssignRecord as taskAssignRecord))"; 
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.ID DESC";

        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }


}
