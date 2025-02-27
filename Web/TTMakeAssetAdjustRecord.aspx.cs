using System; using System.Resources;
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


using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTMakeAssetAdjustRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strUserName;

        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产登记入库", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            DLC_BuyTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

            LoadWareHouseListByAuthority(strUserCode);

            LB_OwnerCode.Text = strUserCode;
            LB_OwnerName.Text = strUserName;

            //2013-07-18 Liujp 
            BindAssetData(DL_Type.SelectedValue.Trim(), DL_WareHouse.SelectedValue.Trim());
        }
    }

    protected void BT_FindAsset_Click(object sender, EventArgs e)
    {
        BindAssetData(DL_Type.SelectedValue.Trim(), DL_WareHouse.SelectedValue.Trim());
        //string strHQL;
        //IList lst;

        //string strAssetType, strAssetCode, strAssetName, strModelNumber, strSpec;
        //string strWareHouse;
        //string strUserCode, strDepartString;

        //DataGrid4.CurrentPageIndex = 0;

        //strUserCode = LB_UserCode.Text.Trim();
        //strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        //strAssetType = DL_Type.SelectedValue.Trim();
        //strAssetCode = TB_AssetCode.Text.Trim();
        //strAssetName = TB_AssetName.Text.Trim();
        //strModelNumber = TB_ModelNumber.Text.Trim();
        //strSpec = TB_Spec.Text.Trim();

        //strAssetType = "%" + strAssetType + "%";
        //strAssetCode = "%" + strAssetCode + "%";
        //strAssetName = "%" + strAssetName + "%";
        //strModelNumber = "%" + strModelNumber + "%";
        //strSpec = "%" + strSpec + "%";

        //strWareHouse = DL_WareHouse.SelectedValue.Trim();

        //strHQL = "From Asset as asset Where asset.Type Like " + "'" +strAssetType+ "'" + " and asset.AssetCode Like " + "'" + strAssetCode + "'" + " and asset.AssetName like " + "'" + strAssetName + "'";
        //strHQL += " and asset.ModelNumber Like " + "'" + strModelNumber + "'" + " and asset.Spec Like " + "'" + strSpec + "'";
        //strHQL += " and asset.OwnerCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        //strHQL += " and asset.Number > 0";
        //strHQL += " and asset.Status = 'InUse' Order by asset.ID DESC";
        //AssetBLL assetBLL = new AssetBLL();
        //lst = assetBLL.GetAllAssets(strHQL);

        //DataGrid4.DataSource = lst;
        //DataGrid4.DataBind();

        //LB_Sql4.Text = strHQL;
    }

    protected void BindAssetData(string assetType, string assetWareHouse)
    {
        string strHQL;
        IList lst;

        string strAssetType, strAssetCode, strAssetName, strModelNumber, strSpec;
        string strWareHouse;
        string strUserCode, strDepartString;

        DataGrid4.CurrentPageIndex = 0;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strAssetType = DL_Type.SelectedValue.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strWareHouse = DL_WareHouse.SelectedValue.Trim();

        strHQL = "From Asset as asset Where asset.Type Like '%" + strAssetType + "%' ";//and asset.Position='" + strWareHouse + "' ";
        strHQL += " and asset.OwnerCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and asset.Number > 0 and asset.Status = 'InUse' ";
        if (!string.IsNullOrEmpty(strAssetCode))
        {
            strHQL += " and asset.AssetCode Like '%" + strAssetCode + "%' ";
        }
        if (!string.IsNullOrEmpty(strAssetName))
        {
            strHQL += " and asset.AssetName like '%" + strAssetName + "%' ";
        }
        if (!string.IsNullOrEmpty(strModelNumber))
        {
            strHQL += " and asset.ModelNumber Like '%" + strModelNumber + "%' ";
        }
        if (!string.IsNullOrEmpty(strSpec))
        {
            strHQL += " and asset.Spec Like '%" + strSpec + "%' ";
        }
        strHQL += " Order by asset.ID DESC ";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_AssetCode.Text = "";
        TB_AssetName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strAssetCode;
            string strUserCode = LB_UserCode.Text;

            strID = e.Item.Cells[0].Text;
            strAssetCode = ((Button)e.Item.FindControl("BT_AssetCode")).Text.Trim();

            strHQL = "from Asset as asset where asset.ID = " + "'" + strID + "'";
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            AssetBLL assetBLL = new AssetBLL();
            lst = assetBLL.GetAllAssets(strHQL);
            Asset asset = (Asset)lst[0];

            LB_ID.Text = asset.ID.ToString();
            TB_AssetCode.Text = asset.AssetCode.Trim();
            LB_OwnerCode.Text = asset.OwnerCode;
            lbl_CheckId.Text = asset.CheckInID.ToString();
            LB_OwnerName.Visible = true;
            LB_OwnerName.Text = GetUserName(asset.OwnerCode);

            TB_AssetCode.Text = asset.AssetCode;
            TB_AssetName.Text = asset.AssetName;
            TB_ModelNumber.Text = asset.ModelNumber;
            NB_Number.Amount = asset.Number;
            DL_Unit.SelectedValue = asset.UnitName;
            DL_Type.SelectedValue = asset.Type;
            TB_ModelNumber.Text = asset.ModelNumber;
            TB_Spec.Text = asset.Spec;
            TB_IP.Text = asset.IP;
            NB_Price.Amount = asset.Price;
            DLC_BuyTime.Text = asset.BuyTime.ToString("yyyy-MM-dd");

            TB_Manufacturer.Text = asset.Manufacturer;
            TB_Memo.Text = asset.Memo.Trim();

            BT_Adjust.Enabled = true;

            LoadAssetAdjustRecord(strID);
        }
    }

    protected void BT_Adjust_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strCIOID, strID, strOwnerCode, strType,strModelNumber, strAssetCode, strAssetName, strSpec, strIP, strManufacturer, strPosition, strMemo;
        DateTime dtBuyTime;
        decimal dePrice;

        string strUserCode = LB_UserCode.Text;      
        string strUnitName;
        decimal deNumber;

        IList lst;

       // strCIOID = "0";
        strCIOID = lbl_CheckId.Text.Trim();
        strID = LB_ID.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPosition = DL_WareHouse.SelectedValue.Trim();
        strIP = TB_IP.Text.Trim();
        dePrice = NB_Price.Amount;
        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strManufacturer = TB_Manufacturer.Text.Trim();
        strMemo = TB_Memo.Text.Trim();

        if (strOwnerCode == "" | strType == "" | strAssetCode == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBGRLXZCBMCKZRHYXDBNWKJC").ToString().Trim()+"')", true);
            return;
        }

        if (!IsAssetData(strAssetCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGZCBCZWFDZJC").ToString().Trim()+"')", true);
            return;
        }

        AssetBLL assetBLL = new AssetBLL();
        strHQL = "from Asset as asset where asset.AssetCode = " + "'" + strAssetCode + "'";
        lst = assetBLL.GetAllAssets(strHQL);
        Asset asset = (Asset)lst[0];

        asset.ID = int.Parse(strID);
        asset.CheckInID = int.Parse(strCIOID);
        asset.AssetCode = strAssetCode;
        asset.AssetName = strAssetName;
        asset.Number = deNumber;
        asset.UnitName = strUnitName;
        asset.OwnerCode = strOwnerCode;
        try
        {
            asset.OwnerName = GetUserName(strOwnerCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim()+"')", true);
            return;
        }
        asset.Type = strType;
        asset.ModelNumber = strModelNumber;
        asset.Spec = strSpec;
        asset.Position = strPosition;
        asset.IP = strIP;
        asset.Price = dePrice;
        asset.Memo = strMemo;
        asset.Manufacturer = strManufacturer;
        asset.BuyTime = dtBuyTime;

        try
        {
            //添加初始记录到资产调整表，以保存资产没调整前数据
            AddFirstAssetAdjustRecord(strID);

            assetBLL.UpdateAsset(asset, int.Parse(strID));

            AddAssetAdjustRecord(int.Parse(strID));

            LoadAssetBySql(LB_Sql4.Text.Trim());
            LoadAssetAdjustRecord(strID);


            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;

                if (strID == DataGrid4.Items[i].Cells[0].Text.Trim())
                {
                    DataGrid4.Items[i].ForeColor = Color.Red;
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDZCCJC").ToString().Trim()+"')", true);
        }
    }

    protected bool IsAssetData(string strassetcode)
    {
        //是资产的话，返回true;否则返回false
        bool flag = true;
        string strHQL = "Select AssetCode From T_Asset Where AssetCode='" + strassetcode + "' and Number > 0 and Status = 'InUse' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Asset");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    protected void AddFirstAssetAdjustRecord(string strAssetID)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strHQL = "from AssetAdjustRecord as assetAdjustRecord where assetAdjustRecord.AssetID = " + strAssetID;
        strHQL += " Order by assetAdjustRecord.ID DESC";
        AssetAdjustRecordBLL assetAdjustRecordBLL = new AssetAdjustRecordBLL();
        lst = assetAdjustRecordBLL.GetAllAssetAdjustRecords(strHQL);

        if (lst.Count == 0)
        {
            strHQL = "Insert Into T_AssetAdjustRecord(AssetID,CheckInID,AssetCode,AssetName,Number,UnitName,";
            strHQL += "OwnerCode,OwnerName,Type,ModelNumber,Spec,Position,IP,Price,BuyTime,Manufacturer,Memo,Status,AdjusterCode,AdjusterName,AdjustTime)";
            strHQL += " Select ID,CheckInID ,AssetCode,AssetName,Number,UnitName,";
            strHQL += "OwnerCode,OwnerName,Type,ModelNumber,Spec,Position,IP,Price,BuyTime,Manufacturer,Memo,Status,ApplicantCode,ApplicantName,now()";
            strHQL += " From T_Asset Where ID = " + strAssetID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void AddAssetAdjustRecord(int intAssetID)
    {
        string strCIOID, strOwnerCode, strType,strModelNumber, strAssetCode, strAssetName, strSpec, strIP, strPosition, strManufacturer,strMemo;
        DateTime dtBuyTime;
        decimal dePrice;

        string strUserCode = LB_UserCode.Text;

        string strUnitName;
        decimal deNumber;

        // strCIOID = "0";
        strCIOID = lbl_CheckId.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPosition = DL_WareHouse.SelectedValue.Trim();
        strIP = TB_IP.Text.Trim();
        dePrice = NB_Price.Amount;
        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strManufacturer = TB_Manufacturer.Text.Trim();
        strMemo = TB_Memo.Text.Trim();

        if (strOwnerCode == "" | strType == "" | strAssetCode == "" | strSpec == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim()+"')", true);
        }
        else
        {
            AssetAdjustRecordBLL assetAdjustRecordBLL = new AssetAdjustRecordBLL();
            AssetAdjustRecord assetAdjustRecord = new AssetAdjustRecord();

            assetAdjustRecord.CheckInID = int.Parse(strCIOID);
            assetAdjustRecord.AssetID = intAssetID;
            assetAdjustRecord.AssetCode = strAssetCode;
            assetAdjustRecord.AssetName = strAssetName;
            assetAdjustRecord.Number = deNumber;
            assetAdjustRecord.UnitName = strUnitName;
            assetAdjustRecord.OwnerCode = strOwnerCode;
            try
            {
                assetAdjustRecord.OwnerName = GetUserName(strOwnerCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim()+"')", true);
                return;
            }
            assetAdjustRecord.Type = strType;
            assetAdjustRecord.ModelNumber = strModelNumber;
            assetAdjustRecord.Spec = strSpec;
            assetAdjustRecord.Position = strPosition;
            assetAdjustRecord.IP = strIP;
            assetAdjustRecord.Price = dePrice;
            assetAdjustRecord.BuyTime = dtBuyTime;
            assetAdjustRecord.Manufacturer = strManufacturer;
            assetAdjustRecord.Memo = strMemo;
            assetAdjustRecord.Status = "InUse";

            assetAdjustRecord.AdjusterCode = strUserCode;
            assetAdjustRecord.AdjusterName = GetUserName(strUserCode);
            assetAdjustRecord.AdjustTime = DateTime.Now;

            //try
            //{
            assetAdjustRecordBLL.AddAssetAdjustRecord(assetAdjustRecord);
            //}
            //catch
            //{
            //}
        }
    }

    protected void LoadAssetByAssetID(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Asset as asset where asset.ID = " + strID;
        strHQL += " Order by asset.Number DESC,asset.ID DESC";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void LoadAssetBySql(string strHQL)
    {
        IList lst;

        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void LoadAssetAdjustRecord(string strAssetID)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetAdjustRecord as assetAdjustRecord where assetAdjustRecord.AssetID = " + strAssetID;
        strHQL += " Order by assetAdjustRecord.ID DESC";
        AssetAdjustRecordBLL assetAdjustRecordBLL = new AssetAdjustRecordBLL();
        lst = assetAdjustRecordBLL.GetAllAssetAdjustRecords(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
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

    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    protected void DL_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAssetData(DL_Type.SelectedValue.Trim(), DL_WareHouse.SelectedValue.Trim());
    }
}
