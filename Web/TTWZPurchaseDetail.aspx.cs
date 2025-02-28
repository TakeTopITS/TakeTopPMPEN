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

public partial class TTWZPurchaseDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            if (Request.QueryString["PurchaseCode"] != null)
            {
                string strPurchaseCode = Request.QueryString["PurchaseCode"];

                HF_PurchaseCode.Value = strPurchaseCode;

                DataPickingPlanBander();

                DG_PickingPlanDetail.DataSource = "";
                DG_PickingPlanDetail.DataBind();

                DataPurchaseDetailBinder(strPurchaseCode);
            }
        }
    }


    private void DataPickingPlanBander()
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
            string strWZPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
            IList listWZPurchase = wZPurchaseBLL.GetAllWZPurchases(strWZPurchaseHQL);
            if (listWZPurchase != null && listWZPurchase.Count > 0)
            {
                WZPurchase wZPurchase = (WZPurchase)listWZPurchase[0];

                WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                string strWZPickingPlanHQL = string.Format(@"from WZPickingPlan  as wZPickingPlan
                            where Progress = 'Sign for Receipt'
                            and PurchaseEngineer = '{0}'
                            and ProjectCode = '{1}'
                            and SupplyMethod = 'Self-purchase'
                            order by PlanCode", wZPurchase.PurchaseEngineer, wZPurchase.ProjectCode);   //ChineseWord
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

            string strWZPickingPlanDetailHQL = string.Format(@"select d.*,o.ObjectName,o.Model,o.Criterion,o.Grade,s.UnitName,c.UnitName as ConvertUnitName,o.ConvertRatio from T_WZPickingPlanDetail d
                                        left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                                        left join T_WZSpan s on o.Unit = s.ID
                                        left join T_WZSpan c on o.ConvertUnit = c.ID
                                        where d.PlanCode = '{0}'
                                        order by o.DLCode,o.ObjectName,o.Model", strPickingPlan);
            DataTable dtPickingPlanDetail = ShareClass.GetDataSetFromSql(strWZPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];

            DG_PickingPlanDetail.DataSource = dtPickingPlanDetail;
            DG_PickingPlanDetail.DataBind();

            string strObjectCode, strOldObjectCode;
            for (int i = 0; i < DG_PickingPlanDetail.Items.Count; i++)
            {
                strObjectCode = DG_PickingPlanDetail.Items[i].Cells[16].Text;
                strOldObjectCode = DG_PickingPlanDetail.Items[i].Cells[22].Text;

                if (strObjectCode == strOldObjectCode)
                {
                    DG_PickingPlanDetail.Items[i].Cells[22].Text = "";
                }
            }
        }
    }



    private void DataPurchaseDetailBinder(string strPurchaseCode)
    {
        //DG_PurchaseDetail.CurrentPageIndex = 0;

        //string strWZPurchaseDetailHQL = string.Format(@"select d.*,p.PlanCode,t.DivideType,o.ObjectName from T_WZPurchaseDetail d
        //                        left join T_WZPickingPlanDetail p on d.PlanDetailID = p.ID
        //                        left join T_WZDivide t on d.MajorType = t.DivideCode
        //                        left join T_WZObject o on d.ObjectCode = o.ObjectCode
        //                        where d.PurchaseCode= '{0}'", strPurchaseCode);
        string strWZPurchaseDetailHQL = string.Format(@"select d.*,p.PlanCode,o.ObjectName,o.Model,o.Criterion,o.Grade,s.UnitName from T_WZPurchaseDetail d
                                left join T_WZPickingPlanDetail p on d.PlanDetailID = p.ID
                                left join T_WZObject o on d.ObjectCode = o.ObjectCode
                                left join T_WZSpan s on o.Unit = s.ID
                                where d.PurchaseCode= '{0}'
                                order by o.DLCode,o.ObjectName,o.Model", strPurchaseCode);
        DataTable dtWZPurchaseDetail = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "PurchaseDetail").Tables[0];

        DG_PurchaseDetail.DataSource = dtWZPurchaseDetail;
        DG_PurchaseDetail.DataBind();

        LB_Sql.Text = strWZPurchaseDetailHQL;

        LB_ShowCount.Text = dtWZPurchaseDetail.Rows.Count.ToString();
        #region 注释原来的
        //WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
        //string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode= '" + strPurchaseCode + "'";
        //IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);

        //DG_PurchaseDetail.DataSource = listWZPurchaseDetail;
        //DG_PurchaseDetail.DataBind();
        #endregion
    }


    protected void DG_PurchaseDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_PurchaseDetail.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtWZPurchaseDetail = ShareClass.GetDataSetFromSql(strHQL, "PurchaseDetail").Tables[0];

        DG_PurchaseDetail.DataSource = dtWZPurchaseDetail;
        DG_PurchaseDetail.DataBind();
    }

    protected void DG_PickPlanDetailDetailList_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "add")
            {
                try
                {
                    string cmdArges = e.CommandArgument.ToString();
                    string[] arrArges = cmdArges.Split('|');                //ID|ObjectCode|ShortNumber|ShortConver

                    string strPurchaseCode = HF_PurchaseCode.Value;
                    if (!string.IsNullOrEmpty(strPurchaseCode))
                    {
                        WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();

                        //判断计划明细是否已经添加在采购清单里面
                        string strCheckPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode = '" + strPurchaseCode + "' and PlanDetailID = '" + arrArges[0] + "'";
                        IList lstCheckPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strCheckPurchaseDetailHQL);
                        if (lstCheckPurchaseDetail != null && lstCheckPurchaseDetail.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGJHMXZCGDZYJCZBNZTJ").ToString().Trim() + "')", true);
                            return;
                        }

                        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();

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
                            wZPurchaseDetail.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                            wZPurchaseDetail.IsMark = 0;

                            //专业类别
                            wZPurchaseDetail.MajorType = arrArges[1].Substring(0, 2);//strDivideType;

                            wZPurchaseDetailBLL.AddWZPurchaseDetail(wZPurchaseDetail);

                            //重新计算采购文件里面的条数，预计费用，总价
                            string strSelectPurchaseDetailHQL = string.Format(@"select COUNT(1) as RowNumber,SUM(PlanMoney) as PlanMoney,
                            SUM(TotalMoney) as TotalMoney from T_WZPurchaseDetail where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                            DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strSelectPurchaseDetailHQL, "strSelectPurchaseDetailHQL").Tables[0];
                            if (dtPurchaseDetail != null && dtPurchaseDetail.Rows.Count > 0)
                            {
                                string strUpdatePurchaseHQL = string.Format(@"update T_WZPurchase set RowNumber = {0},PlanMoney={1},TotalMoney={2},IsMark=-1 where PurchaseCode = '{3}'",
                                    dtPurchaseDetail.Rows[0]["RowNumber"], dtPurchaseDetail.Rows[0]["PlanMoney"], dtPurchaseDetail.Rows[0]["TotalMoney"], wZPurchase.PurchaseCode);
                                ShareClass.RunSqlCommand(strUpdatePurchaseHQL);
                            }
                            //修改计划明细的使用标记
                            string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail set Progress = '询价',PurchaseCode='{0}',IsMark = -1 where ID = {1}", wZPurchase.PurchaseCode, intPlanDetailID);   //ChineseWord
                            ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);
                            //重新加载采购清单
                            DataPurchaseDetailBinder(strPurchaseCode);
                            //计划明细重新加载
                            DataPickingPlanDetailBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZTianJiaChengGong").ToString().Trim()+"')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCGWJ").ToString().Trim() + "')", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJYCYCYYEXMESSAGE").ToString().Trim() + "')", true);
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

                    if (wZPurchaseDetail.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZPurchaseDetail.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGDJDBSLRHZSYBW0BNBJ").ToString().Trim() + "')", true);
                        return;
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseDetailEdit.aspx?ID=" + cmdArges + "');", true);
                   
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

                    if (wZPurchaseDetail.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZPurchaseDetail.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRYJSYBJBW0SBYXSC").ToString().Trim() + "')", true);
                        return;
                    }

                    wZPurchaseDetailBLL.DeleteWZPurchaseDetail(wZPurchaseDetail);

                    //重新计算采购文件里面的条数，预计费用，总价
                    string strSelectPurchaseDetailHQL = string.Format(@"select COUNT(1) as RowNumber,COALESCE(SUM(PlanMoney),0) as PlanMoney,
                            COALESCE(SUM(TotalMoney),0) as TotalMoney from T_WZPurchaseDetail where PurchaseCode = '{0}'", wZPurchaseDetail.PurchaseCode);
                    DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strSelectPurchaseDetailHQL, "strSelectPurchaseDetailHQL").Tables[0];
                    if (dtPurchaseDetail.Rows.Count > 0)
                    {
                        string strUpdatePurchaseHQL = string.Format(@"update T_WZPurchase set RowNumber = {0},PlanMoney={1},TotalMoney={2} where PurchaseCode = '{3}'",
                            dtPurchaseDetail.Rows[0]["RowNumber"], dtPurchaseDetail.Rows[0]["PlanMoney"], dtPurchaseDetail.Rows[0]["TotalMoney"], wZPurchaseDetail.PurchaseCode);
                        ShareClass.RunSqlCommand(strUpdatePurchaseHQL);


                        if (dtPurchaseDetail.Rows[0]["RowNumber"].ToString() == "0")
                        {
                            strUpdatePurchaseHQL = string.Format(@"update T_WZPurchase set IsMark = 0 where PurchaseCode = '{0}'", wZPurchaseDetail.PurchaseCode);
                            ShareClass.RunSqlCommand(strUpdatePurchaseHQL);
                        }
                    }

                    //修改计划明细的使用标记
                    string strUpdatePlanDetailHQL = string.Format(@"update T_WZPickingPlanDetail set Progress = '录入',PurchaseCode='-',IsMark = 0 where ID = {0}", wZPurchaseDetail.PlanDetailID);   //ChineseWord
                    ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                    //重新加载采购清单
                    DG_PurchaseDetail.CurrentPageIndex = 0;
                    DataPurchaseDetailBinder(HF_PurchaseCode.Value);
                }
            }
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

            //if (string.IsNullOrEmpty(strMajorType))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYLBBNWKBC").ToString().Trim()+"')", true);
            //    return;
            //}
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
            //if (string.IsNullOrEmpty(strPurchaseNumber) || decimalPurchaseNumber <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGSLBNW0HZKXG").ToString().Trim()+"')", true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(strPlanMoney) || decimalPlanMoney <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYJFYBNW0HZKXG").ToString().Trim()+"')", true);
            //    return;
            //}
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
                DataPurchaseDetailBinder(HF_PurchaseCode.Value);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCGD").ToString().Trim() + "')", true);
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


    protected void BT_MajorType_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
            string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode= '" + strPurchaseCode + "'";
            IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
            if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count > 0)
            {
                for (int i = 0; i < listWZPurchaseDetail.Count; i++)
                {
                    WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[i];

                    string strObjectCode = wZPurchaseDetail.ObjectCode;
                    string strDLCode = strObjectCode.Substring(0, 2);

                    string strDivideHQL = string.Format("select * from T_WZDivide where DLCode like '%{0}%'", strDLCode);
                    DataTable dtDivide = ShareClass.GetDataSetFromSql(strDivideHQL, "Divide").Tables[0];
                    if (dtDivide != null && dtDivide.Rows.Count > 0)
                    {
                        string strDivideCode = ShareClass.ObjectToString(dtDivide.Rows[0]["DivideCode"]);
                        string strDivideType = ShareClass.ObjectToString(dtDivide.Rows[0]["DivideType"]);
                        wZPurchaseDetail.MajorType = strDivideCode;//strDivideType;

                        wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWZDMSTROBJECTCODEZBDZYLBXTJDLSTRDLCODEDZYLB").ToString().Trim() + "')", true);
                        return;
                    }
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTBZYLBCG").ToString().Trim() + "')", true);

                //重新加载采购清单
                DataPurchaseDetailBinder(HF_PurchaseCode.Value);
            }
        }
    }




    protected void BT_SortSearch_Click(object sender, EventArgs e)
    {
        string strWZPurchaseDetailHQL = string.Format(@"select d.*,p.PlanCode,o.ObjectName,o.Model,o.Criterion,o.Grade,s.UnitName from T_WZPurchaseDetail d
                                left join T_WZPickingPlanDetail p on d.PlanDetailID = p.ID
                                left join T_WZObject o on d.ObjectCode = o.ObjectCode
                                left join T_WZSpan s on o.Unit = s.ID
                                where d.PurchaseCode= '{0}'", HF_PurchaseCode.Value);

        if (!string.IsNullOrEmpty(HF_SortSearch.Value))
        {
            strWZPurchaseDetailHQL += " order by d.Tenders desc,o.DLCode desc,o.ObjectName desc,o.Model desc";

            HF_SortSearch.Value = "";
        }
        else
        {
            strWZPurchaseDetailHQL += " order by d.Tenders asc,o.DLCode asc,o.ObjectName asc,o.Model asc";

            HF_SortSearch.Value = "Tenders";
        }

        DataTable dtWZPurchaseDetail = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "PurchaseDetail").Tables[0];

        DG_PurchaseDetail.DataSource = dtWZPurchaseDetail;
        DG_PurchaseDetail.DataBind();

        LB_Sql.Text = strWZPurchaseDetailHQL;

        LB_ShowCount.Text = dtWZPurchaseDetail.Rows.Count.ToString();
    }



    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        //重新加载采购清单
        DataPurchaseDetailBinder(HF_PurchaseCode.Value);
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

            TXT_ConvertNumber.Text = decimalConvertNumber.ToString("#0.00");
            TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.00");
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

            TXT_PurchaseNumber.Text = decimalPurchaseNumber.ToString("#0.00");
            TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.00");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCGD").ToString().Trim() + "')", true);
            return;
        }
    }
}
