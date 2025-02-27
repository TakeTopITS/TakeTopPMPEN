using System; using System.Resources;
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

public partial class TTGoodsPurchaseOrderDetail : System.Web.UI.Page
{
    string strPOID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName;

        strPOID = Request.QueryString["POID"];

        //this.Title = "ŒÔ¡œ≤…π∫…Í«Î";

        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_ArrivalTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_PurTime.Text = DateTime.Now.ToString("yyyy-MM-dd");


            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();
            DL_Type.Items.Insert(0, new ListItem("--Select--", ""));


            TB_PurManCode.Text = strUserCode;
            LB_PurManName.Text = strUserName;

            TB_ApplicantCode.Text = strUserCode;
            LB_ApplicantName.Text = strUserCode;

            ShareClass.LoadWFTemplate(strUserCode, "MaterialProcurement", DL_TemName);

            LoadGoodsPurchaseOrder(strPOID);
            LoadRelatedConstract(strPOID);

            ShareClass.LoadMemberByUserCodeForDataGrid(strUserCode, "All", DataGrid3);
            ShareClass.InitialInvolvedProjectTree(TreeView2, strUserCode);
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strProjectID = treeNode.Target.Trim();

            NB_RelatedID.Amount = int.Parse(strProjectID);
        }
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strPOID;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strPOID = LB_POID.Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsPurRecord as goodsPurRecord where goodsPurRecord.ID = " + strID;

            GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
            lst = goodsPurRecordBLL.GetAllGoodsPurRecords(strHQL);
            GoodsPurRecord goodsPurRecord = (GoodsPurRecord)lst[0];

            TB_GoodsName.Text = goodsPurRecord.GoodsName;
            TB_ModelNumber.Text = goodsPurRecord.ModelNumber.Trim();
            TB_Spec.Text = goodsPurRecord.Spec;
            TB_PurReason.Text = goodsPurRecord.PurReason;
            NB_Price.Amount = goodsPurRecord.Price;
            TB_ApplicantCode.Text = goodsPurRecord.ApplicantCode;
          
            LB_ApplicantName.Text = GetUserName(goodsPurRecord.ApplicantCode);
            DL_Type.SelectedValue = goodsPurRecord.Type;
            DL_Unit.SelectedValue = goodsPurRecord.Unit;
            NB_Number.Amount = goodsPurRecord.Number;
          
            int intWLNumber = GetRelatedWorkFlowNumber("MaterialProcurement", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPOID);
            if (intWLNumber == 0)
            {
                BT_New.Enabled = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_New.Enabled = false;
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
            }
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        TB_ApplicantCode.Text = strUserCode;
        LB_ApplicantName.Text = GetUserName(strUserCode);
    }

    protected void LoadGoodsPurchaseOrder(string strPOID)
    {
        string strHQL;
        IList lst;
        int intWLNumber;

        string strUserCode = LB_UserCode.Text;

        strHQL = "from GoodsPurchaseOrder as goodsPurchaseOrder where goodsPurchaseOrder.POID = " + strPOID;
        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        lst = goodsPurchaseOrderBLL.GetAllGoodsPurchaseOrders(strHQL);
        GoodsPurchaseOrder goodsPurchaseOrder = (GoodsPurchaseOrder)lst[0];

        LB_POID.Text = goodsPurchaseOrder.POID.ToString();
        TB_POName.Text = goodsPurchaseOrder.GPOName.Trim();
        DLC_PurTime.Text = goodsPurchaseOrder.PurTime.ToString("yyyy-MM-dd");
        DLC_ArrivalTime.Text = goodsPurchaseOrder.ArrivalTime.ToString("yyyy-MM-dd");
        NB_Amount.Amount = goodsPurchaseOrder.Amount;
        TB_Comment.Text = goodsPurchaseOrder.Comment.Trim();
        DL_POStatus.SelectedValue = goodsPurchaseOrder.Status.Trim();
        TB_PurManCode.Text = goodsPurchaseOrder.OperatorCode.Trim();
        LB_PurManName.Text = goodsPurchaseOrder.OperatorName.Trim();

        DL_RelatedType.SelectedValue = goodsPurchaseOrder.RelatedType.Trim();
        NB_RelatedID.Amount = goodsPurchaseOrder.RelatedID;

        LoadGoodsPurchaseOrderDetail(strPOID);

        BT_Update.Enabled = false;
        BT_Delete.Enabled = false;

        intWLNumber = GetRelatedWorkFlowNumber("MaterialProcurement", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPOID);

        if (intWLNumber == 0)
        {
            BT_UpdatePO.Enabled = true;

            BT_New.Enabled = true;
        }
        else
        {
            BT_UpdatePO.Enabled = false;
         
            BT_New.Enabled = false;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;
        }


        TB_WLName.Text = LanguageHandle.GetWord("GouMai").ToString().Trim()  + goodsPurchaseOrder.GPOName.Trim() + LanguageHandle.GetWord("ShenQing").ToString().Trim();

        LoadRelatedWL("MaterialProcurement", LanguageHandle.GetWord("WuLiao").ToString().Trim(), goodsPurchaseOrder.POID);

        BT_SubmitApply.Enabled = true;

    }

    protected void BT_AddPO_Click(object sender, EventArgs e)
    {
        string strPOID, strPOName, strPurManCode, strPurManName, strOperatorCode, strOperatorName, strComment;
        DateTime dtPurTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;

        strPOName = TB_POName.Text.Trim();
        strPurManCode = TB_PurManCode.Text.Trim();
        strPurManName = LB_PurManName.Text.Trim();
        strOperatorCode = LB_UserCode.Text.Trim();
        strOperatorName = LB_UserName.Text.Trim();
        strComment = TB_Comment.Text.Trim();
        dtPurTime = DateTime.Parse(DLC_PurTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);
        deAmount = NB_Amount.Amount;
        strStatus = DL_POStatus.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();

        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        GoodsPurchaseOrder goodsPurchaseOrder = new GoodsPurchaseOrder();

        goodsPurchaseOrder.GPOName = strPOName;
        goodsPurchaseOrder.PurManCode = strPurManCode;
        try
        {
            goodsPurchaseOrder.PurManName = GetUserName(strPurManCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWCGRDMBZCWCRJC").ToString().Trim()+"')", true);
            return;
        }
        goodsPurchaseOrder.OperatorCode = strOperatorCode;
        goodsPurchaseOrder.OperatorName = strOperatorName;
        goodsPurchaseOrder.PurTime = dtPurTime;
        goodsPurchaseOrder.ArrivalTime = dtArrivalTime;
        goodsPurchaseOrder.Amount = 0;
        goodsPurchaseOrder.Comment = strComment;
        goodsPurchaseOrder.Status = "New";


        if (DL_RelatedType.SelectedValue.Trim() == "Other")
        {
            goodsPurchaseOrder.RelatedType = "Other";
            goodsPurchaseOrder.RelatedID = 0;
        }

        if (DL_RelatedType.SelectedValue.Trim() == "Project")
        {
            goodsPurchaseOrder.RelatedType = "Project";
            goodsPurchaseOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());
        }


        try
        {
            goodsPurchaseOrderBLL.AddGoodsPurchaseOrder(goodsPurchaseOrder);

            strPOID = ShareClass.GetMyCreatedMaxGoodsPurchaseOrderID(strOperatorCode);
            LB_POID.Text = strPOID;

            BT_UpdatePO.Enabled = true;
         

            BT_New.Enabled = true;

            NB_Amount.Amount = 0;

            LB_GoodsOwner.Text = LanguageHandle.GetWord("CaiGouDan").ToString().Trim() + strPOID + LanguageHandle.GetWord("MingXi").ToString().Trim();
            TB_WLName.Text = LanguageHandle.GetWord("GouMai").ToString().Trim() + strPOName + LanguageHandle.GetWord("ShenQing").ToString().Trim();

            BT_SubmitApply.Enabled = true;

        
            LoadGoodsPurchaseOrderDetail(strPOID);

            LoadRelatedWL("MaterialProcurement", LanguageHandle.GetWord("WuLiao").ToString().Trim(), goodsPurchaseOrder.POID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXJCCKNCGMCZD50GHZHBZZSZD100GHZGDJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_UpdatePO_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strPOID, strUserCode, strPOName, strPurManCode, strPurManName, strComment;
        DateTime dtPurTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();

        strPOID = LB_POID.Text.Trim();
        strPOName = TB_POName.Text.Trim();
        strPurManCode = TB_PurManCode.Text.Trim();
        strPurManName = LB_PurManName.Text.Trim();
        dtPurTime = DateTime.Parse(DLC_PurTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);
        deAmount = NB_Amount.Amount;
        strComment = TB_Comment.Text.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();

        strHQL = "from GoodsPurchaseOrder as goodsPurchaseOrder where goodsPurchaseOrder.POID = " + strPOID;
        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        lst = goodsPurchaseOrderBLL.GetAllGoodsPurchaseOrders(strHQL);

        GoodsPurchaseOrder goodsPurchaseOrder = (GoodsPurchaseOrder)lst[0];

        goodsPurchaseOrder.GPOName = strPOName;

        goodsPurchaseOrder.PurManCode = strPurManCode;
        try
        {
            goodsPurchaseOrder.PurManName = GetUserName(strPurManCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWCGRDMBZCWCRJC").ToString().Trim()+"')", true);
            return;
        }

        goodsPurchaseOrder.PurTime = dtPurTime;
        goodsPurchaseOrder.ArrivalTime = dtArrivalTime;
        goodsPurchaseOrder.Amount = deAmount;
        goodsPurchaseOrder.Comment = strComment;
        goodsPurchaseOrder.Status = strStatus;

        if (DL_RelatedType.SelectedValue.Trim() == "Other")
        {
            goodsPurchaseOrder.RelatedType = "Other";
            goodsPurchaseOrder.RelatedID = 0;
        }

        if (DL_RelatedType.SelectedValue.Trim() == "Project")
        {
            goodsPurchaseOrder.RelatedType = "Project";
            goodsPurchaseOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());
        }

        try
        {
            goodsPurchaseOrderBLL.UpdateGoodsPurchaseOrder(goodsPurchaseOrder, int.Parse(strPOID));
            LoadGoodsPurchaseOrder(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_DeletePO_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPOID, strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        strPOID = LB_POID.Text.Trim();

        strHQL = "Delete From T_GoodsPurchaseOrder Where POID = " + strPOID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_UpdatePO.Enabled = false;
          

            BT_New.Enabled = false;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;
            BT_SubmitApply.Enabled = false;

            LoadGoodsPurchaseOrder(strUserCode);
            LoadGoodsPurchaseOrderDetail(strPOID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strRecordID, strPOID, strType, strGoodsName, strModelNumber, strSpec, strStatus;
        string strSupplier, strSupplierPhone, strUnitName;
        decimal decNumber;
        DateTime dtBuyTime;
        decimal dePrice;
        string strApplicantCode, strPurReason;
        string strRelatedType, strRelatedID;

        strPOID = LB_POID.Text.Trim();
        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        strRelatedID = NB_RelatedID.Amount.ToString();

        strType = DL_Type.SelectedValue.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        decNumber = NB_Number.Amount;
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPurReason = TB_PurReason.Text.Trim();
        dePrice = NB_Price.Amount;
        strApplicantCode = TB_ApplicantCode.Text.Trim();
        dtBuyTime = DateTime.Now;
        strSupplier = TB_Supplier.Text.Trim();
        strSupplierPhone = TB_SupplierPhone.Text.Trim();
   

        if (strType == "" | strGoodsName == "" | strSpec == "" | strApplicantCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
            GoodsPurRecord goodsPurRecord = new GoodsPurRecord();

            goodsPurRecord.POID = int.Parse(strPOID);
            goodsPurRecord.Type = strType;
            goodsPurRecord.GoodsCode = "";
            goodsPurRecord.GoodsName = strGoodsName;
            goodsPurRecord.Number = decNumber;
            goodsPurRecord.Unit = strUnitName;
            goodsPurRecord.Number = decNumber;
            goodsPurRecord.Price = dePrice;
            goodsPurRecord.ModelNumber = strModelNumber;
            goodsPurRecord.Spec = strSpec;
            goodsPurRecord.PurReason = strPurReason;
            goodsPurRecord.PurTime = dtBuyTime;

            goodsPurRecord.BomVerID = 1;

            goodsPurRecord.ApplicantCode = strApplicantCode;
            try
            {
                goodsPurRecord.ApplicantName = GetUserName(strApplicantCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWSRDMBZCWCRJC").ToString().Trim()+"')", true);
                return;
            }

          
            goodsPurRecord.CheckInNumber = 0;

            try
            {
                goodsPurRecordBLL.AddGoodsPurRecord(goodsPurRecord);

                strRecordID = ShareClass.GetMyCreatedMaxGoodsPurRecordID(strPOID);
                LB_ID.Text = strRecordID;

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                LoadGoodsPurchaseOrderDetail(strPOID);

                NB_Amount.Amount = SumGoodsPurchaseOrderAmount(strPOID);
                UpdateGoodsPurchaseOrderAmount(strPOID, NB_Amount.Amount);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strType, strGoodsName, strModelNumber, strSpec, strStatus;
        string strSupplier, strSupplierPhone, strPurReason, strUnitName;
        DateTime dtBuyTime;
        decimal dePrice, deNumber;
        string strApplicantCode;
        string strRelatedType, strRelatedID;

        string strID, strPOID, strPurManCode, strPurManName;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();

        strPOID = LB_POID.Text.Trim();
        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        strRelatedID = NB_RelatedID.Amount.ToString();

        strType = DL_Type.SelectedValue.Trim();

        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPurReason = TB_PurReason.Text.Trim();
        dePrice = NB_Price.Amount;
        deNumber = NB_Number.Amount;
        strApplicantCode = TB_ApplicantCode.Text.Trim();
        dtBuyTime = DateTime.Now;
        strSupplier = TB_Supplier.Text.Trim();
        strSupplierPhone = TB_SupplierPhone.Text.Trim();
        strPurManCode = TB_PurManCode.Text.Trim();
        strPurManName = LB_PurManName.Text.Trim();
      

        if (strType == "" | strGoodsName == "" | strSpec == "" | strApplicantCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
            strHQL = "from GoodsPurRecord as goodsPurRecord where goodsPurRecord.ID = " + strID;
            lst = goodsPurRecordBLL.GetAllGoodsPurRecords(strHQL);
            GoodsPurRecord goodsPurRecord = (GoodsPurRecord)lst[0];

            goodsPurRecord.POID = int.Parse(strPOID);
            goodsPurRecord.Type = strType;
            goodsPurRecord.GoodsCode = "";
            goodsPurRecord.GoodsName = strGoodsName;
            goodsPurRecord.Number = deNumber;
            goodsPurRecord.Unit = strUnitName;
            goodsPurRecord.Price = dePrice;
            goodsPurRecord.ModelNumber = strModelNumber;
            goodsPurRecord.Spec = strSpec;
            goodsPurRecord.PurReason = strPurReason;
            goodsPurRecord.PurTime = dtBuyTime;
          
            goodsPurRecord.ApplicantCode = strApplicantCode;
            try
            {
                goodsPurRecord.ApplicantName = GetUserName(strApplicantCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWSZDMBZCWCRJC").ToString().Trim()+"')", true);
                return;
            }
       

            try
            {
                goodsPurRecordBLL.UpdateGoodsPurRecord(goodsPurRecord, int.Parse(strID));

                LoadGoodsPurchaseOrderDetail(strPOID);

                NB_Amount.Amount = SumGoodsPurchaseOrderAmount(strPOID);
                UpdateGoodsPurchaseOrderAmount(strPOID, NB_Amount.Amount);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strPOID = LB_POID.Text.Trim();

        string strHQL;
        IList lst;

        GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
        strHQL = "from GoodsPurRecord as goodsPurRecord where goodsPurRecord.ID = " + strID;
        lst = goodsPurRecordBLL.GetAllGoodsPurRecords(strHQL);
        GoodsPurRecord goodsPurRecord = (GoodsPurRecord)lst[0];

        try
        {
            goodsPurRecordBLL.DeleteGoodsPurRecord(goodsPurRecord);

            LoadGoodsPurchaseOrderDetail(strPOID);

            NB_Amount.Amount = SumGoodsPurchaseOrderAmount(strPOID);
            UpdateGoodsPurchaseOrderAmount(strPOID, NB_Amount.Amount);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
        }
    }

   
    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText, strPOID;
        string strWLID, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strPOID = LB_POID.Text.Trim();

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = GetUserName(strCreatorCode);

        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim()+"');</script>");
            return "0";
        }

        strXMLFileName = strWLType + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        WorkFlow workFlow = new WorkFlow();

        workFlow.WLName = strWLName;
        workFlow.WLType = strWLType;
        workFlow.XMLFile = strXMLFile2;
        workFlow.TemName = strTemName;
        workFlow.Description = strDescription;
        workFlow.CreatorCode = strCreatorCode;
        workFlow.CreatorName = strCreatorName;
        workFlow.CreateTime = DateTime.Now;
        workFlow.RelatedType = LanguageHandle.GetWord("WuLiao").ToString().Trim();
        workFlow.Status = "New";
        workFlow.RelatedID = int.Parse(strPOID);
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

            LoadRelatedWL(strWLType, LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strPOID));

            UpdateGoodsPurchaseStatus(strPOID, "InProgress");
            DL_POStatus.SelectedValue = "InProgress";

            strCmdText = "select * from T_GoodsPurchaseOrder where POID = " + strPOID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsPurchaseOrder", strXMLFile2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLPCGSSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLPCGSSBKNGZLMCGCZD25GHZJC").ToString().Trim()+"')", true);
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

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'MaterialProcurement'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected void DL_POStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPOID, strStatus, strUserCode;

        strPOID = LB_POID.Text.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strPOID != "")
        {
            UpdateGoodsPurchaseStatus(strPOID, strStatus);
            LoadGoodsPurchaseOrder(strUserCode);
        }
    }

  

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }


    protected void UpdateGoodsPurchaseStatus(string strPOID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsPurchaseOrder as goodsPurchaseOrder where goodsPurchaseOrder.POID = " + strPOID;
        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        lst = goodsPurchaseOrderBLL.GetAllGoodsPurchaseOrders(strHQL);

        GoodsPurchaseOrder goodsPurchaseOrder = (GoodsPurchaseOrder)lst[0];

        goodsPurchaseOrder.Status = strStatus;

        try
        {
            goodsPurchaseOrderBLL.UpdateGoodsPurchaseOrder(goodsPurchaseOrder, int.Parse(strPOID));
        }
        catch
        {
        }
    }

    protected void UpdateGoodsGoodsPurchaseOrderDetailStatus(string strID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsPurRecord as goodsPurRecord where goodsPurRecord.ID = " + strID;
        GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
        lst = goodsPurRecordBLL.GetAllGoodsPurRecords(strHQL);

        GoodsPurRecord goodsPurRecord = (GoodsPurRecord)lst[0];

      

        try
        {
            goodsPurRecordBLL.UpdateGoodsPurRecord(goodsPurRecord, int.Parse(strID));
        }
        catch
        {
        }
    }

    protected void LoadRelatedConstract(string strPOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.Status <> 'Archived'";
        strHQL += " and constract.ConstractCode in (select constractRelatedGoodsPurchaseOrder.ConstractCode from ConstractRelatedGoodsPurchaseOrder as constractRelatedGoodsPurchaseOrder where constractRelatedGoodsPurchaseOrder.POID = " + strPOID + ")";
        strHQL += " Order by constract.SignDate DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }


    protected void LoadGoodsPurchaseOrderDetail(string strPOID)
    {
        LB_GoodsOwner.Text = LanguageHandle.GetWord("CaiGouDan").ToString().Trim() + ": " + strPOID + LanguageHandle.GetWord("MingXi").ToString().Trim();

        string strHQL = "Select * from T_GoodsPurRecord where POID = " + strPOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected int GetWLID()
    {
        int intWLID;
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where WLID in (select max(workFlow1.MLID) from WorkFlow as workFlow)";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        WorkFlow workFlow = (WorkFlow)lst[0];

        intWLID = workFlow.WLID;

        return intWLID;
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strUserName = projectMember.UserName.Trim();
        return strUserName.Trim();
    }

    protected decimal SumGoodsPurchaseOrderAmount(string strPOID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from GoodsPurRecord as goodsPurRecord where goodsPurRecord.POID = " + strPOID;
        GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
        lst = goodsPurRecordBLL.GetAllGoodsPurRecords(strHQL);

        GoodsPurRecord goodsPurRecord = new GoodsPurRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            goodsPurRecord = (GoodsPurRecord)lst[i];
            deAmount += goodsPurRecord.Number * goodsPurRecord.Price;
        }

        return deAmount;
    }

    protected void UpdateGoodsPurchaseOrderAmount(string strPOID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsPurchaseOrder as goodsPurchaseOrder where goodsPurchaseOrder.POID = " + strPOID;
        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        lst = goodsPurchaseOrderBLL.GetAllGoodsPurchaseOrders(strHQL);

        GoodsPurchaseOrder goodsPurchaseOrder = (GoodsPurchaseOrder)lst[0];

        goodsPurchaseOrder.Amount = deAmount;

        try
        {
            goodsPurchaseOrderBLL.UpdateGoodsPurchaseOrder(goodsPurchaseOrder, int.Parse(strPOID));
        }
        catch
        {
        }
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
}


