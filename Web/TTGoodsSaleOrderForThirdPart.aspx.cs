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

public partial class TTGoodsSaleOrderForThirdPart : System.Web.UI.Page
{
    string strRelatedType, strRelatedID;
    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        string strUserCode = Session["UserCode"].ToString();

        //从流程中打开的业务单
        strToDoWLID = Request.QueryString["WLID"]; strToDoWLDetailID = Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
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

            LB_SalesCode.Text = strUserCode;
            LB_SalesName.Text = strUserName;

            LB_ApplicantCode.Text = strUserCode;
            LB_ApplicantName.Text = strUserCode;

            ShareClass.LoadWFTemplate(strUserCode, "MaterialSales", DL_TemName);

            LoadGoodsSaleOrder(strUserCode);
            ShareClass.LoadCustomer(DL_Customer, strUserCode);
            LoadGoodsSaleQuotationOrder(strUserCode);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        LB_ApplicantCode.Text = strUserCode;
        LB_ApplicantName.Text = GetUserName(strUserCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
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
                    //DL_Customer.SelectedValue = goodsSaleOrder.CustomerCode.Trim();
                }


                DLC_SaleTime.Text = goodsSaleOrder.SaleTime.ToString("yyyy-MM-dd");
                DLC_ArrivalTime.Text = goodsSaleOrder.ArrivalTime.ToString("yyyy-MM-dd");
                NB_Amount.Amount = goodsSaleOrder.Amount;

                DL_CurrencyType.SelectedValue = goodsSaleOrder.CurrencyType;

                TB_Comment.Text = goodsSaleOrder.Comment.Trim();
                DL_SOStatus.SelectedValue = goodsSaleOrder.Status.Trim();
                

                strRelatedType = goodsSaleOrder.RelatedType.Trim();

                LoadGoodsSaleOrderDetail(strSOID);
                LoadRelatedConstract(strSOID);
                LoadGoodsSaleQuotationOrder(LB_UserCode.Text.Trim(), goodsSaleOrder.CustomerCode.Trim());


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
                strHQL = "Delete From T_GoodsSaleorder Where SOID = " + strSOID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

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

    protected void DL_Customer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCustomerCode, strOperatorCode;

        strCustomerCode = DL_Customer.SelectedValue.Trim();
        strOperatorCode = LB_UserCode.Text.Trim();

