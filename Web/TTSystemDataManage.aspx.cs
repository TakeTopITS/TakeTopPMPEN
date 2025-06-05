using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Diagnostics;
using System.Text.RegularExpressions;

public partial class TTSystemDataManage : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "系统数据管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "系统数据管理---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_EventStart.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            DLC_PlanStart.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            DLC_WorkflowStart.Text = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            DLC_CollaborationStart.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");


            TXT_DataName.Text = ShareClass.GetSystemDBName();
            BindBackDBPrame();

            BindBackDocPrame();

            BindBackDBReadOnlyUserInformation();

            HL_ErrorLog.NavigateUrl = "Doc\\Log\\LogFile.txt";

            //取得前次备份数据时间
            LB_LastestBackupDBTime.Text = ShareClass.GetAllreadyBackupDBLastestTime();
            LB_LastestBackupDocTime.Text = ShareClass.GetAllreadyBackupDocLastestTime();
        }
    }

    protected void BT_CreateRTXAccountData_Click(object sender, EventArgs e)
    {
        try
        {
            ShareClass.RunSqlCommand("Exec Pr_CreateRTXAccountData");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSJCCG") + "')", true);
        }
        catch
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSJCSBKNYGRSTDYSCSZSJKLZJZXPR")+"')", true);
        }
    }

    protected void BT_CreateRTXDataToTXT_Click(object sender, EventArgs e)
    {
        string strExtName, strFileName, strDocURL, strDocSavePath;

        strExtName = ".txt";

        strFileName = "RTXAccountData" + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

        strDocURL = "Doc\\" + "RTXAccount\\" + strFileName;

        strDocSavePath = Server.MapPath("Doc") + "\\RTXAccount\\" + strFileName;

        try
        {
            CreateRTXAccountDataToTXT(strDocSavePath);

            HL_RTXAccountDataFile.NavigateUrl = strDocURL;
            HL_RTXAccountDataFile.Target = "_blank";

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCWJCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCWJSBKNYHSTDYSCSJC") + "')", true);
        }
    }

    protected void BT_ExportLoginLog_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("YongHuDengLuRiZhi") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";

                CreateExcel(fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC") + "')", true);
            }
        }
    }

    private void CreateExcel(string fileName)
    {
        string strHQL;

        strHQL = string.Format(@"Select ID {0}, UserCode {1},UserName {2},UserIP IP,UserHostName {3},LoginTime {4},LastestTime {5}, Position {6} From T_LogonLog ",
        LanguageHandle.GetWord("BianHao"),
        LanguageHandle.GetWord("DaiMa"),
        LanguageHandle.GetWord("XingMing"),
        LanguageHandle.GetWord("DianNaoMingCheng"),
        LanguageHandle.GetWord("DengLuShiJian"),
        LanguageHandle.GetWord("ZuiHouCaoZuoShiJian"),
        LanguageHandle.GetWord("WeiZhi"));

        strHQL += " Order by ID DESC";

        MSExcelHandler.DataTableToExcel(strHQL, fileName);
    }

    protected void BT_ClearLogoLog_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "TrunCate Table T_LogonLog";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYKRZB") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void BT_ClearSMSSent_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "TrunCate Table Sms_Send";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYKXXFSB") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }


    protected void BT_ClearLeftBar_Click(object sender, EventArgs e)
    {
        try
        {
            //给相关页面文件添加空行以刷新页面缓存
            ShareClass.AddSpaceLineToFile("TakeTopLRExLeft.aspx", "");
            ShareClass.AddSpaceLineToFile("TakeTopCSLRLeft.aspx", "");

            ////完成更新之后，此存储过程将报告已为所有的表更新了统计信息
            //ShareClass.RunSqlCommand("exec sp_updatestats");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZQingChuWanCheng") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZQingChuShiBaiQJC") + "')", true);
        }

    }
    protected void BT_ClearSystemCache_Click(object sender, EventArgs e)
    {
        try
        {
            //给相关页面文件添加空行以刷新页面缓存
            ShareClass.AddSpaceLineToFileForRefreshCache();

            ////完成更新之后，此存储过程将报告已为所有的表更新了统计信息
            //ShareClass.RunSqlCommand("exec sp_updatestats");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZQingChuWanCheng") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZQingChuShiBaiQJC") + "')", true);
        }
    }

    //数据库数据保存为文本的示例代码
    public static void CreateRTXAccountDataToTXT(string strFileName)
    {
        string strPath = strFileName;
        string strSql = "Select RTXCode,UserName,DepartNameString,RTXNumber,EMail,MBPhoneNumber From T_RTXAccountData Order By ID ASC";


        //FileStream fsobj = new FileStream(path, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(strPath, true, System.Text.Encoding.GetEncoding("GB2312"));


        sw.Write(LanguageHandle.GetWord("YongHuMing") + "\t" + LanguageHandle.GetWord("XingMing") + "\t" + LanguageHandle.GetWord("BuMenMingChen") + "\t" + LanguageHandle.GetWord("RTXFenJiHao") + "\t" + LanguageHandle.GetWord("DianZiYouXiang") + "\t" + LanguageHandle.GetWord("ShouJiHao") + "\r\n");

        DataSet ds = ShareClass.GetDataSetFromSql(strSql, "T_RTXAccountData");
        DataTable dt = ds.Tables[0];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (i < dt.Rows.Count - 1)
            {
                sw.Write(dt.Rows[i]["RTXCode"].ToString().Trim() + "\t" + dt.Rows[i]["UserName"].ToString().Trim() + "\t" + dt.Rows[i]["DepartNameString"].ToString().Trim() + "\t" + dt.Rows[i]["RTXNumber"].ToString().Trim() + "\t" + dt.Rows[i]["EMail"].ToString().Trim() + "\t" + dt.Rows[i]["MBPhoneNumber"].ToString().Trim() + "\r\n");
            }
            else
            {
                sw.Write(dt.Rows[i]["RTXCode"].ToString().Trim() + "\t" + dt.Rows[i]["UserName"].ToString().Trim() + "\t" + dt.Rows[i]["DepartNameString"].ToString().Trim() + "\t" + dt.Rows[i]["RTXNumber"].ToString().Trim() + "\t" + dt.Rows[i]["EMail"].ToString().Trim() + "\t" + dt.Rows[i]["MBPhoneNumber"].ToString().Trim());
            }
        }
        sw.Close();
    }

    protected void BT_BackupSchedule_Click(object sender, EventArgs e)
    {
        string strHQL;
        TimeSpan ts;

        string strScheduleStart = DLC_EventStart.Text;
        ts = DateTime.Now - DateTime.Parse(strScheduleStart);

        if (ts.Days >= 90)
        {
            try
            {
                strHQL = "Insert Into T_ScheduleEventBackup Select * From T_ScheduleEvent";
                strHQL += " Where to_char( EventStart, 'yyyymmdd') <= '" + DateTime.Parse(strScheduleStart).ToString("yyyyMMdd") + "'";
                strHQL += " and ID not in (Select ID From T_ScheduleEventBackup)";
                strHQL += " Order By ID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_ScheduleEvent_LeaderReviewBackup Select * From T_ScheduleEvent_LeaderReview";
                strHQL += " Where ScheduleID in (Select ID From T_ScheduleEvent";
                strHQL += " Where to_char( EventStart, 'yyyymmdd') <= '" + DateTime.Parse(strScheduleStart).ToString("yyyyMMdd") + "')";
                strHQL += " and ReviewID Not In (Select ReviewID From T_ScheduleEvent_LeaderReviewBackup)";
                strHQL += " Order By ReviewID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_ScheduleEvent";
                strHQL += " Where ID in (Select ID From T_ScheduleEventBackup)";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_ScheduleEvent_LeaderReview";
                strHQL += " Where ReviewID In (Select ReviewID From T_ScheduleEvent_LeaderReviewBackup)";

                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZShuJiZhuanYuChenGong") + "')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZNZYSGYZQDSJQJC") + "')", true);
        }
    }

    protected void BT_BackupPlan_Click(object sender, EventArgs e)
    {
        string strHQL;
        TimeSpan ts;

        string strPlanStart = DLC_PlanStart.Text;

        ts = DateTime.Now - DateTime.Parse(strPlanStart);

        if (ts.Days >= 90)
        {
            try
            {
                strHQL = "Insert Into T_PlanBackup Select * From T_Plan";
                strHQL += " Where to_char( StartTime, 'yyyymmdd') <= '" + DateTime.Parse(strPlanStart).ToString("yyyyMMdd") + "'";
                strHQL += " and PlanID Not In (Select PlanID From T_PlanBackup)";
                strHQL += " Order By PlanID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_Plan_LeaderReviewBackup Select * From T_Plan_LeaderReview";
                strHQL += " Where PlanID in (Select PlanID From T_Plan";
                strHQL += " Where to_char( StartTime, 'yyyymmdd') <= '" + DateTime.Parse(strPlanStart).ToString("yyyyMMdd") + "')";
                strHQL += " and ID Not In (Select ID From T_Plan_LeaderReviewBackup)";
                strHQL += " Order By ID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_Plan_WorkLogBackup Select * From T_Plan_WorkLog";
                strHQL += " Where PlanID in (Select PlanID From T_Plan";
                strHQL += " Where to_char( StartTime, 'yyyymmdd') <= '" + DateTime.Parse(strPlanStart).ToString("yyyyMMdd") + "')";
                strHQL += " Order By ID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_Plan";
                strHQL += " Where PlanID In (Select PlanID From T_PlanBackup)";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_Plan_LeaderReview";
                strHQL += " Where ID In (Select ID From T_Plan_LeaderReviewBackup)";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_Plan_WorkLog";
                strHQL += " Where ID In (Select ID From T_Plan_WorkLogBackup)";
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZShuJiZhuanYuChenGong") + "')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZNZYSGYZQDSJQJC") + "')", true);
        }
    }

    protected void BT_BackupCollaboration_Click(object sender, EventArgs e)
    {
        string strHQL;
        TimeSpan ts;

        string strCollaborationStart = DLC_CollaborationStart.Text;

        ts = DateTime.Now - DateTime.Parse(strCollaborationStart);

        if (ts.Days >= 90)
        {
            try
            {
                strHQL = "Insert Into T_CollaborationBackup Select * From T_Collaboration";
                strHQL += " Where to_char( CreateTime, 'yyyymmdd') <= '" + DateTime.Parse(strCollaborationStart).ToString("yyyyMMdd") + "'";
                strHQL += " and CoID Not In (Select CoID From T_CollaborationBackup)";
                strHQL += " Order By CoID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_CollaborationLogBackup Select * From T_CollaborationLog";
                strHQL += " Where LogID Not In (Select LogID From T_CollaborationLogBackup) ";
                strHQL += " and CoID In (Select CoID From T_Collaboration Where to_char( CreateTime, 'yyyymmdd') <= '" + DateTime.Parse(strCollaborationStart).ToString("yyyyMMdd") + "')";
                strHQL += " Order By LogID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_CollaborationMemberBackup Select * From T_CollaborationMember";
                strHQL += " Where MemID Not In (Select MemID From T_CollaborationMemberBackup )";
                strHQL += " and CoID In (Select CoID From T_Collaboration Where to_char( CreateTime, 'yyyymmdd') <= '" + DateTime.Parse(strCollaborationStart).ToString("yyyyMMdd") + "')";
                strHQL += " Order By MemID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_Collaboration";
                strHQL += " Where CoID In (Select CoID From T_CollaborationBackup)";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_CollaborationLog";
                strHQL += " Where LogID in (Select LogID From T_CollaborationLogBackup)";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_CollaborationMember";
                strHQL += " Where MemID in (Select MemID From T_CollaborationMemberBackup)";
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZShuJiZhuanYuChenGong") + "')", true);
            }
            catch (Exception ex)
            {
                //LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC") + " : " + LanguageHandle.GetWord("HangHao") + ": " + (lineNumber + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa") + ": " + strObjectCode + " : " + err.Message.ToString());
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZNZYSGYZQDSJQJC") + "')", true);
        }
    }

    protected void BT_BackupWorkflow_Click(object sender, EventArgs e)
    {
        string strHQL = "";
        TimeSpan ts;

        string strWorkflowStart = DLC_WorkflowStart.Text;

        ts = DateTime.Now - DateTime.Parse(strWorkflowStart);

        if (ts.Days >= 365)
        {
            try
            {
                strHQL = @"Insert Into T_WorkFlowBackup Select WLID
                              ,WLName
                              ,WLType
                              ,RelatedType
                              ,RelatedID
                              ,XMLFile
                              ,XSNFile
                              ,TemName
                              ,CreatorCode
                              ,CreatorName
                              ,CreateTime
                              ,Description
                              ,ReceiveSMS
                              ,ReceiveEMail
                              ,Status
                              ,DIYNextStep
                              ,WFXMLData
                              ,FieldList
                              ,EditFieldList
                              ,RelatedCode
                              ,CanNotNullFieldList
                              ,MainTableID
                              ,IsPlanMainWorkflow
                              ,Expense
                              ,ManHour
                              ,BusinessType
                              ,BusinessCode
                            From T_WorkFlow";
                strHQL += " Where to_char(CreateTime,'yyyymmdd') <= '" + DateTime.Parse(strWorkflowStart).ToString("yyyyMMdd") + "'";
                strHQL += " and WLID Not In (Select WLID From T_WorkflowBackup)";
                strHQL += " Order By WLID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_WorkFlowStepBackup Select * From T_WorkFlowStep";
                strHQL += " Where WLID In (Select WLID From T_WorkFlowBackup)";
                strHQL += " and StepID Not In (Select StepID From T_WorkflowStepBackup )";
                strHQL += " Order By StepID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_WorkFlowStepDetailBackup Select * From T_WorkFlowStepDetail";
                strHQL += " Where WLID In (Select WLID From T_WorkFlowBackup)";
                strHQL += " and ID Not In (Select ID From T_WorkflowStepDetailBackup)";
                strHQL += " Order By ID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_WorkFlowStepOperationBackup Select * From T_WorkFlowStepOperation";
                strHQL += " Where StepID in (Select StepID From T_WorkFlowStepBackup)";
                strHQL += " and OperationID Not In (Select OperationID From T_WorkFlowStepOperationBackup)";
                strHQL += " Order By OperationID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_WorkFlowStepBusinessMemberBackup Select * From T_WorkFlowStepBusinessMember";
                strHQL += " Where StepID in (Select StepID From T_WorkFlowStepBackup)";
                strHQL += " and ID Not In (Select ID From T_WorkFlowStepBusinessMemberBackup)";
                strHQL += " Order By ID ASC";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_ApproveFlowBackup Select * From T_ApproveFlow ";
                strHQL += " Where Type = 'Workflow'";
                strHQL += " and RelatedID In (Select WLID From T_WorkFlowBackup)";
                strHQL += " and ID Not In (Select ID From T_ApproveFlowBackup)";
                strHQL += " Order By ID ASC";
                ShareClass.RunSqlCommand(strHQL);

                try
                {
                    //删除已转移的记录
                    strHQL = "Delete From T_WorkFlow Where WLID in (Select WLID From T_WorkFlowBackup)";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_WorkFlowStep";
                    strHQL += " Where StepID In (Select StepID From T_WorkflowStepBackup)";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_WorkFlowStepDetail";
                    strHQL += " Where ID In (Select ID From T_WorkflowStepDetailBackup)";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_WorkFlowStepOperation";
                    strHQL += " Where OperationID In (Select OperationID From T_WorkFlowStepOperationBackup)";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_WorkFlowStepBusinessMember";
                    strHQL += " Where ID In (Select ID From T_WorkFlowStepBusinessMemberBackup)";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_ApproveFlow ";
                    strHQL += " Where Type = 'Workflow'";
                    strHQL += " and ID In (Select ID From T_ApproveFlowBackup)";
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                    Label1.Text = strHQL;
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZShuJiZhuanYuChenGong") + "')", true);
            }
            catch (Exception ex)
            {
                //LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC") + " : " + LanguageHandle.GetWord("HangHao") + ": " + (lineNumber + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa") + ": " + strObjectCode + " : " + err.Message.ToString());
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZNZYSEGYZQDSJQJC") + "')", true);
        }
    }

    //导入自定义表单工作流数据到数据库
    protected void BT_ImportDIYWFXMLData_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strWLID, strXMLFile, strTemName;

        int intRowCount;

        try
        {
            strHQL = "Truncate Table T_WorkFlowFormData";
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }

        strHQL = "Select WLID,XMLFile,TemName From T_WorkFlowBackup Where COALESCE(XSNFile,'0') <> '0' AND ltrim(rtrim(XSNFile)) <> ''";
        strHQL += " Union ";
        strHQL += "Select WLID,XMLFile,TemName From T_WorkFlow Where COALESCE(XSNFile,'0') <> '0' AND ltrim(rtrim(XSNFile)) <> ''";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        intRowCount = ds.Tables[0].Rows.Count;

        for (int i = 0; i < intRowCount; i++)
        {
            strWLID = ds.Tables[0].Rows[i]["WLID"].ToString().Trim();
            strXMLFile = ds.Tables[0].Rows[i]["XMLFile"].ToString().Trim();
            strTemName = ds.Tables[0].Rows[i]["TemName"].ToString().Trim();

            if (File.Exists(Server.MapPath(strXMLFile)))
            {
                try
                {
                    //把流程XML数据保存在WFXMLData列 
                    ShareClass.UpdateWFXMLData(Server.MapPath(strXMLFile), strWLID);
                }
                catch
                {
                }

                try
                {
                    //把流程XML数据保存在流程数据分析表 
                    XmlDbWorker.AddFormFromXml(Server.MapPath(strXMLFile), int.Parse(strWLID), strTemName);
                }
                catch
                {
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDRWC") + "')", true);
    }

    protected void BT_CopyData_Click(object sender, EventArgs e)
    {
        string strDBName, strBackupDBName, strBackupDBSavePath, strDirectory, strBackupDirectory, strBackupDirectorySavePath;
        string strBackupPeriodDay;
        int intResult;

        try
        {
            strBackupPeriodDay = int.Parse(NB_BackDBPeriodDay.Amount.ToString()).ToString();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click111", "alert('" + LanguageHandle.GetWord("ZZJGBFJGTSBXWSZJC") + "')", true);
            return;
        }

        if (strBackupPeriodDay == "0" | int.Parse(strBackupPeriodDay) < 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click222", "alert('" + LanguageHandle.GetWord("ZZJGBFJGTSBXDYLWZSJC") + "')", true);
            return;
        }


        strDirectory = TXT_BackDBUrl.Text.Trim();
        strBackupDirectory = DateTime.Now.ToString("yyyyMMdd");
        strBackupDirectorySavePath = strDirectory + "\\" + strBackupDirectory;

        strDBName = TXT_DataName.Text.Trim();
        strBackupDBName = strDBName + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".bak";
        strBackupDBSavePath = strBackupDirectorySavePath + "\\" + strBackupDBName;

        if (strBackupDirectorySavePath != "")
        {
            intResult = ShareClass.CreateDirectory(strBackupDirectorySavePath);
            if (intResult == 2)
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoBeiFenMuLuChuangJianL")+"')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click333", "alert('" + LanguageHandle.GetWord("ZZJGBFMLBNWKJC") + "')", true);
            return;
        }

        FileInfo fi = new FileInfo(strBackupDBSavePath);

        if (!fi.Exists)
        {
            try
            {
                //先查询是否存在，如果不存在，则插入，存在，则更新
                string strSelectBackDBHQL = "select * from T_BackDBPrame";
                DataTable dtBackDB = ShareClass.GetDataSetFromSql(strSelectBackDBHQL, "strSelectBackDBHQL").Tables[0];
                if (dtBackDB != null && dtBackDB.Rows.Count > 0)
                {
                    //把路径改为最新
                    string strIsertBackDBHQL = string.Format(@"update T_BackDBPrame set BackDBUrl = '{0}',BackPeriodDay = {1}", strDirectory, strBackupPeriodDay);
                    ShareClass.RunSqlCommand(strIsertBackDBHQL);
                }
                else
                {
                    //把路径改为最新
                    string strIsertBackDBHQL = string.Format(@"insert into T_BackDBPrame(BackDBUrl,BackPeriodDay) values('{0}', {1})", strDirectory, strBackupPeriodDay);
                    ShareClass.RunSqlCommand(strIsertBackDBHQL);
                }

                //备份数据库
                ShareClass.BackupCurrentSiteDB(ShareClass.GetSystemDBName(), ShareClass.GetSystemDBBackupSaveDir(), strUserName,"SELF");

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click444", "alert('" + LanguageHandle.GetWord("ZZSJKBFWC") + "')", true);
            }
            catch (Exception err)
            {
                //写日志
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click555", "alert('" + LanguageHandle.GetWord("ZZSJKBFSBKNSJKMCYCJC") + "')", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("BFSBCZTMWJJC") + "')", true);
        }
    }


    protected void BT_CopyDirectory_Click(object sender, EventArgs e)
    {
        string strBackupDirectorySavePath, strDirectory, strBackupDirectory;
        string strBackupPeriodDay;

        try
        {
            if (ShareClass.GetBackupDBLastestTimeDifferMonth() > 1)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click099", "alert('" + LanguageHandle.GetWord("CGLYWBFQBSCBFSJDSYZQDSJZYCZTBF") + "')", true);
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

        try
        {
            strBackupPeriodDay = int.Parse(NB_BackDocPeriodDay.Amount.ToString()).ToString();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click011", "alert('" + LanguageHandle.GetWord("ZZJGBFJGTSBXWSZJC") + "')", true);
            return;
        }

        if (strBackupPeriodDay == "0" | int.Parse(strBackupPeriodDay) < 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click022", "alert('" + LanguageHandle.GetWord("ZZJGBFJGTSBXDYLWZSJC") + "')", true);
            return;
        }

        strDirectory = TXT_BackDocUrl.Text.Trim();
        strBackupDirectory = DateTime.Now.ToString("yyyyMMdd");
        strBackupDirectorySavePath = strDirectory + "\\" + strBackupDirectory;

        try
        {
            //先查询是否存在，如果不存在，则插入，存在，则更新
            string strSelectBackDocHQL = "select * from T_BackDocPrame";
            DataTable dtBackDoc = ShareClass.GetDataSetFromSql(strSelectBackDocHQL, "strSelectBackDocHQL").Tables[0];
            if (dtBackDoc != null && dtBackDoc.Rows.Count > 0)
            {
                //把路径改为最新
                string strIsertBackDocHQL = string.Format(@"update T_BackDocPrame set BackDocUrl = '{0}',BackPeriodDay = {1}", strDirectory, strBackupPeriodDay);
                ShareClass.RunSqlCommand(strIsertBackDocHQL);
            }
            else
            {
                //把路径改为最新
                string strIsertBackDocHQL = string.Format(@"insert into T_BackDocPrame(BackDocUrl,BackPeriodDay) values('{0}', {1})", strDirectory, strBackupPeriodDay);
                ShareClass.RunSqlCommand(strIsertBackDocHQL);
            }

            ShareClass.BackupCurrentSiteDoc(strUserName);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click044", "alert('" + LanguageHandle.GetWord("WenDang") + LanguageHandle.GetWord("ZZBeiFenChengGong") + "')", true);
        }
        catch (Exception err)
        {
            //写日志
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("WenDang") + LanguageHandle.GetWord("ZZBeiFenShiBaiQingJianCha") + ": " + err.Message.ToString() + "')", true);
        }
    }

    protected void BT_SaveDBPath_Click(object sender, EventArgs e)
    {
        string strDirectory, strBackPeriodDay;
        strDirectory = TXT_BackDBUrl.Text.Trim();
        strBackPeriodDay = NB_BackDBPeriodDay.Amount.ToString();

        //先查询是否存在，如果不存在，则插入，存在，则更新
        string strSelectBackDBHQL = "select * from T_BackDBPrame";
        DataTable dtBackDB = ShareClass.GetDataSetFromSql(strSelectBackDBHQL, "strSelectBackDBHQL").Tables[0];
        if (dtBackDB != null && dtBackDB.Rows.Count > 0)
        {
            //把路径改为最新
            string strIsertBackDBHQL = string.Format(@"update T_BackDBPrame set BackDBUrl = '{0}',BackPeriodDay= {1}", strDirectory, strBackPeriodDay);
            ShareClass.RunSqlCommand(strIsertBackDBHQL);
        }
        else
        {
            //把路径改为最新
            string strIsertBackDBHQL = string.Format(@"insert into T_BackDBPrame(BackDBUrl,BackPeriodDay) values('{0}',{1})", strDirectory, strBackPeriodDay);
            ShareClass.RunSqlCommand(strIsertBackDBHQL);
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
    }

    protected void BT_SaveDocPath_Click(object sender, EventArgs e)
    {
        string strDirectory, strBackPeriodDay;
        strDirectory = TXT_BackDocUrl.Text.Trim();
        strBackPeriodDay = NB_BackDocPeriodDay.Amount.ToString();
        //先查询是否存在，如果不存在，则插入，存在，则更新
        string strSelectBackDocHQL = "select * from T_BackDocPrame";
        DataTable dtBackDoc = ShareClass.GetDataSetFromSql(strSelectBackDocHQL, "strSelectBackDocHQL").Tables[0];
        if (dtBackDoc != null && dtBackDoc.Rows.Count > 0)
        {
            //把路径改为最新
            string strIsertBackDocHQL = string.Format(@"update T_BackDocPrame set BackDocUrl = '{0}', BackPeriodDay={1}", strDirectory, strBackPeriodDay);
            ShareClass.RunSqlCommand(strIsertBackDocHQL);
        }
        else
        {
            //把路径改为最新
            string strIsertBackDocHQL = string.Format(@"insert into T_BackDocPrame(BackDocUrl,BackPeriodDay) values('{0}',{1})", strDirectory, strBackPeriodDay);
            ShareClass.RunSqlCommand(strIsertBackDocHQL);
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
    }

    protected void BT_SaveDBUserIDPassword_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDBUserID = LB_DBUserID.Text.Trim();
        string strPassword = TB_ReadOnlyUserPassword.Text.Trim();

        if (strPassword != "")
        {
            strHQL = "truncate table t_dbreadonlyuserinfor";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format("Insert Into T_DBReadOnlyUserInfor(DBUserID,Password) values('{0}','{1}')", strDBUserID, strPassword);
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("ZZMMBNWKJC") + "')", true);
        }
    }

    protected void BT_DeleteDBUserIDPassword_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "truncate table t_dbreadonlyuserinfor";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);

            BindBackDBReadOnlyUserInformation();
        }
        catch (Exception err)
        {
            //写日志
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }

    }

    private void BindBackDBPrame()
    {
        string strBackDBHQL = "select * from T_BackDBPrame";
        DataTable dtBackDB = ShareClass.GetDataSetFromSql(strBackDBHQL, "strBackDBHQL").Tables[0];

        if (dtBackDB != null && dtBackDB.Rows.Count > 0)
        {
            DataRow dr = dtBackDB.Rows[0];
            TXT_BackDBUrl.Text = dr["BackDBUrl"] == DBNull.Value ? "" : dr["BackDBUrl"].ToString();
            NB_BackDBPeriodDay.Amount = int.Parse(dr["BackPeriodDay"] == DBNull.Value ? "" : dr["BackPeriodDay"].ToString());
        }
    }

    private void BindBackDocPrame()
    {
        string strBackDocHQL = "select * from T_BackDocPrame";
        DataTable dtBackDoc = ShareClass.GetDataSetFromSql(strBackDocHQL, "strBackDocHQL").Tables[0];

        if (dtBackDoc != null && dtBackDoc.Rows.Count > 0)
        {
            DataRow dr = dtBackDoc.Rows[0];
            TXT_BackDocUrl.Text = dr["BackDocUrl"] == DBNull.Value ? "" : dr["BackDocUrl"].ToString();
            NB_BackDocPeriodDay.Amount = int.Parse(dr["BackPeriodDay"] == DBNull.Value ? "" : dr["BackPeriodDay"].ToString());
        }
    }

    private void BindBackDBReadOnlyUserInformation()
    {
        LB_DBUserID.Text = ShareClass.getDBReadOnlyUserID();
    }
}
