using System; using System.Resources;
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

using System.Text;
using System.IO;
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTContactListView : System.Web.UI.Page
{
    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = GetUserName(strUserCode);

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";

            //this.Title = LanguageHandle.GetWord("Project") + strRelatedType + " " + strRelatedID + "联系人列表";
        }

        if (strRelatedType == "Customer")
        {
            strRelatedType = "Customer";

            //this.Title = strRelatedType + " " + strRelatedID + " " + GetCustomerName(strRelatedID) + " 联系人列表";
        }

        if (strRelatedType == "Vendor")
        {
            strRelatedType = "Supplier";

            //this.Title = strRelatedType + " " + strRelatedID + " " + GetVendorName(strRelatedID) + " 联系人列表";
        }

        if (strRelatedType == "Other")
        {
            strRelatedType = "Other";

            //this.Title = "我的通讯薄";
        }


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LB_UserCode.Text = strUserCode;

            LoadContactList(strUserCode, strRelatedType, strRelatedID);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strHQL;
        IList lst;

        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            DataGrid1.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        strHQL = "from ContactInfor as contactInfor where contactInfor.ID = " + strID;
        ContactInforBLL contactInforBLL = new ContactInforBLL();
        lst = contactInforBLL.GetAllContactInfors(strHQL);

        ContactInfor contactInfor = (ContactInfor)lst[0];

        LB_ID.Text = contactInfor.ID.ToString();
        TB_FirstName.Text = contactInfor.FirstName;
      
        DL_Type.SelectedValue = contactInfor.Type.Trim();
        DL_Gender.SelectedValue = contactInfor.Gender.Trim();
        TB_Age.Text = contactInfor.Age.ToString();
        TB_EMail1.Text = contactInfor.Email1;
        //TB_EMail2.Text = contactInfor.Email2;
        TB_WebSite.Text = contactInfor.WebSite.Trim();
        TB_OfficePhone.Text = contactInfor.OfficePhone;
        TB_HomePhone.Text = contactInfor.HomePhone;
        TB_MobilePhone.Text = contactInfor.MobilePhone;
        //TB_Msn.Text = contactInfor.Msn;
        //TB_Ysn.Text = contactInfor.Ysn;
        TB_QQ.Text = contactInfor.QQ;
        TB_Company.Text = contactInfor.Company;
        TB_Department.Text = contactInfor.Department;
        TB_Duty.Text = contactInfor.Duty;
        TB_CompanyAddress.Text = contactInfor.CompanyAddress;
        TB_Country.Text = contactInfor.Country;
        TB_State.Text = contactInfor.State;
        TB_City.Text = contactInfor.City;
        TB_PostCode.Text = contactInfor.PostCode;
        TB_HomeAddress.Text = contactInfor.HomeAddress;

        TabContainer1.ActiveTabIndex = 1;

    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ContactInforBLL contactInforBLL = new ContactInforBLL();
        IList lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    

    protected void DL_ContactType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType, strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        strType = DL_ContactType.SelectedValue.Trim();

        if (strRelatedType == "Other")
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode= " + "'" + strUserCode + "'" + " and contactInfor.Type = " + "'" + strType + "'" + " and contactInfor.RelatedType=" + "'" + strRelatedType + "'" + " and contactInfor.RelatedID= " + "'" + strRelatedID + "'" + " order by contactInfor.ID DESC";
        }
        else
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.Type = " + "'" + strType + "'" + " and contactInfor.RelatedType=" + "'" + strRelatedType + "'" + " and contactInfor.RelatedID= " + "'" + strRelatedID + "'" + " order by contactInfor.ID DESC";
        }

        ContactInforBLL contactInforBLL = new ContactInforBLL();
        lst = contactInforBLL.GetAllContactInfors(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

 
    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strContactName, strCompanyName, strContactType;

        string strUserCode = LB_UserCode.Text.Trim();

        strContactType = "%" + DL_ContactType.SelectedValue.Trim() + "%";
        strContactName = "%" + TB_ContactName.Text.Trim() + "%";
        strCompanyName = "%" + TB_CompanyName.Text.Trim() + "%";

        ContactInforBLL contactInforBLL = new ContactInforBLL();


        if (strRelatedType == "Other")
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'";
            strHQL += " and contactInfor.FirstName Like " + "'" + strContactName + "'";
            strHQL += " and contactInfor.Company like " + "'" + strCompanyName + "'";
            strHQL += " and contactInfor.Type Like " + "'" + strContactType + "'";
            strHQL += " order by contactInfor.ID DESC";
        }
        else
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.Type Like " + "'" + strContactType + "'";
            strHQL += " and contactInfor.FirstName Like " + "'" + strContactName + "'";
            strHQL += " and contactInfor.Company like " + "'" + strCompanyName + "'";
            strHQL += " and contactInfor.RelatedType=" + "'" + strRelatedType + "'";
            strHQL += " and contactInfor.RelatedID= " + "'" + strRelatedID + "'";
            strHQL += " order by contactInfor.ID DESC";
        }


        lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }




    protected void LoadContactList(string strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        if (strRelatedType == "Other")
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'" + " and contactInfor.RelatedType=" + "'" + strRelatedType + "'" + " and contactInfor.RelatedID= " + "'" + strRelatedID + "'" + " order by contactInfor.ID DESC";
        }
        else
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.RelatedType=" + "'" + strRelatedType + "'" + " and contactInfor.RelatedID= " + "'" + strRelatedID + "'" + " order by contactInfor.ID DESC";
        }
        ContactInforBLL contactInforBLL = new ContactInforBLL();
        lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected string GetCustomerName(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Customer as customer where customer.CustomerCode = " + "'" + strCustomerCode + "'";
        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        Customer customer = (Customer)lst[0];

        return customer.CustomerName.Trim();
    }

    protected string GetVendorName(string strVendorCode)
    {
        string strHQL;
        IList lst;


        strHQL = "from Vendor as vendor where vendor.VendorCode = " + "'" + strVendorCode + "'";
        VendorBLL vendorBLL = new VendorBLL();
        lst = vendorBLL.GetAllVendors(strHQL);

        Vendor vendor = (Vendor)lst[0];

        return vendor.VendorName.Trim();
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }
   
}
