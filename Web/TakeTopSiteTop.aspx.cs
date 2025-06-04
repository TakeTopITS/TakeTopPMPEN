using Stimulsoft.Svg;

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TakeTopSiteTop : System.Web.UI.Page
{
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                InitializeCulture();

                LoadLanguageForDropListForWebSite(ddlLangSwitcher);

                // ȷ����������ʾ��ǰ���ԣ����ȴ�Cookie��ȡ��
                strLangCode = Session["LangCode"].ToString();

                ddlLangSwitcher.SelectedValue = strLangCode;

                BindHeadLineData();
                BindModuleData();
            }
            catch (Exception ex)
            {
                LogClass.WriteLogFile(ex.ToString());
                Response.Redirect("TTDisplayErrors.aspx");
            }
        }

    }

    protected override void InitializeCulture()
    {
        // ȷ�����Դ�������ȼ���Cookie > Session > Ĭ������
        strLangCode = Request.Cookies["LangCode"]?.Value ??
                     Session["LangCode"]?.ToString() ??
                     System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];

        // ȷ��Session��Cookieͬ��
        Session["LangCode"] = strLangCode;
        Response.SetCookie(new HttpCookie("LangCode", strLangCode)
        {
            Expires = DateTime.Now.AddYears(1)
        });

        // ���õ�ǰ�̵߳��Ļ�
        System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Globalization.CultureInfo.CreateSpecificCulture(strLangCode);
        System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo(strLangCode);

        Page.Culture = strLangCode;
        Page.UICulture = strLangCode;

        base.InitializeCulture();
    }

    public void LoadLanguageForDropListForWebSite(DropDownList DL_Language)
    {
        string strHQL = "Select trim(LangCode) as LangCode,trim(Language) as Language From T_SystemLanguage Where LangCode in('zh-CN','en') Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "T_SystemLanguage");
        DL_Language.DataSource = ds;
        DL_Language.DataBind();
    }

    protected void ddlLangSwitcher_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedLang = ddlLangSwitcher.SelectedValue.Trim();

        // ����Session��Cookie
        Session["LangCode"] = selectedLang;


        Response.SetCookie(new HttpCookie("LangCode", selectedLang)
        {
            Expires = DateTime.Now.AddYears(1)
        });

        // ���¼���������ܼ�
        string script = @"
            <script type='text/javascript'>
                if(top != self) {  // �����ǰҳ�����ڿ����
                    top.location.href = top.location.href;  // ˢ��������ܼ�
                } else {
                    window.location.href = window.location.href;  // ��ͨҳ��ˢ��
                }
            </script>";

        ClientScript.RegisterStartupScript(this.GetType(), "RefreshParent", script);
    }

    protected void BindModuleData()
    {
        string strHQL = "Select Distinct * From T_ProModuleLevel A Where ParentModule = '' " +
                       "and A.Visible = 'YES' and A.IsDeleted = 'NO' and A.ModuleType = 'SITE' " +
                       "and A.LangCode = '" + strLangCode + "' " +
                       "Order By A.SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "T_ProModuleLevel");
        RP_ModuleList.DataSource = ds;
        RP_ModuleList.DataBind();
    }

    protected void BindHeadLineData()
    {
        string strHtml = string.Empty;
        string strHQL = "Select * from T_HeadLine where LangCode = '" + strLangCode + "' " +
                       "and Type = '�ⲿ' and Status = '����' and IsHead = 'YES' " +
                       "Order by ID DESC";

        DataTable dtHeadLine = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "HeadLine").Tables[0];
        DataView dvHeadLine = new DataView(dtHeadLine);

        strHtml += "<ul class=\"content\">";
        foreach (DataRowView row in dvHeadLine)
        {
            string strID = ShareClass.ObjectToString(row["ID"]);
            string strHeadLineName = ShareClass.ObjectToString(row["Title"]);
            strHtml += "<li><a onmousedown='OnMouseDownEventForWholePage(this)' href='TakeTopSiteNewsView.aspx?ID=" +
                      strID + "' target='SiteRightContainerFrame'>" + strHeadLineName + "</a></li>";
        }
        strHtml += "</ul>";

        LT_Result.Text = strHtml;
    }
}