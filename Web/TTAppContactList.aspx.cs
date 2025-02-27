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

public partial class TTAppContactList : System.Web.UI.Page
{
    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName;

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        strRelatedType = "Other";

         if (Page.IsPostBack == false)
        {
            LoadContactList(strUserCode);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = e.Item.Cells[0].Text.Trim();
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strCreatorCode;

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
        NB_Age.Amount = contactInfor.Age;
        TB_EMail1.Text = contactInfor.Email1;
        //TB_EMail2.Text = contactInfor.Email2;
        TB_WebSite.Text = contactInfor.WebSite;
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

        strCreatorCode = contactInfor.UserCode.Trim();

        if (strUserCode == strCreatorCode)
        {
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
        else
        {
            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;
        }

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

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strFirstName, strLastName, strType, strGender, strAge, strEmail1, strEmail2, strWebSite, strMsn, strYsn, strQQ;
        string strOfficePhone, strHomePhone, strMobilePhone, strCompany, strDepartment, strDuty, strCompanyAddress;
        string strCountry, strState, strCity, strPostCode, strHomeAddress;

        string strUserCode = LB_UserCode.Text.Trim();

        strFirstName = TB_FirstName.Text.Trim();
        strLastName = "";
        strType = DL_Type.SelectedValue;
        strGender = DL_Gender.SelectedValue;
        strAge = NB_Age.Amount.ToString();
        strEmail1 = TB_EMail1.Text.Trim();
        //strEmail2 = TB_EMail2.Text.Trim();
        strWebSite = TB_WebSite.Text.Trim();
        //strMsn = TB_Msn.Text.Trim();
        //strYsn = TB_Ysn.Text.Trim();
        strQQ = TB_QQ.Text.Trim();
        strOfficePhone = TB_OfficePhone.Text.Trim();
        strHomePhone = TB_HomePhone.Text.Trim();
        strMobilePhone = TB_MobilePhone.Text.Trim();
        strCompany = TB_Company.Text.Trim();
        strDepartment = TB_Department.Text.Trim();
        strDuty = TB_Duty.Text.Trim();
        strCompany = TB_Company.Text.Trim();
        strDepartment = TB_Department.Text.Trim();
        strDuty = TB_Duty.Text.Trim();
        strCompanyAddress = TB_CompanyAddress.Text.Trim();
        strCountry = TB_Country.Text.Trim();
        strState = TB_State.Text.Trim();
        strCity = TB_City.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strHomeAddress = TB_HomeAddress.Text.Trim();

        ContactInforBLL contactInforBLL = new ContactInforBLL();
        ContactInfor contactInfor = new ContactInfor();

        contactInfor.FirstName = strFirstName;
        contactInfor.LastName = strLastName;
        contactInfor.Type = strType;
        contactInfor.Gender = strGender;
        contactInfor.Age = int.Parse(strAge);
        contactInfor.Email1 = strEmail1;
        contactInfor.Email2 = "";
        contactInfor.WebSite = strWebSite;
        contactInfor.OfficePhone = strOfficePhone;
        contactInfor.HomePhone = strHomePhone;
        contactInfor.MobilePhone = strMobilePhone;
        contactInfor.Msn = "";
        contactInfor.Ysn = "";
        contactInfor.QQ = strQQ;
        contactInfor.Company = strCompany;
        contactInfor.Department = strDepartment;
        contactInfor.Duty = strDuty;
        contactInfor.CompanyAddress = strCompanyAddress;
        contactInfor.Country = strCountry;
        contactInfor.State = strState;
        contactInfor.City = strCity;
        contactInfor.HomeAddress = strHomeAddress;
        contactInfor.PostCode = strPostCode;
        contactInfor.UserCode = strUserCode;
        contactInfor.RelatedType = strRelatedType;
        contactInfor.RelatedID = strRelatedID;

        try
        {
            contactInforBLL.AddContactInfor(contactInfor);

            LB_ID.Text = ShareClass.GetMyCreatedMaxContactInforID(strUserCode);

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            LoadContactList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strFirstName, strLastName, strType, strGender, strAge, strEmail1, strEmail2, strEmail3, strMsn, strYsn, strQQ;
        string strOfficePhone, strHomePhone, strMobilePhone, strCompany, strDepartment, strDuty, strCompanyAddress;
        string strCountry, strState, strCity, strPostCode, strHomeAddress;

        string strUserCode = LB_UserCode.Text.Trim();
        string strID = LB_ID.Text.Trim();

        string strHQL = "from ContactInfor as contactInfor where contactInfor.ID = " + strID;
        ContactInforBLL contactInforBLL = new ContactInforBLL();
        IList lst = contactInforBLL.GetAllContactInfors(strHQL);

        ContactInfor contactInfor = (ContactInfor)lst[0];

        strFirstName = TB_FirstName.Text.Trim();
        strLastName = "";
        strType = DL_Type.SelectedValue;
        strGender = DL_Gender.SelectedValue;
        strAge = NB_Age.Amount.ToString();
        strEmail1 = TB_EMail1.Text.Trim();
        //strEmail2 = TB_EMail2.Text.Trim();
        strEmail3 = TB_WebSite.Text.Trim();
        //strMsn = TB_Msn.Text.Trim();
        //strYsn = TB_Ysn.Text.Trim();
        strQQ = TB_QQ.Text.Trim();
        strOfficePhone = TB_OfficePhone.Text.Trim();
        strHomePhone = TB_HomePhone.Text.Trim();
        strMobilePhone = TB_MobilePhone.Text.Trim();
        strCompany = TB_Company.Text.Trim();
        strDepartment = TB_Department.Text.Trim();
        strDuty = TB_Duty.Text.Trim();
        strCompany = TB_Company.Text.Trim();
        strDepartment = TB_Department.Text.Trim();
        strDuty = TB_Duty.Text.Trim();
        strCompanyAddress = TB_CompanyAddress.Text.Trim();
        strCountry = TB_Country.Text.Trim();
        strState = TB_State.Text.Trim();
        strCity = TB_City.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strHomeAddress = TB_HomeAddress.Text.Trim();

        contactInfor.FirstName = strFirstName;
        contactInfor.LastName = strLastName;
        contactInfor.Type = strType;
        contactInfor.Gender = strGender;
        contactInfor.Age = int.Parse(strAge);
        contactInfor.Email1 = strEmail1;
        contactInfor.Email2 = "";
        contactInfor.WebSite = strEmail3;
        contactInfor.OfficePhone = strOfficePhone;
        contactInfor.HomePhone = strHomePhone;
        contactInfor.MobilePhone = strMobilePhone;
        contactInfor.Msn = "";
        contactInfor.Ysn = "";
        contactInfor.QQ = strQQ;
        contactInfor.Company = strCompany;
        contactInfor.Department = strDepartment;
        contactInfor.Duty = strDuty;
        contactInfor.CompanyAddress = strCompanyAddress;
        contactInfor.Country = strCountry;
        contactInfor.State = strState;
        contactInfor.City = strCity;
        contactInfor.HomeAddress = strHomeAddress;
        contactInfor.PostCode = strPostCode;
        contactInfor.UserCode = strUserCode;
        //contactInfor.RelatedType = strRelatedType;
        //contactInfor.RelatedID = strRelatedID;

        try
        {
            contactInforBLL.UpdateContactInfor(contactInfor, int.Parse(strID));

            LoadContactList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID = LB_ID.Text.Trim();
        string strUserCode = LB_UserCode.Text;

        string strHQL = "from ContactInfor as contactInfor where contactInfor.ID = " + strID;
        ContactInforBLL contactInforBLL = new ContactInforBLL();
        IList lst = contactInforBLL.GetAllContactInfors(strHQL);

        ContactInfor contactInfor = (ContactInfor)lst[0];

        try
        {
            contactInforBLL.DeleteContactInfor(contactInfor);

            LoadContactList(strUserCode);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim()+"')", true);
        }

    }



    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strContactName;

        string strUserCode = LB_UserCode.Text.Trim();


        strContactName = "%" + TB_ContactName.Text.Trim() + "%";


        ContactInforBLL contactInforBLL = new ContactInforBLL();


        if (strRelatedType == "Other")
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'";
            strHQL += " and contactInfor.FirstName Like " + "'" + strContactName + "'";

            strHQL += " order by contactInfor.ID DESC";
        }
        else
        {
            strHQL = "from ContactInfor as contactInfor where ";
            strHQL += " and contactInfor.FirstName Like " + "'" + strContactName + "'";

            strHQL += " and contactInfor.RelatedType=" + "'" + strRelatedType + "'";
            strHQL += " and contactInfor.RelatedID= " + "'" + strRelatedID + "'";
            strHQL += " order by contactInfor.ID DESC";
        }


        lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }


    protected void LoadContactList(string strUserCode)
    {
        string strHQL;
        IList lst;



        strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'" + " order by contactInfor.ID DESC";


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

}
