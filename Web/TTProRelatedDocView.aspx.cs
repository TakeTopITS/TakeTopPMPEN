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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProRelatedDocView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL, strProjectID;
        IList lst;

        string strUserName;
        string strRelatedID = Request.QueryString["RelatedID"]; 
        string strDepartCode = GetDepartCode(strUserCode);        
       
        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        strProjectID = strRelatedID;

        if (Page.IsPostBack != true)
        {            
            strHQL = "from Project as project where project.ProjectID = " + strRelatedID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);

            Project project = (Project)lst[0];

            //this.Title = LanguageHandle.GetWord("Project") + strRelatedID + project.ProjectName + " ÎÄµµÁÐ±í";

            strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire'))))";   
            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + "))";
            strHQL += "or (document.RelatedType = 'Risk' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID =" + strProjectID + "))";  
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
            strHQL += "or (document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedID =" + strProjectID + "))";  
            strHQL += " and ((document.Visible in ('Meeting','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
            strHQL += " or (document.Visible = 'Entire' )))))";   
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";
            
            DocumentBLL documentBLL = new DocumentBLL();               
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            HL_DocTree.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=Project&RelatedID=" + strProjectID;           
        }
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
        strDepartCode = projectMember.DepartCode.Trim();
        return strDepartCode;
    }
}
