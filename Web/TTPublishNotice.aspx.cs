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
using System.IO;


using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;

public partial class TTPublishNotice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName, strDepartCode, strDepartString;

        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;
        TB_Author.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "公告发布", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityNewsNotice(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);

            strHQL = "from PublicNotice as publicNotice where publicNotice.UploadManCode = " + "'" + strUserCode + "'" + " and publicNotice.DocType in ('AnnouncementDocument','NotificationDocument') and publicNotice.Status <> 'Deleted' Order by publicNotice.DocID DESC";
            PublicNoticeBLL publicNoticeBLL = new PublicNoticeBLL();
            lst = publicNoticeBLL.GetAllPublicNotices(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_Sql.Text = strHQL;

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            TB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text.Trim();
            string strUserName = ShareClass.GetUserName(strUserCode);
            string strDocID = e.Item.Cells[0].Text.Trim();
            string strDocName = e.Item.Cells[1].Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();


            strHQL = "from PublicNotice as publicNotice where publicNotice.DocID = " + strDocID;
            PublicNoticeBLL publicNoticeBLL = new PublicNoticeBLL();
            lst = publicNoticeBLL.GetAllPublicNotices(strHQL);
            PublicNotice publicNotice = (PublicNotice)lst[0];

            string strUploadManName = publicNotice.UploadManName.Trim();

            if (strUserName == strUploadManName)
            {
                publicNotice.Status = "Deleted";
                publicNoticeBLL.UpdatePublicNotice(publicNotice, int.Parse(strDocID));

                strHQL = "from PublicNotice as publicNotice where publicNotice.UploadManCode= " + "'" + strUserCode + "'" + " and publicNotice.DocType in ('AnnouncementDocument','NotificationDocument') and publicNotice.Status <> 'Deleted' Order by publicNotice.DocID DESC";
                publicNoticeBLL = new PublicNoticeBLL();
                lst = publicNoticeBLL.GetAllPublicNotices(strHQL);
                DataGrid1.DataSource = lst;
                DataGrid1.DataBind();

                //设置缓存更改标志，并刷新页面缓存
                ChangePageCache();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFFCZNBNSCBRSCDWJ").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BtnUP_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string strHQL;
            IList lst;

            string strUserCode = LB_UserCode.Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();
            string strAuthor = TB_Author.Text.Trim();
            string strDocType = DL_DocType.SelectedValue.Trim();
            string strDepartCode = TB_DepartCode.Text.Trim();
            string strDepratName = LB_DepartName.Text.Trim();
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (strDepartCode == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGSBMBNWKSCSBJC").ToString().Trim() + "')", true);

                return;
            }

            if (fi.Exists)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
            }
            else
            {
                PublicNoticeBLL publicNoticeBLL = new PublicNoticeBLL();
                PublicNotice publicNotice = new PublicNotice();

                publicNotice.DocType = strDocType;
                publicNotice.Author = strAuthor;
                publicNotice.DocName = strFileName2;
                publicNotice.Address = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                publicNotice.UploadManCode = strUserCode;
                publicNotice.UploadManName = ShareClass.GetUserName(strUserCode);
                publicNotice.UploadTime = DateTime.Now;
                publicNotice.RelatedDepartCode = strDepartCode;
                publicNotice.RelatedDepartName = strDepratName;
                publicNotice.Scope = DL_Scope.SelectedValue.Trim();
                publicNotice.Status = "Publish";   


                try
                {
                    publicNoticeBLL.AddPublicNotice(publicNotice);

                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);

                    strHQL = "from PublicNotice as publicNotice where publicNotice.UploadManCode = " + "'" + strUserCode + "'" + " and publicNotice.DocType in ('AnnouncementDocument','NotificationDocument') and publicNotice.Status <> 'Deleted' Order by publicNotice.DocID DESC";
                    publicNoticeBLL = new PublicNoticeBLL();
                    lst = publicNoticeBLL.GetAllPublicNotices(strHQL);
                    DataGrid1.DataSource = lst;
                    DataGrid1.DataBind();

                    LB_Sql.Text = strHQL;

                    //设置缓存更改标志，并刷新页面缓存
                    ChangePageCache();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);

        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        PublicNoticeBLL publicNoticeBLL = new PublicNoticeBLL();
        IList lst = publicNoticeBLL.GetAllPublicNotices(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {
        //设置缓存更改标志
        ShareClass.SetPageCacheMark("1");
        Session["CssDirectoryChangeNumber"] = "1";

        //设置缓存更改标志
        ShareClass.AddSpaceLineToLeftColumnForRefreshCache();
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
