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

public partial class TTWPQMPQR3 : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","PQR-3№ЬАн", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_UpDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWPQMWeldProQuaName();
            LoadWPQMPQR3List();
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

    protected void LoadWPQMPQR3List()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMPQR3 Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or RTRepNumber like '%" + TextBox1.Text.Trim() + "%' or RTInsResult like '%" + TextBox1.Text.Trim() + "%' " +
            "or MTRepNumber like '%" + TextBox1.Text.Trim() + "%' or MTInsResult like '%" + TextBox1.Text.Trim() + "%' or UTRepNumber like '%" + TextBox1.Text.Trim() + "%' or " +
            "UTInsResult like '%" + TextBox1.Text.Trim() + "%' or PTRepNumber like '%" + TextBox1.Text.Trim() + "%' or PTInsResult like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR3");

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
        if (IsWPQMPQR3(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPQR3YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMPQR3BLL wPQMPQR3BLL = new WPQMPQR3BLL();
        WPQMPQR3 wPQMPQR3 = new WPQMPQR3();

        wPQMPQR3.EnterCode = strUserCode.Trim();
        wPQMPQR3.MetallographicWeld = TB_MetallographicWeld.Text.Trim();
        wPQMPQR3.CheComp_C = TB_CheComp_C.Text.Trim();
        wPQMPQR3.AdditionalIns = TB_AdditionalIns.Text.Trim();
        wPQMPQR3.Conclusion = TB_Conclusion.Text.Trim();
        wPQMPQR3.MetallographicRoot = TB_MetallographicRoot.Text.Trim();
        wPQMPQR3.MetallographicZone = TB_MetallographicZone.Text.Trim();
        wPQMPQR3.CheComTestRepNumber = TB_CheComTestRepNumber.Text.Trim();
        wPQMPQR3.EvaluationResult = TB_EvaluationResult.Text.Trim();
        wPQMPQR3.UTInsResult = TB_UTInsResult.Text.Trim();
        wPQMPQR3.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPQR3.RTInsResult = TB_RTInsResult.Text.Trim();
        wPQMPQR3.MTInsResult = TB_MTInsResult.Text.Trim();
        wPQMPQR3.InsSecIVPoor = TB_InsSecIVPoor.Text.Trim();
        wPQMPQR3.InsSecIIIPoor = TB_InsSecIIIPoor.Text.Trim();
        wPQMPQR3.InsSecIIPoor = TB_InsSecIIPoor.Text.Trim();
        wPQMPQR3.InsSecIPoor = TB_InsSecIPoor.Text.Trim();
        wPQMPQR3.CheComp_Mn = TB_CheComp_Mn.Text.Trim();
        wPQMPQR3.WeldCode = TB_WeldCode.Text.Trim();
        wPQMPQR3.WeldName = TB_WeldName.Text.Trim();
        wPQMPQR3.UTRepNumber = TB_UTRepNumber.Text.Trim();
        wPQMPQR3.RTRepNumber = TB_RTRepNumber.Text.Trim();
        wPQMPQR3.PTRepNumber = TB_PTRepNumber.Text.Trim();
        wPQMPQR3.MTRepNumber = TB_MTRepNumber.Text.Trim();
        wPQMPQR3.InsSecVPoor = TB_InsSecVPoor.Text.Trim();
        wPQMPQR3.UpDateTime = DateTime.Parse(string.IsNullOrEmpty(DLC_UpDateTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_UpDateTime.Text.Trim());
        wPQMPQR3.CheComp_P = TB_CheComp_P.Text.Trim();
        wPQMPQR3.CheComp_S = TB_CheComp_S.Text.Trim();
        wPQMPQR3.CheComp_Si = TB_CheComp_Si.Text.Trim();
        wPQMPQR3.CheComp_Cr = TB_CheComp_Cr.Text.Trim();
        wPQMPQR3.CheComp_Cu = TB_CheComp_Cu.Text.Trim();
        wPQMPQR3.CheComp_Mo = TB_CheComp_Mo.Text.Trim();
        wPQMPQR3.CheComp_Nb = TB_CheComp_Nb.Text.Trim();
        wPQMPQR3.CheComp_Ni = TB_CheComp_Ni.Text.Trim();
        wPQMPQR3.CheComp_Ti = TB_CheComp_Ti.Text.Trim();
        wPQMPQR3.PTInsResult = TB_PTInsResult.Text.Trim();
        wPQMPQR3.SurfaceDistance = TB_SurfaceDistance.Text.Trim();

        try
        {
            wPQMPQR3BLL.AddWPQMPQR3(wPQMPQR3);
            lbl_ID.Text = GetMaxWPQMPQR3ID(wPQMPQR3).ToString();

            LoadWPQMPQR3List();

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

    protected int GetMaxWPQMPQR3ID(WPQMPQR3 bmbp)
    {
        string strHQL = "Select ID From T_WPQMPQR3 where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR3").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMPQR3(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMPQR3 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMPQR3 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR3").Tables[0];
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
        if (IsWPQMPQR3(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPQR3YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMPQR3 as wPQMPQR3 where wPQMPQR3.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMPQR3BLL wPQMPQR3BLL = new WPQMPQR3BLL();
        IList lst = wPQMPQR3BLL.GetAllWPQMPQR3s(strHQL);

        WPQMPQR3 wPQMPQR3 = (WPQMPQR3)lst[0];

        wPQMPQR3.MetallographicWeld = TB_MetallographicWeld.Text.Trim();
        wPQMPQR3.CheComp_C = TB_CheComp_C.Text.Trim();
        wPQMPQR3.AdditionalIns = TB_AdditionalIns.Text.Trim();
        wPQMPQR3.Conclusion = TB_Conclusion.Text.Trim();
        wPQMPQR3.MetallographicRoot = TB_MetallographicRoot.Text.Trim();
        wPQMPQR3.MetallographicZone = TB_MetallographicZone.Text.Trim();
        wPQMPQR3.CheComTestRepNumber = TB_CheComTestRepNumber.Text.Trim();
        wPQMPQR3.EvaluationResult = TB_EvaluationResult.Text.Trim();
        wPQMPQR3.UTInsResult = TB_UTInsResult.Text.Trim();
        wPQMPQR3.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPQR3.RTInsResult = TB_RTInsResult.Text.Trim();
        wPQMPQR3.MTInsResult = TB_MTInsResult.Text.Trim();
        wPQMPQR3.InsSecIVPoor = TB_InsSecIVPoor.Text.Trim();
        wPQMPQR3.InsSecIIIPoor = TB_InsSecIIIPoor.Text.Trim();
        wPQMPQR3.InsSecIIPoor = TB_InsSecIIPoor.Text.Trim();
        wPQMPQR3.InsSecIPoor = TB_InsSecIPoor.Text.Trim();
        wPQMPQR3.CheComp_Mn = TB_CheComp_Mn.Text.Trim();
        wPQMPQR3.WeldCode = TB_WeldCode.Text.Trim();
        wPQMPQR3.WeldName = TB_WeldName.Text.Trim();
        wPQMPQR3.UTRepNumber = TB_UTRepNumber.Text.Trim();
        wPQMPQR3.RTRepNumber = TB_RTRepNumber.Text.Trim();
        wPQMPQR3.PTRepNumber = TB_PTRepNumber.Text.Trim();
        wPQMPQR3.MTRepNumber = TB_MTRepNumber.Text.Trim();
        wPQMPQR3.InsSecVPoor = TB_InsSecVPoor.Text.Trim();
        wPQMPQR3.UpDateTime = DateTime.Parse(string.IsNullOrEmpty(DLC_UpDateTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_UpDateTime.Text.Trim());
        wPQMPQR3.CheComp_P = TB_CheComp_P.Text.Trim();
        wPQMPQR3.CheComp_S = TB_CheComp_S.Text.Trim();
        wPQMPQR3.CheComp_Si = TB_CheComp_Si.Text.Trim();
        wPQMPQR3.CheComp_Cr = TB_CheComp_Cr.Text.Trim();
        wPQMPQR3.CheComp_Cu = TB_CheComp_Cu.Text.Trim();
        wPQMPQR3.CheComp_Mo = TB_CheComp_Mo.Text.Trim();
        wPQMPQR3.CheComp_Nb = TB_CheComp_Nb.Text.Trim();
        wPQMPQR3.CheComp_Ni = TB_CheComp_Ni.Text.Trim();
        wPQMPQR3.CheComp_Ti = TB_CheComp_Ti.Text.Trim();
        wPQMPQR3.PTInsResult = TB_PTInsResult.Text.Trim();
        wPQMPQR3.SurfaceDistance = TB_SurfaceDistance.Text.Trim();

        try
        {
            wPQMPQR3BLL.UpdateWPQMPQR3(wPQMPQR3, wPQMPQR3.ID);

            LoadWPQMPQR3List();

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
        string strHQL;
        string strCode = lbl_ID.Text.Trim();

        strHQL = "Delete From T_WPQMPQR3 Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMPQR3List();

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
        LoadWPQMPQR3List();
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

            strHQL = "From WPQMPQR3 as wPQMPQR3 where wPQMPQR3.ID = '" + strId + "'";
            WPQMPQR3BLL wPQMPQR3BLL = new WPQMPQR3BLL();
            lst = wPQMPQR3BLL.GetAllWPQMPQR3s(strHQL);

            WPQMPQR3 wPQMPQR3 = (WPQMPQR3)lst[0];

            lbl_ID.Text = wPQMPQR3.ID.ToString();
            TB_RTInsResult.Text = wPQMPQR3.RTInsResult.Trim();
            TB_MTInsResult.Text = wPQMPQR3.MTInsResult.Trim();
            TB_InsSecIVPoor.Text = wPQMPQR3.InsSecIVPoor.Trim();
            TB_InsSecIIIPoor.Text = wPQMPQR3.InsSecIIIPoor.Trim();
            TB_EvaluationResult.Text = wPQMPQR3.EvaluationResult.Trim();
            TB_InsSecIIPoor.Text = wPQMPQR3.InsSecIIPoor.Trim();
            TB_InsSecIPoor.Text = wPQMPQR3.InsSecIPoor.Trim();
            TB_CheComp_Mn.Text = wPQMPQR3.CheComp_Mn.Trim();
            TB_WeldCode.Text = wPQMPQR3.WeldCode.Trim();
            TB_WeldName.Text = wPQMPQR3.WeldName.Trim();
            TB_UTRepNumber.Text = wPQMPQR3.UTRepNumber.Trim();
            TB_UTInsResult.Text = wPQMPQR3.UTInsResult.Trim();
            TB_RTRepNumber.Text = wPQMPQR3.RTRepNumber.Trim();
            TB_PTRepNumber.Text = wPQMPQR3.PTRepNumber.Trim();
            TB_MTRepNumber.Text = wPQMPQR3.MTRepNumber.Trim();
            TB_InsSecVPoor.Text = wPQMPQR3.InsSecVPoor.Trim();
            TB_AdditionalIns.Text = wPQMPQR3.AdditionalIns.Trim();
            DLC_UpDateTime.Text = wPQMPQR3.UpDateTime.ToString("yyyy-MM-dd");
            TB_CheComp_P.Text = wPQMPQR3.CheComp_P.Trim();
            TB_CheComp_S.Text = wPQMPQR3.CheComp_S.Trim();
            TB_MetallographicRoot.Text = wPQMPQR3.MetallographicRoot.Trim();
            TB_CheComTestRepNumber.Text = wPQMPQR3.CheComTestRepNumber.Trim();
            TB_CheComp_C.Text = wPQMPQR3.CheComp_C.Trim();
            TB_Conclusion.Text = wPQMPQR3.Conclusion.Trim();
            TB_CheComp_Si.Text = wPQMPQR3.CheComp_Si.Trim();
            TB_MetallographicZone.Text = wPQMPQR3.MetallographicZone.Trim();
            TB_MetallographicWeld.Text = wPQMPQR3.MetallographicWeld.Trim();
            DL_WeldProCode.SelectedValue = wPQMPQR3.WeldProCode.Trim();
            TB_CheComp_Cr.Text=wPQMPQR3.CheComp_Cr.Trim();
            TB_CheComp_Cu.Text=wPQMPQR3.CheComp_Cu.Trim();
            TB_CheComp_Mo.Text = wPQMPQR3.CheComp_Mo.Trim();
            TB_CheComp_Nb.Text = wPQMPQR3.CheComp_Nb.Trim();
            TB_CheComp_Ni.Text = wPQMPQR3.CheComp_Ni.Trim();
            TB_CheComp_Ti.Text = wPQMPQR3.CheComp_Ti.Trim();
            TB_PTInsResult.Text = wPQMPQR3.PTInsResult.Trim();
            TB_SurfaceDistance.Text = wPQMPQR3.SurfaceDistance.Trim();

            if (wPQMPQR3.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR3");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}