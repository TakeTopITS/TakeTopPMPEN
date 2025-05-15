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

public partial class TTWPQMQuaAssessNotice : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","合格工评通知单", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_NoteSentTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            TB_NoteSender.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadWPQMWeldProQuaName();
            LoadWPQMQuaAssessNoticeList();
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

    protected void LoadWPQMQuaAssessNoticeList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMQuaAssessNotice Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or NoteSender like '%" + TextBox1.Text.Trim() + "%' or NoteReviewer like '%" + TextBox1.Text.Trim() + "%' or NoteRecipient like '%" + TextBox1.Text.Trim() + "%' " +
            "or Conclusion like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-NoteSentTime::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-NoteSentTime::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMQuaAssessNotice");

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
        if (IsWPQMQuaAssessNotice(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGHGGPTZDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMQuaAssessNoticeBLL wPQMQuaAssessNoticeBLL = new WPQMQuaAssessNoticeBLL();
        WPQMQuaAssessNotice wPQMQuaAssessNotice = new WPQMQuaAssessNotice();
        wPQMQuaAssessNotice.NoteSentTime = DateTime.Parse(string.IsNullOrEmpty(DLC_NoteSentTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DLC_NoteSentTime.Text.Trim());
        wPQMQuaAssessNotice.NoteSender = TB_NoteSender.Text.Trim();
        wPQMQuaAssessNotice.EnterCode = strUserCode.Trim();
        wPQMQuaAssessNotice.NoteRecipient = TB_NoteRecipient.Text.Trim();
        wPQMQuaAssessNotice.Conclusion = TB_Conclusion.Text.Trim();
        wPQMQuaAssessNotice.NoteReviewer = TB_NoteReviewer.Text.Trim();
        wPQMQuaAssessNotice.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMQuaAssessNoticeBLL.AddWPQMQuaAssessNotice(wPQMQuaAssessNotice);
            lbl_ID.Text = GetMaxWPQMQuaAssessNoticeID(wPQMQuaAssessNotice).ToString();
            
            LoadWPQMQuaAssessNoticeList();

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

    protected int GetMaxWPQMQuaAssessNoticeID(WPQMQuaAssessNotice bmbp)
    {
        string strHQL = "Select ID From T_WPQMQuaAssessNotice where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMQuaAssessNotice").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMQuaAssessNotice(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMQuaAssessNotice Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMQuaAssessNotice Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMQuaAssessNotice").Tables[0];
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
        if (IsWPQMQuaAssessNotice(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGHGGPTZDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMQuaAssessNotice as wPQMQuaAssessNotice where wPQMQuaAssessNotice.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMQuaAssessNoticeBLL wPQMQuaAssessNoticeBLL = new WPQMQuaAssessNoticeBLL();
        IList lst = wPQMQuaAssessNoticeBLL.GetAllWPQMQuaAssessNotices(strHQL);
        WPQMQuaAssessNotice wPQMQuaAssessNotice = (WPQMQuaAssessNotice)lst[0];

        wPQMQuaAssessNotice.NoteSentTime = DateTime.Parse(string.IsNullOrEmpty(DLC_NoteSentTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DLC_NoteSentTime.Text.Trim());
        wPQMQuaAssessNotice.NoteSender = TB_NoteSender.Text.Trim();
        wPQMQuaAssessNotice.NoteRecipient = TB_NoteRecipient.Text.Trim();
        wPQMQuaAssessNotice.Conclusion = TB_Conclusion.Text.Trim();
        wPQMQuaAssessNotice.NoteReviewer = TB_NoteReviewer.Text.Trim();
        wPQMQuaAssessNotice.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMQuaAssessNoticeBLL.UpdateWPQMQuaAssessNotice(wPQMQuaAssessNotice, wPQMQuaAssessNotice.ID);
            
            LoadWPQMQuaAssessNoticeList();

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
        string strHQL = "Delete From T_WPQMQuaAssessNotice Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMQuaAssessNoticeList();
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
        LoadWPQMQuaAssessNoticeList();
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
            strHQL = "From WPQMQuaAssessNotice as wPQMQuaAssessNotice where wPQMQuaAssessNotice.ID = '" + strId + "'";
            WPQMQuaAssessNoticeBLL wPQMQuaAssessNoticeBLL = new WPQMQuaAssessNoticeBLL();
            lst = wPQMQuaAssessNoticeBLL.GetAllWPQMQuaAssessNotices(strHQL);
            WPQMQuaAssessNotice wPQMQuaAssessNotice = (WPQMQuaAssessNotice)lst[0];
            TB_NoteSender.Text = wPQMQuaAssessNotice.NoteSender.Trim();
            TB_NoteRecipient.Text = wPQMQuaAssessNotice.NoteRecipient.Trim();
            TB_Conclusion.Text = wPQMQuaAssessNotice.Conclusion.Trim();
            TB_NoteReviewer.Text = wPQMQuaAssessNotice.NoteReviewer.Trim();
            DL_WeldProCode.SelectedValue = wPQMQuaAssessNotice.WeldProCode.Trim();
            DLC_NoteSentTime.Text = wPQMQuaAssessNotice.NoteSentTime.ToString("yyyy-MM-dd HH:mm:ss");
            lbl_ID.Text = wPQMQuaAssessNotice.ID.ToString();

            if (wPQMQuaAssessNotice.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMQuaAssessNotice");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}