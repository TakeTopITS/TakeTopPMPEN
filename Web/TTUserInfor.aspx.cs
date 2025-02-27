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
using System.Data.SqlClient;
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;
using System.Globalization;

public partial class TTUserInfor : System.Web.UI.Page
{
    string strCurrentUserType;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strOperatorCode, strOperatorName;
        string strDepartString = "";
        string strHQL;

        if (Session["UserCode"] != null)
        {
            strOperatorCode = Session["UserCode"].ToString();
            LB_UserCode.Text = strOperatorCode;
        }
        else
        {
            Session["UserCode"] = LB_UserCode.Text.Trim();
            strOperatorCode = LB_UserCode.Text.Trim();
        }

        strOperatorName = ShareClass.GetUserName(strOperatorCode);
        strCurrentUserType = ShareClass.GetUserType(strOperatorCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strOperatorCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工档案设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_BirthDay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_LunarBirthDay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EffectDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SchoolEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SchoolStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_WorkEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_WorkStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWorkType();
            LoadDepartPosition();
            LoadUserDuty();

            if (strOperatorCode == "ADMIN")
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3);

                strHQL = "Select DepartCode,DepartName From T_Department Order By DepartCode ASC";
            }
            else
            {
                string strSystemVersionType = Session["SystemVersionType"].ToString();
                if (strSystemVersionType == "GROUP")
                {
                    strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strOperatorCode);
                    TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strOperatorCode);
                    TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3, strOperatorCode);

                    LB_DepartString.Text = strDepartString;
                    strHQL = "Select DepartCode,DepartName From T_Department Where DepartCode in " + strDepartString;
                }
                else
                {
                    LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                    TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                    TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3);

                    strHQL = "Select DepartCode,DepartName From T_Department Order By DepartCode ASC";
                }
            }

            ShareClass.LoadSystemMDIStyle(DL_SystemMDIStyle);

            if (strCurrentUserType == "OUTER")
            {
                DL_UserType.SelectedValue = "OUTER";
                DL_UserType.Enabled = false;
            }

            ShareClass.LoadLanguageForDropList(ddlLangSwitcher);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        Session["UserCode"] = LB_UserCode.Text.Trim();

        string strDepartCode;
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target;

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'" + " Order By projectMember.SortNumber ASC";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_DepartCode.Text = strDepartCode;

            LB_Sql2.Text = strHQL;
        }

        Session["UserCode"] = LB_UserCode.Text.Trim();

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }

    //取得成员微信ID
    protected string getWXID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select COALESCE(WeChatOpenID,'') || ',' || COALESCE(WeChatUserID,'') || ',' || COALESCE(WeChatDeviceID,'') From T_ProjectMember Where UserCode = '" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        return ds.Tables[0].Rows[0][0].ToString();
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            TB_PartTimeDepartCode.Text = strDepartCode;
            LB_PartTimeDepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true','popPartTimeDetailWindow') ", true);
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;
        try
        {
            TreeNode treeNode = new TreeNode();
            treeNode = TreeView3.SelectedNode;

            if (treeNode.Target != "0")
            {
                strDepartCode = treeNode.Target.Trim();

                TB_DepartCode.Text = strDepartCode;
                LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
            }
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = e.Item.Cells[2].Text.Trim();

        for (int i = 0; i < DataGrid2.Items.Count; i++)
        {
            DataGrid2.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        TB_UserCode.Text = strUserCode;

        LoadPhotoList(strUserCode);
        LoadWorkExperienceList(strUserCode);
        LoadEducationExperienceList(strUserCode);
        LoadFamilyMemberList(strUserCode);
        LoadTransactionList(strUserCode);
        LoadPartTimeJobList(strUserCode);

        if (e.CommandName == "Update")
        {
            strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            ProjectMember projectMember = (ProjectMember)lst[0];

            try
            {
                LB_NewUserCode.Text = projectMember.UserCode.Trim();

                TB_UserCode.Text = projectMember.UserCode;
                TB_UserName.Text = projectMember.UserName;
                DL_Gender.SelectedValue = projectMember.Gender.Trim();
                NB_Age.Amount = projectMember.Age;

                TB_Duty.Text = projectMember.Duty;
                TB_JobTitle.Text = projectMember.JobTitle;

                TB_DepartCode.Text = projectMember.DepartCode;
                LB_DepartName.Text = projectMember.DepartName;

                TB_ChildDepartment.Text = projectMember.ChildDepartment.Trim();
                TB_OfficePhone.Text = projectMember.OfficePhone;
                TB_MobilePhone.Text = projectMember.MobilePhone;
                TB_EMail.Text = projectMember.EMail;
                TB_WorkScope.Text = projectMember.WorkScope;
                DLC_JoinDate.Text = projectMember.JoinDate.ToString("yyyy-MM-dd");
                DL_Status.SelectedValue = projectMember.Status.Trim();

                TB_RefUserCode.Text = projectMember.RefUserCode.Trim();
                NB_SortNumber.Amount = projectMember.SortNumber;

                TB_EnglishName.Text = projectMember.EnglishName.Trim();
                TB_Nationality.Text = projectMember.Nationality.Trim();
                TB_NativePlace.Text = projectMember.NativePlace.Trim();
                TB_HuKou.Text = projectMember.HuKou.Trim();
                TB_Residency.Text = projectMember.Residency.Trim();
                TB_Address.Text = projectMember.Address.Trim();
                DLC_BirthDay.Text = projectMember.BirthDay.ToString("yyyy-MM-dd");
                DLC_LunarBirthDay.Text = projectMember.LunarBirthDay.ToString("yyyy-MM-dd");
                DL_MaritalStatus.SelectedValue = projectMember.MaritalStatus.Trim();
                TB_Degree.Text = projectMember.Degree.Trim();
                TB_Major.Text = projectMember.Major.Trim();
                TB_GraduateSchool.Text = projectMember.GraduateSchool.Trim();
                TB_IDCard.Text = projectMember.IDCard.Trim();
                TB_BloodType.Text = projectMember.BloodType.Trim();
                NB_Height.Amount = projectMember.Height;

                try
                {
                    ddlLangSwitcher.SelectedValue = projectMember.LangCode.Trim();
                }
                catch
                {
                }
                TB_UrgencyPerson.Text = projectMember.UrgencyPerson.Trim();
                TB_UrgencyCall.Text = projectMember.UrgencyCall.Trim();
                TB_Introducer.Text = projectMember.Introducer.Trim();
                TB_IntroducerDepartment.Text = projectMember.IntroducerDepartment.Trim();
                TB_IntroducerRelation.Text = projectMember.IntroducerRelation.Trim();
                TB_Comment.Text = projectMember.Comment.Trim();

                DL_UserType.SelectedValue = projectMember.UserType.Trim();

                TB_UserRTXCode.Text = projectMember.UserRTXCode.Trim();

                DL_CssDirectory.SelectedValue = projectMember.CssDirectory.Trim();

                DL_SystemMDIStyle.SelectedValue = projectMember.MDIStyle.Trim();

                DL_WorkType.SelectedValue = projectMember.WorkType.Trim();

                DL_AllowDevice.SelectedValue = projectMember.AllowDevice.Trim();

                NB_HourlySalary.Amount = projectMember.HourlySalary;
                NB_MonthlySalary.Amount = projectMember.MonthlySalary;

                TXT_ContractEndTime.Text = projectMember.ContractEndTime;
                HF_ContractDocument.Value = projectMember.ContractDocument;
                HF_ContractDocumentURL.Value = projectMember.ContractDocumentURL;
                LT_ContractDocumentText.Text = "<a href='" + projectMember.ContractDocumentURL + "' class=\"notTab\" target=\"_blank\">" + projectMember.ContractDocument + "</a>";
            }
            catch (Exception ex)
            {
            }

            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            BT_UploadPhoto.Enabled = true;
            BT_UploadSignPicture.Enabled = true;
            BT_UploadSignPicture2.Enabled = true;
            BT_UploadSignPicture3.Enabled = true;

            BT_DeletePhoto.Enabled = true;

            BT_DeleteSignPicture.Enabled = true;
            BT_DeleteSignPicture2.Enabled = true;
            BT_DeleteSignPicture3.Enabled = true;

            BT_ContractDocument.Enabled = true;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }

        if (e.CommandName == "Photo")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
        }

        if (e.CommandName == "Work")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkWindow','true') ", true);
        }

        if (e.CommandName == "Education")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationWindow','true') ", true);
        }

        if (e.CommandName == "Family")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyWindow','true') ", true);
        }

        if (e.CommandName == "Change")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true') ", true);
        }

        if (e.CommandName == "PartTime")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true') ", true);
        }

        if (e.CommandName == "ClearWXID")
        {
            strHQL = "Update T_ProjectMember Set WeChatOpenID = '',WeChatUserID = '',WeChatDeviceID = '' Where UserCode = '" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZQingChuWanCheng").ToString().Trim() + "')", true);
        }

        if (e.CommandName == "Delete")
        {
            string strUser;
            string strUserName, strDepartCode;

            strUserName = ShareClass.GetUserName(strUserCode);
            strDepartCode = LB_DepartCode.Text.Trim();

            if (strUserCode == "ADMIN" | strUserCode == "SAMPLE")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGADMINHSAMPLESNZYHBNSCJC").ToString().Trim() + "')", true);
                return;
            }

            if (strUserCode != "ADMIN")
            {
                string strDepartString = LB_DepartString.Text.Trim();
                if (ShareClass.VerifyUserCode(strUserCode, strDepartString) == false | ShareClass.VerifyUserName(strUserName, strDepartString) == false)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNMYXCZCYHJC").ToString().Trim() + "')", true);
                    return;
                }
            }

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            ProjectMember projectMember = new ProjectMember();

            projectMember.UserCode = strUserCode;

            try
            {
                projectMemberBLL.DeleteProjectMember(projectMember);

                BT_UploadPhoto.Enabled = false;
                BT_UploadSignPicture.Enabled = false;
                BT_UploadSignPicture2.Enabled = false;
                BT_UploadSignPicture3.Enabled = false;

                BT_DeletePhoto.Enabled = false;

                BT_DeleteSignPicture.Enabled = false;
                BT_DeleteSignPicture2.Enabled = false;
                BT_DeleteSignPicture3.Enabled = false;

                BT_ContractDocument.Enabled = false;

                LoadProjectMember(strDepartCode);

                strHQL = "Delete From T_MailBoxAuthority Where UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_ProModule Where UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_MemberLevel where UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_SystemActiveUser Where UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_UserAttendanceRule Where UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                //删除RTX帐户
                try
                {
                    strUser = strUserCode + strUserName;
                    ShareClass.DeleteRTXUser(strUser);
                }
                catch
                {
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_CountChinaDate_Click(object sender, EventArgs e)
    {
        DateTime dsGLDate = DateTime.Parse(DLC_BirthDay.Text);

        DLC_LunarBirthDay.Text = ChinaDate.GetSunYearDate(dsGLDate).ToString("yyyy-MM-dd");

        NB_Age.Amount = DateTime.Now.Year - dsGLDate.Year;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_CountGLDate_Click(object sender, EventArgs e)
    {
        DateTime dsNLDate = DateTime.Parse(DLC_LunarBirthDay.Text);

        ChineseLunisolarCalendar cls = new ChineseLunisolarCalendar();

        DLC_BirthDay.Text = cls.ToDateTime(dsNLDate.Year, dsNLDate.Month, dsNLDate.Day, 0, 0, 0, 0).ToString("yyyy-MM-dd");

        NB_Age.Amount = DateTime.Now.Year - DateTime.Parse(DLC_BirthDay.Text).Year;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql2.Text;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void BT_UploadPhoto_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strUserCode = TB_UserCode.Text.Trim();
            string strHQL;

            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            int i;

            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;


                string strDocSavePath = Server.MapPath("Doc") + "\\UserPhoto\\";
                string strFileName3 = "Doc\\UserPhoto\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_File.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_ProjectMember Set PhotoURL = " + "'" + strFileName3 + "'" + " Where UserCode = " + "'" + strUserCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_MemberPhoto.ImageUrl = strFileName3;
                    HL_MemberPhoto.NavigateUrl = strFileName3;

                    ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
                }
                catch
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                // ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeletePhoto_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode = TB_UserCode.Text.Trim();

        try
        {
            strHQL = "Update T_ProjectMember Set PhotoURL = '' Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_MemberPhoto.ImageUrl = "";
            HL_MemberPhoto.NavigateUrl = "";
            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteSignPicture_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode = TB_UserCode.Text.Trim();

        try
        {
            strHQL = "Update T_ProjectMember Set SignPictureURL = '' Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_SignPicture.ImageUrl = "";
            HL_SignPicture.NavigateUrl = "";

            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteSignPicture2_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode = TB_UserCode.Text.Trim();

        try
        {
            strHQL = "Update T_ProjectMember Set SignPictureURL2 = '' Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_SignPicture2.ImageUrl = "";
            HL_SignPicture2.NavigateUrl = "";

            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteSignPicture3_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode = TB_UserCode.Text.Trim();

        try
        {
            strHQL = "Update T_ProjectMember Set SignPictureURL3 = '' Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_SignPicture3.ImageUrl = "";
            HL_SignPicture3.NavigateUrl = "";

            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UploadSignPicture_Click(object sender, EventArgs e)
    {
        if (this.FUP_Sign.PostedFile != null)
        {
            string strFileName1 = FUP_Sign.PostedFile.FileName.Trim();
            string strUserCode = TB_UserCode.Text.Trim();
            string strHQL;

            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            int i;

            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;


                string strDocSavePath = Server.MapPath("Doc") + "\\UserPhoto\\";
                string strFileName3 = "Doc\\UserPhoto\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_Sign.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_ProjectMember Set SignPictureURL = " + "'" + strFileName3 + "'" + " Where UserCode = " + "'" + strUserCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_SignPicture.ImageUrl = strFileName3;
                    HL_SignPicture.NavigateUrl = strFileName3;

                    ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
                }
                catch
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                // ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UploadSignPicture2_Click(object sender, EventArgs e)
    {
        if (this.FUP_Sign2.PostedFile != null)
        {
            string strFileName1 = FUP_Sign2.PostedFile.FileName.Trim();
            string strUserCode = TB_UserCode.Text.Trim();
            string strHQL;

            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            int i;

            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

                string strDocSavePath = Server.MapPath("Doc") + "\\UserPhoto\\";
                string strFileName3 = "Doc\\UserPhoto\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_Sign2.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_ProjectMember Set SignPictureURL2 = " + "'" + strFileName3 + "'" + " Where UserCode = " + "'" + strUserCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_SignPicture2.ImageUrl = strFileName3;
                    HL_SignPicture2.NavigateUrl = strFileName3;

                    ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
                }
                catch
                {
                    // ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UploadSignPicture3_Click(object sender, EventArgs e)
    {
        if (this.FUP_Sign3.PostedFile != null)
        {
            string strFileName1 = FUP_Sign3.PostedFile.FileName.Trim();
            string strUserCode = TB_UserCode.Text.Trim();
            string strHQL;

            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            int i;

            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

                string strDocSavePath = Server.MapPath("Doc") + "\\UserPhoto\\";
                string strFileName3 = "Doc\\UserPhoto\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUP_Sign3.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_ProjectMember Set SignPictureURL3 = " + "'" + strFileName3 + "'" + " Where UserCode = " + "'" + strUserCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_SignPicture3.ImageUrl = strFileName3;
                    HL_SignPicture3.NavigateUrl = strFileName3;

                    ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popPhotoWindow','true') ", true);
                }
                catch
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void BT_ContractDocument_Click(object sender, EventArgs e)
    {
        if (this.FUL_ContractDocument.PostedFile != null)
        {
            string strFileName1 = FUL_ContractDocument.PostedFile.FileName.Trim();
            string strUserCode = LB_UserCode.Text.Trim();
            string strHQL;

            string strLoginUserCode = Session["UserCode"].ToString().Trim();
            int i;

            if (strFileName1 != "")
            {
                //获取初始文件名
                i = strFileName1.LastIndexOf("."); //取得文件名中最后一个"."的索引
                string strNewExt = strFileName1.Substring(i); //获取文件扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间

                string strFileName2 = System.IO.Path.GetFileName(strFileName1);
                string strExtName = Path.GetExtension(strFileName2);
                strFileName2 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;


                string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\";
                string strFileName3 = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\" + strFileName2;
                string strFileName4 = strDocSavePath + strFileName2;

                FileInfo fi = new FileInfo(strFileName4);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                try
                {
                    FUL_ContractDocument.PostedFile.SaveAs(strFileName4);

                    strHQL = "Update T_ProjectMember Set ContractDocument = " + "'" + Path.GetFileNameWithoutExtension(strFileName2) + "'" + ",ContractDocumentURL='" + strFileName3 + "' Where UserCode = " + "'" + strUserCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    LT_ContractDocumentText.Text = "<a href='" + strFileName3 + "' class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_ContractDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_ContractDocumentURL.Value = strFileName3;

                    ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "pop", "popShow('popwindow','true') ", true);
                }
                catch (Exception ex)
                {
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString () + "')", true);
                }
            }
            else
            {
                // ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
            }
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
        }
    }

    protected void DDL_Duty_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strDuty;

        strDuty = DL_Duty.SelectedValue.Trim();

        TB_Duty.Text = strDuty;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DDL_PartTimeDuty_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strDuty;

        strDuty = DL_PartTimeDuty.SelectedValue.Trim();

        TB_PartTimeDuty.Text = strDuty;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true','popPartTimeDetailWindow') ", true);
    }

    protected void DDL_JobTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPosition;

        strPosition = DDL_JobTitle.SelectedValue.Trim();

        TB_JobTitle.Text = strPosition;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_NewUserCode.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = LB_NewUserCode.Text.Trim();

        if (strUserCode == "")
        {
            AddUser();
        }
        else
        {
            UpdateUser();
        }

    }

    protected void AddUser()
    {
        string strHQL;

        string strUser, strDepart;
        string strDepartCode = TB_DepartCode.Text.Trim();
        string strDepartName = LB_DepartName.Text.Trim();

        string strUserCode = TB_UserCode.Text.Trim().ToUpper();
        string strUserName = TB_UserName.Text.Trim();
        string strRefUsercode = TB_RefUserCode.Text.Trim();
        string strPassword = TB_Password.Text.Trim();
        string strDuty = TB_Duty.Text.Trim();
        string strMobilePhone = TB_MobilePhone.Text.Trim();
        string strEmail = TB_EMail.Text.Trim();
        string strCreatorCode = Session["UserCode"].ToString().Trim();

        string strChildDepartment = TB_ChildDepartment.Text.Trim();
        string strRefUserCode = TB_RefUserCode.Text.Trim();
        string strUserRTXCode = TB_UserRTXCode.Text.Trim();
        int intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        string strContractEndTime = TXT_ContractEndTime.Text.Trim();
        string strContractDocument = HF_ContractDocument.Value;
        string strContractDocumentURL = HF_ContractDocumentURL.Value;

        if (strPassword.Length < 8)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBMMCDBXDYHDY8WJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        if (strRefUsercode == "")
        {
            strRefUsercode = strUserCode;
            TB_RefUserCode.Text = strRefUsercode;
        }

        if (strUserCode.Length > 20 | strUserName.Length > 20)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYHDMHYHMCDBNCGSGHZ").ToString().Trim() + "')", true);
            return;
        }

        if (strUserRTXCode == "")
        {
            TB_UserRTXCode.Text = strUserCode + strUserName;
        }

        if (strPassword != "" & strUserCode != "" & strUserName != "" & strDuty != "" & strEmail != "" & strDepartCode != "")
        {
            if (ShareClass.SqlFilter(strUserCode) | ShareClass.SqlFilter(strPassword))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZHHYFFZHDLSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }

            ProjectMember projectMember = new ProjectMember();

            projectMember.UserCode = TB_UserCode.Text.Trim().ToUpper();
            projectMember.UserName = TB_UserName.Text.Trim();
            projectMember.Gender = DL_Gender.SelectedValue.Trim();
            projectMember.Age = int.Parse(NB_Age.Amount.ToString());
            projectMember.Password = EncryptPassword(TB_Password.Text.Trim(), "MD5");
            projectMember.PasswordShal = EncryptPasswordShal(TB_Password.Text.Trim());

            projectMember.RefUserCode = TB_RefUserCode.Text.Trim();
            projectMember.SortNumber = intSortNumber;

            projectMember.DepartCode = TB_DepartCode.Text.Trim();
            projectMember.DepartName = LB_DepartName.Text.Trim();
            projectMember.ChildDepartment = TB_ChildDepartment.Text.Trim();
            projectMember.Duty = TB_Duty.Text.Trim();
            projectMember.JobTitle = TB_JobTitle.Text.Trim();

            projectMember.OfficePhone = TB_OfficePhone.Text.Trim();
            projectMember.MobilePhone = TB_MobilePhone.Text.Trim();
            projectMember.EMail = TB_EMail.Text.Trim();
            projectMember.JoinDate = DateTime.Parse(DLC_JoinDate.Text);
            projectMember.WorkScope = TB_WorkScope.Text.Trim();
            projectMember.CreatorCode = strCreatorCode;
            projectMember.Status = DL_Status.SelectedValue.Trim();

            projectMember.EnglishName = TB_EnglishName.Text.Trim();
            projectMember.Nationality = TB_Nationality.Text.Trim();
            projectMember.NativePlace = TB_NativePlace.Text.Trim();
            projectMember.HuKou = TB_HuKou.Text.Trim();
            projectMember.Residency = TB_Residency.Text.Trim();
            projectMember.Address = TB_Address.Text.Trim();
            projectMember.BirthDay = DateTime.Parse(DLC_BirthDay.Text);
            projectMember.LunarBirthDay = DateTime.Parse(DLC_LunarBirthDay.Text);
            projectMember.MaritalStatus = DL_MaritalStatus.SelectedValue.Trim();
            projectMember.Degree = TB_Degree.Text.Trim();
            projectMember.Major = TB_Major.Text.Trim();
            projectMember.GraduateSchool = TB_GraduateSchool.Text.Trim();
            projectMember.IDCard = TB_IDCard.Text.Trim();
            projectMember.BloodType = TB_BloodType.Text.Trim();
            projectMember.Height = int.Parse(NB_Height.Amount.ToString());
            try
            {
                projectMember.LangCode = ddlLangSwitcher.SelectedValue.Trim();
                projectMember.Language = ddlLangSwitcher.SelectedItem.Text.Trim();
            }
            catch
            {
            }
            projectMember.UrgencyPerson = TB_UrgencyPerson.Text.Trim();
            projectMember.UrgencyCall = TB_UrgencyCall.Text.Trim();
            projectMember.Introducer = TB_Introducer.Text.Trim();
            projectMember.IntroducerDepartment = TB_IntroducerDepartment.Text.Trim();
            projectMember.IntroducerRelation = TB_IntroducerRelation.Text.Trim();
            projectMember.Comment = TB_Comment.Text.Trim();
            projectMember.PhotoURL = HL_MemberPhoto.NavigateUrl.Trim();

            projectMember.SignPictureURL = HL_SignPicture.NavigateUrl.Trim();
            projectMember.SignPictureURL2 = HL_SignPicture2.NavigateUrl.Trim();
            projectMember.SignPictureURL3 = HL_SignPicture3.NavigateUrl.Trim();

            projectMember.UserType = DL_UserType.SelectedValue.Trim();
            projectMember.UserRTXCode = TB_UserRTXCode.Text.Trim();
            projectMember.WorkType = DL_WorkType.SelectedValue.Trim();
            projectMember.CssDirectory = DL_CssDirectory.SelectedValue.Trim();
            projectMember.MDIStyle = DL_SystemMDIStyle.SelectedValue.Trim();
            projectMember.AllowDevice = DL_AllowDevice.SelectedValue.Trim();

            projectMember.ContractEndTime = strContractEndTime;
            projectMember.ContractDocument = strContractDocument;
            projectMember.ContractDocumentURL = strContractDocumentURL;

            projectMember.HourlySalary = NB_HourlySalary.Amount;
            projectMember.MonthlySalary = NB_MonthlySalary.Amount;

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            try
            {
                projectMemberBLL.AddProjectMember(projectMember);

                LB_NewUserCode.Text = TB_UserCode.Text.Trim().ToUpper();

                LoadProjectMember(LB_DepartCode.Text.Trim());
                LoadPartTimeJobList(strUserCode);

                BT_UploadPhoto.Enabled = true;

                BT_DeletePhoto.Enabled = true;

                BT_DeleteSignPicture.Enabled = true;
                BT_DeleteSignPicture2.Enabled = true;
                BT_DeleteSignPicture3.Enabled = true;

                strHQL = "insert T_MailBoxAuthority(UserCode,PasswordSet,DeleteOperate) Values (" + "'" + strUserCode + "'" + ",'YES','YES'" + ")";
                ShareClass.RunSqlCommand(strHQL);

                //增加用户到RTX
                try
                {
                    strUser = strUserCode + strUserName;
                    strDepart = strDepartCode + " " + strDepartName;

                    ShareClass.AddRTXUser(strUser, strDepart);
                }
                catch
                {
                }


                //给员工增加考勤规则
                try
                {
                    strHQL = "Insert Into T_UserAttendanceRule(UserCode,UserName,CreateDate,MCheckInStart,MCheckInEnd,MCheckOutStart,MCheckOutEnd,";
                    strHQL += "ACheckInStart,ACheckInEnd,ACheckOutStart,ACheckOutEnd,NCheckInStart,NCheckInEnd,NCheckOutStart,NCheckOutEnd,";
                    strHQL += "OCheckInStart,OCheckInEnd,OCheckOutStart,OCheckOutEnd,Status,MCheckInIsMust,MCheckOutIsMust,ACheckInIsMust,ACheckOutIsMust,NCheckInIsMust,NCheckOutIsMust,OCheckInIsMust,OCheckOutIsMust,LargestDistance,LeaderCode,LeaderName,OfficeLongitude,OfficeLatitude)";
                    strHQL += " Select A.UserCode,A.UserName,now(),B.MCheckInStart,B.MCheckInEnd,B.MCheckOutStart,B.MCheckOutEnd,";
                    strHQL += "B.ACheckInStart,B.ACheckInEnd,B.ACheckOutStart,B.ACheckOutEnd,B.NCheckInStart,B.NCheckInEnd,B.NCheckOutStart,B.NCheckOutEnd,";
                    strHQL += "B.OCheckInStart,B.OCheckInEnd,B.OCheckOutStart,B.OCheckOutEnd,'InProgress',B.MCheckInIsMust,B.MCheckOutIsMust,B.ACheckInIsMust,B.ACheckOutIsMust,B.NCheckInIsMust,B.NCheckOutIsMust,B.OCheckInIsMust,B.OCheckOutIsMust,B.LargestDistance,'','',OfficeLongitude,OfficeLatitude";
                    strHQL += " From T_ProjectMember A, T_AttendanceRule B";
                    strHQL += " Where A.UserCode = '" + strUserCode + "' and A.UserCode not in (Select UserCode From T_UserAttendanceRule) and A.Status not in ('Resign','Stop') ";
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {

                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJCDMZFHMXWK").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHDMYHMCMMZWEMAILDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateUser()
    {
        string strHQL;
        IList lst;

        string strUser, strDepart;
        string strDepartCode, strDepartName;
        string strUserCode, strUserName, strPassword, strDuty, strEmail;
        string strRefUserCode, strUserRTXCode;
        int intSortNumber;

        strUserCode = TB_UserCode.Text.Trim().ToUpper();
        strUserName = TB_UserName.Text.Trim();
        strDepartCode = TB_DepartCode.Text.Trim();
        strDepartName = LB_DepartName.Text.Trim();

        strPassword = TB_Password.Text.Trim();
        strDuty = TB_Duty.Text.Trim();
        strEmail = TB_EMail.Text.Trim();

        strRefUserCode = TB_RefUserCode.Text.Trim();
        strUserRTXCode = TB_UserRTXCode.Text.Trim();

        intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        string strDepartString = LB_DepartString.Text.Trim();
        if (ShareClass.VerifyUserCode(strUserCode, strDepartString) == false | ShareClass.VerifyUserName(strUserName, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNMYXCZCYHJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        if (strPassword != "")
        {
            if (strPassword.Length < 8)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBMMCDBXDYHDY8WJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }
        }

        if (strUserCode.Length > 20 | strUserName.Length > 20)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYHDMHYHMCDBNCGSGHZ").ToString().Trim() + "')", true);
            return;
        }

        if (strUserRTXCode == "")
        {
            TB_UserRTXCode.Text = strUserCode + strUserName;
        }

        if (strUserCode != "" & strUserName != "" & strDuty != "" & strEmail != "" & strDepartCode != "")
        {
            if (ShareClass.SqlFilter(strUserCode) | ShareClass.SqlFilter(strPassword))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZHHYFFZHDLSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }

            strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);

            ProjectMember projectMember = (ProjectMember)lst[0];

            projectMember.UserCode = TB_UserCode.Text.Trim();
            projectMember.UserName = TB_UserName.Text.Trim();

            projectMember.Gender = DL_Gender.SelectedValue.Trim();
            projectMember.Age = int.Parse(NB_Age.Amount.ToString());

            if (strPassword != "")
            {
                projectMember.Password = EncryptPassword(strPassword, "MD5");
                projectMember.PasswordShal = EncryptPasswordShal(strPassword);
            }

            projectMember.RefUserCode = strRefUserCode;
            projectMember.SortNumber = intSortNumber;

            projectMember.DepartCode = strDepartCode;
            projectMember.DepartName = strDepartName;
            projectMember.ChildDepartment = TB_ChildDepartment.Text.Trim();

            projectMember.Duty = TB_Duty.Text.Trim();
            projectMember.JobTitle = TB_JobTitle.Text.Trim();

            projectMember.OfficePhone = TB_OfficePhone.Text.Trim();
            projectMember.MobilePhone = TB_MobilePhone.Text.Trim();
            projectMember.EMail = TB_EMail.Text.Trim();
            projectMember.JoinDate = DateTime.Parse(DLC_JoinDate.Text);
            projectMember.WorkScope = TB_WorkScope.Text.Trim();
            projectMember.Status = DL_Status.SelectedValue.Trim();

            projectMember.EnglishName = TB_EnglishName.Text.Trim();
            projectMember.Nationality = TB_Nationality.Text.Trim();
            projectMember.NativePlace = TB_NativePlace.Text.Trim();
            projectMember.HuKou = TB_HuKou.Text.Trim();
            projectMember.Residency = TB_Residency.Text.Trim();
            projectMember.Address = TB_Address.Text.Trim();
            projectMember.BirthDay = DateTime.Parse(DLC_BirthDay.Text);
            projectMember.LunarBirthDay = DateTime.Parse(DLC_LunarBirthDay.Text);
            projectMember.MaritalStatus = DL_MaritalStatus.SelectedValue.Trim();
            projectMember.Degree = TB_Degree.Text.Trim();
            projectMember.Major = TB_Major.Text.Trim();
            projectMember.GraduateSchool = TB_GraduateSchool.Text.Trim();
            projectMember.IDCard = TB_IDCard.Text.Trim();
            projectMember.BloodType = TB_BloodType.Text.Trim();
            projectMember.Height = int.Parse(NB_Height.Amount.ToString());
            try
            {
                projectMember.LangCode = ddlLangSwitcher.SelectedValue.Trim();
                projectMember.Language = ddlLangSwitcher.SelectedItem.Text.Trim();
            }
            catch
            {
            }

            projectMember.UrgencyPerson = TB_UrgencyPerson.Text.Trim();
            projectMember.UrgencyCall = TB_UrgencyCall.Text.Trim();
            projectMember.Introducer = TB_Introducer.Text.Trim();
            projectMember.IntroducerDepartment = TB_IntroducerDepartment.Text.Trim();
            projectMember.IntroducerRelation = TB_IntroducerRelation.Text.Trim();

            projectMember.PhotoURL = HL_MemberPhoto.NavigateUrl.Trim();
            projectMember.SignPictureURL = HL_SignPicture.NavigateUrl.Trim();
            projectMember.SignPictureURL2 = HL_SignPicture2.NavigateUrl.Trim();
            projectMember.SignPictureURL3 = HL_SignPicture3.NavigateUrl.Trim();

            projectMember.UserType = DL_UserType.SelectedValue.Trim();
            projectMember.UserRTXCode = TB_UserRTXCode.Text.Trim();

            projectMember.Comment = TB_Comment.Text.Trim();

            projectMember.WorkType = DL_WorkType.SelectedValue.Trim();
            projectMember.CssDirectory = DL_CssDirectory.SelectedValue.Trim();
            projectMember.MDIStyle = DL_SystemMDIStyle.SelectedValue.Trim();
            projectMember.AllowDevice = DL_AllowDevice.SelectedValue.Trim();

            projectMember.ContractEndTime = TXT_ContractEndTime.Text.Trim();
            projectMember.ContractDocument = HF_ContractDocument.Value;
            projectMember.ContractDocumentURL = HF_ContractDocumentURL.Value;

            projectMember.HourlySalary = NB_HourlySalary.Amount;
            projectMember.MonthlySalary = NB_MonthlySalary.Amount;

            try
            {
                projectMemberBLL.UpdateProjectMember(projectMember, strUserCode);

                LoadProjectMember(LB_DepartCode.Text.Trim());

                //改变用户到RTX
                try
                {
                    strUser = strUserCode + strUserName;
                    strDepart = strDepartCode + " " + strDepartName;

                    ShareClass.DeleteRTXUser(strUser);
                    ShareClass.AddRTXUser(strUser, strDepart);
                }
                catch
                {
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);


            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJCDMZFHMXWK").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHDMYHMCZWEMAILDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartString = LB_DepartString.Text.Trim();

        string strUserCode = "%" + TB_UserCodeFind.Text.Trim() + "%";
        string strUserName = "%" + TB_UserNameFind.Text.Trim() + "%";

        if (strUserCode != "ADMIN")
        {
            strHQL = "from ProjectMember as projectMember where projectMember.UserCode Like " + "'" + strUserCode + "'" + " and projectMember.UserName Like " + "'" + strUserName + "'";
            strHQL += " and projectMember.DepartCode in " + strDepartString;
        }
        else
        {
            strHQL = "from ProjectMember as projectMember where projectMember.UserCode Like " + "'" + strUserCode + "'" + " and projectMember.UserName Like " + "'" + strUserName + "'";
        }

        strHQL += " Order By projectMember.SortNumber ASC";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql2.Text = strHQL;
    }

    protected void BT_FindWXID_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDepartString = LB_DepartString.Text.Trim();
        string strWXID = "%" + TB_WXID.Text + "%";

        if (strWXID != "%%")
        {
            strHQL = "Select * from T_ProjectMember where COALESCE(WeChatOpenID,'') +',' + COALESCE(WeChatUserID,'') + ',' + COALESCE(WeChatDeviceID,'')  Like " + "'" + strWXID + "'";
            strHQL += " and DepartCode in " + strDepartString;
            strHQL += " Order By SortNumber ASC";

            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSWXIDBNWKQJC").ToString().Trim() + "')", true);
        }
    }


    protected void BT_ClearAllMemberWXID_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strDepartString = LB_DepartString.Text.Trim();

        strHQL = "Update T_ProjectMember Set WeChatOpenID = '',WeChatUserID = '',WeChatDeviceID = '' Where DepartCode in " + strDepartString;
        ShareClass.RunSqlCommand(strHQL);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZQingChuWanCheng").ToString().Trim() + "')", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        ShareClass.ColorDataGridSelectRow(DataGrid1, e);

        string strEducationID = e.Item.Cells[2].Text;

        if (e.CommandName == "Update")
        {
            strHQL = "from EducationExperience as educationExperience where educationExperience.ID = " + strEducationID;
            EducationExperienceBLL educationExperienceBLL = new EducationExperienceBLL();
            lst = educationExperienceBLL.GetAllEducationExperiences(strHQL);

            EducationExperience educationExperience = (EducationExperience)lst[0];

            LB_EducationID.Text = educationExperience.ID.ToString();
            DLC_SchoolStartTime.Text = educationExperience.StartTime.ToString("yyyy-MM-dd");
            DLC_SchoolEndTime.Text = educationExperience.EndTime.ToString("yyyy-MM-dd");
            LB_EducationID.Text = educationExperience.ID.ToString();
            TB_School.Text = educationExperience.School.Trim();
            TB_SchoolMajor.Text = educationExperience.Major.Trim();
            TB_Certificate.Text = educationExperience.Certificate.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationWindow','true','popEducationDetailWindow') ", true);
        }

        if (e.CommandName == "Delete")
        {
            string strUserCode = TB_UserCode.Text.Trim();

            strHQL = "from EducationExperience as educationExperience where educationExperience.ID = " + strEducationID;
            EducationExperienceBLL educationExperienceBLL = new EducationExperienceBLL();
            lst = educationExperienceBLL.GetAllEducationExperiences(strHQL);

            EducationExperience educationExperience = (EducationExperience)lst[0];

            try
            {
                educationExperienceBLL.DeleteEducationExperience(educationExperience);


                LB_EducationID.Text = "";

                LoadEducationExperienceList(strUserCode);
            }
            catch
            {
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationWindow','true') ", true);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        ShareClass.ColorDataGridSelectRow(DataGrid3, e);

        string strWorkID = e.Item.Cells[2].Text;

        if (e.CommandName == "Update")
        {
            strHQL = "from WorkExperience as workExperience where workExperience.ID = " + strWorkID;
            WorkExperienceBLL workExperienceBLL = new WorkExperienceBLL();
            lst = workExperienceBLL.GetAllWorkExperiences(strHQL);

            WorkExperience workExperience = (WorkExperience)lst[0];

            LB_WorkID.Text = strWorkID;
            DLC_WorkStartTime.Text = workExperience.StartTime.ToString("yyyy-MM-dd");
            DLC_WorkEndTime.Text = workExperience.EndTime.ToString("yyyy-MM-dd");
            TB_WorkCompany.Text = workExperience.Company.Trim();
            TB_WorkDuty.Text = workExperience.Duty.Trim();
            NB_WorkSalary.Amount = workExperience.Salary;
            TB_WorkResignReason.Text = workExperience.ResignReason.Trim();
            TB_WorkRenterence.Text = workExperience.Renterence.Trim();
            TB_WorkRenterenceCall.Text = workExperience.RenterenceCall.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkWindow','true','popWorkDetailWindow') ", true);
        }

        if (e.CommandName == "Delete")
        {
            string strUserCode = TB_UserCode.Text.Trim();

            strHQL = "from WorkExperience as workExperience where workExperience.ID = " + strWorkID;
            WorkExperienceBLL workExperienceBLL = new WorkExperienceBLL();
            lst = workExperienceBLL.GetAllWorkExperiences(strHQL);
            WorkExperience workExperience = (WorkExperience)lst[0];

            workExperience.ID = int.Parse(strWorkID);

            try
            {
                workExperienceBLL.DeleteWorkExperience(workExperience);

                LB_WorkID.Text = "";

                LoadWorkExperienceList(strUserCode);
            }
            catch
            {
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkWindow','true') ", true);

        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        ShareClass.ColorDataGridSelectRow(DataGrid4, e);

        string strMemberID = e.Item.Cells[2].Text;

        if (e.CommandName == "Update")
        {
            strHQL = "from FamilyMember as familyMember where familyMember.ID = " + strMemberID;
            FamilyMemberBLL familyMemberBLL = new FamilyMemberBLL();
            lst = familyMemberBLL.GetAllFamilyMembers(strHQL);
            FamilyMember familyMember = (FamilyMember)lst[0];

            LB_MemberID.Text = familyMember.ID.ToString();
            TB_MemberName.Text = familyMember.MemberName.Trim();
            TB_Relation.Text = familyMember.Relation.Trim();
            TB_WorkAddress.Text = familyMember.WorkAddress.Trim();
            TB_MemberDuty.Text = familyMember.Duty.Trim();
            TB_PostCode.Text = familyMember.PostCode.Trim();
            TB_MemberComment.Text = familyMember.Comment.Trim();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyWindow','true','popFamilyDetailWindow') ", true);
        }

        if (e.CommandName == "Delete")
        {
            string strUserCode = TB_UserCode.Text.Trim();

            strHQL = "from FamilyMember as familyMember where familyMember.ID =" + strMemberID;
            FamilyMemberBLL familyMemberBLL = new FamilyMemberBLL();
            lst = familyMemberBLL.GetAllFamilyMembers(strHQL);

            FamilyMember familyMember = (FamilyMember)lst[0];

            try
            {
                familyMemberBLL.DeleteFamilyMember(familyMember);


                LB_MemberID.Text = "";

                LoadFamilyMemberList(strUserCode);
            }
            catch
            {
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyWindow','true') ", true);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        ShareClass.ColorDataGridSelectRow(DataGrid5, e);

        string strTransID = e.Item.Cells[2].Text;

        if (e.CommandName == "Update")
        {
            strHQL = "From UserTransactionRecord as userTransactionRecord Where userTransactionRecord.ID = " + strTransID;
            UserTransactionRecordBLL userTransactionRecordBLL = new UserTransactionRecordBLL();
            lst = userTransactionRecordBLL.GetAllUserTransactionRecords(strHQL);

            UserTransactionRecord userTransactionRecord = (UserTransactionRecord)lst[0];

            LB_TransID.Text = userTransactionRecord.ID.ToString();
            TB_TransType.Text = userTransactionRecord.TransType.Trim();
            TB_Description.Text = userTransactionRecord.Description.Trim();

            DLC_EffectDate.Text = userTransactionRecord.EffectDate.ToString("yyyy-MM-dd");

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true','popChangeDetailWindow') ", true);

        }

        if (e.CommandName == "Delete")
        {
            string strUserCode;
            strUserCode = TB_UserCode.Text.Trim();

            UserTransactionRecordBLL userTransactionRecordBLL = new UserTransactionRecordBLL();
            UserTransactionRecord userTransactionRecord = new UserTransactionRecord();

            userTransactionRecord.ID = int.Parse(strTransID);


            try
            {
                userTransactionRecordBLL.DeleteUserTransactionRecord(userTransactionRecord);

                LoadTransactionList(strUserCode);

                LB_TransID.Text = "";

                //BT_UpdateTransaction.Enabled = false;
                //BT_DeleteTransaction.Enabled = false;
            }
            catch
            {
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true') ", true);
        }
    }

    protected void DataGrid6_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        ShareClass.ColorDataGridSelectRow(DataGrid6, e);

        string strID = e.Item.Cells[2].Text;

        if (e.CommandName == "Update")
        {
            strHQL = "From PartTimeJob as partTimeJob Where partTimeJob.ID = " + strID;
            PartTimeJobBLL partTimeJobBLL = new PartTimeJobBLL();
            lst = partTimeJobBLL.GetAllPartTimeJobs(strHQL);

            PartTimeJob partTimeJob = (PartTimeJob)lst[0];

            LB_PartTimeID.Text = strID;
            TB_PartTimeDepartCode.Text = partTimeJob.DepartCode.Trim();
            LB_PartTimeDepartName.Text = partTimeJob.DepartName.Trim();
            TB_PartTimeDuty.Text = partTimeJob.Duty.Trim();
            DDC_PartTimeTime.Text = partTimeJob.EffectTime.ToString("yyyy-MM-dd");


            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true','popPartTimeDetailWindow') ", true);
        }

        if (e.CommandName == "Delete")
        {
            string strUserCode;

            strUserCode = TB_UserCode.Text.Trim();

            strHQL = "Delete From T_PartTimeJob Where ID = " + strID;

            try
            {
                ShareClass.RunSqlCommand(strHQL);

                LB_PartTimeID.Text = "";


                LoadPartTimeJobList(strUserCode);
            }
            catch
            {
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true') ", true);
        }
    }


    protected void BT_CreateWork_Click(object sender, EventArgs e)
    {
        LB_WorkID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkDetailWindow','false') ", true);
    }

    protected void BT_NewWork_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_WorkID.Text.Trim();

        if (strID == "")
        {
            AddWork();
        }
        else
        {
            UpdateWork();
        }
    }

    protected void AddWork()
    {
        string strUserCode, strWorkCompany, strWorkDuty, strWorkResignReason, strWorkRenterence, strWorkRenterenceCall;
        DateTime dtStartTime, dtEndTime;
        decimal deSalary;

        strUserCode = TB_UserCode.Text.Trim();
        strWorkCompany = TB_WorkCompany.Text.Trim();
        strWorkDuty = TB_WorkDuty.Text.Trim();
        strWorkResignReason = TB_WorkResignReason.Text.Trim();
        strWorkRenterence = TB_WorkRenterence.Text.Trim();
        strWorkRenterenceCall = TB_WorkRenterenceCall.Text.Trim();
        dtStartTime = DateTime.Parse(DLC_WorkStartTime.Text);
        dtEndTime = DateTime.Parse(DLC_WorkEndTime.Text);
        deSalary = NB_WorkSalary.Amount;

        WorkExperienceBLL workExperienceBLL = new WorkExperienceBLL();
        WorkExperience workExperience = new WorkExperience();

        workExperience.UserCode = strUserCode;
        workExperience.Company = strWorkCompany;
        workExperience.Duty = strWorkDuty;
        workExperience.ResignReason = strWorkResignReason;
        workExperience.Renterence = strWorkRenterence;
        workExperience.RenterenceCall = strWorkRenterenceCall;
        workExperience.StartTime = dtStartTime;
        workExperience.EndTime = dtEndTime;
        workExperience.Salary = deSalary;

        try
        {
            workExperienceBLL.AddWorkExperience(workExperience);
            LB_WorkID.Text = ShareClass.GetMyCreatedMaxWorkExperienceID(strUserCode);


            LoadWorkExperienceList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkWindow','true','popWorkDetailWindow') ", true);
        }
    }

    protected void UpdateWork()
    {
        string strHQL;
        IList lst;

        string strWorkID;

        string strUserCode, strWorkCompany, strWorkDuty, strWorkResignReason, strWorkRenterence, strWorkRenterenceCall;
        DateTime dtStartTime, dtEndTime;
        decimal deSalary;

        strWorkID = LB_WorkID.Text.Trim();
        strUserCode = TB_UserCode.Text.Trim();
        strWorkCompany = TB_WorkCompany.Text.Trim();
        strWorkDuty = TB_WorkDuty.Text.Trim();
        strWorkResignReason = TB_WorkResignReason.Text.Trim();
        strWorkRenterence = TB_WorkRenterence.Text.Trim();
        strWorkRenterenceCall = TB_WorkRenterenceCall.Text.Trim();
        dtStartTime = DateTime.Parse(DLC_WorkStartTime.Text);
        dtEndTime = DateTime.Parse(DLC_WorkEndTime.Text);
        deSalary = NB_WorkSalary.Amount;

        strHQL = "from WorkExperience as workExperience where workExperience.ID = " + strWorkID;
        WorkExperienceBLL workExperienceBLL = new WorkExperienceBLL();
        lst = workExperienceBLL.GetAllWorkExperiences(strHQL);
        WorkExperience workExperience = (WorkExperience)lst[0];

        workExperience.UserCode = strUserCode;
        workExperience.Company = strWorkCompany;
        workExperience.Duty = strWorkDuty;
        workExperience.ResignReason = strWorkResignReason;
        workExperience.Renterence = strWorkRenterence;
        workExperience.RenterenceCall = strWorkRenterenceCall;
        workExperience.StartTime = dtStartTime;
        workExperience.EndTime = dtEndTime;
        workExperience.Salary = deSalary;

        try
        {
            workExperienceBLL.UpdateWorkExperience(workExperience, int.Parse(strWorkID));

            LoadWorkExperienceList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popWorkWindow','true','popWorkDetailWindow') ", true);
        }
    }

    protected void BT_CreateEducation_Click(object sender, EventArgs e)
    {
        LB_EducationID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationDetailWindow','false') ", true);
    }

    protected void BT_NewEducation_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_EducationID.Text.Trim();

        if (strID == "")
        {
            AddEducation();
        }
        else
        {
            UpdateEducation();
        }
    }

    protected void AddEducation()
    {
        string strUserCode, strSchool, strMajor, strCertificate;
        DateTime dtStartTime, dtEndTime;

        strUserCode = TB_UserCode.Text.Trim();
        strSchool = TB_School.Text.Trim();
        strMajor = TB_SchoolMajor.Text.Trim();
        strCertificate = TB_Certificate.Text.Trim();
        dtStartTime = DateTime.Parse(DLC_SchoolStartTime.Text);
        dtEndTime = DateTime.Parse(DLC_SchoolEndTime.Text);

        EducationExperienceBLL educationExperienceBLL = new EducationExperienceBLL();
        EducationExperience educationExperience = new EducationExperience();

        educationExperience.UserCode = strUserCode;
        educationExperience.StartTime = dtStartTime;
        educationExperience.EndTime = dtEndTime;
        educationExperience.School = strSchool;
        educationExperience.Major = strMajor;
        educationExperience.Certificate = strCertificate;

        try
        {
            educationExperienceBLL.AddEducationExperience(educationExperience);
            LB_EducationID.Text = ShareClass.GetMyCreatedMaxEducationExpericenceID(strUserCode);

            LoadEducationExperienceList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationWindow','true','popEducationDetailWindow') ", true);
        }
    }

    protected void UpdateEducation()
    {
        string strHQL;
        IList lst;

        string strUserCode, strSchool, strMajor, strCertificate, strEducationID;
        DateTime dtStartTime, dtEndTime;

        strEducationID = LB_EducationID.Text.Trim();
        strUserCode = TB_UserCode.Text.Trim();
        strSchool = TB_School.Text.Trim();
        strMajor = TB_SchoolMajor.Text.Trim();
        strCertificate = TB_Certificate.Text.Trim();
        dtStartTime = DateTime.Parse(DLC_SchoolStartTime.Text);
        dtEndTime = DateTime.Parse(DLC_SchoolEndTime.Text);


        strHQL = "from EducationExperience as educationExperience where educationExperience.ID = " + strEducationID;
        EducationExperienceBLL educationExperienceBLL = new EducationExperienceBLL();
        lst = educationExperienceBLL.GetAllEducationExperiences(strHQL);

        EducationExperience educationExperience = (EducationExperience)lst[0];

        educationExperience.UserCode = strUserCode;
        educationExperience.StartTime = dtStartTime;
        educationExperience.EndTime = dtEndTime;
        educationExperience.School = strSchool;
        educationExperience.Major = strMajor;
        educationExperience.Certificate = strCertificate;

        try
        {
            educationExperienceBLL.UpdateEducationExperience(educationExperience, int.Parse(strEducationID));

            LoadEducationExperienceList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popEducationWindow','true','popEducationDetailWindow') ", true);
        }
    }

    protected void BT_CreateFamily_Click(object sender, EventArgs e)
    {
        LB_MemberID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyDetailWindow','false') ", true);
    }

    protected void BT_NewFamily_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_MemberID.Text.Trim();

        if (strID == "")
        {
            AddFamily();
        }
        else
        {
            UpdateFamily();
        }
    }

    protected void AddFamily()
    {
        string strUserCode, strMemberName, strRelation, strWorkAddress, strDuty, strPostCode, strComment;

        strUserCode = TB_UserCode.Text.Trim();
        strMemberName = TB_MemberName.Text.Trim();
        strRelation = TB_Relation.Text.Trim();
        strWorkAddress = TB_WorkAddress.Text.Trim();
        strDuty = TB_MemberDuty.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strComment = TB_Comment.Text.Trim();

        FamilyMemberBLL familyMemberBLL = new FamilyMemberBLL();
        FamilyMember familyMember = new FamilyMember();

        familyMember.UserCode = strUserCode;
        familyMember.MemberName = strMemberName;
        familyMember.Relation = strRelation;
        familyMember.WorkAddress = strWorkAddress;
        familyMember.PostCode = strPostCode;
        familyMember.Duty = strDuty;
        familyMember.Comment = strComment;

        try
        {
            familyMemberBLL.AddFamilyMember(familyMember);
            LB_MemberID.Text = ShareClass.GetMyCreatedMaxFamilyMemberID(strUserCode);

            LoadFamilyMemberList(strUserCode);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyWindow','true','popFamilyDetailWindow') ", true);
        }
    }

    protected void UpdateFamily()
    {
        string strHQL;
        IList lst;

        string strMemberID, strUserCode, strMemberName, strRelation, strWorkAddress, strDuty, strPostCode, strComment;

        strMemberID = LB_MemberID.Text.Trim();
        strUserCode = TB_UserCode.Text.Trim();
        strMemberName = TB_MemberName.Text.Trim();
        strRelation = TB_Relation.Text.Trim();
        strWorkAddress = TB_WorkAddress.Text.Trim();
        strDuty = TB_MemberDuty.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strComment = TB_Comment.Text.Trim();

        strHQL = "from FamilyMember as familyMember where familyMember.ID =" + strMemberID;
        FamilyMemberBLL familyMemberBLL = new FamilyMemberBLL();
        lst = familyMemberBLL.GetAllFamilyMembers(strHQL);

        FamilyMember familyMember = (FamilyMember)lst[0];

        familyMember.UserCode = strUserCode;
        familyMember.MemberName = strMemberName;
        familyMember.Relation = strRelation;
        familyMember.WorkAddress = strWorkAddress;
        familyMember.PostCode = strPostCode;
        familyMember.Duty = strDuty;
        familyMember.Comment = strComment;

        try
        {
            familyMemberBLL.UpdateFamilyMember(familyMember, int.Parse(strMemberID));

            LoadFamilyMemberList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFamilyWindow','true','popFamilyDetailWindow') ", true);
        }
    }


    protected void DL_TransType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_TransType.Text = DL_TransType.SelectedValue.Trim();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true','popChangeDetailWindow') ", true);
    }

    protected void BT_CreateChange_Click(object sender, EventArgs e)
    {
        LB_TransID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeDetailWindow','false') ", true);
    }

    protected void BT_NewChange_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_TransID.Text.Trim();

        if (strID == "")
        {
            AddChange();
        }
        else
        {
            UpdateChange();
        }
    }

    protected void AddChange()
    {
        string strUserCode;
        string strTransType, strDescription;
        DateTime dtTransDate;

        strUserCode = TB_UserCode.Text.Trim();

        strTransType = TB_TransType.Text.Trim();
        strDescription = TB_Description.Text.Trim();
        dtTransDate = DateTime.Parse(DLC_EffectDate.Text);

        UserTransactionRecordBLL userTransactionRecordBLL = new UserTransactionRecordBLL();
        UserTransactionRecord userTransactionRecord = new UserTransactionRecord();

        userTransactionRecord.UserCode = strUserCode;
        userTransactionRecord.TransType = strTransType;
        userTransactionRecord.Description = strDescription;
        userTransactionRecord.EffectDate = dtTransDate;

        try
        {
            userTransactionRecordBLL.AddUserTransactionRecord(userTransactionRecord);

            LoadTransactionList(strUserCode);

            LB_TransID.Text = ShareClass.GetMyCreatedMaxUserTransactionRecordID(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true','popChangeDetailWindow') ", true);
        }
    }

    protected void UpdateChange()
    {
        string strUserCode;
        string strID, strTransType, strDescription;
        DateTime dtTransDate;

        strUserCode = TB_UserCode.Text.Trim();

        strID = LB_TransID.Text.Trim();
        strTransType = TB_TransType.Text.Trim();
        strDescription = TB_Description.Text.Trim();
        dtTransDate = DateTime.Parse(DLC_EffectDate.Text);

        UserTransactionRecordBLL userTransactionRecordBLL = new UserTransactionRecordBLL();
        UserTransactionRecord userTransactionRecord = new UserTransactionRecord();

        userTransactionRecord.UserCode = strUserCode;
        userTransactionRecord.TransType = strTransType;
        userTransactionRecord.Description = strDescription;
        userTransactionRecord.EffectDate = dtTransDate;

        try
        {
            userTransactionRecordBLL.UpdateUserTransactionRecord(userTransactionRecord, int.Parse(strID));

            LoadTransactionList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popChangeWindow','true','popChangeDetailWindow') ", true);
        }
    }

    protected void BT_CreatePartTime_Click(object sender, EventArgs e)
    {
        LB_PartTimeID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeDetailWindow','false') ", true);
    }

    protected void BT_NewPartTime_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_PartTimeID.Text.Trim();

        if (strID == "")
        {
            AddPartTime();
        }
        else
        {
            UpdatePartTime();
        }
    }

    protected void AddPartTime()
    {
        string strID;
        string strDepartCode, strDepartName, strDuty, strUserCode, strUserName;
        DateTime dtEffectTime;

        strUserCode = TB_UserCode.Text.Trim();
        strUserName = TB_UserName.Text.Trim();

        strDepartCode = TB_PartTimeDepartCode.Text.Trim();
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        strDuty = TB_PartTimeDuty.Text.Trim();
        dtEffectTime = DateTime.Parse(DDC_PartTimeTime.Text);


        PartTimeJobBLL partTimeJobBLL = new PartTimeJobBLL();
        PartTimeJob partTimeJob = new PartTimeJob();

        partTimeJob.DepartCode = strDepartCode;
        partTimeJob.DepartName = strDepartName;
        partTimeJob.UserCode = strUserCode;
        partTimeJob.UserName = strUserName;
        partTimeJob.Duty = strDuty;
        partTimeJob.EffectTime = dtEffectTime;

        try
        {
            partTimeJobBLL.AddPartTimeJob(partTimeJob);

            strID = ShareClass.GetMyCreatedMaxPartTimeJobID();

            LB_PartTimeID.Text = strID;

            LoadPartTimeJobList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true','popPartTimeDetailWindow') ", true);
        }
    }

    protected void UpdatePartTime()
    {
        string strID;
        string strDepartCode, strDepartName, strDuty, strUserCode, strUserName;
        DateTime dtEffectTime;

        strDepartCode = TB_PartTimeDepartCode.Text.Trim();
        strDepartName = ShareClass.GetDepartName(strDepartCode);
        strUserCode = TB_UserCode.Text.Trim();
        strUserName = TB_UserName.Text.Trim();
        strDuty = TB_PartTimeDuty.Text.Trim();
        dtEffectTime = DateTime.Parse(DDC_PartTimeTime.Text);


        PartTimeJobBLL partTimeJobBLL = new PartTimeJobBLL();
        PartTimeJob partTimeJob = new PartTimeJob();

        strID = LB_PartTimeID.Text.Trim();
        partTimeJob.DepartCode = strDepartCode;
        partTimeJob.DepartName = strDepartName;
        partTimeJob.UserCode = strUserCode;
        partTimeJob.UserName = strUserName;
        partTimeJob.Duty = strDuty;
        partTimeJob.EffectTime = dtEffectTime;

        try
        {
            partTimeJobBLL.UpdatePartTimeJob(partTimeJob, int.Parse(strID));

            LoadPartTimeJobList(strUserCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popPartTimeWindow','true','popPartTimeDetailWindow') ", true);
        }
    }


    protected void BT_SavePhoto_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strUserPhotoString;

        strUserCode = TB_UserCode.Text.Trim();
        strUserPhotoString = TB_PhotoString1.Text.Trim();
        strUserPhotoString += TB_PhotoString2.Text.Trim();
        strUserPhotoString += TB_PhotoString3.Text.Trim();
        strUserPhotoString += TB_PhotoString4.Text.Trim();

        if (strUserPhotoString != "")
        {
            var binaryData = Convert.FromBase64String(strUserPhotoString);

            string strDateTime = DateTime.Now.ToString("yyyyMMddHHMMssff");
            string strUserPhotoURL = "Doc\\" + "UserPhoto\\" + strUserCode + strDateTime + ".jpg";
            var imageFilePath = Server.MapPath("Doc") + "\\UserPhoto\\" + strUserCode + strDateTime + ".jpg";

            if (File.Exists(imageFilePath))
            { File.Delete(imageFilePath); }
            var stream = new System.IO.FileStream(imageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Write(binaryData, 0, binaryData.Length);
            stream.Close();

            string strHQL = "Update T_ProjectMember Set PhotoURL = " + "'" + strUserPhotoURL + "'" + " Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_MemberPhoto.ImageUrl = GetUserPhotoURL(strUserCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void LoadWorkType()
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkType as workType Order by workType.SortNo ASC";
        WorkTypeBLL workTypeBLL = new WorkTypeBLL();
        lst = workTypeBLL.GetAllWorkType(strHQL);
        DL_WorkType.DataSource = lst;
        DL_WorkType.DataBind();
        DL_WorkType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadUserDuty()
    {
        string strHQL;
        IList lst;

        strHQL = "from UserDuty as userDuty Order By SortNumber ASC";
        UserDutyBLL userDutyBLL = new UserDutyBLL();
        lst = userDutyBLL.GetAllUserDutys(strHQL);

        DL_Duty.DataSource = lst;
        DL_Duty.DataBind();
        DL_Duty.Items.Insert(0, new ListItem("--Select--", ""));

        DL_PartTimeDuty.DataSource = lst;
        DL_PartTimeDuty.DataBind();
        DL_PartTimeDuty.Items.Insert(0, new ListItem("--Select--", ""));
    }


    protected void LoadDepartPosition()
    {
        string strHQL;
        IList lst;

        strHQL = "From DepartPosition as departPosition ";
        DepartPositionBLL departPositionBLL = new DepartPositionBLL();
        lst = departPositionBLL.GetAllDepartPositions(strHQL);

        DDL_JobTitle.DataSource = lst;
        DDL_JobTitle.DataBind();

        DDL_JobTitle.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadPhotoList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode= " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        IM_MemberPhoto.ImageUrl = projectMember.PhotoURL.Trim();
        HL_MemberPhoto.NavigateUrl = projectMember.PhotoURL.Trim();

        IM_SignPicture.ImageUrl = projectMember.SignPictureURL;
        HL_SignPicture.NavigateUrl = projectMember.SignPictureURL;

        IM_SignPicture2.ImageUrl = projectMember.SignPictureURL2;
        HL_SignPicture2.NavigateUrl = projectMember.SignPictureURL2;

        IM_SignPicture3.ImageUrl = projectMember.SignPictureURL3;
        HL_SignPicture3.NavigateUrl = projectMember.SignPictureURL3;

        BT_UploadPhoto.Enabled = true;
        BT_UploadSignPicture.Enabled = true;
        BT_UploadSignPicture2.Enabled = true;
        BT_UploadSignPicture3.Enabled = true;

        BT_DeletePhoto.Enabled = true;

        BT_DeleteSignPicture.Enabled = true;
        BT_DeleteSignPicture2.Enabled = true;
        BT_DeleteSignPicture3.Enabled = true;
    }

    protected void LoadPartTimeJobList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From PartTimeJob as partTimeJob Where partTimeJob.UserCode = " + "'" + strUserCode + "'" + " Order By partTimeJob.ID ASC";
        PartTimeJobBLL partTimeJobBLL = new PartTimeJobBLL();
        lst = partTimeJobBLL.GetAllPartTimeJobs(strHQL);

        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();
    }

    protected void LoadWorkExperienceList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkExperience as workExperience where workExperience.UserCode = " + "'" + strUserCode + "'";
        WorkExperienceBLL workExperienceBLL = new WorkExperienceBLL();
        lst = workExperienceBLL.GetAllWorkExperiences(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void LoadEducationExperienceList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from EducationExperience as educationExperience where educationExperience.UserCode = " + "'" + strUserCode + "'";
        EducationExperienceBLL educationExperienceBLL = new EducationExperienceBLL();
        lst = educationExperienceBLL.GetAllEducationExperiences(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadFamilyMemberList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from FamilyMember as familyMember where familyMember.UserCode = " + "'" + strUserCode + "'";
        FamilyMemberBLL familyMemberBLL = new FamilyMemberBLL();
        lst = familyMemberBLL.GetAllFamilyMembers(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadTransactionList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserTransactionRecord as userTransactionRecord Where userTransactionRecord.UserCode = " + "'" + strUserCode + "'" + " Order By userTransactionRecord.ID ASC";
        UserTransactionRecordBLL userTransactionRecordBLL = new UserTransactionRecordBLL();
        lst = userTransactionRecordBLL.GetAllUserTransactionRecords(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
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

    protected void LoadProjectMember(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);
        Department department = (Department)lst[0];

        return department.DepartName.Trim();
    }


    protected string GetUserPhotoURL(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        return projectMember.PhotoURL.Trim();
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
