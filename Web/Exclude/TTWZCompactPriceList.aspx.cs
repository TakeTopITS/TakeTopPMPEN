using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Drawing;
using System.Data;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZCompactPriceList : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode 
                        where (c.ControlMoney = '{0}' or c.DelegateAgent = '{0}' or c.JuridicalPerson = '{0}') ", strUserCode);

        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != "全部")
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }
        strCompactHQL += " order by c.MarkTime desc";

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();
    }

    protected void DDL_WhereProgress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
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
            if (cmdName == "click")
            {
                //操作
                string cmdArges = e.CommandArgument.ToString();

                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];

                    ControlStatusChange(wZCompact.ControlMoney, wZCompact.DelegateAgent, wZCompact.JuridicalPerson, wZCompact.CompactMoney, wZCompact.Progress, wZCompact.IsMark.ToString());

                    HF_NewCompactCode.Value = wZCompact.CompactCode;
                    HF_ControlMoney.Value = wZCompact.ControlMoney;
                    HF_DelegateAgent.Value = wZCompact.DelegateAgent;
                    HF_JuridicalPerson.Value = wZCompact.JuridicalPerson;
                    HF_CompactMoney.Value = wZCompact.CompactMoney.ToString();
                    HF_Progress.Value = wZCompact.Progress;
                    HF_IsMark.Value = wZCompact.IsMark.ToString();
                }
            }
            else if (cmdName == "audit")
            {
                //审核
                string cmdArges = e.CommandArgument.ToString();
                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    if (wZCompact.Progress == "草签")
                    {
                        wZCompact.Progress = "审核";
                        wZCompact.VerifyTime = DateTime.Now.ToString("yyyy-MM-dd");

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSHCG + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHTJDBSCZTBNSH + "')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "notAudit")
            {
                //审核退回
                string cmdArges = e.CommandArgument.ToString();
                WZCompactBLL wZCompactBLL = new WZCompactBLL();
                string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + cmdArges + "'";
                IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
                if (listCompact != null && listCompact.Count == 1)
                {
                    WZCompact wZCompact = (WZCompact)listCompact[0];
                    if (wZCompact.Progress == "审核")
                    {
                        wZCompact.Progress = "草签";
                        wZCompact.VerifyTime = "-";

                        wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSHTHCG + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHTJDBSSHZTBNSHTH + "')", true);
                        return;
                    }
                }
            }
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strCompactHQL = LB_Sql.Text;
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    protected void BT_NewCompactDetail_Click(object sender, EventArgs e)
    {
        //合同明细
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDHTLB + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZCompactDetail.aspx?CompactCode=" + strEditCompactCode + "');", true);
        return;
    }

    protected void BT_NewAudit_Click(object sender, EventArgs e)
    {
        //审核
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDHTLB + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];

            wZCompact.VerifyTime = DateTime.Now.ToString("yyyy-MM-dd");
            wZCompact.Progress = "审核";

            wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSHCG + "');ControlStatusCloseChange();", true);
        }
    }

    protected void BT_NewAuditReturn_Click(object sender, EventArgs e)
    {
        //审核退回
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDHTLB + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];

            wZCompact.VerifyTime = "-";
            wZCompact.Progress = "草签";

            wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSHTHCG + "');ControlStatusCloseChange();", true);
        }
    }

    protected void BT_NewApprove_Click(object sender, EventArgs e)
    {
        //批准
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDHTLB + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];

            //       合同〈生效日期〉＝系统日期												
            //合同〈进度〉＝“生效”												
            //计划明细〈合同编号〉＝合同明细〈合同编号〉												
            //计划明细〈进度〉＝“合同”												
            //采购清单〈进度〉＝“合同”												
            //物资代码〈市场行情〉＝合同明细〈合同单价〉												
            //物资代码〈采集日期〉＝合同〈生效日期〉												
            //工程项目〈合同金额〉＝原值＋合同〈合同金额〉			

            wZCompact.EffectTime = DateTime.Now.ToString("yyyy-MM-dd");
            wZCompact.Progress = "生效";


            wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);


            ContractEffect(wZCompact.CompactCode);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('删除成功！');ControlStatusCloseChange();", true);
        }
    }

    protected void BT_NewApproveReturn_Click(object sender, EventArgs e)
    {
        //批准退回
        string strEditCompactCode = HF_NewCompactCode.Value;
        if (string.IsNullOrEmpty(strEditCompactCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDHTLB + "')", true);
            return;
        }

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strCompactSql = "from WZCompact as wZCompact where CompactCode = '" + strEditCompactCode + "'";
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strCompactSql);
        if (listCompact != null && listCompact.Count == 1)
        {
            WZCompact wZCompact = (WZCompact)listCompact[0];

            wZCompact.EffectTime = "-";
            wZCompact.Progress = "审核";

            wZCompactBLL.UpdateWZCompact(wZCompact, wZCompact.CompactCode);

            ContractEffectCancel(wZCompact.CompactCode);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('删除成功！');ControlStatusCloseChange();", true);
        }
    }

    protected void BT_SortCompactCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode 
                        where (c.ControlMoney = '{0}' or c.DelegateAgent = '{0}' or c.JuridicalPerson = '{0}') ", strUserCode);

        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != "全部")
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortCompactCode.Value))
        {
            strCompactHQL += " order by c.CompactCode desc";

            HF_SortCompactCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.CompactCode asc";

            HF_SortCompactCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();
    }

    protected void BT_SortProjectCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode 
                        where (c.ControlMoney = '{0}' or c.DelegateAgent = '{0}' or c.JuridicalPerson = '{0}') ", strUserCode);

        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != "全部")
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortProjectCode.Value))
        {
            strCompactHQL += " order by c.ProjectCode desc";

            HF_SortProjectCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.ProjectCode asc";

            HF_SortProjectCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();
    }

    protected void BT_SortSupplierCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode 
                        where (c.ControlMoney = '{0}' or c.DelegateAgent = '{0}' or c.JuridicalPerson = '{0}') ", strUserCode);

        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != "全部")
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortSupplierCode.Value))
        {
            strCompactHQL += " order by c.SupplierCode desc";

            HF_SortSupplierCode.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.SupplierCode asc";

            HF_SortSupplierCode.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();
    }

    protected void BT_SortSingTime_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strCompactHQL = string.Format(@"select c.*,s.SupplierName,
                        p.UserName as PurchaseEngineerName,
                        m.UserName as ControlMoneyName,
                        j.UserName as JuridicalPersonName,
                        d.UserName as DelegateAgentName,
                        t.UserName as CompacterName,
                        k.UserName as SafekeepName,
                        h.UserName as CheckerName
                        from T_WZCompact c
                        left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                        left join T_ProjectMember p on c.PurchaseEngineer = p.UserCode
                        left join T_ProjectMember m on c.ControlMoney = m.UserCode
                        left join T_ProjectMember j on c.JuridicalPerson = j.UserCode
                        left join T_ProjectMember d on c.DelegateAgent = d.UserCode
                        left join T_ProjectMember t on c.Compacter = t.UserCode
                        left join T_ProjectMember k on c.Safekeep = k.UserCode
                        left join T_ProjectMember h on c.Checker = h.UserCode 
                        where (c.ControlMoney = '{0}' or c.DelegateAgent = '{0}' or c.JuridicalPerson = '{0}') ", strUserCode);

        string strProgress = DDL_WhereProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != "全部")
        {
            strCompactHQL += " and c.Progress = '" + strProgress + "'";
        }
        string strProjectCode = TXT_WhereProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCompactHQL += " and c.ProjectCode like '%" + strProjectCode + "%'";
        }
        string strCompactName = TXT_WhereCompactName.Text.Trim();
        if (!string.IsNullOrEmpty(strCompactName))
        {
            strCompactHQL += " and c.CompactName like '%" + strCompactName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortSingTime.Value))
        {
            strCompactHQL += " order by c.SingTime desc";

            HF_SortSingTime.Value = "";
        }
        else
        {
            strCompactHQL += " order by c.SingTime asc";

            HF_SortSingTime.Value = "asc";
        }

        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DG_List.DataSource = dtCompact;
        DG_List.DataBind();

        LB_Sql.Text = strCompactHQL;

        LB_RecordCount.Text = dtCompact.Rows.Count.ToString();

    }

    private void ControlStatusChange(string strControlMoney, string strDelegateAgent, string strJuridicalPerson, decimal decimalCompactMoney, string strProgress, string strIsMark)
    {
        if (strControlMoney.Trim() == strUserCode.Trim() && strProgress == "草签")
        {
            BT_NewAudit.Enabled = true;
            BT_NewAuditReturn.Enabled = false;

        }
        else if (strControlMoney.Trim() == strUserCode.Trim() && strProgress == "审核")
        {
            BT_NewAudit.Enabled = false;
            BT_NewAuditReturn.Enabled = true;

        }
        else
        {
            BT_NewAudit.Enabled = false;
            BT_NewAuditReturn.Enabled = false;

        }


        if (strDelegateAgent.Trim() == strUserCode.Trim() && strProgress == "审核" && decimalCompactMoney < 300000)
        {
            BT_NewApprove.Enabled = true;
            BT_NewApproveReturn.Enabled = false;
        }
        else if (strDelegateAgent.Trim() == strUserCode.Trim() && strProgress == "审核" && decimalCompactMoney >= 300000)
        {
            BT_NewApprove.Enabled = false;
            BT_NewApproveReturn.Enabled = true;
        }
        else
        {
            BT_NewApprove.Enabled = false;
            BT_NewApproveReturn.Enabled = false;
        }

        if (strControlMoney.Trim() == strUserCode.Trim() && strProgress == "草签")
        {
            BT_NewCompactDetail.Enabled = true;
        }
        else if (strDelegateAgent.Trim() == strUserCode.Trim() && strProgress == "审核" && decimalCompactMoney < 300000)
        {
            BT_NewCompactDetail.Enabled = true;
        }
        else if (strDelegateAgent.Trim() == strUserCode.Trim() && strProgress == "审核" && decimalCompactMoney >= 300000)
        {
            BT_NewCompactDetail.Enabled = true;
        }
        else
        {
            BT_NewCompactDetail.Enabled = false;
        }
    }



    private void ControlStatusCloseChange()
    {
        BT_NewAudit.Enabled = false;
        BT_NewAuditReturn.Enabled = false;

        BT_NewApprove.Enabled = false;
        BT_NewApproveReturn.Enabled = false;
    }





    /// <summary>
    ///  合同生效
    /// </summary>
    private void ContractEffect(string strCompactCode)
    {
        //当合同〈进度〉＝“生效”后，写记录												
        //计划明细〈合同编号〉＝合同明细〈合同编号〉												
        //计划明细〈进度〉＝“合同”												
        //采购清单〈进度〉＝“合同”												
        //物资代码〈市场行情〉＝合同明细〈合同单价〉												
        //物资代码〈采集日期〉＝合同〈生效日期〉												
        //工程项目〈合同金额〉＝原值＋合同〈合同金额〉	

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strWZCompactHQL = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
        IList listWZCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
        if (listWZCompact != null && listWZCompact.Count > 0)
        {
            WZCompact wZCompact = (WZCompact)listWZCompact[0];

            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailSql = "from WZCompactDetail as wZCompactDetail where CompactCode = '" + strCompactCode + "'";
            IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailSql);
            if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
            {
                for (int i = 0; i < listWZCompactDetail.Count; i++)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[i];

                    //采购清单
                    WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                    string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where ID = " + wZCompactDetail.PurchaseDetailID;
                    IList lstWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                    if (lstWZPurchaseDetail != null && lstWZPurchaseDetail.Count > 0)
                    {
                        WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)lstWZPurchaseDetail[0];
                        wZPurchaseDetail.Progress = "合同";

                        wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);

                        //计划明细
                        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                        string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + wZPurchaseDetail.PlanDetailID;
                        IList lstWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                        if (lstWZPickingPlanDetail != null && lstWZPickingPlanDetail.Count > 0)
                        {
                            WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)lstWZPickingPlanDetail[0];
                            wZPickingPlanDetail.ContractCode = strCompactCode;
                            wZPickingPlanDetail.Progress = "合同";

                            wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);
                        }
                    }
                    //物资代码
                    WZObjectBLL wZObjectBLL = new WZObjectBLL();
                    string strWZObjectHQL = "from WZObject as wZObject where ObjectCode = '" + wZCompactDetail.ObjectCode + "'";
                    IList lstWZObject = wZObjectBLL.GetAllWZObjects(strWZObjectHQL);
                    if (lstWZObject != null && lstWZObject.Count > 0)
                    {
                        WZObject wZObject = (WZObject)lstWZObject[0];
                        wZObject.Market = wZCompactDetail.CompactPrice;
                        DateTime dtEffectTime = DateTime.Now;
                        DateTime.TryParse(wZCompact.EffectTime, out dtEffectTime);
                        wZObject.CollectTime = dtEffectTime;

                        wZObjectBLL.UpdateWZObject(wZObject, wZObject.ObjectCode);
                    }
                }
            }
            //工程项目
            WZProjectBLL wZProjectBLL = new WZProjectBLL();
            string strWZProjectHQL = "from WZProject as wZProject where ProjectCode = '" + wZCompact.ProjectCode + "'";
            IList lstWZProject = wZProjectBLL.GetAllWZProjects(strWZProjectHQL);
            if (lstWZProject != null && lstWZProject.Count > 0)
            {
                WZProject wZProject = (WZProject)lstWZProject[0];
                wZProject.ContractMoney += wZCompact.CompactMoney;

                wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
            }
        }
    }


    /// <summary>
    ///  合同生效取消
    /// </summary>
    private void ContractEffectCancel(string strCompactCode)
    {
        //计划明细〈合同编号〉＝“-”												
        //计划明细〈进度〉＝“决策”												
        //采购清单〈进度〉＝“决策”												
        //工程项目〈合同金额〉＝原值－合同〈合同金额〉												

        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strWZCompactHQL = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
        IList listWZCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
        if (listWZCompact != null && listWZCompact.Count > 0)
        {
            WZCompact wZCompact = (WZCompact)listWZCompact[0];

            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailSql = "from WZCompactDetail as wZCompactDetail where CompactCode = '" + strCompactCode + "'";
            IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailSql);
            if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
            {
                for (int i = 0; i < listWZCompactDetail.Count; i++)
                {
                    WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[i];

                    //采购清单
                    WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                    string strWZPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where ID = " + wZCompactDetail.PurchaseDetailID;
                    IList lstWZPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strWZPurchaseDetailHQL);
                    if (lstWZPurchaseDetail != null && lstWZPurchaseDetail.Count > 0)
                    {
                        WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)lstWZPurchaseDetail[0];
                        wZPurchaseDetail.Progress = "决策";

                        wZPurchaseDetailBLL.UpdateWZPurchaseDetail(wZPurchaseDetail, wZPurchaseDetail.ID);

                        //计划明细
                        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                        string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + wZPurchaseDetail.PlanDetailID;
                        IList lstWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                        if (lstWZPickingPlanDetail != null && lstWZPickingPlanDetail.Count > 0)
                        {
                            WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)lstWZPickingPlanDetail[0];
                            wZPickingPlanDetail.ContractCode = "-";
                            wZPickingPlanDetail.Progress = "决策";

                            wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, wZPickingPlanDetail.ID);
                        }
                    }
                }
            }
            //工程项目
            WZProjectBLL wZProjectBLL = new WZProjectBLL();
            string strWZProjectHQL = "from WZProject as wZProject where ProjectCode = '" + wZCompact.ProjectCode + "'";
            IList lstWZProject = wZProjectBLL.GetAllWZProjects(strWZProjectHQL);
            if (lstWZProject != null && lstWZProject.Count > 0)
            {
                WZProject wZProject = (WZProject)lstWZProject[0];
                wZProject.ContractMoney -= wZCompact.CompactMoney;

                wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
            }
        }
    }


}