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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ڳ����ݵ���", strUserCode);
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
                    and r.Progress in ('���','���','����') 
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
                //���
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != "���")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSKZTBNSH+"')", true);
                        return;
                    }

                    wZRequest.Progress = "���";

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    //�������б�
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHCG+"')", true);
                }
            }
            else if (cmdName == "notRequest")
            {
                //�˻����
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != "���")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSSHZTBNTH+"')", true);
                        return;
                    }

                    wZRequest.Progress = "���";

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    //�������б�
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTHCG+"')", true);
                }
            }
            else if (cmdName == "account")
            {
                //����
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != "���")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSSHZTBNBX+"')", true);
                        return;
                    }

                    wZRequest.Progress = "����";
                    wZRequest.CancelTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    AccountHandler(wZRequest.RequestCode);

                    //�������б�
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBXCG+"')", true);
                }
            }
            else if (cmdName == "notAccount")
            {
                //�����˻�
                string cmdArges = e.CommandArgument.ToString();
                WZRequestBLL wZRequestBLL = new WZRequestBLL();
                string strRequestHQL = "from WZRequest as wZRequest where RequestCode = '" + cmdArges + "'";
                IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
                if (listRequest != null && listRequest.Count == 1)
                {
                    WZRequest wZRequest = (WZRequest)listRequest[0];

                    if (wZRequest.Progress != "����" || wZRequest.Approver.Trim() != strUserCode || wZRequest.IsPay != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSBXZTHZSHRBZHZFKBZBW0BNTH+"')", true);
                        return;
                    }

                    wZRequest.Progress = "���";
                    wZRequest.CancelTime = "-";
                    wZRequest.BeforePayMoney = 0;
                    wZRequest.Arrearage = 0;

                    wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);

                    CancelAccountHandler(wZRequest.RequestCode);

                    //�������б�
                    DataRequestBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTHCG+"')", true);
                }
            }
        }
    }


    /// <summary>
    /// ����
    /// </summary>
    private void AccountHandler(string strRequestCode)
    {
        #region ע��
        //д��ͬ��												
        //   ��ͬ��������ԭֵ����������												
        //   ��ͬ��δ�������ͬ�������ܶ����ͬ������												
        //д���ϵ���												
        //   ���ϵ���������ˡ�������������ˡ�												
        //   ���ϵ����������ȡ�����������												
        //д�����򿪺�ͬ����������ԭ��д��¼��												
        //    �ٵ���ͬ��Ԥ��������0��ʱ��												
        //        ����Ԥ�������0��												
        //        ����Ƿ�������������Ԥ��������Ѹ��												
        //    �ڵ���ͬ��Ԥ��������������ʱ��												
        //        ����Ԥ�������ͬ��Ԥ����												
        //        ����Ƿ�������������Ԥ��������Ѹ��												
        //        Ȼ��ʹ��ͬ��Ԥ��������0��												
        //    �۵���ͬ��Ԥ��������������ʱ��												
        //        ����Ԥ�������������												
        //        ����Ƿ�����0��												
        //        ��ͬ��Ԥ������ԭֵ������Ԥ���												
        #endregion
        WZRequestBLL wZRequestBLL = new WZRequestBLL();
        string strRequestHQL = string.Format("from WZRequest as wZRequest where RequestCode ='{0}'", strRequestCode);
        IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
        if (listRequest != null && listRequest.Count > 0)
        {
            WZRequest wZRequest = (WZRequest)listRequest[0];

            //д��ͬ
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
                    //����ͬ��Ԥ��������0��ʱ
                    wZRequest.BeforePayMoney = 0;
                    wZRequest.Arrearage = wZRequest.BorrowMoney - wZRequest.BeforePayMoney - wZRequest.PayMoney;
                }
                else if (wZCompact.BeforePayBalance < wZRequest.BorrowMoney)
                {
                    //����ͬ��Ԥ��������������ʱ��
                    wZRequest.BeforePayMoney = wZCompact.BeforePayBalance;
                    wZRequest.Arrearage = wZRequest.BorrowMoney - wZRequest.BeforePayMoney - wZRequest.PayMoney;

                    wZCompact.BeforePayBalance = 0;
                }
                else if (wZCompact.BeforePayBalance > wZRequest.BorrowMoney)
                {
                    //����ͬ��Ԥ��������������ʱ��
                    wZRequest.BeforePayMoney = wZRequest.BorrowMoney;
                    wZRequest.Arrearage = 0;

                    wZCompact.BeforePayBalance -= wZRequest.BeforePayMoney;
                }

                //���º�ͬ
                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
                //������
                wZRequestBLL.UpdateWZRequest(wZRequest, wZRequest.RequestCode);
            }
            //д���ϵ�
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = string.Format("from WZCollect as wZCollect where RequestCode ='{0}'", wZRequest.RequestCode);
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count > 0)
            {
                for (int i = 0; i < listCollect.Count; i++)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[i];
                    wZCollect.FinanceApprove = wZRequest.Approver;
                    wZCollect.PayProcess = "����";

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);
                }
            }
        }
    }

    /// <summary>
    /// ȡ������
    /// </summary>
    private void CancelAccountHandler(string strRequestCode)
    {
        #region ע��
        //д��ͬ��												
        //   ��ͬ����������ԭֵ����������												
        //   ��ͬ��δ�������ͬ�������ܶ����ͬ������												
        //   ��ͬ��Ԥ����												
        //      ����Ԥ�������0��ʱ����ͬ��Ԥ��������0��												
        //      ����Ԥ�������0��ʱ����ͬ��Ԥ������ԭֵ������Ԥ���												
        //д���ϵ���												
        //   ���ϵ���������ˡ�����-��												
        //   ���ϵ����������ȡ�����¼�롱												
        //д����												
        //   �������ȡ�������ˡ�												
        //   �����������ڡ�����-��												
        //   ����Ԥ�������0��												
        //   ����Ƿ�����0��		
        #endregion
        WZRequestBLL wZRequestBLL = new WZRequestBLL();
        string strRequestHQL = string.Format("from WZRequest as wZRequest where RequestCode ='{0}'", strRequestCode);
        IList listRequest = wZRequestBLL.GetAllWZRequests(strRequestHQL);
        if (listRequest != null && listRequest.Count > 0)
        {
            WZRequest wZRequest = (WZRequest)listRequest[0];

            //д��ͬ
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
                    //����Ԥ�������0��ʱ
                    wZCompact.BeforePayBalance = 0;
                }
                else if (wZRequest.BeforePayMoney > 0)
                {
                    //����Ԥ�������0��ʱ
                    wZCompact.BeforePayBalance += wZRequest.BeforePayMoney;
                }

                //���º�ͬ
                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
            }
            //д���ϵ�
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = string.Format("from WZCollect as wZCollect where RequestCode ='{0}'", wZRequest.RequestCode);
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count > 0)
            {
                for (int i = 0; i < listCollect.Count; i++)
                {
                    WZCollect wZCollect = (WZCollect)listCollect[i];
                    wZCollect.FinanceApprove = "-";
                    wZCollect.PayProcess = "¼��";

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);
                }
            }
        }
    }

}