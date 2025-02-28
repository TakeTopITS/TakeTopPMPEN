using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZPurchaseDetailEditaspx : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataPickingPlanBander();
            DataPurchaseBinder();

            DG_PickingPlanDetail.DataSource = "";
            DG_PickingPlanDetail.DataBind();
            DG_PurchaseDetail.DataSource = "";
            DG_PurchaseDetail.DataBind();
        }
    }


    private void DataPickingPlanBander()
    {
        string strPurchaseCode = LB_Purchase.SelectedValue;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
            string strWZPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
            IList listWZPurchase = wZPurchaseBLL.GetAllWZPurchases(strWZPurchaseHQL);
            if (listWZPurchase != null && listWZPurchase.Count > 0)
            {
                WZPurchase wZPurchase = (WZPurchase)listWZPurchase[0];

                WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                string strWZPickingPlanHQL = string.Format(@"from WZPickingPlan as wZPickingPlan
                            where Progress = '签收'
                            and PurchaseEngineer = '{0}'
                            and ProjectCode = '{1}'
                            and SupplyMethod = '自购'", wZPurchase.PurchaseEngineer, wZPurchase.ProjectCode);
                IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);

                LB_PickingPlan.DataSource = listWZPickingPlan;
                LB_PickingPlan.DataBind();
            }
        }
    }

    private void DataPickingPlanDetailBinder()
    {
        string strPickingPlan = LB_PickingPlan.SelectedValue;
        if (!string.IsNullOrEmpty(strPickingPlan))
        {
            WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
            string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = " + strPickingPlan;
            IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
            DG_PickingPlanDetail.DataSource = listWZPickingPlanDetail;
            DG_PickingPlanDetail.DataBind();
        }
    }


    private void DataPurchaseBinder()
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strWZPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseEngineer = '" + strUserCode + "'";
        IList listWZPurchase = wZPurchaseBLL.GetAllWZPurchases(strWZPurchaseHQL);
        LB_Purchase.DataSource = listWZPurchase;
        LB_Purchase.DataBind();
    }


    private void DataPurchaseDetailBinder()
    {
        if (!string.IsNullOrEmpty(LB_Purchase.SelectedValue))
        {
            WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
            string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode= '" + LB_Purchase.SelectedValue + "'";
            IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
            DG_PurchaseDetail.DataSource = listWZPurchaseDetail;
            DG_PurchaseDetail.DataBind();
        }
    }


    protected void DG_PickPlanDetailDetailList_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "add")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrArges = cmdArges.Split('|');                //ID|ObjectCode|ShortNumber|ShortConver

                string strPurchaseCode = LB_Purchase.SelectedValue;
                if (!string.IsNullOrEmpty(strPurchaseCode))
                {
                    WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                    WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                    string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                    IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                    if (listPurchase != null && listPurchase.Count > 0)
                    {
                        WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
                        WZPurchaseDetail wZPurchaseDetail = new WZPurchaseDetail();
                        wZPurchaseDetail.PurchaseCode = wZPurchase.PurchaseCode;
                        int intPlanDetailID = 0;
                        int.TryParse(arrArges[0], out intPlanDetailID);
                        wZPurchaseDetail.PlanDetailID = intPlanDetailID;
                        wZPurchaseDetail.ObjectCode = arrArges[1];
                        decimal decimalPurchaseNumber = 0;
                        decimal.TryParse(arrArges[2], out decimalPurchaseNumber);
                        wZPurchaseDetail.PurchaseNumber = decimalPurchaseNumber;
                        decimal decimalConvertNumber = 0;
                        decimal.TryParse(arrArges[3], out decimalConvertNumber);
                        wZPurchaseDetail.ConvertNumber = decimalConvertNumber;
                        wZPurchaseDetail.PlanMoney = wZPurchaseDetail.PurchaseNumber * GetMarketByObjectCode(arrArges[1]);
                        wZPurchaseDetail.Progress = "录入";


                        wZPurchaseDetailBLL.AddWZPurchaseDetail(wZPurchaseDetail);
                        //重新计算采购文件里面的条数，预计费用，总价
                        string strSelectPurchaseDetailHQL = string.Format(@"select COUNT(1) as RowNumber,SUM(PlanMoney) as PlanMoney,
                            SUM(TotalMoney) as TotalMoney from T_WZPurchaseDetail where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                        DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strSelectPurchaseDetailHQL, "strSelectPurchaseDetailHQL").Tables[0];
                        if (dtPurchaseDetail != null && dtPurchaseDetail.Rows.Count > 0)
                        {
                            string strUpdatePurchaseHQL = string.Format(@"update T_WZPurchase set RowNumber = {0},PlanMoney={1},TotalMoney={2} where PurchaseCode = '{3}'",
                                dtPurchaseDetail.Rows[0]["RowNumber"], dtPurchaseDetail.Rows[0]["PlanMoney"], dtPurchaseDetail.Rows[0]["TotalMoney"], wZPurchase.PurchaseCode);
                            ShareClass.RunSqlCommand(strUpdatePurchaseHQL);
                        }
                        //修改计划明细的使用标记
                        string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail set Progress = '询价',PurchaseCode='{0}',IsMark = -1 where ID = {1}", wZPurchase.PurchaseCode, intPlanDetailID);
                        ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);
                        //重新加载采购清单
                        DataPurchaseDetailBinder();
                        //计划明细重新加载
                        DataPickingPlanDetailBinder();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZCGWJ+"')", true);
                    return;
                }
            }
        }
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

                WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where ID = " + cmdArges;
                IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count == 1)
                {
                    WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[0];

                    TXT_SerialNumber.Text = wZPurchaseDetail.SerialNumber;
                    TXT_Tenders.Text = wZPurchaseDetail.Tenders;
                    TXT_MajorType.Text = wZPurchaseDetail.MajorType;
                    TXT_PurchaseNumber.Text = wZPurchaseDetail.PurchaseNumber.ToString();
                    TXT_ConvertNumber.Text = wZPurchaseDetail.ConvertNumber.ToString();
                    TXT_PlanMoney.Text = wZPurchaseDetail.PlanMoney.ToString();
                    TXT_Factory.Text = wZPurchaseDetail.Factory;
                    TXT_StandardCode.Text = wZPurchaseDetail.StandardCode;
                    TXT_Remark.Text = wZPurchaseDetail.Remark;

                    HF_PurchaseDetailID.Value = wZPurchaseDetail.ID.ToString();
                    HF_ConvertRatio.Value = GetConvertRatioByObjectCode(wZPurchaseDetail.ObjectCode).ToString();
                    HF_Market.Value = GetMarketByObjectCode(wZPurchaseDetail.ObjectCode).ToString();
                }

            }
            else if (cmdName == "del")
            {
                WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where ID = " + cmdArges;
                IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count == 1)
                {
                    WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[0];

                    if (wZPurchaseDetail.Progress != "录入" || wZPurchaseDetail.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBWLRYJSYBJBW0SBYXSC+"')", true);
                        return;
                    }

                    wZPurchaseDetailBLL.DeleteWZPurchaseDetail(wZPurchaseDetail);

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
                    //修改计划明细的使用标记
                    string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail set Progress = '录入',PurchaseCode='-',IsMark = 0 where ID = {0}", wZPurchaseDetail.ID);
                    ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                    //重新加载采购清单
                    DataPurchaseDetailBinder();
                }
            }
        }
    }

    protected void LB_Purchase_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_Purchase.SelectedValue))
        {
            DataPurchaseDetailBinder();
            DataPickingPlanBander();
        }
    }

    protected void LB_PickingPlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_PickingPlan.SelectedValue))
        {
            DataPickingPlanDetailBinder();
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
            string strMajorType = TXT_MajorType.Text.Trim();
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

            if (string.IsNullOrEmpty(strMajorType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZYLBBNWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPurchaseNumber) || decimalPurchaseNumber <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGSLBNW0HZKXG+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPlanMoney) || decimalPlanMoney <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYJFYBNW0HZKXG+"')", true);
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
                wZPurchaseDetail.MajorType = strMajorType;
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
                DataPurchaseDetailBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
            }

        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZCGD+"')", true);
            return;
        }
    }


    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_PurchaseDetail.Items.Count; i++)
        {
            DG_PurchaseDetail.Items[i].ForeColor = Color.Black;
        }

        TXT_SerialNumber.Text = "";
        TXT_Tenders.Text = "";
        TXT_MajorType.Text = "";
        TXT_PurchaseNumber.Text = "";
        TXT_ConvertNumber.Text = "";
        TXT_PlanMoney.Text = "";
        TXT_Factory.Text = "";
        TXT_StandardCode.Text = "";
        TXT_Remark.Text = "";

        HF_PurchaseDetailID.Value = "";
    }
}