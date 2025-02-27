using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;

public partial class TTCreateProjectReqDetail : System.Web.UI.Page
{
    string strProjectID;
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strUserName;
        string strHQL;
        IList lst;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        string strReqID = Request.QueryString["ReqID"];

        strProjectID = GetReqRelatedProjectID(strReqID);

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        //this.Title = "建立和分派项目需求";

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
            DLC_ReqFinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from ReqType as reqType Order by reqType.SortNumber ASC";
            ReqTypeBLL reqTypeBLL = new ReqTypeBLL();
            lst = reqTypeBLL.GetAllReqTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            ShareClass.InitialProjectMemberTree(TreeView1, strProjectID);

            LoadProjectRelatedReq(strReqID, strUserCode);

            HL_ReqToTask.NavigateUrl = "TTReqToTask.aspx?ReqID=" + strReqID + "&ProjectID=" + strProjectID;
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strReqType, strReqName, strReqDetail, strAcceptStandard;
        string strReqFinishedDate, strApplicantCode, strStatus;
        DateTime dtMakeDate;

        string strReqID = LB_ReqID.Text.Trim();

        strReqType = DL_Type.SelectedValue;
        strReqName = TB_ReqName.Text.Trim();
        strReqDetail = TB_ReqDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strReqFinishedDate = DLC_ReqFinishedDate.Text;
        strApplicantCode = strUserCode;
        dtMakeDate = DateTime.Now;
        strStatus = LB_Status.Text.Trim();

