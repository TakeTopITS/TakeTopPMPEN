using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZProjectList : System.Web.UI.Page
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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx",LanguageHandle.GetWord("LiXiang").ToString().Trim(), strUserCode);

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
                        where p.Marker = '{0}'", strUserCode);
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

        
        #region ע��
        //DG_List.CurrentPageIndex = 0;

        //WZProjectBLL wZProjectBLL = new WZProjectBLL();
        //string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
        //string strProjectHQL = "from WZProject as wZProject where Marker = '" + strUserCode + "' ";
        //if (!string.IsNullOrEmpty(strProjectCode))
        //{
        //    strProjectHQL += " and ProjectCode = '" + strProjectCode + "'";
        //}
        //if (!string.IsNullOrEmpty(strProjectName))
        //{
        //    strProjectHQL += " and ProjectName like '%" + strProjectName + "%'";
        //}
        //if (!string.IsNullOrEmpty(strProgress))
        //{
        //    strProjectHQL += " and Progress = '" + strProgress + "'";
        //}
        //strProjectHQL += " order by MarkTime desc";
        //IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        //DG_List.DataSource = listProject;
        //DG_List.DataBind();


        //LB_ProjectSql.Text = strProjectHQL;
        #endregion
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
                string strIsStatus = arrOperate[2];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strIsStatus + "');", true);
                
                HF_ProjectCode.Value = strEditProjectCode;
                HF_Progress.Value = strProgress;
                HF_IsStatus.Value = strIsStatus;
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRSBYXSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZProjectBLL.DeleteWZProject(wZProject);

                    //���¼����б�
                    string strProjectCode = TXT_ProjectCode.Text.Trim();
                    string strProjectName = TXT_ProjectName.Text.Trim();
                    string strProgress = DDL_Progress.SelectedValue;

                    DataBinder(strProjectCode, strProjectName, strProgress);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }

            }
            else if (cmdName == "project")
            {
                //����
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress == LanguageHandle.GetWord("LuRu").ToString().Trim())
                    {
                        wZProject.Progress = LanguageHandle.GetWord("LiXiang").ToString().Trim();
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //���¼����б�
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLXCG").ToString().Trim()+"')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRSBYXLX").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "projectReturn")
            {
                //�����˻�
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.Progress == LanguageHandle.GetWord("LiXiang").ToString().Trim())
                    {
                        wZProject.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                        wZProject.StoreRoom = "";
                        wZProject.DelegateAgent = "";
                        wZProject.PurchaseManager = "";
                        wZProject.PurchaseEngineer = "";
                        wZProject.Contracter = "";
                        wZProject.Checker = "";
                        wZProject.Safekeep = "";
                        wZProject.SupplementEditor = "-";

                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //���¼����б�
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLXTHCG").ToString().Trim()+"')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLXSBYXLXTH").ToString().Trim()+"')", true);
                        return;
                    }
                }
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
                        wZProject.Progress = LanguageHandle.GetWord("KaiGong").ToString().Trim();
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //���¼����б�
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKGCG").ToString().Trim()+"')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLXSBYXKG").ToString().Trim()+"')", true);
                        return;
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
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWKGSBYXKGTH").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "cancel")
            {
                //����
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
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
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSTRMESSAGE").ToString().Trim()+"')", true);
                                return;
                            }

                            wZProject.Progress = LanguageHandle.GetWord("HeXiao").ToString().Trim();
                            wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                            //���¼����б�
                            string strProjectCode = TXT_ProjectCode.Text.Trim();
                            string strProjectName = TXT_ProjectName.Text.Trim();
                            string strProgress = DDL_Progress.SelectedValue;

                            DataBinder(strProjectCode, strProjectName, strProgress);


                            // �������������Ϊ��ӪN�⣬�Ƿ�һ������
                            if (!wZProject.StoreRoom.Contains(LanguageHandle.GetWord("ZiYing").ToString().Trim()))
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHXCG").ToString().Trim()+"')", true);
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
            else if (cmdName == "cancelReturn")
            {
                //�����˻�
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
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
                            wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

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

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHXTHCG").ToString().Trim()+"')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWHXSBYXHXTH").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "close")
            {
                //��Ŀ�ر�
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.IsStatus == "Normal")
                    {
                        wZProject.IsStatus = "Closed";
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //���¼����б�
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMGBCG").ToString().Trim()+"')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWZCBYXGB").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "notClose")
            {
                //�ر��˻�
                string cmdArges = e.CommandArgument.ToString();
                WZProjectBLL wZProjectBLL = new WZProjectBLL();
                string strProjectSql = "from WZProject as wZProject where ProjectCode = '" + cmdArges + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strProjectSql);
                if (projectList != null && projectList.Count == 1)
                {
                    WZProject wZProject = (WZProject)projectList[0];
                    if (wZProject.IsStatus == "Closed")
                    {
                        wZProject.IsStatus = "Normal";
                        wZProjectBLL.UpdateWZProject(wZProject, cmdArges);

                        //���¼����б�
                        string strProjectCode = TXT_ProjectCode.Text.Trim();
                        string strProjectName = TXT_ProjectName.Text.Trim();
                        string strProgress = DDL_Progress.SelectedValue;

                        DataBinder(strProjectCode, strProjectName, strProgress);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGBTHCG").ToString().Trim()+"')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWGBBYXGBTH").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
            else if (cmdName == "browse")
            {
                //���
                string cmdArges = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectBrowse.aspx?strProjectCode=" + cmdArges + "')", true);
                return;
            }
            else if (cmdName == "edit")
            {
                //�༭
                string cmdArges = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectAdd.aspx?strProjectCode=" + cmdArges + "')", true);
                return;
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_ProjectSql.Text.Trim();
        DataTable dtProject = ShareClass.GetDataSetFromSql(strHQL, "Project").Tables[0];

        DG_List.DataSource = dtProject;
        DG_List.DataBind();

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


            //���¼����б�
            string strProjectCode = TXT_ProjectCode.Text.Trim();
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProgress = DDL_Progress.SelectedValue;

            DataBinder(strProjectCode, strProjectName, strProgress);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZZSYBJWC").ToString().Trim()+"')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZSMYJDZKGDXMSHYKGXMSZZZSYBJ").ToString().Trim()+"')", true);
            return;
        }
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

            //���¼����б�
            string strControlProjectCode = TXT_ProjectCode.Text.Trim();
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProgress = DDL_Progress.SelectedValue;

            DataBinder(strControlProjectCode, strProjectName, strProgress);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��Ŀͳ����ɣ�');ControlStatusCloseChange();", true);   //ChineseWord
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ʱû����Ŀ�����Ժ�����Ŀʱ������Ŀͳ�ƣ�');ControlStatusCloseChange();", true);   //ChineseWord
            return;
        }
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
                        where p.Marker = '{0}' ", strUserCode);

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
                        where p.Marker = '{0}'", strUserCode);

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
    ///  ������������
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
                        where p.Marker = '{0}'", strUserCode);

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
            strWZProjectHQL += " order by p.StartTime desc";

            HF_SortStartTime.Value = "";
        }
        else
        {
            strWZProjectHQL += " order by p.StartTime asc";

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

    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        //�������������ѯ�����
        string strProjectCode = TXT_ProjectCode.Text.Trim();
        string strProjectName = TXT_ProjectName.Text.Trim();
        string strProgress = DDL_Progress.SelectedValue;

        DataBinder(strProjectCode, strProjectName, strProgress);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }




    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //�༭
        string strEditProjectCode = HF_ProjectCode.Value;
        if (string.IsNullOrEmpty(strEditProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDXM").ToString().Trim()+"')", true);
            return;
        }

        string strProgress = HF_Progress.Value;
        string strIsStatus = HF_IsStatus.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectAdd.aspx?strProjectCode=" + strEditProjectCode + "');ControlStatusChange('" + strProgress + "','" + strIsStatus + "');", true);
        
    }



    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //ɾ��
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
            if (wZProject.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRSBYXSC").ToString().Trim()+"')", true);
                return;
            }

            wZProjectBLL.DeleteWZProject(wZProject);

            //���¼����б�
            string strProjectCode = TXT_ProjectCode.Text.Trim();
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProgress = DDL_Progress.SelectedValue;

            DataBinder(strProjectCode, strProjectName, strProgress);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
        }
    }



    protected void BT_NewProject_Click(object sender, EventArgs e)
    {
        //����
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
            if (wZProject.Progress == LanguageHandle.GetWord("LuRu").ToString().Trim())
            {
                wZProject.Progress = LanguageHandle.GetWord("LiXiang").ToString().Trim();
                wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                //���¼����б�
                string strProjectCode = TXT_ProjectCode.Text.Trim();
                string strProjectName = TXT_ProjectName.Text.Trim();
                string strProgress = DDL_Progress.SelectedValue;

                DataBinder(strProjectCode, strProjectName, strProgress);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLXCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLRSBYXLX").ToString().Trim()+"');ControlStatusCloseChange();", true);
                return;
            }
        }
    }



    protected void BT_NewNotProject_Click(object sender, EventArgs e)
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
            if (wZProject.Progress == LanguageHandle.GetWord("LiXiang").ToString().Trim())
            {
                wZProject.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                wZProject.StoreRoom = "";
                wZProject.DelegateAgent = "";
                wZProject.PurchaseManager = "";
                wZProject.PurchaseEngineer = "";
                wZProject.Contracter = "";
                wZProject.Checker = "";
                wZProject.Safekeep = "";
                wZProject.SupplementEditor = "-";


                wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                //���¼����б�
                string strProjectCode = TXT_ProjectCode.Text.Trim();
                string strProjectName = TXT_ProjectName.Text.Trim();
                string strProgress = DDL_Progress.SelectedValue;

                DataBinder(strProjectCode, strProjectName, strProgress);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLXTHCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWLXSBYXLXTH").ToString().Trim()+"');ControlStatusCloseChange();", true);
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

        string strProgress =HF_Progress.Value;
        string strIsStatus = HF_IsStatus.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZProjectBrowse.aspx?strProjectCode=" + strEditProjectCode + "');ControlStatusChange('" + strProgress + "','" + strIsStatus + "');", true);
        
    }



    protected void BT_NewProjectClose_Click(object sender, EventArgs e)
    {
        //��Ŀ�ر�
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
            if (wZProject.IsStatus == "Normal")
            {
                wZProject.IsStatus = "Closed";
                wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                //���¼����б�
                string strProjectCode = TXT_ProjectCode.Text.Trim();
                string strProjectName = TXT_ProjectName.Text.Trim();
                string strProgress = DDL_Progress.SelectedValue;

                DataBinder(strProjectCode, strProjectName, strProgress);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMGBCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWZCBYXGB").ToString().Trim()+"')", true);
                return;
            }
        }
    }



    protected void BT_NewReturnProject_Click(object sender, EventArgs e)
    {
        //�ر��˻�
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
            if (wZProject.IsStatus == "Closed")
            {
                wZProject.IsStatus = "Normal";
                wZProjectBLL.UpdateWZProject(wZProject, strEditProjectCode);

                //���¼����б�
                string strProjectCode = TXT_ProjectCode.Text.Trim();
                string strProjectName = TXT_ProjectName.Text.Trim();
                string strProgress = DDL_Progress.SelectedValue;

                DataBinder(strProjectCode, strProjectName, strProgress);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGBTHCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWGBBYXGBTH").ToString().Trim()+"')", true);
                return;
            }
        }
    }
    
}
