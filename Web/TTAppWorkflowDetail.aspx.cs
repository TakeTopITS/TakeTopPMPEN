using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TakeTopWF;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTAppWorkflowDetail : System.Web.UI.Page
{
    string strRequsite, strAutoSaveWFOperator;
    string strRelatedType, strRelatedID, strStepName, strWFTemStepGUID;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strID = Request.QueryString["ID"];
        string strStepID, strTemStepID;

        string strWLID;
        string strWLName;
        string strTemName;
        string strXMLFile;
        string strHQL;
        string strReceiveEMail, strReceiveSMS, strDIYNextStep;

        IList lst;
        string strWFStatus, strStepOperatorStatus;
        string strUserCode, strUserName, strCreatorCode, strUserType;
        string strCurrentStepSortNumber;

        XmlDocument docXml = new XmlDocument();

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strAutoSaveWFOperator = Session["AutoSaveWFOperator"].ToString().Trim();

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.ID = " + strID;
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
        WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst[0];

        strStepID = workFlowStepDetail.StepID.ToString();
        strStepName = GetWorkFlowStepName(strStepID);

        strWLID = workFlowStepDetail.WLID.ToString();
        strRequsite = workFlowStepDetail.Requisite.Trim();

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        strWLID = workFlow.WLID.ToString().Trim();
        strWLName = workFlow.WLName.Trim();
        strReceiveEMail = workFlow.ReceiveEMail.Trim();
        strReceiveSMS = workFlow.ReceiveSMS.Trim();
        strDIYNextStep = workFlow.DIYNextStep.Trim();
        strCreatorCode = workFlow.CreatorCode.Trim();
        strTemName = workFlow.TemName.Trim();
        strXMLFile = workFlow.XMLFile.Trim();

        strRelatedType = workFlow.RelatedType.Trim();
        strRelatedID = workFlow.RelatedID.ToString();
        strWFStatus = workFlow.Status.Trim();

        DataList2.DataSource = lst;
        DataList2.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack != true)
        {
            if (strAutoSaveWFOperator == "NO")
            {
                BT_SaveOperatorSelect.Visible = true;
            }
            else
            {
                BT_SaveOperatorSelect.Visible = false;
            }

            if (Session["DIYWFModule"].ToString() == "YES")
            {
                a_return.Visible = false;
            }

            DLC_SignDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strWFStatus = LB_Status.Text.Trim();

            LB_UserCode.Text = strUserCode;
            strUserName = ShareClass.GetUserName(strUserCode);
            LB_UserName.Text = strUserName;

            HL_ApproveRecord.NavigateUrl = "TTWorkflowApproveRecord.aspx?Type=WorkFlow&RelatedID=" + strWLID + "&StepID=" + strStepID;
            HL_StepApproveRecord.NavigateUrl = "TTWorkflowApproveRecord.aspx?Type=Step&RelatedID=" + strWLID + "&StepID=" + strStepID;

            HL_WLRelatedDoc.NavigateUrl = "TTAPPWLRelatedDoc.aspx?WLID=" + strWLID + "&ID=" + strID;

            HL_RelatedMeeting.NavigateUrl = "TTMakeWLMeeting.aspx?MeetingID=" + strWLID;
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=WORKFLOW&RelatedID=" + strWLID;

            HL_BusinessMember.NavigateUrl = "TTWorkFlowStepBusinessMember.aspx?StepID=" + strStepID;

            if (strRelatedType == "Plan")
            {
                HL_Expense.NavigateUrl = "TTProExpense.aspx?ProjectID=" + GetProjectIDByPlanID(strRelatedID) + "&TaskID=0&RecordID=0&PlanID=" + strRelatedID + "&WorkflowStepDetailID=" + strID;
            }
            else
            {
                if (strRelatedType == "Project")
                {
                    HL_Expense.NavigateUrl = "TTProExpense.aspx?ProjectID=" + strRelatedID + "&TaskID=0&RecordID=0&PlanID=" + strRelatedID + "&WorkflowStepDetailID=" + strID;
                }
                else
                {
                    HL_Expense.NavigateUrl = "TTProExpense.aspx?ProjectID=1&TaskID=0&RecordID=0&PlanID=0&WorkflowStepDetailID=" + strID;
                }
            }


            strUserType = ShareClass.GetUserType(strUserCode);
            if (strUserType != "INNER")
            {
                HL_RelatedMeeting.Visible = false;
                HL_MakeCollaboration.Visible = false;
            }

            LB_WLID.Text = workFlow.WLID.ToString();
            LB_WLType.Text = workFlow.WLType.Trim();
            TB_WLName.Text = workFlow.WLName.Trim();
            LB_WFName.Text = workFlow.WLName.Trim();
            TB_WLDescription.Text = workFlow.Description.Trim();
            LB_WLDescription.Text = workFlow.Description.Trim();
            LB_TemName.Text = workFlow.TemName.Trim();
            LB_CreatorCode.Text = workFlow.CreatorCode.Trim();
            LB_CreatorName.Text = workFlow.CreatorName.Trim();
            LB_CreateTime.Text = workFlow.CreateTime.ToString();
            LB_Status.Text = workFlow.Status.Trim();

            try
            {
                LB_IsPlanMainWorkflow.Text = workFlow.IsPlanMainWorkflow.Trim();
            }
            catch
            {
                LB_IsPlanMainWorkflow.Text = "NO";
            }

            strWFStatus = LB_Status.Text.Trim();
            strWLName = TB_WLName.Text.Trim();
            strTemName = LB_TemName.Text.Trim();

            LoadWorkFlowStep(strStepID);
            LoadWorkFlowStepDetailByID(strID);
            LoadRelatedWFTemplateStepList(strTemName);
            LoadWorkflowApproveRecord(strWLID);

            try
            {
                ////显示流程红绿灯
                //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strWFStatus, Repeater1);

                string strDesignType = ShareClass.GetWLTemplateDesignType(strTemName);
                if (strDesignType == "SL")
                {
                    HL_WFChartView.NavigateUrl = "TTWFChartViewSL.aspx?WLID=" + LB_WLID.Text + "&IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(strTemName);
                }
                if (strDesignType == "JS")
                {
                    HL_WFChartView.NavigateUrl = "TTWFChartViewJS.aspx?WLID=" + LB_WLID.Text + "&IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(strTemName);
                }

                //列出由要此审批者选择的下一步审批者
                LoadNextIsPriorSelectWorkFlowStep(strTemName, strWLID, strStepID);

                //取得当前步骤的步序号，并列出小于此步序的所有步骤
                strCurrentStepSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID).ToString();
                LoadWorkFlowTStep(strTemName, strCurrentStepSortNumber);

                LB_SortNumber.Text = strCurrentStepSortNumber;


                //取得归属于此步的子流程模板
                WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, int.Parse(strCurrentStepSortNumber));
                strTemStepID = workFlowTStep.StepID.ToString();
                LoadWFTStepRelatedTem(strTemStepID);
                LB_TemStepID.Text = strTemStepID;

                strWFTemStepGUID = workFlowTStep.GUID.Trim();

                LB_FinishPercent.Text = workFlowTStep.FinishPercent.ToString();

                //取得归属于此步的子工作流
                LoadWFStepChildWF(strWLID, strStepID, strTemName, int.Parse(strCurrentStepSortNumber));

                //列出子工作流
                LoadChildWorkflow(strWLID);

                try
                {
                    //列出关联模块
                    ShareClass.LoadWorkFlowTStepRelatedModule(RP_RelatedModule, strWLID, strStepID, strID, strWFTemStepGUID, strLangCode, strUserCode);
                }
                catch
                {
                }

                //是否允许本步加签
                if (ShareClass.GetWorkflowTemplateStepAllowCurrentStepAddApprover(strTemStepID) == "NO")
                {
                    BT_AddApprover.Visible = false;
                }
                else
                {
                    BT_AddApprover.Visible = true;
                }


                workFlowStepDetail = GetWorkFlowStepDetail(strID);
                strStepOperatorStatus = workFlowStepDetail.Status.Trim();
                strRequsite = workFlowStepDetail.Requisite.Trim();

                NB_ManHour.Amount = workFlowStepDetail.ManHour;

                if (strDIYNextStep == "YES" & int.Parse(strCurrentStepSortNumber) > 1)
                {
                    Panel_DIYStep.Visible = true;
                }
                else
                {
                    Panel_DIYStep.Visible = false;
                }

                //判断工作流数据文件是否存在
                try
                {
                    docXml.Load(Server.MapPath(strXMLFile));
                }
                catch
                {
                    BT_Agree.Enabled = false;
                    BT_Cancel.Enabled = false;
                    BT_Checking.Enabled = false;
                    BT_Refuse.Enabled = false;
                    BT_Signing.Enabled = false;
                    BT_Review.Enabled = false;
                    BT_BackPirorStep.Enabled = false;

                    BT_SaveWFRelatedData.Enabled = false;

                    BT_AddApprover.Enabled = false;

                    return;
                }

                //工作流步骤操作状态
                if (strStepOperatorStatus == "Approved")
                {
                    BT_Agree.Enabled = false;

                    //如此步后存在被批准的步骤，则不能撤消
                    if (GetWorkFlowNextStepDetailAlreadyApprovedNumber(strWLID, strStepID) == 0)
                    {
                        BT_Cancel.Enabled = true;
                    }
                    else
                    {
                        BT_Cancel.Enabled = false;
                    }

                    BT_Checking.Enabled = false;
                    BT_Refuse.Enabled = false;
                    BT_Signing.Enabled = false;
                    BT_Review.Enabled = false;
                    BT_BackPirorStep.Enabled = false;

                    BT_SaveWFRelatedData.Enabled = false;

                    BT_AddApprover.Enabled = false;
                }

                if (strWFStatus == "Closed" | strWFStatus == "Updating" | strWFStatus == "CaseClosed")
                {
                    BT_Agree.Enabled = false;
                    BT_Cancel.Enabled = false;
                    BT_Checking.Enabled = false;
                    BT_Refuse.Enabled = false;
                    BT_Signing.Enabled = false;
                    BT_Review.Enabled = false;
                    BT_BackPirorStep.Enabled = false;

                    BT_SaveWFRelatedData.Enabled = false;

                    BT_AddApprover.Enabled = false;
                }

                if (strReceiveEMail == "YES")
                {
                    CB_Mail.Checked = true;
                }
                if (strReceiveSMS == "YES")
                {
                    CB_SMS.Checked = true;
                }

                LB_WLID.Text = strWLID;
                LB_StepID.Text = strStepID;
                LB_ID.Text = strID;

                //附加工作流步骤用户自定义的JSCode到页面
                WFShareClass.AttachUserJSCodeFromWFTemplateStep(strTemName, strCurrentStepSortNumber, LIT_AttachUserWFStepJSCode, strCreatorCode);
            }
            catch (Exception err)
            {
                //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGZLMBBCZQJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_SaveWFRelatedData_Click(object sender, EventArgs e)
    {
        string strID, strWLID;
        decimal deManHour;

        strID = LB_ID.Text;
        strWLID = LB_WLID.Text;

        deManHour = NB_ManHour.Amount;

        try
        {
            //更新流程的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, deManHour);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Agree_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strStepID = LB_StepID.Text.Trim(); ;
        string strWLID = LB_WLID.Text.Trim();
        string strWFName = TB_WLName.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        string strIsCloseCurrentWFPage = "NO";
        try
        {
            strIsCloseCurrentWFPage = System.Configuration.ConfigurationManager.AppSettings["CloseCurrentWFTab"];
        }
        catch
        {
        }

        string strTitle = LanguageHandle.GetWord("GongZuoLiuGuanLi").ToString().Trim();
        string strMsgText = LanguageHandle.GetWord("ZZPZCG").ToString().Trim();

        string strSignDate;
        try
        {
            strSignDate = DateTime.Parse(DLC_SignDate.Text).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
        }
        catch
        {
            strSignDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }


        WFDetailHandle wfDetailHandle = new WFDetailHandle();
        int intResult = wfDetailHandle.AgreeWF(strTemName, strWLType, strWLID, strWFName, strStatus, strContent, strStepID, strID, strRelatedID, strUserCode, strSignDate,
                     TB_Content, TB_Message, LB_Status, LB_NextStepID, LB_NextSortNumber, LB_NextStepName, BT_Agree, BT_Cancel, BT_Checking, BT_Refuse, BT_Signing, BT_Review, BT_BackPirorStep,
                     DataGrid1, DataGrid2, DataGrid4, DataGrid5, DataGrid6, Repeater1, UpdatePanel1, Panel_NextStep, Panel_BelongChildWF);

        if (intResult >= 0)
        {
            //添加签名域
            WFDataHandle wfDataHandle = new WFDataHandle();
            wfDataHandle.AddUserSignPictureField(strWLID, strID, strSignDate, Session["UserCode"].ToString());

            ////显示流程红绿灯
            //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

            TB_Message.Text = ShareClass.GetUserName(strUserCode).Trim() + LanguageHandle.GetWord("ZZZPZLNDGZLSQ").ToString().Trim() + "：" + strWLID + " " + GetWorkFlow(strWLID).WLName.Trim();

            //刷新流程数据页，兼容FIREFOX和其它所有浏览器
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click1", "window.parent.frames['right'].location.href = window.parent.frames['right'].location.href ", true);

            if (intResult == 0)
            {
                if (strIsCloseCurrentWFPage != "YES")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "CloseTab('" + strMsgText + "')", true);
                }
            }

            if (intResult == 11)
            {
                //更改相关业务对象的状态值
                UpdateRelatedBusinessStatus(strWLType, strRelatedID, "Agree");
                if (strIsCloseCurrentWFPage != "YES")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "CloseTab('" + strMsgText + "')", true);
                }
            }

            if (intResult == 1)
            {
                if (strIsCloseCurrentWFPage != "YES")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "CloseTab('" + strMsgText + "')", true);
                }
            }

            if (intResult == 2)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click111", "alert('" + LanguageHandle.GetWord("ZZPZCGXYBSPZYYNDZ").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click222", "ChangeMenu(0)", true);
            }

            if (intResult == 3)
            {
                if (strIsCloseCurrentWFPage != "YES")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCGDCBHXTCYRYPZCNTG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "CloseTab('" + strMsgText + "')", true);
                }
            }

            if (intResult == 4)
            {
                if (strIsCloseCurrentWFPage != "YES")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCGCBZYJBSPTGL").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "CloseTab('" + strMsgText + "')", true);
                }
            }

            if (CB_SMS.Checked == true | CB_Mail.Checked == true)
            {
                try
                {
                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            //发送信息通知申请者
                            Msg msg = new Msg();
                            msg.SendMSM("Message", LB_CreatorCode.Text.Trim(), LB_UserCode.Text.Trim() + LB_UserName.Text.Trim() + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + LanguageHandle.GetWord("PiZhun").ToString().Trim() + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName, strUserCode);
                        }
                        catch
                        {
                        }
                    }).Start();
                }
                catch
                {
                }
            }

            try
            {
                //发送消息给额外的人员
                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        Msg msg = new Msg();
                        msg.SendMsgToOtherMemberForWorkflow(strUserCode, strTemName, LB_TemStepID.Text.Trim(), strWLID, strWFName, strStepID, strStepName, "AgreeNotice", LanguageHandle.GetWord("PiZhun").ToString().Trim());
                    }
                    catch
                    {

                    }
                }).Start();
            }
            catch
            {
            }


            try
            {
                //如流程是项目计划发起的，更新关联项目计划完成程度
                if (strRelatedType == "Plan" & strRelatedID != "0")
                {
                    //更改关联的计划进度
                    if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strRelatedID)) == "NO")
                    {
                        ShareClass.UpdateProjectPlanSchedule(strRelatedType, strRelatedID);
                    }
                }

                //如果流程是由项目或项目计划发起的，那么增加项目日志到项目中
                if (strContent == "")
                {
                    strContent = "Approve workflow：" + strWLID + strWFName;
                }
                ShareClass.UpdateProjectDaiyWorkByWorkflow(strRelatedType, strRelatedID, strWLID, strContent, strUserCode);
            }
            catch (Exception err)
            {
                //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }

            //批准成功后，如果有用户附加JS代码，则执行
            try
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "myscript", "AfterAgree();", true);
            }
            catch (Exception err)
            {
                //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
        else
        {
            if (intResult == -1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZSBYBXDDZGZLMYDHGBCBDYSPTGDZLCMYTGJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click222", "ChangeMenu(0)", true);
            }

            if (intResult == -2)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCGZLSJWJBCZBNSPJC").ToString().Trim() + "')", true);

            }

            if (intResult == -3)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZBZSYBYQTGDBZMTGQJC").ToString().Trim() + "')", true);

            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        try
        {
            //更新处理流程花费的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, NB_ManHour.Amount);
            //更新处理流程花费的费用
            ShareClass.UpdateWorkFlowExpense(strRelatedType, strRelatedID, strWLID, strID);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void BT_BackPirorStep_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strStepID = LB_StepID.Text.Trim(); ;
        string strWLID = LB_WLID.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strWFName = TB_WLName.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();

        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        string strSignDate;
        try
        {
            strSignDate = DateTime.Parse(DLC_SignDate.Text).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
        }
        catch
        {
            strSignDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        WFDetailHandle wfDetailHandle = new WFDetailHandle();
        int intResult = wfDetailHandle.BackPirorStepWF(strTemName, strWLType, strWLID, strWFName, strStatus, strContent, strStepID, strID, strRelatedID, strUserCode, strSignDate,
                     TB_Content, TB_Message, LB_Status, LB_NextStepID, LB_NextSortNumber, LB_NextStepName, BT_Agree, BT_Cancel, BT_Checking, BT_Refuse, BT_Signing, BT_Review, BT_BackPirorStep,
                     DataGrid1, DataGrid2, DataGrid4, DataGrid5, DataGrid6, Repeater1, UpdatePanel1, Panel_NextStep, Panel_BelongChildWF, DL_NextStep);

        if (intResult > 0)
        {
            //添加签名域
            WFDataHandle wfDataHandle = new WFDataHandle();
            wfDataHandle.AddUserSignPictureField(strWLID, strID, strSignDate, Session["UserCode"].ToString());

            ////显示流程红绿灯
            //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

            //刷新流程数据页，兼容FIREFOX和其它所有浏览器
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click1", "window.parent.frames['right'].location.href = window.parent.frames['right'].location.href ", true);


            if (intResult == 1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHHCG").ToString().Trim() + "')", true);
            }

            if (intResult == 2)
            {
                TB_Message.Text = ShareClass.GetUserName(strUserCode).Trim() + LanguageHandle.GetWord("ZZZBNDGZLSQ").ToString().Trim() + " ：" + strWLID + " " + strWFName + " "+LanguageHandle.GetWord("FanHuiDaoDi")+"" + DL_NextStep.SelectedValue.Trim() + " "+LanguageHandle.GetWord("BuLe")+"！";   

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSYFHMBZZDSPZ").ToString().Trim() + "')", true);
            }

            if (intResult == 3)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGPZFHCGDCBHXTCYRYPZTGHCNFHMBZ").ToString().Trim() + "')", true);
            }

            if (intResult == 4)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSCBZYJBSPTGBYJBFHDMBZ").ToString().Trim() + "')", true);
            }


            if (CB_SMS.Checked == true | CB_Mail.Checked == true)
            {
                try
                {
                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            //发送没有发送的信息
                            Msg msg = new Msg();
                            msg.SendMSM("Message", LB_CreatorCode.Text.Trim(), LB_UserCode.Text.Trim() + LB_UserName.Text.Trim() + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + LanguageHandle.GetWord("PiZhunFanHui").ToString().Trim() + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName, strUserCode);
                        }
                        catch
                        {

                        }
                    }).Start();
                }
                catch
                {
                }
            }

            try
            {
                //发送消息给额外的人员
                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        Msg msg = new Msg();
                        msg.SendMsgToOtherMemberForWorkflow(strUserCode, strTemName, LB_TemStepID.Text.Trim(), strWLID, strWFName, strStepID, strStepName, "AgreeBackNotice", LanguageHandle.GetWord("PiZhunFanHui").ToString().Trim());
                    }
                    catch (Exception err)
                    {
                    }
                }).Start();
            }
            catch (Exception err)
            {
            }

            try
            {
                //如流程是项目计划发起的，更新关联项目计划完成程度
                if (strRelatedType == "Plan" & strRelatedID != "0")
                {
                    //更改关联的计划进度
                    if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strRelatedID)) == "NO")
                    {
                        ShareClass.UpdateProjectPlanSchedule(strRelatedType, strRelatedID);
                    }
                }


                //如果流程是由项目或项目计划发起的，那么增加项目日志到项目中
                if (strContent == "")
                {
                    strContent = "Approve workflow：" + strWLID + strWFName + " to back prior step !";
                }
                ShareClass.UpdateProjectDaiyWorkByWorkflow(strRelatedType, strRelatedID, strWLID, strContent, strUserCode);
            }
            catch (Exception err)
            {
                //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }


            //返回前面步骤后，如果有用户附加JS代码，则执行
            try
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "myscript", "AfterBackPirorStep();", true);
            }
            catch
            {
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFHMBZSBJC").ToString().Trim() + "')", true);
        }

        try
        {
            //更新处理流程花费的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, NB_ManHour.Amount);
            //更新处理流程花费的费用
            ShareClass.UpdateWorkFlowExpense(strRelatedType, strRelatedID, strWLID, strID);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void BT_Refuse_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strStepID = LB_StepID.Text.Trim();
        string strWLID = LB_WLID.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strWFName = TB_WLName.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();

        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        WFDetailHandle wfDetailHandle = new WFDetailHandle();
        int intResult = wfDetailHandle.RefuseWF(strTemName, strWLType, strWLID, strWFName, strStatus, strContent, strStepID, strID, strRelatedID, strUserCode,
                     TB_Content, TB_Message, LB_Status, LB_NextStepID, LB_NextSortNumber, LB_NextStepName, BT_Agree, BT_Cancel, BT_Checking, BT_Refuse, BT_Signing, BT_Review, BT_BackPirorStep,
                     DataGrid1, DataGrid2, DataGrid4, DataGrid5, DataGrid6, Repeater1, UpdatePanel1, Panel_NextStep, Panel_BelongChildWF);

        if (intResult == 1)
        {
            TB_Message.Text = ShareClass.GetUserName(strUserCode).Trim() + LanguageHandle.GetWord("ZZZBHLNDGZLSQ").ToString().Trim() + " ：" + strWLID + " " + GetWorkFlow(strWLID).WLName.Trim();

            //判断整个工作流审批通过情况，如果用户是必须审批的人，那么驳回将终止流程
            int intSortNumber = wfDetailHandle.GetWorkFlowStep(strStepID).SortNumber;
            WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, intSortNumber);
            int intLimitedOperator = workFlowTStep.LimitedOperator;
            bool blStatus = wfDetailHandle.CheckStepFinishedStatus(strWLID, strStepID, intLimitedOperator);
            if (blStatus == false)
            {
                //判断此用户在此流程步骤中是不是必须审批的人
                bool blRequisite = CheckOperatorIsRequisiteForWorkflowStep(strWLID, strStepID, strUserCode);
                if (blRequisite == true)
                {
                    string strHQL;

                    strHQL = "Update T_WorkFlow set Status = 'Rejected' where WLID = " + strWLID;
                    WFShareClass.RunSqlCommand(strHQL);

                    strHQL = "Update T_WorkFlowStep Set Status = 'Rejected' Where WLID = " + strWLID + " and SortNumber = " + workFlowTStep.SortNumber.ToString();
                    WFShareClass.RunSqlCommand(strHQL);

                    LB_Status.Text = "Rejected";
                }
            }

            ////显示流程红绿灯
            //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBHCG").ToString().Trim() + "')", true);

            if (CB_SMS.Checked == true | CB_Mail.Checked == true)
            {
                try
                {
                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            //发送没有发送的信息
                            Msg msg = new Msg();
                            msg.SendMSM("Message", LB_CreatorCode.Text.Trim(), LB_UserCode.Text.Trim() + LB_UserName.Text.Trim() + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + LanguageHandle.GetWord("BoHui").ToString().Trim() + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName, strUserCode);

                        }
                        catch
                        {

                        }
                    }).Start();
                }
                catch
                {
                }
            }

            try
            {
                //发送消息给额外的人员
                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        Msg msg = new Msg();
                        msg.SendMsgToOtherMemberForWorkflow(strUserCode, strTemName, LB_TemStepID.Text.Trim(), strWLID, strWFName, strStepID, strStepName, "RefuseNotice", LanguageHandle.GetWord("BoHuiZhongZhi").ToString().Trim());
                    }
                    catch (Exception err)
                    {
                    }
                }).Start();
            }
            catch (Exception err)
            {
            }


            //驳回终止后，如果有用户附加JS代码，则执行
            try
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "myscript", "AfterRefuseAgree();", true);
            }
            catch
            {
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        try
        {
            //更新处理流程花费的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, NB_ManHour.Amount);
            //更新处理流程花费的费用
            ShareClass.UpdateWorkFlowExpense(strRelatedType, strRelatedID, strWLID, strID);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strStepID = LB_StepID.Text.Trim();
        string strWLID = LB_WLID.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strWFName = TB_WLName.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();

        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();


        WFDetailHandle wfDetailHandle = new WFDetailHandle();
        int intResult = wfDetailHandle.CancelWF(strTemName, strWLType, strWLID, strWFName, strStatus, strContent, strStepID, strID, strRelatedID, strUserCode,
                     TB_Content, TB_Message, LB_Status, LB_NextStepID, LB_NextSortNumber, LB_NextStepName, BT_Agree, BT_Cancel, BT_Checking, BT_Refuse, BT_Signing, BT_Review, BT_BackPirorStep,
                     DataGrid1, DataGrid2, DataGrid4, DataGrid5, DataGrid6, Repeater1, UpdatePanel1, Panel_NextStep, Panel_BelongChildWF);

        if (intResult == 1)
        {
            //删除签名域
            WFDataHandle wfDataHandle = new WFDataHandle();
            wfDataHandle.DeleteUserSignPictureField(strWLID, strID);

            //显示流程红绿灯
            strStatus = LB_Status.Text.Trim();
            //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

            TB_Message.Text = WFShareClass.GetUserName(strUserCode).Trim() + LanguageHandle.GetWord("ZZZCXLNDGZLSQ").ToString().Trim() + " ：" + strWLID + " " + GetWorkFlow(strWLID).WLName.Trim();

            //刷新流程数据页，兼容FIREFOX和其它所有浏览器
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click1", "window.parent.frames['right'].location.href = window.parent.frames['right'].location.href ", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click2", "alert('" + LanguageHandle.GetWord("ZZCXPZCG").ToString().Trim() + "')", true);


            try
            {
                //如流程是项目计划发起的，更新关联项目计划完成程度
                if (strRelatedType == "Plan" & strRelatedID != "0")
                {
                    //更改关联的计划进度
                    if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strRelatedID)) == "NO")
                    {
                        ShareClass.UpdateProjectPlanSchedule(strRelatedType, strRelatedID);
                    }
                }


                //如果流程是由项目或项目计划发起的，那么增加项目日志到项目中
                if (strContent == "")
                {
                    strContent = "Cancel workflow: " + strWLID + strWFName;
                }
                ShareClass.UpdateProjectDaiyWorkByWorkflow(strRelatedType, strRelatedID, strWLID, strContent, strUserCode);
            }
            catch (Exception err)
            {
                //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }


            if (CB_SMS.Checked == true | CB_Mail.Checked == true)
            {
                try
                {
                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            //发送没有发送的信息
                            Msg msg = new Msg();
                            msg.SendMSM("Message", LB_CreatorCode.Text.Trim(), LB_UserCode.Text.Trim() + LB_UserName.Text.Trim() + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + LanguageHandle.GetWord("CheXiaoPiZhun").ToString().Trim() + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName, strUserCode);
                        }
                        catch
                        {

                        }
                    }).Start();
                }
                catch
                {

                }
            }

            try
            {
                //发送消息给额外的人员
                new System.Threading.Thread(delegate ()
                {
                    try
                    {

                        Msg msg = new Msg();
                        msg.SendMsgToOtherMemberForWorkflow(strUserCode, strTemName, LB_TemStepID.Text.Trim(), strWLID, strWFName, strStepID, strStepName, "CancelNotice", LanguageHandle.GetWord("ZZCXPZ").ToString().Trim());
                    }
                    catch (Exception err)
                    {
                    }
                }).Start();
            }
            catch (Exception err)
            {
            }


            //撤消批准后，如果有用户附加JS代码，则执行
            try
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "myscript", "AfterCancelAgree();", true);
            }
            catch
            {
            }
        }

        if (intResult == 11)
        {
            UpdateRelatedBusinessStatus(strWLType, strRelatedID, "Cancel");

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click2", "alert('" + LanguageHandle.GetWord("ZZCXPZCG").ToString().Trim() + "')", true);
        }

        if (intResult == 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        if (intResult == -1)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBNCXPZYYRPZLXYBJC").ToString().Trim() + "')", true);
        }

        try
        {
            //更新处理流程花费的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, NB_ManHour.Amount);
            //更新处理流程花费的费用
            ShareClass.UpdateWorkFlowExpense(strRelatedType, strRelatedID, strWLID, strID);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void BT_Checking_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strStepID = LB_StepID.Text.Trim();
        string strWLID = LB_WLID.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strWFName = TB_WLName.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();

        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        WFDetailHandle wfDetailHandle = new WFDetailHandle();
        int intResult = wfDetailHandle.CheckingWF(strTemName, strWLType, strWLID, strWFName, strStatus, strContent, strStepID, strID, strRelatedID, strUserCode,
                     TB_Content, TB_Message, LB_Status, LB_NextStepID, LB_NextSortNumber, LB_NextStepName, BT_Agree, BT_Cancel, BT_Checking, BT_Refuse, BT_Signing, BT_Review, BT_BackPirorStep,
                     DataGrid1, DataGrid2, DataGrid4, DataGrid5, DataGrid6, Repeater1, UpdatePanel1, Panel_NextStep, Panel_BelongChildWF);

        if (intResult == 1)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }

        if (intResult == 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        if (intResult == -1)
        {
            TB_Message.Text = WFShareClass.GetUserName(strUserCode).Trim() + LanguageHandle.GetWord("ZZZZCSHNDGZLSQ").ToString().Trim() + " ：" + strWLID + " " + GetWorkFlow(strWLID).WLName.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHSTCZYGBHZZXGCSHSBNSZZTWSHZL").ToString().Trim() + "')", true);
        }


        if (CB_SMS.Checked == true | CB_Mail.Checked == true)
        {
            try
            {
                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        //发送没有发送的信息
                        Msg msg = new Msg();
                        msg.SendMSM("Message", LB_CreatorCode.Text.Trim(), LB_UserCode.Text.Trim() + LB_UserName.Text.Trim() + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + LanguageHandle.GetWord("ShenHeZhong").ToString().Trim() + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName, strUserCode);
                    }
                    catch
                    {

                    }
                }).Start();
            }
            catch
            {
            }
        }

        try
        {
            //发送消息给额外的人员
            new System.Threading.Thread(delegate ()
            {
                try
                {

                    Msg msg = new Msg();
                    msg.SendMsgToOtherMemberForWorkflow(strUserCode, strTemName, LB_TemStepID.Text.Trim(), strWLID, strWFName, strStepID, strStepName, "CheckingNotice", LanguageHandle.GetWord("ShenHe").ToString().Trim());
                }
                catch (Exception err)
                {
                }
            }).Start();
        }
        catch (Exception err)
        {
        }


        try
        {
            //更新处理流程花费的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, NB_ManHour.Amount);
            //更新处理流程花费的费用
            ShareClass.UpdateWorkFlowExpense(strRelatedType, strRelatedID, strWLID, strID);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

    }

    protected void BT_Signing_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strStepID = LB_StepID.Text.Trim();
        string strWLID = LB_WLID.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strWFName = TB_WLName.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();

        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        WFDetailHandle wfDetailHandle = new WFDetailHandle();
        int intResult = wfDetailHandle.SigningWF(strTemName, strWLType, strWLID, strWFName, strStatus, strContent, strStepID, strID, strRelatedID, strUserCode,
                     TB_Content, TB_Message, LB_Status, LB_NextStepID, LB_NextSortNumber, LB_NextStepName, BT_Agree, BT_Cancel, BT_Checking, BT_Refuse, BT_Signing, BT_Review, BT_BackPirorStep,
                     DataGrid1, DataGrid2, DataGrid4, DataGrid5, DataGrid6, Repeater1, UpdatePanel1, Panel_NextStep, Panel_BelongChildWF);

        if (intResult == 1)
        {
            TB_Message.Text = WFShareClass.GetUserName(strUserCode).Trim() + LanguageHandle.GetWord("ZZZZZHQNDGZLSQ").ToString().Trim() + " ：" + strWLID + " " + GetWorkFlow(strWLID).WLName.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }

        if (intResult == 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        if (intResult == -1)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHSTCZYGBHZZXGCSHSBNSZZTWHZL").ToString().Trim() + "')", true);
        }


        if (CB_SMS.Checked == true | CB_Mail.Checked == true)
        {
            try
            {
                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        //发送没有发送的信息
                        Msg msg = new Msg();
                        msg.SendMSM("Message", LB_CreatorCode.Text.Trim(), LB_UserCode.Text.Trim() + LB_UserName.Text.Trim() + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + LanguageHandle.GetWord("HuiQianZhong").ToString().Trim() + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName, strUserCode);
                    }
                    catch
                    {

                    }
                }).Start();
            }
            catch
            {
            }
        }

        try
        {
            //发送消息给额外的人员
            new System.Threading.Thread(delegate ()
            {
                try
                {

                    Msg msg = new Msg();
                    msg.SendMsgToOtherMemberForWorkflow(strUserCode, strTemName, LB_TemStepID.Text.Trim(), strWLID, strWFName, strStepID, strStepName, "SigningNotice", LanguageHandle.GetWord("HuiQianZhong").ToString().Trim());
                }
                catch (Exception err)
                {
                }
            }).Start();
        }
        catch (Exception err)
        {
        }


        try
        {
            //更新处理流程花费的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, NB_ManHour.Amount);
            //更新处理流程花费的费用
            ShareClass.UpdateWorkFlowExpense(strRelatedType, strRelatedID, strWLID, strID);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void BT_Review_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strStepID = LB_StepID.Text.Trim();
        string strWLID = LB_WLID.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strWFName = TB_WLName.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();

        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        WFDetailHandle wfDetailHandle = new WFDetailHandle();
        int intResult = wfDetailHandle.ReviewWF(strTemName, strWLType, strWLID, strWFName, strStatus, strContent, strStepID, strID, strRelatedID, strUserCode,
                     TB_Content, TB_Message, LB_Status, LB_NextStepID, LB_NextSortNumber, LB_NextStepName, BT_Agree, BT_Cancel, BT_Checking, BT_Refuse, BT_Signing, BT_Review, BT_BackPirorStep,
                     DataGrid1, DataGrid2, DataGrid4, DataGrid5, DataGrid6, Repeater1, UpdatePanel1, Panel_NextStep, Panel_BelongChildWF);

        if (intResult == 1)
        {
            TB_Message.Text = WFShareClass.GetUserName(strUserCode).Trim() + LanguageHandle.GetWord("ZZZZZFHNDGZLSQ").ToString().Trim() + " ：" + strWLID + " " + GetWorkFlow(strWLID).WLName.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }

        if (intResult == 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        if (intResult == -1)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHSTCZYGBHZZXGCSHSBNSZZTWFHZL").ToString().Trim() + "')", true);
        }

        if (CB_SMS.Checked == true | CB_Mail.Checked == true)
        {
            try
            {
                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        //发送没有发送的信息
                        Msg msg = new Msg();
                        msg.SendMSM("Message", LB_CreatorCode.Text.Trim(), LB_UserCode.Text.Trim() + LB_UserName.Text.Trim() + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + LanguageHandle.GetWord("FuHeZhong").ToString().Trim() + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName, strUserCode);
                    }
                    catch
                    {

                    }
                }).Start();
            }
            catch
            {
            }
        }

        try
        {
            //发送消息给额外的人员
            new System.Threading.Thread(delegate ()
            {
                try
                {

                    Msg msg = new Msg();
                    msg.SendMsgToOtherMemberForWorkflow(strUserCode, strTemName, LB_TemStepID.Text.Trim(), strWLID, strWFName, strStepID, strStepName, "ReviewNotice", LanguageHandle.GetWord("FuHeTong").ToString().Trim());
                }
                catch (Exception err)
                {
                }
            }).Start();
        }
        catch (Exception err)
        {
        }


        try
        {
            //更新处理流程花费的工时
            ShareClass.UpdateWorkFlowManHour(strRelatedType, strRelatedID, strWLID, strID, NB_ManHour.Amount);
            //更新处理流程花费的费用
            ShareClass.UpdateWorkFlowExpense(strRelatedType, strRelatedID, strWLID, strID);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected void RB_WorkflowOperation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RB_WorkflowOperation.SelectedValue == "Agree")   
        {
            TB_Content.Text = TB_Content.Text.Replace(LanguageHandle.GetWord("BuTongYi").ToString().Trim(), "");
        }
        else
        {
            TB_Content.Text = TB_Content.Text.Replace(LanguageHandle.GetWord("TongYi").ToString().Trim(), "");
        }

        TB_Content.Text += RB_WorkflowOperation.SelectedValue + "。";
    }


    protected void BT_AddApprover_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strStepID;

        strStepID = LB_StepID.Text;
        LB_AddApproverType.Text = "CurrentStep";

        strUserCode = Session["UserCode"].ToString();
        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);

        LoadAdditionApprover(strStepID, strUserCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop323", "popShow('popwindowAddApprover','false') ", true);
    }

    //生成部门树（根据权限）
    public static string InitialDepartmentTreeByAuthority(string strTreeName, TreeView TreeView, String strUserCode)
    {
        string strHQL, strDepartCode, strDepartName;
        string strDepartString = "", strParentDepartString, strUnderDepartString;

        strParentDepartString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);
        strUnderDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        //添加根节点
        TreeView.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();

        //node1.Text = "<B>" + LanguageHandle.GetWord("ZZJGT").ToString().Trim() + "</B>";

        node1.Text = "<B>" + strTreeName + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView.Nodes.Add(node1);

        strHQL = "Select DepartCode,DepartName From T_Department Where 1=1 ";
        strHQL += " and ParentCode not in (Select DepartCode From T_Department)";
        strHQL += " and ((Authority = 'All')";
        strHQL += " or ((Authority = 'Part') ";
        strHQL += " and (DepartCode in (select DepartCode from T_DepartmentUser where UserCode =" + "'" + strUserCode + "'" + "))))";
        strHQL += " and (DepartCode in " + strParentDepartString + " or DepartCode in " + strUnderDepartString + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strDepartCode = ds.Tables[0].Rows[i][0].ToString().Trim();
            strDepartName = ds.Tables[0].Rows[i][1].ToString().Trim();

            if (strDepartString.IndexOf("'" + strDepartCode + "'" + ",") < 0)
            {
                strDepartString += "'" + strDepartCode + "'" + ",";

                node2 = new TreeNode();

                node2.Text = strDepartCode + " " + strDepartName;
                node2.Target = strDepartCode;
                node2.Expanded = true;

                node1.ChildNodes.Add(node2);
                strDepartString += DepartmentTreeShowByAuthority(strDepartCode, node2, strUserCode, strParentDepartString, strUnderDepartString);
                TreeView.DataBind();
            }
        }

        if (strDepartString != "")
        {
            strDepartString = strDepartString.Substring(0, strDepartString.Length - 1);
        }
        else
        {
            strDepartString = "''";
        }

        strDepartString = "(" + strDepartString + ")";

        return strDepartString;
    }

    public static string DepartmentTreeShowByAuthority(string strParentCode, TreeNode treeNode, string strUserCode, string strParentDepartString, string strUnderDepartString)
    {
        string strHQL;

        DataSet ds1, ds2;

        string strDepartString = "", strDepartCode, strDepartName;

        strHQL = "Select DepartCode,DepartName From T_Department Where ParentCode = " + "'" + strParentCode + "'";
        strHQL += " and ((Authority = 'All')";
        strHQL += " or ((Authority = 'Part') ";
        strHQL += " and (DepartCode in (select DepartCode from T_DepartmentUser where UserCode =" + "'" + strUserCode + "'" + "))))";
        strHQL += " and (DepartCode in " + strParentDepartString + " or DepartCode in " + strUnderDepartString + ")";

        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strDepartCode = ds1.Tables[0].Rows[i][0].ToString().Trim();
            strDepartName = ds1.Tables[0].Rows[i][1].ToString().Trim();

            if (strDepartString.IndexOf("'" + strDepartCode + "'" + ",") < 0)
            {
                strDepartString += "'" + strDepartCode + "'" + ",";

                TreeNode node = new TreeNode();
                node.Target = strDepartCode;
                node.Text = strDepartCode + " " + strDepartName;
                treeNode.ChildNodes.Add(node);
                node.Expanded = false;

                strHQL = "Select DepartCode,DepartName From T_Department Where ParentCode = " + "'" + strDepartCode + "'";
                ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strDepartString += DepartmentTreeShowByAuthority(strDepartCode, node, strUserCode, strParentDepartString, strUnderDepartString);
                }
            }
        }

        return strDepartString;
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid8);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop45", "popShow('popwindowAddApprover','false') ", true);

        TreeView2.SelectedNode.Selected = false;
    }

    protected void BT_FindApprover_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strApproverName = TB_FindApproverName.Text.Trim();

        strApproverName = "%" + strApproverName + "%";

        strHQL = "Select UserCode,UserName From T_ProjectMember Where UserName Like '" + strApproverName + "'";
        strHQL += " and DepartCode in " + LB_DepartString.Text.Trim();

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");

        DataGrid8.DataSource = ds;
        DataGrid8.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strDetailID;
        string strStepID;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        string strIsMust;

        if (((CheckBox)e.Item.FindControl("CB_IsMust")).Checked)
        {
            strIsMust = "YES";
        }
        else
        {
            strIsMust = "NO";
        }


        string strApproverCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strApproverName = ShareClass.GetUserName(strApproverCode);

        string strApproverType = LB_AddApproverType.Text.Trim();

        if (strApproverType == "CurrentStep")
        {
            strStepID = LB_StepID.Text;
            strDetailID = LB_ID.Text;
        }
        else
        {
            strStepID = LB_NextStepID.Text;
            strDetailID = LB_NextStepDetailID.Text;
        }

        strHQL = "Select *  From T_WorkFlowStepDetail Where OperatorCode = '" + strApproverCode + "' and StepID = " + strStepID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZCYYCZBNCFTJQJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);

            return;
        }

        string strApproverComment;
        strApproverComment = TB_ApproverComment.Text.Trim();

        if (strApproverComment == "")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGZNRBNWKQJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);
            return;
        }

        strHQL = string.Format(@"Insert Into T_WorkFlowStepDetail(
           StepID
          ,WLID
          ,OperatorCode
          ,OperatorName
          ,Operation
          ,OperatorCommand
          ,CheckingTime
          ,Status
          ,WorkDetail
          ,Actor
          ,FinishedTime
          ,Requisite
          ,FieldList
          ,EditFieldList
          ,IsOperator
          ,SignPictureField
          ,AllowFullEdit
          ,CanNotNullFieldList
          ,IsMust
          ,MainTableCanEdit
          ,MainTableCanDelete
          ,DetailTableCanEdit
          ,DetailTableCanDelete
          ,ManHour
          ,Expense
          ,CreatorCode
          ,CreatorName)

        Select StepID
          ,WLID
          ,'{0}'
          ,'{1}'
          ,Operation
          ,OperatorCommand
          ,CheckingTime
          ,'InProgress'
          ,'{2}'
          ,Actor
          ,FinishedTime
          ,'{3}'
          ,''
          ,''
          ,'YES'
          ,''
          ,'NO'
          ,''
          ,'{3}'
          ,MainTableCanEdit
          ,MainTableCanDelete
          ,DetailTableCanEdit
          ,DetailTableCanDelete
          ,ManHour
          ,Expense,'{4}','{5}' 
           From T_WorkFlowStepDetail Where ID = {6}", strApproverCode, strApproverName, strApproverComment, strIsMust, strUserCode, strUserName, strDetailID);

        ShareClass.RunSqlCommand(strHQL);

        LoadAdditionApprover(strStepID, strUserCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);
    }

    protected void DataGrid9_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strStepID = LB_StepID.Text;
            string strUserCode = Session["UserCode"].ToString();
            string strOperatorCode = ((Button)e.Item.FindControl("BT_OperatorCode")).Text;

            string strApproverType = LB_AddApproverType.Text.Trim();

            if (strApproverType == "CurrentStep")
            {
                strStepID = LB_StepID.Text;
            }
            else
            {
                strStepID = LB_NextStepID.Text;
            }


            strHQL = "Delete From T_WorkFlowStepDetail Where CreatorCode = '" + strUserCode + "' and OperatorCode = '" + strOperatorCode + "' and StepID = " + strStepID;

            ShareClass.RunSqlCommand(strHQL);

            LoadAdditionApprover(strStepID, strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);
        }
    }

    protected void RP_AdditionApprover_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strStepID = LB_StepID.Text;
            string strUserCode = Session["UserCode"].ToString();
            string strOperatorCode = ((Button)e.Item.FindControl("BT_OperatorCode")).Text;

            string strApproverType = LB_AddApproverType.Text.Trim();

            if (strApproverType == "CurrentStep")
            {
                strStepID = LB_StepID.Text;
            }
            else
            {
                strStepID = LB_NextStepID.Text;
            }


            strHQL = "Delete From T_WorkFlowStepDetail Where CreatorCode = '" + strUserCode + "' and OperatorCode = '" + strOperatorCode + "' and StepID = " + strStepID;

            ShareClass.RunSqlCommand(strHQL);

            LoadAdditionApprover(strStepID, strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);
        }
    }

    protected void LoadAdditionApprover(string strStepID, string strCreatorCode)
    {
        string strHQL;

        strHQL = "Select OperatorCode,OperatorName,Requisite From T_WorkFlowStepDetail Where StepID = " + strStepID + " and CreatorCode = '" + strCreatorCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        DataGrid9.DataSource = ds;
        DataGrid9.DataBind();

        //RP_AdditionApprover.DataSource = ds;
        //RP_AdditionApprover.DataBind();
    }


    protected void BT_GetRelatedData_Click(object sender, EventArgs e)
    {
        //如果有用户附加JS代码，则执行以查找审批的相关数据
        try
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "myscript2432434", "getRelatedData();", true);
        }
        catch
        {
        }
    }

    protected void DL_RelatedWFTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWFID, strStepID, strChildTemName;

        strWFID = LB_WLID.Text.Trim();
        strStepID = LB_StepID.Text.Trim();
        strChildTemName = DL_RelatedWFTemplate.SelectedValue.Trim();

        LoadChildWFTemplate(strWFID, strStepID, strChildTemName);
    }

    protected void BT_FindOperator_Click(object sender, EventArgs e)
    {
        string strTemName, strWLID, strStepID;

        strTemName = LB_TemName.Text.Trim();
        strWLID = LB_WLID.Text.Trim();
        strStepID = LB_StepID.Text.Trim();

        //列出由要此审批者选择的下一步审批者
        LoadNextIsPriorSelectWorkFlowStep(strTemName, strWLID, strStepID);
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "From WorkFlowStepDetail as workFlowStepDetail Where workFlowStepDetail.ID = " + strID;
            WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
            lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
            WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst[0];

            LB_OperatorID.Text = strID;
            LB_OperatorCode.Text = workFlowStepDetail.OperatorCode.Trim();
            LB_OperatorName.Text = workFlowStepDetail.OperatorName.Trim();

            BT_SendMsg.Enabled = true;
            TB_SendMsg.Text = LanguageHandle.GetWord("GZLSPTZNHNYLCSC").ToString().Trim() + ": " + LB_WLID.Text.Trim() + " " + TB_WLName.Text.Trim() + LanguageHandle.GetWord("YSPQJSCL").ToString().Trim();

            if (e.CommandName == "AddApprover")
            {
                string strUserCode;
                string strStepID;

                strStepID = LB_NextStepID.Text;
                LB_NextStepDetailID.Text = strID;

                string strStepStatus = ShareClass.GetWorkFlowStepStatus(strStepID);
                if (strStepStatus == "InProgress")
                {
                    strUserCode = Session["UserCode"].ToString();
                    LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);

                    LoadAdditionApprover(strStepID, strUserCode);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop456", "popShow('popwindowAddApprover','false') ", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click333", "alert('" + LanguageHandle.GetWord("ZZJGZGZLBZYJTGHZZBNJJLQJC").ToString().Trim() + "')", true);
                }
            }
        }
    }


    protected void BT_SaveOperatorSelect_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        try
        {
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                strID = ((Button)DataGrid5.Items[i].FindControl("BT_ID")).Text.Trim();

                if (((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Checked == true)
                {
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'YES' Where ID = " + strID;
                }
                else
                {
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'NO'  Where ID = " + strID;
                }
                ShareClass.RunSqlCommand(strHQL);

                if (((CheckBox)DataGrid5.Items[i].FindControl("CB_IsMust")).Checked == true)
                {
                    strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'YES' Where ID = " + strID;
                }
                else
                {
                    strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'NO' Where ID = " + strID;
                }

                ShareClass.RunSqlCommand(strHQL);
            }


            string strMsgText = LanguageHandle.GetWord("BCCGYGBDQYMM").ToString().Trim();
            string strIsCloseCurrentWFPage = "NO";
            try
            {
                strIsCloseCurrentWFPage = System.Configuration.ConfigurationManager.AppSettings["CloseCurrentWFTab"];
            }
            catch
            {
            }
            if (strIsCloseCurrentWFPage != "YES")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click111", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click2222", "CloseTab('" + strMsgText + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click333", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid5_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox cbOperator = (CheckBox)e.Item.FindControl("CB_IsOperator");
            cbOperator.CheckedChanged += new System.EventHandler(this.cbSelectOperatorChanged);

            CheckBox cbMust = (CheckBox)e.Item.FindControl("CB_IsMust");
            cbMust.CheckedChanged += new System.EventHandler(this.cbSelectMustChanged);
        }
    }

    //创建事件
    protected void cbSelectOperatorChanged(object sender, System.EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        DataGridItem li = (DataGridItem)cb.Parent.Parent;

        if (strAutoSaveWFOperator == "YES")
        {
            string strHQL;
            string strID = ((Button)(li.FindControl("BT_ID"))).Text.Trim();

            if (cb.Checked == true)
            {
                strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'YES' Where ID = " + strID;
            }
            else
            {
                strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'NO'  Where ID = " + strID;
            }

            ShareClass.RunSqlCommand(strHQL);
        }
    }

    //创建事件
    protected void cbSelectMustChanged(object sender, System.EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        DataGridItem li = (DataGridItem)cb.Parent.Parent;

        if (cb.Checked == true)
        {
            ((CheckBox)(li.FindControl("CB_IsOperator"))).Checked = true;
        }

        if (strAutoSaveWFOperator == "YES")
        {
            string strHQL;
            string strID = ((Button)(li.FindControl("BT_ID"))).Text.Trim();

            if (cb.Checked == true)
            {
                ((CheckBox)(li.FindControl("CB_IsOperator"))).Checked = true;
                strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'YES',IsOperator = 'YES' Where ID = " + strID;
            }
            else
            {
                ((CheckBox)(li.FindControl("CB_IsOperator"))).Checked = false;
                strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'NO',IsOperator = 'NO'  Where ID = " + strID;
            }

            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void BT_SendMsg_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;
        string strOperatorCode, strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        strOperatorCode = LB_OperatorCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_SendMsg.Checked == true | CB_SendMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("GongZuoLiuShenPiTongZhi").ToString().Trim();
            strMsg = TB_SendMsg.Text.Trim();

            if (CB_SendMsg.Checked == true)
            {
                msg.SendMSM("Message", strOperatorCode, strMsg, strUserCode);
            }

            if (CB_SendMail.Checked == true)
            {
                msg.SendMail(strOperatorCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB").ToString().Trim() + "')", true);
    }


    protected void CB_AllOperator_CheckedChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        if (CB_AllOperator.Checked == true)
        {
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Checked = true;
                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid5.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'YES' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }
        else
        {
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Checked = false;

                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid5.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'NO' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }
    }

    protected void CB_AllMust_CheckedChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        if (CB_AllMust.Checked == true)
        {
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsMust")).Checked = true;
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Checked = true;

                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid5.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'YES',IsOperator = 'YES' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }
        else
        {
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsMust")).Checked = false;
                //((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Checked = false;

                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid5.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'NO',IsOperator = 'NO' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }
    }


    protected void LoadNextWorkFlowStepDetail(string strStepID, string strIsOperatorSelect, string strIsPriorStepSelect, string strStepStatus)
    {
        string strHQL;
        IList lst;
        string strIsOperator, strIsRequisite;
        string strID;
        int i;

        string strOperatorName;
        strOperatorName = "%" + TB_OperatorName.Text.Trim() + "%";

        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.StepID = " + strStepID;
        strHQL += " and workFlowStepDetail.OperatorName Like " + "'" + strOperatorName + "'";
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        WorkFlowStepDetail workFlowStepDetail = new WorkFlowStepDetail();

        for (i = 0; i < lst.Count; i++)
        {
            workFlowStepDetail = (WorkFlowStepDetail)lst[i];
            strID = workFlowStepDetail.ID.ToString();

            //是否操作者
            strIsOperator = GetWorkFlowStepDetailOperatorStatus(strID);
            if (strIsOperator == "YES")
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Checked = false;
            }

            if (strIsOperatorSelect == "NO" | strIsPriorStepSelect == "NO" | strStepStatus == "Passed")
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsOperator")).Enabled = false;
            }

            //是否必须
            strIsRequisite = GetWorkFlowStepDetailMustStatus(strID);

            Label50.Text = strIsRequisite;
            if (strIsRequisite == "YES")
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsMust")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsMust")).Checked = false;
            }

            if (strIsOperatorSelect == "NO" | strIsPriorStepSelect == "NO" | strStepStatus == "Passed")
            {
                ((CheckBox)DataGrid5.Items[i].FindControl("CB_IsMust")).Enabled = false;
            }

            if (ShareClass.GetWorkflowTemplateStepAllowNextStepAddApprover(LB_TemStepID.Text) == "YES")
            {
                ((Button)DataGrid5.Items[i].FindControl("BT_AddApprover")).Enabled = true;
            }
            else
            {
                ((Button)DataGrid5.Items[i].FindControl("BT_AddApprover")).Enabled = false;
            }
        }
    }

    protected string GetProjectIDByPlanID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select ProjectID From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }
    protected string GetProjectIDByProExpensePlanID(string strWorkflowID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProExpense as proExpense where proExpense.WorkflowID = " + strWorkflowID;
        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        lst = proExpenseBLL.GetAllProExpenses(strHQL);

        if (lst.Count > 0)
        {
            ProExpense proExpense = (ProExpense)lst[0];
            return proExpense.ProjectID.ToString();
        }
        else
        {
            return "1";
        }
    }

    //取得此工作流相关子工作流列表
    protected void LoadChildWorkflow(string strWLID)
    {
        string strHQL;

        strHQL = "Select * From T_Workflow Where WLID in ( Select WFChildID From T_WFStepRelatedWF Where WFID = " + strWLID + ")";
        strHQL += " Order By WLID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        DataGrid7.DataSource = ds;
        DataGrid7.DataBind();
    }

    protected string GetWorkFlowStepDetailOperatorStatus(string strID)
    {
        string strHQL;

        strHQL = "Select IsOperator from T_WorkFlowStepDetail where ID = " + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    protected string GetWorkFlowStepDetailMustStatus(string strID)
    {
        string strHQL;

        strHQL = "Select IsMust from T_WorkFlowStepDetail where ID = " + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strStepID = ((Button)e.Item.FindControl("BT_StepID")).Text.ToString();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID;
            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);
            WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

            LB_WFTemStepID.Text = strStepID;
            LB_StepName.Text = workFlowTStep.StepName.Trim();
            LB_SortNumber.Text = workFlowTStep.SortNumber.ToString();

            strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.StepID = " + strStepID;
            WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
            lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SqlWL.Text;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;

        string strWLID = LB_WLID.Text.Trim();
        string strReceiverCode = GetWorkFlow(strWLID).CreatorCode;
        string strUserCode = LB_UserCode.Text.Trim();

        if (CB_SMS.Checked == true | CB_Mail.Checked == true)
        {
            Msg msg = new Msg();

            strSubject = LanguageHandle.GetWord("GongZuoLiuChuLiTongZhi").ToString().Trim();
            strMsg = TB_Message.Text.Trim();

            if (CB_SMS.Checked == true)
            {
                msg.SendMSM("Message", strReceiverCode, strMsg, strUserCode);
            }

            if (CB_Mail.Checked == true)
            {
                msg.SendMail(strReceiverCode, strSubject, strMsg, strUserCode);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSCG").ToString().Trim() + "')", true);
    }

    //更改流程相关对象状态
    public bool UpdateRelatedBusinessStatus(string strWFType, string strRelatedID, string strOperation)
    {
        string strHQL;

        try
        {
            if (strOperation == "Agree")
            {
                if (strWFType == "CustomerServiceReview")
                {
                    strHQL = "Update T_CustomerQuestion Set Status = 'Completed' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "VehicleRequest")
                {
                    strHQL = "Update T_CarApplyForm Set Status = 'Passed' Where ID  = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "MeetingRequest")
                {
                    strHQL = "Update T_Meeting Set Status = 'Passed' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "ExpenseRequest")
                {
                    strHQL = "Update T_ExpenseApplyWL Set Status = 'Passed' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "ExpenseReimbursement")
                {
                    strHQL = "Update T_ExpenseClaim Set Status = 'Passed' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }
            }

            if (strOperation == "Cancel")
            {
                if (strWFType == "CustomerServiceReview")
                {
                    strHQL = "Update T_CustomerQuestion Set Status = 'InProgress' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "VehicleRequest")
                {
                    strHQL = "Update T_CarApplyForm Set Status = 'InProgress' Where ID  = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "MeetingRequest")
                {
                    strHQL = "Update T_Meeting Set Status = 'InProgress' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "ExpenseRequest")
                {
                    strHQL = "Update T_ExpenseApplyWL Set Status = 'InProgress' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "ExpenseReimbursement")
                {
                    strHQL = "Update T_ExpenseClaim Set Status = 'InProgress' Where ID = " + strRelatedID;
                    WFShareClass.RunSqlCommand(strHQL);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    protected bool CheckOperatorIsRequisiteForWorkflowStep(string strWLID, string strStepID, string strOperatorCode)
    {
        string strHQL1, strHQL2;
        DataSet ds1, ds2;
        int i = 1;

        strHQL1 = "Select * From T_WorkflowStepDetail Where ";
        strHQL1 += " StepID = " + strStepID + " and WLID = " + strWLID;
        ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_WorkflowStepDetail");
        if (ds1.Tables[0].Rows.Count == 1)
        {
            i = -1;
        }

        strHQL2 = "Select * From T_WorkflowStepDetail Where ";
        strHQL2 += " StepID = " + strStepID + " and WLID = " + strWLID;
        strHQL2 += " and OperatorCode = '" + strOperatorCode + "' and Requisite = 'YES'";
        ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_WorkflowStepDetail");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            i = -1;
        }

        if (i == -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void LoadChildWFTemplate(string strWFID, string strStepID, string strChildTemName)
    {
        string strHQL;
        IList lst;

        string strIdentifyString;

        strHQL = "From WFTStepRelatedTem as wfTStepRelatedTem Where wfTStepRelatedTem.RelatedWFTemName = " + "'" + strChildTemName + "'";
        WFTStepRelatedTemBLL wfTStepRelatedTemBLL = new WFTStepRelatedTemBLL();
        lst = wfTStepRelatedTemBLL.GetAllWFTStepRelatedTems(strHQL);
        WFTStepRelatedTem wfTStepRelatedTem = (WFTStepRelatedTem)lst[0];

        LB_ChildWFRequisite.Text = wfTStepRelatedTem.Requisite.Trim();
        LB_BelongStepSortNumber.Text = wfTStepRelatedTem.BelongStepSortNumber.ToString();
        LB_BelongIsPassed.Text = wfTStepRelatedTem.BelongIsMustPassed.Trim();

        strIdentifyString = ShareClass.GetWLTemplateIdentifyString(strChildTemName);

        strHQL = "From WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName = " + "'" + strChildTemName + "'";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        string strWFTemplate = workFlowTemplate.TemName.Trim();
        string strXSNFile, strWFType, strPageName;
        try
        {
            strXSNFile = workFlowTemplate.XSNFile.Trim();
            HL_ChildWF.NavigateUrl = "TTDIYChildWorkFlowForm.aspx?IdentifyString=" + strIdentifyString + "&WFID=" + strWFID + "&StepID=" + strStepID;
        }
        catch
        {
            strWFType = workFlowTemplate.Type.Trim();

            strHQL = "Select B.ID,B.WFName,B.PageName,B.WFType,A.TemName ";
            strHQL += " From T_WorkFlowTemplate A, T_WorkFlowPage B";
            strHQL += " Where A.TemName  Like '%' || B.WFName  || '%' and  B.WFType = 'WorkFlow'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowPage");

            if (ds.Tables[0].Rows.Count > 0)
            {
                strPageName = ds.Tables[0].Rows[0][2].ToString();
                HL_ChildWF.NavigateUrl = strPageName + "?TargetID=" + ds.Tables[0].Rows[0][0].ToString() + "&WFID=" + strWFID;
            }
        }
    }

    protected void BT_CloseCurrentPate_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "CloseWebPage();", true);
    }

    protected void LoadWorkFlowStep(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.StepID = " + strStepID;
        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadNextIsPriorSelectWorkFlowStep(string strTemName, string strWLID, string strStepID)
    {
        int intNextSortNumber, intSortNumber;
        string strNextStepID;
        string strIsOperatorSelect, strIsPriorStepSelect;
        int r = 0;

        WFDetailHandle wfDetailHandle = new WFDetailHandle();

        WorkFlowStep workFlowStep = wfDetailHandle.GetWorkFlowStep(strStepID);
        intSortNumber = workFlowStep.SortNumber;

        WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, intSortNumber);

        //有错误
        try
        {
            string[] strNextSortNumberList = wfDetailHandle.GetNextSortNumber(strWLID, workFlowTStep.StepID, UpdatePanel1).Split(",".ToCharArray());

            int q = 0;
            string strNextStepIDList = "";

            strTemName = GetWorkFlow(strWLID).TemName.Trim();

            for (q = 0; q < strNextSortNumberList.Length; q++)
            {
                intNextSortNumber = int.Parse(strNextSortNumberList[q]);

                if (intNextSortNumber > 0)
                {
                    workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, intNextSortNumber);

                    strIsOperatorSelect = workFlowTStep.OperatorSelect.Trim();
                    strIsPriorStepSelect = workFlowTStep.IsPriorStepSelect.Trim();

                    //判断是否存在活动的下一步
                    if (wfDetailHandle.CheckIsExistNextWorkflowStep(strWLID, intNextSortNumber) == true)
                    {
                        if (strIsPriorStepSelect == "YES")
                        {
                            Panel_NextStep.Visible = true;

                            strNextStepID = wfDetailHandle.getNextWorkflowStepID(strWLID, intNextSortNumber).ToString();

                            strNextStepIDList = strNextStepIDList + strNextStepID + ",";

                            r = 2;
                        }
                    }
                }
            }

            if (r == 2)
            {
                //列出由前步选择的成员
                strNextStepIDList = "(" + strNextStepIDList.TrimEnd(',') + ")";
                wfDetailHandle.LoadNextWorkFlowStepDetailByStepIDList(strWLID, strNextStepIDList, "YES", "InProgress", DataGrid5);
            }
        }
        catch
        {
            string strErrorMsg = LanguageHandle.GetWord("ZZJGCGZLGLMB").ToString().Trim() + ": " + strTemName + " " + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + intSortNumber.ToString() + " " + workFlowTStep.StepName.Trim() + " " + LanguageHandle.GetWord("ZZTJSZYCWQJC").ToString().Trim();
            Response.Redirect("TTDisplayCustomErrorMessage.aspx?ErrorMsg=" + strErrorMsg);
        }
    }


    protected void LoadWorkFlowStepDetailByID(string strID)
    {
        string strHQL;
        IList lst;

        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.ID = " + strID;
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadRelatedWFTemplateStepList(string strTemName)
    {
        string strHQL;
        IList lst;


        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName = " + "'" + strTemName + "'" + " Order by workFlowTStep.SortNumber ASC";
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_WorkFlow.Text = strTemName;
    }

    protected void LoadWFStepChildWF(string strParentWLID, string strParentStepID, string strTemName, int intSortNumber)
    {
        string strHQL;

        string strWFTStepID;

        DataSet ds = new DataSet();

        WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, intSortNumber);
        strWFTStepID = workFlowTStep.StepID.ToString();

        //判断是否存在必须通过的归属流程
        strHQL = "Select B.WLID,B.WLName,B.WLType,B.CreateTime,B.CreatorCode,B.CreatorName,C.BelongStepSortNumber,C.BelongIsMustPassed,B.Status From T_WFStepRelatedWF A,T_WorkFlow B,T_WFTStepRelatedTem C";
        strHQL += " Where A.WFChildID = B.WLID and B.TemName = C.RelatedWFTemName";
        strHQL += " and A.WFID = " + strParentWLID;
        strHQL += " and C.BelongStepSortNumber = " + intSortNumber.ToString();
        strHQL += " and C.BelongIsMustPassed = 'YES'";
        strHQL += " and C.Requisite = 'YES'";
        strHQL += " and B.Status not in ('Passed','CaseClosed')";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WFStepRelatedWF");

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataGrid6.DataSource = ds;
            DataGrid6.DataBind();

            Panel_BelongChildWF.Visible = true;
        }
    }

    protected void LoadWorkFlowTStep(string strTemName, string strCurrentStepSortNumber)
    {
        string strHQL;

        if (strCurrentStepSortNumber == "0")
        {
            strHQL = "select SortNumber,cast( SortNumber as char(4))  ||  StepName as StepValue from T_WorkFlowTStep where TemName = " + "'" + strTemName + "'" + " and SortNumber <> 0 Order by SortNumber ASC";
        }
        else
        {
            strHQL = "select SortNumber,cast( SortNumber as char(4))  ||  StepName as StepValue from T_WorkFlowTStep where TemName = " + "'" + strTemName + "'" + " and SortNumber <> 0 and SortNumber < " + strCurrentStepSortNumber + " Order by SortNumber ASC";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStep");

        DL_NextStep.DataSource = ds;
        DL_NextStep.DataBind();
    }

    protected void LoadWFTStepRelatedTem(string strStepID)
    {
        string strHQL;
        IList lst;
        string strChildTemName;
        string strWFID;

        strWFID = LB_WLID.Text.Trim();

        strHQL = "From WFTStepRelatedTem as wfTStepRelatedTem Where wfTStepRelatedTem.RelatedStepID = " + strStepID;
        WFStepRelatedWFBLL wfStepRelatedWFBLL = new WFStepRelatedWFBLL();
        lst = wfStepRelatedWFBLL.GetAllWFStepRelatedWFs(strHQL);

        DL_RelatedWFTemplate.DataSource = lst;
        DL_RelatedWFTemplate.DataBind();

        if (lst.Count > 0)
        {
            Panel_ChildWF.Visible = true;

            strChildTemName = DL_RelatedWFTemplate.SelectedValue.Trim();
            LoadChildWFTemplate(strWFID, strStepID, strChildTemName);
        }
    }

    protected void LoadWorkflowApproveRecord(string strWFID)
    {
        string strHQL;
        IList lst;

        try
        {
            strHQL = "from Approve as approve where approve.Type = 'Workflow' and approve.RelatedID = " + strWFID + " Order by approve.ID DESC";
            ApproveBLL approveBLL = new ApproveBLL();
            lst = approveBLL.GetAllApproves(strHQL);

            DataList1.DataSource = lst;
            DataList1.DataBind();
        }
        catch
        {
        }
    }

    protected WFTStepRelatedTem GetWFTStepRelatedTem(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "From WFTStepRelatedTem as wfTStepRelatedTem Where wfTStepRelatedTem.RelatedStepID = " + strStepID;
        WFStepRelatedWFBLL wfStepRelatedWFBLL = new WFStepRelatedWFBLL();
        lst = wfStepRelatedWFBLL.GetAllWFStepRelatedWFs(strHQL);

        WFTStepRelatedTem wfTStepRelatedTem = (WFTStepRelatedTem)lst[0];

        return wfTStepRelatedTem;
    }



    protected string GetWorkFlowStatus(string strWLID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow.Status.Trim();
    }

    protected string GetWorkFlowStepStatus(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.StepID = " + strStepID;
        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);

        WorkFlowStep workFlowStep = (WorkFlowStep)lst[0];

        return workFlowStep.Status.Trim();
    }

    protected string GetWorkFlowStepName(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.StepID = " + strStepID;
        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);

        WorkFlowStep workFlowStep = (WorkFlowStep)lst[0];

        return workFlowStep.StepName.Trim();
    }

    protected int GetWorkFlowStepIDBySortNumber(string strWLID, string strSortNumber)
    {
        string strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.WLID = " + strWLID + " and workFlowStep.SortNumber = " + strSortNumber;
        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        IList lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);
        WorkFlowStep workFlowStep = (WorkFlowStep)lst[0];

        int intStepID = workFlowStep.StepID;

        return intStepID;
    }

    protected WorkFlowStepDetail GetWorkFlowStepDetail(string strID)
    {
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();

        string strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.ID = " + strID;
        IList lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);

        WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst[0];

        return workFlowStepDetail;
    }

    protected int GetWorkFlowNextStepDetailAlreadyApprovedNumber(string strWLID, string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.StepID > " + strStepID + " and workFlowStepDetail.WLID = " + strWLID + " and workFlowStepDetail.Status = 'Approved'";
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);

        return lst.Count;
    }

    protected WorkFlow GetWorkFlow(string strWLID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow;
    }


}
