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
using System.Text;
using System.Drawing;

public partial class TTWZAdvanceList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","Ԥ����ƻ�", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataAdvanceBinder();

            DataProjectBinder();
        }
    }

    private void DataAdvanceBinder()
    {
        DG_Advance.CurrentPageIndex = 0;

        string strWZAdvanceHQL = string.Format(@"select a.*,m.UserName as MarkerName from T_WZAdvance a
                    left join T_ProjectMember m on a.Marker = m.UserCode 
                    where a.Marker ='{0}' 
                    order by a.AdvanceTime desc", strUserCode);
        DataTable dtWZAdvance = ShareClass.GetDataSetFromSql(strWZAdvanceHQL, "Advance").Tables[0];

        DG_Advance.DataSource = dtWZAdvance;
        DG_Advance.DataBind();

        LB_AdvanceSql.Text = strWZAdvanceHQL;
    }


    private void DataProjectBinder()
    {
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject where PowerPurchase = '��' order by MarkTime desc";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        DDL_Project.DataSource = listProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void DG_Advance_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                //����
                for (int i = 0; i < DG_Advance.Items.Count; i++)
                {
                    DG_Advance.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditAdvanceCode = arrOperate[0];
                string strProgress = arrOperate[1];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);

                HF_NewAdvanceCode.Value = strEditAdvanceCode;
                HF_NewProgress.Value = strProgress;
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + cmdArges + "'";
                IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
                if (listWZAdvance != null && listWZAdvance.Count == 1)
                {
                    WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];
                    if (wZAdvance.Progress != "¼��" || wZAdvance.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBWLRYJSYBJBW0BYXSC+"')", true);
                        return;
                    }

                    wZAdvanceBLL.DeleteWZAdvance(wZAdvance);

                    //���¼����б�
                    DataAdvanceBinder();
                }

            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Advance.Items.Count; i++)
                {
                    DG_Advance.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + cmdArges + "'";
                IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
                if (listWZAdvance != null && listWZAdvance.Count == 1)
                {
                    WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

                    if (wZAdvance.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJBW0BYXBJ+"')", true);
                        return;
                    }

                    TXT_AdvanceCode.Text = wZAdvance.AdvanceCode;
                    DDL_Project.SelectedValue = wZAdvance.ProjectCode;
                    TXT_AdvanceName.Text = wZAdvance.AdvanceName;
                }
            }
            else if (cmdName == "request")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + cmdArges + "'";
                IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
                if (listWZAdvance != null && listWZAdvance.Count == 1)
                {
                    WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

                    if (wZAdvance.Progress != "¼��")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSLRZTBNBP+"')", true);
                        return;
                    }

                    wZAdvance.Progress = "����";

                    wZAdvanceBLL.UpdateWZAdvance(wZAdvance, wZAdvance.AdvanceCode);

                    //���¼���Ԥ�����б�
                    DataAdvanceBinder();
                }
            }
            else if (cmdName == "execute")
            {
                //ִ��
                string cmdArges = e.CommandArgument.ToString();
                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + cmdArges + "'";
                IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
                if (listWZAdvance != null && listWZAdvance.Count == 1)
                {
                    WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

                    if (wZAdvance.Progress != "��׼")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSLRZTBNBP+"')", true);
                        return;
                    }

                    //���1
                    bool IsCheck1 = false;
                    string strCheckHQL1 = string.Format(@"select c.* from T_WZAdvance  a
                        left join T_WZAdvanceDetail d on a.AdvanceCode = d.AdvanceCode
                        left join T_WZCompact c on d.ContractCode = c.CompactCode
                        where a.AdvanceCode = '{0}'", cmdArges);
                    DataTable dtCheck1 = ShareClass.GetDataSetFromSql(strCheckHQL1, "Check1").Tables[0];
                    if (dtCheck1 != null && dtCheck1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCheck1.Rows)
                        {
                            if (!"��Ч,�ļ�".Contains(ShareClass.ObjectToString(dr["Progress"])))
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYHTJDBSSXHCJXCL+"')", true);
                                return;
                            }
                            else
                            {
                                IsCheck1 = true;
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZBDXYDHTLB+"')", true);
                        return;
                    }

                    if (IsCheck1)
                    {
                        //���2
                        bool IsCheck2 = false;
                        string strCheckHQL2 = string.Format(@"select r.* from T_WZAdvance  a
                                    left join T_WZAdvanceDetail d on a.AdvanceCode = d.AdvanceCode
                                    left join T_WZRequest r on d.ContractCode = r.CompactCode
                                    where a.AdvanceCode = '{0}'", cmdArges);
                        DataTable dtCheck2 = ShareClass.GetDataSetFromSql(strCheckHQL2, "Check2").Tables[0];
                        if (dtCheck2 != null && dtCheck2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtCheck2.Rows)
                            {
                                if (!"����,���".Contains(ShareClass.ObjectToString(dr["Progress"])))
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYHTJDBSSXHCJXCL+"')", true);
                                    return;
                                }
                                else
                                {
                                    IsCheck2 = true;
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZBDXYDKDLB+"')", true);
                            return;
                        }

                        if (IsCheck2)
                        {
                            wZAdvance.Progress = "����";
                            wZAdvanceBLL.UpdateWZAdvance(wZAdvance, wZAdvance.AdvanceCode);

                            WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
                            string strWZAdvanceDetailHQL = string.Format(@"from WZAdvanceDetail as wZAdvanceDetail 
                                        where AdvanceCode= '{0}'", wZAdvance.AdvanceCode);
                            IList listWZAdvanceDetail = wZAdvanceDetailBLL.GetAllWZAdvanceDetails(strWZAdvanceDetailHQL);
                            if (listWZAdvanceDetail != null && listWZAdvanceDetail.Count > 0)
                            {
                                for (int i = 0; i < listWZAdvanceDetail.Count; i++)
                                {
                                    WZAdvanceDetail wZAdvanceDetail = (WZAdvanceDetail)listWZAdvanceDetail[i];

                                    //Ԥ����ϸ<Ԥ������> = "����"
                                    wZAdvanceDetail.PayProgress = "����";

                                    wZAdvanceDetailBLL.UpdateWZAdvanceDetail(wZAdvanceDetail, wZAdvanceDetail.ID);

                                    //��ͬ
                                    WZCompactBLL wZCompactBLL = new WZCompactBLL();
                                    string strWZCompactHQL = string.Format(@"from WZCompact as wZCompact
                                    where CompactCode = '{0}'", wZAdvanceDetail.ContractCode);
                                    IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
                                    if (listCompact != null && listCompact.Count > 0)
                                    {
                                        WZCompact wZCompact = (WZCompact)listCompact[0];

                                        wZCompact.BeforePayMoney = wZCompact.BeforePayMoney + wZAdvanceDetail.PayMoney;
                                        wZCompact.BeforePayBalance = wZCompact.BeforePayMoney;

                                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
                                    }
                                }
                            }

                            //���¼���Ԥ�����б�
                            DataAdvanceBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZXCG+"')", true);
                        }
                    }
                }
            }
        }
    }



    protected void DG_Advance_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Advance.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_AdvanceSql.Text.Trim();
        DataTable dtWZAdvance = ShareClass.GetDataSetFromSql(strHQL, "Advance").Tables[0];

        DG_Advance.DataSource = dtWZAdvance;
        DG_Advance.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void BT_Save_Click(object sender, EventArgs e)
    {
        try
        {
            string strAdvanceCode = TXT_AdvanceCode.Text;
            string strProjectCode = DDL_Project.SelectedValue;
            string strAdvanceName = TXT_AdvanceName.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��Ŀ���벻��Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            if (string.IsNullOrEmpty(strAdvanceName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Ԥ�������Ʋ���Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAdvanceName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Ԥ�������Ʋ���Ϊ�Ƿ��ַ���');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }


            if (!string.IsNullOrEmpty(strAdvanceCode))
            {
                //�޸�






                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + strAdvanceCode + "'";
                IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
                if (listWZAdvance != null && listWZAdvance.Count == 1)
                {
                    WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

                    wZAdvance.ProjectCode = strProjectCode;
                    wZAdvance.AdvanceName = strAdvanceName;

                    wZAdvanceBLL.UpdateWZAdvance(wZAdvance, wZAdvance.AdvanceCode);

                    //���¼���
                    DataAdvanceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true);
                }
            }
            else
            {
                //����


                WZAdvance wZAdvance = new WZAdvance();
                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                //����Ԥ����ID
                wZAdvance.AdvanceCode = CreateNewAdvanceCode();
                wZAdvance.ProjectCode = strProjectCode;
                wZAdvance.AdvanceName = strAdvanceName;
                wZAdvance.AdvanceTime = DateTime.Now;
                wZAdvance.Marker = strUserCode;
                wZAdvance.Progress = "¼��";

                wZAdvanceBLL.AddWZAdvance(wZAdvance);

                //���¼���
                DataAdvanceBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true);
            }
        }
        catch (Exception ex) { }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        try
        {
            string strAdvanceCode = TXT_AdvanceCode.Text;
            string strProjectCode = DDL_Project.SelectedValue;
            string strAdvanceName = TXT_AdvanceName.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��Ŀ���벻��Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            if (string.IsNullOrEmpty(strAdvanceName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Ԥ�������Ʋ���Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAdvanceName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Ԥ�������Ʋ���Ϊ�Ƿ��ַ���');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            //����
            WZAdvance wZAdvance = new WZAdvance();
            WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
            //����Ԥ����ID
            wZAdvance.AdvanceCode = CreateNewAdvanceCode();
            wZAdvance.ProjectCode = strProjectCode;
            wZAdvance.AdvanceName = strAdvanceName;
            wZAdvance.AdvanceTime = DateTime.Now;
            wZAdvance.Marker = strUserCode;
            wZAdvance.Progress = "¼��";

            wZAdvanceBLL.AddWZAdvance(wZAdvance);

            //���¼���
            DataAdvanceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�½��ɹ���');ControlStatusCloseChange();", true);
        }
        catch (Exception ex) { }
    }


    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        try
        {
            string strAdvanceCode = TXT_AdvanceCode.Text;
            string strProjectCode = DDL_Project.SelectedValue;
            string strAdvanceName = TXT_AdvanceName.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��Ŀ���벻��Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            if (string.IsNullOrEmpty(strAdvanceName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Ԥ�������Ʋ���Ϊ�գ��벹�䣡');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAdvanceName))
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Ԥ�������Ʋ���Ϊ�Ƿ��ַ���');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }


            if (!string.IsNullOrEmpty(strAdvanceCode))
            {
                //�޸�

                WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
                string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + strAdvanceCode + "'";
                IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
                if (listWZAdvance != null && listWZAdvance.Count == 1)
                {
                    WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

                    wZAdvance.ProjectCode = strProjectCode;
                    wZAdvance.AdvanceName = strAdvanceName;

                    wZAdvanceBLL.UpdateWZAdvance(wZAdvance, wZAdvance.AdvanceCode);

                    //���¼���
                    DataAdvanceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�޸ĳɹ���');ControlStatusCloseChange();", true);
                }
            }
            else
            {
                //����
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�޸ĵ�Ԥ���ƻ��б�');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }
        }
        catch (Exception ex) { }
    }


    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Advance.Items.Count; i++)
        {
            DG_Advance.Items[i].ForeColor = Color.Black;
        }

        TXT_AdvanceCode.Text = "";
        DDL_Project.SelectedValue = "";
        TXT_AdvanceName.Text = "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    /// <summary>
    ///  ����Ԥ����ID
    /// </summary>
    private string CreateNewAdvanceCode()
    {
        string strNewAdvanceCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strAdvanceCodeHQL = string.Format("select count(1) as RowNumber from T_WZAdvance where to_char( AdvanceTime, 'yyyy-mm-dd') like '{0}%'", DateTime.Now.ToString("yyyy-MM"));
                DataTable dtAdvanceCode = ShareClass.GetDataSetFromSql(strAdvanceCodeHQL, "AdvanceCode").Tables[0];
                int intAdvanceCodeNumber = int.Parse(dtAdvanceCode.Rows[0]["RowNumber"].ToString());
                intAdvanceCodeNumber = intAdvanceCodeNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbAdvanceCode = new StringBuilder();
                    for (int j = 3 - intAdvanceCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbAdvanceCode.Append(" ");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewAdvanceCode = strYear + "" + strMonth + "-" + sbAdvanceCode.ToString() + intAdvanceCodeNumber.ToString();

                    //��֤�µ�Ԥ����ID�Ƿ����
                    string strCheckNewAdvanceCodeHQL = "select count(1) as RowNumber from T_WZAdvance where AdvanceCode = '" + strNewAdvanceCode + "'";
                    DataTable dtCheckNewAdvanceCode = ShareClass.GetDataSetFromSql(strCheckNewAdvanceCodeHQL, "CheckNewAdvanceCode").Tables[0];
                    int intCheckNewAdvanceCode = int.Parse(dtCheckNewAdvanceCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewAdvanceCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intAdvanceCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewAdvanceCode;
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //�༭
        string strEditAdvanceCode = HF_NewAdvanceCode.Value;
        if (string.IsNullOrEmpty(strEditAdvanceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDYFKLB+"')", true);
            return;
        }

        WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
        string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + strEditAdvanceCode + "'";
        IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);

        string strNewProgress = HF_NewProgress.Value;

        if (listWZAdvance != null && listWZAdvance.Count == 1)
        {
            WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

            if (wZAdvance.IsMark != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYBJBW0BYXBJ+"');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            TXT_AdvanceCode.Text = wZAdvance.AdvanceCode;
            DDL_Project.SelectedValue = wZAdvance.ProjectCode;
            TXT_AdvanceName.Text = wZAdvance.AdvanceName;
        }

        
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
    }



    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //ɾ��
        string strEditAdvanceCode = HF_NewAdvanceCode.Value;
        if (string.IsNullOrEmpty(strEditAdvanceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDYFKLB+"')", true);
            return;
        }

        WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
        string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + strEditAdvanceCode + "'";
        IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
        if (listWZAdvance != null && listWZAdvance.Count == 1)
        {
            WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];
            if (wZAdvance.Progress != "¼��" || wZAdvance.IsMark != 0)
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBWLRYJSYBJBW0BYXSC+"');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            wZAdvanceBLL.DeleteWZAdvance(wZAdvance);

            //���¼����б�
            DataAdvanceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }



    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        //��ϸ
        string strEditAdvanceCode = HF_NewAdvanceCode.Value;
        if (string.IsNullOrEmpty(strEditAdvanceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDYFKLB+"')", true);
            return;
        }

        Response.Redirect("TTWZAdvanceDetail.aspx?AdvanceCode=" + strEditAdvanceCode);
    }



    protected void BT_NewReport_Click(object sender, EventArgs e)
    {
        //����
        string strEditAdvanceCode = HF_NewAdvanceCode.Value;
        if (string.IsNullOrEmpty(strEditAdvanceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDYFKLB+"')", true);
            return;
        }

        WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
        string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + strEditAdvanceCode + "'";
        IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
        if (listWZAdvance != null && listWZAdvance.Count == 1)
        {
            WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

            if (wZAdvance.Progress != "¼��")
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSLRZTBNBP+"');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            wZAdvance.Progress = "����";

            wZAdvanceBLL.UpdateWZAdvance(wZAdvance, wZAdvance.AdvanceCode);

            //���¼���Ԥ�����б�
            DataAdvanceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }



    protected void BT_NewExecute_Click(object sender, EventArgs e)
    {
        //ִ��
        string strEditAdvanceCode = HF_NewAdvanceCode.Value;
        if (string.IsNullOrEmpty(strEditAdvanceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJYCZDYFKLB+"')", true);
            return;
        }

        WZAdvanceBLL wZAdvanceBLL = new WZAdvanceBLL();
        string strWZAdvanceHQL = "from WZAdvance as wZAdvance where AdvanceCode = '" + strEditAdvanceCode + "'";
        IList listWZAdvance = wZAdvanceBLL.GetAllWZAdvances(strWZAdvanceHQL);
        if (listWZAdvance != null && listWZAdvance.Count == 1)
        {
            WZAdvance wZAdvance = (WZAdvance)listWZAdvance[0];

            if (wZAdvance.Progress != "��׼")
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBSLRZTBNBP+"');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            //���1
            bool IsCheck1 = false;
            string strCheckHQL1 = string.Format(@"select c.* from T_WZAdvance  a
                        left join T_WZAdvanceDetail d on a.AdvanceCode = d.AdvanceCode
                        left join T_WZCompact c on d.ContractCode = c.CompactCode
                        where a.AdvanceCode = '{0}'", strEditAdvanceCode);
            DataTable dtCheck1 = ShareClass.GetDataSetFromSql(strCheckHQL1, "Check1").Tables[0];
            if (dtCheck1 != null && dtCheck1.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCheck1.Rows)
                {
                    if (!"��Ч,�ļ�".Contains(ShareClass.ObjectToString(dr["Progress"])))
                    {
                        string strNewProgress = HF_NewProgress.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYHTJDBSSXHCJXCL+"');ControlStatusChange('" + strNewProgress + "');", true);
                        return;
                    }
                    else
                    {
                        IsCheck1 = true;
                    }
                }
            }
            else
            {
                string strNewProgress = HF_NewProgress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZBDXYDHTLB+"');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            if (IsCheck1)
            {
                //���2
                bool IsCheck2 = false;
                string strCheckHQL2 = string.Format(@"select r.* from T_WZAdvance  a
                                    left join T_WZAdvanceDetail d on a.AdvanceCode = d.AdvanceCode
                                    left join T_WZRequest r on d.ContractCode = r.CompactCode
                                    where a.AdvanceCode = '{0}'", strEditAdvanceCode);
                DataTable dtCheck2 = ShareClass.GetDataSetFromSql(strCheckHQL2, "Check2").Tables[0];
                if (dtCheck2 != null && dtCheck2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCheck2.Rows)
                    {
                        if (!"����,���".Contains(ShareClass.ObjectToString(dr["Progress"])))
                        {
                            string strNewProgress = HF_NewProgress.Value;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYHTJDBSSXHCJXCL+"');ControlStatusChange('" + strNewProgress + "');", true);
                            return;
                        }
                        else
                        {
                            IsCheck2 = true;
                        }
                    }
                }
                else
                {
                    string strNewProgress = HF_NewProgress.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZBDXYDKDLB+"');ControlStatusChange('" + strNewProgress + "');", true);
                    return;
                }

                if (IsCheck2)
                {
                    wZAdvance.Progress = "����";
                    wZAdvanceBLL.UpdateWZAdvance(wZAdvance, wZAdvance.AdvanceCode);

                    WZAdvanceDetailBLL wZAdvanceDetailBLL = new WZAdvanceDetailBLL();
                    string strWZAdvanceDetailHQL = string.Format(@"from WZAdvanceDetail as wZAdvanceDetail 
                                        where AdvanceCode= '{0}'", wZAdvance.AdvanceCode);
                    IList listWZAdvanceDetail = wZAdvanceDetailBLL.GetAllWZAdvanceDetails(strWZAdvanceDetailHQL);
                    if (listWZAdvanceDetail != null && listWZAdvanceDetail.Count > 0)
                    {
                        for (int i = 0; i < listWZAdvanceDetail.Count; i++)
                        {
                            WZAdvanceDetail wZAdvanceDetail = (WZAdvanceDetail)listWZAdvanceDetail[i];

                            //Ԥ����ϸ<Ԥ������> = "����"
                            wZAdvanceDetail.PayProgress = "����";

                            wZAdvanceDetailBLL.UpdateWZAdvanceDetail(wZAdvanceDetail, wZAdvanceDetail.ID);

                            //��ͬ
                            WZCompactBLL wZCompactBLL = new WZCompactBLL();
                            string strWZCompactHQL = string.Format(@"from WZCompact as wZCompact
                                    where CompactCode = '{0}'", wZAdvanceDetail.ContractCode);
                            IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
                            if (listCompact != null && listCompact.Count > 0)
                            {
                                WZCompact wZCompact = (WZCompact)listCompact[0];

                                wZCompact.BeforePayMoney = wZCompact.BeforePayMoney + wZAdvanceDetail.PayMoney;
                                wZCompact.BeforePayBalance = wZCompact.BeforePayMoney;

                                wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);
                            }
                        }
                    }

                    //���¼���Ԥ�����б�
                    DataAdvanceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZXCG+"');ControlStatusCloseChange();", true);
                }
            }
        }
    }

}