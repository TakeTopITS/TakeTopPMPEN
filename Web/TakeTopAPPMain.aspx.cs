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

using ProjectMgt.BLL;
using ProjectMgt.Model;

using TakeTopCore;
using TakeTopSecurity;

public partial class TakeTopAPPMain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶拓鼎（2006－2026）

        string strUserCode;
        string strLangCode;
        string strForbitModule;

        string strUserName, strUserType;


        if (Page.IsPostBack != true)
        {
            strUserCode = Session["UserCode"].ToString();
            strUserName = Session["UserName"].ToString();
            strUserType = Session["UserType"].ToString();
            strLangCode = Session["LangCode"].ToString();

            strForbitModule = Session["ForbitModule"].ToString();

            Session["SystemName"] = System.Configuration.ConfigurationManager.AppSettings["SystemName"];

            try
            {
                SetAnalystModuleVisible(strUserCode, "AnalysisChart", strLangCode, strUserType);   
            }
            catch
            {
            }

            try
            {
                BindModuleData(strUserCode, strUserType, strForbitModule, strLangCode);
            }
            catch
            {
            }
        }
    }

    protected void IMB_OpenLock_Click(object sender, ImageClickEventArgs e)
    {
        string strHQL;

        string strUserCode = Session["UserCode"].ToString();

        strHQL = "Update T_ProjectMember Set WeChatOpenID ='',WeChatUserID='',WeChatDeviceID='' Where UserCode = '" + strUserCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZNDZHHWXHYJJCSD") + "')", true);
    }

    protected void BindModuleData(string strUserCode, string strUserType, string strForbitModule, string strLangCode)
    {
        string strHtml = string.Empty;
        string strHQL;

        strHQL = string.Format(@"Select Distinct A.ID,A.ModuleName,A.HomeModuleName,A.ParentModule,A.PageName,A.ModuleType,
                A.UserType,A.IconURL,A.SortNumber From T_ProModuleLevel A, T_ProModule B Where rtrim(A.ModuleName)
                ||rtrim(A.ModuleType)||rtrim(A.UserType) = rtrim(B.ModuleName) ||rtrim(B.ModuleType) 
                ||rtrim(B.UserType)  and A.Visible = 'YES' and A.IsDeleted = 'NO' and A.ModuleName <> 'AnalysisChart'
                and A.ModuleType In ('APP','DIYAPP') and A.UserType = 'INNER' and B.UserType = '{3}' 
                and B.UserCode = '{0}' and B.Visible = 'YES' and B.ModuleType in('APP','DIYAPP') and position(rtrim(A.ModuleName)||',' in '{1}') = 0
                and A.LangCode = '{2}' Order By A.SortNumber ASC", strUserCode, strForbitModule, strLangCode, strUserType);   

        DataTable dtModule = ShareClass.GetDataSetFromSql(strHQL, "Module").Tables[0];
        DataView dvModule = new DataView(dtModule);
        dvModule.RowFilter = " ParentModule = '' ";

        //bool isFirst = false;
        strHtml += "<div class=\"boxline\">";
        strHtml += "<div class=\"cline\"></div>";

        foreach (DataRowView row in dvModule)
        {
            string classTemp = "box";
            strHtml += "<div class=\"###\">";

            string strModuleName = ShareClass.ObjectToString(row["ModuleName"]).Trim();
            string strHomeModuleName = ShareClass.ObjectToString(row["HomeModuleName"]).Trim();
            string strPageName = ShareClass.ObjectToString(row["PageName"]).Trim();
            string strModuleType = ShareClass.ObjectToString(row["ModuleType"]).Trim();

            if (strLangCode == "ja" & strHomeModuleName.Length > 12)
            {
                strHomeModuleName = strHomeModuleName.Substring(0, 12);
            }

            if (strModuleName == LanguageHandle.GetWord("XinWen"))
            {
                try
                {
                    strHomeModuleName += "[<font color='red'>" + getUNReadNewsCount(strUserCode, strLangCode).ToString() + "</font>]";
                }
                catch
                {
                }
            }

            if (strModuleName == LanguageHandle.GetWord("DaiBanShiXiang"))
            {
                try
                {
                    strHomeModuleName += "[<font color='red'>" + GetUNHandledWorkCount(strUserCode, strLangCode).ToString() + "</font>]";
                }
                catch
                {
                }
            }

            if (strModuleType == "DIYAPP")
            {
                if (strPageName.IndexOf("?") >= 0)
                {
                    strPageName = strPageName + "&ModuleName=" + strModuleName + "&ModuleType=" + strModuleType;
                }
                else
                {
                    strPageName = strPageName + "?ModuleName=" + strModuleName + "&ModuleType=" + strModuleType;
                }
            }

            string strIconURL = ShareClass.ObjectToString(row["IconURL"]);
            if (strIconURL.Trim() == "")
            {
                strIconURL = @"imagesSkin/ModuleIcon.png";
            }

            dvModule.RowFilter = "ParentModule='" + strModuleName + "'";

            if (dvModule.Count > 0)
            {
                strHtml += "<div class=\"SpaceLine\"></div>";
                strHtml += "<div class=\"cline\"></div>";

                //需要给父节点加一个类
                classTemp = "box boxes";
                strHtml += "<a href=" + strPageName + "><span name=\"parent1\" class=\"boxs\" onmouseover=\"OnMouseOverEvent(this)\" onmouseout=\"OnMouseOutEvent(this)\"><span onclick=\"OnPlusEvent(this)\" class=\"plusSpan\"> </span><span onclick=\"OnMinusEvent(this)\" class=\"minusSpan\"><img src=\'" + strIconURL + "' /></span><span class=\"titleSpan\" onclick=\"javascript:document.getElementById('IMG_Waiting').style.display = 'block';location.href='" + strPageName + "'\" ondblclick=\"javascript:document.getElementById('IMG_Waiting').style.display = 'block';OnDoubleClickModule(this)\"><img src=\'" + strIconURL + "' />" + strHomeModuleName + "</span></span><a>";

                strHtml += "<div class=\"text\"> ";
                strHtml += "<ul class=\"content\">";

                foreach (DataRowView rowChild in dvModule)
                {
                    strIconURL = ShareClass.ObjectToString(rowChild["IconURL"]);
                    if (strIconURL.Trim() == "")
                    {
                        strIconURL = @"imagesSkin/ModuleIcon.png";
                    }

                    string strChildModuleName = ShareClass.ObjectToString(rowChild["ModuleName"]).Trim();
                    string strChildHomeModuleName = ShareClass.ObjectToString(rowChild["HomeModuleName"]).Trim();
                    string strChildPageName = ShareClass.ObjectToString(rowChild["PageName"]).Trim();
                    string strChildModuleType = ShareClass.ObjectToString(rowChild["ModuleType"]).Trim();

                    if(strChildModuleType == "DIYAPP")
                    {
                        if (strChildPageName.IndexOf("?") >= 0)
                        {
                            strChildPageName = strChildPageName + "&ModuleName=" + strChildModuleName + "&ModuleType=" + strChildModuleType;
                        }
                        else
                        {
                            strChildPageName = strChildPageName + "?ModuleName=" + strChildModuleName + "&ModuleType=" + strChildModuleType;
                        }
                    }

                    strHtml += "<li><a href=" + strChildPageName + "><span class=\"titleSpan\" onmouseover=\"OnMouseOverEvent(this)\" onmouseout=\"OnMouseOutEvent(this)\"><span onclick=\"OnMinusEvent(this)\" class=\"minusSpan\"><img src=\'" + strIconURL + "' /></span><span class=\"titleSpan\" onclick=\"javascript:document.getElementById('IMG_Waiting').style.display = 'block';location.href='" + strChildPageName + "'\" ondblclick=\"javascript:document.getElementById('IMG_Waiting').style.display = 'block';OnDoubleClickModule(this)\">" + strChildHomeModuleName + "</span></span></a></li>";
                }

                strHtml += "</ul>";
                strHtml += "</div>";

                strHtml += "<div class=\"SpaceLine\"></div>";
                strHtml += "<div class=\"cline\"></div>";
            }
            else
            {
                strHtml += "<a href=" + strPageName + "><span name=\"parent1\" class=\"boxs\" onmouseover=\"OnMouseOverEvent(this)\" onmouseout=\"OnMouseOutEvent(this)\"><span onclick=\"OnPlusEvent(this)\" class=\"plusSpan\"> </span><span onclick=\"OnMinusEvent(this)\" class=\"minusSpan\"><img src=\'" + strIconURL + "' /></span><span class=\"titleSpan\" onclick=\"javascript:document.getElementById('IMG_Waiting').style.display = 'block';location.href='" + strPageName + "'\" ondblclick=\"javascript:document.getElementById('IMG_Waiting').style.display = 'block';OnDoubleClickModule(this)\">" + strHomeModuleName + "</span></span></a>";
            }


            strHtml += "</div>";
            strHtml = strHtml.Replace("###", classTemp);


        }

        strHtml += "</div>";

        LT_Result.Text = strHtml + "<br /><br />";
    }


    //设置分析图模块是否可视
    protected void SetAnalystModuleVisible(string strUserCode, string strModuleName, string strLangCode, string strUserType)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_ProModule A,T_ProModuleLevel B Where A.ModuleName = B.ModuleName and A.UserCode = '{0}' and A.ModuleName = '{1}' and B.LangCode='{2}' and B.UserType='{3}' and A.Visible ='YES' and B.Visible='YES' and B.IsDeleted = 'NO'",
            strUserCode, strModuleName, strLangCode, strUserType);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModule");
        if (ds.Tables[0].Rows.Count > 0)
        {
            nav1.Visible = true;
        }
        else
        {
            nav1.Visible = false;

        }
    }

    protected int getUNReadNewsCount(string strUserCode, string strLangCode)
    {
        string strHQL;
        string strDepartCode;

        string strUserType = ShareClass.GetUserType(strUserCode);
        if (strUserType == "OUTER")
        {
            strUserType = "External";
        }
        else
        {
            strUserType = "Internal";
        }

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);

        strHQL = "Select * From T_HeadLine ";
        strHQL += " Where (RelatedDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))  or RelatedDepartCode = '" + strDepartCode + "')";
        strHQL += " and LangCode = '" + strLangCode + "' and ID Not in (Select NewsID From T_NewsRelatedUser Where UserCode  = '" + strUserCode + "')";
        strHQL += " And Type = " + "'" + strUserType + "'" + " and Status = 'Publish' Order By ID DESC";   

        DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "T_HeadLine");

        return ds.Tables[0].Rows.Count;
    }

    protected int GetUNHandledWorkCount(string strUserCode, string strLangCode)
    {
        #region 追加信息提示框信息  By LiuJianping 2014-02-12


        int i = 0;

        string strNumber = "";

        string strSuperDepartString = LB_SuperDepartString.Text.Trim();

        StringBuilder OldNumList = new StringBuilder();
        FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
        string strHQL_Fun = "From FunInforDialBox as funInforDialBoxBySystem Where funInforDialBoxBySystem.Status='Enabled'";
        strHQL_Fun += " and char_length(Ltrim(Rtrim(funInforDialBoxBySystem.MobileLinkAddress))) > 0 ";
        strHQL_Fun += " and funInforDialBoxBySystem.LangCode = " + "'" + strLangCode + "'";
        strHQL_Fun += " Order By funInforDialBoxBySystem.ID Desc ";
        IList lst_Fun = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL_Fun);
        if (lst_Fun.Count > 0 && lst_Fun != null)
        {
            for (int k = 0; k < lst_Fun.Count; k++)
            {
                FunInforDialBox funInforDialBox = (FunInforDialBox)lst_Fun[k];

                try
                {
                    string strHQL;
                    strHQL = funInforDialBox.SQLCode.Trim();
                    strHQL = strHQL.Replace("[TAKETOPUSERCODE]", strUserCode);
                    //strHQL = strHQL.Replace("[TAKETOPSUPERDEPARTSTRING]", strSuperDepartString);

                    DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "FunInforDialBoxList");

                    OldNumList.AppendFormat("{0},", ds.Tables[0].Rows.Count.ToString());
                }
                catch
                {
                }
            }
        }

        if (!string.IsNullOrEmpty(OldNumList.ToString().Trim()))
        {
            strNumber = OldNumList.ToString().Substring(0, OldNumList.ToString().Length - 1);

            string[] tempOldNumList = strNumber.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int L = 0; L < tempOldNumList.Length; L++)
            {
                i += int.Parse(tempOldNumList[L]);
            }
        }
        else
        {
            return 0;
        }
        #endregion

        return i;
    }
}
