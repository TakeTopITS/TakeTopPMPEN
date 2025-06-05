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

public partial class TTAssetMTRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strAssetID = Request.QueryString["ID"];
        string strUserCode = Session["UserCode"].ToString();

        string strHQL;
        IList lst;

        //this.Title = "资产编号：" + strAssetID + "维护记录！";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_MTTime .Text=  DateTime.Now.ToString("yyyy-MM-dd");           

            strHQL = "from MTType as mtType Order By mtType.SortNumber ASC";
            MTTypeBLL mtTypeBLL = new MTTypeBLL();
            lst = mtTypeBLL.GetAllMTTypes(strHQL);

            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            TB_MTManCode.Text = strUserCode;
            LB_AssetID.Text = strAssetID;

            AssetBLL assetBLL = new AssetBLL();
            Asset asset = new Asset();
            strHQL = "from Asset as asset where asset.ID =" + strAssetID;
            lst = assetBLL.GetAllAssets(strHQL);
            asset = (Asset)lst[0];
           
            LB_AssetCode.Text = asset.AssetCode;

            if (asset.Number == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoCiZiChanShuLiangWei0B")+"')", true);

            }

            strHQL = "from AssetMTRecord as assetMTRecord where assetMTRecord.AssetID = " + strAssetID;
            AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();
            lst = assetMTRecordBLL.GetAllAssetMTRecords(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;   
        }
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Gray;
                }

                e.Item.ForeColor = Color.Red;

                string strHQL = "from AssetMTRecord as assetMTRecord where assetMTRecord.ID= " + strID;

                AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();

                IList lst = assetMTRecordBLL.GetAllAssetMTRecords(strHQL);

                AssetMTRecord assetMTRecord = (AssetMTRecord)lst[0];

                LB_ID.Text = assetMTRecord.ID.ToString();
                TB_MTManCode.Text = assetMTRecord.MTManCode.ToString();

                TB_Description.Text = assetMTRecord.Description;
                TB_Cost.Amount = assetMTRecord.Cost;
                DLC_MTTime.Text = assetMTRecord.MTTime.ToString("yyyy-MM-dd");

                DL_Type.SelectedValue = assetMTRecord.Type;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                if (strID != "")
                {
                    AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();
                    AssetMTRecord assetMTRecord = new AssetMTRecord();

                    assetMTRecord.ID = int.Parse(strID);
                    assetMTRecord.AssetCode = LB_AssetCode.Text.Trim();

                    try
                    {
                        assetMTRecordBLL.DeleteAssetMTRecord(assetMTRecord);

                        //BT_Update.Enabled = false;
                        //BT_Delete.Enabled = false;

                        LoadAssetMTRecord();

                        UpdateCarInformationStatus(assetMTRecord.AssetCode, "InUse");

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXJLBNSC") + "')", true);
                }
            }
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddMTRecord();
        }
        else
        {
            UpdateMTRecord();
        }
    }

    protected void AddMTRecord()
    {
        string strAssetID, strAssetCode, strAssetName, strID, strType;
        string strDescription, strMTTime;
        string strMTManCode;
        decimal deCost;
      
        AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();
        AssetMTRecord assetMTRecord = new AssetMTRecord();

        strAssetID = LB_AssetID.Text.Trim();
        strAssetCode = LB_AssetCode.Text.Trim();
        strAssetName = ShareClass.GetAssetName(strAssetCode);
        strMTManCode = TB_MTManCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strMTTime = DLC_MTTime.Text;
        deCost = TB_Cost.Amount;       

        if (strMTManCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYHBNWKJC")+"')", true);
        }
        else
        {
            assetMTRecord.AssetID = int.Parse(strAssetID);
            assetMTRecord.MTManCode = strMTManCode;
            assetMTRecord.MTManName = ShareClass.GetUserName(strMTManCode);
            assetMTRecord.AssetCode = strAssetCode;
            assetMTRecord.AssetName = strAssetName;
            assetMTRecord.Type = strType;
            assetMTRecord.MTTime = DateTime.Parse(strMTTime);
            assetMTRecord.Description = strDescription;
            assetMTRecord.Cost = deCost;

            try
            {
                assetMTRecordBLL = new AssetMTRecordBLL();
                assetMTRecordBLL.AddAssetMTRecord(assetMTRecord);
                strID = ShareClass.GetMyCreatedMaxAssetMtRecordID(strAssetCode);
                LB_ID.Text = strID;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;
                
                LoadAssetMTRecord();
                if (UpdateCarInformationStatus(strAssetCode, "Maintenance"))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCLWXXZCGWXWCHDCLGLCLDAZWHCLZTXX")+"')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }


    protected void UpdateMTRecord()
    {
        string strAssetCode, strID,strAssetID, strType;
        string strDescription, strMTTime;
        string strMTManCode, strCost;
      

        strID = LB_ID.Text.Trim();

        AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();
        AssetMTRecord assetMTRecord = new AssetMTRecord();

        strAssetID = LB_AssetID.Text.Trim();
        strAssetCode = LB_AssetCode.Text.Trim();
        strMTManCode = TB_MTManCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strMTTime = DLC_MTTime.Text;
        strCost = TB_Cost.Amount.ToString().Trim();

        if (strCost == "")
            strCost = "0";

        if (strID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYHBNWKJC")+"')", true);
        }
        else
        {
            assetMTRecord.AssetID = int.Parse(strAssetID);
            assetMTRecord.MTManCode = strMTManCode;
            assetMTRecord.MTManName = ShareClass.GetUserName(strMTManCode);
            assetMTRecord.AssetCode = strAssetCode;
            assetMTRecord.Type = strType;
            assetMTRecord.MTTime = DateTime.Parse(strMTTime);
            assetMTRecord.Description = strDescription;
            assetMTRecord.Cost = decimal.Parse(strCost);

            try
            {            
                assetMTRecordBLL.UpdateAssetMTRecord(assetMTRecord,int.Parse(strID));

                LoadAssetMTRecord();

                if (UpdateCarInformationStatus(strAssetCode, "Maintenance"))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCLWXGXCGWXWCHDCLGLCLDAZWHCLZTXX")+"')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

 

    protected void LoadAssetMTRecord()
    {
        string strAssetID = LB_AssetID.Text.Trim();

        string strHQL = "from AssetMTRecord as assetMTRecord where assetMTRecord.AssetID = " + strAssetID;
        AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();
        IList lst = assetMTRecordBLL.GetAllAssetMTRecords(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();
        IList lst = assetMTRecordBLL.GetAllAssetMTRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }


    /// <summary>
    /// 新增车辆资产维护时，更新车辆档案中车辆状态，维修完成后请到车辆档案中维护车辆状态信息 更新返回true；否则返回false
    /// </summary>
    /// <param name="strCarCode"></param>
    protected bool UpdateCarInformationStatus(string strCarCode, string strStatus)
    {
        bool flag = false;
        string strHQL = " from CarInformation as carInformation where carInformation.CarCode = " + "'" + strCarCode + "'";
        CarInformationBLL carInformationBLL = new CarInformationBLL();
        IList lst = carInformationBLL.GetAllCarInformations(strHQL);
        if (lst.Count > 0)
        {
            CarInformation carInformation = (CarInformation)lst[0];
            carInformation.Status = strStatus;
            carInformationBLL.UpdateCarInformation(carInformation, strCarCode);
            flag = true;
        }
        else
            flag = false;
        return flag;
    }
}
