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

public partial class TTDocumentTypeSet : System.Web.UI.Page
{
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strLangCode = Session["LangCode"].ToString();
        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        LB_UserCode.Text = strUserCode;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","文档类型设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            strHQL = "from ActorGroup as actorGroup ";
            strHQL += " Where actorGroup.LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order by actorGroup.SortNumber ASC";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            DL_Authority.DataSource = lst;
            DL_Authority.DataBind();

            DL_Authority.Items.Insert(0, new ListItem("集团", "集团")); 

            ShareClass.InitialAllDocTypeTree(TreeView1);
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode;
        IList lst;

        
        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strDocTypeID = treeNode.Target.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);
        DocumentBLL documentBLL = new DocumentBLL();

        try
        {
            if (lst.Count > 0)
            {
                DocType docType = (DocType)lst[0];

                LB_DocTypeID.Text = strDocTypeID;
                TB_DocType.Text = docType.Type.Trim();
                NB_DocTypeSoft.Amount = docType.SortNumber;

                try
                {
                    DL_Authority.SelectedValue = docType.SaveType;
                }
                catch
                {
                }

                BT_UpdateDocType.Enabled = true;
                BT_DeleteDocType.Enabled = true;

            }
            else
            {
                LB_DocTypeID.Text = "0";
                TB_DocType.Text = "";
                NB_DocTypeSoft.Amount = 0;
            }

        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCWJC").ToString().Trim()+"');</script>");
        }
    }


    protected void BT_NewDocType_Click(object sender, EventArgs e)
    {
        string strDocType = TB_DocType.Text.Trim();
        string strSortNumber = NB_DocTypeSoft.Amount.ToString();
        string strUserCode = Session["UserCode"].ToString();
        string strParentID = LB_DocTypeID.Text.Trim();
        string strAuthority = DL_Authority.SelectedValue.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        try
        {
            docType.Type = strDocType;
            docType.SortNumber = int.Parse(strSortNumber);
            docType.ParentID = int.Parse(strParentID);
            docType.UserCode = strUserCode;
            docType.SaveType = strAuthority;

            docTypeBLL.AddDocType(docType);

            ShareClass.InitialAllDocTypeTree(TreeView1);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }

    }
    protected void BT_DeleteDocType_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode;
        IList lst;

        strUserCode = LB_UserCode.Text.Trim();

        string strDocTypeID = LB_DocTypeID.Text.Trim();
        string strDocType = TB_DocType.Text.Trim();
        string strSortNumber = NB_DocTypeSoft.Amount.ToString();

        DocTypeBLL docTypeBLL = new DocTypeBLL();

        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];

            strHQL = "from Document as document where  document.DocTypeID = " + "'" + strDocTypeID + "'";
            strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);

            if (lst.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZJGZGWJLXCZCLXWJNBNSC").ToString().Trim()+"');</script>");
            }
            else
            {
                try
                {
                    docTypeBLL.DeleteDocType(docType);

                    ShareClass.InitialAllDocTypeTree(TreeView1);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
                }
            }
        }
    }

    protected void BT_UpdateDocType_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        int intID;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();

        string strDocTypeID = LB_DocTypeID.Text.Trim();
        string strDocType = TB_DocType.Text.Trim();
        string strSortNumber = NB_DocTypeSoft.Amount.ToString();
        string strAuthority = DL_Authority.SelectedValue.Trim();

        intID = int.Parse(strDocTypeID);

        DocTypeBLL docTypeBLL = new DocTypeBLL();

        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];

            try
            {
                strHQL = "from Document as document where  document.DocTypeID = " + "'" + strDocTypeID + "'";
                strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";
                DocumentBLL documentBLL = new DocumentBLL();
                lst = documentBLL.GetAllDocuments(strHQL);

                if (lst.Count == 0)
                {
                    docType.Type = strDocType;
                }
                docType.SortNumber = int.Parse(strSortNumber);
                docType.SaveType = strAuthority;

                docTypeBLL.UpdateDocType(docType, intID);

                ShareClass.InitialAllDocTypeTree(TreeView1);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
            }
        }
    }

    protected string GetDocTypeCreator(string strDocTypeID)
    {

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.UserCode.Trim();
    }
}
