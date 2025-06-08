///第三方dll
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

using log4net;
using log4net.Config;

using Microsoft.Web.Administration;

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Internal;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Xml;

public partial class TakeTopSoftRent_SiteInfoByCustomerAPP : System.Web.UI.Page
{
    string strLangCode;
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", Resources.lang.ZuYongZhanDianWeiHu, strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadRentSiteInfoByCustomer();

            LoadRentProductType();
            LoadRentProductVertype();
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strRentUserPhoneNumber, strProduct, strVersionType;

        strRentUserPhoneNumber = "%" + TB_RentUserPhoneNumber.Text.Trim() + "%";
        strProduct = "%" + DL_Product.SelectedValue.Trim() + "%";
        strVersionType = "%" + DL_VersionType.SelectedValue.Trim() + "%";

        strHQL = string.Format(@"Select * From T_RentSiteInfoByCustomer 
            Where RentUserPhoneNumber like '{0}' and RentProductName like '{1}'
            and RentProductVersion like '{2}' Order By ID DESC", strRentUserPhoneNumber, strProduct, strVersionType);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteInfoByCustomer");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_MoreThanCapacity_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strRentUserPhoneNumber, strProduct, strVersionType;

        strRentUserPhoneNumber = "%" + TB_RentUserPhoneNumber.Text.Trim() + "%";
        strProduct = "%" + DL_Product.SelectedValue.Trim() + "%";
        strVersionType = "%" + DL_VersionType.SelectedValue.Trim() + "%";

        strHQL = string.Format(@"Select * From T_RentSiteInfoByCustomer 
            Where RentUserPhoneNumber like '{0}' and RentProductName like '{1}'
            and RentProductVersion like '{2}' and CurrentCapacity > BuyCapacity Order By ID DESC", strRentUserPhoneNumber, strProduct, strVersionType);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteInfoByCustomer");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_GetAllCapacity_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strSiteDirectory, strSiteAppURL;

        try
        {
            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                strID = DataGrid1.Items[i].Cells[6].Text.Trim();

                strHQL = "Select * From T_RentSiteInfoByCustomer Where ID = " + strID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteInfoByCustomer");

                strSiteDirectory = ds.Tables[0].Rows[0]["SiteDirectory"].ToString().Trim();
                strSiteAppURL = ds.Tables[0].Rows[0]["SiteAppURL"].ToString().Trim();

                try
                {
                    string strDirectory = "?DirPath=" + strSiteDirectory.Replace(@"\Site", "");
                    string strFolderSize = ((decimal.Parse(ShareClass.GetPostDataPage(strSiteAppURL + @"/Handler/GetFoldSize.ashx" + strDirectory, "")) / 1024) / 1024 / 1024).ToString("f2");

                    strHQL = "Update T_RentSiteInfoByCustomer Set CurrentCapacity = " + strFolderSize + " Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);

                    ((TextBox)(DataGrid1.Items[i].FindControl("TB_CurrentCapacity"))).Text = strFolderSize;
                }
                catch (Exception ex)
                {
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.YiGengXinXianYouRongLiang, true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
        }
    }

