using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGoodsDeliveryOrder : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产登记入库", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_DeliveryTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ArrivalTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            ShareClass.InitialInvolvedProjectTree(TreeView2, strUserCode);

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            ShareClass.LoadCurrencyType(DL_CurrencyType);

            LoadGoodsDeliveryOrder(strUserCode);
            LoadGoodsShipmentOrder(strUserCode);
            LoadGoodsSaleOrder(strUserCode);
        }
    }

    protected void DL_SourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strRelatedType;

        strRelatedType = DL_SourceType.SelectedValue.Trim();

        if (strRelatedType == "Othter")
        {
            NB_SourceID.Amount = 0;
        }

        if (strRelatedType == "GoodsSHO")
        {
            BT_SelectSHO.Visible = true;
            LoadGoodsShipmentOrder(strUserCode);
        }
        else
        {
            BT_SelectSHO.Visible = false;
        }

        if (strRelatedType == "GoodsSO")
        {
            BT_SelectSO.Visible = true;
            LoadGoodsSaleOrder(strUserCode);
        }
        else
        {
            BT_SelectSO.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strSHOID;

        if (e.CommandName != "Page")
        {
            TabContainer1.ActiveTabIndex = 2;

            strSHOID = ((Button)e.Item.FindControl("BT_ShipmentNO")).Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_SHOID.Text = strSHOID;

            NB_SourceID.Amount = int.Parse(strSHOID);

            LoadGoodsShipmentOrderDetail(strSHOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strSOID;

        if (e.CommandName != "Page")
        {
            TabContainer1.ActiveTabIndex = 3;

            strSOID = ((Button)e.Item.FindControl("BT_SOID")).Text.Trim();

            for (int i = 0; i < DataGrid6.Items.Count; i++)
            {
                DataGrid6.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_SOID.Text = strSOID;
            NB_SourceID.Amount = int.Parse(strSOID);
            NB_RelatedID.Amount = int.Parse(strSOID);

            LoadGoodsSaleOrderDetail(strSOID); ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;

        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        IList lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strDOID;

            strDOID = e.Item.Cells[2].Text.Trim();
            LB_DOID.Text = strDOID;

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid5.Items.Count; i++)
                {
                    DataGrid5.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From GoodsDeliveryOrder as goodsDeliveryOrder Where goodsDeliveryOrder.DOID = " + strDOID;
                GoodsDeliveryOrderBLL goodsDeliveryOrderBLL = new GoodsDeliveryOrderBLL();
                lst = goodsDeliveryOrderBLL.GetAllGoodsDeliveryOrders(strHQL);

                GoodsDeliveryOrder goodsDeliveryOrder = (GoodsDeliveryOrder)lst[0];

                LB_DOID.Text = strDOID;
                TB_DOName.Text = goodsDeliveryOrder.DOName.Trim();
                DLC_DeliveryTime.Text = goodsDeliveryOrder.DeliveryTime.ToString("yyyy-MM-dd");
                DLC_ArrivalTime.Text = goodsDeliveryOrder.ArrivalTime.ToString("yyyy-MM-dd");

                DL_CurrencyType.SelectedValue = goodsDeliveryOrder.CurrencyType;
                NB_TotalAmount.Amount = goodsDeliveryOrder.Amount;
                TB_InvoiceHead.Text = goodsDeliveryOrder.InvoiceHead.Trim();
                TB_ReceiverName.Text = goodsDeliveryOrder.ReceiverName.Trim();

                TB_PurchaseName.Text = goodsDeliveryOrder.PurchaseName.Trim();
                TB_PurchasePhone.Text = goodsDeliveryOrder.PurchasePhone.Trim();

                TB_Driver.Text = goodsDeliveryOrder.Driver.Trim();
                TB_CarCode.Text = goodsDeliveryOrder.CarCode.Trim();
                TB_CarTeam.Text = goodsDeliveryOrder.CarTeam.Trim();

                DL_RelatedType.SelectedValue = goodsDeliveryOrder.RelatedType.Trim();
                NB_RelatedID.Amount = goodsDeliveryOrder.RelatedID;

                DL_DOStatus.SelectedValue = goodsDeliveryOrder.Status.Trim();

                TB_Comment.Text = goodsDeliveryOrder.Comment.Trim();

                LoadGoodsDeliveryOrderDetail(strDOID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }

            if (e.CommandName == "Delete")
            {
                try
                {
                    if (DataGrid1.Items.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZMXJLSCSBQJC") + "')", true);
                        return;
                    }

                    strHQL = "Delete From T_GoodsDeliveryOrder Where DOID = " + strDOID;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_GoodsDeliveryOrderDetail Where DOID = " + strDOID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadGoodsDeliveryOrder(strUserCode);
                    LoadGoodsDeliveryOrderDetail(strDOID);


                    LB_DOID.Text = "";
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCKNCZMXJLJC") + "')", true);
                }
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql2.Text;

        GoodsBLL goodsBLL = new GoodsBLL();
        IList lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        GoodsShipmentOrderBLL goodsShipmentOrderBLL = new GoodsShipmentOrderBLL();
        IList lst = goodsShipmentOrderBLL.GetAllGoodsShipmentOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
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

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_DOID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        string strNewDOCode = ShareClass.GetCodeByRule("DeliveryOrderCode", "DeliveryOrderCode", "00");
        if (strNewDOCode != "")
        {
            TB_DOName.Text = strNewDOCode;
        }


        LoadGoodsDeliveryOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strDOID;

        strDOID = LB_DOID.Text.Trim();

        if (strDOID == "")
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
        string strDOID, strDOName, strPurchaseName, strPurchasePHone, strReceiverName, strCarCode, strCarTeam, strDriver, strComment, strRelatedType, strStatus;
        DateTime dtDeliveryTime, dtArrivalTime;
        int intRelatedID;


        strDOName = TB_DOName.Text.Trim();
        strReceiverName = TB_ReceiverName.Text.Trim();

        dtDeliveryTime = DateTime.Parse(DLC_DeliveryTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);

        strCarCode = TB_CarCode.Text.Trim();
        strCarTeam = TB_CarTeam.Text.Trim();
        strDriver = TB_Driver.Text.Trim();

        strPurchaseName = TB_PurchaseName.Text.Trim();
        strPurchasePHone = TB_PurchasePhone.Text.Trim();

        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        intRelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        strStatus = DL_DOStatus.SelectedValue.Trim();

        strComment = TB_Comment.Text.Trim();

        try
        {
            GoodsDeliveryOrderBLL goodsDeliveryOrderBLL = new GoodsDeliveryOrderBLL();
            GoodsDeliveryOrder goodsDeliveryOrder = new GoodsDeliveryOrder();

            goodsDeliveryOrder.DOName = strDOName;
            goodsDeliveryOrder.ArrivalTime = dtArrivalTime;
            goodsDeliveryOrder.DeliveryTime = dtDeliveryTime;
            goodsDeliveryOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            goodsDeliveryOrder.Amount = NB_TotalAmount.Amount;
            goodsDeliveryOrder.InvoiceHead = TB_InvoiceHead.Text.Trim();

            goodsDeliveryOrder.PurchaseName = strPurchaseName;
            goodsDeliveryOrder.PurchasePhone = strPurchasePHone;
            goodsDeliveryOrder.ReceiverName = TB_ReceiverName.Text.Trim();

            goodsDeliveryOrder.CarTeam = strCarTeam;
            goodsDeliveryOrder.CarCode = strCarCode;
            goodsDeliveryOrder.Driver = strDriver;

            goodsDeliveryOrder.RelatedType = strRelatedType;
            goodsDeliveryOrder.RelatedID = intRelatedID;

            goodsDeliveryOrder.OperatorCode = strUserCode;
            goodsDeliveryOrder.OperatorName = ShareClass.GetUserName(strUserCode);
            goodsDeliveryOrder.Status = strStatus;

            goodsDeliveryOrder.Comment = strComment;

            goodsDeliveryOrderBLL.AddGoodsDeliveryOrder(goodsDeliveryOrder);

            strDOID = ShareClass.GetMyCreatedMaxGoodsDeliveryOrderID(strUserCode).ToString();

            LB_DOID.Text = strDOID;

            string strNewDOCode = ShareClass.GetCodeByRule("DeliveryOrderCode", "DeliveryOrderCode", strDOID);
            if (strNewDOCode != "")
            {
                TB_DOName.Text = strNewDOCode;
                string strHQL = "Update T_GoodsDeliveryOrder Set DOName = " + "'" + strNewDOCode + "'" + " Where DOID = " + strDOID;
                ShareClass.RunSqlCommand(strHQL);
            }


            LoadGoodsDeliveryOrder(strUserCode);
            LoadGoodsDeliveryOrderDetail(strDOID);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strDOID, strDOName, strPurchaseName, strPurchasePHone, strReceiverName, strCarCode, strCarTeam, strDriver, strComment, strRelatedType, strStatus;
        DateTime dtDeliveryTime, dtArrivalTime;
        int intRelatedID;

        strDOID = LB_DOID.Text.Trim();
        strDOName = TB_DOName.Text.Trim();
        strReceiverName = TB_ReceiverName.Text.Trim();

        dtDeliveryTime = DateTime.Parse(DLC_DeliveryTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);

        strCarCode = TB_CarCode.Text.Trim();
        strCarTeam = TB_CarTeam.Text.Trim();
        strDriver = TB_Driver.Text.Trim();

        strPurchaseName = TB_PurchaseName.Text.Trim();
        strPurchasePHone = TB_PurchasePhone.Text.Trim();

        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        intRelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        strStatus = DL_DOStatus.SelectedValue.Trim();

        strComment = TB_Comment.Text.Trim();
        try
        {
            strHQL = "From GoodsDeliveryOrder as goodsDeliveryOrder Where goodsDeliveryOrder.DOID = " + strDOID;
            GoodsDeliveryOrderBLL goodsDeliveryOrderBLL = new GoodsDeliveryOrderBLL();
            lst = goodsDeliveryOrderBLL.GetAllGoodsDeliveryOrders(strHQL);
            GoodsDeliveryOrder goodsDeliveryOrder = (GoodsDeliveryOrder)lst[0];

            goodsDeliveryOrder.DOName = strDOName;
            goodsDeliveryOrder.ArrivalTime = dtArrivalTime;
            goodsDeliveryOrder.DeliveryTime = dtDeliveryTime;

            goodsDeliveryOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            goodsDeliveryOrder.Amount = NB_TotalAmount.Amount;

            goodsDeliveryOrder.PurchaseName = strPurchaseName;
            goodsDeliveryOrder.PurchasePhone = strPurchasePHone;
            goodsDeliveryOrder.ReceiverName = TB_ReceiverName.Text.Trim();

            goodsDeliveryOrder.CarTeam = strCarTeam;
            goodsDeliveryOrder.CarCode = strCarCode;
            goodsDeliveryOrder.Driver = strDriver;

            goodsDeliveryOrder.RelatedType = strRelatedType;
            goodsDeliveryOrder.RelatedID = intRelatedID;

            goodsDeliveryOrder.OperatorCode = strUserCode;
            goodsDeliveryOrder.OperatorName = ShareClass.GetUserName(strUserCode);
            goodsDeliveryOrder.Status = strStatus;

            goodsDeliveryOrder.Comment = strComment;

            goodsDeliveryOrderBLL.UpdateGoodsDeliveryOrder(goodsDeliveryOrder, int.Parse(strDOID));

            LoadGoodsDeliveryOrder(strUserCode);
            LoadGoodsDeliveryOrderDetail(strDOID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DL_RecordSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_RecordSourceID.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strDepartString;

        TabContainer1.ActiveTabIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = LB_DepartString.Text.Trim();


        strHQL = "Select * From T_Goods  Where GoodsCode Like " + "'" + strGoodsCode + "'" + " and GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and ModelNumber Like " + "'" + strModelNumber + "'" + " and Spec Like " + "'" + strSpec + "'";
        strHQL += " and Number > 0";
        strHQL += " and Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
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

                DL_Unit.SelectedValue = item.Unit;
                TB_ModelNumber.Text = item.ModelNumber.Trim();
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

                NB_Price.Amount = item.SalePrice;

                if (LB_SourceRelatedID.Text.Trim() == "0")
                {
                    DL_RecordSourceType.SelectedValue = "Other";
                    NB_RecordSourceID.Amount = 0;
                    NB_Price.Amount = item.SalePrice;
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = e.Item.Cells[0].Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
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


            NB_Price.Amount = goods.Price;

            if (LB_SourceRelatedID.Text.Trim() == "0")
            {
                DL_RecordSourceType.SelectedValue = "Other";
                NB_RecordSourceID.Amount = 0;
                NB_Price.Amount = goods.Price;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsShipmentDetail as goodsShipmentDetail where goodsShipmentDetail.ID = " + strID;
            GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
            lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);

            if (lst.Count > 0)
            {
                try
                {
                    GoodsShipmentDetail goodsShipmentDetail = (GoodsShipmentDetail)lst[0];

                    TB_GoodsCode.Text = goodsShipmentDetail.GoodsCode;
                    TB_GoodsName.Text = goodsShipmentDetail.GoodsName.Trim();
                    TB_ModelNumber.Text = goodsShipmentDetail.ModelNumber.Trim();
                    TB_Spec.Text = goodsShipmentDetail.Spec.Trim();
                    TB_Brand.Text = goodsShipmentDetail.Manufacturer;

                    NB_Number.Amount = goodsShipmentDetail.Number - goodsShipmentDetail.DeliveryNumber;
                    NB_Price.Amount = goodsShipmentDetail.Price;
                    DL_Unit.SelectedValue = goodsShipmentDetail.UnitName;

                    LB_SourceRelatedID.Text = goodsShipmentDetail.ShipmentNO.ToString();
                    DL_RecordSourceType.SelectedValue = "GoodsSHORecord";
                    NB_RecordSourceID.Amount = goodsShipmentDetail.ID;
                }
                catch
                {

                }
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

            string strID;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid7.Items.Count; i++)
            {
                DataGrid7.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsSaleRecord as goodsSaleRecord where goodsSaleRecord.ID = " + strID;
            GoodsSaleRecordBLL goodsSaleRecordBLL = new GoodsSaleRecordBLL();
            lst = goodsSaleRecordBLL.GetAllGoodsSaleRecords(strHQL);

            if (lst.Count > 0)
            {
                try
                {
                    GoodsSaleRecord goodsSaleRecord = (GoodsSaleRecord)lst[0];

                    TB_GoodsCode.Text = goodsSaleRecord.GoodsCode;
                    TB_GoodsName.Text = goodsSaleRecord.GoodsName.Trim();
                    TB_ModelNumber.Text = goodsSaleRecord.ModelNumber.Trim();
                    TB_Spec.Text = goodsSaleRecord.Spec.Trim();
                    TB_Brand.Text = goodsSaleRecord.Brand;

                    NB_Number.Amount = goodsSaleRecord.Number - goodsSaleRecord.DeliveryNumber;
                    DL_Unit.SelectedValue = goodsSaleRecord.Unit;
                    NB_Price.Amount = goodsSaleRecord.Price;

                    LB_SourceRelatedID.Text = goodsSaleRecord.SOID.ToString();
                    DL_RecordSourceType.SelectedValue = "GoodsSORecord";
                    NB_RecordSourceID.Amount = goodsSaleRecord.ID;
                }
                catch
                {

                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID = e.Item.Cells[2].Text.Trim();
            LB_ID.Text = strID;

            if (e.CommandName == "Update")
            {

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From GoodsDeliveryOrderDetail as goodsDeliveryOrderDetail Where goodsDeliveryOrderDetail.ID = " + strID;
                GoodsDeliveryOrderDetailBLL goodsDeliveryOrderDetailBLL = new GoodsDeliveryOrderDetailBLL();
                lst = goodsDeliveryOrderDetailBLL.GetAllGoodsDeliveryOrderDetails(strHQL);

                GoodsDeliveryOrderDetail goodsDeliveryOrderDetail = (GoodsDeliveryOrderDetail)lst[0];

                LB_ID.Text = strID;
                TB_GoodsCode.Text = goodsDeliveryOrderDetail.GoodsCode.Trim();
                TB_GoodsName.Text = goodsDeliveryOrderDetail.GoodsName.Trim();
                TB_ModelNumber.Text = goodsDeliveryOrderDetail.ModelNumber.Trim();
                TB_Spec.Text = goodsDeliveryOrderDetail.Spec.Trim();
                TB_Brand.Text = goodsDeliveryOrderDetail.Brand;

                NB_Number.Amount = goodsDeliveryOrderDetail.Number;
                NB_Price.Amount = goodsDeliveryOrderDetail.Price;
                DL_Unit.SelectedValue = goodsDeliveryOrderDetail.Unit;

                LB_SourceRelatedID.Text = goodsDeliveryOrderDetail.RelatedID.ToString();
                DL_RecordSourceType.SelectedValue = goodsDeliveryOrderDetail.SourceType.Trim();
                NB_RecordSourceID.Amount = goodsDeliveryOrderDetail.SourceID;

                NB_RealReceiveNumber.Amount = goodsDeliveryOrderDetail.RealReceiveNumber;


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strDOID, strGoodsCode, strWareHouse;
                string strSourceType, strSourceID;
                decimal deOldNumber;

                strGoodsCode = TB_GoodsCode.Text.Trim();

                deOldNumber = NB_Number.Amount;

                strSourceType = DL_RecordSourceType.SelectedValue.Trim();
                strSourceID = NB_RecordSourceID.Amount.ToString();


                strDOID = LB_DOID.Text.Trim();

                strID = LB_ID.Text.Trim();
                strHQL = "Delete From T_GoodsDeliveryOrderDetail Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    //更新总金额
                    CountSumAmount(strDOID);

                    //更新出库单和销售单的已发货数据
                    UpdateGoodsSOOrSHONumber(strSourceType, strSourceID);

                    //删除应收应付数据到应收应付表
                    string strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
                    if (strSourceType == "GoodsSORecord")
                    {
                        ShareClass.DeleteReceivablesOrPayable("GoodsDO", "GoodsSO", strSourceID);
                    }
                    if (strSourceType == "GoodsSHORecord")
                    {
                        ShareClass.DeleteReceivablesOrPayable("GoodsDO", "GoodsSHO", strSourceID);
                    }

                    if (strSourceType == "GoodsSHORecord")
                    {
                        LoadGoodsShipmentOrderDetail(LB_SHOID.Text.Trim());
                    }
                    if (strSourceType == "GoodsSORecord")
                    {
                        LoadGoodsSaleOrderDetail(LB_SOID.Text.Trim());
                    }

                    LoadGoodsDeliveryOrderDetail(strDOID);



                    LB_ID.Text = "0";

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
        string strDOID;

        strDOID = LB_DOID.Text.Trim();

        if (strDOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strDOID = LB_DOID.Text.Trim();


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
        string strDOID, strGoodsCode, strGoodsName, strSN, strModelNumber, strSpec, strUnitName;
        decimal deNumber, deRealReceiveNumber, dePrice;
        string strSourceType, strSourceID, strSourceRelatedID;


        strDOID = LB_DOID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strSN = TB_SN.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        deRealReceiveNumber = NB_RealReceiveNumber.Amount;
        dePrice = NB_Price.Amount;

        strUnitName = DL_Unit.SelectedValue.Trim();

        strSourceRelatedID = LB_SourceRelatedID.Text.Trim();
        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strSourceID = NB_RecordSourceID.Amount.ToString();


        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);

        }
        else
        {
            GoodsDeliveryOrderDetailBLL goodsDeliveryOrderDetailBLL = new GoodsDeliveryOrderDetailBLL();
            GoodsDeliveryOrderDetail goodsDeliveryOrderDetail = new GoodsDeliveryOrderDetail();

            goodsDeliveryOrderDetail.DOID = int.Parse(strDOID);
            goodsDeliveryOrderDetail.GoodsCode = strGoodsCode;
            goodsDeliveryOrderDetail.GoodsName = strGoodsName;
            goodsDeliveryOrderDetail.SN = strSN;
            goodsDeliveryOrderDetail.ModelNumber = strModelNumber;
            goodsDeliveryOrderDetail.Spec = strSpec;
            goodsDeliveryOrderDetail.Brand = TB_Brand.Text;

            goodsDeliveryOrderDetail.Number = deNumber;
            goodsDeliveryOrderDetail.RealReceiveNumber = deRealReceiveNumber;
            goodsDeliveryOrderDetail.Unit = strUnitName;
            goodsDeliveryOrderDetail.Price = dePrice;
            goodsDeliveryOrderDetail.Amount = dePrice * deNumber;
            goodsDeliveryOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            if (strSourceRelatedID == "")
            {
                goodsDeliveryOrderDetail.RelatedID = 0;
            }
            else
            {
                goodsDeliveryOrderDetail.RelatedID = int.Parse(strSourceRelatedID);
            }

            if (strSourceType == "")
            {
                goodsDeliveryOrderDetail.SourceType = "Other";
            }
            else
            {
                goodsDeliveryOrderDetail.SourceType = strSourceType;
            }
            if (strSourceID == "")
            {
                goodsDeliveryOrderDetail.SourceID = 0;
            }
            else
            {
                goodsDeliveryOrderDetail.SourceID = int.Parse(strSourceID);
            }


            try
            {
                goodsDeliveryOrderDetailBLL.AddGoodsDeliveryOrderDetail(goodsDeliveryOrderDetail);

                LB_ID.Text = ShareClass.GetMyCreatedMaxGoodsDeliveryOrderDetailID(strUserCode).ToString();

                //更新总金额
                CountSumAmount(strDOID);

                //更新出库单和销售单的已发货，实收货数据
                UpdateGoodsSOOrSHONumber(strSourceType, strSourceID);

                //插入应收应付数据到应收应付表
                string strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
                if (strSourceType == "GoodsSORecord")
                {
                    string strCustomerName = ShareClass.GetCustomerNameFromGoodsSaleOrder(strSourceRelatedID);

                    //ShareClass.InsertReceivablesOrPayable("GoodsDO", "GoodsSO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strCustomerName, strUserCode);
                }
                if (strSourceType == "GoodsSHORecord")
                {
                    string strReceiverName = TB_ReceiverName.Text.Trim();

                    //ShareClass.InsertReceivablesOrPayable("GoodsDO", "GoodsSHO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strReceiverName, strUserCode);
                }


                if (strSourceType == "GoodsSHORecord")
                {
                    LoadGoodsShipmentOrderDetail(LB_SHOID.Text.Trim());
                }
                if (strSourceType == "GoodsSORecord")
                {
                    LoadGoodsSaleOrderDetail(LB_SOID.Text.Trim());
                }

                LoadGoodsDeliveryOrderDetail(strDOID);



                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);

            }
        }
    }

    protected void UpdateDetail()
    {
        string strID, strDOID, strGoodsCode, strGoodsName, strSN, strModelNumber, strSpec, strUnitName;
        string strSourceType, strSourceID, strSourceRelatedID;
        decimal deNumber, deRealReceiveNumber, dePrice;


        strID = LB_ID.Text.Trim();
        strDOID = LB_DOID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strSN = TB_SN.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        deRealReceiveNumber = NB_RealReceiveNumber.Amount;
        dePrice = NB_Price.Amount;

        strUnitName = DL_Unit.SelectedValue.Trim();

        strSourceRelatedID = LB_SourceRelatedID.Text.Trim();
        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strSourceID = NB_RecordSourceID.Amount.ToString();

        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsDeliveryOrderDetailBLL goodsDeliveryOrderDetailBLL = new GoodsDeliveryOrderDetailBLL();
            GoodsDeliveryOrderDetail goodsDeliveryOrderDetail = new GoodsDeliveryOrderDetail();

            goodsDeliveryOrderDetail.DOID = int.Parse(strDOID);
            goodsDeliveryOrderDetail.GoodsCode = strGoodsCode;
            goodsDeliveryOrderDetail.GoodsName = strGoodsName;
            goodsDeliveryOrderDetail.SN = strSN;
            goodsDeliveryOrderDetail.ModelNumber = strModelNumber;
            goodsDeliveryOrderDetail.Spec = strSpec;
            goodsDeliveryOrderDetail.Brand = TB_Brand.Text;

            goodsDeliveryOrderDetail.Number = deNumber;
            goodsDeliveryOrderDetail.RealReceiveNumber = deRealReceiveNumber;
            goodsDeliveryOrderDetail.Price = dePrice;
            goodsDeliveryOrderDetail.Unit = strUnitName;
            goodsDeliveryOrderDetail.Amount = dePrice * deNumber;
            goodsDeliveryOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            if (strSourceRelatedID == "")
            {
                goodsDeliveryOrderDetail.RelatedID = 0;
            }
            else
            {
                goodsDeliveryOrderDetail.RelatedID = int.Parse(strSourceRelatedID);
            }

            if (strSourceType == "")
            {
                goodsDeliveryOrderDetail.SourceType = "Other";
            }
            else
            {
                goodsDeliveryOrderDetail.SourceType = strSourceType;
            }
            if (strSourceID == "")
            {
                goodsDeliveryOrderDetail.SourceID = 0;
            }
            else
            {
                goodsDeliveryOrderDetail.SourceID = int.Parse(strSourceID);
            }

            try
            {
                goodsDeliveryOrderDetailBLL.UpdateGoodsDeliveryOrderDetail(goodsDeliveryOrderDetail, int.Parse(strID));

                CountSumAmount(strDOID);

                //更新出库单和销售单的已发货数据
                UpdateGoodsSOOrSHONumber(strSourceType, strSourceID);


                //更新应收应付数据到应收应付表
                string strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
                if (strSourceType == "GoodsSORecord")
                {
                    string strCustomerName = ShareClass.GetCustomerNameFromGoodsSaleOrder(strSourceRelatedID);
                    ShareClass.UpdateReceivablesOrPayable("GoodsDO", "GoodsSO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strCustomerName, strUserCode);
                }
                if (strSourceType == "GoodsSHORecord")
                {
                    string strReceiverName = TB_ReceiverName.Text.Trim();
                    ShareClass.UpdateReceivablesOrPayable("GoodsDO", "GoodsSHO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strReceiverName, strUserCode);
                }


                if (strSourceType == "GoodsSHORecord")
                {
                    LoadGoodsShipmentOrderDetail(LB_SHOID.Text.Trim());
                }
                if (strSourceType == "GoodsSORecord")
                {
                    LoadGoodsSaleOrderDetail(LB_SOID.Text.Trim());
                }

                LoadGoodsDeliveryOrderDetail(strDOID);


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);

            }
        }
    }

    protected void UpdateGoodsNumberForAdd(string strFromGoodsID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number - " + deNumber.ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateGoodsNumberForDelete(string strFromGoodsID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number + " + deNumber.ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected bool CheckShipNumber(string strFromGoodsID, decimal deShipNumber)
    {
        string strHQL;
        IList lst;

        decimal deGoodsNumber;

        strHQL = "From Goods as goods where goods.ID = " + strFromGoodsID;
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        Goods goods = (Goods)lst[0];

        deGoodsNumber = goods.Number;

        if (deGoodsNumber < deShipNumber)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void LoadGoodsDeliveryOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsDeliveryOrder as goodsDeliveryOrder Where (goodsDeliveryOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " or goodsDeliveryOrder.OperatorCode in (select  memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ")) ";
        strHQL += " Order By goodsDeliveryOrder.DOID DESC";
        GoodsDeliveryOrderBLL goodsDeliveryOrderBLL = new GoodsDeliveryOrderBLL();
        lst = goodsDeliveryOrderBLL.GetAllGoodsDeliveryOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadGoodsDeliveryOrderDetail(string strDOID)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsDeliveryOrderDetail as goodsDeliveryOrderDetail Where goodsDeliveryOrderDetail.DOID = " + strDOID + " Order By goodsDeliveryOrderDetail.ID ASC";
        GoodsDeliveryOrderDetailBLL goodsDeliveryOrderDetailBLL = new GoodsDeliveryOrderDetailBLL();
        lst = goodsDeliveryOrderDetailBLL.GetAllGoodsDeliveryOrderDetails(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    protected void LoadGoodsShipmentOrderDetail(string strShipmentNO)
    {
        string strHQL;
        IList lst;

        if (strShipmentNO != "")
        {
            strHQL = "From GoodsShipmentDetail as goodsShipmentDetail Where goodsShipmentDetail.ShipmentNO = " + strShipmentNO + " Order By goodsShipmentDetail.ID ASC";
            GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
            lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();
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

    protected void LoadGoods(string strGoodsCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Goods as goods Where goods.Number > 0 and goods.GoodsCode = " + "'" + strGoodsCode + "'";
        strHQL += " Order by goods.ID ASC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        TB_GoodsName.Focus();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadAllGoodsByWareHouse(string strWareHouse)
    {
        string strHQL;
        IList lst;

        strHQL = "From Goods as goods Where goods.Number > 0 and goods.Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by goods.ID ASC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadGoodsShipmentOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from GoodsShipmentOrder as goodsShipmentOrder where goodsShipmentOrder.OperatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order by goodsShipmentOrder.ShipmentNO DESC";
        GoodsShipmentOrderBLL goodsShipmentOrderBLL = new GoodsShipmentOrderBLL();
        lst = goodsShipmentOrderBLL.GetAllGoodsShipmentOrders(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql3.Text = strHQL;
    }

    protected void LoadGoodsSaleOrder(string strOperatorCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and goodsSaleOrder.OperatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and goodsSaleOrder.SalesCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order by goodsSaleOrder.SOID DESC";
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();

        LB_Sql8.Text = strHQL;
    }

    protected void LoadGoodsShipmentDetail(string strShipmentNO)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsShipmentDetail as goodsShipmentDetail where goodsShipmentDetail.ShipmentNO = " + strShipmentNO;
        GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
        lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void LoadGoodsSaleOrderDetail(string strSOID)
    {
        LB_GoodsOwner.Text = LanguageHandle.GetWord("XiaoShouDan") + ":" + strSOID + LanguageHandle.GetWord("MingXi");

        if (strSOID != "")
        {
            string strHQL = "Select * from T_GoodsSaleRecord where SOID = " + strSOID + " Order by ID DESC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

            DataGrid7.DataSource = ds;
            DataGrid7.DataBind();

            LB_Sql7.Text = strHQL;
        }
    }

    protected void UpdateGoodsSOOrSHONumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber, deSumRealReceiveNumber;

        if (strSourceType == "GoodsSORecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0),Sum(RealReceiveNumber) From T_GoodsDeliveryOrderDetail Where SourceType = 'GoodsSORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsDeliveryOrderDetail");
            deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            deSumRealReceiveNumber = decimal.Parse(ds.Tables[0].Rows[0][1].ToString());

            strHQL = "Update T_GoodsSaleRecord Set DeliveryNumber = " + deSumNumber.ToString();
            strHQL += ",RealReceiveNumber = " + deSumRealReceiveNumber;
            strHQL += " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strSourceType == "GoodsSHORecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsDeliveryOrderDetail Where SourceType = 'GoodsSHORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsDeliveryOrderDetail");
            deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

            strHQL = "Update T_GoodsShipmentDetail Set DeliveryNumber = " + deSumNumber.ToString() + " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void CountSumAmount(string strDOID)
    {
        string strHQL;
        decimal deTotalAmount;

        strHQL = "Select COALESCE(Sum(Amount),0) From T_GoodsDeliveryOrderDetail Where DOID=" + strDOID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsDeliveryOrderDetail");

        deTotalAmount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

        strHQL = "Update T_GoodsDeliveryOrder Set Amount = " + deTotalAmount.ToString();
        ShareClass.RunSqlCommand(strHQL);

        NB_TotalAmount.Amount = deTotalAmount;
    }

}
