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

public partial class TTWZSupplierMember : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TakeTopCore.CoreShareClass.InitialAllDepartmentTree(Resources.lang.ZZJGT, TreeView1);

            LoadWZSupplierInfo();

            strHQL = "from Department as department ";
            DepartmentBLL departmentBLL = new DepartmentBLL();
            lst = departmentBLL.GetAllDepartments(strHQL);
            DL_Department.DataSource = lst;
            DL_Department.DataBind();

            strHQL = "from WorkType as workType Order by workType.SortNo ASC";
            BookReaderTypeBLL bookReaderTypeBLL = new BookReaderTypeBLL();
            lst = bookReaderTypeBLL.GetAllBookReaderType(strHQL);
            DL_WorkType.DataSource = lst;
            DL_WorkType.DataBind();
            DL_WorkType.Items.Insert(0, new ListItem("--Select--", ""));

            ShareClass.LoadLanguageForDropList(ddlLangSwitcher);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strHQL;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target;

            strHQL = "Select DepartCode,DepartName from T_Department where DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " Union Select DepartCode,DepartName from T_Department where ParentCode = " + "'" + strDepartCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartMent");
            DL_Department.DataSource = ds;
            DL_Department.DataBind();
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "click")
        {
            string strLinkID = e.Item.Cells[0].Text.Trim();
            string strID = ((Button)e.Item.FindControl("BT_SupplierID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string strNewID = e.CommandArgument.ToString();
            string strHQL = "from WZSupplierInfo as wZSupplierInfo where wZSupplierInfo.ID = " + strNewID;
            WZSupplierInfoBLL wZSupplierInfoBLL = new WZSupplierInfoBLL();
            IList lst = wZSupplierInfoBLL.GetAllWZSupplierInfos(strHQL);
            WZSupplierInfo wZSupplierInfo = (WZSupplierInfo)lst[0];

            TB_UserCode.Text = wZSupplierInfo.SupplierCode.Trim();
            TB_UserName.Text = wZSupplierInfo.SupplierName.Trim();
            TB_Password.Text = wZSupplierInfo.SupplierPass.Trim();

            ProjectMember projectMember = ProjectMemberData(wZSupplierInfo.SupplierCode.Trim());
            SystemActiveUser systemActiveUser = SystemActiveUserData(wZSupplierInfo.SupplierCode.Trim());
            if (projectMember != null)
            {
                TB_UserCode.Text = projectMember.UserCode.Trim();
                TB_UserName.Text = projectMember.UserName.Trim();
                DL_Gender.SelectedValue = projectMember.Gender.Trim();
                TB_Age.Amount = projectMember.Age;
                TB_Duty.Text = projectMember.Duty.Trim();
                DL_Department.SelectedValue = projectMember.DepartCode;
                TB_ChildDepartment.Text = projectMember.ChildDepartment.Trim();
                TB_OfficePhone.Text = projectMember.OfficePhone.Trim();
                TB_MobilePhone.Text = projectMember.MobilePhone.Trim();
                TB_EMail.Text = projectMember.EMail.Trim();
                TB_WorkScope.Text = projectMember.WorkScope.Trim();
                DLC_JoinDate.Text = projectMember.JoinDate.ToString("yyyy-MM-dd");
                DL_Status.SelectedValue = projectMember.Status.Trim();
                TB_RefUserCode.Text = projectMember.RefUserCode.Trim();
                NB_SortNumber.Amount = projectMember.SortNumber;
                DL_UserType.SelectedValue = projectMember.UserType.Trim();

                TB_UserRTXCode.Text = projectMember.UserRTXCode.Trim();
                DL_SystemMDIStyle.SelectedValue = projectMember.MDIStyle.Trim();
                DL_AllowDevice.SelectedValue = projectMember.AllowDevice.Trim();
                IM_MemberPhoto.ImageUrl = projectMember.PhotoURL.Trim();
                HL_MemberPhoto.NavigateUrl = projectMember.PhotoURL.Trim();

                try
                {
                    DL_WorkType.SelectedValue = projectMember.WorkType;
                }
                catch
                {
                    DL_WorkType.SelectedValue = "";
                }

                try
                {
                    ddlLangSwitcher.SelectedValue = projectMember.LangCode.Trim();
                }
                catch
                {

                }

                if (projectMember.CreatorCode.Trim() == strUserCode.Trim())
                {
                    if (systemActiveUser != null)
                    {
                        BT_Update.Visible = false;
                    }
                    else
                    {
                        BT_Update.Visible = true;
                        BT_Update.Enabled = true;
                    }
                    BT_Delete.Visible = true;
                    BT_Delete.Enabled = true;
                    BT_UploadPhoto.Enabled = true;
                    BT_DeletePhoto.Enabled = true;
                    BT_TakeUserPhoto.Enabled = true;
                }
                else
                {
                    BT_Update.Visible = false;
                    BT_Delete.Visible = false;
                    BT_UploadPhoto.Enabled = false;
                    BT_DeletePhoto.Enabled = false;
                    BT_TakeUserPhoto.Enabled = false;
                }
            }
            else
            {
                TB_Age.Amount = 0;
                TB_ChildDepartment.Text = "";
                DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TB_RefUserCode.Text = "";
                NB_SortNumber.Amount = 0;
                TB_UserRTXCode.Text = "";
                IM_MemberPhoto.ImageUrl = "";
                HL_MemberPhoto.NavigateUrl = "";

                BT_TakeUserPhoto.Enabled = true;
                BT_UploadPhoto.Enabled = true;
                BT_DeletePhoto.Enabled = false;
            }
        }
    }

    protected BMSupplierLink GetBMSupplierLinkData(string strCode)
    {
        string strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.SupplierCode = '" + strCode + "' Order by bMSupplierLink.ID Asc ";
        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        IList lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
        if (lst.Count > 0 && lst != null)
            return (BMSupplierLink)lst[0];
        else
            return null;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadWZSupplierInfo()
    {
        string strHQL = "Select * from T_WZSupplierInfo Where 1=1 ";

        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and like '%" + txt_SupplierInfo.Text.Trim() + "%' ";
        }

        strHQL += " Order by ID desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZSupplierInfo");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
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

    protected SystemActiveUser SystemActiveUserData(string strusercode)
    {
        string strHQL = "from SystemActiveUser as systemActiveUser where systemActiveUser.UserCode = '" + strusercode + "' ";
        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        IList lst = systemActiveUserBLL.GetAllSystemActiveUsers(strHQL);
        if (lst.Count > 0 && lst != null)
            return (SystemActiveUser)lst[0];
        else
            return null;
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

                    IM_MemberPhoto.ImageUrl = strFileName3;
                    HL_MemberPhoto.NavigateUrl = strFileName3;


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCHCG + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYSCDWJ + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYSCDWJ + "')", true);
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

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode = TB_UserCode.Text.Trim();
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
        string strLangCode = ddlLangSwitcher.SelectedValue.Trim();

        int intSortNumber = int.Parse(NB_SortNumber.Amount.ToString());

        if (strRefUserCode == "")
        {
            strRefUserCode = strUserCode;
            TB_RefUserCode.Text = strRefUserCode;
        }

        if (strUserRTXCode == "")
        {
            strUserRTXCode = strUserCode + strUserName;
        }

        if (strUserCode.Length > 20 | strUserName.Length > 20)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGYHDMHYHMCDBNCGSGHZ + "')", true);
            return;
        }


        ProjectMember projectMember1 = ProjectMemberData(TB_UserCode.Text.Trim().ToUpper());
        SystemActiveUser systemActiveUser = SystemActiveUserData(TB_UserCode.Text.Trim());
        if (strUserCode != "" & strUserName != "" & strDuty != "" & strEmail != "")
        {
            if (projectMember1 != null)//更新
            {
                if (strPassword != "")
                {
                    if (strPassword.Length < 8)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGXSBMMCDBXDYHDY8WJC + "')", true);
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
                projectMember1.OfficePhone = TB_OfficePhone.Text.Trim();
                projectMember1.MobilePhone = TB_MobilePhone.Text.Trim();
                projectMember1.EMail = TB_EMail.Text.Trim();
                projectMember1.JoinDate = DateTime.Parse(DLC_JoinDate.Text);
                projectMember1.WorkScope = TB_WorkScope.Text.Trim();
                projectMember1.Status = DL_Status.SelectedValue.Trim();
                projectMember1.PhotoURL = HL_MemberPhoto.NavigateUrl.Trim();
                projectMember1.UserType = DL_UserType.SelectedValue.Trim();

                projectMember1.WorkType = DL_WorkType.SelectedValue.Trim();

                projectMember1.LunarBirthDay = DateTime.Now;
                projectMember1.LangCode = strLangCode;

                projectMember1.UserRTXCode = strUserRTXCode;
                projectMember1.MDIStyle = strMDIStyle;
                projectMember1.AllowDevice = strAllowDevice;
                projectMember1.IDCard = TB_IDCard.Text.Trim();
                projectMember1.Status = DL_Status.SelectedValue.Trim();

                ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

                try
                {
                    projectMemberBLL.UpdateProjectMember(projectMember1, projectMember1.UserCode.Trim());
                    TB_UserRTXCode.Text = projectMember1.UserRTXCode.Trim();

                    if (systemActiveUser != null)
                        BT_Update.Visible = false;
                    else
                    {
                        BT_Update.Visible = true;
                        BT_Update.Enabled = true;
                    }
                    BT_Delete.Visible = true;
                    BT_Delete.Enabled = true;
                    BT_UploadPhoto.Enabled = true;

                    LoadWZSupplierInfo();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGXCG + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGXSBJCDMZFHMXWK + "')", true);
                }
            }
            else //增加
            {
                if (strPassword == "")
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZMMBNWKJC + "')", true);
                    return;
                }
                if (strPassword.Length < 8)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHKTSBMMCDBXDYHDY8WJC + "')", true);
                    return;
                }
                ProjectMember projectMember = new ProjectMember();

                projectMember.UserCode = TB_UserCode.Text.Trim().ToUpper();
                projectMember.UserName = TB_UserName.Text.Trim();
                projectMember.Gender = DL_Gender.SelectedValue.Trim();
                projectMember.Age = int.Parse(TB_Age.Amount.ToString());
                projectMember.Password = EncryptPassword(TB_Password.Text.Trim(), "MD5");

                projectMember.RefUserCode = TB_RefUserCode.Text.Trim();
                projectMember.SortNumber = intSortNumber;

                projectMember.DepartCode = DL_Department.SelectedValue.Trim();
                projectMember.DepartName = DL_Department.SelectedItem.Text.Trim();
                projectMember.ChildDepartment = TB_ChildDepartment.Text.Trim();
                projectMember.Duty = TB_Duty.Text.Trim();
                projectMember.OfficePhone = TB_OfficePhone.Text.Trim();
                projectMember.MobilePhone = TB_MobilePhone.Text.Trim();
                projectMember.EMail = TB_EMail.Text.Trim();
                projectMember.JoinDate = DateTime.Parse(DLC_JoinDate.Text);
                projectMember.WorkScope = TB_WorkScope.Text.Trim();
                projectMember.CreatorCode = strCreatorCode;
                projectMember.Status = DL_Status.SelectedValue.Trim();

                projectMember.WorkType = "合同工";
                projectMember.IDCard = TB_IDCard.Text.Trim();

                projectMember.EnglishName = "";
                projectMember.Nationality = "";
                projectMember.NativePlace = "";
                projectMember.HuKou = "";
                projectMember.Residency = "";
                projectMember.Address = "";
                projectMember.BirthDay = DateTime.Now;
                projectMember.LunarBirthDay = DateTime.Now;
                projectMember.MaritalStatus = "未婚";
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
                projectMember.PhotoURL = HL_MemberPhoto.NavigateUrl.Trim();

                projectMember.UserType = DL_UserType.SelectedValue.Trim();
                projectMember.UserRTXCode = strUserRTXCode;

                projectMember.CssDirectory = "CssRed";
                projectMember.MDIStyle = strMDIStyle;
                projectMember.AllowDevice = strAllowDevice;

                ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

                try
                {
                    projectMemberBLL.AddProjectMember(projectMember);

                    TB_UserRTXCode.Text = projectMember.UserRTXCode.Trim();

                    if (systemActiveUser != null)
                        BT_Update.Visible = false;
                    else
                    {
                        BT_Update.Visible = true;
                        BT_Update.Enabled = true;
                    }

                    BT_Delete.Visible = true;
                    BT_Delete.Enabled = true;
                    BT_UploadPhoto.Enabled = true;


                    strHQL = "insert into T_MailBoxAuthority(UserCode,PasswordSet,DeleteOperate) Values (" + "'" + strUserCode + "'" + ",'YES','YES'" + ")";
                    ShareClass.RunSqlCommand(strHQL);

                    LoadWZSupplierInfo();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHKTCG + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHKTSBJCDMZFHMXWK + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYHDMYHMCMMZWEMAILDBNWKJC + "')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWZSupplierInfo();
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        int intWEBLicenseNumber, intAPPLicenseNumer, intWEBUserNumber, intAPPUserNumber;
        string strLicenseType;

        string strUserCode = TB_UserCode.Text.Trim();
        string strUserName;

        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        SystemActiveUser systemActiveUser = new SystemActiveUser();

        string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];

        TakeTopLicense license = new TakeTopLicense();
        intWEBLicenseNumber = license.GetWEBLicenseNumber(strServerName);
        intAPPLicenseNumer = license.GetAPPLicenseNumber(strServerName);

        //取得已增加的WEB和APP用户数
        intWEBUserNumber = GetWEBSystemUserNumber();
        intAPPUserNumber = GetAPPSystemUserNumber();

        strLicenseType = license.GetLicenseType(strServerName);
        if (strLicenseType == "REGISTER")
        {
            strLicenseType = "";

            intWEBLicenseNumber += 1;
            intAPPLicenseNumer += 1;

            if (intWEBUserNumber > intWEBLicenseNumber)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGWEBYHSYJDYNDZCSAPPLICENSEGDDSMBNZXZXDWEBYHL + "')", true);
                return;
            }

            if (intAPPUserNumber > intAPPLicenseNumer)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGAPPYHSYJDYNDZCSAPPLICENSEGDDSMBNZXZXDAPPYHL + "')", true);
                return;
            }

            if (GetUserCount(strUserCode) == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSBCYHBCZJC + "')", true);
                return;
            }

            if (intWEBUserNumber < intWEBLicenseNumber)
            {
                if (strUserCode != "")
                {
                    strUserName = ShareClass.GetUserName(strUserCode);
                    TB_UserName.Text = strUserName;

                    systemActiveUser.UserCode = strUserCode;
                    systemActiveUser.UserName = strUserName;
                    systemActiveUser.JoinTime = DateTime.Now;
                    systemActiveUser.OperatorCode = Session["UserCode"].ToString().Trim();
                    systemActiveUser.WebUser = "YES";
                    systemActiveUser.AppUser = "NO";

                    try
                    {
                        systemActiveUserBLL.AddSystemActiveUser(systemActiveUser);

                        BT_Update.Visible = false;
                        BT_Delete.Visible = true;
                        BT_Delete.Enabled = true;

                        string strHQL;

                        //从模板用户复制模组
                        string strSampleUserCode = System.Configuration.ConfigurationManager.AppSettings["ModuleSampleUser"];
                        strHQL = "INSERT INTO T_ProModule(ModuleName, UserCode, Visible, ModuleType,UserType)";
                        strHQL += " Select A.ModuleName," + "'" + strUserCode + "'" + ",A.Visible,A.ModuleType,A.UserType from T_ProModule A";
                        strHQL += " where A.UserCode = " + "'" + strSampleUserCode + "'"; ;
                        strHQL += " and (rtrim(A.ModuleName) not in (Select rtrim(B.ModuleName) From T_ProModule B where B.UserCode =" + "'" + strUserCode + "'" + " AND B.ModuleType = A.ModuleType AND B.UserType = A.UserType )";
                        strHQL += " and RTRIM(ModuleName) in (Select rtrim(ModuleName) From T_ProModuleLevel C where C.ModuleType = A.ModuleType AND C.UserType = A.UserType AND C.Visible = 'YES' AND C.IsDeleted = 'NO'))";
                        ShareClass.RunSqlCommand(strHQL);

                        //Label2.Text = strHQL;

                        LoadWZSupplierInfo();
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHJHCG + "')", true);
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHJHSBCYHKNYCZBNZFTJJC + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHJHSBYHDMBNWK + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXTYHSYJDYNDSLICENSEGDDSMBNZXZXDYHL + "')", true);
            }
        }
        else
        {
            if (strUserCode != "")
            {
                if (GetUserCount(strUserCode) == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('帐号" + Resources.lang.ZZJHSBCYHBCZJC + "')", true);
                    return;
                }

                strUserName = ShareClass.GetUserName(strUserCode);
                TB_UserName.Text = strUserName;

                systemActiveUser.UserCode = strUserCode;
                systemActiveUser.UserName = strUserName;
                systemActiveUser.JoinTime = DateTime.Now;
                systemActiveUser.OperatorCode = Session["UserCode"].ToString().Trim();
                systemActiveUser.WebUser = "YES";
                systemActiveUser.AppUser = "NO";


                try
                {
                    systemActiveUserBLL.AddSystemActiveUser(systemActiveUser);

                    BT_Update.Visible = false;
                    BT_Delete.Visible = true;
                    BT_Delete.Enabled = true;

                    LoadWZSupplierInfo();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHJHCG + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHJHSBCYHKNYCZBNZFTJJC + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZHJHSBYHDMBNWK + "')", true);
            }
        }
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

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode;

        strUserCode = TB_UserCode.Text.Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        ProjectMember projectMember = new ProjectMember();

        projectMember.UserCode = strUserCode;

        try
        {
            projectMemberBLL.DeleteProjectMember(projectMember);

            BT_Update.Visible = false;
            BT_Delete.Visible = false;
            BT_UploadPhoto.Enabled = false;

            strHQL = "Delete From T_SystemActiveUser where UserCode='" + strUserCode + "' ";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_MailBoxAuthority Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ProModule Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_UserAttendanceRule Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadWZSupplierInfo();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCSBJC + "')", true);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZSBXSRYHDMHMCZYCNC + "')", true);
            return;
        }

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'" + " or projectMember.UserName = " + "'" + strUserName + "'";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        if (lst.Count > 0)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            TB_UserCode.Text = projectMember.UserCode;
            TB_UserName.Text = projectMember.UserName;
            DL_Gender.SelectedValue = projectMember.Gender.Trim();
            TB_Age.Amount = projectMember.Age;
            TB_Duty.Text = projectMember.Duty;
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

            IM_MemberPhoto.ImageUrl = projectMember.PhotoURL.Trim();
            HL_MemberPhoto.NavigateUrl = projectMember.PhotoURL.Trim();
            NB_SortNumber.Amount = projectMember.SortNumber;

            DL_SystemMDIStyle.SelectedValue = projectMember.MDIStyle.Trim();
            DL_AllowDevice.SelectedValue = projectMember.AllowDevice.Trim();

            SystemActiveUser systemActiveUser = SystemActiveUserData(projectMember.UserCode.Trim());
            if (systemActiveUser != null)
                BT_Update.Visible = false;
            else
            {
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
            }
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_UploadPhoto.Enabled = true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZMYCYHJCYHDMHMCSFZ + "')", true);
        }
    }

    protected void BT_TakeUserPhoto_Click(object sender, EventArgs e)
    {
        Panel2.Visible = true;
    }

    protected void BT_SaveUserPhoto_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strUserPhotoString;

        strUserCode = TB_UserCode.Text.Trim();
        strUserPhotoString = TB_UserPhotoString.Text.Trim();

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

    protected string GetProjectMember(string strusercode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strusercode + "' ";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst != null && lst.Count > 0)
            return "已开通";
        else
            return "未开通";
    }

    protected string GetSystemActionUser(string strusercode)
    {
        string strHQL;
        IList lst;

        strHQL = "from SystemActiveUser as systemActiveUser where systemActiveUser.UserCode = '" + strusercode + "' ";
        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        lst = systemActiveUserBLL.GetAllSystemActiveUsers(strHQL);
        if (lst != null && lst.Count > 0)
            return "已激活";
        else
            return "未激活";
    }

    protected string GetUserPhotoURL(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        return projectMember.PhotoURL.Trim();
    }

    protected int GetWEBSystemUserNumber()
    {
        string strHQL;
        IList lst;

        strHQL = "From SystemActiveUser as systemActiveUser Where systemActiveUser.WebUser = 'YES'";
        SystemActiveUserBLL systemAcitveUserBLL = new SystemActiveUserBLL();
        lst = systemAcitveUserBLL.GetAllSystemActiveUsers(strHQL);

        return lst.Count;
    }

    protected int GetAPPSystemUserNumber()
    {
        string strHQL;
        IList lst;

        strHQL = "From SystemActiveUser as systemActiveUser Where systemActiveUser.AppUser = 'YES'";
        SystemActiveUserBLL systemAcitveUserBLL = new SystemActiveUserBLL();
        lst = systemAcitveUserBLL.GetAllSystemActiveUsers(strHQL);

        return lst.Count;
    }
}