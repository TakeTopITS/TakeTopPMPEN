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

public partial class TTWZPlanPurchaseList : System.Web.UI.Page
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
            DataBinder();
        }
    }

    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        string strWZPickingPlanHQL = string.Format(@"select pp.*,
                        pm.UserName as PlanMarkerName,
                        pf.UserName as FeeManageName,
                        pe.UserName as PurchaseEngineerName
                        from T_WZPickingPlan pp
                        left join T_ProjectMember pm on pp.PlanMarker = pm.UserCode
                        left join T_ProjectMember pf on pp.FeeManage = pf.UserCode
                        left join T_ProjectMember pe on pp.PurchaseEngineer = pe.UserCode
                        where pp.PurchaseEngineer = '{0}' 
                        and pp.Progress not in ( '¼��','�ᱨ')", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZPickingPlanHQL += " and pp.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZPickingPlanHQL += " and pp.ProjectCode = '" + strProjectCode + "'";
        }
        string strPlanName = TXT_PlanName.Text.Trim();
        if (!string.IsNullOrEmpty(strPlanName))
        {
            strWZPickingPlanHQL += " and pp.PlanName like '%" + strPlanName + "%'";
        }

        strWZPickingPlanHQL += " order by pp.MarkerTime desc";

        DataTable dtPlan = ShareClass.GetDataSetFromSql(strWZPickingPlanHQL, "Plan").Tables[0];

        DG_List.DataSource = dtPlan;
        DG_List.DataBind();

        LB_PlanSQL.Text = strWZPickingPlanHQL;

        LB_ShowRecordCount.Text = dtPlan.Rows.Count.ToString();
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
                if (cmdName == "click")
                {
                    //����
                    string strPlanCode = wZPickingPlan.PlanCode.Trim();
                    string strProgress = wZPickingPlan.Progress.Trim();
                    string strPurchaseEngineer = wZPickingPlan.PurchaseEngineer.Trim();

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strPurchaseEngineer + "','" + strUserCode.Trim() + "');", true);
                    ControlStatusChange(strProgress, strPurchaseEngineer, strUserCode);

                    HF_NewPlanCode.Value = strPlanCode;
                    HF_PlanCodeValue.Value = strPlanCode;
                    HF_ReturnPlanCode.Value = strPlanCode;

                    HF_NewProgress.Value = strProgress;
                    HF_NewPurchaseEngineer.Value = strPurchaseEngineer;

                    if (wZPickingPlan.Progress == LanguageHandle.GetWord("QianShou").ToString().Trim())
                    {
                        BT_NewBalance.Enabled = true;
                    }
                    else
                    {
                        BT_NewBalance.Enabled = false;
                    }

                }
                else if (cmdName == "sign")
                {
                    if (wZPickingPlan.Progress == LanguageHandle.GetWord("ShenHe").ToString().Trim())
                    {
                        wZPickingPlan.Progress = LanguageHandle.GetWord("QianShou").ToString().Trim();
                        wZPickingPlan.SignTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        //���¼����б�
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCG").ToString().Trim() + "')", true);
                    }
                }
                else if (cmdName == "signReturn")
                {
                    //�˻�ǩ��
                    if (wZPickingPlan.Progress == LanguageHandle.GetWord("QianShou").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickSignReturn('" + cmdArges + "')", true);
                        return;
                        //wZPickingPlan.Progress = LanguageHandle.GetWord("ShenHe").ToString().Trim();
                        //wZPickingPlan.SignTime = "-";
                        //wZPickingPlan.ReturnReason = TXT_ReturnReason.Text.Trim();

                        //wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        ////���¼����б�
                        //DataBinder();

                        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSTHCG").ToString().Trim()+"')", true);
                    }
                }
                else if (cmdName == "returnPlan")
                {
                    //�˻�
                    if (wZPickingPlan.Progress == LanguageHandle.GetWord("ShenHe").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReturn('" + cmdArges + "')", true);
                        return;
                    }
                }
                else if (cmdName == "cancel")
                {
                    //����
                    if (wZPickingPlan.Progress == LanguageHandle.GetWord("QianShou").ToString().Trim())
                    {
                        //���ƻ���ϸ<ȱ������>
                        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                        string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + wZPickingPlan.PlanCode + "'";
                        IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                        string strMessage = string.Empty;
                        if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                        {
                            for (int i = 0; i < listWZPickingPlanDetail.Count; i++)
                            {
                                WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[i];
                                if (wZPickingPlanDetail.ShortNumber > 0)
                                {
                                    strMessage += LanguageHandle.GetWord("JiHuaMingXiYouQueKouDaYu0DeShi").ToString().Trim();
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(strMessage))
                        {
                            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSTRMESSAGE").ToString().Trim()+"')", true);
                            HF_CancelText.Value = strMessage;
                            HF_PickingPlanCode.Value = wZPickingPlan.PlanCode;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertCancel()", true);

                            return;
                        }
                        //�鿴�ɹ��ļ��Ƿ���û�к�����
                        string strPurchaseHQL = string.Format(@"select p.PurchaseCode,p.Progress from T_WZPickingPlanDetail pl
                                left join T_WZPurchaseDetail pd on pl.ID = pd.PlanDetailID
                                left join T_WZPurchase p on pd.PurchaseCode = p.PurchaseCode
                                where pl.PlanCode = '{0}'
                                and p.Progress != '����'", wZPickingPlan.PlanCode); 
                        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
                        if (dtPurchase != null && dtPurchase.Rows.Count > 0)
                        {
                            string strResult = string.Empty;
                            foreach (DataRow drPurchase in dtPurchase.Rows)
                            {
                                strResult += LanguageHandle.GetWord("CaiGouBianHao").ToString().Trim() + ShareClass.ObjectToString(drPurchase["PurchaseCode"]) + LanguageHandle.GetWord("WeiHeXiaobr").ToString().Trim();
                            }
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRRESULT").ToString().Trim() + "')", true);
                            return;
                        }
                        //���ƻ�����£����з��ϵ��������ǡ�����-1�����粻�������ϵ�����ʾ��ȷ�Ϻ��˳�����
                        string strSendHQL = string.Format(@"select s.SendCode,s.Progress from T_WZPickingPlanDetail pl
                            left join T_WZSend s on pl.ID = s.PlanDetaiID
                            where pl.PlanCode = '{0}'
                            and s.IsMark != -1", wZPickingPlan.PlanCode);
                        DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];
                        if (dtSend != null && dtSend.Rows.Count > 0)
                        {
                            string strResult = string.Empty;
                            foreach (DataRow drSend in dtSend.Rows)
                            {
                                strResult += LanguageHandle.GetWord("FaLiaoChan").ToString().Trim() + ShareClass.ObjectToString(drSend["SendCode"]) + LanguageHandle.GetWord("JieSuanBiaoJiBuWei1br").ToString().Trim();
                            }
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRRESULT").ToString().Trim() + "')", true);
                            return;
                        }

                        wZPickingPlan.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
                        wZPickingPlan.CancelTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        //���¼����б�
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXCG").ToString().Trim() + "')", true);
                    }
                }
                else if (cmdName == "cancelReturn")
                {
                    //�����˻�
                    if (wZPickingPlan.Progress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
                    {
                        wZPickingPlan.Progress = LanguageHandle.GetWord("QianShou").ToString().Trim();
                        wZPickingPlan.CancelTime = "-";

                        wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        //���¼����б�
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXTHCG").ToString().Trim() + "')", true);
                    }
                }
                else if (cmdName == "balance")
                {
                    //ƽ��
                    //TODO
                    //��ƽ���־���ߵ����ƽ�⡱��ť��������Ҫ��ƽ���棺												
                    //�������ƻ���ϸ�����ʴ��롵����𡵣���桴���ʴ��롵�����    ��ͬ����š��Ŀ�������ϼ�												
                    //д��¼��												
                    //�����ơ�����������ݼƻ���ϸ���ƻ����������ƻ���ϸ��ƽ���־��������ԣ��												
                    //�����ơ�������������ƻ���ϸ���ƻ����������ƻ���ϸ��ƽ���־���������㡱												
                    //�����ơ��������������0�����޼�¼���ƻ���ϸ��ƽ���־�������޿�桱		
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + wZPickingPlan.PlanCode + "'";
                    IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                    if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                    {
                        for (int i = 0; i < listWZPickingPlanDetail.Count; i++)
                        {
                            WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[i];
                            string strStoreHQL = string.Format(@"select COALESCE(SUM(StoreNumber),0) as TotalStoreNumber from T_WZStore
                                        where StockCode = '{0}'
                                        and ObjectCode = '{1}'", wZPickingPlan.StoreRoom, wZPickingPlanDetail.ObjectCode);
                            DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                            decimal decimalStoreNumber = 0;
                            decimal.TryParse(ShareClass.ObjectToString(dtStore.Rows[0]["TotalStoreNumber"]), out decimalStoreNumber);
                            if (decimalStoreNumber == 0)
                            {
                                wZPickingPlanDetail.StoreSign = LanguageHandle.GetWord("MoKuCun").ToString().Trim();
                            }
                            else if (decimalStoreNumber >= wZPickingPlanDetail.PlanNumber)
                            {
                                wZPickingPlanDetail.StoreSign = LanguageHandle.GetWord("FuYu").ToString().Trim();
                            }
                            else if (decimalStoreNumber < wZPickingPlanDetail.PlanNumber)
                            {
                                wZPickingPlanDetail.StoreSign = LanguageHandle.GetWord("BuZu").ToString().Trim();
                            }

                            wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);
                        }

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPKWC").ToString().Trim() + "')", true);
                    }
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_PlanSQL.Text.Trim();
        DataTable dtPlan = ShareClass.GetDataSetFromSql(strHQL, "Plan").Tables[0];

        DG_List.DataSource = dtPlan;
        DG_List.DataBind();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }


    protected void BT_NewSign_Click(object sender, EventArgs e)
    {
        //ǩ��
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            if (wZPickingPlan.Progress == LanguageHandle.GetWord("ShenHe").ToString().Trim())
            {
                wZPickingPlan.Progress = LanguageHandle.GetWord("QianShou").ToString().Trim();
                wZPickingPlan.SignTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strEditPlanCode);

                //���¼����б�
                DataBinder();


                BT_NewBalance.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCG").ToString().Trim() + "');", true);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewSignReturn_Click(object sender, EventArgs e)
    {
        //ǩ���˻�
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickSignReturn('" + strEditPlanCode + "');", true);
        return;
    }

    protected void BT_NewPlanReturn_Click(object sender, EventArgs e)
    {
        //�ύ�˻�
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReturn('" + strEditPlanCode + "');", true);
        return;
    }



    protected void BT_NewPlanBrowse_Click(object sender, EventArgs e)
    {
        //�ƻ����
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanBrowse.aspx?planCode=" + strEditPlanCode + "');", true);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZPlanBrowse.aspx?planCode=" + strEditPlanCode + "');", true);
        return;
    }


    protected void BT_NewDetailBrowse_Click(object sender, EventArgs e)
    {
        //��ϸ���
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanDetailBrowse.aspx?planCode=" + strEditPlanCode + "');", true);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZPlanDetailBrowse.aspx?planCode=" + strEditPlanCode + "');", true);
        return;
    }

    protected void BT_NewBalance_Click(object sender, EventArgs e)
    {
        //ƽ��
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
            string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + wZPickingPlan.PlanCode + "'";
            IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
            if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
            {
                for (int i = 0; i < listWZPickingPlanDetail.Count; i++)
                {
                    WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[i];
                    string strStoreHQL = string.Format(@"select COALESCE(SUM(StoreNumber),0) as TotalStoreNumber from T_WZStore
                                        where StockCode = '{0}'
                                        and ObjectCode = '{1}'", wZPickingPlan.StoreRoom, wZPickingPlanDetail.ObjectCode);
                    DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                    decimal decimalStoreNumber = 0;
                    decimal.TryParse(ShareClass.ObjectToString(dtStore.Rows[0]["TotalStoreNumber"]), out decimalStoreNumber);
                    if (decimalStoreNumber == 0)
                    {
                        wZPickingPlanDetail.StoreSign = LanguageHandle.GetWord("MoKuCun").ToString().Trim();
                    }
                    else if (decimalStoreNumber >= wZPickingPlanDetail.PlanNumber)
                    {
                        wZPickingPlanDetail.StoreSign = LanguageHandle.GetWord("FuYu").ToString().Trim();
                    }
                    else if (decimalStoreNumber < wZPickingPlanDetail.PlanNumber)
                    {
                        wZPickingPlanDetail.StoreSign = LanguageHandle.GetWord("BuZu").ToString().Trim();
                    }

                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPKWC").ToString().Trim() + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�����ϼƻ���ϸ��');", true); 
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }



    protected void BT_NewCancel_Click(object sender, EventArgs e)
    {
        //����
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            if (wZPickingPlan.Progress == LanguageHandle.GetWord("QianShou").ToString().Trim())
            {
                //���ƻ���ϸ<ȱ������>
                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + wZPickingPlan.PlanCode + "'";
                IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                string strMessage = string.Empty;
                if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                {
                    for (int i = 0; i < listWZPickingPlanDetail.Count; i++)
                    {
                        WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[i];
                        if (wZPickingPlanDetail.ShortNumber > 0)
                        {
                            strMessage += LanguageHandle.GetWord("JiHuaMingXiYouQueKouDaYu0DeShi").ToString().Trim();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(strMessage))
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSTRMESSAGE").ToString().Trim()+"')", true);
                    HF_CancelText.Value = strMessage;
                    HF_PickingPlanCode.Value = wZPickingPlan.PlanCode;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertCancel();", true);

                    return;
                }
                //�鿴�ɹ��ļ��Ƿ���û�к�����
                string strPurchaseHQL = string.Format(@"select p.PurchaseCode,p.Progress from T_WZPickingPlanDetail pl
                                left join T_WZPurchaseDetail pd on pl.ID = pd.PlanDetailID
                                left join T_WZPurchase p on pd.PurchaseCode = p.PurchaseCode
                                where pl.PlanCode = '{0}'
                                and p.Progress != '����'", wZPickingPlan.PlanCode); 
                DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
                if (dtPurchase != null && dtPurchase.Rows.Count > 0)
                {
                    string strResult = string.Empty;
                    foreach (DataRow drPurchase in dtPurchase.Rows)
                    {
                        strResult += LanguageHandle.GetWord("CaiGouBianHao").ToString().Trim() + ShareClass.ObjectToString(drPurchase["PurchaseCode"]) + LanguageHandle.GetWord("WeiHeXiaobr").ToString().Trim();
                    }
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRRESULT").ToString().Trim() + "');", true);
                    return;
                }
                //���ƻ�����£����з��ϵ��������ǡ�����-1�����粻�������ϵ�����ʾ��ȷ�Ϻ��˳�����
                string strSendHQL = string.Format(@"select s.SendCode,s.Progress from T_WZPickingPlanDetail pl
                            left join T_WZSend s on pl.ID = s.PlanDetaiID
                            where pl.PlanCode = '{0}'
                            and s.IsMark != -1", wZPickingPlan.PlanCode);
                DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];
                if (dtSend != null && dtSend.Rows.Count > 0)
                {
                    string strResult = string.Empty;
                    foreach (DataRow drSend in dtSend.Rows)
                    {
                        strResult += LanguageHandle.GetWord("FaLiaoChan").ToString().Trim() + ShareClass.ObjectToString(drSend["SendCode"]) + LanguageHandle.GetWord("JieSuanBiaoJiBuWei1br").ToString().Trim();
                    }
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRRESULT").ToString().Trim() + "');", true);
                    return;
                }

                wZPickingPlan.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
                wZPickingPlan.CancelTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strEditPlanCode);

                //���¼����б�
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXCG").ToString().Trim() + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ǰ���ϼƻ����Ȳ���ǩ�գ����ܺ�����');", true); 
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewCancelReturn_Click(object sender, EventArgs e)
    {
        //�����˻�
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            if (wZPickingPlan.Progress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
            {
                wZPickingPlan.Progress = LanguageHandle.GetWord("QianShou").ToString().Trim();
                wZPickingPlan.CancelTime = "-";

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strEditPlanCode);

                //���¼����б�
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXTHCG").ToString().Trim() + "');", true);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewUnitChange_Click(object sender, EventArgs e)
    {
        //��λ���
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }
        // 1. ���ϼƻ������ϵ�λ�����												
        //�� ��������ϼƻ��µļƻ���ϸ��ʹ�ñ�ǡ�����0������ֱ�Ӱ��ڢ���ǩ���˻ش���												
        //�� ��������ϼƻ��µļƻ���ϸ��ʹ�ñ�ǡ�����-1���������³�����												
        //    �������λ�������ť												
        //    �����飺												
        //       ���ϼƻ����ƻ���š����ƽ���ϸ���ƻ���š�												
        //       ���ϼƻ����ƻ���š����ƽ���ϸ���ƻ���š������Ӧ���ƽ��������ȡ�����¼�롱												
        //       ���ϼƻ����ƻ���š��ٷ��ϵ����ƻ���š�												
        //       ���ϼƻ����ƻ���š������ϵ����ƻ���š������ϵ��������ǡ�����0��												
        //       ���ϼƻ����ƻ���š������ϵ����ƻ���š������ϵ��������ǡ�����-1�������ϵ����������ڡ�������ǰ�¡�												
        //    ͨ����飬�򵯳������ϵ�λ������Ի���ѡ���滻�󣬱��淵��												
        //       д��¼��												
        //          ���ϼƻ�����λ��š��������ġ���λ��š�												
        //          ���ϼƻ������ϵ�λ���������ġ����ϵ�λ��												
        //          ���ϵ�����λ��š������ϼƻ�����λ��š�												
        //          ���ϵ������ϵ�λ�������ϼƻ������ϵ�λ��												
        //          �ƽ���ϸ�����ϵ�λ�������ϼƻ������ϵ�λ��												
        //             �ƽ�������λ��š������ϼƻ�����λ��š�												
        //             �ƽ��������ϵ�λ�������ϼƻ������ϵ�λ��												
        //    δͨ����飬�������ʾ���£�ȷ���󷵻�												

        //    1.***���ƽ��������ȡ��١�¼�롱											
        //    2.***�ŷ��ϵ����������ڡ��١���ǰ�¡�											
        //       ��������ʾ�м�������ʾ������											
        //    �޷��������ϵ�λ�����    ��ȷ����			

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
            string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + wZPickingPlan.PlanCode + "' and IsMark = 0";
            IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);

            if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
            {

            }
        }
    }



    protected void BT_NewProjectChange_Click(object sender, EventArgs e)
    {
        //��Ŀ���
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

    }



    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        //��ϸ
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ��Ҫ�����ļƻ���');", true); 
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewPurchaseEngineer = HF_NewPurchaseEngineer.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanPurchaseListDetail.aspx?planCode=" + strEditPlanCode + "');", true);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZPlanPurchaseListDetail.aspx?planCode=" + strEditPlanCode + "');", true);
        return;
    }


    protected void BT_HiddenButton_Click(object sender, EventArgs e)
    {
        string strPlanCode = HF_PlanCodeValue.Value;
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            if (wZPickingPlan.Progress == LanguageHandle.GetWord("QianShou").ToString().Trim())
            {
                wZPickingPlan.Progress = LanguageHandle.GetWord("ShenHe").ToString().Trim();
                //wZPickingPlan.ReturnReason = HF_WriteText.Value;
                wZPickingPlan.ReturnReason = TB_SignReturnReason.Text.Trim();

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strPlanCode);

                //���¼����б�
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTHCG").ToString().Trim() + "');", true);
            }
        }
    }



    protected void BT_HiddenCancel_Click(object sender, EventArgs e)
    {
        string strPlanCode = HF_PickingPlanCode.Value;
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            //�鿴�ɹ��ļ��Ƿ���û�к�����
            string strPurchaseHQL = string.Format(@"select p.PurchaseCode,p.Progress from T_WZPickingPlanDetail pl
                                left join T_WZPurchaseDetail pd on pl.ID = pd.PlanDetailID
                                left join T_WZPurchase p on pd.PurchaseCode = p.PurchaseCode
                                where pl.PlanCode = '{0}'
                                and p.Progress != '����'", wZPickingPlan.PlanCode); 
            DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
            if (dtPurchase != null && dtPurchase.Rows.Count > 0)
            {
                string strResult = string.Empty;
                foreach (DataRow drPurchase in dtPurchase.Rows)
                {
                    strResult += LanguageHandle.GetWord("CaiGouBianHao").ToString().Trim() + ShareClass.ObjectToString(drPurchase["PurchaseCode"]) + LanguageHandle.GetWord("WeiHeXiaobr").ToString().Trim();
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRRESULT").ToString().Trim() + "');", true);
                return;
            }
            //���ƻ�����£����з��ϵ��������ǡ�����-1�����粻�������ϵ�����ʾ��ȷ�Ϻ��˳�����
            string strSendHQL = string.Format(@"select s.SendCode,s.Progress from T_WZPickingPlanDetail pl
                            left join T_WZSend s on pl.ID = s.PlanDetaiID
                            where pl.PlanCode = '{0}'
                            and s.IsMark != -1", wZPickingPlan.PlanCode);
            DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];
            if (dtSend != null && dtSend.Rows.Count > 0)
            {
                string strResult = string.Empty;
                foreach (DataRow drSend in dtSend.Rows)
                {
                    strResult += LanguageHandle.GetWord("FaLiaoChan").ToString().Trim() + ShareClass.ObjectToString(drSend["SendCode"]) + LanguageHandle.GetWord("JieSuanBiaoJiBuWei1br").ToString().Trim();
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRRESULT").ToString().Trim() + "');", true);
                return;
            }

            wZPickingPlan.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
            wZPickingPlan.CancelTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strPlanCode);

            //���¼����б�
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHXCG").ToString().Trim() + "');", true);
        }
    }



    protected void BT_ReturnButton_Click(object sender, EventArgs e)
    {
        string strPlanCode = HF_ReturnPlanCode.Value;
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            if (wZPickingPlan.Progress == LanguageHandle.GetWord("ShenHe").ToString().Trim())
            {
                wZPickingPlan.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                wZPickingPlan.ApproveTime = "-";
                //wZPickingPlan.ReturnReason = HF_ReturnWriteText.Value;
                wZPickingPlan.ReturnReason = TB_PlanReturnReason.Text.Trim();

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strPlanCode);

                //���¼����б�
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJHTHCG").ToString().Trim() + "');", true);
            }
        }
    }




    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
    }

    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }


    /// <summary>
    ///  ���¼����б�
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();
    }


    /// <summary>
    /// �ƻ��������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BT_SortPlanCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strWZPickingPlanHQL = string.Format(@"select pp.*,
                        pm.UserName as PlanMarkerName,
                        pf.UserName as FeeManageName,
                        pe.UserName as PurchaseEngineerName
                        from T_WZPickingPlan pp
                        left join T_ProjectMember pm on pp.PlanMarker = pm.UserCode
                        left join T_ProjectMember pf on pp.FeeManage = pf.UserCode
                        left join T_ProjectMember pe on pp.PurchaseEngineer = pe.UserCode
                        where pp.PurchaseEngineer = '{0}' 
                        and pp.Progress != '¼��'", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZPickingPlanHQL += " and pp.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZPickingPlanHQL += " and pp.ProjectCode = '" + strProjectCode + "'";
        }
        string strPlanName = TXT_PlanName.Text.Trim();
        if (!string.IsNullOrEmpty(strPlanName))
        {
            strWZPickingPlanHQL += " and pp.PlanName = '" + strPlanName + "'";
        }

        if (!string.IsNullOrEmpty(HF_SortPlanCode.Value))
        {
            strWZPickingPlanHQL += " order by pp.PlanCode desc";

            HF_SortPlanCode.Value = "";
        }
        else
        {
            strWZPickingPlanHQL += " order by pp.PlanCode asc";

            HF_SortPlanCode.Value = "PlanCode";
        }

        DataTable dtPickingPlan = ShareClass.GetDataSetFromSql(strWZPickingPlanHQL, "PickingPlan").Tables[0];

        DG_List.DataSource = dtPickingPlan;
        DG_List.DataBind();

        LB_PlanSQL.Text = strWZPickingPlanHQL;
        LB_ShowRecordCount.Text = dtPickingPlan.Rows.Count.ToString();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }



    /// <summary>
    /// �ƻ���������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BT_SortPlanName_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strWZPickingPlanHQL = string.Format(@"select pp.*,
                        pm.UserName as PlanMarkerName,
                        pf.UserName as FeeManageName,
                        pe.UserName as PurchaseEngineerName
                        from T_WZPickingPlan pp
                        left join T_ProjectMember pm on pp.PlanMarker = pm.UserCode
                        left join T_ProjectMember pf on pp.FeeManage = pf.UserCode
                        left join T_ProjectMember pe on pp.PurchaseEngineer = pe.UserCode
                        where pp.PurchaseEngineer = '{0}' 
                        and pp.Progress != '¼��'", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZPickingPlanHQL += " and pp.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZPickingPlanHQL += " and pp.ProjectCode = '" + strProjectCode + "'";
        }
        string strPlanName = TXT_PlanName.Text.Trim();
        if (!string.IsNullOrEmpty(strPlanName))
        {
            strWZPickingPlanHQL += " and pp.PlanName = '" + strPlanName + "'";
        }

        if (!string.IsNullOrEmpty(HF_SortPlanName.Value))
        {
            strWZPickingPlanHQL += " order by pp.PlanName desc";

            HF_SortPlanName.Value = "";
        }
        else
        {
            strWZPickingPlanHQL += " order by pp.PlanName asc";

            HF_SortPlanName.Value = "PlanName";
        }

        DataTable dtPickingPlan = ShareClass.GetDataSetFromSql(strWZPickingPlanHQL, "PickingPlan").Tables[0];

        DG_List.DataSource = dtPickingPlan;
        DG_List.DataBind();

        LB_PlanSQL.Text = strWZPickingPlanHQL;
        LB_ShowRecordCount.Text = dtPickingPlan.Rows.Count.ToString();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }


    /// <summary>
    /// ��Ŀ��������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BT_SortProjectCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strWZPickingPlanHQL = string.Format(@"select pp.*,
                        pm.UserName as PlanMarkerName,
                        pf.UserName as FeeManageName,
                        pe.UserName as PurchaseEngineerName
                        from T_WZPickingPlan pp
                        left join T_ProjectMember pm on pp.PlanMarker = pm.UserCode
                        left join T_ProjectMember pf on pp.FeeManage = pf.UserCode
                        left join T_ProjectMember pe on pp.PurchaseEngineer = pe.UserCode
                        where pp.PurchaseEngineer = '{0}' 
                        and pp.Progress != '¼��'", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZPickingPlanHQL += " and pp.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZPickingPlanHQL += " and pp.ProjectCode = '" + strProjectCode + "'";
        }
        string strPlanName = TXT_PlanName.Text.Trim();
        if (!string.IsNullOrEmpty(strPlanName))
        {
            strWZPickingPlanHQL += " and pp.PlanName = '" + strPlanName + "'";
        }

        if (!string.IsNullOrEmpty(HF_SortProjectCode.Value))
        {
            strWZPickingPlanHQL += " order by pp.ProjectCode desc,pp.PlanCode desc";

            HF_SortProjectCode.Value = "";
        }
        else
        {
            strWZPickingPlanHQL += " order by pp.ProjectCode asc,pp.PlanCode asc";

            HF_SortProjectCode.Value = "ProjectCode";
        }

        DataTable dtPickingPlan = ShareClass.GetDataSetFromSql(strWZPickingPlanHQL, "PickingPlan").Tables[0];

        DG_List.DataSource = dtPickingPlan;
        DG_List.DataBind();

        LB_PlanSQL.Text = strWZPickingPlanHQL;
        LB_ShowRecordCount.Text = dtPickingPlan.Rows.Count.ToString();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }

    private void ControlStatusChange(string objProgress, string objPurchaseEngineer, string objUserCode)
    {
        BT_NewPlanBrowse.Enabled = true;
        BT_NewDetailBrowse.Enabled = true;
        BT_NewUnitChange.Enabled = true;
        BT_NewProjectChange.Enabled = true;

        //Label1.Text = objPurchaseEngineer + "-----" + objUserCode;
 
        if (objProgress == LanguageHandle.GetWord("ShenHe").ToString().Trim() && objPurchaseEngineer == objUserCode)
        {
            BT_NewSign.Enabled = true;
            BT_NewSignReturn.Enabled = false;
            BT_NewPlanReturn.Enabled = true;
            BT_NewBalance.Enabled = true;
            BT_NewCancel.Enabled = false;
            BT_NewCancelReturn.Enabled = false;

        }
        else if (objProgress == LanguageHandle.GetWord("QianShou").ToString().Trim() && objPurchaseEngineer == objUserCode)
        {
            BT_NewBalance.Enabled = true;
            BT_NewSign.Enabled = false;
            BT_NewSignReturn.Enabled = true;
            BT_NewPlanReturn.Enabled = false;
            BT_NewCancel.Enabled = true;
            BT_NewCancelReturn.Enabled = false;


        }
        else if (objProgress == LanguageHandle.GetWord("HeXiao").ToString().Trim() && objPurchaseEngineer == objUserCode)
        {
            BT_NewSign.Enabled = false;
            BT_NewSignReturn.Enabled = false;
            BT_NewPlanReturn.Enabled = false;
            BT_NewBalance.Enabled = false;
            BT_NewCancel.Enabled = false;
            BT_NewCancelReturn.Enabled = true;
        }
        else
        {
            BT_NewSign.Enabled = false;
            BT_NewSignReturn.Enabled = false;
            BT_NewPlanReturn.Enabled = false;
            BT_NewBalance.Enabled = false;
            BT_NewCancel.Enabled = false;
            BT_NewCancelReturn.Enabled = false;
        }

        if (objPurchaseEngineer == objUserCode)
        {
            BT_NewDetail.Visible = true;
        }
        else
        {
            BT_NewDetail.Visible = false;
        }


    }



    private void ControlStatusCloseChange()
    {
        BT_NewSign.Enabled = false;
        BT_NewSignReturn.Enabled = false;
        BT_NewPlanReturn.Enabled = false;
        BT_NewPlanBrowse.Enabled = false;
        BT_NewDetailBrowse.Enabled = false;

        BT_NewBalance.Enabled = false;
        BT_NewCancel.Enabled = false;
        BT_NewCancelReturn.Enabled = false;
        BT_NewUnitChange.Enabled = false;
        BT_NewProjectChange.Enabled = false;
        BT_NewDetail.Visible = false;
    }


}
