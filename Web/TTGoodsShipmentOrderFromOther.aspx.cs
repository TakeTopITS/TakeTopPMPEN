using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGoodsShipmentOrderFromOther : System.Web.UI.Page
{
    string strUserCode;
    string strRelatedType, strRelatedID, strConstractCode, strRelatedGoodsCode = "";
    decimal deDeliveablesNumber = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        strUserCode = Session["UserCode"].ToString();


        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        if (strRelatedType == null)
        {
            strRelatedType = "Other";
            strRelatedID = "0";
        }

        if (strRelatedType == "Constract")
        {
            strHQL = "from ConstractGoodsDeliveryPlan as constractGoodsDeliveryPlan where constractGoodsDeliveryPlan.ID = " + strRelatedID;
            ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
            lst = constractGoodsDeliveryPlanBLL.GetAllConstractGoodsDeliveryPlans(strHQL);

            ConstractGoodsDeliveryPlan constractGoodsDeliveryPlan = (ConstractGoodsDeliveryPlan)lst[0];

            strConstractCode = constractGoodsDeliveryPlan.ConstractCode.Trim();
            strRelatedGoodsCode = constractGoodsDeliveryPlan.GoodsCode.Trim();
            deDeliveablesNumber = constractGoodsDeliveryPlan.Number;

            DL_GoodsType.SelectedValue = constractGoodsDeliveryPlan.Type;
            TB_GoodsCode.Text = constractGoodsDeliveryPlan.GoodsCode;
            TB_GoodsName.Text = constractGoodsDeliveryPlan.GoodsName;
            TB_ModelNumber.Text = constractGoodsDeliveryPlan.ModelNumber;
            TB_Spec.Text = constractGoodsDeliveryPlan.Spec;
            TB_Manufacturer.Text = constractGoodsDeliveryPlan.Brand;

            NB_Price.Amount = constractGoodsDeliveryPlan.Price;

            DL_RecordSourceType.Enabled = false;
            NB_RecordSourceID.Enabled = false;
            DL_GoodsType.Enabled = false;

            TB_GoodsCode.Enabled = false;
            TB_GoodsName.Enabled = false;
            TB_ModelNumber.Enabled = false;
            TB_Spec.Enabled = false;
            TB_Manufacturer.Enabled = false;

            BT_Clear.Enabled = false;
        }
        else
        {
            strRelatedGoodsCode = "";
            deDeliveablesNumber = 0;
        }

        string strWareHouse;

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ShipTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "Select * From T_GoodsShipmentType Order By SortNumber ASC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentType");
            DL_ShipmentType.DataSource = ds;
            DL_ShipmentType.DataBind();
            DL_ShipmentType.Items.Insert(0, new ListItem("--Select--", ""));

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_GoodsType.DataSource = lst;
            DL_GoodsType.DataBind();
            DL_GoodsType.Items.Insert(0, new ListItem("--Select--", ""));

            ShareClass.LoadCurrencyType(DL_CurrencyType);

            LoadWareHouseListByAuthority(strDepartString);

            LoadGoodsApplication(strUserCode);
            LoadGoodsSaleOrder(strUserCode);
            ShareClass.LoadCustomer(DL_Customer, strUserCode);

            LoadGoodsShipmentOrder(strUserCode, strRelatedType, strRelatedID);

            if (DL_WareHouse.Items.Count > 0)
            {
                strWareHouse = DL_WareHouse.SelectedValue.Trim();
                LoadAllGoodsByWareHouse(strWareHouse);
            }

            ShareClass.InitialInvolvedProjectTree(TreeView1, strUserCode);
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

        if (strSourceType == "GoodsAO")
        {
            BT_SelectAO.Visible = true;
        }
        else
        {
            BT_SelectAO.Visible = false;
        }

        if (strSourceType == "GoodsSO")
        {
            BT_SelectSO.Visible = true;
        }
        else
        {
            BT_SelectSO.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strAAID;

        if (e.CommandName != "Page")
        {
            TabContainer1.ActiveTabIndex = 2;

            strAAID = ((Button)e.Item.FindControl("BT_AAID")).Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_AOID.Text = strAAID;

            NB_SourceID.Amount = int.Parse(strAAID);

            LoadGoodsApplicationDetail(strAAID);

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

            LoadGoodsSaleOrderDetail(strSOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strProjectID = treeNode.Target.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strShipmentNO;
            DateTime dtCheckOutTime, dtCurrentTime;

            strShipmentNO = e.Item.Cells[2].Text.Trim();
            LB_ShipmentNO.Text = strShipmentNO;

            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "From GoodsShipmentOrder as goodsShipmentOrder Where goodsShipmentOrder.ShipmentNO = " + strShipmentNO;
            GoodsShipmentOrderBLL goodsShipmentOrderBLL = new GoodsShipmentOrderBLL();
            lst = goodsShipmentOrderBLL.GetAllGoodsShipmentOrders(strHQL);
            GoodsShipmentOrder goodsShipmentOrder = (GoodsShipmentOrder)lst[0];
            LB_ShipmentNO.Text = strShipmentNO;

            dtCurrentTime = DateTime.Now;
            dtCheckOutTime = goodsShipmentOrder.ShipTime;
            TimeSpan ts = dtCurrentTime - dtCheckOutTime;

            LoadGoodsShipmentDetail(strShipmentNO);

            if (e.CommandName == "Update")
            {
                try
                {
                    DL_ShipmentType.SelectedValue = goodsShipmentOrder.ShipmentType;
                }
                catch
                {
                }

                try
                {
                    DL_WareHouse.SelectedValue = goodsShipmentOrder.WareHouse;
                }
                catch
                {
                }

                try
                {
                    DL_Customer.SelectedValue = goodsShipmentOrder.CustomerCode;
                }
                catch
                {
                }

                try
                {
                    DL_CurrencyType.SelectedValue = goodsShipmentOrder.CurrencyType;
                }
                catch
                {
                }

                TB_GSHOName.Text = goodsShipmentOrder.GSHOName.Trim();
                DLC_ShipTime.Text = goodsShipmentOrder.ShipTime.ToString("yyyy-MM-dd");
                TB_ShipmentReason.Text = goodsShipmentOrder.ApplicationReason.Trim();
                TB_Applicant.Text = goodsShipmentOrder.Applicant.Trim();

              

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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuShiBaiZhiNengShanChuD")+"')", true);
                    return;
                }

                try
                {
                    if (DataGrid1.Items.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZMXJLSCSBQJC") + "')", true);
                        return;
                    }

                    strHQL = "Delete From T_GoodsShipmentOrder Where ShipmentNO = " + strShipmentNO;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_GoodsShipmentDetail Where ShipmentNO = " + strShipmentNO;
                    ShareClass.RunSqlCommand(strHQL);

                    LB_ShipmentNO.Text = "";

                    LoadGoodsShipmentOrder(strUserCode, strRelatedType, strRelatedID);
                    LoadGoodsShipmentDetail(strShipmentNO);
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

    protected void DL_WareHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWareHouse;

        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        LoadAllGoodsByWareHouse(strWareHouse);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_ShipmentNO.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;
        BT_CreateDetail.Visible = true;

        DLC_ShipTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

        LoadGoodsShipmentDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strShipmentNO;

        strShipmentNO = LB_ShipmentNO.Text.Trim();

        if (strShipmentNO == "")
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
        string strShipmentNO, strWareHouse, strApplicant, strShipReason, strSourceType, strSourceID, strCurrencyType, strCustomerCode, strCustomerName, strGSHOName;
        DateTime dtShipTime;


        strGSHOName = TB_GSHOName.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        dtShipTime = DateTime.Parse(DLC_ShipTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strShipReason = TB_ShipmentReason.Text.Trim();

        strCustomerCode = DL_Customer.SelectedValue;
        if (strCustomerCode != "")
        {
            strCustomerName = DL_Customer.SelectedItem.Text;
        }
        else
        {
            strCustomerName = "";
        }

        strSourceType = DL_SourceType.SelectedValue.Trim();
        strSourceID = NB_SourceID.Amount.ToString();

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        try
        {
            GoodsShipmentOrderBLL goodsShipmentOrderBLL = new GoodsShipmentOrderBLL();
            GoodsShipmentOrder goodsShipmentOrder = new GoodsShipmentOrder();

            goodsShipmentOrder.GSHOName = strGSHOName;
            goodsShipmentOrder.ShipmentType = DL_ShipmentType.SelectedValue;
            goodsShipmentOrder.WareHouse = strWareHouse;
            goodsShipmentOrder.Applicant = strApplicant;
            goodsShipmentOrder.ShipTime = dtShipTime;
            goodsShipmentOrder.ApplicationReason = strShipReason;
            goodsShipmentOrder.RelatedType = strRelatedType;
            goodsShipmentOrder.RelatedID = int.Parse(strRelatedID);

            if (strRelatedType == "Constract")
            {
                goodsShipmentOrder.RelatedCode = strConstractCode;
            }

            goodsShipmentOrder.CustomerCode = strCustomerCode;
            goodsShipmentOrder.CustomerName = strCustomerName;

            goodsShipmentOrder.OperatorCode = strUserCode;
            goodsShipmentOrder.OperatorName = ShareClass.GetUserName(strUserCode);

            goodsShipmentOrder.CurrencyType = strCurrencyType;



            goodsShipmentOrderBLL.AddGoodsShipmentOrder(goodsShipmentOrder);

            strShipmentNO = ShareClass.GetMyCreatedMaxGoodsShipmentNO(strUserCode).ToString();

            LB_ShipmentNO.Text = strShipmentNO;

            LoadGoodsShipmentOrder(strUserCode, strRelatedType, strRelatedID);
            LoadGoodsShipmentDetail(strShipmentNO);

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

        string strShipmentNO, strShipmentType, strWareHouse, strApplicant, strShipReason, strSourceType, strSourceID, strCurrencyType, strCustomerCode, strCustomerName, strGSHOName;
        DateTime dtShipTime;

        strShipmentType = DL_ShipmentType.SelectedValue;
        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strGSHOName = TB_GSHOName.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        dtShipTime = DateTime.Parse(DLC_ShipTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strShipReason = TB_ShipmentReason.Text.Trim();
        strCustomerCode = DL_Customer.SelectedValue;

        if (strCustomerCode != "")
        {
            strCustomerName = DL_Customer.SelectedItem.Text;
        }
        else
        {
            strCustomerName = "";
        }

        strSourceType = DL_SourceType.SelectedValue.Trim();
        strSourceID = NB_SourceID.Amount.ToString();

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        Label2.Text = strShipmentType;

        try
        {
            strHQL = "From GoodsShipmentOrder as goodsShipmentOrder Where goodsShipmentOrder.ShipmentNO = " + strShipmentNO;
            GoodsShipmentOrderBLL goodsShipmentOrderBLL = new GoodsShipmentOrderBLL();
            lst = goodsShipmentOrderBLL.GetAllGoodsShipmentOrders(strHQL);
            GoodsShipmentOrder goodsShipmentOrder = (GoodsShipmentOrder)lst[0];

            goodsShipmentOrder.GSHOName = strGSHOName;
            goodsShipmentOrder.ShipmentType = strShipmentType;
            goodsShipmentOrder.WareHouse = strWareHouse;
            goodsShipmentOrder.Applicant = strApplicant;
            goodsShipmentOrder.ShipTime = dtShipTime;
            goodsShipmentOrder.ApplicationReason = strShipReason;

            goodsShipmentOrder.RelatedType = strRelatedType;
            goodsShipmentOrder.RelatedID = int.Parse(strRelatedID);

            goodsShipmentOrder.CustomerCode = strCustomerCode;
            goodsShipmentOrder.CustomerName = strCustomerName;

            goodsShipmentOrder.OperatorCode = strUserCode;
            goodsShipmentOrder.OperatorName = ShareClass.GetUserName(strUserCode);

            goodsShipmentOrder.CurrencyType = strCurrencyType;

            goodsShipmentOrderBLL.UpdateGoodsShipmentOrder(goodsShipmentOrder, int.Parse(strShipmentNO));

            LoadGoodsShipmentOrder(strUserCode, strRelatedType, strRelatedID);
            LoadGoodsShipmentDetail(strShipmentNO);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DL_InWareHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWareHouse;

        strWareHouse = DL_InWareHouse.SelectedValue.Trim();
        TB_ToPosition.Text = strWareHouse;

        ShareClass.LoadWareHousePositions(strWareHouse, DL_WHPosition);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = e.Item.Cells[3].Text;

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            string strShipmentNO = LB_ShipmentNO.Text;
            DateTime dtCheckOutTime, dtCurrentTime;
            strHQL = "From GoodsShipmentOrder as goodsShipmentOrder Where goodsShipmentOrder.ShipmentNO = " + strShipmentNO;
            GoodsShipmentOrderBLL goodsShipmentOrderBLL = new GoodsShipmentOrderBLL();
            lst = goodsShipmentOrderBLL.GetAllGoodsShipmentOrders(strHQL);
            GoodsShipmentOrder goodsShipmentOrder = (GoodsShipmentOrder)lst[0];

            dtCurrentTime = DateTime.Now;
            dtCheckOutTime = goodsShipmentOrder.ShipTime;
            TimeSpan ts = dtCurrentTime - dtCheckOutTime;

            if (e.CommandName == "Update")
            {
                strHQL = "From GoodsShipmentDetail as goodsShipmentDetail Where goodsShipmentDetail.ID = " + strID;
                GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
                lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);

                GoodsShipmentDetail goodsShipmentDetail = (GoodsShipmentDetail)lst[0];

                try
                {
                    DL_GoodsType.SelectedValue = goodsShipmentDetail.Type;
                }
                catch
                {
                }

                try
                {
                    DL_Unit.SelectedValue = goodsShipmentDetail.UnitName;
                }
                catch
                {
                }

                try
                {
                    DL_RecordSourceType.SelectedValue = goodsShipmentDetail.SourceType.Trim();
                }
                catch
                {
                }

               


                LB_ID.Text = strID;
            
                TB_GoodsCode.Text = goodsShipmentDetail.GoodsCode.Trim();
                TB_GoodsName.Text = goodsShipmentDetail.GoodsName.Trim();
                TB_ModelNumber.Text = goodsShipmentDetail.ModelNumber.Trim();
                TB_Spec.Text = goodsShipmentDetail.Spec.Trim();
                NB_Number.Amount = goodsShipmentDetail.Number;
                NB_Price.Amount = goodsShipmentDetail.Price;

                TB_Manufacturer.Text = goodsShipmentDetail.Manufacturer.Trim();
                TB_ToPosition.Text = goodsShipmentDetail.ToPosition.Trim();

                try
                {
                    ShareClass.LoadWareHousePositions(TB_ToPosition.Text.Trim(), DL_WHPosition);
                }
                catch
                {
                }

                try
                {
                    DL_WHPosition.SelectedValue = goodsShipmentDetail.WHPosition.Trim();
                }
                catch
                {
                }

                TB_FromPosition.Text = goodsShipmentDetail.FromPosition;
                TB_FromWHPosition.Text = goodsShipmentDetail.FromWHPosition.Trim();

                LB_FromGoodsID.Text = goodsShipmentDetail.FromGoodsID.ToString();
                TB_Comment.Text = goodsShipmentDetail.Comment.Trim();
                LB_SourceRelatedID.Text = goodsShipmentDetail.RelatedID.ToString();
                NB_RecordSourceID.Amount = goodsShipmentDetail.SourceID;

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
                string strFromGoodsID, strGoodsCode;
                string strSourceType, strSourceID;
                decimal deOldNumber;

                if (ts.Days > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuShiBaiZhiNengShanChuD")+"')", true);
                    return;
                }

                GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
                strHQL = "From GoodsShipmentDetail as goodsShipmentDetail Where goodsShipmentDetail.ID =" + strID;
                lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);
                GoodsShipmentDetail goodsShipmentDetail = (GoodsShipmentDetail)lst[0];

                strSourceType = goodsShipmentDetail.SourceType.Trim();
                strSourceID = goodsShipmentDetail.SourceID.ToString();

                strGoodsCode = goodsShipmentDetail.GoodsCode.Trim();

                strFromGoodsID = goodsShipmentDetail.FromGoodsID.ToString();
                deOldNumber = goodsShipmentDetail.Number;

                try
                {
                    goodsShipmentDetailBLL.DeleteGoodsShipmentDetail(goodsShipmentDetail);
                    LB_ID.Text = "";

                    //更改物料库存原位置数量
                    ShareClass.UpdateGoodsNumberForDelete(strFromGoodsID, deOldNumber);

                    LoadGoodsShipmentDetail(strShipmentNO);

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

    protected void DL_RecordSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_RecordSourceID.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strGoodsCode, strGoodsName, strType, strModelNumber, strSpec;
        string strWareHouse, strDepartString;

        TabContainer1.ActiveTabIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strType = DL_GoodsType.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strType = "%" + strType + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = LB_DepartString.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();

        strHQL = "Select * From T_Goods  Where GoodsCode Like " + "'" + strGoodsCode + "'" + " and GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and type Like " + "'" + strType + "'" + " and ModelNumber Like " + "'" + strModelNumber + "'" + " and Spec Like " + "'" + strSpec + "'";
        strHQL += " and Number > 0";
        strHQL += " and Position = " + "'" + strWareHouse + "'";
        strHQL += " and Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

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
                    DL_GoodsType.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_GoodsType.SelectedValue = "";
                }

                DL_Unit.SelectedValue = item.Unit;
                TB_ModelNumber.Text = item.ModelNumber.Trim();
                TB_Spec.Text = item.Specification;
                TB_Manufacturer.Text = item.Brand;

                NB_WarrantyPeriod.Amount = item.WarrantyPeriod;

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

            try
            {
                DL_GoodsType.SelectedValue = goods.Type;
            }
            catch
            {
                DL_GoodsType.SelectedValue = "";
            }


            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();

            DL_Unit.SelectedValue = goods.UnitName;
            TB_Manufacturer.Text = goods.Manufacturer.Trim();

            TB_SN.Text = goods.SN;
            NB_WarrantyPeriod.Amount = goods.WarrantyPeriod;

            TB_FromPosition.Text = goods.Position.Trim();
            TB_FromWHPosition.Text = goods.WHPosition.Trim();

            LB_FromGoodsID.Text = goods.ID.ToString();

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

            strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.ID = " + strID;
            GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
            lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);

            if (lst.Count > 0)
            {
                try
                {
                    GoodsApplicationDetail goodsApplicationDetail = (GoodsApplicationDetail)lst[0];

                    TB_GoodsCode.Text = goodsApplicationDetail.GoodsCode;
                    TB_GoodsName.Text = goodsApplicationDetail.GoodsName.Trim();

                    try
                    {
                        DL_GoodsType.SelectedValue = goodsApplicationDetail.Type;
                    }
                    catch
                    {
                        DL_GoodsType.SelectedValue = "";
                    }


                    TB_ModelNumber.Text = goodsApplicationDetail.ModelNumber.Trim();
                    TB_Spec.Text = goodsApplicationDetail.Spec.Trim();
                    TB_Manufacturer.Text = goodsApplicationDetail.Brand;

                    NB_Number.Amount = goodsApplicationDetail.Number - goodsApplicationDetail.CheckOutNumber;
                    DL_Unit.SelectedValue = goodsApplicationDetail.Unit;

                    LB_SourceRelatedID.Text = goodsApplicationDetail.AAID.ToString();
                    DL_RecordSourceType.SelectedValue = "GoodsAORecord";
                    NB_RecordSourceID.Amount = goodsApplicationDetail.AAID;
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

                    try
                    {
                        DL_GoodsType.SelectedValue = goodsSaleRecord.Type;
                    }
                    catch
                    {
                        DL_GoodsType.SelectedValue = "";
                    }

                    TB_ModelNumber.Text = goodsSaleRecord.ModelNumber.Trim();
                    TB_Spec.Text = goodsSaleRecord.Spec.Trim();
                    TB_Manufacturer.Text = goodsSaleRecord.Brand;

                    NB_Number.Amount = goodsSaleRecord.Number - goodsSaleRecord.CheckOutNumber;
                    NB_Price.Amount = goodsSaleRecord.Price;
                    DL_Unit.SelectedValue = goodsSaleRecord.Unit;

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

    protected void BT_TransferFromApplicationForm_Click(object sender, EventArgs e)
    {
        string strHQL0, strHQL1, strHQL2, strHQL3, strHQL4;
        string strADID, strAOID, strShipmentNO, strApplicationGoodsCode, strGoodsCode, strGoodsType, strGoodsID, strApplicantName, strCurrencyType;
        string strDepartString;
        decimal deEveryGoodsNumber, deApplicationNumber, deGoodsNumber, deGoodsPrice;
        int i, j, k = 0;

        DataSet ds0, ds1, ds2, ds3;

        strDepartString = LB_DepartString.Text.Trim();

        strAOID = LB_AOID.Text.Trim();
        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strApplicantName = ShareClass.GetApplicantNameFromGoodsApplicaitonOrder(strAOID);

        strHQL0 = " Select GoodsCode,Number From T_GoodsShipmentDetail ";
        strHQL0 += " Where ShipmentNO = " + strShipmentNO;
        ds0 = ShareClass.GetDataSetFromSql(strHQL0, "T_GoodsShipmentDetail");
        if (ds0.Tables[0].Rows.Count == 0)
        {
            strHQL1 = "Select GoodsCode,Number,CheckOutNumber From T_GoodsApplicationDetail Where AAID = " + strAOID;
            strHQL1 += " Order By ID ASC";
            ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_GoodsApplicationDetail");
            for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                strApplicationGoodsCode = ds1.Tables[0].Rows[i][0].ToString();
                deApplicationNumber = decimal.Parse(ds1.Tables[0].Rows[i][1].ToString()) - decimal.Parse(ds1.Tables[0].Rows[i][2].ToString());

                strHQL2 = "Select A.GoodsCode,sum(A.Number) From T_Goods A,T_GoodsApplicationDetail B ";
                strHQL2 += " Where A.GoodsCode = B.GoodsCode and A.Spec = B.Spec and A.ModelNumber = B.ModelNumber ";
                strHQL2 += " and A.GoodsCode = " + "'" + strApplicationGoodsCode + "'" + " and A.Number > 0 ";
                strHQL2 += " and A.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
                strHQL2 += " Group By A.GoodsCode";
                ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_Goods");

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strGoodsCode = ds2.Tables[0].Rows[0][0].ToString();
                    deGoodsNumber = decimal.Parse(ds2.Tables[0].Rows[0][1].ToString());

                    if (deApplicationNumber <= deGoodsNumber)
                    {
                        strHQL3 = " Select A.ID, A.GoodsCode,A.Number,A.Price,B.ID,B.AAID,B.CurrencyType,A.Type From T_Goods A,T_GoodsApplicationDetail B ";
                        strHQL3 += " Where A.GoodsCode = B.GoodsCode and A.Spec = B.Spec and A.ModelNumber = B.ModelNumber ";
                        strHQL3 += " and A.GoodsCode = " + "'" + strApplicationGoodsCode + "'" + " and A.Number > 0 ";
                        strHQL3 += " and A.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
                        strHQL3 += " Order By A.Number ASC";
                        ds3 = ShareClass.GetDataSetFromSql(strHQL3, "T_Goods");

                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            for (j = 0; j < ds3.Tables[0].Rows.Count; j++)
                            {
                                strGoodsID = ds3.Tables[0].Rows[j][0].ToString();
                                deEveryGoodsNumber = decimal.Parse(ds3.Tables[0].Rows[j][2].ToString());
                                deGoodsPrice = decimal.Parse(ds3.Tables[0].Rows[j][3].ToString());
                                strADID = ds3.Tables[0].Rows[j][4].ToString();
                                strAOID = ds3.Tables[0].Rows[j][5].ToString();
                                strCurrencyType = ds3.Tables[0].Rows[j][6].ToString();
                                strGoodsType = ds3.Tables[0].Rows[j][7].ToString();

                                if (deApplicationNumber > 0)
                                {
                                    if (deApplicationNumber <= deEveryGoodsNumber)
                                    {
                                        strHQL4 = "Insert Into T_GoodsShipmentDetail(ShipmentNO,GoodsCode,GoodsName,Spec,Number,AleadyOutNumber,Price,UnitName,FromPosition,FromGoodsID,ToPosition,Comment,ModelNumber,Manufacturer,Type)";
                                        strHQL4 += " Select " + strShipmentNO + ",GoodsCode,GoodsName,Spec," + deApplicationNumber.ToString() + "," + deApplicationNumber.ToString() + ",Price,UnitName,Position,ID,'','',ModelNumber,Manufacturer,Type From T_Goods";
                                        strHQL4 += " Where ID = " + strGoodsID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        //插入应收应付数据到应收应付表
                                        //ShareClass.InsertReceivablesOrPayable("GoodsSHO", "GoodsAO", strAOID, strADID, deApplicationNumber * deGoodsPrice, strCurrencyType, strApplicantName, strUserCode);


                                        strHQL4 = "Update T_Goods Set Number = " + (deEveryGoodsNumber - deApplicationNumber).ToString() + " From T_Goods  ";
                                        strHQL4 += " Where ID = " + strGoodsID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        deApplicationNumber = 0;

                                        k = 1;
                                    }
                                    else
                                    {
                                        strHQL4 = "Insert Into T_GoodsShipmentDetail(ShipmentNO,GoodsCode,GoodsName,Spec,Number,AleadyOutNumber,Price,UnitName,FromPosition,FromGoodsID,ToPosition,Comment,ModelNumber,Manufacturer,Type)";
                                        strHQL4 += " Select " + strShipmentNO + ",GoodsCode,GoodsName,Spec," + deApplicationNumber.ToString() + "," + deApplicationNumber.ToString() + ",Price,UnitName,Position,ID,'','',ModelNumber,Manufacturer,Type From T_Goods";
                                        strHQL4 += " Where ID = " + strGoodsID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        //插入应收应付数据到应收应付表
                                        //ShareClass.InsertReceivablesOrPayable("GoodsSHO", "GoodsAO", strAOID, strADID, deEveryGoodsNumber * deGoodsPrice, strCurrencyType, strApplicantName, strUserCode);

                                        strHQL4 = "Update T_Goods Set Number = 0 " + " From T_Goods  ";
                                        strHQL4 += " Where ID = " + strGoodsID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        deApplicationNumber -= deEveryGoodsNumber;

                                        k = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (k == 1)
            {
                LoadGoodsShipmentDetail(strShipmentNO);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJZCCKDCG") + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYFHTJDJLJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCCKDYYJLBNZLJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void BT_TransferFromSaleOrderForm_Click(object sender, EventArgs e)
    {
        string strHQL0, strHQL1, strHQL2, strHQL3, strHQL4;
        string strSOID, strSDID, strCustomerName, strShipmentNO, strSaleGoodsCode, strGoodsCode, strGoodsID, strCurrencyType, strGoodstype;
        string strDepartString;
        decimal deEveryGoodsNumber, deSaleNumber, deGoodsNumber, deGoodsSalePrice;
        int i, j, k = 0;

        DataSet ds0, ds1, ds2, ds3;

        strDepartString = LB_DepartString.Text.Trim();

        strSOID = LB_SOID.Text.Trim();
        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strCustomerName = ShareClass.GetCustomerNameFromGoodsSaleOrder(strSOID);

        strHQL0 = " Select GoodsCode,Number From T_GoodsShipmentDetail ";
        strHQL0 += " Where ShipmentNO = " + strShipmentNO;
        ds0 = ShareClass.GetDataSetFromSql(strHQL0, "T_GoodsShipmentDetail");
        if (ds0.Tables[0].Rows.Count == 0)
        {
            strHQL1 = "Select GoodsCode,Number,CheckOutNumber From T_GoodsSaleRecord Where SOID = " + strSOID;
            strHQL1 += " Order By ID ASC";
            ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_GoodsSaleRecord");
            for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                strSaleGoodsCode = ds1.Tables[0].Rows[i][0].ToString();
                deSaleNumber = decimal.Parse(ds1.Tables[0].Rows[i][1].ToString()) - decimal.Parse(ds1.Tables[0].Rows[i][2].ToString());

                strHQL2 = "Select A.GoodsCode,sum(A.Number) From T_Goods A,T_GoodsSaleRecord B ";
                strHQL2 += " Where A.GoodsCode = B.GoodsCode and A.Spec = B.Spec and A.ModelNumber = B.ModelNumber ";
                strHQL2 += " and A.GoodsCode = " + "'" + strSaleGoodsCode + "'" + " and A.Number > 0 ";
                strHQL2 += " and A.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
                strHQL2 += " Group By A.GoodsCode";
                ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_Goods");

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strGoodsCode = ds2.Tables[0].Rows[0][0].ToString();
                    deGoodsNumber = decimal.Parse(ds2.Tables[0].Rows[0][1].ToString());

                    if (deSaleNumber <= deGoodsNumber)
                    {
                        strHQL3 = " Select A.ID, A.GoodsCode,A.Number,B.SOID,B.ID,B.Price,B.CurrencyType,A.Type From T_Goods A,T_GoodsSaleRecord B ";
                        strHQL3 += " Where A.GoodsCode = B.GoodsCode and A.Spec = B.Spec and A.ModelNumber = B.ModelNumber ";
                        strHQL3 += " and A.GoodsCode = " + "'" + strSaleGoodsCode + "'" + " and A.Number > 0 ";
                        strHQL3 += " and A.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
                        strHQL3 += " Order By A.Number ASC";
                        ds3 = ShareClass.GetDataSetFromSql(strHQL3, "T_Goods");

                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            for (j = 0; j < ds3.Tables[0].Rows.Count; j++)
                            {
                                strGoodsID = ds3.Tables[0].Rows[j][0].ToString();
                                deEveryGoodsNumber = decimal.Parse(ds3.Tables[0].Rows[j][2].ToString());
                                strSOID = ds3.Tables[0].Rows[j][3].ToString();
                                strSDID = ds3.Tables[0].Rows[j][4].ToString();
                                deGoodsSalePrice = decimal.Parse(ds3.Tables[0].Rows[j][5].ToString());
                                strCurrencyType = ds3.Tables[0].Rows[j][6].ToString();
                                strGoodstype = ds3.Tables[0].Rows[j][7].ToString();


                                if (deSaleNumber > 0)
                                {
                                    if (deSaleNumber <= deEveryGoodsNumber)
                                    {
                                        strHQL4 = "Insert Into T_GoodsShipmentDetail(ShipmentNO,GoodsCode,GoodsName,Spec,Number,AleadyOutNumber,Price,UnitName,FromPosition,FromGoodsID,ToPosition,Comment,ModelNumber,Manufacturer,Type)";
                                        strHQL4 += " Select " + strShipmentNO + ",A.GoodsCode,A.GoodsName,A.Spec," + deSaleNumber.ToString() + "," + deSaleNumber.ToString() + ",B.Price,A.UnitName,A.Position,A.ID,'','',A.ModelNumber,A.Manufacturer,A.Type From T_Goods A,T_GoodsSaleRecord B";
                                        strHQL4 += " Where A.ID = " + strGoodsID + " AND A.GoodsCode = B.GoodsCode";
                                        ShareClass.RunSqlCommand(strHQL4);

                                        //插入应收应付数据到应收应付表
                                        //ShareClass.InsertReceivablesOrPayable("GoodsSHO", "GoodsSO", strSOID, strSDID, deSaleNumber * deGoodsSalePrice, strCurrencyType, strCustomerName, strUserCode);

                                        strHQL4 = "Update T_Goods Set Number = " + (deEveryGoodsNumber - deSaleNumber).ToString() + " From T_Goods  ";
                                        strHQL4 += " Where ID = " + strGoodsID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        deSaleNumber = 0;

                                        k = 1;
                                    }
                                    else
                                    {
                                        strHQL4 = "Insert Into T_GoodsShipmentDetail(ShipmentNO,GoodsCode,GoodsName,Spec,Number,AleadyOutNumber,Price,UnitName,FromPosition,FromGoodsID,ToPosition,Comment,ModelNumber,Manufacturer,Type)";
                                        strHQL4 += " Select " + strShipmentNO + ",A.GoodsCode,A.GoodsName,A.Spec," + deEveryGoodsNumber.ToString() + "," + deEveryGoodsNumber.ToString() + ",B.Price,A.UnitName,A.Position,A.ID,'','',A.ModelNumber,A.Manufacturer,A.Type From T_Goods A,T_GoodsSaleRecord B";
                                        strHQL4 += " Where A.ID = " + strGoodsID + " AND A.GoodsCode = B.GoodsCode";
                                        ShareClass.RunSqlCommand(strHQL4);

                                        //插入应收应付数据到应收应付表
                                        //ShareClass.InsertReceivablesOrPayable("GoodsSHO", "GoodsSO", strSOID, strSDID, deEveryGoodsNumber * deGoodsSalePrice, strCurrencyType, strCustomerName, strUserCode);

                                        strHQL4 = "Update T_Goods Set Number = 0 " + " From T_Goods  ";
                                        strHQL4 += " Where ID = " + strGoodsID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        deSaleNumber -= deEveryGoodsNumber;

                                        k = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (k == 1)
            {
                LoadGoodsShipmentDetail(strShipmentNO);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJZCCKDCG") + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBMYFHTJDJLJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCCKDYYJLBNZLJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }


    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ShareClass.LoadWareHouseListByAuthorityForDropDownList(strUserCode, DL_InWareHouse);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }


    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strShipmentNO;

        strShipmentNO = LB_ShipmentNO.Text.Trim();

        if (strShipmentNO == "")
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
        string strShipmentNO, strGoodsCode, strGoodsName, strGoodsType, strSN, strModelNumber, strSpec, strUnitName, strManufacturer, strFromPosition, strToPosition, strComment;
        string strSourceType, strSourceID, strSourceRelatedID;
        decimal deNumber, dePrice;
        string strFromGoodsID, strWareHouse;
        int intWarrantyPeriod;

        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        strFromGoodsID = LB_FromGoodsID.Text;
        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strGoodsType = DL_GoodsType.SelectedValue.Trim();
        strSN = TB_SN.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        dePrice = NB_Price.Amount;

        strUnitName = DL_Unit.SelectedValue.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());
        strFromPosition = TB_FromPosition.Text.Trim();
        strToPosition = TB_ToPosition.Text.Trim();
        strComment = TB_Comment.Text.Trim();

        if (strRelatedType == "Constract")
        {
            if (strRelatedGoodsCode == "" | strRelatedGoodsCode != strGoodsCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click11", "alert('" + LanguageHandle.GetWord("ZZCWZHTCHJHGDDLPJC") + "')", true);
                return;
            }
        }

        if (strFromGoodsID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZCWZYCKDZCJC") + "')", true);
            return;
        }

        if (CheckShipNumber(strFromGoodsID, deNumber) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZCWCKSLBNDYCLPKCSLJC") + "')", true);
            return;
        }

        strSourceRelatedID = LB_SourceRelatedID.Text.Trim();
        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strSourceID = NB_RecordSourceID.Amount.ToString();

        if (strFromGoodsID == "" | strGoodsCode == "" | strGoodsName == "" | strSpec == "" | strFromPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click44", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
            GoodsShipmentDetail goodsShipmentDetail = new GoodsShipmentDetail();

            goodsShipmentDetail.ShipmentNO = int.Parse(strShipmentNO);
            goodsShipmentDetail.GoodsCode = strGoodsCode;
            goodsShipmentDetail.GoodsName = strGoodsName;
            goodsShipmentDetail.Type = strGoodsType;
            if (strSN == "")
            {
                //strSN = strGoodsCode;
            }
            goodsShipmentDetail.SN = strSN;
            goodsShipmentDetail.ModelNumber = strModelNumber;
            goodsShipmentDetail.Manufacturer = strManufacturer;
            goodsShipmentDetail.Spec = strSpec;
            goodsShipmentDetail.Number = deNumber;
            goodsShipmentDetail.AleadyOutNumber = deNumber;
            goodsShipmentDetail.DeliveryNumber = 0;
            goodsShipmentDetail.Price = dePrice;
            goodsShipmentDetail.Amount = deNumber * dePrice;
            goodsShipmentDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            goodsShipmentDetail.WarrantyPeriod = intWarrantyPeriod;

            goodsShipmentDetail.UnitName = strUnitName;
            goodsShipmentDetail.FromPosition = strFromPosition;
            goodsShipmentDetail.FromWHPosition = TB_FromWHPosition.Text.Trim();

            goodsShipmentDetail.FromGoodsID = int.Parse(strFromGoodsID);
            goodsShipmentDetail.ToPosition = strToPosition;
            goodsShipmentDetail.WHPosition = DL_WHPosition.SelectedValue.Trim();

            goodsShipmentDetail.Comment = strComment;

            if (strSourceRelatedID == "")
            {
                goodsShipmentDetail.RelatedID = 0;
                strSourceRelatedID = "0";
            }
            else
            {
                goodsShipmentDetail.RelatedID = int.Parse(strSourceRelatedID);
            }

            if (strSourceType == "")
            {
                goodsShipmentDetail.SourceType = "Other";
            }
            else
            {
                goodsShipmentDetail.SourceType = strSourceType;
            }
            if (strSourceID == "")
            {
                goodsShipmentDetail.SourceID = 0;
            }
            else
            {
                goodsShipmentDetail.SourceID = int.Parse(strSourceID);
            }

            try
            {
                goodsShipmentDetailBLL.AddGoodsShipmentDetail(goodsShipmentDetail);

                LB_ID.Text = ShareClass.GetMyCreatedMaxGoodsShipmentDetailID().ToString();

                //更新对应物料记录数量
                ShareClass.UpdateGoodsNumberForAdd(strFromGoodsID, deNumber);

                //更新对应销售单或申请单数量
                UpdateGoodsSOOrAONumber(strSourceType, strSourceID);

                ////插入应收应付数据到应收应付表
                //string strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
                //if (strSourceType == "GoodsSORecord")
                //{
                //    string strCustomerName = ShareClass.GetCustomerNameFromGoodsSaleOrder(strSourceRelatedID);

                //    ShareClass.InsertReceivablesOrPayable("GoodsSHO", "GoodsSO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strCustomerName, strUserCode);
                //}
                //if (strSourceType == "GoodsAORecord")
                //{
                //    string strApplicantName = ShareClass.GetApplicantNameFromGoodsApplicaitonOrder(strSourceRelatedID);

                //    ShareClass.InsertReceivablesOrPayable("GoodsSHO", "GoodsAO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strApplicantName, strUserCode);
                //}

                LoadGoodsShipmentDetail(strShipmentNO);
                LoadGoods(strGoodsCode, strWareHouse);

                if (strSourceType == "GoodsAORecord")
                {
                    LoadGoodsApplicationDetail(LB_AOID.Text.Trim());
                }
                if (strSourceType == "GoodsSORecord")
                {
                    LoadGoodsSaleOrderDetail(LB_SOID.Text.Trim());
                }

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                //更新合同出货计划的数量
                if (strRelatedType == "Constract")
                {
                    CountGoodsDeliveryNumber(strRelatedType, strRelatedID);
                }


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click55", "alert('" + LanguageHandle.GetWord("ZZXJCCJC") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdateDetail()
    {
        string strShipmentNO, strGoodsCode, strGoodsName, strGoodsType, strSN, strModelNumber, strSpec, strUnitName, strManufacturer, strFromPosition, strToPosition, strComment;
        string strSourceType, strSourceID, strSourceRelatedID;
        decimal deNumber, deOldNumber, dePrice;
        int intID, intWarrantyPeriod;
        string strFromGoodsID, strWareHouse;
        string strHQL;

        strFromGoodsID = LB_FromGoodsID.Text.Trim();

        intID = int.Parse(LB_ID.Text);
        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strGoodsType = DL_GoodsType.SelectedValue.Trim();
        strSN = TB_SN.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        dePrice = NB_Price.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        strFromPosition = TB_FromPosition.Text.Trim();
        strToPosition = TB_ToPosition.Text.Trim();
        strComment = TB_Comment.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();

        strSourceRelatedID = LB_SourceRelatedID.Text.Trim();
        strSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strSourceID = NB_RecordSourceID.Amount.ToString();

        if (strRelatedType == "Constract")
        {
            if (strRelatedGoodsCode == "" | strRelatedGoodsCode != strGoodsCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWZHTCHJHGDDLPJC") + "')", true);
                return;
            }
        }

        if (strFromGoodsID == "" | strGoodsCode == "" | strGoodsName == "" | strSpec == "" | strFromPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
            strHQL = "From GoodsShipmentDetail as goodsShipmentDetail Where goodsShipmentDetail.ID = " + intID.ToString();
            IList lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);
            GoodsShipmentDetail goodsShipmentDetail = (GoodsShipmentDetail)lst[0];

            deOldNumber = goodsShipmentDetail.Number;

            goodsShipmentDetail.ShipmentNO = int.Parse(strShipmentNO);
            goodsShipmentDetail.GoodsCode = strGoodsCode;
            goodsShipmentDetail.GoodsName = strGoodsName;
            goodsShipmentDetail.Type = strGoodsType;
            if (strSN == "")
            {
                //strSN = strGoodsCode;
            }
            goodsShipmentDetail.SN = strSN;

            goodsShipmentDetail.ModelNumber = strModelNumber;
            goodsShipmentDetail.Spec = strSpec;
            goodsShipmentDetail.Number = deNumber;
            goodsShipmentDetail.AleadyOutNumber = deNumber;
            goodsShipmentDetail.Price = dePrice;
            goodsShipmentDetail.Amount = deNumber * dePrice;
            goodsShipmentDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            goodsShipmentDetail.UnitName = strUnitName;
            goodsShipmentDetail.Manufacturer = strManufacturer;
            goodsShipmentDetail.WarrantyPeriod = intWarrantyPeriod;

            goodsShipmentDetail.FromPosition = strFromPosition;
            goodsShipmentDetail.FromWHPosition = TB_FromWHPosition.Text.Trim();

            goodsShipmentDetail.FromGoodsID = int.Parse(strFromGoodsID);
            goodsShipmentDetail.ToPosition = strToPosition;
            goodsShipmentDetail.WHPosition = DL_WHPosition.SelectedValue.Trim();

            goodsShipmentDetail.Comment = strComment;

            if (strSourceRelatedID == "")
            {
                goodsShipmentDetail.RelatedID = 0;
                strSourceRelatedID = "0";
            }
            else
            {
                goodsShipmentDetail.RelatedID = int.Parse(strSourceRelatedID);
            }

            if (strSourceType == "")
            {
                goodsShipmentDetail.SourceType = "Other";
            }
            else
            {
                goodsShipmentDetail.SourceType = strSourceType;
            }
            if (strSourceID == "")
            {
                goodsShipmentDetail.SourceID = 0;
            }
            else
            {
                goodsShipmentDetail.SourceID = int.Parse(strSourceID);
            }

            try
            {
                goodsShipmentDetailBLL.UpdateGoodsShipmentDetail(goodsShipmentDetail, intID);

                //更改物料原位置库存数量
                ShareClass.UpdateGoodsNumberForUpdate(strFromGoodsID, deNumber, deOldNumber);

                //更新对应销售单或申请单数量
                UpdateGoodsSOOrAONumber(strSourceType, strSourceID);

                ////更新应收应付数据到应收应付表
                //string strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
                //if (strSourceType == "GoodsSORecord")
                //{
                //    string strCustomerName = ShareClass.GetCustomerNameFromGoodsSaleOrder(strSourceRelatedID);
                //    ShareClass.UpdateReceivablesOrPayable("GoodsSHO", "GoodsSO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strCustomerName, strUserCode);
                //}
                //if (strSourceType == "GoodsAORecord")
                //{
                //    string strApplicantName = ShareClass.GetApplicantNameFromGoodsApplicaitonOrder(strSourceRelatedID);
                //    ShareClass.UpdateReceivablesOrPayable("GoodsSHO", "GoodsAO", strSourceRelatedID, strSourceID, deNumber * dePrice, strCurrencyType, strApplicantName, strUserCode);
                //}

                if (strSourceType == "GoodsAORecord")
                {
                    LoadGoodsApplicationDetail(LB_AOID.Text.Trim());
                }
                if (strSourceType == "GoodsSORecord")
                {
                    LoadGoodsSaleOrderDetail(LB_SOID.Text.Trim());
                }

                LoadGoodsShipmentDetail(strShipmentNO);
                LoadGoods(strGoodsCode, strWareHouse);

                //更新合同出货计划的数量
                if (strRelatedType == "Constract")
                {
                    CountGoodsDeliveryNumber(strRelatedType, strRelatedID);
                }


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCJC") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdateGoodsSOOrAONumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber;

        if (strSourceType == "GoodsSORecord")
        {
            strHQL = "Select  COALESCE(Sum(Number),0) From T_GoodsShipmentDetail Where SourceType = 'GoodsSORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentDetail");


            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_GoodsSaleRecord Set CheckOutNumber = " + deSumNumber.ToString() + " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strSourceType == "GoodsAORecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsShipmentDetail Where SourceType = 'GoodsAORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentDetail");

            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_GoodsApplicationDetail Set CheckOutNumber = " + deSumNumber.ToString() + " Where AAID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
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

    protected void LoadGoodsShipmentOrder(string strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsShipmentOrder as goodsShipmentOrder Where goodsShipmentOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " And goodsShipmentOrder.RelatedType=" + "'" + strRelatedType + "'" + " and goodsShipmentOrder.RelatedID = " + strRelatedID;
        strHQL += " and ShipmentType <> 'Transfer'";
        strHQL += " Order By goodsShipmentOrder.ShipmentNO DESC";
        GoodsShipmentOrderBLL goodsShipmentOrderBLL = new GoodsShipmentOrderBLL();
        lst = goodsShipmentOrderBLL.GetAllGoodsShipmentOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }


    protected void LoadGoodsShipmentOrder(string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_GoodsShipmentOrder Where (OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " or OperatorCode in (select UnderCode from T_MemberLevel where UserCode = " + "'" + strUserCode + "'" + ")) ";
        strHQL += " and ShipmentType <> 'Transfer'";
        strHQL += " Order By ShipmentNO DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentOrder");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadGoodsShipmentDetail(string strShipmentNO)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsShipmentDetail as goodsShipmentDetail Where goodsShipmentDetail.ShipmentNO = " + strShipmentNO + " Order By goodsShipmentDetail.ID ASC";
        GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
        lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    protected void LoadGoods(string strGoodsCode, string strWareHouse)
    {
        string strHQL;
        IList lst;

        strHQL = "From Goods as goods Where goods.Number > 0 and goods.GoodsCode = " + "'" + strGoodsCode + "'";
        strHQL += " and goods.Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by goods.ID ASC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        TB_GoodsName.Focus();

        LB_Sql2.Text = strHQL;
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

    protected void LoadWareHouseListByAuthority(string strDepartString)
    {
        string strHQL;


        strHQL = " Select WHName From T_WareHouse Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By SortNumber DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse");

        DL_WareHouse.DataSource = ds;
        DL_WareHouse.DataBind();
    }

    protected void LoadGoodsApplication(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from GoodsApplication as goodsApplication where ";
        strHQL += " (goodsApplication.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " or goodsApplication.ApplicantCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ")) ";
        strHQL += " Order by goodsApplication.AAID DESC";
        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

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

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where ( goodsSaleOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " or goodsSaleOrder.OperatorCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ") ";
        strHQL += " or goodsSaleOrder.OperatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " or goodsSaleOrder.SalesCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " Order by goodsSaleOrder.SOID DESC";
        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();

        LB_Sql8.Text = strHQL;
    }

    protected void LoadGoodsApplicationDetail(string strAAID)
    {
        string strHQL;
        IList lst;

        if (strAAID != "")
        {
            strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.AAID = " + strAAID;
            GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
            lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();

            LB_Sql4.Text = strHQL;
        }
    }

    protected void LoadGoodsSaleOrderDetail(string strSOID)
    {
        LB_GoodsOwner.Text = LanguageHandle.GetWord("XiaoShouDan") + ": " + strSOID + LanguageHandle.GetWord("MingXi");

        if (strSOID != "")
        {
            string strHQL = "Select * from T_GoodsSaleRecord where SOID = " + strSOID + " Order by ID DESC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

            DataGrid7.DataSource = ds;
            DataGrid7.DataBind();

            LB_Sql7.Text = strHQL;
        }
    }

    protected void CountGoodsDeliveryNumber(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        decimal deDeliveryNumber = 0;

        strHQL = "from GoodsShipmentDetail as goodsShipmentDetail where goodsShipmentDetail.ShipmentNO in ";
        strHQL += " (Select goodsShipmentOrder.ShipmentNO From GoodsShipmentOrder as goodsShipmentOrder Where goodsShipmentOrder.RelatedType = " + "'" + strRelatedType + "'" + " and goodsShipmentOrder.RelatedID = " + strRelatedID + ")";
        strHQL += " Order by goodsShipmentDetail.Number DESC,goodsShipmentDetail.ID DESC";
        GoodsShipmentDetailBLL goodsShipmentDetailBLL = new GoodsShipmentDetailBLL();
        lst = goodsShipmentDetailBLL.GetAllGoodsShipmentDetails(strHQL);

        GoodsShipmentDetail goodsShipmentDetail = new GoodsShipmentDetail();

        for (int i = 0; i < lst.Count; i++)
        {
            goodsShipmentDetail = (GoodsShipmentDetail)lst[i];

            deDeliveryNumber += goodsShipmentDetail.Number;
        }

        strHQL = "Update T_ConstractGoodsDeliveryPlan Set DeliveredNumber = " + deDeliveryNumber.ToString() + " where ID =" + strRelatedID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ConstractGoodsDeliveryPlan Set UNDeliveredNumber = " + (deDeliveablesNumber - deDeliveryNumber).ToString() + " where ID =" + strRelatedID;
        ShareClass.RunSqlCommand(strHQL);
    }

}
