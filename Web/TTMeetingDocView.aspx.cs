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

public partial class TTMeetingDocView : System.Web.UI.Page
{

    string strMeetingID, strMeetingName;
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {    
        strUserCode = Session["UserCode"].ToString();
        strUserName = GetUserName(strUserCode);

        strMeetingID = Request.QueryString["RelatedID"];
        strMeetingName = GetMeetingName(strMeetingID);

        LB_MeetingID.Text = strMeetingID;

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        //this.Title = "ª·“È£∫" + strMeetingID + " " + strMeetingName + " related document";

        if (Page.IsPostBack == false)
        {
            LoadMeetingDoc(strUserCode, strMeetingID);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text.Trim();
            string strUserName = GetUserName(strUserCode);
            string strDocID = e.Item.Cells[0].Text.Trim();
            string strDocName = e.Item.Cells[1].Text.Trim();
            string strUploadMan = e.Item.Cells[5].Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();


            if (strUserName == strUploadMan)
            {
                DocumentBLL documentBLL = new DocumentBLL();
                Document document = new Document();

                document.DocID = int.Parse(strDocID);
                documentBLL.DeleteDocument(document);

                LoadMeetingDoc(strUserCode, strMeetingID);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZJGNBNSCBRSCDWJ").ToString().Trim()+"');</script>");
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
        strDepartCode = projectMember.DepartCode;
        return strDepartCode;
    }

    protected string GetMeetingName(string strMeetingID)
    {
        string strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);
        Meeting meeting = (Meeting)lst[0];

        string strMeetingName = meeting.Name.Trim();

        return strMeetingName;
    }


    protected string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }

    protected void LoadMeetingDoc(string strUserCode, string strMeetingID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.RelatedType = 'Meeting' ";  
        strHQL += " and document.RelatedID = " + strMeetingID;
        strHQL += " and ((document.Visible = 'Entire' and document.RelatedID in (select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + " ))";   
        strHQL += " or  (document.Visible = 'Individual' and document.UploadManCode = " + "'" + strUserCode + "'" + "))";  
        strHQL += " Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        documentBLL = new DocumentBLL();

        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }
}
