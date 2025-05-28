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

public partial class TTCreateDefectDetail : System.Web.UI.Page
{
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strUserName;
        string strDepartCode;
        string strDefectID;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor��ʼ��
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_Operation);
HE_Operation.Language = Session["LangCode"].ToString();

        strDefectID = Request.QueryString["DefectID"];

        //this.Title = "�����ͷ�������";

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

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
            DLC_DefectFinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from DefectType as defectType Order by defectType.SortNumber ASC";
            DefectTypeBLL defectTypeBLL = new DefectTypeBLL();
            lst = defectTypeBLL.GetAllDefectTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);

            LoadDefectment(strDefectID, strUserCode);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        TreeView1.SelectedNode.Selected = false;
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
        }
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strApplicantName;
        string strDefectID,strDefectType, strDefectName, strDefectDetail, strAcceptStandard;
        string strDefectFinishedDate, strApplicantCode, strStatus;
        DateTime dtMakeDate;


        strDefectType = DL_Type.SelectedValue;
        strDefectName = TB_DefectName.Text.Trim();
        strDefectDetail = TB_DefectDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strDefectFinishedDate = DLC_DefectFinishedDate.Text;
        strApplicantCode = strUserCode;
        strApplicantName = ShareClass.GetUserName(strUserCode);
        dtMakeDate = DateTime.Now;
        strStatus = "Plan";

        if (strDefectName == "" | strDefectDetail == "" | strDefectFinishedDate == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            Defectment defectment = new Defectment();
            DefectmentBLL defectmentBLL = new DefectmentBLL();

            defectment.DefectType = strDefectType;
            defectment.DefectName = strDefectName;
            defectment.DefectDetail = strDefectDetail;
            defectment.AcceptStandard = strAcceptStandard;
            defectment.DefectFinishedDate = DateTime.Parse(strDefectFinishedDate);
            defectment.MakeDate = dtMakeDate;
            defectment.ApplicantCode = strApplicantCode;
            defectment.ApplicantName = strApplicantName;
            defectment.Status = strStatus;

            try
            {
                defectmentBLL.AddDefectment(defectment);

                strDefectID = ShareClass.GetMyCreatedMaxDefectID(strUserCode);
                LB_DefectID.Text = strDefectID;

                //����ȱ�޸��Լ�
                AssignDefect(int.Parse(strDefectID), strDefectType, strDefectName);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                BT_Assign.Enabled = false;

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            }
        }
    }

    //�Զ�����ȱ�ݸ�������
    protected void AssignDefect(int intDefectID, string strType, string strDefectName)
    {
        int intPriorID;
        string strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;

        DateTime dtBeginDate, dtEndDate, dtMakeDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        strOperatorCode = LB_UserCode.Text.Trim();
        strOperatorName = LB_UserName.Text.Trim();

        strAssignManCode = LB_UserCode.Text.Trim();
        strAssignManName = LB_UserName.Text.Trim();

        intPriorID = 0;
        dtBeginDate = DateTime.Parse(DLC_BeginDate.Text);
        dtEndDate = DateTime.Parse(DLC_EndDate.Text);
        dtMakeDate = DateTime.Now;

        DefectAssignRecordBLL defectAssignRecordBLL = new DefectAssignRecordBLL();
        DefectAssignRecord defectAssignRecord = new DefectAssignRecord();

        defectAssignRecord.DefectID = intDefectID;
        defectAssignRecord.Type = strType;
        defectAssignRecord.DefectName = strDefectName;
        defectAssignRecord.OperatorCode = strOperatorCode;
        defectAssignRecord.OperatorName = strOperatorName;
        defectAssignRecord.OperatorContent = "";
        defectAssignRecord.OperationTime = DateTime.Now;
        defectAssignRecord.BeginDate = dtBeginDate;
        defectAssignRecord.EndDate = dtEndDate;
        defectAssignRecord.AssignManCode = strAssignManCode;
        defectAssignRecord.AssignManName = strAssignManName;
        defectAssignRecord.Content = "";
        defectAssignRecord.Operation = strDefectName;
        defectAssignRecord.PriorID = intPriorID;
        defectAssignRecord.RouteNumber = GetRouteNumber(intDefectID.ToString());
        defectAssignRecord.MakeDate = dtMakeDate;
        defectAssignRecord.Status = "ToHandle";
        defectAssignRecord.MoveTime = DateTime.Now;

        try
        {
            defectAssignRecordBLL.AddDefectAssignRecord(defectAssignRecord);

            string strAssignID = ShareClass.GetMyCreatedMaxDefectAssignRecordID(intDefectID.ToString(), strUserCode);
            //BusinessForm,���������ҵ�������
            ShareClass.InsertOrUpdateTaskAssignRecordWFXMLData("Defect", intDefectID.ToString(), "DefectRecord", strAssignID, strUserCode);

            UpdateDefectStatus(intDefectID.ToString(), "InProgress");
        }
        catch
        {
        }
    }


    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strDefectType, strDefectName, strDefectDetail, strAcceptStandard;
        string strDefectFinishedDate, strApplicantCode, strStatus;
        DateTime dtMakeDate;

        string strDefectID = LB_DefectID.Text.Trim();

        strDefectType = DL_Type.SelectedValue;
        strDefectName = TB_DefectName.Text.Trim();
        strDefectDetail = TB_DefectDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strDefectFinishedDate = DLC_DefectFinishedDate.Text;
        strApplicantCode = strUserCode;
        dtMakeDate = DateTime.Now;
        strStatus = LB_Status.Text.Trim();

        if (strDefectID != "")
        {
            if (strDefectName == "" | strDefectDetail == "" | strDefectFinishedDate == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);
            }
            else
            {
                Defectment defectment = new Defectment();
                DefectmentBLL defectmentBLL = new DefectmentBLL();

                defectment.DefectType = strDefectType;
                defectment.DefectName = strDefectName;
                defectment.DefectDetail = strDefectDetail;
                defectment.AcceptStandard = strAcceptStandard;
                defectment.DefectFinishedDate = DateTime.Parse(strDefectFinishedDate);
                defectment.MakeDate = dtMakeDate;
                defectment.ApplicantCode = strApplicantCode;
                defectment.ApplicantName = ShareClass.GetUserName(strApplicantCode);
                defectment.Status = strStatus;

                try
                {
                    defectmentBLL.UpdateDefectment(defectment, int.Parse(strDefectID));

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
        string strDefectType, strDefectName, strDefectDetail, strAcceptStandard;
        string strDefectFinishedDate, strReceiverCode, strApplicantCode, strStatus;
        DateTime dtMakeDate;

        string strDefectID = LB_DefectID.Text.Trim();

        strDefectType = DL_Type.SelectedValue;
        strDefectName = TB_DefectName.Text.Trim();
        strDefectDetail = TB_DefectDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strDefectFinishedDate = DLC_DefectFinishedDate.Text;
        strReceiverCode = TB_ReceiverCode.Text.Trim();
        strApplicantCode = strUserCode;
        dtMakeDate = DateTime.Now;
        strStatus = LB_Status.Text.Trim();

        if (strDefectID != "")
        {
            if (strDefectName == "" | strDefectDetail == "" | strDefectFinishedDate == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);
            }
            else
            {

                Defectment defectment = new Defectment();
                DefectmentBLL defectmentBLL = new DefectmentBLL();

                defectment.DefectType = strDefectType;
                defectment.DefectName = strDefectName;
                defectment.DefectDetail = strDefectDetail;
                defectment.AcceptStandard = strAcceptStandard;
                defectment.DefectFinishedDate = DateTime.Parse(strDefectFinishedDate);
                defectment.MakeDate = dtMakeDate;
                defectment.ApplicantCode = strApplicantCode;
                defectment.ApplicantName = ShareClass.GetUserName(strApplicantCode);
                defectment.Status = "Closed";

                try
                {
                    defectmentBLL.UpdateDefectment(defectment, int.Parse(strDefectID));

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
        string strDefectType, strDefectName, strDefectDetail, strAcceptStandard;
        string strDefectFinishedDate, strReceiverCode, strApplicantCode, strStatus;
        DateTime dtMakeDate;

        string strDefectID = LB_DefectID.Text.Trim();

        strDefectType = DL_Type.SelectedValue;
        strDefectName = TB_DefectName.Text.Trim();
        strDefectDetail = TB_DefectDetail.Text.Trim();
        strAcceptStandard = TB_AcceptStandard.Text.Trim();
        strDefectFinishedDate = DLC_DefectFinishedDate.Text;
        strReceiverCode = TB_ReceiverCode.Text.Trim();
        strApplicantCode = strUserCode;
        dtMakeDate = DateTime.Now;
        strStatus = LB_Status.Text.Trim();


        if (strDefectName == "" | strDefectDetail == "" | strDefectFinishedDate == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMXNRYWCRSLRBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            Defectment defectment = new Defectment();
            DefectmentBLL defectmentBLL = new DefectmentBLL();

            defectment.DefectType = strDefectType;
            defectment.DefectName = strDefectName;
            defectment.DefectDetail = strDefectDetail;
            defectment.AcceptStandard = strAcceptStandard;
            defectment.DefectFinishedDate = DateTime.Parse(strDefectFinishedDate);
            defectment.MakeDate = dtMakeDate;
            defectment.ApplicantCode = strApplicantCode;
            defectment.ApplicantName = ShareClass.GetUserName(strApplicantCode);
            defectment.Status = "InProgress";

            try
            {
                defectmentBLL.UpdateDefectment(defectment, int.Parse(strDefectID));

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

    protected void LoadDefectment(string strDefectID, string strUserCode)
    {
        string strHQL;
        string strStatus;
        string strDefectName;

        strHQL = "from  Defectment as defectment where defectment.DefectID= " + strDefectID;
        DefectmentBLL defectmentBLL = new DefectmentBLL();
        IList lst = defectmentBLL.GetAllDefectments(strHQL);
        Defectment defectment = (Defectment)lst[0];

        strDefectName = defectment.DefectName.Trim();

        LB_DefectID.Text = defectment.DefectID.ToString();
        DL_Type.SelectedValue = defectment.DefectType;
        TB_DefectName.Text = defectment.DefectName;
        TB_DefectDetail.Text = defectment.DefectDetail;
        TB_AcceptStandard.Text = defectment.AcceptStandard;
        DLC_DefectFinishedDate.Text = defectment.DefectFinishedDate.ToString("yyyy-MM-dd");
        LB_Status.Text = defectment.Status;
        strStatus = defectment.Status.Trim();

        HL_RelatedDoc.NavigateUrl = "";
        HL_RelatedDoc.NavigateUrl = "TTDefectRelatedDoc.aspx?DefectID=" + strDefectID;
        HL_ApproveRecord.NavigateUrl = "";
        HL_ApproveRecord.NavigateUrl = "TTDefectAssignRecord.aspx?DefectID=" + strDefectID;

        HL_DefectReview.Enabled = true;
        HL_DefectReview.NavigateUrl = "TTDefectReviewWL.aspx?DefectID=" + strDefectID;
        HL_WFTemName.Enabled = true;
        HL_WFTemName.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=Defect&RelatedID=" + strDefectID;

        HL_RunDefectByWF.Enabled = true;
        HL_RunDefectByWF.NavigateUrl = "TTRelatedDIYWorkFlowForm.aspx?RelatedType=Defect&RelatedID=" + strDefectID;

        HL_RelatedWorkFlowTemplate.Enabled = true;
        HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=Defect&RelatedID=" + strDefectID;
        HL_ActorGroup.Enabled = true;
        HL_ActorGroup.NavigateUrl = "TTRelatedActorGroup.aspx?RelatedType=Defect&RelatedID=" + strDefectID;

        HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=REQ&RelatedID=" + strDefectID;

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

        TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("GeiNiFenPaLeXuQiu").ToString().Trim() + strDefectID + "  " + strDefectName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();
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
        string strDefectID = LB_DefectID.Text.Trim();
        string strHQL;
        IList lst;

        if (strDefectID != "")
        {
            strHQL = "from Approve as approve where approve.Type = 'Requirement' and approve.RelatedID = " + strDefectID;
            ApproveBLL approveBLL = new ApproveBLL();
            lst = approveBLL.GetAllApproves(strHQL);

            if (lst.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCXCZSPJLBNSC").ToString().Trim() + "')", true);
            }
            else
            {
                Defectment defectment = new Defectment();
                DefectmentBLL defectmentBLL = new DefectmentBLL();

                defectment.DefectID = int.Parse(strDefectID);

                try
                {
                    defectmentBLL.DeleteDefectment(defectment);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);

                    LB_DefectID.Text = "";
                    TB_DefectName.Text = "";
                    TB_DefectDetail.Text = "";
                    TB_ReceiverCode.Text = "";
                    DLC_DefectFinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
        int intDefectID, intPriorID;
        string strDefectName, strOperatorCode, strOperatorName, strAssignManCode, strAssignManName;
        string strContent, strOperation, strType;
        DateTime dtBeginDate, dtEndDate, dtMakeDate;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
        intDefectID = int.Parse(LB_DefectID.Text.Trim());
        strType = DL_Type.SelectedValue.Trim();
        strDefectName = TB_DefectName.Text.Trim();
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

        DefectAssignRecordBLL defectAssignRecordBLL = new DefectAssignRecordBLL();
        DefectAssignRecord defectAssignRecord = new DefectAssignRecord();

        defectAssignRecord.DefectID = intDefectID;
        defectAssignRecord.Type = strType;
        defectAssignRecord.DefectName = strDefectName;
        defectAssignRecord.OperatorCode = strOperatorCode;
        defectAssignRecord.OperatorName = strOperatorName;
        defectAssignRecord.OperatorContent = "";
        defectAssignRecord.OperationTime = DateTime.Now;
        defectAssignRecord.BeginDate = dtBeginDate;
        defectAssignRecord.EndDate = dtEndDate;
        defectAssignRecord.AssignManCode = strAssignManCode;
        defectAssignRecord.AssignManName = strAssignManName;
        defectAssignRecord.Content = "";
        defectAssignRecord.Operation = strOperation;
        defectAssignRecord.PriorID = intPriorID;
        defectAssignRecord.RouteNumber = GetRouteNumber(intDefectID.ToString());
        defectAssignRecord.MakeDate = dtMakeDate;
        defectAssignRecord.Status = "ToHandle";

        try
        {
            defectAssignRecordBLL.AddDefectAssignRecord(defectAssignRecord);
            UpdateDefectStatus(intDefectID.ToString(), "InProgress");

            TB_Message.Text = ShareClass.GetUserName(strUserCode) + LanguageHandle.GetWord("FenPaLeXuQiu").ToString().Trim() + intDefectID.ToString() + " " + strDefectName + LanguageHandle.GetWord("GeiNiQingJiShiShouLi").ToString().Trim();

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

    protected void UpdateDefectStatus(string strDefectID, string strStatus)
    {
        string strHQL = "from Defectment as defectment where defectment.DefectID = " + strDefectID;
        DefectmentBLL defectmentBLL = new DefectmentBLL();
        IList lst = defectmentBLL.GetAllDefectments(strHQL);
        Defectment defectment = (Defectment)lst[0];
        defectment.Status = "InProgress";

        int intRouteNumber = defectment.RouteNumber;
        defectment.RouteNumber = intRouteNumber + 1;

        try
        {
            defectmentBLL.UpdateDefectment(defectment, int.Parse(strDefectID));
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFPSBJC").ToString().Trim() + "')", true);
        }
    }

    private int GetRouteNumber(string strDefectID)
    {
        DefectmentBLL defectmentBLL = new DefectmentBLL();
        string strHQL = "from Defectment as defectment where defectment.DefectID = " + strDefectID;
        IList lst = defectmentBLL.GetAllDefectments(strHQL);

        Defectment defectment = (Defectment)lst[0];

        return defectment.RouteNumber + 1;
    }

}
