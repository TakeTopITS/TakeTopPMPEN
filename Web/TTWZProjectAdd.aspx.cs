using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZProjectAdd : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
        strUserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            BindProjectAttribute();
            BindProjectNature();

            if (!string.IsNullOrEmpty(Request.QueryString["strProjectCode"]))
            {
                string strProjectCode = Request.QueryString["strProjectCode"].ToString();
                HF_ID.Value = strProjectCode;

                BindProjectData(strProjectCode);
            }
            else
            {
                TXT_MarkTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TXT_Marker.Text = strUserName;
                HF_Marker.Value = strUserCode;


                TXT_ProjectName.BackColor = Color.CornflowerBlue;
                TXT_ProjectManager.BackColor = Color.CornflowerBlue;
                TXT_StartTime.BackColor = Color.CornflowerBlue;
                TXT_EndTime.BackColor = Color.CornflowerBlue;
                TXT_PowerPurchase.BackColor = Color.CornflowerBlue;
                TXT_ForCost.BackColor = Color.CornflowerBlue;
                TXT_SelfCost.BackColor = Color.CornflowerBlue;
                TXT_BuildUnit.BackColor = Color.CornflowerBlue;
                TXT_SupervisorUnit.BackColor = Color.CornflowerBlue;
                TXT_ProjectDesc.BackColor = Color.CornflowerBlue;
                TXT_RelatedCode.BackColor = Color.CornflowerBlue;
                DDL_ProjectAttribute.BackColor = Color.CornflowerBlue;
                DDL_ProjectNature.BackColor = Color.CornflowerBlue;
            }
        }
    }




    protected void BT_PlatformProject_Click(object sender, EventArgs e)
    {
        string strPlatformID = HF_PlatformID.Value;
        if (!string.IsNullOrEmpty(strPlatformID))
        {
            string strHQL = "from Project as project where project.ProjectID = " + strPlatformID;

            ProjectBLL projectBLL = new ProjectBLL();
            IList lst = projectBLL.GetAllProjects(strHQL);

            Project project = (Project)lst[0];

            TXT_RelatedCode.Text = project.ProjectID.ToString();

            TXT_ProjectName.Text = project.ProjectName.Trim();


            HF_ProjectManager.Value = project.PMCode;
            TXT_ProjectManager.Text = project.PMName;

            TXT_StartTime.Text = project.BeginDate.ToString();
            TXT_EndTime.Text = project.EndDate.ToString();


            TXT_ProjectDesc.Text = project.ProjectDetail;
            TXT_MarkTime.Text = project.MakeDate.ToString();
            TXT_Marker.Text = project.UserName;
            HF_Marker.Value = project.UserCode;

            ProjectWZDetailBLL projectWZDetailBLL = new ProjectWZDetailBLL();
            strHQL = "from ProjectWZDetail as projectWZDetail where projectWZDetail.ProjectID = " + strPlatformID;
            lst = projectWZDetailBLL.GetAllProjectWZDetails(strHQL);
            if (lst != null && lst.Count > 0)
            {
                ProjectWZDetail projectWZDetail = (ProjectWZDetail)lst[0];


                TXT_PowerPurchase.SelectedValue = projectWZDetail.AuthorizedProcurement.Trim();//放到子级中
                TXT_ForCost.Text = projectWZDetail.ABudgetFor.ToString().Trim();
                TXT_SelfCost.Text = projectWZDetail.SincePurchaseBudget.ToString().Trim();
                TXT_BuildUnit.Text = projectWZDetail.ConstructionUnit.Trim();
                TXT_SupervisorUnit.Text = projectWZDetail.SupervisionUnit.Trim();


                HF_PlatformLeader.Value = projectWZDetail.Leader.Trim();
                HF_PlatformFeeManage.Value = projectWZDetail.FeeManage.Trim();
                HF_PlatformMaterialPerson.Value = projectWZDetail.MaterialPerson.Trim();
                HF_PlatformIsMark.Value = projectWZDetail.IsMark.ToString().Trim();

            }
            else
            {
                TXT_PowerPurchase.SelectedIndex = 0;
                TXT_ForCost.Text = "0";
                TXT_SelfCost.Text = "0";
                TXT_BuildUnit.Text = "";
                TXT_SupervisorUnit.Text = "";


                HF_PlatformLeader.Value = "";
                HF_PlatformFeeManage.Value = "";
                HF_PlatformMaterialPerson.Value = "";
                HF_PlatformIsMark.Value = "0";
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strProjectName = TXT_ProjectName.Text.Trim();
            string strProjectManager = HF_ProjectManager.Value;
            string strStartTime = TXT_StartTime.Text.Trim();
            string strEndTime = TXT_EndTime.Text.Trim();
            string strPowerPurchase = TXT_PowerPurchase.Text.Trim();

            string strForCost = TXT_ForCost.Text.Trim();
            string strSelfCost = TXT_SelfCost.Text.Trim();
            string strBuildUnit = TXT_BuildUnit.Text.Trim();
            string strSupervisorUnit = TXT_SupervisorUnit.Text.Trim();
            string strProjectDesc = TXT_ProjectDesc.Text.Trim();

            string strMarkTime = TXT_MarkTime.Text;
            string strMarker = HF_Marker.Value;

            string strRelatedCode = TXT_RelatedCode.Text.Trim();


            //主管领导，费控主管，材料员，使用标记
            string strLeader = HF_PlatformLeader.Value;
            string strFeeManage = HF_PlatformFeeManage.Value;
            string strMaterialPerson = HF_PlatformMaterialPerson.Value;
            string strIsMark = HF_PlatformIsMark.Value;
            int intIsMark = 0;
            int.TryParse(strIsMark, out intIsMark);


            if (string.IsNullOrEmpty(strProjectName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMCBYXWKBC+"')", true);
                return;
            }
            else
            {
                if (!ShareClass.CheckStringRight(strProjectName))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMCBNBHFFZF+"')", true);
                    return;
                }
                if (strProjectName.Length > 30)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMCBNCG30GZF+"')", true);
                    return;
                }
            }
            if (string.IsNullOrEmpty(strProjectManager))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMJLBYXWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strStartTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKGRBYXWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strEndTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWGRBYXWKBC+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strForCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJGGSBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strForCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJGGSBXWXSHZZS+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strSelfCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZGGSBYXWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strSelfCost))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZGGSBXWXSHZZS+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strBuildUnit))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJSDWBYXWKBC+"')", true);
                return;
            }
            else
            {
                if (strBuildUnit.Length > 20)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJSDWBNCG20GZF+"')", true);
                    return;
                }
            }
            if (string.IsNullOrEmpty(strSupervisorUnit))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJLDWBYXWKBC+"')", true);
                return;
            }
            else
            {
                if (strSupervisorUnit.Length > 20)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJLDWBNCG20GZF+"')", true);
                    return;
                }
            }
            if (!ShareClass.CheckStringRight(strProjectDesc))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMSBNBHFFZF+"')", true);
                return;
            }
            if (strProjectDesc.Length > 2000)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMSBNCG2000GZF+"')", true);
                return;
            }

            DateTime dtStartTime = DateTime.Now;
            if (!string.IsNullOrEmpty(strStartTime))
            {
                DateTime.TryParse(strStartTime, out dtStartTime);
            }
            DateTime dtEndTime = DateTime.Now;
            if (!string.IsNullOrEmpty(strEndTime))
            {
                DateTime.TryParse(strEndTime, out dtEndTime);
            }

            //if (dtStartTime < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKGRBNXYDSJ+"')", true);
            //    return;
            //}
            if (dtEndTime < dtStartTime)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWGRBXDYKGR+"')", true);
                return;
            }

            decimal decimalForCost = 0;
            if (!string.IsNullOrEmpty(strForCost))
            {
                decimal.TryParse(strForCost, out decimalForCost);
            }
            decimal decimalSelfCost = 0;
            if (!string.IsNullOrEmpty(strSelfCost))
            {
                decimal.TryParse(strSelfCost, out decimalSelfCost);
            }

            WZProjectBLL wZProjectBLL = new WZProjectBLL();

            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                string strProjectCode = HF_ID.Value;

                //判断项目名称是否重复
                string strCheckProjectNameSQL = string.Format(@"select * from T_WZProject
                            where ProjectName = '{0}'", strProjectName);
                DataTable dtCheckProjectName = ShareClass.GetDataSetFromSql(strCheckProjectNameSQL, "CheckProjectName").Tables[0];
                if (dtCheckProjectName != null && dtCheckProjectName.Rows.Count > 0)
                {
                    string strCheckProjectCode = ShareClass.ObjectToString(dtCheckProjectName.Rows[0]["ProjectCode"]);
                    if (strCheckProjectCode.Trim() != strProjectCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMCZFXG+"')", true);
                        return;
                    }
                }

                string strWZProjectSql = "from WZProject as wZProject where ProjectCode = '" + strProjectCode + "'";
                IList projectList = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
                if (projectList != null && projectList.Count > 0)
                {
                    WZProject wZProject = (WZProject)projectList[0];


                    if (wZProject.Progress == "录入")
                    {
                        wZProject.ProjectName = strProjectName;
                        wZProject.ProjectManager = strProjectManager;
                        wZProject.StartTime = dtStartTime;
                        wZProject.EndTime = dtEndTime;
                        wZProject.PowerPurchase = strPowerPurchase;
                        wZProject.ForCost = decimalForCost;
                        wZProject.SelfCost = decimalSelfCost;
                        wZProject.BuildUnit = strBuildUnit;
                        wZProject.SupervisorUnit = strSupervisorUnit;
                        wZProject.ProjectDesc = strProjectDesc;

                        wZProject.RelatedCode = strRelatedCode;

                        //连接平台字段
                        wZProject.Leader = strLeader;
                        wZProject.FeeManage = strFeeManage;
                        wZProject.Checker = strMaterialPerson;
                        wZProject.IsMark = intIsMark;

                        wZProjectBLL.UpdateWZProject(wZProject, strProjectCode);

                    }
                }

            }
            else
            {
                //判断项目名称是否重复
                string strCheckProjectNameSQL = string.Format(@"select * from T_WZProject
                            where ProjectName = '{0}'", strProjectName);
                DataTable dtCheckProjectName = ShareClass.GetDataSetFromSql(strCheckProjectNameSQL, "CheckProjectName").Tables[0];
                if (dtCheckProjectName != null && dtCheckProjectName.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMMCZFXG+"')", true);
                    return;
                }

                WZProject wZProject = new WZProject();
                wZProject.ProjectName = strProjectName;
                wZProject.ProjectManager = strProjectManager;
                wZProject.StartTime = dtStartTime;
                wZProject.EndTime = dtEndTime;
                wZProject.PowerPurchase = strPowerPurchase;
                wZProject.ForCost = decimalForCost;
                wZProject.SelfCost = decimalSelfCost;
                wZProject.BuildUnit = strBuildUnit;
                wZProject.SupervisorUnit = strSupervisorUnit;
                wZProject.ProjectDesc = strProjectDesc;

                wZProject.MarkTime = DateTime.Parse(strMarkTime);
                wZProject.Marker = strMarker;

                wZProject.Progress = "录入";
                wZProject.IsMark = 0;

                wZProject.SupplementEditor = "-";

                wZProject.RelatedCode = strRelatedCode;
                wZProject.IsStatus = "正常";


                //连接平台字段
                wZProject.Leader = strLeader;
                wZProject.FeeManage = strFeeManage;
                wZProject.Checker = strMaterialPerson;
                wZProject.IsMark = intIsMark;

                //增加
                wZProject.ProjectCode = CreateNewProjectCode();
                wZProjectBLL.AddWZProject(wZProject);
            }


            //Response.Redirect("TTWZProjectList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindProjectData(string strProjectCode)
    {
        string strWZProjectHQL = string.Format(@"select p.*,
                        pp.UserName as ProjectManagerName,
                        pd.UserName as DelegateAgentName,
                        pm.UserName as PurchaseManagerName,
                        pe.UserName as PurchaseEngineerName,
                        pc.UserName as ContracterName,
                        pk.UserName as CheckerName,
                        ps.UserName as SafekeepName
                        from T_WZProject p
                        left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
                        left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
                        left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
                        left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                        left join T_ProjectMember pc on p.Contracter = pc.UserCode
                        left join T_ProjectMember pk on p.Checker = pk.UserCode
                        left join T_ProjectMember ps on p.Safekeep = ps.UserCode
                        where p.ProjectCode = '{0}'", strProjectCode);
        DataTable dtProject = ShareClass.GetDataSetFromSql(strWZProjectHQL, "Project").Tables[0];
        if (dtProject != null && dtProject.Rows.Count > 0)
        {
            DataRow drProject = dtProject.Rows[0];

            TXT_ProjectCode.Text = ShareClass.ObjectToString(drProject["ProjectCode"]);
            TXT_ProjectName.Text = ShareClass.ObjectToString(drProject["ProjectName"]);
            TXT_ProjectManager.Text = ShareClass.ObjectToString(drProject["ProjectManagerName"]);
            HF_ProjectManager.Value = ShareClass.ObjectToString(drProject["ProjectManager"]);
            TXT_StartTime.Text = ShareClass.ObjectToString(drProject["StartTime"]);
            TXT_EndTime.Text = ShareClass.ObjectToString(drProject["EndTime"]);
            TXT_PowerPurchase.Text = ShareClass.ObjectToString(drProject["PowerPurchase"]);
            TXT_ForCost.Text = ShareClass.ObjectToString(drProject["ForCost"]);
            TXT_SelfCost.Text = ShareClass.ObjectToString(drProject["SelfCost"]);
            TXT_BuildUnit.Text = ShareClass.ObjectToString(drProject["BuildUnit"]);
            TXT_SupervisorUnit.Text = ShareClass.ObjectToString(drProject["SupervisorUnit"]);
            TXT_ProjectDesc.Text = ShareClass.ObjectToString(drProject["ProjectDesc"]);


            TXT_RelatedCode.Text = ShareClass.ObjectToString(drProject["RelatedCode"]);


            TXT_MarkTime.Text = ShareClass.ObjectToString(drProject["MarkTime"]);
            TXT_Marker.Text = strUserName;
            HF_Marker.Value = strUserCode;


            string strOldProjectCode = ShareClass.ObjectToString(drProject["ProjectCode"]);
            string strAttribute = strProjectCode.Substring(0, 1);
            DDL_ProjectAttribute.SelectedValue = strAttribute;
            string strNature = strProjectCode.Substring(5, 1);
            DDL_ProjectNature.SelectedValue = strNature;

            string strProgress = ShareClass.ObjectToString(drProject["Progress"]);

            if (strProgress == "录入")
            {
                TXT_ProjectName.ReadOnly = false;
                TXT_ProjectManager.ReadOnly = false;
                btnProjectManager.Disabled = false;
                TXT_StartTime.ReadOnly = false;
                TXT_EndTime.ReadOnly = false;
                TXT_PowerPurchase.Enabled = true;
                TXT_ForCost.ReadOnly = false;
                TXT_SelfCost.ReadOnly = false;
                TXT_BuildUnit.ReadOnly = false;
                TXT_SupervisorUnit.ReadOnly = false;
                TXT_ProjectDesc.Enabled = true;

                TXT_ProjectName.BackColor = Color.CornflowerBlue;
                TXT_ProjectManager.BackColor = Color.CornflowerBlue;
                TXT_StartTime.BackColor = Color.CornflowerBlue;
                TXT_EndTime.BackColor = Color.CornflowerBlue;
                TXT_PowerPurchase.BackColor = Color.CornflowerBlue;
                TXT_ForCost.BackColor = Color.CornflowerBlue;
                TXT_SelfCost.BackColor = Color.CornflowerBlue;
                TXT_BuildUnit.BackColor = Color.CornflowerBlue;
                TXT_SupervisorUnit.BackColor = Color.CornflowerBlue;
                TXT_ProjectDesc.BackColor = Color.CornflowerBlue;
                TXT_RelatedCode.BackColor = Color.CornflowerBlue;
            }
            else if (strProgress == "立项")
            {
                TXT_ProjectName.ReadOnly = true;
                TXT_ProjectManager.ReadOnly = true;
                btnProjectManager.Disabled = true;
                TXT_StartTime.ReadOnly = true;
                TXT_EndTime.ReadOnly = true;
                TXT_PowerPurchase.Enabled = false;
                TXT_ForCost.ReadOnly = true;
                TXT_SelfCost.ReadOnly = true;
                TXT_BuildUnit.ReadOnly = true;
                TXT_SupervisorUnit.ReadOnly = true;
                TXT_ProjectDesc.Enabled = false;
            }
            else
            {
                TXT_ProjectName.ReadOnly = true;
                TXT_ProjectManager.ReadOnly = true;
                TXT_StartTime.ReadOnly = true;
                TXT_EndTime.ReadOnly = true;
                TXT_PowerPurchase.Enabled = false;
                TXT_ForCost.ReadOnly = true;
                TXT_SelfCost.ReadOnly = true;
                TXT_BuildUnit.ReadOnly = true;
                TXT_SupervisorUnit.ReadOnly = true;
                TXT_ProjectDesc.Enabled = false;

            }
        }
    }



    private void BindProjectAttribute()
    {
        string strProjectAttributeHQL = "select * from T_WZProjectAttribute order by AttributeCode ";
        DataTable dtProjectAttribute = ShareClass.GetDataSetFromSql(strProjectAttributeHQL, "strProjectAttributeHQL").Tables[0];

        DDL_ProjectAttribute.DataSource = dtProjectAttribute;
        DDL_ProjectAttribute.DataTextField = "AttributeDesc";
        DDL_ProjectAttribute.DataValueField = "AttributeCode";
        DDL_ProjectAttribute.DataBind();
    }


    private void BindProjectNature()
    {
        string strProjectNatureHQL = "select * from T_WZProjectNature order by NatureCode ";
        DataTable dtProjectNature = ShareClass.GetDataSetFromSql(strProjectNatureHQL, "strProjectNatureHQL").Tables[0];

        DDL_ProjectNature.DataSource = dtProjectNature;
        DDL_ProjectNature.DataTextField = "NatureDesc";
        DDL_ProjectNature.DataValueField = "NatureCode";
        DDL_ProjectNature.DataBind();
    }




    private string CreateNewProjectCode()
    {
        //生成项目编号 〈项目编码〉＝“G2013J0001” 总共长度为10位
        string strNewProjectCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strProjectCodeHQL = string.Format("select COUNT(1) as RowNumber from T_WZProject where MarkTime like '%{0}%'", DateTime.Now.Year.ToString());
                DataTable dtProjectCode = ShareClass.GetDataSetFromSql(strProjectCodeHQL, "strProjectCodeHQL").Tables[0];
                int intProjectCodeNumber = int.Parse(dtProjectCode.Rows[0]["RowNumber"].ToString());
                intProjectCodeNumber = intProjectCodeNumber + 1;
                do
                {
                    StringBuilder sbProjectCode = new StringBuilder();
                    for (int j = 4 - intProjectCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbProjectCode.Append("0");
                    }
                    string strProjectAttribute = DDL_ProjectAttribute.SelectedValue;
                    string strProjectNature = DDL_ProjectNature.SelectedValue;
                    strNewProjectCode = strProjectAttribute + DateTime.Now.Year + strProjectNature + sbProjectCode.ToString() + intProjectCodeNumber.ToString();

                    //验证新的项目编号是滞存在
                    string strCheckNewProjectCodeHQL = "select count(1) as RowNumber from T_WZProject where ProjectCode = '" + strNewProjectCode + "'";
                    DataTable dtCheckNewProjectCode = ShareClass.GetDataSetFromSql(strCheckNewProjectCodeHQL, "strCheckNewProjectCodeHQL").Tables[0];
                    int intCheckNewProjectCode = int.Parse(dtCheckNewProjectCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewProjectCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intProjectCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }

        return strNewProjectCode;
    }
}