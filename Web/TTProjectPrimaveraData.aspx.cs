using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTProjectPrimaveraData : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","项目导入/导出", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            BindDDLOther();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)//导入项目WBS
    {
        if (ddl_Code.SelectedValue.Trim() == "" || string.IsNullOrEmpty(ddl_Code.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYXDDCJKBMJC")+"')", true);
            return;
        }
        AddOrUpdatePrimProjectList();
    }

    protected void Button2_Click(object sender, EventArgs e)//导入项目费用
    {
        if (ddl_Code.SelectedValue.Trim() == "" || string.IsNullOrEmpty(ddl_Code.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYXDDCJKBMJC")+"')", true);
            return;
        }
        AddOrUpdatePrimProjCostList();
    }

    protected void BindDDLOther()
    {
        string strHQL = "from ProjectDataLink as projectDataLink Order by projectDataLink.Code ASC";
        ProjectDataLinkBLL projectDataLinkBLL = new ProjectDataLinkBLL();
        IList lst = projectDataLinkBLL.GetAllProjectDataLinks(strHQL);
        ddl_Code.DataSource = lst;
        ddl_Code.DataBind();
        ddl_Code.Items.Insert(0, new ListItem("--Select--", ""));
    }

    private void CreateExcel(DataTable dt, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }
    
    /// <summary>
    /// 获取P6项目WBS数据
    /// </summary>
    /// <returns></returns>
    protected DataTable getPrimProjectList()
    {
        DataTable dt = new DataTable();
        ///创建链接
        SqlConnection con = new SqlConnection(GetDataBaseLinkAddress(ddl_Code.SelectedValue.Trim()));
        ///定义SQL语句
        string cmdText = "SELECT proj_id 'ProjectID',proj_short_name 'ProjectCode',guid 'ProjectIdentificationCode',plan_start_date 'ProjectStartDate'," +   
            "COALESCE(plan_end_date,plan_start_date) 'ProjectEndDate',create_date 'ProjectCreationTime' FROM PROJECT Order by proj_id ASC ";   
        ///打开链接
        con.Open();
        ///创建Command
        SqlDataAdapter da = new SqlDataAdapter(cmdText, con);
        da.Fill(dt);
        return dt;
    }

    /// <summary>
    /// 直接把P6项目WBS数据导入到中油二建项目管理平台
    /// </summary>
    protected void AddOrUpdatePrimProjectList()
    {
        DataTable dt = getPrimProjectList();
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();
        ProjectPrimaveraBLL projectPrimaveraBLL = new ProjectPrimaveraBLL();
        ProjectPrimavera projectPrimavera = new ProjectPrimavera();
        DataRow[] dr = dt.Select();                        //定义一个DataRow数组
        int rowsnum = dt.Rows.Count;
        if (rowsnum == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGP6XMWBSBWSJ")+"')", true);
        }
        else
        {
            #region [导入项目中 新增或更新]
            for (int i = 0; i < dr.Length; i++)
            {
                project.AcceptStandard = "";
                project.BeginDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("XiangMuKaiShiRiJi")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("XiangMuKaiShiRiJi")].ToString());
                project.Budget = 0;
                project.CustomerPMName = "";
                project.EndDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("XiangMuJieShuRiJi")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("XiangMuJieShuRiJi")].ToString());
                project.FinishPercent = 0;
                project.MakeDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("XiangMuChuangJianShiJian")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("XiangMuChuangJianShiJian")].ToString());
                project.ManHour = 0;
                project.ManNumber = 0;
                project.ParentID = 0;
                project.PMCode = strUserCode.Trim();
                project.PMName = GetUserName(strUserCode).Trim();
                project.ProjectAmount = 0;
                project.ProjectClass = "NormalProject";
                project.ProjectDetail = "";
                project.ProjectName = getPrimProjName(dr[i][LanguageHandle.GetWord("XiangMuBianHao")].ToString(), dr[i][LanguageHandle.GetWord("XiangMuDaiMa")].ToString()).Trim();
                project.ProjectType = "PrimaveraProject";   
                project.Status = "New";
                project.StatusValue = "InProgress";
                project.UserCode = strUserCode.Trim();
                project.UserName = GetUserName(strUserCode).Trim();
                int ProjectId = getProjectIdByPrimavera(dr[i][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString());

                if (ProjectId == 0)//新增
                {
                    projectBLL.AddProject(project);

                    projectPrimavera.BeginDate = project.BeginDate;
                    projectPrimavera.EndDate = project.EndDate;
                    projectPrimavera.Guid = dr[i][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString().Trim();
                    projectPrimavera.MakeDate = project.MakeDate;
                    projectPrimavera.ProjectCode = dr[i][LanguageHandle.GetWord("XiangMuDaiMa")].ToString().Trim();
                    projectPrimavera.ProjectID = getMaxProjectId(project);
                    projectPrimavera.ProjectName = project.ProjectName;

                    projectPrimaveraBLL.AddProjectPrimavera(projectPrimavera);
                }
                else//更新
                {
                    string strHQL = "from Project as project where project.ProjectID='" + ProjectId.ToString().Trim() + "' ";
                    IList lst = projectBLL.GetAllProjects(strHQL);
                    Project project1 = (Project)lst[0];
                    project1.BeginDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("XiangMuKaiShiRiJi")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("XiangMuKaiShiRiJi")].ToString());
                    project1.EndDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("XiangMuJieShuRiJi")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("XiangMuJieShuRiJi")].ToString());
                    project1.MakeDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("XiangMuChuangJianShiJian")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("XiangMuChuangJianShiJian")].ToString());
                    project1.PMCode = strUserCode.Trim();
                    project1.PMName = GetUserName(strUserCode).Trim();
                    project1.ProjectName = getPrimProjName(dr[i][LanguageHandle.GetWord("XiangMuBianHao")].ToString(), dr[i][LanguageHandle.GetWord("XiangMuDaiMa")].ToString()).Trim();
                    project1.UserCode = strUserCode.Trim();
                    project1.UserName = GetUserName(strUserCode).Trim();

                    projectBLL.UpdateProject(project1, ProjectId);

                    strHQL = "from ProjectPrimavera as projectPrimavera where projectPrimavera.ProjectID='" + ProjectId.ToString().Trim() + "' ";
                    lst = projectPrimaveraBLL.GetAllProjectPrimaveras(strHQL);
                    ProjectPrimavera projectPrimavera1 = (ProjectPrimavera)lst[0];
                    projectPrimavera1.BeginDate = project1.BeginDate;
                    projectPrimavera1.EndDate = project1.EndDate;
                    projectPrimavera1.Guid = dr[i][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString().Trim();
                    projectPrimavera1.MakeDate = project1.MakeDate;
                    projectPrimavera1.ProjectCode = dr[i][LanguageHandle.GetWord("XiangMuDaiMa")].ToString().Trim();
                    projectPrimavera1.ProjectID = ProjectId;
                    projectPrimavera1.ProjectName = project1.ProjectName;

                    projectPrimaveraBLL.UpdateProjectPrimavera(projectPrimavera1, projectPrimavera1.ID);
                }
            }
            #endregion

            #region [更新项目中的父级项目编号]
            for (int j = 0; j < dr.Length; j++)
            {
                //项目Id
                string strGuid = dr[j][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString().Trim();
                int strProjId = getProjectIdByPrimavera(strGuid);

                //父级项目ID
                string strParentGuid = getPrimProjGuid(getPrimProjParentId(dr[j][LanguageHandle.GetWord("XiangMuBianHao")].ToString(), dr[j][LanguageHandle.GetWord("XiangMuDaiMa")].ToString()).ToString()).Trim();
                int strParentProjId = getProjectIdByPrimavera(strParentGuid);

                string strHQL = "from Project as project where project.ProjectID='" + strProjId.ToString().Trim() + "' ";
                IList lst = projectBLL.GetAllProjects(strHQL);
                Project project1 = (Project)lst[0];

                project1.ParentID = strParentProjId == 0 ? GetProjectIdByParentId() : strParentProjId;

                projectBLL.UpdateProject(project1, strProjId);
            }
            #endregion

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMWBSSJDRCG")+"')", true);
        }
    }

    /// <summary>
    /// 获取总项目的编号
    /// </summary>
    /// <returns></returns>
    protected int GetProjectIdByParentId()
    {
        ProjectBLL projectBLL = new ProjectBLL();
        string strHQL = "from Project as project where project.ParentID=0 Order by project.ProjectID ASC";
        IList lst = projectBLL.GetAllProjects(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            Project project = (Project)lst[0];
            return project.ProjectID;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// 获取P6项目费用数据
    /// </summary>
    /// <returns></returns>
    protected DataTable getPrimProjCostList()
    {
        DataTable dt = new DataTable();
        ///创建链接
        SqlConnection con = new SqlConnection(GetDataBaseLinkAddress(ddl_Code.SelectedValue.Trim()));
        ///定义SQL语句
        string cmdText = "SELECT cost_item_id 'SerialNumber',A.proj_id 'ProjectID',A.task_id 'JobNumber',target_cost 'BudgetedCostAtCompletion',act_cost 'ActualCostAtCompletion'," +   
            "act_cost+remain_cost 'TotalCostAtCompletion',A.create_date 'CreationTime',B.guid 'ProjectIdentificationCode',C.guid 'JobIdentificationCode' FROM PROJCOST as A,PROJECT as B,TASK as C " +   
            "Where A.proj_id=B.proj_id and A.task_id=C.task_id ";
        ///打开链接
        con.Open();
        ///创建Command
        SqlDataAdapter da = new SqlDataAdapter(cmdText, con);
        da.Fill(dt);
        return dt;
    }

    /// <summary>
    /// 直接把P6项目费用数据导入到中油二建项目管理平台
    /// </summary>
    protected void AddOrUpdatePrimProjCostList()
    {
        DataTable dt = getPrimProjCostList();
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        ProjectBudget projectBudget = new ProjectBudget();
        ProjectPrimaveraBudgetBLL projectPrimaveraBudgetBLL = new ProjectPrimaveraBudgetBLL();
        ProjectPrimaveraBudget projectPrimaveraBudget = new ProjectPrimaveraBudget();
        ProjectBLL projectBLL = new ProjectBLL();
        DataRow[] dr = dt.Select();                        //定义一个DataRow数组
        int rowsnum = dt.Rows.Count;
        if (rowsnum == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGP6XMFYBWSJ")+"')", true);
        }
        else
        {
            #region [导入项目费用]
            for (int i = 0; i < dr.Length; i++)
            {
                //项目Id
                string strGuid = dr[i][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString().Trim();
                int strProjId = getProjectIdByPrimavera(strGuid);
                //作业ID
                string strtaskGuid = dr[i][LanguageHandle.GetWord("ZuoYeBiaoShiMa")].ToString().Trim();
                int strTaskId = getTaskIdByPrimavera(strtaskGuid);
                if (strProjId > 0 && strTaskId > 0)
                {
                    projectBudget.ProjectID = strProjId;
                    projectBudget.Account = "Other";
                    projectBudget.Amount = decimal.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("WanChengShiYuSuanFeiYong")].ToString()) ? "0" : dr[i][LanguageHandle.GetWord("WanChengShiYuSuanFeiYong")].ToString());
                    projectBudget.CreateTime = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("ChuangJianShiJian")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("ChuangJianShiJian")].ToString());
                    projectBudget.CreatorCode = strUserCode.Trim();
                    projectBudget.CreatorName = GetUserName(strUserCode).Trim();
                    projectBudget.Description = LanguageHandle.GetWord("primavera6DaoRuXiangMuFeiYong");

                    DeleteProjectPrimaveraBudget(dr[i][LanguageHandle.GetWord("XuHao")].ToString().Trim());//导入前，先删除原来数据(有的话则删除)

                    projectBudgetBLL.AddProjectBudget(projectBudget);

                    projectPrimaveraBudget.BudgetAmount = decimal.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("WanChengShiYuSuanFeiYong")].ToString().Trim()) ? "0" : dr[i][LanguageHandle.GetWord("WanChengShiYuSuanFeiYong")].ToString().Trim());
                    projectPrimaveraBudget.ProjBudgID = getMaxProjectBudgetId(projectBudget);
                    projectPrimaveraBudget.ProjGuid = dr[i][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString().Trim();
                    projectPrimaveraBudget.ProjectID = projectBudget.ProjectID;
                    projectPrimaveraBudget.RealAmount = decimal.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("WanChengShiShiJiFeiYong")].ToString().Trim()) ? "0" : dr[i][LanguageHandle.GetWord("WanChengShiShiJiFeiYong")].ToString().Trim());
                    projectPrimaveraBudget.TotalAmount = decimal.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("WanChengShiZongFeiYong")].ToString().Trim()) ? "0" : dr[i][LanguageHandle.GetWord("WanChengShiZongFeiYong")].ToString().Trim());
                    projectPrimaveraBudget.TaskID = strTaskId;
                    projectPrimaveraBudget.P6ID = int.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("XuHao")].ToString().Trim()) ? "0" : dr[i][LanguageHandle.GetWord("XuHao")].ToString().Trim());

                    projectPrimaveraBudgetBLL.AddProjectPrimaveraBudget(projectPrimaveraBudget);

                    string strHQL = "from Project as project where project.ProjectID='" + strProjId.ToString().Trim() + "' ";
                    IList lst = projectBLL.GetAllProjects(strHQL);
                    Project project = (Project)lst[0];
                    project.Budget = getProjectBudget(strProjId.ToString().Trim());
                    projectBLL.UpdateProject(project, strProjId);
                }
            }
            #endregion

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMFYSJDRCG")+"')", true);
        }
    }

    protected DataTable getPrimProjTaskList()
    {
        DataTable dt = new DataTable();
        ///创建链接
        SqlConnection con = new SqlConnection(GetDataBaseLinkAddress(ddl_Code.SelectedValue.Trim()));
        ///定义SQL语句
        string cmdText = "SELECT task_id 'JobNumber',A.proj_id 'ProjectID',wbs_id 'WBSCode',task_code 'JobCode',task_name 'JobName',A.guid 'JobIdentificationCode',A.create_date 'CreationTime',"+   
            "COALESCE(target_start_date,restart_date) 'JobStartDate',COALESCE(target_end_date,reend_date) 'JobEndDate',B.guid 'ProjectIdentificationCode' FROM TASK as A,PROJECT as B where A.proj_id=B.proj_id ";   
        ///打开链接
        con.Open();
        ///创建Command
        SqlDataAdapter da = new SqlDataAdapter(cmdText, con);
        da.Fill(dt);
        return dt;
    }

    /// <summary>
    /// 直接把P6项目作业数据导入到中油二建项目管理平台
    /// </summary>
    protected void AddOrUpdatePrimProjTaskList()
    {
        DataTable dt = getPrimProjTaskList();
        ProjectPrimaveraTaskBLL projectPrimaveraTaskBLL = new ProjectPrimaveraTaskBLL();
        ProjectPrimaveraTask projectPrimaveraTask = new ProjectPrimaveraTask();
        ProjectPrimaveraBLL projectPrimaveraBLL = new ProjectPrimaveraBLL();
        DataRow[] dr = dt.Select();                        //定义一个DataRow数组
        int rowsnum = dt.Rows.Count;
        if (rowsnum == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGP6XMZYBWSJ")+"')", true);
        }
        else
        {
            #region [导入项目作业]
            for (int i = 0; i < dr.Length; i++)
            {
                //项目Id
                string strGuid = dr[i][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString().Trim();
                int strProjId = getProjectIdByPrimavera(strGuid);
                if (strProjId == 0)
                {
                }
                else
                {
                    projectPrimaveraTask.ProjectID = strProjId;
                    projectPrimaveraTask.TaskCode = dr[i][LanguageHandle.GetWord("ZuoYeBianMa")].ToString().Trim();
                    projectPrimaveraTask.BeginDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("ZuoYeKaiShiRiJi")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("ZuoYeKaiShiRiJi")].ToString());
                    projectPrimaveraTask.EndDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("ZuoYeJieShuRiJi")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("ZuoYeJieShuRiJi")].ToString());
                    projectPrimaveraTask.CreateDate = DateTime.Parse(string.IsNullOrEmpty(dr[i][LanguageHandle.GetWord("ChuangJianShiJian")].ToString()) ? DateTime.Now.ToString() : dr[i][LanguageHandle.GetWord("ChuangJianShiJian")].ToString());
                    projectPrimaveraTask.ProjGuid = dr[i][LanguageHandle.GetWord("XiangMuBiaoShiMa")].ToString().Trim();
                    projectPrimaveraTask.TaskGuid = dr[i][LanguageHandle.GetWord("ZuoYeBiaoShiMa")].ToString().Trim();
                    projectPrimaveraTask.TaskName = dr[i][LanguageHandle.GetWord("ZuoYeMingChen")].ToString().Trim();

                    DeleteProjectPrimaveraTask(dr[i][LanguageHandle.GetWord("ZuoYeBiaoShiMa")].ToString().Trim());//导入前，先删除原来数据(有的话则删除)

                    projectPrimaveraTaskBLL.AddProjectPrimaveraTask(projectPrimaveraTask);
                }
            }
            #endregion

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMZYSJDRCG")+"')", true);
        }
    }

    protected string GetDataBaseLinkAddress(string strcode)
    {
        string strconnection = string.Empty;
        string strHQL = "from ProjectDataLink as projectDataLink Where projectDataLink.Code='" + strcode + "' ";
        ProjectDataLinkBLL projectDataLinkBLL = new ProjectDataLinkBLL();
        IList lst = projectDataLinkBLL.GetAllProjectDataLinks(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectDataLink projectDataLink = (ProjectDataLink)lst[0];
            strconnection = "data Source=" + projectDataLink.Host.Trim() + ";database=" + projectDataLink.DataBaseName.Trim() + ";user id=" + projectDataLink.LoginNo.Trim() + ";pwd=" + projectDataLink.PassWord.Trim();
            return strconnection;
        }
        else
            return strconnection;
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        if (ddl_Code.SelectedValue.Trim() == "" || string.IsNullOrEmpty(ddl_Code.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYXDDCJKBMJC")+"')", true);
            return;
        }
        AddOrUpdatePrimProjTaskList();
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    /// <summary>
    /// 获取项目的父级项目编号
    /// </summary>
    /// <param name="strprojid"></param>
    /// <param name="strprojcode"></param>
    /// <returns></returns>
    protected int getPrimProjParentId(string strprojid, string strprojcode)
    {
        int strparentid = 0;
        DataTable dt = new DataTable();
        ///创建链接
        SqlConnection con = new SqlConnection(GetDataBaseLinkAddress(ddl_Code.SelectedValue.Trim()));
        ///定义SQL语句
        string cmdText = "SELECT * FROM PROJWBS where proj_id='" + strprojid + "' and wbs_short_name='" + strprojcode + "' ";
        ///打开链接
        con.Open();
        ///创建Command
        SqlDataAdapter da = new SqlDataAdapter(cmdText, con);
        da.Fill(dt);
        if (dt.Rows.Count > 0 && dt != null)
        {
            string wbsparentid = dt.Rows[0]["parent_wbs_id"].ToString().Trim();
            if (string.IsNullOrEmpty(wbsparentid) || wbsparentid == "")
            {
                strparentid = 0;
            }
            else
            {
                strparentid = getPrimWBSProjId(wbsparentid);
            }
        }
        else
            strparentid = 0;
        return strparentid;
    }

    /// <summary>
    /// 获取项目名称
    /// </summary>
    /// <param name="strprojid"></param>
    /// <param name="strprojcode"></param>
    /// <returns></returns>
    protected string getPrimProjName(string strprojid, string strprojcode)
    {
        DataTable dt = new DataTable();
        ///创建链接
        SqlConnection con = new SqlConnection(GetDataBaseLinkAddress(ddl_Code.SelectedValue.Trim()));
        ///定义SQL语句
        string cmdText = "SELECT * FROM PROJWBS where proj_id='" + strprojid + "' and wbs_short_name='" + strprojcode + "' ";
        ///打开链接
        con.Open();
        ///创建Command
        SqlDataAdapter da = new SqlDataAdapter(cmdText, con);
        da.Fill(dt);
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["wbs_name"].ToString().Trim();
        }
        else
            return "";
    }

    /// <summary>
    /// 根据WBS编号，获取父级项目编号
    /// </summary>
    /// <param name="strwbsid"></param>
    /// <returns></returns>
    protected int getPrimWBSProjId(string strwbsid)
    {
        int strprojid = 0;
        DataTable dt = new DataTable();
        ///创建链接
        SqlConnection con = new SqlConnection(GetDataBaseLinkAddress(ddl_Code.SelectedValue.Trim()));
        ///定义SQL语句
        string cmdText = "SELECT * FROM PROJWBS where wbs_id='" + strwbsid + "' ";
        ///打开链接
        con.Open();
        ///创建Command
        SqlDataAdapter da = new SqlDataAdapter(cmdText, con);
        da.Fill(dt);
        if (dt.Rows.Count > 0 && dt != null)
        {
            string projid = dt.Rows[0]["proj_id"].ToString().Trim();
            if (string.IsNullOrEmpty(projid) || projid == "")
            {
                strprojid = 0;
            }
            else
            {
                strprojid = int.Parse(projid);
            }
        }
        else
            strprojid = 0;
        return strprojid;
    }

    /// <summary>
    /// 根据父级项目编号获取父级项目Guid值
    /// </summary>
    /// <param name="strprojid"></param>
    /// <returns></returns>
    protected string getPrimProjGuid(string strprojid)
    {
        DataTable dt = new DataTable();
        ///创建链接
        SqlConnection con = new SqlConnection(GetDataBaseLinkAddress(ddl_Code.SelectedValue.Trim()));
        ///定义SQL语句
        string cmdText = "SELECT * FROM PROJECT where proj_id='" + strprojid + "' ";
        ///打开链接
        con.Open();
        ///创建Command
        SqlDataAdapter da = new SqlDataAdapter(cmdText, con);
        da.Fill(dt);
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["guid"].ToString().Trim();
        }
        else
            return "";
    }

    /// <summary>
    /// 获取项目的即时编号
    /// </summary>
    /// <param name="pj"></param>
    /// <returns></returns>
    protected int getMaxProjectId(Project pj)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectName='" + pj.ProjectName.Trim() + "' and project.ProjectType = 'PrimaveraProject' and project.PMCode = '" + pj.PMCode.Trim() + "' and project.UserCode = '" + pj.UserCode.Trim() + "' Order by project.ProjectID DESC";   
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.ProjectID;
    }

    /// <summary>
    /// 通过T_ProjectPrimavera表，获取项目的编号
    /// </summary>
    /// <param name="pj"></param>
    /// <returns></returns>
    protected int getProjectIdByPrimavera(string strguid)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPrimavera as projectPrimavera where projectPrimavera.Guid='" +strguid + "' ";
        ProjectPrimaveraBLL projectPrimaveraBLL = new ProjectPrimaveraBLL();
        lst = projectPrimaveraBLL.GetAllProjectPrimaveras(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectPrimavera projectPrimavera = (ProjectPrimavera)lst[0];
            return projectPrimavera.ProjectID;
        }
        else
            return 0;
    }

    /// <summary>
    /// 通过T_ProjectPrimaveraTask表，获取作业的编号
    /// </summary>
    /// <param name="pj"></param>
    /// <returns></returns>
    protected int getTaskIdByPrimavera(string strguid)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPrimaveraTask as projectPrimaveraTask where projectPrimaveraTask.TaskGuid='" + strguid + "' ";
        ProjectPrimaveraTaskBLL projectPrimaveraTaskBLL = new ProjectPrimaveraTaskBLL();
        lst = projectPrimaveraTaskBLL.GetAllProjectPrimaveraTasks(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectPrimaveraTask projectPrimaveraTask = (ProjectPrimaveraTask)lst[0];
            return projectPrimaveraTask.ID;
        }
        else
            return 0;
    }

    /// <summary>
    /// 通过T_ProjectPrimaveraBudget表的P6ID，删除对应的ProjectPrimaveraBudget及ProjectBudget数据
    /// </summary>
    /// <param name="pj"></param>
    /// <returns></returns>
    protected void DeleteProjectPrimaveraBudget(string strP6ID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPrimaveraBudget as projectPrimaveraBudget where projectPrimaveraBudget.P6ID='" + strP6ID + "' ";
        ProjectPrimaveraBudgetBLL projectPrimaveraBudgetBLL = new ProjectPrimaveraBudgetBLL();
        lst = projectPrimaveraBudgetBLL.GetAllProjectPrimaveraBudgets(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectPrimaveraBudget projectPrimaveraBudget = (ProjectPrimaveraBudget)lst[0];
            strHQL = "Delete From T_ProjectBudget Where ID = '" + projectPrimaveraBudget.ProjBudgID.ToString() + "' ";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ProjectPrimaveraBudget Where P6ID = '" + strP6ID + "' ";
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    /// <summary>
    /// 获取项目费用的即时编号
    /// </summary>
    /// <param name="pj"></param>
    /// <returns></returns>
    protected int getMaxProjectBudgetId(ProjectBudget pj)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectBudget as projectBudget where projectBudget.Account='" + pj.Account.Trim() + "' and projectBudget.Description = '" + pj.Description.Trim() + "' " +
            "and projectBudget.ProjectID = '" + pj.ProjectID.ToString().Trim() + "' and projectBudget.CreatorCode = '" + pj.CreatorCode.Trim() + "' and " +
            "projectBudget.Amount='" + pj.Amount.ToString().Trim() + "' Order by projectBudget.ID DESC";
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        ProjectBudget projectBudget = (ProjectBudget)lst[0];

        return projectBudget.ID;
    }

    /// <summary>
    /// 根据项目ID获取项目预算费用
    /// </summary>
    /// <param name="pj"></param>
    /// <returns></returns>
    protected decimal getProjectBudget(string strProjId)
    {
        decimal budget = 0;
        string strHQL = "from ProjectBudget as projectBudget where projectBudget.ProjectID = '" + strProjId.Trim() + "' ";
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        IList lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                ProjectBudget projectBudget = (ProjectBudget)lst[i];
                budget += projectBudget.Amount;
            }
        }
        else
            budget = 0;

        return budget;
    }

    /// <summary>
    /// 通过T_ProjectPrimaveraTask表的TaskGuid，删除对应的ProjectPrimaveraTask数据
    /// </summary>
    /// <param name="pj"></param>
    /// <returns></returns>
    protected void DeleteProjectPrimaveraTask(string strguid)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPrimaveraTask as projectPrimaveraTask where projectPrimaveraTask.TaskGuid='" + strguid + "' ";
        ProjectPrimaveraTaskBLL projectPrimaveraTaskBLL = new ProjectPrimaveraTaskBLL();
        lst = projectPrimaveraTaskBLL.GetAllProjectPrimaveraTasks(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            strHQL = "Delete From T_ProjectPrimaveraTask Where TaskGuid = '" + strguid + "' ";
            ShareClass.RunSqlCommand(strHQL);
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        if (ddl_Code.SelectedValue.Trim() == "" || string.IsNullOrEmpty(ddl_Code.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYXDDCJKBMJC")+"')", true);
            return;
        }
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("PrimXiangMuShuJu") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";

                CreateExcel(getPrimProjectList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC")+"')", true);
            }
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        if (ddl_Code.SelectedValue.Trim() == "" || string.IsNullOrEmpty(ddl_Code.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYXDDCJKBMJC")+"')", true);
            return;
        }
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("PrimXiangMuFeiYongShuJu") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";

                CreateExcel(getPrimProjCostList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC")+"')", true);
            }
        }
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        if (ddl_Code.SelectedValue.Trim() == "" || string.IsNullOrEmpty(ddl_Code.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZYXDDCJKBMJC")+"')", true);
            return;
        }
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("PrimXiangMuZuoYeShuJu") + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";

                CreateExcel(getPrimProjTaskList(), fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDCDSJYWJC")+"')", true);
            }
        }
    }
}
