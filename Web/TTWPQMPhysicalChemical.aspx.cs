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

public partial class TTWPQMPhysicalChemical : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","理化管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_ClientTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWPQMWeldProQuaName();
            LoadWPQMPhysicalChemicalList();
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

    protected void LoadWPQMPhysicalChemicalList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMPhysicalChemical Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or ChemicalClient like '%" + TextBox1.Text.Trim() + "%' or PhysicalCheTensileSample like '%" + TextBox1.Text.Trim() + "%' or LateralBending like '%" + TextBox1.Text.Trim() + "%' " +
            "or ColdBendSpecimen like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-ClientTime::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-ClientTime::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPhysicalChemical");

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
        if (IsWPQMPhysicalChemical(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGLHGLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMPhysicalChemicalBLL wPQMPhysicalChemicalBLL = new WPQMPhysicalChemicalBLL();
        WPQMPhysicalChemical wPQMPhysicalChemical = new WPQMPhysicalChemical();
        wPQMPhysicalChemical.ClientTime = DateTime.Parse(string.IsNullOrEmpty(DLC_ClientTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_ClientTime.Text.Trim());
        wPQMPhysicalChemical.ChemicalClient = TB_ChemicalClient.Text.Trim();
        wPQMPhysicalChemical.LateralBending = TB_LateralBending.Text.Trim();
        wPQMPhysicalChemical.LowTemperature = TB_LowTemperature.Text.Trim();
        wPQMPhysicalChemical.InterCorrosionSpecimen = TB_InterCorrosionSpecimen.Text.Trim();
        wPQMPhysicalChemical.ChemicalReviewer = TB_ChemicalReviewer.Text.Trim();
        wPQMPhysicalChemical.MacroMetalloSpecimen = TB_MacroMetalloSpecimen.Text.Trim();
        wPQMPhysicalChemical.EnterCode = strUserCode.Trim();
        wPQMPhysicalChemical.PhysicalCheTensileSample = TB_PhysicalCheTensileSample.Text.Trim();
        wPQMPhysicalChemical.Remark = TB_Remark.Text.Trim();
        wPQMPhysicalChemical.Value_1 = TB_Value_1.Text.Trim();
        wPQMPhysicalChemical.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPhysicalChemical.ColdBendSpecimen = TB_ColdBendSpecimen.Text.Trim();
        wPQMPhysicalChemical.ContentReq = TB_ContentReq.Text.Trim();
        wPQMPhysicalChemical.ContentStandard = TB_ContentStandard.Text.Trim();
        wPQMPhysicalChemical.DValue = TB_DValue.Text.Trim();
        wPQMPhysicalChemical.FilletMeasure = TB_FilletMeasure.Text.Trim();
        wPQMPhysicalChemical.FilletMeasureStandard = TB_FilletMeasureStandard.Text.Trim();
        wPQMPhysicalChemical.GrooveSample = TB_GrooveSample.Text.Trim();
        wPQMPhysicalChemical.GrooveSampleStandard = TB_GrooveSampleStandard.Text.Trim();
        wPQMPhysicalChemical.IntCorrSpeStandard = TB_IntCorrSpeStandard.Text.Trim();
        NB_LowTempImpact.Amount = NB_LowTempMetaImpact.Amount + NB_LowTempWarmImpact.Amount + NB_LowTempWeldImpact.Amount;
        wPQMPhysicalChemical.LowTempImpact = NB_LowTempImpact.Amount;
        wPQMPhysicalChemical.LowTempMetaImpact = NB_LowTempMetaImpact.Amount;
        wPQMPhysicalChemical.LowTempWarmImpact = NB_LowTempWarmImpact.Amount;
        wPQMPhysicalChemical.LowTempWeldImpact = NB_LowTempWeldImpact.Amount;
        wPQMPhysicalChemical.MacMetSpeStandard = TB_MacMetSpeStandard.Text.Trim();
        NB_NormalTemShock.Amount = NB_NormalTemHeatZoneShock.Amount + NB_NormalTemMetaAreaShock.Amount + NB_NormalTemWeldZoneShock.Amount;
        wPQMPhysicalChemical.NormalTemHeatZoneShock = NB_NormalTemHeatZoneShock.Amount;
        wPQMPhysicalChemical.NormalTemMetaAreaShock = NB_NormalTemMetaAreaShock.Amount;
        wPQMPhysicalChemical.NormalTemShock = NB_NormalTemShock.Amount;
        wPQMPhysicalChemical.NormalTemWeldZoneShock = NB_NormalTemWeldZoneShock.Amount;

        try
        {
            wPQMPhysicalChemicalBLL.AddWPQMPhysicalChemical(wPQMPhysicalChemical);
            lbl_ID.Text = GetMaxWPQMPhysicalChemicalID(wPQMPhysicalChemical).ToString();

            LoadWPQMPhysicalChemicalList();

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

    protected int GetMaxWPQMPhysicalChemicalID(WPQMPhysicalChemical bmbp)
    {
        string strHQL = "Select ID From T_WPQMPhysicalChemical where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPhysicalChemical").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMPhysicalChemical(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMPhysicalChemical Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMPhysicalChemical Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPhysicalChemical").Tables[0];
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
        if (IsWPQMPhysicalChemical(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGLHGLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMPhysicalChemical as wPQMPhysicalChemical where wPQMPhysicalChemical.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMPhysicalChemicalBLL wPQMPhysicalChemicalBLL = new WPQMPhysicalChemicalBLL();
        IList lst = wPQMPhysicalChemicalBLL.GetAllWPQMPhysicalChemicals(strHQL);
        WPQMPhysicalChemical wPQMPhysicalChemical = (WPQMPhysicalChemical)lst[0];

        wPQMPhysicalChemical.ClientTime = DateTime.Parse(string.IsNullOrEmpty(DLC_ClientTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_ClientTime.Text.Trim());
        wPQMPhysicalChemical.ChemicalClient = TB_ChemicalClient.Text.Trim();
        wPQMPhysicalChemical.LateralBending = TB_LateralBending.Text.Trim();
        wPQMPhysicalChemical.LowTemperature = TB_LowTemperature.Text.Trim();
        wPQMPhysicalChemical.InterCorrosionSpecimen = TB_InterCorrosionSpecimen.Text.Trim();
        wPQMPhysicalChemical.ChemicalReviewer = TB_ChemicalReviewer.Text.Trim();
        wPQMPhysicalChemical.MacroMetalloSpecimen = TB_MacroMetalloSpecimen.Text.Trim();
        wPQMPhysicalChemical.PhysicalCheTensileSample = TB_PhysicalCheTensileSample.Text.Trim();
        wPQMPhysicalChemical.Remark = TB_Remark.Text.Trim();
        wPQMPhysicalChemical.Value_1 = TB_Value_1.Text.Trim();
        wPQMPhysicalChemical.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPhysicalChemical.ColdBendSpecimen = TB_ColdBendSpecimen.Text.Trim();
        wPQMPhysicalChemical.ContentReq = TB_ContentReq.Text.Trim();
        wPQMPhysicalChemical.ContentStandard = TB_ContentStandard.Text.Trim();
        wPQMPhysicalChemical.DValue = TB_DValue.Text.Trim();
        wPQMPhysicalChemical.FilletMeasure = TB_FilletMeasure.Text.Trim();
        wPQMPhysicalChemical.FilletMeasureStandard = TB_FilletMeasureStandard.Text.Trim();
        wPQMPhysicalChemical.GrooveSample = TB_GrooveSample.Text.Trim();
        wPQMPhysicalChemical.GrooveSampleStandard = TB_GrooveSampleStandard.Text.Trim();
        wPQMPhysicalChemical.IntCorrSpeStandard = TB_IntCorrSpeStandard.Text.Trim();
        NB_LowTempImpact.Amount = NB_LowTempMetaImpact.Amount + NB_LowTempWarmImpact.Amount + NB_LowTempWeldImpact.Amount;
        wPQMPhysicalChemical.LowTempImpact = NB_LowTempImpact.Amount;
        wPQMPhysicalChemical.LowTempMetaImpact = NB_LowTempMetaImpact.Amount;
        wPQMPhysicalChemical.LowTempWarmImpact = NB_LowTempWarmImpact.Amount;
        wPQMPhysicalChemical.LowTempWeldImpact = NB_LowTempWeldImpact.Amount;
        wPQMPhysicalChemical.MacMetSpeStandard = TB_MacMetSpeStandard.Text.Trim();
        NB_NormalTemShock.Amount = NB_NormalTemHeatZoneShock.Amount + NB_NormalTemMetaAreaShock.Amount + NB_NormalTemWeldZoneShock.Amount;
        wPQMPhysicalChemical.NormalTemHeatZoneShock = NB_NormalTemHeatZoneShock.Amount;
        wPQMPhysicalChemical.NormalTemMetaAreaShock = NB_NormalTemMetaAreaShock.Amount;
        wPQMPhysicalChemical.NormalTemShock = NB_NormalTemShock.Amount;
        wPQMPhysicalChemical.NormalTemWeldZoneShock = NB_NormalTemWeldZoneShock.Amount;

        try
        {
            wPQMPhysicalChemicalBLL.UpdateWPQMPhysicalChemical(wPQMPhysicalChemical, wPQMPhysicalChemical.ID);

            LoadWPQMPhysicalChemicalList();

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
        string strHQL = "Delete From T_WPQMPhysicalChemical Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMPhysicalChemicalList();
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
        LoadWPQMPhysicalChemicalList();
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
            strHQL = "From WPQMPhysicalChemical as wPQMPhysicalChemical where wPQMPhysicalChemical.ID = '" + strId + "'";
            WPQMPhysicalChemicalBLL wPQMPhysicalChemicalBLL = new WPQMPhysicalChemicalBLL();
            lst = wPQMPhysicalChemicalBLL.GetAllWPQMPhysicalChemicals(strHQL);
            WPQMPhysicalChemical wPQMPhysicalChemical = (WPQMPhysicalChemical)lst[0];

            TB_ChemicalClient.Text = wPQMPhysicalChemical.ChemicalClient.Trim();
            TB_PhysicalCheTensileSample.Text = wPQMPhysicalChemical.PhysicalCheTensileSample.Trim();
            TB_Remark.Text = wPQMPhysicalChemical.Remark.Trim();
            TB_Value_1.Text = wPQMPhysicalChemical.Value_1.Trim();
            DL_WeldProCode.SelectedValue = wPQMPhysicalChemical.WeldProCode.Trim();
            DLC_ClientTime.Text = wPQMPhysicalChemical.ClientTime.ToString("yyyy-MM-dd");
            TB_LateralBending.Text = wPQMPhysicalChemical.LateralBending.Trim();
            TB_LowTemperature.Text = wPQMPhysicalChemical.LowTemperature.Trim();
            TB_InterCorrosionSpecimen.Text = wPQMPhysicalChemical.InterCorrosionSpecimen.Trim();
            TB_ChemicalReviewer.Text = wPQMPhysicalChemical.ChemicalReviewer.Trim();
            TB_MacroMetalloSpecimen.Text = wPQMPhysicalChemical.MacroMetalloSpecimen.Trim();
            lbl_ID.Text = wPQMPhysicalChemical.ID.ToString();
            TB_ColdBendSpecimen.Text = wPQMPhysicalChemical.ColdBendSpecimen.Trim();
            TB_ContentReq.Text = wPQMPhysicalChemical.ContentReq.Trim();
            TB_ContentStandard.Text = wPQMPhysicalChemical.ContentStandard.Trim();
            TB_DValue.Text = wPQMPhysicalChemical.DValue.Trim();
            TB_FilletMeasure.Text = wPQMPhysicalChemical.FilletMeasure.Trim();
            TB_FilletMeasureStandard.Text = wPQMPhysicalChemical.FilletMeasureStandard.Trim();
            TB_GrooveSample.Text = wPQMPhysicalChemical.GrooveSample.Trim();
            TB_GrooveSampleStandard.Text = wPQMPhysicalChemical.GrooveSampleStandard.Trim();
            TB_IntCorrSpeStandard.Text = wPQMPhysicalChemical.IntCorrSpeStandard.Trim();
            TB_MacMetSpeStandard.Text = wPQMPhysicalChemical.MacMetSpeStandard.Trim();
            NB_LowTempImpact.Amount = wPQMPhysicalChemical.LowTempImpact;
            NB_LowTempMetaImpact.Amount = wPQMPhysicalChemical.LowTempMetaImpact;
            NB_LowTempWarmImpact.Amount = wPQMPhysicalChemical.LowTempWarmImpact;
            NB_LowTempWeldImpact.Amount = wPQMPhysicalChemical.LowTempWeldImpact;
            NB_NormalTemHeatZoneShock.Amount = wPQMPhysicalChemical.NormalTemHeatZoneShock;
            NB_NormalTemMetaAreaShock.Amount = wPQMPhysicalChemical.NormalTemMetaAreaShock;
            NB_NormalTemShock.Amount = wPQMPhysicalChemical.NormalTemShock;
            NB_NormalTemWeldZoneShock.Amount = wPQMPhysicalChemical.NormalTemWeldZoneShock;

            if (wPQMPhysicalChemical.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPhysicalChemical");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}