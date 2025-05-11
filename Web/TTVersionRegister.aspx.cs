using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Web.UI;

using System.Xml;

using ProjectMgt.BLL;
using ProjectMgt.Model;

using TakeTopCore;

using TakeTopSecurity;
using System.Web;
using com.sun.source.tree;
using Npgsql;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class TTVersionRegister : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strRegisterCode, strCPUCode, strMacAddress, strEncryptCode;
        int intWEBLicenseNumber, intAPPLicenseNumber;
        //decimal deLicenseStorageNumber, deActualStorageNumber;
        string strDeadline;
        string strLicenseType, strVerType, strSiteName, strForbitModule;

        if (Page.IsPostBack != true)
        {
            try
            {
                //补齐系统启动所需要的数据表缺的字段
                string strHQL;

                strHQL = "Alter Table T_SystemActiveUser Add WebUser char(10) Default 'NO'";
                ShareClass.RunSqlCommand(strHQL);
                strHQL = "Update T_SystemActiveUser Set WebUser = 'YES'";
                ShareClass.RunSqlCommand(strHQL);
            }
            catch (Exception err)
            {
                //LogClass.WriteLogFile(err.Message.ToString());
            }

            string strIsOEMVersion = System.Configuration.ConfigurationManager.AppSettings["IsOEMVersion"];
            LB_Copyright.Text = LanguageHandle.GetWord("CopyrightTaiDingTaDingTakeTopS").ToString().Trim() + "<a href=https://www.taketopits.com>www.taketopits.com</a>";

            string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
            LB_ServerName.Text = strServerName;


            if (strIsOEMVersion == "NO")
            {
                LB_Copyright.Visible = true;
            }
            else
            {
                LB_Copyright.Visible = true;
                LB_Copyright.Text = "<a href=TTVersionRegister.aspx>Copyright? 2006-2026 all rights is reserved to the copyright owner</a>";
            }

            string strSystemType;
            strSystemType = ShareClass.GetSystemType().ToUpper();

            try
            {
                if (strSystemType.IndexOf("UNIX") < 0)
                {
                    TakeTopLicense license = new TakeTopLicense();

                    strCPUCode = license.GetCpuInfo();
                    strMacAddress = license.GetMacAddress();

                    if (strMacAddress == "")
                    {
                        strRegisterCode = strCPUCode;

                        LB_MySN.Text = LanguageHandle.GetWord("CuoWuCiJiQiBiXuLianHuLianWangC").ToString().Trim();
                        LB_MySN.ForeColor = Color.Red;
                    }
                    else
                    {
                        strRegisterCode = strCPUCode + "'" + strMacAddress;
                        strEncryptCode = license.Encrypt(strRegisterCode);

                        LB_MySN.Text = strEncryptCode;
                    }

                    intWEBLicenseNumber = license.GetWEBLicenseNumber(strServerName);
                    intAPPLicenseNumber = license.GetAPPLicenseNumber(strServerName);
                    strDeadline = license.GetLicenseDeadline(strServerName);
                    strDeadline = strDeadline.Substring(0, 4) + "-" + strDeadline.Substring(4, 2) + "-" + strDeadline.Substring(6, 2);
                    strLicenseType = license.GetLicenseType(strServerName);
                    strVerType = license.GetVerType(strServerName);
                    strSiteName = license.GetSiteName(strServerName);
                    strForbitModule = license.GetForbitModuleString(strServerName).TrimEnd(',');

                    LB_LicenseNumber.Text = ""+LanguageHandle.GetWord("ShouQuanLeiXing")+"：" + strLicenseType + "，"+LanguageHandle.GetWord("BanBen")+"：" + strVerType + "，"+LanguageHandle.GetWord("ZhanDianMing")+"：" + strSiteName + "，"+LanguageHandle.GetWord("ShouQuanYongHuShu")+"：WEB:" + intWEBLicenseNumber.ToString() + ", APP: " + intAPPLicenseNumber.ToString() + "，"+LanguageHandle.GetWord("YouXiaoQiZhi")+"：" + strDeadline + "，"+LanguageHandle.GetWord("JinYongMoKuai")+"：" + strForbitModule;   
                    LB_LicenseNumber.Text += LanguageHandle.GetWord("XianYouYongHuShu").ToString().Trim() + "WEB：" + GetCurrentWebUserNumber() + ", APP：" + GetCurrentAppUserNumber();
                }
                else
                {
                    LB_LicenseNumber.Text += LanguageHandle.GetWord("XianYouYongHuShu").ToString().Trim() + "WEB：" + GetCurrentWebUserNumber() + ", APP：" + GetCurrentAppUserNumber();
                }
            }
            catch (System.Exception err)
            {
                LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    protected void BT_Register_Click(object sender, EventArgs e)
    {
        string strRegisterCodeNew, strRegisterCode1, strRegisterCode2, strCPUCode, strMacAddress;
        string strDescryptString, strDescryptString1, strDescryptString2, strDeadline;
        int intWEBLicenseNumber, intAPPLicenseNumber;
        string strLicenseType, strVerType, strServerNameNew, strServerName, strSiteName, strForbitModule;

        string strHQL;
        IList lst;

        strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
        strServerNameNew = LB_ServerName.Text.Trim();
        strRegisterCodeNew = TB_RegisterCode.Text.Trim();

        if (strRegisterCodeNew == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCSBZCMBNWKJC").ToString().Trim() + "')", true);
            return;
        }
        else
        {
            TakeTopLicense license = new TakeTopLicense();

            strCPUCode = license.GetCpuInfo();
            strMacAddress = license.GetMacAddress();

            if (strMacAddress == "")
            {
                strRegisterCode1 = strCPUCode;
            }
            else
            {
                strRegisterCode1 = strCPUCode + "'" + strMacAddress;
            }

            try
            {
                strDescryptString = license.Decrypt(strRegisterCodeNew);

                if (strMacAddress == "")
                {
                    strRegisterCode2 = strDescryptString.Substring(0, strDescryptString.IndexOf("'"));
                }
                else
                {
                    strRegisterCode2 = strDescryptString.Substring(0, strDescryptString.IndexOf("-"));
                }

                if (strRegisterCode1 == strRegisterCode2)
                {
                    strDescryptString1 = strDescryptString.Substring(strDescryptString.IndexOf("-") + 1, strDescryptString.Length - strDescryptString.IndexOf("-") - 1);
                    strDescryptString2 = strDescryptString1.Substring(0, strDescryptString1.IndexOf("-"));

                    intWEBLicenseNumber = int.Parse(strDescryptString2);

                    strHQL = "From LicenseVerification as licenseVerification Where licenseVerification.ServerName = " + "'" + strServerName + "'";
                    LicenseVerificationBLL licenseVerificationBLL = new LicenseVerificationBLL();
                    lst = licenseVerificationBLL.GetAllLicenseVerifications(strHQL);

                    if (lst.Count > 0)
                    {
                        LicenseVerification licenseVerification = (LicenseVerification)lst[0];
                        licenseVerification.VerificationString = strRegisterCodeNew;

                        try
                        {
                            licenseVerificationBLL.UpdateLicenseVerification(licenseVerification, strServerName);

                            intWEBLicenseNumber = license.GetWEBLicenseNumber(strServerName);
                            intAPPLicenseNumber = license.GetAPPLicenseNumber(strServerName);
                            strDeadline = license.GetLicenseDeadline(strServerName);
                            strDeadline = strDeadline.Substring(0, 4) + "-" + strDeadline.Substring(4, 2) + "-" + strDeadline.Substring(6, 2);
                            strLicenseType = license.GetLicenseType(strServerName);
                            strVerType = license.GetVerType(strServerName);
                            strSiteName = license.GetSiteName(strServerName);
                            strForbitModule = license.GetForbitModuleString(strServerName);

                            LB_LicenseNumber.Text = ""+LanguageHandle.GetWord("ShouQuanLeiXing")+"：" + strLicenseType + "，"+LanguageHandle.GetWord("BanBen")+"：" + strVerType + "，"+LanguageHandle.GetWord("ZhanDianMing")+"：" + strSiteName + "，" + LanguageHandle.GetWord("YongHuShu") + " WEB:" + intWEBLicenseNumber.ToString() + ", APP: " + intAPPLicenseNumber.ToString() + "，"+LanguageHandle.GetWord("YouXiaoQiZhi")+"：" + strDeadline + "，"+LanguageHandle.GetWord("JinYongMoKuai")+"：" + strForbitModule;   
                            BT_Register.ToolTip = strRegisterCodeNew;

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCCG").ToString().Trim() + "')", true);
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCSBJC").ToString().Trim() + "')", true);
                        }
                    }
                    else
                    {
                        LicenseVerification licenseVerification = new LicenseVerification();
                        licenseVerification.ServerName = strServerNameNew;
                        licenseVerification.VerificationString = strRegisterCodeNew;

                        try
                        {
                            licenseVerificationBLL.AddLicenseVerification(licenseVerification);

                            intWEBLicenseNumber = license.GetWEBLicenseNumber(strServerName);
                            intAPPLicenseNumber = license.GetAPPLicenseNumber(strServerName);
                            strDeadline = license.GetLicenseDeadline(strServerName);
                            strDeadline = strDeadline.Substring(0, 4) + "-" + strDeadline.Substring(4, 2) + "-" + strDeadline.Substring(6, 2);
                            strLicenseType = license.GetLicenseType(strServerName);
                            strVerType = license.GetVerType(strServerName);
                            strSiteName = license.GetSiteName(strServerName);

                            //设置缓存更改标志
                            ChangePageCache();

                            LB_LicenseNumber.Text = ""+LanguageHandle.GetWord("ShouQuanLeiXing")+"：" + strLicenseType + "，"+LanguageHandle.GetWord("BanBen")+"：" + strVerType + "，"+LanguageHandle.GetWord("ZhanDianMing")+"：" + strSiteName + "，" +LanguageHandle.GetWord("YongHuShu") + "WEB:" + intWEBLicenseNumber.ToString() + ", APP: " + intAPPLicenseNumber.ToString() + "，"+LanguageHandle.GetWord("YouXiaoQiZhi")+"：" + strDeadline;   
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCCG").ToString().Trim() + "')", true);
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCSBZCMBNXRHTJCGXLJCS").ToString().Trim() + "')", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZCMBDJC").ToString().Trim() + "')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZCMBDJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void IMB_Logo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //如果存在升级语句，那么升级数据库
            if (TakeTopCore.CoreShareClass.UpgradeDataBase() == false)
            {

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJSJSBKNSSSAMPLEYHJC").ToString().Trim() + "')", true);
            }
            else
            {
                //设置缓存更改标志，并刷新页面缓存
                ChangePageCache();
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSJCG").ToString().Trim() + "')", true);

        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSJSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void IMB_Copyright_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            UpdateLeftBarModules();
            UpdatePageBarModules();

           
            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click11", "alert('" + LanguageHandle.GetWord("ZZTBMZYYSJCG").ToString().Trim() + "')", true);
        }

        catch (System.Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("SBKNWJGSBDHLCDBGQJC").ToString().Trim() + "')", true);
        }

        return;
    }

    //更新左边样模组
    protected void UpdateLeftBarModules()
    {
        string strHQL1;
        string strModuleName, strParentModule, strLangCode, strHomeModuleName, strPageName, strModuleType, strUserType, strVisible, strIsDeleted, strSortNUmber, strIconURL;

        DataSet ds1;
        DataTable dt1;

        dt1 = new DataTable();

        string strpath = Server.MapPath("UpdateCode\\Modules.xls");
        dt1 = MSExcelHandler.ReadExcelToDataTable(strpath, "");

        DataRow[] dr = dt1.Select();  //定义一个DataRow数组
        int rowsnum = dt1.Rows.Count;
        if (rowsnum == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
        }
        else
        {
            for (int i = 0; i < dr.Length; i++)
            {
                strModuleName = dr[i]["ModuleName"].ToString().Trim();
                strParentModule = dr[i]["ParentModule"].ToString().Trim();
                strLangCode = dr[i]["LangCode"].ToString().Trim();
                strHomeModuleName = dr[i]["HomeModuleName"].ToString().Trim();
                strPageName = dr[i]["PageName"].ToString().Trim();
                strModuleType = dr[i]["ModuleType"].ToString().Trim();
                strUserType = dr[i]["UserType"].ToString().Trim();
                strVisible = dr[i]["Visible"].ToString().Trim();
                strIsDeleted = dr[i]["IsDeleted"].ToString().Trim();
                strSortNUmber = dr[i]["SortNumber"].ToString().Trim();
                strIconURL = dr[i]["IconURL"].ToString().Trim();
                strIconURL = strIconURL.Replace("//", "/");
                strIconURL = strIconURL.Replace("\\", "/");

                strHQL1 = "Select * From T_ProModuleLevel Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and ModuleType = '" + strModuleType + "' and LangCode='" + strLangCode + "' and UserType = '" + strUserType + "'";
                ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_ProModuleLevel");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strHQL1 = "Update T_ProModuleLevel Set HomeModuleName = '" + strHomeModuleName.Replace("'", "").Replace("\"", "").Replace("\\", "") + "'" + ",IconURL = " + "'" + strIconURL + "'";
                    strHQL1 += " Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and LangCode='" + strLangCode + "' and ModuleType = '" + strModuleType + "' and UserType = '" + strUserType + "'";

                    ShareClass.RunSqlCommand(strHQL1);
                }
            }

            string strDefaultLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            strHQL1 = string.Format(@"UPDATE T_ProModuleLevel B
                        SET SortNumber = A.SortNumber
                        FROM T_ProModuleLevel A
                        WHERE A.ModuleName = B.ModuleName 
                        AND A.ModuleType = B.ModuleType 
                        AND A.UserType = B.UserType 
                        AND A.LangCode <> B.LangCode 
                        AND A.LangCode = '{0}'", strDefaultLangCode);
            ShareClass.RunSqlCommand(strHQL1);
        }
    }

    //更新页面模组
    protected void UpdatePageBarModules()
    {
        string strHQL1;
        string strModuleName, strParentModule, strLangCode, strHomeModuleName, strPageName, strModuleType, strUserType, strVisible, strIsDeleted, strSortNUmber, strIconURL;

        DataSet ds1;

        string strpath = Server.MapPath("UpdateCode\\PageModules.xls");

        DataTable dt1;
        dt1 = new DataTable();
        dt1 = MSExcelHandler.ReadExcelToDataTable(strpath, "");
        DataRow[] dr = dt1.Select();  //定义一个DataRow数组
        int rowsnum = dt1.Rows.Count;

        if (rowsnum == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
        }
        else
        {
            for (int i = 0; i < dr.Length; i++)
            {
                strModuleName = dr[i]["ModuleName"].ToString().Trim();
                strParentModule = dr[i]["ParentModule"].ToString().Trim();
                strLangCode = dr[i]["LangCode"].ToString().Trim();
                strHomeModuleName = dr[i]["HomeModuleName"].ToString().Trim();
                strPageName = dr[i]["PageName"].ToString().Trim();
                strModuleType = dr[i]["ModuleType"].ToString().Trim();
                strUserType = dr[i]["UserType"].ToString().Trim();
                strVisible = dr[i]["Visible"].ToString().Trim();
                strIsDeleted = dr[i]["IsDeleted"].ToString().Trim();
                strSortNUmber = dr[i]["SortNumber"].ToString().Trim();
                strIconURL = dr[i]["IconURL"].ToString().Trim();
                strIconURL = strIconURL.Replace("//", "/");
                strIconURL = strIconURL.Replace("\\", "/");


                strHQL1 = "Select * From T_ProModuleLevelForPage  Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and ModuleType = '" + strModuleType + "' and LangCode='" + strLangCode + "' and UserType = '" + strUserType + "'";
                ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_ProModuleLevel");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strHQL1 = "Update T_ProModuleLevelForPage Set HomeModuleName = '" + strHomeModuleName.Replace("'", "").Replace("\"", "").Replace("\\", "") + "'" + ",IconURL = " + "'" + strIconURL + "'";
                    strHQL1 += " Where ParentModule = '" + strParentModule + "' and ModuleName = '" + strModuleName + "' and LangCode='" + strLangCode + "' and ModuleType = '" + strModuleType + "' and UserType = " + "'" + strUserType + "'";

                    ShareClass.RunSqlCommand(strHQL1);
                }
            }

            string strDefaultLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            strHQL1 = string.Format(@"UPDATE T_ProModuleLevelForPage B
                        SET SortNumber = A.SortNumber
                        FROM T_ProModuleLevelForPage A
                        WHERE A.ModuleName = B.ModuleName 
                        AND A.ModuleType = B.ModuleType 
                        AND A.UserType = B.UserType 
                        AND A.LangCode <> B.LangCode 
                        AND A.LangCode = '{0}'", strDefaultLangCode);
            ShareClass.RunSqlCommand(strHQL1);
        }
    }

    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {
        ////设置缓存更改标志
        //ShareClass.SetPageCacheMark("1");
        //Session["CssDirectoryChangeNumber"] = "1";

        //设置缓存更改标志
        ShareClass.AddSpaceLineToFile("TakeTopLRTop.aspx", "");
    }

    protected string GetCurrentAppUserNumber()
    {
        string strHQL;

        strHQL = "Select * From T_SystemActiveUser Where AppUser = 'YES'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows.Count.ToString();
        }
        else
        {
            return "0";
        }
    }

    protected string GetCurrentWebUserNumber()
    {
        string strHQL;

        strHQL = "Select * From T_SystemActiveUser Where WebUser = 'YES'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemActiveUser");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows.Count.ToString();
        }
        else
        {
            return "0";
        }
    }

}

