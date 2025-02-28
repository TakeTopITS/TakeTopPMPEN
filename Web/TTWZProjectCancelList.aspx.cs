using System; using System.Resources;
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

public partial class TTWZProjectCancelList : System.Web.UI.Page
{
    public string strUserCode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

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
                    where (p.Progress in ('Start Work' ,'����') or p.IsStatus = 'Closed')
                    and p.PurchaseEngineer = '{0}'", strUserCode);   //ChineseWord
        //        string strWZProjectHQL = string.Format(@"select p.*,
        //                    pp.UserName as ProjectManagerName,
        //                    pd.UserName as DelegateAgentName,
        //                    pf.UserName as FeeManageName,
        //                    pm.UserName as PurchaseManagerName,
        //                    pe.UserName as PurchaseEngineerName,
        //                    pc.UserName as ContracterName,
        //                    pk.UserName as CheckerName,
        //                    ps.UserName as SafekeepName
        //                    from T_WZProject p
        //                    left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
        //                    left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
        //                    left join T_ProjectMember pf on p.FeeManage = pf.UserCode
        //                    left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
        //                    left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
        //                    left join T_ProjectMember pc on p.Contracter = pc.UserCode
        //                    left join T_ProjectMember pk on p.Checker = pk.UserCode
        //                    left join T_ProjectMember ps on p.Safekeep = ps.UserCode
        //                    where p.Progress = '����'
        //                    and (COALESCE(SupplementEditor, '') = '' or SupplementEditor = '{0}')", strUserCode);   //ChineseWord
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
                //����
                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditProjectCode = arrOperate[0];
                string strProgress = arrOperate[1];

                string strImportSQL = string.Format(@"select pj.ProjectCode,pj.ProjectName,
                    pd.PlanCode,pd.ObjectCode,pd.PlanNumber,pd.ShortNumber,pd.Progress as PlanDetailProgress,
                    po.ObjectName,po.Model,po.Criterion,po.Grade,
                    ps.UnitName
                    from T_WZPickingPlanDetail pd
                    left join T_WZObject po on pd.ObjectCode = po.ObjectCode
                    left join T_WZSpan ps on po.Unit = ps.ID
                    left join T_WZPickingPlan pp on pd.PlanCode = pp.PlanCode
                    left join T_WZProject pj on pp.ProjectCode = pj.ProjectCode
                    where pj.ProjectCode = '{0}'
                    and pj.Progress = 'Start Work'
                    and pj.PurchaseEngineer = '{1}'
                    and pp.Progress = 'Sign for Receipt'
                    and pd.ShortNumber > 0", strEditProjectCode, strUserCode);   //ChineseWord
                DataTable dtImport = ShareClass.GetDataSetFromSql(strImportSQL, "Import").Tables[0];

                string strGapValue = string.Empty;
                if (dtImport != null && dtImport.Rows.Count > 0)
                {
                    strGapValue = LanguageHandle.GetWord("You").ToString().Trim();
                }
                else {
                    strGapValue = LanguageHandle.GetWord("Mo").ToString().Trim();
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strGapValue + "');", true);

                HF_ProjectCode.Value = strEditProjectCode;
                HF_Progress.Value = strProgress;
                HF_GapValue.Value = strGapValue;
            }
            else if (cmdName == "start")
            {
                //����
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress == LanguageHandle.GetWord("LiXiang").ToString().Trim())
                    {
                        if (string.IsNullOrEmpty(wZProject.StoreRoom) && string.IsNullOrEmpty(wZProject.PurchaseManager) &&
                            string.IsNullOrEmpty(wZProject.PurchaseEngineer) && string.IsNullOrEmpty(wZProject.Contracter) &&
                            string.IsNullOrEmpty(wZProject.Checker) && string.IsNullOrEmpty(wZProject.Safekeep))
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKGZXWCLXLMDJCZL").ToString().Trim()+"')", true);
                            return;
                        }

