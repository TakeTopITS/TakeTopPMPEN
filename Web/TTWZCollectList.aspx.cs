using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZCollectList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "开收料单", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataCollectBinder();
            DataCompactBander();

            DG_CompactDetail.DataSource = "";
            DG_CompactDetail.DataBind();

            DDL_CollectCode.Items.Insert(0, new ListItem("--Select--", ""));
            DDL_CompactCode.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }

    private void DataCollectBinder()
    {
        DG_Collect.CurrentPageIndex = 0;

        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Contacter ='{0}' 
                            and c.Progress in ('录入','材检','开票') 
                           ", strUserCode);   //ChineseWord

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strCollectCode = DDL_CollectCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            strCollectHQL += " and c.CollectCode like '%" + strCollectCode + "%'";
        }
        string strCompactCode = DDL_CompactCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCollectHQL += " and c.CompactCode like '%" + strCompactCode + "%'";
        }
        strCollectHQL += "  order by c.TicketTime desc";
        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        DDL_CollectCode.DataSource = dtCollect;
        DDL_CollectCode.DataBind();
        DDL_CollectCode.Items.Insert(0, new ListItem("--Select--", "0"));

        DDL_CompactCode.DataSource = dtCollect;
        DDL_CompactCode.DataBind();
        DDL_CompactCode.Items.Insert(0, new ListItem("--Select--", "0"));

        LB_CheckRecord.Text = dtCollect.Rows.Count.ToString();
        LB_CollectSql.Text = strCollectHQL;
    }

    private void DataCompactBander()
    {
        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strWZCompactHQL = string.Format("from WZCompact as wZCompact where Compacter = '{0}' and Progress = '生效' order by MarkTime desc", strUserCode);   //ChineseWord
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);

        LB_Compact.DataSource = listCompact;
        LB_Compact.DataBind();
    }

    private void DataCompactDetailBinder()
    {
        string strCompactCode = LB_Compact.SelectedValue;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            string strWZCompactDetailHQL = string.Format(@"select d.*,o.ObjectName,o.Model,O.Criterion,o.Grade,k.UnitName,p.PurchaseCode,l.PlanCode from T_WZCompactDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                            left join T_WZPurchaseDetail p on d.PurchaseDetailID = p.ID
                            left join T_WZPickingPlanDetail l on d.PlanDetailID = l.ID
                            left join T_WZSpan k on o.Unit = k.ID
                            where d.CompactCode = '{0}'", strCompactCode);
            DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strWZCompactDetailHQL, "CompactDetail").Tables[0];

            DG_CompactDetail.DataSource = dtCompactDetail;
            DG_CompactDetail.DataBind();

            LB_CompactDetailRecordNumber.Text = dtCompactDetail.Rows.Count.ToString();

            LB_CompactDetailSql.Text = strWZCompactDetailHQL;
        }
    }


    protected void DG_Collect_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZCollectBLL wZCollectBLL = new WZCollectBLL();
                string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
                IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
                if (listCollect != null && listCollect.Count == 1)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[0];
                    if (wZCollect.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZCollect.Contacter != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRYJHTYBWDDLYHSBYXSC").ToString().Trim() + "')", true);
                        return;
                    }

                    wZCollectBLL.DeleteWZCollect(wZCollect);

                    //重新加载列表
                    DG_Collect.CurrentPageIndex = 0;
                    DataCollectBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }

            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Collect.Items.Count; i++)
                {
                    DG_Collect.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZCollectBLL wZCollectBLL = new WZCollectBLL();
                string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
                IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
                if (listCollect != null && listCollect.Count == 1)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[0];

                    HF_CollectCode.Value = wZCollect.CollectCode;
                    LB_CollectCode.Text = wZCollect.CollectCode;
                    DDL_CollectMethod.SelectedValue = wZCollect.CollectMethod;
                    LB_CollectNumber.Text = wZCollect.CollectNumber.ToString();

                    TXT_ActualNumber.Text = wZCollect.ActualNumber.ToString();
                    TXT_Ratio.Text = wZCollect.Ratio.ToString();
                    TXT_Freight.Text = wZCollect.Freight.ToString();
                    TXT_OtherObject.Text = wZCollect.OtherObject.ToString();
                    TXT_TicketNumber.Text = wZCollect.TicketNumber;

                    TXT_ActualPrice.Text = wZCollect.ActualPrice.ToString();
                    TXT_ActualMoney.Text = wZCollect.ActualMoney.ToString();
                    TXT_RatioMoney.Text = wZCollect.RatioMoney.ToString();

                    TXT_CompactPrice.Text = "";

                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID.ToString();
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];
                        TXT_CompactPrice.Text = wZCompactDetail.CompactPrice.ToString();
                        TXT_CollectMoney.Text = (wZCompactDetail.CompactPrice * wZCollect.ActualNumber).ToString();
                    }

                    if (wZCollect.CollectMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
                    {
                        TXT_ActualNumber.BackColor = Color.Red;
                    }
                    else
                    {
                        TXT_ActualNumber.BackColor = Color.White;
                    }
                }
            }
            else if (cmdName == "submit")
            {
                //提交
                string cmdArges = e.CommandArgument.ToString();
                WZCollectBLL wZCollectBLL = new WZCollectBLL();
                string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
                IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
                if (listCollect != null && listCollect.Count == 1)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[0];
                    if (wZCollect.Progress == LanguageHandle.GetWord("LuRu").ToString().Trim())
                    {
                        string strCheckCode = wZCollect.CheckCode;

                        wZCollect.Progress = LanguageHandle.GetWord("CaiJian").ToString().Trim();

                        //if (!string.IsNullOrEmpty(strCheckCode) && strCheckCode != "-")
                        //{
                        //    //材检
                        //    wZCollect.Progress = LanguageHandle.GetWord("CaiJian").ToString().Trim();
                        //}
                        //else
                        //{
                        //    //开票
                        //    wZCollect.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                        //}

                        wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                        //重新加载收料单列表
                        DataCollectBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRBNTJ").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "notSubmit1")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZCollectBLL wZCollectBLL = new WZCollectBLL();
                string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
                IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
                if (listCollect != null && listCollect.Count == 1)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[0];
                    if (wZCollect.Progress == LanguageHandle.GetWord("KaiPiao").ToString().Trim() || wZCollect.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
                    {
                        wZCollect.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                        wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                        //重新加载收料单列表
                        DataCollectBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPTHCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWKPHZCJZTBNTH").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "notSubmit2")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZCollectBLL wZCollectBLL = new WZCollectBLL();
                string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
                IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
                if (listCollect != null && listCollect.Count == 1)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[0];
                    if (wZCollect.Progress == LanguageHandle.GetWord("KaiPiao").ToString().Trim() || wZCollect.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
                    {
                        wZCollect.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                        wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                        //重新加载收料单列表
                        DataCollectBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCJTHCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWKPHZCJZTBNTH").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Collect.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_CollectSql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZCollect");
        DG_Collect.DataSource = ds;
        DG_Collect.DataBind();
    }

    protected void LB_Compact_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_Compact.SelectedValue))
        {
            DataCompactDetailBinder();
        }
    }

    protected void DG_CompactDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "add")
        {
            string cmdArges = e.CommandArgument.ToString();

            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + cmdArges;
            IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
            if (listCompactDetail != null && listCompactDetail.Count > 0)
            {
                WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strWZCompactHQL = string.Format("from WZCompact as wZCompact where CompactCode = '{0}'", wZCompactDetail.CompactCode);
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
                if (listCompact != null && listCompact.Count > 0)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];

                    //增加收料单
                    //收料编号
                    string strNewCollectCode = CreateNewCollectCode();

                    WZCollect wZCollect = new WZCollect();
                    wZCollect.CollectCode = strNewCollectCode;
                    wZCollect.TicketTime = DateTime.Now;

                    wZCollect.CollectMethod = LanguageHandle.GetWord("LanPiao").ToString().Trim();

                    wZCollect.Contacter = strUserCode;
                    wZCollect.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                    wZCollect.CompactDetailID = wZCompactDetail.ID;
                    wZCollect.PlanDetailID = wZCompactDetail.PlanDetailID;
                    wZCollect.ObjectCode = wZCompactDetail.ObjectCode;
                    wZCollect.CheckCode = wZCompactDetail.CheckCode;
                    wZCollect.ProjectCode = wZCompact.ProjectCode;
                    wZCollect.SupplierCode = wZCompact.SupplierCode;
                    wZCollect.StoreRoom = wZCompact.StoreRoom;
                    wZCollect.Checker = wZCompact.Checker;
                    wZCollect.Safekeeper = wZCompact.Safekeep;
                    //wZCollect.CheckTime = DateTime.Now;
                    //wZCollect.CollectTime = DateTime.Now;
                    wZCollect.RequestCode = "-";
                    wZCollect.FinanceApprove = "-";
                    wZCollect.PayProcess = "-";

                    wZCollect.CompactCode = wZCompact.CompactCode;

                    //在途收料单〈实收数量〉
                    decimal decimalCurrentNumber = 0;
                    string strCollectHQL = string.Format(@"select CompactDetailID,SUM(ActualNumber) as SumActualNumber from T_WZCollect
                                   where CompactDetailID={0} and IsMark = 0 group by CompactDetailID", wZCompactDetail.ID);
                    DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];
                    if (dtCollect != null && dtCollect.Rows.Count > 0)
                    {
                        decimal.TryParse(ShareClass.ObjectToString(dtCollect.Rows[0]["SumActualNumber"]), out decimalCurrentNumber);
                    }
                    wZCollect.CollectNumber = wZCompactDetail.CompactNumber - wZCompactDetail.CollectNumber - decimalCurrentNumber;
                    wZCollect.ActualNumber = wZCollect.CollectNumber;

                    decimal decimalVar1 = wZCompactDetail.CompactPrice;
                    decimal decimalVar2 = decimalVar1 * wZCollect.ActualNumber;
                    wZCollect.Ratio = decimal.Parse("0.17");
                    if (wZCollect.Ratio != 0)
                    {
                        wZCollect.ActualPrice = decimalVar1 / (1 + wZCollect.Ratio);
                    }
                    else
                    {
                        wZCollect.ActualPrice = 0;
                    }
                    wZCollect.ActualMoney = wZCollect.ActualPrice * wZCollect.ActualNumber;
                    wZCollect.RatioMoney = wZCompactDetail.CompactPrice * wZCollect.ActualNumber - wZCollect.ActualMoney;

                    //收料单<换算数量>
                    decimal decimalConvertRatio = GetConvertRatioByObjectCode(wZCompactDetail.ObjectCode);  //物资代码<换算系数>
                    if (decimalConvertRatio != 0)
                    {
                        wZCollect.ConvertNumber = wZCollect.ActualNumber / decimalConvertRatio;
                    }
                    else
                    {
                        wZCollect.ConvertNumber = 0;
                    }
                    WZCollectBLL wZCollectBLL = new WZCollectBLL();
                    wZCollectBLL.AddWZCollect(wZCollect);

                    //修改合同明细<使用标记> = -1
                    wZCompactDetail.IsMark = -1;
                    wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                    LB_IsMark.Text = wZCompactDetail.IsMark.ToString();

                    //重新加载收料单列表，合同明细也重新加载
                    DataCollectBinder();
                    DataCompactDetailBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void DG_CompactDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_CompactDetail.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_CompactDetailSql.Text.Trim();
        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strHQL, "CompactDetail").Tables[0];

        DG_CompactDetail.DataSource = dtCompactDetail;
        DG_CompactDetail.DataBind();
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strCollectCode = HF_CollectCode.Value;
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            string strCollectMethod = DDL_CollectMethod.SelectedValue;
            string strActualNumber = TXT_ActualNumber.Text.Trim();
            string strRatio = TXT_Ratio.Text.Trim();
            string strFreight = TXT_Freight.Text.Trim();
            string strOtherObject = TXT_OtherObject.Text.Trim();
            string strTicketNumber = TXT_TicketNumber.Text.Trim();

            string strActualPrice = TXT_ActualPrice.Text.Trim();
            string strActualMoney = TXT_ActualMoney.Text.Trim();
            string strRatioMoney = TXT_RatioMoney.Text.Trim();

            if (string.IsNullOrEmpty(strCollectMethod))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLFSBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSSLBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strRatio))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBNWKBC").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal decimalRatio = 0;
            decimal.TryParse(strRatio, out decimalRatio);
            decimal decimalFreight = 0;
            decimal.TryParse(strFreight, out decimalFreight);
            decimal decimalOtherObject = 0;
            decimal.TryParse(strOtherObject, out decimalOtherObject);

            decimal decimalActualPrice = 0;
            decimal.TryParse(strActualPrice, out decimalActualPrice);
            decimal decimalActualMoney = 0;
            decimal.TryParse(strActualMoney, out decimalActualMoney);
            decimal decimalRatioMoney = 0;
            decimal.TryParse(strRatioMoney, out decimalRatioMoney);

            //if (strCollectMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
            //{
            //    decimalActualNumber = decimalActualNumber > 0 ? (-1 * decimalActualNumber) : decimalActualNumber;
            //}

            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + strCollectCode + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];

                wZCollect.CollectMethod = strCollectMethod;
                wZCollect.ActualNumber = decimalActualNumber;
                //计算<实购金额>，<税金>，<换算数量>，<实购单价>

                WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                if (listCompactDetail != null && listCompactDetail.Count > 0)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                    //decimal decimalVar1 = wZCompactDetail.CompactPrice;
                    //decimal decimalVar2 = decimalVar1 * wZCollect.ActualNumber;
                    //wZCollect.Ratio = decimal.Parse("0.17");
                    //if (wZCollect.Ratio != 0)
                    //{
                    //    wZCollect.ActualPrice = decimalVar1 / wZCollect.Ratio;
                    //}
                    //else
                    //{
                    //    wZCollect.ActualPrice = 0;
                    //}
                    //wZCollect.ActualMoney = wZCollect.ActualPrice * wZCollect.ActualNumber;
                    //wZCollect.RatioMoney = decimalVar2 - wZCollect.ActualMoney;


                    wZCollect.ActualPrice = decimalActualPrice;
                    wZCollect.ActualMoney = decimalActualMoney;
                    wZCollect.RatioMoney = decimalRatioMoney;

                    wZCollect.Ratio = decimalRatio;
                    //收料单<换算数量>
                    decimal decimalConvertRatio = GetConvertRatioByObjectCode(wZCompactDetail.ObjectCode);  //物资代码<换算系数>
                    if (decimalConvertRatio != 0)
                    {
                        wZCollect.ConvertNumber = wZCollect.ActualNumber / decimalConvertRatio;
                    }
                    else
                    {
                        wZCollect.ConvertNumber = 0;
                    }

                    wZCollect.Freight = decimalFreight;
                    wZCollect.OtherObject = decimalOtherObject;
                    wZCollect.TicketNumber = strTicketNumber;

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //重新加载收料单列表
                    DataCollectBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYXGDSLD").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        string strCollectCode = HF_CollectCode.Value;
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            string strCollectMethod = DDL_CollectMethod.SelectedValue;
            string strActualNumber = TXT_ActualNumber.Text.Trim();
            string strRatio = TXT_Ratio.Text.Trim();
            string strFreight = TXT_Freight.Text.Trim();
            string strOtherObject = TXT_OtherObject.Text.Trim();
            string strTicketNumber = TXT_TicketNumber.Text.Trim();

            string strActualPrice = TXT_ActualPrice.Text.Trim();
            string strActualMoney = TXT_ActualMoney.Text.Trim();
            string strRatioMoney = TXT_RatioMoney.Text.Trim();

            if (string.IsNullOrEmpty(strCollectMethod))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLFSBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSSLBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strRatio))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBNWKBC").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal decimalRatio = 0;
            decimal.TryParse(strRatio, out decimalRatio);
            decimal decimalFreight = 0;
            decimal.TryParse(strFreight, out decimalFreight);
            decimal decimalOtherObject = 0;
            decimal.TryParse(strOtherObject, out decimalOtherObject);

            decimal decimalActualPrice = 0;
            decimal.TryParse(strActualPrice, out decimalActualPrice);
            decimal decimalActualMoney = 0;
            decimal.TryParse(strActualMoney, out decimalActualMoney);
            decimal decimalRatioMoney = 0;
            decimal.TryParse(strRatioMoney, out decimalRatioMoney);

            //if (strCollectMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
            //{
            //    decimalActualNumber = decimalActualNumber > 0 ? (-1 * decimalActualNumber) : decimalActualNumber;
            //}

            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + strCollectCode + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];

                wZCollect.CollectMethod = strCollectMethod;
                wZCollect.ActualNumber = decimalActualNumber;
                //计算<实购金额>，<税金>，<换算数量>，<实购单价>

                WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                if (listCompactDetail != null && listCompactDetail.Count > 0)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                    //decimal decimalVar1 = wZCompactDetail.CompactPrice;
                    //decimal decimalVar2 = decimalVar1 * wZCollect.ActualNumber;
                    //wZCollect.Ratio = decimal.Parse("0.17");
                    //if (wZCollect.Ratio != 0)
                    //{
                    //    wZCollect.ActualPrice = decimalVar1 / wZCollect.Ratio;
                    //}
                    //else
                    //{
                    //    wZCollect.ActualPrice = 0;
                    //}
                    //wZCollect.ActualMoney = wZCollect.ActualPrice * wZCollect.ActualNumber;
                    //wZCollect.RatioMoney = decimalVar2 - wZCollect.ActualMoney;


                    wZCollect.ActualPrice = decimalActualPrice;
                    wZCollect.ActualMoney = decimalActualMoney;
                    wZCollect.RatioMoney = decimalRatioMoney;

                    wZCollect.Ratio = decimalRatio;
                    //收料单<换算数量>
                    decimal decimalConvertRatio = GetConvertRatioByObjectCode(wZCompactDetail.ObjectCode);  //物资代码<换算系数>
                    if (decimalConvertRatio != 0)
                    {
                        wZCollect.ConvertNumber = wZCollect.ActualNumber / decimalConvertRatio;
                    }
                    else
                    {
                        wZCollect.ConvertNumber = 0;
                    }

                    wZCollect.Freight = decimalFreight;
                    wZCollect.OtherObject = decimalOtherObject;
                    wZCollect.TicketNumber = strTicketNumber;

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //重新加载收料单列表
                    DataCollectBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYXGDSLD").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        string strCollectCode = HF_CollectCode.Value;
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            string strCollectMethod = DDL_CollectMethod.SelectedValue;
            string strActualNumber = TXT_ActualNumber.Text.Trim();
            string strRatio = TXT_Ratio.Text.Trim();
            string strFreight = TXT_Freight.Text.Trim();
            string strOtherObject = TXT_OtherObject.Text.Trim();
            string strTicketNumber = TXT_TicketNumber.Text.Trim();

            string strActualPrice = TXT_ActualPrice.Text.Trim();
            string strActualMoney = TXT_ActualMoney.Text.Trim();
            string strRatioMoney = TXT_RatioMoney.Text.Trim();

            if (string.IsNullOrEmpty(strCollectMethod))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLFSBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSSLBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strRatio))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBNWKBC").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal decimalRatio = 0;
            decimal.TryParse(strRatio, out decimalRatio);
            decimal decimalFreight = 0;
            decimal.TryParse(strFreight, out decimalFreight);
            decimal decimalOtherObject = 0;
            decimal.TryParse(strOtherObject, out decimalOtherObject);

            decimal decimalActualPrice = 0;
            decimal.TryParse(strActualPrice, out decimalActualPrice);
            decimal decimalActualMoney = 0;
            decimal.TryParse(strActualMoney, out decimalActualMoney);
            decimal decimalRatioMoney = 0;
            decimal.TryParse(strRatioMoney, out decimalRatioMoney);

            //if (strCollectMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
            //{
            //    decimalActualNumber = decimalActualNumber > 0 ? (-1 * decimalActualNumber) : decimalActualNumber;
            //}

            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + strCollectCode + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];

                wZCollect.CollectMethod = strCollectMethod;
                wZCollect.ActualNumber = decimalActualNumber;
                //计算<实购金额>，<税金>，<换算数量>，<实购单价>

                WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                if (listCompactDetail != null && listCompactDetail.Count > 0)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                    //decimal decimalVar1 = wZCompactDetail.CompactPrice;
                    //decimal decimalVar2 = decimalVar1 * wZCollect.ActualNumber;
                    //wZCollect.Ratio = decimal.Parse("0.17");
                    //if (wZCollect.Ratio != 0)
                    //{
                    //    wZCollect.ActualPrice = decimalVar1 / wZCollect.Ratio;
                    //}
                    //else
                    //{
                    //    wZCollect.ActualPrice = 0;
                    //}
                    //wZCollect.ActualMoney = wZCollect.ActualPrice * wZCollect.ActualNumber;
                    //wZCollect.RatioMoney = decimalVar2 - wZCollect.ActualMoney;


                    wZCollect.ActualPrice = decimalActualPrice;
                    wZCollect.ActualMoney = decimalActualMoney;
                    wZCollect.RatioMoney = decimalRatioMoney;

                    wZCollect.Ratio = decimalRatio;
                    //收料单<换算数量>
                    decimal decimalConvertRatio = GetConvertRatioByObjectCode(wZCompactDetail.ObjectCode);  //物资代码<换算系数>
                    if (decimalConvertRatio != 0)
                    {
                        wZCollect.ConvertNumber = wZCollect.ActualNumber / decimalConvertRatio;
                    }
                    else
                    {
                        wZCollect.ConvertNumber = 0;
                    }

                    wZCollect.Freight = decimalFreight;
                    wZCollect.OtherObject = decimalOtherObject;
                    wZCollect.TicketNumber = strTicketNumber;

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //重新加载收料单列表
                    DataCollectBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYXGDSLD").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Collect.Items.Count; i++)
        {
            DG_Collect.Items[i].ForeColor = Color.Black;
        }

        HF_CollectCode.Value = "";
        LB_CollectCode.Text = "";
        DDL_CollectMethod.SelectedValue = "";
        TXT_ActualNumber.Text = "0";
        TXT_Ratio.Text = "0";
        TXT_Freight.Text = "0";
        TXT_OtherObject.Text = "";
        TXT_TicketNumber.Text = "";
    }

    protected void BT_MoreAdd_Click(object sender, EventArgs e)
    {
        string strCompactDetailID = Request.Form["cb_CompactDetail_ID"];
        if (!string.IsNullOrEmpty(strCompactDetailID))
        {
            string[] arrCompactDetailID = strCompactDetailID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrCompactDetailID.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrCompactDetailID[i]))
                {
                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + arrCompactDetailID[i];
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                        WZCompactBLL wZCompactBLL = new WZCompactBLL();
                        string strWZCompactHQL = string.Format("from WZCompact as wZCompact where CompactCode = '{0}'", wZCompactDetail.CompactCode);
                        IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
                        if (listCompact != null && listCompact.Count > 0)
                        {
                            WZCompact wZCompact = (WZCompact)listCompact[0];

                            //增加收料单
                            //收料编号
                            string strNewCollectCode = CreateNewCollectCode();

                            WZCollect wZCollect = new WZCollect();
                            wZCollect.CollectCode = strNewCollectCode;
                            wZCollect.TicketTime = DateTime.Now;

                            wZCollect.CollectMethod = LanguageHandle.GetWord("LanPiao").ToString().Trim();

                            wZCollect.Contacter = strUserCode;
                            wZCollect.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                            wZCollect.CompactDetailID = wZCompactDetail.ID;
                            wZCollect.PlanDetailID = wZCompactDetail.PlanDetailID;
                            wZCollect.ObjectCode = wZCompactDetail.ObjectCode;
                            wZCollect.CheckCode = wZCompactDetail.CheckCode;
                            wZCollect.ProjectCode = wZCompact.ProjectCode;
                            wZCollect.SupplierCode = wZCompact.SupplierCode;
                            wZCollect.StoreRoom = wZCompact.StoreRoom;
                            wZCollect.Checker = wZCompact.Checker;
                            wZCollect.Safekeeper = wZCompact.Safekeep;
                            //wZCollect.CheckTime = DateTime.Now;
                            //wZCollect.CollectTime = DateTime.Now;
                            wZCollect.RequestCode = "-";
                            wZCollect.FinanceApprove = "-";
                            wZCollect.PayProcess = "-";

                            wZCollect.CompactCode = wZCompact.CompactCode;

                            //在途收料单〈实收数量〉
                            decimal decimalCurrentNumber = 0;
                            string strCollectHQL = string.Format(@"select CompactDetailID,SUM(ActualNumber) as SumActualNumber from T_WZCollect
                                   where CompactDetailID={0} group by CompactDetailID", wZCompactDetail.ID);
                            DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];
                            if (dtCollect != null && dtCollect.Rows.Count > 0)
                            {
                                decimal.TryParse(ShareClass.ObjectToString(dtCollect.Rows[0]["SumActualNumber"]), out decimalCurrentNumber);
                            }
                            wZCollect.CollectNumber = wZCompactDetail.CompactNumber - wZCompactDetail.CollectNumber - decimalCurrentNumber;
                            wZCollect.ActualNumber = wZCollect.CollectNumber;

                            decimal decimalVar1 = wZCompactDetail.CompactPrice;
                            decimal decimalVar2 = decimalVar1 * wZCollect.ActualNumber;
                            wZCollect.Ratio = decimal.Parse("0.17");
                            if (wZCollect.Ratio != 0)
                            {
                                wZCollect.ActualPrice = decimalVar1 / wZCollect.Ratio;
                            }
                            else
                            {
                                wZCollect.ActualPrice = 0;
                            }
                            wZCollect.ActualMoney = wZCollect.ActualPrice * wZCollect.ActualNumber;
                            wZCollect.RatioMoney = decimalVar2 - wZCollect.ActualMoney;
                            //收料单<换算数量>
                            decimal decimalConvertRatio = GetConvertRatioByObjectCode(wZCompactDetail.ObjectCode);  //物资代码<换算系数>
                            if (decimalConvertRatio != 0)
                            {
                                wZCollect.ConvertNumber = wZCollect.ActualNumber / decimalConvertRatio;
                            }
                            else
                            {
                                wZCollect.ConvertNumber = 0;
                            }
                            WZCollectBLL wZCollectBLL = new WZCollectBLL();
                            wZCollectBLL.AddWZCollect(wZCollect);

                            //修改合同明细<使用标记> = -1
                            wZCompactDetail.IsMark = -1;
                            wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);
                        }
                    }
                }
            }

            //重新加载收料单列表，合同明细也重新加载
            DataCollectBinder();
            DataCompactDetailBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZBiaoZhuPiLiangLanguageHandle").ToString().Trim()+"')", true); 
        }
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataCollectBinder();
    }

    protected void BT_SortCollectCode_Click(object sender, EventArgs e)
    {
        DG_Collect.CurrentPageIndex = 0;

        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Contacter ='{0}' 
                            and c.Progress in ('录入','材检','开票') 
                           ", strUserCode);   //ChineseWord

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strCollectCode = DDL_CollectCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            strCollectHQL += " and c.CollectCode like '%" + strCollectCode + "%'";
        }
        string strCompactCode = DDL_CompactCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCollectHQL += " and c.CompactCode like '%" + strCompactCode + "%'";
        }
    
        if (!string.IsNullOrEmpty(HF_SortCollectCode.Value))
        {
            strCollectHQL += " order by c.CollectCode desc";

            HF_SortCollectCode.Value = "";
        }
        else
        {
            strCollectHQL += " order by c.CollectCode asc";

            HF_SortCollectCode.Value = "asc";
        }

        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        LB_CollectSql.Text = strCollectHQL;
    }

    protected void BT_SortTicketTime_Click(object sender, EventArgs e)
    {
        DG_Collect.CurrentPageIndex = 0;

        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Contacter ='{0}' 
                            and c.Progress in ('录入','材检','开票') 
                           ", strUserCode);   //ChineseWord

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strCollectCode = DDL_CollectCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            strCollectHQL += " and c.CollectCode like '%" + strCollectCode + "%'";
        }
        string strCompactCode = DDL_CompactCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCollectHQL += " and c.CompactCode like '%" + strCompactCode + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortTicketTime.Value))
        {
            strCollectHQL += "  order by c.TicketTime desc";

            HF_SortTicketTime.Value = "";
        }
        else
        {
            strCollectHQL += "  order by c.TicketTime asc";

            HF_SortTicketTime.Value = "asc";
        }

        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        LB_CollectSql.Text = strCollectHQL;
    }

    protected void BT_SortCompactCode_Click(object sender, EventArgs e)
    {
        DG_Collect.CurrentPageIndex = 0;

        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Contacter ='{0}' 
                            and c.Progress in ('录入','材检','开票') 
                           ", strUserCode);   //ChineseWord

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strCollectCode = DDL_CollectCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            strCollectHQL += " and c.CollectCode like '%" + strCollectCode + "%'";
        }
        string strCompactCode = DDL_CompactCode.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCollectHQL += " and c.CompactCode like '%" + strCompactCode + "%'";
        }


        if (!string.IsNullOrEmpty(HF_SortCompactCode.Value))
        {
            strCollectHQL += " order by c.CompactCode desc";

            HF_SortCompactCode.Value = "";
        }
        else
        {
            strCollectHQL += " order by c.CompactCode asc";

            HF_SortCompactCode.Value = "asc";
        }

        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        LB_CollectSql.Text = strCollectHQL;
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

    protected void DDL_CollectMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCollectMethod = DDL_CollectMethod.SelectedValue;
        if (!string.IsNullOrEmpty(strCollectMethod))
        {
            decimal decimalActualNumber = 0;
            decimal.TryParse(TXT_ActualNumber.Text.Trim(), out decimalActualNumber);

            if (strCollectMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
            {
                decimalActualNumber = decimalActualNumber > 0 ? -1 * decimalActualNumber : decimalActualNumber;

                if (decimalActualNumber == 0)
                {
                    TXT_ActualNumber.Text = "-0.00";
                }
                else
                {
                    TXT_ActualNumber.Text = decimalActualNumber.ToString("#0.00");
                }

                //TXT_ActualNumber.Style.Add("color", "red");
                TXT_ActualNumber.BackColor = Color.Red;
            }
            else
            {
                decimalActualNumber = decimalActualNumber > 0 ? decimalActualNumber : -1 * decimalActualNumber;

                TXT_ActualNumber.Text = decimalActualNumber.ToString("#0.00");

                //TXT_ActualNumber.Style.Remove("color");
                TXT_ActualNumber.BackColor = Color.White;
            }
        }
    }

   

    protected void DG_Collect_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Collect.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_CollectSql.Text.Trim();
        DataTable dtCollect = ShareClass.GetDataSetFromSql(strHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();
    }

    protected void BT_AutualPrice_Click(object sender, EventArgs e)
    {
        decimal deActualNumber, deActualMoney;

        try
        {
            deActualNumber = decimal.Parse(TXT_ActualNumber.Text);
            deActualMoney = decimal.Parse(TXT_ActualMoney.Text);

            TXT_ActualPrice.Text = (deActualMoney / deActualNumber).ToString();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoShiShouJinEHeShiShouS").ToString().Trim()+"')", true);
            return;
        }
    }

    protected void BT_ActualNumber_Click(object sender, EventArgs e)
    {
        decimal deActualNumber, deActualPrice;

        try
        {
            deActualNumber = decimal.Parse(TXT_ActualNumber.Text);
            deActualPrice = decimal.Parse(TXT_ActualPrice.Text);

            TXT_ActualMoney.Text = (deActualNumber * deActualPrice).ToString();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoShiShouChanJiaHeShiSh").ToString().Trim()+"')", true);
            return;
        }

        //string strCollectCode = LB_CollectCode.Text;
        //if (!string.IsNullOrEmpty(strCollectCode))
        //{
        //    string strActualNumber = TXT_ActualNumber.Text.Trim();
        //    string strRatio = TXT_Ratio.Text.Trim();

        //    if (!ShareClass.CheckIsNumber(strActualNumber))
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSSLBXWXSHZZS").ToString().Trim() + "')", true);
        //        return;
        //    }
        //    if (!ShareClass.CheckIsNumber(strRatio))
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBXWXSHZZS").ToString().Trim() + "')", true);
        //        return;
        //    }

        //    decimal decimalActualNumber = 0;
        //    decimal.TryParse(strActualNumber, out decimalActualNumber);
        //    decimal decimalRatio = 0;
        //    decimal.TryParse(strRatio, out decimalRatio);

        //    WZCollectBLL wZCollectBLL = new WZCollectBLL();
        //    string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + strCollectCode + "'";
        //    IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
        //    if (listCollect != null && listCollect.Count == 1)
        //    {
        //        WZCollect wZCollect = (WZCollect)listCollect[0];

        //        WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
        //        string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
        //        IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
        //        if (listCompactDetail != null && listCompactDetail.Count > 0)
        //        {
        //            WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

        //            //计算<实购金额>，<税金>，<换算数量>，<实购单价>

        //            decimal decimalVar1 = wZCompactDetail.CompactPrice;
        //            decimal decimalVar2 = decimalVar1 * decimalActualNumber;

        //            decimal decimalResult = 0;
        //            if (decimalRatio != 0)
        //            {
        //                decimalResult = decimalVar1 / decimalRatio;
        //            }

        //            TXT_ActualPrice.Text = decimalResult.ToString("#0.00");

        //            decimal decimalActualMoney = decimalResult * decimalActualNumber;

        //            TXT_ActualMoney.Text = decimalActualMoney.ToString("#0.00");
        //            TXT_RatioMoney.Text = (decimalVar2 - decimalActualMoney).ToString("#0.00");
        //        }
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYXGDSLD").ToString().Trim() + "')", true);
        //    return;
        //}
    }


    protected void BT_Ratio_Click(object sender, EventArgs e)
    {
        decimal deCollectMoney, DeActualMoney;

        try
        {
            deCollectMoney = decimal.Parse(TXT_CollectMoney.Text);
            DeActualMoney = decimal.Parse(TXT_ActualMoney.Text);

            TXT_RatioMoney.Text = (deCollectMoney - DeActualMoney).ToString();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoShiShouJinEHeShiGouJi").ToString().Trim()+"')", true);
            return;
        }

        //string strCollectCode = LB_CollectCode.Text;
        //if (!string.IsNullOrEmpty(strCollectCode))
        //{
        //    string strActualNumber = TXT_ActualNumber.Text.Trim();
        //    string strRatio = TXT_Ratio.Text.Trim();

        //    if (!ShareClass.CheckIsNumber(strActualNumber))
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSSLBXWXSHZZS").ToString().Trim() + "')", true);
        //        return;
        //    }
        //    if (!ShareClass.CheckIsNumber(strRatio))
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBXWXSHZZS").ToString().Trim() + "')", true);
        //        return;
        //    }

        //    decimal decimalActualNumber = 0;
        //    decimal.TryParse(strActualNumber, out decimalActualNumber);
        //    decimal decimalRatio = 0;
        //    decimal.TryParse(strRatio, out decimalRatio);

        //    WZCollectBLL wZCollectBLL = new WZCollectBLL();
        //    string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + strCollectCode + "'";
        //    IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
        //    if (listCollect != null && listCollect.Count == 1)
        //    {
        //        WZCollect wZCollect = (WZCollect)listCollect[0];

        //        WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
        //        string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
        //        IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
        //        if (listCompactDetail != null && listCompactDetail.Count > 0)
        //        {
        //            WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

        //            //计算<实购金额>，<税金>，<换算数量>，<实购单价>

        //            decimal decimalVar1 = wZCompactDetail.CompactPrice;
        //            decimal decimalVar2 = decimalVar1 * decimalActualNumber;

        //            decimal decimalResult = 0;
        //            if (decimalRatio != 0)
        //            {
        //                decimalResult = decimalVar1 / decimalRatio;
        //            }

        //            TXT_ActualPrice.Text = decimalResult.ToString("#0.00");

        //            decimal decimalActualMoney = decimalResult * decimalActualNumber;
        //            TXT_ActualMoney.Text = decimalActualMoney.ToString("#0.00");
        //            TXT_RatioMoney.Text = (decimalVar2 - decimalActualMoney).ToString("#0.00");
        //        }
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYXGDSLD").ToString().Trim() + "')", true);
        //    return;
        //}
    }


    protected void BT_CollectMoney_Click(object sender, EventArgs e)
    {
        decimal deActualNumber, deCompactPrice;

        try
        {
            deActualNumber = decimal.Parse(TXT_ActualNumber.Text);
            deCompactPrice = decimal.Parse(TXT_CompactPrice.Text);

            TXT_CollectMoney.Text = (deActualNumber * deCompactPrice).ToString();

        }
        catch
        {

        }
    }


    private string CreateNewCollectCode()
    {
        string strNewCollectCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strCollectCodeHQL = string.Format("select count(1) as RowNumber from T_WZCollect where to_char( TicketTime, 'yyyy-mm-dd') like '{0}%'", DateTime.Now.ToString("yyyy-MM"));
                DataTable dtCollectCode = ShareClass.GetDataSetFromSql(strCollectCodeHQL, "CollectCode").Tables[0];
                int intCollectCodeNumber = int.Parse(dtCollectCode.Rows[0]["RowNumber"].ToString());
                intCollectCodeNumber = intCollectCodeNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbCollectCode = new StringBuilder();
                    for (int j = 4 - intCollectCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbCollectCode.Append("0");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewCollectCode = strYear + strMonth + sbCollectCode.ToString() + intCollectCodeNumber.ToString();

                    //验证新的合同编号是否存在
                    string strCheckNewCollectCodeHQL = "select count(1) as RowNumber from T_WZCollect where CollectCode = '" + strNewCollectCode + "'";
                    DataTable dtCheckNewCollectCode = ShareClass.GetDataSetFromSql(strCheckNewCollectCodeHQL, "CheckNewCollectCode").Tables[0];
                    int intCheckNewCollectCode = int.Parse(dtCheckNewCollectCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewCollectCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intCollectCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }

        return strNewCollectCode;
    }

}
