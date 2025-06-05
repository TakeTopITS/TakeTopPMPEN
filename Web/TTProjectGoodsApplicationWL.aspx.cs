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

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProjectGoodsApplicationWL : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strRelatedTypeWF;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName, strHQL;
        IList lst;
        string strReviewType;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        strRelatedTypeWF = strRelatedType;

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";
            //this.Title = strRelatedType + ":" + strRelatedID + "资产领用申请";
        }

        if (strRelatedType == "Other")
        {
            strRelatedType = "Other";
        }

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_ApplyTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_FinishTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();
            DL_Type.Items.Insert(0, new ListItem("--Select--", ""));

            strHQL = "from JNUnit as jnUnit Order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            TB_ApplicantCode.Text = LB_UserCode.Text.Trim();
            LB_ApplicantName.Text = LB_UserName.Text.Trim();

            strReviewType = "AssetWithdrawal";
            strReviewType = "%" + strReviewType + "%";

            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName in ";
            strHQL += "(Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = " + "'" + strRelatedTypeWF + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + ")";
            strHQL += " and workFlowTemplate.Type like " + "'" + strReviewType + "'";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
          
            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();

            LoadGoodsApplication(strUserCode, strRelatedType, strRelatedID);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        string strAAID;
        IList lst;
        int intWLNumber = 0;

        if (e.CommandName != "Page")
        {
            strAAID = ((Button)e.Item.FindControl("BT_AAID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsApplication as goodsApplication where goodsApplication.AAID = " + strAAID;
            GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
            lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);
            GoodsApplication goodsApplication = (GoodsApplication)lst[0];

            LB_AAID.Text = goodsApplication.AAID.ToString();
            TB_AAName.Text = goodsApplication.GAAName.Trim();
            TB_ApplicantCode.Text = goodsApplication.ApplicantCode.Trim();
            LB_ApplicantName.Text = goodsApplication.ApplicantName.Trim();
            TB_ApplyReason.Text = goodsApplication.ApplyReason.Trim();
            DL_Type.SelectedValue = goodsApplication.Type;
            DLC_ApplyTime.Text = goodsApplication.ApplyTime.ToString("yyyy-MM-dd");
            DLC_FinishTime.Text = goodsApplication.FinishTime.ToString("yyyy-MM-dd");
            DL_Status.SelectedValue = goodsApplication.Status.Trim();

            LoadGoodsApplicationDetail(strAAID);

            intWLNumber = LoadRelatedWL("AssetWithdrawal", "Assets", int.Parse(strAAID));


            if (intWLNumber > 0)
            {
                BT_SubmitApply.Enabled = false;

                BT_New.Enabled = false;
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;

                BT_NewAA.Enabled = true;
                BT_UpdateAA.Enabled = false;
                BT_DeleteAA.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                BT_NewAA.Enabled = true;
                BT_UpdateAA.Enabled = true;
                BT_DeleteAA.Enabled = true;

                BT_SubmitApply.Enabled = true;
            }
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            int intWLNumber;
            string strStatus = DL_Status.SelectedValue.Trim();
            string strAAID = LB_AAID.Text.Trim();
            string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string strHQL = " from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.ID = " + strID;

            GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
            IList lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);
            GoodsApplicationDetail goodsApplicationDetail = (GoodsApplicationDetail)lst[0];

            LB_DetailID.Text = goodsApplicationDetail.ID.ToString();
            TB_GoodsCode.Text = goodsApplicationDetail.GoodsCode;
            TB_GoodsName.Text = goodsApplicationDetail.GoodsName.Trim();
            TB_Spec.Text = goodsApplicationDetail.Spec.Trim();
            TB_Brand.Text = goodsApplicationDetail.Brand;

            NB_Number.Amount = goodsApplicationDetail.Number;
            DL_Unit.SelectedValue = goodsApplicationDetail.Unit;

            TB_ModelNumber.Text = goodsApplicationDetail.ModelNumber.Trim();
            TB_IP.Text = goodsApplicationDetail.IP.Trim();

            intWLNumber = GetRelatedWorkFlowNumber("AssetWithdrawal", "Assets", strAAID);

            if (intWLNumber > 0)
            {
                BT_New.Enabled = false;
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
        }
    }

    protected void BT_NewClaim_Click(object sender, EventArgs e)
    {
        string strAAID, strAAName, strApplyReason, strApplicantCode, strApplicantName;
        string strType, strStatus;
        DateTime dtApplyTime, dtFinishTime;

        string strUserCode = LB_UserCode.Text.Trim();

        strAAName = TB_AAName.Text.Trim();
        strApplyReason = TB_ApplyReason.Text.Trim();
        strApplicantCode = TB_ApplicantCode.Text.Trim();
        strApplicantName = ShareClass.GetUserName(TB_ApplicantCode.Text.Trim());
        dtApplyTime = DateTime.Parse(DLC_ApplyTime.Text);
        dtFinishTime = DateTime.Parse(DLC_FinishTime.Text);
        strType = DL_Type.SelectedValue;
        strStatus = DL_Status.SelectedValue;

        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        GoodsApplication goodsApplication = new GoodsApplication();

        goodsApplication.GAAName = strAAName;
        goodsApplication.ApplyReason = strApplyReason;
        goodsApplication.ApplicantCode = strApplicantCode;
        goodsApplication.ApplicantName = strApplicantName;
        goodsApplication.ApplyTime = dtApplyTime;
        goodsApplication.FinishTime = dtFinishTime;
        goodsApplication.Type = strType;
        goodsApplication.Status = strStatus;
        goodsApplication.RelatedType = strRelatedType;
        goodsApplication.RelatedID = int.Parse(strRelatedID);

        try
        {
            goodsApplicationBLL.AddGoodsApplication(goodsApplication);
            strAAID = ShareClass.GetMyCreatedMaxGoodsApplicationID(strUserCode);
            LB_AAID.Text = strAAID;

            BT_New.Enabled = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;


            BT_UpdateAA.Enabled = true;
            BT_DeleteAA.Enabled = true;
            BT_SubmitApply.Enabled = true;


            LoadGoodsApplication(strUserCode, strRelatedType, strRelatedID);
            LoadRelatedWL("AssetWithdrawal", "Assets", int.Parse(strAAID));


            DL_Status.SelectedValue = "New";


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_UpdateClaim_Click(object sender, EventArgs e)
    {
        string strAAID, strAAName, strApplyReason, strApplicantCode, strApplicantName;
        string strType, strStatus;
        DateTime dtApplyTime, dtFinishTime;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        strAAID = LB_AAID.Text.Trim();
        strAAName = TB_AAName.Text.Trim();
        strApplyReason = TB_ApplyReason.Text.Trim();
        strApplicantCode = TB_ApplicantCode.Text.Trim();
        strApplicantName = ShareClass.GetUserName(TB_ApplicantCode.Text.Trim());
        dtApplyTime = DateTime.Parse(DLC_ApplyTime.Text);
        dtFinishTime = DateTime.Parse(DLC_FinishTime.Text);
        strType = DL_Type.SelectedValue;
        strStatus = DL_Status.SelectedValue;

        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        strHQL = "from GoodsApplication as goodsApplication where goodsApplication.AAID = " + strAAID;
        lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);
        GoodsApplication goodsApplication = (GoodsApplication)lst[0];

        goodsApplication.GAAName = strAAName;
        goodsApplication.ApplyReason = strApplyReason;
        goodsApplication.ApplicantCode = strApplicantCode;
        goodsApplication.ApplicantName = strApplicantName;
        goodsApplication.ApplyTime = dtApplyTime;
        goodsApplication.FinishTime = dtFinishTime;
        goodsApplication.Type = strType;
        goodsApplication.Status = strStatus;
        goodsApplication.RelatedType = strRelatedType;
        goodsApplication.RelatedID = int.Parse(strRelatedID);
        try
        {
            goodsApplicationBLL.UpdateGoodsApplication(goodsApplication, int.Parse(strAAID));
            LoadGoodsApplication(strUserCode, strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_DeleteClaim_Click(object sender, EventArgs e)
    {
        string strAAID;
        string strHQL;

        string strUserCode = LB_UserCode.Text.Trim();

        strAAID = LB_AAID.Text.Trim();

        strHQL = "delete from T_GoodsApplication where AAID = " + strAAID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_DeleteAA.Enabled = false;
            BT_UpdateAA.Enabled = false;

            BT_New.Enabled = false;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            BT_SubmitApply.Enabled = false;

            LoadGoodsApplication(strUserCode, strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = e.Item.Cells[0].Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From Goods as goods where goods.ID = " + strID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            Goods goods = (Goods)lst[0];

            TB_GoodsCode.Text = goods.GoodsCode.Trim();
            TB_GoodsName.Text = goods.GoodsName.Trim();
            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();
            TB_Brand.Text = goods.Manufacturer;

            NB_Number.Amount = goods.Number;
            DL_Unit.SelectedValue = goods.UnitName;
        }
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql3.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strGoodsCode, strGoodsName, strModelNumber, strSpec;


        DataGrid3.CurrentPageIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();

        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";

        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strHQL = "Select * From T_Goods as goods Where goods.GoodsCode Like " + "'" + strGoodsCode + "'" + " and goods.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and goods.ModelNumber Like " + "'" + strModelNumber + "'";
        strHQL += " and goods.Spec Like " + "'" + strSpec + "'";
        strHQL += " and goods.Number > 0";
        strHQL += " Order by goods.Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        LB_Sql3.Text = strHQL;
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;
        string strAAID = LB_AAID.Text.Trim();
        string strStatus = DL_Status.SelectedValue.Trim();
        string strGoodsCode = TB_GoodsCode.Text.Trim();
        string strGoodsName = TB_GoodsName.Text.Trim();

        string strSpec = TB_Spec.Text.Trim();
        decimal deNumber = NB_Number.Amount;
        string strUnit = DL_Unit.SelectedValue;
        string strManufacturer = TB_Manufacturer.Text.Trim();
        string strIP = TB_IP.Text.Trim();
        string strModelNumber = TB_ModelNumber.Text.Trim();


        GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
        GoodsApplicationDetail goodsApplicationDetail = new GoodsApplicationDetail();

        goodsApplicationDetail.AAID = int.Parse(strAAID);
        goodsApplicationDetail.GoodsCode = strGoodsCode;
        goodsApplicationDetail.GoodsName = strGoodsName;

        goodsApplicationDetail.Spec = strSpec;
        goodsApplicationDetail.Brand = TB_Brand.Text;

        goodsApplicationDetail.Number = deNumber;
        goodsApplicationDetail.Unit = strUnit;

        goodsApplicationDetail.ModelNumber = strModelNumber;
        goodsApplicationDetail.IP = strIP;


        try
        {
            goodsApplicationDetailBLL.AddGoodsApplicationDetail(goodsApplicationDetail);

            strID = ShareClass.GetMyCreatedMaxGoodsApplicationDetailID(strAAID);
            LB_DetailID.Text = strID;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            LoadGoodsApplicationDetail(strAAID);
        }
        catch
        {
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = LB_DetailID.Text.Trim();
        string strAAID = LB_AAID.Text.Trim();
        string strStatus = DL_Status.SelectedValue.Trim();
        string strGoodsCode = TB_GoodsCode.Text.Trim();
        string strGoodsName = TB_GoodsName.Text.Trim();
        string strSpec = TB_Spec.Text.Trim();
        decimal deNumber = NB_Number.Amount;
        string strUnit = DL_Unit.SelectedValue;

        string strManufacturer = TB_Manufacturer.Text.Trim();
        string strIP = TB_IP.Text.Trim();
        string strModelNumber = TB_ModelNumber.Text.Trim();

        GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
        strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.ID = " + strID;
        lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);
        GoodsApplicationDetail goodsApplicationDetail = (GoodsApplicationDetail)lst[0];

        goodsApplicationDetail.AAID = int.Parse(strAAID);
        goodsApplicationDetail.GoodsCode = strGoodsCode;
        goodsApplicationDetail.GoodsName = strGoodsName;
        goodsApplicationDetail.Spec = strSpec;
        goodsApplicationDetail.Brand = TB_Brand.Text;

        goodsApplicationDetail.Number = deNumber;
        goodsApplicationDetail.Unit = strUnit;

        goodsApplicationDetail.ModelNumber = strModelNumber;
        goodsApplicationDetail.IP = strIP;


        try
        {
            goodsApplicationDetailBLL.UpdateGoodsApplicationDetail(goodsApplicationDetail, int.Parse(strID));

            LoadGoodsApplicationDetail(strAAID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = LB_DetailID.Text.Trim();
        string strAAID = LB_AAID.Text.Trim();
        string strStatus = DL_Status.SelectedValue.Trim();
        string strGoodsName = TB_AAName.Text.Trim();
        string strSpec = TB_Spec.Text.Trim();
        decimal deNumber = NB_Number.Amount;
        string strUnit = DL_Unit.SelectedValue;

        GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
        strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.ID = " + strID;
        lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);
        GoodsApplicationDetail goodsApplicationDetail = (GoodsApplicationDetail)lst[0];


        try
        {
            goodsApplicationDetailBLL.DeleteGoodsApplicationDetail(goodsApplicationDetail);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            LoadGoodsApplicationDetail(strAAID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
        }
    }

    protected string SubmitApply()
    {
        string strHQL;
        string strAAName, strApplyReason, strCmdText;

        string strAAID, strXMLFileName, strXMLFile2;
        IList lst;

        string strWLID, strTemName, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strAAID = LB_AAID.Text.Trim();
        strAAName = TB_AAName.Text.Trim();
        strApplyReason = TB_ApplyReason.Text.Trim();

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC") + "');</script>");
            return "0";
        }

        XMLProcess xmlProcess = new XMLProcess();
        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        strHQL = "from GoodsApplication as goodsApplication where goodsApplication.AAID = " + strAAID;
        lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);
        GoodsApplication goodsApplication = (GoodsApplication)lst[0];

        goodsApplication.Status = "InProgress";


        try
        {
            goodsApplicationBLL.UpdateGoodsApplication(goodsApplication, int.Parse(strAAID));

            strXMLFileName = "AssetWithdrawal" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;



            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = strAAName;
            workFlow.WLType = "AssetWithdrawal";
            workFlow.Status = "New";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = ShareClass.GetUserName(strUserCode);
            workFlow.Description = strApplyReason;
            workFlow.XMLFile = strXMLFile2;
            workFlow.RelatedType = "Assets";
            workFlow.RelatedID = goodsApplication.AAID;
            workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

            if (CB_SMS.Checked == true)
            {
                workFlow.ReceiveSMS = "YES";
            }
            else
            {
                workFlow.ReceiveSMS = "NO";
            }

            if (CB_Mail.Checked == true)
            {
                workFlow.ReceiveEMail = "YES";
            }
            else
            {
                workFlow.ReceiveEMail = "NO";
            }

            try
            {
                workFlowBLL.AddWorkFlow(workFlow);

                strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

                strCmdText = "select * from T_GoodsApplication where AAID = " + strAAID;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_GoodsApplication", strXMLFile2);

                LoadRelatedWL("AssetWithdrawal", "Assets", int.Parse(strAAID));

                BT_New.Enabled = false;
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                BT_NewAA.Enabled = true;
                BT_UpdateAA.Enabled = false;
                BT_DeleteAA.Enabled = false;
                DL_Status.SelectedValue = "InProgress";

                BT_SubmitApply.Enabled = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCLYGZLSCCG") + "')", true);
            }
            catch
            {
                strWLID = "0";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCLYGZLSSCSB") + "')", true);
            }
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }

    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'AssetWithdrawal'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }


    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strAAID, strStatus;
        strAAID = LB_AAID.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        if (strAAID != "")
        {

            strHQL = "from GoodsApplication as goodsApplication where goodsApplication.AAID = " + strAAID;
            GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
            lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

            GoodsApplication goodsApplication = (GoodsApplication)lst[0];

            goodsApplication.Status = strStatus;

            try
            {
                goodsApplicationBLL.UpdateGoodsApplication(goodsApplication, int.Parse(strAAID));
            }
            catch
            {
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        IList lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadGoodsApplication(string strApplicantCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsApplication as goodsApplication where goodsApplication.ApplicantCode = " + "'" + strApplicantCode + "'" + " and goodsApplication.RelatedType = " + "'" + strRelatedType + "'" + " and goodsApplication.RelatedID = " + strRelatedID + "  Order by goodsApplication.AAID DESC";
        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }


    protected void LoadGoodsApplicationDetail(string strAAID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.AAID = " + strAAID;
        GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
        lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


    protected int LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        return lst.Count;
    }

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

    protected string GetWorkFlowStatus(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow.Status.Trim();
    }



}
