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

public partial class TTWPQMHeatTreatReport : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","热处理报告", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_HeatTreatTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            TB_HeatTreatReporter.Text = ShareClass.GetUserName(strUserCode.Trim());
            TB_HeatTreatRepOperation.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadWPQMWeldProQuaName();
            LoadWPQMHeatTreatReportList();
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

    protected void LoadWPQMHeatTreatReportList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMHeatTreatReport Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or BoilingTemp like '%" + TextBox1.Text.Trim() + "%' or CoolingMethod like '%" + TextBox1.Text.Trim() + "%' or " +
                "HeatingSpeed like '%" + TextBox1.Text.Trim() + "%' or CoolingSpeed like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-HeatTreatTime::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-HeatTreatTime::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatReport");

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJGYPDWBXJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMHeatTreatReport(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGRCLBGYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMHeatTreatReportBLL wPQMHeatTreatReportBLL = new WPQMHeatTreatReportBLL();
        WPQMHeatTreatReport wPQMHeatTreatReport = new WPQMHeatTreatReport();

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
            wPQMHeatTreatReport.TimeCurvePath = "";
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            wPQMHeatTreatReport.TimeCurvePath = strAttach;
        }

        wPQMHeatTreatReport.HeatTreatTime = DateTime.Parse(string.IsNullOrEmpty(DLC_HeatTreatTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DLC_HeatTreatTime.Text.Trim());
        wPQMHeatTreatReport.HeatTreatReporter = TB_HeatTreatReporter.Text.Trim();
        wPQMHeatTreatReport.Remark = TB_Remark.Text.Trim();
        wPQMHeatTreatReport.EnterCode = strUserCode.Trim();
        wPQMHeatTreatReport.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMHeatTreatReport.BoilingTemp = TB_BoilingTemp.Text.Trim();
        wPQMHeatTreatReport.CoolingMethod = TB_CoolingMethod.Text.Trim();
        wPQMHeatTreatReport.CoolingSpeed = TB_CoolingSpeed.Text.Trim();
        wPQMHeatTreatReport.HeatingSpeed = TB_HeatingSpeed.Text.Trim();
        wPQMHeatTreatReport.HeatTreatRepOperation = TB_HeatTreatRepOperation.Text.Trim();
        wPQMHeatTreatReport.HeatTreatReportReviewer = TB_HeatTreatReportReviewer.Text.Trim();

        try
        {
            wPQMHeatTreatReportBLL.AddWPQMHeatTreatReport(wPQMHeatTreatReport);
            lbl_ID.Text = GetMaxWPQMHeatTreatReportID(wPQMHeatTreatReport).ToString();
            UpdateWPQMWeldProQuaData(wPQMHeatTreatReport.WeldProCode.Trim());
            LoadWPQMHeatTreatReportList();

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

    protected int GetMaxWPQMHeatTreatReportID(WPQMHeatTreatReport bmbp)
    {
        string strHQL = "Select ID From T_WPQMHeatTreatReport where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatReport").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMHeatTreatReport(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMHeatTreatReport Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMHeatTreatReport Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatReport").Tables[0];
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
        if (IsWPQMHeatTreatReport(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGRCLBGYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMHeatTreatReport as wPQMHeatTreatReport where wPQMHeatTreatReport.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMHeatTreatReportBLL wPQMHeatTreatReportBLL = new WPQMHeatTreatReportBLL();
        IList lst = wPQMHeatTreatReportBLL.GetAllWPQMHeatTreatReports(strHQL);
        WPQMHeatTreatReport wPQMHeatTreatReport = (WPQMHeatTreatReport)lst[0];

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            wPQMHeatTreatReport.TimeCurvePath = strAttach;
        }

        wPQMHeatTreatReport.HeatTreatTime = DateTime.Parse(string.IsNullOrEmpty(DLC_HeatTreatTime.Text) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DLC_HeatTreatTime.Text.Trim());
        wPQMHeatTreatReport.HeatTreatReporter = TB_HeatTreatReporter.Text.Trim();
        wPQMHeatTreatReport.Remark = TB_Remark.Text.Trim();
        wPQMHeatTreatReport.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMHeatTreatReport.BoilingTemp = TB_BoilingTemp.Text.Trim();
        wPQMHeatTreatReport.CoolingMethod = TB_CoolingMethod.Text.Trim();
        wPQMHeatTreatReport.CoolingSpeed = TB_CoolingSpeed.Text.Trim();
        wPQMHeatTreatReport.HeatingSpeed = TB_HeatingSpeed.Text.Trim();
        wPQMHeatTreatReport.HeatTreatRepOperation = TB_HeatTreatRepOperation.Text.Trim();
        wPQMHeatTreatReport.HeatTreatReportReviewer = TB_HeatTreatReportReviewer.Text.Trim();

        try
        {
            wPQMHeatTreatReportBLL.UpdateWPQMHeatTreatReport(wPQMHeatTreatReport, wPQMHeatTreatReport.ID);
            UpdateWPQMWeldProQuaData(wPQMHeatTreatReport.WeldProCode.Trim());
            LoadWPQMHeatTreatReportList();

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
        string strHQL = "Delete From T_WPQMHeatTreatReport Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMHeatTreatReportList();
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
        LoadWPQMHeatTreatReportList();
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
            strHQL = "From WPQMHeatTreatReport as wPQMHeatTreatReport where wPQMHeatTreatReport.ID = '" + strId + "'";
            WPQMHeatTreatReportBLL wPQMHeatTreatReportBLL = new WPQMHeatTreatReportBLL();
            lst = wPQMHeatTreatReportBLL.GetAllWPQMHeatTreatReports(strHQL);
            WPQMHeatTreatReport wPQMHeatTreatReport = (WPQMHeatTreatReport)lst[0];
            TB_HeatTreatReporter.Text = wPQMHeatTreatReport.HeatTreatReporter.Trim();
            TB_Remark.Text = wPQMHeatTreatReport.Remark.Trim();
            DL_WeldProCode.SelectedValue = wPQMHeatTreatReport.WeldProCode.Trim();
            DLC_HeatTreatTime.Text = wPQMHeatTreatReport.HeatTreatTime.ToString("yyyy-MM-dd HH:mm:ss");
            lbl_ID.Text = wPQMHeatTreatReport.ID.ToString();
            TB_BoilingTemp.Text = wPQMHeatTreatReport.BoilingTemp.Trim();
            TB_CoolingMethod.Text = wPQMHeatTreatReport.CoolingMethod.Trim();
            TB_CoolingSpeed.Text = wPQMHeatTreatReport.CoolingSpeed.Trim();
            TB_HeatingSpeed.Text = wPQMHeatTreatReport.HeatingSpeed.Trim();
            TB_HeatTreatRepOperation.Text = wPQMHeatTreatReport.HeatTreatRepOperation.Trim();
            TB_HeatTreatReportReviewer.Text = wPQMHeatTreatReport.HeatTreatReportReviewer.Trim();

            if (wPQMHeatTreatReport.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMHeatTreatReport");
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
            TB_CoolingMethod.Text = string.IsNullOrEmpty(wPQMWeldProQua.CoolingMethod) ? "" : wPQMWeldProQua.CoolingMethod.Trim().Substring(wPQMWeldProQua.CoolingMethod.Trim().IndexOf("-") + 1);
            TB_CoolingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.CoolingSpeed) ? "" : wPQMWeldProQua.CoolingSpeed.Trim();
            TB_HeatingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.HeatingSpeed) ? "" : wPQMWeldProQua.HeatingSpeed.Trim();
        }
        else
        {
            TB_BoilingTemp.Text = "";
            TB_CoolingMethod.Text = "";
            TB_CoolingSpeed.Text = "";
            TB_HeatingSpeed.Text = "";
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
            wPQMWeldProQua.HeatingSpeed = TB_HeatingSpeed.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }
}