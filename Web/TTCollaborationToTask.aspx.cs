using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTCollaborationToTask : System.Web.UI.Page
{
    string strTaskStatus;
    string strIsMobileDevice;

    string strLangCode;
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strProjectID = Request.QueryString["ProjectID"];
        string strCollaborationID = Request.QueryString["CollaborationID"];

        string strHQL;
        IList lst;

        string strUserName;

        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/"; Session["PageName"] = "TakeTopSiteContentEdit";
        _FileBrowser.SetupCKEditor(HE_Operation);
HE_Operation.Language = Session["LangCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCollaborationID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();

        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];
        //this.Title = "协作：" + strCollaborationID + collaboration.CollaborationName.Trim() + " 转成任务";

        DataList1.DataSource = lst;
        DataList1.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
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

            ShareClass.LoadTaskType(DL_Type);

            ShareClass.LoadTaskStatus(DL_Status, strLangCode);

            ShareClass.LoadTaskRecordType(DL_RecordType);
            LoadCollaborationMember(strCollaborationID, DL_OperatorCode, strUserCode);

            ShareClass.LoadTaskWorkRequest(DL_WorkRequest);

            TB_Task.Text = collaboration.CollaborationName.Trim();
            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LB_MakeDate.Text = DateTime.Now.ToShortDateString();

            LB_CollaborationID.Text = strCollaborationID;
            LB_ProjectID.Text = strProjectID;

            if (strIsMobileDevice == "YES")
            {
                HT_Operation.Text = LanguageHandle.GetWord("ZhuTi") + collaboration.CollaborationName.Trim() + LanguageHandle.GetWord("brbrNeiRong") + collaboration.Comment.Trim();
            }
            else
            {
                HE_Operation.Text = LanguageHandle.GetWord("ZhuTi") + collaboration.CollaborationName.Trim() + LanguageHandle.GetWord("brbrNeiRong") + collaboration.Comment.Trim();
            }

            LoadProjectTask();

            SetProTaskColor();

            //BusinessForm,列出业务表单类型 
            ShareClass.LoadWorkflowType(DL_WLType, strLangCode);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        //BusinessForm，隐藏业务表单元素
        Panel_RelatedBusiness.Visible = false;

        LB_TaskNO.Text = "";

        TB_Budget.Amount = 0;
        TB_Expense.Amount = 0;
        NB_ManHour.Amount = 0;
        NB_RealManHour.Amount = 0;
        TB_FinishPercent.Amount = 0;


        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strProjectID = LB_ProjectID.Text.Trim();

            string strHQL;
            IList lst;

            string strTaskID = e.Item.Cells[3].Text.Trim();

            if (e.CommandName == "Update" | e.CommandName == "Assign" | e.CommandName == "Other")
            {
                SetProTaskColor();

                e.Item.ForeColor = Color.Green;

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
                try
                {
                    DL_Status.SelectedValue = projectTask.Status;
                }
                catch
                {
                    DL_Status.SelectedValue = projectTask.Status.Trim();
                }
                TB_Expense.Amount = projectTask.Expense;
                TB_FinishPercent.Amount = projectTask.FinishPercent;
                DL_Priority.SelectedValue = projectTask.Priority.Trim();
                NB_ManHour.Amount = projectTask.ManHour;
                NB_RealManHour.Amount = projectTask.RealManHour;

                strTaskStatus = projectTask.Status.Trim();
                LB_Status.Text = strTaskStatus;

                LB_TaskName.Visible = true;
                LB_TaskName.Text = LanguageHandle.GetWord("RenWu") + projectTask.TaskID.ToString().Trim() + "  " + projectTask.Task.Trim() + LanguageHandle.GetWord("DeFenPaJiLu");

                HL_TaskRelatedDoc.Enabled = true;
                HL_TaskRelatedDoc.NavigateUrl = "TTProTaskRelatedDoc.aspx?TaskID=" + strTaskID;
                HL_TestCase.Enabled = true;
                HL_TestCase.NavigateUrl = "TTMakeTaskTestCase.aspx?TaskID=" + strTaskID + "&ProjectID=" + strProjectID;
                HL_AssignRecord.Enabled = true;
                HL_AssignRecord.NavigateUrl = "TTTaskAssignRecord.aspx?TaskID=" + strTaskID;

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

                LoadAssignRecord(strTaskID);

                //BusinessForm，列出关联表单模板
                try
                {
                    Panel_RelatedBusiness.Visible = true;

                    string strTemName;
                    strHQL = "Select * From T_RelatedBusinessForm Where RelatedType = 'Task' and RelatedID = " + strTaskID;
                    strHQL += " Order By CreateTime DESC";

                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessForm");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strTemName = ds.Tables[0].Rows[0]["TemName"].ToString().Trim();

                        DL_WLType.SelectedValue = ShareClass.GetWorkTemplateType(strTemName);
                        ShareClass.LoadWFTemplate(strUserCode, DL_WLType.SelectedValue.Trim(), DL_WFTemplate);
                        DL_WFTemplate.SelectedValue = strTemName;

                        DL_AllowUpdate.SelectedValue = ds.Tables[0].Rows[0]["AllowUpdate"].ToString().Trim();
                    }
                }
                catch
                {
                }

                //BusinessForm,装载关联信息
                TabContainer1.ActiveTabIndex = 0;
                ShareClass.LoadBusinessForm("Task", strTaskID, DL_WFTemplate.SelectedValue.Trim(), IFrame_RelatedInformation);


                if (strTaskStatus == "Closed")
                {
                    BT_Assign.Enabled = false;
                }
                else
                {
                    BT_Assign.Enabled = true;
                }

                BT_Close.Enabled = true;
                BT_Active.Enabled = true;


                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                }

                if (e.CommandName == "Assign")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

                }

                if (e.CommandName == "Other")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popOtherWindow','true') ", true);

                }
            }

            if (e.CommandName == "Delete")
            {
                ProjectTask projectTask = new ProjectTask();
                ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();

                projectTask.TaskID = int.Parse(strTaskID);

                try
                {
                    projectTaskBLL.DeleteProjectTask(projectTask);

                    HL_TaskRelatedDoc.Enabled = false;
                    HL_TestCase.Enabled = false;
                    HL_AssignRecord.Enabled = false;
                    HL_TaskReview.Enabled = false;
                    HL_RelatedWorkFlowTemplate.Enabled = false;
                    HL_ActorGroup.Enabled = false;
                    HL_WLTem.Enabled = false;

                    BT_Assign.Enabled = false;

                    BT_Close.Enabled = false;
                    BT_Active.Enabled = false;


                    LoadProjectTask();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
                }
            }
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;
            string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            SetTaskAssignRecordColor();

            e.Item.ForeColor = Color.Green;

            strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
            TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
            TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
            lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
            taskAssignRecord = (TaskAssignRecord)lst[0];

            LB_ID.Text = taskAssignRecord.ID.ToString();

            DL_OperatorCode.SelectedValue = taskAssignRecord.OperatorCode;
            DL_RecordType.SelectedValue = taskAssignRecord.Type;

            if (strIsMobileDevice == "YES")
            {
                HT_Operation.Text = taskAssignRecord.Operation.Trim();
            }
            else
            {
                HE_Operation.Text = taskAssignRecord.Operation.Trim();
            }

            DLC_TaskBegin.Text = taskAssignRecord.BeginDate.ToString("yyyy-MM-dd");
            DLC_TaskEnd.Text = taskAssignRecord.EndDate.ToString("yyyy-MM-dd");

            HL_TaskReview.NavigateUrl = "TTProjectTaskReviewWL.aspx?TaskID=" + strID;
            HL_TaskReview.Enabled = true;


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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }


    protected void DL_OperatorCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strOperatorCode;

        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();

        if (strOperatorCode != "")
        {
            HL_MemberWorkload.NavigateUrl = "TTMemberWorkload.aspx?UserCode=" + strOperatorCode;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void DL_AllowUpdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strTaskID, strAllowUpdate;

        strAllowUpdate = DL_AllowUpdate.SelectedValue;
        strTaskID = LB_TaskNO.Text.Trim();

        try
        {
            strHQL = "Update T_RelatedBusinessForm Set AllowUpdate = '" + strAllowUpdate + "'  Where RelatedType = 'Task' and RelatedID = " + strTaskID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_RelatedBusinessForm Set AllowUpdate = '" + strAllowUpdate + "'  Where RelatedType = 'TaskRecord' ";
            strHQL += " and RelatedID in (Select ID From T_TaskAssignRecord Where TaskID =  " + strTaskID + ")";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strTaskNO;

        strTaskNO = LB_TaskNO.Text.Trim();

        if (strTaskNO == "")
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
        string strTask, strOperatorCode, strUserCode, strMakeMan, strMakeManCode;
        string strBeginDate, strEndDate, strTaskID, strBudget;
        string strStatus, strPriority, strType;
        DateTime dtMakeDate;
        int intProjectID;
        decimal deFinishPercent;
        decimal deManHour;


        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        ProjectTask projectTask = new ProjectTask();

        intProjectID = int.Parse(LB_ProjectID.Text.Trim());
        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        strType = DL_Type.SelectedValue.Trim();
        strTask = TB_Task.Text.Trim();
        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();
        strBudget = TB_Budget.Amount.ToString();
        strBeginDate = DLC_BeginDate.Text;
        strEndDate = DLC_EndDate.Text;
        strMakeManCode = strUserCode;
        strMakeMan = ShareClass.GetUserName(strUserCode);
        dtMakeDate = DateTime.Now;
        strStatus = DL_Status.SelectedValue;
        deFinishPercent = TB_FinishPercent.Amount;
        strPriority = DL_Priority.SelectedValue;
        deManHour = NB_ManHour.Amount;

        if (strTask == "" | strBeginDate == "" | strEndDate == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRWKSRJSRZXRBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        else
        {
            if (strBudget == "")
                strBudget = "0";

            projectTask.ProjectID = intProjectID;
            projectTask.Type = strType;
            projectTask.Task = strTask;
            projectTask.Budget = decimal.Parse(strBudget);
            projectTask.Expense = 0;
            projectTask.ManHour = deManHour;
            projectTask.RealManHour = 0;
            projectTask.BeginDate = DateTime.Parse(strBeginDate);
            projectTask.EndDate = DateTime.Parse(strEndDate);
            projectTask.MakeManCode = strMakeManCode;
            projectTask.MakeManName = ShareClass.GetUserName(strUserCode);
            projectTask.MakeDate = dtMakeDate;
            projectTask.Status = strStatus;
            projectTask.FinishPercent = deFinishPercent;
            projectTask.Priority = strPriority;
            projectTask.CollaborationID = int.Parse(LB_CollaborationID.Text.Trim());
            projectTask.IsPlanMainTask = "NO";

            projectTask.RequireNumber = 0;
            projectTask.FinishedNumber = 0;
            projectTask.UnitName = "";
            projectTask.Price = 0;

            try
            {
                projectTaskBLL.AddProjectTask(projectTask);

                strTaskID = ShareClass.GetMyCreatedMaxTaskID(intProjectID.ToString(), strUserCode);
                LB_TaskNO.Text = strTaskID;

                LB_TaskName.Visible = true;
                LB_TaskName.Text = LanguageHandle.GetWord("RenWu") + strTaskID + "  " + strTask + LanguageHandle.GetWord("DeFenPaJiLu");

                //自动分派任务给创建者
                string strHQL3 = "Insert Into T_TaskAssignRecord(TaskID,Task,Type,OperatorCode,OperatorName,OperatorContent,OperationTime,BeginDate,EndDate,AssignManCode,AssignManName,Content,Operation,PriorID,RouteNumber,MakeDate,Status,FinishedNumber,UnitName,MoveTime)";
                strHQL3 += " Select A.TaskID,A.Task,'Task',A.MakeManCode,A.MakeManName,'',now(),A.BeginDate,A.EndDate,A.MakeManCode,A.MakeManName,'',A.Task,0,A.TaskID,now(),'ToHandle',0,UnitName,now()";
                strHQL3 += " From T_ProjectTask A Where A.TaskID = " + strTaskID;
                strHQL3 += " and A.TaskID Not In (Select TaskID From T_TaskAssignRecord)";
                ShareClass.RunSqlCommand(strHQL3);


                HL_TaskRelatedDoc.Enabled = true;
                HL_TaskRelatedDoc.NavigateUrl = "TTProTaskRelatedDoc.aspx?TaskID=" + strTaskID;
                HL_TestCase.Enabled = true;
                HL_TestCase.NavigateUrl = "TTMakeTaskTestCase.aspx?TaskID=" + strTaskID + "&ProjectID=" + intProjectID.ToString();
                HL_AssignRecord.Enabled = true;
                HL_AssignRecord.NavigateUrl = "TTTaskAssignRecord.aspx?TaskID=" + strTaskID;
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

                BT_Assign.Enabled = true;

                BT_Close.Enabled = true;
                BT_Active.Enabled = true;

                LoadProjectTask();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void UpdateTask()
    {
        string strTaskID, strProjectID, strTask, strBeginDate;
        string strStatus, strEndDate, strMakeDate;
        string strOperatorCode, strHQL, strPriority, strType;
        decimal deManHour, deRealManHour;
        string strBudget;
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

            projectTask.ProjectID = int.Parse(strProjectID);
            projectTask.Type = strType;
            projectTask.Task = strTask;
            projectTask.BeginDate = DateTime.Parse(strBeginDate);
            projectTask.EndDate = DateTime.Parse(strEndDate);
            projectTask.MakeDate = DateTime.Parse(strMakeDate);
            projectTask.Status = strStatus;
            projectTask.Budget = decimal.Parse(strBudget);
            projectTask.ManHour = deManHour;
            projectTask.FinishPercent = deFinishPercent;
            projectTask.Priority = strPriority;
            projectTask.CollaborationID = int.Parse(LB_CollaborationID.Text.Trim());
            projectTask.IsPlanMainTask = "NO";


            try
            {
                projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));

                //BusinessForm，关联相应的业务表单模板
                ShareClass.SaveRelatedBusinessForm("Task", strTaskID, DL_WFTemplate.SelectedValue, DL_AllowUpdate.SelectedValue, strUserCode);

                LoadProjectTask();
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXJHBNXG") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
            LoadProjectTask();

            BT_Assign.Enabled = false;

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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
            LoadProjectTask();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHCG") + "')", true);

            BT_Assign.Enabled = true;

            LB_Status.Text = "InProgress";
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHRWSBJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

        taskAssignRecord.Type = DL_RecordType.SelectedValue;

        if (strIsMobileDevice == "YES")
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void BT_DeleteAssign_Click(object sender, EventArgs e)
    {
        string strHQL, strID;
        IList lst;

        strID = LB_ID.Text.Trim();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL); taskAssignRecord = (TaskAssignRecord)lst[0];


        try
        {
            taskAssignRecordBLL.DeleteTaskAssignRecord(taskAssignRecord);


            BT_UpdateAssign.Enabled = false;
            BT_DeleteAssign.Enabled = false;
            BT_Assign.Enabled = true;

            LoadAssignRecord(LB_TaskNO.Text.Trim());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }



    protected void DLC_TaskBegin_TextChanged(object sender, EventArgs e)
    {
        string strHQL1;

        int intTaskID;
        DateTime dtBeginDate, dtEndDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
        intTaskID = int.Parse(LB_TaskID.Text.Trim());
        dtBeginDate = DateTime.Parse(DLC_BeginDate.Text);
        dtEndDate = DateTime.Parse(DLC_EndDate.Text);

        strHQL1 = "Select *  From V_ProjectMember_WorkLoadSchedule";
        strHQL1 += " Where UserCode = " + "'" + strUserCode + "'";
        strHQL1 += " and ((to_char(BeginDate,'yyyymmdd') >= to_char(cast('" + DLC_BeginDate.Text + "' as timestamp) ,'yyyymmdd') and to_char(BeginDate,'yyyymmdd') <= to_char( cast('" + DLC_EndDate.Text + "' as timestamp),'yyyymmdd'))";
        strHQL1 += " or (to_char(EndDate,'yyyymmdd') >= to_char(cast('" + DLC_BeginDate.Text + "' as timestamp),'yyyymmdd') and to_char(EndDate,'yyyymmdd') <= to_char(cast('" + DLC_EndDate.Text + "' as timestamp),'yyyymmdd'))";
        strHQL1 += " or (to_char(BeginDate,'yyyymmdd') <= to_char(cast('" + DLC_BeginDate.Text + "' as timestamp),'yyyymmdd') and to_char(EndDate,'yyyymmdd') >= to_char(cast('" + DLC_EndDate.Text + "' as timestamp),'yyyymmdd')))";
        strHQL1 += " and Type ='Task'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "V_ProjectMember_WorkLoadSchedule");

        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click111", "alert('" + LanguageHandle.GetWord("ZZTSCCYZCSJDYQTRWJX") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DLC_TaskEnd_TextChanged(object sender, EventArgs e)
    {
        string strHQL1;

        int intTaskID;
        DateTime dtBeginDate, dtEndDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
        intTaskID = int.Parse(LB_TaskID.Text.Trim());
        dtBeginDate = DateTime.Parse(DLC_BeginDate.Text);
        dtEndDate = DateTime.Parse(DLC_EndDate.Text);

        strHQL1 = "Select *  From V_ProjectMember_WorkLoadSchedule";
        strHQL1 += " Where UserCode = " + "'" + strUserCode + "'";
        strHQL1 += " and ((to_char(BeginDate,'yyyymmdd') >= to_char(cast('" + DLC_BeginDate.Text + "' as timestamp) ,'yyyymmdd') and to_char(BeginDate,'yyyymmdd') <= to_char( cast('" + DLC_EndDate.Text + "' as timestamp),'yyyymmdd'))";
        strHQL1 += " or (to_char(EndDate,'yyyymmdd') >= to_char(cast('" + DLC_BeginDate.Text + "' as timestamp),'yyyymmdd') and to_char(EndDate,'yyyymmdd') <= to_char(cast('" + DLC_EndDate.Text + "' as timestamp),'yyyymmdd'))";
        strHQL1 += " or (to_char(BeginDate,'yyyymmdd') <= to_char(cast('" + DLC_BeginDate.Text + "' as timestamp),'yyyymmdd') and to_char(EndDate,'yyyymmdd') >= to_char(cast('" + DLC_EndDate.Text + "' as timestamp),'yyyymmdd')))";
        strHQL1 += " and Type ='Task'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "V_ProjectMember_WorkLoadSchedule");

        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click111", "alert('" + LanguageHandle.GetWord("ZZTSCCYZCSJDYQTRWJX") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Assign_Click(object sender, EventArgs e)
    {
        string strAssignID;

        int intTaskID, intPriorID;
        string strTask, strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;
        string strOperation, strType;
        DateTime dtBeginDate, dtEndDate, dtMakeDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        intTaskID = int.Parse(LB_TaskNO.Text.Trim());
        strType = DL_RecordType.SelectedValue;
        strTask = TB_Task.Text.Trim();
        strOperatorCode = DL_OperatorCode.SelectedValue;
        strOperatorName = ShareClass.GetUserName(strOperatorCode);
        strAssignManCode = LB_UserCode.Text.Trim();
        strAssignManName = LB_UserName.Text.Trim();

        if (strIsMobileDevice == "YES")
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

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

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

            strAssignID = ShareClass.GetMyCreatedMaxTaskAssignRecordID(intTaskID.ToString(), strUserCode);
            LB_ID.Text = strAssignID;

            //BusinessForm,处理关联的业务表单数据
            ShareClass.InsertOrUpdateTaskAssignRecordWFXMLData("Task", intTaskID.ToString(), "TaskRecord", strAssignID, strUserCode);

            BT_UpdateAssign.Enabled = true;
            BT_DeleteAssign.Enabled = true;
            BT_Assign.Enabled = true;

            LoadAssignRecord(LB_TaskNO.Text.Trim());
            UpdateTaskStatus(intTaskID.ToString(), "InProgress");

            ShareClass.SendInstantMessage(LanguageHandle.GetWord("RenWuFenPaiTongZhi"), ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeRenWu") + " :" + intTaskID.ToString() + "  " + strTask + "，" + LanguageHandle.GetWord("QingJiShiChuLi"), strUserCode, strOperatorCode);

            TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeRenWu") + " :" + intTaskID.ToString() + "  " + "，" + LanguageHandle.GetWord("QingJiShiChuLi");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPCG") + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg, strOperatorCode, strUserCode;

        Msg msg = new Msg();

        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();

        strUserCode = LB_UserCode.Text.Trim();

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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_WorkRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWorkReques = DL_WorkRequest.SelectedValue.Trim();

        if (strIsMobileDevice == "YES")
        {
            HT_Operation.Text = strWorkReques;
        }
        else
        {
            HE_Operation.Text = strWorkReques;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadProjectTask()
    {
        string strProjectID = LB_ProjectID.Text.Trim();
        string strCollaborationID = LB_CollaborationID.Text.Trim();

        string strHQL = "from ProjectTask as projectTask where projectTask.CollaborationID = " + strCollaborationID + " Order by projectTask.TaskID ASC";
        ProjectTask projectTask = new ProjectTask();
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }


    //BusinessForm,工作流类型查询
    protected void DL_WLType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL, strWLType;

        strWLType = DL_WLType.SelectedValue.Trim();
        if (string.IsNullOrEmpty(strWLType))
        {
            return;
        }
        strHQL = "Select TemName From T_WorkFlowTemplate Where type = " + "'" + strWLType + "'" + " and Visible = 'YES' and Authority = 'All'";
        strHQL += " Order by SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");
        DL_WFTemplate.DataSource = ds;
        DL_WFTemplate.DataBind();

        DL_WFTemplate.Items.Add(new ListItem(""));

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    //BusinessForm,启动关联的业务表单
    protected void BT_StartupBusinessForm_Click(object sender, EventArgs e)
    {
        string strURL;
        string strTemName, strIdentifyString;
        strTemName = DL_WFTemplate.SelectedValue.Trim();
        strIdentifyString = ShareClass.GetWLTemplateIdentifyString(strTemName);

        string strTaskID;
        strTaskID = LB_TaskNO.Text.Trim();


        if (strTaskID == "")
        {
            strTaskID = "0";
        }

        //strURL = "popShowByURL(" + "'TTRelatedDIYBusinessForm.aspx?RelatedType=Task&RelatedID=" + strTaskID + "&IdentifyString=" + strIdentifyString  + "','" + LanguageHandle.GetWord("XiangGuanYeWuDan") + "', 800, 600,window.location);";
        //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop12", strURL, true);

        strURL = "TTRelatedDIYBusinessForm.aspx?RelatedType=Task&RelatedID=" + strTaskID + "&IdentifyString=" + strIdentifyString;
        IFrame_RelatedInformation.Attributes.Add("src", strURL);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    //BusinessForm,删除关联的业务表单
    protected void BT_DeleteBusinessForm_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strTemName;
        strTemName = DL_WFTemplate.SelectedValue.Trim();

        string strTaskID;
        strTaskID = LB_TaskNO.Text.Trim();

        strHQL = "Delete From T_RelatedBusinessForm Where RelatedType = 'Task' and RelatedID = " + strTaskID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void SetProTaskColor()
    {
        int i;
        DateTime dtNowDate, dtFinishedDate;
        string strStatus;

        for (i = 0; i < DataGrid1.Items.Count; i++)
        {
            dtFinishedDate = DateTime.Parse(DataGrid1.Items[i].Cells[8].Text.Trim());
            dtNowDate = DateTime.Now;
            strStatus = DataGrid1.Items[i].Cells[12].Text.Trim();

            if (strStatus != "Completed" | strStatus != "Closed")
            {
                if (dtFinishedDate < dtNowDate)
                {
                    DataGrid1.Items[i].ForeColor = Color.Red;
                }
            }
        }
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
            strStatus = DataGrid2.Items[i].Cells[10].Text.Trim();

            if (strStatus != "Completed" & strStatus != LanguageHandle.GetWord("YiWanCheng"))
            {
                if (dtFinishedDate < dtNowDate)
                {
                    DataGrid2.Items[i].ForeColor = Color.Red;
                }
            }
        }
    }

    protected void LoadCollaborationMember(string strCollaborationID, DropDownList DL_OperatorCode, string strUserCode)
    {
        string strHQL;

        strHQL = "Select UserCode,UserName From T_ProjectMember Where UserCode in (Select UnderCode From T_MemberLevel Where Usercode = " + "'" + strUserCode + "'" + ")";
        strHQL += " Or UserCode in (Select UserCode From T_CollaborationMember Where CoID=" + strCollaborationID + ")";
        strHQL += " Order By SortNumber ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CollaborationMember");

        DL_OperatorCode.DataSource = ds;
        DL_OperatorCode.DataBind();
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

    protected void UpdateTaskStatus(string strTaskID, string strStatus)
    {
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        projectTask.Status = strStatus;

        projectTaskBLL.UpdateProjectTask(projectTask, projectTask.TaskID);
    }

    protected int GetRouteNumber(string strTaskID)
    {
        string strHQL;
        IList lst;
        int intRoutNumber;

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.TaskID = " + strTaskID + " and taskAssignRecord.PriorID = 0";
        strHQL += " Order By taskAssignRecord.RouteNumber DESC";
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        if (lst.Count > 0)
        {
            TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];
            intRoutNumber = taskAssignRecord.RouteNumber;
            return intRoutNumber + 1;
        }
        else
        {
            return 1;
        }
    }

    protected void UpdateReqAssignRecordStatus(string strReqRecordID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ReqAssignRecord as reqAssignRecord where reqAssignRecord.ID = " + strReqRecordID;
        ReqAssignRecordBLL reqAssignRecordBLL = new ReqAssignRecordBLL();
        lst = reqAssignRecordBLL.GetAllReqAssignRecords(strHQL);
        ReqAssignRecord reqAssignRecord = (ReqAssignRecord)lst[0];

        reqAssignRecord.Status = "ToTask";

        try
        {
            reqAssignRecordBLL.UpdateReqAssignRecord(reqAssignRecord, int.Parse(strReqRecordID));
        }
        catch
        {
        }
    }
}
