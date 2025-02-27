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

public partial class TTMyWorkFlowDetail : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strAutoSaveWFOperator;
    string strSubmitRelatedType, strWFTemStepGUID;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strWLID = Request.QueryString["WLID"];
        strSubmitRelatedType = Request.QueryString["RelatedType"];

        string strWLName;
        string strTemName;
        string strXMLFile;
        string strHQL;
        IList lst;
        string strStatus;
        string strUserCode, strUserName, strCreatorCode;
        string strDIYNextStep, strSMS, strEMail;
        int intStepCount;

        XmlDocument docXml = new XmlDocument();

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strAutoSaveWFOperator = Session["AutoSaveWFOperator"].ToString().Trim();

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        strWLID = workFlow.WLID.ToString().Trim();
        strWLName = workFlow.WLName.Trim();
        strTemName = workFlow.TemName.Trim();

        strXMLFile = workFlow.XMLFile.Trim();
        strDIYNextStep = workFlow.DIYNextStep.Trim();
        strEMail = workFlow.ReceiveEMail.Trim();
        strSMS = workFlow.ReceiveSMS.Trim();
        strStatus = workFlow.Status.Trim();

        strRelatedType = workFlow.RelatedType.Trim();
        strRelatedID = workFlow.RelatedID.ToString();

        strCreatorCode = workFlow.CreatorCode.Trim();

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

            Session["AutoActiveWorkflow"] = ShareClass.GetWorkflowTemplateAutoActive(strTemName);

            BT_Active.Attributes.Add("onclick ", "SaveDIYFormDate('Active');");
            BT_Close.Attributes.Add("onclick ", "SaveDIYFormDate('Close');");
            BT_Updating.Attributes.Add("onclick ", "SaveDIYFormDate('Updating');");
            BT_ReActive.Attributes.Add("onclick ", "SaveDIYFormDate('ReActive');");

            LB_UserCode.Text = strUserCode;
            strUserName = ShareClass.GetUserName(strUserCode);
            LB_UserName.Text = strUserName;

            HL_WLRelatedDoc.NavigateUrl = "TTWLRelatedDoc.aspx?WLID=" + strWLID;
            HL_ApproveRecord.NavigateUrl = "TTWorkflowApproveRecord.aspx?Type=WorkFlow&RelatedID=" + strWLID + "&StepID=0";
            HL_StepApproveRecord.NavigateUrl = "TTWorkflowApproveRecord.aspx?Type=Step&RelatedID=" + strWLID + "&StepID=0";
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=WORKFLOW&RelatedID=" + strWLID;
            if (strRelatedType == "Plan")
            {
                CB_IsPlanMainWorkflow.Visible = true;
                HL_Expense.Visible = true;
                HL_Expense.NavigateUrl = "TTProExpense.aspx?ProjectID=" + GetProjectIDByPlanID(strRelatedID) + "&TaskID=0&RecordID=0&PlanID=" + strRelatedID + "&WorkflowID=" + strWLID;
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

            HL_XMLFile.NavigateUrl = strXMLFile;

            strWLName = TB_WLName.Text.Trim();

            intStepCount = LoadWorkFlowStep(strWLID);
            LB_UNReviewNumber.Text = GetUNReviewWorkFlowStepNumber(strWLID).ToString();
         
            try
            {
                ////显示流程红绿灯
                //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

                string strDesignType = ShareClass.GetWLTemplateDesignType(strTemName);
                if (strDesignType == "SL")
                {
                    HL_WFChartView.NavigateUrl = "TTWFChartViewSL.aspx?WLID=" + LB_WLID.Text + "&IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(strTemName);
                }
                if (strDesignType == "JS")
                {
                    HL_WFChartView.NavigateUrl = "TTWFChartViewJS.aspx?WLID=" + LB_WLID.Text + "&IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(strTemName);
                }

                //显示关联项目树
                ShareClass.InitialPrjectTreeByAuthority(TreeView1, strUserCode);
                LoadRelatedProject(strWLID);

                //列出子工作流
                LoadChildWorkflow(strWLID);

                try
                {
                    //取得归属于此步的流程模板步骤
                    WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, 0);
                    strWFTemStepGUID = workFlowTStep.GUID.Trim();
                    //列出关联模块
                    ShareClass.LoadWorkFlowTStepRelatedModule(RP_RelatedModule, strWLID, "0", "0", strWFTemStepGUID, strLangCode, strUserCode);
                }
                catch
                {
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGZLMBBCZQJC").ToString().Trim() + "')", true);
            }

            //判断计划进度是否受任务和流程进度影响
            if (strRelatedType == "Plan" & strRelatedID != "0")
            {
                if (ShareClass.CheckMemberCanUpdatePlanByUserCode(GetProjectIDByPlanID(strRelatedID), strUserCode))
                {
                    if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strRelatedID)) == "NO")
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
            }
            else
            {
                BT_ConfirmEffectPlanProgress.Visible = false;
            }


            //判断工作流数据文件是否存在
            try
            {
                docXml.Load(Server.MapPath(strXMLFile));
            }
            catch
            {
                BT_Active.Enabled = false;
                BT_Updating.Enabled = false;
                BT_Delete.Enabled = false;
                BT_ReActive.Enabled = false;
                BT_Close.Enabled = false;
                BT_Success.Enabled = false;
                BT_ConfirmEffectPlanProgress.Visible = false;
                return;
            }

            if (strStatus == "CaseClosed")
            {
                BT_Active.Enabled = false;
                BT_Updating.Enabled = false;
                BT_Delete.Enabled = false;
                BT_ReActive.Enabled = false;
                BT_Close.Enabled = false;
                BT_Success.Enabled = false;
                BT_ConfirmEffectPlanProgress.Visible = false;
            }
            else
            {
                if (strStatus == "Passed")
                {
                    BT_Active.Enabled = false;
                    BT_Updating.Enabled = false;
                    BT_ReActive.Enabled = false;
                    BT_Delete.Enabled = false;
                    BT_Close.Enabled = false;
                    BT_Success.Enabled = true;
                }
                else
                {
                    BT_Active.Enabled = true;
                    BT_Updating.Enabled = true;
                    BT_ReActive.Enabled = true;
                    BT_Delete.Enabled = true;
                    BT_Close.Enabled = true;
                    BT_Success.Enabled = false;
                }
            }

            if (intStepCount > 0)
            {
                BT_Active.Enabled = false;
            }

            //判断流程启动后，有人批准后，能不能删除这个流程
            if (GetWorkFlowDetailStatusCount(strWLID, "Approved") > 0)
            {
                string strCanDeleteWorkflowByApproved;
                try
                {
                    strCanDeleteWorkflowByApproved = System.Configuration.ConfigurationManager.AppSettings["CanDeleteWorkflowAfterApproved"];
                }
                catch
                {
                    strCanDeleteWorkflowByApproved = "YES";
                }

                if (strCanDeleteWorkflowByApproved == "YES")
                {
                    BT_Delete.Enabled = true;
                }
                else
                {
                    BT_Delete.Enabled = false;
                }
            }

            if (strDIYNextStep == "YES")
            {
                CB_DIYNextStep.Checked = true;
            }
            else
            {
                CB_DIYNextStep.Checked = false;
            }

            if (strSMS == "YES")
            {
                CB_SMS.Checked = true;
            }
            else
            {
                CB_SMS.Checked = false;
            }

            if (strEMail == "YES")
            {
                CB_Mail.Checked = true;
            }
            else
            {
                CB_Mail.Checked = false;
            }

            HL_WorkFlowStep.NavigateUrl = "TTWorkFlowTemplateView.aspx?IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(strTemName);
            HL_WLRelatedDoc.NavigateUrl = "TTWLRelatedDoc.aspx?WLID=" + strWLID;
        }
    }


    protected void CB_IsPlanMainWorkflow_CheckedChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strWLID;
        string strIsPlanMainWorkflow;

        try
        {
            strWLID = LB_WLID.Text;

            if (CB_IsPlanMainWorkflow.Checked == true)
            {
                strIsPlanMainWorkflow = "YES";
            }
            else
            {
                strIsPlanMainWorkflow = "NO";
            }

            strHQL = "Update T_WorkFlow Set IsPlanMainWorkflow = '" + strIsPlanMainWorkflow + "' Where WLID = " + strWLID;
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }
    }

    protected void BT_Active_Click(object sender, EventArgs e)
    {
        int intResult;

        string strUserCode = LB_UserCode.Text.Trim();
        string strWLType = LB_WLType.Text.Trim();
        string strWLID = LB_WLID.Text.Trim();
        string strID = LB_ID.Text.Trim();
        string strStepID = "0";
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();
        string strTemName = LB_TemName.Text.Trim();

        WFActiveHandle wfActiveHandle = new WFActiveHandle();
        intResult = wfActiveHandle.ActiveWF(strWLType, strTemName, strWLID, strStepID, strID, strStatus, strContent, strUserCode, TB_Content, LB_TemName, LB_Status, LB_UNReviewNumber, CB_SMS, CB_Mail, CB_DIYNextStep, DataGrid2, UpdatePanel1);

        if (intResult >= 0)
        {
            BT_Active.Enabled = false;

            string strCmdText = "Update T_WorkFlowStep Set Status = 'Passed' Where WLID = " + strWLID + " and StepID = " + strStepID;
            ShareClass.RunSqlCommand(strCmdText);

            LoadWorkFlowStep(strWLID);
            LB_UNReviewNumber.Text = wfActiveHandle.GetUNReviewWorkFlowStepNumber(strWLID, strTemName).ToString();

            ////显示流程红绿灯
            //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

            LoadWorkFlow(strWLID);

            //自动激活时不弹出“激活成功”提示框
            if (Session["AutoActiveWorkflow"].ToString() != "YES")
            {
                if (intResult == 1)
                {
                    string strMsgText = LanguageHandle.GetWord("JHCGYGBDQYMM").ToString().Trim();
                    string strIsCloseCurrentWFPage = "NO";
                    try
                    {
                        strIsCloseCurrentWFPage = System.Configuration.ConfigurationManager.AppSettings["CloseCurrentWFTab"];
                    }
                    catch
                    {
                    }
                    if (strIsCloseCurrentWFPage == "YES")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "CloseAllTabAndAddNewTab('" + strMsgText + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click111", "alert('OK')", true);
                    }
                }

                if (intResult == 2)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHCGXYBSPZYNXDQXZ").ToString().Trim() + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click333", "ChangeMenu(0)", true);
                }
            }
            else
            {
                if (intResult == 1)
                {
                    string strMsgText = LanguageHandle.GetWord("JHCGYGBDQYMM").ToString().Trim();
                    string strIsCloseCurrentWFPage = "NO";
                    try
                    {
                        strIsCloseCurrentWFPage = System.Configuration.ConfigurationManager.AppSettings["CloseCurrentWFTab"];
                    }
                    catch
                    {
                    }

                    if (strIsCloseCurrentWFPage == "YES")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", " CloseAllTabAndAddNewTab('" + strMsgText + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click111", "alert('OK')", true);
                    }
                }

                if (intResult == 2)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click222", "alert('" + LanguageHandle.GetWord("ZZJHCGXYBSPZYNXDQXZ").ToString().Trim() + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click333", "ChangeMenu(0)", true);
                }
            }
        }

        if (intResult == -1)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click444", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Updating_Click(object sender, EventArgs e)
    {
        int intResult;

        string strWLID = LB_WLID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        WFActiveHandle wfActiveHandle = new WFActiveHandle();
        intResult = wfActiveHandle.UpdatingWF(strWLID, strStatus, strContent, strUserCode, TB_Content, LB_Status, CB_SMS, CB_Mail, CB_DIYNextStep, DataGrid2, UpdatePanel1);

        if (intResult == 1)
        {
            LoadWorkFlow(strWLID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_ReActive_Click(object sender, EventArgs e)
    {
        int intResult;

        string strWLID = LB_WLID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        WFActiveHandle wfActiveHandle = new WFActiveHandle();
        intResult = wfActiveHandle.ReActiveWF(strWLID, strStatus, strContent, strUserCode, TB_Content, LB_Status, CB_SMS, CB_Mail, CB_DIYNextStep, DataGrid2, UpdatePanel1);

        if (intResult == 1)
        {
            LoadWorkFlow(strWLID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Close_Click(object sender, EventArgs e)
    {
        int intResult;

        string strWLID = LB_WLID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strStatus = LB_Status.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        WFActiveHandle wfActiveHandle = new WFActiveHandle();
        intResult = wfActiveHandle.CloseWF(strWLID, strStatus, strContent, strUserCode, TB_Content, LB_Status, CB_SMS, CB_Mail, CB_DIYNextStep, DataGrid2, UpdatePanel1);


        if (intResult == 1)
        {
            LoadWorkFlow(strWLID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Success_Click(object sender, EventArgs e)
    {
        int intResult;

        string strWLID = LB_WLID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strContent = TB_Content.Text.Trim();
        string strStatus = LB_Status.Text.Trim();

        WFActiveHandle wfActiveHandle = new WFActiveHandle();
        intResult = wfActiveHandle.SucessWF(strWLID, strStatus, strContent, strUserCode, TB_Content, LB_Status, CB_SMS, CB_Mail, CB_DIYNextStep, DataGrid2, BT_Success, UpdatePanel1);

        if (intResult == 1)
        {
            LoadWorkFlow(strWLID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        int intResult;

        string strWLID = LB_WLID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strReceiverCode = strUserCode;
        string strContent = TB_Content.Text.Trim();
        string strStatus = LB_Status.Text.Trim();

        WFActiveHandle wfActiveHandle = new WFActiveHandle();
        intResult = wfActiveHandle.DeleteWF(strWLID, strStatus, strContent, strUserCode, TB_Content, LB_Status, CB_SMS, CB_Mail, CB_DIYNextStep, DataGrid2, BT_Active, BT_Updating, BT_Close, BT_Success, BT_Delete, UpdatePanel1);

        if (intResult == 1)
        {
            try
            {
                //如流程是项目计划发起的，更新关联项目计划完成程度
                ShareClass.UpdateProjectPlanSchedule(strRelatedType, strRelatedID);

                //如果流程是由项目或项目计划发起的，那么增加项目日志到项目中
                ShareClass.UpdateProjectDaiyWorkByWorkflow(strRelatedType, strRelatedID, strWLID, strContent, strUserCode);

                //删除此流程相关的数据
                try
                {
                    string strHQL;
                    strHQL = "Delete From T_WorkFlowRelatedModule Where WorkflowID = " + strWLID;
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }


            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        ////发送没有发送的信息
        //Msg msg = new Msg();
        //msg.SendUNSentSMSBySP();
    }

    //确认影响计划进度
    protected void BT_ConfirmEffectPlanProgress_Click(object sender, EventArgs e)
    {
        //如流程是项目计划发起的，更新关联项目计划完成程度
        if (strRelatedType == "Plan" & strRelatedID != "0")
        {
            //更改关联的计划进度
            if (ShareClass.GetPlanProgressNeedPlanerConfirmByProject(ShareClass.GetProjectIDByPlanID(strRelatedID)) == "YES")
            {
                ShareClass.UpdateProjectPlanSchedule(strRelatedType, strRelatedID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYYXGLJHJD").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStepID, strStepName, strStepStatus;
        string strIsOperatorSelect, strIsPriorStepSelect, strTemName;

        int intSortNumber;

        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strStepID = e.Item.Cells[0].Text.Trim();

            strStepName = ShareClass.GetWorkFlowStep(strStepID).StepName.Trim();
            strStepStatus = ShareClass.GetWorkFlowStep(strStepID).Status.Trim();

            LB_StepID.Text = strStepID;
            LB_StepName.Text = strStepName;

            intSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID);
            strTemName = LB_TemName.Text.Trim();

            WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, intSortNumber);
            strIsOperatorSelect = workFlowTStep.OperatorSelect.Trim();
            strIsPriorStepSelect = workFlowTStep.IsPriorStepSelect.Trim();

            BT_Send.Enabled = false;
            TB_Message.Text = "";

            LoadWorkFlowStepDetail(strStepID, strIsOperatorSelect, strIsPriorStepSelect, strStepStatus);

            if (strIsOperatorSelect == "NO" | strStepStatus == "Passed")
            {
                CB_AllOperator.Enabled = false;
                CB_AllMust.Enabled = false;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strReceiverCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strStepID;

        strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
        strStepID = LB_StepID.Text.Trim();

        if (e.CommandName != "Page")
        {
            LB_ID.Text = strID;

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Select")
            {
                strHQL = "From WorkFlowStepDetail as workFlowStepDetail Where workFlowStepDetail.ID = " + strID;
                WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
                lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
                WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst[0];

                LB_OperatorCode.Text = workFlowStepDetail.OperatorCode.Trim();
                LB_OperatorName.Text = workFlowStepDetail.OperatorName.Trim();

                BT_Send.Enabled = true;
                TB_Message.Text = LanguageHandle.GetWord("GZLSPTZNHNYLCSC").ToString().Trim() + ": " + LB_WLID.Text.Trim() + " " + TB_WLName.Text.Trim() + LanguageHandle.GetWord("YSPQJSCL").ToString().Trim();

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShow('popwindowWFTStepDetail','false') ", true);
            }

            if (e.CommandName == "AddApprover")
            {
                string strUserCode;

                strUserCode = Session["UserCode"].ToString();
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);

                LoadAdditionApprover(strStepID, strUserCode);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);
            }
        }
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

        TreeView2.SelectedNode.Selected = false;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strDetailID;
        string strStepID;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();

        string strApproverCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strApproverName = ShareClass.GetUserName(strApproverCode);

        string strApproverType = LB_AddApproverType.Text.Trim();


        strStepID = LB_StepID.Text;
        strDetailID = LB_ID.Text;

        string strIsMust;
        if (((CheckBox)e.Item.FindControl("CB_IsMust")).Checked)
        {
            strIsMust = "YES";
        }
        else
        {
            strIsMust = "NO";
        }



        strHQL = "Select *  From T_WorkFlowStepDetail Where OperatorCode = '" + strApproverCode + "' and StepID = " + strStepID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoCiChengYuanYiCunZaiBu").ToString().Trim()+"')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);

            return;
        }

        string strApproverComment;
        strApproverComment = TB_ApproverComment.Text.Trim();

        if (strApproverComment == "")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoGongZuoNeiRongBuNengK").ToString().Trim()+"')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowAddApprover','false') ", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);
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
          ,FieldList
          ,EditFieldList
          ,'YES'
          ,''
          ,AllowFullEdit
          ,CanNotNullFieldList
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);
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

            strStepID = LB_StepID.Text;

            strHQL = "Delete From T_WorkFlowStepDetail Where CreatorCode = '" + strUserCode + "' and OperatorCode = '" + strOperatorCode + "' and StepID = " + strStepID;

            ShareClass.RunSqlCommand(strHQL);

            LoadAdditionApprover(strStepID, strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);
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

            strStepID = LB_StepID.Text;

            strHQL = "Delete From T_WorkFlowStepDetail Where CreatorCode = '" + strUserCode + "' and OperatorCode = '" + strOperatorCode + "' and StepID = " + strStepID;

            ShareClass.RunSqlCommand(strHQL);

            LoadAdditionApprover(strStepID, strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop34455", "popShow('popwindowWFTStepDetail','true','popwindowAddApprover') ", true);
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


    protected void BT_FindWFOperator_Click(object sender, EventArgs e)
    {
        string strOperatorName;
        string strStepID, strStepStatus;
        string strIsOperatorSelect, strIsPriorStepSelect, strTemName;
        int intSortNumber;

        strStepID = LB_StepID.Text.Trim();
        intSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID);
        strTemName = LB_TemName.Text.Trim();

        strStepStatus = ShareClass.GetWorkFlowStep(strStepID).Status.Trim();

        WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, intSortNumber);
        strIsOperatorSelect = workFlowTStep.OperatorSelect.Trim();
        strIsPriorStepSelect = workFlowTStep.IsPriorStepSelect.Trim();

        strOperatorName = TB_WFOperatorName.Text.Trim();

        LoadWorkFlowStepDetailByOperatorName(strStepID, strIsOperatorSelect, strIsPriorStepSelect, strStepStatus, strOperatorName);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }

    protected void BT_SaveOperatorSelect_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        try
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                strID = ((Button)DataGrid4.Items[i].FindControl("BT_ID")).Text.Trim();

                if (((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked == true)
                {
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'YES' Where ID = " + strID;
                }
                else
                {
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'NO'  Where ID = " + strID;
                }
                ShareClass.RunSqlCommand(strHQL);

                if (((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Checked == true)
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
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click111", "CloseTab('" + strMsgText + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click222", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }

    protected void DataGrid4_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox cbOperator = (CheckBox)e.Item.FindControl("CB_IsOperator");
            cbOperator.CheckedChanged += new System.EventHandler(this.cbSelectOperatorChanged);

            CheckBox cbMust = (CheckBox)e.Item.FindControl("CB_IsMust");
            cbMust.CheckedChanged += new System.EventHandler(this.cbSelectMustChanged);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }


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
                strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'NO' Where ID = " + strID;
            }

            ShareClass.RunSqlCommand(strHQL);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
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
                strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'NO' Where ID = " + strID;
            }
            ShareClass.RunSqlCommand(strHQL);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }

    protected void CB_AllOperator_CheckedChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        if (CB_AllOperator.Checked == true)
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = true;

                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid4.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'YES' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }
        else
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = false;

                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid4.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsOperator = 'NO' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }

    protected void CB_AllMust_CheckedChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        if (CB_AllMust.Checked == true)
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Checked = true;
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = true;

                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid4.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'YES',IsOperator = 'YES' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }
        else
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Checked = false;
                //((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = false;

                if (strAutoSaveWFOperator == "YES")
                {
                    strID = ((Button)(DataGrid4.Items[i].FindControl("BT_ID"))).Text.Trim();
                    strHQL = "Update T_WorkFlowStepDetail Set IsMust = 'NO',IsOperator = 'NO' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }


    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;
        string strOperatorCode, strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        strOperatorCode = LB_OperatorCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_SendMsg.Checked == true | CB_SendMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("GongZuoLiuShenPiTongZhi").ToString().Trim();
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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB").ToString().Trim() + "')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strProjectID = treeNode.Target.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        TB_ProjectID.Text = project.ProjectID.ToString();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWFTStepDetail','false') ", true);
    }

    protected void BT_AddProjectID_Click(object sender, EventArgs e)
    {
        string strProjectID, strWFID;
        string strHQL;
        IList lst;

        strProjectID = TB_ProjectID.Text.Trim();
        strWFID = LB_WLID.Text.Trim();

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        if (lst.Count > 0)
        {
            strHQL = "Update T_WorkFlow Set RelatedType = 'Project',RelatedID = " + strProjectID + " Where WLID = " + strWFID;
            ShareClass.RunSqlCommand(strHQL);

            LoadRelatedProject(strWFID);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJSBXMHCWWCXMSC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strUserCode, strProjectID, strWFID;


            strUserCode = LB_UserCode.Text.Trim();
            strWFID = LB_WLID.Text.Trim();

            strProjectID = e.Item.Cells[0].Text.Trim();

            strHQL = "Update T_WorkFlow Set RelatedType = 'Other',RelatedID = 0 Where WLID = " + strWFID;
            ShareClass.RunSqlCommand(strHQL);

            LoadRelatedProject(strWFID);
        }
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


    protected void LoadRelatedProject(string strWFID)
    {
        string strHQL;


        strHQL = "Select * From T_Project Where ProjectID in (Select RelatedID From T_WorkFlow Where RelatedType = 'Project' And WLID = " + strWFID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

    }

    protected int LoadWorkFlowStep(string strWLID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.WLID = " + strWLID + " Order by workFlowStep.StepID ASC";
        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        return lst.Count;
    }

    protected void LoadWorkFlow(string strWLID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataList2.DataSource = lst;
        DataList2.DataBind();
    }

    protected void LoadWorkFlowStepDetail(string strStepID, string strIsOperatorSelect, string strIsPriorStepSelect, string strStepStatus)
    {
        string strHQL;
        IList lst;
        string strIsOperator, strIsMust;
        string strID;
        int i;


        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.StepID = " + strStepID;
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        WorkFlowStepDetail workFlowStepDetail = new WorkFlowStepDetail();

        for (i = 0; i < lst.Count; i++)
        {
            workFlowStepDetail = (WorkFlowStepDetail)lst[i];

            strID = workFlowStepDetail.ID.ToString();

            strIsOperator = GetWorkFlowStepDetailOperatorStatus(strID);
            if (strIsOperator == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = false;
            }

            if (strIsOperatorSelect == "NO" | strStepStatus == "Passed")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Enabled = false;
            }

            //是否必须
            strIsMust = GetWorkFlowStepDetailMustStatus(strID);
            if (strIsMust == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Checked = false;
            }

            if (strIsOperatorSelect == "NO" | strStepStatus == "Passed")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Enabled = false;
            }

            if (strStepStatus == "Passed")
            {
                ((Button)DataGrid4.Items[i].FindControl("BT_AddApprover")).Enabled = false;
            }

            string strTemStepID = ShareClass.GetWorkFlowTStep(LB_TemName.Text, ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID)).StepID.ToString();
            if (ShareClass.GetWorkflowTemplateStepAllowNextStepAddApprover(strTemStepID) == "YES")
            {
                ((Button)DataGrid4.Items[i].FindControl("BT_AddApprover")).Enabled = true;
            }
            else
            {
                ((Button)DataGrid4.Items[i].FindControl("BT_AddApprover")).Enabled = false;
            }
        }
    }

    protected void LoadWorkFlowStepDetailByOperatorName(string strStepID, string strIsOperatorSelect, string strIsPriorStepSelect, string strStepStatus, string strOperatorName)
    {
        string strHQL;
        IList lst;
        string strIsOperator, strIsMust;
        string strID;
        int i;

        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();

        strOperatorName = "%" + strOperatorName + "%";

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.StepID = " + strStepID;
        strHQL += " and workFlowStepDetail.OperatorName Like '" + strOperatorName + "'";
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        WorkFlowStepDetail workFlowStepDetail = new WorkFlowStepDetail();

        for (i = 0; i < lst.Count; i++)
        {
            workFlowStepDetail = (WorkFlowStepDetail)lst[i];

            strID = workFlowStepDetail.ID.ToString();

            strIsOperator = GetWorkFlowStepDetailOperatorStatus(strID);
            if (strIsOperator == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Checked = false;
            }

            if (strIsOperatorSelect == "NO" | strStepStatus == "Passed")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Enabled = false;
            }

            //是否必须
            strIsMust = GetWorkFlowStepDetailMustStatus(strID);
            if (strIsMust == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Checked = false;
            }

            if (strIsOperatorSelect == "NO" | strStepStatus == "Passed")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsMust")).Enabled = false;
            }

            string strTemStepID = ShareClass.GetWorkFlowTStep(LB_TemName.Text, ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID)).StepID.ToString();
            if (ShareClass.GetWorkflowTemplateStepAllowNextStepAddApprover(strTemStepID) == "YES")
            {
                ((Button)DataGrid4.Items[i].FindControl("BT_AddApprover")).Enabled = true;
            }
            else
            {
                ((Button)DataGrid4.Items[i].FindControl("BT_AddApprover")).Enabled = false;
            }
        }
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

    protected int GetWorkFlowDetailStatusCount(string strWLID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.WLID = " + strWLID + " and workFlowStepDetail.Status = " + "'" + strStatus + "'";
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);

        return lst.Count;
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

    protected int GetUNReviewWorkFlowStepNumber(string strWLID)
    {
        string strHQL;
        IList lst;
        string strStatus;

        string strWLTemName = LB_TemName.Text.Trim();
        int intWLTemStepNumber, intWLSortNumber;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName = " + "'" + strWLTemName + "'";
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        intWLTemStepNumber = lst.Count;

        strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.WLID = " + strWLID + " AND workFlowStep.StepID = (select  max(workFlowStep.StepID) from  WorkFlowStep as workFlowStep where  workFlowStep.WLID = " + strWLID + ") Order by workFlowStep.StepID ASC";
        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);

        if (lst.Count > 0)
        {
            WorkFlowStep workFlowStep = (WorkFlowStep)lst[0];

            intWLSortNumber = workFlowStep.SortNumber;
            strStatus = workFlowStep.Status.Trim();

            if (strStatus == "Passed")
            {
                return intWLTemStepNumber - intWLSortNumber;
            }
            else
            {
                return intWLTemStepNumber - intWLSortNumber + 1;
            }
        }
        else
        {
            return 0;
        }
    }

}
