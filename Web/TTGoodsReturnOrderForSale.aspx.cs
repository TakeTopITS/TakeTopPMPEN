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

public partial class TTGoodsReturnOrderForSale : System.Web.UI.Page
{
    string strUserCode;
    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        //从流程中打开的业务单
        strToDoWLID = Request.QueryString["WLID"]; strToDoWLDetailID= Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        string strUserName;
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

            ShareClass.LoadCurrencyType(DL_CurrencyType);
            ShareClass.LoadCustomer(DL_Customer, strUserCode);

            LoadGoodsApplication(strUserCode);
            LoadGoodsSaleOrder(strUserCode);

            LoadGoodsReturnOrder(strUserCode, "SALE");

            ShareClass.LoadWFTemplate(strUserCode, "SalesReturn", DL_TemName);
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

            int intWLNumber = ShareClass.GetRelatedWorkFlowNumber("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
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

                LB_ROID.Text = strROID;
                TB_ReturnName.Text = goodsReturnOrder.ReturnName.Trim();
                DLC_ReturnTime.Text = goodsReturnOrder.ReturnTime.ToString("yyyy-MM-dd");
                TB_Applicant.Text = goodsReturnOrder.Applicant.Trim();

                DL_CurrencyType.SelectedValue = goodsReturnOrder.CurrencyType;
                try
                {
                    DL_Customer.SelectedValue = goodsReturnOrder.CustomerCode;
                }
                catch
                {
                }
                DL_ReturnOrderStatus.SelectedValue = goodsReturnOrder.Status.Trim();

                NB_Amount.Amount = goodsReturnOrder.Amount;

                LoadGoodsReturnDetail(strROID);

                TB_WLName.Text = LanguageHandle.GetWord("XiaoShouTuiHuo") + goodsReturnOrder.ReturnName.Trim() + LanguageHandle.GetWord("ShenQing");
                ShareClass.LoadRelatedWL("SalesReturn", LanguageHandle.GetWord("WuLiao"), goodsReturnOrder.ROID, DataGrid8);

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
                intWLNumber = ShareClass.GetRelatedWorkFlowNumber("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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

                    LoadGoodsReturnOrder(strUserCode, "SALE");
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

    protected void DL_Customer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCustomerCode, strOperatorCode;

        strCustomerCode = DL_Customer.SelectedValue.Trim();
        strOperatorCode = LB_UserCode.Text.Trim();
        
        LoadCustomerRelatedGoodsList(strCustomerCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
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

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        deAmount = NB_Amount.Amount;


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

            LoadGoodsReturnOrder(strUserCode, "SALE");
            LoadGoodsReturnDetail(strROID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strROID, strType, strReturnName, strApplicant, strReturnReason, strStatus;
        string strCustomerCode, strCustomerName, strCurrencyType;
        decimal deAmount;

        DateTime dtReturnTime;

        strROID = LB_ROID.Text.Trim();
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

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        deAmount = NB_Amount.Amount;

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
            goodsReturnOrder.Amount = deAmount;
            goodsReturnOrder.CurrencyType = strCurrencyType;
            goodsReturnOrder.CustomerCode = strCustomerCode;
            goodsReturnOrder.CustomerName = strCustomerName;
            goodsReturnOrder.OperatorCode = strUserCode;
            goodsReturnOrder.OperatorName = ShareClass.GetUserName(strUserCode);
            goodsReturnOrder.Status = strStatus;


            goodsReturnOrderBLL.UpdateGoodsReturnOrder(goodsReturnOrder, int.Parse(strROID));

            LoadGoodsReturnOrder(strUserCode, "SALE");
            LoadGoodsReturnDetail(strROID);


            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select ROID as SaleROID,ROID as DetailROID, * from T_GoodsReturnOrder where ROID = " + strROID;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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
            NB_SourceID.Amount = 0;
        }
        else
        {
            BT_SelectAO.Visible = false;
        }

        if (strSourceType == "GoodsSO")
        {
            BT_SelectSO.Visible = true;
            NB_SourceID.Amount = 0;
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

            LB_A0ID.Text = strAAID;

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

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;

        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        IList lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
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

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strROID = LB_ROID.Text.Trim();

            int intWLNumber = ShareClass.GetRelatedWorkFlowNumber("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
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
                NB_Price.Amount = goodsReturnDetail.Price;
                DL_Unit.SelectedValue = goodsReturnDetail.UnitName;
                TB_ReturnReason.Text = goodsReturnDetail.ReturnReason.Trim();

                LB_SourceRelatedID.Text = goodsReturnDetail.RelatedID.ToString();
                DL_RecordSourceType.SelectedValue = goodsReturnDetail.SourceType.Trim();
                NB_RecordSourceID.Amount = goodsReturnDetail.SourceID;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = ShareClass.GetRelatedWorkFlowNumber("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                    return;
                }
             
                string strGoodsCode;
                decimal deOldNumber;

                string strSourceType = DL_SourceType.SelectedValue;
                string strSourceID = NB_SourceID.Amount.ToString();
                string strRecordSourceType = DL_RecordSourceType.SelectedValue;
                string strRecordSourceID = NB_RecordSourceID.Amount.ToString();

                strGoodsCode = TB_GoodsCode.Text.Trim();

                deOldNumber = NB_Number.Amount;

                strROID = LB_ROID.Text.Trim();

                strID = LB_ID.Text.Trim();
                strHQL = "Delete From T_GoodsReturnDetail Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadGoodsReturnDetail(strROID);

                    NB_Number.Enabled = true;

                    LB_ID.Text = "0";

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText;

                        strCmdText = "select ROID as BorrowROID,ROID as DetailROID,* from T_GoodsReturnOrder where ROID = " + strROID;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            }
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

                DL_Type.SelectedValue = item.SmallType;

                TB_ModelNumber.Text = item.ModelNumber.Trim();
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

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
            TB_SN.Text = goods.SN.Trim();
            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();
            TB_Brand.Text = goods.Manufacturer;

            DL_Type.SelectedValue = goods.Type;

            DL_Unit.SelectedValue = goods.UnitName;
            NB_WarrantyPeriod.Amount = goods.WarrantyPeriod;


            NB_Number.Enabled = true;

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
                    DL_Type.SelectedValue = goodsApplicationDetail.Type;
                    TB_ModelNumber.Text = goodsApplicationDetail.ModelNumber.Trim();
                    TB_Spec.Text = goodsApplicationDetail.Spec.Trim();
                    TB_Brand.Text = goodsApplicationDetail.Brand;

                    NB_Number.Amount = goodsApplicationDetail.Number;
                    NB_Price.Amount = goodsApplicationDetail.Price;
                    DL_Unit.SelectedValue = goodsApplicationDetail.Unit;

                    LB_SourceRelatedID.Text = goodsApplicationDetail.AAID.ToString();
                    DL_RecordSourceType.SelectedValue = "GoodsAORecord";
                    NB_RecordSourceID.Amount = goodsApplicationDetail.ID;
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
                    DL_Type.SelectedValue = goodsSaleRecord.Type;
                    TB_ModelNumber.Text = goodsSaleRecord.ModelNumber.Trim();
                    TB_Spec.Text = goodsSaleRecord.Spec.Trim();
                    TB_Brand.Text = goodsSaleRecord.Brand;

                    NB_Number.Amount = goodsSaleRecord.Number;
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

    protected void DL_RecordSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        NB_RecordSourceID.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

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
        int intWLNumber = ShareClass.GetRelatedWorkFlowNumber("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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
        string strReturnDetailID, strROID, strGoodsCode, strGoodsName, strSN, strType, strModelNumber, strSpec, strUnitName, strManufacturer, strRecordSourceType, strRecordSourceID, strToPosition, strReturnReason;
        decimal deNumber;
        string strCurrencyType, strRelatedID;
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
        dePrice = NB_Price.Amount;

        strUnitName = DL_Unit.SelectedValue.Trim();

        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

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

                ////添加相同记录到物料表
                //addGoods(strReturnDetailID, strGoodsCode, strGoodsName,strSN, deNumber, strUnitName,
                //   strUserCode, strType, strSpec, strModelNumber, strToPosition, "", dePrice, strCurrencyType, dtReturnTime,intWarrantyPeriod,
                //   strManufacturer, strReturnReason, LB_ID.Text);

                //更新物料退货单已退货数量
                NB_Amount.Amount = SumGoodsReturnOrderAmount(strROID);
                UpdateGoodsReturnOrderAmount(strROID, NB_Amount.Amount);

                ////插入应收应付数据到应收应付表
                //if (strRecordSourceType == "GoodsSORecord")
                //{
                //    string strCustomerName = ShareClass.GetCustomerNameFromGoodsSaleOrder(strRelatedID);
                //    ShareClass.InsertReceivablesOrPayable("GoodsRO", "GoodsSO", strRelatedID, strRecordSourceID, deNumber * dePrice, strCurrencyType, strCustomerName, strUserCode);
                //}
                //if (strRecordSourceType == "GoodsAORecord")
                //{
                //    string strApplicantName = ShareClass.GetApplicantNameFromGoodsApplicaitonOrder(strRelatedID);
                //    ShareClass.InsertReceivablesOrPayable("GoodsRO", "GoodsAO", strRelatedID, strRecordSourceID, deNumber * dePrice, strCurrencyType, strApplicantName, strUserCode);
                //}

                ////更新物料借出单已退货数量
                //if (strRecordSourceType == "GoodsBORecord")
                //{
                //    strHQL = "Update T_GoodsBorrowOrderDetail Set ReturnNumber =  " + GetSumReturnNumberForGoodsBorrow(strRecordSourceID).ToString() + " Where ID = " + strRecordSourceID;
                //    ShareClass.RunSqlCommand(strHQL);

                //    LoadGoodsBorrowOrderDetail(LB_BorrowNO.Text.Trim());
                //}

                LoadGoodsReturnDetail(strROID);

         
                NB_Number.Enabled = false;

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select ROID as BorrowROID,ROID as DetailROID,* from T_GoodsReturnOrder where ROID = " + strROID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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

    protected decimal GetSumReturnNumberForGoodsBorrow(string strRecordSourceID)
    {
        string strHQL;

        strHQL = " Select COALESCE(Sum(Number),0) From T_GoodsReturnDetail Where SourceID = " + strRecordSourceID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsReturnDetail");

        return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

  
    protected void UpdateDetail()
    {
        string strReturnDetailID, strROID, strGoodsCode, strGoodsName, strSN, strType, strModelNumber, strSpec, strUnitName, strManufacturer, strRelatedID, strRecordSourceType, strRecordSourceID, strToPosition, strReturnReason, strCurrencyType;
        decimal deNumber;
        int intID, intWarrantyPeriod;
        decimal dePrice;
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
        dePrice = NB_Price.Amount;

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        intWarrantyPeriod = int.Parse(NB_WarrantyPeriod.Amount.ToString());

        strRelatedID = LB_SourceRelatedID.Text.Trim();
        strRecordSourceType = DL_RecordSourceType.SelectedValue;
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
            goodsReturnDetail.SN = strSN;
            goodsReturnDetail.Type = strType;
            goodsReturnDetail.ModelNumber = strModelNumber;
            goodsReturnDetail.Spec = strSpec;
            goodsReturnDetail.Brand = TB_Brand.Text;

            goodsReturnDetail.Price = dePrice;
            goodsReturnDetail.Number = deNumber;
            goodsReturnDetail.Amount = dePrice * deNumber;

            goodsReturnDetail.UnitName = strUnitName;
            goodsReturnDetail.WarrantyPeriod = intWarrantyPeriod;

            goodsReturnDetail.RelatedID = int.Parse(LB_SourceRelatedID.Text.Trim());
            goodsReturnDetail.SourceType = strRecordSourceType;
            goodsReturnDetail.SourceID = int.Parse(strRecordSourceID);

            goodsReturnDetail.ReturnReason = strReturnReason;

            try
            {
                goodsReturnDetailBLL.UpdateGoodsReturnDetail(goodsReturnDetail, intID);


                ////删除物料明细表相同记录
                //strHQL = "Delete From T_Goods Where ReturnDetailID =" + LB_ID.Text;
                //ShareClass.RunSqlCommand(strHQL);

                //if (strRecordSourceType == "GoodsBORecord")
                //{
                //    strHQL = "Update T_GoodsBorrowOrderDetail Set ReturnNumber =  " + GetSumReturnNumberForGoodsBorrow(strRecordSourceID).ToString() + " Where ID = " + strRecordSourceID;
                //    ShareClass.RunSqlCommand(strHQL);

                //    LoadGoodsBorrowOrderDetail(LB_BorrowNO.Text.Trim());
                //}

                ////添加相同记录到物料表
                //addGoods(strReturnDetailID, strGoodsCode, strGoodsName, strSN,deNumber, strUnitName,
                //   strUserCode, strType, strSpec, strModelNumber, strToPosition, "", dePrice, strCurrencyType, dtReturnTime,intWarrantyPeriod,
                //   strManufacturer, strReturnReason, LB_ID.Text);

                ////更新退货单已退货数量
                //NB_Amount.Amount = SumGoodsReturnOrderAmount(strROID);
                //UpdateGoodsReturnOrderAmount(strROID, NB_Amount.Amount);


                ////更新应收应付数据到应收应付表
                //if (strRecordSourceType == "GoodsSORecord")
                //{
                //    string strCustomerName = ShareClass.GetCustomerNameFromGoodsSaleOrder(strRelatedID);
                //    ShareClass.UpdateReceivablesOrPayable("GoodsRO", "GoodsSO", strRelatedID, strRecordSourceID, deNumber * dePrice, strCurrencyType, strCustomerName, strUserCode);
                //}
                //if (strRecordSourceType == "GoodsAORecord")
                //{
                //    string strApplicantName = ShareClass.GetApplicantNameFromGoodsApplicaitonOrder(strRelatedID);
                //    ShareClass.UpdateReceivablesOrPayable("GoodsRO", "GoodsAO", strRelatedID, strRecordSourceID, deNumber * dePrice, strCurrencyType, strApplicantName, strUserCode);
                //}


                LoadGoodsReturnDetail(strROID);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;

                    strCmdText = "select ROID as BorrowROID,ROID as DetailROID,* from T_GoodsReturnOrder where ROID = " + strROID;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("SalesReturn", LanguageHandle.GetWord("WuLiao"), strROID);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC") + "')", true);
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

            LoadRelatedWL(strWLType, LanguageHandle.GetWord("WuLiao"), int.Parse(strROID));

            UpdateGoodsReturnOrderStatus(strROID, "InProgress");
            DL_ReturnOrderStatus.SelectedValue = "InProgress";

            strCmdText = "select ROID as SaleROID,ROID as DetailROID, * from T_GoodsReturnOrder where ROID = " + strROID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_GoodsReturnOrder", strXMLFile2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPGHSSCCG") + "')", true);
        }
        catch(Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPGHGSSBKNGZLMCGCZD25GHZJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

            return "0";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

        return strWLID;
    }


    protected void UpdateGoodsReturnOrderStatus(string strROID, string strStatus)
    {
        string strHQL;
       
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

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid8.DataSource = lst;
        DataGrid8.DataBind();
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

        strHQL = "From GoodsReturnOrder as goodsReturnOrder Where (goodsReturnOrder.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " or goodsReturnOrder.OperatorCode in (select  memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ")) ";
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

    protected void LoadGoodsApplication(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "from GoodsApplication as goodsApplication where goodsApplication.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
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


    protected void LoadCustomer(DropDownList DL_Customer, string strUserCode)
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

    protected void LoadGoodsApplicationDetail(string strAAID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.AAID = " + strAAID;
        GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
        lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
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

    protected void LoadGoodsSaleOrderDetail(string strSOID)
    {
        LB_GoodsOwner.Text = LanguageHandle.GetWord("XiaoShouDan") + ": " + strSOID + LanguageHandle.GetWord("MingXi");

        string strHQL = "Select * from T_GoodsSaleRecord where SOID = " + strSOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

        DataGrid7.DataSource = ds;
        DataGrid7.DataBind();

        LB_Sql7.Text = strHQL;
    }

    
}
