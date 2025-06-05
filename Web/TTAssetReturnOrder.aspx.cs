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

public partial class TTAssetReturnOrder : System.Web.UI.Page
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

            DLC_ReturnTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from AssetType as assetType Order by assetType.SortNumber ASC";
            AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
            lst = assetTypeBLL.GetAllAssetTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            ShareClass.LoadCurrencyType(DL_CurrencyType);

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadWareHouseListByAuthority(strDepartString);

            LoadAssetApplication(strUserCode);

            LoadAssetReturnOrder(strUserCode);

            if (DL_WareHouse.Items.Count > 0)
            {
                strWareHouse = DL_WareHouse.SelectedValue.Trim();
                LoadAllAssetByWareHouse(strWareHouse);
            }
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strROID;
            strROID = e.Item.Cells[2].Text.Trim();
            LB_ROID.Text = strROID;

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid5.Items.Count; i++)
                {
                    DataGrid5.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From AssetReturnOrder as assetReturnOrder Where assetReturnOrder.ROID = " + strROID;
                AssetReturnOrderBLL assetReturnOrderBLL = new AssetReturnOrderBLL();
                lst = assetReturnOrderBLL.GetAllAssetReturnOrders(strHQL);

                AssetReturnOrder assetReturnOrder = (AssetReturnOrder)lst[0];

                LB_ROID.Text = strROID;
                TB_ReturnName.Text = assetReturnOrder.ReturnName.Trim();
                NB_Amount.Amount = assetReturnOrder.Amount;
                DL_CurrencyType.SelectedValue = assetReturnOrder.CurrencyType;
                DLC_ReturnTime.Text = assetReturnOrder.ReturnTime.ToString("yyyy-MM-dd");
                TB_Applicant.Text = assetReturnOrder.Applicant.Trim();

                LoadAssetReturnDetail(strROID);


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strFromAssetID;
                decimal deOldNumber;
                
                try
                {
                    strHQL = "Delete From T_AssetReturnOrder Where ROID = " + strROID;
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Select Number,FromAssetID From T_AssetReturnDetail Where ROID = " + strROID;
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetReturnDetail");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strFromAssetID = ds.Tables[0].Rows[i][1].ToString();
                        deOldNumber = decimal.Parse(ds.Tables[0].Rows[i][0].ToString());

                        UpdateAssetNumberForDelete(strFromAssetID, deOldNumber);
                    }

                    strHQL = "Delete From T_AssetReturnDetail Where ROID = " + strROID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadAssetReturnOrder(strUserCode);
                    LoadAssetReturnDetail(strROID);

                

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

        if (strSourceType == "AssetAO")
        {
            BT_SelectAO.Visible = true;
        }
        else
        {
            BT_SelectAO.Visible = false;
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
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql2.Text;

        AssetBLL assetBLL = new AssetBLL();
        IList lst = assetBLL.GetAllAssets(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        AssetReturnOrderBLL assetReturnOrderBLL = new AssetReturnOrderBLL();
        IList lst = assetReturnOrderBLL.GetAllAssetReturnOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void DL_WareHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strWareHouse;

        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        TB_ToPosition.Text = strWareHouse;

        LoadAllAssetByWareHouse(strWareHouse);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }


    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_ROID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        LoadAssetReturnDetail("0");

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
        string strROID, strReturnName, strWareHouse, strApplicant, strReturnReason;
        DateTime dtReturnTime;

        strReturnName = TB_ReturnName.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);
        strApplicant = TB_Applicant.Text.Trim();
        strReturnReason = TB_ReturnReason.Text.Trim();


        try
        {
            AssetReturnOrderBLL assetReturnOrderBLL = new AssetReturnOrderBLL();
            AssetReturnOrder assetReturnOrder = new AssetReturnOrder();


            assetReturnOrder.ReturnName = strReturnName;
            assetReturnOrder.Amount = 0;
            assetReturnOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            assetReturnOrder.Applicant = strApplicant;
            assetReturnOrder.ReturnTime = dtReturnTime;
            assetReturnOrder.OperatorCode = strUserCode;
            assetReturnOrder.OperatorName = GetUserName(strUserCode);


            assetReturnOrderBLL.AddAssetReturnOrder(assetReturnOrder);

            strROID = ShareClass.GetMyCreatedMaxAssetROID(strUserCode).ToString();

            LB_ROID.Text = strROID;

            LoadAssetReturnOrder(strUserCode);
            LoadAssetReturnDetail(strROID);

         

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

        string strROID, strReturnName, strApplicant;
        DateTime dtReturnTime;


        strROID = LB_ROID.Text.Trim();
        strReturnName = TB_ReturnName.Text.Trim();
        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);
        strApplicant = TB_Applicant.Text.Trim();

        try
        {
            strHQL = "From AssetReturnOrder as assetReturnOrder Where assetReturnOrder.ROID = " + strROID;
            AssetReturnOrderBLL assetReturnOrderBLL = new AssetReturnOrderBLL();
            lst = assetReturnOrderBLL.GetAllAssetReturnOrders(strHQL);
            AssetReturnOrder assetReturnOrder = (AssetReturnOrder)lst[0];

            assetReturnOrder.Applicant = strApplicant;
            assetReturnOrder.ReturnTime = dtReturnTime;
            assetReturnOrder.ReturnName = strReturnName;
            assetReturnOrder.Amount = NB_Amount.Amount;
            assetReturnOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            assetReturnOrder.OperatorCode = strUserCode;
            assetReturnOrder.OperatorName = GetUserName(strUserCode);


            assetReturnOrderBLL.UpdateAssetReturnOrder(assetReturnOrder, int.Parse(strROID));

            LoadAssetReturnOrder(strUserCode);
            LoadAssetReturnDetail(strROID);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        }
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
        NB_Price.Amount = 0;

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
                try
                {
                    DL_Type.SelectedValue = item.SmallType;
                }
                catch
                {
                    DL_Type.SelectedValue = "";
                }
                DL_Unit.SelectedValue = item.Unit;
                TB_Spec.Text = item.Specification;
                NB_Price.Amount = item.SalePrice;
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
            NB_Price.Amount = asset.Price;
            DL_Unit.SelectedValue = asset.UnitName;
            TB_Manufacturer.Text = asset.Manufacturer.Trim();

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
        string strAOID, strROID, strApplicationAssetCode, strAssetCode, strAssetID;
        string strDepartString;
        decimal deEveryAssetNumber, deApplicationNumber, deAssetNumber;
        int i, j, k;

        DataSet ds0, ds1, ds2, ds3;

        strDepartString = LB_DepartString.Text.Trim();

        strAOID = LB_A0ID.Text.Trim();
        strROID = LB_ROID.Text.Trim();

        strHQL0 = " Select AssetCode,Number From T_AssetReturnDetail ";
        strHQL0 += " Where ROID = " + strROID;
        ds0 = ShareClass.GetDataSetFromSql(strHQL0, "T_AssetReturnDetail");
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
                                        strHQL4 = "Insert Into T_AssetReturnDetail(ROID,AssetCode,AssetName,Spec,Number,UnitName,FromPosition,FromAssetID,ToPosition,Comment,ModelNumber,Manufacturer)";
                                        strHQL4 += " Select " + strROID + ",AssetCode,AssetName,Spec," + deApplicationNumber.ToString() + ",UnitName,Position,ID,'','',ModelNumber,Manufacturer From T_Asset";
                                        strHQL4 += " Where ID = " + strAssetID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        strHQL4 = "Update T_Asset Set Number = " + (deEveryAssetNumber - deApplicationNumber).ToString() + " From T_Asset  ";
                                        strHQL4 += " Where ID = " + strAssetID;
                                        ShareClass.RunSqlCommand(strHQL4);

                                        deApplicationNumber = 0;
                                    }
                                    else
                                    {
                                        strHQL4 = "Insert Into T_AssetReturnDetail(ROID,AssetCode,AssetName,Spec,Number,UnitName,FromPosition,FromAssetID,ToPosition,Comment,ModelNumber,Manufacturer)";
                                        strHQL4 += " Select " + strROID + ",AssetCode,AssetName,Spec," + deEveryAssetNumber.ToString() + ",UnitName,Position,ID,'','',ModelNumber,Manufacturer From T_Asset";
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


            LoadAssetReturnDetail(strROID);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJZCTHDCG") + "')", true);

            //strHQL = "Insert Into T_AssetReturnDetail(ROID,AssetCode,AssetName,Spec,Number,UnitName,FromPosition,FromAssetID,ToPosition,Comment,ModelNumber,Manufacturer)";
            //strHQL += " Select " + strROID + ",A.AssetCode,A.AssetName,A.Spec,A.Number,A.Unit,B.Position,B.ID,'','',B.ModelNumber,B.Manufacturer From T_AssetApplicationDetail A,T_Asset B";
            //strHQL += " Where A.AssetCode = B.AssetCode  AND B.NUMBER >= A.NUMBER  and A.AAID = " + strAOID;
            //strHQL += " and B.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
            //strHQL += " and B.ID  IN (Select DISTINCT MAX(C.ID) From T_Asset C,T_AssetApplicationDetail D Where C.AssetCode = D.AssetCode Group By C.AssetCode, D.ID )";
            //strHQL += " and A.AssetCode not in (Select AssetCode From T_AssetReturnDetail Where ROID = " + strROID + ")";
            //ShareClass.RunSqlCommand(strHQL);


            //strHQL = "Update T_Asset Set Number = B.Number - A.Number From T_AssetReturnDetail A, T_Asset B ";
            //strHQL += " Where A.AssetCode = B.AssetCode  AND B.NUMBER >= A.NUMBER ";
            //strHQL += " and B.Position in (Select WHName From T_WareHouse Where BelongDepartCode in " + strDepartString + ")";
            //strHQL += " and B.ID  IN (Select DISTINCT MAX(C.ID) From T_Asset C,T_AssetReturnDetail D Where C.AssetCode = D.AssetCode Group By C.AssetCode, D.ID )";
            //strHQL += " and A.ROID = " + strROID;

            //ShareClass.RunSqlCommand(strHQL);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCTHDYYJLBNZLJC") + "')", true);
        }

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


            string strID = e.Item.Cells[2].Text.Trim();
            LB_ID.Text = strID;

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From AssetReturnDetail as assetReturnDetail Where assetReturnDetail.ID = " + strID;
                AssetReturnDetailBLL assetReturnDetailBLL = new AssetReturnDetailBLL();
                lst = assetReturnDetailBLL.GetAllAssetReturnDetails(strHQL);

                AssetReturnDetail assetReturnDetail = (AssetReturnDetail)lst[0];

                LB_ID.Text = strID;
                TB_AssetCode.Text = assetReturnDetail.AssetCode.Trim();
                TB_AssetName.Text = assetReturnDetail.AssetName.Trim();
                TB_ModelNumber.Text = assetReturnDetail.ModelNumber.Trim();
                TB_Spec.Text = assetReturnDetail.Spec.Trim();
                NB_Number.Amount = assetReturnDetail.Number;
                DL_Unit.SelectedValue = assetReturnDetail.UnitName;
                TB_Manufacturer.Text = assetReturnDetail.Manufacturer.Trim();

                TB_ToPosition.Text = assetReturnDetail.ToPosition.Trim();
                TB_ReturnReason.Text = assetReturnDetail.ReturnReason.Trim();

                DL_SourceType.SelectedValue = assetReturnDetail.SourceType.Trim();
                NB_SourceID.Amount = assetReturnDetail.SourceID;


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strAssetCode, strWareHouse;
                decimal deOldNumber;

                strAssetCode = TB_AssetCode.Text.Trim();
                strWareHouse = DL_WareHouse.SelectedValue.Trim();

                deOldNumber = NB_Number.Amount;

                strROID = LB_ROID.Text.Trim();

                strID = LB_ID.Text.Trim();
                strHQL = "Delete From T_AssetReturnDetail Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    //删除物料明细表相同记录
                    strHQL = "Delete From T_Asset Where ReturnDetailID =" + LB_ID.Text;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadAssetReturnDetail(strROID);

                    LoadAsset(strAssetCode, strWareHouse);
                    
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
        string strReturnDetailID, strROID, strAssetCode, strAssetName, strType, strModelNumber, strSpec, strUnitName, strManufacturer, strSourceType, strSourceID, strToPosition, strReturnReason;
        decimal deNumber;
        string strWareHouse, strCurrencyType;
        int intSourceID;
        decimal dePrice;
        DateTime dtReturnTime;

        strROID = LB_ROID.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        dePrice = NB_Price.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue.Trim();
        strToPosition = TB_ToPosition.Text.Trim();
        strSourceType = DL_SourceType.SelectedValue;
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());
        strReturnReason = TB_ReturnReason.Text.Trim();
        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);
        strToPosition = DL_WareHouse.SelectedValue.Trim();


        if (strAssetCode == "" | strAssetName == "" | strSpec == "" | strToPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
        else
        {
            AssetReturnDetailBLL assetReturnDetailBLL = new AssetReturnDetailBLL();
            AssetReturnDetail assetReturnDetail = new AssetReturnDetail();

            assetReturnDetail.ROID = int.Parse(strROID);
            assetReturnDetail.AssetCode = strAssetCode;
            assetReturnDetail.AssetName = strAssetName;
            assetReturnDetail.Type = strType;

            assetReturnDetail.ModelNumber = strModelNumber;

            assetReturnDetail.Spec = strSpec;
            assetReturnDetail.Number = deNumber;
            assetReturnDetail.UnitName = strUnitName;
            assetReturnDetail.Price = dePrice;
            assetReturnDetail.Amount = deNumber * dePrice;
            assetReturnDetail.CurrencyType = strCurrencyType;

            assetReturnDetail.Manufacturer = strManufacturer;
            assetReturnDetail.SourceType = strSourceType;
            assetReturnDetail.SourceID = intSourceID;
            assetReturnDetail.ToPosition = strToPosition;
            assetReturnDetail.ReturnReason = strReturnReason;

            try
            {
                assetReturnDetailBLL.AddAssetReturnDetail(assetReturnDetail);

                LB_ID.Text = ShareClass.GetMyCreatedMaxAssetReturnDetailID().ToString();
                strReturnDetailID = LB_ID.Text;

                //添加相同记录到物料表
                addAsset(strReturnDetailID, strAssetCode, strAssetName, deNumber, strUnitName,
                   strUserCode, strType, strSpec, strModelNumber, strToPosition, "", dePrice, strCurrencyType, dtReturnTime,
                   strManufacturer, strReturnReason, LB_ID.Text);

                LoadAssetReturnDetail(strROID);

                LoadAsset(strAssetCode, strWareHouse);

                NB_Amount.Amount = SumReturnOrderAmount(strROID);
                UpdateReturnOrderAmount(strROID, NB_Amount.Amount);

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

    public void addAsset(string strReturnDetailID, string strAssetCode, string strAssetName, decimal deNumber, string strUnitName,
       string strOwnerCode, string strType, string strSpec, string strModelNumber, string strPosition, string strIP, decimal dePrice, string strCurrencyType, DateTime dtBuyTime,
       string strManufacturer, string strMemo, string strAssetID)
    {
        AssetBLL assetBLL = new AssetBLL();
        Asset asset = new Asset();

        asset.CheckInID = 0;
        asset.CheckInDetailID = 0;
        asset.ReturnDetailID = int.Parse(strReturnDetailID);
        asset.AssetCode = strAssetCode;
        asset.AssetName = strAssetName;
        asset.Number = deNumber;
        asset.ReturnNumber = deNumber;
        asset.UnitName = strUnitName;
        asset.OwnerCode = strOwnerCode;
        try
        {
            asset.OwnerName = GetUserName(strOwnerCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC") + "')", true);
            return;
        }
        asset.Type = strType;
        asset.Spec = strSpec;
        asset.ModelNumber = strModelNumber;
        asset.Position = strPosition;
        asset.IP = strIP;
        asset.Price = dePrice;
        asset.CurrencyType = strCurrencyType;
        asset.BuyTime = dtBuyTime;
        asset.Manufacturer = strManufacturer;
        asset.Memo = strMemo;
        asset.Status = "InUse";

        //try
        //{
        assetBLL.AddAsset(asset);
        //}
        //catch
        //{
        //}
    }

    protected void UpdateDetail()
    {
        string strReturnDetailID, strROID, strAssetCode, strAssetName, strType, strModelNumber, strSpec, strUnitName, strManufacturer, strSourceType, strSourceID, strToPosition, strReturnReason, strCurrencyType;
        decimal deNumber;
        int intID, intSourceID;
        decimal dePrice;
        DateTime dtReturnTime;

        string strHQL;

        intID = int.Parse(LB_ID.Text);
        strROID = LB_ROID.Text.Trim();
        strReturnDetailID = LB_ID.Text.Trim();

        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        dePrice = NB_Price.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();

        strManufacturer = TB_Manufacturer.Text.Trim();

        strSourceType = DL_SourceType.SelectedValue;
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());

        strToPosition = TB_ToPosition.Text.Trim();
        strReturnReason = TB_ReturnReason.Text.Trim();

        dtReturnTime = DateTime.Parse(DLC_ReturnTime.Text);

        if (strAssetCode == "" | strAssetName == "" | strSpec == "" | strToPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
        else
        {
            AssetReturnDetailBLL assetReturnDetailBLL = new AssetReturnDetailBLL();
            AssetReturnDetail assetReturnDetail = new AssetReturnDetail();

            assetReturnDetail.ROID = int.Parse(strROID);
            assetReturnDetail.AssetCode = strAssetCode;
            assetReturnDetail.AssetName = strAssetName;
            assetReturnDetail.Type = strType;
            assetReturnDetail.ModelNumber = strModelNumber;
            assetReturnDetail.Spec = strSpec;
            assetReturnDetail.Number = deNumber;
            assetReturnDetail.UnitName = strUnitName;
            assetReturnDetail.Manufacturer = strManufacturer;
            assetReturnDetail.Price = dePrice;
            assetReturnDetail.Amount = deNumber * dePrice;
            assetReturnDetail.CurrencyType = strCurrencyType;

            assetReturnDetail.SourceType = strSourceType;
            assetReturnDetail.SourceID = intSourceID;

            assetReturnDetail.ToPosition = strToPosition;
            assetReturnDetail.ReturnReason = strReturnReason;

            try
            {
                assetReturnDetailBLL.UpdateAssetReturnDetail(assetReturnDetail, intID);


                //删除物料明细表相同记录
                strHQL = "Delete From T_Asset Where ReturnDetailID =" + LB_ID.Text;
                ShareClass.RunSqlCommand(strHQL);


                //添加相同记录到物料表
                addAsset(strReturnDetailID, strAssetCode, strAssetName, deNumber, strUnitName,
                   strUserCode, strType, strSpec, strModelNumber, strToPosition, "", dePrice, strCurrencyType, dtReturnTime,
                   strManufacturer, strReturnReason, LB_ID.Text);

                NB_Amount.Amount = SumReturnOrderAmount(strROID);
                UpdateReturnOrderAmount(strROID, NB_Amount.Amount);

                LoadAssetReturnDetail(strROID);


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


    protected void UpdateAssetNumberForAdd(string strFromAssetID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Asset Set Number = Number - " + deNumber.ToString() + " Where ID = " + strFromAssetID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected decimal SumReturnOrderAmount(string strROID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from AssetReturnDetail as assetReturnDetail where assetReturnDetail.ROID = " + strROID;
        AssetReturnDetailBLL assetReturnDetailBLL = new AssetReturnDetailBLL();
        lst = assetReturnDetailBLL.GetAllAssetReturnDetails(strHQL);

        AssetReturnDetail assetReturnDetail = new AssetReturnDetail();

        for (int i = 0; i < lst.Count; i++)
        {
            assetReturnDetail = (AssetReturnDetail)lst[i];
            deAmount += assetReturnDetail.Number * assetReturnDetail.Price;
        }

        return deAmount;
    }

    protected void UpdateReturnOrderAmount(string strROID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetReturnOrder as assetReturnOrder where assetReturnOrder.ROID = " + strROID;
        AssetReturnOrderBLL assetReturnOrderBLL = new AssetReturnOrderBLL();
        lst = assetReturnOrderBLL.GetAllAssetReturnOrders(strHQL);

        AssetReturnOrder assetReturnOrder = (AssetReturnOrder)lst[0];

        assetReturnOrder.Amount = deAmount;

        try
        {
            assetReturnOrderBLL.UpdateAssetReturnOrder(assetReturnOrder, int.Parse(strROID));
        }
        catch
        {
        }
    }

    protected void UpdateAssetNumberForDelete(string strFromAssetID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Asset Set Number = Number + " + deNumber.ToString() + " Where ID = " + strFromAssetID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected bool CheckReturnNumber(string strFromAssetID, decimal deReturnNumber)
    {
        string strHQL;
        IList lst;

        decimal deAssetNumber;

        strHQL = "From Asset as asset where asset.ID = " + strFromAssetID;
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);

        Asset asset = (Asset)lst[0];

        deAssetNumber = asset.Number;

        if (deAssetNumber < deReturnNumber)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void LoadAssetReturnOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From AssetReturnOrder as assetReturnOrder Where assetReturnOrder.OperatorCode = " + "'" + strUserCode + "'" + " Order By assetReturnOrder.ROID DESC";
        AssetReturnOrderBLL assetReturnOrderBLL = new AssetReturnOrderBLL();
        lst = assetReturnOrderBLL.GetAllAssetReturnOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadAssetReturnDetail(string strROID)
    {
        string strHQL;
        IList lst;

        strHQL = "From AssetReturnDetail as assetReturnDetail Where assetReturnDetail.ROID = " + strROID + " Order By assetReturnDetail.ID ASC";
        AssetReturnDetailBLL assetReturnDetailBLL = new AssetReturnDetailBLL();
        lst = assetReturnDetailBLL.GetAllAssetReturnDetails(strHQL);

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

    protected void LoadWareHouseListByAuthority(string strDepartString)
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


}
