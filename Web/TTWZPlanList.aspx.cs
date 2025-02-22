using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZPlanList : System.Web.UI.Page
{
    public string strUserCode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "领料计划编制", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();

            TXT_ProjectCode.BackColor = Color.CornflowerBlue;
            TXT_PlanName.BackColor = Color.CornflowerBlue;
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
                    where pp.PlanMarker = '{0}'", strUserCode);

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

        LB_PlanSQL.Text = strWZPickingPlanHQL;
        LB_ShowRecordCount.Text = dtPickingPlan.Rows.Count.ToString();

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
                    string strPlanCode = wZPickingPlan.PlanCode;
                    string strProgress = wZPickingPlan.Progress;
                    string strPlanMarker = wZPickingPlan.PlanMarker;
                    string strIsMark = wZPickingPlan.IsMark.ToString();

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strPlanMarker + "','" + strUserCode + "','" + strIsMark + "');", true);
                    ControlStatusChange(strProgress, strPlanMarker, strUserCode, strIsMark);

                    HF_PlanCode.Value = strPlanCode;
                    HF_Progress.Value = strProgress;
                    HF_PlanMarker.Value = strPlanMarker;
                    HF_IsMark.Value = strIsMark;
                }
                else if (cmdName == "del")
                {
                    if (wZPickingPlan.Progress != "录入" || wZPickingPlan.IsMark != 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWLRYJSYBJBW0SBYXSC + "')", true);
                        return;
                    }
                    wZPickingPlanBLL.DeleteWZPickingPlan(wZPickingPlan);

                    //重新加载列表
                    DG_List.CurrentPageIndex = 0;
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
                }
                else if (cmdName == "submit")
                {
                    if (wZPickingPlan.Progress == "录入")
                    {
                        if (wZPickingPlan.DetailCount == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXTJJHMXZTJJH + "')", true);
                            return;
                        }

                        wZPickingPlan.Progress = "提报";
                        wZPickingPlan.CommitTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        wZPickingPlan.ReturnReason = "";

                        wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHTJCG + "')", true);
                    }
                }
                else if (cmdName == "submitReturn")
                {
                    if (wZPickingPlan.Progress == "提报")
                    {
                        wZPickingPlan.Progress = "录入";
                        wZPickingPlan.CommitTime = "-";

                        wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, cmdArges);

                        //重新加载列表
                        DataBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHTHCG + "')", true);
                    }
                }
                else if (cmdName == "edit")
                {
                    //编辑
                    Response.Redirect("TTWZPlanEdit.aspx?id=" + wZPickingPlan.PlanCode);
                }
                else if (cmdName == "detail")
                {
                    //编辑
                    Response.Redirect("TTWZPlanDetail.aspx?planCode=" + wZPickingPlan.PlanCode);
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_PlanSQL.Text.Trim(); ;
        DataTable dtPickingPlan = ShareClass.GetDataSetFromSql(strHQL, "PickingPlan").Tables[0];

        DG_List.DataSource = dtPickingPlan;
        DG_List.DataBind();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanEdit.aspx?id=');", true);
        return;
    }

    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditPlanCode = HF_PlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJH + "')", true);
            return;
        }


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanEdit.aspx?id=" + strEditPlanCode + "');", true);
        return;
    }

    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditPlanCode = HF_PlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJH + "')", true);
            return;
        }

        string strProgress = HF_Progress.Value;
        string strPlanMarker = HF_PlanMarker.Value;
        string strIsMark = HF_IsMark.Value;

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];
            if (wZPickingPlan.Progress != "录入" || wZPickingPlan.IsMark != 0)
            {
                ControlStatusChange(strProgress, strPlanMarker, strUserCode, strIsMark);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJDBWLRYJSYBJBW0SBYXSC + "');", true);
                return;
            }
            wZPickingPlanBLL.DeleteWZPickingPlan(wZPickingPlan);

            //重新加载列表
            DG_List.CurrentPageIndex = 0;
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "');", true);
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }
    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        //明细
        string strEditPlanCode = HF_PlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ControlStatusCloseChange();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJH + "');", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanDetail.aspx?planCode=" + strEditPlanCode + "');", true);
        return;
    }

    protected void BT_NewPlanSubmit_Click(object sender, EventArgs e)
    {
        //计划提交
        string strEditPlanCode = HF_PlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ControlStatusCloseChange();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJH + "');", true);
            return;
        }

        string strHQL = "Select * From T_WZPickingPlanDetail Where PlanCode = '" + strEditPlanCode + "' and PlanNumber = 0";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPickingPlanDetail");
        if(ds.Tables [0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click0", "alert('" + Resources.lang.JGJHMXJHSLCZWLDJLBNTJQJC + "');", true);
            return;
        }

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];
            if (wZPickingPlan.Progress == "录入")
            {
                if (wZPickingPlan.DetailCount == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click1", "alert('" + Resources.lang.ZZXTJJHMXZTJJH + "');", true);
                    return;
                }

                wZPickingPlan.Progress = "提报";
                wZPickingPlan.CommitTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                wZPickingPlan.ReturnReason = "";

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strEditPlanCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click2", "alert('" + Resources.lang.ZZJHTJCG + "');", true);
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
    protected void BT_NewSubmitReturn_Click(object sender, EventArgs e)
    {
        //提交退回
        string strEditPlanCode = HF_PlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJH + "');", true);
            return;
        }

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strEditPlanCode + "'";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);
        if (listWZPickingPlan != null && listWZPickingPlan.Count == 1)
        {
            WZPickingPlan wZPickingPlan = (WZPickingPlan)listWZPickingPlan[0];
            if (wZPickingPlan.Progress == "提报")
            {
                wZPickingPlan.Progress = "录入";
                wZPickingPlan.CommitTime = "-";

                wZPickingPlanBLL.UpdateWZPickingPlan(wZPickingPlan, strEditPlanCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHTHCG + "');", true);
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

    protected void BT_NewPlanBrowse_Click(object sender, EventArgs e)
    {
        //计划浏览
        string strEditPlanCode = HF_PlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ControlStatusCloseChange();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJH + "');", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanBrowse.aspx?planCode=" + strEditPlanCode + "');", true);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZPlanBrowse.aspx?planCode=" + strEditPlanCode + "');", true);
        return;
    }


    protected void BT_NewDetailBrowse_Click(object sender, EventArgs e)
    {
        //明细浏览
        string strEditPlanCode = HF_PlanCode.Value;
        if (string.IsNullOrEmpty(strEditPlanCode))
        {
            ControlStatusCloseChange();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJH + "');", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanDetailBrowse.aspx?planCode=" + strEditPlanCode + "');", true);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZPlanDetailBrowse.aspx?planCode=" + strEditPlanCode + "');", true);
        return;
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
                    where pp.PlanMarker = '{0}'", strUserCode);

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
                    where pp.PlanMarker = '{0}'", strUserCode);

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
                    where pp.PlanMarker = '{0}'", strUserCode);

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





    private void ControlStatusChange(string objProgress, string objPlanMarker, string objUserCode, string objIsMark)
    {

        BT_NewPlanBrowse.Enabled = true;
        BT_NewDetailBrowse.Enabled = true;

        if(objIsMark == "-1")
        {
            BT_NewSubmitReturn.Enabled = true;
        }
        else
        {
            BT_NewSubmitReturn.Enabled = false;
        }

        if (objProgress == "录入")
        {
            BT_NewEdit.Enabled = true;
            BT_NewPlanSubmit.Enabled = true;
            BT_NewSubmitReturn.Enabled = false;

        }
        else if (objProgress == "提报")
        {
            BT_NewEdit.Enabled = false;
            BT_NewPlanSubmit.Enabled = false;
            BT_NewSubmitReturn.Enabled = true;

        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewPlanSubmit.Enabled = false;
            BT_NewSubmitReturn.Enabled = false;

        }

        if (objProgress == "录入" && objPlanMarker == objUserCode && objIsMark == "0")
        {
            BT_NewDelete.Enabled = true;

        }
        else
        {
            BT_NewDelete.Enabled = false;

        }

        if (objProgress == "录入" && objPlanMarker == objUserCode)
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
        BT_NewEdit.Enabled = false;
        BT_NewDelete.Enabled = false;
        BT_NewDetail.Visible = false;
        BT_NewPlanSubmit.Enabled = false;
        BT_NewSubmitReturn.Enabled = false;
        BT_NewPlanBrowse.Enabled = false;
        BT_NewDetailBrowse.Enabled = false;

    }



}