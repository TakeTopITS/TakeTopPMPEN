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

public partial class TTWZPurchaseApplyDetail : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (Request.QueryString["PurchaseCode"] != null)
            {
                string strPurchaseCode = Request.QueryString["PurchaseCode"];

                HF_PurchaseCode.Value = strPurchaseCode;

                DataPurchaseDetailBinder(strPurchaseCode);
            }
        }
    }

    private void DataPurchaseDetailBinder(string strPurchaseCode)
    {
        #region 正式版
        string strWZPurchaseDetailHQL = string.Format(@"select o.*,z.PlanCode,s.UnitName from T_WZPurchaseOfferRecord o
                    left join T_WZSpan s on o.Unit = s.ID 
                    left join T_WZPickingPlanDetail z on o.PlanDetailID = z.ID
                    where o.SupplierCode = '{0}'
                    and o.PurchaseCode = '{1}'", strUserCode, strPurchaseCode);
        #endregion

        DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "PurchaseDetail").Tables[0];

        DG_PurchaseDetail.DataSource = dtPurchaseDetail;
        DG_PurchaseDetail.DataBind();

        LB_Sql.Text = strWZPurchaseDetailHQL;

        LB_TotalApplyAmount.Text = SumPurchaseDetailTotalPrice(strPurchaseCode).ToString();
    }


    protected decimal SumPurchaseDetailTotalPrice(string strPurchaseCode)
    {
        #region 正式版
        string strWZPurchaseDetailHQL = string.Format(@"select sum(TotalMoney) from T_WZPurchaseOfferRecord o
                    left join T_WZSpan s on o.Unit = s.ID 
                    left join T_WZPickingPlanDetail z on o.PlanDetailID = z.ID
                    where o.SupplierCode = '{0}'
                    and o.PurchaseCode = '{1}'", strUserCode, strPurchaseCode);
        #endregion

        DataSet ds = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "T_WZPurchaseOfferRecord");

        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                return 0;

            }
        }
        else
        {
            return 0;
        }

    }


    protected void DG_PurchaseDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_PurchaseDetail.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strHQL, "PurchaseDetail").Tables[0];

        DG_PurchaseDetail.DataSource = dtPurchaseDetail;
        DG_PurchaseDetail.DataBind();
    }


    protected void DG_PurchaseDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            string cmdArges = e.CommandArgument.ToString();
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_PurchaseDetail.Items.Count; i++)
                {
                    DG_PurchaseDetail.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string[] arrPurchaseDetail = cmdArges.Split('|');

                TXT_ObjectCode.Text = arrPurchaseDetail[1];
                TXT_ObjectName.Text = arrPurchaseDetail[2];

                TXT_ApplyMoney.Text = arrPurchaseDetail[3];
                TXT_ReplaceCode.Text = arrPurchaseDetail[4];

                HF_PurchaseDetailID.Value = arrPurchaseDetail[5];
                HF_PurchaseOfferRecordID.Value = arrPurchaseDetail[0];
            }
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strPurchaseDetailID = HF_PurchaseDetailID.Value;
        string strPurchaseOfferRecordID = HF_PurchaseOfferRecordID.Value;
        if (!string.IsNullOrEmpty(strPurchaseOfferRecordID))
        {
            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
            string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + HF_PurchaseCode.Value + "'";
            IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
            if (listPurchase != null && listPurchase.Count == 1)
            {
                WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                DateTime dtEditPurchaseEndTime = DateTime.Now;
                DateTime.TryParse(wZPurchase.PurchaseEndTime, out dtEditPurchaseEndTime);

                //if (int.Parse(dtEditPurchaseEndTime.ToString("yyyyMMdd")) < int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd")))
                //{
                //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJJZRYGBNZBJ").ToString().Trim() + "')", true);
                //    return;
                //}

                DateTime dtPurchaseStartTime = DateTime.Now;
                DateTime.TryParse(wZPurchase.PurchaseStartTime, out dtPurchaseStartTime);

                //if (int.Parse(DateTime.Now.ToString("yyyyMMdd")) < int.Parse(dtPurchaseStartTime.AddDays(1).ToString("yyyyMMdd")))
                //{
                //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJKSRWDBNBJ").ToString().Trim() + "')", true);
                //    return;
                //}

                if (wZPurchase.Progress != LanguageHandle.GetWord("XunJia").ToString().Trim())
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSJBNZBJ").ToString().Trim() + "')", true);
                    return;
                }
            }


            //判断是否已经提交
            //string strCheckPurchaseSupplierSQL = string.Format(@"select * from T_WZPurchaseOfferRecord
            //                    where SupplierCode = '{0}'
            //                    and PurchaseCode = '{1}'", strUserCode, HF_PurchaseCode.Value);

            string strCheckPurchaseSupplierSQL = string.Format(@"select * from T_WZPurchaseOfferRecord
                                where ID = {0} ", strPurchaseOfferRecordID);
            DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierSQL, "CheckPurchaseSupplier").Tables[0];
            if (dtCheckPurchaseSupplier != null && dtCheckPurchaseSupplier.Rows.Count > 0)
            {
                if (ShareClass.ObjectToString(dtCheckPurchaseSupplier.Rows[0]["Progress"]) == LanguageHandle.GetWord("BaoJia").ToString().Trim())
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJYJTJBNBCBJMXXTH").ToString().Trim() + "')", true);
                    return;
                }
            }

            string strTenders = ShareClass.ObjectToString(dtCheckPurchaseSupplier.Rows[0]["Tenders"]);
            string strApplyMoney = TXT_ApplyMoney.Text.Trim();

            if (!ShareClass.CheckIsNumber(strApplyMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }

            int intPurchaseOfferRecordID = 0;
            int.TryParse(strPurchaseOfferRecordID, out intPurchaseOfferRecordID);

            decimal decimalApplyMoney = 0;
            decimal.TryParse(strApplyMoney, out decimalApplyMoney);

            //如果同一标段存在报价大于零的，那么这个报价也要大于零
            if (checkExistMoreThanZeroPurchaseApplyPriceRecord(HF_PurchaseCode.Value))
            {
                if (decimalApplyMoney == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoTongYiBiaoDuanMoQuanB").ToString().Trim()+"')", true);
                    return;
                }
            }

            string strReplaceCode = TXT_ReplaceCode.Text.Trim();

            WZPurchaseOfferRecordBLL wZPurchaseOfferRecordBLL = new WZPurchaseOfferRecordBLL();
            string strWZPurchaseOfferRecordHQL = "from WZPurchaseOfferRecord as wZPurchaseOfferRecord where wZPurchaseOfferRecord.ID = " + intPurchaseOfferRecordID.ToString();
            IList listWZPurchaseOfferRecord = wZPurchaseOfferRecordBLL.GetAllWZPurchaseOfferRecords(strWZPurchaseOfferRecordHQL);
            if (listWZPurchaseOfferRecord != null && listWZPurchaseOfferRecord.Count == 1)
            {
                //修改
                WZPurchaseOfferRecord wZPurchaseOfferRecord = (WZPurchaseOfferRecord)listWZPurchaseOfferRecord[0];

                wZPurchaseOfferRecord.ApplyMoney = decimalApplyMoney;

                //报价合计
                decimal decimalTotalMoney = wZPurchaseOfferRecord.PurchaseNumber * decimalApplyMoney;
                wZPurchaseOfferRecord.TotalMoney = decimalTotalMoney;

                wZPurchaseOfferRecord.ReplaceCode = strReplaceCode;

                wZPurchaseOfferRecordBLL.UpdateWZPurchaseOfferRecord(wZPurchaseOfferRecord, wZPurchaseOfferRecord.ID);
            }

            //重新加载采购清单
            DataPurchaseDetailBinder(HF_PurchaseCode.Value);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCGD").ToString().Trim() + "')", true);
            return;
        }
    }

    //判断是否存在报价大于零的记录
    protected bool checkExistMoreThanZeroPurchaseApplyPriceRecord(string strPurchaseCode)
    {
        #region 正式版
        string strWZPurchaseDetailHQL = string.Format(@"select o.*,z.PlanCode,s.UnitName from T_WZPurchaseOfferRecord o
                    left join T_WZSpan s on o.Unit = s.ID 
                    left join T_WZPickingPlanDetail z on o.PlanDetailID = z.ID
                    where o.ApplyMoney > 0 and o.SupplierCode = '{0}'
                    and o.PurchaseCode = '{1}'", strUserCode, strPurchaseCode);
        #endregion

        DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "PurchaseDetail").Tables[0];

        if (dtPurchaseDetail.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void BT_Close_Click(object sender, EventArgs e)
    {
        decimal decimalApplyMoney = 0;
        string strPurchaseDetailID = HF_PurchaseDetailID.Value;
        string strPurchaseOfferRecordID = HF_PurchaseOfferRecordID.Value;
        string strTenders;

        string strReplaceCode = TXT_ReplaceCode.Text.Trim();

        WZPurchaseOfferRecordBLL wZPurchaseOfferRecordBLL = new WZPurchaseOfferRecordBLL();
        string strWZPurchaseOfferRecordHQL = "from WZPurchaseOfferRecord as wZPurchaseOfferRecord where wZPurchaseOfferRecord.PurchaseCode = '" + HF_PurchaseCode.Value + "'";
        IList listWZPurchaseOfferRecord = wZPurchaseOfferRecordBLL.GetAllWZPurchaseOfferRecords(strWZPurchaseOfferRecordHQL);

        WZPurchaseOfferRecord wZPurchaseOfferRecord;

        for (int i = 0; i < listWZPurchaseOfferRecord.Count; i++)
        {
            //修改
            wZPurchaseOfferRecord = (WZPurchaseOfferRecord)listWZPurchaseOfferRecord[i];

            strPurchaseOfferRecordID = wZPurchaseOfferRecord.ID.ToString();
            decimalApplyMoney = wZPurchaseOfferRecord.ApplyMoney;
            strTenders = wZPurchaseOfferRecord.Tenders.Trim();

            //检查同一标段是否有不同金额
            if (!CheckValueOfPurchaseOfferRecord(strUserCode, HF_PurchaseCode.Value, strPurchaseOfferRecordID, strTenders, decimalApplyMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoTongYiBiaoDuanWeiQuan").ToString().Trim()+"')", true);
                return;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.returnValue = false;  CloseLayer();", true);
    }


    //检查同一标段是否全部报价或全部未报价
    protected bool CheckValueOfPurchaseOfferRecord(string strUserCode, string strPurchaseCode, string strOfferRecordID, string strTenders, decimal deMoneyNumber)
    {
        if (deMoneyNumber > 0)
        {
            string strWZPurchaseDetailHQL = string.Format(@"select o.*,z.PlanCode,s.UnitName from T_WZPurchaseOfferRecord o
                    left join T_WZSpan s on o.Unit = s.ID 
                    left join T_WZPickingPlanDetail z on o.PlanDetailID = z.ID
                    where o.SupplierCode = '{0}'
                    and o.ApplyMoney = 0
                    and o.PurchaseCode = '{1}'
                    and o.Tenders = '{2}' 
                    and o.ID <> {3} ", strUserCode, strPurchaseCode, strTenders, strOfferRecordID);

            DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "PurchaseDetail").Tables[0];
            if (dtPurchaseDetail.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        if (deMoneyNumber == 0)
        {
            string strWZPurchaseDetailHQL = string.Format(@"select o.*,z.PlanCode,s.UnitName from T_WZPurchaseOfferRecord o
                    left join T_WZSpan s on o.Unit = s.ID 
                    left join T_WZPickingPlanDetail z on o.PlanDetailID = z.ID
                    where o.SupplierCode = '{0}'
                    and o.ApplyMoney > 0
                    and o.PurchaseCode = '{1}'
                    and o.Tenders = '{2}' 
                    and o.ID <> {3} ", strUserCode, strPurchaseCode, strTenders, strOfferRecordID);

            DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "PurchaseDetail").Tables[0];
            if (dtPurchaseDetail.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return true;
    }

    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_PurchaseDetail.Items.Count; i++)
        {
            DG_PurchaseDetail.Items[i].ForeColor = Color.Black;
        }

        TXT_ObjectCode.Text = "";
        TXT_ApplyMoney.Text = "";
        TXT_ObjectName.Text = "";

        HF_PurchaseDetailID.Value = "";
        HF_PurchaseOfferRecordID.Value = "";
    }

}