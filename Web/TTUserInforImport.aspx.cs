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

using NPOI;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using TakeTopSecurity;
using java.nio.file;


public partial class TTUserInforImport : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode.Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工资料导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByUserInfor(strUserCode);

            strHQL = "Select RTRIM(DepartCode) DepartCode,RTRIM(DepartName) DepartName from T_Department ";
            strHQL += " Where DepartCode in " + LB_DepartString.Text.Trim();
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");
            DL_Department.DataSource = ds;
            DL_Department.DataBind();

            LoadWorkType();
            LoadDepartPosition();
            LoadSystemMDIStyle();

            LoadCustomerOperationRecord(strUserCode.Trim());

            string strUserType = ShareClass.GetUserType(strUserCode);
            if (strUserType == "OUTER")
            {
                DL_UserType.SelectedValue = "OUTER";
                DL_UserType.Enabled = false;
            }

            if (strUserType == "INNER")
            {
                DL_UserType.SelectedValue = "INNER";
                DL_UserType.Enabled = true;
            }
        }
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
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

                DL_SystemMDIStyle.SelectedValue = "LeftRightDownExhibition";   
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

            DL_SystemMDIStyle.SelectedValue = "LeftRightDownExhibition";   
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
                string strExtName = System.IO.Path.GetExtension(strFileName2);
                strFileName2 = System.IO.Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtName;

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

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadCustomerOperationRecord(strUserCode.Trim());
    }

    protected void btn_ExcelToDB_Click(object sender, EventArgs e)
    {
        string strUser, strDepart, strNewUserCode;
        string strErrorUserCodeString = "";

        if (ExcelToDBTest() == -1)
        {
            LogClass.WriteLogFile(LB_ErrorText.Text);
            return;
        }
        else
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
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
                        strNewUserCode = dr[i][LanguageHandle.GetWord("ChengYuanDaiMa").ToString().Trim()].ToString().Trim().ToUpper();

                        if (strNewUserCode != "")
                        {
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
                                    projectMember.JoinDate = DateTime.Parse(dr[i]["JoinDate"].ToString().Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : dr[i]["JoinDate"].ToString().Trim());   
                                    projectMember.WorkScope = dr[i][LanguageHandle.GetWord("GongZuoFanWei").ToString().Trim()].ToString().Trim();
                                    projectMember.Status = dr[i]["Status"].ToString().Trim() == "" ? "Employed" : dr[i]["Status"].ToString().Trim();   

                                    try
                                    {
                                        projectMember.LangCode = System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];
                                    }
                                    catch
                                    {
                                        projectMember.LangCode = "zh-CN";
                                    }

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
                                    projectMember.LunarBirthDay = DateTime.Now;
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

                                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strNewUserCode + " : " + err.Message.ToString() + "<br/>"; ;

                                    LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strNewUserCode + " : " + err.Message.ToString());
                                }

                                //增加用户到RTX
                                try
                                {
                                    strHQL = "insert T_MailBoxAuthority(UserCode,PasswordSet,DeleteOperate) Values (" + "'" + strNewUserCode + "'" + ",'YES','YES'" + ")";
                                    ShareClass.RunSqlCommand(strHQL);

                                    strUser = dr[i][LanguageHandle.GetWord("CanKaoGongHao").ToString().Trim()].ToString().Trim() + dr[i][LanguageHandle.GetWord("ChengYuanXingMing").ToString().Trim()].ToString().Trim();
                                    strDepart = dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim() + " " + dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim();

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
                                    LogClass.WriteLogFile(strHQL);
                                    
                                    ShareClass.RunSqlCommand(strHQL);
                                }
                                catch
                                {

                                }
                            }
                            continue;
                        }
                    }


                    LoadCustomerOperationRecord(strUserCode.Trim());

                    if (strErrorUserCodeString == "")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click555", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRBWC").ToString().Trim() + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click666", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRJBWCDXMRYSJDRSBSTRERRORUSERCODESTRINGJC").ToString().Trim() + "')", true);
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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click222", "alert('" + LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() + "')", true);
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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click333", "alert('" + LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() + "')", true);
                    j = -1;
                }
                else
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        //LogClass.WriteLogFile(dr[i][1].ToString());
                        //LogClass.WriteLogFile(dr[i][2].ToString());

                        string strusercode = dr[i][LanguageHandle.GetWord("ChengYuanDaiMa").ToString().Trim()].ToString().Trim().ToUpper();
                        if (strusercode != "")
                        {
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
                                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGCBMDMHMCBYZHBZNGXFWDRIBMDMTOSTRINGTRIMDRIBMMCTOSTRINGTRIMJC").ToString().Trim() + dr[i][LanguageHandle.GetWord("BuMenDaiMa").ToString().Trim()].ToString().Trim() + ", " + dr[i][LanguageHandle.GetWord("BuMenMingChen").ToString().Trim()].ToString().Trim();
                                   
                                    j = -1;
                                }

                                projectMember.ChildDepartment = dr[i][LanguageHandle.GetWord("ZiBuMen").ToString().Trim()].ToString().Trim();
                                projectMember.Duty = dr[i][LanguageHandle.GetWord("ZhiWu").ToString().Trim()].ToString().Trim();
                                projectMember.OfficePhone = dr[i][LanguageHandle.GetWord("BanGongDianHua").ToString().Trim()].ToString().Trim();
                                projectMember.MobilePhone = dr[i][LanguageHandle.GetWord("ShouJi").ToString().Trim()].ToString().Trim();
                                projectMember.EMail = dr[i]["E_Mail"].ToString().Trim();
                                projectMember.JoinDate = DateTime.Parse(dr[i]["JoinDate"].ToString().Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : dr[i]["JoinDate"].ToString().Trim());   
                                projectMember.WorkScope = dr[i][LanguageHandle.GetWord("GongZuoFanWei").ToString().Trim()].ToString().Trim();
                                projectMember.Status = dr[i]["Status"].ToString().Trim() == "" ? "Employed" : dr[i]["Status"].ToString().Trim();   
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
                                projectMember.LunarBirthDay = DateTime.Now;
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
        }
        catch (Exception err)
        {
            LB_ErrorText.Text += err.Message.ToString() + "<br/>"; ;
            j = -1;
        }

        return j;
    }

    protected int CheckDepartment(string strDepartCode, string strDepartName)
    {
        string strHQL;
        IList lst;

        strHQL = "From Department as department Where department.DepartCode = " + "'" + strDepartCode + "'" + " and department.DepartName = " + "'" + strDepartName + "'";
        strHQL += " and department.DepartCode in " + LB_DepartString.Text.Trim();
        DepartmentBLL departmentBLL = new DepartmentBLL();

        lst = departmentBLL.GetAllDepartments(strHQL);

        return lst.Count;
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = "Select * From T_CustomerOperationRecord Where Creater='" + strUserCode + "' ";
        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and (UserCode like '%" + txt_SupplierInfo.Text.Trim() + "%' or UserName like '%" + txt_SupplierInfo.Text.Trim() + "%' or " +
                "Creater like '%" + txt_SupplierInfo.Text.Trim() + "%' or CreaterName like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order by ID";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerOperationRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
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

    protected string GetUserPhotoURL(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        return projectMember.PhotoURL.Trim();
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

}