        LoadGoodsSaleQuotationOrder(strOperatorCode, strCustomerCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_SOID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

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
        string strSOID, strCustomerCode, strCustomerName, strSOName, strSalesCode, strSalesName, strOperatorCode, strOperatorName, strCurrencyType, strComment;
        DateTime dtSaleTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;


        strSOName = TB_SOName.Text.Trim();
        strSalesCode = LB_SalesCode.Text.Trim();
        strSalesName = LB_SalesName.Text.Trim();
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
            goodsSaleOrder.SalesName = GetUserName(strSalesCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXSRDMBZCWCRJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        goodsSaleOrder.OperatorCode = strOperatorCode;
        goodsSaleOrder.OperatorName = strOperatorName;

        goodsSaleOrder.CustomerCode = strCustomerCode;
        goodsSaleOrder.CustomerName = strCustomerName;

        goodsSaleOrder.SaleTime = dtSaleTime;
        goodsSaleOrder.ArrivalTime = dtArrivalTime;
        goodsSaleOrder.Amount = 0;
        goodsSaleOrder.CurrencyType = strCurrencyType;
        goodsSaleOrder.Comment = strComment;
        goodsSaleOrder.Status = "New";

        goodsSaleOrder.RelatedType = "Other";
        goodsSaleOrder.RelatedID = 0;

        try
        {
            goodsSaleOrderBLL.AddGoodsSaleOrder(goodsSaleOrder);

            strSOID = ShareClass.GetMyCreatedMaxGoodsSaleOrderID(strOperatorCode);
            LB_SOID.Text = strSOID;

            //BT_UpdateSO.Enabled = true;
            //BT_DeleteSO.Enabled = true;

            //BT_New.Enabled = true;

            NB_Amount.Amount = 0;

            LB_GoodsOwner.Text = LanguageHandle.GetWord("XiaoShouDan").ToString().Trim() + strSOID + LanguageHandle.GetWord("MingXi").ToString().Trim();
            TB_WLName.Text = LanguageHandle.GetWord("XiaoShou").ToString().Trim()  + strSOName + LanguageHandle.GetWord("ShenQing").ToString().Trim();

            BT_SubmitApply.Enabled = true;

            LoadGoodsSaleOrder(strSalesCode);
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

        strSalesCode = LB_SalesCode.Text.Trim();
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

        goodsSaleOrder.CustomerCode = strCustomerCode;
        goodsSaleOrder.CustomerName = strCustomerName;

        goodsSaleOrder.SalesCode = strSalesCode;
        try
        {
            goodsSaleOrder.SalesName = GetUserName(strSalesCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXSRDMBZCWCRJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        goodsSaleOrder.SaleTime = dtSaleTime;
        goodsSaleOrder.ArrivalTime = dtArrivalTime;
        goodsSaleOrder.Amount = deAmount;
        goodsSaleOrder.CurrencyType = strCurrencyType;
        goodsSaleOrder.Comment = strComment;
        goodsSaleOrder.Status = strStatus;

        goodsSaleOrder.RelatedType = "Other";
        goodsSaleOrder.RelatedID = 0;


        try
        {
            goodsSaleOrderBLL.UpdateGoodsSaleOrder(goodsSaleOrder, int.Parse(strSOID));
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strGoodsCode, strGoodsName, strSpec, strModelNumber;

        TabContainer2.ActiveTabIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strSpec = "%" + strSpec + "%";
        strModelNumber = "%" + strModelNumber + "%";

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
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

            //LB_QOID.Text = strQOID;

            LoadGoodsSaleQuotationOrderDetail(strQOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strID, strQOID;
            IList lst;

            TabContainer1.ActiveTabIndex = 0;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;


            for (int i = 0; i < DataGrid1.Items.Count; i++)
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

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = GetRelatedWorkFlowNumber("MaterialSales", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strSOID);

                if (intWLNumber > 0)
                {
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
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }
                            ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
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
        string strApplicantCode, strSaleReason;


        strSOID = LB_SOID.Text.Trim();
        strRelatedType = "Other";
        strRelatedID = "0";

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        decNumber = NB_Number.Amount;
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strSaleReason = TB_SaleReason.Text.Trim();
        dePrice = NB_Price.Amount;
        strApplicantCode = LB_ApplicantCode.Text.Trim();
        dtBuyTime = DateTime.Now;

        if (strType == "" | strGoodsName == "" | strSpec == "" | strApplicantCode == "")
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
            goodsSaleRecord.ModelNumber = strModelNumber;
            goodsSaleRecord.Spec = strSpec;
            goodsSaleRecord.Brand = TB_Brand.Text;

            goodsSaleRecord.SaleReason = strSaleReason;
            goodsSaleRecord.CheckOutNumber = 0;
            goodsSaleRecord.DeliveryNumber = 0;
            
            try
            {
                goodsSaleRecordBLL.AddGoodsSaleRecord(goodsSaleRecord);

                strRecordID = ShareClass.GetMyCreatedMaxGoodsSaleRecordID(strSOID);
                LB_ID.Text = strRecordID;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                LoadGoodsSaleOrderDetail(strSOID);

                NB_Amount.Amount = SumGoodsSaleOrderAmount(strSOID);
                UpdateGoodsSaleOrderAmount(strSOID, NB_Amount.Amount);

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
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }
                        ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
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
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strSaleReason, strUnitName;
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
        strRelatedType = "Other";
        strRelatedID = "0";

        strType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strSaleReason = TB_SaleReason.Text.Trim();
        dePrice = NB_Price.Amount;
        deNumber = NB_Number.Amount;
        strApplicantCode = LB_ApplicantCode.Text.Trim();
        dtBuyTime = DateTime.Now;

        strSalesCode = LB_SalesCode.Text.Trim();
        strSalesName = LB_SalesName.Text.Trim();


        if (strType == "" | strGoodsName == "" | strSpec == "" | strApplicantCode == "")
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
            goodsSaleRecord.ModelNumber = strModelNumber;
            goodsSaleRecord.Spec = strSpec;
            goodsSaleRecord.Brand = TB_Brand.Text;

            goodsSaleRecord.SaleReason = strSaleReason;

            try
            {
                goodsSaleRecordBLL.UpdateGoodsSaleRecord(goodsSaleRecord, int.Parse(strID));

                LoadGoodsSaleOrderDetail(strSOID);

                NB_Amount.Amount = SumGoodsSaleOrderAmount(strSOID);
                UpdateGoodsSaleOrderAmount(strSOID, NB_Amount.Amount);

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
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }
                        ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
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
        strCreatorName = GetUserName(strCreatorCode);

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

            strCmdText = "select * from T_GoodsSaleOrder where SOID = " + strSOID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsSaleOrder", strXMLFile2);

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

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        IList lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        DataGrid5.DataSource = lst;
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

    protected void LoadGoodsSaleQuotationOrderDetail(string strQOID)
    {
        LB_GoodsOwner.Text = LanguageHandle.GetWord("BaoJiaDan").ToString().Trim() + ": " + strQOID + LanguageHandle.GetWord("MingXi").ToString().Trim();

        string strHQL = "Select * from T_GoodsSaleQuotationOrderDetail where QOID = " + strQOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleQuotationOrderDetail");

        DataGrid8.DataSource = ds;
        DataGrid8.DataBind();
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
        IList lst;

        strHQL = "from GoodsSaleOrder as goodsSaleOrder where goodsSaleOrder.OperatorCode = " + "'" + strOperatorCode + "'" + " Order by goodsSaleOrder.SOID DESC";

        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "Select * from T_GoodsSaleOrder where SOID = " + strWLBusinessID;
        }

        GoodsSaleOrderBLL goodsSaleOrderBLL = new GoodsSaleOrderBLL();
        lst = goodsSaleOrderBLL.GetAllGoodsSaleOrders(strHQL);

        DataGrid5.DataSource = lst;
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
        LB_GoodsOwner.Text = LanguageHandle.GetWord("XiaoShouDan").ToString().Trim() + ": " + strSOID + LanguageHandle.GetWord("MingXi").ToString().Trim();

        string strHQL = "Select * from T_GoodsSaleRecord where SOID = " + strSOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

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
}

