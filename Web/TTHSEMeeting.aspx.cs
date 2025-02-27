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

public partial class TTHSEMeeting : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","HSE例会", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TB_Hoster.Text = ShareClass.GetUserName(strUserCode.Trim());
            TB_DepartName.Text = GetUserDepartName(strUserCode.Trim());

            LoadProjectName();

            LoadHSEMeetingList();
        }
    }

    /// <summary>
    /// 获取人员所在部门
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <returns></returns>
    protected string GetUserDepartName(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            strHQL = "From Department as department Where department.DepartCode = '" + projectMember.DepartCode.Trim() + "'";
            DepartmentBLL departmentBLL = new DepartmentBLL();
            lst = departmentBLL.GetAllDepartments(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                Department department = (Department)lst[0];
                return department.DepartName.Trim();
            }
            else
                return "";
        }
        else
            return "";
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

    protected void LoadHSEMeetingList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEMeeting Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or MeetingPlace like '%" + TextBox1.Text.Trim() + "%' or DepartName like '%" + TextBox1.Text.Trim() + "%' " +
                "or SummaryType like '%" + TextBox1.Text.Trim() + "%' or SummaryContent like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (ProjectId like '%" + TextBox2.Text.Trim() + "%' or ProjectName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-MeetingDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-MeetingDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEMeeting");

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGHYZTBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEMeeting(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGHYZTYCZCZSBJC").ToString().Trim() + "')", true);
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

        HSEMeetingBLL hSEMeetingBLL = new HSEMeetingBLL();
        HSEMeeting hSEMeeting = new HSEMeeting();

        hSEMeeting.Code = GetHSEMeetingCode();
        LB_Code.Text = hSEMeeting.Code.Trim();
        hSEMeeting.DepartCode = "";
        hSEMeeting.DepartName = TB_DepartName.Text.Trim();
        hSEMeeting.EndTime = DateTime.Parse(DLC_EndTime.Text.Trim() + " " + DL_EndHour.SelectedValue.Trim() + ":" + DL_EndMinute.SelectedValue.Trim() + ":00");
        hSEMeeting.Hoster = TB_Hoster.Text.Trim();
        hSEMeeting.SummaryContent = TB_SummaryContent.Text.Trim();
        hSEMeeting.Name = TB_Name.Text.Trim();
        hSEMeeting.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSEMeeting.StartTime = DateTime.Parse(DLC_StartTime.Text.Trim() + " " + DL_StartHour.SelectedValue.Trim() + ":" + DL_StartMinute.SelectedValue.Trim() + ":00");
        hSEMeeting.MeetingDate = DateTime.Parse(DLC_StartTime.Text.Trim());
        hSEMeeting.ProjectName = GetProjectName(hSEMeeting.ProjectId.ToString());
        hSEMeeting.SummaryType = DL_SummaryType.SelectedValue.Trim();
        hSEMeeting.MeetingPlace = TB_MeetingPlace.Text.Trim();
        hSEMeeting.Participants = TB_Participants.Text.Trim();

        if (hSEMeeting.StartTime > hSEMeeting.EndTime)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGHYKSSJBNDYHYJSSJCZSBJC").ToString().Trim() + "')", true);
            DLC_StartTime.Focus();
            return;
        }
        hSEMeeting.EnterCode = strUserCode.Trim();

        try
        {
            hSEMeetingBLL.AddHSEMeeting(hSEMeeting);

            LoadHSEMeetingList();

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
    /// 新增或更新时，检查HSE例会主题是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEMeeting(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSEMeeting Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSEMeeting Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEMeeting").Tables[0];
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
    /// 新增时，获取表T_HSEMeeting中最大编号 规则HSEMTGX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEMeetingCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEMeeting Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEMeeting").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEMTG" + pa.ToString();
        }
        else
        {
            flag = "HSEMTG1";
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGHYZTBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEMeeting(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGHYZTYCZCZSBJC").ToString().Trim() + "')", true);
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

        string strHQL = "From HSEMeeting as hSEMeeting where hSEMeeting.Code = '" + LB_Code.Text.Trim() + "'";
        HSEMeetingBLL hSEMeetingBLL = new HSEMeetingBLL();
        IList lst = hSEMeetingBLL.GetAllHSEMeetings(strHQL);

        HSEMeeting hSEMeeting = (HSEMeeting)lst[0];

        hSEMeeting.DepartName = TB_DepartName.Text.Trim();
        hSEMeeting.EndTime = DateTime.Parse(DLC_EndTime.Text.Trim() + " " + DL_EndHour.SelectedValue.Trim() + ":" + DL_EndMinute.SelectedValue.Trim() + ":00");
        hSEMeeting.Hoster = TB_Hoster.Text.Trim();
        hSEMeeting.SummaryContent = TB_SummaryContent.Text.Trim();
        hSEMeeting.Name = TB_Name.Text.Trim();
        hSEMeeting.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSEMeeting.StartTime = DateTime.Parse(DLC_StartTime.Text.Trim() + " " + DL_StartHour.SelectedValue.Trim() + ":" + DL_StartMinute.SelectedValue.Trim() + ":00");
        hSEMeeting.MeetingDate = DateTime.Parse(DLC_StartTime.Text.Trim());
        hSEMeeting.ProjectName = GetProjectName(hSEMeeting.ProjectId.ToString());
        hSEMeeting.SummaryType = DL_SummaryType.SelectedValue.Trim();
        hSEMeeting.MeetingPlace = TB_MeetingPlace.Text.Trim();
        hSEMeeting.Participants = TB_Participants.Text.Trim();

        if (hSEMeeting.StartTime > hSEMeeting.EndTime)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGHYKSSJBNDYHYJSSJCZSBJC").ToString().Trim() + "')", true);
            DLC_StartTime.Focus();
            return;
        }

        try
        {
            hSEMeetingBLL.UpdateHSEMeeting(hSEMeeting, hSEMeeting.Code);

            LoadHSEMeetingList();

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
        strHQL = "Delete From T_HSEMeeting Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEMeetingList();

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


                strHQL = "From HSEMeeting as hSEMeeting where hSEMeeting.Code = '" + strCode + "'";
                HSEMeetingBLL hSEMeetingBLL = new HSEMeetingBLL();
                lst = hSEMeetingBLL.GetAllHSEMeetings(strHQL);

                HSEMeeting hSEMeeting = (HSEMeeting)lst[0];

                LB_Code.Text = hSEMeeting.Code.Trim();
                TB_DepartName.Text = hSEMeeting.DepartName.Trim();

                try
                {
                    DL_ProjectId.SelectedValue = hSEMeeting.ProjectId.ToString().Trim();
                }
                catch
                {
                }
                TB_Hoster.Text = hSEMeeting.Hoster.Trim();
                TB_SummaryContent.Text = hSEMeeting.SummaryContent.Trim();
                TB_Name.Text = hSEMeeting.Name.Trim();
                DLC_StartTime.Text = hSEMeeting.StartTime.ToString("yyyy-MM-dd");
                DL_StartHour.SelectedValue = hSEMeeting.StartTime.ToString("HH");
                DL_StartMinute.SelectedValue = hSEMeeting.StartTime.ToString("mm");
                DLC_EndTime.Text = hSEMeeting.EndTime.ToString("yyyy-MM-dd");
                DL_EndHour.SelectedValue = hSEMeeting.EndTime.ToString("HH");
                DL_EndMinute.SelectedValue = hSEMeeting.EndTime.ToString("mm");
                TB_MeetingPlace.Text = hSEMeeting.MeetingPlace.Trim();
                DL_SummaryType.SelectedValue = hSEMeeting.SummaryType.Trim();
                TB_Participants.Text = hSEMeeting.Participants.Trim();
                //if (hSEMeeting.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEMeeting");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEMeetingList();
    }
}