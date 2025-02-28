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

public partial class TTHSEAccidentDescription : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","事故描述", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_AccidentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadProjectName();

            LoadHSEAccidentDescriptionList();
        }
    }

    protected void LoadProjectName()
    {
        string strHQL;
        IList lst;
        //绑定项目名称Where project.ProjectType='Primavera项目' and project.Status<>'CaseClosed' and project.Status<>'Cancel' and project.Status<>'Rejected' and " +
        //   "project.Status<>'Deleted' and project.Status<>'Archived' 
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

    protected void LoadHSEAccidentDescriptionList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEAccidentDescription Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or AccidentAfter like '%" + TextBox1.Text.Trim() + "%' " +
                "or AccidentScope like '%" + TextBox1.Text.Trim() + "%' or AccidentBecause like '%" + TextBox1.Text.Trim() + "%' or Measures like '%" + TextBox1.Text.Trim() + "%' " +
                "or Others like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (ProjectId like '%" + TextBox2.Text.Trim() + "%' or ProjectName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-AccidentDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-AccidentDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentDescription");

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
        if (IsHSEAccidentDescription(string.Empty, TB_Name.Text.Trim()))
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

        HSEAccidentDescription hSEAccidentDescription = new HSEAccidentDescription();
        HSEAccidentDescriptionBLL hSEAccidentDescriptionBLL = new HSEAccidentDescriptionBLL();

        hSEAccidentDescription.Code = GetHSEAccidentDescriptionCode();
        LB_Code.Text = hSEAccidentDescription.Code.Trim();
        hSEAccidentDescription.AccidentDate = DateTime.Parse(DLC_AccidentDate.Text.Trim());
        hSEAccidentDescription.Name = TB_Name.Text.Trim();
        hSEAccidentDescription.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSEAccidentDescription.ProjectName = GetProjectName(hSEAccidentDescription.ProjectId.ToString());
        hSEAccidentDescription.Others = TB_Others.Text.Trim();
        hSEAccidentDescription.Status = DL_Status.SelectedValue.Trim();
        hSEAccidentDescription.AccidentAddr = TB_AccidentAddr.Text.Trim();
        hSEAccidentDescription.AccidentAfter = TB_AccidentAfter.Text.Trim();
        hSEAccidentDescription.AccidentBecause = TB_AccidentBecause.Text.Trim();
        hSEAccidentDescription.AccidentScope = TB_AccidentScope.Text.Trim();
        hSEAccidentDescription.AccidentType = TB_AccidentType.Text.Trim();
        hSEAccidentDescription.DeathNum = int.Parse(NB_DeathNum.Amount.ToString());
        hSEAccidentDescription.DepartCode = TB_DepartCode.Text.Trim();
        hSEAccidentDescription.Measures = TB_Measures.Text.Trim();
        hSEAccidentDescription.MinorInjury = int.Parse(NB_MinorInjury.Amount.ToString());
        hSEAccidentDescription.SceneLeader = TB_SceneLeader.Text.Trim();
        hSEAccidentDescription.SeriousInjury = int.Parse(NB_SeriousInjury.Amount.ToString());
        hSEAccidentDescription.EnterCode = strUserCode.Trim();

        try
        {
            hSEAccidentDescriptionBLL.AddHSEAccidentDescription(hSEAccidentDescription);

            LoadHSEAccidentDescriptionList();

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
    /// 新增或更新时，检查事故描述名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEAccidentDescription(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSEAccidentDescription Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSEAccidentDescription Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentDescription").Tables[0];
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

    /// <summary>
    /// 新增时，获取表T_HSEAccidentDescription中最大编号 规则HSEADDX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEAccidentDescriptionCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEAccidentDescription Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentDescription").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEADD" + pa.ToString();
        }
        else
        {
            flag = "HSEADD1";
        }
        return flag;
    }

    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEAccidentDescription(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
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

        string strHQL = "From HSEAccidentDescription as hSEAccidentDescription Where hSEAccidentDescription.Code='" + LB_Code.Text.Trim() + "' ";
        HSEAccidentDescriptionBLL hSEAccidentDescriptionBLL = new HSEAccidentDescriptionBLL();
        IList lst = hSEAccidentDescriptionBLL.GetAllHSEAccidentDescriptions(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEAccidentDescription hSEAccidentDescription = (HSEAccidentDescription)lst[0];
            hSEAccidentDescription.AccidentDate = DateTime.Parse(DLC_AccidentDate.Text.Trim());
            hSEAccidentDescription.Name = TB_Name.Text.Trim();
            hSEAccidentDescription.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
            hSEAccidentDescription.ProjectName = GetProjectName(hSEAccidentDescription.ProjectId.ToString());
            hSEAccidentDescription.Others = TB_Others.Text.Trim();
            hSEAccidentDescription.Status = DL_Status.SelectedValue.Trim();
            hSEAccidentDescription.AccidentAddr = TB_AccidentAddr.Text.Trim();
            hSEAccidentDescription.AccidentAfter = TB_AccidentAfter.Text.Trim();
            hSEAccidentDescription.AccidentBecause = TB_AccidentBecause.Text.Trim();
            hSEAccidentDescription.AccidentScope = TB_AccidentScope.Text.Trim();
            hSEAccidentDescription.AccidentType = TB_AccidentType.Text.Trim();
            hSEAccidentDescription.DeathNum = int.Parse(NB_DeathNum.Amount.ToString());
            hSEAccidentDescription.DepartCode = TB_DepartCode.Text.Trim();
            hSEAccidentDescription.Measures = TB_Measures.Text.Trim();
            hSEAccidentDescription.MinorInjury = int.Parse(NB_MinorInjury.Amount.ToString());
            hSEAccidentDescription.SceneLeader = TB_SceneLeader.Text.Trim();
            hSEAccidentDescription.SeriousInjury = int.Parse(NB_SeriousInjury.Amount.ToString());
            try
            {
                hSEAccidentDescriptionBLL.UpdateHSEAccidentDescription(hSEAccidentDescription, hSEAccidentDescription.Code);

                LoadHSEAccidentDescriptionList();

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
    }

    protected void Delete()
    {
        string strCode = LB_Code.Text.Trim();
        if (IsHSEAccidentInvestigation(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSGDCBGDZYDYGSGMSDWFSC").ToString().Trim() + "')", true);
            return;
        }
        string strHQL = "Delete From T_HSEAccidentDescription Where Code = '" + strCode + "' ";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEAccidentDescriptionList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    /// <summary>
    /// 删除时，检查事故调查报告是否存在该事故描述单，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEAccidentInvestigation(string strCode)
    {
        bool flag = true;
        string strHQL = "Select Code From T_HSEAccidentInvestigation Where AccidentDescriptionCode='" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentInvestigation").Tables[0];
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

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEAccidentDescriptionList();
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


                strHQL = "From HSEAccidentDescription as hSEAccidentDescription where hSEAccidentDescription.Code = '" + strCode + "'";
                HSEAccidentDescriptionBLL hSEAccidentDescriptionBLL = new HSEAccidentDescriptionBLL();
                lst = hSEAccidentDescriptionBLL.GetAllHSEAccidentDescriptions(strHQL);
                HSEAccidentDescription hSEAccidentDescription = (HSEAccidentDescription)lst[0];

                LB_Code.Text = hSEAccidentDescription.Code.Trim();
                try
                {
                    DL_ProjectId.SelectedValue = hSEAccidentDescription.ProjectId.ToString().Trim();
                }
                catch
                {
                }
                TB_Name.Text = hSEAccidentDescription.Name.Trim();
                DL_Status.SelectedValue = hSEAccidentDescription.Status.Trim();
                TB_Others.Text = hSEAccidentDescription.Others.Trim();
                TB_SceneLeader.Text = hSEAccidentDescription.SceneLeader.Trim();
                TB_Measures.Text = hSEAccidentDescription.Measures.Trim();
                TB_DepartCode.Text = hSEAccidentDescription.DepartCode.Trim();
                TB_AccidentType.Text = hSEAccidentDescription.AccidentType.Trim();
                TB_AccidentScope.Text = hSEAccidentDescription.AccidentScope.Trim();
                TB_AccidentBecause.Text = hSEAccidentDescription.AccidentBecause.Trim();
                TB_AccidentAfter.Text = hSEAccidentDescription.AccidentAfter.Trim();
                TB_AccidentAddr.Text = hSEAccidentDescription.AccidentAddr.Trim();
                NB_SeriousInjury.Amount = hSEAccidentDescription.SeriousInjury;
                NB_MinorInjury.Amount = hSEAccidentDescription.MinorInjury;
                NB_DeathNum.Amount = hSEAccidentDescription.DeathNum;
                DLC_AccidentDate.Text = hSEAccidentDescription.AccidentDate.ToString("yyyy-MM-dd");
                //if (hSEAccidentDescription.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentDescription");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}