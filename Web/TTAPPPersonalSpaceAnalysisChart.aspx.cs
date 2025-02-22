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
using Npgsql;//using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAPPPersonalSpaceAnalysisChart : System.Web.UI.Page
{
    int intRunNumber;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickParentA", "aHandlerForSpecialPopWindow();", true);
        if (Page.IsPostBack == false)
        {
            ////清空页面缓存，用于改变皮肤
            //SetPageNoCache();

            intRunNumber = 0;

            AsyncWork();
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

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //if (intRunNumber == 0)
        //{
        //    AsyncWork();

        //    intRunNumber = 1;

        //    Timer1.Interval = 3600000;
        //}
    }

    private void AsyncWork()
    {
        string strUserCode;
        String strLangCode;

        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();

        new System.Threading.Thread(delegate ()
        {
            //增加分析图给用户
            AddChartToUser(strUserCode);

        }).Start();

        LoadSytemChart(strUserCode, "PersonalSpacePage", RP_ChartList);
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
            strHQL = "Select trim(A.FormType) as FormType,trim(A.ChartName) as ChartName,(Select trim(SqlCode) From T_SystemAnalystChartManagement Where ChartName = A.ChartName and charttype Not In ('HRuningProjectStatus','HDelayProjectStatus','HAnnualPaymentStatus','HAnnualWorkHourStatus','HRuningTaskStatus') ) as SqlCode,(Select trim(ChartType) From T_SystemAnalystChartManagement Where ChartName = A.ChartName ) as ChartType  From T_SystemAnalystChartRelatedUser A ";
            strHQL += " Where A.UserCode = '" + strUserCode + "' and A.FormType = '" + strFormType + "'";
            strHQL += " and chartName Not In (Select ChartName From T_SystemAnalystChartManagement where ChartType In  ('HRuningProjectStatus','HDelayProjectStatus','HAnnualPaymentStatus','HAnnualWorkHourStatus','HRuningTaskStatus'))";
            strHQL += " Order By A.SortNumber ASC";

            LogClass.WriteLogFile(strHQL);
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



    //增加分析图给用户
    private void AddChartToUser(string strUserCode)
    {
        string strHQL;

        if (GetUserChartNumber(strUserCode) == 0)
        {
            strHQL = string.Format(@"Insert Into t_systemanalystchartrelateduser(UserCode,ChartName,FormType,SortNumber) 
              Select '{0}',ChartName,FormType,SortNumber From t_systemanalystchartrelateduser 
                Where UserCode = 'ADMIN'", strUserCode);

            ShareClass.RunSqlCommand(strHQL);
        }
    }

    //取得用户分析图数量
    private int GetUserChartNumber(string strUserCode)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From t_systemanalystchartrelateduser
    Where UserCode = '{0}'", strUserCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "t_systemanalystchartrelateduser");

        return ds.Tables[0].Rows.Count;

    }
}