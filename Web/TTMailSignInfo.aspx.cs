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


using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMailSignInfo : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;

        //CKEditor³õÊ¼»¯
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/"; Session["PageName"] = "TakeTopSiteContentEdit";
        _FileBrowser.SetupCKEditor(CKEditor1);
CKEditor1.Language = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustListBoxHeight();", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            try
            {
                strHQL = "Select ID,Title,SignInfo,Status From T_MailSignInfo Where UserCode = " + "'" + strUserCode + "'" + " Order By ID DESC";
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailSignInfo");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    LB_ID.Text = ds.Tables[0].Rows[0][0].ToString();
                    TB_Title.Text = ds.Tables[0].Rows[0][1].ToString().Trim();
                    CKEditor1.Text = ds.Tables[0].Rows[0][2].ToString().Trim();
                    DL_Status.SelectedValue = ds.Tables[0].Rows[0][3].ToString().Trim();
                }

                LoadMailSignInfoList(strUserCode);
            }
            catch
            {
            }
        }
    }


    protected void LLB_SignInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strHQL;


        strID = LLB_SignInfo.SelectedValue.Trim();

        strHQL = "Select ID,Title,SignInfo,Status From T_MailSignInfo Where UserCode = " + "'" + strUserCode + "'" + " and ID = " + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailSignInfo");

        if (ds.Tables[0].Rows.Count > 0)
        {
            LB_ID.Text = ds.Tables[0].Rows[0][0].ToString();
            TB_Title.Text = ds.Tables[0].Rows[0][1].ToString().Trim();
            CKEditor1.Text = ds.Tables[0].Rows[0][2].ToString().Trim();
            DL_Status.SelectedValue = ds.Tables[0].Rows[0][3].ToString().Trim();

            BT_New.Enabled = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strTitle, strContent, strStatus;

        strTitle = TB_Title.Text.Trim();
        strContent = CKEditor1.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();


        try
        {
            strHQL = "Insert Into T_MailSignInfo(UserCode,Title,SignInfo,Status) Values(" + "'" + strUserCode + "'" + "," + "'" + strTitle + "'" + "," + "'" + strContent + "'" + "," + "'" + strStatus + "'" + ")";
            ShareClass.RunSqlCommand(strHQL);

            LoadMailSignInfoList(strUserCode);

            LB_ID.Text = ShareClass.GetMyCreatedMaxMailSignInfoID(strUserCode);

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strTitle, strContent, strStatus;


        strID = LB_ID.Text.Trim();
        strTitle = TB_Title.Text.Trim();
        strContent = CKEditor1.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();


        try
        {
            strHQL = "Update T_MailSignInfo Set Title = " + "'" + strTitle + "'" + " Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_MailSignInfo Set SignInfo = " + "'" + strContent + "'" + " Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_MailSignInfo Set Status = " + "'" + strStatus + "'" + " Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadMailSignInfoList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID, strHQL;

        strID = LB_ID.Text.Trim();

        strHQL = "Delete From T_MailSignInfo Where ID = " + strID;
        ShareClass.RunSqlCommand(strHQL);

        try
        {
            BT_New.Enabled = true;
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            LB_ID.Text = "";

            LoadMailSignInfoList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }


    protected void LoadMailSignInfoList(string strUserCode)
    {
        string strHQL;

        strHQL = "Select ID,Title From T_MailSignInfo Where UserCode = " + "'" + strUserCode + "'" + " Order By ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MailSignInfo");

        LLB_SignInfo.DataSource = ds;
        LLB_SignInfo.DataBind();
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName.Trim();
        return strUserName.Trim();
    }



}
