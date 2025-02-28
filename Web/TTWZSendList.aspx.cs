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

public partial class TTWZSendList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "���ϵ�", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            LoadCarListByAuthority();

            BindStockData();

            DataSendBinder();
            DataWZPickingPlanBander();

            DG_PickingPlanDetail.DataSource = "";
            DG_PickingPlanDetail.DataBind();

            DG_Store.DataSource = "";
            DG_Store.DataBind();
        }
    }

    private void DataSendBinder()
    {
        //DG_Send.CurrentPageIndex = 0;

        string strSendHQL = string.Format(@"select s.*,d.PlanCode,o.ObjectName,
                        c.UserName as CheckerName,
                        f.UserName as SafekeeperName,
                        k.UserName as UpLeaderName,
                        p.UserName as PurchaseEngineerName
                        from T_WZSend s
                        left join T_WZPickingPlanDetail d on s.PlanDetaiID = d.ID
                        left join T_WZObject o on s.ObjectCode = o.ObjectCode
                        left join T_ProjectMember c on s.Checker = c.UserCode
                        left join T_ProjectMember f on s.Safekeeper = f.UserCode
                        left join T_ProjectMember k on s.UpLeader = k.UserCode
                        left join T_ProjectMember p on s.PurchaseEngineer = p.UserCode
                        where s.PurchaseEngineer ='{0}' 
                        and s.Progress in ('¼��','�ļ�','��Ʊ') 
                        order by s.TicketTime desc", strUserCode);   //ChineseWord
        DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];

        DG_Send.DataSource = dtSend;
        DG_Send.DataBind();

        LB_SendSql.Text = strSendHQL;
        #region ע��
        //DG_Send.CurrentPageIndex = 0;

        //WZSendBLL wZSendBLL = new WZSendBLL();
        //string strSendHQL = string.Format("from WZSend as wZSend where PurchaseEngineer ='{0}' and Progress = '¼��' order by TicketTime desc", strUserCode);
        //IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);

        //DG_Send.DataSource = listSend;
        //DG_Send.DataBind();

        //LB_SendSql.Text = strSendHQL;
        #endregion
    }

    private void DataWZPickingPlanBander()
    {
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = string.Format(@"from WZPickingPlan as wZPickingPlan 
                                                    where PurchaseEngineer = '{0}' 
                                                    and Progress = 'Sign for Receipt' 
                                                    and SupplyMethod='Self-purchase' 
                                                    order by MarkerTime desc", strUserCode);   //ChineseWord
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);

        LB_Plan.DataSource = listWZPickingPlan;
        LB_Plan.DataBind();
    }

    private void DataPlanDetailBinder()
    {
        string strPlanCode = LB_Plan.SelectedValue;
        if (!string.IsNullOrEmpty(strPlanCode))
        {
            string strWZPickingPlanDetailHQL = string.Format(@"select d.*,o.ObjectName from T_WZPickingPlanDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                            where d.PlanCode = '{0}'", strPlanCode);
            DataTable dtPlanDetail = ShareClass.GetDataSetFromSql(strWZPickingPlanDetailHQL, "PlanDetail").Tables[0];

            DG_PickingPlanDetail.DataSource = dtPlanDetail;
            DG_PickingPlanDetail.DataBind();
        }
    }


    private string  GetCheckCode(string strObjectCode)
    {
        string strStoreHQL = string.Format(@"select s.CheckCode from T_WZStore s
            left join T_WZObject o on s.ObjectCode = o.ObjectCode
            where s.ObjectCode = '{0}'", strObjectCode);
        DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];

        if(dtStore .Rows .Count > 0)
        {
            return dtStore.Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
         
    }

    private void DataStoreBinder(string strObjectCode)
    {
        string strStoreHQL = string.Format(@"select s.*,
            o.ObjectName,o.Model,o.Criterion,o.Grade,o.DLCode,o.ZLCode,o.XLCode from T_WZStore s
            left join T_WZObject o on s.ObjectCode = o.ObjectCode
            where s.ObjectCode = '{0}'", strObjectCode);
        DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];

        DG_Store.DataSource = dtStore;
        DG_Store.DataBind();
    }

    private void BindStockData()
    {
        WZStockBLL wZStockBLL = new WZStockBLL();
        string strStockHQL = "from WZStock as wZStock";
        IList lstStock = wZStockBLL.GetAllWZStocks(strStockHQL);

        DDL_StoreRoom.DataSource = lstStock;
        DDL_StoreRoom.DataBind();

        DDL_StoreRoom.Items.Insert(0, new ListItem("-", ""));
    }

    protected void DG_Send_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                //����
                for (int i = 0; i < DG_Send.Items.Count; i++)
                {
                    DG_Send.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditSendCode = arrOperate[0];
                string strProgress = arrOperate[1];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);

                HF_NewSendCode.Value = strEditSendCode;
                HF_NewProgress.Value = strProgress;

            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZSend.PurchaseEngineer != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRYJHTYBWDDLYHSBYXSC").ToString().Trim() + "')", true);
                        return;
                    }

                    wZSendBLL.DeleteWZSend(wZSend);

                    //���¼����б�
                    DG_Send.CurrentPageIndex = 0;
                    DataSendBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Send.Items.Count; i++)
                {
                    DG_Send.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];

                    HF_SendCode.Value = wZSend.SendCode;
                    TXT_SendCode.Text = wZSend.SendCode;

                    TXT_ObjectCode.Text = wZSend.ObjectCode;
                    TXT_TicketTime.Text = wZSend.TicketTime.ToString("yyyy-MM-dd");
                    //DDL_StoreRoom.Items.Add(new ListItem(wZSend.StoreRoom, wZSend.StoreRoom));
                    DDL_StoreRoom.SelectedValue = wZSend.StoreRoom;

                    DDL_SendMethod.SelectedValue = wZSend.SendMethod;    //���Ϸ�ʽ
                    TXT_ActualNumber.Text = wZSend.ActualNumber.ToString();  //ʵ������
                    TXT_PlanPrice.Text = wZSend.PlanPrice.ToString();        //�ƻ�����
                    TXT_PlanMoney.Text = wZSend.PlanMoney.ToString();        //�ƻ����
                    TXT_DownMoney.Text = wZSend.DownMoney.ToString();        //��ֵ���
                    TXT_CleanMoney.Text = wZSend.CleanMoney.ToString();      //����
                    TXT_ReduceCode.Text = wZSend.ReduceCode;                   //��ֵ���
                    TXT_WearyCode.Text = wZSend.WearyCode;                     //��ѹ���
                    TXT_CheckCode.Text = wZSend.CheckCode;                 //���
                    TXT_GoodsCode.Text = wZSend.GoodsCode;                   //��λ��
                    TXT_ManageRate.Text = wZSend.ManageRate.ToString();      //�������

                    if (wZSend.SendMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
                    {
                        TXT_ActualNumber.BackColor = Color.Red;
                    }
                    else
                    {
                        TXT_ActualNumber.BackColor = Color.White;
                    }

                    DL_CarCode.SelectedValue = wZSend.CarCode;
                    TXT_Comment.Text = wZSend.Comment;

                    //���ؿ���б�
                    DataStoreBinder(wZSend.ObjectCode);
                }
            }
            else if (cmdName == "submit")
            {
                //�ύ
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress == LanguageHandle.GetWord("LuRu").ToString().Trim())
                    {
                        string strCheckCode = wZSend.CheckCode;
                        if (!string.IsNullOrEmpty(strCheckCode))
                        {
                            //�ļ�
                            wZSend.Progress = LanguageHandle.GetWord("CaiJian").ToString().Trim();
                        }
                        else
                        {
                            //��Ʊ
                            wZSend.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                        }

                        wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                        //���¼������ϵ��б�
                        DataSendBinder();

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
                //�ύ
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress == LanguageHandle.GetWord("KaiPiao").ToString().Trim())
                    {
                        string strCheckCode = wZSend.CheckCode;
                        //if (!string.IsNullOrEmpty(strCheckCode))
                        //{
                        //�ļ�
                        wZSend.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        //}
                        //else
                        //{
                        //    //��Ʊ
                        //    wZSend.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                        //}

                        wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                        //���¼������ϵ��б�
                        DataSendBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTHCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRBNTJ").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "notSubmit2")
            {
                //�ύ
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
                    {
                        string strCheckCode = wZSend.CheckCode;
                        //if (!string.IsNullOrEmpty(strCheckCode))
                        //{
                        //�ļ�
                        wZSend.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        //}
                        //else
                        //{
                        //    //��Ʊ
                        //    wZSend.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                        //}

                        wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                        //���¼������ϵ��б�
                        DataSendBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTHCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRBNTJ").ToString().Trim() + "')", true);
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

    protected void LB_Plan_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_Plan.SelectedValue))
        {
            DataPlanDetailBinder();
        }
    }

    protected void DG_PickingPlanDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "add")
        {
            string cmdArges = e.CommandArgument.ToString();

            WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
            string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + cmdArges;
            IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
            if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
            {
                WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];

                WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                string strWZPickingPlanHQL = string.Format("from WZPickingPlan as wZPickingPlan where PlanCode = '{0}'", wZPickingPlanDetail.PlanCode);
                IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
                if (listWZPickingPlan != null && listWZPickingPlan.Count > 0)
                {
                    WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

                    //���ӷ��ϵ�
                    WZSend wZSend = new WZSend();
                    wZSend.SendCode = CreateNewSendCode();          //���ϱ��
                    wZSend.TicketTime = DateTime.Now;
                    wZSend.PurchaseEngineer = strUserCode;
                    wZSend.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                    wZSend.IsMark = 0;
                    wZSend.SendMethod = LanguageHandle.GetWord("LanPiao").ToString().Trim();

                    //���ϵ�����Ŀ���롵����𡵡���λ��š������ϵ�λ�������ϼƻ�����Ŀ���롵����𡵡���λ��š������ϵ�λ��
                    wZSend.ProjectCode = wZPickingPlan.ProjectCode;
                    wZSend.StoreRoom = wZPickingPlan.StoreRoom;
                    wZSend.UnitCode = wZPickingPlan.UnitCode;
                    wZSend.PickingUnit = wZPickingPlan.PickingUnit;
                    wZSend.UpLeader = GetPickUnitLeaderCode(wZPickingPlan.UnitCode);             

                    //���ϵ����ļ�Ա��������Ա�������ݡ���Ŀ���롵�ӹ�����Ŀ������
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZPickingPlan.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZSend.Checker = wZProject.Checker;
                        wZSend.Safekeeper = wZProject.Safekeep;
                        wZSend.UpLeader = wZProject.Leader;
                    }

                    //���ϵ��������쵼�������ݡ���λ��š������ϵ�λ������
                    WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
                    string strWZGetUnitSql = "from WZGetUnit as wZGetUnit where UnitCode = '" + wZPickingPlan.UnitCode + "'";
                    IList unitList = wZGetUnitBLL.GetAllWZGetUnits(strWZGetUnitSql);
                    if (unitList != null && unitList.Count > 0)
                    {
                        WZGetUnit wZGetUnit = (WZGetUnit)unitList[0];

                        wZSend.UpLeader = wZGetUnit.Leader;
                    }
                    //���ϵ����ƻ���š������ʴ��롵���ƻ���ϸ���ƻ���š������ʴ��롵
                    wZSend.PlanDetaiID = wZPickingPlanDetail.ID;
                    wZSend.ObjectCode = wZPickingPlanDetail.ObjectCode;
                    wZSend.CheckCode = GetCheckCode(wZPickingPlanDetail.ObjectCode.Trim());

                    //��;���ϵ����ѹ�������
                    decimal decimalCurrentNumber = 0;
                    string strSendHQL = string.Format(@"select ID,SUM(ReceivedNumber) as SumReceivedNumber from T_WZPickingPlanDetail
                                   where ID={0} group by ID", wZPickingPlanDetail.ID);
                    DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];
                    if (dtSend != null && dtSend.Rows.Count > 0)
                    {
                        decimal.TryParse(ShareClass.ObjectToString(dtSend.Rows[0]["SumReceivedNumber"]), out decimalCurrentNumber);
                    }
                    //���ϵ����ƻ����������ƻ���ϸ��ȱ������������;���ϵ����ѹ�������
                    wZSend.PlanNumber = wZPickingPlanDetail.ShortNumber - decimalCurrentNumber;
                    //���ϵ���ʵ�������������ϵ����ƻ�������
                    wZSend.ActualNumber = wZSend.PlanNumber;
                    //���ϵ����ļ����ڡ����������ڡ�����-��
                    wZSend.CheckTime = "-";//DateTime.Now;
                    wZSend.SendTime = "-"; //DateTime.Now;
                    wZSend.CarCode = "-";
                    wZSend.Comment = "-";

                    WZSendBLL wZSendBLL = new WZSendBLL();
                    wZSendBLL.AddWZSend(wZSend);

                    //�޸ļƻ���ϸ<ʹ�ñ��> = -1
                    wZPickingPlanDetail.IsMark = -1;
                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);

                    //���¼��ط��ϵ��б��ƻ���ϸҲ���¼���
                    DataSendBinder();
                    DataPlanDetailBinder();
                }
            }
        }
    }

    //ȡ�����ϵ�λ�����쵼����
    protected string GetPickUnitLeaderCode(string strEditUnitCode)
    {
        string strGetUnitHQL = string.Format(@"select g.Leader 
                    from T_WZGetUnit g
                    left join T_ProjectMember pl on g.Leader = pl.UserCode
                    left join T_ProjectMember pf on g.FeeManage = pf.UserCode
                    left join T_ProjectMember pm on g.MaterialPerson = pm.UserCode
                    where g.UnitCode = '{0}'", strEditUnitCode);
        DataTable dtGetUnit = ShareClass.GetDataSetFromSql(strGetUnitHQL, "GetUnit").Tables[0];

        if (dtGetUnit.Rows.Count  > 0)
        {
            return dtGetUnit.Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    protected void DG_Store_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            //�����
            string cmdName = e.CommandName;
            if (cmdName == "add")
            {
                string strSendCode = HF_SendCode.Value;
                if (!string.IsNullOrEmpty(strSendCode))
                {
                    string cmdArges = e.CommandArgument.ToString();


                    WZStoreBLL wZStoreBLL = new WZStoreBLL();
                    string strWZStoreHQL = "from WZStore as wZStore where ID = " + cmdArges;
                    IList lstStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                    if (lstStore != null && lstStore.Count > 0)
                    {
                        WZStore wZStore = (WZStore)lstStore[0];

                        if (wZStore.StockCode != DDL_StoreRoom.SelectedValue)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKCBYZ").ToString().Trim() + "')", true);
                            return;
                        }

                        //���ϵ����ƻ����ۡ�����桴��浥�ۡ�												
                        //���ϵ����ƻ�������ʵ�������������ƻ����ۡ�												
                        //���ϵ�����ֵ������桴��ֵ�����������ƻ���												
                        //���ϵ�����������ƻ���������ֵ��												
                        //���ϵ�����ֵ��š�����桴��ֵ��š�												
                        //���ϵ�����ѹ��š�����桴��ѹ��š�												
                        //���ϵ�����š�����桴��š�												
                        //���ϵ�����λ�š�����桴��λ�š�												
                        TXT_PlanPrice.Text = wZStore.StorePrice.ToString();
                        decimal decimalActualNumber = 0;
                        decimal.TryParse(TXT_ActualNumber.Text.Trim(), out decimalActualNumber);
                        decimal decimalPlanMoney = decimalActualNumber * wZStore.StorePrice;
                        TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.00");
                        HF_DownRatio.Value = wZStore.DownRatio.ToString();
                        decimal decimalDownMoney = wZStore.DownRatio * decimalPlanMoney;
                        TXT_DownMoney.Text = decimalDownMoney.ToString("#0,00");
                        TXT_CleanMoney.Text = (decimalPlanMoney - decimalDownMoney).ToString();
                        TXT_ReduceCode.Text = wZStore.DownCode;
                        TXT_WearyCode.Text = wZStore.WearyCode;
                        TXT_CheckCode.Text = wZStore.CheckCode;
                        TXT_GoodsCode.Text = wZStore.GoodsCode;

                        TXT_PlanPrice.Text = wZStore.StorePrice.ToString();

                        for (int i = 0; i < DG_Store.Items.Count; i++)
                        {
                            DG_Store.Items[i].ForeColor = Color.Black;
                        }

                        e.Item.ForeColor = Color.Red;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJFLD").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
    }

    protected void BT_Calc_Click(object sender, EventArgs e)
    {
        string strActualNumber = TXT_ActualNumber.Text.Trim();
        if (!string.IsNullOrEmpty(strActualNumber))
        {
            if (!ShareClass.CheckIsNumber(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSFSLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }
            else
            {
                decimal decimalActualNumber = 0;
                decimal.TryParse(strActualNumber, out decimalActualNumber);
                decimal decimalPlanPrice = 0;
                decimal.TryParse(TXT_PlanPrice.Text.Trim(), out decimalPlanPrice);

                decimal decimalPlanMoney = decimalActualNumber * decimalPlanPrice;
                TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.00");

                decimal decimalDownRatio = 0;
                decimal.TryParse(HF_DownRatio.Value, out decimalDownRatio);
                decimal decimalDownMoney = decimalDownRatio * decimalPlanMoney;
                TXT_DownMoney.Text = decimalDownMoney.ToString("#0,00");
                TXT_CleanMoney.Text = (decimalPlanMoney - decimalDownMoney).ToString();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSFSLBNWK").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strSendCode = HF_SendCode.Value;
        if (!string.IsNullOrEmpty(strSendCode))
        {
            string strSendMethod = DDL_SendMethod.SelectedValue;    //���Ϸ�ʽ
            string strActualNumber = TXT_ActualNumber.Text.Trim();  //ʵ������
            string strPlanPrice = TXT_PlanPrice.Text.Trim();        //�ƻ�����
            string strPlanMoney = TXT_PlanMoney.Text.Trim();        //�ƻ����
            string strDownMoney = TXT_DownMoney.Text.Trim();        //��ֵ���
            string strCleanMoney = TXT_CleanMoney.Text.Trim();      //����
            string strReduceCode = TXT_ReduceCode.Text.Trim();      //��ֵ���
            string strWearyCode = TXT_WearyCode.Text.Trim();        //��ѹ���
            string strCheckCode = TXT_CheckCode.Text.Trim();        //���
            string strGoodsCode = TXT_GoodsCode.Text.Trim();        //��λ��
            string strManageRate = TXT_ManageRate.Text.Trim();      //�������
            string strCarCode = DL_CarCode.SelectedValue.Trim();
            string strComment = TXT_Comment.Text.Trim();

            if (string.IsNullOrEmpty(strSendMethod))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFLFSBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSSLBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSSSLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strManageRate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGLFLBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strManageRate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGLFLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }


            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal deciamlManageRate = 0;
            decimal.TryParse(strManageRate, out deciamlManageRate);

            WZSendBLL wZSendBLL = new WZSendBLL();
            string strWZSendHQL = "from WZSend as wZSend where SendCode = '" + strSendCode + "'";
            IList listWZSend = wZSendBLL.GetAllWZSends(strWZSendHQL);
            if (listWZSend != null && listWZSend.Count == 1)
            {
                WZSend wZSend = (WZSend)listWZSend[0];

                wZSend.SendMethod = strSendMethod;
                wZSend.ActualNumber = decimalActualNumber;
                wZSend.PlanPrice = decimal.Parse(strPlanPrice);
                wZSend.PlanMoney = decimal.Parse(strPlanMoney);
                wZSend.DownMoney = decimal.Parse(strDownMoney);
                wZSend.CleanMoney = decimal.Parse(strCleanMoney);
                wZSend.ReduceCode = strReduceCode;
                wZSend.WearyCode = strWearyCode;
                wZSend.CheckCode = strCheckCode;
                wZSend.GoodsCode = strGoodsCode;

                wZSend.CarCode = strCarCode;
                wZSend.Comment = strComment;

                wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                //���¼��ط��ϵ��б�
                DataSendBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
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
        for (int i = 0; i < DG_Send.Items.Count; i++)
        {
            DG_Send.Items[i].ForeColor = Color.Black;
        }

        for (int i = 0; i < DG_Store.Items.Count; i++)
        {
            DG_Store.Items[i].ForeColor = Color.Black;
        }

        DDL_SendMethod.SelectedValue = "";           //���Ϸ�ʽ
        TXT_ActualNumber.Text = "0";                 //ʵ������
        TXT_PlanPrice.Text = "0";                     //�ƻ�����
        TXT_PlanMoney.Text = "0";                    //�ƻ����
        TXT_DownMoney.Text = "0";                 //��ֵ���
        TXT_CleanMoney.Text = "0";               //����
        TXT_ReduceCode.Text = "";                //��ֵ���
        TXT_WearyCode.Text = "";                  //��ѹ���
        TXT_CheckCode.Text = "";                  //���
        TXT_GoodsCode.Text = "";                  //��λ��
        TXT_ManageRate.Text = "0";                //�������
        DL_CarCode.SelectedValue = "";
        TXT_Comment.Text = "";
    }

    protected void BT_MoreAdd_Click(object sender, EventArgs e)
    {
        string strPlanDetailID = Request.Form["cb_PlanDetail_ID"];
        if (!string.IsNullOrEmpty(strPlanDetailID))
        {
            string[] arrPlanDetailID = strPlanDetailID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrPlanDetailID.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrPlanDetailID[i]))
                {
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + arrPlanDetailID[i];
                    IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                    if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];

                        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                        string strWZPickingPlanHQL = string.Format("from WZPickingPlan as wZPickingPlan where PlanCode = '{0}'", wZPickingPlanDetail.PlanCode);
                        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
                        if (listWZPickingPlan != null && listWZPickingPlan.Count > 0)
                        {
                            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

                            //���ӷ��ϵ�
                            WZSend wZSend = new WZSend();
                            wZSend.SendCode = CreateNewSendCode();          //���ϱ��
                            wZSend.TicketTime = DateTime.Now;
                            wZSend.PurchaseEngineer = strUserCode;
                            wZSend.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                            wZSend.IsMark = 0;
                            wZSend.SendMethod = LanguageHandle.GetWord("LanPiao").ToString().Trim();

                            //���ϵ�����Ŀ���롵����𡵡���λ��š������ϵ�λ�������ϼƻ�����Ŀ���롵����𡵡���λ��š������ϵ�λ��
                            wZSend.ProjectCode = wZPickingPlan.ProjectCode;
                            wZSend.StoreRoom = wZPickingPlan.StoreRoom;
                            wZSend.UnitCode = wZPickingPlan.UnitCode;
                            wZSend.PickingUnit = wZPickingPlan.PickingUnit;


                            //���ϵ����ļ�Ա��������Ա�������ݡ���Ŀ���롵�ӹ�����Ŀ������
                            WZProjectBLL wZProjectBLL = new WZProjectBLL();
                            string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZPickingPlan.ProjectCode + "'";
                            IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                            if (projectList != null && projectList.Count > 0)
                            {
                                WZProject wZProject = (WZProject)projectList[0];

                                wZSend.Checker = wZProject.Checker;
                                wZSend.Safekeeper = wZProject.Safekeep;
                            }

                            //���ϵ��������쵼�������ݡ���λ��š������ϵ�λ������
                            WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
                            string strWZGetUnitSql = "from WZGetUnit as wZGetUnit where UnitCode = '" + wZPickingPlan.UnitCode + "'";
                            IList unitList = wZGetUnitBLL.GetAllWZGetUnits(strWZGetUnitSql);
                            if (unitList != null && unitList.Count > 0)
                            {
                                WZGetUnit wZGetUnit = (WZGetUnit)unitList[0];

                                wZSend.UpLeader = wZGetUnit.Leader;
                            }
                            //���ϵ����ƻ���š������ʴ��롵���ƻ���ϸ���ƻ���š������ʴ��롵
                            wZSend.PlanDetaiID = wZPickingPlanDetail.ID;
                            wZSend.ObjectCode = wZPickingPlanDetail.ObjectCode;

                            //��;���ϵ���ʵ��������
                            decimal decimalCurrentNumber = 0;
                            string strSendHQL = string.Format(@"select PlanDetaiID,SUM(ActualNumber) as SumActualNumber from T_WZSend
                                   where PlanDetaiID={0} group by PlanDetaiID", wZPickingPlanDetail.ID);
                            DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];
                            if (dtSend != null && dtSend.Rows.Count > 0)
                            {
                                decimal.TryParse(ShareClass.ObjectToString(dtSend.Rows[0]["SumActualNumber"]), out decimalCurrentNumber);
                            }
                            //���ϵ����ƻ����������ƻ���ϸ��ȱ������������;���ϵ���ʵ��������
                            wZSend.PlanNumber = wZPickingPlanDetail.ShortNumber - decimalCurrentNumber;
                            //���ϵ���ʵ�������������ϵ����ƻ�������
                            wZSend.ActualNumber = wZSend.PlanNumber;
                            //���ϵ����ļ����ڡ����������ڡ�����-��
                            wZSend.CheckTime = "-"; //DateTime.Now;
                            wZSend.SendTime = "-"; //DateTime.Now;

                            wZSend.CarCode = DL_CarCode.SelectedValue.Trim();
                            wZSend.Comment = TXT_Comment.Text.Trim();

                            WZSendBLL wZSendBLL = new WZSendBLL();
                            wZSendBLL.AddWZSend(wZSend);

                            //�޸ļƻ���ϸ<ʹ�ñ��> = -1
                            wZPickingPlanDetail.IsMark = -1;
                            wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);
                        }
                    }
                }
            }

            //���¼��ط��ϵ��б��ƻ���ϸҲ���¼���
            DataSendBinder();
            DataPlanDetailBinder();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZJHMX").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void DDL_SendMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSendMethod = DDL_SendMethod.SelectedValue;
        if (!string.IsNullOrEmpty(strSendMethod))
        {
            decimal decimalActualNumber = 0;
            decimal.TryParse(TXT_ActualNumber.Text.Trim(), out decimalActualNumber);

            if (strSendMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
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

                TXT_ActualNumber.BackColor = Color.Red;
            }
            else
            {
                decimalActualNumber = decimalActualNumber > 0 ? decimalActualNumber : -1 * decimalActualNumber;

                TXT_ActualNumber.Text = decimalActualNumber.ToString("#0.00");

                TXT_ActualNumber.BackColor = Color.White;
            }
        }
    }

    /// <summary>
    ///  ���ɷ��ϵ�Code
    /// </summary>
    private string CreateNewSendCode()
    {
        string strNewSendCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strSendCodeHQL = string.Format("select count(1) as RowNumber from T_WZSend where to_char( TicketTime, 'yyyy-mm-dd' ) like '{0}%'", DateTime.Now.ToString("yyyy-MM"));
                DataTable dtSendCode = ShareClass.GetDataSetFromSql(strSendCodeHQL, "SendCode").Tables[0];
                int intSendCodeNumber = int.Parse(dtSendCode.Rows[0]["RowNumber"].ToString());
                intSendCodeNumber = intSendCodeNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbSendCode = new StringBuilder();
                    for (int j = 4 - intSendCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbSendCode.Append("0");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewSendCode = strYear + "" + strMonth + sbSendCode.ToString() + intSendCodeNumber.ToString();

                    //��֤�µķ��ϵ�Code�Ƿ����
                    string strCheckNewSendCodeHQL = "select count(1) as RowNumber from T_WZSend where SendCode = '" + strNewSendCode + "'";
                    DataTable dtCheckNewSendCode = ShareClass.GetDataSetFromSql(strCheckNewSendCodeHQL, "CheckNewSendCode").Tables[0];
                    int intCheckNewSendCode = int.Parse(dtCheckNewSendCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewSendCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intSendCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewSendCode;
    }

    /// <summary>
    /// ������ʴ���Ļ���ϵ��
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

    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //�༭
        string strEditSendCode = HF_NewSendCode.Value;
        if (string.IsNullOrEmpty(strEditSendCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDFLDLB").ToString().Trim() + "')", true);
            return;
        }

        WZSendBLL wZSendBLL = new WZSendBLL();
        string strSendHQL = "from WZSend as wZSend where SendCode = '" + strEditSendCode + "'";
        IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
        if (listSend != null && listSend.Count == 1)
        {
            WZSend wZSend = (WZSend)listSend[0];

            HF_SendCode.Value = wZSend.SendCode;
            TXT_SendCode.Text = wZSend.SendCode;

            TXT_ObjectCode.Text = wZSend.ObjectCode;
            TXT_TicketTime.Text = wZSend.TicketTime.ToString("yyyy-MM-dd");
            //DDL_StoreRoom.Items.Add(new ListItem(wZSend.StoreRoom, wZSend.StoreRoom));
            DDL_StoreRoom.SelectedValue = wZSend.StoreRoom;

            DDL_SendMethod.SelectedValue = wZSend.SendMethod;    //���Ϸ�ʽ
            TXT_ActualNumber.Text = wZSend.ActualNumber.ToString();  //ʵ������
            TXT_PlanPrice.Text = wZSend.PlanPrice.ToString();        //�ƻ�����
            TXT_PlanMoney.Text = wZSend.PlanMoney.ToString();        //�ƻ����
            TXT_DownMoney.Text = wZSend.DownMoney.ToString();        //��ֵ���
            TXT_CleanMoney.Text = wZSend.CleanMoney.ToString();      //����
            TXT_ReduceCode.Text = wZSend.ReduceCode;                   //��ֵ���
            TXT_WearyCode.Text = wZSend.WearyCode;                     //��ѹ���
            TXT_CheckCode.Text = wZSend.CheckCode;                 //���
            TXT_GoodsCode.Text = wZSend.GoodsCode;                   //��λ��
            TXT_ManageRate.Text = wZSend.ManageRate.ToString();      //�������

            if (wZSend.SendMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
            {
                TXT_ActualNumber.BackColor = Color.Red;
            }
            else
            {
                TXT_ActualNumber.BackColor = Color.White;
            }

            try
            {
                DL_CarCode.SelectedValue = wZSend.CarCode.Trim();
            }
            catch
            {
                DL_CarCode.SelectedValue = "-";
            }
            TXT_Comment.Text = wZSend.Comment;

            //���ؿ���б�
            DataStoreBinder(wZSend.ObjectCode);
        }
    }

    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //ɾ��
        string strEditSendCode = HF_NewSendCode.Value;
        if (string.IsNullOrEmpty(strEditSendCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDFLDLB").ToString().Trim() + "')", true);
            return;
        }

        WZSendBLL wZSendBLL = new WZSendBLL();
        string strSendHQL = "from WZSend as wZSend where SendCode = '" + strEditSendCode + "'";
        IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
        if (listSend != null && listSend.Count == 1)
        {
            WZSend wZSend = (WZSend)listSend[0];
            if (wZSend.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim() || wZSend.PurchaseEngineer != strUserCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRYJHTYBWDDLYHSBYXSC").ToString().Trim() + "')", true);
                return;
            }

            wZSendBLL.DeleteWZSend(wZSend);

            //���¼����б�
            DG_Send.CurrentPageIndex = 0;
            DataSendBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
    }

    protected void BT_NewSubmit_Click(object sender, EventArgs e)
    {
        //�ύ
        string strEditSendCode = HF_NewSendCode.Value;
        if (string.IsNullOrEmpty(strEditSendCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDFLDLB").ToString().Trim() + "')", true);
            return;
        }

        WZSendBLL wZSendBLL = new WZSendBLL();
        string strSendHQL = "from WZSend as wZSend where SendCode = '" + strEditSendCode + "'";
        IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
        if (listSend != null && listSend.Count == 1)
        {
            WZSend wZSend = (WZSend)listSend[0];
            if (wZSend.Progress == LanguageHandle.GetWord("LuRu").ToString().Trim())
            {
                string strCheckCode = wZSend.CheckCode;
                if (!string.IsNullOrEmpty(strCheckCode))
                {
                    //�ļ�
                    wZSend.Progress = LanguageHandle.GetWord("CaiJian").ToString().Trim();
                }
                else
                {
                    //��Ʊ
                    wZSend.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                }

                wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                //���¼������ϵ��б�
                DataSendBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWLRBNTJ").ToString().Trim() + "')", true);
                return;
            }
        }
    }

    protected void BT_NewTicketReturn_Click(object sender, EventArgs e)
    {
        //��Ʊ�˻�
        string strEditSendCode = HF_NewSendCode.Value;
        if (string.IsNullOrEmpty(strEditSendCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDFLDLB").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_NewCheckReturn_Click(object sender, EventArgs e)
    {
        //�ļ��˻�
        string strEditSendCode = HF_NewSendCode.Value;
        if (string.IsNullOrEmpty(strEditSendCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDFLDLB").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_NewPrint_Click(object sender, EventArgs e)
    {
        //��ӡ
        string strEditSendCode = HF_NewSendCode.Value;
        if (string.IsNullOrEmpty(strEditSendCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDFLDLB").ToString().Trim() + "')", true);
            return;
        }

        Response.Redirect("TTWZSendPrintPage.aspx?sendCode=" + strEditSendCode);
    }


    protected void LoadCarListByAuthority()
    {
        string strHQL;
   
        strHQL = " Select CarCode From T_CarInformation Where ";
        strHQL += " Status='InUse' ";
        //      strHQL += " and Status='InUse' and CarCode not in (Select CarCode From T_CarAssignForm Where Status='Departure') ";
        strHQL += " Order By PurchaseTime DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarInformation");

        DL_CarCode.DataSource = ds;
        DL_CarCode.DataBind();

        DL_CarCode.Items.Insert(0, new ListItem("--Select--", "-"));
    }

}
