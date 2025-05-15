using System; using System.Resources;
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

public partial class TTWPQMWeldTaskList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","焊接任务单管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_WeldTaskCommissionTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_UpTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            LoadWPQMWeldProQuaName();
            LoadWPQMWeldTaskListList();
        }
    }

    protected void LoadWPQMWeldProQuaName()
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Order By wPQMWeldProQua.Code Desc";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        DL_WeldProCode.DataSource = lst;
        DL_WeldProCode.DataBind();
        DL_WeldProCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMWeldTaskListList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMWeldTaskList Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or GrooveForm like '%" + TextBox1.Text.Trim() + "%' or BackClearRootMethod like '%" + TextBox1.Text.Trim() + "%' or WeldTechnicalMeasures like '%" + TextBox1.Text.Trim() + "%' " +
            "or TaskPrincipal like '%" + TextBox1.Text.Trim() + "%' or ReviewerTask like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-WeldTaskCommissionTime::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-WeldTaskCommissionTime::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldTaskList");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJGYPDWBXJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMWeldTaskList(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGHJRWDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMWeldTaskListBLL wPQMWeldTaskListBLL = new WPQMWeldTaskListBLL();
        WPQMWeldTaskList wPQMWeldTaskList = new WPQMWeldTaskList();
        wPQMWeldTaskList.WeldTaskCommissionTime = DateTime.Parse(string.IsNullOrEmpty(DLC_WeldTaskCommissionTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_WeldTaskCommissionTime.Text.Trim());
        wPQMWeldTaskList.TaskPrincipal = TB_TaskPrincipal.Text.Trim();
        wPQMWeldTaskList.ReviewerTask = TB_ReviewerTask.Text.Trim();
        wPQMWeldTaskList.EnterCode = strUserCode.Trim();
        wPQMWeldTaskList.GrooveForm = TB_GrooveForm.Text.Trim();
        wPQMWeldTaskList.WeldTechnicalMeasures = TB_WeldTechnicalMeasures.Text.Trim();
        wPQMWeldTaskList.UpTime = DateTime.Parse(string.IsNullOrEmpty(DLC_UpTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DLC_UpTime.Text.Trim());
        wPQMWeldTaskList.BackClearRootMethod = TB_BackClearRootMethod.Text.Trim();
        wPQMWeldTaskList.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMWeldTaskListBLL.AddWPQMWeldTaskList(wPQMWeldTaskList);
            lbl_ID.Text = GetMaxWPQMWeldTaskListID(wPQMWeldTaskList).ToString();
            UpdateWPQMWeldProQuaData(wPQMWeldTaskList.WeldProCode.Trim());
            LoadWPQMWeldTaskListList();

            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected int GetMaxWPQMWeldTaskListID(WPQMWeldTaskList bmbp)
    {
        string strHQL = "Select ID From T_WPQMWeldTaskList where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldTaskList").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMWeldTaskList(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMWeldTaskList Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMWeldTaskList Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldTaskList").Tables[0];
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

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJGYPDWBXJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMWeldTaskList(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGHJRWDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMWeldTaskList as wPQMWeldTaskList where wPQMWeldTaskList.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMWeldTaskListBLL wPQMWeldTaskListBLL = new WPQMWeldTaskListBLL();
        IList lst = wPQMWeldTaskListBLL.GetAllWPQMWeldTaskLists(strHQL);
        WPQMWeldTaskList wPQMWeldTaskList = (WPQMWeldTaskList)lst[0];

        wPQMWeldTaskList.WeldTaskCommissionTime = DateTime.Parse(string.IsNullOrEmpty(DLC_WeldTaskCommissionTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_WeldTaskCommissionTime.Text.Trim());
        wPQMWeldTaskList.TaskPrincipal = TB_TaskPrincipal.Text.Trim();
        wPQMWeldTaskList.ReviewerTask = TB_ReviewerTask.Text.Trim();
        wPQMWeldTaskList.GrooveForm = TB_GrooveForm.Text.Trim();
        wPQMWeldTaskList.WeldTechnicalMeasures = TB_WeldTechnicalMeasures.Text.Trim();
        wPQMWeldTaskList.UpTime = DateTime.Parse(string.IsNullOrEmpty(DLC_UpTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DLC_UpTime.Text.Trim());
        wPQMWeldTaskList.BackClearRootMethod = TB_BackClearRootMethod.Text.Trim();
        wPQMWeldTaskList.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMWeldTaskListBLL.UpdateWPQMWeldTaskList(wPQMWeldTaskList, wPQMWeldTaskList.ID);
            UpdateWPQMWeldProQuaData(wPQMWeldTaskList.WeldProCode.Trim());
            LoadWPQMWeldTaskListList();

            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID = lbl_ID.Text.Trim();
        string strHQL = "Delete From T_WPQMWeldTaskList Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMWeldTaskListList();
            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWPQMWeldTaskListList();
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
            strHQL = "From WPQMWeldTaskList as wPQMWeldTaskList where wPQMWeldTaskList.ID = '" + strId + "'";
            WPQMWeldTaskListBLL wPQMWeldTaskListBLL = new WPQMWeldTaskListBLL();
            lst = wPQMWeldTaskListBLL.GetAllWPQMWeldTaskLists(strHQL);
            WPQMWeldTaskList wPQMWeldTaskList = (WPQMWeldTaskList)lst[0];
            TB_TaskPrincipal.Text = wPQMWeldTaskList.TaskPrincipal.Trim();
            TB_ReviewerTask.Text = wPQMWeldTaskList.ReviewerTask.Trim();
            TB_GrooveForm.Text = wPQMWeldTaskList.GrooveForm.Trim();
            TB_WeldTechnicalMeasures.Text = wPQMWeldTaskList.WeldTechnicalMeasures.Trim();
            DLC_UpTime.Text = wPQMWeldTaskList.UpTime.ToString("yyyy-MM-dd HH:mm:ss");
            TB_BackClearRootMethod.Text = wPQMWeldTaskList.BackClearRootMethod.Trim();
            DL_WeldProCode.SelectedValue = wPQMWeldTaskList.WeldProCode.Trim();
            DLC_WeldTaskCommissionTime.Text = wPQMWeldTaskList.WeldTaskCommissionTime.ToString("yyyy-MM-dd");
            lbl_ID.Text = wPQMWeldTaskList.ID.ToString();

            if (wPQMWeldTaskList.EnterCode.Trim() == strUserCode.Trim())
            {
                BT_Delete.Visible = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_Update.Visible = false;
                BT_Delete.Visible = false;
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldTaskList");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DL_WeldProCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + DL_WeldProCode.SelectedValue.Trim() + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            TB_GrooveForm.Text = string.IsNullOrEmpty(wPQMWeldProQua.GrooveForm) ? "" : wPQMWeldProQua.GrooveForm.Trim();
            TB_BackClearRootMethod.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackClearRootMethod) ? "" : wPQMWeldProQua.BackClearRootMethod.Trim();
        }
        else
        {
            TB_GrooveForm.Text = "";
            TB_BackClearRootMethod.Text = "";
        }
    }

    protected void UpdateWPQMWeldProQuaData(string strCode)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + strCode + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            wPQMWeldProQua.GrooveForm = TB_GrooveForm.Text.Trim();
            wPQMWeldProQua.BackClearRootMethod = TB_BackClearRootMethod.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }
}