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

public partial class TTWPQMWeldProcedureSpe : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","预焊接工艺规程", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMWeldProQuaName();
            LoadWPQMAllDataName();
            LoadWPQMWeldProcedureSpeList();
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

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='焊后热处理类别' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_AfterWeldingClass.DataSource = lst;
        DL_AfterWeldingClass.DataBind();
        DL_AfterWeldingClass.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMWeldProcedureSpeList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMWeldProcedureSpe Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or WeldingType like '%" + TextBox1.Text.Trim() + "%' or FigureNumber like '%" + TextBox1.Text.Trim() + "%' " +
            "or WeldingTechnology like '%" + TextBox1.Text.Trim() + "%' or WeldingPosition like '%" + TextBox1.Text.Trim() + "%' or AfterHot like '%" + TextBox1.Text.Trim() + "%' or "+
            "JointNumber like '%" + TextBox1.Text.Trim() + "%' or WeldingProcess like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldProcedureSpe");

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
        if (IsWPQMWeldProcedureSpe(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGYHJGYGCYCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMWeldProcedureSpeBLL wPQMWeldProcedureSpeBLL = new WPQMWeldProcedureSpeBLL();
        WPQMWeldProcedureSpe wPQMWeldProcedureSpe = new WPQMWeldProcedureSpe();

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
            wPQMWeldProcedureSpe.WeldedJointDiagram = lbl_Path.Text.Trim();
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
            lbl_Path.Text = strAttach;
            wPQMWeldProcedureSpe.WeldedJointDiagram = strAttach;
        }

        wPQMWeldProcedureSpe.Polarity = TB_Polarity.Text.Trim();
        wPQMWeldProcedureSpe.FigureNumber = TB_FigureNumber.Text.Trim();
        wPQMWeldProcedureSpe.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldProcedureSpe.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMWeldProcedureSpe.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMWeldProcedureSpe.ArcVoltage = TB_ArcVoltage.Text.Trim();
        wPQMWeldProcedureSpe.EnterCode = strUserCode.Trim();
        wPQMWeldProcedureSpe.PulseWidth = TB_PulseWidth.Text.Trim();
        wPQMWeldProcedureSpe.GasFlowFront = TB_GasFlowFront.Text.Trim();
        wPQMWeldProcedureSpe.GasComposition = TB_GasComposition.Text.Trim();
        wPQMWeldProcedureSpe.GasFlowReverse = TB_GasFlowReverse.Text.Trim();
        wPQMWeldProcedureSpe.AfterWeldingClass = DL_AfterWeldingClass.SelectedValue.Trim() + "-" + DL_AfterWeldingClass.SelectedItem.Text.Trim();
        wPQMWeldProcedureSpe.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
        wPQMWeldProcedureSpe.WeldingProcess = TB_WeldingProcess.Text.Trim();
        wPQMWeldProcedureSpe.WeldingTechnology = TB_WeldingTechnology.Text.Trim();
        wPQMWeldProcedureSpe.WeldingPosition = TB_WeldingPosition.Text.Trim();
        wPQMWeldProcedureSpe.Layer = TB_Layer.Text.Trim();
        wPQMWeldProcedureSpe.JointNumber = TB_JointNumber.Text.Trim();
        wPQMWeldProcedureSpe.WeldingType = TB_WeldingType.Text.Trim();
        wPQMWeldProcedureSpe.HolderWeldProject = TB_HolderWeldProject.Text.Trim();
        wPQMWeldProcedureSpe.WeldMetalThickness = TB_WeldMetalThickness.Text.Trim();
        wPQMWeldProcedureSpe.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
        wPQMWeldProcedureSpe.LayerTemperature = TB_LayerTemperature.Text.Trim();
        wPQMWeldProcedureSpe.PulseFrequency = TB_PulseFrequency.Text.Trim();
        wPQMWeldProcedureSpe.TunElecDiameter = TB_TunElecDiameter.Text.Trim();
        wPQMWeldProcedureSpe.NozzleDiameter = TB_NozzleDiameter.Text.Trim();
        wPQMWeldProcedureSpe.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMWeldProcedureSpeBLL.AddWPQMWeldProcedureSpe(wPQMWeldProcedureSpe);
            lbl_ID.Text = GetMaxWPQMWeldProcedureSpeID(wPQMWeldProcedureSpe).ToString();
            UpdateWPQMWeldProQuaData(wPQMWeldProcedureSpe.WeldProCode.Trim());
            LoadWPQMWeldProcedureSpeList();

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

    protected void UpdateWPQMWeldProQuaData(string strCode)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + strCode + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            wPQMWeldProQua.WeldingPosition = TB_WeldingPosition.Text.Trim();
            wPQMWeldProQua.WeldedJointDiagram = lbl_Path.Text.Trim();
            wPQMWeldProQua.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
            wPQMWeldProQua.LayerTemperature = TB_LayerTemperature.Text.Trim();
            wPQMWeldProQua.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
            wPQMWeldProQua.AfterWeldingClass = DL_AfterWeldingClass.SelectedValue.Trim() + "-" + DL_AfterWeldingClass.SelectedItem.Text.Trim();
            wPQMWeldProQua.TunElecDiameter = TB_TunElecDiameter.Text.Trim();
            wPQMWeldProQua.NozzleDiameter = TB_NozzleDiameter.Text.Trim();
            wPQMWeldProQua.Polarity = TB_Polarity.Text.Trim();
            wPQMWeldProQua.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
            wPQMWeldProQua.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
            wPQMWeldProQua.LineEnergy = TB_LineEnergy.Text.Trim();

            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }

    protected int GetMaxWPQMWeldProcedureSpeID(WPQMWeldProcedureSpe bmbp)
    {
        string strHQL = "Select ID From T_WPQMWeldProcedureSpe where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldProcedureSpe").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMWeldProcedureSpe(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMWeldProcedureSpe Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMWeldProcedureSpe Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldProcedureSpe").Tables[0];
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
        if (IsWPQMWeldProcedureSpe(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGYHJGYGCYCZWXZJJC+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMWeldProcedureSpe as wPQMWeldProcedureSpe where wPQMWeldProcedureSpe.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMWeldProcedureSpeBLL wPQMWeldProcedureSpeBLL = new WPQMWeldProcedureSpeBLL();
        IList lst = wPQMWeldProcedureSpeBLL.GetAllWPQMWeldProcedureSpes(strHQL);

        WPQMWeldProcedureSpe wPQMWeldProcedureSpe = (WPQMWeldProcedureSpe)lst[0];

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
            lbl_Path.Text = strAttach;
            wPQMWeldProcedureSpe.WeldedJointDiagram = strAttach;
        }

        wPQMWeldProcedureSpe.Polarity = TB_Polarity.Text.Trim();
        wPQMWeldProcedureSpe.FigureNumber = TB_FigureNumber.Text.Trim();
        wPQMWeldProcedureSpe.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldProcedureSpe.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMWeldProcedureSpe.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMWeldProcedureSpe.ArcVoltage = TB_ArcVoltage.Text.Trim();
        wPQMWeldProcedureSpe.PulseWidth = TB_PulseWidth.Text.Trim();
        wPQMWeldProcedureSpe.GasFlowFront = TB_GasFlowFront.Text.Trim();
        wPQMWeldProcedureSpe.GasComposition = TB_GasComposition.Text.Trim();
        wPQMWeldProcedureSpe.GasFlowReverse = TB_GasFlowReverse.Text.Trim();
        wPQMWeldProcedureSpe.AfterWeldingClass = DL_AfterWeldingClass.SelectedValue.Trim() + "-" + DL_AfterWeldingClass.SelectedItem.Text.Trim();
        wPQMWeldProcedureSpe.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
        wPQMWeldProcedureSpe.WeldingProcess = TB_WeldingProcess.Text.Trim();
        wPQMWeldProcedureSpe.WeldingTechnology = TB_WeldingTechnology.Text.Trim();
        wPQMWeldProcedureSpe.WeldingPosition = TB_WeldingPosition.Text.Trim();
        wPQMWeldProcedureSpe.Layer = TB_Layer.Text.Trim();
        wPQMWeldProcedureSpe.JointNumber = TB_JointNumber.Text.Trim();
        wPQMWeldProcedureSpe.WeldingType = TB_WeldingType.Text.Trim();
        wPQMWeldProcedureSpe.HolderWeldProject = TB_HolderWeldProject.Text.Trim();
        wPQMWeldProcedureSpe.WeldMetalThickness = TB_WeldMetalThickness.Text.Trim();
        wPQMWeldProcedureSpe.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
        wPQMWeldProcedureSpe.LayerTemperature = TB_LayerTemperature.Text.Trim();
        wPQMWeldProcedureSpe.PulseFrequency = TB_PulseFrequency.Text.Trim();
        wPQMWeldProcedureSpe.TunElecDiameter = TB_TunElecDiameter.Text.Trim();
        wPQMWeldProcedureSpe.NozzleDiameter = TB_NozzleDiameter.Text.Trim();
        wPQMWeldProcedureSpe.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMWeldProcedureSpeBLL.UpdateWPQMWeldProcedureSpe(wPQMWeldProcedureSpe, wPQMWeldProcedureSpe.ID);
            UpdateWPQMWeldProQuaData(wPQMWeldProcedureSpe.WeldProCode.Trim());
            LoadWPQMWeldProcedureSpeList();

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

        strHQL = "Delete From T_WPQMWeldProcedureSpe Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMWeldProcedureSpeList();

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
        LoadWPQMWeldProcedureSpeList();
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

            strHQL = "From WPQMWeldProcedureSpe as wPQMWeldProcedureSpe where wPQMWeldProcedureSpe.ID = '" + strId + "'";
            WPQMWeldProcedureSpeBLL wPQMWeldProcedureSpeBLL = new WPQMWeldProcedureSpeBLL();
            lst = wPQMWeldProcedureSpeBLL.GetAllWPQMWeldProcedureSpes(strHQL);

            WPQMWeldProcedureSpe wPQMWeldProcedureSpe = (WPQMWeldProcedureSpe)lst[0];

            lbl_ID.Text = wPQMWeldProcedureSpe.ID.ToString();
            TB_Polarity.Text = wPQMWeldProcedureSpe.Polarity.Trim();
            TB_FigureNumber.Text = wPQMWeldProcedureSpe.FigureNumber.Trim();
            TB_WeldingCurrent.Text = wPQMWeldProcedureSpe.WeldingCurrent.Trim();
            TB_LineEnergy.Text = wPQMWeldProcedureSpe.LineEnergy.Trim();
            TB_WeldingSpeed.Text = wPQMWeldProcedureSpe.WeldingSpeed.Trim();
            TB_ArcVoltage.Text = wPQMWeldProcedureSpe.ArcVoltage.Trim();
            TB_PulseWidth.Text = wPQMWeldProcedureSpe.PulseWidth.Trim();
            TB_GasFlowFront.Text = wPQMWeldProcedureSpe.GasFlowFront.Trim();
            TB_GasComposition.Text = wPQMWeldProcedureSpe.GasComposition.Trim();
            TB_GasFlowReverse.Text = wPQMWeldProcedureSpe.GasFlowReverse.Trim();
            DL_AfterHot.SelectedValue = wPQMWeldProcedureSpe.AfterHot.Trim().Substring(0, wPQMWeldProcedureSpe.AfterHot.Trim().IndexOf("-"));
            DL_AfterWeldingClass.SelectedValue = wPQMWeldProcedureSpe.AfterWeldingClass.Trim().Substring(0, wPQMWeldProcedureSpe.AfterWeldingClass.Trim().IndexOf("-"));
            TB_WeldingProcess.Text = wPQMWeldProcedureSpe.WeldingProcess.Trim();
            TB_WeldingTechnology.Text = wPQMWeldProcedureSpe.WeldingTechnology.Trim();
            TB_WeldingPosition.Text = wPQMWeldProcedureSpe.WeldingPosition.Trim();
            TB_Layer.Text = wPQMWeldProcedureSpe.Layer.Trim();
            TB_JointNumber.Text = wPQMWeldProcedureSpe.JointNumber.Trim();
            TB_WeldingType.Text = wPQMWeldProcedureSpe.WeldingType.Trim();
            TB_HolderWeldProject.Text = wPQMWeldProcedureSpe.HolderWeldProject.Trim();
            TB_WeldMetalThickness.Text = wPQMWeldProcedureSpe.WeldMetalThickness.Trim();
            TB_PreheatingTemperature.Text = wPQMWeldProcedureSpe.PreheatingTemperature.Trim();
            TB_LayerTemperature.Text = wPQMWeldProcedureSpe.LayerTemperature.Trim();
            TB_PulseFrequency.Text = wPQMWeldProcedureSpe.PulseFrequency.Trim();
            TB_TunElecDiameter.Text = wPQMWeldProcedureSpe.TunElecDiameter.Trim();
            TB_NozzleDiameter.Text = wPQMWeldProcedureSpe.NozzleDiameter.Trim();
            DL_WeldProCode.SelectedValue = wPQMWeldProcedureSpe.WeldProCode.Trim();
            lbl_Path.Text = wPQMWeldProcedureSpe.WeldedJointDiagram.Trim();

            if (wPQMWeldProcedureSpe.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldProcedureSpe");
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

            TB_WeldingPosition.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingPosition) ? "" : wPQMWeldProQua.WeldingPosition.Trim();
            TB_PreheatingTemperature.Text = string.IsNullOrEmpty(wPQMWeldProQua.PreheatingTemperature) ? "" : wPQMWeldProQua.PreheatingTemperature.Trim();
            TB_LayerTemperature.Text = string.IsNullOrEmpty(wPQMWeldProQua.LayerTemperature) ? "" : wPQMWeldProQua.LayerTemperature.Trim();
            DL_AfterHot.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.AfterHot) ? "" : wPQMWeldProQua.AfterHot.Trim().Substring(0, wPQMWeldProQua.AfterHot.Trim().IndexOf("-"));
            DL_AfterWeldingClass.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.AfterWeldingClass) ? "" : wPQMWeldProQua.AfterWeldingClass.Trim().Substring(0, wPQMWeldProQua.AfterWeldingClass.Trim().IndexOf("-"));
            TB_TunElecDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.TunElecDiameter) ? "" : wPQMWeldProQua.TunElecDiameter.Trim();
            TB_NozzleDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.NozzleDiameter) ? "" : wPQMWeldProQua.NozzleDiameter.Trim();
            TB_GasComposition.Text = string.IsNullOrEmpty(wPQMWeldProQua.ProtectiveGas) ? "" : wPQMWeldProQua.ProtectiveGas.Trim();
            TB_Polarity.Text = string.IsNullOrEmpty(wPQMWeldProQua.Polarity) ? "" : wPQMWeldProQua.Polarity.Trim();
            TB_WeldingCurrent.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingCurrent) ? "" : wPQMWeldProQua.WeldingCurrent.Trim();
            TB_WeldingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldingSpeed) ? "" : wPQMWeldProQua.WeldingSpeed.Trim();
            TB_LineEnergy.Text = string.IsNullOrEmpty(wPQMWeldProQua.LineEnergy) ? "" : wPQMWeldProQua.LineEnergy.Trim();
            lbl_Path.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldedJointDiagram) ? "" : wPQMWeldProQua.WeldedJointDiagram.Trim();
        }
        else
        {
            TB_WeldingPosition.Text = "";
            lbl_Path.Text = "";
            TB_PreheatingTemperature.Text = "";
            TB_LayerTemperature.Text = "";
            DL_AfterWeldingClass.SelectedValue = "";
            DL_AfterHot.SelectedValue = "";
            TB_TunElecDiameter.Text = "";
            TB_NozzleDiameter.Text = "";
            TB_GasComposition.Text = "";
            TB_Polarity.Text = "";
            TB_WeldingCurrent.Text = "";
            TB_WeldingSpeed.Text = "";
            TB_LineEnergy.Text = "";
        }
    }
}