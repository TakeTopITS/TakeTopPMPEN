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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTCreateReqDetailThirdPart : System.Web.UI.Page
{
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strUserName;
        string strDepartCode;
        string strReqID;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_Operation);
HE_Operation.Language = Session["LangCode"].ToString();

        strReqID = Request.QueryString["ReqID"];

        //this.Title = "建立和分派需求（合作）";

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

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

            strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName ";
            strHQL += " in ( Select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();


            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);

            LoadRequirement(strReqID, strUserCode);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupName = ((Button)e.Item.FindControl("BT_ActorGroup")).Text.Trim();
            string strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'";

            ActorGroupDetailBLL actroGroupDetailBLL = new ActorGroupDetailBLL();
            IList lst = actroGroupDetailBLL.GetAllActorGroupDetails(strHQL);

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strApplicantName;
        string strReqID,strReqType, strReqName, strReqDetail, strAcceptStandard;
        string strReqFinishedDate, strApplicantCode, strStatus;
        DateTime dtMakeDate;


        strReqType = DL_Type.SelectedValue;
        strReqName = TB_ReqName.Text.Trim();
        strReqDetail = TB_ReqDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strReqFinishedDate = DLC_ReqFinishedDate.Text;
        strApplicantCode = strUserCode;
        strApplicantName = ShareClass.GetUserName(strUserCode);
        dtMakeDate = DateTime.Now;
        strStatus = "Plan";

        if (strReqName == "" | strReqDetail == "" | strReqFinishedDate == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC") + "')", true);
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
            requirement.ApplicantName = strApplicantName;
            requirement.Status = strStatus;

            try
            {
                requirementBLL.AddRequirement(requirement);

                strReqID = ShareClass.GetMyCreatedMaxReqID(strUserCode);

                //自动分派需求给创建者
                AssignRequirement(int.Parse(strReqID), strReqType, strReqName, strReqName);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                BT_Assign.Enabled = false;

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            }
        }
    }

    //自动分派需求给创建者
    protected void AssignRequirement(int intReqID, string strType, string strReqName, string strOperation)
    {
        int intPriorID;
        string strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;
        DateTime dtBeginDate, dtEndDate, dtMakeDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        strOperatorCode = Session["UserCode"].ToString();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBFPRDMCWCWCRJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
            return;
        }
        strAssignManCode = LB_UserCode.Text.Trim();
        strAssignManName = LB_UserName.Text.Trim();

        intPriorID = 0;
        dtBeginDate = DateTime.Parse(DLC_BeginDate.Text);
        dtEndDate = DateTime.Parse(DLC_EndDate.Text);
        dtMakeDate = DateTime.Now;

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

            string strAssignID = ShareClass.GetMyCreatedMaxReqAssignRecordID(intReqID.ToString(), strUserCode);

            //BusinessForm,处理关联的业务表单数据
            ShareClass.InsertOrUpdateTaskAssignRecordWFXMLData("Req", intReqID.ToString(), "ReqRecord", strAssignID, strUserCode);
        }
        catch
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC") + "')", true);
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC") + "')", true);
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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXBNGXJC") + "')", true);
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC") + "')", true);
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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGBCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGBCCJC") + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXBNGXJC") + "')", true);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC") + "')", true);
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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJHCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJHCCJC") + "')", true);
            }
        }
    }


    protected void BT_Select_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadRequirement(string strReqID, string strUserCode)
    {
        string strHQL;
        string strStatus;
        string strReqName;

        strHQL = "from  Requirement as requirement where requirement.ReqID= " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();
        IList lst = requirementBLL.GetAllRequirements(strHQL);
        Requirement requirement = (Requirement)lst[0];

        strReqName = requirement.ReqName.Trim();

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

        HL_RunReqByWF.Enabled = true;
        HL_RunReqByWF.NavigateUrl = "TTRelatedDIYWorkFlowForm.aspx?RelatedType=Req&RelatedID=" + strReqID;

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

        TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaLeXuQiu") + strReqID + "  " + strReqName + LanguageHandle.GetWord("QingJiShiShouLi");
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strReceiverCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

        LB_ReceiverName.Visible = true;

        TB_ReceiverCode.Text = strReceiverCode;
        LB_ReceiverName.Text = ShareClass.GetUserName(strReceiverCode);
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXCZSPJLBNSC") + "')", true);
            }
            else
            {
                Requirement requirement = new Requirement();
                RequirementBLL requirementBLL = new RequirementBLL();

                requirement.ReqID = int.Parse(strReqID);

                try
                {
                    requirementBLL.DeleteRequirement(requirement);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);

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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXBNSCJC") + "')", true);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBFPRYJGZYHSLRBNWKJC") + "')", true);
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

            TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("FenPaLeXuQiu") + intReqID.ToString() + " " + strReqName + LanguageHandle.GetWord("GeiNiQingJiShiShouLi");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC") + "')", true);
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
            strSubject = LanguageHandle.GetWord("XuQiuFenPaTongZhi");
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

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strUserCode = LB_UserCode.Text.Trim();
        string strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName ";
        strHQL += " in ( Select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        IList lst = actorGroupBLL.GetAllActorGroups(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC") + "')", true);
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

}
