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
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTTakeTopIMMain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strChatterCode;

        strChatterCode = Request.QueryString["ChatterCode"];

        //this.Title = "TakeTopIM---" + ShareClass.GetUserName(strChatterCode);
    }
}