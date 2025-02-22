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

public partial class TTAppMyWorkDetailMain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strWLID = Request.QueryString["WLID"];
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();

        Response.Redirect("TTAppMyWorkDetailMainUpDown.aspx?WLID=" + strWLID + "&RelatedType=Null");
    }

    /// ���� Agent �ж��Ƿ���IOS�豸
    ///
    ///
    public bool CheckAgentIsIOSDevice()
    {
        bool flag = false;
        string agent = HttpContext.Current.Request.UserAgent;
        string[] keywords = { "iPhone", "iPod", "iPad" };
        //�ų�Window ����ϵͳ �� ƻ������ϵͳ
        if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
        {
            foreach (string item in keywords)
            {
                if (agent.Contains(item))
                {
                    flag = true;
                    break;
                }
            }
        }
        return flag;
    }
}
