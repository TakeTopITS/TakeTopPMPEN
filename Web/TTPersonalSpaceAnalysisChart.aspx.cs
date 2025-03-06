using System;
using System.Collections;
using System.Web.UI;


public partial class TTPersonalSpaceAnalysisChart : System.Web.UI.Page
{
    int intRunNumber;
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ////���ҳ�滺�棬���ڸı�Ƥ��
            //SetPageNoCache();

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

            intRunNumber = 1;

            Timer1.Interval = 3600000;
        }
    }

    private void AsyncWork()
    {
        string strUserCode;
        String strLangCode;

        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();


        ShareClass.LoadSytemChart(strUserCode, "PersonalSpacePage", RP_ChartList);
    }
}