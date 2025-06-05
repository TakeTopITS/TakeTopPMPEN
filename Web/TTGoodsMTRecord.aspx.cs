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

public partial class TTGoodsMTRecord : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string strGoodsID = Request.QueryString["ID"];
        string strUserCode = Session["UserCode"].ToString();

        string strHQL;
        IList lst;


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_MTTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from MTType as mtType Order By mtType.SortNumber ASC";
            MTTypeBLL mtTypeBLL = new MTTypeBLL();
            lst = mtTypeBLL.GetAllMTTypes(strHQL);

            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            TB_MTManCode.Text = strUserCode;
            LB_GoodsID.Text = strGoodsID;

            GoodsBLL goodsBLL = new GoodsBLL();
            Goods goods = new Goods();
            strHQL = "from Goods as goods where goods.ID =" + strGoodsID;
            lst = goodsBLL.GetAllGoodss(strHQL);
            goods = (Goods)lst[0];

            LB_GoodsCode.Text = goods.GoodsCode;

            if (goods.Number == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoCiWuLiaoShuLiangWei0B")+"')", true);
            }

            strHQL = "from GoodsMTRecord as goodsMTRecord where goodsMTRecord.GoodsID = " + strGoodsID;
            GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();
            lst = goodsMTRecordBLL.GetAllGoodsMTRecords(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType;

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

                string strHQL = "from GoodsMTRecord as goodsMTRecord where goodsMTRecord.ID= " + strID;

                GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();

                IList lst = goodsMTRecordBLL.GetAllGoodsMTRecords(strHQL);

                GoodsMTRecord goodsMTRecord = (GoodsMTRecord)lst[0];

                LB_ID.Text = goodsMTRecord.ID.ToString();
                TB_MTManCode.Text = goodsMTRecord.MTManCode.ToString();

                TB_Description.Text = goodsMTRecord.Description;
                TB_Cost.Amount = goodsMTRecord.Cost;
                DLC_MTTime.Text = goodsMTRecord.MTTime.ToString("yyyy-MM-dd");

                strType = goodsMTRecord.Type;

                DL_Type.SelectedValue = strType;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                if (strID != "")
                {
                    GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();
                    GoodsMTRecord goodsMTRecord = new GoodsMTRecord();

                    goodsMTRecord.ID = int.Parse(strID);
                    goodsMTRecord.GoodsCode = LB_GoodsCode.Text.Trim();

                    try
                    {
                        goodsMTRecordBLL.DeleteGoodsMTRecord(goodsMTRecord);

                        //BT_Update.Enabled = false;
                        //BT_Delete.Enabled = false;

                        LoadGoodsMTRecord();

                        UpdateCarInformationStatus(goodsMTRecord.GoodsCode, "InUse");

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
        string strGoodsID, strGoodsCode, strGoodsName, strID, strType;
        string strDescription, strMTTime;
        string strMTManCode;
        decimal deCost;

        GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();
        GoodsMTRecord goodsMTRecord = new GoodsMTRecord();

        strGoodsID = LB_GoodsID.Text.Trim();
        strGoodsCode = LB_GoodsCode.Text.Trim();
        
        if (strGoodsCode != "")
        {
            strGoodsName = ShareClass.GetGoodsName(strGoodsCode);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoWuLiaoShuLiangWei0BuY")+"')", true);
            return;
        }

        strMTManCode = TB_MTManCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strMTTime = DLC_MTTime.Text;
        deCost = TB_Cost.Amount;

        if (strMTManCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHBNWKJC") + "')", true);
        }
        else
        {
            goodsMTRecord.GoodsID = int.Parse(strGoodsID);
            goodsMTRecord.MTManCode = strMTManCode;
            goodsMTRecord.MTManName = ShareClass.GetUserName(strMTManCode);
            goodsMTRecord.GoodsCode = strGoodsCode;
            goodsMTRecord.GoodsName = strGoodsName;
            goodsMTRecord.Type = strType;
            goodsMTRecord.MTTime = DateTime.Parse(strMTTime);
            goodsMTRecord.Description = strDescription;
            goodsMTRecord.Cost = deCost;

            try
            {
                goodsMTRecordBLL = new GoodsMTRecordBLL();
                goodsMTRecordBLL.AddGoodsMTRecord(goodsMTRecord);
                strID = ShareClass.GetMyCreatedMaxGoodsMtRecordID(strGoodsCode);
                LB_ID.Text = strID;

                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;

                LoadGoodsMTRecord();
                if (UpdateCarInformationStatus(strGoodsCode, "Maintenance"))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCLWXXZCGWXWCHDCLGLCLDAZWHCLZTXX") + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }


    protected void UpdateMTRecord()
    {
        string strGoodsCode, strID, strGoodsID, strType;
        string strDescription, strMTTime;
        string strMTManCode, strCost;


        strID = LB_ID.Text.Trim();

        GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();
        GoodsMTRecord goodsMTRecord = new GoodsMTRecord();

        strGoodsID = LB_GoodsID.Text.Trim();
        strGoodsCode = LB_GoodsCode.Text.Trim();
        strMTManCode = TB_MTManCode.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strMTTime = DLC_MTTime.Text;
        strCost = TB_Cost.Amount.ToString().Trim();

        if (strCost == "")
            strCost = "0";

        if (strID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHBNWKJC") + "')", true);
        }
        else
        {
            goodsMTRecord.GoodsID = int.Parse(strGoodsID);
            goodsMTRecord.MTManCode = strMTManCode;
            goodsMTRecord.MTManName = ShareClass.GetUserName(strMTManCode);
            goodsMTRecord.GoodsCode = strGoodsCode;
            goodsMTRecord.Type = strType;
            goodsMTRecord.MTTime = DateTime.Parse(strMTTime);
            goodsMTRecord.Description = strDescription;
            goodsMTRecord.Cost = decimal.Parse(strCost);

            try
            {
                goodsMTRecordBLL.UpdateGoodsMTRecord(goodsMTRecord, int.Parse(strID));

                LoadGoodsMTRecord();

                if (UpdateCarInformationStatus(strGoodsCode, "Maintenance"))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCLWXGXCGWXWCHDCLGLCLDAZWHCLZTXX") + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }
    

    protected void LoadGoodsMTRecord()
    {
        string strGoodsID = LB_GoodsID.Text.Trim();

        string strHQL = "from GoodsMTRecord as goodsMTRecord where goodsMTRecord.GoodsID = " + strGoodsID;
        GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();
        IList lst = goodsMTRecordBLL.GetAllGoodsMTRecords(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }



    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();
        IList lst = goodsMTRecordBLL.GetAllGoodsMTRecords(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }




    /// <summary>
    /// 新增车辆物料维护时，更新车辆档案中车辆状态，维修完成后请到车辆档案中维护车辆状态信息 更新返回true；否则返回false
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
