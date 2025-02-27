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

public partial class TTAssetShipmentOrder : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;
        strUserCode = Session["UserCode"].ToString();
        string strWareHouse;

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产登记入库", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            DLC_ShipTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadWareHouseListByAuthority(strDepartString, DL_WareHouse);
            LoadWareHouseListByAuthority(strDepartString, DL_InWareHouse);
            LoadAssetApplication(strUserCode);

            LoadAssetShipmentOrder(strUserCode);

            if (DL_WareHouse.Items.Count > 0)
            {
                strWareHouse = DL_WareHouse.SelectedValue.Trim();
                LoadAllAssetByWareHouse(strWareHouse);
            }

            ShareClass.InitialInvolvedProjectTree(TreeView1, strUserCode);
        }
    }


    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strShipmentNO, strRelatedType, strSourceType;

            strShipmentNO = e.Item.Cells[2].Text.Trim();
            LB_ShipmentNO.Text = strShipmentNO;

            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "From AssetShipmentOrder as assetShipmentOrder Where assetShipmentOrder.ShipmentNO = " + strShipmentNO;
                AssetShipmentOrderBLL assetShipmentOrderBLL = new AssetShipmentOrderBLL();
                lst = assetShipmentOrderBLL.GetAllAssetShipmentOrders(strHQL);

                AssetShipmentOrder assetShipmentOrder = (AssetShipmentOrder)lst[0];

                try
                {
                    DL_WareHouse.SelectedValue = assetShipmentOrder.WareHouse;
                }
                catch
                {

                }

                try
                {
                    DL_RelatedType.SelectedValue = assetShipmentOrder.RelatedType;
                }
                catch
                {
                }

                try
                {
                    DL_SourceType.SelectedValue = assetShipmentOrder.SourceType;
                }
                catch
                {
                }

                LB_ShipmentNO.Text = strShipmentNO;
                DLC_ShipTime.Text = assetShipmentOrder.ShipTime.ToString("yyyy-MM-dd");
                TB_ShipmentReason.Text = assetShipmentOrder.ApplicationReason.Trim();
                TB_Applicant.Text = assetShipmentOrder.Applicant.Trim();
                NB_RelatedID.Amount = assetShipmentOrder.RelatedID;
                DL_SourceType.SelectedValue = assetShipmentOrder.SourceType.Trim();
                NB_SourceID.Amount = assetShipmentOrder.SourceID;

                LoadAssetShipmentDetail(strShipmentNO);

                strSourceType = assetShipmentOrder.SourceType.Trim();
                if (strSourceType == "AssetAO")
                {
                    BT_SelectAO.Visible = true;
                }
                else
                {
                    BT_SelectAO.Visible = false;
                }

                strRelatedType = assetShipmentOrder.RelatedType.Trim();
                if (strRelatedType == "Project")
                {
                    BT_RelatedProject.Visible = true;
                }
                else
                {
                    BT_RelatedProject.Visible = false;
                }

                //BT_Update.Enabled = false;
                //BT_Delete.Enabled = false;
                //BT_UpdatePO.Enabled = true;
                //BT_DeletePO.Enabled = true;

                //BT_New.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strFromAssetID;
                decimal deOldNumber;

                try
                {
                    strHQL = "Delete From T_AssetShipmentOrder Where ShipmentNO = " + strShipmentNO;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Select Number,FromAssetID From T_AssetShipmentDetail Where ShipmentNO = " + strShipmentNO;
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetShipmentDetail");

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strFromAssetID = ds.Tables[0].Rows[i][1].ToString();
                        deOldNumber = decimal.Parse(ds.Tables[0].Rows[i][0].ToString());

                        UpdateAssetNumberForDelete(strFromAssetID, deOldNumber);
                    }

                    strHQL = "Delete From T_AssetShipmentDetail Where ShipmentNO = " + strShipmentNO;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadAssetShipmentOrder(strUserCode);
                    LoadAssetShipmentDetail(strShipmentNO);

                    //BT_UpdatePO.Enabled = false;
                    //BT_DeletePO.Enabled = false;

                    //BT_New.Enabled = false;
                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;

                    LB_ShipmentNO.Text = "0";
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCKNCZMXJLJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_ShipmentNO.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        LoadAssetShipmentDetail("0");

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
        string strShipmentNO, strWareHouse, strApplicant, strShipReason, strRelatedType, strSourceType, strSourceID;
        DateTime dtShipTime;
        int intRelatedID;

        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        dtShipTime = DateTime.Parse(DLC_ShipTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strShipReason = TB_ShipmentReason.Text.Trim();
        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        intRelatedID = int.Parse(NB_RelatedID.Amount.ToString());
        strSourceType = DL_SourceType.SelectedValue.Trim();
        strSourceID = NB_SourceID.Amount.ToString();

        try
        {
            AssetShipmentOrderBLL assetShipmentOrderBLL = new AssetShipmentOrderBLL();
            AssetShipmentOrder assetShipmentOrder = new AssetShipmentOrder();

            assetShipmentOrder.WareHouse = strWareHouse;
            assetShipmentOrder.Applicant = strApplicant;
            assetShipmentOrder.ShipTime = dtShipTime;
            assetShipmentOrder.ApplicationReason = strShipReason;
            assetShipmentOrder.RelatedType = strRelatedType;
            assetShipmentOrder.RelatedID = intRelatedID;
            assetShipmentOrder.SourceType = strSourceType;
            assetShipmentOrder.SourceID = int.Parse(strSourceID);

            assetShipmentOrder.OperatorCode = strUserCode;
            assetShipmentOrder.OperatorName = GetUserName(strUserCode);

            assetShipmentOrderBLL.AddAssetShipmentOrder(assetShipmentOrder);

            strShipmentNO = ShareClass.GetMyCreatedMaxAssetShipmentNO(strUserCode).ToString();

            LB_ShipmentNO.Text = strShipmentNO;

            LoadAssetShipmentOrder(strUserCode);
            LoadAssetShipmentDetail(strShipmentNO);
            //BT_UpdatePO.Enabled = true;
            //BT_UpdatePO.Enabled = true;

            //BT_New.Enabled = true;

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

        string strShipmentNO, strWareHouse, strApplicant, strShipReason, strRelatedType, strSourceType, strSourceID;
        DateTime dtShipTime;
        int intRelatedID;

        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        dtShipTime = DateTime.Parse(DLC_ShipTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strShipReason = TB_ShipmentReason.Text.Trim();
        strRelatedType = DL_RelatedType.SelectedValue.Trim();
        intRelatedID = int.Parse(NB_RelatedID.Amount.ToString());

        strSourceType = DL_SourceType.SelectedValue.Trim();
        strSourceID = NB_SourceID.Amount.ToString();

        try
        {
            strHQL = "From AssetShipmentOrder as assetShipmentOrder Where assetShipmentOrder.ShipmentNO = " + strShipmentNO;
            AssetShipmentOrderBLL assetShipmentOrderBLL = new AssetShipmentOrderBLL();
            lst = assetShipmentOrderBLL.GetAllAssetShipmentOrders(strHQL);
            AssetShipmentOrder assetShipmentOrder = (AssetShipmentOrder)lst[0];

            assetShipmentOrder.WareHouse = strWareHouse;
            assetShipmentOrder.Applicant = strApplicant;
            assetShipmentOrder.ShipTime = dtShipTime;
            assetShipmentOrder.ApplicationReason = strShipReason;
            assetShipmentOrder.RelatedType = strRelatedType;
            assetShipmentOrder.RelatedID = intRelatedID;

            assetShipmentOrder.SourceType = strSourceType;
            assetShipmentOrder.SourceID = int.Parse(strSourceID);

            assetShipmentOrder.OperatorCode = strUserCode;
            assetShipmentOrder.OperatorName = GetUserName(strUserCode);

            assetShipmentOrderBLL.UpdateAssetShipmentOrder(assetShipmentOrder, int.Parse(strShipmentNO));

            LoadAssetShipmentOrder(strUserCode);
            LoadAssetShipmentDetail(strShipmentNO);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

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

        if (strSourceType == "AssetAO")
        {
            BT_SelectAO.Visible = true;
            //TabContainer1.ActiveTabIndex = 0;
        }
        else
        {
            BT_SelectAO.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

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

        }
        else
        {
            BT_RelatedProject.Visible = false;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

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

            LoadAssetApplicationDetail(strAAID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
    }


    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;

        AssetApplicationBLL assetApplicationBLL = new AssetApplicationBLL();
        IList lst = assetApplicationBLL.GetAllAssetApplications(strHQL);

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

            NB_RelatedID.Amount = int.Parse(strProjectID);

        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql2.Text;

        AssetBLL assetBLL = new AssetBLL();
        IList lst = assetBLL.GetAllAssets(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        AssetShipmentOrderBLL assetShipmentOrderBLL = new AssetShipmentOrderBLL();
        IList lst = assetShipmentOrderBLL.GetAllAssetShipmentOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DL_WareHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWareHouse;

        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        LoadAllAssetByWareHouse(strWareHouse);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DL_InWareHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWareHouse;

        strWareHouse = DL_InWareHouse.SelectedValue.Trim();
        TB_ToPosition.Text = strWareHouse;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }


    protected void BT_FindAsset_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strAssetCode, strAssetName, strModelNumber, strSpec;
        string strWareHouse, strDepartString;

        TabContainer1.ActiveTabIndex = 0;

        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strAssetCode = "%" + strAssetCode + "%";
        strAssetName = "%" + strAssetName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strDepartString = LB_DepartString.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();

        strHQL = "Select * From T_Asset  Where AssetCode Like " + "'" + strAssetCode + "'" + " and AssetName like " + "'" + strAssetName + "'";
        strHQL += " and ModelNumber Like " + "'" + strModelNumber + "'" + " and Spec Like " + "'" + strSpec + "'";
        strHQL += " and Number > 0";
        strHQL += " and Position = " + "'" + strWareHouse + "'";
        strHQL += " and Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order by Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Asset");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        strHQL = "From Item as item Where item.ItemCode Like " + "'" + strAssetCode + "'" + " and item.ItemName like " + "'" + strAssetName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Asset'";

        ItemBLL itemBLL = new ItemBLL();
        IList lst = itemBLL.GetAllItems(strHQL);

        DataGrid7.DataSource = lst;
        DataGrid7.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_AssetCode.Text = "";
        TB_AssetName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";

        NB_Number.Amount = 0;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid7.Items.Count; i++)
            {
                DataGrid7.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_AssetCode.Text = item.ItemCode;
                TB_AssetName.Text = item.ItemName;
                DL_Unit.SelectedValue = item.Unit;
                TB_Spec.Text = item.Specification;
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

            strHQL = "From Asset as asset where asset.ID = " + strID;
            AssetBLL assetBLL = new AssetBLL();
            lst = assetBLL.GetAllAssets(strHQL);

            Asset asset = (Asset)lst[0];

            TB_AssetCode.Text = asset.AssetCode.Trim();
            TB_AssetName.Text = asset.AssetName.Trim();
            TB_ModelNumber.Text = asset.ModelNumber.Trim();
            TB_Spec.Text = asset.Spec.Trim();
            NB_Number.Amount = asset.Number;
            DL_Unit.SelectedValue = asset.UnitName;
            TB_Manufacturer.Text = asset.Manufacturer.Trim();

            TB_FromPosition.Text = asset.Position.Trim();
            LB_FromAssetID.Text = asset.ID.ToString();

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

            strHQL = "from AssetApplicationDetail as assetApplicationDetail where assetApplicationDetail.ID = " + strID;
            AssetApplicationDetailBLL assetApplicationDetailBLL = new AssetApplicationDetailBLL();
            lst = assetApplicationDetailBLL.GetAllAssetApplicationDetails(strHQL);

            if (lst.Count > 0)
            {
                try
                {
                    AssetApplicationDetail assetApplicationDetail = (AssetApplicationDetail)lst[0];

                    TB_AssetCode.Text = assetApplicationDetail.AssetCode;
                    TB_AssetName.Text = assetApplicationDetail.AssetName.Trim();
                    TB_ModelNumber.Text = assetApplicationDetail.ModelNumber.Trim();
                    TB_Spec.Text = assetApplicationDetail.Spec.Trim();
                    NB_Number.Amount = assetApplicationDetail.Number;
                    DL_Unit.SelectedValue = assetApplicationDetail.Unit;

                    TB_Manufacturer.Text = assetApplicationDetail.Manufacturer.Trim();

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
        string strAOID, strShipmentNO, strApplicationAssetCode, strAssetCode, strAssetID;
        string strDepartString;
        decimal deEveryAssetNumber, deApplicationNumber, deAssetNumber;
        int i, j, k;

        DataSet ds0, ds1, ds2, ds3;

        strDepartString = LB_DepartString.Text.Trim();

        strAOID = LB_A0ID.Text.Trim();
        strShipmentNO = LB_ShipmentNO.Text.Trim();

        strHQL0 = " Select AssetCode,Number From T_AssetShipmentDetail ";
        strHQL0 += " Where ShipmentNO = " + strShipmentNO;
        ds0 = ShareClass.GetDataSetFromSql(strHQL0, "T_AssetShipmentDetail");
        if (ds0.Tables[0].Rows.Count == 0)
        {
            strHQL1 = "Select AssetCode,Number From T_AssetApplicationDetail Where AAID = " + strAOID;
            strHQL1 += " Order By ID ASC";
            ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_AssetApplicationDetail");
            for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                strApplicationAssetCode = ds1.Tables[0].Rows[i][0].ToString();
                deApplicationNumber = decimal.Parse(ds1.Tables[0].Rows[i][1].ToString());

                strHQL2 = "Select A.AssetCode,sum(A.Number) From T_Asset A,T_AssetApplicationDetail B ";
                strHQL2 += " Where A.AssetCode = B.AssetCode and A.Spec = B.Spec and A.ModelNumber = B.ModelNumber ";
                strHQL2 += " and A.AssetCode = " + "'" + strApplicationAssetCode + "'" + " and A.Number > 0 ";
                strHQL2 += " and A.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
                strHQL2 += " Group By A.AssetCode";
                ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_Asset");

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strAssetCode = ds2.Tables[0].Rows[0][0].ToString();
                    deAssetNumber = decimal.Parse(ds2.Tables[0].Rows[0][1].ToString());

                    if (deApplicationNumber <= deAssetNumber)
                    {
                        strHQL3 = " Select A.ID, A.AssetCode,A.Number From T_Asset A,T_AssetApplicationDetail B ";
                        strHQL3 += " Where A.AssetCode = B.AssetCode and A.Spec = B.Spec and A.ModelNumber = B.ModelNumber ";
                        strHQL3 += " and A.AssetCode = " + "'" + strApplicationAssetCode + "'" + " and A.Number > 0 ";
                        strHQL3 += " and A.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
                        strHQL3 += " Order By A.Number ASC";
                        ds3 = ShareClass.GetDataSetFromSql(strHQL3, "T_Asset");

                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            for (j = 0; j < ds3.Tables[0].Rows.Count; j++)
                            {
                                strAssetID = ds3.Tables[0].Rows[j][0].ToString();
                                deEveryAssetNumber = decimal.Parse(ds3.Tables[0].Rows[j][2].ToString());

                                if (deApplicationNumber > 0)
                                {
                                    if (deApplicationNumber <= deEveryAssetNumber)
                                    {
                                        strHQL4 = "Insert Into T_AssetShipmentDetail(ShipmentNO,AssetCode,AssetName,Spec,Number,UnitName,FromPosition,FromAssetID,ToPosition,Comment,ModelNumber,Manufacturer)";
                                        strHQL4 += " Select " + strShipmentNO + ",AssetCode,AssetName,Spec," + deApplicationNumber.ToString() + ",UnitName,Position,ID,'','',ModelNumber,Manufacturer From T_Asset";
                                        strHQL4 += " Where ID = " + strAssetID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        strHQL4 = "Update T_Asset Set Number = " + (deEveryAssetNumber - deApplicationNumber).ToString() + " From T_Asset  ";
                                        strHQL4 += " Where ID = " + strAssetID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        deApplicationNumber = 0;
                                    }
                                    else
                                    {
                                        strHQL4 = "Insert Into T_AssetShipmentDetail(ShipmentNO,AssetCode,AssetName,Spec,Number,UnitName,FromPosition,FromAssetID,ToPosition,Comment,ModelNumber,Manufacturer)";
                                        strHQL4 += " Select " + strShipmentNO + ",AssetCode,AssetName,Spec," + deEveryAssetNumber.ToString() + ",UnitName,Position,ID,'','',ModelNumber,Manufacturer From T_Asset";
                                        strHQL4 += " Where ID = " + strAssetID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        strHQL4 = "Update T_Asset Set Number = 0 " + " From T_Asset  ";
                                        strHQL4 += " Where ID = " + strAssetID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        deApplicationNumber -= deEveryAssetNumber;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            LoadAssetShipmentDetail(strShipmentNO);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJZCCKDCG").ToString().Trim() + "')", true);

            //strHQL = "Insert Into T_AssetShipmentDetail(ShipmentNO,AssetCode,AssetName,Spec,Number,UnitName,FromPosition,FromAssetID,ToPosition,Comment,ModelNumber,Manufacturer)";
            //strHQL += " Select " + strShipmentNO + ",A.AssetCode,A.AssetName,A.Spec,A.Number,A.Unit,B.Position,B.ID,'','',B.ModelNumber,B.Manufacturer From T_AssetApplicationDetail A,T_Asset B";
            //strHQL += " Where A.AssetCode = B.AssetCode  AND B.NUMBER >= A.NUMBER  and A.AAID = " + strAOID;
            //strHQL += " and B.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
            //strHQL += " and B.ID  IN (Select DISTINCT MAX(C.ID) From T_Asset C,T_AssetApplicationDetail D Where C.AssetCode = D.AssetCode Group By C.AssetCode, D.ID )";
            //strHQL += " and A.AssetCode not in (Select AssetCode From T_AssetShipmentDetail Where ShipmentNO = " + strShipmentNO + ")";
            //ShareClass.RunSqlCommand(strHQL);


            //strHQL = "Update T_Asset Set Number = B.Number - A.Number From T_AssetShipmentDetail A, T_Asset B ";
            //strHQL += " Where A.AssetCode = B.AssetCode  AND B.NUMBER >= A.NUMBER ";
            //strHQL += " and B.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
            //strHQL += " and B.ID  IN (Select DISTINCT MAX(C.ID) From T_Asset C,T_AssetShipmentDetail D Where C.AssetCode = D.AssetCode Group By C.AssetCode, D.ID )";
            //strHQL += " and A.ShipmentNO = " + strShipmentNO;

            //ShareClass.RunSqlCommand(strHQL);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCCKDYYJLBNZLJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);


    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID;

            strID = e.Item.Cells[3].Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "From AssetShipmentDetail as assetShipmentDetail Where assetShipmentDetail.ID = " + strID;
                AssetShipmentDetailBLL assetShipmentDetailBLL = new AssetShipmentDetailBLL();
                lst = assetShipmentDetailBLL.GetAllAssetShipmentDetails(strHQL);

                AssetShipmentDetail assetShipmentDetail = (AssetShipmentDetail)lst[0];

                LB_ID.Text = strID;
                TB_AssetCode.Text = assetShipmentDetail.AssetCode.Trim();
                TB_AssetName.Text = assetShipmentDetail.AssetName.Trim();
                TB_ModelNumber.Text = assetShipmentDetail.ModelNumber.Trim();
                TB_Spec.Text = assetShipmentDetail.Spec.Trim();
                NB_Number.Amount = assetShipmentDetail.Number;
                DL_Unit.SelectedValue = assetShipmentDetail.UnitName;
                TB_Manufacturer.Text = assetShipmentDetail.Manufacturer.Trim();

                TB_ToPosition.Text = assetShipmentDetail.ToPosition.Trim();
                TB_FromPosition.Text = assetShipmentDetail.FromPosition;

                LB_FromAssetID.Text = assetShipmentDetail.FromAssetID.ToString();
                TB_Comment.Text = assetShipmentDetail.Comment.Trim();

                //BT_New.Enabled = true;
                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }


            if (e.CommandName == "Delete")
            {
                string strShipmentNO, strFromAssetID, strAssetCode, strWareHouse;
                decimal deOldNumber;

                strAssetCode = TB_AssetCode.Text.Trim();
                strWareHouse = DL_WareHouse.SelectedValue.Trim();
                strFromAssetID = LB_FromAssetID.Text.Trim();
                deOldNumber = NB_Number.Amount;

                strShipmentNO = LB_ShipmentNO.Text.Trim();

                strHQL = "Delete From T_AssetShipmentDetail Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    //更改资产库存原位置数量
                    UpdateAssetNumberForDelete(strFromAssetID, deOldNumber);

                    LoadAssetShipmentDetail(strShipmentNO);

                    LoadAsset(strAssetCode, strWareHouse);

                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;

                    LB_ID.Text = "0";

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
        string strShipmentNO, strAssetCode, strAssetName, strModelNumber, strSpec, strUnitName, strManufacturer, strFromPosition, strToPosition, strComment;
        decimal deNumber;
        string strFromAssetID, strWareHouse;

        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        strFromAssetID = LB_FromAssetID.Text;
        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        strFromPosition = TB_FromPosition.Text.Trim();
        strToPosition = TB_ToPosition.Text.Trim();
        strComment = TB_Comment.Text.Trim();

        if (strFromAssetID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWZYCKDZCJC").ToString().Trim() + "')", true);
            return;
        }

        if (CheckShipNumber(strFromAssetID, deNumber) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCKSLBNDYCZCKCSLJC").ToString().Trim() + "')", true);
            return;
        }


        if (strFromAssetID == "" | strAssetCode == "" | strAssetName == "" | strSpec == "" | strFromPosition == "" | strToPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            AssetShipmentDetailBLL assetShipmentDetailBLL = new AssetShipmentDetailBLL();
            AssetShipmentDetail assetShipmentDetail = new AssetShipmentDetail();

            assetShipmentDetail.ShipmentNO = int.Parse(strShipmentNO);
            assetShipmentDetail.AssetCode = strAssetCode;
            assetShipmentDetail.AssetName = strAssetName;
            assetShipmentDetail.ModelNumber = strModelNumber;
            assetShipmentDetail.Manufacturer = strManufacturer;
            assetShipmentDetail.Spec = strSpec;
            assetShipmentDetail.Number = deNumber;
            assetShipmentDetail.UnitName = strUnitName;
            assetShipmentDetail.FromPosition = strFromPosition;
            assetShipmentDetail.FromAssetID = int.Parse(strFromAssetID);
            assetShipmentDetail.ToPosition = strToPosition;
            assetShipmentDetail.Comment = strComment;

            try
            {
                assetShipmentDetailBLL.AddAssetShipmentDetail(assetShipmentDetail);

                LB_ID.Text = ShareClass.GetMyCreatedMaxAssetShipmentDetailID().ToString();

                //更新对应资产记录数量
                UpdateAssetNumberForAdd(strFromAssetID, deNumber);

                LoadAssetShipmentDetail(strShipmentNO);

                LoadAsset(strAssetCode, strWareHouse);

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }

    protected void UpdateDetail()
    {
        string strShipmentNO, strAssetCode, strAssetName, strModelNumber, strSpec, strUnitName, strManufacturer, strFromPosition, strToPosition, strComment;
        decimal deNumber, deOldNumber;
        int intID;
        string strFromAssetID, strWareHouse;
        string strHQL;

        strFromAssetID = LB_FromAssetID.Text.Trim();

        intID = int.Parse(LB_ID.Text);
        strShipmentNO = LB_ShipmentNO.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        strFromPosition = TB_FromPosition.Text.Trim();
        strToPosition = TB_ToPosition.Text.Trim();
        strComment = TB_Comment.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();

        if (strFromAssetID == "" | strAssetCode == "" | strAssetName == "" | strSpec == "" | strFromPosition == "" | strToPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            AssetShipmentDetailBLL assetShipmentDetailBLL = new AssetShipmentDetailBLL();
            strHQL = "From AssetShipmentDetail as assetShipmentDetail Where assetShipmentDetail.ID = " + intID.ToString();
            IList lst = assetShipmentDetailBLL.GetAllAssetShipmentDetails(strHQL);
            AssetShipmentDetail assetShipmentDetail = (AssetShipmentDetail)lst[0];

            deOldNumber = assetShipmentDetail.Number;

            assetShipmentDetail.ShipmentNO = int.Parse(strShipmentNO);
            assetShipmentDetail.AssetCode = strAssetCode;
            assetShipmentDetail.AssetName = strAssetName;
            assetShipmentDetail.ModelNumber = strModelNumber;
            assetShipmentDetail.Spec = strSpec;
            assetShipmentDetail.Number = deNumber;
            assetShipmentDetail.UnitName = strUnitName;
            assetShipmentDetail.Manufacturer = strManufacturer;
            assetShipmentDetail.FromPosition = strFromPosition;
            assetShipmentDetail.FromAssetID = int.Parse(strFromAssetID);
            assetShipmentDetail.ToPosition = strToPosition;
            assetShipmentDetail.Comment = strComment;

            LoadAsset(strAssetCode, strWareHouse);

            try
            {
                assetShipmentDetailBLL.UpdateAssetShipmentDetail(assetShipmentDetail, intID);

                //更改资产原位置库存数量
                UpdateAssetNumberForUpdate(strFromAssetID, deNumber, deOldNumber);

                LoadAssetShipmentDetail(strShipmentNO);


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }


    protected void UpdateAssetNumberForAdd(string strFromAssetID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Asset Set Number = Number - " + deNumber.ToString() + " Where ID = " + strFromAssetID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateAssetNumberForUpdate(string strFromAssetID, decimal deNumber, decimal deOldNumber)
    {
        string strHQL;

        strHQL = "Update T_Asset Set Number = Number - " + (deNumber - deOldNumber).ToString() + " Where ID = " + strFromAssetID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateAssetNumberForDelete(string strFromAssetID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Asset Set Number = Number + " + deNumber.ToString() + " Where ID = " + strFromAssetID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected bool CheckShipNumber(string strFromAssetID, decimal deShipNumber)
    {
        string strHQL;
        IList lst;

        decimal deAssetNumber;

        strHQL = "From Asset as asset where asset.ID = " + strFromAssetID;
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);

        Asset asset = (Asset)lst[0];

        deAssetNumber = asset.Number;

        if (deAssetNumber < deShipNumber)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void LoadAssetShipmentOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From AssetShipmentOrder as assetShipmentOrder Where assetShipmentOrder.OperatorCode = " + "'" + strUserCode + "'" + " Order By assetShipmentOrder.ShipmentNO DESC";
        AssetShipmentOrderBLL assetShipmentOrderBLL = new AssetShipmentOrderBLL();
        lst = assetShipmentOrderBLL.GetAllAssetShipmentOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadAssetShipmentDetail(string strShipmentNO)
    {
        string strHQL;
        IList lst;

        strHQL = "From AssetShipmentDetail as assetShipmentDetail Where assetShipmentDetail.ShipmentNO = " + strShipmentNO + " Order By assetShipmentDetail.ID ASC";
        AssetShipmentDetailBLL assetShipmentDetailBLL = new AssetShipmentDetailBLL();
        lst = assetShipmentDetailBLL.GetAllAssetShipmentDetails(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    protected void LoadAsset(string strAssetCode, string strWareHouse)
    {
        string strHQL;
        IList lst;

        strHQL = "From Asset as asset Where asset.Number > 0 and asset.AssetCode = " + "'" + strAssetCode + "'";
        strHQL += " and asset.Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by asset.ID ASC";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        TB_AssetName.Focus();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadAllAssetByWareHouse(string strWareHouse)
    {
        string strHQL;
        IList lst;

        strHQL = "From Asset as asset Where asset.Number > 0 and asset.Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by asset.ID ASC";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadWareHouseListByAuthority(string strDepartString, DropDownList DL_WareHouse)
    {
        string strHQL;


        strHQL = " Select WHName From T_WareHouse Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By SortNumber DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse");

        DL_WareHouse.DataSource = ds;
        DL_WareHouse.DataBind();

        DL_WareHouse.Items.Insert(0, new ListItem("--Select--", ""));

    }

    protected void LoadAssetApplication(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "from AssetApplication as assetApplication where assetApplication.ApplicantCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order by assetApplication.AAID DESC";
        AssetApplicationBLL assetApplicationBLL = new AssetApplicationBLL();
        lst = assetApplicationBLL.GetAllAssetApplications(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql3.Text = strHQL;
    }

    protected void LoadAssetApplicationDetail(string strAAID)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetApplicationDetail as assetApplicationDetail where assetApplicationDetail.AAID = " + strAAID;
        AssetApplicationDetailBLL assetApplicationDetailBLL = new AssetApplicationDetailBLL();
        lst = assetApplicationDetailBLL.GetAllAssetApplicationDetails(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
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



    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            IList lst;

            string strAssetCode = TB_AssetCode.Text.Trim();

            strHQL = "From Asset as asset where asset.AssetCode = '" + strAssetCode + "'";
            AssetBLL assetBLL = new AssetBLL();
            lst = assetBLL.GetAllAssets(strHQL);

            if (lst != null && lst.Count > 0)
            {
                Asset asset = (Asset)lst[0];

                TB_AssetCode.Text = asset.AssetCode.Trim();
                TB_AssetName.Text = asset.AssetName.Trim();
                TB_ModelNumber.Text = asset.ModelNumber.Trim();
                TB_Spec.Text = asset.Spec.Trim();
                NB_Number.Amount = asset.Number;
                DL_Unit.SelectedValue = asset.UnitName;
                TB_Manufacturer.Text = asset.Manufacturer.Trim();

                TB_FromPosition.Text = asset.Position.Trim();
                LB_FromAssetID.Text = asset.ID.ToString();
            }
        }
        catch (Exception ex) { }
    }
}
