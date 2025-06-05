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

public partial class TTQuestionToCustomer : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strQuestionID;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strQuestionID = Request.QueryString["QuestionID"];

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        //this.Title = "服务转客户档案---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadIndustryType(DL_IndustryType);
            LoadIndustryType(DL_IndustryTypeFind);
            LoadIndustryType(DL_FindAgencyIndustryType);

            LoadCustomerListRelatedQuestion();
            LoadCustomerList();

            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView2, strUserCode);
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView3, strUserCode);

            strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
            CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
            lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);
            CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

            LB_QuestionID.Text = strQuestionID;
            LB_QuestionName.Text = customerQuestion.Question.Trim();
            TB_CustomerName.Text = customerQuestion.Company.Trim();

            TB_ContactName.Text = customerQuestion.ContactPerson;
            TB_Tel1.Text = customerQuestion.PhoneNumber;

            TB_AddressCN.Text = customerQuestion.Address;
            LB_BelongAgencyName.Text = customerQuestion.AgencyName;

            TB_FindAgencyName.Text = customerQuestion.AgencyName;

            TB_Comment.Text = customerQuestion.BusinessSource;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strCustomerCode, strCreatorCode;

        if (e.CommandName != "Page")
        {
            strCustomerCode = ((Button)e.Item.FindControl("BT_CustomerCode")).Text;

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
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

            TB_SimpleName.Text = customer.SimpleName.Trim();
            TB_WorkSiteURL.Text = customer.WorkSiteURL.Trim();

            LB_BelongDepartCode.Text = customer.BelongDepartCode;
            LB_BelongDepartName.Text = customer.BelongDepartName;

            LB_BelongAgencyCode.Text = customer.BelongAgencyCode;
            LB_BelongAgencyName.Text = customer.BelongAgencyName;

            strCreatorCode = customer.CreatorCode.Trim();
            if (strCreatorCode == strUserCode)
            {
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_Delete.Enabled = false;
            }
            BT_Update.Enabled = true;

            BT_RelatedCustomer.Enabled = false;
            BT_CancelRelatedCustomer.Enabled = true;

            LoadProjectList(strCustomerCode);
            LoadCustomerRelatedUser(strCustomerCode);

            HL_RelatedContactInfor.Enabled = true;
            HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;

            HL_TransferProject.Enabled = true;
            HL_TransferProject.NavigateUrl = "TTMakeProjectFromOther.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strCustomerCode, strCreatorCode;

        if (e.CommandName != "Page")
        {
            strCustomerCode = ((Button)e.Item.FindControl("BT_CustomerCode")).Text;

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

            TB_SimpleName.Text = customer.SimpleName.Trim();
            TB_WorkSiteURL.Text = customer.WorkSiteURL.Trim();

            LB_BelongDepartCode.Text = customer.BelongDepartCode;
            LB_BelongDepartName.Text = customer.BelongDepartName;

            LB_BelongAgencyCode.Text = customer.BelongAgencyCode;
            LB_BelongAgencyName.Text = customer.BelongAgencyName;


            strCreatorCode = customer.CreatorCode.Trim();
            if (strCreatorCode == strUserCode)
            {
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_Delete.Enabled = false;
            }
            BT_Update.Enabled = true;

            BT_RelatedCustomer.Enabled = true;
            BT_CancelRelatedCustomer.Enabled = true;

            LoadProjectList(strCustomerCode);
            LoadCustomerRelatedUser(strCustomerCode);

            HL_RelatedContactInfor.Enabled = true;
            HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;

            HL_TransferProject.Enabled = true;
            HL_TransferProject.NavigateUrl = "TTMakeProjectFromOther.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;
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

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strCustomerCode, strProjectID;

        if (e.CommandName != "Page")
        {
            strCustomerCode = TB_CustomerCode.Text.Trim();
            strProjectID = e.Item.Cells[0].Text.Trim();

            strHQL = "from ProjectCustomer as projectCustomer where projectCustomer.ProjectID = " + strProjectID + " and projectCustomer.CustomerCode = " + "'" + strCustomerCode + "'";
            ProjectCustomerBLL projectCustomerBLL = new ProjectCustomerBLL();
            lst = projectCustomerBLL.GetAllProjectCustomers(strHQL);

            ProjectCustomer projectCustomer = (ProjectCustomer)lst[0];

            try
            {
                projectCustomerBLL.DeleteProjectCustomer(projectCustomer);
                LoadProjectList(strCustomerCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
            }
        }
    }

    protected void DL_IndustryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_IndustryType.SelectedValue.Trim();

        TB_Type.Text = strType;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strCustomerCode, strCustomerName, strType, strContactName, strSalesPerson;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strRegistrationAddressCN, strRegistrationAddressEN;
        string strCustomerEnglishName, strComment, strSimpleName;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount;

        string strBelongAgencyCode, strBelongAgencyName, strBelongDepartCode, strBelongDepartName;

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

        strSimpleName = TB_SimpleName.Text.Trim();

        strBelongDepartCode = LB_BelongDepartCode.Text;
        strBelongDepartName = LB_BelongDepartName.Text;

        strBelongAgencyCode = LB_BelongAgencyCode.Text.Trim();
        strBelongAgencyName = LB_BelongAgencyName.Text.Trim();

        dtCreateDate = DateTime.Now;

        deDiscount = NB_Discount.Amount;
        deCreditRate = NB_CreditRate.Amount;

        strComment = TB_Comment.Text.Trim();

        if(strBelongAgencyCode == "" & strBelongAgencyName != "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShiBaiFenGuanJingXiaoShangDa")+"')", true);
            return;
        }

        if (strCustomerCode != "" | strCustomerName != "" | strSimpleName != "" | strType != "")
        {
            if (CheckCustomerNameIsExist(strCustomerCode, strCustomerName, strSimpleName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click1111", "alert('" + LanguageHandle.GetWord("ZZJGCZXTMTDKHQJC") + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                return;
            }


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
            customer.CreateDate = DateTime.Now;
            customer.CreditRate = deCreditRate;
            customer.Discount = deDiscount;
            customer.CreatorCode = strUserCode;
            customer.Comment = strComment;

            customer.SimpleName = TB_SimpleName.Text.Trim();
            customer.WorkSiteURL = TB_WorkSiteURL.Text.Trim();

            customer.BelongDepartCode = strBelongDepartCode;
            customer.BelongDepartName = strBelongDepartName;

            customer.BelongAgencyCode = strBelongAgencyCode;
            customer.BelongAgencyName = strBelongAgencyName;

            try
            {
                customerBLL.AddCustomer(customer);

                try
                {
                    string strHQL;
                    strHQL = "Select CustomerCode From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + " and QuestionID = " + strQuestionID;
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerToQuestion");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        strHQL = "Insert Into T_CustomerRelatedQuestion(CustomerCode,QuestionID) Values(" + "'" + strCustomerCode + "'" + "," + strQuestionID + ")";
                        ShareClass.RunSqlCommand(strHQL);
                    }
                    else
                    {
                    }
                }
                catch
                {
                }

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                LoadCustomerListRelatedQuestion();
                LoadCustomerList();

                HL_RelatedContactInfor.Enabled = true;
                HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCCJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKHDMHMCBNWKJC") + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strCustomerCode, strCustomerName, strType, strContactName, strSalesPerson;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strRegistrationAddressCN, strRegistrationAddressEN;
        string strCustomerEnglishName, strComment, strSimpleName;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount;

        string strBelongAgencyCode, strBelongAgencyName, strBelongDepartCode, strBelongDepartName;

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

        strSimpleName = TB_SimpleName.Text.Trim();

        strBelongDepartCode = LB_BelongDepartCode.Text;
        strBelongDepartName = LB_BelongDepartName.Text;

        strBelongAgencyCode = LB_BelongAgencyCode.Text;
        strBelongAgencyName = LB_BelongAgencyName.Text;

        dtCreateDate = DateTime.Now;

        deDiscount = NB_Discount.Amount;
        deCreditRate = NB_CreditRate.Amount;

        strComment = TB_Comment.Text.Trim();

        if (strCustomerCode != "" | strCustomerName != "" | strSimpleName != "" | strType != "")
        {
            if (CheckCustomerNameIsExist(strCustomerCode, strCustomerName, strSimpleName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click1111", "alert('" + LanguageHandle.GetWord("ZZJGCZXTMTDKHQJC") + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                return;
            }

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

            customer.SimpleName = TB_SimpleName.Text.Trim();
            customer.WorkSiteURL = TB_WorkSiteURL.Text.Trim();

            customer.BelongDepartCode = strBelongDepartCode;
            customer.BelongDepartName = strBelongDepartName;

            customer.BelongAgencyCode = strBelongAgencyCode;
            customer.BelongAgencyName = strBelongAgencyName;

            try
            {
                customerBLL.UpdateCustomer(customer, strCustomerCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                LoadCustomerListRelatedQuestion();
                LoadCustomerList();

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGYSDMHMCBNWKJC") + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strCustomerCode;

        strCustomerCode = TB_CustomerCode.Text.Trim();

        CustomerBLL customerBLL = new CustomerBLL();
        Customer customer = new Customer();

        customer.CustomerCode = strCustomerCode;

        try
        {
            customerBLL.DeleteCustomer(customer);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);

            LoadCustomerListRelatedQuestion();
            LoadCustomerList();

            HL_RelatedContactInfor.Enabled = false;
            HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=Customer&RelatedID=" + strCustomerCode;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
        }
    }

    protected void BT_RelatedCustomer_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCustomerCode;

        strCustomerCode = TB_CustomerCode.Text.Trim();

        try
        {
            strHQL = "Select CustomerCode From T_CustomerRelatedQuestion Where QuestionID = " + strQuestionID + " and CustomerCode = " + "'" + strCustomerCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerToQuestion");
            if (ds.Tables[0].Rows.Count == 0)
            {
                strHQL = "Insert Into T_CustomerRelatedQuestion(CustomerCode,QuestionID) Values(" + "'" + strCustomerCode + "'" + "," + strQuestionID + ")";
                ShareClass.RunSqlCommand(strHQL);
            }
            else
            {
            }
        }
        catch
        {
        }

        LoadCustomerListRelatedQuestion();
        LoadCustomerList();
    }

    protected void BT_CancelRelatedCustomer_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCustomerCode;

        strCustomerCode = TB_CustomerCode.Text.Trim();

        try
        {
            strHQL = "Delete From  T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strCustomerCode + "'" + " and QuestionID = " + strQuestionID;
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }

        LoadCustomerListRelatedQuestion();
        LoadCustomerList();
    }

    protected void BT_RelatedProject_Click(object sender, EventArgs e)
    {
        string strProjectID;
        string strCustomerCode;

        strProjectID = TB_ProjectID.Text.Trim();
        strCustomerCode = TB_CustomerCode.Text.Trim();

        ProjectCustomerBLL projectCustomerBLL = new ProjectCustomerBLL();
        ProjectCustomer projectCustomer = new ProjectCustomer();

        projectCustomer.ProjectID = int.Parse(strProjectID);
        projectCustomer.CustomerCode = strCustomerCode;

        try
        {
            projectCustomerBLL.AddProjectCustomer(projectCustomer);
            LoadProjectList(strCustomerCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGLCCJC") + "')", true);
        }
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
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strCustomerCode, strOperatorCode;

        strCustomerCode = TB_CustomerCode.Text.Trim();

        if (strCustomerCode != "")
        {
            strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

            CustomerRelatedUserBLL customerRelatedUserBLL = new CustomerRelatedUserBLL();
            CustomerRelatedUser customerRelatedUser = new CustomerRelatedUser();

            customerRelatedUser.CustomerCode = strCustomerCode;
            customerRelatedUser.UserCode = strOperatorCode;
            customerRelatedUser.UserName = ShareClass.GetUserName(strOperatorCode);

            try
            {
                customerRelatedUserBLL.AddCustomerRelatedUser(customerRelatedUser);

                strHQL = "from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.CustomerCode = " + "'" + strCustomerCode + "'";
                lst = customerRelatedUserBLL.GetAllCustomerRelatedUsers(strHQL);

                RP_CustomerMember.DataSource = lst;
                RP_CustomerMember.DataBind();
            }
            catch
            {
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZHTCNZJCY") + "')", true);
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            string strHQL, strCustomerCode;
            IList lst;

            strCustomerCode = TB_CustomerCode.Text.Trim();
            strHQL = "from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.CustomerCode = " + "'" + strCustomerCode + "'" + " and customerRelatedUser.UserName = " + "'" + strUserName + "'";
            CustomerRelatedUserBLL customerRelatedUserBLL = new CustomerRelatedUserBLL();
            lst = customerRelatedUserBLL.GetAllCustomerRelatedUsers(strHQL);

            CustomerRelatedUser customerRelatedUser = (CustomerRelatedUser)lst[0];

            customerRelatedUserBLL.DeleteCustomerRelatedUser(customerRelatedUser);

            LoadCustomerRelatedUser(strCustomerCode);
        }
    }

    protected void LoadCustomerRelatedUser(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.CustomerCode = " + "'" + strCustomerCode + "'";
        CustomerRelatedUserBLL customerRelatedUserBLL = new CustomerRelatedUserBLL();
        lst = customerRelatedUserBLL.GetAllCustomerRelatedUsers(strHQL);

        RP_CustomerMember.DataSource = lst;
        RP_CustomerMember.DataBind();
    }

    protected void LoadCustomerRelatedProject(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID in ( select customerRelatedProject.ProjectID from  CustomerRelatedProject as customerRelatedProject where customerRelatedProject.CustomerCode = " + "'" + strCustomerCode + "'" + ")";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strIndustryType, strCustomerCode, strCustomerName;
        string strState, strCity, strAreaAddress, strAgencyName, strDepartString;

        strDepartString = LB_DepartString.Text.Trim();

        strCustomerCode = "%" + TB_CustCode.Text.Trim() + "%";
        strCustomerName = "%" + TB_CustName.Text.Trim() + "%";
        strIndustryType = "%" + TB_IndustryTypeFind.Text.Trim() + "%";

        strState = "%" + TB_State.Text.Trim() + "%";
        strCity = "%" + TB_City.Text.Trim() + "%";
        strAreaAddress = "%" + TB_AreaAddress.Text.Trim() + "%";
        strAgencyName = "%" + TB_AgencyName.Text.Trim() + "%";

        strHQL = "from Customer as customer where 1=1 ";
        strHQL += " and customer.Type like  " + "'" + strIndustryType + "'";
        strHQL += " and customer.CustomerCode like " + "'" + strCustomerCode + "'";
        strHQL += " and customer.CustomerName like  " + "'" + strCustomerName + "'";
        strHQL += " and customer.BelongAgencyName Like " + "'" + strAgencyName + "'";
        strHQL += " and customer.State Like " + "'" + strState + "'";
        strHQL += " and customer.City Like " + "'" + strCity + "'";
        strHQL += " and customer.AreaAddress Like " + "'" + strAreaAddress + "'";
        strHQL += " and (customer.CreatorCode = '" + strUserCode + "'";
        strHQL += " or (customer.CustomerCode in (Select customerRelatedUser.CustomerCode from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")))";
        strHQL += " Order By customer.CreateDate DESC";

        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DL_IndustryTypeFind_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_IndustryTypeFind.SelectedValue.Trim();

        TB_IndustryTypeFind.Text = strType;
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            LB_BelongDepartCode.Text = strDepartCode;
            LB_BelongDepartName.Text = ShareClass.GetDepartName(strDepartCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void BT_BelongAgency_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFindAgencyWindow','true') ", true); 
    }


    protected void DL_FindAgencyIndustryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_FindAgencyIndustryType.SelectedValue.Trim();

        TB_FindAgencyIndustryType.Text = strType;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFindAgencyWindow','true') ", true);
    }

    protected void BT_FindAgency_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strIndustryType, strCustomerCode, strCustomerName, strDepartString;

        strDepartString = LB_DepartString.Text;

        strCustomerCode = "%" + TB_FindAgencyCode.Text.Trim() + "%";
        strCustomerName = "%" + TB_FindAgencyName.Text.Trim() + "%";
        strIndustryType = "%" + TB_FindAgencyIndustryType.Text.Trim() + "%";

        strHQL = "from Customer as customer where 1=1 ";
        strHQL += " and customer.Type like  " + "'" + strIndustryType + "'";
        strHQL += " and customer.CustomerCode like " + "'" + strCustomerCode + "'";
        strHQL += " and customer.CustomerName like  " + "'" + strCustomerName + "'";
        strHQL += " and customer.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " and customer.BelongDepartCode in " + strDepartString;
        strHQL += " Order By customer.CreateDate DESC";

        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DataGrid15.DataSource = lst;
        DataGrid15.DataBind();

        LB_Sql15.Text = strHQL;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFindAgencyWindow','true') ", true);
    }

    protected void DataGrid15_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid15.Items.Count; i++)
            {
                DataGrid15.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            string strAgencyCode = e.Item.Cells[1].Text.Trim();
            string strAgencyName = ShareClass.GetCustomerNameFromCustomerCode(strAgencyCode);


            LB_BelongAgencyCode.Text = strAgencyCode;
            LB_BelongAgencyName.Text = strAgencyName;
        }
    }

    protected void DataGrid15_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql15.Text;
        IList lst;

        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DataGrid15.DataSource = lst;
        DataGrid15.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popFindAgencyWindow','true') ", true);
    }


    protected void LoadIndustryType(DropDownList DL_Type)
    {
        string strHQL;
        IList lst;

        strHQL = "From IndustryType as industryType Order By industryType.SortNumber ASC";
        IndustryTypeBLL industryTypeBLL = new IndustryTypeBLL();
        lst = industryTypeBLL.GetAllIndustryTypes(strHQL);

        DL_Type.DataSource = lst;
        DL_Type.DataBind();

        DL_Type.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadCustomerList()
    {
        string strHQL;
        IList lst;

        strHQL = "from Customer as customer ";
        strHQL += " Where 1=1";
        strHQL += " and (customer.CreatorCode = '" + strUserCode + "'";
        strHQL += " or (customer.CustomerCode in (Select customerRelatedUser.CustomerCode from CustomerRelatedUser as customerRelatedUser where customerRelatedUser.UserCode = " + "'" + strUserCode + "'" + ")))";
        strHQL += " Order by customer.CreateDate DESC";
        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadCustomerListRelatedQuestion()
    {
        string strHQL;
        IList lst;

        strHQL = "from Customer as customer where ((customer.CreatorCode =" + "'" + strUserCode + "'" + ")";
        strHQL += " or  (customer.CreatorCode  in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.CustomerServiceVisible = 'YES' and memberLevel.UserCode = " + "'" + strUserCode + "'" + ")))";
        strHQL += " and customer.CustomerCode  in (Select customerRelatedQuestion.CustomerCode From CustomerRelatedQuestion as customerRelatedQuestion Where customerRelatedQuestion.QuestionID = " + strQuestionID + ")";
        strHQL += " Order by customer.CreateDate DESC";


        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadProjectList(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID in (select projectCustomer.ProjectID from ProjectCustomer as projectCustomer where projectCustomer.CustomerCode = " + "'" + strCustomerCode + "'" + ")";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    //判断客户名称和简称是否存在
    protected bool CheckCustomerNameIsExist(string strCustomerCode, string strCustomerName, string strCustomerSimpleName)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_Customer Where (CustomerName = '{0}' or SimpleName = '{1}') and CustomerCode <> '{2}'", strCustomerName, strCustomerSimpleName, strCustomerCode);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Customer");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    protected string GetQuestionName(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        return customerQuestion.Question.Trim();
    }
}