        if (strReqID != "")
        {
            if (strReqName == "" | strReqDetail == "" | strReqFinishedDate == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);
            }
            else
            {
                Requirement requirement = new Requirement();
                RequirementBLL requirementBLL = new RequirementBLL();

                requirement.ReqType = strReqType;
                requirement.ReqName = strReqName;
                requirement.ReqDetail = strReqDetail;
                requirement.AcceptStandard = strAcceptStandard;
                requirement.ReqFinishedDate = DateTime.Parse(strReqFinishedDate);
                requirement.MakeDate = dtMakeDate;
                requirement.ApplicantCode = strApplicantCode;
                requirement.ApplicantName = ShareClass.GetUserName(strApplicantCode);
                requirement.Status = strStatus;

                try
                {
                    requirementBLL.UpdateRequirement(requirement, int.Parse(strReqID));

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXBNGXJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Close_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strReqType, strReqName, strReqDetail, strAcceptStandard;
        string strReqFinishedDate, strReceiverCode, strApplicantCode, strStatus;
        DateTime dtMakeDate;

        string strReqID = LB_ReqID.Text.Trim();

        strReqType = DL_Type.SelectedValue;
        strReqName = TB_ReqName.Text.Trim();
        strReqDetail = TB_ReqDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strReqFinishedDate = DLC_ReqFinishedDate.Text;
        strReceiverCode = TB_ReceiverCode.Text.Trim();
        strApplicantCode = strUserCode;
        dtMakeDate = DateTime.Now;
        strStatus = LB_Status.Text.Trim();

        if (strReqID != "")
        {
            if (strReqName == "" | strReqDetail == "" | strReqFinishedDate == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);
            }
            else
            {
                Requirement requirement = new Requirement();
                RequirementBLL requirementBLL = new RequirementBLL();

                requirement.ReqType = strReqType;
                requirement.ReqName = strReqName;
                requirement.ReqDetail = strReqDetail;
                requirement.AcceptStandard = strAcceptStandard;
                requirement.ReqFinishedDate = DateTime.Parse(strReqFinishedDate);
                requirement.MakeDate = dtMakeDate;
                requirement.ApplicantCode = strApplicantCode;
                requirement.ApplicantName = ShareClass.GetUserName(strApplicantCode);
                requirement.Status = "Closed";

                try
                {
                    requirementBLL.UpdateRequirement(requirement, int.Parse(strReqID));

                    LB_Status.Text = "Closed";

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGBCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGBCCJC").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXBNGXJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Open_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strReqType, strReqName, strReqDetail, strAcceptStandard;
        string strReqFinishedDate, strReceiverCode, strApplicantCode, strStatus;
        DateTime dtMakeDate;

        string strReqID = LB_ReqID.Text.Trim();

        strReqType = DL_Type.SelectedValue;
        strReqName = TB_ReqName.Text.Trim();
        strReqDetail = TB_ReqDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strReqFinishedDate = DLC_ReqFinishedDate.Text;
        strReceiverCode = TB_ReceiverCode.Text.Trim();
        strApplicantCode = strUserCode;
        dtMakeDate = DateTime.Now;
        strStatus = LB_Status.Text.Trim();


        if (strReqName == "" | strReqDetail == "" | strReqFinishedDate == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            Requirement requirement = new Requirement();
            RequirementBLL requirementBLL = new RequirementBLL();

            requirement.ReqType = strReqType;
            requirement.ReqName = strReqName;
            requirement.ReqDetail = strReqDetail;
            requirement.AcceptStandard = strAcceptStandard;
            requirement.ReqFinishedDate = DateTime.Parse(strReqFinishedDate);
            requirement.MakeDate = dtMakeDate;
            requirement.ApplicantCode = strApplicantCode;
            requirement.ApplicantName = ShareClass.GetUserName(strApplicantCode);
            requirement.Status = "InProgress";

            try
            {
                requirementBLL.UpdateRequirement(requirement, int.Parse(strReqID));

                LB_Status.Text = "InProgress";

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJHCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJHCCJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_Select_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadProjectRelatedReq(string strReqID, string strUserCode)
    {
        string strMakeUserCode, strHQL;
        string strStatus;
        string strReqName;

        strHQL = "from  Requirement as requirement where requirement.ReqID= " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();
        IList lst = requirementBLL.GetAllRequirements(strHQL);
        Requirement requirement = (Requirement)lst[0];

        strReqName = requirement.ReqName.Trim();
        strMakeUserCode = requirement.ApplicantCode.Trim();

        LB_ReqID.Text = requirement.ReqID.ToString();
        DL_Type.SelectedValue = requirement.ReqType;
        TB_ReqName.Text = requirement.ReqName;
        TB_ReqDetail.Text = requirement.ReqDetail;
        TB_AcceptStandard.Text = requirement.AcceptStandard;
        DLC_ReqFinishedDate.Text = requirement.ReqFinishedDate.ToString("yyyy-MM-dd");
        LB_Status.Text = requirement.Status;
        strStatus = requirement.Status.Trim();

        HL_RelatedDoc.NavigateUrl = "";
        HL_RelatedDoc.NavigateUrl = "TTReqRelatedDoc.aspx?ReqID=" + strReqID;
        HL_ApproveRecord.NavigateUrl = "";
        HL_ApproveRecord.NavigateUrl = "TTReqAssignRecord.aspx?ReqID=" + strReqID;

        HL_ReqReview.Enabled = true;
        HL_ReqReview.NavigateUrl = "TTReqReviewWL.aspx?ReqID=" + strReqID;
        HL_WFTemName.Enabled = true;
        HL_WFTemName.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=Req&RelatedID=" + strReqID;

        HL_RelatedWorkFlowTemplate.Enabled = true;
        HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=Req&RelatedID=" + strReqID;
        HL_ActorGroup.Enabled = true;
        HL_ActorGroup.NavigateUrl = "TTRelatedActorGroup.aspx?RelatedType=Req&RelatedID=" + strReqID;

        HL_RelatedDoc.Enabled = true;
        HL_ApproveRecord.Enabled = true;

        BT_Update.Enabled = true;
        BT_Delete.Enabled = true;
        BT_Close.Enabled = true;
        BT_Open.Enabled = true;
        BT_Assign.Enabled = true;

        if (strStatus == "Closed")
        {
            BT_Open.Enabled = true;
        }

        TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaLeXuQiu").ToString().Trim() + strReqID + "  " + strReqName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL, strID, strUserCode, strUserName;
        IList lst;

        strID = TreeView1.SelectedNode.Target.Trim();

        try
        {
            strID = int.Parse(strID).ToString();

            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ID = " + strID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ProRelatedUser proRelatedUser = (ProRelatedUser)lst[0];

                strUserCode = proRelatedUser.UserCode.Trim();
                strUserName = proRelatedUser.UserName.Trim();

                LB_ReceiverName.Visible = true;

                TB_ReceiverCode.Text = strUserCode;
                LB_ReceiverName.Text = strUserName;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWDJCYMCJD").ToString().Trim() + "')", true);
            }
        }
        catch
        {
        }

        TreeView1.SelectedNode.Selected = false;
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strReqID = LB_ReqID.Text.Trim();
        string strHQL;
        IList lst;

        if (strReqID != "")
        {
            strHQL = "from Approve as approve where approve.Type = 'Requirement' and approve.RelatedID = " + strReqID;
            ApproveBLL approveBLL = new ApproveBLL();
            lst = approveBLL.GetAllApproves(strHQL);

            if (lst.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXCZSPJLBNSC").ToString().Trim() + "')", true);
            }
            else
            {
                Requirement requirement = new Requirement();
                RequirementBLL requirementBLL = new RequirementBLL();

                requirement.ReqID = int.Parse(strReqID);

                try
                {
                    requirementBLL.DeleteRequirement(requirement);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);


                    LB_ReqID.Text = "";
                    TB_ReqName.Text = "";
                    TB_ReqDetail.Text = "";
                    TB_ReceiverCode.Text = "";
                    DLC_ReqFinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TB_AcceptStandard.Text = "";
                    LB_Status.Text = "Plan";
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXBNSCJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Assign_Click(object sender, EventArgs e)
    {
        int intReqID, intPriorID;
        string strReqName, strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;
        string strContent, strOperation, strType;
        DateTime dtBeginDate, dtEndDate, dtMakeDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
        intReqID = int.Parse(LB_ReqID.Text.Trim());
        strType = DL_Type.SelectedValue.Trim();
        strReqName = TB_ReqName.Text.Trim();
        strOperatorCode = TB_ReceiverCode.Text.Trim();
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
        dtBeginDate = DateTime.Parse(DLC_BeginDate.Text);
        dtEndDate = DateTime.Parse(DLC_EndDate.Text);
        dtMakeDate = DateTime.Now;

        if (strOperation == "" | strOperatorCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBFPRYJGZYHSLRBNWKJC").ToString().Trim() + "')", true);
            return;
        }

        ReqAssignRecordBLL reqAssignRecordBLL = new ReqAssignRecordBLL();
        ReqAssignRecord reqAssignRecord = new ReqAssignRecord();

        reqAssignRecord.ReqID = intReqID;
        reqAssignRecord.Type = strType;
        reqAssignRecord.ReqName = strReqName;
        reqAssignRecord.OperatorCode = strOperatorCode;
        reqAssignRecord.OperatorName = strOperatorName;
        reqAssignRecord.OperatorContent = "";
        reqAssignRecord.OperationTime = DateTime.Now;
        reqAssignRecord.BeginDate = dtBeginDate;
        reqAssignRecord.EndDate = dtEndDate;
        reqAssignRecord.AssignManCode = strAssignManCode;
        reqAssignRecord.AssignManName = strAssignManName;
        reqAssignRecord.Content = "";
        reqAssignRecord.Operation = strOperation;
        reqAssignRecord.PriorID = intPriorID;
        reqAssignRecord.RouteNumber = GetRouteNumber(intReqID.ToString());
        reqAssignRecord.MakeDate = dtMakeDate;
        reqAssignRecord.Status = "ToHandle";
        reqAssignRecord.MoveTime = DateTime.Now;

        try
        {
            reqAssignRecordBLL.AddReqAssignRecord(reqAssignRecord);
            UpdateReqStatus(intReqID.ToString(), "InProgress");

            TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("FenPaLeXuQiu").ToString().Trim() + intReqID.ToString() + " " + strReqName + LanguageHandle.GetWord("GeiNiQingJiShiShouLi").ToString().Trim();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strSubject, strMsg;
        string strOperatorCode, strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        strOperatorCode = TB_ReceiverCode.Text.Trim();

        Msg msg = new Msg();

        if (CB_SendMsg.Checked == true | CB_SendMail.Checked == true)
        {
            strSubject = LanguageHandle.GetWord("XuQiuFenPaTongZhi").ToString().Trim();
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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSWB").ToString().Trim() + "')", true);
    }

    protected void AddProjectRelatedReq(string strProjectID, string strUserCode)
    {
        string strHQL;
        IList lst;
        string strReqID;

        string strStatus = ShareClass.GetProjectStatus(strProjectID);

        if (strStatus == "CaseClosed" || strStatus == "Suspended" || strStatus == "Cancel")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFJSBCXMYBJAGHX").ToString().Trim() + "')", true);
        }
        else
        {
            strHQL = "from Requirement as requirement where requirement.ApplicantCode = " + "'" + strUserCode + "'" + " Order By ReqID DESC";
            RequirementBLL requirementBLL = new RequirementBLL();
            lst = requirementBLL.GetAllRequirements(strHQL);
            Requirement requirement = (Requirement)lst[0];

            strReqID = requirement.ReqID.ToString();

            RelatedReqBLL relatedReqBLL = new RelatedReqBLL();
            strHQL = "from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + " and relatedReq.ReqID = " + strReqID;
            lst = relatedReqBLL.GetAllRelatedReqs(strHQL);

            if (lst.Count == 0)
            {
                RelatedReq relatedReq = new RelatedReq();

                relatedReq.ProjectID = int.Parse(strProjectID);
                relatedReq.ReqID = int.Parse(strReqID);

                try
                {
                    relatedReqBLL.AddRelatedReq(relatedReq);
                }
                catch
                {

                }
            }
            else
            {

            }
        }
    }

    protected void UpdateReqStatus(string strReqID, string strStatus)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();
        IList lst = requirementBLL.GetAllRequirements(strHQL);
        Requirement requirement = (Requirement)lst[0];
        requirement.Status = "InProgress";

        int intRouteNumber = requirement.RouteNumber;
        requirement.RouteNumber = intRouteNumber + 1;

        try
        {
            requirementBLL.UpdateRequirement(requirement, int.Parse(strReqID));
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC").ToString().Trim() + "')", true);
        }
    }

    private int GetRouteNumber(string strReqID)
    {
        RequirementBLL requirementBLL = new RequirementBLL();
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement.RouteNumber + 1;
    }


    protected Project GetProject(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project;
    }

    protected string GetProjectStatus(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.Status.Trim();
    }

    protected string GetReqRelatedProjectID(string strReqID)
    {
        string strHQL;
        IList lst;

        strHQL = "from RelatedReq as relatedReq where relatedReq.ReqID = " + strReqID;
        RelatedReqBLL relatedReqBLL = new RelatedReqBLL();
        lst = relatedReqBLL.GetAllRelatedReqs(strHQL);

        RelatedReq relatedReq = (RelatedReq)lst[0];

        return relatedReq.ProjectID.ToString();

    }

}
