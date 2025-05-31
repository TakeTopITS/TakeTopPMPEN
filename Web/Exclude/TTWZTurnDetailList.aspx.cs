using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZTurnDetailList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataTurnBander();
            DataPickingPlanBander();
            DataTurnDetailBinder("");
            DataPickingPlanDetailBinder("");
        }
    }


    private void DataTurnBander()
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        WZTurnBLL wZTurnBLL = new WZTurnBLL();
        string strWZTurnHQL = "from WZTurn as wZTurn where PurchaseEngineer = '" + strUserCode + "' order by TurnTime desc";
        IList listWZTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);

        LB_Turn.DataSource = listWZTurn;
        LB_Turn.DataBind();
    }

    private void DataTurnDetailBinder(string turnCode)
    {
        string strWZTurnDetailHQL = string.Format(@"select t.*,p.PlanCode as PickingPlanCode,m.UserName as MaterialPersonName from T_WZTurnDetail t
                                left join T_WZPickingPlanDetail p on t.PlanCode = p.ID 
                                left join T_ProjectMember m on t.MaterialPerson = m.UserCode
                                where t.TurnCode= '{0}'", turnCode);
        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strWZTurnDetailHQL, "TurnDetail").Tables[0];

        DG_TurnDetail.DataSource = dtTurnDetail;
        DG_TurnDetail.DataBind();

        //WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
        //string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where TurnCode= '" + turnCode + "'";
        //IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
        //DG_TurnDetail.DataSource = listWZTurnDetail;
        //DG_TurnDetail.DataBind();
    }



    protected void DG_TurnDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();             //�ƽ�����ϸID
                WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where ID= " + cmdArges;
                IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
                {
                    WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];
                    wZTurnDetailBLL.DeleteWZTurnDetail(wZTurnDetail);

                    //�޸� �ƻ���ϸ <����>=¼�� ���ƻ���ϸ<�ƽ�����> = '-'
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strWZPickingPlanDetailHQL = string.Format(@"from WZPickingPlanDetail as wZPickingPlanDetail where ID = {0}", wZTurnDetail.PlanCode);
                    IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                    if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
                        wZPickingPlanDetail.Progress = "¼��";
                        wZPickingPlanDetail.TurnCode = "-";
                        wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, int.Parse(wZTurnDetail.PlanCode));
                    }

                    //�޸��ƽ������������
                    string strUpdateTurnNumberHQL = "update T_WZTurn set RowNumber = RowNumber -1 where TurnCode = '" + wZTurnDetail.TurnCode + "'";
                    ShareClass.RunSqlCommand(strUpdateTurnNumberHQL);

                    //���¼����ƽ�����ϸ�б�
                    DataTurnDetailBinder(wZTurnDetail.TurnCode);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDYJDMXBCZ+"')", true);
                    return;
                }
            }
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

                string strTurnCode = LB_Turn.SelectedValue;
                if (!string.IsNullOrEmpty(strTurnCode))
                {
                    WZTurnBLL wZTurnBLL = new WZTurnBLL();
                    string strWZTurnHQL = "from WZTurn as wZTurn where TurnCode = '" + strTurnCode + "'";
                    IList listWZTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
                    if (listWZTurn != null && listWZTurn.Count > 0)
                    {
                        WZTurn wZTurn = (WZTurn)listWZTurn[0];

                        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                        string strWZPickingPlanHQL = string.Format(@"from WZPickingPlan as wZPickingPlan where PlanCode = {0}", arrCmdArges[1]);
                        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
                        if (listWZPickingPlan != null && listWZPickingPlan.Count > 0)
                        {
                            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];
                            //���ϼƻ�<����> = ��ǩ�ա������ϼƻ�����Ŀ���롵���ƽ�������Ŀ���롵�� ���ϼƻ�����λ��š����ƽ�������λ��š������ϼƻ����ɹ�����ʦ�����ƽ������ɹ�����ʦ�������ϼƻ�����Ӧ��ʽ�������׹���
                            if (wZPickingPlan.Progress == "ǩ��"
                                && wZPickingPlan.ProjectCode == wZTurn.ProjectCode
                                && wZPickingPlan.UnitCode == wZTurn.UnitCode
                                && wZPickingPlan.PurchaseEngineer == wZTurn.PurchaseEngineer
                                && wZPickingPlan.SupplyMethod == "�׹�")
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
                                    wZTurnDetail.Progress = "¼��";
                                    wZTurnDetail.IsMark = 0;
                                    wZTurnDetail.PlanCode = wZPickingPlanDetail.ID.ToString();
                                    wZTurnDetail.ObjectCode = wZPickingPlanDetail.ObjectCode;
                                    wZTurnDetail.TicketNumber = wZPickingPlanDetail.ShortNumber;
                                    wZTurnDetail.ActualNumber = wZTurnDetail.TicketNumber;
                                    wZTurnDetail.PickingTime = DateTime.Now;
                                    WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                                    wZTurnDetailBLL.AddWZTurnDetail(wZTurnDetail);

                                    //��ѯ�ƽ�����ϸ���ID
                                    string strMaxTurnDetailIDHQL = "select COALESCE(max(id),0) as ID from T_WZTurnDetail";
                                    DataTable dtMaxTurnDetailID = ShareClass.GetDataSetFromSql(strMaxTurnDetailIDHQL, "MaxTurnDetailID").Tables[0];
                                    int intMaxTurnDetailID = int.Parse(dtMaxTurnDetailID.Rows[0]["ID"].ToString());

                                    //�޸ļƻ���ϸ�Ľ��Ⱥ��ƽ������
                                    wZPickingPlanDetail.Progress = "�ƽ�";
                                    wZPickingPlanDetail.TurnCode = intMaxTurnDetailID + "";
                                    wZPickingPlanDetail.IsMark = -1;
                                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);

                                    //�޸��ƽ��������������ʹ�ñ��
                                    string strUpdateTurnNumberHQL = "update T_WZTurn set RowNumber = RowNumber +1,IsMark=-1 where TurnCode = '" + wZTurn.TurnCode + "'";
                                    ShareClass.RunSqlCommand(strUpdateTurnNumberHQL);

                                    //���¼����ƽ�����ϸ
                                    DataTurnDetailBinder(strTurnCode);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSDJHMXJDYWSLLFSWJGXMBMDWBHCGGCSYYDDYJDXXXT+"')", true);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZYJDH+"')", true);
                    return;
                }
            }
        }
    }


    private void DataPickingPlanBander()
    {
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan order by MarkerTime desc";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);

        LB_PickingPlan.DataSource = listWZPickingPlan;
        LB_PickingPlan.DataBind();
    }

    private void DataPickingPlanDetailBinder(string strPlanCode)
    {
        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
        string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + strPlanCode +"'";
        IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);

        DG_PickPlanDetailList.DataSource = listWZPickingPlanDetail;
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

    protected void LB_Turn_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strTurnCode = LB_Turn.SelectedValue;
        if (!string.IsNullOrEmpty(strTurnCode))
        {
            DataTurnDetailBinder(strTurnCode);
        }
    }
}