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

public partial class TTWZRequestApprove : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (!IsPostBack)
        {
            DataRequestBinder();
        }
    }

    private void DataRequestBinder()
    {
        string strRequestHQL = string.Format(@"select r.*,a.UserName as ApproverName,m.UserName as BorrowerName,s.SupplierName from T_WZRequest r
                    left join T_ProjectMember m on r.Borrower = m.UserCode
                    left join T_ProjectMember a on r.Approver = a.UserCode 
                    left join T_WZSupplier s on r.SupplierCode = s.SupplierCode
                    where r.Approver ='{0}' 
                    and r.Progress in ('请款','审核','报销') 
                    order by r.RequestTime desc", strUserCode); 
        DataTable dtRequest = ShareClass.GetDataSetFromSql(strRequestHQL, "Request").Tables[0];

        DG_Request.DataSource = dtRequest;
        DG_Request.DataBind();
    }

    protected void DG_Request_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "approve")
            {
                //审核
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != LanguageHandle.GetWord("QingKuan").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBSKZTBNSH").ToString().Trim()+"')", true);
                        return;
                    }

                    wZRequest.Progress = LanguageHandle.GetWord("ShenHe").ToString().Trim();

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    //加载请款单列表
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHCG").ToString().Trim()+"')", true);
                }
            }
            else if (cmdName == "notRequest")
            {
                //退回审核
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != LanguageHandle.GetWord("ShenHe").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBSSHZTBNTH").ToString().Trim()+"')", true);
                        return;
                    }

                    wZRequest.Progress = LanguageHandle.GetWord("QingKuan").ToString().Trim();

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    //加载请款单列表
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"')", true);
                }
            }
            else if (cmdName == "account")
            {
                //报销
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != LanguageHandle.GetWord("ShenHe").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBSSHZTBNBX").ToString().Trim()+"')", true);
                        return;
                    }

                    wZRequest.Progress = LanguageHandle.GetWord("BaoXiao").ToString().Trim();
                    wZRequest.CancelTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    AccountHandler(wZRequest.RequestCode);

                    //加载请款单列表
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBXCG").ToString().Trim()+"')", true);
                }
            }
            else if (cmdName == "notAccount")
            {
                //报销退回
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != LanguageHandle.GetWord("BaoXiao").ToString().Trim() || wZRequest.Approver.Trim() != strUserCode || wZRequest.IsPay != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBSBXZTHZSHRBZHZFKBZBW0BNTH").ToString().Trim()+"')", true);
                        return;
                    }

                    wZRequest.Progress = LanguageHandle.GetWord("ShenHe").ToString().Trim();
                    wZRequest.CancelTime = "-";
                    wZRequest.BeforePayMoney = 0;
                    wZRequest.Arrearage = 0;

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    CancelAccountHandler(wZRequest.RequestCode);

                    //加载请款单列表
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"')", true);
                }
            }
        }
    }


    /// <summary>
    /// 报销
    /// </summary>
    private void AccountHandler(string strRequestCode)
    {
        #region 注释
        //写合同：												
        //   合同〈请款金额〉＝原值＋请款单〈借款金额〉												
        //   合同〈未请款额〉＝合同〈收料总额〉－合同〈请款金额〉												
        //写收料单：												
        //   收料单〈财务审核〉＝请款单〈财务审核〉												
        //   收料单〈报销进度〉＝“报销”												
        //写请款单。打开合同表单，按下列原则写记录：												
        //    ①当合同〈预付余额〉＝“0”时：												
        //        请款单〈预付款〉＝“0”												
        //        请款单〈欠款〉＝〈借款金额〉－〈预付款〉－〈已付款〉												
        //    ②当合同〈预付余额〉＜请款单〈借款金额〉时：												
        //        请款单〈预付款〉＝合同〈预付余额〉												
        //        请款单〈欠款〉＝〈借款金额〉－〈预付款〉－〈已付款〉												
        //        然后使合同〈预付余额〉＝“0”												
        //    ③当合同〈预付余额〉＞请款单〈借款金额〉时：												
        //        请款单〈预付款〉＝请款单〈借款金额〉												
        //        请款单〈欠款〉＝“0”												
        //        合同〈预付余额〉＝原值－请款单〈预付款〉												
        #endregion
        WZRequestBLL wZRequestBLL = new WZRequestBLL();
        string strRequestHQL = string.Format("from WZRequest as wZRequest where RequestCode ='{0}'", strRequestCode);
        IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
        if (listRequest != null && listRequest.Count > 0)
        {
            WZRequest wZRequest = (WZRequest)listRequest[0];

            //写合同
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactHQL = string.Format("from WZCompact as wZCompact where CompactCode = '{0}'", wZRequest.CompactCode);
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactHQL);
            if (listCompact != null && listCompact.Count > 0)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                wZCompact.RequestMoney += wZRequest.BorrowMoney;
                wZCompact.NotRequestMoney = wZCompact.CollectMoney - wZCompact.RequestMoney;

                if (wZCompact.BeforePayBalance == 0)
                {
                    //当合同〈预付余额〉＝“0”时
                    wZRequest.BeforePayMoney = 0;
                    wZRequest.Arrearage = wZRequest.BorrowMoney - wZRequest.BeforePayMoney - wZRequest.PayMoney;
                }
                else if (wZCompact.BeforePayBalance < wZRequest.BorrowMoney)
                {
                    //当合同〈预付余额〉＜请款单〈借款金额〉时：
                    wZRequest.BeforePayMoney = wZCompact.BeforePayBalance;
                    wZRequest.Arrearage = wZRequest.BorrowMoney - wZRequest.BeforePayMoney - wZRequest.PayMoney;

                    wZCompact.BeforePayBalance = 0;
                }
                else if (wZCompact.BeforePayBalance > wZRequest.BorrowMoney)
                {
                    //当合同〈预付余额〉＞请款单〈借款金额〉时：
                    wZRequest.BeforePayMoney = wZRequest.BorrowMoney;
                    wZRequest.Arrearage = 0;

                    wZCompact.BeforePayBalance -= wZRequest.BeforePayMoney;
                }

                //更新合同
                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
                //更新请款单
                wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);
            }
            //写收料单
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = string.Format("from WZCollect as wZCollect where RequestCode ='{0}'", wZRequest.RequestCode);
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count > 0)
            {
                for (int i = 0; i < listCollect.Count; i++)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[i];
                    wZCollect.FinanceApprove = wZRequest.Approver;
                    wZCollect.PayProcess = LanguageHandle.GetWord("BaoXiao").ToString().Trim();

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);
                }
            }
        }
    }

    /// <summary>
    /// 取消报销
    /// </summary>
    private void CancelAccountHandler(string strRequestCode)
    {
        #region 注释
        //写合同：												
        //   合同〈请款金额〉＝＝原值－请款单〈借款金额〉												
        //   合同〈未请款额〉＝合同〈收料总额〉－合同〈请款金额〉												
        //   合同〈预付余额〉												
        //      请款单〈预付款〉＝“0”时，合同〈预付余额〉＝“0”												
        //      请款单〈预付款〉＞“0”时，合同〈预付余额〉＝原值＋请款单〈预付款〉												
        //写收料单：												
        //   收料单〈财务审核〉＝“-”												
        //   收料单〈报销进度〉＝“录入”												
        //写请款单：												
        //   请款单〈进度〉＝“审核”												
        //   请款单〈报销日期〉＝“-”												
        //   请款单〈预付款〉＝“0”												
        //   请款单〈欠款〉＝“0”		
        #endregion
        WZRequestBLL wZRequestBLL = new WZRequestBLL();
        string strRequestHQL = string.Format("from WZRequest as wZRequest where RequestCode ='{0}'", strRequestCode);
        IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
        if (listRequest != null && listRequest.Count > 0)
        {
            WZRequest wZRequest = (WZRequest)listRequest[0];

            //写合同
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strCompactHQL = string.Format("from WZCompact as wZCompact where CompactCode = '{0}'", wZRequest.CompactCode);
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactHQL);
            if (listCompact != null && listCompact.Count > 0)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];
                wZCompact.RequestMoney -= wZRequest.BorrowMoney;
                wZCompact.NotRequestMoney = wZCompact.CollectMoney - wZCompact.RequestMoney;

                if (wZRequest.BeforePayMoney == 0)
                {
                    //请款单〈预付款〉＝“0”时
                    wZCompact.BeforePayBalance = 0;
                }
                else if (wZRequest.BeforePayMoney > 0)
                {
                    //请款单〈预付款〉＞“0”时
                    wZCompact.BeforePayBalance += wZRequest.BeforePayMoney;
                }

                //更新合同
                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
            }
            //写收料单
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = string.Format("from WZCollect as wZCollect where RequestCode ='{0}'", wZRequest.RequestCode);
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count > 0)
            {
                for (int i = 0; i < listCollect.Count; i++)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[i];
                    wZCollect.FinanceApprove = "-";
                    wZCollect.PayProcess = LanguageHandle.GetWord("LuRu").ToString().Trim();

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);
                }
            }
        }
    }

}
