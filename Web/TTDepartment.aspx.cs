using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopSecurity;
using TakeTopCore;

public partial class TTDepartment : System.Web.UI.Page
{
    string strCurrentUserType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();
        strCurrentUserType = ShareClass.GetUserType(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "组织架构设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll1", "SaveScroll(Div_TreeView1);");
        //ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll2", "SaveScroll(Div_TreeView2);");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            string strSystemVersionType = Session["SystemVersionType"].ToString();
            if (strUserCode == "ADMIN")
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3);
            }
            else
            {
                if (strSystemVersionType == "GROUP")
                {
                    LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
                    TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
                    TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3, strUserCode);
                }
                else
                {
                    LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                    TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                    TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3);
                }
            }

            LoadProductLineForDataGridByUserCode(strUserCode, DataGrid12);
            LoadProductLineForDataGridByUserCode(strUserCode, DataGrid13);

            try
            {
                if (strSystemVersionType != "GROUP" & strSystemVersionType != "ENTERPRISE")
                {
                    TR_Authority.Visible = false;
                    TR_AuthorityAdd.Visible = false;

                    TABLE_ProductLine.Visible = false;

                    TabPanel9.Visible = false;
                    TabPanel10.Visible = false;
                    TabPanel11.Visible = false;
                    TabPanel12.Visible = false;
                }
            }
            catch
            {

            }
        }
    }
    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            TB_ParentCodeNew.Text = strDepartCode;
            LB_ParentNameNew.Text = ShareClass.GetDepartName(strDepartCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        TB_DepartCodeNew.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strDepartCode;

        strDepartCode = TB_DepartCodeNew.Text.Trim();

        AddDepartment();
    }

    protected void AddDepartment()
    {
        string strHQL;
        string strDepart, strParentDepart;
        string strDepartCode = TB_DepartCodeNew.Text.Trim();
        string strDepartName = TB_DepartNameNew.Text.Trim();
        string strParentCode = TB_ParentCodeNew.Text.Trim();

        string strUserCode = Session["UserCode"].ToString();
        string strAuthority = DL_AuthorityNew.SelectedValue.Trim();
        string strProductLineRelated = DL_DepartProductLineRelated.SelectedValue.Trim();

        string strWorkAddress = TB_WorkAddressNew.Text.Trim();
        string strIsDefaultAddress = DL_IsDefaultWorkAddressNew.SelectedValue.Trim();
        string strLongitude = TB_LongitudeNew.Text.Trim();
        string strLatitude = TB_LatitudeNew.Text.Trim();


        string strDepartString = LB_DepartString.Text.Trim();
        if (TakeTopAuthority.VerifyDepartCode(strParentCode, strUserCode, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSJDM0H1ZNYADMINYHSZBZGKFWJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        if (strDepartCode != "" & strDepartCode != "1" & strDepartCode != "0" & strDepartName != "" & strParentCode != "")
        {
            DepartmentBLL departmentBLL = new DepartmentBLL();
            Department department = new Department();
            department.DepartCode = strDepartCode;
            department.DepartName = strDepartName;
            department.ParentCode = strParentCode;
            department.CreatorCode = strUserCode;
            department.Authority = strAuthority;
            department.ProductLineRelated = strProductLineRelated;

            department.WorkAddress = strWorkAddress;
            department.IsDefaultAddress = strIsDefaultAddress;
            department.Longitude = strLongitude;
            department.Latitude = strLatitude;

            try
            {
                departmentBLL.AddDepartment(department);

                //更新部门其它信息
                UpdateNewDepartmentOtherInformation(strDepartCode);

                TB_DepartCode.Text = TB_DepartCodeNew.Text;
                TB_DepartName.Text = TB_DepartNameNew.Text;
                TB_ParentCode.Text = TB_ParentCodeNew.Text;
                DL_Authority.SelectedValue = DL_AuthorityNew.SelectedValue;

                TB_ContactPerson.Text = TB_ContactPersonNew.Text;
                TB_CompanyAddress.Text = TB_CompanyAddressNew.Text;

                TB_WorkAddress.Text = TB_WorkAddressNew.Text;
                DL_IsDefaultWorkAddress.SelectedValue = DL_IsDefaultWorkAddressNew.SelectedValue;
                TB_Latitude.Text = TB_LatitudeNew.Text;
                TB_Longitude.Text = TB_LongitudeNew.Text;

                if (strUserCode == "ADMIN")
                {
                    LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                    TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                }
                else
                {
                    string strSystemVersionType = Session["SystemVersionType"].ToString();
                    if (strSystemVersionType == "GROUP")
                    {
                        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
                        TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
                    }
                    else
                    {
                        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                        TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                    }
                }

                strHQL = "INSERT INTO T_DepartmentUser(DepartCode,DepartName,UserCode,UserName)";
                strHQL += "Values(" + "'" + strDepartCode + "'" + "," + "'" + strDepartName + "'" + "," + "'" + strUserCode + "'" + "," + "'" + ShareClass.GetUserName(strUserCode) + "'" + ")";
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                LB_DepartCode.Text = strDepartCode;

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                //增加部门到RTX
                try
                {
                    strDepart = strDepartCode + " " + strDepartName;
                    strParentDepart = strParentCode + " " + ShareClass.GetDepartName(strParentCode);

                    ShareClass.AddRTXDepartment(strDepart, strParentDepart);
                }
                catch
                {
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBDMBNZFW0W1YSSYXBNWKJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBDMBNZFW0W1YSSYXBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        int intChildCount, intMemberCount;
        string strHQL;
        IList lst;

        string strOldDepartCode = LB_DepartCode.Text.Trim();

        string strDepartCode = TB_DepartCode.Text.Trim();
        string strDepartName = TB_DepartName.Text.Trim();
        string strParentCode = TB_ParentCode.Text.Trim();
        string strUserCode = Session["UserCode"].ToString();
        string strAuthority = DL_Authority.SelectedValue.Trim();
        string strProductLineRelated = DL_DepartProductLineRelated.SelectedValue.Trim();

        string strWorkAddress = TB_WorkAddress.Text.Trim();
        string strIsDefaultAddress = DL_IsDefaultWorkAddress.SelectedValue.Trim();
        string strLongitude = TB_Longitude.Text.Trim();
        string strLatitude = TB_Latitude.Text.Trim();

        string strDepartString = LB_DepartString.Text.Trim();
        if (TakeTopAuthority.VerifyDepartCode(strParentCode, strUserCode, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSJDM0H1ZNYADMINYHSZBZGKFWJC").ToString().Trim() + "')", true);
            return;
        }

        if (strDepartCode != "" & strDepartCode != "1" & strDepartCode != "0" & strDepartName != "" & strParentCode != "")
        {
            intChildCount = GetChildNodeCount(strDepartCode);
            intMemberCount = GetMemberCount(strDepartCode);

            if (intChildCount == 0 & intMemberCount == 0)
            {
                strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
                DepartmentBLL departmentBLL = new DepartmentBLL();
                lst = departmentBLL.GetAllDepartments(strHQL);

                Department department = (Department)lst[0];
                department.DepartCode = strDepartCode;
                department.DepartName = strDepartName;
                department.ParentCode = strParentCode;
                department.CreatorCode = strUserCode;
                department.Authority = strAuthority;
                department.ProductLineRelated = strProductLineRelated;

                department.WorkAddress = strWorkAddress;
                department.IsDefaultAddress = strIsDefaultAddress;
                department.Longitude = strLongitude;
                department.Latitude = strLatitude;

                try
                {
                    departmentBLL.UpdateDepartment(department, strOldDepartCode);

                    //更新部门其它信息
                    UpdateDepartmentOtherInformation(strOldDepartCode);

                    if (strUserCode == "ADMIN")
                    {
                        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                        TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                    }
                    else
                    {
                        string strSystemVersionType = Session["SystemVersionType"].ToString();
                        if (strSystemVersionType == "GROUP")
                        {
                            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
                            TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
                        }
                        else
                        {
                            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                            TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                        }
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBDMBNZFW0W1YSSYXBNWKJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
                DepartmentBLL departmentBLL = new DepartmentBLL();
                lst = departmentBLL.GetAllDepartments(strHQL);

                Department department = (Department)lst[0];

                department.WorkAddress = strWorkAddress;
                department.IsDefaultAddress = strIsDefaultAddress;
                department.Longitude = strLongitude;
                department.Latitude = strLatitude;

                try
                {
                    departmentBLL.UpdateDepartment(department, strOldDepartCode);

                    //更新部门其它信息
                    UpdateDepartmentOtherInformation(strOldDepartCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCBMCZXJBMHYCYZCBMBNGGZNGGBGDZHJWD").ToString().Trim() + "')", true);

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBDMBNZFW0W1YSSYXBNWKJC").ToString().Trim() + "')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBDMBNZFW0W1YSSYXBNWKJC").ToString().Trim() + "')", true);
        }
    }


    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        int intChildCount, intMemberCount;

        string strDepart;
        string strDepartCode = TB_DepartCode.Text.Trim();
        string strDepartName = TB_DepartName.Text.Trim();
        string strUserCode = Session["UserCode"].ToString();

        intChildCount = GetChildNodeCount(strDepartCode);
        intMemberCount = GetMemberCount(strDepartCode);


        string strDepartString = LB_DepartString.Text.Trim();
        if (TakeTopAuthority.VerifyDepartCode(strDepartCode, strUserCode, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCBMBZNGKFWJC").ToString().Trim() + "')", true);
            return;
        }

        if (intChildCount == 0 & intMemberCount == 0)
        {
            string strHQL = "Delete from T_Department where DepartCode = " + "'" + strDepartCode + "'";

            try
            {
                ShareClass.RunSqlCommand(strHQL);


                if (strUserCode == "ADMIN")
                {
                    LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                    TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                }
                else
                {
                    string strSystemVersionType = Session["SystemVersionType"].ToString();
                    if (strSystemVersionType == "GROUP")
                    {
                        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
                        TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
                    }
                    else
                    {
                        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                        TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                    }
                }

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;
                BT_Adjust.Enabled = false;


                //删除RTX相应部门
                try
                {
                    strDepart = strDepartCode + " " + strDepartName;
                    ShareClass.DeleteRTXDepartment(strDepart);
                }
                catch
                {
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCBMCZXJBMHHYCYZCBMBNSC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_Authority_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strDepartCode = TB_DepartCode.Text.Trim();

        string strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        department.Authority = DL_Authority.SelectedValue.Trim();

        try
        {
            departmentBLL.UpdateDepartment(department, strDepartCode);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXYGG").ToString().Trim() + "')", true);
        }
        catch
        {
        }
    }

    protected void BT_Adjust_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDepartCode = TB_DepartCode1.Text.Trim().ToUpper();
        string strDepartName = TB_DepartName1.Text.Trim();
        string strParentCode = TB_ParentCode1.Text.Trim();
        string strUserCode = Session["UserCode"].ToString();

        string strDepartString = LB_DepartString.Text.Trim();
        if (TakeTopAuthority.VerifyDepartCode(strParentCode, strUserCode, strDepartString) == false)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSJDM0H1ZNYADMINYHSZBZGKFWJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "from Department as department where department.DepartCode = " + "'" + strParentCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        if (lst.Count > 0)
        {
            Department department = (Department)lst[0];

            department.DepartCode = strDepartCode;
            department.DepartName = strDepartName;
            department.ParentCode = strParentCode;

            department.WorkAddress = TB_WorkAddress.Text.Trim();
            department.IsDefaultAddress = DL_IsDefaultWorkAddress.SelectedValue;
            department.Longitude = TB_Longitude.Text.Trim();
            department.Latitude = TB_Latitude.Text.Trim();

            try
            {
                departmentBLL.UpdateDepartment(department, strDepartCode);

                //更新部门其它信息
                UpdateDepartmentOtherInformation(strDepartCode);

                if (strUserCode == "ADMIN")
                {
                    LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                    TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                }
                else
                {
                    string strSystemVersionType = Session["SystemVersionType"].ToString();
                    if (strSystemVersionType == "GROUP")
                    {
                        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
                        TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
                    }
                    else
                    {
                        LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);
                        TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2);
                    }
                }

                ////调整RTX相应部门
                //try
                //{
                //    strDepart = strDepartCode + " " + strDepartName;
                //    ShareClass.DeleteRTXDepartment(strDepart);

                //    strParentDepart = strParentCode + " " + ShareClass.GetDepartName(strParentCode);
                //    ShareClass.AddRTXDepartment(strDepart, strParentDepart);
                //}
                //catch
                //{
                //}

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBMDZCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBMDZSBJCSFMXWK").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSJBMBCZBNDZ").ToString().Trim() + "')", true);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strHQL;
        string strUserCode = Session["UserCode"].ToString();
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            DepartmentBLL departmentBLL = new DepartmentBLL();
            strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
            lst = departmentBLL.GetAllDepartments(strHQL);

            Department department = (Department)lst[0];

            TB_DepartCode.Text = strDepartCode;
            TB_DepartName.Text = department.DepartName.Trim();
            TB_ParentCode.Text = department.ParentCode;

            TB_DepartCode1.Text = strDepartCode;
            TB_DepartName1.Text = department.DepartName.Trim();
            TB_ParentCode1.Text = department.ParentCode;
            DL_Authority.SelectedValue = department.Authority.Trim();
            DL_DepartProductLineRelated.SelectedValue = department.ProductLineRelated.Trim();

            TB_WorkAddress.Text = department.WorkAddress.Trim();
            DL_IsDefaultWorkAddress.SelectedValue = department.IsDefaultAddress.Trim();
            TB_Longitude.Text = department.Longitude.Trim();
            TB_Latitude.Text = department.Latitude.Trim();

            TB_ParentCodeNew.Text = department.DepartCode;

            LB_DepartCode.Text = strDepartCode;

            //取得部门其它信息
            GetDepartmentOtherInformation(strDepartCode);

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
            BT_Adjust.Enabled = true;

            LoadDepartmentUser(strDepartCode);
            LoadDepartAssetRelatedUser(strDepartCode);
            LoadDepartNewsNoticeRelatedUser(strDepartCode);
            LoadDepartUserInforRelatedUser(strDepartCode);
            LoadDepartRelatedProjectLeader(strDepartCode);
            LoadDepartRelatedSuperUser(strDepartCode);
            LoadDepartRelatedWZFeeUser(strDepartCode);
            LoadDepartRelatedWZCheckUser(strDepartCode);
            LoadDepartRelatedWZDelegateUser(strDepartCode);

            LoadDepartRelatedProductLine(strDepartCode);
            LoadDepartSuperUserRelatedProductLine(strDepartCode, strUserCode);
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll(EndRequestHandler1);", true);

    }

    protected void GetDepartmentOtherInformation(string strDepartCode)
    {
        string strHQL1;
        strHQL1 = "Select CompanyAddress,ContactPerson From T_Department Where DepartCode = '" + strDepartCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "T_Department");

        TB_CompanyAddress.Text = ds.Tables[0].Rows[0][0].ToString();
        TB_ContactPerson.Text = ds.Tables[0].Rows[0][1].ToString();
    }

    protected void UpdateDepartmentOtherInformation(string strDepartCode)
    {
        string strHQL1;
        strHQL1 = "Update T_Department Set CompanyAddress = '" + TB_CompanyAddress.Text.Trim() + "',ContactPerson = '" + TB_ContactPerson.Text.Trim() + "' Where DepartCode = '" + strDepartCode + "'";
        ShareClass.RunSqlCommand(strHQL1);
    }

    protected void UpdateNewDepartmentOtherInformation(string strDepartCode)
    {
        string strHQL1;
        strHQL1 = "Update T_Department Set CompanyAddress = '" + TB_CompanyAddressNew.Text.Trim() + "',ContactPerson = '" + TB_ContactPersonNew.Text.Trim() + "' Where DepartCode = '" + strDepartCode + "'";
        ShareClass.RunSqlCommand(strHQL1);
    }


    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }

        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll(EndRequestHandler2);", true);

    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;

            if (e.CommandName == "Delete")
            {
                strHQL = "Delete From T_DepartAssetRelatedUser Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                LoadDepartAssetRelatedUser(strDepartCode);
            }
            else
            {

            }
        }
    }

    protected void DL_PriceVisible_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 选择用户   semir.customClass.database dbnew = new semir.customClass.database();
        DropDownList DrplRole = (DropDownList)sender;
        TableCell cell = (TableCell)DrplRole.Parent;
        DataGridItem item = (DataGridItem)cell.Parent;

        string strDepartCode = TB_DepartCode.Text.Trim();

        string StrPower = ((DropDownList)sender).SelectedItem.ToString(); // 取出DropDownList选中项文本
        int ITid = Convert.ToInt32(item.Cells[0].Text);    // 取出该行的第一格的数据（主键）
        string sqlUpPower = "update T_DepartAssetRelatedUser set Pricevisuable ='" + StrPower + "' where ID='" + ITid + "'";
        ShareClass.RunSqlCommand(sqlUpPower);

        //LoadDepartAssetRelatedUser(strDepartCode);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            strHQL = "from DepartmentUser as departmentUser where departmentUser.ID = " + strID;
            DepartmentUserBLL departmentUserBLL = new DepartmentUserBLL();
            lst = departmentUserBLL.GetAllDepartmentUsers(strHQL);

            DepartmentUser departmentUser = (DepartmentUser)lst[0];

            try
            {
                departmentUserBLL.DeleteDepartmentUser(departmentUser);
                LoadDepartmentUser(strDepartCode);
            }
            catch
            {
            }
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strDepartCode, strDepartName;

        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        if (e.CommandName != "Page")
        {
            strDepartCode = TB_DepartCode.Text.Trim();
            strDepartName = TB_DepartName.Text.Trim();

            if (TabContainer1.ActiveTabIndex == 0)
            {
                DepartmentUserBLL departmentUserBLL = new DepartmentUserBLL();
                DepartmentUser departmentUser = new DepartmentUser();

                departmentUser.DepartCode = strDepartCode;
                departmentUser.DepartName = strDepartName;
                departmentUser.UserCode = strUserCode;
                departmentUser.UserName = strUserName;

                try
                {
                    departmentUserBLL.AddDepartmentUser(departmentUser);
                    LoadDepartmentUser(strDepartCode);
                }
                catch
                {
                }
            }



            if (TabContainer1.ActiveTabIndex == 1)
            {
                DepartNewsNoticeRelatedUserBLL departNewsNoticeRelatedUserBLL = new DepartNewsNoticeRelatedUserBLL();
                DepartNewsNoticeRelatedUser departNewsNoticeRelatedUser = new DepartNewsNoticeRelatedUser();

                departNewsNoticeRelatedUser.DepartCode = strDepartCode;
                departNewsNoticeRelatedUser.UserCode = strUserCode;
                departNewsNoticeRelatedUser.UserName = strUserName;
                departNewsNoticeRelatedUser.EffectDate = DateTime.Now;

                try
                {
                    departNewsNoticeRelatedUserBLL.AddDepartNewsNoticeRelatedUser(departNewsNoticeRelatedUser);
                    LoadDepartNewsNoticeRelatedUser(strDepartCode);
                }
                catch
                {
                }
            }

            if (TabContainer1.ActiveTabIndex == 2)
            {
                DepartUserInforRelatedUserBLL departUserInforRelatedUserBLL = new DepartUserInforRelatedUserBLL();
                DepartUserInforRelatedUser departUserInforRelatedUser = new DepartUserInforRelatedUser();

                departUserInforRelatedUser.DepartCode = strDepartCode;
                departUserInforRelatedUser.UserCode = strUserCode;
                departUserInforRelatedUser.UserName = strUserName;
                departUserInforRelatedUser.EffectDate = DateTime.Now;

                try
                {
                    departUserInforRelatedUserBLL.AddDepartUserInforRelatedUser(departUserInforRelatedUser);
                    LoadDepartUserInforRelatedUser(strDepartCode);
                }
                catch
                {
                }
            }

            if (TabContainer1.ActiveTabIndex == 3)
            {
                DepartAssetRelatedUserBLL departAssetRelatedUserBLL = new DepartAssetRelatedUserBLL();
                DepartAssetRelatedUser departAssetRelatedUser = new DepartAssetRelatedUser();

                departAssetRelatedUser.DepartCode = strDepartCode;
                departAssetRelatedUser.UserCode = strUserCode;
                departAssetRelatedUser.UserName = strUserName;
                departAssetRelatedUser.EffectDate = DateTime.Now;

                try
                {
                    departAssetRelatedUserBLL.AddDepartAssetRelatedUser(departAssetRelatedUser);
                    LoadDepartAssetRelatedUser(strDepartCode);
                }
                catch
                {
                }
            }

            if (TabContainer1.ActiveTabIndex == 4)
            {
                DepartRelatedProjectLeaderBLL departRelatedProjectLeaderBLL = new DepartRelatedProjectLeaderBLL();
                DepartRelatedProjectLeader departRelatedProjectLeader = new DepartRelatedProjectLeader();

                departRelatedProjectLeader.DepartCode = strDepartCode;
                departRelatedProjectLeader.UserCode = strUserCode;
                departRelatedProjectLeader.UserName = strUserName;
                departRelatedProjectLeader.EffectDate = DateTime.Now;

                try
                {
                    departRelatedProjectLeaderBLL.AddDepartRelatedProjectLeader(departRelatedProjectLeader);
                    LoadDepartRelatedProjectLeader(strDepartCode);
                }
                catch
                {
                }
            }

            if (TabContainer1.ActiveTabIndex == 5)
            {
                DepartRelatedSuperUserBLL departRelatedSuperUserBLL = new DepartRelatedSuperUserBLL();
                DepartRelatedSuperUser departRelatedSuperUser = new DepartRelatedSuperUser();

                departRelatedSuperUser.DepartCode = strDepartCode;
                departRelatedSuperUser.UserCode = strUserCode;
                departRelatedSuperUser.UserName = strUserName;
                departRelatedSuperUser.EffectDate = DateTime.Now;
                departRelatedSuperUser.ProductLineRelated = "NO";

                try
                {
                    departRelatedSuperUserBLL.AddDepartRelatedSuperUser(departRelatedSuperUser);
                    LoadDepartRelatedSuperUser(strDepartCode);
                }
                catch
                {
                }
            }

            if (TabContainer1.ActiveTabIndex == 6)
            {
                DepartRelatedWZFeeUserBLL departRelatedWZFeeUserBLL = new DepartRelatedWZFeeUserBLL();
                DepartRelatedWZFeeUser departRelatedWZFeeUser = new DepartRelatedWZFeeUser();

                departRelatedWZFeeUser.DepartCode = strDepartCode;
                departRelatedWZFeeUser.UserCode = strUserCode;
                departRelatedWZFeeUser.UserName = strUserName;
                departRelatedWZFeeUser.EffectDate = DateTime.Now;

                try
                {
                    departRelatedWZFeeUserBLL.AddDepartRelatedWZFeeUser(departRelatedWZFeeUser);
                    LoadDepartRelatedWZFeeUser(strDepartCode);
                }
                catch
                {
                }
            }

            if (TabContainer1.ActiveTabIndex == 7)
            {
                DepartRelatedWZCheckUserBLL departRelatedWZFeeUserBLL = new DepartRelatedWZCheckUserBLL();
                DepartRelatedWZCheckUser departRelatedWZCheckUser = new DepartRelatedWZCheckUser();

                departRelatedWZCheckUser.DepartCode = strDepartCode;
                departRelatedWZCheckUser.UserCode = strUserCode;
                departRelatedWZCheckUser.UserName = strUserName;
                departRelatedWZCheckUser.EffectDate = DateTime.Now;

                try
                {
                    departRelatedWZFeeUserBLL.AddDepartRelatedWZCheckUser(departRelatedWZCheckUser);
                    LoadDepartRelatedWZCheckUser(strDepartCode);
                }
                catch
                {
                }
            }

            if (TabContainer1.ActiveTabIndex == 8)
            {
                DepartRelatedWZDelegateUserBLL departRelatedWZDelegateUserBLL = new DepartRelatedWZDelegateUserBLL();
                DepartRelatedWZDelegateUser departRelatedWZDelegateUser = new DepartRelatedWZDelegateUser();

                departRelatedWZDelegateUser.DepartCode = strDepartCode;
                departRelatedWZDelegateUser.UserCode = strUserCode;
                departRelatedWZDelegateUser.UserName = strUserName;
                departRelatedWZDelegateUser.EffectDate = DateTime.Now;

                try
                {
                    departRelatedWZDelegateUserBLL.AddDepartRelatedWZDelegateUser(departRelatedWZDelegateUser);
                    LoadDepartRelatedWZDelegateUser(strDepartCode);
                }
                catch
                {
                }
            }
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;


            strHQL = "Delete From T_DepartNewsNoticeRelatedUser Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartNewsNoticeRelatedUser(strDepartCode);
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;


            strHQL = "Delete From T_DepartUserInforRelatedUser Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartUserInforRelatedUser(strDepartCode);
        }
    }

    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strID = ((Button)e.Item.FindControl("BT_SuperUserID")).Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();
        string strUserCode = e.Item.Cells[1].Text;
        string strUserName = e.Item.Cells[2].Text;
        string strProductLineRelated = e.Item.Cells[3].Text.Trim();

        for (int i = 0; i < DataGrid7.Items.Count; i++)
        {
            DataGrid7.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        LB_SuperUserID.Text = strID;
        LB_SuperUserCode.Text = strUserCode;
        LB_SuperUserName.Text = strUserName;
        DL_SuperUserProductLineRelated.SelectedValue = strProductLineRelated;

        if (e.CommandName == "Delete")
        {
            strHQL = "Delete From T_DepartRelatedSuperUser Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LB_SuperUserID.Text = "";
            LB_SuperUserCode.Text = "";
            LB_SuperUserName.Text = "";
            DL_SuperUserProductLineRelated.SelectedValue = "NO";

            LoadDepartRelatedSuperUser(strDepartCode);
            LoadDepartSuperUserRelatedProductLine(strDepartCode, strUserCode);
        }
        else
        {
            LoadDepartSuperUserRelatedProductLine(strDepartCode, strUserCode);
        }
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;


            strHQL = "Delete From T_DepartRelatedProjectLeader Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartRelatedProjectLeader(strDepartCode);
        }
    }

    protected void DataGrid9_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            strHQL = "Delete From T_DepartRelatedWZFeeUser Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartRelatedWZFeeUser(strDepartCode);
        }
    }

    protected void DataGrid10_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;


            strHQL = "Delete From T_DepartRelatedWZCheckUser Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartRelatedWZCheckUser(strDepartCode);
        }
    }

    protected void DataGrid11_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;


            strHQL = "Delete From T_DepartRelatedWZDelegateUser Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartRelatedWZDelegateUser(strDepartCode);
        }
    }

    protected void DL_DepartProductLineRelated_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strDepartCode, strProductLineRelated;

        strDepartCode = TB_DepartCode.Text.Trim();

        strProductLineRelated = DL_DepartProductLineRelated.SelectedValue.Trim();

        strHQL = "Update T_Department Set ProductLineRelated = " + "'" + strProductLineRelated + "'" + " Where DepartCode = " + "'" + strDepartCode + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DL_SuperUserProductLineRelated_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strID, strDepartCode, strProductLineRelated;

        strID = LB_SuperUserID.Text.Trim();
        strDepartCode = TB_DepartCode.Text.Trim();

        strProductLineRelated = DL_SuperUserProductLineRelated.SelectedValue.Trim();

        strHQL = "Update T_DepartRelatedSuperUser Set ProductLineRelated = " + "'" + strProductLineRelated + "'" + " Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void DataGrid12_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strProductLineName = ((Button)e.Item.FindControl("BT_ProductLineName")).Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;

            strHQL = "Insert Into T_DepartRelatedProductLine(DepartCode,ProductLineName) Values ( " + "'" + strDepartCode + "'" + "," + "'" + strProductLineName + "'" + ")";
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartRelatedProductLine(strDepartCode);
        }
    }

    protected void RP_DepartProductLine_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        if (e.CommandName != "Page")
        {
            string strDepartCode = TB_DepartCode.Text.Trim();
            string strProductLineName = ((Button)e.Item.FindControl("BT_ProductLineName")).Text.Trim();

            string strHQL;

            strHQL = "Delete From T_DepartRelatedProductLine Where ProductLineName = " + "'" + strProductLineName + "'";
            strHQL += " and DepartCode = " + "'" + strDepartCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartRelatedProductLine(strDepartCode);
        }
    }

    protected void DataGrid13_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strProductLineName = ((Button)e.Item.FindControl("BT_ProductLineName")).Text.Trim();
        string strDepartCode = TB_DepartCode.Text.Trim();
        string strUserCode = LB_SuperUserCode.Text.Trim();

        if (e.CommandName != "Page")
        {
            string strHQL;

            if (strUserCode != "")
            {
                strHQL = "Insert Into T_DepartSuperUserRelatedProductLine(DepartCode,UserCode,ProductLineName) Values ( " + "'" + strDepartCode + "'" + "," + "'" + strUserCode + "'" + "," + "'" + strProductLineName + "'" + ")";
                ShareClass.RunSqlCommand(strHQL);

                LoadDepartSuperUserRelatedProductLine(strDepartCode, strUserCode);
            }
        }
    }

    protected void RP_SuperUserProductLine_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;

            string strDepartCode = TB_DepartCode.Text.Trim();
            string strUserCode = LB_SuperUserCode.Text.Trim();

            string strProductLineName = ((Button)e.Item.FindControl("BT_ProductLineName")).Text.Trim();

            strHQL = "Delete From T_DepartSuperUserRelatedProductLine Where ProductLineName = " + "'" + strProductLineName + "'";
            strHQL += " and DepartCode = " + "'" + strDepartCode + "'" + " and UserCode = " + "'" + strUserCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadDepartSuperUserRelatedProductLine(strDepartCode, strUserCode);
        }
    }

    protected void LoadDepartmentUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartmentUser as departmentUser where departmentUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentUserBLL departmentUserBLL = new DepartmentUserBLL();
        lst = departmentUserBLL.GetAllDepartmentUsers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadDepartAssetRelatedUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        string strVisible;

        strHQL = "from DepartAssetRelatedUser as departAssetRelatedUser where departAssetRelatedUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartAssetRelatedUserBLL departAssetRelateduserBLL = new DepartAssetRelatedUserBLL();
        lst = departAssetRelateduserBLL.GetAllDepartAssetRelatedUsers(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        //for (int i = 0; i < lst.Count; i++)
        //{
        //    strVisible = ((DepartAssetRelatedUser)lst[i]).PriceVisuable.Trim();

        //    ((DropDownList)DataGrid1.Items[i].FindControl("DL_PriceVisible")).SelectedValue = strVisible;
        //}
    }

    protected void LoadDepartNewsNoticeRelatedUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartNewsNoticeRelatedUser as departNewsNoticeRelatedUser where departNewsNoticeRelatedUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartNewsNoticeRelatedUserBLL departAssetRelateduserBLL = new DepartNewsNoticeRelatedUserBLL();
        lst = departAssetRelateduserBLL.GetAllDepartNewsNoticeRelatedUsers(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadDepartUserInforRelatedUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartUserInforRelatedUser as departUserInforRelatedUser where departUserInforRelatedUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartUserInforRelatedUserBLL departAssetRelateduserBLL = new DepartUserInforRelatedUserBLL();
        lst = departAssetRelateduserBLL.GetAllDepartUserInforRelatedUsers(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void LoadDepartRelatedProjectLeader(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartRelatedProjectLeader as departRelatedProjectLeader where departRelatedProjectLeader.DepartCode = " + "'" + strDepartCode + "'";
        DepartRelatedProjectLeaderBLL departAssetRelateduserBLL = new DepartRelatedProjectLeaderBLL();
        lst = departAssetRelateduserBLL.GetAllDepartRelatedProjectLeaders(strHQL);

        DataGrid8.DataSource = lst;
        DataGrid8.DataBind();
    }

    protected void LoadDepartRelatedSuperUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartRelatedSuperUser as departRelatedSuperUser where departRelatedSuperUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartRelatedSuperUserBLL departAssetRelateduserBLL = new DepartRelatedSuperUserBLL();
        lst = departAssetRelateduserBLL.GetAllDepartRelatedSuperUsers(strHQL);

        DataGrid7.DataSource = lst;
        DataGrid7.DataBind();
    }

    protected void LoadDepartRelatedWZFeeUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartRelatedWZFeeUser as departRelatedWZFeeUser where departRelatedWZFeeUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartRelatedWZFeeUserBLL departRelatedWZFeeUserBLL = new DepartRelatedWZFeeUserBLL();
        lst = departRelatedWZFeeUserBLL.GetAllDepartRelatedWZFeeUsers(strHQL);

        DataGrid9.DataSource = lst;
        DataGrid9.DataBind();
    }

    protected void LoadDepartRelatedWZCheckUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartRelatedWZCheckUser as departRelatedWZCheckUser where departRelatedWZCheckUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartRelatedWZCheckUserBLL departRelatedWZCheckUserBLL = new DepartRelatedWZCheckUserBLL();
        lst = departRelatedWZCheckUserBLL.GetAllDepartRelatedWZCheckUsers(strHQL);

        DataGrid10.DataSource = lst;
        DataGrid10.DataBind();
    }

    protected void LoadDepartRelatedWZDelegateUser(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from DepartRelatedWZDelegateUser as departRelatedWZDelegateUser where departRelatedWZDelegateUser.DepartCode = " + "'" + strDepartCode + "'";
        DepartRelatedWZDelegateUserBLL DepartRelatedWZDelegateUserBLL = new DepartRelatedWZDelegateUserBLL();
        lst = DepartRelatedWZDelegateUserBLL.GetAllDepartRelatedWZDelegateUsers(strHQL);

        DataGrid11.DataSource = lst;
        DataGrid11.DataBind();
    }

    protected void LoadDepartRelatedProductLine(string strDepartCode)
    {
        string strHQL;

        strHQL = "Select ProductLineName From T_DepartRelatedProductLine Where DepartCode = " + "'" + TB_DepartCode.Text.Trim() + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartProductLine");
        RP_DepartProductLine.DataSource = ds;
        RP_DepartProductLine.DataBind();
    }

    protected void LoadDepartSuperUserRelatedProductLine(string strDepartCode, string strUserCode)
    {
        string strHQL;

        strHQL = "Select ProductLineName From T_DepartSuperUserRelatedProductLine Where DepartCode = " + "'" + TB_DepartCode.Text.Trim() + "'" + " and UserCode = " + "'" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartSuperUserProductLine");
        RP_SuperUserProductLine.DataSource = ds;
        RP_SuperUserProductLine.DataBind();
    }

    protected void LoadProductLineForDataGridByUserCode(string strUserCode, DataGrid DataGrid1)
    {
        string strHQL;
        string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        string strProductLineRelated = ShareClass.GetDepartRelatedProductLineFromUserCode(strUserCode);

        if (strUserCode == "ADMIN" | strProductLineRelated == "NO")
        {
            strHQL = "select Name  from T_ProjectProductLine_YYUP";
        }
        else
        {
            strHQL = "select Name  from T_ProjectProductLine_YYUP Where ";
            strHQL += " Name in (Select ProductLineName From T_DepartRelatedProductLine Where DepartCode = " + "'" + strDepartCode + "'" + ")";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartProductLine");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected int GetChildNodeCount(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Department as department where department.ParentCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        return lst.Count;
    }

    protected int GetMemberCount(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        return lst.Count;
    }


}
