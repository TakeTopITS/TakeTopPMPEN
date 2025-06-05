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
using System.Data.SqlClient;
using System.Linq;

using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.Text;
using System.Collections.Generic;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopWF;


public partial class TTAppMyWorkflowDetail : System.Web.UI.Page
{
    string strLangCode;
    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strWLID = Request.QueryString["WLID"];

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
        strCreatorCode = workFlow.CreatorCode.Trim();

        strRelatedType = workFlow.RelatedType.Trim();
        strRelatedID = workFlow.RelatedID.ToString();

        DataList2.DataSource = lst;
        DataList2.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack != true)
        {
            strUserName = Session["UserName"].ToString();
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;

            if (Session["DIYWFModule"].ToString() == "YES")
            {
                a_return.Visible = false;
            }

            BT_Active.Attributes.Add("onclick ", "SaveDIYFormDate('Active');");
            BT_Close.Attributes.Add("onclick ", "SaveDIYFormDate('Close');");
            BT_Updating.Attributes.Add("onclick ", "SaveDIYFormDate('Updating');");
            BT_ReActive.Attributes.Add("onclick ", "SaveDIYFormDate('ReActive');");

            HL_WLRelatedDoc.NavigateUrl = "TTAPPWLRelatedDoc.aspx?WLID=" + strWLID + "&ID=0";
            HL_ApproveRecord.NavigateUrl = "TTWorkflowApproveRecord.aspx?Type=WorkFlow&RelatedID=" + strWLID + "&StepID=0";
            HL_StepApproveRecord.NavigateUrl = "TTWorkflowApproveRecord.aspx?Type=Step&RelatedID=" + strWLID + "&StepID=0";
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=WORKFLOW&RelatedID=" + strWLID;

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

            ////显示流程红绿灯
            //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

            string strDesignType = ShareClass.GetWLTemplateDesignType(strTemName);
            if (strDesignType == "SL")
            {
                HL_WFChartView.NavigateUrl = "TTWFChartViewSL.aspx?IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(strTemName);
            }
            if (strDesignType == "JS")
            {
                HL_WFChartView.NavigateUrl = "TTWFChartViewJS.aspx?IdentifyString=" + ShareClass.GetWLTemplateIdentifyString(strTemName);
            }
            try
            {
                //取得归属于此步的流程模板步骤
                WorkFlowTStep workFlowTStep = ShareClass.GetWorkFlowTStep(strTemName, 0);
                string strWFTemStepGUID = workFlowTStep.GUID.Trim();
                //列出关联模块
                LoadWorkFlowTStepRelatedModule(strWLID, "0", "0", strWFTemStepGUID, strLangCode, strUserCode);
            }
            catch
            {

            }


            //显示关联项目树
            ShareClass.InitialPrjectTreeByAuthority(TreeView1, strUserCode);
            LoadRelatedProject(strWLID);

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
                BT_Updating.Enabled = false;
                BT_Delete.Enabled = false;
                BT_Active.Enabled = false;
                BT_Close.Enabled = false;
                BT_Success.Enabled = false;

                BT_ConfirmEffectPlanProgress.Visible = false;
                return;
            }

            if (strStatus == "CaseClosed")
            {
                BT_Updating.Enabled = false;
                BT_Delete.Enabled = false;
                BT_Active.Enabled = false;
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

        if (intResult == 1)
        {
            BT_Active.Enabled = false;

            LoadWorkFlowStep(strWLID);
            LB_UNReviewNumber.Text = wfActiveHandle.GetUNReviewWorkFlowStepNumber(strWLID, strTemName).ToString();

            ////显示流程红绿灯
            //ShareClass.DisplayRelatedWFStepDump(strTemName, strWLID, strStatus, Repeater1);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZChongXinLanguageHandleGetWor") + LanguageHandle.GetWord("ZZSBJC") + "')", true); 
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
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert(OK')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYYXGLJHJD") + "')", true);
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

            strStepID = ((Button)e.Item.FindControl("BT_StepID")).Text;
            strStepName = ShareClass.GetWorkFlowStep(strStepID).StepName.Trim();
            strStepStatus = ShareClass.GetWorkFlowStep(strStepID).Status.Trim();

            LB_StepID.Text = strStepID;
            LB_StepName.Text = strStepName;

            intSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID);
            strTemName = LB_TemName.Text.Trim();

            WorkFlowTStep workFlowTStep = ShareClass. GetWorkFlowTStep(strTemName, intSortNumber);
            strIsOperatorSelect = workFlowTStep.OperatorSelect.Trim();
            strIsPriorStepSelect = workFlowTStep.IsPriorStepSelect.Trim();


            BT_Send.Enabled = false;
            TB_Message.Text = "";

            LoadWorkFlowStepDetail(strStepID, strIsOperatorSelect, strIsPriorStepSelect, strStepStatus);

            //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strReceiverCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

        //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
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
                TB_Message.Text = LanguageHandle.GetWord("GZLSPTZNHNYLCSC") + ": " + LB_WLID.Text.Trim() + " " + TB_WLName.Text.Trim() + LanguageHandle.GetWord("YSPQJSCL");
            }

            //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
        }
    }

    protected void DataGrid4_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox cb = (CheckBox)e.Item.FindControl("CB_IsOperator");
            cb.CheckedChanged += new System.EventHandler(this.cbselectChanged);
        }
    }

    //创建事件
    protected void cbselectChanged(object sender, System.EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        DataGridItem li = (DataGridItem)cb.Parent.Parent;

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

        //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
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
            strSubject = LanguageHandle.GetWord("GongZuoLiuShenPiTongZhi");
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

        //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
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

        //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJSBXMHCWWCXMSC") + "')", true);
        }

        //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
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

            //DivID.Style.Value = "width: 600px; height: 500px; overflow: auto;";
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

    protected void LoadWorkFlowStepDetail(string strStepID, string strIsOperatorSelect, string strIsPriorStepSelect, string strStepStatus)
    {
        string strHQL;
        IList lst;
        string strIsOperator;
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

            if (strIsOperatorSelect == "NO" | strIsPriorStepSelect == "YES" | strStepStatus == "Passed")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_IsOperator")).Enabled = false;
            }
        }
    }

    protected void LoadWorkFlowTStepRelatedModule(string strWorkflowID, string strWorkflowStepID, string strWorkflowStepDetailID, string strStepGUID, string strLangCode, string strUserCode)
    {
        string strHQL;

        strHQL = " Select distinct B.HomeModuleName,B.PageName,A.ModuleName," + strWorkflowID + " as WorkflowID," + strWorkflowStepID + " as WorkflowStepID," + strWorkflowStepDetailID + " as WorkflowStepDetailID" + " From T_WorkFlowTStepRelatedModule A,T_ProModuleLevel B ";
        strHQL += " Where StepGUID = '" + strStepGUID + "' and A.ModuleName = B.ModuleName and B.LangCode = '" + strLangCode + "'";
        strHQL += " And A.ModuleName in (Select ModuleName From T_ProModule Where Visible = 'YES' and IsDeleted = 'NO' and UserCode = '" + strUserCode + "')";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStepRelatedModule");

        RP_RelatedModule.DataSource = ds;
        RP_RelatedModule.DataBind();
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

    protected int GetWorkFlowDetailStatusCount(string strWLID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.WLID = " + strWLID + " and workFlowStepDetail.Status = " + "'" + strStatus + "'";
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);

        return lst.Count;
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

    protected string GetWorkFlowStepDetailOperatorStatus(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.ID = " + strID;
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
        WorkFlowStepDetail workFlowStepDetail = (WorkFlowStepDetail)lst[0];

        return workFlowStepDetail.IsOperator.Trim();
    }

}
