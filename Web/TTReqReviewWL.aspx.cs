using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTReqReviewWL : System.Web.UI.Page
{
    string strUserCode, strUserName, strReqID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode).Trim();
        string strHQL, strReviewType;
        IList lst;

        strReqID = Request.QueryString["ReqID"];

        strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();
        lst = requirementBLL.GetAllRequirements(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        Requirement requirement = (Requirement)lst[0];
        //this.Title = "–Ë«Û£∫" + strReqID + " " + requirement.ReqName + LanguageHandle.GetWord("PingShen");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            strReviewType = "Requirement";
            strReviewType = "%" + strReviewType + "%";


            strHQL = "from WorkFlowTemplate as workFlowTemplate where (workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Req' and relatedWorkFlowTemplate.RelatedID = " + strReqID + ")";
            strHQL += " and workFlowTemplate.Type like " + "'" + strReviewType + "'" + ")";
            strHQL += " Or  (workFlowTemplate.Type like " + "'" + strReviewType + "'" + " and workFlowTemplate.Authority = 'All')";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
          
            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();

            LoadRelatedWL("RequirementReview", "Requirement", int.Parse(strReqID));


            TB_WLName.Text = LanguageHandle.GetWord("XuQiu")  + strReqID  + requirement.ReqName + LanguageHandle.GetWord("PingShen");
            //HL_WLTem.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=Req&RelatedID=" + strReqID;
        }
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID;
        strWLID = SubmitApply();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }


    protected string SubmitApply()
    {
        string strWLID, strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText;
        DateTime dtCreateTime;

        XMLProcess xmlProcess = new XMLProcess();

        strWLID = "0";

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = strUserCode;
        strCreatorName = strUserName;
        dtCreateTime = DateTime.Now;

        strXMLFileName = strWLType + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        WorkFlow workFlow = new WorkFlow();

        workFlow.WLName = strWLName;
        workFlow.WLType = strWLType;
        workFlow.XMLFile = strXMLFile2;
        workFlow.TemName = strTemName;
        workFlow.Description = strDescription;
        workFlow.CreatorCode = strCreatorCode;
        workFlow.CreatorName = strCreatorName;
        workFlow.CreateTime = dtCreateTime;
        workFlow.Status = "New";
        workFlow.RelatedType = "Requirement";
        workFlow.RelatedID = int.Parse(strReqID);
        workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

        if (CB_SMS.Checked == true)
        {
            workFlow.ReceiveSMS = "YES";
        }
        else
        {
            workFlow.ReceiveSMS = "NO";
        }

        if (CB_Mail.Checked == true)
        {
            workFlow.ReceiveEMail = "YES";
        }
        else
        {
            workFlow.ReceiveEMail = "NO";
        }

        try
        {
            workFlowBLL.AddWorkFlow(workFlow);
            strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

            strCmdText = "select * from T_Requirement where ReqID = " + strReqID;
            strXMLFile2 = Server.MapPath(strXMLFile2);

            xmlProcess.DbToXML(strCmdText, "T_Requirement", strXMLFile2);

            LoadRelatedWL("RequirementReview", "Requirement", int.Parse(strReqID));

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXPSSGZLSCDGZLGLYMJHCGZLS")+"')", true);
        }
        catch
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXPSSGZLSB")+"')", true);
        }

        return strWLID;
    }

    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        string strHQL, strKeyWord;
        IList lst;

        strKeyWord = TB_KeyWord.Text.Trim();
        strKeyWord = "%" + strKeyWord + "%";

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = 'Req' and relatedWorkFlowTemplate.RelatedID = " + strReqID + ")";
        strHQL += " and workFlowTemplate.TemName like " + "'" + strKeyWord + "'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
      
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);
        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }


    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName.Trim();
        return strUserName.Trim();
    }


}
