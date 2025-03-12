
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
//using NPOI.SS.Formula.Functions;
//using Microsoft.Office.Interop.MSProject;
//using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using TakeTopCore;

using ZXing;
using ZXing.QrCode;

using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;
using LumiSoft.Net.POP3.Client;

using Microsoft.CSharp;

using Npgsql;

using ProjectMgt.BLL;
using ProjectMgt.Model;

using RTXSAPILib;

using RTXServerApi;
using MSXML2;
using TakeTopGantt;

/// <summary>
/// Summary description for ShareClass
/// </summary>
public static class ShareClass
{
    static ShareClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string SystemVersionID = "V2025.3.11";

    public static string SystemLatestLoginUser = "";
    public static string SystemDBer = "";
    public static DateTime systemStartupTime = DateTime.Now;

    #region �û���¼����

    //��ȡ�����չ��״̬
    public static string GetLeftBarExtendStatus(string strUserCode)
    {
        string strHQL;

        strHQL = "Select LeftBarExtend From T_ProjectMember Where UserCode ='" + strUserCode + "'";
        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            return ds.Tables[0].Rows[0]["LeftBarExtend"].ToString().Trim();
        }
        catch
        {
            return "NO";
        }
    }


    //���������չ��״̬
    public static void UpdateLeftBarExtendStatus(string strUserCode, string strLeftBarExtend)
    {
        string strHQL;

        strHQL = "Update T_ProjectMember Set LeftBarExtend = '" + strLeftBarExtend + "' Where UserCode ='" + strUserCode + "'";
        ShareClass.RunSqlCommand(strHQL);
    }

    //�ض���ҳ�浽ָ�����
    public static void Redirect(this HttpResponse response, string url, string target, string windowFeatures)
    {
        if ((String.IsNullOrEmpty(target) ||
        target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
        String.IsNullOrEmpty(windowFeatures))
        {
            response.Redirect(url);
        }
        else
        {
            Page page = (Page)HttpContext.Current.Handler; if (page == null)
            {
                throw new
                InvalidOperationException("Cannot redirect to new window .");
            }
            url = page.ResolveClientUrl(url);
            string script;
            if (!String.IsNullOrEmpty(windowFeatures))
            {
                script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            }
            else
            {
                script = @"window.open(""{0}"", ""{1}"");";
            }
            script = String.Format(script, url, target, windowFeatures);
            ScriptManager.RegisterStartupScript(page,
            typeof(Page), "Redirect", script, true);
        }
    }


    //ִ�ж�ʱ��ҳ
    public static void ExecuteTakeTopTimer()
    {
        if (ShareClass.SystemLatestLoginUser == "")
        {
            ShareClass.SystemLatestLoginUser = "Timer";

            try
            {
                System.Threading.Thread.Sleep(5000);

                string strUrl = ShareClass.GetCurrentSiteRootPath() + "TakeTopTimer.aspx";

                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
                System.Net.HttpWebResponse _HttpWebResponse = (System.Net.HttpWebResponse)_HttpWebRequest.GetResponse();
                System.IO.Stream _Stream = _HttpWebResponse.GetResponseStream();//�õ���д���ֽ��� 

                //ִ�� AnotherPage.aspx����������������ڵ�ǰҳ����


                HttpContext.Current.Server.Execute(strUrl);
            }
            catch (Exception err)
            {
                //LogClass.WriteLogFile(err.Message.ToString());
            }

            //����¼�û�
            ShareClass.SystemLatestLoginUser = "";
        }
    }



    //��ʼ���û�ģ��
    public static void InitialUserModules(string strSampleUserCode, string strCurrentUserCode)
    {
        string strHQL;

        strHQL = string.Format(@"insert into t_promodule(modulename, usercode, visible, moduletype, usertype,ModuleDefinition,DiyFlow)
            select a.modulename,'{0}','NO',a.moduletype,a.usertype,a.ModuleDefinition,a.DiyFlow from t_promodule a
            where a.usercode = 'ADMIN' and rtrim(a.modulename) || rtrim(a.moduletype) || rtrim(a.usertype)
                not in (select rtrim(b.modulename) || rtrim(b.moduletype) || rtrim(b.usertype) from t_promodule b where b.usercode = '{0}' and b.moduletype = a.moduletype and b.usertype = a.usertype)", strSampleUserCode);
        ShareClass.RunSqlCommand(strHQL);

        strHQL = string.Format(@"insert into t_promodule(modulename, usercode, visible, moduletype, usertype,ModuleDefinition,DiyFlow)
            select a.modulename,'{0}',a.visible,a.moduletype,a.usertype,a.ModuleDefinition,a.DiyFlow from t_promodule a
            where a.usercode = '{1}'
                and(rtrim(a.modulename) || rtrim(a.moduletype) || rtrim(a.usertype) not in (select rtrim(b.modulename) || rtrim(b.moduletype) || rtrim(b.usertype) from t_promodule b where b.usercode = '{0}' and b.moduletype = a.moduletype and b.usertype = a.usertype)
                and rtrim(a.modulename)|| rtrim(a.moduletype) || rtrim(a.usertype) in (select rtrim(c.modulename) || rtrim(c.moduletype) || rtrim(c.usertype) from t_promodulelevel c where c.moduletype = a.moduletype and c.usertype = a.usertype  and c.visible = 'YES' and c.isdeleted = 'NO'))", strCurrentUserCode, strSampleUserCode);
        ShareClass.RunSqlCommand(strHQL);
    }

    //ȡ�÷���������ϵͳ����:UNIX Or WIN
    public static string GetSystemType()
    {
        //��ȡϵͳ��Ϣ
        System.OperatingSystem osInfo = System.Environment.OSVersion;
        //��ȡ����ϵͳID
        System.PlatformID platformID = osInfo.Platform;

        return platformID.ToString();
    }

    public static string URLEncode(string strURL)
    {
        return System.Web.HttpUtility.UrlEncode(strURL);
    }

    public static string UrlDecode(string strURL)
    {
        return System.Web.HttpUtility.UrlDecode(strURL);
    }

    //ȡ�õ�ǰģ��ĵ�ǰ��������
    public static string GetPageTitle(string strPageName)
    {
        string strHQL;
        try
        {
            string strModuleName = HttpContext.Current.Request.QueryString["ModuleName"];
            string strModuleType = HttpContext.Current.Request.QueryString["ModuleType"];

            string strUserType = HttpContext.Current.Session["UserType"].ToString();
            string strLangCode = HttpContext.Current.Session["LangCode"].ToString();

            if (strModuleName != null & strModuleType != null)
            {
                strHQL = "Select HomeModuleName From T_ProModuleLevel Where ModuleName = '" + strModuleName + "' and ModuleType = '" + strModuleType + "' and UserType = '" + strUserType + "' and PageName = '" + strPageName + "' and LangCode = '" + strLangCode + "' limit 1";

                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString().Trim();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                //strHQL = "Select HomeModuleName From T_ProModuleLevel Where UserType = '" + strUserType + "' and PageName = '" + strPageName + "' and LangCode = '" + strLangCode + "' limit 1";
                return "";
            }

        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            return "";
        }
    }

    //���û�����ı�־
    public static void SetPageCacheMark(string strMark)
    {
        string strHQL;
        strHQL = "Update T_ProjectMember Set CssDirectoryChangeNumber = " + strMark + " Where UserCode = '" + HttpContext.Current.Session["UserCode"].ToString() + "'";
        ShareClass.RunSqlCommand(strHQL);
    }

    //�����ҳ���ļ���ӿ�����ˢ��ҳ�滺��
    public static void AddSpaceLineToFileForRefreshCache()
    {
        //��ƽ̨���������һ��ע��
        ShareClass.AddSpaceLineToFile("TakeTopLRTop.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopLRExLeft.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopCSLRLeft.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopMainTab.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopMainTop.aspx", "");

        ShareClass.AddSpaceLineToFile("TakeTopPersonalSpace.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopPersonalSpaceForOuterUser.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopPersonalSpaceSAAS.aspx", "");

        ShareClass.AddSpaceLineToFile("TTPersonalSpaceAnalysisChart.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceMembersWebAddress.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceMembersWebAddressForOuter.aspx", "");

        ShareClass.AddSpaceLineToFile("TTPersonalSpaceMyMonthSchedule.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceNewsList.aspx", "");

        ShareClass.AddSpaceLineToFile("TTPersonalSpaceNewsNotice.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceNewsNoticeForSAAS.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceNoticeList.aspx", "");

        ShareClass.AddSpaceLineToFile("TTPersonalSpaceProject.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceTask.aspx", "");

        ShareClass.AddSpaceLineToFile("TTPersonalSpaceToDoNewsForOuter.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceToDoList.aspx", "");

        ShareClass.AddSpaceLineToFile("TTPersonalSpaceWorkflow.aspx", "");
        ShareClass.AddSpaceLineToFile("TTPersonalSpaceWorkflowForOuter.aspx", "");
    }

    //����������������ҳ���ļ���ӿ�����ˢ��ҳ�滺��
    public static void AddSpaceLineToLeftColumnForRefreshCache()
    {
        ////��ƽ̨���������һ��ע��
        //ShareClass.AddSpaceLineToFile("TakeTopLRExLeft.aspx", "");
        //ShareClass.AddSpaceLineToFile("TakeTopCSLRLeft.aspx", "");
        //ShareClass.AddSpaceLineToFile("TakeTopLRTop.aspx", "");
    }

    //����������˿ռ����ҳ���ļ���ӿ�����ˢ��ҳ�滺��
    public static void AddSpaceLineToPersonalSpaceForRefreshCache()
    {
        //ҳ���ļ���ע���ַ�����ˢ�»���
        ShareClass.AddSpaceLineToFile("TakeTopPersonalSpace.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopPersonalSpaceForOuterUser.aspx", "");
        ShareClass.AddSpaceLineToFile("TakeTopPersonalSpaceSAAS.aspx", "");
    }

    //��ʼ��ʵ���࣬�Լӿ�����Ĳ����ٶ�
    public static void InitialNhibernateEntryClass()
    {
        try
        {
            string strHQL;
            strHQL = "From UserDuty as userDuty";
            UserDutyBLL userDutyBLL = new UserDutyBLL();
            IList lst = userDutyBLL.GetAllUserDutys(strHQL);
        }
        catch
        {
        }
    }

    //�����û���¼IP���û����ж��Ƿ���ֹ���û���¼ϵͳ
    public static bool CheckUserLoginManage(string strUserCode, string strUserName)
    {
        //�����û���¼IP�ж��Ƿ���ֹ�û���¼ϵͳ
        string strHQL;
        string strLoginID, strIsAllMember, strIsForbidLogin, strLoginUserCode;
        string strMsg, strIP, strUserHostAddress;

        strUserHostAddress = HttpContext.Current.Request.UserHostAddress.Trim();

        if (strUserCode != "ADMIN")
        {
            DataSet ds = ShareClass.GetUserLoginManageDataSet(strUserCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strLoginID = ds.Tables[0].Rows[0][0].ToString().Trim();
                strIsAllMember = ds.Tables[0].Rows[0][1].ToString().Trim();
                strIsForbidLogin = ds.Tables[0].Rows[0][2].ToString().Trim();
                strMsg = ds.Tables[0].Rows[0][3].ToString().Trim();
                strLoginUserCode = ds.Tables[0].Rows[0][4].ToString().Trim();
                strIP = ds.Tables[0].Rows[0][5].ToString().Trim();

                if (strIP == "" | strIP.IndexOf(strUserHostAddress) >= 0)
                {
                    if (strIsForbidLogin == "YES")
                    {
                        if (strMsg != "")
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + strMsg + "');</script>");

                            strHQL = "Insert Into T_UserLoginManageMsgRelatedUser(LoginID,UserCode,UserName)";
                            strHQL += " Values(" + strLoginID + ",'" + strUserCode + "','" + strUserName + "')";
                            ShareClass.RunSqlCommand(strHQL);
                        }

                        return false;
                    }
                    else
                    {
                        if (strMsg != "")
                        {
                            strHQL = "Select LoginID From T_UserLoginManageMsgRelatedUser Where LoginID = " + strLoginID + " and UserCode Like " + "'" + strUserCode + "'";
                            ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + strMsg + "');</script>");

                                strHQL = "Insert Into T_UserLoginManageMsgRelatedUser(LoginID,UserCode,UserName)";
                                strHQL += " Values(" + strLoginID + ",'" + strUserCode + "','" + strUserName + "')";
                                ShareClass.RunSqlCommand(strHQL);
                            }
                        }

                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    public static DataSet GetSystemMDIStyle(string strMDIStyle)
    {
        string strHQL;

        strHQL = "Select * From T_SystemMDIStyle Where MDIStyle = " + "'" + strMDIStyle + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemMDIStyle");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds;
        }
        else
        {
            return null;
        }
    }

    //��ʼ��ҳ������ģ��
    public static void CopyAllModuleForHomeLanguage()
    {
        string strHQL, strLangHQL;
        string strLangCode;

        string strFromLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];

        strLangHQL = "Select LangCode From T_SystemLanguage Where LangCode <> " + "'" + strFromLangCode + "'";
        strLangHQL += " Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strLangHQL, "T_SystemLanguage");
        try
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strLangCode = ds.Tables[0].Rows[i][0].ToString().Trim();

                strHQL = "Insert Into T_ProModuleLevelForPage(ModuleName,ParentModule,SortNumber,PageName ,ModuleType ,UserType ,Visible,LangCode,HomeModuleName,IsDeleted)";
                strHQL += " SELECT ModuleName,ParentModule,SortNumber,PageName ,ModuleType ,UserType ,Visible," + "'" + strLangCode + "'" + ",HomeModuleName,IsDeleted FROM T_ProModuleLevelForPage";
                strHQL += " Where LangCode = '" + strFromLangCode + "' and ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) ||ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType))  Not in (Select ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) ||ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType)) From T_ProModuleLevelForPage Where LangCode = " + "'" + strLangCode + "'" + ")";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Update B Set B.SortNumber = A.SortNumber From T_ProModuleLevelForPage A,T_ProModuleLevelForPage B Where A.ModuleName = B.ModuleName and A.LangCode = '" + strFromLangCode + "' AND B.LangCode =" + "'" + strLangCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_ProModuleLevelForPage Where LangCode = " + "'" + strLangCode + "'" + " and ModuleType in ('SYSTEM','APP')";
                strHQL += " and ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) || ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType))  Not in (Select ltrim(rtrim(ModuleName)) || ltrim(rtrim(ParentModule)) || ltrim(rtrim(ModuleType)) || ltrim(rtrim(UserType)) From T_ProModuleLevelForPage Where LangCode = '" + strFromLangCode + "')";
                ShareClass.RunSqlCommand(strHQL);
            }
        }
        catch
        {
        }
    }

    //�ж��û��Ƿ��д�ģ��
    public static bool IsExistModuleByUserCode(string strUserCode, string strModuleName, string strModuleType, string strUserType)
    {
        string strHQL = "Select * From T_ProModule Where UserCode = " + "'" + strUserCode + "'" + " and ModuleName = " + "'" + strModuleName + "'" + " and ModuleType = " + "'" + strModuleType + "'";
        strHQL += " and UserType = " + "'" + strUserType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModule");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //ȡ��ģ��HOME����
    public static string GetHomeModuleName(string strModuleName, string strLangCode)
    {
        string strHQL;

        strHQL = "Select HomeModuleName From T_ProModuleLevel Where ModuleName = " + "'" + strModuleName + "'" + " and LangCode = " + "'" + strLangCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    //�����û�����͵�¼IP�ж��Ƿ���ֹ�û���¼ϵͳ
    public static DataSet GetUserLoginManageDataSet(string strUserCode)
    {
        string strHQL;

        strUserCode = "%" + strUserCode + "%";

        strHQL = "Select ID, IsAllMember,IsForbidLogin,Message,UserCode,IP From T_UserLoginManage Where ";
        strHQL += " ((UserCode Like " + "'" + strUserCode + "'" + ")";
        strHQL += " Or (IsAllMember = 'YES'))";
        strHQL += " And Status = 'InUse'";
        strHQL += " Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "T_UserLoginManage");

        return ds;
    }

    //ȡ�û���¼������Ϣ
    public static string GetUserLoginMessage(string strUserCode)
    {
        string strHQL;
        string strUserHostAddress, strMessage;

        strUserCode = "%" + strUserCode + "%";

        strUserHostAddress = HttpContext.Current.Request.UserHostAddress.Trim();
        strUserHostAddress = "%" + strUserHostAddress + "%";

        strHQL = "Select Message From T_UserLoginManage Where UserCode Like " + "'" + strUserCode + "'" + " and IP Like " + "'" + strUserHostAddress + "'";
        strHQL += " and Status = 'InUse'";
        strHQL += " Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "T_UserLoginManage");

        if (ds.Tables[0].Rows.Count > 0)
        {
            strMessage = ds.Tables[0].Rows[0][0].ToString().Trim();

            return strMessage;
        }
        else
        {
            return "";
        }
    }

    //�����û���־
    public static void InsertUserLogonLog(string strUserCode, string strUserName, string strDeviceType)
    {
        string strUserHostAddress = HttpContext.Current.Request.UserHostAddress.Trim();
        string strUserHostName = HttpContext.Current.Request.UserHostName.Trim();

        try
        {
            string strHQL;
            strHQL = "Insert Into T_LogonLog(UserIP,UserHostName,Position,LoginTime,UserCode,UserName,LastestTime,DeviceType)";
            strHQL += " Values('" + strUserHostAddress + "','" + strUserHostName + "','" + ShareClass.GetIPinArea(strUserHostAddress) + "',now(),";
            strHQL += " '" + strUserCode + "','" + strUserName + "',now(),'" + strDeviceType + "')";

            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('���棬�û���¼��־����ϸ������־��¼�ѳ���21�ڣ��뼰ʱ��յ�¼��־��');</script>");
        }
    }

    public static void InsertUserLogonLogForHandler(string strUserCode, string strUserName, string strUserHostAddress, string strUserHostName, string strPosition, string strDeviceType)
    {
        LogonLogBLL logonLogBLL = new LogonLogBLL();
        LogonLog logonLog = new LogonLog();

        logonLog.UserIP = strUserHostAddress;
        logonLog.UserHostName = strUserHostName;
        logonLog.Position = strPosition;
        logonLog.LoginTime = DateTime.Now;
        logonLog.UserCode = strUserCode;
        logonLog.UserName = strUserName;
        logonLog.LastestTime = DateTime.Now;
        logonLog.DeviceType = strDeviceType;

        try
        {
            logonLogBLL.AddLogonLog(logonLog);
        }
        catch
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('���棬�û���¼��־����ϸ������־��¼�ѳ���21�ڣ��뼰ʱ��յ�¼��־��');</script>");
        }
    }

    public static void LoadSystemMDIStyle(DropDownList DL_SystemMDIStyle)
    {
        string strHQL = "from SystemMDIStyle as systemMDIStyle Order By systemMDIStyle.SortNumber ASC";

        SystemMDIStyleBLL systemMDIStyleBLL = new SystemMDIStyleBLL();
        IList lst = systemMDIStyleBLL.GetAllSystemMDIStyles(strHQL);

        DL_SystemMDIStyle.DataSource = lst;
        DL_SystemMDIStyle.DataBind();
    }

    public static void LoadLanguageForDropList(DropDownList DL_Language)
    {
        string strHQL;

        strHQL = "Select trim(LangCode) as LangCode,Language From T_SystemLanguage Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemLanguage");
        DL_Language.DataSource = ds;
        DL_Language.DataBind();
    }

    public static string GetDayPilotLanguage()
    {
        string strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        if (strLangCode == "zh-CN")
        {
            return "zh-CN";
        }

        if (strLangCode == "en")
        {
            return "en-US";
        }

        if (strLangCode == "fr")
        {
            return "fr-FR";
        }

        if (strLangCode == "de")
        {
            return "de-DE";
        }

        if (strLangCode == "ja")
        {
            return "ja-JP";
        }

        if (strLangCode == "ru")
        {
            return "ru-RU";
        }

        if (strLangCode == "es")
        {
            return "es-ES";
        }

        if (strLangCode == "zh-tw")
        {
            return "zh-CN";
        }

        return "en-US";
    }

    public static void MakeUserDirectory(string strUserCode)
    {
        string strDirectory, strDocSavePath, strYearMonth;
        int intResult;

        //����˽���ļ�Ŀ¼
        strDocSavePath = HttpContext.Current.Server.MapPath("Doc");
        strYearMonth = DateTime.Now.ToString("yyyyMM");

        strDirectory = strDocSavePath + "\\" + strYearMonth + "\\" + strUserCode + "\\Doc";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create Doc Director ��')", true);

        strDirectory = strDocSavePath + "\\" + strYearMonth + "\\" + strUserCode + "\\Images";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create Images Director ��')", true);

        strDirectory = strDocSavePath + "\\" + strYearMonth + "\\" + strUserCode + "\\MailAttachments";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create  MailAttachments Director ��')", true);

        strDirectory = strDocSavePath + "\\XML";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create  XML Director ��')", true);

        strDirectory = strDocSavePath + "\\Log";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create  XML Director ��')", true);

        strDirectory = strDocSavePath + "\\WorkFlowTemplate";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create  WorkFlowTemplate Director ��')", true);

        strDirectory = strDocSavePath + "\\UserPhoto";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create  UserPhoto Director ��')", true);

        strDirectory = strDocSavePath + "\\Report";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create  Report Director ��')", true);

        strDirectory = strDocSavePath + "\\RTXAccount";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create RTXAccount Director ��')", true);

        strDirectory = strDocSavePath + "\\BackupDB";
        intResult = CreateDirectory(strDirectory);
        //if (intResult == 2)
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create RTXAccount Director ��')", true);

        //strDirectory = strDocSavePath + "\\" + strYearMonth + "\\BackupDB";
        //intResult = CreateDirectory(strDirectory);
        ////if (intResult == 2)
        ////    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('Failed to create  BackupDB Director ��')", true);
    }

    public static int CreateDirectory(string strDirectory)
    {
        DirectoryInfo NewDirInfo;

        if (!Directory.Exists(strDirectory))
        {
            try
            {
                Directory.CreateDirectory(strDirectory);
                NewDirInfo = new DirectoryInfo(strDirectory);
                NewDirInfo.Attributes = FileAttributes.Normal;
                return 1;
            }
            catch
            {
                return 2;
            }
        }
        else
        {
            return 3;
        }
    }

    //   /****************************************
    // * �������ƣ�GetDirectoryLength(string dirPath)
    // * ����˵������ȡ�ļ��д�С
    // * ��    ����dirPath:�ļ�����ϸ·��
    // * ����ʾ�У�
    // *           string Path = Server.MapPath("templates");
    // *           Response.Write(EC.FileObj.GetDirectoryLength(Path));
    //*****************************************/
    /// <summary>
    /// ��ȡ�ļ��д�С
    /// </summary>
    /// <param name="dirPath">�ļ���·��</param>
    /// <returns></returns>
    public static long GetDirectoryLength(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            return 0;
        long len = 0;
        DirectoryInfo di = new DirectoryInfo(dirPath);
        foreach (FileInfo fi in di.GetFiles())
        {
            len += fi.Length;
        }
        DirectoryInfo[] dis = di.GetDirectories();
        if (dis.Length > 0)
        {
            for (int i = 0; i < dis.Length; i++)
            {
                len += GetDirectoryLength(dis[i].FullName);
            }
        }
        return len;
    }

    //��ȡĳ���ļ��еĴ�С������һ��
    public static long GetFoldSize(string dirPath)
    {
        FileInfo info = new FileInfo(dirPath);

        return info.Length;
    }

    //�������ݿ�ֻ���û�ID��һ���ڱ��������
    public static string getDBReadOnlyUserID()
    {
        string[] strConnectStringList;

        strConnectStringList = HttpContext.Current.Request.Url.AbsolutePath.Split("/".ToCharArray());
        return (strConnectStringList[1].Replace(".aspx", "") + "DBReadOnlyUser").ToLower();
    }

    //����������
    public static string genernalPassword()
    {
        string chars = "0123456789ABCDEFGHIJKLMNOPQSTUVWXYZabcdefghijklmnpqrstuvwxyz@*";
        Random randrom = new Random(getNewSeed());

        string str = "";
        for (int j = 0; j < 50; j++)
        {
            str = "";
            for (int i = 0; i < 8; i++)
            {
                str += chars[randrom.Next(chars.Length)];//randrom.Next(int i)����һ��С����ָ�����ֵ�ķǸ������
            }
            //������������������
            if (!Regex.IsMatch(str, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$"))
            {
                continue;
            }
            else
            {
                break;
            }
        }

        return str;
    }

    public static int getNewSeed()
    {
        byte[] rndBytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(rndBytes);
        return BitConverter.ToInt32(rndBytes, 0);
    }

    #endregion �û���¼����

    #region Ա����������

    //�ж��û��Ƿ��ڴ������µ���
    public static bool CheckUserIsExist(string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_ProjectMember Where UserCode = '" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string DepartmentStringByAuthoritySuperUserForGroup(string strParentCode, string strUserCode)
    {
        string strHQL;

        DataSet ds1, ds2;

        string strDepartCode, strDepartName;
        string strDepartString = "";

        strHQL = "Select DepartCode,DepartName From T_Department Where ParentCode = " + "'" + strParentCode + "'";
        strHQL += " and ((Authority = 'All')";
        strHQL += " or ((Authority = 'Part') ";
        strHQL += " and (DepartCode in (select DepartCode from T_DepartmentUser where UserCode =" + "'" + strUserCode + "'" + "))))";
        strHQL += " Order By DepartCode ASC";

        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strDepartCode = ds1.Tables[0].Rows[i][0].ToString();
            strDepartName = ds1.Tables[0].Rows[i][1].ToString();

            if (strDepartString.IndexOf("'" + strDepartCode + "'" + ",") < 0)
            {
                strDepartString += "'" + strDepartCode + "'" + ",";

                strHQL = "Select DepartCode,DepartName From T_Department Where ParentCode = " + "'" + strDepartCode + "'";
                ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strDepartString += DepartmentStringByAuthoritySuperUserForGroup(strDepartCode, strUserCode);
                }
            }
        }

        return strDepartString;
    }

    public static bool VerifyUserCode(string strUserCode, String strDepartString)
    {
        string strHQL;
        IList lst;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        if (lst.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool VerifyUserName(string strUserName, string strDepartString)
    {
        string strHQL;
        IList lst;

        strHQL = " from ProjectMember as projectMember where projectMember.UserName = " + "'" + strUserName + "'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        if (lst.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static decimal UpdateKPICheckDetailTotalPoint(string strKPICheckID)
    {
        string strHQL1, strHQL2, strHQL3, strHQL4;
        string strID;
        decimal deSelfCheckWeight = 0, deLeaderCheckWeight = 0, deThirdPartCheckWeight = 0, deSqlCheckWeight = 0, deHRCheckWeight = 0;
        decimal deDetailTotalPoint = 0, deTotalPoint = 0, deWeight = 0;
        DataSet ds;

        strHQL1 = "Select SelfCheckWeight,LeaderCheckWeight,ThirdPartCheckWeight,SqlCheckWeight,HRCheckWeight From T_KPICheckTypeWeight";
        ds = ShareClass.GetDataSetFromSql(strHQL1, "T_KPICheckTypeWeight");

        if (ds.Tables[0].Rows.Count > 0)
        {
            deSelfCheckWeight = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            deLeaderCheckWeight = decimal.Parse(ds.Tables[0].Rows[0][1].ToString());
            deThirdPartCheckWeight = decimal.Parse(ds.Tables[0].Rows[0][2].ToString());
            deSqlCheckWeight = decimal.Parse(ds.Tables[0].Rows[0][3].ToString());
            deHRCheckWeight = decimal.Parse(ds.Tables[0].Rows[0][4].ToString());
        }

        strHQL2 = "Select ID,(SelfPoint*" + deSelfCheckWeight.ToString() + "+LeaderPoint*" + deLeaderCheckWeight.ToString() + "+ThirdPartPoint*" + deThirdPartCheckWeight + "+SqlPoint*" + deSqlCheckWeight + "+HRPoint*" + deHRCheckWeight + ") as TotalDetailPoint,Weight From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID;
        ds = ShareClass.GetDataSetFromSql(strHQL2, "T_UserKPICheck");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strID = ds.Tables[0].Rows[i][0].ToString().Trim();
            deDetailTotalPoint = decimal.Parse(ds.Tables[0].Rows[i][1].ToString());
            deWeight = decimal.Parse(ds.Tables[0].Rows[i][2].ToString());

            strHQL3 = "Update T_UserKPICheckDetail Set Point = " + deDetailTotalPoint.ToString() + " Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL3);

            deTotalPoint += deDetailTotalPoint * deWeight;
        }

        strHQL4 = " Update T_UserKPICheck Set TotalPoint = " + deTotalPoint.ToString() + " Where KPICheckID = " + strKPICheckID;
        ShareClass.RunSqlCommand(strHQL4);

        return deTotalPoint;
    }

    //ȡ�ô�Ա������Ĵ����͵��������
    public static string GetTotalLeaveDayNumberInCurrentYear(string strLeaveType, string strApplicantCode, string strLeaveTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_LeaveApplyForm Where SUBSTRING(to_char(StartTime, 'yyyymmdd'), 1, 4) = '" + strLeaveTime.Substring(0, 4) + "'";
        strHQL += " And LeaveType = '" + strLeaveType + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_LeaveApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô�Ա�����µĴ����͵��������
    public static string GetTotalLeaveDayNumberInCurrentMonth(string strLeaveType, string strApplicantCode, string strLeaveTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_LeaveApplyForm Where SUBSTRING (to_char( StartTime, 'yyyymmdd'), 1, 6)= '" + strLeaveTime.Substring(0, 6) + "'";
        strHQL += " And LeaveType = '" + strLeaveType + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_LeaveApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô�Ա��������������͵��������
    public static string GetTotalAllLeaveDayNumberInCurrentYear(string strLeaveType, string strApplicantCode, string strLeaveTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_LeaveApplyForm Where SUBSTRING(to_char( StartTime, 'yyyymmdd'), 1, 4) = '" + strLeaveTime.Substring(0, 4) + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_LeaveApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô�Ա�����µ��������͵��������
    public static string GetTotalAllLeaveDayNumberInCurrentMonth(string strLeaveType, string strApplicantCode, string strLeaveTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_LeaveApplyForm Where SUBSTRING (to_char( StartTime, 'yyyymmdd'), 1, 6)= '" + strLeaveTime.Substring(0, 6) + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_LeaveApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô�Ա������Ĵ����͵��������
    public static string GetTotalOvertimeDayNumberInCurrentYear(string strOvertimeType, string strApplicantCode, string strOvertimeTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_OvertimeApplyForm Where SUBSTRING(to_char( StartTime, 'yyyymmdd'), 1, 4) = '" + strOvertimeTime.Substring(0, 4) + "'";
        strHQL += " And OvertimeType = '" + strOvertimeType + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_OvertimeApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô�Ա�����µĴ����͵��������
    public static string GetTotalOvertimeDayNumberInCurrentMonth(string strOvertimeType, string strApplicantCode, string strOvertimeTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_OvertimeApplyForm Where SUBSTRING (to_char( StartTime, 'yyyymmdd'), 1, 6)= '" + strOvertimeTime.Substring(0, 6) + "'";
        strHQL += " And OvertimeType = '" + strOvertimeType + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_OvertimeApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô�Ա��������������͵��������
    public static string GetTotalAllOvertimeDayNumberInCurrentYear(string strOvertimeType, string strApplicantCode, string strOvertimeTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_OvertimeApplyForm Where SUBSTRING(to_char( StartTime, 'yyyymmdd'), 1, 4) = '" + strOvertimeTime.Substring(0, 4) + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_OvertimeApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô�Ա�����µ��������͵��������
    public static string GetTotalAllOvertimeDayNumberInCurrentMonth(string strOvertimeType, string strApplicantCode, string strOvertimeTime)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(DayNum),0) From T_OvertimeApplyForm Where SUBSTRING (to_char( StartTime, 'yyyymmdd'), 1, 6)= '" + strOvertimeTime.Substring(0, 6) + "'";
        strHQL += " And Creator = '" + strApplicantCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_OvertimeApplyForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //����KPI��ϵͳ����
    public static decimal CalculateSystemPoint(string strKPICheckID)
    {
        string strHQL1, strHQL2;
        IList lst1, lst2;

        string strSql1, strKPIUserCode, strStatus;

        string strKPICheckStartDate, strKPICheckEndDate;
        decimal deSystemPoint, deUnitSqlPoint;
        int intID;

        strHQL1 = "From UserKPICheck as userKPICheck Where KPICheckID = " + strKPICheckID;
        UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
        lst1 = userKPICheckBLL.GetAllUserKPIChecks(strHQL1);

        UserKPICheck userKPICheck = (UserKPICheck)lst1[0];

        strStatus = userKPICheck.Status.Trim();

        if (strStatus == "Closed")
        {
            return userKPICheck.TotalSqlPoint;
        }

        strKPICheckStartDate = userKPICheck.StartTime.ToString("yyyyMMdd");
        strKPICheckEndDate = userKPICheck.EndTime.ToString("yyyyMMdd");
        strKPIUserCode = userKPICheck.UserCode.Trim();


        strHQL2 = "From UserKPICheckDetail as userKPICheckDetail Where userKPICheckDetail.KPICheckID = " + strKPICheckID;
        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst2 = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL2);

        UserKPICheckDetail userKPICheckDetail = new UserKPICheckDetail();

        DataSet ds = new DataSet();

        for (int i = 0; i < lst2.Count; i++)
        {
            userKPICheckDetail = (UserKPICheckDetail)lst2[i];

            deUnitSqlPoint = userKPICheckDetail.UnitSqlPoint;
            strSql1 = userKPICheckDetail.SqlCode.Trim();
            strSql1 = strSql1.Replace("[TAKETOPKPISTARTTIME]", strKPICheckStartDate).Replace("[TAKETOPKPIENDTIME]", strKPICheckEndDate).Replace("[TAKETOPKPIUSERCODE]", strKPIUserCode);

            try
            {
                ds = ShareClass.GetDataSetFromSql(strSql1, "T_SystemPoint");

                deSystemPoint = decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) * deUnitSqlPoint;
            }
            catch
            {
                deSystemPoint = 0;
            }

            intID = userKPICheckDetail.ID;

            userKPICheckDetail.SqlPoint = 100 + deSystemPoint;

            try
            {
                userKPICheckDetailBLL.UpdateUserKPICheckDetail(userKPICheckDetail, intID);
            }
            catch
            {
            }
        }

        if (lst2.Count > 0)
        {
            return UpdateSystemKPICheckPoint(strKPICheckID);
        }
        else
        {
            return 100;
        }
    }

    public static decimal UpdateSystemKPICheckPoint(string strKPICheckID)
    {
        string strHQL;
        string strTotalSqlPoint;

        DataSet ds;

        strHQL = "Select Sum(SqlPoint * Weight) From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheckDetail");
        strTotalSqlPoint = ds.Tables[0].Rows[0][0].ToString();

        if (strTotalSqlPoint == "")
        {
            strTotalSqlPoint = "0";
        }

        strHQL = "Update T_UserKPICheck Set TotalSqlPoint = " + strTotalSqlPoint + " where KPICheckID = " + strKPICheckID;
        ShareClass.RunSqlCommand(strHQL);

        return decimal.Parse(strTotalSqlPoint);
    }

    #endregion Ա����������

    #region ��������ϲ�������

    //ȡ����������
    public static string GetItemType(string strItemCode)
    {
        string strHQL;

        strHQL = "Select Type From T_Item Where ItemCode = '" + strItemCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    public static decimal GetCurrencyTypeExchangeRate(string strCurrencyType)
    {
        decimal flag = 0;
        string strHQL = "Select ExchangeRate From T_CurrencyType where Type='" + strCurrencyType + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_CurrencyType").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = decimal.Parse(dt.Rows[0]["ExchangeRate"].ToString());
        }
        else
        {
            flag = 0;
        }
        return flag;
    }

    //����������
    public static void addOrUpdateGoods(string strCountMethod, string strGoodsID, string strCIOID, string strGoodsCode, string strGoodsName, string strSN, decimal deNumber, string strUnitName,
       string strOwnerCode, string strType, string strSpec, string strModelNumber, string strPosition, string strWHPosition, decimal dePrice, string strIsTaxPrice, string strCurrencyType, DateTime dtBuyTime, int intWarrantyPeriod,
       string strManufacturer, string strMemo, string strCheckInDetailID, string strPhotoURL, decimal deOldCheckInNumber, decimal deOldCheckInPrice,
       string strBatchNumber, DateTime dtProductDate,
       DateTime dtExpiryDate, string strProductionEquipmentNumber, string strMaterialFormNumber)
    {
        string strHQL;

        GoodsBLL goodsBLL = new GoodsBLL();
        Goods goods = new Goods();

        if (strCountMethod == "FIFO")
        {
            strHQL = "Delete From T_Goods Where ID =" + strGoodsID;
            ShareClass.RunSqlCommand(strHQL);

            goods.GoodsCode = strGoodsCode;
            goods.GoodsName = strGoodsName;
            goods.SN = strSN;
            goods.Number = deNumber;
            goods.CheckInNumber = deNumber;
            goods.UnitName = strUnitName;
            goods.OwnerCode = strOwnerCode;
            try
            {
                goods.OwnerName = ShareClass.GetUserName(strOwnerCode);
            }
            catch
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
                return;
            }
            goods.Type = strType;
            goods.Spec = strSpec;
            goods.ModelNumber = strModelNumber;
            goods.Position = strPosition;
            goods.WHPosition = strWHPosition;

            goods.Price = dePrice;
            goods.IsTaxPrice = strIsTaxPrice;
            goods.CurrencyType = strCurrencyType;
            goods.BuyTime = dtBuyTime;
            goods.Manufacturer = strManufacturer;
            goods.Memo = strMemo;
            goods.WarrantyPeriod = intWarrantyPeriod;
            goods.Status = "InUse";

            goods.PhotoURL = strPhotoURL;

            goods.BatchNumber = strBatchNumber;
            goods.ProductDate = dtProductDate;
            goods.ExpiryDate = dtExpiryDate;
            goods.ProductionEquipmentNumber = strProductionEquipmentNumber;
            goods.MaterialFormNumber = strMaterialFormNumber;

            try
            {
                goodsBLL.AddGoods(goods);

                strGoodsID = ShareClass.GetMyCreatedMaxGoodsID().ToString();

                //��¼������ϴ����ID��
                try
                {
                    strHQL = "Update T_GoodsCheckInOrderDetail Set ToGoodsID = " + strGoodsID;
                    strHQL += " Where ID = " + strCheckInDetailID;
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch (Exception err)
                {
                    //Label27.Text = err.Message.ToString();
                }
            }
            catch (Exception err)
            {
                //Label27.Text = err.Message.ToString();

                return;
            }
        }

        if (strCountMethod == "MWAM")
        {
            if (strGoodsID != "0")
            {
                ShareClass.CountGoodsStockByMWAM(strGoodsID, deNumber, dePrice, deOldCheckInNumber, deOldCheckInPrice);
            }
            else
            {
                strGoodsID = ShareClass.CheckSameGoodsExistInStock(strGoodsCode, strType, strModelNumber, strSpec, strManufacturer, strPosition, strWHPosition);
                if (strGoodsID == "")
                {
                    goods.GoodsCode = strGoodsCode;
                    goods.GoodsName = strGoodsName;
                    goods.SN = strSN;
                    goods.Number = deNumber;
                    goods.CheckInNumber = deNumber;
                    goods.UnitName = strUnitName;
                    goods.OwnerCode = strOwnerCode;
                    try
                    {
                        goods.OwnerName = ShareClass.GetUserName(strOwnerCode);
                    }
                    catch
                    {
                        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBGRDMCCCWCRJC").ToString().Trim() + "')", true);
                        return;
                    }
                    goods.Type = strType;
                    goods.Spec = strSpec;
                    goods.ModelNumber = strModelNumber;
                    goods.Position = strPosition;
                    goods.WHPosition = strWHPosition;

                    goods.Price = dePrice;
                    goods.IsTaxPrice = strIsTaxPrice;
                    goods.CurrencyType = strCurrencyType;
                    goods.BuyTime = dtBuyTime;
                    goods.Manufacturer = strManufacturer;
                    goods.Memo = strMemo;
                    goods.WarrantyPeriod = intWarrantyPeriod;
                    goods.Status = "InUse";

                    goods.PhotoURL = strPhotoURL;


                    goods.BatchNumber = strBatchNumber;
                    goods.ProductDate = dtProductDate;
                    goods.ExpiryDate = dtExpiryDate;
                    goods.ProductionEquipmentNumber = strProductionEquipmentNumber;
                    goods.MaterialFormNumber = strMaterialFormNumber;

                    try
                    {
                        goodsBLL.AddGoods(goods);

                        strGoodsID = ShareClass.GetMyCreatedMaxGoodsID().ToString();
                    }
                    catch (Exception err)
                    {
                        return;
                    }
                }
                else
                {
                    ShareClass.CountGoodsStockByMWAM(strGoodsID, deNumber, dePrice, deOldCheckInNumber, deOldCheckInPrice);
                }

                //��¼������ϴ����ID��
                try
                {
                    strHQL = "Update T_GoodsCheckInOrderDetail Set ToGoodsID = " + strGoodsID;
                    strHQL += " Where ID = " + strCheckInDetailID;
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch (Exception err)
                {
                }
            }

            return;
        }

        return;
    }


    //�ж��Ƿ������ͬ�����Ͽ��
    public static string CheckSameGoodsExistInStock(string strGoodsCode, string strType, string strModelNumber, string strSpecification, string strManufacture, string strWareHouse, string strWHPosition)
    {
        string strHQL;

        strHQL = "Select ID From T_Goods Where GoodsCode = '" + strGoodsCode + "' and Type = '" + strType + "' and Spec = '" + strSpecification + "' and ModelNumber = '" + strModelNumber + "' and manufacturer = '" + strManufacture + "' and Position = '" + strWareHouse + "' and WHPosition = '" + strWHPosition + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }


    //��������ͣ��������ҵ���������
    public static void UpdateGoodsRelatedBusinessNubmer(string strRelatedType, string strRelatedID, string strGoodsCode, string strSourceType, string strSourceID, DataGrid DataGrid1)
    {
        string strHQL;

        decimal deCheckinNumber;

        if (strSourceType == "GoodsPORecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where SourceType= 'GoodsPORecord' and SourceID =" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            if (ds.Tables[0].Rows.Count > 0)
            {
                deCheckinNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deCheckinNumber = 0;
            }

            strHQL = "Update T_GoodsPurRecord Set CheckInNumber = " + deCheckinNumber.ToString();
            strHQL += " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strSourceType == "GoodsPDRecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where SourceType= 'GoodsPDRecord' and SourceID =" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            try
            {
                deCheckinNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deCheckinNumber = 0;
            }

            strHQL = "Update T_GoodsProductionOrderDetail Set CheckInNumber = " + deCheckinNumber.ToString();
            strHQL += " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }


        if (strSourceType == "GoodsSURecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where SourceType= 'GoodsSURecord' and SourceID =" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            try
            {
                deCheckinNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deCheckinNumber = 0;
            }

            strHQL = "Select SourceType,SourceID From T_GoodsSupplyOrderDetail Where ID = " + strSourceID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSupplyOrderDetail");

            strSourceType = ds.Tables[0].Rows[0][0].ToString().Trim();
            strSourceID = ds.Tables[0].Rows[0][1].ToString();

            if (strSourceType == "GoodsPORecord")
            {
                strHQL = "Update T_GoodsPurRecord Set CheckInNumber = " + deCheckinNumber.ToString();
                strHQL += " Where ID in (Select SourceID From T_GoodsSupplyOrderDetail Where SourceType = 'GoodsPORecord' and SourceID = " + strSourceID + ")";
                ShareClass.RunSqlCommand(strHQL);
            }
        }

        if (strSourceType == "GoodsCSRecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where SourceType= 'GoodsCSRecord' and SourceID =" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            try
            {
                deCheckinNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deCheckinNumber = 0;
            }

            strHQL = "Update T_ConstractRelatedGoods Set PurchaseOrderNumber = " + deCheckinNumber.ToString();
            strHQL += " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }

        //������Ŀ���������µ���
        if (strSourceType == "GoodsPJRecord")
        {
            UpdatProjectRelatedItemNumber(strSourceType, strSourceID);
        }

        //����������������͸�����Ŀ����Ԥ������ϴ����Ԥ��ʹ����
        if (strRelatedType == "Project")
        {
            UpdateProjectRelatedItemNumberByBudgetBusinessType("CHECKIN", strRelatedType, strRelatedID, strGoodsCode);
            RefreshProjectRelatedItemNumber(strRelatedID, DataGrid1);
        }
    }


    //����������������͸�����Ŀ����Ԥ������ϴ����Ԥ��ʹ����
    public static void UpdateProjectRelatedItemNumberByBudgetBusinessType(string strBusinessType, string strRelatedType, string strRelatedID, string strGoodsCode)
    {
        string strHQL;
        decimal deSumNumber;

        if (strBusinessType == "PURCHASE")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsPurRecord Where POID in (Select POID From T_GoodsPurchaseOrder Where RelatedType = '" + strRelatedType + "' And RelatedID=" + strRelatedID + ")";
            strHQL += " and GoodsCode = '" + strGoodsCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");
            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadyPurchased = " + deSumNumber.ToString() + " Where ProjectID = " + strRelatedID + " and ItemCode = '" + strGoodsCode + "'";
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strBusinessType == "CHECKIN")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where CheckInID in (Select CheckInID From T_GoodsCheckInOrder Where RelatedType = '" + strRelatedType + "' And RelatedID=" + strRelatedID + ")";
            strHQL += " and GoodsCode = '" + strGoodsCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");
            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadyCheckIn = " + deSumNumber.ToString() + " Where ProjectID = " + strRelatedID + " and ItemCode = '" + strGoodsCode + "'";
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strBusinessType == "PICK")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsApplicationDetail Where AAID in (Select AAID From T_GoodsApplication Where RelatedType = '" + strRelatedType + "' And RelatedID=" + strRelatedID + ")";
            strHQL += " and GoodsCode = '" + strGoodsCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsApplicationDetail");
            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadyPick = " + deSumNumber.ToString() + " Where ProjectID = " + strRelatedID + " and ItemCode = '" + strGoodsCode + "'";
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strBusinessType == "CHECKOUT")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsShipmentDetail Where ShipmentNO in (Select ShipmentNO From T_GoodsShipmentOrder Where RelatedType = '" + strRelatedType + "' And RelatedID=" + strRelatedID + ")";
            strHQL += " and GoodsCode = '" + strGoodsCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");
            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadyCheckOut = " + deSumNumber.ToString() + " Where ProjectID = " + strRelatedID + " and ItemCode = '" + strGoodsCode + "'";
            ShareClass.RunSqlCommand(strHQL);
        }

        if (strBusinessType == "PRODUCTION")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsProductionOrderDetail Where PDID in (Select PDID From T_GoodsProductionOrder Where RelatedType = '" + strRelatedType + "' And RelatedID=" + strRelatedID + ")";
            strHQL += " and GoodsCode = '" + strGoodsCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");
            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadyProduction = " + deSumNumber.ToString() + " Where ProjectID = " + strRelatedID + " and ItemCode = '" + strGoodsCode + "'";
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    public static void UpdatProjectRelatedItemNumber(string strSourceType, string strSourceID)
    {
        string strHQL;
        decimal deSumNumber;

        if (strSourceType == "GoodsPJRecord")
        {
            strHQL = "Select COALESCE(Sum(Number),0) From T_GoodsCheckInOrderDetail Where SourceType = 'GoodsPJRecord' And SourceID=" + strSourceID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurRecord");

            try
            {
                deSumNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                deSumNumber = 0;
            }

            strHQL = "Update T_ProjectRelatedItem Set AleadyCheckIn = " + deSumNumber.ToString() + " Where ID = " + strSourceID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    //�ж��������Ƿ����Ԥ��������������Ŀ����Ԥ��
    public static bool checkRequireNumberIsMoreHaveNumberForProjectRelatedItemNumber(string strProjectRelatedItemID, string strAleadyNumberColumnName, decimal deNumber)
    {
        string strHQL;
        decimal deRequireNumber, deFinishedNumber;

        strHQL = "Select Number, " + strAleadyNumberColumnName + " From T_ProjectRelatedItem Where ID = " + strProjectRelatedItemID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectRelatedItem");
        if (ds.Tables[0].Rows.Count > 0)
        {
            deRequireNumber = decimal.Parse(ds.Tables[0].Rows[0]["Number"].ToString());
            deFinishedNumber = decimal.Parse(ds.Tables[0].Rows[0][strAleadyNumberColumnName].ToString());
        }
        else
        {
            deRequireNumber = 0;
            deFinishedNumber = 0;
        }

        if (deNumber > (deRequireNumber - deFinishedNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static void RefreshProjectRelatedItemNumber(string strProjectID, DataGrid DataGrid1)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectRelatedItem as projectRelatedItem where projectRelatedItem.ProjectID = " + strProjectID + " Order by projectRelatedItem.ID ASC";
        ProjectRelatedItemBLL projectRelatedItemBLL = new ProjectRelatedItemBLL();
        lst = projectRelatedItemBLL.GetAllProjectRelatedItems(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    //��Ȩƽ����������
    public static void CountGoodsStockByMWAM(string strGoodsID, decimal deCheckInNumber, decimal deCheckInPrice, decimal deOldCheckInNumber, decimal deOldCheckInPrice)
    {
        string strHQL;

        if (deOldCheckInNumber == 0)
        {
            strHQL = "Update T_Goods Set Number = " + "Number + " + deCheckInNumber.ToString() + ",Price = " + " (Number * Price + " + (deCheckInNumber * deCheckInPrice).ToString() + ")/(" + "Number + " + deCheckInNumber.ToString() + ") From T_Goods";
            strHQL += " Where ID =" + strGoodsID;
        }
        else if (deOldCheckInNumber > 0)
        {
            strHQL = "Update T_Goods Set Number = " + "Number - " + deOldCheckInNumber.ToString() + "+ " + deCheckInNumber.ToString() + ",Price = " + " (Number * Price - " + (deOldCheckInNumber * deOldCheckInPrice).ToString() + "+" + (deCheckInNumber * deCheckInPrice).ToString() + ")/(" + "Number - " + deOldCheckInNumber + "+ " + deCheckInNumber.ToString() + ") From T_Goods";
            strHQL += " Where ID =" + strGoodsID;
        }
        else
        {
            strHQL = "";
        }

        try
        {
            //Label27.Text = strHQL;

            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }
    }

    //ȡ�����Ͽ������㷨
    public static string GetGoodsStockCountMethod(string strWHName)
    {
        string strHQL;

        strHQL = "Select CapitalMethod From T_WareHouse Where WHName = '" + strWHName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse ");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "FIFO";
        }
    }

    public static void UpdateGoodsNumberForAdd(string strFromGoodsID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number - " + deNumber.ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    public static void UpdateGoodsNumberForUpdate(string strFromGoodsID, decimal deNumber, decimal deOldNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number - " + (deNumber - deOldNumber).ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    public static void UpdateGoodsNumberForDelete(string strFromGoodsID, decimal deNumber)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Number = Number + " + deNumber.ToString() + " Where ID = " + strFromGoodsID;
        ShareClass.RunSqlCommand(strHQL);
    }

    /// <summary>
    /// Liujp 2013-07-17 �������ϵǼ�����ʱ���������ϱ��вֿ��ֶ�
    /// </summary>
    /// <param name="goodsCheckInOrderId"></param>
    /// <param name="strPosition"></param>
    public static void UpdateGoodsPositionByGoodsCheckInOrder(string strGoodsCheckInOrderId, string strPosition)
    {
        string strHQL;

        strHQL = "Update T_Goods Set Position = '" + strPosition + "' Where ID in (Select ToGoodsID From T_GoodsCheckInOrderDetail Where CheckInID = " + strGoodsCheckInOrderId + ")";

        ShareClass.RunSqlCommand(strHQL);
    }

    //��ȡ��������
    public static string GetHomeCurrencyType()
    {
        string strHQL;

        strHQL = "Select Type From T_CurrencyType Where IsHome = 'YES'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CurrencyType");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return LanguageHandle.GetWord("RenMinBi").ToString().Trim();
        }
    }

    /// <summary>
    /// �ж��Ƿ������ñ�׼���
    /// </summary>
    /// <param name="strID"></param>
    /// <param name="strDepartCode"></param>
    /// <param name="strAccountName"></param>
    /// <param name="strYearNum"></param>
    /// <param name="strMonthNum"></param>
    /// <returns></returns>
    public static bool IsBMBaseDataExits(string strID, string strDepartCode, string strAccountName, int strYearNum, int strMonthNum, string strusercode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.EnterCode='" + strusercode + "' and bDBaseData.Type='Base' ";
        }
        else
        {
            strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.EnterCode='" + strusercode + "' and bDBaseData.Type='Base' and bDBaseData.ID<>'" + strID + "' ";
        }
        BDBaseDataBLL bdBaseDataRecordBLL = new BDBaseDataBLL();
        IList lst = bdBaseDataRecordBLL.GetAllBDBaseDatas(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;

        return flag;
    }

    /// <summary>
    /// ȡ��Ԥ�����
    /// </summary>
    /// <param name="strID"></param>
    /// <param name="strDepartCode"></param>
    /// <param name="strAccountName"></param>
    /// <param name="strYearNum"></param>
    /// <param name="strMonthNum"></param>
    /// <returns></returns>
    public static decimal GetBMBaseDataMoneyNum(string strDepartCode, string strAccountName, int strYearNum, int strMonthNum, string strType)
    {
        decimal deBalance = 0;
        decimal deMoneyBase = 0;
        decimal deMoneyUsed = 0;
        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.Type='" + strType + "' ";
        BDBaseDataBLL bdBaseDataBLL = new BDBaseDataBLL();
        IList lst = bdBaseDataBLL.GetAllBDBaseDatas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                BDBaseData bdBaseData = (BDBaseData)lst[i];
                deMoneyBase += bdBaseData.MoneyNum;
            }
        }

        BDBaseDataRecordBLL bdBaseDataRecordBLL = new BDBaseDataRecordBLL();
        strHQL = "From BDBaseDataRecord as bdBaseDataRecord where bdBaseDataRecord.DepartCode = '" + strDepartCode + "' and bdBaseDataRecord.AccountName='" + strAccountName + "' and " +
                "bdBaseDataRecord.YearNum='" + strYearNum.ToString() + "' and bdBaseDataRecord.MonthNum = '" + strMonthNum.ToString() + "' and bdBaseDataRecord.Type='Operation' ";
        lst = bdBaseDataRecordBLL.GetAllBDBaseDataRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                BDBaseDataRecord bdBaseDataRecord = (BDBaseDataRecord)lst[j];
                deMoneyUsed += bdBaseDataRecord.MoneyNum;
            }
        }

        deBalance = deMoneyBase - deMoneyUsed;

        return deBalance;
    }

    //ȡ�ò���Ԥ���¼ID
    public static int GetBMBaseDataID(string strDepartCode, string strAccountCode, string strAccountName, int strYearNum, int strMonthNum, string strType)
    {
        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountCode = '" + strAccountCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.Type='" + strType + "' ";
        BDBaseDataBLL bdBaseDataBLL = new BDBaseDataBLL();
        IList lst = bdBaseDataBLL.GetAllBDBaseDatas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BDBaseData bdBaseData = (BDBaseData)lst[0];

            return bdBaseData.ID;
        }
        else
        {
            return 0;
        }
    }

    //�ѱ�����������Ԥ�����
    public static void AddClaimExpenseToBudget(string strAccountCode, string strAccountName, int intBDBaseDataID, string strUserCode, decimal deAmount, int intYear, int intMonth)
    {
        string strDepartCode, strDepartName;
        int intBMBaseDataID = 0;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        intBMBaseDataID = ShareClass.GetBMBaseDataID(strDepartCode, strAccountCode, strAccountName, intYear, intMonth, "Base");
        if (intBMBaseDataID <= 0)
        {
            return;
        }

        BDBaseDataRecordBLL bdBaseDataRecordBLL = new BDBaseDataRecordBLL();
        BDBaseDataRecord bdBaseDataRecord = new BDBaseDataRecord();

        bdBaseDataRecord.AccountCode = strAccountCode;
        bdBaseDataRecord.AccountName = strAccountName;
        bdBaseDataRecord.BDBaseDataID = intBDBaseDataID;
        bdBaseDataRecord.DepartCode = strDepartCode;
        bdBaseDataRecord.DepartName = strDepartName;
        bdBaseDataRecord.EnterCode = strUserCode.Trim();
        bdBaseDataRecord.MoneyNum = deAmount;
        bdBaseDataRecord.YearNum = intYear;
        bdBaseDataRecord.MonthNum = intMonth;
        bdBaseDataRecord.Type = "Operation";

        try
        {
            bdBaseDataRecordBLL.AddBDBaseDataRecord(bdBaseDataRecord);
        }
        catch
        {
        }
    }

    //������ȡ��������״̬
    public static string GetCodeRuleStatusByType(string strCodeType)
    {
        string strHQL;

        strHQL = "Select IsStartup From T_CodeRule  Where CodeType = " + "'" + strCodeType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CodeRule");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "NO";
        }
    }

    //���������ȡ����ش���
    public static string GetCodeByRule(string strCodeType, string strObjectType, string strID)
    {
        string strHQL;

        string strHeadchar, strFieldName, strIsStartup, strKeyWord;
        int intFlowIDWidth, intLength;
        string strFlowID, strFlowCode, strCode;

        strFlowID = "";
        strKeyWord = "";

        DataSet ds;

        if (strCodeType == "ProjectCode")
        {
            strHQL = "Select KeyWord From T_ProjectType Where Type = " + "'" + strObjectType + "'";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectType");
            if (ds.Tables[0].Rows.Count > 0)
            {
                strKeyWord = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                strKeyWord = "";
            }
        }

        if (strCodeType == "ConstractCode")
        {
            strHQL = "Select KeyWord From T_ConstractType Where Type = " + "'" + strObjectType + "'";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractType");

            if (ds.Tables[0].Rows.Count > 0)
            {
                strKeyWord = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                strKeyWord = "";
            }
        }

        strHQL = "Select HeadChar,FieldName,FlowIDWidth,IsStartup From T_CodeRule  Where CodeType = " + "'" + strCodeType + "'";
        strHQL += " And IsStartup = 'YES'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_CodeRule");

        if (ds.Tables[0].Rows.Count > 0)
        {
            strHeadchar = ds.Tables[0].Rows[0][0].ToString().Trim();
            strFieldName = ds.Tables[0].Rows[0][1].ToString().Trim();

            intFlowIDWidth = int.Parse(ds.Tables[0].Rows[0][2].ToString().Trim());
            strIsStartup = ds.Tables[0].Rows[0][3].ToString().Trim();

            intLength = intFlowIDWidth - strID.Length;
            if (intLength > 0)
            {
                for (int i = 1; i <= intLength; i++)
                {
                    strFlowID += "0";
                }

                strFlowCode = strFlowID + strID;
            }
            else
            {
                strFlowCode = strID;
            }

            if (strFieldName == "[TAKETOPYEARMONTHDAY]")
            {
                strCode = strHeadchar + strKeyWord + DateTime.Now.ToString("yyyyMMdd") + strFlowCode;
            }
            else
            {
                if (strFieldName == "[TAKETOPYEARMONTH]")
                {
                    strCode = strHeadchar + strKeyWord + DateTime.Now.ToString("yyyyMM") + strFlowCode;
                }
                else
                {
                    strCode = strHeadchar + strKeyWord + strFlowID;
                }
            }

            return strCode;
        }
        else
        {
            return "";
        }
    }

    //���ɲֿ���������Ȩ�޺Ͳ����ʲ�����Ա��
    public static void InitialWarehouseTreeByAuthorityAsset(TreeView TreeView, String strUserCode, string strDepartString)
    {
        string strHQL, strWareHouse;
        IList lst;

        //��Ӹ��ڵ�
        TreeView.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>�ֿ��б�</B>";  
        node1.Target = "1";
        node1.Expanded = true;
        TreeView.Nodes.Add(node1);

        strHQL = "from WareHouse as wareHouse where wareHouse.BelongDepartCode in " + strDepartString;
        strHQL += " and wareHouse.ParentWH  = '1' Or COALESCE(wareHouse.ParentWH,'') = ''";
        strHQL += " order by wareHouse.SortNumber ASC";
        WareHouseBLL WareHouseBLL = new WareHouseBLL();
        WareHouse WareHouse = new WareHouse();

        lst = WareHouseBLL.GetAllWareHouses(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            WareHouse = (WareHouse)lst[i];

            strWareHouse = WareHouse.WHName.Trim();

            node3 = new TreeNode();

            node3.Text = strWareHouse;
            node3.Target = strWareHouse;
            node3.Expanded = true;

            node1.ChildNodes.Add(node3);
            WareHouseTreeShowByAuthority(strWareHouse, node3);
            TreeView.DataBind();
        }
    }

    public static void WareHouseTreeShowByAuthority(string strParentWH, TreeNode treeNode)
    {
        string strHQL, strWareHouse;
        IList lst1, lst2;

        WareHouseBLL WareHouseBLL = new WareHouseBLL();
        WareHouse WareHouse = new WareHouse();

        strHQL = "from WareHouse as wareHouse where wareHouse.ParentWH = '" + strParentWH + "'";
        strHQL += " order by wareHouse.SortNumber ASC";
        lst1 = WareHouseBLL.GetAllWareHouses(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            WareHouse = (WareHouse)lst1[i];

            strWareHouse = WareHouse.WHName.Trim();

            TreeNode node = new TreeNode();

            node.Target = strWareHouse;
            node.Text = strWareHouse;
            treeNode.ChildNodes.Add(node);
            node.Expanded = true;

            strHQL = "from WareHouse as wareHouse where wareHouse.ParentWH = '" + strWareHouse + "'";
            strHQL += " Order by wareHouse.SortNumber ASC";
            lst2 = WareHouseBLL.GetAllWareHouses(strHQL);

            if (lst2.Count > 0)
            {
                WareHouseTreeShowByAuthority(strWareHouse, node);
            }
        }
    }

    //ȡ��Ȩ���ڲֿ��б�
    public static void LoadWareHouseListByAuthorityForDropDownList(string strUserCode, DropDownList DL_WareHouse)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = GetDepartCodeFromUserCode(strUserCode);
        strDepartString = CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = " Select WHName From T_WareHouse Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By SortNumber DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHouse");

        DL_WareHouse.DataSource = ds;
        DL_WareHouse.DataBind();

        DL_WareHouse.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //ȡ�òֿ��λ�б�
    public static void LoadWareHousePositions(string strWHName, DropDownList DL_WHPosition)
    {
        string strHQL;

        strHQL = "Select * From T_WareHousePositions Where WHName = " + "'" + strWHName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WareHousePositions");

        DL_WHPosition.DataSource = ds;
        DL_WHPosition.DataBind();

        DL_WHPosition.Items.Insert(0, new ListItem("--Select--", ""));
    }

    public static string GetAccountName(string strAccountCode)
    {
        string flag = "";
        string strHQL = "Select AccountName From T_Account where AccountCode='" + strAccountCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_Account").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["AccountName"].ToString();
        }
        else
        {
            flag = "";
        }
        return flag;
    }

    public static string GetAccountCode(string strAccountName)
    {
        string flag = "";
        string strHQL = "Select AccountCode From T_Account where AccountName='" + strAccountName + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_Account").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["AccountCode"].ToString();
        }
        else
        {
            flag = "";
        }
        return flag;
    }

    public static void LoadAccountForDDL(DropDownList DL_Account)
    {
        DataTable dt = GetAccountList(string.Empty);
        if (dt != null && dt.Rows.Count > 0)
        {
            DL_Account.Items.Clear();
            DL_Account.Items.Insert(0, new ListItem("--Select--", ""));
            SetInterval(DL_Account, "0", " ");
        }
        else
        {
            DL_Account.Items.Clear();
            DL_Account.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }

    public static DataTable GetAccountList(string strParentID)
    {
        string strHQL = "Select * From T_Account ";
        if (!string.IsNullOrEmpty(strParentID))
        {
            strHQL += " Where ParentID='" + strParentID.Trim() + "' ";
        }
        strHQL += " Order By SortNumber ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Account");
        return ds.Tables[0];
    }

    public static void SetInterval(DropDownList DDL, string strParentID, string interval)
    {
        interval += "��";

        DataTable list = GetAccountList(strParentID);
        if (list.Rows.Count > 0 && list != null)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                DDL.Items.Add(new ListItem(string.Format("{0}{1}", interval, list.Rows[i]["AccountType"].ToString().Trim() + "-" + list.Rows[i]["AccountName"].ToString().Trim()), list.Rows[i]["AccountCode"].ToString().Trim()));

                ///�ݹ�
                SetInterval(DDL, list.Rows[i]["ID"].ToString().Trim(), interval);
            }
        }
    }

    public static void LoadCurrencyType(DropDownList DL_CurrencyType)
    {
        string strHQL;
        IList lst;

        strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        DL_CurrencyType.DataSource = lst;
        DL_CurrencyType.DataBind();
    }

    public static string GetCustomerNameFromGoodsSaleOrder(string strSOID)
    {
        string strHQL;
        string strCustomerName;
        strHQL = "Select CustomerName From T_GoodsSaleOrder Where SOID = " + strSOID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsSaleOrder");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strCustomerName = ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            strCustomerName = "";
        }

        return strCustomerName;
    }

    public static string GetCustomerNameFromCustomerCode(string strCustomerCode)
    {
        string strHQL;
        string strCustomerName;
        strHQL = "Select CustomerName From T_Customer Where CustomerCode = " + "'" + strCustomerCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Customer");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strCustomerName = ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            strCustomerName = "";
        }

        return strCustomerName;
    }

    public static string GetApplicantNameFromGoodsApplicaitonOrder(string strAOID)
    {
        string strHQL;
        string strApplicantName;
        strHQL = "Select ApplicantName From T_GoodsApplication Where AAID = " + strAOID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsApplication");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strApplicantName = ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            strApplicantName = "";
        }

        return strApplicantName;
    }

    public static bool IsBusinessFormRelatedConstract(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        DataSet ds = new DataSet();

        if (strRelatedType == "GoodsSO")
        {
            strHQL = "Select ConstractCode From T_ConstractRelatedGoodsSaleOrder Where SOID =" + strRelatedID;
            ds = GetDataSetFromSql(strHQL, "T_ConstractRelated");
        }

        if (strRelatedType == "GoodsPO")
        {
            strHQL = "Select ConstractCode From T_ConstractRelatedGoodsPurchaseOrder Where POID = " + strRelatedID;
            ds = GetDataSetFromSql(strHQL, "T_ConstractRelated");
        }

        if (strRelatedType == "AssetPO")
        {
            strHQL = "Select ConstractCode From T_ConstractRelatedAssetPurchaseOrder Where  Where POID = " + strRelatedID;
            ds = GetDataSetFromSql(strHQL, "T_ConstractRelated");
        }

        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public static int InsertReceivablesOrPayable(string strFormCode, string strRelatedType, string strRelatedID, string strRelatedRecordID, Decimal deAmount, string strCurrencyType, string strReOrPayer, string strOperatorCode, int intRelatedProjectID)
    {
        string strHQL;
        IList lst;

        string strReOrPay, strRelatedAccount, strRelatedAccountCode, strReceivableID, strPayableID;

        //���ҵ�񵥹����˺�ͬ���Ͳ���Ӧ����Ӧ��
        if (IsBusinessFormRelatedConstract(strRelatedType, strRelatedID))
        {
            return 0;
        }

        strHQL = "From BusinessFormReAndPay as businessFormReAndPay Where FormCode = " + "'" + strFormCode + "'";
        BusinessFormReAndPayBLL businessFormReAndPayBLL = new BusinessFormReAndPayBLL();
        lst = businessFormReAndPayBLL.GetAllBusinessFormReAndPays(strHQL);
        if (lst.Count > 0)
        {
            BusinessFormReAndPay businessFormReAndPay = (BusinessFormReAndPay)lst[0];

            strReOrPay = businessFormReAndPay.ReceiveOrPay.Trim();

            if (strReOrPay == "Receivables")
            {
                strRelatedAccountCode = businessFormReAndPay.RelatedAccountCode.Trim();
                strRelatedAccount = businessFormReAndPay.RelatedAccount.Trim();

                ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
                ConstractReceivables constractReceivables = new ConstractReceivables();

                constractReceivables.ConstractCode = "";
                constractReceivables.BillCode = "";

                constractReceivables.ReceivablesAccount = deAmount;
                constractReceivables.ReceivablesTime = DateTime.Now;

                constractReceivables.AccountCode = strRelatedAccountCode;
                constractReceivables.Account = strRelatedAccount;

                constractReceivables.ReceiverAccount = 0;
                constractReceivables.ReceiverTime = DateTime.Now;

                constractReceivables.InvoiceAccount = 0;
                constractReceivables.UNReceiveAmount = deAmount;
                constractReceivables.CurrencyType = strCurrencyType;

                constractReceivables.Payer = strReOrPayer;

                constractReceivables.OperatorCode = strOperatorCode;
                constractReceivables.OperatorName = GetUserName(strOperatorCode);
                constractReceivables.OperateTime = DateTime.Now;
                constractReceivables.PreDays = 5;
                constractReceivables.Status = "Plan";
                constractReceivables.Comment = "";

                constractReceivables.RelatedType = strRelatedType;
                constractReceivables.RelatedID = int.Parse(strRelatedID);
                constractReceivables.RelatedRecordID = int.Parse(strRelatedRecordID);

                constractReceivables.RelatedProjectID = intRelatedProjectID;

                try
                {
                    constractReceivablesBLL.AddConstractReceivables(constractReceivables);

                    strReceivableID = ShareClass.GetMyCreatedMaxConstractReceivableID("");

                    return int.Parse(strReceivableID);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                if (strReOrPay == "Payables")
                {
                    strRelatedAccountCode = businessFormReAndPay.RelatedAccountCode.Trim();
                    strRelatedAccount = businessFormReAndPay.RelatedAccount.Trim();

                    ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
                    ConstractPayable constractPayable = new ConstractPayable();

                    constractPayable.ConstractCode = "";
                    constractPayable.BillCode = "";

                    constractPayable.PayableAccount = deAmount;
                    constractPayable.PayableTime = DateTime.Now;

                    constractPayable.AccountCode = strRelatedAccountCode;
                    constractPayable.Account = strRelatedAccount;

                    constractPayable.OutOfPocketAccount = 0;
                    constractPayable.OutOfPocketTime = DateTime.Now;

                    constractPayable.InvoiceAccount = 0;
                    constractPayable.UNPayAmount = deAmount;
                    constractPayable.CurrencyType = strCurrencyType;

                    constractPayable.Receiver = strReOrPayer;

                    constractPayable.OperatorCode = strOperatorCode;
                    constractPayable.OperatorName = GetUserName(strOperatorCode);
                    constractPayable.OperateTime = DateTime.Now;
                    constractPayable.PreDays = 5;
                    constractPayable.Status = "Plan";
                    constractPayable.Comment = "";

                    constractPayable.RelatedType = strRelatedType;
                    constractPayable.RelatedID = int.Parse(strRelatedID);
                    constractPayable.RelatedRecordID = int.Parse(strRelatedRecordID);

                    constractPayable.RelatedProjectID = intRelatedProjectID;

                    try
                    {
                        constractPayableBLL.AddConstractPayable(constractPayable);

                        strPayableID = ShareClass.GetMyCreatedMaxConstractPayableID("");

                        return int.Parse(strPayableID);
                    }
                    catch
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
        else
        {
            return 0;
        }
    }

    public static void UpdateReceivablesOrPayable(string strFormCode, string strRelatedType, string strRelatedID, string strRelatedRecordID, Decimal deAmount, string strCurrencyType, string strReOrPayer, string strOperatorCode)
    {
        string strHQL;
        IList lst;

        string strReOrPay, strRelatedAccount, strRelatedAccountCode;
        int intID;

        strHQL = "From BusinessFormReAndPay as businessFormReAndPay Where FormCode = " + "'" + strFormCode + "'";

        BusinessFormReAndPayBLL businessFormReAndPayBLL = new BusinessFormReAndPayBLL();
        lst = businessFormReAndPayBLL.GetAllBusinessFormReAndPays(strHQL);
        if (lst.Count > 0)
        {
            BusinessFormReAndPay businessFormReAndPay = (BusinessFormReAndPay)lst[0];

            strReOrPay = businessFormReAndPay.ReceiveOrPay.Trim();

            if (strReOrPay == "Receivables")
            {
                strRelatedAccountCode = businessFormReAndPay.RelatedAccountCode.Trim();
                strRelatedAccount = businessFormReAndPay.RelatedAccount.Trim();

                strHQL = "From ConstractReceivables as constractReceivables Where constractReceivables.RelatedType = " + "'" + strFormCode + "'" + " and constractReceivables.RelatedRecordID = " + strRelatedRecordID;
                ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
                lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

                if (lst.Count > 0)
                {
                    ConstractReceivables constractReceivables = (ConstractReceivables)lst[0];

                    intID = constractReceivables.ID;

                    constractReceivables.ConstractCode = "";
                    constractReceivables.BillCode = "";

                    constractReceivables.ReceivablesAccount = deAmount;
                    constractReceivables.ReceivablesTime = DateTime.Now;
                    constractReceivables.AccountCode = strRelatedAccountCode;
                    constractReceivables.Account = strRelatedAccount;
                    constractReceivables.UNReceiveAmount = deAmount - constractReceivables.ReceiverAccount;

                    constractReceivables.CurrencyType = strCurrencyType;

                    constractReceivables.Payer = strReOrPayer;

                    constractReceivables.OperatorCode = strOperatorCode;
                    constractReceivables.OperatorName = GetUserName(strOperatorCode);
                    constractReceivables.OperateTime = DateTime.Now;
                    constractReceivables.PreDays = 5;

                    constractReceivables.RelatedRecordID = int.Parse(strRelatedRecordID);

                    try
                    {
                        constractReceivablesBLL.UpdateConstractReceivables(constractReceivables, intID);
                    }
                    catch
                    {
                    }
                }
            }

            if (strReOrPay == "Payables")
            {
                strRelatedAccountCode = businessFormReAndPay.RelatedAccountCode.Trim();
                strRelatedAccount = businessFormReAndPay.RelatedAccount.Trim();

                strHQL = "From ConstractPayable as constractPayable Where constractPayable.RelatedType = " + "'" + strFormCode + "'" + " and constractPayable.RelatedRecordID = " + strRelatedRecordID;
                ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
                lst = constractPayableBLL.GetAllConstractPayables(strHQL);

                if (lst.Count > 0)
                {
                    ConstractPayable constractPayable = (ConstractPayable)lst[0];

                    intID = constractPayable.ID;
                    constractPayable.ConstractCode = "";
                    constractPayable.BillCode = "";

                    constractPayable.PayableAccount = deAmount;
                    constractPayable.PayableTime = DateTime.Now;
                    constractPayable.AccountCode = strRelatedAccountCode;
                    constractPayable.Account = strRelatedAccount;
                    constractPayable.UNPayAmount = deAmount - constractPayable.OutOfPocketAccount;

                    constractPayable.CurrencyType = strCurrencyType;

                    constractPayable.Receiver = strReOrPayer;

                    constractPayable.OperatorCode = strOperatorCode;
                    constractPayable.OperatorName = GetUserName(strOperatorCode);
                    constractPayable.OperateTime = DateTime.Now;

                    constractPayable.RelatedRecordID = int.Parse(strRelatedRecordID);

                    try
                    {
                        constractPayableBLL.UpdateConstractPayable(constractPayable, intID);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    public static void DeleteReceivablesOrPayable(string strFormCode, string strRelatedType, string strRelatedRecordID)
    {
        string strHQL;
        IList lst;

        string strReOrPay;

        strHQL = "From BusinessFormReAndPay as businessFormReAndPay Where businessFormReAndPay.FormCode = " + "'" + strFormCode + "'";
        BusinessFormReAndPayBLL businessFormReAndPayBLL = new BusinessFormReAndPayBLL();
        lst = businessFormReAndPayBLL.GetAllBusinessFormReAndPays(strHQL);
        if (lst.Count > 0)
        {
            BusinessFormReAndPay businessFormReAndPay = (BusinessFormReAndPay)lst[0];

            strReOrPay = businessFormReAndPay.ReceiveOrPay.Trim();

            if (strReOrPay == "Receivables")
            {
                strHQL = "Delete From T_ConstractReceivables Where RelatedType = " + "'" + strRelatedType + "'" + " and RelatedRecordID = " + strRelatedRecordID;
                ShareClass.RunSqlCommand(strHQL);
            }

            if (strReOrPay == "Payables")
            {
                strHQL = "Delete From T_ConstractPayable Where RelatedType = " + "'" + strRelatedType + "'" + " and RelatedRecordID = " + strRelatedRecordID;
                ShareClass.RunSqlCommand(strHQL);
            }
        }
    }

    public static int InsertReceivablesOrPayableByAccount(string strReOrPay, string strFormCode, string strRelatedType, string strRelatedID, string strRelatedRecordID, string strAccountCode, string strAccount, Decimal deAmount, string strCurrencyType, string strReOrPayer, string strOperatorCode, int intRelatedProjectID)
    {
        string strReceivableID, strPayableID;

        if (strReOrPay == "Receivables")
        {
            ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
            ConstractReceivables constractReceivables = new ConstractReceivables();

            constractReceivables.ConstractCode = "";
            constractReceivables.BillCode = "";

            constractReceivables.ReceivablesAccount = deAmount;
            constractReceivables.ReceivablesTime = DateTime.Now;

            constractReceivables.AccountCode = strAccountCode;
            constractReceivables.Account = strAccount;

            constractReceivables.ReceiverAccount = 0;
            constractReceivables.ReceiverTime = DateTime.Now;

            constractReceivables.InvoiceAccount = 0;
            constractReceivables.UNReceiveAmount = deAmount;
            constractReceivables.CurrencyType = strCurrencyType;

            constractReceivables.Payer = strReOrPayer;

            constractReceivables.OperatorCode = strOperatorCode;
            constractReceivables.OperatorName = GetUserName(strOperatorCode);
            constractReceivables.OperateTime = DateTime.Now;
            constractReceivables.PreDays = 5;
            constractReceivables.Status = "Completed";
            constractReceivables.Comment = "";

            constractReceivables.RelatedType = strRelatedType;
            constractReceivables.RelatedID = int.Parse(strRelatedID);
            constractReceivables.RelatedRecordID = int.Parse(strRelatedRecordID);

            constractReceivables.RelatedProjectID = intRelatedProjectID;

            try
            {
                constractReceivablesBLL.AddConstractReceivables(constractReceivables);

                strReceivableID = ShareClass.GetMyCreatedMaxConstractReceivableID("");

                return int.Parse(strReceivableID);
            }
            catch
            {
                return 0;
            }
        }
        else
        {
            if (strReOrPay == "Payables")
            {
                ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
                ConstractPayable constractPayable = new ConstractPayable();

                constractPayable.ConstractCode = "";
                constractPayable.BillCode = "";

                constractPayable.PayableAccount = deAmount;
                constractPayable.PayableTime = DateTime.Now;

                constractPayable.AccountCode = strAccountCode;
                constractPayable.Account = strAccount;

                constractPayable.OutOfPocketAccount = 0;
                constractPayable.OutOfPocketTime = DateTime.Now;

                constractPayable.InvoiceAccount = 0;
                constractPayable.UNPayAmount = deAmount;
                constractPayable.CurrencyType = strCurrencyType;

                constractPayable.Receiver = strReOrPayer;

                constractPayable.OperatorCode = strOperatorCode;
                constractPayable.OperatorName = GetUserName(strOperatorCode);
                constractPayable.OperateTime = DateTime.Now;
                constractPayable.PreDays = 5;
                constractPayable.Status = "Completed";
                constractPayable.Comment = "";

                constractPayable.RelatedType = strRelatedType;
                constractPayable.RelatedID = int.Parse(strRelatedID);
                constractPayable.RelatedRecordID = int.Parse(strRelatedRecordID);

                constractPayable.RelatedProjectID = intRelatedProjectID;

                try
                {
                    constractPayableBLL.AddConstractPayable(constractPayable);

                    strPayableID = ShareClass.GetMyCreatedMaxConstractPayableID("");

                    return int.Parse(strPayableID);
                }
                catch
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

    //�����ո����¼
    public static void InsertReceivablesOrPayableRecord(string strReOrPay, int intRelatedID, Decimal deAmount, string strCurrencyType, string strReOrPayerType, string strReOrPayer, string strOperatorCode, int intRelatedProjectID)
    {
        if (strReOrPay == "Receivables")
        {
            ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
            ConstractReceivablesRecord constractReceivablesRecord = new ConstractReceivablesRecord();

            constractReceivablesRecord.ReceivablesID = intRelatedID;
            constractReceivablesRecord.ConstractCode = "";

            constractReceivablesRecord.ReAndPayType = strReOrPayerType;
            constractReceivablesRecord.Currency = strCurrencyType; ;
            constractReceivablesRecord.Bank = "";
            constractReceivablesRecord.ExchangeRate = GetExchangeRateByCurrencyType(strCurrencyType);

            constractReceivablesRecord.ReceiverAccount = deAmount;
            constractReceivablesRecord.ReceiverTime = DateTime.Now;
            constractReceivablesRecord.InvoiceAccount = deAmount;

            constractReceivablesRecord.Payer = strReOrPayer;
            constractReceivablesRecord.OperatorCode = strOperatorCode;
            constractReceivablesRecord.OperatorName = ShareClass.GetUserName(strOperatorCode);
            constractReceivablesRecord.OperateTime = DateTime.Now;
            constractReceivablesRecord.Comment = "";

            constractReceivablesRecord.RelatedProjectID = intRelatedProjectID;

            try
            {
                constractReceivablesRecordBLL.AddConstractReceivablesRecord(constractReceivablesRecord);
            }
            catch
            {
            }
        }

        if (strReOrPay == "Payables")
        {
            decimal deExchangRate = GetExchangeRateByCurrencyType(strCurrencyType);

            ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
            ConstractPayableRecord constractPayableRecord = new ConstractPayableRecord();

            constractPayableRecord.PayableID = intRelatedID;
            constractPayableRecord.ConstractCode = "";

            constractPayableRecord.ReAndPayType = strReOrPayerType;
            constractPayableRecord.Currency = strCurrencyType;
            constractPayableRecord.Bank = "";
            constractPayableRecord.ExchangeRate = deExchangRate;

            constractPayableRecord.OutOfPocketAccount = deAmount;
            constractPayableRecord.OutOfPocketTime = DateTime.Now;
            constractPayableRecord.InvoiceAccount = deAmount;
            constractPayableRecord.HomeCurrencyAmount = deAmount * deExchangRate;

            constractPayableRecord.Receiver = strReOrPayer;
            constractPayableRecord.OperatorCode = strOperatorCode;
            constractPayableRecord.OperatorName = ShareClass.GetUserName(strOperatorCode);
            constractPayableRecord.OperateTime = DateTime.Now;
            constractPayableRecord.Comment = "";

            constractPayableRecord.RelatedProjectID = intRelatedProjectID;

            try
            {
                constractPayableRecordBLL.AddConstractPayableRecord(constractPayableRecord);
            }
            catch
            {
            }
        }
    }

    //ȡ�û���
    public static decimal GetExchangeRateByCurrencyType(string strCurrencyType)
    {
        string strHQL;

        strHQL = "Select ExchangeRate From T_CurrencyType Where Type = " + "'" + strCurrencyType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CurrencyType");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 1;
        }
    }

    #endregion ��������ϲ�������

    #region ��Ŀ��ز�������

    //�г����õĹ�����ģ��
    public static void LoadProjectPlanStartupRelatedWorkflowTemplate(string strUserCode, DropDownList DL_TemName)
    {
        string strHQL;

        string strUserParentDepartmentString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);
        string strUserUnderDepartmentString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        strHQL = "Select TemName From T_WorkFlowTemplate Where Visible = 'YES' and Authority = 'All'";
        strHQL += " and (BelongDepartCode in " + strUserParentDepartmentString;
        strHQL += " Or BelongDepartCode in " + strUserUnderDepartmentString + ")";
        strHQL += " Order by SortNumber ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkflowTemplate");

        DL_TemName.DataSource = ds;
        DL_TemName.DataBind();

        DL_TemName.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //�жϵ�ǰ�û���û���޸��û��ƻ���Ȩ��
    public static string CheckUserIsCanUpdatePlan(string strProjectID, string strVerID)
    {
        string strHQL;
        string strPlanVerType, strCurrentUserCode;

        strCurrentUserCode = HttpContext.Current.Session["UserCode"].ToString();

        if (strVerID == null)
        {
            return "False";
        }

        //����Ŀ�����Ƿ���������������Ŀ�ƻ��ж��ܷ��޸ļƻ�
        if (CheckProjectPlanCanBeUpdate(strProjectID) == "NO")
        {
            return "False";
        }

        try
        {
            strHQL = "Select Type From T_ProjectPlanVersion Where ProjectID = " + strProjectID + " and VerID = " + strVerID;
            DataSet ds0 = GetDataSetFromSql(strHQL, "T_ProjectPlanVersion");
            strPlanVerType = ds0.Tables[0].Rows[0][0].ToString().Trim();

            if (strPlanVerType != "Baseline")
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and (PMCode = " + "'" + strCurrentUserCode + "'";
                strHQL += " or UserCode = " + "'" + strCurrentUserCode + "')";
                DataSet ds1 = GetDataSetFromSql(strHQL, "T_Project");

                strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID;
                strHQL += " and CanUpdatePlan = 'YES'";
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds2 = GetDataSetFromSql(strHQL, "T_RelatedUser");

                if (ds1.Tables[0].Rows.Count > 0 | ds2.Tables[0].Rows.Count > 0)
                {
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
            else
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds3 = GetDataSetFromSql(strHQL, "T_Project");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
        }
        catch
        {
            return "False";
        }
    }

    //����Ŀ�����Ƿ���������������Ŀ�ƻ��ж��ܷ��޸ļƻ�
    public static string CheckProjectPlanCanBeUpdate(string strProjectID)
    {
        //�ж��ܷ���ļƻ�
        if (ShareClass.CheckStartupPlanIsLock(strProjectID) == "YES" & ShareClass.CheckProjectPlanIsStartup(strProjectID) == "YES")
        {
            return "NO";
        }
        else
        {
            return "YES";
        }
    }

    //�ж��ܷ������Ŀ�ƻ�
    public static string CheckProjectPlanIsStartup(string strProjectID)
    {
        string strHQL;

        strHQL = string.Format(@"Select ProjectPlanStartupStatus From T_Project Where ProjectID = {0}", strProjectID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["ProjectPlanStartupStatus"].ToString().Trim();
        }
        else
        {
            return "NO";
        }
    }

    //�ж��Ƿ���������������Ŀ�ƻ�
    public static string CheckStartupPlanIsLock(string strProjectID)
    {
        string strHQL;

        strHQL = "Select LockStartupedPlan From T_Project Where ProjectID =" + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "NO";
        }
    }

    //�����Ŀ��Ա�Ƿ��Ѵ���
    public static int CheckProjectMemberIsExisted(string strProjectID, string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID + " and UserCode = '" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");

        return ds.Tables[0].Rows.Count;
    }

    //���÷����ı���ɫ
    public static void SetRiskLabelColor(DataGrid dataGrid, int intCellNumber)
    {
        string strProjectID;
        int i;

        for (i = 0; i < dataGrid.Items.Count; i++)
        {
            strProjectID = dataGrid.Items[i].Cells[intCellNumber].Text.Trim();

            if (GetRiskUnFinishNumber(strProjectID) > 0)
            {
                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RiskNumber")).BackColor = Color.Red;
            }
        }
    }

    //ȡ��û�а�ʱ��ɵķ�������
    public static int GetRiskUnFinishNumber(string strProjectID)
    {
        string strHQL;

        strHQL = "Select * From T_ProjectRisk Where EffectDate < now() and Status <> 'Resolved' and ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectRisk");

        return ds.Tables[0].Rows.Count;
    }

    //����ȱ���ı���ɫ
    public static void SetDefectLabelColor(DataGrid dataGrid, int intCellNumber)
    {
        string strProjectID;
        int i;

        for (i = 0; i < dataGrid.Items.Count; i++)
        {
            strProjectID = dataGrid.Items[i].Cells[intCellNumber].Text.Trim();

            if (GetDefectUnFinishNumber(strProjectID) > 0)
            {
                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefectNumber")).BackColor = Color.Red;
            }
        }
    }

    //ȡ��û�а�ʱ��ɵ�ȱ������
    public static int GetDefectUnFinishNumber(string strProjectID)
    {
        string strHQL;

        strHQL = "Select * From T_Defectment Where DefectFinishedDate < now() and  Status <> 'Closed' and DefectID Not In (Select DefectId From T_RelatedDefect Where ProjectID = " + strProjectID + ") ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectRisk");

        return ds.Tables[0].Rows.Count;
    }


    //ȡ����Ŀ��ȷ�Ϲ�ʱ
    public static string GetProjectTotalConfirmWorkHour(string strProjectID)
    {
        string strHQL;

        strHQL = "Select Sum(ConfirmManHour) From V_ProjectMemberManHourSummary";
        strHQL += " Where ProjectID = " + strProjectID;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DailyWork");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ����Ŀδ�����������
    public static string GetProjectTotalUNFinishRiskNumber(string strProjectID)
    {
        string strCmdText;

        strCmdText = "select count(*) from T_ProjectRisk ";
        strCmdText += " where ProjectID = " + strProjectID;
        strCmdText += " and Status <> 'Resolved'";

        DataSet ds = ShareClass.GetDataSetFromSql(strCmdText, "T_ProjectRisk");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ����Ŀ��������
    public static string GetProjectTotalRiskNumber(string strProjectID)
    {
        string strCmdText;

        strCmdText = "select count(*) from T_ProjectRisk ";
        strCmdText += " where ProjectID = " + strProjectID;

        DataSet ds = ShareClass.GetDataSetFromSql(strCmdText, "T_ProjectRisk");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ����Ŀδ�����������/��Ŀ��������
    public static string GetProjectRiskUnFinishAndFinishNumber(string strProjectID)
    {
        return GetProjectTotalUNFinishRiskNumber(strProjectID) + "/" + GetProjectTotalRiskNumber(strProjectID);
    }

    //ȡ����Ŀδ�ر�ȱ������
    public static string GetProjectTotalUNFinishDefectNumber(string strProjectID)
    {
        string strCmdText;

        strCmdText = "select count(*) from T_Defectment ";
        strCmdText += " where DefectID in (Select DefectID From T_RelatedDefect Where ProjectID = " + strProjectID + ")";
        strCmdText += " and Status <> 'Closed'";

        DataSet ds = ShareClass.GetDataSetFromSql(strCmdText, "T_Defectment");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ����Ŀȱ������
    public static string GetProjectTotalDefectNumber(string strProjectID)
    {
        string strCmdText;

        strCmdText = "select count(*) from T_Defectment ";
        strCmdText += " where DefectID in (Select DefectID From T_RelatedDefect Where ProjectID = " + strProjectID + ")";

        DataSet ds = ShareClass.GetDataSetFromSql(strCmdText, "T_Defectment");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ����Ŀδ�ر�ȱ������/��Ŀȱ������
    public static string GetProjectDefectUnFinishAndFinishNumber(string strProjectID)
    {
        return GetProjectTotalUNFinishDefectNumber(strProjectID) + "/" + GetProjectTotalDefectNumber(strProjectID);
    }

    //ȡ����Ŀ�ĵ�����
    public static string GetProjectDocumentNumber(string strProjectID)
    {
        string strHQL;

        string strUserCode = HttpContext.Current.Session["UserCode"].ToString();
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = string.Format(@"Select  DocName  from T_Document as document where ((document.RelatedType = 'Project' and document.RelatedID = {0})
                   or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID = {0}))
                   or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From T_WorkFlow as workFlow Where workFlow.RelatedType = 'Project' and workFlow.RelatedID = {0}))
                   or (document.RelatedType = '����' and document.RelatedID in (select projectRisk.ID from T_ProjectRisk as projectRisk where projectRisk.ProjectID = {0}))
                   or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from T_ProjectTask as projectTask where projectTask.ProjectID = {0}))
                   or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID From T_ImplePlan as workPlan where workPlan.ProjectID = {0}))
                   or (document.RelatedType = 'Workflow' and document.RelatedID in (Select workFlow.WLID From T_WorkFlow as workFlow Where workFlow.RelatedType = 'Plan' and workFlow.RelatedID in (select workPlan.ID From T_ImplePlan as workPlan where workPlan.ProjectID = {0})))
                   or (document.RelatedType = '����' and document.RelatedID in (select meeting.ID from T_Meeting as meeting where meeting.RelatedID = {0}))
                   )))
                   and rtrim(ltrim(document.Status)) <> 'Deleted'", strProjectID, strUserCode, strDepartCode);  

        //

        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Document");

        return ds1.Tables[0].Rows.Count.ToString();
    }

    //ȡ����Ŀ�ƻ���ģ��ʵ���ύ���ĵ�����ģ��涨Ӧ�ύ���ĵ���
    public static string GetProjectDocmentNumberAndRequiseDocument(string strProjectID)
    {
        string strHQL1, strHQL2;

        strHQL1 = string.Format(@"select t1.DocName,t2.MustUploadDoc FROM  t_documentForProjectPlanTemplate t1,T_Document t2
            where t1.RelatedType='Plan' and t1.Status<> 'Deleted' and t1.RelatedID in (Select ID From T_ImplePlan Where ProjectID = {0})
			and t2.RelatedType='Plan' and t2.Status<> 'Deleted' and t2.RelatedID in (Select ID From T_ImplePlan Where ProjectID = {0})
			and trim(t1.DocName) = trim(t2.MustUploadDoc)", strProjectID);
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_Document");

        strHQL2 = string.Format(@"select * FROM   
            t_documentForProjectPlanTemplate
            where RelatedType='Plan' and Status <> 'Deleted' and RelatedID in (Select ID From T_ImplePlan Where ProjectID = {0})", strProjectID);
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_Document");

        return ds1.Tables[0].Rows.Count.ToString() + "/" + ds2.Tables[0].Rows.Count.ToString();
    }


    //�г���Ŀ��̱�״̬ͼ
    public static void DisplayRelatedMileStoneStepDump(string strProjectID, string strVerID, Repeater Repeater1)
    {
        string strHQL;
        int intPercentDone;
        string strPlanDetail;

        DataSet ds;

        strHQL = "Select trim(Name) as Name,Percent_Done From T_ImplePlan Where ProjectID = " + strProjectID + " and VerID = " + strVerID;
        strHQL += " And to_char(Start_Date,'yyyymmdd') >= to_char(End_Date,'yyyymmdd')";
        strHQL += " Order By Start_Date ASC";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
        Repeater1.DataSource = ds;
        Repeater1.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strPlanDetail = ds.Tables[0].Rows[i]["Name"].ToString().Trim();
                    intPercentDone = int.Parse(ds.Tables[0].Rows[i]["Percent_Done"].ToString().Trim());

                    if (intPercentDone == 100)
                    {
                        ((ImageButton)Repeater1.Items[i].FindControl("IBT_MileStone")).ImageUrl = "Images/GreenDumpLarge.jpg";
                    }
                    else
                    {
                        ((ImageButton)Repeater1.Items[i].FindControl("IBT_MileStone")).ImageUrl = "Images/RegDumpLarge.jpg";
                    }
                }
            }
            catch
            {
            }
        }
    }



    //�г���Ŀ���� 
    public static void LoadProjectType(DropDownList DL_ProjectType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectType as projectType Order by projectType.SortNumber ASC";
        ProjectTypeBLL projectTypeBLL = new ProjectTypeBLL();
        lst = projectTypeBLL.GetAllProjectTypes(strHQL);
        DL_ProjectType.DataSource = lst;
        DL_ProjectType.DataBind();
    }

    //�г���Ŀ״̬
    public static void LoadProjectForPMStatus(string strProjectType, string strLangCode, DropDownList DL_Status)
    {
        string strHQL;
        IList lst;

        if (strProjectType != "")
        {
            string strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();
            string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
            {
                strHQL = "from ProjectStatus as projectStatus";
                strHQL += " Where projectStatus.ProjectType = " + "'" + strProjectType + "'";
                strHQL += " and projectStatus.Status not in ('New','Review','Plan','Accepted')";
            }
            else
            {
                strHQL = "from ProjectStatus as projectStatus";
                strHQL += " Where projectStatus.ProjectType = " + "'" + strProjectType + "'";
            }

            strHQL += " And projectStatus.LangCode =" + "'" + strLangCode + "'";
            strHQL += " Order by projectStatus.SortNumber ASC";

            ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
            lst = projectStatusBLL.GetAllProjectStatuss(strHQL);
            DL_Status.DataSource = lst;
            DL_Status.DataBind();
        }
    }

    //�г���Ŀ״̬
    public static void LoadProjectStatusForDataGrid(string strLangCode, DataGrid dataGrid)
    {
        string strHQL;
        strHQL = string.Format(@"select distinct A.Status, rtrim(A.HomeName) as HomeName,A.SortNumber from T_ProjectStatus A 
                Where A.LangCode ='{0}' 
                and A.SortNumber in (Select min(B.SortNumber) From T_ProjectStatus B Where B.LangCode = '{0}' and A.Status = B.Status)
                and Status not in ( 'Deleted', 'Archived')
                Order By A.SortNumber ASC", strLangCode);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectStatus");
        dataGrid.DataSource = ds;
        dataGrid.DataBind();
    }

    //�г��������Ŀ״̬
    public static void LoadInvolvedProjectStatusForDataGrid(string strLangCode, DataGrid dataGrid)
    {
        string strHQL;
        strHQL = string.Format(@"select distinct A.Status, rtrim(A.HomeName) as HomeName,A.SortNumber from T_ProjectStatus A 
                Where A.LangCode ='{0}' 
                and A.SortNumber in (Select min(B.SortNumber) From T_ProjectStatus B Where B.LangCode = '{0}' and A.Status = B.Status)
                and Status not in ('New','Review', 'Hided','Deleted','Archived','Pause','Stop')
                Order By A.SortNumber ASC", strLangCode);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectStatus");
        dataGrid.DataSource = ds;
        dataGrid.DataBind();
    }

    //�г���Ŀ״̬
    public static void LoadProjectStatusForDropDownList(string strLangCode, DropDownList DL_Status)
    {
        string strHQL;

        strHQL = string.Format(@"select distinct A.Status, rtrim(A.HomeName) as HomeName,A.SortNumber from T_ProjectStatus A 
                Where A.LangCode ='{0}' 
                and A.SortNumber in (Select min(B.SortNumber) From T_ProjectStatus B Where B.LangCode = '{0}' and A.Status = B.Status)
                and Status not in ( 'Deleted', 'Archived')
                Order By A.SortNumber ASC", strLangCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectStatus");
        DL_Status.DataSource = ds;
        DL_Status.DataBind();

        DL_Status.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //�����Ŀ��Ա
    public static void AddProjectMember(string strProjectID, string strActorCode, string strActor, string strWorkDetail, string strStatus)
    {
        string strProjectName = ShareClass.GetProjectName(strProjectID);

        string strActorName = ShareClass.GetUserName(strActorCode);
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strActorCode);
        string strDepartName = ShareClass.GetDepartName(strDepartCode);

        string strJoinDate = DateTime.Now.ToString("yyyy-MM-dd");
        string strLeaveDate = DateTime.Now.ToString("yyyy-MM-dd");
        string strSalaryMethod = "��ʱ";  
        decimal dePromissionScale = 0;
        decimal deHourSalary = 0;
        string strCanUpdatePlan = "YES";

        string strHQL;
        strHQL = "Select * From T_RelatedUser Where UserCode = '" + strActorCode + "' and Actor = '" + strActor + "' and ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return;
        }

        try
        {
            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            RelatedUser relatedUser = new RelatedUser();

            relatedUser.ProjectID = int.Parse(strProjectID);
            relatedUser.ProjectName = strProjectName;
            relatedUser.UserCode = strActorCode;
            relatedUser.UserName = strActorName;
            relatedUser.DepartCode = strDepartCode;
            relatedUser.DepartName = ShareClass.GetDepartName(strDepartCode);
            relatedUser.Actor = strActor;
            relatedUser.JoinDate = DateTime.Parse(strJoinDate);
            relatedUser.LeaveDate = DateTime.Parse(strLeaveDate);
            relatedUser.Status = strStatus;
            relatedUser.WorkDetail = strWorkDetail;
            relatedUser.SMSCount = 0;
            relatedUser.SalaryMethod = strSalaryMethod;
            relatedUser.PromissionScale = dePromissionScale;
            relatedUser.UnitHourSalary = deHourSalary;
            relatedUser.CanUpdatePlan = strCanUpdatePlan;

            relatedUserBLL.AddRelatedUser(relatedUser);
        }
        catch
        {
        }
    }


    //���ƻ�IDȡ����Ŀ����
    public static string GetProjectTypeByPlanID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select ProjectType From T_Project where ProjectID in (Select ProjectID From T_ImplePlan Where ID = " + strPlanID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "OtherProject";
        }
    }

    //���ƻ�IDȡ����ĿID
    public static string GetProjectIDByPlanID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select ProjectID From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "0";
        }
    }

    //�ж��Ƿ�Ҫ�ƻ�Աȷ������ʱ�Ȳ���Ӱ��ƻ�����
    public static string GetPlanProgressNeedPlanerConfirmByProject(string strProjectID)
    {
        string strHQL = "Select PlanProgressNeedPlanerConfirm From T_Project Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectType");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "NO";
        }
    }

    //ȡ����Ŀ�ƻ���KEY ID
    public static string GetProjectPlanKeyIDByVerID(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.VerID = " + "'" + strVerID + "'";

        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
            return projectPlanVersion.ID.ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ����Ŀ״ֵ̬
    public static string GetProjectStatusValue(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.StatusValue.Trim();
    }

    //ȡ����Ŀ����
    public static string GetProjectClass(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        string strProjectClass = project.ProjectClass.Trim();
        return strProjectClass;
    }

    //ȡ�������ߴ���
    public static string GetProjectCreatorCode(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.UserCode.Trim();
    }

    //�����Ŀ�Ѿ�����\�᰸\�鵵����ô���ܸ��ļƻ���Ϣ
    public static bool CheckProjectIsFinish(string strProjectID)
    {
        string strHQL;

        strHQL = "Select * From T_Project Where Status in ('Suspended','Cancel','Acceptance','CaseClosed','Archived') and ProjectId = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //����û��Ƿ�ƻ�Ա
    public static bool CheckMemberIsProjectPlanOperator(string strProjectID, string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID;
        strHQL += " and CanUpdatePlan = 'YES'";
        strHQL += " and UserCode = " + "'" + strUserCode + "'";
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");

        if (ds2.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //�������̵Ĺ�ʱ
    public static void UpdateWorkFlowManHour(string strRelatedType, string strRelatedID, string strWLID, string strID, decimal deManHour)
    {
        string strHQL;
        decimal deTotalManHour;

        try
        {
            strHQL = "Update T_WorkFlowStepDetail Set ManHour = " + deManHour.ToString() + " Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            deTotalManHour = GetWorkflowTotalManHour(strWLID);
            strHQL = "Update T_WorkFlow Set ManHour = " + deTotalManHour.ToString() + " Where WLID = " + strWLID;
            ShareClass.RunSqlCommand(strHQL);

            if (strRelatedType == "Plan")
            {
                strHQL = "update T_ImplePlan Set ActualHour = " + ShareClass.GetTotalRealManHourByPlan(strRelatedID);
                strHQL += " Where PID = " + strRelatedID;
                ShareClass.RunSqlCommand(strHQL);
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //ȡ�����̵Ĺ�ʱ�ܶ�
    public static decimal GetWorkflowTotalManHour(string strWLID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(ManHour),0) From T_WorkFlowStepDetail Where WLID = " + strWLID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //�������̵Ĺ�ʱ
    public static void UpdateWorkFlowExpense(string strRelatedType, string strRelatedID, string strWLID, string strID)
    {
        string strHQL;
        decimal deExpense, deTotalExpense;

        deExpense = GetWorkflowStepDetailTotalExpense(strID);
        try
        {
            strHQL = "Update T_WorkFlowStepDetail Set Expense = " + deExpense.ToString() + " Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            deTotalExpense = GetWorkflowTotalExpense(strWLID);
            strHQL = "Update T_WorkFlow Set Expense = " + deTotalExpense.ToString() + " Where WLID = " + strWLID;
            ShareClass.RunSqlCommand(strHQL);

            if (strRelatedType == "Plan")
            {
                strHQL = "update T_ImplePlan Set Expense = " + ShareClass.GetTotalRealExpenseByPlan(strRelatedID);
                strHQL += " Where ID = " + strRelatedID;

                ShareClass.RunSqlCommand(strHQL);
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //ȡ������������¼�ķ����ܶ�
    public static decimal GetWorkflowStepDetailTotalExpense(string strID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(ConfirmAmount),0) From T_ProExpense Where WorkflowID = " + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //ȡ�����̵ķ����ܶ�
    public static decimal GetWorkflowTotalExpense(string strWLID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(Expense),0) From T_WorkFlowStepDetail Where WLID = " + strWLID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    public static string GetProjectAllowPMChangeStatus(string strProjectID)
    {
        string strHQL;
        string strAllowPMChangeStatus;

        strHQL = "Select AllowPMChangeStatus From T_Project Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strAllowPMChangeStatus = ds.Tables[0].Rows[0][0].ToString().Trim();
            return strAllowPMChangeStatus;
        }
        else
        {
            return "NO";
        }
    }

    //ȡ�üƻ�����������
    public static string getProjectPlanLeaderName(string strPlanID)
    {
        string strHQL;

        strHQL = "Select Leader From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = CoreShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return "";
        }
    }

    //�Ѹ����˴���Ϊ�ո���Ϊ��Ϊ��
    public static void UpdateProjectWorkPlanLeaderCodeToNotNull(string strProjectID, string strVerID)
    {
        string strHQL;
        string strPMCode, strPMName;

        strHQL = "Update T_ImplePlan Set LeaderCode = F_GetUserCodeByUserName(Leader) Where ProjectID = " + strProjectID + " and VerID = " + strVerID + " and COALESCE(LeaderCode,'') = ''";
        ShareClass.RunSqlCommand(strHQL);

        strPMCode = ShareClass.GetProjectPMCode(strProjectID);
        strPMName = ShareClass.GetUserName(strPMCode);

        strHQL = "Update T_ImplePlan Set Leader = '" + strPMName + "',LeaderCode = '" + strPMCode + "'  Where ProjectID = " + strProjectID + " and VerID = " + strVerID + " and LeaderCode = 'SAMPLE'";
        ShareClass.RunSqlCommand(strHQL);
    }

    //ֻ�������ߣ���Ŀ�����ƻ�����Ա�����ܷ��ɼƻ���Դ
    public static bool CheckUserCanAssignRecourceForPlan(string strPlanID, string strCurrentUserCode)
    {
        string strHQL;
        string strVerID;

        try
        {
            string strProjectID = ShareClass.getProjectIDByPlanID(strPlanID);
            strVerID = getProjectWorkPlanVerIDByPlanID(strPlanID);

            string strPlanVerType, strPlanCreatorCode;

            strPlanCreatorCode = GetProjectPlanCreatorCode(strPlanID);

            strHQL = "Select Type From T_ProjectPlanVersion Where ProjectID = " + strProjectID + " and VerID = " + strVerID;
            DataSet ds0 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectPlanVersion");
            strPlanVerType = ds0.Tables[0].Rows[0][0].ToString().Trim();

            if (strPlanVerType != "Baseline")
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and (PMCode = " + "'" + strCurrentUserCode + "'";
                strHQL += " or UserCode = " + "'" + strCurrentUserCode + "')";
                DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

                strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID;
                strHQL += " and CanUpdatePlan = 'YES'";
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");

                if (ds1.Tables[0].Rows.Count > 0 | ds2.Tables[0].Rows.Count > 0 | strPlanCreatorCode == strCurrentUserCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    //ֻ�������ߣ���Ŀ�����ƻ�����Ա����Ŀ�����ˣ��ƻ������˲��ܷ������̺�����
    public static bool CheckUserCanControlProjectPlan(string strPlanID, string strCurrentUserCode)
    {
        string strHQL;
        string strVerID;

        try
        {
            string strProjectID = ShareClass.getProjectIDByPlanID(strPlanID);
            strVerID = ShareClass.getProjectWorkPlanVerIDByPlanID(strPlanID);

            string strPlanVerType, strPlanCreatorCode, strPlanLeaderCode, strPlanLeaderName, strCurrentUserName, strDepartString;

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strCurrentUserCode);
            strPlanCreatorCode = ShareClass.GetProjectPlanCreatorCode(strPlanID);
            strPlanLeaderCode = ShareClass.GetProjectPlanLeaderCode(strPlanID);
            strPlanLeaderName = ShareClass.GetProjectPlanLeaderName(strPlanID);
            strCurrentUserName = ShareClass.GetUserName(strCurrentUserCode);

            strHQL = "Select Type From T_ProjectPlanVersion Where ProjectID = " + strProjectID + " and VerID = " + strVerID;
            DataSet ds0 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectPlanVersion");
            strPlanVerType = ds0.Tables[0].Rows[0][0].ToString().Trim();

            if (strPlanVerType != "Baseline")
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and (PMCode = " + "'" + strCurrentUserCode + "'";
                strHQL += " or UserCode = " + "'" + strCurrentUserCode + "')";
                DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

                strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID;
                strHQL += " and CanUpdatePlan = 'YES'";
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");

                strHQL = "Select * from T_Project ";
                strHQL += " Where ProjectID = " + strProjectID + " and PMCode in (select UnderCode from T_MemberLevel where ProjectVisible = 'YES' and UserCode = " + "'" + strCurrentUserCode + "'" + ")";
                DataSet ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

                strHQL = "Select * from T_Project ";
                strHQL += " Where ProjectID = " + strProjectID + " and PMCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
                DataSet ds4 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

                if (ds1.Tables[0].Rows.Count > 0 | ds2.Tables[0].Rows.Count > 0 | ds3.Tables[0].Rows.Count > 0 | ds4.Tables[0].Rows.Count > 0 | strPlanCreatorCode == strCurrentUserCode | strPlanLeaderCode == strCurrentUserCode | strPlanLeaderName == strCurrentUserName)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    //ֻ�������ߣ���Ŀ�����ƻ�����Ա����Ŀ�����ˣ��ƻ������˲�������ƻ�����
    public static bool CheckUserCanViewProjectPlan(string strPlanID, string strCurrentUserCode)
    {
        string strHQL;
        string strVerID;

        try
        {
            string strProjectID = ShareClass.getProjectIDByPlanID(strPlanID);
            strVerID = ShareClass.getProjectWorkPlanVerIDByPlanID(strPlanID);

            string strPlanVerType, strPlanCreatorCode, strPlanLeaderCode, strPlanLeaderName, strCurrentUserName, strDepartString;

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strCurrentUserCode);
            strPlanCreatorCode = ShareClass.GetProjectPlanCreatorCode(strPlanID);
            strPlanLeaderCode = ShareClass.GetProjectPlanLeaderCode(strPlanID);
            strPlanLeaderName = ShareClass.GetProjectPlanLeaderName(strPlanID);
            strCurrentUserName = ShareClass.GetUserName(strCurrentUserCode);

            strHQL = "Select Type From T_ProjectPlanVersion Where ProjectID = " + strProjectID + " and VerID = " + strVerID;
            DataSet ds0 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectPlanVersion");
            strPlanVerType = ds0.Tables[0].Rows[0][0].ToString().Trim();

            if (strPlanVerType != "Baseline")
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and (PMCode = " + "'" + strCurrentUserCode + "'";
                strHQL += " or UserCode = " + "'" + strCurrentUserCode + "')";
                DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

                strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID;
                strHQL += " and CanUpdatePlan = 'YES'";
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");

                strHQL = "Select * from T_Project ";
                strHQL += " Where ProjectID = " + strProjectID + " and PMCode in (select UnderCode from T_MemberLevel where ProjectVisible = 'YES' and UserCode = " + "'" + strCurrentUserCode + "'" + ")";
                DataSet ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

                strHQL = "Select * from T_Project ";
                strHQL += " Where ProjectID = " + strProjectID + " and PMCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
                DataSet ds4 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

                if (ds1.Tables[0].Rows.Count > 0 | ds2.Tables[0].Rows.Count > 0 | ds3.Tables[0].Rows.Count > 0 | ds4.Tables[0].Rows.Count > 0 | strPlanCreatorCode == strCurrentUserCode | strPlanLeaderCode == strCurrentUserCode | strPlanLeaderName == strCurrentUserName)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
                strHQL += " and UserCode = " + "'" + strCurrentUserCode + "'";
                DataSet ds3 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    //ȡ�üƻ�������
    public static string GetProjectPlanCreatorCode(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkPlan as workPlan where workPlan.ID = " + strPlanID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);

        WorkPlan workPlan = (WorkPlan)lst[0];

        return workPlan.CreatorCode.Trim();
    }

    //ȡ����������ƻ��ļƻ��ĸ����˴���
    public static string GetProjectPlanLeaderCode(string strPlanID)
    {
        string strHQL;
        string strLeaderCode;

        strHQL = "Select LeaderCode From T_ImplePlan Where ID = " + strPlanID;

        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
            strLeaderCode = ds.Tables[0].Rows[0][0].ToString().Trim();

            return strLeaderCode;
        }
        catch
        {
            return "";
        }
    }

    //ȡ����������
    public static string GetProjectPlanDetail(string strPlanID)
    {
        string strHQL;
        string strPlanDetail;

        strHQL = "Select Name From T_ImplePlan Where ID = " + strPlanID;

        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
            strPlanDetail = ds.Tables[0].Rows[0][0].ToString().Trim();

            return strPlanDetail;
        }
        catch
        {
            return "";
        }
    }

    //ȡ����������ƻ��ļƻ��ĸ���������
    public static string GetProjectPlanLeaderName(string strPlanID)
    {
        string strHQL;
        string strLeaderName;

        strHQL = "Select Leader From T_ImplePlan Where ID = " + strPlanID;

        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
            strLeaderName = ds.Tables[0].Rows[0][0].ToString().Trim();

            return strLeaderName;
        }
        catch
        {
            return "";
        }
    }

    //���ƻ���ȡ�ô˼ƻ��İ汾��
    public static string getProjectWorkPlanVerIDByPlanID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select VerID From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    //ȡ����Ŀ�ƻ����İ汾��
    public static string GetLargestProjectPlanVerID(string strProjectID)
    {
        string strHQL;

        strHQL = "Select Max(VerID) from T_ImplePlan Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�ð汾��ID
    public static int GetProjectPlanVersionID(string strProjectID, string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.Type = " + "'" + strType + "'";

        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
            return projectPlanVersion.ID;
        }
        else
        {
            return 0;
        }
    }

    //���ƻ�����ȡ�ð汾��
    public static int GetProjectPlanVersionIDByType(string strProjectID, string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.Type = " + "'" + strType + "'";

        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
            return projectPlanVersion.VerID;
        }
        else
        {
            return 0;
        }
    }

    //ȡ�ð汾��
    public static int GetProjectPlanVerID(string strProjectID, string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.Type = " + "'" + strType + "'";

        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
            return projectPlanVersion.VerID;
        }
        else
        {
            return 0;
        }
    }

    //ȡ����Ŀ�ƻ���ص���ĿID��
    public static string getProjectIDByPlanID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select ProjectID From T_ImplePlan Where ID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    //��������Ĺ�ʱ�ͷ���
    public static void UpdateTaskExpenseManHourSummary(string strTaskID)
    {
        string strHQL;
        IList lst;
        decimal deExpenseSum = 0, deManHourSum = 0;

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.TaskID = " + strTaskID;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        TaskAssignRecord taskAssignRecord = new TaskAssignRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            taskAssignRecord = (TaskAssignRecord)lst[i];

            deExpenseSum += taskAssignRecord.Expense;
            deManHourSum += taskAssignRecord.ManHour;
        }

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = (ProjectTask)lst[0];

        projectTask.Expense = deExpenseSum;
        projectTask.RealManHour = deManHourSum;

        projectTaskBLL.UpdateProjectTask(projectTask, projectTask.TaskID);
    }

    //�������������
    public static decimal UpdateTaskProgress(string strTaskID)
    {
        string strHQL;
        decimal deProgress = 0;

        try
        {
            strHQL = "Select avg(FinishPercent) From T_TaskAssignRecord Where TaskID = " + strTaskID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

            if (ds.Tables[0].Rows.Count > 0)
            {
                deProgress = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deProgress = 0;
            }

            strHQL = "Update T_ProjectTask Set FinishPercent = " + deProgress.ToString();
            strHQL += " Where TaskID = " + strTaskID;
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }

        return deProgress;
    }

    //���¹�����������Ŀ�ƻ���ɳ̶�
    public static void UpdateProjectPlanSchedule(string strRelatedType, string strRelatedID)
    {
        try
        {
            if (strRelatedType == "Plan")
            {
                UpdateTaskOrWorkflowPlanProgressAndExpenseWorkHour(strRelatedID);
            }
        }
        catch
        {
        }
    }

    //���ƻ���ع����������񣬸�����Ŀ�˼ƻ����Ⱥ��ܽ���
    public static void UpdateTaskOrWorkflowPlanProgressAndExpenseWorkHour(string strPlanID)
    {
        string strHQL;
        string strProjectID, strVerID;
        decimal deProgress = 0, deProjectProgress = 0;

        try
        {
            strProjectID = ShareClass.getProjectIDByPlanID(strPlanID);
            strVerID = ShareClass.getProjectWorkPlanVerIDByPlanID(strPlanID);

            deProgress = ShareClass.GetTaskOrWorkflowPlanProgress(strPlanID);

            strHQL = "Update T_ImplePlan Set Percent_Done = " + deProgress.ToString();
            strHQL += " Where ID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Select COALESCE(Avg(Percent_Done),0) From T_ImplePlan Where ProjectID = " + strProjectID + " And VerID = " + strVerID;
            strHQL += " and ID Not In (Select ParentID From T_ImplePlan Where ProjectID = " + strProjectID + " And VerID = " + strVerID + ")";
            strHQL += " and Parent_ID > 0";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

            deProjectProgress = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

            strHQL = "Update T_ImplePlan Set Percent_Done = " + deProjectProgress.ToString();
            strHQL += " Where Parent_ID = 0 and ProjectID =" + strProjectID + " and VerID = " + strVerID;
            ShareClass.RunSqlCommand(strHQL);

            //�����������Ŀ�ƻ������ģ���ô���ļƻ���ʱ�ͷ���
            strHQL = "update T_ImplePlan Set ActualHour = " + ShareClass.GetTotalRealManHourByPlan(strPlanID);
            strHQL += ",Expense = " + ShareClass.GetTotalRealExpenseByPlan(strPlanID);
            strHQL += " Where ID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }
    }

    //ȡ�ù��������δ�����
    public static decimal GetTaskUnFinishedNumber(string strTaskID)
    {
        string strHQL;

        strHQL = "Select (RequireNumber -FinishedNumber) as UnFinishedNumber From T_ ProjectTask Where TaskID = " + strTaskID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //���������������
    public static decimal UpdateTaskFinishedNumber(string strTaskID)
    {
        string strHQL;
        decimal deFinishedNumber = 0;

        try
        {
            strHQL = "Select Sum(FinishedNumber) From T_TaskAssignRecord Where TaskID = " + strTaskID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

            if (ds.Tables[0].Rows.Count > 0)
            {
                deFinishedNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deFinishedNumber = 0;
            }

            strHQL = "Update T_ProjectTask Set FinishedNumber = " + deFinishedNumber.ToString();
            strHQL += " Where TaskID = " + strTaskID;
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }

        return deFinishedNumber;
    }

    //���¹�����������Ŀ�ƻ��������
    public static void UpdateProjectPlanFinishedNumber(string strRelatedType, string strRelatedID)
    {
        try
        {
            if (strRelatedType == "Plan")
            {
                UpdateTaskPlanFinishedNumber(strRelatedID);
            }
        }
        catch
        {
        }
    }

    //���ƻ�������񣬸�����Ŀ�˼ƻ����������
    public static void UpdateTaskPlanFinishedNumber(string strPlanID)
    {
        string strHQL;
        decimal deFinishedNumber = 0;

        try
        {
            strHQL = "Select sum(COALESCE(FinishedNumber,0)) From T_ProjectTask Where PlanID = " + strPlanID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");
            if (ds.Tables[0].Rows.Count > 0)
            {
                deFinishedNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deFinishedNumber = 0;
            }

            strHQL = "Update T_ImplePlan Set FinishedNumber = " + deFinishedNumber.ToString();
            strHQL += " Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }
    }


    //�������������Ŀ����Ŀ�ƻ�����ģ���ô������Ŀ��־����Ŀ��
    public static void UpdateProjectDaiyWorkByWorkflow(string strRelatedType, string strRelatedID, string strWLID, string strContent, string strUserCode)
    {
        if (strRelatedType == "Project" || strRelatedType == "Plan")
        {
            string strProjectID;

            try
            {
                if (strRelatedType == "Project")
                {
                    ShareClass.UpdateDailyWork(strUserCode, strRelatedID, "Workflow", strWLID, strContent);
                }
                if (strRelatedType == "Plan")
                {
                    strProjectID = ShareClass.getProjectIDByPlanID(strRelatedID);
                    if (strProjectID != "")
                    {
                        ShareClass.UpdateDailyWork(strUserCode, strProjectID, "Workflow", strWLID, strContent);
                    }
                }
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    //ȡ�ù�����������̵ļƻ��ܽ���
    public static decimal GetTaskOrWorkflowPlanProgress(string strPlanID)
    {
        decimal deProgress, deTaskProgress, deWorkflowProgress;

        deTaskProgress = GetTaskPlanProgress(strPlanID);
        deWorkflowProgress = GetWorkflowtPlanProgress(strPlanID);

        if (deTaskProgress == 0 | deWorkflowProgress == 0)
        {
            deProgress = deTaskProgress + deWorkflowProgress;
        }
        else
        {
            deProgress = (deTaskProgress + deWorkflowProgress) / 2;
        }

        return deProgress;
    }

    //ȡ�üƻ�����������
    public static decimal GetTaskPlanProgress(string strPlanID)
    {
        string strHQL;
        decimal deTaskProgress;

        try
        {
            strHQL = "Select avg(COALESCE(FinishPercent,0)) From T_ProjectTask Where PlanID = " + strPlanID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");
            if (ds.Tables[0].Rows.Count > 0)
            {
                deTaskProgress = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deTaskProgress = 0;
            }

            return deTaskProgress;
        }
        catch
        {
            return 0;
        }
    }

    //ȡ�üƻ���ع�������ƽ������
    public static decimal GetWorkflowtPlanProgress(string strPlanID)
    {
        string strHQL1, strHQL;
        DataSet ds1, ds;

        string strWLID, strStepID, strTemName;
        int intSortNumber, i = 0;
        decimal deTotalFinishPercent = 0;

        try
        {
            strHQL = "Select WLID From T_WorkFlowStep Where WLID In ( Select WLID From T_WorkFlow Where RelatedType = 'Plan' and RelatedID = " + strPlanID + ")";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStep");

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strWLID = ds.Tables[0].Rows[i][0].ToString();

                strHQL1 = "Select max(StepID) From T_WorkFlowStep Where Status = 'Passed' and WLID = " + strWLID;
                ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_WorkflowStep");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strStepID = ds1.Tables[0].Rows[0][0].ToString().Trim();
                    intSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID);
                    strTemName = ShareClass.GetWorkflowTemNameByWLID(strWLID);

                    deTotalFinishPercent += ShareClass.GetWorkFlowTStep(strTemName, intSortNumber).FinishPercent;
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return deTotalFinishPercent / ds.Tables[0].Rows.Count;
            }
            else
            {
                return 0;
            }
        }
        catch
        {
            return 0;
        }
    }

    //ȡ����Ŀ���ȣ���ʱ�����õ�������ݣ�������Ŀ����
    public static string getCurrentDateTaskTotalForPM(string strProjectID, string strUserCode, string strWorkDate)
    {
        return LanguageHandle.GetWord("DangRiRenWu").ToString().Trim() + ":" + LanguageHandle.GetWord("JingDu").ToString().Trim() + ":" + getCurrentDateTotalProgressForPM(strProjectID) + "%," + LanguageHandle.GetWord("ManHour").ToString().Trim() + ":" + getCurrentDateTotalManHourByOneOperator(strProjectID, strUserCode, strWorkDate) + "," + LanguageHandle.GetWord("FeiYong").ToString().Trim() + ":" + getCurrentDateTotalExpenseByOneOperator(strProjectID, strUserCode, strWorkDate);
    }

    //ȡ����Ŀ���ȣ���ʱ�����õ�������ݣ�������Ŀ��Ա
    public static string getCurrentDateTaskTotalForMember(string strProjectID, string strUserCode, string strWorkDate)
    {
        return LanguageHandle.GetWord("DangRiRenWu").ToString().Trim() + ":" + LanguageHandle.GetWord("JingDu").ToString().Trim() + ":" + getCurrentDateTotalProgressForMember(strProjectID, strUserCode) + "%," + LanguageHandle.GetWord("ManHour").ToString().Trim() + ":" + getCurrentDateTotalManHourByOneOperator(strProjectID, strUserCode, strWorkDate) + "," + LanguageHandle.GetWord("FeiYong").ToString().Trim() + ":" + getCurrentDateTotalExpenseByOneOperator(strProjectID, strUserCode, strWorkDate);
    }

    //ȡ����Ŀ������ʱ���ܽ��ȣ�������Ŀ����
    public static string getCurrentDateTotalProgressForPM(string strProjectID)
    {
        if (decimal.Parse(getCurrentDateTaskTotalProgressForPM(strProjectID)) == 0 || decimal.Parse(getCurrentDateWorkflowTotalProgressForPM(strProjectID)) == 0)
        {
            return (decimal.Parse(getCurrentDateTaskTotalProgressForPM(strProjectID)) + decimal.Parse(getCurrentDateWorkflowTotalProgressForPM(strProjectID))).ToString();
        }
        else
        {
            return ((decimal.Parse(getCurrentDateTaskTotalProgressForPM(strProjectID)) + decimal.Parse(getCurrentDateWorkflowTotalProgressForPM(strProjectID))) / 2).ToString();
        }
    }

    //ȡ����Ŀ������ʱ���ܽ��ȣ�������Ŀ��Ա
    public static string getCurrentDateTotalProgressForMember(string strProjectID, string strUserCode)
    {
        if (decimal.Parse(getCurrentDateTaskTotalProgressForMember(strProjectID, strUserCode)) == 0 || decimal.Parse(getCurrentDateWorkflowTotalProgressForMember(strProjectID, strUserCode)) == 0)
        {
            return (decimal.Parse(getCurrentDateTaskTotalProgressForMember(strProjectID, strUserCode)) + decimal.Parse(getCurrentDateWorkflowTotalProgressForMember(strProjectID, strUserCode))).ToString();
        }
        else
        {
            return ((decimal.Parse(getCurrentDateTaskTotalProgressForMember(strProjectID, strUserCode)) + decimal.Parse(getCurrentDateWorkflowTotalProgressForMember(strProjectID, strUserCode))) / 2).ToString();
        }
    }

    //ȡ�õ���ʵʩ��Ŀ�ܹ�ʱ
    public static string getCurrentDateTotalManHourByOneOperator(string strProjectID, string strUserCode, string strWorkDate)
    {
        string strHQL;
        DataSet ds1, ds2;

        strHQL = "Select COALESCE(Sum(ManHour),0) From T_TaskAssignRecord Where ID In ";
        strHQL += "(Select ID from T_TaskAssignRecord ";
        strHQL += " where ((TaskID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + ")) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where PlanID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where ReqID In (Select ReqID From T_RelatedReq Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where DefectID In (Select DefectID From T_RelatedDefect Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where RiskID In (Select ID From T_ProjectRisk Where ProjectID = " + strProjectID + ")))) ";
        strHQL += " and OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and to_char(MakeDate,'yyyymmdd') = " + "'" + strWorkDate + "')";

        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

        strHQL = "Select COALESCE(Sum(ManHour),0) From T_WorkFlowStepDetail Where WLID In ";
        strHQL += " (Select WLID From T_WorkFlow Where ((RelatedType = 'Project' and RelatedID = " + strProjectID + ")";
        strHQL += " Or (RelatedType = 'Plan' and RelatedID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))))";
        strHQL += " and OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and to_char(CheckingTime,'yyyymmdd') = " + "'" + strWorkDate + "'";

        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        return (decimal.Parse(ds1.Tables[0].Rows[0][0].ToString()) + decimal.Parse(ds2.Tables[0].Rows[0][0].ToString())).ToString();
    }

    //ȡ�õ���ʵʩ��Ŀ�ܷ���
    public static string getCurrentDateTotalExpenseByOneOperator(string strProjectID, string strUserCode, string strWorkDate)
    {
        string strHQL;
        DataSet ds1, ds2;

        strHQL = "Select COALESCE(Sum(Expense),0) From T_TaskAssignRecord Where ID In ";
        strHQL += "(Select ID from T_TaskAssignRecord ";
        strHQL += " where ((TaskID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + ")) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where PlanID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where ReqID In (Select ReqID From T_RelatedReq Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where DefectID In (Select DefectID From T_RelatedDefect Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where RiskID In (Select ID From T_ProjectRisk Where ProjectID = " + strProjectID + ")))) ";
        strHQL += " and OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and to_char(MakeDate,'yyyymmdd') = " + "'" + strWorkDate + "')";
        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

        strHQL = "Select COALESCE(Sum(Expense),0) From T_WorkFlowStepDetail Where WLID In ";
        strHQL += " (Select WLID From T_WorkFlow Where ((RelatedType = 'Project' and RelatedID = " + strProjectID + ")";
        strHQL += " Or (RelatedType = 'Plan' and RelatedID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))))";
        strHQL += " and OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and to_char(CheckingTime,'yyyymmdd') = " + "'" + strWorkDate + "'";

        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        return (decimal.Parse(ds1.Tables[0].Rows[0][0].ToString()) + decimal.Parse(ds2.Tables[0].Rows[0][0].ToString())).ToString();
    }

    //ȡ�õ���ʵʩ��Ŀ�����ܽ��ȣ�������Ŀ����
    public static string getCurrentDateTaskTotalProgressForPM(string strProjectID)
    {
        string strHQL;
        DataSet ds1;

        strHQL = "Select COALESCE(Avg(FinishPercent),0) From T_TaskAssignRecord Where ID In ";
        strHQL += "(Select ID from T_TaskAssignRecord ";
        strHQL += " where ((TaskID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + ")) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where PlanID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where ReqID In (Select ReqID From T_RelatedReq Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where DefectID In (Select DefectID From T_RelatedDefect Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where RiskID In (Select ID From T_ProjectRisk Where ProjectID = " + strProjectID + "))))) ";
        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

        if (ds1.Tables[0].Rows.Count > 0)
        {
            return ds1.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�õ���ʵʩ��Ŀ�ܽ��ȣ�������Ŀ��Ա
    public static string getCurrentDateTaskTotalProgressForMember(string strProjectID, string strUserCode)
    {
        string strHQL;
        DataSet ds1;

        strHQL = "Select COALESCE(Avg(FinishPercent),0) From T_TaskAssignRecord Where ID In ";
        strHQL += "(Select ID from T_TaskAssignRecord ";
        strHQL += " where ((TaskID in (select TaskID from T_ProjectTask where ProjectID = " + strProjectID + ")) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where PlanID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where ReqID In (Select ReqID From T_RelatedReq Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where DefectID In (Select DefectID From T_RelatedDefect Where ProjectID = " + strProjectID + "))) ";
        strHQL += " Or (TaskID in (select TaskID from T_ProjectTask where RiskID In (Select ID From T_ProjectRisk Where ProjectID = " + strProjectID + ")))) ";
        strHQL += " and OperatorCode = " + "'" + strUserCode + "')";
        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

        if (ds1.Tables[0].Rows.Count > 0)
        {
            return ds1.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�üƻ���ع�������ƽ�����ȣ�������Ŀ����
    public static string getCurrentDateWorkflowTotalProgressForPM(string strProjectID)
    {
        string strHQL1, strHQL;
        DataSet ds1, ds;

        string strWLID, strStepID, strTemName;
        int intSortNumber, i = 0;
        decimal deTotalFinishPercent = 0;

        try
        {
            strHQL = "Select WLID From T_WorkFlowStep Where WLID In ";
            strHQL += " ((Select WLID From T_WorkFlow Where RelatedType = 'Plan' and RelatedID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))";
            strHQL += " or (Select WLID From T_WorkFlow Where RelatedType = 'Project' and RelatedID = " + strProjectID + "))";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStep");

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strWLID = ds.Tables[0].Rows[i][0].ToString();

                strHQL1 = "Select max(StepID) From T_WorkFlowStep Where Status = 'Passed' and WLID = " + strWLID;
                ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_WorkflowStep");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strStepID = ds1.Tables[0].Rows[0][0].ToString().Trim();
                    intSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID);
                    strTemName = ShareClass.GetWorkflowTemNameByWLID(strWLID);

                    deTotalFinishPercent += ShareClass.GetWorkFlowTStep(strTemName, intSortNumber).FinishPercent;
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return (deTotalFinishPercent / ds.Tables[0].Rows.Count).ToString();
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ�üƻ���ع�������ƽ�����ȣ�������Ŀ��Ա
    public static string getCurrentDateWorkflowTotalProgressForMember(string strProjectID, string strUserCode)
    {
        string strHQL1, strHQL;
        DataSet ds1, ds;

        string strWLID, strStepID, strTemName;
        int intSortNumber, i = 0;
        decimal deTotalFinishPercent = 0;

        try
        {
            strHQL = "Select WLID From T_WorkFlowStep Where WLID In ";
            strHQL += " ((Select WLID From T_WorkFlow Where RelatedType = 'Plan' and RelatedID In (Select ID From T_ImplePlan Where ProjectID = " + strProjectID + "))";
            strHQL += " or (Select WLID From T_WorkFlow Where RelatedType = 'Project' and RelatedID = " + strProjectID + "))";
            strHQL += " and StepID in (Select StepID From T_WorkflowStepDetail Where OperatorCode = '" + strUserCode + "')";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStep");

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strWLID = ds.Tables[0].Rows[i][0].ToString();

                strHQL1 = "Select max(StepID) From T_WorkFlowStep Where Status = 'Passed' and WLID = " + strWLID;
                ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_WorkflowStep");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strStepID = ds1.Tables[0].Rows[0][0].ToString().Trim();
                    intSortNumber = ShareClass.GetWorkFlowCurrentStepSortNumber(strStepID);
                    strTemName = ShareClass.GetWorkflowTemNameByWLID(strWLID);

                    deTotalFinishPercent += ShareClass.GetWorkFlowTStep(strTemName, intSortNumber).FinishPercent;
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return (deTotalFinishPercent / ds.Tables[0].Rows.Count).ToString();
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����ĩ��ʼʱ��
    public static string GetWeekendFirstDay()
    {
        string strHQL;

        strHQL = "Select WeekendFirstDay From T_WorkingDayRule";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkingDayRule");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "6";
        }
    }

    //ȡ����ĩ����ʱ��
    public static string GetWeekendSecondDay()
    {
        string strHQL;

        strHQL = "Select WeekendSecondDay From T_WorkingDayRule";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkingDayRule");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ����ĩ�Ƿ�����
    public static string GetWeekendsAreWorkdays()
    {
        string strHQL;

        strHQL = "Select WeekendsAreWorkdays From T_WorkingDayRule";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkingDayRule");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "false";
        }
    }

    //���б�������ӹ����Ĺ�����ģ��
    public static string AddRelatedWorkFlowTemplateByBMBidType(string strBMBidType, string strBMBidPlanID)
    {
        string strHQL;

        strHQL = "Insert Into T_RelatedWorkFlowTemplate(RelatedType,RelatedID,WFTemplateName,IdentifyString,RelatedName)";
        strHQL += " Select 'BMBidType'," + strBMBidPlanID + ",WFTemplateName,IdentifyString,RelatedName From T_RelatedWorkFlowTemplate";
        strHQL += " Where RelatedType = 'BMBidType' and RelatedName = '" + strBMBidType + "'";
        strHQL += " and WFTemplateName Not In (Select WFTemplateName From T_RelatedWorkFlowTemplate Where RelatedType = 'BMBidType' and RelatedID = " + strBMBidPlanID + ")";
        ShareClass.RunSqlCommand(strHQL);

        return strHQL;
    }

    //����Ŀ������ӹ����Ĺ�����ģ��
    public static void AddRelatedWorkFlowTemplateByProjectType(string strRelatedType, string strRelatedID, string strKeyWord, string strKeyType, string strKeyRelatedType)
    {
        string strHQL;


        strHQL = "Insert Into T_RelatedWorkFlowTemplate(RelatedType,RelatedID,WFTemplateName,IdentifyString,RelatedName)";
        strHQL += " Select '" + strKeyWord + "'," + strRelatedID + ",WFTemplateName,IdentifyString,RelatedName From T_RelatedWorkFlowTemplate";
        strHQL += " Where RelatedType = '" + strKeyRelatedType + "' and RelatedName = '" + strRelatedType + "'";
        strHQL += " and WFTemplateName Not In (Select WFTemplateName From T_RelatedWorkFlowTemplate Where RelatedType = '" + strKeyWord + "' and RelatedID = " + strRelatedID + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace + " Sql: " + strHQL);
        }
    }

    //����Ŀ������ӹ������ĵ�ģ��
    public static void AddRelatedDocumentTemplateByProjectType(string strRelatedType, string strRelatedID, string strKeyWord, string strKeyType)
    {
        string strHQL;

        strHQL = "Insert Into T_Document(RelatedType,DocTypeID,DocType,RelatedID,DocName,Description,Address,Author,";
        strHQL += "DepartCode ,DepartName,UploadManCode,UploadManName,UploadTime,Visible  ,Status ,RelatedName)";
        strHQL += " Select '" + strKeyWord + "',DocTypeID,DocType," + strRelatedID + ",DocName,Description,Address,Author,";
        strHQL += "DepartCode ,DepartName,UploadManCode,UploadManName,UploadTime,Visible,Status,RelatedName";
        strHQL += " From T_Document";
        strHQL += " Where RelatedType = '" + strKeyType + "' and RelatedName = '" + strRelatedType + "'";
        strHQL += " and DocName Not In (Select DocName From T_Document Where RelatedType = '" + strKeyType + "' and RelatedID = " + strRelatedID + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace + " Sql : " + strHQL);
        }

    }

    //ȡ��MRP�ƻ���������
    public static string GetRelatedBusinessTypeAndName(string strRelatedType, string strRelatedID)
    {
        string strHQL;

        DataSet ds;

        strRelatedType = strRelatedType.Trim();

        if (strRelatedType != "Other")
        {
            if (strRelatedType == "SaleOrder")
            {
                try
                {
                    strHQL = "Select SOID,SOName From T_GoodsSaleOrder Where SOID = " + strRelatedID; ;
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessObject");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return "SaleOrder: Name: " + ds.Tables[0].Rows[0][1].ToString().Trim();
                    }
                    else
                    {
                        return "SaleOrder:0";
                    }
                }
                catch
                {
                    return "SaleOrder:0";
                }
            }

            if (strRelatedType == "Project")
            {
                try
                {
                    strHQL = "Select ProjectID,ProjectName From T_Project Where ProjectID =" + strRelatedID; ;
                    ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessObject");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return "Project: Name: " + ds.Tables[0].Rows[0][1].ToString().Trim();
                    }
                    else
                    {
                        return "Project:0";
                    }
                }
                catch
                {
                    return "Project:0";
                }
            }

            return "Other:0";
        }
        else
        {
            return "Other:0";
        }
    }

    //ȡ��MRP�ƻ���������
    public static string GetMRPFormTypeAndName(string strSourceType, string strSourceRecordID)
    {
        string strHQL;

        DataSet ds;

        strSourceType = strSourceType.Trim();

        if (strSourceType != "Other")
        {
            if (strSourceType == "GoodsSORecord")
            {
                strHQL = "Select SOID,SOName From T_GoodsSaleOrder Where SOID in (Select SOID From T_GoodsSaleRecord Where ID = " + strSourceRecordID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessObject");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "SaleOrder: ID: " + ds.Tables[0].Rows[0][0].ToString().Trim() + " Name: " + ds.Tables[0].Rows[0][1].ToString().Trim();
                }
                else
                {
                    return "SaleOrder:0";
                }
            }

            if (strSourceType == "GoodsAORecord")
            {
                strHQL = "Select AAID,GAAName From T_GoodsApplication Where AAID in (Select AAID From T_GoodsApplicationDetail Where ID = " + strSourceRecordID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessObject");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "SaleApplication:ID: " + ds.Tables[0].Rows[0][0].ToString().Trim() + " Name: " + ds.Tables[0].Rows[0][1].ToString().Trim();
                }
                else
                {
                    return "SaleApplication:0";
                }
            }

            if (strSourceType == "GoodsPJRecord")
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where ProjectID in (Select ProjectID From T_ProjectRelatedItem Where ID = " + strSourceRecordID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessObject");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Project:ID: " + ds.Tables[0].Rows[0][0].ToString().Trim() + " Name: " + ds.Tables[0].Rows[0][1].ToString().Trim();
                }
                else
                {
                    return "Project:0";
                }
            }

            return "Other:0";
        }
        else
        {
            return "Other:0";
        }
    }

    //ȡ����������״̬�ı���
    public static string GetStatusHomeNameByOtherStatus(string strStatus)
    {
        string strHQL;
        string strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strHQL = "Select HomeName From T_OtherStatus Where Status = " + "'" + strStatus.Trim() + "'";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_OtherStatus");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return strStatus;
        }
    }

    //ȡ�üƻ�״̬�ı���
    public static string GetStatusHomeNameByPlanStatus(string strStatus)
    {
        string strHQL;
        string strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strHQL = "Select HomeName From T_PlanStatus Where Status = " + "'" + strStatus.Trim() + "'";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_PlanStatus");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return strStatus;
        }
    }

    //ȡ������״̬�ı���
    public static string GetStatusHomeNameByRequirementStatus(string strStatus)
    {
        string strHQL;
        string strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strHQL = "Select HomeName From T_ReqStatus Where Status = " + "'" + strStatus.Trim() + "'";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RequireStatus");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return strStatus;
        }
    }

    //ȡ������״̬�ı���
    public static string GetStatusHomeNameByDefectmentStatus(string strStatus)
    {
        string strHQL;
        string strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strHQL = "Select HomeName From T_DefectStatus Where Status = " + "'" + strStatus.Trim() + "'";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DefectStatus");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return strStatus;
        }
    }

    //ȡ�ù�����״̬�ı���
    public static string GetStatusHomeNameByTaskStatus(string strStatus)
    {
        string strHQL;
        string strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strHQL = "Select HomeName From T_TaskStatus Where Status = " + "'" + strStatus.Trim() + "'";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskStatus");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return strStatus;
        }
    }

    //ȡ�ù�����״̬�ı���
    public static string GetStatusHomeNameByWorkflowStatus(string strStatus)
    {
        string strHQL;
        string strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strHQL = "Select HomeName From T_WLStatus Where Status = " + "'" + strStatus.Trim() + "'";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkflowStatus");

        if (ds.Tables[0].Rows.Count > 0)
        {

            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return strStatus;
        }
    }

    //�ж��Ƿ�ʱ�ǳ�ʱ�Զ�����ͨ���Ĳ���
    public static string GetWorkflowStepStatusByAuto(string strStepID)
    {
        string strHQL;

        strHQL = "Select * From T_ApproveFlow Where StepID = " + strStepID + " and UserName = 'Timer'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ApproveFlow");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return LanguageHandle.GetWord("CaoShi").ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    //�ж��Ƿ�ʱ�ǳ�ʱ�Զ�����ͨ���Ĺ�����
    public static string GetWorkflowStatusByAuto(string strWLID)
    {
        string strHQL;

        strHQL = "Select * From T_ApproveFlow Where Type = 'Workflow' and RelatedID = " + strWLID + " and UserName = 'Timer'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ApproveFlow");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return LanguageHandle.GetWord("CaoShi").ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    //ȡ����Ŀ״̬�ı���
    public static string GetStatusHomeNameByProjectStatus(string strStatus, string strProjectType)
    {
        string strHQL;
        string strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strHQL = string.Format(@"Select HomeName From T_ProjectStatus Where Status = '{0}' and LangCode = '{1}' and ProjectType = '{2}'", strStatus.Trim(), strLangCode, strProjectType.Trim());
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectStatus");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return strStatus;
        }
    }

    //����û��Ƿ�����Ŀ��Ŀ��Ա
    public static bool CheckUserIsProjectMember(string strProjectID, string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_RelatedUser Where ProjectID = " + strProjectID + " And UserCode = '" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //����û��Ƿ�����Ŀ����
    public static bool CheckUserIsProjectManager(string strProjectID, string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_Project Where PMCode = '" + strUserCode + "'" + " and ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //����û��Ƿ���������
    public static bool CheckUserIsProjectCreator(string strProjectID, string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_Project Where UserCode = '" + strUserCode + "'" + " and ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //����û��Ƿ��ܸ���Ŀ�ƻ�
    public static bool CheckMemberCanUpdatePlanByUserCode(string strProjectID, string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_RelatedUser Where UserCode = " + "'" + strUserCode + "'" + " and ProjectID = " + strProjectID;
        strHQL += " and (UserCode in (Select PMCode From T_Project Where ProjectID = " + strProjectID + ")";
        strHQL += " or UserCode in (Select UserCode From T_Project Where ProjectID = " + strProjectID + ")";
        strHQL += " Or CanUpdatePlan = 'YES')";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //ȡ����Ŀ��Ա�б�
    public static void LoadProjectMember(string strProjectID, DropDownList DL_OperatorCode)
    {
        string strHQL;

        strHQL = "Select UserCode,UserName From T_RelatedUser Where ProjectID = " + strProjectID;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");
        DL_OperatorCode.DataSource = ds;
        DL_OperatorCode.DataBind();

        DL_OperatorCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //ȡ����Ŀ��Ա��������Ա���б�
    public static void LoadProjectMemberAndDirectMember(string strProjectID, string strUserCode, DropDownList DL_OperatorCode)
    {
        string strHQL;

        string strOperatorCode, strOperatorName;
        strOperatorCode = HttpContext.Current.Session["UserCode"].ToString();
        strOperatorName = ShareClass.GetUserName(strUserCode);

        strHQL = "Select UserCode,UserName From T_ProjectMember Where UserCode in (Select UnderCode From T_MemberLevel Where Usercode = " + "'" + strUserCode + "'" + ")";
        if (strProjectID != null | strProjectID != "")
        {
            strHQL += " Or UserCode in ( Select UserCode From T_RelatedUser Where ProjectID = " + strProjectID + ")";
        }
        strHQL += " and UserCode <> '" + strOperatorCode + "'";
        strHQL += " Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");
        DL_OperatorCode.DataSource = ds;
        DL_OperatorCode.DataBind();

        DL_OperatorCode.Items.Insert(0, new ListItem(strOperatorName, strOperatorCode));
    }

    //ȡ��������Ա�б�
    public static void LoadMemberList(string strUserCode, DropDownList DL_OperatorCode)
    {
        string strHQL;

        string strOperatorCode, strOperatorName;
        strOperatorCode = HttpContext.Current.Session["UserCode"].ToString();
        strOperatorName = ShareClass.GetUserName(strUserCode);

        string strSystemVersionType, strProductType;

        strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();
        strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strProductType == "LOCALSAAS" | strProductType == "SERVERSAAS")
        {
            strHQL = string.Format(@"Select Distinct UserCode,UserName,SortNumber From(Select OperatorCode as UserCode,OperatorName as UserName ,1 as SortNumber From T_TaskAssignRecord Where AssignManCode = '{0}'
                 Union Select UserCode, UserName, 2 as SortNumber From T_ProjectMember Where UserCode Not In(Select OperatorCode From T_TaskAssignRecord Where AssignManCode = '{0}')) A
                 Order By SortNumber ASC", strUserCode);
        }
        else
        {
            strHQL = "Select UserCode, UserName From T_ProjectMember Where UserCode in (select UnderCode from T_MemberLevel  where UserCode = " + "'" + strUserCode + "')";
            strHQL += "and UserCode <> '" + strOperatorCode + "'";
            strHQL += " Order By SortNumber ASC";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        DL_OperatorCode.DataSource = ds;
        DL_OperatorCode.DataBind();

        DL_OperatorCode.Items.Insert(0, new ListItem(strOperatorName, strOperatorCode));
    }

    public static void LoadTaskType(DropDownList DL_Type)
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskType as taskType order by taskType.SortNumber ASC";
        TaskTypeBLL taskTypeBLL = new TaskTypeBLL();
        lst = taskTypeBLL.GetAllTaskTypes(strHQL);
        DL_Type.DataSource = lst;
        DL_Type.DataBind();
        //DL_Type.Items.Insert(0, new ListItem("--Select--", ""));
    }

    public static void LoadTaskStatus(DropDownList DL_Status, string strLangCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskStatus as taskStatus";
        strHQL += " Where taskStatus.LangCode = " + "'" + strLangCode + "'";
        strHQL += " order by taskStatus.SortNumber ASC";
        TaskStatusBLL taskStatusBLL = new TaskStatusBLL();
        lst = taskStatusBLL.GetAllTaskStatuss(strHQL);
        DL_Status.DataSource = lst;
        DL_Status.DataBind();
    }

    public static void LoadTaskWorkRequest(DropDownList DL_WorkRequest)
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskOperation as taskOperation order by taskOperation.SortNumber ASC";
        TaskOperationBLL taskOperationBLL = new TaskOperationBLL();
        lst = taskOperationBLL.GetAllTaskOperations(strHQL);
        DL_WorkRequest.DataSource = lst;
        DL_WorkRequest.DataBind();
    }

    //ȡ�������¼�����б�
    public static void LoadTaskRecordType(DropDownList DL_RecordType)
    {
        string strHQL;

        strHQL = "Select Type From T_TaskRecordType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskRecordType");

        DL_RecordType.DataSource = ds;
        DL_RecordType.DataBind();

        //DL_RecordType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //ȡ�õ�ǰʱ����ĿӦ��ɵĽ���
    public static int GetProjectDefaultFinishPercent(string strProjectID)
    {
        string strHQL;
        int intVerID;
        string strWidth;
        int intDefaultSchedule = 0, intRealSchedule = 0;

        intVerID = GetProjectPlanVersionVerID(strProjectID, "Baseline");

        if (intVerID > 0)
        {
            DataSet ds;

            strHQL = "Select DefaultSchedule From T_ImplePlan Where ProjectID = " + strProjectID + " And VerID = " + intVerID.ToString();
            strHQL += " And  End_Date <= now()";
            strHQL += " Order By DefaultSchedule DESC limit 1 ";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
            if (ds.Tables[0].Rows.Count > 0)
            {
                strWidth = ds.Tables[0].Rows[0][0].ToString().Trim();

                try
                {
                    intDefaultSchedule = Decimal.ToInt32(decimal.Parse(strWidth));
                }
                catch (OverflowException e)
                {
                    intDefaultSchedule = 0;
                }
            }
            else
            {
                intDefaultSchedule = 0;
            }

            strHQL = string.Format(@"Select Percent_Done From T_ImplePlan Where Parent_ID = 0 and ProjectID = {0}
                       and VerID In(Select VerID From T_ProjectPlanVersion Where ProjectID = {0} and Type = 'Baseline')", strProjectID);
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
            if (ds.Tables[0].Rows.Count > 0)
            {
                strWidth = ds.Tables[0].Rows[0][0].ToString().Trim();

                try
                {
                    intRealSchedule = Decimal.ToInt32(decimal.Parse(strWidth));
                }
                catch (OverflowException e)
                {
                    intRealSchedule = 0;
                }
            }
            else
            {
                intRealSchedule = 0;
            }

            if (intDefaultSchedule > intRealSchedule)
            {
                return intDefaultSchedule;
            }
            else
            {
                return intRealSchedule;
            }
        }
        else
        {
            return 0;
        }
    }

    //ȡ�õ�ǰʱ����ĿӦ��ɵĳɱ�
    public static Decimal GetProjectDefaultFinishCost(string strProjectID)
    {
        string strHQL;
        int intVerID;
        string strCost;

        intVerID = GetProjectPlanVersionVerID(strProjectID, "Baseline");

        if (intVerID > 0)
        {
            strHQL = "Select DefaultCost From T_ImplePlan Where ProjectID = " + strProjectID + " And VerID = " + intVerID.ToString();
            strHQL += " And  End_Date <= now()";
            strHQL += " Order By DefaultCost DESC limit 1";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

            if (ds.Tables[0].Rows.Count > 0)
            {
                strCost = ds.Tables[0].Rows[0][0].ToString().Trim();

                try
                {
                    return Decimal.ToInt32(decimal.Parse(strCost));
                }
                catch (OverflowException e)
                {
                    // decimal ֵ����intֵ��Χ
                    return 0;
                }
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

    //ȡ����Ŀ��ƻ��汾�İ汾��
    public static int GetProjectPlanVersionVerID(string strProjectID, string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.Type = " + "'" + strType + "'";

        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
            return projectPlanVersion.VerID;
        }
        else
        {
            return 0;
        }
    }

    //�ж��Ƿ����ͬ������Ŀ
    public static int GetProjecCountByProjectCodeAndID(string strProjectCode, string strProjectID)
    {
        string strHQL;

        strHQL = "Select ProjectName From T_Project Where ProjectCode = " + "'" + strProjectCode + "'" + " and  ProjectID <> " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        return ds.Tables[0].Rows.Count;
    }

    //ȡ����Ŀֱ�ӷ���Ĺ�������ʵ�ʹ�ʱ
    public static string GetTotalRealManHourByProjectWorkflowStepDetail(string strWLID, string strWorkDate)
    {
        string strHQL;
        DataSet ds2;

        strHQL = "Select COALESCE(Sum(ManHour),0) From T_WorkFlowStepDetail Where WLID = " + strWLID;
        strHQL += " and to_char(CheckingTime,'yyyymmdd') = '" + strWorkDate + "'";
        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        return decimal.Parse(ds2.Tables[0].Rows[0][0].ToString()).ToString();
    }

    //ȡ����Ŀֱ�ӷ���Ĺ�������ʵ�ʷ���
    public static string GetTotalRealExpenseByProjectWorkflowStepDetail(string strWLID, string strWorkDate)
    {
        string strHQL;
        DataSet ds2;

        strHQL = "Select COALESCE(Sum(Expense),0) From T_WorkFlowStepDetail Where WLID = " + strWLID;
        strHQL += " and to_char(CheckingTime,'yyyymmdd') = '" + strWorkDate + "'";
        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepDetail");

        return decimal.Parse(ds2.Tables[0].Rows[0][0].ToString()).ToString();
    }

    //ȡ�üƻ��������͹�������ʵ�ʹ�ʱ
    public static string GetTotalRealManHourByPlan(string strPlanID)
    {
        string strHQL;

        DataSet ds1, ds2;

        strHQL = "Select COALESCE(Sum(RealManHour),0) From T_ProjectTask Where PlanID = " + strPlanID;
        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        strHQL = "Select COALESCE(Sum(ManHour),0) From T_WorkFlow Where RelatedType = 'Plan' and RelatedID = " + strPlanID;
        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        return (decimal.Parse(ds1.Tables[0].Rows[0][0].ToString()) + decimal.Parse(ds2.Tables[0].Rows[0][0].ToString())).ToString();
    }

    //ȡ�üƻ��������͹�������ʵ�ʷ���
    public static string GetTotalRealExpenseByPlan(string strPlanID)
    {
        string strHQL;

        DataSet ds1, ds2;

        strHQL = "Select COALESCE(Sum(Expense),0) From T_ProjectTask Where PlanID = " + strPlanID;
        ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectTask");

        strHQL = "Select COALESCE(Sum(Expense),0) From T_WorkFlow Where RelatedType = 'Plan' and RelatedID = " + strPlanID;
        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        return (decimal.Parse(ds1.Tables[0].Rows[0][0].ToString()) + decimal.Parse(ds2.Tables[0].Rows[0][0].ToString())).ToString();
    }

    public static void AddConstractPayAmountToProExpense(string strProjectID, string strPayID, string strAccountCode, string strAccount, string strDecription, decimal deAmount, string strCurrencyType, string strUserCode, string strUserName)
    {
        string strProjectCurrency;
        decimal deProjectCurrencyExchangeRate = 1, deConstractCurrencyExchangeRate = 1;

        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        ProExpense proExpense = new ProExpense();

        strProjectCurrency = ShareClass.GetProject(strProjectID).CurrencyType.Trim();
        deProjectCurrencyExchangeRate = ShareClass.GetExchangeRateByCurrencyType(strProjectCurrency);

        deConstractCurrencyExchangeRate = ShareClass.GetExchangeRateByCurrencyType(strCurrencyType);

        proExpense.ProjectID = int.Parse(strProjectID);
        proExpense.TaskID = 0;
        proExpense.RecordID = 0;
        proExpense.BMPUPayID = int.Parse(strPayID);
        proExpense.UserCode = strUserCode;
        proExpense.UserName = strUserName;
        proExpense.AccountCode = strAccountCode;
        proExpense.Account = strAccount;
        proExpense.Description = strDecription;
        proExpense.Amount = (deAmount * deConstractCurrencyExchangeRate) / deProjectCurrencyExchangeRate;
        proExpense.ConfirmAmount = (deAmount * deConstractCurrencyExchangeRate) / deProjectCurrencyExchangeRate;
        proExpense.CurrencyType = strProjectCurrency;
        proExpense.EffectDate = DateTime.Now;
        proExpense.RegisterDate = DateTime.Now;
        proExpense.FinancialStaffCode = "";
        proExpense.FinancialStaffName = "";

        try
        {
            proExpenseBLL.AddProExpense(proExpense);
        }
        catch
        {
        }
    }

    public static void UpdateConstractPayAmountToProExpense(string strProjectID, string strPayID, string strAccountCode, string strAccount, string strDecription, decimal deAmount, string strCurrencyType, string strUserCode, string strUserName)
    {
        string strHQL;

        strHQL = "Delete From T_ProExpense Where ConstractPayID = " + strPayID;
        ShareClass.RunSqlCommand(strHQL);

        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        ProExpense proExpense = new ProExpense();

        proExpense.ProjectID = int.Parse(strProjectID);
        proExpense.TaskID = 0;
        proExpense.RecordID = 0;
        proExpense.BMPUPayID = int.Parse(strPayID);
        proExpense.UserCode = strUserCode;
        proExpense.UserName = strUserName;
        proExpense.AccountCode = strAccountCode;
        proExpense.Account = strAccount;
        proExpense.CurrencyType = strCurrencyType;
        proExpense.Description = strDecription;
        proExpense.Amount = deAmount;
        proExpense.ConfirmAmount = deAmount;
        proExpense.EffectDate = DateTime.Now;
        proExpense.RegisterDate = DateTime.Now;
        proExpense.FinancialStaffCode = "";
        proExpense.FinancialStaffName = "";

        try
        {
            proExpenseBLL.AddProExpense(proExpense);
        }
        catch
        {
        }
    }

    #endregion ��Ŀ��ز�������

    #region ��Ŀ���ü���

    //ɾ�������ĵ�
    public static void DeleteMoreDocByDataGrid(DataGrid dataGrid1)
    {
        string strHQL;

        string strDocID;

        for (int i = 0; i < dataGrid1.Items.Count; i++)
        {
            if (((CheckBox)(dataGrid1.Items[i].FindControl("CB_Select"))).Checked == true)
            {
                strDocID = dataGrid1.Items[i].Cells[0].Text.Trim();

                try
                {
                    strHQL = "Update T_Document Set Status = 'Deleted' Where DocID = " + strDocID;
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {
                }
            }
        }
    }

    //�����Ӧ��Ŀ��ĿԤ����û�г�֧
    public static bool CheckProjectExpenseBudget(string strProjectID, string strAccount, decimal deExpense)
    {
        string strHQL;
        IList lst;

        decimal deProBudget, deProAccountBudget, deSumAccountAmount;

        if (strProjectID == "1")
        {
            return true;
        }

        try
        {
            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);

            Project project = (Project)lst[0];
            deProBudget = project.Budget;

            strHQL = "From ProjectBudget as projectBudget Where projectBudget.ProjectID = " + strProjectID + " and projectBudget.Account = " + "'" + strAccount + "'";
            ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
            lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
            if (lst.Count > 0)
            {
                ProjectBudget projectBudget = (ProjectBudget)lst[0];
                deProAccountBudget = projectBudget.Amount;
            }
            else
            {
                deProAccountBudget = 0;
            }

            strHQL = "Select COALESCE(Sum(ConfirmAmount),0) From T_ProExpense Where ProjectID = " + strProjectID + " and Account = " + "'" + strAccount + "'";
            strHQL += " Group By Account";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");
            if (ds.Tables[0].Rows.Count > 0)
            {
                deSumAccountAmount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deSumAccountAmount = 0;
            }
            deSumAccountAmount += deExpense;

            if (deSumAccountAmount > deProAccountBudget & deProAccountBudget != 0)
            {
                return false;
            }

            if (deSumAccountAmount > deProBudget)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    //�����Ŀ���ʸ������뵥���Ӧ��Ŀ��ĿԤ����û�г�֧
    public static bool CheckProjectExpenseBudgetByProjectMaterialPayApplicant(string strProjectID, string strAccount, decimal deExpense)
    {
        string strHQL;
        IList lst;

        decimal deProBudget, deProAccountBudget, deSumAccountAmount;

        if (strProjectID == "1")
        {
            return true;
        }

        try
        {
            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);

            Project project = (Project)lst[0];
            deProBudget = project.Budget;

            strHQL = "From ProjectBudget as projectBudget Where projectBudget.ProjectID = " + strProjectID + " and projectBudget.Account = " + "'" + strAccount + "'";
            ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
            lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
            if (lst.Count > 0)
            {
                ProjectBudget projectBudget = (ProjectBudget)lst[0];
                deProAccountBudget = projectBudget.Amount;
            }
            else
            {
                deProAccountBudget = 0;
            }

            strHQL = "Select COALESCE(Sum(Amount),0) From T_ProjectMaterialPaymentApplicantDetail Where AOID In (Select AOID From T_ProjectMaterialPaymentApplicant Where ProjectID = " + strProjectID + ")";
            strHQL += " and AccountName = " + "'" + strAccount + "'";
            strHQL += " Group By AccountName";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMaterialPaymentApplicantDetail");
            if (ds.Tables[0].Rows.Count > 0)
            {
                deSumAccountAmount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                deSumAccountAmount = 0;
            }
            deSumAccountAmount += deExpense;

            if (deSumAccountAmount > deProAccountBudget & deProAccountBudget != 0)
            {
                return false;
            }

            if (deSumAccountAmount > deProBudget)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }


    //�����ð���Ŀ�ƻ����ȸ��ĵ�ǰʱ����Ŀ��ɽ���
    public static void UpdateProjectScheduleByActivityPlanSchedule(string strProjectID)
    {
        string strHQL;
        int intVerID;
        string strProjectType, strImpact, strSchedule;
        DataSet ds1, ds2;

        intVerID = GetProjectPlanVersionVerID(strProjectID, "InUse");

        if (intVerID > 0)
        {
            strHQL = string.Format(@"Select Percent_Done From T_ImplePlan Where ProjectID = {0} and VerID = {1} and Parent_ID = 0", strProjectID, intVerID);
            ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

            if (ds1.Tables[0].Rows.Count > 0)
            {
                strSchedule = ds1.Tables[0].Rows[0][0].ToString().Trim();

                Project project = ShareClass.GetProject(strProjectID);
                strProjectType = project.ProjectType.Trim();

                strHQL = string.Format(@"Select ProgressByDetailImpact From T_Project Where ProjectID = {0}", strProjectID);
                ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectType");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strImpact = ds2.Tables[0].Rows[0][0].ToString().Trim();

                    if (strImpact == "YES")
                    {
                        try
                        {
                            strHQL = string.Format(@"Update T_Project Set FinishPercent = {0} Where ProjectID = {1}", strSchedule, strProjectID);
                            ShareClass.RunSqlCommand(strHQL);
                        }
                        catch (Exception err)
                        {
                            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                        }
                    }
                }
            }
        }

    }


    //������Ŀ����
    public static void FinishPercentPicture(DataGrid dataGrid, int intCellNumber)
    {
        string strProjectID;
        int intWidth;
        int i;

        try
        {

            for (i = 0; i < dataGrid.Items.Count; i++)
            {
                strProjectID = dataGrid.Items[i].Cells[intCellNumber].Text.Trim();

                intWidth = int.Parse((((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_FinishPercent")).Text));

                if (intWidth > 30)
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_FinishPercent")).Width = (Unit)intWidth;
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_FinishPercent")).Width = (Unit)(intWidth + 30);
                }

                if (decimal.Parse(((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_FinishPercent")).Width.ToString().Replace("px", "")) > 70)
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_FinishPercent")).Width = (Unit)70;
                }

                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_FinishPercent")).Text = "  " + intWidth.ToString() + "%";
                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_FinishPercent")).ToolTip = "  " + intWidth.ToString() + "%";

                intWidth = ShareClass.GetProjectDefaultFinishPercent(strProjectID);
                if (intWidth > 30)
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefaultPercent")).Width = (Unit)(intWidth);
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefaultPercent")).Width = (Unit)(intWidth + 30);
                }
                if (decimal.Parse(((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefaultPercent")).Width.ToString().Replace("px", "")) > 100)
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefaultPercent")).Width = (Unit)100;
                }

                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefaultPercent")).Text = "  " + intWidth.ToString() + "%";
                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefaultPercent")).ToolTip = "  " + intWidth.ToString() + "%";

                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DefaultPercent")).Width = (Unit)100;
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + HttpContext.Current.Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

    }

    //������Ŀ������Ԥ�����
    public static void FinChargePercentByRow(DataGrid dataGrid, int intCellNumber)
    {
        string strProjectID;
        decimal deBudget, deRealCharge;
        decimal deChargePercent;
        int i;
        string strHQL;
        IList lst;

        try
        {

            ProjectBLL projectBLL = new ProjectBLL();
            Project project = new Project();

            ProRealChargeBLL proRealChargeBLL = new ProRealChargeBLL();
            ProRealCharge proRealCharge = new ProRealCharge();

            for (i = 0; i < dataGrid.Items.Count; i++)
            {
                strProjectID = dataGrid.Items[i].Cells[intCellNumber].Text.Trim();

                strHQL = "from Project as project where project.ProjectID = " + strProjectID;
                lst = projectBLL.GetAllProjects(strHQL);
                project = (Project)lst[0];

                deBudget = project.Budget;

                //ʵ�ʷ��ú�Ԥ��Ա�
                strHQL = "from ProRealCharge as proRealCharge where proRealCharge.ProjectID = " + strProjectID;
                lst = proRealChargeBLL.GetAllProRealCharges(strHQL);
                if (lst.Count == 0)
                {
                    deRealCharge = 0;
                    deChargePercent = 0;
                }
                else
                {
                    proRealCharge = (ProRealCharge)lst[0];
                    deRealCharge = proRealCharge.RealCharge;

                    if (deBudget == 0)
                    {
                        deChargePercent = deRealCharge;
                    }
                    else
                    {
                        deChargePercent = (deRealCharge / deBudget) * 100;
                    }
                }

                if (deChargePercent > 30 & deChargePercent <= 100)
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RealChargePercent")).Width = (Unit)deChargePercent;
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RealChargePercent")).Width = (Unit)(deChargePercent + 30);
                }
                if (deChargePercent > 70)
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RealChargePercent")).Width = (Unit)(70);
                }

                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RealChargePercent")).Text = "  " + int.Parse(deChargePercent.ToString("#0")) + "%";
                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RealChargePercent")).ToolTip = "  " + deRealCharge.ToString("#0.00") + "--" + deBudget.ToString("#0.00");

                //((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BudgetPercent")).Text = deBudget.ToString("#0.00");
                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BudgetPercent")).ToolTip = "  " + deRealCharge.ToString("#0.00") + "--" + deBudget.ToString("#0.00");

                ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BudgetPercent")).Width = (Unit)(100);

                if (deRealCharge > deBudget)
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RealChargePercent")).BackColor = Color.Red;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BudgetPercent")).BackColor = Color.Red;

                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_RealChargePercent")).ForeColor = Color.Yellow;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BudgetPercent")).ForeColor = Color.Yellow;
                }
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + HttpContext.Current.Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //������Ŀʱ��ͳ�������
    public static void SetProjectStartAndEndTime(DataGrid dataGrid, int intCellNumber)
    {
        int i;
        DateTime dtNowDate, dtBeginDate, dtEndDate;
        string strProjectID, strProjectStatus, strProjectStatusValue;

        for (i = 0; i < dataGrid.Items.Count; i++)
        {
            strProjectID = dataGrid.Items[i].Cells[intCellNumber].Text.Trim();

            Project Project = ShareClass.GetProject(strProjectID);

            strProjectStatus = Project.Status.Trim();
            strProjectStatusValue = Project.StatusValue.Trim();

            dtBeginDate = Project.BeginDate;
            dtEndDate = Project.EndDate;
            dtNowDate = DateTime.Now;

            TimeSpan sp = dtNowDate.Subtract(dtEndDate);
            int intDays = sp.Days;

            ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BeginDate")).Text = dtBeginDate.ToString("yyyy-MM-dd");
            ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_EndDate")).Text = dtEndDate.ToString("yyyy-MM-dd");

            if (intDays > 0)
            {
                if (strProjectStatus == "CaseClosed" | strProjectStatus == "Cancel")
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ChaoQi").ToString().Trim();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = "0";
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).BackColor = Color.White;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DayUnit")).ForeColor = Color.Green;
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ChaoQi").ToString().Trim();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = intDays.ToString();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BeginDate")).BackColor = Color.Red;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_EndDate")).BackColor = Color.Red;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_BeginDate")).ForeColor = Color.Yellow;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_EndDate")).ForeColor = Color.Yellow;
                }
            }
            else
            {
                if (strProjectStatus == "CaseClosed" | strProjectStatus == "Cancel")
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ShengYu").ToString().Trim();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = "0";
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).BackColor = Color.White;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DayUnit")).ForeColor = Color.Green;
                }
                else
                {
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).Text = LanguageHandle.GetWord("ShengYu").ToString().Trim();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).Text = (0 - intDays).ToString();
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).BackColor = Color.White;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_MoreTime")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DelayDays")).ForeColor = Color.Green;
                    ((System.Web.UI.WebControls.Label)dataGrid.Items[i].FindControl("LB_DayUnit")).ForeColor = Color.Green;
                }
            }
        }
    }

    //�滻HTML���
    public static string NoHTML(string Htmlstring)
    {
        //ɾ���ű�
        Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

        //ɾ��HTML
        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
        Htmlstring.Replace("<", "");
        Htmlstring.Replace(">", "");
        Htmlstring.Replace("\r\n", "");
        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
        return Htmlstring;
    }

    //�õ���Ŀ��־����(��������ֵ��Ϊ����ֵ��
    public static int GetDailyWorkLogLength(string strWorkLog)
    {
        int intLength, intCharUpper;

        intCharUpper = GetCharUpper();

        intLength = NoHTML(strWorkLog).Length;

        if (intLength > intCharUpper)
        {
            intLength = intCharUpper;
        }

        return intLength;
    }

    //�õ��û�ÿ���ϴ�����Ŀ�ĵ���(��������ֵ��Ϊ����ֵ��
    public static int GetDailyUploadDocNumber(string strUserCode, string strProjectID)
    {
        string strHQL;
        IList lst;
        int intCount = 0, intDocUpper = 0;

        string strCurrentDate = DateTime.Now.ToString("yyyyMMdd");
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strProjectID + ")";
        strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
        strHQL += " or (document.Visible in ( 'Department','Entire'))))";
        strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedDefect.DefectID from RelatedDefect as relatedDefect where relatedDefect.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = '����' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID = " + strProjectID + "))";  
        strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + "))";
        strHQL += " or (document.RelatedType = '����' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Project' and  meeting.RelatedID = " + strProjectID + "))";  
        strHQL += " and ((document.Visible in ('����','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
        strHQL += " or (document.Visible = 'Entire' )))))";
        strHQL += " and to_char(document.UploadTime,'yyyymmdd') = " + "'" + strCurrentDate + "'";
        strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' Order by document.DocID DESC";

        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);

        intCount = lst.Count;

        intDocUpper = GetDocUpper();

        if (intCount > intDocUpper)
        {
            intCount = intDocUpper;
        }

        return intCount;
    }

    public static Decimal GetEveryCharPrice()
    {
        string strHQL;
        IList lst;
        decimal decWorkUnitBonus = 0;

        strHQL = "from DailyWorkUnitBonus as dailyWorkUnitBonus Order By dailyWorkUnitBonus.ID DESC";
        DailyWorkUnitBonusBLL dailyWorkUnitBonusBLL = new DailyWorkUnitBonusBLL();
        lst = dailyWorkUnitBonusBLL.GetAllDailyWorkUnitBonuss(strHQL);

        if (lst.Count > 0)
        {
            DailyWorkUnitBonus dailyWorkUnitBonus = (DailyWorkUnitBonus)lst[0];
            decWorkUnitBonus = dailyWorkUnitBonus.EveryCharPrice;
        }
        else
        {
            decWorkUnitBonus = 0;
        }

        return decWorkUnitBonus;
    }

    public static int GetCharUpper()
    {
        string strHQL;
        IList lst;
        int intCharUpper = 0;

        strHQL = "from DailyWorkUnitBonus as dailyWorkUnitBonus Order By dailyWorkUnitBonus.ID DESC";
        DailyWorkUnitBonusBLL dailyWorkUnitBonusBLL = new DailyWorkUnitBonusBLL();
        lst = dailyWorkUnitBonusBLL.GetAllDailyWorkUnitBonuss(strHQL);

        if (lst.Count > 0)
        {
            DailyWorkUnitBonus dailyWorkUnitBonus = (DailyWorkUnitBonus)lst[0];
            intCharUpper = dailyWorkUnitBonus.CharUpper;
        }
        else
        {
            intCharUpper = 0;
        }

        return intCharUpper;
    }

    public static Decimal GetEveryDocPrice()
    {
        string strHQL;
        IList lst;
        decimal decEverDocPrice = 0;

        strHQL = "from DailyWorkUnitBonus as dailyWorkUnitBonus Order By dailyWorkUnitBonus.ID DESC";
        DailyWorkUnitBonusBLL dailyWorkUnitBonusBLL = new DailyWorkUnitBonusBLL();
        lst = dailyWorkUnitBonusBLL.GetAllDailyWorkUnitBonuss(strHQL);

        if (lst.Count > 0)
        {
            DailyWorkUnitBonus dailyWorkUnitBonus = (DailyWorkUnitBonus)lst[0];
            decEverDocPrice = dailyWorkUnitBonus.EveryDocPrice;
        }
        else
        {
            decEverDocPrice = 0;
        }

        return decEverDocPrice;
    }

    public static int GetDocUpper()
    {
        string strHQL;
        IList lst;
        int intDocUpper = 0;

        strHQL = "from DailyWorkUnitBonus as dailyWorkUnitBonus Order By dailyWorkUnitBonus.ID DESC";
        DailyWorkUnitBonusBLL dailyWorkUnitBonusBLL = new DailyWorkUnitBonusBLL();
        lst = dailyWorkUnitBonusBLL.GetAllDailyWorkUnitBonuss(strHQL);

        if (lst.Count > 0)
        {
            DailyWorkUnitBonus dailyWorkUnitBonus = (DailyWorkUnitBonus)lst[0];
            intDocUpper = dailyWorkUnitBonus.DocUpper;
        }
        else
        {
            intDocUpper = 0;
        }

        return intDocUpper;
    }

    //ȡ����Ŀ��������
    public static string GetDocTypeName(string strDocTypeID)
    {
        DocTypeBLL docTypeBLL = new DocTypeBLL();

        string strHQL = "from DocType as docType where docType.ID = " + strDocTypeID;
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DocType docType = (DocType)lst[0];

        return docType.Type.Trim();
    }

    #endregion ��Ŀ���ü���

    #region ��������ز�������

    //����ȱʡ���ļ�����
    public static void SetDefaultDocType(string strDocType, Label LB_DocTypeID, TextBox TB_DocType)
    {
        string strHQL;
        string strDocTypeID, strDocTypeName;

        strDocType = "%" + strDocType + "%";

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = string.Format(@"from DocType as docType where docType.Type Like '{0}'", strDocType);
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        if (lst.Count > 0)
        {
            DocType docType = (DocType)lst[0];
            strDocTypeName = docType.Type.Trim();
            strDocTypeID = docType.ID.ToString();

            LB_DocTypeID.Text = strDocTypeID;
            TB_DocType.Text = strDocTypeName;
        }
    }

    //����ȱʡ�Ĺ�����ģ����
    public static void SetDefaultWorkflowTemplate(string strDocType, DropDownList DL_TemName)
    {
        string strHQL;

        strDocType = "%" + strDocType + "%";

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = string.Format(@"from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName Like '{0}'", strDocType);
        IList lst = docTypeBLL.GetAllDocTypes(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    //���ö������ȱʡ�Ĺ�����ģ��
    public static void SetDefaultWorkflowTemplateByRelateName(string strRelatedType, string strRelatedID, string strRelateName, DropDownList DL_WorkFlowTemName)
    {
        string strHQL;

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        strHQL = string.Format(@"select * from T_WorkFlowTemplate  where (TemName in (Select wftemplatename From T_RelatedWorkflowTemplate Where RelatedType = '{0}' and RelatedID = {1})
                                 Or (Type = 'DocumentReview' and Authority = 'All' )) and Visible = 'YES' Order By SortNumber
                                ", strRelatedType, strRelatedID);
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_WorkFlowTemName.DataSource = ds1;
        DL_WorkFlowTemName.DataBind();

        strHQL = string.Format(@"select * from T_WorkFlowTemplate  where (TemName in (Select wftemplatename From T_RelatedWorkflowTemplate Where RelatedType = '{0}' and RelatedID = {1}  and '{2}' like '%'|| wftemplatename ||'%' )
                              Or (Type = 'DocumentReview' and Authority = 'All' ))
                              and Visible = 'YES'", strRelatedType, strRelatedID, strRelateName);
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        if (ds2.Tables[0].Rows.Count > 0)
        {
            DL_WorkFlowTemName.SelectedValue = ds2.Tables[0].Rows[0]["TemName"].ToString().Trim();

        }

    }

    //�����ĵ����޹������������ɾ����ť
    public static void HideDataGridDeleteButtonForDocUploadPage(DataGrid dataGrid1)
    {
        string strHQL;

        string strDocID, strDocName;
        Document document;

        try
        {
            for (int i = 0; i < dataGrid1.Items.Count; i++)
            {
                strDocID = dataGrid1.Items[i].Cells[0].Text;

                document = ShareClass.GetDocumentByDocID(strDocID);
                strDocName = document.DocName.Trim();

                strHQL = string.Format(@"Select * From T_Document Where RelatedType ='Workflow' and DocName ='{0}'", strDocName);
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Document");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ((LinkButton)dataGrid1.Items[i].FindControl("LBT_Delete")).Visible = false;
                }
                else
                {
                    ((LinkButton)dataGrid1.Items[i].FindControl("LBT_Delete")).Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    //ȡ���ĵ���ع���������
    public static int GetRelatedWorkflowCountForDoc(string strRelatedType, string strRelatedID, string strRelatedName)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_WorkFlow A Where RelatedType ='{0}' and (RelatedID = {1}
	        or WLID in (Select RelatedID From T_Document Where DocID ={1} and RelatedName ='{2}'))", strRelatedType, strRelatedID, strRelatedName);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");

        return ds.Tables[0].Rows.Count;
    }

    //ȡ���ĵ�
    public static Document GetDocumentByDocID(string strDocID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.DocID = " + strDocID;
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);

        Document document = (Document)lst[0];

        return document;
    }


    //ȡ�ù�����ģ���Ƿ����Զ�����״̬
    public static string GetWorkflowTemplateIsAutoActiveStatus(string strTemName)
    {
        string strHQL;

        strHQL = string.Format(@"Select AutoActive From T_WorkflowTemplate Where TemName = '{0}'", strTemName);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkflowTemplate");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "NO";
        }
    }


    //�Զ�����������ѡ���Ҫ����Ĺ������ļ�
    public static void AddMoreWLSelectedDocumentForUploadDocPage(DataGrid dataGrid1, int intRelatedID, string strExcludeID)
    {
        string strDocID;

        try
        {

            for (int i = 0; i < dataGrid1.Items.Count; i++)
            {
                if (((CheckBox)(dataGrid1.Items[i].FindControl("CB_Select"))).Checked == true)
                {
                    strDocID = dataGrid1.Items[i].Cells[0].Text.Trim();

                    if (strDocID != strExcludeID)
                    {
                        AddWLDocumentForUploadDocPage(strDocID, intRelatedID);
                    }
                }
            }
        }
        catch
        {
        }
    }

    //�Զ�����Ҫ����Ĺ������ļ�
    public static void AddWLDocumentForUploadDocPage(string strFromDocID, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.DocID = " + strFromDocID;
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        Document document = (Document)lst[0];

        document.RelatedType = "Workflow";
        document.RelatedID = intRelatedID;
        document.RelatedName = strFromDocID;
        document.UploadTime = DateTime.Now;
        document.Status = LanguageHandle.GetWord("YiPingSheng").ToString().Trim();

        try
        {
            documentBLL.AddDocument(document);

            strHQL = string.Format(@"Update T_Document Set Status = '{0}' Where DocID = {1}", LanguageHandle.GetWord("YiPingSheng").ToString().Trim(), strFromDocID);
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }
    }

    //�����ĵ��������ͺ�ID
    public static void UpdateDocumentRelatedTypeAndRelatedID(string strDocID, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Document as document where document.DocID = " + strDocID;
        DocumentBLL documentBLL = new DocumentBLL();
        lst = documentBLL.GetAllDocuments(strHQL);
        Document document = (Document)lst[0];

        document.RelatedType = strRelatedType;
        document.RelatedID = intRelatedID;
        document.RelatedName = strDocID;

        try
        {
            documentBLL.UpdateDocument(document, int.Parse(strDocID));
        }
        catch
        {
        }
    }

    //ȡ�ù�����������״̬
    public static string GetRelatedWorkflowStatus(string strRelatedType, string strRelatedID)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_WorkFlow Where RelatedType ='{0}' and RelatedID ={1}", strRelatedType, strRelatedID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return GetStatusHomeNameByWorkflowStatus(ds.Tables[0].Rows[0]["Status"].ToString().Trim());
        }
        else
        {
            return LanguageHandle.GetWord("NoReviewed").ToString().Trim();
        }
    }

    //ȡ�ù�����������״̬
    public static string GetRelatedWorkflowStatusForDocUploadPage(string strDocName, string strDocID1)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_Document Where RelatedType ='Workflow' and DocName ='{0}' and DocID ={1}", strDocName, strDocID1);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Document");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return LanguageHandle.GetWord("YiPingSheng").ToString().Trim();
        }
        else
        {
            return LanguageHandle.GetWord("NoReviewed").ToString().Trim();
        }
    }

    //ȡ��ǰ��������
    public static string GetPriorStepLastestOperator(string strWLID, string strStepID, string strStepDetailID)
    {
        string strHQL1, strHQL2, strHQL3;
        DataSet ds1, ds2, ds3;

        string strPriorStepDetailID, strPriorStepID, strPriorOperatorName;

        strHQL1 = "Select max(ID) From T_WorkFlowStepDetail Where WLID = " + strWLID + " and StepID <> " + strStepID + " and ID <> " + strStepDetailID + " and ID < " + strStepDetailID + " and char_length(rtrim(Operation)) > 0";
        ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_WorkflowStepDetail");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            strPriorStepDetailID = ds1.Tables[0].Rows[0][0].ToString().Trim();

            if (strPriorStepDetailID != "")
            {
                strHQL2 = "Select StepID From T_WorkFlowStepDetail Where WLID = " + strWLID + " and ID = " + strPriorStepDetailID;
                ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_WorkflowStep");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strPriorStepID = ds2.Tables[0].Rows[0][0].ToString().Trim();

                    if (strPriorStepID != "")
                    {
                        //strHQL3 = "select stuff((select ','+ rtrim(A.OperatorName) from T_WorkFlowStepDetail A  Where A.WLID =" + strWLID + " and A.StepID = " + strPriorStepID + " and char_length(rtrim(Operation)) > 0 FOR xml PATH('')), 1, 1, '') as OperatorName";

                        strHQL3 = string.Format(@"select 
                            array_to_string
                                    (
                                    ARRAY(
                                            SELECT  rtrim(A.OperatorName)
                                            FROM    T_WorkFlowStepDetail A
                                            WHERE   A.WLID = B.WLID AND A.STEPID = B.STEPID
                            ),
                                    ', '
                                    ) AS group_concat
                            FROM   T_WorkFlowStepDetail B
                            WHERE B.WLID = {0} AND B.STEPID = {1}", strWLID, strPriorStepID);

                        ds3 = ShareClass.GetDataSetFromSql(strHQL3, "T_WorkflowStep");
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            strPriorOperatorName = ds3.Tables[0].Rows[0][0].ToString().Trim();
                            return strPriorOperatorName;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    //ȡ����һ��������
    public static string GetNextStepLastestOperator(string strWLID, string strStepID, string strStepDetailID)
    {
        string strHQL1, strHQL2, strHQL3;
        DataSet ds1, ds2, ds3;

        string strNextStepDetailID, strNextStepID, strNextOperatorName;

        strHQL1 = "Select min(ID) From T_WorkFlowStepDetail Where WLID = " + strWLID + " and StepID <> " + strStepID + " and ID <> " + strStepDetailID + " and ID > " + strStepDetailID;
        ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_WorkflowStepDetail");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            strNextStepDetailID = ds1.Tables[0].Rows[0][0].ToString().Trim();

            if (strNextStepDetailID != "")
            {
                strHQL2 = "Select StepID From T_WorkFlowStepDetail Where WLID = " + strWLID + " and ID = " + strNextStepDetailID;
                ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_WorkflowStep");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strNextStepID = ds2.Tables[0].Rows[0][0].ToString().Trim();

                    if (strNextStepID != "")
                    {
                        //strHQL3 = "select stuff((select ','+ rtrim(A.OperatorName) from T_WorkFlowStepDetail A  Where A.WLID =" + strWLID + " and A.StepID = " + strNextStepID + " FOR xml PATH('')), 1, 1, '') as OperatorName";

                        strHQL3 = string.Format(@"select 
                            array_to_string
                                    (
                                    ARRAY(
                                            SELECT  rtrim(A.OperatorName)
                                            FROM    T_WorkFlowStepDetail A
                                            WHERE   A.WLID = B.WLID AND A.STEPID = B.STEPID
                            ),
                                    ', '
                                    ) AS group_concat
                            FROM   T_WorkFlowStepDetail B
                            WHERE B.WLID = {0} AND B.STEPID = {1}", strWLID, strNextStepID);

                        ds3 = ShareClass.GetDataSetFromSql(strHQL3, "T_WorkflowStep");
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            strNextOperatorName = ds3.Tables[0].Rows[0][0].ToString().Trim();
                            return strNextOperatorName;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }


    //ȡ������һ��������
    public static string GetLastestStepLastestOperator(string strWLID)
    {
        string strHQL1;
        DataSet ds1;

        int i = 0;

        string strLastestOperatorName, strLastestOperatorNameList = "";


        strHQL1 = "Select distinct OperatorCode,OperatorName From T_WorkFlowStepDetail Where IsOperator = 'YES' and StepID in (Select Max(StepID) From T_WorkFlowStep Where WLID = " + strWLID + ")";
        ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_WorkflowStepDetail");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                if (i <= 6)
                {
                    strLastestOperatorName = "<a href='TTUserInforSimple.aspx?UserCode=" + ds1.Tables[0].Rows[i]["OperatorCode"].ToString().Trim() + "'>" + ds1.Tables[0].Rows[i]["OperatorName"].ToString().Trim() + "</a>,";
                    strLastestOperatorNameList += strLastestOperatorName;
                }

            }

            if (i >= 6)
            {
                strLastestOperatorNameList += "...";
            }

            return Regex.Replace(strLastestOperatorNameList, ",(?=[^,]*$)", "");
        }
        else
        {
            return "";
        }
    }


    //ȡ��ҵ�������������ID
    public static string GetBusinessRelatedWorkFlowID(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        if (lst.Count > 0)
        {
            WorkFlow workFlow = (WorkFlow)lst[0];
            return workFlow.WLID.ToString();
        }
        else
        {
            return null;
        }
    }

    //ȡ��ҵ������������̲���ģ���Ƿ����ȫ��༭
    public static string GetWorkflowTemplateStepFullAllowEditValue(string strWLType, string strRelatedType, string strRelatedID, string strStepSortNumber)
    {
        string strTemName, strAllowFullEdit;
        DataSet ds, ds1, ds2;

        string strHQL;

        strHQL = "Select TemName from T_WorkFlow where WLType = " + "'" + strWLType + "'" + " and RelatedType=" + "'" + strRelatedType + "'" + " and RelatedID = " + strRelatedID + " Order by WLID DESC";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select TemName from T_WorkFlowBackup where WLType = " + "'" + strWLType + "'" + " and RelatedType = " + "'" + strRelatedType + "'" + " and RelatedID = " + strRelatedID + " Order by WLID DESC";
            ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                strTemName = ds1.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                return "YES";
            }
        }
        else
        {
            strTemName = ds.Tables[0].Rows[0][0].ToString().Trim();
        }

        strHQL = "Select AllowFullEdit From T_WorkFlowTStepOperator Where AllowFullEdit = 'YES' and StepID In (Select StepID From T_WorkFlowTStep Where SortNumber = " + strStepSortNumber + " and TemName = '" + strTemName + "')";
        ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStepOperator");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            strAllowFullEdit = ds2.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            strAllowFullEdit = "NO";
        }

        return strAllowFullEdit;
    }

    //ȡ�ù���������
    public static string GetWorkFlowName(string strWLID)
    {
        string strHQL;

        strHQL = "Select * From T_WorkFlow where WLID = " + strWLID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        return ds.Tables[0].Rows[0]["WLName"].ToString().Trim();
    }

    //ȡ�ù���������
    public static string GetWorkFlowType(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlow where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        return ds.Tables[0].Rows[0]["WLType"].ToString().Trim();
    }

    //ȡ�ù�������������
    public static string GetWorkFlowRelatedType(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlow where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        return ds.Tables[0].Rows[0]["RelatedType"].ToString().Trim();
    }

    //ȡ�ù���������ID
    public static string GetWorkFlowRelatedID(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlow where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        return ds.Tables[0].Rows[0]["RelatedID"].ToString().Trim();
    }

    //ȡ�ù�������ǰ����ģ���Ƿ������ǩ
    public static string GetWorkflowTemplateStepAllowCurrentStepAddApprover(string strStepID)
    {
        string strHQL;

        strHQL = "Select AllowCurrentStepAddApprover From T_WorkFlowTStep Where StepID = " + strStepID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStep");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    //ȡ�ù�������һ����ģ���Ƿ������ǩ
    public static string GetWorkflowTemplateStepAllowNextStepAddApprover(string strStepID)
    {
        string strHQL;

        strHQL = "Select AllowNextStepAddApprover From T_WorkFlowTStep Where StepID = " + strStepID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStep");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    //ȡ�ô����̵��ϼ�����ID
    public static string GetParentWorklowID(string strWLID)
    {
        string strHQL;

        strHQL = "Select WFID From T_WFStepRelatedWF Where WFChildID = " + strWLID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WFStepRelatedWF");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�ô����̵��ϼ����̲���ID
    public static string GetParentWorklowStepID(string strWLID)
    {
        string strHQL;

        strHQL = "Select WFStepID From T_WFStepRelatedWF Where WFChildID = " + strWLID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WFStepRelatedWF");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "0";
        }
    }

    //������ID��ȡ�ô����̵��¼�����ID
    public static string GetChildWorklowIDByStepID(string strWLID, string strStepID)
    {
        string strHQL;

        strHQL = "Select WFChildID From T_WFStepRelatedWF Where WFID = " + strWLID + " and WFStepID = " + strStepID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WFStepRelatedWF");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "0";
        }
    }

    //BusinessForm,װ�ع�����Ϣ
    public static void LoadBusinessForm(string strRelatedType, string strRelatedID, string strTemName, System.Web.UI.HtmlControls.HtmlIframe IFrame_RelatedInformation)
    {
        string strURL;
        string strIdentifyString;

        strIdentifyString = ShareClass.GetWLTemplateIdentifyString(strTemName);

        if (strRelatedID == "")
        {
            strRelatedID = "0";
        }

        strURL = "TTRelatedDIYBusinessForm.aspx?RelatedType=" + strRelatedType + "&RelatedID=" + strRelatedID + "&IdentifyString=" + strIdentifyString;
        IFrame_RelatedInformation.Attributes.Add("src", strURL);
    }

    //BusinessForm,�����ͺ�IDȡ������ģ������
    public static string getBusinessFormTemName(string strRelatedType, string strRelatedID)
    {
        string strHQL;

        DataSet ds = null;

        strHQL = "Select TemName From T_RelatedBusinessForm Where RelatedType = '" + strRelatedType + "' and  RelatedID = " + strRelatedID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessForm");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["TemName"].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    //ȡ�ù��������͵ı��ػ�����
    public static string GetWorkflowTypeHomeName(string strWLType)
    {
        string strHQL;
        IList lst;

        strHQL = string.Format(@"Select HomeName From T_WLType Where Type = '{0}' and LangCode ='{1}'", strWLType, HttpContext.Current.Session["LangCode"].ToString());
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WLType");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return strWLType;
        }
    }

    //BusinessForm,�г�ҵ�������
    public static void LoadWorkflowType(DropDownList DL_WLType, string strLangCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from WLType as wlType ";
        strHQL += " Where wlType.LangCode =" + "'" + strLangCode + "'";
        strHQL += " and wlType.Type in (Select workFlowTemplate.Type From WorkFlowTemplate as workFlowTemplate)";
        strHQL += " order by wlType.SortNumber ASC";
        WLTypeBLL wlTypeBLL = new WLTypeBLL();
        lst = wlTypeBLL.GetAllWLTypes(strHQL);
        DL_WLType.DataSource = lst;
        DL_WLType.DataBind();
        DL_WLType.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    //BusinessForm,ȡҵ���ģ������
    public static string GetWorkTemplateType(string strTemName)
    {
        IList lst;
        string strHQL, strTemType;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strTemType = workFlowTemplate.Type.Trim();
        }
        catch
        {
            strTemType = "";
        }

        return strTemType;
    }

    //���Ĺ����������������ļ�
    //strCmdText = "select * from T_AssetPurchaseOrder where POID = " + strPOID;
    public static void UpdateWokflowRelatedXMLFile(string strTableType, string strWLID, string strWLStepDetailID, string strCmdText)
    {
        string strXMLFileName, strXMLFile, strXMLFile1;
        string strWLType;

        string strHQL;

        XMLProcess xmlProcess = new XMLProcess();

        strWLType = ShareClass.GetWorkFlowType(strWLID);

        strXMLFileName = strWLType + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        strXMLFile = "Doc\\" + "XML" + "\\" + strXMLFileName;

        strXMLFile1 = HttpContext.Current.Server.MapPath(strXMLFile);
        xmlProcess.DbToXML(strCmdText, "T_BusinessFormDataTable", strXMLFile1);

        if (strTableType == "MainTable")
        {
            strHQL = "Update T_WorkFlow Set XMLFile = '" + strXMLFile + "' Where WLID = " + strWLID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_WorkFlowStepDetail Set XMLFile = '" + strXMLFile + "' Where ID = " + strWLStepDetailID;
            ShareClass.RunSqlCommand(strHQL);
        }
        else
        {
            strHQL = "Update T_WorkFlowStepDetail Set DetailXMLFile = '" + strXMLFile + "' Where ID = " + strWLStepDetailID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    //BusinessForm��������Ӧ��ҵ���ģ��
    public static void SaveRelatedBusinessForm(string strRelatedType, string strRelatedID, string strTemName, string strAllowUpdate, string strUserCode)
    {
        string strHQL;
        string strXSNFile, strOldTemName, strOldXSNFile;

        if (strTemName != "")
        {
            strXSNFile = ShareClass.GetWorkFlowTemplateXSNFile(strTemName);

            strHQL = "Select * From T_RelatedBusinessForm Where RelatedType = '" + strRelatedType + "' and RelatedID =" + strRelatedID;
            strHQL += " and TemName = '" + strTemName + "' and XSNFile = '" + strXSNFile + "'";
            DataSet ds = GetDataSetFromSql(strHQL, "T_RelatedBusinessForm");
            if (ds.Tables[0].Rows.Count > 0)
            {
                strOldTemName = ds.Tables[0].Rows[0]["TemName"].ToString().Trim();
                strOldXSNFile = ds.Tables[0].Rows[0]["XSNFile"].ToString().Trim();

                if (strTemName != strOldTemName || strXSNFile != strOldXSNFile)
                {
                    strHQL = "Delete From T_RelatedBusinessForm Where RelatedType = '" + strRelatedType + "' and RelatedID =" + strRelatedID;
                    strHQL += " and TemName = '" + strTemName + "' and XSNFile = '" + strXSNFile + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Insert Into T_RelatedBusinessForm(RelatedType,RelatedID,TemName,XSNFile,XMLFile,AllowUpdate,OperatorCode,OperatorName,CreateTime)";
                    strHQL += " Values('" + strRelatedType + "'," + strRelatedID + ",'" + strTemName + "','" + strXSNFile + "','','" + strAllowUpdate + "','" + strUserCode + "','" + ShareClass.GetUserName(strUserCode) + "',now())";
                    ShareClass.RunSqlCommand(strHQL);
                }
            }
            else
            {
                strHQL = "Insert Into T_RelatedBusinessForm(RelatedType,RelatedID,TemName,XSNFile,XMLFile,AllowUpdate,OperatorCode,OperatorName,CreateTime)";
                strHQL += " Values('" + strRelatedType + "'," + strRelatedID + ",'" + strTemName + "','" + strXSNFile + "','','" + strAllowUpdate + "','" + strUserCode + "','" + ShareClass.GetUserName(strUserCode) + "',now())";
                ShareClass.RunSqlCommand(strHQL);
            }
        }
    }

    //BusinessForm,ȡ��ҵ���ģ������
    public static string getRelatedBusinessFormTemName(string strRelatedType, string strRelatedID)
    {
        string strHQL;

        strHQL = "Select * From T_RelatedBusinessForm Where RelatedType ='" + strRelatedType + "' and RelatedID =" + strRelatedID;
        strHQL += " Order By CreateTime DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessForm");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["TemName"].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    //BusinessForm,���������ҵ�������
    public static void InsertOrUpdateTaskAssignRecordWFXMLData(string strRelatedType, string strRelatedID, string strAssignType, string strAssignID, string strUserCode)
    {
        string strHQL;
        int i;
        string strTemName, strXSNFile, strXMLFile, strWFXMLData;

        strHQL = "Select * From T_RelatedBusinessForm Where RelatedType = '" + strRelatedType + "' and RelatedID =" + strRelatedID;
        strHQL += " Order By CreateTime ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessForm");
        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strTemName = ds.Tables[0].Rows[i]["TemName"].ToString().Trim();
            strXSNFile = ds.Tables[0].Rows[i]["XSNFile"].ToString().Trim();
            strXMLFile = ds.Tables[0].Rows[i]["XMLFile"].ToString().Trim();
            strWFXMLData = ds.Tables[0].Rows[i]["WFXMlData"].ToString();

            strHQL = "Insert Into T_RelatedBusinessForm(RelatedType,RelatedID,TemName,XSNFile,XMLFile,OperatorCode,OperatorName,CreateTime)";
            strHQL += " Values('" + strAssignType + "'," + strAssignID + ",'" + strTemName + "','" + strXSNFile + "','" + strXMLFile + "','" + strUserCode + "','" + ShareClass.GetUserName(strUserCode) + "',now())";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_RelatedBusinessForm Set WFXMLData = '" + strWFXMLData + "'";
            strHQL += " Where RelatedType='" + strAssignType + "' and  RelatedID = " + strAssignID;
            strHQL += " and TemName ='" + strTemName + "' and XSNFile = '" + strXSNFile + "'";
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    //�г����̹���ģ��
    public static void LoadWorkFlowTStepRelatedModule(Repeater RP_RelatedModule, string strWorkflowID, string strWorkflowStepID, string strWorkflowStepDetailID, string strStepGUID, string strLangCode, string strUserCode)
    {
        string strHQL;

        strHQL = string.Format(@"Select distinct B.HomeModuleName,A.PageName,A.ModuleName,A.ModuleType,3750 as WorkflowID,5819 as WorkflowStepID,6877 as WorkflowStepDetailID 
            ,MainTableCanAdd,DetailTableCanAdd,MainTableCanEdit,MainTableCanDelete,DetailTableCanEdit,DetailTableCanDelete 
            From T_WorkFlowTStepRelatedModule A,T_ProModuleLevel B  Where StepGUID = '{0}' 
            and A.ModuleName = B.ModuleName and B.LangCode = '{1}' And A.ModuleName in (Select ModuleName From T_ProModule 
            Where Visible = 'YES' and IsDeleted = 'NO' and UserCode = '{2}')
              and B.ID in (Select min(B.ID) From T_WorkFlowTStepRelatedModule A,T_ProModuleLevel B  Where StepGUID = '{0}' 
            and A.ModuleName = B.ModuleName and B.LangCode = '{1}' And A.ModuleName in (Select ModuleName From T_ProModule 
            Where Visible = 'YES' and IsDeleted = 'NO' and UserCode = '{2}')
               Group By B.ModuleName,B.PageName)", strStepGUID, strLangCode, strUserCode);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStepRelatedModule");

        RP_RelatedModule.DataSource = ds;
        RP_RelatedModule.DataBind();
    }

    //���������ģ�飬��ʼ��
    public static void InitialWorkflowRelatedModule(string strRelatedWorkflowID, string strRelatedWorkflowStepID, Button BT_CreateMain, LinkButton BT_NewMain, Button BT_CreateDetail, LinkButton BT_NewDetail, string strMainTableCanAdd, string strDetailTableCanAdd, string strMainTableCanEdit, string strDetailTableCanEdit)
    {
        //WorkFlow,������ɹ�����������ҵ����ô����ģ����ѯ����
        if (strRelatedWorkflowID != null)
        {
            if (strMainTableCanAdd == "YES")
            {
                BT_CreateMain.Visible = true;
            }
            else
            {
                BT_CreateMain.Visible = false;
            }

            if (strDetailTableCanAdd == "YES")
            {
                BT_CreateDetail.Visible = true;
            }
            else
            {
                BT_CreateDetail.Visible = false;
            }

            if (strMainTableCanEdit == "YES")
            {
                BT_NewMain.Visible = true;
            }
            else
            {
                BT_NewMain.Visible = false;
            }

            if (strDetailTableCanEdit == "YES")
            {
                BT_NewDetail.Visible = true;
            }
            else
            {
                BT_NewDetail.Visible = false;
            }
        }
    }

    //���������ģ�飬�����ʼ��
    public static void MainTableChangeWorkflowRelatedModule(string strCurrentUserCode, string strRelatedBusinessType, string strRelatedBusinessID, string strRelatedBusinessCreatorCode, string strRelatedWorkflowID, string strRelatedWorkflowStepID, string strRelatedWorkflowStepDetailID, Button BT_CreateMain, LinkButton BT_NewMain, Button BT_CreateDetail, LinkButton BT_NewDetail, string strMainTableCanEdit)
    {
        //WorkFlow,����˵��͹�������أ���ô��������״̬�����ܷ񱣴浥������
        string strWFStatus, strStepStatus;

        //������������ʱ���ж���û����ص����̼�¼
        if (strRelatedWorkflowID == null)
        {
            WorkFlowRelatedModule workFlowRelatedModule = ShareClass.getModuleToRelatedWorkflow(strRelatedBusinessType, strRelatedBusinessID);
            if (workFlowRelatedModule != null)
            {
                strRelatedWorkflowID = workFlowRelatedModule.WorkflowID.ToString();
                strRelatedWorkflowStepID = workFlowRelatedModule.WorkflowStepID.ToString();
                strRelatedWorkflowStepDetailID = workFlowRelatedModule.WorkflowStepDetailID.ToString();
            }
        }

        if (strRelatedWorkflowID != null)
        {
            strWFStatus = ShareClass.GetWorkFlowStatus(strRelatedWorkflowID);
            strStepStatus = ShareClass.GetWorkFlowStepStatus(strRelatedWorkflowStepID);
            if ((strStepStatus == "InProgress" | strStepStatus == "New") & (strWFStatus != "Passed" & strWFStatus != "CaseClosed"))
            {
                if (strWFStatus == "InProgress" & strRelatedWorkflowStepID == "0")
                {
                    BT_NewMain.Visible = false;
                }
            }
            else
            {
                BT_NewMain.Visible = false;
            }

            if (strMainTableCanEdit == "NO")
            {
                BT_NewMain.Visible = false;
            }
        }
    }

    //���������ģ�飬����ɾ��
    public static bool MainTableDeleteWorkflowRelatedModule(string strCurrentUserCode, string strRelatedBusinessCreatorCode, string strRelatedWorkflowID, string strRelatedWorkflowStepID, string strRelatedWorkflowStepDetailID, string strMainTableCanDelete)
    {
        //Workflow,������ڹ�������������ôҪִ������Ĵ���
        string strWFStatus, strStepStatus;

        if (strRelatedWorkflowID != null)
        {
            strWFStatus = ShareClass.GetWorkFlowStatus(strRelatedWorkflowID);
            strStepStatus = ShareClass.GetWorkFlowStepStatus(strRelatedWorkflowStepID);
            if (!((strStepStatus == "InProgress" | strStepStatus == "New") & (strWFStatus != "Passed" & strWFStatus != "CaseClosed")))
            {
                return false;
            }

            if (strMainTableCanDelete == "NO")
            {
                return false;
            }

            return true;
        }

        return true;
    }

    //���������ģ�飬��ϸ���ʼ��
    public static void DetailTableChangeWorkflowRelatedModule(string strCurrentUserCode, string strRelatedBusinessType, string strRelatedBusinessID, string strRelatedWorkflowID, string strRelatedWorkflowStepID, string strRelatedWorkflowStepDetailID, Button BT_CreateMain, LinkButton BT_NewMain, Button BT_CreateDetail, LinkButton BT_NewDetail, string strDetailTableCanAdd, string strDetailTableCanEdit)
    {
        //WorkFlow,����˵��͹�������أ���ô��������״̬�����ܷ񱣴浥������
        string strWFStatus, strStepStatus;

        //������������ʱ���ж���û����ص����̼�¼
        if (strRelatedWorkflowID == null)
        {
            WorkFlowRelatedModule workFlowRelatedModule = ShareClass.getModuleToRelatedWorkflow(strRelatedBusinessType, strRelatedBusinessID);
            if (workFlowRelatedModule != null)
            {
                strRelatedWorkflowID = workFlowRelatedModule.WorkflowID.ToString();
                strRelatedWorkflowStepID = workFlowRelatedModule.WorkflowStepID.ToString();
                strRelatedWorkflowStepDetailID = workFlowRelatedModule.WorkflowStepDetailID.ToString();
            }
        }

        if (strRelatedWorkflowID != null)
        {
            strWFStatus = ShareClass.GetWorkFlowStatus(strRelatedWorkflowID);
            strStepStatus = ShareClass.GetWorkFlowStepStatus(strRelatedWorkflowStepID);
            if ((strStepStatus == "InProgress" | strStepStatus == "New") & (strWFStatus != "Passed" & strWFStatus != "CaseClosed"))
            {
                if (strWFStatus == "InProgress" & strRelatedWorkflowStepID == "0")
                {
                    BT_NewDetail.Visible = false;
                }
                //else
                //{
                //    WorkFlowRelatedModule workFlowRelatedModule = ShareClass.getModuleToRelatedWorkflow(strRelatedBusinessType, strRelatedBusinessID);
                //    if (strRelatedWorkflowStepDetailID != workFlowRelatedModule.WorkflowStepDetailID.ToString())
                //    {
                //        BT_NewDetail.Visible = false;
                //    }
                //}
            }
            else
            {
                BT_NewDetail.Visible = false;
            }

            if (strDetailTableCanEdit == "NO")
            {
                BT_NewDetail.Visible = false;
            }
        }
    }

    //���������ģ�飬��ϸ��ɾ��
    public static bool DetailTableDeleteWorkflowRelatedModule(string strCurrentUserCode, string strRelatedBusinessCreatorCode, string strRelatedWorkflowID, string strRelatedWorkflowStepID, string strRelatedWorkflowStepDetailID, string strDetailTableCanDelete)
    {
        string strWFStatus, strStepStatus;
        if (strRelatedWorkflowID != null)
        {
            strWFStatus = ShareClass.GetWorkFlowStatus(strRelatedWorkflowID);
            strStepStatus = ShareClass.GetWorkFlowStepStatus(strRelatedWorkflowStepID);
            if (!((strStepStatus == "InProgress" | strStepStatus == "New") & (strWFStatus != "Passed" & strWFStatus != "CaseClosed")))
            {
                return false;
            }
            else
            {
                if (strDetailTableCanDelete == "NO")
                {
                    return false;
                }
            }

            if (strDetailTableCanDelete == "NO")
            {
                return false;
            }

            return true;
        }

        return true;
    }

    //ȡ�����̲�����ϸ��
    public static WorkFlowStepDetail GetWorkFlowStepDetail(string strWorkflowStepDetailID)
    {
        string strHQL;
        IList lst;

        strHQL = "From WorkFlowStepDetail as workFlowStepDetail Where workFlowStepDetail.ID = " + strWorkflowStepDetailID;
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);

        if (lst.Count > 0)
        {
            return (WorkFlowStepDetail)lst[0];
        }
        else
        {
            return null;
        }
    }

    //ȡ������ģ�岽��
    public static WorkFlowTStep GetWorkFlowTStep(string strTemName, int intSortNumber)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName = " + "'" + strTemName + "'" + " and workFlowTStep.SortNumber = " + intSortNumber.ToString();
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

        return workFlowTStep;
    }

    //ȡ�����̲���
    public static WorkFlowStep GetWorkFlowStep(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.StepID = " + strStepID;
        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);
        WorkFlowStep workFlowStep = (WorkFlowStep)lst[0];

        return workFlowStep;
    }

    public static string GetWorkflowTemNameByWLID(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlow where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        return ds.Tables[0].Rows[0]["TemName"].ToString().Trim();
    }

    public static string GetWorkFlowStatus(string strWLID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlow where WLID = " + strWLID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowBackup where WLID = " + strWLID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["Status"].ToString().Trim();
        }
        else
        {
            return "New";
        }
    }

    public static string GetWorkFlowStepStatus(string strStepID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlowStep where StepID = " + strStepID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStep");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowStepBackup where StepID = " + strStepID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepBackup");
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["Status"].ToString().Trim();
        }
        else
        {
            return "New";
        }
    }

    //���ģ��������̼�¼
    public static void AddModuleToRelatedWorkflow(string strWorkflowID, string strWorkflowStepID, string strWorkflowStepDetailID, string strRelatedModuleName, string strRelatedID)
    {
        if (strWorkflowID != null)
        {
            WorkFlowRelatedModuleBLL workFlowRelatedModuleBLL = new WorkFlowRelatedModuleBLL();
            WorkFlowRelatedModule workFlowRelatedModule = new WorkFlowRelatedModule();

            workFlowRelatedModule.WorkflowID = int.Parse(strWorkflowID);
            workFlowRelatedModule.WorkflowStepID = int.Parse(strWorkflowStepID);
            workFlowRelatedModule.WorkflowStepDetailID = int.Parse(strWorkflowStepDetailID);
            workFlowRelatedModule.RelatedModuleName = strRelatedModuleName;
            workFlowRelatedModule.RelatedID = strRelatedID;

            try
            {
                workFlowRelatedModuleBLL.AddWorkFlowRelatedModule(workFlowRelatedModule);
            }
            catch
            {
            }
        }
    }

    //ɾ��ģ��������̼�¼
    public static void DeleteModuleToRelatedWorkflow(string strWorkflowID, string strWorkflowStepID, string strWorkflowStepDetailID, string strRelatedModuleName, string strRelatedID)
    {
        if (strWorkflowID != null)
        {
            string strHQL;

            strHQL = "Delete From T_WorkFlowRelatedModule Where RelatedModuleName = '" + strRelatedModuleName + "' and RelatedID = '" + strRelatedID + "'";
            strHQL += " and WorkflowID = " + strWorkflowID + " and WorkflowStepID = " + strWorkflowStepID + " and WorkflowStepDetailID = " + strWorkflowStepDetailID;

            try
            {
                ShareClass.RunSqlCommand(strHQL);
            }
            catch
            {
            }
        }
    }

    //ȡ��ģ��������̼�¼
    public static WorkFlowRelatedModule getModuleToRelatedWorkflow(string strRelatedModuleName, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "From WorkFlowRelatedModule as workFlowRelatedModule Where workFlowRelatedModule.RelatedModuleName = '" + strRelatedModuleName + "' and workFlowRelatedModule.RelatedID = '" + strRelatedID + "'";
        WorkFlowRelatedModuleBLL workFlowRelatedModuleBLL = new WorkFlowRelatedModuleBLL();
        lst = workFlowRelatedModuleBLL.GetAllWorkFlowRelatedModules(strHQL);

        if (lst.Count > 0)
        {
            return (WorkFlowRelatedModule)lst[0];
        }
        else
        {
            return null;
        }
    }

    //ȡ���Ƿ��Զ��������
    public static string GetWorkflowTemplateAutoActive(string strTemName)
    {
        string strHQL;

        strHQL = "Select AutoActive From T_WorkFlowTemplate Where TemName = '" + strTemName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    //ȡ�ù�����������MainTableID
    public static int GetWorkflowMainTableID(string strWFID)
    {
        string strHQL;

        strHQL = "Select MainTableID From T_WorkFlow Where WLID = " + strWFID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Workflow");

        try
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString().Trim());
        }
        catch
        {
            return 0;
        }
    }

    //ȡ�ù�����XML�ļ�
    public static string GetWorkflowXMLFile(string strWFID)
    {
        string strHQL;

        strHQL = "Select XMLFile From T_WorkFlow Where WLID = " + strWFID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Workflow");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select XMLFile From T_WorkFlow Where WLID = " + strWFID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowBackup");
        }

        try
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch
        {
            return "";
        }
    }

    //ȡ�ù�����ģ���������
    public static string GetWLTemplateDesignType(string strTemName)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName = " + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate.DesignType.Trim();
    }

    //ȡ�ù�����ģ�崮
    public static string GetWLTemplateIdentifyString(string strTemName)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName = " + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        if (lst.Count > 0)
        {
            WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

            return workFlowTemplate.IdentifyString.Trim();
        }
        else
        {
            return "";
        }
    }

    //ȡ�ù�����ģ����
    public static string GetWLTemplateNameByIdentifyString(string strIdentifyString)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.IdentifyString = " + "'" + strIdentifyString + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        if (lst.Count > 0)
        {
            WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

            return workFlowTemplate.TemName.Trim();
        }
        else
        {
            return "";
        }
    }

    public static int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

    public static void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID, DataGrid dataGrid)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        dataGrid.DataSource = lst;
        dataGrid.DataBind();
    }

    public static string GetWorkFlowLastestXMLFile(string strTemName)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.TemName = '" + strTemName + "'";
        strHQL += " Order By workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        if (lst.Count > 0)
        {
            WorkFlow workFlow = (WorkFlow)lst[0];
            return workFlow.XMLFile.Trim();
        }
        else
        {
            return "";
        }
    }

    public static string GetWorkFlowTemplateXSNFile(string strTemName)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName = " + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        if (lst.Count > 0)
        {
            WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

            if (workFlowTemplate.XSNFile != "")
            {
                return workFlowTemplate.XSNFile.Trim();
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    public static void LoadWFTemplate(string strUserCode, string strWFType, DropDownList DL_TemName)
    {
        string strHQL;
        string strDepartCode;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        string strParentDepartString = TakeTopCore.CoreShareClass.InitialParentDepartmentStringByAuthority(strUserCode);
        string strUnderDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        strHQL = "Select TemName From T_WorkFlowTemplate Where Type = " + "'" + strWFType + "'" + " and Authority = 'All'";
        //strHQL += " and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " and (BelongDepartCode in " + strParentDepartString;
        strHQL += " Or BelongDepartCode in " + strUnderDepartString;
        strHQL += " Or TemName in (Select TemName From T_WorkFlowTemplateBusinessMember Where UserCode = '" + strUserCode + "')";
        strHQL += " Or TemName in (Select TemName From T_WorkFlowTemplateBusinessDepartment Where DepartCode in " + strParentDepartString + "))";
        strHQL += " Order by SortNumber ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        DL_TemName.DataSource = ds;
        DL_TemName.DataBind();
    }

    public static void DisplayRelatedWFStepDump(string strTemName, string strWLID, string strWFStatus, Repeater Repeater1)
    {
        string strHQL;
        int intSortNumber;
        string strWFStepStatus, strStepName;
        DataSet ds;

        strHQL = "Select A.SortNumber,COALESCE(c.Status,'InProgress') as Status,A.StepName From T_WorkFlowTStep A";
        strHQL += " Left Join T_WorkFlow B On A.TemName = B.TemName ";
        strHQL += " Left Join T_WorkFlowStep C On  A.SortNumber = C.SortNumber and B.WLID = C.WLID  ";
        strHQL += " Where A.TemName = '" + strTemName + "' and B.WLID = " + strWLID;
        strHQL += " and A.SortNumber > 0 ";
        strHQL += " Order By A.SortNumber ASC";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStep");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select A.SortNumber,COALESCE(c.Status,'InProgress') as Status,A.StepName From T_WorkFlowTStep A";
            strHQL += " Left Join T_WorkFlowBackup B On A.TemName = B.TemName ";
            strHQL += " Left Join T_WorkFlowStepBackup C On  A.SortNumber = C.SortNumber and B.WLID = C.WLID  ";
            strHQL += " Where A.TemName = '" + strTemName + "' and B.WLID = " + strWLID;
            strHQL += " and A.SortNumber > 0 ";
            strHQL += " Order By A.SortNumber ASC";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStep");
        }

        Repeater1.DataSource = ds;
        Repeater1.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    intSortNumber = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                    strWFStepStatus = ds.Tables[0].Rows[i][1].ToString().Trim();
                    strStepName = ds.Tables[0].Rows[i][2].ToString().Trim();

                    if (strWFStepStatus == "Passed")
                    {
                        ((ImageButton)Repeater1.Items[i].FindControl("IBT_WFStep")).ImageUrl = "Images/GreenDump.png";
                    }
                    else
                    {
                        ((ImageButton)Repeater1.Items[i].FindControl("IBT_WFStep")).ImageUrl = "Images/RedDump.png";
                    }
                    //}

                    ((ImageButton)Repeater1.Items[i].FindControl("IBT_WFStep")).ToolTip = intSortNumber.ToString() + " " + strStepName;
                }
            }
            catch
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ((ImageButton)Repeater1.Items[i].FindControl("IBT_WFStep")).ImageUrl = "Images/RedDump.png";

                    intSortNumber = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                    strWFStepStatus = ds.Tables[0].Rows[i][1].ToString().Trim();
                    strStepName = ds.Tables[0].Rows[i][2].ToString().Trim();

                    ((ImageButton)Repeater1.Items[i].FindControl("IBT_WFStep")).ToolTip = intSortNumber.ToString() + " " + strStepName;
                }
            }
        }
    }

    public static WorkFlowStep GetWorkFlowMaxApprovedStep(string strWLID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowStep as workFlowStep where workFlowStep.WLID = " + strWLID;
        strHQL += " Order By workFlowStep.StepID DESC";

        WorkFlowStepBLL workFlowStepBLL = new WorkFlowStepBLL();
        lst = workFlowStepBLL.GetAllWorkFlowSteps(strHQL);

        WorkFlowStep workFlowStep = new WorkFlowStep();

        if (lst.Count > 0)
        {
            workFlowStep = (WorkFlowStep)lst[0];

            return workFlowStep;
        }
        else
        {
            return workFlowStep;
        }
    }

    //ȡ�����̲����
    public static int GetWorkFlowCurrentStepSortNumber(string strStepID)
    {
        string strHQL;
        DataSet ds;

        strHQL = "Select * from T_WorkFlowStep where StepID = " + strStepID;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStep");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select * from T_WorkFlowStepBackup where StepID = " + strStepID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStepBackup");
        }

        int intSortNumber = int.Parse(ds.Tables[0].Rows[0]["SortNumber"].ToString().Trim());

        return intSortNumber;
    }

    //����������ض���״̬
    public static bool UpdateRelatedBusinessStatus(string strWFType, string strRelatedID, string strOperation)
    {
        string strHQL;

        try
        {
            if (strOperation == "Agree")
            {
                if (strWFType == "CustomerServiceReview")
                {
                    strHQL = "Update T_CustomerQuestion Set Status = 'Completed' Where ID = " + strRelatedID;
                    ShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "VehicleRequest")
                {
                    strHQL = "Update T_CarApplyForm Set Status = 'Passed' Where ID  = " + strRelatedID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }

            if (strOperation == "Cancel")
            {
                if (strWFType == "CustomerServiceReview")
                {
                    strHQL = "Update T_CustomerQuestion Set Status = 'InProgress' Where ID = " + strRelatedID;
                    ShareClass.RunSqlCommand(strHQL);
                }

                if (strWFType == "VehicleRequest")
                {
                    strHQL = "Update T_CarApplyForm Set Status = 'InProgress' Where ID  = " + strRelatedID;
                    ShareClass.RunSqlCommand(strHQL);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string GetWFDefinition(string strTemName)
    {
        IList lst;
        string strHQL, strWFDefinition;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName =" + "'" + strTemName + "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        try
        {
            strWFDefinition = workFlowTemplate.WFDefinition.Trim();
        }
        catch
        {
            strWFDefinition = "";
        }

        return strWFDefinition;
    }

    #endregion ��������ز�������

    #region �������ҵ����

    //�����ͬ��
    public static void InitialConstractTree(TreeView ConstractTreeView)
    {
        string strHQL;
        IList lst;

        string strConstractID, strConstractCode, strConstractName;

        //��Ӹ��ڵ�
        ConstractTreeView.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WoDeHeTong").ToString().Trim() + "</B>";
        node1.Target = "";
        node1.Expanded = true;
        ConstractTreeView.Nodes.Add(node1);

        Constract constract = new Constract();

        strHQL = "from Constract as constract where ";
        strHQL += " constract.ParentCode = ''";
        strHQL += " and constract.Status not in ('Archived','Deleted') ";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);


        for (int i = 0; i < lst.Count; i++)
        {
            constract = (Constract)lst[i];

            strConstractID = constract.ConstractID.ToString();
            strConstractCode = constract.ConstractCode.Trim();
            strConstractName = constract.ConstractName.Trim();

            node3 = new TreeNode();

            node3.Text = strConstractID + " " + strConstractCode + " " + strConstractName;
            node3.Target = strConstractCode;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            ConstractTreeShow(strConstractCode, node3);
            ConstractTreeView.DataBind();
        }
    }

    public static void ConstractTreeShow(string strParentCode, TreeNode treeNode)
    {
        string strHQL;
        IList lst1, lst2;

        string strConstractID, strConstractCode, strConstractName;

        ConstractBLL constractBLL = new ConstractBLL();
        Constract constract = new Constract();

        strHQL = "from Constract as constract where ";
        strHQL += " constract.ParentCode = " + "'" + strParentCode + "'";
        strHQL += " and constract.Status not in ('Archived','Deleted') ";
        strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";

        lst1 = constractBLL.GetAllConstracts(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            constract = (Constract)lst1[i];

            strConstractID = constract.ConstractID.ToString();
            strConstractCode = constract.ConstractCode.Trim();
            strConstractName = constract.ConstractName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strConstractCode;
            node.Text = strConstractID + " " + strConstractCode + " " + strConstractName;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;


            strHQL = "from Constract as constract where ";
            strHQL += " constract.ParentCode = " + "'" + strConstractCode + "'";
            strHQL += " and constract.Status not in ('Archived','Deleted') ";
            strHQL += " order by constract.SignDate DESC,constract.ConstractCode DESC";
            lst2 = constractBLL.GetAllConstracts(strHQL);

            if (lst2.Count > 0)
            {
                ConstractTreeShow(strConstractCode, node);
            }
        }
    }

    //ȡ�ú�ͬ����
    public static string GetConstractName(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Constract as constract Where constract.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractBLL constractBLL = new ConstractBLL();

        try
        {
            lst = constractBLL.GetAllConstracts(strHQL);
            Constract constract = (Constract)lst[0];
            return constract.ConstractName.Trim();
        }
        catch
        {
            return "";
        }
    }

    //ȡ�ú�ͬID��
    public static int GetConstractID(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Constract as constract Where constract.ConstractCode = " + "'" + strConstractCode + "'";
        ConstractBLL constractBLL = new ConstractBLL();

        try
        {
            lst = constractBLL.GetAllConstracts(strHQL);
            Constract constract = (Constract)lst[0];
            return constract.ConstractID;
        }
        catch
        {
            return 0;
        }
    }


    //���������ĵ�������
    public static void InitialAllDocTypeTree(TreeView TreeView3)
    {
        string strHQL, strDocTypeID, strDocType;
        IList lst;

        //��Ӹ��ڵ�
        TreeView3.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<Strong>" + LanguageHandle.GetWord("WenDangLeiXing").ToString().Trim() + "</Strong>"; ;
        node1.Target = "0";
        node1.Expanded = true;
        TreeView3.Nodes.Add(node1);

        strHQL = "from DocType as docType  where ";
        strHQL += " docType.ParentID not in (select docType.ID from DocType as docType)";
        strHQL += " order by docType.SortNumber ASC";

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        lst = docTypeBLL.GetAllDocTypes(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            docType = (DocType)lst[i];

            strDocTypeID = docType.ID.ToString();

            strDocType = docType.Type.Trim();

            node3 = new TreeNode();

            node3.Text = (i + 1).ToString() + "." + strDocType;
            node3.Target = strDocTypeID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            AllDocTypeTreeShow(strDocTypeID, node3);
            TreeView3.DataBind();
        }
    }

    public static void AllDocTypeTreeShow(string strParentID, TreeNode treeNode)
    {
        string strHQL, strDocTypeID, strDocType;
        IList lst1, lst2;

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        strHQL = "from DocType as docType  where ";
        strHQL += " docType.ParentID = " + strParentID;
        //strHQL += " order by docType.ParentID DESC";
        strHQL += " order by docType.SortNumber ASC";
        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            docType = (DocType)lst1[i];
            strDocTypeID = docType.ID.ToString();
            strDocType = docType.Type.Trim();

            TreeNode node = new TreeNode();
            node.Target = strDocTypeID;
            node.Text = (i + 1).ToString() + "." + strDocType;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from DocType as docType where docType.ParentID = " + strDocTypeID + " Order by docType.ID DESC";
            lst2 = docTypeBLL.GetAllDocTypes(strHQL);

            if (lst2.Count > 0)
            {
                AllDocTypeTreeShow(strDocTypeID, node);
            }
        }
    }

    public static void InitialAllUserDocTypeTree(TreeView TreeView3, string strUserCode)
    {
        string strHQL, strDocTypeID, strDocType, strDepartCode;
        IList lst;

        //��Ӹ��ڵ�
        TreeView3.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<Strong>" + LanguageHandle.GetWord("WenDangLeiXing").ToString().Trim() + "</Strong>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView3.Nodes.Add(node1);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group') or (docType.SaveType = 'All') ";
        strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
        strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
        strHQL += " and docType.ParentID not in (select docType.ID from DocType as docType)";
        strHQL += " order by docType.SortNumber ASC";

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        lst = docTypeBLL.GetAllDocTypes(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            docType = (DocType)lst[i];

            strDocTypeID = docType.ID.ToString();

            strDocType = docType.Type.Trim();

            node3 = new TreeNode();

            node3.Text = (i + 1).ToString() + "." + strDocType;
            node3.Target = strDocTypeID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            AllUserDocTypeTreeShow(strDocTypeID, node3, strUserCode, strDepartCode);

            TreeView3.DataBind();
        }
    }

    public static void AllUserDocTypeTreeShow(string strParentID, TreeNode treeNode, string strUserCode, string strDepartCode)
    {
        string strHQL, strDocTypeID, strDocType;
        IList lst1, lst2;

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group')  or (docType.SaveType = 'All') ";
        strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
        strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
        strHQL += " and docType.ParentID = " + strParentID;
        strHQL += " order by docType.SortNumber ASC";

        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            docType = (DocType)lst1[i];
            strDocTypeID = docType.ID.ToString();
            strDocType = docType.Type.Trim();

            TreeNode node = new TreeNode();
            node.Target = strDocTypeID;
            node.Text = (i + 1).ToString() + "." + strDocType;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from DocType as docType where docType.ParentID = " + strDocTypeID + " Order by docType.ID DESC";
            lst2 = docTypeBLL.GetAllDocTypes(strHQL);

            if (lst2.Count > 0)
            {
                AllUserDocTypeTreeShow(strDocTypeID, node, strUserCode, strDepartCode);
            }
        }
    }

    public static void InitialUserDocTypeTree(TreeView TreeView3, string strUserCode)
    {
        string strHQL, strDocTypeID, strDocType, strDepartCode;
        IList lst;

        //��Ӹ��ڵ�
        TreeView3.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<Strong>" + LanguageHandle.GetWord("WenDangLeiXing").ToString().Trim() + "</Strong>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView3.Nodes.Add(node1);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        if (strUserCode == "ADMIN")
        {
            strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group')  or (docType.SaveType = 'All')";
            strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
            strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
            strHQL += " and docType.ParentID not in (select docType.ID from DocType as docType)";
            strHQL += " order by docType.SortNumber ASC";
        }
        else
        {
            strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group')  or (docType.SaveType = 'All') ";
            strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
            strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
            strHQL += " and docType.ParentID not in (select docType.ID from DocType as docType)";
            strHQL += " and docType.Type not in ('�����','֪ʶ��')";  
            strHQL += " order by docType.SortNumber ASC";
        }

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        lst = docTypeBLL.GetAllDocTypes(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            docType = (DocType)lst[i];

            strDocTypeID = docType.ID.ToString();

            strDocType = docType.Type.Trim();

            node3 = new TreeNode();

            node3.Text = (i + 1).ToString() + "." + strDocType;
            node3.Target = strDocTypeID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            UserDocTypeTreeShow(strDocTypeID, node3, strUserCode, strDepartCode);

            TreeView3.DataBind();
        }
    }

    public static void UserDocTypeTreeShow(string strParentID, TreeNode treeNode, string strUserCode, string strDepartCode)
    {
        string strHQL, strDocTypeID, strDocType;
        IList lst1, lst2;

        DocTypeBLL docTypeBLL = new DocTypeBLL();
        DocType docType = new DocType();

        if (strUserCode == "ADMIN")
        {
            strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group')  or (docType.SaveType = 'All')";
            strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
            strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
            strHQL += " and docType.ParentID = " + strParentID;
            strHQL += " order by docType.SortNumber ASC";
        }
        else
        {
            strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group')  or (docType.SaveType = 'All') ";
            strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
            strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
            strHQL += " and docType.ParentID = " + strParentID;
            strHQL += " and docType.Type not in ('�����','֪ʶ��')";  
            strHQL += " order by docType.SortNumber ASC";
        }
        lst1 = docTypeBLL.GetAllDocTypes(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            docType = (DocType)lst1[i];
            strDocTypeID = docType.ID.ToString();
            strDocType = docType.Type.Trim();

            TreeNode node = new TreeNode();
            node.Target = strDocTypeID;
            node.Text = (i + 1).ToString() + "." + strDocType;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from DocType as docType where docType.ParentID = " + strDocTypeID + " Order by docType.ID DESC";
            lst2 = docTypeBLL.GetAllDocTypes(strHQL);

            if (lst2.Count > 0)
            {
                UserDocTypeTreeShow(strDocTypeID, node, strUserCode, strDepartCode);
            }
        }
    }

    //��������Ͷ����ĵ�������
    public static void InitialDocTypeTree(TreeView TreeView1, string strUserCode, string strRelatedType, string strRelatedID, string strRelatedName)
    {
        string strHQL;
        IList lst;

        string strDocTypeID, strTotalDocType = "", strDocType, strDepartCode;

        int j = 1;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        if (strRelatedType == "֪ʶ����")  
        {
            node1.Text = LanguageHandle.GetWord("WSCDSYWD").ToString().Trim();
        }
        else
        {
            node1.Text = strRelatedType + "��" + strRelatedID + " " + strRelatedName + " " + LanguageHandle.GetWord("WenDangLieBiao").ToString().Trim();
        }
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "from DocTypeFilter as docTypeFilter where docTypeFilter.RelatedType = " + "'" + strRelatedType + "'" + " and docTypeFilter.RelatedID = " + strRelatedID;

        if (strRelatedType == "Project")
        {
            strHQL = "from DocTypeFilter as docTypeFilter where docTypeFilter.DocType in (";
            strHQL += "Select distinct document.DocType from Document as document where (((document.RelatedType = 'Project' and document.RelatedID = " + strRelatedID + ")";
            strHQL += " and ((document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'" + ")";
            strHQL += " or (document.Visible in ( 'Department','Entire'))))";
            strHQL += " or (((document.RelatedType = 'Requirement' and document.RelatedID in (select relatedDefect.DefectID from RelatedDefect as relatedDefect where relatedDefect.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = '����' and document.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID = " + strRelatedID + "))";  
            strHQL += " or (document.RelatedType = 'Task' and document.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = 'Plan' and document.RelatedID in (select workPlan.ID from WorkPlan as workPlan where workPlan.ProjectID = " + strRelatedID + "))";
            strHQL += " or (document.RelatedType = '����' and document.RelatedID in (select meeting.ID from Meeting as meeting where meeting.RelatedType='Project' and  meeting.RelatedID = " + strRelatedID + "))";  
            strHQL += " and ((document.Visible in ('����','Department') and document.DepartCode = " + "'" + strDepartCode + "'" + " ) ";  
            strHQL += " or (document.Visible = 'Entire' )))))";
            strHQL += " and rtrim(ltrim(document.Status)) <> 'Deleted' )";
        }

        if (strRelatedType == "ProjectType")
        {
            strHQL = "from DocTypeFilter as docTypeFilter where docTypeFilter.DocType in (";
            strHQL += "Select distinct document.DocType from Document as document where document.RelatedName = " + "'" + strRelatedName + "'";
            strHQL += " and document.Status <> 'Deleted' )";
        }

        if (strRelatedType == "֪ʶ����")  
        {
            strHQL = "from DocTypeFilter as docTypeFilter where docTypeFilter.DocType in (";
            strHQL += "Select distinct document.DocType from Document as document where document.UploadManCode = " + "'" + strUserCode + "'" + " and document.DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " and document.Status <> 'Deleted' )";
        }

        DocTypeFilterBLL docTypeFilterBLL = new DocTypeFilterBLL();
        DocTypeFilter docTypeFilter = new DocTypeFilter();

        lst = docTypeFilterBLL.GetAllDocTypeFilters(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            docTypeFilter = (DocTypeFilter)lst[i];

            strDocTypeID = docTypeFilter.DocTypeID.ToString();

            strDocType = docTypeFilter.DocType.Trim();

            if (strTotalDocType.IndexOf(strDocType) <= -1)
            {
                strTotalDocType += strDocType + ",";

                node3 = new TreeNode();

                node3.Text = j.ToString() + "." + strDocType;
                node3.Target = strDocTypeID;
                node3.Expanded = true;

                j++;

                node1.ChildNodes.Add(node3);

                TreeView1.DataBind();
            }
        }
    }

    //ȡ���ļ������б�
    public static IList GetDocTypeList(string strUserCode)
    {
        string strHQL, strDepartCode;
        IList lst;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        if (strUserCode == "ADMIN")
        {
            strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group') or (docType.SaveType = 'All')";
            strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
            strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
            strHQL += " and docType.ParentID not in (select docType.ID from DocType as docType)";
            strHQL += " order by docType.SortNumber ASC";
        }
        else
        {
            strHQL = "from DocType as docType  where ((docType.SaveType = 'Company') or (docType.SaveType = 'Group')  or (docType.SaveType = 'All')";
            strHQL += " or (docType.UserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " or (docType.SaveType = 'Department' and docType.UserCode in (Select projectMember.UserCode from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'" + "))";
            strHQL += " or (docType.SaveType not in ('All','Group','Company','Individual','Department') and docType.SaveType in (select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")))";
            strHQL += " and docType.ParentID not in (select docType.ID from DocType as docType)";
            strHQL += " and docType.Type not in ('�����','֪ʶ��')";  
            strHQL += " order by docType.SortNumber ASC";
        }
        DocTypeBLL docTypeBLL = new DocTypeBLL();
        lst = docTypeBLL.GetAllDocTypes(strHQL);

        return lst;
    }

    //ȡ���ĵ����͵��ϼ�����
    public static string getDocParentTypeByID(string strTypeID)
    {
        string strHQL;
        string strParentTypeID;

        try
        {
            strHQL = "Select ParentID From T_DocType Where ID = " + strTypeID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DocType");

            strParentTypeID = ds.Tables[0].Rows[0][0].ToString().Trim();

            strHQL = "Select Type From T_DocType Where ID = " + strParentTypeID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_DocType");

            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// ���忢��������ʾ����Ŀ���ṹ By LiuJianping 2013-09-13
    /// </summary>
    /// <param name="TreeView1"></param>
    /// <param name="strUserCode"></param>
    public static void InitialInvolvedProjectComTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProject;
        string strProjectIDString = "";
        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<A href=TTCompletionDataManage.aspx?TargetID=Project_0 Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        node1.Target = "Project" + "_" + "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "Select distinct ProjectID,ProjectName from V_ProRelatedUser Where UserCode = '" + strUserCode + "' Order by ProjectID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_ProRelatedUser");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i]["ProjectID"].ToString().Trim();
            strProject = ds.Tables[0].Rows[i]["ProjectName"].ToString().Trim();

            if (strProjectIDString.IndexOf(strProjectID + ",") >= 0)
            {
                continue;
            }
            else
            {
                strProjectIDString += strProjectID + ",";
            }

            node3 = new TreeNode();
            node3.Text = "<A href=TTCompletionDataManage.aspx?TargetID=Project_" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);

            TreeView1.DataBind();
        }
    }

    /// <summary>
    /// ���ݼ���������ȡԤ�����  2013-11-15  By LiuJianping
    /// </summary>
    /// <param name="dg">DataGrid�ؼ�</param>
    /// <param name="strDepartCode">���ű���-�ǿ�</param>
    /// <param name="strAccountName">��ƿ�Ŀ-�ɿ�</param>
    /// <param name="strYearNum">���-�ǿ� Ϊ�յĻ�����0����</param>
    /// <param name="strMonthNum">�·�-�ǿգ�0�����</param>
    /// <param name="strType">�������� �������������֣�������ΪԤ��������õı�׼��ȣ�������Ϊʵ��Ԥ����ü�¼��������</param>
    public static void GetBMBaseDataList(ref DataGrid dg, string strDepartCode, string strAccountName, int strYearNum, int strMonthNum, string strType)
    {
        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
                "bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.Type='" + strType + "' ";
        if (strYearNum != 0)
        {
            strHQL += "and bDBaseData.YearNum='" + strYearNum.ToString() + "' ";
        }

        BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
        IList lst = bMBaseDataBLL.GetAllBDBaseDatas(strHQL);

        dg.DataSource = lst;
        dg.DataBind();
    }

    //ȫ����Ŀ��������ɾ���ߵ�
    public static void InitialPrjectTreeWithDeleteLine(TreeView TreeView1)
    {
        string strHQL;
        IList lst;

        string strProjectID, strProject;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();

        node1.Text = "<B>1." + LanguageHandle.GetWord("ZongXiangMu").ToString().Trim() + " </B>";
        node1.Target = "1";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where ";
        strHQL += " project.ParentID = 1 Or project.ParentID = 0";
        strHQL += " And project.ProjectID <> 1";
        strHQL += " order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();
        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString();
            if (project.ProjectClass.Trim() == "ģ����Ŀ")  
            {
                strProject = "[<font size='2'  color='#FF0000'>" + LanguageHandle.GetWord("MoBan").ToString().Trim() + "</font>]" + project.ProjectName.Trim();
            }
            else
            {
                strProject = project.ProjectName.Trim();
            }

            node2 = new TreeNode();

            if (project.Status.Trim() == "Deleted")
            {
                node2.Text = "<strike><font size='2'  color='#FF0000'>" + strProjectID + "." + strProject + "</font></strike>";
            }
            else
            {
                node2.Text = strProjectID + "." + strProject;
            }
            node2.Target = strProjectID;
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);
            TreeShowWithDeleteLine(strProjectID, node2);
            TreeView1.DataBind();
        }
    }

    public static void TreeShowWithDeleteLine(string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            if (project.ProjectClass.Trim() == "ģ����Ŀ")  
            {
                strProject = "[<font size='2'  color='#FF0000'>" + LanguageHandle.GetWord("MoBan").ToString().Trim() + "</font>]" + project.ProjectName.Trim();
            }
            else
            {
                strProject = project.ProjectName.Trim();
            }

            TreeNode node = new TreeNode();
            node.Target = strProjectID;

            if (project.Status.Trim() == "Deleted")
            {
                node.Text = "<strike><font size='2'  color='#FF0000'>" + strProjectID + "." + strProject + "</font></strike>";
            }
            else
            {
                node.Text = strProjectID + "." + strProject;
            }

            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID + " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                TreeShowWithDeleteLine(strProjectID, node);
            }
        }
    }

    //����������Ŀ��
    public static void InitialAllProjectTree(TreeView TreeView1, string strDepartString)
    {
        string strHQL;
        IList lst;

        string strProjectID, strProject;
        String strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_0 Target=Right><B> " + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B></a>";
        node1.Target = "Project" + "_" + "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where ";
        strHQL += " project.ParentID = 1";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node3 = new TreeNode();
            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            AllProjectTreeShow(strProjectID, node3, strDepartString);

            TreeView1.DataBind();
        }
    }

    public static void AllProjectTreeShow(string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID;
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + "." + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID;
            strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                AllProjectTreeShow(strProjectID, node, strDepartString);
            }
        }
    }

    //����������Ŀ�� FOR YYUP
    public static void InitialAllProjectTree_YYUP(TreeView TreeView1, string strDepartString)
    {
        string strHQL;
        IList lst;

        string strProjectID, strProject, strProductLineRelated;
        String strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        strProductLineRelated = ShareClass.GetDepartSuperUserRelatedProductLineFromUserCode(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_0 Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B></a>";
        node1.Target = "Project" + "_" + "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where ";
        strHQL += "  project.ParentID = 1 ";
        strHQL += "  and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";

        if (strProductLineRelated == "YES")
        {
            strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
            strHQL += " (Select departSuperUserRelatedProductLine.ProductLineName From DepartSuperUserRelatedProductLine as departSuperUserRelatedProductLine Where departSuperUserRelatedProductLine.UserCode = " + "'" + strUserCode + "'" + "))";
        }

        strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node3 = new TreeNode();
            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            AllProjectTreeShow_YYUP(strProjectID, node3, strDepartString);

            TreeView1.DataBind();
        }
    }

    public static void AllProjectTreeShow_YYUP(string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID;
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + "." + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID;
            strHQL += "  and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                AllProjectTreeShow_YYUP(strProjectID, node, strDepartString);
            }
        }
    }

    //����ֱ�ӳ�Ա����Ŀ��
    public static void InitialMembersProjectTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL;
        IList lst;

        string strProjectID, strProject, strProductLineRelated, strOperatorDepartCode;

        strProductLineRelated = ShareClass.GetDepartRelatedProductLineFromUserCode(strUserCode);
        strOperatorDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WDZJCYDXMHTMDZXM").ToString().Trim() + " </B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'" + " and project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ") ";
        strHQL += " and project.Status not in ('Deleted','Archived') ";
        strHQL += " order by project.ProjectID DESC";

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            MembersProjectTreeShow(strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    public static void MembersProjectTreeShow(string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID + " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                MembersProjectTreeShow(strProjectID, node);
            }
        }
    }

    //����ֱ�ӳ�Ա����Ŀ�� FOR YYUP
    public static void InitialMembersProjectTree_YYUP(TreeView TreeView1, string strUserCode)
    {
        string strHQL;
        IList lst;

        string strProjectID, strProject, strProductLineRelated, strOperatorDepartCode;

        strProductLineRelated = ShareClass.GetDepartRelatedProductLineFromUserCode(strUserCode);
        strOperatorDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WDZJCYDXMHTMDZXM").ToString().Trim() + " </B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'" + " and project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.ProjectVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ") ";

        if (strProductLineRelated == "YES")
        {
            strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
            strHQL += " (Select departRelatedProductLine.ProductLineName From DepartRelatedProductLine as departRelatedProductLine Where departRelatedProductLine.DepartCode = " + "'" + strOperatorDepartCode + "'" + "))";
        }

        strHQL += " and project.Status not in ('Deleted','Archived') ";
        strHQL += " order by project.ProjectID DESC";

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            MembersProjectTreeShow_YYUP(strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    public static void MembersProjectTreeShow_YYUP(string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID + " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                MembersProjectTreeShow_YYUP(strProjectID, node);
            }
        }
    }

    //�����ҵĵ���Ŀ��
    public static void InitialMyProjectTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProject;
        IList lst;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WDXMHTMDZXM").ToString().Trim() + " </B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where project.PMCode = " + "'" + strUserCode + "'";
        strHQL += " and project.ParentID not in (select project.ProjectID from Project as project where project.PMCode = " + "'" + strUserCode + "'" + ")";
        strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            MyProjectTreeShow(strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    public static void MyProjectTreeShow(string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID + " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                MyProjectTreeShow(strProjectID, node);
            }
        }
    }

    //����������Ŀ�ĵ���
    public static string InitialAllProjectDocTree(TreeView TreeView1, string strUserCode, string strQueryCount, string strOperationType, string strMinProjectID, string strMaxProjectID)
    {
        string strHQL;
        string strDepartString;
        string strProjectID, strProject;

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_0 Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        node1.Target = "Project" + "_" + "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strOperationType == "All")
        {
            strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

            strHQL += " And Status Not in ('Deleted','Archived')";
            strHQL += " Order By ProjectID DESC limit " + strQueryCount;
        }
        else
        {
            if (strOperationType == "Piror")
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

                strHQL += " And ProjectID > " + strMaxProjectID;
                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID ASC limit " + strQueryCount;
            }
            else
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";
                strHQL += " And ProjectID < " + strMinProjectID;

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID DESC limit " + strQueryCount;
            }
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i][0].ToString().Trim();
            strProject = ds.Tables[0].Rows[i][1].ToString().Trim();

            node3 = new TreeNode();
            node3.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            //AllProjectDocTreeShowIncludeAll(strUserCode, strProjectID, node3);

            if (getChildProjectNumber(strProjectID, strDepartString) > 0)
            {
                node4 = new TreeNode();
                node4.Text = "����Ŀ";  
                node4.Target = strProjectID + "_ChildrenProject";
                node4.Expanded = false;
                node3.ChildNodes.Add(node4);
                AllProjectDocTreeShowIncludeChild(strProjectID, node4, strDepartString);
            }

            TreeView1.DataBind();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (strOperationType == "All" | strOperationType == "Next")
            {
                strMaxProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMinProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }
            else
            {
                strMinProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMaxProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }

            return strMinProjectID + "-" + strMaxProjectID;
        }
        else
        {
            return "";
        }
    }

    //�����Ŀ�Ƿ��������Ŀ
    public static int getChildProjectNumber(string strParentProjectID, string strDepartString)
    {
        string strHQL;

        strHQL = "Select * From T_Project Where ParentID = " + strParentProjectID;
        strHQL += " and Status not in ('Deleted','Archived')";
        strHQL += " and PMCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows.Count;
        }
        else
        {
            return 0;
        }
    }

    public static void AllProjectDocTreeShowIncludeChild(string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL;
        IList lst1, lst2;

        string strProjectID, strProject;
        String strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        TreeNode node4 = new TreeNode();

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived')";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";

        strHQL += " order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            if (getChildProjectNumber(strProjectID, strDepartString) > 0)
            {
                node4 = new TreeNode();
                node4.Text = "����Ŀ";  
                node4.Target = strProjectID + "_ChildrenProject";
                node4.Expanded = false;
                node.ChildNodes.Add(node4);
                AllProjectDocTreeShowIncludeChild(strProjectID, node4, strDepartString);
            }
        }
    }

    //����������Ŀ����ҳ��ѯ��
    public static string InitialAllProjectTreeForPageFind(TreeView TreeView1, string strUserCode, string strQueryCount, string strOperationType, string strMinProjectID, string strMaxProjectID)
    {
        string strHQL;
        string strDepartString;
        string strProjectID, strProject;

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B>";
        node1.Target = "1";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strOperationType == "All")
        {
            strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

            strHQL += " And Status Not in ('Deleted','Archived')";
            strHQL += " Order By ProjectID DESC limit " + strQueryCount;
        }
        else
        {
            if (strOperationType == "Piror")
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

                strHQL += " And ProjectID > " + strMaxProjectID;
                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID ASC limit " + strQueryCount;
            }
            else
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";
                strHQL += " And ProjectID < " + strMinProjectID;

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID DESC limit " + strQueryCount;
            }
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i][0].ToString().Trim();
            strProject = ds.Tables[0].Rows[i][1].ToString().Trim();

            node3 = new TreeNode();
            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;

            node1.ChildNodes.Add(node3);
            node3.Expanded = false;

            AllProjectTreeShowForPageFind(strProjectID, node3, strDepartString);

            TreeView1.DataBind();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (strOperationType == "All" | strOperationType == "Next")
            {
                strMaxProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMinProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }
            else
            {
                strMinProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMaxProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }

            return strMinProjectID + "-" + strMaxProjectID;
        }
        else
        {
            return "";
        }
    }

    public static void AllProjectTreeShowForPageFind(string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL;
        IList lst1;

        string strProjectID, strProject;
        String strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        TreeNode node4 = new TreeNode();

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived')";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";

        strHQL += " order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + "." + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            AllProjectTreeShowForPageFind(strProjectID, node, strDepartString);
        }
    }

    //����������Ŀ����ҳ��ѯ�� For YYUP
    public static string InitialAllProjectTreeForPageFind_YYUP(TreeView TreeView1, string strUserCode, string strQueryCount, string strOperationType, string strMinProjectID, string strMaxProjectID)
    {
        string strHQL;
        string strDepartString;

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        string strProjectID, strProject, strProductLineRelated;

        strProductLineRelated = ShareClass.GetDepartSuperUserRelatedProductLineFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B>";
        node1.Target = "1";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strOperationType == "All")
        {
            strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

            if (strProductLineRelated == "YES")
            {
                strHQL += " and ProjectID in (Select ProjectID From T_Project_YYUP  Where ProductLine in ";
                strHQL += " (Select ProductLineName From T_DepartSuperUserRelatedProductLine Where UserCode = " + "'" + strUserCode + "'" + "))";
            }

            strHQL += " And Status Not in ('Deleted','Archived')";
            strHQL += " Order By ProjectID DESC limit " + strQueryCount;
        }
        else
        {
            if (strOperationType == "Piror")
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

                if (strProductLineRelated == "YES")
                {
                    strHQL += " and ProjectID in (Select ProjectID From T_Project_YYUP Where ProductLine in ";
                    strHQL += " (Select ProductLineName From T_DepartSuperUserRelatedProductLine  Where UserCode = " + "'" + strUserCode + "'" + "))";
                }

                strHQL += " And ProjectID > " + strMaxProjectID;
                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID ASC limit " + strQueryCount;
            }
            else
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";
                strHQL += " And ProjectID < " + strMinProjectID;

                if (strProductLineRelated == "YES")
                {
                    strHQL += " and ProjectID in (Select ProjectID From T_Project_YYUP Where ProductLine in ";
                    strHQL += " (Select ProductLineName From T_DepartSuperUserRelatedProductLine Where UserCode = " + "'" + strUserCode + "'" + "))";
                }

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID DESC limit " + strQueryCount;
            }
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i][0].ToString().Trim();
            strProject = ds.Tables[0].Rows[i][1].ToString().Trim();

            node3 = new TreeNode();
            node3.Text = strProjectID + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            AllProjectTreeShowForPageFind_YYUP(strProjectID, node3, strDepartString);

            TreeView1.DataBind();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (strOperationType == "All" | strOperationType == "Next")
            {
                strMaxProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMinProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }
            else
            {
                strMinProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMaxProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }

            return strMinProjectID + "-" + strMaxProjectID;
        }
        else
        {
            return "";
        }
    }

    public static void AllProjectTreeShowForPageFind_YYUP(string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL;
        IList lst1;

        string strProjectID, strProject;
        String strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived')";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";

        strHQL += " order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + "." + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            AllProjectTreeShowForPageFind_YYUP(strProjectID, node, strDepartString);
        }
    }

    //����������Ŀ�ĵ��� FOR YYUP
    public static string InitialAllProjectDocTree_YYUP(TreeView TreeView1, string strUserCode, string strQueryCount, string strOperationType, string strMinProjectID, string strMaxProjectID)
    {
        string strHQL;
        string strDepartString;
        string strProjectID, strProject, strProductLineRelated;

        strProductLineRelated = ShareClass.GetDepartSuperUserRelatedProductLineFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();

        node1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_0 Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        node1.Target = "Project" + "_" + "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strOperationType == "All")
        {
            strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

            if (strProductLineRelated == "YES")
            {
                strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
                strHQL += " (Select departSuperUserRelatedProductLine.ProductLineName From DepartSuperUserRelatedProductLine as departSuperUserRelatedProductLine Where departSuperUserRelatedProductLine.UserCode = " + "'" + strUserCode + "'" + "))";
            }

            strHQL += " And Status Not in ('Deleted','Archived')";
            strHQL += " Order By ProjectID DESC limit " + strQueryCount;
        }
        else
        {
            if (strOperationType == "Piror")
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";

                if (strProductLineRelated == "YES")
                {
                    strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
                    strHQL += " (Select departSuperUserRelatedProductLine.ProductLineName From DepartSuperUserRelatedProductLine as departSuperUserRelatedProductLine Where departSuperUserRelatedProductLine.UserCode = " + "'" + strUserCode + "'" + "))";
                }

                strHQL += " And ProjectID > " + strMaxProjectID;
                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID ASC limit " + strQueryCount;
            }
            else
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where  DepartCode in " + strDepartString + ")";
                strHQL += " And ProjectID < " + strMinProjectID;

                if (strProductLineRelated == "YES")
                {
                    strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
                    strHQL += " (Select departSuperUserRelatedProductLine.ProductLineName From DepartSuperUserRelatedProductLine as departSuperUserRelatedProductLine Where departSuperUserRelatedProductLine.UserCode = " + "'" + strUserCode + "'" + "))";
                }

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID DESC limit " + strQueryCount;
            }
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i][0].ToString().Trim();
            strProject = ds.Tables[0].Rows[i][1].ToString().Trim();

            node3 = new TreeNode();
            node3.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            //AllProjectDocTreeShowIncludeAll(strUserCode, strProjectID, node3);

            if (getChildProjectNumber(strProjectID, strDepartString) > 0)
            {
                node4 = new TreeNode();
                node4.Text = "����Ŀ";  
                node4.Target = strProjectID + "_ChildrenProject";
                node4.Expanded = false;
                node3.ChildNodes.Add(node4);
                AllProjectDocTreeShowIncludeChild_YYUP(strProjectID, node4, strDepartString);
            }

            TreeView1.DataBind();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (strOperationType == "All" | strOperationType == "Next")
            {
                strMaxProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMinProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }
            else
            {
                strMinProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMaxProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }

            return strMinProjectID + "-" + strMaxProjectID;
        }
        else
        {
            return "";
        }
    }

    public static void AllProjectDocTreeShowIncludeChild_YYUP(string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL;
        IList lst1, lst2;

        string strProjectID, strProject;
        String strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        TreeNode node4 = new TreeNode();

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived')";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";

        strHQL += " order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Project_" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            if (getChildProjectNumber(strProjectID, strDepartString) > 0)
            {
                node4 = new TreeNode();
                node4.Text = "����Ŀ";  
                node4.Target = strProjectID + "_ChildrenProject";
                node4.Expanded = false;
                node.ChildNodes.Add(node4);
                AllProjectDocTreeShowIncludeChild_YYUP(strProjectID, node4, strDepartString);
            }
        }
    }

    //��Ŀ�ĵ��������������̣����󣬷��յ�������Ŀ��ص��ĵ���
    public static void AllProjectDocTreeShowIncludeAll(string strUserCode, string strProjectID, TreeNode treeNode)
    {
        string strHQL;
        IList lst;
        string strTaskID, strTaskName, strDefectID, strDefectName, strWLID, strWLName, strRiskID, strRiskName;
        int i;

        TreeNode node = new TreeNode();
        TreeNode treeNode1 = new TreeNode();

        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=ProjectPlan_0&ProjectID=" + strProjectID + " Target=Right>�ƻ�</a>";  
        treeNode1.Target = "ProjectPlan" + "_" + "0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        TakeTopPlan.InitialProjectPlanTreeOnTreeNode(treeNode1, strProjectID, GetProjectPlanVersion(strProjectID, "InUse").ToString());

        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=ProjectTask_0&ProjectID=" + strProjectID + " Target=Right>����</a>";  
        treeNode1.Target = "ProjectTask_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        strHQL = "from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID;
        strHQL += " and projectTask.TaskID in (Select document.RelatedID from Document as document where document.RelatedType = 'Task' and document.Status <> 'Deleted')";
        strHQL += " Order by projectTask.TaskID DESC";
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = new ProjectTask();
        for (i = 0; i < lst.Count; i++)
        {
            projectTask = (ProjectTask)lst[i];
            strTaskID = projectTask.TaskID.ToString();
            strTaskName = projectTask.Task.Trim();

            node = new TreeNode();
            node.Target = "ProjectTask_" + strTaskID;
            node.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=ProjectTask_" + strTaskID + " Target=Right>" + strTaskID + ". " + strTaskName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }

        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Defect_0&ProjectID=" + strProjectID + " Target=Right>����</a>";  
        treeNode1.Target = "Defect_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        strHQL = "from Defectment as defectment where defectment.DefectID in (select relatedDefect.DefectID from RelatedDefect as relatedDefect where relatedDefect.ProjectID = " + strProjectID + ")";
        strHQL += " and defectment.DefectID in (Select document.RelatedID from Document as document where document.RelatedType = 'Requirement' and document.Status <> 'Deleted')";
        strHQL += " Order by defectment.DefectID DESC";
        DefectmentBLL defectmentBLL = new DefectmentBLL();
        lst = defectmentBLL.GetAllDefectments(strHQL);
        Defectment defectment = new Defectment();
        for (i = 0; i < lst.Count; i++)
        {
            defectment = (Defectment)lst[i];
            strDefectID = defectment.DefectID.ToString();
            strDefectName = defectment.DefectName.Trim();

            node = new TreeNode();
            node.Target = "Defect" + "_" + strDefectID;
            node.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Defect_" + strDefectID + " Target=Right>" + strDefectID + ". " + strDefectName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }
        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=WorkFlow_0&ProjectID=" + strProjectID + " Target=Right>������</a>";  
        treeNode1.Target = "WorkFlow_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);

        strHQL = "from WorkFlow as workFlow where";
        strHQL += " ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
        strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
        strHQL += " or (workFlow.RelatedType = '����' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID = " + strProjectID + "))";  
        strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedDefect from RelatedDefect as relatedDefect where relatedDefect.ProjectID = " + strProjectID + ")))";
        strHQL += " and workFlow.WLID in (Select document.RelatedID from Document as document where document.RelatedType = 'Workflow' and document.Status <> 'Deleted')";
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = new WorkFlow();
        for (i = 0; i < lst.Count; i++)
        {
            workFlow = (WorkFlow)lst[i];
            strWLID = workFlow.WLID.ToString();
            strWLName = workFlow.WLName.Trim();

            node = new TreeNode();
            node.Target = "WorkFlow" + "_" + strWLID;
            node.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=WorkFlow_" + strWLID + " Target=Right>" + strWLID + ". " + strWLName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }

        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Risk_0&ProjectID=" + strProjectID + " Target=Right>����</a>";  
        treeNode1.Target = "Risk_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        strHQL = "From ProjectRisk as projectRisk where projectRisk.ProjectID = " + strProjectID;
        strHQL += " and projectRisk.ID in (Select document.RelatedID from Document as document where document.RelatedType = '����' and document.Status <> 'Deleted')";  
        strHQL += " Order by projectRisk.ID DESC";
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);
        ProjectRisk projectRisk = new ProjectRisk();
        for (i = 0; i < lst.Count; i++)
        {
            projectRisk = (ProjectRisk)lst[i];
            strRiskID = projectRisk.ID.ToString();
            strRiskName = projectRisk.Risk.Trim();

            node = new TreeNode();
            node.Target = "Risk" + "_" + strRiskID;
            node.Text = "<A href=TTAllProjectDocuments.aspx?TargetID=Risk_" + strRiskID + " Target=Right>" + strRiskID + ". " + strRiskName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }
    }

    //����������Ŀ����Ŀ��(����ҳ�����ӣ�
    public static string InitialAllProjectRelatedPageTree(TreeView TreeView1, string strUserCode, string strRelatedType, string strQueryCount, string strOperationType, string strMinProjectID, string strMaxProjectID)
    {
        string strHQL;
        string strProjectID, strProject;
        string strDepartString;

        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        if (strRelatedType == "Defect")
        {
            node1.Text = "<A href=TTProjectDefectmentManage.aspx?ProjectID=0" + " Target=Right>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }
        if (strRelatedType == "ProjectTask")
        {
            node1.Text = "<A href=TTAllProjectTask.aspx?ProjectID=0" + " Target=Right>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }
        if (strRelatedType == "ProjectDoc")
        {
            node1.Text = "<A href=TTProjectDocManage.aspx?Project_0" + " Target=Right>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }
        if (strRelatedType == "WorkFlow")
        {
            node1.Text = "<A href=TTProjectWorkFlowManage.aspx?ProjectID=0" + " Target=Right>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }

        if (strRelatedType == "ProjectRisk")
        {
            node1.Text = "<A href=TTProjectRiskManage.aspx?ProjectID=0" + " Target=Right>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }

        if (strRelatedType == "ProjectBonus")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B>";
        }

        if (strRelatedType == "ProjectExpense")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B>";
        }

        if (strRelatedType == "ProjectIncomeAndExpense")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B>";
        }

        if (strRelatedType == "InAndOut")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B>";
        }

        if (strRelatedType == "MakeBudget" | strRelatedType == "MakeBudgetAll")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("XiangMuYuSuan").ToString().Trim() + "</B>";
        }

        if (strRelatedType == "MakeItemBudget")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("XiangMuWuZiYuSuan").ToString().Trim() + "</B>";
        }

        if (strRelatedType == "BudgetReport")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + "</B>";
        }

        if (strRelatedType == "MaterialExpenseApply")
        {
            node1.Text = "<B>��Ŀ���ʷ�������</B>";  
        }

        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strOperationType == "All")
        {
            strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";

            strHQL += " And Status not in ('Deleted','Archived') ";
            strHQL += " Order By ProjectID DESC limit " + strQueryCount;
        }
        else
        {
            if (strOperationType == "Piror")
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";
                strHQL += " And ProjectID > " + strMaxProjectID;

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID ASC limit " + strQueryCount;
            }
            else
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";
                strHQL += " And ProjectID < " + strMinProjectID;

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID DESC limit " + strQueryCount + "";
            }
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i][0].ToString().Trim();
            strProject = ds.Tables[0].Rows[i][1].ToString().Trim();

            node3 = new TreeNode();

            if (strRelatedType == "ProjectDefect")
            {
                node3.Text = "<A href=TTAllProjectDefectment.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask")
            {
                node3.Text = "<A href=TTAllProjectTask.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectWorkFlow")
            {
                node3.Text = "<A href=TTAllProjectWorkFlow.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectRisk")
            {
                node3.Text = "<A href=TTAllProjectRisk.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectBonus")
            {
                node3.Text = "<A href=TTConfirmDailyWorkBonus.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectExpense")
            {
                node3.Text = "<A href=TTConfirmProjectExpenseForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectIncomeAndExpense")
            {
                node3.Text = "<A href=TTProjectIncomeAndExpenseReportForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "InAndOut")
            {
                node3.Text = "<A href=TTProjectIncomeExpense.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MakeBudget")
            {
                node3.Text = "<A href=TTMakeProjectBudget.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MakeBudgetAll")
            {
                node3.Text = "<A href=TTMakeProjectBudgetForAll.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MakeItemBudget")
            {
                node3.Text = "<A href=TTProjectRelatedItem.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "BudgetReport")
            {
                node3.Text = "<A href=TTProjectBudgetReport.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MaterialExpenseApply")
            {
                node3.Text = "<A href=TTProjectMaterialPaymentApplicant.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);

            AllProjectTreeRelatedPageShow(strUserCode, strRelatedType, strProjectID, node3, strDepartString);
            TreeView1.DataBind();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (strOperationType == "All" | strOperationType == "Next")
            {
                strMaxProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMinProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }
            else
            {
                strMinProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMaxProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }

            return strMinProjectID + "-" + strMaxProjectID;
        }
        else
        {
            return "";
        }
    }

    public static void AllProjectTreeRelatedPageShow(string strUserCode, string strRelatedType, string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived') ";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;

            if (strRelatedType == "Defect")
            {
                node.Text = "<A href=TTProjectDefectmentManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask")
            {
                node.Text = "<A href=TTProjectTaskManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "WorkFlow")
            {
                node.Text = "<A href=TTProjectWorkFlowManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectRisk")
            {
                node.Text = "<A href=TTAllProjectRisk.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectBonus")
            {
                node.Text = "<A href=TTConfirmDailyWorkBonus.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectExpense")
            {
                node.Text = "<A href=TTConfirmProjectExpenseForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectIncomeAndExpense")
            {
                node.Text = "<A href=TTProjectIncomeAndExpenseReportForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "InAndOut")
            {
                node.Text = "<A href=TTProjectIncomeExpense.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MakeBudget")
            {
                node.Text = "<A href=TTMakeProjectBudget.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MakeBudgetAll")
            {
                node.Text = "<A href=TTMakeProjectBudgetForAll.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MakeItemBudget")
            {
                node.Text = "<A href=TTProjectRelatedItem.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "BudgetReport")
            {
                node.Text = "<A href=TTProjectBudgetReport.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "MaterialExpenseApply")
            {
                node.Text = "<A href=TTProjectMaterialPaymentApplicant.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID;
            strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                AllProjectTreeRelatedPageShow(strUserCode, strRelatedType, strProjectID, node, strDepartString);
            }
        }
    }

    //����������Ŀ����Ŀ��(����ҳ�����ӣ�--FOR YYUP
    public static string InitialAllProjectRelatedPageTree_YYUP(TreeView TreeView1, string strUserCode, string strRelatedType, string strQueryCount, string strOperationType, string strMinProjectID, string strMaxProjectID)
    {
        string strHQL;

        string strProjectID, strProject, strProductLineRelated;

        strProductLineRelated = ShareClass.GetDepartSuperUserRelatedProductLineFromUserCode(strUserCode);

        string strDepartString;
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        if (strRelatedType == "Defect")
        {
            node1.Text = "<A href=TTProjectDefectmentManage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }
        if (strRelatedType == "ProjectTask")
        {
            node1.Text = "<A href=TTAllProjectTask.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }
        if (strRelatedType == "ProjectDoc")
        {
            node1.Text = "<A href=TTProjectDocManage.aspx?Project_0" + " Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }
        if (strRelatedType == "WorkFlow")
        {
            node1.Text = "<A href=TTProjectWorkFlowManage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }

        if (strRelatedType == "ProjectRisk")
        {
            node1.Text = "<A href=TTProjectRiskManage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B></a>";
        }

        if (strRelatedType == "ProjectBonus")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B>";
        }

        if (strRelatedType == "ProjectExpense")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B>";
        }

        if (strRelatedType == "ProjectIncomeAndExpense")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B>";
        }

        if (strRelatedType == "InAndOut")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B>";
        }

        if (strRelatedType == "Budget")
        {
            node1.Text = "<B>" + LanguageHandle.GetWord("SYXMHTMDZXM").ToString().Trim() + " </B>";
        }

        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strOperationType == "All")
        {
            strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";

            if (strProductLineRelated == "YES")
            {
                strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
                strHQL += " (Select departSuperUserRelatedProductLine.ProductLineName From DepartSuperUserRelatedProductLine as departSuperUserRelatedProductLine Where departSuperUserRelatedProductLine.UserCode = " + "'" + strUserCode + "'" + "))";
            }

            strHQL += " And Status not in ('Deleted','Archived') ";
            strHQL += " Order By ProjectID DESC limit " + strQueryCount;
        }
        else
        {
            if (strOperationType == "Piror")
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";
                strHQL += " And ProjectID > " + strMaxProjectID;

                if (strProductLineRelated == "YES")
                {
                    strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
                    strHQL += " (Select departSuperUserRelatedProductLine.ProductLineName From DepartSuperUserRelatedProductLine as departSuperUserRelatedProductLine Where departSuperUserRelatedProductLine.UserCode = " + "'" + strUserCode + "'" + "))";
                }

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID ASC limit " + strQueryCount;
            }
            else
            {
                strHQL = "Select ProjectID,ProjectName From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";
                strHQL += " And ProjectID < " + strMinProjectID;

                if (strProductLineRelated == "YES")
                {
                    strHQL += " and project.ProjectID in (Select project_YYUP.ProjectID From Project_YYUP as project_YYUP Where project_YYUP.ProductLine in ";
                    strHQL += " (Select departSuperUserRelatedProductLine.ProductLineName From DepartSuperUserRelatedProductLine as departSuperUserRelatedProductLine Where departSuperUserRelatedProductLine.UserCode = " + "'" + strUserCode + "'" + "))";
                }

                strHQL += " And Status Not in ('Deleted','Archived')";
                strHQL += " Order By ProjectID DESC limit " + strQueryCount;
            }
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i][0].ToString().Trim();
            strProject = ds.Tables[0].Rows[i][1].ToString().Trim();

            node3 = new TreeNode();

            if (strRelatedType == "ProjectDefect")
            {
                node3.Text = "<A href=TTAllProjectDefectment.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask")
            {
                node3.Text = "<A href=TTAllProjectTask.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectWorkFlow")
            {
                node3.Text = "<A href=TTAllProjectWorkFlow.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectRisk")
            {
                node3.Text = "<A href=TTAllProjectRisk.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectBonus")
            {
                node3.Text = "<A href=TTConfirmDailyWorkBonus.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectExpense")
            {
                node3.Text = "<A href=TTConfirmProjectExpenseForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectIncomeAndExpense")
            {
                node3.Text = "<A href=TTProjectIncomeAndExpenseReportForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "InAndOut")
            {
                node3.Text = "<A href=TTProjectIncomeExpense.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "Budget")
            {
                node3.Text = "<A href=TTProjectBudgetReport.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);

            AllProjectTreeRelatedPageShow_YYUP(strUserCode, strRelatedType, strProjectID, node3, strDepartString);
            TreeView1.DataBind();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (strOperationType == "All" | strOperationType == "Next")
            {
                strMaxProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMinProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }
            else
            {
                strMinProjectID = ds.Tables[0].Rows[0][0].ToString();
                strMaxProjectID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0].ToString();
            }

            return strMinProjectID + "-" + strMaxProjectID;
        }
        else
        {
            return "";
        }
    }

    public static void AllProjectTreeRelatedPageShow_YYUP(string strUserCode, string strRelatedType, string strParentID, TreeNode treeNode, string strDepartString)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived') ";
        strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;

            if (strRelatedType == "Defect")
            {
                node.Text = "<A href=TTProjectDefectmentManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask")
            {
                node.Text = "<A href=TTProjectTaskManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "WorkFlow")
            {
                node.Text = "<A href=TTProjectWorkFlowManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectRisk")
            {
                node.Text = "<A href=TTAllProjectRisk.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectBonus")
            {
                node.Text = "<A href=TTConfirmDailyWorkBonus.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectExpense")
            {
                node.Text = "<A href=TTConfirmProjectExpenseForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectIncomeAndExpense")
            {
                node.Text = "<A href=TTProjectIncomeAndExpenseReportForFIN.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "InAndOut")
            {
                node.Text = "<A href=TTProjectIncomeExpense.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID;
            strHQL += " and project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                AllProjectTreeRelatedPageShow_YYUP(strUserCode, strRelatedType, strProjectID, node, strDepartString);
            }
        }
    }

    //ȡ�������ĿID��
    public static int GetMaxProjectIDForAllProjectList(int intProjectID, int intProjectCount, string strDepartString)
    {
        string strHQL;

        strHQL = "Select Max(ProjectID) From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";
        strHQL += " And ProjectID <= " + intProjectID.ToString();
        strHQL += " And Status not in ('Deleted','Archived') ";
        strHQL += " Order By ProjectID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //ȡ����С��ĿID��
    public static int GetMinProjectIDForAllProjectList(string strDepartString, int intProjectCount)
    {
        string strHQL;

        strHQL = "Select Min(ProjectID) From T_Project Where PMCode in (Select UserCode From T_ProjectMember Where DepartCode In " + strDepartString + ")";
        strHQL += " And Status not in ('Deleted','Archived') Order By ProjectID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //���������Ŀ�ĵ���
    public static void InitialInvolvedProjectDocTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProject;
        IList lst;
        string strProjectIDString = "";

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<A href=TTProjectDocManage.aspx?TargetID=Project_0 Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        node1.Target = "Project" + "_" + "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " Order by proRelatedUser.ProjectID DESC";
        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();
        lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);
        for (int i = 0; i < lst.Count; i++)
        {
            proRelatedUser = (ProRelatedUser)lst[i];

            strProjectID = proRelatedUser.ProjectID.ToString(); ;
            strProject = proRelatedUser.ProjectName.Trim();

            if (strProjectIDString.IndexOf(strProjectID + ",") >= 0)
            {
                continue;
            }
            else
            {
                strProjectIDString += strProjectID + ",";
            }

            node3 = new TreeNode();
            node3.Text = "<A href=TTProjectDocManage.aspx?TargetID=Project_" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            //InvolvedProjectDocTreeShowIncludeAll(strUserCode, strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    //��Ŀ�ĵ��������������󣬼ƻ������յ���Ŀ��ص��ĵ���
    public static void InvolvedProjectDocTreeShowIncludeAll(string strUserCode, string strProjectID, TreeNode treeNode)
    {
        string strHQL;
        IList lst;
        string strTaskID, strTaskName, strReqID, strReqName, strDefectID, strDefectName, strWLID, strWLName, strRiskID, strRiskName;
        int i;

        TreeNode node = new TreeNode();
        TreeNode treeNode1 = new TreeNode();

        //���ɼƻ��ڵ�
        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTProjectDocManage.aspx?TargetID=ProjectPlan_0&ProjectID=" + strProjectID + " Target=Right>�ƻ�</a>";  
        treeNode1.Target = "ProjectPlan" + "_" + "0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        TakeTopPlan.InitialProjectPlanTreeOnTreeNode(treeNode1, strProjectID, GetProjectPlanVersion(strProjectID, "InUse").ToString());

        //��������ڵ�
        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTProjectDocManage.aspx?TargetID=ProjectTask_0&ProjectID=" + strProjectID + " Target=Right>����</a>";  
        treeNode1.Target = "ProjectTask_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        strHQL = "from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + " and projectTask.PlanID = 0 ";
        strHQL += " and projectTask.TaskID in (Select document.RelatedID from Document as document where document.RelatedType = 'Task' and document.Status <> 'Deleted')";
        strHQL += " Order by projectTask.TaskID DESC";
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);
        ProjectTask projectTask = new ProjectTask();
        for (i = 0; i < lst.Count; i++)
        {
            projectTask = (ProjectTask)lst[i];
            strTaskID = projectTask.TaskID.ToString();
            strTaskName = projectTask.Task.Trim();

            node = new TreeNode();
            node.Target = "ProjectTask_" + strTaskID;
            node.Text = "<A href=TTProjectDocManage.aspx?TargetID=ProjectTask_" + strTaskID + " Target=Right>" + strTaskID + ". " + strTaskName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }

        //��������ڵ�
        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTProjectDocManage.aspx?TargetID=Req_0&ProjectID=" + strProjectID + " Target=Right>����</a>";  
        treeNode1.Target = "Defect_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        strHQL = "from Requirement as requirement where requirement.ReqID in (select relatedReq.ReqID from RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + ")";
        strHQL += " and requirement.ReqID in (Select document.RelatedID from Document as document where document.RelatedType = 'Requirement' and document.Status <> 'Deleted')";
        strHQL += " Order by requirement.ReqID DESC";
        RequirementBLL requirementBLL = new RequirementBLL();
        lst = requirementBLL.GetAllRequirements(strHQL);
        Requirement requirement = new Requirement();
        for (i = 0; i < lst.Count; i++)
        {
            requirement = (Requirement)lst[i];
            strReqID = requirement.ReqID.ToString();
            strReqName = requirement.ReqName.Trim();

            node = new TreeNode();
            node.Target = "Req" + "_" + strReqID;
            node.Text = "<A href=TTProjectDocManage.aspx?TargetID=Req_" + strReqID + " Target=Right>" + strReqID + ". " + strReqName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }

        //����ȱ�ݽڵ�
        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTProjectDocManage.aspx?TargetID=Defect_0&ProjectID=" + strProjectID + " Target=Right>ȱ��</a>";  
        treeNode1.Target = "Defect_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        strHQL = "from Defectment as defectment where defectment.DefectID in (select relatedDefect.DefectID from RelatedDefect as relatedDefect where relatedDefect.ProjectID = " + strProjectID + ")";
        strHQL += " and defectment.DefectID in (Select document.RelatedID from Document as document where document.RelatedType = 'Defect' and document.Status <> 'Deleted')";
        strHQL += " Order by defectment.DefectID DESC";
        DefectmentBLL defectmentBLL = new DefectmentBLL();
        lst = defectmentBLL.GetAllDefectments(strHQL);
        Defectment defectment = new Defectment();
        for (i = 0; i < lst.Count; i++)
        {
            defectment = (Defectment)lst[i];
            strDefectID = defectment.DefectID.ToString();
            strDefectName = defectment.DefectName.Trim();

            node = new TreeNode();
            node.Target = "Defect" + "_" + strDefectID;
            node.Text = "<A href=TTProjectDocManage.aspx?TargetID=Defect_" + strDefectID + " Target=Right>" + strDefectID + ". " + strDefectName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }

        //���ɹ������ڵ�
        strHQL = "from WorkFlow as workFlow where";
        strHQL += " ((workFlow.RelatedType = 'Project' and workFlow.RelatedID = " + strProjectID + ")";
        strHQL += " or (workFlow.RelatedType = 'Task' and workFlow.RelatedID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + "))";
        strHQL += " or (workFlow.RelatedType = '����' and workFlow.RelatedID in (select projectRisk.ID from ProjectRisk as projectRisk where projectRisk.ProjectID = " + strProjectID + "))";  
        strHQL += " or (workFlow.RelatedType = 'Requirement' and workFlow.RelatedID in (select relatedDefect from RelatedDefect as relatedDefect where relatedDefect.ProjectID = " + strProjectID + ")))";
        strHQL += " and workFlow.WLID in (Select document.RelatedID from Document as document where document.RelatedType = 'Workflow' and document.Status <> 'Deleted')";
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = new WorkFlow();
        for (i = 0; i < lst.Count; i++)
        {
            workFlow = (WorkFlow)lst[i];
            strWLID = workFlow.WLID.ToString();
            strWLName = workFlow.WLName.Trim();

            node = new TreeNode();
            node.Target = "WorkFlow" + "_" + strWLID;
            node.Text = "<A href=TTProjectDocManage.aspx?TargetID=WorkFlow_" + strWLID + " Target=Right>" + strWLID + ". " + strWLName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }

        //���ɷ��սڵ�
        treeNode1 = new TreeNode();
        treeNode1.Text = "<A href=TTProjectDocManage.aspx?TargetID=Risk_0&ProjectID=" + strProjectID + " Target=Right>����</a>";  
        treeNode1.Target = "Risk_0";
        treeNode1.Expanded = false;
        treeNode.ChildNodes.Add(treeNode1);
        strHQL = "From ProjectRisk as projectRisk where projectRisk.ProjectID = " + strProjectID; ;
        strHQL += " and projectRisk.ID in (Select document.RelatedID from Document as document where document.RelatedType = '����' and document.Status <> 'Deleted')";  
        strHQL += " Order by projectRisk.ID DESC";
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);
        ProjectRisk projectRisk = new ProjectRisk();
        for (i = 0; i < lst.Count; i++)
        {
            projectRisk = (ProjectRisk)lst[i];
            strRiskID = projectRisk.ID.ToString();
            strRiskName = projectRisk.Risk.Trim();

            node = new TreeNode();
            node.Target = "Risk" + "_" + strRiskID;
            node.Text = "<A href=TTProjectDocManage.aspx?TargetID=Risk_" + strRiskID + " Target=Right>" + strRiskID + ". " + strRiskName + "</a>";
            treeNode1.ChildNodes.Add(node);
            node.Expanded = false;
        }
    }

    //���������Ŀ����Ŀ��(����ҳ�����ӣ�
    public static void InitialInvolvedProjectRelatedPageTree(TreeView TreeView1, string strUserCode, string strRelatedType)
    {
        string strHQL, strProjectID, strProject;
        IList lst;

        string strProjectIDString = "";

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        if (strRelatedType == "Req")
        {
            node1.Text = "<A href=TTProjectReqHandlePage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }

        if (strRelatedType == "Defect")
        {
            node1.Text = "<A href=TTProjectDefectmentHandlePage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }

        if (strRelatedType == "ProjectTask")
        {
            node1.Text = "<A href=TTProjectTaskHandlePage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }

        if (strRelatedType == "ProjectTask_JYX")
        {
            node1.Text = "<A href=TTProjectTaskHandlePage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }

        if (strRelatedType == "ProjectDoc")
        {
            node1.Text = "<A href=TTProjectDocManage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }
        if (strRelatedType == "WorkFlow")
        {
            node1.Text = "<A href=TTProjectWorkFlowManage.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }
        if (strRelatedType == "InAndOut")
        {
            node1.Text = "<A href=TTProjectIncomeExpense.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }
        if (strRelatedType == "ProjectCost")
        {
            node1.Text = "<A href=TTRCJProjectTotalCostFee.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }
        if (strRelatedType == "ProjectCostCheck")
        {
            node1.Text = "<A href=TTRCJProjectFundStartPlanApproval.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }
        if (strRelatedType == "ProjectIncomeAndExpense")
        {
            node1.Text = "<A href=TTRCJProjectCost.aspx?ProjectID=0" + " Target=Right><B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B></a>";
        }

        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID not in (Select proRelatedUser1.ProjectID From ProRelatedUser as proRelatedUser1 Where  proRelatedUser1.UserCode = " + "'" + strUserCode + "'" + " ) and proRelatedUser.UserCode = " + "'" + strUserCode + "'" + "  Order by proRelatedUser.ProjectID DESC";
        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();

        lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            proRelatedUser = (ProRelatedUser)lst[i];

            strProjectID = proRelatedUser.ProjectID.ToString();
            strProject = proRelatedUser.ProjectName.Trim();

            if (strProjectIDString.IndexOf(strProjectID + ",") >= 0)
            {
                continue;
            }
            else
            {
                strProjectIDString += strProjectID + ",";
            }

            node3 = new TreeNode();

            if (strRelatedType == "Req")
            {
                node3.Text = "<A href=TTProjectReqHandlePage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "Defect")
            {
                node3.Text = "<A href=TTProjectDefectmentHandlePage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask")
            {
                node3.Text = "<A href=TTProjectTaskHandlePage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask_JYX")
            {
                node3.Text = "<A href=TTProjectTaskHandlePage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "WorkFlow")
            {
                node3.Text = "<A href=TTProjectWorkFlowManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "InAndOut")
            {
                node3.Text = "<A href=TTProjectIncomeExpense.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectCost")
            {
                node3.Text = "<A href=TTRCJProjectTotalCostFee.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }
            if (strRelatedType == "ProjectCostCheck")
            {
                node3.Text = "<A href=TTRCJProjectFundStartPlanApproval.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectIncomeAndExpense")
            {
                node3.Text = "<A href=TTRCJProjectCost.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            node3.Target = strProjectID;
            node3.Expanded = true;

            node1.ChildNodes.Add(node3);
            InvolvedProjectTreeRelatedPageShow(strUserCode, strRelatedType, strProjectID, node3, strProjectIDString);
            TreeView1.DataBind();
        }
    }

    public static void InvolvedProjectTreeRelatedPageShow(string strUserCode, string strRelatedType, string strParentID, TreeNode treeNode, string strProjectIDString)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID = " + strParentID + " and proRelatedUser.UserCode = " + "'" + strUserCode + "'" + "  Order by proRelatedUser.ProjectID DESC";
        lst1 = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            proRelatedUser = (ProRelatedUser)lst1[i];
            strProjectID = proRelatedUser.ProjectID.ToString();
            strProject = proRelatedUser.ProjectName.Trim();

            if (strProjectIDString.IndexOf(strProjectID + ",") >= 0)
            {
                continue;
            }
            else
            {
                strProjectIDString += strProjectID + ",";
            }

            TreeNode node = new TreeNode();
            node.Target = strProjectID;

            if (strRelatedType == "Req")
            {
                node.Text = "<A href=TTProjectRequirementManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "Defect")
            {
                node.Text = "<A href=TTProjectDefectmentManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask")
            {
                node.Text = "<A href=TTProjectTaskManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectTask_JYX")
            {
                node.Text = "<A href=TTProjectTaskManage_JYX.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "WorkFlow")
            {
                node.Text = "<A href=TTProjectWorkFlowManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "InAndOut")
            {
                node.Text = "<A href=TTProjectIncomeExpense.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectCost")
            {
                node.Text = "<A href=TTRCJProjectTotalCostFee.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }
            if (strRelatedType == "ProjectCostCheck")
            {
                node.Text = "<A href=TTRCJProjectFundStartPlanApproval.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            if (strRelatedType == "ProjectIncomeAndExpense")
            {
                node.Text = "<A href=TTRCJProjectCost.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            }

            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID = " + strProjectID + "  Order by proRelatedUser.ProjectID DESC";
            lst2 = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst2.Count > 0)
            {
                InvolvedProjectTreeRelatedPageShow(strUserCode, strRelatedType, strProjectID, node, strProjectIDString);
            }
        }
    }



    //���������Ŀ����Ŀ��
    public static void InitialInvolvedProjectTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProject;
        IList lst;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID not in (Select proRelatedUser1.ProjectID From ProRelatedUser as proRelatedUser1 Where  proRelatedUser1.UserCode = " + "'" + strUserCode + "'" + " ) and proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " Order by proRelatedUser.ProjectID DESC";

        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();

        lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            proRelatedUser = (ProRelatedUser)lst[i];

            strProjectID = proRelatedUser.ProjectID.ToString(); ;
            strProject = proRelatedUser.ProjectName.Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            InvolvedProjectTreeShow(strUserCode, strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    public static void InvolvedProjectTreeShow(string strUserCode, string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID = " + strParentID + " and proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " order by proRelatedUser.ProjectID DESC";
        lst1 = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            proRelatedUser = (ProRelatedUser)lst1[i];
            strProjectID = proRelatedUser.ProjectID.ToString();
            strProject = proRelatedUser.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID = " + strProjectID + " Order by proRelatedUser.ProjectID DESC";
            lst2 = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst2.Count > 0)
            {
                InvolvedProjectTreeShow(strUserCode, strProjectID, node);
            }
        }
    }

    //�����������ĺ��Ҳ�����Ŀ����Ŀ��
    public static void InitialMyUnderTakeAndInvolvedProjectTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProject;
        IList lst;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WCYDXMHTMDZXM").ToString().Trim() + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID not in (Select proRelatedUser1.ProjectID From ProRelatedUser as proRelatedUser1 Where  proRelatedUser1.UserCode = " + "'" + strUserCode + "'" + " ) and proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " Order by proRelatedUser.ProjectID DESC";

        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();

        lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            proRelatedUser = (ProRelatedUser)lst[i];

            strProjectID = proRelatedUser.ProjectID.ToString(); ;
            strProject = proRelatedUser.ProjectName.Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            MyUnderTakeAndInvolvedProjectTreeShow(strUserCode, strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    public static void MyUnderTakeAndInvolvedProjectTreeShow(string strUserCode, string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();

        strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID = " + strParentID + " and proRelatedUser.UserCode = " + "'" + strUserCode + "'" + " order by proRelatedUser.ProjectID DESC";
        lst1 = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            proRelatedUser = (ProRelatedUser)lst1[i];
            strProjectID = proRelatedUser.ProjectID.ToString();
            strProject = proRelatedUser.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ParentID = " + strProjectID + " Order by proRelatedUser.ProjectID DESC";
            lst2 = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst2.Count > 0)
            {
                MyUnderTakeAndInvolvedProjectTreeShow(strUserCode, strProjectID, node);
            }
        }
    }

    //������Ŀ��������Ȩ�ޣ�
    public static void InitialPrjectTreeByAuthority(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProjectName;
        //string strUserCode;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>1." + LanguageHandle.GetWord("ZongXiangMu").ToString().Trim() + "</B>";
        node1.Target = "1";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = string.Format(@"Select * from T_Project as project where project.ParentID  = 1 
          and (project.PMCode = '{1}' Or project.UserCode ='{1}' or ProjectID in (Select ProjectID From T_RelatedUser Where UserCode = '{1}'))
          and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC", strUserCode, strUserCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds.Tables[0].Rows[i]["ProjectID"].ToString().Trim();
            strProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString().Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProjectName;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            ProjectTreeShowByAuthority(strProjectID, node3, strUserCode);
            TreeView1.DataBind();
        }
    }

    public static void ProjectTreeShowByAuthority(string strParentID, TreeNode treeNode, string strUserCode)
    {
        string strHQL, strProjectID, strProjectName;

        strHQL = string.Format(@"Select * from T_Project as project where project.ParentID ={0} 
                  and (project.PMCode = '{1}' Or project.UserCode ='{1}' or ProjectID in (Select ProjectID From T_RelatedUser Where UserCode = '{1}'))
                  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC", strParentID, strUserCode);
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            strProjectID = ds1.Tables[0].Rows[i]["ProjectID"].ToString().Trim();
            strProjectName = ds1.Tables[0].Rows[i]["ProjectName"].ToString().Trim();


            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProjectName;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = string.Format(@"Select * from T_Project as project where project.ParentID ={0} 
                    and (project.PMCode = '{1}' Or project.UserCode ='{1}' or ProjectID in (Select ProjectID From T_RelatedUser Where UserCode = '{1}'))
                    and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC", strProjectID, strUserCode);
            DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ProjectTreeShowByAuthority(strProjectID, node, strUserCode);
            }
        }
    }

    //�����Լ���������Ŀ����Ŀ��
    public static void InitialMyCreateProjectTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProject;
        IList lst;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("WJLDXM").ToString().Trim() + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and project.ParentID not in (select project.ProjectID from Project as project where project.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += "  and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            MyCreateProjectTreeShow(strUserCode, strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    public static void MyCreateProjectTreeShow(string strUserCode, string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'" + " and project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where  project.UserCode = " + "'" + strUserCode + "'" + " and project.ParentID = " + strProjectID + " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                MyCreateProjectTreeShow(strUserCode, strProjectID, node);
            }
        }
    }

    //�Ҹ������Ŀ����Ŀ��(������Ŀ���չ���
    public static void InitialMyTakeOverProjectTree(TreeView TreeView1, string strUserCode)
    {
        string strHQL, strProjectID, strProject;
        IList lst;
        string strProjectIDString = "";

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<A href=TTProjectRiskManage.aspx?ProjectID=0 Target=Right>" + "<B>" + LanguageHandle.GetWord("WFZDXM").ToString().Trim() + "</B>" + "</a>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where project.PMCode = " + "'" + strUserCode + "'";
        strHQL += " and project.ParentID not in (select project.ProjectID from Project as project where project.PMCode = " + "'" + strUserCode + "'";
        strHQL += " and project.ProjectID in (select projectRisk.ProjectID from ProjectRisk as projectRisk))";
        strHQL += " and project.ProjectID in (select projectRisk.ProjectID from ProjectRisk as projectRisk) ";
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            if (strProjectIDString.IndexOf(strProjectID + ",") >= 0)
            {
                continue;
            }
            else
            {
                strProjectIDString += strProjectID + ",";
            }

            node3 = new TreeNode();

            node3.Text = "<A href=TTProjectRiskManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            MyTakeoverProjectTreeShow(strUserCode, strProjectID, node3, strProjectIDString);
            TreeView1.DataBind();
        }
    }

    public static void MyTakeoverProjectTreeShow(string strPMCode, string strParentID, TreeNode treeNode, string strProjectIDString)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID;
        strHQL += "  and project.PMCode = " + "'" + strPMCode + "'";
        strHQL += " and project.ProjectID in (select projectRisk.ProjectID from ProjectRisk as projectRisk) ";
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            if (strProjectIDString.IndexOf(strProjectID + ",") >= 0)
            {
                continue;
            }
            else
            {
                strProjectIDString += strProjectID + ",";
            }

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = "<A href=TTProjectRiskManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID + " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                MyTakeoverProjectTreeShow(strPMCode, strProjectID, node, strProjectIDString);
            }
        }
    }

    //�Ҹ������Ŀ����Ŀ��(������Ŀ���չ���
    public static void InitialAllProjectTree(TreeView TreeView1, string strUserCode, string strDepartString)
    {
        string strHQL, strProjectID, strProject;
        IList lst;
        string strProjectIDString = "";

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<A href=TTProjectRiskManage.aspx?ProjectID=0 Target=Right>" + "<B>" + LanguageHandle.GetWord("WFZDXM").ToString().Trim() + "</B>" + "</a>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where project.PMCode = " + "'" + strUserCode + "'";
        strHQL += " and project.ParentID not in (select project.ProjectID from Project as project where project.PMCode = " + "'" + strUserCode + "'";
        strHQL += " and project.ProjectID in (select projectRisk.ProjectID from ProjectRisk as projectRisk))";
        strHQL += " and project.ProjectID in (select projectRisk.ProjectID from ProjectRisk as projectRisk) ";
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            if (strProjectIDString.IndexOf(strProjectID + ",") >= 0)
            {
                continue;
            }
            else
            {
                strProjectIDString += strProjectID + ",";
            }

            node3 = new TreeNode();

            node3.Text = "<A href=TTProjectRiskManage.aspx?ProjectID=" + strProjectID + " Target=Right>" + strProjectID + "." + strProject + "</a>";
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            MyTakeoverProjectTreeShow(strUserCode, strProjectID, node3, strProjectIDString);
            TreeView1.DataBind();
        }
    }

    //ά����Ŀ��־
    public static void UpdateDailyWork(string strUserCode, string strProjectID, string strRelatedType, string strRelatedID, string strWorkDetail)
    {
        string strHQL;
        IList lst;
        int intID;
        decimal deBonus;
        string strProjectType, strPMCode, strImpactByDetail;

        try
        {
            Project project = ShareClass.GetProject(strProjectID);
            strProjectType = project.ProjectType.Trim();
            strPMCode = project.PMCode.Trim();

            strImpactByDetail = ShareClass.GetProjectTypeImpactByDetail(strProjectID);

            strWorkDetail = " [" + strRelatedType + ":" + strRelatedID + "]WorkLog��" + strWorkDetail + " ";

            strHQL = "from DailyWork as dailyWork where dailyWork.UserCode = " + "'" + strUserCode + "'" + " and dailyWork.ProjectID = " + strProjectID + " and to_char(dailyWork.WorkDate,'yyyymmdd') = to_char(now(),'yyyymmdd')";
            DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
            lst = dailyWorkBLL.GetAllDailyWorks(strHQL);

            DailyWork dailyWork = new DailyWork();

            if (lst.Count == 0)
            {
                if (strUserCode == strPMCode)
                {
                    dailyWork.Type = "����";  
                }
                else
                {
                    dailyWork.Type = "����";  
                }

                dailyWork.Charge = decimal.Parse(ShareClass.getCurrentDateTotalExpenseByOneOperator(strProjectID, strUserCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ManHour = decimal.Parse(ShareClass.getCurrentDateTotalManHourByOneOperator(strProjectID, strUserCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ConfirmManHour = dailyWork.ManHour;

                //���ĸ���Ĺ�������
                if (strImpactByDetail == "YES")
                {
                    dailyWork.FinishPercent = decimal.Parse(ShareClass.getCurrentDateTotalProgressForMember(strProjectID, strUserCode));
                }
                else
                {
                    dailyWork.FinishPercent = 0;
                }

                dailyWork.UserCode = strUserCode;
                dailyWork.UserName = GetUserName(strUserCode);
                dailyWork.WorkDate = DateTime.Now;
                dailyWork.ProjectID = int.Parse(strProjectID);
                dailyWork.ProjectName = ShareClass.GetProjectName(strProjectID);
                dailyWork.DailySummary = strWorkDetail;

                dailyWork.RecordTime = DateTime.Now;
                dailyWork.Address = "";
                dailyWork.Achievement = "";

                deBonus = ShareClass.GetDailyWorkLogLength(dailyWork.DailySummary) * ShareClass.GetEveryCharPrice() + ShareClass.GetDailyUploadDocNumber(strUserCode, strProjectID) * ShareClass.GetEveryDocPrice();

                dailyWork.Bonus = deBonus;
                dailyWork.ConfirmBonus = deBonus;
                dailyWork.Authority = "NO";

                try
                {
                    dailyWorkBLL.AddDailyWork(dailyWork);
                }
                catch
                {
                }
            }
            else
            {
                dailyWork = (DailyWork)lst[0];
                intID = dailyWork.WorkID;
                dailyWork.DailySummary += "<BR/>" + strWorkDetail;

                if (strUserCode == strPMCode)
                {
                    dailyWork.Type = "����";  
                }
                else
                {
                    dailyWork.Type = "����";  
                }

                dailyWork.Charge = decimal.Parse(ShareClass.getCurrentDateTotalExpenseByOneOperator(strProjectID, strUserCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ManHour = decimal.Parse(ShareClass.getCurrentDateTotalManHourByOneOperator(strProjectID, strUserCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ConfirmManHour = dailyWork.ManHour;

                //���ĸ���Ĺ�������
                if (strImpactByDetail == "YES")
                {
                    dailyWork.FinishPercent = decimal.Parse(ShareClass.getCurrentDateTotalProgressForMember(strProjectID, strUserCode));
                }

                deBonus = ShareClass.GetDailyWorkLogLength(dailyWork.DailySummary) * ShareClass.GetEveryCharPrice() + ShareClass.GetDailyUploadDocNumber(strUserCode, strProjectID) * ShareClass.GetEveryDocPrice();
                dailyWork.Bonus = deBonus;
                dailyWork.ConfirmBonus = deBonus;

                dailyWork.Authority = "NO";

                try
                {
                    dailyWorkBLL.UpdateDailyWork(dailyWork, intID);
                }
                catch
                {
                }
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //ά����Ŀ������Ŀ��־
    public static void UpdateDailyWorkForPM(string strProjectID, string strRelatedType, string strRelatedID, string strWorkDetail)
    {
        string strHQL;
        IList lst;
        int intID;
        decimal deBonus;
        string strProjectType, strPMCode, strImpactByDetail;

        try
        {
            Project project = ShareClass.GetProject(strProjectID);
            strProjectType = project.ProjectType.Trim();
            strPMCode = project.PMCode.Trim();

            strImpactByDetail = ShareClass.GetProjectTypeImpactByDetail(strProjectID);

            strWorkDetail = " [" + strRelatedType + ":" + strRelatedID + "]WorkLog��" + strWorkDetail + " ";

            strHQL = "from DailyWork as dailyWork where dailyWork.UserCode = " + "'" + strPMCode + "'" + " and dailyWork.ProjectID = " + strProjectID + " and to_char(dailyWork.WorkDate,'yyyymmdd') = to_char(now(),'yyyymmdd')";
            DailyWorkBLL dailyWorkBLL = new DailyWorkBLL();
            lst = dailyWorkBLL.GetAllDailyWorks(strHQL);

            DailyWork dailyWork = new DailyWork();

            if (lst.Count == 0)
            {
                dailyWork.Type = "����";  

                dailyWork.Charge = decimal.Parse(ShareClass.getCurrentDateTotalExpenseByOneOperator(strProjectID, strPMCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ManHour = decimal.Parse(ShareClass.getCurrentDateTotalManHourByOneOperator(strProjectID, strPMCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ConfirmManHour = dailyWork.ManHour;

                //���ĸ���Ĺ�������
                if (strImpactByDetail == "YES")
                {
                    dailyWork.FinishPercent = decimal.Parse(ShareClass.getCurrentDateTotalProgressForPM(strProjectID));
                }
                else
                {
                    dailyWork.FinishPercent = 0;
                }

                dailyWork.UserCode = strPMCode;
                dailyWork.UserName = GetUserName(strPMCode);
                dailyWork.WorkDate = DateTime.Now;
                dailyWork.ProjectID = int.Parse(strProjectID);
                dailyWork.ProjectName = ShareClass.GetProjectName(strProjectID);
                dailyWork.DailySummary = strWorkDetail;

                dailyWork.RecordTime = DateTime.Now;
                dailyWork.Address = "";
                dailyWork.Achievement = "";

                deBonus = ShareClass.GetDailyWorkLogLength(dailyWork.DailySummary) * ShareClass.GetEveryCharPrice() + ShareClass.GetDailyUploadDocNumber(strPMCode, strProjectID) * ShareClass.GetEveryDocPrice();

                dailyWork.Bonus = deBonus;
                dailyWork.ConfirmBonus = deBonus;
                dailyWork.Authority = "NO";

                try
                {
                    dailyWorkBLL.AddDailyWork(dailyWork);

                    //���ĸ������Ŀ�ܽ���
                    if (strImpactByDetail == "YES")
                    {
                        UpdateProjectCompleteDegree(strProjectID, dailyWork.FinishPercent);
                    }
                }
                catch
                {
                }
            }
            else
            {
                dailyWork = (DailyWork)lst[0];
                intID = dailyWork.WorkID;
                //dailyWork.DailySummary += "<BR/>" + strWorkDetail;

                dailyWork.Type = "����";  

                dailyWork.Charge = decimal.Parse(ShareClass.getCurrentDateTotalExpenseByOneOperator(strProjectID, strPMCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ManHour = decimal.Parse(ShareClass.getCurrentDateTotalManHourByOneOperator(strProjectID, strPMCode, DateTime.Now.ToString("yyyyMMdd")));
                dailyWork.ConfirmManHour = dailyWork.ManHour;

                //���ĸ���Ĺ�������
                if (strImpactByDetail == "YES")
                {
                    dailyWork.FinishPercent = decimal.Parse(ShareClass.getCurrentDateTotalProgressForPM(strProjectID));
                }

                deBonus = ShareClass.GetDailyWorkLogLength(dailyWork.DailySummary) * ShareClass.GetEveryCharPrice() + ShareClass.GetDailyUploadDocNumber(strPMCode, strProjectID) * ShareClass.GetEveryDocPrice();
                dailyWork.Bonus = deBonus;
                dailyWork.ConfirmBonus = deBonus;

                dailyWork.Authority = "NO";

                try
                {
                    dailyWorkBLL.UpdateDailyWork(dailyWork, intID);

                    //���ĸ������Ŀ�ܽ���
                    if (strImpactByDetail == "YES")
                    {
                        UpdateProjectCompleteDegree(strProjectID, dailyWork.FinishPercent);
                    }
                }
                catch
                {
                }
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    public static void UpdateProjectCompleteDegree(string strProjectID, decimal deFinishPercent)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        project.FinishPercent = deFinishPercent;

        try
        {
            projectBLL.UpdateProject(project, int.Parse(strProjectID));
        }
        catch
        {
        }
    }

    //����������ְ��Ա��KPI������
    public static void InitialKPICheckTreeByDepartPosition(TreeView TreeView1, String strDepartCode, string strPosition)
    {
        string strHQL1, strHQL2;
        IList lst1, lst2;

        string strDepartName;
        string strUserCode, strUserName;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();

        strDepartName = ShareClass.GetDepartName(strDepartCode);
        node1.Text = "<B>" + strDepartCode + " " + strDepartName + " KPI</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strPosition == "")
        {
            strHQL1 = "From ProjectMember as projectMember Where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            strHQL1 += " Order By projectMember.SortNumber ASC";
        }
        else
        {
            strHQL1 = "From ProjectMember as projectMember Where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            strHQL1 += " And projectMember.JobTitle = " + "'" + strPosition + "'";
            strHQL1 += " Order By projectMember.SortNumber ASC";
        }
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst1 = projectMemberBLL.GetAllProjectMembers(strHQL1);

        for (int i = 0; i < lst1.Count; i++)
        {
            ProjectMember projectMember = (ProjectMember)lst1[i];

            strUserCode = projectMember.UserCode.Trim();
            strUserName = projectMember.UserName.Trim();

            node2 = new TreeNode();
            node2.Text = strUserName + "[" + projectMember.JobTitle + "]";
            node2.Target = strUserCode;
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);

            strHQL2 = "from UserKPICheck as userKPICheck where userKPICheck.UserCode = " + "'" + strUserCode + "'";
            strHQL2 += " Order By userKPICheck.StartTime DESC";
            UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
            lst2 = userKPICheckBLL.GetAllUserKPIChecks(strHQL2);

            UserKPICheck userKPICheck = new UserKPICheck();

            for (int j = 0; j < lst2.Count; j++)
            {
                userKPICheck = (UserKPICheck)lst2[j];

                node3 = new TreeNode();
                node3.Text = userKPICheck.KPICheckName.Trim();
                node3.Target = userKPICheck.KPICheckID.ToString();
                node3.Expanded = false;

                node2.ChildNodes.Add(node3);
            }
        }

        TreeView1.DataBind();
    }

    //����Ա��KPI������
    public static void InitialKPICheckTreeByUserCode(TreeView TreeView1, String strUserCode)
    {
        string strHQL1;
        IList lst1;

        string strUserName;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();

        strUserName = ShareClass.GetUserName(strUserCode);

        node1.Text = "<B>" + strUserName + " KPI</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL1 = "from UserKPICheck as userKPICheck where userKPICheck.UserCode = " + "'" + strUserCode + "'";
        strHQL1 += " Order By userKPICheck.StartTime DESC";
        UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
        lst1 = userKPICheckBLL.GetAllUserKPIChecks(strHQL1);

        UserKPICheck userKPICheck = new UserKPICheck();

        for (int i = 0; i < lst1.Count; i++)
        {
            userKPICheck = (UserKPICheck)lst1[i];

            node2 = new TreeNode();
            node2.Text = userKPICheck.KPICheckName.Trim();
            node2.Target = userKPICheck.KPICheckID.ToString();
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);
        }

        TreeView1.DataBind();
    }

    //����Ա��KPI������
    public static void InitialKPICheckTreeByDepartCode(TreeView TreeView1, String strDepartCode, string strDepartString)
    {
        string strHQL1;
        IList lst1;

        string strDepartName;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();

        strDepartName = ShareClass.GetDepartName(strDepartCode);

        node1.Text = "<B>" + strDepartName + " KPI</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL1 = "from UserKPICheck as userKPICheck where userKPICheck.UserCode in ( Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL1 += " Order By userKPICheck.StartTime DESC";
        UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
        lst1 = userKPICheckBLL.GetAllUserKPIChecks(strHQL1);

        UserKPICheck userKPICheck = new UserKPICheck();

        for (int i = 0; i < lst1.Count; i++)
        {
            userKPICheck = (UserKPICheck)lst1[i];

            node2 = new TreeNode();
            node2.Text = userKPICheck.KPICheckName.Trim();
            node2.Target = userKPICheck.KPICheckID.ToString();
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);
        }

        TreeView1.DataBind();
    }

    //������Ŀ��Ա��
    public static void InitialProjectMemberTree(TreeView TreeView, string strProjectID)
    {
        string strHQL;
        IList lst;

        int i, j;

        DataSet ds;

        string strActor, strUserCode, strUserName, strID, strPMCode, strPMName, strUserType;

        ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
        ProRelatedUser proRelatedUser = new ProRelatedUser();
        RelatedUser relatedUser = new RelatedUser();

        //��Ӹ��ڵ�
        TreeView.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + LanguageHandle.GetWord("XMTD").ToString().Trim() + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView.Nodes.Add(node1);

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        strPMCode = project.PMCode.Trim();
        strPMName = project.PMName.Trim();

        node2 = new TreeNode();
        node2.Text = "[" + LanguageHandle.GetWord("XiangMuJingLi").ToString().Trim() + "]";
        node2.Target = LanguageHandle.GetWord("XiangMuJingLi").ToString().Trim();
        node2.Expanded = true;
        node1.ChildNodes.Add(node2);

        strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.UserCode = " + "'" + strPMCode + "'";
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
        if (lst.Count > 0)
        {
            relatedUser = (RelatedUser)lst[0];
            strID = relatedUser.ID.ToString();
            node3 = new TreeNode();
            node3.Text = "<B>" + strPMCode + " " + strPMName + "</B>";
            node3.Target = strID;
            node2.ChildNodes.Add(node3);
        }

        strHQL = "select distinct Actor from T_RelatedUser where ProjectID = " + strProjectID + " and UserCode <> " + "'" + strPMCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedUser");
        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strActor = ds.Tables[0].Rows[i][0].ToString().Trim();

            node2 = new TreeNode();

            node2.Text = "[" + strActor + "]";
            node2.Target = strActor;
            node2.Expanded = true;

            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ProjectID = " + "'" + strProjectID + "'" + "   and proRelatedUser.Actor = " + "'" + strActor + "'";
            proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            for (j = 0; j < lst.Count; j++)
            {
                proRelatedUser = (ProRelatedUser)lst[j];
                strUserCode = proRelatedUser.UserCode.Trim();
                strUserName = proRelatedUser.UserName.Trim();
                strUserType = ShareClass.GetUserType(strUserCode);

                //if (strUserType == "")
                //{
                strID = proRelatedUser.ID.ToString();

                node3 = new TreeNode();

                if (strUserType == "OUTER")
                {
                    node3.Text = "<font color='red'>" + strUserCode + " " + strUserName + "</font>";
                }
                else
                {
                    node3.Text = strUserCode + " " + strUserName;
                }
                node3.Target = strID;
                node2.ChildNodes.Add(node3);
                //}
            }

            node1.ChildNodes.Add(node2);
        }

        node1.Expand();
        TreeView.DataBind();
    }

    //������ĿίԱ�������Ŀ����������ĿίԱ�ᣩ
    public static void InitialPrjectTreeByAuthorityProjectLeader(TreeView TreeView1, string strUserCode, string strDepartString)
    {
        string strHQL, strProjectID, strProject;
        IList lst;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>1." + LanguageHandle.GetWord("ZongXiangMu").ToString().Trim() + "</B>";
        node1.Target = "1";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Project as project where (project.PMCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Or project.UserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " and project.ParentID  = 1";
        strHQL += " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        lst = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            project = (Project)lst[i];

            strProjectID = project.ProjectID.ToString(); ;
            strProject = project.ProjectName.Trim();

            node3 = new TreeNode();

            node3.Text = strProjectID + "." + strProject;
            node3.Target = strProjectID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            ProjectTreeShowByAuthorityProjectLeader(strProjectID, node3);
            TreeView1.DataBind();
        }
    }

    public static void ProjectTreeShowByAuthorityProjectLeader(string strParentID, TreeNode treeNode)
    {
        string strHQL, strProjectID, strProject;
        IList lst1, lst2;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = new Project();

        strHQL = "from Project as project where project.ParentID = " + strParentID + " and project.Status not in ('Deleted','Archived') order by project.ProjectID DESC";
        lst1 = projectBLL.GetAllProjects(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            project = (Project)lst1[i];
            strProjectID = project.ProjectID.ToString();
            strProject = project.ProjectName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strProjectID;
            node.Text = strProjectID + ". " + strProject;
            treeNode.ChildNodes.Add(node);
            node.Expanded = false;

            strHQL = "from Project as project where project.ParentID = " + strProjectID + " Order by project.ProjectID DESC";
            lst2 = projectBLL.GetAllProjects(strHQL);

            if (lst2.Count > 0)
            {
                ProjectTreeShowByAuthorityProjectLeader(strProjectID, node);
            }
        }
    }

    //�����˸��ƻ���
    public static void InitialPlanTreeByUserCode(TreeView TreeView1, String strUserCode, string strRelatedType)
    {
        string strHQL;
        IList lst;

        string strUserName;

        strUserName = ShareClass.GetUserName(strUserCode);

        string strPlanID, strPlanName, strBackupPlanID;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "<B>" + strUserName + " " + LanguageHandle.GetWord("Plan").ToString().Trim() + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Plan as plan where  plan.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and plan.ParentID not in (Select plan.BackupPlanID From Plan as plan Where plan.UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and plan.Status not in ('Deleted','Archived') ";
        if (strRelatedType != "OTHER")
        {
            strHQL += " and plan.RelatedType = " + "'" + strRelatedType + "'";
        }
        strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
        PlanBLL planBLL = new PlanBLL();
        Plan plan = new Plan();

        lst = planBLL.GetAllPlans(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            plan = (Plan)lst[i];

            strPlanID = plan.PlanID.ToString(); ;
            strPlanName = plan.PlanName.Trim();
            strBackupPlanID = plan.BackupPlanID.ToString();

            node3 = new TreeNode();

            node3.Text = strPlanName;
            node3.Target = strPlanID;
            node3.Expanded = false;

            node1.ChildNodes.Add(node3);
            PlanTreeShowByUserCode(strUserCode, strBackupPlanID, node3, strRelatedType);
            TreeView1.DataBind();
        }
    }

    //�����˸��ƻ���
    public static void InitialPlanTreeByUserCode(TreeView TreeView1, String strUserCode, String strRelatedType, String strRelatedID, String strRelatedCode)
    {
        string strHQL;
        IList lst;

        string strPlanID, strBackupPlanID, strPlanName;
        string strUserName;

        strUserName = ShareClass.GetUserName(strUserCode);

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        if (strRelatedType != "OTHER")
        {
            node1.Text = "<B>" + strUserName + ":" + " " + LanguageHandle.GetWord("Plan").ToString().Trim() + "</B>";
        }
        else
        {
            node1.Text = "<B>" + strUserName + " " + LanguageHandle.GetWord("Plan").ToString().Trim() + "</B>";
        }
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from Plan as plan where plan.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and ((plan.ParentID not in (Select plan1.BackupPlanID From Plan as plan1 Where plan1.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " or (plan.ParentID = 0))";
        //strHQL += " and plan.ParentID = 0";
        strHQL += " and plan.Status not in ('Deleted','Archived') ";

        if (strRelatedType != "OTHER")
        {
            strHQL += " and plan.RelatedType = " + "'" + strRelatedType + "'";
        }

        if (strRelatedID != "0")
        {
            strHQL += " and plan.RelatedID = " + strRelatedID;
        }

        if (!string.IsNullOrEmpty(strRelatedCode))
        {
            strHQL += " and plan.RelatedCode  = '" + strRelatedCode + "'";
        }

        strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
        PlanBLL planBLL = new PlanBLL();
        Plan plan = new Plan();

        lst = planBLL.GetAllPlans(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            plan = (Plan)lst[i];

            strPlanID = plan.PlanID.ToString();
            strPlanName = plan.PlanName.Trim();

            strBackupPlanID = plan.BackupPlanID.ToString();

            node3 = new TreeNode();

            node3.Text = strPlanName;
            node3.Target = strPlanID;
            node3.Expanded = true;

            node1.ChildNodes.Add(node3);
            PlanTreeShowByUserCode(strUserCode, strBackupPlanID, node3, strRelatedType);
            TreeView1.DataBind();
        }
    }

    public static void PlanTreeShowByUserCode(string strUserCode, string strParentID, TreeNode treeNode, string strRelatedType)
    {
        string strHQL, strPlanID, strBackupPlanID, strPlanName;
        IList lst1, lst2;

        PlanBLL planBLL = new PlanBLL();
        Plan plan = new Plan();

        strHQL = "from Plan as plan where UserCode = " + "'" + strUserCode + "'" + " and plan.ParentID = " + strParentID;
        if (strRelatedType != "OTHER")
        {
            strHQL += " and plan.RelatedType = " + "'" + strRelatedType + "'";
        }
        strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
        lst1 = planBLL.GetAllPlans(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            plan = (Plan)lst1[i];

            strPlanID = plan.PlanID.ToString();
            strBackupPlanID = plan.BackupPlanID.ToString();

            strPlanName = plan.PlanName.Trim();

            TreeNode node = new TreeNode();
            node.Target = strPlanID;
            node.Text = strPlanName;
            treeNode.ChildNodes.Add(node);
            node.Expanded = true;

            strHQL = "from Plan as plan where UserCode = " + "'" + strUserCode + "'" + " and plan.ParentID = " + strParentID;
            strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
            lst2 = planBLL.GetAllPlans(strHQL);

            if (lst2.Count > 0)
            {
                PlanTreeShowByUserCode(strUserCode, strBackupPlanID, node, strRelatedType);
            }
        }
    }

    //����KPI����
    public static void InitialKPITree(TreeView TreeView1)
    {
        string strHQL;
        IList lst;

        string strKPIType;

        //��Ӹ��ڵ�
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();

        node1.Text = "<B>" + "KPI " + LanguageHandle.GetWord("MuBanKu").ToString().Trim() + "</B>";
        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        strHQL = "from KPIType as kpiType ";
        strHQL += " Order By kpiType.SortNumber ASC";
        KPITypeBLL kpiTypeBLL = new KPITypeBLL();
        KPIType kpiType = new KPIType();

        lst = kpiTypeBLL.GetAllKPITypes(strHQL);

        for (int i = 0; i < lst.Count; i++)
        {
            kpiType = (KPIType)lst[i];

            strKPIType = kpiType.Type;

            node2 = new TreeNode();

            node2.Text = strKPIType;
            node2.Target = strKPIType;
            node2.Expanded = false;

            node1.ChildNodes.Add(node2);
            KPITreeShow(strKPIType, node2);
            TreeView1.DataBind();
        }
    }

    public static void KPITreeShow(string strKPIType, TreeNode treeNode)
    {
        string strHQL;
        IList lst1;

        string strKPIID, strKPI;
        int intSortNumber;

        KPILibraryBLL kpiLibraryBLL = new KPILibraryBLL();
        KPILibrary kpiLibrary = new KPILibrary();

        strHQL = "from KPILibrary as kpiLibrary where kpiLibrary.KPIType = " + "'" + strKPIType + "'" + " Order By kpiLibrary.ID ASC";
        lst1 = kpiLibraryBLL.GetAllKPILibrarys(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            kpiLibrary = (KPILibrary)lst1[i];

            strKPIID = kpiLibrary.ID.ToString();
            strKPI = kpiLibrary.KPI.Trim();
            intSortNumber = kpiLibrary.SortNumber;

            TreeNode node = new TreeNode();
            node.Target = strKPIID;
            node.Text = intSortNumber.ToString() + ". " + strKPI;
            treeNode.ChildNodes.Add(node);
            node.Expanded = true;
        }
    }

    #endregion �������ҵ����

    #region ȡ���û������Ķ�������ID��

    //ȡ���û�������ģ�����ģ���
    public static string GetMyCreatedMaxModuleID()
    {
        string strHQL;

        strHQL = "Select max(ID) From T_ProModuleLevel ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û���������Ŀ�����Ŀ��
    public static string GetMyCreatedMaxUserLoginManageID()
    {
        string strHQL;

        strHQL = "Select max(ID) From T_UserLoginManage ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserLoginManage");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û�������������ͼ�κ�
    public static string GetMyCreatedMaxSystemAnalystChartID()
    {
        string strHQL;

        strHQL = "Select max(ID) From T_SystemAnalystChartManagement ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û���������Ŀ�����Ŀ��
    public static string GetMyCreatedMaxProjectID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'" + " Order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.ProjectID.ToString();
    }

    //ȡ���û�������������������
    public static string GetMyCreatedMaxDefectID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Defectment as defectment where defectment.ApplicantCode = " + "'" + strUserCode + "'" + " Order by defectment.DefectID DESC";
        DefectmentBLL defectmentBLL = new DefectmentBLL();
        lst = defectmentBLL.GetAllDefectments(strHQL);

        Defectment defectment = (Defectment)lst[0];

        return defectment.DefectID.ToString();
    }

    //ȡ���û�������������������
    public static string GetMyCreatedMaxReqID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Requirement as requirement where requirement.ApplicantCode = " + "'" + strUserCode + "'" + " Order by requirement.ReqID DESC";
        RequirementBLL requirementBLL = new RequirementBLL();
        lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement.ReqID.ToString();
    }

    //ȡ���û������Ļ����������
    public static string GetMyCreatedMaxMeetingID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Meeting as meeting where meeting.BuilderCode = " + "'" + strUserCode + "'" + " Order by meeting.ID DESC";
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);

        Meeting meeting = (Meeting)lst[0];

        return meeting.ID.ToString();
    }

    //ȡ���û��������������ұ��
    public static string GetMyCreatedMaxMeetingRoomID()
    {
        string strHQL;
        IList lst;

        strHQL = "from MeetingRoom as meetingRoom";
        MeetingRoomBLL meetingRoomBLL = new MeetingRoomBLL();
        lst = meetingRoomBLL.GetAllMeetingRooms(strHQL);

        MeetingRoom meetingRoom = (MeetingRoom)lst[0];

        return meetingRoom.ID.ToString();
    }

    //ȡ���û������������Ŀ����ţ�
    public static string GetMyCreatedMaxTaskID(string strProjectID, string strUserCode)
    {
        string strHQL = "from ProjectTask as projectTask where projectTask.ProjectID = " + strProjectID + " Order by projectTask.TaskID DESC";

        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        IList lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.TaskID.ToString();
    }

    //ȡ���û������������Ŀ���պţ�
    public static string GetMyCreatedMaxRiskID(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectRisk as projectRisk where projectRisk.ProjectID = " + strProjectID + " order by projectRisk.ID DESC";
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);

        ProjectRisk projectRisk = (ProjectRisk)lst[0];

        return projectRisk.ID.ToString();
    }

    //ȡ���û������������Ŀ���ɼ�¼�ţ�
    public static string GetMyCreatedMaxTaskAssignRecordID(string strTaskID, string strUserCode)
    {
        string strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.TaskID = " + strTaskID + " and taskAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + " Order by taskAssignRecord.ID Desc";
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        IList lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        TaskAssignRecord taskAssignRecord = (TaskAssignRecord)lst[0];

        return taskAssignRecord.ID.ToString();
    }

    //ȡ���û����������ȱ�ݷ��ɼ�¼�ţ�
    public static string GetMyCreatedMaxDefectAssignRecordID(string strDefectID, string strUserCode)
    {
        string strHQL = "from DefectAssignRecord as defectAssignRecord where defectAssignRecord.DefectID = " + strDefectID + " and defectAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + " Order by defectAssignRecord.ID Desc";
        DefectAssignRecordBLL defectAssignRecordBLL = new DefectAssignRecordBLL();
        IList lst = defectAssignRecordBLL.GetAllDefectAssignRecords(strHQL);

        DefectAssignRecord defectAssignRecord = (DefectAssignRecord)lst[0];

        return defectAssignRecord.ID.ToString();
    }

    //ȡ���û����������������ɼ�¼�ţ�
    public static string GetMyCreatedMaxReqAssignRecordID(string strReqID, string strUserCode)
    {
        string strHQL = "from ReqAssignRecord as reqAssignRecord where reqAssignRecord.ReqID = " + strReqID + " and reqAssignRecord.AssignManCode = " + "'" + strUserCode + "'" + " Order by reqAssignRecord.ID Desc";
        ReqAssignRecordBLL reqAssignRecordBLL = new ReqAssignRecordBLL();
        IList lst = reqAssignRecordBLL.GetAllReqAssignRecords(strHQL);

        ReqAssignRecord reqAssignRecord = (ReqAssignRecord)lst[0];

        return reqAssignRecord.ID.ToString();
    }

    //ȡ���û�����������ͬ��
    public static string GetMyCreatedMaxConstractID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.RecorderCode = " + "'" + strUserCode + "'" + " Order by constract.ConstractID DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        return constract.ConstractID.ToString();
    }

    //ȡ���û������ĺ�ͬҵ��Ա����ID��
    public static string GetMyCreatedMaxConstractSalesID(string strConstractCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_ConstractSales Where ConstractCode = " + "'" + strConstractCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractSales");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������ĺ�ͬ���������ID��
    public static string GetMyCreatedMaxConstractChangeRecordID(string strConstractCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_ConstractChangeRecord Where ConstractCode = " + "'" + strConstractCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractChangeRecord");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������ĺ�ͬ���ص�����ID��
    public static string GetMyCreatedMaxConstractEntryID(string strConstractCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_ConstractRelatedEntryOrder Where ConstractCode = " + "'" + strConstractCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractRelatedEntryOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û����������ĺ�ͬ�����
    public static string GetMyCreatedMaxConstractPayableID(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayable as constractPayable where constractPayable.ConstractCode = " + "'" + strConstractCode + "'" + " Order by constractPayable.ID DESC";
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);

        ConstractPayable constractPayable = (ConstractPayable)lst[0];

        return constractPayable.ID.ToString();
    }

    //ȡ���û����������ĺ�ͬ�����
    public static string GetMyCreatedMaxConstractPayablePlanID(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayable as constractPayable where constractPayable.RelatedType = " + "'" + strRelatedType + "'" + " and constractPayable.RelatedID = " + strRelatedID;
        strHQL += " Order By constractPayable.ID DESC";
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);

        ConstractPayable constractPayable = (ConstractPayable)lst[0];

        return constractPayable.ID.ToString();
    }

    //ȡ���û����������ĺ�ͬ�����¼��
    public static string GetMyCreatedMaxConstractPayableRecordID(string strPayableID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractPayableRecord as constractPayableRecord where constractPayableRecord.PayableID = " + strPayableID + " Order by constractPayableRecord.ID DESC";
        ConstractPayableRecordBLL constractPayableRecordBLL = new ConstractPayableRecordBLL();
        lst = constractPayableRecordBLL.GetAllConstractPayableRecords(strHQL);

        ConstractPayableRecord constractPayableRecord = (ConstractPayableRecord)lst[0];

        return constractPayableRecord.ID.ToString();
    }

    //ȡ���û����������ĺ�ͬ�տ��
    public static string GetMyCreatedMaxConstractReceivableID(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.ConstractCode = " + "'" + strConstractCode + "'" + "Order by constractReceivables.ID DESC";
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

        ConstractReceivables constractReceivables = (ConstractReceivables)lst[0];

        return constractReceivables.ID.ToString();
    }

    //ȡ���û����������ĺ�ͬ�տ��
    public static string GetMyCreatedMaxConstractReceivablePlanID(string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivables as constractReceivables where constractReceivables.RelatedType = " + "'" + strRelatedType + "'" + " and constractReceivables.RelatedID = " + strRelatedID;
        strHQL += " Order By constractReceivables.ID DESC";
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);

        ConstractReceivables constractReceivables = (ConstractReceivables)lst[0];

        return constractReceivables.ID.ToString();
    }

    //ȡ���û����������ĺ�ͬ�տ��¼��
    public static string GetMyCreatedMaxConstractReceivableRecordID(string strReceivablesID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractReceivablesRecord as constractReceivablesRecord where constractReceivablesRecord.ReceivablesID = " + strReceivablesID + " Order by constractReceivablesRecord.ID DESC";
        ConstractReceivablesRecordBLL constractReceivablesRecordBLL = new ConstractReceivablesRecordBLL();
        lst = constractReceivablesRecordBLL.GetAllConstractReceivablesRecords(strHQL);

        ConstractReceivablesRecord constractReceivablesRecord = (ConstractReceivablesRecord)lst[0];

        return constractReceivablesRecord.ID.ToString();
    }

    //ȡ���û����������Э����
    public static string GetMyCreatedMaxColloaborationID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(CoID) From T_Collaboration Where CreatorCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Collaboration");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    //ȡ���û������Ķ��ű��
    public static string GetMyCreatedMaxSMSID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_SMSSendDIY Where UserCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SMSSendDIY");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ����֯����Ϣ���͵ı��
    public static string GetMyCreatedMaxDepartmentMsgPushID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(MsgID) From T_DepartmentMsgPush Where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartmentMsgPush");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û�����������������
    public static string GetMyCreatedWorkFlowID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.CreatorCode = " + "'" + strUserCode + "'" + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow.WLID.ToString();
    }

    //ȡ���û��������������������
    public static string GetMyCreatedWorkFlowStepID(string strWLID)
    {
        string strHQL;

        strHQL = "Select Max(StepID) From T_WorkFlowStep Where WLID = " + strWLID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowStep");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ���û��������������ģ�岽���
    public static string GetMyCreatedWorkFlowTStepID(string strTemName)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName = " + "'" + strTemName + "'" + " Order by workFlowTStep.StepID DESC";
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

        WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

        return workFlowTStep.StepID.ToString();
    }

    //ȡ���û������������������ϸ�ں�
    public static string GetMyCreatedMaxWorkFlowTStepOperatorID(string strStepID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.StepID = " + strStepID + " Order by workFlowTStepOperator.ID DESC";
        WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
        lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);

        WorkFlowTStepOperator workFlowTStepOperator = (WorkFlowTStepOperator)lst[0];

        return workFlowTStepOperator.ID.ToString();
    }

    ////ȡ���û�����������������������
    //public static string GetMyCreatedMaxWorkFlowTStepOperationID(string strStepID)
    //{
    //    string strHQL = "from WorkFlowTStepOperation as workFlowTStepOperation where workFlowTStepOperation.StepID = " + strStepID + " Order by workFlowTStepOperation.OperationID DESC";
    //    WorkFlowTStepOperationBLL workFlowTStepOperationBLL = new WorkFlowTStepOperationBLL();
    //    IList lst = workFlowTStepOperationBLL.GetAllWorkFlowTStepOperations(strHQL);

    //    WorkFlowTStepOperation workFlowTStepOperation = (WorkFlowTStepOperation)lst[0];

    //    return workFlowTStepOperation.OperationID.ToString();
    //}

    //ȡ���û��������������ģ��XML�ڵ��������ID��
    public static string GetMyCreatedMaxWFTemplateXMLNodeGlobalVariableID()
    {
        string strHQL;

        strHQL = "Select MAX(ID) From T_WFTemplateXMLNodeGlobalVariable";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WFTemplateXMLNodeGlobalVariable");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������������������������
    public static string GetMyCreatedMaxWorkFlowTStepConditionID()
    {
        string strHQL;
        IList lst;

        strHQL = "from WLTStepCondition as wlTStepCondition Order by wlTStepCondition.ConID DESC";
        WLTStepConditionBLL wlTStepConditionBLL = new WLTStepConditionBLL();
        lst = wlTStepConditionBLL.GetAllWLTStepConditions(strHQL);

        WLTStepCondition wlTStepCondition = (WLTStepCondition)lst[0];

        return wlTStepCondition.ConID.ToString();
    }

    //ȡ���û�������������������������ʽ��
    public static string GetMyCreatedMaxWorkFlowTStepConditionExpressionID()
    {
        string strHQL;
        IList lst;

        strHQL = "from WLTStepConditionExpression as wlTStepConditionExpression Order by wlTStepConditionExpression.ID DESC";
        WLTStepConditionExpressionBLL wlTStepConditionExpressionBLL = new WLTStepConditionExpressionBLL();
        lst = wlTStepConditionExpressionBLL.GetAllWLTStepConditionExpressions(strHQL);

        WLTStepConditionExpression wlTStepConditionExpression = (WLTStepConditionExpression)lst[0];

        return wlTStepConditionExpression.ID.ToString();
    }

    //ȡ���û�������������������������ʽ��
    public static string GetMyCreatedMaxWFTStepRelatedTem(string strStepID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From  T_WFTStepRelatedTem Where RelatedStepID = " + strStepID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_WFTStepRElatedTem");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û�����������ɫ���Ա��
    public static string GetMyCreatedMaxActorGroupDetailID(string strGroupName)
    {
        string strHQL;
        IList lst;

        strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName= " + "'" + strGroupName + "'" + " Order by actorGroupDetail.GroupID DESC";
        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        ActorGroupDetail actorGroupDetail = (ActorGroupDetail)lst[0];

        return actorGroupDetail.GroupID.ToString();
    }

    public static string GetMyCreatedMaxSitemContentID()
    {
        string strHQL;
        IList lst;

        strHQL = "Select max(ID) From T_SiteModuleContent";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SiteModuleContent");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������������Ŀ��ԱID��
    public static string GetMyCreatedMaxProjectRelatedUserID(string strProjectID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_RelatedUser Where ProjectID = " + strProjectID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_RelatedUser");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������������Ŀ���ú�
    public static string GetMyCreatedMaxProExpenseID(string strProjectID, string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strUserCode + "'" + " order by proExpense.ID DESC";
        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        lst = proExpenseBLL.GetAllProExpenses(strHQL);

        ProExpense proExpense = (ProExpense)lst[0];

        return proExpense.ID.ToString();
    }

    //ȡ���û������������Ŀ���ú�
    public static string GetMyCreatedMaxProBudgetID(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectBudget as projectBudget where projectBudget.ProjectID = " + strProjectID + " order by projectBudget.ID DESC";
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);

        ProjectBudget projectBudget = (ProjectBudget)lst[0];

        return projectBudget.ID.ToString();
    }

    //ȡ���û������������Ŀ���ú�
    public static string GetMyCreatedMaxAllProBudgetID(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "Select Max(ID) from t_ProjectBudget where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectBudgetByAll");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������������Ŀ�ƻ���
    public static string GetMyCreatedMaxProPlanID(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkPlan as workPlan where workPlan.ProjectID = " + strProjectID + " and workPlan.VerID = " + strVerID + " Order by workPlan.ID DESC";
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);

        WorkPlan workPlan = (WorkPlan)lst[0];

        return workPlan.ID.ToString();
    }

    //ȡ���û���������Ŀ�ƻ���Ա�����ID��
    public static string GetMyCreatedMaxPlanMemberID(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from PlanMember as planMember where planMember.PlanID = " + strPlanID + " Order by planMember.ID DESC";
        PlanMemberBLL planMemberBLL = new PlanMemberBLL();
        lst = planMemberBLL.GetAllPlanMembers(strHQL);

        PlanMember planMember = (PlanMember)lst[0];

        return planMember.ID.ToString();
    }

    //ȡ���û������ķ����������ID��
    public static string GetMyCreatedMaxExpenseApplyWLID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseApplyWL as expenseApplyWL where  expenseApplyWL.ApplicantCode = " + "'" + strUserCode + "'" + " Order by expenseApplyWL.ID DESC";
        ExpenseApplyWLBLL expenseApplyWLBLL = new ExpenseApplyWLBLL();
        lst = expenseApplyWLBLL.GetAllExpenseApplyWLs(strHQL);

        ExpenseApplyWL expenseApplyWL = (ExpenseApplyWL)lst[0];

        return expenseApplyWL.ID.ToString();
    }

    //ȡ���û������ķ��ñ������ID��
    public static string GetMyCreatedMaxExpenseClaimWLID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ExpenseClaim as expenseClaim where expenseClaim.ApplicantCode=" + "'" + strUserCode + "'" + " Order by expenseClaim.ECID DESC";
        ExpenseClaimBLL expenseClaimBLL = new ExpenseClaimBLL();
        lst = expenseClaimBLL.GetAllExpenseClaims(strHQL);

        ExpenseClaim expenseClaim = (ExpenseClaim)lst[0];

        return expenseClaim.ECID.ToString();
    }

    //ȡ���û������ķ��ñ�����ϸ�����ID��
    public static string GetMyCreatedMaxExpenseClaimDetailID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_ExpenseClaimDetail Where UserCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ExpenseClaimDetail");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    //ȡ�����Ĳ���������
    public static string GetMyCreatedMaxTaskTestCaseID()
    {
        string strHQL;
        IList lst;

        strHQL = "from TaskTestCase as taskTestCase Order by taskTestCase.ID DESC";
        TaskTestCaseBLL taskTestCaseBLL = new TaskTestCaseBLL();
        lst = taskTestCaseBLL.GetAllTaskTestCases(strHQL);

        TaskTestCase taskTestCase = (TaskTestCase)lst[0];

        return taskTestCase.ID.ToString();
    }

    //ȡ���û��������ʲ��ɹ������ID��
    public static string GetMyCreatedMaxAssetPurchaseOrderID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.OperatorCode = " + "'" + strUserCode + "'" + " Order by assetPurchaseOrder.POID DESC";
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        return assetPurchaseOrder.POID.ToString();
    }

    //ȡ���û���������Ŀ���ʷ������뵥���ID��
    public static string GetMyCreatedMaxProjectMaterialPaymentApplicantID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMaterialPaymentApplicant as projectMaterialPaymentApplicant where projectMaterialPaymentApplicant.UserCode = " + "'" + strUserCode + "'" + " Order by projectMaterialPaymentApplicant.AOID DESC";
        ProjectMaterialPaymentApplicantBLL projectMaterialPaymentApplicantBLL = new ProjectMaterialPaymentApplicantBLL();
        lst = projectMaterialPaymentApplicantBLL.GetAllProjectMaterialPaymentApplicants(strHQL);

        ProjectMaterialPaymentApplicant projectMaterialPaymentApplicant = (ProjectMaterialPaymentApplicant)lst[0];

        return projectMaterialPaymentApplicant.AOID.ToString();
    }

    //ȡ���û���������Ŀ���ʷ������뵥��ϸ���ID��
    public static string GetMyCreatedMaxProjectMaterialPaymentApplicantDetailID(string strAOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMaterialPaymentApplicantDetail as projectMaterialPaymentApplicantDetail where projectMaterialPaymentApplicantDetail.AOID = " + "'" + strAOID + "'" + " Order by projectMaterialPaymentApplicantDetail.ID DESC";
        ProjectMaterialPaymentApplicantDetailBLL projectMaterialPaymentApplicantDetailBLL = new ProjectMaterialPaymentApplicantDetailBLL();
        lst = projectMaterialPaymentApplicantDetailBLL.GetAllProjectMaterialPaymentApplicantDetails(strHQL);

        ProjectMaterialPaymentApplicantDetail projectMaterialPaymentApplicantDetail = (ProjectMaterialPaymentApplicantDetail)lst[0];

        return projectMaterialPaymentApplicantDetail.ID.ToString();
    }

    //ȡ���û��������ʲ��������뵥���ID��
    public static string GetMyCreatedMaxSupplierAssetPaymentApplicantID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from SupplierAssetPaymentApplicant as supplierAssetPaymentApplicant where supplierAssetPaymentApplicant.UserCode = " + "'" + strUserCode + "'" + " Order by supplierAssetPaymentApplicant.AOID DESC";
        SupplierAssetPaymentApplicantBLL supplierAssetPaymentApplicantBLL = new SupplierAssetPaymentApplicantBLL();
        lst = supplierAssetPaymentApplicantBLL.GetAllSupplierAssetPaymentApplicants(strHQL);

        SupplierAssetPaymentApplicant supplierAssetPaymentApplicant = (SupplierAssetPaymentApplicant)lst[0];

        return supplierAssetPaymentApplicant.AOID.ToString();
    }

    //ȡ���û��������ʲ��������뵥��ϸ���ID��
    public static string GetMyCreatedMaxSupplierAssetPaymentApplicantDetailID(string strAOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from SupplierAssetPaymentApplicantDetail as supplierAssetPaymentApplicantDetail where supplierAssetPaymentApplicantDetail.AOID = " + "'" + strAOID + "'" + " Order by supplierAssetPaymentApplicantDetail.ID DESC";
        SupplierAssetPaymentApplicantDetailBLL supplierAssetPaymentApplicantDetailBLL = new SupplierAssetPaymentApplicantDetailBLL();
        lst = supplierAssetPaymentApplicantDetailBLL.GetAllSupplierAssetPaymentApplicantDetails(strHQL);

        SupplierAssetPaymentApplicantDetail supplierAssetPaymentApplicantDetail = (SupplierAssetPaymentApplicantDetail)lst[0];

        return supplierAssetPaymentApplicantDetail.ID.ToString();
    }

    //ȡ���û����������ϲɹ������ID��
    public static string GetMyCreatedMaxGoodsPurchaseOrderID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsPurchaseOrder as goodsPurchaseOrder where goodsPurchaseOrder.OperatorCode = " + "'" + strUserCode + "'" + " Order by goodsPurchaseOrder.POID DESC";
        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        lst = goodsPurchaseOrderBLL.GetAllGoodsPurchaseOrders(strHQL);

        GoodsPurchaseOrder goodsPurchaseOrder = (GoodsPurchaseOrder)lst[0];

        return goodsPurchaseOrder.POID.ToString();
    }

    //ȡ�����Ϲ����������ID
    public static string GetMyCreatedMaxGoodsSupplyOrderID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(SUID) From T_GoodsSupplyOrder where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsSupplyOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�����������������ID
    public static string GetMyCreatedMaxGoodsProductionOrderID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(PDID) From T_GoodsProductionOrder where CreatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsProductionOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ��������������ϸ�����ID
    public static string GetMyCreatedMaxGoodsProductionOrderDetailID(string strPDID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsProductionOrderDetail Where PDID = " + strPDID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsProductionOrderDetail");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���ʲ���ⵥ�����ID
    public static string GetMyCreatedMaxAssetCheckInID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(CheckInID) From T_AssetCheckInOrder where CreatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_AssetCheckInOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û��������ʲ��ɹ���ϸ���ID��
    public static string GetMyCreatedMaxAssetPurRecordID(string strPOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.POID = " + "'" + strPOID + "'" + " Order by assetPurRecord.ID DESC";
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);

        AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

        return assetPurRecord.ID.ToString();
    }

    //ȡ���û����������ϲɹ���ϸ���ID��
    public static string GetMyCreatedMaxGoodsPurRecordID(string strPOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsPurRecord as goodsPurRecord where goodsPurRecord.POID = " + "'" + strPOID + "'" + " Order by goodsPurRecord.ID DESC";
        GoodsPurRecordBLL goodsPurRecordBLL = new GoodsPurRecordBLL();
        lst = goodsPurRecordBLL.GetAllGoodsPurRecords(strHQL);

        GoodsPurRecord goodsPurRecord = (GoodsPurRecord)lst[0];

        return goodsPurRecord.ID.ToString();
    }

    //ȡ�����Ϲ�������ϸ�����ID
    public static string GetMyCreatedMaxGoodsSupplyOrderDetailID(string strSUID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsSupplyOrderDetail Where SUID = " + strSUID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsSupplyOrderDetail");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������ĺ�ͬ������ϸ�����ID��
    public static string GetMyCreatedMaxConstractRelatedGoodsID(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractRelatedGoods as constractRelatedGoods where constractRelatedGoods.ConstractCode = " + "'" + strConstractCode + "'" + " Order by constractRelatedGoods.ID DESC";
        ConstractRelatedGoodsBLL constractRelatedGoodsBLL = new ConstractRelatedGoodsBLL();
        lst = constractRelatedGoodsBLL.GetAllConstractRelatedGoodss(strHQL);

        ConstractRelatedGoods constractRelatedGoods = (ConstractRelatedGoods)lst[0];

        return constractRelatedGoods.ID.ToString();
    }

    //ȡ���û������Ŀͻ�������ϸ�����ID��
    public static string GetMyCreatedMaxCustomerRelatedGoodsInforID(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerRelatedGoodsInfor as customerRelatedGoodsInfor where customerRelatedGoodsInfor.CustomerCode = " + "'" + strCustomerCode + "'" + " Order by customerRelatedGoodsInfor.ID DESC";
        CustomerRelatedGoodsInforBLL customerRelatedGoodsInforBLL = new CustomerRelatedGoodsInforBLL();
        lst = customerRelatedGoodsInforBLL.GetAllCustomerRelatedGoodsInfors(strHQL);

        CustomerRelatedGoodsInfor customerRelatedGoodsInfor = (CustomerRelatedGoodsInfor)lst[0];

        return customerRelatedGoodsInfor.ID.ToString();
    }

    //ȡ���û������Ĺ�Ӧ��������ϸ�����ID��
    public static string GetMyCreatedMaxVendorRelatedGoodsInforID(string strVendorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from VendorRelatedGoodsInfor as vendorRelatedGoodsInfor where vendorRelatedGoodsInfor.VendorCode = " + "'" + strVendorCode + "'" + " Order by vendorRelatedGoodsInfor.ID DESC";
        VendorRelatedGoodsInforBLL vendorRelatedGoodsInforBLL = new VendorRelatedGoodsInforBLL();
        lst = vendorRelatedGoodsInforBLL.GetAllVendorRelatedGoodsInfors(strHQL);

        VendorRelatedGoodsInfor vendorRelatedGoodsInfor = (VendorRelatedGoodsInfor)lst[0];

        return vendorRelatedGoodsInfor.ID.ToString();
    }

    //ȡ���û������ĺ�ͬ�ջ��ƻ���ϸ�����ID��
    public static string GetMyCreatedMaxConstractGoodsReceiptPlanID(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractGoodsReceiptPlan as constractGoodsReceiptPlan where constractGoodsReceiptPlan.ConstractCode = " + "'" + strConstractCode + "'" + " Order by constractGoodsReceiptPlan.ID DESC";
        ConstractGoodsReceiptPlanBLL constractGoodsReceiptPlanBLL = new ConstractGoodsReceiptPlanBLL();
        lst = constractGoodsReceiptPlanBLL.GetAllConstractGoodsReceiptPlans(strHQL);

        ConstractGoodsReceiptPlan constractGoodsReceiptPlan = (ConstractGoodsReceiptPlan)lst[0];

        return constractGoodsReceiptPlan.ID.ToString();
    }

    //ȡ���û������ĺ�ͬ�ջ���¼��ϸ�����ID��
    public static string GetMyCreatedMaxConstractGoodsReceiptRecordID(string strReceiptPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractGoodsReceiptRecord as constractGoodsReceiptRecord where constractGoodsReceiptRecord.ReceiptPlanID = " + strReceiptPlanID + " Order by constractGoodsReceiptRecord.ID DESC";
        ConstractGoodsReceiptRecordBLL constractGoodsReceiptRecordBLL = new ConstractGoodsReceiptRecordBLL();
        lst = constractGoodsReceiptRecordBLL.GetAllConstractGoodsReceiptRecords(strHQL);

        ConstractGoodsReceiptRecord constractGoodsReceiptRecord = (ConstractGoodsReceiptRecord)lst[0];

        return constractGoodsReceiptRecord.ID.ToString();
    }

    //ȡ���û������ĺ�ͬ�����ƻ���ϸ�����ID��
    public static string GetMyCreatedMaxConstractGoodsDeliveryPlanID(string strConstractCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractGoodsDeliveryPlan as constractGoodsDeliveryPlan where constractGoodsDeliveryPlan.ConstractCode = " + "'" + strConstractCode + "'" + " Order by constractGoodsDeliveryPlan.ID DESC";
        ConstractGoodsDeliveryPlanBLL constractGoodsDeliveryPlanBLL = new ConstractGoodsDeliveryPlanBLL();
        lst = constractGoodsDeliveryPlanBLL.GetAllConstractGoodsDeliveryPlans(strHQL);

        ConstractGoodsDeliveryPlan constractGoodsDeliveryPlan = (ConstractGoodsDeliveryPlan)lst[0];

        return constractGoodsDeliveryPlan.ID.ToString();
    }

    //ȡ���û������ĺ�ͬ������¼��ϸ�����ID��
    public static string GetMyCreatedMaxConstractGoodsDeliveryRecordID(string strDeliveryPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractGoodsDeliveryRecord as constractGoodsDeliveryRecord where constractGoodsDeliveryRecord.DeliveryPlanID = " + strDeliveryPlanID + " Order by constractGoodsDeliveryRecord.ID DESC";
        ConstractGoodsDeliveryRecordBLL constractGoodsDeliveryRecordBLL = new ConstractGoodsDeliveryRecordBLL();
        lst = constractGoodsDeliveryRecordBLL.GetAllConstractGoodsDeliveryRecords(strHQL);

        ConstractGoodsDeliveryRecord constractGoodsDeliveryRecord = (ConstractGoodsDeliveryRecord)lst[0];

        return constractGoodsDeliveryRecord.ID.ToString();
    }

    //ȡ���û��������ʲ���������ID��
    public static string GetMyCreatedMaxAssetApplicationID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetApplication as assetApplication where assetApplication.ApplicantCode = " + "'" + strUserCode + "'" + " Order by assetApplication.AAID DESC";
        AssetApplicationBLL assetApplicationBLL = new AssetApplicationBLL();
        lst = assetApplicationBLL.GetAllAssetApplications(strHQL);

        AssetApplication assetApplication = (AssetApplication)lst[0];

        return assetApplication.AAID.ToString();
    }

    //ȡ���û��������ʲ��������ϸ���ID��
    public static string GetMyCreatedMaxAssetApplicationDetailID(string strAAID)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetApplicationDetail as assetApplicationDetail where assetApplicationDetail.AAID = " + strAAID;
        AssetApplicationDetailBLL assetApplicationDetailBLL = new AssetApplicationDetailBLL();
        lst = assetApplicationDetailBLL.GetAllAssetApplicationDetails(strHQL);

        AssetApplicationDetail assetApplicationDetail = (AssetApplicationDetail)lst[0];

        return assetApplicationDetail.ID.ToString();
    }

    //ȡ���ʲ���ⵥ���ĵ���
    public static int GetMyCreatedMaxAssetCheckInDetailID(string strCIOID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_AssetCheckInOrderDetail where CheckInID = " + strCIOID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_AssetCheckInOrderDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ���ʲ����ⵥ���ĵ���
    public static int GetMyCreatedMaxAssetShipmentNO(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ShipmentNO) From T_AssetShipmentOrder Where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_AssetShipmentOrder");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ���ʲ����ⵥ��ϸ�����ID
    public static int GetMyCreatedMaxAssetShipmentDetailID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_AssetShipmentDetail ";
        DataSet ds = GetDataSetFromSql(strHQL, "T_AssetShipmentDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ���û����������µ��ʲ�
    public static string GetMyCreatedMaxAssetCode(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Asset as asset where asset.OwnerCode = " + "'" + strUserCode + "'" + " Order by asset.Number DESC,asset.ID DESC";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);

        Asset asset = (Asset)lst[0];

        return asset.AssetCode.Trim();
    }

    //ȡ���û��������ʲ����µ����¼ID
    public static string GetMyCreatedMaxAssetuserRecordID(string strAssetCode)
    {
        string strHQL = "from AssetUserRecord as assetUserRecord where assetUserRecord.AssetCode = " + "'" + strAssetCode + "'" + " Order by assetUserRecord.ID DESC";
        AssetUserRecordBLL assetUserRecordBLL = new AssetUserRecordBLL();
        IList lst = assetUserRecordBLL.GetAllAssetUserRecords(strHQL);
        AssetUserRecord assetUserRecord = (AssetUserRecord)lst[0];

        return assetUserRecord.ID.ToString();
    }

    //ȡ���û��������ʲ�ά�����¼�¼ID
    public static string GetMyCreatedMaxAssetMtRecordID(string strAssetCode)
    {
        string strHQL = "from AssetMTRecord as assetMTRecord where assetMTRecord.AssetCode = " + "'" + strAssetCode + "'" + " Order by assetMTRecord.ID DESC";

        AssetMTRecordBLL assetMTRecordBLL = new AssetMTRecordBLL();
        IList lst = assetMTRecordBLL.GetAllAssetMTRecords(strHQL);

        AssetMTRecord assetMTRecord = (AssetMTRecord)lst[0];

        return assetMTRecord.ID.ToString();
    }

    //ȡ���������۵����ļ�¼��
    public static string GetMyCreatedMaxScheduleDailyWorkID()
    {
        string strHQL;

        strHQL = "Select Max(ReviewID) From T_ScheduleEvent_LeaderReview ";
        DataSet ds = GetDataSetFromSql(strHQL, "T_ScheduleEvent_LeaderReview ");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ����Ƹ��Ա�����ID
    public static string GetMyCreatedMaxCandidateID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_LTCandidateInformation where CreatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_LTCandidateInformation");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ��������ⵥ�����ID
    public static string GetMyCreatedMaxGoodsCheckInID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(CheckInID) From T_GoodsCheckInOrder where CreatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsCheckInOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���������۵������ID
    public static string GetMyCreatedMaxGoodsSaleOrderID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(SOID) From T_GoodsSaleOrder where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsSaleOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���������۵����ļ�¼��
    public static string GetMyCreatedMaxGoodsSaleRecordID(string strSOID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsSaleRecord where SOID = " + strSOID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsSaleRecord");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���������۱��۵������ID
    public static string GetMyCreatedMaxGoodsSaleQuotationOrderID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(QOID) From T_GoodsSaleQuotationOrder where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsSaleQuotationOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���������۵����ļ�¼��
    public static string GetMyCreatedMaxGoodsSaleQuotationOrderDetailID(string strQOID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsSaleQuotationOrderDetail where QOID = " + strQOID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsSaleQuotationOrderDetail");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û����������ϳ���֪ͨ�����ID��
    public static string GetMyCreatedMaxGoodsCheckOutNoticeOrderID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsCheckOutNoticeOrder as goodsCheckOutNoticeOrder where goodsCheckOutNoticeOrder.ApplicantCode = " + "'" + strUserCode + "'" + " Order by goodsCheckOutNoticeOrder.COOID DESC";
        GoodsCheckOutNoticeOrderBLL goodsCheckOutNoticeOrderBLL = new GoodsCheckOutNoticeOrderBLL();
        lst = goodsCheckOutNoticeOrderBLL.GetAllGoodsCheckOutNoticeOrders(strHQL);

        GoodsCheckOutNoticeOrder goodsCheckOutNoticeOrder = (GoodsCheckOutNoticeOrder)lst[0];

        return goodsCheckOutNoticeOrder.COOID.ToString();
    }

    //ȡ���û����������ϳ���֪ͨ����ϸ���ID��
    public static string GetMyCreatedMaxGoodsCheckOutNoticeOrderDetailID(string strAAID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsCheckOutNoticeOrderDetail as goodsCheckOutNoticeOrderDetail where goodsCheckOutNoticeOrderDetail.COOID = " + strAAID;
        GoodsCheckOutNoticeOrderDetailBLL goodsCheckOutNoticeOrderDetailBLL = new GoodsCheckOutNoticeOrderDetailBLL();
        lst = goodsCheckOutNoticeOrderDetailBLL.GetAllGoodsCheckOutNoticeOrderDetails(strHQL);

        GoodsCheckOutNoticeOrderDetail goodsCheckOutNoticeOrderDetail = (GoodsCheckOutNoticeOrderDetail)lst[0];

        return goodsCheckOutNoticeOrderDetail.ID.ToString();
    }

    //ȡ���û�������������������ID��
    public static string GetMyCreatedMaxGoodsApplicationID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsApplication as goodsApplication where goodsApplication.ApplicantCode = " + "'" + strUserCode + "'" + " Order by goodsApplication.AAID DESC";
        GoodsApplicationBLL goodsApplicationBLL = new GoodsApplicationBLL();
        lst = goodsApplicationBLL.GetAllGoodsApplications(strHQL);

        GoodsApplication goodsApplication = (GoodsApplication)lst[0];

        return goodsApplication.AAID.ToString();
    }

    //ȡ���û������������������ϸ���ID��
    public static string GetMyCreatedMaxGoodsApplicationDetailID(string strAAID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsApplicationDetail as goodsApplicationDetail where goodsApplicationDetail.AAID = " + strAAID;
        GoodsApplicationDetailBLL goodsApplicationDetailBLL = new GoodsApplicationDetailBLL();
        lst = goodsApplicationDetailBLL.GetAllGoodsApplicationDetails(strHQL);

        GoodsApplicationDetail goodsApplicationDetail = (GoodsApplicationDetail)lst[0];

        return goodsApplicationDetail.ID.ToString();
    }

    //ȡ������MRP�ƻ����ĵ���
    public static int GetMyCreatedMaxGoodsMrpMainPlanVerID(string strCreatorCode)
    {
        string strHQL;

        strHQL = "Select Max(PlanVerID) From T_ItemMainPlan where CreatorCode = " + "'" + strCreatorCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_ItemMainPlan");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ������MRP�ƻ���ϸ���ĵ���
    public static int GetMyCreatedMaxGoodsMrpMainPlanVerDetailID(string strPlanVerID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_ItemMainPlanDetail where PlanVerID = " + strPlanVerID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_ItemMainPlanDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ��������ⵥ���ĵ���
    public static int GetMyCreatedMaxGoodsCheckInDetailID(string strCIOID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsCheckInOrderDetail where CheckInID = " + strCIOID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsCheckInOrderDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ��������������ID
    public static int GetMyCreatedMaxGoodsID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_Goods";
        DataSet ds = GetDataSetFromSql(strHQL, "T_Goods");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�����ϳ��ⵥ���ĵ���
    public static int GetMyCreatedMaxGoodsShipmentNO(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ShipmentNO) From T_GoodsShipmentOrder Where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsShipmentOrder");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�����ϳ��ⵥ���ĵ���
    public static int GetMyCreatedMaxGoodsBorrowNO(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(BorrowNO) From T_GoodsBorrowOrder Where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsBorrowOrder");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�������˻������ĵ���
    public static int GetMyCreatedMaxGoodsROID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ROID) From T_GoodsReturnOrder Where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsReturnOrder");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�������ͻ������ĵ���
    public static int GetMyCreatedMaxGoodsDeliveryOrderID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(DOID) From T_GoodsDeliveryOrder Where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsDeliveryOrder");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�������ͻ�����ϸ�����ļ�¼��
    public static int GetMyCreatedMaxGoodsDeliveryOrderDetailID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsDeliveryOrderDetail ";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsDeliveryOrderDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ���ʲ��˻������ĵ���
    public static int GetMyCreatedMaxAssetROID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ROID) From T_AssetReturnOrder Where OperatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_AssetReturnOrder");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�������˻�����ϸ�����ID
    public static int GetMyCreatedMaxGoodsReturnDetailID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsReturnDetail ";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsReturnDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ���ʲ��˿ⵥ��ϸ�����ID
    public static int GetMyCreatedMaxAssetReturnDetailID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_AssetReturnDetail ";
        DataSet ds = GetDataSetFromSql(strHQL, "T_AssetReturnDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�����ϳ��ⵥ��ϸ�����ID
    public static int GetMyCreatedMaxGoodsShipmentDetailID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsShipmentDetail ";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsShipmentDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ�����ϳ��ⵥ��ϸ�����ID
    public static int GetMyCreatedMaxGoodsBorrowOrderDetailID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_GoodsBorrowOrderDetail ";
        DataSet ds = GetDataSetFromSql(strHQL, "T_GoodsBorrowOrderDetail");

        return int.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    //ȡ���û����������µ�����
    public static string GetMyCreatedMaxGoodsCode(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Goods as goods where goods.OwnerCode = " + "'" + strUserCode + "'" + " Order by goods.Number DESC,goods.ID DESC";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);

        Goods goods = (Goods)lst[0];

        return goods.GoodsCode.Trim();
    }

    //ȡ���û��������������µ����¼ID
    public static string GetMyCreatedMaxGoodsuserRecordID(string strGoodsCode)
    {
        string strHQL = "from GoodsUserRecord as goodsUserRecord where goodsUserRecord.GoodsCode = " + "'" + strGoodsCode + "'" + " Order by goodsUserRecord.ID DESC";
        GoodsUserRecordBLL goodsUserRecordBLL = new GoodsUserRecordBLL();
        IList lst = goodsUserRecordBLL.GetAllGoodsUserRecords(strHQL);
        GoodsUserRecord goodsUserRecord = (GoodsUserRecord)lst[0];

        return goodsUserRecord.ID.ToString();
    }

    //ȡ���û�����������ά�����¼�¼ID
    public static string GetMyCreatedMaxGoodsMtRecordID(string strGoodsCode)
    {
        string strHQL = "from GoodsMTRecord as goodsMTRecord where goodsMTRecord.GoodsCode = " + "'" + strGoodsCode + "'" + " Order by goodsMTRecord.ID DESC";

        GoodsMTRecordBLL goodsMTRecordBLL = new GoodsMTRecordBLL();
        IList lst = goodsMTRecordBLL.GetAllGoodsMTRecords(strHQL);

        GoodsMTRecord goodsMTRecord = (GoodsMTRecord)lst[0];

        return goodsMTRecord.ID.ToString();
    }

    //ȡ���û����������Ŀͻ������
    public static string GetMyCreatedMaxCustomerQuestionID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.RecorderCode = " + "'" + strUserCode + "'" + " Order by customerQuestion.ID DESC";
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        return customerQuestion.ID.ToString();
    }

    //ȡ���û������Ŀͻ������¼���ļ�¼��
    public static string GetMyCreatedMaxcustomerQuestionDetailID(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestionHandleRecord as customerQuestionHandleRecord where customerQuestionHandleRecord.QuestionID = " + strQuestionID + " Order by customerQuestionHandleRecord.ID DESC";
        CustomerQuestionHandleRecordBLL customerQuestionHandleRecordBLL = new CustomerQuestionHandleRecordBLL();
        lst = customerQuestionHandleRecordBLL.GetAllCustomerQuestionHandleRecords(strHQL);

        CustomerQuestionHandleRecord customerQuestionHandleRecord = (CustomerQuestionHandleRecord)lst[0];

        return customerQuestionHandleRecord.ID.ToString();
    }

    //ȡ���û�������Ա�����Ͻ�����������¼��
    public static string GetMyCreatedMaxEducationExpericenceID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from EducationExperience as educationExperience where educationExperience.UserCode = " + "'" + strUserCode + "'" + " Order by educationExpericence.ID DESC";
        EducationExperienceBLL educationExperienceBLL = new EducationExperienceBLL();
        lst = educationExperienceBLL.GetAllEducationExperiences(strHQL);

        EducationExperience educationExperience = (EducationExperience)lst[0];

        return educationExperience.ID.ToString();
    }

    //ȡ���û�������Ա�����Ϲ�����������¼��
    public static string GetMyCreatedMaxWorkExperienceID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkExperience as workExperience where workExperience.UserCode = " + "'" + strUserCode + "'" + " Order by workExperience.ID DESC";

        WorkExperienceBLL workExperienceBLL = new WorkExperienceBLL();
        lst = workExperienceBLL.GetAllWorkExperiences(strHQL);

        WorkExperience workExperience = (WorkExperience)lst[0];

        return workExperience.ID.ToString();
    }

    //ȡ���û�������Ա�����ϼ�ͥ��Ա���ID��
    public static string GetMyCreatedMaxFamilyMemberID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from FamilyMember as familyMember where familyMember.UserCode = " + "'" + strUserCode + "'" + " Order by familyMember.ID DESC";
        FamilyMemberBLL familyMemberBLL = new FamilyMemberBLL();
        lst = familyMemberBLL.GetAllFamilyMembers(strHQL);

        FamilyMember familyMember = (FamilyMember)lst[0];

        return familyMember.ID.ToString();
    }

    //ȡ���û�������Ա�������춯��¼���ID��
    public static string GetMyCreatedMaxUserTransactionRecordID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_UserTransactionRecord Where UserCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserTransactionRecord");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û����ڹ������ID��
    public static string GetMyCreatedMaxUserAttendanceRule(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserAttendanceRule as userAttendanceRule Where userAttendanceRule.UserCode = " + "'" + strUserCode + "'";
        strHQL += " Order by userAttendanceRule.ID DESC";
        UserAttendanceRuleBLL userAttendanceRuleBLL = new UserAttendanceRuleBLL();
        lst = userAttendanceRuleBLL.GetAllUserAttendanceRules(strHQL);

        UserAttendanceRule userAttendanceRule = (UserAttendanceRule)lst[0];

        return userAttendanceRule.ID.ToString();
    }

    //ȡ���û��������ճ����ID��
    public static string GetMyCreatedMaxScheduleID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Schedule as schedule where schedule.UserCode = " + "'" + strUserCode + "'" + " Order by schedule.ID DESC";
        ScheduleBLL scheduleBLL = new ScheduleBLL();
        lst = scheduleBLL.GetAllSchedules(strHQL);

        ProjectMgt.Model.Schedule schedule = (ProjectMgt.Model.Schedule)lst[0];

        return schedule.ID.ToString();
    }

    // ȡ���û��������������ID��
    public static string GetMyCreatedMaxHeadLineID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from HeadLine as headLine where headLine.PublisherCode =" + "'" + strUserCode + "'" + " Order by headLine.ID DESC";
        HeadLineBLL headLineBLL = new HeadLineBLL();
        lst = headLineBLL.GetAllHeadLines(strHQL);

        HeadLine headLine = (HeadLine)lst[0];

        return headLine.ID.ToString();
    }

    // ȡ���û������������ID��
    public static string GetMyCreatedMaxOfficialDocumentID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from OfficialDocument as officialDocument where officialDocument.PublisherCode =" + "'" + strUserCode + "'" + " Order by officialDocument.ID DESC";
        OfficialDocumentBLL officialDocumentBLL = new OfficialDocumentBLL();
        lst = officialDocumentBLL.GetAllOfficialDocuments(strHQL);

        OfficialDocument officialDocument = (OfficialDocument)lst[0];

        return officialDocument.ID.ToString();
    }

    // ȡ���û��������������ID��
    public static string GetMyCreatedMaxMailSignInfoID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_MailSignInfo Where UserCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailSignInfo");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û������������ϵ�˱��
    public static string GetMyCreatedMaxContactInforID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'" + " Order by contactInfor.ID DESC";
        ContactInforBLL contactInforBLL = new ContactInforBLL();
        lst = contactInforBLL.GetAllContactInfors(strHQL);

        ContactInfor contactInfor = (ContactInfor)lst[0];

        return contactInfor.ID.ToString();
    }

    //ȡ���û���ε�����ID��
    public static string GetMyCreatedMaxMemberLevelID()
    {
        string strHQL;
        IList lst;

        strHQL = "from MemberLevel as memberLevel Order by memberLevel.ID DESC ";
        MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
        lst = memberLevelBLL.GetAllMemberLevels(strHQL);

        MemberLevel memberLevel = (MemberLevel)lst[0];

        return memberLevel.ID.ToString();
    }

    //ȡ�ó������������ID��
    public static string GetMyCreatedMaxCarApplyFormID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_CarApplyForm";
        DataSet ds = GetDataSetFromSql(strHQL, "T_CarApplyForm");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�ó������������ID��
    public static string GetMyCreatedMaxCarAssignFormID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_CarAssignForm";
        DataSet ds = GetDataSetFromSql(strHQL, "T_CarAssignForm");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ��Ա����ְ����ID��
    public static string GetMyCreatedMaxPartTimeJobID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_PartTimeJob";
        DataSet ds = GetDataSetFromSql(strHQL, "T_PartTimeJob");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�üƻ�����ID��
    public static string GetMyCreatedMaxPlanID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(PlanID) From T_Plan Where CreatorCode = " + "'" + strUserCode + "'";
        DataSet ds = GetDataSetFromSql(strHQL, "T_Plan");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�üƻ�Ŀ������ID��
    public static string GetMyCreatedMaxPlanTargetID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_Plan_Target Where PlanID = " + strPlanID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_Plan");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�üƻ��쵼����ID��
    public static string GetMyCreatedMaxPlanRelatedLeaderID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_Plan_RelatedLeader Where PlanID = " + strPlanID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_Plan");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�üƻ��쵼��������ID��
    public static string GetMyCreatedMaxPlanLeaderReviewID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_Plan_LeaderReview Where PlanID = " + strPlanID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_PlanLeaderReview");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�üƻ���־������ID��
    public static string GetMyCreatedMaxPlanWorkLogID(string strPlanID)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_Plan_WorkLog Where PlanID = " + strPlanID;
        DataSet ds = GetDataSetFromSql(strHQL, "T_PlanWorkLog");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ��KPI����ID��
    public static string GetMyCreatedMaxKPIID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_KPILibrary";
        DataSet ds = GetDataSetFromSql(strHQL, "T_KPILibrary");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ��KPIְ��ģ������ID��
    public static string GetMyCreatedMaxKPIDepartPositionTemplateID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_KPITemplateForDepartPosition";
        DataSet ds = GetDataSetFromSql(strHQL, "T_KPITemplateForDepartPosition");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ��Ա��KPI��������ID��
    public static string GetMyCreatedMaxUserKPICheckID()
    {
        string strHQL;

        strHQL = "Select Max(KPICheckID) From T_UserKPICheck";
        DataSet ds = GetDataSetFromSql(strHQL, "T_UserKPICheck");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ��Ա��KPI��������ID��
    public static string GetMyCreatedMaxUserKPICheckDetailID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_UserKPICheckDetail";
        DataSet ds = GetDataSetFromSql(strHQL, "T_UserKPICheckDetail");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ������վ��ַ�ı��
    public static string GetMyCreatedMaxWebSiteID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_WebSite Where UserCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WebSite");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�ñ���ģ������ID��
    public static string GetMyCreatedMaxReportTemplateID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_ReportTemplate Where CreatorCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReportTemplate");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���ϴ��ĵ������ID��
    public static string GetMyCreatedMaxDocID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(DocID) From T_Document Where UploadManCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Document");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ�����ݽ���������ID��
    public static string GetMyCreatedSystemExchangeDBSqlOrderID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_SystemExchangeOrder";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemExchangeOrder");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    #endregion ȡ���û������Ķ�������ID��

    #region ȡ�ø��ֶ���ID�Ż�����

    //ȡ����ĿID��������Ŀ���ƣ�
    public static string GetProjectID(string strProjectName)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where rtrim(ltrim(project.ProjectName)) = " + "'" + strProjectName + "'";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.ProjectID.ToString();
    }

    //ȡ����Ŀ���ƣ�������Ŀ�ţ�
    public static string GetProjectName(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

    //ȡ����Ŀʵʩ��������Ŀ�ţ�
    public static Project GetProject(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project;
    }

    //ȡ����Ŀ״̬��������Ŀ�ţ�
    public static string GetProjectStatus(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.Status.Trim();
    }

    //ȡ����Ŀ������루������Ŀ�ţ�
    public static string GetProjectPMCode(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.PMCode.Trim();
    }

    //ȡ����Ŀ�ƻ��汾
    public static int GetProjectPlanVersion(string strProjectID, string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.Type = " + "'" + strType + "'";
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];
            return projectPlanVersion.VerID;
        }
        else
        {
            return 0;
        }
    }

    //ȡ�ô���Ŀ���͵���Ŀ�Ƿ���ϸ�ڣ��ƻ������񣬹�������Ӱ��
    public static string GetProjectTypeImpactByDetail(string strProjectID)
    {
        string strHQL;

        strHQL = "Select ProgressByDetailImpact From T_Project Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        if (ds.Tables[0].Rows.Count > 0)
        {

            return ds.Tables[0].Rows[0]["ProgressByDetailImpact"].ToString().Trim();

        }
        else
        {
            return "NO";
        }
    }

    //ȡ����Ŀ��������
    public static string GetProjectTaskName(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];
        return projectTask.Task.Trim();
    }

    //ȡ��������󣨸�������ţ�
    public static Defectment GetDefectment(string strDefectID)
    {
        string strHQL = "from Defectment as defectment where defectment.DefectID = " + strDefectID;
        DefectmentBLL defectmentBLL = new DefectmentBLL();

        IList lst = defectmentBLL.GetAllDefectments(strHQL);

        Defectment defectment = (Defectment)lst[0];

        return defectment;
    }

    //ȡ��������󣨸�������ţ�
    public static Requirement GetRequirement(string strReqID)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();

        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement;
    }

    //ȡ�ò�������
    public static string GetDepartName(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "Select DepartName From T_Department Where DepartCode = " + "'" + strDepartCode + "'";
        DataSet ds = GetDataSetFromSqlNOOperateLog(strHQL, "T_Department");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    //ȡ���û����Ŵ���(�����û����룩
    public static string GetParentDepartCodeFromDepartCode(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.ParentCode.Trim();
    }

    //ȡ���û����Ŵ���(�����û����룩
    public static string GetDepartCodeFromUserCode(string strUserCode)
    {
        string strDepartCode, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strDepartCode = projectMember.DepartCode;
        return strDepartCode.Trim();
    }

    //ȡ�ÿͻ��������Ŵ���(���ݿͻ����룩
    public static string GetDepartCodeFromCustomerCode(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Customer as customer Where customer.CustomerCode = " + "'" + strCustomerCode + "'";
        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);
        Customer customer = (Customer)lst[0];

        return customer.BelongDepartCode.Trim();
    }

    //ȡ���û����Ų�Ʒ�����(�����û����룩
    public static string GetDepartRelatedProductLineFromUserCode(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartCode;
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = " From Department as department Where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);
        Department department = (Department)lst[0];

        try
        {
            return department.ProductLineRelated.Trim();
        }
        catch
        {
            return "NO";
        }
    }

    //ȡ���û����ų����û���Ʒ�����(�����û����룩
    public static string GetDepartSuperUserRelatedProductLineFromUserCode(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From DepartRelatedSuperUser as departRelatedSuperUser Where departRelatedSuperUser.UserCode = " + "'" + strUserCode + "'";
        DepartRelatedSuperUserBLL departRelatedSuperUserBLL = new DepartRelatedSuperUserBLL();
        lst = departRelatedSuperUserBLL.GetAllDepartRelatedSuperUsers(strHQL);

        if (lst.Count > 0)
        {
            DepartRelatedSuperUser departRelatedSuperUser = (DepartRelatedSuperUser)lst[0];

            return departRelatedSuperUser.ProductLineRelated.Trim();
        }
        else
        {
            return "NO";
        }
    }

    public static string GetUserSMSCode(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From SMSCode as smsCode Where smsCode.UserCode = " + "'" + strUserCode + "'" + " and to_char(smsCode.SendTime,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'";
        SMSCodeBLL smsCodeBLL = new SMSCodeBLL();
        lst = smsCodeBLL.GetAllSMSCodes(strHQL);

        SMSCode smsCode = new SMSCode();

        if (lst.Count > 0)
        {
            smsCode = (SMSCode)lst[0];

            return smsCode.RandomCode.Trim();
        }
        else
        {
            return "";
        }
    }

    public static string GetWebSiteCustomerServiceOperatorCode(string strWebSite)
    {
        string strHQL;

        if (strWebSite == null)
        {
            strWebSite = "NULL";
        }

        strHQL = "Select UserCode From T_SiteCustomerServiceOperator Where Upper(WebSite) = " + "'" + strWebSite.ToUpper() + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SiteCustomerServiceOperator");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "ADMIN";
        }
    }

    public static string GetUserName(string strUserCode)
    {
        string strHQL;
        string strUserName;

        strHQL = "Select UserName From T_ProjectMember Where UserCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        strUserName = ds.Tables[0].Rows[0][0].ToString().Trim();

        return strUserName;
    }

    //���û�����ȡ���û���
    public static string GetUserCodeByUserName(string strUserName)
    {
        string strHQL;

        strHQL = "Select UserCode From T_ProjectMember Where UserName = " + "'" + strUserName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    public static string GetUserStatus(string strUserCode)
    {
        string strStatus, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strStatus = projectMember.Status.Trim();

        return strStatus;
    }

    public static string GetUserDuty(string strUserCode)
    {
        string strDuty;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strDuty = projectMember.Duty.Trim();
        return strDuty;
    }

    public static string GetUserLangCode(string strUserCode)
    {
        string strLangCode;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            try
            {
                strLangCode = projectMember.LangCode.Trim();
            }
            catch
            {
                strLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            }

            if (strLangCode == "")
            {
                strLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
            }
        }
        else
        {
            strLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
        }

        return strLangCode;
    }

    public static string GetUserJobTitle(string strUserCode)
    {
        string strJobTitle;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        try
        {
            strJobTitle = projectMember.JobTitle.Trim();
            return strJobTitle;
        }
        catch
        {
            return "";
        }
    }

    public static string GetUserType(string strUserCode)
    {
        string strUserType, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            strUserType = projectMember.UserType.Trim();

            return strUserType;
        }
        else
        {
            return "";
        }
    }

    public static Asset GetAsset(string strAssetCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Asset as asset Where asset.AssetCode = " + "'" + strAssetCode + "'";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);
        Asset asset = (Asset)lst[0];

        return asset;
    }

    public static string GetAssetName(string strAssetCode)
    {
        string strHQL;
        IList lst;

        string strAssetName;

        strHQL = "From Asset as asset Where asset.AssetCode = " + "'" + strAssetCode + "'";
        AssetBLL assetBLL = new AssetBLL();
        lst = assetBLL.GetAllAssets(strHQL);
        Asset asset = (Asset)lst[0];

        strAssetName = asset.AssetName;

        return strAssetName;
    }

    public static string GetItemName(string strItemCode)
    {
        string strHQL;
        IList lst;

        string strItemName;

        strHQL = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);
        Item item = (Item)lst[0];

        strItemName = item.ItemName.Trim();

        return strItemName;
    }

    public static string GetItemSmallType(string strItemCode)
    {
        string strHQL;

        strHQL = "Select SmallType From T_Item Where ItemCode = " + "'" + strItemCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    //ȡ����Ʒ����
    public static Goods GetGoods(string strGoodsCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Goods as goods Where goods.GoodsCode = " + "'" + strGoodsCode + "'";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);
        Goods goods = (Goods)lst[0];

        return goods;
    }

    public static string GetGoodsName(string strGoodsCode)
    {
        string strHQL;
        IList lst;

        string strGoodsName;

        strHQL = "From Goods as goods Where goods.GoodsCode = " + "'" + strGoodsCode + "'";
        GoodsBLL goodsBLL = new GoodsBLL();
        lst = goodsBLL.GetAllGoodss(strHQL);
        Goods goods = (Goods)lst[0];

        strGoodsName = goods.GoodsName;

        return strGoodsName;
    }

    //ȡ�����Ͽ����
    public static string GetMaterialsStockNumber(string strGoodsCode)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(Number),0) From T_Goods Where GoodsCode = " + "'" + strGoodsCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        return ds.Tables[0].Rows[0][0].ToString();
    }

    #endregion ȡ�ø��ֶ���ID�Ż�����

    #region RTX��������

    //����û���RTX,�򵥷���
    public static bool AddRTXUser(string strUser, string strDepart)
    {
        string strHQL;
        IList lst;

        string strServerIP;
        string strServerPort;
        string strWebSite;

        RTXSAPILib.RTXSAPIRootObj RootObj;  //����һ��������

        strHQL = "From RTXConfig as rtxConfig";
        RTXConfigBLL rtxConfigBLL = new RTXConfigBLL();
        lst = rtxConfigBLL.GetAllRTXConfigs(strHQL);

        RTXConfig rtxConfig = new RTXConfig();

        for (int i = 0; i < lst.Count; i++)
        {
            rtxConfig = (RTXConfig)lst[i];

            strServerIP = rtxConfig.ServerIP.Trim();
            strServerPort = rtxConfig.ServerPort.ToString();
            strWebSite = rtxConfig.WebSite.Trim();

            if (strServerIP == "" | strServerPort == "")
            {
                return false;
            }

            RootObj = new RTXSAPIRootObj();     //����������
            RootObj.ServerIP = strServerIP; //���÷�����IP
            RootObj.ServerPort = Convert.ToInt16(strServerPort); //���÷������˿�

            //������Ϣ
            try
            {
                RootObj.UserManager.AddUser(strUser, 0);
                RootObj.DeptManager.AddUserToDept(strUser, null, strDepart, false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    //ɾ���û�RTX���򵥷���
    public static bool DeleteRTXUser(string strUser)
    {
        string strHQL;
        IList lst;

        string strServerIP;
        string strServerPort;
        string strWebSite;

        RTXSAPILib.RTXSAPIRootObj RootObj;  //����һ��������

        strHQL = "From RTXConfig as rtxConfig";
        RTXConfigBLL rtxConfigBLL = new RTXConfigBLL();
        lst = rtxConfigBLL.GetAllRTXConfigs(strHQL);

        RTXConfig rtxConfig = new RTXConfig();

        for (int i = 0; i < lst.Count; i++)
        {
            rtxConfig = (RTXConfig)lst[i];

            strServerIP = rtxConfig.ServerIP.Trim();
            strServerPort = rtxConfig.ServerPort.ToString();
            strWebSite = rtxConfig.WebSite.Trim();

            if (strServerIP == "" | strServerPort == "")
            {
                return false;
            }

            RootObj = new RTXSAPIRootObj();     //����������
            RootObj.ServerIP = strServerIP; //���÷�����IP
            RootObj.ServerPort = Convert.ToInt16(strServerPort); //���÷������˿�

            //������Ϣ
            try
            {
                RootObj.UserManager.DeleteUser(strUser);
                return true;
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    //��Ӳ��Ÿ�RTX,�򵥷���
    public static bool AddRTXDepartment(string strDepart, string strParentDepart)
    {
        string strHQL;
        IList lst;

        string strServerIP;
        string strServerPort;
        string strWebSite;

        RTXSAPILib.RTXSAPIRootObj RootObj;  //����һ��������

        strHQL = "From RTXConfig as rtxConfig";
        RTXConfigBLL rtxConfigBLL = new RTXConfigBLL();
        lst = rtxConfigBLL.GetAllRTXConfigs(strHQL);

        RTXConfig rtxConfig = new RTXConfig();

        for (int i = 0; i < lst.Count; i++)
        {
            rtxConfig = (RTXConfig)lst[i];

            strServerIP = rtxConfig.ServerIP.Trim();
            strServerPort = rtxConfig.ServerPort.ToString();
            strWebSite = rtxConfig.WebSite.Trim();

            if (strServerIP == "" | strServerPort == "")
            {
                return false;
            }

            RootObj = new RTXSAPIRootObj();     //����������
            RootObj.ServerIP = strServerIP; //���÷�����IP
            RootObj.ServerPort = Convert.ToInt16(strServerPort); //���÷������˿�

            //������Ϣ
            try
            {
                RootObj.DeptManager.AddDept(strDepart, strParentDepart);

                return true;
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    //ɾ��RTX���ţ��򵥷���
    public static bool DeleteRTXDepartment(string strDepart)
    {
        string strHQL;
        IList lst;

        string strServerIP;
        string strServerPort;
        string strWebSite;

        RTXSAPILib.RTXSAPIRootObj RootObj;  //����һ��������

        strHQL = "From RTXConfig as rtxConfig";
        RTXConfigBLL rtxConfigBLL = new RTXConfigBLL();
        lst = rtxConfigBLL.GetAllRTXConfigs(strHQL);

        RTXConfig rtxConfig = new RTXConfig();

        for (int i = 0; i < lst.Count; i++)
        {
            rtxConfig = (RTXConfig)lst[i];

            strServerIP = rtxConfig.ServerIP.Trim();
            strServerPort = rtxConfig.ServerPort.ToString();
            strWebSite = rtxConfig.WebSite.Trim();

            if (strServerIP == "" | strServerPort == "")
            {
                return false;
            }

            RootObj = new RTXSAPIRootObj();     //����������
            RootObj.ServerIP = strServerIP; //���÷�����IP
            RootObj.ServerPort = Convert.ToInt16(strServerPort); //���÷������˿�

            //������Ϣ
            try
            {
                RootObj.DeptManager.DelDept(strDepart, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    public static bool RTXADDDEPT(int Pdeptid, string Deptid, string name, string info)//��Ӳ���
    {
        //����:��Ӳ���
        //����˵��:Pdeptid:��������()�ϼ����ŵ�ID
        //deptid:���ӵĸò��ŵ�ID
        //name:�����Ӳ��ŵ�����
        //info:�����Ӳ��ŵ������Ϣ

        try
        {
            RTXObjectClass RTXObj = new RTXObjectClass();
            RTXCollectionClass RTXParams = new RTXCollectionClass();
            RTXObj.Name = "USERMANAGER";
            RTXParams.Add("PDEPTID", Pdeptid);
            RTXParams.Add("DEPTID", Deptid);
            RTXParams.Add("NAME", name);
            RTXParams.Add("INFO", info);
            Object iStatus = new Object();
            iStatus = RTXObj.Call2(RTXServerApi.enumCommand_.PRO_ADDDEPT, RTXParams);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool RTXDelDEPT(string dpmtid, string delall)//ɾ������
    {
        //����:ɾ������
        //����˵��:
        //dpmtid:Ҫɾ�����ŵ�ID��
        //delall:ɾ�����ŵ��������ŵ�ѡ��(0Ϊ��ɾ��,Ϊɾ��)
        try
        {
            RTXObjectClass RTXObj = new RTXObjectClass();
            RTXCollectionClass RTXParams = new RTXCollectionClass();
            RTXObj.Name = "USERMANAGER";
            RTXParams.Add("DEPTID", dpmtid);
            RTXParams.Add("COMPLETEDELBS", delall);
            Object iStatus = new Object();
            iStatus = RTXObj.Call2(RTXServerApi.enumCommand_.PRO_DELDEPT, RTXParams);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool RTXADDUSER(string Dpmid, string Nick, string pwd, string name, string rtxnumber, string mobile)//����û�
    {
        //����:����û�
        //����˵��:
        //Dpmid:�û������ڵ�ID��
        //Nick:�û��ĵ�½��
        //pwd:�û��ĵ�½����
        //name:�û���
        //rtxnumber:�û���RTX����
        //mobile:�û����ֻ�����
        try
        {
            RTXObjectClass RTXObj = new RTXObjectClass();
            RTXCollectionClass RTXParams = new RTXCollectionClass();
            RTXObj.Name = "USERMANAGER";
            RTXParams.Add("DEPTID", Dpmid);
            RTXParams.Add("NICK", Nick);
            RTXParams.Add("PWD", pwd);
            RTXParams.Add("NAME", name);
            RTXParams.Add("UIN", rtxnumber);
            RTXParams.Add("MOBILE", mobile);
            Object iStatus = new Object();
            iStatus = RTXObj.Call2(RTXServerApi.enumCommand_.PRO_ADDUSER, RTXParams);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool RTXDelUSR(string unick)//ɾ���û�
    {
        //����:ɾ���û�
        //����˵��:unick:�û��ĵ�½�����û���RTX���붼��
        try
        {
            RTXObjectClass RTXObj = new RTXObjectClass();
            RTXCollectionClass RTXParams = new RTXCollectionClass();
            RTXObj.Name = "USERMANAGER";
            RTXParams.Add("USERNAME", unick);
            Object iStatus = new Object();
            iStatus = RTXObj.Call2(RTXServerApi.enumCommand_.PRO_DELUSER, RTXParams);
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion RTX��������

    #region �ʼ���������

    //������Ϣ���ʼ�
    public static void SendInstantMessage(string strSubject, string strMsg, string strSentUserCode, string strReciverUserCode)
    {
        Msg msg = new Msg();
        try
        {
            msg.SendMSM(strSubject, strReciverUserCode, strMsg, strSentUserCode);
        }
        catch
        {
        }

        //try
        //{
        //    msg.SendMail(strReciverUserCode, strSubject, strMsg, strSentUserCode);
        //}
        //catch
        //{
        //}
    }

    //�����ʼ�����
    public static void ReceiveMail(string POP3Server, string strUserCode, string strLoginName, string strPassword, int intPort, string strDocSavePath)
    {
        string file1, file2;
        Stream decodedDataStream;
        IMail imail = new Mail();
        int nMailID;
        int n = 0;
        int intMailContain = 0;
        int intMailAttachmentContain = 0;

        Folder folder = new Folder();

        POP3_Client _POP3Client = new POP3_Client();
        Mail_Message mime;

        //POP3_ClientMessage message;
        try
        {
            _POP3Client.Connect(POP3Server, intPort);
            _POP3Client.Authenticate(strLoginName, strPassword, true);

            var q = (from POP3_ClientMessage x in _POP3Client.Messages select x).OrderBy(x => -x.SequenceNumber);
            foreach (POP3_ClientMessage message in q)//����������ʼ��ȽϿ�
            {
                try
                {
                    mime = Mail_Message.ParseFromByte(message.HeaderToByte());
                }
                catch
                {
                    continue;
                }

                try
                {
                    ///������ȡ�ʼ��ĸ���
                    mime = Mail_Message.ParseFromByte(message.MessageToByte());

                    if (mime.BodyHtmlText != null)
                    {
                        intMailContain = mime.BodyHtmlText.Length;

                        nMailID = imail.SaveAsMail(mime.Subject, mime.BodyHtmlText, mime.From.ToString(), mime.To.ToString(), mime.Cc == null ? "" : mime.Cc.ToString(), 1,
                        intMailContain, mime.Attachments.Length > 0 ? 1 : 0, 0, folder.GetFolderID("New", strUserCode), strUserCode);
                    }
                    else
                    {
                        if (mime.BodyText != null)
                        {
                            intMailContain = mime.BodyText.Length;

                            nMailID = imail.SaveAsMail(mime.Subject, mime.BodyText, mime.From.ToString().Trim(), mime.To.ToString(), mime.Cc == null ? "" : mime.Cc.ToString(), 1,
                            intMailContain, mime.Attachments.Length > 0 ? 1 : 0, 0, folder.GetFolderID("New", strUserCode), strUserCode);
                        }
                        else
                        {
                            intMailContain = 0;

                            nMailID = imail.SaveAsMail(mime.Subject, "--Null--", mime.From.ToString().Trim(), mime.To.ToString(), mime.Cc == null ? "" : mime.Cc.ToString(), 1,
                            intMailContain, mime.Attachments.Length > 0 ? 1 : 0, 0, folder.GetFolderID("New", strUserCode), strUserCode);
                        }
                    }

                    //��ȡ�ʼ�
                    if (nMailID > 0)
                    {
                        for (n = 0; n < mime.Attachments.Length; n++)
                        {
                            ///��ӵ�������
                            ///
                            try
                            {
                                //�����ǽ��ո����ķ���
                                decodedDataStream = ((MIME_b_SinglepartBase)mime.Attachments[n].Body).GetDataStream();
                                file1 = mime.Attachments[n].ContentType.Param_Name;

                                file1 = Path.GetFileNameWithoutExtension(file1) + DateTime.Now.ToString("MMss") + n.ToString() + Path.GetExtension(file1);

                                file2 = strDocSavePath + "\\" + strUserCode + "\\MailAttachments\\" + file1;
                                using (FileStream fs = File.Create(file2))
                                {
                                    LumiSoft.Net.Net_Utils.StreamCopy(decodedDataStream, fs, 4000);
                                    intMailAttachmentContain = int.Parse(fs.Length.ToString());
                                }

                                ///������ȡ�ʼ��ĸ���
                                imail.SaveAsMailAttachment(
                                    file1,
                                    "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\MailAttachments\\" + file1,
                                    mime.Attachments[n].ContentType.Name,
                                    intMailAttachmentContain,
                                    nMailID);
                            }
                            catch
                            {
                            }
                        }
                    }

                    //ɾ������ȡ���ʼ�
                    message.MarkForDeletion();
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                }

                _POP3Client.Disconnect();
                _POP3Client.Dispose();
                _POP3Client = null;
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //�����ʼ��������޸������ڲ���Ա���໥���ͣ�
    public static bool SendMail(string strUserCode, string strSubject, string strBody, string strSendUserCode)
    {
        int nContain = 0;
        string strHQL;
        IList lst;

        string strTo;
        int nMailID;

        Folder folder = new Folder();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        if (lst.Count == 0)
            return false;

        ProjectMember projectMember = (ProjectMember)lst[0];

        if (projectMember.EMail == null)
            return false;

        strTo = projectMember.EMail.Trim();

        if (strTo == "")
            return false;

        strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strSendUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        lst = mailProfileBLL.GetAllMailProfiles(strHQL);

        if (lst.Count == 0)
            return false;

        MailProfile mailProfile = (MailProfile)lst[0];

        if (mailProfile.Email == null)
            return false;

        ///��ӷ����˵�ַ
        string strFrom = mailProfile.Email.Trim();

        if (strFrom == "")
            return false;

        MailMessage mailMsg = new MailMessage();

        mailMsg.From = new MailAddress(strFrom, mailProfile.UserName.Trim());
        mailMsg.To.Add(strTo);
        nContain += strTo.Length;

        mailMsg.CC.Add(strTo);
        nContain += strTo.Length;

        ///����ʼ�����
        mailMsg.Subject = strSubject;
        nContain += strSubject.Length;

        ///����ʼ�����
        mailMsg.Body = strBody;
        mailMsg.BodyEncoding = Encoding.UTF8;
        mailMsg.IsBodyHtml = true;

        nContain += strBody.Length;

        //nContain += 100;

        try
        {
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            ////�û���
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mailProfile.AliasName.Trim());
            ////����
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", mailProfile.Password.Trim());

            IMail mail = new Mail();
            SmtpClient smtpClient = new SmtpClient(mailProfile.SmtpServerIP, mailProfile.SmtpServerPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailProfile.AliasName.Trim(), mailProfile.Password.Trim());
            /*ָ����δ���������ʼ�*/
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                //�����ʼ�
                smtpClient.Send(mailMsg);

                nMailID = mail.SaveAsMail(mailMsg.Subject, mailMsg.Body, strFrom,
                    strTo, strTo, 1,
                    nContain, mailMsg.Attachments.Count > 0 ? 1 : 0, 1, folder.GetFolderID("Send", strUserCode), strSendUserCode);

                return true;
            }
            catch
            {
                nMailID = mail.SaveAsMail(mailMsg.Subject, mailMsg.Body, strFrom,
                    strTo, strTo, 1,
                    nContain, mailMsg.Attachments.Count > 0 ? 1 : 0, 1, folder.GetFolderID("Waiting", strUserCode), strSendUserCode);

                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    #endregion �ʼ���������

    #region �ļ���COPY��ͼƬ���š����롢��ά�빦��

    //�����ļ���
    //bool copy = CopyDirectory("c:\\temp\\index\\", "c:\\temp\\newindex\\", true);
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
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ret = false;
        }
        return ret;
    }



    //���ļ�������һ������
    public static void AddSpaceLineToFile(string strFileName, string strString)
    {
        try
        {
            string path = HttpContext.Current.Server.MapPath(strFileName);//�ļ���·������֤�ļ����ڡ�
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(strString);
            sw.Close();
            fs.Close();
        }
        catch
        {
        }
    }

    //ȡ�ö�ά��ͼƬ�ļ�URL
    public static string GetQRCodeURLByZXingNet(String strURL, int intWidth, int intHeight)
    {
        try
        {
            var writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE, Options = new QrCodeEncodingOptions { Height = 300, Width = 300, CharacterSet = "UTF-8" } };
            var qrCode = writer.Write(strURL);

            string strFileName = "BarCode" + DateTime.Now.ToString("yyyyMMddHHmmsssssfffffff") + ".gif";
            string strDocSavePath = HttpContext.Current.Server.MapPath("Doc") + "\\Bar\\";
            string strUrl = strDocSavePath + strFileName;

            if (Directory.Exists(strDocSavePath) == false)
            {
                //��������ھʹ���file�ļ���{
                Directory.CreateDirectory(strDocSavePath);
            }

            qrCode.Save(strUrl, System.Drawing.Imaging.ImageFormat.Png);

            return "Doc/Bar/" + strFileName;
        }
        catch (Exception ex)
        {
            //�쳣���
            return ex.Message.ToString();
        }
    }

    //�����룺BarCode, ��ά�� ����ͼ��NoLogoQrCode,��ͼ��HaveLogoQrCode
    public static string ShowQrCodeForTaskAssignRecord(string strAssignID, int intWidth, int intHight)
    {
        string strHQL;
        string strQrCode;

        strHQL = "Select QrCode From T_TaskAssignRecord Where ID = " + strAssignID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TaskAssignRecord");

        strQrCode = ds.Tables[0].Rows[0][0].ToString().Trim();

        return ShareClass.GenerateQrCodeImage(ShareClass.GetBarType(), strQrCode, intWidth, intHight);
    }

    public static string GenerateQrCodeImage(string strBarType, string strQrCodeString, int intWidth, int intHight)
    {
        string strImageUrl;

        try
        {
            try
            {
                System.Drawing.Bitmap imgTemp;

                if (strBarType == "NoLogoQrCode")
                {
                    //����ͼ��ά��
                    imgTemp = BarcodeHelper.GenerateNoLogoQrCode(strQrCodeString, intWidth, intHight);
                }
                else if (strBarType == "HaveLogoQrCode")
                {
                    //��ͼ��ά��
                    imgTemp = BarcodeHelper.GenerateHaveLogoQrCode(strQrCodeString, intWidth, intHight);
                }
                else if (strBarType == "BarCode")
                {
                    //������
                    imgTemp = BarcodeHelper.GenerateBarCode(strQrCodeString, 10, 50);
                }
                else
                {
                    return "";
                }

                ////��ͼ��ά��
                //System.Drawing.Bitmap imgTemp = BarcodeHelper.GenerateHaveLogoQrCode(strQrCodeString, 240, 240);

                string strFileName = strQrCodeString + "BarCode" + DateTime.Now.ToString("yyyyMMddHHmmsssssfffffff") + ".gif";
                string strDocSavePath = HttpContext.Current.Server.MapPath("Doc") + "\\Bar\\";
                string strUrl = strDocSavePath + strFileName;

                if (Directory.Exists(strDocSavePath) == false)
                {
                    //��������ھʹ���file�ļ���{
                    Directory.CreateDirectory(strDocSavePath);
                }

                imgTemp.Save(strUrl, System.Drawing.Imaging.ImageFormat.Gif);

                strImageUrl = "Doc/Bar/" + strFileName;

                return strImageUrl;
            }
            catch
            {
                return "";
            }
        }
        catch
        {
            return "";
        }
    }

    public static string GetBarType()
    {
        string strHQL = "Select Type From T_BarType";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BarType");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "BarType";
        }
    }

    /// <summary>
    /// ͼƬ����
    /// </summary>
    /// <param name="savePath">ͼƬ���·��</param>
    /// <param name="fileName">ͼƬ����</param>
    /// <param name="destWidth">���ſ��</param>
    /// <param name="destHeight">�߶�</param>
    /// <param name="type">1--�̶����ţ�2--���������ţ�3--ָ�����,��ȴ���ָ����Ȱ�ָ����Ƚ��еȱ����ţ�С��ָ����Ȱ�ԭͼ��С�ϴ���4--ԭͼֱ���ϴ�</param>
    /// <returns></returns>
    public static void ReducesPic(string savePath, string fileName, int destWidth, int destHeight, int type)
    {
        if (!fileName.Equals(""))
        {
            //string Allpath = System.Web.HttpContext.Current.Server.MapPath("/") + savePath;

            string Allpath = savePath;

            //����ԭͼ
            System.IO.Stream stream = System.IO.File.OpenRead(Allpath + fileName);
            System.Drawing.Image oImage = System.Drawing.Image.FromStream(stream);
            stream.Close();
            stream.Dispose();

            System.Drawing.Image.GetThumbnailImageAbort callb = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

            string fileType = fileName.Substring(fileName.LastIndexOf(".") + 1);
            int oWidth = oImage.Width;
            int oHeight = oImage.Height;

            int tWidth = destWidth; //��������ͼ��ʼ���
            int tHeight = destHeight; //��������ͼ��ʼ�߶�

            //��ָ���������
            if (type == 1)
            {
                tWidth = destWidth;
                tHeight = destHeight;
            }
            //���������������ͼ�Ŀ�Ⱥ͸߶�
            else if (type == 2)
            {
                if (oWidth > tWidth || oHeight > tHeight)
                {
                    if (oWidth >= oHeight)
                    {
                        tHeight = (int)Math.Floor(Convert.ToDouble(oHeight) * (Convert.ToDouble(tWidth) / Convert.ToDouble(oWidth)));
                    }
                    else
                    {
                        tWidth = (int)Math.Floor(Convert.ToDouble(oWidth) * (Convert.ToDouble(tHeight) / Convert.ToDouble(oHeight)));
                    }
                }
                else
                {
                    tWidth = oWidth; //ԭͼ���
                    tHeight = oHeight; //ԭͼ�߶�
                }
            }
            //ָ�����,��ȴ���ָ����Ȱ�ָ����Ƚ��еȱ����ţ�С��ָ����Ȱ�ԭͼ��С�ϴ�
            else if (type == 3)
            {
                if (oWidth >= tWidth)
                {
                    if (oWidth >= oHeight)
                    {
                        tHeight = (int)Math.Floor(Convert.ToDouble(oHeight) * (Convert.ToDouble(tWidth) / Convert.ToDouble(oWidth)));
                    }
                    else
                    {
                        tWidth = (int)Math.Floor(Convert.ToDouble(oWidth) * (Convert.ToDouble(tHeight) / Convert.ToDouble(oHeight)));
                    }
                }
                else
                {
                    tWidth = oWidth; //ԭͼ���
                    tHeight = oHeight; //ԭͼ�߶�
                }
            }
            else
            {
                tWidth = oWidth; //ԭͼ���
                tHeight = oHeight; //ԭͼ�߶�
            }
            //��������ԭͼ
            oImage = oImage.GetThumbnailImage(tWidth, tHeight, callb, IntPtr.Zero);
            oImage.Save(Allpath + fileName);
        }
    }

    public static bool ThumbnailCallback()
    {
        return false;
    }

    /// <summary>
    /// ��������ͼ
    /// </summary>
    /// <param name="originalImagePath">Դͼ·��������·����</param>
    /// <param name="thumbnailPath">����ͼ·��������·����</param>
    /// <param name="width">����ͼ���</param>
    /// <param name="height">����ͼ�߶�</param>
    /// <param name="mode">��������ͼ�ķ�ʽ</param>
    public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
    {
        System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
        int towidth = width;
        int toheight = height;
        int x = 0;
        int y = 0;
        int ow = originalImage.Width;
        int oh = originalImage.Height; switch (mode)
        {
            case "HW"://ָ���߿����ţ����ܱ��Σ�
                break;

            case "W"://ָ�����߰�����
                toheight = originalImage.Height * width / originalImage.Width;
                break;

            case "H"://ָ���ߣ�������
                towidth = originalImage.Width * height / originalImage.Height;
                break;

            case "Cut"://ָ���߿�ü��������Σ�
                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    oh = originalImage.Height;
                    ow = originalImage.Height * towidth / toheight;
                    y = 0;
                    x = (originalImage.Width - ow) / 2;
                }
                else
                {
                    ow = originalImage.Width;
                    oh = originalImage.Width * height / towidth;
                    x = 0;
                    y = (originalImage.Height - oh) / 2;
                }
                break;

            default:
                break;
        } //�½�һ��bmpͼƬ
        System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight); //�½�һ������
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap); //���ø�������ֵ��
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; //���ø�����,���ٶȳ���ƽ���̶�
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //��ջ�������͸������ɫ���
        g.Clear(System.Drawing.Color.Transparent); //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
        g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
        new System.Drawing.Rectangle(x, y, ow, oh),
        System.Drawing.GraphicsUnit.Pixel); try
        {
            //��jpg��ʽ��������ͼ
            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            originalImage.Dispose();
            bitmap.Dispose();
            g.Dispose();
        }
    }

    #endregion �ļ���COPY��ͼƬ���š����롢��ά�빦��

    #region DataSet,DataGrid,DropDownList ��������

    //����Ŀ������ɫ��
    public static void LoadProjectActorGroupForDropDownList(DropDownList DL_Visible, string strProjectID)
    {
        string strHQL;
        string strLangCode, strUserCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();
        strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();

        string strDepartString;
        string strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();

        if (strSystemVersionType != "GROUP" & strSystemVersionType != "ENTERPRISE")
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialAllDepartmentString();
        }
        else
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityProjectLeader(strUserCode);
        }

        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            strHQL = "Select distinct GroupName,HomeName from T_ActorGroup where (GroupName = 'Individual' or GroupName = 'Entire')";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
        }
        else
        {
            strHQL = "Select distinct GroupName,HomeName from T_ActorGroup where (GroupName = 'Individual' or GroupName = 'Entire' ";
            strHQL += " Or GroupName in (select ActorGroupName from T_RelatedActorGroup where RelatedType = 'Project' and RelatedID = " + strProjectID + ")";
            strHQL += " Or BelongDepartCode in " + strDepartString;
            strHQL += " Or MakeUserCode = " + "'" + strUserCode + "')";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
        DL_Visible.DataSource = ds;
        DL_Visible.DataBind();
    }

    //�󶨽�ɫ�飬ȫ������
    public static void LoadActorGroupDropDownList(DropDownList DL_Visible, string strUserCode)
    {
        string strHQL;

        string strDepartString, strDepartCode, strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select rtrim(GroupName) as GroupName ,rtrim(HomeName) as HomeName from T_ActorGroup where GroupName <> 'Entire' and Type = 'All' ";
        strHQL += " and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ")";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order by SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
        DL_Visible.DataSource = ds;
        DL_Visible.DataBind();
    }

    //�󶨽�ɫ�飬ȫ������
    public static void LoadWorkflowActorGroupDropDownList(DropDownList DL_Visible, string strUserCode)
    {
        string strHQL;

        string strDepartString, strDepartCode, strLangCode;

        strLangCode = HttpContext.Current.Session["LangCode"].ToString();

        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select rtrim(GroupName) as GroupName ,rtrim(HomeName) as HomeName from T_ActorGroup where Type = 'All' ";
        strHQL += " and (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ")";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order by SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
        DL_Visible.DataSource = ds;
        DL_Visible.DataBind();
    }

    //������
    public static void LoadBankForDropDownList(DropDownList DL_Bank)
    {
        string strHQL;
        IList lst;

        strHQL = "From Bank as bank Order By bank.SortNumber ASC";
        BankBLL bankBLL = new BankBLL();
        lst = bankBLL.GetAllBanks(strHQL);

        DL_Bank.DataSource = lst;
        DL_Bank.DataBind();

        DL_Bank.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //�󶨱ұ�
    public static void LoadCurrencyForDropDownList(DropDownList DL_Currency)
    {
        string strHQL;
        IList lst;

        strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        DL_Currency.DataSource = lst;
        DL_Currency.DataBind();

        DL_Currency.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //���ո��ʽ
    public static void LoadReceivePayWayForDropDownList(DropDownList DL_ReAndPayType)
    {
        string strHQL;

        strHQL = "Select rtrim(ltrim(Type)) as Type,SortNumber From T_ReceivePayWay Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReceivePayWay");

        DL_ReAndPayType.DataSource = ds;
        DL_ReAndPayType.DataBind();

        //DL_ReAndPayType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //��Ȩ���г��ͻ�
    public static void LoadCustomer(DropDownList DL_Customer, string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "from Customer as customer ";
        strHQL += " Where (customer.CreatorCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " or (customer.CustomerCode in (Select customerRelatedUser.CustomerCode from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " Or customer.CreatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode In  " + strDepartString + ")";
        strHQL += " Order by customer.CustomerName ASC";

        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DL_Customer.DataSource = lst;
        DL_Customer.DataBind();

        DL_Customer.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //��Ȩ���г���Ӧ�̺ͳа���
    public static void LoadVendorList(DropDownList DL_VendorList, string strUserCode)
    {
        string strHQL;

        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = "Select VendorCode,VendorName from T_Vendor where CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " or VendorCode in (Select VendorCode from T_VendorRelatedUser where UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " Or CreatorCode in (Select UserCode From T_ProjectMember Where DepartCode In  " + strDepartString + ")";
        strHQL += " UNION ";
        strHQL += " Select Code as VendorCode,Name as VendorName From T_BMSupplierInfo where EnterPer = " + "'" + strUserCode + "'";
        strHQL += " or Code in (Select VendorCode from T_VendorRelatedUser where UserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " Or EnterPer in (Select UserCode From T_ProjectMember Where DepartCode In  " + strDepartString + ")";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Vendor");
        DL_VendorList.DataSource = ds;
        DL_VendorList.DataBind();

        DL_VendorList.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //��DataGrid
    public static void DataGridBindingDataSet(string strHQL, DataGrid dataGrid)
    {
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TakeTopTable");

        dataGrid.DataSource = ds;
        dataGrid.DataBind();
    }

    //��DataList
    public static void DataGridBindingDataSet(string strHQL, DataList dataList)
    {
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TakeTopTable");

        dataList.DataSource = ds;
        dataList.DataBind();
    }

    //���ڲ����б�Ա����DATAGRID
    public static int LoadUserByDepartCodeForDataGrid(string strDepartCode, DataGrid dataGrid)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'" + " Order By projectMember.SortNumber ASC";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        dataGrid.DataSource = lst;
        dataGrid.DataBind();

        return lst.Count;
    }

    //���ڲ����б�Ա��KIP��DATAGRID
    public static int LoadUserKPIByDepartCodeForDataGrid(string strDepartString, DataGrid dataGrid)
    {
        string strHQL;

        strHQL = "Select * From V_UserKPIList Where DepartCode in " + strDepartString;
        strHQL += " Order By StartTime DESC,TotalPoint DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "V_UserKPIList");

        dataGrid.DataSource = ds;
        dataGrid.DataBind();

        return ds.Tables[0].Rows.Count;
    }

    //���ڲ����б�Ա����DATAGRID
    public static int LoadUserByDepartStringForDataGrid(string strDepartString, DataGrid dataGrid)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.DepartCode in " + strDepartString + " Order By projectMember.SortNumber ASC";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        dataGrid.DataSource = lst;
        dataGrid.DataBind();

        return lst.Count;
    }

    public static void LoadUnderUserByDutyAndAuthorityAsset(string strDutyKeyWord, string strUserCode, DropDownList DL_Duty)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);

        strHQL = " Select UserCode,UserName From T_ProjectMember Where (Duty in (Select Duty From T_UserDuty Where KeyWord =" + "'" + strDutyKeyWord + "'" + ")";
        strHQL += " or UserCode in (Select UserCode From T_PartTimeJob Where Duty in (Select Duty From T_UserDuty Where KeyWord =" + "'" + strDutyKeyWord + "'" + ")))";
        strHQL += " and DepartCode in " + strDepartString;
        strHQL += " Order By SortNumber ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CarInformation");

        DL_Duty.DataSource = ds;
        DL_Duty.DataBind();

        DL_Duty.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //������Ŀ��Ա�б�DATAGRID
    public static void LoadProjectMemberByProjectIDForDataGrid(string strProjectID, DataGrid dataGrid)
    {
        string strHQL;
        IList lst;

        strHQL = "from RelatedUser as relatedUser Where relatedUser.ProjectID = " + strProjectID;
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

        dataGrid.DataSource = lst;
        dataGrid.DataBind();
    }

    //����ֱ�ӳ�Ա�б�DATAGRID
    public static void LoadMemberByUserCodeForDataGrid(string strUserCode, string strAuthorityType, DataGrid dataGrid)
    {
        string strHQL;
        string strSystemVersionType, strProductType;

        strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();
        strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strProductType == "LOCALSAAS" | strProductType == "SERVERSAAS")
        {
            strHQL = string.Format(@"Select Distinct UserCode,UserName,SortNumber From (Select UserCode,UserName, 1 as SortNumber From T_RelatedUser Where ProjectID in (Select ProjectID From T_Project Where PMCode = '{0}')
                     Union Select UserCode,UserName,2 as SortNumber From T_ProjectMember Where UserCode Not In ( Select UserCode From T_RelatedUser Where ProjectID in (Select ProjectID From T_Project Where PMCode = '{0}'))) C 
                     Order By SortNumber ASC", strUserCode);
        }
        else
        {
            strHQL = "Select A.UnderCode as UserCode,B.UserName From T_MemberLevel A,T_ProjectMember B Where A.UnderCode = B.UserCode  and A.UserCode = " + "'" + strUserCode + "'";
            if (strAuthorityType == "Project")
            {
                strHQL += " and A.ProjectVisible = 'YES'";
            }

            if (strAuthorityType == "Plan")
            {
                strHQL += " and A.PlanVisible = 'YES'";
            }

            if (strAuthorityType == "KPI")
            {
                strHQL += " and A.KPIVisible = 'YES'";
            }

            if (strAuthorityType == "Workload")
            {
                strHQL += " and A.WorkloadVisible = 'YES'";
            }

            if (strAuthorityType == "Schedule")
            {
                strHQL += " and A.ScheduleVisible = 'YES'";
            }

            if (strAuthorityType == "Workflow")
            {
                strHQL += " and A.WorkflowVisible = 'YES'";
            }

            if (strAuthorityType == "CustomerService")
            {
                strHQL += " and A.CustomerServiceVisible = 'YES'";
            }

            if (strAuthorityType == "Constract")
            {
                strHQL += " and A.ConstractVisible = 'YES'";
            }

            if (strAuthorityType == "Position")
            {
                strHQL += " and A.PositionVisible = 'YES'";
            }
            strHQL += " Order By A.SortNumber ASC";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");
        dataGrid.DataSource = ds;
        dataGrid.DataBind();
    }

    //����ֱ�ӳ�Ա�б�DropDownList
    public static void LoadMemberByUserCodeForDropDownList(string strUserCode, DropDownList dropDownList)
    {
        string strHQL;

        string strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();
        if (strSystemVersionType != "GROUP" & strSystemVersionType != "ENTERPRISE" & strSystemVersionType != "SAAS")
        {
            strHQL = "Select A.UserCode,A.UserName From T_ProjectMember A ";
        }
        else
        {
            strHQL = "Select A.UnderCode as UserCode,B.UserName From T_MemberLevel A,T_ProjectMember B Where A.UnderCode = B.UserCode  and A.UserCode = " + "'" + strUserCode + "'";
        }
        strHQL += " Order By A.SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");

        dropDownList.DataSource = ds;
        dropDownList.DataBind();

        dropDownList.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //��λ�б�DropDownList
    public static void LoadUnitForDropDownList(DropDownList DL_Unit)
    {
        string strHQL;
        IList lst;

        strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
        JNUnitBLL jnUnitBLL = new JNUnitBLL();
        lst = jnUnitBLL.GetAllJNUnits(strHQL);
        DL_Unit.DataSource = lst;
        DL_Unit.DataBind();

        DL_Unit.Items.Insert(0, new ListItem("--Select--", ""));
    }

    //����ֱ�ӳ�Ա�б�DropDownList
    public static void LoadPMByUserCodeForDropDownList(string strUserCode, string strDepartString, DropDownList dropDownList)
    {
        string strHQL;

        string strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();
        if (strSystemVersionType == "GROUP" | strSystemVersionType == "ENTERPRISE")
        {
            strHQL = "Select UserCode,UserName From T_ProjectMember Where (UserCode in (Select UnderCode From T_MemberLevel Where Usercode = " + "'" + strUserCode + "'" + ")";
            strHQL += " Or UserCode in (Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString + "))";
            strHQL += "  And Status = 'Employed'";
        }
        else
        {
            strHQL = "Select UserCode,UserName From T_ProjectMember ";
        }
        strHQL += " Order By SortNumber ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");

        dropDownList.DataSource = ds;
        dropDownList.DataBind();
    }

    //��Ǳ�ѡȡ��DATAGRID��Ϊ��ɫ
    public static void ColorDataGridSelectRow(DataGrid dataGrid, DataGridCommandEventArgs e)
    {
        for (int i = 0; i < dataGrid.Items.Count; i++)
        {
            dataGrid.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;
    }

    #endregion DataSet,DataGrid,DropDownList ��������

    #region SQL����\XML����\WebService���÷���


    //ִ��һ�㴦�����
    //��content-type:  application/x-www-from-urlencodeʱ��������ʽΪ��name="zzzz"&id="aaaaa"
    /// <summary>
    /// ִ��һ�㴦�����
    /// ���÷�����string strResult = GetPostDataPage("http://localhost:16422/Web/Handler/test.ashx", "");
    /// </summary>
    /// <param name="posturl"></param>
    /// <param name="postData"></param>
    /// <returns></returns>
    public static string GetPostDataPage(string posturl, string postData)
    {
        Stream outstream = null;
        Stream instream = null;
        StreamReader sr = null;
        //HttpWebResponse response = null;
        HttpWebRequest defectuest = null;
        Encoding encoding = Encoding.UTF8;
        byte[] data = encoding.GetBytes(postData);
        // ׼������...
        try
        {
            // ���ò���
            defectuest = WebRequest.Create(posturl) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            defectuest.CookieContainer = cookieContainer;
            defectuest.AllowAutoRedirect = true;
            defectuest.Method = "POST";
            //defectuest.ContentType = "application/x-www-form-urlencoded";
            defectuest.ContentType = "text/xml";
            defectuest.ContentLength = data.Length;
            outstream = defectuest.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Close();
            //�������󲢻�ȡ��Ӧ��Ӧ����

            //response = defectuest.GetResponse() as HttpWebResponse;
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)defectuest.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }

            //ֱ��defectuest.GetResponse()����ſ�ʼ��Ŀ����ҳ����Post����
            instream = res.GetResponseStream();
            sr = new StreamReader(instream, encoding);
            //���ؽ����ҳ��html������
            string content = sr.ReadToEnd();
            string err = string.Empty;

            return content;
        }
        catch (Exception ex)
        {
            string err = ex.Message;
            return string.Empty;
        }
    }

    //���л�SQL
    public static string Escape(string str)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            sb.Append((Char.IsLetterOrDigit(c)
            || c == '-' || c == '_' || c == '\\'
            || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
        }
        return sb.ToString();
    }

    //�����л�SQL
    public static string UnEscape(string str)
    {
        StringBuilder sb = new StringBuilder();
        int len = str.Length;
        int i = 0;
        while (i != len)
        {
            if (Uri.IsHexEncoding(str, i))
                sb.Append(Uri.HexUnescape(str, ref i));
            else
                sb.Append(str[i++]);
        }
        return sb.ToString();
    }

    //�������ݿ��û�
    public static bool CreateDBUserAccount(string loginUser, string password, string strIsSecurityadmin)
    {
        string cmdText1, cmdText2;

        try
        {
            ////������½�ʻ���create login��
            cmdText1 = string.Format(@"create user {0} with password '{1}';", loginUser, password);
            ShareClass.RunSqlCommand(cmdText1);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }

        try
        {
            cmdText2 = string.Format(@"alter user {0} with password '{1}'; ", loginUser, password);
            ShareClass.RunSqlCommand(cmdText2);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }


        return true;
    }

    //�����û����ݿ�Ȩ��
    public static bool AuthorizationDBToUser(string loginUser, string password, string databasename, string strIsSecurityadmin)
    {
        string cmdText1;
        try
        {
            if (strIsSecurityadmin == "NO")
            {
                ////�����ݿ�ֻ��Ȩ�޸���loginuser
                cmdText1 = string.Format(@"REVOKE CREATE ON SCHEMA public from public;
                    GRANT SELECT ON ALL TABLES IN SCHEMA public TO {0};
                    ALTER DEFAULT PRIVILEGES IN SCHEMA public grant select on tables to {0}; ", loginUser, password);
                ShareClass.RunSqlCommand(cmdText1);
            }
            else
            {
                ////�����ݿ������Ȩ�޸���loginuser������ֻ�ܵ�¼psql��û���κ����ݿ����Ȩ��
                cmdText1 = string.Format(@"REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM {1};
                                 grant all privileges on database {2}{0}{2} to {1};
                                 alter database {2}{0}{2} owner to {1};
                                 ", databasename, loginUser, "\"");
                ShareClass.RunSqlCommand(cmdText1);

                //�����Խ�վ���û�����Ȩ��
                GanttAllPrivilegesToSiteUser(databasename, loginUser);
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }

        return true;
    }

    //�����Խ�վ���û�����Ȩ��
    public static void GanttAllPrivilegesToSiteUser(string strSiteDBName, string strSiteUser)
    {
        string strConnectString;

        try
        {
            // ��ȡ�����ַ���
            strConnectString = ShareClass.GetSiteConnectString(strSiteDBName);
            var conn = new NpgsqlConnection(strConnectString);
            conn.Open();

            // ���� SQL ����
            string sql = string.Format(@"ALTER USER {0} WITH CREATEROLE;GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO {0};ALTER USER {0} WITH SUPERUSER;", strSiteUser);
            var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }
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


    //ֱ��ɾ��ָ��Ŀ¼�µ������ļ�
    public static void DeleteFileUnderDirectory(string strDirectory)
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
                            //ѭ���ݹ�ɾ�����ļ���
                            DeleteFileUnderDirectory(f);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        catch (Exception ex) // �쳣����
        {
        }
    }

    public static void LoadSytemChart(string strUserCode, string strFormType, Repeater RP_ChartList)
    {
        string strHQL, strSql;
        string strLangCode = HttpContext.Current.Session["LangCode"].ToString();
        string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
        HttpContext.Current.Session["DepartString"] = strDepartString;

        strHQL = "Select * From T_SystemAnalystChartRelatedUser Where UserCode = '" + strUserCode + "'" + " and FormType = '" + strFormType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strHQL = "Select trim(A.FormType) as FormType,trim(A.ChartName) as ChartName,(Select trim(SqlCode) From T_SystemAnalystChartManagement Where ChartName = A.ChartName ) as SqlCode,(Select trim(ChartType) From T_SystemAnalystChartManagement Where ChartName = A.ChartName ) as ChartType  From T_SystemAnalystChartRelatedUser A ";
            strHQL += " Where A.UserCode = '" + strUserCode + "' and A.FormType = '" + strFormType + "'";
            strHQL += " Order By A.SortNumber ASC";
        }
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_SystemAnalystChartManagement");

        DataSet dsBackup = ds;

        for (int i = 0; i < dsBackup.Tables[0].Rows.Count; i++)
        {
            strSql = dsBackup.Tables[0].Rows[i]["SqlCode"].ToString();
            strSql = strSql.Replace("[TAKETOPUSERCODE]", strUserCode).Replace("[TAKETOPDEPARTSTRING]", strDepartString).Replace("[TAKETOPLANGCODE]", strLangCode);
            DataSet dsSql = ShareClass.GetDataSetFromSql(strSql, "T_Sql");
            if (dsSql.Tables[0].Rows.Count == 0)
            {
                try
                {
                    ds.Tables[0].Rows[i].Delete();
                }
                catch
                {
                }
            }
        }

        RP_ChartList.DataSource = ds;
        RP_ChartList.DataBind();
    }


    //�Զ�����ϵͳ����
    public static void AutoBackupDataBySystem()
    {
        string strHQL1, strHQL2;
        strHQL1 = "Select * From T_BackDBLog Where to_char(BackTime,'yyyymmdd') = to_char(now(),'yyyymmdd')";
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_BackDBLog");
        if (ds1.Tables[0].Rows.Count == 0)
        {
            try
            {
                //�������ݿ�
                ShareClass.BackupCurrentSiteDB(ShareClass.GetSystemDBName(), ShareClass.GetSystemDBBackupSaveDir(), "Timer", "SELF");
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }

        strHQL2 = "Select * From T_BackDocLog Where to_char(BackTime,'yyyymmdd') = to_char(now(),'yyyymmdd')";
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_BackDocLog");
        if (ds2.Tables[0].Rows.Count == 0)
        {
            try
            {
                //�����ĵ�
                ShareClass.BackupCurrentSiteDoc("Timer");
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }


    //����ƽ̨�ĵ�
    public static int BackupCurrentSiteDoc(string strBackupUser)
    {
        string strDirectory, strBackupPeriodDay, strBackupDirectorySavePath, strBackupDirectory;
        string strDocDirectory;
        int intResult;

        string strBackDocHQL = "select BackDocUrl,BackPeriodDay from T_BackDocPrame";
        DataSet ds = ShareClass.GetDataSetFromSql(strBackDocHQL, "strBackDocHQL");
        if (ds.Tables[0].Rows.Count == 0)
        {
            return -1;
        }
        else
        {
            try
            {
                strDirectory = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            catch
            {
                strDirectory = "";
            }

            if (strDirectory == "")
            {
                return -1;
            }

            try
            {
                strBackupPeriodDay = int.Parse(ds.Tables[0].Rows[0][1].ToString()).ToString();
            }
            catch
            {
                strBackupPeriodDay = "0";
            }
        }

        strBackupDirectory = DateTime.Now.ToString("yyyyMMdd");
        strBackupDirectorySavePath = strDirectory + "\\" + strBackupDirectory;

        if (strDirectory != "")
        {
            intResult = ShareClass.CreateDirectory(strBackupDirectorySavePath);
            if (intResult == 2)
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���棬����Ŀ¼����"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
                return -1;
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click033", "alert('" + LanguageHandle.GetWord("ZZJGBFMLBNWKJC").ToString().Trim() + "')", true);
            return 1;
        }

        bool blCopy;
        string strFromDirectory, strToDirectory;
        string strErrorMsg = "";

        try
        {
            try
            {
                //�жϵ����ǲ��ǵ�һ�α���
                if (GetCurrentMonthBackupNumber() == 0)
                {
                    strDocDirectory = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
                    strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\" + strDocDirectory + "\\";
                    strToDirectory = strBackupDirectorySavePath + "\\" + strDocDirectory;
                    blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
                }
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy Doc directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy Doc
                strDocDirectory = DateTime.Now.ToString("yyyyMM");
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\" + strDocDirectory + "\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + strDocDirectory;
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy Doc directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy WorkflowTemplate
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\WorkFlowTemplate\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + "WorkFlowTemplate";
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy WorkflowTemplateL directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy XML
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\XML\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + "XML";
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy XML directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy UserPhoto
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\UserPhoto\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + "UserPhoto";
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy UserPhoto directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy Report
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\Report\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + "Report";
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy Report directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy Bar
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\Bar\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + "Bar";
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy Bar directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy RTXAccount
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\RTXAccount\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + "RTXAccount";
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy RTXAccount directory Error: " + err.Message.ToString() + ";";
            }

            try
            {
                //copy Log
                strFromDirectory = HttpContext.Current.Server.MapPath("Doc") + "\\Log\\";
                strToDirectory = strBackupDirectorySavePath + "\\" + "Log";
                blCopy = ShareClass.CopyDirectory(strFromDirectory, strToDirectory + "\\", false);
            }
            catch (Exception err)
            {
                strErrorMsg += "Copy Log directory Error: " + err.Message.ToString() + ";";
            }

            //д��־
            string strInsertBackLogHQL = string.Format(@"insert into T_BackDocLog(BackTime,BackDocUrl,UserCode,UserName,IsSucc) values('{0}','{1}','{2}','{3}',1)",
                DateTime.Now, strBackupDirectorySavePath, strBackupUser, strBackupUser);
            ShareClass.RunSqlCommand(strInsertBackLogHQL);

            if (strErrorMsg == "")
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click044", "alert('" + LanguageHandle.GetWord("ZZBeiFenChengGong").ToString().Trim() + "')", true);
                return 1;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click055", "alert('" + LanguageHandle.GetWord("ZZBeiFenShiBaiQingJianCha").ToString().Trim() + ": " + strErrorMsg + "')", true);
                return -1;
            }
        }
        catch (Exception err)
        {
            //д��־
            string strInsertBackLogHQL = string.Format(@"insert into T_BackDocLog(BackTime,BackDocUrl,UserCode,UserName,IsSucc) values('{0}','{1}','{2}','{3}',0)",
                DateTime.Now, strBackupDirectorySavePath, strBackupUser, strBackupUser);
            ShareClass.RunSqlCommand(strInsertBackLogHQL);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click066", "alert('" + LanguageHandle.GetWord("ZZBeiFenShiBaiQingJianCha").ToString().Trim() + ": " + err.Message.ToString() + "')", true);
            return -1;
        }
    }

    //ȡ�õ��±��ݴ���
    public static int GetCurrentMonthBackupNumber()
    {
        string strHQL;
        strHQL = "Select * From T_BackDocLog Where extract(year from BackTime) = extract(year from now()) And extract(month from BackTime) = extract(month from now())";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BackDocLog");

        return ds.Tables[0].Rows.Count;
    }

    //ȡ�����±����ĵ�ʱ��
    public static string GetAllreadyBackupDocLastestTime()
    {
        string strHQL;
        strHQL = "Select Max(BackTime) From T_BackDocLog";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BackDocLog");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    //ȡ�����±����ĵ�ʱ��
    public static string GetAllreadyBackupDBLastestTime()
    {
        string strHQL;
        strHQL = "Select Max(BackTime) From T_BackDBLog";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BackDBLog");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    //ȡ���ϴα���ʱ�䵽���ڵ��·�
    public static int GetBackupDBLastestTimeDifferMonth()
    {
        string strHQL;
        strHQL = "Select * From T_BackDocLog";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BackDBLog");
        if (ds.Tables[0].Rows.Count == 0)
        {
            return 10;
        }

        strHQL = "Select (DATE_PART('year', now()::date) - DATE_PART('year', Max(BackTime)::date)) * 12 +(DATE_PART('month', now()::date) - DATE_PART('month', Max(BackTime)::date)) From T_BackDocLog";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_BackDBLog");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 10;
        }
    }

    //ȡ��������Դ�ļ���KEYֵ
    public static string GetLanguageResourceKeyValue(string strLangCode, string strKey)
    {
        string strResouceFile = "lang." + strLangCode.Trim() + ".resx";
        if (!String.IsNullOrEmpty(strKey))
        {
            return HttpContext.GetGlobalResourceObject(strResouceFile, strKey).ToString();
        }
        else
        {
            return null;
        }
    }

    //�첽ִ��ҳ��
    public static void SyncProjectPlanSchedule(string strURL)
    {
        string strSPInterfaceURL;

        new System.Threading.Thread(delegate ()
        {
            strSPInterfaceURL = strURL;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strSPInterfaceURL);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            try
            {
                using (WebResponse wr = req.GetResponse())
                {
                    //������Խ��յ���ҳ�����ݽ��д���
                }
            }
            catch
            {
            }

        }).Start();
    }


    /*  ��̬����WebServiceʾ��
        //string url = "http://www.webxml.com.cn/WebServices/WeatherWebservice.asmx";
        //string[] args = new string[1];
        //args[0] = "����";
        //object result = ShareClass.InvokeWebService(url, "getWeatherbyCityName", args);
        //Response.Write(result.ToString());
        //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + result.ToString() + "��');</script>");
    */

    //ִ��һ�㴦�����
    //��content-type:  application/x-www-from-urlencodeʱ��������ʽΪ��name="zzzz"&id="aaaaa"
    public static string GetResponseByPost(string apiUrl, string queryString)
    {
        string responseString = string.Empty;
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl + queryString);
        request.ContentType = "text/html";
        request.Method = "POST";
        request.ContentLength = queryString.Length;
        request.Timeout = 20000;
        byte[] bytes = Encoding.UTF8.GetBytes(queryString);
        Stream os = null;
        try
        { // send the Post
            request.ContentLength = bytes.Length;   //Count bytes to send
            os = request.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);         //Send it
        }
        catch (WebException ex)
        {
            throw ex;
        }
        finally
        {
            if (os != null)
            {
                os.Close();
            }
        }

        HttpWebResponse response = null;
        try
        {
            response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseString = reader.ReadToEnd();
            }
        }
        catch (Exception ex2)
        {
            throw ex2;
        }
        finally
        {
            if (response != null)
                response.Close();
        }
        return responseString;
    }

    #region InvokeWebService

    /// < summary>
    /// ��̬����web����
    /// < /summary>
    /// < param name="url">WSDL�����ַ< /param>
    /// < param name="methodname">������< /param>
    /// < param name="args">����< /param>
    /// < returns>< /returns>
    public static object InvokeWebService(string url, string methodname, object[] args)
    {
        return InvokeWebService(url, null, methodname, args);
    }

    /// < summary>
    /// ��̬����web����
    /// < /summary>
    /// < param name="url">WSDL�����ַ< /param>
    /// < param name="classname">����< /param>
    /// < param name="methodname">������< /param>
    /// < param name="args">����< /param>
    /// < returns>< /returns>
    public static object InvokeWebService(string url, string classname, string methodname, object[] args)
    {
        string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
        if ((classname == null) || (classname == ""))
        {
            classname = GetWsClassName(url);
        }

        try
        {
            //��ȡWSDL
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url + "?WSDL");
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);

            //���ɿͻ��˴��������
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider icc = new CSharpCodeProvider();

            //�趨�������
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");

            //���������
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }

            //���ɴ���ʵ���������÷���
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            Type t = assembly.GetType(@namespace + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);
            System.Reflection.MethodInfo mi = t.GetMethod(methodname);

            return mi.Invoke(obj, args);

            /*
            PropertyInfo propertyInfo = type.GetProperty(propertyname);
            return propertyInfo.GetValue(obj, null);
            */
        }
        catch (Exception ex)
        {
            throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
        }
    }

    private static string GetWsClassName(string wsUrl)
    {
        string[] parts = wsUrl.Split('/');
        string[] pps = parts[parts.Length - 1].Split('.');

        return pps[0];
    }

    #endregion InvokeWebService

    //���湤����XML���ݵ���������
    public static bool UpdateWFXMLData(string strXMLName, string strWFID)
    {
        string strHQL;
        int intStart, intLength;
        string strWFXMLData;
        string strNameSpacePrefix, strNameSpaceURI;
        string strRootName, strChildNodeName;

        try
        {
            //������XML���ݱ�����WFXMLData��
            XmlDocument document = new XmlDocument();
            document.Load(strXMLName);

            XmlElement root = document.DocumentElement;

            strRootName = root.Name;

            root.RemoveAttribute("xmlns:xsi");
            root.RemoveAttribute("xmlns:xd");
            root.RemoveAttribute("xmlns:my");

            strNameSpacePrefix = root.Prefix;
            strNameSpaceURI = root.NamespaceURI;

            strChildNodeName = root.ChildNodes[0].Name;

            strWFXMLData = xmlDocument2String(document);
            intStart = strWFXMLData.IndexOf("<" + strChildNodeName);
            intLength = strWFXMLData.Length - intStart;
            strWFXMLData = strWFXMLData.Substring(intStart, intLength);

            strWFXMLData = "<" + strRootName + ">" + strWFXMLData;

            strWFXMLData = strWFXMLData.Replace(strNameSpacePrefix + ":", "");

            strHQL = "Update T_WorkFlow Set WFXMLData = " + "'" + strWFXMLData + "'" + " Where WLID = " + strWFID;
            RunSqlCommand(strHQL);

            return true;
        }
        catch
        {
            return false;
        }
    }

    //��XML�ĵ�ת���ַ���
    public static string xmlDocument2String(XmlDocument doc)
    {
        MemoryStream stream = new MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        doc.Save(writer);
        StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
        stream.Position = 0;
        string xmlstring = sr.ReadToEnd();
        sr.Close();
        stream.Close();
        return xmlstring;
    }

    //SQLȡ�����ݼ�
    public static DataSet GetDataSetFromSql(string strHQL, string strTableName)
    {
        DataSet dataSet = new DataSet();
        NpgsqlConnection npgsqlConnection = null;
        NpgsqlTransaction transaction = null;

        try
        {
            // ��������
            npgsqlConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
            npgsqlConnection.Open();

            // ��ʼ�������ø��뼶��Ϊ Read Committed��Ĭ�ϣ�
            transaction = npgsqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

            // ���������������
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(strHQL, npgsqlConnection, transaction);
            npgsqlCommand.CommandTimeout = 1000;

            // ������������������������
            NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(npgsqlCommand);
            npgsqlDataAdapter.SelectCommand.CommandTimeout = 1000;

            // ������ݼ�
            npgsqlDataAdapter.Fill(dataSet, strTableName);

            // �ύ����
            transaction.Commit();

            // ��¼������־
            InsertUserOperateLog(strHQL);
        }
        catch (Exception ex)
        {
            // �ع�����
            transaction?.Rollback();
            // ��¼������־���׳��쳣
            throw new Exception("An error occurred while executing the query.", ex);
        }
        finally
        {
            // �ر�����
            npgsqlConnection?.Close();
            // �ͷ���Դ
            transaction?.Dispose();
            npgsqlConnection?.Dispose();
        }

        return dataSet;
    }

    //SQLȡ�����ݼ�,ִ�в�����־��������־��
    public static DataSet GetDataSetFromSqlNOOperateLog(string strHQL, string strTableName)
    {
        DataSet dataSet = new DataSet();
        NpgsqlConnection npgsqlConnection = null;
        NpgsqlTransaction transaction = null;

        try
        {
            // ��������
            npgsqlConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
            npgsqlConnection.Open();

            // ��ʼ�������ø��뼶��Ϊ Read Committed��Ĭ�ϣ�
            transaction = npgsqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

            // ���������������
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(strHQL, npgsqlConnection, transaction);
            npgsqlCommand.CommandTimeout = 1000;

            // ������������������������
            NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(npgsqlCommand);
            npgsqlDataAdapter.SelectCommand.CommandTimeout = 1000;

            // ������ݼ�
            npgsqlDataAdapter.Fill(dataSet, strTableName);

            // �ύ����
            transaction.Commit();
        }
        catch (Exception ex)
        {
            // �ع�����
            transaction?.Rollback();
            // ��¼������־���׳��쳣
            throw new Exception("An error occurred while executing the query.", ex);
        }
        finally
        {
            // �ر�����
            npgsqlConnection?.Close();
            // �ͷ���Դ
            transaction?.Dispose();
            npgsqlConnection?.Dispose();
        }

        return dataSet;
    }

    //����SQL���
    public static void RunSqlCommand(string strCmdText)
    {
        NpgsqlConnection myConnection = null;
        NpgsqlTransaction transaction = null;

        try
        {
            // ��������
            myConnection = new NpgsqlConnection(
                ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
            myConnection.Open();

            // ��ʼ����
            transaction = myConnection.BeginTransaction();

            // ���������������
            using (NpgsqlCommand myCommand = new NpgsqlCommand(strCmdText, myConnection, transaction))
            {
                myCommand.CommandTimeout = 600;

                // ִ�� SQL ����
                myCommand.ExecuteNonQuery();

                // �ύ����
                transaction.Commit();
            }

            // �����û�������־����־���������ύ��ִ�У�
            InsertUserOperateLog(strCmdText);
        }
        catch (Exception ex)
        {
            // �ع�����
            transaction?.Rollback();
            // ��¼������־���׳��쳣
            throw new Exception("An error occurred while executing the SQL command.", ex);
        }
        finally
        {
            // �ر�����
            myConnection?.Close();
            // �ͷ�������Դ
            transaction?.Dispose();
            myConnection?.Dispose();
        }
    }

    //����SQL���,ִ�в�����־��������־��
    public static void RunSqlCommandForNOOperateLog(string strCmdText)
    {
        NpgsqlConnection myConnection = null;
        NpgsqlTransaction transaction = null;

        try
        {
            // ��������
            myConnection = new NpgsqlConnection(
                ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
            myConnection.Open();

            // ��ʼ����
            transaction = myConnection.BeginTransaction();

            // ���������������
            using (NpgsqlCommand myCommand = new NpgsqlCommand(strCmdText, myConnection, transaction))
            {
                myCommand.CommandTimeout = 600;

                // ִ�� SQL ����
                myCommand.ExecuteNonQuery();

                // �ύ����
                transaction.Commit();
            }

        }
        catch (Exception ex)
        {
            // �ع�����
            transaction?.Rollback();
            // ��¼������־���׳��쳣
            throw new Exception("An error occurred while executing the SQL command.", ex);
        }
        finally
        {
            // �ر�����
            myConnection?.Close();
            // �ͷ�������Դ
            transaction?.Dispose();
            myConnection?.Dispose();
        }
    }



    //���д����ز����Ĵ洢����
    public static void RunSQLProcedure(string pro, List<NpgsqlParameter> values, ref Hashtable htReturn)
    {
        NpgsqlConnection myConnection = null;
        NpgsqlTransaction transaction = null;

        try
        {
            // ��������
            myConnection = new NpgsqlConnection(
                ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
            myConnection.Open();

            // ��ʼ����
            transaction = myConnection.BeginTransaction();

            // ���������������
            using (NpgsqlCommand myCommand = new NpgsqlCommand(pro, myConnection, transaction))
            {
                myCommand.CommandTimeout = 600;
                myCommand.CommandType = CommandType.StoredProcedure; // ����Ϊ�洢����

                // ��Ӳ���
                foreach (NpgsqlParameter sp in values)
                {
                    myCommand.Parameters.Add(sp);
                }

                // ִ�д洢����
                myCommand.ExecuteNonQuery();

                // �ύ����
                transaction.Commit();

                // ��ȡ���������ֵ
                List<string> keys = new List<string>();
                foreach (string key in htReturn.Keys)
                {
                    keys.Add(key);
                }
                foreach (string key in keys)
                {
                    if (myCommand.Parameters.Contains(key))
                    {
                        htReturn[key] = myCommand.Parameters[key].Value?.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // �ع�����
            transaction?.Rollback();
            // ��¼������־���׳��쳣
            throw new Exception("An error occurred while executing the stored procedure.", ex);
        }
        finally
        {
            // �ر�����
            myConnection?.Close();
            // �ͷ�������Դ
            transaction?.Dispose();
            myConnection?.Dispose();
        }
    }


    //���д����ز����Ĵ洢����
    public static DataSet RunSQLProcedure(string pro, List<NpgsqlParameter> values)
    {
        DataSet ds = new DataSet();
        NpgsqlConnection myConnection = null;
        NpgsqlTransaction transaction = null;

        try
        {
            // ��������
            myConnection = new NpgsqlConnection(
                ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString);
            myConnection.Open();

            // ��ʼ����
            transaction = myConnection.BeginTransaction();

            // ���������������
            using (NpgsqlCommand myCommand = new NpgsqlCommand(pro, myConnection, transaction))
            {
                myCommand.CommandTimeout = 600;
                myCommand.CommandType = CommandType.StoredProcedure; // ����Ϊ�洢����

                // ��Ӳ���
                foreach (NpgsqlParameter sp in values)
                {
                    myCommand.Parameters.Add(sp);
                }

                // ��������������
                using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter(myCommand))
                {
                    sda.SelectCommand.CommandTimeout = 600; // ���ó�ʱʱ��
                    sda.Fill(ds); // ������ݼ�
                }

                // �ύ����
                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            // �ع�����
            transaction?.Rollback();
            // ��¼������־���׳��쳣
            throw new Exception("An error occurred while executing the stored procedure.", ex);
        }
        finally
        {
            // �ر�����
            myConnection?.Close();
            // �ͷ�������Դ
            transaction?.Dispose();
            myConnection?.Dispose();
        }

        return ds;
    }



    //�����û�������־����־��
    public static void InsertUserOperateLog(string strHQL)
    {
        try
        {
            // ����Ƿ�������־��¼
            if (System.Configuration.ConfigurationManager.AppSettings["SaveOperateLog"] != "YES")
            {
                return;
            }

            // ����Ƿ�Ϊϵͳ����
            if (strHQL.IndexOf("BySystem") != -1)
            {
                return;
            }

            // ��ȡ�û���Ϣ
            if (HttpContext.Current.Session["UserCode"] == null || HttpContext.Current.Session["UserName"] == null)
            {
                return;
            }

            string strUserCode = HttpContext.Current.Session["UserCode"].ToString().Trim();
            string strUserName = HttpContext.Current.Session["UserName"].ToString();
            string strUserIP = HttpContext.Current.Request.UserHostAddress.Trim();

            // ת�� SQL �еĵ�����
            string escapedHQL = strHQL.Replace("'", "''");

            // ʹ�� Task �첽ִ����־�������
            Task.Run(() => InsertLogAsync(strUserCode, strUserName, strUserIP, escapedHQL));
        }
        catch (Exception ex)
        {
            // ��¼��־����ʧ�ܵĴ���
            LogError("Failed to insert user operate log.", ex);
        }
    }

    private static void InsertLogAsync(string userCode, string userName, string userIP, string operateContent)
    {
        try
        {
            // ʹ�ò�������ѯ���� SQL ע��
            string strSQL = @"
            INSERT INTO T_UserOperateLog (UserCode, UserName, UserIP, OperateContent, OperateTime)
            VALUES (@UserCode, @UserName, @UserIP, @OperateContent, NOW())";

            // ���������б�
            var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("@UserCode", userCode),
            new NpgsqlParameter("@UserName", userName),
            new NpgsqlParameter("@UserIP", userIP),
            new NpgsqlParameter("@OperateContent", operateContent)
        };

            // ִ�� SQL ����
            RunSqlCommandForNOOperateLog(strSQL, parameters);
        }
        catch (Exception ex)
        {
            // ��¼��־����ʧ�ܵĴ���
            LogError("Failed to insert user operate log asynchronously.", ex);
        }
    }

    private static void RunSqlCommandForNOOperateLog(string sql, List<NpgsqlParameter> parameters = null)
    {
        using (var myConnection = new NpgsqlConnection(
            ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString))
        {
            myConnection.Open();

            using (var myCommand = new NpgsqlCommand(sql, myConnection))
            {
                if (parameters != null)
                {
                    myCommand.Parameters.AddRange(parameters.ToArray());
                }

                myCommand.ExecuteNonQuery();
            }
        }
    }

    private static void LogError(string message, Exception ex)
    {
        // �������ʵ����־��¼�߼�������д���ļ������ݿ����־ϵͳ
        Console.Error.WriteLine($"[ERROR] {DateTime.Now}: {message} - {ex.Message}");
    }


    //���˷Ƿ��ַ�����ֹע��ʽ������
    public static bool SqlFilter(string InText)
    {
        string word = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join|or|;|-|+|*|%|";
        if (InText == null)
            return false;

        foreach (string i in word.Split('|'))
        {
            if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
            {
                return true;
            }
        }
        return false;
    }

    //��EXCEL��ȡ�����ݼ�
    public static DataSet ExcelToDataSet(string filenameurl, string table)
    {
        string strConn;
        string extension = Path.GetExtension(filenameurl);

        if (extension.ToLower() == ".xlsx")
        {
            //2013�漰���ϰ汾����
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filenameurl + ";Extended Properties='Excel 12.0;IMEX=1'";
        }
        else
        {
            strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + filenameurl + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
        }
        OleDbConnection conn = new OleDbConnection(strConn);
        OleDbDataAdapter odda = new OleDbDataAdapter("select * from [Sheet1$]", conn);
        DataSet ds = new DataSet();
        odda.Fill(ds, table);
        return ds;
    }

    //����EXCEL�ļ�
    public static void CreateExcel(string strHQL, string fileName, Page page)
    {
        int i = 0, j = 0;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Excel");
        DataTable dt = ds.Tables[0];

        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();
        for (i = 0; i < dg.Items.Count; i++)
        {
            for (j = 0; j < dg.Items[i].Cells.Count; j++)
            {
                dg.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.Charset = "UTF-8";
        page.EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=UTF-8\"/>" + ostrwrite.ToString());
        HttpContext.Current.Response.End();
    }

    //DataGrid����ΪEXCEL
    public static void DataGridExportToExecl(DataGrid dataGrid, string strTableTitle, string strFileName, DataSet ds)
    {
        dataGrid.AllowPaging = false;
        dataGrid.DataSource = ds;

        HttpContext.Current.Response.Charset = "GB2312 ";
        HttpContext.Current.Response.AppendHeader("Content-Disposition ", "attachment;filename= " + strFileName);

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
        HttpContext.Current.Response.ContentType = "application/ms-excel ";

        //dataGrid.Page.EnableViewState = false;

        System.IO.StringWriter tw = new System.IO.StringWriter();

        System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(tw);

        dataGrid.RenderControl(hw);


        HttpContext.Current.Response.Write(" <form runat=server> " + strTableTitle + tw.ToString() + " </form> ");
        HttpContext.Current.Response.End();
    }


    //ModifyWebConfigDBConnectionString �޸�web.config���������ݿ���ַ�����ƽ̨���ƺ��Ƿ�OEM��
    public static bool ModifyWebConfigDBConnectionStringAndSystemName(string strSiteDirectory, string NhibernateConnectionString, string SQLConnectionString, string GanttSQLConnectionString, string strDBOwerID, string strPassword, string strDBName, string strSysteName, string strSiteAppURL, string strRentProductType, string strRentProductVersionType, string strIsOEM)
    {
        try
        {
            string strDBServerName;

            string strWebConfigFile = strSiteDirectory + "\\web.config";

            System.IO.FileInfo FileInfo = new System.IO.FileInfo(strWebConfigFile);
            if (!FileInfo.Exists)
            {
                //throw new InstallException("Missing config file :" + config);
            }
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(FileInfo.FullName);

            bool FoundIt = false;

            //�޸�NHibernate���ݿ����Ӳ���
            strDBServerName = GetDBServerName("connection.connection_string");
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["hibernate-configuration"]["session-factory"])
            {
                if (Node.Name == "property")
                {
                    if (Node.Attributes.GetNamedItem("name").Value == NhibernateConnectionString)
                    {
                        Node.InnerText = String.Format("Server={0};Database={1};User ID={2};Password={3};Enlist=true;Pooling=true;Minimum Pool Size=100;Maximum Pool Size=1024;Timeout=1000;", strDBServerName, strDBName, strDBOwerID, strPassword);
                        FoundIt = true;
                    }
                }
            }
            if (!FoundIt)
            {
                //throw new InstallException("Error when writing the config file: web.config");
            }



            //�޸�ƽ̨����
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["appSettings"])
            {
                if (Node.Name == "add")
                {
                    if (Node.Attributes.GetNamedItem("key").Value == "SystemName")
                    {
                        Node.Attributes.GetNamedItem("value").Value = strSysteName;
                        FoundIt = true;
                    }
                }
            }
            if (!FoundIt)
            {
                //throw new InstallException("Error when writing the config file: web.config");
            }

            //�޸�ƽ̨URL
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["appSettings"])
            {
                if (Node.Name == "add")
                {
                    if (Node.Attributes.GetNamedItem("key").Value == "CurrentSite")
                    {
                        Node.Attributes.GetNamedItem("value").Value = strSiteAppURL;
                        FoundIt = true;
                    }
                }
            }
            if (!FoundIt)
            {
                //throw new InstallException("Error when writing the config file: web.config");
            }

            //�޸Ĳ�Ʒ����
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["appSettings"])
            {
                if (Node.Name == "add")
                {
                    if (Node.Attributes.GetNamedItem("key").Value == "ProductType")
                    {
                        Node.Attributes.GetNamedItem("value").Value = strRentProductType;
                        FoundIt = true;
                    }
                }
            }
            if (!FoundIt)
            {
                //throw new InstallException("Error when writing the config file: web.config");
            }

            //�޸Ĳ�Ʒ�汾
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["appSettings"])
            {
                if (Node.Name == "add")
                {
                    if (Node.Attributes.GetNamedItem("key").Value == "GroupVersion")
                    {
                        Node.Attributes.GetNamedItem("value").Value = strRentProductVersionType;
                        FoundIt = true;
                    }
                }
            }
            if (!FoundIt)
            {
                //throw new InstallException("Error when writing the config file: web.config");
            }

            //�޸�Identity�ڵ�ADMINISTRATOR ����
            string strIdentityPassword = GetIdentityUserPassword();
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["system.web"])
            {
                if (Node.Name == "identity")
                {
                    Node.Attributes.GetNamedItem("password").Value = strIdentityPassword;
                    FoundIt = true;
                }
            }
            if (!FoundIt)
            {
                //throw new InstallException("Error when writing the config file: web.config");
            }

            //�޸�OEM�汾����
            if (strIsOEM == "YES")
            {
                foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["appSettings"])
                {
                    if (Node.Name == "add")
                    {

                        if (Node.Attributes.GetNamedItem("key").Value == "IsOEMVersion")
                        {
                            Node.Attributes.GetNamedItem("value").Value = strIsOEM;
                            FoundIt = true;
                        }

                    }
                }
                if (!FoundIt)
                {
                    //throw new InstallException("Error when writing the config file: web.config");
                }
            }
            xmlDocument.Save(FileInfo.FullName);


            //�޸�SQl���ݿ����Ӳ���
            strDBServerName = GetDBServerName("connectionString");
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["connectionStrings"])
            {
                if (Node.Name == "add")
                {
                    if (Node.Attributes.GetNamedItem("name").Value == SQLConnectionString)
                    {
                        Node.Attributes.GetNamedItem("connectionString").Value = String.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};Pooling=true;Minimum Pool Size=100;Maximum Pool Size=1024;Timeout=1000;", strDBServerName, strDBOwerID, strPassword, strDBName);
                        FoundIt = true;

                        xmlDocument.Save(FileInfo.FullName);
                    }

                    if (Node.Attributes.GetNamedItem("name").Value == GanttSQLConnectionString)
                    {
                        Node.Attributes.GetNamedItem("connectionString").Value = String.Format("User Id={0};Password={1};Host={2};Database={3};Unicode=True;Persist Security Info=True;Initial Schema=public;", strDBOwerID, strPassword, strDBServerName, strDBName);
                        FoundIt = true;

                        xmlDocument.Save(FileInfo.FullName);
                    }
                }
            }
            if (!FoundIt)
            {
                //throw new InstallException("Error when writing the config file: web.config");
            }

            return FoundIt;
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            return false;
        }
    }

    //����POSTGRESQL
    public static void ConfigPostgreSqlPGPassFile(string strDBName)
    {
        try
        {
            string strDBUser = ShareClass.GetSystemDBUser();
            string strDBPassword = ShareClass.GetSystemDBPassword();
            string strDBServer = ShareClass.GetSystemDBServer();
            string strDBServerPort = ShareClass.GetSystemDBServerPort();
            if (strDBServer == "127.0.0.1")
            {
                strDBServer = "localhost";
            }

            string strConfigString = strDBServer + ":" + strDBServerPort + ":" + strDBName + ":" + strDBUser + ":" + strDBPassword;

            WriteDataToTextFile.WriteTextFile("PGConfig", "pgpass.conf", strConfigString);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //����OEMվ�����ݿ�
    public static void BackupOEMSiteDB(string strSiteDBName, string strBackupDBSavePath, string strBackupOperatorName)
    {
        int intResult;

        intResult = ShareClass.CreateDirectory(strBackupDBSavePath);
        if (intResult == 2)
        {
            return;
        }

        try
        {
            BackupCurrentSiteDB(strSiteDBName, strBackupDBSavePath, strBackupOperatorName, "OEM");
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //����ƽ̨���ݿ�
    public static int BackupCurrentSiteDB(string strDBName, string strBackupDirectory, string strBackupOperatorName, string strBackupType)
    {
        string strBackupDBName, strBackupDBSavePathName, strBackupDirectorySavePath, strAppDBServer, strAppDBPort, strAppDBPassword;
        int intResult;

        strAppDBServer = ShareClass.GetSystemDBServer();
        strAppDBPort = ShareClass.GetSystemDBServerPort();
        strAppDBPassword = ShareClass.GetSystemDBPassword();

        if (strBackupType == "OEM")
        {
            strBackupDirectorySavePath = strBackupDirectory;
        }
        else
        {
            strBackupDirectorySavePath = strBackupDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd");
        }

        strBackupDBName = strDBName + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".bak";
        strBackupDBSavePathName = strBackupDirectorySavePath + "\\" + strBackupDBName;

        if (strBackupDirectorySavePath != "")
        {
            intResult = ShareClass.CreateDirectory(strBackupDirectorySavePath);
            if (intResult == 2)
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���棬����Ŀ¼����"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
                return 2;
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBFMLBNWKJC").ToString().Trim() + "')", true);
            return -4;
        }

        //����������
        string bat = string.Format("set PGPASSWORD={5}\r\necho on\r\n{0} -h {1} -p {2} -U postgres -F c -b -v -f \"{3}\" {4}",
           HttpContext.Current.Server.MapPath("PGTools") + "\\pg_dump.exe",
            strAppDBServer,
            strAppDBPort,
            strBackupDBSavePathName,
            strDBName.ToLower(),
            strAppDBPassword
            );

        System.IO.File.WriteAllText(strBackupDirectorySavePath + "\\backup.bat", bat);

        Process theProcess = new Process();
        theProcess.StartInfo.FileName = strBackupDirectorySavePath + "\\backup.bat";
        //theProcess.StartInfo.Arguments = arguments;
        theProcess.StartInfo.CreateNoWindow = true;
        theProcess.Start();//��������
        theProcess.WaitForExit();

        //д��־
        string strInsertBackLogHQL = string.Format(@"insert into T_BackDBLog(BackTime,BackDBUrl,UserCode,UserName,IsSucc) values('{0}','{1}','{2}','{3}',1)",
            DateTime.Now, strBackupDBSavePathName, strBackupOperatorName, strBackupOperatorName);
        ShareClass.RunSqlCommand(strInsertBackLogHQL);

        return 0;
    }

    //ȡ�����ݿⱸ��·��
    public static string GetSystemDBBackupSaveDir()
    {
        string strDirectory = "";
        string strBackDBHQL = "select BackDBUrl,BackPeriodDay from T_BackDBPrame";
        DataSet ds = ShareClass.GetDataSetFromSql(strBackDBHQL, "strBackDBHQL");
        if (ds.Tables[0].Rows.Count == 0)
        {
            return strDirectory;
        }
        else
        {
            try
            {
                strDirectory = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            catch
            {
                strDirectory = "";
            }
        }

        return strDirectory;
    }

    //-------------��ģ��վ��ָ����ݿ�--------------------------------------------------------------------
    public static bool RestoreDatabaseFromTemplateDB(string strDBName, string strDBRestoreFile)
    {
        return RestoreDatabase(strDBName, strDBRestoreFile);
    }

    //-------------��OEM�û�վ��ָ����ݿ�--------------------------------------------------------------------
    public static bool RestoreDatabaseFromOEMUserDB(string strDBName, string strDBRestoreFile)
    {
        return RestoreDatabase(strDBName, strDBRestoreFile);
    }

    //-------------��ģ��վ��ָ����ݿ�(���ã�--------------------------------------------------------------------
    public static bool RestoreDatabase(string strDBName, string strDBRestoreFile)
    {
        string strAppDBPasswd, strAppDBServer, strAppDBPort;

        strAppDBPasswd = ShareClass.GetSystemDBPassword();
        strAppDBServer = ShareClass.GetSystemDBServer();
        strAppDBPort = ShareClass.GetSystemDBServerPort();

        string strDirectory = strDBRestoreFile.Substring(0, strDBRestoreFile.LastIndexOf("\\"));

        //����������
        string bat = string.Format("set PGPASSWORD={0}\r\necho on\r\n{5} -h {1} -p {2} -U postgres -w -d {3} -v {4}",
            strAppDBPasswd,
            strAppDBServer,
            strAppDBPort,
            strDBName.ToLower(),
            strDBRestoreFile,
            HttpContext.Current.Server.MapPath("PGTools") + "\\pg_restore.exe");

        System.IO.File.WriteAllText(strDirectory + @"\restore.bat", bat);

        try
        {
            string strHQL;
            //�ж��Ƿ����ͬ�����ݿ�
            strHQL = "SELECT u.datname  FROM pg_catalog.pg_database u where u.datname='" + strDBName.ToLower() + "'";
            if (!IsExistedSqlServerInstanceOrDB(strHQL))
            {
                //create database
                strHQL = string.Format(@"create database {0} ", strDBName.ToLower());
                ShareClass.RunSqlCommand(strHQL);
            }
            Process theProcess = new Process();

            theProcess.StartInfo.FileName = strDirectory + @"\restore.bat";
            //theProcess.StartInfo.Arguments = arguments;
            theProcess.StartInfo.CreateNoWindow = true;
            theProcess.Start();//��������
            theProcess.WaitForExit();

            return true;
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

            return false;
        }
    }

    //�ж��Ƿ�������ݿ�ʵ�������ݿ�
    public static bool IsExistedSqlServerInstanceOrDB(string strHQL)
    {
        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ServerOrDBName");

            if (ds.Tables[0].Rows.Count <= 0)
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static void DeleteSiteDBAndDBLoginUserID(string strDBName, string strDBLoginUserID)
    {
        string strHQL;

        try
        {
            strHQL = string.Format(@"alter database {0} owner to postgres;
                 revoke all on database {0} from {1};", strDBName, strDBLoginUserID);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE datname = '{0}' AND pid<>pg_backend_pid();", strDBName);
            ShareClass.RunSqlCommand(strHQL);

            try
            {
                strHQL = string.Format(@"drop database if exists {0};", strDBName);
                ShareClass.RunSqlCommand(strHQL);
            }
            catch
            {
            }

            strHQL = string.Format(@"DROP OWNED BY {0};", strDBLoginUserID);
            ShareClass.RunSqlCommand(strHQL);

            strHQL = string.Format(@"DROP user {0};", strDBLoginUserID);
            ShareClass.RunSqlCommand(strHQL);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //ȡ�õ�ǰ���ݿ������ʵ������
    public static string GetDBServerName(string strConfigKeyType)
    {
        string strConfigKeyValue;
        string[] strConnectStringList;
        string strServerNameString;
        string strDBServerName = "";

        string strWebConfigFile = HttpContext.Current.Server.MapPath("~/web.config");

        System.IO.FileInfo FileInfo = new System.IO.FileInfo(strWebConfigFile);
        if (!FileInfo.Exists)
        {
            return "";
        }
        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
        xmlDocument.Load(FileInfo.FullName);

        if (strConfigKeyType == "connection.connection_string")
        {
            //�޸����ݿ����Ӳ���
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["hibernate-configuration"]["session-factory"])
            {
                if (Node.Name == "property")
                {
                    if (Node.Attributes.GetNamedItem("name").Value == "connection.connection_string")
                    {
                        //strConfigKeyValue = Node.Attributes.GetNamedItem("value").Value;

                        strConfigKeyValue = Node.InnerText;

                        strConnectStringList = strConfigKeyValue.Split(";".ToCharArray());
                        strServerNameString = strConnectStringList[0];

                        strDBServerName = strServerNameString.Replace("Server=", "");
                    }
                }
            }
        }
        else
        {
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["connectionStrings"])
            {
                if (Node.Name == "add")
                {
                    if (Node.Attributes.GetNamedItem("name").Value == "SQLCONNECTIONSTRING")
                    {
                        strConfigKeyValue = Node.Attributes.GetNamedItem("connectionString").Value;
                        strConnectStringList = strConfigKeyValue.Split(";".ToCharArray());
                        strServerNameString = strConnectStringList[0];

                        strDBServerName = strServerNameString.Replace("Server=", "");
                    }
                }
            }
        }


        return strDBServerName.Trim();
    }

    //ȡ��Identity��ADMINISTRATOR PASSWORD
    public static string GetIdentityUserPassword()
    {
        string strIdentityUserPassword = "";
        string strWebConfigFile = HttpContext.Current.Server.MapPath("~/web.config");

        System.IO.FileInfo FileInfo = new System.IO.FileInfo(strWebConfigFile);
        if (!FileInfo.Exists)
        {
            return "";
        }
        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
        xmlDocument.Load(FileInfo.FullName);

        //�޸����ݿ����Ӳ���
        foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["system.web"])
        {
            if (Node.Name == "identity")
            {
                strIdentityUserPassword = Node.Attributes.GetNamedItem("password").Value;
            }
        }

        return strIdentityUserPassword;
    }


    //ȡ�����ݿ���
    public static string GetSystemDBName()
    {
        string strConnectString, strDBName;
        string[] strConnectStringList;

        strConnectString = ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString;
        strConnectStringList = strConnectString.Split(";".ToCharArray());
        strDBName = strConnectStringList[4];

        strDBName = strDBName.Substring(strDBName.IndexOf("=") + 1, strDBName.Length - strDBName.IndexOf("=") - 1);

        return strDBName;
    }

    //ȡ�����ݿ��û�
    public static string GetSystemDBUser()
    {
        string strConnectString, strDBUser;
        string[] strConnectStringList;

        strConnectString = ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString;
        strConnectStringList = strConnectString.Split(";".ToCharArray());
        strDBUser = strConnectStringList[2];

        strDBUser = strDBUser.Substring(strDBUser.IndexOf("=") + 1, strDBUser.Length - strDBUser.IndexOf("=") - 1);

        return strDBUser;
    }

    //ȡ�����ݿ�����
    public static string GetSystemDBPassword()
    {
        string strConnectString, strDBPassword;
        string[] strConnectStringList;

        strConnectString = ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString;
        strConnectStringList = strConnectString.Split(";".ToCharArray());
        strDBPassword = strConnectStringList[3];

        strDBPassword = strDBPassword.Substring(strDBPassword.IndexOf("=") + 1, strDBPassword.Length - strDBPassword.IndexOf("=") - 1);

        return strDBPassword;
    }

    //ȡ�����ݿ����������
    public static string GetSystemDBServer()
    {
        string strConnectString, strDBServer;
        string[] strConnectStringList;

        strConnectString = ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString;
        strConnectStringList = strConnectString.Split(";".ToCharArray());
        strDBServer = strConnectStringList[0];

        strDBServer = strDBServer.Substring(strDBServer.IndexOf("=") + 1, strDBServer.Length - strDBServer.IndexOf("=") - 1);

        return strDBServer;
    }

    //ȡ�����ݿ�������˿�
    public static string GetSystemDBServerPort()
    {
        string strConnectString, strDBServerPort;
        string[] strConnectStringList;

        strConnectString = ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString;
        strConnectStringList = strConnectString.Split(";".ToCharArray());
        strDBServerPort = strConnectStringList[1];

        strDBServerPort = strDBServerPort.Substring(strDBServerPort.IndexOf("=") + 1, strDBServerPort.Length - strDBServerPort.IndexOf("=") - 1);

        return strDBServerPort;
    }

    //ȡ������վ������ݿ����Ӵ�
    public static string GetRentSiteConnecting(string strRentSiteDBName)
    {
        string strConnectString, strDBName;

        strConnectString = ConfigurationManager.ConnectionStrings["SQLCONNECTIONSTRING"].ConnectionString;
        strDBName = ShareClass.GetSystemDBName();

        strConnectString = strConnectString.Replace("=" + strDBName, "=" + strRentSiteDBName);

        return strConnectString;
    }

    //��������վ���û�����Ȩ��
    public static void GanttAllPrivilegesToUser(string strRentSiteDBName, string strRentSiteUser)
    {
        string strConnectString;

        try
        {
            // ��ȡ�����ַ���
            strConnectString = GetRentSiteConnecting(strRentSiteDBName);
            var conn = new NpgsqlConnection(strConnectString);
            conn.Open();

            // ���� SQL ����
            string sql = "GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO " + strRentSiteUser;
            var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }


    #endregion SQL����\XML����\WebService���÷���

    #region IP��MAC��ַ���ƶ��豸����

    /// <summary>
    /// �õ���ǰ��վ�ĸ���ַ
    /// </summary>
    /// <returns></returns>
    /// <summary>
    public static string GetCurrentSiteRootPath()
    {
        string secure = "off";
        // �Ƿ�ΪSSL��֤վ��

        try
        {
            secure = HttpContext.Current.Request.ServerVariables["HTTPS"];
        }
        catch
        {
            secure = "off";
        }

        string httpProtocol = (secure == "on" ? "https://" : "http://");


        httpProtocol = "http://";
        // ����������
        string serverName = HttpContext.Current.Request.ServerVariables["Server_Name"];
        string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
        // Ӧ�÷�������
        string applicationName = HttpContext.Current.Request.ApplicationPath;

        if (applicationName.Substring(applicationName.Length - 1, 1) != "/")
        {
            return httpProtocol + serverName + (port.Length > 0 ? ":" + port : string.Empty) + applicationName + "/";
        }
        else
        {
            return httpProtocol + serverName + (port.Length > 0 ? ":" + port : string.Empty) + applicationName;
        }

    }

//�õ���ǰ��վ�ĸ���ַ,������վ����,
public static string GetCurrentSiteRootPathNoSiteName()
    {
        // �Ƿ�ΪSSL��֤վ��
        string secure = HttpContext.Current.Request.ServerVariables["HTTPS"];
        string httpProtocol = (secure == "on" ? "https://" : "http://");
        // ����������
        string serverName = HttpContext.Current.Request.ServerVariables["Server_Name"];
        string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
        // Ӧ�÷�������
        string applicationName = HttpContext.Current.Request.ApplicationPath;

        if (applicationName.Substring(applicationName.Length - 1, 1) != "/")
        {
            return httpProtocol + serverName + (port.Length > 0 ? ":" + port : string.Empty) + "/";
        }
        else
        {
            return httpProtocol + serverName + (port.Length > 0 ? ":" + port : string.Empty);
        }
    }

    // <summary>
    /// �������������ַ� 
    /// </summary>
    /// <param name="browser"></param>
    /// <returns>FF(Firfox) SF(Safari) OE(Opera) IE</returns>
    public static string GetBrowser(HttpBrowserCapabilities browser)
    {
        if (browser == null)
        {
            return "IE";
        }
        else if (browser.Browser.IndexOf("IE", StringComparison.CurrentCultureIgnoreCase) != -1)
        {
            return "IE";
        }
        if (browser.Browser.IndexOf("Firefox", StringComparison.CurrentCultureIgnoreCase) != -1)
        {
            return "FF";
        }
        else if (browser.Browser.IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
        {
            return "SF";
        }
        else if (browser.Browser.IndexOf("Opera", StringComparison.CurrentCultureIgnoreCase) != -1)
        {
            return "OE";
        }
        else if (browser.Browser.IndexOf("Chrome", StringComparison.CurrentCultureIgnoreCase) != -1)
        {
            return "CH";
        }
        else
        {
            return "IE";
        }
    }

    /// <summary>
    /// ���� Agent �ж��Ƿ��������ֻ�
    /// </summary>
    /// <returns></returns>
    public static bool IsMobileDeviceCheckAgent()
    {
        //bool flag = false;
        string agent = HttpContext.Current.Request.UserAgent;
        string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };

        //�ų�Window ����ϵͳ �� ƻ������ϵͳ
        if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //�ж��Ƿ���IOS�豸
    public static bool isIOSDevice()
    {
        bool isIPhone = HttpContext.Current.Request.UserAgent.Contains("iPhone");
        bool isIPad = HttpContext.Current.Request.UserAgent.Contains("iPad");
        bool isIPod = HttpContext.Current.Request.UserAgent.Contains("iPod");

        if (isIPhone | isIPad | isIPod)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// ���� Agent �ж��Ƿ���IOS�豸
    ///
    ///
    public static bool CheckAgentIsIOSDevice()
    {
        bool flag = false;
        string agent = HttpContext.Current.Request.UserAgent;
        string[] keywords = { "iPhone", "iPod", "iPad" };
        //�ų�Window ����ϵͳ �� ƻ������ϵͳ
        if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
        {
            foreach (string item in keywords)
            {
                if (agent.Contains(item))
                {
                    flag = true;
                    break;
                }
            }
        }
        return flag;
    }

    //ȡ��IP���ڵ�ַ��
    public static string GetUserLocation(string userIP)
    {
        try
        {
            WebClient webGetting = new WebClient();
            //string userIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            string ipQueryResult = webGetting.DownloadString("http://www.ip.cn/getip.php?action=queryip&ip_url=" + userIP);
            string startString = @"���ԣ�";  
            int startIndex = ipQueryResult.LastIndexOf(startString) + startString.Length;
            int endIndex = ipQueryResult.LastIndexOf(@" ", startIndex);
            return ipQueryResult.Substring(startIndex, ipQueryResult.Length - startIndex);
        }
        catch
        {
            return "";
        }
    }

    public static string GetUserLocation()
    {
        try
        {
            WebClient webGetting = new WebClient();
            string userIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            string ipQueryResult = webGetting.DownloadString("http://www.ip.cn/getip.php?action=queryip&ip_url=" + userIP);
            string startString = @"���ԣ�";  
            int startIndex = ipQueryResult.LastIndexOf(startString) + startString.Length;
            int endIndex = ipQueryResult.LastIndexOf(@" ", startIndex);
            return ipQueryResult.Substring(startIndex, ipQueryResult.Length - startIndex);
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string GetIPinArea(string strIP)//strIPΪIP
    {
        string stringIpAddress = "";

        //string sURL = "http://www.youdao.com/smartresult-xml/search.s?type=ip&q=" + strIP + "";

        //try
        //{
        //    using (XmlReader read = XmlReader.Create(sURL))//��ȡ���ص�xml��ʽ�ļ�����
        //    {
        //        while (read.Read())
        //        {
        //            switch (read.NodeType)
        //            {
        //                case XmlNodeType.Text://ȡxml��ʽ�ļ����е��ı�����
        //                    if (string.Format("{0}", read.Value).ToString().Trim() != strIP)//youdao���ص�xml��ʽ�ļ�����һ����IP����һ����IP��ַ
        //                    {
        //                        stringIpAddress = string.Format("{0}", read.Value).ToString().Trim();//��ֵ
        //                    }
        //                    break;
        //                //other
        //            }
        //        }
        //    }

        try
        {
            stringIpAddress = IpSearch.GetAddressWithIP(strIP);

            if (stringIpAddress == "")
            {
                return "����:" + strIP;  
            }
            else
            {
                return stringIpAddress.Trim();
            }
        }
        catch
        {
            return stringIpAddress.Trim();
        }
    }

    public static string GetMacAddress()
    {
        string clientIp = HttpContext.Current.Request.UserHostAddress;

        string mac = "";
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "nbtstat";
        process.StartInfo.Arguments = "-a " + clientIp;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        int length = output.IndexOf("MAC Address =");
        if (length > 0)
        {
            mac = output.Substring(length + 14, 17);
        }
        return mac;
    }

    /// <summary>
    /// ��γ������
    /// </summary>
    public class Degree
    {
        public Degree(double x, double y)
        {
            X = x;
            Y = y;
        }

        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }
    }

    public class CoordDispose
    {
        private const double EARTH_RADIUS = 6378137.0;//����뾶(��)

        /// <summary>
        /// �Ƕ���ת��Ϊ���ȹ�ʽ
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double radians(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// ����ת��Ϊ�Ƕ�����ʽ
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double degrees(double d)
        {
            return d * (180 / Math.PI);
        }

        /// <summary>
        /// ����������γ��֮���ֱ�Ӿ���
        /// </summary>
        public static double GetDistance(Degree Degree1, Degree Degree2)
        {
            double radLat1 = radians(Degree1.X);
            double radLat2 = radians(Degree2.X);
            double a = radLat1 - radLat2;
            double b = radians(Degree1.Y) - radians(Degree2.Y);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// ����������γ��֮���ֱ�Ӿ���(google �㷨)
        /// </summary>
        public static double GetDistanceGoogle(Degree Degree1, Degree Degree2)
        {
            double radLat1 = radians(Degree1.X);
            double radLng1 = radians(Degree1.Y);
            double radLat2 = radians(Degree2.X);
            double radLng2 = radians(Degree2.Y);
            double s = Math.Acos(Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Cos(radLng1 - radLng2) + Math.Sin(radLat1) * Math.Sin(radLat2));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// ��һ����γ��Ϊ���ļ�����ĸ�����
        /// </summary>
        /// <param name="distance">�뾶(��)</param>
        /// <returns></returns>
        public static Degree[] GetDegreeCoordinates(Degree Degree1, double distance)
        {
            double dlng = 2 * Math.Asin(Math.Sin(distance / (2 * EARTH_RADIUS)) / Math.Cos(Degree1.X));
            dlng = degrees(dlng);//һ��ת���ɽǶ���  ԭPHP��������ط�˵�Ĳ������������ȷ ����lz�ֲ��˺ܶ��������ڸ㶨��
            double dlat = distance / EARTH_RADIUS;
            dlat = degrees(dlat);//һ��ת���ɽǶ���
            return new Degree[] { new Degree(Math.Round(Degree1.X + dlat,6), Math.Round(Degree1.Y - dlng,6)),//left-top
                                  new Degree(Math.Round(Degree1.X - dlat,6), Math.Round(Degree1.Y - dlng,6)),//left-bottom
                                  new Degree(Math.Round(Degree1.X + dlat,6), Math.Round(Degree1.Y + dlng,6)),//right-top
                                  new Degree(Math.Round(Degree1.X - dlat,6), Math.Round(Degree1.Y + dlng,6)) //right-bottom
            };
        }

        /// <summary>
        /// ���Է���
        /// </summary>
        private static void Main(string[] args)
        {
            double a = CoordDispose.GetDistance(new Degree(116.412007, 39.947545), new Degree(116.412924, 39.947918));//116.416984,39.944959
            double b = CoordDispose.GetDistanceGoogle(new Degree(116.412007, 39.947545), new Degree(116.412924, 39.947918));
            Degree[] dd = CoordDispose.GetDegreeCoordinates(new Degree(116.412007, 39.947545), 102);
            Console.WriteLine(a + " " + b);
            Console.WriteLine(dd[0].X + "," + dd[0].Y);
            Console.WriteLine(dd[3].X + "," + dd[3].Y);
            Console.ReadLine();
        }
    }

    public static string GetUserRuleAddressLongitudeByLeader(string strUserCode, string strLeaderCode)
    {
        string strHQL;
        DataSet ds;

        string strLongitude;

        strHQL = "Select OfficeLongitude From T_UserAttendanceRule Where UserCode = '" + strUserCode + "' and LeaderCode = '" + strLeaderCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserAttendanceRule");

        strLongitude = ds.Tables[0].Rows[0][0].ToString().Trim();

        if (strLongitude == "")
        {
            return "0";
        }
        else
        {
            return strLongitude;
        }
    }

    public static string GetUserRuleAddressLatitudeByLeader(string strUserCode, string strLeaderCode)
    {
        string strHQL;
        DataSet ds;

        string strLatitude;

        strHQL = "Select OfficeLatitude From T_UserAttendanceRule Where UserCode = '" + strUserCode + "' and LeaderCode = '" + strLeaderCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserAttendanceRule");

        strLatitude = ds.Tables[0].Rows[0][0].ToString().Trim();

        if (strLatitude == "")
        {
            return "0";
        }
        else
        {
            return strLatitude;
        }
    }

    public static string GetUserDepartmentAddressLongitude(string strUserCode)
    {
        string strHQL;
        DataSet ds;

        string strDepartCode, strLongitude;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select Longitude From T_Department Where DepartCode = " + "'" + strDepartCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

        strLongitude = ds.Tables[0].Rows[0][0].ToString().Trim();

        if (strLongitude == "")
        {
            strHQL = "Select Longitude From T_Department Where IsDefaultAddress = 'YES'";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

            if (ds.Tables[0].Rows.Count > 0)
            {
                strLongitude = ds.Tables[0].Rows[0][0].ToString().Trim();

                if (strLongitude == "")
                {
                    return "0";
                }
                else
                {
                    return strLongitude;
                }
            }
            else
            {
                return "0";
            }
        }
        else
        {
            return strLongitude;
        }
    }

    public static string GetUserDepartmentAddressLatitude(string strUserCode)
    {
        string strHQL;
        DataSet ds;

        string strDepartCode, strLatitude;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select Latitude From T_Department Where DepartCode = " + "'" + strDepartCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

        strLatitude = ds.Tables[0].Rows[0][0].ToString().Trim();

        if (strLatitude == "")
        {
            strHQL = "Select Latitude From T_Department Where IsDefaultAddress = 'YES'";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");

            if (ds.Tables[0].Rows.Count > 0)
            {
                strLatitude = ds.Tables[0].Rows[0][0].ToString().Trim();

                if (strLatitude == "")
                {
                    return "0";
                }
                else
                {
                    return strLatitude;
                }
            }
            else
            {
                return "0";
            }
        }
        else
        {
            return strLatitude;
        }
    }

    #endregion IP��MAC��ַ���ƶ��豸����

    #region ���ݱ���Զ�����Ĺ���

    //���Ϲ���RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(�ܽ�)

    /// <summary>
    ///  ȫ��Ϊ����
    /// </summary>
    public static bool CheckIsAllNumber(string strValue)
    {
        bool IsBool = false;
        Regex rex = new Regex(@"^[0-9]+$");
        Match ma = rex.Match(strValue);
        if (ma.Success)
        {
            IsBool = true;
            //��Ϊ����
        }
        return IsBool;
    }

    //��DataSetת��Ϊxml�����ַ���
    public static string ConvertDataSetToXML(DataSet xmlDS)
    {
        MemoryStream stream = null;
        XmlTextWriter writer = null;

        try
        {
            stream = new MemoryStream();
            //��streamװ�ص�XmlTextReader
            writer = new XmlTextWriter(stream, Encoding.Unicode);

            //��WriteXml����д���ļ�.
            xmlDS.WriteXml(writer);
            int count = (int)stream.Length;
            byte[] arr = new byte[count];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(arr, 0, count);

            UnicodeEncoding utf = new UnicodeEncoding();
            return utf.GetString(arr).Trim();
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }

    //��xml���������ַ���ת��ΪDataSet
    public static DataSet ConvertXMLToDataSet(string xmlData)
    {
        StringReader stream = null;
        XmlTextReader reader = null;
        try
        {
            DataSet xmlDS = new DataSet();
            stream = new StringReader(xmlData);
            //��streamװ�ص�XmlTextReader
            reader = new XmlTextReader(stream);
            xmlDS.ReadXml(reader);
            return xmlDS;
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }

    /// <summary>
    /// дXML���ļ�����
    /// </summary>
    /// <param name="xml">xml�ַ���</param>
    /// <param name="filePath">·��</param>
    /// <returns></returns>
    private static void InsertXML(string xml, string filePath)
    {
        try
        {
            string strXML = xml;

            FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
            StreamWriter sw = new System.IO.StreamWriter(fs);
            sw.WriteLine(strXML);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    /// <summary>
    ///  ��ȡXML�ļ�
    /// </summary>
    /// <param name="filePath">·��</param>
    /// <returns></returns>
    public static string ReadXML(string filePath)
    {
        XmlDocument xx = new XmlDocument();
        xx.Load(filePath);
        return xx.OuterXml;
    }

    /// <summary>
    ///  ������ݿⱸ��ʱ���Ƿ�ʱ
    /// </summary>
    public static string CheckBackDBOverTime()
    {
        string strResult = string.Empty;
        string strBackDBHQL = "select (now()::date - ((select BackTime from T_BackDBLog order by BackTime desc limit 1)+BackPeriodDay * interval '1 day')::date) as DayPeriod from T_BackDBPrame";
        DataTable dtBackDB = ShareClass.GetDataSetFromSql(strBackDBHQL, "strBackDBHQL").Tables[0];
        if (dtBackDB != null && dtBackDB.Rows.Count > 0)
        {
            int intDay = 0;
            int.TryParse(dtBackDB.Rows[0]["DayPeriod"] == DBNull.Value ? "0" : dtBackDB.Rows[0]["DayPeriod"].ToString(), out intDay);
            if (intDay > 0)
            {
                strResult = "���ݼ��ʱ���ѵ�����Ҫ���±��ݣ�";  
            }
            else
            {
                strResult = "���ݼ��ʱ��δ����";  
            }
        }
        return strResult;
    }

    /// <summary>
    ///  ��֤�Ƿ�Ϊ����
    /// </summary>
    public static bool CheckIsNumber(string strValue)
    {
        bool IsBool = false;
        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");

        if (reg1.IsMatch(strValue))
        {
            IsBool = true;
        }
        return IsBool;
    }

    /// <summary>
    /// object��ת��Ϊstring��
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ObjectToString(object value)
    {
        return ObjectToString(value, String.Empty);
    }

    /// <summary>
    /// object��ת��Ϊstring��
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string ObjectToString(object value, string defaultValue)
    {
        return (null == value || value == DBNull.Value) ? defaultValue : value.ToString();
    }

    //�жϷǷ��ַ�
    public static bool CheckStringRight(string str_char)
    {
        if (str_char.IndexOf("'") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("!") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("delete") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("and") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("update") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("insert") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf(";") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("master") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("mid") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("user") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("db_name") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("backup") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("select") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("char") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("nchar") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("xp_cmdshell") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf(",") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("--") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("exec") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("from") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("update") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("count") >= 0)
        {
            return false;
        }
        else if (str_char.IndexOf("\"") >= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //����ϵͳͨ�ù���
    public static void UpdateXLCodeStatus(string strXLCode)
    {
        string strUpdateXLCodeHQL = "update T_WZMaterialXL set IsMark = -1 where XLCode = '" + strXLCode + "'";
        ShareClass.RunSqlCommand(strUpdateXLCodeHQL);
    }

    //�ѷ���������������Ϊ��
    public static string SetDateStringToEmpty(string strDateString)
    {
        if (strDateString.IndexOf("0001") >= 0)
        {
            return "";
        }
        else
        {
            return strDateString;
        }
    }

    //��ȡ��Ӧ������
    public static string StringCutByRequire(string strString, int intShowCount)
    {
        string strResult = string.Empty;
        if (string.IsNullOrEmpty(strString))
        {
            strResult = "";
        }
        else
        {
            if (strString.Length <= intShowCount)
            {
                strResult = strString;
            }
            else
            {
                strResult = strString.Substring(0, intShowCount) + "...";
            }
        }
        return strResult;
    }

    public static string StringToDateTime(string strString, string strFormat)
    {
        string strResult = string.Empty;
        if (string.IsNullOrEmpty(strString))
        {
            strResult = "";
        }
        else
        {
            try
            {
                strResult = DateTime.Parse(strString).ToString(strFormat);
            }
            catch
            {
                return strString;
            }
        }

        return strResult;
    }

    //ȡ�õ��¿�ʼ����
    public static DateTime getCurrentMonthStartDay()
    {
        DateTime dt = DateTime.Now;
        //���µ�һ��ʱ��      
        DateTime dt_First = dt.AddDays(1 - (dt.Day));
        //���ĳ��ĳ�µ�����    
        int year = dt.Date.Year;
        int month = dt.Date.Month;
        int dayCount = DateTime.DaysInMonth(year, month);
        //�������һ��ʱ��    
        DateTime dt_Last = dt_First.AddDays(dayCount - 1);

        return dt_First;
    }

    //ȡ�õ��½�������
    public static DateTime getCurrentMonthEndDay()
    {
        DateTime dt = DateTime.Now;
        //���µ�һ��ʱ��      
        DateTime dt_First = dt.AddDays(1 - (dt.Day));
        //���ĳ��ĳ�µ�����    
        int year = dt.Date.Year;
        int month = dt.Date.Month;
        int dayCount = DateTime.DaysInMonth(year, month);
        //�������һ��ʱ��    
        DateTime dt_Last = dt_First.AddDays(dayCount - 1);

        return dt_Last;
    }

    //������º��·ݷ����û�ѡ��
    public static void InitYearMonthList(DropDownList DDL_YearList, DropDownList DDL_MonthList)
    {
        //���
        DateTime dt = DateTime.Now;
        for (int i = dt.Year - 15; i < dt.Year + 85; i++)
        {
            DDL_YearList.Items.Add(new ListItem(i.ToString()));
        }
        DDL_YearList.SelectedValue = dt.Year.ToString();
        //�·�
        for (int i = 1; i <= 12; i++)
        {
            DDL_MonthList.Items.Add(new ListItem(i.ToString()));
        }
        DDL_MonthList.SelectedValue = dt.Month.ToString();
    }

    //��ȡ����ַ�����201601��ʾ2016��1�£�
    public static string GetYearMonthString(DropDownList DDL_YearList, DropDownList DDL_MonthList)
    {
        int month = Convert.ToInt32(DDL_MonthList.Text);
        string str = string.Format("{0:D2}", month);

        return DDL_YearList.Text + str;
    }

    //�ж�������ַ����Ƿ�Ϸ�
    public static bool IsValidYearMonth(string ymstr)
    {
        if (ymstr.Trim().Length != 6)
            return false;

        if (false == ShareClass.CheckIsAllNumber(ymstr))
            return false;

        int year = Convert.ToInt32(ymstr.Substring(0, 4));
        int month = Convert.ToInt32(ymstr.Substring(4, 2));

        if (year < 1900 || year > 9999)
            return false;

        if (month < 1 || month > 12)
            return false;

        return true;
    }

    //// <summary>

    /// ����Ҵ�Сд���ת��
    /// </summary>
    public static class RMBCapitalization
    {
        private const string DXSZ = "��Ҽ��������½��ƾ�";  
        private const string DXDW = "����ֽ�Ԫʰ��Ǫ�fʰ��Ǫ��ʰ��Ǫ�f��ʰ��Ǫ�f�ھ�ʰ��Ǫ�f������";  
        private const string SCDW = "Ԫʰ��Ǫ�f�ھ�����";  

        /// <summary>
        /// ת������Ϊ��д���
        /// ��߾���Ϊ�򣬱���С�����4λ��ʵ�ʾ���Ϊ�����Ѿ��㹻�ˣ������Ͼ��������ƣ�������ʾ��
        /// ���:...30.29.28.27.26.25.24  23.22.21.20.19.18  17.16.15.14.13  12.11.10.9   8 7.6.5.4  . 3.2.1.0
        /// ��λ:...�������fǪ��ʰ        �����fǪ��ʰ       ���fǪ��ʰ      ��Ǫ��ʰ     �fǪ��ʰԪ . �Ƿ����
        /// ��ֵ:...1000000               000000             00000           0000         00000      . 0000
        /// �����г����������������ʵ�λ��
        /// Ԫ��ʮ���١�ǧ�����ڡ��ס�����������𦡢�������������ء���
        /// </summary>
        /// <param name="capValue">����ֵ</param>
        /// <returns>���ش�д���</returns>
        public static string ConvertIntToUppercaseAmount(string capValue)
        {
            string currCap = "";    //��ǰ���
            string capResult = "";  //������
            string currentUnit = "";//��ǰ��λ
            string resultUnit = ""; //�����λ
            int prevChar = -1;      //��һλ��ֵ
            int currChar = 0;       //��ǰλ��ֵ
            int posIndex = 4;       //λ����������"Ԫ"��ʼ

            if (Convert.ToDouble(capValue) == 0) return "";
            for (int i = capValue.Length - 1; i >= 0; i--)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (posIndex > 30)
                {
                    //�ѳ�����󾫶�"��"��ע�����Խ�30�ĳ�22��ʹ֮��ȷ�����ھ��㹻��
                    break;
                }
                else if (currChar != 0)
                {
                    //��ǰλΪ����ֵ����ֱ��ת���ɴ�д���
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    //��ֹת������ֶ������,���磺3000020
                    switch (posIndex)
                    {
                        case 4: currCap = "Ԫ"; break;  
                        case 8: currCap = "�f"; break;  
                        case 12: currCap = "��"; break;  
                        case 17: currCap = "��"; break;  
                        case 23: currCap = "��"; break;  
                        case 30: currCap = "��"; break;  
                        default: break;
                    }
                    if (prevChar != 0)
                    {
                        if (currCap != "")
                        {
                            if (currCap != "Ԫ") currCap += "��";  
                        }
                        else
                        {
                            currCap = "��";  
                        }
                    }
                }
                //�Խ�������ݴ���
                if (capResult.Length > 0)
                {
                    resultUnit = capResult.Substring(0, 1);
                    currentUnit = DXDW.Substring(posIndex, 1);
                    if (SCDW.IndexOf(resultUnit) > 0)
                    {
                        if (SCDW.IndexOf(currentUnit) > SCDW.IndexOf(resultUnit))
                        {
                            capResult = capResult.Substring(1);
                        }
                    }
                }
                capResult = currCap + capResult;
                prevChar = currChar;
                posIndex += 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// ת��С��Ϊ��д���
        /// </summary>
        /// <param name="capValue">С��ֵ</param>
        /// <param name="addZero">�Ƿ�������λ</param>
        /// <returns>���ش�д���</returns>
        public static string ConvertDecToUppercaseAmount(string capValue, bool addZero)
        {
            string currCap = "";
            string capResult = "";
            int prevChar = addZero ? -1 : 0;
            int currChar = 0;
            int posIndex = 3;

            if (Convert.ToInt16(capValue) == 0) return "";
            for (int i = 0; i < capValue.Length; i++)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (currChar != 0)
                {
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                }
                else
                {
                    if (Convert.ToInt16(capValue.Substring(i)) == 0)
                    {
                        break;
                    }
                    else if (prevChar != 0)
                    {
                        currCap = "��";  
                    }
                }
                capResult += currCap;
                prevChar = currChar;
                posIndex -= 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// ����Ҵ�д���
        /// </summary>
        /// <param name="value">��������ֽ��ֵ</param>
        /// <returns>��������Ҵ�д���</returns>
        public static string RMBAmount(double value)
        {
            string capResult = "";
            string capValue = string.Format("{0:f4}", value);       //��ʽ��
            int dotPos = capValue.IndexOf(".");                     //С����λ��
            bool addInt = (Convert.ToInt32(capValue.Substring(dotPos + 1)) == 0);//�Ƿ��ڽ���м�"��"
            bool addMinus = (capValue.Substring(0, 1) == "-");      //�Ƿ��ڽ���м�"��"
            int beginPos = addMinus ? 1 : 0;                        //��ʼλ��
            string capInt = capValue.Substring(beginPos, dotPos);   //����
            string capDec = capValue.Substring(dotPos + 1);         //С��

            if (dotPos > 0)
            {
                capResult = ConvertIntToUppercaseAmount(capInt) +
                    ConvertDecToUppercaseAmount(capDec, Convert.ToDouble(capInt) != 0 ? true : false);
            }
            else
            {
                capResult = ConvertIntToUppercaseAmount(capDec);
            }
            if (addMinus) capResult = "��" + capResult;  
            if (addInt) capResult += "��";  
            return capResult;
        }
    }

    #endregion ���ݱ���Զ�����Ĺ���
}
