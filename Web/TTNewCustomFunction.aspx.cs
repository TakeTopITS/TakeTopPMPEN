using System; using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;

public partial class TTNewCustomFunction : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

         if (!IsPostBack)
        {
            LoadCustomTree();
        }
    }

    private void LoadCustomTree()
    {
        string strCustomerHQL = string.Empty;
        strCustomerHQL = "from Customer as customer where CreatorCode = '" + strUserCode + "' order by CreateDate desc";
        CustomerBLL customerBLL = new CustomerBLL();
        IList lis_Customer = customerBLL.GetAllCustomers(strCustomerHQL);
        if (lis_Customer != null && lis_Customer.Count > 0)
        {
            TV_Customers.Nodes.Clear();
            TreeNode Node = new TreeNode();
            Node.Text = strUserName + LanguageHandle.GetWord("DeKeHu");
            Node.Value = "all|" + strUserCode;
            for (int i = 0; i < lis_Customer.Count; i++)
            {
                Customer customer = (Customer)lis_Customer[i];

                TreeNode NodeChild = new TreeNode();
                NodeChild.Text = customer.CustomerName;
                string strCustomerCode = customer.CustomerCode;
                NodeChild.Value = "name|" + strCustomerCode;
                //目标客户模块
                TreeNode NodeTarget = new TreeNode();
                NodeTarget.Text = LanguageHandle.GetWord("KeHuKaiFa");
                NodeTarget.Value = "target|" + strCustomerCode;
                NodeChild.ChildNodes.Add(NodeTarget);
                //开发计划模块
                TreeNode NodePlan = new TreeNode();
                NodePlan.Text = LanguageHandle.GetWord("KaiFaJiHua");
                NodePlan.Value = "plan|" + strCustomerCode;
                NodeChild.ChildNodes.Add(NodePlan);
                //产品报价模块
                TreeNode NodeProduct = new TreeNode();
                NodeProduct.Text = LanguageHandle.GetWord("ChanPinBaoJia");
                NodeProduct.Value = "product|" + strCustomerCode;
                NodeChild.ChildNodes.Add(NodeProduct);
                //专题讨论模块
                TreeNode NodeProject = new TreeNode();
                NodeProject.Text = LanguageHandle.GetWord("ZhuanTiTaoLun");
                NodeProject.Value = "project|" + strCustomerCode;
                NodeChild.ChildNodes.Add(NodeProject);
                //销售跟踪模块
                TreeNode NodeSale = new TreeNode();
                NodeSale.Text = LanguageHandle.GetWord("XiaoShouGenZong");
                NodeSale.Value = "sale|" + strCustomerCode;
                NodeChild.ChildNodes.Add(NodeSale);
                Node.ChildNodes.Add(NodeChild);
            }
            Node.ExpandAll();
            TV_Customers.Nodes.Add(Node);
        }
    }

    protected void DL_IndustryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_IndustryType.SelectedValue.Trim();

        TB_Type.Text = strType;
    }

    private void ShowCustomerInfo(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Customer as customer where customer.CustomerCode = " + "'" + strCustomerCode + "'";
        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);
        if (lst != null && lst.Count > 0)
        {
            Customer customer = (Customer)lst[0];

            TB_CustomerCode.Text = customer.CustomerCode.Trim();
            TB_CustomerName.Text = customer.CustomerName.Trim();
            TB_CustomerEnglishName.Text = customer.CustomerEnglishName.Trim();
            TB_Type.Text = customer.Type.Trim();
            TB_ContactName.Text = customer.ContactName.Trim();
            TB_SalePerson.Text = customer.SalesPerson.Trim();
            TB_EMailAddress.Text = customer.EmailAddress.Trim();
            TB_WebSite.Text = customer.WebSite.Trim();
            TB_Tel1.Text = customer.Tel1.Trim();
            TB_Tel2.Text = customer.Tel2.Trim();
            TB_ZP.Text = customer.ZP.Trim();
            TB_Fax.Text = customer.Fax.Trim();
            TB_InvoiceAddress.Text = customer.InvoiceAddress.Trim();
            TB_Bank.Text = customer.Bank.Trim();
            TB_BankAccount.Text = customer.BankAccount.Trim();
            DL_Currency.SelectedValue = customer.Currency.Trim();
            TB_Country.Text = customer.Country.Trim();
            TB_State.Text = customer.State.Trim();
            TB_City.Text = customer.City.Trim();
            TB_AddressCN.Text = customer.RegistrationAddressCN.Trim();
            TB_AddressEN.Text = customer.RegistrationAddressEN.Trim();
            LB_CreateDate.Text = customer.CreateDate.ToString();
            TB_Comment.Text = customer.Comment.Trim();

            NB_CreditRate.Amount = customer.CreditRate;
            NB_Discount.Amount = customer.Discount;


        }
        else
        {
            TB_CustomerCode.Text = "";
            TB_CustomerName.Text = "";
            TB_CustomerEnglishName.Text = "";
            TB_Type.Text = "";
            TB_ContactName.Text = "";
            TB_SalePerson.Text = "";
            TB_EMailAddress.Text = "";
            TB_WebSite.Text = "";
            TB_Tel1.Text = "";
            TB_Tel2.Text = "";
            TB_ZP.Text = "";
            TB_Fax.Text = "";
            TB_InvoiceAddress.Text = "";
            TB_Bank.Text = "";
            TB_BankAccount.Text = "";
            DL_Currency.SelectedValue = "";
            TB_Country.Text = "";
            TB_State.Text = "";
            TB_City.Text = "";
            TB_AddressCN.Text = "";
            TB_AddressEN.Text = "";
            LB_CreateDate.Text = "";
            TB_Comment.Text = "";

            NB_CreditRate.Amount = 0;
            NB_Discount.Amount = 0;
        }
    }


    protected void BT_Add_Click(object sender, EventArgs e)
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
            customer.CreateDate = DateTime.Now;
            customer.CreditRate = deCreditRate;
            customer.Discount = deDiscount;
            customer.CreatorCode = strUserCode;
            customer.Comment = strComment;

            try
            {
                customerBLL.AddCustomer(customer);


                LoadCustomTree();
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>ShowDiv();</script>");
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertXinZengChuCuoQingJi"));
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertKeHuDaiMaHeMingChen"));
        }
    }


    protected void BT_Update_Click(object sender, EventArgs e)
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
            if (lst != null && lst.Count > 0)
            {
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

                    LoadCustomTree();

                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertBaoCunChengGongShow"));
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertXiuGaiChuCuoQingJia"));
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertBuNengXiuGaiKeHuDai"));
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertGongYingShangDaiMaH"));
        }
    }


    protected void TV_Customers_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (TV_Customers.SelectedNode != null)
        {
            LLB_CustomModule.Visible = false;
            RPT_WF.DataSource = null;
            RPT_WF.DataBind();
            string strSelectNodeValue = TV_Customers.SelectedNode.Value;
            string[] arrSelectNodeValue = strSelectNodeValue.Split('|');
            if (arrSelectNodeValue[0] == "all")
            {
                mainFrame.Attributes.Add("src", "");
                string strCustomerCode = arrSelectNodeValue[1];
                BT_Update.Enabled = false;
                BT_Add.Enabled = true;
                ShowCustomerInfo("");
            }
            else if (arrSelectNodeValue[0] == "name")
            {
                mainFrame.Attributes.Add("src", "");
                string strCustomerCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCustomerInfo(strCustomerCode);
            }
            else if (arrSelectNodeValue[0] == "target")
            {
                string strCustomerCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCustomerInfo(strCustomerCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("target", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strCustomerCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strCustomerCode;
                        //LLB_CustomModule.Visible = true;
                        mainFrame.Attributes.Add("src", "");
                        //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('设定了多个客户开发的模组！请选择一个模组');ShowDiv();</script>");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
            else if (arrSelectNodeValue[0] == "plan")
            {
                string strCustomerCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCustomerInfo(strCustomerCode);

                mainFrame.Attributes.Add("src", "TTPlanManagementRelatedOther.aspx?RelatedType=WORKFLOW&RelatedID=0&RelatedCode=" + strCustomerCode);
            }
            else if (arrSelectNodeValue[0] == "product")
            {
                string strCustomerCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCustomerInfo(strCustomerCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("product", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strCustomerCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strCustomerCode;
                        //LLB_CustomModule.Visible = true;
                        mainFrame.Attributes.Add("src", "");
                        //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('设定了多个客户开发的模组！请选择一个模组');ShowDiv();</script>");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
            else if (arrSelectNodeValue[0] == "project")
            {
                string strCustomerCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCustomerInfo(strCustomerCode);

                mainFrame.Attributes.Add("src", "TTCollaborationManageRelatedOther.aspx?RelatedType=WORKFLOW&RelatedID=0&RelatedCode=" + strCustomerCode);
            }
            else if (arrSelectNodeValue[0] == "sale")
            {
                string strCustomerCode = arrSelectNodeValue[1];
                BT_Update.Enabled = true;
                BT_Add.Enabled = false;
                ShowCustomerInfo(strCustomerCode);

                string strTemName = string.Empty;
                string strIdentifyString = string.Empty;
                string strDuo = string.Empty;
                ReadCustomerModule("sale", out strTemName, out strIdentifyString, out strDuo);
                if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString) && !string.IsNullOrEmpty(strDuo))
                {
                    if (strDuo == "1")
                    {
                        mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strCustomerCode);
                    }
                    else
                    {
                        HF_RelatedCode.Value = strCustomerCode;
                        //LLB_CustomModule.Visible = true;
                        mainFrame.Attributes.Add("src", "");
                        //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('设定了多个客户开发的模组！请选择一个模组');ShowDiv();</script>");
                    }
                }
                else
                {
                    mainFrame.Attributes.Add("src", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "", LanguageHandle.GetWord("scriptalertWeiSheDingKeHuKaiFa"));
                }
            }
        }
    }


    private void ReadCustomerModule(string strCustomerType, out string strTemName, out string strIdentifyString, out string strDuo)
    {
        CustomModuleBLL customModuleBLL = new CustomModuleBLL();
        string strCustomModuleSql = "from CustomModule as customModule where CustomType = '" + strCustomerType + "'";
        IList customModuleList = customModuleBLL.GetAllCustomModules(strCustomModuleSql);
        if (customModuleList != null && customModuleList.Count > 0)
        {
            if (customModuleList.Count == 1)
            {
                CustomModule customModule = (CustomModule)customModuleList[0];
                strTemName = customModule.TemName;
                strIdentifyString = customModule.IdentifyString;
                strDuo = "1";
            }
            else
            {
                LLB_CustomModule.DataSource = customModuleList;
                LLB_CustomModule.DataBind();
                //TODO
                RPT_WF.DataSource = customModuleList;
                RPT_WF.DataBind();
                strTemName = "2";
                strIdentifyString = "2";
                strDuo = "2";
            }
        }
        else
        {
            strTemName = "";
            strIdentifyString = "";
            strDuo = "";
        }
    }


    protected void LLB_CustomModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strTemName = LLB_CustomModule.SelectedItem.Text.Trim();
        string strIdentifyString = LLB_CustomModule.SelectedValue.Trim();

        if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString))
        {
            string strCustomerCode = HF_RelatedCode.Value;
            mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strCustomerCode);
        }
    }

    protected void RPT_WF_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Button btnTemName = (Button)e.Item.FindControl("BTN_ClickWF");
        HiddenField hfIdentifyString = (HiddenField)e.Item.FindControl("HF_IdentifyString");

        string strTemName = btnTemName.Text;
        string strIdentifyString = hfIdentifyString.Value;
        if (!string.IsNullOrEmpty(strTemName) && !string.IsNullOrEmpty(strIdentifyString))
        {
            string strCustomerCode = HF_RelatedCode.Value;
            mainFrame.Attributes.Add("src", "TTDIYModuleByWFFormRelatedOther.aspx?ModuleName=" + strTemName + "&TemIdentifyString=" + strIdentifyString + "&RelatedCode=" + strCustomerCode);
        }
        else
        {
            mainFrame.Attributes.Add("src", "");
        }
    }
}