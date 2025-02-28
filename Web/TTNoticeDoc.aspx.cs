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

public partial class TTNoticeDoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = GetDepartCode(strUserCode);

        string strHQL;
        IList lst;
    
    
        string strUserName;

        //this.Title = "公司公告文件管理";

        LB_UserCode.Text = strUserCode;
        strUserName = GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        if (Page.IsPostBack != true)
        {
            strHQL = "from DocType as docType  where docType.Type in ('AnnouncementDocument','NotificationDocument') and ((docType.SaveType = 'Company') ";  
            strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";  
            strHQL += " or (docType.SaveType not in ('Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";  
            strHQL += " Order by docType.SortNumber ASC";
            DocTypeBLL docTypeBLL = new DocTypeBLL();
            lst = docTypeBLL.GetAllDocTypes(strHQL);
            DL_DocType.DataSource = lst;
            DL_DocType.DataBind();

            LB_DocOwner.Text = LanguageHandle.GetWord("GongSiGongGaoWenJianLieBiao").ToString().Trim();
          
            strHQL = "from Document as document where ((document.Visible = 'Company') or (document.DepartCode = " + "'" +strDepartCode+ "'" + ")) and document.DocType in ('AnnouncementDocument','NotificationDocument') and document.Status <> 'Deleted' Order by document.DocID DESC";  

            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;

            HL_UploadDoc.NavigateUrl = "TTPublishNotice.aspx";
        }
    }   

    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strUserName = projectMember.UserName;
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

    protected void DataGrid2_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    protected void DataGrid2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strDocType = DL_DocType.SelectedValue;
        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = GetDepartCode(strUserCode);
        string strHQL;
        IList lst;

        LB_DocOwner.Text = LanguageHandle.GetWord("LeiXing").ToString().Trim() + strDocType + LanguageHandle.GetWord("DeWenDangLieBiao").ToString().Trim();
        strHQL = "from Document as document where  document.DocType = " + "'" + strDocType + "'" + " and ((document.DepartCode = " + "'" + strDepartCode + "'" + ") or (document.Visible = 'Company')) and document.Status<> 'Deleted' Order by document.DocID DESC";  
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
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
    protected void BT_HazyFind_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = GetDepartCode(strUserCode);

        string strDocName = TB_HazyFind.Text;
        string strHQL;
        IList lst;

        LB_DocOwner.Text = LanguageHandle.GetWord("WenJianMingBaoHan").ToString().Trim() + strDocName + LanguageHandle.GetWord("ZiFuChuanDeWenDangLieBiao").ToString().Trim();

        strDocName = "%" + strDocName + "%";
        strHQL = "from Document as document where document.DocName like " + "'" + strDocName + "'" + " and ((document.Visible = 'Company') or (document.DepartCode = " + "'" +strDepartCode+ "'" + ")) and document.DocType in ('AnnouncementDocument','NotificationDocument') and document.Status <> 'Deleted'  Order by document.DocID DESC";  
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }


  
}
