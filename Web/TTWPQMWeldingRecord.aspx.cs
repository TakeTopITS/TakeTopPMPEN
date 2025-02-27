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

public partial class TTWPQMWeldingRecord : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","焊接记录表", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMWeldProQuaName();
            LoadWPQMAllDataName();
            LoadWPQMWeldingRecordList();
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
        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='HeatingMethod' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_HeatingMode.DataSource = lst;
        DL_HeatingMode.DataBind();
        DL_HeatingMode.Items.Insert(0, new ListItem("--Select--", ""));

        DL_HeatingMode1.DataSource = lst;
        DL_HeatingMode1.DataBind();
        DL_HeatingMode1.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingDirection' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WeldingDirection.DataSource = lst;
        DL_WeldingDirection.DataBind();
        DL_WeldingDirection.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='CategoryGroupNumber' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_CategoryGroups.DataSource = lst;
        DL_CategoryGroups.DataBind();
        DL_CategoryGroups.Items.Insert(0, new ListItem("--Select--", ""));

        DL_CategoryGroups1.DataSource = lst;
        DL_CategoryGroups1.DataBind();
        DL_CategoryGroups1.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingRodBrand' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_ElecBrand.DataSource = lst;
        DL_ElecBrand.DataBind();
        DL_ElecBrand.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingRodSpecification' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_ElecSpe.DataSource = lst;
        DL_ElecSpe.DataBind();
        DL_ElecSpe.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingRodModel' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_ElecType.DataSource = lst;
        DL_ElecType.DataBind();
        DL_ElecType.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='FluxBrand' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_FluxBrand.DataSource = lst;
        DL_FluxBrand.DataBind();
        DL_FluxBrand.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='FluxSpecification' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_FluxSpe.DataSource = lst;
        DL_FluxSpe.DataBind();
        DL_FluxSpe.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='FluxModel' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_FluxType.DataSource = lst;
        DL_FluxType.DataBind();
        DL_FluxType.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingWireBrand' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WireBrand.DataSource = lst;
        DL_WireBrand.DataBind();
        DL_WireBrand.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingWireSpecification' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WireSpe.DataSource = lst;
        DL_WireSpe.DataBind();
        DL_WireSpe.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingWireModel' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WireType.DataSource = lst;
        DL_WireType.DataBind();
        DL_WireType.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='BaseMaterialSteelGrade' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_MaterialNo.DataSource = lst;
        DL_MaterialNo.DataBind();
        DL_MaterialNo.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='BaseMaterialSpecification' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_MaterialSpecification.DataSource = lst;
        DL_MaterialSpecification.DataBind();
        DL_MaterialSpecification.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingMethod' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WeldingMethod.DataSource = lst;
        DL_WeldingMethod.DataBind();
        DL_WeldingMethod.Items.Insert(0, new ListItem("--Select--", ""));

        DL_WeldingMethod1.DataSource = lst;
        DL_WeldingMethod1.DataBind();
        DL_WeldingMethod1.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='WeldingMaterialCategory' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WeldMaterialCategory.DataSource = lst;
        DL_WeldMaterialCategory.DataBind();
        DL_WeldMaterialCategory.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMWeldingRecordList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMWeldingRecord Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (MaterialNo like '%" + TextBox1.Text.Trim() + "%' or MaterialSpecification like '%" + TextBox1.Text.Trim() + "%' or WeldProCode like '%" + TextBox1.Text.Trim() + "%' " +
            "or WeldingFormDiagram like '%" + TextBox1.Text.Trim() + "%' or TempMeasureMethod like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(DL_HeatingMode1.SelectedValue) && DL_HeatingMode1.SelectedValue.Trim() != "")
        {
            strHQL += " and HeatingMode like '" + DL_HeatingMode1.SelectedValue.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(DL_CategoryGroups1.SelectedValue) && DL_CategoryGroups1.SelectedValue.Trim() != "")
        {
            strHQL += " and CategoryGroups like '" + DL_CategoryGroups1.SelectedValue.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(DL_WeldingMethod1.SelectedValue) && DL_WeldingMethod1.SelectedValue.Trim() != "")
        {
            strHQL += " and WeldingMethod like '" + DL_WeldingMethod1.SelectedValue.Trim() + "%' ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldingRecord");

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
        if (IsWPQMWeldingRecord(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGHJJLBYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMWeldingRecordBLL wPQMWeldingRecordBLL = new WPQMWeldingRecordBLL();
        WPQMWeldingRecord wPQMWeldingRecord = new WPQMWeldingRecord();

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
            wPQMWeldingRecord.GrooveDiagramPath = "";
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
            wPQMWeldingRecord.GrooveDiagramPath = strAttach;
        }

        wPQMWeldingRecord.AfterHotTemp = TB_AfterHotTemp.Text.Trim();
        wPQMWeldingRecord.AfterHotTime = TB_AfterHotTime.Text.Trim();
        wPQMWeldingRecord.BackClearRootMethod = TB_BackClearRootMethod.Text.Trim();
        wPQMWeldingRecord.BackGasFlowRate = TB_BackGasFlowRate.Text.Trim();
        wPQMWeldingRecord.BackProtectiveGas = TB_BackProtectiveGas.Text.Trim();
        wPQMWeldingRecord.BackProtectiveGasMixRatio = TB_BackProtectiveGasMixRatio.Text.Trim();
        wPQMWeldingRecord.CategoryGroups = DL_CategoryGroups.SelectedValue.Trim() + "-" + DL_CategoryGroups.SelectedItem.Text.Trim();
        wPQMWeldingRecord.CleanBefWelding = TB_CleanBefWelding.Text.Trim();
        wPQMWeldingRecord.ConductiveMouthArtifacts = TB_ConductiveMouthArtifacts.Text.Trim();
        wPQMWeldingRecord.CurrentType = TB_CurrentType.Text.Trim();
        TB_ElecTypeBrandSpe.Text = DL_ElecType.SelectedValue.Trim() + "-" + DL_ElecType.SelectedItem.Text.Trim() + ";" + DL_ElecBrand.SelectedValue.Trim() + "-" + DL_ElecBrand.SelectedItem.Text.Trim() + ";" + DL_ElecSpe.SelectedValue.Trim() + "-" + DL_ElecSpe.SelectedItem.Text.Trim();
        wPQMWeldingRecord.ElecTypeBrandSpe = TB_ElecTypeBrandSpe.Text.Trim();
        wPQMWeldingRecord.EnterCode = strUserCode.Trim();
        wPQMWeldingRecord.EnvironmentTemperature = TB_EnvironmentTemperature.Text.Trim();
        TB_FluxTypeBrandSpe.Text = DL_FluxType.SelectedValue.Trim() + "-" + DL_FluxType.SelectedItem.Text.Trim() + ";" + DL_FluxBrand.SelectedValue.Trim() + "-" + DL_FluxBrand.SelectedItem.Text.Trim() + ";" + DL_FluxSpe.SelectedValue.Trim() + "-" + DL_FluxSpe.SelectedItem.Text.Trim();
        wPQMWeldingRecord.FluxTypeBrandSpe = TB_FluxTypeBrandSpe.Text.Trim();
        wPQMWeldingRecord.HeatingMode = DL_HeatingMode.SelectedValue.Trim() + "-" + DL_HeatingMode.SelectedItem.Text.Trim();
        wPQMWeldingRecord.LayerClean = TB_LayerClean.Text.Trim();
        wPQMWeldingRecord.LayerTemperature = TB_LayerTemperature.Text.Trim();
        wPQMWeldingRecord.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMWeldingRecord.MaterialNo = DL_MaterialNo.SelectedValue.Trim() + "-" + DL_MaterialNo.SelectedItem.Text.Trim();
        wPQMWeldingRecord.MaterialSpecification = DL_MaterialSpecification.SelectedValue.Trim() + "-" + DL_MaterialSpecification.SelectedItem.Text.Trim();
        wPQMWeldingRecord.NozzleDiameter = TB_NozzleDiameter.Text.Trim();
        wPQMWeldingRecord.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
        wPQMWeldingRecord.ProGasMixRatio = TB_ProGasMixRatio.Text.Trim();
        wPQMWeldingRecord.ProtectiveGas = TB_ProtectiveGas.Text.Trim();
        wPQMWeldingRecord.RelativeHumidity = TB_RelativeHumidity.Text.Trim();
        wPQMWeldingRecord.ShieldingGasFlowRate = TB_ShieldingGasFlowRate.Text.Trim();
        wPQMWeldingRecord.TailGasFlowRate = TB_TailGasFlowRate.Text.Trim();
        wPQMWeldingRecord.TailProtectiveGas = TB_TailProtectiveGas.Text.Trim();
        wPQMWeldingRecord.TailProtectiveGasMixRatio = TB_TailProtectiveGasMixRatio.Text.Trim();
        wPQMWeldingRecord.TempMeasureMethod = TB_TempMeasureMethod.Text.Trim();
        wPQMWeldingRecord.TunElecDiameter = TB_TunElecDiameter.Text.Trim();
        wPQMWeldingRecord.TunElecType = TB_TunElecType.Text.Trim();
        wPQMWeldingRecord.WarmUpTime = TB_WarmUpTime.Text.Trim();
        wPQMWeldingRecord.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldingRecord.WeldingDirection = DL_WeldingDirection.SelectedValue.Trim() + "-" + DL_WeldingDirection.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WeldingFormDiagram = TB_WeldingFormDiagram.Text.Trim();
        wPQMWeldingRecord.WeldingMethod = DL_WeldingMethod.SelectedValue.Trim() + "-" + DL_WeldingMethod.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WeldingPosition = TB_WeldingPosition.Text.Trim();
        wPQMWeldingRecord.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMWeldingRecord.WeldingVoltage = TB_WeldingVoltage.Text.Trim();
        wPQMWeldingRecord.WeldMaterialCategory = DL_WeldMaterialCategory.SelectedValue.Trim() + "-" + DL_WeldMaterialCategory.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMWeldingRecord.WireSpeed = TB_WireSpeed.Text.Trim();
        TB_WireTypeBrandSpe.Text = DL_WireType.SelectedValue.Trim() + "-" + DL_WireType.SelectedItem.Text.Trim() + ";" + DL_WireBrand.SelectedValue.Trim() + "-" + DL_WireBrand.SelectedItem.Text.Trim() + ";" + DL_WireSpe.SelectedValue.Trim() + "-" + DL_WireSpe.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WireTypeBrandSpe = TB_WireTypeBrandSpe.Text.Trim();

        try
        {
            wPQMWeldingRecordBLL.AddWPQMWeldingRecord(wPQMWeldingRecord);
            lbl_ID.Text = GetMaxWPQMWeldingRecordID(wPQMWeldingRecord).ToString();
            UpdateWPQMWeldProQuaData(wPQMWeldingRecord.WeldProCode.Trim());
            LoadWPQMWeldingRecordList();

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
            wPQMWeldProQua.AfterHotTemp = TB_AfterHotTemp.Text.Trim();
            wPQMWeldProQua.AfterHotTime = TB_AfterHotTime.Text.Trim();
            wPQMWeldProQua.BackClearRootMethod = TB_BackClearRootMethod.Text.Trim();
            wPQMWeldProQua.BackGasFlowRate = TB_BackGasFlowRate.Text.Trim();
            wPQMWeldProQua.BackProtectiveGas = TB_BackProtectiveGas.Text.Trim();
            wPQMWeldProQua.BackProtectiveGasMixRatio = TB_BackProtectiveGasMixRatio.Text.Trim();
            wPQMWeldProQua.CategoryGroups = DL_CategoryGroups.SelectedValue.Trim() + "-" + DL_CategoryGroups.SelectedItem.Text.Trim();
            wPQMWeldProQua.CleanBefWelding = TB_CleanBefWelding.Text.Trim();
            wPQMWeldProQua.CurrentType = TB_CurrentType.Text.Trim();
            wPQMWeldProQua.ElecTypeBrandSpe = TB_ElecTypeBrandSpe.Text.Trim();
            wPQMWeldProQua.FluxTypeBrandSpe = TB_FluxTypeBrandSpe.Text.Trim();
            wPQMWeldProQua.HeatingMode = DL_HeatingMode.SelectedValue.Trim() + "-" + DL_HeatingMode.SelectedItem.Text.Trim();
            wPQMWeldProQua.LayerClean = TB_LayerClean.Text.Trim();
            wPQMWeldProQua.LayerTemperature = TB_LayerTemperature.Text.Trim();
            wPQMWeldProQua.LineEnergy = TB_LineEnergy.Text.Trim();
            wPQMWeldProQua.MaterialNo = DL_MaterialNo.SelectedValue.Trim() + "-" + DL_MaterialNo.SelectedItem.Text.Trim();
            wPQMWeldProQua.MaterialSpecification = DL_MaterialSpecification.SelectedValue.Trim() + "-" + DL_MaterialSpecification.SelectedItem.Text.Trim();
            wPQMWeldProQua.NozzleDiameter = TB_NozzleDiameter.Text.Trim();
            wPQMWeldProQua.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
            wPQMWeldProQua.ProGasMixRatio = TB_ProGasMixRatio.Text.Trim();
            wPQMWeldProQua.ProtectiveGas = TB_ProtectiveGas.Text.Trim();
            wPQMWeldProQua.ShieldingGasFlowRate = TB_ShieldingGasFlowRate.Text.Trim();
            wPQMWeldProQua.TailGasFlowRate = TB_TailGasFlowRate.Text.Trim();
            wPQMWeldProQua.TailProtectiveGas = TB_TailProtectiveGas.Text.Trim();
            wPQMWeldProQua.TailProtectiveGasMixRatio = TB_TailProtectiveGasMixRatio.Text.Trim();
            wPQMWeldProQua.TempMeasureMethod = TB_TempMeasureMethod.Text.Trim();
            wPQMWeldProQua.TunElecDiameter = TB_TunElecDiameter.Text.Trim();
            wPQMWeldProQua.TunElecType = TB_TunElecType.Text.Trim();
            wPQMWeldProQua.WarmUpTime = TB_WarmUpTime.Text.Trim();
            wPQMWeldProQua.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
            wPQMWeldProQua.WeldingDirection = DL_WeldingDirection.SelectedValue.Trim() + "-" + DL_WeldingDirection.SelectedItem.Text.Trim();
            wPQMWeldProQua.WeldingMethod = DL_WeldingMethod.SelectedValue.Trim() + "-" + DL_WeldingMethod.SelectedItem.Text.Trim();
            wPQMWeldProQua.WeldingPosition = TB_WeldingPosition.Text.Trim();
            wPQMWeldProQua.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
            wPQMWeldProQua.WeldingVoltage = TB_WeldingVoltage.Text.Trim();
            wPQMWeldProQua.WeldMaterialCategory = DL_WeldMaterialCategory.SelectedValue.Trim() + "-" + DL_WeldMaterialCategory.SelectedItem.Text.Trim();
            wPQMWeldProQua.WireSpeed = TB_WireSpeed.Text.Trim();
            wPQMWeldProQua.WireTypeBrandSpe = TB_WireTypeBrandSpe.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }

    protected int GetMaxWPQMWeldingRecordID(WPQMWeldingRecord bmbp)
    {
        string strHQL = "Select ID From T_WPQMWeldingRecord where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldingRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMWeldingRecord(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMWeldingRecord Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMWeldingRecord Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldingRecord").Tables[0];
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
        if (IsWPQMWeldingRecord(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGHJJLBYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMWeldingRecord as wPQMWeldingRecord where wPQMWeldingRecord.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMWeldingRecordBLL wPQMWeldingRecordBLL = new WPQMWeldingRecordBLL();
        IList lst = wPQMWeldingRecordBLL.GetAllWPQMWeldingRecords(strHQL);

        WPQMWeldingRecord wPQMWeldingRecord = (WPQMWeldingRecord)lst[0];

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
            wPQMWeldingRecord.GrooveDiagramPath = strAttach;
        }

        wPQMWeldingRecord.AfterHotTemp = TB_AfterHotTemp.Text.Trim();
        wPQMWeldingRecord.AfterHotTime = TB_AfterHotTime.Text.Trim();
        wPQMWeldingRecord.BackClearRootMethod = TB_BackClearRootMethod.Text.Trim();
        wPQMWeldingRecord.BackGasFlowRate = TB_BackGasFlowRate.Text.Trim();
        wPQMWeldingRecord.BackProtectiveGas = TB_BackProtectiveGas.Text.Trim();
        wPQMWeldingRecord.BackProtectiveGasMixRatio = TB_BackProtectiveGasMixRatio.Text.Trim();
        wPQMWeldingRecord.CategoryGroups = DL_CategoryGroups.SelectedValue.Trim() + "-" + DL_CategoryGroups.SelectedItem.Text.Trim();
        wPQMWeldingRecord.CleanBefWelding = TB_CleanBefWelding.Text.Trim();
        wPQMWeldingRecord.ConductiveMouthArtifacts = TB_ConductiveMouthArtifacts.Text.Trim();
        wPQMWeldingRecord.CurrentType = TB_CurrentType.Text.Trim();
        TB_ElecTypeBrandSpe.Text = DL_ElecType.SelectedValue.Trim() + "-" + DL_ElecType.SelectedItem.Text.Trim() + ";" + DL_ElecBrand.SelectedValue.Trim() + "-" + DL_ElecBrand.SelectedItem.Text.Trim() + ";" + DL_ElecSpe.SelectedValue.Trim() + "-" + DL_ElecSpe.SelectedItem.Text.Trim();
        wPQMWeldingRecord.ElecTypeBrandSpe = TB_ElecTypeBrandSpe.Text.Trim();
        wPQMWeldingRecord.EnvironmentTemperature = TB_EnvironmentTemperature.Text.Trim();
        TB_FluxTypeBrandSpe.Text = DL_FluxType.SelectedValue.Trim() + "-" + DL_FluxType.SelectedItem.Text.Trim() + ";" + DL_FluxBrand.SelectedValue.Trim() + "-" + DL_FluxBrand.SelectedItem.Text.Trim() + ";" + DL_FluxSpe.SelectedValue.Trim() + "-" + DL_FluxSpe.SelectedItem.Text.Trim();
        wPQMWeldingRecord.FluxTypeBrandSpe = TB_FluxTypeBrandSpe.Text.Trim();
        wPQMWeldingRecord.HeatingMode = DL_HeatingMode.SelectedValue.Trim() + "-" + DL_HeatingMode.SelectedItem.Text.Trim();
        wPQMWeldingRecord.LayerClean = TB_LayerClean.Text.Trim();
        wPQMWeldingRecord.LayerTemperature = TB_LayerTemperature.Text.Trim();
        wPQMWeldingRecord.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMWeldingRecord.MaterialNo = DL_MaterialNo.SelectedValue.Trim() + "-" + DL_MaterialNo.SelectedItem.Text.Trim();
        wPQMWeldingRecord.MaterialSpecification = DL_MaterialSpecification.SelectedValue.Trim() + "-" + DL_MaterialSpecification.SelectedItem.Text.Trim();
        wPQMWeldingRecord.NozzleDiameter = TB_NozzleDiameter.Text.Trim();
        wPQMWeldingRecord.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
        wPQMWeldingRecord.ProGasMixRatio = TB_ProGasMixRatio.Text.Trim();
        wPQMWeldingRecord.ProtectiveGas = TB_ProtectiveGas.Text.Trim();
        wPQMWeldingRecord.RelativeHumidity = TB_RelativeHumidity.Text.Trim();
        wPQMWeldingRecord.ShieldingGasFlowRate = TB_ShieldingGasFlowRate.Text.Trim();
        wPQMWeldingRecord.TailGasFlowRate = TB_TailGasFlowRate.Text.Trim();
        wPQMWeldingRecord.TailProtectiveGas = TB_TailProtectiveGas.Text.Trim();
        wPQMWeldingRecord.TailProtectiveGasMixRatio = TB_TailProtectiveGasMixRatio.Text.Trim();
        wPQMWeldingRecord.TempMeasureMethod = TB_TempMeasureMethod.Text.Trim();
        wPQMWeldingRecord.TunElecDiameter = TB_TunElecDiameter.Text.Trim();
        wPQMWeldingRecord.TunElecType = TB_TunElecType.Text.Trim();
        wPQMWeldingRecord.WarmUpTime = TB_WarmUpTime.Text.Trim();
        wPQMWeldingRecord.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldingRecord.WeldingDirection = DL_WeldingDirection.SelectedValue.Trim() + "-" + DL_WeldingDirection.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WeldingFormDiagram = TB_WeldingFormDiagram.Text.Trim();
        wPQMWeldingRecord.WeldingMethod = DL_WeldingMethod.SelectedValue.Trim() + "-" + DL_WeldingMethod.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WeldingPosition = TB_WeldingPosition.Text.Trim();
        wPQMWeldingRecord.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMWeldingRecord.WeldingVoltage = TB_WeldingVoltage.Text.Trim();
        wPQMWeldingRecord.WeldMaterialCategory = DL_WeldMaterialCategory.SelectedValue.Trim() + "-" + DL_WeldMaterialCategory.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        wPQMWeldingRecord.WireSpeed = TB_WireSpeed.Text.Trim();
        TB_WireTypeBrandSpe.Text = DL_WireType.SelectedValue.Trim() + "-" + DL_WireType.SelectedItem.Text.Trim() + ";" + DL_WireBrand.SelectedValue.Trim() + "-" + DL_WireBrand.SelectedItem.Text.Trim() + ";" + DL_WireSpe.SelectedValue.Trim() + "-" + DL_WireSpe.SelectedItem.Text.Trim();
        wPQMWeldingRecord.WireTypeBrandSpe = TB_WireTypeBrandSpe.Text.Trim();

        try
        {
            wPQMWeldingRecordBLL.UpdateWPQMWeldingRecord(wPQMWeldingRecord, wPQMWeldingRecord.ID);
            UpdateWPQMWeldProQuaData(wPQMWeldingRecord.WeldProCode.Trim());
            LoadWPQMWeldingRecordList();

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

        strHQL = "Delete From T_WPQMWeldingRecord Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMWeldingRecordList();

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
        LoadWPQMWeldingRecordList();
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

            strHQL = "From WPQMWeldingRecord as wPQMWeldingRecord where wPQMWeldingRecord.ID = '" + strId + "'";
            WPQMWeldingRecordBLL wPQMWeldingRecordBLL = new WPQMWeldingRecordBLL();
            lst = wPQMWeldingRecordBLL.GetAllWPQMWeldingRecords(strHQL);

            WPQMWeldingRecord wPQMWeldingRecord = (WPQMWeldingRecord)lst[0];

            lbl_ID.Text = wPQMWeldingRecord.ID.ToString();
            TB_AfterHotTemp.Text = wPQMWeldingRecord.AfterHotTemp.Trim();
            TB_AfterHotTime.Text = wPQMWeldingRecord.AfterHotTime.Trim();
            TB_BackClearRootMethod.Text = wPQMWeldingRecord.BackClearRootMethod.Trim();
            TB_BackGasFlowRate.Text = wPQMWeldingRecord.BackGasFlowRate.Trim();
            TB_BackProtectiveGas.Text = wPQMWeldingRecord.BackProtectiveGas.Trim();
            TB_BackProtectiveGasMixRatio.Text = wPQMWeldingRecord.BackProtectiveGasMixRatio.Trim();
            TB_CleanBefWelding.Text = wPQMWeldingRecord.CleanBefWelding.Trim();
            TB_ConductiveMouthArtifacts.Text = wPQMWeldingRecord.ConductiveMouthArtifacts.Trim();
            TB_CurrentType.Text = wPQMWeldingRecord.CurrentType.Trim();
            TB_ElecTypeBrandSpe.Text = wPQMWeldingRecord.ElecTypeBrandSpe.Trim();
            DL_ElecBrand.SelectedValue = wPQMWeldingRecord.ElecTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.ElecTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldingRecord.ElecTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.ElecTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_ElecSpe.SelectedValue = wPQMWeldingRecord.ElecTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.ElecTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldingRecord.ElecTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldingRecord.ElecTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_ElecType.SelectedValue = wPQMWeldingRecord.ElecTypeBrandSpe.Trim().Substring(0, wPQMWeldingRecord.ElecTypeBrandSpe.Trim().IndexOf("-"));
            TB_EnvironmentTemperature.Text = wPQMWeldingRecord.EnvironmentTemperature.Trim();
            TB_FluxTypeBrandSpe.Text = wPQMWeldingRecord.FluxTypeBrandSpe.Trim();
            DL_FluxBrand.SelectedValue = wPQMWeldingRecord.FluxTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.FluxTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldingRecord.FluxTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.FluxTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_FluxSpe.SelectedValue = wPQMWeldingRecord.FluxTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.FluxTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldingRecord.FluxTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldingRecord.FluxTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_FluxType.SelectedValue = wPQMWeldingRecord.FluxTypeBrandSpe.Trim().Substring(0, wPQMWeldingRecord.FluxTypeBrandSpe.Trim().IndexOf("-"));
            TB_LayerClean.Text = wPQMWeldingRecord.LayerClean.Trim();
            TB_LayerTemperature.Text = wPQMWeldingRecord.LayerTemperature.Trim();
            TB_LineEnergy.Text = wPQMWeldingRecord.LineEnergy.Trim();
            TB_NozzleDiameter.Text = wPQMWeldingRecord.NozzleDiameter.Trim();
            TB_PreheatingTemperature.Text = wPQMWeldingRecord.PreheatingTemperature.Trim();
            TB_ProGasMixRatio.Text = wPQMWeldingRecord.ProGasMixRatio.Trim();
            TB_ProtectiveGas.Text = wPQMWeldingRecord.ProtectiveGas.Trim();
            TB_RelativeHumidity.Text = wPQMWeldingRecord.RelativeHumidity.Trim();
            TB_ShieldingGasFlowRate.Text = wPQMWeldingRecord.ShieldingGasFlowRate.Trim();
            TB_TailGasFlowRate.Text = wPQMWeldingRecord.TailGasFlowRate.Trim();
            TB_TailProtectiveGas.Text = wPQMWeldingRecord.TailProtectiveGas.Trim();
            TB_TailProtectiveGasMixRatio.Text = wPQMWeldingRecord.TailProtectiveGasMixRatio.Trim();
            TB_TempMeasureMethod.Text = wPQMWeldingRecord.TempMeasureMethod.Trim();
            TB_TunElecDiameter.Text = wPQMWeldingRecord.TunElecDiameter.Trim();
            TB_TunElecType.Text = wPQMWeldingRecord.TunElecType.Trim();
            TB_WarmUpTime.Text = wPQMWeldingRecord.WarmUpTime.Trim();
            TB_WeldingCurrent.Text = wPQMWeldingRecord.WeldingCurrent.Trim();
            TB_WeldingFormDiagram.Text = wPQMWeldingRecord.WeldingFormDiagram.Trim();
            TB_WeldingPosition.Text = wPQMWeldingRecord.WeldingPosition.Trim();
            TB_WeldingSpeed.Text = wPQMWeldingRecord.WeldingSpeed.Trim();
            TB_WeldingVoltage.Text = wPQMWeldingRecord.WeldingVoltage.Trim();
            TB_WireSpeed.Text = wPQMWeldingRecord.WireSpeed.Trim();
            TB_WireTypeBrandSpe.Text = wPQMWeldingRecord.WireTypeBrandSpe.Trim();
            DL_WireBrand.SelectedValue = wPQMWeldingRecord.WireTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.WireTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldingRecord.WireTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.WireTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_WireSpe.SelectedValue = wPQMWeldingRecord.WireTypeBrandSpe.Trim().Substring(wPQMWeldingRecord.WireTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldingRecord.WireTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldingRecord.WireTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_WireType.SelectedValue = wPQMWeldingRecord.WireTypeBrandSpe.Trim().Substring(0, wPQMWeldingRecord.WireTypeBrandSpe.Trim().IndexOf("-"));
            DL_WeldProCode.SelectedValue = wPQMWeldingRecord.WeldProCode.Trim();
            DL_CategoryGroups.SelectedValue = wPQMWeldingRecord.CategoryGroups.Trim().Substring(0, wPQMWeldingRecord.CategoryGroups.Trim().IndexOf("-"));
            DL_HeatingMode.SelectedValue = wPQMWeldingRecord.HeatingMode.Trim().Substring(0, wPQMWeldingRecord.HeatingMode.Trim().IndexOf("-"));
            DL_WeldingDirection.SelectedValue = wPQMWeldingRecord.WeldingDirection.Trim().Substring(0, wPQMWeldingRecord.WeldingDirection.Trim().IndexOf("-"));
            DL_MaterialNo.SelectedValue = wPQMWeldingRecord.MaterialNo.Trim().Substring(0, wPQMWeldingRecord.MaterialNo.Trim().IndexOf("-"));
            DL_MaterialSpecification.SelectedValue = wPQMWeldingRecord.MaterialSpecification.Trim().Substring(0, wPQMWeldingRecord.MaterialSpecification.Trim().IndexOf("-"));
            DL_WeldingMethod.SelectedValue = wPQMWeldingRecord.WeldingMethod.Trim().Substring(0, wPQMWeldingRecord.WeldingMethod.Trim().IndexOf("-"));
            DL_WeldMaterialCategory.SelectedValue = wPQMWeldingRecord.WeldMaterialCategory.Trim().Substring(0, wPQMWeldingRecord.WeldMaterialCategory.Trim().IndexOf("-"));

            if (wPQMWeldingRecord.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldingRecord");
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

            TB_ElecTypeBrandSpe.Text = wPQMWeldProQua.ElecTypeBrandSpe.Trim();
            DL_ElecBrand.SelectedValue = wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(wPQMWeldProQua.ElecTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(wPQMWeldProQua.ElecTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_ElecSpe.SelectedValue = wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_ElecType.SelectedValue = wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(0, wPQMWeldProQua.ElecTypeBrandSpe.Trim().IndexOf("-"));
            TB_FluxTypeBrandSpe.Text = wPQMWeldProQua.FluxTypeBrandSpe.Trim();
            DL_FluxBrand.SelectedValue = wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(wPQMWeldProQua.FluxTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(wPQMWeldProQua.FluxTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_FluxSpe.SelectedValue = wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(wPQMWeldProQua.FluxTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldProQua.FluxTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldProQua.FluxTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_FluxType.SelectedValue = wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(0, wPQMWeldProQua.FluxTypeBrandSpe.Trim().IndexOf("-"));
            TB_LayerTemperature.Text = wPQMWeldProQua.LayerTemperature.Trim();
            TB_LineEnergy.Text = wPQMWeldProQua.LineEnergy.Trim();
            TB_PreheatingTemperature.Text = wPQMWeldProQua.PreheatingTemperature.Trim();
            TB_ProGasMixRatio.Text = wPQMWeldProQua.ProGasMixRatio.Trim();
            TB_ProtectiveGas.Text = wPQMWeldProQua.ProtectiveGas.Trim();
            TB_ShieldingGasFlowRate.Text = wPQMWeldProQua.ShieldingGasFlowRate.Trim();
            TB_WeldingCurrent.Text = wPQMWeldProQua.WeldingCurrent.Trim();
            TB_WeldingPosition.Text = wPQMWeldProQua.WeldingPosition.Trim();
            TB_WeldingSpeed.Text = wPQMWeldProQua.WeldingSpeed.Trim();
            TB_WeldingVoltage.Text = wPQMWeldProQua.WeldingVoltage.Trim();
            TB_WireTypeBrandSpe.Text = wPQMWeldProQua.WireTypeBrandSpe.Trim();
            DL_WireBrand.SelectedValue = wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(wPQMWeldProQua.WireTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(wPQMWeldProQua.WireTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_WireSpe.SelectedValue = wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(wPQMWeldProQua.WireTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldProQua.WireTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldProQua.WireTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_WireType.SelectedValue = wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(0, wPQMWeldProQua.WireTypeBrandSpe.Trim().IndexOf("-"));
            DL_MaterialNo.SelectedValue = wPQMWeldProQua.MaterialNo.Trim().Substring(0, wPQMWeldProQua.MaterialNo.Trim().IndexOf("-"));
            DL_MaterialSpecification.SelectedValue = wPQMWeldProQua.MaterialSpecification.Trim().Substring(0, wPQMWeldProQua.MaterialSpecification.Trim().IndexOf("-"));
            DL_WeldingMethod.SelectedValue = wPQMWeldProQua.WeldingMethod.Trim().Substring(0, wPQMWeldProQua.WeldingMethod.Trim().IndexOf("-"));
            DL_WeldMaterialCategory.SelectedValue = wPQMWeldProQua.WeldMaterialCategory.Trim().Substring(0, wPQMWeldProQua.WeldMaterialCategory.Trim().IndexOf("-"));

            TB_AfterHotTemp.Text = string.IsNullOrEmpty(wPQMWeldProQua.AfterHotTemp) ? "" : wPQMWeldProQua.AfterHotTemp.Trim();
            TB_AfterHotTime.Text = string.IsNullOrEmpty(wPQMWeldProQua.AfterHotTime) ? "" : wPQMWeldProQua.AfterHotTime.Trim();
            TB_BackClearRootMethod.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackClearRootMethod) ? "" : wPQMWeldProQua.BackClearRootMethod.Trim();
            TB_BackGasFlowRate.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackGasFlowRate) ? "" : wPQMWeldProQua.BackGasFlowRate.Trim();
            TB_BackProtectiveGas.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackProtectiveGas) ? "" : wPQMWeldProQua.BackProtectiveGas.Trim();
            TB_BackProtectiveGasMixRatio.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackProtectiveGasMixRatio) ? "" : wPQMWeldProQua.BackProtectiveGasMixRatio.Trim();
            TB_CleanBefWelding.Text = string.IsNullOrEmpty(wPQMWeldProQua.CleanBefWelding) ? "" : wPQMWeldProQua.CleanBefWelding.Trim();
            TB_CurrentType.Text = string.IsNullOrEmpty(wPQMWeldProQua.CurrentType) ? "" : wPQMWeldProQua.CurrentType.Trim();
            TB_LayerClean.Text = string.IsNullOrEmpty(wPQMWeldProQua.LayerClean) ? "" : wPQMWeldProQua.LayerClean.Trim();
            TB_NozzleDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.NozzleDiameter) ? "" : wPQMWeldProQua.NozzleDiameter.Trim();
            TB_TailGasFlowRate.Text = string.IsNullOrEmpty(wPQMWeldProQua.TailGasFlowRate) ? "" : wPQMWeldProQua.TailGasFlowRate.Trim();
            TB_TailProtectiveGas.Text = string.IsNullOrEmpty(wPQMWeldProQua.TailProtectiveGas) ? "" : wPQMWeldProQua.TailProtectiveGas.Trim();
            TB_TailProtectiveGasMixRatio.Text = string.IsNullOrEmpty(wPQMWeldProQua.TailProtectiveGasMixRatio) ? "" : wPQMWeldProQua.TailProtectiveGasMixRatio.Trim();
            TB_TempMeasureMethod.Text = string.IsNullOrEmpty(wPQMWeldProQua.TempMeasureMethod) ? "" : wPQMWeldProQua.TempMeasureMethod.Trim();
            TB_TunElecDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.TunElecDiameter) ? "" : wPQMWeldProQua.TunElecDiameter.Trim();
            TB_TunElecType.Text = string.IsNullOrEmpty(wPQMWeldProQua.TunElecType) ? "" : wPQMWeldProQua.TunElecType.Trim();
            TB_WarmUpTime.Text = string.IsNullOrEmpty(wPQMWeldProQua.WarmUpTime) ? "" : wPQMWeldProQua.WarmUpTime.Trim();
            TB_WireSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.WireSpeed) ? "" : wPQMWeldProQua.WireSpeed.Trim();
            DL_CategoryGroups.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.CategoryGroups) ? "" : wPQMWeldProQua.CategoryGroups.Trim().Substring(0, wPQMWeldProQua.CategoryGroups.Trim().IndexOf("-"));
            DL_HeatingMode.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.HeatingMode) ? "" : wPQMWeldProQua.HeatingMode.Trim().Substring(0, wPQMWeldProQua.HeatingMode.Trim().IndexOf("-"));
            DL_WeldingDirection.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.WeldingDirection) ? "" : wPQMWeldProQua.WeldingDirection.Trim().Substring(0, wPQMWeldProQua.WeldingDirection.Trim().IndexOf("-"));
        }
        else
        {
            TB_ElecTypeBrandSpe.Text = "";
            DL_ElecBrand.SelectedValue = "";
            DL_ElecSpe.SelectedValue = "";
            DL_ElecType.SelectedValue = "";
            TB_FluxTypeBrandSpe.Text = "";
            DL_FluxBrand.SelectedValue = "";
            DL_FluxSpe.SelectedValue = "";
            DL_FluxType.SelectedValue = "";
            TB_LayerTemperature.Text = "";
            TB_LineEnergy.Text = "";
            TB_PreheatingTemperature.Text = "";
            TB_ProGasMixRatio.Text = "";
            TB_ProtectiveGas.Text = "";
            TB_ShieldingGasFlowRate.Text = "";
            TB_WeldingCurrent.Text = "";
            TB_WeldingPosition.Text = "";
            TB_WeldingSpeed.Text = "";
            TB_WeldingVoltage.Text = "";
            TB_WireTypeBrandSpe.Text = "";
            DL_WireBrand.SelectedValue = "";
            DL_WireSpe.SelectedValue = "";
            DL_WireType.SelectedValue = "";
            DL_MaterialNo.SelectedValue = "";
            DL_MaterialSpecification.SelectedValue = "";
            DL_WeldingMethod.SelectedValue = "";
            DL_WeldMaterialCategory.SelectedValue = "";
            TB_AfterHotTemp.Text = "";
            TB_AfterHotTime.Text = "";
            TB_BackClearRootMethod.Text = "";
            TB_BackGasFlowRate.Text = "";
            TB_BackProtectiveGas.Text = "";
            TB_BackProtectiveGasMixRatio.Text = "";
            TB_CleanBefWelding.Text = "";
            TB_CurrentType.Text = "";
            TB_LayerClean.Text = "";
            TB_NozzleDiameter.Text = "";
            TB_TailGasFlowRate.Text = "";
            TB_TailProtectiveGas.Text = "";
            TB_TailProtectiveGasMixRatio.Text = "";
            TB_TempMeasureMethod.Text = "";
            TB_TunElecDiameter.Text = "";
            TB_TunElecType.Text = "";
            TB_WarmUpTime.Text = "";
            TB_WireSpeed.Text = "";
            DL_CategoryGroups.SelectedValue = "";
            DL_HeatingMode.SelectedValue = "";
            DL_WeldingDirection.SelectedValue = "";
        }
    }
}