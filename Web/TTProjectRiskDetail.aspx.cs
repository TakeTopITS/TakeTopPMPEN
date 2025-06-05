using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;

public partial class TTProjectRiskDetail : System.Web.UI.Page
{
    string strRiskID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName;
        string strRiskName, strProjectID, strProjectName;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strRiskID = Request.QueryString["ID"];

        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);
        ProjectRisk projectRisk = (ProjectRisk)lst[0];

        strRiskName = projectRisk.Risk.Trim();
        strProjectID = projectRisk.ProjectID.ToString();

        strProjectName = ShareClass.GetProjectName(strProjectID);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_EffectDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_FindDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LB_ID.Text = projectRisk.ID.ToString();
            TB_RiskName.Text = projectRisk.Risk.Trim();
            TB_RiskDetail.Text = projectRisk.Detail.Trim();
            DLC_EffectDate.Text = projectRisk.EffectDate.ToString("yyyy-MM-dd");
            DLC_FindDate.Text = projectRisk.FindDate.ToString("yyyy-MM-dd");
            DL_RiskLevel.SelectedValue = projectRisk.RiskLevel;
            DL_Status.SelectedValue = projectRisk.Status;

            strProjectID = projectRisk.ProjectID.ToString();
            LB_ProjectID.Text = projectRisk.ProjectID.ToString();

            HL_RiskToTask.Enabled = true;
            HL_RiskToTask.NavigateUrl = "TTRiskToTask.aspx?RiskID=" + strRiskID + "&ProjectID=" + strProjectID;
            HL_RiskRelatedDoc.Enabled = true;
            HL_RiskRelatedDoc.NavigateUrl = "TTRiskRelatedDoc.aspx?RelatedID=" + strRiskID;
            HL_RiskReviewWF.Enabled = true;
            HL_RiskReviewWF.NavigateUrl = "TTRiskReviewWF.aspx?RiskID=" + strRiskID;

            HL_RunRiskByWF.Enabled = true;
            HL_RunRiskByWF.NavigateUrl = "TTRelatedDIYWorkFlowForm.aspx?RelatedType=ProjectRisk&RelatedID=" + strRiskID;
        }
    }


    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strProjectID, strRisk, strDetail, strLevel, strStatus;
        DateTime dtEffectDate, dtFindDate;


        strProjectID = LB_ProjectID.Text.Trim();
        strRisk = TB_RiskName.Text.Trim();
        strDetail = TB_RiskDetail.Text.Trim();
        dtEffectDate = DateTime.Parse(DLC_EffectDate.Text);
        dtFindDate = DateTime.Parse(DLC_FindDate.Text);

        strLevel = DL_RiskLevel.SelectedValue;
        strStatus = DL_Status.SelectedValue.Trim();

        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        string strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
        IList lst = projectRiskBLL.GetAllProjectRisks(strHQL);

        ProjectRisk projectRisk = (ProjectRisk)lst[0];

        projectRisk.ProjectID = int.Parse(strProjectID);
        projectRisk.Risk = strRisk;
        projectRisk.Detail = strDetail;
        projectRisk.EffectDate = dtEffectDate;
        projectRisk.FindDate = dtFindDate;
        projectRisk.RiskLevel = strLevel;
        projectRisk.Status = strStatus;

        try
        {
            projectRiskBLL.UpdateProjectRisk(projectRisk, int.Parse(strRiskID));

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

}
