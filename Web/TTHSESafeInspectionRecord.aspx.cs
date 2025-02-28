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

public partial class TTHSESafeInspectionRecord : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","安全检查记录", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_InspectionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadProjectName();

            LoadHSESafeInspectionRecordList();
        }
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
        if (IsHSESafeInspectionRecord(string.Empty, TB_Name.Text.Trim()))
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

        HSESafeInspectionRecordBLL hSESafeInspectionRecordBLL = new HSESafeInspectionRecordBLL();
        HSESafeInspectionRecord hSESafeInspectionRecord = new HSESafeInspectionRecord();

        hSESafeInspectionRecord.Code = GetHSESafeInspectionRecordCode();
        LB_Code.Text = hSESafeInspectionRecord.Code.Trim();
        hSESafeInspectionRecord.HeadCode = TB_HeadCode.Text.Trim();
        hSESafeInspectionRecord.InspectContentFindings = TB_InspectContentFindings.Text.Trim();
        hSESafeInspectionRecord.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        hSESafeInspectionRecord.InspectionOverView = TB_InspectionOverView.Text.Trim();
        hSESafeInspectionRecord.InspectionTeamLeader = TB_InspectionTeamLeader.Text.Trim();
        hSESafeInspectionRecord.InspectorsCode = TB_InspectorsCode.Text.Trim();
        hSESafeInspectionRecord.Name = TB_Name.Text.Trim();
        hSESafeInspectionRecord.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSESafeInspectionRecord.ProjectManager = TB_ProjectManager.Text.Trim();
        hSESafeInspectionRecord.ProjectName = GetProjectName(hSESafeInspectionRecord.ProjectId.ToString());
        hSESafeInspectionRecord.QESEngineer = TB_QESEngineer.Text.Trim();
        hSESafeInspectionRecord.Status = DL_Status.SelectedValue.Trim();
        hSESafeInspectionRecord.EnterCode = strUserCode.Trim();

        try
        {
            hSESafeInspectionRecordBLL.AddHSESafeInspectionRecord(hSESafeInspectionRecord);

            LoadHSESafeInspectionRecordList();

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

    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSESafeInspectionRecord(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
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

        string strHQL = "From HSESafeInspectionRecord as hSESafeInspectionRecord where hSESafeInspectionRecord.Code = '" + LB_Code.Text.Trim() + "'";
        HSESafeInspectionRecordBLL hSESafeInspectionRecordBLL = new HSESafeInspectionRecordBLL();
        IList lst = hSESafeInspectionRecordBLL.GetAllHSESafeInspectionRecords(strHQL);

        HSESafeInspectionRecord hSESafeInspectionRecord = (HSESafeInspectionRecord)lst[0];

        //   hSESafeInspectionRecord.Code = LB_Code.Text.Trim();
        hSESafeInspectionRecord.HeadCode = TB_HeadCode.Text.Trim();
        hSESafeInspectionRecord.InspectContentFindings = TB_InspectContentFindings.Text.Trim();
        hSESafeInspectionRecord.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        hSESafeInspectionRecord.InspectionOverView = TB_InspectionOverView.Text.Trim();
        hSESafeInspectionRecord.InspectionTeamLeader = TB_InspectionTeamLeader.Text.Trim();
        hSESafeInspectionRecord.InspectorsCode = TB_InspectorsCode.Text.Trim();
        hSESafeInspectionRecord.Name = TB_Name.Text.Trim();
        hSESafeInspectionRecord.ProjectId = int.Parse(DL_ProjectId.SelectedValue.Trim());
        hSESafeInspectionRecord.ProjectManager = TB_ProjectManager.Text.Trim();
        hSESafeInspectionRecord.ProjectName = GetProjectName(hSESafeInspectionRecord.ProjectId.ToString());
        hSESafeInspectionRecord.QESEngineer = TB_QESEngineer.Text.Trim();
        hSESafeInspectionRecord.Status = DL_Status.SelectedValue.Trim();

        try
        {
            hSESafeInspectionRecordBLL.UpdateHSESafeInspectionRecord(hSESafeInspectionRecord, hSESafeInspectionRecord.Code);

            LoadHSESafeInspectionRecordList();

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

                strHQL = "From HSESafeInspectionRecord as hSESafeInspectionRecord where hSESafeInspectionRecord.Code = '" + strCode + "'";
                HSESafeInspectionRecordBLL hSESafeInspectionRecordBLL = new HSESafeInspectionRecordBLL();
                lst = hSESafeInspectionRecordBLL.GetAllHSESafeInspectionRecords(strHQL);

                HSESafeInspectionRecord hSESafeInspectionRecord = (HSESafeInspectionRecord)lst[0];

                LB_Code.Text = hSESafeInspectionRecord.Code.Trim();
                TB_HeadCode.Text = hSESafeInspectionRecord.HeadCode.Trim();

                try
                {
                    DL_ProjectId.SelectedValue = hSESafeInspectionRecord.ProjectId.ToString().Trim();
                }
                catch
                {

                }
                TB_InspectContentFindings.Text = hSESafeInspectionRecord.InspectContentFindings.Trim();
                TB_InspectionOverView.Text = hSESafeInspectionRecord.InspectionOverView.Trim();
                TB_InspectionTeamLeader.Text = hSESafeInspectionRecord.InspectionTeamLeader.Trim();
                TB_InspectorsCode.Text = hSESafeInspectionRecord.InspectorsCode.Trim();
                TB_Name.Text = hSESafeInspectionRecord.Name.Trim();
                TB_ProjectManager.Text = hSESafeInspectionRecord.ProjectManager.Trim();
                TB_QESEngineer.Text = hSESafeInspectionRecord.QESEngineer.Trim();
                DL_Status.SelectedValue = hSESafeInspectionRecord.Status.Trim();
                DLC_InspectionDate.Text = hSESafeInspectionRecord.InspectionDate.ToString("yyyy-MM-dd");
                //if (hSESafeInspectionRecord.EnterCode.Trim() == strUserCode.Trim())
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


    protected void Delete()
    {
        string strHQL;
        string strCode = LB_Code.Text.Trim();
        if (IsHSERectificationNotice(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZAYHZGTZDZYDYGAJJLDWFSC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_HSESafeInspectionRecord Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSESafeInspectionRecordList();

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
    /// 删除时，检查安全隐患整改通知是否存在该安检单，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSERectificationNotice(string strCode)
    {
        bool flag = true;
        string strHQL = "Select Code From T_HSERectificationNotice Where SafeInspectId='" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectificationNotice").Tables[0];
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

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeInspectionRecord");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    /// <summary>
    /// 新增或更新时，检查安检名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSESafeInspectionRecord(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSESafeInspectionRecord Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSESafeInspectionRecord Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeInspectionRecord").Tables[0];
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
    /// 新增时，获取表T_HSESafeInspectionRecord中最大编号 规则HSESIRX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSESafeInspectionRecordCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSESafeInspectionRecord Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeInspectionRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSESIR" + pa.ToString();
        }
        else
        {
            flag = "HSESIR1";
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

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSESafeInspectionRecordList();
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

    protected void LoadHSESafeInspectionRecordList()
    {
        string strHQL;

        strHQL = "Select * From T_HSESafeInspectionRecord Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or InspectionOverView like '%" + TextBox1.Text.Trim() + "%' or InspectContentFindings like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (ProjectId like '%" + TextBox2.Text.Trim() + "%' or ProjectName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-InspectionDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-InspectionDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSESafeInspectionRecord");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
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
}