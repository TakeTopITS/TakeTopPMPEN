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

public partial class TTWPQMWeldProQuaAll : System.Web.UI.Page
{
    string strUserCode, strCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strCode = Request.QueryString["Code"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMAllDataName();
            LoadWPQMWeldProQuaList(strCode);
        }
    }

    protected void LoadWPQMAllDataName()
    {
        string strHQL;
        IList lst;
        WPQMAllDataBLL wPQMAllDataBLL = new WPQMAllDataBLL();
        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='冷却方法' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_CoolingMethod.DataSource = lst;
        DL_CoolingMethod.DataBind();
        DL_CoolingMethod.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='类别组号' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_CategoryGroups.DataSource = lst;
        DL_CategoryGroups.DataBind();
        DL_CategoryGroups.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='焊接方向' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WeldingDirection.DataSource = lst;
        DL_WeldingDirection.DataBind();
        DL_WeldingDirection.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='加热方式' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_HeatingMode.DataSource = lst;
        DL_HeatingMode.DataBind();
        DL_HeatingMode.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='焊材标准' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_WeldingMaterialStandard.DataSource = lst;
        DL_WeldingMaterialStandard.DataBind();
        DL_WeldingMaterialStandard.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMWeldProQuaList(string strcode)
    {
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua where wPQMWeldProQua.Code = '" + strcode + "'";
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            TB_AfterHotTemp.Text = string.IsNullOrEmpty(wPQMWeldProQua.AfterHotTemp) ? "" : wPQMWeldProQua.AfterHotTemp.Trim();
            TB_AfterHotTime.Text = string.IsNullOrEmpty(wPQMWeldProQua.AfterHotTime) ? "" : wPQMWeldProQua.AfterHotTime.Trim();
            TB_BackClearRootMethod.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackClearRootMethod) ? "" : wPQMWeldProQua.BackClearRootMethod.Trim();
            TB_BackGasFlowRate.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackGasFlowRate) ? "" : wPQMWeldProQua.BackGasFlowRate.Trim();
            TB_BackProtectiveGas.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackProtectiveGas) ? "" : wPQMWeldProQua.BackProtectiveGas.Trim();
            TB_BackProtectiveGasMixRatio.Text = string.IsNullOrEmpty(wPQMWeldProQua.BackProtectiveGasMixRatio) ? "" : wPQMWeldProQua.BackProtectiveGasMixRatio.Trim();
            TB_BoilingTemp.Text = string.IsNullOrEmpty(wPQMWeldProQua.BoilingTemp) ? "" : wPQMWeldProQua.BoilingTemp.Trim();
            TB_CleanBefWelding.Text = string.IsNullOrEmpty(wPQMWeldProQua.CleanBefWelding) ? "" : wPQMWeldProQua.CleanBefWelding.Trim();
            TB_Code.Text = wPQMWeldProQua.Code.Trim();
            TB_CoolingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.CoolingSpeed) ? "" : wPQMWeldProQua.CoolingSpeed.Trim();
            TB_CurrentType.Text = string.IsNullOrEmpty(wPQMWeldProQua.CurrentType) ? "" : wPQMWeldProQua.CurrentType.Trim();
            TB_EntityName.Text = string.IsNullOrEmpty(wPQMWeldProQua.EntityName) ? "" : wPQMWeldProQua.EntityName.Trim();
            TB_FilletWeldPosition.Text = string.IsNullOrEmpty(wPQMWeldProQua.FilletWeldPosition) ? "" : wPQMWeldProQua.FilletWeldPosition.Trim();
            TB_GrooveForm.Text = string.IsNullOrEmpty(wPQMWeldProQua.GrooveForm) ? "" : wPQMWeldProQua.GrooveForm.Trim();
            TB_HeatingSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.HeatingSpeed) ? "" : wPQMWeldProQua.HeatingSpeed.Trim();
            TB_HeatTreatmentMethod.Text = string.IsNullOrEmpty(wPQMWeldProQua.HeatTreatmentMethod) ? "" : wPQMWeldProQua.HeatTreatmentMethod.Trim();
            TB_HeatTreFurnModel.Text = string.IsNullOrEmpty(wPQMWeldProQua.HeatTreFurnModel) ? "" : wPQMWeldProQua.HeatTreFurnModel.Trim();
            TB_LayerClean.Text = string.IsNullOrEmpty(wPQMWeldProQua.LayerClean) ? "" : wPQMWeldProQua.LayerClean.Trim();
            TB_MechanizationDegree.Text = string.IsNullOrEmpty(wPQMWeldProQua.MechanizationDegree) ? "" : wPQMWeldProQua.MechanizationDegree.Trim();
            TB_NondestructiveTestReq.Text = string.IsNullOrEmpty(wPQMWeldProQua.NondestructiveTestReq) ? "" : wPQMWeldProQua.NondestructiveTestReq.Trim();
            TB_NozzleDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.NozzleDiameter) ? "" : wPQMWeldProQua.NozzleDiameter.Trim();
            NB_NumberSpecimens.Text = string.IsNullOrEmpty(wPQMWeldProQua.NumberSpecimens) ? "" : wPQMWeldProQua.NumberSpecimens.Trim();
            TB_OscillationParameters.Text = string.IsNullOrEmpty(wPQMWeldProQua.OscillationParameters) ? "" : wPQMWeldProQua.OscillationParameters.Trim();
            TB_PadMaterialSpec.Text = string.IsNullOrEmpty(wPQMWeldProQua.PadMaterialSpec) ? "" : wPQMWeldProQua.PadMaterialSpec.Trim();
            TB_Polarity.Text = string.IsNullOrEmpty(wPQMWeldProQua.Polarity) ? "" : wPQMWeldProQua.Polarity.Trim();
            TB_PWPSStandardNo.Text = string.IsNullOrEmpty(wPQMWeldProQua.PWPSStandardNo) ? "" : wPQMWeldProQua.PWPSStandardNo.Trim();
            TB_SpecificationDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.SpecificationDiameter) ? "" : wPQMWeldProQua.SpecificationDiameter.Trim();
            TB_SpecificationThickness.Text = string.IsNullOrEmpty(wPQMWeldProQua.SpecificationThickness) ? "" : wPQMWeldProQua.SpecificationThickness.Trim();
            TB_TailGasFlowRate.Text = string.IsNullOrEmpty(wPQMWeldProQua.TailGasFlowRate) ? "" : wPQMWeldProQua.TailGasFlowRate.Trim();
            TB_TailProtectiveGas.Text = string.IsNullOrEmpty(wPQMWeldProQua.TailProtectiveGas) ? "" : wPQMWeldProQua.TailProtectiveGas.Trim();
            TB_TailProtectiveGasMixRatio.Text = string.IsNullOrEmpty(wPQMWeldProQua.TailProtectiveGasMixRatio) ? "" : wPQMWeldProQua.TailProtectiveGasMixRatio.Trim();
            TB_TempMeasureMethod.Text = string.IsNullOrEmpty(wPQMWeldProQua.TempMeasureMethod) ? "" : wPQMWeldProQua.TempMeasureMethod.Trim();
            TB_TunElecDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.TunElecDiameter) ? "" : wPQMWeldProQua.TunElecDiameter.Trim();
            TB_TunElecType.Text = string.IsNullOrEmpty(wPQMWeldProQua.TunElecType) ? "" : wPQMWeldProQua.TunElecType.Trim();
            TB_Value_1.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_1) ? "" : wPQMWeldProQua.Value_1.Trim();
            TB_Value_2.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_2) ? "" : wPQMWeldProQua.Value_2.Trim();
            TB_Value_3.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_3) ? "" : wPQMWeldProQua.Value_3.Trim();
            TB_Value_4.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_4) ? "" : wPQMWeldProQua.Value_4.Trim();
            TB_Value_5.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_5) ? "" : wPQMWeldProQua.Value_5.Trim();
            TB_Value_6.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_6) ? "" : wPQMWeldProQua.Value_6.Trim();
            TB_Value_7.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_7) ? "" : wPQMWeldProQua.Value_7.Trim();
            TB_VerticalWeldingDirection.Text = string.IsNullOrEmpty(wPQMWeldProQua.VerticalWeldingDirection) ? "" : wPQMWeldProQua.VerticalWeldingDirection.Trim();
            TB_WarmUpTime.Text = string.IsNullOrEmpty(wPQMWeldProQua.WarmUpTime) ? "" : wPQMWeldProQua.WarmUpTime.Trim();
            TB_WireSpeed.Text = string.IsNullOrEmpty(wPQMWeldProQua.WireSpeed) ? "" : wPQMWeldProQua.WireSpeed.Trim();
            DL_CategoryGroups.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.CategoryGroups) ? "" : wPQMWeldProQua.CategoryGroups.Trim().Substring(0, wPQMWeldProQua.CategoryGroups.Trim().IndexOf("-"));
            DL_CoolingMethod.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.CoolingMethod) ? "" : wPQMWeldProQua.CoolingMethod.Trim().Substring(0, wPQMWeldProQua.CoolingMethod.Trim().IndexOf("-"));
            DL_HeatingMode.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.HeatingMode) ? "" : wPQMWeldProQua.HeatingMode.Trim().Substring(0, wPQMWeldProQua.HeatingMode.Trim().IndexOf("-"));
            DL_PassWeldingType.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.PassWeldingType) ? "请选择" : wPQMWeldProQua.PassWeldingType.Trim();
            DL_SwingType.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.SwingType) ? "请选择" : wPQMWeldProQua.SwingType.Trim();
            DL_WeldingArcType.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.WeldingArcType) ? "请选择" : wPQMWeldProQua.WeldingArcType.Trim();
            DL_WeldingDirection.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.WeldingDirection) ? "" : wPQMWeldProQua.WeldingDirection.Trim().Substring(0, wPQMWeldProQua.WeldingDirection.Trim().IndexOf("-"));
            DL_WeldingMaterialStandard.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.WeldingMaterialStandard) ? "" : wPQMWeldProQua.WeldingMaterialStandard.Trim().Substring(0, wPQMWeldProQua.WeldingMaterialStandard.Trim().IndexOf("-"));
            DL_WireWeldingType.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.WireWeldingType) ? "请选择" : wPQMWeldProQua.WireWeldingType.Trim();
            lbl_WeldedJointDiagram.Text = string.IsNullOrEmpty(wPQMWeldProQua.WeldedJointDiagram) ? "" : wPQMWeldProQua.WeldedJointDiagram.Trim();
        }
        else
        {
            TB_AfterHotTemp.Text = "";
            TB_AfterHotTime.Text = "";
            TB_BackClearRootMethod.Text = "";
            TB_BackGasFlowRate.Text = "";
            TB_BackProtectiveGas.Text = "";
            TB_BackProtectiveGasMixRatio.Text = "";
            TB_BoilingTemp.Text = "";
            TB_CleanBefWelding.Text = "";
            TB_Code.Text = "";
            TB_CoolingSpeed.Text = "";
            TB_CurrentType.Text = "";
            TB_EntityName.Text = "";
            TB_FilletWeldPosition.Text = "";
            TB_GrooveForm.Text = "";
            TB_HeatingSpeed.Text = "";
            TB_HeatTreatmentMethod.Text = "";
            TB_HeatTreFurnModel.Text = "";
            TB_LayerClean.Text = "";
            TB_MechanizationDegree.Text = "";
            TB_NondestructiveTestReq.Text = "";
            TB_NozzleDiameter.Text = "";
            NB_NumberSpecimens.Text = "";
            TB_OscillationParameters.Text = "";
            TB_PadMaterialSpec.Text = "";
            TB_Polarity.Text = "";
            TB_PWPSStandardNo.Text = "";
            TB_SpecificationDiameter.Text = "";
            TB_SpecificationThickness.Text = "";
            TB_TailGasFlowRate.Text = "";
            TB_TailProtectiveGas.Text = "";
            TB_TailProtectiveGasMixRatio.Text = "";
            TB_TempMeasureMethod.Text = "";
            TB_TunElecDiameter.Text = "";
            TB_TunElecType.Text = "";
            TB_Value_1.Text = "";
            TB_Value_2.Text = "";
            TB_Value_3.Text = "";
            TB_Value_4.Text = "";
            TB_Value_5.Text = "";
            TB_Value_6.Text = "";
            TB_Value_7.Text = "";
            TB_VerticalWeldingDirection.Text = "";
            TB_WarmUpTime.Text = "";
            TB_WireSpeed.Text = "";
            DL_CategoryGroups.SelectedValue = "";
            DL_CoolingMethod.SelectedValue = "";
            DL_HeatingMode.SelectedValue = "";
            DL_PassWeldingType.SelectedValue = "请选择";
            DL_SwingType.SelectedValue = "请选择";
            DL_WeldingArcType.SelectedValue = "请选择";
            DL_WeldingDirection.SelectedValue = "";
            DL_WeldingMaterialStandard.SelectedValue = "";
            DL_WireWeldingType.SelectedValue = "请选择";
            lbl_WeldedJointDiagram.Text = "";
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

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || TB_Code.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTSGYPDBMWKCZSBJC+"')", true);
            TB_Code.Focus();
            return;
        }

        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "from WPQMWeldProQua as wPQMWeldProQua where wPQMWeldProQua.Code = '" + TB_Code.Text.Trim() + "' ";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];

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
                wPQMWeldProQua.WeldedJointDiagram = strAttach;
            }
            wPQMWeldProQua.AfterHotTemp = TB_AfterHotTemp.Text.Trim();
            wPQMWeldProQua.AfterHotTime = TB_AfterHotTime.Text.Trim();
            wPQMWeldProQua.BackClearRootMethod = TB_BackClearRootMethod.Text.Trim();
            wPQMWeldProQua.BackGasFlowRate = TB_BackGasFlowRate.Text.Trim();
            wPQMWeldProQua.BackProtectiveGas = TB_BackProtectiveGas.Text.Trim();
            wPQMWeldProQua.BackProtectiveGasMixRatio = TB_BackProtectiveGasMixRatio.Text.Trim();
            wPQMWeldProQua.BoilingTemp = TB_BoilingTemp.Text.Trim();
            wPQMWeldProQua.CleanBefWelding = TB_CleanBefWelding.Text.Trim();
            wPQMWeldProQua.CoolingSpeed = TB_CoolingSpeed.Text.Trim();
            wPQMWeldProQua.CurrentType = TB_CurrentType.Text.Trim();
            wPQMWeldProQua.EntityName = TB_EntityName.Text.Trim();
            wPQMWeldProQua.FilletWeldPosition = TB_FilletWeldPosition.Text.Trim();
            wPQMWeldProQua.GrooveForm = TB_GrooveForm.Text.Trim();
            wPQMWeldProQua.HeatingSpeed = TB_HeatingSpeed.Text.Trim();
            wPQMWeldProQua.HeatTreatmentMethod = TB_HeatTreatmentMethod.Text.Trim();
            wPQMWeldProQua.HeatTreFurnModel = TB_HeatTreFurnModel.Text.Trim();
            wPQMWeldProQua.LayerClean = TB_LayerClean.Text.Trim();
            wPQMWeldProQua.MechanizationDegree = TB_MechanizationDegree.Text.Trim();
            wPQMWeldProQua.NondestructiveTestReq = TB_NondestructiveTestReq.Text.Trim();
            wPQMWeldProQua.NozzleDiameter = TB_NozzleDiameter.Text.Trim();
            wPQMWeldProQua.NumberSpecimens = NB_NumberSpecimens.Text.Trim();
            wPQMWeldProQua.OscillationParameters = TB_OscillationParameters.Text.Trim();
            wPQMWeldProQua.PadMaterialSpec = TB_PadMaterialSpec.Text.Trim();
            wPQMWeldProQua.Polarity = TB_Polarity.Text.Trim();
            wPQMWeldProQua.PWPSStandardNo = TB_PWPSStandardNo.Text.Trim();
            wPQMWeldProQua.SpecificationDiameter = TB_SpecificationDiameter.Text.Trim();
            wPQMWeldProQua.SpecificationThickness = TB_SpecificationThickness.Text.Trim();
            wPQMWeldProQua.TailGasFlowRate = TB_TailGasFlowRate.Text.Trim();
            wPQMWeldProQua.TailProtectiveGas = TB_TailProtectiveGas.Text.Trim();
            wPQMWeldProQua.TailProtectiveGasMixRatio = TB_TailProtectiveGasMixRatio.Text.Trim();
            wPQMWeldProQua.TempMeasureMethod = TB_TempMeasureMethod.Text.Trim();
            wPQMWeldProQua.TunElecDiameter = TB_TunElecDiameter.Text.Trim();
            wPQMWeldProQua.TunElecType = TB_TunElecType.Text.Trim();
            wPQMWeldProQua.Value_1 = TB_Value_1.Text.Trim();
            wPQMWeldProQua.Value_2 = TB_Value_2.Text.Trim();
            wPQMWeldProQua.Value_3 = TB_Value_3.Text.Trim();
            wPQMWeldProQua.Value_4 = TB_Value_4.Text.Trim();
            wPQMWeldProQua.Value_5 = TB_Value_5.Text.Trim();
            wPQMWeldProQua.Value_6 = TB_Value_6.Text.Trim();
            wPQMWeldProQua.Value_7 = TB_Value_7.Text.Trim();
            wPQMWeldProQua.VerticalWeldingDirection = TB_VerticalWeldingDirection.Text.Trim();
            wPQMWeldProQua.WarmUpTime = TB_WarmUpTime.Text.Trim();
            wPQMWeldProQua.WireSpeed = TB_WireSpeed.Text.Trim();
            wPQMWeldProQua.CategoryGroups = DL_CategoryGroups.SelectedValue.Trim() + "-" + DL_CategoryGroups.SelectedItem.Text.Trim();
            wPQMWeldProQua.CoolingMethod = DL_CoolingMethod.SelectedValue.Trim() + "-" + DL_CoolingMethod.SelectedItem.Text.Trim();
            wPQMWeldProQua.HeatingMode = DL_HeatingMode.SelectedValue.Trim() + "-" + DL_HeatingMode.SelectedItem.Text.Trim();
            wPQMWeldProQua.PassWeldingType = DL_PassWeldingType.SelectedValue.Trim();
            wPQMWeldProQua.SwingType = DL_SwingType.SelectedValue.Trim();
            wPQMWeldProQua.WeldingArcType = DL_WeldingArcType.SelectedValue.Trim();
            wPQMWeldProQua.WeldingDirection = DL_WeldingDirection.SelectedValue.Trim() + "-" + DL_WeldingDirection.SelectedItem.Text.Trim();
            wPQMWeldProQua.WeldingMaterialStandard = DL_WeldingMaterialStandard.SelectedValue.Trim() + "-" + DL_WeldingMaterialStandard.SelectedItem.Text.Trim();
            wPQMWeldProQua.WireWeldingType = DL_WireWeldingType.SelectedValue.Trim();

            try
            {
                wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXGSB+"')", true);
            }
        }
    }
}