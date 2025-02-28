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

public partial class TTGoodsSaleQuotationOrder : System.Web.UI.Page
{
    string strUserCode;
    string strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID;
    private string strMainTableCanAdd, strDetailTableCanAdd, strMainTableCanEdit, strMainTableCanDelete, strDetailTableCanEdit, strDetailTableCanDelete;
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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
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
            DLC_QuotationTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ValidityPeriod.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

            ShareClass.LoadWFTemplate(strUserCode, "MaterialQuotation", DL_TemName);

            LoadGoodsSaleQuotationOrder(strUserCode);
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
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

        //DL_Customer.Items.Insert(0, new ListItem("--Select--", ""));

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

            string strGoodsCode;

            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();
            strHQL = "From Goods as goods Where goods.GoodsCode = " + "'" + strGoodsCode + "'";
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            Goods goods = (Goods)lst[0];

            DL_Type.SelectedValue = goods.Type;
            TB_GoodsCode.Text = strGoodsCode;
            TB_GoodsName.Text = goods.GoodsName.Trim();
            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();
            TB_Brand.Text = goods.Manufacturer;

            NB_Number.Amount = goods.Number;
            DL_Unit.SelectedValue = goods.UnitName;
            NB_Price.Amount = goods.Price;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strQOID;
            IList lst;
            int intWLNumber;

            strQOID = e.Item.Cells[3].Text.Trim();

            intWLNumber = GetRelatedWorkFlowNumber("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
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
            string strCreateUserCode = GetGoodsSaleQuotationOrderCreatorCode(strQOID);
            ShareClass.MainTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("WuLiaoBaoJiaChan").ToString().Trim(), strQOID, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strMainTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID, "0");
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

                strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.QOID = " + strQOID;
                GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
                lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);
                GoodsSaleQuotationOrder goodsSaleQuotationOrder = (GoodsSaleQuotationOrder)lst[0];

                LB_QOID.Text = goodsSaleQuotationOrder.QOID.ToString();
                TB_QOName.Text = goodsSaleQuotationOrder.QOName.Trim();

                try
                {
                    DL_Customer.SelectedValue = goodsSaleQuotationOrder.CustomerCode;
                }
                catch
                {
                }


                NB_Amount.Amount = goodsSaleQuotationOrder.Amount;

                DL_CurrencyType.SelectedValue = goodsSaleQuotationOrder.CurrencyType;

                TB_Comment.Text = goodsSaleQuotationOrder.Comment.Trim();
                DL_QOStatus.SelectedValue = goodsSaleQuotationOrder.Status.Trim();
                TB_SalesCode.Text = goodsSaleQuotationOrder.OperatorCode.Trim();
                LB_SalesName.Text = goodsSaleQuotationOrder.OperatorName.Trim();

                DLC_QuotationTime.Text = goodsSaleQuotationOrder.QuotationTime.ToString("yyyy-MM-dd");
                DLC_ValidityPeriod.Text = goodsSaleQuotationOrder.ValidityPeriod.ToString("yyyy-MM-dd");

                DL_RelatedType.SelectedValue = goodsSaleQuotationOrder.RelatedType.Trim();
                NB_RelatedID.Amount = goodsSaleQuotationOrder.RelatedID;

                LoadGoodsSaleQuotationOrderDetail(strQOID);

                TB_WLName.Text = LanguageHandle.GetWord("BaoJia").ToString().Trim() + goodsSaleQuotationOrder.QOName.Trim() + LanguageHandle.GetWord("ShenQing").ToString().Trim();

                LoadRelatedWL("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), goodsSaleQuotationOrder.QOID);


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
                intWLNumber = GetRelatedWorkFlowNumber("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
                if (intWLNumber > 0)
                {
                    return;
                }

                //Workflow,如果存在关联工作流，那么要执行下面的代码
                if (!ShareClass.MainTableDeleteWorkflowRelatedModule(strUserCode, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, strMainTableCanDelete))
                {
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZSBNWQSCQJC").ToString().Trim() + "')", true);
                        return;
                    }
                }

                try
                {
                    strHQL = "Delete From T_GoodsSaleQuotationOrder Where QOID = " + strQOID;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_GoodsSaleQuotationOrderDetail Where QOID = " + strQOID;
                    ShareClass.RunSqlCommand(strHQL);

                    //Workflow,删除流程模组关联记录
                    ShareClass.DeleteModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("WuLiaoBaoJiaChan").ToString().Trim(), strQOID);


                    BT_SubmitApply.Enabled = false;

                    LoadGoodsSaleQuotationOrder(strUserCode);
                    LoadGoodsSaleQuotationOrderDetail(strQOID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_QOID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        string strNewQOCode = ShareClass.GetCodeByRule("QuotationOrderCode", "QuotationOrderCode", "00");
        if (strNewQOCode != "")
        {
            TB_QOName.Text = strNewQOCode;
        }

        LoadGoodsSaleQuotationOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strQOID;

        strQOID = LB_QOID.Text.Trim();

        if (strQOID == "")
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
        string strQOID, strCustomerCode, strCustomerName, strQOName, strSalesCode, strSalesName, strOperatorCode, strOperatorName, strCurrencyType, strComment;
        DateTime dtQuotationTime, dtValidityPeriod;
        decimal deAmount;
        string strStatus;

        strQOName = TB_QOName.Text.Trim();
        strSalesCode = TB_SalesCode.Text.Trim();
        strSalesName = LB_SalesName.Text.Trim();
        strOperatorCode = LB_UserCode.Text.Trim();
        strOperatorName = LB_UserName.Text.Trim();

        strCustomerCode = DL_Customer.SelectedValue.Trim();
        strCustomerName = DL_Customer.SelectedItem.Text;

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        strComment = TB_Comment.Text.Trim();
        dtQuotationTime = DateTime.Parse(DLC_QuotationTime.Text);
        dtValidityPeriod = DateTime.Parse(DLC_ValidityPeriod.Text);

        deAmount = NB_Amount.Amount;
        strStatus = DL_QOStatus.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();

        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        GoodsSaleQuotationOrder goodsSaleQuotationOrder = new GoodsSaleQuotationOrder();

        goodsSaleQuotationOrder.QOName = strQOName;
        goodsSaleQuotationOrder.SalesCode = strSalesCode;
        try
        {
            goodsSaleQuotationOrder.SalesName = ShareClass.GetUserName(strSalesCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWBJRDMBZCWCRJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
        goodsSaleQuotationOrder.OperatorCode = strOperatorCode;
        goodsSaleQuotationOrder.OperatorName = strOperatorName;

        goodsSaleQuotationOrder.CustomerCode = strCustomerCode;
        goodsSaleQuotationOrder.CustomerName = strCustomerName;

        goodsSaleQuotationOrder.QuotationTime = dtQuotationTime;
        goodsSaleQuotationOrder.ValidityPeriod = dtValidityPeriod;


        goodsSaleQuotationOrder.Amount = 0;
        goodsSaleQuotationOrder.CurrencyType = strCurrencyType;
        goodsSaleQuotationOrder.Comment = strComment;
        goodsSaleQuotationOrder.Status = "New";

        goodsSaleQuotationOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        goodsSaleQuotationOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        try
        {
            goodsSaleQuotationOrderBLL.AddGoodsSaleQuotationOrder(goodsSaleQuotationOrder);

            strQOID = ShareClass.GetMyCreatedMaxGoodsSaleQuotationOrderID(strOperatorCode);
            LB_QOID.Text = strQOID;

            string strNewQOCode = ShareClass.GetCodeByRule("QuotationOrderCode", "QuotationOrderCode", strQOID);
            if (strNewQOCode != "")
            {
                TB_QOName.Text = strNewQOCode;
                string strHQL = "Update T_GoodsSaleQuotationOrder Set QOName = " + "'" + strNewQOCode + "'" + " Where QOID = " + strQOID;
                ShareClass.RunSqlCommand(strHQL);
            }

            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, LanguageHandle.GetWord("WuLiaoBaoJiaChan").ToString().Trim(), strQOID);


            NB_Amount.Amount = 0;


            TB_WLName.Text = LanguageHandle.GetWord("BaoJia").ToString().Trim() + strQOName + LanguageHandle.GetWord("ShenQing").ToString().Trim();

            BT_SubmitApply.Enabled = true;

            LoadGoodsSaleQuotationOrder(strSalesCode);
            LoadGoodsSaleQuotationOrderDetail(strQOID);

            LoadRelatedWL("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), goodsSaleQuotationOrder.QOID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCKNBJMCZD50GHZHBZZSZD100GHZGDJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strQOID, strUserCode, strCustomerCode, strCustomerName, strQOName, strSalesCode, strSalesName, strCurrencyType, strComment;
        DateTime dtQuotationTime, dtValidityPeriod;
        decimal deAmount;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();

        strQOID = LB_QOID.Text.Trim();
        strQOName = TB_QOName.Text.Trim();

        strCustomerCode = DL_Customer.SelectedValue.Trim();
        strCustomerName = DL_Customer.SelectedItem.Text;

        strSalesCode = TB_SalesCode.Text.Trim();
        strSalesName = LB_SalesName.Text.Trim();
        dtQuotationTime = DateTime.Parse(DLC_QuotationTime.Text);
        dtValidityPeriod = DateTime.Parse(DLC_ValidityPeriod.Text);

        deAmount = NB_Amount.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();
        strStatus = DL_QOStatus.SelectedValue.Trim();

        strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.QOID = " + strQOID;
        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        GoodsSaleQuotationOrder goodsSaleQuotationOrder = (GoodsSaleQuotationOrder)lst[0];

        goodsSaleQuotationOrder.QOName = strQOName;

        goodsSaleQuotationOrder.CustomerCode = strCustomerCode;
        goodsSaleQuotationOrder.CustomerName = strCustomerName;

        goodsSaleQuotationOrder.SalesCode = strSalesCode;
        try
        {
            goodsSaleQuotationOrder.SalesName = ShareClass.GetUserName(strSalesCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWBJRDMBZCWCRJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        goodsSaleQuotationOrder.QuotationTime = dtQuotationTime;
        goodsSaleQuotationOrder.ValidityPeriod = dtValidityPeriod;

        goodsSaleQuotationOrder.Amount = deAmount;
        goodsSaleQuotationOrder.CurrencyType = strCurrencyType;
        goodsSaleQuotationOrder.Comment = strComment;
        goodsSaleQuotationOrder.Status = strStatus;

        goodsSaleQuotationOrder.RelatedType = DL_RelatedType.SelectedValue.Trim();
        goodsSaleQuotationOrder.RelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        try
        {
            goodsSaleQuotationOrderBLL.UpdateGoodsSaleQuotationOrder(goodsSaleQuotationOrder, int.Parse(strQOID));
            LoadGoodsSaleQuotationOrder(strUserCode);

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select QOID as DetailQOID, * from T_goodsSaleQuotationOrder where QOID = " + strQOID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        }
    }


    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strGoodsCode, strGoodsName, strSpec, strModelNumber;


        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strSpec = "%" + strSpec + "%";
        strModelNumber = "%" + strModelNumber + "%";

        strHQL = "Select GoodsCode,GoodsName,Type,ModelNumber,Spec,Manufacturer,UnitName,Price, COALESCE(Sum(Number),0) as TotalNumber From T_Goods";
        strHQL += " Where GoodsCode Like " + "'" + strGoodsCode + "'";
        strHQL += " and GoodsName Like " + "'" + strGoodsName + "'";
        strHQL += " and ModelNumber Like " + "'" + strModelNumber + "'";
        strHQL += " and Spec Like " + "'" + strSpec + "'";
        strHQL += " Group By  GoodsCode,GoodsName,Type,ModelNumber,Spec,Manufacturer,UnitName,Price";
        strHQL += " Order By COALESCE(Sum(Number),0) Desc";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        LB_Sql3.Text = strHQL;

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType Like 'Goods'";

        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid9.DataSource = ds;
        DataGrid9.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";

        NB_Number.Amount = 0;
        NB_Price.Amount = 0;

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

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strQOID;
            IList lst;

            strQOID = LB_QOID.Text.Trim();

            int intWLNumber = GetRelatedWorkFlowNumber("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
            if (intWLNumber > 0)
            {
                BT_NewDetail.Visible = false;
            }
            else
            {
                BT_NewDetail.Visible = true;
            }

            //WorkFlow,如果此单和工作流相关，那么依工作流状态决定能否保存单据数据
            ShareClass.DetailTableChangeWorkflowRelatedModule(strUserCode, LanguageHandle.GetWord("WuLiaoBaoJiaChan").ToString().Trim(), strQOID, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, BT_CreateMain, BT_NewMain, BT_CreateDetail, BT_NewDetail, strDetailTableCanAdd, strDetailTableCanEdit);

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID, "0");
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

            if (e.CommandName == "Delete")
            {
                intWLNumber = GetRelatedWorkFlowNumber("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    return;
                }

                //Workflow,如果存在关联工作流，那么要执行下面的代码
                string strCreateUserCode;
                strCreateUserCode = GetGoodsSaleQuotationOrderCreatorCode(strQOID);
                if (!ShareClass.DetailTableDeleteWorkflowRelatedModule(strUserCode, strCreateUserCode, strRelatedWorkflowID, strRelatedWorkflowStepID, strRelatedWorkflowStepDetailID, strDetailTableCanDelete))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZSBNWQSCQJC").ToString().Trim() + "')", true);
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    return;
                }



                GoodsSaleQuotationOrderDetailBLL goodsSaleQuotationOrderDetailBLL = new GoodsSaleQuotationOrderDetailBLL();
                strHQL = "from GoodsSaleQuotationOrderDetail as goodsSaleQuotationOrderDetail where goodsSaleQuotationOrderDetail.ID = " + strID;
                lst = goodsSaleQuotationOrderDetailBLL.GetAllGoodsSaleQuotationOrderDetails(strHQL);
                GoodsSaleQuotationOrderDetail goodsSaleQuotationOrderDetail = (GoodsSaleQuotationOrderDetail)lst[0];

                try
                {
                    goodsSaleQuotationOrderDetailBLL.DeleteGoodsSaleQuotationOrderDetail(goodsSaleQuotationOrderDetail);

                    LoadGoodsSaleQuotationOrderDetail(strQOID);

                    NB_Amount.Amount = SumGoodsSaleQuotationOrderAmount(strQOID);
                    UpdateGoodsSaleQuotationOrderAmount(strQOID, NB_Amount.Amount);

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText;

                        strCmdText = "select QOID as DetailQOID, * from T_goodsSaleQuotationOrder where QOID = " + strQOID;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
                        }

                        if (strToDoWLID != null)
                        {
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }

                        if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                        {
                            strCmdText = "select * from T_GoodsSaleQuotationOrderDetail where QOID = " + strQOID;
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

    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strQOID;

        strQOID = LB_QOID.Text.Trim();

        if (strQOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strQOID = LB_QOID.Text.Trim();
        int intWLNumber = GetRelatedWorkFlowNumber("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
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
        string strRecordID, strQOID, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strUnitName;
        decimal decNumber;
        DateTime dtBuyTime;
        decimal dePrice;

        strQOID = LB_QOID.Text.Trim();

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        decNumber = NB_Number.Amount;
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        dePrice = NB_Price.Amount;

        dtBuyTime = DateTime.Now;


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
        else
        {
            GoodsSaleQuotationOrderDetailBLL goodsSaleQuotationOrderDetailBLL = new GoodsSaleQuotationOrderDetailBLL();
            GoodsSaleQuotationOrderDetail goodsSaleQuotationOrderDetail = new GoodsSaleQuotationOrderDetail();

            goodsSaleQuotationOrderDetail.QOID = int.Parse(strQOID);
            goodsSaleQuotationOrderDetail.Type = strType;
            goodsSaleQuotationOrderDetail.GoodsCode = strGoodsCode;
            goodsSaleQuotationOrderDetail.GoodsName = strGoodsName;
            goodsSaleQuotationOrderDetail.Number = decNumber;
            goodsSaleQuotationOrderDetail.Unit = strUnitName;
            goodsSaleQuotationOrderDetail.Number = decNumber;
            goodsSaleQuotationOrderDetail.Price = dePrice;
            goodsSaleQuotationOrderDetail.Amount = dePrice * decNumber;
            goodsSaleQuotationOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            goodsSaleQuotationOrderDetail.ModelNumber = strModelNumber;
            goodsSaleQuotationOrderDetail.Spec = strSpec;
            goodsSaleQuotationOrderDetail.Brand = TB_Brand.Text;

            try
            {
                goodsSaleQuotationOrderDetailBLL.AddGoodsSaleQuotationOrderDetail(goodsSaleQuotationOrderDetail);

                strRecordID = ShareClass.GetMyCreatedMaxGoodsSaleQuotationOrderDetailID(strQOID);
                LB_ID.Text = strRecordID;

                LoadGoodsSaleQuotationOrderDetail(strQOID);

                NB_Amount.Amount = SumGoodsSaleQuotationOrderAmount(strQOID);
                UpdateGoodsSaleQuotationOrderAmount(strQOID, NB_Amount.Amount);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select QOID as DetailQOID, * from T_goodsSaleQuotationOrder where QOID = " + strQOID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsSaleQuotationOrderDetail where QOID = " + strQOID;
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
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;
        DateTime dtBuyTime;
        decimal dePrice, deNumber;


        string strID, strQOID, strSalesCode, strSalesName;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();

        strQOID = LB_QOID.Text.Trim();

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        dePrice = NB_Price.Amount;
        deNumber = NB_Number.Amount;

        dtBuyTime = DateTime.Now;

        strSalesCode = TB_SalesCode.Text.Trim();
        strSalesName = LB_SalesName.Text.Trim();


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
        else
        {
            GoodsSaleQuotationOrderDetailBLL goodsSaleQuotationOrderDetailBLL = new GoodsSaleQuotationOrderDetailBLL();
            strHQL = "from GoodsSaleQuotationOrderDetail as goodsSaleQuotationOrderDetail where goodsSaleQuotationOrderDetail.ID = " + strID;
            lst = goodsSaleQuotationOrderDetailBLL.GetAllGoodsSaleQuotationOrderDetails(strHQL);
            GoodsSaleQuotationOrderDetail goodsSaleQuotationOrderDetail = (GoodsSaleQuotationOrderDetail)lst[0];

            goodsSaleQuotationOrderDetail.QOID = int.Parse(strQOID);
            goodsSaleQuotationOrderDetail.Type = strType;
            goodsSaleQuotationOrderDetail.GoodsCode = strGoodsCode;
            goodsSaleQuotationOrderDetail.GoodsName = strGoodsName;
            goodsSaleQuotationOrderDetail.Number = deNumber;
            goodsSaleQuotationOrderDetail.Unit = strUnitName;
            goodsSaleQuotationOrderDetail.Price = dePrice;
            goodsSaleQuotationOrderDetail.Amount = dePrice * deNumber;
            goodsSaleQuotationOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            goodsSaleQuotationOrderDetail.ModelNumber = strModelNumber;
            goodsSaleQuotationOrderDetail.Spec = strSpec;

            goodsSaleQuotationOrderDetail.Brand = TB_Brand.Text;

            try
            {
                goodsSaleQuotationOrderDetailBLL.UpdateGoodsSaleQuotationOrderDetail(goodsSaleQuotationOrderDetail, int.Parse(strID));

                LoadGoodsSaleQuotationOrderDetail(strQOID);

                NB_Amount.Amount = SumGoodsSaleQuotationOrderAmount(strQOID);
                UpdateGoodsSaleQuotationOrderAmount(strQOID, NB_Amount.Amount);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select QOID as DetailQOID, * from T_goodsSaleQuotationOrder where QOID = " + strQOID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialQuotation", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strQOID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsSaleQuotationOrderDetail where QOID = " + strQOID;
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
        string strCmdText, strQOID;
        string strWLID, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strQOID = LB_QOID.Text.Trim();

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
        workFlow.RelatedID = int.Parse(strQOID);
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

            LoadRelatedWL(strWLType, LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strQOID));

            UpdateGoodsGoodsSaleStatus(strQOID, "InProgress");
            DL_QOStatus.SelectedValue = "InProgress";

            strCmdText = "select QOID as DetailQOID, * from T_GoodsSaleQuotationOrder where QOID = " + strQOID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsSaleQuotationOrder", strXMLFile2);

            //Workflow,添加模组关联流程记录
            ShareClass.AddModuleToRelatedWorkflow(strWLID, "0", "0", LanguageHandle.GetWord("WuLiaoBaoJiaChan").ToString().Trim(), strQOID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPBJSSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPBJSSBKNGZLMCGCZD25GHZJC").ToString().Trim() + "')", true);
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

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'MaterialQuotation'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
    }

    protected void DL_QOStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strQOID, strStatus, strUserCode;

        strQOID = LB_QOID.Text.Trim();
        strStatus = DL_QOStatus.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strQOID != "")
        {
            UpdateGoodsGoodsSaleStatus(strQOID, strStatus);
            LoadGoodsSaleQuotationOrder(strUserCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleQuotationOrderDetail");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        IList lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void UpdateGoodsGoodsSaleStatus(string strQOID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.QOID = " + strQOID;
        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        GoodsSaleQuotationOrder goodsSaleQuotationOrder = (GoodsSaleQuotationOrder)lst[0];

        goodsSaleQuotationOrder.Status = strStatus;

        try
        {
            goodsSaleQuotationOrderBLL.UpdateGoodsSaleQuotationOrder(goodsSaleQuotationOrder, int.Parse(strQOID));
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

    protected void LoadGoodsSaleQuotationOrder(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        //Workflow,对流程相关模组作判断
        if (strRelatedWorkflowID == null)
        {
            strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.OperatorCode = " + "'" + strOperatorCode + "'";
        }
        else
        {
            strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where ";
            strHQL += "goodsSaleQuotationOrder.QOID in (Select workFlowRelatedModule.RelatedID  From WorkFlowRelatedModule as workFlowRelatedModule Where workFlowRelatedModule.RelatedModuleName = 'MaterialQuotation' and workFlowRelatedModule.WorkflowID =" + strRelatedWorkflowID + ")";   //ChineseWord
        }
        strHQL += " Order by goodsSaleQuotationOrder.QOID DESC";

        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.QOID = " + strWLBusinessID;
        }

        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    //Workflow,取得单据创建人代码
    protected string GetGoodsSaleQuotationOrderCreatorCode(string strQOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.QOID = " + strQOID;
        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        return ((GoodsSaleQuotationOrder)lst[0]).OperatorCode.Trim();
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

    protected void LoadGoodsSaleQuotationOrderDetail(string strQOID)
    {
        string strHQL = "Select * from T_GoodsSaleQuotationOrderDetail where QOID = " + strQOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleQuotationOrderDetail");

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


    protected decimal SumGoodsSaleQuotationOrderAmount(string strQOID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from GoodsSaleQuotationOrderDetail as goodsSaleQuotationOrderDetail where goodsSaleQuotationOrderDetail.QOID = " + strQOID;
        GoodsSaleQuotationOrderDetailBLL goodsSaleQuotationOrderDetailBLL = new GoodsSaleQuotationOrderDetailBLL();
        lst = goodsSaleQuotationOrderDetailBLL.GetAllGoodsSaleQuotationOrderDetails(strHQL);

        GoodsSaleQuotationOrderDetail goodsSaleQuotationOrderDetail = new GoodsSaleQuotationOrderDetail();

        for (int i = 0; i < lst.Count; i++)
        {
            goodsSaleQuotationOrderDetail = (GoodsSaleQuotationOrderDetail)lst[i];
            deAmount += goodsSaleQuotationOrderDetail.Number * goodsSaleQuotationOrderDetail.Price;
        }

        return deAmount;
    }

    protected void UpdateGoodsSaleQuotationOrderAmount(string strQOID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsSaleQuotationOrder as goodsSaleQuotationOrder where goodsSaleQuotationOrder.QOID = " + strQOID;
        GoodsSaleQuotationOrderBLL goodsSaleQuotationOrderBLL = new GoodsSaleQuotationOrderBLL();
        lst = goodsSaleQuotationOrderBLL.GetAllGoodsSaleQuotationOrders(strHQL);

        GoodsSaleQuotationOrder goodsSaleQuotationOrder = (GoodsSaleQuotationOrder)lst[0];

        goodsSaleQuotationOrder.Amount = deAmount;

        try
        {
            goodsSaleQuotationOrderBLL.UpdateGoodsSaleQuotationOrder(goodsSaleQuotationOrder, int.Parse(strQOID));
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

