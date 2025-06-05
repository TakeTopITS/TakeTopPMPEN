using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TTSuperDocumentManage : System.Web.UI.Page
{
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strParentDepartString, strUnderDepartString;

        strLangCode = Session["LangCode"].ToString();
        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        LB_UserCode.Text = strUserCode;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "全局知识管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            strParentDepartString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);
            strUnderDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

            LB_ParentDepratString.Text = strParentDepartString;
            LB_UnderDepartStringg.Text = strUnderDepartString;

            strHQL = "from ActorGroup as actorGroup ";
            strHQL += " Where actorGroup.LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order by actorGroup.SortNumber ASC";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            DL_Authority.DataSource = lst;
            DL_Authority.DataBind();

            //DL_Authority.Items.Insert(0, new ListItem("Group", "Group"));

            strHQL = "from Document as document where ";
            strHQL += " document.Status <> 'Deleted' Order by document.DocID DESC";
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();

            LB_Sql.Text = strHQL;

            strHQL = "from Document as document ";
            documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);

            LB_TotalCount.Text = LanguageHandle.GetWord("ZongWenJianShu") + lst.Count.ToString();

            ShareClass.InitialAllDocTypeTree(TreeView1);
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode;
        IList lst;

        string strParentDepartString, strUnderDepartString;

        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

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
                    DL_Authority.SelectedValue = docType.SaveType.Trim();
                }
                catch
                {
                }

                strHQL = "from Document as document where  document.DocTypeID = " + "'" + strDocTypeID + "'";
                strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";

                lst = documentBLL.GetAllDocuments(strHQL);
                DataGrid1.DataSource = lst;
                DataGrid1.DataBind();

                LB_Sql.Text = strHQL;

                BT_UpdateDocType.Enabled = true;
                BT_DeleteDocType.Enabled = true;
            }
            else
            {
                strHQL = "from Document as document where ";
                strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";

                lst = documentBLL.GetAllDocuments(strHQL);
                DataGrid1.DataSource = lst;
                DataGrid1.DataBind();

                LB_Sql.Text = strHQL;

                LB_DocTypeID.Text = "0";
                TB_DocType.Text = "";
                NB_DocTypeSoft.Amount = 0;
            }

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWJC") + "')", true);
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
        }
        catch
        {
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

            if (DataGrid1.Items.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZGWJLXCZCLXWJNBNSC") + "')", true);
            }
            else
            {
                try
                {
                    docTypeBLL.DeleteDocType(docType);

                    ShareClass.InitialAllDocTypeTree(TreeView1);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB") + "')", true);
                }
            }
        }
    }

    protected void BT_UpdateDocType_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode;
        int intID;
        IList lst;

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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }
    }


    protected void BT_HazyFind_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        string strDocName = TB_DocName.Text.Trim();
        string strUploadManName = TB_UploadManName.Text.Trim();

        string strParentDepartString, strUnderDepartString;

        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

        strDocName = "%" + strDocName + "%";
        strUploadManName = "%" + strUploadManName + "%";

        strHQL = "from Document as document where document.DocName like " + "'" + strDocName + "'";
        strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
    }


    protected void BT_FindUploadMan_Click(object sender, EventArgs e)
    {
        string strHQL, strUploadManName;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        string strParentDepartString, strUnderDepartString;

        strParentDepartString = LB_ParentDepratString.Text.Trim();
        strUnderDepartString = LB_UnderDepartStringg.Text.Trim();

        strUploadManName = TB_UploadManName.Text.Trim();

        strUploadManName = "%" + strUploadManName + "%";
        strHQL = "from Document as document where document.UploadManName like " + "'" + strUploadManName + "'";
        strHQL += " and document.Status <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS") + ": " + lst.Count.ToString();
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
