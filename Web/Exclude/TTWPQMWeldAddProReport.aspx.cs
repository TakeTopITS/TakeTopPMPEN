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

public partial class TTWPQMWeldAddProReport : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","工艺评定报告", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_WeldingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWPQMWeldProQuaName();
            LoadWPQMAllDataName();
            LoadWPQMWeldAddProReportList();
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

    protected void LoadWPQMAllDataName()
    {
        string strHQL;
        IList lst;
        WPQMAllDataBLL wPQMAllDataBLL = new WPQMAllDataBLL();
        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='焊后热处理方法' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_AfterHot.DataSource = lst;
        DL_AfterHot.DataBind();
        DL_AfterHot.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMWeldAddProReportList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMWeldAddProReport Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or SpanWidth like '%" + TextBox1.Text.Trim() + "%' or FilletWeldThick like '%" + TextBox1.Text.Trim() + "%' " +
            "or WeldingCurrent like '%" + TextBox1.Text.Trim() + "%' or MetalLiner like '%" + TextBox1.Text.Trim() + "%' or AfterHot like '%" + TextBox1.Text.Trim() + "%' or " +
            "WelderName like '%" + TextBox1.Text.Trim() + "%' or AppInspectionResult like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldAddProReport");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSHJGYPDWBXJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMWeldAddProReport(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGGYPDBGYCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMWeldAddProReportBLL wPQMWeldAddProReportBLL = new WPQMWeldAddProReportBLL();
        WPQMWeldAddProReport wPQMWeldAddProReport = new WPQMWeldAddProReport();

        wPQMWeldAddProReport.AppInsRepNumber = TB_AppInsRepNumber.Text.Trim();
        wPQMWeldAddProReport.FilletWeldThick = TB_FilletWeldThick.Text.Trim();
        wPQMWeldAddProReport.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldAddProReport.CleaningMethod = TB_CleaningMethod.Text.Trim();
        wPQMWeldAddProReport.EnterCode = strUserCode.Trim();
        wPQMWeldAddProReport.MetaFaceNumber_CIF = TB_MetaFaceNumber_CIF.Text.Trim();
        wPQMWeldAddProReport.MetaFaceNumber_FWT = TB_MetaFaceNumber_FWT.Text.Trim();
        wPQMWeldAddProReport.MetaFaceNumber_PEN = TB_MetaFaceNumber_PEN.Text.Trim();
        wPQMWeldAddProReport.AppInspectionResult = TB_AppInspectionResult.Text.Trim();
        wPQMWeldAddProReport.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
        wPQMWeldAddProReport.VerticalWeldingDirection = TB_VerticalWeldingDirection.Text.Trim();
        wPQMWeldAddProReport.MetalLinerShapeSize = TB_MetalLinerShapeSize.Text.Trim();
        wPQMWeldAddProReport.ConnectingWay = TB_ConnectingWay.Text.Trim();
        wPQMWeldAddProReport.Conclusion = TB_Conclusion.Text.Trim();
        wPQMWeldAddProReport.EvaluationResult = TB_EvaluationResult.Text.Trim();
        wPQMWeldAddProReport.SpanWidth = TB_SpanWidth.Text.Trim();
        wPQMWeldAddProReport.MetalLiner = TB_MetalLiner.Text.Trim();
        wPQMWeldAddProReport.WelderName = TB_WelderName.Text.Trim();
        wPQMWeldAddProReport.WelderCode = TB_WelderCode.Text.Trim();
        wPQMWeldAddProReport.WeldingDate = DateTime.Parse(string.IsNullOrEmpty(DLC_WeldingDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_WeldingDate.Text.Trim());
        wPQMWeldAddProReport.PenInsJointNumber = TB_PenInsJointNumber.Text.Trim();
        wPQMWeldAddProReport.PenInsRepNumber = TB_PenInsRepNumber.Text.Trim();
        wPQMWeldAddProReport.MetaInsRepNumber = TB_MetaInsRepNumber.Text.Trim();
        wPQMWeldAddProReport.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMWeldAddProReportBLL.AddWPQMWeldAddProReport(wPQMWeldAddProReport);
            lbl_ID.Text = GetMaxWPQMWeldAddProReportID(wPQMWeldAddProReport).ToString();
            UpdateWPQMWeldProQuaData(wPQMWeldAddProReport.WeldProCode.Trim());
            LoadWPQMWeldAddProReportList();

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

    protected void UpdateWPQMWeldProQuaData(string strCode)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + strCode + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            wPQMWeldProQua.VerticalWeldingDirection = TB_VerticalWeldingDirection.Text.Trim();
            wPQMWeldProQua.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
            wPQMWeldProQua.WeldingCurrent = TB_WeldingCurrent.Text.Trim();

            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }

    protected int GetMaxWPQMWeldAddProReportID(WPQMWeldAddProReport bmbp)
    {
        string strHQL = "Select ID From T_WPQMWeldAddProReport where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldAddProReport").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMWeldAddProReport(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMWeldAddProReport Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMWeldAddProReport Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldAddProReport").Tables[0];
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
        if (IsWPQMWeldAddProReport(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGGYPDBGYCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMWeldAddProReport as wPQMWeldAddProReport where wPQMWeldAddProReport.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMWeldAddProReportBLL wPQMWeldAddProReportBLL = new WPQMWeldAddProReportBLL();
        IList lst = wPQMWeldAddProReportBLL.GetAllWPQMWeldAddProReports(strHQL);

        WPQMWeldAddProReport wPQMWeldAddProReport = (WPQMWeldAddProReport)lst[0];

        wPQMWeldAddProReport.AppInsRepNumber = TB_AppInsRepNumber.Text.Trim();
        wPQMWeldAddProReport.FilletWeldThick = TB_FilletWeldThick.Text.Trim();
        wPQMWeldAddProReport.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldAddProReport.CleaningMethod = TB_CleaningMethod.Text.Trim();
        wPQMWeldAddProReport.MetaFaceNumber_CIF = TB_MetaFaceNumber_CIF.Text.Trim();
        wPQMWeldAddProReport.MetaFaceNumber_FWT = TB_MetaFaceNumber_FWT.Text.Trim();
        wPQMWeldAddProReport.MetaFaceNumber_PEN = TB_MetaFaceNumber_PEN.Text.Trim();
        wPQMWeldAddProReport.AppInspectionResult = TB_AppInspectionResult.Text.Trim();
        wPQMWeldAddProReport.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
        wPQMWeldAddProReport.VerticalWeldingDirection = TB_VerticalWeldingDirection.Text.Trim();
        wPQMWeldAddProReport.MetalLinerShapeSize = TB_MetalLinerShapeSize.Text.Trim();
        wPQMWeldAddProReport.ConnectingWay = TB_ConnectingWay.Text.Trim();
        wPQMWeldAddProReport.Conclusion = TB_Conclusion.Text.Trim();
        wPQMWeldAddProReport.EvaluationResult = TB_EvaluationResult.Text.Trim();
        wPQMWeldAddProReport.SpanWidth = TB_SpanWidth.Text.Trim();
        wPQMWeldAddProReport.MetalLiner = TB_MetalLiner.Text.Trim();
        wPQMWeldAddProReport.WelderName = TB_WelderName.Text.Trim();
        wPQMWeldAddProReport.WelderCode = TB_WelderCode.Text.Trim();
        wPQMWeldAddProReport.WeldingDate = DateTime.Parse(string.IsNullOrEmpty(DLC_WeldingDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_WeldingDate.Text.Trim());
        wPQMWeldAddProReport.PenInsJointNumber = TB_PenInsJointNumber.Text.Trim();
        wPQMWeldAddProReport.PenInsRepNumber = TB_PenInsRepNumber.Text.Trim();
        wPQMWeldAddProReport.MetaInsRepNumber = TB_MetaInsRepNumber.Text.Trim();
        wPQMWeldAddProReport.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMWeldAddProReportBLL.UpdateWPQMWeldAddProReport(wPQMWeldAddProReport, wPQMWeldAddProReport.ID);
            UpdateWPQMWeldProQuaData(wPQMWeldAddProReport.WeldProCode.Trim());
            LoadWPQMWeldAddProReportList();

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
        string strHQL;
        string strCode = lbl_ID.Text.Trim();

        strHQL = "Delete From T_WPQMWeldAddProReport Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMWeldAddProReportList();

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
        LoadWPQMWeldAddProReportList();
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

            strHQL = "From WPQMWeldAddProReport as wPQMWeldAddProReport where wPQMWeldAddProReport.ID = '" + strId + "'";
            WPQMWeldAddProReportBLL wPQMWeldAddProReportBLL = new WPQMWeldAddProReportBLL();
            lst = wPQMWeldAddProReportBLL.GetAllWPQMWeldAddProReports(strHQL);

            WPQMWeldAddProReport wPQMWeldAddProReport = (WPQMWeldAddProReport)lst[0];

            lbl_ID.Text = wPQMWeldAddProReport.ID.ToString();
            TB_AppInsRepNumber.Text = wPQMWeldAddProReport.AppInsRepNumber.Trim();
            TB_FilletWeldThick.Text = wPQMWeldAddProReport.FilletWeldThick.Trim();
            TB_WeldingCurrent.Text = wPQMWeldAddProReport.WeldingCurrent.Trim();
            TB_CleaningMethod.Text = wPQMWeldAddProReport.CleaningMethod.Trim();
            TB_MetaFaceNumber_CIF.Text = wPQMWeldAddProReport.MetaFaceNumber_CIF.Trim();
            TB_MetaFaceNumber_FWT.Text = wPQMWeldAddProReport.MetaFaceNumber_FWT.Trim();
            TB_MetaFaceNumber_PEN.Text = wPQMWeldAddProReport.MetaFaceNumber_PEN.Trim();
            TB_AppInspectionResult.Text = wPQMWeldAddProReport.AppInspectionResult.Trim();
            DL_AfterHot.SelectedValue = wPQMWeldAddProReport.AfterHot.Trim().Substring(0, wPQMWeldAddProReport.AfterHot.Trim().IndexOf("-"));
            TB_VerticalWeldingDirection.Text = wPQMWeldAddProReport.VerticalWeldingDirection.Trim();
            TB_MetalLinerShapeSize.Text = wPQMWeldAddProReport.MetalLinerShapeSize.Trim();
            TB_ConnectingWay.Text = wPQMWeldAddProReport.ConnectingWay.Trim();
            TB_Conclusion.Text = wPQMWeldAddProReport.Conclusion.Trim();
            TB_EvaluationResult.Text = wPQMWeldAddProReport.EvaluationResult.Trim();
            TB_SpanWidth.Text = wPQMWeldAddProReport.SpanWidth.Trim();
            TB_MetalLiner.Text = wPQMWeldAddProReport.MetalLiner.Trim();
            TB_WelderName.Text = wPQMWeldAddProReport.WelderName.Trim();
            TB_WelderCode.Text = wPQMWeldAddProReport.WelderCode.Trim();
            DLC_WeldingDate.Text = wPQMWeldAddProReport.WeldingDate.ToString("yyyy-MM-dd");
            TB_PenInsJointNumber.Text = wPQMWeldAddProReport.PenInsJointNumber.Trim();
            TB_PenInsRepNumber.Text = wPQMWeldAddProReport.PenInsRepNumber.Trim();
            TB_MetaInsRepNumber.Text = wPQMWeldAddProReport.MetaInsRepNumber.Trim();
            DL_WeldProCode.SelectedValue = wPQMWeldAddProReport.WeldProCode.Trim();

            if (wPQMWeldAddProReport.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldAddProReport");
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

            TB_VerticalWeldingDirection.Text = string.IsNullOrEmpty(wPQMWeldProQua.VerticalWeldingDirection) ? "" : wPQMWeldProQua.VerticalWeldingDirection.Trim();
            DL_AfterHot.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.AfterHot) ? "" : wPQMWeldProQua.AfterHot.Trim().Substring(0, wPQMWeldProQua.AfterHot.Trim().IndexOf("-"));
            TB_WeldingCurrent.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingCurrent) ? "" : wPQMWeldProQua.WeldingCurrent.Trim();
        }
        else
        {
            TB_VerticalWeldingDirection.Text = "";
            DL_AfterHot.SelectedValue = "";
            TB_WeldingCurrent.Text = "";
        }
    }
}