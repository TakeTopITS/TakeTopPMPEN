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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class NewFolder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
	protected void NewBtn_Click(object sender,EventArgs e)
	{
        string strUserCode = Session["UserCode"].ToString();

		try
		{   ///�������
			IFolder folder = new Folder();
			///ִ�����ݿ����
			folder.NewFolder(Name.Text.Trim(),strUserCode);
			Response.Write("<script>alert('" + LanguageHandle.GetWord("TianJiaShuJuChengGongQingTuoSh").ToString().Trim() + "');</script>");
            
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
