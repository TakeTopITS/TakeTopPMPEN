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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAssetUserRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;
     
        string strAssetID = Request.QueryString["ID"];
    
        //this.Title = "资产编号：" + strAssetID + "用户记录！";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_BeginUseTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndUseTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);

            string strDepartCode = ShareClass.GetDepartCodeFromUserCode (strUserCode);
            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();            

            LB_AssetID.Text = strAssetID;

            strHQL = "from ChangeType as changeType Order by changeType.SortNumber ASC";
            ChangeTypeBLL changeTypeBLL = new ChangeTypeBLL();
            lst = changeTypeBLL.GetAllChangeTypes(strHQL);

            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            LoadWareHouseListByAuthority(strUserCode);

            strHQL = "from AssetUserRecord as assetUserRecord where assetUserRecord.AssetID = " + strAssetID;
            AssetUserRecordBLL assetUserRecordBLL = new AssetUserRecordBLL();
            lst = assetUserRecordBLL.GetAllAssetUserRecords(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql1.Text = strHQL;

            AssetBLL assetBLL = new AssetBLL();
            Asset asset = new Asset();
            strHQL = "from Asset as asset where asset.ID =" + strAssetID;
            lst = assetBLL.GetAllAssets(strHQL);
            asset = (Asset)lst[0];
            if (asset.Number == 0)
            {
                BT_New.Enabled = false;
            }

            LB_AssetCode.Text = asset.AssetCode.Trim();
        }
    }

    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
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

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popwindowDepartment') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strAssetID, strAssetCode, strID, strType, strPosition;
        string strBeginUseTime, strEndUseTime;
        string strUserCode, strHQL;
        decimal deNumber1, deNumber2;
      
      
        IList lst;

        AssetBLL assetBLL = new AssetBLL();
        Asset asset = new Asset();

        strAssetID = LB_AssetID.Text.Trim();
      
        AssetUserRecordBLL assetUserRecordBLL = new AssetUserRecordBLL();
        AssetUserRecord assetUserRecord = new AssetUserRecord();

        strAssetCode = LB_AssetCode.Text.Trim();
        strUserCode = TB_UserCode.Text.Trim().ToUpper();     
        strType = DL_Type.SelectedValue;
        deNumber1 = NB_Number.Amount;       
        strBeginUseTime = DLC_BeginUseTime.Text;
        strEndUseTime = DLC_EndUseTime.Text;
        strPosition = DL_WareHouse.SelectedValue.Trim();

        strHQL = "from Asset as asset where asset.ID =" + strAssetID;
        lst = assetBLL.GetAllAssets(strHQL);
        asset = (Asset)lst[0];
        deNumber2 = asset.Number;

        if (deNumber1 > deNumber2 | deNumber2 == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCCCJLSLWLHSLBNDYCZCKCSLJC")+"')", true);
            return;
        }
              
        if (strAssetCode == "" )
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYHBNWKJC")+"')", true);
        }
        else
        {
            assetUserRecord.AssetID = int.Parse(strAssetID);
            assetUserRecord.UserCode = strUserCode;
            assetUserRecord.UserName = ShareClass. GetUserName(strUserCode);           
            assetUserRecord.AssetCode = strAssetCode;
            assetUserRecord.Type = strType;
            assetUserRecord.Number = deNumber1;            
            assetUserRecord.BeginUseTime = DateTime.Parse(strBeginUseTime);
            assetUserRecord.EndUseTime = DateTime.Parse(strEndUseTime);
            assetUserRecord.Position = strPosition;

            try
            {                
                assetUserRecordBLL.AddAssetUserRecord(assetUserRecord);
                strID = ShareClass.GetMyCreatedMaxAssetuserRecordID(strAssetCode);
                LB_ID.Text = strID;           

                strHQL = "from Asset as asset where asset.ID =" + strAssetID;         
                lst = assetBLL.GetAllAssets(strHQL);
                asset = (Asset)lst[0];

                deNumber2 = asset.Number;
                asset.Number = deNumber2 - deNumber1;
                assetBLL.UpdateAsset(asset, int.Parse(strAssetID));

                //strHQL = "from Asset as asset where asset.AssetCode = " + "'" + strAssetCode + "'" + " and asset.OwnerCode = " + "'" + strUserCode + "'";
                //lst = assetBLL.GetAllAssets(strHQL);
                //if (lst.Count > 0)
                //{
                //    asset = (Asset)lst[0];
                //    intID = asset.ID;
                //    deNumber3 = asset.Number;
                //    asset.Number = deNumber1 + deNumber3;

                //    assetBLL.UpdateAsset(asset, intID);
                //}
                //else
                //{
                    asset.Number = deNumber1;
                    asset.OwnerCode = strUserCode;
                    asset.OwnerName = ShareClass. GetUserName(strUserCode);
                    asset.Position = strPosition;
                    assetBLL.AddAsset(asset);
                //}

                LoadAssetUserRecord();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDPCG")+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDPCCJC")+"')", true);
            }
        }
    }

    protected void BT_Select_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popwindowDepartment') ", true);

    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Gray;
            }

            e.Item.ForeColor = Color.Red;

            string strHQL = "from AssetUserRecord as assetUserRecord where assetUserRecord.ID= " + strID;

            AssetUserRecordBLL assetUserRecordBLL = new AssetUserRecordBLL();

            IList lst = assetUserRecordBLL.GetAllAssetUserRecords(strHQL);

            AssetUserRecord assetUserRecord = (AssetUserRecord)lst[0];

            LB_ID.Text = assetUserRecord.ID.ToString();
            TB_UserCode.Text = assetUserRecord.UserCode.ToString();
            LB_UserName.Text = assetUserRecord.UserName.Trim();
            DL_Type.SelectedValue = assetUserRecord.Type;
            DLC_BeginUseTime.Text = assetUserRecord.BeginUseTime.ToString("yyyy-MM-dd");
            DLC_EndUseTime.Text = assetUserRecord.EndUseTime.ToString("yyyy-MM-dd");
            NB_Number.Amount = assetUserRecord.Number;
            DL_WareHouse.SelectedValue = assetUserRecord.Position.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql1.Text;

        AssetUserRecordBLL assetUserRecordBLL = new AssetUserRecordBLL();
        IList lst = assetUserRecordBLL.GetAllAssetUserRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        TB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadAssetUserRecord()
    {
        string strAssetID = LB_AssetID.Text;
        string strHQL = "from AssetUserRecord as assetUserRecord where assetUserRecord.AssetID = " + strAssetID;

        AssetUserRecordBLL assetUserRecordBLL = new AssetUserRecordBLL();
        IList lst = assetUserRecordBLL.GetAllAssetUserRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
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
  

}
