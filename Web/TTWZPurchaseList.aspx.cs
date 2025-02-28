using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPurchaseList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ɹ��ļ�", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();

            TXT_SearchProjectCode.BackColor = Color.CornflowerBlue;
            TXT_SearchPurchaseName.BackColor = Color.CornflowerBlue;

            ControlStatusCloseChange();
        }
    }

    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        //string strPurchaseHQL = string.Format(@"select p.*,
        //                e.UserName as PurchaseEngineerName,
        //                u.UserName as UpLeaderName,
        //                m.UserName as PurchaseManagerName,
        //                d.UserName as DisciplinarySupervisionName,
        //                c.UserName as ControlMoneyName,
        //                t.UserName as TenderCompetentName,
        //                s.UserName as DecisionName
        //                from T_WZPurchase p
        //                left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
        //                left join T_ProjectMember u on p.UpLeader = u.UserCode
        //                left join T_ProjectMember m on p.PurchaseManager = m.UserCode
        //                left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
        //                left join T_ProjectMember c on p.ControlMoney = c.UserCode
        //                left join T_ProjectMember t on p.TenderCompetent = t.UserCode
        //                left join T_ProjectMember s on p.Decision = s.UserCode
        //                where p.PurchaseEngineer = '{0}'", strUserCode);

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                    e.UserName as PurchaseEngineerName,
                    t.UserName as TenderCompetentName,
                    c.UserName as ControlMoneyName,
                    d.UserName as DisciplinarySupervisionName,
                    e1.Name as ExpertCode1Name,
                    e2.Name as ExpertCode2Name,
                    e3.Name as ExpertCode3Name,
                    j.UserName as PurchaseManagerName,
                    s.UserName as DecisionName,
                    u.UserName as UpLeaderName,
                    s1.SupplierName as SupplierCode1Name,
                    s2.SupplierName as SupplierCode2Name,
                    s3.SupplierName as SupplierCode3Name,
                    s4.SupplierName as SupplierCode4Name,
                    s5.SupplierName as SupplierCode5Name,
                    s6.SupplierName as SupplierCode6Name
                    from T_WZPurchase p
                     left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                    left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                    left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                    left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode
                    left join T_ProjectMember j on p.PurchaseManager = j.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode
                    left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                    left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                    left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                    left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                    left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                    left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode
                        where p.PurchaseEngineer = '{0}'", strUserCode);


        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        strPurchaseHQL += "order by p.MarkTime desc";
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        ControlStatusCloseChange();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
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

                    if (cmdName == "click")
                    {
                        string strProgress = wZPurchase.Progress;
                        int intIsMark = wZPurchase.IsMark;
                        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);

                        ControlStatusChange(strProgress, intIsMark.ToString());

                        HF_NewPurchaseCode.Value = wZPurchase.PurchaseCode;
                        HF_Progress.Value = strProgress;
                        HF_IsMark.Value = intIsMark.ToString();

                        if (wZPurchase.Progress == "¼��" || wZPurchase.IsMark == 0)
                        {
                            BT_NewDelete.Enabled = true;
                        }

                    }
                    else if (cmdName == "del")
                    {
                        if (wZPurchase.Progress != "¼��" || wZPurchase.IsMark != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWLRYJSYBJBW0SBYXSC + "')", true);
                            return;
                        }

                        wZPurchaseBLL.DeleteWZPurchase(wZPurchase);

                        //ɾ����ϸ
                        string strPurchaseDetailSQL = string.Format("delete T_WZPurchaseDetail where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                        ShareClass.RunSqlCommand(strPurchaseDetailSQL);

                        //���¼����б�
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);

                    }
                    else if (cmdName == "edit")
                    {

                    }
                    else if (cmdName == "approval")
                    {
                        //����
                        if (wZPurchase.Progress == "¼��")
                        {
                            //�ж��Ƿ��Ѿ�ѡ�ù�Ӧ�̣�ר��
                            string strCheckSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                                    where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                            DataTable dtCheckSupplier = ShareClass.GetDataSetFromSql(strCheckSupplierHQL, "Supplier").Tables[0];
                            if (dtCheckSupplier == null || dtCheckSupplier.Rows.Count <= 0)
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZDCGWJWZGYSBNBP + "')", true);
                                return;
                            }
                            //�ж��Ƿ��Ѿ�ѡ�ù�Ӧ�̣�ר��
                            string strCheckExpertHQL = string.Format(@"select * from T_WZPurchaseExpert
                                    where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                            DataTable dtCheckExpert = ShareClass.GetDataSetFromSql(strCheckExpertHQL, "Expert").Tables[0];
                            if (dtCheckExpert == null || dtCheckExpert.Rows.Count <= 0)
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZDCGWJWZZJBNBP + "')", true);
                                return;
                            }

                            wZPurchase.Progress = "����";

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //���¼����б�
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBPCG + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWLRBNBP + "')", true);
                            return;
                        }
                    }
                    else if (cmdName == "notApproval")
                    {
                        //ȡ������
                        if (wZPurchase.Progress == "����")
                        {
                            wZPurchase.Progress = "¼��";

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //���¼����б�
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ȡ��" + Resources.lang.ZZBPCG + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWBPBNXBP + "')", true);
                            return;
                        }
                    }
                    else if (cmdName == "cancel")
                    {
                        //����
                        if (wZPurchase.Progress == "����")
                        {
                            wZPurchase.Progress = "����";

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //���¼����б�
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHXCG + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWJCBNHX + "')", true);
                            return;
                        }
                    }
                    else if (cmdName == "notCancel")
                    {
                        //ȡ������
                        if (wZPurchase.Progress == "����")
                        {
                            wZPurchase.Progress = "����";

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //���¼����б�
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ȡ��" + Resources.lang.ZZHXCG + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWHXBNXHX + "')", true);
                            return;
                        }
                    }
                    else if (cmdName == "assess")
                    {
                        //����
                        if (wZPurchase.Progress == "ѯ��")
                        {
                            wZPurchase.Progress = "����";

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //���¼����б�
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZPBCG + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWJBNPB + "')", true);
                            return;
                        }
                    }
                    else if (cmdName == "apply")
                    {
                        //����
                        if (wZPurchase.Progress == "����")
                        {
                            wZPurchase.Progress = "����";

                            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                            //���¼����б�
                            DataBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBJCG + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWPBBNBJ + "')", true);
                            return;
                        }
                    }
                    else if (cmdName == "detail")
                    {
                        Response.Redirect("TTWZPurchaseDetail.aspx?PurchaseCode=" + wZPurchase.PurchaseCode);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYCYCYYSEXMESSAGE + "')", true);
        }
    }



    /// <summary>
    ///  ���ɲɹ�Code
    /// </summary>
    private string CreateNewPurchaseCode()
    {
        string strNewPurchaseCode = string.Empty;
        try
        {
            lock (this)
            {
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                if (strMonth.Length == 1)
                {
                    strMonth = "0" + strMonth;
                }

                bool isExist = true;
                string strPurchaseCodeHQL = "select count(1) as RowNumber from T_WZPurchase where to_char( MarkTime, 'yyyy-mm-dd') like '%" + strYear + "-" + strMonth + "%'";
                DataTable dtPurchaseCode = ShareClass.GetDataSetFromSql(strPurchaseCodeHQL, "PurchaseCode").Tables[0];
                int intPurchaseCodeNumber = int.Parse(dtPurchaseCode.Rows[0]["RowNumber"].ToString());
                intPurchaseCodeNumber = intPurchaseCodeNumber + 1;
                do
                {
                    StringBuilder sbPurchaseCode = new StringBuilder();
                    for (int j = 3 - intPurchaseCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbPurchaseCode.Append("0");
                    }
                    strNewPurchaseCode = strYear + strMonth + "-" + sbPurchaseCode.ToString() + intPurchaseCodeNumber.ToString();

                    //��֤�µĲɹ�����Ƿ����
                    string strCheckNewPurchaseCodeHQL = "select count(1) as RowNumber from T_WZPurchase where PurchaseCode = '" + strNewPurchaseCode + "'";
                    DataTable dtCheckNewPurchaseCode = ShareClass.GetDataSetFromSql(strCheckNewPurchaseCodeHQL, "CheckNewPurchaseCode").Tables[0];
                    int intCheckNewPurchaseCode = int.Parse(dtCheckNewPurchaseCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewPurchaseCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intPurchaseCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewPurchaseCode;
    }




    protected void BT_NewCreate_Click(object sender, EventArgs e)
    {
        //����
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;

        if (!string.IsNullOrEmpty(strEditPurchaseCode))
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseListDetail.aspx?PurchaseCode=');", true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseEdit.aspx?PurchaseCode=');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseEdit.aspx?PurchaseCode=');", true);
        }
    }

    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //�༭
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseEdit.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
        return;
    }


    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //ɾ��
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (wZPurchase.Progress != "¼��" || wZPurchase.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWLRYJSYBJBW0SBYXSC + "');", true);
                return;
            }

            wZPurchaseBLL.DeleteWZPurchase(wZPurchase);

            //ɾ����ϸ
            string strPurchaseDetailSQL = string.Format("delete T_WZPurchaseDetail where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
            ShareClass.RunSqlCommand(strPurchaseDetailSQL);

            //���¼����б�
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "');", true);
        }
    }


    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;


        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }


        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);

        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (wZPurchase.Progress == "¼��")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseDetail.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseDetailView.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
            }
        }
    }


    protected void BT_NewApproval_Click(object sender, EventArgs e)
    {
        //����
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (wZPurchase.Progress == "¼��" && wZPurchase.IsMark == -1)
            {


                //�ж��Ƿ��Ѿ�ѡ�ù�Ӧ�̣�ר��
                //                string strCheckSupplierHQL = string.Format(@"select * from T_WZPurchaseSupplier
                //                                    where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                //                DataTable dtCheckSupplier = ShareClass.GetDataSetFromSql(strCheckSupplierHQL, "Supplier").Tables[0];
                //                if (dtCheckSupplier == null || dtCheckSupplier.Rows.Count <= 0)
                //                {
                //                    string strNewProgress = HF_Progress.Value;
                //                    string strIsMark = HF_IsMark.Value;
                //                    ControlStatusChange(strNewProgress, strIsMark);
                //                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDCGWJWZGYSBNBP+"');", true);
                //                    return;
                //                }
                //�ж��Ƿ��Ѿ�ѡ�ù�Ӧ�̣�ר��
                //                string strCheckExpertHQL = string.Format(@"select * from T_WZPurchaseExpert
                //                                    where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                //                DataTable dtCheckExpert = ShareClass.GetDataSetFromSql(strCheckExpertHQL, "Expert").Tables[0];
                //                if (dtCheckExpert == null || dtCheckExpert.Rows.Count <= 0)
                //                {
                //                    string strNewProgress = HF_Progress.Value;
                //                    string strIsMark = HF_IsMark.Value;
                //                    ControlStatusChange(strNewProgress, strIsMark);
                //                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDCGWJWZZJBNBP+"');", true);
                //                    return;
                //                }



                if (wZPurchase.PlanMoney >= 300000)
                {
                    string strNewProgress = HF_Progress.Value;
                    string strIsMark = HF_IsMark.Value;
                    ControlStatusChange(strNewProgress, strIsMark);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "SelectTenderCompetent();", true);

                }
                else
                {
                    wZPurchase.Progress = "�ύ";

                    wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                    //���¼����б�
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�ύ�ɹ���');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���Ȳ�Ϊ¼�룬����ʹ�ò�Ϊ-1�������ύ��');", true);
                return;
            }
        }
    }


    protected void BT_NewNotApproval_Click(object sender, EventArgs e)
    {
        //�ύ�˻�
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (wZPurchase.Progress == "�ύ")
            {
                wZPurchase.Progress = "¼��";

                wZPurchase.TenderCompetent = "";

                wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                //���¼����б�
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�ύ�˻سɹ���');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���Ȳ�Ϊ�ύ������ȡ���ύ�˻أ�');", true);
                return;
            }
        }
    }



    protected void BT_NewCancel_Click(object sender, EventArgs e)
    {
        //����
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (wZPurchase.Progress == "����")
            {
                //��ѯ�ɹ��嵥
                string strPurchaseDetailSQL = string.Format(@"select * from T_WZPurchaseDetail
                            where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strPurchaseDetailSQL, "PurchaseDetail").Tables[0];
                if (dtPurchaseDetail != null && dtPurchaseDetail.Rows.Count > 0)
                {
                    foreach (DataRow drPurchaseDetail in dtPurchaseDetail.Rows)
                    {
                        string strPurchaseDetailProgress = ShareClass.ObjectToString(drPurchaseDetail["Progress"]);
                        if (strPurchaseDetailProgress.Trim() == "��ͬ")
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertPurchaseDetail();", true);
                            return;
                        }
                    }
                }

                //��ͬ
                string strCompactSQL = string.Format(@"select * from T_WZCompact
                            where CompactCode in
                            (
                            select distinct c.CompactCode from T_WZCompactDetail c
                            left join T_WZPurchaseDetail p on c.PurchaseDetailID = p.ID
                            where p.PurchaseCode = '{0}'
                            )
                            and Progress = '����'", wZPurchase.PurchaseCode);
                DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactSQL, "Compact").Tables[0];
                string strCompactResult = string.Empty;
                if (dtCompact != null && dtCompact.Rows.Count > 0)
                {
                    foreach (DataRow drCompact in dtCompact.Rows)
                    {
                        strCompactResult += ShareClass.ObjectToString(drCompact["CompactCode"]) + "��";
                    }
                }

                if (!string.IsNullOrEmpty(strCompactResult))
                {

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertCompact('���ɹ��ļ�������δ�����ĺ�ͬ�����ܺ���<br />��ͬ��ţ�<br />" + strCompactResult + "');", true);
                    return;
                }

                wZPurchase.Progress = "����";

                wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                //���¼����б�
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHXCG + "');", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWJCBNHX + "');", true);
                return;
            }
        }
    }


    protected void BT_NewNotCancel_Click(object sender, EventArgs e)
    {
        //ȡ������
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (wZPurchase.Progress == "����")
            {
                //���ϼƻ�
                string strPlanSQL = string.Format(@"select * from T_WZPickingPlan
                            where PlanCode in
                            (
                            select distinct c.PlanCode from T_WZPickingPlanDetail c
                            left join T_WZPurchaseDetail p on c.ID = p.PlanDetailID
                            where p.PurchaseCode = '{0}'
                            )
                            and Progress = '����'", wZPurchase.PurchaseCode);
                DataTable dtPlan = ShareClass.GetDataSetFromSql(strPlanSQL, "Plan").Tables[0];
                if (dtPlan != null && dtPlan.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertCompact('�òɹ��ļ��漰�����ϼƻ��Ѻ����������˻�');", true);
                    return;
                }

                wZPurchase.Progress = "����";

                wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                //���¼����б�
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ȡ��" + Resources.lang.ZZHXCG + "');", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWHXBNXHX + "');", true);
                return;
            }
        }
    }






    protected void DDL_SearchProgress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();

    }



    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();

    }


    /// <summary>
    ///  ���¼����б�
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();

        ControlStatusCloseChange();
    }


    protected void BT_TenderCompetent_Click(object sender, EventArgs e)
    {
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = "�ύ";

            string strTenderCompetent = HF_TenderCompetent.Value;
            string[] arrTenderCompetent = strTenderCompetent.Split('|');
            wZPurchase.TenderCompetent = arrTenderCompetent[0];

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            //���¼����б�
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�ύ�ɹ���');", true);
        }


    }



    protected void BT_CancelPurchaseDetail_Click(object sender, EventArgs e)
    {
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDCGLB + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
            //��ͬ
            string strCompactSQL = string.Format(@"select * from T_WZCompact
                            where CompactCode in
                            (
                            select distinct c.CompactCode from T_WZCompactDetail c
                            left join T_WZPurchaseDetail p on c.PurchaseDetailID = p.ID
                            where p.PurchaseCode = '{0}'
                            )
                            and Progress = '����'", wZPurchase.PurchaseCode);
            DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactSQL, "Compact").Tables[0];
            string strCompactResult = string.Empty;
            if (dtCompact != null && dtCompact.Rows.Count > 0)
            {
                foreach (DataRow drCompact in dtCompact.Rows)
                {
                    strCompactResult += ShareClass.ObjectToString(drCompact["CompactCode"]) + "��";
                }
            }

            if (!string.IsNullOrEmpty(strCompactResult))
            {

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertCompact('���ɹ��ļ�������δ�����ĺ�ͬ�����ܺ���<br />��ͬ��ţ�<br />" + strCompactResult + "');", true);
                return;
            }

            wZPurchase.Progress = "����";

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            //���¼����б�
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHXCG + "');", true);
        }
    }




    protected void BT_SortPurchaseCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        //string strPurchaseHQL = string.Format(@"select p.*,
        //                e.UserName as PurchaseEngineerName,
        //                u.UserName as UpLeaderName,
        //                m.UserName as PurchaseManagerName,
        //                d.UserName as DisciplinarySupervisionName,
        //                c.UserName as ControlMoneyName,
        //                t.UserName as TenderCompetentName,
        //                s.UserName as DecisionName
        //                from T_WZPurchase p
        //                left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
        //                left join T_ProjectMember u on p.UpLeader = u.UserCode
        //                left join T_ProjectMember m on p.PurchaseManager = m.UserCode
        //                left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
        //                left join T_ProjectMember c on p.ControlMoney = c.UserCode
        //                left join T_ProjectMember t on p.TenderCompetent = t.UserCode
        //                left join T_ProjectMember s on p.Decision = s.UserCode
        //                where p.PurchaseEngineer = '{0}'", strUserCode);
        //string strSearchProgress = DDL_SearchProgress.SelectedValue;
        //if (!string.IsNullOrEmpty(strSearchProgress))
        //{
        //    strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        //}
        //string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchProjectCode))
        //{
        //    strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        //}
        //string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchPurchaseName))
        //{
        //    strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        //}

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                    e.UserName as PurchaseEngineerName,
                    t.UserName as TenderCompetentName,
                    c.UserName as ControlMoneyName,
                    d.UserName as DisciplinarySupervisionName,
                    e1.Name as ExpertCode1Name,
                    e2.Name as ExpertCode2Name,
                    e3.Name as ExpertCode3Name,
                    j.UserName as PurchaseManagerName,
                    s.UserName as DecisionName,
                    u.UserName as UpLeaderName,
                    s1.SupplierName as SupplierCode1Name,
                    s2.SupplierName as SupplierCode2Name,
                    s3.SupplierName as SupplierCode3Name,
                    s4.SupplierName as SupplierCode4Name,
                    s5.SupplierName as SupplierCode5Name,
                    s6.SupplierName as SupplierCode6Name
                    from T_WZPurchase p
                     left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                    left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                    left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                    left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode
                    left join T_ProjectMember j on p.PurchaseManager = j.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode
                    left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                    left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                    left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                    left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                    left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                    left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode
                        where p.PurchaseEngineer = '{0}'", strUserCode);


        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortPurchaseCode.Value))
        {
            strPurchaseHQL += " order by p.PurchaseCode desc";

            HF_SortPurchaseCode.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by p.PurchaseCode asc";

            HF_SortPurchaseCode.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }



    protected void BT_SortProjectCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        //string strPurchaseHQL = string.Format(@"select p.*,
        //                e.UserName as PurchaseEngineerName,
        //                u.UserName as UpLeaderName,
        //                m.UserName as PurchaseManagerName,
        //                d.UserName as DisciplinarySupervisionName,
        //                c.UserName as ControlMoneyName,
        //                t.UserName as TenderCompetentName,
        //                s.UserName as DecisionName
        //                from T_WZPurchase p
        //                left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
        //                left join T_ProjectMember u on p.UpLeader = u.UserCode
        //                left join T_ProjectMember m on p.PurchaseManager = m.UserCode
        //                left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
        //                left join T_ProjectMember c on p.ControlMoney = c.UserCode
        //                left join T_ProjectMember t on p.TenderCompetent = t.UserCode
        //                left join T_ProjectMember s on p.Decision = s.UserCode
        //                where p.PurchaseEngineer = '{0}'", strUserCode);
        //string strSearchProgress = DDL_SearchProgress.SelectedValue;
        //if (!string.IsNullOrEmpty(strSearchProgress))
        //{
        //    strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        //}
        //string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchProjectCode))
        //{
        //    strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        //}
        //string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchPurchaseName))
        //{
        //    strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        //}

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                    e.UserName as PurchaseEngineerName,
                    t.UserName as TenderCompetentName,
                    c.UserName as ControlMoneyName,
                    d.UserName as DisciplinarySupervisionName,
                    e1.Name as ExpertCode1Name,
                    e2.Name as ExpertCode2Name,
                    e3.Name as ExpertCode3Name,
                    j.UserName as PurchaseManagerName,
                    s.UserName as DecisionName,
                    u.UserName as UpLeaderName,
                    s1.SupplierName as SupplierCode1Name,
                    s2.SupplierName as SupplierCode2Name,
                    s3.SupplierName as SupplierCode3Name,
                    s4.SupplierName as SupplierCode4Name,
                    s5.SupplierName as SupplierCode5Name,
                    s6.SupplierName as SupplierCode6Name
                    from T_WZPurchase p
                     left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                    left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                    left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                    left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode
                    left join T_ProjectMember j on p.PurchaseManager = j.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode
                    left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                    left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                    left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                    left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                    left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                    left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode
                        where p.PurchaseEngineer = '{0}'", strUserCode);


        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortProjectCode.Value))
        {
            strPurchaseHQL += " order by p.ProjectCode desc";

            HF_SortProjectCode.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by p.ProjectCode asc";

            HF_SortProjectCode.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }



    protected void BT_SortMarkTime_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        //string strPurchaseHQL = string.Format(@"select p.*,
        //                e.UserName as PurchaseEngineerName,
        //                u.UserName as UpLeaderName,
        //                m.UserName as PurchaseManagerName,
        //                d.UserName as DisciplinarySupervisionName,
        //                c.UserName as ControlMoneyName,
        //                t.UserName as TenderCompetentName,
        //                s.UserName as DecisionName
        //                from T_WZPurchase p
        //                left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
        //                left join T_ProjectMember u on p.UpLeader = u.UserCode
        //                left join T_ProjectMember m on p.PurchaseManager = m.UserCode
        //                left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
        //                left join T_ProjectMember c on p.ControlMoney = c.UserCode
        //                left join T_ProjectMember t on p.TenderCompetent = t.UserCode
        //                left join T_ProjectMember s on p.Decision = s.UserCode
        //                where p.PurchaseEngineer = '{0}'", strUserCode);
        //string strSearchProgress = DDL_SearchProgress.SelectedValue;
        //if (!string.IsNullOrEmpty(strSearchProgress))
        //{
        //    strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        //}
        //string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchProjectCode))
        //{
        //    strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        //}
        //string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchPurchaseName))
        //{
        //    strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        //}

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                    e.UserName as PurchaseEngineerName,
                    t.UserName as TenderCompetentName,
                    c.UserName as ControlMoneyName,
                    d.UserName as DisciplinarySupervisionName,
                    e1.Name as ExpertCode1Name,
                    e2.Name as ExpertCode2Name,
                    e3.Name as ExpertCode3Name,
                    j.UserName as PurchaseManagerName,
                    s.UserName as DecisionName,
                    u.UserName as UpLeaderName,
                    s1.SupplierName as SupplierCode1Name,
                    s2.SupplierName as SupplierCode2Name,
                    s3.SupplierName as SupplierCode3Name,
                    s4.SupplierName as SupplierCode4Name,
                    s5.SupplierName as SupplierCode5Name,
                    s6.SupplierName as SupplierCode6Name
                    from T_WZPurchase p
                     left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                    left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                    left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                    left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode
                    left join T_ProjectMember j on p.PurchaseManager = j.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode
                    left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                    left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                    left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                    left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                    left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                    left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode
                        where p.PurchaseEngineer = '{0}'", strUserCode);


        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortMarkTime.Value))
        {
            strPurchaseHQL += " order by p.MarkTime desc";

            HF_SortMarkTime.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by p.MarkTime asc";

            HF_SortMarkTime.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }

    private void ControlStatusChange(string objProgress, string objIsMark)
    {

        if (objProgress == "¼��")
        {
            BT_NewEdit.Enabled = true;
            BT_NewDetail.Visible = true;

            BT_NewNotApproval.Enabled = false;
            BT_NewCancel.Enabled = false;
            BT_NewNotCancel.Enabled = false;
        }
        else if (objProgress == "�ύ")
        {
            BT_NewEdit.Enabled = false;
            BT_NewDetail.Visible = false;

            BT_NewNotApproval.Enabled = true;

            BT_NewCancel.Enabled = false;
            BT_NewNotCancel.Enabled = false;
        }
        else if (objProgress == "����")
        {
            BT_NewEdit.Enabled = false;
            BT_NewDetail.Visible = false;
            BT_NewNotApproval.Enabled = false;

            BT_NewCancel.Enabled = true;

            BT_NewNotCancel.Enabled = false;
        }
        else if (objProgress == "����")
        {
            BT_NewEdit.Enabled = false;
            BT_NewDetail.Visible = false;
            BT_NewNotApproval.Enabled = false;
            BT_NewCancel.Enabled = false;

            BT_NewNotCancel.Enabled = true;
        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewDetail.Visible = false;
            BT_NewNotApproval.Enabled = false;
            BT_NewCancel.Enabled = false;
            BT_NewNotCancel.Enabled = false;
        }

        if (objProgress == "¼��" && objIsMark == "0")
        {
            BT_NewDelete.Enabled = true;               //ɾ��
        }
        else
        {
            BT_NewDelete.Enabled = false;
        }

        if (objProgress == "¼��" && objIsMark == "-1")
        {
            BT_NewApproval.Enabled = true;                //����
        }
        else
        {
            BT_NewApproval.Enabled = false;
        }

        BT_NewDetail.Visible = true;
    }



    private void ControlStatusCloseChange()
    {
        BT_NewEdit.Enabled = false;
        BT_NewDelete.Enabled = false;
        BT_NewDetail.Visible = true;
        BT_NewNotApproval.Enabled = false;
        BT_NewCancel.Enabled = false;
        BT_NewNotCancel.Enabled = false;
    }


}