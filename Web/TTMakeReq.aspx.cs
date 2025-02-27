using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTMakeReq : System.Web.UI.Page
{
    string strUserCode;
    string strIsMobileDevice;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        string strDepartCode;

        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_Operation);

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;


        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx");
        //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}

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

            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ReqFinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from ReqType as reqType Order by reqType.SortNumber ASC";
            ReqTypeBLL reqTypeBLL = new ReqTypeBLL();
            lst = reqTypeBLL.GetAllReqTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();
            DL_Type.Items.Insert(0, new ListItem("--Select--", ""));

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);

            LoadRequirement();

            //BusinessForm,列出业务表单类型 
            ShareClass.LoadWorkflowType(DL_WLType, strLangCode);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        //BusinessForm，隐藏业务表单元素
        Panel_RelatedBusiness.Visible = false;

        LB_ReqID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop1", "popShow('popAssignWindow','true') ", true);
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop2", "popShow('popSelectwindow','false') ", true);

        TreeView1.SelectedNode.Selected = false;
    }

    protected void BT_Select_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop1", "popShow('popAssignWindow','true') ", true);
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop2", "popShow('popSelectwindow','false') ", true);
    }

    protected void BT_AllReq_Click(object sender, EventArgs e)
    {
        LoadRequirement();
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strDepartCode = ((Button)e.Item.FindControl("BT_DepartCode")).Text.Trim();
            string strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

            IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void DL_AllowUpdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strReqID, strAllowUpdate;

        strAllowUpdate = DL_AllowUpdate.SelectedValue;
        strReqID = LB_ReqID.Text.Trim();

        try
        {
            strHQL = "Update T_RelatedBusinessForm Set AllowUpdate = '" + strAllowUpdate + "'  Where RelatedType = 'Req' and RelatedID = " + strReqID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_RelatedBusinessForm Set AllowUpdate = '" + strAllowUpdate + "'  Where RelatedType = 'ReqRecord' ";
            strHQL += " and RelatedID in (Select ID From T_ReqAssignRecord Where ReqID =  " + strReqID + ")";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strReqID;

        strReqID = LB_ReqID.Text.Trim();

        if (strReqID == "")
        {
            AddRequirement();
        }
        else
        {
            UpdateRequirement();
        }
    }

    protected void AddRequirement()
    {
        string strReqID;
        string strUserCode = LB_UserCode.Text.Trim();
        string strApplicantName;
        string strReqType, strReqName, strReqDetail, strAcceptStandard;
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
                LB_ReqID.Text = strReqID;

                //自动分派需求给创建者
                AssignRequirement(int.Parse(strReqID), strReqType, strReqName, strReqName);

                HL_RelatedDoc.NavigateUrl = "TTReqRelatedDoc.aspx?ReqID=" + strReqID;
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
                BT_Close.Enabled = true;
                BT_Open.Enabled = true;
                BT_Assign.Enabled = true;

                LoadRequirement();


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBFPRDMCWCWCRJC").ToString().Trim() + "')", true);

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
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void UpdateRequirement()
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

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

                    //BusinessForm，关联相应的业务表单模板
                    ShareClass.SaveRelatedBusinessForm("Req", strReqID, DL_WFTemplate.SelectedValue, DL_AllowUpdate.SelectedValue, strUserCode);

                    LoadRequirement();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXBNGXJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
                    LoadRequirement();
                    LB_Status.Text = ShareClass.GetStatusHomeNameByRequirementStatus("Closed");

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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
                LoadRequirement();
                LB_Status.Text = ShareClass.GetStatusHomeNameByRequirementStatus("InProgress");

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJHCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJHCCJC").ToString().Trim() + "')", true);
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected bool UpdateWorkFlowStatus(string strWLID, string strStatus)
    {
        string strCmdText, strDIYNextStep, strSMS, strEMail;

        strSMS = "YES";
        strEMail = "YES";
        strDIYNextStep = "YES";

        strCmdText = "Update T_WorkFlow Set Status = " + "'" + strStatus + "'" + ",DIYNextStep = " + "'" + strDIYNextStep + "'" + ",ReceiveSMS = " + "'" + strSMS + "'" + ",ReceiveEMail = " + "'" + strEMail + "'" + "  where WLID = " + strWLID;

        try
        {
            ShareClass.RunSqlCommand(strCmdText);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode, strHQL;
            string strStatus;
            string strReqID, strReqName;

            strUserCode = LB_UserCode.Text.Trim();

            strReqID = e.Item.Cells[3].Text.Trim();

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                strHQL = "from  Requirement as requirement where requirement.ReqID= " + strReqID;

                RequirementBLL requirementBLL = new RequirementBLL();

                IList lst = requirementBLL.GetAllRequirements(strHQL);

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                Requirement requirement = (Requirement)lst[0];

                strReqName = requirement.ReqName.Trim();

                LB_ReqID.Text = requirement.ReqID.ToString();
                DL_Type.SelectedValue = requirement.ReqType;
                TB_ReqName.Text = requirement.ReqName;
                TB_ReqDetail.Text = requirement.ReqDetail;
                TB_AcceptStandard.Text = requirement.AcceptStandard;
                DLC_ReqFinishedDate.Text = requirement.ReqFinishedDate.ToString("yyyy-MM-dd");
                LB_Status.Text = ShareClass.GetStatusHomeNameByRequirementStatus(requirement.Status.Trim());
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
                BT_Close.Enabled = true;
                BT_Open.Enabled = true;
                BT_Assign.Enabled = true;

                //BusinessForm，列出关联表单模板
                try
                {
                    Panel_RelatedBusiness.Visible = true;

                    string strTemName;
                    strHQL = "Select * From T_RelatedBusinessForm Where RelatedType = 'Req' and RelatedID = " + strReqID;
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
                ShareClass.LoadBusinessForm("Req", strReqID, DL_WFTemplate.SelectedValue.Trim(), IFrame_RelatedInformation);


                if (strStatus == "Closed")
                {
                    BT_Open.Enabled = true;
                }

                TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeXuQiu").ToString().Trim() + "：" + strReqID  + " " + strReqName + "，" + LanguageHandle.GetWord("QingJiShiChuLi").ToString().Trim();

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                }

                if (e.CommandName == "Assign")
                {
                    HT_Operation.Text = TB_ReqDetail.Text;
                    HE_Operation.Text = TB_ReqDetail.Text;
                    try
                    {
                        TB_ReceiverCode.Text = strUserCode;
                        LB_ReceiverName.Text = ShareClass.GetUserName(strUserCode);
                        LB_ReceiverName.Visible = true;
                    }
                    catch
                    {
                    }

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
                }
            }


            if (e.CommandName == "Delete")
            {
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

                            LoadRequirement();

                            LB_ReqID.Text = "";
                            TB_ReqName.Text = "";
                            TB_ReqDetail.Text = "";
                            TB_ReceiverCode.Text = "";
                            DLC_ReqFinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                            TB_AcceptStandard.Text = "";
                            LB_Status.Text = ShareClass.GetStatusHomeNameByRequirementStatus("Plan");

                            HL_RelatedDoc.Enabled = false;
                            HL_ApproveRecord.Enabled = false;

                            BT_Close.Enabled = false;
                            BT_Open.Enabled = false;
                            BT_Assign.Enabled = false;

                            HL_ReqReview.Enabled = false;
                            HL_WFTemName.Enabled = false;

                            HL_ActorGroup.Enabled = false;
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
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strReceiverCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

        LB_ReceiverName.Visible = true;

        TB_ReceiverCode.Text = strReceiverCode;
        LB_ReceiverName.Text = ShareClass.GetUserName(strReceiverCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        RequirementBLL requirementBLL = new RequirementBLL();
        IList lst = requirementBLL.GetAllRequirements(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void BT_Assign_Click(object sender, EventArgs e)
    {
        int intReqID, intPriorID;
        string strReqName, strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;
        string strOperation, strType;
        DateTime dtBeginDate, dtEndDate, dtMakeDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
        intReqID = int.Parse(LB_ReqID.Text.Trim());
        strType = DL_Type.SelectedValue.Trim();
        strReqName = TB_ReqName.Text.Trim();
        strOperatorCode = TB_ReceiverCode.Text.Trim();
        try
        {
            strOperatorName = ShareClass.GetUserName(strOperatorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBFPRDMCWCWCRJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
            return;
        }
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

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
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

            string strAssignID = ShareClass.GetMyCreatedMaxReqAssignRecordID(intReqID.ToString(), strUserCode);

            //BusinessForm,处理关联的业务表单数据
            ShareClass.InsertOrUpdateTaskAssignRecordWFXMLData("Req", intReqID.ToString(), "ReqRecord", strAssignID, strUserCode);

            ShareClass.SendInstantMessage(LanguageHandle.GetWord("XuQiuFenPaiTongZhi").ToString().Trim(), ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeXuQiu").ToString().Trim() + " :" + intReqID.ToString() + "  " + strReqName + "，" + LanguageHandle.GetWord("QingJiShiChuLi").ToString().Trim(), strUserCode, strOperatorCode);

            TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaiLeXuQiu").ToString().Trim() + "：" + intReqID.ToString() + " " + strReqName + "，" + LanguageHandle.GetWord("QingJiShiChuLi").ToString().Trim();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

        string strReqID;
        strReqID = LB_ReqID.Text.Trim();


        if (strReqID == "")
        {
            strReqID = "0";
        }

        //strURL = "popShowByURL(" + "'TTRelatedDIYBusinessForm.aspx?RelatedType=Req&RelatedID=" + strReqID + "&IdentifyString=" + strIdentifyString + "','" + LanguageHandle.GetWord("XiangGuanYeWuDan").ToString().Trim() + "', 800, 600,window.location);";
        //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop12", strURL, true);

        strURL = "TTRelatedDIYBusinessForm.aspx?RelatedType=Req&RelatedID=" + strReqID + "&IdentifyString=" + strIdentifyString;
        IFrame_RelatedInformation.Attributes.Add("src", strURL);


        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    //BusinessForm,删除关联的业务表单
    protected void BT_DeleteBusinessForm_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strTemName;
        strTemName = DL_WFTemplate.SelectedValue.Trim();

        string strReqID;
        strReqID = LB_ReqID.Text.Trim();

        strHQL = "Delete From T_RelatedBusinessForm Where RelatedType = 'Req' and RelatedID = " + strReqID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void UpdateReqStatus(string strReqID, string strStatus)
    {
        string strHQL;
        IList lst;
        int intRouteNumber;

        strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();
        lst = requirementBLL.GetAllRequirements(strHQL);
        Requirement requirement = (Requirement)lst[0];
        requirement.Status = "InProgress";

        intRouteNumber = requirement.RouteNumber;
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

    protected void LoadRequirement()
    {
        string strHQL, strUserCode;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();

        strHQL = "from Requirement as requirement where requirement.ApplicantCode = " + "'" + strUserCode + "'" + " order by requirement.ReqID DESC";

        RequirementBLL requirementBLL = new RequirementBLL();
        lst = requirementBLL.GetAllRequirements(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }


}