                        wZProject.Progress = LanguageHandle.GetWord("KaiGong").ToString().Trim();
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //���¼����б�
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKGCG").ToString().Trim()+"')", true);
                    }
                }
            }
            else if (cmdName == "startReturn")
            {
                //�����˻�
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress == LanguageHandle.GetWord("KaiGong").ToString().Trim())
                    {
                        wZProject.Progress = LanguageHandle.GetWord("LiXiang").ToString().Trim();
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //���¼����б�
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKGTHCG").ToString().Trim()+"')", true);
                    }
                }
            }
            else if (cmdName == "browse")
            {
                //���
                string cmdArges = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectBrowse.aspx?strProjectCode=" + cmdArges + "');", true);
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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }



    protected void btnSeach_Click(object sender, EventArgs e)
    {
        //�������������ѯ�����
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        DataBinder(strProjectCode, strProjectName, strProgress);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    /// <summary>
    /// ��Ŀ��������
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
                    where (p.Progress in ('Start Work' ,'����') or p.IsStatus = 'Closed')
                    and p.PurchaseEngineer = '{0}'", strUserCode);   //ChineseWord

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    /// <summary>
    ///  ��Ŀ��������
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
                    where (p.Progress in ('Start Work' ,'����') or p.IsStatus = 'Closed')
                    and p.PurchaseEngineer = '{0}'", strUserCode);   //ChineseWord

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    /// <summary>
    ///  ������������                �����ɹ�����ʦ���������ڣ���Ŀ���ơ�����
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
                    where (p.Progress in ('Start Work' ,'����') or p.IsStatus = 'Closed')
                    and p.PurchaseEngineer = '{0}'", strUserCode);   //ChineseWord

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    /// <summary>
    ///  ���¼����б�
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder("", "", "");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }



    protected void BT_RepeatMark_Click(object sender, EventArgs e)
    {
        //�ڱ������б�ѡ�񡴽��ȡ�����������												
        //�������ǡ���ť�������п�����Ŀ��������ʹ�ñ��												
        //������ϼƻ������Ƿ��С���Ŀ���롵��������Ŀ����Ŀ���롵�ļ�¼												
        //�У���д��¼��������Ŀ��ʹ�ñ�ǡ�����-1����Ȼ���������һ��												
        //�ޣ���д��¼��������Ŀ��ʹ�ñ�ǡ�����0����Ȼ���������һ��												
        //ѭ����飬ֱ��������Ŀ�����һ����¼�����												
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject where Progress = 'Start Work'";   //ChineseWord
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

            string strProjectCode = TXT_ProjectCode.Text.Trim();
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProgress = DDL_Progress.SelectedValue;

            DataBinder(strProjectCode, strProjectName, strProgress);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ʹ�ñ����ɣ�');ControlStatusCloseChange();", true);   //ChineseWord
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZSMYJDZKGDXMSHYKGXMSZZZSYBJ").ToString().Trim()+"')", true);
            return;
        }
    }


    protected void BT_ProjectTotal_Click(object sender, EventArgs e)
    {
        //2. �����ͳ�ơ���ť����ͳ�Ʋ�д��												
        // ͳ��д������������Ŀ���롵���												
        //������Ԥ�㡵�����ƽ���ϸ��Ԥ�������Χ���ƽ���ϸ��ƾ֤��ǡ�����-1��												
        //����ͬ�����ƺ�ͬ����ͬ������Χ����ͬ�����ȡ�������Ч��												
        //��ʵ�����������ϵ���ʵ��������Χ�����ϵ��������ǡ�����-1��												
        //��˰�𡵣������ϵ���˰�𡵣���Χ�����ϵ��������ǡ�����-1��												
        //�����ӷѡ��������ϵ������˷ѡ�����������������Χ�������ϵ��������ǡ�����-1��												
        //�����Ͻ����Ʒ��ϵ����ƻ�������Χ�������ϵ��������ǡ�����-1��												
        //���ɹ����ȣ�������������Ԥ�㡵����ʵ��������˰�𡵣������ӷѡ����£����׹�Ԥ�㡵�����Թ����㡵����100��												
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
        if (listProject != null && listProject.Count > 0)
        {
            for (int i = 0; i < listProject.Count; i++)
            {
                WZProject wZProject = (WZProject)listProject[i];
                string strProjectCode = wZProject.ProjectCode;
                decimal decimalForAndSelf = wZProject.ForCost + wZProject.SelfCost;//���׹�Ԥ�㡵�����Թ����㡵

                //����Ԥ��
                string strTurnDetailHQL = string.Format(@"select 
                            COALESCE(SUM(PlanMoney),0) as TotalPlanMoney 
                            from T_WZTurnDetail
                            where CardIsMark = -1
                            and ProjectCode = '{0}'", strProjectCode);
                DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strTurnDetailHQL, "TurnDetail").Tables[0];
                decimal decimalTheBudget = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtTurnDetail.Rows[0]["TotalPlanMoney"]), out decimalTheBudget);
                //��ͬ���
                string strCompactHQL = string.Format(@"select 
                            COALESCE(SUM(CompactMoney),0) as TotalCompactMoney 
                            from T_WZCompact
                            where Progress = '��Ч'
                            and ProjectCode = '{0}'", strProjectCode);   //ChineseWord
                DataTable dtCompact = ShareClass.GetDataSetFromSql(strCompactHQL, "Compact").Tables[0];
                decimal decimalContractMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtCompact.Rows[0]["TotalCompactMoney"]), out decimalContractMoney);
                //ʵ����˰�����ӷ�
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
                //���Ͻ��
                string strSendHQL = string.Format(@"select 
                            COALESCE(SUM(PlanMoney),0) as TotalPlanMoney
                            from T_WZSend
                            where IsMark = -1
                            and ProjectCode = '{0}'", strProjectCode);
                DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];
                decimal decimalSendMoney = 0;
                decimal.TryParse(ShareClass.ObjectToString(dtSend.Rows[0]["TotalPlanMoney"]), out decimalSendMoney);
                //���ɹ����ȣ�������������Ԥ�㡵����ʵ��������˰�𡵣������ӷѡ����£����׹�Ԥ�㡵�����Թ����㡵����100��
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

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��Ŀͳ����ɣ�');ControlStatusCloseChange();", true);   //ChineseWord
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZSMYXMSHYXMSZZXMTJ").ToString().Trim()+"')", true);
            return;
        }
    }


    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        //�������������ѯ�����
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        DataBinder(strProjectCode, strProjectName, strProgress);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }





    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        string strStockCode = HF_StockCode.Value;
        CancelStock(strStockCode);
    }

    //�������
    private void CancelStock(string strStockCode)
    {
        WZStockBLL wZStockBLL = new WZStockBLL();
        string strWZStockHQL = "from WZStock as wZStock where StockCode = '" + strStockCode + "'";
        IList lstWZStock = wZStockBLL.GetAllWZStocks(strWZStockHQL);
        if (lstWZStock != null && lstWZStock.Count > 0)
        {
            WZStock wZStock = (WZStock)lstWZStock[0];
            if (!wZStock.StockCode.Contains(LanguageHandle.GetWord("ZiYing").ToString().Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�������ӪN�⣬�����������');ControlStatusCloseChange();", true);   //ChineseWord
                return;
            }
            else
            {
                wZStock.IsCancel = -1;

                wZStockBLL.UpdateWZStock(wZStock, wZStock.ID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�������ɹ���');ControlStatusCloseChange();", true);   //ChineseWord
            }
        }
    }


    protected void BT_NewProjectCancel_Click(object sender, EventArgs e)
    {
        //��Ŀ����
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXM").ToString().Trim()+"')", true);
            return;
        }

        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + strEditProjectCode + "'";
        IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
        if (projectList != null && projectList.Count == 1)
        {
            WZProject wZProject = (WZProject)projectList[0];
            if (wZProject.Progress == LanguageHandle.GetWord("KaiGong").ToString().Trim())
            {
                //����Ա=�ɹ�����ʦ
                if (strUserCode == wZProject.PurchaseEngineer.Trim())
                {
                    //TODO
                    //�Ȳ�ѯ���ϼƻ��������ȡ�Ϊ������������ϼƻ���������״̬��������ʾ
                    string strPlanHQL = "select * from T_WZPickingPlan where ProjectCode = '" + wZProject.ProjectCode + "' and Progress != '����'";   //ChineseWord
                    DataTable dtPlan = ShareClass.GetDataSetFromSql(strPlanHQL, "Plan").Tables[0];
                    if (dtPlan != null && dtPlan.Rows.Count > 0)
                    {
                        string strPlanCodes = string.Empty;
                        foreach (DataRow drPlan in dtPlan.Rows)
                        {
                            strPlanCodes += ShareClass.ObjectToString(drPlan["PlanCode"]) + "<br />";
                        }
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDXMBMHYLLJHWHXXHXLLJHSTRPLANCODES").ToString().Trim()+"')", true);
                        return;
                    }
                    //�ٲ�ѯ�ա����ϵ����������ǡ�= -1���粻�������ϵ�����ʾ
                    //�գ����ϵ�
                    string strCollectSendHQL = string.Format(@"select 
                            CollectCode as SingleCode,1 as Ty
                            from T_WZCollect
                            where IsMark = -1
                            and ProjectCode = '{0}'
                            union all
                            select 
                            SendCode as SingleCode,2 as Ty
                            from T_WZSend
                            where IsMark = -1
                            and ProjectCode = '{0}'", wZProject.ProjectCode);
                    DataTable dtSingle = ShareClass.GetDataSetFromSql(strCollectSendHQL, "CollectSend").Tables[0];
                    if (dtSingle != null && dtSingle.Rows.Count > 0)
                    {
                        string strMessage = string.Empty;
                        foreach (DataRow drSingle in dtSingle.Rows)
                        {
                            if (ShareClass.ObjectToString(drSingle["Ty"]) == "1")
                            {
                                strMessage += LanguageHandle.GetWord("ShouLiaoChanHao").ToString().Trim() + ShareClass.ObjectToString(drSingle["SingleCode"]) + LanguageHandle.GetWord("WeiJieSuanbr").ToString().Trim();
                            }
                            else
                            {
                                strMessage += LanguageHandle.GetWord("FaLiaoChanHao").ToString().Trim() + ShareClass.ObjectToString(drSingle["SingleCode"]) + LanguageHandle.GetWord("WeiJieSuanbr").ToString().Trim();
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + strMessage + "');", true);
                        return;
                    }

                    wZProject.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
                    wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                    //���¼����б�
                    string strProjectCode = TXT_ProjectCode.Text.Trim();
                    string strProjectName = TXT_ProjectName.Text.Trim();
                    string strProgress = DDL_Progress.SelectedValue;

                    DataBinder(strProjectCode, strProjectName, strProgress);


                    // �������������Ϊ��ӪN�⣬�Ƿ�һ������
                    if (!wZProject.StoreRoom.Contains(LanguageHandle.GetWord("ZiYing").ToString().Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�����ɹ���');ControlStatusCloseChange();", true);   //ChineseWord
                    }
                    else
                    {
                        HF_StockCode.Value = wZProject.StoreRoom;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertStock()", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWKGSBYXHX").ToString().Trim()+"')", true);
                return;
            }
        }
    }



    protected void BT_NewCancelReturn_Click(object sender, EventArgs e)
    {
        //�����˻�
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXM").ToString().Trim()+"')", true);
            return;
        }

        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + strEditProjectCode + "'";
        IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
        if (projectList != null && projectList.Count == 1)
        {
            WZProject wZProject = (WZProject)projectList[0];
            if (wZProject.Progress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
            {
                //����Ա=�ɹ�����ʦ
                if (strUserCode == wZProject.PurchaseEngineer)
                {
                    wZProject.Progress = LanguageHandle.GetWord("KaiGong").ToString().Trim();
                    wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                    //��𡶺�����ǡ�Ϊ0�����Ϊ��ӪN��ʱ
                    if (wZProject.StoreRoom.Contains(LanguageHandle.GetWord("ZiYing").ToString().Trim()))
                    {
                        string strUpdateStockCodeHQL = "update T_WZStock set IsCancel = 0 where StockCode = '" + wZProject.StoreRoom + "'";
                        ShareClass.RunSqlCommand(strUpdateStockCodeHQL);
                    }

                    //���¼����б�
                    string strProjectCode = TXT_ProjectCode.Text.Trim();
                    string strProjectName = TXT_ProjectName.Text.Trim();
                    string strProgress = DDL_Progress.SelectedValue;

                    DataBinder(strProjectCode, strProjectName, strProgress);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�����˻سɹ���');ControlStatusCloseChange();", true);   //ChineseWord
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWHXSBYXHXTH").ToString().Trim()+"')", true);
                return;
            }
        }
    }



    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //���
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXM").ToString().Trim()+"')", true);
            return;
        }

        string strProgress= HF_Progress.Value;
        string strGapValue= HF_GapValue.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectBrowse.aspx?strProjectCode=" + strEditProjectCode + "');ControlStatusChange('" + strProgress + "','" + strGapValue + "');", true);

    }




    protected void BT_NewGapImport_Click(object sender, EventArgs e)
    {
        //ȱ�ڵ���
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXM").ToString().Trim()+"')", true);
            return;
        }


        string strImportSQL = string.Format(@"select pj.ProjectCode,pj.ProjectName,
                    pd.PlanCode,pd.ObjectCode,pd.PlanNumber,pd.ShortNumber,pd.Progress as PlanDetailProgress,
                    po.ObjectName,po.Model,po.Criterion,po.Grade,
                    ps.UnitName
                    from T_WZPickingPlanDetail pd
                    left join T_WZObject po on pd.ObjectCode = po.ObjectCode
                    left join T_WZSpan ps on po.Unit = ps.ID
                    left join T_WZPickingPlan pp on pd.PlanCode = pp.PlanCode
                    left join T_WZProject pj on pp.ProjectCode = pj.ProjectCode
                    where pj.ProjectCode = '{0}'
                    and pj.Progress = 'Start Work'
                    and pj.PurchaseEngineer = '{1}'
                    and pp.Progress = 'Sign for Receipt'
                    and pd.ShortNumber > 0", strEditProjectCode, strUserCode);   //ChineseWord
        DataTable dtImport = ShareClass.GetDataSetFromSql(strImportSQL, "Import").Tables[0];


        Export3Excel(dtImport, strEditProjectCode + LanguageHandle.GetWord("QueKouBaoBiao").ToString().Trim());

    }




    public void Export3Excel(DataTable dtData, string strFileName)
    {
        DataGrid dgControl = new DataGrid();
        dgControl.DataSource = dtData;
        dgControl.DataBind();


        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dgControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();

    }
}
