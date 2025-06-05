using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Threading;
using System.Globalization;

using System.IO;
using System.Net;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;
using TakeTopCore;

public partial class DefaultDemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ(jack.erp@gmail.com)
        //̩���ض����ţ�TakeTop Software��2006��2026\

        string strVerificationCode, strSMSVerification;
        string strUserCode, strPassword;
        string strUserHostAddress, strSystemName;

        if (ShareClass.IsMobileDeviceCheckAgent())
        {
            Response.Redirect("DefaultMobileDemo.aspx");
        }

        strUserCode = Request.QueryString["UserCode"];
        strPassword = Request.QueryString["Password"];

        if (strUserCode == null)
        {
            strUserCode = "C7094";
        }

        if (strPassword == null)
        {
            strPassword = "12345678@";
        }

        TB_UserCode.Text = strUserCode;
        TB_Password.Text = strPassword;

        TB_PasswordBackup.Text = "********";

        strUserHostAddress = Request.UserHostAddress;

        strSystemName = System.Configuration.ConfigurationManager.AppSettings["SystemName"];
        this.Title = strSystemName + " " + ShareClass.SystemVersionID + "---" + System.Configuration.ConfigurationManager.AppSettings["Slogan"];

        LB_SystemName.Text = System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        if (Page.IsPostBack != true)
        {
            HL_UserID.Visible = true;
            HL_UserID.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["WebSiteValue"];
            HL_UserID.Text = System.Configuration.ConfigurationManager.AppSettings["WebSite"];

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

            string strIsOEMVersion = System.Configuration.ConfigurationManager.AppSettings["IsOEMVersion"];
            if (strIsOEMVersion == "YES")
            {
                LB_Copyright.Visible = true;
                LB_Copyright.Text = "<a href=TTVersionRegister.aspx>Copyright? 2006-2026 all rights is reserved to the copyright owner</a>";
            }
            else
            {
                LB_Copyright.Visible = true;
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
            catch (Exception ex)
            {
            }

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>clickLoginButton();</script>");
        }
    }

    protected void LB_Login_Click(object sender, EventArgs e)
    {
        string strUserCode, strUserName, strPassword;
        string strUserType, strMDIStyle, strMDIPageName, strMDIMobilePageName, strThirdPartPageNme, strThirdPartMobilePageName;
        string strVerificationCode, strSMSVerification, strSMSCode, strCheckCode;
        string strUserHostAddress, strAllowDevice;

        string strHQL;

        strUserHostAddress = Request.UserHostAddress;

        strUserCode = TB_UserCode.Text.Trim().ToUpper();
        strPassword = TB_Password.Text.Trim();
        if (strUserCode == "" | strPassword == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZYHMHMMDBNWKJC") + "');</script>");
            return;
        }

        if (ShareClass.SqlFilter(strUserCode) | ShareClass.SqlFilter(strPassword))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZZHHYFFZHDLSB") + "');</script>");
            return;
        }

        strSMSVerification = System.Configuration.ConfigurationManager.AppSettings["SMSVerification"].Trim().ToUpper();
        strVerificationCode = System.Configuration.ConfigurationManager.AppSettings["VerificationCode"].Trim().ToUpper();
        if (strVerificationCode == "YES")
        {
            if (!ShareClass.IsMobileDeviceCheckAgent())
            {
                if (Request.Cookies["CheckCode"] == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZNDLSZYBJYCOOKIESNBXSZLYXSYCOOKIESXHCNSYBXT") + "');</script>");
                    TB_CheckCode.Text = "";
                    return;
                }

                if (String.Compare(Request.Cookies["CheckCode"].Value, TB_CheckCode.Text, true) != 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZYZMCWSRZDYZM") + "');</script>");
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
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGYSJDXMYZXDDXM") + "');</script>");
            //        return;
            //    }
            //    else
            //    {
            //        if (strSMSCode != strCheckCode)
            //        {
            //            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGDXYZMCWJC") + "');</script>");
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
                //�������ݿ�
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
                Session["CssDirectory"] = ds.Tables[0].Rows[0]["CssDirectory"].ToString().Trim();
                Session["CssDirectoryChangeNumber"] = ds.Tables[0].Rows[0]["CssDirectoryChangeNumber"].ToString().Trim();
                try
                {
                    Session["LeftBarExtend"] = ds.Tables[0].Rows[0]["LeftBarExtend"].ToString().Trim();
                }
                catch
                {
                    Session["LeftBarExtend"] = "NO";
                }

                //HttpBrowserCapabilities GetBrowserCapabilities = HttpContext.Current.Request.Browser;
                //if (ShareClass.GetBrowser(GetBrowserCapabilities) == "SF")
                //{

                //    if (ds.Tables[0].Rows[0]["CssDirectory"].ToString().Trim() == "CssBlue")
                //    {
                //        Session["CssDirectory"] = "CssGreen";
                //        //Session["CssDirectoryChangeNumber"] = "1";

                //        string strHQL1 = "Update T_ProjectMember Set CssDirectory = 'CssGreen' where UserCode = " + "'" + strUserCode + "'";
                //        ShareClass.RunSqlCommand(strHQL1);

                //        //�����ҳ���ļ���ӿ�����ˢ��ҳ�滺��
                //        ShareClass.AddSpaceLineToFileForRefreshCache();
                //    }
                //}

                Session["SkinFlag"] = ds.Tables[0].Rows[0]["CssDirectory"].ToString().Trim() + ds.Tables[0].Rows[0]["LangCode"].ToString().Trim();

                try
                {

                    //��ʼ����������
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

                //YESʱҳ������ڿ���ڴ򿪣�����ر�
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

                //�Ƿ��Զ���������������ѡ����һ����������ѡ��Ա
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

                //���ע�����Ƿ�Ϸ�
                string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                try
                {
                    TakeTopLicense license = new TakeTopLicense();
                   
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

                //��ʼ�����˿ռ�ģ��
                new System.Threading.Thread(delegate ()
                {
                    String strLangCode;

                    strUserCode = Session["UserCode"].ToString();
                    strUserType = Session["UserType"].ToString();
                    strLangCode = Session["LangCode"].ToString();

                    try
                    {
                        strHQL = string.Format(@"Insert Into t_promodulelevelforpageuser(modulename,usercode,usertype,visible,sortnumber) 
               select modulename,'{0}','{1}',visible,sortnumber from T_ProModuleLevelForPage  Where ParentModule = 'PersonalSpaceSaaS'
               and PageName <> 'TTPersonalSpaceNews.aspx' and LangCode = '{2}' and Visible ='YES' and IsDeleted = 'NO'
               and modulename not in (select modulename from t_promodulelevelforpageuser where usercode = '{0}' and UserType = '{1}') 
               Order By SortNumber ASC", strUserCode, strUserType, strLangCode);
                        ShareClass.RunSqlCommand(strHQL);
                    }
                    catch
                    {
                    }

                }).Start();


                try
                {
                    if (Session["CssDirectoryChangeNumber"].ToString() != "2" & Session["CssDirectoryChangeNumber"].ToString() != "0")
                    {
                        //���û�����ı�־
                        ShareClass.SetPageCacheMark("2");
                    }

                    //�����¼��־
                    ShareClass.InsertUserLogonLog(strUserCode, strUserName, "WEB");
                }
                catch
                {
                }

                if (Session["SystemVersionType"].ToString() == "SAAS")
                {
                    string strScript = "<script>openMDIFrom('TakeTopLRExMDISAAS.html');</script>";
                    ClientScript.RegisterStartupScript(GetType(), "", strScript);
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
                ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGYHDMHMMCWHYBZZSY") + "');</script>");
            }
        }
        catch (Exception err)
        {
            Response.Redirect("TTDisplayErrors.aspx");
        }

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

            strMsg =  LanguageHandle.GetWord("DuanXinYanZhengMa") +"��" + strSMSCode + "," + LanguageHandle.GetWord("DangTianYouXiao");

            if (msg.SendMSM("Message", strUserCode, strMsg, strUserCode))
            {
                InsertOrUpdateSMSCode(strUserCode, strSMSCode);

                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZDXYZMYFSCS") + "');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGDXYZMFSSBJCDXJKHWLLJ") + "');</script>");
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZJGYHDMHMMCWBNDDXMJC") + "');</script>");
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

        Response.Redirect("DefaultDemo.aspx");
    }

    protected string GetModulePageName(string strModuleName)
    {
        string strHQL;

        strHQL = "select PageName from T_ProModuleLevel  where ModuleName = " + "'" + strModuleName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        return ds.Tables[0].Rows[0][0].ToString();
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

    protected SystemMDIStyle GetSystemMDIStyle(string strMDIStyle)
    {
        string strHQL;
        IList lst;


        strHQL = "From SystemMDIStyle as systemMDIStyle Where MDIStyle = " + "'" + strMDIStyle + "'";
        SystemMDIStyleBLL systemMDIStyleBLL = new SystemMDIStyleBLL();
        lst = systemMDIStyleBLL.GetAllSystemMDIStyles(strHQL);

        if (lst.Count > 0)
        {
            SystemMDIStyle systemMDIStyle = (SystemMDIStyle)lst[0];
            return systemMDIStyle;
        }
        else
        {
            return null;
        }
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
