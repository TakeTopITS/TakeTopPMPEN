using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWFTStepSet : System.Web.UI.Page
{
    private string strUserCode, strMakeUserCode, strIdentifyString, strTemName, strGUID, strDesignType;
    private string strUserType;
    private string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName, strAction, strStepName;

        strLangCode = Session["LangCode"].ToString();

        strGUID = Request.QueryString["GUID"].Trim();
        strAction = Request.QueryString["Action"].Trim();
        strIdentifyString = Request.QueryString["IdentifyString"].Trim();

        strDesignType = Request.QueryString["DesignType"];
        strStepName = Request.QueryString["RectText"];

        strTemName = GetWorkFlowTemName(strIdentifyString);
        strMakeUserCode = GetMakeUserCode(strTemName);

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strUserType = ShareClass.GetUserType(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        LB_WFTemplate.Text = strTemName;

        Page.ClientScript.RegisterStartupScript(this.GetType(), "test3", "javascript:document.forms[0]['TB_WFXML'].value=window.parent.document.getElementById('TB_WFXML').value;", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strUserType == "INNER")
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthority(strUserCode);
            }
            else
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByUserInfor(strUserCode);
            }

            InitialActorGroupTree(TreeView1, strUserCode, strLangCode);

            LoadRelatedWFType(strUserCode);
            LoadRelatedWorkFlowTemplate(strUserCode);

            if (strAction == "Insert")
            {
                CreateOrLoadWorkFlowStep(strGUID);
            }

            if (strAction == "Delete")
            {
                if (Session["SuperWFAdmin"].ToString() == "YES" | strMakeUserCode == strUserCode)
                {
                    DeleteWorkFlowTStep(strGUID);
                }
            }

            if (strStepName != null)
            {
                TB_StepName.Text = strStepName;
                TB_WorkDetail.Text = strStepName;

                string strHQL;
                IList lst;

                strHQL = "Update T_WorkFlowTStep Set StepName = '" + strStepName + "' Where GUID = '" + strGUID + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.GUID = " + "'" + strGUID + "'";
                WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
                lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

                DataGrid2.DataSource = lst;
                DataGrid2.DataBind();
            }

            if (Session["SuperWFAdmin"].ToString() == "YES" | strMakeUserCode == strUserCode)
            {
                BT_Update.Enabled = true;
                BT_AddChileTem.Enabled = true;
                BT_AddStepDetail.Enabled = true;
            }
            else
            {
                BT_Update.Enabled = false;
                BT_AddChileTem.Enabled = false;
                BT_AddStepDetail.Enabled = false;
            }

            HL_XMLFile.NavigateUrl = ShareClass.GetWorkFlowLastestXMLFile(strTemName);

            HL_BusinessMember.NavigateUrl = "TTWorkFlowTemplateStepBusinessMember.aspx?IdentifyString=" + strIdentifyString + "&StepGUID=" + strGUID;
            HL_BusinessMember.Target = "_blank";
            HL_BusinessMember.Enabled = true;

            InitialModuleTree(TreeView2, "INNER", strLangCode);

            LoadWorkFlowTStepRelatedModule(strGUID, strLangCode, strUserCode);
        }

    }

    //生成角色组树
    public void InitialActorGroupTree(TreeView TreeView, String strUserCode, string strLangCode)
    {
        string strHQL;

        string strActorGroupName, strDepartString, strDepartCode;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = LB_DepartString.Text.Trim();

        //添加根节点
        TreeView.Nodes.Clear();

        TreeNode node0 = new TreeNode();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node0.Text = "<B>" + LanguageHandle.GetWord("ActorGroup").ToString().Trim() + "</B>";
        node0.Target = "0";
        node0.Expanded = true;
        TreeView.Nodes.Add(node0);

        strHQL = "Select GroupName From T_ActorGroup Where Type <>'Part'";
        strHQL += " and (BelongDepartCode in " + strDepartString + " Or Type = 'Super'";
        strHQL += " Or MakeUserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order by SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strActorGroupName = ds.Tables[0].Rows[i][0].ToString();

            node3 = new TreeNode();

            node3.Text = strActorGroupName;
            node3.Target = strActorGroupName;
            node3.Expanded = true;

            node0.ChildNodes.Add(node3);
        }

        node2 = new TreeNode();
        node2.Text = "<B>" + LanguageHandle.GetWord("BuFen").ToString().Trim() + "</B>";
        node2.Target = "1";
        node2.Expanded = false;
        node0.ChildNodes.Add(node2);

        strHQL = "Select GroupName From T_ActorGroup Where Type = 'Part'";
        strHQL += " and (BelongDepartCode in " + strDepartString + " Or Type = 'Super'";
        strHQL += " Or MakeUserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strActorGroupName = ds.Tables[0].Rows[i][0].ToString();

            node4 = new TreeNode();

            node4.Text = strActorGroupName;
            node4.Target = strActorGroupName;
            node4.Expanded = true;

            node2.ChildNodes.Add(node4);
        }

        TreeView.DataBind();
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strActorGroupName, strIdentifyString, strActorGroupMakeUserCode;
        string strDepartString, strGroupType, strBelongDepartCode, strMakeUserCode;

        strDepartString = LB_DepartString.Text.Trim();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0" & treeNode.Target != "1")
        {
            strActorGroupName = treeNode.Target.Trim();

            ActorGroup actorGroup = GetActorGroup(strActorGroupName);
            strGroupType = actorGroup.Type.Trim();
            strBelongDepartCode = actorGroup.BelongDepartCode.Trim();
            strIdentifyString = actorGroup.IdentifyString.Trim();
            strMakeUserCode = actorGroup.MakeUserCode.Trim();

            TB_ActorGroup.Text = strActorGroupName;
            TB_WLActorGroup.Text = strActorGroupName;
            HL_ActorGroup.Text = strActorGroupName;
            LB_ActorGroupType.Text = strGroupType;

            if ((strGroupType == "Super" & strDepartString.IndexOf(strBelongDepartCode) > -1) | strMakeUserCode == strUserCode)
            {
                HL_ActorGroup.NavigateUrl = "TTActorGroupDetail.aspx?IdentifyString=" + strIdentifyString;
            }
            else
            {
                HL_ActorGroup.NavigateUrl = "TTActorGroupMemberView.aspx?IdentifyString=" + strIdentifyString;
            }

            strActorGroupMakeUserCode = actorGroup.MakeUserCode.Trim();

            if (strUserCode != strActorGroupMakeUserCode | strActorGroupName == LanguageHandle.GetWord("GeRen").ToString().Trim() | strActorGroupName == LanguageHandle.GetWord("QuanTi").ToString().Trim() | strActorGroupName == LanguageHandle.GetWord("GongSi").ToString().Trim() | strActorGroupName == LanguageHandle.GetWord("JiTuan").ToString().Trim())
            {
                BT_WLDelete.Enabled = false;
            }
            else
            {
                BT_WLDelete.Enabled = true;
            }
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID, strHQL;
            IList lst;

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.ID = " + strID;
            WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
            lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
            WorkFlowTStepOperator workFlowTStepOperator = (WorkFlowTStepOperator)lst[0];

            LB_DetailID.Text = workFlowTStepOperator.ID.ToString();
            TB_ActorGroup.Text = workFlowTStepOperator.ActorGroup;
            DL_Requisite.SelectedValue = workFlowTStepOperator.Requisite.Trim();
            TB_WorkDetail.Text = workFlowTStepOperator.WorkDetail;
            DL_Actor.SelectedValue = workFlowTStepOperator.Actor.Trim();
            TB_FinishedTime.Amount = workFlowTStepOperator.LimitedTime;

            try
            {
                TB_FieldList.Text = workFlowTStepOperator.FieldList.Trim();
            }
            catch
            {
                TB_FieldList.Text = "";
            }

            DL_AllowFullEdit.SelectedValue = workFlowTStepOperator.AllowFullEdit.Trim();

            try
            {
                TB_EditFieldList.Text = workFlowTStepOperator.EditFieldList.Trim();
            }
            catch
            {
                TB_EditFieldList.Text = "";
            }

            try
            {
                TB_CanNotNullFieldList.Text = workFlowTStepOperator.CanNotNullFieldList.Trim();
            }
            catch
            {
                TB_CanNotNullFieldList.Text = "";
            }

            try
            {
                TB_SignPictureField.Text = workFlowTStepOperator.SignPictureField.Trim();
            }
            catch
            {
                TB_SignPictureField.Text = "";
            }

            if (Session["SuperWFAdmin"].ToString() == "YES" | strUserCode == strMakeUserCode)
            {
                BT_AddStepDetail.Enabled = true;
                BT_UpdateStepDetail.Enabled = true;
                BT_DeleteStepDetail.Enabled = true;
            }
            else
            {
                BT_AddStepDetail.Enabled = false;
                BT_UpdateStepDetail.Enabled = false;
                BT_DeleteStepDetail.Enabled = false;
            }
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strChildTemID = ((Button)e.Item.FindControl("BT_ID")).Text.ToString();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "From WFTStepRelatedTem as wfTStepRelatedTem Where ID = " + strChildTemID;
            WFTStepRelatedTemBLL wfTStepRelatedTemBLL = new WFTStepRelatedTemBLL();
            lst = wfTStepRelatedTemBLL.GetAllWFTStepRelatedTems(strHQL);

            WFTStepRelatedTem wfTStepRelatedTem = (WFTStepRelatedTem)lst[0];

            LB_ChildTemID.Text = wfTStepRelatedTem.ID.ToString();
            DL_RelatedWFTemplate.SelectedValue = wfTStepRelatedTem.RelatedWFTemName;
            DL_ChildTemRequisite.SelectedValue = wfTStepRelatedTem.Requisite.Trim();
            NB_BelongStepSortNumber.Amount = wfTStepRelatedTem.BelongStepSortNumber;
            DL_BelongIsPassed.SelectedValue = wfTStepRelatedTem.BelongIsMustPassed.Trim();

            WorkFlowTemplate workFlowTemplate = GetWorkFlowTemplate(wfTStepRelatedTem.RelatedWFTemName.Trim());
            string strWFDesignType = workFlowTemplate.DesignType.Trim();
            string strWFIdentifyString = workFlowTemplate.IdentifyString.Trim();

            HL_WorkFlowDesigner.NavigateUrl = "TTWFChartViewJS.aspx?TemName=" + wfTStepRelatedTem.RelatedWFTemName.Trim();
            HL_WorkFlowDesigner.Target = "_blank";
            HL_WorkFlowDesigner.Enabled = true;

            if (Session["SuperWFAdmin"].ToString() == "YES" | strMakeUserCode == strUserCode)
            {
                BT_AddChileTem.Enabled = true;
                BT_UpdateChileTem.Enabled = true;
                BT_DeleteChileTem.Enabled = true;
            }
            else
            {
                BT_AddChileTem.Enabled = false;
                BT_UpdateChileTem.Enabled = false;
                BT_DeleteChileTem.Enabled = false;
            }
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strActorGroup = ((Button)e.Item.FindControl("BT_ActorGroup")).Text.Trim();

            TB_ActorGroup.Text = strActorGroup;
            TB_WLActorGroup.Text = strActorGroup;
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

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SqlWL.Text;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DL_RelatedWFType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL, strWLType;
        string strDepartCode, strDepartString;

        strWLType = DL_RelatedWFType.SelectedValue.Trim();

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        strHQL = "Select TemName From T_WorkFlowTemplate Where Visible = 'YES' and type = " + "'" + strWLType + "'" + " and Authority = 'All'";
        strHQL += " and char_length(rtrim(ltrim(XSNFile)))>0 ";
        strHQL += " and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by CreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_RelatedWFTemplate.DataSource = ds;
        DL_RelatedWFTemplate.DataBind();

        DL_RelatedWFTemplate.Items.Insert(0, new ListItem("--Select--", ""));

        HL_WorkFlowDesigner.Enabled = false;
    }

    protected void DL_RelatedWFTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strTemName, strIdentifyString, strDesignType;

        strTemName = DL_RelatedWFTemplate.SelectedValue.Trim();

        if (strTemName == "")
        {
            HL_WorkFlowDesigner.Enabled = false;
            return;
        }
        else
        {
            HL_WorkFlowDesigner.Enabled = true;

            try
            {
                WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
                strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
                lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
                WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

                strIdentifyString = workFlowTemplate.IdentifyString.Trim();
                strDesignType = workFlowTemplate.DesignType.Trim();

                HL_WorkFlowDesigner.NavigateUrl = "TTWFChartViewJS.aspx?TemName=" + strTemName;
                HL_WorkFlowDesigner.Target = "_blank";
                HL_WorkFlowDesigner.Enabled = true;
            }
            catch
            {
                HL_WorkFlowDesigner.Enabled = false;
            }
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strStepName, strStepID;
        int intSortNumber, intLimitedOperator, intLimitedTime;
        string strHQL, strTemName, strSelfReview, strIsPriorStepSelect, strDepartRelated, strOperatorSelect, strPartTimeReview, strProjectRelated, strAllowSelfPass;
        string strAllowPriorOperatorPass, strNextStepMust;
        int intNextSortNumber, intDepartRelatedLelvelNumber;
        IList lst;

        strStepID = LB_StepID.Text.Trim();
        strTemName = LB_WFTemplate.Text.Trim();
        strStepName = TB_StepName.Text.Trim();
        intSortNumber = int.Parse(TB_SortNumber.Amount.ToString());
        intLimitedOperator = int.Parse(TB_LimitedOperator.Amount.ToString());
        intLimitedTime = int.Parse(TB_LimitedTime.Amount.ToString());
        intNextSortNumber = int.Parse(TB_NextSortNumber.Amount.ToString());

        strNextStepMust = DL_NextStepMust.SelectedValue.Trim();

        strSelfReview = DL_SelfReview.SelectedValue.Trim();
        strIsPriorStepSelect = DL_IsPriorStepSelect.SelectedValue.Trim();
        strAllowSelfPass = DL_AllowSelfPass.SelectedValue.Trim();
        strAllowPriorOperatorPass = DL_AllowPriorOperatorPass.SelectedValue.Trim();

        strDepartRelated = DL_DepartRelated.SelectedValue.Trim();
        intDepartRelatedLelvelNumber = int.Parse(NB_DepartRelatedLevelNumber.Amount.ToString());
        strOperatorSelect = DL_OperatorSelect.SelectedValue.Trim();
        strPartTimeReview = DL_PartTimeReview.SelectedValue.Trim();
        strProjectRelated = DL_ProjectRelated.SelectedValue.Trim();

        try
        {
            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID;
            lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

            WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

            workFlowTStep.SortNumber = intSortNumber;
            workFlowTStep.StepName = strStepName;
            workFlowTStep.LimitedTime = intLimitedTime;
            workFlowTStep.LimitedOperator = intLimitedOperator;
            workFlowTStep.NextSortNumber = intNextSortNumber;

            workFlowTStep.NextStepMust = strNextStepMust;

            workFlowTStep.SelfReview = strSelfReview;
            workFlowTStep.IsPriorStepSelect = strIsPriorStepSelect;

            workFlowTStep.AllowSelfPass = strAllowSelfPass;
            workFlowTStep.AllowPriorOperatorPass = strAllowPriorOperatorPass;

            workFlowTStep.DepartRelated = strDepartRelated;
            workFlowTStep.DepartRelatedLevelNumber = intDepartRelatedLelvelNumber;
            workFlowTStep.OperatorSelect = strOperatorSelect;
            workFlowTStep.PartTimeReview = strPartTimeReview;
            workFlowTStep.ProjectRelated = strProjectRelated;

            workFlowTStep.FinishPercent = NB_FinishPercent.Amount;

            workFlowTStep.AllowCurrentStepAddApprover = DL_AllowCurrentStepAddApprover.SelectedValue;
            workFlowTStep.AllowNextStepAddApprover = DL_AllowNextStepAddApprover.SelectedValue;

            workFlowTStep.OverTimeAutoAgree = DL_OverTimeAutoAgree.SelectedValue;
            workFlowTStep.OverTimeHourNumber = int.Parse(NB_OverTimeHourNumber.Amount.ToString());

            if (CB_SendSMS.Checked == true)
            {
                workFlowTStep.SendSMS = "YES";
            }
            else
            {
                workFlowTStep.SendSMS = "NO";
            }

            if (CB_SendEMail.Checked == true)
            {
                workFlowTStep.SendEMail = "YES";
            }
            else
            {
                workFlowTStep.SendEMail = "NO";
            }

            workFlowTStepBLL.UpdateWorkFlowTStep(workFlowTStep, int.Parse(strStepID));

            if (strDesignType != "JS")
            {
                SaveWFTemplateDefinationXML(strIdentifyString, "1");
            }

            LoadWorkFlowTStep(strStepID);
            LoadWFTStepRelatedTem(strStepID);
            LoadWorkFlowTStepOperator(strStepID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCCJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_AddChileTem_Click(object sender, EventArgs e)
    {
        string strChildTemID, strStepID, strTemName, strChildTemName, strIsMust, strIsPassed;
        int intStepSortNumber, intBelongSortNumber;

        strChildTemID = LB_ChildTemID.Text.Trim();
        strStepID = LB_StepID.Text.Trim();
        strTemName = LB_WFTemplate.Text.Trim();

        intStepSortNumber = int.Parse(TB_SortNumber.Amount.ToString());
        strChildTemName = DL_RelatedWFTemplate.SelectedValue.Trim();
        strIsMust = DL_ChildTemRequisite.SelectedValue.Trim();
        intBelongSortNumber = int.Parse(NB_BelongStepSortNumber.Amount.ToString());
        strIsPassed = DL_BelongIsPassed.SelectedValue.Trim();

        if (intBelongSortNumber != 0)
        {
            if (intBelongSortNumber < intStepSortNumber)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJSGSBXBNXYBBXJC").ToString().Trim() + "')", true);
                return;
            }
        }

        WFTStepRelatedTemBLL wfTStepRelatedTemBLL = new WFTStepRelatedTemBLL();
        WFTStepRelatedTem wfTStepRelatedTem = new WFTStepRelatedTem();

        wfTStepRelatedTem.RelatedStepID = int.Parse(strStepID);
        wfTStepRelatedTem.RelatedWFTemName = strChildTemName;
        wfTStepRelatedTem.Requisite = strIsMust;
        wfTStepRelatedTem.BelongStepSortNumber = intBelongSortNumber;
        wfTStepRelatedTem.BelongIsMustPassed = strIsPassed;

        try
        {
            wfTStepRelatedTemBLL.AddWFTStepRelatedTem(wfTStepRelatedTem);

            strChildTemID = ShareClass.GetMyCreatedMaxWFTStepRelatedTem(strStepID);

            LB_ChildTemID.Text = strChildTemID;

            LoadWFTStepRelatedTem(strStepID);

            BT_UpdateChileTem.Enabled = true;
            BT_DeleteChileTem.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UpdateChileTem_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strChildTemID, strStepID, strTemName, strChildTemName, strIsMust, strIsPassed;
        int intStepSortNumber, intBelongSortNumber;

        strChildTemID = LB_ChildTemID.Text.Trim();
        strStepID = LB_StepID.Text.Trim();
        strTemName = LB_WFTemplate.Text.Trim();

        intStepSortNumber = int.Parse(TB_SortNumber.Amount.ToString());
        strChildTemName = DL_RelatedWFTemplate.SelectedValue.Trim();
        strIsMust = DL_ChildTemRequisite.SelectedValue.Trim();
        intBelongSortNumber = int.Parse(NB_BelongStepSortNumber.Amount.ToString());
        strIsPassed = DL_BelongIsPassed.SelectedValue.Trim();

        if (intBelongSortNumber != 0)
        {
            if (intBelongSortNumber < intStepSortNumber)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJSGSBXBNXYBBXJC").ToString().Trim() + "')", true);
                return;
            }
        }

        strHQL = "From WFTStepRelatedTem as wfTStepRelatedTem  where wfTStepRelatedTem.ID = " + strChildTemID;
        WFTStepRelatedTemBLL wfTStepRelatedTemBLL = new WFTStepRelatedTemBLL();
        lst = wfTStepRelatedTemBLL.GetAllWFTStepRelatedTems(strHQL);
        WFTStepRelatedTem wfTStepRelatedTem = (WFTStepRelatedTem)lst[0];

        wfTStepRelatedTem.ID = int.Parse(strChildTemID);
        wfTStepRelatedTem.RelatedStepID = int.Parse(strStepID);
        wfTStepRelatedTem.RelatedWFTemName = strChildTemName;
        wfTStepRelatedTem.Requisite = strIsMust;
        wfTStepRelatedTem.BelongStepSortNumber = intBelongSortNumber;
        wfTStepRelatedTem.BelongIsMustPassed = strIsPassed;

        try
        {
            wfTStepRelatedTemBLL.UpdateWFTStepRelatedTem(wfTStepRelatedTem, int.Parse(strChildTemID));

            LoadWFTStepRelatedTem(strStepID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteChileTem_Click(object sender, EventArgs e)
    {
        string strHQL, strChildTemID, strStepID;

        strStepID = LB_StepID.Text.Trim();
        strChildTemID = LB_ChildTemID.Text.Trim();

        strHQL = "Delete From T_WFTStepRelatedTem Where ID = " + strChildTemID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWFTStepRelatedTem(strStepID);

            BT_UpdateChileTem.Enabled = false;
            BT_DeleteChileTem.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void LoadWFTStepRelatedTem(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "From WFTStepRelatedTem as wfTStepRelatedTem Where wfTStepRelatedTem.RelatedStepID = " + strStepID;
        WFStepRelatedWFBLL wfStepRelatedWFBLL = new WFStepRelatedWFBLL();
        lst = wfStepRelatedWFBLL.GetAllWFStepRelatedWFs(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void CreateOrLoadWorkFlowStep(string strGUID)
    {
        string strSendSMS, strSendEMail;

        string strHQL, strStepID;
        IList lst;

        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        WorkFlowTStep workFlowTStep = new WorkFlowTStep();

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.GUID = " + "'" + strGUID + "'";
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        if (lst.Count > 0)
        {
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            workFlowTStep = (WorkFlowTStep)lst[0];

            LB_ID.Text = workFlowTStep.StepID.ToString();
            LB_StepID.Text = workFlowTStep.StepID.ToString();
            LB_StepName.Text = workFlowTStep.StepName.Trim();
            TB_StepName.Text = workFlowTStep.StepName.Trim();
            LB_SortNumber.Text = workFlowTStep.SortNumber.ToString();
            TB_SortNumber.Amount = workFlowTStep.SortNumber;
            TB_LimitedOperator.Amount = workFlowTStep.LimitedOperator;
            TB_LimitedTime.Amount = workFlowTStep.LimitedTime;
            TB_NextSortNumber.Amount = workFlowTStep.NextSortNumber;

            DL_NextStepMust.SelectedValue = workFlowTStep.NextStepMust.Trim();

            DL_SelfReview.SelectedValue = workFlowTStep.SelfReview.Trim();
            DL_IsPriorStepSelect.SelectedValue = workFlowTStep.IsPriorStepSelect.Trim();

            DL_DepartRelated.SelectedValue = workFlowTStep.DepartRelated.Trim();
            NB_DepartRelatedLevelNumber.Amount = workFlowTStep.DepartRelatedLevelNumber;
            DL_OperatorSelect.SelectedValue = workFlowTStep.OperatorSelect.Trim();
            DL_PartTimeReview.SelectedValue = workFlowTStep.PartTimeReview.Trim();
            DL_ProjectRelated.SelectedValue = workFlowTStep.ProjectRelated.Trim();
            DL_AllowSelfPass.SelectedValue = workFlowTStep.AllowSelfPass.Trim();
            DL_AllowPriorOperatorPass.SelectedValue = workFlowTStep.AllowPriorOperatorPass.Trim();

            NB_FinishPercent.Amount = workFlowTStep.FinishPercent;

            try
            {
                DL_AllowCurrentStepAddApprover.SelectedValue = workFlowTStep.AllowCurrentStepAddApprover.Trim();
                DL_AllowNextStepAddApprover.SelectedValue = workFlowTStep.AllowNextStepAddApprover.Trim();
            }
            catch
            {
            }

            DL_OverTimeAutoAgree.SelectedValue = workFlowTStep.OverTimeAutoAgree.Trim();
            NB_OverTimeHourNumber.Amount = workFlowTStep.OverTimeHourNumber;

            strSendSMS = workFlowTStep.SendSMS.Trim();
            strSendEMail = workFlowTStep.SendEMail.Trim();

            if (strSendSMS == "YES")
            {
                CB_SendSMS.Checked = true;
            }
            else
            {
                CB_SendSMS.Checked = false;
            }

            if (strSendEMail == "YES")
            {
                CB_SendEMail.Checked = true;
            }
            else
            {
                CB_SendEMail.Checked = false;
            }

            strStepID = workFlowTStep.StepID.ToString();
            LB_StepID.Text = strStepID;
            NB_BelongStepSortNumber.Amount = workFlowTStep.SortNumber;

            LoadWFTStepRelatedTem(strStepID);
            LoadWorkFlowTStepOperator(strStepID);

            if (Session["SuperWFAdmin"].ToString() == "YES" | strMakeUserCode == strUserCode)
            {
                BT_Update.Enabled = true;
            }
            else
            {
                BT_Update.Enabled = false;
            }
        }
        else
        {
            workFlowTStep.SortNumber = 1;
            workFlowTStep.StepName = LanguageHandle.GetWord("BuZhouMingChen").ToString().Trim();
            workFlowTStep.NextSortNumber = 0;

            workFlowTStep.NextStepMust = "NO";

            workFlowTStep.LimitedTime = 1;
            workFlowTStep.LimitedOperator = 1;
            workFlowTStep.TemName = strTemName;

            workFlowTStep.SelfReview = "NO";
            workFlowTStep.IsPriorStepSelect = "NO";

            workFlowTStep.DepartRelated = "NO";
            workFlowTStep.DepartRelatedLevelNumber = 100;
            workFlowTStep.OperatorSelect = "NO";
            workFlowTStep.PartTimeReview = "NO";
            workFlowTStep.ProjectRelated = "NO";
            workFlowTStep.AllowSelfPass = "NO";
            workFlowTStep.AllowPriorOperatorPass = "NO";

            workFlowTStep.SendEMail = "YES";
            workFlowTStep.SendSMS = "YES";
            workFlowTStep.GUID = strGUID;

            workFlowTStep.FinishPercent = 0;

            workFlowTStep.AllowCurrentStepAddApprover = "NO";
            workFlowTStep.AllowNextStepAddApprover = "NO";

            workFlowTStep.OverTimeAutoAgree = "NO";
            workFlowTStep.OverTimeHourNumber = 24;

            try
            {
                if (Session["SuperWFAdmin"].ToString() == "YES" | strMakeUserCode == strUserCode)
                {
                    workFlowTStepBLL.AddWorkFlowTStep(workFlowTStep);

                    strStepID = ShareClass.GetMyCreatedWorkFlowTStepID(strTemName);
                    LB_StepID.Text = strStepID;
                    NB_BelongStepSortNumber.Amount = 1;

                    LB_StepID.Text = strStepID;
                    LB_ID.Text = strStepID;
                    LB_SortNumber.Text = "1";
                    LB_StepName.Text = LanguageHandle.GetWord("BuZhou").ToString().Trim();

                    BT_Update.Enabled = true;

                    LoadWFTStepRelatedTem(strStepID);
                    LoadWorkFlowTStepOperator(strStepID);

                    CreateOrLoadWorkFlowStep(strGUID);
                }
            }
            catch
            {
            }
        }

        LoadWorkFlowTStepRelatedModule(strGUID, strLangCode, strUserCode);
    }

    protected void DeleteWorkFlowTStep(string strGUID)
    {
        string strHQL, strStepID;
        IList lst;

        try
        {
            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.GUID = " + "'" + strGUID + "'";
            lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

            if (lst.Count > 0)
            {
                WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

                strStepID = workFlowTStep.StepID.ToString();
                LB_StepID.Text = strStepID;

                workFlowTStepBLL.DeleteWorkFlowTStep(workFlowTStep);

                ShareClass.RunSqlCommand("Delete From T_WorkFlowTStepOperator Where StepID = " + strStepID);

                BT_Update.Enabled = false;

                LoadWorkFlowTStep(strStepID);
                LoadWorkFlowTStepOperator(strStepID);

                LB_SqlWL.Text = strHQL;
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_AddStepDetail_Click(object sender, EventArgs e)
    {
        string strStepID, strTemName, strActorGroup, strActor, strAllowFullEdit;
        string strWorkDetail, strRequisiste, strFieldList, strEditFieldList, strCanNotNullFieldList, strSignPictureField, strRelatedDepart;
        string strIdentifyString;
        int intLimitedTime;

        WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
        WorkFlowTStepOperator workFlowTStepOperator = new WorkFlowTStepOperator();

        strStepID = LB_StepID.Text.Trim();
        strTemName = LB_WFTemplate.Text.Trim();
        strActorGroup = TB_ActorGroup.Text.Trim();
        strWorkDetail = TB_WorkDetail.Text;
        strActor = DL_Actor.SelectedValue.Trim();
        strRequisiste = DL_Requisite.SelectedValue.Trim();
        intLimitedTime = int.Parse(TB_FinishedTime.Amount.ToString());
        strFieldList = TB_FieldList.Text.Trim();
        strAllowFullEdit = DL_AllowFullEdit.SelectedValue.Trim();
        strEditFieldList = TB_EditFieldList.Text.Trim();
        strCanNotNullFieldList = TB_CanNotNullFieldList.Text.Trim();
        strSignPictureField = TB_SignPictureField.Text.Trim();

        strRelatedDepart = DL_DepartRelated.SelectedValue.Trim();
        strIdentifyString = GetActorGroupIdentityString(strActorGroup);

        if (strActorGroup != "" | strWorkDetail != "")
        {
            workFlowTStepOperator.StepID = int.Parse(strStepID);
            workFlowTStepOperator.TemName = strTemName;
            workFlowTStepOperator.ActorGroup = strActorGroup;
            workFlowTStepOperator.WorkDetail = strWorkDetail;
            workFlowTStepOperator.Actor = strActor;
            workFlowTStepOperator.Requisite = strRequisiste;
            workFlowTStepOperator.LimitedTime = intLimitedTime;
            workFlowTStepOperator.FieldList = strFieldList;
            workFlowTStepOperator.AllowFullEdit = strAllowFullEdit;
            workFlowTStepOperator.EditFieldList = strEditFieldList;
            workFlowTStepOperator.CanNotNullFieldList = strCanNotNullFieldList;

            workFlowTStepOperator.SignPictureField = strSignPictureField;

            workFlowTStepOperator.IdentifyString = strIdentifyString;

            try
            {
                workFlowTStepOperatorBLL.AddWorkFlowTStepOperator(workFlowTStepOperator);
                LB_DetailID.Text = ShareClass.GetMyCreatedMaxWorkFlowTStepOperatorID(strStepID);

                BT_UpdateStepDetail.Enabled = true;
                BT_DeleteStepDetail.Enabled = true;

                LoadWorkFlowTStepOperator(strStepID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCYZHGZNRBNWKJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UpdateStepDetail_Click(object sender, EventArgs e)
    {
        string strID, strStepID, strTemName, strHQL, strActorGroup, strIdentifyString, strAllowFullEdit;
        string strActor, strWorkDetail, strRequisite, strFieldList, strEditFieldList, strCanNotNullFieldList, strSignPictureField, strRelatedDepart;
        int intFinishedTime;
        IList lst;

        strStepID = LB_StepID.Text.Trim();
        strTemName = LB_WFTemplate.Text.Trim();
        strID = LB_DetailID.Text.Trim();
        strActorGroup = TB_ActorGroup.Text.Trim();
        strRequisite = DL_Requisite.SelectedValue.Trim();
        strWorkDetail = TB_WorkDetail.Text.Trim();
        strActor = DL_Actor.SelectedValue.Trim();
        intFinishedTime = int.Parse(TB_FinishedTime.Amount.ToString());
        strFieldList = TB_FieldList.Text.Trim();
        strAllowFullEdit = DL_AllowFullEdit.SelectedValue.Trim();
        strEditFieldList = TB_EditFieldList.Text.Trim();
        strCanNotNullFieldList = TB_CanNotNullFieldList.Text.Trim();
        strRelatedDepart = DL_DepartRelated.SelectedValue.Trim();
        strIdentifyString = GetActorGroupIdentityString(strActorGroup);
        strSignPictureField = TB_SignPictureField.Text.Trim();

        if (strActorGroup != "" | strWorkDetail != "")
        {
            strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.ID = " + strID;
            WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
            lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
            WorkFlowTStepOperator workFlowTStepOperator = (WorkFlowTStepOperator)lst[0];

            workFlowTStepOperator.ActorGroup = strActorGroup;
            workFlowTStepOperator.Requisite = strRequisite;
            workFlowTStepOperator.WorkDetail = strWorkDetail;
            workFlowTStepOperator.Actor = strActor;
            workFlowTStepOperator.LimitedTime = intFinishedTime;
            workFlowTStepOperator.FieldList = strFieldList;
            workFlowTStepOperator.AllowFullEdit = strAllowFullEdit;
            workFlowTStepOperator.EditFieldList = strEditFieldList;
            workFlowTStepOperator.CanNotNullFieldList = strCanNotNullFieldList;
            workFlowTStepOperator.SignPictureField = strSignPictureField;

            workFlowTStepOperator.IdentifyString = strIdentifyString;

            workFlowTStepOperatorBLL.UpdateWorkFlowTStepOperator(workFlowTStepOperator, int.Parse(strID));
            LoadWorkFlowTStepOperator(strStepID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCYZHGZNRBNWKJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteStepDetail_Click(object sender, EventArgs e)
    {
        int intID;
        string strStepID;

        strStepID = LB_StepID.Text.Trim();

        intID = int.Parse(LB_DetailID.Text.Trim());

        WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
        WorkFlowTStepOperator workFlowTStepOperator = new WorkFlowTStepOperator();

        workFlowTStepOperator.ID = intID;

        try
        {
            workFlowTStepOperatorBLL.DeleteWorkFlowTStepOperator(workFlowTStepOperator);

            BT_UpdateStepDetail.Enabled = false;
            BT_DeleteStepDetail.Enabled = false;

            LoadWorkFlowTStepOperator(strStepID);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_WLNew_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strGroupName, strUserCode, strIdentifyString, strDepartCode, strDepartName;
        string strType;

        strGroupName = TB_WLActorGroup.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strType = "All";

        strIdentifyString = DateTime.Now.ToString("yyyyMMddHHMMssff");

        if (strGroupName != "")
        {
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            ActorGroup actorGroup = new ActorGroup();

            strHQL = "from ActorGroup as actorGroup Where actorGroup.GroupName = " + "'" + strGroupName + "'";
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            if (lst.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMDJSZQJC").ToString().Trim() + "')", true);
                return;
            }

            actorGroup.GroupName = strGroupName;
            actorGroup.MakeUserCode = strUserCode;
            actorGroup.SortNumber = lst.Count + 1;
            actorGroup.Type = strType;
            actorGroup.IdentifyString = strIdentifyString;
            actorGroup.BelongDepartCode = strDepartCode;
            actorGroup.BelongDepartName = strDepartName;
            actorGroup.LangCode = strLangCode;
            actorGroup.HomeName = strGroupName;
            actorGroup.MakeType = "DIY";

            try
            {
                actorGroupBLL.AddActorGroup(actorGroup);
                InitialActorGroupTree(TreeView1, strUserCode, strLangCode);

                HL_ActorGroup.Text = strGroupName;
                HL_ActorGroup.NavigateUrl = "TTActorGroupDetail.aspx?IdentifyString=" + strIdentifyString;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSZZMBNWK").ToString().Trim() + "')", true);
        }
    }

    protected void BT_WLDelete_Click(object sender, EventArgs e)
    {
        string strHQL, strActorGroup;
        IList lst;

        strActorGroup = TB_WLActorGroup.Text.Trim();

        strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.ActorGroup = " + "'" + strActorGroup + "'";
        WorkFlowTStepOperatorBLL workFlowTStepPoeratorBLL = new WorkFlowTStepOperatorBLL();
        lst = workFlowTStepPoeratorBLL.GetAllWorkFlowTStepOperators(strHQL);

        if (lst.Count == 0)
        {
            strActorGroup = TB_WLActorGroup.Text.Trim();

            strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName =  " + "'" + strActorGroup + "'";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            ActorGroup actorGroup = (ActorGroup)lst[0];

            try
            {
                actorGroupBLL.DeleteActorGroup(actorGroup);
                InitialActorGroupTree(TreeView1, strUserCode, strLangCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBNSCYYGZLSYCJSZ").ToString().Trim() + "')", true);
        }
    }

    protected void LoadWorkFlowTStep(string strStepID)
    {
        string strHQL;
        IList lst;

        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + "'" + strStepID + "'";
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void SaveWFTemplateDefinationXML(string strIdentifyString, string strMark)
    {
        string strHQL;
        string strXML;

        strXML = TB_WFXML.Text.Trim();

        try
        {
            if (strXML != "")
            {
                strHQL = "update T_WorkFlowTemplate Set WFDefinition = " + "'" + strXML + "'" + " Where IdentifyString = " + "'" + strIdentifyString + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            if (strMark == "0")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZLMBDYBCCG").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void LoadRelatedWFType(string strUserCode)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        strHQL = string.Format(@"Select Type,HomeName From T_WLType Where Type In (Select distinct Type From T_WorkFlowTemplate Where Authority = 'All'
                and char_length(rtrim(ltrim(XSNFile)))>0
                and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode('{0}'))
                Or BelongDepartCode in {1})) and LangCode ='{2}'", strDepartCode,strDepartString,strLangCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_RelatedWFType.DataSource = ds;
        DL_RelatedWFType.DataBind();
    }

    protected void LoadRelatedWorkFlowTemplate(string strUserCode)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        strHQL = "Select distinct Type,TemName,CreateTime From T_WorkFlowTemplate Where Visible = 'YES' and Authority = 'All'";
        strHQL += " and char_length(rtrim(ltrim(XSNFile)))>0 ";
        strHQL += " and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by CreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_RelatedWFTemplate.DataSource = ds;
        DL_RelatedWFTemplate.DataBind();

        DL_RelatedWFTemplate.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected WorkFlowTemplate GetWorkFlowTemplate(string strTemName)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate;
    }

    protected string GetMakeUserCode(string strTemName)
    {
        IList lst;
        string strHQL, strMakeUserCode;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strMakeUserCode = workFlowTemplate.CreatorCode.Trim();

        return strMakeUserCode;
    }

    protected string GetWorkFlowTemplateIdentifyString(string strTemName)
    {
        IList lst;
        string strHQL, strIdentifyString;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strIdentifyString = workFlowTemplate.IdentifyString.Trim();

        return strIdentifyString;
    }

    protected string GetWorkFlowTemName(string strIdentifyString)
    {
        IList lst;
        string strHQL, strTemName;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.IdentifyString =" + "'" + strIdentifyString + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];
        strTemName = workFlowTemplate.TemName.Trim();

        return strTemName;
    }

    protected string GetWorkTemplateXSNFile(string strTemName)
    {
        IList lst;
        string strHQL, strXSNFile;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strXSNFile = workFlowTemplate.XSNFile.Trim();
        }
        catch
        {
            strXSNFile = "";
        }

        return strXSNFile;
    }

    protected void LoadWorkFlowTStepOperator(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.StepID = " + strStepID;
        WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();

        lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected string GetActorGroupMakeUserCode(string strActorGroup)
    {
        string strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName = " + "'" + strActorGroup + "'";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        IList lst = actorGroupBLL.GetAllActorGroups(strHQL);

        ActorGroup actorGroup = (ActorGroup)lst[0];

        return actorGroup.MakeUserCode.Trim();
    }

    protected void DL_ForUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strUserType;

        strUserType = DL_ForUserType.SelectedValue.Trim();

        InitialModuleTree(TreeView2, strUserType, strLangCode);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strModuleID, strUserType, strModuleType, strModuleName, strPageName;
        int intLevel;

        strUserType = DL_ForUserType.SelectedValue.Trim();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        intLevel = treeNode.Depth;

        strModuleID = treeNode.Target.Trim();

        if (strModuleID == "0")
        {
        }
        else
        {
            strHQL = "Select ID,ParentModule,ModuleName,PageName,SortNumber,ModuleType,Visible,HomeModuleName,IconURL,UserType From T_ProModuleLevel Where ID = " + strModuleID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");
            strModuleName = ds.Tables[0].Rows[0]["ModuleName"].ToString().Trim();
            strPageName = ds.Tables[0].Rows[0]["PageName"].ToString().Trim();
            strModuleType = ds.Tables[0].Rows[0]["ModuleType"].ToString().Trim();

            if (strPageName.IndexOf("?") >= 0)
            {
                strPageName = strPageName + "&ModuleName=" + strModuleName + "&ModuleType=" + strModuleType;
            }
            else
            {
                strPageName = strPageName + "?ModuleName=" + strModuleName + "&ModuleType=" + strModuleType;
            }

            if (!CheckModuleIsExistInThisWFStep(strGUID, strModuleName))
            {
                strHQL = "Insert Into T_WorkFlowTStepRelatedModule(StepGUID,TemName,ModuleName,PageName,ModuleType,MainTableCanAdd,DetailTableCanAdd,MainTableCanEdit,MainTableCanDelete,DetailTableCanEdit,DetailTableCanDelete)";
                strHQL += " values('" + strGUID + "','" + strTemName + "','" + strModuleName + "','" + strPageName + "','" + strModuleType + "','YES','YES','YES','YES','YES','YES')";

                ShareClass.RunSqlCommand(strHQL);
            }

            LoadWorkFlowTStepRelatedModule(strGUID, strLangCode, strUserCode);
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }



    protected void RP_RelatedModule_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strModuleName = ((Button)e.Item.FindControl("BT_ModuleName")).Text.Trim();

            for (int i = 0; i < RP_RelatedModule.Items.Count; i++)
            {
                ((Button)RP_RelatedModule.Items[i].FindControl("BT_HomeModuleName")).ForeColor = Color.Black;
            }
            ((Button)e.Item.FindControl("BT_HomeModuleName")).ForeColor = Color.Red;


            if (e.CommandName == "Delete")
            {
                strHQL = "Delete From T_WorkFlowTStepRelatedModule Where StepGUID = " + "'" + strGUID + "'" + " And ModuleName = " + "'" + strModuleName + "'";
                ShareClass.RunSqlCommand(strHQL);

                LoadWorkFlowTStepRelatedModule(strGUID, strLangCode, strUserCode);
            }

            if (e.CommandName == "Save")
            {
                string strMainTableCanAdd, strDetailTableCanAdd, strMainTableCanEdit, strMainTableCanDelete, strDetailTableCanEdit, strDetailTableCanDelete;

                if (((CheckBox)e.Item.FindControl("CB_MainTableCanAdd")).Checked)
                {
                    strMainTableCanAdd = "YES";
                }
                else
                {
                    strMainTableCanAdd = "NO";
                }
                if (((CheckBox)e.Item.FindControl("CB_DetailTableCanAdd")).Checked)
                {
                    strDetailTableCanAdd = "YES";
                }
                else
                {
                    strDetailTableCanAdd = "NO";
                }

                if (((CheckBox)e.Item.FindControl("CB_MainTableCanEdit")).Checked)
                {
                    strMainTableCanEdit = "YES";
                }
                else
                {
                    strMainTableCanEdit = "NO";
                }
                if (((CheckBox)e.Item.FindControl("CB_MainTableCanDelete")).Checked)
                {
                    strMainTableCanDelete = "YES";
                }
                else
                {
                    strMainTableCanDelete = "NO";
                }
                if (((CheckBox)e.Item.FindControl("CB_DetailTableCanEdit")).Checked)
                {
                    strDetailTableCanEdit = "YES";
                }
                else
                {
                    strDetailTableCanEdit = "NO";
                }
                if (((CheckBox)e.Item.FindControl("CB_DetailTableCanDelete")).Checked)
                {
                    strDetailTableCanDelete = "YES";
                }
                else
                {
                    strDetailTableCanDelete = "NO";
                }

                LB_WFTemplate.Text = "kdfkdfk";


                try
                {
                    strHQL = "Update T_WorkFlowTStepRelatedModule Set MainTableCanAdd = '" + strMainTableCanAdd + "',DetailTableCanAdd = '" + strDetailTableCanAdd + "',MainTableCanEdit = '" + strMainTableCanEdit + "',MainTableCanDelete = '" + strMainTableCanDelete + "',DetailTableCanEdit='" + strDetailTableCanEdit + "',DetailTableCanDelete='" + strDetailTableCanDelete + "'";
                    strHQL += " Where StepGUID = " + "'" + strGUID + "'" + " And ModuleName = " + "'" + strModuleName + "'";

                    ShareClass.RunSqlCommand(strHQL);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
                }

            }
        }
    }


    protected void LoadWorkFlowTStepRelatedModule(string strStepGUID, string strLangCode, string strUserCode)
    {
        string strHQL;
        int i = 0;

        strHQL = string.Format(@"Select distinct B.HomeModuleName, B.PageName, A.ModuleName, A.MainTableCanAdd, A.DetailTableCanAdd, A.MainTableCanEdit,
                    A.MainTableCanDelete, A.DetailTableCanEdit, A.DetailTableCanDelete From T_WorkFlowTStepRelatedModule A, T_ProModuleLevel B
                     Where A.StepGUID = '{0}' and A.ModuleName = B.ModuleName and B.LangCode = '{1}'
                     And A.ModuleName in (Select ModuleName From T_ProModule Where Visible = 'YES' and IsDeleted = 'NO' and UserCode = '{2}')
                      and B.ID In(Select min(B.ID) From T_WorkFlowTStepRelatedModule A, T_ProModuleLevel B
                     Where A.StepGUID = '{0}' and A.ModuleName = B.ModuleName and B.LangCode = '{1}'
                     And A.ModuleName in (Select ModuleName From T_ProModule Where Visible = 'YES' and IsDeleted = 'NO' and UserCode = '{2}')
                       Group By B.ModuleName,B.PageName)", strStepGUID, strLangCode, strUserCode);


        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStepRelatedModule");
        RP_RelatedModule.DataSource = ds;
        RP_RelatedModule.DataBind();

        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (ds.Tables[0].Rows[i]["MainTableCanAdd"].ToString().Trim() == "YES")
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_MainTableCanAdd")).Checked = true;
            }
            else
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_MainTableCanAdd")).Checked = false;
            }

            if (ds.Tables[0].Rows[i]["DetailTableCanAdd"].ToString().Trim() == "YES")
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_DetailTableCanAdd")).Checked = true;
            }
            else
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_DetailTableCanAdd")).Checked = false;
            }

            if (ds.Tables[0].Rows[i]["MainTableCanEdit"].ToString().Trim() == "YES")
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_MainTableCanEdit")).Checked = true;
            }
            else
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_MainTableCanEdit")).Checked = false;
            }

            if (ds.Tables[0].Rows[i]["MainTableCanDelete"].ToString().Trim() == "YES")
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_MainTableCanDelete")).Checked = true;
            }
            else
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_MainTableCanDelete")).Checked = false;
            }

            if (ds.Tables[0].Rows[i]["DetailTableCanEdit"].ToString().Trim() == "YES")
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_DetailTableCanEdit")).Checked = true;
            }
            else
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_DetailTableCanEdit")).Checked = false;
            }

            if (ds.Tables[0].Rows[i]["DetailTableCanDelete"].ToString().Trim() == "YES")
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_DetailTableCanDelete")).Checked = true;
            }
            else
            {
                ((CheckBox)RP_RelatedModule.Items[i].FindControl("CB_DetailTableCanDelete")).Checked = false;
            }

        }
    }

    //检查模组是否存在
    protected bool CheckModuleIsExistInThisWFStep(string strStepGUID, string strModuleName)
    {
        string strHQL;

        strHQL = "Select * From T_WorkFlowTStepRelatedModule Where StepGUID = '" + strGUID + "' and ModuleName = '" + strModuleName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void InitialModuleTree(TreeView treeView, string strUserType, string strLangCode)
    {
        string strHQL;

        string strModuleID, strModuleName, strModuleType, strHomeModuleName;

        string strForbitModule = Session["ForbitModule"].ToString();

        //添加根节点
        treeView.Nodes.Clear();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();

        node1.Text = "<b>" + LanguageHandle.GetWord("XiTongMoZu").ToString().Trim() + "<b>";
        node1.Target = "0";
        node1.Expanded = true;
        treeView.Nodes.Add(node1);

        strHQL = "Select ID,ModuleName,HomeModuleName,ModuleType From T_ProModuleLevel Where UserType = " + "'" + strUserType + "'" + " and char_length(ParentModule) = 0 ";
        strHQL += " and position(rtrim(ModuleName)|| ',' in '" + strForbitModule + "') = 0";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " and IsDeleted = 'NO'";
        strHQL += " Order By ModuleType DESC,SortNumber ASC";
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strModuleID = ds1.Tables[0].Rows[i][0].ToString();
            strModuleName = ds1.Tables[0].Rows[i][1].ToString();
            strHomeModuleName = ds1.Tables[0].Rows[i][2].ToString();
            strModuleType = ds1.Tables[0].Rows[i][3].ToString();

            if (strForbitModule.IndexOf(strModuleName + ",") >= 0)
            {
                continue;
            }

            node2 = new TreeNode();

            node2.Text = (i + 1).ToString() + "." + strHomeModuleName;
            node2.Target = strModuleID;
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);

            ModuleTreeShow(strUserType, strModuleType, strModuleName, node2, strLangCode);

            treeView.DataBind();
        }
    }

    public void ModuleTreeShow(string strUserType, string strModuleType, string strParentModule, TreeNode treeNode, string strLangCode)
    {
        string strHQL;

        string strModuleID, strModuleName, strHomeModuleName;
        string strForbitModule = Session["ForbitModule"].ToString();

        TreeNode node1 = new TreeNode();

        strHQL = "Select ID,ModuleName,HomeModuleName From T_ProModuleLevel Where UserType = " + "'" + strUserType + "'" + " and ParentModule = " + "'" + strParentModule + "'";
        strHQL += " and position(rtrim(ModuleName)||',' in '" + strForbitModule + "') = 0";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " and IsDeleted = 'NO'";
        strHQL += " Order By SortNumber ASC";
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strModuleID = ds1.Tables[0].Rows[i][0].ToString();
            strModuleName = ds1.Tables[0].Rows[i][1].ToString();
            strHomeModuleName = ds1.Tables[0].Rows[i][2].ToString();

            if (strForbitModule.IndexOf(strModuleName + ",") >= 0)
            {
                continue;
            }

            node1 = new TreeNode();
            node1.Text = (i + 1).ToString() + "." + strHomeModuleName;
            node1.Target = strModuleID;

            node1.Expanded = false;

            treeNode.ChildNodes.Add(node1);

            ModuleTreeShow(strUserType, strModuleType, strModuleName, node1, strLangCode);
        }
    }

    protected string GetActorGroupIdentityString(string strActorGroup)
    {
        string strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName = " + "'" + strActorGroup + "'";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        IList lst = actorGroupBLL.GetAllActorGroups(strHQL);

        if (lst.Count > 0)
        {
            ActorGroup actorGroup = (ActorGroup)lst[0];

            return actorGroup.IdentifyString.Trim();
        }
        else
        {
            return "";
        }
    }

    protected ActorGroup GetActorGroup(string strGroupName)
    {
        string strHQL;
        IList lst;

        strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName = " + "'" + strGroupName + "'";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        if (lst.Count > 0)
        {
            ActorGroup actorGroup = (ActorGroup)lst[0];

            return actorGroup;
        }
        else
        {
            return null;
        }
    }

    protected string GetActorGroupType(string strGroupName)
    {
        string strHQL;
        IList lst;

        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        ActorGroup actorGroup = new ActorGroup();
        strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName = " + "'" + strGroupName + "'";
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        if (lst.Count > 0)
        {
            actorGroup = (ActorGroup)lst[0];

            return actorGroup.Type.Trim();
        }
        else
        {
            return "";
        }
    }

    protected string getWFTemsStepID(string strGUID)
    {
        string strHQL;
        IList lst;

        string strStepID = "";

        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.GUID = " + "'" + strGUID + "'";
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        if (lst.Count > 0)
        {
            WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

            strStepID = workFlowTStep.StepID.ToString();
        }

        return strStepID;
    }

}