using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

using TakeTopSecurity;

public partial class TTEarlyWarningOrderSet : System.Web.UI.Page
{
    private string strLangCode;
    private string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strLangCode = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "预警命令设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            TB_InforName.Enabled = true;
            TB_LinkAddress.Enabled = true;
            TB_SQLCode.Enabled = true;
            ddl_BoxType.Enabled = true;
            DL_Status.Enabled = true;

            LB_SuperDepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);

            LoadFunInforDialBoxList(strLangCode);
        }
    }

    protected void BT_AllStartup_Click(object sender, EventArgs e)
    {
        try
        {
            //向所有系统用户推送预警消息
            Msg msg = new Msg();
            msg.StartNotificationToSystemActiveUser();

            //插入推送日志
            string strHQL = "Insert Into T_MsgPushLog(PushTime,UserCode,UserName) Values(now()," + "'" + strUserCode + "'" + "," + "'" + strUserName + "'" + ")";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYJXXBTSWC") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYJXXTSSBJC") + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.ID = '" + strId + "'";
            FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
            lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
            FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];

            LB_ID.Text = funInforDialBox.ID.ToString();
            DL_Status.SelectedValue = funInforDialBox.Status.Trim();
            TB_SQLCode.Text = funInforDialBox.SQLCode.Trim();
            TB_InforName.Text = funInforDialBox.InforName.Trim();
            ddl_BoxType.SelectedValue = funInforDialBox.BoxType.Trim();
            DL_UserType.SelectedValue = funInforDialBox.UserType.Trim();
            DL_IsForceInfor.SelectedValue = funInforDialBox.IsForceInfor.Trim();
            TB_LinkAddress.Text = funInforDialBox.LinkAddress.Trim();
            TB_MobileLinkAddress.Text = funInforDialBox.MobileLinkAddress.Trim();

            if (funInforDialBox.IsSendMsg.Trim() == "YES")
                CB_Msg.Checked = true;
            else
                CB_Msg.Checked = false;

            if (funInforDialBox.IsSendEmail.Trim() == "YES")
                CB_Email.Checked = true;
            else
                CB_Email.Checked = false;

            if (funInforDialBox.BoxType.Trim() == "SYS")
            {
                TB_InforName.Enabled = false;
                TB_SQLCode.Enabled = false;
            }
            else
            {
                TB_InforName.Enabled = true;
                TB_SQLCode.Enabled = true;
            }

            TB_SortNumber.Text = funInforDialBox.SortNumber.ToString();

            TB_InforName.Enabled = true;
            TB_SQLCode.Enabled = true;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void BT_TestCode_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strSuperDepartString;
        strSuperDepartString = LB_SuperDepartString.Text.Trim();

        strHQL = TB_SQLCode.Text.Trim().Replace("[TAKETOPUSERCODE]", strUserCode);
        strHQL = strHQL.Replace("[TAKETOPSUPERDEPARTSTRING]", strSuperDepartString);

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDMZ") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDMYCJC") + "')", true);
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(TB_InforName.Text.Trim()) && TB_InforName.Text.Trim().Contains("@"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTSGNBNBHZFJC") + "')", true);
            TB_InforName.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TB_SQLCode.Text.Trim()) || TB_SQLCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLYJBNWKJC") + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (!(TB_SQLCode.Text.Trim().ToLower().Contains("select") && TB_SQLCode.Text.Trim().ToLower().Contains("from") && TB_SQLCode.Text.Trim().ToUpper().Contains("[TAKETOPUSERCODE]")))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC") + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (TB_SQLCode.Text.Trim().ToLower().Contains("create ") || TB_SQLCode.Text.Trim().ToLower().Contains("execute ") || TB_SQLCode.Text.Trim().ToLower().Contains("delete ") || TB_SQLCode.Text.Trim().ToLower().Contains("update") || TB_SQLCode.Text.Trim().ToLower().Contains("drop ")
            || TB_SQLCode.Text.Trim().ToLower().Contains("insert ") || TB_SQLCode.Text.Trim().ToLower().Contains("alter "))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC") + "')", true);
            TB_SQLCode.Focus();
            return;
        }

        if (ddl_BoxType.SelectedValue.Trim() == "SYS")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNBNZJLXWXTDYJJC") + "')", true);
            return;
        }

        FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
        FunInforDialBox funInforDialBox = new FunInforDialBox();

        funInforDialBox.Status = DL_Status.SelectedValue.Trim();
        funInforDialBox.SQLCode = TB_SQLCode.Text.Trim();
        funInforDialBox.InforName = TB_InforName.Text.Trim();
        funInforDialBox.HomeName = TB_InforName.Text.Trim();
        funInforDialBox.LangCode = strLangCode;
        funInforDialBox.CreateTime = DateTime.Now;
        funInforDialBox.BoxType = ddl_BoxType.SelectedValue.Trim();
        funInforDialBox.UserType = DL_UserType.SelectedValue.Trim();
        funInforDialBox.IsForceInfor = DL_IsForceInfor.SelectedValue.Trim();
        funInforDialBox.LinkAddress = TB_LinkAddress.Text.Trim();
        funInforDialBox.MobileLinkAddress = TB_MobileLinkAddress.Text.Trim();

        if (CB_Msg.Checked)
            funInforDialBox.IsSendMsg = "YES";
        else
            funInforDialBox.IsSendMsg = "NO";
        if (CB_Email.Checked)
            funInforDialBox.IsSendEmail = "YES";
        else
            funInforDialBox.IsSendEmail = "NO";

        try
        {
            funInforDialBox.SortNumber = int.Parse(TB_SortNumber.Text.Trim());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSRZSXZWSXH") + "')", true);
            return;
        }

        try
        {
            funInforDialBoxBLL.AddFunInforDialBox(funInforDialBox);
            LB_ID.Text = GetFunInforDialBoxID();

            LoadFunInforDialBoxList(strLangCode);

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            TB_InforName.Enabled = true;
            TB_LinkAddress.Enabled = true;
            TB_SQLCode.Enabled = true;
            DL_Status.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(TB_InforName.Text.Trim()) && TB_InforName.Text.Trim().Contains("@"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGTSGNBNBHZFJC") + "')", true);
            TB_InforName.Focus();
            return;
        }
        if (string.IsNullOrEmpty(TB_SQLCode.Text.Trim()) || TB_SQLCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLYJBNWKJC") + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (!(TB_SQLCode.Text.Trim().ToLower().Contains("select") && TB_SQLCode.Text.Trim().ToLower().Contains("from") && TB_SQLCode.Text.Trim().ToUpper().Contains("[TAKETOPUSERCODE]")))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC") + "')", true);
            TB_SQLCode.Focus();
            return;
        }
        if (TB_SQLCode.Text.Trim().ToLower().Contains("create ") || TB_SQLCode.Text.Trim().ToLower().Contains("execute ") || TB_SQLCode.Text.Trim().ToLower().Contains("delete ") || TB_SQLCode.Text.Trim().ToLower().Contains("update ") || TB_SQLCode.Text.Trim().ToLower().Contains("drop ")
            || TB_SQLCode.Text.Trim().ToLower().Contains("insert ") || TB_SQLCode.Text.Trim().ToLower().Contains("alter "))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCSQLYJYWJC") + "')", true);
            TB_SQLCode.Focus();
            return;
        }

        string strHQL1;
        IList lst1;
        int intID;
        FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
        FunInforDialBox funInforDialBox;

        strHQL1 = "From FunInforDialBox as funInforDialBox where funInforDialBox.InforName = '" + TB_InforName.Text.Trim() + "'";
        lst1 = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL1);
        if (lst1.Count > 0)
        {
            try
            {
                for (int i = 0; i < lst1.Count; i++)
                {
                    funInforDialBox = (FunInforDialBox)lst1[i];

                    intID = funInforDialBox.ID;


                    funInforDialBox.SQLCode = TB_SQLCode.Text.Trim();
                    funInforDialBox.InforName = TB_InforName.Text.Trim();


                    funInforDialBox.LinkAddress = TB_LinkAddress.Text.Trim();
                    funInforDialBox.MobileLinkAddress = TB_MobileLinkAddress.Text.Trim();
                    funInforDialBox.UserType = DL_UserType.SelectedValue.Trim();
                    funInforDialBox.IsForceInfor = DL_IsForceInfor.SelectedValue.Trim();
                    funInforDialBox.Status = DL_Status.SelectedValue.Trim();

                    if (CB_Msg.Checked)
                        funInforDialBox.IsSendMsg = "YES";
                    else
                        funInforDialBox.IsSendMsg = "NO";
                    if (CB_Email.Checked)
                        funInforDialBox.IsSendEmail = "YES";
                    else
                        funInforDialBox.IsSendEmail = "NO";

                    try
                    {
                        funInforDialBox.SortNumber = int.Parse(TB_SortNumber.Text.Trim());
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSRZSXZWSXH") + "')", true);
                        return;
                    }

                    funInforDialBoxBLL.UpdateFunInforDialBox(funInforDialBox, intID);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
            }

            LoadFunInforDialBoxList(strLangCode);
            BT_Delete.Enabled = true;
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strId = LB_ID.Text.Trim();

        strHQL = "From FunInforDialBox as funInforDialBox where funInforDialBox.ID = '" + LB_ID.Text.Trim() + "'";
        FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
        IList lst = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL);
        FunInforDialBox funInforDialBox = (FunInforDialBox)lst[0];
        if (funInforDialBox.BoxType.Trim() == "SYS")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNBNSCXTLXDYJJC") + "')", true);
            return;
        }

        strHQL = "Delete From T_FunInforDialBox Where InforName = '" + TB_InforName.Text.Trim() + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadFunInforDialBoxList(strLangCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void LoadFunInforDialBoxList(string strLangCode)
    {
        string strHQL = "Select * From T_FunInforDialBox  Where LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order By SortNumber ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_FunInforDialBox");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    /// <summary>
    /// 获取当前信息的ID编号
    /// </summary>
    /// <returns></returns>
    protected string GetFunInforDialBoxID()
    {
        string strHQL = " Select ID From T_FunInforDialBox Order by ID Desc ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_FunInforDialBox").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["ID"].ToString();
        }
        else
        {
            return "0";
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_FunInforDialBox");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}