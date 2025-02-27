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
        bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ڳ����ݵ���", strUserCode);
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
                    and s.Progress in ('��Ʊ','����') 
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
                //����
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

                        //����
                        string strMessage = AccountHandler(wZSend.SendCode);
                        if (strMessage == "Success")
                        {
                            //���¼������ϵ��б�
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
                //����
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

                        //����
                        string strMessage = cancelAccountHandler(wZSend.SendCode);
                        if (strMessage == "Success")
                        {
                            //���¼������ϵ��б�
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
    /// ���ʣ�д����
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

                //<���>��<���ʴ���>��<���>���жϿ�����Ƿ���ڵ�ǰ�������
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
                    //����д��ڵ�ǰ����
                    WZStore wZStore = (WZStore)lstWZStore[0];

                    wZStore.OutNumber += wZSend.ActualNumber;//��������
                    wZStore.OutPrice += wZSend.TotalMoney;   //������
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
                    wZStore.EndOutTime = DateTime.Now;//ĩ�����
                    wZStore.IsMark = -1;                //ʹ�ñ��

                    //�޸Ŀ��
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //���ϵ�<��������>=ϵͳ���ڣ�����=���ϣ�������=-1
                    wZSend.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZSend.Progress = LanguageHandle.GetWord("FaLiao").ToString().Trim();
                    wZSend.IsMark = -1;
                    //�޸����ϵ�
                    wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                    //�޸ļƻ���ϸ
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + wZSend.PlanDetaiID;
                    IList listPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strPlanDetailHQL);
                    if (listPlanDetail != null && listPlanDetail.Count > 0)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listPlanDetail[0];

                        wZPickingPlanDetail.ShortNumber += wZSend.ActualNumber;              //ȱ������
                        decimal decimalShortConver = 0;
                        decimal decimalConvertRatio = GetConvertRatioByObjectCode(strObjectCode);   //����ϵ��
                        if (decimalConvertRatio != 0)
                        {
                            decimalShortConver = wZPickingPlanDetail.ShortNumber / decimalConvertRatio;
                        }
                        wZPickingPlanDetail.ShortConver += decimalShortConver;    //ȱ�ڻ���

                        wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);

                    }

                    //������Ŀ��<���Ͻ��>��<˰��>��<���ӷ�>
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZSend.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZProject.SendMoney += wZSend.TotalMoney;

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                    }

                    //�ƻ���ϸ<����> = LanguageHandle.GetWord("FaLiao").ToString().Trim()
                    string strUpdatePlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '����' where ID = " + wZSend.PlanDetaiID; 
                    ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                    strResult = "Success";
                    return strResult;
                }
                else if (lstWZStore.Count > 1)
                {
                    //����д��ڶ����ǰ����
                    strResult = LanguageHandle.GetWord("KuCunZhongCunZaiDuoGeKuBieWuZi").ToString().Trim() + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
                }
                else
                {
                    //����в����ڵ�ǰ����
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
    /// ���ʣ�д����
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

                //<���>��<���ʴ���>��<���>���жϿ�����Ƿ���ڵ�ǰ�������
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
                    //����д��ڵ�ǰ����
                    WZStore wZStore = (WZStore)lstWZStore[0];

                    wZStore.OutNumber -= wZSend.ActualNumber;//��������
                    wZStore.OutPrice -= wZSend.TotalMoney;   //������
                    wZStore.StoreNumber -= (wZStore.YearNumber + wZStore.InNumber - wZStore.OutNumber);//�������
                    wZStore.StoreMoney -= (wZStore.YearMoney + wZStore.InMoney - wZStore.OutPrice);    //�����
                    //��浥��
                    if (wZStore.StoreNumber != 0)
                    {
                        wZStore.StorePrice = wZStore.StoreMoney / wZStore.StoreNumber;
                    }
                    else
                    {
                        wZStore.StorePrice = 0;
                    }
                    wZStore.EndOutTime = DateTime.Now;//ĩ�����
                    wZStore.IsMark = 0;                //ʹ�ñ��

                    //�޸Ŀ��
                    wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                    //���ϵ�<��������>=ϵͳ���ڣ�����=���ϣ�������=-1
                    wZSend.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    wZSend.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                    wZSend.IsMark = 0;
                    //�޸����ϵ�
                    wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                    //�޸ļƻ���ϸ
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + wZSend.PlanDetaiID;
                    IList listPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strPlanDetailHQL);
                    if (listPlanDetail != null && listPlanDetail.Count > 0)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listPlanDetail[0];

                        wZPickingPlanDetail.ShortNumber -= wZSend.ActualNumber;              //ȱ������
                        decimal decimalShortConver = 0;
                        decimal decimalConvertRatio = GetConvertRatioByObjectCode(strObjectCode);   //����ϵ��
                        if (decimalConvertRatio != 0)
                        {
                            decimalShortConver = wZPickingPlanDetail.ShortNumber / decimalConvertRatio;
                        }
                        wZPickingPlanDetail.ShortConver -= decimalShortConver;    //ȱ�ڻ���

                        wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);

                    }

                    //������Ŀ��<���Ͻ��>��<˰��>��<���ӷ�>
                    WZProjectBLL wZProjectBLL = new WZProjectBLL();
                    string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + wZSend.ProjectCode + "'";
                    IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                    if (projectList != null && projectList.Count > 0)
                    {
                        WZProject wZProject = (WZProject)projectList[0];

                        wZProject.SendMoney -= wZSend.TotalMoney;

                        wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
                    }

                    //�ƻ���ϸ<����> = LanguageHandle.GetWord("FaLiao").ToString().Trim()
                    string strUpdatePlanDetailHQL = "update T_WZPickingPlanDetail set Progress = '��Ʊ' where ID = " + wZSend.PlanDetaiID; 
                    ShareClass.RunSqlCommand(strUpdatePlanDetailHQL);

                    strResult = "Success";
                    return strResult;
                }
                else if (lstWZStore.Count > 1)
                {
                    //����д��ڶ����ǰ����
                    strResult = LanguageHandle.GetWord("KuCunZhongCunZaiDuoGeKuBieWuZi").ToString().Trim() + strStockCode + "," + strObjectCode + "," + strCheckCode + ")";
                    return strResult;
                }
                else
                {
                    //����в����ڵ�ǰ����
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
}
