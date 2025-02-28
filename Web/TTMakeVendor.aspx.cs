using ProjectMgt.BLL;
using ProjectMgt.Model;

using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTMakeVendor : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strLangCode = Session["LangCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "供应商档案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadIndustryType();
            ShareClass.LoadCurrencyType(DL_CurrencyType);

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadVendorList(strUserCode);

            strHQL = "Select GroupName From T_ActorGroup Where Type <>'Part' and GroupName not in ('Individual','Department','Company','Group','All')";  
            strHQL += " and (BelongDepartCode in " + strDepartString + " Or Type = 'Super'";
            strHQL += " Or MakeUserCode = " + "'" + strUserCode + "'" + ")";
            strHQL += " and LangCode = " + "'" + strLangCode + "'";
            strHQL += " Order by SortNumber ASC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
            Repeater1.DataSource = ds;

            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from GoodsType as goodsType Order by goodsType.SortNumber ASC";
            GoodsTypeBLL goodsTypeBLL = new GoodsTypeBLL();
            lst = goodsTypeBLL.GetAllGoodsTypes(strHQL);
            DL_GoodsType.DataSource = lst;
            DL_GoodsType.DataBind();
            DL_GoodsType.Items.Insert(0, new ListItem("--Select--", ""));

            LB_BelongDepartCode.Text = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            LB_BelongDepartName.Text = ShareClass.GetDepartName(LB_BelongDepartCode.Text);
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3, strUserCode);

            ShareClass.LoadReceivePayWayForDropDownList(DL_ClearingForm);
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);

            try
            {
                string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
                if (strProductType != "ERP" & strProductType != "CRM" & strProductType != "SIMP" & strProductType != "EDPMP" & strProductType != "ECMP" & strProductType != "DEMO")
                {
                    TabPanel5.Visible = false;
                }
            }
            catch
            {
            }

            //如果自动产生客户编码，禁用客户代码输入框 
            if (ShareClass.GetCodeRuleStatusByType("VendorCode") == "YES")
            {
                TB_VendorCode.Enabled = false;
                TB_VendorCode.Text = DateTime.Now.ToString("yyyyMMddHHMMss");
            }
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
                DL_CurrencyType.SelectedValue = vendor.Currency.Trim();
                TB_Country.Text = vendor.Country;
                TB_State.Text = vendor.State;
                TB_City.Text = vendor.City;
                TB_AreaAddress.Text = vendor.AreaAddress;
                TB_AddressCN.Text = vendor.RegistrationAddressCN;
                TB_AddressEN.Text = vendor.RegistrationAddressEN;
                LB_CreateDate.Text = vendor.CreateDate.ToString();
                TB_Comment.Text = vendor.Comment;

                LB_BelongDepartCode.Text = vendor.BelongDepartCode;
                LB_BelongDepartName.Text = vendor.BelongDepartName;

                NB_CreditRate.Amount = vendor.CreditRate;
                NB_Discount.Amount = vendor.Discount;

                NB_TaxRate.Amount = vendor.TaxRate;

                TB_DeviceName.Text = vendor.DeviceName;

                try
                {
                    DL_ClearingForm.SelectedValue = vendor.ClearingForm.Trim();
                }
                catch
                {

                }

                LoadProjectList(strVendorCode);
                LoadVendorRelatedUser(strVendorCode);
                LoadVendorRelatedGoodsPurchaseOrder(vendor.VendorName.Trim());
                LoadVendorRelatedAssetPurchaseOrder(vendor.VendorName.Trim());
                LoadRelatedConstract(strVendorCode);


                HL_RelatedContactInfor.Enabled = true;
                HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=VendorCode&RelatedID=" + strVendorCode;

                LoadVendorRelatedGoodsList(strVendorCode);

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

                    LoadVendorList(strUserCode);

                    HL_RelatedContactInfor.Enabled = false;
                    HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=VendorCode&RelatedID=" + strVendorCode;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void BT_SortByAreaAddress_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strSortName = LB_UpDown.Text.Trim();

        if (strSortName == "UP")
        {
            strHQL = "from Vendor as vendor where vendor.CreatorCode = " + "'" + strUserCode + "'";
            strHQL += " Order by vendor.AreaAddress DESC";

            LB_UpDown.Text = "DOWN";
        }
        else
        {
            strHQL = "from Vendor as vendor where vendor.CreatorCode = " + "'" + strUserCode + "'";
            strHQL += " Order by vendor.AreaAddress ASC";

            LB_UpDown.Text = "UP";
        }

        VendorBLL vendorBLL = new VendorBLL();
        lst = vendorBLL.GetAllVendors(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
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


    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strVendorCode, strProjectID;

        if (e.CommandName != "Page")
        {
            strVendorCode = TB_VendorCode.Text.Trim();
            strProjectID = e.Item.Cells[0].Text.Trim();

            strHQL = "from ProjectVendor as projectVendor where projectVendor.ProjectID = " + strProjectID + " and projectVendor.VendorCode = " + "'" + strVendorCode + "'";
            ProjectVendorBLL projectVendorBLL = new ProjectVendorBLL();
            lst = projectVendorBLL.GetAllProjectVendors(strHQL);

            ProjectVendor projectVendor = (ProjectVendor)lst[0];

            try
            {
                projectVendorBLL.DeleteProjectVendor(projectVendor);
                LoadProjectList(strVendorCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_IndustryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_IndustryType.SelectedValue.Trim();

        TB_Type.Text = strType;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_IndustryTypeFind_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_IndustryTypeFind.SelectedValue.Trim();

        TB_IndustryTypeFind.Text = strType;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strVendorCode;

        strVendorCode = LB_VendorCode.Text.Trim();

        if (strVendorCode == "")
        {
            //如果自动产生客户编码，禁用客户代码输入框 
            if (ShareClass.GetCodeRuleStatusByType("VendorCode") == "YES")
            {
                TB_VendorCode.Enabled = false;
                TB_VendorCode.Text = DateTime.Now.ToString("yyyyMMddHHMMss");
            }

            AddVendor();
        }
        else
        {
            UpdateVendor();
        }
    }

    protected void AddVendor()
    {
        string strVendorCode, strVendorName, strType, strContactName, strSalesPerson, strBelongDepartCode, strBelongDepartName;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strAreaAddress, strRegistrationAddressCN, strRegistrationAddressEN;
        string strVendorEnglishName, strComment, strClearingForm;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount, deTaxRate;

        strVendorCode = TB_VendorCode.Text.Trim();
        strVendorName = TB_VendorName.Text.Trim();
        strVendorEnglishName = TB_VendorEnglishName.Text.Trim();
        strType = TB_Type.Text.Trim();
        strContactName = TB_ContactName.Text.Trim();
        strSalesPerson = TB_SalePerson.Text.Trim();
        strInvoiceAddress = TB_InvoiceAddress.Text.Trim();
        strBank = TB_Bank.Text.Trim();
        strBankAccount = TB_BankAccount.Text.Trim();
        strCurrency = DL_CurrencyType.SelectedValue;
        strTel1 = TB_Tel1.Text.Trim();
        strTel2 = TB_Tel2.Text.Trim();
        strFax = TB_Fax.Text.Trim();
        strEmailAddress = TB_EMailAddress.Text.Trim();
        strWebSite = TB_WebSite.Text.Trim();
        strZP = TB_ZP.Text.Trim();
        strCountry = TB_Country.Text.Trim();
        strState = TB_State.Text.Trim();
        strCity = TB_City.Text.Trim();
        strAreaAddress = TB_AreaAddress.Text.Trim();
        strRegistrationAddressCN = TB_AddressCN.Text.Trim();
        strRegistrationAddressEN = TB_AddressEN.Text.Trim();

        strBelongDepartCode = LB_BelongDepartCode.Text;
        strBelongDepartName = LB_BelongDepartName.Text;

        dtCreateDate = DateTime.Now;

        deDiscount = NB_Discount.Amount;
        deCreditRate = NB_CreditRate.Amount;

        deTaxRate = NB_TaxRate.Amount;
        strClearingForm = DL_ClearingForm.SelectedValue.Trim();

        strComment = TB_Comment.Text.Trim();

        if (strVendorCode != "" | strVendorName != "" | strType != "")
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
            vendor.AreaAddress = strAreaAddress;
            vendor.RegistrationAddressCN = strRegistrationAddressCN;
            vendor.RegistrationAddressEN = strRegistrationAddressEN;
            vendor.CreateDate = DateTime.Now;
            vendor.CreditRate = deCreditRate;
            vendor.Discount = deDiscount;
            vendor.CreatorCode = strUserCode;
            vendor.BelongDepartCode = strBelongDepartCode;
            vendor.BelongDepartName = strBelongDepartName;

            vendor.TaxRate = deTaxRate;
            vendor.ClearingForm = strClearingForm;

            vendor.DeviceName = TB_DeviceName.Text.Trim();

            vendor.Comment = strComment;

            try
            {
                vendorBLL.AddVendor(vendor);

                string strVendorID = GetMyCreatedMaxVendorID(strUserCode);
                string strNewVendorCode = ShareClass.GetCodeByRule("VendorCode", "", strVendorID);
                if (strNewVendorCode != "")
                {
                    TB_VendorCode.Text = strNewVendorCode;
                    string strHQL = "Update T_Vendor Set VendorCode = " + "'" + strNewVendorCode + "'" + " Where VendorID = " + strVendorID;
                    ShareClass.RunSqlCommand(strHQL);

                    strVendorCode = strNewVendorCode;
                }
                LB_VendorCode.Text = strVendorCode;

                LoadVendorList(strUserCode);

                HL_RelatedContactInfor.Enabled = true;
                HL_RelatedContactInfor.NavigateUrl = "TTContactList.aspx?RelatedType=VendorCode&RelatedID=" + strVendorCode;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGYSDMMCHLXBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    public static string GetMyCreatedMaxVendorID(string strUserCode)
    {
        string strHQL;

        strHQL = "Select Max(VendorID) From T_Vendor";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Vendor");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    protected void UpdateVendor()
    {
        string strHQL;
        IList lst;

        string strVendorCode, strVendorName, strType, strContactName, strSalesPerson, strBelongDepartCode, strBelongDepartName;
        string strInvoiceAddress, strBankAccount, strCurrency, strBank, strTel1, strTel2, strFax, strEmailAddress;
        string strWebSite, strZP, strCountry, strState, strCity, strAreaAddress, strRegistrationAddressCN, strRegistrationAddressEN;
        string strVendorEnglishName, strComment, strClearingForm;
        DateTime dtCreateDate;
        decimal deCreditRate, deDiscount, deTaxRate;

        strVendorCode = TB_VendorCode.Text.Trim();
        strVendorName = TB_VendorName.Text.Trim();
        strVendorEnglishName = TB_VendorEnglishName.Text.Trim();
        strType = TB_Type.Text.Trim();
        strContactName = TB_ContactName.Text.Trim();
        strSalesPerson = TB_SalePerson.Text.Trim();
        strInvoiceAddress = TB_InvoiceAddress.Text.Trim();
        strBank = TB_Bank.Text.Trim();
        strBankAccount = TB_BankAccount.Text.Trim();
        strCurrency = DL_CurrencyType.SelectedValue;
        strTel1 = TB_Tel1.Text.Trim();
        strTel2 = TB_Tel2.Text.Trim();
        strFax = TB_Fax.Text.Trim();
        strEmailAddress = TB_EMailAddress.Text.Trim();
        strWebSite = TB_WebSite.Text.Trim();
        strZP = TB_ZP.Text.Trim();
        strCountry = TB_Country.Text.Trim();
        strState = TB_State.Text.Trim();
        strCity = TB_City.Text.Trim();
        strAreaAddress = TB_AreaAddress.Text.Trim();
        strRegistrationAddressCN = TB_AddressCN.Text.Trim();
        strRegistrationAddressEN = TB_AddressEN.Text.Trim();

        dtCreateDate = DateTime.Now;

        strBelongDepartCode = LB_BelongDepartCode.Text;
        strBelongDepartName = LB_BelongDepartName.Text;

        deDiscount = NB_Discount.Amount;
        deCreditRate = NB_CreditRate.Amount;

        deTaxRate = NB_TaxRate.Amount;
        strClearingForm = DL_ClearingForm.SelectedValue.Trim();

        strComment = TB_Comment.Text.Trim();

        if (strVendorCode != "" | strVendorName != "" | strType != "")
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
            vendor.AreaAddress = strAreaAddress;
            vendor.RegistrationAddressCN = strRegistrationAddressCN;
            vendor.RegistrationAddressEN = strRegistrationAddressEN;
            vendor.CreditRate = deCreditRate;
            vendor.Discount = deDiscount;
            vendor.Comment = strComment;
            vendor.CreatorCode = strUserCode;
            vendor.BelongDepartCode = strBelongDepartCode;
            vendor.BelongDepartName = strBelongDepartName;

            vendor.TaxRate = deTaxRate;
            vendor.ClearingForm = strClearingForm;

            vendor.DeviceName = TB_DeviceName.Text.Trim();

            try
            {
                vendorBLL.UpdateVendor(vendor, strVendorCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                LoadVendorList(strUserCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGYSDMMCHLXBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void BT_RelatedProject_Click(object sender, EventArgs e)
    {
        string strProjectID;
        string strVendorCode;

        strProjectID = TB_ProjectID.Text.Trim();
        strVendorCode = TB_VendorCode.Text.Trim();

        //判断项目是不是存在其管理范围
        if (checkProjectIsValid(strProjectID, LB_DepartString.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSJBCZJC").ToString().Trim() + "')", true);
            return;
        }

        ProjectVendorBLL projectVendorBLL = new ProjectVendorBLL();
        ProjectVendor projectVendor = new ProjectVendor();

        projectVendor.ProjectID = int.Parse(strProjectID);
        projectVendor.VendorCode = strVendorCode;

        try
        {
            projectVendorBLL.AddProjectVendor(projectVendor);
            LoadProjectList(strVendorCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGLCCJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    //判断项目是不是存在其管理范围
    protected bool checkProjectIsValid(string strProjectID, string strDepartString)
    {
        string strHQL;

        strHQL = "Select * From T_Project Where ProjectID = " + strProjectID;
        strHQL += " and BelongDepartCode in " + strDepartString;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strVendorCode, strVendorName;
        string strIndustryType, strDeviceName;

        strVendorCode = "%" + TB_VenCode.Text.Trim() + "%";
        strVendorName = "%" + TB_VenName.Text.Trim() + "%";
        strIndustryType = "%" + TB_IndustryTypeFind.Text.Trim() + "%";
        strDeviceName = "%" + TB_DeviceNameFind.Text.Trim() + "%";

        strHQL = "from Vendor as vendor where ";
        strHQL += " vendor.Type like  " + "'" + strIndustryType + "'";
        strHQL += " and vendor.VendorCode like " + "'" + strVendorCode + "'";
        strHQL += " and vendor.VendorName like  " + "'" + strVendorName + "'";
        strHQL += " and vendor.DeviceName like " + "'" + strDeviceName + "'";
        strHQL += " and vendor.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " Order by vendor.CreateDate DESC";

        VendorBLL vendorBLL = new VendorBLL();
        lst = vendorBLL.GetAllVendors(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strVendorCode, strOperatorCode;

        strVendorCode = TB_VendorCode.Text.Trim();

        if (strVendorCode != "")
        {
            strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

            VendorRelatedUserBLL vendorRelatedUserBLL = new VendorRelatedUserBLL();
            VendorRelatedUser vendorRelatedUser = new VendorRelatedUser();

            vendorRelatedUser.VendorCode = strVendorCode;
            vendorRelatedUser.UserCode = strOperatorCode;
            vendorRelatedUser.UserName = ShareClass.GetUserName(strOperatorCode);

            try
            {
                vendorRelatedUserBLL.AddVendorRelatedUser(vendorRelatedUser);

                strHQL = "from VendorRelatedUser as vendorRelatedUser where vendorRelatedUser.VendorCode = " + "'" + strVendorCode + "'";
                lst = vendorRelatedUserBLL.GetAllVendorRelatedUsers(strHQL);

                RP_VendorMember.DataSource = lst;
                RP_VendorMember.DataBind();
            }
            catch
            {
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZHTCNZJCY").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void RP_VendorMember_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            string strHQL, strVendorCode;
            IList lst;

            strVendorCode = TB_VendorCode.Text.Trim();
            strHQL = "from VendorRelatedUser as vendorRelatedUser where vendorRelatedUser.VendorCode = " + "'" + strVendorCode + "'" + " and vendorRelatedUser.UserName = " + "'" + strUserName + "'";
            VendorRelatedUserBLL vendorRelatedUserBLL = new VendorRelatedUserBLL();
            lst = vendorRelatedUserBLL.GetAllVendorRelatedUsers(strHQL);

            VendorRelatedUser vendorRelatedUser = (VendorRelatedUser)lst[0];

            vendorRelatedUserBLL.DeleteVendorRelatedUser(vendorRelatedUser);

            LoadVendorRelatedUser(strVendorCode);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_FindGroup_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strGroupName = TB_ActorGroupName.Text.Trim();
        strGroupName = "%" + strGroupName + "%";

        strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName not in ('Individual','Department','Company','Group','All')";  
        strHQL += " and GroupName Like " + "'" + strGroupName + "'";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);
        Repeater1.DataSource = lst;
        Repeater1.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            int i = 0;
            string strVendorCode, strGroupName;
            string strUserCode;

            strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();


            TB_ActorGroupName.Text = strGroupName;

            strVendorCode = TB_VendorCode.Text.Trim();

            VendorRelatedUserBLL vendorRelatedUserBLL = new VendorRelatedUserBLL();
            VendorRelatedUser vendorRelatedUser = new VendorRelatedUser();


            ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
            ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

            if (strVendorCode != "")
            {
                strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'";
                lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

                for (i = 0; i < lst.Count; i++)
                {
                    actorGroupDetail = (ActorGroupDetail)lst[i];

                    strUserCode = actorGroupDetail.UserCode.Trim();

                    vendorRelatedUser.VendorCode = strVendorCode;
                    vendorRelatedUser.UserCode = strUserCode;
                    vendorRelatedUser.UserName = ShareClass.GetUserName(strUserCode);


                    try
                    {
                        vendorRelatedUserBLL.AddVendorRelatedUser(vendorRelatedUser);
                    }
                    catch
                    {
                    }
                }

                LoadVendorRelatedUser(strVendorCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZGYSCNZJCY").ToString().Trim() + "')", true);
            }

        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void DataGrid12_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL, strID, strVendorCode;
            IList lst;

            strID = e.Item.Cells[2].Text;
            LB_ID.Text = strID;

            strVendorCode = TB_VendorCode.Text.Trim();

            for (int i = 0; i < DataGrid12.Items.Count; i++)
            {
                DataGrid12.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
                strHQL = "from VendorRelatedGoodsInfor as vendorRelatedGoodsInfor Where vendorRelatedGoodsInfor.VendorCode = " + "'" + strVendorCode + "'";

                VendorRelatedGoodsInforBLL vendorRelatedGoodsInforBLL = new VendorRelatedGoodsInforBLL();
                lst = vendorRelatedGoodsInforBLL.GetAllVendorRelatedGoodsInfors(strHQL);
                VendorRelatedGoodsInfor vendorRelatedGoodsInfor = (VendorRelatedGoodsInfor)lst[0];

                TB_GoodsCode.Text = vendorRelatedGoodsInfor.GoodsCode;
                TB_GoodsName.Text = vendorRelatedGoodsInfor.GoodsName;
                TB_ModelNumber.Text = vendorRelatedGoodsInfor.ModelNumber;
                TB_Spec.Text = vendorRelatedGoodsInfor.Spec;
                TB_Brand.Text = vendorRelatedGoodsInfor.Brand;

                DL_GoodsType.SelectedValue = vendorRelatedGoodsInfor.Type;
                DL_Unit.SelectedValue = vendorRelatedGoodsInfor.Unit;

                NB_Price.Amount = vendorRelatedGoodsInfor.Price;

                DL_Unit.SelectedValue = vendorRelatedGoodsInfor.Unit;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);
            }


            if (e.CommandName == "Delete")
            {
                VendorRelatedGoodsInforBLL vendorRelatedGoodsInforBLL = new VendorRelatedGoodsInforBLL();
                strHQL = "from VendorRelatedGoodsInfor as vendorRelatedGoodsInfor where vendorRelatedGoodsInfor.ID = " + strID;
                lst = vendorRelatedGoodsInforBLL.GetAllVendorRelatedGoodsInfors(strHQL);
                VendorRelatedGoodsInfor vendorRelatedGoodsInfor = (VendorRelatedGoodsInfor)lst[0];

                try
                {
                    vendorRelatedGoodsInforBLL.DeleteVendorRelatedGoodsInfor(vendorRelatedGoodsInfor);

                    LoadVendorRelatedGoodsList(strVendorCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected void DataGrid14_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strItemCode;

            strID = e.Item.Cells[0].Text;
            strItemCode = ((Button)e.Item.FindControl("BT_ItemCode")).Text.Trim();

            for (int i = 0; i < DataGrid14.Items.Count; i++)
            {
                DataGrid14.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Item as item where ItemCode = " + "'" + strItemCode + "'";
            ItemBLL itemBLL = new ItemBLL();
            lst = itemBLL.GetAllItems(strHQL);

            if (lst.Count > 0)
            {
                Item item = (Item)lst[0];

                TB_GoodsCode.Text = item.ItemCode;
                TB_GoodsName.Text = item.ItemName;
                try
                {
                    DL_GoodsType.SelectedValue = item.SmallType;
                }
                catch
                {

                }
                TB_ModelNumber.Text = item.ModelNumber;
                DL_Unit.SelectedValue = item.Unit;
                TB_Spec.Text = item.Specification;
                TB_Brand.Text = item.Brand;

                NB_Price.Amount = item.PurchasePrice;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);
        }
    }


    protected void DataGrid13_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID, strGoodsCode;

            strID = e.Item.Cells[0].Text;
            strGoodsCode = ((Button)e.Item.FindControl("BT_GoodsCode")).Text.Trim();

            for (int i = 0; i < DataGrid13.Items.Count; i++)
            {
                DataGrid13.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from Goods as goods where goods.ID = " + strID;
            GoodsBLL goodsBLL = new GoodsBLL();
            lst = goodsBLL.GetAllGoodss(strHQL);

            if (lst.Count > 0)
            {
                Goods goods = (Goods)lst[0];

                TB_GoodsCode.Text = goods.GoodsCode;
                TB_GoodsName.Text = goods.GoodsName;
                TB_ModelNumber.Text = goods.ModelNumber;
                DL_Unit.SelectedValue = goods.UnitName;
                TB_Spec.Text = goods.Spec;
                TB_Brand.Text = goods.Manufacturer;

                DL_GoodsType.SelectedValue = goods.Type;
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);
        }
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;


        //TabContainer1.ActiveTabIndex = 0;

        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        strType = "%" + strType + "%";
        strGoodsCode = "%" + strGoodsCode + "%";
        strGoodsName = "%" + strGoodsName + "%";
        strModelNumber = "%" + strModelNumber + "%";
        strSpec = "%" + strSpec + "%";

        strHQL = "Select * From T_Goods as goods Where goods.GoodsCode Like " + "'" + strGoodsCode + "'" + " and goods.GoodsName like " + "'" + strGoodsName + "'";
        strHQL += " and goods.Type Like " + "'" + strType + "'" + " and goods.ModelNumber Like " + "'" + strModelNumber + "'" + " and goods.Spec Like " + "'" + strSpec + "'";
        strHQL += " Order by goods.Number DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");
        DataGrid13.DataSource = ds;
        DataGrid13.DataBind();

        strHQL = "Select * From T_Item as item Where item.ItemCode Like " + "'" + strGoodsCode + "'" + " and item.ItemName like " + "'" + strGoodsName + "'";
        strHQL += " and item.Specification Like " + "'" + strSpec + "'";
        strHQL += " and item.BigType = 'Goods'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_Item");
        DataGrid14.DataSource = ds;
        DataGrid14.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);
    }

    protected void BT_Clear_Click(object sender, EventArgs e)
    {
        TB_GoodsCode.Text = "";
        TB_GoodsName.Text = "";
        TB_ModelNumber.Text = "";
        TB_Spec.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);
    }


    protected void BT_CreateGoodsList_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);
    }


    protected void BT_SaveGoods_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddGoods();
        }
        else
        {
            UpdateGoods();
        }
    }

    protected void AddGoods()
    {
        string strRecordID, strVendorCode, strType, strGoodsCode, strGoodsName, strModelNumber, strSpec, strStatus;
        string strUnitName;

        decimal dePrice;


        strVendorCode = TB_VendorCode.Text.Trim();
        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();

        strSpec = TB_Spec.Text.Trim();
        dePrice = NB_Price.Amount;


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);
        }
        else
        {
            VendorRelatedGoodsInforBLL vendorRelatedGoodsInforBLL = new VendorRelatedGoodsInforBLL();
            VendorRelatedGoodsInfor vendorRelatedGoodsInfor = new VendorRelatedGoodsInfor();

            vendorRelatedGoodsInfor.VendorCode = strVendorCode;
            vendorRelatedGoodsInfor.Type = strType;
            vendorRelatedGoodsInfor.GoodsCode = strGoodsCode;
            vendorRelatedGoodsInfor.GoodsName = strGoodsName;

            vendorRelatedGoodsInfor.ModelNumber = strModelNumber;
            vendorRelatedGoodsInfor.Spec = strSpec;
            vendorRelatedGoodsInfor.Brand = TB_Brand.Text;

            vendorRelatedGoodsInfor.Number = 0;
            vendorRelatedGoodsInfor.Unit = strUnitName;
            vendorRelatedGoodsInfor.Price = dePrice;


            try
            {
                vendorRelatedGoodsInforBLL.AddVendorRelatedGoodsInfor(vendorRelatedGoodsInfor);

                strRecordID = ShareClass.GetMyCreatedMaxVendorRelatedGoodsInforID(strVendorCode);
                LB_ID.Text = strRecordID;

                LoadVendorRelatedGoodsList(strVendorCode);


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);

            }
        }
    }

    protected void UpdateGoods()
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strUnitName;

        decimal dePrice, deNumber;

        string strID, strVendorCode;
        string strHQL;
        IList lst;


        strID = LB_ID.Text.Trim();

        strVendorCode = TB_VendorCode.Text.Trim();
        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        dePrice = NB_Price.Amount;


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popGoodsListWindow','false') ", true);
        }
        else
        {
            VendorRelatedGoodsInforBLL vendorRelatedGoodsInforBLL = new VendorRelatedGoodsInforBLL();
            strHQL = "from VendorRelatedGoodsInfor as vendorRelatedGoodsInfor where vendorRelatedGoodsInfor.ID = " + strID;
            lst = vendorRelatedGoodsInforBLL.GetAllVendorRelatedGoodsInfors(strHQL);
            VendorRelatedGoodsInfor vendorRelatedGoodsInfor = (VendorRelatedGoodsInfor)lst[0];

            vendorRelatedGoodsInfor.VendorCode = strVendorCode;
            vendorRelatedGoodsInfor.Type = strType;
            vendorRelatedGoodsInfor.GoodsCode = strGoodsCode;
            vendorRelatedGoodsInfor.GoodsName = strGoodsName;
            vendorRelatedGoodsInfor.ModelNumber = strModelNumber;
            vendorRelatedGoodsInfor.Spec = strSpec;
            vendorRelatedGoodsInfor.Brand = TB_Brand.Text;

            vendorRelatedGoodsInfor.Number = 0;
            vendorRelatedGoodsInfor.Unit = strUnitName;
            vendorRelatedGoodsInfor.Price = dePrice;



            try
            {
                vendorRelatedGoodsInforBLL.UpdateVendorRelatedGoodsInfor(vendorRelatedGoodsInfor, int.Parse(strID));

                LoadVendorRelatedGoodsList(strVendorCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);

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
            LB_BelongDepartCode.Text = strDepartCode;
            LB_BelongDepartName.Text = ShareClass.GetDepartName(strDepartCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void LoadVendorRelatedGoodsList(string strVendorCode)
    {
        string strHQL;
        IList lst;


        VendorRelatedGoodsInforBLL vendorRelatedGoodsInforBLL = new VendorRelatedGoodsInforBLL();
        strHQL = "from VendorRelatedGoodsInfor as vendorRelatedGoodsInfor where vendorRelatedGoodsInfor.VendorCode = " + "'" + strVendorCode + "'";
        lst = vendorRelatedGoodsInforBLL.GetAllVendorRelatedGoodsInfors(strHQL);

        DataGrid12.DataSource = lst;
        DataGrid12.DataBind();
    }



    protected void LoadVendorRelatedUser(string strVendorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from VendorRelatedUser as vendorRelatedUser where vendorRelatedUser.VendorCode = " + "'" + strVendorCode + "'";
        VendorRelatedUserBLL vendorRelatedUserBLL = new VendorRelatedUserBLL();
        lst = vendorRelatedUserBLL.GetAllVendorRelatedUsers(strHQL);

        RP_VendorMember.DataSource = lst;
        RP_VendorMember.DataBind();
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

        DL_IndustryTypeFind.DataSource = lst;
        DL_IndustryTypeFind.DataBind();

        DL_IndustryType.Items.Insert(0, new ListItem("--Select--", ""));
        DL_IndustryTypeFind.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadProjectList(string strVendorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID in (select projectVendor.ProjectID from ProjectVendor as projectVendor where projectVendor.VendorCode = " + "'" + strVendorCode + "'" + ")";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadVendorRelatedGoodsPurchaseOrder(string strVendorName)
    {
        string strHQL;

        strHQL = "Select * From T_GoodsPurchaseOrder Where Supplier  Like " + "'%" + strVendorName + "%'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsPurchaseOrder");

        DataGrid8.DataSource = ds;
        DataGrid8.DataBind();
    }

    protected void LoadVendorRelatedAssetPurchaseOrder(string strVendorName)
    {
        string strHQL;
        IList lst;

        strHQL = "From AssetPurchaseOrder as assetPurchaseOrder Where assetPurchaseOrder.POID in (Select assetPurRecord.POID From AssetPurRecord as assetPurRecord Where assetPurRecord.Supplier = " + "'" + strVendorName + "'" + ")";
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DataGrid10.DataSource = lst;
        DataGrid10.DataBind();
    }

    protected void LoadRelatedConstract(string strVendorCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.Status <> 'Archived'";
        strHQL += " and constract.RelatedVendorCode = " + "'" + strVendorCode + "'";
        strHQL += " Order by constract.SignDate DESC";
        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);
        DataGrid6.DataSource = lst;
        DataGrid6.DataBind();
    }


    protected void LoadVendorList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Vendor as vendor where vendor.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " Order by vendor.CreateDate DESC";

        VendorBLL vendorBLL = new VendorBLL();
        lst = vendorBLL.GetAllVendors(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

}
