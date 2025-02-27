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


using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class TTAllUsersForNoUpdatePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //��������Ʒ��jack.erp@gmail.com)
        //Taketop Software 2006��2012

        string strUserCode = Session["UserCode"].ToString();
        string strHQL;
        IList lst;

        string strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�鿴���г�Ա", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}


        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;

            LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB").ToString().Trim();

            strHQL = "from ProjectMember as projectMember ";
            strHQL += " Where projectMember.DepartCode in " + strDepartString;
            strHQL += " and projectMember.Password = '25D55AD283AA400AF464C76D713C07AD'";
            strHQL += " and projectMember.UserCode in (Select systemActiveUser.UserCode From SystemActiveUser as systemActiveUser)";
            strHQL += " Order by projectMember.SortNumber ASC";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + lst.Count.ToString();
            LB_Sql.Text = strHQL;

            strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Male'";
            strHQL += " and projectMember.Password = '25D55AD283AA400AF464C76D713C07AD'";
            strHQL += " and projectMember.DepartCode in " + strDepartString;
            strHQL += " and projectMember.UserCode in (Select systemActiveUser.UserCode From SystemActiveUser as systemActiveUser)";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            LB_MaleUserNumber.Text = lst.Count.ToString();

            strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Female'";
            strHQL += " and projectMember.Password = '25D55AD283AA400AF464C76D713C07AD'";
            strHQL += " and projectMember.DepartCode in " + strDepartString;
            strHQL += " and projectMember.UserCode in (Select systemActiveUser.UserCode From SystemActiveUser as systemActiveUser)";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            LB_FemaleUserNumber.Text = lst.Count.ToString();
        }
    }



    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        LB_ProjectMemberOwner.Text = LanguageHandle.GetWord("SYCYLB").ToString().Trim();

        string strDepartString = LB_DepartString.Text.Trim();

        string strStatus = "%" + DL_Status.SelectedValue + "%";
        string strUserCode = "%" + TB_UserCode.Text.Trim() + "%";
        string strUserName = "%" + TB_UserName.Text.Trim() + "%";

        strHQL = "from ProjectMember as projectMember where ";
        strHQL += " projectMember.UserCode Like " + "'" + strUserCode + "'";
        strHQL += " and projectMember.UserName Like " + "'" + strUserName + "'";
        strHQL += " and projectMember.Status Like " + "'" + strStatus + "'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        strHQL += " and projectMember.Password = '25D55AD283AA400AF464C76D713C07AD'";
        strHQL += " and projectMember.UserCode in (Select systemActiveUser.UserCode From SystemActiveUser as systemActiveUser)";
        strHQL += " Order by projectMember.SortNumber ASC";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_UserNumber.Text = LanguageHandle.GetWord("GCXD").ToString().Trim() + lst.Count.ToString();
        LB_Sql.Text = strHQL;


        strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Male'";
        strHQL += " and projectMember.Password = '25D55AD283AA400AF464C76D713C07AD'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        strHQL += " and projectMember.UserCode in (Select systemActiveUser.UserCode From SystemActiveUser as systemActiveUser)";
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        LB_MaleUserNumber.Text = lst.Count.ToString();

        strHQL = "from ProjectMember as projectMember where projectMember.Gender = 'Female'";
        strHQL += " and projectMember.Password = '25D55AD283AA400AF464C76D713C07AD'";
        strHQL += " and projectMember.DepartCode in " + strDepartString;
        strHQL += " and projectMember.UserCode in (Select systemActiveUser.UserCode From SystemActiveUser as systemActiveUser)";
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        LB_FemaleUserNumber.Text = lst.Count.ToString();

        LB_DepartCode.Text = "";

    }


    protected void BT_ExportToExcel_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName = LanguageHandle.GetWord("YongHuChengYuanXinXi").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                CreateExcel(fileName);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }
        }
    }

    private void CreateExcel(string fileName)
    {
        string strHQL;
        string strDepartCode = LB_DepartCode.Text.Trim();

        if (strDepartCode == "")//���г�Ա�����
        {
            string strDepartString = LB_DepartString.Text.Trim();

            strHQL = string.Format(@"Select UserCode ����,UserName ����,Gender �Ա�,Age ����,DepartCode ���Ŵ���,DepartName ��������,
                Duty ְ��,OfficePhone �칫�绰,MobilePhone �ƶ��绰,EMail EMail,WorkScope ������Χ,JoinDate ��������,Status ״̬,
                RefUserCode �ο�����,IDCard ���֤��,SortNumber ˳���,(case when UserCode in (select UserCode from T_SystemActiveUser) then '�ѿ�ͨ'
								 else 'δ��ͨ' end) Ȩ�� 
                From T_ProjectMember Where DepartCode in ") + strDepartString + " ";

            if (!string.IsNullOrEmpty(DL_Status.SelectedValue.Trim()))
            {
                strHQL += " and Status like '%" + DL_Status.SelectedValue.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(TB_UserCode.Text.Trim()))
            {
                strHQL += " and UserCode like '%" + TB_UserCode.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(TB_UserName.Text.Trim()))
            {
                strHQL += " and UserName like '%" + TB_UserName.Text.Trim() + "%' ";
            }
            strHQL += " and Password = '25D55AD283AA400AF464C76D713C07AD'";
            strHQL += " and UserCode in (Select UserCode From T_SystemActiveUser)";
            strHQL += " Order by SortNumber ASC";
        }
        else//����֯�ܹ���ѯ��
        {
            strHQL = string.Format(@"Select UserCode ����,UserName ����,Gender �Ա�,Age ����,DepartCode ���Ŵ���,DepartName ��������,
                Duty ְ��,OfficePhone �칫�绰,MobilePhone �ƶ��绰,EMail EMail,WorkScope ������Χ,JoinDate ��������,Status ״̬,
                RefUserCode �ο�����,IDCard ���֤��,SortNumber ˳���,(case when UserCode in (select UserCode from T_SystemActiveUser) then '�ѿ�ͨ'
								 else 'δ��ͨ' end) Ȩ�� 
                From T_ProjectMember Where DepartCode = '") + strDepartCode + "' and  Password = '25D55AD283AA400AF464C76D713C07AD'" +
                 " and UserCode in (Select UserCode From T_SystemActiveUser)" +
                " Order by SortNumber ASC ";

        }

        MSExcelHandler.DataTableToExcel(strHQL, fileName); 
    }


    protected string GetUserStatus(string strUserCode)
    {
        string strHQL = "From SystemActiveUser as systemActiveUser where systemActiveUser.UserCode = '" + strUserCode.Trim() + "'";
        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        IList lst = systemActiveUserBLL.GetAllSystemActiveUsers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            return "�ѿ�ͨ"; 
        }
        else
        {
            return "δ��ͨ"; 
        }
    }
}
