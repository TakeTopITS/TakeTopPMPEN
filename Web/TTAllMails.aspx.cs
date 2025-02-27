using System;
using System.Resources;
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
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using Npgsql;

public partial class TTAllMails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件2006－2012

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "查看所有成员邮件", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LB_QueryScope.Text = LanguageHandle.GetWord("ZZMailOwnerALL").ToString().Trim();

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = GetDepartName(strDepartCode);

            LB_QueryScope.Text = LanguageHandle.GetWord("ZZZBuMen").ToString().Trim() + strDepartName;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);
        }

        
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strOperatorName = GetUserName(strOperatorCode);

        LB_OperatorCode.Text = strOperatorCode;
        LB_OperatorName.Text = strOperatorName;

        LB_QueryScope.Text = LanguageHandle.GetWord("ZZMailOwner").ToString().Trim() + strOperatorCode + strOperatorName;

        InitOperationTree(strOperatorCode);

        BT_ReciveMails.Enabled = true;

    }

    protected void BT_ReciveMails_Click(object sender, EventArgs e)
    {
        string strUserCode, strDocSavePath;

        strDocSavePath = Server.MapPath("Doc");

        Msg msg = new Msg();

        strUserCode = LB_OperatorCode.Text.Trim();
        msg.ReceiveMailByUserCode(strUserCode, strDocSavePath);


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJSFWC").ToString().Trim() + "')", true);
    }

    protected void OperationView_SelectedNodeChanged(object sender, EventArgs e)
    {
        int nFoldID;

        nFoldID = int.Parse(OperationView.SelectedValue);

        ///获取数据
        IMail mail = new Mail();
        NpgsqlDataReader dr = mail.GetMailsByFloder(nFoldID);
        ///绑定数据
        MailView.DataSource = dr;
        MailView.DataBind();
        dr.Close();
    }

    private void InitOperationTree(string strUserCode)
    {
        string strFolderID, strHQL;
        IList lst;



        ///找到“邮件文件夹”节点
        TreeNode mailFolderNode = OperationView.FindNode("-1/0");
        if (mailFolderNode == null)
        {
            return;
        }

        mailFolderNode.ChildNodes.Clear();

        CreateMailFolder(strUserCode, "New", LanguageHandle.GetWord("XinShouYouJian").ToString().Trim());
        CreateMailFolder(strUserCode, "Read", LanguageHandle.GetWord("YiYueYouJian").ToString().Trim());
        CreateMailFolder(strUserCode, "Waiting", LanguageHandle.GetWord("DaiFaYouJian").ToString().Trim());
        CreateMailFolder(strUserCode, "Send", LanguageHandle.GetWord("YiFaYouJian").ToString().Trim());
        CreateMailFolder(strUserCode, "Draft", LanguageHandle.GetWord("CaoGaoXiang").ToString().Trim());
        CreateMailFolder(strUserCode, "Rubbish", LanguageHandle.GetWord("LaJiXiang").ToString().Trim());

        strHQL = "FROM Folders as folders where folders.OwnerCode = " + "'" + strUserCode + "'" + " order by folders.FolderID ASC";
        FoldersBLL foldersBLL = new FoldersBLL();
        lst = foldersBLL.GetAllFolderss(strHQL);

        Folders folders = new Folders();

        for (int i = 0; i < lst.Count; i++)
        {
            folders = (Folders)lst[i];

            strFolderID = folders.FolderID.ToString();

            TreeNode node = new TreeNode();
            //node.NavigateUrl = "~/TTViewMail.aspx?FolderID=" + strFolderID;
            node.Target = "Desktop";
            node.Text = folders.Name.Trim();
            node.Value = strFolderID;

            mailFolderNode.ChildNodes.Add(node);
        }
    }

    protected void CreateMailFolder(string strUserCode, string strKeyWord, string strFolderName)
    {
        string strHQL;
        IList lst;

        FoldersBLL foldersBLL = new FoldersBLL();
        Folders folders = new Folders();
        strHQL = "FROM Folders as folders where folders.KeyWord =" + "'" + strKeyWord + "'" + " and folders.OwnerCode = " + "'" + strUserCode + "'";
        lst = foldersBLL.GetAllFolderss(strHQL);

        if (lst.Count == 0)
        {
            folders.Name = strFolderName;
            folders.Total = 0;
            folders.NoReader = 0;
            folders.Contain = 0;
            folders.CreateDate = DateTime.Now;
            folders.Flag = 0;
            folders.OwnerCode = strUserCode;
            folders.KeyWord = strKeyWord;

            try
            {
                foldersBLL.AddFolders(folders);
            }
            catch
            {
            }
        }
    }

    protected string GetMailFolderID(string strUserCode, string strKeyWord)
    {
        string strHQL;
        IList lst;

        strHQL = "from Folder as folder where folder.UserCode = " + "'" + strUserCode + "'" + " and folder.KeyWord = " + "'" + strKeyWord + "'";
        FoldersBLL folderBLL = new FoldersBLL();
        lst = folderBLL.GetAllFolderss(strHQL);

        Folders folder = (Folders)lst[0];

        return folder.FolderID.ToString();
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName.Trim();

    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }



}
