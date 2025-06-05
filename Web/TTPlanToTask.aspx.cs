using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTPlanToTask : System.Web.UI.Page
{
    string strTaskStatus;
    string strIsMobileDevice;
    string strLangCode;
    string strUserCode;
    string strPlanID;
    string strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strPlanID = Request.QueryString["PlanID"];
        strProjectID = ShareClass.getProjectIDByPlanID(strPlanID);

        string strHQL;
        IList lst;
        string strUserName;

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_Operation);
HE_Operation.Language = Session["LangCode"].ToString();

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        strHQL = "from WorkPlan as workPlan where workPlan.ID = " + strPlanID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);
        WorkPlan workPlan = (WorkPlan)lst[0];

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        DataList1.DataSource = lst;
        DataList1.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_Operation.Visible = true; HT_Operation.Toolbar = "";
            }
            else
            {
                HE_Operation.Visible = true;
            }

            //if (ShareClass.CheckUserCanControlProjectPlan(strPlanID, strUserCode) == false)
            //{
            //    Response.Redirect("TTDisplayCustomErrorMessage.aspx?ErrorMsg='" + LanguageHandle.GetWord("ZZJGZYXMJLJHYJHCJRHLXZJHFZRCNJXZCZQJC") + "'");
            //}

            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_TaskBegin.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_TaskEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ShareClass.LoadTaskType(DL_Type);
            ShareClass.LoadTaskRecordType(DL_RecordType);
            ShareClass.LoadTaskStatus(DL_Status, strLangCode);
            ShareClass.LoadUnitForDropDownList(DL_UnitName);

            ShareClass.LoadTaskWorkRequest(DL_WorkRequest);
            LoadProjectMember(strProjectID, strPlanID, strUserCode, DL_OperatorCode);

            strHQL = "Select LeaderCode From T_ImplePlan Where ID = " + strPlanID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
            string strLeaderCode;
            try
            {
                strLeaderCode = ds.Tables[0].Rows[0][0].ToString().Trim();
                DL_OperatorCode.SelectedValue = strLeaderCode;
            }
            catch
            {
            }

            strHQL = "from PlanMember as planMember where planMember.PlanID = " + strPlanID;
            PlanMemberBLL planMemberBLL = new PlanMemberBLL();
            lst = planMemberBLL.GetAllPlanMembers(strHQL);
            DL_PlanMember.DataSource = lst;
            DL_PlanMember.DataBind();

            TB_Task.Text = workPlan.Name.Trim();
            DLC_BeginDate.Text = workPlan.Start_Date.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = workPlan.End_Date.ToString("yyyy-MM-dd");
            LB_MakeDate.Text = DateTime.Now.ToShortDateString();

            string strHQL1;
            strHQL1 = "Select RequireNumber,FinishedNumber,UnitName,Price From T_ImplePlan Where ID = " + strPlanID;
            DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_ImplePlan");
            NB_RequireNumber.Amount = decimal.Parse(ds1.Tables[0].Rows[0]["RequireNumber"].ToString()) - decimal.Parse(ds1.Tables[0].Rows[0]["FinishedNumber"].ToString());
            try
            {
                DL_UnitName.SelectedValue = ds1.Tables[0].Rows[0]["UnitName"].ToString();
            }
            catch
            {
            }
            NB_Price.Amount = decimal.Parse(ds1.Tables[0].Rows[0]["Price"].ToString());

            HE_Operation.Text = LanguageHandle.GetWord("JiHuaNeiRong") + workPlan.Name.Trim();
            LB_PlanID.Text = strPlanID;
            LB_ProjectID.Text = strProjectID;

            LoadProjectTask(strPlanID);

            SetProTaskColor();

            DL_Priority.SelectedValue = GetWorkPlanByPlanID(strPlanID);

            string strSystemVersionType = Session["SystemVersionType"].ToString();
            string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
            {
                TabPanel2.Visible = false;
            }

            //BusinessForm,列出业务表单类型 
            ShareClass.LoadWorkflowType(DL_WLType, strLangCode);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        //BusinessForm，隐藏业务表单元素
        Panel_RelatedBusiness.Visible = false;

        LB_TaskNO.Text = "";

        NB_Budget.Amount = 0;
        TB_Expense.Amount = 0;
        NB_ManHour.Amount = 0;
        NB_RealManHour.Amount = 0;
        TB_FinishPercent.Amount = 0;


        string strPlanID;
        strPlanID = LB_PlanID.Text;

        try
        {
            DataSet ds = getProjectWorkPlan(strPlanID);

            if (decimal.Parse(ds.Tables[0].Rows[0]["Budget"].ToString()) > getTotalPlanTaskExpenseBudget(strPlanID))
            {
                NB_Budget.Amount = decimal.Parse(ds.Tables[0].Rows[0]["Budget"].ToString()) - getTotalPlanTaskExpenseBudget(strPlanID);
            }
            else
            {
                NB_Budget.Amount = 0;
            }

            if (decimal.Parse(ds.Tables[0].Rows[0]["WorkHour"].ToString()) > getTotalPlanTaskManhourBudget(strPlanID))
            {
                NB_ManHour.Amount = decimal.Parse(ds.Tables[0].Rows[0]["WorkHour"].ToString()) - getTotalPlanTaskManhourBudget(strPlanID);
            }
            else
            {
                NB_ManHour.Amount = 0;
            }
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strProjectID = LB_ProjectID.Text.Trim();
            string strTaskID = e.Item.Cells[3].Text.Trim();

            IList lst;
            string strHQL;


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
                NB_Budget.Amount = projectTask.Budget;
                LB_MakeDate.Text = projectTask.MakeDate.ToString();
                try
                {
                    DL_Status.SelectedValue = projectTask.Status;
                }
                catch
                {

                }
                TB_Expense.Amount = projectTask.Expense;
                TB_FinishPercent.Amount = projectTask.FinishPercent;
                DL_Priority.SelectedValue = projectTask.Priority.Trim();
                NB_ManHour.Amount = projectTask.ManHour;
                NB_RealManHour.Amount = projectTask.RealManHour;
                LB_TaskName.Visible = true;
                LB_TaskName.Text = LanguageHandle.GetWord("RenWu") + projectTask.TaskID.ToString().Trim() + "  " + projectTask.Task.Trim() + LanguageHandle.GetWord("FenPaJiLu");

                NB_RequireNumber.Amount = projectTask.RequireNumber;
                NB_FinishedNumber.Amount = projectTask.FinishedNumber;
                NB_Price.Amount = projectTask.Price;

                try
                {
                    DL_UnitName.SelectedValue = projectTask.UnitName;
                }
                catch
                {
                }

                DLC_TaskBegin.Text = projectTask.BeginDate.ToString("yyyy-MM-dd");
                DLC_TaskEnd.Text = projectTask.EndDate.ToString("yyyy-MM-dd");

                strTaskStatus = projectTask.Status.Trim();

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

                LoadAssignRecord(strTaskID);

                HL_TestCase.Enabled = true;
                HL_TestCase.NavigateUrl = "TTMakeTaskTestCase.aspx?TaskID=" + strTaskID + "&ProjectID=" + strProjectID;
                HL_AssignRecord.Enabled = true;
                HL_AssignRecord.NavigateUrl = "TTTaskAssignRecord.aspx?TaskID=" + strTaskID;
                HL_TaskRelatedDoc.Enabled = true;
                HL_TaskRelatedDoc.NavigateUrl = "TTProTaskRelatedDoc.aspx?TaskID=" + strTaskID;

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


                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Assign")
                {
                    if (projectTask.PlanID > 0)
                    {
                        HT_Operation.Text = TB_Task.Text;
                        HE_Operation.Text = TB_Task.Text;

                        try
                        {
                            DataSet ds = getProjectWorkPlan(LB_PlanID.Text);
                            DL_OperatorCode.SelectedValue = ds.Tables[0].Rows[0]["LeaderCode"].ToString();

                            HL_MemberWorkload.NavigateUrl = "TTMemberWorkload.aspx?UserCode=" + ds.Tables[0].Rows[0]["LeaderCode"].ToString();
                        }
                        catch
                        {
                        }
                    }

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
                }


                if (e.CommandName == "Other")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popOtherWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                string strPlanID;

                strPlanID = LB_PlanID.Text.Trim();

                ProjectTask projectTask = new ProjectTask();
                ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();

                projectTask.TaskID = int.Parse(strTaskID);

                try
                {
                    projectTaskBLL.DeleteProjectTask(projectTask);

                    if (projectTask.PlanID > 0)
                    {
                        strHQL = "update T_ImplePlan Set Percent_Done = " + ShareClass.GetTaskOrWorkflowPlanProgress(strPlanID);
                        strHQL += ",ActualHour = " + ShareClass.GetTotalRealManHourByPlan(strPlanID);
                        strHQL += ",Expense = " + ShareClass.GetTotalRealExpenseByPlan(strPlanID);
                        strHQL += " Where PlanID = " + strPlanID;
                        ShareClass.RunSqlCommand(strHQL);
                    }

                    LoadProjectTask(strPlanID);

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
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
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

            strTaskStatus = LB_Status.Text.Trim();

            HL_TaskReview.NavigateUrl = "TTProjectTaskReviewWL.aspx?TaskID=" + strID;
            HL_TaskReview.Enabled = true;

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
        string strTaskID, strTask, strOperatorCode, strUserCode, strMakeMan, strMakeManCode;
        string strBeginDate, strEndDate, strBudget, strPlanID;
        string strStatus, strPriority, strType;
        DateTime dtMakeDate;
        int intProjectID;
        decimal deFinishPercent;
        decimal deManHour;


        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        ProjectTask projectTask = new ProjectTask();

        intProjectID = int.Parse(LB_ProjectID.Text.Trim());
        strPlanID = LB_PlanID.Text.Trim();
        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        strType = DL_Type.SelectedValue.Trim();
        strTask = TB_Task.Text.Trim();
        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();
        strBudget = NB_Budget.Amount.ToString();
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRWKSRJSRCZZBNWKJC") + "')", true);
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
            projectTask.PlanID = int.Parse(LB_PlanID.Text.Trim());
            projectTask.IsPlanMainTask = "YES";

            projectTask.RequireNumber = NB_RequireNumber.Amount;
            projectTask.FinishedNumber = NB_FinishedNumber.Amount;
            projectTask.UnitName = DL_UnitName.SelectedValue;
            projectTask.Price = NB_Price.Amount;

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

                LoadProjectTask(strPlanID);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void UpdateTask()
    {
        string strTaskID, strProjectID, strTask, strBeginDate;
        string strStatus, strEndDate, strMakeDate;
        string strOperatorCode, strPriority, strType, strPlanID;
        decimal deManHour, deRealManHour, deFinishPercent;
        string strBudget;
        string strHQL;
        IList lst;

        strPlanID = LB_PlanID.Text.Trim();
        strTaskID = LB_TaskNO.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strOperatorCode = DL_OperatorCode.SelectedValue;
        strProjectID = LB_ProjectID.Text.Trim();
        strTask = TB_Task.Text.Trim();
        strBeginDate = DLC_BeginDate.Text;
        strEndDate = DLC_EndDate.Text;
        strMakeDate = LB_MakeDate.Text.Trim();
        strBudget = NB_Budget.Amount.ToString();
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
            projectTask.PlanID = int.Parse(LB_PlanID.Text.Trim());
            projectTask.IsPlanMainTask = "YES";

            projectTask.RequireNumber = NB_RequireNumber.Amount;
            projectTask.FinishedNumber = NB_FinishedNumber.Amount;
            projectTask.UnitName = DL_UnitName.SelectedValue;
            projectTask.Price = NB_Price.Amount;

            try
            {
                projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));

                //BusinessForm，关联相应的业务表单模板
                ShareClass.SaveRelatedBusinessForm("Task", strTaskID, DL_WFTemplate.SelectedValue, DL_AllowUpdate.SelectedValue, strUserCode);

                if (projectTask.PlanID > 0)
                {
                    strHQL = "update T_ImplePlan Set Percent_Done = " + ShareClass.GetTaskOrWorkflowPlanProgress(strPlanID);
                    strHQL += ",ActualHour = " + ShareClass.GetTotalRealManHourByPlan(strPlanID);
                    strHQL += ",Expense = " + ShareClass.GetTotalRealExpenseByPlan(strPlanID);
                    strHQL += " Where PlanID = " + strPlanID;
                    ShareClass.RunSqlCommand(strHQL);
                }

                LoadProjectTask(strPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch (Exception err)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJLBNBGX") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        }
    }

    protected void BT_Close_Click(object sender, EventArgs e)
    {
        string strTaskID = LB_TaskNO.Text.Trim();
        string strPlanID = LB_PlanID.Text.Trim();

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        ProjectTask projectTask = new ProjectTask();
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        projectTask = (ProjectTask)lst[0];

        projectTask.Status = "Closed";

        try
        {
            projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));
            LoadProjectTask(strPlanID);

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
        string strPlanID = LB_PlanID.Text.Trim();

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        ProjectTask projectTask = new ProjectTask();
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        projectTask = (ProjectTask)lst[0];

        projectTask.Status = "InProgress";

        try
        {
            projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));
            LoadProjectTask(strPlanID);
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

    protected void DL_PlanMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strOperatorCode;

        strOperatorCode = DL_PlanMember.SelectedValue.Trim();

        if (strOperatorCode != "")
        {
            HL_MemberWorkload.NavigateUrl = "TTMemberWorkload.aspx?UserCode=" + strOperatorCode;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXSBJC") + "')", true);
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

        DateTime dtBeginDate, dtEndDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
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

        DateTime dtBeginDate, dtEndDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
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
        strOperatorCode = DL_OperatorCode.SelectedValue.Trim();
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
                msg.SendMSM("Message", strOperatorCode, strMsg, strUserCode);
            }

            if (CB_SendMail.Checked == true)
            {
                msg.SendMail(strOperatorCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB") + "')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

    }

    protected void DL_WorkRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWorkRequest = DL_WorkRequest.SelectedValue.Trim();
        HE_Operation.Text = strWorkRequest;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
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

    protected void LoadProjectTask(string strPlanID)
    {
        string strProjectID = LB_ProjectID.Text.Trim();

        string strHQL = "from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + " and projectTask.PlanID = " + strPlanID + " Order by projectTask.TaskID ASC";
        ProjectTask projectTask = new ProjectTask();
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
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

    protected void LoadAssignRecord(string strTaskID)
    {
        string strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.TaskID = " + strTaskID + " Order by taskAssignRecord.ID Desc";
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        SetTaskAssignRecordColor();
    }

    public void LoadProjectMember(string strProjectID, string strPlanID, string strUserCode, DropDownList DL_OperatorCode)
    {
        string strHQL;

        strHQL = string.Format(@"select distinct UserCode ,UserName ,SortNumber From (
                Select UserCode, UserName, 1 as SortNumber From T_PlanMember Where PlanID = {0}
                 Union Select UserCode, UserName, 2 as SortNumber From T_RelatedUser Where ProjectID = {1}
                 union Select UserCode,UserName,3 as SortNumber From T_ProjectMember Where UserCode in (Select UnderCode From T_MemberLevel Where Usercode = '{2}')) A
                   Order By A.SortNumber ASC", strPlanID, strProjectID, strUserCode);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");
        DL_OperatorCode.DataSource = ds;
        DL_OperatorCode.DataBind();

        DL_OperatorCode.Items.Insert(0, new ListItem("--Select--", ""));
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

    //取得已用计划费用预算总额
    protected decimal getTotalPlanTaskExpenseBudget(string strPlanID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(Budget),0) From T_ProjectTask Where PlanID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //取得已用计划工时预算总额
    protected decimal getTotalPlanTaskManhourBudget(string strPlanID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(ManHour),0) From T_ProjectTask Where PlanID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetWorkPlanByPlanID(string strPlanID)
    {
        string strHQL;

        string strPriority;

        strHQL = "Select Priority From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        strPriority = ds.Tables[0].Rows[0][0].ToString().Trim();

        if (strPriority == "0")
        {
            return "Low";
        }

        if (strPriority == "1")
        {
            return "Normal";
        }

        if (strPriority == "2")
        {
            return "High";
        }

        return "Normal";
    }


    //取得项目计划
    protected DataSet getProjectWorkPlan(string strPlanID)
    {
        string strHQL;

        strHQL = "Select * From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        return ds;
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

}
