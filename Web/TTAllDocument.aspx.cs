using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTAllDocument : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        string strUserCode = Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        LB_UserCode.Text = strUserCode;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "查看所有文档", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;

            ShareClass.LoadActorGroupDropDownList(DL_Authority, strUserCode);

            strHQL = "from Document as document";
            strHQL += " Where ((document.DepartCode in " + strDepartString;

            strHQL += " ) Or document.Visible = '集团' or document.Visible = 'All')"; 
            strHQL += " and document.Status <> 'Deleted'";
            strHQL += " Order by document.DocID DESC";
            DocumentBLL documentBLL = new DocumentBLL();
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();

            LB_Sql.Text = strHQL;
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWSCZ").ToString().Trim() + ": " + "'" + ShareClass.GetUserName(strUserCode) + "'";

            LB_TotalCount.Text = LanguageHandle.GetWord("ZongWenJianShu").ToString().Trim() + lst.Count.ToString();

            ShareClass.InitialAllDocTypeTree(TreeView1);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDocTypeID, strHQL, strUserCode, strDepartCode;
        IList lst;

        string strDepartString = LB_DepartString.Text.Trim();

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strDocTypeID = treeNode.Target.Trim();

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;

        lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocumentBLL documentBLL = new DocumentBLL();

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];

            LB_DocTypeID.Text = strDocTypeID;
            TB_DocType.Text = docType.Type.Trim();
            NB_DocTypeSort.Text = docType.SortNumber.ToString();

            try
            {
                DL_Authority.SelectedValue = docType.SaveType.Trim();
            }
            catch
            {
            }

            strHQL = "from Document as document where document.DocTypeID = " + "'" + strDocTypeID + "'";
            strHQL += " and (document.DepartCode in " + strDepartString;

            strHQL += " Or document.Visible = '集团' or document.Visible = 'All')"; 
            strHQL += " and document.Status <> 'Deleted'";
            strHQL += " Order by document.DocID DESC";
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;

            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLX").ToString().Trim() + "'" + docType.Type.Trim() + "'";

            if (docType.UserCode.Trim() == strUserCode & DataGrid1.Items.Count == 0)
            {
                BT_UpdateDocType.Enabled = true;
                BT_DeleteDocType.Enabled = true;
            }
            else
            {
                BT_UpdateDocType.Enabled = false;
                BT_DeleteDocType.Enabled = false;
            }

            if (docType.UserCode.Trim() == strUserCode)
            {
                BT_UpdateDocType.Enabled = true;
            }
        }
        else
        {
            strHQL = "from Document as document where ";
            strHQL += " (document.DepartCode in " + strDepartString;

            strHQL += " Or document.Visible = '集团' or document.Visible = 'All')"; 
            strHQL += " and document.Status <> 'Deleted'";
            strHQL += " Order by document.DocID DESC";
            lst = documentBLL.GetAllDocuments(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWWJLXSY").ToString().Trim();

            LB_Sql.Text = strHQL;

            LB_DocTypeID.Text = "0";
            TB_DocType.Text = "";
            NB_DocTypeSort.Text = "";
        }

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();
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
        string strSortNumber = NB_DocTypeSort.Text.Trim();
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
        string strSortNumber = NB_DocTypeSort.Amount.ToString();

        DocTypeBLL docTypeBLL = new DocTypeBLL();

        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];

            if (strUserCode != docType.UserCode.Trim() | DataGrid1.Items.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGWJLXBSNCJDHCZCLXWJNBNSC").ToString().Trim() + "')", true);
            }
            else
            {
                try
                {
                    docTypeBLL.DeleteDocType(docType);

                    ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB").ToString().Trim() + "')", true);
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
        string strSortNumber = NB_DocTypeSort.Amount.ToString();
        string strAuthority = DL_Authority.SelectedValue.Trim();
        intID = int.Parse(strDocTypeID);

        DocTypeBLL docTypeBLL = new DocTypeBLL();

        strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        lst = docTypeBLL.GetAllDocTypes(strHQL);

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];

            if (strUserCode != docType.UserCode.Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGWJLXBSNCJDNBNGG").ToString().Trim() + "')", true);
            }
            else
            {
                try
                {
                    docType.Type = strDocType;
                    docType.SortNumber = int.Parse(strSortNumber);
                    docType.SaveType = strAuthority;

                    docTypeBLL.UpdateDocType(docType, intID);

                    ShareClass.InitialUserDocTypeTree(TreeView1, strUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        string strUploadManName = TB_UploadManName.Text.Trim();
        string strDocName = TB_DocName.Text.Trim();

        if (strUploadManName == "")
        {
            LB_FindCondition.Text = LanguageHandle.GetWord("CXFWSYWD").ToString().Trim();
        }
        else
        {
            LB_FindCondition.Text = LanguageHandle.GetWord("SCZXMBH").ToString().Trim() + ": " + "'" + strUploadManName + "'";
        }

        string strDepartString = LB_DepartString.Text.Trim();

        strUploadManName = "%" + strUploadManName + "%";
        strDocName = "%" + strDocName + "%";

        strHQL = "from Document as document where ((document.UploadManName like " + "'" + strUploadManName + "'";
        strHQL += " and document.DocName like " + "'" + strDocName + "'";
        strHQL += " and document.DepartCode in " + strDepartString;

        strHQL += " ) Or document.Visible = '集团' or document.Visible = 'All')"; 
        strHQL += " and document.Status <> 'Deleted'";
        strHQL += " Order by document.DocID DESC";
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        LB_Count.Text = LanguageHandle.GetWord("CXDDWJS").ToString().Trim() + ": " + lst.Count.ToString();
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
