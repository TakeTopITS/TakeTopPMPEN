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
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakeWebSite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;


        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadWebSite(strUserCode);
        }
    }



    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strID;
        string strUserCode;
        string strSiteName, strSiteAddress;
        int intSortNumber;


        strSiteName = TB_SiteName.Text.Trim();
        strSiteAddress = TB_SiteAddress.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        WebSiteBLL webSiteBLL = new WebSiteBLL();
        WebSite webSite = new WebSite();

        webSite.SiteName = strSiteName;
        webSite.SiteAddress = strSiteAddress;
        webSite.SortNumber = intSortNumber;
        webSite.UserCode = strUserCode;


        try
        {
            webSiteBLL.AddWebSite(webSite);

            strID = ShareClass.GetMyCreatedMaxWebSiteID(strUserCode);
            LB_ID.Text = strID;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            LoadWebSite(strUserCode);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);

        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strID;
        string strUserCode;
        string strSiteName, strSiteAddress;
        int intSortNumber;


        strID = LB_ID.Text.Trim();
        strSiteName = TB_SiteName.Text.Trim();
        strSiteAddress = TB_SiteAddress.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        WebSiteBLL webSiteBLL = new WebSiteBLL();
        WebSite webSite = new WebSite();

        webSite.SiteName = strSiteName;
        webSite.SiteAddress = strSiteAddress;
        webSite.SortNumber = intSortNumber;
        webSite.UserCode = strUserCode;

        try
        {
            webSiteBLL.UpdateWebSite(webSite, int.Parse(strID));

            LoadWebSite(strUserCode);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID;
        string strUserCode;
        string strHQL;

        strUserCode = LB_UserCode.Text.Trim();
        strID = LB_ID.Text.Trim();

        strHQL = "Delete From T_WebSite Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            LoadWebSite(strUserCode);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text;

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From WebSite as webSite Where webSite.ID = " + strID;
            WebSiteBLL webSiteBLL = new WebSiteBLL();
            lst = webSiteBLL.GetAllWebSites(strHQL);

            WebSite webSite = (WebSite)lst[0];

            LB_ID.Text = webSite.ID.ToString();
            TB_SiteName.Text = webSite.SiteName.Trim();
            TB_SiteAddress.Text = webSite.SiteAddress.Trim();
            NB_SortNumber.Amount = webSite.SortNumber;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }


    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        IList lst;

        WebSiteBLL webSiteBLL = new WebSiteBLL();
        lst = webSiteBLL.GetAllWebSites(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadWebSite(string strUserCode)
    {
        string strHQL;
        IList lst;

        DataGrid4.CurrentPageIndex = 0;

        strHQL = "From WebSite as webSite where webSite.UserCode = " + "'" + strUserCode + "'";
        strHQL += " Order By webSite.SortNumber ASC";
        WebSiteBLL webSiteBLL = new WebSiteBLL();
        lst = webSiteBLL.GetAllWebSites(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql.Text = strHQL;
    }


    protected string GetDepartCode(string strUserCode)
    {
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        string strDepartCode = projectMember.DepartCode;

        return strDepartCode;
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
