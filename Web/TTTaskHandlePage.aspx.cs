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
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTTaskHandlePage : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ҵ�����", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        if (Page.IsPostBack != true)
        {
            string strSystemVersionType = Session["SystemVersionType"].ToString();
            string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
            {
                HL_CreateTask.NavigateUrl = "TTCreateOtherTaskSAAS.aspx";
            }
            else
            {
                HL_CreateTask.NavigateUrl = "TTCreateOtherTask.aspx";
            }

            try
            {
                LoadProjectTaskAssignRecord(strUserCode, strProjectID);
                LoadProjectTask(strUserCode, strProjectID);

            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    protected void BT_CreateTask_Click(object sender, EventArgs e)
    {
        string strURL;

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            strURL = "popShowByURL('TTCreateOtherTaskSAAS.aspx'title, 800, 600,window.location);";
        }
        else
        {
            strURL = "popShowByURL('TTCreateOtherTask.aspx'title, 800, 600,window.location);";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop12", strURL, true);
    }

    protected void BT_UpdateStatus_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strStatus;

        try
        {
            strID = LB_SourceID.Value;
            strStatus = LB_TargetStatus.Value;

            strHQL = "Update T_TaskAssignRecord Set Status =  '" + strStatus + "',MoveTime = now() Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadProjectTaskAssignRecord(strUserCode, strProjectID);
        }
        catch
        {
        }
    }

    protected void LoadProjectTaskAssignRecord(string strUserCode, string strProjectID)
    {
        string strHQL;
        DataSet ds;

        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();

        strHQL = "Select * from T_TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and taskAssignRecord.Status in ('Plan','Accepted','ToHandle')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from T_Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.MoveTime DESC limit 40";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");
        DataList_ToBeHandled.DataSource = ds;
        DataList_ToBeHandled.DataBind();
        SetTaskRecordColorForDataList(ds, DataList_ToBeHandled, "ToHandle");

        strHQL = "Select * from T_TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and taskAssignRecord.Status in ('InProgress','InProgress')"; 
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from T_Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.MoveTime DESC limit 40";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");
        DataList_Handling.DataSource = ds;
        DataList_Handling.DataBind();
        SetTaskRecordColorForDataList(ds, DataList_Handling, "InProgress");

        strHQL = "Select * from T_TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and taskAssignRecord.Status in ('Rejected','Suspended','Cancel','Completed','Completed')";     
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask  where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from T_Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.MoveTime DESC limit 40";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");
        DataList_FinishedUnAssigned.DataSource = ds;
        DataList_FinishedUnAssigned.DataBind();
        SetTaskRecordColorForDataList(ds, DataList_FinishedUnAssigned, "Completed");   

        strHQL = "Select * from T_TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        //strHQL += " and (taskAssignRecord.ID in (select taskAssignRecord.PriorID from T_TaskAssignRecord as taskAssignRecord) and  taskAssignRecord.Status in ('Rejected','Suspended','Cancel','Plan','Accepted','ToHandle','InProgress','InProgress','Completed','Completed','Assigned'))";
        strHQL += " and taskAssignRecord.Status = 'Assigned'";   
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from T_ProjectTask as projectTask where (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from T_Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by taskAssignRecord.MoveTime DESC limit 40";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");
        DataList_Assigned.DataSource = ds;
        DataList_Assigned.DataBind();
        SetTaskRecordColorForDataList(ds, DataList_Assigned, "Assigned");   
    }

    protected void RP_ToBeHandled_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        
    }

    protected void RP_Handling_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
    }

    protected void RP_Finished_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
    }

    protected void RP_ToBeAssigned_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text;

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        FinishPercentPicture4();
        SetProTaskColorForDataGrid(DataGrid4);

        LoadProjectTaskAssignRecord(strUserCode, strProjectID);
    }

    protected void DataGrid6_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid6.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql6.Text;

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();

        FinishPercentPicture6();
        SetProTaskColorForDataGrid(DataGrid6);

        LoadProjectTaskAssignRecord(strUserCode, strProjectID);
    }

    protected void LoadProjectTask(string strUserCode, string strProjectID)
    {
        string strHQL;
        IList lst;

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();

        strHQL = "from ProjectTask as projectTask where projectTask.MakeManCode = " + "'" + strUserCode + "'";
        strHQL += " and ( (projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " Order by projectTask.TaskID DESC";
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
        SetProTaskColorForDataGrid(DataGrid4);
        LB_Sql4.Text = strHQL;
        LB_TotalNumber4.Text = LanguageHandle.GetWord("JiLuShu") + lst.Count.ToString();

        strHQL = "from ProjectTask as projectTask where projectTask.MakeManCode = " + "'" + strUserCode + "'";
        strHQL += " and ((projectTask.ProjectID = 1) or (projectTask.ProjectID in (select project.ProjectID from Project as project where project.Status not in ('New','Hided','Deleted','Archived'))))";
        strHQL += " and projectTask.TaskID not in (Select taskAssignRecord.TaskID From TaskAssignRecord as taskAssignRecord)";
        strHQL += " Order by projectTask.TaskID DESC";
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();
        SetProTaskColorForDataGrid(DataGrid6);
        LB_Sql6.Text = strHQL;
        LB_TotalNumber6.Text = LanguageHandle.GetWord("JiLuShu") + lst.Count.ToString();

        FinishPercentPicture4();
        FinishPercentPicture6();
    }

    protected void FinishPercentPicture4()
    {
        string strProjectID;
        double decFinishPercent;
        int intWidth;
        int i;

        for (i = 0; i < DataGrid4.Items.Count; i++)
        {
            strProjectID = DataGrid4.Items[i].Cells[0].Text.Trim();

            string strWidth = ((System.Web.UI.WebControls.Label)DataGrid4.Items[i].FindControl("LB_FinishPercent")).Text;

            try
            {
                decFinishPercent = double.Parse((((System.Web.UI.WebControls.Label)DataGrid4.Items[i].FindControl("LB_FinishPercent")).Text));
                intWidth = int.Parse(decFinishPercent.ToString());
            }
            catch
            {
                intWidth = 0;
            }

            if (intWidth > 25)
            {
                ((System.Web.UI.WebControls.Label)DataGrid4.Items[i].FindControl("LB_FinishPercent")).Width = Unit.Pixel(intWidth);
            }
            ((System.Web.UI.WebControls.Label)DataGrid4.Items[i].FindControl("LB_FinishPercent")).BackColor = Color.YellowGreen;
            ((System.Web.UI.WebControls.Label)DataGrid4.Items[i].FindControl("LB_FinishPercent")).Text = intWidth.ToString() + "%";
        }
    }

    protected void FinishPercentPicture6()
    {
        string strProjectID;
        double decFinishPercent;
        int intWidth;
        int i;

        for (i = 0; i < DataGrid6.Items.Count; i++)
        {
            strProjectID = DataGrid6.Items[i].Cells[0].Text.Trim();

            try
            {
                decFinishPercent = double.Parse((((System.Web.UI.WebControls.Label)DataGrid6.Items[i].FindControl("LB_FinishPercent")).Text));
                intWidth = int.Parse(decFinishPercent.ToString());
            }
            catch
            {
                intWidth = 0;
            }

            if (intWidth > 25)
            {
                ((System.Web.UI.WebControls.Label)DataGrid6.Items[i].FindControl("LB_FinishPercent")).Width = Unit.Pixel(intWidth);
            }
            ((System.Web.UI.WebControls.Label)DataGrid6.Items[i].FindControl("LB_FinishPercent")).BackColor = Color.YellowGreen;
            ((System.Web.UI.WebControls.Label)DataGrid6.Items[i].FindControl("LB_FinishPercent")).Text = intWidth.ToString() + "%";
        }
    }

    protected void SetTaskRecordColorForDataList(DataSet ds, DataList dataList, string strTaskStatus)
    {
        int i;
        DateTime dtNowDate, dtFinishedDate;
        string strStatus;

        //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        //{
        //    dtFinishedDate = DateTime.Parse(ds.Tables[0].Rows[i]["EndDate"].ToString());

        //    dtNowDate = DateTime.Now;

        //    strStatus = ds.Tables[0].Rows[i]["Status"].ToString().Trim();

        //    if (strStatus != "Completed" & strStatus != "Completed")
        //    {
        //        if (strTaskStatus == "ToHandle")
        //        {
        //            ((Label)(dataList.Items[i].FindControl("LB_ID"))).ForeColor = Color.Gray;
        //            ((Label)(dataList.Items[i].FindControl("LB_Status"))).ForeColor = Color.Gray;

                  
        //        }
        //        else if(strTaskStatus == "InProgress")
        //        {
        //            ((Label)(dataList.Items[i].FindControl("LB_ID"))).ForeColor = Color.Red;
        //            ((Label)(dataList.Items[i].FindControl("LB_Status"))).ForeColor = Color.Red;
        //        }
        //        else
        //        {
        //            ((Label)(dataList.Items[i].FindControl("LB_ID"))).ForeColor = Color.Green;
        //            ((Label)(dataList.Items[i].FindControl("LB_Status"))).ForeColor = Color.Green;
        //        }    
        //    }
        //    else
        //    {
        //        if (strTaskStatus == "Assigned")
        //        {
        //            ((Label)(dataList.Items[i].FindControl("LB_ID"))).ForeColor = Color.Green;
        //            ((Label)(dataList.Items[i].FindControl("LB_Status"))).ForeColor = Color.Green;
        //        }
        //        else
        //        {
        //            ((Label)(dataList.Items[i].FindControl("LB_ID"))).ForeColor = Color.Blue;
        //            ((Label)(dataList.Items[i].FindControl("LB_Status"))).ForeColor = Color.Blue;
        //        }
        //    }
        //}
    }

    protected void SetProTaskColorForDataGrid(DataGrid dataGrid)
    {
        int i;
        DateTime dtNowDate, dtFinishedDate;
        string strStatus;

        for (i = 0; i < dataGrid.Items.Count; i++)
        {
            dtFinishedDate = DateTime.Parse(dataGrid.Items[i].Cells[5].Text.Trim());
            dtNowDate = DateTime.Now;
            strStatus = dataGrid.Items[i].Cells[11].Text.Trim();

            if (strStatus != "Completed" | strStatus != "Closed")
            {
                if (dtFinishedDate < dtNowDate)
                {
                    dataGrid.Items[i].ForeColor = Color.Red;
                }
            }
        }
    }

    protected string GetTaskPriority(string strTaskID)
    {
        string strHQL;

        strHQL = "Select Priority From T_ProjectTask Where TaskID = " + strTaskID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }
}
