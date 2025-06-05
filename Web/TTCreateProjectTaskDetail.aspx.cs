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

public partial class TTCreateProjectTaskDetail : System.Web.UI.Page
{
    string strTaskID, strTaskName, strTaskStatus;
    string strUserCode;
    string strIsMobildeDevice;

    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectID, strMeetingID, strCoID;

        strUserCode = Session["UserCode"].ToString();

        strLangCode = Session["LangCode"].ToString();
        strIsMobildeDevice = Session["IsMobileDevice"].ToString();

        strTaskID = Request.QueryString["TaskID"];
        strTaskName = GetProjectTaskName(strTaskID);

        strProjectID = GetTaskRelatedProjectID(strTaskID);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            if (strIsMobildeDevice == "YES")
            {
                HT_Operation.Visible = true; HT_Operation.Toolbar = "";
            }
            else
            {
                 HE_Operation.Visible = true; 
            }

            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_TaskBegin.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_TaskEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LB_UserCode.Text = strUserCode;
            LB_ProjectID.Text = strProjectID;

            ShareClass.LoadTaskType(DL_Type);

            strHQL = "from TaskRecordType as taskRecordType order by taskRecordType.SortNumber ASC";
            TaskRecordTypeBLL taskRecordTypeBLL = new TaskRecordTypeBLL();
            lst = taskRecordTypeBLL.GetAllTaskRecordTypes(strHQL);

            DL_RecordType.DataSource = lst;
            DL_RecordType.DataBind();

            ShareClass.LoadTaskStatus(DL_Status, strLangCode);

            ShareClass.LoadTaskWorkRequest(DL_WorkRequest);

            LoadProjectTask(strTaskID);

            strHQL = "from ProjectTask as ProjectTask where ProjectTask.TaskID= " + strTaskID;
            ProjectTaskBLL ProjectTaskBLL = new ProjectTaskBLL();
            lst = ProjectTaskBLL.GetAllProjectTasks(strHQL);
            ProjectTask projectTask = (ProjectTask)lst[0];

            strMeetingID = projectTask.MeetingID.ToString();
            if (strMeetingID != "0")
            {
                LoadMeetingMember(strMeetingID);
                return;
            }

            strCoID = projectTask.CollaborationID.ToString();
            if (strCoID != "0")
            {
                LoadCollaborationMember(strCoID);
                return;
            }

