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

public partial class TTHSETraining : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","HSE培训", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_TrainingEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_TrainingStart.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TB_Hoster.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadProjectName();

            LoadHSETrainingList();
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

    protected void LoadHSETrainingList()
    {
        string strHQL;

        strHQL = "Select * From T_HSETraining Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or Speaker like '%" + TextBox1.Text.Trim() + "%' or TrainingContent like '%" + TextBox1.Text.Trim() + "%' " +
                "or TrainingType like '%" + TextBox1.Text.Trim() + "%' or TrainingSite like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (ProjectId like '%" + TextBox2.Text.Trim() + "%' or ProjectName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-TrainingEnd::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-TrainingStart::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSETraining");

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
        if (IsHSETraining(string.Empty, TB_Name.Text.Trim()))
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

        HSETrainingBLL hSETrainingBLL = new HSETrainingBLL();
        HSETraining hSETraining = new HSETraining();

        hSETraining.Code = GetHSETrainingCode();
        LB_Code.Text = hSETraining.Code.Trim();
        hSETraining.Speaker = TB_Speaker.Text.Trim();
        hSETraining.TrainingEnd = DateTime.Parse(DLC_TrainingEnd.Text.Trim() + " " + DL_EndHour.SelectedValue.Trim() + ":" + DL_EndMinute.SelectedValue.Trim() + ":00");
        hSETraining.Hoster = TB_Hoster.Text.Trim();
        hSETraining.TrainingContent = TB_TrainingContent.Text.Trim();
        hSETraining.Name = TB_Name.Text.Trim();
        hSETraining.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSETraining.TrainingStart = DateTime.Parse(DLC_TrainingStart.Text.Trim() + " " + DL_StartHour.SelectedValue.Trim() + ":" + DL_StartMinute.SelectedValue.Trim() + ":00");
        hSETraining.ProjectName = GetProjectName(hSETraining.ProjectId.ToString());
        hSETraining.TrainingType = DL_TrainingType.SelectedValue.Trim();
        hSETraining.TrainingSite = TB_TrainingSite.Text.Trim();
        if (hSETraining.TrainingStart > hSETraining.TrainingEnd)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGPKSSJBNDYPJSSJCZSBJC").ToString().Trim() + "')", true);
            DLC_TrainingStart.Focus();
            return;
        }
        hSETraining.EnterCode = strUserCode.Trim();

        try
        {
            hSETrainingBLL.AddHSETraining(hSETraining);

            LoadHSETrainingList();

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
    /// 新增或更新时，检查HSE培训计划名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSETraining(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSETraining Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSETraining Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSETraining").Tables[0];
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
    /// 新增时，获取表T_HSETraining中最大编号 规则HSETRNX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSETrainingCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSETraining Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSETraining").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSETRN" + pa.ToString();
        }
        else
        {
            flag = "HSETRN1";
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
        if (IsHSETraining(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
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

        string strHQL = "From HSETraining as hSETraining where hSETraining.Code = '" + LB_Code.Text.Trim() + "'";
        HSETrainingBLL hSETrainingBLL = new HSETrainingBLL();
        IList lst = hSETrainingBLL.GetAllHSETrainings(strHQL);

        HSETraining hSETraining = (HSETraining)lst[0];

        hSETraining.Speaker = TB_Speaker.Text.Trim();
        hSETraining.TrainingEnd = DateTime.Parse(DLC_TrainingEnd.Text.Trim() + " " + DL_EndHour.SelectedValue.Trim() + ":" + DL_EndMinute.SelectedValue.Trim() + ":00");
        hSETraining.Hoster = TB_Hoster.Text.Trim();
        hSETraining.TrainingContent = TB_TrainingContent.Text.Trim();
        hSETraining.Name = TB_Name.Text.Trim();
        hSETraining.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSETraining.TrainingStart = DateTime.Parse(DLC_TrainingStart.Text.Trim() + " " + DL_StartHour.SelectedValue.Trim() + ":" + DL_StartMinute.SelectedValue.Trim() + ":00");
        hSETraining.ProjectName = GetProjectName(hSETraining.ProjectId.ToString());
        hSETraining.TrainingType = DL_TrainingType.SelectedValue.Trim();
        hSETraining.TrainingSite = TB_TrainingSite.Text.Trim();
        if (hSETraining.TrainingStart > hSETraining.TrainingEnd)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGPKSSJBNDYPJSSJCZSBJC").ToString().Trim() + "')", true);
            DLC_TrainingStart.Focus();
            return;
        }

        try
        {
            hSETrainingBLL.UpdateHSETraining(hSETraining, hSETraining.Code);

            LoadHSETrainingList();

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
        strHQL = "Delete From T_HSETraining Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSETrainingList();

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


                strHQL = "From HSETraining as hSETraining where hSETraining.Code = '" + strCode + "'";
                HSETrainingBLL hSETrainingBLL = new HSETrainingBLL();
                lst = hSETrainingBLL.GetAllHSETrainings(strHQL);

                HSETraining hSETraining = (HSETraining)lst[0];

                LB_Code.Text = hSETraining.Code.Trim();
                TB_Speaker.Text = hSETraining.Speaker.Trim();
                try
                {
                    DL_ProjectId.SelectedValue = hSETraining.ProjectId.ToString().Trim();
                }
                catch
                {
                }
                TB_Hoster.Text = hSETraining.Hoster.Trim();
                TB_TrainingContent.Text = hSETraining.TrainingContent.Trim();
                TB_Name.Text = hSETraining.Name.Trim();
                DLC_TrainingStart.Text = hSETraining.TrainingStart.ToString("yyyy-MM-dd");
                DLC_TrainingEnd.Text = hSETraining.TrainingEnd.ToString("yyyy-MM-dd");
                TB_TrainingSite.Text = hSETraining.TrainingSite.Trim();
                DL_TrainingType.SelectedValue = hSETraining.TrainingType.Trim();
                DL_StartHour.SelectedValue = hSETraining.TrainingStart.ToString("HH");
                DL_StartMinute.SelectedValue = hSETraining.TrainingStart.ToString("mm");
                DL_EndHour.SelectedValue = hSETraining.TrainingEnd.ToString("HH");
                DL_EndMinute.SelectedValue = hSETraining.TrainingEnd.ToString("mm");
                //if (hSETraining.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSETraining");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSETrainingList();
    }
}