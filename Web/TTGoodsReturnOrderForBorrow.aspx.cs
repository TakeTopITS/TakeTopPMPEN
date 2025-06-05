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


public partial class TTGoodsReturnOrderForBorrow : System.Web.UI.Page
{
    string strUserCode;

    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        //从流程中打开的业务单
        strToDoWLID = Request.QueryString["WLID"]; strToDoWLDetailID= Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ReturnTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

            ShareClass.LoadCustomer(DL_Customer, strUserCode);

            LoadGoodsReturnOrder(strUserCode, "BORROW");

            LoadGoodsBorrowOrder(strUserCode);

            ShareClass.LoadWFTemplate(strUserCode, "BorrowingReturn", DL_TemName);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strROID;
            strROID = e.Item.Cells[3].Text.Trim();
            LB_ROID.Text = strROID;

            int intWLNumber = intWLNumber = ShareClass.GetRelatedWorkFlowNumber("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
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

                strHQL = "From GoodsReturnOrder as goodsReturnOrder Where goodsReturnOrder.ROID = " + strROID;
                GoodsReturnOrderBLL goodsReturnOrderBLL = new GoodsReturnOrderBLL();
                lst = goodsReturnOrderBLL.GetAllGoodsReturnOrders(strHQL);
                GoodsReturnOrder goodsReturnOrder = (GoodsReturnOrder)lst[0];

                TB_ReturnName.Text = goodsReturnOrder.ReturnName.Trim();
                DLC_ReturnTime.Text = goodsReturnOrder.ReturnTime.ToString("yyyy-MM-dd");
                TB_Applicant.Text = goodsReturnOrder.Applicant.Trim();

                try
                {
                    DL_Customer.SelectedValue = goodsReturnOrder.CustomerCode;
                }
                catch
                {
                }
                DL_ReturnOrderStatus.SelectedValue = goodsReturnOrder.Status.Trim();

                LoadGoodsReturnDetail(strROID);

               
                TB_WLName.Text = LanguageHandle.GetWord("JieChuGuiHuan") + goodsReturnOrder.ReturnName.Trim() + LanguageHandle.GetWord("ShenQing");
                ShareClass.LoadRelatedWL("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), goodsReturnOrder.ROID, DataGrid8);

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
                intWLNumber = intWLNumber = ShareClass.GetRelatedWorkFlowNumber("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
                if (intWLNumber > 0)
                {
                    return;
                }
                
                try
                {
                    strHQL = "Delete From T_GoodsReturnOrder Where ROID = " + strROID;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_GoodsReturnDetail Where ROID = " + strROID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadGoodsReturnOrder(strUserCode, "BORROW");
                    LoadGoodsReturnDetail(strROID);


                    LB_ROID.Text = "0";
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCKNCZMXJLJC") + "')", true);
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

        if (strSourceType == "GoodsBO")
        {
            BT_SelectBO.Visible = true;
            NB_SourceID.Amount = 0;
        }
        else
        {
            BT_SelectBO.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

        GoodsReturnOrderBLL goodsReturnOrderBLL = new GoodsReturnOrderBLL();
        IList lst = goodsReturnOrderBLL.GetAllGoodsReturnOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }


    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_ROID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        string strNewROCode = ShareClass.GetCodeByRule("ReturnOrderCode", "ReturnOrderCode", "00");
        if (strNewROCode != "")
        {
            TB_ReturnName.Text = strNewROCode;
        }

        LoadGoodsReturnDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strROID;

        strROID = LB_ROID.Text.Trim();

        if (strROID == "")
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
        string strROID, strType, strReturnName, strApplicant, strReturnReason, strStatus;
        string strCustomerCode, strCustomerName, strCurrencyType;
        decimal deAmount;

        DateTime dtReturnTime;

        strType = DL_FormType.SelectedValue.Trim();
        strReturnName = TB_ReturnName.Text.Trim();

        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strReturnReason = TB_ReturnReason.Text.Trim();
        strStatus = DL_ReturnOrderStatus.SelectedValue.Trim();

        strCustomerCode = DL_Customer.SelectedValue.Trim();

        if (strCustomerCode != "")
        {
            strCustomerName = DL_Customer.SelectedItem.Text;
        }
        else
        {
            strCustomerName = "";
        }

        strCurrencyType = "";
        deAmount = 0;


        try
        {
            GoodsReturnOrderBLL goodsReturnOrderBLL = new GoodsReturnOrderBLL();
            GoodsReturnOrder goodsReturnOrder = new GoodsReturnOrder();

            goodsReturnOrder.Type = strType;
            goodsReturnOrder.ReturnName = strReturnName;
            goodsReturnOrder.Applicant = strApplicant;
            goodsReturnOrder.ReturnTime = dtReturnTime;
            goodsReturnOrder.Amount = 0;
            goodsReturnOrder.CurrencyType = strCurrencyType;
            goodsReturnOrder.CustomerCode = strCustomerCode;
            goodsReturnOrder.CustomerName = strCustomerName;
            goodsReturnOrder.OperatorCode = strUserCode;
            goodsReturnOrder.OperatorName = ShareClass.GetUserName(strUserCode);
            goodsReturnOrder.Status = strStatus;

            goodsReturnOrderBLL.AddGoodsReturnOrder(goodsReturnOrder);

            strROID = ShareClass.GetMyCreatedMaxGoodsROID(strUserCode).ToString();

            LB_ROID.Text = strROID;

            string strNewROCode = ShareClass.GetCodeByRule("ReturnOrderCode", "ReturnOrderCode", strROID);
            if (strNewROCode != "")
            {
                TB_ReturnName.Text = strNewROCode;
                string strHQL = "Update T_GoodsReturnOrder Set ReturnName = " + "'" + strNewROCode + "'" + " Where ROID = " + strROID;
                ShareClass.RunSqlCommand(strHQL);
            }


            LoadGoodsReturnOrder(strUserCode, "BORROW");
            LoadGoodsReturnDetail(strROID);


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

        string strROID, strType, strReturnName, strWareHouse, strApplicant, strReturnReason, strStatus;
        string strCustomerCode, strCustomerName, strCurrencyType;

        DateTime dtReturnTime;

        strType = DL_FormType.SelectedValue.Trim();
        strROID = LB_ROID.Text.Trim();
        strReturnName = TB_ReturnName.Text.Trim();
        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strReturnReason = TB_ReturnReason.Text.Trim();
        strStatus = DL_ReturnOrderStatus.SelectedValue.Trim();

        strCustomerCode = DL_Customer.SelectedValue.Trim();

        if (strCustomerCode != "")
        {
            strCustomerName = DL_Customer.SelectedItem.Text;
        }
        else
        {
            strCustomerName = "";
        }

        strCurrencyType = "";

        try
        {
            strHQL = "From GoodsReturnOrder as goodsReturnOrder Where goodsReturnOrder.ROID = " + strROID;
            GoodsReturnOrderBLL goodsReturnOrderBLL = new GoodsReturnOrderBLL();
            lst = goodsReturnOrderBLL.GetAllGoodsReturnOrders(strHQL);
            GoodsReturnOrder goodsReturnOrder = (GoodsReturnOrder)lst[0];

            goodsReturnOrder.Type = strType;
            goodsReturnOrder.Applicant = strApplicant;
            goodsReturnOrder.ReturnTime = dtReturnTime;
            goodsReturnOrder.ReturnName = strReturnName;
            //goodsReturnOrder.Amount = deAmount;
            goodsReturnOrder.CurrencyType = strCurrencyType;
            goodsReturnOrder.CustomerCode = strCustomerCode;
            goodsReturnOrder.CustomerName = strCustomerName;
            goodsReturnOrder.OperatorCode = strUserCode;
            goodsReturnOrder.OperatorName = ShareClass.GetUserName(strUserCode);
            goodsReturnOrder.Status = strStatus;

            goodsReturnOrderBLL.UpdateGoodsReturnOrder(goodsReturnOrder, int.Parse(strROID));

            LoadGoodsReturnOrder(strUserCode, "BORROW");
            LoadGoodsReturnDetail(strROID);

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select ROID as BorrowROID,ROID as DetailROID,* from T_GoodsReturnOrder where ROID = " + strROID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strGoodsCode, strGoodsName, strGoodsType, strModelNumber, strSpec;
        string strWareHouse, strDepartString;

        TabContainer1.ActiveTabIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strGoodsType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strGoodsType = "%" + strGoodsType + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "Select * From T_Goods  Where GoodsCode Like " + "'" + strGoodsCode + "'" + " and GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and type Like " + "'" + strGoodsType + "'" + " and ModelNumber Like " + "'" + strModelNumber + "'" + " and Spec Like " + "'" + strSpec + "'";
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
                NB_WarrantyPeriod.Amount = item.WarrantyPeriod;

                try
                {
                    DL_Type.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_Type.SelectedValue = "";
                }

                TB_ModelNumber.Text = item.ModelNumber.Trim();
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

                if (LB_SourceRelatedID.Text.Trim() == "0")
                {
                    DL_RecordSourceType.SelectedValue = "Other";
                    NB_RecordSourceID.Amount = 0;
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
            TB_SN.Text = goods.SN.Trim();
            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();
            TB_Brand.Text = goods.Manufacturer;

            DL_Type.SelectedValue = goods.Type;

            DL_Unit.SelectedValue = goods.UnitName;
            NB_WarrantyPeriod.Amount = goods.WarrantyPeriod;

            if (LB_SourceRelatedID.Text.Trim() == "0")
            {
                DL_RecordSourceType.SelectedValue = "Other";
                NB_RecordSourceID.Amount = 0;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
    }

    protected void DataGrid8_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid8.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql8.Text;

        GoodsBorrowOrderBLL goodsBorrowOrderBLL = new GoodsBorrowOrderBLL();
        IList lst = goodsBorrowOrderBLL.GetAllGoodsBorrowOrders(strHQL);

        DataGrid8.DataSource = lst;
        DataGrid8.DataBind();
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strBorrowNO;

            TabContainer1.ActiveTabIndex = 4;

            strBorrowNO = ((Button)e.Item.FindControl("BT_BorrowNO")).Text.Trim();

            for (int i = 0; i < DataGrid8.Items.Count; i++)
            {
                DataGrid8.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_BorrowNO.Text = strBorrowNO;

            NB_SourceID.Amount = int.Parse(strBorrowNO);

            LoadGoodsBorrowOrderDetail(strBorrowNO);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void LoadGoodsBorrowOrderDetail(string strBorrowNO)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsBorrowOrderDetail as goodsBorrowOrderDetail Where goodsBorrowOrderDetail.BorrowNO = " + strBorrowNO + " Order By goodsBorrowOrderDetail.ID ASC";
        GoodsBorrowOrderDetailBLL goodsBorrowOrderDetailBLL = new GoodsBorrowOrderDetailBLL();
        lst = goodsBorrowOrderDetailBLL.GetAllGoodsBorrowOrderDetails(strHQL);

        DataGrid10.DataSource = lst;
        DataGrid10.DataBind();

        LB_Sql10.Text = strHQL;
    }

    protected void DataGrid10_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid10.Items.Count; i++)
            {
                DataGrid10.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From GoodsBorrowOrderDetail as goodsBorrowOrderDetail Where goodsBorrowOrderDetail.ID = " + strID;
            GoodsBorrowOrderDetailBLL goodsBorrowOrderDetailBLL = new GoodsBorrowOrderDetailBLL();
            lst = goodsBorrowOrderDetailBLL.GetAllGoodsBorrowOrderDetails(strHQL);

            GoodsBorrowOrderDetail goodsBorrowOrderDetail = (GoodsBorrowOrderDetail)lst[0];

            //LB_ID.Text = strID;
            TB_GoodsCode.Text = goodsBorrowOrderDetail.GoodsCode.Trim();
            TB_GoodsName.Text = goodsBorrowOrderDetail.GoodsName.Trim();
            TB_ModelNumber.Text = goodsBorrowOrderDetail.ModelNumber.Trim();
            TB_Spec.Text = goodsBorrowOrderDetail.Spec.Trim();
            TB_Brand.Text = goodsBorrowOrderDetail.Brand;

            NB_Number.Amount = goodsBorrowOrderDetail.Number;

            LB_SourceRelatedID.Text = goodsBorrowOrderDetail.BorrowNO.ToString();
            DL_RecordSourceType.SelectedValue = "GoodsBORecord";
            NB_RecordSourceID.Amount = goodsBorrowOrderDetail.ID;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }


    protected void DL_RecordSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_RecordSourceID.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strROID;

            strROID = LB_ROID.Text.Trim();

            int intWLNumber = ShareClass.GetRelatedWorkFlowNumber("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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

            //从流程中打开的业务单
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
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

                strHQL = "From GoodsReturnDetail as goodsReturnDetail Where goodsReturnDetail.ID = " + strID;
                GoodsReturnDetailBLL goodsReturnDetailBLL = new GoodsReturnDetailBLL();
                lst = goodsReturnDetailBLL.GetAllGoodsReturnDetails(strHQL);

                GoodsReturnDetail goodsReturnDetail = (GoodsReturnDetail)lst[0];

                LB_ID.Text = strID;
                TB_GoodsCode.Text = goodsReturnDetail.GoodsCode.Trim();
                TB_GoodsName.Text = goodsReturnDetail.GoodsName.Trim();
                try
                {
                    DL_Type.SelectedValue = goodsReturnDetail.Type;
                }
                catch
                {
                    DL_Type.SelectedValue = "";
                }
                TB_ModelNumber.Text = goodsReturnDetail.ModelNumber.Trim();
                TB_Spec.Text = goodsReturnDetail.Spec.Trim();
                TB_Brand.Text = goodsReturnDetail.Brand;

                NB_Number.Amount = goodsReturnDetail.Number;
                DL_Type.SelectedValue = goodsReturnDetail.Type;

                DL_Unit.SelectedValue = goodsReturnDetail.UnitName;

                TB_ReturnReason.Text = goodsReturnDetail.ReturnReason.Trim();

                LB_SourceRelatedID.Text = goodsReturnDetail.RelatedID.ToString();
                DL_RecordSourceType.SelectedValue = goodsReturnDetail.SourceType.Trim();
                NB_RecordSourceID.Amount = goodsReturnDetail.SourceID;



                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = ShareClass.GetRelatedWorkFlowNumber("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    return;
                }
                    strHQL = "Delete From T_GoodsReturnDetail Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadGoodsReturnDetail(strROID);

                
                    LB_ID.Text = "0";

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText;
                        
                        strCmdText = "select ROID as BorrowROID,ROID as DetailROID,* from T_GoodsReturnOrder where ROID = " + strROID;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
                        }

                        if (strToDoWLID != null)
                        {
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }

                        if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                        {
                            strCmdText = "select * from T_GoodsReturnDetail where ROID = " + strROID;
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
        string strROID;

        strROID = LB_ROID.Text.Trim();

        if (strROID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strROID = LB_ROID.Text.Trim();
        int intWLNumber = ShareClass.GetRelatedWorkFlowNumber("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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
        string strReturnDetailID, strROID, strGoodsCode, strGoodsName, strSN, strType, strModelNumber, strSpec, strUnitName, strRecordSourceType, strRecordSourceID,  strReturnReason;
        decimal deNumber;
        string strRelatedID;
        int intWarrantyPeriod;


        decimal dePrice;
        DateTime dtReturnTime;

        strROID = LB_ROID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strSN = TB_SN.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        deNumber = NB_Number.Amount;
        dePrice = 0;

        strUnitName = DL_Unit.SelectedValue.Trim();

        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        strRelatedID = LB_SourceRelatedID.Text.Trim();
        strRecordSourceType = DL_RecordSourceType.SelectedValue.Trim();
        strRecordSourceID = NB_RecordSourceID.Amount.ToString();

        strReturnReason = TB_ReturnReason.Text.Trim();
        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);

        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
        else
        {
            GoodsReturnDetailBLL goodsReturnDetailBLL = new GoodsReturnDetailBLL();
            GoodsReturnDetail goodsReturnDetail = new GoodsReturnDetail();

            goodsReturnDetail.ROID = int.Parse(strROID);
            goodsReturnDetail.GoodsCode = strGoodsCode;
            goodsReturnDetail.GoodsName = strGoodsName;
            if (strSN == "")
            {
                //strSN = strGoodsCode;
            }
            goodsReturnDetail.SN = strSN;
            goodsReturnDetail.Type = strType;

            goodsReturnDetail.ModelNumber = strModelNumber;

            goodsReturnDetail.Spec = strSpec;
            goodsReturnDetail.Brand = TB_Brand.Text;

            goodsReturnDetail.Number = deNumber;
            goodsReturnDetail.UnitName = strUnitName;
            goodsReturnDetail.Price = dePrice;

            goodsReturnDetail.Amount = deNumber * dePrice;
            goodsReturnDetail.WarrantyPeriod = intWarrantyPeriod;


            goodsReturnDetail.RelatedID = int.Parse(LB_SourceRelatedID.Text.Trim());
            goodsReturnDetail.SourceType = strRecordSourceType;
            goodsReturnDetail.SourceID = int.Parse(strRecordSourceID);

            goodsReturnDetail.ReturnReason = strReturnReason;

            try
            {
                goodsReturnDetailBLL.AddGoodsReturnDetail(goodsReturnDetail);

                LB_ID.Text = ShareClass.GetMyCreatedMaxGoodsReturnDetailID().ToString();
                strReturnDetailID = LB_ID.Text;

                LoadGoodsReturnDetail(strROID);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select ROID as BorrowROID,ROID as DetailROID,* from T_GoodsReturnOrder where ROID = " + strROID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsReturnDetail where ROID = " + strROID;
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
        string strReturnDetailID, strROID, strGoodsCode, strGoodsName, strSN, strType, strModelNumber, strSpec, strUnitName, strManufacturer, strRelatedID, strRecordSourceType, strRecordSourceID, strToPosition, strReturnReason, strCurrencyType;
        decimal deNumber;
        int intID, intWarrantyPeriod;

        DateTime dtReturnTime;


        intID = int.Parse(LB_ID.Text);
        strROID = LB_ROID.Text.Trim();
        strReturnDetailID = LB_ID.Text.Trim();

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strSN = TB_SN.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();

        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        strRelatedID = LB_SourceRelatedID.Text.Trim();
        strRecordSourceType = DL_RecordSourceType.SelectedValue;
        strRecordSourceID = NB_RecordSourceID.Amount.ToString();

        strReturnReason = TB_ReturnReason.Text.Trim();

        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);

        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsReturnDetailBLL goodsReturnDetailBLL = new GoodsReturnDetailBLL();
            GoodsReturnDetail goodsReturnDetail = new GoodsReturnDetail();

            goodsReturnDetail.ROID = int.Parse(strROID);
            goodsReturnDetail.GoodsCode = strGoodsCode;
            goodsReturnDetail.GoodsName = strGoodsName;
            goodsReturnDetail.SN = strSN;
            goodsReturnDetail.Type = strType;
            goodsReturnDetail.ModelNumber = strModelNumber;
            goodsReturnDetail.Spec = strSpec;
            goodsReturnDetail.Brand = TB_Brand.Text;

            goodsReturnDetail.Price = 0;
            goodsReturnDetail.Number = deNumber;
            goodsReturnDetail.Amount = 0;

            goodsReturnDetail.UnitName = strUnitName;
            goodsReturnDetail.WarrantyPeriod = intWarrantyPeriod;


            goodsReturnDetail.RelatedID = int.Parse(LB_SourceRelatedID.Text.Trim());
            goodsReturnDetail.SourceType = strRecordSourceType;
            goodsReturnDetail.SourceID = int.Parse(strRecordSourceID);

            goodsReturnDetail.ReturnReason = strReturnReason;

            try
            {
                goodsReturnDetailBLL.UpdateGoodsReturnDetail(goodsReturnDetail, intID);

                LoadGoodsReturnDetail(strROID);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select ROID as BorrowROID,ROID as DetailROID,* from T_GoodsReturnOrder where ROID = " + strROID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("BorrowingReturn", LanguageHandle.GetWord("WuLiao"), strROID);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsReturnDetail where ROID = " + strROID;
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


    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText, strROID;

        string strWLID, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strROID = LB_ROID.Text.Trim();

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
        workFlow.RelatedType = LanguageHandle.GetWord("WuLiao");
        workFlow.Status = "New";
        workFlow.RelatedID = int.Parse(strROID);
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

            ShareClass.LoadRelatedWL(strWLType, LanguageHandle.GetWord("WuLiao"), int.Parse(strROID), DataGrid8);

            UpdateGoodsReturnOrderStatus(strROID, "InProgress");
            DL_ReturnOrderStatus.SelectedValue = "InProgress";

            strCmdText = "select ROID as DetailROID, ROID as BorrowROID, * from T_GoodsReturnOrder where ROID = " + strROID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsReturnOrder", strXMLFile2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPGHSSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPGHGSSBKNGZLMCGCZD25GHZJC") + "')", true);
            return "0";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

        return strWLID;
    }


    protected void UpdateGoodsReturnOrderStatus(string strROID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "Update T_GoodsReturnOrder Set Status = " + "'" + strStatus + "'" + " Where ROID = " + strROID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

    }


    protected decimal GetSumReturnNumberForGoodsBorrow(string strRecordSourceID)
    {
        string strHQL;

        strHQL = " Select COALESCE(Sum(Number),0) From T_GoodsReturnDetail Where SourceID = " + strRecordSourceID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsReturnDetail");

        return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
    }


    protected decimal SumGoodsReturnOrderAmount(string strROID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from GoodsReturnDetail as goodsReturnDetail where goodsReturnDetail.ROID = " + strROID;
        GoodsReturnDetailBLL goodsReturnDetailBLL = new GoodsReturnDetailBLL();
        lst = goodsReturnDetailBLL.GetAllGoodsReturnDetails(strHQL);

        GoodsReturnDetail goodsReturnDetail = new GoodsReturnDetail();

        for (int i = 0; i < lst.Count; i++)
        {
            goodsReturnDetail = (GoodsReturnDetail)lst[i];
            deAmount += goodsReturnDetail.Number * goodsReturnDetail.Price;
        }

        return deAmount;
    }

    protected void UpdateGoodsReturnOrderAmount(string strROID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsReturnOrder as goodsReturnOrder where goodsReturnOrder.ROID = " + strROID;
        GoodsReturnOrderBLL goodsReturnOrderBLL = new GoodsReturnOrderBLL();
        lst = goodsReturnOrderBLL.GetAllGoodsReturnOrders(strHQL);

        GoodsReturnOrder goodsReturnOrder = (GoodsReturnOrder)lst[0];

        goodsReturnOrder.Amount = deAmount;

        try
        {
            goodsReturnOrderBLL.UpdateGoodsReturnOrder(goodsReturnOrder, int.Parse(strROID));
        }
        catch
        {
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

    protected bool CheckReturnNumber(string strFromGoodsID, decimal deReturnNumber)
    {
        string strHQL;
        IList lst;

        decimal deGoodsNumber;

        strHQL = "From Goods as goods where goods.ID = " + strFromGoodsID;
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        Goods goods = (Goods)lst[0];

        deGoodsNumber = goods.Number;

        if (deGoodsNumber < deReturnNumber)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void LoadGoodsReturnOrder(string strUserCode, string strFormType)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsReturnOrder as goodsReturnOrder Where goodsReturnOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and Type = " + "'" + strFormType + "'";
        strHQL += " Order By goodsReturnOrder.ROID DESC";

        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "From GoodsReturnOrder as goodsReturnOrder Where goodsReturnOrder.ROID = " + strWLBusinessID;
        }

        GoodsReturnOrderBLL goodsReturnOrderBLL = new GoodsReturnOrderBLL();
        lst = goodsReturnOrderBLL.GetAllGoodsReturnOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadGoodsReturnDetail(string strROID)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsReturnDetail as goodsReturnDetail Where goodsReturnDetail.ROID = " + strROID + " Order By goodsReturnDetail.ID ASC";
        GoodsReturnDetailBLL goodsReturnDetailBLL = new GoodsReturnDetailBLL();
        lst = goodsReturnDetailBLL.GetAllGoodsReturnDetails(strHQL);

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


    protected void LoadGoodsBorrowOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "From GoodsBorrowOrder as goodsBorrowOrder Where goodsBorrowOrder.OperatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order By goodsBorrowOrder.BorrowNO DESC";
        GoodsBorrowOrderBLL goodsBorrowOrderBLL = new GoodsBorrowOrderBLL();
        lst = goodsBorrowOrderBLL.GetAllGoodsBorrowOrders(strHQL);

        DataGrid8.DataSource = lst;
        DataGrid8.DataBind();

        LB_Sql8.Text = strHQL;
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


}
