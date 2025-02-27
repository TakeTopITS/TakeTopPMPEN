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

public partial class TTWPQMWorkReviewCommit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","工评任务书", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWPQMWeldProQuaName();
            LoadWPQMAllDataName();
            LoadWPQMWorkReviewCommitList();
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

        strHQL = "From WPQMAllData as wPQMAllData Where wPQMAllData.Type='CoolingMethod' Order By wPQMAllData.ID Desc";
        lst = wPQMAllDataBLL.GetAllWPQMAllDatas(strHQL);

        DL_CoolingMethod.DataSource = lst;
        DL_CoolingMethod.DataBind();
        DL_CoolingMethod.Items.Insert(0, new ListItem("--Select--", ""));

        DL_CoolingMethod1.DataSource = lst;
        DL_CoolingMethod1.DataBind();
        DL_CoolingMethod1.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMWorkReviewCommitList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMWorkReviewCommit Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or EvaluationPurposes like '%" + TextBox1.Text.Trim() + "%' or SpecificationThickness like '%" + TextBox1.Text.Trim() + "%' " +
            "or SpecificationDiameter like '%" + TextBox1.Text.Trim() + "%' or SpecificationPad like '%" + TextBox1.Text.Trim() + "%' or SpecificationOther like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(DL_CoolingMethod1.SelectedValue) && DL_CoolingMethod1.SelectedValue.Trim() != "")
        {
            strHQL += " and CoolingMethod like '" + DL_CoolingMethod1.SelectedValue.Trim() + "%' ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWorkReviewCommit");

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
        if (IsWPQMWorkReviewCommit(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGGPRWSYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMWorkReviewCommitBLL wPQMWorkReviewCommitBLL = new WPQMWorkReviewCommitBLL();
        WPQMWorkReviewCommit wPQMWorkReviewCommit = new WPQMWorkReviewCommit();

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
            wPQMWorkReviewCommit.DetailWeldSizePath = "";
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
            wPQMWorkReviewCommit.DetailWeldSizePath = strAttach;
        }

        wPQMWorkReviewCommit.AfterWeldingClass = TB_AfterWeldingClass.Text.Trim();
        wPQMWorkReviewCommit.AfterWeldingPreTime = TB_AfterWeldingPreTime.Text.Trim();
        wPQMWorkReviewCommit.AfterWeldingTem = TB_AfterWeldingTem.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_C = TB_ChemicalComp_C.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Cr = TB_ChemicalComp_Cr.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Mn = TB_ChemicalComp_Mn.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Mo = TB_ChemicalComp_Mo.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Nb = TB_ChemicalComp_Nb.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Ni = TB_ChemicalComp_Ni.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_P = TB_ChemicalComp_P.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_S = TB_ChemicalComp_S.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Si = TB_ChemicalComp_Si.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Ti = TB_ChemicalComp_Ti.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_V = TB_ChemicalComp_V.Text.Trim();
        wPQMWorkReviewCommit.CoolingMethod = DL_CoolingMethod.SelectedValue.Trim() + "-" + DL_CoolingMethod.SelectedItem.Text.Trim();
        wPQMWorkReviewCommit.EnterCode = strUserCode.Trim();
        wPQMWorkReviewCommit.EvaluationPurposes = TB_EvaluationPurposes.Text.Trim();
        wPQMWorkReviewCommit.Flattening = TB_Flattening.Text.Trim();
        wPQMWorkReviewCommit.Fracture = TB_Fracture.Text.Trim();
        wPQMWorkReviewCommit.HamBreak = TB_HamBreak.Text.Trim();
        wPQMWorkReviewCommit.HardnessHRB = TB_HardnessHRB.Text.Trim();
        wPQMWorkReviewCommit.HardnessHRC = TB_HardnessHRC.Text.Trim();
        wPQMWorkReviewCommit.HardnessHV = TB_HardnessHV.Text.Trim();
        wPQMWorkReviewCommit.ImpactHeatZone = TB_ImpactHeatZone.Text.Trim();
        wPQMWorkReviewCommit.ImpactMetalArea = TB_ImpactMetalArea.Text.Trim();
        wPQMWorkReviewCommit.ImpactWeldZone = TB_ImpactWeldZone.Text.Trim();
        wPQMWorkReviewCommit.IntergranularCorrTest = TB_IntergranularCorrTest.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerBack = TB_MechanicalPerBack.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerBend = TB_MechanicalPerBend.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerReqRel = TB_MechanicalPerReqRel.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerReqRm = TB_MechanicalPerReqRm.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerScol = TB_MechanicalPerScol.Text.Trim();
        wPQMWorkReviewCommit.Metallographic = TB_Metallographic.Text.Trim();
        wPQMWorkReviewCommit.MTEvaluationCriteria = TB_MTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.MTInsProportion = TB_MTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.MTQualifiedLevel = TB_MTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.PTEvaluationCriteria = TB_PTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.PTInsProportion = TB_PTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.PTQualifiedLevel = TB_PTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.Remark = TB_Remark.Text.Trim();
        wPQMWorkReviewCommit.RTEvaluationCriteria = TB_RTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.RTInsProportion = TB_RTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.RTQualifiedLevel = TB_RTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.ShockTemperature = TB_ShockTemperature.Text.Trim();
        wPQMWorkReviewCommit.SpecificationDiameter = TB_SpecificationDiameter.Text.Trim();
        wPQMWorkReviewCommit.SpecificationOther = TB_SpecificationOther.Text.Trim();
        wPQMWorkReviewCommit.SpecificationOtherReq = TB_SpecificationOtherReq.Text.Trim();
        wPQMWorkReviewCommit.SpecificationPad = TB_SpecificationPad.Text.Trim();
        wPQMWorkReviewCommit.SpecificationThickness = TB_SpecificationThickness.Text.Trim();
        wPQMWorkReviewCommit.UTEvaluationCriteria = TB_UTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.UTInsProportion = TB_UTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.UTQualifiedLevel = TB_UTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.Value_1 = TB_Value_1.Text.Trim();
        wPQMWorkReviewCommit.Value_2 = TB_Value_2.Text.Trim();
        wPQMWorkReviewCommit.Value_3 = TB_Value_3.Text.Trim();
        wPQMWorkReviewCommit.Value_4 = TB_Value_4.Text.Trim();
        wPQMWorkReviewCommit.Value_5 = TB_Value_5.Text.Trim();
        wPQMWorkReviewCommit.Value_6 = TB_Value_6.Text.Trim();
        wPQMWorkReviewCommit.Value_7 = TB_Value_7.Text.Trim();
        wPQMWorkReviewCommit.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        
        try
        {
            wPQMWorkReviewCommitBLL.AddWPQMWorkReviewCommit(wPQMWorkReviewCommit);
            lbl_ID.Text = GetMaxWPQMWorkReviewCommitID(wPQMWorkReviewCommit).ToString();
            UpdateWPQMWeldProQuaData(wPQMWorkReviewCommit.WeldProCode.Trim());
            LoadWPQMWorkReviewCommitList();

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
            wPQMWeldProQua.SpecificationThickness = TB_SpecificationThickness.Text.Trim();
            wPQMWeldProQua.SpecificationDiameter = TB_SpecificationDiameter.Text.Trim();
            wPQMWeldProQua.PadMaterialSpec = TB_SpecificationPad.Text.Trim();
            wPQMWeldProQua.Value_1 = TB_Value_1.Text.Trim();
            wPQMWeldProQua.Value_2 = TB_Value_2.Text.Trim();
            wPQMWeldProQua.Value_3 = TB_Value_3.Text.Trim();
            wPQMWeldProQua.Value_4 = TB_Value_4.Text.Trim();
            wPQMWeldProQua.Value_5 = TB_Value_5.Text.Trim();
            wPQMWeldProQua.Value_6 = TB_Value_6.Text.Trim();
            wPQMWeldProQua.Value_7 = TB_Value_7.Text.Trim();
            wPQMWeldProQua.AfterWeldingClass = TB_AfterWeldingClass.Text.Trim();
            wPQMWeldProQua.AfterWeldingPreTime = TB_AfterWeldingPreTime.Text.Trim();
            wPQMWeldProQua.AfterWeldingTem = TB_AfterWeldingTem.Text.Trim();
            wPQMWeldProQua.CoolingMethod = DL_CoolingMethod.SelectedValue.Trim() + "-" + DL_CoolingMethod.SelectedItem.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }

    protected int GetMaxWPQMWorkReviewCommitID(WPQMWorkReviewCommit bmbp)
    {
        string strHQL = "Select ID From T_WPQMWorkReviewCommit where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWorkReviewCommit").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMWorkReviewCommit(string strWeldProCode, string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMWorkReviewCommit Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMWorkReviewCommit Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWorkReviewCommit").Tables[0];
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
        if (IsWPQMWorkReviewCommit(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGGPRWSYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMWorkReviewCommit as wPQMWorkReviewCommit where wPQMWorkReviewCommit.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMWorkReviewCommitBLL wPQMWorkReviewCommitBLL = new WPQMWorkReviewCommitBLL();
        IList lst = wPQMWorkReviewCommitBLL.GetAllWPQMWorkReviewCommits(strHQL);

        WPQMWorkReviewCommit wPQMWorkReviewCommit = (WPQMWorkReviewCommit)lst[0];

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
            wPQMWorkReviewCommit.DetailWeldSizePath = strAttach;
        }

        wPQMWorkReviewCommit.AfterWeldingClass = TB_AfterWeldingClass.Text.Trim();
        wPQMWorkReviewCommit.AfterWeldingPreTime = TB_AfterWeldingPreTime.Text.Trim();
        wPQMWorkReviewCommit.AfterWeldingTem = TB_AfterWeldingTem.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_C = TB_ChemicalComp_C.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Cr = TB_ChemicalComp_Cr.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Mn = TB_ChemicalComp_Mn.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Mo = TB_ChemicalComp_Mo.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Nb = TB_ChemicalComp_Nb.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Ni = TB_ChemicalComp_Ni.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_P = TB_ChemicalComp_P.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_S = TB_ChemicalComp_S.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Si = TB_ChemicalComp_Si.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_Ti = TB_ChemicalComp_Ti.Text.Trim();
        wPQMWorkReviewCommit.ChemicalComp_V = TB_ChemicalComp_V.Text.Trim();
        wPQMWorkReviewCommit.CoolingMethod = DL_CoolingMethod.SelectedValue.Trim() + "-" + DL_CoolingMethod.SelectedItem.Text.Trim();
        wPQMWorkReviewCommit.EvaluationPurposes = TB_EvaluationPurposes.Text.Trim();
        wPQMWorkReviewCommit.Flattening = TB_Flattening.Text.Trim();
        wPQMWorkReviewCommit.Fracture = TB_Fracture.Text.Trim();
        wPQMWorkReviewCommit.HamBreak = TB_HamBreak.Text.Trim();
        wPQMWorkReviewCommit.HardnessHRB = TB_HardnessHRB.Text.Trim();
        wPQMWorkReviewCommit.HardnessHRC = TB_HardnessHRC.Text.Trim();
        wPQMWorkReviewCommit.HardnessHV = TB_HardnessHV.Text.Trim();
        wPQMWorkReviewCommit.ImpactHeatZone = TB_ImpactHeatZone.Text.Trim();
        wPQMWorkReviewCommit.ImpactMetalArea = TB_ImpactMetalArea.Text.Trim();
        wPQMWorkReviewCommit.ImpactWeldZone = TB_ImpactWeldZone.Text.Trim();
        wPQMWorkReviewCommit.IntergranularCorrTest = TB_IntergranularCorrTest.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerBack = TB_MechanicalPerBack.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerBend = TB_MechanicalPerBend.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerReqRel = TB_MechanicalPerReqRel.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerReqRm = TB_MechanicalPerReqRm.Text.Trim();
        wPQMWorkReviewCommit.MechanicalPerScol = TB_MechanicalPerScol.Text.Trim();
        wPQMWorkReviewCommit.Metallographic = TB_Metallographic.Text.Trim();
        wPQMWorkReviewCommit.MTEvaluationCriteria = TB_MTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.MTInsProportion = TB_MTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.MTQualifiedLevel = TB_MTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.PTEvaluationCriteria = TB_PTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.PTInsProportion = TB_PTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.PTQualifiedLevel = TB_PTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.Remark = TB_Remark.Text.Trim();
        wPQMWorkReviewCommit.RTEvaluationCriteria = TB_RTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.RTInsProportion = TB_RTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.RTQualifiedLevel = TB_RTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.ShockTemperature = TB_ShockTemperature.Text.Trim();
        wPQMWorkReviewCommit.SpecificationDiameter = TB_SpecificationDiameter.Text.Trim();
        wPQMWorkReviewCommit.SpecificationOther = TB_SpecificationOther.Text.Trim();
        wPQMWorkReviewCommit.SpecificationOtherReq = TB_SpecificationOtherReq.Text.Trim();
        wPQMWorkReviewCommit.SpecificationPad = TB_SpecificationPad.Text.Trim();
        wPQMWorkReviewCommit.SpecificationThickness = TB_SpecificationThickness.Text.Trim();
        wPQMWorkReviewCommit.UTEvaluationCriteria = TB_UTEvaluationCriteria.Text.Trim();
        wPQMWorkReviewCommit.UTInsProportion = TB_UTInsProportion.Text.Trim();
        wPQMWorkReviewCommit.UTQualifiedLevel = TB_UTQualifiedLevel.Text.Trim();
        wPQMWorkReviewCommit.Value_1 = TB_Value_1.Text.Trim();
        wPQMWorkReviewCommit.Value_2 = TB_Value_2.Text.Trim();
        wPQMWorkReviewCommit.Value_3 = TB_Value_3.Text.Trim();
        wPQMWorkReviewCommit.Value_4 = TB_Value_4.Text.Trim();
        wPQMWorkReviewCommit.Value_5 = TB_Value_5.Text.Trim();
        wPQMWorkReviewCommit.Value_6 = TB_Value_6.Text.Trim();
        wPQMWorkReviewCommit.Value_7 = TB_Value_7.Text.Trim();
        wPQMWorkReviewCommit.WeldProCode = DL_WeldProCode.SelectedValue.Trim();

        try
        {
            wPQMWorkReviewCommitBLL.UpdateWPQMWorkReviewCommit(wPQMWorkReviewCommit, wPQMWorkReviewCommit.ID);
            UpdateWPQMWeldProQuaData(wPQMWorkReviewCommit.WeldProCode.Trim());
            LoadWPQMWorkReviewCommitList();

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

        strHQL = "Delete From T_WPQMWorkReviewCommit Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadWPQMWorkReviewCommitList();

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
        LoadWPQMWorkReviewCommitList();
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

            strHQL = "From WPQMWorkReviewCommit as wPQMWorkReviewCommit where wPQMWorkReviewCommit.ID = '" + strId + "'";
            WPQMWorkReviewCommitBLL wPQMWorkReviewCommitBLL = new WPQMWorkReviewCommitBLL();
            lst = wPQMWorkReviewCommitBLL.GetAllWPQMWorkReviewCommits(strHQL);

            WPQMWorkReviewCommit wPQMWorkReviewCommit = (WPQMWorkReviewCommit)lst[0];

            lbl_ID.Text = wPQMWorkReviewCommit.ID.ToString();
            TB_AfterWeldingClass.Text = wPQMWorkReviewCommit.AfterWeldingClass.Trim();
            TB_AfterWeldingPreTime.Text = wPQMWorkReviewCommit.AfterWeldingPreTime.Trim();
            TB_AfterWeldingTem.Text = wPQMWorkReviewCommit.AfterWeldingTem.Trim();
            TB_ChemicalComp_C.Text = wPQMWorkReviewCommit.ChemicalComp_C.Trim();
            TB_ChemicalComp_Cr.Text = wPQMWorkReviewCommit.ChemicalComp_Cr.Trim();
            TB_ChemicalComp_Mn.Text = wPQMWorkReviewCommit.ChemicalComp_Mn.Trim();
            TB_ChemicalComp_Mo.Text = wPQMWorkReviewCommit.ChemicalComp_Mo.Trim();
            TB_ChemicalComp_Nb.Text = wPQMWorkReviewCommit.ChemicalComp_Nb.Trim();
            TB_ChemicalComp_Ni.Text = wPQMWorkReviewCommit.ChemicalComp_Ni.Trim();
            TB_ChemicalComp_P.Text = wPQMWorkReviewCommit.ChemicalComp_P.Trim();
            TB_ChemicalComp_S.Text = wPQMWorkReviewCommit.ChemicalComp_S.Trim();
            TB_ChemicalComp_Si.Text = wPQMWorkReviewCommit.ChemicalComp_Si.Trim();
            TB_ChemicalComp_Ti.Text = wPQMWorkReviewCommit.ChemicalComp_Ti.Trim();
            TB_ChemicalComp_V.Text = wPQMWorkReviewCommit.ChemicalComp_V.Trim();
            DL_CoolingMethod.SelectedValue = wPQMWorkReviewCommit.CoolingMethod.Trim().Substring(0, wPQMWorkReviewCommit.CoolingMethod.Trim().IndexOf("-"));
            TB_EvaluationPurposes.Text = wPQMWorkReviewCommit.EvaluationPurposes.Trim();
            TB_Flattening.Text = wPQMWorkReviewCommit.Flattening.Trim();
            TB_Fracture.Text = wPQMWorkReviewCommit.Fracture.Trim();
            TB_HamBreak.Text = wPQMWorkReviewCommit.HamBreak.Trim();
            TB_HardnessHRB.Text = wPQMWorkReviewCommit.HardnessHRB.Trim();
            TB_HardnessHRC.Text = wPQMWorkReviewCommit.HardnessHRC.Trim();
            TB_HardnessHV.Text = wPQMWorkReviewCommit.HardnessHV.Trim();
            TB_ImpactHeatZone.Text = wPQMWorkReviewCommit.ImpactHeatZone.Trim();
            TB_ImpactMetalArea.Text = wPQMWorkReviewCommit.ImpactMetalArea.Trim();
            TB_ImpactWeldZone.Text = wPQMWorkReviewCommit.ImpactWeldZone.Trim();
            TB_IntergranularCorrTest.Text = wPQMWorkReviewCommit.IntergranularCorrTest.Trim();
            TB_MechanicalPerBack.Text = wPQMWorkReviewCommit.MechanicalPerBack.Trim();
            TB_MechanicalPerBend.Text = wPQMWorkReviewCommit.MechanicalPerBend.Trim();
            TB_MechanicalPerReqRel.Text = wPQMWorkReviewCommit.MechanicalPerReqRel.Trim();
            TB_MechanicalPerReqRm.Text = wPQMWorkReviewCommit.MechanicalPerReqRm.Trim();
            TB_MechanicalPerScol.Text = wPQMWorkReviewCommit.MechanicalPerScol.Trim();
            TB_Metallographic.Text = wPQMWorkReviewCommit.Metallographic.Trim();
            TB_MTEvaluationCriteria.Text = wPQMWorkReviewCommit.MTEvaluationCriteria.Trim();
            TB_MTInsProportion.Text = wPQMWorkReviewCommit.MTInsProportion.Trim();
            TB_MTQualifiedLevel.Text = wPQMWorkReviewCommit.MTQualifiedLevel.Trim();
            TB_PTEvaluationCriteria.Text = wPQMWorkReviewCommit.PTEvaluationCriteria.Trim();
            TB_PTInsProportion.Text = wPQMWorkReviewCommit.PTInsProportion.Trim();
            TB_PTQualifiedLevel.Text = wPQMWorkReviewCommit.PTQualifiedLevel.Trim();
            TB_Remark.Text = wPQMWorkReviewCommit.Remark.Trim();
            TB_RTEvaluationCriteria.Text = wPQMWorkReviewCommit.RTEvaluationCriteria.Trim();
            TB_RTInsProportion.Text = wPQMWorkReviewCommit.RTInsProportion.Trim();
            TB_RTQualifiedLevel.Text = wPQMWorkReviewCommit.RTQualifiedLevel.Trim();
            TB_ShockTemperature.Text = wPQMWorkReviewCommit.ShockTemperature.Trim();
            TB_SpecificationDiameter.Text = wPQMWorkReviewCommit.SpecificationDiameter.Trim();
            TB_SpecificationOther.Text = wPQMWorkReviewCommit.SpecificationOther.Trim();
            TB_SpecificationOtherReq.Text = wPQMWorkReviewCommit.SpecificationOtherReq.Trim();
            TB_SpecificationPad.Text = wPQMWorkReviewCommit.SpecificationPad.Trim();
            TB_SpecificationThickness.Text = wPQMWorkReviewCommit.SpecificationThickness.Trim();
            TB_UTEvaluationCriteria.Text = wPQMWorkReviewCommit.UTEvaluationCriteria.Trim();
            TB_UTInsProportion.Text = wPQMWorkReviewCommit.UTInsProportion.Trim();
            TB_UTQualifiedLevel.Text = wPQMWorkReviewCommit.UTQualifiedLevel.Trim();
            TB_Value_1.Text = wPQMWorkReviewCommit.Value_1.Trim();
            TB_Value_2.Text = wPQMWorkReviewCommit.Value_2.Trim();
            TB_Value_3.Text = wPQMWorkReviewCommit.Value_3.Trim();
            TB_Value_4.Text = wPQMWorkReviewCommit.Value_4.Trim();
            TB_Value_5.Text = wPQMWorkReviewCommit.Value_5.Trim();
            TB_Value_6.Text = wPQMWorkReviewCommit.Value_6.Trim();
            TB_Value_7.Text = wPQMWorkReviewCommit.Value_7.Trim();
            DL_WeldProCode.SelectedValue = wPQMWorkReviewCommit.WeldProCode.Trim();
            
            if (wPQMWorkReviewCommit.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMWorkReviewCommit");
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

            TB_SpecificationPad.Text = string.IsNullOrEmpty(wPQMWeldProQua.PadMaterialSpec) ? "" : wPQMWeldProQua.PadMaterialSpec.Trim();
            TB_SpecificationDiameter.Text = string.IsNullOrEmpty(wPQMWeldProQua.SpecificationDiameter) ? "" : wPQMWeldProQua.SpecificationDiameter.Trim();
            TB_SpecificationThickness.Text = string.IsNullOrEmpty(wPQMWeldProQua.SpecificationThickness) ? "" : wPQMWeldProQua.SpecificationThickness.Trim();
            TB_Value_7.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_7) ? "" : wPQMWeldProQua.Value_7.Trim();
            TB_Value_6.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_6) ? "" : wPQMWeldProQua.Value_6.Trim();
            TB_Value_5.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_5) ? "" : wPQMWeldProQua.Value_5.Trim();
            TB_Value_4.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_4) ? "" : wPQMWeldProQua.Value_4.Trim();
            TB_Value_3.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_3) ? "" : wPQMWeldProQua.Value_3.Trim();
            TB_Value_2.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_2) ? "" : wPQMWeldProQua.Value_2.Trim();
            TB_Value_1.Text = string.IsNullOrEmpty(wPQMWeldProQua.Value_1) ? "" : wPQMWeldProQua.Value_1.Trim();
            TB_MechanicalPerReqRel.Text = string.IsNullOrEmpty(wPQMWeldProQua.MechanicalPerReq) ? "" : wPQMWeldProQua.MechanicalPerReq.Trim();
            TB_MechanicalPerReqRm.Text = string.IsNullOrEmpty(wPQMWeldProQua.MechanicalPerReq) ? "" : wPQMWeldProQua.MechanicalPerReq.Trim();
            TB_AfterWeldingClass.Text = string.IsNullOrEmpty(wPQMWeldProQua.AfterWeldingClass) ? "" : wPQMWeldProQua.AfterWeldingClass.Trim();
            TB_AfterWeldingPreTime.Text = string.IsNullOrEmpty(wPQMWeldProQua.AfterWeldingPreTime) ? "" : wPQMWeldProQua.AfterWeldingPreTime.Trim();
            TB_AfterWeldingTem.Text = string.IsNullOrEmpty(wPQMWeldProQua.AfterWeldingTem) ? "" : wPQMWeldProQua.AfterWeldingTem.Trim();
            DL_CoolingMethod.SelectedValue = string.IsNullOrEmpty(wPQMWeldProQua.CoolingMethod) ? "" : wPQMWeldProQua.CoolingMethod.Trim().Substring(0, wPQMWeldProQua.CoolingMethod.Trim().IndexOf("-"));
        }
        else
        {
            TB_SpecificationPad.Text = "";
            TB_SpecificationDiameter.Text = "";
            TB_SpecificationThickness.Text = "";
            TB_Value_7.Text = "";
            TB_Value_6.Text = "";
            TB_Value_5.Text = "";
            TB_Value_4.Text = "";
            TB_Value_3.Text = "";
            TB_Value_2.Text = "";
            TB_Value_1.Text = "";
            TB_MechanicalPerReqRel.Text = "";
            TB_MechanicalPerReqRm.Text = "";
            TB_AfterWeldingClass.Text = "";
            TB_AfterWeldingPreTime.Text = "";
            TB_AfterWeldingTem.Text = "";
            DL_CoolingMethod.SelectedValue = "";
        }
    }
}