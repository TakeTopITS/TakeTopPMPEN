using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;
using TakeTopCore;

public partial class TTAppProjectTaskDetail : System.Web.UI.Page
{
    string strMakeManCode, strAssignManCode, strTaskStatus, strRecordStatus;
    string strProjectID, strTaskID, strTaskName, strPlanID;
    string strUserCode, strUserName;

    string strIsMobileDevice;

    public SignModel signModel;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;
        string strUserName;
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        string strID = Request.QueryString["ID"];

        string strHQL;
        IList lst;

        string strProjectName, strProjectStatus;
        string strCreatorCode, strRelatedUserStatus;
        string strPMCode;

       

        try
        {
            //扫码功能必须
            signModel = TakeTopCore.WXHelper.GetWXInfo(Request.Url.ToString());
            if (signModel != null)
            {
                if (signModel.appId == null)
                {
                    signModel.appId = "";
                }
            }
            else
            {
                signModel.appId = "";
            }
        }
        catch
        {
            signModel = new SignModel();
            signModel.appId = "";
        }



        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_FinishContent);
HE_FinishContent.Language = Session["LangCode"].ToString();
        _FileBrowser.SetupCKEditor(HE_Operation);
HE_Operation.Language = Session["LangCode"].ToString();


        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        DataList2.DataSource = lst;
        DataList2.DataBind();

        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
        taskAssignRecord = (TaskAssignRecord)lst[0];

        strTaskID = taskAssignRecord.TaskID.ToString();
        strTaskName = taskAssignRecord.Task.Trim();
        strRecordStatus = taskAssignRecord.Status.Trim();
        strAssignManCode = taskAssignRecord.AssignManCode.Trim();

        strProjectID = GetProjectID(strTaskID);
        strProjectName = ShareClass.GetProjectName(strProjectID);


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_FinishContent.Visible = true;
                HT_FinishContent.Text = taskAssignRecord.OperatorContent.Trim();
                HT_FinishContent.Toolbar = "";

                HT_Operation.Visible = true;
                HT_Operation.Toolbar = "";
            }
            else
            {
                HE_FinishContent.Visible = true;
                HE_FinishContent.Text = taskAssignRecord.OperatorContent.Trim();

                HE_Operation.Visible = true; 
            }

            string strSystemVersionType = Session["SystemVersionType"].ToString();
            string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
            {
                HL_GoodsApplication.Visible = false;
            }


            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TB_Expense.Amount = taskAssignRecord.Expense;
            NB_ManHour.Amount = taskAssignRecord.ManHour;
            NB_FinishPercent.Amount = taskAssignRecord.FinishPercent;

            LB_AssignID.Text = taskAssignRecord.ID.ToString();

            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = ShareClass.GetUserName(strUserCode);

            LB_TaskID.Text = strTaskID;
            LB_Task.Text = strTaskName;
            LB_RouteNumber.Text = taskAssignRecord.RouteNumber.ToString();

            ShareClass.LoadTaskRecordType(DL_RecordType);

            strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID; ;
            ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
            lst = projectTaskBLL.GetAllProjectTasks(strHQL);

            DataList3.DataSource = lst;
            DataList3.DataBind();

            ProjectTask projectTask = (ProjectTask)lst[0];
            LB_TaskID.Text = projectTask.TaskID.ToString();
            strTaskStatus = projectTask.Status.Trim();
            strMakeManCode = projectTask.MakeManCode.Trim();
            strCreatorCode = projectTask.MakeManCode.Trim();

            NB_TaskProgress.Amount = projectTask.FinishPercent;

            strPlanID = projectTask.PlanID.ToString();
            LB_PlanID.Text = projectTask.PlanID.ToString();

            if (strUserCode == strCreatorCode)
            {
                BT_CloseTask.Enabled = true;
                BT_ActiveTask.Enabled = true;
            }


            strHQL = "from RelatedUser as relatedUser where relatedUser.UserCode = " + "'" + strUserCode + "'" + " and relatedUser.ProjectID = " + strProjectID;
            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
            RelatedUser relatedUser = (RelatedUser)lst[0];

            strRelatedUserStatus = relatedUser.Status.Trim();

            //判断计划进度是否受任务和流程进度影响
            if (ShareClass.CheckMemberCanUpdatePlanByUserCode(strProjectID, strUserCode))
            {
                if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strPlanID)) == "NO")
                {
                    BT_ConfirmEffectPlanProgress.Visible = false;
                }
                else
                {
                    BT_ConfirmEffectPlanProgress.Visible = true;
                }
            }
            else
            {
                BT_ConfirmEffectPlanProgress.Visible = false;
            }

            if (strTaskStatus == "Closed" || strRelatedUserStatus == "Pause" || strRelatedUserStatus == "Stop")
            {
                BT_Activity.Enabled = false;

                BT_Finish.Enabled = false;
                BT_TBD.Enabled = false;
                BT_Assign.Enabled = false;
                HL_TestCase.Enabled = false;
                HL_TaskRelatedDoc.Enabled = false;
                HL_Expense.Enabled = false;

                BT_ConfirmEffectPlanProgress.Visible = false;
            }

            strProjectID = projectTask.ProjectID.ToString();
            LB_ProjectID.Text = strProjectID;

            strProjectStatus = ShareClass.GetProjectStatus(strProjectID);
            if (strProjectStatus == "Suspended" || strProjectStatus == "Cancel")
            {
                BT_Activity.Enabled = false;

                BT_Finish.Enabled = false;
                BT_TBD.Enabled = false;
                BT_Assign.Enabled = false;
                HL_TestCase.Enabled = false;
                HL_TaskRelatedDoc.Enabled = false;
                HL_Expense.Enabled = false;
                HL_ProjectDetail.Enabled = false;

                BT_ConfirmEffectPlanProgress.Visible = false;
            }

            ShareClass.InitialProjectMemberTree(TreeView2, strProjectID);

            LoadChildRecord(strID);

            ShareClass.LoadTaskWorkRequest(DL_WorkRequest);

            HL_TaskRelatedDoc.NavigateUrl = "TTProTaskRelatedDoc.aspx?TaskID=" + strTaskID;
            HL_Expense.NavigateUrl = "TTProExpense.aspx?ProjectID=" + strProjectID + "&TaskID=" + strTaskID + "&RecordID=" + strID + "&QuestionID=0";
            HL_TestCase.NavigateUrl = "TTTaskTestCase.aspx?TaskID=" + strTaskID + "&ProjectID=" + strProjectID;

            HL_TaskAssignRecord.NavigateUrl = "TTTaskAssignRecord.aspx?TaskID=" + projectTask.TaskID.ToString();

            HL_RelatedProjectID.Text = strProjectID;
            HL_RelatedProjectID.NavigateUrl = "TTProjectDetailView.aspx?ProjectID=" + strProjectID;
            HL_RelatedProjectName.Text = strProjectName;

            HL_GoodsApplication.NavigateUrl = "TTAPPGoodsApplicationWFForOther.aspx?RelatedType=AfterSale&RelatedID=" + strTaskID;

            HL_TaskReview.Enabled = true;
            HL_TaskReview.NavigateUrl = "TTProjectTaskReviewWL.aspx?TaskID=" + projectTask.TaskID.ToString();

            HL_MakeProjectReq.NavigateUrl = "TTMakeProjectRequirement.aspx?ProjectID=" + strProjectID;

            strPMCode = GetProjectPMCode(strProjectID);

            if (strUserCode == strPMCode)
            {
                HL_ProjectDetail.NavigateUrl = "TTProjectDetail.aspx?ProjectID=" + strProjectID;
            }
            else
            {
                HL_ProjectDetail.NavigateUrl = "TTInvolvedProDetail.aspx?ProjectID=" + strProjectID;
            }

            //处理和显示分派记录的条形码
            IMG_QrCode.ImageUrl = ShareClass.ShowQrCodeForTaskAssignRecord(strID, 200, 200);
            string strTemName, strIdentifyString;
            strTemName = ShareClass.getRelatedBusinessFormTemName("TaskRecord", strID);
            if (strTemName != "")
            {
                strIdentifyString = ShareClass.GetWLTemplateIdentifyString(strTemName);
                HL_StartupBusinessForm.NavigateUrl = "TTRelatedDIYBusinessForm.aspx?RelatedType=TaskRecord&RelatedID=" + strID + "&IdentifyString=" + strIdentifyString;
            }

            if (ShareClass.getRelatedBusinessFormTemName("TaskRecord", strID) == "")
            {
                HL_StartupBusinessForm.Visible = false;
            }
        }
    }

    public static string GetWeiXinGZHAppID()
    {
        string strHQL;

        string strStatus;

        strHQL = "Select WeiXinNo,Status From T_WeiXinStand";
        DataSet ds = CoreShareClass.GetDataSetFromSql(strHQL, "T_WeiXinStand");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strStatus = ds.Tables[0].Rows[0][1].ToString().Trim();
            if (strStatus == "YES")
            {
                return ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    //处理和显示分派记录的条形码
    protected void BT_SaveQrCode_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strQrCode;
        string strAssignID;

        strAssignID = LB_AssignID.Text.Trim();

        strQrCode = TB_QrCode.Text.Trim();

        strHQL = "Update T_TaskAssignRecord Set QrCode = '" + strQrCode + "' Where ID = " + strAssignID;
        ShareClass.RunSqlCommand(strHQL);

        IMG_QrCode.ImageUrl =ShareClass.GenerateQrCodeImage(ShareClass.GetBarType(),strQrCode, 200, 200);
    }


    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID;
        string strUserCode, strUserName;

        strID = TreeView2.SelectedNode.Target.Trim();

        try
        {
            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ID = " + strID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ProRelatedUser proRelatedUser = (ProRelatedUser)lst[0];

                strUserCode = proRelatedUser.UserCode.Trim();
                strUserName = proRelatedUser.UserName.Trim();

                TB_OperatorCode.Text = strUserCode;
                LB_OperatorManName.Text = strUserName;
            }
        }
        catch
        {
        }
    }

    protected void BT_Approve_Click(object sender, EventArgs e)
    {
        string strHQL, strContent;
        string strID, strTaskID;
        int intFinishPercent;

        strTaskID = LB_TaskID.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HT_FinishContent.Text.Trim();
        }
        else
        {
            strContent = HE_FinishContent.Text.Trim();
        }

        intFinishPercent = int.Parse(NB_FinishPercent.Amount.ToString());

        if (strContent == "")
        {
            strContent = "Accepted";

            if (strIsMobileDevice == "YES")
            {
                HT_FinishContent.Text = strContent;
            }
            else
            {
                HE_FinishContent.Text = strContent;
            }
        }

        strID = LB_AssignID.Text.Trim();
        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];

        taskAssignRecord.OperatorContent = strContent;
        taskAssignRecord.Status = "Accepted";
        taskAssignRecord.FinishPercent = intFinishPercent;
        taskAssignRecord.MakeDate = DateTime.Now;


        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));
            LoadAssignRecord(strID);

            TB_Message.Text = strUserName + LanguageHandle.GetWord("ShouLiLeNiDeRenWu") + strTaskID + " " + strTaskName;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLSBJC") + "')", true);
        }
    }

    protected void BT_Refuse_Click(object sender, EventArgs e)
    {
        string strHQL, strContent;
        string strID, strTaskID;
        string strUserCode;
        int intFinishPercent;

        strUserCode = LB_UserCode.Text.Trim();

        strTaskID = LB_TaskID.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HT_FinishContent.Text.Trim();
        }
        else
        {
            strContent = HE_FinishContent.Text.Trim();
        }

        intFinishPercent = int.Parse(NB_FinishPercent.Amount.ToString());

        if (strContent == "")
        {
            strContent = "Rejected";

            if (strIsMobileDevice == "YES")
            {
                HT_FinishContent.Text = strContent;
            }
            else
            {
                HE_FinishContent.Text = strContent;
            }
        }

        strID = LB_AssignID.Text.Trim();
        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];
        taskAssignRecord.OperatorContent = strContent;
        taskAssignRecord.Status = "Rejected";
        taskAssignRecord.FinishPercent = intFinishPercent;
        taskAssignRecord.MakeDate = DateTime.Now;


        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));
            LoadAssignRecord(strID);

            TB_Message.Text = strUserName + LanguageHandle.GetWord("JuJueLeNiDeRenWu") + strTaskID + " " + strTaskName;

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJJSBJC") + "')", true);
        }
    }

    protected void BT_Assign_Click(object sender, EventArgs e)
    {
        int intTaskID, intPriorID;
        string strTask, strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;
        string strOperation, strType;
        string strRouteNumber;
        DateTime dtBeginDate, dtEndDate, dtMakeDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        intTaskID = int.Parse(LB_TaskID.Text.Trim());
        strType = DL_RecordType.SelectedValue.Trim();
        strTask = LB_Task.Text.Trim();
        strOperatorCode = TB_OperatorCode.Text.Trim();
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


        intPriorID = int.Parse(LB_AssignID.Text.Trim());
        dtBeginDate = DateTime.Parse(DLC_BeginDate.Text);
        dtEndDate = DateTime.Parse(DLC_EndDate.Text);
        dtMakeDate = DateTime.Now;
        strRouteNumber = LB_RouteNumber.Text.Trim();

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
        taskAssignRecord.OperationTime = DateTime.Now;
        taskAssignRecord.OperatorContent = " ";
        taskAssignRecord.BeginDate = dtBeginDate;
        taskAssignRecord.EndDate = dtEndDate;
        taskAssignRecord.AssignManCode = strAssignManCode;
        taskAssignRecord.AssignManName = strAssignManName;
        taskAssignRecord.Content = "";
        taskAssignRecord.Operation = strOperation;
        taskAssignRecord.PriorID = intPriorID;
        taskAssignRecord.RouteNumber = int.Parse(strRouteNumber);
        taskAssignRecord.MakeDate = dtMakeDate;
        taskAssignRecord.Status = "ToHandle";

        taskAssignRecord.FinishedNumber = 0;
        taskAssignRecord.UnitName = ""; 
        taskAssignRecord.MoveTime = DateTime.Now;

        try
        {
            taskAssignRecordBLL.AddTaskAssignRecord(taskAssignRecord);

            string strAssignID = ShareClass.GetMyCreatedMaxTaskAssignRecordID(intTaskID.ToString(), strUserCode);
            //BusinessForm,处理关联的业务表单数据
            ShareClass.InsertOrUpdateTaskAssignRecordWFXMLData("TaskRecord", intPriorID.ToString(), "TaskRecord", strAssignID, strUserCode);

            LoadAssignRecord(LB_AssignID.Text);
            LoadChildRecord(intPriorID.ToString());

            ShareClass.SendInstantMessage(LanguageHandle.GetWord("RenWuFenPaiTongZhi"), ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeRenWu") + " :" + intTaskID.ToString() + "  " + strTask + "，" + LanguageHandle.GetWord("QingJiShiChuLi"), strUserCode, strOperatorCode);

            TB_AssignMessage.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeRenWu") + " :" + intTaskID.ToString() + "  " + "，" + LanguageHandle.GetWord("QingJiShiChuLi");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC") + "')", true);
        }
    }

    //BusinessForm,启动关联的业务表单
    protected void BT_StartupBusinessForm_Click(object sender, EventArgs e)
    {
        string strURL;
        string strTaskRedordID = LB_AssignID.Text;

        string strTemName, strIdentifyString;

        strTemName = ShareClass.getRelatedBusinessFormTemName("TaskRecord", strTaskRedordID);
        if (strTemName != "")
        {
            strIdentifyString = ShareClass.GetWLTemplateIdentifyString(strTemName);
            strURL = "popShowByURL(" + "'TTRelatedDIYBusinessForm.aspx?RelatedType=TaskRecord&RelatedID=" + strTaskRedordID + "&IdentifyString=" + strIdentifyString + "','" + LanguageHandle.GetWord("XiangGuanYeWuDan") + "', 800, 600,window.location);";
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop12", strURL, true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀

            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);
            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;
            string strDocSavePath = Server.MapPath("Doc") + "\\UserPhoto\\";

            string strPhotoURL = "<p><img src=Doc/UserPhoto/" + strFileName3 + " width=100 height=100 /></p>";


            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "');</script>");
            }
            else
            {

                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

                    if (strExtName.ToUpper().IndexOf("JPG|JPEG|PNG|BMP|GIF") >= 0)
                    {
                        //缩小尺寸，便于上传
                        ShareClass.ReducesPic(strDocSavePath, strFileName3, 640, 480, 3);
                    }

                    HT_FinishContent.Text += strPhotoURL;
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "');</script>");
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZYSCDWJ") + "');</script>");
        }
    }


    protected void BT_Activity_Click(object sender, EventArgs e)
    {
        string strHQL, strContent;
        string strID, strTaskID;
        string strUserCode;
        int intFinishPercent;

        strUserCode = LB_UserCode.Text.Trim();

        strTaskID = LB_TaskID.Text.Trim();
        strContent = HE_FinishContent.Text.Trim();
        intFinishPercent = int.Parse(NB_FinishPercent.Amount.ToString());

        Msg msg = new Msg();

        if (strContent == "")
        {
            strContent = "InProgress";
            HE_FinishContent.Text = strContent;
        }

        strID = LB_AssignID.Text.Trim();
        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];

        taskAssignRecord.OperatorContent = strContent;
        taskAssignRecord.Status = "InProgress";

        taskAssignRecord.ManHour = NB_ManHour.Amount;
        taskAssignRecord.FinishPercent = intFinishPercent;
        taskAssignRecord.MakeDate = DateTime.Now;

        taskAssignRecord.FinishedNumber = NB_FinishedNumber.Amount;
        taskAssignRecord.UnitName = LB_UnitName.Text;


        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));

            LoadAssignRecord(strID);
            ShareClass.UpdateTaskExpenseManHourSummary(strTaskID);

            //当更改任务进度
            NB_TaskProgress.Amount = ShareClass.UpdateTaskProgress(strTaskID);

            strPlanID = LB_PlanID.Text.Trim();
            if (strPlanID != "0")
            {
                //更改关联的计划进度
                if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strPlanID)) == "NO")
                {
                    ShareClass.UpdateTaskOrWorkflowPlanProgressAndExpenseWorkHour(strPlanID);
                }
            }

            //更改关联的任务的已完成量
            ShareClass.UpdateTaskFinishedNumber(strTaskID);

            //更新关联的任务关联计划的已完成量
            strPlanID = LB_PlanID.Text.Trim();
            if (strPlanID != "0")
            {
                ShareClass.UpdateProjectPlanFinishedNumber("Plan", strPlanID);
            }

            ShareClass.UpdateDailyWork(strUserCode, strProjectID, "Task", strTaskID, strContent);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            TB_Message.Text = strUserName + LanguageHandle.GetWord("ZhengZaiChuLiNiDeRenWu") + strTaskID + " " + strTaskName;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_Finish_Click(object sender, EventArgs e)
    {
        string strHQL, strContent;
        string strID, strTaskID;
        string strUserCode;
        int intFinishPercent;

        strUserCode = LB_UserCode.Text.Trim();

        strTaskID = LB_TaskID.Text.Trim();
        strContent = HE_FinishContent.Text.Trim();
        intFinishPercent = int.Parse(NB_FinishPercent.Amount.ToString());

        Msg msg = new Msg();

        if (strContent == "")
        {
            strContent = "Completed";
            HE_FinishContent.Text = strContent;
        }

        strID = LB_AssignID.Text.Trim();
        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];

        taskAssignRecord.OperatorContent = strContent;
        taskAssignRecord.Status = "Completed";

        taskAssignRecord.ManHour = NB_ManHour.Amount;
        taskAssignRecord.FinishPercent = intFinishPercent;
        taskAssignRecord.MakeDate = DateTime.Now;

        taskAssignRecord.FinishedNumber = NB_FinishedNumber.Amount;
        taskAssignRecord.UnitName = LB_UnitName.Text;

        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));

            LoadAssignRecord(strID);
            ShareClass.UpdateTaskExpenseManHourSummary(strTaskID);

            //当更改任务进度
            NB_TaskProgress.Amount = ShareClass.UpdateTaskProgress(strTaskID);

            strPlanID = LB_PlanID.Text.Trim();
            if (strPlanID != "0")
            {
                //更改关联的计划进度
                if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strPlanID)) == "NO")
                {
                    ShareClass.UpdateTaskOrWorkflowPlanProgressAndExpenseWorkHour(strPlanID);
                }
            }

            //更改关联的任务的已完成量
            ShareClass.UpdateTaskFinishedNumber(strTaskID);

            //更新关联的任务关联计划的已完成量
            strPlanID = LB_PlanID.Text.Trim();
            if (strPlanID != "0")
            {
                ShareClass.UpdateProjectPlanFinishedNumber("Plan", strPlanID);
            }

            ShareClass.UpdateDailyWork(strUserCode, strProjectID, "Task", strTaskID, strContent);

            TB_Message.Text = strUserName + LanguageHandle.GetWord("WanChengLeNiDeRenWu") + strTaskID + " " + strTaskName;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCCGNRYTJDDTXMRZDXMCLYMZL") + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWCSBJC") + "')", true);
        }
    }

    //确认影响计划进度
    protected void BT_ConfirmEffectPlanProgress_Click(object sender, EventArgs e)
    {
        strPlanID = LB_PlanID.Text.Trim();
        if (strPlanID != "0")
        {
            string strHQL, strContent;
            string strID, strTaskID;
            string strUserCode;
            int intFinishPercent;

            strUserCode = LB_UserCode.Text.Trim();

            strTaskID = LB_TaskID.Text.Trim();
            strContent = HE_FinishContent.Text.Trim();
            intFinishPercent = int.Parse(NB_FinishPercent.Amount.ToString());

            Msg msg = new Msg();

            if (strContent == "")
            {
                strContent = "Completed";
                HE_FinishContent.Text = strContent;
            }

            strID = LB_AssignID.Text.Trim();
            strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
            TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
            IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
            TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];

            taskAssignRecord.OperatorContent = strContent;
            taskAssignRecord.Status = "Completed";

            taskAssignRecord.ManHour = NB_ManHour.Amount;
            taskAssignRecord.FinishPercent = intFinishPercent;
            taskAssignRecord.MakeDate = DateTime.Now;

            try
            {
                taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));

                LoadAssignRecord(strID);

                //更改关联的任务费用和工时
                ShareClass.UpdateTaskExpenseManHourSummary(strTaskID);

                //更改关联的任务进度
                NB_TaskProgress.Amount = ShareClass.UpdateTaskProgress(strTaskID);

                //提交当天的项目日志
                ShareClass.UpdateDailyWork(strUserCode, strProjectID, "Task", strTaskID, strContent);
                TB_Message.Text = strUserName + LanguageHandle.GetWord("WanChengLeNiDeRenWu") + strTaskID + " " + strTaskName;

                //确认更改关联计划的进度、费用和工时
                ShareClass.UpdateTaskOrWorkflowPlanProgressAndExpenseWorkHour(strPlanID);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYYXGLJHJD") + "')", true);
            }
            catch
            {
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_TBD_Click(object sender, EventArgs e)
    {
        string strHQL, strContent;
        string strID, strTaskID;


        strTaskID = LB_TaskID.Text.Trim();
        strContent = HE_FinishContent.Text.Trim();


        if (strContent == "")
        {
            strContent = "Suspended";
            HE_FinishContent.Text = strContent;
        }

        strID = LB_AssignID.Text.Trim();
        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];

        taskAssignRecord.OperatorContent = strContent;
        taskAssignRecord.Status = "Suspended";


        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));
            LoadAssignRecord(strID);


            TB_Message.Text = strUserName + LanguageHandle.GetWord("GuaQiLeNiDeRenWu") + strTaskID + " " + strTaskName;

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGSBJC") + "')", true);
        }
    }

    protected void BT_CloseTask_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        projectTask.Status = "Closed";


        try
        {
            projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRWGBCG") + "')", true);

            BT_Finish.Enabled = false;
            BT_TBD.Enabled = false;
            BT_Assign.Enabled = false;
            HL_TestCase.Enabled = false;
            HL_TaskRelatedDoc.Enabled = false;
            HL_Expense.Enabled = false;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRWGBSBJC") + "')", true);
        }
    }

    protected void BT_ActiveTask_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        projectTask.Status = "InProgress";

        try
        {
            projectTaskBLL.UpdateProjectTask(projectTask, int.Parse(strTaskID));
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRWJHCG") + "')", true);

            BT_Finish.Enabled = true;
            BT_TBD.Enabled = true;
            BT_Assign.Enabled = true;
            HL_TestCase.Enabled = true;
            HL_TaskRelatedDoc.Enabled = true;
            HL_Expense.Enabled = true;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRWJHSBJC") + "')", true);
        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;

        Msg msg = new Msg();

        if (CB_ReturnMsg.Checked == true | CB_ReturnMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("ZZRWCLQKFK");
            strMsg = TB_Message.Text.Trim();

            if (CB_ReturnMsg.Checked == true)
            {
                msg.SendMSM("Message",strAssignManCode, strMsg, strUserCode);
            }

            if (CB_ReturnMail.Checked == true)
            {
                msg.SendMail(strAssignManCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB") + "')", true);
    }

    protected void BT_SendAssignMsg_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg, strOperatorCode;

        strOperatorCode = TB_OperatorCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_SendMsg.Checked == true | CB_SendMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("RenWuFenPaiTongZhi");
            strMsg = TB_AssignMessage.Text.Trim();

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

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strProjectStatus;
            string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
            TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
            TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
            lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

            taskAssignRecord = (TaskAssignRecord)lst[0];

            LB_ID.Text = taskAssignRecord.ID.ToString();

            TB_OperatorCode.Text = taskAssignRecord.OperatorCode;

            DL_RecordType.SelectedValue = taskAssignRecord.Type;

            if (strIsMobileDevice == "YES")
            {
                HT_Operation.Text = taskAssignRecord.Operation.Trim();
            }
            else
            {
                HE_Operation.Text = taskAssignRecord.Operation.Trim();
            }

            DLC_BeginDate.Text = taskAssignRecord.BeginDate.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = taskAssignRecord.EndDate.ToString("yyyy-MM-dd");

            if (strTaskStatus == "Closed")
            {
                BT_UpdateAssign.Enabled = false;
                BT_DeleteAssign.Enabled = false;
            }
            else
            {
                BT_UpdateAssign.Enabled = true;
                BT_DeleteAssign.Enabled = true;
            }

            strProjectID = LB_ProjectID.Text.Trim();
            strProjectStatus = ShareClass.GetProjectStatus(strProjectID);
            if (strProjectStatus == "Suspended" || strProjectStatus == "Cancel")
            {
                BT_UpdateAssign.Enabled = false;
                BT_DeleteAssign.Enabled = false;
                BT_Assign.Enabled = false;
            }
        }
    }


    protected void BT_UpdateAssign_Click(object sender, EventArgs e)
    {
        string strHQL, strID;
        IList lst;

        strID = LB_ID.Text.Trim();

        string strPriorID = LB_AssignID.Text.Trim();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        taskAssignRecord = (TaskAssignRecord)lst[0];

        taskAssignRecord.Type = DL_RecordType.SelectedValue.Trim();
        taskAssignRecord.OperatorContent = "";

        if (strIsMobileDevice == "YES")
        {
            taskAssignRecord.Operation = HT_Operation.Text.Trim();
        }
        else
        {
            taskAssignRecord.Operation = HE_Operation.Text.Trim();

        }
        taskAssignRecord.OperatorCode = TB_OperatorCode.Text.Trim();
        taskAssignRecord.OperatorName = ShareClass.GetUserName(TB_OperatorCode.Text.Trim());
        taskAssignRecord.BeginDate = DateTime.Parse(DLC_BeginDate.Text);
        taskAssignRecord.EndDate = DateTime.Parse(DLC_EndDate.Text);

        try
        {
            taskAssignRecordBLL.UpdateTaskAssignRecord(taskAssignRecord, int.Parse(strID));
            LoadChildRecord(strPriorID);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_DeleteAssign_Click(object sender, EventArgs e)
    {
        string strHQL, strID;
        IList lst;

        string strPriorID = LB_AssignID.Text.Trim();

        strID = LB_ID.Text.Trim();

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL); taskAssignRecord = (TaskAssignRecord)lst[0];

        try
        {
            taskAssignRecordBLL.DeleteTaskAssignRecord(taskAssignRecord);
            LoadChildRecord(strPriorID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void DL_WorkRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWorkRequest = DL_WorkRequest.SelectedValue.Trim();

        if (strIsMobileDevice == "YES")
        {
            HT_Operation.Text = strWorkRequest;
        }
        else
        {
            HE_Operation.Text = strWorkRequest;
        }
    }

    protected void UpdateTaskStatus(string strTaskID, decimal deExpenseSum)
    {
        string strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        projectTask.Expense = deExpenseSum;

        projectTaskBLL.UpdateProjectTask(projectTask, projectTask.TaskID);
    }



    protected void LoadAssignRecord(string strID)
    {
        string strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.ID = " + strID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        DataList2.DataSource = lst;
        DataList2.DataBind();
    }

    protected void LoadChildRecord(string strPriorID)
    {
        string strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.PriorID = " + strPriorID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetProjectPMCode(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.PMCode.Trim();
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

    protected string GetProjectID(string strTaskID)
    {
        string strHQL, strProjectID;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        strProjectID = projectTask.ProjectID.ToString();

        return strProjectID;
    }


}
