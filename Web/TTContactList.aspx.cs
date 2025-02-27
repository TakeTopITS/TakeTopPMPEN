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

using System.Text;
using System.IO;
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTContactList : System.Web.UI.Page
{
    string strRelatedType, strRelatedID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strUserName = ShareClass.GetUserName(strUserCode);

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTContactListSAAS.aspx");
        }


        if (strRelatedType == "Project")
        {
            strRelatedType = "Project";

            //this.Title = strRelatedType + ": " + strRelatedID + "联系人列表";
        }

        if (strRelatedType == "Customer")
        {
            strRelatedType = "Customer";

            //this.Title = strRelatedType + ": " + strRelatedID + " " + GetCustomerName(strRelatedID) + " 联系人列表";
        }

        if (strRelatedType == "Vendor")
        {
            strRelatedType = "Supplier";

            //this.Title = strRelatedType + ": " + strRelatedID + " " + GetVendorName(strRelatedID) + " 联系人列表";
        }

        if (strRelatedType == "Other")
        {
            strRelatedType = "Other";
            //this.Title = "我的通讯薄";
        }

        if (strRelatedType == null)
        {
            //this.Title = "我的通讯薄";
        }


        LB_UserCode.Text = strUserCode;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadContactList(strUserCode, strRelatedType, strRelatedID);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {

        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strCreatorCode;

        string strID = e.Item.Cells[2].Text.Trim();

        if (e.CommandName == "Update")
        {
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
                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;
            }
            else
            {
                //BT_Update.Enabled = false;
                //BT_Delete.Enabled = false;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }

        if (e.CommandName == "Delete")
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.ID = " + strID;
            ContactInforBLL contactInforBLL = new ContactInforBLL();
            lst = contactInforBLL.GetAllContactInfors(strHQL);

            ContactInfor contactInfor = (ContactInfor)lst[0];

            try
            {
                contactInforBLL.DeleteContactInfor(contactInfor);

                LoadContactList(strUserCode, strRelatedType, strRelatedID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            }
        }
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
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddContact();
        }
        else
        {
            UpdateContact();
        }
    }

    protected void AddContact()
    {
        string strFirstName, strLastName, strType, strGender, strAge, strEmail1, strEmail2, strEmail3, strMsn, strYsn, strQQ;
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

        ContactInforBLL contactInforBLL = new ContactInforBLL();
        ContactInfor contactInfor = new ContactInfor();

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
        contactInfor.RelatedType = strRelatedType;
        contactInfor.RelatedID = strRelatedID;

        try
        {
            contactInforBLL.AddContactInfor(contactInfor);

            LB_ID.Text = ShareClass.GetMyCreatedMaxContactInforID(strUserCode);

       
            LoadContactList(strUserCode, strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateContact()
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

            LoadContactList(strUserCode, strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void DL_ContactType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType, strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();

        strType = DL_ContactType.SelectedValue.Trim();

        if (strRelatedType == "Other" | strRelatedType == null)
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode= " + "'" + strUserCode + "'" + " and contactInfor.Type = " + "'" + strType + "'" + " order by contactInfor.ID DESC";
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


        if (strRelatedType == "Other" | strRelatedType == null)
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

        LB_Sql.Text = strHQL;
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadContactList(string strUserCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;


        if (strRelatedType == "Other" | strRelatedType == null)
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.UserCode = " + "'" + strUserCode + "'" + " order by contactInfor.FirstName ASC";
        }
        else
        {
            strHQL = "from ContactInfor as contactInfor where contactInfor.RelatedType=" + "'" + strRelatedType + "'" + " and contactInfor.RelatedID= " + "'" + strRelatedID + "'" + " order by contactInfor.ID ASC";
        }

        ContactInforBLL contactInforBLL = new ContactInforBLL();
        lst = contactInforBLL.GetAllContactInfors(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
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
