using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using System.Security.Cryptography;
using System.Security.Permissions;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;

using ZedGraph;
using ZedGraph.Web;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTProjectManageSAAS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ��jack.erp@gmail.com)
        //̩�������TakeTop Software)2006��2012
        string strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "��Ŀ����", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        if (Page.IsPostBack != true)
        {
        }
    }

}