    protected void BT_SortCapacity_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "Select * From T_RentSiteInfoByCustomer Order By CurrentCapacity DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteInfoByCustomer");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }
    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            string strID = e.Item.Cells[7].Text.Trim();

            string strHQL;
            strHQL = "Select * From T_RentSiteInfoByCustomer Where ID = " + strID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteInfoByCustomer");

            string strRentUserCompanyName = ds.Tables[0].Rows[0]["RentUserCompanyName"].ToString().Trim();
            string strRentUserName = ds.Tables[0].Rows[0]["RentUserName"].ToString().Trim();
            string strRentUserPhoneNumber = ds.Tables[0].Rows[0]["RentUserPhoneNumber"].ToString().Trim();
            string strRentUserEMail = ds.Tables[0].Rows[0]["RentUserEMail"].ToString().Trim();
            string strRentProductName = ds.Tables[0].Rows[0]["RentProductName"].ToString().Trim();
            string strRentProductVersion = ds.Tables[0].Rows[0]["RentProductVersion"].ToString().Trim();
            string strRentUserNumber = ds.Tables[0].Rows[0]["RentUserNumber"].ToString().Trim();

            string strSiteAppName = ds.Tables[0].Rows[0]["SiteAppName"].ToString().Trim();
            string strSiteName = ds.Tables[0].Rows[0]["SiteName"].ToString().Trim();
            string strSiteDBName = ds.Tables[0].Rows[0]["SiteDBName"].ToString().Trim();
            string strSiteVirtualDirectoryPhysicalPath = ds.Tables[0].Rows[0]["SiteVirtualDirectoryPhysicalPath"].ToString().Trim();
            string strSiteDBLoginUserID = ds.Tables[0].Rows[0]["SiteDBLoginUserID"].ToString().Trim();

            string strSiteDirectory = ds.Tables[0].Rows[0]["SiteDirectory"].ToString().Trim();
            string strDBLoginUserID = ds.Tables[0].Rows[0]["SiteDBLoginUserID"].ToString().Trim();
            string strDBUserLoginPassword = ds.Tables[0].Rows[0]["SiteDBUserLoginPassword"].ToString().Trim();
            string strSiteAppSystemName = ds.Tables[0].Rows[0]["SiteAppSystemName"].ToString().Trim();
            string strSiteTemplateDirectory = ds.Tables[0].Rows[0]["SiteTemplateDirectory"].ToString().Trim();
            string strSiteAppURL = ds.Tables[0].Rows[0]["SiteAppURL"].ToString().Trim();
            string strSiteAppStatus = ds.Tables[0].Rows[0]["SiteStatus"].ToString().Trim();
            string strSiteURL = ds.Tables[0].Rows[0]["SiteURL"].ToString().Trim();
            string strServerType = ds.Tables[0].Rows[0]["ServerType"].ToString().Trim();
            string strSiteCreatorAppName = GetSiteCreatorAppName(strRentProductName, strRentProductVersion);
            string strIsOEM = ds.Tables[0].Rows[0]["IsOEM"].ToString().Trim();

            strSiteDBLoginUserID = strSiteDBLoginUserID.ToLower();
            strSiteDBName = strSiteDBName.ToLower();

            if (e.CommandName == "BuildSite")
            {
                try
                {
                    LB_SiteMsg.Text = "";
                    IFrame_Site.Src = "TakeTopSoftRent_BuildSite.aspx?RentUserCompanyName=" + strRentUserCompanyName + "&RentUserName=" + strRentUserName + "&RentUserPhoneNumber=" + strRentUserPhoneNumber + "&RentUserEMail=" + strRentUserEMail + "&RentProductName=" + strRentProductName + "&RentProductVersion=" + strRentProductVersion + "&RentUserNumber=" + strRentUserNumber + "&SiteID=" + strID + "&SiteAppSystemName=" + strSiteAppSystemName + "&SiteAppName=" + strSiteAppName + "&ServerType=" + strServerType;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSite','true') ", true);
                }
                catch
                {
                }
            }

            if (e.CommandName == "UpdateSite")
            {
                try
                {
                    LB_SiteMsg.Text = Resources.lang.ZhengZaiShengJiZheGeZhanDianDa;
                    IFrame_Site.Src = strSiteURL + @"/" + strSiteCreatorAppName + @"/TakeTopSoftRent_UpdateSiteAuto.aspx?SiteName=" + strSiteName + "&SiteAppName=" + strSiteAppName + "&SiteDirectory=" + strSiteDirectory + "&SiteTemplateDirectory=" + strSiteTemplateDirectory + "&DBLoginUserID=" + strDBLoginUserID + "&DBUserLoginPassword=" + strDBUserLoginPassword + "&SiteDBName=" + strSiteDBName + "&SiteAppSystemName=" + strSiteAppSystemName + "&SiteAppURL=" + strSiteAppURL + "&RentProductName=" + strRentProductName + "&RentUserEMail=" + strRentUserEMail + "&ServerType=" + strServerType + "&RentProductVersion=" + strRentProductVersion + "&SiteVirtualDirectoryPhysicalPath=" + strSiteVirtualDirectoryPhysicalPath + "&IsOEM=" + strIsOEM + "&SiteID=" + strID; ;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSite','true') ", true);

                    //发送邮件给用户
                    try
                    {
                        Action action = new Action(delegate ()
                        {
                            Msg msg = new Msg();
                            string strEMailMsg = Resources.lang.NiHaoNi + strServerType + Resources.lang.De + strSiteAppSystemName + Resources.lang.ZhanDian + strSiteAppURL + Resources.lang.YiBeiShengJiDaoZuiXinBanShengJ;

                            try
                            {
                                msg.SendMailByEmail(strRentUserEMail, Resources.lang.ZhanDianShengJiTongZhi, strEMailMsg, "ADMIN");
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                        System.Threading.Tasks.Task.Factory.StartNew(action);
                    }
                    catch
                    {
                    }
                }
                catch (Exception err)
                {
                    LB_ErrorMsg.Text = err.Message.ToString();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.ZhanDianShengJiShiBaiQingJianC, true);
                }
            }

            if (e.CommandName == "SendSiteBackupDoc")
            {
                try
                {
                    //压缩并生成备份文件
                    string strZipDocName = strSiteAppName + "Doc" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".zip";
                    string strDownloadDocURL = strSiteAppURL + @"/BackupDoc/" + strZipDocName;

                    LB_SiteMsg.Text = Resources.lang.ZhengZaiFaSongZheGeZhanDianBei;
                    IFrame_Site.Src = strSiteURL + @"/" + strSiteCreatorAppName + @"/TakeTopSoftRent_SendSiteBackupDocAuto.aspx?SiteName=" + strSiteName + "&SiteAppName=" + strSiteAppName + "&SiteDirectory=" + strSiteDirectory + "&SiteVirtualDirectoryPhysicalPath=" + strSiteVirtualDirectoryPhysicalPath + "&SiteTemplateDirectory=" + strSiteTemplateDirectory + "&DBLoginUserID=" + strDBLoginUserID + "&DBUserLoginPassword=" + strDBUserLoginPassword + "&SiteDBName=" + strSiteDBName + "&SiteAppSystemName=" + strSiteAppSystemName + "&SiteAppURL=" + strSiteAppURL + "&RentProductName=" + strRentProductName + "&RentUserEMail=" + strRentUserEMail + "&ZipDocName=" + strZipDocName + "&ServerType=" + strServerType;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSite','true') ", true);

                    //发送邮件给用户
                    try
                    {
                        Action action = new Action(delegate ()
                        {
                            Msg msg = new Msg();
                            string strEMailMsg = Resources.lang.NiHaoNi + strServerType + Resources.lang.De + strSiteAppSystemName + Resources.lang.ZhanDian + strSiteAppURL + Resources.lang.DeShuJuBeiFenWenJianXiaZaiDiZh + strDownloadDocURL + Resources.lang.QingJiShiXiaZaiRuYouWenTiQingL;

                            try
                            {
                                msg.SendMailByEmail(strRentUserEMail, Resources.lang.BeiFenWenJianXiaZaiTongZhi, strEMailMsg, "ADMIN");
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                        System.Threading.Tasks.Task.Factory.StartNew(action);
                    }
                    catch
                    {
                    }
                }
                catch (Exception err)
                {
                    LB_ErrorMsg.Text = err.Message.ToString();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.TiShiFaSongShiBaiQingJianCha, true);
                }
            }

            if (e.CommandName == "DeleteSite")
            {
                try
                {
                    LB_SiteMsg.Text = Resources.lang.ZhengZaiShanChuZheGeZhanDianDa;

                    IFrame_Site.Src = strSiteURL + @"/" + strSiteCreatorAppName + @"/TakeTopSoftRent_DeleteSiteAuto.aspx?SiteName=" + strSiteName + "&SiteAppName=" + strSiteAppName + "&SiteDirectory=" + strSiteDirectory + "&SiteVirtualDirectoryPhysicalPath=" + strSiteVirtualDirectoryPhysicalPath + "&SiteTemplateDirectory=" + strSiteTemplateDirectory + "&DBLoginUserID=" + strDBLoginUserID + "&DBUserLoginPassword=" + strDBUserLoginPassword + "&SiteDBName=" + strSiteDBName + "&SiteAppSystemName=" + strSiteAppSystemName + "&SiteAppURL=" + strSiteAppURL + "&RentProductName=" + strRentProductName + "&RentUserEMail=" + strRentUserEMail + "&ServerType=" + strServerType;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSite','true') ", true);

                    UpdateSiteStatus(strID, "DELETED");

                    LoadRentSiteInfoByCustomer();

                    //发送邮件给用户
                    try
                    {
                        Action action = new Action(delegate ()
                        {
                            Msg msg = new Msg();
                            string strEMailMsg = Resources.lang.NiHaoNi + strServerType + Resources.lang.De + strSiteAppSystemName + Resources.lang.ZhanDian + strSiteAppURL + Resources.lang.YiBeiShanChuRuYouWenTiQingLian;

                            try
                            {
                                msg.SendMailByEmail(strRentUserEMail, Resources.lang.ZhanDianShanChuTongZhi, strEMailMsg, "ADMIN");
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                        System.Threading.Tasks.Task.Factory.StartNew(action);
                    }
                    catch
                    {
                    }
                }
                catch (Exception err)
                {
                    LB_ErrorMsg.Text = err.Message.ToString();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
                }
            }

            if (e.CommandName == "RecoverSite")
            {
                string strSiteDBUserLoginPassword = ds.Tables[0].Rows[0]["SiteDBUserLoginPassword"].ToString().Trim();

                try
                {
                    LB_SiteMsg.Text = "";
                    IFrame_Site.Src = "TakeTopSoftRent_RecoverSite.aspx?RentUserCompanyName=" + strRentUserCompanyName + "&RentUserName=" + strRentUserName + "&RentUserPhoneNumber=" + strRentUserPhoneNumber + "&RentUserEMail=" + strRentUserEMail + "&RentProductName=" + strRentProductName + "&RentProductVersion=" + strRentProductVersion + "&RentUserNumber=" + strRentUserNumber + "&SiteID=" + strID + "&SiteAppSystemName=" + strSiteAppSystemName + "&SiteAppName=" + strSiteAppName + "&ServerType=" + strServerType + "&SiteDBUserLoginPassword=" + strSiteDBUserLoginPassword + "&SiteVirtualDirectoryPhysicalPath=" + strSiteVirtualDirectoryPhysicalPath + "&IsOEM=" + strIsOEM;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSite','true') ", true);
                }
                catch
                {
                }
            }

            if (e.CommandName == "GetFoldSize")
            {
                if (UrlIsExist(strSiteAppURL))
                {
                    try
                    {
                        string strDirectory = "?DirPath=" + strSiteDirectory.Replace(@"\Site", "");
                        string strFolderSize = ((decimal.Parse(ShareClass.GetPostDataPage(strSiteAppURL + @"/Handler/GetFoldSize.ashx" + strDirectory, "")) / 1024) / 1024 / 1024).ToString("f2");

                        strHQL = "Update T_RentSiteInfoByCustomer Set CurrentCapacity = " + strFolderSize + " Where ID = " + strID;
                        ShareClass.RunSqlCommand(strHQL);

                        ((TextBox)e.Item.FindControl("TB_CurrentCapacity")).Text = strFolderSize;

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.YiGengXinXianYouRongLiang, true);

                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.TiShiCiZhanDianBuCunZaiQingJia, true);
                }
            }

            if (e.CommandName == "SaveCapacity")
            {
                if (UrlIsExist(strSiteAppURL))
                {
                    try
                    {
                        string strCurrentCapacity = ((TextBox)e.Item.FindControl("TB_BuyCapacity")).Text.Trim();
                        strHQL = "Update T_RentSiteInfoByCustomer Set BuyCapacity = " + strCurrentCapacity + " Where ID = " + strID;
                        ShareClass.RunSqlCommand(strHQL);

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.BaoCunChengGong, true);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.TiShiCiZhanDianBuCunZaiQingJia, true);
                }
            }

            if (e.CommandName == "UpdateVirtualDirectoryPhysicalPath")
            {
                if (UrlIsExist(strSiteAppURL))
                {
                    LB_SiteMsg.Text = Resources.lang.ZhengZaiBaoCunZheGeZhanDianDeX;
                    strSiteVirtualDirectoryPhysicalPath = ((TextBox)e.Item.FindControl("TB_SiteVirtualDirectoryPhysicalPath")).Text.Trim();

                    strHQL = "Update T_RentSiteInfoByCustomer Set SiteVirtualDirectoryPhysicalPath = '" + strSiteVirtualDirectoryPhysicalPath + "' Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);


                    IFrame_Site.Src = strSiteURL + @"/" + strSiteCreatorAppName + @"/TakeTopSoftRent_UpdateSiteVirtualDirectoryPhysicalPath.aspx?SiteName=" + strSiteName + "&SiteAppName=" + strSiteAppName + "&SiteDirectory=" + strSiteDirectory + "&SiteVirtualDirectoryPhysicalPath=" + strSiteVirtualDirectoryPhysicalPath + "&SiteTemplateDirectory=" + strSiteTemplateDirectory + "&DBLoginUserID=" + strDBLoginUserID + "&DBUserLoginPassword=" + strDBUserLoginPassword + "&SiteDBName=" + strSiteDBName + "&SiteAppSystemName=" + strSiteAppSystemName + "&SiteAppURL=" + strSiteAppURL + "&RentProductName=" + strRentProductName + "&RentUserEMail=" + strRentUserEMail + "&ServerType=" + strServerType + "&SiteID=" + strID;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowSite','true') ", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.TiShiCiZhanDianBuCunZaiQingJia, true);
                }
            }

            if (e.CommandName == "Delete")
            {
                if (!UrlIsExist(strSiteAppURL))
                {
                    try
                    {
                        strHQL = "Delete From T_RentSiteInfoByCustomer Where ID = " + strID;
                        ShareClass.RunSqlCommand(strHQL);

                        LoadRentSiteInfoByCustomer();
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", Resources.lang.TiShiCiZhanDianHaiCunZaiJiLuBu, true);
                }
            }
        }
    }


    //取得创建站点的站点应用名
    protected string GetSiteCreatorAppName(string strRentProductName, string strRentProductVersion)
    {
        string strHQL;
        strHQL = "Select * From T_RentSiteBaseData Where RentProductName = '" + strRentProductName + "' and RentProductVersion = '" + strRentProductVersion + "' and IsCanUse = 'YES'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteBaseData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["SiteCreatorAppName"].ToString().Trim();
        }
        else
        {
            return "";
        }
    }


    //直接删除指定目录下的所有文件及文件夹
    public static void DeleteDirectory(string strDirectory)
    {
        try
        {
            //去除文件夹和子文件的只读属性
            //去除文件夹的只读属性
            System.IO.DirectoryInfo fileInfo = new DirectoryInfo(strDirectory);
            fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

            //去除文件的只读属性
            System.IO.File.SetAttributes(strDirectory, System.IO.FileAttributes.Normal);

            //判断文件夹是否还存在
            if (Directory.Exists(strDirectory))
            {
                foreach (string f in Directory.GetFileSystemEntries(strDirectory))
                {
                    if (File.Exists(f))
                    {
                        try
                        {
                            //如果有子文件删除文件
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
                                //循环递归删除子文件夹
                                DeleteDirectory(f);
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                //删除空文件夹
                Directory.Delete(strDirectory);
            }
        }
        catch (Exception ex) // 异常处理
        {
        }
    }

    protected void UpdateSiteStatus(string strSiteID, string strSiteStatus)
    {
        string strHQL;

        strHQL = "Update T_RentSiteInfoByCustomer Set SiteStatus = '" + strSiteStatus + "' Where ID = " + strSiteID;
        ShareClass.RunSqlCommand(strHQL);
    }

    protected void LoadRentSiteInfoByCustomer()
    {
        string strHQL;

        strHQL = "Select * From T_RentSiteInfoByCustomer  Order By SiteCreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentSiteInfoByCustomer");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadRentProductType()
    {
        string strHQL;

        strHQL = "Select * From T_RentProductType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentProductType");

        DL_Product.DataSource = ds;
        DL_Product.DataBind();
    }

    protected void LoadRentProductVertype()
    {
        string strHQL;

        strHQL = "Select * From T_RentProductVertype Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "RentProductVertype");

        DL_VersionType.DataSource = ds;
        DL_VersionType.DataBind();
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

            else // 否则直接压缩文件
            {
                //打开压缩文件
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

    //判断站点应用是否存在
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

}

