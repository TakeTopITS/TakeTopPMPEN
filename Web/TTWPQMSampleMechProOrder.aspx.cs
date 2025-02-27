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

public partial class TTWPQMSampleMechProOrder : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","试样机加工管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_CommissionedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_MachiningDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWPQMWeldProQuaName();
            LoadWPQMSampleMechProOrderList();
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

    protected void LoadWPQMSampleMechProOrderList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMSampleMechProOrder Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or TensileTestSpecimen like '%" + TextBox1.Text.Trim() + "%' or SurfaceBending like '%" + TextBox1.Text.Trim() + "%' or MachiningPrincipal like '%" + TextBox1.Text.Trim() + "%' " +
            "or MachiningInstruction like '%" + TextBox1.Text.Trim() + "%' or WeldZoneImpact like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-CommissionedDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-CommissionedDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMSampleMechProOrder");

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
        if (IsWPQMSampleMechProOrder(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGJJGWTDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMSampleMechProOrderBLL wPQMSampleMechProOrderBLL = new WPQMSampleMechProOrderBLL();
        WPQMSampleMechProOrder wPQMSampleMechProOrder = new WPQMSampleMechProOrder();
        wPQMSampleMechProOrder.CommissionedDate = DateTime.Parse(string.IsNullOrEmpty(DLC_CommissionedDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_CommissionedDate.Text.Trim());
        wPQMSampleMechProOrder.MachiningPrincipal = TB_MachiningPrincipal.Text.Trim();
        wPQMSampleMechProOrder.CurvedBack = TB_CurvedBack.Text.Trim();
        wPQMSampleMechProOrder.CurvedSide = TB_CurvedSide.Text.Trim();
        wPQMSampleMechProOrder.HeatAffectedZone = TB_HeatAffectedZone.Text.Trim();
        wPQMSampleMechProOrder.InterCorrosionSpecimen = TB_InterCorrosionSpecimen.Text.Trim();
        wPQMSampleMechProOrder.MachiningDate = DateTime.Parse(string.IsNullOrEmpty(DLC_MachiningDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_MachiningDate.Text.Trim());
        wPQMSampleMechProOrder.MachiningReviewer = TB_MachiningReviewer.Text.Trim();
        wPQMSampleMechProOrder.MacroMetalloSpecimen = TB_MacroMetalloSpecimen.Text.Trim();
        wPQMSampleMechProOrder.MetalAreaImpact = TB_MetalAreaImpact.Text.Trim();
        wPQMSampleMechProOrder.WeldZoneImpact = TB_WeldZoneImpact.Text.Trim();
        wPQMSampleMechProOrder.EnterCode = strUserCode.Trim();
        wPQMSampleMechProOrder.TensileTestSpecimen = TB_TensileTestSpecimen.Text.Trim();
        wPQMSampleMechProOrder.MachiningInstruction = TB_MachiningInstruction.Text.Trim();
        wPQMSampleMechProOrder.SurfaceBending = TB_SurfaceBending.Text.Trim();
        wPQMSampleMechProOrder.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMSampleMechProOrderBLL.AddWPQMSampleMechProOrder(wPQMSampleMechProOrder);
            lbl_ID.Text = GetMaxWPQMSampleMechProOrderID(wPQMSampleMechProOrder).ToString();
            
            LoadWPQMSampleMechProOrderList();

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

    protected int GetMaxWPQMSampleMechProOrderID(WPQMSampleMechProOrder bmbp)
    {
        string strHQL = "Select ID From T_WPQMSampleMechProOrder where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMSampleMechProOrder").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMSampleMechProOrder(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMSampleMechProOrder Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMSampleMechProOrder Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMSampleMechProOrder").Tables[0];
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
        if (IsWPQMSampleMechProOrder(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGJJGWTDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMSampleMechProOrder as wPQMSampleMechProOrder where wPQMSampleMechProOrder.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMSampleMechProOrderBLL wPQMSampleMechProOrderBLL = new WPQMSampleMechProOrderBLL();
        IList lst = wPQMSampleMechProOrderBLL.GetAllWPQMSampleMechProOrders(strHQL);
        WPQMSampleMechProOrder wPQMSampleMechProOrder = (WPQMSampleMechProOrder)lst[0];

        wPQMSampleMechProOrder.CommissionedDate = DateTime.Parse(string.IsNullOrEmpty(DLC_CommissionedDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_CommissionedDate.Text.Trim());
        wPQMSampleMechProOrder.MachiningPrincipal = TB_MachiningPrincipal.Text.Trim();
        wPQMSampleMechProOrder.CurvedBack = TB_CurvedBack.Text.Trim();
        wPQMSampleMechProOrder.CurvedSide = TB_CurvedSide.Text.Trim();
        wPQMSampleMechProOrder.HeatAffectedZone = TB_HeatAffectedZone.Text.Trim();
        wPQMSampleMechProOrder.InterCorrosionSpecimen = TB_InterCorrosionSpecimen.Text.Trim();
        wPQMSampleMechProOrder.MachiningDate = DateTime.Parse(string.IsNullOrEmpty(DLC_MachiningDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_MachiningDate.Text.Trim());
        wPQMSampleMechProOrder.MachiningReviewer = TB_MachiningReviewer.Text.Trim();
        wPQMSampleMechProOrder.MacroMetalloSpecimen = TB_MacroMetalloSpecimen.Text.Trim();
        wPQMSampleMechProOrder.MetalAreaImpact = TB_MetalAreaImpact.Text.Trim();
        wPQMSampleMechProOrder.WeldZoneImpact = TB_WeldZoneImpact.Text.Trim();
        wPQMSampleMechProOrder.TensileTestSpecimen = TB_TensileTestSpecimen.Text.Trim();
        wPQMSampleMechProOrder.MachiningInstruction = TB_MachiningInstruction.Text.Trim();
        wPQMSampleMechProOrder.SurfaceBending = TB_SurfaceBending.Text.Trim();
        wPQMSampleMechProOrder.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMSampleMechProOrderBLL.UpdateWPQMSampleMechProOrder(wPQMSampleMechProOrder, wPQMSampleMechProOrder.ID);
            
            LoadWPQMSampleMechProOrderList();

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
        string strHQL = "Delete From T_WPQMSampleMechProOrder Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMSampleMechProOrderList();
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
        LoadWPQMSampleMechProOrderList();
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
            strHQL = "From WPQMSampleMechProOrder as wPQMSampleMechProOrder where wPQMSampleMechProOrder.ID = '" + strId + "'";
            WPQMSampleMechProOrderBLL wPQMSampleMechProOrderBLL = new WPQMSampleMechProOrderBLL();
            lst = wPQMSampleMechProOrderBLL.GetAllWPQMSampleMechProOrders(strHQL);
            WPQMSampleMechProOrder wPQMSampleMechProOrder = (WPQMSampleMechProOrder)lst[0];

            TB_MachiningPrincipal.Text = wPQMSampleMechProOrder.MachiningPrincipal.Trim();
            TB_TensileTestSpecimen.Text = wPQMSampleMechProOrder.TensileTestSpecimen.Trim();
            TB_MachiningInstruction.Text = wPQMSampleMechProOrder.MachiningInstruction.Trim();
            TB_SurfaceBending.Text = wPQMSampleMechProOrder.SurfaceBending.Trim();
            DL_WeldProCode.SelectedValue = wPQMSampleMechProOrder.WeldProCode.Trim();
            DLC_CommissionedDate.Text = wPQMSampleMechProOrder.CommissionedDate.ToString("yyyy-MM-dd");
            TB_CurvedBack.Text = wPQMSampleMechProOrder.CurvedBack.Trim();
            TB_CurvedSide.Text = wPQMSampleMechProOrder.CurvedSide.Trim();
            TB_HeatAffectedZone.Text = wPQMSampleMechProOrder.HeatAffectedZone.Trim();
            TB_InterCorrosionSpecimen.Text = wPQMSampleMechProOrder.InterCorrosionSpecimen.Trim();
            TB_MachiningReviewer.Text = wPQMSampleMechProOrder.MachiningReviewer.Trim();
            TB_MacroMetalloSpecimen.Text = wPQMSampleMechProOrder.MacroMetalloSpecimen.Trim();
            TB_MetalAreaImpact.Text = wPQMSampleMechProOrder.MetalAreaImpact.Trim();
            TB_WeldZoneImpact.Text = wPQMSampleMechProOrder.WeldZoneImpact.Trim();
            DLC_MachiningDate.Text = wPQMSampleMechProOrder.MachiningDate.ToString("yyyy-MM-dd");
            lbl_ID.Text = wPQMSampleMechProOrder.ID.ToString();

            if (wPQMSampleMechProOrder.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMSampleMechProOrder");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}