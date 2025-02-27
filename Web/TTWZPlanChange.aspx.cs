using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Drawing;
using System.Collections;
using System.Data;

public partial class TTWZPlanChange : System.Web.UI.Page
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
            DataGetUnitBinder();

            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strWZPickingPlanHQL = string.Format(@"select pp.*,
                    pm.UserName as PlanMarkerName,
                    pf.UserName as FeeManageName,
                    pe.UserName as PurchaseEngineerName
                    from T_WZPickingPlan pp
                    left join T_ProjectMember pm on pp.PlanMarker = pm.UserCode
                    left join T_ProjectMember pf on pp.FeeManage = pf.UserCode
                    left join T_ProjectMember pe on pp.PurchaseEngineer = pe.UserCode
                    where pp.PlanMarker = '{0}'
                    order by pp.MarkerTime desc", strUserCode);
        DataTable dtPickingPlan = ShareClass.GetDataSetFromSql(strWZPickingPlanHQL, "PickingPlan").Tables[0];

        DG_List.DataSource = dtPickingPlan;
        DG_List.DataBind();

        LB_Sql.Text = strWZPickingPlanHQL;

        //DG_List.CurrentPageIndex = 0;

        //WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        //string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanMarker = '" + strUserCode + "' order by MarkerTime desc";
        //IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);

        //DG_List.DataSource = listWZPickingPlan;
        //DG_List.DataBind();

        //LB_Sql.Text = strWZPickingPlanHQL;
    }


    private void DataGetUnitBinder()
    {
        WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
        string strWZGetUnitHQL = "from WZGetUnit as wZGetUnit";
        IList listWZGetUnit = wZGetUnitBLL.GetAllWZGetUnits(strWZGetUnitHQL);

        DDL_OldPickingUnit.DataSource = listWZGetUnit;
        DDL_OldPickingUnit.DataBind();

        DDL_NewPickingUnit.DataSource = listWZGetUnit;
        DDL_NewPickingUnit.DataBind();
    }

    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            string cmdArges = e.CommandArgument.ToString();

            WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
            string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + cmdArges + "'";
            IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
            if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
            {
                WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];
                if (cmdName == "select")
                {
                    DDL_OldPickingUnit.SelectedValue = wZPickingPlan.UnitCode;

                    DDL_OldPickingUnit.Enabled = false;

                    HF_PlanCode.Value = wZPickingPlan.PlanCode;
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text.Trim();
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strHQL);

        DG_List.DataSource = listWZPickingPlan;
        DG_List.DataBind();
    }

    protected void BT_Replace_Click(object sender, EventArgs e)
    {
        string strPlanCode = HF_PlanCode.Value;
        if (!string.IsNullOrEmpty(strPlanCode))
        {
            if (!string.IsNullOrEmpty(DDL_NewPickingUnit.SelectedValue))
            {
                WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
                IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
                if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
                {
                    WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

                    if (wZPickingPlan.SupplyMethod == LanguageHandle.GetWord("ZiGou").ToString().Trim())
                    {
                        //手动将当前月，该计划下所有〈结算标记〉＝“-1”的发料单退单；
                        string strWZSendHQL = string.Format(@"select s.* from T_WZSend s
                            inner join T_WZPickingPlanDetail pd on s.PlanDetaiID = pd.ID
                            inner join T_WZPickingPlan p on pd.PlanCode = p.PlanCode
                            where p.PlanCode = '{0}'
                            and s.IsMark = -1
                            and SUBSTRING(to_char( s.SendTime, 'yyyy-mm-dd'), 0, 8) = SUBSTRING(to_char( now(), 'yyyy-mm-dd'), 0, 8)
                            ", wZPickingPlan.PlanCode);
                        DataTable dtSend = ShareClass.GetDataSetFromSql(strWZSendHQL, "WZSend").Tables[0];
                        if (dtSend != null && dtSend.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGJHXYFLDJSBJW1DWTD").ToString().Trim()+"')", true);
                            return;
                        }

                        //手动删除该计划下所有〈结算标记〉＝“0”的发料单；
                        string strDelWZSendHQL = string.Format(@"select * from T_WZSend s
                            inner join T_WZPickingPlanDetail pd on s.PlanDetaiID = pd.ID
                            inner join T_WZPickingPlan p on pd.PlanCode = p.PlanCode
                            where p.PlanCode = '{0}'
                            and s.IsMark = 0", wZPickingPlan.PlanCode);
                        DataTable dtDelSend = ShareClass.GetDataSetFromSql(strDelWZSendHQL, "DelWZSend").Tables[0];
                        if (dtDelSend != null && dtDelSend.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGJHXYFLDJSBJW0DWSC").ToString().Trim()+"')", true);
                            return;
                        }

                        //该计划下所有〈结算标记〉＝“-1”的发料单退单；
                        string strAllWZSendHQL = string.Format(@"select * from T_WZSend s
                            inner join T_WZPickingPlanDetail pd on s.PlanDetaiID = pd.ID
                            inner join T_WZPickingPlan p on pd.PlanCode = p.PlanCode
                            where p.PlanCode = '{0}'
                            and s.IsMark = -1", wZPickingPlan.PlanCode);
                        DataTable dtAllSend = ShareClass.GetDataSetFromSql(strAllWZSendHQL, "AllWZSend").Tables[0];
                        if (dtAllSend != null && dtAllSend.Rows.Count > 0)
                        {
                            string strMessage = string.Empty;
                            int intRowCount = 0;
                            string strResult = string.Empty;
                            strResult += LanguageHandle.GetWord("FaLiaoChanHaonbspnbspnbspnbspn").ToString().Trim();
                            foreach (DataRow drAllSend in dtAllSend.Rows)
                            {
                                string strSendCode = ShareClass.ObjectToString(drAllSend["SendCode"]);
                                decimal decimalActualNumber = 0;
                                decimal.TryParse(ShareClass.ObjectToString(drAllSend["ActualNumber"]), out decimalActualNumber);

                                decimal decimalPlanMoney = 0;
                                decimal.TryParse(ShareClass.ObjectToString(drAllSend["PlanMoney"]), out decimalPlanMoney);

                                intRowCount++;

                                strResult += strSendCode + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + decimalActualNumber + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + decimalPlanMoney + "<br />";
                            }
                            strMessage = LanguageHandle.GetWord("GaiJiHuaXiaYiShengXiaoDeFaLiao").ToString().Trim() + intRowCount + LanguageHandle.GetWord("Tiaobr").ToString().Trim();
                            strMessage += strResult;

                            HF_Message.Value = strMessage;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertChange();", true);
                            return;
                        }
                    }
                    else if (wZPickingPlan.SupplyMethod == LanguageHandle.GetWord("JiaGong").ToString().Trim())
                    {
                        //领料计划〈计划编号〉＝移交明细〈计划编号〉												
                        //移交明细〈凭证标记〉＝“-1”												
                        string strTurnDetailHQL = string.Format(@"select * from T_WZTurnDetail 
                                where PlanCode = '{0}'
                                and CardIsMark = -1", wZPickingPlan.PlanCode);
                        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strTurnDetailHQL, "TurnDetail").Tables[0];
                        if (dtTurnDetail != null && dtTurnDetail.Rows.Count > 0)
                        {
                            string strMessage = string.Empty;
                            int intRowCount = 0;
                            string strResult = string.Empty;
                            strResult += LanguageHandle.GetWord("YiJiaoChanHaonbspnbspnbspnbspS").ToString().Trim();
                            foreach (DataRow drTurnDetail in dtTurnDetail.Rows)
                            {
                                string strTurnCode = ShareClass.ObjectToString(drTurnDetail["TurnCode"]);
                                decimal decimalActualNumber = 0;
                                decimal.TryParse(ShareClass.ObjectToString(drTurnDetail["ActualNumber"]), out decimalActualNumber);
                                decimal decimalTicketMoney = 0;
                                decimal.TryParse(ShareClass.ObjectToString(drTurnDetail["TicketMoney"]), out decimalTicketMoney);
                                decimal decimalPlanMoney = 0;
                                decimal.TryParse(ShareClass.ObjectToString(drTurnDetail["PlanMoney"]), out decimalPlanMoney);
                                intRowCount++;
                                strResult += strTurnCode + "&nbsp;&nbsp;&nbsp;&nbsp;" + decimalActualNumber + "&nbsp;&nbsp;&nbsp;&nbsp;" + decimalTicketMoney + "&nbsp;&nbsp;&nbsp;&nbsp; " + decimalPlanMoney + "<br />";
                            }
                            strMessage = LanguageHandle.GetWord("GaiJiHuaXiaYiTanRuChengBenDeJi").ToString().Trim() + intRowCount + LanguageHandle.GetWord("Tiaobr").ToString().Trim();
                            strMessage += strResult;

                            HF_TurnMessage.Value = strMessage;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertTurnChange();", true);
                            return;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZXDLLDW").ToString().Trim()+"')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZLLJH").ToString().Trim()+"')", true);
            return;
        }
    }

    protected void BT_Continue_Click(object sender, EventArgs e)
    {
        //写记录：												
        // 领料计划〈单位编号〉＝拟变更的〈单位编号〉												
        // 领料计划〈领料单位〉＝拟变更的〈领料单位〉												
        // 发料单〈单位编号〉＝领料计划〈单位编号〉												
        // 发料单〈领料单位〉＝领料计划〈领料单位〉												

        string strPlanCode = HF_PlanCode.Value;
        if (!string.IsNullOrEmpty(strPlanCode))
        {
            WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
            string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
            IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
            if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
            {
                WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

                wZPickingPlan.UnitCode = DDL_NewPickingUnit.SelectedValue;
                wZPickingPlan.PickingUnit = DDL_NewPickingUnit.SelectedItem.Text;

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, wZPickingPlan.PlanCode);

                //该计划下所有〈结算标记〉＝“-1”的发料单退单；
                string strAllWZSendHQL = string.Format(@"select s.* from T_WZSend s
                            inner join T_WZPickingPlanDetail pd on s.PlanDetaiID = pd.ID
                            inner join T_WZPickingPlan p on pd.PlanCode = p.PlanCode
                            where p.PlanCode = '{0}'
                            and s.IsMark = -1", wZPickingPlan.PlanCode);
                DataTable dtAllSend = ShareClass.GetDataSetFromSql(strAllWZSendHQL, "AllWZSend").Tables[0];
                if (dtAllSend != null && dtAllSend.Rows.Count > 0)
                {
                    foreach (DataRow drSend in dtAllSend.Rows)
                    {
                        string strUpdateSendHQL = string.Format(@"update T_WZSend 
                                    set UnitCode = '{0}',
                                    PickingUnit = '{1}'
                                    where SendCode = '{2}'", DDL_NewPickingUnit.SelectedValue, DDL_NewPickingUnit.SelectedItem.Text,
                                                           ShareClass.ObjectToString(drSend["SendCode"]));
                        ShareClass.RunSqlCommand(strUpdateSendHQL);
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"')", true);
                }
            }
        }
    }

    protected void BT_TurnMessage_Click(object sender, EventArgs e)
    {
         //写记录：												
         // 领料计划〈单位编号〉＝拟变更的〈单位编号〉												
         // 领料计划〈领料单位〉＝拟变更的〈领料单位〉												
         // 移交单〈单位编号〉＝领料计划〈单位编号〉												
         // 移交单〈领料单位〉＝领料计划〈领料单位〉												
         // 移交明细〈领料单位〉＝领料计划〈领料单位〉												
        string strPlanCode = HF_PlanCode.Value;
        if (!string.IsNullOrEmpty(strPlanCode))
        {
            WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
            string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
            IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
            if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
            {
                WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

                wZPickingPlan.UnitCode = DDL_NewPickingUnit.SelectedValue;
                wZPickingPlan.PickingUnit = DDL_NewPickingUnit.SelectedItem.Text;

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, wZPickingPlan.PlanCode);

                //移交单
                string strTurnHQL = string.Format(@"select t.* from T_WZTurnDetail  d
                                left join T_WZTurn t on d.TurnCode = t.TurnCode
                                where d.PlanCode = '{0}'
                                and d.CardIsMark = -1", wZPickingPlan.PlanCode);
                DataTable dtTurn = ShareClass.GetDataSetFromSql(strTurnHQL, "Turn").Tables[0];
                if (dtTurn != null && dtTurn.Rows.Count > 0)
                { 
                    foreach(DataRow drTurn in dtTurn.Rows)
                    {

                        string strUpdateTurnHQL = string.Format(@"update T_WZTurn 
                                    set UnitCode = '{0}',
                                    PickingUnit = '{1}'
                                    where TurnCode = '{2}'", DDL_NewPickingUnit.SelectedValue, DDL_NewPickingUnit.SelectedItem.Text,
                                                           ShareClass.ObjectToString(drTurn["TurnCode"]));
                        ShareClass.RunSqlCommand(strUpdateTurnHQL);
                    }
                }

                //移交明细〈凭证标记〉＝“-1”												
                string strTurnDetailHQL = string.Format(@"select * from T_WZTurnDetail 
                                where PlanCode = '{0}'
                                and CardIsMark = -1", wZPickingPlan.PlanCode);
                DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strTurnDetailHQL, "TurnDetail").Tables[0];
                if (dtTurnDetail != null && dtTurnDetail.Rows.Count > 0)
                {
                    foreach (DataRow drTurnDetail in dtTurnDetail.Rows)
                    {
                        string strUpdateTurnDetailHQL = string.Format(@"update T_WZTurnDetail
                                    set PickingUnit = '{0}'
                                    where ID = {1}", DDL_NewPickingUnit.SelectedItem.Text, ShareClass.ObjectToString(drTurnDetail["ID"]));
                        ShareClass.RunSqlCommand(strUpdateTurnDetailHQL);
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTHCG").ToString().Trim()+"')", true);
                }
            }
        }
    }
}