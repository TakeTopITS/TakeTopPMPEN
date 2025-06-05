using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTDocRelatedUser : System.Web.UI.Page
{
    string strUserCode, strUserName, strDocID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strDocID = Request.QueryString["DocID"];

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView2, strUserCode);

            LoadDocRelatedUser(strDocID);

            LB_DocID.Text = strDocID;
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strDocID, strOperatorCode;

        strDocID = LB_DocID.Text.Trim();

        if (strDocID != "")
        {
            strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

            DocRelatedUserBLL docRelatedUserBLL = new DocRelatedUserBLL();
            DocRelatedUser docRelatedUser = new DocRelatedUser();

            docRelatedUser.DocID = int.Parse(strDocID);
            docRelatedUser.UserCode = strOperatorCode;
            docRelatedUser.UserName = ShareClass.GetUserName(strOperatorCode);

            try
            {
                docRelatedUserBLL.AddDocRelatedUser(docRelatedUser);

                LoadDocRelatedUser(strDocID);
            }
            catch
            {
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZHTCNZJCY") + "')", true);
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            string strHQL, strDocID;
            IList lst;

            strDocID = LB_DocID.Text.Trim();
            strHQL = "from DocRelatedUser as docRelatedUser where docRelatedUser.DocID = " + "'" + strDocID + "'" + " and docRelatedUser.UserName = " + "'" + strUserName + "'";
            DocRelatedUserBLL docRelatedUserBLL = new DocRelatedUserBLL();
            lst = docRelatedUserBLL.GetAllDocRelatedUsers(strHQL);

            DocRelatedUser docRelatedUser = (DocRelatedUser)lst[0];

            docRelatedUserBLL.DeleteDocRelatedUser(docRelatedUser);

            LoadDocRelatedUser(strDocID);
        }
    }

    protected void LoadDocRelatedUser(string strDocID)
    {
        string strHQL;
        IList lst;

        strHQL = "from DocRelatedUser as docRelatedUser where docRelatedUser.DocID = " + strDocID;
        DocRelatedUserBLL docRelatedUserBLL = new DocRelatedUserBLL();
        lst = docRelatedUserBLL.GetAllDocRelatedUsers(strHQL);

        RP_DocRelatedUser.DataSource = lst;
        RP_DocRelatedUser.DataBind();
    }

}