using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZStoreEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            //加载库别
            BindStockData();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindProjectData(intID);
            }
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            WZStoreBLL wZStoreBLL = new WZStoreBLL();
            WZStore wZStore = new WZStore();
            string strStockCode = DDL_StockCode.SelectedValue;          //TXT_StockCode.Text.Trim();
            string strObjectCode = TXT_ObjectCode.Text.Trim();          //物资代码
            string strCheckCode = TXT_CheckCode.Text.Trim();            //检号
            string strYearTime = TXT_YearTime.Text.Trim();              //初始日期
            string strYearNumber = TXT_YearNumber.Text.Trim();          //初始数量
            string strYearPrice = TXT_YearPrice.Text.Trim();           //初始单价
            string strYearMoney = TXT_YearMoney.Text.Trim();           //初始金额
            string strInNumber = TXT_InNumber.Text.Trim();           //入库数量
            string strInMoney = TXT_InMoney.Text.Trim();            //入库金额
            string strEndInTime = TXT_EndInTime.Text.Trim();           //末次入库
            string strOutNumber = TXT_OutNumber.Text.Trim();           //出库数量
            string strOutPrice = TXT_OutPrice.Text.Trim();              //出库金额
            string strEndOutTime = TXT_EndOutTime.Text.Trim();           //末次出库
            string strStoreNumber = TXT_StoreNumber.Text.Trim();           //库存数量
            string strStorePrice = TXT_StorePrice.Text.Trim();           //库存单价
            string strStoreMoney = TXT_StoreMoney.Text.Trim();           //库存金额
            string strGoodsCode = TXT_GoodsCode.Text.Trim();           //货位号
            string strIsMark = TXT_IsMark.Text.Trim();                  //使用标记
            string strDownRatio = TXT_DownRatio.Text.Trim();           //减值比例
            string strDownMoney = TXT_DownMoney.Text.Trim();           //减值金额
            string strCleanMoney = TXT_CleanMoney.Text.Trim();           //净额
            string strDownCode = TXT_DownCode.Text.Trim();           //减值编号
            string strDownDesc = TXT_DownDesc.Text.Trim();           //减值标记
            string strWearyCode = TXT_WearyCode.Text.Trim();           //积压编号
            string strWearyDesc = TXT_WearyDesc.Text.Trim();           //积压标记

            if (string.IsNullOrEmpty(strStockCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZKB").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strStockCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZWZDM").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCheckCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHBNWFFZFC").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strYearTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCSRBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strYearNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCSSLZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strYearPrice))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCSDJZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strYearMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCSJEZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strInNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRKSLZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strInMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRKJEZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strEndInTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMCRKBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strOutNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCKSLZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strOutPrice))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCKJEZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strEndOutTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMCCKBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strStoreNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKCSLZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strStorePrice))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKCDJZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strStoreMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKCJEZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strGoodsCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHWHBNWFFZFC").ToString().Trim()+"')", true);
                return;
            }
            if (strIsMark != "0" && strIsMark != "-1")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBXW0HZ1").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strDownRatio))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJZBLZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strDownMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJZJEZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strCleanMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJEZNSXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDownCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJZBHBNWFFZFC").ToString().Trim()+"')", true);
                return;
            }
            if (strDownDesc != "0" && strDownDesc != "-1")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBXW0HZ1").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWearyCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJYBHBNWFFZFC").ToString().Trim()+"')", true);
                return;
            }
            if (strWearyDesc != "0" && strWearyDesc != "-1")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBXW0HZ1").ToString().Trim()+"')", true);
                return;
            }
           

            wZStore.StockCode = strStockCode;
            wZStore.ObjectCode = strObjectCode;
            wZStore.CheckCode = strCheckCode;
            DateTime dtYearTime = DateTime.Now;
            DateTime.TryParse(strYearTime, out dtYearTime);
            wZStore.YearTime = dtYearTime;
            decimal decimalYearNumber = 0;
            decimal.TryParse(strYearNumber, out decimalYearNumber);
            wZStore.YearNumber = decimalYearNumber;
            decimal decimalYearPrice = 0;
            decimal.TryParse(strYearPrice, out decimalYearPrice);
            wZStore.YearPrice = decimalYearPrice;
            decimal decimalYearMoney = 0;
            decimal.TryParse(strYearMoney, out decimalYearMoney);
            wZStore.YearMoney = decimalYearMoney;
            decimal decimalInNumber = 0;
            decimal.TryParse(strInNumber, out decimalInNumber);
            wZStore.InNumber = decimalInNumber;
            decimal decimalInMoney = 0;
            decimal.TryParse(strInMoney, out decimalInMoney);
            wZStore.InMoney = decimalInMoney;
            DateTime dtEndInTime = DateTime.Now;
            DateTime.TryParse(strEndInTime, out dtEndInTime);
            wZStore.EndInTime = dtEndInTime;
            decimal decimalOutNumber = 0;
            decimal.TryParse(strOutNumber, out decimalOutNumber);
            wZStore.OutNumber = decimalOutNumber;
            decimal decimalOutPrice = 0;
            decimal.TryParse(strOutPrice, out decimalOutPrice);
            wZStore.OutPrice = decimalOutPrice;
            DateTime dtEndOutTime = DateTime.Now;
            DateTime.TryParse(strEndOutTime, out dtEndOutTime);
            wZStore.EndOutTime = dtEndOutTime;
            decimal decimalStoreNumber = 0;
            decimal.TryParse(strStoreNumber, out decimalStoreNumber);
            wZStore.StoreNumber = decimalStoreNumber;
            decimal decimalStorePrice = 0;
            decimal.TryParse(strStorePrice, out decimalStorePrice);
            wZStore.StorePrice = decimalStorePrice;
            decimal decimalStoreMoney = 0;
            decimal.TryParse(strStoreMoney, out decimalStoreMoney);
            wZStore.StoreMoney = decimalStoreMoney;
            wZStore.GoodsCode = strGoodsCode;
            int intIsMark = 0;
            int.TryParse(strIsMark, out intIsMark);
            wZStore.IsMark = intIsMark;
            decimal decimalDownRatio = 0;
            decimal.TryParse(strDownRatio, out decimalDownRatio);
            wZStore.DownRatio = decimalDownRatio;
            decimal decimalDownMoney = 0;
            decimal.TryParse(strDownMoney, out decimalDownMoney);
            wZStore.DownMoney = decimalDownMoney;
            decimal decimalCleanMoney = 0;
            decimal.TryParse(strCleanMoney, out decimalCleanMoney);
            wZStore.CleanMoney = decimalCleanMoney;
            wZStore.DownCode = strDownCode;
            int intDownDesc = 0;
            int.TryParse(strDownDesc, out intDownDesc);
            wZStore.DownDesc = intDownDesc;
            wZStore.WearyCode = strWearyCode;
            int intWearyDesc = 0;
            int.TryParse(strWearyDesc, out intWearyDesc);
            wZStore.WearyDesc = intWearyDesc;
            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);
                wZStoreBLL.UpdateWZStore(wZStore, intID);
            }
            else
            {
                string strCheckWZStoreHQL = string.Format(@"from WZStore as wZStore
                    where StockCode = '{0}'
                    and ObjectCode = '{1}'
                    and CheckCode = '{2}'", strStockCode, strObjectCode, strCheckCode);

                IList lstWZStore = wZStoreBLL.GetAllWZStores(strCheckWZStoreHQL);
                if (lstWZStore != null && lstWZStore.Count == 1)
                {
                    //库存中存在当前物资  直接添加数量

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKCZCZDDKCLBWZDMCJHDLBZZDDWZDJXG").ToString().Trim()+"')", true);
                    return;
                }
                else
                {
                    //增加
                    wZStoreBLL.AddWZStore(wZStore);
                }
            }

            Response.Redirect("TTWZStoreList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindProjectData(int id)
    {
        WZStoreBLL wZStoreBLL = new WZStoreBLL();
        string strWZStoreSql = "from WZStore as wZStore where id = " + id;
        IList listStore = wZStoreBLL.GetAllWZStores(strWZStoreSql);
        if (listStore != null && listStore.Count > 0)
        {
            WZStore wZStore = (WZStore)listStore[0];
            //TXT_StockCode.Text = wZStore.StockCode;
            DDL_StockCode.SelectedValue = wZStore.StockCode;

            TXT_ObjectCode.Text = wZStore.ObjectCode;
            TXT_CheckCode.Text = wZStore.CheckCode;
            TXT_YearTime.Text = wZStore.YearTime.ToString();
            TXT_YearNumber.Text = wZStore.YearNumber.ToString();
            TXT_YearPrice.Text= wZStore.YearPrice.ToString();
            TXT_YearMoney.Text = wZStore.YearMoney.ToString();
            TXT_InNumber.Text = wZStore.InNumber.ToString();
            TXT_InMoney.Text = wZStore.InMoney.ToString();
            TXT_EndInTime.Text = wZStore.EndInTime.ToString();
            TXT_OutNumber.Text = wZStore.OutNumber.ToString();
            TXT_OutPrice.Text = wZStore.OutPrice.ToString();
            TXT_EndOutTime.Text = wZStore.EndOutTime.ToString();
            TXT_StoreNumber.Text = wZStore.StoreNumber.ToString();
            TXT_StorePrice.Text = wZStore.StorePrice.ToString();
            TXT_StoreMoney.Text = wZStore.StoreMoney.ToString();
            TXT_GoodsCode.Text = wZStore.GoodsCode;
            TXT_IsMark.Text = wZStore.IsMark.ToString();
            TXT_DownRatio.Text = wZStore.DownRatio.ToString();
            TXT_DownMoney.Text = wZStore.DownMoney.ToString();
            TXT_CleanMoney.Text = wZStore.CleanMoney.ToString();
            TXT_DownCode.Text = wZStore.DownCode;
            TXT_DownDesc.Text = wZStore.DownDesc.ToString();
            TXT_WearyCode.Text = wZStore.WearyCode;
            TXT_WearyDesc.Text = wZStore.WearyDesc.ToString();
        }
    }

    private void BindStockData()
    {
        WZStockBLL wZStockBLL = new WZStockBLL();
        string strStockHQL = "from WZStock as wZStock";
        IList lstStock = wZStockBLL.GetAllWZStocks(strStockHQL);

        DDL_StockCode.DataSource = lstStock;
        DDL_StockCode.DataTextField = "StockCode";
        DDL_StockCode.DataValueField = "StockCode";
        DDL_StockCode.DataBind();

        DDL_StockCode.Items.Insert(0, new ListItem("-", ""));
    }


}