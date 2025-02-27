using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using TakeTopSecurity;

public partial class TTAllCustomerUsers : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strDepartString;

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode.Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadWorkType();
            LoadDepartPosition();
            LoadSystemMDIStyle();

            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);
            strHQL = "Select RTRIM(DepartCode) DepartCode,RTRIM(DepartName) DepartName from T_Department ";
            strHQL += " Where DepartCode in " + LB_DepartString.Text;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");
            DL_Department.DataSource = ds;
            DL_Department.DataBind();

            LoadCustomerOperationRecord(strUserCode.Trim());

            TakeTopCore.CoreShareClass.InitialUnderDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode.Trim());
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            string strDepartCode = treeNode.Target;

            DL_Department.SelectedValue = strDepartCode.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);
            LB_DepartCode.Text = strDepartCode;
        }


    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string struserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + struserCode + "'";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst != null && lst.Count > 0)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            TB_UserCode.Text = projectMember.UserCode.Trim();
            lbl_UserCode.Text = projectMember.UserCode.Trim();
            TB_UserName.Text = projectMember.UserName.Trim();
            DL_Gender.SelectedValue = projectMember.Gender.Trim();
            TB_Age.Amount = projectMember.Age;
            TB_Password.Text = "";

            TB_Duty.Text = projectMember.Duty.Trim();
            TB_JobTitle.Text = projectMember.JobTitle;

            DL_Department.SelectedValue = projectMember.DepartCode.Trim();
            TB_ChildDepartment.Text = projectMember.ChildDepartment.Trim();
            TB_OfficePhone.Text = projectMember.OfficePhone.Trim();
            TB_MobilePhone.Text = projectMember.MobilePhone.Trim();
            TB_EMail.Text = projectMember.EMail.Trim();
            DL_Status.SelectedValue = projectMember.Status.Trim();
            TB_WorkScope.Text = projectMember.WorkScope.Trim();
            DLC_JoinDate.Text = projectMember.JoinDate.ToString("yyyy-MM-dd");
            TB_RefUserCode.Text = projectMember.RefUserCode.Trim();

            DL_UserType.SelectedValue = projectMember.UserType.Trim();
            DL_WorkType.SelectedValue = projectMember.WorkType.Trim();

            DL_SystemMDIStyle.SelectedValue = projectMember.MDIStyle.Trim();
            DL_AllowDevice.SelectedValue = projectMember.AllowDevice.Trim();
            TB_UserRTXCode.Text = projectMember.UserRTXCode.Trim();
            NB_SortNumber.Amount = projectMember.SortNumber;
            IM_MemberPhoto.ImageUrl = string.IsNullOrEmpty(projectMember.PhotoURL) ? "" : projectMember.PhotoURL.Trim();
            if (!string.IsNullOrEmpty(projectMember.PhotoURL) && projectMember.PhotoURL.Trim() != "")
            {
                HL_MemberPhoto.Visible = true;
                HL_MemberPhoto.NavigateUrl = projectMember.PhotoURL.Trim();
                BT_DeletePhoto.Enabled = true;
            }
            else
            {
                HL_MemberPhoto.Visible = false;
                BT_DeletePhoto.Enabled = false;
            }

            BT_UploadPhoto.Enabled = true;
            BT_TakePhoto.Enabled = true;

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
        else
        {
            TB_UserCode.Text = "";
            TB_UserName.Text = "";
            DL_Gender.SelectedValue = "Male";
            TB_Age.Amount = 0;
            TB_Password.Text = "";

            TB_Duty.Text = "";
            TB_JobTitle.Text = "";

            TB_ChildDepartment.Text = "";
            TB_OfficePhone.Text = "";
            TB_MobilePhone.Text = "";
            TB_EMail.Text = "";
            TB_WorkScope.Text = "";
            DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_RefUserCode.Text = "";

            DL_UserType.SelectedValue = "OUTER";
            DL_WorkType.SelectedValue = "";

            DL_SystemMDIStyle.SelectedValue = "左右下展"; 
            DL_AllowDevice.SelectedValue = "ALL";
            TB_UserRTXCode.Text = "";
            NB_SortNumber.Amount = 0;
            IM_MemberPhoto.ImageUrl = "";
            HL_MemberPhoto.Visible = false;
            BT_UploadPhoto.Enabled = false;
            BT_DeletePhoto.Enabled = false;
            BT_TakePhoto.Enabled = false;
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strUserCode = TB_UserCode.Text.Trim();
        string strUserName = TB_UserName.Text.Trim();

        string strHQL;
        IList lst;

        if (strUserCode == "" & strUserName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZSBXSRYHDMHMCZYCNC").ToString().Trim() + "')", true);
            return;
        }

        string strDepartString = LB_DepartString.Text.Trim();
        if (ShareClass.VerifyUserCode(strUserCode, strDepartString) == false | ShareClass.VerifyUserName(strUserName, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNMYXCZCYHJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'" + " or projectMember.UserName = " + "'" + strUserName + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            TB_Password.Text = "";
            TB_ChildDepartment.Text = projectMember.ChildDepartment.Trim();
            TB_UserCode.Text = projectMember.UserCode;
            TB_UserName.Text = projectMember.UserName;
            DL_Gender.SelectedValue = projectMember.Gender.Trim();
            TB_Age.Amount = projectMember.Age;

            TB_Duty.Text = projectMember.Duty;
            TB_JobTitle.Text = projectMember.JobTitle;

            DL_Department.SelectedValue = projectMember.DepartCode;
            TB_OfficePhone.Text = projectMember.OfficePhone;
            TB_MobilePhone.Text = projectMember.MobilePhone;
            TB_EMail.Text = projectMember.EMail;
            TB_WorkScope.Text = projectMember.WorkScope;
            DLC_JoinDate.Text = projectMember.JoinDate.ToString("yyyy-MM-dd");
            DL_Status.SelectedValue = projectMember.Status.Trim();

            TB_RefUserCode.Text = projectMember.RefUserCode.Trim();
            TB_UserRTXCode.Text = projectMember.UserRTXCode.Trim();

            DL_UserType.SelectedValue = projectMember.UserType.Trim();
            DL_WorkType.SelectedValue = projectMember.WorkType.Trim();

            IM_MemberPhoto.ImageUrl = string.IsNullOrEmpty(projectMember.PhotoURL) ? "" : projectMember.PhotoURL.Trim();
            if (!string.IsNullOrEmpty(projectMember.PhotoURL) && projectMember.PhotoURL.Trim() != "")
            {
                HL_MemberPhoto.Visible = true;
                HL_MemberPhoto.NavigateUrl = projectMember.PhotoURL.Trim();
                BT_DeletePhoto.Enabled = true;
            }
            else
            {
                HL_MemberPhoto.Visible = false;
                BT_DeletePhoto.Enabled = false;
            }
            NB_SortNumber.Amount = projectMember.SortNumber;

            DL_SystemMDIStyle.SelectedValue = projectMember.MDIStyle.Trim();
            DL_AllowDevice.SelectedValue = projectMember.AllowDevice.Trim();

            BT_UploadPhoto.Enabled = true;
            BT_TakePhoto.Enabled = true;
        }
        else
        {
            TB_UserCode.Text = "";
            TB_UserName.Text = "";
            DL_Gender.SelectedValue = "Male";
            TB_Age.Amount = 0;
            TB_Password.Text = "";

            TB_Duty.Text = "";
            TB_JobTitle.Text = "";

            TB_ChildDepartment.Text = "";
            TB_OfficePhone.Text = "";
            TB_MobilePhone.Text = "";
            TB_EMail.Text = "";
            TB_WorkScope.Text = "";
            DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_RefUserCode.Text = "";

            DL_UserType.SelectedValue = "OUTER";
            DL_WorkType.SelectedValue = "";

            DL_SystemMDIStyle.SelectedValue = "左右下展"; 
            DL_AllowDevice.SelectedValue = "ALL";
            TB_UserRTXCode.Text = "";
            NB_SortNumber.Amount = 0;
            IM_MemberPhoto.ImageUrl = "";
            HL_MemberPhoto.Visible = false;
            BT_UploadPhoto.Enabled = false;
            BT_DeletePhoto.Enabled = false;
            BT_TakePhoto.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMYCYHJCYHDMHMCSFZ").ToString().Trim() + "')", true);
        }
    }

    protected void DDL_JobTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPosition;

        strPosition = DDL_JobTitle.SelectedValue.Trim();

        TB_JobTitle.Text = strPosition;
    }


    protected void BT_TakePhoto_Click(object sender, EventArgs e)
    {
        Panel2.Visible = true;
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
    }

    protected string GetUserPhotoURL(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        return projectMember.PhotoURL.Trim();
    }

    protected void BT_DeletePhoto_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strusercode = TB_UserCode.Text.Trim();

        try
        {
            strHQL = "Update T_ProjectMember Set PhotoURL = '' Where UserCode = " + "'" + strusercode + "'";
            ShareClass.RunSqlCommand(strHQL);

            AddCustomerOperationRecord(strusercode, LanguageHandle.GetWord("ShanChuTuPianXinXi").ToString().Trim());

            IM_MemberPhoto.ImageUrl = "";
            HL_MemberPhoto.NavigateUrl = "";
            HL_MemberPhoto.Visible = false;

            LoadCustomerOperationRecord(strUserCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
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

                string strDocSavePath = Server.MapPath("Doc") + "\\" + strLoginUserCode + "\\Images\\";
                string strFileName3 = "Doc\\" + strLoginUserCode + "\\Images\\" + strFileName2;
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

                    AddCustomerOperationRecord(strUserCode, LanguageHandle.GetWord("GengXinTuPianXinXi").ToString().Trim());

                    IM_MemberPhoto.ImageUrl = strFileName3;
                    HL_MemberPhoto.NavigateUrl = strFileName3;
                    HL_MemberPhoto.Visible = true;

                    LoadCustomerOperationRecord(strLoginUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCHCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDTP").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDTP").ToString().Trim() + "')", true);
        }
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strusercode = TB_UserCode.Text.Trim();
        string strUserName = TB_UserName.Text.Trim();
        string strPassword = TB_Password.Text.Trim();
        string strDuty = TB_Duty.Text.Trim();
        string strEmail = TB_EMail.Text.Trim();
        string strCreatorCode = Session["UserCode"].ToString().Trim();
        string strDepartCode = DL_Department.SelectedValue.Trim();
        string strDepartName = DL_Department.SelectedItem.Text.Trim();
        string strChildDepartment = TB_ChildDepartment.Text.Trim();
        string strRefUserCode = TB_RefUserCode.Text.Trim();
        string strUserRTXCode = TB_UserRTXCode.Text.Trim();
        string strMDIStyle = DL_SystemMDIStyle.SelectedValue.Trim();
        string strAllowDevice = DL_AllowDevice.SelectedValue.Trim();

        int intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        if (strRefUserCode == "")
        {
            strRefUserCode = strusercode;
            TB_RefUserCode.Text = strRefUserCode;
        }

        if (strUserRTXCode == "")
        {
            strUserRTXCode = strusercode + strUserName;
        }

        if (strusercode == "" || strUserName == "" || strDuty == "" || strEmail == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHDMYHMCZWEMAILDBNWKJC").ToString().Trim() + "')", true);
            TB_UserCode.Focus();
            TB_UserName.Focus();
            TB_Duty.Focus();
            TB_EMail.Focus();
            return;
        }
        if (LB_DepartCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZZCZBMXXJC").ToString().Trim() + "')", true);
            return;
        }
        ProjectMember pj1 = ProjectMemberData(TB_UserCode.Text.Trim().ToUpper());
        if (pj1 != null)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGYHDMYCZJC").ToString().Trim() + "')", true);
            TB_UserCode.Focus();
            return;
        }
        else
        {
            int intLicenseNumber, intUserNumber;
            string strLicenseType;

            SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
            SystemActiveUser systemActiveUser = new SystemActiveUser();

            string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];

            TakeTopLicense license = new TakeTopLicense();
            intLicenseNumber = license.GetWEBLicenseNumber(strServerName);
            intUserNumber = GetSystemUserNumber();
            strLicenseType = license.GetLicenseType(strServerName);
            if (strLicenseType == "REGISTER")
            {
                if (intUserNumber >= intLicenseNumber)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXTYHSYJDYHDYNDSLICENSEGDDSMBNZXZXDYHL").ToString().Trim() + "')", true);
                    return;
                }
            }

            if (strPassword == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMMBNWKJC").ToString().Trim() + "')", true);
                return;
            }
            if (strPassword.Length < 8)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZHKTSBMMCDBXDYHDY8WJC").ToString().Trim() + "')", true);
                return;
            }
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            ProjectMember projectMember1 = new ProjectMember();

            projectMember1.UserCode = TB_UserCode.Text.Trim().ToUpper();
            projectMember1.UserName = TB_UserName.Text.Trim();
            projectMember1.Gender = DL_Gender.SelectedValue.Trim();
            projectMember1.Age = int.Parse(TB_Age.Amount.ToString());
            projectMember1.Password = EncryptPassword(TB_Password.Text.Trim(), "MD5");
            projectMember1.RefUserCode = TB_RefUserCode.Text.Trim();
            projectMember1.SortNumber = intSortNumber;
            projectMember1.DepartCode = strDepartCode;
            projectMember1.DepartName = strDepartName;
            projectMember1.ChildDepartment = TB_ChildDepartment.Text.Trim();

            projectMember1.Duty = TB_Duty.Text.Trim();
            projectMember1.JobTitle = TB_JobTitle.Text.Trim();

            projectMember1.OfficePhone = TB_OfficePhone.Text.Trim();
            projectMember1.MobilePhone = TB_MobilePhone.Text.Trim();
            projectMember1.EMail = TB_EMail.Text.Trim();
            projectMember1.JoinDate = DLC_JoinDate.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_JoinDate.Text);
            projectMember1.WorkScope = TB_WorkScope.Text.Trim();
            projectMember1.Status = DL_Status.SelectedValue.Trim();
            projectMember1.PhotoURL = HL_MemberPhoto.NavigateUrl.Trim();

            projectMember1.UserType = DL_UserType.SelectedValue.Trim();
            projectMember1.WorkType = DL_WorkType.SelectedValue.Trim();

            projectMember1.UserRTXCode = strUserRTXCode;
            projectMember1.MDIStyle = strMDIStyle;
            projectMember1.AllowDevice = strAllowDevice;
            projectMember1.CreatorCode = strCreatorCode;
            projectMember1.EnglishName = "";
            projectMember1.Nationality = "";
            projectMember1.NativePlace = "";
            projectMember1.HuKou = "";
            projectMember1.Residency = "";
            projectMember1.Address = "";
            projectMember1.BirthDay = DateTime.Now;
            projectMember1.MaritalStatus = "Unmarried";
            projectMember1.Degree = "";
            projectMember1.Major = "";
            projectMember1.GraduateSchool = "";
            projectMember1.IDCard = "";
            projectMember1.BloodType = "";
            projectMember1.Height = 0;
            projectMember1.Language = "";
            projectMember1.UrgencyPerson = "";
            projectMember1.UrgencyCall = "";
            projectMember1.Introducer = "";
            projectMember1.IntroducerDepartment = "";
            projectMember1.IntroducerRelation = "";
            projectMember1.Comment = "";

            try
            {
                projectMemberBLL.AddProjectMember(projectMember1);

                lbl_UserCode.Text = projectMember1.UserCode.Trim();
                TB_UserRTXCode.Text = projectMember1.UserRTXCode.Trim();

                AddCustomerOperationRecord(projectMember1.UserCode.Trim(), LanguageHandle.GetWord("XinZengYongHuXinXi").ToString().Trim());

                systemActiveUser.UserCode = strusercode;
                systemActiveUser.UserName = strUserName;
                systemActiveUser.JoinTime = DateTime.Now;
                systemActiveUser.OperatorCode = strCreatorCode;

                systemActiveUserBLL.AddSystemActiveUser(systemActiveUser);

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                BT_UploadPhoto.Enabled = true;
                BT_TakePhoto.Enabled = true;
                BT_DeletePhoto.Enabled = true;

                LoadCustomerOperationRecord(strUserCode.Trim());

                ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJCDMZFHMXWK").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strusercode = TB_UserCode.Text.Trim();
        string strusercodeold = lbl_UserCode.Text.Trim();
        string strUserName = TB_UserName.Text.Trim();
        string strPassword = TB_Password.Text.Trim();
        string strDuty = TB_Duty.Text.Trim();
        string strEmail = TB_EMail.Text.Trim();
        string strCreatorCode = Session["UserCode"].ToString().Trim();
        string strChildDepartment = TB_ChildDepartment.Text.Trim();
        string strRefUserCode = TB_RefUserCode.Text.Trim();
        string strUserRTXCode = TB_UserRTXCode.Text.Trim();
        string strMDIStyle = DL_SystemMDIStyle.SelectedValue.Trim();
        string strAllowDevice = DL_AllowDevice.SelectedValue.Trim();

        if (strusercode != strusercodeold)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXSBDMBNGGJC").ToString().Trim() + "')", true);
            TB_UserCode.Focus();
            return;
        }

        string strDepartString = LB_DepartString.Text.Trim();
        if (ShareClass.VerifyUserCode(strUserCode, strDepartString) == false | ShareClass.VerifyUserName(strUserName, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNMYXCZCYHJC").ToString().Trim() + "')", true);
            return;
        }

        int intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        if (strRefUserCode == "")
        {
            strRefUserCode = strusercode;
            TB_RefUserCode.Text = strRefUserCode;
        }

        if (strUserRTXCode == "")
        {
            strUserRTXCode = strusercode + strUserName;
        }

        if (strusercode != "" & strUserName != "" & strDuty != "" & strEmail != "")
        {
            string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strusercode + "' ";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            if (lst != null && lst.Count > 0)
            {
                ProjectMember projectMember1 = (ProjectMember)lst[0];
                if (strPassword != "")
                {
                    if (strPassword.Length < 8)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXSBMMCDBXDYHDY8WJC").ToString().Trim() + "')", true);
                        TB_Password.Focus();
                        return;
                    }
                    projectMember1.Password = EncryptPassword(TB_Password.Text.Trim(), "MD5");
                }
                projectMember1.UserName = TB_UserName.Text.Trim();
                projectMember1.Gender = DL_Gender.SelectedValue.Trim();
                projectMember1.Age = int.Parse(TB_Age.Amount.ToString());
                projectMember1.RefUserCode = TB_RefUserCode.Text.Trim();
                projectMember1.SortNumber = intSortNumber;
                projectMember1.DepartCode = DL_Department.SelectedValue.Trim();
                projectMember1.DepartName = DL_Department.SelectedItem.Text.Trim();
                projectMember1.ChildDepartment = TB_ChildDepartment.Text.Trim();

                projectMember1.Duty = TB_Duty.Text.Trim();
                projectMember1.JobTitle = TB_JobTitle.Text.Trim();

                projectMember1.OfficePhone = TB_OfficePhone.Text.Trim();
                projectMember1.MobilePhone = TB_MobilePhone.Text.Trim();
                projectMember1.EMail = TB_EMail.Text.Trim();
                projectMember1.JoinDate = DLC_JoinDate.Text.Trim() == "" ? DateTime.Now : DateTime.Parse(DLC_JoinDate.Text);
                projectMember1.WorkScope = TB_WorkScope.Text.Trim();
                projectMember1.Status = DL_Status.SelectedValue.Trim();
                projectMember1.PhotoURL = HL_MemberPhoto.NavigateUrl.Trim();

                projectMember1.UserType = DL_UserType.SelectedValue.Trim();
                projectMember1.WorkType = DL_WorkType.SelectedValue.Trim();

                projectMember1.UserRTXCode = strUserRTXCode;
                projectMember1.MDIStyle = strMDIStyle;
                projectMember1.AllowDevice = strAllowDevice;

                try
                {
                    projectMemberBLL.UpdateProjectMember(projectMember1, projectMember1.UserCode.Trim());
                    TB_UserRTXCode.Text = projectMember1.UserRTXCode.Trim();

                    AddCustomerOperationRecord(projectMember1.UserCode.Trim(), LanguageHandle.GetWord("GengXinSuoYouXinXi").ToString().Trim());

                    BT_UploadPhoto.Enabled = true;
                    BT_TakePhoto.Enabled = true;
                    BT_DeletePhoto.Enabled = true;

                    LoadCustomerOperationRecord(strUserCode.Trim());

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXSBJCDMZFHMXWK").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYHDMYHMCZWEMAILDBNWKJC").ToString().Trim() + "')", true);
        }
    }


    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUser;
        string strUserCode, strUserName, strDepartCode;

        strUserCode = TB_UserCode.Text.Trim();
        strUserName = ShareClass.GetUserName(strUserCode);
        strDepartCode = LB_DepartCode.Text.Trim();

        if (strUserCode == "ADMIN" | strUserCode == "SAMPLE")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGADMINHSAMPLESNZYHBNSCJC").ToString().Trim() + "')", true);
            return;
        }

        string strDepartString = LB_DepartString.Text.Trim();
        if (ShareClass.VerifyUserCode(strUserCode, strDepartString) == false | ShareClass.VerifyUserName(strUserName, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGNMYXCZCYHJC").ToString().Trim() + "')", true);
            return;
        }

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        ProjectMember projectMember = new ProjectMember();

        projectMember.UserCode = strUserCode;

        try
        {
            projectMemberBLL.DeleteProjectMember(projectMember);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            BT_UploadPhoto.Enabled = false;
            BT_DeletePhoto.Enabled = false;
            BT_TakePhoto.Enabled = false;

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

            BT_TakePhoto.Enabled = false;

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

    protected ProjectMember ProjectMemberData(string strusercode)
    {
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strusercode + "' ";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
            return (ProjectMember)lst[0];
        else
            return null;
    }

    protected int GetSystemUserNumber()
    {
        string strHQL;
        IList lst;

        strHQL = "From SystemActiveUser as systemActiveUser";
        SystemActiveUserBLL systemAcitveUserBLL = new SystemActiveUserBLL();
        lst = systemAcitveUserBLL.GetAllSystemActiveUsers(strHQL);

        return lst.Count;
    }

    protected int GetUserCount(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        return lst.Count;
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadCustomerOperationRecord(strUserCode.Trim());
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerOperationRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void AddCustomerOperationRecord(string strusercode, string strRemark)
    {
        CustomerOperationRecordBLL customerOperationRecordBLL = new CustomerOperationRecordBLL();
        CustomerOperationRecord customerOperationRecord = new CustomerOperationRecord();
        customerOperationRecord.Creater = strUserCode.Trim();
        customerOperationRecord.CreaterName = ShareClass.GetUserName(strUserCode.Trim());
        customerOperationRecord.CreateTime = DateTime.Now;
        customerOperationRecord.UserCode = strusercode.Trim();
        customerOperationRecord.UserName = ShareClass.GetUserName(strusercode.Trim());
        customerOperationRecord.Remark = strRemark;

        customerOperationRecordBLL.AddCustomerOperationRecord(customerOperationRecord);
    }

    protected void LoadCustomerOperationRecord(string strusercode)
    {
        string strHQL = "Select * From T_CustomerOperationRecord Where Creater='" + strusercode + "' ";
        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and (UserCode like '%" + txt_SupplierInfo.Text.Trim() + "%' or UserName like '%" + txt_SupplierInfo.Text.Trim() + "%' or " +
                "Creater like '%" + txt_SupplierInfo.Text.Trim() + "%' or CreaterName like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order by ID";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerOperationRecord");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadSystemMDIStyle()
    {
        string strHQL = "from SystemMDIStyle as systemMDIStyle Order By systemMDIStyle.SortNumber ASC";

        SystemMDIStyleBLL systemMDIStyleBLL = new SystemMDIStyleBLL();
        IList lst = systemMDIStyleBLL.GetAllSystemMDIStyles(strHQL);

        DL_SystemMDIStyle.DataSource = lst;
        DL_SystemMDIStyle.DataBind();
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

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        string strUser, strDepart, strNewUserCode;
        string strErrorUserCodeString = "";

        if (ExcelToDBTest() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim();
            return;
        }
        else
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
                return;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;
                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        strNewUserCode = dr[i][LanguageHandle.GetWord("ChengYuanDaiMa").ToString().Trim()].ToString().Trim();
                        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strNewUserCode + "' ";
                        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                        if (lst != null && lst.Count > 0)//存在，则不操作
                        {
                        }
                        else//新增
                        {
                            ProjectMember projectMember = new ProjectMember();

                            try
                            {
                                projectMember.UserCode = strNewUserCode;
                                projectMember.UserName = dr[i][LanguageHandle.GetWord("ChengYuanXingMing").ToString().Trim()].ToString().Trim();
                                projectMember.Gender = dr[i][LanguageHandle.GetWord("XingBie").ToString().Trim()].ToString().Trim() == "" ? "Male" : dr[i][LanguageHandle.GetWord("XingBie").ToString().Trim()].ToString().Trim();
                                projectMember.Age = int.Parse(dr[i][LanguageHandle.GetWord("NianLing").ToString().Trim()].ToString().Trim() == "" ? "0" : dr[i][LanguageHandle.GetWord("NianLing").ToString().Trim()].ToString().Trim());
                                projectMember.Password = dr[i][LanguageHandle.GetWord("MiMa").ToString().Trim()].ToString().Trim() == "" ? EncryptPassword("12345678", "MD5") : EncryptPassword(dr[i][LanguageHandle.GetWord("MiMa").ToString().Trim()].ToString().Trim(), "MD5");
                                projectMember.RefUserCode = dr[i][LanguageHandle.GetWord("CanKaoGongHao").ToString().Trim()].ToString().Trim() == "" ? strNewUserCode : dr[i][LanguageHandle.GetWord("CanKaoGongHao").ToString().Trim()].ToString().Trim();
                                projectMember.SortNumber = int.Parse(dr[i][LanguageHandle.GetWord("ShunXuHao").ToString().Trim()].ToString().Trim() == "" ? "0" : dr[i][LanguageHandle.GetWord("ShunXuHao").ToString().Trim()].ToString().Trim());

                                projectMember.DepartCode = dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim() == "" ? ShareClass.GetDepartCodeFromUserCode(strUserCode.Trim()) : dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim();
                                projectMember.DepartName = dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim() == "" ? ShareClass.GetDepartName(projectMember.DepartCode.Trim()) : dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim();

                                projectMember.ChildDepartment = dr[i][LanguageHandle.GetWord("ZiBuMen").ToString().Trim()].ToString().Trim();
                                projectMember.Duty = dr[i][LanguageHandle.GetWord("ZhiWu").ToString().Trim()].ToString().Trim();
                                projectMember.OfficePhone = dr[i][LanguageHandle.GetWord("BanGongDianHua").ToString().Trim()].ToString().Trim();
                                projectMember.MobilePhone = dr[i][LanguageHandle.GetWord("ShouJi").ToString().Trim()].ToString().Trim();
                                projectMember.EMail = dr[i]["E_Mail"].ToString().Trim();
                                projectMember.JoinDate = DateTime.Parse(dr[i]["加入日期"].ToString().Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : dr[i]["加入日期"].ToString().Trim()); 
                                projectMember.WorkScope = dr[i][LanguageHandle.GetWord("GongZuoFanWei").ToString().Trim()].ToString().Trim();
                                projectMember.Status = dr[i]["状态"].ToString().Trim() == "" ? "Employed" : dr[i]["状态"].ToString().Trim(); 
                                projectMember.PhotoURL = "";

                                projectMember.JobTitle = TB_JobTitle.Text.Trim();
                                projectMember.UserType = DL_UserType.SelectedValue.Trim();
                                projectMember.WorkType = DL_WorkType.SelectedValue.Trim();

                                projectMember.UserRTXCode = dr[i][LanguageHandle.GetWord("RTXZhangHu").ToString().Trim()].ToString().Trim() == "" ? strNewUserCode + projectMember.UserName.Trim() : dr[i][LanguageHandle.GetWord("RTXZhangHu").ToString().Trim()].ToString().Trim();
                                projectMember.MDIStyle = DL_SystemMDIStyle.SelectedValue.Trim();
                                projectMember.AllowDevice = DL_AllowDevice.SelectedValue.Trim();
                                projectMember.CreatorCode = strUserCode.Trim();

                                projectMember.CssDirectory = "CssRed";
                                projectMember.EnglishName = "";
                                projectMember.Nationality = "";
                                projectMember.NativePlace = "";
                                projectMember.HuKou = "";
                                projectMember.Residency = "";
                                projectMember.Address = "";
                                projectMember.BirthDay = DateTime.Now;
                                projectMember.MaritalStatus = "Unmarried";
                                projectMember.Degree = "";
                                projectMember.Major = "";
                                projectMember.GraduateSchool = "";
                                projectMember.IDCard = "";
                                projectMember.BloodType = "";
                                projectMember.Height = 0;
                                projectMember.Language = "";
                                projectMember.UrgencyPerson = "";
                                projectMember.UrgencyCall = "";
                                projectMember.Introducer = "";
                                projectMember.IntroducerDepartment = "";
                                projectMember.IntroducerRelation = "";
                                projectMember.Comment = "";

                                projectMemberBLL.AddProjectMember(projectMember);
                                AddCustomerOperationRecord(projectMember.UserCode.Trim(), LanguageHandle.GetWord("XinZengYongHuXinXi").ToString().Trim());
                            }
                            catch (Exception err)
                            {
                                strErrorUserCodeString += strNewUserCode + ",";

                                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 1).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strNewUserCode + " : " + err.Message.ToString() +"<br/>";
                            }

                            //增加用户到RTX
                            try
                            {
                                strUser = dr[i][LanguageHandle.GetWord("CanKaoGongHao").ToString().Trim()].ToString().Trim() + dr[i][LanguageHandle.GetWord("ChengYuanXingMing").ToString().Trim()].ToString().Trim();
                                strDepart = dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim() + " " + dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim();

                                ShareClass.AddRTXUser(strUser, strDepart);

                                strHQL = "insert T_MailBoxAuthority(UserCode,PasswordSet,DeleteOperate) Values (" + "'" + strNewUserCode + "'" + ",'YES','YES'" + ")";
                                ShareClass.RunSqlCommand(strHQL);
                            }
                            catch
                            {
                            }
                        }
                        continue;
                    }

                    LoadCustomerOperationRecord(strUserCode.Trim());

                    if (strErrorUserCodeString == "")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRBWC").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRJBWCDXMRYSJDRSBSTRERRORUSERCODESTRINGJC").ToString().Trim() + "')", true);
                    }
                }
            }
        }
    }

    protected int ExcelToDBTest()
    {
        int j = 0;


        try
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();

                j = -1;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
                j = -1;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
                j = -1;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
                    j = -1;
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        string strusercode = dr[i][LanguageHandle.GetWord("ChengYuanDaiMa").ToString().Trim()].ToString().Trim();
                        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strusercode + "' ";
                        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                        if (lst != null && lst.Count > 0)//存在，则不操作
                        {
                            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGCZXTZHDYGDRICYDMTOSTRINGTRIMDRICYXMTOSTRINGTRIMJC").ToString().Trim();

                            j = -1;
                        }
                        else//新增
                        {
                            ProjectMember projectMember = new ProjectMember();

                            projectMember.UserCode = strusercode;
                            projectMember.UserName = dr[i][LanguageHandle.GetWord("ChengYuanXingMing").ToString().Trim()].ToString().Trim();
                            projectMember.Gender = dr[i][LanguageHandle.GetWord("XingBie").ToString().Trim()].ToString().Trim() == "" ? "Male" : dr[i][LanguageHandle.GetWord("XingBie").ToString().Trim()].ToString().Trim();
                            projectMember.Age = int.Parse(dr[i][LanguageHandle.GetWord("NianLing").ToString().Trim()].ToString().Trim() == "" ? "0" : dr[i][LanguageHandle.GetWord("NianLing").ToString().Trim()].ToString().Trim());
                            projectMember.Password = dr[i][LanguageHandle.GetWord("MiMa").ToString().Trim()].ToString().Trim() == "" ? EncryptPassword("12345678", "MD5") : EncryptPassword(dr[i][LanguageHandle.GetWord("MiMa").ToString().Trim()].ToString().Trim(), "MD5");
                            projectMember.RefUserCode = dr[i][LanguageHandle.GetWord("CanKaoGongHao").ToString().Trim()].ToString().Trim() == "" ? strusercode : dr[i][LanguageHandle.GetWord("CanKaoGongHao").ToString().Trim()].ToString().Trim();
                            projectMember.SortNumber = int.Parse(dr[i][LanguageHandle.GetWord("ShunXuHao").ToString().Trim()].ToString().Trim() == "" ? "0" : dr[i][LanguageHandle.GetWord("ShunXuHao").ToString().Trim()].ToString().Trim());

                            if (CheckDepartment(dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim(), dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim()) > 0)
                            {
                                projectMember.DepartCode = dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim() == "" ? ShareClass.GetDepartCodeFromUserCode(strUserCode.Trim()) : dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim();
                                projectMember.DepartName = dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim() == "" ? ShareClass.GetDepartName(projectMember.DepartCode.Trim()) : dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim();
                            }
                            else
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGCBMDMHMCBYZHBCZDRIBMDMTOSTRINGTRIMDRIBMMCTOSTRINGTRIMJC").ToString().Trim();
                                j = -1;
                            }

                            projectMember.ChildDepartment = dr[i][LanguageHandle.GetWord("ZiBuMen").ToString().Trim()].ToString().Trim();
                            projectMember.Duty = dr[i][LanguageHandle.GetWord("ZhiWu").ToString().Trim()].ToString().Trim();
                            projectMember.OfficePhone = dr[i][LanguageHandle.GetWord("BanGongDianHua").ToString().Trim()].ToString().Trim();
                            projectMember.MobilePhone = dr[i][LanguageHandle.GetWord("ShouJi").ToString().Trim()].ToString().Trim();
                            projectMember.EMail = dr[i]["E_Mail"].ToString().Trim();
                            projectMember.JoinDate = DateTime.Parse(dr[i]["加入日期"].ToString().Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : dr[i]["加入日期"].ToString().Trim()); 
                            projectMember.WorkScope = dr[i][LanguageHandle.GetWord("GongZuoFanWei").ToString().Trim()].ToString().Trim();
                            projectMember.Status = dr[i]["状态"].ToString().Trim() == "" ? "Employed" : dr[i]["状态"].ToString().Trim(); 
                            projectMember.PhotoURL = "";

                            projectMember.JobTitle = TB_JobTitle.Text.Trim();
                            projectMember.UserType = DL_UserType.SelectedValue.Trim();
                            projectMember.WorkType = DL_WorkType.SelectedValue.Trim();

                            projectMember.UserRTXCode = dr[i][LanguageHandle.GetWord("RTXZhangHu").ToString().Trim()].ToString().Trim() == "" ? strusercode + projectMember.UserName.Trim() : dr[i][LanguageHandle.GetWord("RTXZhangHu").ToString().Trim()].ToString().Trim();
                            projectMember.MDIStyle = DL_SystemMDIStyle.SelectedValue.Trim();
                            projectMember.AllowDevice = DL_AllowDevice.SelectedValue.Trim();
                            projectMember.CreatorCode = strUserCode.Trim();

                            projectMember.EnglishName = "";
                            projectMember.Nationality = "";
                            projectMember.NativePlace = "";
                            projectMember.HuKou = "";
                            projectMember.Residency = "";
                            projectMember.Address = "";
                            projectMember.BirthDay = DateTime.Now;
                            projectMember.MaritalStatus = "Unmarried";
                            projectMember.Degree = "";
                            projectMember.Major = "";
                            projectMember.GraduateSchool = "";
                            projectMember.IDCard = "";
                            projectMember.BloodType = "";
                            projectMember.Height = 0;
                            projectMember.Language = "";
                            projectMember.UrgencyPerson = "";
                            projectMember.UrgencyCall = "";
                            projectMember.Introducer = "";
                            projectMember.IntroducerDepartment = "";
                            projectMember.IntroducerRelation = "";
                            projectMember.Comment = "";
                        }
                        continue;
                    }
                }
            }
        }
        catch (Exception err)
        {
            LB_ErrorText.Text += err.Message.ToString() + "<br/>";

            j = -1;
        }

        return j;
    }

    protected int CheckDepartment(string strDepartCode, string strDepartName)
    {
        string strHQL;
        IList lst;

        strHQL = "From Department as department Where department.DepartCode = " + "'" + strDepartCode + "'" + " and department.DepartName = " + "'" + strDepartName + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();

        lst = departmentBLL.GetAllDepartments(strHQL);

        return lst.Count;
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
}
