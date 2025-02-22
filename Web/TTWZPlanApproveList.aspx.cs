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

public partial class TTWZPlanApproveList : System.Web.UI.Page
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
            DataBinder();

            TXT_ProjectCode.BackColor = Color.CornflowerBlue;
            TXT_PlanName.BackColor = Color.CornflowerBlue;
        }
    }

    private void DataBinder()
    {


        string strWZPickingPlanHQL = string.Format(@"select pp.*,
                        pm.UserName as PlanMarkerName,
                        pf.UserName as FeeManageName,
                        pe.UserName as PurchaseEngineerName
                        from T_WZPickingPlan pp
                        left join T_ProjectMember pm on pp.PlanMarker = pm.UserCode
                        left join T_ProjectMember pf on pp.FeeManage = pf.UserCode
                        left join T_ProjectMember pe on pp.PurchaseEngineer = pe.UserCode
                        where pp.FeeManage = '{0}' 
                        and pp.Progress != '录入'", strUserCode);

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

        DataTable dtPickingPlan = ShareClass.GetDataSetFromSql(strWZPickingPlanHQL, "PickingPlan").Tables[0];

        DG_List.DataSource = dtPickingPlan;
        DG_List.DataBind();

        LB_ShowRecordCount.Text = dtPickingPlan.Rows.Count.ToString();
        LB_PlanSQL.Text = strWZPickingPlanHQL;

        ControlStatusCloseChange();
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
                    //操作
                    string strPlanCode = wZPickingPlan.PlanCode.Trim();
                    string strProgress = wZPickingPlan.Progress.Trim();
                    string strFeeManage = wZPickingPlan.FeeManage.Trim();

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strFeeManage + "','" + strUserCode.Trim() + "');", true);
                    ControlStatusChange(strProgress, strFeeManage, strUserCode);

                    HF_NewPlanCode.Value = strPlanCode;
                    HF_PlanCodeValue.Value = strPlanCode;
                    HF_NewProgress.Value = strProgress;
                    HF_NewFeeManage.Value = strFeeManage;
                }
                else if (cmdName == "sign")
                {
                    //审核
                    if (wZPickingPlan.Progress == "提报")
                    {
                        wZPickingPlan.Progress = "审核";
                        wZPickingPlan.ReturnReason = "";
                        wZPickingPlan.ApproveTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHCG+"')", true);
                    }
                }
                else if (cmdName == "signReturn")
                {
                    //退回审核
                    if (wZPickingPlan.Progress == "审核")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickSignReturn('" + cmdArges + "')", true);
                        return;

                        //wZPickingPlan.Progress = "提报";
                        //wZPickingPlan.ApproveTime = "-";
                        //wZPickingPlan.ReturnReason = TXT_ReturnReason.Text.Trim();

                        //wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        ////重新加载列表
                        //DataBinder();

                        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHTHCG+"')", true);
                    }
                    else
                    {

                    }
                }
                else if (cmdName == "notApprove")
                {
                    //驳回
                    if (wZPickingPlan.Progress == "提报")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReturn('" + cmdArges + "')", true);
                        return;
                        //wZPickingPlan.Progress = "提报";
                        //wZPickingPlan.ReturnReason = TXT_ReturnReason.Text.Trim();

                        //wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        ////重新加载列表
                        //DataBinder();

                        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHBTGCG+"')", true);
                    }
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_PlanSQL.Text.Trim(); ;
        DataTable dtPlan = ShareClass.GetDataSetFromSql(strHQL, "Plan").Tables[0];

        DG_List.DataSource = dtPlan;
        DG_List.DataBind();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }



    protected void BT_NewAudit_Click(object sender, EventArgs e)
    {
        //审核
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先点击要操作的计划！');", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strPlanMarker = HF_NewFeeManage.Value;

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];
            if (wZPickingPlan.Progress == "提报")
            {
                wZPickingPlan.Progress = "审核";
                wZPickingPlan.ReturnReason = "";
                wZPickingPlan.ApproveTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strEditPlanCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHCG+"');", true);
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


    protected void BT_NewAuditReturn_Click(object sender, EventArgs e)
    {
        //审核退回
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先点击要操作的计划！');", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickSignReturn('" + strEditPlanCode + "');", true);
        return;
    }

    protected void BT_NewPlanReturn_Click(object sender, EventArgs e)
    {
        //提交退回
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先点击要操作的计划！');", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReturn('" + strEditPlanCode + "');", true);
        return;
    }



    protected void BT_NewPlanBrowse_Click(object sender, EventArgs e)
    {
        //计划浏览
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先点击要操作的计划！');", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanBrowse.aspx?planCode=" + strEditPlanCode + "');", true);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZPlanBrowse.aspx?planCode=" + strEditPlanCode + "');", true);
        return;
    }


    protected void BT_NewDetailBrowse_Click(object sender, EventArgs e)
    {
        //明细浏览
        string strEditPlanCode = HF_NewPlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先点击要操作的计划！');", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanDetailBrowse.aspx?planCode=" + strEditPlanCode + "');", true);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZPlanDetailBrowse.aspx?planCode=" + strEditPlanCode + "');", true);
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

            //退回审核
            if (wZPickingPlan.Progress == "审核")
            {
                wZPickingPlan.Progress = "提报";
                wZPickingPlan.ApproveTime = "-";
                //wZPickingPlan.ReturnReason = HF_WriteText.Value;
                //wZPickingPlan.ReturnReason = TB_AduditReturnReason.Text.Trim();

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strPlanCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHTHCG+"')", true);
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


    protected void BT_ReturnButton_Click(object sender, EventArgs e)
    {
        string strPlanCode = HF_PlanCodeValue.Value;
        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];

            //退回录入
            if (wZPickingPlan.Progress == "提报")
            {
                wZPickingPlan.Progress = "录入";
                wZPickingPlan.CommitTime = "-";
                //wZPickingPlan.ReturnReason = HF_WriteText.Value;

                wZPickingPlan.ReturnReason = TB_PlanReturnReason.Text.Trim();

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strPlanCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHTHCG + "！');", true);
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



    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    /// <summary>
    /// 计划编号排序
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
                        where pp.FeeManage = '{0}' 
                        and pp.Progress != '录入'", strUserCode);

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
    /// 计划名称排序
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
                        where pp.FeeManage = '{0}' 
                        and pp.Progress != '录入'", strUserCode);

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
    /// 项目编码排序
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
                        where pp.FeeManage = '{0}' 
                        and pp.Progress != '录入'", strUserCode);

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




    private void ControlStatusChange(string objProgress, string objFeeManage, string objUserCode)
    {
        BT_NewPlanBrowse.Enabled = true;
        BT_NewDetailBrowse.Enabled = true;


        if (objProgress == "提报" && objFeeManage == objUserCode)
        {
            BT_NewAudit.Enabled = true;
            BT_NewAuditReturn.Enabled = false;
            BT_NewPlanReturn.Enabled = true;

        }
        else if (objProgress == "审核" && objFeeManage == objUserCode)
        {
            BT_NewAudit.Enabled = false;
            BT_NewAuditReturn.Enabled = true;
            BT_NewPlanReturn.Enabled = false;
        }
        else
        {
            BT_NewAudit.Enabled = false;
            BT_NewAuditReturn.Enabled = false;
            BT_NewPlanReturn.Enabled = false;
        }


    }



    private void ControlStatusCloseChange()
    {
        BT_NewAudit.Enabled = false;
        BT_NewAuditReturn.Enabled = false;
        BT_NewPlanReturn.Enabled = false;
        BT_NewPlanBrowse.Enabled = false;
        BT_NewDetailBrowse.Enabled = false;

    }




}