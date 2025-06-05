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
using System.Diagnostics;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TakeTopSoftRent_DeleteSiteAuto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSiteName, strSiteAppName;

        strSiteName = Request.QueryString["SiteName"];
        strSiteAppName = Request.QueryString["SiteAppName"];

        LB_Message.Text = LanguageHandle.GetWord("ZhengZaiShanChuNiDeYingYongZha");

        if (Page.IsPostBack == false)
        {
            if (VerifyWebSiteAppIsExist(strSiteName, strSiteAppName))
            {
                try
                {
                    //ɾ��վ��
                    DeleteSite();
                    LB_Message.Text = LanguageHandle.GetWord("ZhanDianShanChuChengGong");
                }
                catch (Exception err)
                {
                    LB_Message.Text = LanguageHandle.GetWord("ShanChuShiBaiQingJianCha");
                }
            }
            else
            {
                LB_Message.Text = LanguageHandle.GetWord("TiShiCiZhanDianBuCunZaiQingJia");
            }

            IMB_Process.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>displayRelatedUI();</script>");
        }
    }

    //ɾ��վ��
    protected void DeleteSite()
    {
        string strSiteName, strSiteAppName, strSiteAppSystemName, strSiteDirectory, strSiteVirtualDirectoryPhysicalPath, strSiteTemplateDirectory, strDBLoginUserID, strDBUserLoginPassword, strSiteDBName, strSiteAppURL, strRentProductName, strRentUserEMail, strServerType;

        strSiteAppName = Request.QueryString["SiteAppName"];
        strSiteAppSystemName = Request.QueryString["SiteAppSystemName"];

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

        ////����POSTGRESQL���ݿ�Ļ�������
        //try
        //{
        //    ShareClass.ConfigPostgreSqlPGPassFile(strSiteDBName);
        //}
        //catch (Exception err)
        //{
        //    LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        //}


        DeleteSiteApp(strSiteName, strSiteAppName);

        DeleteSiteApplicationPool(strSiteAppName + "Pool");

        //ɾ��վ���µ������ļ�
        DeleteDirectory(strSiteDirectory);

        try
        {
            //�������ݿ�
            string strBackupPath = strSiteVirtualDirectoryPhysicalPath + "\\BackupDB";
            ShareClass.BackupOEMSiteDB(strSiteDBName, strBackupPath, "SiteCreator");

            ShareClass.DeleteSiteDBAndDBLoginUserID(strSiteDBName, strDBLoginUserID);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //ֱ��ɾ��ָ��Ŀ¼�µ������ļ����ļ���
    public static void DeleteDirectory(string strDirectory)
    {
        try
        {
            //ȥ���ļ��к����ļ���ֻ������
            //ȥ���ļ��е�ֻ������
            System.IO.DirectoryInfo fileInfo = new DirectoryInfo(strDirectory);
            fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

            //ȥ���ļ���ֻ������
            System.IO.File.SetAttributes(strDirectory, System.IO.FileAttributes.Normal);

            //�ж��ļ����Ƿ񻹴���
            if (Directory.Exists(strDirectory))
            {
                foreach (string f in Directory.GetFileSystemEntries(strDirectory))
                {
                    if (File.Exists(f))
                    {
                        try
                        {
                            //��������ļ�ɾ���ļ�
                            File.Delete(f);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            if (!f.Contains("Logo"))
                            {
                                //ѭ���ݹ�ɾ�����ļ���
                                DeleteDirectory(f);
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                //ɾ�����ļ���
                Directory.Delete(strDirectory);
            }
        }
        catch (Exception ex) // �쳣����
        {
        }
    }



    /// <summary> 
    /// ɾ��һ����վ�����Ӧ��
    /// </summary> 
    /// <param name="siteName">Site name.</param> 
    public void DeleteSiteApp(string siteName, string siteAppName)
    {
        using (ServerManager mgr = new ServerManager())
        {
            Site site = mgr.Sites[siteName];

            Application app = site.Applications["/" + siteAppName];
            if (app != null)
            {
                mgr.Sites[siteName].Applications.Remove(app);
                mgr.CommitChanges();
            }
        }
    }

    //ɾ��һ��Ӧ�ó����
    public void DeleteSiteApplicationPool(String poolName)
    {
        ServerManager iisManager = new ServerManager();
        ApplicationPool appPool = iisManager.ApplicationPools[poolName];
        iisManager.ApplicationPools.Remove(appPool); iisManager.CommitChanges();
    }

    //�ж�վ��Ӧ���Ƿ����
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

