using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using log4net.Util;

public partial class TTUpdateUserInfor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        if (Session["SystemVersionType"].ToString() == "SAAS")
        {
            Response.Redirect("TTUpdateUserInforSAAS.aspx");
        }


        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx");
        //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "我的档案", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            try
            {
                ShareClass.LoadLanguageForDropList(ddlLangSwitcher);

                LoadSystemMDIStyle();

                strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
                projectMemberBLL = new ProjectMemberBLL();
                projectMemberBLL = new ProjectMemberBLL();
                lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                ProjectMember projectMember = (ProjectMember)lst[0];

                TB_UserCode.Text = projectMember.UserCode.Trim();
                TB_UserName.Text = projectMember.UserName.Trim();
                LB_Gender.Text = projectMember.Gender.Trim();
                TB_Age.Text = projectMember.Age.ToString();

                TB_Duty.Text = projectMember.Duty;
                TB_JobTitle.Text = projectMember.JobTitle;

                if (projectMember.PhotoURL.Trim() != "")
                {
                    IM_MemberPhoto.ImageUrl = projectMember.PhotoURL.Trim();
                }
                else
                {
                    IM_MemberPhoto.ImageUrl = "Images/DefaultUserPhoto.png";
                }

                LB_Department.Text = projectMember.DepartName;
                TB_ChildDepartment.Text = projectMember.ChildDepartment;
                TB_OfficePhone.Text = projectMember.OfficePhone;
                TB_MobilePhone.Text = projectMember.MobilePhone;
                TB_EMail.Text = projectMember.EMail;
                TB_WorkScope.Text = projectMember.WorkScope;
                TB_JoinDate.Text = projectMember.JoinDate.ToString("yyyy-MM-dd");
                LB_Status.Text = projectMember.Status.Trim();

                LB_UserType.Text = projectMember.UserType.Trim();
                TB_RefUserCode.Text = projectMember.RefUserCode.Trim();
                TB_UserRTXCode.Text = projectMember.UserRTXCode.Trim();
                NB_SortNumber.Text = projectMember.SortNumber.ToString();

                try
                {
                    DL_WorkType.Text = projectMember.WorkType.Trim();
                }
                catch
                {

                }
                DL_CssDirectory.SelectedValue = projectMember.CssDirectory.Trim();
                DL_SystemMDIStyle.SelectedValue = projectMember.MDIStyle.Trim();
                DL_AllowDevice.SelectedValue = projectMember.AllowDevice.Trim();

                try
                {
                    ddlLangSwitcher.SelectedValue = projectMember.LangCode.Trim();
                }
                catch
                {
                    ddlLangSwitcher.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
                }

                try
                {
                    DL_LeftBarExtend.SelectedValue = ShareClass.GetLeftBarExtendStatus(strUserCode);
                }
                catch
                {
                    DL_LeftBarExtend.SelectedValue = "NO";
                }

                LoadMemberAgencyList(strUserCode);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strOldPassword = TB_OldPassword.Text.Trim();
        string strPassword = TB_Password.Text.Trim();
        string strStatus = LB_Status.Text;

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];


        if (strOldPassword == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click11", "alert('" + LanguageHandle.GetWord("ZZYuanMiMaChuWuHouWeiKong").ToString().Trim() + "')", true);
            return;
        }

        if (EncryptPassword(strOldPassword, "MD5") != projectMember.Password.Trim())
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZYuanMiMaChuWuHouWeiKong").ToString().Trim() + "')", true);
            return;
        }

        if (strPassword != "")
        {
            if (ShareClass.SqlFilter(strPassword))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZZHHYFFZHDLSB").ToString().Trim() + "')", true);
                return;
            }



            #region 判断输入的密码是否是字母与数字的结合，且长度要大于或等于8位 2013-09-03 By LiuJianping
            if (!IsPassword(strPassword))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click44", "alert('" + LanguageHandle.GetWord("ZZGGSBMMCDBXDYHDY8WBXBHSZJZMJC").ToString().Trim() + "')", true);
                TB_Password.Focus();
                return;
            }
            string strPasswordShal = EncryptPasswordShal(strPassword);
            strPassword = EncryptPassword(strPassword, "MD5");
            projectMember.Password = strPassword;
            #endregion
        }

        projectMember.Age = int.Parse(TB_Age.Amount.ToString());
        projectMember.OfficePhone = TB_OfficePhone.Text.Trim();
        projectMember.MobilePhone = TB_MobilePhone.Text.Trim();
        projectMember.EMail = TB_EMail.Text.Trim();
        projectMember.CssDirectory = DL_CssDirectory.SelectedValue.Trim();
        projectMember.MDIStyle = DL_SystemMDIStyle.SelectedValue.Trim();
        projectMember.LangCode = ddlLangSwitcher.SelectedValue.Trim();

        try
        {
            projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

            Session["CssDirectory"] = DL_CssDirectory.SelectedValue.Trim();

    
            ////设置缓存更改标志
            //ChangePageCache();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click55", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click66", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    //设置缓存更改标志，并刷新页面缓存
    protected void ChangePageCache()
    {
        //设置缓存更改标志
        ShareClass.SetPageCacheMark("1");
        Session["CssDirectoryChangeNumber"] = "1";

        ////给相关页面文件添加空行以刷新页面缓存
        //ShareClass.AddSpaceLineToFileForRefreshCache();
    }

    protected void DL_LeftBarExtend_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strUserCode;
        string strLeftBarExtend;


        strUserCode = Session["UserCode"].ToString();
        strLeftBarExtend = DL_LeftBarExtend.SelectedValue.Trim();

        try
        {
            //更新左边栏展开状态
            ShareClass.UpdateLeftBarExtendStatus(strUserCode, strLeftBarExtend);

            Session["LeftBarExtend"] = strLeftBarExtend;

            ShareClass.AddSpaceLineToFile("TakeTopLRExLeft.aspx", "<%--***--%>");
            ShareClass.AddSpaceLineToFile("TakeTopCSLRLeft.aspx", "<%--***--%>");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click55", "changeLeftBarExtend('" + strLeftBarExtend + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click66", "alert('" + LanguageHandle.GetWord("ZZGGSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void DL_MemberAgency_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strAgencyCode, strUserCode, strAgencyName;

        strAgencyCode = DL_MemberAgency.SelectedValue.Trim();
        strAgencyName = ShareClass.GetUserName(strAgencyCode);

        strUserCode = Session["UserCode"].ToString();

        strHQL = "Update T_MemberLevel Set AgencyStatus = 1 Where UserCode = " + "'" + strUserCode + "'" + " and UnderCode = " + "'" + strAgencyCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_MemberLevel Set AgencyStatus = 0 Where UserCode = " + "'" + strUserCode + "'" + " and UnderCode <> " + "'" + strAgencyCode + "'";
        ShareClass.RunSqlCommand(strHQL);

        if (strUserCode != strAgencyCode)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click66", "alert('" + LanguageHandle.GetWord("ZZYSZSTRAGENCYNAMEWNDGZLSPDLRXZXSWNZJJK").ToString().Trim() + "')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click66", "alert('" + LanguageHandle.GetWord("ZZYXGZLSPDL").ToString().Trim() + "')", true);
        }
    }

    protected void LoadMemberAgencyList(string strUserCode)
    {
        string strHQL;

        strHQL = "select A.UnderCode as UserCode,B.UserName as UserName From T_MemberLevel A,T_ProjectMember B Where A.UnderCode = B.UserCode and A.UserCode = " + "'" + strUserCode + "'";
        strHQL += " Order by A.AgencyStatus DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");

        DL_MemberAgency.DataSource = ds;
        DL_MemberAgency.DataBind();
    }

    protected void LoadSystemMDIStyle()
    {
        string strHQL = "from SystemMDIStyle as systemMDIStyle Order By systemMDIStyle.SortNumber ASC";

        SystemMDIStyleBLL systemMDIStyleBLL = new SystemMDIStyleBLL();
        IList lst = systemMDIStyleBLL.GetAllSystemMDIStyles(strHQL);

        DL_SystemMDIStyle.DataSource = lst;
        DL_SystemMDIStyle.DataBind();
    }

    /// <summary>
    /// 判断输入的密码是否是字母与数字的结合 By LiuJianping 2013-09-03
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private bool IsPassword(string str)
    {
        //字母，数字，非字母字符(例如 !、$、#、%) ^(?:(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])|(?=.*[A-Z])(?=.*[a-z])(?=.*[^A-Za-z0-9])|(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])|(?=.*[a-z])(?=.*[0-9])(?=.*[^A-Za-z0-9])).{8,}
        System.Text.RegularExpressions.Regex reg
            = new System.Text.RegularExpressions.Regex("^(?:(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])|(?=.*[A-Z])(?=.*[0-9])|(?=.*[a-z])(?=.*[0-9])).{8,}");
        return reg.IsMatch(str);
    }

    protected string EncryptPassword(string strPassword, string strFormat)
    {
        string strNewPassword;

        if (strFormat == "SHA1")
        {
            strNewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "SHA1");
        }
        else
        {
            strNewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "MD5");
        }

        return strNewPassword;
    }


    /// <summary>
    ///  散列加密
    /// </summary>
    protected string EncryptPasswordShal(string strPlaintext)
    {
        byte[] srcBuffer = System.Text.Encoding.UTF8.GetBytes(strPlaintext);

        System.Security.Cryptography.HashAlgorithm hash = System.Security.Cryptography.HashAlgorithm.Create("SHA1"); //将参数换成“MD5”，则执行 MD5 加密。不区分大小写。
        byte[] destBuffer = hash.ComputeHash(srcBuffer);

        string strHashedText = BitConverter.ToString(destBuffer).Replace("-", "");
        return strHashedText.ToLower();
    }
}
