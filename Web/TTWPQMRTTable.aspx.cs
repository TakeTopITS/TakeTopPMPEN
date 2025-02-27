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

public partial class TTWPQMRTTable : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","RTπ‹¿Ì", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_RTCommissionedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWPQMWeldProQuaName();
            LoadWPQMRTTableList();
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

    protected void LoadWPQMRTTableList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMRTTable Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or NondestructTestCategory like '%" + TextBox1.Text.Trim() + "%' or InspectionProportion like '%" + TextBox1.Text.Trim() + "%' or QualifiedLevel like '%" + TextBox1.Text.Trim() + "%' " +
            "or RTEvaluationCriteria like '%" + TextBox1.Text.Trim() + "%' or NumberSpecimens like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-RTCommissionedDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-RTCommissionedDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMRTTable");

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
        if (IsWPQMRTTable(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGRTGLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMRTTableBLL wPQMRTTableBLL = new WPQMRTTableBLL();
        WPQMRTTable wPQMRTTable = new WPQMRTTable();
        wPQMRTTable.RTCommissionedDate = DateTime.Parse(string.IsNullOrEmpty(DLC_RTCommissionedDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_RTCommissionedDate.Text.Trim());
        wPQMRTTable.QualifiedLevel = TB_QualifiedLevel.Text.Trim();
        wPQMRTTable.NumberSpecimens = NB_NumberSpecimens.Text.Trim();
        wPQMRTTable.EnterCode = strUserCode.Trim();
        wPQMRTTable.NondestructTestCategory = TB_NondestructTestCategory.Text.Trim();
        wPQMRTTable.RTEvaluationCriteria = TB_RTEvaluationCriteria.Text.Trim();
        wPQMRTTable.InspectionProportion = TB_InspectionProportion.Text.Trim();
        wPQMRTTable.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMRTTableBLL.AddWPQMRTTable(wPQMRTTable);
            lbl_ID.Text = GetMaxWPQMRTTableID(wPQMRTTable).ToString();
            UpdateWPQMWeldProQuaData(wPQMRTTable.WeldProCode.Trim());
            LoadWPQMRTTableList();

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

    protected int GetMaxWPQMRTTableID(WPQMRTTable bmbp)
    {
        string strHQL = "Select ID From T_WPQMRTTable where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMRTTable").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMRTTable(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMRTTable Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMRTTable Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMRTTable").Tables[0];
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
        if (IsWPQMRTTable(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGRTGLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMRTTable as wPQMRTTable where wPQMRTTable.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMRTTableBLL wPQMRTTableBLL = new WPQMRTTableBLL();
        IList lst = wPQMRTTableBLL.GetAllWPQMRTTables(strHQL);
        WPQMRTTable wPQMRTTable = (WPQMRTTable)lst[0];

        wPQMRTTable.RTCommissionedDate = DateTime.Parse(string.IsNullOrEmpty(DLC_RTCommissionedDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_RTCommissionedDate.Text.Trim());
        wPQMRTTable.QualifiedLevel = TB_QualifiedLevel.Text.Trim();
        wPQMRTTable.NumberSpecimens = NB_NumberSpecimens.Text.Trim();
        wPQMRTTable.NondestructTestCategory = TB_NondestructTestCategory.Text.Trim();
        wPQMRTTable.RTEvaluationCriteria = TB_RTEvaluationCriteria.Text.Trim();
        wPQMRTTable.InspectionProportion = TB_InspectionProportion.Text.Trim();
        wPQMRTTable.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMRTTableBLL.UpdateWPQMRTTable(wPQMRTTable, wPQMRTTable.ID);
            UpdateWPQMWeldProQuaData(wPQMRTTable.WeldProCode.Trim());
            LoadWPQMRTTableList();

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
        string strHQL = "Delete From T_WPQMRTTable Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMRTTableList();
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
        LoadWPQMRTTableList();
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
            strHQL = "From WPQMRTTable as wPQMRTTable where wPQMRTTable.ID = '" + strId + "'";
            WPQMRTTableBLL wPQMRTTableBLL = new WPQMRTTableBLL();
            lst = wPQMRTTableBLL.GetAllWPQMRTTables(strHQL);
            WPQMRTTable wPQMRTTable = (WPQMRTTable)lst[0];
            TB_QualifiedLevel.Text = wPQMRTTable.QualifiedLevel.Trim();
            TB_NondestructTestCategory.Text = wPQMRTTable.NondestructTestCategory.Trim();
            TB_RTEvaluationCriteria.Text = wPQMRTTable.RTEvaluationCriteria.Trim();
            NB_NumberSpecimens.Text = wPQMRTTable.NumberSpecimens.Trim();
            TB_InspectionProportion.Text = wPQMRTTable.InspectionProportion.Trim();
            DL_WeldProCode.SelectedValue = wPQMRTTable.WeldProCode.Trim();
            DLC_RTCommissionedDate.Text = wPQMRTTable.RTCommissionedDate.ToString("yyyy-MM-dd");
            lbl_ID.Text = wPQMRTTable.ID.ToString();

            if (wPQMRTTable.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMRTTable");
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
            NB_NumberSpecimens.Text = string.IsNullOrEmpty(wPQMWeldProQua.NumberSpecimens) ? "" : wPQMWeldProQua.NumberSpecimens.Trim();
        }
        else
        {
            NB_NumberSpecimens.Text = "";
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
            wPQMWeldProQua.NumberSpecimens = NB_NumberSpecimens.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }
}