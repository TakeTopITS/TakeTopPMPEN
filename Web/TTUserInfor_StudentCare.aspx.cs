using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Drawing;

public partial class TTUserInfor_StudentCare : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            TXT_CheckTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(Request.QueryString["StudentCode"]))
            {
                string strStudentCode = ShareClass.ObjectToString(Request.QueryString["StudentCode"]);
                HF_StudentCode.Value = strStudentCode;
                string strStudentName = ShareClass.ObjectToString(Request.QueryString["StudentName"]);
                HF_StudentName.Value = strStudentName;
                DataBinder(strStudentCode);

                LB_StudentCode.Text = strStudentCode;
                LB_StudentName.Text = strStudentName;
            }
        }
    }


    private void DataBinder(string strStudentCode)
    {
        DG_List.CurrentPageIndex = 0;

        ProjectMemberStudentCareBLL projectMemberStudentCareBLL = new ProjectMemberStudentCareBLL();
        string strProjectMemberStudentCareHQL = string.Format("from ProjectMemberStudentCare as projectMemberStudentCare where StudentCode = '{0}'  order by projectMemberStudentCare.StudentCode desc", strStudentCode);
        IList listProjectMemberStudentCare = projectMemberStudentCareBLL.GetAllProjectMemberStudentCares(strProjectMemberStudentCareHQL);

        DG_List.DataSource = listProjectMemberStudentCare;
        DG_List.DataBind();

        LB_QualitySql.Text = strProjectMemberStudentCareHQL;
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectMemberStudentCareBLL projectMemberStudentCareBLL = new ProjectMemberStudentCareBLL();
            ProjectMemberStudentCare projectMemberStudentCare = new ProjectMemberStudentCare();

            string strEditID = TXT_ID.Text.Trim();
            string strCheckTime = TXT_CheckTime.Text.Trim();

            DateTime dtCheckTime = DateTime.Now;
            DateTime.TryParse(strCheckTime, out dtCheckTime);

            string strActualAge = TXT_ActualAge.Text.Trim();
            string strWeightKG = TXT_WeightKG.Text.Trim();
            string strWeightEvaluation = TXT_WeightEvaluation.Text.Trim();
            string strHeightCM = TXT_HeightCM.Text.Trim();
            string strHeightEvaluation = TXT_HeightEvaluation.Text.Trim();
            string strEyesight = TXT_Eyesight.Text.Trim();
            string strEar = TXT_Ear.Text.Trim();
            string strNose = TXT_Nose.Text.Trim();
            string strPharynxFlat = TXT_PharynxFlat.Text.Trim();
            string strHeart = TXT_Heart.Text.Trim();
            string strLung = TXT_Lung.Text.Trim();
            string strLiverSpleen = TXT_LiverSpleen.Text.Trim();
            string strGenitals = TXT_Genitals.Text.Trim();
            string strHearingScreening = TXT_HearingScreening.Text.Trim();
            string strRefractiveScreening = TXT_RefractiveScreening.Text.Trim();
            string strHemoglobin = TXT_Hemoglobin.Text.Trim();
            string strTurnEnzyme = TXT_TurnEnzyme.Text.Trim();
            string strHepatitisAntigen = TXT_HepatitisAntigen.Text.Trim();
            string strOtherRemark = TXT_OtherRemark.Text.Trim();
            string strPhysicianGuidance = TXT_PhysicianGuidance.Text.Trim();
            string strPhysicianSignature = TXT_PhysicianSignature.Text.Trim();

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strProjectMemberStudentCareHQL = string.Format(@"from ProjectMemberStudentCare as projectMemberStudentCare where ID = " + strID);
                IList lstProjectMemberStudentCare = projectMemberStudentCareBLL.GetAllProjectMemberStudentCares(strProjectMemberStudentCareHQL);
                if (lstProjectMemberStudentCare != null && lstProjectMemberStudentCare.Count > 0)
                {
                    projectMemberStudentCare = (ProjectMemberStudentCare)lstProjectMemberStudentCare[0];

                    projectMemberStudentCare.CheckTime = dtCheckTime;
                    projectMemberStudentCare.ActualAge = strActualAge;
                    projectMemberStudentCare.WeightKG = strWeightKG;

                    projectMemberStudentCare.WeightEvaluation = strWeightEvaluation;
                    projectMemberStudentCare.HeightCM = strHeightCM;
                    projectMemberStudentCare.HeightEvaluation = strHeightEvaluation;
                    projectMemberStudentCare.Eyesight = strEyesight;
                    projectMemberStudentCare.Ear = strEar;
                    projectMemberStudentCare.Nose = strNose;
                    projectMemberStudentCare.PharynxFlat = strPharynxFlat;
                    projectMemberStudentCare.Heart = strHeart;
                    projectMemberStudentCare.Lung = strLung;
                    projectMemberStudentCare.LiverSpleen = strLiverSpleen;
                    projectMemberStudentCare.Genitals = strGenitals;
                    projectMemberStudentCare.HearingScreening = strHearingScreening;
                    projectMemberStudentCare.RefractiveScreening = strRefractiveScreening;
                    projectMemberStudentCare.Hemoglobin = strHemoglobin;
                    projectMemberStudentCare.TurnEnzyme = strTurnEnzyme;
                    projectMemberStudentCare.HepatitisAntigen = strHepatitisAntigen;
                    projectMemberStudentCare.OtherRemark = strOtherRemark;
                    projectMemberStudentCare.PhysicianGuidance = strPhysicianGuidance;
                    projectMemberStudentCare.PhysicianSignature = strPhysicianSignature;

                    projectMemberStudentCareBLL.UpdateProjectMemberStudentCare(projectMemberStudentCare, int.Parse(strID));
                }
            }
            else
            {
                projectMemberStudentCare.CheckTime = dtCheckTime;
                projectMemberStudentCare.ActualAge = strActualAge;
                projectMemberStudentCare.WeightKG = strWeightKG;

                projectMemberStudentCare.WeightEvaluation = strWeightEvaluation;
                projectMemberStudentCare.HeightCM = strHeightCM;
                projectMemberStudentCare.HeightEvaluation = strHeightEvaluation;
                projectMemberStudentCare.Eyesight = strEyesight;
                projectMemberStudentCare.Ear = strEar;
                projectMemberStudentCare.Nose = strNose;
                projectMemberStudentCare.PharynxFlat = strPharynxFlat;
                projectMemberStudentCare.Heart = strHeart;
                projectMemberStudentCare.Lung = strLung;
                projectMemberStudentCare.LiverSpleen = strLiverSpleen;
                projectMemberStudentCare.Genitals = strGenitals;
                projectMemberStudentCare.HearingScreening = strHearingScreening;
                projectMemberStudentCare.RefractiveScreening = strRefractiveScreening;
                projectMemberStudentCare.Hemoglobin = strHemoglobin;
                projectMemberStudentCare.TurnEnzyme = strTurnEnzyme;
                projectMemberStudentCare.HepatitisAntigen = strHepatitisAntigen;
                projectMemberStudentCare.OtherRemark = strOtherRemark;
                projectMemberStudentCare.PhysicianGuidance = strPhysicianGuidance;
                projectMemberStudentCare.PhysicianSignature = strPhysicianSignature;

                projectMemberStudentCare.StudentCode = HF_StudentCode.Value;
                projectMemberStudentCare.StudentName = HF_StudentName.Value;

                projectMemberStudentCareBLL.AddProjectMemberStudentCare(projectMemberStudentCare);

                
            }

            DataBinder(HF_StudentCode.Value);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch (Exception ex) { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";

        TXT_CheckTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

        TXT_ActualAge.Text ="";
        TXT_WeightKG.Text = "";
        TXT_WeightEvaluation.Text = "";
        TXT_HeightCM.Text = "";
        TXT_HeightEvaluation.Text = "";
        TXT_Eyesight.Text = "";
        TXT_Ear.Text = "";
        TXT_Nose.Text ="";
        TXT_PharynxFlat.Text ="";
        TXT_Heart.Text ="";
        TXT_Lung.Text ="";
        TXT_LiverSpleen.Text ="";
        TXT_Genitals.Text ="";
        TXT_HearingScreening.Text ="";
        TXT_RefractiveScreening.Text ="";
        TXT_Hemoglobin.Text ="";
        TXT_TurnEnzyme.Text ="";
        TXT_HepatitisAntigen.Text = "";
        TXT_OtherRemark.Text ="";
         TXT_PhysicianGuidance.Text = "";
        TXT_PhysicianSignature.Text = "";
    }


    protected void BT_Seach_Click(object sender, EventArgs e)
    {
        DataBinder(HF_StudentCode.Value);
    }

    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strCmdArgu = e.CommandArgument.ToString();

                ProjectMemberStudentCareBLL projectMemberStudentCareBLL = new ProjectMemberStudentCareBLL();
                string strProjectMemberStudentCareHQL = string.Format(@"from ProjectMemberStudentCare as projectMemberStudentCare where ID = " + strCmdArgu);
                IList lstProjectMemberStudentCare = projectMemberStudentCareBLL.GetAllProjectMemberStudentCares(strProjectMemberStudentCareHQL);
                if (lstProjectMemberStudentCare != null && lstProjectMemberStudentCare.Count > 0)
                {
                    ProjectMemberStudentCare projectMemberStudentCare = (ProjectMemberStudentCare)lstProjectMemberStudentCare[0];

                    HF_ID.Value = projectMemberStudentCare.ID.ToString();
                    TXT_ID.Text = projectMemberStudentCare.ID.ToString();
                    TXT_CheckTime.Text = projectMemberStudentCare.CheckTime.ToString();

                    TXT_ActualAge.Text = projectMemberStudentCare.ActualAge;
                    TXT_WeightKG.Text = projectMemberStudentCare.WeightKG;
                    TXT_WeightEvaluation.Text = projectMemberStudentCare.WeightEvaluation;
                    TXT_HeightCM.Text = projectMemberStudentCare.HeightCM;
                    TXT_HeightEvaluation.Text = projectMemberStudentCare.HeightEvaluation;
                    TXT_Eyesight.Text = projectMemberStudentCare.Eyesight;
                    TXT_Ear.Text = projectMemberStudentCare.Ear;
                    TXT_Nose.Text = projectMemberStudentCare.Nose;
                    TXT_PharynxFlat.Text = projectMemberStudentCare.PharynxFlat;
                    TXT_Heart.Text = projectMemberStudentCare.Heart;
                    TXT_Lung.Text = projectMemberStudentCare.Lung;
                    TXT_LiverSpleen.Text = projectMemberStudentCare.LiverSpleen;
                    TXT_Genitals.Text = projectMemberStudentCare.Genitals;
                    TXT_HearingScreening.Text = projectMemberStudentCare.HearingScreening;
                    TXT_RefractiveScreening.Text = projectMemberStudentCare.RefractiveScreening;
                    TXT_Hemoglobin.Text = projectMemberStudentCare.Hemoglobin;
                    TXT_TurnEnzyme.Text = projectMemberStudentCare.TurnEnzyme;
                    TXT_HepatitisAntigen.Text = projectMemberStudentCare.HepatitisAntigen;
                    TXT_OtherRemark.Text = projectMemberStudentCare.OtherRemark;
                    TXT_PhysicianGuidance.Text = projectMemberStudentCare.PhysicianGuidance;
                    TXT_PhysicianSignature.Text = projectMemberStudentCare.PhysicianSignature;
                }
            }
            else if (cmdName == "del")
            {
                string strCmdArgu = e.CommandArgument.ToString();

                ProjectMemberStudentCareBLL projectMemberStudentCareBLL = new ProjectMemberStudentCareBLL();
                string strProjectMemberStudentCareHQL = string.Format(@"from ProjectMemberStudentCare as projectMemberStudentCare where ID = " + strCmdArgu);
                IList lstProjectMemberStudentCare = projectMemberStudentCareBLL.GetAllProjectMemberStudentCares(strProjectMemberStudentCareHQL);
                if (lstProjectMemberStudentCare != null && lstProjectMemberStudentCare.Count > 0)
                {
                    ProjectMemberStudentCare projectMemberStudentCare = (ProjectMemberStudentCare)lstProjectMemberStudentCare[0];

                    projectMemberStudentCareBLL.DeleteProjectMemberStudentCare(projectMemberStudentCare);

                    DataBinder(HF_StudentCode.Value);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }
            }
        }
        catch (Exception ex) { }
    }




}