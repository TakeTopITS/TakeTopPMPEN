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

public partial class TTWZPurchaseUpLeadList : System.Web.UI.Page
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

            TXT_SearchProjectCode.BackColor = Color.CornflowerBlue;
            TXT_SearchPurchaseName.BackColor = Color.CornflowerBlue;
        }
    }

    private void DataBinder()
    {
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

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

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
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.UpLeader = '{0}' or p.PurchaseManager = '{0}' or p.TenderCompetent = '{0}')


                        and p.Progress in ('Submit','上报','Approved')", strUserCode);   //ChineseWord
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

        //ControlStatusCloseChange();
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

                if (cmdName == "click")
                {
                    string strProgress = wZPurchase.Progress;

                    //ControlStatusChange(wZPurchase.PurchaseManager.Trim(), wZPurchase.Decision.Trim(), wZPurchase.UpLeader, strProgress, wZPurchase.PlanMoney);


                    HF_NewPurchaseCode.Value = wZPurchase.PurchaseCode;
                    HF_NewPurchaseManager.Value = wZPurchase.PurchaseManager;
                    HF_NewDecision.Value = wZPurchase.Decision;
                    HF_NewUpLeader.Value = wZPurchase.UpLeader;
                    HF_NewProgress.Value = strProgress;
                    HF_NewPlanMoney.Value = wZPurchase.PlanMoney.ToString();
                }
                else if (cmdName == "approval")
                {
                    //批准
                    if (wZPurchase.Progress == LanguageHandle.GetWord("ChengBao").ToString().Trim())
                    {
                        wZPurchase.Progress = "Approved";

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSCBBNPZ").ToString().Trim() + "')", true);
                        return;
                    }
                }
                else if (cmdName == "notApproval")
                {
                    //退回
                    if (wZPurchase.Progress == LanguageHandle.GetWord("ChengBao").ToString().Trim())
                    {
                        wZPurchase.Progress = LanguageHandle.GetWord("BaoPi").ToString().Trim();

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTHCG").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSCBBNTHBP").ToString().Trim() + "')", true);
                        return;
                    }
                }
            }
        }
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




    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //修改
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseUpLeadListEdit.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
        return;
    }


    protected void BT_NewApproval_Click(object sender, EventArgs e)
    {
        //批准
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = "Approved";

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            BT_NewApproval.Enabled = false;
            BT_NewApprovalReturn.Enabled = true;

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPZCG").ToString().Trim() + "');", true);
        }
    }



    protected void BT_NewApprovalReturn_Click(object sender, EventArgs e)
    {
        //批准退回
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("ShangBao").ToString().Trim();

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            BT_NewApproval.Enabled = true;
            BT_NewApprovalReturn.Enabled = false;

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('批准" + LanguageHandle.GetWord("ZZTHCG").ToString().Trim() + "');", true);   //ChineseWord
        }
    }


    protected void BT_NewRepetition_Click(object sender, EventArgs e)
    {
        //重报
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("DiJiao").ToString().Trim();

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('重报成功！');", true);   //ChineseWord
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
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();

    }




    protected void BT_SortPurchaseCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

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
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.UpLeader = '{0}' or p.PurchaseManager = '{0}' or p.TenderCompetent = '{0}')


                        and p.Progress in ('Submit','上报','Approved')", strUserCode);   //ChineseWord
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

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

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
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.UpLeader = '{0}' or p.PurchaseManager = '{0}' or p.TenderCompetent = '{0}')


                        and p.Progress in ('Submit','上报','Approved')", strUserCode);   //ChineseWord
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



    protected void BT_SortPurchaseStartTime_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

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
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.UpLeader = '{0}' or p.PurchaseManager = '{0}' or p.TenderCompetent = '{0}')


                        and p.Progress in ('Submit','上报','Approved')", strUserCode);   //ChineseWord
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

        if (!string.IsNullOrEmpty(HF_SortPurchaseStartTime.Value))
        {
            strPurchaseHQL += " order by p.PurchaseStartTime desc";

            HF_SortPurchaseStartTime.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by p.PurchaseStartTime asc";

            HF_SortPurchaseStartTime.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }



    private void ControlStatusChange(string objPurchaseManager, string objDecision, string objUpLeader, string objProgress, decimal objPlanMoney)
    {

        if (((objPurchaseManager == strUserCode || objDecision == strUserCode) && objProgress == LanguageHandle.GetWord("ShangBao").ToString().Trim() && objPlanMoney < 300000) || ((objUpLeader == null ? "" : objUpLeader.Trim()) == strUserCode && objProgress == LanguageHandle.GetWord("ShangBao").ToString().Trim() && objPlanMoney >= 300000))
        {

            BT_NewEdit.Enabled = true;
            BT_NewApproval.Enabled = true;
            BT_NewApprovalReturn.Enabled = false;
            BT_NewRepetition.Enabled = true;


        }
        else if (((objPurchaseManager == strUserCode || objDecision == strUserCode) && objProgress == "Approved" && objPlanMoney < 300000) || ((objUpLeader == null ? "" : objUpLeader.Trim()) == strUserCode && objProgress == "Approved" && objPlanMoney >= 300000))
        {

            BT_NewEdit.Enabled = false;
            BT_NewApproval.Enabled = false;
            BT_NewApprovalReturn.Enabled = true;
            BT_NewRepetition.Enabled = false;


        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewApproval.Enabled = false;
            BT_NewApprovalReturn.Enabled = false;
            BT_NewRepetition.Enabled = false;
        }


    }



    private void ControlStatusCloseChange()
    {
        BT_NewEdit.Enabled = false;
        BT_NewApproval.Enabled = false;
        BT_NewApprovalReturn.Enabled = false;

    }



}
