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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ڳ����ݵ���", strUserCode);
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
                            and c.Progress in('��Ʊ','����')  ", strUserCode);


        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != "ȫ��")
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
            //����
            string cmdArges = e.CommandArgument.ToString();
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];
                if (wZCollect.Progress == "��Ʊ")
                {
                    wZCollect.Progress = "����";
                    wZCollect.IsMark = -1;
                    wZCollect.CollectTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string strMessage = AccountHandler(wZCollect.CollectCode);

                    if (strMessage == "�ɹ�")
                    {
                        //���¼������ϵ��б�
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
            //�˵�
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
                if (wZCollect.Progress == "����")
                {
                    wZCollect.Progress = "��Ʊ";
                    wZCollect.IsMark = 0;
                    wZCollect.CollectTime = "";

                    string strMessage = NotAccountHandler(wZCollect.CollectCode);

                    if (strMessage == "�ɹ�")
                    {
                        //���¼������ϵ��б�
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
            //��ӡ
            string cmdArges = e.CommandArgument.ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCollectPrintPag.aspx?collectCode=" + cmdArges + "');", true);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZCollectPrintPag.aspx?collectCode=" + cmdArges + "')", true);
            return;
        }
    }

    /// <summary>
    /// ���ʣ�д����
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

                //<���>��<���ʴ���>��<���>���жϿ�����Ƿ���ڵ�ǰ�������
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
                    //����д��ڵ�ǰ����
                    WZStore wZStore = (WZStore)lstWZStore[0];
                    if (wZStore.DownDesc == -1 || wZStore.WearyDesc == -1)
                    {
                        strResult = "��ѹ�ƻ��ͼ�ֵ�ƻ���Ϊ0����ƽ�⣬��ɹ���";
                        return strResult;
                    }

                    wZStore.InNumber += wZCollect.ActualNumber;//�������
                    wZStore.InMoney += wZCollect.ActualMoney;   //ʵ�����
                    wZStore.StoreNumber = wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber;//�������
                    wZStore.StoreMoney = wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice;    //�����
                                                                                                    //��浥��
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

                    wZStore.EndInTime = DateTime.Now;//ĩ�����
                    wZStore.IsMark = -1;                //ʹ�ñ��

                    //�޸Ŀ��
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //���ϵ�<��������>=ϵͳ���ڣ�����=���ϣ�������=-1
                    wZCollect.CollectTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZCollect.Progress = "����";
                    wZCollect.IsMark = -1;
                    //�޸����ϵ�
                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //�޸ĺ�ͬ��ϸ
                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                        wZCompactDetail.CollectNumber += wZCollect.ActualNumber;
                        wZCompactDetail.CollectMoney += wZCollect.ActualMoney + wZCollect.RatioMoney + wZCollect.Freight + wZCollect.OtherObject;

                        wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                        //�޸ĺ�ͬ�������ܶ=��ͬ��ϸ���ܺ�
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

                    //������Ŀ��<���Ͻ��>��<˰��>��<���ӷ�>
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

                    //�ƻ���ϸ<����> = "����"
                    string strPlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '����' where ID = " + wZCollect.PlanDetailID;
                    ShareClass.RunSqlCommand(strPlanDetailHQL);

                    strResult = "�ɹ�";
                }
                else if (lstWZStore.Count > 1)
                {
                    //����д��ڶ����ǰ����
                    strResult = "����д��ڶ����������ʴ��룬���һ����(" + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
                }
                else
                {
                    //����в����ڵ�ǰ����
                    WZStore wZStore = new WZStore();
                    wZStore.StockCode = strStockCode;
                    wZStore.ObjectCode = strObjectCode;
                    wZStore.CheckCode = strCheckCode;

                    wZStore.InNumber += wZCollect.ActualNumber;//�������
                    wZStore.InMoney += wZCollect.ActualMoney;   //ʵ�����
                    wZStore.StoreNumber = wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber;//�������
                    wZStore.StoreMoney = wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice;    //�����
                                                                                                    //��浥��
                    if (wZStore.StoreNumber != 0)
                    {
                        wZStore.StorePrice = wZStore.StoreMoney / wZStore.StoreNumber;
                    }
                    else
                    {
                        wZStore.StorePrice = 0;
                    }
                    //wZStore.EndInTime = DateTime.Now;//ĩ�����              
                    wZStore.IsMark = -1; //ʹ�ñ��

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

                    //���ӿ��
                    wZStoreBLL.AddWZStore(wZStore);

                    //���ϵ�<��������>=ϵͳ���ڣ�����=���ϣ�������=-1
                    //wZCollect.CollectTime = DateTime.Now;
                    wZCollect.CollectTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZCollect.Progress = "����";
                    wZCollect.IsMark = -1;

                    //�޸����ϵ�
                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //�޸ĺ�ͬ��ϸ
                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                        wZCompactDetail.CollectNumber += wZCollect.ActualNumber;
                        wZCompactDetail.CollectMoney += wZCollect.ActualMoney + wZCollect.RatioMoney + wZCollect.Freight + wZCollect.OtherObject;

                        wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                        //�޸ĺ�ͬ�������ܶ=��ͬ��ϸ���ܺ�
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

                    //������Ŀ��<���Ͻ��>��<˰��>��<���ӷ�>
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

                    //�ƻ���ϸ<����> = "����"
                    string strPlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '����' where ID = " + wZCollect.PlanDetailID;
                    ShareClass.RunSqlCommand(strPlanDetailHQL);

                    strResult = "�ɹ�";
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
    ///  �˵�
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
                    //����д��ڵ�ǰ����
                    WZStore wZStore = (WZStore)lstWZStore[0];

                    wZStore.InNumber -= wZCollect.ActualNumber;//�������
                    wZStore.InMoney -= wZCollect.ActualMoney;   //ʵ�����
                    wZStore.StoreNumber = wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber;//�������
                    wZStore.StoreMoney = wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice;    //�����
                                                                                                    //��浥��
                    wZStore.CleanMoney = wZStore.StoreMoney = wZStore.DownMoney; //��澻��

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


                        wZStore.EndInTime = dtCollectTime;//ĩ����⣬֮ǰ���һ�����ϵ�����������
                    }

                    ////�ж��Ƿ��ǡ�0��桱���������֡��������͵��ֶ�ֵ ="0"
                    //if (wZStore.YearNumber == 0 && wZStore.YearMoney == 0
                    //    && wZStore.InNumber == 0 && wZStore.InMoney == 0
                    //    && wZStore.OutNumber == 0 && wZStore.OutPrice == 0
                    //    && wZStore.StoreNumber == 0 && wZStore.StoreMoney == 0)
                    //{
                    //    wZCollect.IsMark = 0;
                    //}

                    //�ж��Ƿ��ǡ�0��桱
                    if (wZStore.StoreNumber == 0 )
                    {
                        wZStore.IsMark = 0;

                        wZCollect.IsMark = 0;
                    }

                    //�޸Ŀ��
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //���ϵ�����=��Ʊ��������=0
                    wZCollect.CollectTime = "-";
                    wZCollect.Progress = "��Ʊ";
                    wZCollect.IsMark = 0;
                    //�޸����ϵ�
                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //�޸ĺ�ͬ��ϸ
                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    string strWZCompactDetailHQL = "from WZCompactDetail as wZCompactDetail where ID = " + wZCollect.CompactDetailID;
                    IList listCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
                    if (listCompactDetail != null && listCompactDetail.Count > 0)
                    {
                        WZCompactDetail wZCompactDetail = (WZCompactDetail)listCompactDetail[0];

                        wZCompactDetail.CollectNumber -= wZCollect.ActualNumber;
                        wZCompactDetail.CollectMoney = wZCompactDetail.CollectMoney - (wZCollect.ActualMoney + wZCollect.RatioMoney + wZCollect.Freight + wZCollect.OtherObject);

                        wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                        //�޸ĺ�ͬ�������ܶ=��ͬ��ϸ���ܺ�
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

                    //������Ŀ��<���Ͻ��>��<˰��>��<���ӷ�>
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

                    //�ƻ���ϸ<����> = "��ͬ"
                    string strPlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '��ͬ' where ID = " + wZCollect.PlanDetailID;
                    ShareClass.RunSqlCommand(strPlanDetailHQL);

                    strResult = "�ɹ�";
                }
                else
                {
                    strResult = "���棬����в����ڵ�ǰ���ʣ����飡";
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