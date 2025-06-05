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


public partial class TTGoodsScrape : System.Web.UI.Page
{
    string strDepartString;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strGoodsID = Request.QueryString["ID"];

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ScrapeTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            TB_OperatorCode.Text = strUserCode;
            LB_OperatorName.Text = strUserName;

            LoadScrapeGoods();
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;
        string strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_GoodsOwner.Text = strDepartName + LanguageHandle.GetWord("DLPLB");
            LB_GoodsOwner.Visible = true;

            strHQL = "from Goods as goods where goods.OwnerCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") and goods.Number > 0 Order by goods.Number DESC,goods.ID DESC";
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);

            LB_DepartCode.Text = strDepartCode;
            LB_OwnerCode.Text = "";
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strGoodsID;

        string strUserCode = LB_UserCode.Text;
        string strHQL;
        IList lst2;

        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strGoodsID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "from Goods as goods where goods.ID = " + strGoodsID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst2 = goodsBLL.GetAllGoodss(strHQL);

            Goods goods = (Goods)lst2[0];

            LB_GoodsID.Text = strGoodsID;
            TB_GoodsCode.Text = goods.GoodsCode;
            TB_GoodsName.Text = goods.GoodsName;
            TB_Type.Text = goods.Type;
            TB_OldUserCode.Text = goods.OwnerCode;
            LB_OldUserName.Text = GetUserName(goods.OwnerCode);

            NB_Number.Amount = goods.Number;
            NB_ScrapeNumber.Amount = goods.Number;

            TB_ScrapeReason.Text = "";
            TB_AfterUse.Text = "";
            TB_GetAmount.Amount = 0;
            DLC_ScrapeTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_OperatorCode.Text = strUserCode;

            BT_Scrape.Enabled = true;
            BT_Reduce.Enabled = false;
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        LB_OldUserName.Visible = true;

        TB_OldUserCode.Text = strUserCode;
        LB_OldUserName.Text = strUserName;

        LB_GoodsOwner.Text = strUserName + LanguageHandle.GetWord("DLPLB");

        string strHQL = "from Goods as goods where goods.OwnerCode = " + "'" + strUserCode + "'" + " and goods.Number > 0 Order by goods.Number DESC,goods.ID DESC";
        GoodsBLL goodsBLL = new GoodsBLL();
        IList lst = goodsBLL.GetAllGoodss(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_OwnerCode.Text = strUserCode;
        LB_DepartCode.Text = "";
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID;

        string strUserCode = LB_UserCode.Text;
        string strHQL;
        IList lst1;

        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            GoodsScrapeBLL goodsScrapeBLL = new GoodsScrapeBLL();
            strHQL = "from GoodsScrape as goodsScrape where  goodsScrape.ID = " + strID;
            lst1 = goodsScrapeBLL.GetAllGoodsScrapes(strHQL);

            if (lst1.Count > 0)
            {
                GoodsScrape goodsScrape = (GoodsScrape)lst1[0];

                LB_ID.Text = strID;
                LB_GoodsID.Text = goodsScrape.GoodsID.ToString();
                TB_GoodsCode.Text = goodsScrape.GoodsCode;
                TB_GoodsName.Text = goodsScrape.GoodsName;
                TB_Type.Text = goodsScrape.Type;
                TB_OldUserCode.Text = goodsScrape.OldUserCode;
                LB_OldUserName.Text = GetUserName(goodsScrape.OldUserCode);

                NB_Number.Amount = goodsScrape.ScrapeNumber;
                NB_ScrapeNumber.Amount = goodsScrape.ScrapeNumber;

                TB_ScrapeReason.Text = goodsScrape.ScrapeReason;
                TB_AfterUse.Text = goodsScrape.AfterScrapeUse;
                TB_GetAmount.Amount = goodsScrape.GetAmount;
                DLC_ScrapeTime.Text = goodsScrape.ScrapeTime.ToString("yyyy-MM-dd");
                TB_OperatorCode.Text = goodsScrape.OperatorCode;
                LB_OperatorName.Text = GetUserName(goodsScrape.OperatorCode);

                BT_Scrape.Enabled = false;
                BT_Reduce.Enabled = true;
            }
        }
    }

    protected void BT_Scrape_Click(object sender, EventArgs e)
    {
        string strOldUserCode, strType, strGoodsID, strGoodsCode, strGoodsName;
        string strOperatorCode, strScrapeReason;
        DateTime dtScrapeTime;
        decimal deNumber, deScrapeNumber, deGetAmount;

        string strUserCode = LB_UserCode.Text;

        strGoodsID = LB_GoodsID.Text.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strType = TB_Type.Text.Trim();

        deNumber = NB_Number.Amount;
        deScrapeNumber = NB_ScrapeNumber.Amount;

        if (deScrapeNumber > deNumber)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBBFSLBNDYLPKCSLJC") + "')", true);
            return;
        }
        if (deScrapeNumber <= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBBFSLYDY0JC") + "')", true);
            return;
        }

        strScrapeReason = TB_ScrapeReason.Text.Trim();
        strOldUserCode = TB_OldUserCode.Text.Trim();
        strOperatorCode = TB_OperatorCode.Text.Trim();
        deGetAmount = TB_GetAmount.Amount;
        dtScrapeTime = DateTime.Parse(DLC_ScrapeTime.Text);

        if (strOldUserCode == "" | strType == "" | strGoodsCode == "" | strOperatorCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYYHDMLXDMJJBRDMZRHYXDBNWKJC") + "')", true);
        }
        else
        {
            GoodsScrapeBLL goodsScrapeBLL = new GoodsScrapeBLL();
            GoodsScrape goodsScrape = new GoodsScrape();

            goodsScrape.GoodsID = int.Parse(strGoodsID);
            goodsScrape.GoodsCode = strGoodsCode;
            goodsScrape.GoodsName = strGoodsName;
            goodsScrape.Type = strType;
            goodsScrape.OldUserCode = strOldUserCode;
            goodsScrape.OldUserName = GetUserName(strOldUserCode);
            goodsScrape.OperatorCode = strOperatorCode;
            goodsScrape.OperatorName = GetUserName(strOperatorCode);
            goodsScrape.ScrapeReason = strScrapeReason;
            goodsScrape.AfterScrapeUse = TB_AfterUse.Text.Trim();
            goodsScrape.ScrapeTime = dtScrapeTime;

            goodsScrape.ScrapeNumber = deScrapeNumber;
            goodsScrape.GetAmount = deGetAmount;

            try
            {
                goodsScrapeBLL.AddGoodsScrape(goodsScrape);

                UpdateGoodsStatus(strGoodsID, "Scrapped", strOldUserCode, deScrapeNumber);

                LoadScrapeGoods();
                LoadGoods();

                NB_Number.Amount = deNumber - deScrapeNumber;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBFCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBFCCJC") + "')", true);
            }
        }
    }

    protected void BT_Reduce_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strGoodsID = LB_GoodsID.Text.Trim();

        string strHQL;
        IList lst;

        decimal deNumber, deScrapeNumber;

        deNumber = NB_Number.Amount;
        deScrapeNumber = NB_ScrapeNumber.Amount;

        if (deScrapeNumber > deNumber)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBHYSJBNDYBFSJJC") + "')", true);
            return;
        }
        if (deScrapeNumber <= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBHYSLYDY0JC") + "')", true);
            return;
        }

        GoodsScrapeBLL goodsScrapeBLL = new GoodsScrapeBLL();
        strHQL = "from GoodsScrape as goodsScrape where goodsScrape.ID = " + strID;
        lst = goodsScrapeBLL.GetAllGoodsScrapes(strHQL);
        GoodsScrape goodsScrape = (GoodsScrape)lst[0];

        try
        {
            goodsScrape.ScrapeNumber = deNumber - deScrapeNumber;

            goodsScrapeBLL.UpdateGoodsScrape(goodsScrape, int.Parse(strID));

            UpdateGoodsStatus(strGoodsID, "InUse", goodsScrape.OldUserCode, deScrapeNumber);

            BT_Reduce.Enabled = false;

            LoadScrapeGoods();
            LoadGoods();

            NB_Number.Amount = deNumber - deScrapeNumber;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPHYCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLPHYSBJC") + "')", true);
        }
    }

    protected void BT_FindAll_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strGoodsName, strWareHouse;
        string strUserCode = TB_OldUserCode.Text.Trim();

        strDepartString = LB_DepartString.Text.Trim();

        strGoodsName = "%" + TB_FindGoodsName.Text.Trim() + "%";
        strWareHouse = "%" + TB_FindWareHouse.Text.Trim() + "%";


        strHQL = "from Goods as goods where goods.OwnerCode = " + "'" + strUserCode + "'" + " and goods.Number > 0 ";
        strHQL += " And goods.GoodsName Like '" + strGoodsName + "'";
        strHQL += " And goods.Position Like '" + strWareHouse + "'";
        strHQL += " Order by goods.Number DESC,goods.ID DESC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text;

        GoodsBLL goodsBLL = new GoodsBLL();
        IList lst = goodsBLL.GetAllGoodss(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text.Trim();
        GoodsScrapeBLL goodsScrapeBLL = new GoodsScrapeBLL();
        IList lst = goodsScrapeBLL.GetAllGoodsScrapes(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void UpdateGoodsStatus(string strGoodsID, string strStatus, string strOwnerCode, decimal deScrapeNumber)
    {
        string strHQL;
        IList lst;

        decimal deOldNumber;

        GoodsBLL goodsBLL = new GoodsBLL();
        strHQL = "from Goods as goods where goods.ID = " + strGoodsID;
        lst = goodsBLL.GetAllGoodss(strHQL);
        Goods goods = (Goods)lst[0];

        if (strStatus == "Scrapped")
        {
            try
            {
                deOldNumber = goods.Number;
                goods.Number = deOldNumber - deScrapeNumber;
                //goods.Status = "Scrapped";
                //goods.OwnerCode = "";
                //goods.OwnerName = "";

                goodsBLL.UpdateGoods(goods, int.Parse(strGoodsID));
            }
            catch
            {
            }
        }
        else
        {
            try
            {
                //goods.Status = "InUse";

                deOldNumber = goods.Number;
                goods.Number = deOldNumber + deScrapeNumber;
                //goods.OwnerCode = strOwnerCode;
                //goods.OwnerName = GetUserName(strOwnerCode);

                goodsBLL.UpdateGoods(goods, int.Parse(strGoodsID));
            }
            catch
            {
            }
        }
    }

    protected void LoadScrapeGoods()
    {
        string strHQL;
        IList lst;

        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "From GoodsScrape as goodsScrape Where  ";
        strHQL += " goodsScrape.OldUserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and goodsScrape.ScrapeNumber > 0 ";
        strHQL += " Order by goodsScrape.ID DESC";

        GoodsScrapeBLL goodsScrapeBLL = new GoodsScrapeBLL();
        lst = goodsScrapeBLL.GetAllGoodsScrapes(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void LoadGoods()
    {
        string strHQL;
        IList lst;
        string strDepartCode, strOwnerCode;

        strDepartCode = LB_DepartCode.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();

        GoodsBLL goodsBLL = new GoodsBLL();

        if (strDepartCode != "")
        {
            LB_GoodsOwner.Text = GetDepartName(strDepartCode) + LanguageHandle.GetWord("DLPLB");
            LB_GoodsOwner.Visible = true;

            strHQL = "from Goods as goods where goods.OwnerCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") and goods.Number > 0 Order by goods.Number DESC,goods.ID DESC";

            lst = goodsBLL.GetAllGoodss(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;
        }
        else
        {
            LB_GoodsOwner.Text = GetUserName(strOwnerCode) + LanguageHandle.GetWord("DeWuLiaoLieBiao");
            LB_GoodsOwner.Visible = true;

            strHQL = "from Goods as goods where goods.OwnerCode = " + "'" + strOwnerCode + "'" + " and goods.Number > 0 Order by goods.Number DESC,goods.ID DESC";

            lst = goodsBLL.GetAllGoodss(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;
        }
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strDepartName;

        string strHQL = " from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);
        Department department = (Department)lst[0];

        strDepartName = department.DepartName.Trim();

        return strDepartName;
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

}
