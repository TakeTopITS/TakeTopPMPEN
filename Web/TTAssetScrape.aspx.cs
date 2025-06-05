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

public partial class TTAssetScrape : System.Web.UI.Page
{
    string strDepartString;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strAssetID = Request.QueryString["ID"];

        string strUserName;

        //this.Title = "资产报废";

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "资产管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ScrapeTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            TB_OperatorCode.Text = strUserCode;
            LB_OperatorName.Text = strUserName;

            LoadScrapeAsset();
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

            LB_AssetOwner.Text = strDepartName + LanguageHandle.GetWord("DZCLB");
            LB_AssetOwner.Visible = true;

            strHQL = "from Asset as asset where asset.OwnerCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") and asset.Number > 0 Order by asset.Number DESC,asset.ID DESC";
            AssetBLL assetBLL = new AssetBLL();
            lst = assetBLL.GetAllAssets(strHQL);
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
        string strAssetID;

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

            strAssetID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            strHQL = "from Asset as asset where asset.ID = " + strAssetID;
            AssetBLL assetBLL = new AssetBLL();
            lst2 = assetBLL.GetAllAssets(strHQL);

            Asset asset = (Asset)lst2[0];

            LB_AssetID.Text = strAssetID;
            TB_AssetCode.Text = asset.AssetCode;
            TB_AssetName.Text = asset.AssetName;
            TB_Type.Text = asset.Type;
            TB_OldUserCode.Text = asset.OwnerCode;
            LB_OldUserName.Text = GetUserName(asset.OwnerCode);

            NB_Number.Amount = asset.Number;
            NB_ScrapeNumber.Amount = asset.Number;

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

        LB_AssetOwner.Text = strUserName + LanguageHandle.GetWord("DZCLB");

        string strHQL = "from Asset as asset where asset.OwnerCode = " + "'" + strUserCode + "'" + " and asset.Number > 0 Order by asset.Number DESC,asset.ID DESC";
        AssetBLL assetBLL = new AssetBLL();
        IList lst = assetBLL.GetAllAssets(strHQL);
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

            AssetScrapeBLL assetScrapeBLL = new AssetScrapeBLL();
            strHQL = "from AssetScrape as assetScrape where  assetScrape.ID = " + strID;
            lst1 = assetScrapeBLL.GetAllAssetScrapes(strHQL);

            if (lst1.Count > 0)
            {
                AssetScrape assetScrape = (AssetScrape)lst1[0];

                LB_ID.Text = strID;
                LB_AssetID.Text = assetScrape.AssetID.ToString();
                TB_AssetCode.Text = assetScrape.AssetCode;
                TB_AssetName.Text = assetScrape.AssetName;
                TB_Type.Text = assetScrape.Type;
                TB_OldUserCode.Text = assetScrape.OldUserCode;
                LB_OldUserName.Text = GetUserName(assetScrape.OldUserCode);

                NB_Number.Amount = assetScrape.ScrapeNumber;
                NB_ScrapeNumber.Amount = assetScrape.ScrapeNumber;

                TB_ScrapeReason.Text = assetScrape.ScrapeReason;
                TB_AfterUse.Text = assetScrape.AfterScrapeUse;
                TB_GetAmount.Amount = assetScrape.GetAmount;
                DLC_ScrapeTime.Text = assetScrape.ScrapeTime.ToString("yyyy-MM-dd");
                TB_OperatorCode.Text = assetScrape.OperatorCode;
                LB_OperatorName.Text = GetUserName(assetScrape.OperatorCode);

                BT_Scrape.Enabled = false;
                BT_Reduce.Enabled = true;
            }
        }
    }

    protected void BT_Scrape_Click(object sender, EventArgs e)
    {
        string strOldUserCode, strType, strAssetID, strAssetCode, strAssetName;
        string strOperatorCode, strScrapeReason;
        DateTime dtScrapeTime;
        decimal deNumber, deScrapeNumber, deGetAmount;

        string strUserCode = LB_UserCode.Text;

        strAssetID = LB_AssetID.Text.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strType = TB_Type.Text.Trim();

        deNumber = NB_Number.Amount;
        deScrapeNumber = NB_ScrapeNumber.Amount;

        if (deScrapeNumber > deNumber)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBBFSLBNDYZCKCSLJC")+"')", true);
            return;
        }
        if (deScrapeNumber <= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBBFSLYDY0JC")+"')", true);
            return;
        }

        strScrapeReason = TB_ScrapeReason.Text.Trim();
        strOldUserCode = TB_OldUserCode.Text.Trim();
        strOperatorCode = TB_OperatorCode.Text.Trim();
        deGetAmount = TB_GetAmount.Amount;
        dtScrapeTime = DateTime.Parse(DLC_ScrapeTime.Text);

        if (strOldUserCode == "" | strType == "" | strAssetCode == "" | strOperatorCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYYHDMLXDMJJBRDMZRHYXDBNWKJC")+"')", true);
        }
        else
        {
            AssetScrapeBLL assetScrapeBLL = new AssetScrapeBLL();
            AssetScrape assetScrape = new AssetScrape();

            assetScrape.AssetID = int.Parse(strAssetID);
            assetScrape.AssetCode = strAssetCode;
            assetScrape.AssetName = strAssetName;
            assetScrape.Type = strType;
            assetScrape.OldUserCode = strOldUserCode;
            assetScrape.OldUserName = GetUserName(strOldUserCode);
            assetScrape.OperatorCode = strOperatorCode;
            assetScrape.OperatorName = GetUserName(strOperatorCode);
            assetScrape.ScrapeReason = strScrapeReason;
            assetScrape.AfterScrapeUse = TB_AfterUse.Text.Trim();
            assetScrape.ScrapeTime = dtScrapeTime;

            assetScrape.ScrapeNumber = deScrapeNumber;
            assetScrape.GetAmount = deGetAmount;

            try
            {
                assetScrapeBLL.AddAssetScrape(assetScrape);

                UpdateAssetStatus(strAssetID, "Scrapped", strOldUserCode, deScrapeNumber);

                LoadScrapeAsset();
                LoadAsset();

                NB_Number.Amount = deNumber - deScrapeNumber;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBFCG")+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBFCCJC")+"')", true);
            }
        }
    }   

    protected void BT_Reduce_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strAssetID = LB_AssetID.Text.Trim();

        string strHQL;
        IList lst;

        decimal deNumber, deScrapeNumber;

        deNumber = NB_Number.Amount;
        deScrapeNumber = NB_ScrapeNumber.Amount;

        if (deScrapeNumber > deNumber)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBHYSJBNDYBFSJJC")+"')", true);
            return;
        }
        if (deScrapeNumber <= 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBHYSLYDY0JC")+"')", true);
            return;
        }

        AssetScrapeBLL assetScrapeBLL = new AssetScrapeBLL();
        strHQL = "from AssetScrape as assetScrape where assetScrape.ID = " + strID;
        lst = assetScrapeBLL.GetAllAssetScrapes(strHQL);
        AssetScrape assetScrape = (AssetScrape)lst[0];

        try
        {
            assetScrape.ScrapeNumber = deNumber - deScrapeNumber;

            assetScrapeBLL.UpdateAssetScrape(assetScrape, int.Parse(strID));

            UpdateAssetStatus(strAssetID, "InUse", assetScrape.OldUserCode, deScrapeNumber);

            BT_Reduce.Enabled = false;

            LoadScrapeAsset();
            LoadAsset();

            NB_Number.Amount = deNumber - deScrapeNumber;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCHYCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZCHYSBJC")+"')", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text;

        AssetBLL assetBLL = new AssetBLL();
        IList lst = assetBLL.GetAllAssets(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text.Trim();
        AssetScrapeBLL assetScrapeBLL = new AssetScrapeBLL();
        IList lst = assetScrapeBLL.GetAllAssetScrapes(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void UpdateAssetStatus(string strAssetID, string strStatus, string strOwnerCode, decimal deScrapeNumber)
    {
        string strHQL;
        IList lst;

        decimal deOldNumber;

        AssetBLL assetBLL = new AssetBLL();
        strHQL = "from Asset as asset where asset.ID = " + strAssetID;
        lst = assetBLL.GetAllAssets(strHQL);
        Asset asset = (Asset)lst[0];

        if (strStatus == "Scrapped")
        {
            try
            {
                deOldNumber = asset.Number;
                asset.Number = deOldNumber - deScrapeNumber;
                //asset.Status = "Scrapped";
                //asset.OwnerCode = "";
                //asset.OwnerName = "";

                assetBLL.UpdateAsset(asset, int.Parse(strAssetID));
            }
            catch
            {
            }
        }
        else
        {
            try
            {
                //asset.Status = "InUse";

                deOldNumber = asset.Number;
                asset.Number = deOldNumber + deScrapeNumber;
                //asset.OwnerCode = strOwnerCode;
                //asset.OwnerName = GetUserName(strOwnerCode);

                assetBLL.UpdateAsset(asset, int.Parse(strAssetID));
            }
            catch
            {
            }
        }
    }

    protected void LoadScrapeAsset()
    {
        string strHQL;
        IList lst;

        strDepartString = LB_DepartString.Text.Trim();

        strHQL = "From AssetScrape as assetScrape Where  ";
        strHQL += " assetScrape.OldUserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and assetScrape.ScrapeNumber > 0 ";
        strHQL += " Order by assetScrape.ID DESC";

        AssetScrapeBLL assetScrapeBLL = new AssetScrapeBLL();
        lst = assetScrapeBLL.GetAllAssetScrapes(strHQL);
        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void LoadAsset()
    {
        string strHQL;
        IList lst;
        string strDepartCode, strOwnerCode;

        strDepartCode = LB_DepartCode.Text.Trim();
        strOwnerCode = LB_OwnerCode.Text.Trim();

        AssetBLL assetBLL = new AssetBLL();

        if (strDepartCode != "")
        {
            LB_AssetOwner.Text = GetDepartName(strDepartCode) + LanguageHandle.GetWord("DZCLB");
            LB_AssetOwner.Visible = true;

            strHQL = "from Asset as asset where asset.OwnerCode in (select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ") and asset.Number > 0 Order by asset.Number DESC,asset.ID DESC";

            lst = assetBLL.GetAllAssets(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;
        }
        else
        {
            LB_AssetOwner.Text = GetUserName(strOwnerCode) + LanguageHandle.GetWord("DZCLB");
            LB_AssetOwner.Visible = true;

            strHQL = "from Asset as asset where asset.OwnerCode = " + "'" + strOwnerCode + "'" + " and asset.Number > 0 Order by asset.Number DESC,asset.ID DESC";

            lst = assetBLL.GetAllAssets(strHQL);
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
