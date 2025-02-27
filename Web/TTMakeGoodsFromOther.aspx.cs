using System;
using System.Resources;
using System.IO;
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

public partial class TTMakeGoodsFromOther : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strConstractCode, strRelatedGoodsCode = "";
    decimal deReceivablesNumber = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName;

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        if (strRelatedType == null)
        {
            strRelatedType = "Other";
            strRelatedID = "0";
        }

        if (strRelatedType == "Constract")
        {
            strHQL = "from ConstractGoodsReceiptPlan as constractGoodsReceiptPlan where constractGoodsReceiptPlan.ID = " + strRelatedID;
            ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
            lst = constractGoodsReceiptPlanBLL.GetAllConstractGoodsReceiptPlans(strHQL);
            if (lst.Count > 0)
            {
                ConstractGoodsReceiptPlan constractGoodsReceiptPlan = (ConstractGoodsReceiptPlan)lst[0];
                strConstractCode = constractGoodsReceiptPlan.ConstractCode.Trim();
                strRelatedGoodsCode = constractGoodsReceiptPlan.GoodsCode.Trim();
                deReceivablesNumber = constractGoodsReceiptPlan.Number;

                DL_Type.SelectedValue = constractGoodsReceiptPlan.Type;
                TB_GoodsCode.Text = constractGoodsReceiptPlan.GoodsCode;
                TB_GoodsName.Text = constractGoodsReceiptPlan.GoodsName;
                TB_ModelNumber.Text = constractGoodsReceiptPlan.ModelNumber;
                TB_Spec.Text = constractGoodsReceiptPlan.Spec;
                NB_Price.Amount = constractGoodsReceiptPlan.Price;

                DL_RecordSourceType.Enabled = false;
                NB_RecordSourceID.Enabled = false;
                DL_Type.Enabled = false;

                TB_GoodsCode.Enabled = false;
                TB_GoodsName.Enabled = false;
                TB_ModelNumber.Enabled = false;
                TB_Spec.Enabled = false;
                BT_Clear.Enabled = false;
            }
            else
            {
                strRelatedGoodsCode = "";
                deReceivablesNumber = 0;
            }
        }

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            DLC_BuyTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_CheckInTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

            strHQL = "Select * From T_GoodsCheckInType Order By SortNumber ASC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInType");
            DL_CheckInType.DataSource = ds;
            DL_CheckInType.DataBind();
            DL_CheckInType.Items.Insert(0, new ListItem("--Select--", ""));

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

            LoadWareHouseListByAuthority(strUserCode);

            LoadGoodsCheckInOrder(strUserCode, strRelatedType, strRelatedID);
            LoadGoodsPurchaseOrder(strUserCode);

            LB_OwnerCode.Text = strUserCode;
            LB_OwnerName.Text = strUserName;
        }
    }

    protected void BT_FindAll_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strWareHouse, strVendorName;
        string strUserCode = LB_UserCode.Text.Trim();


        strWareHouse = "%" + TB_FindWareHouse.Text.Trim() + "%";
        strVendorName = "%" + TB_FindVendorName.Text.Trim() + "%";

        strHQL = "From GoodsCheckInOrder as goodsCheckInOrder where goodsCheckInOrder.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " and goodsCheckInOrder.RelatedType=" + "'" + strRelatedType + "'" + " and goodsCheckInOrder.RelatedID = " + strRelatedID;
        strHQL += " and goodsCheckInOrder.WareHouse Like '" + strWareHouse + "' And goodsCheckInOrder.VendorName Like '" + strVendorName + "'";
        strHQL += " Order By goodsCheckInOrder.CheckInID DESC";
        GoodsCheckInOrderBLL goodsCheckInOrderBLL = new GoodsCheckInOrderBLL();
        lst = goodsCheckInOrderBLL.GetAllGoodsCheckInOrders(strHQL);
        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strUserCode = LB_UserCode.Text;
            string strCIOID;

            DateTime dtCheckInTime, dtCurrentTime;

            strCIOID = e.Item.Cells[2].Text.Trim();
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "From GoodsCheckInOrder as goodsCheckInOrder Where goodsCheckInOrder.CheckInID = " + strCIOID;
            GoodsCheckInOrderBLL goodsCheckInOrderBLL = new GoodsCheckInOrderBLL();
            lst = goodsCheckInOrderBLL.GetAllGoodsCheckInOrders(strHQL);
            GoodsCheckInOrder goodsCheckInOrder = (GoodsCheckInOrder)lst[0];
            dtCurrentTime = DateTime.Now;
            dtCheckInTime = goodsCheckInOrder.CheckInDate;
            TimeSpan ts = dtCurrentTime - dtCheckInTime;

            LoadGoodsCheckInOrderDetail(strCIOID);

            if (e.CommandName == "Update")
            {

                LB_CheckInID.Text = strCIOID;

                try
                {
                    DL_CheckInType.SelectedValue = goodsCheckInOrder.CheckInType;
                }
                catch
                {
                }

                TB_GCIOName.Text = goodsCheckInOrder.GCIOName;
                DL_WareHouse.SelectedValue = goodsCheckInOrder.WareHouse.Trim();
                NB_Amount.Amount = goodsCheckInOrder.Amount;
                DLC_CheckInTime.Text = goodsCheckInOrder.CheckInDate.ToString("yyyy-MM-dd");
                LB_CheckInTime.Text = goodsCheckInOrder.CheckInDate.ToString("yyyy-MM-dd");
                DL_CurrencyType.SelectedValue = goodsCheckInOrder.CurrencyType;



                if (ts.Days == 0)
                {
                    BT_NewMain.Visible = true;
                    BT_CreateDetail.Visible = true;
                    BT_NewDetail.Visible = true;
                }
                else
                {
                    BT_NewMain.Visible = false;
                    BT_CreateDetail.Visible = false;
                    BT_NewDetail.Visible = false;
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                if (ts.Days > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuShiBaiZhiNengShanChuD").ToString().Trim()+"')", true);
                    return;
                }

                string strCreatorCode;
                strCreatorCode = LB_UserCode.Text.Trim();

                if (DataGrid1.Items.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZMXJLSCSBQJC").ToString().Trim() + "')", true);
                    return;
                }

                strHQL = "Delete From T_GoodsCheckInOrder Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_GoodsCheckInOrderDetail Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                LoadGoodsCheckInOrder(strCreatorCode, strRelatedType, strRelatedID);
                LoadGoodsCheckInOrderDetail(strCIOID);
            }
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_CheckInID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;
        BT_CreateDetail.Visible = true;

        string strNewCIOCode = ShareClass.GetCodeByRule("CheckInOrderCode", "CheckInOrderCode", "00");
        if (strNewCIOCode != "")
        {
            TB_GCIOName.Text = strNewCIOCode;
        }

        LoadGoodsCheckInOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strCIOID;

        strCIOID = LB_CheckInID.Text.Trim();

        if (strCIOID == "")
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
        string strCIOID, strSourceType, strWareHouse;
        string strCreatorCode, strCreatorName, strGCIOName;
        int intSourceID;
        decimal deAmount;
        DateTime dtCheckInDate;

        strGCIOName = TB_GCIOName.Text.Trim();
        strSourceType = DL_SourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = LB_UserName.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue;
        dtCheckInDate = DateTime.Parse(DLC_CheckInTime.Text);
        deAmount = NB_Amount.Amount;

        GoodsCheckInOrderBLL goodsCheckInOrderBLL = new GoodsCheckInOrderBLL();
        GoodsCheckInOrder goodsCheckInOrder = new GoodsCheckInOrder();

        goodsCheckInOrder.GCIOName = strGCIOName;
        goodsCheckInOrder.CheckInDate = dtCheckInDate;
        goodsCheckInOrder.CheckInType = DL_CheckInType.SelectedValue.Trim();
        goodsCheckInOrder.WareHouse = strWareHouse;
        goodsCheckInOrder.CreatorCode = strCreatorCode;
        goodsCheckInOrder.CreatorName = strCreatorName;
        goodsCheckInOrder.Amount = 0;

        goodsCheckInOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

        goodsCheckInOrder.RelatedType = strRelatedType;
        goodsCheckInOrder.RelatedID = int.Parse(strRelatedID);

        goodsCheckInOrder.PayStatus = "NO";

        try
        {
            goodsCheckInOrderBLL.AddGoodsCheckInOrder(goodsCheckInOrder);

            strCIOID = ShareClass.GetMyCreatedMaxGoodsCheckInID(strCreatorCode);
            LB_CheckInID.Text = strCIOID;
            LB_CheckInTime.Text = dtCheckInDate.ToString("yyyy-MM-dd");

            NB_Amount.Amount = 0;

            string strNewCIOCode = ShareClass.GetCodeByRule("CheckInOrderCode", "CheckInOrderCode", strCIOID);
            if (strNewCIOCode != "")
            {
                TB_GCIOName.Text = strNewCIOCode;
                string strHQL = "Update T_GoodsCheckInOrder Set GCIOName = " + "'" + strNewCIOCode + "'" + " Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);
            }


            LoadGoodsCheckInOrder(strCreatorCode, strRelatedType, strRelatedID);
            LoadGoodsCheckInOrderDetail(strCIOID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strCIOID, strSourceType, strWareHouse, strGCIOName;
        string strCreatorCode, strCreatorName;
        int intSourceID;
        decimal deAmount;
        DateTime dtCheckInDate;


        strCIOID = LB_CheckInID.Text.Trim();
        strGCIOName = TB_GCIOName.Text.Trim();
        strSourceType = DL_SourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = LB_UserName.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue;
        dtCheckInDate = DateTime.Parse(DLC_CheckInTime.Text);
        deAmount = NB_Amount.Amount;

        strHQL = "From GoodsCheckInOrder as goodsCheckInOrder Where goodsCheckInOrder.CheckInID = " + strCIOID;
        GoodsCheckInOrderBLL goodsCheckInOrderBLL = new GoodsCheckInOrderBLL();
        lst = goodsCheckInOrderBLL.GetAllGoodsCheckInOrders(strHQL);

        GoodsCheckInOrder goodsCheckInOrder = (GoodsCheckInOrder)lst[0];

        goodsCheckInOrder.GCIOName = strGCIOName;
        goodsCheckInOrder.CheckInDate = dtCheckInDate;
        goodsCheckInOrder.CheckInType = DL_CheckInType.SelectedValue.Trim();
        goodsCheckInOrder.WareHouse = strWareHouse;
        goodsCheckInOrder.CreatorCode = strCreatorCode;
        goodsCheckInOrder.CreatorName = strCreatorName;
        goodsCheckInOrder.Amount = deAmount;
        goodsCheckInOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

        goodsCheckInOrder.RelatedType = strRelatedType;
        goodsCheckInOrder.RelatedID = int.Parse(strRelatedID);

        try
        {
            goodsCheckInOrderBLL.UpdateGoodsCheckInOrder(goodsCheckInOrder, int.Parse(strCIOID));
            //Liujp 2013-07-17 更新物料登记入库表时，更新物料表中仓库字段
            ShareClass.UpdateGoodsPositionByGoodsCheckInOrder(strCIOID, strWareHouse);

            LoadGoodsCheckInOrder(strCreatorCode, strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID;
        string strUserCode;

        DateTime dtCurrentTime, dtCheckInTime;

        strUserCode = LB_UserCode.Text;


        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            ShareClass.LoadWareHousePositions(DL_WareHouse.SelectedValue.Trim(), DL_WHPosition);

            string strCIOID = LB_CheckInID.Text;
            strHQL = "From GoodsCheckInOrder as goodsCheckInOrder Where goodsCheckInOrder.CheckInID = " + strCIOID;
            GoodsCheckInOrderBLL goodsCheckInOrderBLL = new GoodsCheckInOrderBLL();
            lst = goodsCheckInOrderBLL.GetAllGoodsCheckInOrders(strHQL);
            GoodsCheckInOrder goodsCheckInOrder = (GoodsCheckInOrder)lst[0];
            dtCurrentTime = DateTime.Now;
            dtCheckInTime = goodsCheckInOrder.CheckInDate;
            TimeSpan ts = dtCurrentTime - dtCheckInTime;

            strID = e.Item.Cells[3].Text.Trim();

            if (e.CommandName == "Update")
            {
                strHQL = "from GoodsCheckInOrderDetail as goodsCheckInOrderDetail where goodsCheckInOrderDetail.ID = " + "'" + strID + "'";
                GoodsCheckInOrderDetailBLL goodsCheckInOrderDetailBLL = new GoodsCheckInOrderDetailBLL();
                lst = goodsCheckInOrderDetailBLL.GetAllGoodsCheckInOrderDetails(strHQL);
                GoodsCheckInOrderDetail goodsCheckInOrderDetail = (GoodsCheckInOrderDetail)lst[0];

                try
                {
                    DL_Type.SelectedValue = goodsCheckInOrderDetail.Type;
                }
                catch
                {
                }
                try
                {
                    DL_RecordSourceType.SelectedValue = goodsCheckInOrderDetail.SourceType.Trim();
                }
                catch
                {
                }

                try
                {
                    DL_Unit.SelectedValue = goodsCheckInOrderDetail.UnitName;
                }
                catch
                {
                }

                LB_ID.Text = goodsCheckInOrderDetail.ID.ToString();
                TB_GoodsCode.Text = goodsCheckInOrderDetail.GoodsCode.Trim();
                LB_OwnerCode.Text = goodsCheckInOrderDetail.OwnerCode;
                LB_OwnerName.Visible = true;
                LB_OwnerName.Text = ShareClass.GetUserName(goodsCheckInOrderDetail.OwnerCode);
                TB_GoodsCode.Text = goodsCheckInOrderDetail.GoodsCode;
                TB_GoodsName.Text = goodsCheckInOrderDetail.GoodsName;
                TB_SN.Text = goodsCheckInOrderDetail.SN;
                NB_WarrantyPeriod.Amount = goodsCheckInOrderDetail.WarrantyPeriod;
                NB_Number.Amount = goodsCheckInOrderDetail.CheckInNumber;
                NB_Price.Amount = goodsCheckInOrderDetail.Price;
                TB_Spec.Text = goodsCheckInOrderDetail.Spec;
                NB_Price.Amount = goodsCheckInOrderDetail.Price;
                if (goodsCheckInOrderDetail.IsTaxPrice.Trim() == "YES")
                {
                    CB_IsTaxPrice.Checked = true;
                }
                else
                {
                    CB_IsTaxPrice.Checked = false;
                }
                DLC_BuyTime.Text = goodsCheckInOrderDetail.BuyTime.ToString("yyyy-MM-dd");
                TB_ModelNumber.Text = goodsCheckInOrderDetail.ModelNumber;
                TB_SN.Text = goodsCheckInOrderDetail.SN.Trim();
                NB_WarrantyPeriod.Amount = goodsCheckInOrderDetail.WarrantyPeriod;
                TB_Supplier.Text = goodsCheckInOrderDetail.Supplier.Trim();
                TB_Manufacturer.Text = goodsCheckInOrderDetail.Manufacturer;
                TB_Memo.Text = goodsCheckInOrderDetail.Memo.Trim();
                LB_SourceRelatedID.Text = goodsCheckInOrderDetail.RelatedID.ToString();
                NB_RecordSourceID.Amount = goodsCheckInOrderDetail.SourceID;
                IM_ItemPhoto.ImageUrl = GetGoodsPhotoURL(goodsCheckInOrderDetail.GoodsCode.Trim(), strID);
                HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
                TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;

                BT_TakePhoto.Enabled = true;
                BT_DeletePhoto.Enabled = true;

                if (ts.Days == 0)
                {
                    BT_NewMain.Visible = true;
                    BT_CreateDetail.Visible = true;
                    BT_NewDetail.Visible = true;
                }
                else
                {
                    BT_NewMain.Visible = false;
                    BT_CreateDetail.Visible = false;
                    BT_NewDetail.Visible = false;
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strSourceType, strSourceID, strGoodsID;
                decimal dePrice, deNumber;


                if (ts.Days > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuShiBaiZhiNengShanChuD").ToString().Trim()+"')", true);
                    return;
                }


                GoodsCheckInOrderDetailBLL goodsCheckInOrderDetailBLL = new GoodsCheckInOrderDetailBLL();
                strHQL = "from GoodsCheckInOrderDetail as goodsCheckInOrderDetail where goodsCheckInOrderDetail.ID = " + strID;
                lst = goodsCheckInOrderDetailBLL.GetAllGoodsCheckInOrderDetails(strHQL);
                GoodsCheckInOrderDetail goodsCheckInOrderDetail = (GoodsCheckInOrderDetail)lst[0];

                strGoodsID = goodsCheckInOrderDetail.ToGoodsID.ToString();
                deNumber = goodsCheckInOrderDetail.Number;
                dePrice = goodsCheckInOrderDetail.Price;

                strSourceType = goodsCheckInOrderDetail.SourceType.Trim();
                strSourceID = goodsCheckInOrderDetail.SourceID.ToString();

                try
                {
                    goodsCheckInOrderDetailBLL.DeleteGoodsCheckInOrderDetail(goodsCheckInOrderDetail);

                    BT_TakePhoto.Enabled = false;
                    BT_DeletePhoto.Enabled = false;


                    //更新采购单生产单入库数量
                    UpdateGoodsPOOrPDCheckInNubmer(strSourceType, strSourceID);

                    LoadGoodsPurchaseOrderRecord(LB_POID.Text.Trim());
                    LoadGoodsCheckInOrderDetail(strCIOID);

                    HL_ItemPhoto.NavigateUrl = "";

                    if (ShareClass.GetGoodsStockCountMethod(DL_WareHouse.SelectedValue.Trim()) == "FIFO")
                    {
                        //删除物料明细表相同记录
                        strHQL = "Delete From T_Goods Where ID =" + strGoodsID;
                        ShareClass.RunSqlCommand(strHQL);
                    }
                    else
                    {
                        //按移动加权平均方法计算
                        ShareClass.CountGoodsStockByMWAM(strGoodsID, 0 - deNumber, dePrice, 0, 0);
                    }

                    //更新合同收货计划物料数量
                    if (strRelatedType == "Constract")
                    {
                        CountGoodsReceiptNumber(strRelatedType, strRelatedID);
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);

                }

            }
        }
    }

    protected void DL_SourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSourceType;

        strSourceType = DL_SourceType.SelectedValue.Trim();

        if (strSourceType == "Other")
        {
            NB_SourceID.Amount = 0;
        }

        if (strSourceType == "GoodsPO")
        {
            BT_Select.Visible = true;
            TabContainer1.ActiveTabIndex = 0;
        }
        else
        {
            BT_Select.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            TabContainer1.ActiveTabIndex = 2;

            string strPOID;

            strPOID = ((Button)e.Item.FindControl("BT_POID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            NB_SourceID.Amount = int.Parse(strPOID);

            LB_POID.Text = strPOID;

            LoadGoodsPurchaseOrderRecord(strPOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
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

        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strBrand;
        string strWareHouse;

        DataGrid4.CurrentPageIndex = 0;
        TabContainer1.ActiveTabIndex = 0;

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strBrand = TB_Manufacturer.Text.Trim();

        strType = "%" + strType + "%";
        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";
        strBrand = "%" + strBrand + "%";

        strWareHouse = DL_WareHouse.SelectedValue.Trim();

        strHQL = "Select * From T_Goods as goods Where goods.GoodsCode Like " + "'" + strGoodsCode + "'" + " and goods.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and goods.Type Like " + "'" + strType + "'" + " and goods.ModelNumber Like " + "'" + strModelNumber + "'" + " and goods.Spec Like " + "'" + strSpec + "'";
        strHQL += " and goods.Manufacturer Like " + "'" + strBrand + "'";
        strHQL += " and goods.Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by goods.Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;

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

        IM_ItemPhoto.ImageUrl = "";
        HL_ItemPhoto.NavigateUrl = "";
        TB_PhotoURL.Text = "";

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
                NB_Price.Amount = item.SalePrice;

                NB_WarrantyPeriod.Amount = item.WarrantyPeriod;

                IM_ItemPhoto.ImageUrl = item.PhotoURL;
                HL_ItemPhoto.NavigateUrl = item.PhotoURL;

                if (LB_SourceRelatedID.Text.Trim() == "0")
                {
                    DL_RecordSourceType.SelectedValue = "Other";
                    NB_RecordSourceID.Amount = 0;
                    NB_Price.Amount = item.PurchasePrice;
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }


    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strPOID;
            IList lst;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text;

            strPOID = LB_POID.Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from GoodsPurRecord as goodsPurRecord where goodsPurRecord.ID = " + strID;

            GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
            lst = goodsPurRecordBLL.GetAllGoodsPurRecords(strHQL);
            GoodsPurRecord goodsPurRecord = (GoodsPurRecord)lst[0];

            TB_GoodsCode.Text = goodsPurRecord.GoodsCode;
            TB_GoodsName.Text = goodsPurRecord.GoodsName;
            TB_ModelNumber.Text = goodsPurRecord.ModelNumber;
            TB_Spec.Text = goodsPurRecord.Spec;
            DL_Type.SelectedValue = goodsPurRecord.Type;
            NB_Number.Amount = goodsPurRecord.Number - goodsPurRecord.CheckInNumber;
            NB_Price.Amount = goodsPurRecord.Price;

            DL_Type.SelectedValue = goodsPurRecord.Type;
            DL_Unit.SelectedValue = goodsPurRecord.Unit;
            DLC_BuyTime.Text = goodsPurRecord.PurTime.ToString("yyyy-MM-dd");

            TB_Manufacturer.Text = goodsPurRecord.Brand;

            LB_SourceRelatedID.Text = goodsPurRecord.POID.ToString();
            DL_RecordSourceType.SelectedValue = "GoodsPORecord";
            NB_RecordSourceID.Amount = goodsPurRecord.ID;

            IM_ItemPhoto.ImageUrl = GetGoodsPhotoURL(goodsPurRecord.GoodsCode, "0");
            HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
            TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;

            TB_Memo.Text = "";

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }


    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
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
                DL_Type.SelectedValue = goods.Type;
                TB_Manufacturer.Text = goods.Manufacturer;

                TB_SN.Text = goods.SN.Trim();
                NB_WarrantyPeriod.Amount = goods.WarrantyPeriod;

                IM_ItemPhoto.ImageUrl = goods.PhotoURL;
                HL_ItemPhoto.NavigateUrl = goods.PhotoURL;

                if (LB_SourceRelatedID.Text.Trim() == "0")
                {
                    DL_RecordSourceType.SelectedValue = "Other";
                    NB_RecordSourceID.Amount = 0;
                    NB_Price.Amount = goods.Price;
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }


    protected void BT_TakePhoto_Click(object sender, EventArgs e)
    {
        Panel9.Visible = true;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void BT_DeletePhoto_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strGoodsCode = TB_GoodsCode.Text.Trim();

        try
        {
            strHQL = "Update T_Goods Set PhotoURL = '' Where GoodsCode = " + "'" + strGoodsCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_ItemPhoto.ImageUrl = "";
            HL_ItemPhoto.NavigateUrl = "";


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void BT_SavePhoto_Click(object sender, EventArgs e)
    {
        string strGoodsCode;
        string strGoodsPhotoString;

        strGoodsCode = TB_GoodsCode.Text.Trim();

        strGoodsPhotoString = TB_PhotoString1.Text.Trim();
        strGoodsPhotoString += TB_PhotoString2.Text.Trim();
        strGoodsPhotoString += TB_PhotoString3.Text.Trim();
        strGoodsPhotoString += TB_PhotoString4.Text.Trim();

        if (strGoodsPhotoString != "")
        {
            var binaryData = Convert.FromBase64String(strGoodsPhotoString);

            string strDateTime = DateTime.Now.ToString("yyyyMMddHHMMssff");
            string strGoodsPhotoURL = "Doc\\" + "UserPhoto\\" + strGoodsCode + strDateTime + ".jpg";
            var imageFilePath = Server.MapPath("Doc") + "\\UserPhoto\\" + strGoodsCode + strDateTime + ".jpg";

            if (File.Exists(imageFilePath))
            { File.Delete(imageFilePath); }
            var stream = new System.IO.FileStream(imageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Write(binaryData, 0, binaryData.Length);
            stream.Close();

            string strHQL = "Update T_Goods Set PhotoURL = " + "'" + strGoodsPhotoURL + "'" + " Where GoodsCode = " + "'" + strGoodsCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_ItemPhoto.ImageUrl = strGoodsPhotoURL;
            HL_ItemPhoto.NavigateUrl = strGoodsPhotoURL;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_UploadPhoto_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            string strGoodsCode = TB_GoodsCode.Text.Trim();
            string strHQL;
            int i;

            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;


                string strDocSavePath = Server.MapPath("Doc") + "\\UserPhoto\\";
                string strFileName3 = "Doc\\UserPhoto\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_File.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_Goods Set PhotoURL = " + "'" + strFileName3 + "'" + " Where GoodsCode = " + "'" + strGoodsCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_ItemPhoto.ImageUrl = strFileName3;
                    HL_ItemPhoto.NavigateUrl = strFileName3;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCHCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ShareClass.LoadWareHousePositions(DL_WareHouse.SelectedValue.Trim(), DL_WHPosition);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }


    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strCIOID;

        strCIOID = LB_CheckInID.Text.Trim();

        if (strCIOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
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
        string strHQL;

        string strCIOID, strOwnerCode, strType, strGoodsCode, strGoodsName, strSN, strModelNumber, strSpec, strManufacturer, strIP, strPosition, strMemo;
        string strSourceType, strSourceID, strSourceRelatedID, strCurrencyType, strIsTaxPrice;
        DateTime dtBuyTime;
        decimal dePrice;
        int intWarrantyPeriod;

        string strUserCode = LB_UserCode.Text;

        string strUnitName;
        decimal deNumber;

        strCIOID = LB_CheckInID.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strSN = TB_SN.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        strPosition = DL_WareHouse.SelectedValue.Trim();

        dePrice = NB_Price.Amount;
        if (CB_IsTaxPrice.Checked)
        {
            strIsTaxPrice = "YES";
        }
        else
        {
            strIsTaxPrice = "NO";
        }

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strMemo = TB_Memo.Text.Trim();

        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        strSourceRelatedID = LB_SourceRelatedID.Text.Trim();
        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strSourceID = NB_RecordSourceID.Amount.ToString();

        if (strRelatedType == "Constract")
        {
            if (strRelatedGoodsCode == "" | strRelatedGoodsCode != strGoodsCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click11", "alert('" + LanguageHandle.GetWord("ZZCWZHTSHJHGDDLPJC").ToString().Trim() + "')", true);
                return;
            }
        }

        if (strOwnerCode == "" | strType == "" | strGoodsCode == "" | strSpec == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickDFDFF", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
        else
        {
            GoodsCheckInOrderDetailBLL goodsCheckInOrderDetailBLL = new GoodsCheckInOrderDetailBLL();
            GoodsCheckInOrderDetail goodsCheckInOrderDetail = new GoodsCheckInOrderDetail();

            goodsCheckInOrderDetail.CheckInID = int.Parse(strCIOID);
            goodsCheckInOrderDetail.GoodsCode = strGoodsCode;
            goodsCheckInOrderDetail.GoodsName = strGoodsName;
            goodsCheckInOrderDetail.Number = deNumber;
            goodsCheckInOrderDetail.CheckInNumber = deNumber;
            goodsCheckInOrderDetail.UnitName = strUnitName;
            goodsCheckInOrderDetail.OwnerCode = strOwnerCode;


            try
            {
                goodsCheckInOrderDetail.OwnerName = ShareClass.GetUserName(strOwnerCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
                return;
            }
            goodsCheckInOrderDetail.Type = strType;
            goodsCheckInOrderDetail.Spec = strSpec;
            goodsCheckInOrderDetail.ModelNumber = strModelNumber;
            goodsCheckInOrderDetail.Position = strPosition;
            goodsCheckInOrderDetail.WHPosition = DL_WHPosition.SelectedValue.Trim();

            goodsCheckInOrderDetail.Price = dePrice;
            goodsCheckInOrderDetail.IsTaxPrice = strIsTaxPrice;
            goodsCheckInOrderDetail.Amount = deNumber * dePrice;
            goodsCheckInOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            if (strSN == "")
            {
                //strSN = strGoodsCode;
            }

            goodsCheckInOrderDetail.SN = strSN;
            goodsCheckInOrderDetail.WarrantyPeriod = intWarrantyPeriod;

            goodsCheckInOrderDetail.BuyTime = dtBuyTime;
            goodsCheckInOrderDetail.Supplier = TB_Supplier.Text.Trim();
            goodsCheckInOrderDetail.Manufacturer = strManufacturer;
            goodsCheckInOrderDetail.Memo = strMemo;
            goodsCheckInOrderDetail.Status = "InUse";

            if (strSourceRelatedID == "")
            {
                goodsCheckInOrderDetail.RelatedID = 0;
            }
            else
            {
                goodsCheckInOrderDetail.RelatedID = int.Parse(strSourceRelatedID);
            }
            if (strSourceType == "")
            {
                goodsCheckInOrderDetail.SourceType = "Other";
            }
            else
            {
                goodsCheckInOrderDetail.SourceType = strSourceType;
            }
            if (strSourceID == "")
            {
                goodsCheckInOrderDetail.SourceID = 0;
            }
            else
            {
                goodsCheckInOrderDetail.SourceID = int.Parse(strSourceID);
            }


            try
            {
                goodsCheckInOrderDetailBLL.AddGoodsCheckInOrderDetail(goodsCheckInOrderDetail);
                LB_ID.Text = ShareClass.GetMyCreatedMaxGoodsCheckInDetailID(strCIOID).ToString();


                LoadGoodsCheckInOrderDetail(strCIOID);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                BT_TakePhoto.Enabled = true;
                BT_DeletePhoto.Enabled = true;

                NB_Amount.Amount = SumGoodsCheckInOrderAmount(strCIOID);
                strHQL = "Update T_GoodsCheckInOrder Set Amount = " + NB_Amount.Amount.ToString();
                strHQL += " Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                //更新采购单生产单入库数量
                UpdateGoodsPOOrPDCheckInNubmer(strSourceType, strSourceID);

                LoadGoodsPurchaseOrderRecord(LB_POID.Text.Trim());



                string strCountMethod = ShareClass.GetGoodsStockCountMethod(DL_WareHouse.SelectedValue.Trim());
                string strPhotoURL = HL_ItemPhoto.NavigateUrl.Trim();
                //添加相同记录到物料表
                ShareClass.addOrUpdateGoods(strCountMethod, "0", strCIOID, strGoodsCode, strGoodsName, strSN, deNumber, strUnitName,
                    strOwnerCode, strType, strSpec, strModelNumber, strPosition, DL_WHPosition.SelectedValue.Trim(), dePrice, strIsTaxPrice, strCurrencyType, dtBuyTime, intWarrantyPeriod,
                    strManufacturer, strMemo, LB_ID.Text, strPhotoURL, 0, 0,
                    "", DateTime.Now, DateTime.Now, "", "");


                //保存产品图片
                UpdateGoodsPhoto(LB_ID.Text);

                IM_ItemPhoto.ImageUrl = "";
                HL_ItemPhoto.NavigateUrl = "";

                //更新合同收货计划物料数量
                if (strRelatedType == "Constract")
                {
                    CountGoodsReceiptNumber(strRelatedType, strRelatedID);
                }

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdateDetail()
    {
        string strCIOID, strID, strOwnerCode, strType, strGoodsCode, strGoodsName, strSN, strModelNumber, strSpec, strIP, strManufacturer, strPosition, strMemo;
        string strSourceType, strSourceID, strSourceRelatedID, strCurrencyType, strIsTaxPrice;
        DateTime dtBuyTime;
        decimal dePrice;
        int intWarrantyPeriod;

        string strUserCode = LB_UserCode.Text;
        string strHQL;
        string strUnitName;
        decimal deNumber;

        IList lst;

        strCIOID = LB_CheckInID.Text.Trim();
        strID = LB_ID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strSN = TB_SN.Text.Trim();
        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());
        strModelNumber = TB_ModelNumber.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        strPosition = DL_WareHouse.SelectedValue.Trim();

        dePrice = NB_Price.Amount;
        if (CB_IsTaxPrice.Checked)
        {
            strIsTaxPrice = "YES";
        }
        else
        {
            strIsTaxPrice = "NO";
        }
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strMemo = TB_Memo.Text.Trim();

        strSourceRelatedID = LB_SourceRelatedID.Text.Trim();
        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strSourceID = NB_RecordSourceID.Amount.ToString();

        if (strRelatedType == "Constract")
        {
            if (strRelatedGoodsCode == "" | strRelatedGoodsCode != strGoodsCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click44", "alert('" + LanguageHandle.GetWord("ZZCWZHTSHJHGDDLPJC").ToString().Trim() + "')", true);
                return;
            }
        }

        if (strOwnerCode == "" | strType == "" | strGoodsCode == "" | strSpec == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click55", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
        else
        {
            GoodsCheckInOrderDetailBLL goodsCheckInOrderDetailBLL = new GoodsCheckInOrderDetailBLL();
            strHQL = "from GoodsCheckInOrderDetail as goodsCheckInOrderDetail where goodsCheckInOrderDetail.ID = " + strID;
            lst = goodsCheckInOrderDetailBLL.GetAllGoodsCheckInOrderDetails(strHQL);
            GoodsCheckInOrderDetail goodsCheckInOrderDetail = (GoodsCheckInOrderDetail)lst[0];

            goodsCheckInOrderDetail.ID = int.Parse(strID);
            goodsCheckInOrderDetail.CheckInID = int.Parse(strCIOID);
            goodsCheckInOrderDetail.GoodsCode = strGoodsCode;
            goodsCheckInOrderDetail.GoodsName = strGoodsName;
            if (strSN == "")
            {
                //strSN = strGoodsCode;
            }
            goodsCheckInOrderDetail.SN = strSN;
            goodsCheckInOrderDetail.WarrantyPeriod = intWarrantyPeriod;

            goodsCheckInOrderDetail.Number = deNumber;
            goodsCheckInOrderDetail.CheckInNumber = deNumber;
            goodsCheckInOrderDetail.UnitName = strUnitName;
            goodsCheckInOrderDetail.OwnerCode = strOwnerCode;
            try
            {
                goodsCheckInOrderDetail.OwnerName = ShareClass.GetUserName(strOwnerCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click66", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
                return;
            }

            goodsCheckInOrderDetail.Type = strType;
            goodsCheckInOrderDetail.ModelNumber = strModelNumber;
            goodsCheckInOrderDetail.Spec = strSpec;
            goodsCheckInOrderDetail.Position = strPosition;
            goodsCheckInOrderDetail.WHPosition = DL_WHPosition.SelectedValue.Trim();

            goodsCheckInOrderDetail.Price = dePrice;
            goodsCheckInOrderDetail.IsTaxPrice = strIsTaxPrice;
            goodsCheckInOrderDetail.Amount = deNumber * dePrice;

            goodsCheckInOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            goodsCheckInOrderDetail.Supplier = TB_Supplier.Text.Trim();
            goodsCheckInOrderDetail.Manufacturer = strManufacturer;
            goodsCheckInOrderDetail.Memo = strMemo;
            goodsCheckInOrderDetail.BuyTime = dtBuyTime;

            if (strSourceRelatedID == "")
            {
                goodsCheckInOrderDetail.RelatedID = 0;
            }
            else
            {
                goodsCheckInOrderDetail.RelatedID = int.Parse(strSourceRelatedID);
            }

            if (strSourceType == "")
            {
                goodsCheckInOrderDetail.SourceType = "Other";
            }
            else
            {
                goodsCheckInOrderDetail.SourceType = strSourceType;
            }
            if (strSourceID == "")
            {
                goodsCheckInOrderDetail.SourceID = 0;
            }
            else
            {
                goodsCheckInOrderDetail.SourceID = int.Parse(strSourceID);
            }


            try
            {
                goodsCheckInOrderDetailBLL.UpdateGoodsCheckInOrderDetail(goodsCheckInOrderDetail, int.Parse(strID));

                NB_Amount.Amount = SumGoodsCheckInOrderAmount(strCIOID);
                strHQL = "Update T_GoodsCheckInOrder Set Amount = " + NB_Amount.Amount.ToString();
                strHQL += " Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                //更新采购单生产单入库数量
                UpdateGoodsPOOrPDCheckInNubmer(strSourceType, strSourceID);

                ////更新应收应付数据到应收应付表
                //if (strSourceType == "GoodsPORecord")
                //{
                //    string strSupplier = TB_Supplier.Text.Trim();
                //    ShareClass.UpdateReceivablesOrPayable("GoodsCO", "GoodsPO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strSupplier, strUserCode);
                //}

                LoadGoodsPurchaseOrderRecord(LB_POID.Text.Trim());
                LoadGoodsCheckInOrderDetail(strCIOID);

                //删除物料明细表相同记录
                strHQL = "Delete From T_Goods Where CheckInDetailID =" + LB_ID.Text;
                ShareClass.RunSqlCommand(strHQL);

                //添加相同记录到物料明细表
                string strGoodsID = goodsCheckInOrderDetail.ToGoodsID.ToString();
                string strCountMethod = ShareClass.GetGoodsStockCountMethod(DL_WareHouse.SelectedValue.Trim());
                string strPhotoURL = HL_ItemPhoto.NavigateUrl.Trim();
                //取得原来的数量和价格
                decimal deOldCheckInNumber = goodsCheckInOrderDetail.Number;
                decimal deOldCheckInPrice = goodsCheckInOrderDetail.Price;

                ShareClass.addOrUpdateGoods(strCountMethod, strGoodsID, strCIOID, strGoodsCode, strGoodsName, strSN, deNumber, strUnitName,
                   strOwnerCode, strType, strSpec, strModelNumber, strPosition, DL_WHPosition.SelectedValue.Trim(), dePrice, strIsTaxPrice, strCurrencyType, dtBuyTime, intWarrantyPeriod,
                   strManufacturer, strMemo, strID, strPhotoURL, deOldCheckInNumber, deOldCheckInPrice, 
                    "", DateTime.Now, DateTime.Now, "", "");

                //保存产品图片
                UpdateGoodsPhoto(strID);

                //更新合同收货计划物料数量
                if (strRelatedType == "Constract")
                {
                    CountGoodsReceiptNumber(strRelatedType, strRelatedID);
                }


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click77", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdateGoodsPhoto(string strID)
    {
        string strHQL;
        string strPhotoURL = TB_PhotoURL.Text.Trim();
        string strGoodsCode = TB_GoodsCode.Text.Trim();

        if (strPhotoURL != "" & strGoodsCode != "")
        {
            strHQL = "Update T_Goods Set PhotoURL = " + "'" + strPhotoURL + "'" + " Where GoodsCode = " + "'" + strGoodsCode + "'";
            strHQL += " and CheckInDetailID = " + strID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void UpdateGoodsPOOrPDCheckInNubmer(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deCheckinNumber;

        if (strSourceType == "GoodsPORecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where SourceType= 'GoodsPORecord' and SourceID =" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            if (ds.Tables[0].Rows.Count > 0)
            {
                deCheckinNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deCheckinNumber = 0;
            }

            strHQL = "Update T_GoodsPurRecord Set CheckInNumber = " + deCheckinNumber.ToString();
            strHQL += " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strSourceType == "GoodsPDRecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where SourceType= 'GoodsPDRecord' and SourceID =" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            try
            {
                deCheckinNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deCheckinNumber = 0;
            }

            strHQL = "Update T_GoodsProductionOrderDetail Set CheckInNumber = " + deCheckinNumber.ToString();
            strHQL += " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql2.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurchaseOrder");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        GoodsCheckInOrderBLL goodsCheckInOrderBLL = new GoodsCheckInOrderBLL();
        IList lst = goodsCheckInOrderBLL.GetAllGoodsCheckInOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected string GetGoodsPhotoURL(string strGoodsCode, string strCheckInDetailID)
    {
        string strHQL = "Select PhotoURL from T_Goods as goods where goods.GoodsCode = " + "'" + strGoodsCode + "'";
        if (strCheckInDetailID != "0")
        {
            strHQL += " and goods.checkInDetailID = " + strCheckInDetailID;
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }


    protected decimal SumGoodsCheckInOrderAmount(string strCIOID)
    {
        string strHQL;

        decimal deAmount = 0;

        strHQL = "Select Sum(CheckInNumber * Price) From T_GoodsCheckInOrderDetail Where CheckInID = " + strCIOID;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");

        deAmount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

        return deAmount;
    }

    protected void LoadGoodsCheckInOrderDetail(string strCheckInID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsCheckInOrderDetail as goodsCheckInOrderDetail where goodsCheckInOrderDetail.CheckInID = " + strCheckInID;
        strHQL += " Order by goodsCheckInOrderDetail.Number DESC,goodsCheckInOrderDetail.ID DESC";
        GoodsCheckInOrderDetailBLL goodsCheckInOrderDetailBLL = new GoodsCheckInOrderDetailBLL();
        lst = goodsCheckInOrderDetailBLL.GetAllGoodsCheckInOrderDetails(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadGoodsCheckInOrder(string strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsCheckInOrder as goodsCheckInOrder where goodsCheckInOrder.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " and goodsCheckInOrder.RelatedType=" + "'" + strRelatedType + "'" + " and goodsCheckInOrder.RelatedID = " + strRelatedID;
        strHQL += " Order By goodsCheckInOrder.CheckInID DESC";
        GoodsCheckInOrderBLL goodsCheckInOrderBLL = new GoodsCheckInOrderBLL();
        lst = goodsCheckInOrderBLL.GetAllGoodsCheckInOrders(strHQL);
        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadGoodsPurchaseOrder(string strOperatorCode)
    {
        string strHQL;


        strHQL = "Select * from T_GoodsPurchaseOrder where OperatorCode in ( Select UserCode From T_ProjectMember Where DepartCode in " + LB_DepartString.Text.Trim() + ")";
        strHQL += " Order by POID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurchaseOrder");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadGoodsPurchaseOrderRecord(string strPOID)
    {
        string strHQL;

        if (strPOID == "")
        {
            strPOID = "0";
        }

        strHQL = "Select * from T_GoodsPurRecord where POID = " + strPOID;
        strHQL += " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        LB_POID.Text = strPOID;

        LB_Sql3.Text = strHQL;
    }

    protected void LoadWareHouseListByAuthority(string strUserCode)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = " Select WHName From T_WareHouse Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By SortNumber DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse");

        DL_WareHouse.DataSource = ds;
        DL_WareHouse.DataBind();
    }

    protected void CountGoodsReceiptNumber(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        decimal deReceiverNumber = 0;

        strHQL = "from GoodsCheckInOrderDetail as goodsCheckInOrderDetail where goodsCheckInOrderDetail.CheckInID in ";
        strHQL += " (Select goodsCheckInOrder.CheckInID From GoodsCheckInOrder as goodsCheckInOrder Where goodsCheckInOrder.RelatedType = " + "'" + strRelatedType + "'" + " and goodsCheckInOrder.RelatedID = " + strRelatedID + ")";
        strHQL += " Order by goodsCheckInOrderDetail.Number DESC,goodsCheckInOrderDetail.ID DESC";
        GoodsCheckInOrderDetailBLL goodsCheckInOrderDetailBLL = new GoodsCheckInOrderDetailBLL();
        lst = goodsCheckInOrderDetailBLL.GetAllGoodsCheckInOrderDetails(strHQL);

        GoodsCheckInOrderDetail goodsCheckInOrderDetail = new GoodsCheckInOrderDetail();

        for (int i = 0; i < lst.Count; i++)
        {
            goodsCheckInOrderDetail = (GoodsCheckInOrderDetail)lst[i];

            deReceiverNumber += goodsCheckInOrderDetail.Number;
        }

        strHQL = "Update T_ConstractGoodsReceiptPlan Set ReceiptedNumber = " + deReceiverNumber.ToString() + " where ID =" + strRelatedID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractGoodsReceiptPlan Set UNReceiptedNumber = " + (deReceivablesNumber - deReceiverNumber).ToString() + " where ID =" + strRelatedID;
        ShareClass.RunSqlCommand(strHQL);
    }

}
