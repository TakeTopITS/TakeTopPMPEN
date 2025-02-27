using System; using System.Resources;
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

public partial class TTWZAdvanceDetail : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (Request.QueryString["AdvanceCode"] != null)
            {
                string strAdvanceCode = Request.QueryString["AdvanceCode"];

                HF_AdvanceCode.Value = strAdvanceCode;

                //加载预付计划明细
                DataAdvanceDetailBinder();

                //加载合同
                DataCompactBander();
            }
        }
    }


  

    private void DataAdvanceDetailBinder()
    {
        string strAdvanceCode = HF_AdvanceCode.Value;
        if (!string.IsNullOrEmpty(strAdvanceCode))
        {
            DG_AdvanceDetail.CurrentPageIndex = 0;

            WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
            string strWZAdvanceDetailHQL = string.Format(@"from WZAdvanceDetail as wZAdvanceDetail 
                        where AdvanceCode= '{0}'", strAdvanceCode);
            IList listWZAdvanceDetail = wZAdvanceDetailBLL.GetAllWZAdvanceDetails(strWZAdvanceDetailHQL);

            DG_AdvanceDetail.DataSource = listWZAdvanceDetail;
            DG_AdvanceDetail.DataBind();

            LB_AdvanceDetailSql.Text = strWZAdvanceDetailHQL;
        }
    }


    private void DataCompactBander()
    {
        string strAdvanceCode = HF_AdvanceCode.Value;
        if (!string.IsNullOrEmpty(strAdvanceCode))
        {
            string strWZCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode
                        where c.Progress in ('生效','材检')
                        and c.ProjectCode = (select ProjectCode from T_WZAdvance where AdvanceCode = '{0}')
                        and c.CompactCode not in (select CompactCode from T_WZRequest)", strAdvanceCode); 
            DataTable dtCompact = ShareClass.GetDataSetFromSql(strWZCompactHQL, "Compact").Tables[0];

            DG_Compact.DataSource = dtCompact;
            DG_Compact.DataBind();
        }
    }


    protected void DG_Compact_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "add")
        {
            string cmdArges = e.CommandArgument.ToString();

            string strAdvanceCode = HF_AdvanceCode.Value;
            if (!string.IsNullOrEmpty(strAdvanceCode))
            {
                //检验合同编号是否在当前预付明细中
                string strCheckCompactHQL = string.Format(@"select * from T_WZAdvanceDetail
                                where AdvanceCode = '{0}'
                                and ContractCode = '{1}'", strAdvanceCode, cmdArges);
                DataTable dtCheckCompact = ShareClass.GetDataSetFromSql(strCheckCompactHQL, "Compact").Tables[0];
                if (dtCheckCompact != null && dtCheckCompact.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDHTBHYJZYFMXZ").ToString().Trim()+"')", true);
                    return;
                }

                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strWZCompactHQL = string.Format(@"from WZCompact as wZCompact
                    where CompactCode = '{0}'", cmdArges);
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
                if (listCompact != null && listCompact.Count > 0)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
                    WZAdvanceDetail wZAdvanceDetail = new WZAdvanceDetail();
                    wZAdvanceDetail.AdvanceCode = strAdvanceCode;
                    wZAdvanceDetail.ContractCode = wZCompact.CompactCode;
                    wZAdvanceDetail.ContractName = wZCompact.CompactName;
                    wZAdvanceDetail.ContractMoney = wZCompact.CompactMoney;
                    DateTime dtEffectTime = DateTime.Now;
                    DateTime.TryParse(wZCompact.EffectTime, out dtEffectTime);
                    wZAdvanceDetail.EffectTime = dtEffectTime;
                    wZAdvanceDetail.SupplierCode = wZCompact.SupplierCode;
                    wZAdvanceDetail.SupplierName = GetSuppliceNameBySuppliceCode(wZCompact.SupplierCode);
                    wZAdvanceDetail.PayMoney = wZCompact.CompactMoney - wZCompact.BeforePayMoney;
                    wZAdvanceDetail.PayProgress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                    wZAdvanceDetailBLL.AddWZAdvanceDetail(wZAdvanceDetail);

                    //预付计划
                    string strWZAdvanceDetailHQL = string.Format(@"select COALESCE(SUM(PayMoney),0) as PayMoney from T_WZAdvanceDetail
                                    where AdvanceCode = '{0}'", strAdvanceCode);
                    DataTable dtAdvanceDetail = ShareClass.GetDataSetFromSql(strWZAdvanceDetailHQL, "AdvanceDetail").Tables[0];
                    decimal decimalPayMoney = 0;
                    decimal.TryParse(ShareClass.ObjectToString(dtAdvanceDetail.Rows[0]["PayMoney"]), out decimalPayMoney);
                    string strUpdateWZAdvanceHQL = string.Format(@"update T_WZAdvance 
                                    set AdvanceMoney = {0},
                                    IsMark = -1 where AdvanceCode = '{1}'", decimalPayMoney, strAdvanceCode);
                    ShareClass.RunSqlCommand(strUpdateWZAdvanceHQL);

                    //修改合同预付标志
                    wZCompact.PayIsMark = -1;
                    wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    //重新加载预付款明细列表
                    DataAdvanceDetailBinder();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYFK").ToString().Trim()+"')", true);
                return;
            }
        }
    }



    protected void DG_AdvanceDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();

                WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
                string strWZAdvanceDetailHQL = string.Format(@"from WZAdvanceDetail as wZAdvanceDetail 
                        where ID = {0}", cmdArges);
                IList listWZAdvanceDetail = wZAdvanceDetailBLL.GetAllWZAdvanceDetails(strWZAdvanceDetailHQL);
                if (listWZAdvanceDetail != null && listWZAdvanceDetail.Count > 0)
                {
                    WZAdvanceDetail wZAdvanceDetail = (WZAdvanceDetail)listWZAdvanceDetail[0];

                    wZAdvanceDetailBLL.DeleteWZAdvanceDetail(wZAdvanceDetail);

                    //预付计划
                    string strWZAdvanceDetailHQL2 = string.Format(@"select COALESCE(SUM(PayMoney),0) as PayMoney,count(1) as RowNumber from T_WZAdvanceDetail
                                    where AdvanceCode = '{0}'", wZAdvanceDetail.AdvanceCode);
                    DataTable dtAdvanceDetail = ShareClass.GetDataSetFromSql(strWZAdvanceDetailHQL2, "AdvanceDetail").Tables[0];
                    decimal decimalPayMoney = 0;
                    decimal.TryParse(ShareClass.ObjectToString(dtAdvanceDetail.Rows[0]["PayMoney"]), out decimalPayMoney);
                    int intRowNumber = 0;
                    int.TryParse(ShareClass.ObjectToString(dtAdvanceDetail.Rows[0]["RowNumber"]), out intRowNumber);
                    string strUpdateWZAdvanceHQL = string.Empty;
                    if (intRowNumber == 0)
                    {
                        strUpdateWZAdvanceHQL = string.Format(@"update T_WZAdvance 
                                    set AdvanceMoney = {0},
                                    IsMark = 0 where AdvanceCode = '{1}'", decimalPayMoney, wZAdvanceDetail.AdvanceCode);
                    }
                    else
                    {
                        strUpdateWZAdvanceHQL = string.Format(@"update T_WZAdvance 
                                    set AdvanceMoney = {0}
                                    where AdvanceCode = '{1}'", decimalPayMoney, wZAdvanceDetail.AdvanceCode);
                    }
                    ShareClass.RunSqlCommand(strUpdateWZAdvanceHQL);

                    //修改合同预付标志
                    WZCompactBLL wZCompactBLL = new WZCompactBLL();
                    string strWZCompactHQL = string.Format(@"from WZCompact as wZCompact
                    where CompactCode = '{0}'", wZAdvanceDetail.ContractCode);
                    IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
                    if (listCompact != null && listCompact.Count > 0)
                    {
                        WZCompact wZCompact = (WZCompact)listCompact[0];

                        wZCompact.PayIsMark = 0;
                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                    }

                    //重新加载预付款明细列表
                    DataAdvanceDetailBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }
            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_AdvanceDetail.Items.Count; i++)
                {
                    DG_AdvanceDetail.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();                             //ID

                WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
                string strWZAdvanceDetailHQL = string.Format(@"from WZAdvanceDetail as wZAdvanceDetail 
                        where ID = {0}", cmdArges);
                IList listWZAdvanceDetail = wZAdvanceDetailBLL.GetAllWZAdvanceDetails(strWZAdvanceDetailHQL);
                if (listWZAdvanceDetail != null && listWZAdvanceDetail.Count > 0)
                {
                    WZAdvanceDetail wZAdvanceDetail = (WZAdvanceDetail)listWZAdvanceDetail[0];

                    HF_AdvanceDetailID.Value = wZAdvanceDetail.ID.ToString();
                    TXT_PayMoney.Text = wZAdvanceDetail.PayMoney.ToString();
                    DDL_UseWay.SelectedValue = wZAdvanceDetail.UseWay;
                    #region 对状态控件
                    //if (wZAdvanceDetail.PayProgress != LanguageHandle.GetWord("LuRu").ToString().Trim())
                    //{
                    //    TXT_PayMoney.ReadOnly = true;
                    //    DDL_UseWay.Enabled = false;

                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJBW0SBYXXG").ToString().Trim()+"')", true);
                    //    return;
                    //}
                    //else
                    //{
                    //    TXT_PayMoney.ReadOnly = false;
                    //    DDL_UseWay.Enabled = true;

                    //    HF_AdvanceDetailID.Value = wZAdvanceDetail.ID.ToString();
                    //    TXT_PayMoney.Text = wZAdvanceDetail.PayMoney.ToString();
                    //    DDL_UseWay.SelectedValue = wZAdvanceDetail.UseWay;
                    //}
                    #endregion
                }
            }
        }
    }


    protected void BT_MoreAdd_Click(object sender, EventArgs e)
    {
        string strAdvanceCode = HF_AdvanceCode.Value;
        if (!string.IsNullOrEmpty(strAdvanceCode))
        {
            string strCompactCodes = Request.Form["cb_Compact_Code"];
            if (!string.IsNullOrEmpty(strCompactCodes))
            {
                string[] arrCompactCode = strCompactCodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < arrCompactCode.Length; j++)
                {
                    string strCompactCode = arrCompactCode[j];
                    if (!string.IsNullOrEmpty(strCompactCode))
                    {
                        //检验合同编号是否在当前预付明细中
                        string strCheckCompactHQL = string.Format(@"select * from T_WZAdvanceDetail
                                where AdvanceCode = '{0}'
                                and ContractCode = '{1}'", strAdvanceCode, strCompactCode);
                        DataTable dtCheckCompact = ShareClass.GetDataSetFromSql(strCheckCompactHQL, "Compact").Tables[0];
                        if (dtCheckCompact != null && dtCheckCompact.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDHTBHSTRCOMPACTCODEYJZYFMXZ").ToString().Trim()+"')", true);
                            return;
                        }
                    }
                }

                for (int i = 0; i < arrCompactCode.Length; i++)
                {
                    string strCompactCode = arrCompactCode[i];
                    if (!string.IsNullOrEmpty(strCompactCode))
                    {
                        WZCompactBLL wZCompactBLL = new WZCompactBLL();
                        string strWZCompactHQL = string.Format(@"from WZCompact as wZCompact
                                    where CompactCode = '{0}'", strCompactCode);
                        IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
                        if (listCompact != null && listCompact.Count > 0)
                        {
                            WZCompact wZCompact = (WZCompact)listCompact[0];
                            WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
                            WZAdvanceDetail wZAdvanceDetail = new WZAdvanceDetail();
                            wZAdvanceDetail.AdvanceCode = strAdvanceCode;
                            wZAdvanceDetail.ContractCode = wZCompact.CompactCode;
                            wZAdvanceDetail.ContractName = wZCompact.CompactName;
                            wZAdvanceDetail.ContractMoney = wZCompact.CompactMoney;
                            DateTime dtEffectTime = DateTime.Now;
                            DateTime.TryParse(wZCompact.EffectTime, out dtEffectTime);
                            wZAdvanceDetail.EffectTime = dtEffectTime;
                            wZAdvanceDetail.SupplierCode = wZCompact.SupplierCode;
                            wZAdvanceDetail.SupplierName = GetSuppliceNameBySuppliceCode(wZCompact.SupplierCode);
                            wZAdvanceDetail.PayMoney = wZCompact.CompactMoney - wZCompact.BeforePayMoney;
                            wZAdvanceDetail.PayProgress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                            wZAdvanceDetailBLL.AddWZAdvanceDetail(wZAdvanceDetail);

                            //预付计划
                            string strWZAdvanceDetailHQL = string.Format(@"select COALESCE(SUM(PayMoney),0) as PayMoney from T_WZAdvanceDetail
                                    where AdvanceCode = '{0}'", strAdvanceCode);
                            DataTable dtAdvanceDetail = ShareClass.GetDataSetFromSql(strWZAdvanceDetailHQL, "AdvanceDetail").Tables[0];
                            decimal decimalPayMoney = 0;
                            decimal.TryParse(ShareClass.ObjectToString(dtAdvanceDetail.Rows[0]["PayMoney"]), out decimalPayMoney);
                            string strUpdateWZAdvanceHQL = string.Format(@"update T_WZAdvance 
                                    set AdvanceMoney = {0},
                                    IsMark = -1 where AdvanceCode = '{1}'", decimalPayMoney, strAdvanceCode);
                            ShareClass.RunSqlCommand(strUpdateWZAdvanceHQL);

                            //修改合同预付标志
                            wZCompact.PayIsMark = -1;
                            wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
                        }
                    }
                }

                //重新加载预付款明细列表
                DataAdvanceDetailBinder();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZHT").ToString().Trim()+"')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYFK").ToString().Trim()+"')", true);
            return;
        }
    }
    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strAdvanceDetailID = HF_AdvanceDetailID.Value;
        if (!string.IsNullOrEmpty(strAdvanceDetailID))
        {
            string strPayMoney = TXT_PayMoney.Text;
            string strUseWay = DDL_UseWay.SelectedValue;

            if (string.IsNullOrEmpty(strPayMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYFKBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strUseWay))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYTBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            decimal decimalPayMoney = 0;
            decimal.TryParse(strPayMoney, out decimalPayMoney);

            int intAdvanceDetailID = 0;
            int.TryParse(strAdvanceDetailID, out intAdvanceDetailID);
            WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
            string strWZAdvanceDetailHQL = string.Format(@"from WZAdvanceDetail as wZAdvanceDetail 
                        where ID = {0}", intAdvanceDetailID);
            IList listWZAdvanceDetail = wZAdvanceDetailBLL.GetAllWZAdvanceDetails(strWZAdvanceDetailHQL);
            if (listWZAdvanceDetail != null && listWZAdvanceDetail.Count > 0)
            {
                WZAdvanceDetail wZAdvanceDetail = (WZAdvanceDetail)listWZAdvanceDetail[0];

                wZAdvanceDetail.PayMoney = decimalPayMoney;
                wZAdvanceDetail.UseWay = strUseWay;

                wZAdvanceDetailBLL.UpdateWZAdvanceDetail(wZAdvanceDetail, wZAdvanceDetail.ID);

                //重新加载预付款明细列表
                DataAdvanceDetailBinder();

                //修改 预付计划<预付总额>
                string strWZAdvanceDetailHQL2 = string.Format(@"select COALESCE(SUM(PayMoney),0) as PayMoney,count(1) as RowNumber from T_WZAdvanceDetail
                                    where AdvanceCode = '{0}'", wZAdvanceDetail.AdvanceCode);
                DataTable dtAdvanceDetail = ShareClass.GetDataSetFromSql(strWZAdvanceDetailHQL2, "AdvanceDetail").Tables[0];
                decimal decimalTotalPayMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtAdvanceDetail.Rows[0]["PayMoney"]), out decimalTotalPayMoney);
                string strUpdateWZAdvanceHQL = string.Format(@"update T_WZAdvance 
                                    set AdvanceMoney = {0}
                                    where AdvanceCode = '{1}'", decimalTotalPayMoney, wZAdvanceDetail.AdvanceCode);
                ShareClass.RunSqlCommand(strUpdateWZAdvanceHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYXGDHTMX").ToString().Trim()+"')", true);
            return;
        }
    }

    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_AdvanceDetail.Items.Count; i++)
        {
            DG_AdvanceDetail.Items[i].ForeColor = Color.Black;
        }

        HF_AdvanceDetailID.Value = "";
        TXT_PayMoney.Text = "";
        DDL_UseWay.SelectedValue = "";
    }


    /// <summary>
    /// 根据供方编码获得供方名称
    /// </summary>
    private string GetSuppliceNameBySuppliceCode(string strSuppliceCode)
    {
        string strSuppliceName = string.Empty;
        try
        {
            WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
            string strSupplierHQL = string.Format("from WZSupplier as wZSupplier where SupplierCode = '{0}'", strSuppliceCode);
            IList listSupplier = wZSupplierBLL.GetAllWZSuppliers(strSupplierHQL);
            if (listSupplier != null && listSupplier.Count > 0)
            {
                WZSupplier wZSupplier = (WZSupplier)listSupplier[0];

                strSuppliceName = wZSupplier.SupplierName;
            }
        }
        catch (Exception ex) { }
        return strSuppliceName;
    }


    protected void DG_AdvanceDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_AdvanceDetail.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_AdvanceDetailSql.Text.Trim();
        WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
        IList listWZAdvanceDetail = wZAdvanceDetailBLL.GetAllWZAdvanceDetails(strHQL);

        DG_AdvanceDetail.DataSource = listWZAdvanceDetail;
        DG_AdvanceDetail.DataBind();
    }
}
