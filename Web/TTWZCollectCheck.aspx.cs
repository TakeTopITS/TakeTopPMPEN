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

public partial class TTWZCollectCheck : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
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

    private void DataCollectBinder()
    {
        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Checker ='{0}' 
                            and c.Progress in ('材检','开票') 
                            ", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }

        strCollectHQL += "  order by c.TicketTime desc";


        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        LB_CheckRecord.Text = dtCollect.Rows.Count.ToString();
    }

    protected void DG_Collect_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;

        if (cmdName == "select")
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('DDDDDD')", true);
            ////开票
            //string strCompactCode, strCheckCode, strChecker;
            //string cmdArges = e.CommandArgument.ToString();
            //WZCollectBLL wZCollectBLL = new WZCollectBLL();
            //string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
            //IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            //if (listCollect != null && listCollect.Count == 1)
            //{
            //    WZCollect wZCollect = (WZCollect)listCollect[0];

            //    strCheckCode = wZCollect.CheckCode.Trim();
            //    strChecker = wZCollect.Checker.Trim();

            //    DataCompactCheckBinder(strCheckCode, strChecker);
            //}

            //开票
            string cmdArges = e.CommandArgument.ToString();
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];
                if (wZCollect.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
                {
                   string strCheckCode = wZCollect.CheckCode.Trim();
                   string strChecker = wZCollect.Checker.Trim();

                    try
                    {
                        string strCompactCheckHQL = "select c.*,s.UnitName,d.UserName as CheckerName from T_WZCompactCheck c";
                        strCompactCheckHQL += " left join T_ProjectMember d on c.Checker = d.UserCode";
                        strCompactCheckHQL += " left join T_WZSpan s on c.Unit = s.ID";
                        strCompactCheckHQL += " where c.Checker = '" + strChecker + "' and c.CheckCode = '" + strCheckCode + "'";

                        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactCheckHQL, "Compact").Tables[0];

                        DataGrid1.DataSource = dtCompact;
                        DataGrid1.DataBind();

                        LB_CheckCodeRecord.Text = dtCompact.Rows.Count.ToString();
                        LB_CompactDetailSql.Text = strCompactCheckHQL;
                    }
                    catch (Exception err)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + err.Message .ToString() + "')", true);
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWCJBNKP").ToString().Trim() + "')", true);
                    return;
                }
            }
        }

        else if (cmdName == "ticket")
        {
            //开票
            string cmdArges = e.CommandArgument.ToString();
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];
                if (wZCollect.Progress == LanguageHandle.GetWord("CaiJian").ToString().Trim())
                {
                    wZCollect.Progress = LanguageHandle.GetWord("KaiPiao").ToString().Trim();
                    wZCollect.CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //重新加载收料单列表
                    DataCollectBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWCJBNKP").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
        else if (cmdName == "notTicket")
        {
            //退回
            string cmdArges = e.CommandArgument.ToString();
            WZCollectBLL wZCollectBLL = new WZCollectBLL();
            string strCollectHQL = "from WZCollect as wZCollect where CollectCode = '" + cmdArges + "'";
            IList listCollect = wZCollectBLL.GetAllWZCollects(strCollectHQL);
            if (listCollect != null && listCollect.Count == 1)
            {
                WZCollect wZCollect = (WZCollect)listCollect[0];
                if (wZCollect.Progress == LanguageHandle.GetWord("KaiPiao").ToString().Trim())
                {
                    wZCollect.Progress = LanguageHandle.GetWord("CaiJian").ToString().Trim();
                    wZCollect.CheckTime = "";

                    wZCollectBLL.UpdateWZCollect(wZCollect, wZCollect.CollectCode);

                    //重新加载收料单列表
                    DataCollectBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPTHCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWCJBNTH").ToString().Trim() + "')", true);
                    return;
                }
            }
        }
    }

    private void DataCompactCheckBinder(string strCheckCode,string strChecker)
    {
        //string strCompactCheckHQL = string.Format(@"select c.*,s.UnitName,d.UserName as CheckerName from T_WZCompactCheck c
        //            left join T_ProjectMember d on c.Checker = d.UserCode
        //            left join T_WZSpan s on c.Unit = s.ID
        //            where c.Checker = '{0}' and c.CheckCode ='{1}'" ,strChecker , strCheckCode);

        string strCompactCheckHQL = "select c.*,s.UnitName,d.UserName as CheckerName from T_WZCompactCheck c";
        strCompactCheckHQL += " left join T_ProjectMember d on c.Checker = d.UserCode";
        strCompactCheckHQL += " left join T_WZSpan s on c.Unit = s.ID";
        strCompactCheckHQL += " where c.Checker = '" + strChecker + "' and c.CheckCode = '" + strCheckCode + "'";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +strCompactCheckHQL + "')", true);

        //DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactCheckHQL, "Compact").Tables[0];

        //DataGrid1.DataSource = dtCompact;
        //DataGrid1.DataBind();

        //LB_CompactDetailSql.Text = strCompactCheckHQL;

        //LB_CheckCodeRecord.Text = dtCompact.Rows.Count.ToString();
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strCompactHQL = LB_CompactDetailSql.Text;
        DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];

        DataGrid1.DataSource = dtCompact;
        DataGrid1.DataBind();
    }


    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataCollectBinder();
    }

    protected void BT_SortCollectCode_Click(object sender, EventArgs e)
    {
        DG_Collect.CurrentPageIndex = 0;

        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Checker ='{0}' 
                            and c.Progress in ('材检','开票') 
                            ", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }

        if (!string.IsNullOrEmpty(HF_SortCollectCode.Value))
        {
            strCollectHQL += " order by c.CollectCode desc";

            HF_SortCollectCode.Value = "";
        }
        else
        {
            strCollectHQL += " order by c.CollectCode asc";

            HF_SortCollectCode.Value = "asc";
        }

        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        LB_CollectSql.Text = strCollectHQL;
    }

    protected void BT_SortTicketTime_Click(object sender, EventArgs e)
    {
        DG_Collect.CurrentPageIndex = 0;

        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Checker ='{0}' 
                            and c.Progress in ('材检','开票') 
                            ", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }

        if (!string.IsNullOrEmpty(HF_SortTicketTime.Value))
        {
            strCollectHQL += "  order by c.TicketTime desc";

            HF_SortTicketTime.Value = "";
        }
        else
        {
            strCollectHQL += "  order by c.TicketTime asc";

            HF_SortTicketTime.Value = "asc";
        }

        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        LB_CollectSql.Text = strCollectHQL;
    }

    protected void BT_SortCompactCode_Click(object sender, EventArgs e)
    {
        DG_Collect.CurrentPageIndex = 0;

        string strCollectHQL = string.Format(@"select c.*,o.ObjectName,o.Model,O.Criterion,o.Grade,s.SupplierName,h.UserName as CheckerName,l.PlanCode,
                            a.UserName as SafekeeperName,n.UserName as ContacterName,k.UnitName
                            from T_WZCollect c
                            left join T_WZObject o on c.ObjectCode = o.ObjectCode
                            left join T_WZPickingPlanDetail l on c.PlanDetailID = l.ID
                            left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                            left join T_ProjectMember h on c.Checker = h.UserCode
                            left join T_ProjectMember a on c.Safekeeper = a.UserCode
                            left join T_ProjectMember n on c.Contacter = n.UserCode
                            left join T_WZSpan k on o.Unit = k.ID
                            where c.Checker ='{0}' 
                            and c.Progress in ('材检','开票') 
                            ", strUserCode); 

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress) & strProgress != LanguageHandle.GetWord("QuanBu").ToString().Trim())
        {
            strCollectHQL += " and c.Progress = '" + strProgress + "'";
        }

        if (!string.IsNullOrEmpty(HF_SortCompactCode.Value))
        {
            strCollectHQL += " order by c.CompactCode desc";

            HF_SortCompactCode.Value = "";
        }
        else
        {
            strCollectHQL += " order by c.CompactCode asc";

            HF_SortCompactCode.Value = "asc";
        }

        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();

        LB_CollectSql.Text = strCollectHQL;
    }

    protected void DG_Collect_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_Collect.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_CollectSql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZCollect");
        DG_Collect.DataSource = ds;
        DG_Collect.DataBind();
    }
}
