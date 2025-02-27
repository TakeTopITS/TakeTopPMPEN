using System; using System.Resources;
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

using System.Text;
using System.IO;
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTConstractReviewWL : System.Web.UI.Page
{
    string strUserCode, strUserName, strConstractCode, strConstractID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;     
        IList lst;

        string strConstract;
        string strDepartCode, strDepartString;     

        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);
        strConstractCode = Request.QueryString["ConstractCode"];

        strConstract = GetConstractName(strConstractCode);
        //this.Title = "合同：" + strConstractCode + " " + strConstract + " 评审工作流";

        strHQL = "from Constract as constract where constract.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractBLL constractBLL = new ConstractBLL();

        lst = constractBLL.GetAllConstracts(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        Constract constract = (Constract)lst[0];

        strConstractID = constract.ConstractID.ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        { 
            strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

            strHQL = "Select TemName From T_WorkFlowTemplate Where Type = 'ContractReview'";
            strHQL += " and ((TemName in (Select WFTemplateName From T_RelatedWorkFlowTemplate where RelatedType = 'Constract' and RelatedID = " + strConstractID + "))";         
            strHQL += " or ((BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
            strHQL += " Or BelongDepartCode in " + strDepartString + ") ";
            strHQL += " and Authority = 'All'))";
            strHQL += " Order by CreateTime DESC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

            DL_TemName.DataSource = ds;
            DL_TemName.DataBind();

            TB_WLName.Text = LanguageHandle.GetWord("HeTong").ToString().Trim() + strConstractCode  + strConstract + LanguageHandle.GetWord("PingShen").ToString().Trim();

            HL_WLTem.NavigateUrl = "TTRelatedWorkFlowTemplate.aspx?RelatedType=Constract&RelatedID=" + strConstractID;

            LoadRelatedWL("ContractReview", LanguageHandle.GetWord("GeTong").ToString().Trim(), int.Parse(strConstractID));


            HL_RelatedWorkFlowTemplate.Enabled = true;
            HL_RelatedWorkFlowTemplate.NavigateUrl = "TTAttachWorkFlowTemplate.aspx?RelatedType=Constract&RelatedID=" + strConstractID;
        }
    }

    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText;
        DateTime dtCreateTime;

        string strWLID;

        strWLID = "0";

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = strUserCode;
        strCreatorName = GetUserName(strCreatorCode);
        dtCreateTime = DateTime.Now;

        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim()+"');</script>");
            return "0";
        }

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
        workFlow.RelatedType = LanguageHandle.GetWord("GeTong").ToString().Trim();
        workFlow.RelatedID = int.Parse(strConstractID);
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

            UpdateConstractStatus(strConstractID, "Reviewing");

            strCmdText = "select * from T_Constract where ConstractID = " + strConstractID;
            strXMLFile2 = Server.MapPath(strXMLFile2);

            xmlProcess.DbToXML(strCmdText, "T_Constract", strXMLFile2);

            LoadRelatedWL("ContractReview", LanguageHandle.GetWord("GeTong").ToString().Trim(), int.Parse(strConstractID));

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHTPSSGZLSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            strWLID = "0";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHTPSSGZLSCSB").ToString().Trim()+"')", true);
        }

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }

    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        string strHQL, strKeyWord;  
        string strDepartCode, strDepartString;

        strKeyWord = TB_KeyWord.Text.Trim();
        strKeyWord = "%" + strKeyWord + "%";    

        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select TemName From T_WorkFlowTemplate Where Type = 'ContractReview'";
        strHQL += " and ((TemName in (Select WFTemplateName From T_RelatedWorkFlowTemplate where RelatedType = 'Constract' and RelatedID = " + strConstractID + "))";
        strHQL += " or ((BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ") ";
        strHQL += " and Authority = 'All'))";
        strHQL += " and TemName like " + "'" + strKeyWord + "'";
        strHQL += " Order by CreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_TemName.DataSource = ds;
        DL_TemName.DataBind();
    }

    protected void UpdateConstractStatus(string strConstractID,string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        constract.Status = strStatus;

        try
        {
            constractBLL.UpdateConstract(constract, int.Parse(strConstractID));
        }
        catch
        {
        }
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

    protected string GetDepartCode(string strUserCode)
    {
        string strDepartCode, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strDepartCode = projectMember.DepartCode;
        return strDepartCode;
    }

    protected string GetConstractName(string strConstractCode)
    {
        string strHQL = "from Constract as constract where constract.ConstractCode = " + "'" + strConstractCode + "'";;
        ConstractBLL constractBLL = new ConstractBLL();
        IList lst = constractBLL.GetAllConstracts(strHQL);
        Constract constract = (Constract)lst[0];

        string strConstractName = constract.ConstractName.Trim();
        return strConstractName;
    }



}
