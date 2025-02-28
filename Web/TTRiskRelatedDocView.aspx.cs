using System; using System.Resources;
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

public partial class TTRiskRelatedDocView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;
        int i;
        string strUserName;
        string strRiskID = Request.QueryString["RelatedID"];
        string strDepartCode = GetDepartCode(strUserCode);

        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        if (Page.IsPostBack != true)
        {
            strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
            ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();

            lst = projectRiskBLL.GetAllProjectRisks(strHQL);

            ProjectRisk projectRisk = (ProjectRisk)lst[0];
            //this.Title = "���գ�" + strRiskID + " " + projectRisk.Risk + " �ĵ��б�";

            strHQL = "from Document as document where ";
            strHQL += " (document.RelatedType = 'Risk' and document.RelatedID = " + strRiskID;  
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + " )";  
            strHQL += " or ( document.Visible = 'Entire'))) ";   //ChineseWord
            strHQL += "or ((document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Risk' and meeting.RelatedID =" + strRiskID + "))";  
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or ( document.Visible = 'Meeting')))";  
            strHQL += " and document.Status <> 'Deleted' ";
            strHQL += " order by document.DocID DESC";

            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();
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
