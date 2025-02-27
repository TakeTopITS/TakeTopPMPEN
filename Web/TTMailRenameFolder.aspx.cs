using System; using System.Resources;
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

public partial class MailRenameFolder : System.Web.UI.Page
{
	int nFolderID = -1;
	protected void Page_Load(object sender,EventArgs e)
	{
        string strUserCode = Session["UserCode"].ToString();

        ///��ȡ����nFolderID��ֵ
        if (Request.Params["FolderID"] != null)
		{
			if(Int32.TryParse(Request.Params["FolderID"].ToString(),out nFolderID) == false)
			{
				return;
			}
		}
		if(!Page.IsPostBack)
		{   ///��ʾ�ļ��е�����
			if(nFolderID > -1)
			{
				BindFolderData(nFolderID);
			}
		}
	}

	private void BindFolderData(int nFolderID)
	{
		IFolder folder = new Folder();
		NpgsqlDataReader dr = folder.GetSingleFolder(nFolderID);
		if(dr.Read())
		{
			Name.Text = dr["Name"].ToString();
		}
		dr.Close();
	}
	
	protected void NewBtn_Click(object sender,EventArgs e)
	{
		try
		{   ///�������
			IFolder folder = new Folder();
			///ִ�����ݿ����
			folder.RenameFolder(nFolderID,Name.Text.Trim());
			Response.Write("<script>alert('" + LanguageHandle.GetWord("XiuGaiShuJuChengGongQingTuoSha").ToString().Trim() + "');</script>");
		}
		catch(Exception ex)
		{   ///��ת���쳣������ҳ��
			Response.Redirect("TTErrorPage.aspx?ErrorMsg=" + ex.Message.Replace("<br>","").Replace("\n","")
				+ "&ErrorUrl=" + Request.Url.ToString().Replace("<br>","").Replace("\n",""));
		}
	}
	protected void ReturnBtn_Click(object sender,EventArgs e)
	{   ///���ص��ʼ��б�ҳ��
		Response.Redirect("~/TTMailDesktop.aspx");
	}	
}
