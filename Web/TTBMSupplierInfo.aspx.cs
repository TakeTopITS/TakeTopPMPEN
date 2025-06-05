using ProjectMgt.BLL;
using ProjectMgt.Model;

using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTBMSupplierInfo : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "承包商登记", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_SetUpTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            LoadBMSupplierInfo();

            LoadSupplierBigType();
            LoadSupplierSmallType(DL_SupplierBigType.SelectedValue.Trim());

            string strHQL;
            IList lst;

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView2, strUserCode);

            strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName not in ('Individual','Department','Company','Group','All')";  
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            Repeater1.DataSource = lst;
            Repeater1.DataBind();

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
        }
    }

    protected void DL_SupplierBigType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strBigType;

        strBigType = DL_SupplierBigType.SelectedValue.Trim();
        LoadSupplierSmallType(strBigType);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;



                strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = " + strID;
                BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

                LB_SupplierInfoID.Text = bMSupplierInfo.ID.ToString();
                DL_CompanyType.SelectedValue = bMSupplierInfo.CompanyType.Trim();
                DL_Status.SelectedValue = bMSupplierInfo.Status.Trim();
                TB_Address.Text = bMSupplierInfo.Address.Trim();
                TB_Bank.Text = bMSupplierInfo.Bank.Trim();
                TB_BankNo.Text = bMSupplierInfo.BankNo.Trim();
                TB_Code.Text = bMSupplierInfo.Code.Trim();
                TB_CompanyFor.Text = bMSupplierInfo.CompanyFor.Trim();
                TB_EinNo.Text = bMSupplierInfo.EinNo.Trim();
                TB_Email.Text = bMSupplierInfo.Email.Trim();
                TB_Fax.Text = bMSupplierInfo.Fax.Trim();
                TB_Name.Text = bMSupplierInfo.Name.Trim();
                TB_PhoneNum.Text = bMSupplierInfo.PhoneNum.Trim();
                TB_Qualification.Text = bMSupplierInfo.Qualification.Trim();
                TB_SupplyScope.Text = bMSupplierInfo.SupplyScope.Trim();
                TB_WebUrl.Text = bMSupplierInfo.WebUrl.Trim();
                TB_ZipCode.Text = bMSupplierInfo.ZipCode.Trim();
                TB_BusinessLicense.Text = bMSupplierInfo.BusinessLicense;
                TB_EmployeesNum.Text = bMSupplierInfo.EmployeesNum.ToString();
                DL_IsLand.SelectedValue = bMSupplierInfo.IsLand.Trim();
                TB_ITNumber.Text = bMSupplierInfo.ITNumber.ToString();
                TB_LastFinalistsNumber.Text = bMSupplierInfo.LastFinalistsNumber.Trim();
                TB_LegalRepresentative.Text = bMSupplierInfo.LegalRepresentative.Trim();
                TB_LegalTel.Text = bMSupplierInfo.LegalTel.Trim();
                TB_MNumber.Text = bMSupplierInfo.MNumber.ToString();
                TB_PMNumber.Text = bMSupplierInfo.PMNumber.ToString();
                TB_PTNumber.Text = bMSupplierInfo.PTNumber.ToString();
                TB_QualificationCertificate.Text = bMSupplierInfo.QualificationCertificate.Trim();
                TB_RecommendedUnits.Text = bMSupplierInfo.RecommendedUnits.Trim();
                TB_RegisteredCapital.Text = bMSupplierInfo.RegisteredCapital.ToString();
                DLC_SetUpTime.Text = bMSupplierInfo.SetUpTime.ToString("yyyy-MM-dd");
                TB_STNumber.Text = bMSupplierInfo.STNumber.ToString();
                TB_SubcontractProfessional.Text = bMSupplierInfo.SubcontractProfessional.Trim();
                TB_TechnicalDirector.Text = bMSupplierInfo.TechnicalDirector.Trim();
                TB_TechnicalTel.Text = bMSupplierInfo.TechnicalTel.Trim();
                TB_TechnicalTitles.Text = bMSupplierInfo.TechnicalTitles.Trim();
                TB_TechnicalTitles_T.Text = bMSupplierInfo.TechnicalTitles_T.Trim();

                DL_SupplierBigType.SelectedValue = bMSupplierInfo.SupplierBigType;
                LoadSupplierSmallType(DL_SupplierBigType.SelectedValue.Trim());
                TB_SupplierSmallType.Text = bMSupplierInfo.SupplierSmallType.Trim();

                HL_Accessories.NavigateUrl = bMSupplierInfo.AccessoriesPath;

                if (bMSupplierInfo.EnginerringSupplier.Trim() == "YES")
                {
                    CB_IsEnginerringSupplier.Checked = true;
                }
                else
                {
                    CB_IsEnginerringSupplier.Checked = false;
                }
                if (bMSupplierInfo.InternationSupplier.Trim() == "YES")
                {
                    CB_IsInternationSupplier.Checked = true;
                }
                else
                {
                    CB_IsInternationSupplier.Checked = false;
                }
                if (bMSupplierInfo.RetailSupplier.Trim() == "YES")
                {
                    CB_IsRetailSupplier.Checked = true;
                }
                else
                {
                    CB_IsRetailSupplier.Checked = false;
                }
                if (bMSupplierInfo.StoreSupplier.Trim() == "YES")
                {
                    CB_IsStoreSupplier.Checked = true;
                }
                else
                {
                    CB_IsStoreSupplier.Checked = false;
                }

                LoadSupplierInfoAttach(bMSupplierInfo.ID.ToString());
                LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());

                LoadProjectList(bMSupplierInfo.Code.Trim());

                LoadVendorRelatedGoodsPurchaseOrder(bMSupplierInfo.Name.Trim());
                LoadVendorRelatedAssetPurchaseOrder(bMSupplierInfo.Name.Trim());
                LoadRelatedConstract(bMSupplierInfo.Code.Trim());
                LoadVendorRelatedUser(bMSupplierInfo.Code.Trim());
                LoadVendorRelatedGoodsList(bMSupplierInfo.Code.Trim());


                //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                if (bMSupplierInfo.EnterPer.Trim() == strUserCode.Trim())
                {
                    //BT_UpdateAA.Visible = true;
                    //BT_UpdateAA.Enabled = true;
                    //BT_DeleteAA.Visible = true;
                    //BT_DeleteAA.Enabled = true;
                    if (bMSupplierInfo.AccessoriesPath.Trim() == "")
                    {
                        BT_DeleteAccessories.Visible = false;
                    }
                    else
                    {
                        BT_DeleteAccessories.Enabled = true;
                        BT_DeleteAccessories.Visible = true;
                    }

                    //BT_AddFile.Enabled = true;
                }
                else
                {
                    BT_DeleteAccessories.Visible = false;
                    //BT_UpdateAA.Visible = false;
                    //BT_DeleteAA.Visible = false;
                }
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                Delete();

            }
        }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_SupplierInfoID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_SupplierInfoID.Text;

        if (strID == "")
        {
            Add();
        }
        else
        {
            Update();
        }
    }

    protected void Add()
    {
        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMYMCDBNWKJC") + "')", true);
            return;
        }
        if (TB_Code.Text.Trim().Contains("-") || TB_Code.Text.Trim().Contains(","))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMGZYWBNBHDJC") + "')", true);
            TB_Code.Focus();
            return;
        }
        if (IsBMSupplierInfo(TB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWGYBMYCZJC") + "')", true);
            return;
        }

        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        BMSupplierInfo bMSupplierInfo = new BMSupplierInfo();

        bMSupplierInfo.Address = TB_Address.Text.Trim();
        bMSupplierInfo.Bank = TB_Bank.Text.Trim();
        bMSupplierInfo.BankNo = TB_BankNo.Text.Trim();
        bMSupplierInfo.Code = TB_Code.Text.Trim();
        bMSupplierInfo.CompanyFor = TB_CompanyFor.Text.Trim();
        bMSupplierInfo.CompanyType = DL_CompanyType.SelectedValue.Trim();
        bMSupplierInfo.EinNo = TB_EinNo.Text.Trim();
        bMSupplierInfo.Email = TB_Email.Text.Trim();
        bMSupplierInfo.EnterDate = DateTime.Now;
        bMSupplierInfo.EnterPer = strUserCode.Trim();
        bMSupplierInfo.Fax = TB_Fax.Text.Trim();
        bMSupplierInfo.Name = TB_Name.Text.Trim();
        bMSupplierInfo.PhoneNum = TB_PhoneNum.Text.Trim();
        bMSupplierInfo.Qualification = TB_Qualification.Text.Trim();
        bMSupplierInfo.Remark = "";
        bMSupplierInfo.Status = DL_Status.SelectedValue.Trim();
        bMSupplierInfo.SupplyScope = TB_SupplyScope.Text.Trim();
        bMSupplierInfo.WebUrl = TB_WebUrl.Text.Trim();
        bMSupplierInfo.ZipCode = TB_ZipCode.Text.Trim();
        bMSupplierInfo.SubcontractProfessional = TB_SubcontractProfessional.Text.Trim();

        string strAccessoriesPath = UploadAttachMain();
        if (strAccessoriesPath.Equals("0"))
        {
            bMSupplierInfo.AccessoriesPath = "";
        }
        else if (strAccessoriesPath.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "')", true);
            return;
        }
        else if (strAccessoriesPath.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
            return;
        }
        else
        {
            bMSupplierInfo.AccessoriesPath = strAccessoriesPath;
        }

        bMSupplierInfo.BusinessLicense = TB_BusinessLicense.Text.Trim();
        bMSupplierInfo.EmployeesNum = int.Parse(TB_EmployeesNum.Text.Trim() == "" ? "0" : TB_EmployeesNum.Text.Trim());
        bMSupplierInfo.IsLand = DL_IsLand.SelectedValue.Trim();
        bMSupplierInfo.ITNumber = int.Parse(TB_ITNumber.Text.Trim() == "" ? "0" : TB_ITNumber.Text.Trim());
        bMSupplierInfo.LastFinalistsNumber = TB_LastFinalistsNumber.Text.Trim();
        bMSupplierInfo.LegalRepresentative = TB_LegalRepresentative.Text.Trim();
        bMSupplierInfo.LegalTel = TB_LegalTel.Text.Trim();
        bMSupplierInfo.MNumber = int.Parse(TB_MNumber.Text.Trim() == "" ? "0" : TB_MNumber.Text.Trim());
        bMSupplierInfo.PMNumber = int.Parse(TB_PMNumber.Text.Trim() == "" ? "0" : TB_PMNumber.Text.Trim());
        bMSupplierInfo.PTNumber = int.Parse(TB_PTNumber.Text.Trim() == "" ? "0" : TB_PTNumber.Text.Trim());
        bMSupplierInfo.QualificationCertificate = TB_QualificationCertificate.Text.Trim();
        bMSupplierInfo.RecommendedUnits = TB_RecommendedUnits.Text.Trim();
        bMSupplierInfo.RegisteredCapital = decimal.Parse(TB_RegisteredCapital.Text.Trim() == "" ? "0" : TB_RegisteredCapital.Text.Trim());
        bMSupplierInfo.SetUpTime = DateTime.Parse(DLC_SetUpTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_SetUpTime.Text) ? DateTime.Now.ToString() : DLC_SetUpTime.Text.Trim());
        bMSupplierInfo.STNumber = int.Parse(TB_STNumber.Text.Trim() == "" ? "0" : TB_STNumber.Text.Trim());
        bMSupplierInfo.TechnicalDirector = TB_TechnicalDirector.Text.Trim();
        bMSupplierInfo.TechnicalTel = TB_TechnicalTel.Text.Trim();
        bMSupplierInfo.TechnicalTitles = TB_TechnicalTitles.Text.Trim();
        bMSupplierInfo.TechnicalTitles_T = TB_TechnicalTitles_T.Text.Trim();

        bMSupplierInfo.PassWord = "12345678";
        bMSupplierInfo.ReviewDate = DateTime.Now;
        bMSupplierInfo.Reviewer = ShareClass.GetUserName(strUserCode.Trim());
        bMSupplierInfo.Point = 0;

        bMSupplierInfo.AccessoriesPath = HL_Accessories.NavigateUrl;

        if (CB_IsEnginerringSupplier.Checked == true)
        {
            bMSupplierInfo.EnginerringSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.EnginerringSupplier = "NO";
        }
        if (CB_IsInternationSupplier.Checked == true)
        {
            bMSupplierInfo.InternationSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.InternationSupplier = "NO";
        }
        if (CB_IsRetailSupplier.Checked == true)
        {
            bMSupplierInfo.RetailSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.RetailSupplier = "NO";
        }

        if (CB_IsStoreSupplier.Checked == true)
        {
            bMSupplierInfo.StoreSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.StoreSupplier = "NO";
        }


        bMSupplierInfo.SupplierBigType = DL_SupplierBigType.SelectedValue.Trim();
        bMSupplierInfo.SupplierSmallType = TB_SupplierSmallType.Text.Trim();

        try
        {
            bMSupplierInfoBLL.AddBMSupplierInfo(bMSupplierInfo);
            LB_SupplierInfoID.Text = GetBMSupplierInfoID(bMSupplierInfo.Code.Trim()).ToString();

            LoadSupplierInfoAttach(LB_SupplierInfoID.Text.Trim());

            if (bMSupplierInfo.AccessoriesPath.Trim() == "")
            {
                BT_DeleteAccessories.Visible = false;
            }
            else
            {
                BT_DeleteAccessories.Enabled = true;
                BT_DeleteAccessories.Visible = true;
            }

            //BT_UpdateAA.Enabled = true;
            //BT_UpdateAA.Visible = true;
            //BT_DeleteAA.Enabled = true;
            //BT_DeleteAA.Visible = true;

            //BT_AddFile.Enabled = true;

            LoadBMSupplierInfo();

            DL_Status.SelectedValue = "New";

            string strMsg = LanguageHandle.GetWord("XinJianChengGongQingJiShiZaiQi");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + strMsg + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void Update()
    {
        string strID = LB_SupplierInfoID.Text.Trim();

        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMYMCDBNWKJC") + "')", true);
            return;
        }
        if (TB_Code.Text.Trim().Contains("-") || TB_Code.Text.Trim().Contains(","))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMGZYWBNBHDJC") + "')", true);
            TB_Code.Focus();
            return;
        }

        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strID + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

        if (!TB_Code.Text.Trim().Equals(bMSupplierInfo.Code.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMBNGGJC") + "')", true);
            return;
        }

        bMSupplierInfo.Address = TB_Address.Text.Trim();
        bMSupplierInfo.Bank = TB_Bank.Text.Trim();
        bMSupplierInfo.BankNo = TB_BankNo.Text.Trim();
        bMSupplierInfo.CompanyFor = TB_CompanyFor.Text.Trim();
        bMSupplierInfo.CompanyType = DL_CompanyType.SelectedValue.Trim();
        bMSupplierInfo.EinNo = TB_EinNo.Text.Trim();
        bMSupplierInfo.Email = TB_Email.Text.Trim();
        bMSupplierInfo.Fax = TB_Fax.Text.Trim();
        bMSupplierInfo.Name = TB_Name.Text.Trim();
        bMSupplierInfo.PhoneNum = TB_PhoneNum.Text.Trim();
        bMSupplierInfo.Qualification = TB_Qualification.Text.Trim();
        bMSupplierInfo.Status = DL_Status.SelectedValue.Trim();
        bMSupplierInfo.SupplyScope = TB_SupplyScope.Text.Trim();
        bMSupplierInfo.WebUrl = TB_WebUrl.Text.Trim();
        bMSupplierInfo.ZipCode = TB_ZipCode.Text.Trim();
        bMSupplierInfo.SubcontractProfessional = TB_SubcontractProfessional.Text.Trim();

        string strAccessoriesPath = UploadAttachMain();
        if (strAccessoriesPath.Equals("0"))
        {
        }
        else if (strAccessoriesPath.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "')", true);
            return;
        }
        else if (strAccessoriesPath.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
            return;
        }
        else
        {
            bMSupplierInfo.AccessoriesPath = strAccessoriesPath;
        }

        bMSupplierInfo.BusinessLicense = TB_BusinessLicense.Text.Trim();
        bMSupplierInfo.EmployeesNum = int.Parse(TB_EmployeesNum.Text.Trim() == "" ? "0" : TB_EmployeesNum.Text.Trim());
        bMSupplierInfo.IsLand = DL_IsLand.SelectedValue.Trim();
        bMSupplierInfo.ITNumber = int.Parse(TB_ITNumber.Text.Trim() == "" ? "0" : TB_ITNumber.Text.Trim());
        bMSupplierInfo.LastFinalistsNumber = TB_LastFinalistsNumber.Text.Trim();
        bMSupplierInfo.LegalRepresentative = TB_LegalRepresentative.Text.Trim();
        bMSupplierInfo.LegalTel = TB_LegalTel.Text.Trim();
        bMSupplierInfo.MNumber = int.Parse(TB_MNumber.Text.Trim() == "" ? "0" : TB_MNumber.Text.Trim());
        bMSupplierInfo.PMNumber = int.Parse(TB_PMNumber.Text.Trim() == "" ? "0" : TB_PMNumber.Text.Trim());
        bMSupplierInfo.PTNumber = int.Parse(TB_PTNumber.Text.Trim() == "" ? "0" : TB_PTNumber.Text.Trim());
        bMSupplierInfo.QualificationCertificate = TB_QualificationCertificate.Text.Trim();
        bMSupplierInfo.RecommendedUnits = TB_RecommendedUnits.Text.Trim();
        bMSupplierInfo.RegisteredCapital = decimal.Parse(TB_RegisteredCapital.Text.Trim() == "" ? "0" : TB_RegisteredCapital.Text.Trim());
        bMSupplierInfo.SetUpTime = DateTime.Parse(DLC_SetUpTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_SetUpTime.Text) ? DateTime.Now.ToString() : DLC_SetUpTime.Text.Trim());
        bMSupplierInfo.STNumber = int.Parse(TB_STNumber.Text.Trim() == "" ? "0" : TB_STNumber.Text.Trim());
        bMSupplierInfo.TechnicalDirector = TB_TechnicalDirector.Text.Trim();
        bMSupplierInfo.TechnicalTel = TB_TechnicalTel.Text.Trim();
        bMSupplierInfo.TechnicalTitles = TB_TechnicalTitles.Text.Trim();
        bMSupplierInfo.TechnicalTitles_T = TB_TechnicalTitles_T.Text.Trim();

        bMSupplierInfo.AccessoriesPath = HL_Accessories.NavigateUrl;

        if (CB_IsEnginerringSupplier.Checked == true)
        {
            bMSupplierInfo.EnginerringSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.EnginerringSupplier = "NO";
        }
        if (CB_IsInternationSupplier.Checked == true)
        {
            bMSupplierInfo.InternationSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.InternationSupplier = "NO";
        }
        if (CB_IsRetailSupplier.Checked == true)
        {
            bMSupplierInfo.RetailSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.RetailSupplier = "NO";
        }

        if (CB_IsStoreSupplier.Checked == true)
        {
            bMSupplierInfo.StoreSupplier = "YES";
        }
        else
        {
            bMSupplierInfo.StoreSupplier = "NO";
        }

        bMSupplierInfo.SupplierBigType = DL_SupplierBigType.SelectedValue.Trim();
        bMSupplierInfo.SupplierSmallType = TB_SupplierSmallType.Text.Trim();

        try
        {
            bMSupplierInfoBLL.UpdateBMSupplierInfo(bMSupplierInfo, bMSupplierInfo.ID);

            LoadSupplierInfoAttach(bMSupplierInfo.ID.ToString());

            if (bMSupplierInfo.AccessoriesPath.Trim() == "")
            {
                BT_DeleteAccessories.Visible = false;
            }
            else
            {
                BT_DeleteAccessories.Enabled = true;
                BT_DeleteAccessories.Visible = true;
            }

            //BT_UpdateAA.Enabled = true;
            //BT_UpdateAA.Visible = true;
            //BT_DeleteAA.Enabled = true;
            //BT_DeleteAA.Visible = true;

            LoadBMSupplierInfo();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void Delete()
    {
        string strID;
        string strHQL;
        if (IsSupplierInfoExits(TB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSGYYKTZHHZHYJHBNSC") + "')", true);
            return;
        }

        strID = LB_SupplierInfoID.Text.Trim();

        strHQL = "delete from T_BMSupplierInfo where ID = '" + strID + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSupplierInfoAttach("0");

            BT_DeleteAccessories.Visible = false;
            //BT_DeleteAA.Visible = false;
            //BT_UpdateAA.Visible = false;
            //BT_AddFile.Enabled = false;

            LoadBMSupplierInfo();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void BT_UploadAccessories_Click(object sender, EventArgs e)
    {
        try
        {
            HL_Accessories.NavigateUrl = UploadAttachMain();
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected string UploadAttachMain()
    {
        //上传附件
        if (InputFile1.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.InputFile1.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                return "1";
            }
            else
            {
                try
                {
                    InputFile1.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);


                    return "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                }
                catch
                {
                    return "2";
                }
            }
        }
        else
        {
            return "0";
        }
    }


    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID, strHQL;
            IList lst;

            if (e.CommandName != "Page")
            {
                strID = e.Item.Cells[2].Text.Trim();

                lbl_CerID.Text = strID;

                if (e.CommandName == "Update")
                {
                    for (int i = 0; i < DataGrid3.Items.Count; i++)
                    {
                        DataGrid3.Items[i].ForeColor = Color.Black;
                    }
                    e.Item.ForeColor = Color.Red;

                    strHQL = " from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = '" + strID + "' ";
                    BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
                    lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);
                    BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];


                    TB_CertificateName.Text = bMSupplierCertification.CertificateName.Trim();
                    TB_CertificateNum.Text = bMSupplierCertification.CertificateNum.Trim();
                    TB_LicenseAgency.Text = bMSupplierCertification.LicenseAgency.Trim();
                    DLC_ReleaseTime.Text = bMSupplierCertification.ReleaseTime.ToString("yyyy-MM-dd");
                    HL_DetailFile.NavigateUrl = bMSupplierCertification.Attach;

                    //BT_AddFile.Enabled = true;
                    //BT_UpdateFile.Enabled = true;
                    //BT_DeleteFile.Enabled = true;

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popFileWindow') ", true);
                }

                if (e.CommandName == "Delete")
                {
                    DeleteFile();

                }
            }
        }
    }

    protected void BT_CreateFile_Click(object sender, EventArgs e)
    {
        lbl_CerID.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popFileWindow') ", true);
    }

    protected void BT_NewFile_Click(object sender, EventArgs e)
    {
        string strID;

        strID = lbl_CerID.Text;

        if (strID == "")
        {
            AddFile();
        }
        else
        {
            UpdateFile();
        }
    }

    protected void AddFile()
    {
        if (string.IsNullOrEmpty(LB_SupplierInfoID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCBSXXXZLXRXXSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popFileWindow') ", true);

            return;
        }

        string strAttach = UploadAttachDetail();

        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        BMSupplierCertification bMSupplierCertification = new BMSupplierCertification();
        bMSupplierCertification.CertificateName = TB_CertificateName.Text.Trim();
        bMSupplierCertification.CertificateNum = TB_CertificateNum.Text.Trim();
        bMSupplierCertification.LicenseAgency = TB_LicenseAgency.Text.Trim();
        bMSupplierCertification.ReleaseTime = DateTime.Parse(DLC_ReleaseTime.Text.Trim());
        bMSupplierCertification.SupplierCode = GetBMSupplierInfoCode(LB_SupplierInfoID.Text.Trim());
        if (strAttach.Equals("0"))
        {
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popFileWindow') ", true);

            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popFileWindow') ", true);

            return;
        }
        else
        {
            bMSupplierCertification.Attach = strAttach;
        }
        bMSupplierCertification.Attach = HL_DetailFile.NavigateUrl;

        try
        {
           

            bMSupplierCertificationBLL.AddBMSupplierCertification(bMSupplierCertification);
            lbl_CerID.Text = GetBMSupplierCertificationID(bMSupplierCertification.SupplierCode.Trim()).ToString();
            //BT_UpdateFile.Enabled = true;
            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZSXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZSXZSB") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }


    protected void UpdateFile()
    {
        string strHQL;
        IList lst;

        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        strHQL = "from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = '" + lbl_CerID.Text.Trim() + "' ";
        lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);
        BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];

        string strAttach = UploadAttachDetail();
        if (strAttach.Equals("0"))
        {
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC") + "')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
            return;
        }
        else
        {
            bMSupplierCertification.Attach = strAttach;
        }

        bMSupplierCertification.Attach = HL_DetailFile.NavigateUrl;
        bMSupplierCertification.CertificateName = TB_CertificateName.Text.Trim();
        bMSupplierCertification.CertificateNum = TB_CertificateNum.Text.Trim();
        bMSupplierCertification.LicenseAgency = TB_LicenseAgency.Text.Trim();
        bMSupplierCertification.ReleaseTime = DateTime.Parse(DLC_ReleaseTime.Text.Trim());
      

        try
        {
            bMSupplierCertificationBLL.UpdateBMSupplierCertification(bMSupplierCertification, bMSupplierCertification.ID);

            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSB") + "')", true);
        }


        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }


    protected void DeleteFile()
    {
        string strHQL;
        IList lst;

        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        strHQL = "from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = " + lbl_CerID.Text.Trim();

        lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);
        BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];

        try
        {
            bMSupplierCertificationBLL.DeleteBMSupplierCertification(bMSupplierCertification);

            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSB") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_AttachDetailFile_Click(object sender, EventArgs e)
    {
        try
        {
            HL_DetailFile.NavigateUrl = UploadAttachDetail();
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popFileWindow') ", true);
    }

    protected string UploadAttachDetail()
    {
        //上传附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            strFileName1 = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            strExtendName = System.IO.Path.GetExtension(strFileName1);//获取扩展名

            DateTime dtUploadNow = DateTime.Now; //获取系统时间

            string strFileName2 = System.IO.Path.GetFileName(strFileName1);
            string strExtName = Path.GetExtension(strFileName2);

            string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";

            FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

            if (fi.Exists)
            {
                return "1";
            }
            else
            {
                try
                {
                    AttachFile.MoveTo(strDocSavePath + strFileName3, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                    return "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                }
                catch
                {
                    return "2";
                }
            }
        }
        else
        {

            return "0";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }



    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strVendorCode, strOperatorCode;

        strVendorCode = TB_Code.Text.Trim();

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZHTCNZJCY") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }



    /// <summary>
    /// 取得供应商证书当前对应的ID号
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected int GetBMSupplierCertificationID(string strCode)
    {
        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        string strHQL = "from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.SupplierCode = '" + strCode + "' Order by ID Desc ";
        IList lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);

        BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];

        return bMSupplierCertification.ID;
    }




    protected void LoadSupplierBigType()
    {
        string strHQL;

        strHQL = "Select * From T_BMSupplierBigType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SupplierBigType");

        DL_SupplierBigType.DataSource = ds;
        DL_SupplierBigType.DataBind();
    }

    protected void LoadSupplierSmallType(string strBigType)
    {
        string strHQL;

        strHQL = "Select * From T_BMSupplierSmallType Where BigType = '" + strBigType + "'";
        strHQL += " Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SupplierBigType");

        DL_SupplierSmallType.DataSource = ds;
        DL_SupplierSmallType.DataBind();

        DL_SupplierSmallType.Items.Insert(0, new ListItem("--Select--", ""));
    }


    protected void LoadBMSupplierCertification(string strCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.SupplierCode = '" + strCode + "' Order by bMSupplierCertification.ID DESC";
        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }


    protected void LoadSupplierInfoAttach(string strId)
    {
        //绑定附件列表
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strId + "' ";
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
            if (!string.IsNullOrEmpty(bMSupplierInfo.AccessoriesPath) && bMSupplierInfo.AccessoriesPath.Trim() != "")
            {
                RP_AccessoriesPath.DataSource = lst;
                RP_AccessoriesPath.DataBind();
            }
            else
            {
                RP_AccessoriesPath.DataSource = null;
                RP_AccessoriesPath.DataBind();
            }
        }
    }
    protected void DL_SupplierSmallType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSmallType, strSmallTypeString, strSmallTypeString1;

        strSmallType = DL_SupplierSmallType.SelectedValue.Trim();
        strSmallTypeString = TB_SupplierSmallType.Text.Trim();
        strSmallTypeString1 = strSmallTypeString;

        //if (strSmallTypeString.Length > 0)
        //{
        //    if (strSmallTypeString.Substring(strSmallTypeString1.Length - 1, 1) != ",")
        //    {
        //        strSmallTypeString += ",";
        //    }
        //}

        if (strSmallType != "" & strSmallTypeString.IndexOf(strSmallType + ",") < 0)
        {
            TB_SupplierSmallType.Text += strSmallType + ",";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadBMSupplierInfo()
    {
        string strHQL;

        strHQL = "Select * From T_BMSupplierInfo Where 1=1 ";
        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and (Code like '%" + txt_SupplierInfo.Text.Trim() + "%' or Name like '%" + txt_SupplierInfo.Text.Trim() + "%' or CompanyFor like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
                "or SupplyScope like '%" + txt_SupplierInfo.Text.Trim() + "%' or Qualification like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-EnterDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-EnterDate::date>=0 ";
        }
        strHQL += " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }


    protected void BT_RelatedProject_Click(object sender, EventArgs e)
    {
        string strProjectID;
        string strVendorCode;

        strProjectID = TB_ProjectID.Text.Trim();
        strVendorCode = TB_Code.Text.Trim();

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGLCCJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strVendorCode, strProjectID;

        if (e.CommandName != "Page")
        {
            strVendorCode = TB_Code.Text.Trim();
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);
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

            strVendorCode = TB_Code.Text.Trim();
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

            strVendorCode = TB_Code.Text.Trim();

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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZGYSCNZJCY") + "')", true);
            }

        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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


        strVendorCode = TB_Code.Text.Trim();
        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();

        strSpec = TB_Spec.Text.Trim();
        dePrice = NB_Price.Amount;


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);

            }
        }
    }

    protected void UpdateGoods()
    {
        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;
        string strUnitName;

        decimal dePrice;

        string strID, strVendorCode;
        string strHQL;
        IList lst;


        strID = LB_ID.Text.Trim();

        strVendorCode = TB_Code.Text.Trim();
        strType = DL_GoodsType.SelectedValue.Trim();
        strGoodsCode = TB_GoodsCode.Text.Trim();
        strGoodsName = TB_GoodsName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();

        dePrice = NB_Price.Amount;


        if (strType == "" | strGoodsName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC") + "')", true);

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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);


                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popGoodsListWindow') ", true);

            }
        }
    }

    protected void BT_FindGoods_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strType, strGoodsCode, strGoodsName, strModelNumber, strSpec;

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


    protected void DataGrid12_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL, strID, strVendorCode;
            IList lst;

            strID = e.Item.Cells[2].Text;
            LB_ID.Text = strID;

            strVendorCode = TB_Code.Text.Trim();

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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
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


    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMSupplierInfo();
    }

    protected void BT_DeleteAccessories_Click(object sender, EventArgs e)
    {
        string strID = LB_SupplierInfoID.Text.Trim();
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strID + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst != null && lst.Count > 0)
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
            bMSupplierInfo.AccessoriesPath = "";
            try
            {
                bMSupplierInfoBLL.UpdateBMSupplierInfo(bMSupplierInfo, bMSupplierInfo.ID);

                BT_DeleteAccessories.Visible = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZFuJianLanguageHandleGetWordZ") + LanguageHandle.GetWord("ZZSCCG") + "')", true); 
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZFuJianLanguageHandleGetWordZ") + LanguageHandle.GetWord("ZZSCCG") + "')", true); 
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
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
        IList lst;

        strHQL = "From GoodsPurchaseOrder as goodsPurchaseOrder Where goodsPurchaseOrder.Supplier = " + "'" + strVendorName + "'";
        GoodsPurchaseOrderBLL goodsPurchaseOrderBLL = new GoodsPurchaseOrderBLL();
        lst = goodsPurchaseOrderBLL.GetAllGoodsPurchaseOrders(strHQL);

        DataGrid8.DataSource = lst;
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


    /// <summary>
    /// 判断该供应商是否已开通帐号或激活帐号
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected bool IsSupplierInfoExits(string strCode)
    {
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strCode + "' ";
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 判断编码在后台用户及供应商信息中是否存在
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected bool IsBMSupplierInfo(string strCode)
    {
        bool flag1 = true, flag2 = false;
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.Code = '" + strCode + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag1 = true;
        }
        else
        {
            flag1 = false;
        }

        return flag1;


        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strCode + "' ";
        //lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        //if (lst.Count > 0 && lst != null)
        //{
        //    flag2 = true;
        //}
        //else
        //    flag2 = false;

        //if (flag1 || flag2)
        //    return true;
        //else
        //return false;
    }

    /// <summary>
    /// 取得供应商对应的ID号
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected int GetBMSupplierInfoID(string strCode)
    {
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.Code = '" + strCode + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);

        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

        return bMSupplierInfo.ID;
    }

    /// <summary>
    /// 取得供应商编码
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected string GetBMSupplierInfoCode(string strID)
    {
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strID + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);

        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

        return bMSupplierInfo.Code.Trim();
    }

}
