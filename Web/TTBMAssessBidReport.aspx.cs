using System; using System.Resources;
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

public partial class TTBMAssessBidReport : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_AssessReportDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_AssessSpeaker.Text = ShareClass.GetUserName(strUserCode);
            DLC_ReviewDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadBMAssessBidRecordName();

            LoadBMAssessBidReportList();
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

    protected void LoadBMAssessBidRecordName()
    {
        string strHQL;
        //绑定评标记录名称T_BMAssessBidRecord
     //   strHQL = "select * From T_BMAssessBidRecord where ID not in (select AssessBidRecordID from T_BMAssessBidReport) Order By ID Desc";
        strHQL = "select * From T_BMAssessBidRecord Order By ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidRecord");
        DL_AssessBidRecordID.DataSource = ds;
        DL_AssessBidRecordID.DataBind();
        DL_AssessBidRecordID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    /// <summary>
    /// 新增或更新时，评标记录ID是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMAssessBidReportAssessBidRecordID(string strAssessBidRecordId, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMAssessBidReport Where AssessBidRecordID='" + strAssessBidRecordId + "' ";
        }
        else
            strHQL = "Select ID From T_BMAssessBidReport Where AssessBidRecordID='" + strAssessBidRecordId + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport").Tables[0];
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

    protected void LoadBMAssessBidReportList()
    {
        string strHQL;

        strHQL = "Select * From T_BMAssessBidReport Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (AssessSpeaker like '%" + TextBox1.Text.Trim() + "%' or AssessReportContent like '%" + TextBox1.Text.Trim() + "%' or Reviewer like '%" + TextBox1.Text.Trim() + "%' " +
            "or ReviewResult like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and AssessBidRecordName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-AssessReportDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-AssessReportDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (DL_AssessBidRecordID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGPBJLBJC").ToString().Trim()+"')", true);
            DL_AssessBidRecordID.Focus();
            return;
        }
        if (IsBMAssessBidReportAssessBidRecordID(DL_AssessBidRecordID.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGPBJLYJXPBBGJC").ToString().Trim()+"')", true);
            DL_AssessBidRecordID.Focus();
            return;
        }
        BMAssessBidReportBLL bMAssessBidReportBLL = new BMAssessBidReportBLL();
        BMAssessBidReport bMAssessBidReport = new BMAssessBidReport();

        bMAssessBidReport.AssessSpeaker = TB_AssessSpeaker.Text.Trim();
        bMAssessBidReport.Reviewer = TB_Reviewer.Text.Trim();
        bMAssessBidReport.ReviewResult = TB_ReviewResult.Text.Trim();
        bMAssessBidReport.AssessReportContent = TB_AssessReportContent.Text.Trim();
        bMAssessBidReport.AssessBidRecordID = int.Parse(DL_AssessBidRecordID.SelectedValue.Trim());
        bMAssessBidReport.AssessBidRecordName = GetBMAssessBidRecordName(bMAssessBidReport.AssessBidRecordID.ToString().Trim());
        bMAssessBidReport.AssessReportDate = DateTime.Parse(DLC_AssessReportDate.Text.Trim());
        bMAssessBidReport.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());

        try
        {
            bMAssessBidReportBLL.AddBMAssessBidReport(bMAssessBidReport);
            TB_ID.Text = GetMaxBMAssessBidReportID(bMAssessBidReport).ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);

            LoadBMAssessBidReportList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    /// <summary>
    /// 新增时，获取表T_BMAssessBidReport中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMAssessBidReportID(BMAssessBidReport bmbp)
    {
        string strHQL = "Select ID From T_BMAssessBidReport where AssessSpeaker='" + bmbp.AssessSpeaker.Trim() + "' and AssessBidRecordID='" + bmbp.AssessBidRecordID.ToString().Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMAssessBidRecordName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定评标记录名称
        strHQL = "From BMAssessBidRecord as bMAssessBidRecord Where bMAssessBidRecord.ID='" + strID + "' ";
        BMAssessBidRecordBLL bMAssessBidRecordBLL = new BMAssessBidRecordBLL();
        lst = bMAssessBidRecordBLL.GetAllBMAssessBidRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAssessBidRecord bMAssessBidRecord = (BMAssessBidRecord)lst[0];
            return bMAssessBidRecord.Name.Trim();
        }
        else
            return "";
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (DL_AssessBidRecordID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGPBJLBJC").ToString().Trim()+"')", true);
            DL_AssessBidRecordID.Focus();
            return;
        }
        if (IsBMAssessBidReportAssessBidRecordID(DL_AssessBidRecordID.SelectedValue.Trim(), TB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGPBJLYJXPBBGJC").ToString().Trim()+"')", true);
            DL_AssessBidRecordID.Focus();
            return;
        }
        string strHQL = "From BMAssessBidReport as bMAssessBidReport where bMAssessBidReport.ID = '" + TB_ID.Text.Trim() + "'";
        BMAssessBidReportBLL bMAssessBidReportBLL = new BMAssessBidReportBLL();
        IList lst = bMAssessBidReportBLL.GetAllBMAssessBidReports(strHQL);
        BMAssessBidReport bMAssessBidReport = (BMAssessBidReport)lst[0];

        bMAssessBidReport.AssessSpeaker = TB_AssessSpeaker.Text.Trim();
        bMAssessBidReport.Reviewer = TB_Reviewer.Text.Trim();
        bMAssessBidReport.ReviewResult = TB_ReviewResult.Text.Trim();
        bMAssessBidReport.AssessReportContent = TB_AssessReportContent.Text.Trim();
        bMAssessBidReport.AssessBidRecordID = int.Parse(DL_AssessBidRecordID.SelectedValue.Trim());
        bMAssessBidReport.AssessBidRecordName = GetBMAssessBidRecordName(bMAssessBidReport.AssessBidRecordID.ToString().Trim());
        bMAssessBidReport.AssessReportDate = DateTime.Parse(DLC_AssessReportDate.Text.Trim());
        bMAssessBidReport.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());

        try
        {
            bMAssessBidReportBLL.UpdateBMAssessBidReport(bMAssessBidReport, bMAssessBidReport.ID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

            LoadBMAssessBidReportList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = TB_ID.Text.Trim();

        strHQL = "Delete From T_BMAssessBidReport Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);

            LoadBMAssessBidReportList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From BMAssessBidReport as bMAssessBidReport where bMAssessBidReport.ID = '" + strId + "'";
            BMAssessBidReportBLL bMAssessBidReportBLL = new BMAssessBidReportBLL();
            lst = bMAssessBidReportBLL.GetAllBMAssessBidReports(strHQL);
            BMAssessBidReport bMAssessBidReport = (BMAssessBidReport)lst[0];

            TB_ID.Text = bMAssessBidReport.ID.ToString().Trim();
            DL_AssessBidRecordID.SelectedValue = bMAssessBidReport.AssessBidRecordID.ToString().Trim();
            DLC_AssessReportDate.Text = bMAssessBidReport.AssessReportDate.ToString("yyyy-MM-dd");
            TB_AssessSpeaker.Text = bMAssessBidReport.AssessSpeaker.Trim();
            TB_AssessReportContent.Text = bMAssessBidReport.AssessReportContent.Trim();
            TB_Reviewer.Text = bMAssessBidReport.Reviewer.Trim();
            TB_ReviewResult.Text = bMAssessBidReport.ReviewResult.Trim();
            DLC_ReviewDate.Text = bMAssessBidReport.ReviewDate.ToString("yyyy-MM-dd");

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMAssessBidReportList();
    }
}