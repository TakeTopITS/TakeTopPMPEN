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

public partial class TakeTopSoftRent_SendSiteBackupDocAuto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSiteName, strSiteAppName;

        strSiteName = Request.QueryString["SiteName"];
        strSiteAppName = Request.QueryString["SiteAppName"];

        LB_Message.Text = LanguageHandle.GetWord("ZhengZaiBeiFenNiDeZhanDianWenJ");

        if (Page.IsPostBack == false)
        {
            if (VerifyWebSiteAppIsExist(strSiteName, strSiteAppName))
            {
                try
                {
                    //����վ�㱸���ļ�
                    SendSiteBackupDoc();
                    LB_Message.Text = LanguageHandle.GetWord("ZhanDianBeiFenWenJianFaSongChe");
                }
                catch
                {
                    LB_Message.Text = LanguageHandle.GetWord("TiShiFaSongShiBaiQingJianCha");
                }
            }
            else
            {
                LB_Message.Text = LanguageHandle.GetWord("TiShiCiZhanDianBuCunZaiQingJia");
            }
        }

        IMB_Process.Visible = false;
        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>displayRelatedUI();</script>");
    }

    //����վ�㱸���ļ�
    protected void SendSiteBackupDoc()
    {
        string strSiteName, strSiteAppName, strSiteDirectory, strSiteVirtualDirectoryPhysicalPath, strSiteTemplateDirectory, strDBLoginUserID, strDBUserLoginPassword, strSiteDBName, strSiteAppSystemName, strSiteAppURL, strRentProductName, strRentUserEMail, strServerType;

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
        strSiteVirtualDirectoryPhysicalPath = Request.QueryString["SiteVirtualDirectoryPhysicalPath"];
        strServerType = Request.QueryString["ServerType"];

        strDBLoginUserID = strDBLoginUserID.ToLower();
        strSiteDBName = strSiteDBName.ToLower();


        ////����POSTGRESQL���ݿ�Ļ�������
        //try
        //{
        //    ShareClass.ConfigPostgreSqlPGPassFile(strSiteDBName);
        //}
        //catch (Exception err)
        //{
        //    LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        //}

        //�������ݿ�
        ShareClass.BackupOEMSiteDB(strSiteDBName, strSiteVirtualDirectoryPhysicalPath + "\\BackupDB","SiteCreator");

        //ѹ�������ɱ����ļ�
        string strZipDocName = Request.QueryString["ZipDocName"];
        string strDocBackupPath = strSiteDirectory + @"\BackupDoc";
        string strZipDocPath = strDocBackupPath + @"\" + strZipDocName;
        string strDownloadDocURL = strSiteAppURL + @"/BackupDoc/" + strZipDocName;
        ShareClass.CreateDirectory(strDocBackupPath);
        ZipFile(strSiteVirtualDirectoryPhysicalPath, strZipDocPath);
    }

    //ֱ��ɾ��ָ��Ŀ¼�µ������ļ����ļ���
    public void DeleteDirectory(string strDirectory)
    {
        try
        {
            string strSiteVirtualDirectoryPhysicalPath, strSiteDBName;
            strSiteDBName = Request.QueryString["SiteDBName"];
            strSiteVirtualDirectoryPhysicalPath = Request.QueryString["SiteVirtualDirectoryPhysicalPath"];

            ////�������ݿ�
            //ShareClass.BackupOEMSiteDB(strSiteDBName, strSiteVirtualDirectoryPhysicalPath + "\\BackupDB");

            //ȥ���ļ��к����ļ���ֻ������
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


    protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// ����ѹ�����ķ���
    /// </summary>
    /// <param name="strFile"></param>
    /// <param name="strZip"></param>
    /// <param name="sPassWord"></param>
    public void ZipFile(string strFile, string strZip, string sPassWord)
    {
        if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            strFile += Path.DirectorySeparatorChar;
        ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
        if (sPassWord != "")
        {
            s.Password = sPassWord; //Zipѹ���ļ�����
        }
        s.SetLevel(6);
        zip(strFile, s, strFile);
        s.Finish();
        s.Close();
    }
    /// <summary>
    /// ѹ���ļ���
    /// </summary>
    /// <param name="strFile"></param>
    /// <param name="strZip"></param>
    /// <param name="sPassWord"></param>
    public void ZipFile(string strFile, string strZip)
    {
        if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            strFile += Path.DirectorySeparatorChar;
        ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
        s.SetLevel(6);
        zip(strFile, s, strFile);
        s.Finish();
        s.Close();
    }
    private void zip(string strFile, ZipOutputStream s, string staticFile)
    {
        if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
        Crc32 crc = new Crc32();
        string[] filenames = Directory.GetFileSystemEntries(strFile);
        foreach (string file in filenames)
        {

            if (Directory.Exists(file))
            {
                zip(file, s, staticFile);
            }

            else // ����ֱ��ѹ���ļ�
            {
                //��ѹ���ļ�
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(tempfile);

                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);
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
}
