using System;
using System.Collections;
using System.Data;
using System.Web.UI;

public partial class TakeTopLRExLeft : System.Web.UI.Page
{
    int intRunNumber;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack != true)
        {
            //清空页面缓存，用于改变皮肤
            SetPageNoCache();

            intRunNumber = 0;

            AsyncWork();
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (intRunNumber == 0)
        {
            AsyncWork();

            Timer1.Interval = 3600000;

            intRunNumber = 1;
        }
    }

    //清空页面缓存，用于改变皮肤
    public void SetPageNoCache()
    {
        if (Session["CssDirectoryChangeNumber"].ToString() == "1")
        {
            //清除全部缓存
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

    private void AsyncWork()
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶拓鼎（2006－2026）

        string strUserCode;
        string strLangCode;
        string strForbitModule;

        string strHQL;


        strLangCode = Session["LangCode"].ToString();
        strForbitModule = Session["ForbitModule"].ToString().Trim();

        string strUserType, strUserName, strSampleUserCode;
        string strModuleName;

        strUserCode = Session["UserCode"].ToString();
        strModuleName = Request.QueryString["ModuleName"];

        strUserType = Session["UserType"].ToString();
        if (strUserType == "OUTER")
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            ID_SMSSend.Visible = false;
            ID_MakeIM.Visible = false;
        }

        strUserName = Session["UserName"].ToString();
        if (strUserCode == "ADMIN")
        {
            if (ShareClass.IsExistModuleByUserCode("ADMIN", LanguageHandle.GetWord("GuanKongZhongXin").ToString().Trim(), "SYSTEM", strUserType) == false)
            {
                //strHQL = "Insert Into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType) Values('ControlCenter','ADMIN','YES','SYSTEM','" + strUserType + "')";   
                //ShareClass.RunSqlCommand(strHQL);
            }
            else
            {
                strHQL = "Update T_ProModule Set Visible = 'YES' Where ModuleName = 'ControlCenter' and UserCode = 'ADMIN'";   
                ShareClass.RunSqlCommand(strHQL);
            }

            if (ShareClass.IsExistModuleByUserCode("ADMIN", LanguageHandle.GetWord("JiTongMoZuSheDing").ToString().Trim(), "SYSTEM", strUserType) == false)
            {
                //strHQL = "Insert Into T_ProModule(ModuleName,UserCode,Visible,ModuleType,UserType) Values('SystemModuleSettings','ADMIN','YES','SYSTEM','" + strUserType + "')";   
                //ShareClass.RunSqlCommand(strHQL);
            }
            else
            {
                strHQL = "Update T_ProModule Set Visible = 'YES' Where ModuleName = 'SystemModuleSettings' and UserCode = 'ADMIN'";   
                ShareClass.RunSqlCommand(strHQL);
            }

            strHQL = "Update T_ProModuleLevel Set Visible = 'YES' Where ModuleName = 'ControlCenter'";   
            ShareClass.RunSqlCommand(strHQL);
            strHQL = "Update T_ProModuleLevel Set Visible = 'YES' Where ModuleName = 'SystemModuleSettings'";   
            ShareClass.RunSqlCommand(strHQL);
        }

        //初始化用户模组
        strSampleUserCode = "SAMPLE";
        ShareClass.InitialUserModules(strSampleUserCode, strUserCode);

        ////Key Function
        //TakeTopLicense.BindSystemKeyModule();

        BindModuleData();

    }

    protected void BindModuleData()
    {
        string strHtml = string.Empty;
        string strHQL;

        string strUserCode;
        string strLangCode;
        string strForbitModule;
        string strUserType;
        string strModuleID, strModuleName, strHomeModuleName, strPageName, strIconURL, strModuleType, strDIYFlow, strPageNameForDoubleClick;

        if (Session["LangCode"] == null)
        {
            Session["LangCode"] = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
        }
        strLangCode = Session["LangCode"].ToString();
        strForbitModule = Session["ForbitModule"].ToString().Trim();

        strUserCode = Session["UserCode"].ToString();
        strModuleName = Request.QueryString["ModuleName"];

        strUserType = Session["UserType"].ToString();

        strHQL = string.Format(@"Select Distinct B.ID,A.ModuleName,A.HomeModuleName,A.ParentModule,A.PageName,A.ModuleType,
                A.UserType,A.IconURL,A.SortNumber,B.DIYFlow From T_ProModuleLevel A, T_ProModule B Where rtrim(A.ModuleName)
                ||rtrim(A.ModuleType)||rtrim(A.UserType) = rtrim(B.ModuleName) ||rtrim(B.ModuleType) 
                ||rtrim(B.UserType)  and A.Visible = 'YES' and A.IsDeleted = 'NO' 
                and A.ModuleType Not In ('APP','DIYAPP','SITE') and A.UserType = 'INNER' and B.UserType = 'INNER' 
                and B.UserCode = '{0}' and B.Visible = 'YES' and B.ModuleType Not In ('APP','DIYAPP','SITE') and position(rtrim(A.ModuleName)||',' in '{1}') = 0
                and A.LangCode = '{2}' Order By A.SortNumber ASC", strUserCode, strForbitModule, strLangCode);
        DataTable dtModule = ShareClass.GetDataSetFromSql(strHQL, "Module").Tables[0];
        DataView dvModule = new DataView(dtModule);
        dvModule.RowFilter = "ParentModule = '' ";

        bool isFirst = false;

        foreach (DataRowView row in dvModule)
        {
            strHtml += "<div class=\"box\">";

            strModuleID = ShareClass.ObjectToString(row["ID"]).Trim();
            strModuleName = ShareClass.ObjectToString(row["ModuleName"]).Trim();
            strHomeModuleName = ShareClass.ObjectToString(row["HomeModuleName"]).Trim();
            strPageName = ShareClass.ObjectToString(row["PageName"]).Trim();
            strIconURL = ShareClass.ObjectToString(row["IconURL"]).Trim();
            strModuleType = ShareClass.ObjectToString(row["ModuleType"]).Trim();
            strDIYFlow = ShareClass.ObjectToString(row["DIYFlow"]).Trim();

            if (strIconURL.Trim() == "")
            {
                strIconURL = @"imagesSkin/ModuleIcon.png";
            }

            if (strPageName.IndexOf("?") >= 0)
            {
                strPageName = strPageName + "&ModuleName=" + strModuleName + "&ModuleType=" + strModuleType;
            }
            else
            {
                strPageName = strPageName + "?ModuleName=" + strModuleName + "&ModuleType=" + strModuleType;
            }

            if (strDIYFlow == "YES")
            {
                strPageName = ShareClass.ObjectToString("TTModuleFlowChartViewJS.aspx?Type=UserModule&IdentifyString=" + strModuleID);
            }
            strPageNameForDoubleClick = ShareClass.ObjectToString("TTModuleFlowDesignerJS.aspx?Type=UserModule&IdentifyString=" + strModuleID);


            strHtml += "<span name=\"parent1\"  onmouseover=\"OnMouseOverEvent(this)\" onmouseout=\"OnMouseOutEvent(this)\"><span onclick=\"OnPlusEvent(this)\" class=\"plusSpan\"><img src=\'" + strIconURL + "' />&nbsp;<span onclick=\"OnMinusEvent(this)\" class=\"minusSpan\"><span class=\"titleSpan\" onclick=\"CreateTabModule('" + strHomeModuleName + "','" + strPageName + "',this)\" ondblclick=\"CreateTabModule('" + strHomeModuleName + "','" + strPageNameForDoubleClick + "',this)\">" + strHomeModuleName + "</span></span></span></span> ";
            if (!isFirst)
            {
                HF_ClickValue.Value = strHomeModuleName;
                isFirst = true;
            }

            //增加儿子模组
            strHtml += "<div class=\"text\">";
            dvModule.RowFilter = "ParentModule='" + strModuleName + "'";

            strHtml += "<ul class=\"content\">";
            foreach (DataRowView rowChild in dvModule)
            {
                string strChildModuleID = ShareClass.ObjectToString(rowChild["ID"]).Trim();
                string strChildModuleName = ShareClass.ObjectToString(rowChild["ModuleName"]).Trim();
                string strChildHomeModuleName = ShareClass.ObjectToString(rowChild["HomeModuleName"]).Trim();
                string strChildPageName = ShareClass.ObjectToString(rowChild["PageName"]).Trim();
                string strChildPageNameForDoubleClick;

                string strChildIconURL = ShareClass.ObjectToString(rowChild["IconURL"]).Trim();
                string strChildModuleType = ShareClass.ObjectToString(rowChild["ModuleType"]).Trim();
                string strChildDIYFlow = ShareClass.ObjectToString(rowChild["DIYFlow"]).Trim();

                if (strChildPageName.IndexOf("?") >= 0)
                {
                    strChildPageName = strChildPageName + "&ModuleName=" + strChildModuleName + "&ModuleType=" + strChildModuleType;
                }
                else
                {
                    strChildPageName = strChildPageName + "?ModuleName=" + strChildModuleName + "&ModuleType=" + strChildModuleType;
                }

                if (strChildDIYFlow == "YES")
                {
                    strChildPageName = ShareClass.ObjectToString("TTModuleFlowChartViewJS.aspx?Type=UserModule&IdentifyString=" + strChildModuleID);
                }
                strChildPageNameForDoubleClick = ShareClass.ObjectToString("TTModuleFlowDesignerJS.aspx?Type=UserModule&IdentifyString=" + strChildModuleID);


                //增加孙子模组
                dvModule.RowFilter = "ParentModule='" + strChildModuleName + "'";

                if (dvModule.Count > 0)
                {
                    strHtml += "<li><span class=\"titleSpan\" onmouseover=\"OnMouseOverEvent(this)\" onmouseout=\"OnMouseOutEvent(this)\"  ondblclick=\"CreateTabModule('" + strHomeModuleName + "','" + strChildPageNameForDoubleClick + "',this)\"><span class=\"titleSpan\" onclick=\"CreateTabModule('" + strChildHomeModuleName + "','" + strChildPageName + "',this)\" >&nbsp;&nbsp;" + strChildHomeModuleName + "</span></span></li>";
                }
                else
                {
                    strHtml += "<li><span class=\"titleSpan\" onmouseover=\"OnMouseOverEvent(this)\" onmouseout=\"OnMouseOutEvent(this)\" ondblclick=\"CreateTabModule('" + strHomeModuleName + "','" + strChildPageNameForDoubleClick + "',this)\" onclick=\"CreateTab('" + strChildHomeModuleName + "','" + strChildPageName + "',this)\">&nbsp;" + strChildHomeModuleName + "</span></li>";
                }

                strHtml += "<div class=\"text\">";
                strHtml += "<ul class=\"content\">";
                foreach (DataRowView rowGrandSon in dvModule)
                {
                    string strGrandSonModuleID = ShareClass.ObjectToString(rowGrandSon["ID"]).Trim();
                    string strGrandSonModuleName = ShareClass.ObjectToString(rowGrandSon["ModuleName"]).Trim();
                    string strGrandSonHomeModuleName = ShareClass.ObjectToString(rowGrandSon["HomeModuleName"]).Trim();
                    string strGrandSonPageName = ShareClass.ObjectToString(rowGrandSon["PageName"]).Trim();
                    string strGrandSonPageNameForDoubleClick;

                    string strGrandSonIconURL = ShareClass.ObjectToString(rowGrandSon["IconURL"]).Trim();
                    string strGrandSonModuleType = ShareClass.ObjectToString(rowGrandSon["ModuleType"]).Trim();
                    string strGrandSonDIYFlow = ShareClass.ObjectToString(rowGrandSon["DIYFlow"]).Trim();

                    if (strGrandSonPageName.IndexOf("?") >= 0)
                    {
                        strGrandSonPageName = strGrandSonPageName + "&ModuleName=" + strGrandSonModuleName + "&ModuleType=" + strGrandSonModuleType;
                    }
                    else
                    {
                        strGrandSonPageName = strGrandSonPageName + "?ModuleName=" + strGrandSonModuleName + "&ModuleType=" + strGrandSonModuleType;
                    }

                    if (strGrandSonDIYFlow == "YES")
                    {
                        strGrandSonPageName = ShareClass.ObjectToString("TTModuleFlowChartViewJS.aspx?Type=UserModule&IdentifyString=" + strGrandSonModuleID);
                    }
                    strGrandSonPageNameForDoubleClick = ShareClass.ObjectToString("TTModuleFlowDesignerJS.aspx?Type=UserModule&IdentifyString=" + strGrandSonModuleID);


                    strHtml += "<li><span class=\"titleSpan\" onmouseover=\"OnMouseOverEvent(this)\" onmouseout=\"OnMouseOutEvent(this)\" onclick=\"CreateTab('" + strGrandSonHomeModuleName + "','" + strGrandSonPageName + "',this)\" ondblclick=\"CreateTabModule('" + strHomeModuleName + "','" + strGrandSonPageNameForDoubleClick + "',this)\">&nbsp;" + strGrandSonHomeModuleName + "</span></li>";
                }

                strHtml += "</ul>";
                strHtml += "</div>";
            }

            strHtml += "</ul>";
            strHtml += "</div>";
            strHtml += "</div>";
        }

        LT_Result.Text = strHtml + "<br /><br />";
    }

    protected void BT_MakeIM_Click(object sender, EventArgs e)
    {
        string strRandomID;
        string strJavaScriptFuntion;
        string strMessage;

        strRandomID = "IMMember";
        strMessage = "TTTakeTopIM.aspx";

        strJavaScriptFuntion = "opim(" + "'" + strRandomID + "'" + "," + "'" + strMessage + "'" + ");";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", strJavaScriptFuntion, true);
    }

}
