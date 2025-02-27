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
using System.Security.Permissions;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Resources;
using NPOI.HSSF.Record;
using sun.swing;
using Microsoft.International.Converters.PinYinConverter;
using TakeTopGantt.models;
using static sun.rmi.log.ReliableLog;
using System.Xml.Linq;


public partial class TTDatabaseUpgrade : System.Web.UI.Page
{
    string strUserCode;
    string strLangCode;
    string strFileName = "DBUpdateFile.xml";
    protected void Page_Load(object sender, EventArgs e)
    {

        strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "数据库升级维护", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            lbl_ID.Text = GetMaxDataBaseUpgradeID();
            TB_ID.Text = (int.Parse(lbl_ID.Text.Trim()) + 1).ToString();

            LoadDatabaseUpgrateRecord();

            ShareClass.LoadLanguageForDropList(ddlLangSwitcher);
            ddlLangSwitcher.SelectedValue = strLangCode;
        }

    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strNewSQL = TB_NewUpdateSQL.Text.Trim();
        string strID = TB_ID.Text.Trim();

        string strPassword = TB_Password.Text.Trim();

        if (strPassword != "ZHONGLYISGREATEMAN@#!")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMYCBDBNSJJC").ToString().Trim() + "')", true);
            return;
        }

        lbl_ID.Text = GetMaxDataBaseUpgradeID();
        if (strID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJBHBNWKBXSDY0DZSJC").ToString().Trim() + "')", true);
            TB_ID.Focus();
            return;
        }
        if (int.Parse(strID) <= int.Parse(lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJBHBXDYXZDBHDZSQJC").ToString().Trim() + "')", true);
            TB_ID.Focus();
            return;
        }
        if (strNewSQL == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJNRBNWKJC").ToString().Trim() + "')", true);
            TB_NewUpdateSQL.Focus();
            return;
        }

        try
        {
            ShareClass.RunSqlCommand(strNewSQL);

            //更新SQL语句存贮在数据库更新表和XML文件
            //string strHQL;
            //strHQL = "Insert Into T_DatabaseUpgrate(ID,SQLText,IsSucess,UpdateTime) Values(" + strID + ",'" + strNewSQL + "','YES',now()" +")";
            //ShareClass.RunSqlCommand(strHQL);

            DataBaseUpgrateBLL dataBaseUpgrateBLL = new DataBaseUpgrateBLL();
            DataBaseUpgrate dataBaseUpgrate = new DataBaseUpgrate();
            dataBaseUpgrate.ID = int.Parse(strID);
            dataBaseUpgrate.SQLText = strNewSQL;
            dataBaseUpgrate.IsSucess = "YES";
            dataBaseUpgrate.UpdateTime = DateTime.Now;
            dataBaseUpgrateBLL.AddDataBaseUpgrate(dataBaseUpgrate);

            FileInfo fi = new FileInfo(Server.MapPath("../UpdateCode") + "\\" + strFileName);
            if (fi.Exists)//存在，则追加XML，并保存到数据库中
            {
                AddXmlInformation(Server.MapPath("../UpdateCode") + "\\" + strFileName, int.Parse(strID), strNewSQL);
            }
            else//不存在，则创建XML，并保存到数据库中
            {
                GenerateXMLFile(Server.MapPath("../UpdateCode") + "\\" + strFileName, int.Parse(strID), strNewSQL);
            }

            lbl_ID.Text = GetMaxDataBaseUpgradeID();

            LoadDatabaseUpgrateRecord();

            //设置缓存更改标志
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGSJJLYBC").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBSJDMYWJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_CompareByHomeLanguage_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string strHQL;
            string strHomeLangCode;

            strHomeLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];

            strLangCode = ddlLangSwitcher.SelectedValue.Trim();

            strHQL = "Truncate Table T_LanguageResourceHome";
            ShareClass.RunSqlCommand(strHQL);
            strHQL = "Truncate Table T_LanguageResourceOther";
            ShareClass.RunSqlCommand(strHQL);

            string strOtherLangResxFile = Server.MapPath("../App_GlobalResources//lang." + strLangCode + ".resx");
            ResXResourceReader rrOther = new ResXResourceReader(strOtherLangResxFile);
            IDictionaryEnumerator ideOther = rrOther.GetEnumerator();
            while (ideOther.MoveNext())
            {
                strHQL = "Insert Into T_LanguageResourceOther(KeyName,KeyValue) Values('" + ideOther.Key + "','" + ideOther.Value.ToString().Replace("'", "") + "')";
                ShareClass.RunSqlCommand(strHQL);
            }
            rrOther.Close();

            string strHomeLangResxFile = Server.MapPath("../App_GlobalResources//lang.resx");
            ResXResourceReader rrHome = new ResXResourceReader(strHomeLangResxFile);
            IDictionaryEnumerator ideHome = rrHome.GetEnumerator();
            while (ideHome.MoveNext())
            {
                if (ideHome.Value.ToString().Trim() == "")
                {
                    strHQL = "Delete From T_LanguageResourceHome Where KeyName = '" + ideHome.Key + "'";
                    continue;
                }
                strHQL = "Insert Into T_LanguageResourceHome(KeyName,KeyValue) Values('" + ideHome.Key + "','" + ideHome.Value.ToString().Replace("'", "") + "')";
                ShareClass.RunSqlCommand(strHQL);
            }
            rrHome.Close();

            strHQL = "Select KeyName,KeyValue From T_LanguageResourceHome Where KeyName Not In (Select KeyName From T_LanguageResourceOther)";

            MSExcelHandler.DataTableToExcel(strHQL, strOtherLangResxFile + ".xls");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('OK！')", true);

            return;
        }
    }


    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {
        //设置缓存更改标志
        ShareClass.SetPageCacheMark("1");
        Session["CssDirectoryChangeNumber"] = "1";

        //设置缓存更改标志
        ShareClass.AddSpaceLineToLeftColumnForRefreshCache();
    }

    protected void LoadDatabaseUpgrateRecord()
    {
        string strHQL;

        strHQL = "Select ID,SqlText,IsSucess,UpdateTime From T_DatabaseUpgrate Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DatabaseUpgrate");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        string strHQL;

        strHQL = "Select ID,SqlText,IsSucess,UpdateTime From T_DatabaseUpgrate Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DatabaseUpgrate");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    /// <summary>
    /// 获取最大ID编号
    /// </summary>
    /// <returns></returns>
    protected string GetMaxDataBaseUpgradeID()
    {
        string flag = "0";
        string strHQL = "Select COALESCE(Max(ID),0) From T_DataBaseUpgrate ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DataBaseUpgrate");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = ds.Tables[0].Rows[0][0].ToString();
        }
        return flag;
    }

    /// <summary>
    /// 创建数据库更新XML文件
    /// </summary>
    /// <param name="xmlFilePath"></param>
    /// <param name="strID"></param>
    /// <param name="strSQLText"></param>
    protected void GenerateXMLFile(string xmlFilePath, int strID, string strSQLText)
    {
        try
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement rootElement = xmldoc.CreateElement("DataBaseUpgradeFiles");
            xmldoc.AppendChild(rootElement);

            XmlElement firstLevelElement1 = xmldoc.CreateElement("DataBaseUpgradeFile");
            rootElement.AppendChild(firstLevelElement1);
            XmlElement secondLevelElement11 = xmldoc.CreateElement("ID");
            secondLevelElement11.InnerText = strID.ToString();
            firstLevelElement1.AppendChild(secondLevelElement11);
            XmlElement secondLevelElement12 = xmldoc.CreateElement("SQLText");
            secondLevelElement12.InnerText = strSQLText;
            firstLevelElement1.AppendChild(secondLevelElement12);

            xmldoc.Save(xmlFilePath);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 追加数据库更新文件XML
    /// </summary>
    /// <param name="xmlFilePath"></param>
    /// <param name="strID"></param>
    /// <param name="strSQLText"></param>
    protected void AddXmlInformation(string xmlFilePath, int strID, string strSQLText)
    {
        try
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlFilePath);
            XmlNode root = xmldoc.SelectSingleNode("DataBaseUpgradeFiles");
            XmlElement firstLevelElement1 = xmldoc.CreateElement("DataBaseUpgradeFile");
            XmlElement secondLevelElement11 = xmldoc.CreateElement("ID");
            secondLevelElement11.InnerText = strID.ToString();
            firstLevelElement1.AppendChild(secondLevelElement11);
            XmlElement secondLevelElement12 = xmldoc.CreateElement("SQLText");
            secondLevelElement12.InnerText = strSQLText;
            firstLevelElement1.AppendChild(secondLevelElement12);
            root.AppendChild(firstLevelElement1);
            xmldoc.Save(xmlFilePath);
        }
        catch
        {
        }
    }

}
