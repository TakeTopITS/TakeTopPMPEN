using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

using System.Security.Cryptography;
using System.Security.Permissions;
using System.Data.SqlClient;

using TakeTopSecurity;

public partial class TakeTopLRTop : System.Web.UI.Page
{
    int intRunNumber;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = System.Configuration.ConfigurationManager.AppSettings["SystemName"] + " " + ShareClass.SystemVersionID + "---" + System.Configuration.ConfigurationManager.AppSettings["Slogan"];

        if (Page.IsPostBack != true)
        {
            //���ҳ�滺�棬���ڸı�Ƥ��
            SetPageNoCache();

            intRunNumber = 0;

            AsyncWork();
        }
    }

    //���ҳ�滺�棬���ڸı�Ƥ��
    public void SetPageNoCache()
    {
        if (Session["CssDirectoryChangeNumber"].ToString() == "1")
        {
            //���ȫ������
            IDictionaryEnumerator allCaches = Page.Cache.GetEnumerator();
            while (allCaches.MoveNext())
            {
                Page.Cache.Remove(allCaches.Key.ToString());
            }

            Page.Response.Buffer = true;
            Page.Response.AddHeader("Pragma", "No-Cache");

            Page.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Page.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Page.Response.Expires = 0;
            Page.Response.CacheControl = "no-cache";
            Page.Response.Cache.SetNoStore();
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (intRunNumber == 0)
        {
            AsyncWork();

            Timer1.Interval = 18000000;

            intRunNumber = 1;
        }
    }

    private void AsyncWork()
    {
        string strUserCode;
        strUserCode = Session["UserCode"].ToString();

        string licenseKey = System.Configuration.ConfigurationManager.AppSettings["CopyRight"];
        if (licenseKey != "̩��-�ض�-������")
        {
            Response.Write("<script>top.window.location.href = 'TTDisplayErrors.aspx';</script>");
            return;
        }

        try
        {
            //�����û�Ŀ¼
            ShareClass.MakeUserDirectory(strUserCode);
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

        try
        {
            string strCurrentDate, strNoticeDate, strDeadline, strMessage;
            string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];

            TakeTopLicense license = new TakeTopLicense();
            strDeadline = license.GetLicenseDeadline(strServerName);

            strCurrentDate = DateTime.Now.ToString("yyyyMMdd");
            strNoticeDate = DateTime.Now.AddDays(5).ToString("yyyyMMdd");

            string strDeadlineDate = strDeadline.Substring(0, 4) + "-" + strDeadline.Substring(4, 2) + "-" + strDeadline.Substring(6, 2);

            if (string.Compare(strNoticeDate, strDeadline) > 0 & string.Compare(strDeadline, strCurrentDate) >= 0)
            {
                strMessage = "��ע�⣬ϵͳ��Ȩ���� " + strDeadlineDate + " ���ڣ��뾡�����Ѻͱ������ݣ�";
            }
            else
            {
                if (string.Compare(strCurrentDate, strDeadline) > 0)
                {
                    strMessage = "��ע�⣬ϵͳ��Ȩ���� " + strDeadline + " ���ڣ���������ʹ�ã�";
                }
                else
                {
                    strMessage = "";
                }
            }

            if (strMessage != "")
            {
                LB_Message.Text = strMessage;
                IM_Logo.Visible = false;
            }
        }
        catch (Exception err)
        {
            //LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

    }
}
