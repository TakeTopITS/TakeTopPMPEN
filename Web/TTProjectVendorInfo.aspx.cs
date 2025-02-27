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

public partial class TTProjectVendorInfo : System.Web.UI.Page
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

        //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + project.ProjectName + "供应商资料";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LoadIndustryType();

            LoadVendorList(strProjectID);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_VendorCode.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strVendorCode;

        if (e.CommandName != "Page")
        {
            strVendorCode = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from Vendor as vendor where vendor.VendorCode = " + "'" + strVendorCode + "'";
                VendorBLL vendorBLL = new VendorBLL();
                lst = vendorBLL.GetAllVendors(strHQL);

                Vendor vendor = (Vendor)lst[0];

                TB_VendorCode.Text = vendor.VendorCode;
                TB_VendorName.Text = vendor.VendorName;
                TB_VendorEnglishName.Text = vendor.VendorEnglishName;
                TB_Type.Text = vendor.Type.Trim();
                TB_ContactName.Text = vendor.ContactName;
                TB_SalePerson.Text = vendor.SalesPerson;
                TB_EMailAddress.Text = vendor.EmailAddress;
                TB_WebSite.Text = vendor.WebSite;
                TB_Tel1.Text = vendor.Tel1;
                TB_Tel2.Text = vendor.Tel2;
                TB_ZP.Text = vendor.ZP;
                TB_Fax.Text = vendor.Fax;
                TB_InvoiceAddress.Text = vendor.InvoiceAddress;
                TB_Bank.Text = vendor.Bank;
                TB_BankAccount.Text = vendor.BankAccount;
                DL_Currency.SelectedValue = vendor.Currency.Trim();
                TB_Country.Text = vendor.Country;
                TB_State.Text = vendor.State;
                TB_City.Text = vendor.City;
                TB_AddressCN.Text = vendor.RegistrationAddressCN;
                TB_AddressEN.Text = vendor.RegistrationAddressEN;
                LB_CreateDate.Text = vendor.CreateDate.ToString();
                TB_Comment.Text = vendor.Comment;

                NB_CreditRate.Amount = vendor.CreditRate;
                NB_Discount.Amount = vendor.Discount;

                HL_RelatedContactInfor.Enabled = true;
                HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Vendor&RelatedID=" + strVendorCode;

                LB_VendorCode.Text = strVendorCode;
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                VendorBLL vendorBLL = new VendorBLL();
                Vendor vendor = new Vendor();

                vendor.VendorCode = strVendorCode;

                try
                {
                    vendorBLL.DeleteVendor(vendor);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);

                    LoadVendorList(strProjectID);

                    HL_RelatedContactInfor.Enabled = false;

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
        string strVendorCode;

        strVendorCode = LB_VendorCode.Text.Trim();

        if (strVendorCode == "")
        {
            AddVendor();
        }
        else
        {
            UpdateVendor();
        }
    }

    protected void AddVendor()
    {
        string strVendorCode, strVendorName, strType, strContactName, strSalesPerson;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strRegistrationAddressCN, strRegistrationAddressEN;
        string strVendorEnglishName, strComment;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount;

        strVendorCode = TB_VendorCode.Text.Trim();
        strVendorName = TB_VendorName.Text.Trim();
        strVendorEnglishName = TB_VendorEnglishName.Text.Trim();
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


        if (strVendorCode != "" | strVendorName != "")
        {
            VendorBLL vendorBLL = new VendorBLL();
            Vendor vendor = new Vendor();

            vendor.VendorCode = strVendorCode;
            vendor.VendorName = strVendorName;
            vendor.VendorEnglishName = strVendorEnglishName;
            vendor.VendorEnglishName = strVendorEnglishName;
            vendor.Type = strType;
            vendor.ContactName = strContactName;
            vendor.SalesPerson = strSalesPerson;
            vendor.InvoiceAddress = strInvoiceAddress;
            vendor.Bank = strBank;
            vendor.BankAccount = strBankAccount;
            vendor.Currency = strCurrency;
            vendor.Tel1 = strTel1;
            vendor.Tel2 = strTel2;
            vendor.Fax = strFax;
            vendor.EmailAddress = strEmailAddress;
            vendor.WebSite = strWebSite;
            vendor.ZP = strZP;
            vendor.Country = strCountry;
            vendor.State = strState;
            vendor.City = strCity;
            vendor.RegistrationAddressCN = strRegistrationAddressCN;
            vendor.RegistrationAddressEN = strRegistrationAddressEN;
            vendor.CreateDate = dtCreateDate;
            vendor.CreditRate = deCreditRate;
            vendor.Discount = deDiscount;
            vendor.Comment = strComment;
            vendor.CreatorCode = strUserCode;


            try
            {
                vendorBLL.AddVendor(vendor);


                ProjectVendorBLL projectVendorBLL = new ProjectVendorBLL();
                ProjectVendor projectVendor = new ProjectVendor();

                projectVendor.ProjectID = int.Parse(strProjectID);
                projectVendor.VendorCode = strVendorCode;

                try
                {
                    projectVendorBLL.AddProjectVendor(projectVendor);
                    LoadVendorList(strProjectID);

                    HL_RelatedContactInfor.Enabled = true;
                    HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Vendor&RelatedID=" + strVendorCode;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGYSDMHMCBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateVendor()
    {
        string strHQL;
        IList lst;

        string strVendorCode, strVendorName, strType, strContactName, strSalesPerson;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strRegistrationAddressCN, strRegistrationAddressEN;
        string strVendorEnglishName, strComment;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount;

        strVendorCode = TB_VendorCode.Text.Trim();
        strVendorName = TB_VendorName.Text.Trim();
        strVendorEnglishName = TB_VendorEnglishName.Text.Trim();
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

        if (strVendorCode != "" | strVendorName != "")
        {
            strHQL = "from Vendor as vendor where vendor.VendorCode=" + "'" + strVendorCode + "'";
            VendorBLL vendorBLL = new VendorBLL();
            lst = vendorBLL.GetAllVendors(strHQL);
            Vendor vendor = (Vendor)lst[0];

            vendor.VendorName = strVendorName;
            vendor.VendorEnglishName = strVendorEnglishName;
            vendor.VendorEnglishName = strVendorEnglishName;
            vendor.Type = strType;
            vendor.ContactName = strContactName;
            vendor.SalesPerson = strSalesPerson;
            vendor.InvoiceAddress = strInvoiceAddress;
            vendor.Bank = strBank;
            vendor.BankAccount = strBankAccount;
            vendor.Currency = strCurrency;
            vendor.Tel1 = strTel1;
            vendor.Tel2 = strTel2;
            vendor.Fax = strFax;
            vendor.EmailAddress = strEmailAddress;
            vendor.WebSite = strWebSite;
            vendor.ZP = strZP;
            vendor.Country = strCountry;
            vendor.State = strState;
            vendor.City = strCity;
            vendor.RegistrationAddressCN = strRegistrationAddressCN;
            vendor.RegistrationAddressEN = strRegistrationAddressEN;
            vendor.CreditRate = deCreditRate;
            vendor.Discount = deDiscount;
            vendor.Comment = strComment;
            vendor.CreatorCode = strUserCode;

            try
            {
                vendorBLL.UpdateVendor(vendor, strVendorCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                LoadVendorList(strProjectID);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGYSDMHMCBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        VendorBLL vendorBLL = new VendorBLL();
        IList lst = vendorBLL.GetAllVendors(strHQL);

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

    protected void LoadVendorList(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Vendor as vendor where vendor.VendorCode in (select projectVendor.VendorCode from ProjectVendor as projectVendor where projectVendor.ProjectID = " + strProjectID + ")";

        VendorBLL vendorBLL = new VendorBLL();
        lst = vendorBLL.GetAllVendors(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

}
