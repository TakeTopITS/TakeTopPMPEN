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

public partial class TTWZPayDetail : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (Request.QueryString["PayID"] != null)
            {
                string strPayID = Request.QueryString["PayID"];

                HF_PayID.Value = strPayID;


                //加载付款计划明细
                DataPayDetailBinder();

                //加载请款单
                DataRequestBander();
            }
        }
    }



    private void DataPayDetailBinder()
    {
        string strPayID = HF_PayID.Value;
        if (!string.IsNullOrEmpty(strPayID))
        {
            DG_PayDetail.CurrentPageIndex = 0;

            string strWZPayDetailHQL = string.Format(@"select p.*,m.UserName as BorrowerName from T_WZPayDetail p
                        left join T_ProjectMember m on p.Borrower = m.UserCode 
                        where p.PayID= '{0}'", strPayID);
            DataTable dtWZPayDetail = ShareClass.GetDataSetFromSql(strWZPayDetailHQL, "PayDetail").Tables[0];

            DG_PayDetail.DataSource = dtWZPayDetail;
            DG_PayDetail.DataBind();

            LB_PayDetailSql.Text = strWZPayDetailHQL;
        }
    }


    private void DataRequestBander()
    {
        string strPayID = HF_PayID.Value;
        if (!string.IsNullOrEmpty(strPayID))
        {
            string strWZRequestHQL = string.Format(@"select r.*,a.UserName as ApproverName,m.UserName as BorrowerName,s.SupplierName from T_WZRequest r
                        left join T_ProjectMember m on r.Borrower = m.UserCode
                        left join T_ProjectMember a on r.Approver = a.UserCode
                        left join T_WZSupplier s on r.SupplierCode = s.SupplierCode
                        where r.Progress in ('报销')
                        and r.IsFinisth = 0
                        and r.ProjectCode in (select ProjectCode from T_WZPay where PayID = '{0}')", strPayID); 
            DataTable dtRequest = ShareClass.GetDataSetFromSql(strWZRequestHQL, "Request").Tables[0];

            DG_Request.DataSource = dtRequest;
            DG_Request.DataBind();
        }
    }


    protected void DG_Request_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "add")
            {
                string cmdArges = e.CommandArgument.ToString();

                string strPayID = HF_PayID.Value;
                if (!string.IsNullOrEmpty(strPayID))
                {
                    WZRequestBLL wZRequestBLL = new WZRequestBLL();
                    string strWZRequestHQL = string.Format(@"from WZRequest as wZRequest
                    where RequestCode = '{0}'", cmdArges);
                    IList listRequest = wZRequestBLL.GetAllWZRequests(strWZRequestHQL);
                    if (listRequest != null && listRequest.Count > 0)
                    {
                        WZRequest wZRequest = (WZRequest)listRequest[0];
                        WZPayDetailBLL wZPayDetailBLL = new WZPayDetailBLL();
                        WZPayDetail wZPayDetail = new WZPayDetail();
                        wZPayDetail.PayID = strPayID;

                        //合同编号
                        wZPayDetail.CompactCode = wZRequest.CompactCode;

                        wZPayDetail.RequestCode = wZRequest.RequestCode;
                        DateTime dtCancelTime = DateTime.Now;
                        DateTime.TryParse(wZRequest.CancelTime, out dtCancelTime);
                        wZPayDetail.CancelTime = dtCancelTime;
                        wZPayDetail.SupplierCode = wZRequest.SupplierCode;
                        wZPayDetail.Supplier = GetSuppliceNameBySuppliceCode(wZRequest.SupplierCode);
                        wZPayDetail.PlanMoney = wZRequest.Arrearage;
                        wZPayDetail.Borrower = wZRequest.Borrower;
                        wZPayDetail.UseWay = wZRequest.UseWay;
                        wZPayDetail.PayProcess = LanguageHandle.GetWord("LuRu").ToString().Trim();

                        wZPayDetailBLL.AddWZPayDetail(wZPayDetail);

                        //付款计划
                        string strWZPayDetailHQL = string.Format(@"select COALESCE(SUM(PlanMoney),0) as PlanMoney,count(1) as RowNumber from T_WZPayDetail
                                    where PayID = '{0}'", strPayID);
                        DataTable dtPayDetail = ShareClass.GetDataSetFromSql(strWZPayDetailHQL, "PayDetail").Tables[0];
                        decimal decimalPlanMoney = 0;
                        decimal.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["PlanMoney"]), out decimalPlanMoney);
                        int intRowNumber = 0;
                        int.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["RowNumber"]), out intRowNumber);
                        string strUpdateWZPayHQL = string.Format(@"update T_WZPay 
                                    set PayTotal = {0},
                                    RowNumber = {1},
                                    IsMark = -1 where PayID = '{2}'", decimalPlanMoney, intRowNumber, strPayID);
                        ShareClass.RunSqlCommand(strUpdateWZPayHQL);

                        //修改请款单使用标志
                        wZRequest.IsPay = -1;
                        wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                        //重新加载付款明细列表
                        DataPayDetailBinder();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZFKJH").ToString().Trim()+"')", true);
                    return;
                }
            }
        }
    }



    protected void DG_PayDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();

            WZPayDetailBLL wZPayDetailBLL = new WZPayDetailBLL();
            string strWZPayDetailHQL = string.Format(@"from WZPayDetail as wZPayDetail 
                        where ID = {0}", cmdArges);
            IList listWZPayDetail = wZPayDetailBLL.GetAllWZPayDetails(strWZPayDetailHQL);
            if (listWZPayDetail != null && listWZPayDetail.Count > 0)
            {
                WZPayDetail wZPayDetail = (WZPayDetail)listWZPayDetail[0];

                wZPayDetailBLL.DeleteWZPayDetail(wZPayDetail);

                //预付计划
                string strWZPayDetailHQL2 = string.Format(@"select COALESCE(SUM(PlanMoney),0) as PlanMoney,count(1) as RowNumber from T_WZPayDetail
                                    where PayID = '{0}'", wZPayDetail.PayID);
                DataTable dtPayDetail = ShareClass.GetDataSetFromSql(strWZPayDetailHQL2, "PayDetail").Tables[0];
                decimal decimalPlanMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["PlanMoney"]), out decimalPlanMoney);
                int intRowNumber = 0;
                int.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["RowNumber"]), out intRowNumber);
                string strUpdateWZPayHQL = string.Empty;
                if (intRowNumber == 0)
                {
                    strUpdateWZPayHQL = string.Format(@"update T_WZPay 
                                    set PayTotal = {0},
                                    RowNumber = {1},
                                    IsMark = 0 where PayID = '{2}'", decimalPlanMoney, intRowNumber, wZPayDetail.PayID);
                }
                else
                {
                    strUpdateWZPayHQL = string.Format(@"update T_WZAdvance 
                                    set PayTotal = {0},
                                    RowNumber = {1}
                                    where PayID = '{2}'", decimalPlanMoney, intRowNumber, wZPayDetail.PayID);
                }
                ShareClass.RunSqlCommand(strUpdateWZPayHQL);

                //修改请款单付款标志
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strWZRequestHQL = string.Format(@"from WZRequest as wZRequest
                    where RequestCode = '{0}'", wZPayDetail.RequestCode);
                IList listRequest = wZRequestBLL.GetAllWZRequests(strWZRequestHQL);
                if (listRequest != null && listRequest.Count > 0)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    wZRequest.IsPay = 0;
                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                }

                //重新加载付款明细列表
                DataPayDetailBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }
        }
        else if (cmdName == "edit")
        {
            for (int i = 0; i < DG_PayDetail.Items.Count; i++)
            {
                DG_PayDetail.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdArges = e.CommandArgument.ToString();                             //ID

            WZPayDetailBLL wZPayDetailBLL = new WZPayDetailBLL();
            string strWZPayDetailHQL = string.Format(@"from WZPayDetail as wZPayDetail 
                        where ID = {0}", cmdArges);
            IList listWZPayDetail = wZPayDetailBLL.GetAllWZPayDetails(strWZPayDetailHQL);
            if (listWZPayDetail != null && listWZPayDetail.Count > 0)
            {
                WZPayDetail wZPayDetail = (WZPayDetail)listWZPayDetail[0];

                HF_PayDetailID.Value = wZPayDetail.ID.ToString();

                TXT_PlanMoney.Text = wZPayDetail.PlanMoney.ToString();
                DDL_UseWay.SelectedValue = wZPayDetail.UseWay;
            }
        }
    }

   

    protected void BT_MoreAdd_Click(object sender, EventArgs e)
    {
        string strPayID = HF_PayID.Value;
        if (!string.IsNullOrEmpty(strPayID))
        {
            string strRequestCodes = Request.Form["cb_Request_Code"];
            if (!string.IsNullOrEmpty(strRequestCodes))
            {
                string[] arrRequestCode = strRequestCodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arrRequestCode.Length; i++)
                {
                    string strRequestCode = arrRequestCode[i];
                    if (!string.IsNullOrEmpty(strRequestCode))
                    {
                        WZRequestBLL wZRequestBLL = new WZRequestBLL();
                        string strWZRequestHQL = string.Format(@"from WZRequest as wZRequest
                                    where RequestCode = '{0}'", strRequestCode);
                        IList listRequest = wZRequestBLL.GetAllWZRequests(strWZRequestHQL);
                        if (listRequest != null && listRequest.Count > 0)
                        {
                            WZRequest wZRequest = (WZRequest)listRequest[0];
                            WZPayDetailBLL wZPayDetailBLL = new WZPayDetailBLL();
                            WZPayDetail wZPayDetail = new WZPayDetail();
                            wZPayDetail.PayID = strPayID;

                            //合同编号
                            wZPayDetail.CompactCode = wZRequest.CompactCode;

                            wZPayDetail.RequestCode = wZRequest.RequestCode;
                            DateTime dtCancelTime = DateTime.Now;
                            DateTime.TryParse(wZRequest.CancelTime, out dtCancelTime);
                            wZPayDetail.CancelTime = dtCancelTime;
                            wZPayDetail.SupplierCode = wZRequest.SupplierCode;
                            wZPayDetail.Supplier = GetSuppliceNameBySuppliceCode(wZRequest.SupplierCode);
                            wZPayDetail.PlanMoney = wZRequest.Arrearage;
                            wZPayDetail.Borrower = wZRequest.Borrower;
                            wZPayDetail.UseWay = wZRequest.UseWay;
                            wZPayDetail.PayProcess = LanguageHandle.GetWord("LuRu").ToString().Trim();

                            wZPayDetailBLL.AddWZPayDetail(wZPayDetail);

                            //付款计划
                            string strWZPayDetailHQL = string.Format(@"select COALESCE(SUM(PlanMoney),0) as PlanMoney,count(1) as RowNumber from T_WZPayDetail
                                    where PayID = '{0}'", strPayID);
                            DataTable dtPayDetail = ShareClass.GetDataSetFromSql(strWZPayDetailHQL, "PayDetail").Tables[0];
                            decimal decimalPlanMoney = 0;
                            decimal.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["PlanMoney"]), out decimalPlanMoney);
                            int intRowNumber = 0;
                            int.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["RowNumber"]), out intRowNumber);
                            string strUpdateWZPayHQL = string.Format(@"update T_WZPay 
                                    set PayTotal = {0},
                                    RowNumber = {1},
                                    IsMark = -1 where PayID = '{2}'", decimalPlanMoney, intRowNumber, strPayID);
                            ShareClass.RunSqlCommand(strUpdateWZPayHQL);

                            //修改请款单使用标志
                            wZRequest.IsPay = -1;
                            wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);
                        }
                    }
                }

                //重新加载付款明细列表
                DataPayDetailBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPLBZCG").ToString().Trim()+"')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZKD").ToString().Trim()+"')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZFKJH").ToString().Trim()+"')", true);
            return;
        }
    }
    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strPayDetailID = HF_PayDetailID.Value;
        if (!string.IsNullOrEmpty(strPayDetailID))
        {
            string strPlanMoney = TXT_PlanMoney.Text.Trim();
            string strUseWay = DDL_UseWay.SelectedValue;

            if (string.IsNullOrEmpty(strPlanMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHFKBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);

            int intPayDetailID = 0;
            int.TryParse(strPayDetailID, out intPayDetailID);
            WZPayDetailBLL wZPayDetailBLL = new WZPayDetailBLL();
            string strWZPayDetailHQL = string.Format(@"from WZPayDetail as wZPayDetail 
                        where ID = {0}", intPayDetailID);
            IList listWZPayDetail = wZPayDetailBLL.GetAllWZPayDetails(strWZPayDetailHQL);
            if (listWZPayDetail != null && listWZPayDetail.Count > 0)
            {
                WZPayDetail wZPayDetail = (WZPayDetail)listWZPayDetail[0];

                wZPayDetail.PlanMoney = decimalPlanMoney;
                //wZPayDetail.UseWay = strUseWay;

                wZPayDetailBLL.UpdateWZPayDetail(wZPayDetail, wZPayDetail.ID);


                //修改 预付计划<预付总额>
                string strWZPayDetailHQL2 = string.Format(@"select COALESCE(SUM(PlanMoney),0) as PlanMoney,count(1) as RowNumber from T_WZPayDetail
                                    where PayID = '{0}'", wZPayDetail.PayID);
                DataTable dtPayDetail = ShareClass.GetDataSetFromSql(strWZPayDetailHQL2, "PayDetail").Tables[0];
                decimal decimalTotalPlanMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["PlanMoney"]), out decimalTotalPlanMoney);
                int intRowNumber = 0;
                int.TryParse(ShareClass.ObjectToString(dtPayDetail.Rows[0]["RowNumber"]), out intRowNumber);
                string strUpdateWZPayHQL = string.Format(@"update T_WZPay 
                                    set PayTotal = {0},
                                    RowNumber = {1}
                                    where PayID = '{2}'", decimalTotalPlanMoney, intRowNumber, wZPayDetail.PayID);
                ShareClass.RunSqlCommand(strUpdateWZPayHQL);

                //重新加载付款明细列表
                DataPayDetailBinder();

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
        for (int i = 0; i < DG_PayDetail.Items.Count; i++)
        {
            DG_PayDetail.Items[i].ForeColor = Color.Black;
        }

        HF_PayDetailID.Value = "";

        TXT_PlanMoney.Text = "";
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


    protected void DG_PayDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_PayDetail.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_PayDetailSql.Text.Trim();
        DataTable dtWZPayDetail = ShareClass.GetDataSetFromSql(strHQL, "PayDetail").Tables[0];

        DG_PayDetail.DataSource = dtWZPayDetail;
        DG_PayDetail.DataBind();
    }
}
