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

using TakeTopCore;

public partial class TTGoodsSaleOrderWF : System.Web.UI.Page
{
    string strUserCode;
    string strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID;
    string strMainTableCanAdd, strDetailTableCanAdd, strMainTableCanEdit, strMainTableCanDelete, strDetailTableCanEdit, strDetailTableCanDelete;
    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;

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
        strToDoWLID = Request.QueryString["WLID"]; strToDoWLDetailID= Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "销售订单", strUserCode);
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
            DLC_ArrivalTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SaleTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_OpenDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_OpenInvoiceTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
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

            ShareClass.LoadCurrencyType(DL_CurrencyType);
            ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_GoodsSales);

            ShareClass.LoadWFTemplate(strUserCode, "MaterialSales", DL_TemName);

            LoadGoodsSaleOrder(strUserCode);
            LoadGoodsSaleQuotationOrder(strUserCode);
            LoadUsingConstract(strUserCode);
            LoadSaleType();
            LoadInvoiceType();

            ShareClass.LoadCustomer(DL_Customer, strUserCode);

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

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID;

            strID = e.Item.Cells[0].Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string strHQL;
            IList lst;

            string strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            strHQL = "From Goods as goods where goods.GoodsCode = " + "'" + strGoodsCode + "'";
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);
            Goods goods = (Goods)lst[0];

            TB_GoodsCode.Text = goods.GoodsCode.Trim();
            TB_GoodsName.Text = goods.GoodsName.Trim();

            try
            {
                DL_Type.SelectedValue = goods.Type;
            }
            catch
            {
            }

            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();
            TB_Brand.Text = goods.Manufacturer.Trim();

            NB_Number.Amount = goods.Number;
            DL_Unit.SelectedValue = goods.UnitName;
            NB_Price.Amount = goods.Price;


            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void BT_FindCustomer_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strCustomerName = "%" + TB_CustomerName.Text.Trim() + "%";

        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "from Customer as customer ";
        strHQL += " Where ((customer.CreatorCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " or (customer.CustomerCode in (Select customerRelatedUser.CustomerCode from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " Or customer.CreatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode In  " + strDepartString + "))";
        strHQL += " and customer.CustomerName Like '" + strCustomerName + "'";
        strHQL += " Order by customer.CustomerName ASC";
        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DL_Customer.DataSource = lst;
        DL_Customer.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strConstractCode;

            strConstractCode = ((Button)e.Item.FindControl("BT_ConstractCode")).Text.Trim();

            for (int i = 0; i < DataGrid6.Items.Count; i++)
            {
                DataGrid6.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_ConstractCode.Text = strConstractCode;

            LoadConstractRelatedGoodsList(strConstractCode);

            TabContainer2.ActiveTabIndex = 3;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid10_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strConstractCode;

            strConstractCode = ((Button)e.Item.FindControl("BT_ConstractCode")).Text.Trim();

            for (int i = 0; i < DataGrid10.Items.Count; i++)
            {
                DataGrid10.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_ConstractCode.Text = strConstractCode;

            LoadConstractRelatedGoodsList(strConstractCode);

            TabContainer2.ActiveTabIndex = 3;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strSOID, strRelatedType;
            IList lst;
            int intWLNumber;

            strSOID = e.Item.Cells[3].Text.Trim();

            intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
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
            string strCreateUserCode = getGoodsSaleOrderCreatorCode(strSOID);
            ShareClass.MainTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("WuLiaoXiaoShouChan").ToString().Trim(), strSOID, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strMainTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                BT_NewMain.Visible = true;
                BT_NewDetail.Visible = true;
            }

            if (e.CommandName == "Update" | e.CommandName == "Assign" | e.CommandName == "INVOICE")
            {
                for (int i = 0; i < DataGrid5.Items.Count; i++)
                {
                    DataGrid5.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.SOID = " + strSOID;
                GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
                lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);
                GoodsSaleOrder goodsSaleOrder = (GoodsSaleOrder)lst[0];

                LB_SOID.Text = goodsSaleOrder.SOID.ToString();
                TB_SOName.Text = goodsSaleOrder.SOName.Trim();

                try
                {
                    DL_Customer.SelectedValue = goodsSaleOrder.CustomerCode;
                }
                catch
                {
                }


                DLC_SaleTime.Text = goodsSaleOrder.SaleTime.ToString("yyyy-MM-dd");
                DLC_ArrivalTime.Text = goodsSaleOrder.ArrivalTime.ToString("yyyy-MM-dd");
                NB_Amount.Amount = goodsSaleOrder.Amount;

                DL_CurrencyType.SelectedValue = goodsSaleOrder.CurrencyType;

                DL_SaleType.SelectedValue = goodsSaleOrder.SaleType;
                TB_ContactPerson.Text = goodsSaleOrder.ContactPerson;
                TB_ContactPhoneNumber.Text = goodsSaleOrder.ContactPhoneNumber;
                TB_ReceiverAddress.Text = goodsSaleOrder.ReceiverAddress;
                DL_InvoiceType.SelectedValue = goodsSaleOrder.InvoiceType;

                TB_Comment.Text = goodsSaleOrder.Comment.Trim();
                DL_SOStatus.SelectedValue = goodsSaleOrder.Status.Trim();

                try
                {
                    DL_GoodsSales.SelectedValue = goodsSaleOrder.SalesCode;
                }
                catch
                {

                }

                DL_RelatedType.SelectedValue = goodsSaleOrder.RelatedType.Trim();
                NB_RelatedID.Amount = goodsSaleOrder.RelatedID;

                strRelatedType = goodsSaleOrder.RelatedType.Trim();

                //取得客户专用信息
                GetSaleOrderOtherData(strSOID);

                LoadGoodsSaleOrderDetail(strSOID);
                LoadRelatedConstract(strSOID);
                LoadGoodsSaleQuotationOrder(LB_UserCode.Text.Trim(), goodsSaleOrder.CustomerCode.Trim());

                LoadCustomerRelatedGoodsList(goodsSaleOrder.CustomerCode.Trim());

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


                if (strRelatedType == "Other")
                {
                    BT_Select.Visible = false;
                }
                else
                {
                    BT_Select.Visible = true;
                }

                TB_WLName.Text = LanguageHandle.GetWord("XiaoShou").ToString().Trim() + goodsSaleOrder.SOName.Trim() + LanguageHandle.GetWord("ShenQing").ToString().Trim();

                LoadRelatedWL("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), goodsSaleOrder.SOID);

                BT_SubmitApply.Enabled = true;

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Assign")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
                }

                if (e.CommandName == "INVOICE")
                {
                    LoadConstractRelatedInvoice(strSOID);
                    CountInvoiceAmount(strSOID);
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
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

                try
                {
                    strHQL = "Delete From T_GoodsSaleorder Where SOID = " + strSOID;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_GoodsSaleRecord Where SOID =" + strSOID;
                    ShareClass.RunSqlCommand(strHQL);

                    //Workflow,删除流程模组关联记录
                    ShareClass.DeleteModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("WuLiaoXiaoShouChan").ToString().Trim(), strSOID);

                    LoadGoodsSaleOrder(strUserCode);
                    LoadGoodsSaleOrderDetail(strSOID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    //Workflow,取得单据创建人代码
    protected string getGoodsSaleOrderCreatorCode(string strSOID)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsSaleOrder as goodsSaleOrder Where goodsSaleOrder.SOID = " + strSOID;
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        return ((GoodsSaleOrder)lst[0]).OperatorCode.Trim();
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

    protected void DL_Customer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCustomerCode, strOperatorCode;

        strCustomerCode = DL_Customer.SelectedValue.Trim();
        strOperatorCode = LB_UserCode.Text.Trim();

        LoadGoodsSaleQuotationOrder(strOperatorCode, strCustomerCode);

        LoadCustomerRelatedGoodsList(strCustomerCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode, strItemName, strBomVerID, strUnit, strDefaultProcess;
            decimal deNumber, deReservedNumber;

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }


            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ID = " + strID;
            ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
            lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

            ProjectRelatedItem projectRelatedItem = (ProjectRelatedItem)lst[0];

            strItemCode = projectRelatedItem.ItemCode.Trim();
            strItemName = projectRelatedItem.ItemName.Trim();
            strBomVerID = projectRelatedItem.BomVersionID.ToString();
            strUnit = projectRelatedItem.Unit;
            deNumber = projectRelatedItem.Number;
            deReservedNumber = projectRelatedItem.ReservedNumber;
            strDefaultProcess = projectRelatedItem.DefaultProcess.Trim();


            TB_GoodsCode.Text = strItemCode;
            TB_GoodsName.Text = strItemName;

            try
            {
                DL_Type.SelectedValue = projectRelatedItem.ItemType;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }

            TB_Spec.Text = projectRelatedItem.Specification;
            TB_ModelNumber.Text = projectRelatedItem.ModelNumber;
            TB_Brand.Text = projectRelatedItem.Brand;


            NB_Number.Amount = projectRelatedItem.Number - projectRelatedItem.AleadySale;

            DL_Unit.SelectedValue = strUnit;

            DL_RecordSourceType.SelectedValue = "GoodsPJRecord";
            NB_RecordSourceID.Amount = projectRelatedItem.ID;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DL_RecordSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_RecordSourceID.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void RefreshProjectRelatedItemNumber(string strProjectID)
    {
        LoadProjectRelatedItem(strProjectID);

    }

    protected void LoadProjectRelatedItem(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ProjectID = " + strProjectID + " Order by projectRelatedItem.ID ASC";
        ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
        lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
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
                NB_Number.Amount = projectRelatedItemBom.Number - projectRelatedItemBom.AleadySale;

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

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_SOID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        string strNewSOCode = ShareClass.GetCodeByRule("SaleOrderCode", "SaleOrderCode", "00");
        if (strNewSOCode != "")
        {
            TB_SOName.Text = strNewSOCode;
        }

        LoadGoodsSaleOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strSOID;

        strSOID = LB_SOID.Text.Trim();

        if (strSOID == "")
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
        string strSOID, strCustomerCode, strCustomerName, strSOName, strUserCode, strSalesCode, strSalesName, strOperatorCode, strOperatorName, strCurrencyType, strComment;
        DateTime dtSaleTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();

        strSOName = TB_SOName.Text.Trim();
        strSalesCode = DL_GoodsSales.SelectedValue.Trim();
        if (strSalesCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click222", "alert('" + LanguageHandle.GetWord("ZZJGXSRYBNWKQJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        strSalesName = ShareClass.GetUserName(strSalesCode);
        strOperatorCode = LB_UserCode.Text.Trim();
        strOperatorName = LB_UserName.Text.Trim();

        strCustomerCode = DL_Customer.SelectedValue.Trim();
        strCustomerName = DL_Customer.SelectedItem.Text;

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        strComment = TB_Comment.Text.Trim();
        dtSaleTime = DateTime.Parse(DLC_SaleTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);
        deAmount = NB_Amount.Amount;
        strStatus = DL_SOStatus.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();

        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        GoodsSaleOrder goodsSaleOrder = new GoodsSaleOrder();

        goodsSaleOrder.SOName = strSOName;
        goodsSaleOrder.SalesCode = strSalesCode;
        try
        {
            goodsSaleOrder.SalesName = ShareClass.GetUserName(strSalesCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXSRDMBZCWCRJC").ToString().Trim() + "')", true);
            return;
        }
        goodsSaleOrder.OperatorCode = strOperatorCode;
        goodsSaleOrder.OperatorName = strOperatorName;

        goodsSaleOrder.CustomerCode = strCustomerCode;
        goodsSaleOrder.CustomerName = strCustomerName;

        goodsSaleOrder.SaleType = DL_SaleType.SelectedValue;
        goodsSaleOrder.ContactPerson = TB_ContactPerson.Text.Trim();
        goodsSaleOrder.ContactPhoneNumber = TB_ContactPhoneNumber.Text.Trim();
        goodsSaleOrder.ReceiverAddress = TB_ReceiverAddress.Text.Trim();
        goodsSaleOrder.InvoiceType = DL_InvoiceType.SelectedValue;

        goodsSaleOrder.SaleTime = dtSaleTime;
        goodsSaleOrder.ArrivalTime = dtArrivalTime;
        goodsSaleOrder.Amount = 0;
        goodsSaleOrder.CurrencyType = strCurrencyType;
        goodsSaleOrder.Comment = strComment;
        goodsSaleOrder.Status = "New";

        goodsSaleOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        goodsSaleOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        try
        {
            goodsSaleOrderBLL.AddGoodsSaleOrder(goodsSaleOrder);

            strSOID = ShareClass.GetMyCreatedMaxGoodsSaleOrderID(strOperatorCode);
            LB_SOID.Text = strSOID;

            //更新客户专用信息
            UpdateSaleOrderOtherData(strSOID);

            //自动产生单号
            string strNewSOCode = ShareClass.GetCodeByRule("SaleOrderCode", "SaleOrderCode", strSOID);
            if (strNewSOCode != "")
            {
                TB_SOName.Text = strNewSOCode;
                string strHQL = "Update T_GoodsSaleOrder Set SOName = " + "'" + strNewSOCode + "'" + " Where SOID = " + strSOID;
                ShareClass.RunSqlCommand(strHQL);
            }


            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("WuLiaoXiaoShouChan").ToString().Trim(), strSOID);


            NB_Amount.Amount = 0;
            TB_WLName.Text = LanguageHandle.GetWord("XiaoShou").ToString().Trim() + strSOName + LanguageHandle.GetWord("ShenQing").ToString().Trim();

            BT_SubmitApply.Enabled = true;

            LoadGoodsSaleOrder(strUserCode);
            LoadGoodsSaleOrderDetail(strSOID);
            LoadRelatedConstract(strSOID);

            LoadRelatedWL("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), goodsSaleOrder.SOID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCKNXSMCZD50GHZHBZZSZD100GHZGDJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strSOID, strUserCode, strCustomerCode, strCustomerName, strSOName, strSalesCode, strSalesName, strCurrencyType, strComment;
        DateTime dtSaleTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();

        strSOID = LB_SOID.Text.Trim();
        strSOName = TB_SOName.Text.Trim();

        strCustomerCode = DL_Customer.SelectedValue.Trim();
        strCustomerName = DL_Customer.SelectedItem.Text;

        strSalesCode = DL_GoodsSales.SelectedValue.Trim();
        if (strSalesCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXSRYBNWKQJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        strSalesName = ShareClass.GetUserName(strSalesCode);

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

        goodsSaleOrder.CustomerCode = strCustomerCode;
        goodsSaleOrder.CustomerName = strCustomerName;

        goodsSaleOrder.SalesCode = strSalesCode;
        try
        {
            goodsSaleOrder.SalesName = ShareClass.GetUserName(strSalesCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXSRDMBZCWCRJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        goodsSaleOrder.SaleType = DL_SaleType.SelectedValue;
        goodsSaleOrder.ContactPerson = TB_ContactPerson.Text.Trim();
        goodsSaleOrder.ContactPhoneNumber = TB_ContactPhoneNumber.Text.Trim();
        goodsSaleOrder.ReceiverAddress = TB_ReceiverAddress.Text.Trim();
        goodsSaleOrder.InvoiceType = DL_InvoiceType.SelectedValue;

        goodsSaleOrder.SaleTime = dtSaleTime;
        goodsSaleOrder.ArrivalTime = dtArrivalTime;
        goodsSaleOrder.Amount = deAmount;
        goodsSaleOrder.CurrencyType = strCurrencyType;
        goodsSaleOrder.Comment = strComment;
        goodsSaleOrder.Status = strStatus;

        goodsSaleOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        goodsSaleOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        try
        {
            goodsSaleOrderBLL.UpdateGoodsSaleOrder(goodsSaleOrder, int.Parse(strSOID));

            //更新客户专用信息
            UpdateSaleOrderOtherData(strSOID);
            LoadGoodsSaleOrder(strUserCode);


            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select SOID as DetailSOID, * from T_GoodsSaleOrder where SOID = " + strSOID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
                }

                if (strToDoWLID != null)
                {
                    if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strGoodsCode, strGoodsName, strSpec, strModelNumber, strType;

        TabContainer2.ActiveTabIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strType = "%" + strType + "%";
        strSpec = "%" + strSpec + "%";
        strModelNumber = "%" + strModelNumber + "%";

        strHQL = "Select GoodsCode,GoodsName,Type,ModelNumber,Spec,Manufacturer,UnitName,Price, coalesce(Sum(Number),0) as TotalNumber From T_Goods";
        strHQL += " Where GoodsCode Like " + "'" + strGoodsCode + "'";
        strHQL += " and GoodsName Like " + "'" + strGoodsName + "'";
        strHQL += " and Type Like " + "'" + strType + "'";
        strHQL += " and ModelNumber Like " + "'" + strModelNumber + "'";
        strHQL += " and Spec Like " + "'" + strSpec + "'";
        strHQL += " Group By  GoodsCode,GoodsName,Type,ModelNumber,Spec,Manufacturer,UnitName,Price";
        strHQL += " Order By coalesce(Sum(Number),0) Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        int intCount = ds.Tables[0].Rows.Count;

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid9.DataSource = ds;
        DataGrid9.DataBind();

        intCount = ds.Tables[0].Rows.Count;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";

        NB_Number.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid9_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid9.Items.Count; i++)
            {
                DataGrid9.Items[i].ForeColor = Color.Black;
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

                try
                {
                    DL_Type.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_Type.SelectedValue = "";
                }

                DL_Unit.SelectedValue = item.Unit;
                TB_ModelNumber.Text = item.ModelNumber.Trim();
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

                NB_Price.Amount = item.SalePrice;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }


    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strQOID;

            TabContainer2.ActiveTabIndex = 2;


            strQOID = ((Button)e.Item.FindControl("BT_QOID")).Text.Trim();

            for (int i = 0; i < DataGrid7.Items.Count; i++)
            {
                DataGrid7.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_QOID.Text = strQOID;

            LoadGoodsSaleQuotationOrderDetail(strQOID);

            TabContainer2.ActiveTabIndex = 2;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID;
            IList lst;

            TabContainer1.ActiveTabIndex = 0;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid8.Items.Count; i++)
            {
                DataGrid8.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsSaleQuotationOrderDetail as goodsSaleQuotationOrderDetail where goodsSaleQuotationOrderDetail.ID = " + strID;

            GoodsSaleQuotationOrderDetailBLL goodsSaleQuotationOrderDetailBLL = new GoodsSaleQuotationOrderDetailBLL();
            lst = goodsSaleQuotationOrderDetailBLL.GetAllGoodsSaleQuotationOrderDetails(strHQL);
            GoodsSaleQuotationOrderDetail goodsSaleQuotationOrderDetail = (GoodsSaleQuotationOrderDetail)lst[0];

            TB_GoodsCode.Text = goodsSaleQuotationOrderDetail.GoodsCode;
            TB_GoodsName.Text = goodsSaleQuotationOrderDetail.GoodsName;
            TB_ModelNumber.Text = goodsSaleQuotationOrderDetail.ModelNumber.Trim();
            TB_Spec.Text = goodsSaleQuotationOrderDetail.Spec;
            TB_Brand.Text = goodsSaleQuotationOrderDetail.Brand;

            NB_Price.Amount = goodsSaleQuotationOrderDetail.Price;

            DL_Type.SelectedValue = goodsSaleQuotationOrderDetail.Type;
            DL_Unit.SelectedValue = goodsSaleQuotationOrderDetail.Unit;
            NB_Number.Amount = goodsSaleQuotationOrderDetail.Number;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid24_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strConstractCode;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strConstractCode = LB_ConstractCode.Text.Trim();

            for (int i = 0; i < DataGrid24.Items.Count; i++)
            {
                DataGrid24.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from ConstractRelatedGoods as constractRelatedGoods Where constractRelatedGoods.ConstractCode = " + "'" + strConstractCode + "'";

            ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
            lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);
            ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];

            TB_GoodsCode.Text = constractRelatedGoods.GoodsCode;
            TB_GoodsName.Text = constractRelatedGoods.GoodsName;
            TB_ModelNumber.Text = constractRelatedGoods.ModelNumber;
            TB_Spec.Text = constractRelatedGoods.Spec;
            TB_Brand.Text = constractRelatedGoods.Brand;

            DL_Type.SelectedValue = constractRelatedGoods.Type;
            DL_Unit.SelectedValue = constractRelatedGoods.Unit;
            NB_Number.Amount = constractRelatedGoods.Number;
            DL_Unit.SelectedValue = constractRelatedGoods.Unit;
            NB_Price.Amount = constractRelatedGoods.Price;

            //LB_SourceRelatedID.Text = constractRelatedGoods.ID.ToString();
            //DL_RecordSourceType.SelectedValue = "GoodsCSRecord";
            //NB_RecordSourceID.Amount = constractRelatedGoods.ID;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void LoadGoodsSaleQuotationOrder(string strOperatorCode, string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.OperatorCode = " + "'" + strOperatorCode + "'";
        strHQL += " and goodsSaleQuotationOrder.CustomerCode = " + "'" + strCustomerCode + "'";
        strHQL += " Order by goodsSaleQuotationOrder.QOID DESC";
        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        DataGrid7.DataSource = lst;
        DataGrid7.DataBind();
    }

    protected void LoadGoodsSaleQuotationOrder(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.OperatorCode = " + "'" + strOperatorCode + "'" + " Order by goodsSaleQuotationOrder.QOID DESC";
        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        DataGrid7.DataSource = lst;
        DataGrid7.DataBind();
    }

    protected void LoadGoodsSaleQuotationOrderDetail(string strQOID)
    {
        string strHQL = "Select * from T_GoodsSaleQuotationOrderDetail where QOID = " + strQOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleQuotationOrderDetail");

        DataGrid8.DataSource = ds;
        DataGrid8.DataBind();
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strSOID;
            IList lst;

            strSOID = LB_SOID.Text.Trim();

            int intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
            if (intWLNumber > 0)
            {
                BT_NewDetail.Visible = false;
            }
            else
            {
                BT_NewDetail.Visible = true;
            }

            //WorkFlow,如果此单和工作流相关，那么依工作流状态决定能否保存单据数据
            ShareClass.DetailTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("WuLiaoXiaoShouChan").ToString().Trim(), strSOID, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strDetailTableCanAdd, strDetailTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID, "0");
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

                strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.ID = " + strID;

                GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
                lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);
                GoodsSaleRecord goodsSaleRecord = (GoodsSaleRecord)lst[0];

                TB_GoodsCode.Text = goodsSaleRecord.GoodsCode;
                TB_GoodsName.Text = goodsSaleRecord.GoodsName;
                TB_ModelNumber.Text = goodsSaleRecord.ModelNumber.Trim();
                TB_Spec.Text = goodsSaleRecord.Spec;
                TB_Brand.Text = goodsSaleRecord.Brand;

                TB_SaleReason.Text = goodsSaleRecord.SaleReason;
                NB_Price.Amount = goodsSaleRecord.Price;
                DL_Type.SelectedValue = goodsSaleRecord.Type;
                DL_Unit.SelectedValue = goodsSaleRecord.Unit;
                NB_Number.Amount = goodsSaleRecord.Number;

                DL_RecordSourceType.SelectedValue = goodsSaleRecord.SourceType.Trim();
                NB_RecordSourceID.Amount = goodsSaleRecord.SourceID;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                    return;
                }


                //Workflow,如果存在关联工作流，那么要执行下面的代码
                string strCreateUserCode;
                strCreateUserCode = getGoodsSaleOrderCreatorCode(strSOID);
                if (!ShareClass.DetailTableDeleteWorkflowRelatedModule(strUserCode, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, strDetailTableCanDelete))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZSBNWQSCQJC").ToString().Trim() + "')", true);
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    return;
                }


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

                    string strSourceType;
                    int intSourceID;

                    strSourceType = goodsSaleRecord.SourceType.Trim();
                    intSourceID = goodsSaleRecord.SourceID;
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
                        ShareClass.UpdateProjectRelatedItemNumberByBudgetBusinessType("SALE", strRelatedType, strRelatedID, goodsSaleRecord.GoodsCode.Trim());
                        RefreshProjectRelatedItemNumber(strRelatedID);
                    }

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText;

                        strCmdText = "select SOID as DetailSOID, * from T_GoodsSaleOrder where SOID = " + strSOID;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
                        }

                        if (strToDoWLID != null)
                        {
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }

                        if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                        {
                            strCmdText = "select * from T_GoodsSaleRecord where SOID = " + strSOID;
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


    protected void UpdatProjectRelatedItemNumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber;

        if (strSourceType == "GoodsPJRecord")
        {
            strHQL = "Select coalesce(Sum(Number),0) From T_GoodsSaleRecord Where SourceType = 'GoodsPJRecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadySale = " + deSumNumber.ToString() + " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strSOID;

        strSOID = LB_SOID.Text.Trim();

        if (strSOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strSOID = LB_SOID.Text.Trim();
        int intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
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
        string strRecordID, strSOID, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;
        decimal decNumber;
        DateTime dtBuyTime;
        decimal dePrice;
        string strSaleReason;
        string strRelatedType, strRelatedID;

        string strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        int intSourceID = int.Parse(NB_RecordSourceID.Amount.ToString());


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
      

        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

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
            goodsSaleRecord.Amount = decNumber * dePrice;
            goodsSaleRecord.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            goodsSaleRecord.ModelNumber = strModelNumber;
            goodsSaleRecord.Spec = strSpec;
            goodsSaleRecord.Brand = TB_Brand.Text;
            goodsSaleRecord.SaleReason = strSaleReason;
            goodsSaleRecord.CheckOutNumber = 0;
            goodsSaleRecord.DeliveryNumber = 0;
            goodsSaleRecord.NoticeOutNumber = 0;
            goodsSaleRecord.RealReceiveNumber = 0;

            goodsSaleRecord.SourceType = strSourceType;
            goodsSaleRecord.SourceID = intSourceID;

            try
            {
                goodsSaleRecordBLL.AddGoodsSaleRecord(goodsSaleRecord);

                strRecordID = ShareClass.GetMyCreatedMaxGoodsSaleRecordID(strSOID);
                LB_ID.Text = strRecordID;

                LoadGoodsSaleOrderDetail(strSOID);

                NB_Amount.Amount = SumGoodsSaleOrderAmount(strSOID);
                UpdateGoodsSaleOrderAmount(strSOID, NB_Amount.Amount);

                //string strSourceType;
                //int intSourceID;

                strSourceType = goodsSaleRecord.SourceType.Trim();
                intSourceID = goodsSaleRecord.SourceID;
                //更改项目关联物资下单量
                if (strSourceType == "GoodsPJRecord")
                {
                    UpdatProjectRelatedItemNumber(strSourceType, intSourceID.ToString());
                }

                //依单据主体关联类型更新项目物资预算的物料代码的预算使用量
                //string strRelatedType = DL_RelatedType.SelectedValue.Trim();
                //string strRelatedID = NB_RelatedID.Amount.ToString();
                if (DL_RelatedType.SelectedValue.Trim() == "Project")
                {
                    ShareClass.UpdateProjectRelatedItemNumberByBudgetBusinessType("SALE", strRelatedType, strRelatedID, goodsSaleRecord.GoodsCode.Trim());
                    RefreshProjectRelatedItemNumber(strRelatedID);
                }


                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select SOID as DetailSOID, * from T_GoodsSaleOrder where SOID = " + strSOID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsSaleRecord where SOID = " + strSOID;
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

    protected void UpdateDetail()
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strSaleReason, strUnitName;
        DateTime dtBuyTime;
        decimal dePrice, deNumber;
        string strRelatedType, strRelatedID;

        string strID, strSOID, strSalesCode, strSalesName;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        string strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        int intSourceID = int.Parse(NB_RecordSourceID.Amount.ToString());

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
        deNumber = NB_Number.Amount;
        dtBuyTime = DateTime.Now;

        strSalesCode = DL_GoodsSales.SelectedValue.Trim();
        strSalesName = ShareClass.GetUserName(strSalesCode);


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

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
            goodsSaleRecord.Amount = deNumber * dePrice;
            goodsSaleRecord.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            goodsSaleRecord.ModelNumber = strModelNumber;
            goodsSaleRecord.Spec = strSpec;
            goodsSaleRecord.Brand = TB_Brand.Text;

            goodsSaleRecord.SaleReason = strSaleReason;

            goodsSaleRecord.SourceType = strSourceType;
            goodsSaleRecord.SourceID = intSourceID;

            try
            {
                goodsSaleRecordBLL.UpdateGoodsSaleRecord(goodsSaleRecord, int.Parse(strID));

                LoadGoodsSaleOrderDetail(strSOID);

                NB_Amount.Amount = SumGoodsSaleOrderAmount(strSOID);
                UpdateGoodsSaleOrderAmount(strSOID, NB_Amount.Amount);

                //string strSourceType;
                //int intSourceID;

                strSourceType = goodsSaleRecord.SourceType.Trim();
                intSourceID = goodsSaleRecord.SourceID;
                //更改项目关联物资下单量
                if (strSourceType == "GoodsPJRecord")
                {
                    UpdatProjectRelatedItemNumber(strSourceType, intSourceID.ToString());
                }

                //依单据主体关联类型更新项目物资预算的物料代码的预算使用量
                //string strRelatedType = DL_RelatedType.SelectedValue.Trim();
                //string strRelatedID = NB_RelatedID.Amount.ToString();
                if (DL_RelatedType.SelectedValue.Trim() == "Project")
                {
                    ShareClass.UpdateProjectRelatedItemNumberByBudgetBusinessType("SALE", strRelatedType, strRelatedID, goodsSaleRecord.GoodsCode.Trim());
                    RefreshProjectRelatedItemNumber(strRelatedID);
                }

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select SOID as DetailSOID, * from T_GoodsSaleOrder where SOID = " + strSOID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsSaleRecord where SOID = " + strSOID;
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
        workFlow.RelatedType = LanguageHandle.GetWord("WuLiao").ToString().Trim();
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

            LoadRelatedWL(strWLType, LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strSOID));

            UpdateGoodsGoodsSaleStatus(strSOID, "InProgress");
            DL_SOStatus.SelectedValue = "InProgress";

            strCmdText = "select SOID as DetailSOID, * from T_GoodsSaleOrder where SOID = " + strSOID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsSaleOrder", strXMLFile2);


            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strWLID, "0", "0", LanguageHandle.GetWord("WuLiaoXiaoShouChan").ToString().Trim(), strSOID);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPXSSSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPXSSSBKNGZLMCGCZD25GHZJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

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

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleOrder");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();
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

    protected void DataGrid11_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strCustomerCode;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strCustomerCode = DL_Customer.SelectedValue.Trim();

            for (int i = 0; i < DataGrid11.Items.Count; i++)
            {
                DataGrid11.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from CustomerRelatedGoodsInfor as customerRelatedGoodsInfor Where customerRelatedGoodsInfor.ID = " + strID;

            CustomerRelatedGoodsInforBLL customerRelatedGoodsInforBLL = new CustomerRelatedGoodsInforBLL();
            lst = customerRelatedGoodsInforBLL.GetAllCustomerRelatedGoodsInfors(strHQL);
            CustomerRelatedGoodsInfor customerRelatedGoodsInfor = (CustomerRelatedGoodsInfor)lst[0];

            TB_GoodsCode.Text = customerRelatedGoodsInfor.GoodsCode;
            TB_GoodsName.Text = customerRelatedGoodsInfor.GoodsName;
            TB_ModelNumber.Text = customerRelatedGoodsInfor.ModelNumber;
            TB_Spec.Text = customerRelatedGoodsInfor.Spec;
            TB_Brand.Text = customerRelatedGoodsInfor.Brand;

            DL_Type.SelectedValue = customerRelatedGoodsInfor.Type;
            DL_Unit.SelectedValue = customerRelatedGoodsInfor.Unit;

            NB_Price.Amount = customerRelatedGoodsInfor.Price;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid12_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strUserCode;
            string strID;

            strUserCode = LB_UserCode.Text.Trim();

            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid12.Items.Count; i++)
                {
                    DataGrid12.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;


                strHQL = "From ConstractRelatedInvoice as constractRelatedInvoice Where constractRelatedInvoice.ID = " + strID;
                ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
                lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);

                ConstractRelatedInvoice constractRelatedInvoice = (ConstractRelatedInvoice)lst[0];


                LB_InvoiceID.Text = constractRelatedInvoice.ID.ToString();
                DL_InvoiceReceiveOPen.SelectedValue = constractRelatedInvoice.ReceiveOpen.Trim();
                DL_TaxType.SelectedValue = constractRelatedInvoice.TaxType.Trim();
                TB_InvoiceCode.Text = constractRelatedInvoice.InvoiceCode.Trim();
                NB_InvoiceAmount.Amount = constractRelatedInvoice.Amount;
                NB_InvoiceTaxRate.Amount = constractRelatedInvoice.TaxRate;

                DLC_OpenDate.Text = constractRelatedInvoice.OpenDate.ToString("yyyy-MM-dd");

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true','popInvoiceDetailWindow') ", true);

            }

            if (e.CommandName == "Delete")
            {
                string strInvoiceCode, strSOID;
                decimal deAmount, deTaxRate;

                strSOID = LB_SOID.Text.Trim();
                strInvoiceCode = TB_InvoiceCode.Text.Trim();
                deAmount = NB_InvoiceAmount.Amount;
                deTaxRate = NB_InvoiceTaxRate.Amount;

                ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
                strHQL = "From ConstractRelatedInvoice as constractRelatedInvoice Where constractRelatedInvoice.ID = " + strID;
                lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);
                ConstractRelatedInvoice constractRelatedInvoice = (ConstractRelatedInvoice)lst[0];

                constractRelatedInvoice.ConstractCode = strSOID;
                constractRelatedInvoice.InvoiceCode = TB_InvoiceCode.Text.Trim();
                constractRelatedInvoice.Amount = deAmount;
                constractRelatedInvoice.TaxRate = deTaxRate;

                try
                {
                    constractRelatedInvoiceBLL.DeleteConstractRelatedInvoice(constractRelatedInvoice);
                    LoadConstractRelatedInvoice(strSOID);

                    CountInvoiceAmount(strSOID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true') ", true);
            }
        }
    }


    protected void BT_OpenInvoice_Click(object sender, EventArgs e)
    {
        LB_InvoiceID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true','popInvoiceDetailWindow') ", true);
    }

    protected void BT_Invoice_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_InvoiceID.Text.Trim();

        if (strID == "")
        {
            AddInvoice();
        }
        else
        {
            UpdateInvoice();
        }
    }

    protected void AddInvoice()
    {
        string strInvoiceCode, strSOID, strTaxType;
        decimal deAmount, deTaxRate;
        DateTime dtOpenDate;

        strSOID = LB_SOID.Text.Trim();
        strInvoiceCode = TB_InvoiceCode.Text.Trim();
        strTaxType = DL_TaxType.SelectedValue.Trim();
        deAmount = NB_InvoiceAmount.Amount;
        deTaxRate = NB_InvoiceTaxRate.Amount;
        dtOpenDate = DateTime.Parse(DLC_OpenDate.Text);

        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        ConstractRelatedInvoice constractRelatedInvoice = new ConstractRelatedInvoice();

        constractRelatedInvoice.ReceiveOpen = DL_InvoiceReceiveOPen.SelectedValue.Trim();
        constractRelatedInvoice.ConstractCode = "";
        constractRelatedInvoice.InvoiceCode = TB_InvoiceCode.Text.Trim();
        constractRelatedInvoice.TaxType = strTaxType;
        constractRelatedInvoice.Amount = deAmount;
        constractRelatedInvoice.TaxRate = deTaxRate;
        constractRelatedInvoice.OpenDate = dtOpenDate;
        constractRelatedInvoice.RelatedType = "SO";
        constractRelatedInvoice.RelatedID = strSOID;

        try
        {
            constractRelatedInvoiceBLL.AddConstractRelatedInvoice(constractRelatedInvoice);
            LoadConstractRelatedInvoice(strSOID);

            CountInvoiceAmount(strSOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true') ", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true','popInvoiceDetailWindow') ", true);
        }
    }

    protected void UpdateInvoice()
    {
        string strHQL;
        IList lst;

        string strID, strInvoiceCode, strSOID, strTaxType;
        decimal deAmount, deTaxRate;
        DateTime dtOpenDate;

        strID = LB_InvoiceID.Text.Trim();
        strSOID = LB_SOID.Text.Trim();
        strTaxType = DL_TaxType.SelectedValue.Trim();
        strInvoiceCode = TB_InvoiceCode.Text.Trim();
        deAmount = NB_InvoiceAmount.Amount;
        deTaxRate = NB_InvoiceTaxRate.Amount;
        dtOpenDate = DateTime.Parse(DLC_OpenDate.Text);

        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        strHQL = "From ConstractRelatedInvoice as constractRelatedInvoice Where constractRelatedInvoice.ID = " + strID;
        lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);
        ConstractRelatedInvoice constractRelatedInvoice = (ConstractRelatedInvoice)lst[0];

        constractRelatedInvoice.ReceiveOpen = DL_InvoiceReceiveOPen.SelectedValue.Trim();
        constractRelatedInvoice.ConstractCode = "";
        constractRelatedInvoice.InvoiceCode = TB_InvoiceCode.Text.Trim();
        constractRelatedInvoice.TaxType = strTaxType;
        constractRelatedInvoice.Amount = deAmount;
        constractRelatedInvoice.TaxRate = deTaxRate;
        constractRelatedInvoice.OpenDate = dtOpenDate;
        constractRelatedInvoice.RelatedType = "SO";
        constractRelatedInvoice.RelatedID = strSOID;

        try
        {
            constractRelatedInvoiceBLL.UpdateConstractRelatedInvoice(constractRelatedInvoice, int.Parse(strID));

            LoadConstractRelatedInvoice(strSOID);

            CountInvoiceAmount(strSOID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popInvoiceWindow','true','popInvoiceDetailWindow') ", true);
        }
    }

    protected void CountInvoiceAmount(string strSOID)
    {
        string strHQL;
        IList lst;

        decimal deOpenInvoiceAmount = 0, deReceiveInvoiceAmount = 0;
        string strType;

        strHQL = "from ConstractRelatedInvoice as constractRelatedInvoice where constractRelatedInvoice.RelatedType = 'SO' And RelatedID = " + "'" + strSOID + "'";
        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);

        ConstractRelatedInvoice constractRelatedInvoice = new ConstractRelatedInvoice();

        for (int i = 0; i < lst.Count; i++)
        {
            constractRelatedInvoice = (ConstractRelatedInvoice)lst[i];

            strType = constractRelatedInvoice.ReceiveOpen.Trim();

            if (strType == "OPEN")
            {

                deOpenInvoiceAmount += constractRelatedInvoice.Amount;
            }

            if (strType == "RECEIVE")
            {
                deReceiveInvoiceAmount += constractRelatedInvoice.Amount;
            }
        }


        LB_TotalOpenInvoiceAmount.Text = deOpenInvoiceAmount.ToString();
        LB_TotalReceiveInvoiceAmount.Text = deReceiveInvoiceAmount.ToString();

    }

    //取得订单专用信息
    protected void GetSaleOrderOtherData(string strOrderID)
    {
        string strHQL;

        strHQL = "Select CarCode,GoodsOrigin,PackWarehouse,CustomerInformation,OpenInvoiceTime,InvoiceCode From T_GoodsSaleOrder";
        strHQL += " Where SOID = " + strOrderID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleOrder");

        DLC_OpenInvoiceTime.Text = ds.Tables[0].Rows[0]["OpenInvoiceTime"].ToString();
        TB_InvoiceCodeString.Text = ds.Tables[0].Rows[0]["InvoiceCode"].ToString();
    }

    //更新订单专用信息
    protected void UpdateSaleOrderOtherData(string strOrderID)
    {
        string strHQL;

        string strOpenInvoiceTime, strInvoiceCode;

        strOpenInvoiceTime = DLC_OpenInvoiceTime.Text;
        strInvoiceCode = TB_InvoiceCodeString.Text;

        strHQL = "Update T_GoodsSaleOrder Set ";
        strHQL += "OpenInvoiceTime = '" + DLC_OpenInvoiceTime.Text + "',";
        strHQL += "InvoiceCode = '" + strInvoiceCode + "'";
        strHQL += " Where SOID = " + strOrderID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {

        }
    }


    protected void LoadConstractRelatedInvoice(string strSOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractRelatedInvoice as constractRelatedInvoice where constractRelatedInvoice.RelatedType = 'SO' And RelatedID = " + "'" + strSOID + "'";
        ConstractRelatedInvoiceBLL constractRelatedInvoiceBLL = new ConstractRelatedInvoiceBLL();
        lst = constractRelatedInvoiceBLL.GetAllConstractRelatedInvoices(strHQL);

        DataGrid12.DataSource = lst;
        DataGrid12.DataBind();
    }


    protected void LoadCustomerRelatedGoodsList(string strCustomerCode)
    {
        string strHQL;
        IList lst;


        CustomerRelatedGoodsInforBLL customerRelatedGoodsInforBLL = new CustomerRelatedGoodsInforBLL();
        strHQL = "from CustomerRelatedGoodsInfor as customerRelatedGoodsInfor where customerRelatedGoodsInfor.CustomerCode = " + "'" + strCustomerCode + "'";
        lst = customerRelatedGoodsInforBLL.GetAllCustomerRelatedGoodsInfors(strHQL);

        DataGrid11.DataSource = lst;
        DataGrid11.DataBind();
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

    protected void LoadGoodsSaleOrder(string strOperatorCode)
    {
        string strHQL;

        //Workflow,对流程相关模组作判断
        if (strRelatedWorkflowID == null)
        {
            strHQL = "Select * from T_GoodsSaleOrder where ";
            strHQL += "(OperatorCode = " + "'" + strOperatorCode + "'";
            strHQL += " or OperatorCode in (select UnderCode from T_MemberLevel where UserCode = " + "'" + strOperatorCode + "'" + ")) ";
        }
        else
        {
            strHQL = "Select * from T_GoodsSaleOrder where ";
            strHQL += "SOID in (Select RelatedID  From T_WorkFlowRelatedModule Where RelatedModuleName = 'MaterialSalesOrder' and WorkflowID =" + strRelatedWorkflowID + ")";   
        }
        strHQL += " Order by SOID DESC";

        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "Select * from T_GoodsSaleOrder where SOID = " + strWLBusinessID;
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleOrder");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
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

    protected void LoadGoodsSaleOrderDetail(string strSOID)
    {
        string strHQL = "Select * from T_GoodsSaleRecord where SOID = " + strSOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadConstractRelatedGoodsList(string strConstractCode)
    {
        string strHQL;
        IList lst;

        ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
        strHQL = "from ConstractRelatedGoods as constractRelatedGoods where constractRelatedGoods.ConstractCode = " + "'" + strConstractCode + "'";
        lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);

        DataGrid24.DataSource = lst;
        DataGrid24.DataBind();
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

    protected void LoadRelatedConstract(string strSOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.Status <> 'Archived'";
        strHQL += " and constract.ConstractCode in (select constractRelatedGoodsSaleOrder.ConstractCode from ConstractRelatedGoodsSaleOrder as constractRelatedGoodsSaleOrder where constractRelatedGoodsSaleOrder.SOID = " + strSOID + ")";
        strHQL += " Order by constract.SignDate DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();
    }

    protected void LoadUsingConstract(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where  constract.Status not in ('Archived','Deleted') ";
        strHQL += " and (constract.RecorderCode = " + "'" + strUserCode + "'" + " Or constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        DataGrid10.DataSource = lst;
        DataGrid10.DataBind();
    }

    protected void LoadSaleType()
    {
        string strHQL;

        strHQL = "Select Type From T_SaleType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_PackingType");

        DL_SaleType.DataSource = ds;
        DL_SaleType.DataBind();

        DL_SaleType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadInvoiceType()
    {
        string strHQL;

        strHQL = "Select Type From T_InvoiceType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_PackingType");

        DL_InvoiceType.DataSource = ds;
        DL_InvoiceType.DataBind();

        DL_InvoiceType.Items.Insert(0, new ListItem("--Select--", ""));
    }
}


