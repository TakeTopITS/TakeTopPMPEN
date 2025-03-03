using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using ProjectMgt.BLL;
using ProjectMgt.Model;

using TakeTopSecurity;

public partial class _default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品(jack.erp@gmail.com)
        //泰顶拓鼎集团（TakeTop Software）2006－2026

        string strDevelopStatus = "NO";

        if (strDevelopStatus == "YES")
        {
            //应用会话对象，以保持在线连接
            Session["UserCode"] = "C7094";
            Session["UserName"] = "员工C7094";
            Session["IsMobileDevice"] = "NO";
            Session["IsMobileDevice"] = "NO";
            Session["SystemType"] = "WEB";
            Session["CssDirectory"] = "css/CssBlack";
            Session["SystemVersionType"] = "GROUP";
            Session["ForbitModule"] = "NONE";
            Session["MustInFrame"] = "YES";
            Session["AutoSaveWFOperator"] = "YES";
            Session["LangCode"] = "zh-CN";
            Session["UserType"] = "INNER";
            Session["LeftBarExtend"] = "NO";

            Response.Redirect("TTWorkPlanGanttForProject.aspx?pid=153");

            //Response.Redirect("Handler/SyncProjectBaselinePlanTime.ashx?pid=15320");
        }
        else
        {
            string strVerificationCode, strSMSVerification, strIsOEMVersion, strProductType;
            string strUserHostAddress = Request.UserHostAddress;

            this.Title = System.Configuration.ConfigurationManager.AppSettings["SystemName"] + " " + ShareClass.SystemVersionID + "---" + System.Configuration.ConfigurationManager.AppSettings["Slogan"];

            LB_SystemName.Text = System.Configuration.ConfigurationManager.AppSettings["SystemName"];

            if (Page.IsPostBack != true)
            {
                HL_UserID.Visible = true;
                HL_UserID.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["WebSiteValue"];
                HL_UserID.Text = System.Configuration.ConfigurationManager.AppSettings["WebSite"];
                LB_Slogon.Text = System.Configuration.ConfigurationManager.AppSettings["Slogon"];

                strVerificationCode = System.Configuration.ConfigurationManager.AppSettings["VerificationCode"].Trim().ToUpper();
                if (strVerificationCode == "NO")
                {
                    TB_CheckCode.Enabled = false;
                    TB_CheckCode.Text = "*********";
                    IM_CheckCode.Visible = false;
                }
                else
                {
                    strSMSVerification = System.Configuration.ConfigurationManager.AppSettings["SMSVerification"].Trim().ToUpper();
                    if (strSMSVerification == "YES")
                    {
                        IB_GetSMS.Visible = true;
                        IM_CheckCode.Visible = false;
                    }
                    else
                    {
                        if (ShareClass.IsMobileDeviceCheckAgent())
                        {
                            TB_CheckCode.Enabled = false;
                            TB_CheckCode.Text = "********";
                        }
                    }
                }

                LB_Copyright.Text = "<a href=TTVersionRegister.aspx>Copyright? TakeTopITS Group</a> 2006-2026 " + "<a href=https://www.taketopits.com>www.taketopits.com</a>";

                try
                {
                    strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
                    if (strProductType == "SAAS")
                    {
                        Response.Redirect("Logo/indexSAAS.html");
                    }
                    if (strProductType == "LOCALSAAS")
                    {
                        Response.Redirect("Logo/indexLocalSAAS.html");
                    }
                    if (strProductType == "SERVERSAAS")
                    {
                        Response.Redirect("Logo/indexXMB.html");
                    }


                    strIsOEMVersion = System.Configuration.ConfigurationManager.AppSettings["IsOEMVersion"];
                    if (strIsOEMVersion == "YES")
                    {
                        LB_Copyright.Visible = true;
                        LB_Copyright.Text = "<a href=TTVersionRegister.aspx>Copyright? 2006-2026 all rights is reserved to the copyright owner</a>";
                    }
                    else
                    {
                        LB_Copyright.Visible = true;
                    }

                    if (ShareClass.IsMobileDeviceCheckAgent())
                    {
                        Response.Redirect("~/DefaultMobile.aspx", false);
                    }
                }
                catch
                {
                }

                try
                {
                    ShareClass.LoadLanguageForDropList(ddlLangSwitcher);

                    if (Request.Cookies["LangCode"] != null)
                    {
                        ddlLangSwitcher.SelectedValue = Request.Cookies["LangCode"].Value;
                    }

                    if (Session["LangCode"] != null)
                    {
                        ddlLangSwitcher.SelectedValue = Session["LangCode"].ToString();
                    }

                    InitializeCulture();
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }

                //运行更新字段值代码
                DatabaseUpdateHandle.RunUpdateColumnValueCode();
            }
        }
    }

    protected void LB_Login_Click(object sender, EventArgs e)
    {
        string strUserCode, strUserName, strPassword;
        string strUserType, strMDIStyle, strMDIPageName, strMDIMobilePageName, strThirdPartPageNme, strThirdPartMobilePageName;
        string strVerificationCode, strSMSVerification;
        string strUserHostAddress, strAllowDevice;

        string strHQL;

        strUserHostAddress = Request.UserHostAddress;
        strUserCode = TB_UserCode.Text.Trim().ToUpper();
        strPassword = TB_Password.Text.Trim();
        if (strUserCode == "" | strPassword == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZYHMHMMDBNWKJC").ToString().Trim() + "');</script>");
            return;
        }

        if (ShareClass.SqlFilter(strUserCode) | ShareClass.SqlFilter(strPassword))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZHHYFFZHDLSB").ToString().Trim() + "');</script>");
            return;
        }

        //运行模组名称英文化代码
        DatabaseUpdateHandle.RunUpdateModuleNameCode();

        strSMSVerification = System.Configuration.ConfigurationManager.AppSettings["SMSVerification"].Trim().ToUpper();
        strVerificationCode = System.Configuration.ConfigurationManager.AppSettings["VerificationCode"].Trim().ToUpper();
        if (strVerificationCode == "YES")
        {
            if (!ShareClass.IsMobileDeviceCheckAgent())
            {
                if (Request.Cookies["CheckCode"] == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZNDLSZYBJYCOOKIESNBXSZLYXSYCOOKIESXHCNSYBXT").ToString().Trim() + "');</script>");
                    TB_CheckCode.Text = "";
                    return;
                }

                if (String.Compare(Request.Cookies["CheckCode"].Value, TB_CheckCode.Text, true) != 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZYZMCWSRZDYZM").ToString().Trim() + "');</script>");
                    TB_CheckCode.Text = "";
                    return;
                }
            }

            //if (strSMSVerification == "YES")
            //{
            //    strSMSCode = ShareClass.GetUserSMSCode(strUserCode);

            //    strCheckCode = TB_CheckCode.Text.Trim();

            //    if (strSMSCode == "")
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGYSJDXMYZXDDXM").ToString().Trim() + "');</script>");
            //        return;
            //    }
            //    else
            //    {
            //        if (strSMSCode != strCheckCode)
            //        {
            //            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGDXYZMCWJC").ToString().Trim() + "');</script>");
            //            return;
            //        }
            //    }
            //}
            //else
            //{
            //}
        }

        try
        {
            strPassword = EncryptPassword(strPassword, "MD5");
            strHQL = "Select * from T_ProjectMember where UserCode = " + "'" + strUserCode + "'" + " and Password = " + "'" + strPassword + "'" + " and " + " rtrim(ltrim(Status)) not in ( 'Stop','Resign')";
            strHQL += " And UserCode in (Select UserCode From T_SystemActiveUser Where WebUser = 'YES')";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //升级数据库
                if (ShareClass.SystemDBer == "")
                {
                    ShareClass.SystemDBer = strUserCode;
                    TakeTopCore.CoreShareClass.UpgradeDataBase();
                }

                strUserName = ds.Tables[0].Rows[0]["UserName"].ToString().Trim();
                strUserType = ds.Tables[0].Rows[0]["UserType"].ToString().Trim();
                strAllowDevice = ds.Tables[0].Rows[0]["AllowDevice"].ToString().Trim();

                strMDIStyle = ds.Tables[0].Rows[0]["MDIStyle"].ToString().Trim();
                DataSet dsMDIStyle = ShareClass.GetSystemMDIStyle(strMDIStyle);
                if (dsMDIStyle != null)
                {
                    strMDIPageName = dsMDIStyle.Tables[0].Rows[0]["PageName"].ToString().Trim();
                    strMDIMobilePageName = dsMDIStyle.Tables[0].Rows[0]["MobilePageName"].ToString().Trim();
                    strThirdPartPageNme = dsMDIStyle.Tables[0].Rows[0]["ThirdPartPageName"].ToString().Trim();
                    strThirdPartMobilePageName = dsMDIStyle.Tables[0].Rows[0]["ThirdPartMobilePageName"].ToString().Trim();
                }
                else
                {
                    strMDIPageName = "TakeTopLRExMDI.html";
                    strMDIMobilePageName = "TakeTopLRExMDI.html";
                    strThirdPartPageNme = "TakeTopCSMDI.html";
                    strThirdPartMobilePageName = "TakeTopCSMDI.html";
                }

                Session["UserCode"] = strUserCode;
                Session["UserName"] = strUserName;
                Session["UserType"] = strUserType;
                Session["IsMobileDevice"] = "NO";
                Session["SystemType"] = "WEB";

                try
                {
                    Session["LeftBarExtend"] = ds.Tables[0].Rows[0]["LeftBarExtend"].ToString().Trim();
                }
                catch
                {
                    Session["LeftBarExtend"] = "NO";
                }

                Session["CssDirectory"] = ds.Tables[0].Rows[0]["CssDirectory"].ToString().Trim();
                Session["CssDirectoryChangeNumber"] = ds.Tables[0].Rows[0]["CssDirectoryChangeNumber"].ToString().Trim();

                //HttpBrowserCapabilities GetBrowserCapabilities = HttpContext.Current.Request.Browser;
                //if (ShareClass.GetBrowser(GetBrowserCapabilities) == "SF")
                //{
                //    if (ds.Tables[0].Rows[0]["CssDirectory"].ToString().Trim() == "CssBlue")
                //    {
                //        Session["CssDirectory"] = "CssGreen";
                //        //Session["CssDirectoryChangeNumber"] = "1";

                //        string strHQL1 = "Update T_ProjectMember Set CssDirectory = 'CssGreen' where UserCode = " + "'" + strUserCode + "'";
                //        ShareClass.RunSqlCommand(strHQL1);

                //        //给相关页面文件添加空行以刷新页面缓存
                //        ShareClass.AddSpaceLineToFileForRefreshCache();
                //    }
                //}



                try
                {
                    try
                    {
                        Session["LangCode"] = ds.Tables[0].Rows[0]["LangCode"].ToString().Trim();
                    }
                    catch
                    {
                    }

                    if (Session["LangCode"] == null)
                    {
                        try
                        {
                            Session["LangCode"] = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
                        }
                        catch
                        {
                            Session["LangCode"] = "zh-CN";
                        }
                    }

                    Session["SkinFlag"] = ds.Tables[0].Rows[0]["CssDirectory"].ToString().Trim() + Session["LangCode"].ToString();

                    InitializeCulture();
                }
                catch
                {
                }

                //YES时页面必须在框架内打开，否则关闭
                try
                {
                    Session["MustInFrame"] = System.Configuration.ConfigurationManager.AppSettings["MustInFrame"];
                }
                catch
                {
                }
                if (Session["MustInFrame"] == null)
                {
                    Session["MustInFrame"] = "YES";
                }

                //是否自动工作流申请者自选或上一步审批者自选人员
                try
                {
                    Session["AutoSaveWFOperator"] = System.Configuration.ConfigurationManager.AppSettings["AutoSaveWFOperator"];
                }
                catch
                {
                }
                if (Session["AutoSaveWFOperator"] == null)
                {
                    Session["AutoSaveWFOperator"] = "YES";
                }

                //---判断用户能用来登录的设备类型----------------------
                if (strAllowDevice != "ALL")
                {
                    if (ShareClass.IsMobileDeviceCheckAgent())
                    {
                        if (strAllowDevice == "PC")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGNBNYYDSBDLPTJC").ToString().Trim() + "');</script>");
                            return;
                        }
                    }
                    else
                    {
                        if (strAllowDevice == "MOBILE")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGNZNYYDSBDLPTJC").ToString().Trim() + "');</script>");
                            return;
                        }
                    }
                }


                //检查注册码是否合法
                string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                try
                {
                    TakeTopLicense license = new TakeTopLicense();
                    if (!license.CheckWEBLicense(strServerName))
                    {
                        Response.Redirect("TTDisplayErrors.aspx");
                        return;
                    }

                    Session["SystemVersionType"] = license.GetVerType(strServerName);
                    Session["ForbitModule"] = license.GetForbitModuleString(strServerName);
                }
                catch
                {

                    Session["SystemVersionType"] = "GROUP";
                    Session["ForbitModule"] = "NONE";
                }
                if (System.Configuration.ConfigurationManager.AppSettings["ProductType"].IndexOf("SAAS") > -1)
                {
                    Session["SystemVersionType"] = "SAAS";
                }



                try
                {
                    if (Session["CssDirectoryChangeNumber"].ToString() != "2" & Session["CssDirectoryChangeNumber"].ToString() != "0")
                    {
                        //设置缓存更改标志
                        ShareClass.SetPageCacheMark("2");
                    }

                    //插入登录日志
                    ShareClass.InsertUserLogonLog(strUserCode, strUserName, "WEB");
                }
                catch
                {
                }

                if (strUserType != "OUTER")
                {
                    Session["IsMobileDevice"] = "NO";
                    Response.Redirect(strMDIPageName, false);
                }
                else
                {
                    Session["IsMobileDevice"] = "NO";
                    Response.Redirect(strThirdPartPageNme, false);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGYHDMHMMCWHYBZZSY").ToString().Trim() + "');</script>");
            }
        }
        catch (Exception err)
        {
            Response.Redirect("TTDisplayErrors.aspx");
        }
    }

    protected void LB_WeChatQRCode_Click(object sender, EventArgs e)
    {
        logoImg.Src = "Logo/TakeTopXMB.png";
    }

    protected string GetModulePageName(string strModuleName)
    {
        string strHQL;

        strHQL = "select PageName from T_ProModuleLevel  where ModuleName = " + "'" + strModuleName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    protected void LB_Register_Click(object sender, EventArgs e)
    {
        Response.Redirect("TTDisplayErrors.aspx");
    }

    protected void IB_GetSMS_Click(object sender, ImageClickEventArgs e)
    {
        string strUserCode, strPassword, strSMSCode, strMsg;
        int intCount;

        strUserCode = TB_UserCode.Text.Trim();
        strPassword = TB_Password.Text.Trim();

        strPassword = EncryptPassword(strPassword, "MD5");

        intCount = GetUserCount(strUserCode, strPassword);

        Msg msg = new Msg();

        if (intCount > 0)
        {
            strSMSCode = msg.CreateRandomCode(5);

            strMsg =  LanguageHandle.GetWord("DuanXinYanZhengMa") +"：" + strSMSCode + "," + LanguageHandle.GetWord("DangTianYouXiao");

            if (msg.SendMSM("Message", strUserCode, strMsg, strUserCode))
            {
                InsertOrUpdateSMSCode(strUserCode, strSMSCode);

                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZDXYZMYFSCS").ToString().Trim() + "');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGDXYZMFSSBJCDXJKHWLLJ").ToString().Trim() + "');</script>");
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGYHDMHMMCWBNDDXMJC").ToString().Trim() + "');</script>");
        }
    }

    protected void ddlLangSwitcher_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLangSwitcher.SelectedValue != "")
        {
            Session["LangCode"] = ddlLangSwitcher.SelectedValue;
        }
        else
        {
            Session["LangCode"] = null;
        }

        InitializeCulture();

        Response.Redirect("Default.aspx");
    }

    protected override void InitializeCulture()
    {
        string strLangCode;

        if (Session["LangCode"] == null)
        {
            strLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            Session["LangCode"] = strLangCode;
        }
        else
        {
            strLangCode = Session["LangCode"].ToString();
        }

        try
        {
            Response.SetCookie(new HttpCookie("LangCode", strLangCode));
        }
        catch
        {
        }

        if (Request.Cookies["LangCode"] != null)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(Request.Cookies["LangCode"].Value.ToString());
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Request.Cookies["LangCode"].Value.ToString());

            Page.Culture = Request.Cookies["LangCode"].Value;
            Page.UICulture = Request.Cookies["LangCode"].Value;

            base.InitializeCulture();
        }
    }

    protected void InsertOrUpdateSMSCode(string strUserCode, string strSMSCode)
    {
        string strHQL;
        IList lst;

        int intID;

        strHQL = "From SMSCode as smsCode Where smsCode.UserCode = " + "'" + strUserCode + "'" + " and to_char(smsCode.SendTime,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'";
        SMSCodeBLL smsCodeBLL = new SMSCodeBLL();
        lst = smsCodeBLL.GetAllSMSCodes(strHQL);

        SMSCode smsCode = new SMSCode();

        if (lst.Count > 0)
        {
            smsCode = (SMSCode)lst[0];

            intID = smsCode.ID;
            smsCode.UserCode = strUserCode;
            smsCode.RandomCode = strSMSCode;
            smsCode.SendTime = DateTime.Now;

            try
            {
                smsCodeBLL.UpdateSMSCode(smsCode, intID);
            }
            catch
            {
            }
        }
        else
        {
            smsCode.UserCode = strUserCode;
            smsCode.RandomCode = strSMSCode;
            smsCode.SendTime = DateTime.Now;

            try
            {
                smsCodeBLL.AddSMSCode(smsCode);
            }
            catch
            {
            }
        }
    }

    protected string EncryptPassword(string strPassword, string strFormat)
    {
        string strNewPassword;

        if (strFormat == "SHA1")
        {
            strNewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "SHA1");
        }
        else
        {
            strNewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "MD5");
        }

        return strNewPassword;
    }

    protected int GetUserCount(string strUserCode, string strPassword)
    {
        string strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'" + " and projectMember.Password = " + "'" + strPassword + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        return lst.Count;
    }

    protected int GetNetSegmentCount(string strHostIPaddress)
    {
        string strHQL;
        IList lst;

        string strIPAddress, strBeginIPAddress, strEndIPAddress;
        string strNewIPAddress;

        if (strHostIPaddress.IndexOf(".") >= 0)
        {
            strNewIPAddress = strHostIPaddress.Substring(0, strHostIPaddress.LastIndexOf("."));

            strIPAddress = strNewIPAddress + "%";
            strBeginIPAddress = strNewIPAddress + ".0";
            strEndIPAddress = strNewIPAddress + ".255";

            strHQL = "From SMSNetSegment as smsNetSegment where smsNetSegment.BeginSegment >=" + "'" + strBeginIPAddress + "'" + " and smsNetSegment.EndSegment <= " + "'" + strEndIPAddress + "'";
            strHQL += " and smsNetSegment.BeginSegment Like " + "'" + strIPAddress + "'" + " and smsNetSegment.EndSegment Like " + "'" + strIPAddress + "'";
            SMSNetSegmentBLL smsNetSegmentBLL = new SMSNetSegmentBLL();
            lst = smsNetSegmentBLL.GetAllSMSNetSegments(strHQL);

            if (lst.Count > 0)
            {
                return lst.Count;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
}
