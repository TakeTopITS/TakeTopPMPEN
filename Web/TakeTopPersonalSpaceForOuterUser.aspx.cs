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

public partial class TakeTopPersonalSpaceForOuterUser : System.Web.UI.Page
{
    int intRunNumber;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        if (Page.IsPostBack == false)
        {
            LB_UserName.Text = ShareClass.GetUserName(strUserCode);

            if (Session["SystemVersionType"].ToString() == "SAAS")
            {
                Response.Redirect("TakeTopPersonalSpaceSAAS.aspx?UserCode=" + strUserCode + "&Flag=" + Session["SkinFlag"].ToString());
            }

            if (Request.QueryString["UserCode"] == null)
            {
                Response.Redirect("TakeTopPersonalSpaceForOuterUser.aspx?UserCode=" + strUserCode + "&Flag=" + Session["SkinFlag"].ToString());
            }

            //���ҳ�滺�棬���ڸı�Ƥ��
            SetPageNoCache();

            intRunNumber = 0;

            RegisterAsyncTask(new PageAsyncTask(AsyncWork));
        }
    }


    protected void BT_Extend_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strLeftBarExtend;


        strUserCode = Session["UserCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        if (Session["LeftBarExtend"].ToString() == "YES")
        {
            strLeftBarExtend = "NO";
        }
        else
        {
            strLeftBarExtend = "YES";
        }

        try
        {
            //���������չ��״̬
            ShareClass.UpdateLeftBarExtendStatus(strUserCode, strLeftBarExtend);

            Session["LeftBarExtend"] = strLeftBarExtend;

            ShareClass.AddSpaceLineToFile("TakeTopLRExLeft.aspx", "");
            ShareClass.AddSpaceLineToFile("TakeTopCSLRLeft.aspx", "");

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click55", "changeLeftBarExtend('" + strLeftBarExtend + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click66", "alert('" + LanguageHandle.GetWord("ZZGGSBJC") + "')", true);
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
            BindNewsAndNoticeTypeData();

            Timer1.Interval = 36000000;

            intRunNumber = 1;
        }
    }

    private async System.Threading.Tasks.Task AsyncWork()
    {
        await System.Threading.Tasks.Task.Delay(8000);

        string strUserCode;
        string strUserType;
        String strLangCode;

        strUserCode = Session["UserCode"].ToString();
        strUserType = Session["UserType"].ToString();
        strLangCode = Session["LangCode"].ToString();

        string strHQL;
        DataSet ds;

        BindNewsAndNoticeTypeData();


        strHQL = String.Format(@"Select distinct B.HomeModuleName,(rtrim(B.PageName)||'?UserCode={0}') as ModulePage,A.SortNumber  From T_ProModuleLevelForPageUser A,T_ProModuleLevelForPage B 
                Where A.ModuleName = B.ModuleName and A.UserType= '{1}' and B.Visible = 'YES' and B.IsDeleted = 'NO' and A.UserCode = '{0}' and B.ParentModule = 'ExternalPersonalSpace'
                and B.PageName <> 'TTPersonalSpaceNews.aspx' and B.LangCode = '{2}' and A.Visible ='YES' Order By A.SortNumber ASC", strUserCode, strUserType, strLangCode);
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = String.Format(@"Select distinct B.HomeModuleName, (rtrim(B.PageName)||'?UserCode={0}') as ModulePage,A.SortNumber  From T_ProModuleLevelForPageUser A,T_ProModuleLevelForPage B  
              Where A.ModuleName = B.ModuleName and A.UserType= '{1}' and B.Visible = 'YES' and B.IsDeleted = 'NO' and B.ParentModule = 'ExternalPersonalSpace'  and B.LangCode = 'zh-CN' 
            and A.Visible ='YES' Order By A.SortNumber ASC", strUserCode, strUserType, strLangCode);
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");
        }
        Repeater1.DataSource = ds;
        Repeater1.DataBind();
    }

    protected void BindNewsAndNoticeTypeData()
    {
        string strHtml = string.Empty;
        string strHQL;
        DataSet ds;

        string strLangCode = Session["LangCode"].ToString();

        strHQL = "Select Distinct * From T_NewsType Where LangCode = " + "'" + strLangCode + "' and Visible = 'YES' and NewsScope in ('ALL','OUTER') Order By SortNumber ASC";
        ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "T_NewsType");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Select Distinct * From T_NewsType Where LangCode = 'zh-CN' and Visible = 'YES' and NewsScope in ('ALL','OUTER') Order By SortNumber ASC";
            ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "T_NewsType");
        }

        RP_NewsTypeList.DataSource = ds;
        RP_NewsTypeList.DataBind();
    }
}
