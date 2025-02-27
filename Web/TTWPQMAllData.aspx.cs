using System;
using System.Resources;
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

public partial class TTWPQMAllData : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "工艺基础数据", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMAllDataList();
        }
    }

    protected void LoadWPQMAllDataList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMAllData Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Description like '%" + TextBox1.Text.Trim() + "%' or Type like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMAllData");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Code.Text) || TB_Code.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBMBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_Type.SelectedValue) || DL_Type.SelectedValue.Trim() == "" || DL_Type.SelectedValue.Trim() == "PleaseSelect")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGLXBCZSBJC").ToString().Trim() + "')", true);
            DL_Type.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TB_Description.Text) || TB_Description.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJMSBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Description.Focus();
            return;
        }
        if (DL_Type.SelectedValue.Trim() == "WeldingWireModel" || DL_Type.SelectedValue.Trim() == "WeldingRodModel" || DL_Type.SelectedValue.Trim() == "FluxModel"
            || DL_Type.SelectedValue.Trim() == "WeldingWireBrand" || DL_Type.SelectedValue.Trim() == "WeldingRodBrand" || DL_Type.SelectedValue.Trim() == "FluxBrand"
            || DL_Type.SelectedValue.Trim() == "WeldingWireSpecification" || DL_Type.SelectedValue.Trim() == "WeldingRodSpecification" || DL_Type.SelectedValue.Trim() == "FluxSpecification")
        {
            if (TB_Description.Text.Trim().Contains("-") || TB_Description.Text.Trim().Contains(";"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSSJMSBNBHHZFJC").ToString().Trim() + "')", true);
                TB_Description.Focus();
                return;
            }
        }
        if (IsWPQMAllDataCode(TB_Code.Text.Trim(), DL_Type.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBMJLXZHYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            DL_Type.Focus();
            return;
        }

        WPQMAllDataBLL wPQMAllDataBLL = new WPQMAllDataBLL();
        WPQMAllData wPQMAllData = new WPQMAllData();
        wPQMAllData.Code = TB_Code.Text.Trim();
        wPQMAllData.Description = TB_Description.Text.Trim();
        wPQMAllData.EnterCode = strUserCode.Trim();
        wPQMAllData.Type = DL_Type.SelectedValue.Trim();

        try
        {
            wPQMAllDataBLL.AddWPQMAllData(wPQMAllData);
            lbl_ID.Text = GetWPQMAllDataID(wPQMAllData).ToString();

            BT_Update.Visible = true;
            BT_Delete.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            LoadWPQMAllDataList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected bool IsWPQMAllDataCode(string strCode, string strType, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMAllData Where Code='" + strCode + "' and Type='" + strType + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMAllData Where Code='" + strCode + "' and Type='" + strType + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMAllData").Tables[0];
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

    protected int GetWPQMAllDataID(WPQMAllData bmbp)
    {
        string strHQL = "Select ID From T_WPQMAllData where Type='" + bmbp.Type.Trim() + "' and EnterCode='" + bmbp.EnterCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMAllData").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || TB_Code.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBMBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_Type.SelectedValue.Trim()) || DL_Type.SelectedValue.Trim() == "" || DL_Type.SelectedValue.Trim() == "PleaseSelect")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGLXBCZSBJC").ToString().Trim() + "')", true);
            DL_Type.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TB_Description.Text) || TB_Description.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJMSBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Description.Focus();
            return;
        }
        if (DL_Type.SelectedValue.Trim() == "WeldingWireModel" || DL_Type.SelectedValue.Trim() == "WeldingRodModel" || DL_Type.SelectedValue.Trim() == "FluxModel"
            || DL_Type.SelectedValue.Trim() == "WeldingWireBrand" || DL_Type.SelectedValue.Trim() == "WeldingRodBrand" || DL_Type.SelectedValue.Trim() == "FluxBrand"
            || DL_Type.SelectedValue.Trim() == "WeldingWireSpecification" || DL_Type.SelectedValue.Trim() == "WeldingRodSpecification" || DL_Type.SelectedValue.Trim() == "FluxSpecification")
        {
            if (TB_Description.Text.Trim().Contains("-") || TB_Description.Text.Trim().Contains(";"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSSJMSBNBHHZFJC").ToString().Trim() + "')", true);
                TB_Description.Focus();
                return;
            }
        }
        if (IsWPQMAllDataCode(TB_Code.Text.Trim(), DL_Type.SelectedValue.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBMJLXZHYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            DL_Type.Focus();
            return;
        }

        string strHQL = "From WPQMAllData as wPQMAllData where wPQMAllData.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMAllDataBLL wPQMAllDataBLL = new WPQMAllDataBLL();
        IList lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);
        WPQMAllData wPQMAllData = (WPQMAllData)lst[0];

        wPQMAllData.Code = TB_Code.Text.Trim();
        wPQMAllData.Description = TB_Description.Text.Trim();
        wPQMAllData.Type = DL_Type.SelectedValue.Trim();

        try
        {
            wPQMAllDataBLL.UpdateWPQMAllData(wPQMAllData, wPQMAllData.ID);

            BT_Update.Visible = true;
            BT_Delete.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            LoadWPQMAllDataList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = lbl_ID.Text.Trim();

        strHQL = "Delete From T_WPQMAllData Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            LoadWPQMAllDataList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWPQMAllDataList();
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

            strHQL = "From WPQMAllData as wPQMAllData where wPQMAllData.ID = '" + strId + "'";
            WPQMAllDataBLL wPQMAllDataBLL = new WPQMAllDataBLL();
            lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

            WPQMAllData wPQMAllData = (WPQMAllData)lst[0];
            lbl_ID.Text = wPQMAllData.ID.ToString();
            TB_Code.Text = wPQMAllData.Code.Trim();
            TB_Description.Text = wPQMAllData.Description.Trim();
            DL_Type.SelectedValue = wPQMAllData.Type.Trim();

            if (wPQMAllData.EnterCode.Trim() == strUserCode.Trim())
            {
                BT_Update.Visible = true;
                BT_Delete.Visible = true;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMAllData");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}