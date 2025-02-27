using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Drawing;

public partial class TTWZPurchaseDetailEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            if (Request.QueryString["ID"] != null)
            {
                string strID = Request.QueryString["ID"];

                HF_PurchaseDetailID.Value = strID;

                DataPurchaseDetailBinder(strID);
            }
        }
    }


    private void DataPurchaseDetailBinder(string strID)
    {
        WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
        string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where ID = " + strID;
        IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
        if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count == 1)
        {
            WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[0];

            TXT_SerialNumber.Text = wZPurchaseDetail.SerialNumber;
            TXT_Tenders.Text = wZPurchaseDetail.Tenders;

            TXT_PurchaseNumber.Text = wZPurchaseDetail.PurchaseNumber.ToString();
            TXT_ConvertNumber.Text = wZPurchaseDetail.ConvertNumber.ToString();
            TXT_PlanMoney.Text = wZPurchaseDetail.PlanMoney.ToString();
            TXT_Factory.Text = wZPurchaseDetail.Factory;
            TXT_StandardCode.Text = wZPurchaseDetail.StandardCode;
            TXT_Remark.Text = wZPurchaseDetail.Remark;

            HF_PurchaseDetailID.Value = wZPurchaseDetail.ID.ToString();
            HF_ConvertRatio.Value = GetConvertRatioByObjectCode(wZPurchaseDetail.ObjectCode).ToString();
            HF_Market.Value = GetMarketByObjectCode(wZPurchaseDetail.ObjectCode).ToString();
            HF_PurchaseCode.Value = wZPurchaseDetail.PurchaseCode;


            TXT_SerialNumber.BackColor = Color.CornflowerBlue;
            TXT_Tenders.BackColor = Color.CornflowerBlue;

            TXT_PurchaseNumber.BackColor = Color.CornflowerBlue;
            TXT_ConvertNumber.BackColor = Color.CornflowerBlue;
            TXT_PlanMoney.BackColor = Color.CornflowerBlue;
            TXT_Factory.BackColor = Color.CornflowerBlue;
            TXT_StandardCode.BackColor = Color.CornflowerBlue;
            TXT_Remark.BackColor = Color.CornflowerBlue;
        }
    }



    /// <summary>
    /// 获得物资代码的市场行情
    /// </summary>
    private decimal GetMarketByObjectCode(string strObjectCode)
    {
        decimal decimalResult = 0;
        string strObjectMarketHQL = string.Format("select Market from T_WZObject where ObjectCode = '{0}'", strObjectCode);
        DataTable dtObjectMarket = ShareClass.GetDataSetFromSql(strObjectMarketHQL, "Market").Tables[0];
        if (dtObjectMarket != null && dtObjectMarket.Rows.Count > 0)
        {
            decimal.TryParse(ShareClass.ObjectToString(dtObjectMarket.Rows[0]["Market"]), out decimalResult);
        }
        return decimalResult;
    }


    /// <summary>
    /// 获得物资代码的换算系数
    /// </summary>
    private decimal GetConvertRatioByObjectCode(string strObjectCode)
    {
        decimal decimalResult = 0;
        string strObjectConvertRatioHQL = string.Format("select ConvertRatio from T_WZObject where ObjectCode = '{0}'", strObjectCode);
        DataTable dtObjectConvertRatio = ShareClass.GetDataSetFromSql(strObjectConvertRatioHQL, "Market").Tables[0];
        if (dtObjectConvertRatio != null && dtObjectConvertRatio.Rows.Count > 0)
        {
            decimal.TryParse(ShareClass.ObjectToString(dtObjectConvertRatio.Rows[0]["ConvertRatio"]), out decimalResult);
        }
        return decimalResult;
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strPurchaseDetailID = HF_PurchaseDetailID.Value;
        if (!string.IsNullOrEmpty(strPurchaseDetailID))
        {
            string strSerialNumber = TXT_SerialNumber.Text.Trim();
            string strTenders = TXT_Tenders.Text.Trim();
            //string strMajorType = TXT_MajorType.Text.Trim();
            string strPurchaseNumber = TXT_PurchaseNumber.Text.Trim();
            decimal decimalPurchaseNumber = 0;
            decimal.TryParse(strPurchaseNumber, out decimalPurchaseNumber);
            string strConvertNumber = TXT_ConvertNumber.Text.Trim();
            decimal decimalConvertNumber = 0;
            decimal.TryParse(strConvertNumber, out decimalConvertNumber);
            string strPlanMoney = TXT_PlanMoney.Text.Trim();
            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);

            string strFactory = TXT_Factory.Text.Trim();
            string strStandardCode = TXT_StandardCode.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();


            if (!ShareClass.CheckIsNumber(strPurchaseNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGSLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }
            if (decimalPurchaseNumber <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGSLBXDY0").ToString().Trim() + "')", true);
                return;
            }

            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJFYBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }
            if (decimalPlanMoney <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJFYBXDY0KYGGGSZ").ToString().Trim() + "')", true);
                return;
            }


            WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
            string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where ID = " + strPurchaseDetailID;
            IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
            if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count == 1)
            {
                WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[0];

                wZPurchaseDetail.SerialNumber = strSerialNumber;
                wZPurchaseDetail.Tenders = strTenders;
                //wZPurchaseDetail.MajorType = strMajorType;
                wZPurchaseDetail.PurchaseNumber = decimalPurchaseNumber;
                wZPurchaseDetail.ConvertNumber = decimalConvertNumber;
                wZPurchaseDetail.PlanMoney = decimalPlanMoney;
                wZPurchaseDetail.Factory = strFactory;
                wZPurchaseDetail.StandardCode = strStandardCode;
                wZPurchaseDetail.Remark = strRemark;

                wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);

                //重新计算采购文件里面的条数，预计费用，总价
                string strSelectPurchaseDetailHQL = string.Format(@"select COUNT(1) as RowNumber,SUM(PlanMoney) as PlanMoney,
                            SUM(TotalMoney) as TotalMoney from T_WZPurchaseDetail where PurchaseCode = '{0}'", wZPurchaseDetail.PurchaseCode);
                DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strSelectPurchaseDetailHQL, "strSelectPurchaseDetailHQL").Tables[0];
                if (dtPurchaseDetail != null && dtPurchaseDetail.Rows.Count > 0)
                {
                    string strUpdatePurchaseHQL = string.Format(@"update T_WZPurchase set RowNumber = {0},PlanMoney={1},TotalMoney={2} where PurchaseCode = '{3}'",
                        dtPurchaseDetail.Rows[0]["RowNumber"], dtPurchaseDetail.Rows[0]["PlanMoney"], dtPurchaseDetail.Rows[0]["TotalMoney"], wZPurchaseDetail.PurchaseCode);
                    ShareClass.RunSqlCommand(strUpdatePurchaseHQL);
                }

                //重新加载采购清单
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCGD").ToString().Trim() + "')", true);
            return;
        }
    }


    protected void BT_PurchaseNumber_Click(object sender, EventArgs e)
    {
        string strPurchaseDetailID = HF_PurchaseDetailID.Value;
        if (!string.IsNullOrEmpty(strPurchaseDetailID))
        {
            string strPurchaseNumber = TXT_PurchaseNumber.Text.Trim();
            decimal decimalPurchaseNumber = 0;
            decimal.TryParse(strPurchaseNumber, out decimalPurchaseNumber);

            if (string.IsNullOrEmpty(strPurchaseNumber) || decimalPurchaseNumber <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGSLBNW0HZKXG").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strPurchaseNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGSLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalConvertRatio = 0;
            decimal.TryParse(HF_ConvertRatio.Value, out decimalConvertRatio);
            decimal decimalMarket = 0;
            decimal.TryParse(HF_Market.Value, out decimalMarket);

            decimal decimalConvertNumber = 0;
            decimal decimalPlanMoney = 0;

            if (decimalConvertRatio != 0)
            {
                decimalConvertNumber = decimalPurchaseNumber / decimalConvertRatio;
            }
            decimalPlanMoney = decimalPurchaseNumber * decimalMarket;

            TXT_ConvertNumber.Text = decimalConvertNumber.ToString("#0.000");
            TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.000");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCGD").ToString().Trim() + "')", true);
            return;
        }
    }


    protected void BT_ConvertNumber_Click(object sender, EventArgs e)
    {
        string strPurchaseDetailID = HF_PurchaseDetailID.Value;
        if (!string.IsNullOrEmpty(strPurchaseDetailID))
        {
            string strConvertNumber = TXT_ConvertNumber.Text.Trim();
            decimal decimalConvertNumber = 0;
            decimal.TryParse(strConvertNumber, out decimalConvertNumber);

            if (string.IsNullOrEmpty(strConvertNumber) || decimalConvertNumber <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJFYBNW0HZKXG").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strConvertNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJFYBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalConvertRatio = 0;
            decimal.TryParse(HF_ConvertRatio.Value, out decimalConvertRatio);
            decimal decimalMarket = 0;
            decimal.TryParse(HF_Market.Value, out decimalMarket);

            decimal decimalPurchaseNumber = 0;
            decimal decimalPlanMoney = 0;

            decimalPurchaseNumber = decimalConvertNumber * decimalConvertRatio;
            decimalPlanMoney = decimalPurchaseNumber * decimalMarket;

            TXT_PurchaseNumber.Text = decimalPurchaseNumber.ToString("#0.000");
            TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.000");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCGD").ToString().Trim() + "')", true);
            return;
        }
    }
}