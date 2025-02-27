using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;
using TakeTopCore;

public partial class TTGoodsProductionOrder : System.Web.UI.Page
{
    private string strUserCode;
    private string strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID;
    private string strMainTableCanAdd, strDetailTableCanAdd, strMainTableCanEdit, strMainTableCanDelete, strDetailTableCanEdit, strDetailTableCanDelete;
    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    string strRelatedType, strRelatedID;
    string strProjectRelatedTypeCN, strProjectRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName, strDepartString;

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        if (strRelatedType == "Project")
        {
            strProjectRelatedTypeCN = "Project";
            strProjectRelatedID = strRelatedID;

            DL_RelatedType.SelectedValue = "Project";
            NB_RelatedID.Amount = int.Parse(strRelatedID);
        }
        if (strRelatedType == "Plan")
        {
            strProjectRelatedTypeCN = "Plan";
            strProjectRelatedID = strRelatedID;

            DL_RelatedType.SelectedValue = "Project";
            NB_RelatedID.Amount = int.Parse(ShareClass.getProjectIDByPlanID(strRelatedID));
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
        strToDoWLDetailID = Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "生产作业单", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //WorkFlow,如果是由工作流启动的业务,初始化相关按钮
        ShareClass.InitialWorkflowRelatedModule(strRelatedWorkflowID, strRelatedWorkflowStepID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strMainTableCanAdd, strDetailTableCanAdd, strMainTableCanEdit, strDetailTableCanEdit);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ProductionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_FinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_DeliveryDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TB_ProductionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_FinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DL_PlanProductionType.Items.Insert(0, new ListItem(LanguageHandle.GetWord("ZiZhi").ToString().Trim(), "SelfMade"));
            DL_PlanProductionType.Items.Insert(1, new ListItem(LanguageHandle.GetWord("WeiWai").ToString().Trim(), "OutSourcing"));

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView5, strUserCode);
            LB_DepartString.Text = strDepartString;

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

            ShareClass.LoadWFTemplate(strUserCode, "MaterialProduction", DL_TemName);

            LoadGoodsProductionOrder(strUserCode);

            LoadPlanVerID();
            LoadItemMainPlan(strUserCode);
            LoadGoodsSaleOrder(strUserCode);

            LoadProductProcess();

            ShareClass.LoadCurrencyType(DL_CurrencyType);

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
            LB_ProjectID.Text = strProjectID;
            NB_RelatedID.Amount = int.Parse(strProjectID);

