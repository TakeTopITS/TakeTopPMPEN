using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.Win32.SafeHandles;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTPersonalSpaceNewsTypeEdit : System.Web.UI.Page
{
    private string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ��jack.erp@gmail.com)
        //̩�����2006��2012

        string strUserCode = Session["UserCode"].ToString();
        strLangCode = Session["LangCode"].ToString();

        LB_UserCode.Text = strUserCode;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "������������", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadNewsTypeList(strLangCode);

            ShareClass.LoadLanguageForDropList(ddlLangSwitcher);
            ddlLangSwitcher.SelectedValue = strLangCode;
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();
            LB_ID.Text = strID;

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "Select * From T_NewsType Where ID = " + strID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_NewsTy;e");

                string strNewsType = ds.Tables[0].Rows[0]["Type"].ToString();
                string strHomeNewsType = ds.Tables[0].Rows[0]["HomeName"].ToString().Trim();
                string strNewsScope = ds.Tables[0].Rows[0]["NewsScope"].ToString().Trim();
                string strPageName = ds.Tables[0].Rows[0]["PageName"].ToString().Trim();
                string strVisible = ds.Tables[0].Rows[0]["Visible"].ToString().Trim();
                string strSortNumber = ds.Tables[0].Rows[0]["SortNumber"].ToString();


                TB_NewsType.Text = strNewsType;
                TB_HomeNewsType.Text = strHomeNewsType;
                DL_NewsScope.SelectedValue = strNewsScope;
                TB_NewsTypePageName.Text = strPageName;
                DL_Visible.SelectedValue = strVisible;
                TB_NewsTypeSort.Text = strSortNumber;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                DeleteType();
            }
        }
    }
    protected void ddlLangSwitcher_SelectedIndexChanged(object sender, EventArgs e)
    {
        strLangCode = ddlLangSwitcher.SelectedValue;

        LoadNewsTypeList(strLangCode);
    }

    protected void DL_NewsTypeRelatedPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_NewsTypePageName.Text = DL_NewsTypeRelatedPage.SelectedValue;
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddType();
        }
        else
        {
            UpdateType();
        }
    }

    protected void AddType()
    {
        string strHQL;
        string strType, strHomeType, strNewsScope, strPageName, strVisible, strFromLangCode, strSortNumber;

        strType = TB_NewsType.Text.Trim();
        strHomeType = TB_HomeNewsType.Text.Trim();
        strNewsScope = DL_NewsScope.SelectedValue.Trim();
        strPageName = TB_NewsTypePageName.Text.Trim();
        strVisible = DL_Visible.SelectedValue.Trim();
        strSortNumber = TB_NewsTypeSort.Text.Trim();

        strFromLangCode = ddlLangSwitcher.SelectedValue.Trim();

        if (strType == "" | strPageName == "" | strSortNumber == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        try
        {
            strHQL = "Insert Into T_NewsType(Type,HomeName,NewsScope,PageName,LangCode,Visible,SortNumber) Values('" + strType + "','" + strHomeType + "','" + strNewsScope + "','" + strPageName + "','" + strFromLangCode + "','" + strVisible + "'," + strSortNumber + ")";
            ShareClass.RunSqlCommand(strHQL);
            LB_ID.Text = GetNewsTypeMaxID().ToString();

            LoadNewsTypeList(ddlLangSwitcher.SelectedValue.Trim());

            //���û�����ı�־����ˢ��ҳ�滺��
            ChangePageCache();
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateType()
    {
        string strHQL;
        string strType, strHomeName, strID, strNewsScope, strPageName, strVisible, strFromLangCode, strSortNumber;

        strFromLangCode = ddlLangSwitcher.SelectedValue.Trim();

        strID = LB_ID.Text;
        strType = TB_NewsType.Text.Trim();
        strVisible = DL_Visible.SelectedValue.Trim();
        strSortNumber = TB_NewsTypeSort.Text.Trim();
        strPageName = TB_NewsTypePageName.Text.Trim();
        strHomeName = TB_HomeNewsType.Text.Trim();
        strNewsScope = DL_NewsScope.SelectedValue.Trim();

        try
        {

            strHQL = "Update T_NewsType Set HomeName = '" + strHomeName + "', SortNumber = " + strSortNumber + ",PageName = '" + strPageName + "',Visible = '" + strVisible + "',NewsScope ='" + strNewsScope + "' Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadNewsTypeList(ddlLangSwitcher.SelectedValue.Trim());

            //���û�����ı�־����ˢ��ҳ�滺��
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DeleteType()
    {
        string strHQL;
        string strID;

        strID = LB_ID.Text.Trim();

        try
        {
            strHQL = "Delete From T_NewsType Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadNewsTypeList(ddlLangSwitcher.SelectedValue.Trim());

            //���û�����ı�־����ˢ��ҳ�滺��
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_SaveSortNumber_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strVisible, strNewsScoope;
        int j = 0, intSortNumber;

        try
        {
            for (j = 0; j < DataGrid4.Items.Count; j++)
            {
                strID = DataGrid4.Items[j].Cells[2].Text;

                if (((CheckBox)DataGrid4.Items[j].FindControl("CB_Visible")).Checked)
                {
                    strVisible = "YES";
                }
                else
                {
                    strVisible = "NO";
                }

                strNewsScoope = ((DropDownList)DataGrid4.Items[j].FindControl("DL_NewsScope")).SelectedValue;
                intSortNumber = int.Parse(((TextBox)(DataGrid4.Items[j].FindControl("TB_SortNumber"))).Text.Trim());

                strHQL = string.Format(@"Update T_NewsType Set Visible = '{0}',NewsScope='{1}',SortNumber = {2} Where ID = {3}", strVisible, strNewsScoope, intSortNumber.ToString(), strID);
                ShareClass.RunSqlCommand(strHQL);

            }

            //���û�����ı�־����ˢ��ҳ�滺��
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_CopyForHomeLanguage_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strFromLangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];

        strLangCode = ddlLangSwitcher.SelectedValue.Trim();
        try
        {
            strHQL = "Insert Into T_NewsType(Type,SortNumber,PageName,LangCode,HomeName,Visible,NewsScope)";
            strHQL += " SELECT Type,SortNumber ,PageName," + "'" + strLangCode + "'" + ",HomeName,Visible,NewsScope FROM T_NewsType";
            strHQL += " Where LangCode = '" + strFromLangCode + "' and ltrim(rtrim(Type)) || " + "'" + strLangCode + "'" + " Not in (Select ltrim(rtrim(Type)) || ltrim(rtrim(LangCode))  From T_NewsType Where LangCode = " + "'" + strLangCode + "'" + ")";
            ShareClass.RunSqlCommand(strHQL);

            LoadNewsTypeList(strLangCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void LoadNewsTypeList(string strLangCode)
    {
        string strVisible, strNewsScope;

        string strHQL = "Select * From T_NewsType  Where LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_NewsType");
        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();


        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strVisible = ds.Tables[0].Rows[i]["Visible"].ToString().Trim();

            if (strVisible == "YES")
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_Visible")).Checked = true;
            }
            else
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_Visible")).Checked = false;
            }

            strNewsScope = ds.Tables[0].Rows[i]["NewsScope"].ToString().Trim();
            ((DropDownList)DataGrid4.Items[i].FindControl("DL_NewsScope")).SelectedValue = strNewsScope;
        }
    }

    protected int GetNewsTypeMaxID()
    {
        string strHQL;

        strHQL = "Select Max(ID) From T_NewsType";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_NewsType");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    //���û�����ı�־����ˢ��ҳ�滺��
    protected void ChangePageCache()
    {
        //���û�����ı�־
        ShareClass.SetPageCacheMark("1");
        Session["CssDirectoryChangeNumber"] = "1";

        //����������˿ռ���ӿ�����ˢ��ҳ�滺��
        ShareClass.AddSpaceLineToPersonalSpaceForRefreshCache();
    }
}