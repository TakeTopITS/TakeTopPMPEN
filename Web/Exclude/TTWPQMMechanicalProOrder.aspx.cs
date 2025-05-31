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

public partial class TTWPQMMechanicalProOrder : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","机械加工委托单", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_ProCommDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWPQMWeldProQuaName();
            LoadWPQMMechanicalProOrderList();
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

    protected void LoadWPQMMechanicalProOrderList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMMechanicalProOrder Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or MacComSpeNumber like '%" + TextBox1.Text.Trim() + "%' or " +
                "MachiningProject like '%" + TextBox1.Text.Trim() + "%' or MachanicalProInstro like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-ProCommDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-ProCommDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMMechanicalProOrder");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected string UploadAttach()
    {
        //上传附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Images\\";

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                return "1";
            }
            else
            {
                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                    return "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Images\\" + strFileName3;
                }
                catch
                {
                    return "2";
                }
            }
        }
        else
        {
            return "0";
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSHJGYPDWBXJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMMechanicalProOrder(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGJXJGWTDYCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMMechanicalProOrderBLL wPQMMechanicalProOrderBLL = new WPQMMechanicalProOrderBLL();
        WPQMMechanicalProOrder wPQMMechanicalProOrder = new WPQMMechanicalProOrder();

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
            wPQMMechanicalProOrder.MachiningDrawPath = "";
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCZTMWJSCSBGMHZSC+"')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCSBJC+"')", true);
            return;
        }
        else
        {
            wPQMMechanicalProOrder.MachiningDrawPath = strAttach;
        }

        wPQMMechanicalProOrder.ProCommDate = DateTime.Parse(string.IsNullOrEmpty(DLC_ProCommDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_ProCommDate.Text.Trim());
        wPQMMechanicalProOrder.MachiningProject = TB_MachiningProject.Text.Trim();
        wPQMMechanicalProOrder.MachanicalProInstro = TB_MachanicalProInstro.Text.Trim();
        wPQMMechanicalProOrder.EnterCode = strUserCode.Trim();
        wPQMMechanicalProOrder.MacComSpeNumber = TB_MacComSpeNumber.Text.Trim();
        wPQMMechanicalProOrder.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMMechanicalProOrderBLL.AddWPQMMechanicalProOrder(wPQMMechanicalProOrder);
            lbl_ID.Text = GetMaxWPQMMechanicalProOrderID(wPQMMechanicalProOrder).ToString();
            UpdateWPQMWeldProQuaData(wPQMMechanicalProOrder.WeldProCode.Trim());
            LoadWPQMMechanicalProOrderList();

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

    protected int GetMaxWPQMMechanicalProOrderID(WPQMMechanicalProOrder bmbp)
    {
        string strHQL = "Select ID From T_WPQMMechanicalProOrder where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMMechanicalProOrder").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMMechanicalProOrder(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMMechanicalProOrder Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMMechanicalProOrder Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMMechanicalProOrder").Tables[0];
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
        if (IsWPQMMechanicalProOrder(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGJXJGWTDYCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMMechanicalProOrder as wPQMMechanicalProOrder where wPQMMechanicalProOrder.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMMechanicalProOrderBLL wPQMMechanicalProOrderBLL = new WPQMMechanicalProOrderBLL();
        IList lst = wPQMMechanicalProOrderBLL.GetAllWPQMMechanicalProOrders(strHQL);
        WPQMMechanicalProOrder wPQMMechanicalProOrder = (WPQMMechanicalProOrder)lst[0];

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCZTMWJSCSBGMHZSC+"')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCSBJC+"')", true);
            return;
        }
        else
        {
            wPQMMechanicalProOrder.MachiningDrawPath = strAttach;
        }

        wPQMMechanicalProOrder.ProCommDate = DateTime.Parse(string.IsNullOrEmpty(DLC_ProCommDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_ProCommDate.Text.Trim());
        wPQMMechanicalProOrder.MachiningProject = TB_MachiningProject.Text.Trim();
        wPQMMechanicalProOrder.MachanicalProInstro = TB_MachanicalProInstro.Text.Trim();
        wPQMMechanicalProOrder.MacComSpeNumber = TB_MacComSpeNumber.Text.Trim();
        wPQMMechanicalProOrder.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMMechanicalProOrderBLL.UpdateWPQMMechanicalProOrder(wPQMMechanicalProOrder, wPQMMechanicalProOrder.ID);
            UpdateWPQMWeldProQuaData(wPQMMechanicalProOrder.WeldProCode.Trim());
            LoadWPQMMechanicalProOrderList();

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
        string strID = lbl_ID.Text.Trim();
        string strHQL = "Delete From T_WPQMMechanicalProOrder Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMMechanicalProOrderList();
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
        LoadWPQMMechanicalProOrderList();
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
            strHQL = "From WPQMMechanicalProOrder as wPQMMechanicalProOrder where wPQMMechanicalProOrder.ID = '" + strId + "'";
            WPQMMechanicalProOrderBLL wPQMMechanicalProOrderBLL = new WPQMMechanicalProOrderBLL();
            lst = wPQMMechanicalProOrderBLL.GetAllWPQMMechanicalProOrders(strHQL);
            WPQMMechanicalProOrder wPQMMechanicalProOrder = (WPQMMechanicalProOrder)lst[0];
            TB_MachiningProject.Text = wPQMMechanicalProOrder.MachiningProject.Trim();
            TB_MachanicalProInstro.Text = wPQMMechanicalProOrder.MachanicalProInstro.Trim();
            TB_MacComSpeNumber.Text = wPQMMechanicalProOrder.MacComSpeNumber.Trim();
            DL_WeldProCode.SelectedValue = wPQMMechanicalProOrder.WeldProCode.Trim();
            DLC_ProCommDate.Text = wPQMMechanicalProOrder.ProCommDate.ToString("yyyy-MM-dd");
            lbl_ID.Text = wPQMMechanicalProOrder.ID.ToString();

            if (wPQMMechanicalProOrder.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMMechanicalProOrder");
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
            TB_MacComSpeNumber.Text = string.IsNullOrEmpty(wPQMWeldProQua.NumberSpecimens) ? "" : wPQMWeldProQua.NumberSpecimens.Trim();
        }
        else
        {
            TB_MacComSpeNumber.Text = "";
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
            wPQMWeldProQua.NumberSpecimens = TB_MacComSpeNumber.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }
}