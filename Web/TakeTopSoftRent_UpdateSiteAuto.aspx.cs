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
using System.Diagnostics;
using System.Text.RegularExpressions;

///������dll
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using log4net.Config;
using Npgsql;

public partial class TakeTopSoftRent_UpdateSiteAuto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSiteName, strSiteAppName;

        strSiteName = Request.QueryString["SiteName"];
        strSiteAppName = Request.QueryString["SiteAppName"];

        LB_Message.Text = "�����������Ӧ��վ�㣬�����Ҫ5���ӣ������ĵȺ�......";

        if (Page.IsPostBack == false)
        {
            if (VerifyWebSiteAppIsExist(strSiteName, strSiteAppName))
            {
                try
                {
                    UpdateSite();
                    LB_Message.Text = "վ�������ɹ���";
                }
                catch
                {
                    LB_Message.Text = "վ������ʧ�ܣ����飡";
                }
            }
            else
            {
                LB_Message.Text = "��ʾ����վ�㲻���ڣ����飡";
            }

            IMB_Process.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>displayRelatedUI();</script>");
        }
    }

    //����վ��
    protected void UpdateSite()
    {
        string strSiteID, strRentProductType, strRentProductVersionType, strRentProductVersion, strSiteName, strSiteAppName, strSiteDirectory, strSiteTemplateDirectory, strDBLoginUserID, strDBUserLoginPassword, strSiteDBName, strSiteAppSystemName, strSiteAppURL, strRentProductName, strRentUserEMail, strServerType, strIsOEM;

        strSiteID = Request.QueryString["SiteID"];
        strSiteName = Request.QueryString["SiteName"];
        strSiteAppName = Request.QueryString["SiteAppName"];
        strSiteDirectory = Request.QueryString["SiteDirectory"];
        strSiteTemplateDirectory = Request.QueryString["SiteTemplateDirectory"];

        strDBLoginUserID = Request.QueryString["DBLoginUserID"];
        strDBUserLoginPassword = Request.QueryString["DBUserLoginPassword"];
        strSiteDBName = Request.QueryString["SiteDBName"];
        strSiteAppSystemName = Request.QueryString["SiteAppSystemName"];
        strSiteAppURL = Request.QueryString["SiteAppURL"];
        strRentProductName = Request.QueryString["RentProductName"];
        strRentUserEMail = Request.QueryString["RentUserEMail"];
        strServerType = Request.QueryString["ServerType"];
        strIsOEM = Request.QueryString["IsOEM"];

        strRentProductType = Request.QueryString["RentProductType"];
        strRentProductVersion = Request.QueryString["RentProductVersion"];
        if (strRentProductVersion == "���Ű�")
        {
            strRentProductVersionType = "YES";
        }
        else
        {
            strRentProductVersionType = "NO";
        }

        strDBLoginUserID = strDBLoginUserID.ToLower();
        strSiteDBName = strSiteDBName.ToLower();

        string strHQL;
        strHQL = "Select * From T_RentSiteBaseData Where RentProductName = '" + strRentProductName + "' and RentProductVersion = '" + strRentProductVersion + "' and IsCanUse = 'YES'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteBaseData");
        if (ds.Tables[0].Rows.Count == 0)
        {
            LB_Message.Text = "����ʧ�ܣ�û����ѡȡ�Ĳ�Ʒ�汾�Ŀ���վ��������������飡";
            return;
        }
        strRentProductType = ds.Tables[0].Rows[0]["RentProductType"].ToString().Trim();

        //ɾ��վ���µ������ļ�
        DeleteDirectory(strSiteDirectory);

        //������վ���ļ����Ƶ�վ��
        CopySiteFile(strSiteTemplateDirectory, strSiteDirectory);

        //ModifyWebConfigDBConnectionString �޸�web.config���������ݿ���ַ�����ƽ̨���ƺ��Ƿ�OEM��
        ShareClass.ModifyWebConfigDBConnectionStringAndSystemName(strSiteDirectory, "connection.connection_string", "SQLCONNECTIONSTRING", "extganttDataContextConnectionString",strDBLoginUserID, strDBUserLoginPassword, strSiteDBName, strSiteAppSystemName, strSiteAppURL, strRentProductType, strRentProductVersionType, strIsOEM);

        //�����Խ�վ���û�����Ȩ��
        ShareClass.GanttAllPrivilegesToSiteUser(strSiteDBName, strDBLoginUserID);

        // ������վ����ʱ��
        UpdateSiteCreateTime(strSiteID);
    }

    //ȡ���Խ�վ������ݿ����Ӵ�
    public static string GetSiteConnectString(string strSiteDBName)
    {
        string strConnectString, strDBName;

        strConnectString = ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString;
        strDBName = ShareClass.GetSystemDBName();

        strConnectString = strConnectString.Replace("=" + strDBName, "=" + strSiteDBName);

        return strConnectString;
    }


    //������վ����ʱ��
    protected void UpdateSiteCreateTime(string strSiteID)
    {
        string strHQL;

        strHQL = "Update T_RentSiteInfoByCustomer Set SiteCreateTime = now() Where ID = " + strSiteID;
        ShareClass.RunSqlCommand(strHQL);
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

    //-------------����վ���ļ�------------------------------------------------------------------
    public static bool CopySiteFile(string strFromDirectory, string strToDirectory)
    {
        try
        {
            //û��Ŀ¼�ľʹ���Ŀ¼
            ShareClass.CreateDirectory(strToDirectory);

            bool blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory, false);
            return blCopy;
        }
        catch
        {
            return false;
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

}