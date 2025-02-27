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
using System.Text;
using System.Drawing;

public partial class TTWZSendSafekeep : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx");
        bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {

            DataSendBinder();

        }
    }

    private void DataSendBinder()
    {
        string strSendHQL = string.Format(@"select s.*,d.PlanCode,o.ObjectName,
                    b.UserName as UpLeaderName,
                    c.UserName as CheckerName,
                    f.UserName as SafekeeperName,
                    p.UserName as PurchaseEngineerName
                    from T_WZSend s
                    left join T_WZPickingPlanDetail d on s.PlanDetaiID = d.ID
                    left join T_WZObject o on s.ObjectCode = o.ObjectCode
                    left join T_ProjectMember b on s.UpLeader = b.UserCode
                    left join T_ProjectMember c on s.Checker = c.UserCode
                    left join T_ProjectMember f on s.Safekeeper = f.UserCode
                    left join T_ProjectMember p on s.PurchaseEngineer = p.UserCode
                    where s.Safekeeper ='{0}' 
                    and s.Progress in ('开票','发料') 
                    order by s.TicketTime desc", strUserCode); 
        DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];

        DG_Send.DataSource = dtSend;
        DG_Send.DataBind();

        LB_SendSql.Text = strSendHQL;
    }

    protected void DG_Send_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "account")
            {
                //上帐
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress == LanguageHandle.GetWord("KaiPiao").ToString().Trim())
                    {
                        wZSend.Progress = LanguageHandle.GetWord("FaLiao").ToString().Trim();
                        wZSend.IsMark = -1;
                        wZSend.SendTime = DateTime.Now.ToString();

                        //上帐
                        string strMessage = AccountHandler(wZSend.SendCode);
                        if (strMessage == "Success")
                        {
                            //重新加载收料单列表
                            DataSendBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSZCG").ToString().Trim() + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + strMessage + "')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWKPBNSZ").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }

            if (cmdName == "cancelAccount")
            {
                //上帐
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress == LanguageHandle.GetWord("FaLiao").ToString().Trim())
                    {
                        wZSend.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                        wZSend.IsMark = 0;
                        wZSend.SendTime = "-";

                        //上帐
                        string strMessage = cancelAccountHandler(wZSend.SendCode);
                        if (strMessage == "Success")
                        {
                            //重新加载收料单列表
                            DataSendBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQuXiaoShangZhangChengGong").ToString().Trim()+"')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + strMessage + "')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJinDuBuWeiFaLiaoBuNengQuXiao").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
        }
    }

    protected void DG_Send_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Send.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SendSql.Text.Trim();
        DataTable dtSend = ShareClass.GetDataSetFromSql(strHQL, "Send").Tables[0];

        DG_Send.DataSource = dtSend;
        DG_Send.DataBind();
    }




    /// <summary>
    /// 上帐，写入库存
    /// </summary>
    private string AccountHandler(string strSendCode)
    {
        string strResult = string.Empty;

        try
        {
            WZSendBLL wZSendBLL = new WZSendBLL();
            string strSendHQL = "from WZSend as wZSend where SendCode = '" + strSendCode + "'";
            IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
            if (listSend != null && listSend.Count == 1)
            {
                WZSend wZSend = (WZSend)listSend[0];

                //<库别>，<物资代码>，<检号>，判断库存中是否存在当前入库物资
                string strStockCode = wZSend.StoreRoom;
                string strObjectCode = wZSend.ObjectCode;
                string strCheckCode = wZSend.CheckCode;

                string strWZStoreHQL = string.Format(@"from WZStore as wZStore
                    where StockCode = '{0}'
                    and ObjectCode = '{1}'
                    and CheckCode = '{2}'", strStockCode, strObjectCode, strCheckCode);
                WZStoreBLL wZStoreBLL = new WZStoreBLL();
                IList lstWZStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                if (lstWZStore != null && lstWZStore.Count == 1)
                {
                    //库存中存在当前物资
                    WZStore wZStore = (WZStore)lstWZStore[0];

                    wZStore.OutNumber += wZSend.ActualNumber;//出库数量
                    wZStore.OutPrice += wZSend.TotalMoney;   //出库金额
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
                    wZStore.EndOutTime = DateTime.Now;//末次入库
                    wZStore.IsMark = -1;                //使用标记

                    //修改库存
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //发料单<收料日期>=系统日期，进度=发料，结算标记=-1
                    wZSend.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZSend.Progress = LanguageHandle.GetWord("FaLiao").ToString().Trim();
                    wZSend.IsMark = -1;
                    //修改收料单
                    wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                    //修改计划明细
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + wZSend.PlanDetaiID;
                    IList listPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strPlanDetailHQL);
                    if (listPlanDetail != null && listPlanDetail.Count > 0)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listPlanDetail[0];

                        wZPickingPlanDetail.ShortNumber += wZSend.ActualNumber;              //缺口数量
                        decimal decimalShortConver = 0;
                        decimal decimalConvertRatio = GetConvertRatioByObjectCode(strObjectCode);   //换算系数
                        if (decimalConvertRatio != 0)
                        {
                            decimalShortConver = wZPickingPlanDetail.ShortNumber / decimalConvertRatio;
                        }
                        wZPickingPlanDetail.ShortConver += decimalShortConver;    //缺口换算

                        wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);

                    }

                    //工程项目，<收料金额>，<税金>，<运杂费>
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZSend.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZProject.SendMoney += wZSend.TotalMoney;

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                    }

                    //计划明细<进度> = LanguageHandle.GetWord("FaLiao").ToString().Trim()
                    string strUpdatePlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '发料' where ID = " + wZSend.PlanDetaiID; 
                    ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                    strResult = "Success";
                    return strResult;
                }
                else if (lstWZStore.Count > 1)
                {
                    //库存中存在多个当前物资
                    strResult = LanguageHandle.GetWord("KuCunZhongCunZaiDuoGeKuBieWuZi").ToString().Trim() + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
                }
                else
                {
                    //库存中不存在当前物资
                    strResult = LanguageHandle.GetWord("KuCunZhongBuCunZaiDangQianWuZi").ToString().Trim() + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
                }
            }
        }
        catch (Exception ex)
        {
            strResult = LanguageHandle.GetWord("ChuCuoLeQingLianJiGuanLiYuan").ToString().Trim();
        }
        return strResult;
    }

    /// <summary>
    /// 上帐，写入库存
    /// </summary>
    private string cancelAccountHandler(string strSendCode)
    {
        string strResult = string.Empty;

        try
        {
            WZSendBLL wZSendBLL = new WZSendBLL();
            string strSendHQL = "from WZSend as wZSend where SendCode = '" + strSendCode + "'";
            IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
            if (listSend != null && listSend.Count == 1)
            {
                WZSend wZSend = (WZSend)listSend[0];

                //<库别>，<物资代码>，<检号>，判断库存中是否存在当前入库物资
                string strStockCode = wZSend.StoreRoom;
                string strObjectCode = wZSend.ObjectCode;
                string strCheckCode = wZSend.CheckCode;

                string strWZStoreHQL = string.Format(@"from WZStore as wZStore
                    where StockCode = '{0}'
                    and ObjectCode = '{1}'
                    and CheckCode = '{2}'", strStockCode, strObjectCode, strCheckCode);
                WZStoreBLL wZStoreBLL = new WZStoreBLL();
                IList lstWZStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                if (lstWZStore != null && lstWZStore.Count == 1)
                {
                    //库存中存在当前物资
                    WZStore wZStore = (WZStore)lstWZStore[0];

                    wZStore.OutNumber -= wZSend.ActualNumber;//出库数量
                    wZStore.OutPrice -= wZSend.TotalMoney;   //出库金额
                    wZStore.StoreNumber -= (wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber);//库存数量
                    wZStore.StoreMoney -= (wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice);    //库存金额
                    //库存单价
                    if (wZStore.StoreNumber != 0)
                    {
                        wZStore.StorePrice = wZStore.StoreMoney / wZStore.StoreNumber;
                    }
                    else
                    {
                        wZStore.StorePrice = 0;
                    }
                    wZStore.EndOutTime = DateTime.Now;//末次入库
                    wZStore.IsMark = 0;                //使用标记

                    //修改库存
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //发料单<收料日期>=系统日期，进度=发料，结算标记=-1
                    wZSend.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZSend.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                    wZSend.IsMark = 0;
                    //修改收料单
                    wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                    //修改计划明细
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + wZSend.PlanDetaiID;
                    IList listPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strPlanDetailHQL);
                    if (listPlanDetail != null && listPlanDetail.Count > 0)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listPlanDetail[0];

                        wZPickingPlanDetail.ShortNumber -= wZSend.ActualNumber;              //缺口数量
                        decimal decimalShortConver = 0;
                        decimal decimalConvertRatio = GetConvertRatioByObjectCode(strObjectCode);   //换算系数
                        if (decimalConvertRatio != 0)
                        {
                            decimalShortConver = wZPickingPlanDetail.ShortNumber / decimalConvertRatio;
                        }
                        wZPickingPlanDetail.ShortConver -= decimalShortConver;    //缺口换算

                        wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);

                    }

                    //工程项目，<收料金额>，<税金>，<运杂费>
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZSend.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZProject.SendMoney -= wZSend.TotalMoney;

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                    }

                    //计划明细<进度> = LanguageHandle.GetWord("FaLiao").ToString().Trim()
                    string strUpdatePlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '开票' where ID = " + wZSend.PlanDetaiID; 
                    ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                    strResult = "Success";
                    return strResult;
                }
                else if (lstWZStore.Count > 1)
                {
                    //库存中存在多个当前物资
                    strResult = LanguageHandle.GetWord("KuCunZhongCunZaiDuoGeKuBieWuZi").ToString().Trim() + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
                }
                else
                {
                    //库存中不存在当前物资
                    strResult = LanguageHandle.GetWord("KuCunZhongBuCunZaiDangQianWuZi").ToString().Trim() + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
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
}
