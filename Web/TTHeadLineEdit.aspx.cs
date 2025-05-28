using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;

public partial class TTHeadLineEdit : System.Web.UI.Page
{
    string strUserCode;
    string strIsMobileDevice;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartString;

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(CKE_MainContent);
CKE_MainContent.Language = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "新闻编辑", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            try
            {
                ShareClass.LoadLanguageForDropList(ddlLangSwitcher);
                strLangCode = Session["LangCode"].ToString();
                ddlLangSwitcher.SelectedValue = strLangCode;
            }
            catch
            {
                Response.Redirect("TTDisplayErrors.aspx");
            }


            if (strIsMobileDevice == "YES")
            {
                HTEditor1.Visible = true;
            }
            else
            {
                CKE_MainContent.Visible = true;
            }
            LoadNewsType();

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityNewsNotice(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);

            strHQL = "from HeadLine as headLine where headLine.PublisherCode = " + "'" + strUserCode + "'";
            strHQL += " Order by headLine.ID DESC";
            HeadLineBLL headLineBLL = new HeadLineBLL();
            lst = headLineBLL.GetAllHeadLines(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_Sql.Text = strHQL;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            TB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                strHQL = "Select * From T_HeadLine Where ID = " + strID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HeadLine");

                LB_ID.Text = strID;
                TB_Title.Text = ds.Tables[0].Rows[0]["Title"].ToString().Trim();

                if (strIsMobileDevice == "YES")
                {
                    HTEditor1.Text = ds.Tables[0].Rows[0]["Content"].ToString().Trim();
                }
                else
                {
                    CKE_MainContent.Text = ds.Tables[0].Rows[0]["Content"].ToString().Trim();
                }

                try
                {
                    TB_DepartCode.Text = ds.Tables[0].Rows[0]["RelatedDepartCode"].ToString().Trim();
                    LB_DepartName.Text = ShareClass.GetDepartName(TB_DepartCode.Text);
                }
                catch
                {
                }

                try
                {

                    HL_ContentDocURL.NavigateUrl = ds.Tables[0].Rows[0]["ContentDocURL"].ToString().Trim();
                    TB_ContentDocURL.Text = ds.Tables[0].Rows[0]["ContentDocURL"].ToString().Trim();

                    DL_Statu.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString().Trim();

                    DL_NewsType.SelectedValue = ds.Tables[0].Rows[0]["NewsType"].ToString().Trim();

                    DL_Type.SelectedValue = ds.Tables[0].Rows[0]["Type"].ToString().Trim();


                    DL_IsHead.SelectedValue = ds.Tables[0].Rows[0]["IsHead"].ToString().Trim();

                    ddlLangSwitcher.SelectedValue = ds.Tables[0].Rows[0]["LangCode"].ToString().Trim();

                }
                catch(Exception err)
                {
                    LogClass.WriteLogFile(err.Message.ToString());
                }

             
                BT_Publish.Enabled = true;
                BT_Archive.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }


