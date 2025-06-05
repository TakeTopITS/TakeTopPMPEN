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
using System.Linq;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TakeTopSoftRent_BuildSite : System.Web.UI.Page
{
    string strWebSite;

    protected void Page_Load(object sender, EventArgs e)
    {
        strWebSite = Request.QueryString["WebSite"];
        if (strWebSite == null)
        {
            strWebSite = "";
        }

        if (Page.IsPostBack == false)
        {
            TB_SiteAppSystemName.Text = Request.QueryString["SiteAppSystemName"];
            TB_SiteAppName.Text = Request.QueryString["SiteAppName"];

            ClientScript.RegisterStartupScript(GetType(), "dd", "<script>hideRelatedUI();</script>");
        }
    }

    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strSiteAppSystemName, strSiteAppName, strIsOEM;

        strSiteAppSystemName = TB_SiteAppSystemName.Text.Trim();
        strSiteAppName = TB_SiteAppName.Text.Trim();

        LB_Message.Text = "";

        if (strSiteAppSystemName == "" | strSiteAppName == "")
        {
            LB_Message.Text = LanguageHandle.GetWord("TiShiDaiHaoDeDuBuNengKongQingJ");
        }
        else
        {

            if(!isAlphabeticNoSpace(strSiteAppName))
            {
                LB_Message.Text = LanguageHandle.GetWord("JingGaoZhanDianMingChenZhiNeng");
                return;
            }

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
                string strSiteCreatorName = strRentUserName;
                string strServerType = Request.QueryString["ServerType"];
                if (CB_IsOEM.Checked)
                {
                    strIsOEM = "YES";
                }
                else
                {
                    strIsOEM = "NO";
                }

                string strHQL;
                strHQL = "Select * From T_RentSiteBaseData Where RentProductName = '" + strRentProductName + "' and RentProductVersion = '" + strRentProductVersion + "' and IsCanUse = 'YES'";
            
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteBaseData");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    LB_Message.Text = LanguageHandle.GetWord("ChuangJianShiBaiMeiYouNiXuanQu");
                    return;
                }

                //string strSiteName = "Default Web Site";
                //string strSiteURL = @"http://localhost";
                //string strSiteBindingInfo = "*:80:";
                //string strSiteDirectory = @"D:\����վ��";
                //string strSiteTemplateDirectory = @"C:\inetpub\wwwroot\TakeTopECMP";
                //string strSiteVirtualDirectoryName = "/Doc";
                //string strSiteVirtualDirectoryPhysicalPath = @"D:\�����ĵ�\onedrive\ProjectDoc";
                //string strSiteDBName = "TakeTopMPDB";
                //string strDBRestoreFile = @"E:\WorkDrive\WebDrive\VS2017ProjectTEST\ProjectMgt\Web\BackupFile\DB\TakeTopAppDBBackup.bak";
                //string strDBSetupDirectory = @"D:\�����ĵ�\onedrive\ProjectDoc";
                //string strDBLoginUserID = "DBAdmin";

                string strRentProductType = ds.Tables[0].Rows[0]["RentProductType"].ToString().Trim();
                string strSiteCreatorAppName = ds.Tables[0].Rows[0]["SiteCreatorAppName"].ToString().Trim();
                string strSiteName = "Default Web Site";
                string strSiteURL = ds.Tables[0].Rows[0]["SiteURL"].ToString().Trim();

                string strSiteAppURL = strSiteURL + "//" + strSiteAppName;

                //if(strIsOEM == "YES")
                //{
                //    strSiteAppURL =   "http://www.ourpm.net/" + strSiteAppName;
                //}

                string strSiteBindingInfo = ds.Tables[0].Rows[0]["SiteBindingInfo"].ToString().Trim();
                string strSiteDirectory = ds.Tables[0].Rows[0]["SiteDirectory"].ToString().Trim();
                string strSiteTemplateDirectory = ds.Tables[0].Rows[0]["SiteTemplateDirectory"].ToString().Trim();
                string strSiteVirtualDirectoryName = "/Doc";
                string strSiteVirtualDirectoryPhysicalPath = ds.Tables[0].Rows[0]["SiteVirtualDirectoryPhysicalPath"].ToString().Trim();
                string strSiteDBName = "TakeTopMPDB";
                string strSiteDBRestoreFile = ds.Tables[0].Rows[0]["SiteDBRestoreFile"].ToString().Trim();
                string strSiteDBSetupDirectory = ds.Tables[0].Rows[0]["SiteDBSetupDirectory"].ToString().Trim();
                string strSiteDBLoginUserID = ds.Tables[0].Rows[0]["SiteDBLoginUserID"].ToString().Trim();
                string strSiteDBUserLoginPassword = ShareClass.genernalPassword();

                if (UrlIsExist(strSiteAppURL))
                {
                    LB_Message.Text = LanguageHandle.GetWord("ChuangJianShiBaiCunZaiXiangTon");
                    return;
                }

                strSiteDirectory += "\\" + strSiteAppName + "\\Site";
                strSiteVirtualDirectoryPhysicalPath += "\\" + strSiteAppName + "\\Doc";
                strSiteDBSetupDirectory += "\\" + strSiteAppName + "\\DB";
                strSiteDBLoginUserID += strSiteAppName;
                strSiteDBName += strSiteAppName;

                strSiteDBLoginUserID = strSiteDBLoginUserID.ToLower();
                strSiteDBName = strSiteDBName.ToLower();
              

                //�洢վ����Ϣ
                strHQL = string.Format(@"Update T_RentSiteInfoByCustomer Set
                                RentUserPhoneNumber ='{0}'
                               ,RentUserEMail = '{1}'
                               ,RentUserName  = '{2}'
                               ,RentUserCompanyName = '{3}'
                               ,RentProductName = '{4}'
                               ,RentProductVersion = '{5}'
                               ,RentUserNumber = '{6}'
                               ,SiteAppSystemName = '{7}'
                               ,SiteAppName = '{8}'
                               ,SiteAppURL  = '{9}'
                               ,SiteName  = '{10}'
                               ,SiteURL  = '{11}'
                               ,SiteBindingInfo  = '{12}'
                               ,SiteDirectory = '{13}'
                               ,SiteTemplateDirectory = '{14}'
                               ,SiteVirtualDirectoryName  = '{15}'
                               ,SiteVirtualDirectoryPhysicalPath = '{16}'
                               ,SiteDBName = '{17}'
                               ,SiteDBRestoreFile = '{18}'
                               ,SiteDBSetupDirectory  = '{19}'
                               ,SiteDBLoginUserID  = '{20}'
                               ,SiteDBUserLoginPassword = '{21}'
                               ,SiteCreatorName  = '{22}'
                               ,SiteCreateTime = now()
                               ,SiteStatus  = '{23}'
                               ,IsOEM = '{24}'
                                 Where ID = {25}", strRentUserPhoneNumber, strRentUserEMail, strRentUserName, strRentUserCompanyName, strRentProductName, strRentProductVersion, strRentUserNumber, strSiteAppSystemName, strSiteAppName,
                                    strSiteAppURL, strSiteName, strSiteURL, strSiteBindingInfo, strSiteDirectory, strSiteTemplateDirectory, strSiteVirtualDirectoryName, strSiteVirtualDirectoryPhysicalPath,
                                    strSiteDBName, strSiteDBRestoreFile, strSiteDBSetupDirectory, strSiteDBLoginUserID, strSiteDBUserLoginPassword, strSiteCreatorName, "EXIST", strIsOEM, strSiteID);
                try
                {
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {
                }

                //������Ϣ���ͷ�����
                try
                {
                    string strCSOperatorCode = ShareClass.GetWebSiteCustomerServiceOperatorCode(strWebSite);
                    string strMSMMsg = strRentUserName + LanguageHandle.GetWord("DianHua") + strRentUserPhoneNumber + LanguageHandle.GetWord("YiChuangJian") + strRentProductName + "(" + strRentProductVersion + LanguageHandle.GetWord("ZuYongZhanDian") + strSiteAppURL;
                    Action action = new Action(delegate ()
                    {
                        Msg msg = new Msg();

                        try
                        {
                            msg.SendMSM("Message", strCSOperatorCode, strMSMMsg, "ADMIN");
                        }
                        catch (Exception ex)
                        {
                        }

                        string strEMailMsg = LanguageHandle.GetWord("NiHaoNi") + strServerType + LanguageHandle.GetWord("De") + strSiteAppSystemName + LanguageHandle.GetWord("YiChuangJianZhanDianDiZhiShi") + strSiteAppURL + LanguageHandle.GetWord("DengLuZhangHaoMiMaQingFangXinS");

                        try
                        {
                            msg.SendMailByEmail(strRentUserEMail, LanguageHandle.GetWord("ZhanDianChuangJianTongZhi"), strEMailMsg, "ADMIN");
                        }
                        catch
                        {
                        }
                    });
                    System.Threading.Tasks.Task.Factory.StartNew(action);
                }
                catch
                {
                }

                ClientScript.RegisterStartupScript(GetType(), "dd", "<script>displayRelatedUI();</script>");

                //��תҳ��strSiteURL + @"/" + strSiteCreatorAppName + @"/
                if (string.IsNullOrEmpty(strWebSite))
                {
                    strWebSite = "WWW.TAKETOPITS.COM";
                }

                string strCreateSiteURL = strSiteURL + @"/" + strSiteCreatorAppName + @"/TakeTopSoftRent_BuildSiteAuto.aspx?SiteAppSystemName=" + strSiteAppSystemName + "&SiteAppName=" + strSiteAppName + "&RentUserCompanyName=" + strRentUserCompanyName + "&RentUserName=" + strRentUserName + "&RentUserPhoneNumber=" + strRentUserPhoneNumber + "&RentUserEMail=" + strRentUserEMail + "&RentProductName=" + strRentProductName + "&RentProductVersion=" + strRentProductVersion + "&RentUserNumber=" + strRentUserNumber + "&SiteID=0&WebSite=" + strWebSite;
                strCreateSiteURL += "&SiteName=" + strSiteName + "&SiteURL=" + strSiteURL + "&SiteAppURL=" + strSiteAppURL + "&SiteBindingInfo=" + strSiteBindingInfo + "&SiteDirectory=" + strSiteDirectory + "&SiteTemplateDirectory=" + strSiteTemplateDirectory;
                strCreateSiteURL += "&SiteVirtualDirectoryName=" + strSiteVirtualDirectoryName + "&SiteVirtualDirectoryPhysicalPath=" + strSiteVirtualDirectoryPhysicalPath;
                strCreateSiteURL += "&SiteDBName=" + strSiteDBName + "&SiteDBRestoreFile=" + strSiteDBRestoreFile + "&SiteDBSetupDirectory=" + strSiteDBSetupDirectory + "&SiteDBLoginUserID=" + strSiteDBLoginUserID + "&SiteDBUserLoginPassword=" + strSiteDBUserLoginPassword + "&ServerType=" + strServerType + "&RentProductType=" + strRentProductType + "&IsOEM=" + strIsOEM;

                Response.Redirect(strCreateSiteURL);
            }
            catch (Exception ex)
            {
                LB_Message.Text = ex.Message.ToString();
            }
        }
    }

    //�ж�վ��Ӧ���Ƿ����
    private bool UrlIsExist(String url)
    {
        System.Uri u = null;
        try
        {
            u = new Uri(url);
        }
        catch { return false; }
        bool isExist = false;
        System.Net.HttpWebRequest r = System.Net.HttpWebRequest.Create(u) as System.Net.HttpWebRequest;
        r.Method = "HEAD";
        try
        {
            System.Net.HttpWebResponse s = r.GetResponse() as System.Net.HttpWebResponse;
            if (s.StatusCode == System.Net.HttpStatusCode.OK)
            {
                isExist = true;
            }
        }
        catch (System.Net.WebException x)
        {
            try
            {
                isExist = ((x.Response as System.Net.HttpWebResponse).StatusCode != System.Net.HttpStatusCode.NotFound);
            }
            catch { isExist = (x.Status == System.Net.WebExceptionStatus.Success); }
        }
        return isExist;
    }

    // �ж��ַ����Ƿ�ֻ������ĸ�����ܰ����ո�
    bool isAlphabeticNoSpace(string str)
    {
        Regex regex = new Regex("^[a-zA-Z]+$");
        return regex.IsMatch(str);
    }

    //��ȡ�ļ������������ɵ��ļ�
    public string GetLatestTimefile(string filePath)
    {
        DirectoryInfo info = new DirectoryInfo(filePath);
        FileInfo newestFile = info.GetFiles().OrderBy(n => n.LastWriteTime).First();

        return newestFile.FullName;
    }

    //ȡ�õ�ǰ���ݿ������ʵ������
    public string GetSqlServerName()
    {
        string strHQL;

        strHQL = "select @@SERVERNAME";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ServerTable");
        return ds.Tables[0].Rows[0][0].ToString();
    }
}
