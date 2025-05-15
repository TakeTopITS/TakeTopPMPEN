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

public partial class TTWPQMWeldProQua : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","焊接工艺评定", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMAllDataName();
            LoadWPQMWeldProQuaList();
        }
    }

    protected void LoadWPQMAllDataName()
    {
        string strHQL;
        IList lst;
        WPQMAllDataBLL wPQMAllDataBLL = new WPQMAllDataBLL();
        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='Post-WeldHeatTreatmentMethod' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_AfterHot.DataSource = lst;
        DL_AfterHot.DataBind();
        DL_AfterHot.Items.Insert(0, new ListItem("--Select--", ""));

        DL_AfterHot1.DataSource = lst;
        DL_AfterHot1.DataBind();
        DL_AfterHot1.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='Post-WeldHeatTreatmentCategory' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_AfterWeldingClass.DataSource = lst;
        DL_AfterWeldingClass.DataBind();
        DL_AfterWeldingClass.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='ApplicableCategory' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_ApplicableCategories.DataSource = lst;
        DL_ApplicableCategories.DataBind();
        DL_ApplicableCategories.Items.Insert(0, new ListItem("--Select--", ""));

        DL_ApplicableCategories1.DataSource = lst;
        DL_ApplicableCategories1.DataBind();
        DL_ApplicableCategories1.Items.Insert(0, new ListItem("--Select--", ""));

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='BaseMaterialCategory' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_BaseClass.DataSource = lst;
        DL_BaseClass.DataBind();
        DL_BaseClass.Items.Insert(0, new ListItem("--Select--", ""));

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

    protected void LoadWPQMWeldProQuaList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMWeldProQua Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (MaterialNo like '%" + TextBox1.Text.Trim() + "%' or MaterialSpecification like '%" + TextBox1.Text.Trim() + "%' or WeldmentThickness like '%" + TextBox1.Text.Trim() + "%' "+
            "or BaseClass like '%" + TextBox1.Text.Trim() + "%' or AfterWeldingClass like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(DL_AfterHot1.SelectedValue) && DL_AfterHot1.SelectedValue.Trim() != "")
        {
            strHQL += " and AfterHot like '" + DL_AfterHot1.SelectedValue.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(DL_ApplicableCategories1.SelectedValue) && DL_ApplicableCategories1.SelectedValue.Trim() != "")
        {
            strHQL += " and ApplicableCategories like '" + DL_ApplicableCategories1.SelectedValue.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(DL_WeldingMethod1.SelectedValue) && DL_WeldingMethod1.SelectedValue.Trim() != "")
        {
            strHQL += " and WeldingMethod like '" + DL_WeldingMethod1.SelectedValue.Trim() + "%' ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldProQua");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        WPQMWeldProQua wPQMWeldProQua = new WPQMWeldProQua();
        wPQMWeldProQua.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
        wPQMWeldProQua.Code = GetWPQMWeldProQuaCode();
        TB_Code.Text = wPQMWeldProQua.Code.Trim();
        if (TB_Code.Text.Trim() == "" || string.IsNullOrEmpty(TB_Code.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSPDBMSCCWJC").ToString().Trim()+"')", true);
            TB_Code.Focus();
            return;
        }
        wPQMWeldProQua.ApplicableCategories = DL_ApplicableCategories.SelectedValue.Trim() + "-" + DL_ApplicableCategories.SelectedItem.Text.Trim();
        wPQMWeldProQua.MaterialNo = DL_MaterialNo.SelectedValue.Trim() + "-" + DL_MaterialNo.SelectedItem.Text.Trim();
        wPQMWeldProQua.MaterialSpecification = DL_MaterialSpecification.SelectedValue.Trim() + "-" + DL_MaterialSpecification.SelectedItem.Text.Trim();
        wPQMWeldProQua.WeldmentThickness = TB_WeldmentThickness.Text.Trim();
        wPQMWeldProQua.BaseClass = DL_BaseClass.SelectedValue.Trim() + "-" + DL_BaseClass.SelectedItem.Text.Trim();
        wPQMWeldProQua.GroupForm = TB_GroupForm.Text.Trim();
        wPQMWeldProQua.WeldMaterialCategory = DL_WeldMaterialCategory.SelectedValue.Trim() + "-" + DL_WeldMaterialCategory.SelectedItem.Text.Trim();
        wPQMWeldProQua.WeldingMethod = DL_WeldingMethod.SelectedValue.Trim() + "-" + DL_WeldingMethod.SelectedItem.Text.Trim();
        wPQMWeldProQua.WeldingPosition = TB_WeldingPosition.Text.Trim();
        wPQMWeldProQua.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
        wPQMWeldProQua.LayerTemperature = TB_LayerTemperature.Text.Trim();
        wPQMWeldProQua.AfterWeldingTem = TB_AfterWeldingTem.Text.Trim();
        wPQMWeldProQua.AfterWeldingPreTime = TB_AfterWeldingPreTime.Text.Trim();
        wPQMWeldProQua.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldProQua.WeldingVoltage = TB_WeldingVoltage.Text.Trim();
        TB_WireTypeBrandSpe.Text = DL_WireType.SelectedValue.Trim() + "-" + DL_WireType.SelectedItem.Text.Trim() + ";" + DL_WireBrand.SelectedValue.Trim() + "-" + DL_WireBrand.SelectedItem.Text.Trim() + ";" + DL_WireSpe.SelectedValue.Trim() + "-" + DL_WireSpe.SelectedItem.Text.Trim();
        wPQMWeldProQua.WireTypeBrandSpe = TB_WireTypeBrandSpe.Text.Trim();
        wPQMWeldProQua.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMWeldProQua.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMWeldProQua.ProtectiveGas = TB_ProtectiveGas.Text.Trim();
        TB_ElecTypeBrandSpe.Text = DL_ElecType.SelectedValue.Trim() + "-" + DL_ElecType.SelectedItem.Text.Trim() + ";" + DL_ElecBrand.SelectedValue.Trim() + "-" + DL_ElecBrand.SelectedItem.Text.Trim() + ";" + DL_ElecSpe.SelectedValue.Trim() + "-" + DL_ElecSpe.SelectedItem.Text.Trim();
        wPQMWeldProQua.ElecTypeBrandSpe = TB_ElecTypeBrandSpe.Text.Trim();
        wPQMWeldProQua.ProGasMixRatio = TB_ProGasMixRatio.Text.Trim();
        wPQMWeldProQua.ShieldingGasFlowRate = TB_ShieldingGasFlowRate.Text.Trim();
        TB_FluxTypeBrandSpe.Text = DL_FluxType.SelectedValue.Trim() + "-" + DL_FluxType.SelectedItem.Text.Trim() + ";" + DL_FluxBrand.SelectedValue.Trim() + "-" + DL_FluxBrand.SelectedItem.Text.Trim() + ";" + DL_FluxSpe.SelectedValue.Trim() + "-" + DL_FluxSpe.SelectedItem.Text.Trim();
        wPQMWeldProQua.FluxTypeBrandSpe = TB_FluxTypeBrandSpe.Text.Trim();
        wPQMWeldProQua.EvaluationProject = TB_EvaluationProject.Text.Trim();
        wPQMWeldProQua.MechanicalPerReq = TB_MechanicalPerReq.Text.Trim();
        wPQMWeldProQua.OtherPerReq = TB_OtherPerReq.Text.Trim();
        wPQMWeldProQua.AfterWeldingClass = DL_AfterWeldingClass.SelectedValue.Trim() + "-" + DL_AfterWeldingClass.SelectedItem.Text.Trim();
        wPQMWeldProQua.EnterCode = strUserCode.Trim();
        try
        {
            wPQMWeldProQuaBLL.AddWPQMWeldProQua(wPQMWeldProQua);
            LoadWPQMWeldProQuaList();

            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            HL_Complety.Visible = true;
            HL_Complety.Enabled = true;
            HL_Complety.NavigateUrl = "TTWPQMWeldProQuaAll.aspx?Code=" + TB_Code.Text.Trim() + "";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected string GetWPQMWeldProQuaCode()
    {
        string flag = string.Empty;
        string strCode = "PE" + DateTime.Now.ToString("yyyyMM");
        string strHQL = "Select Code From T_WPQMWeldProQua Where Code like '" + strCode + "%' Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldProQua").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(8)) + 1;
            if (pa >= 10 && pa < 100)
                flag = strCode + "00" + pa.ToString();
            else if (pa < 10)
                flag = strCode + "000" + pa.ToString();
            else if (pa >= 100 && pa < 1000)
                flag = strCode + "0" + pa.ToString();
            else if (pa >= 1000 && pa < 10000)
                flag = strCode + pa.ToString();
            else
                flag = "";
        }
        else
        {
            flag = strCode + "0001";
        }
        return flag;
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua where wPQMWeldProQua.Code = '" + TB_Code.Text.Trim() + "'";
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);

        WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];

        wPQMWeldProQua.AfterHot = DL_AfterHot.SelectedValue.Trim() + "-" + DL_AfterHot.SelectedItem.Text.Trim();
        wPQMWeldProQua.ApplicableCategories = DL_ApplicableCategories.SelectedValue.Trim() + "-" + DL_ApplicableCategories.SelectedItem.Text.Trim();
        wPQMWeldProQua.MaterialNo = DL_MaterialNo.SelectedValue.Trim() + "-" + DL_MaterialNo.SelectedItem.Text.Trim();
        wPQMWeldProQua.MaterialSpecification = DL_MaterialSpecification.SelectedValue.Trim() + "-" + DL_MaterialSpecification.SelectedItem.Text.Trim();
        wPQMWeldProQua.WeldmentThickness = TB_WeldmentThickness.Text.Trim();
        wPQMWeldProQua.BaseClass = DL_BaseClass.SelectedValue.Trim() + "-" + DL_BaseClass.SelectedItem.Text.Trim();
        wPQMWeldProQua.GroupForm = TB_GroupForm.Text.Trim();
        wPQMWeldProQua.WeldMaterialCategory = DL_WeldMaterialCategory.SelectedValue.Trim() + "-" + DL_WeldMaterialCategory.SelectedItem.Text.Trim();
        wPQMWeldProQua.WeldingMethod = DL_WeldingMethod.SelectedValue.Trim() + "-" + DL_WeldingMethod.SelectedItem.Text.Trim();
        wPQMWeldProQua.WeldingPosition = TB_WeldingPosition.Text.Trim();
        wPQMWeldProQua.PreheatingTemperature = TB_PreheatingTemperature.Text.Trim();
        wPQMWeldProQua.LayerTemperature = TB_LayerTemperature.Text.Trim();
        wPQMWeldProQua.AfterWeldingTem = TB_AfterWeldingTem.Text.Trim();
        wPQMWeldProQua.AfterWeldingPreTime = TB_AfterWeldingPreTime.Text.Trim();
        wPQMWeldProQua.WeldingCurrent = TB_WeldingCurrent.Text.Trim();
        wPQMWeldProQua.WeldingVoltage = TB_WeldingVoltage.Text.Trim();
        TB_WireTypeBrandSpe.Text = DL_WireType.SelectedValue.Trim() + "-" + DL_WireType.SelectedItem.Text.Trim() + ";" + DL_WireBrand.SelectedValue.Trim() + "-" + DL_WireBrand.SelectedItem.Text.Trim() + ";" + DL_WireSpe.SelectedValue.Trim() + "-" + DL_WireSpe.SelectedItem.Text.Trim();
        wPQMWeldProQua.WireTypeBrandSpe = TB_WireTypeBrandSpe.Text.Trim();
        wPQMWeldProQua.WeldingSpeed = TB_WeldingSpeed.Text.Trim();
        wPQMWeldProQua.LineEnergy = TB_LineEnergy.Text.Trim();
        wPQMWeldProQua.ProtectiveGas = TB_ProtectiveGas.Text.Trim();
        TB_ElecTypeBrandSpe.Text = DL_ElecType.SelectedValue.Trim() + "-" + DL_ElecType.SelectedItem.Text.Trim() + ";" + DL_ElecBrand.SelectedValue.Trim() + "-" + DL_ElecBrand.SelectedItem.Text.Trim() + ";" + DL_ElecSpe.SelectedValue.Trim() + "-" + DL_ElecSpe.SelectedItem.Text.Trim();
        wPQMWeldProQua.ElecTypeBrandSpe = TB_ElecTypeBrandSpe.Text.Trim();
        wPQMWeldProQua.ProGasMixRatio = TB_ProGasMixRatio.Text.Trim();
        wPQMWeldProQua.ShieldingGasFlowRate = TB_ShieldingGasFlowRate.Text.Trim();
        TB_FluxTypeBrandSpe.Text = DL_FluxType.SelectedValue.Trim() + "-" + DL_FluxType.SelectedItem.Text.Trim() + ";" + DL_FluxBrand.SelectedValue.Trim() + "-" + DL_FluxBrand.SelectedItem.Text.Trim() + ";" + DL_FluxSpe.SelectedValue.Trim() + "-" + DL_FluxSpe.SelectedItem.Text.Trim();
        wPQMWeldProQua.FluxTypeBrandSpe = TB_FluxTypeBrandSpe.Text.Trim();
        wPQMWeldProQua.EvaluationProject = TB_EvaluationProject.Text.Trim();
        wPQMWeldProQua.MechanicalPerReq = TB_MechanicalPerReq.Text.Trim();
        wPQMWeldProQua.OtherPerReq = TB_OtherPerReq.Text.Trim();
        wPQMWeldProQua.AfterWeldingClass = DL_AfterWeldingClass.SelectedValue.Trim() + "-" + DL_AfterWeldingClass.SelectedItem.Text.Trim();

        try
        {
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());

            LoadWPQMWeldProQuaList();

            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            HL_Complety.Visible = true;
            HL_Complety.Enabled = true;
            HL_Complety.NavigateUrl = "TTWPQMWeldProQuaAll.aspx?Code=" + TB_Code.Text.Trim() + "";

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
        string strCode = TB_Code.Text.Trim();

        strHQL = "Delete From T_WPQMWeldProQua Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMWeldProQuaList();

            BT_Update.Visible = false;
            BT_Delete.Visible = false;
            HL_Complety.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWPQMWeldProQuaList();
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

            strHQL = "From WPQMWeldProQua as wPQMWeldProQua where wPQMWeldProQua.Code = '" + strId + "'";
            WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
            lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);

            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];

            TB_Code.Text = wPQMWeldProQua.Code.Trim();
            TB_AfterWeldingPreTime.Text = wPQMWeldProQua.AfterWeldingPreTime.Trim();
            TB_AfterWeldingTem.Text = wPQMWeldProQua.AfterWeldingTem.Trim();

            TB_ElecTypeBrandSpe.Text = wPQMWeldProQua.ElecTypeBrandSpe.Trim();
            DL_ElecBrand.SelectedValue = wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(wPQMWeldProQua.ElecTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(wPQMWeldProQua.ElecTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            int K = wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf(";");
            int j = wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf("-");
            DL_ElecSpe.SelectedValue = wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldProQua.ElecTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_ElecType.SelectedValue = wPQMWeldProQua.ElecTypeBrandSpe.Trim().Substring(0,wPQMWeldProQua.ElecTypeBrandSpe.Trim().IndexOf("-"));

            TB_EvaluationProject.Text = wPQMWeldProQua.EvaluationProject.Trim();

            TB_FluxTypeBrandSpe.Text = wPQMWeldProQua.FluxTypeBrandSpe.Trim();
            DL_FluxBrand.SelectedValue = wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(wPQMWeldProQua.FluxTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(wPQMWeldProQua.FluxTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_FluxSpe.SelectedValue = wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(wPQMWeldProQua.FluxTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldProQua.FluxTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldProQua.FluxTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_FluxType.SelectedValue = wPQMWeldProQua.FluxTypeBrandSpe.Trim().Substring(0, wPQMWeldProQua.FluxTypeBrandSpe.Trim().IndexOf("-"));

            TB_GroupForm.Text = wPQMWeldProQua.GroupForm.Trim();
            TB_LayerTemperature.Text = wPQMWeldProQua.LayerTemperature.Trim();
            TB_LineEnergy.Text = wPQMWeldProQua.LineEnergy.Trim();
            TB_MechanicalPerReq.Text = wPQMWeldProQua.MechanicalPerReq.Trim();
            TB_OtherPerReq.Text = wPQMWeldProQua.OtherPerReq.Trim();
            TB_PreheatingTemperature.Text = wPQMWeldProQua.PreheatingTemperature.Trim();
            TB_ProGasMixRatio.Text = wPQMWeldProQua.ProGasMixRatio.Trim();
            TB_ProtectiveGas.Text = wPQMWeldProQua.ProtectiveGas.Trim();
            TB_ShieldingGasFlowRate.Text = wPQMWeldProQua.ShieldingGasFlowRate.Trim();
            TB_WeldingCurrent.Text = wPQMWeldProQua.WeldingCurrent.Trim();
            TB_WeldingPosition.Text = wPQMWeldProQua.WeldingPosition.Trim();
            TB_WeldingSpeed.Text = wPQMWeldProQua.WeldingSpeed.Trim();
            TB_WeldingVoltage.Text = wPQMWeldProQua.WeldingVoltage.Trim();
            TB_WeldmentThickness.Text = wPQMWeldProQua.WeldmentThickness.Trim();

            TB_WireTypeBrandSpe.Text = wPQMWeldProQua.WireTypeBrandSpe.Trim();
            DL_WireBrand.SelectedValue = wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(wPQMWeldProQua.WireTypeBrandSpe.Trim().IndexOf(";") + 1).Substring(0, wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(wPQMWeldProQua.WireTypeBrandSpe.Trim().IndexOf(";") + 1).IndexOf("-"));
            DL_WireSpe.SelectedValue = wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(wPQMWeldProQua.WireTypeBrandSpe.Trim().LastIndexOf(";") + 1, wPQMWeldProQua.WireTypeBrandSpe.Trim().LastIndexOf("-") - wPQMWeldProQua.WireTypeBrandSpe.Trim().LastIndexOf(";") - 1);
            DL_WireType.SelectedValue = wPQMWeldProQua.WireTypeBrandSpe.Trim().Substring(0, wPQMWeldProQua.WireTypeBrandSpe.Trim().IndexOf("-"));

            DL_AfterHot.SelectedValue = wPQMWeldProQua.AfterHot.Trim().Substring(0,wPQMWeldProQua.AfterHot.Trim().IndexOf("-"));
            DL_AfterWeldingClass.SelectedValue = wPQMWeldProQua.AfterWeldingClass.Trim().Substring(0, wPQMWeldProQua.AfterWeldingClass.Trim().IndexOf("-"));
            DL_ApplicableCategories.SelectedValue = wPQMWeldProQua.ApplicableCategories.Trim().Substring(0, wPQMWeldProQua.ApplicableCategories.Trim().IndexOf("-"));
            DL_BaseClass.SelectedValue = wPQMWeldProQua.BaseClass.Trim().Substring(0, wPQMWeldProQua.BaseClass.Trim().IndexOf("-"));
            DL_MaterialNo.SelectedValue = wPQMWeldProQua.MaterialNo.Trim().Substring(0, wPQMWeldProQua.MaterialNo.Trim().IndexOf("-"));
            DL_MaterialSpecification.SelectedValue = wPQMWeldProQua.MaterialSpecification.Trim().Substring(0, wPQMWeldProQua.MaterialSpecification.Trim().IndexOf("-"));
            DL_WeldingMethod.SelectedValue = wPQMWeldProQua.WeldingMethod.Trim().Substring(0, wPQMWeldProQua.WeldingMethod.Trim().IndexOf("-"));
            DL_WeldMaterialCategory.SelectedValue = wPQMWeldProQua.WeldMaterialCategory.Trim().Substring(0, wPQMWeldProQua.WeldMaterialCategory.Trim().IndexOf("-"));
            
            if (wPQMWeldProQua.EnterCode.Trim() == strUserCode.Trim())
            {
                BT_Delete.Visible = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
                HL_Complety.Visible = true;
                HL_Complety.Enabled = true;
                HL_Complety.NavigateUrl = "TTWPQMWeldProQuaAll.aspx?Code=" + TB_Code.Text.Trim()+"";
            }
            else
            {
                BT_Update.Visible = false;
                BT_Delete.Visible = false;
                HL_Complety.Visible = false;
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWeldProQua");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}