            if (e.CommandName == "Delete")
            {
                try
                {
                    strHQL = "Delete From T_HeadLine Where ID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);

                
                    BT_Publish.Enabled = false;
                    BT_Archive.Enabled = false;

                    LoadHeadLine();

                    //设置缓存更改标志，并刷新页面缓存
                    ChangePageCache();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_Import_Click(object sender, EventArgs e)
    {
        string strWordFile, strHTMLFile, strDocSavePath, strContentFileName;

        try
        {
            strWordFile = TB_ContentDocURL.Text.Trim();

            if (strWordFile != "")
            {
                strWordFile = Server.MapPath(strWordFile);

                strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";
                strContentFileName = "WordHeadLine" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".html";

                strHTMLFile = strDocSavePath + strContentFileName;

                MSWordHandler.WordToHTML(strWordFile, strHTMLFile);

                CKE_MainContent.Text = MSWordHandler.ImportToStringFromFile(strHTMLFile);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDRCG").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDRSBJC").ToString().Trim() + "')", true);
            }

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDRSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
            AddNews();
        }
        else
        {
            UpdateNews();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void AddNews()
    {
        string strTitle, strType, strContent, strDepartCode, strDepartName;

        strType = DL_Type.SelectedValue.Trim();
        strTitle = TB_Title.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HTEditor1.Text.Trim();
        }
        else
        {
            strContent = CKE_MainContent.Text.Trim();
        }

        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = LB_DepartName.Text.Trim();

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGSBMBNWKCZSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }
 
        HeadLineBLL headLineBLL = new HeadLineBLL();
        HeadLine headLine = new HeadLine();

        headLine.Title = strTitle;
        headLine.Type = strType;
        headLine.Content = strContent;
        headLine.ContentDocUrl = TB_ContentDocURL.Text.Trim();

        headLine.RelatedDepartCode = strDepartCode;
        headLine.RelatedDepartName = strDepartName;
        headLine.PublishTime = DateTime.Now;
        headLine.PublisherCode = strUserCode;
        headLine.PublisherName = ShareClass.GetUserName(strUserCode);
        headLine.Status = "New";
        headLine.IsHead = DL_IsHead.SelectedValue.Trim();
        headLine.LangCode = ddlLangSwitcher.SelectedValue.Trim();
        headLine.NewsType = DL_NewsType.SelectedValue.Trim();

        try
        {
            headLineBLL.AddHeadLine(headLine);

            LB_ID.Text = ShareClass.GetMyCreatedMaxHeadLineID(strUserCode);

            string strID = LB_ID.Text.Trim();

            ////保存内容文件URL
            SaveHeadlineDocURL(strID, MSWordHandler.HTMLToWord(strID, CKE_MainContent.Text));

            DL_Statu.SelectedValue = "New";

            BT_Publish.Enabled = true;
            BT_Archive.Enabled = true;

            LoadHeadLine();

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateNews()
    {
        string strHQL;
        IList lst;
        string strID, strTitle, strType, strContent, strDepartCode, strDepartName;

        strID = LB_ID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strTitle = TB_Title.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HTEditor1.Text.Trim();
        }
        else
        {
            strContent = CKE_MainContent.Text.Trim();
        }

        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = LB_DepartName.Text.Trim();

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGSBMBNWKCZSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        try
        {
            strHQL = "From HeadLine as headLine Where headLine.ID = " + strID;
            HeadLineBLL headLineBLL = new HeadLineBLL();
            lst = headLineBLL.GetAllHeadLines(strHQL);
            HeadLine headLine = (HeadLine)lst[0];

            headLine.Title = strTitle;
            headLine.Type = strType;
            headLine.Content = strContent;
            headLine.ContentDocUrl = TB_ContentDocURL.Text.Trim();

            headLine.RelatedDepartCode = strDepartCode;
            headLine.RelatedDepartName = strDepartName;
            headLine.PublishTime = DateTime.Now;
            headLine.PublisherCode = strUserCode;
            headLine.PublisherName = ShareClass.GetUserName(strUserCode);
            headLine.IsHead = DL_IsHead.SelectedValue.Trim();
            headLine.LangCode = ddlLangSwitcher.SelectedValue.Trim();
            headLine.NewsType = DL_NewsType.SelectedValue.Trim();

           

            headLineBLL.UpdateHeadLine(headLine, int.Parse(strID));

            new System.Threading.Thread(delegate ()
            {
                //保存内容文件URL
                SaveHeadlineDocURL(strID, MSWordHandler.HTMLToWord(strID, CKE_MainContent.Text));

            }).Start();

            LoadHeadLine();

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch(Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Publish_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strTitle, strType, strContent, strDepartCode, strDepartName;


        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddNews();
        }
        else
        {
            UpdateNews();
        }

        strID = LB_ID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strTitle = TB_Title.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HTEditor1.Text.Trim();
        }
        else
        {
            strContent = CKE_MainContent.Text.Trim();
        }

        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = LB_DepartName.Text.Trim();

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGSBMBNWKCZSBJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            strHQL = "From HeadLine as headLine Where headLine.ID = " + strID;
            HeadLineBLL headLineBLL = new HeadLineBLL();
            lst = headLineBLL.GetAllHeadLines(strHQL);
            HeadLine headLine = (HeadLine)lst[0];

            headLine.Title = strTitle;
            headLine.Type = strType;
            headLine.Content = strContent;
            headLine.ContentDocUrl = TB_ContentDocURL.Text.Trim();

            headLine.RelatedDepartCode = strDepartCode;
            headLine.RelatedDepartName = strDepartName;
            headLine.PublishTime = DateTime.Now;
            headLine.PublisherCode = strUserCode;
            headLine.PublisherName = ShareClass.GetUserName(strUserCode);
            headLine.IsHead = DL_IsHead.SelectedValue.Trim();
            headLine.LangCode = ddlLangSwitcher.SelectedValue.Trim();
            headLine.NewsType = DL_NewsType.SelectedValue.Trim();
            headLine.Status = "Publish";   

            headLineBLL.UpdateHeadLine(headLine, int.Parse(strID));

            new System.Threading.Thread(delegate ()
            {
                //保存内容文件URL
                SaveHeadlineDocURL(strID, MSWordHandler.HTMLToWord(strID, CKE_MainContent.Text));

            }).Start();

            DL_Statu.SelectedValue = "Publish";   
        
            LoadHeadLine();

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFBCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFBSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }
    protected void BT_Archive_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strID, strTitle, strType, strContent, strDepartCode, strDepartName;


        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddNews();
        }
        else
        {
            UpdateNews();
        }

