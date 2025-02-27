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

public partial class TTWPQMPWPS2 : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","PWPS-2№ЬАн", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMWeldProQuaName();
            LoadWPQMPWPS2List();
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

    protected void LoadWPQMPWPS2List()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMPWPS2 Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or FilletWeldPosition like '%" + TextBox1.Text.Trim() + "%' or VerticalWeldingDirection like '%" + TextBox1.Text.Trim() + "%' " +
            "or CurrentTypeChara like '%" + TextBox1.Text.Trim() + "%' or WeldingCurrent like '%" + TextBox1.Text.Trim() + "%' or WeldingSpeed like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS2");

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
        if (IsWPQMPWPS2(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPWPS2YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMPWPS2BLL wPQMPWPS2BLL = new WPQMPWPS2BLL();
        WPQMPWPS2 wPQMPWPS2 = new WPQMPWPS2();

        wPQMPWPS2.Polarity = TB_Polarity.Text.Trim();
        wPQMPWPS2.EnterCode = strUserCode.Trim();
        wPQMPWPS2.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMPWPS2.ConductiveMouthWork = TB_ConductiveMouthWork.Text.Trim();
        wPQMPWPS2.Hammer = TB_Hammer.Text.Trim();
        wPQMPWPS2.VerticalWeldingDirection = TB_VerticalWeldingDirection.Text.Trim();
        wPQMPWPS2.FilletWeldPosition = TB_FilletWeldPosition.Text.Trim();
        wPQMPWPS2.WeldCurrentRange = TB_WeldCurrentRange.Text.Trim();
        wPQMPWPS2.CurrentType = TB_CurrentType.Text.Trim();
        wPQMPWPS2.ArcVoltage = TB_ArcVoltage.Text.Trim();
        wPQMPWPS2.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMPWPS2.BeadWeldingLayer = TB_BeadWeldingLayer.Text.Trim();
        wPQMPWPS2.CurrentTypeChara = TB_CurrentTypeChara.Text.Trim();
        wPQMPWPS2.OscillationParameters = TB_OscillationParameters.Text.Trim();
        wPQMPWPS2.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMPWPS2.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPWPS2.PassWeldingType = DL_PassWeldingType.SelectedValue.Trim();
        wPQMPWPS2.SwingType = DL_SwingType.SelectedValue.Trim();
        wPQMPWPS2.WireWeldingType = DL_WireWeldingType.SelectedValue.Trim();

        try
        {
            wPQMPWPS2BLL.AddWPQMPWPS2(wPQMPWPS2);
            lbl_ID.Text = GetMaxWPQMPWPS2ID(wPQMPWPS2).ToString();
            UpdateWPQMWeldProQuaData(wPQMPWPS2.WeldProCode.Trim());
            LoadWPQMPWPS2List();

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

    protected void UpdateWPQMWeldProQuaData(string strCode)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + strCode + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            wPQMWeldProQua.OscillationParameters = TB_OscillationParameters.Text.Trim();
            wPQMWeldProQua.FilletWeldPosition = TB_FilletWeldPosition.Text.Trim();
            wPQMWeldProQua.VerticalWeldingDirection = TB_VerticalWeldingDirection.Text.Trim();
            wPQMWeldProQua.CurrentType = TB_CurrentType.Text.Trim();
            wPQMWeldProQua.Polarity = TB_Polarity.Text.Trim();
            wPQMWeldProQua.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
            wPQMWeldProQua.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
            wPQMWeldProQua.LineEnergy = TB_LineEnergy.Text.Trim();
            wPQMWeldProQua.SwingType = DL_SwingType.SelectedValue.Trim();
            wPQMWeldProQua.PassWeldingType = DL_PassWeldingType.SelectedValue.Trim();
            wPQMWeldProQua.WireWeldingType = DL_WireWeldingType.SelectedValue.Trim();

            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }

    protected int GetMaxWPQMPWPS2ID(WPQMPWPS2 bmbp)
    {
        string strHQL = "Select ID From T_WPQMPWPS2 where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS2").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMPWPS2(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMPWPS2 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMPWPS2 Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS2").Tables[0];
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
        if (IsWPQMPWPS2(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGPWPS2YCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMPWPS2 as wPQMPWPS2 where wPQMPWPS2.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMPWPS2BLL wPQMPWPS2BLL = new WPQMPWPS2BLL();
        IList lst = wPQMPWPS2BLL.GetAllWPQMPWPS2s(strHQL);

        WPQMPWPS2 wPQMPWPS2 = (WPQMPWPS2)lst[0];

        wPQMPWPS2.Polarity = TB_Polarity.Text.Trim();
        wPQMPWPS2.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMPWPS2.ConductiveMouthWork = TB_ConductiveMouthWork.Text.Trim();
        wPQMPWPS2.Hammer = TB_Hammer.Text.Trim();
        wPQMPWPS2.VerticalWeldingDirection = TB_VerticalWeldingDirection.Text.Trim();
        wPQMPWPS2.FilletWeldPosition = TB_FilletWeldPosition.Text.Trim();
        wPQMPWPS2.WeldCurrentRange = TB_WeldCurrentRange.Text.Trim();
        wPQMPWPS2.CurrentType = TB_CurrentType.Text.Trim();
        wPQMPWPS2.ArcVoltage = TB_ArcVoltage.Text.Trim();
        wPQMPWPS2.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMPWPS2.BeadWeldingLayer = TB_BeadWeldingLayer.Text.Trim();
        wPQMPWPS2.CurrentTypeChara = TB_CurrentTypeChara.Text.Trim();
        wPQMPWPS2.OscillationParameters = TB_OscillationParameters.Text.Trim();
        wPQMPWPS2.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMPWPS2.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMPWPS2.PassWeldingType = DL_PassWeldingType.SelectedValue.Trim();
        wPQMPWPS2.SwingType = DL_SwingType.SelectedValue.Trim();
        wPQMPWPS2.WireWeldingType = DL_WireWeldingType.SelectedValue.Trim();

        try
        {
            wPQMPWPS2BLL.UpdateWPQMPWPS2(wPQMPWPS2, wPQMPWPS2.ID);
            UpdateWPQMWeldProQuaData(wPQMPWPS2.WeldProCode.Trim());
            LoadWPQMPWPS2List();

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

        strHQL = "Delete From T_WPQMPWPS2 Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMPWPS2List();

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
        LoadWPQMPWPS2List();
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

            strHQL = "From WPQMPWPS2 as wPQMPWPS2 where wPQMPWPS2.ID = '" + strId + "'";
            WPQMPWPS2BLL wPQMPWPS2BLL = new WPQMPWPS2BLL();
            lst = wPQMPWPS2BLL.GetAllWPQMPWPS2s(strHQL);

            WPQMPWPS2 wPQMPWPS2 = (WPQMPWPS2)lst[0];

            lbl_ID.Text = wPQMPWPS2.ID.ToString();
            TB_Polarity.Text = wPQMPWPS2.Polarity.Trim();
            TB_WeldingCurrent.Text = wPQMPWPS2.WeldingCurrent.Trim();
            TB_ConductiveMouthWork.Text = wPQMPWPS2.ConductiveMouthWork.Trim();
            TB_Hammer.Text = wPQMPWPS2.Hammer.Trim();
            TB_VerticalWeldingDirection.Text = wPQMPWPS2.VerticalWeldingDirection.Trim();
            TB_FilletWeldPosition.Text = wPQMPWPS2.FilletWeldPosition.Trim();
            TB_WeldCurrentRange.Text = wPQMPWPS2.WeldCurrentRange.Trim();
            TB_CurrentType.Text = wPQMPWPS2.CurrentType.Trim();
            TB_ArcVoltage.Text = wPQMPWPS2.ArcVoltage.Trim();
            TB_WeldingSpeed.Text = wPQMPWPS2.WeldingSpeed.Trim();
            TB_BeadWeldingLayer.Text = wPQMPWPS2.BeadWeldingLayer.Trim();
            TB_CurrentTypeChara.Text = wPQMPWPS2.CurrentTypeChara.Trim();
            TB_OscillationParameters.Text = wPQMPWPS2.OscillationParameters.Trim();
            TB_LineEnergy.Text = wPQMPWPS2.LineEnergy.Trim();
            DL_WeldProCode.SelectedValue = wPQMPWPS2.WeldProCode.Trim();
            DL_PassWeldingType.SelectedValue = wPQMPWPS2.PassWeldingType.Trim();
            DL_SwingType.SelectedValue = wPQMPWPS2.SwingType.Trim();
            DL_WireWeldingType.SelectedValue = wPQMPWPS2.WireWeldingType.Trim();

            if (wPQMPWPS2.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMPWPS2");
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

            TB_CurrentType.Text = string.IsNullOrEmpty(wPQMWeldProQua.CurrentType) ? "" : wPQMWeldProQua.CurrentType.Trim();
            TB_FilletWeldPosition.Text = string.IsNullOrEmpty(wPQMWeldProQua.FilletWeldPosition) ? "" : wPQMWeldProQua.FilletWeldPosition.Trim();
            TB_VerticalWeldingDirection.Text = string.IsNullOrEmpty(wPQMWeldProQua.VerticalWeldingDirection) ? "" : wPQMWeldProQua.VerticalWeldingDirection.Trim();
            TB_Polarity.Text = string.IsNullOrEmpty(wPQMWeldProQua.Polarity) ? "" : wPQMWeldProQua.Polarity.Trim();
            TB_WeldingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingSpeed) ? "" : wPQMWeldProQua.WeldingSpeed.Trim();
            TB_WeldingCurrent.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingCurrent) ? "" : wPQMWeldProQua.WeldingCurrent.Trim();
            TB_LineEnergy.Text = string.IsNullOrEmpty(wPQMWeldProQua.LineEnergy) ? "" : wPQMWeldProQua.LineEnergy.Trim();
            DL_PassWeldingType.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.PassWeldingType) ? "PleaseSelect" : wPQMWeldProQua.PassWeldingType.Trim();
            DL_SwingType.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.SwingType) ? "PleaseSelect" : wPQMWeldProQua.SwingType.Trim();
            DL_WireWeldingType.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.WireWeldingType) ? "PleaseSelect" : wPQMWeldProQua.WireWeldingType.Trim();
            TB_OscillationParameters.Text = string.IsNullOrEmpty(wPQMWeldProQua.OscillationParameters) ? "" : wPQMWeldProQua.OscillationParameters.Trim();
        }
        else
        {
            TB_CurrentType.Text = "";
            TB_FilletWeldPosition.Text = "";
            TB_VerticalWeldingDirection.Text = "";
            TB_Polarity.Text = "";
            TB_WeldingSpeed.Text = "";
            TB_WeldingCurrent.Text = "";
            TB_LineEnergy.Text = "";
            TB_OscillationParameters.Text = "";
            DL_PassWeldingType.SelectedValue = "PleaseSelect";
            DL_SwingType.SelectedValue = "PleaseSelect";
            DL_WireWeldingType.SelectedValue = "PleaseSelect";
        }
    }
}