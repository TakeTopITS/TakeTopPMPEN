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

public partial class TTWPQMPQR2 : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","PQR-2№ЬАн", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMWeldProQuaName();
            LoadWPQMPQR2List();
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

    protected void LoadWPQMPQR2List()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMPQR2 Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or TensileTestReportNo like '%" + TextBox1.Text.Trim() + "%' or TenSpePartChara like '%" + TextBox1.Text.Trim() + "%' " +
            "or BendTestReportNo like '%" + TextBox1.Text.Trim() + "%' or BendSpeResults like '%" + TextBox1.Text.Trim() + "%' or ImpactTestReportNo like '%" + TextBox1.Text.Trim() + "%' or "+
            "ImpactSampRemark like '%" + TextBox1.Text.Trim() + "%' or OtherTestReportNo like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR2");

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
        if (IsWPQMPQR2(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPQR2YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMPQR2BLL wPQMPQR2BLL = new WPQMPQR2BLL();
        WPQMPQR2 wPQMPQR2 = new WPQMPQR2();

        wPQMPQR2.EnterCode = strUserCode.Trim();
        wPQMPQR2.TenSpeWidth = TB_TenSpeWidth.Text.Trim();
        wPQMPQR2.TenSpeBreLoad = TB_TenSpeBreLoad.Text.Trim();
        wPQMPQR2.OtherExpRemark = TB_OtherExpRemark.Text.Trim();
        wPQMPQR2.TenSpePartChara = TB_TenSpePartChara.Text.Trim();
        wPQMPQR2.TensileTestReportNo = TB_TensileTestReportNo.Text.Trim();
        wPQMPQR2.TenSpeThickness = TB_TenSpeThickness.Text.Trim();
        wPQMPQR2.TenSpeArea = TB_TenSpeArea.Text.Trim();
        wPQMPQR2.BendSpeResults = TB_BendSpeResults.Text.Trim();
        wPQMPQR2.ImpactSampRemark = TB_ImpactSampRemark.Text.Trim();
        wPQMPQR2.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPQR2.AppInsConclusion = TB_AppInsConclusion.Text.Trim();
        wPQMPQR2.AppInsEvaResults = TB_AppInsEvaResults.Text.Trim();
        wPQMPQR2.BendSpeAngle = TB_BendSpeAngle.Text.Trim();
        wPQMPQR2.BendSpeDiameter = TB_BendSpeDiameter.Text.Trim();
        wPQMPQR2.BendSpeThickness = TB_BendSpeThickness.Text.Trim();
        wPQMPQR2.BendSpeType = TB_BendSpeType.Text.Trim();
        wPQMPQR2.BendTestReportNo = TB_BendTestReportNo.Text.Trim();
        wPQMPQR2.ImpactSampExpAmount = TB_ImpactSampExpAmount.Text.Trim();
        wPQMPQR2.ImpactSampFunction = TB_ImpactSampFunction.Text.Trim();
        wPQMPQR2.ImpactSampPosition = TB_ImpactSampPosition.Text.Trim();
        wPQMPQR2.ImpactSampSize = TB_ImpactSampSize.Text.Trim();
        wPQMPQR2.ImpactSampTemperature = TB_ImpactSampTemperature.Text.Trim();
        wPQMPQR2.ImpactSampType = TB_ImpactSampType.Text.Trim();
        wPQMPQR2.ImpactTestReportNo = TB_ImpactTestReportNo.Text.Trim();
        wPQMPQR2.OtherTestName = TB_OtherTestName.Text.Trim();
        wPQMPQR2.OtherTestReportNo = TB_OtherTestReportNo.Text.Trim();
        wPQMPQR2.OtherTestSize = TB_OtherTestSize.Text.Trim();
        wPQMPQR2.TenSpeSheStrength = TB_TenSpeSheStrength.Text.Trim();

        try
        {
            wPQMPQR2BLL.AddWPQMPQR2(wPQMPQR2);
            lbl_ID.Text = GetMaxWPQMPQR2ID(wPQMPQR2).ToString();
           
            LoadWPQMPQR2List();

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

    protected int GetMaxWPQMPQR2ID(WPQMPQR2 bmbp)
    {
        string strHQL = "Select ID From T_WPQMPQR2 where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR2").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMPQR2(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMPQR2 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMPQR2 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR2").Tables[0];
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
        if (IsWPQMPQR2(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPQR2YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMPQR2 as wPQMPQR2 where wPQMPQR2.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMPQR2BLL wPQMPQR2BLL = new WPQMPQR2BLL();
        IList lst = wPQMPQR2BLL.GetAllWPQMPQR2s(strHQL);

        WPQMPQR2 wPQMPQR2 = (WPQMPQR2)lst[0];

        wPQMPQR2.TenSpeWidth = TB_TenSpeWidth.Text.Trim();
        wPQMPQR2.TenSpeBreLoad = TB_TenSpeBreLoad.Text.Trim();
        wPQMPQR2.OtherExpRemark = TB_OtherExpRemark.Text.Trim();
        wPQMPQR2.TenSpePartChara = TB_TenSpePartChara.Text.Trim();
        wPQMPQR2.TensileTestReportNo = TB_TensileTestReportNo.Text.Trim();
        wPQMPQR2.TenSpeThickness = TB_TenSpeThickness.Text.Trim();
        wPQMPQR2.TenSpeArea = TB_TenSpeArea.Text.Trim();
        wPQMPQR2.BendSpeResults = TB_BendSpeResults.Text.Trim();
        wPQMPQR2.ImpactSampRemark = TB_ImpactSampRemark.Text.Trim();
        wPQMPQR2.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPQR2.AppInsConclusion = TB_AppInsConclusion.Text.Trim();
        wPQMPQR2.AppInsEvaResults = TB_AppInsEvaResults.Text.Trim();
        wPQMPQR2.BendSpeAngle = TB_BendSpeAngle.Text.Trim();
        wPQMPQR2.BendSpeDiameter = TB_BendSpeDiameter.Text.Trim();
        wPQMPQR2.BendSpeThickness = TB_BendSpeThickness.Text.Trim();
        wPQMPQR2.BendSpeType = TB_BendSpeType.Text.Trim();
        wPQMPQR2.BendTestReportNo = TB_BendTestReportNo.Text.Trim();
        wPQMPQR2.ImpactSampExpAmount = TB_ImpactSampExpAmount.Text.Trim();
        wPQMPQR2.ImpactSampFunction = TB_ImpactSampFunction.Text.Trim();
        wPQMPQR2.ImpactSampPosition = TB_ImpactSampPosition.Text.Trim();
        wPQMPQR2.ImpactSampSize = TB_ImpactSampSize.Text.Trim();
        wPQMPQR2.ImpactSampTemperature = TB_ImpactSampTemperature.Text.Trim();
        wPQMPQR2.ImpactSampType = TB_ImpactSampType.Text.Trim();
        wPQMPQR2.ImpactTestReportNo = TB_ImpactTestReportNo.Text.Trim();
        wPQMPQR2.OtherTestName = TB_OtherTestName.Text.Trim();
        wPQMPQR2.OtherTestReportNo = TB_OtherTestReportNo.Text.Trim();
        wPQMPQR2.OtherTestSize = TB_OtherTestSize.Text.Trim();
        wPQMPQR2.TenSpeSheStrength = TB_TenSpeSheStrength.Text.Trim();

        try
        {
            wPQMPQR2BLL.UpdateWPQMPQR2(wPQMPQR2, wPQMPQR2.ID);
            
            LoadWPQMPQR2List();

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

        strHQL = "Delete From T_WPQMPQR2 Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMPQR2List();

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
        LoadWPQMPQR2List();
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

            strHQL = "From WPQMPQR2 as wPQMPQR2 where wPQMPQR2.ID = '" + strId + "'";
            WPQMPQR2BLL wPQMPQR2BLL = new WPQMPQR2BLL();
            lst = wPQMPQR2BLL.GetAllWPQMPQR2s(strHQL);

            WPQMPQR2 wPQMPQR2 = (WPQMPQR2)lst[0];

            lbl_ID.Text = wPQMPQR2.ID.ToString();
            TB_AppInsConclusion.Text = wPQMPQR2.AppInsConclusion.Trim();
            TB_AppInsEvaResults.Text = wPQMPQR2.AppInsEvaResults.Trim();
            TB_BendSpeAngle.Text = wPQMPQR2.BendSpeAngle.Trim();
            TB_BendSpeDiameter.Text = wPQMPQR2.BendSpeDiameter.Trim();
            TB_BendSpeResults.Text = wPQMPQR2.BendSpeResults.Trim();
            TB_BendSpeThickness.Text = wPQMPQR2.BendSpeThickness.Trim();
            TB_BendSpeType.Text = wPQMPQR2.BendSpeType.Trim();
            TB_BendTestReportNo.Text = wPQMPQR2.BendTestReportNo.Trim();
            TB_ImpactSampExpAmount.Text = wPQMPQR2.ImpactSampExpAmount.Trim();
            TB_ImpactSampFunction.Text = wPQMPQR2.ImpactSampFunction.Trim();
            TB_ImpactSampPosition.Text = wPQMPQR2.ImpactSampPosition.Trim();
            TB_ImpactSampRemark.Text = wPQMPQR2.ImpactSampRemark.Trim();
            TB_ImpactSampSize.Text = wPQMPQR2.ImpactSampSize.Trim();
            TB_ImpactSampTemperature.Text = wPQMPQR2.ImpactSampTemperature.Trim();
            TB_ImpactSampType.Text = wPQMPQR2.ImpactSampType.Trim();
            TB_ImpactTestReportNo.Text = wPQMPQR2.ImpactTestReportNo.Trim();
            TB_OtherExpRemark.Text = wPQMPQR2.OtherExpRemark.Trim();
            TB_OtherTestName.Text = wPQMPQR2.OtherTestName.Trim();
            TB_OtherTestReportNo.Text = wPQMPQR2.OtherTestReportNo.Trim();
            TB_OtherTestSize.Text = wPQMPQR2.OtherTestSize.Trim();
            TB_TensileTestReportNo.Text = wPQMPQR2.TensileTestReportNo.Trim();
            TB_TenSpeArea.Text = wPQMPQR2.TenSpeArea.Trim();
            TB_TenSpeBreLoad.Text = wPQMPQR2.TenSpeBreLoad.Trim();
            TB_TenSpePartChara.Text = wPQMPQR2.TenSpePartChara.Trim();
            TB_TenSpeSheStrength.Text = wPQMPQR2.TenSpeSheStrength.Trim();
            TB_TenSpeThickness.Text = wPQMPQR2.TenSpeThickness.Trim();
            TB_TenSpeWidth.Text = wPQMPQR2.TenSpeWidth.Trim();
            DL_WeldProCode.SelectedValue = wPQMPQR2.WeldProCode.Trim();

            if (wPQMPQR2.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPQR2");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}