        strID = LB_ID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strTitle = TB_Title.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HTEditor1.Text.Trim();
        }
        else
        {
            strContent = CKE_MainContent.Text.Trim();
        }

        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = LB_DepartName.Text.Trim();

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGSBMBNWKCZSBJC").ToString().Trim() + "')", true);
            return;
        }

        strID = LB_ID.Text.Trim();
        strTitle = TB_Title.Text.Trim();

        try
        {
            strHQL = "From HeadLine as headLine Where headLine.ID = " + strID;
            HeadLineBLL headLineBLL = new HeadLineBLL();
            lst = headLineBLL.GetAllHeadLines(strHQL);
            HeadLine headLine = (HeadLine)lst[0];

            headLine.Title = strTitle;
            headLine.Type = strType;
            headLine.Content = strContent;
            headLine.ContentDocUrl = TB_ContentDocURL.Text.Trim();

            headLine.RelatedDepartCode = strDepartCode;
            headLine.RelatedDepartName = strDepartName;
            headLine.PublishTime = DateTime.Now;
            headLine.PublisherCode = strUserCode;
            headLine.PublisherName = ShareClass.GetUserName(strUserCode);
            headLine.IsHead = DL_IsHead.SelectedValue.Trim();
            headLine.LangCode = ddlLangSwitcher.SelectedValue.Trim();
            headLine.NewsType = DL_NewsType.SelectedValue.Trim();

            headLine.Status = "Archived";

            headLineBLL.UpdateHeadLine(headLine, int.Parse(strID));

            new System.Threading.Thread(delegate ()
            {
                //保存内容文件URL
                SaveHeadlineDocURL(strID, MSWordHandler.HTMLToWord(strID, CKE_MainContent.Text));

            }).Start();


            DL_Statu.SelectedValue = "Archived";
            LoadHeadLine();

            //设置缓存更改标志，并刷新页面缓存
            ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG").ToString().Trim() + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        IList lst;

        HeadLineBLL headLineBLL = new HeadLineBLL();
        lst = headLineBLL.GetAllHeadLines(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    //保存新闻内容的文档链接
    public void SaveHeadlineDocURL(string strID, string strFileURL)
    {
        string strHQL;

        try
        {
            strHQL = "Update T_Headline Set ContentDocURL = '" + strFileURL + "' Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            HL_ContentDocURL.NavigateUrl = strFileURL;
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
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

    protected void LoadHeadLine()
    {
        string strHQL;
        IList lst;

        strHQL = "from HeadLine as headLine where headLine.PublisherCode = " + "'" + strUserCode + "'";
        strHQL += " Order by headLine.ID DESC";
        HeadLineBLL headLineBLL = new HeadLineBLL();
        lst = headLineBLL.GetAllHeadLines(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadNewsType()
    {
        string strHQL;

        strHQL = "Select * From T_NewsType Where LangCode = '" + strLangCode + "'";
        strHQL += " Order By SortNumber ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_NewsType");
        DL_NewsType.DataSource = ds;
        DL_NewsType.DataBind();
    }
}
