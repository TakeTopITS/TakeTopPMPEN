using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Data;
using System.Drawing;
using System.Collections;

public partial class TTWZProjectReple : System.Web.UI.Page
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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "物资管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder("", "", "");

        }
    }

    private void DataBinder(string strProjectCode, string strProjectName, string strProgress)
    {
        DG_List.CurrentPageIndex = 0;

        string strWZProjectHQL = string.Format(@"select p.*,
                    pp.UserName as ProjectManagerName,
                    pd.UserName as DelegateAgentName,
                    pm.UserName as PurchaseManagerName,
                    pe.UserName as PurchaseEngineerName,
                    pc.UserName as ContracterName,
                    pk.UserName as CheckerName,
                    ps.UserName as SafekeepName,
                    pa.UserName as MarkerName,
                    pu.UserName as SupplementEditorName
                    from T_WZProject p
                    left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
                    left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
                    left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
                    left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                    left join T_ProjectMember pc on p.Contracter = pc.UserCode
                    left join T_ProjectMember pk on p.Checker = pk.UserCode
                    left join T_ProjectMember ps on p.Safekeep = ps.UserCode
                    left join T_ProjectMember pa on p.Marker = pa.UserCode
                    left join T_ProjectMember pu on p.SupplementEditor = pu.UserCode
                    where p.Progress != '录入' 
                    and (p.SupplementEditor = '{0}' or p.SupplementEditor = '-')
                    and ProjectCode not in 
                    (
                    select ProjectCode from T_Project
                    where Status in ('删除')
                    )", strUserCode);
       
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZProjectHQL += " and p.ProjectCode like '%" + strProjectCode + "%'";
        }
        if (!string.IsNullOrEmpty(strProjectName))
        {
            strWZProjectHQL += " and p.ProjectName like '%" + strProjectName + "%'";
        }
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZProjectHQL += " and p.Progress = '" + strProgress + "'";
        }
        strWZProjectHQL += " order by p.MarkTime desc";
        DataTable dtProject = ShareClass.GetDataSetFromSql(strWZProjectHQL, "Project").Tables[0];

        DG_List.DataSource = dtProject;
        DG_List.DataBind();


        LB_ProjectSql.Text = strWZProjectHQL;

        LB_RecordCount.Text = dtProject.Rows.Count.ToString();
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
                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditProjectCode = arrOperate[0];
                string strProgress = arrOperate[1];
                string strSupplementEditor = arrOperate[2];
                string strIsMark = arrOperate[3];

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strSupplementEditor + "','" + strUserCode + "');", true);

                ControlStatusChange(strProgress, strSupplementEditor, strIsMark);

                HF_ProjectCode.Value = strEditProjectCode;
                HF_Progress.Value = strProgress;
                HF_SupplementEditor.Value = strSupplementEditor;
                HF_IsMark.Value = strIsMark;

            }
            else if (cmdName == "start")
            {
                //开工
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress == "立项")
                    {
                        if (string.IsNullOrEmpty(wZProject.StoreRoom) && string.IsNullOrEmpty(wZProject.PurchaseManager) &&
                            string.IsNullOrEmpty(wZProject.PurchaseEngineer) && string.IsNullOrEmpty(wZProject.Contracter) &&
                            string.IsNullOrEmpty(wZProject.Checker) && string.IsNullOrEmpty(wZProject.Safekeep))
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKGZXWCLXLMDJCZL + "')", true);
                            return;
                        }

                        wZProject.Progress = "开工";
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //重新加载列表
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        //把按钮状态禁用
                        ControlStatusCloseChange();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKGCG + "')", true);
                    }
                }
            }
            else if (cmdName == "startReturn")
            {
                //开工退回
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress == "开工")
                    {
                        wZProject.Progress = "立项";
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //重新加载列表
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        //把按钮状态禁用
                        ControlStatusCloseChange();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKGTHCG + "')", true);
                    }
                }
            }
            else if (cmdName == "browse")
            {
                //浏览
                string cmdArges = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectBrowse.aspx?strProjectCode=" + cmdArges + "')", true);
                return;
            }
            else if (cmdName == "edit")
            {
                //编辑
                string cmdArges = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectRepleAdd.aspx?strProjectCode=" + cmdArges + "')", true);
                return;
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_ProjectSql.Text.Trim(); ;
        DataTable dtProject = ShareClass.GetDataSetFromSql(strHQL, "Project").Tables[0];

        DG_List.DataSource = dtProject;
        DG_List.DataBind();

        //把按钮状态禁用
        ControlStatusCloseChange();
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }



    protected void btnSeach_Click(object sender, EventArgs e)
    {
        //根据相关条件查询立项工程
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        DataBinder(strProjectCode, strProjectName, strProgress);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        //把按钮状态禁用
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

        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        string strWZProjectHQL = string.Format(@"select p.*,
                    pp.UserName as ProjectManagerName,
                    pd.UserName as DelegateAgentName,
                    pm.UserName as PurchaseManagerName,
                    pe.UserName as PurchaseEngineerName,
                    pc.UserName as ContracterName,
                    pk.UserName as CheckerName,
                    ps.UserName as SafekeepName,
                    pa.UserName as MarkerName,
                    pu.UserName as SupplementEditorName
                    from T_WZProject p
                    left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
                    left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
                    left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
                    left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                    left join T_ProjectMember pc on p.Contracter = pc.UserCode
                    left join T_ProjectMember pk on p.Checker = pk.UserCode
                    left join T_ProjectMember ps on p.Safekeep = ps.UserCode
                    left join T_ProjectMember pa on p.Marker = pa.UserCode
                    left join T_ProjectMember pu on p.SupplementEditor = pu.UserCode
                    where p.Progress != '录入' 
                    and (p.SupplementEditor = '{0}' or p.SupplementEditor = '-')", strUserCode);

        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZProjectHQL += " and p.ProjectCode = '" + strProjectCode + "'";
        }
        if (!string.IsNullOrEmpty(strProjectName))
        {
            strWZProjectHQL += " and p.ProjectName like '%" + strProjectName + "%'";
        }
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZProjectHQL += " and p.Progress = '" + strProgress + "'";
        }
        if (!string.IsNullOrEmpty(HF_SortProjectCode.Value))
        {
            strWZProjectHQL += " order by p.ProjectCode desc";

            HF_SortProjectCode.Value = "";
        }
        else
        {
            strWZProjectHQL += " order by p.ProjectCode asc";

            HF_SortProjectCode.Value = "asc";
        }
        DataTable dtProject = ShareClass.GetDataSetFromSql(strWZProjectHQL, "Project").Tables[0];

        DG_List.DataSource = dtProject;
        DG_List.DataBind();

        LB_ProjectSql.Text = strWZProjectHQL;

        //LB_ProjectSql.Text = strWZProjectHQL;
        LB_RecordCount.Text = dtProject.Rows.Count.ToString();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        //把按钮状态禁用
        ControlStatusCloseChange();
    }

    /// <summary>
    ///  项目名称排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BT_SortProjectName_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        string strWZProjectHQL = string.Format(@"select p.*,
                    pp.UserName as ProjectManagerName,
                    pd.UserName as DelegateAgentName,
                    pm.UserName as PurchaseManagerName,
                    pe.UserName as PurchaseEngineerName,
                    pc.UserName as ContracterName,
                    pk.UserName as CheckerName,
                    ps.UserName as SafekeepName,
                    pa.UserName as MarkerName,
                    pu.UserName as SupplementEditorName
                    from T_WZProject p
                    left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
                    left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
                    left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
                    left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                    left join T_ProjectMember pc on p.Contracter = pc.UserCode
                    left join T_ProjectMember pk on p.Checker = pk.UserCode
                    left join T_ProjectMember ps on p.Safekeep = ps.UserCode
                    left join T_ProjectMember pa on p.Marker = pa.UserCode
                    left join T_ProjectMember pu on p.SupplementEditor = pu.UserCode
                    where p.Progress != '录入' 
                    and (p.SupplementEditor = '{0}' or p.SupplementEditor = '-')", strUserCode);

        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZProjectHQL += " and p.ProjectCode = '" + strProjectCode + "'";
        }
        if (!string.IsNullOrEmpty(strProjectName))
        {
            strWZProjectHQL += " and p.ProjectName like '%" + strProjectName + "%'";
        }
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZProjectHQL += " and p.Progress = '" + strProgress + "'";
        }
        if (!string.IsNullOrEmpty(HF_SortProjectName.Value))
        {
            strWZProjectHQL += " order by p.ProjectName desc";

            HF_SortProjectName.Value = "";
        }
        else
        {
            strWZProjectHQL += " order by p.ProjectName asc";

            HF_SortProjectName.Value = "asc";
        }
        DataTable dtProject = ShareClass.GetDataSetFromSql(strWZProjectHQL, "Project").Tables[0];

        DG_List.DataSource = dtProject;
        DG_List.DataBind();

        LB_ProjectSql.Text = strWZProjectHQL;

        LB_RecordCount.Text = dtProject.Rows.Count.ToString();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        //把按钮状态禁用
        ControlStatusCloseChange();
    }

    /// <summary>
    ///  开工日期排序                按“采购工程师＋开工日期＋项目名称”排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BT_SortStartTime_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        string strWZProjectHQL = string.Format(@"select p.*,
                    pp.UserName as ProjectManagerName,
                    pd.UserName as DelegateAgentName,
                    pm.UserName as PurchaseManagerName,
                    pe.UserName as PurchaseEngineerName,
                    pc.UserName as ContracterName,
                    pk.UserName as CheckerName,
                    ps.UserName as SafekeepName,
                    pa.UserName as MarkerName,
                    pu.UserName as SupplementEditorName
                    from T_WZProject p
                    left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
                    left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
                    left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
                    left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                    left join T_ProjectMember pc on p.Contracter = pc.UserCode
                    left join T_ProjectMember pk on p.Checker = pk.UserCode
                    left join T_ProjectMember ps on p.Safekeep = ps.UserCode
                    left join T_ProjectMember pa on p.Marker = pa.UserCode
                    left join T_ProjectMember pu on p.SupplementEditor = pu.UserCode
                    where p.Progress != '录入' 
                    and (p.SupplementEditor = '{0}' or p.SupplementEditor = '-')", strUserCode);

        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strWZProjectHQL += " and p.ProjectCode = '" + strProjectCode + "'";
        }
        if (!string.IsNullOrEmpty(strProjectName))
        {
            strWZProjectHQL += " and p.ProjectName like '%" + strProjectName + "%'";
        }
        if (!string.IsNullOrEmpty(strProgress))
        {
            strWZProjectHQL += " and p.Progress = '" + strProgress + "'";
        }
        if (!string.IsNullOrEmpty(HF_SortStartTime.Value))
        {
            strWZProjectHQL += " order by p.PurchaseEngineer desc,p.StartTime desc,p.ProjectName desc";

            HF_SortStartTime.Value = "";
        }
        else
        {
            strWZProjectHQL += " order by p.PurchaseEngineer asc,p.StartTime asc,p.ProjectName asc";

            HF_SortStartTime.Value = "asc";
        }
        DataTable dtProject = ShareClass.GetDataSetFromSql(strWZProjectHQL, "Project").Tables[0];

        DG_List.DataSource = dtProject;
        DG_List.DataBind();

        LB_ProjectSql.Text = strWZProjectHQL;

        LB_RecordCount.Text = dtProject.Rows.Count.ToString();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        //把按钮状态禁用
        ControlStatusCloseChange();
    }


    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder("", "", "");

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        //把按钮状态禁用
        ControlStatusCloseChange();
    }



    protected void BT_RepeatMark_Click(object sender, EventArgs e)
    {
        //在本界面列表处选择〈进度〉＝“开工”												
        //点击“标记”按钮，对所有开工项目逐条重做使用标记												
        //检查领料计划表单中是否有〈项目编码〉＝工程项目〈项目编码〉的记录												
        //有，则写记录：工程项目〈使用标记〉＝“-1”，然后继续做下一条												
        //无，则写记录：工程项目〈使用标记〉＝“0”，然后继续做下一条												
        //循环检查，直到工程项目表单最后一条记录后结束												
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject where Progress = '开工'";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
        if (listProject != null && listProject.Count > 0)
        {
            for (int i = 0; i < listProject.Count; i++)
            {
                WZProject wZProject = (WZProject)listProject[i];

                string strPlanHQL = "select * from T_WZPickingPlan where ProjectCode = '" + wZProject.ProjectCode + "'";
                DataTable dtPlan = ShareClass.GetDataSetFromSql(strPlanHQL, "Plan").Tables[0];
                if (dtPlan != null && dtPlan.Rows.Count > 0)
                {
                    wZProject.IsMark = -1;
                }
                else
                {
                    wZProject.IsMark = 0;
                }

                wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
            }

            //根据相关条件查询立项工程
            string strProjectCode = TXT_ProjectCode.Text.Trim();
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProgress = DDL_Progress.SelectedValue;

            DataBinder(strProjectCode, strProjectName, strProgress);

            //把按钮状态禁用
            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('重做使用标记完成！');", true);
        }
        else
        {
            //把按钮状态禁用
            //ControlStatusCloseChange();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('暂时没有进度在开工的项目，请稍后有开工项目时再重做使用标记！');", true);
            return;
        }
    }


    protected void BT_ProjectTotal_Click(object sender, EventArgs e)
    {
        //2. 点击“统计”按钮进行统计并写入												
        // 统计写入条件：〈项目编码〉相等												
        //〈甲领预算〉＝∑移交明细〈预算金额〉，范围：移交明细〈凭证标记〉＝“-1”												
        //〈合同金额〉＝∑合同〈合同金额〉，范围：合同〈进度〉＝“生效”												
        //〈实购金额〉＝∑收料单〈实购金额〉，范围：收料单〈结算标记〉＝“-1”												
        //〈税金〉＝∑收料单〈税金〉，范围：收料单〈结算标记〉＝“-1”												
        //〈运杂费〉＝∑收料单（〈运费〉＋〈其它〉），范围：：收料单〈结算标记〉＝“-1”												
        //〈发料金额〉＝∑发料单〈计划金额〉，范围：：发料单〈结算标记〉＝“-1”												
        //〈采购进度％〉＝（〈甲领预算〉＋〈实购金额〉＋〈税金〉＋〈运杂费〉）÷（〈甲供预算〉＋〈自购概算〉）×100％												
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
        if (listProject != null && listProject.Count > 0)
        {
            for (int i = 0; i < listProject.Count; i++)
            {
                WZProject wZProject = (WZProject)listProject[i];
                string strProjectCode = wZProject.ProjectCode;
                decimal decimalForAndSelf = wZProject.ForCost + wZProject.SelfCost;//〈甲供预算〉＋〈自购概算〉

                //甲领预算
                string strTurnDetailHQL = string.Format(@"select 
                            COALESCE(SUM(PlanMoney),0) as TotalPlanMoney 
                            from T_WZTurnDetail
                            where CardIsMark = -1
                            and ProjectCode = '{0}'", strProjectCode);
                DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strTurnDetailHQL, "TurnDetail").Tables[0];
                decimal decimalTheBudget = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtTurnDetail.Rows[0]["TotalPlanMoney"]), out decimalTheBudget);
                //合同金额
                string strCompactHQL = string.Format(@"select 
                            COALESCE(SUM(CompactMoney),0) as TotalCompactMoney 
                            from T_WZCompact
                            where Progress = '生效'
                            and ProjectCode = '{0}'", strProjectCode);
                DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];
                decimal decimalContractMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtCompact.Rows[0]["TotalCompactMoney"]), out decimalContractMoney);
                //实购金额，税金，运杂费
                string strCollectHQL = string.Format(@"select 
                            COALESCE(SUM(ActualMoney),0) as TotalActualMoney, 
                            COALESCE(SUM(RatioMoney),0) as TotalRatioMoney,
                            COALESCE(SUM(Freight),0) as TotalFreight,
                            COALESCE(SUM(OtherObject),0) as TotalOtherObject
                            from T_WZCollect
                            where IsMark = -1
                            and ProjectCode = '{0}'", strProjectCode);
                DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];
                decimal decimalAcceptMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtCollect.Rows[0]["TotalActualMoney"]), out decimalAcceptMoney);
                decimal decimalProjectTax = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtCollect.Rows[0]["TotalRatioMoney"]), out decimalProjectTax);
                decimal decimalFreight = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtCollect.Rows[0]["TotalFreight"]), out decimalFreight);
                decimal decimalOtherObject = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtCollect.Rows[0]["TotalOtherObject"]), out decimalOtherObject);
                decimal decimalTheFreight = decimalFreight + decimalOtherObject;
                //发料金额
                string strSendHQL = string.Format(@"select 
                            COALESCE(SUM(PlanMoney),0) as TotalPlanMoney
                            from T_WZSend
                            where IsMark = -1
                            and ProjectCode = '{0}'", strProjectCode);
                DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];
                decimal decimalSendMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtSend.Rows[0]["TotalPlanMoney"]), out decimalSendMoney);
                //〈采购进度％〉＝（〈甲领预算〉＋〈实购金额〉＋〈税金〉＋〈运杂费〉）÷（〈甲供预算〉＋〈自购概算〉）×100％
                decimal decimalFinishingRate = 0;
                if (decimalForAndSelf != 0)
                {
                    decimalFinishingRate = ((decimalTheBudget + decimalAcceptMoney + decimalProjectTax + decimalTheFreight) / decimalForAndSelf) * 100;
                }

                wZProject.TheBudget = decimalTheBudget;
                wZProject.ContractMoney = decimalContractMoney;
                wZProject.AcceptMoney = decimalAcceptMoney;
                wZProject.ProjectTax = decimalProjectTax;
                wZProject.TheFreight = decimalTheFreight;
                wZProject.SendMoney = decimalSendMoney;
                wZProject.FinishingRate = decimalFinishingRate;

                wZProjectBLL.UpdateWZProject(wZProject, wZProject.ProjectCode);
            }

            //根据相关条件查询立项工程
            string strControlProjectCode = TXT_ProjectCode.Text.Trim();
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProgress = DDL_Progress.SelectedValue;

            DataBinder(strControlProjectCode, strProjectName, strProgress);

            //把按钮状态禁用
            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('项目统计完成！');", true);
        }
        else
        {
            //把按钮状态禁用
            //ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('暂时没有项目，请稍后有项目时再做项目统计！');", true);
            return;
        }
    }
    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        //根据相关条件查询立项工程
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        DataBinder(strProjectCode, strProjectName, strProgress);

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        //把按钮状态禁用
        ControlStatusCloseChange();
    }




    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDXM + "')", true);
            return;
        }

        //string strProgress = HF_Progress.Value;
        //string strSupplementEditor = HF_SupplementEditor.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectRepleAdd.aspx?strProjectCode=" + strEditProjectCode + "');", true);
        //按钮控制
        //ControlStatusChange(HF_Progress.Value, HF_SupplementEditor.Value);
    }


    protected void BT_NewStart_Click(object sender, EventArgs e)
    {
        //开工
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDXM + "')", true);
            return;
        }

        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + strEditProjectCode + "'";
        IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
        if (projectList != null && projectList.Count == 1)
        {
            WZProject wZProject = (WZProject)projectList[0];
            if (wZProject.Progress == "立项")
            {
                string strCurrentProgress = wZProject.Progress;
                string strSupplementEditor = wZProject.SupplementEditor;
                string strAuthorityPurchase = wZProject.PowerPurchase.Trim();

                if (strAuthorityPurchase == "有")
                {
                    if (string.IsNullOrEmpty(wZProject.StoreRoom) || string.IsNullOrEmpty(wZProject.PurchaseManager) ||
                      string.IsNullOrEmpty(wZProject.PurchaseEngineer) || string.IsNullOrEmpty(wZProject.Contracter) ||
                      string.IsNullOrEmpty(wZProject.Checker) || string.IsNullOrEmpty(wZProject.Safekeep))
                    {
                        ////按钮控制
                        //ControlStatusChange(HF_Progress.Value, HF_SupplementEditor.Value);
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKGZXWCLXLMDJCZL + "');", true);
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(wZProject.PurchaseManager) || string.IsNullOrEmpty(wZProject.PurchaseEngineer) || string.IsNullOrEmpty(wZProject.Contracter) ||
                        string.IsNullOrEmpty(wZProject.Checker) )
                    {
                        ////按钮控制
                        //ControlStatusChange(HF_Progress.Value, HF_SupplementEditor.Value);
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKGZXWCLXLMDJCZL + "');", true);
                        return;
                    }
                }

                wZProject.Progress = "开工";
                wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                //重新加载列表
                string strProjectCode = TXT_ProjectCode.Text.Trim();
                string strProjectName = TXT_ProjectName.Text.Trim();
                string strProgress = DDL_Progress.SelectedValue;

                DataBinder(strProjectCode, strProjectName, strProgress);

                //把按钮状态禁用
                ControlStatusCloseChange();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKGCG + "');", true);
            }
        }
    }



    protected void BT_NewReturnStart_Click(object sender, EventArgs e)
    {
        //开工退回
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDXM + "')", true);
            return;
        }

        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + strEditProjectCode + "'";
        IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
        if (projectList != null && projectList.Count == 1)
        {
            WZProject wZProject = (WZProject)projectList[0];
            if (wZProject.Progress == "开工")
            {
                wZProject.Progress = "立项";
                wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                //重新加载列表
                string strProjectCode = TXT_ProjectCode.Text.Trim();
                string strProjectName = TXT_ProjectName.Text.Trim();
                string strProgress = DDL_Progress.SelectedValue;

                DataBinder(strProjectCode, strProjectName, strProgress);

                //把按钮状态禁用
                ControlStatusCloseChange();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZKGTHCG + "')", true);
            }
        }
    }



    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //浏览
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDXM + "')", true);
            return;
        }

        string strProgress = HF_Progress.Value;
        string strSupplementEditor = HF_SupplementEditor.Value;
        string strIsMark = HF_IsMark.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectBrowse.aspx?strProjectCode=" + strEditProjectCode + "');", true);
        //按钮控制
        //ControlStatusChange(HF_Progress.Value, HF_SupplementEditor.Value, strIsMark);
    }





    private void ControlStatusChange(string strProgress, string strSupplementEditor, string strIsMark)
    {
        BT_NewBrowse.Enabled = true;

        if ((strProgress == "立项") && (strSupplementEditor == null || strSupplementEditor == "" || strSupplementEditor == strUserCode || strSupplementEditor == "-") && strIsMark != "-1")
        {

            BT_NewEdit.Enabled = true;
        }
        else
        {
            BT_NewEdit.Enabled = false;

        }

        if (strProgress == "立项")
        {
            BT_NewStart.Enabled = true;

        }
        else
        {
            BT_NewStart.Enabled = false;

        }

        if (strProgress == "开工" && strIsMark != "-1")
        {
            BT_NewReturnStart.Enabled = true;
        }
        else
        {
            BT_NewReturnStart.Enabled = false;
        }
    }



    private void ControlStatusCloseChange()
    {

        BT_NewEdit.Enabled = false;
        BT_NewBrowse.Enabled = false;

        BT_NewStart.Enabled = false;

        BT_NewReturnStart.Enabled = false;
    }


}