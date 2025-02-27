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

public partial class TTWPQMHeatTreatProCard : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","热处理工艺卡", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            LoadWPQMWeldProQuaName();
            LoadWPQMHeatTreatProCardList();
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

    protected void LoadWPQMHeatTreatProCardList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMHeatTreatProCard Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or HeatTreFurnModel like '%" + TextBox1.Text.Trim() + "%' or BoilingTemp like '%" + TextBox1.Text.Trim() + "%' or HeatingSpeed like '%" + TextBox1.Text.Trim() + "%' " +
            "or CoolingSpeed like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatProCard");

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
        if (IsWPQMHeatTreatProCard(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGRCLGYKYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMHeatTreatProCardBLL wPQMHeatTreatProCardBLL = new WPQMHeatTreatProCardBLL();
        WPQMHeatTreatProCard wPQMHeatTreatProCard = new WPQMHeatTreatProCard();
        wPQMHeatTreatProCard.HeatTreFurnModel = TB_HeatTreFurnModel.Text.Trim();
        wPQMHeatTreatProCard.BoilingTemp = TB_BoilingTemp.Text.Trim();
        wPQMHeatTreatProCard.EnterCode = strUserCode.Trim();
        wPQMHeatTreatProCard.HeatingSpeed = TB_HeatingSpeed.Text.Trim();
        wPQMHeatTreatProCard.Remark = TB_Remark.Text.Trim();
        wPQMHeatTreatProCard.CoolingSpeed = TB_CoolingSpeed.Text.Trim();
        wPQMHeatTreatProCard.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMHeatTreatProCardBLL.AddWPQMHeatTreatProCard(wPQMHeatTreatProCard);
            lbl_ID.Text = GetMaxWPQMHeatTreatProCardID(wPQMHeatTreatProCard).ToString();
            UpdateWPQMWeldProQuaData(wPQMHeatTreatProCard.WeldProCode.Trim());
            LoadWPQMHeatTreatProCardList();

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

    protected int GetMaxWPQMHeatTreatProCardID(WPQMHeatTreatProCard bmbp)
    {
        string strHQL = "Select ID From T_WPQMHeatTreatProCard where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatProCard").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMHeatTreatProCard(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMHeatTreatProCard Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMHeatTreatProCard Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatProCard").Tables[0];
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
        if (IsWPQMHeatTreatProCard(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGRCLGYKYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMHeatTreatProCard as wPQMHeatTreatProCard where wPQMHeatTreatProCard.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMHeatTreatProCardBLL wPQMHeatTreatProCardBLL = new WPQMHeatTreatProCardBLL();
        IList lst = wPQMHeatTreatProCardBLL.GetAllWPQMHeatTreatProCards(strHQL);
        WPQMHeatTreatProCard wPQMHeatTreatProCard = (WPQMHeatTreatProCard)lst[0];

        wPQMHeatTreatProCard.HeatTreFurnModel = TB_HeatTreFurnModel.Text.Trim();
        wPQMHeatTreatProCard.BoilingTemp = TB_BoilingTemp.Text.Trim();
        wPQMHeatTreatProCard.HeatingSpeed = TB_HeatingSpeed.Text.Trim();
        wPQMHeatTreatProCard.Remark = TB_Remark.Text.Trim();
        wPQMHeatTreatProCard.CoolingSpeed = TB_CoolingSpeed.Text.Trim();
        wPQMHeatTreatProCard.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMHeatTreatProCardBLL.UpdateWPQMHeatTreatProCard(wPQMHeatTreatProCard, wPQMHeatTreatProCard.ID);
            UpdateWPQMWeldProQuaData(wPQMHeatTreatProCard.WeldProCode.Trim());
            LoadWPQMHeatTreatProCardList();

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
        string strHQL = "Delete From T_WPQMHeatTreatProCard Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMHeatTreatProCardList();
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
        LoadWPQMHeatTreatProCardList();
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
            strHQL = "From WPQMHeatTreatProCard as wPQMHeatTreatProCard where wPQMHeatTreatProCard.ID = '" + strId + "'";
            WPQMHeatTreatProCardBLL wPQMHeatTreatProCardBLL = new WPQMHeatTreatProCardBLL();
            lst = wPQMHeatTreatProCardBLL.GetAllWPQMHeatTreatProCards(strHQL);
            WPQMHeatTreatProCard wPQMHeatTreatProCard = (WPQMHeatTreatProCard)lst[0];
            TB_HeatTreFurnModel.Text = wPQMHeatTreatProCard.HeatTreFurnModel.Trim();
            TB_HeatingSpeed.Text = wPQMHeatTreatProCard.HeatingSpeed.Trim();
            TB_Remark.Text = wPQMHeatTreatProCard.Remark.Trim();
            TB_BoilingTemp.Text = wPQMHeatTreatProCard.BoilingTemp.Trim();
            TB_CoolingSpeed.Text = wPQMHeatTreatProCard.CoolingSpeed.Trim();
            DL_WeldProCode.SelectedValue = wPQMHeatTreatProCard.WeldProCode.Trim();
            lbl_ID.Text = wPQMHeatTreatProCard.ID.ToString();

            if (wPQMHeatTreatProCard.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatProCard");
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
            TB_BoilingTemp.Text = string.IsNullOrEmpty(wPQMWeldProQua.BoilingTemp) ? "" : wPQMWeldProQua.BoilingTemp.Trim();
            TB_CoolingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.CoolingSpeed) ? "" : wPQMWeldProQua.CoolingSpeed.Trim();
            TB_HeatingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.HeatingSpeed) ? "" : wPQMWeldProQua.HeatingSpeed.Trim();
            TB_HeatTreFurnModel.Text = string.IsNullOrEmpty(wPQMWeldProQua.HeatTreFurnModel) ? "" : wPQMWeldProQua.HeatTreFurnModel.Trim();
        }
        else
        {
            TB_BoilingTemp.Text = "";
            TB_CoolingSpeed.Text = "";
            TB_HeatingSpeed.Text = "";
            TB_HeatTreFurnModel.Text = "";
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
            wPQMWeldProQua.BoilingTemp = TB_BoilingTemp.Text.Trim();
            wPQMWeldProQua.CoolingSpeed = TB_CoolingSpeed.Text.Trim();
            wPQMWeldProQua.HeatTreFurnModel = TB_HeatTreFurnModel.Text.Trim();
            wPQMWeldProQua.HeatingSpeed = TB_HeatingSpeed.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }
}