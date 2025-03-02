using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using System.Text;
using System.IO;
using System.Web.Mail;

using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using NHibernate.Util;
using TakeTopWF;


public partial class TTModuleFlowChartViewJS : System.Web.UI.Page
{
    private string strIdentifyString;

    int intBegin;
    int intRunNumber;

    string strUserCode, strUserType, strType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strTemName;
        string strID;

        strUserCode = Session["UserCode"].ToString();
        strUserType = ShareClass.GetUserType(strUserCode);

        strID = Request.QueryString["IdentifyString"];
        strType = Request.QueryString["Type"];

        if (Page.IsPostBack == false)
        {
            strHQL = string.Format(@"Select Distinct B.ID,A.ID as SystemModuleID,A.ModuleName,A.HomeModuleName,A.ParentModule,A.PageName,A.ModuleType,B.ModuleDefinition as UserModuleDefinition,A.ModuleDefinition as SystemModuleDefinition,
                A.UserType,A.IconURL,A.SortNumber,A.DIYFlow From T_ProModuleLevel A, T_ProModule B Where rtrim(A.ModuleName)
                ||rtrim(A.ModuleType)||rtrim(A.UserType) = rtrim(B.ModuleName) ||rtrim(B.ModuleType) 
                ||rtrim(B.UserType) and (CHAR_LENGTH(B.ModuleDefinition) > 0 Or CHAR_LENGTH(A.ModuleDefinition) > 0) and B.ID = {0}", strID);

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");

            if (ds.Tables[0].Rows.Count > 0)
            {
                TB_CopyRight.Text = System.Configuration.ConfigurationManager.AppSettings["CopyRight"];
                TB_WFIdentifyString.Text = strIdentifyString;

                strTemName = ds.Tables[0].Rows[0]["ModuleName"].ToString().Trim();
                TB_WFName.Text = strTemName;

                //取得流程图定义
                TB_WFXML.Text = WFMFFlowDefinitionHandle.GetModuleFlowDefinition(strID, strType, ds);

            }
        }
    }



    //取得当前模组名称
    protected static string getSystemModuleDefinition(string strSystemModuleID)
    {
        string strHQL;

        strHQL = "Select ModuleName From T_ProModuleLevel Where ID = " + strSystemModuleID;
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

    //取得当前模组名称
    protected string getTrueModuleNameByURL(string strFromHomeModuleName, string strModuleURL, string strLangCode)
    {
        string strHQL;

        strHQL = "Select ModuleName From T_ProModuleLevel Where HomeModuleName = '" + strFromHomeModuleName + "' and PageName = '" + strModuleURL + "' and LangCode = '" + strLangCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevel");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return strFromHomeModuleName;
        }
    }



}