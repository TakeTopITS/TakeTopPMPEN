using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Data;

public partial class TTWZTurnEdit : System.Web.UI.Page
{
    public string strTurnCode
    {
        get;
        set;
    }
    public string strUserCode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        if (!string.IsNullOrEmpty(Request.QueryString["turnCode"]))
        {
            strTurnCode = Request.QueryString["turnCode"].ToString();
        }
        else
        {
            strTurnCode = "";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataPickingPlanBander();
            DataTurnDetailBinder(strTurnCode);
            DataPickingPlanDetailBinder("");
        }
    }


    private void DataTurnDetailBinder(string strTurnCode)
    {
        string strWZTurnDetailHQL = string.Format(@"select d.*,o.ObjectName,m.UserName as MaterialPersonName from T_WZTurnDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                            left join T_ProjectMember m on d.MaterialPerson = m.UserCode
                            where d.TurnCode= '{0}'", strTurnCode);
        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strWZTurnDetailHQL, "WZTurnDetail").Tables[0];

        DG_TurnDetail.DataSource = dtTurnDetail;
        DG_TurnDetail.DataBind();

        LB_TurnDetailHQL.Text = strWZTurnDetailHQL;

        string strProgress;
        for (int i = 0; i < dtTurnDetail.Rows.Count; i++)
        {
            strProgress = dtTurnDetail.Rows[i]["Progress"].ToString();
            if (strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim())
            {
                DG_TurnDetail.Items[i].Cells[16].Text = "";
            }

            if(GetWZTurnProgress(dtTurnDetail.Rows[i]["TurnCode"].ToString()) == LanguageHandle.GetWord("QianShou").ToString().Trim())
            {
                ((LinkButton)(DG_TurnDetail.Items[i].FindControl("LinkButton1"))).Visible = false;
            }
        }
    }

    protected void DG_TurnDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();             //移交单明细ID
                WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where ID= " + cmdArges;
                IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
                {
                    WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];
                    wZTurnDetailBLL.DeleteWZTurnDetail(wZTurnDetail);

                    //修改 计划明细 <进度>=录入 ，计划明细<移交单号> = '-'
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strWZPickingPlanDetailHQL = string.Format(@"from WZPickingPlanDetail as wZPickingPlanDetail where ID = {0}", wZTurnDetail.PlanCode);
                    IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                    if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
                        wZPickingPlanDetail.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        wZPickingPlanDetail.TurnCode = "-";
                        wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, int.Parse(wZTurnDetail.PlanCode));
                    }

                    //修改移交单里面的条数
                    string strUpdateTurnNumberHQL = "update T_WZTurn set RowNumber = RowNumber -1 where TurnCode = '" + wZTurnDetail.TurnCode + "'";
                    ShareClass.RunSqlCommand(strUpdateTurnNumberHQL);

                    string strHQL;
                    strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'";
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
                    if(ds.Tables [0].Rows.Count == 0)
                    {
                        strHQL = "Update T_WZTurn Set IsMark = 0 Where TurnCode = " + "'" + strTurnCode + "'";
                        ShareClass.RunSqlCommand(strHQL);
                    }


                    //重新加载移交单明细列表
                    DataTurnDetailBinder(wZTurnDetail.TurnCode);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDYJDMXBCZ").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
    }

    protected string GetWZTurnProgress(string strTurnCode)
    {
        string strHQL = "Select Progress From T_WZTurn  where TurnCode = '" + strTurnCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurn");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    protected void DG_PickPlanDetailList_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();                     //ID|PlanCode
                string[] arrCmdArges = cmdArges.Split('|');

                //string strTurnCode = LB_Turn.SelectedValue;
                if (!string.IsNullOrEmpty(strTurnCode))
                {
                    string strCheckWZTurnDetailHQL = string.Format(@"select ID as RowNumber from T_WZTurnDetail 
                        where TurnCode = '{0}'
                        and PlanCode = {1}", strTurnCode, arrCmdArges[0]);
                    DataTable dtCheckTurnDetail = ShareClass.GetDataSetFromSql(strCheckWZTurnDetailHQL, "TurnDetail").Tables[0];
                    if (dtCheckTurnDetail != null && dtCheckTurnDetail.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDJHMXYJZYJDMXZBNZF").ToString().Trim() + "')", true);
                        return;
                    }

                    WZTurnBLL wZTurnBLL = new WZTurnBLL();
                    string strWZTurnHQL = "from WZTurn as wZTurn where TurnCode = '" + strTurnCode + "'";
                    IList listWZTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
                    if (listWZTurn != null && listWZTurn.Count > 0)
                    {
                        WZTurn wZTurn = (WZTurn)listWZTurn[0];

                        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                        string strWZPickingPlanHQL = string.Format(@"from WZPickingPlan as wZPickingPlan where PlanCode = '{0}'", arrCmdArges[1]);
                        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
                        if (listWZPickingPlan != null && listWZPickingPlan.Count > 0)
                        {
                            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];
                            ////领料计划<进度> = “签收”，领料计划〈项目编码〉＝移交单〈项目编码〉， 领料计划〈单位编号〉＝移交单〈单位编号〉，领料计划〈采购工程师〉＝移交单〈采购工程师〉，领料计划〈供应方式〉＝“甲供”
                            //if (wZPickingPlan.Progress == LanguageHandle.GetWord("QianShou").ToString().Trim()
                            //    && wZPickingPlan.ProjectCode == wZTurn.ProjectCode
                            //    && wZPickingPlan.UnitCode == wZTurn.UnitCode
                            //    && wZPickingPlan.PurchaseEngineer.Trim() == wZTurn.PurchaseEngineer.Trim()
                            //    && wZPickingPlan.SupplyMethod == LanguageHandle.GetWord("JiaGong").ToString().Trim())
                            //{
                            if ( wZPickingPlan.ProjectCode == wZTurn.ProjectCode
                               && wZPickingPlan.UnitCode == wZTurn.UnitCode
                               && wZPickingPlan.PurchaseEngineer.Trim() == wZTurn.PurchaseEngineer.Trim()
                               && wZPickingPlan.SupplyMethod == LanguageHandle.GetWord("JiaGong").ToString().Trim())
                            {
                                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                                string strWZPickingPlanDetailHQL = string.Format(@"from WZPickingPlanDetail as wZPickingPlanDetail where ID = {0}", arrCmdArges[0]);
                                IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                                if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                                {
                                    WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];

                                    WZTurnDetail wZTurnDetail = new WZTurnDetail();
                                    wZTurnDetail.TurnCode = wZTurn.TurnCode;
                                    wZTurnDetail.ProjectCode = wZTurn.ProjectCode;
                                    wZTurnDetail.PickingUnit = wZTurn.PickingUnit;
                                    wZTurnDetail.StoreRoom = wZTurn.StoreRoom;
                                    wZTurnDetail.MaterialPerson = wZTurn.MaterialPerson;
                                    wZTurnDetail.PickingCode = wZPickingPlan.PlanCode;
                                    wZTurnDetail.TicketTime = DateTime.Now;
                                    wZTurnDetail.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                                    wZTurnDetail.IsMark = 0;
                                    wZTurnDetail.PlanCode = wZPickingPlanDetail.ID.ToString();
                                    wZTurnDetail.ObjectCode = wZPickingPlanDetail.ObjectCode;
                                    wZTurnDetail.TicketNumber = wZPickingPlanDetail.ShortNumber;
                                    wZTurnDetail.ActualNumber = wZPickingPlanDetail.ShortNumber;
                                    wZTurnDetail.PickingTime = DateTime.Now;
                                    WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                                    wZTurnDetailBLL.AddWZTurnDetail(wZTurnDetail);

                                    //查询移交单明细最大ID
                                    string strMaxTurnDetailIDHQL = "select COALESCE(max(id),0) as ID from T_WZTurnDetail";
                                    DataTable dtMaxTurnDetailID = ShareClass.GetDataSetFromSql(strMaxTurnDetailIDHQL, "MaxTurnDetailID").Tables[0];
                                    int intMaxTurnDetailID = int.Parse(dtMaxTurnDetailID.Rows[0]["ID"].ToString());

                                    //修改计划明细的进度和移交单编号
                                    wZPickingPlanDetail.Progress = LanguageHandle.GetWord("YiJiao").ToString().Trim();
                                    wZPickingPlanDetail.TurnCode = intMaxTurnDetailID + "";
                                    wZPickingPlanDetail.IsMark = -1;
                                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);

                                    //修改移交单里面的条数，使用标记
                                    string strUpdateTurnNumberHQL = "update T_WZTurn set RowNumber = RowNumber +1,IsMark=-1 where TurnCode = '" + wZTurn.TurnCode + "'";
                                    ShareClass.RunSqlCommand(strUpdateTurnNumberHQL);

                                    //重新加载移交单明细
                                    DataTurnDetailBinder(strTurnCode);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSDJHMXJDYWSLLFSWJGXMBMDWBHCGGCSYYDDYJDXXXT").ToString().Trim() + "')", true);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYJDH").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
    }


    private void DataPickingPlanBander()
    {
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        //string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where SupplyMethod = 'Party A Supply' and Progress = 'Sign for Receipt' order by MarkerTime desc";
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where SupplyMethod = 'Party A Supply'  order by MarkerTime desc";   //ChineseWord
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);

        LB_PickingPlan.DataSource = listWZPickingPlan;
        LB_PickingPlan.DataBind();
    }

    private void DataPickingPlanDetailBinder(string strPlanCode)
    {
        string strWZPickingPlanDetailHQL = string.Format(@"select d.*,o.ObjectName from T_WZPickingPlanDetail d
                        left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                        where d.PlanCode = '{0}'", strPlanCode);
        DataTable dtPickingPlanDetail = ShareClass.GetDataSetFromSql(strWZPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];

        DG_PickPlanDetailList.DataSource = dtPickingPlanDetail;
        DG_PickPlanDetailList.DataBind();
    }

    protected void LB_PickingPlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPickPlanID = LB_PickingPlan.SelectedValue;
        if (!string.IsNullOrEmpty(strPickPlanID))
        {
            DataPickingPlanDetailBinder(strPickPlanID);
        }
    }


    protected void DG_TurnDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_TurnDetail.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_TurnDetailHQL.Text.Trim(); ;
        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strHQL, "WZTurnDetail").Tables[0];

        DG_TurnDetail.DataSource = dtTurnDetail;
        DG_TurnDetail.DataBind();

        //DG_TurnDetail.CurrentPageIndex = e.NewPageIndex;
        //string strHQL = LB_TurnDetailHQL.Text.Trim(); ;
        //WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
        //IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strHQL);

        //DG_TurnDetail.DataSource = listWZTurnDetail;
        //DG_TurnDetail.DataBind();
    }
    
}
