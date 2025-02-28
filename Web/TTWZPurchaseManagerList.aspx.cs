using System; using System.Resources;
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

public partial class TTWZPurchaseManagerList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        string strPurchaseHQL = string.Format(@"select p.*,e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName
                        from T_WZPurchase p
                        left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode 
                        where p.PurchaseManager = '{0}' 
                        and p.Progress in ('�ύ','����','��׼','ѯ��') 
                        order by p.MarkTime desc", strUserCode);
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_PurchaseSql.Text = strPurchaseHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;

            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdArges = e.CommandArgument.ToString();
            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
            string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + cmdArges + "'";
            IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
            if (listPurchase != null && listPurchase.Count == 1)
            {
                WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                if (cmdName == "price")
                {
                    //ѯ��
                    //if (wZPurchase.Progress == "����" || wZPurchase.Progress == "��׼")
                    //{
                        //���ۿ�ʼ < ���۽�ֹ����ʾ
                        //if (DateTime.Now < DateTime.Parse(wZPurchase.PurchaseEndTime))
                        //{
                            wZPurchase.Progress = "ѯ��";
                            wZPurchase.PurchaseStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //�ɹ��嵥�Ľ���Ϊѯ��
                            WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                            string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode= '" + wZPurchase.PurchaseCode + "'";
                            IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                            if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count > 0)
                            {
                                for (int i = 0; i < listWZPurchaseDetail.Count; i++)
                                {
                                    WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[i];
                                    wZPurchaseDetail.Progress = "ѯ��";

                                    wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);
                                }
                            }

                            //���¼����б�
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJCG+"')", true);
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBJJZRXYDSJXG+"')", true);
                        //    return;
                        //}
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGWJJDBSBPHZPZBNJ+"')", true);
                    //    return;
                    //}
                }
                else if (cmdName == "report")
                {
                    ////�ʱ�
                    //if (wZPurchase.Progress == "����")
                    //{
                        string strUpLeader = HF_UpLeader.Value;
                        //if (string.IsNullOrEmpty(strUpLeader))
                        //{
                        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZSJLD+"')", true);
                        //    return;
                        //}

                        wZPurchase.Progress = "�ϱ�";
                        wZPurchase.PurchaseStartTime = "-";
                        wZPurchase.UpLeader = strUpLeader;

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //���¼����б�
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCBCG+"')", true);
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGWJJDBSBPBNCB+"')", true);
                    //    return;
                    //}
                }
                else if (cmdName == "return")
                {
                    ////�˻�
                    //if (wZPurchase.Progress == "����")
                    //{
                        wZPurchase.Progress = "¼��";
                        wZPurchase.PurchaseStartTime = "-";

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //���¼����б�
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTHCG+"')", true);
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGWJJDBSBPBNTHLR+"')", true);
                    //    return;
                    //}
                }
                else if (cmdName == "cancel")
                {
                    ////ȡ��
                    //if (wZPurchase.Progress == "ѯ��")
                    //{
                        wZPurchase.Progress = "����";

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //�ɹ��嵥�Ľ���Ϊ¼��
                        WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                        string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode= '" + wZPurchase.PurchaseCode + "'";
                        IList listWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                        if (listWZPurchaseDetail != null && listWZPurchaseDetail.Count > 0)
                        {
                            for (int i = 0; i < listWZPurchaseDetail.Count; i++)
                            {
                                WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)listWZPurchaseDetail[i];
                                wZPurchaseDetail.Progress = "¼��";

                                wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);
                            }
                        }

                        //���¼����б�
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXCG+"')", true);
                    //}
                    //else {
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGWJJDBSJBNXBP+"')", true);
                    //    return;
                    //}
                }
                //else if (cmdName == "report")
                //{
                //    //�ʱ�
                //    //if (wZPurchase.Progress == "����")
                //    //{


                //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReport('" + wZPurchase.PurchaseCode+ "')", true);
                //    //}
                //    //else {
                //    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCGWJJDBSBPBNXCB+"')", true);
                //    //    return;
                //    //}
                //}
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_PurchaseSql.Text.Trim();
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();
    }



    protected void BT_Report_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
        string strUpLeader = HF_UpLoaderCodeName.Value;

        string[] arrUpLeader = strUpLeader.Split('|');

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = "�ʱ�";
            wZPurchase.PurchaseStartTime = "-";
            wZPurchase.UpLeader = arrUpLeader[0];

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            //���¼����б�
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCBCG+"')", true);
        }
    }
}