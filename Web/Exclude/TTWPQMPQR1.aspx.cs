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

public partial class TTWPQMPQR1 : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","PQR-1№ЬАн", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMWeldProQuaName();
            LoadWPQMPQR1List();
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

    protected void LoadWPQMPQR1List()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMPQR1 Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or SecurityMeasureOther like '%" + TextBox1.Text.Trim() + "%' or WeldJointOther like '%" + TextBox1.Text.Trim() + "%' " +
            "or ElecCharaOther like '%" + TextBox1.Text.Trim() + "%' or WeldingCurrent like '%" + TextBox1.Text.Trim() + "%' or WeldingSpeed like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR1");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSHJGYPDWBXJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMPQR1(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGPQR1YCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMPQR1BLL wPQMPQR1BLL = new WPQMPQR1BLL();
        WPQMPQR1 wPQMPQR1 = new WPQMPQR1();

        wPQMPQR1.EnterCode = strUserCode.Trim();
        wPQMPQR1.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMPQR1.SecurityMeasureOther = TB_SecurityMeasureOther.Text.Trim();
        wPQMPQR1.ElecCharaOther = TB_ElecCharaOther.Text.Trim();
        wPQMPQR1.WeldJointOther = TB_WeldJointOther.Text.Trim();
        wPQMPQR1.WeldMetalThick = TB_WeldMetalThick.Text.Trim();
        wPQMPQR1.ArcVoltage = TB_ArcVoltage.Text.Trim();
        wPQMPQR1.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMPQR1.FillerMetalOther = TB_FillerMetalOther.Text.Trim();
        wPQMPQR1.MetalOther = TB_MetalOther.Text.Trim();
        wPQMPQR1.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMPQR1BLL.AddWPQMPQR1(wPQMPQR1);
            lbl_ID.Text = GetMaxWPQMPQR1ID(wPQMPQR1).ToString();
            UpdateWPQMWeldProQuaData(wPQMPQR1.WeldProCode.Trim());
            LoadWPQMPQR1List();

            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZCG+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXZSBJC+"')", true);
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
            wPQMWeldProQua.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
            wPQMWeldProQua.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
            
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }

    protected int GetMaxWPQMPQR1ID(WPQMPQR1 bmbp)
    {
        string strHQL = "Select ID From T_WPQMPQR1 where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR1").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMPQR1(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMPQR1 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMPQR1 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR1").Tables[0];
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSHJGYPDWBXJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMPQR1(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGPQR1YCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMPQR1 as wPQMPQR1 where wPQMPQR1.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMPQR1BLL wPQMPQR1BLL = new WPQMPQR1BLL();
        IList lst = wPQMPQR1BLL.GetAllWPQMPQR1s(strHQL);

        WPQMPQR1 wPQMPQR1 = (WPQMPQR1)lst[0];

        wPQMPQR1.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMPQR1.SecurityMeasureOther = TB_SecurityMeasureOther.Text.Trim();
        wPQMPQR1.ElecCharaOther = TB_ElecCharaOther.Text.Trim();
        wPQMPQR1.WeldJointOther = TB_WeldJointOther.Text.Trim();
        wPQMPQR1.WeldMetalThick = TB_WeldMetalThick.Text.Trim();
        wPQMPQR1.ArcVoltage = TB_ArcVoltage.Text.Trim();
        wPQMPQR1.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMPQR1.FillerMetalOther = TB_FillerMetalOther.Text.Trim();
        wPQMPQR1.MetalOther = TB_MetalOther.Text.Trim();
        wPQMPQR1.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMPQR1BLL.UpdateWPQMPQR1(wPQMPQR1, wPQMPQR1.ID);
            UpdateWPQMWeldProQuaData(wPQMPQR1.WeldProCode.Trim());
            LoadWPQMPQR1List();

            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCSBJC+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = lbl_ID.Text.Trim();

        strHQL = "Delete From T_WPQMPQR1 Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMPQR1List();

            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSBJC+"')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWPQMPQR1List();
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

            strHQL = "From WPQMPQR1 as wPQMPQR1 where wPQMPQR1.ID = '" + strId + "'";
            WPQMPQR1BLL wPQMPQR1BLL = new WPQMPQR1BLL();
            lst = wPQMPQR1BLL.GetAllWPQMPQR1s(strHQL);

            WPQMPQR1 wPQMPQR1 = (WPQMPQR1)lst[0];

            lbl_ID.Text = wPQMPQR1.ID.ToString();
            TB_WeldingCurrent.Text = wPQMPQR1.WeldingCurrent.Trim();
            TB_SecurityMeasureOther.Text = wPQMPQR1.SecurityMeasureOther.Trim();
            TB_ElecCharaOther.Text = wPQMPQR1.ElecCharaOther.Trim();
            TB_WeldJointOther.Text = wPQMPQR1.WeldJointOther.Trim();
            TB_WeldMetalThick.Text = wPQMPQR1.WeldMetalThick.Trim();
            TB_ArcVoltage.Text = wPQMPQR1.ArcVoltage.Trim();
            TB_WeldingSpeed.Text = wPQMPQR1.WeldingSpeed.Trim();
            TB_FillerMetalOther.Text = wPQMPQR1.FillerMetalOther.Trim();
            TB_MetalOther.Text = wPQMPQR1.MetalOther.Trim();
            DL_WeldProCode.SelectedValue = wPQMPQR1.WeldProCode.Trim();

            if (wPQMPQR1.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR1");
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

            TB_WeldingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingSpeed) ? "" : wPQMWeldProQua.WeldingSpeed.Trim();
            TB_WeldingCurrent.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingCurrent) ? "" : wPQMWeldProQua.WeldingCurrent.Trim();
        }
        else
        {
            TB_WeldingSpeed.Text = "";
            TB_WeldingCurrent.Text = "";
        }
    }
}