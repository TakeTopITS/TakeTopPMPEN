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
using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Windows.Forms;

public partial class TTHSESafeManagementPlan : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","安全管理方案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadProjectName();

            LoadHSESafeManagementPlanList();
        }
    }

    protected void LoadProjectName()
    {
        string strHQL;
        IList lst;
        //绑定项目名称Where project.ProjectType='Primavera项目' and project.Status<>'CaseClosed' and project.Status<>'Cancel' and project.Status<>'拒绝' and " +
        // "project.Status<>'Deleted' and project.Status<>'Archived' 
        strHQL = "From Project as project Order By project.ProjectID Desc";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DL_ProjectId.DataSource = lst;
        DL_ProjectId.DataBind();
        DL_ProjectId.Items.Insert(0, new ListItem("--Select--", "0"));

        strHQL = "from HSEPerEquipRecord as hSEPerEquipRecord Where hSEPerEquipRecord.AuditStatus='Qualified' Order by hSEPerEquipRecord.Code Desc ";
        HSEPerEquipRecordBLL hSEPerEquipRecordBLL = new HSEPerEquipRecordBLL();
        lst = hSEPerEquipRecordBLL.GetAllHSEPerEquipRecords(strHQL);
        DL_PerEquipRecordCode.DataSource = lst;
        DL_PerEquipRecordCode.DataBind();
        DL_PerEquipRecordCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    /// <summary>
    /// 结案，取消，拒绝，删除，归档
    /// </summary>
    /// <param name="strProjectId"></param>
    /// <returns></returns>
    protected string GetProjectStatus(string strProjectId)
    {
        string strHQL = "From Project as project Where project.ProjectID='" + strProjectId.Trim() + "' ";
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            Project project = (Project)lst[0];
            return project.Status.Trim();
        }
        else
            return "";
    }

    protected void LoadHSESafeManagementPlanList()
    {
        string strHQL;

        strHQL = "Select * From T_HSESafeManagementPlan Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or PerEquipRecordName like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (ProjectId like '%" + TextBox2.Text.Trim() + "%' or ProjectName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeManagementPlan");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_Code.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strCode;

        strCode = LB_Code.Text;

        if (strCode == "")
        {
            Add();
        }
        else
        {
            Update();
        }
    }

    protected void Add()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSESafeManagementPlan(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_ProjectId.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXMWBXCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        string status = GetProjectStatus(DL_ProjectId.SelectedValue);
        //结案，取消，拒绝，删除，归档
        if (status == "CaseClosed")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJACZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Cancel")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYXCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Rejected")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJJCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Deleted")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYSCCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Archived")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYGDCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_PerEquipRecordCode.SelectedValue.Trim()) || DL_PerEquipRecordCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWFBSWBXJC").ToString().Trim() + "')", true);
            DL_PerEquipRecordCode.Focus();
            return;
        }

        HSESafeManagementPlanBLL hSESafeManagementPlanBLL = new HSESafeManagementPlanBLL();
        HSESafeManagementPlan hSESafeManagementPlan = new HSESafeManagementPlan();

        hSESafeManagementPlan.Code = GetHSESafeManagementPlanCode();
        LB_Code.Text = hSESafeManagementPlan.Code.Trim();
        hSESafeManagementPlan.Leader = TB_Leader.Text.Trim();
        hSESafeManagementPlan.Remark = TB_Remark.Text.Trim();
        hSESafeManagementPlan.Name = TB_Name.Text.Trim();
        hSESafeManagementPlan.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSESafeManagementPlan.ProjectName = GetProjectName(hSESafeManagementPlan.ProjectId.ToString());
        hSESafeManagementPlan.PerEquipRecordCode = DL_PerEquipRecordCode.SelectedValue.Trim();
        hSESafeManagementPlan.PerEquipRecordName = GetHSEPerEquipRecordName(hSESafeManagementPlan.PerEquipRecordCode.Trim());
        hSESafeManagementPlan.EnterCode = strUserCode.Trim();

        try
        {
            hSESafeManagementPlanBLL.AddHSESafeManagementPlan(hSESafeManagementPlan);

            LoadHSESafeManagementPlanList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 取得人员/设备备案的名称。
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetHSEPerEquipRecordName(string strCode)
    {
        string strHQL = "Select * From T_HSEPerEquipRecord Where Code='" + strCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPerEquipRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["Name"].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 新增或更新时，检查安检名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSESafeManagementPlan(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSESafeManagementPlan Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSESafeManagementPlan Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeManagementPlan").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    /// <summary>
    /// 新增时，获取表T_HSESafeManagementPlan中最大编号 规则HSESMPX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSESafeManagementPlanCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSESafeManagementPlan Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeManagementPlan").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSESMP" + pa.ToString();
        }
        else
        {
            flag = "HSESMP1";
        }
        return flag;
    }

    protected string GetProjectName(string strProjId)
    {
        string strHQL;
        IList lst;
        //绑定项目名称
        strHQL = "From Project as project Where project.ProjectID='" + strProjId + "' ";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            Project proj = (Project)lst[0];
            return proj.ProjectName.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSESafeManagementPlan(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_ProjectId.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXMWBXCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        string status = GetProjectStatus(DL_ProjectId.SelectedValue);
        //结案，取消，拒绝，删除，归档
        if (status == "CaseClosed")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJACZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Cancel")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYXCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Rejected")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJJCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Deleted")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYSCCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Archived")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYGDCZSBJC").ToString().Trim() + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_PerEquipRecordCode.SelectedValue.Trim()) || DL_PerEquipRecordCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWFBSWBXJC").ToString().Trim() + "')", true);
            DL_PerEquipRecordCode.Focus();
            return;
        }

        string strHQL = "From HSESafeManagementPlan as hSESafeManagementPlan where hSESafeManagementPlan.Code = '" + LB_Code.Text.Trim() + "'";
        HSESafeManagementPlanBLL hSESafeManagementPlanBLL = new HSESafeManagementPlanBLL();
        IList lst = hSESafeManagementPlanBLL.GetAllHSESafeManagementPlans(strHQL);

        HSESafeManagementPlan hSESafeManagementPlan = (HSESafeManagementPlan)lst[0];

        hSESafeManagementPlan.Leader = TB_Leader.Text.Trim();
        hSESafeManagementPlan.Remark = TB_Remark.Text.Trim();
        hSESafeManagementPlan.Name = TB_Name.Text.Trim();
        hSESafeManagementPlan.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSESafeManagementPlan.ProjectName = GetProjectName(hSESafeManagementPlan.ProjectId.ToString());
        hSESafeManagementPlan.PerEquipRecordCode = DL_PerEquipRecordCode.SelectedValue.Trim();
        hSESafeManagementPlan.PerEquipRecordName = GetHSEPerEquipRecordName(hSESafeManagementPlan.PerEquipRecordCode.Trim());

        try
        {
            hSESafeManagementPlanBLL.UpdateHSESafeManagementPlan(hSESafeManagementPlan, hSESafeManagementPlan.Code);

            LoadHSESafeManagementPlanList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_Code.Text.Trim();
        strHQL = "Delete From T_HSESafeManagementPlan Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSESafeManagementPlanList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strCode, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strCode = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "From HSESafeManagementPlan as hSESafeManagementPlan where hSESafeManagementPlan.Code = '" + strCode + "'";
                HSESafeManagementPlanBLL hSESafeManagementPlanBLL = new HSESafeManagementPlanBLL();
                lst = hSESafeManagementPlanBLL.GetAllHSESafeManagementPlans(strHQL);

                HSESafeManagementPlan hSESafeManagementPlan = (HSESafeManagementPlan)lst[0];

                LB_Code.Text = hSESafeManagementPlan.Code.Trim();
                TB_Leader.Text = hSESafeManagementPlan.Leader.Trim();
                try
                {
                    DL_ProjectId.SelectedValue = hSESafeManagementPlan.ProjectId.ToString().Trim();
                }
                catch
                {
                }
                TB_Remark.Text = hSESafeManagementPlan.Remark.Trim();
                TB_Name.Text = hSESafeManagementPlan.Name.Trim();
                DL_PerEquipRecordCode.SelectedValue = hSESafeManagementPlan.PerEquipRecordCode.Trim();
                //if (hSESafeManagementPlan.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Visible = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Update.Visible = false;
                //    BT_Delete.Visible = false;
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                Delete();

            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeManagementPlan");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSESafeManagementPlanList();
    }
}