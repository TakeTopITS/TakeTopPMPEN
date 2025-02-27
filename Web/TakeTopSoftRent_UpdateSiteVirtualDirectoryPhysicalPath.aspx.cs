using System;
using System.Resources;
using System.Data;
using System.Configuration.Internal;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Microsoft.Web.Administration;
using System.DirectoryServices;
using System.Xml;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TakeTopSoftRent_UpdateSiteVirtualDirectoryPhysicalPath : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSiteName, strSiteAppName;

        strSiteName = Request.QueryString["SiteName"];
        strSiteAppName = Request.QueryString["SiteAppName"];

        LB_Message.Text = LanguageHandle.GetWord("ZhengZaiBaoCunYingYongZhanDian").ToString().Trim();

        if (Page.IsPostBack == false)
        {
            try
            {
                //更新站点虚拟目录
                UpdateVirtualDirectoryPhysicalPath();
                LB_Message.Text = LanguageHandle.GetWord("XuNiMuLuDeJueDuiLuJingBaoCunCh").ToString().Trim();
            }
            catch
            {
                LB_Message.Text = LanguageHandle.GetWord("BaoCunShiBaiQingJianCha").ToString().Trim();
            }

            IMB_Process.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>displayRelatedUI();</script>");
        }
    }

    //删除站点
    protected void UpdateVirtualDirectoryPhysicalPath()
    {
        string strSiteID, strSiteName, strSiteAppName, strSiteAppSystemName, strSiteDirectory, strSiteVirtualDirectoryPhysicalPath, strSiteTemplateDirectory, strDBLoginUserID, strDBUserLoginPassword, strSiteDBName, strSiteAppURL, strRentProductName, strRentUserEMail, strServerType;

        strSiteAppName = Request.QueryString["SiteAppName"];
        strSiteAppSystemName = Request.QueryString["SiteAppSystemName"];

        strSiteID = Request.QueryString["SiteID"];
        strSiteName = Request.QueryString["SiteName"];
        strSiteDirectory = Request.QueryString["SiteDirectory"];
        strSiteTemplateDirectory = Request.QueryString["SiteTemplateDirectory"];

        strDBLoginUserID = Request.QueryString["DBLoginUserID"];
        strDBUserLoginPassword = Request.QueryString["DBUserLoginPassword"];
        strSiteDBName = Request.QueryString["SiteDBName"];
        strSiteAppURL = Request.QueryString["SiteAppURL"];
        strRentProductName = Request.QueryString["RentProductName"];
        strRentUserEMail = Request.QueryString["RentUserEMail"];
        strSiteVirtualDirectoryPhysicalPath = Request.QueryString["SiteVirtualDirectoryPhysicalPath"];
        strServerType = Request.QueryString["ServerType"];

        try
        {
            if (VerifyWebSiteAppIsExist(strSiteName, strSiteAppName))
            {
                DeleteSiteAPPVDir(strSiteName, strSiteAppName, "/Doc");
                CreateSiteAppVDir(strSiteName, strSiteAppName, "/Doc", strSiteVirtualDirectoryPhysicalPath);
            }
            else
            {
                LB_Message.Text = LanguageHandle.GetWord("DiShiCiZhanDianBuCunZaiQingJia").ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            LB_Message.Text = LanguageHandle.GetWord("GengXinShiBaiQingJianCha").ToString().Trim();
        }
    }

    //删除一个网站应用的虚拟目录
    public void DeleteSiteAPPVDir(string siteName, string siteAppName, string vDirName)
    {
        using (ServerManager mgr = new ServerManager())
        {
            Site site = mgr.Sites[siteName];
            if (site == null)
            {
                throw new ApplicationException(String.Format("Web site {0} does not exist", siteName));
            }

            Application app = site.Applications["/" + siteAppName];
            if (app == null)
            {
                throw new ApplicationException(String.Format("Web site app {0} does not exist", siteAppName));
            }

            if (site != null)
            {
                if (app != null)
                {
                    app.VirtualDirectories.Remove(app.VirtualDirectories[vDirName]);

                   
                    mgr.CommitChanges();
                }
            }
        }
    }

    //创建一个网站应用的虚拟目录
    public void CreateSiteAppVDir(string siteName, string siteAppName, string vDirName, string physicalPath)
    {
        using (ServerManager mgr = new ServerManager())
        {
            Site site = mgr.Sites[siteName];
            if (site == null)
            {
                throw new ApplicationException(String.Format("Web site {0} does not exist", siteName));
            }

            Application app = site.Applications["/" + siteAppName];
            if (app == null)
            {
                throw new ApplicationException(String.Format("Web site app {0} does not exist", siteAppName));
            }

            try
            {
                //没有目录的就创建目录
                ShareClass.CreateDirectory(physicalPath);
                app.VirtualDirectories.Add(vDirName, physicalPath);
                mgr.CommitChanges();
            }
            catch
            {
            }
        }
    }

    //更新一个网站应用的虚拟目录
    public void UpdateSiteAppVDir(string siteName, string siteAppName, string vDirName, string physicalPath)
    {
        using (ServerManager mgr = new ServerManager())
        {
            Site site = mgr.Sites[siteName];

            if (site == null)
            {
                throw new ApplicationException(String.Format("Web site {0} does not exist", siteName));
            }
            Application app = site.Applications[siteAppName];
            if (app == null)
            {
                throw new ApplicationException(String.Format("Web site app {0} does not exist", siteAppName));
            }

            if (site != null)
            {
                if (app != null)
                {
                    app.VirtualDirectories[vDirName].PhysicalPath = physicalPath;
                    mgr.CommitChanges();

                    LB_Message.Text = app.VirtualDirectories[vDirName].PhysicalPath;
                }
            }
        }
    }

    //判断站点应用是否存在
    public bool VerifyWebSiteAppIsExist(string siteName, string siteAppName)
    {
        using (ServerManager mgr = new ServerManager())
        {
            Site site = mgr.Sites[siteName];

            Application app = site.Applications["/" + siteAppName];

            if (app != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

