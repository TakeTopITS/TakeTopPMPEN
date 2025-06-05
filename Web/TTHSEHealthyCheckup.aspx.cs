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

public partial class TTHSEHealthyCheckup : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","健康体检", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_CheckDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EnterDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_BirthDay.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TB_EnterCode.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadProjectName();

            LoadHSEHealthyCheckupList();
        }
    }

    protected void LoadProjectName()
    {
        string strHQL;
        IList lst;
        //绑定项目名称Where project.ProjectType='Primavera项目' and project.Status<>'CaseClosed' and project.Status<>'Cancel' and project.Status<>'Rejected' and " +
        // "project.Status<>'Deleted' and project.Status<>'Archived' 
        strHQL = "From Project as project Order By project.ProjectID Desc";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DL_ProjectId.DataSource = lst;
        DL_ProjectId.DataBind();
        DL_ProjectId.Items.Insert(0, new ListItem("--Select--", "0"));
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

    protected void LoadHSEHealthyCheckupList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEHealthyCheckup Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or PersionName like '%" + TextBox1.Text.Trim() + "%' or MedicalConclusion like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (ProjectId like '%" + TextBox2.Text.Trim() + "%' or ProjectName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-CheckDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-CheckDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEHealthyCheckup");

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEHealthyCheckup(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_ProjectId.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXMWBXCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        string status = GetProjectStatus(DL_ProjectId.SelectedValue);
        //结案，取消，拒绝，删除，归档
        if (status == "CaseClosed")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJACZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Cancel")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYXCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Rejected")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJJCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Deleted")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYSCCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Archived")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYGDCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }

        HSEHealthyCheckupBLL hSEHealthyCheckupBLL = new HSEHealthyCheckupBLL();
        HSEHealthyCheckup hSEHealthyCheckup = new HSEHealthyCheckup();

        hSEHealthyCheckup.Code = GetHSEHealthyCheckupCode();
        LB_Code.Text = hSEHealthyCheckup.Code.Trim();
        hSEHealthyCheckup.PersionName = TB_PersionName.Text.Trim();
        hSEHealthyCheckup.CheckDate = DateTime.Parse(DLC_CheckDate.Text.Trim());
        hSEHealthyCheckup.EnterCode = TB_EnterCode.Text.Trim();
        hSEHealthyCheckup.BirthDay = DateTime.Parse(DLC_BirthDay.Text.Trim());
        hSEHealthyCheckup.MedicalConclusion = TB_MedicalConclusion.Text.Trim();
        hSEHealthyCheckup.Name = TB_Name.Text.Trim();
        hSEHealthyCheckup.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSEHealthyCheckup.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        hSEHealthyCheckup.ProjectName = GetProjectName(hSEHealthyCheckup.ProjectId.ToString());
        hSEHealthyCheckup.PerType = DL_PerType.SelectedValue.Trim();
        hSEHealthyCheckup.PerAttribute = DL_PerAttribute.SelectedValue.Trim();
        hSEHealthyCheckup.Gender = DL_Gender.SelectedValue.Trim();
        hSEHealthyCheckup.EnterCodeValue = strUserCode.Trim();

        try
        {
            hSEHealthyCheckupBLL.AddHSEHealthyCheckup(hSEHealthyCheckup);

            LoadHSEHealthyCheckupList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增或更新时，检查健康体检名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEHealthyCheckup(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSEHealthyCheckup Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSEHealthyCheckup Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEHealthyCheckup").Tables[0];
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
    /// 新增时，获取表T_HSEHealthyCheckup中最大编号 规则HSEHCUX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEHealthyCheckupCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEHealthyCheckup Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEHealthyCheckup").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEHCU" + pa.ToString();
        }
        else
        {
            flag = "HSEHCU1";
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEHealthyCheckup(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_ProjectId.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGXMWBXCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        string status = GetProjectStatus(DL_ProjectId.SelectedValue);
        //结案，取消，拒绝，删除，归档
        if (status == "CaseClosed")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJACZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Cancel")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYXCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Rejected")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYJJCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Deleted")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYSCCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }
        if (status == "Archived")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGXMYGDCZSBJC") + "')", true);
            DL_ProjectId.Focus();
            return;
        }

        string strHQL = "From HSEHealthyCheckup as hSEHealthyCheckup where hSEHealthyCheckup.Code = '" + LB_Code.Text.Trim() + "'";
        HSEHealthyCheckupBLL hSEHealthyCheckupBLL = new HSEHealthyCheckupBLL();
        IList lst = hSEHealthyCheckupBLL.GetAllHSEHealthyCheckups(strHQL);

        HSEHealthyCheckup hSEHealthyCheckup = (HSEHealthyCheckup)lst[0];

        hSEHealthyCheckup.PersionName = TB_PersionName.Text.Trim();
        hSEHealthyCheckup.CheckDate = DateTime.Parse(DLC_CheckDate.Text.Trim());
        hSEHealthyCheckup.EnterCode = TB_EnterCode.Text.Trim();
        hSEHealthyCheckup.BirthDay = DateTime.Parse(DLC_BirthDay.Text.Trim());
        hSEHealthyCheckup.MedicalConclusion = TB_MedicalConclusion.Text.Trim();
        hSEHealthyCheckup.Name = TB_Name.Text.Trim();
        hSEHealthyCheckup.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSEHealthyCheckup.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        hSEHealthyCheckup.ProjectName = GetProjectName(hSEHealthyCheckup.ProjectId.ToString());
        hSEHealthyCheckup.PerType = DL_PerType.SelectedValue.Trim();
        hSEHealthyCheckup.PerAttribute = DL_PerAttribute.SelectedValue.Trim();
        hSEHealthyCheckup.Gender = DL_Gender.SelectedValue.Trim();

        try
        {
            hSEHealthyCheckupBLL.UpdateHSEHealthyCheckup(hSEHealthyCheckup, hSEHealthyCheckup.Code);

            LoadHSEHealthyCheckupList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_Code.Text.Trim();
        strHQL = "Delete From T_HSEHealthyCheckup Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEHealthyCheckupList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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

                strHQL = "From HSEHealthyCheckup as hSEHealthyCheckup where hSEHealthyCheckup.Code = '" + strCode + "'";
                HSEHealthyCheckupBLL hSEHealthyCheckupBLL = new HSEHealthyCheckupBLL();
                lst = hSEHealthyCheckupBLL.GetAllHSEHealthyCheckups(strHQL);

                HSEHealthyCheckup hSEHealthyCheckup = (HSEHealthyCheckup)lst[0];

                LB_Code.Text = hSEHealthyCheckup.Code.Trim();
                TB_PersionName.Text = hSEHealthyCheckup.PersionName.Trim();
                try
                {
                    DL_ProjectId.SelectedValue = hSEHealthyCheckup.ProjectId.ToString().Trim();
                }
                catch
                {
                }
                DL_Gender.SelectedValue = hSEHealthyCheckup.Gender.Trim();
                TB_EnterCode.Text = hSEHealthyCheckup.EnterCode.Trim();
                DLC_BirthDay.Text = hSEHealthyCheckup.BirthDay.ToString("yyyy-MM-dd");
                TB_MedicalConclusion.Text = hSEHealthyCheckup.MedicalConclusion.Trim();
                TB_Name.Text = hSEHealthyCheckup.Name.Trim();
                DLC_EnterDate.Text = hSEHealthyCheckup.EnterDate.ToString("yyyy-MM-dd");
                DL_PerAttribute.SelectedValue = hSEHealthyCheckup.PerAttribute.Trim();
                DL_PerType.SelectedValue = hSEHealthyCheckup.PerType.Trim();
                DLC_CheckDate.Text = hSEHealthyCheckup.CheckDate.ToString("yyyy-MM-dd");
                //if (hSEHealthyCheckup.EnterCodeValue.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEHealthyCheckup");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEHealthyCheckupList();
    }
}