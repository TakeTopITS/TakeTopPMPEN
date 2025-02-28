using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTHSEEnvirFactorSurvey : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","环境因素调查", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_EnterDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EvaluationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_EnterCode.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadProjectName();

            LoadHSEEnvirFactorSurveyList();
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

        strHQL = "From HSEEnvironmentalFactors as hSEEnvironmentalFactors Order By hSEEnvironmentalFactors.Code Desc";
        HSEEnvironmentalFactorsBLL hSEEnvironmentalFactorsBLL = new HSEEnvironmentalFactorsBLL();
        lst = hSEEnvironmentalFactorsBLL.GetAllHSEEnvironmentalFactorss(strHQL);
        DL_FactorCode.DataSource = lst;
        DL_FactorCode.DataBind();
        DL_FactorCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    /// <summary>
    /// 时态=过期或状态=异常的因素，不能选择
    /// </summary>
    /// <param name="strProjectId"></param>
    /// <returns></returns>
    protected bool GetHSEEnvironmentalFactorStatus(string strFactorsCode)
    {
        string strHQL = "From HSEEnvironmentalFactors as hSEEnvironmentalFactors Where hSEEnvironmentalFactors.Code='" + strFactorsCode.Trim() + "' ";
        HSEEnvironmentalFactorsBLL hSEEnvironmentalFactorsBLL = new HSEEnvironmentalFactorsBLL();
        IList lst = hSEEnvironmentalFactorsBLL.GetAllHSEEnvironmentalFactorss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEEnvironmentalFactors hSEEnvironmentalFactors = (HSEEnvironmentalFactors)lst[0];
            if (hSEEnvironmentalFactors.Status.Trim() == "Abnormal" || hSEEnvironmentalFactors.Tenses.Trim() == LanguageHandle.GetWord("GuoJi").ToString().Trim())
            {
                return true;
            }
            else
                return false;
        }
        else
            return true;
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

    protected void LoadHSEEnvirFactorSurveyList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEEnvirFactorSurvey Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or EnterCode like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvirFactorSurvey");

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
        if (IsHSEEnvirFactorSurvey(string.Empty, TB_Name.Text.Trim()))
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
        string strAttachName = "";
        string strAttach = UploadAttach(ref strAttachName);

        HSEEnvirFactorSurveyBLL hSEEnvirFactorSurveyBLL = new HSEEnvirFactorSurveyBLL();
        HSEEnvirFactorSurvey hSEEnvirFactorSurvey = new HSEEnvirFactorSurvey();
        if (strAttach.Equals("0"))
        {
            hSEEnvirFactorSurvey.AttachName = "";
            hSEEnvirFactorSurvey.AttachPath = "";
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
        {
            hSEEnvirFactorSurvey.AttachPath = strAttach;
            hSEEnvirFactorSurvey.AttachName = strAttachName;
        }
        hSEEnvirFactorSurvey.Code = GetHSEEnvirFactorSurveyCode();
        LB_Code.Text = hSEEnvirFactorSurvey.Code.Trim();
        hSEEnvirFactorSurvey.EnterCode = TB_EnterCode.Text.Trim();
        hSEEnvirFactorSurvey.EvaluationOpinions = TB_EvaluationOpinions.Text.Trim();
        hSEEnvirFactorSurvey.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        hSEEnvirFactorSurvey.EvaluationPer = TB_EvaluationPer.Text.Trim();
        hSEEnvirFactorSurvey.Leader = TB_Leader.Text.Trim();
        hSEEnvirFactorSurvey.Remark = TB_Remark.Text.Trim();
        hSEEnvirFactorSurvey.Name = TB_Name.Text.Trim();
        hSEEnvirFactorSurvey.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSEEnvirFactorSurvey.EvaluationDate = DateTime.Parse(DLC_EvaluationDate.Text.Trim());
        hSEEnvirFactorSurvey.ProjectName = GetProjectName(hSEEnvirFactorSurvey.ProjectId.ToString());
        hSEEnvirFactorSurvey.UnitCode = TB_UnitCode.Text.Trim();
        hSEEnvirFactorSurvey.Status = DL_Status.SelectedValue.Trim();
        hSEEnvirFactorSurvey.EnterCodeValue = strUserCode.Trim();

        try
        {
            hSEEnvirFactorSurveyBLL.AddHSEEnvirFactorSurvey(hSEEnvirFactorSurvey);

            LoadHSEEnvirFactorSurDetail(hSEEnvirFactorSurvey.Code.Trim());

            LoadHSEEnvirFactorSurveyList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;
            //BT_AddDetail.Visible = true;
            //BT_DeleteDetail.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增或更新时，检查环境因素调查名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEEnvirFactorSurvey(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSEEnvirFactorSurvey Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSEEnvirFactorSurvey Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvirFactorSurvey").Tables[0];
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
    /// 新增时，获取表T_HSEEnvirFactorSurvey中最大编号 规则HSEEFSX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEEnvirFactorSurveyCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEEnvirFactorSurvey Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvirFactorSurvey").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEEFS" + pa.ToString();
        }
        else
        {
            flag = "HSEEFS1";
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
        if (IsHSEEnvirFactorSurvey(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
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

        string strAttachName = "";
        string strAttach = UploadAttach(ref strAttachName);

        string strHQL = "From HSEEnvirFactorSurvey as hSEEnvirFactorSurvey where hSEEnvirFactorSurvey.Code = '" + LB_Code.Text.Trim() + "'";
        HSEEnvirFactorSurveyBLL hSEEnvirFactorSurveyBLL = new HSEEnvirFactorSurveyBLL();
        IList lst = hSEEnvirFactorSurveyBLL.GetAllHSEEnvirFactorSurveys(strHQL);

        HSEEnvirFactorSurvey hSEEnvirFactorSurvey = (HSEEnvirFactorSurvey)lst[0];

        if (strAttach.Equals("0"))
        {
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
        {
            hSEEnvirFactorSurvey.AttachPath = strAttach;
            hSEEnvirFactorSurvey.AttachName = strAttachName;
        }
        hSEEnvirFactorSurvey.EnterCode = TB_EnterCode.Text.Trim();
        hSEEnvirFactorSurvey.EvaluationOpinions = TB_EvaluationOpinions.Text.Trim();
        hSEEnvirFactorSurvey.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        hSEEnvirFactorSurvey.EvaluationPer = TB_EvaluationPer.Text.Trim();
        hSEEnvirFactorSurvey.Leader = TB_Leader.Text.Trim();
        hSEEnvirFactorSurvey.Remark = TB_Remark.Text.Trim();
        hSEEnvirFactorSurvey.Name = TB_Name.Text.Trim();
        hSEEnvirFactorSurvey.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSEEnvirFactorSurvey.EvaluationDate = DateTime.Parse(DLC_EvaluationDate.Text.Trim());
        hSEEnvirFactorSurvey.ProjectName = GetProjectName(hSEEnvirFactorSurvey.ProjectId.ToString());
        hSEEnvirFactorSurvey.UnitCode = TB_UnitCode.Text.Trim();
        hSEEnvirFactorSurvey.Status = DL_Status.SelectedValue.Trim();

        try
        {
            hSEEnvirFactorSurveyBLL.UpdateHSEEnvirFactorSurvey(hSEEnvirFactorSurvey, hSEEnvirFactorSurvey.Code);

            LoadHSEEnvirFactorSurDetail(hSEEnvirFactorSurvey.Code.Trim());

            LoadHSEEnvirFactorSurveyList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;
            //BT_AddDetail.Visible = true;
            //BT_DeleteDetail.Visible = false;

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
        strHQL = "Delete From T_HSEEnvirFactorSurvey Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEEnvirFactorSurDetail("");

            LoadHSEEnvirFactorSurveyList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;
            //BT_AddDetail.Visible = false;
            //BT_DeleteDetail.Visible = false;

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

                strHQL = "From HSEEnvirFactorSurvey as hSEEnvirFactorSurvey where hSEEnvirFactorSurvey.Code = '" + strCode + "'";
                HSEEnvirFactorSurveyBLL hSEEnvirFactorSurveyBLL = new HSEEnvirFactorSurveyBLL();
                lst = hSEEnvirFactorSurveyBLL.GetAllHSEEnvirFactorSurveys(strHQL);

                HSEEnvirFactorSurvey hSEEnvirFactorSurvey = (HSEEnvirFactorSurvey)lst[0];

                LB_Code.Text = hSEEnvirFactorSurvey.Code.Trim();
                TB_EnterCode.Text = hSEEnvirFactorSurvey.EnterCode.Trim();
                try
                {
                    DL_ProjectId.SelectedValue = hSEEnvirFactorSurvey.ProjectId.ToString().Trim();
                }
                catch
                {
                }
                TB_EvaluationOpinions.Text = hSEEnvirFactorSurvey.EvaluationOpinions.Trim();
                TB_EvaluationPer.Text = hSEEnvirFactorSurvey.EvaluationPer.Trim();
                TB_Leader.Text = hSEEnvirFactorSurvey.Leader.Trim();
                TB_Remark.Text = hSEEnvirFactorSurvey.Remark.Trim();
                TB_Name.Text = hSEEnvirFactorSurvey.Name.Trim();
                DLC_EvaluationDate.Text = hSEEnvirFactorSurvey.EvaluationDate.ToString("yyyy-MM-dd");
                TB_UnitCode.Text = hSEEnvirFactorSurvey.UnitCode.Trim();
                DL_Status.SelectedValue = hSEEnvirFactorSurvey.Status.Trim();
                DLC_EnterDate.Text = hSEEnvirFactorSurvey.EnterDate.ToString("yyyy-MM-dd");
                LoadHSEEnvirFactorSurDetail(hSEEnvirFactorSurvey.Code.Trim());
                //if (hSEEnvirFactorSurvey.EnterCodeValue.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Visible = true;
                //    BT_Delete.Enabled = true;
                //    BT_AddDetail.Visible = true;
                //    BT_DeleteDetail.Visible = false;
                //}
                //else
                //{
                //    BT_AddDetail.Visible = true;
                //    BT_DeleteDetail.Visible = false;

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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvirFactorSurvey");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEEnvirFactorSurveyList();
    }

    protected void LoadHSEEnvirFactorSurDetail(string strEnvirFactorSurveyCode)
    {
        string strHQL = "From HSEEnvirFactorSurDetail as hSEEnvirFactorSurDetail where hSEEnvirFactorSurDetail.EnvirFactorSurveyCode = '" + strEnvirFactorSurveyCode + "'";
        HSEEnvirFactorSurDetailBLL hSEEnvirFactorSurDetailBLL = new HSEEnvirFactorSurDetailBLL();
        IList lst = hSEEnvirFactorSurDetailBLL.GetAllHSEEnvirFactorSurDetails(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[2].Text.Trim();
            lbl_ID.Text = strID;

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From HSEEnvirFactorSurDetail as hSEEnvirFactorSurDetail where hSEEnvirFactorSurDetail.ID = '" + strID + "'";
                HSEEnvirFactorSurDetailBLL hSEEnvirFactorSurDetailBLL = new HSEEnvirFactorSurDetailBLL();
                lst = hSEEnvirFactorSurDetailBLL.GetAllHSEEnvirFactorSurDetails(strHQL);

                HSEEnvirFactorSurDetail hSEEnvirFactorSurDetail = (HSEEnvirFactorSurDetail)lst[0];

                lbl_ID.Text = hSEEnvirFactorSurDetail.ID.ToString();
                DL_FactorCode.SelectedValue = hSEEnvirFactorSurDetail.FactorCode.Trim();
                TB_EvaluationResult.Text = hSEEnvirFactorSurDetail.EvaluationResult.Trim();
                DL_SignificantDegree.SelectedValue = hSEEnvirFactorSurDetail.SignificantDegree.Trim();
                //if (hSEEnvirFactorSurDetail.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_AddDetail.Visible = true;
                //    BT_DeleteDetail.Visible = true;
                //    BT_DeleteDetail.Enabled = true;
                //}
                //else
                //{
                //    BT_AddDetail.Visible = true;
                //    BT_DeleteDetail.Visible = false;
                //}

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }


            if (e.CommandName == "Delete")
            {
                DeleteDetail();
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            }
        }
    }

    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        lbl_ID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strID;

        strID = lbl_ID.Text;

        if (strID == "")
        {
            AddDetail();
        }
        else
        {
            UpdateDetail();
        }
    }

    protected void AddDetail()
    {
        if (DL_FactorCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_FactorCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoHuanJingYinSuWeiBiShu").ToString().Trim()+"')", true); 
            DL_FactorCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            return;
        }
        if (IsHSEEnvirFactorSurDetail(string.Empty, DL_FactorCode.SelectedValue.Trim(), LB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoHuanJingYinSuYiCunZai").ToString().Trim()+"')", true); 
            DL_FactorCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            return;
        }
        if (GetHSEEnvironmentalFactorStatus(DL_FactorCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoCiHuanJingYinSuGuoJiH").ToString().Trim()+"')", true); 
            DL_FactorCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            return;
        }

        HSEEnvirFactorSurDetailBLL hSEEnvirFactorSurDetailBLL = new HSEEnvirFactorSurDetailBLL();
        HSEEnvirFactorSurDetail hSEEnvirFactorSurDetail = new HSEEnvirFactorSurDetail();
        hSEEnvirFactorSurDetail.EnvirFactorSurveyCode = LB_Code.Text.Trim();
        hSEEnvirFactorSurDetail.EnvirFactorSurveyName = TB_Name.Text.Trim();
        hSEEnvirFactorSurDetail.EvaluationResult = TB_EvaluationResult.Text.Trim();
        hSEEnvirFactorSurDetail.FactorCode = DL_FactorCode.SelectedValue.Trim();
        hSEEnvirFactorSurDetail.FactorName = GetHSEEnvironmentalFactorsName(hSEEnvirFactorSurDetail.FactorCode.Trim());
        hSEEnvirFactorSurDetail.SignificantDegree = DL_SignificantDegree.SelectedValue.Trim();
        hSEEnvirFactorSurDetail.EnterCode = strUserCode.Trim();

        try
        {
            hSEEnvirFactorSurDetailBLL.AddHSEEnvirFactorSurDetail(hSEEnvirFactorSurDetail);

            lbl_ID.Text = GetHSEEnvirFactorSurDetailID(hSEEnvirFactorSurDetail).ToString();

            LoadHSEEnvirFactorSurDetail(hSEEnvirFactorSurDetail.EnvirFactorSurveyCode.Trim());

            //BT_AddDetail.Visible = true;
            //BT_DeleteDetail.Visible = true;
            //BT_DeleteDetail.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }


    }

    protected void UpdateDetail()
    {
        if (DL_FactorCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_FactorCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoHuanJingYinSuWeiBiShu").ToString().Trim()+"')", true); 
            DL_FactorCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            return;
        }
        if (IsHSEEnvirFactorSurDetail(string.Empty, DL_FactorCode.SelectedValue.Trim(), LB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoHuanJingYinSuYiCunZai").ToString().Trim()+"')", true); 
            DL_FactorCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            return;
        }
        if (GetHSEEnvironmentalFactorStatus(DL_FactorCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoCiHuanJingYinSuGuoJiH").ToString().Trim()+"')", true); 
            DL_FactorCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            return;
        }
        string strHQL;
        IList lst;

        string strID;
        strID = lbl_ID.Text;
        

        strHQL = "From HSEEnvirFactorSurDetail as hSEEnvirFactorSurDetail where hSEEnvirFactorSurDetail.ID = '" + strID + "'";
        HSEEnvirFactorSurDetailBLL hSEEnvirFactorSurDetailBLL = new HSEEnvirFactorSurDetailBLL();
        lst = hSEEnvirFactorSurDetailBLL.GetAllHSEEnvirFactorSurDetails(strHQL);

        HSEEnvirFactorSurDetail hSEEnvirFactorSurDetail = (HSEEnvirFactorSurDetail)lst[0];

        hSEEnvirFactorSurDetail.EnvirFactorSurveyCode = LB_Code.Text.Trim();
        hSEEnvirFactorSurDetail.EnvirFactorSurveyName = TB_Name.Text.Trim();
        hSEEnvirFactorSurDetail.EvaluationResult = TB_EvaluationResult.Text.Trim();
        hSEEnvirFactorSurDetail.FactorCode = DL_FactorCode.SelectedValue.Trim();
        hSEEnvirFactorSurDetail.FactorName = GetHSEEnvironmentalFactorsName(hSEEnvirFactorSurDetail.FactorCode.Trim());
        hSEEnvirFactorSurDetail.SignificantDegree = DL_SignificantDegree.SelectedValue.Trim();
        hSEEnvirFactorSurDetail.EnterCode = strUserCode.Trim();

        try
        {
            hSEEnvirFactorSurDetailBLL.UpdateHSEEnvirFactorSurDetail(hSEEnvirFactorSurDetail, int.Parse(strID));

            lbl_ID.Text = GetHSEEnvirFactorSurDetailID(hSEEnvirFactorSurDetail).ToString();

            LoadHSEEnvirFactorSurDetail(hSEEnvirFactorSurDetail.EnvirFactorSurveyCode.Trim());

            //BT_AddDetail.Visible = true;
            //BT_DeleteDetail.Visible = true;
            //BT_DeleteDetail.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

        }


    }

    protected void DeleteDetail()
    {
        string strHQL;
        string strId = lbl_ID.Text.Trim();
        strHQL = "Delete From T_HSEEnvirFactorSurDetail Where ID = '" + strId + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEEnvirFactorSurDetail(LB_Code.Text.Trim());

            //BT_AddDetail.Visible = true;
            //BT_DeleteDetail.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    /// <summary>
    /// 新增时，获取表T_HSEEnvirFactorSurDetail中最大编号
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetHSEEnvirFactorSurDetailID(HSEEnvirFactorSurDetail hDetail)
    {
        string strHQL = "Select ID From T_HSEEnvirFactorSurDetail Where EnvirFactorSurveyCode='" + hDetail.EnvirFactorSurveyCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvirFactorSurDetail").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 新增时，检查环境因素调查明细是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEEnvirFactorSurDetail(string strId, string strFactorCode, string strEnvirFactorSurveyCode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strId))
        {
            strHQL = "Select ID From T_HSEEnvirFactorSurDetail Where FactorCode='" + strFactorCode + "' and EnvirFactorSurveyCode='" + strEnvirFactorSurveyCode + "' ";
        }
        else
            strHQL = "Select ID From T_HSEEnvirFactorSurDetail Where FactorCode='" + strFactorCode + "' and EnvirFactorSurveyCode='" + strEnvirFactorSurveyCode + "' and ID<>'" + strId + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvirFactorSurDetail").Tables[0];
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

    protected string GetHSEEnvironmentalFactorsName(string strCode)
    {
        string strHQL = "From HSEEnvironmentalFactors as hSEEnvironmentalFactors Where hSEEnvironmentalFactors.Code='" + strCode + "' ";
        HSEEnvironmentalFactorsBLL hSEEnvironmentalFactorsBLL = new HSEEnvironmentalFactorsBLL();
        IList lst = hSEEnvironmentalFactorsBLL.GetAllHSEEnvironmentalFactorss(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEEnvironmentalFactors hSEEnvironmentalFactors = (HSEEnvironmentalFactors)lst[0];
            return hSEEnvironmentalFactors.Name.Trim();
        }
        else
            return "";
    }

    protected string UploadAttach(ref string strAttachName)
    {
        //上传附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                return "1";
            }
            else
            {
                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                    strAttachName = Path.GetFileNameWithoutExtension(strFileName2);
                    return "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                }
                catch
                {
                    return "2";
                }
            }
        }
        else
        {
            return "0";
        }
    }
}
