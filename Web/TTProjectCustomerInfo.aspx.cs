using System;
using System.Resources;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Xml.Linq;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProjectCustomerInfo : System.Web.UI.Page
{
    string strProjectID;
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strProjectID = Request.QueryString["ProjectID"].Trim();

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + project.ProjectName + "客户资料";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LoadIndustryType();

            LoadCustomerList(strProjectID);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_CustomerCode.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strCustomerCode;

        if (e.CommandName != "Page")
        {
            strCustomerCode = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from Customer as customer where customer.CustomerCode = " + "'" + strCustomerCode + "'";
                CustomerBLL customerBLL = new CustomerBLL();
                lst = customerBLL.GetAllCustomers(strHQL);

                Customer customer = (Customer)lst[0];

                TB_CustomerCode.Text = customer.CustomerCode;
                TB_CustomerName.Text = customer.CustomerName;
                TB_CustomerEnglishName.Text = customer.CustomerEnglishName;
                TB_Type.Text = customer.Type.Trim();
                TB_ContactName.Text = customer.ContactName;
                TB_SalePerson.Text = customer.SalesPerson;
                TB_EMailAddress.Text = customer.EmailAddress;
                TB_WebSite.Text = customer.WebSite;
                TB_Tel1.Text = customer.Tel1;
                TB_Tel2.Text = customer.Tel2;
                TB_ZP.Text = customer.ZP;
                TB_Fax.Text = customer.Fax;
                TB_InvoiceAddress.Text = customer.InvoiceAddress;
                TB_Bank.Text = customer.Bank;
                TB_BankAccount.Text = customer.BankAccount;
                DL_Currency.SelectedValue = customer.Currency.Trim();
                TB_Country.Text = customer.Country;
                TB_State.Text = customer.State;
                TB_City.Text = customer.City;
                TB_AddressCN.Text = customer.RegistrationAddressCN;
                TB_AddressEN.Text = customer.RegistrationAddressEN;
                LB_CreateDate.Text = customer.CreateDate.ToString();
                TB_Comment.Text = customer.Comment;

                NB_CreditRate.Amount = customer.CreditRate;
                NB_Discount.Amount = customer.Discount;

                HL_RelatedContactInfor.Enabled = true;
                HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;

                LB_CustomerCode.Text = strCustomerCode;
           
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                CustomerBLL customerBLL = new CustomerBLL();
                Customer customer = new Customer();

                customer.CustomerCode = strCustomerCode;

                try
                {
                    customerBLL.DeleteCustomer(customer);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);

                    LoadCustomerList(strProjectID);

                    HL_RelatedContactInfor.Enabled = false;
                    HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void DL_IndustryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_IndustryType.SelectedValue.Trim();

        TB_Type.Text = strType;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }
    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strCustomerCode;

        strCustomerCode = LB_CustomerCode.Text.Trim();

        if (strCustomerCode == "")
        {
            AddCustomer();
        }
        else
        {
            UpdateCustomer();
        }
    }

    protected void AddCustomer()
    {
        string strCustomerCode, strCustomerName, strType, strContactName, strSalesPerson;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strRegistrationAddressCN, strRegistrationAddressEN;
        string strCustomerEnglishName, strComment;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount;

        strCustomerCode = TB_CustomerCode.Text.Trim();
        strCustomerName = TB_CustomerName.Text.Trim();
        strCustomerEnglishName = TB_CustomerEnglishName.Text.Trim();
        strType = TB_Type.Text.Trim();
        strContactName = TB_ContactName.Text.Trim();
        strSalesPerson = TB_SalePerson.Text.Trim();
        strInvoiceAddress = TB_InvoiceAddress.Text.Trim();
        strBank = TB_Bank.Text.Trim();
        strBankAccount = TB_BankAccount.Text.Trim();
        strCurrency = DL_Currency.SelectedValue;
        strTel1 = TB_Tel1.Text.Trim();
        strTel2 = TB_Tel2.Text.Trim();
        strFax = TB_Fax.Text.Trim();
        strEmailAddress = TB_EMailAddress.Text.Trim();
        strWebSite = TB_WebSite.Text.Trim();
        strZP = TB_ZP.Text.Trim();
        strCountry = TB_Country.Text.Trim();
        strState = TB_State.Text.Trim();
        strCity = TB_City.Text.Trim();
        strRegistrationAddressCN = TB_AddressCN.Text.Trim();
        strRegistrationAddressEN = TB_AddressEN.Text.Trim();

        dtCreateDate = DateTime.Now;

        deDiscount = NB_Discount.Amount;
        deCreditRate = NB_CreditRate.Amount;

        strComment = TB_Comment.Text.Trim();

        if (strCustomerCode != "" | strCustomerName != "")
        {
            CustomerBLL customerBLL = new CustomerBLL();
            Customer customer = new Customer();

            customer.CustomerCode = strCustomerCode;
            customer.CustomerName = strCustomerName;
            customer.CustomerEnglishName = strCustomerEnglishName;
            customer.CustomerEnglishName = strCustomerEnglishName;
            customer.Type = strType;
            customer.ContactName = strContactName;
            customer.SalesPerson = strSalesPerson;
            customer.InvoiceAddress = strInvoiceAddress;
            customer.Bank = strBank;
            customer.BankAccount = strBankAccount;
            customer.Currency = strCurrency;
            customer.Tel1 = strTel1;
            customer.Tel2 = strTel2;
            customer.Fax = strFax;
            customer.EmailAddress = strEmailAddress;
            customer.WebSite = strWebSite;
            customer.ZP = strZP;
            customer.Country = strCountry;
            customer.State = strState;
            customer.City = strCity;
            customer.RegistrationAddressCN = strRegistrationAddressCN;
            customer.RegistrationAddressEN = strRegistrationAddressEN;
            customer.CreditRate = deCreditRate;
            customer.CreateDate = DateTime.Now;
            customer.Discount = deDiscount;
            customer.Comment = strComment;
            customer.CreatorCode = strUserCode;

            try
            {
                customerBLL.AddCustomer(customer);

                ProjectCustomerBLL projectCustomerBLL = new ProjectCustomerBLL();
                ProjectCustomer projectCustomer = new ProjectCustomer();

                projectCustomer.ProjectID = int.Parse(strProjectID);
                projectCustomer.CustomerCode = strCustomerCode;

                try
                {
                    projectCustomerBLL.AddProjectCustomer(projectCustomer);

                    LoadCustomerList(strProjectID);
                   
                    HL_RelatedContactInfor.Enabled = true;
                    HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString() + "')", true);
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + ex.Message.ToString () + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKHDMHMCBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateCustomer()
    {
        string strHQL;
        IList lst;

        string strCustomerCode, strCustomerName, strType, strContactName, strSalesPerson;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strRegistrationAddressCN, strRegistrationAddressEN;
        string strCustomerEnglishName, strComment;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount;

        strCustomerCode = TB_CustomerCode.Text.Trim();
        strCustomerName = TB_CustomerName.Text.Trim();
        strCustomerEnglishName = TB_CustomerEnglishName.Text.Trim();
        strType = TB_Type.Text.Trim();
        strContactName = TB_ContactName.Text.Trim();
        strSalesPerson = TB_SalePerson.Text.Trim();
        strInvoiceAddress = TB_InvoiceAddress.Text.Trim();
        strBank = TB_Bank.Text.Trim();
        strBankAccount = TB_BankAccount.Text.Trim();
        strCurrency = DL_Currency.SelectedValue;
        strTel1 = TB_Tel1.Text.Trim();
        strTel2 = TB_Tel2.Text.Trim();
        strFax = TB_Fax.Text.Trim();
        strEmailAddress = TB_EMailAddress.Text.Trim();
        strWebSite = TB_WebSite.Text.Trim();
        strZP = TB_ZP.Text.Trim();
        strCountry = TB_Country.Text.Trim();
        strState = TB_State.Text.Trim();
        strCity = TB_City.Text.Trim();
        strRegistrationAddressCN = TB_AddressCN.Text.Trim();
        strRegistrationAddressEN = TB_AddressEN.Text.Trim();

        dtCreateDate = DateTime.Now;

        deDiscount = NB_Discount.Amount;
        deCreditRate = NB_CreditRate.Amount;

        strComment = TB_Comment.Text.Trim();

        if (strCustomerCode != "" | strCustomerName != "")
        {
            strHQL = "from Customer as customer where customer.CustomerCode=" + "'" + strCustomerCode + "'";
            CustomerBLL customerBLL = new CustomerBLL();
            lst = customerBLL.GetAllCustomers(strHQL);
            Customer customer = (Customer)lst[0];

            customer.CustomerName = strCustomerName;
            customer.CustomerEnglishName = strCustomerEnglishName;
            customer.CustomerEnglishName = strCustomerEnglishName;
            customer.Type = strType;
            customer.ContactName = strContactName;
            customer.SalesPerson = strSalesPerson;
            customer.InvoiceAddress = strInvoiceAddress;
            customer.Bank = strBank;
            customer.BankAccount = strBankAccount;
            customer.Currency = strCurrency;
            customer.Tel1 = strTel1;
            customer.Tel2 = strTel2;
            customer.Fax = strFax;
            customer.EmailAddress = strEmailAddress;
            customer.WebSite = strWebSite;
            customer.ZP = strZP;
            customer.Country = strCountry;
            customer.State = strState;
            customer.City = strCity;
            customer.RegistrationAddressCN = strRegistrationAddressCN;
            customer.RegistrationAddressEN = strRegistrationAddressEN;
            customer.CreditRate = deCreditRate;
            customer.Discount = deDiscount;
            customer.Comment = strComment;
            customer.CreatorCode = strUserCode;

            try
            {
                customerBLL.UpdateCustomer(customer, strCustomerCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                LoadCustomerList(strProjectID);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKHDMHMCBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        CustomerBLL customerBLL = new CustomerBLL();
        IList lst = customerBLL.GetAllCustomers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadIndustryType()
    {
        string strHQL;
        IList lst;

        strHQL = "From IndustryType as industryType Order By industryType.SortNumber ASC";
        IndustryTypeBLL industryTypeBLL = new IndustryTypeBLL();
        lst = industryTypeBLL.GetAllIndustryTypes(strHQL);

        DL_IndustryType.DataSource = lst;
        DL_IndustryType.DataBind();
    }

    protected void LoadCustomerList(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Customer as customer where customer.CustomerCode in (select projectCustomer.CustomerCode from ProjectCustomer as projectCustomer where projectCustomer.ProjectID = " + strProjectID + ")";

        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }
    
}