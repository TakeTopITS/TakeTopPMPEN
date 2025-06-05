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


public partial class TTGoodsSaleOrderDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strSOID = Request.QueryString["SOID"];
        string strUserName;
        string strUserCode = Session["UserCode"].ToString();


        //this.Title = "物料销售单处理";

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_ArrivalTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SaleTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

            ShareClass.LoadCurrencyType(DL_CurrencyType);

            TB_SalesCode.Text = strUserCode;
            LB_SalesName.Text = strUserName;
            
            ShareClass.LoadWFTemplate(strUserCode, "MaterialSales", DL_TemName);

            ShareClass.LoadCustomer(DL_Customer, strUserCode);

            LoadGoodsSaleOrder(strSOID);
            LoadRelatedConstract(strSOID);
    
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
            string strHQL, strID, strSOID;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strSOID = LB_SOID.Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.ID = " + strID;

            GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
            lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);
            GoodsSaleRecord goodsSaleRecord = (GoodsSaleRecord)lst[0];

            TB_GoodsCode.Text = goodsSaleRecord.GoodsCode;
            TB_GoodsName.Text = goodsSaleRecord.GoodsName;
            TB_ModelNumber.Text = goodsSaleRecord.ModelNumber.Trim();
            TB_Spec.Text = goodsSaleRecord.Spec;
            TB_SaleReason.Text = goodsSaleRecord.SaleReason;
            NB_Price.Amount = goodsSaleRecord.Price;
            DL_Type.SelectedValue = goodsSaleRecord.Type;
            DL_Unit.SelectedValue = goodsSaleRecord.Unit;
            NB_Number.Amount = goodsSaleRecord.Number;


            int intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao"), strSOID);

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
    
    protected void LoadGoodsSaleOrder(string strSOID)
    {
        string strHQL;
        IList lst;
        string strUserCode = LB_UserCode.Text;

        int intWLNumber;

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.SOID = " + strSOID;
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);
        GoodsSaleOrder goodsSaleOrder = (GoodsSaleOrder)lst[0];

        LB_SOID.Text = goodsSaleOrder.SOID.ToString();
        TB_SOName.Text = goodsSaleOrder.SOName.Trim();
        DL_Customer.SelectedValue = goodsSaleOrder.CustomerCode;
        DLC_SaleTime.Text = goodsSaleOrder.SaleTime.ToString("yyyy-MM-dd");
        DLC_ArrivalTime.Text = goodsSaleOrder.ArrivalTime.ToString("yyyy-MM-dd");
        NB_Amount.Amount = goodsSaleOrder.Amount;

        DL_CurrencyType.SelectedValue = goodsSaleOrder.CurrencyType;

        TB_Comment.Text = goodsSaleOrder.Comment.Trim();
        DL_SOStatus.SelectedValue = goodsSaleOrder.Status.Trim();
        TB_SalesCode.Text = goodsSaleOrder.OperatorCode.Trim();
        LB_SalesName.Text = goodsSaleOrder.OperatorName.Trim();

        DL_RelatedType.SelectedValue = goodsSaleOrder.RelatedType.Trim();
        NB_RelatedID.Amount = goodsSaleOrder.RelatedID;

        LoadGoodsSaleOrderDetail(strSOID);

        BT_Update.Enabled = false;
        BT_Delete.Enabled = false;

        intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao"), strSOID);

        if (intWLNumber == 0)
        {
            BT_UpdateSO.Enabled = true;

            BT_New.Enabled = true;
        }
        else
        {
            BT_UpdateSO.Enabled = false;


            BT_New.Enabled = false;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;
        }


        TB_WLName.Text = LanguageHandle.GetWord("XiaoShou") + goodsSaleOrder.SOName.Trim() + LanguageHandle.GetWord("ShenQing");

        LoadRelatedWL("MaterialSales", LanguageHandle.GetWord("WuLiao"), goodsSaleOrder.SOID);

        BT_SubmitApply.Enabled = true;
    }

    protected void DL_RelatedType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strRelatedType;

        strRelatedType = DL_RelatedType.SelectedValue.Trim();

        if (strRelatedType == "Other")
        {
            BT_Select.Visible = false;
            NB_RelatedID.Amount = 0;
        }

        if (strRelatedType == "Project")
        {
            BT_Select.Visible = true;
        }
    }

    protected void BT_UpdateSO_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strSOID, strUserCode, strSOName, strSalesCode, strSalesName, strComment, strCurrencyType;
        DateTime dtSaleTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();

        strSOID = LB_SOID.Text.Trim();
        strSOName = TB_SOName.Text.Trim();
        strSalesCode = TB_SalesCode.Text.Trim();
        strSalesName = LB_SalesName.Text.Trim();
        dtSaleTime = DateTime.Parse(DLC_SaleTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);
        deAmount = NB_Amount.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();
        strStatus = DL_SOStatus.SelectedValue.Trim();

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.SOID = " + strSOID;
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        GoodsSaleOrder goodsSaleOrder = (GoodsSaleOrder)lst[0];

        goodsSaleOrder.SOName = strSOName;

        goodsSaleOrder.SalesCode = strSalesCode;
        try
        {
            goodsSaleOrder.SalesName = ShareClass.GetUserName(strSalesCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXSRDMBZCWCRJC") + "')", true);
            return;
        }

        goodsSaleOrder.SaleTime = dtSaleTime;
        goodsSaleOrder.ArrivalTime = dtArrivalTime;
        goodsSaleOrder.Amount = deAmount;
        goodsSaleOrder.CurrencyType = strCurrencyType;
        goodsSaleOrder.Comment = strComment;
        goodsSaleOrder.Status = strStatus;

        if (DL_RelatedType.SelectedValue.Trim() == "Other")
        {
            goodsSaleOrder.RelatedType = "Other";
            goodsSaleOrder.RelatedID = 0;
        }

        if (DL_RelatedType.SelectedValue.Trim() == "Project")
        {
            goodsSaleOrder.RelatedType = "Project";
            goodsSaleOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());
        }

        try
        {
            goodsSaleOrderBLL.UpdateGoodsSaleOrder(goodsSaleOrder, int.Parse(strSOID));


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strRecordID, strSOID, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strCustomer, strCustomerPhone, strUnitName;
        decimal decNumber;
        DateTime dtBuyTime;
        decimal dePrice;
        string strApplicantCode, strSaleReason;
        string strRelatedType, strRelatedID;

        strSOID = LB_SOID.Text.Trim();
        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        strRelatedID = NB_RelatedID.Amount.ToString();

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        decNumber = NB_Number.Amount;
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strSaleReason = TB_SaleReason.Text.Trim();
        dePrice = NB_Price.Amount;
        dtBuyTime = DateTime.Now;



        if (strType == "" | strGoodsName == "" | strSpec == "" )
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
            GoodsSaleRecord goodsSaleRecord = new GoodsSaleRecord();

            goodsSaleRecord.SOID = int.Parse(strSOID);
            goodsSaleRecord.Type = strType;
            goodsSaleRecord.GoodsCode = strGoodsCode;
            goodsSaleRecord.GoodsName = strGoodsName;
            goodsSaleRecord.Number = decNumber;
            goodsSaleRecord.Unit = strUnitName;
            goodsSaleRecord.Number = decNumber;
            goodsSaleRecord.Price = dePrice;
            goodsSaleRecord.ModelNumber = strModelNumber;
            goodsSaleRecord.Spec = strSpec;
            goodsSaleRecord.SaleReason = strSaleReason;

            goodsSaleRecord.CheckOutNumber = 0;
            goodsSaleRecord.DeliveryNumber = 0;

            try
            {
                goodsSaleRecordBLL.AddGoodsSaleRecord(goodsSaleRecord);

                strRecordID = ShareClass.GetMyCreatedMaxGoodsSaleRecordID(strSOID);
                LB_ID.Text = strRecordID;

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                LoadGoodsSaleOrderDetail(strSOID);

                NB_Amount.Amount = SumGoodsSaleOrderAmount(strSOID);
                UpdateGoodsSaleOrderAmount(strSOID, NB_Amount.Amount);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCJC") + "')", true);
            }
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strCustomer, strCustomerPhone, strSaleReason, strUnitName;
        DateTime dtBuyTime;
        decimal dePrice, deNumber;
        string strApplicantCode;
        string strRelatedType, strRelatedID;

        string strID, strSOID, strSalesCode, strSalesName;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();

        strSOID = LB_SOID.Text.Trim();
        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        strRelatedID = NB_RelatedID.Amount.ToString();

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strSaleReason = TB_SaleReason.Text.Trim();
        dePrice = NB_Price.Amount;
        deNumber = NB_Number.Amount;;
        dtBuyTime = DateTime.Now;

        strSalesCode = TB_SalesCode.Text.Trim();
        strSalesName = LB_SalesName.Text.Trim();


        if (strType == "" | strGoodsName == "" | strSpec == "" )
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
            strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.ID = " + strID;
            lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);
            GoodsSaleRecord goodsSaleRecord = (GoodsSaleRecord)lst[0];

            goodsSaleRecord.SOID = int.Parse(strSOID);
            goodsSaleRecord.Type = strType;
            goodsSaleRecord.GoodsCode = strGoodsCode;
            goodsSaleRecord.GoodsName = strGoodsName;
            goodsSaleRecord.Number = deNumber;
            goodsSaleRecord.Unit = strUnitName;
            goodsSaleRecord.Price = dePrice;
            goodsSaleRecord.ModelNumber = strModelNumber;
            goodsSaleRecord.Spec = strSpec;
            goodsSaleRecord.SaleReason = strSaleReason;

            try
            {
                goodsSaleRecordBLL.UpdateGoodsSaleRecord(goodsSaleRecord, int.Parse(strID));

                LoadGoodsSaleOrderDetail(strSOID);

                NB_Amount.Amount = SumGoodsSaleOrderAmount(strSOID);
                UpdateGoodsSaleOrderAmount(strSOID, NB_Amount.Amount);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
            }
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strSOID = LB_SOID.Text.Trim();

        string strHQL;
        IList lst;

        GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
        strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.ID = " + strID;
        lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);
        GoodsSaleRecord goodsSaleRecord = (GoodsSaleRecord)lst[0];

        try
        {
            goodsSaleRecordBLL.DeleteGoodsSaleRecord(goodsSaleRecord);

            LoadGoodsSaleOrderDetail(strSOID);

            NB_Amount.Amount = SumGoodsSaleOrderAmount(strSOID);
            UpdateGoodsSaleOrderAmount(strSOID, NB_Amount.Amount);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText, strSOID;
        string strWLID, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strSOID = LB_SOID.Text.Trim();

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = ShareClass.GetUserName(strCreatorCode);

        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC") + "');</script>");
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
        workFlow.RelatedType = LanguageHandle.GetWord("WuLiao");
        workFlow.Status = "New";
        workFlow.RelatedID = int.Parse(strSOID);
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

            LoadRelatedWL(strWLType, LanguageHandle.GetWord("WuLiao"), int.Parse(strSOID));

            UpdateGoodsGoodsSaleStatus(strSOID, "InProgress");
            DL_SOStatus.SelectedValue = "InProgress";

            strCmdText = "select * from T_GoodsSaleOrder where SOID = " + strSOID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsSaleOrder", strXMLFile2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPXSSSCCG") + "')", true);
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPXSSSBKNGZLMCGCZD25GHZJC") + "')", true);
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

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'MaterialSales'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected void DL_SOStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSOID, strStatus, strUserCode;

        strSOID = LB_SOID.Text.Trim();
        strStatus = DL_SOStatus.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strSOID != "")
        {
            UpdateGoodsGoodsSaleStatus(strSOID, strStatus);
            LoadGoodsSaleOrder(strUserCode);
        }
    }



    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void UpdateGoodsGoodsSaleStatus(string strSOID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.SOID = " + strSOID;
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        GoodsSaleOrder goodsSaleOrder = (GoodsSaleOrder)lst[0];

        goodsSaleOrder.Status = strStatus;

        try
        {
            goodsSaleOrderBLL.UpdateGoodsSaleOrder(goodsSaleOrder, int.Parse(strSOID));
        }
        catch
        {
        }
    }

    protected void UpdateGoodsGoodsSaleOrderDetailStatus(string strID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.ID = " + strID;
        GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
        lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);

        GoodsSaleRecord goodsSaleRecord = (GoodsSaleRecord)lst[0];


        try
        {
            goodsSaleRecordBLL.UpdateGoodsSaleRecord(goodsSaleRecord, int.Parse(strID));
        }
        catch
        {
        }
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

    protected void LoadGoodsSaleOrderDetail(string strSOID)
    {
        LB_GoodsOwner.Text = LanguageHandle.GetWord("XiaoShouDan") + ": " + strSOID + LanguageHandle.GetWord("MingXi");

        string strHQL = "Select * from T_GoodsSaleRecord where SOID = " + strSOID + " Order by ID DESC";
        SqlConnection myConnection = new SqlConnection(
          ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
        SqlCommand myCommand = new SqlCommand(strHQL, myConnection);

        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(strHQL, myConnection);

        sda.Fill(ds, "T_GoodsSaleRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadRelatedConstract(string strSOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.Status <> 'Archived'";
        strHQL += " and constract.ConstractCode in (select constractRelatedGoodsSaleOrder.ConstractCode from ConstractRelatedGoodsSaleOrder as constractRelatedGoodsSaleOrder where constractRelatedGoodsSaleOrder.SOID = " + strSOID + ")";
        strHQL += " Order by constract.SignDate DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadCustomer(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "from Customer as customer ";
        strHQL += " Where (customer.CreatorCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " or (customer.CustomerCode in (Select customerRelatedUser.CustomerCode from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " Or customer.CreatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode In  " + strDepartString + ")";
        strHQL += " Order by customer.CreateDate DESC";

        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DL_Customer.DataSource = lst;
        DL_Customer.DataBind();

        DL_Customer.Items.Insert(0, new ListItem("--Select--", ""));
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


    protected decimal SumGoodsSaleOrderAmount(string strSOID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.SOID = " + strSOID;
        GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
        lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);

        GoodsSaleRecord goodsSaleRecord = new GoodsSaleRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            goodsSaleRecord = (GoodsSaleRecord)lst[i];
            deAmount += goodsSaleRecord.Number * goodsSaleRecord.Price;
        }

        return deAmount;
    }

    protected void UpdateGoodsSaleOrderAmount(string strSOID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.SOID = " + strSOID;
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        GoodsSaleOrder goodsSaleOrder = (GoodsSaleOrder)lst[0];

        goodsSaleOrder.Amount = deAmount;

        try
        {
            goodsSaleOrderBLL.UpdateGoodsSaleOrder(goodsSaleOrder, int.Parse(strSOID));
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

