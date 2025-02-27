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

public partial class TTGoodsBorrowOrder : System.Web.UI.Page
{
    string strUserCode;
    string strToDoWLID, strToDoWLDetailID, strWLBusinessID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        strUserCode = Session["UserCode"].ToString();

        //从流程中打开的业务单
        strToDoWLID = Request.QueryString["WLID"]; strToDoWLDetailID= Request.QueryString["WLStepDetailID"];
        strWLBusinessID = Request.QueryString["BusinessID"];

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_BorrowTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadGoodsBorrowOrder(strUserCode);

            ShareClass.InitialInvolvedProjectTree(TreeView1, strUserCode);

            LoadRelatedConstract(strUserCode);

            //物料领用
            ShareClass.LoadWFTemplate(strUserCode, "MaterialBorrowing", DL_TemName);
        }
    }

    protected void DL_RelatedType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strRelatedType;

        strRelatedType = DL_RelatedType.SelectedValue.Trim();

        if (strRelatedType == "Other")
        {
            NB_RelatedID.Amount = 0;
        }

        if (strRelatedType == "Project")
        {
            BT_RelatedProject.Visible = true;
            NB_RelatedID.Visible = true;

        }
        else
        {
            BT_RelatedProject.Visible = false;
            NB_RelatedID.Visible = false;
        }

        if (strRelatedType == "Constract")
        {
            BT_RelatedConstract.Visible = true;
            TB_RelatedCode.Visible = true;
        }
        else
        {
            BT_RelatedConstract.Visible = false;
            TB_RelatedCode.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strProjectID = treeNode.Target.Trim();

            NB_RelatedID.Amount = int.Parse(strProjectID);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strBorrowNO, strRelatedType;

            strBorrowNO = e.Item.Cells[3].Text.Trim();
            LB_BorrowNO.Text = strBorrowNO;


            int intWLNumber = LoadRelatedWL("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strBorrowNO));
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

            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO, "0");
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

                strHQL = "From GoodsBorrowOrder as goodsBorrowOrder Where goodsBorrowOrder.BorrowNO = " + strBorrowNO;
                GoodsBorrowOrderBLL goodsBorrowOrderBLL = new GoodsBorrowOrderBLL();
                lst = goodsBorrowOrderBLL.GetAllGoodsBorrowOrders(strHQL);

                GoodsBorrowOrder goodsBorrowOrder = (GoodsBorrowOrder)lst[0];

                LB_BorrowNO.Text = strBorrowNO;

                TB_BOName.Text = goodsBorrowOrder.BOName.Trim();
                DLC_BorrowTime.Text = goodsBorrowOrder.BorrowTime.ToString("yyyy-MM-dd");
                TB_BorrowReason.Text = goodsBorrowOrder.ApplicationReason.Trim();
                TB_Applicant.Text = goodsBorrowOrder.Applicant.Trim();
                DL_RelatedType.SelectedValue = goodsBorrowOrder.RelatedType.Trim();
                NB_RelatedID.Amount = goodsBorrowOrder.RelatedID;
                TB_RelatedCode.Text = goodsBorrowOrder.RelatedCode;
                DL_CurrencyType.SelectedValue = goodsBorrowOrder.CurrencyType;
                DL_Status.SelectedValue = goodsBorrowOrder.Status.Trim();

                LoadGoodsBorrowOrderDetail(strBorrowNO);

                strRelatedType = goodsBorrowOrder.RelatedType.Trim();
                if (strRelatedType == "Project")
                {
                    BT_RelatedProject.Visible = true;
                }
                else
                {
                    BT_RelatedProject.Visible = false;
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
                intWLNumber = LoadRelatedWL("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strBorrowNO));
                if (intWLNumber > 0)
                {
                    return;
                }

                try
                {
                    strHQL = "Delete From T_GoodsBorrowOrder Where BorrowNO = " + strBorrowNO;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_GoodsBorrowOrderDetail Where BorrowNO = " + strBorrowNO;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadGoodsBorrowOrder(strUserCode);
                    LoadGoodsBorrowOrderDetail(strBorrowNO);


                    LB_BorrowNO.Text = "0";
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCKNCZMXJLJC").ToString().Trim() + "')", true);
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

        GoodsBorrowOrderBLL goodsBorrowOrderBLL = new GoodsBorrowOrderBLL();
        IList lst = goodsBorrowOrderBLL.GetAllGoodsBorrowOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }
   
    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strConstractCode;

            strConstractCode = ((Button)e.Item.FindControl("BT_ConstractCode")).Text.Trim();
            TB_RelatedCode.Text = strConstractCode;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_BorrowNO.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        string strNewBOCode = ShareClass.GetCodeByRule("BorrowOrderCode", "BorrowOrderCode", "00");
        if (strNewBOCode != "")
        {
            TB_BOName.Text = strNewBOCode;
        }


        LoadGoodsBorrowOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strBorrowNO;

        strBorrowNO = LB_BorrowNO.Text.Trim();

        if (strBorrowNO == "")
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
        string strBorrowNO, strBOName, strStatus, strApplicant, strBorrowReason, strRelatedType, strRelatedCode, strCurrencyType;
        DateTime dtBorrowTime;
        int intRelatedID;

        strBOName = TB_BOName.Text.Trim();
        dtBorrowTime = DateTime.Parse(DLC_BorrowTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strBorrowReason = TB_BorrowReason.Text.Trim();

        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        intRelatedID = int.Parse(NB_RelatedID.Amount.ToString());
        strRelatedCode = TB_RelatedCode.Text.Trim();

        strStatus = DL_Status.SelectedValue;

        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        try
        {
            GoodsBorrowOrderBLL goodsBorrowOrderBLL = new GoodsBorrowOrderBLL();
            GoodsBorrowOrder goodsBorrowOrder = new GoodsBorrowOrder();

            goodsBorrowOrder.BOName = strBOName;
            goodsBorrowOrder.Applicant = strApplicant;
            goodsBorrowOrder.BorrowTime = dtBorrowTime;
            goodsBorrowOrder.ApplicationReason = strBorrowReason;
            goodsBorrowOrder.RelatedType = strRelatedType;
            goodsBorrowOrder.RelatedID = intRelatedID;
            goodsBorrowOrder.RelatedCode = strRelatedCode;


            goodsBorrowOrder.OperatorCode = strUserCode;
            goodsBorrowOrder.OperatorName = ShareClass.GetUserName(strUserCode);
            goodsBorrowOrder.CurrencyType = strCurrencyType;
            goodsBorrowOrder.Status = strStatus;

            goodsBorrowOrderBLL.AddGoodsBorrowOrder(goodsBorrowOrder);

            strBorrowNO = ShareClass.GetMyCreatedMaxGoodsBorrowNO(strUserCode).ToString();

            LB_BorrowNO.Text = strBorrowNO;

            string strNewBOCode = ShareClass.GetCodeByRule("BorrowOrderCode", "BorrowOrderCode", strBorrowNO);
            if (strNewBOCode != "")
            {
                TB_BOName.Text = strNewBOCode;
                string strHQL = "Update T_GoodsBorrowOrder Set BOName = " + "'" + strNewBOCode + "'" + " Where BorrowNO = " + strBorrowNO;
                ShareClass.RunSqlCommand(strHQL);
            }


            LoadGoodsBorrowOrder(strUserCode);
            LoadGoodsBorrowOrderDetail(strBorrowNO);

       
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

        string strBorrowNO, strBOName, strStatus, strApplicant, strBorrowReason, strRelatedType, strRelatedCode, strCurrencyType;
        DateTime dtBorrowTime;
        int intRelatedID;

        strBorrowNO = LB_BorrowNO.Text.Trim();
        strBOName = TB_BOName.Text.Trim();
        dtBorrowTime = DateTime.Parse(DLC_BorrowTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strBorrowReason = TB_BorrowReason.Text.Trim();
        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        intRelatedID = int.Parse(NB_RelatedID.Amount.ToString());
        strRelatedCode = TB_RelatedCode.Text.Trim();
        strStatus = DL_Status.SelectedValue;


        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        try
        {
            strHQL = "From GoodsBorrowOrder as goodsBorrowOrder Where goodsBorrowOrder.BorrowNO = " + strBorrowNO;
            GoodsBorrowOrderBLL goodsBorrowOrderBLL = new GoodsBorrowOrderBLL();
            lst = goodsBorrowOrderBLL.GetAllGoodsBorrowOrders(strHQL);
            GoodsBorrowOrder goodsBorrowOrder = (GoodsBorrowOrder)lst[0];

            goodsBorrowOrder.BOName = strBOName;
            goodsBorrowOrder.Applicant = strApplicant;
            goodsBorrowOrder.BorrowTime = dtBorrowTime;
            goodsBorrowOrder.ApplicationReason = strBorrowReason;
            goodsBorrowOrder.RelatedType = strRelatedType;
            goodsBorrowOrder.RelatedID = intRelatedID;
            goodsBorrowOrder.RelatedCode = strRelatedCode;
            goodsBorrowOrder.OperatorCode = strUserCode;
            goodsBorrowOrder.OperatorName = ShareClass.GetUserName(strUserCode);

            goodsBorrowOrder.CurrencyType = strCurrencyType;
            goodsBorrowOrder.Status = strStatus;

            goodsBorrowOrderBLL.UpdateGoodsBorrowOrder(goodsBorrowOrder, int.Parse(strBorrowNO));

            LoadGoodsBorrowOrder(strUserCode);
            LoadGoodsBorrowOrderDetail(strBorrowNO);

            //从流程中打开的业务单
            //更改工作流关联的数据文件
            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO, "0");
            if (strToDoWLID != null | strAllowFullEdit == "YES")
            {
                string strCmdText = "select BorrowNO as DetailBorrowNO, * from T_GoodsBorrowOrder where BorrowNO = " + strBorrowNO;
                if (strToDoWLID == null)
                {
                    strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO);
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

        string strGoodsCode, strGoodsName, strGoodsType, strModelNumber, strSpec;
        string strWareHouse, strDepartString;

        TabContainer1.ActiveTabIndex = 0;

        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strGoodsCode = DL_GoodsType.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strGoodsType = DL_GoodsType.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();

        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strGoodsType = "%" + strGoodsType + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = LB_DepartString.Text.Trim();


        strHQL = "Select * From T_Goods  Where GoodsCode Like " + "'" + strGoodsCode + "'" + " and GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and Type Like " + "'" + strGoodsType + "'" + " and ModelNumber Like " + "'" + strModelNumber + "'" + " and Spec Like " + "'" + strSpec + "'";
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
                TB_ModelNumber.Text = item.ModelNumber.Trim();

                DL_Unit.SelectedValue = item.Unit;
           
                TB_Spec.Text = item.Specification;

                TB_Brand.Text = item.Brand;
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


            TB_SN.Text = goods.SN.Trim();
            TB_ModelNumber.Text = goods.ModelNumber.Trim();
            TB_Spec.Text = goods.Spec.Trim();
            TB_Brand.Text = goods.Manufacturer;

            DL_Unit.SelectedValue = goods.UnitName;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strBorrowNO = LB_BorrowNO.Text.Trim();
         
            int intWLNumber = LoadRelatedWL("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strBorrowNO));
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

            string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO, "0");
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

                strHQL = "From GoodsBorrowOrderDetail as goodsBorrowOrderDetail Where goodsBorrowOrderDetail.ID = " + strID;
                GoodsBorrowOrderDetailBLL goodsBorrowOrderDetailBLL = new GoodsBorrowOrderDetailBLL();
                lst = goodsBorrowOrderDetailBLL.GetAllGoodsBorrowOrderDetails(strHQL);

                GoodsBorrowOrderDetail goodsBorrowOrderDetail = (GoodsBorrowOrderDetail)lst[0];

                LB_ID.Text = strID;
                TB_GoodsCode.Text = goodsBorrowOrderDetail.GoodsCode.Trim();
                TB_GoodsName.Text = goodsBorrowOrderDetail.GoodsName.Trim();
                DL_GoodsType.SelectedValue = goodsBorrowOrderDetail.Type;
                TB_ModelNumber.Text = goodsBorrowOrderDetail.ModelNumber.Trim();
                TB_Spec.Text = goodsBorrowOrderDetail.Spec.Trim();
                TB_Brand.Text = goodsBorrowOrderDetail.Brand;

                NB_Number.Amount = goodsBorrowOrderDetail.Number;
                NB_Price.Amount = goodsBorrowOrderDetail.Price;
                DL_Unit.SelectedValue = goodsBorrowOrderDetail.UnitName;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = LoadRelatedWL("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strBorrowNO));

                if (intWLNumber > 0 & strToDoWLID == null)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    return;
                }
            
                strHQL = "Delete From T_GoodsBorrowOrderDetail Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadGoodsBorrowOrderDetail(strBorrowNO);

                    LB_ID.Text = "0";

                    //从流程中打开的业务单
                    //更改工作流关联的数据文件
                    strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO, "0");
                    if (strToDoWLID != null | strAllowFullEdit == "YES")
                    {
                        string strCmdText;
                        strCmdText = "select BorrowNO as DetailBorrowNO, * from T_GoodsBorrowOrder where BorrowNO = " + strBorrowNO;
                        if (strToDoWLID == null)
                        {
                            strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO);
                        }

                        if (strToDoWLID != null)
                        {
                            if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                        }

                        if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                        {
                            strCmdText = "select * from T_GoodsBorrowOrderDetail where BorrowNO = " + strBorrowNO;
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
        string strBorrowNO;

        strBorrowNO = LB_BorrowNO.Text.Trim();

        if (strBorrowNO == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strBorrowNO = LB_BorrowNO.Text.Trim();
        int intWLNumber = LoadRelatedWL("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strBorrowNO));
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
        string strBorrowNO, strGoodsCode, strGoodsName, strGoodsType, strSN, strModelNumber, strSpec, strUnitName;

        decimal deNumber, dePrice;

        strBorrowNO = LB_BorrowNO.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strGoodsType = DL_GoodsType.SelectedValue.Trim();

        strSN = TB_SN.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        dePrice = NB_Price.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();

        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
        else
        {
            GoodsBorrowOrderDetailBLL goodsBorrowOrderDetailBLL = new GoodsBorrowOrderDetailBLL();
            GoodsBorrowOrderDetail goodsBorrowOrderDetail = new GoodsBorrowOrderDetail();

            goodsBorrowOrderDetail.BorrowNO = int.Parse(strBorrowNO);
            goodsBorrowOrderDetail.GoodsCode = strGoodsCode;
            goodsBorrowOrderDetail.GoodsName = strGoodsName;
            goodsBorrowOrderDetail.Type = strGoodsType;
            if (strSN == "")
            {
                //strSN = strGoodsCode;
            }
            goodsBorrowOrderDetail.SN = strSN;
            goodsBorrowOrderDetail.ModelNumber = strModelNumber;

            goodsBorrowOrderDetail.Spec = strSpec;
            goodsBorrowOrderDetail.Brand = TB_Brand.Text;

            goodsBorrowOrderDetail.Number = deNumber;
            goodsBorrowOrderDetail.ReturnNumber = 0;
            goodsBorrowOrderDetail.Price = dePrice;
            goodsBorrowOrderDetail.Amount = deNumber * dePrice;
            goodsBorrowOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            goodsBorrowOrderDetail.UnitName = strUnitName;

            try
            {
                goodsBorrowOrderDetailBLL.AddGoodsBorrowOrderDetail(goodsBorrowOrderDetail);

                LB_ID.Text = ShareClass.GetMyCreatedMaxGoodsBorrowOrderDetailID().ToString();


                LoadGoodsBorrowOrderDetail(strBorrowNO);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;
                    strCmdText = "select BorrowNO as DetailBorrowNO, * from T_GoodsBorrowOrder where BorrowNO = " + strBorrowNO;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsBorrowOrderDetail where BorrowNO = " + strBorrowNO;
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
        string strBorrowNO, strGoodsCode, strGoodsName, strSN, strModelNumber, strSpec, strUnitName, strGoodsType;

        decimal deNumber, deOldNumber, dePrice;
        int intID;

        string strHQL;

        intID = int.Parse(LB_ID.Text);
        strBorrowNO = LB_BorrowNO.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strGoodsType = DL_GoodsType.SelectedValue.Trim();
        strSN = TB_SN.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        dePrice = NB_Price.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();


        if (strGoodsCode == "" | strGoodsName == "" | strSpec == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
        else
        {
            GoodsBorrowOrderDetailBLL goodsBorrowOrderDetailBLL = new GoodsBorrowOrderDetailBLL();
            strHQL = "From GoodsBorrowOrderDetail as goodsBorrowOrderDetail Where goodsBorrowOrderDetail.ID = " + intID.ToString();
            IList lst = goodsBorrowOrderDetailBLL.GetAllGoodsBorrowOrderDetails(strHQL);
            GoodsBorrowOrderDetail goodsBorrowOrderDetail = (GoodsBorrowOrderDetail)lst[0];

            deOldNumber = goodsBorrowOrderDetail.Number;

            goodsBorrowOrderDetail.BorrowNO = int.Parse(strBorrowNO);
            goodsBorrowOrderDetail.GoodsCode = strGoodsCode;
            goodsBorrowOrderDetail.GoodsName = strGoodsName;
            goodsBorrowOrderDetail.Type = strGoodsType;
            if (strSN == "")
            {
                //strSN = strGoodsCode;
            }
            goodsBorrowOrderDetail.SN = strSN;
            goodsBorrowOrderDetail.ModelNumber = strModelNumber;
            goodsBorrowOrderDetail.Spec = strSpec;
            goodsBorrowOrderDetail.Brand = TB_Brand.Text;

            goodsBorrowOrderDetail.Number = deNumber;
            goodsBorrowOrderDetail.Price = dePrice;
            goodsBorrowOrderDetail.Amount = deNumber * dePrice;
            goodsBorrowOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

            goodsBorrowOrderDetail.UnitName = strUnitName;

            try
            {
                goodsBorrowOrderDetailBLL.UpdateGoodsBorrowOrderDetail(goodsBorrowOrderDetail, intID);

                LoadGoodsBorrowOrderDetail(strBorrowNO);

                //从流程中打开的业务单
                //更改工作流关联的数据文件
                string strAllowFullEdit = ShareClass.GetWorkflowTemplateStepFullAllowEditValue("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO, "0");
                if (strToDoWLID != null | strAllowFullEdit == "YES")
                {
                    string strCmdText;
                    strCmdText = "select BorrowNO as DetailBorrowNO, * from T_GoodsBorrowOrder where BorrowNO = " + strBorrowNO;
                    if (strToDoWLID == null)
                    {
                        strToDoWLID = ShareClass.GetBusinessRelatedWorkFlowID("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), strBorrowNO);
                    }

                    if (strToDoWLID != null)
                    {
                        if (strToDoWLDetailID == null) { strToDoWLDetailID = "0"; }  ShareClass.UpdateWokflowRelatedXMLFile("MainTable", strToDoWLID, strToDoWLDetailID, strCmdText);
                    }

                    if (strToDoWLDetailID != null & strToDoWLDetailID != "0")
                    {
                        strCmdText = "select * from T_GoodsBorrowOrderDetail where BorrowNO = " + strBorrowNO;
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

    protected string SubmitApply()
    {
        string strApplyReason, strCmdText;

        string strBorrowNO, strXMLFileName, strXMLFile2;

        string strWLID, strTemName, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strBorrowNO = LB_BorrowNO.Text.Trim();

        strApplyReason = TB_BorrowReason.Text.Trim();

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim() + "');</script>");

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

            return "0";
        }

        XMLProcess xmlProcess = new XMLProcess();

        try
        {
            strXMLFileName = "MaterialBorrowing" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = strApplyReason;
            workFlow.WLType = "MaterialBorrowing";
            workFlow.Status = "New";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = ShareClass.GetUserName(strUserCode);
            workFlow.Description = strApplyReason;
            workFlow.XMLFile = strXMLFile2;
            workFlow.RelatedType = LanguageHandle.GetWord("WuLiao").ToString().Trim();
            workFlow.RelatedID = int.Parse(strBorrowNO);
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

                strCmdText = "select * from T_GoodsBorrowOrder where BorrowNO = " + strBorrowNO;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_GoodsApplication", strXMLFile2);

                LoadRelatedWL("MaterialBorrowing", LanguageHandle.GetWord("WuLiao").ToString().Trim(), int.Parse(strBorrowNO));

                BT_SubmitApply.Enabled = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZLSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                strWLID = "0";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZLSSCSB").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
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

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'MaterialBorrowing'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

    }

    protected void UpdateGoodsSOOrAONumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber;

        if (strSourceType == "GoodsSORecord")
        {
            strHQL = "Select  COALESCE(Sum(Number),0) From T_GoodsBorrowOrderDetail Where SourceType = 'GoodsSORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsBorrowOrderDetail");


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
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsBorrowOrderDetail Where SourceType = 'GoodsAORecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsBorrowOrderDetail");

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


    protected bool CheckBorrowNumber(string strFromGoodsID, decimal deBorrowNumber)
    {
        string strHQL;
        IList lst;

        decimal deGoodsNumber;

        strHQL = "From Goods as goods where goods.ID = " + strFromGoodsID;
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        Goods goods = (Goods)lst[0];

        deGoodsNumber = goods.Number;

        if (deGoodsNumber < deBorrowNumber)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void LoadGoodsBorrowOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsBorrowOrder as goodsBorrowOrder Where goodsBorrowOrder.OperatorCode = " + "'" + strUserCode + "'" + " Order By goodsBorrowOrder.BorrowNO DESC";
        
        //从流程中打开的业务单
        if (strToDoWLID != null & strWLBusinessID != null)
        {
            strHQL = "From GoodsBorrowOrder as goodsBorrowOrder Where goodsBorrowOrder.BorrowNO = " + strWLBusinessID;
        }

        GoodsBorrowOrderBLL goodsBorrowOrderBLL = new GoodsBorrowOrderBLL();
        lst = goodsBorrowOrderBLL.GetAllGoodsBorrowOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadGoodsBorrowOrderDetail(string strBorrowNO)
    {
        string strHQL;
        IList lst;

        strHQL = "From GoodsBorrowOrderDetail as goodsBorrowOrderDetail Where goodsBorrowOrderDetail.BorrowNO = " + strBorrowNO + " Order By goodsBorrowOrderDetail.ID ASC";
        GoodsBorrowOrderDetailBLL goodsBorrowOrderDetailBLL = new GoodsBorrowOrderDetailBLL();
        lst = goodsBorrowOrderDetailBLL.GetAllGoodsBorrowOrderDetails(strHQL);

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

    protected void LoadRelatedConstract(string strUserCode)
    {
        string strHQL;
        IList lst;


        strHQL = "from Constract as constract where  constract.Status not in ('Archived','Deleted') ";
        strHQL += " and (constract.RecorderCode = " + "'" + strUserCode + "'" + " Or constract.ConstractCode in (select constractRelatedUser.ConstractCode from ConstractRelatedUser as constractRelatedUser where constractRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql3.Text = strHQL;
    }

    protected int LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        return lst.Count;
    }

    public static void LoadWFTemplate(string strUserCode, string strWFType, DropDownList DL_TemName)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select TemName From T_WorkFlowTemplate Where Visible = 'YES' and Type = " + "'" + strWFType + "'" + " and Authority = 'All'";
        strHQL += " and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by CreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_TemName.DataSource = ds;
        DL_TemName.DataBind();
    }

}
