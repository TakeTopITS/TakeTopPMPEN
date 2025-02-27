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

public partial class TTWPQMStockList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","备料清单管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_StockEntrustDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_StockUnit.Text = ShareClass.GetDepartName(ShareClass.GetDepartCodeFromUserCode(strUserCode.Trim()));

            LoadWPQMWeldProQuaName();
            LoadWPQMStockListList();
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

    protected void LoadWPQMStockListList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMStockList Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or StockClient like '%" + TextBox1.Text.Trim() + "%' or StockUnit like '%" + TextBox1.Text.Trim() + "%' or SpecimenSpecification like '%" + TextBox1.Text.Trim() + "%' " +
            "or SpecimenNumber like '%" + TextBox1.Text.Trim() + "%' or SpecimenPreparationNote like '%" + TextBox1.Text.Trim() + "%' or WeldMaterialQuantity like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-StockEntrustDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-StockEntrustDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMStockList");

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
        if (IsWPQMStockList(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGBLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMStockListBLL wPQMStockListBLL = new WPQMStockListBLL();
        WPQMStockList wPQMStockList = new WPQMStockList();
        wPQMStockList.StockEntrustDate = DateTime.Parse(string.IsNullOrEmpty(DLC_StockEntrustDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_StockEntrustDate.Text.Trim());
        wPQMStockList.StockClient = TB_StockClient.Text.Trim();
        wPQMStockList.WeldMaterialQuantity = TB_WeldMaterialQuantity.Text.Trim();
        wPQMStockList.EnterCode = strUserCode.Trim();
        wPQMStockList.StockUnit = TB_StockUnit.Text.Trim();
        wPQMStockList.SpecimenPreparationNote = TB_SpecimenPreparationNote.Text.Trim();
        wPQMStockList.SpecimenNumber = TB_SpecimenNumber.Text.Trim();
        wPQMStockList.SpecimenSpecification = TB_SpecimenSpecification.Text.Trim();
        wPQMStockList.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMStockListBLL.AddWPQMStockList(wPQMStockList);
            lbl_ID.Text = GetMaxWPQMStockListID(wPQMStockList).ToString();
            UpdateWPQMWeldProQuaData(wPQMStockList.WeldProCode.Trim());
            LoadWPQMStockListList();

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

    protected int GetMaxWPQMStockListID(WPQMStockList bmbp)
    {
        string strHQL = "Select ID From T_WPQMStockList where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMStockList").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMStockList(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMStockList Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMStockList Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMStockList").Tables[0];
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
        if (IsWPQMStockList(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGBLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMStockList as wPQMStockList where wPQMStockList.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMStockListBLL wPQMStockListBLL = new WPQMStockListBLL();
        IList lst = wPQMStockListBLL.GetAllWPQMStockLists(strHQL);
        WPQMStockList wPQMStockList = (WPQMStockList)lst[0];

        wPQMStockList.StockEntrustDate = DateTime.Parse(string.IsNullOrEmpty(DLC_StockEntrustDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_StockEntrustDate.Text.Trim());
        wPQMStockList.StockClient = TB_StockClient.Text.Trim();
        wPQMStockList.WeldMaterialQuantity = TB_WeldMaterialQuantity.Text.Trim();
        wPQMStockList.StockUnit = TB_StockUnit.Text.Trim();
        wPQMStockList.SpecimenPreparationNote = TB_SpecimenPreparationNote.Text.Trim();
        wPQMStockList.SpecimenNumber = TB_SpecimenNumber.Text.Trim();
        wPQMStockList.SpecimenSpecification = TB_SpecimenSpecification.Text.Trim();
        wPQMStockList.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMStockListBLL.UpdateWPQMStockList(wPQMStockList, wPQMStockList.ID);
            UpdateWPQMWeldProQuaData(wPQMStockList.WeldProCode.Trim());
            LoadWPQMStockListList();

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
        string strHQL = "Delete From T_WPQMStockList Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMStockListList();
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
        LoadWPQMStockListList();
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
            strHQL = "From WPQMStockList as wPQMStockList where wPQMStockList.ID = '" + strId + "'";
            WPQMStockListBLL wPQMStockListBLL = new WPQMStockListBLL();
            lst = wPQMStockListBLL.GetAllWPQMStockLists(strHQL);
            WPQMStockList wPQMStockList = (WPQMStockList)lst[0];
            TB_StockClient.Text = wPQMStockList.StockClient.Trim();
            TB_WeldMaterialQuantity.Text = wPQMStockList.WeldMaterialQuantity.Trim();
            TB_StockUnit.Text = wPQMStockList.StockUnit.Trim();
            TB_SpecimenPreparationNote.Text = wPQMStockList.SpecimenPreparationNote.Trim();
            TB_SpecimenNumber.Text = wPQMStockList.SpecimenNumber.Trim();
            TB_SpecimenSpecification.Text = wPQMStockList.SpecimenSpecification.Trim();
            DL_WeldProCode.SelectedValue = wPQMStockList.WeldProCode.Trim();
            DLC_StockEntrustDate.Text = wPQMStockList.StockEntrustDate.ToString("yyyy-MM-dd");
            lbl_ID.Text = wPQMStockList.ID.ToString();

            if (wPQMStockList.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMStockList");
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
            TB_SpecimenNumber.Text = string.IsNullOrEmpty(wPQMWeldProQua.NumberSpecimens) ? "" : wPQMWeldProQua.NumberSpecimens.Trim();
        }
        else
        {
            TB_SpecimenNumber.Text = "";
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
            wPQMWeldProQua.NumberSpecimens = TB_SpecimenNumber.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }
}