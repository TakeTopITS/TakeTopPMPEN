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

using TakeTopSecurity;

public partial class TTMakeAsset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strUserName;

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

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            DLC_BuyTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_CheckInTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

            ShareClass.LoadVendorList(DL_VendorList, strUserCode);

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

            LoadWareHouseListByAuthority(strUserCode);

            LoadAssetCheckInOrder(strUserCode);
            LoadAssetPurchaseOrder(strUserCode);

            LB_OwnerCode.Text = strUserCode;
            LB_OwnerName.Text = strUserName;
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strUserCode = LB_UserCode.Text;
            string strCIOID, strSourceType;
            int intSourceID;

            DateTime dtCheckInTime, dtCurrentTime;

            strCIOID = e.Item.Cells[2].Text.Trim();
            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "From AssetCheckInOrder as assetCheckInOrder Where assetCheckInOrder.CheckInID = " + strCIOID;
                AssetCheckInOrderBLL assetCheckInOrderBLL = new AssetCheckInOrderBLL();
                lst = assetCheckInOrderBLL.GetAllAssetCheckInOrders(strHQL);

                AssetCheckInOrder assetCheckInOrder = (AssetCheckInOrder)lst[0];

                strSourceType = assetCheckInOrder.SourceType.Trim();
                intSourceID = assetCheckInOrder.SourceID;
                LB_CheckInID.Text = strCIOID;

                try
                {
                    DL_SourceType.SelectedValue = assetCheckInOrder.SourceType;
                }
                catch
                {
                }

                try
                {
                    DL_CurrencyType.SelectedValue = assetCheckInOrder.CurrencyType;
                }
                catch
                {
                }

                try
                {
                    DL_WareHouse.SelectedValue = assetCheckInOrder.WareHouse.Trim();
                }
                catch
                {
                }

                NB_SourceID.Amount = assetCheckInOrder.SourceID;
                NB_Amount.Amount = assetCheckInOrder.Amount;
                DLC_CheckInTime.Text = assetCheckInOrder.CheckInDate.ToString("yyyy-MM-dd");
                LB_CheckInTime.Text = assetCheckInOrder.CheckInDate.ToString("yyyy-MM-dd");

                LoadAssetCheckInOrderDetail(strCIOID);

                if (strSourceType == "PurchaseOrder")
                {
                    LoadAssetPurchaseOrderDetail(intSourceID.ToString());

                    BT_Select.Visible = true;
                }
                else
                {
                    BT_Select.Visible = false;
                }

                dtCurrentTime = DateTime.Now;
                dtCheckInTime = assetCheckInOrder.CheckInDate;
                TimeSpan ts = dtCurrentTime - dtCheckInTime;
                if (ts.Days == 0)
                {
                    //BT_UpdateCIO.Enabled = true;
                    //BT_DeleteCIO.Enabled = true;

                    //BT_New.Enabled = true;
                }
                else
                {
                    //BT_UpdateCIO.Enabled = false;
                    //BT_DeleteCIO.Enabled = false;

                    //BT_New.Enabled = false;
                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;
                }
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strCreatorCode = LB_UserCode.Text.Trim();

                strHQL = "Delete From T_AssetCheckInOrder Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_Asset Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                LoadAssetCheckInOrder(strCreatorCode);
                LoadAssetCheckInOrderDetail(strCIOID);

                //BT_UpdateCIO.Enabled = false;
                //BT_DeleteCIO.Enabled = false;

                //BT_New.Enabled = false;
                //BT_Update.Enabled = false;
                //BT_Delete.Enabled = false;
            }
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_CheckInID.Text = "";

        BT_NewMain.Visible = true;
        BT_NewDetail.Visible = true;

        LoadAssetCheckInOrderDetail("0");

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
        string strCreatorCode, strCreatorName;
        int intSourceID;
        decimal deAmount;
        DateTime dtCheckInDate;


        strSourceType = DL_SourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = LB_UserName.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue;
        dtCheckInDate = DateTime.Parse(DLC_CheckInTime.Text);
        deAmount = NB_Amount.Amount;

        AssetCheckInOrderBLL assetCheckInOrderBLL = new AssetCheckInOrderBLL();
        AssetCheckInOrder assetCheckInOrder = new AssetCheckInOrder();

        assetCheckInOrder.CheckInDate = dtCheckInDate;
        assetCheckInOrder.SourceType = strSourceType;
        assetCheckInOrder.SourceID = intSourceID;
        assetCheckInOrder.WareHouse = strWareHouse;
        assetCheckInOrder.CreatorCode = strCreatorCode;
        assetCheckInOrder.CreatorName = strCreatorName;
        assetCheckInOrder.Amount = deAmount;
        assetCheckInOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

        try
        {
            assetCheckInOrderBLL.AddAssetCheckInOrder(assetCheckInOrder);

            strCIOID = ShareClass.GetMyCreatedMaxAssetCheckInID(strCreatorCode);
            LB_CheckInID.Text = strCIOID;
            LB_CheckInTime.Text = dtCheckInDate.ToString("yyyy-MM-dd");

            LoadAssetCheckInOrder(strCreatorCode);
            LoadAssetCheckInOrderDetail(strCIOID);

            //BT_UpdateCIO.Enabled = true;
            //BT_DeleteCIO.Enabled = true;

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

        string strCIOID, strSourceType, strWareHouse;
        string strCreatorCode, strCreatorName;
        int intSourceID;
        decimal deAmount;
        DateTime dtCheckInDate;


        strCIOID = LB_CheckInID.Text.Trim();

        strSourceType = DL_SourceType.SelectedValue.Trim();
        intSourceID = int.Parse(NB_SourceID.Amount.ToString());
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = LB_UserName.Text.Trim();
        strWareHouse = DL_WareHouse.SelectedValue;
        dtCheckInDate = DateTime.Parse(DLC_CheckInTime.Text);
        deAmount = NB_Amount.Amount;

        strHQL = "From AssetCheckInOrder as assetCheckInOrder Where assetCheckInOrder.CheckInID = " + strCIOID;
        AssetCheckInOrderBLL assetCheckInOrderBLL = new AssetCheckInOrderBLL();
        lst = assetCheckInOrderBLL.GetAllAssetCheckInOrders(strHQL);

        AssetCheckInOrder assetCheckInOrder = (AssetCheckInOrder)lst[0];

        assetCheckInOrder.CheckInDate = dtCheckInDate;
        assetCheckInOrder.SourceType = strSourceType;
        assetCheckInOrder.SourceID = intSourceID;
        assetCheckInOrder.WareHouse = strWareHouse;
        assetCheckInOrder.CreatorCode = strCreatorCode;
        assetCheckInOrder.CreatorName = strCreatorName;
        assetCheckInOrder.Amount = deAmount;
        assetCheckInOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();

        try
        {
            assetCheckInOrderBLL.UpdateAssetCheckInOrder(assetCheckInOrder, int.Parse(strCIOID));
            //Liujp 2013-07-17 更新资产登记入库表时，更新资产表中仓库字段
            UpdateAssetPositionByAssetCheckInOrder(int.Parse(strCIOID), strWareHouse);

            LoadAssetCheckInOrder(strCreatorCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    /// <summary>
    /// Liujp 2013-07-17 更新资产登记入库表时，更新资产表中仓库字段
    /// </summary>
    /// <param name="assetCheckInOrderId"></param>
    /// <param name="strPosition"></param>
    protected void UpdateAssetPositionByAssetCheckInOrder(int assetCheckInOrderId, string strPosition)
    {
        AssetBLL assetBLL = new AssetBLL();
        string strHQL = "From Asset as asset Where asset.CheckInID = " + assetCheckInOrderId;
        IList lst = assetBLL.GetAllAssets(strHQL);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                Asset asset = (Asset)lst[i];
                asset.Position = strPosition;
                assetBLL.UpdateAsset(asset, asset.ID);
            }
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
            strID = e.Item.Cells[3].Text.Trim();
            strHQL = "from AssetCheckInOrderDetail as assetCheckInOrderDetail where assetCheckInOrderDetail.ID = " + "'" + strID + "'";

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
                lst = assetCheckInOrderDetailBLL.GetAllAssetCheckInOrderDetails(strHQL);
                AssetCheckInOrderDetail assetCheckInOrderDetail = (AssetCheckInOrderDetail)lst[0];

                try
                {
                    DL_Unit.SelectedValue = assetCheckInOrderDetail.UnitName;
                }
                catch
                {
                }
                try
                {
                    DL_Type.SelectedValue = assetCheckInOrderDetail.Type;
                }
                catch
                {
                }

                LB_ID.Text = assetCheckInOrderDetail.ID.ToString();
                TB_AssetCode.Text = assetCheckInOrderDetail.AssetCode.Trim();
                LB_OwnerCode.Text = assetCheckInOrderDetail.OwnerCode;

                LB_OwnerName.Visible = true;
                LB_OwnerName.Text = ShareClass.GetUserName(assetCheckInOrderDetail.OwnerCode);

                TB_AssetCode.Text = assetCheckInOrderDetail.AssetCode;
                TB_AssetName.Text = assetCheckInOrderDetail.AssetName;
                NB_Number.Amount = assetCheckInOrderDetail.CheckInNumber;
               
                TB_Spec.Text = assetCheckInOrderDetail.Spec;
                TB_IP.Text = assetCheckInOrderDetail.IP;
                NB_Price.Amount = assetCheckInOrderDetail.Price;
                DLC_BuyTime.Text = assetCheckInOrderDetail.BuyTime.ToString("yyyy-MM-dd");
               
                TB_ModelNumber.Text = assetCheckInOrderDetail.ModelNumber;
                TB_Manufacturer.Text = assetCheckInOrderDetail.Manufacturer;
                TB_Memo.Text = assetCheckInOrderDetail.Memo.Trim();

                HL_UserRecord.NavigateUrl = "TTAssetUserRecordList.aspx?ID=" + strID;

                BT_TakePhoto.Enabled = true;
                BT_DeletePhoto.Enabled = true;

                IM_ItemPhoto.ImageUrl = GetAssetPhotoURL(assetCheckInOrderDetail.AssetCode, strID);
                HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
                TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;

                dtCheckInTime = DateTime.Parse(LB_CheckInTime.Text.Trim());
                dtCurrentTime = DateTime.Now;
                TimeSpan ts = dtCurrentTime - dtCheckInTime;
                if (ts.Days == 0)
                {
                    //BT_New.Enabled = true;
                    //BT_Update.Enabled = true;
                    //BT_Delete.Enabled = true;
                }
                else
                {
                    //BT_New.Enabled = false;
                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }


            if (e.CommandName == "Delete")
            {
                string strCIOID = LB_CheckInID.Text.Trim();

                AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
                strHQL = "from AssetCheckInOrderDetail as assetCheckInOrderDetail where assetCheckInOrderDetail.ID = " + strID;
                lst = assetCheckInOrderDetailBLL.GetAllAssetCheckInOrderDetails(strHQL);
                AssetCheckInOrderDetail assetCheckInOrderDetail = (AssetCheckInOrderDetail)lst[0];

                try
                {
                    assetCheckInOrderDetailBLL.DeleteAssetCheckInOrderDetail(assetCheckInOrderDetail);

                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;

                    BT_TakePhoto.Enabled = false;
                    BT_DeletePhoto.Enabled = false;

                    LoadAssetCheckInOrderDetail(strCIOID);

                    //删除资产明细表相同记录
                    strHQL = "Delete From T_Asset Where CheckInDetailID =" + LB_ID.Text;
                    ShareClass.RunSqlCommand(strHQL);

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

        string strCIOID, strOwnerCode, strType, strAssetCode, strAssetName, strModelNumber, strSpec, strManufacturer, strIP, strPosition, strMemo, strCurrencyType;
        DateTime dtBuyTime;
        decimal dePrice;

        string strUserCode = LB_UserCode.Text;

        string strUnitName;
        decimal deNumber;

        strCIOID = LB_CheckInID.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        strPosition = DL_WareHouse.SelectedValue.Trim();
        strIP = TB_IP.Text.Trim();
        dePrice = NB_Price.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strMemo = TB_Memo.Text.Trim();

        if (strOwnerCode == "" | strType == "" | strAssetCode == "" | strSpec == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickSFASFF", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
        else
        {
            AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
            AssetCheckInOrderDetail assetCheckInOrderDetail = new AssetCheckInOrderDetail();

            assetCheckInOrderDetail.CheckInID = int.Parse(strCIOID);
            assetCheckInOrderDetail.AssetCode = strAssetCode;
            assetCheckInOrderDetail.AssetName = strAssetName;
            assetCheckInOrderDetail.Number = deNumber;
            assetCheckInOrderDetail.CheckInNumber = deNumber;
            assetCheckInOrderDetail.UnitName = strUnitName;
            assetCheckInOrderDetail.OwnerCode = strOwnerCode;
            try
            {
                assetCheckInOrderDetail.OwnerName = ShareClass.GetUserName(strOwnerCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
                return;
            }
            assetCheckInOrderDetail.Type = strType;
            assetCheckInOrderDetail.Spec = strSpec;
            assetCheckInOrderDetail.ModelNumber = strModelNumber;
            assetCheckInOrderDetail.Position = strPosition;
            assetCheckInOrderDetail.IP = strIP;
            assetCheckInOrderDetail.Price = dePrice;
            assetCheckInOrderDetail.Amount = deNumber * dePrice;
            assetCheckInOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            assetCheckInOrderDetail.BuyTime = dtBuyTime;
            assetCheckInOrderDetail.Manufacturer = strManufacturer;
            assetCheckInOrderDetail.Memo = strMemo;
            assetCheckInOrderDetail.Status = "InUse";


            try
            {
                assetCheckInOrderDetailBLL.AddAssetCheckInOrderDetail(assetCheckInOrderDetail);

                LB_ID.Text = ShareClass.GetMyCreatedMaxAssetCheckInDetailID(strCIOID).ToString();

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                BT_TakePhoto.Enabled = true;
                BT_DeletePhoto.Enabled = true;

                NB_Amount.Amount = SumAssetCheckInOrderAmount(strCIOID);
                strHQL = "Update T_AssetCheckInOrder Set Amount = " + NB_Amount.Amount.ToString();
                strHQL += " Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                LoadAssetCheckInOrderDetail(strCIOID);

                string strPhotoURL = HL_ItemPhoto.NavigateUrl;
                //添加相同记录到资产表
                addAsset(strCIOID, strAssetCode, strAssetName, deNumber, strUnitName,
                   strOwnerCode, strType, strSpec, strModelNumber, strPosition, strIP, dePrice, strCurrencyType, dtBuyTime,
                   strManufacturer, strMemo, LB_ID.Text, strPhotoURL);

                //保存资产图片
                UpdateAssetPhoto(LB_ID.Text);

                IM_ItemPhoto.ImageUrl = "";
                HL_ItemPhoto.NavigateUrl = "";
                TB_PhotoURL.Text = "";

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
        string strCIOID, strID, strOwnerCode, strType, strAssetCode, strAssetName, strModelNumber, strSpec, strIP, strManufacturer, strPosition, strMemo, strCurrencyType;
        DateTime dtBuyTime;
        decimal dePrice;

        string strUserCode = LB_UserCode.Text;
        string strHQL;
        string strUnitName;
        decimal deNumber;

        IList lst;

        strCIOID = LB_CheckInID.Text.Trim();
        strID = LB_ID.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strManufacturer = TB_Manufacturer.Text.Trim();
        strPosition = DL_WareHouse.SelectedValue.Trim();
        strIP = TB_IP.Text.Trim();
        dePrice = NB_Price.Amount;
        strCurrencyType = DL_CurrencyType.SelectedValue.Trim();
        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strMemo = TB_Memo.Text.Trim();


        if (strOwnerCode == "" | strType == "" | strAssetCode == "" | strSpec == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickDFSFF", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }
        else
        {
            AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
            strHQL = "from AssetCheckInOrderDetail as assetCheckInOrderDetail where assetCheckInOrderDetail.ID = " + strID;
            lst = assetCheckInOrderDetailBLL.GetAllAssetCheckInOrderDetails(strHQL);
            AssetCheckInOrderDetail assetCheckInOrderDetail = (AssetCheckInOrderDetail)lst[0];


            assetCheckInOrderDetail.ID = int.Parse(strID);
            assetCheckInOrderDetail.CheckInID = int.Parse(strCIOID);
            assetCheckInOrderDetail.AssetCode = strAssetCode;
            assetCheckInOrderDetail.AssetName = strAssetName;
            assetCheckInOrderDetail.Number = deNumber;

            assetCheckInOrderDetail.CheckInNumber = deNumber;
            assetCheckInOrderDetail.UnitName = strUnitName;
            assetCheckInOrderDetail.OwnerCode = strOwnerCode;
            try
            {
                assetCheckInOrderDetail.OwnerName = ShareClass.GetUserName(strOwnerCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
                return;
            }

            assetCheckInOrderDetail.Type = strType;
            assetCheckInOrderDetail.ModelNumber = strModelNumber;
            assetCheckInOrderDetail.Spec = strSpec;
            assetCheckInOrderDetail.Position = strPosition;
            assetCheckInOrderDetail.IP = strIP;
            assetCheckInOrderDetail.Price = dePrice;
            assetCheckInOrderDetail.Amount = deNumber * dePrice;
            assetCheckInOrderDetail.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            assetCheckInOrderDetail.Manufacturer = strManufacturer;
            assetCheckInOrderDetail.Memo = strMemo;
            assetCheckInOrderDetail.BuyTime = dtBuyTime;

            try
            {
                assetCheckInOrderDetailBLL.UpdateAssetCheckInOrderDetail(assetCheckInOrderDetail, int.Parse(strID));

                NB_Amount.Amount = SumAssetCheckInOrderAmount(strCIOID);
                strHQL = "Update T_AssetCheckInOrder Set Amount = " + NB_Amount.Amount.ToString();
                strHQL += " Where CheckInID = " + strCIOID;
                ShareClass.RunSqlCommand(strHQL);

                LoadAssetCheckInOrderDetail(strCIOID);

                //删除资产明细表相同记录
                strHQL = "Delete From T_Asset Where CheckInDetailID =" + strID;
                ShareClass.RunSqlCommand(strHQL);

                string strPhotoURL = HL_ItemPhoto.NavigateUrl;
                //添加相同记录到资产明细表
                addAsset(strCIOID, strAssetCode, strAssetName, deNumber, strUnitName,
                   strOwnerCode, strType, strSpec, strModelNumber, strPosition, strIP, dePrice, strCurrencyType, dtBuyTime,
                   strManufacturer, strMemo, strID, strPhotoURL);

                //保存资产图片
                UpdateAssetPhoto(strID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJCCJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }
        }
    }


    protected void UpdateAssetPhoto(string strCheckInDetailID)
    {
        string strHQL;
        string strPhotoURL = TB_PhotoURL.Text.Trim();
        string strAssetCode = TB_AssetCode.Text.Trim();

        if (strPhotoURL != "" & strAssetCode != "")
        {
            strHQL = "Update T_Asset Set PhotoURL = " + "'" + strPhotoURL + "'" + " Where AssetCode = " + "'" + strAssetCode + "'";
            strHQL += " and CheckInDetailID = " + strCheckInDetailID;
            ShareClass.RunSqlCommand(strHQL);
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

        if (strSourceType == "AssetPO")
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
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPOID;
            IList lst;

            strPOID = ((Button)e.Item.FindControl("BT_POID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
            AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
            lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
            AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

            DL_SourceType.SelectedValue = "AssetPO";
            NB_SourceID.Amount = assetPurchaseOrder.POID;

            LoadAssetPurchaseOrderDetail(strPOID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void BT_FindAsset_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strType, strAssetCode, strAssetName, strModelNumber, strSpec;
        string strWareHouse;

        DataGrid4.CurrentPageIndex = 0;
        TabContainer1.ActiveTabIndex = 1;

        strType = DL_Type.SelectedValue.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strType = "%" + strType + "%";
        strAssetCode = "%" + strAssetCode + "%";
        strAssetName = "%" + strAssetName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strWareHouse = DL_WareHouse.SelectedValue.Trim();

        strHQL = "From Asset as asset Where asset.AssetCode Like " + "'" + strAssetCode + "'" + " and asset.AssetName like " + "'" + strAssetName + "'";
        strHQL += " and asset.Type Like " + "'" + strType + "'" + " and asset.ModelNumber Like " + "'" + strModelNumber + "'" + " and asset.Spec Like " + "'" + strSpec + "'";
        //strHQL += " and asset.Number > 0";
        strHQL += " and Position = " + "'" + strWareHouse + "'";
        strHQL += " Order by asset.Number DESC";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;

        strHQL = "From Item as item Where item.ItemCode Like " + "'" + strAssetCode + "'" + " and item.ItemName like " + "'" + strAssetName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Asset'";

        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);

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

        IM_ItemPhoto.ImageUrl = "";
        HL_ItemPhoto.NavigateUrl = "";
        TB_PhotoURL.Text = "";


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
                NB_Price.Amount = item.PurchasePrice;

                IM_ItemPhoto.ImageUrl = item.PhotoURL;
                HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
                TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;
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


            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
            LB_ID.Text = strID;

            strPOID = LB_POID.Text.Trim();

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;

            AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
            lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
            AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

            TB_AssetCode.Text = assetPurRecord.AssetCode;
            TB_AssetName.Text = assetPurRecord.AssetName;
            TB_ModelNumber.Text = assetPurRecord.ModelNumber;
            TB_Spec.Text = assetPurRecord.Spec;
            DL_Type.SelectedValue = assetPurRecord.Type;
            NB_Number.Amount = assetPurRecord.Number;
            NB_Price.Amount = assetPurRecord.Price;

            DL_Type.SelectedValue = assetPurRecord.Type;
            DL_Unit.SelectedValue = assetPurRecord.Unit;
            DLC_BuyTime.Text = assetPurRecord.PurTime.ToString("yyyy-MM-dd");
            TB_Manufacturer.Text = assetPurRecord.Supplier.Trim();

            IM_ItemPhoto.ImageUrl = GetAssetPhotoURL(assetPurRecord.AssetCode, "0");
            HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
            TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;

            TB_IP.Text = "";
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

            string strID, strAssetCode;

            strID = e.Item.Cells[0].Text;
            strAssetCode = ((Button)e.Item.FindControl("BT_AssetCode")).Text.Trim();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Asset as asset where asset.ID = " + strID;
            AssetBLL assetBLL = new AssetBLL();
            lst = assetBLL.GetAllAssets(strHQL);

            if (lst.Count > 0)
            {
                Asset asset = (Asset)lst[0];

                //LB_OwnerCode.Text = asset.OwnerCode;
                //LB_OwnerName.Visible = true;
                //LB_OwnerName.Text = asset.OwnerName;

                TB_AssetCode.Text = asset.AssetCode;
                TB_AssetName.Text = asset.AssetName;
                TB_ModelNumber.Text = asset.ModelNumber;
                DL_Unit.SelectedValue = asset.UnitName;
                TB_Spec.Text = asset.Spec;
                DL_Type.SelectedValue = asset.Type;
                TB_Manufacturer.Text = asset.Manufacturer;

                IM_ItemPhoto.ImageUrl = GetAssetPhotoURL(asset.AssetCode, "0");
                TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;
                HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
        }
    }

    protected void BT_TakePhoto_Click(object sender, EventArgs e)
    {
        Panel2.Visible = true;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_DeletePhoto_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strAssetCode = TB_AssetCode.Text.Trim();

        try
        {
            strHQL = "Update T_Asset Set PhotoURL = '' Where AssetCode = " + "'" + strAssetCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_ItemPhoto.ImageUrl = "";
            HL_ItemPhoto.NavigateUrl = "";
            TB_PhotoURL.Text = "";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }


    protected void BT_UploadPhoto_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            string strAssetCode = TB_AssetCode.Text.Trim();
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

                    strHQL = "Update T_Asset Set PhotoURL = " + "'" + strFileName3 + "'" + " Where AssetCode = " + "'" + strAssetCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_ItemPhoto.ImageUrl = strFileName3;
                    HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
                    TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;


                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop1", "popShow('popPhotoWindow','true') ", true);

        ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop2", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void BT_SavePhoto_Click(object sender, EventArgs e)
    {
        string strAssetCode;
        string strAssetPhotoString;

        strAssetCode = TB_AssetCode.Text.Trim();

        strAssetPhotoString = TB_PhotoString1.Text.Trim();
        strAssetPhotoString += TB_PhotoString2.Text.Trim();
        strAssetPhotoString += TB_PhotoString3.Text.Trim();
        strAssetPhotoString += TB_PhotoString4.Text.Trim();

        if (strAssetPhotoString != "")
        {
            var binaryData = Convert.FromBase64String(strAssetPhotoString);

            string strDateTime = DateTime.Now.ToString("yyyyMMddHHMMssff");
            string strAssetPhotoURL = "Doc\\" + "UserPhoto\\" + strAssetCode + strDateTime + ".jpg";
            var imageFilePath = Server.MapPath("Doc") + "\\UserPhoto\\" + strAssetCode + strDateTime + ".jpg";

            if (File.Exists(imageFilePath))
            { File.Delete(imageFilePath); }
            var stream = new System.IO.FileStream(imageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Write(binaryData, 0, binaryData.Length);
            stream.Close();

            string strHQL = "Update T_Asset Set PhotoURL = " + "'" + strAssetPhotoURL + "'" + " Where AssetCode = " + "'" + strAssetCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_ItemPhoto.ImageUrl = strAssetPhotoURL;
            HL_ItemPhoto.NavigateUrl = IM_ItemPhoto.ImageUrl;
            TB_PhotoURL.Text = IM_ItemPhoto.ImageUrl;

        }

        ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop2", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected string GetAssetPhotoURL(string strAssetCode, string strCheckInDetailID)
    {
        string strHQL = " from Asset as asset where asset.AssetCode = " + "'" + strAssetCode + "'";

        if (strCheckInDetailID != "0")
        {
            strHQL += " and asset.CheckInDetailID = " + strCheckInDetailID;
        }

        AssetBLL assetBLL = new AssetBLL();
        IList lst = assetBLL.GetAllAssets(strHQL);
        if (lst.Count > 0)
        {
            try
            {
                Asset asset = (Asset)lst[0];
                return asset.PhotoURL.Trim();
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

    protected void DL_VendorList_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_Manufacturer.Text = DL_VendorList.SelectedItem.Text;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text;

        AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
        IList lst = assetCheckInOrderDetailBLL.GetAllAssetCheckInOrderDetails(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql2.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurchaseOrder");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text;
        AssetBLL assetBLL = new AssetBLL();
        IList lst = assetBLL.GetAllAssets(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        AssetCheckInOrderBLL assetCheckInOrderBLL = new AssetCheckInOrderBLL();
        IList lst = assetCheckInOrderBLL.GetAllAssetCheckInOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    public void addAsset(string strCIOID, string strAssetCode, string strAssetName, decimal deNumber, string strUnitName,
       string strOwnerCode, string strType, string strSpec, string strModelNumber, string strPosition, string strIP, decimal dePrice, string strCurrrencyType, DateTime dtBuyTime,
       string strManufacturer, string strMemo, string strCheckInDetailID, string strPhotoURL)
    {
        AssetBLL assetBLL = new AssetBLL();
        Asset asset = new Asset();

        asset.CheckInID = int.Parse(strCIOID);
        asset.AssetCode = strAssetCode;
        asset.AssetName = strAssetName;
        asset.Number = deNumber;
        asset.CheckInNumber = deNumber;
        asset.UnitName = strUnitName;
        asset.OwnerCode = strOwnerCode;
        try
        {
            asset.OwnerName = ShareClass.GetUserName(strOwnerCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
            return;
        }
        asset.Type = strType;
        asset.Spec = strSpec;
        asset.ModelNumber = strModelNumber;
        asset.Position = strPosition;
        asset.IP = strIP;
        asset.Price = dePrice;
        asset.CurrencyType = strCurrrencyType;
        asset.BuyTime = dtBuyTime;
        asset.Manufacturer = strManufacturer;
        asset.Memo = strMemo;
        asset.CheckInDetailID = int.Parse(strCheckInDetailID);
        asset.PhotoURL = strPhotoURL;
        asset.Status = "InUse";

        try
        {
            assetBLL.AddAsset(asset);
        }
        catch
        {
        }
    }

    public void addAssetCheckInOrderDetail(string strCIOID, string strAssetCode, string strAssetName, decimal deNumber, string strUnitName,
          string strOwnerCode, string strType, string strSpec, string strModelNumber, string strPosition, string strIP, decimal dePrice, DateTime dtBuyTime,
          string strManufacturer, string strMemo, string strAssetID)
    {
        AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
        AssetCheckInOrderDetail assetCheckInOrderDetail = new AssetCheckInOrderDetail();

        assetCheckInOrderDetail.CheckInID = int.Parse(strCIOID);
        assetCheckInOrderDetail.AssetCode = strAssetCode;
        assetCheckInOrderDetail.AssetName = strAssetName;
        assetCheckInOrderDetail.Number = deNumber;
        assetCheckInOrderDetail.CheckInNumber = deNumber;
        assetCheckInOrderDetail.UnitName = strUnitName;
        assetCheckInOrderDetail.OwnerCode = strOwnerCode;
        try
        {
            assetCheckInOrderDetail.OwnerName = ShareClass.GetUserName(strOwnerCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
            return;
        }
        assetCheckInOrderDetail.Type = strType;
        assetCheckInOrderDetail.Spec = strSpec;
        assetCheckInOrderDetail.ModelNumber = strModelNumber;
        assetCheckInOrderDetail.Position = strPosition;
        assetCheckInOrderDetail.IP = strIP;
        assetCheckInOrderDetail.Price = dePrice;
        assetCheckInOrderDetail.BuyTime = dtBuyTime;
        assetCheckInOrderDetail.Manufacturer = strManufacturer;
        assetCheckInOrderDetail.Memo = strMemo;
        assetCheckInOrderDetail.Status = "InUse";


        try
        {
            assetCheckInOrderDetailBLL.AddAssetCheckInOrderDetail(assetCheckInOrderDetail);
        }
        catch
        {
        }
    }

    protected decimal SumAssetCheckInOrderAmount(string strCIOID)
    {
        string strHQL;

        decimal deAmount = 0;

        strHQL = "Select Sum(CheckInNumber * Price) From T_AssetCheckInOrderDetail Where CheckInID = " + strCIOID;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetCheckInOrderDetail");

        deAmount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

        return deAmount;
    }

    

    protected void LoadAssetCheckInOrderDetail(string strCheckInID)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetCheckInOrderDetail as assetCheckInOrderDetail where assetCheckInOrderDetail.CheckInID = " + strCheckInID;
        strHQL += " Order by assetCheckInOrderDetail.Number DESC,assetCheckInOrderDetail.ID DESC";
        AssetCheckInOrderDetailBLL assetCheckInOrderDetailBLL = new AssetCheckInOrderDetailBLL();
        lst = assetCheckInOrderDetailBLL.GetAllAssetCheckInOrderDetails(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadAssetCheckInOrder(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From AssetCheckInOrder as assetCheckInOrder where assetCheckInOrder.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " Order By assetCheckInOrder.CheckInID DESC";
        AssetCheckInOrderBLL assetCheckInOrderBLL = new AssetCheckInOrderBLL();
        lst = assetCheckInOrderBLL.GetAllAssetCheckInOrders(strHQL);
        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadAssetPurchaseOrder(string strOperatorCode)
    {
        string strHQL;

        string strDepartString;
        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "Select * from T_AssetPurchaseOrder where OperatorCode in " + "( Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
        strHQL += " And POID not in (Select SourceID From T_AssetCheckInOrder Where SourceType='PurchaseOrder') ";//追加过滤条件，已入库登记的采购单，无需再次入库
        strHQL += " Order by POID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurchaseOrder");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;
    }

    protected void LoadAssetPurchaseOrderDetail(string strPOID)
    {
        string strHQL = "Select * from T_AssetPurRecord where POID = " + strPOID;
        strHQL += " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        LB_POID.Text = strPOID;

        LB_Sql3.Text = strHQL;
    }

    protected void LoadWareHouseListByAuthority(string strUserCode)
    {
        string strHQL;
        string strDepartString;

        strDepartString = LB_DepartString.Text.Trim();

        strHQL = " Select WHName From T_WareHouse Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By SortNumber DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse");

        DL_WareHouse.DataSource = ds;
        DL_WareHouse.DataBind();
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
}
