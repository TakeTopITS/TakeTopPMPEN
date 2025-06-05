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


using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTMakeGoodsAdjustRecord : System.Web.UI.Page
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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "库存管理", strUserCode);
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

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();
            DL_Type.Items.Insert(0, new ListItem("--Select--", ""));

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

            LoadWareHouseListByAuthority(strUserCode);

            ShareClass.InitialWarehouseTreeByAuthorityAsset(TreeView3, strUserCode, strDepartString);

            LB_OwnerCode.Text = strUserCode;
            LB_OwnerName.Text = strUserName;

            //2013-07-18 Liujp 
            BindGoodsData(DL_Type.SelectedValue.Trim(), TB_WHName.Text.Trim());
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "")
        {
            TB_WHName.Text = treeNode.Target.Trim();

            ShareClass.LoadWareHousePositions(TB_WHName.Text.Trim(), DL_WHPosition);
        }
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        //2013-07-18 Liujp 
        BindGoodsData(DL_Type.SelectedValue.Trim(), TB_WHName.Text.Trim());
    }

    protected void BindGoodsData(string goodsType, string goodsWareHouse)
    {
        string strHQL;
        IList lst;

        string strGoodsType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strWareHouse;
        string strUserCode, strDepartString;

        DataGrid4.CurrentPageIndex = 0;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strGoodsType = DL_Type.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strWareHouse = TB_WHName.Text.Trim();

        strHQL = "From Goods as goods Where goods.Type Like '%" + strGoodsType + "%' ";
        strHQL += " and goods.OwnerCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and goods.Number > 0 and goods.Status = 'InUse' ";
        if (!string.IsNullOrEmpty(strGoodsCode))
        {
            strHQL += " and goods.GoodsCode Like '%" + strGoodsCode + "%' ";
        }
        if (!string.IsNullOrEmpty(strGoodsName))
        {
            strHQL += " and goods.GoodsName like '%" + strGoodsName + "%' ";
        }
        if (!string.IsNullOrEmpty(strModelNumber))
        {
            strHQL += " and goods.ModelNumber Like '%" + strModelNumber + "%' ";
        }
        if (!string.IsNullOrEmpty(strSpec))
        {
            strHQL += " and goods.Spec Like '%" + strSpec + "%' ";
        }
        strHQL += " Order by goods.ID DESC ";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;
            string strUserCode = LB_UserCode.Text;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            strHQL = "from Goods as goods where goods.ID = " + "'" + strID + "'";
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);
            Goods goods = (Goods)lst[0];

            LB_ID.Text = goods.ID.ToString();
            TB_GoodsCode.Text = goods.GoodsCode.Trim();
            LB_OwnerCode.Text = goods.OwnerCode;

            LB_OwnerName.Visible = true;
            LB_OwnerName.Text = GetUserName(goods.OwnerCode);

            TB_GoodsCode.Text = goods.GoodsCode;
            TB_GoodsName.Text = goods.GoodsName;
            TB_ModelNumber.Text = goods.ModelNumber;
            TB_Brand.Text = goods.Manufacturer;
            NB_Number.Amount = goods.Number;
            DL_Unit.SelectedValue = goods.UnitName;
            DL_Type.SelectedValue = goods.Type;
            TB_ModelNumber.Text = goods.ModelNumber;
            TB_Spec.Text = goods.Spec;

            NB_Price.Amount = goods.Price;
            DLC_BuyTime.Text = goods.BuyTime.ToString("yyyy-MM-dd");

            TB_WHName.Text = goods.Position;
            ShareClass.LoadWareHousePositions(TB_WHName.Text.Trim(),DL_WHPosition);
            try
            {
                DL_WHPosition.SelectedValue = goods.WHPosition.Trim();
            }
            catch
            {
            }

            TB_Manufacturer.Text = goods.Manufacturer;
            TB_Memo.Text = goods.Memo.Trim();

            BT_Adjust.Enabled = true;

            LoadGoodsAdjustRecord(strID);
        }
    }

    protected void BT_Adjust_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID, strOwnerCode, strType, strModelNumber, strGoodsCode, strGoodsName, strSpec, strIP, strManufacturer, strPosition, strMemo;
        DateTime dtBuyTime;
        decimal dePrice;

        string strUserCode = LB_UserCode.Text;
        string strUnitName;
        decimal deNumber;

        IList lst;

 
        strID = LB_ID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPosition = TB_WHName.Text.Trim();

        dePrice = NB_Price.Amount;
        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strManufacturer = TB_Manufacturer.Text.Trim();
        strMemo = TB_Memo.Text.Trim();

        if (strOwnerCode == "" | strType == "" | strGoodsCode == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRLXLPBMCKZRHYXDBNWKJC") + "')", true);
            return;
        }

        if (!IsGoodsData(strGoodsCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGLPBCZWFDZJC") + "')", true);
            return;
        }

        GoodsBLL goodsBLL = new GoodsBLL();
        strHQL = "from Goods as goods where goods.GoodsCode = " + "'" + strGoodsCode + "'";
        lst = goodsBLL.GetAllGoodss(strHQL);
        Goods goods = (Goods)lst[0];

        goods.ID = int.Parse(strID);
     
        goods.GoodsCode = strGoodsCode;
        goods.GoodsName = strGoodsName;
        goods.Number = deNumber;
        goods.UnitName = strUnitName;
        goods.OwnerCode = strOwnerCode;
        try
        {
            goods.OwnerName = GetUserName(strOwnerCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC") + "')", true);
            return;
        }
        goods.Type = strType;
        goods.ModelNumber = strModelNumber;
        goods.Spec = strSpec;
        goods.Position = strPosition;
        goods.WHPosition = DL_WHPosition.SelectedValue.Trim();

        goods.Price = dePrice;
        goods.Memo = strMemo;
        goods.Manufacturer = strManufacturer;
        goods.BuyTime = dtBuyTime;

        try
        {
            //添加初始记录到物料调整表，以保存物料没调整前数据
            AddFirstGoodsAdjustRecord(strID);

            goodsBLL.UpdateGoods(goods, int.Parse(strID));

            AddGoodsAdjustRecord(int.Parse(strID));

            LoadGoodsBySql(LB_Sql4.Text.Trim());
            LoadGoodsAdjustRecord(strID);


            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;

                if (strID == DataGrid4.Items[i].Cells[0].Text.Trim())
                {
                    DataGrid4.Items[i].ForeColor = Color.Red;
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDZCCJC") + "')", true);
        }
    }

    protected bool IsGoodsData(string strgoodscode)
    {
        //是物料的话，返回true;否则返回false
        bool flag = true;
        string strHQL = "Select GoodsCode From T_Goods Where GoodsCode='" + strgoodscode + "' and Number > 0 and Status = 'InUse' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
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

    protected void AddFirstGoodsAdjustRecord(string strGoodsID)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strHQL = "from GoodsAdjustRecord as goodsAdjustRecord where goodsAdjustRecord.GoodsID = " + strGoodsID;
        strHQL += " Order by goodsAdjustRecord.ID DESC";
        GoodsAdjustRecordBLL goodsAdjustRecordBLL = new GoodsAdjustRecordBLL();
        lst = goodsAdjustRecordBLL.GetAllGoodsAdjustRecords(strHQL);

        if (lst.Count == 0)
        {
            strHQL = "Insert Into T_GoodsAdjustRecord(GoodsID,CheckInID,GoodsCode,GoodsName,Number,UnitName,";
            strHQL += "OwnerCode,OwnerName,Type,ModelNumber,Spec,Position,IP,Price,BuyTime,Manufacturer,Memo,Status,AdjusterCode,AdjusterName,AdjustTime)";
            strHQL += " Select ID,0,GoodsCode,GoodsName,Number,UnitName,";
            strHQL += "OwnerCode,OwnerName,Type,ModelNumber,Spec,Position,IP,Price,BuyTime,Manufacturer,Memo,Status,ApplicantCode,ApplicantName,now()";
            strHQL += " From T_Goods Where ID = " + strGoodsID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void AddGoodsAdjustRecord(int intGoodsID)
    {
        string strCIOID, strOwnerCode, strType, strModelNumber, strGoodsCode, strGoodsName, strSpec, strIP, strPosition, strManufacturer, strMemo;
        DateTime dtBuyTime;
        decimal dePrice;

        string strUserCode = LB_UserCode.Text;

        string strUnitName;
        decimal deNumber;

        strCIOID = "0";
        strOwnerCode = LB_OwnerCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        deNumber = NB_Number.Amount;
        strUnitName = DL_Unit.SelectedValue.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPosition = TB_WHName.Text.Trim();

        dePrice = NB_Price.Amount;
        dtBuyTime = DateTime.Parse(DLC_BuyTime.Text);
        strManufacturer = TB_Manufacturer.Text.Trim();
        strMemo = TB_Memo.Text.Trim();

        if (strOwnerCode == "" | strType == "" | strGoodsCode == "" | strSpec == "" | strPosition == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsAdjustRecordBLL goodsAdjustRecordBLL = new GoodsAdjustRecordBLL();
            GoodsAdjustRecord goodsAdjustRecord = new GoodsAdjustRecord();

            goodsAdjustRecord.CheckInID = int.Parse(strCIOID);
            goodsAdjustRecord.GoodsID = intGoodsID;
            goodsAdjustRecord.GoodsCode = strGoodsCode;
            goodsAdjustRecord.GoodsName = strGoodsName;
            goodsAdjustRecord.Number = deNumber;
            goodsAdjustRecord.UnitName = strUnitName;
            goodsAdjustRecord.OwnerCode = strOwnerCode;
            try
            {
                goodsAdjustRecord.OwnerName = GetUserName(strOwnerCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC") + "')", true);
                return;
            }
            goodsAdjustRecord.Type = strType;
            goodsAdjustRecord.ModelNumber = strModelNumber;
            goodsAdjustRecord.Spec = strSpec;
            goodsAdjustRecord.Position = strPosition;
            goodsAdjustRecord.WHPosition = DL_WHPosition.SelectedValue.Trim();

            goodsAdjustRecord.Price = dePrice;
            goodsAdjustRecord.BuyTime = dtBuyTime;
            goodsAdjustRecord.Manufacturer = strManufacturer;
            goodsAdjustRecord.Memo = strMemo;
            goodsAdjustRecord.Status = "InUse";

            goodsAdjustRecord.AdjusterCode = strUserCode;
            goodsAdjustRecord.AdjusterName = GetUserName(strUserCode);
            goodsAdjustRecord.AdjustTime = DateTime.Now;

            try
            {
                goodsAdjustRecordBLL.AddGoodsAdjustRecord(goodsAdjustRecord);
            }
            catch (Exception err)
            {
                Label1.Text = err.Message.ToString();
            }
        }
    }

    protected void LoadGoodsByGoodsID(string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Goods as goods where goods.ID = " + strID;
        strHQL += " Order by goods.Number DESC,goods.ID DESC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void LoadGoodsBySql(string strHQL)
    {
        IList lst;

        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void LoadGoodsAdjustRecord(string strGoodsID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsAdjustRecord as goodsAdjustRecord where goodsAdjustRecord.GoodsID = " + strGoodsID;
        strHQL += " Order by goodsAdjustRecord.ID DESC";
        GoodsAdjustRecordBLL goodsAdjustRecordBLL = new GoodsAdjustRecordBLL();
        lst = goodsAdjustRecordBLL.GetAllGoodsAdjustRecords(strHQL);
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

        //DL_WareHouse.DataSource = ds;
        //DL_WareHouse.DataBind();
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
        BindGoodsData(DL_Type.SelectedValue.Trim(), TB_WHName.Text.Trim());
    }
}