            try
            {
                if (DL_RelatedType.SelectedValue == "Project")
                {
                    LB_ProjectID.Text = NB_RelatedID.Amount.ToString();
                    LoadProjectRelatedItem(NB_RelatedID.Amount.ToString());
                    LoadProjectItemBomVersion(NB_RelatedID.Amount.ToString());
                    TakeTopBOM.InitialProjectItemBomTree(NB_RelatedID.Amount.ToString(), DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim(), TreeView4);
                }
            }
            catch
            {
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_DefaultProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_DefaultProcess.Text = DL_DefaultProcess.SelectedValue;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DL_ChangeProjecrItemBomVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID;

        string strProjectID = int.Parse(NB_RelatedID.Amount.ToString()).ToString();

        strID = DL_ChangeProjectItemBomVersionID.SelectedValue.Trim();
        strVerID = DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim();

        try
        {
            TakeTopBOM.InitialProjectItemBomTree(strProjectID, strVerID, TreeView4);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message.ToString() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void TreeView4_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView4.SelectedNode;

        if (treeNode.Target != "0")
        {
            strID = treeNode.Target;

            strHQL = "From ProjectRelatedItemBom as projectRelatedItemBom Where projectRelatedItemBom.ID = " + strID;
            ProjectRelatedItemBomBLL projectRelatedItemBomBLL = new ProjectRelatedItemBomBLL();
            lst = projectRelatedItemBomBLL.GetAllProjectRelatedItemBoms(strHQL);

            if (lst.Count > 0)
            {
                ProjectRelatedItemBom projectRelatedItemBom = (ProjectRelatedItemBom)lst[0];

                TB_GoodsCode.Text = projectRelatedItemBom.ItemCode.Trim();
                TB_GoodsName.Text = projectRelatedItemBom.ItemName.Trim();

                try
                {
                    DL_Type.SelectedValue = projectRelatedItemBom.ItemType;
                }
                catch
                {
                }
                TB_ModelNumber.Text = projectRelatedItemBom.ModelNumber;
                TB_Spec.Text = projectRelatedItemBom.Specification.Trim();
                TB_Brand.Text = projectRelatedItemBom.Brand;

                NB_Number.Amount = projectRelatedItemBom.Number - projectRelatedItemBom.AleadyProduction;

                try
                {
                    DL_Unit.SelectedValue = projectRelatedItemBom.Unit;
                }
                catch
                {
                }
            }

            DL_RecordSourceType.SelectedValue = "ProjectBOMRecord";
            NB_RecordSourceID.Amount = int.Parse(strID);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void RefreshProjectRelatedItemNumber(string strProjectID)
    {
        LoadProjectRelatedItem(strProjectID);
    }

    protected void BT_MaterialBudgetFind_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectID = NB_RelatedID.Amount.ToString();
        if (DL_RelatedType.SelectedValue.Trim() == "Project")
        {
            strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ProjectID = " + strProjectID;
            strHQL += " and ItemCode Like '%" + TB_FindItemCode.Text.Trim() + "%'";
            strHQL += " and ItemName Like '%" + TB_FindItemName.Text.Trim() + "%'";
            strHQL += " and Specification Like '%" + TB_FindItemSpec.Text.Trim() + "%'";
            strHQL += " and ModelNumber Like '%" + TB_FindModelNumber.Text.Trim() + "%'";
            strHQL += " Order by projectRelatedItem.ID ASC";
            ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
            lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

            DataGrid11.DataSource = lst;
            DataGrid11.DataBind();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid11_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode, strItemName, strBomVerID, strUnit, strDefaultProcess;
            decimal deNumber, deReservedNumber;

            for (int i = 0; i < DataGrid11.Items.Count; i++)
            {
                DataGrid11.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ID = " + strID;
            ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
            lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

            ProjectRelatedItem projectRelatedItem = (ProjectRelatedItem)lst[0];

            strItemCode = projectRelatedItem.ItemCode.Trim();
            strItemName = projectRelatedItem.ItemName.Trim();

            strUnit = projectRelatedItem.Unit;
            deNumber = projectRelatedItem.Number;
            deReservedNumber = projectRelatedItem.ReservedNumber;
            strDefaultProcess = projectRelatedItem.DefaultProcess.Trim();

            TB_GoodsCode.Text = strItemCode;
            TB_GoodsName.Text = strItemName;

            NB_Number.Amount = projectRelatedItem.Number - projectRelatedItem.AleadyProduction;

            try
            {
                DL_Type.SelectedValue = projectRelatedItem.ItemType;
            }
            catch
            {
            }

            TB_Spec.Text = projectRelatedItem.Specification;
            TB_ModelNumber.Text = projectRelatedItem.ModelNumber;

            TB_Brand.Text = projectRelatedItem.Brand;

            DL_Unit.SelectedValue = strUnit;

            //LB_SourceRelatedID.Text = projectRelatedItem.ProjectID.ToString();
            DL_RecordSourceType.SelectedValue = "GoodsPJRecord";
            NB_RecordSourceID.Amount = projectRelatedItem.ID;

            strBomVerID = projectRelatedItem.BomVersionID.ToString();
            LoadItemBomVersion(strItemCode, DL_BomVerID);
            DL_BomVerID.SelectedValue = strBomVerID;

            HL_ItemRelatedDoc.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=" + TB_GoodsCode.Text.Trim() + "&RelatedID=" + DL_BomVerID.SelectedValue.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPDID, strPDRelatedType;
            IList lst;
            int intWLNumber;

            strPDID = e.Item.Cells[3].Text.Trim();

            intWLNumber = GetRelatedWorkFlowNumber("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
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

            LoadGoodsProductionOrderDetail(strPDID);

            //WorkFlow,如果此单和工作流相关，那么依工作流状态决定能否保存单据数据
            string strCreateUserCode = getGoodsProductionOrderCreatorCode(strPDID);
            ShareClass.MainTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("WuLiaoShengChanChan").ToString().Trim(), strPDID, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strMainTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID, "0");
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

                strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.PDID = " + strPDID;
                GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
                lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);
                GoodsProductionOrder goodsProductionOrder = (GoodsProductionOrder)lst[0];

                LB_PDID.Text = goodsProductionOrder.PDID.ToString();
                TB_PDName.Text = goodsProductionOrder.PDName.Trim();

                TB_RouteName.Text = goodsProductionOrder.RouteName.Trim();

                DL_ProductionType.SelectedValue = goodsProductionOrder.ProductionType.Trim();

                try
                {
                    DL_CurrencyType.SelectedValue = goodsProductionOrder.CurrencyType;
                }
                catch
                {

                }

                NB_Amount.Amount = goodsProductionOrder.Amount;

                DLC_ProductionDate.Text = goodsProductionOrder.ProductionDate.ToString("yyyy-MM-dd");
                DLC_FinishedDate.Text = goodsProductionOrder.FinishedDate.ToString("yyyy-MM-dd");

                DL_PDStatus.SelectedValue = goodsProductionOrder.Status.Trim();

                DL_RelatedType.SelectedValue = goodsProductionOrder.RelatedType.Trim();
                NB_RelatedID.Amount = goodsProductionOrder.RelatedID;

                TB_BelongDepartName.Text = goodsProductionOrder.BelongDepartName;
                LB_BelongDepartCode.Text = goodsProductionOrder.BelongDepartCode;

                strPDRelatedType = goodsProductionOrder.RelatedType.Trim();

                LoadGoodsProductionOrderDetail(strPDID);

                intWLNumber = GetRelatedWorkFlowNumber("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
                if (intWLNumber != 0)
                {
                    BT_SubmitApply.Enabled = false;
                }
                else
                {
                    BT_SubmitApply.Enabled = true;
                }
                LoadRelatedWL("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), goodsProductionOrder.PDID);
                TB_WLName.Text = LanguageHandle.GetWord("ShengChan").ToString().Trim() + goodsProductionOrder.PDName.Trim() + LanguageHandle.GetWord("ShenQing").ToString().Trim();


                try
                {
                    if (DL_RelatedType.SelectedValue == "Project")
                    {
                        LB_ProjectID.Text = NB_RelatedID.Amount.ToString();
                        LoadProjectRelatedItem(NB_RelatedID.Amount.ToString());
                        LoadProjectItemBomVersion(NB_RelatedID.Amount.ToString());
                        TakeTopBOM.InitialProjectItemBomTree(NB_RelatedID.Amount.ToString(), DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim(), TreeView4);
                    }
                }
                catch
                {
                }


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
                intWLNumber = GetRelatedWorkFlowNumber("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
                if (intWLNumber > 0)
                {
                    return;
                }

                //Workflow,如果存在关联工作流，那么要执行下面的代码
                if (!ShareClass.MainTableDeleteWorkflowRelatedModule(strUserCode, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, strMainTableCanDelete))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZSBNWQSCQJC").ToString().Trim() + "')", true);
                    return;
                }

                if (DataGrid1.Items.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZMXJLSCSBQJC").ToString().Trim() + "')", true);
                    return;
                }

                try
                {
                    strHQL = "Delete From T_GoodsProductionOrder Where PDID = " + strPDID;
                    ShareClass.RunSqlCommand(strHQL);

                    //Workflow,删除流程模组关联记录
                    ShareClass.DeleteModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("WuLiaoShengChanChan").ToString().Trim(), strPDID);

                    LoadGoodsProductionOrder(strUserCode);
                    LoadGoodsProductionOrderDetail(strPDID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
                }
            }
        }
    }


    protected void TreeView5_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView5.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_BelongDepartCode.Text = strDepartCode;
            TB_BelongDepartName.Text = strDepartName;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    //Workflow,取得单据创建人代码
    protected string getGoodsProductionOrderCreatorCode(string strPDID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.PDID = " + strPDID;
        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);
        GoodsProductionOrder goodsProductionOrder = (GoodsProductionOrder)lst[0];

        return goodsProductionOrder.CreatorCode.Trim();
    }

    protected void DL_RelatedType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strRelatedType;

        strRelatedType = DL_RelatedType.SelectedValue.Trim();

        if (strRelatedType == "Other")
        {
            NB_RelatedID.Amount = 0;
        }

        if (strRelatedType == "MRPPlan")
        {
            BT_RelatedMRPPlan.Visible = true;
            NB_RelatedID.Amount = 0;
        }
        else
        {
            BT_RelatedMRPPlan.Visible = false;
        }

        if (strRelatedType == "Project")
        {
            BT_RelatedProject.Visible = true;
            NB_RelatedID.Amount = 0;
        }
        else
        {
            BT_RelatedProject.Visible = false;
        }

        if (strRelatedType == "SaleOrder")
        {
            BT_RelatedSaleOrder.Visible = true;
            NB_RelatedID.Amount = 0;
        }
        else
        {
            BT_RelatedSaleOrder.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strSOID;

        if (e.CommandName != "Page")
        {
            strSOID = ((Button)e.Item.FindControl("BT_SOID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            NB_RelatedID.Amount = int.Parse(strSOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_PDID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        string strNewPDCode = ShareClass.GetCodeByRule("ProductionOrderCode", "ProductionOrderCode", "00");
        if (strNewPDCode != "")
        {
            TB_PDName.Text = strNewPDCode;
        }

        LoadGoodsProductionOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strPDID;

        strPDID = LB_PDID.Text.Trim();

        if (strPDID == "")
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
        string strPDID, strPDName, strRouteName, strOperatorCode, strOperatorName, strProductionType;
        DateTime dtProductionDate, dtFinishedDate;


        string strStatus;

        strPDName = TB_PDName.Text.Trim();
        strProductionType = DL_ProductionType.SelectedValue.Trim();
        strRouteName = TB_RouteName.Text.Trim();

        strOperatorCode = LB_UserCode.Text.Trim();
        strOperatorName = LB_UserName.Text.Trim();

        dtProductionDate = DateTime.Parse(DLC_ProductionDate.Text);
        dtFinishedDate = DateTime.Parse(DLC_FinishedDate.Text);

        strStatus = DL_PDStatus.SelectedValue.Trim();

        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        GoodsProductionOrder goodsProductionOrder = new GoodsProductionOrder();

        goodsProductionOrder.PDName = strPDName;
        goodsProductionOrder.RouteName = strRouteName;
        goodsProductionOrder.ProductionType = strProductionType;
        goodsProductionOrder.ProductionDate = dtProductionDate;
        goodsProductionOrder.FinishedDate = dtFinishedDate;
        goodsProductionOrder.CreatorCode = strOperatorCode;
        goodsProductionOrder.CreatorName = strOperatorName;
        goodsProductionOrder.Status = "New";

        goodsProductionOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        goodsProductionOrder.Amount = 0;

        goodsProductionOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        goodsProductionOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        goodsProductionOrder.BusinessType = strRelatedType;
        goodsProductionOrder.BusinessCode = strRelatedID;

        goodsProductionOrder.BelongDepartCode = LB_BelongDepartCode.Text.Trim();
        goodsProductionOrder.BelongDepartName = TB_BelongDepartName.Text.Trim();

        try
        {
            goodsProductionOrderBLL.AddGoodsProductionOrder(goodsProductionOrder);

            strPDID = ShareClass.GetMyCreatedMaxGoodsProductionOrderID(strOperatorCode);
            LB_PDID.Text = strPDID;

            string strNewPDCode = ShareClass.GetCodeByRule("ProductionOrderCode", "ProductionOrderCode", strPDID);
            if (strNewPDCode != "")
            {
                TB_PDName.Text = strNewPDCode;
                string strHQL = "Update T_GoodsProductionOrder Set PDName = " + "'" + strNewPDCode + "'" + " Where PDID = " + strPDID;
                ShareClass.RunSqlCommand(strHQL);
            }

            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("WuLiaoShengChanChan").ToString().Trim(), strPDID);

            TB_WLName.Text = LanguageHandle.GetWord("ShengChan").ToString().Trim() + strPDName + LanguageHandle.GetWord("ShenQing").ToString().Trim();

            BT_SubmitApply.Enabled = true;

            LoadGoodsProductionOrder(strOperatorCode);
            LoadGoodsProductionOrderDetail(strPDID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCKNSCMCZD50GHZHBZZSZD100GHZGDJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strPDID, strRouteName, strUserCode, strPDName, strProductionType;
        DateTime dtProductionDate, dtFinishedDate;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();

        strPDID = LB_PDID.Text.Trim();
        strPDName = TB_PDName.Text.Trim();
        strRouteName = TB_RouteName.Text.Trim();
        strProductionType = DL_ProductionType.SelectedValue.Trim();

        dtProductionDate = DateTime.Parse(DLC_ProductionDate.Text);
        dtFinishedDate = DateTime.Parse(DLC_FinishedDate.Text);

        strStatus = DL_PDStatus.SelectedValue.Trim();

        strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.PDID = " + strPDID;
        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);

        GoodsProductionOrder goodsProductionOrder = (GoodsProductionOrder)lst[0];

        goodsProductionOrder.PDName = strPDName;
        goodsProductionOrder.RouteName = strRouteName;
        goodsProductionOrder.ProductionType = strProductionType;

        goodsProductionOrder.Status = strStatus;
        goodsProductionOrder.FinishedDate = dtFinishedDate;

        goodsProductionOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

        goodsProductionOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        goodsProductionOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        goodsProductionOrder.BelongDepartCode = LB_BelongDepartCode.Text.Trim();
        goodsProductionOrder.BelongDepartName = TB_BelongDepartName.Text.Trim();

        try
        {
            goodsProductionOrderBLL.UpdateGoodsProductionOrder(goodsProductionOrder, int.Parse(strPDID));
            LoadGoodsProductionOrder(strUserCode);

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select PDID as DetailPDID, * from T_GoodsProductionOrder where PDID = " + strPDID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
                }

                if (strToDoWLID != null)
                {
                    if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }
                    ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_SelectDepartment_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popSelectDepartmentWindow') ", true);
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        GoodsProductionOrderBLL puchaseOrderBLL = new GoodsProductionOrderBLL();
        IList lst = puchaseOrderBLL.GetAllGoodsProductionOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void DL_PlanVerID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanVerID;

        strPlanVerID = DL_PlanVerID.SelectedValue;

        strHQL = "Select * From T_ItemMainPlanMRPVersion Where Type = 'ACTIVE' and PlanVerID = " + strPlanVerID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanMRPVersion");

        DL_PlanMRPVersionID.DataSource = ds;
        DL_PlanMRPVersionID.DataBind();

        DL_PlanMRPVersionID.Items.Insert(0, new ListItem("--Select--", "0"));

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DL_PlanMRPVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPlanVerID, strPlanMRPVerID, strID, strProductionType;

        strPlanVerID = DL_PlanVerID.SelectedValue;
        strProductionType = DL_PlanProductionType.SelectedValue.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text;

        if (strProductionType == "SelfMade")
        {
            strProductionType = "MadeParts";
        }
        else
        {
            strProductionType = "OutParts";
        }

        strID = DL_PlanMRPVersionID.SelectedValue;
        if (strID != "0")
        {
            LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID, strProductionType);
        }
        else
        {
            LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, "0", strProductionType);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strPlanVerID, strPlanMRPVerID;
        string strBusinessType, strBussnessID;

        string strID, strProductionType;

        strPlanVerID = DL_PlanVerID.SelectedValue;
        strProductionType = DL_PlanProductionType.SelectedValue.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text;

        DataSet ds;

        strBusinessType = DL_BusinessType.SelectedValue.Trim();
        strBussnessID = NB_BusinessID.Amount.ToString();

        if (strProductionType == "SelfMade")
        {
            strProductionType = "MadeParts";
        }
        else
        {
            strProductionType = "OutParts";
        }

        if (strBusinessType == "SaleOrder")
        {
            if (strProductionType == "MadeParts")
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID in (Select ID From T_GoodsSaleRecord Where SOID in (Select SOID From T_GoodsSaleOrder Where SOID = " + strBussnessID + "))";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();
            }
            else
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID in (Select ID From T_GoodsSaleRecord Where SOID in (Select SOID From T_GoodsSaleOrder Where SOID = " + strBussnessID + "))";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();
            }
        }

        if (strBusinessType == "Project")
        {
            if (strProductionType == "MadeParts")
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID in (Select ID From T_ProjectRelatedItem Where ProjectID = " + strBussnessID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();
            }
            else
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID in (Select ID From T_ProjectRelatedItem Where ProjectID = " + strBussnessID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();
            }
        }

        if (strBusinessType == "ApplicationOrder")
        {
            if (strProductionType == "MadeParts")
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                strHQL += " and SourceType = 'GoodsAORecord' and SourceRecordID in (Select ID From T_GoodsApplicationDetail Where AAID in (Select AID From T_GoodsApplication Where AAID  = " + strBussnessID + "))";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();
            }
            else
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                strHQL += " and SourceType = 'GoodsAORecord' and SourceRecordID in (Select ID From T_GoodsApplicationDetail Where AAID in (Select AID From T_GoodsApplication Where AAID  = " + strBussnessID + "))";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();
            }
        }

        if (strBusinessType == "Other")
        {
            strID = DL_PlanMRPVersionID.SelectedValue;
            if (strID != "0")
            {
                LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID, strProductionType);
            }
            else
            {
                LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, "0", strProductionType);
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DL_RecordSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_RecordSourceID.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strType, strGoodsCode, strGoodsName, strSpec;

        DataGrid4.CurrentPageIndex = 0;
        TabContainer2.ActiveTabIndex = 0;

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();

        strSpec = TB_Spec.Text.Trim();

        strType = "%" + strType + "%";
        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";

        strSpec = "%" + strSpec + "%";

        strHQL = "Select * From T_Goods as goods Where goods.GoodsCode Like " + "'" + strGoodsCode + "'" + " and goods.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and goods.Type Like " + "'" + strType + "'" + "  and goods.Spec Like " + "'" + strSpec + "'";
        strHQL += " Order by goods.Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid7.DataSource = ds;
        DataGrid7.DataBind();

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid6.DataSource = ds;
        DataGrid6.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";

        TB_Spec.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid6.Items.Count; i++)
            {
                DataGrid6.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_GoodsCode.Text = item.ItemCode;
                TB_GoodsName.Text = item.ItemName;
                TB_ModelNumber.Text = item.ModelNumber;

                DL_Unit.SelectedValue = item.Unit;
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

                try
                {
                    DL_Type.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_Type.SelectedValue = "";
                }

                LoadItemBomVersion(item.ItemCode.Trim(), DL_BomVerID);

                HL_ItemRelatedDoc.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=" + TB_GoodsCode.Text.Trim() + "&RelatedID=" + DL_BomVerID.SelectedValue.Trim();
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            for (int i = 0; i < DataGrid7.Items.Count; i++)
            {
                DataGrid7.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Goods as goods where goods.ID = " + strID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            if (lst.Count > 0)
            {
                Goods goods = (Goods)lst[0];

                TB_GoodsCode.Text = goods.GoodsCode;
                TB_GoodsName.Text = goods.GoodsName;
                TB_ModelNumber.Text = goods.ModelNumber;

                DL_Unit.SelectedValue = goods.UnitName;
                TB_Spec.Text = goods.Spec;
                TB_Brand.Text = goods.Manufacturer;

                try
                {
                    DL_Type.SelectedValue = goods.Type;
                }
                catch
                {
                }

                LoadItemBomVersion(goods.GoodsCode.Trim(), DL_BomVerID);
                HL_ItemRelatedDoc.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=" + TB_GoodsCode.Text.Trim() + "&RelatedID=" + DL_BomVerID.SelectedValue.Trim();

            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid10_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strPlanVerID;

            strPlanVerID = ((Button)e.Item.FindControl("BT_PlanVerID")).Text.Trim();

            for (int i = 0; i < DataGrid10.Items.Count; i++)
            {
                DataGrid10.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            NB_RelatedID.Amount = int.Parse(strPlanVerID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;
            decimal deRequireNumber, deOrderNumber;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            deRequireNumber = decimal.Parse(e.Item.Cells[6].Text);
            deOrderNumber = decimal.Parse(e.Item.Cells[7].Text);

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where item.ItemCode = " + "'" + strGoodsCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_GoodsCode.Text = item.ItemCode;
                TB_GoodsName.Text = item.ItemName;
                TB_ModelNumber.Text = item.ModelNumber;

                DL_Unit.SelectedValue = item.Unit;
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

                NB_Number.Amount = deRequireNumber - deOrderNumber;

                try
                {
                    DL_Type.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_Type.SelectedValue = "";
                }

                LoadItemBomVersion(item.ItemCode.Trim(), DL_BomVerID);
                HL_ItemRelatedDoc.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=" + TB_GoodsCode.Text.Trim() + "&RelatedID=" + DL_BomVerID.SelectedValue.Trim();

            }

            DL_RecordSourceType.SelectedValue = "ProductionPlan";
            NB_RecordSourceID.Amount = int.Parse(strID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DL_BomVerID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strItemCode, strVerID;

        strItemCode = TB_GoodsCode.Text.Trim();
        strVerID = DL_BomVerID.SelectedValue.Trim();
        if (strVerID != "0")
        {
            TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);

            HL_ItemRelatedDoc.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=" + TB_GoodsCode.Text.Trim() + "&RelatedID=" + DL_BomVerID.SelectedValue.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPDID;
            IList lst;

            strPDID = LB_PDID.Text.Trim();

            int intWLNumber = GetRelatedWorkFlowNumber("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
            if (intWLNumber > 0)
            {
                BT_NewDetail.Visible = false;
            }
            else
            {
                BT_NewDetail.Visible = true;
            }


            //WorkFlow,如果此单和工作流相关，那么依工作流状态决定能否保存单据数据
            ShareClass.DetailTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("WuLiaoShengChanChan").ToString().Trim(), strPDID, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strDetailTableCanAdd, strDetailTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                BT_NewMain.Visible = true;
                BT_NewDetail.Visible = true;
            }

            string strID = e.Item.Cells[2].Text.Trim();
            LB_ID.Text = strID;

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from GoodsProductionOrderDetail as goodsProductionOrderDetail where goodsProductionOrderDetail.ID = " + strID;

                GoodsProductionOrderDetailBLL goodsProductionOrderDetailBLL = new GoodsProductionOrderDetailBLL();
                lst = goodsProductionOrderDetailBLL.GetAllGoodsProductionOrderDetails(strHQL);
                GoodsProductionOrderDetail goodsProductionOrderDetail = (GoodsProductionOrderDetail)lst[0];

                LoadItemBomVersion(goodsProductionOrderDetail.GoodsCode.Trim(), DL_BomVerID);

                TB_GoodsCode.Text = goodsProductionOrderDetail.GoodsCode;
                TB_GoodsName.Text = goodsProductionOrderDetail.GoodsName;
                TB_ModelNumber.Text = goodsProductionOrderDetail.ModelNumber;
                TB_Spec.Text = goodsProductionOrderDetail.Spec;
                TB_Brand.Text = goodsProductionOrderDetail.Brand;

                DL_Type.SelectedValue = goodsProductionOrderDetail.Type;
                DL_Unit.SelectedValue = goodsProductionOrderDetail.UnitName;
                NB_Number.Amount = goodsProductionOrderDetail.Number;
                DLC_DeliveryDate.Text = goodsProductionOrderDetail.DeliveryDate.ToString("yyyy-MM-dd");
                DL_BomVerID.SelectedValue = goodsProductionOrderDetail.BomVerID.ToString();

                NB_Price.Amount = goodsProductionOrderDetail.Price;

                TB_PackagingSystem.Text = goodsProductionOrderDetail.PackagingSystem;
                TB_DefaultProcess.Text = goodsProductionOrderDetail.DefaultProcess;

                try
                {
                    DL_RecordSourceType.SelectedValue = goodsProductionOrderDetail.SourceType.Trim();
                }
                catch
                {
                }
                NB_RecordSourceID.Amount = goodsProductionOrderDetail.SourceID;

                //取得生产单明细的其它属性
                LoadGoodsProductionOrderDetailOtherColumn(strID);

                string strItemCode = TB_GoodsCode.Text.Trim();
                string strVerID = DL_BomVerID.SelectedValue.Trim();
                if (strVerID != "0")
                {
                    TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView3);
                }


                HL_ItemRelatedDoc.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=" + TB_GoodsCode.Text.Trim() + "&RelatedID=" + DL_BomVerID.SelectedValue;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "BOM")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from GoodsProductionOrderDetail as goodsProductionOrderDetail where goodsProductionOrderDetail.ID = " + strID;

                GoodsProductionOrderDetailBLL goodsProductionOrderDetailBLL = new GoodsProductionOrderDetailBLL();
                lst = goodsProductionOrderDetailBLL.GetAllGoodsProductionOrderDetails(strHQL);
                GoodsProductionOrderDetail goodsProductionOrderDetail = (GoodsProductionOrderDetail)lst[0];

                string strItemCode, strVerID;

                strItemCode = goodsProductionOrderDetail.GoodsCode.Trim();
                strVerID = goodsProductionOrderDetail.BomVerID.ToString();
                if (strVerID != "0")
                {
                    TakeTopBOM.InitialItemBomTreeForNew(strItemCode, strVerID, strItemCode, strVerID, TreeView1);
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popBOMWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = GetRelatedWorkFlowNumber("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);

                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                    return;
                }

                //Workflow,如果存在关联工作流，那么要执行下面的代码
                string strCreateUserCode;
                strCreateUserCode = getGoodsProductionOrderCreatorCode(strPDID);
                if (!ShareClass.DetailTableDeleteWorkflowRelatedModule(strUserCode, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, strDetailTableCanDelete))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZSBNWQSCQJC").ToString().Trim() + "')", true);
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    return;
                }

                string strSourceType;
                int intSourceID;

                GoodsProductionOrderDetailBLL goodsProductionOrderDetailBLL = new GoodsProductionOrderDetailBLL();
                strHQL = "from GoodsProductionOrderDetail as goodsProductionOrderDetail where goodsProductionOrderDetail.ID = " + strID;
                lst = goodsProductionOrderDetailBLL.GetAllGoodsProductionOrderDetails(strHQL);
                GoodsProductionOrderDetail goodsProductionOrderDetail = (GoodsProductionOrderDetail)lst[0];

                strSourceType = goodsProductionOrderDetail.SourceType.Trim();
                intSourceID = goodsProductionOrderDetail.SourceID;


                try
                {
                    goodsProductionOrderDetailBLL.DeleteGoodsProductionOrderDetail(goodsProductionOrderDetail);

                    //更改总金额
                    NB_Amount.Amount = SumGoodsProductionOrderAmount(strPDID);
                    UpdateGoodsProductionOrderAmount(strPDID, NB_Amount.Amount);

                    LoadGoodsProductionOrderDetail(strPDID);

                    if (strSourceType == "ProductionPlan")
                    {
                        UpdatProductionPlanNumber(strSourceType, intSourceID.ToString());
                        RefreshProductionPlanNumber(strSourceType, intSourceID.ToString());
                    }

                    //更改项目关联物资下单量
                    if (strSourceType == "GoodsPJRecord")
                    {
                        UpdatProjectRelatedItemNumber(strSourceType, intSourceID.ToString());
                    }


                    //依单据主体关联类型更新项目物资预算的物料代码的预算使用量
                    string strRelatedType = DL_RelatedType.SelectedValue.Trim();
                    string strRelatedID = NB_RelatedID.Amount.ToString();
                    if (DL_RelatedType.SelectedValue.Trim() == "Project")
                    {
                        ShareClass.UpdateProjectRelatedItemNumberByBudgetBusinessType("PRODUCTION", strRelatedType, strRelatedID, goodsProductionOrderDetail.GoodsCode.Trim());
                        RefreshProjectRelatedItemNumber(strRelatedID);
                    }

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText;

                        strCmdText = "select PDID as DetailPDID, * from T_GoodsProductionOrder where PDID = " + strPDID;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
                        }

                        if (strToDoWLID != null)
                        {
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }
                            ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }

                        if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                        {
                            strCmdText = "select * from T_GoodsProductionOrderDetail where PDID = " + strPDID;
                            ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    //取得生产单明细的其它属性
    protected void LoadGoodsProductionOrderDetailOtherColumn(string strPDDetailID)
    {
        string strHQL;

        strHQL = "Select * From T_GoodsProductionOrderDetail Where ID = " + strPDDetailID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentDetail");

        TB_BatchNumber.Text = ds.Tables[0].Rows[0]["BatchNumber"].ToString();
        TB_ProductionDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["ProductDate"].ToString()).ToString("yyyy-MM-dd");
        TB_FinishedDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["FinishedDate"].ToString()).ToString("yyyy-MM-dd");
        TB_ProductionEquipmentNumber.Text = ds.Tables[0].Rows[0]["ProductionEquipmentNumber"].ToString();
        TB_MaterialFormNumber.Text = ds.Tables[0].Rows[0]["MaterialFormNumber"].ToString();
    }
    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popBOMWindow') ", true);
    }

    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strPDID;

        strPDID = LB_PDID.Text.Trim();

        if (strPDID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strPDID = LB_PDID.Text.Trim();
        int intWLNumber = GetRelatedWorkFlowNumber("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
        if (intWLNumber > 0 & strToDoWLID == null)
        {
            BT_SubmitApply.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCZGLDGZLJLBNSCJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            return;
        }
        else
        {
            BT_SubmitApply.Enabled = true;
        }

        TB_ProductionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        TB_FinishedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");


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
        string strRecordID, strPDID, strType, strGoodsCode, strGoodsName, strSpec, strModelNumber, strBomVerID, strPackingWay, strDefaultProcess;
        string strUnitName;
        decimal decNumber;
        DateTime dtDeliveryDate;

        string strSourceType;
        int intSourceID;

        strPDID = LB_PDID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();

        decNumber = NB_Number.Amount;
        strSpec = TB_Spec.Text.Trim();
        strBomVerID = DL_BomVerID.SelectedValue.Trim();

        dtDeliveryDate = DateTime.Parse(DLC_DeliveryDate.Text);

        strPackingWay = TB_PackagingSystem.Text.Trim();
        strDefaultProcess = TB_DefaultProcess.Text.Trim();

        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_RecordSourceID.Amount.ToString());

        if (strSourceType == "GoodsPJRecord")
        {
            if (!ShareClass.checkRequireNumberIsMoreHaveNumberForProjectRelatedItemNumber(intSourceID.ToString(), "AleadyProduction", decNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2333", "alert('" +LanguageHandle.GetWord("ZZDiShiShengChanLiangChaoChuXu").ToString().Trim()+"')", true);
            }
        }

        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
        else
        {
            GoodsProductionOrderDetailBLL goodsProductionOrderDetailBLL = new GoodsProductionOrderDetailBLL();
            GoodsProductionOrderDetail goodsProductionOrderDetail = new GoodsProductionOrderDetail();

            goodsProductionOrderDetail.PDID = int.Parse(strPDID);
            goodsProductionOrderDetail.Type = strType;
            goodsProductionOrderDetail.ModelNumber = strModelNumber;
            goodsProductionOrderDetail.GoodsCode = strGoodsCode;
            goodsProductionOrderDetail.GoodsName = strGoodsName;
            goodsProductionOrderDetail.Number = decNumber;
            goodsProductionOrderDetail.UnitName = strUnitName;

            goodsProductionOrderDetail.Price = NB_Price.Amount;
            goodsProductionOrderDetail.Amount = NB_Price.Amount * decNumber;

            goodsProductionOrderDetail.CheckInNumber = 0;
            goodsProductionOrderDetail.DeliveryDate = dtDeliveryDate;
            goodsProductionOrderDetail.Spec = strSpec;
            goodsProductionOrderDetail.Brand = TB_Brand.Text;

            goodsProductionOrderDetail.BomVerID = int.Parse(strBomVerID);
            goodsProductionOrderDetail.PackagingSystem = strPackingWay;
            goodsProductionOrderDetail.DefaultProcess = strDefaultProcess;

            goodsProductionOrderDetail.SourceType = strSourceType;
            goodsProductionOrderDetail.SourceID = intSourceID;

            try
            {
                goodsProductionOrderDetailBLL.AddGoodsProductionOrderDetail(goodsProductionOrderDetail);

                strRecordID = ShareClass.GetMyCreatedMaxGoodsProductionOrderDetailID(strPDID);
                LB_ID.Text = strRecordID;

                //更新生产单明细的其它属性
                UpdateGoodsProductionOrderDetailForOtherColumn(LB_ID.Text);

                //更改总金额
                NB_Amount.Amount = SumGoodsProductionOrderAmount(strPDID);
                UpdateGoodsProductionOrderAmount(strPDID, NB_Amount.Amount);

                LoadGoodsProductionOrderDetail(strPDID);

                if (strSourceType == "ProductionPlan")
                {
                    UpdatProductionPlanNumber(strSourceType, intSourceID.ToString());

                    RefreshProductionPlanNumber(strSourceType, intSourceID.ToString());
                }

                //更改项目关联物资下单量
                if (strSourceType == "GoodsPJRecord")
                {
                    UpdatProjectRelatedItemNumber(strSourceType, intSourceID.ToString());
                }

                //依单据主体关联类型更新项目物资预算的物料代码的预算使用量
                string strRelatedType = DL_RelatedType.SelectedValue.Trim();
                string strRelatedID = NB_RelatedID.Amount.ToString();
                if (DL_RelatedType.SelectedValue.Trim() == "Project")
                {
                    ShareClass.UpdateProjectRelatedItemNumberByBudgetBusinessType("PRODUCTION", strRelatedType, strRelatedID, goodsProductionOrderDetail.GoodsCode.Trim());
                    RefreshProjectRelatedItemNumber(strRelatedID);
                }

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select PDID as DetailPDID, * from T_GoodsProductionOrder where PDID = " + strPDID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }
                        ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsProductionOrderDetail where PDID = " + strPDID;
                        ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdatProjectRelatedItemNumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber;

        if (strSourceType == "GoodsPJRecord")
        {
            strHQL = "Select coalesce(Sum(Number),0) From T_GoodsProductionOrderDetail Where SourceType = 'GoodsPJRecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadyProduction = " + deSumNumber.ToString() + " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void UpdateDetail()
    {
        string strType, strGoodsCode, strGoodsName, strSpec, strModelNumber, strPackingWay, strDefaultProcess;
        string strUnitName, strBomVerID;
        decimal deNumber;
        DateTime dtDeliveryDate;

        string strID, strPDID;
        string strHQL;
        IList lst;

        string strSourceType;
        int intSourceID;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();

        strPDID = LB_PDID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strBomVerID = DL_BomVerID.SelectedValue.Trim();

        deNumber = NB_Number.Amount;

        dtDeliveryDate = DateTime.Parse(DLC_DeliveryDate.Text);
        strPackingWay = TB_PackagingSystem.Text.Trim();
        strDefaultProcess = TB_DefaultProcess.Text.Trim();

        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_RecordSourceID.Amount.ToString());

        if (strSourceType == "GoodsPJRecord")
{
            if (!ShareClass.checkRequireNumberIsMoreHaveNumberForProjectRelatedItemNumber(intSourceID.ToString(), "AleadyProduction", deNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2333", "alert('" +LanguageHandle.GetWord("ZZDiShiShengChanLiangChaoChuXu").ToString().Trim()+"')", true);
            }
        }

        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
        else
        {
            GoodsProductionOrderDetailBLL goodsProductionOrderDetailBLL = new GoodsProductionOrderDetailBLL();
            strHQL = "from GoodsProductionOrderDetail as goodsProductionOrderDetail where goodsProductionOrderDetail.ID = " + strID;
            lst = goodsProductionOrderDetailBLL.GetAllGoodsProductionOrderDetails(strHQL);
            GoodsProductionOrderDetail goodsProductionOrderDetail = (GoodsProductionOrderDetail)lst[0];

            goodsProductionOrderDetail.PDID = int.Parse(strPDID);
            goodsProductionOrderDetail.Type = strType;
            goodsProductionOrderDetail.ModelNumber = strModelNumber;
            goodsProductionOrderDetail.GoodsCode = strGoodsCode;
            goodsProductionOrderDetail.GoodsName = strGoodsName;
            goodsProductionOrderDetail.Number = deNumber;
            goodsProductionOrderDetail.UnitName = strUnitName;

            goodsProductionOrderDetail.Price = NB_Price.Amount;
            goodsProductionOrderDetail.Amount = NB_Price.Amount * deNumber;

            goodsProductionOrderDetail.DeliveryDate = dtDeliveryDate;
            goodsProductionOrderDetail.PackagingSystem = strPackingWay;
            goodsProductionOrderDetail.DefaultProcess = strDefaultProcess;

            goodsProductionOrderDetail.Spec = strSpec;
            goodsProductionOrderDetail.Brand = TB_Brand.Text;

            goodsProductionOrderDetail.BomVerID = int.Parse(strBomVerID);

            goodsProductionOrderDetail.SourceType = strSourceType;
            goodsProductionOrderDetail.SourceID = intSourceID;

            try
            {
                goodsProductionOrderDetailBLL.UpdateGoodsProductionOrderDetail(goodsProductionOrderDetail, int.Parse(strID));

                //更改总金额
                NB_Amount.Amount = SumGoodsProductionOrderAmount(strPDID);
                UpdateGoodsProductionOrderAmount(strPDID, NB_Amount.Amount);

                //更新出库单明细的其它属性
                UpdateGoodsProductionOrderDetailForOtherColumn(strID);

                LoadGoodsProductionOrderDetail(strPDID);

                if (strSourceType == "ProductionPlan")
                {
                    UpdatProductionPlanNumber(strSourceType, intSourceID.ToString());

                    RefreshProductionPlanNumber(strSourceType, intSourceID.ToString());
                }

                //更改项目关联物资下单量
                if (strSourceType == "GoodsPJRecord")
                {
                    UpdatProjectRelatedItemNumber(strSourceType, intSourceID.ToString());
                }

                //依单据主体关联类型更新项目物资预算的物料代码的预算使用量
                string strRelatedType = DL_RelatedType.SelectedValue.Trim();
                string strRelatedID = NB_RelatedID.Amount.ToString();
                if (DL_RelatedType.SelectedValue.Trim() == "Project")
                {
                    ShareClass.UpdateProjectRelatedItemNumberByBudgetBusinessType("PRODUCTION", strRelatedType, strRelatedID, goodsProductionOrderDetail.GoodsCode.Trim());
                    RefreshProjectRelatedItemNumber(strRelatedID);
                }

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select PDID as DetailPDID, * from T_GoodsProductionOrder where PDID = " + strPDID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialProduction", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strPDID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }
                        ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsProductionOrderDetail where PDID = " + strPDID;
                        ShareClass.UpdateWokflowRelatedXMLFile("DetailTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    //更新生产单明细的其它属性
    protected void UpdateGoodsProductionOrderDetailForOtherColumn(string strPDDDetailID)
    {
        string strHQL;

        string strBatchNumber, strProductDate, strFinishedDate, strProductionEquipmentNumber, strMaterialFormNumber, strRegistrationNumber;

        strBatchNumber = TB_BatchNumber.Text.Trim();
        strProductDate = TB_ProductionDate.Text;
        strFinishedDate = TB_FinishedDate.Text;
        strProductionEquipmentNumber = TB_ProductionEquipmentNumber.Text.Trim();
        strMaterialFormNumber = TB_MaterialFormNumber.Text.Trim();

        try
        {
            strHQL = "Update T_GoodsProductionOrderDetail Set BatchNumber = '" + strBatchNumber + "',ProductDate = " + strProductDate + ",FinishedDate = " + strFinishedDate + ",ProductionEquipmentNumber = '" + strProductionEquipmentNumber + "',MaterialFormNumber = '" + strMaterialFormNumber + "' Where ID = " + strPDDDetailID;
            ShareClass.RunSqlCommand(strHQL);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    protected decimal SumGoodsProductionOrderAmount(string strPDID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from GoodsProductionOrderDetail as goodsProductionOrderDetail where goodsProductionOrderDetail.PDID = " + strPDID;
        GoodsProductionOrderDetailBLL goodsProductionOrderDetailBLL = new GoodsProductionOrderDetailBLL();
        lst = goodsProductionOrderDetailBLL.GetAllGoodsProductionOrderDetails(strHQL);

        GoodsProductionOrderDetail goodsProductionOrderDetail = new GoodsProductionOrderDetail();

        for (int i = 0; i < lst.Count; i++)
        {
            goodsProductionOrderDetail = (GoodsProductionOrderDetail)lst[i];
            deAmount += goodsProductionOrderDetail.Amount;
        }

        return deAmount;
    }

    protected void UpdateGoodsProductionOrderAmount(string strPDID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.PDID = " + strPDID;
        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);

        GoodsProductionOrder goodsProductionOrder = (GoodsProductionOrder)lst[0];

        goodsProductionOrder.Amount = deAmount;

        try
        {
            goodsProductionOrderBLL.UpdateGoodsProductionOrder(goodsProductionOrder, int.Parse(strPDID));
        }
        catch
        {
        }
    }

    protected void LoadProductionRequirementPlan()
    {
        string strHQL;

        string strPlanVerID, strPlanMRPVerID;
        string strBusinessType, strBussnessID;

        string strID, strProductionType;

        strPlanVerID = DL_PlanVerID.SelectedValue;
        strProductionType = DL_PlanProductionType.SelectedValue.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text;

        DataSet ds;

        strBusinessType = DL_BusinessType.SelectedValue.Trim();
        strBussnessID = NB_BusinessID.Amount.ToString();

        if (strProductionType == "SelfMade")
        {
            strProductionType = "MadeParts";
        }
        else
        {
            strProductionType = "OutParts";
        }

        if (strBussnessID != "")
        {
            if (strBusinessType == "SaleOrder")
            {
                if (strProductionType == "MadeParts")
                {
                    strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                    strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                    strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID in (Select ID From T_GoodsSaleRecord Where SOID in (Select SOID From T_GoodsSaleOrder Where SOID = " + "'" + strBussnessID + "'))";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                    DataGrid3.DataSource = ds;
                    DataGrid3.DataBind();
                }
                else
                {
                    strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                    strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                    strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID in (Select ID From T_GoodsSaleRecord Where SOID in (Select SOID From T_GoodsSaleOrder Where SOID = " + "'" + strBussnessID + "'))";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                    DataGrid3.DataSource = ds;
                    DataGrid3.DataBind();
                }
            }

            if (strBusinessType == "Project")
            {
                if (strProductionType == "MadeParts")
                {
                    strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                    strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                    strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID in (Select ID From T_ProjectRelatedItem Where ProjectID SOID = " + "'" + strBussnessID + "')";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                    DataGrid3.DataSource = ds;
                    DataGrid3.DataBind();
                }
                else
                {
                    strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                    strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                    strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID in (Select ID From T_ProjectRelatedItem Where ProjectID SOID = " + "'" + strBussnessID + "')";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                    DataGrid3.DataSource = ds;
                    DataGrid3.DataBind();
                }
            }

            if (strBusinessType == "ApplicationOrder")
            {
                if (strProductionType == "MadeParts")
                {
                    strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                    strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                    strHQL += " and SourceType = 'GoodsAORecord' and SourceRecordID in (Select ID From T_GoodsApplicationDetail Where AAID in (Select AID From T_GoodsApplication Where AAID = " + "'" + strBussnessID + "'))";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                    DataGrid3.DataSource = ds;
                    DataGrid3.DataBind();
                }
                else
                {
                    strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                    strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                    strHQL += " and SourceType = 'GoodsAORecord' and SourceRecordID in (Select ID From T_GoodsApplicationDetail Where AAID in (Select AID From T_GoodsApplication Where AAID = " + "'" + strBussnessID + "'))";
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                    DataGrid3.DataSource = ds;
                    DataGrid3.DataBind();
                }
            }
        }
        else
        {
            strID = DL_PlanMRPVersionID.SelectedValue;
            if (strID != "0")
            {
                LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID, strProductionType);
            }
            else
            {
                LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, "0", strProductionType);
            }
        }
    }

    protected void UpdatProductionPlanNumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber;

        if (strSourceType == "ProductionPlan")
        {
            strHQL = "Select  coalesce(Sum(Number),0) From T_GoodsProductionOrderDetail Where SourceType = 'ProductionPlan' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsProductionOrderDetail");

            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ItemMainPlanRelatedItemProductPlan Set OrderNumber =  " + deSumNumber.ToString() + " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void BT_AllPurGoods_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();

        LoadGoodsProductionOrder(strUserCode);
    }

    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText, strPDID;

        string strWLID, strUserCode;

        strPDID = LB_PDID.Text.Trim();

        if (strRelatedType == null)
        {
            strProjectRelatedTypeCN = LanguageHandle.GetWord("WuLiao").ToString().Trim();
            strProjectRelatedID = strPDID;
        }

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = ShareClass.GetUserName(strCreatorCode);

        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim() + "');</script>");

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

        workFlow.RelatedType = strProjectRelatedTypeCN;
        workFlow.RelatedID = int.Parse(strProjectRelatedID);

        workFlow.BusinessType = "GoodsProduction";
        workFlow.BusinessCode = strPDID;

        workFlow.Status = "New";
        workFlow.DIYNextStep = "YES";
        workFlow.IsPlanMainWorkflow = "NO";

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

            LoadRelatedWL(strWLType, LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strPDID));

            UpdateGoodsPurchaseStatus(strPDID, "InProgress");
            DL_PDStatus.SelectedValue = "InProgress";

            strCmdText = "select PDID as DetailPDID, * from T_GoodsProductionOrder where PDID = " + strPDID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsProductionOrder", strXMLFile2);

            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strWLID, "0", "0", LanguageHandle.GetWord("WuLiaoShengChanChan").ToString().Trim(), strPDID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPSCSSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPSCSSBKNGZLMCGCZD25GHZJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

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

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'MaterialProduction'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void DL_PDStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPDID, strStatus, strUserCode;

        strPDID = LB_PDID.Text.Trim();
        strStatus = DL_PDStatus.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strPDID != "")
        {
            UpdateGoodsPurchaseStatus(strPDID, strStatus);
            LoadGoodsProductionOrder(strUserCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void UpdateGoodsPurchaseStatus(string strPDID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.PDID = " + strPDID;
        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);

        GoodsProductionOrder goodsProductionOrder = (GoodsProductionOrder)lst[0];

        goodsProductionOrder.Status = strStatus;

        try
        {
            goodsProductionOrderBLL.UpdateGoodsProductionOrder(goodsProductionOrder, int.Parse(strPDID));
        }
        catch
        {
        }
    }

    protected void RefreshProductionPlanNumber(string strSourceType, string strSourceID)
    {
        string strPlanVerID, strPlanMRPVerID, strID, strProductionType;

        strProductionType = DL_PlanProductionType.SelectedValue.Trim();

        string strHQL;

        strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where ID = " + strSourceID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strPlanVerID = ds.Tables[0].Rows[0]["PlanVerID"].ToString();
            strPlanMRPVerID = ds.Tables[0].Rows[0]["PlanMRPVerID"].ToString();

            if (strProductionType == "SelfMade")
            {
                strProductionType = "MadeParts";
            }
            else
            {
                strProductionType = "OutParts";
            }

            strID = ds.Tables[0].Rows[0]["ID"].ToString();
            if (strID != "0")
            {
                LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID, strProductionType);
            }
            else
            {
                LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, "0", strProductionType);
            }
        }
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and ((workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + ")";
        strHQL += " Or (workFlow.BusinessType = 'GoodsProduction' and workFlow.BusinessCode = '" + intRelatedID.ToString() + "'))";
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadProjectRelatedItem(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ProjectID = " + strProjectID + " Order by projectRelatedItem.ID ASC";
        ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
        lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

        DataGrid11.DataSource = lst;
        DataGrid11.DataBind();
    }

    protected void LoadGoodsProductionOrder(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        //Workflow,对流程相关模组作判断
        if (strRelatedWorkflowID == null)
        {
            strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.CreatorCode = " + "'" + strOperatorCode + "'";
        }
        else
        {
            strHQL = "from GoodsProductionOrder as goodsProductionOrder where ";
            strHQL += "goodsProductionOrder.PDID in (Select workFlowRelatedModule.RelatedID  From WorkFlowRelatedModule as workFlowRelatedModule Where workFlowRelatedModule.RelatedModuleName = '物料生产单' and workFlowRelatedModule.WorkflowID =" + strRelatedWorkflowID + ")"; 
        }

        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.PDID = " + strWLBusinessID;
        }

        //从计划中打开的业务单
        if (strRelatedType == "Plan")
        {
            strHQL = string.Format(@"from GoodsProductionOrder as goodsProductionOrder where goodsProductionOrder.BusinessType = '{0}'
            and goodsProductionOrder.BusinessCode = '{1}'", strRelatedType, strRelatedID);
        }
        strHQL += " Order by goodsProductionOrder.PDID DESC";

        GoodsProductionOrderBLL goodsProductionOrderBLL = new GoodsProductionOrderBLL();
        lst = goodsProductionOrderBLL.GetAllGoodsProductionOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadGoodsProductionOrderDetail(string strPDID)
    {
        string strHQL = "Select * from T_GoodsProductionOrderDetail where PDID = " + strPDID + " Order by ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsProductionOrderDetail");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadPlanVerID()
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlan Where Status = 'UNFinished' Order By PlanVerID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanMRPVersion");

        DL_PlanVerID.DataSource = ds;
        DL_PlanVerID.DataBind();

        DL_PlanVerID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadItemMainPlan(string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlan Where CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " Or CreatorCode in (Select UserCode From T_ProjectMember Where DepartCode in  " + LB_DepartString.Text.Trim() + ")";
        strHQL += " Order By PlanVerID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlan");

        DataGrid10.DataSource = ds;
        DataGrid10.DataBind();
    }

    protected void LoadItemMainPlanRelatedItemProductPlan(string strPlanVerID, string strPlanMRPVerID, string strProductionType)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = " + "'" + strProductionType + "')";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void LoadItemBomVersion(string strItemCode, DropDownList DL_VersionID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion Where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " Order by itemBomVersion.VerID DESC";
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);

        DL_VersionID.DataSource = lst;
        DL_VersionID.DataBind();

        DL_VersionID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadProjectItemBomVersion(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectItemBomVersion as projectItemBomVersion Where projectItemBomVersion.ProjectID = " + strProjectID + " Order by projectItemBomVersion.VerID DESC";
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);

        DL_ChangeProjectItemBomVersionID.DataSource = lst;
        DL_ChangeProjectItemBomVersionID.DataBind();
    }

    protected void LoadGoodsSaleOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where ( goodsSaleOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " or goodsSaleOrder.OperatorCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ") ";
        strHQL += " or goodsSaleOrder.OperatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " or goodsSaleOrder.SalesCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " Order by goodsSaleOrder.SOID DESC";
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetProjectItemBomVersionType(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strProjectID + " and projectItemBomVersion.VerID = " + strVerID;
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);
        ProjectItemBomVersion projectItemBomVersion = (ProjectItemBomVersion)lst[0];

        return projectItemBomVersion.Type.Trim();
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

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where (workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID + ")";
        strHQL += " Or (workFlow.BusinessType = 'GoodsProduction' and workFlow.BusinessCode = '" + strRelatedID + "')";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

    protected void LoadProductProcess()
    {
        string strHQL = "From ProductProcess as productProcess Order By productProcess.SortNumber ASC";
        ProductProcessBLL productProcessBLL = new ProductProcessBLL();
        IList lst = productProcessBLL.GetAllProductProcesss(strHQL);

        DL_DefaultProcess.DataSource = lst;
        DL_DefaultProcess.DataBind();

        DL_DefaultProcess.Items.Insert(0, new ListItem("--Select--", ""));
    }

}
