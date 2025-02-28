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
using System.Drawing;
using System.Data;
using System.Text;

public partial class TTWZCollectKeep : System.Web.UI.Page
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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataCollectBinder();
        }
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataCollectBinder();
    }

    private void DataCollectBinder()
    {
        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,s.SupplierName,h.UserName as CheckerName,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,
                            f.UserName as FinanceApproveName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode 
                            left join T_ProjectMember f on c.FinanceApprove = f.UserCode
                            where c.Safekeeper ='{0}' 
                            and c.Progress in('开票','收料')  ", strUserCode);


        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != "全部")
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }

        strCollectHQL += "  order by c.TicketTime desc";

        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();
    }

    protected void DG_Collect_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "account")
        {
            //上帐
            string cmdArges = e.CommandArgument.ToString();
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];
                if (wZCollect.Progress == "开票")
                {
                    wZCollect.Progress = "收料";
                    wZCollect.IsMark = -1;
                    wZCollect.CollectTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string strMessage = AccountHandler(wZCollect.CollectCode);

                    if (strMessage == "成功")
                    {
                        //重新加载收料单列表
                        DataCollectBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSZCG + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSB + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSB + "')", true);
                    return;
                }
            }
        }
        else if (cmdName == "notAccount")
        {
            //退单
            string cmdArges = e.CommandArgument.ToString();
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];
                //string strWZStateHQL = "select * from T_WZState";
                //DataTable dtWZState = ShareClass.GetDataSetFromSql(strWZStateHQL, "State").Tables[0];
                //if (dtWZState != null && dtWZState.Rows.Count > 0)
                //{
                //    string strYear = ShareClass.ObjectToString(dtWZState.Rows[0]["CYear"]);
                //    string strMonth = ShareClass.ObjectToString(dtWZState.Rows[0]["CMonth"]);
                //    string strCurrentTime = strYear + "-" + strMonth + "-01";
                //    string strCollectTime = DateTime.Parse(wZCollect.CollectTime).ToString("yyyy-MM-01");

                //if (strCurrentTime == strCollectTime && wZCollect.IsMark == -1)
                if (wZCollect.Progress == "收料")
                {
                    wZCollect.Progress = "开票";
                    wZCollect.IsMark = 0;
                    wZCollect.CollectTime = "";

                    string strMessage = NotAccountHandler(wZCollect.CollectCode);

                    if (strMessage == "成功")
                    {
                        //重新加载收料单列表
                        DataCollectBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZTDCG + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + strMessage + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSLRBSDNYHZJSBJBW1BNTD + "')", true);
                    return;
                }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSTRMESSAGE + "')", true);
                //}
            }
        }
        else if (cmdName == "print")
        {
            //打印
            string cmdArges = e.CommandArgument.ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCollectPrintPag.aspx?collectCode=" + cmdArges + "');", true);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZCollectPrintPag.aspx?collectCode=" + cmdArges + "')", true);
            return;
        }
    }

    /// <summary>
    /// 上帐，写入库存
    /// </summary>
    private string AccountHandler(string strCollectCode)
    {
        string strResult = string.Empty;

        try
        {
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + strCollectCode + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];

                //<库别>，<物资代码>，<检号>，判断库存中是否存在当前入库物资
                string strStockCode = wZCollect.StoreRoom;
                string strObjectCode = wZCollect.ObjectCode;
                string strCheckCode = wZCollect.CheckCode;

                string strWZStoreHQL = string.Format(@"from WZStore as wZStore
                    where wZStore.StockCode = '{0}'
                    and wZStore.ObjectCode = '{1}'
                    and wZStore.CheckCode = '{2}'", strStockCode, strObjectCode, strCheckCode);
                WZStoreBLL wZStoreBLL = new WZStoreBLL();
                IList lstWZStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                if (lstWZStore != null && lstWZStore.Count == 1)
                {
                    //库存中存在当前物资
                    WZStore wZStore = (WZStore)lstWZStore[0];
                    if (wZStore.DownDesc == -1 || wZStore.WearyDesc == -1)
                    {
                        strResult = "积压计划和减值计划不为0，先平库，后采购！";
                        return strResult;
                    }

                    wZStore.InNumber += wZCollect.ActualNumber;//入库数量
                    wZStore.InMoney += wZCollect.ActualMoney;   //实购金额
                    wZStore.StoreNumber = wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber;//库存数量
                    wZStore.StoreMoney = wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice;    //库存金额
                                                                                                    //库存单价
                    if (wZStore.StoreNumber != 0)
                    {
                        wZStore.StorePrice = wZStore.StoreMoney / wZStore.StoreNumber;
                    }
                    else
                    {
                        wZStore.StorePrice = 0;
                    }

                    wZStore.DownRatio = 0;
                    wZStore.DownMoney = wZStore.StoreMoney * wZStore.DownRatio;
                    wZStore.CleanMoney = wZStore.StoreMoney - wZStore.DownMoney;
                    wZStore.DownCode = "-";
                    wZStore.WearyCode = "-";
                    //wZStore.StockCode = "-";
                    wZStore.GoodsCode = "-";
                    wZStore.DownDesc = 0;
                    wZStore.WearyDesc = 0;

                    wZStore.EndInTime = DateTime.Now;//末次入库
                    wZStore.IsMark = -1;                //使用标记

                    //修改库存
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //收料单<收料日期>=系统日期，进度=收料，结算标记=-1
                    wZCollect.CollectTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZCollect.Progress = "收料";
                    wZCollect.IsMark = -1;
                    //修改收料单
                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //修改合同明细
                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                        wZCompactDetail.CollectNumber += wZCollect.ActualNumber;
                        wZCompactDetail.CollectMoney += wZCollect.ActualMoney + wZCollect.RatioMoney + wZCollect.Freight + wZCollect.OtherObject;

                        wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                        //修改合同《收料总额》=合同明细的总和
                        string strSelectCompactDetailHQL = "select SUM(CollectMoney) as CompactMoney from T_WZCompactDetail where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strSelectCompactDetailHQL, "strSelectCompactDetailHQL").Tables[0];
                        decimal decimalCollectMoney = 0;
                        if (dtCompactDetail != null && dtCompactDetail.Rows.Count > 0)
                        {
                            decimal.TryParse(ShareClass.ObjectToString(dtCompactDetail.Rows[0]["CompactMoney"]), out decimalCollectMoney);

                            string strCompatctHQL = "update T_WZCompact set CollectMoney = " + decimalCollectMoney + " where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                            ShareClass.RunSqlCommand(strCompatctHQL);
                        }
                    }

                    //工程项目，<收料金额>，<税金>，<运杂费>
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZCollect.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZProject.AcceptMoney += wZCollect.ActualMoney;
                        wZProject.ProjectTax += wZCollect.RatioMoney;
                        wZProject.TheFreight += wZCollect.Freight + wZCollect.OtherObject;

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                    }

                    //计划明细<进度> = "收料"
                    string strPlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '收料' where ID = " + wZCollect.PlanDetailID;
                    ShareClass.RunSqlCommand(strPlanDetailHQL);

                    strResult = "成功";
                }
                else if (lstWZStore.Count > 1)
                {
                    //库存中存在多个当前物资
                    strResult = "库存中存在多个，库别，物资代码，检号一样的(" + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
                }
                else
                {
                    //库存中不存在当前物资
                    WZStore wZStore = new WZStore();
                    wZStore.StockCode = strStockCode;
                    wZStore.ObjectCode = strObjectCode;
                    wZStore.CheckCode = strCheckCode;

                    wZStore.InNumber += wZCollect.ActualNumber;//入库数量
                    wZStore.InMoney += wZCollect.ActualMoney;   //实购金额
                    wZStore.StoreNumber = wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber;//库存数量
                    wZStore.StoreMoney = wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice;    //库存金额
                                                                                                    //库存单价
                    if (wZStore.StoreNumber != 0)
                    {
                        wZStore.StorePrice = wZStore.StoreMoney / wZStore.StoreNumber;
                    }
                    else
                    {
                        wZStore.StorePrice = 0;
                    }
                    //wZStore.EndInTime = DateTime.Now;//末次入库              
                    wZStore.IsMark = -1; //使用标记

                    wZStore.DownRatio = 0;
                    wZStore.DownMoney = wZStore.StoreMoney * wZStore.DownRatio;
                    wZStore.CleanMoney = wZStore.StoreMoney - wZStore.DownMoney;
                    wZStore.DownCode = "-";
                    wZStore.WearyCode = "-";
                    //wZStore.StockCode = "-";
                    wZStore.GoodsCode = "-";
                    wZStore.DownDesc = 0;
                    wZStore.WearyDesc = 0;

                    wZStore.YearTime = DateTime.Now;
                    wZStore.EndInTime = DateTime.Now;
                    wZStore.EndOutTime = DateTime.Now;

                    //增加库存
                    wZStoreBLL.AddWZStore(wZStore);

                    //收料单<收料日期>=系统日期，进度=收料，结算标记=-1
                    //wZCollect.CollectTime = DateTime.Now;
                    wZCollect.CollectTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZCollect.Progress = "收料";
                    wZCollect.IsMark = -1;

                    //修改收料单
                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //修改合同明细
                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                        wZCompactDetail.CollectNumber += wZCollect.ActualNumber;
                        wZCompactDetail.CollectMoney += wZCollect.ActualMoney + wZCollect.RatioMoney + wZCollect.Freight + wZCollect.OtherObject;

                        wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                        //修改合同《收料总额》=合同明细的总和
                        string strSelectCompactDetailHQL = "select SUM(CollectMoney) as CompactMoney from T_WZCompactDetail where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strSelectCompactDetailHQL, "strSelectCompactDetailHQL").Tables[0];
                        decimal decimalCollectMoney = 0;
                        if (dtCompactDetail != null && dtCompactDetail.Rows.Count > 0)
                        {
                            decimal.TryParse(ShareClass.ObjectToString(dtCompactDetail.Rows[0]["CompactMoney"]), out decimalCollectMoney);

                            string strCompatctHQL = "update T_WZCompact set CollectMoney = " + decimalCollectMoney + " where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                            ShareClass.RunSqlCommand(strCompatctHQL);
                        }
                    }

                    //工程项目，<收料金额>，<税金>，<运杂费>
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZCollect.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZProject.AcceptMoney += wZCollect.ActualMoney;
                        wZProject.ProjectTax += wZCollect.RatioMoney;
                        wZProject.TheFreight += wZCollect.Freight + wZCollect.OtherObject;

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                    }

                    //计划明细<进度> = "收料"
                    string strPlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '收料' where ID = " + wZCollect.PlanDetailID;
                    ShareClass.RunSqlCommand(strPlanDetailHQL);

                    strResult = "成功";
                }
            }
        }
        catch (Exception ex)
        {
            strResult = ex.Message.ToString();
        }
        return strResult;
    }

    /// <summary>
    ///  退单
    /// </summary>
    private string NotAccountHandler(string strCollectCode)
    {
        string strResult = string.Empty;

        try
        {
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + strCollectCode + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];

                string strStockCode = wZCollect.StoreRoom;
                string strObjectCode = wZCollect.ObjectCode;
                string strCheckCode = wZCollect.CheckCode;

                string strWZStoreHQL = string.Format(@"from WZStore as wZStore
                    where StockCode = '{0}'
                    and ObjectCode = '{1}'
                    and CheckCode = '{2}'", strStockCode, strObjectCode, strCheckCode);
                WZStoreBLL wZStoreBLL = new WZStoreBLL();
                IList lstWZStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                if (lstWZStore != null && lstWZStore.Count == 1)
                //if (lstWZStore != null && lstWZStore.Count > 0)
                {
                    //库存中存在当前物资
                    WZStore wZStore = (WZStore)lstWZStore[0];

                    wZStore.InNumber -= wZCollect.ActualNumber;//入库数量
                    wZStore.InMoney -= wZCollect.ActualMoney;   //实购金额
                    wZStore.StoreNumber = wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber;//库存数量
                    wZStore.StoreMoney = wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice;    //库存金额
                                                                                                    //库存单价
                    wZStore.CleanMoney = wZStore.StoreMoney = wZStore.DownMoney; //库存净额

                    if (wZStore.StoreNumber != 0)
                    {
                        wZStore.StorePrice = wZStore.StoreMoney / wZStore.StoreNumber;
                    }
                    else
                    {
                        wZStore.StorePrice = 0;
                    }
                    string strEndCollectHQL = string.Format(@"select * from T_WZCollect
                    where StoreRoom = '{0}'
                    and ObjectCode = '{1}'
                    and CheckCode = '{2}'
                    and CollectCode != '{3}'", strStockCode, strObjectCode, strCheckCode, strCollectCode);
                    DataTable dtEndCollect = ShareClass.GetDataSetFromSql(strEndCollectHQL, "EndCollect").Tables[0];
                    if (dtEndCollect != null && dtEndCollect.Rows.Count > 0)
                    {
                        DateTime dtCollectTime = DateTime.Now;

                        try
                        {
                            string strCollectTime = DateTime.Parse(dtEndCollect.Rows[0]["CollectTime"].ToString()).ToString("yyyyMMdd");

                            DateTime.TryParse(ShareClass.ObjectToString(dtEndCollect.Rows[0]["CollectTime"]), out dtCollectTime);
                        }
                        catch
                        { }


                        wZStore.EndInTime = dtCollectTime;//末次入库，之前最后一份收料单的收料日期
                    }

                    ////判断是否是“0库存”，所有数字、货币类型的字段值 ="0"
                    //if (wZStore.YearNumber == 0 && wZStore.YearMoney == 0
                    //    && wZStore.InNumber == 0 && wZStore.InMoney == 0
                    //    && wZStore.OutNumber == 0 && wZStore.OutPrice == 0
                    //    && wZStore.StoreNumber == 0 && wZStore.StoreMoney == 0)
                    //{
                    //    wZCollect.IsMark = 0;
                    //}

                    //判断是否是“0库存”
                    if (wZStore.StoreNumber == 0 )
                    {
                        wZStore.IsMark = 0;

                        wZCollect.IsMark = 0;
                    }

                    //修改库存
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //收料单进度=开票，结算标记=0
                    wZCollect.CollectTime = "-";
                    wZCollect.Progress = "开票";
                    wZCollect.IsMark = 0;
                    //修改收料单
                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //修改合同明细
                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                        wZCompactDetail.CollectNumber -= wZCollect.ActualNumber;
                        wZCompactDetail.CollectMoney = wZCompactDetail.CollectMoney - (wZCollect.ActualMoney + wZCollect.RatioMoney + wZCollect.Freight + wZCollect.OtherObject);

                        wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                        //修改合同《收料总额》=合同明细的总和
                        string strSelectCompactDetailHQL = "select SUM(CollectMoney) as CompactMoney from T_WZCompactDetail where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strSelectCompactDetailHQL, "strSelectCompactDetailHQL").Tables[0];
                        decimal decimalCollectMoney = 0;
                        if (dtCompactDetail != null && dtCompactDetail.Rows.Count > 0)
                        {
                            decimal.TryParse(ShareClass.ObjectToString(dtCompactDetail.Rows[0]["CompactMoney"]), out decimalCollectMoney);

                            string strCompatctHQL = "update T_WZCompact set CollectMoney = " + decimalCollectMoney + " where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                            ShareClass.RunSqlCommand(strCompatctHQL);
                        }
                    }

                    //工程项目，<收料金额>，<税金>，<运杂费>
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZCollect.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZProject.AcceptMoney -= wZCollect.ActualMoney;
                        wZProject.ProjectTax -= wZCollect.RatioMoney;
                        wZProject.TheFreight = wZProject.TheFreight - (wZCollect.Freight + wZCollect.OtherObject);

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                    }

                    //计划明细<进度> = "合同"
                    string strPlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '合同' where ID = " + wZCollect.PlanDetailID;
                    ShareClass.RunSqlCommand(strPlanDetailHQL);

                    strResult = "成功";
                }
                else
                {
                    strResult = "警告，库存中不存在当前物资，请检查！";
                }
            }
        }
        catch (Exception ex)
        {
            strResult = ex.Message.ToString();
        }

        return strResult;
    }

}