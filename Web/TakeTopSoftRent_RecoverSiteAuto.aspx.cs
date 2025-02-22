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
using NickLee.Common.SmartWin32;

public partial class TakeTopSoftRent_RecoverSiteAuto : System.Web.UI.Page
{
    string strWebSite;
    string strSiteAppSystemName;
    string strSiteAppName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strWebSite = Request.QueryString["WebSite"];
        if (strWebSite == null)
        {
            strWebSite = "";
        }

        strSiteAppSystemName = Request.QueryString["SiteAppSystemName"];
        strSiteAppName = Request.QueryString["SiteAppName"];

        if (Page.IsPostBack == false)
        {
            if (strSiteAppSystemName != null & strSiteAppName != null)
            {
                CreateSite(strSiteAppSystemName, strSiteAppName);
            }

            ClientScript.RegisterStartupScript(GetType(), "dd", "<script>displayRelatedUI();</script>");
        }
    }

    protected void CreateSite(string strSiteAppSystemName, string strSiteAppName)
    {
        try
        {
            string strRentUserPhoneNumber = Request.QueryString["RentUserPhoneNumber"];
            string strRentUserEMail = Request.QueryString["RentUserEMail"];
            string strRentUserName = Request.QueryString["RentUserName"];
            string strRentUserCompanyName = Request.QueryString["RentUserCompanyName"];
            string strRentProductName = Request.QueryString["RentProductName"];
            string strRentProductVersion = Request.QueryString["RentProductVersion"];
            string strRentUserNumber = Request.QueryString["RentUserNumber"];
            string strSiteID = Request.QueryString["SiteID"];

            string strRentProductType = Request.QueryString["RentProductType"];
            string strSiteCreatorName = strRentUserName;

            string strSiteName = Request.QueryString["SiteName"];
            string strSiteURL = Request.QueryString["SiteURL"];
            string strSiteAppURL = Request.QueryString["SiteAppURL"];
            string strSiteBindingInfo = Request.QueryString["SiteBindingInfo"];
            string strSiteDirectory = Request.QueryString["SiteDirectory"];
            string strSiteTemplateDirectory = Request.QueryString["SiteTemplateDirectory"];
            string strSiteVirtualDirectoryName = Request.QueryString["SiteVirtualDirectoryName"];
            string strSiteVirtualDirectoryPhysicalPath = Request.QueryString["SiteVirtualDirectoryPhysicalPath"];
            string strSiteDBName = Request.QueryString["SiteDBName"];
            string strSiteDBRestoreFile = Request.QueryString["SiteDBRestoreFile"];
            string strSiteDBSetupDirectory = Request.QueryString["SiteDBSetupDirectory"];
            string strSiteDBLoginUserID = Request.QueryString["SiteDBLoginUserID"];
            string strSiteDBUserLoginPassword = Request.QueryString["SiteDBUserLoginPassword"];
            string strServerType = Request.QueryString["ServerType"];

            string strRentProductVersionType;
            if (strRentProductVersion == "GroupEdition")
            {
                strRentProductVersionType = "YES";
            }
            else
            {
                strRentProductVersionType = "NO";
            }

            string strIsOEM = Request.QueryString["IsOEM"];


            if (VerifyWebSiteAppIsExist(strSiteName, strSiteAppName))
            {
                LB_Message.Text = "�ָ�ʧ�ܣ�������ͬ���Ƶ�վ�㣬���飡";
                return;
            }

            //�ָ�վ��Ӧ��
            RecoverSiteAPP(strRentProductType, strRentProductVersionType, strSiteAppSystemName, strSiteAppName, strSiteAppURL, strSiteName, strSiteBindingInfo, strSiteDirectory, strSiteTemplateDirectory, strSiteVirtualDirectoryName, strSiteVirtualDirectoryPhysicalPath, strSiteDBName, strSiteDBRestoreFile, strSiteDBSetupDirectory, strSiteDBLoginUserID, strSiteDBUserLoginPassword, strIsOEM);

            LB_Message.Text = "վ��ָ��ɹ�������Դ򿪺��ղش�վ�� <br/> <a href='" + strSiteAppURL + "' target='_blank'>" + strSiteAppURL + "</a>";
            LB_CloseMessage.Visible = false;

            //LB_Message.Text = strSiteDBRestoreFile;
        }
        catch (Exception ex)
        {
            LB_Message.Text = ex.Message.ToString();
        }
    }

    //-------------�ָ�վ��Ӧ��------------------------------------------------------------------
    /// <summary> 
    /// �ָ�һ��վ��Ӧ��
    /// </summary> 
    /// <param name="siteName"></param> 
    /// <param name="bindingInfo">"*:&lt;port&gt;:&lt;hostname&gt;" <example>"*:80:myhost.com"</example></param> 
    /// <param name="physicalPath"></param> 
    public void RecoverSiteAPP(string strRentProductType, string strRentProductVersionType, string strSysteName, string strSiteAppName, string strSiteAppURL, string strSiteName, string strBindingInfo, string strSiteDirectory, string strSiteTemplateDirectory, string strSiteVirtualDirectoryName, string strSiteVirtualDirectoryPhysicalPath, string strSiteDBName, string strDBRestoreFile, string strDBSetupDirectory, string strDBLoginUserID, string strDBUserLoginPassword, string strIsOEM)
    {
        //������վ���ļ����Ƶ�վ��
        CopySiteFile(strSiteTemplateDirectory, strSiteDirectory);

        ////����POSTGRESQL���ݿ�Ļ�������
        //try
        //{
        //    ShareClass.ConfigPostgreSqlPGPassFile(strSiteDBName);
        //}
        //catch (Exception err)
        //{
        //    LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        //}

        //������ݿ��¼�û�
        ShareClass.CreateDBUserAccount(strDBLoginUserID, strDBUserLoginPassword, "YES");

        //���û��������ݿ�ָ����ݿ�
        ShareClass.RestoreDatabaseFromOEMUserDB(strSiteDBName, strDBRestoreFile);

        //�����û����ݿ�Ȩ��
        ShareClass.AuthorizationDBToUser(strDBLoginUserID,strDBUserLoginPassword, strSiteDBName, "YES");

        //ModifyWebConfigDBConnectionString �޸�web.config���������ݿ���ַ�����ƽ̨���ƺ��Ƿ�OEM��
        ShareClass.ModifyWebConfigDBConnectionStringAndSystemName(strSiteDirectory, "connection.connection_string", "SQLCONNECTIONSTRING", "extganttDataContextConnectionString", strDBLoginUserID, strDBUserLoginPassword, strSiteDBName, strSysteName, strSiteAppURL, strRentProductType, strRentProductVersionType, strIsOEM);

        //C:\Windows\System32\inetsrv\config ���Ŀ¼Ҫ����IIS_USER�û�ȫ��Ȩ�ޣ��������ִ���
        RecoverSiteAPP(strSiteAppName, strSiteName, "http", strBindingInfo, strSiteDirectory, true, strSiteAppName + "Pool", ProcessModelIdentityType.NetworkService, null, null, ManagedPipelineMode.Integrated, null);

        //�ָ���վӦ�õ�����Ŀ¼
        CreateSiteAppVDir(strSiteName, strSiteAppName, strSiteVirtualDirectoryName, strSiteVirtualDirectoryPhysicalPath);
    }

    //�ָ�һ����վ��Ӧ��
    private void RecoverSiteAPP(string strSiteAppName, string strSiteName, string protocol, string bindingInformation, string physicalPath,
           bool createAppPool, string appPoolName, ProcessModelIdentityType identityType,
           string appPoolUserName, string appPoolPassword, ManagedPipelineMode appPoolPipelineMode, string managedRuntimeVersion)
    {
        using (ServerManager mgr = new ServerManager())
        {
            Application app;
            try
            {
                app = mgr.Sites[strSiteName].Applications.Add("/" + strSiteAppName, physicalPath);
            }
            catch (Exception err)
            {
                LB_Message.Text = err.Message.ToString();
                return;
            }

            if (createAppPool)
            {
                ApplicationPool pool = mgr.ApplicationPools.Add(appPoolName);
                if (pool.ProcessModel.IdentityType != identityType)
                {
                    pool.ProcessModel.IdentityType = identityType;
                }
                if (!String.IsNullOrEmpty(appPoolUserName))
                {
                    pool.ProcessModel.UserName = appPoolUserName;
                    pool.ProcessModel.Password = appPoolPassword;
                }

                //��Ϊ����ģʽ
                pool.ManagedPipelineMode = ManagedPipelineMode.Classic;

                //����webӦ�ó���ص�Framework�汾
                pool.ManagedRuntimeVersion = "v4.0";

                //�����Ƿ�����32λӦ�ó���
                pool.SetAttributeValue("enable32BitAppOnWin64", true);
                pool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;

                app.ApplicationPoolName = pool.Name;
            }

            mgr.CommitChanges();
        }
    }


    //�ָ�һ����վӦ�õ�����Ŀ¼
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
                //û��Ŀ¼�ľͻָ�Ŀ¼
                ShareClass.CreateDirectory(physicalPath);

                app.VirtualDirectories.Add(vDirName, physicalPath);
                mgr.CommitChanges();
            }
            catch
            {
            }
        }
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

    //�����ļ���
    public static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
    {
        bool ret = false;
        try
        {
            SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
            DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

            if (Directory.Exists(SourcePath))
            {
                if (Directory.Exists(DestinationPath) == false)
                    Directory.CreateDirectory(DestinationPath);

                foreach (string fls in Directory.GetFiles(SourcePath))
                {
                    FileInfo flinfo = new FileInfo(fls);
                    flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                }
                foreach (string drs in Directory.GetDirectories(SourcePath))
                {
                    DirectoryInfo drinfo = new DirectoryInfo(drs);
                    if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                        ret = false;
                }
            }
            ret = true;
        }
        catch (Exception ex)
        {
            ret = false;
        }
        return ret;
    }



    //-------------����վ���ļ�------------------------------------------------------------------
    public static bool CopySiteFile(string strFromDirectory, string strToDirectory)
    {
        try
        {
            //û��Ŀ¼�ľͻָ�Ŀ¼
            ShareClass.CreateDirectory(strToDirectory);

            bool blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory, false);
            return blCopy;
        }
        catch
        {
            return false;
        }
    }

    //----------------------------------------------------------------------------------------------
    /// <summary> 
    /// �ָ�һ��վ��
    /// </summary> 
    /// <param name="siteName"></param> 
    /// <param name="bindingInfo">"*:&lt;port&gt;:&lt;hostname&gt;" <example>"*:80:myhost.com"</example></param> 
    /// <param name="physicalPath"></param> 
    public void CreateSite(string siteName, string bindingInfo, string physicalPath)
    {
        //C:\Windows\System32\inetsrv\config ���Ŀ¼Ҫ����IIS_USER�û�ȫ��Ȩ�ޣ��������ִ���
        createSite(siteName, "http", bindingInfo, physicalPath, true, siteName + "Pool", ProcessModelIdentityType.NetworkService, null, null, ManagedPipelineMode.Integrated, null);
    }

    //�ָ�һ��վ��
    private void createSite(string siteName, string protocol, string bindingInformation, string physicalPath,
            bool createAppPool, string appPoolName, ProcessModelIdentityType identityType,
            string appPoolUserName, string appPoolPassword, ManagedPipelineMode appPoolPipelineMode, string managedRuntimeVersion)
    {
        using (ServerManager mgr = new ServerManager())
        {
            Site site;
            try
            {
                //�ָ�վ��ʱ�����
                site = mgr.Sites.Add(siteName, protocol, bindingInformation, physicalPath);
            }
            catch (Exception err)
            {
                LB_Message.Text = err.Message.ToString();
                return;
            }

            if (createAppPool)
            {
                ApplicationPool pool = mgr.ApplicationPools.Add(appPoolName);
                if (pool.ProcessModel.IdentityType != identityType)
                {
                    pool.ProcessModel.IdentityType = identityType;
                }
                if (!String.IsNullOrEmpty(appPoolUserName))
                {
                    pool.ProcessModel.UserName = appPoolUserName;
                    pool.ProcessModel.Password = appPoolPassword;
                }
                //if (appPoolPipelineMode != pool.ManagedPipelineMode)
                //{
                //    //��Ϊ����ģʽ
                //    pool.ManagedPipelineMode = appPoolPipelineMode;
                //}

                //��Ϊ����ģʽ
                pool.ManagedPipelineMode = ManagedPipelineMode.Classic;

                //����webӦ�ó���ص�Framework�汾
                pool.ManagedRuntimeVersion = "v4.0";

                //�����Ƿ�����32λӦ�ó���
                pool.SetAttributeValue("enable32BitAppOnWin64", true);

                //�ָ�վ��ʱ�����
                site.Applications["/"].ApplicationPoolName = pool.Name;
            }

            mgr.CommitChanges();
        }
    }

}