            ShareClass.LoadProjectMemberAndDirectMember(strProjectID, strUserCode, DL_OperatorCode);
        }
    }

    protected void LoadProjectTask(string strTaskID)
    {
        string strHQL;
        IList lst;

        string strProjectID = LB_ProjectID.Text.Trim();

        strHQL = "from ProjectTask as ProjectTask where ProjectTask.TaskID= " + strTaskID;
        ProjectTaskBLL ProjectTaskBLL = new ProjectTaskBLL();
        lst = ProjectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = (ProjectTask)lst[0];

        LB_TaskNO.Text = projectTask.TaskID.ToString();
        try
        {
            DL_Type.SelectedValue = projectTask.Type.Trim();
        }
        catch
        {
        }
        try
        {
            DL_Type.SelectedValue = projectTask.Type;
        }
        catch
        {
        }
        TB_Task.Text = projectTask.Task;
        DLC_BeginDate.Text = projectTask.BeginDate.ToString("yyyy-MM-dd");
        DLC_EndDate.Text = projectTask.EndDate.ToString("yyyy-MM-dd");
        TB_Budget.Amount = projectTask.Budget;
        LB_MakeDate.Text = projectTask.MakeDate.ToString();
        DL_Status.SelectedValue = projectTask.Status;
        TB_Expense.Amount = projectTask.Expense;
        TB_FinishPercent.Amount = projectTask.FinishPercent;
        DL_Priority.SelectedValue = projectTask.Priority.Trim();
        NB_ManHour.Amount = projectTask.ManHour;
        NB_RealManHour.Amount = projectTask.RealManHour;

        strTaskStatus = projectTask.Status.Trim();
        LB_Status.Text = strTaskStatus;

        LB_TaskName.Visible = true;
        LB_TaskName.Text = LanguageHandle.GetWord("RenWu") + projectTask.TaskID.ToString().Trim() + "  " + projectTask.Task.Trim() + LanguageHandle.GetWord("DeFenPaJiLu");

        LoadAssignRecord(strTaskID);

        HL_TaskRelatedDoc.Enabled = true;
        HL_TaskRelatedDoc.NavigateUrl = "TTProTaskRelatedDoc.aspx?TaskID=" + strTaskID;
        HL_TestCase.Enabled = true;
        HL_TestCase.NavigateUrl = "TTMakeTaskTestCase.aspx?TaskID=" + strTaskID + "&ProjectID=" + strProjectID;
        HL_TaskReview.Enabled = true;
        HL_TaskReview.NavigateUrl = "TTProjectTaskReviewWL.aspx?TaskID=" + strTaskID;
        HL_RelatedWorkFlowTemplate.Enabled = true;
        HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=ProjectTask&RelatedID=" + strTaskID;
        HL_ActorGroup.Enabled = true;
        HL_ActorGroup.NavigateUrl = "TTRelatedActorGroup.aspx?RelatedType=ProjectTask&RelatedID=" + strTaskID;
        HL_WLTem.Enabled = true;
        HL_WLTem.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=ProjectTask&RelatedID=" + strTaskID;
        HL_RunTaskByWF.Enabled = true;
        HL_RunTaskByWF.NavigateUrl = "TTRelatedDIYWorkflowForm.aspx?RelatedType=ProjectTask&RelatedID=" + strTaskID;

        if (strTaskStatus == "Closed")
        {
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;
            BT_Assign.Enabled = false;
        }
        else
        {
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
            BT_Assign.Enabled = true;
        }

        BT_Close.Enabled = true;
        BT_Active.Enabled = true;

    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[0].Text;
            IList lst;
            string strHQL;

            SetTaskAssignRecordColor();

            e.Item.ForeColor = Color.Green;

            strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
            TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
            TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
            lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

            taskAssignRecord = (TaskAssignRecord)lst[0];

            LB_ID.Text = taskAssignRecord.ID.ToString();

            try
            {
                DL_OperatorCode.Items[0].Text = taskAssignRecord.OperatorName;
                DL_OperatorCode.Items[0].Value = taskAssignRecord.OperatorCode;
            }
            catch
            {
            }

            try
            {
                DL_RecordType.Items[0].Value = taskAssignRecord.Type;
                DL_RecordType.Items[0].Text = taskAssignRecord.Type;
            }
            catch
            {
            }

            if (strIsMobildeDevice == "YES")
            {
                HT_Operation.Text = taskAssignRecord.Operation.Trim();
            }
            else
            {
                HE_Operation.Text = taskAssignRecord.Operation.Trim();
            }
            DLC_TaskBegin.Text = taskAssignRecord.BeginDate.ToString("yyyy-MM-dd");
            DLC_TaskEnd.Text = taskAssignRecord.EndDate.ToString("yyyy-MM-dd");

            strTaskStatus = LB_Status.Text.Trim();

            if (strTaskStatus == "Closed")
            {
                BT_UpdateAssign.Enabled = false;
                BT_DeleteAssign.Enabled = false;
                BT_Assign.Enabled = false;
            }
            else
            {
                BT_UpdateAssign.Enabled = true;
                BT_DeleteAssign.Enabled = true;
                BT_Assign.Enabled = true;
            }
        }
    }


    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strTaskID, strProjectID, strTask, strBeginDate;
        string strStatus, strEndDate, strMakeDate;
        string strOperatorCode, strHQL, strPriority, strType;
        string strBudget;
        decimal deManHour, deRealManHour;
        IList lst;
        decimal deFinishPercent;

        strTaskID = LB_TaskNO.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strOperatorCode = DL_OperatorCode.SelectedValue;
        strProjectID = LB_ProjectID.Text.Trim();
        strTask = TB_Task.Text.Trim();
        strBeginDate = DLC_BeginDate.Text;
        strEndDate = DLC_EndDate.Text;
        strMakeDate = LB_MakeDate.Text.Trim();
        strBudget = TB_Budget.Amount.ToString();
        deManHour = NB_ManHour.Amount;
        deRealManHour = NB_RealManHour.Amount;
        strStatus = DL_Status.SelectedValue;
        deFinishPercent = TB_FinishPercent.Amount;
        strPriority = DL_Priority.SelectedValue.Trim();

        if (strTaskID != "")
        {
            ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
            ProjectTask projectTask = new ProjectTask();
            strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
            lst = projectTaskBLL.GetAllProjectTasks(strHQL);
            projectTask = (ProjectTask)lst[0];

            projectTask.Type = strType;
            projectTask.ProjectID = int.Parse(strProjectID);
            projectTask.Task = strTask;
            projectTask.BeginDate = DateTime.Parse(strBeginDate);
            projectTask.EndDate = DateTime.Parse(strEndDate);
            projectTask.MakeDate = DateTime.Parse(strMakeDate);
            projectTask.Status = strStatus;
            projectTask.Budget = decimal.Parse(strBudget);
            projectTask.ManHour = deManHour;
            projectTask.FinishPercent = deFinishPercent;
            projectTask.Priority = strPriority;

            try
            {
                projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));

                string strPlanID = projectTask.PlanID.ToString();
                if (projectTask.PlanID > 0)
                {
                    strHQL = "update T_ImplePlan Set Percent_Done = " + deFinishPercent.ToString();
                    strHQL += ",ActualHour = " + ShareClass.GetTotalRealManHourByPlan(strPlanID);
                    strHQL += ",Expense = " + ShareClass.GetTotalRealExpenseByPlan(strPlanID);
                    strHQL += " Where PlanID = " + strPlanID;
                    ShareClass.RunSqlCommand(strHQL);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXCCJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXJHBNXG") + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strTaskID, strPlanID;
        string strHQL;

        strTaskID = LB_TaskNO.Text.Trim();

        ProjectTask projectTask = new ProjectTask();
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();

        projectTask.TaskID = int.Parse(strTaskID);

        try
        {
            strPlanID = projectTask.PlanID.ToString();

            projectTaskBLL.DeleteProjectTask(projectTask);

            if (projectTask.PlanID > 0)
            {
                strHQL = "update T_ImplePlan Set Percent_Done = " + ShareClass.GetTaskOrWorkflowPlanProgress(strPlanID);
                strHQL += ",ActualHour = " + ShareClass.GetTotalRealManHourByPlan(strPlanID);
                strHQL += ",Expense = " + ShareClass.GetTotalRealExpenseByPlan(strPlanID);
                strHQL += " Where PlanID = " + strPlanID;
                ShareClass.RunSqlCommand(strHQL);
            }


        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
        }
    }

    protected void BT_Close_Click(object sender, EventArgs e)
    {
        string strTaskID = LB_TaskNO.Text.Trim();

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        ProjectTask projectTask = new ProjectTask();
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        projectTask = (ProjectTask)lst[0];

        projectTask.Status = "Closed";

        try
        {
            projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));


            BT_Assign.Enabled = false;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;
            BT_DeleteAssign.Enabled = false;
            BT_UpdateAssign.Enabled = false;
            BT_Assign.Enabled = false;

            LB_Status.Text = "Closed";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBRWSBJC") + "')", true);
        }
    }

    protected void BT_Active_Click(object sender, EventArgs e)
    {
        string strTaskID = LB_TaskNO.Text.Trim();

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        ProjectTask projectTask = new ProjectTask();
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        projectTask = (ProjectTask)lst[0];

        projectTask.Status = "InProgress";

        try
        {
            projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHCG") + "')", true);

            BT_Assign.Enabled = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            LB_Status.Text = "InProgress";
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHRWSBJC") + "')", true);
        }
    }


    protected void BT_Assign_Click(object sender, EventArgs e)
    {
        int intTaskID, intPriorID;
        string strTask, strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;
        string strContent, strOperation, strType;
        DateTime dtBeginDate, dtEndDate, dtMakeDate;


        strUserCode = LB_UserCode.Text.Trim();

        intTaskID = int.Parse(LB_TaskNO.Text.Trim());
        strType = DL_RecordType.SelectedValue.Trim();
        strTask = TB_Task.Text.Trim();
        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();
        strOperatorName = ShareClass.GetUserName(strOperatorCode);
        strAssignManCode = LB_UserCode.Text.Trim();
        strAssignManName = ShareClass.GetUserName(strAssignManCode);

        if (strIsMobildeDevice == "YES")
        {
            strOperation = HT_Operation.Text.Trim();
        }
        else
        {
            strOperation = HE_Operation.Text.Trim();
        }
        intPriorID = 0;
        dtBeginDate = DateTime.Parse(DLC_TaskBegin.Text);
        dtEndDate = DateTime.Parse(DLC_TaskEnd.Text);
        dtMakeDate = DateTime.Now;

        if (strOperation == "" | strOperatorCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBGZYHSLRBNWKJC") + "')", true);
            return;
        }

        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();

        taskAssignRecord.TaskID = intTaskID;
        taskAssignRecord.Type = strType;
        taskAssignRecord.Task = strTask;
        taskAssignRecord.OperatorCode = strOperatorCode;
        taskAssignRecord.OperatorName = strOperatorName;
        taskAssignRecord.OperatorContent = " ";
        taskAssignRecord.OperationTime = DateTime.Now;
        taskAssignRecord.BeginDate = dtBeginDate;
        taskAssignRecord.EndDate = dtEndDate;
        taskAssignRecord.AssignManCode = strAssignManCode;
        taskAssignRecord.AssignManName = strAssignManName;
        taskAssignRecord.Content = "";
        taskAssignRecord.Operation = strOperation;
        taskAssignRecord.PriorID = intPriorID;
        taskAssignRecord.RouteNumber = GetRouteNumber(intTaskID.ToString());
        taskAssignRecord.MakeDate = dtMakeDate;
        taskAssignRecord.Status = "ToHandle";

        taskAssignRecord.FinishedNumber = 0;
        taskAssignRecord.UnitName = ""; 
        taskAssignRecord.MoveTime = DateTime.Now;

        try
        {
            taskAssignRecordBLL.AddTaskAssignRecord(taskAssignRecord);
            LoadAssignRecord(LB_TaskNO.Text.Trim());
            UpdateTaskStatus(intTaskID.ToString(), "InProgress");

            ShareClass.SendInstantMessage(LanguageHandle.GetWord("RenWuFenPaiTongZhi"), ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeRenWu") + " :" + intTaskID.ToString() + "  " + strTask + "£¬" + LanguageHandle.GetWord("QingJiShiChuLi"), strUserCode, strOperatorCode);

            TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeRenWu") + " :" + intTaskID.ToString() + "  " + "£¬" + LanguageHandle.GetWord("QingJiShiChuLi");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPCG") + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC") + "')", true);
        }
    }


    protected void BT_DeleteAssign_Click(object sender, EventArgs e)
    {
        string strHQL, strID;
        IList lst;

        strID = LB_ID.Text.Trim();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        taskAssignRecord = (TaskAssignRecord)lst[0];

        try
        {
            taskAssignRecordBLL.DeleteTaskAssignRecord(taskAssignRecord);
            LoadAssignRecord(LB_TaskNO.Text.Trim());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void BT_UpdateAssign_Click(object sender, EventArgs e)
    {
        string strHQL, strID;
        IList lst;

        strID = LB_ID.Text.Trim();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        taskAssignRecord = (TaskAssignRecord)lst[0];

        taskAssignRecord.Type = DL_RecordType.SelectedValue.Trim();
        taskAssignRecord.Content = "";

        if (strIsMobildeDevice == "YES")
        {
            taskAssignRecord.Operation = HT_Operation.Text.Trim();
        }
        else
        {
            taskAssignRecord.Operation = HE_Operation.Text.Trim();
        }
        taskAssignRecord.OperatorCode = DL_OperatorCode.SelectedValue;
        taskAssignRecord.OperatorName = ShareClass.GetUserName(DL_OperatorCode.SelectedValue);
        taskAssignRecord.BeginDate = DateTime.Parse(DLC_TaskBegin.Text);
        taskAssignRecord.EndDate = DateTime.Parse(DLC_TaskEnd.Text);

        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));
            LoadAssignRecord(LB_TaskNO.Text.Trim());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg, strOperatorCode;
        Msg msg = new Msg();

        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();

        if (CB_SendMsg.Checked == true | CB_SendMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("RenWuFenPaiTongZhi");
            strMsg = TB_Message.Text.Trim();

            if (CB_SendMsg.Checked == true)
            {
                msg.SendMSM("Message",strOperatorCode, strMsg, strUserCode);
            }

            if (CB_SendMail.Checked == true)
            {
                msg.SendMail(strOperatorCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB") + "')", true);
    }

    protected void SetTaskAssignRecordColor()
    {
        int i;
        DateTime dtNowDate, dtFinishedDate;
        string strStatus;

        for (i = 0; i < DataGrid2.Items.Count; i++)
        {
            dtFinishedDate = DateTime.Parse(DataGrid2.Items[i].Cells[7].Text.Trim());
            dtNowDate = DateTime.Now;
            strStatus = DataGrid2.Items[i].Cells[9].Text.Trim();

            if (strStatus != "Completed" & strStatus != LanguageHandle.GetWord("YiWanCheng"))
            {
                if (dtFinishedDate < dtNowDate)
                {
                    DataGrid2.Items[i].ForeColor = Color.Red;
                }
            }
        }
    }

    protected void LoadAssignRecord(string strTaskID)
    {
        string strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.TaskID = " + strTaskID + " Order by taskAssignRecord.ID Desc";
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        SetTaskAssignRecordColor();
    }

    protected void LoadMeetingMember(string strMeetingID)
    {
        string strHQL;
        IList lst;

        strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID=" + strMeetingID;
        MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
        lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);

        DL_OperatorCode.DataSource = lst;
        DL_OperatorCode.DataBind();
    }

    protected void LoadCollaborationMember(string strCollaborationID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CollaborationMember as collaborationMember where collaborationMember.CoID=" + strCollaborationID;
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        lst = collaborationMemberBLL.GetAllCollaborationMembers(strHQL);

        DL_OperatorCode.DataSource = lst;
        DL_OperatorCode.DataBind();
    }

    protected void LoadProjectMember(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ProjectID = " + strProjectID;
        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

        DL_OperatorCode.DataSource = lst;
        DL_OperatorCode.DataBind();
    }

    protected void UpdateTaskStatus(string strTaskID, string strStatus)
    {
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        projectTask.Status = strStatus;

        projectTaskBLL.UpdateProjectTask(projectTask, projectTask.TaskID);
    }

    protected void DL_WorkRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWorkRequest = DL_WorkRequest.SelectedValue.Trim();

        if (strIsMobildeDevice == "YES")
        {
            HT_Operation.Text = strWorkRequest;
        }
        else
        {
            HE_Operation.Text = strWorkRequest;
        }
    }

    protected string GetTaskRelatedProjectID(string strTaskID)
    {
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.ProjectID.ToString();
    }

    protected string GetProjectTaskName(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.Task.Trim();
    }


    protected int GetRouteNumber(string strTaskID)
    {
        string strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.TaskID = " + strTaskID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        return lst.Count + 1;
    }

}
