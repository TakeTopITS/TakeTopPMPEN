using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTMakeAssetPurchase : System.Web.UI.Page
{
    string strUserCode;
    string strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID;
    private string strMainTableCanAdd, strDetailTableCanAdd, strMainTableCanEdit, strMainTableCanDelete, strDetailTableCanEdit, strDetailTableCanDelete;
    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName, strDepartString;
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产登记入库", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //WorkFlow,如果是由工作流启动的业务，那么给下面三个变量赋值
        strRelatedWorkflowID = Request.QueryString["RelatedWorkflowID"];
        strRelatedWorkflowStepID = Request.QueryString["RelatedWorkflowStepID"];
        strRelatedWorkflowStepDetailID = Request.QueryString["RelatedWorkflowStepDetailID"];
        strMainTableCanAdd = Request.QueryString["MainTableCanAdd"];
        strDetailTableCanAdd = Request.QueryString["DetailTableCanAdd"];
        strMainTableCanEdit = Request.QueryString["MainTableCanEdit"];
        strMainTableCanDelete = Request.QueryString["MainTableCanDelete"];
        strDetailTableCanEdit = Request.QueryString["DetailTableCanEdit"];
        strDetailTableCanDelete = Request.QueryString["DetailTableCanDelete"];

        //从流程中打开的业务单
        strToDoWLID = Request.QueryString["WLID"];
        strToDoWLDetailID= Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        //WorkFlow,如果是由工作流启动的业务,初始化相关按钮
        ShareClass.InitialWorkflowRelatedModule(strRelatedWorkflowID, strRelatedWorkflowStepID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strMainTableCanAdd, strDetailTableCanAdd, strMainTableCanEdit, strDetailTableCanEdit);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ArrivalTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_PurTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from AssetType as assetType Order by assetType.SortNumber ASC";
            AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
            lst = assetTypeBLL.GetAllAssetTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            ShareClass.LoadCurrencyType(DL_CurrencyType);
            ShareClass.LoadVendorList(DL_VendorList, strUserCode);

            ShareClass.LoadWFTemplate(strUserCode, "AssetProcurement", DL_TemName);

            TB_PurManCode.Text = strUserCode;
            LB_PurManName.Text = strUserName;

            TB_ApplicantCode.Text = strUserCode;
            LB_ApplicantName.Text = strUserName;

            LoadAssetPurchaseOrder(strUserCode);

            ShareClass.InitialInvolvedProjectTree(TreeView2, strUserCode);
            LB_DepartString.Text = strDepartString;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        try
        {
            if (treeNode.Target != "0")
            {
                strDepartCode = treeNode.Target.Trim();
                strDepartName = ShareClass.GetDepartName(strDepartCode);

                ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShowWithGrandson('popwindow','true','popDetailWindow','popDeparentUserSelectWindow') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShowWithGrandson('popwindow','true','popDetailWindow','popDeparentUserSelectWindow') ", true);
        }

        TreeView1.SelectedNode.Selected = false;
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        TreeView2.SelectedNode.Selected = false;
    }
    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        TB_ApplicantCode.Text = strUserCode;
        LB_ApplicantName.Text = GetUserName(strUserCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_SelectUser_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShowWithGrandson('popwindow','true','popDetailWindow','popDeparentUserSelectWindow') ", true);
    }

    protected void BT_CloseSelectUser_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPOID, strPORelatedType;
            IList lst;
            int intWLNumber;

            strPOID = e.Item.Cells[3].Text.Trim();

            intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
            if (intWLNumber > 0)
            {
                BT_NewMain.Visible = false;
                BT_NewDetail.Visible = false;

                BT_SubmitApply.Enabled = false;
            }
            else
            {
                BT_NewMain.Visible = true;
                BT_NewDetail.Visible = true;

                BT_SubmitApply.Enabled = true;
            }

            //WorkFlow,如果此单和工作流相关，那么依工作流状态决定能否保存单据数据
            string strCreateUserCode = getAssetPurchaseOrderCreatorCode(strPOID);
            ShareClass.MainTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("ZiChanCaiGouChan"), strPOID, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strMainTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("AssetProcurement", "Assets", strPOID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                BT_NewMain.Visible = true;
                BT_NewDetail.Visible = true;
            }

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid5.Items.Count; i++)
                {
                    DataGrid5.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
                AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
                lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
                AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

                LB_POID.Text = assetPurchaseOrder.POID.ToString();
                TB_POName.Text = assetPurchaseOrder.POName.Trim();
                DLC_PurTime.Text = assetPurchaseOrder.PurTime.ToString("yyyy-MM-dd");
                DLC_ArrivalTime.Text = assetPurchaseOrder.ArrivalTime.ToString("yyyy-MM-dd");
                NB_Amount.Amount = assetPurchaseOrder.Amount;
                DL_CurrencyType.SelectedValue = assetPurchaseOrder.CurrencyType;
                TB_Comment.Text = assetPurchaseOrder.Comment.Trim();
                DL_POStatus.SelectedValue = assetPurchaseOrder.Status.Trim();
                TB_PurManCode.Text = assetPurchaseOrder.OperatorCode.Trim();
                LB_PurManName.Text = assetPurchaseOrder.OperatorName.Trim();

                DL_RelatedType.SelectedValue = assetPurchaseOrder.RelatedType.Trim();
                NB_RelatedID.Amount = assetPurchaseOrder.RelatedID;
                strPORelatedType = DL_RelatedType.SelectedValue.Trim();

                LoadAssetPurchaseOrderDetail(strPOID);
                LoadRelatedConstract(strPOID);

                if (strPORelatedType == "Other")
                {
                    BT_Select.Visible = false;
                }
                else
                {
                    BT_Select.Visible = true;
                }


                TB_WLName.Text = LanguageHandle.GetWord("GouMai") + assetPurchaseOrder.POName.Trim() + LanguageHandle.GetWord("ShenQing");

                LoadRelatedWL("AssetProcurement", "Assets", assetPurchaseOrder.POID);

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Assign")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                strUserCode = LB_UserCode.Text.Trim();

                intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
                if (intWLNumber > 0)
                {
                    return;
                }

                //Workflow,如果存在关联工作流，那么要执行下面的代码
                if (!ShareClass.MainTableDeleteWorkflowRelatedModule(strUserCode, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, strMainTableCanDelete))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZSBNWQSCQJC") + "')", true);
                    return;
                }

                try
                {
                    strHQL = "Delete From T_AssetPurchaseOrder Where POID = " + strPOID;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_AssetPurRecord Where POID = " + strPOID;
                    ShareClass.RunSqlCommand(strHQL);

                    //Workflow,删除流程模组关联记录
                    ShareClass.DeleteModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("ZiChanCaiGouChan"), strPOID);


                    BT_SubmitApply.Enabled = false;

                    LoadAssetPurchaseOrder(strUserCode);
                    LoadAssetPurchaseOrderDetail(strPOID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
                }
            }
        }
    }

    //Workflow,取得单据创建人代码
    protected string getAssetPurchaseOrderCreatorCode(string strPOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        return assetPurchaseOrder.OperatorCode.Trim();
    }


    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_POID.Text = "";

        LoadAssetPurchaseOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strPOID;

        strPOID = LB_POID.Text.Trim();

        if (strPOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }
    }

    protected void AddMain()
    {
        string strPOID, strPOName, strPurManCode, strPurManName, strOperatorCode, strOperatorName, strCurrencyType, strComment;
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
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();

        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        AssetPurchaseOrder assetPurchaseOrder = new AssetPurchaseOrder();

        assetPurchaseOrder.POName = strPOName;

        assetPurchaseOrder.PurManCode = strPurManCode;
        try
        {
            assetPurchaseOrder.PurManName = GetUserName(strPurManCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCGRDMBZCWCRJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        assetPurchaseOrder.OperatorCode = strOperatorCode;
        assetPurchaseOrder.OperatorName = strOperatorName;
        assetPurchaseOrder.PurTime = dtPurTime;
        assetPurchaseOrder.ArrivalTime = dtArrivalTime;
        assetPurchaseOrder.Amount = 0;
        assetPurchaseOrder.CurrencyType = strCurrencyType;
        assetPurchaseOrder.Comment = strComment;
        assetPurchaseOrder.Status = "New";

        assetPurchaseOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        assetPurchaseOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        try
        {
            assetPurchaseOrderBLL.AddAssetPurchaseOrder(assetPurchaseOrder);

            strPOID = ShareClass.GetMyCreatedMaxAssetPurchaseOrderID(strOperatorCode);
            LB_POID.Text = strPOID;


            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("ZiChanCaiGouChan"), strPOID);

            NB_Amount.Amount = 0;


            TB_WLName.Text = LanguageHandle.GetWord("GouMai") + strPOName + LanguageHandle.GetWord("ShenQing");

            BT_SubmitApply.Enabled = true;

            LoadAssetPurchaseOrder(strPurManCode);
            LoadAssetPurchaseOrderDetail(strPOID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCKNCGMCZD50GHZHBZZSZD100GHZGDJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strPOID, strUserCode, strPOName, strPurManCode, strPurManName, strCurrencyType, strComment;
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
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        assetPurchaseOrder.POName = strPOName;

        assetPurchaseOrder.PurManCode = strPurManCode;
        try
        {
            assetPurchaseOrder.PurManName = GetUserName(strPurManCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCGRDMBZCWCRJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        assetPurchaseOrder.PurTime = dtPurTime;
        assetPurchaseOrder.ArrivalTime = dtArrivalTime;
        assetPurchaseOrder.Amount = deAmount;
        assetPurchaseOrder.CurrencyType = strCurrencyType;
        assetPurchaseOrder.Comment = strComment;
        assetPurchaseOrder.Status = strStatus;

        assetPurchaseOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        assetPurchaseOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        try
        {
            assetPurchaseOrderBLL.UpdateAssetPurchaseOrder(assetPurchaseOrder, int.Parse(strPOID));
            LoadAssetPurchaseOrder(strUserCode);

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("AssetProcurement", "Assets", strPOID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select POID as DetailPOID, * from T_AssetPurchaseOrder where POID = " + strPOID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("AssetProcurement", "Assets", strPOID);
                }

                if (strToDoWLID != null)
                {
                    if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        IList lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void DL_VendorList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strVendorCode, strVendorPhone;

        strVendorCode = DL_VendorList.SelectedValue.Trim();
        strHQL = "Select COALESCE(Tel1,'') From T_Vendor Where VendorCode = " + "'" + strVendorCode + "'";

        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Vendor");
            if (ds.Tables[0].Rows.Count == 0)
            {
                strHQL = "Select COALESCE(PhoneNum,'') From T_BMSupplierInfo Where Code = " + "'" + strVendorCode + "'";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
            }

            strVendorPhone = ds.Tables[0].Rows[0][0].ToString();

            TB_SupplierPhone.Text = strVendorPhone;
            TB_Supplier.Text = DL_VendorList.SelectedItem.Text;
        }
        catch
        {
            TB_SupplierPhone.Text = "";
            TB_Supplier.Text = "";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }


    protected void BT_FindAsset_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strAssetCode, strAssetName, strModelNumber, strSpec;
        string strDepartString;

        TabContainer2.ActiveTabIndex = 1;


        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strAssetCode = "%" + strAssetCode + "%";
        strAssetName = "%" + strAssetName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "Select * From T_Asset  Where AssetCode Like " + "'" + strAssetCode + "'" + "and AssetName like " + "'" + strAssetName + "'";
        strHQL += " and ModelNumber Like " + "'" + strModelNumber + "'" + " and Spec Like " + "'" + strSpec + "'";
        strHQL += " and Number > 0";
        strHQL += " and Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Asset");

        DataGrid6.DataSource = ds;
        DataGrid6.DataBind();

        strHQL = "From Item as item Where item.ItemCode Like " + "'" + strAssetCode + "'" + " and item.ItemName like " + "'" + strAssetName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Asset'";

        ItemBLL itemBLL = new ItemBLL();
        IList lst = itemBLL.GetAllItems(strHQL);

        DataGrid7.DataSource = lst;
        DataGrid7.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = e.Item.Cells[0].Text.Trim();

            for (int i = 0; i < DataGrid6.Items.Count; i++)
            {
                DataGrid6.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From Asset as asset where asset.ID = " + strID;
            AssetBLL assetBLL = new AssetBLL();
            lst = assetBLL.GetAllAssets(strHQL);

            Asset asset = (Asset)lst[0];

            TB_AssetCode.Text = asset.AssetCode.Trim();
            TB_AssetName.Text = asset.AssetName.Trim();
            TB_ModelNumber.Text = asset.ModelNumber.Trim();
            TB_Spec.Text = asset.Spec.Trim();
            NB_Number.Amount = asset.Number;
            DL_Unit.SelectedValue = asset.UnitName;
            NB_Price.Amount = asset.Price;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_AssetCode.Text = "";
        TB_AssetName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";

        NB_Number.Amount = 0;
        NB_Price.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid7.Items.Count; i++)
            {
                DataGrid7.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_AssetCode.Text = item.ItemCode;
                TB_AssetName.Text = item.ItemName;
                try
                {
                    DL_Type.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_Type.SelectedValue = "";
                }
                DL_Unit.SelectedValue = item.Unit;
                TB_Spec.Text = item.Specification;
                NB_Price.Amount = item.PurchasePrice;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPOID;
            IList lst;

            strPOID = LB_POID.Text.Trim();

            int intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
            if (intWLNumber > 0)
            {
                BT_NewDetail.Visible = false;
            }
            else
            {
                BT_NewDetail.Visible = true;
            }

            //WorkFlow,如果此单和工作流相关，那么依工作流状态决定能否保存单据数据
            ShareClass.DetailTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("ZiChanCaiGouChan"), strPOID, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strDetailTableCanAdd, strDetailTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("AssetProcurement", "Assets", strPOID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                BT_NewMain.Visible = true;
                BT_NewDetail.Visible = true;
            }


            string strID = e.Item.Cells[2].Text.Trim();
            LB_ID.Text = strID;

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;

                AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
                lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
                AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

                TB_AssetCode.Text = assetPurRecord.AssetCode;
                TB_AssetName.Text = assetPurRecord.AssetName;
                TB_ModelNumber.Text = assetPurRecord.ModelNumber;
                TB_Spec.Text = assetPurRecord.Spec;
                TB_PurReason.Text = assetPurRecord.PurReason;
                NB_Price.Amount = assetPurRecord.Price;
                TB_ApplicantCode.Text = assetPurRecord.ApplicantCode;
                TB_Supplier.Text = assetPurRecord.Supplier;
                TB_SupplierPhone.Text = assetPurRecord.SupplierPhone;
                LB_ApplicantName.Text = GetUserName(assetPurRecord.ApplicantCode);
                DL_Type.SelectedValue = assetPurRecord.Type;
                DL_Unit.SelectedValue = assetPurRecord.Unit;
                NB_Number.Amount = assetPurRecord.Number;
                DL_Status.SelectedValue = assetPurRecord.Status.Trim();

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    return;
                }

                AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
                strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
                lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
                AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

                try
                {
                    assetPurRecordBLL.DeleteAssetPurRecord(assetPurRecord);

                    LoadAssetPurchaseOrderDetail(strPOID);

                    NB_Amount.Amount = SumPurchaseOrderAmount(strPOID);
                    UpdatePurchaseOrderAmount(strPOID, NB_Amount.Amount);

                    //Workflow,如果存在关联工作流，那么要执行下面的代码
                    string strCreateUserCode;
                    strCreateUserCode = getAssetPurchaseOrderCreatorCode(strPOID);
                    if (!ShareClass.DetailTableDeleteWorkflowRelatedModule(strUserCode, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, strDetailTableCanDelete))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZSBNWQSCQJC") + "')", true);
                        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                        return;
                    }

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("AssetProcurement", "Assets", strPOID, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText = "select POID as DetailPOID, * from T_AssetPurchaseOrder where POID = " + strPOID;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("AssetProcurement", "Assets", strPOID);
                        }

                        if (strToDoWLID != null)
                        {
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }

                        if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                        {
                            strCmdText = "select * from T_AssetPurRecord where POID = " + strPOID;
                            ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strPOID;

        strPOID = LB_POID.Text.Trim();

        if (strPOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strPOID = LB_POID.Text.Trim();
        int intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
        if (intWLNumber > 0 & strToDoWLID == null)
        {
            BT_SubmitApply.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCZGLDGZLJLBNSCJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            return;
        }
        else
        {
            BT_SubmitApply.Enabled = true;
        }

        string strDetailID;

        strDetailID = LB_ID.Text.Trim();

        if (strDetailID == "")
        {
            AddDetail();
        }
        else
        {
            UpdateDetail();
        }
    }


    protected void AddDetail()
    {
        string strRecordID, strPOID, strType, strAssetCode, strAssetName, strModelNumber, strSpec, strStatus;
        string strSupplier, strSupplierPhone, strUnitName;
        decimal decNumber;
        DateTime dtBuyTime;
        decimal dePrice;
        string strApplicantCode, strPurReason;


        strPOID = LB_POID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        decNumber = NB_Number.Amount;
        strSpec = TB_Spec.Text.Trim();
        strPurReason = TB_PurReason.Text.Trim();
        dePrice = NB_Price.Amount;
        strApplicantCode = TB_ApplicantCode.Text.Trim();
        dtBuyTime = DateTime.Now;
        strSupplier = TB_Supplier.Text.Trim();
        strSupplierPhone = TB_SupplierPhone.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        if (strType == "" | strAssetName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
            AssetPurRecord assetPurRecord = new AssetPurRecord();

            assetPurRecord.POID = int.Parse(strPOID);
            assetPurRecord.Type = strType;
            assetPurRecord.AssetCode = strAssetCode;
            assetPurRecord.AssetName = strAssetName;
            assetPurRecord.Number = decNumber;
            assetPurRecord.Unit = strUnitName;
            assetPurRecord.Number = decNumber;
            assetPurRecord.Price = dePrice;
            assetPurRecord.Amount = dePrice * decNumber;
            assetPurRecord.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            assetPurRecord.ModelNumber = strModelNumber;
            assetPurRecord.Spec = strSpec;
            assetPurRecord.PurReason = strPurReason;
            assetPurRecord.PurTime = dtBuyTime;
            assetPurRecord.Status = strStatus;
            assetPurRecord.RelatedType = "Other";
            assetPurRecord.RelatedID = 0;

            assetPurRecord.ApplicantCode = strApplicantCode;
            try
            {
                assetPurRecord.ApplicantName = GetUserName(strApplicantCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSRDMBZCWCRJC") + "')", true);
                return;
            }

            assetPurRecord.Supplier = TB_Supplier.Text.Trim();
            assetPurRecord.SupplierPhone = TB_SupplierPhone.Text.Trim();

            try
            {
                assetPurRecordBLL.AddAssetPurRecord(assetPurRecord);

                strRecordID = ShareClass.GetMyCreatedMaxAssetPurRecordID(strPOID);
                LB_ID.Text = strRecordID;

                LoadAssetPurchaseOrderDetail(strPOID);

                NB_Amount.Amount = SumPurchaseOrderAmount(strPOID);
                UpdatePurchaseOrderAmount(strPOID, NB_Amount.Amount);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("AssetProcurement", "Assets", strPOID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText = "select POID as DetailPOID, * from T_AssetPurchaseOrder where POID = " + strPOID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("AssetProcurement", "Assets", strPOID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_AssetPurRecord where POID = " + strPOID;
                        ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdateDetail()
    {
        string strType, strAssetCode, strAssetName, strModelNumber, strSpec, strStatus;
        string strSupplier, strSupplierPhone, strPurReason, strUnitName;
        DateTime dtBuyTime;
        decimal dePrice, deNumber;
        string strApplicantCode;

        string strID, strPOID, strPurManCode, strPurManName;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();

        strPOID = LB_POID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
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
        strStatus = DL_Status.SelectedValue;

        if (strType == "" | strAssetName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
            strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
            lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
            AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

            assetPurRecord.POID = int.Parse(strPOID);
            assetPurRecord.Type = strType;
            assetPurRecord.AssetCode = strAssetCode;
            assetPurRecord.AssetName = strAssetName;
            assetPurRecord.Number = deNumber;
            assetPurRecord.Unit = strUnitName;
            assetPurRecord.Price = dePrice;
            assetPurRecord.Amount = dePrice * deNumber;
            assetPurRecord.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            assetPurRecord.ModelNumber = strModelNumber;
            assetPurRecord.Spec = strSpec;
            assetPurRecord.PurReason = strPurReason;
            assetPurRecord.PurTime = dtBuyTime;
            assetPurRecord.Status = strStatus;

            assetPurRecord.RelatedType = "Other";
            assetPurRecord.RelatedID = 0;

            assetPurRecord.ApplicantCode = strApplicantCode;
            try
            {
                assetPurRecord.ApplicantName = GetUserName(strApplicantCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSZDMBZCWCRJC") + "')", true);
                return;
            }

            assetPurRecord.Supplier = TB_Supplier.Text.Trim();
            assetPurRecord.SupplierPhone = TB_SupplierPhone.Text.Trim();

            try
            {
                assetPurRecordBLL.UpdateAssetPurRecord(assetPurRecord, int.Parse(strID));

                LoadAssetPurchaseOrderDetail(strPOID);

                NB_Amount.Amount = SumPurchaseOrderAmount(strPOID);
                UpdatePurchaseOrderAmount(strPOID, NB_Amount.Amount);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("AssetProcurement", "Assets", strPOID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText = "select POID as DetailPOID, * from T_AssetPurchaseOrder where POID = " + strPOID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("AssetProcurement", "Assets", strPOID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_AssetPurRecord where POID = " + strPOID;
                        ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            }
        }
    }

    protected void BT_AllPurAsset_Click(object sender, EventArgs e)
    {
        //    LB_AssetOwner.Text = LanguageHandle.GetWord("SYCGZCLB") + ": ";
        //    LB_AssetOwner.Visible = true;

        string strUserCode = LB_UserCode.Text.Trim();

        LoadAssetPurchaseOrder(strUserCode);
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC") + "');</script>");

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

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
        workFlow.RelatedType = "Assets";
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

            LoadRelatedWL(strWLType, "Assets", int.Parse(strPOID));

            UpdateAssetPurchaseStatus(strPOID, "InProgress");
            DL_POStatus.SelectedValue = "InProgress";

            strCmdText = "select POID as DetailPOID, * from T_AssetPurchaseOrder where POID = " + strPOID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_AssetPurchaseOrder", strXMLFile2);

            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strWLID, "0", "0", LanguageHandle.GetWord("ZiChanCaiGouChan"), strPOID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCCGSSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCCGSSBKNGZLMCGCZD25GHZJC") + "')", true);
            return "0";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

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

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'AssetProcurement'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void DL_POStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPOID, strStatus, strUserCode;

        strPOID = LB_POID.Text.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strPOID != "")
        {
            UpdateAssetPurchaseStatus(strPOID, strStatus);
            LoadAssetPurchaseOrder(strUserCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strStatus;

        strID = LB_ID.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        if (strID != "")
        {
            UpdateAssetPurchaseOrderDetailStatus(strID, strStatus);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void UpdateAssetPurchaseStatus(string strPOID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        assetPurchaseOrder.Status = strStatus;

        try
        {
            assetPurchaseOrderBLL.UpdateAssetPurchaseOrder(assetPurchaseOrder, int.Parse(strPOID));
        }
        catch
        {
        }
    }

    protected void UpdateAssetPurchaseOrderDetailStatus(string strID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);

        AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

        assetPurRecord.Status = strStatus;

        try
        {
            assetPurRecordBLL.UpdateAssetPurRecord(assetPurRecord, int.Parse(strID));
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

    protected void LoadAssetPurchaseOrder(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        //Workflow,对流程相关模组作判断
        if (strRelatedWorkflowID == null)
        {
            strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.OperatorCode = " + "'" + strOperatorCode + "'";
        }
        else
        {
            strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where ";
            strHQL += "assetPurchaseOrder.POID in (Select workFlowRelatedModule.RelatedID  From WorkFlowRelatedModule as workFlowRelatedModule Where workFlowRelatedModule.RelatedModuleName = 'AssetPurchaseOrder' and workFlowRelatedModule.WorkflowID =" + strRelatedWorkflowID + ")";   
        }
        strHQL += " Order by assetPurchaseOrder.POID DESC";

        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strWLBusinessID;
        }

        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadCurrencyType()
    {
        string strHQL;
        IList lst;

        strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        DL_CurrencyType.DataSource = lst;
        DL_CurrencyType.DataBind();
    }

    protected void LoadAssetPurchaseOrderDetail(string strPOID)
    {
        string strHQL = "Select * from T_AssetPurRecord where POID = " + strPOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadRelatedConstract(string strPOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.Status <> 'Archived'";
        strHQL += " and constract.ConstractCode in (select constractRelatedAssetPurchaseOrder.ConstractCode from ConstractRelatedAssetPurchaseOrder as constractRelatedAssetPurchaseOrder where constractRelatedAssetPurchaseOrder.POID = " + strPOID + ")";
        strHQL += " Order by constract.SignDate DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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

    protected decimal SumPurchaseOrderAmount(string strPOID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.POID = " + strPOID;
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);

        AssetPurRecord assetPurRecord = new AssetPurRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            assetPurRecord = (AssetPurRecord)lst[i];
            deAmount += assetPurRecord.Number * assetPurRecord.Price;
        }

        return deAmount;
    }

    protected void UpdatePurchaseOrderAmount(string strPOID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        assetPurchaseOrder.Amount = deAmount;

        try
        {
            assetPurchaseOrderBLL.UpdateAssetPurchaseOrder(assetPurchaseOrder, int.Parse(strPOID));
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string strAssetCode = TB_AssetCode.Text.Trim();
            string strHQL = "From Asset as asset where asset.AssetCode = '" + strAssetCode + "'";
            AssetBLL assetBLL = new AssetBLL();
            IList lst = assetBLL.GetAllAssets(strHQL);

            if (lst != null && lst.Count > 0)
            {
                Asset asset = (Asset)lst[0];

                TB_AssetCode.Text = asset.AssetCode.Trim();
                TB_AssetName.Text = asset.AssetName.Trim();
                TB_ModelNumber.Text = asset.ModelNumber.Trim();
                TB_Spec.Text = asset.Spec.Trim();
                NB_Number.Amount = asset.Number;
                DL_Unit.SelectedValue = asset.UnitName;
                NB_Price.Amount = asset.Price;
            }
        }
        catch (Exception ex)
        { }
    }

}
