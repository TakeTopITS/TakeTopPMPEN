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

public partial class TTUserInfor_Student : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserCode"] != null)
        {
            strUserCode = Session["UserCode"].ToString();
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);

        if (Page.IsPostBack != true)
        {
            DLC_BirthDay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TXT_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            DataBinder(LB_BelongDepartCode.Text.Trim());

            DataClassBinder();
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_BelongDepartCode.Text = strDepartCode;
            TB_BelongDepartName.Text = strDepartName;

            DataBinder(strDepartCode);
        }
    }

    private void DataBinder(string strBelongDepartCode)
    {
        string strHQL;

        if (strBelongDepartCode == "")
        {
            strHQL = string.Format(@"select s.*,c.ClassName from T_ProjectMemberStudent s
                    , T_ProjectMemberClass  c ,
                     T_ProjectMemberGrade g where s.ClassID =  c.ID and c.GradeID = g.ID
                                        and (g.DepartCode in
                    {0} 
                    or 
                    s.CreatUserCode = '{1}') Order By s.UserCode DESC", LB_DepartString.Text, strUserCode);
        }
        else
        {
            strHQL = string.Format(@"select s.*,c.ClassName from T_ProjectMemberStudent s
                    , T_ProjectMemberClass  c ,
                     T_ProjectMemberGrade g where s.ClassID =  c.ID and c.GradeID = g.ID
                         and g.DepartCode = '{0}'  Order By s.UserCode DESC", strBelongDepartCode);
        }

        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "Student").Tables[0];

        DG_List.DataSource = dtStudent;
        DG_List.DataBind();

        LB_Sql.Text = strHQL;

        LB_StudentNumber.Text = LanguageHandle.GetWord("XueShengShu").ToString().Trim() + dtStudent.Rows.Count.ToString() ;
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strStudentCode, strStudentName;
        strStudentCode = "'" + "%" + TB_UserCode.Text.Trim() + "%" + "'";
        strStudentName = "'" + "%" + TB_UserName.Text.Trim() + "%" + "'";

        strHQL = string.Format(@"select s.*,c.ClassName from T_ProjectMemberStudent s
                    , T_ProjectMemberClass  c ,
                     T_ProjectMemberGrade g where s.ClassID =  c.ID and c.GradeID = g.ID
                                        and (g.DepartCode in
                    {0} 
                    or 
                    s.CreatUserCode = '{1}') and ( s.UserCode Like {2} and s.UserName Like {3} ) Order By s.UserCode DESC", LB_DepartString.Text, strUserCode, strStudentCode, strStudentName);

        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "Student").Tables[0];

        DG_List.DataSource = dtStudent;
        DG_List.DataBind();

        LB_Sql.Text = strHQL;

        LB_StudentNumber.Text = LanguageHandle.GetWord("XueShengShu").ToString().Trim() + dtStudent.Rows.Count.ToString() ;
    }


    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        LB_BelongDepartCode.Text = "";
        TB_BelongDepartName.Text = "";
    }


    private void DataClassBinder()
    {
        string strClassHQL = string.Format(@"select c.* from T_ProjectMemberClass c 
                    left join T_ProjectMemberGrade g on c.GradeID = g.ID
                    where 
                    (
                    g.DepartCode in {0} 
                    or c.UserCode = '{1}' 
                    )
                    ", LB_DepartString.Text, strUserCode);
        DataTable dtClass = ShareClass.GetDataSetFromSql(strClassHQL, "Class").Tables[0];

        DDL_StudentClass.DataSource = dtClass;

        DDL_StudentClass.DataTextField = "ClassName";
        DDL_StudentClass.DataValueField = "ID";

        DDL_StudentClass.DataBind();

    }

    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "click")
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdArges = e.CommandArgument.ToString();

            string strHQL;
            IList lst;

            strHQL = "from ProjectMemberStudent as projectMemberStudent where UserCode = '" + cmdArges + "'";

            ProjectMemberStudentBLL projectMemberStudentBLL = new ProjectMemberStudentBLL();
            lst = projectMemberStudentBLL.GetAllProjectMemberStudents(strHQL);
            if (lst != null && lst.Count > 0)
            {
                ProjectMemberStudent projectMemberStudent = (ProjectMemberStudent)lst[0];

                TXT_JoinDate.Text = projectMemberStudent.JoinDate.ToString();
                TXT_UserCode.Text = projectMemberStudent.UserCode;

                HF_UserCode.Value = projectMemberStudent.UserCode;


                TB_UserName.Text = projectMemberStudent.UserName;
                DL_Gender.SelectedValue = projectMemberStudent.Gender;
                DLC_BirthDay.Text = projectMemberStudent.BirthDay.ToString();
                TB_NativePlace.Text = projectMemberStudent.NativePlace;
                TB_HuKou.Text = projectMemberStudent.HuKou;
                DDL_StudentClass.SelectedValue = projectMemberStudent.ClassID.ToString();
                TB_Residency.Text = projectMemberStudent.Residency;
                TB_OfficePhone.Text = projectMemberStudent.OfficePhone;
                TB_UrgencyPerson.Text = projectMemberStudent.UrgencyPerson;
                TB_UrgencyCall.Text = projectMemberStudent.UrgencyCall;
                TXT_AttendedKindergarten.Text = projectMemberStudent.AttendedKindergarten;
                CKB_ManagedAfterClass.Checked = projectMemberStudent.ManagedAfterClass == 0 ? false : true;
                TXT_FatherName.Text = projectMemberStudent.FatherName;
                TXT_FatherUnit.Text = projectMemberStudent.FatherUnit;
                TXT_FatherPhone.Text = projectMemberStudent.FatherPhone;
                TXT_MonthName.Text = projectMemberStudent.MonthName;
                TXT_MonthUnit.Text = projectMemberStudent.MonthUnit;
                TXT_MonthPhone.Text = projectMemberStudent.MonthPhone;
                DLC_AdmissionDate.Text = projectMemberStudent.AdmissionDate;
                CKB_IsAllergy.Checked = projectMemberStudent.IsAllergy == 0 ? false : true;
                CKB_IsAsthma.Checked = projectMemberStudent.IsAsthma == 0 ? false : true;
                CKB_IsInheritedillnesses.Checked = projectMemberStudent.IsInheritedillnesses == 0 ? false : true;
                CKB_IsMedicalAllergy.Checked = projectMemberStudent.IsMedicalAllergy == 0 ? false : true;
                CKB_IsForbiddenillness.Checked = projectMemberStudent.IsForbiddenillness == 0 ? false : true;
                CKB_IsSurgery.Checked = projectMemberStudent.IsSurgery == 0 ? false : true;
                CKB_IsMajordiseases.Checked = projectMemberStudent.IsMajordiseases == 0 ? false : true;

                TXT_OtherRemark.Text = projectMemberStudent.OtherRemark;

                TXT_WangFeePerSemester.Text = projectMemberStudent.WangFeePerSemester.ToString();
                TXT_Meals.Text = projectMemberStudent.Meals.ToString();
                TXT_ActivityCost.Text = projectMemberStudent.ActivityCost.ToString();
                TXT_CustodyAfterClass.Text = projectMemberStudent.CustodyAfterClass.ToString();
                TXT_ReplaceCosts.Text = projectMemberStudent.ReplaceCosts.ToString();

                IM_MemberPhoto.ImageUrl = projectMemberStudent.PhotoURL;

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                BT_TakePhoto.Enabled = true;
                BT_DeletePhoto.Enabled = true;
            }
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "Student").Tables[0];

        DG_List.DataSource = dtStudent;
        DG_List.DataBind();
    }

    protected void BT_TakePhoto_Click(object sender, EventArgs e)
    {
        Panel2.Visible = true;
    }

    protected void BT_SavePhoto_Click(object sender, EventArgs e)
    {
        string strUserCode;
        string strUserPhotoString;

        strUserCode = TXT_UserCode.Text.Trim();
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

            string strHQL = "Update T_ProjectMemberStudent Set PhotoURL = " + "'" + strUserPhotoURL + "'" + " Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_MemberPhoto.ImageUrl = GetUserPhotoURL(strUserCode);
        }
    }


    protected void BT_UploadPhoto_Click(object sender, EventArgs e)
    {
        if (this.FUP_File.PostedFile != null)
        {
            string strFileName1 = FUP_File.PostedFile.FileName.Trim();
            string strUserCode = TXT_UserCode.Text.Trim();
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

                    strHQL = "Update T_ProjectMemberStudent Set PhotoURL = " + "'" + strFileName3 + "'" + " Where UserCode = " + "'" + strUserCode + "'";
                    ShareClass.RunSqlCommand(strHQL);

                    IM_MemberPhoto.ImageUrl = strFileName3;
                    HL_MemberPhoto.NavigateUrl = strFileName3;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCHCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim()+"')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim()+"')", true);
        }
    }

    protected void BT_DeletePhoto_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode = TXT_UserCode.Text.Trim();

        try
        {
            strHQL = "Update T_ProjectMemberStudent Set PhotoURL = '' Where UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            IM_MemberPhoto.ImageUrl = "";
            HL_MemberPhoto.NavigateUrl = "";


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected string GetUserPhotoURL(string strUserCode)
    {
        string strHQL = " from ProjectMemberStudent as projectMemberStudent where projectMemberStudent.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberStudentBLL projectMemberStudentBLL = new ProjectMemberStudentBLL();
        IList lst = projectMemberStudentBLL.GetAllProjectMemberStudents(strHQL);
        ProjectMemberStudent projectMemberStudent = (ProjectMemberStudent)lst[0];

        return projectMemberStudent.PhotoURL.Trim();
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectMemberStudent projectMemberStudent = new ProjectMemberStudent();

            DateTime dtJoinDate = DateTime.Now;
            DateTime.TryParse(TXT_JoinDate.Text.Trim(), out dtJoinDate);
            projectMemberStudent.JoinDate = dtJoinDate;
            projectMemberStudent.UserCode = TXT_UserCode.Text.Trim();

            projectMemberStudent.UserName = TB_UserName.Text.Trim();
            projectMemberStudent.Gender = DL_Gender.SelectedValue;

            DateTime dtBirthDay = DateTime.Now;
            DateTime.TryParse(DLC_BirthDay.Text, out dtBirthDay);
            projectMemberStudent.BirthDay = dtBirthDay;
            projectMemberStudent.NativePlace = TB_NativePlace.Text.Trim();
            projectMemberStudent.HuKou = TB_HuKou.Text.Trim();
            projectMemberStudent.StudentClass = DDL_StudentClass.SelectedItem.Text;

            int intClassID = 0;
            int.TryParse(DDL_StudentClass.SelectedValue, out intClassID);

            if (intClassID == 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBJ").ToString().Trim()+"')", true);
                return;
            }


            projectMemberStudent.ClassID = intClassID;

            projectMemberStudent.Residency = TB_Residency.Text.Trim();
            projectMemberStudent.OfficePhone = TB_OfficePhone.Text.Trim();
            projectMemberStudent.UrgencyPerson = TB_UrgencyPerson.Text.Trim();
            projectMemberStudent.UrgencyCall = TB_UrgencyCall.Text.Trim();
            projectMemberStudent.AttendedKindergarten = TXT_AttendedKindergarten.Text.Trim();
            projectMemberStudent.ManagedAfterClass = CKB_ManagedAfterClass.Checked == false ? 0 : 1;
            projectMemberStudent.FatherName = TXT_FatherName.Text.Trim();
            projectMemberStudent.FatherUnit = TXT_FatherUnit.Text.Trim();
            projectMemberStudent.FatherPhone = TXT_FatherPhone.Text.Trim();
            projectMemberStudent.MonthName = TXT_MonthName.Text.Trim();
            projectMemberStudent.MonthUnit = TXT_MonthUnit.Text.Trim();
            projectMemberStudent.MonthPhone = TXT_MonthPhone.Text.Trim();
            projectMemberStudent.AdmissionDate = DLC_AdmissionDate.Text.Trim();
            projectMemberStudent.IsAllergy = CKB_IsAllergy.Checked == false ? 0 : 1;
            projectMemberStudent.IsAsthma = CKB_IsAsthma.Checked == false ? 0 : 1;
            projectMemberStudent.IsInheritedillnesses = CKB_IsInheritedillnesses.Checked == false ? 0 : 1;
            projectMemberStudent.IsMedicalAllergy = CKB_IsMedicalAllergy.Checked == false ? 0 : 1;
            projectMemberStudent.IsForbiddenillness = CKB_IsForbiddenillness.Checked == false ? 0 : 1;
            projectMemberStudent.IsSurgery = CKB_IsSurgery.Checked == false ? 0 : 1;
            projectMemberStudent.IsMajordiseases = CKB_IsMajordiseases.Checked == false ? 0 : 1;

            projectMemberStudent.OtherRemark = TXT_OtherRemark.Text.Trim();

            decimal decimalWangFeePerSemester = 0;
            decimal.TryParse(TXT_WangFeePerSemester.Text.Trim(), out decimalWangFeePerSemester);
            projectMemberStudent.WangFeePerSemester = decimalWangFeePerSemester;

            decimal decimalMeals = 0;
            decimal.TryParse(TXT_Meals.Text.Trim(), out decimalMeals);

            projectMemberStudent.Meals = decimalMeals;
            decimal decimalActivityCost = 0;
            decimal.TryParse(TXT_ActivityCost.Text.Trim(), out decimalActivityCost);

            projectMemberStudent.ActivityCost = decimalActivityCost;

            decimal decimalCustodyAfterClass = 0;
            decimal.TryParse(TXT_CustodyAfterClass.Text.Trim(), out decimalCustodyAfterClass);

            projectMemberStudent.CustodyAfterClass = decimalCustodyAfterClass;
            decimal decimalReplaceCosts = 0;
            decimal.TryParse(TXT_ReplaceCosts.Text.Trim(), out decimalReplaceCosts);
            projectMemberStudent.ReplaceCosts = decimalReplaceCosts;

            projectMemberStudent.CreatUserCode = strUserCode;


            ProjectMemberStudentBLL projectMemberStudentBLL = new ProjectMemberStudentBLL();
            projectMemberStudentBLL.AddProjectMemberStudent(projectMemberStudent);

            DataBinder(LB_BelongDepartCode.Text.Trim());

            BT_TakePhoto.Enabled = true;
            BT_DeletePhoto.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJCZFHMXWK").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            IList lst;

            strHQL = "from ProjectMemberStudent as projectMemberStudent where UserCode = '" + TXT_UserCode.Text.Trim() + "'";

            ProjectMemberStudentBLL projectMemberStudentBLL = new ProjectMemberStudentBLL();
            lst = projectMemberStudentBLL.GetAllProjectMemberStudents(strHQL);
            if (lst != null && lst.Count > 0)
            {
                ProjectMemberStudent projectMemberStudent = (ProjectMemberStudent)lst[0];

                DateTime dtJoinDate = DateTime.Now;
                DateTime.TryParse(TXT_JoinDate.Text.Trim(), out dtJoinDate);
                projectMemberStudent.JoinDate = dtJoinDate;
                projectMemberStudent.UserCode = TXT_UserCode.Text.Trim();
                projectMemberStudent.UserName = TB_UserName.Text.Trim();
                projectMemberStudent.Gender = DL_Gender.SelectedValue;

                DateTime dtBirthDay = DateTime.Now;
                DateTime.TryParse(DLC_BirthDay.Text, out dtBirthDay);
                projectMemberStudent.BirthDay = dtBirthDay;
                projectMemberStudent.NativePlace = TB_NativePlace.Text.Trim();
                projectMemberStudent.HuKou = TB_HuKou.Text.Trim();
                projectMemberStudent.StudentClass = DDL_StudentClass.SelectedValue;

                int intClassID = 0;
                int.TryParse(DDL_StudentClass.SelectedValue, out intClassID);

                if (intClassID == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBJ").ToString().Trim()+"')", true);
                    return;
                }

                projectMemberStudent.ClassID = intClassID;

                projectMemberStudent.Residency = TB_Residency.Text.Trim();
                projectMemberStudent.OfficePhone = TB_OfficePhone.Text.Trim();
                projectMemberStudent.UrgencyPerson = TB_UrgencyPerson.Text.Trim();
                projectMemberStudent.UrgencyCall = TB_UrgencyCall.Text.Trim();
                projectMemberStudent.AttendedKindergarten = TXT_AttendedKindergarten.Text.Trim();
                projectMemberStudent.ManagedAfterClass = CKB_ManagedAfterClass.Checked == false ? 0 : 1;
                projectMemberStudent.FatherName = TXT_FatherName.Text.Trim();
                projectMemberStudent.FatherUnit = TXT_FatherUnit.Text.Trim();
                projectMemberStudent.FatherPhone = TXT_FatherPhone.Text.Trim();
                projectMemberStudent.MonthName = TXT_MonthName.Text.Trim();
                projectMemberStudent.MonthUnit = TXT_MonthUnit.Text.Trim();
                projectMemberStudent.MonthPhone = TXT_MonthPhone.Text.Trim();
                projectMemberStudent.AdmissionDate = DLC_AdmissionDate.Text.Trim();
                projectMemberStudent.IsAllergy = CKB_IsAllergy.Checked == false ? 0 : 1;
                projectMemberStudent.IsAsthma = CKB_IsAsthma.Checked == false ? 0 : 1;
                projectMemberStudent.IsInheritedillnesses = CKB_IsInheritedillnesses.Checked == false ? 0 : 1;
                projectMemberStudent.IsMedicalAllergy = CKB_IsMedicalAllergy.Checked == false ? 0 : 1;
                projectMemberStudent.IsForbiddenillness = CKB_IsForbiddenillness.Checked == false ? 0 : 1;
                projectMemberStudent.IsSurgery = CKB_IsSurgery.Checked == false ? 0 : 1;
                projectMemberStudent.IsMajordiseases = CKB_IsMajordiseases.Checked == false ? 0 : 1;

                projectMemberStudent.OtherRemark = TXT_OtherRemark.Text.Trim();

                decimal decimalWangFeePerSemester = 0;
                decimal.TryParse(TXT_WangFeePerSemester.Text.Trim(), out decimalWangFeePerSemester);
                projectMemberStudent.WangFeePerSemester = decimalWangFeePerSemester;

                decimal decimalMeals = 0;
                decimal.TryParse(TXT_Meals.Text.Trim(), out decimalMeals);

                projectMemberStudent.Meals = decimalMeals;
                decimal decimalActivityCost = 0;
                decimal.TryParse(TXT_ActivityCost.Text.Trim(), out decimalActivityCost);

                projectMemberStudent.ActivityCost = decimalActivityCost;

                decimal decimalCustodyAfterClass = 0;
                decimal.TryParse(TXT_CustodyAfterClass.Text.Trim(), out decimalCustodyAfterClass);

                projectMemberStudent.CustodyAfterClass = decimalCustodyAfterClass;
                decimal decimalReplaceCosts = 0;
                decimal.TryParse(TXT_ReplaceCosts.Text.Trim(), out decimalReplaceCosts);
                projectMemberStudent.ReplaceCosts = decimalReplaceCosts;

                projectMemberStudent.CreatUserCode = strUserCode;

                projectMemberStudentBLL.UpdateProjectMemberStudent(projectMemberStudent, TXT_UserCode.Text.Trim());

                DataBinder(LB_BelongDepartCode.Text.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGCG").ToString().Trim()+"')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYXGDSDALB").ToString().Trim()+"')", true);
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGSBJCZFHMXWK").ToString().Trim()+"')", true);
        }


    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;


            strHQL = "from ProjectMemberStudent as projectMemberStudent where UserCode = '" + TXT_UserCode.Text.Trim() + "'";

            strHQL = "Delete From T_ProjectMemberStudent Where UserCode = '" + TXT_UserCode.Text.Trim() + "'";

            ShareClass.RunSqlCommand(strHQL);

            DataBinder(LB_BelongDepartCode.Text.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
        }
    }
}