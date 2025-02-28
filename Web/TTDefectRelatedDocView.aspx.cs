using System;
using System.Resources;
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

public partial class TTDefectRelatedDocView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;
        int i;
        string strUserName;
        string strDefectID = Request.QueryString["RelatedID"];
        string strDepartCode = GetDepartCode(strUserCode);

        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        if (Page.IsPostBack != true)
        {
            strHQL = "from Defectment as defectment where defectment.DefectID = " + strDefectID;
            DefectmentBLL defectmentBLL = new DefectmentBLL();

            lst = defectmentBLL.GetAllDefectments(strHQL);

            Defectment defectment = (Defectment)lst[0];
            //this.Title = "È±ÏÝ£º" + strDefectID + " " + defectment.DefectName + " ÎÄµµÁÐ±í";

            strHQL = "from Document as document where ";
            strHQL += " (document.RelatedType = 'Defect' and document.RelatedID = " + strDefectID;
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible = 'Department' and document.DepartCode = " + "'" + strDepartCode + "'" + " )";  
            strHQL += " or ( document.Visible = 'Entire'))) ";   
            strHQL += "or ((document.RelatedType = 'Meeting' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Defect' and meeting.RelatedID =" + strDefectID + "))";  
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
