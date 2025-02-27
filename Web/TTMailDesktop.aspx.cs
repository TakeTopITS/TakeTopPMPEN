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
using System.Data.SqlClient;

using Npgsql;
using NpgsqlTypes;

public partial class MailDesktop : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                BindFolderData();
            }
            catch
            {

            }
        }
    }
    private void BindFolderData()
    {
        string strUserCode = Session["UserCode"].ToString();

        ///��ȡ����
		IFolder folder = new Folder();
        NpgsqlDataReader dr = folder.GetFolders(strUserCode);
        ///������
        FolderView.DataSource = dr;
        FolderView.DataBind();
        dr.Close();
    }

    protected void NewFolderBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TTMailNewFolder.aspx");
    }

    protected void FolderView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "delete")
        {
            try
            {   ///ɾ������
				IFolder folder = new Folder();
                folder.DeleteFolder(Int32.Parse(e.CommandArgument.ToString()));

                ///���°󶨿ؼ�������				
                BindFolderData();
                Response.Write("<script>alert('" + LanguageHandle.GetWord("ShanChuShuJuChengGongQingTuoSh").ToString().Trim() + "');</script>");
            }
            catch (Exception ex)
            {   ///��ת���쳣������ҳ��
				Response.Redirect("TTErrorPage.aspx?ErrorMsg=" + ex.Message.Replace("<br>", "").Replace("\n", "")
                    + "&ErrorUrl=" + Request.Url.ToString().Replace("<br>", "").Replace("\n", ""));
            }
        }
    }
    protected void FolderView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ///
    }
    protected void FolderView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ImageButton deleteBtn = (ImageButton)e.Row.FindControl("DeleteBtn");
        if (deleteBtn != null)
        {
            deleteBtn.Attributes.Add("onclick", "return confirm('��ȷ��Ҫɾ����ѡ�����������');"); 
        }
    }
}
