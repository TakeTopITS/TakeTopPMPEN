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
using System.IO;

using System.Data.SqlClient;
using System.Data.OleDb;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;

public partial class TTPersonalSpaceModuleSetForUser : System.Web.UI.Page
{
    string strLangCode, strUserCode, strUserType;
    string strForbitModule;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserType = Session["UserType"].ToString();
        strLangCode = Session["LangCode"].ToString();

        strForbitModule = Session["ForbitModule"].ToString();

    
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadUserModule(strUserCode, strUserType, strLangCode);
        }
    }

    protected void BT_ModuleSave_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strVisible;

        int j = 0, intSortNumber;

        try
        {
            for (j = 0; j < DataGrid4.Items.Count; j++)
            {
                strID = DataGrid4.Items[j].Cells[0].Text;

                intSortNumber = int.Parse(((TextBox)(DataGrid4.Items[j].FindControl("TB_SortNumber"))).Text.Trim());

                strHQL = "Update T_ProModuleLevelForPageUser Set SortNumber = " + intSortNumber.ToString() + " Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                if (((CheckBox)DataGrid4.Items[j].FindControl("CB_ModuleVisible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }

                strHQL = "Update T_ProModuleLevelForPageUser Set Visible = " + "'" + strVisible + "'" + " Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);
            }

           
            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click111", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click222", "reloadPrentPage();", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void LoadUserModule(string strUserCode,string strUserType,string strLangCode)
    {
        string strHQL;
        string strSortNumber, strVisible, strModuleName;


        strHQL = string.Format(@"Insert Into T_ProModuleLevelForPageUser(ModuleName,UserCode,UserType,Visible,SortNumber)
                Select ModuleName,'{1}',UserType,Visible,SortNumber From t_promodulelevelforpage
	            Where ParentModule = 'PersonalSpace'  and LangCode = '{0}'
		        and ModuleName Not In (Select ModuleName From T_ProModuleLevelForPageUser Where UserCode = '{1}');", strLangCode, strUserCode);
        ShareClass.RunSqlCommand(strHQL);

        if (Session["SystemVersionType"].ToString() != "SAAS")
        {
            if (strUserType == "INNER")
            {
                strHQL = string.Format(@"select distinct A.ID,A.ModuleName,B.homemodulename,A.SortNumber,A.Visible from T_ProModuleLevelForPageUser A,T_ProModuleLevelForPage B 
                       where A.ModuleName = B.ModuleName and B.Visible = 'YES' and B.IsDeleted = 'NO' and A.UserCode = '{0}' and A.UserType = '{1}' and B.LangCode = '{2}' 
                       and B.ParentModule = 'PersonalSpace' Order By SortNumber ASC", strUserCode, strUserType, strLangCode);
            }
            else
            {
                strHQL = string.Format(@"select distinct A.ID,A.ModuleName,B.homemodulename,A.SortNumber,A.Visible from T_ProModuleLevelForPageUser A,T_ProModuleLevelForPage B 
                       where A.ModuleName = B.ModuleName and B.Visible = 'YES' and B.IsDeleted = 'NO' and A.UserCode = '{0}' and A.UserType = '{1}' and B.LangCode = '{2}' 
                       and B.ParentModule = 'ExternalPersonalSpace' Order By SortNumber ASC", strUserCode, strUserType, strLangCode);
            }
        }
        else
        {
            strHQL = string.Format(@"select distinct A.ID,A.ModuleName,B.homemodulename,A.SortNumber,A.Visible from T_ProModuleLevelForPageUser A,T_ProModuleLevelForPage B 
                       where A.ModuleName = B.ModuleName and B.Visible = 'YES' and B.IsDeleted = 'NO' and A.UserCode = '{0}' and A.UserType = '{1}' and B.LangCode = '{2}' 
                       and B.ParentModule = 'PersonalSpaceSaaS' Order By SortNumber ASC", strUserCode, strUserType, strLangCode);
        }

        LogClass.WriteLogFile(strHQL);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProModuleLevelForPage");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        BT_ModuleSave.Enabled = true;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strSortNumber = ds.Tables[0].Rows[i]["SortNumber"].ToString().Trim();
            ((TextBox)DataGrid4.Items[i].FindControl("TB_SortNumber")).Text = strSortNumber;

            strModuleName = ds.Tables[0].Rows[i]["ModuleName"].ToString().Trim();
            strVisible = ds.Tables[0].Rows[i]["Visible"].ToString().Trim();

            if (strVisible == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_ModuleVisible")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_ModuleVisible")).Checked = false;
            }
        }

        LB_ModuleNumber.Text = ds.Tables[0].Rows.Count.ToString();
    }

 

    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {
        //设置缓存更改标志
        ShareClass.SetPageCacheMark("1");
        Session["CssDirectoryChangeNumber"] = "1";

        //给主界面个人空间添加空行以刷新页面缓存
        ShareClass.AddSpaceLineToPersonalSpaceForRefreshCache();
    }

}
