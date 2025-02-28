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
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProPlanRelatedDocView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName, strDepartCode;
        string strProjectID, strPlanID;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        strUserName = GetUserName(strUserCode);
        strDepartCode = GetDepartCode(strUserCode);

        strPlanID = Request.QueryString["PlanID"];

        strHQL = "from WorkPlan as workPlan where workPlan.ID = " + strPlanID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        WorkPlan workPlan = new WorkPlan();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);
        workPlan = (WorkPlan)lst[0];

        strProjectID = workPlan.ProjectID.ToString().Trim();

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + project.ProjectName.Trim() + "的计划：" + strPlanID + " " + workPlan.Name.Trim() + " 的相关文档";

        LB_PlanID.Text = strPlanID;
        LB_ProjectID.Text = strProjectID;
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        if (Page.IsPostBack == false)
        {
            LoadRelatedDoc(strPlanID, strProjectID);

            HL_DocTree.NavigateUrl = "TTDocumentTreeView.aspx?RelatedType=ProjectPlan&RelatedID=" + strPlanID;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {

        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text.Trim();
            string strUserName = GetUserName(strUserCode);
            string strDocID = e.Item.Cells[0].Text.Trim();
            string strUploadMan = e.Item.Cells[5].Text.Trim();
            string strPlanID = LB_PlanID.Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
            string strDepartCode = GetDepartCode(strUserCode);

            if (strUserName == strUploadMan)
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.DocID = int.Parse(strDocID);
                documentBLL.DeleteDocument(document);

                LoadRelatedDoc(strPlanID, strProjectID);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZFFCZNBNSCBRSCDWJ").ToString().Trim()+"');</script>");
            }
        }
    }
   
    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DocumentBLL documentBLL = new DocumentBLL();
        IList lst = documentBLL.GetAllDocuments(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
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

    protected void LoadRelatedDoc(string strPlanID, string strProjectID)
    {
        string strHQL;
        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = GetDepartCode(strUserCode);
        IList lst;
        DocumentBLL documentBLL = new DocumentBLL();

        strHQL = "from Document as document where ";
        strHQL += " (document.RelatedType = 'Plan' and document.RelatedID = " + strPlanID;
        strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or (document.Visible in ( 'Department','Entire'))))";   //ChineseWord
        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

    }

    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }
}
