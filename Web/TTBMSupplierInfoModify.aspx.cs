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
using System.Data.SqlClient;
using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;
using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using System.IO;

public partial class TTBMSupplierInfoModify : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ReleaseTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SetUpTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
            GetBMSupplierInfoData(strUserCode.Trim());
        }
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

    protected string UploadAttach1()
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

    protected void GetBMSupplierInfoData(string strCode)
    {
        string strHQL;
        IList lst;
        lbl_Code.Text = LanguageHandle.GetWord("GeRenZhangHao").ToString().Trim() + strCode.Trim();
        if (strCode.Trim().Contains("-"))
        {
            strCode = strCode.Trim().Substring(0, strCode.Trim().IndexOf("-"));
        }
        strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.Code = '" + strCode + "' ";
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

            LB_SupplierInfoID.Text = bMSupplierInfo.ID.ToString();
            DL_CompanyType.SelectedValue = bMSupplierInfo.CompanyType.Trim();
            TB_Address.Text = bMSupplierInfo.Address.Trim();
            TB_Bank.Text = bMSupplierInfo.Bank.Trim();
            TB_BankNo.Text = bMSupplierInfo.BankNo.Trim();
            TB_Code.Text = bMSupplierInfo.Code.Trim();
            TB_CompanyFor.Text = bMSupplierInfo.CompanyFor.Trim();
            TB_EinNo.Text = bMSupplierInfo.EinNo.Trim();
            TB_Email.Text = bMSupplierInfo.Email.Trim();
            TB_Fax.Text = bMSupplierInfo.Fax.Trim();
            TB_Name.Text = bMSupplierInfo.Name.Trim();
            TB_PassWord.Text = bMSupplierInfo.PassWord.Trim();
            TB_PhoneNum.Text = bMSupplierInfo.PhoneNum.Trim();
            TB_Qualification.Text = bMSupplierInfo.Qualification.Trim();
            TB_SupplyScope.Text = bMSupplierInfo.SupplyScope.Trim();
            TB_WebUrl.Text = bMSupplierInfo.WebUrl.Trim();
            TB_ZipCode.Text = bMSupplierInfo.ZipCode.Trim();
            DL_Status.SelectedValue = bMSupplierInfo.Status.Trim();
            TB_SubcontractProfessional.Text = bMSupplierInfo.SubcontractProfessional.Trim();

            TB_BusinessLicense.Text = bMSupplierInfo.BusinessLicense.Trim();
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
            TB_TechnicalDirector.Text = bMSupplierInfo.TechnicalDirector.Trim();
            TB_TechnicalTel.Text = bMSupplierInfo.TechnicalTel.Trim();
            TB_TechnicalTitles.Text = bMSupplierInfo.TechnicalTitles.Trim();
            TB_TechnicalTitles_T.Text = bMSupplierInfo.TechnicalTitles_T.Trim();

            TB_SupplierBigType.Text = bMSupplierInfo.SupplierBigType.Trim();
            TB_SupplierSmallType.Text = bMSupplierInfo.SupplierSmallType.Trim();

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
        }
        else
        {
            TB_Code.Text = strCode;

            LoadSupplierInfoAttach("0");
        }

        LoadBMSupplierLink(strCode);
        LoadBMSupplierCertification(strCode);
        //BT_New.Enabled = true;
        //BT_Update.Enabled = false;
        //BT_Delete.Enabled = false;
        BT_UpdateAA.Enabled = true;
        //Button1.Enabled = true;
        //Button2.Enabled = false;
        //Button3.Enabled = false;
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = " from BMSupplierLink as bMSupplierLink where bMSupplierLink.ID = '" + strID + "' ";

                BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
                lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
                BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[0];

                LB_ID.Text = bMSupplierLink.ID.ToString();
                TB_NameLink.Text = bMSupplierLink.Name.Trim();
                TB_Position.Text = bMSupplierLink.Position.Trim();
                DL_Gender.SelectedValue = bMSupplierLink.Gender.Trim();
                TB_OfficePhone.Text = bMSupplierLink.OfficePhone.Trim();
                TB_MobileNum.Text = bMSupplierLink.MobileNum.Trim();
                TB_EmailLink.Text = bMSupplierLink.Email.Trim();

                //BT_New.Enabled = true;
                //BT_Update.Enabled = true;
                //BT_Delete.Enabled = true;
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                DeleteLink();
            }
        }
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

    protected void BT_UpdateClaim_Click(object sender, EventArgs e)
    {
        string strID = LB_SupplierInfoID.Text.Trim();

        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYBMYMCDBNWKJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            TB_Name.Focus();
            return;
        }
        if (TB_Code.Text.Trim().Contains("-") || TB_Code.Text.Trim().Contains(","))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMGZYWBNBHDJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            return;
        }

        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strID + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)//更新
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

            if (!TB_Code.Text.Trim().Equals(bMSupplierInfo.Code.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYBMBNGGJC").ToString().Trim() + "')", true);
                return;
            }
            //if (bMSupplierInfo.Status.Trim().Equals("Qualified"))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGYXXYBSHHGWXZGG").ToString().Trim()+"')", true);
            //    return;
            //}

            if (TB_Code.Text.Trim() == "" | TB_PhoneNum.Text.Trim() == "" | TB_Name.Text.Trim() == "" | TB_BusinessLicense.Text.Trim() == "" | DLC_SetUpTime.Text.Trim() == "" | TB_RegisteredCapital.Text.Trim() == "" | TB_BusinessLicense.Text.Trim() == "" | TB_RegisteredCapital.Text.Trim() == "" | TB_SupplyScope.Text.Trim() == "" | TB_Qualification.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoDaiHaoDeBuNengWeiKong").ToString().Trim()+"')", true);
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
            bMSupplierInfo.PassWord = TB_PassWord.Text.Trim();
            bMSupplierInfo.PhoneNum = TB_PhoneNum.Text.Trim();
            bMSupplierInfo.Qualification = TB_Qualification.Text.Trim();
            bMSupplierInfo.SupplyScope = TB_SupplyScope.Text.Trim();
            bMSupplierInfo.WebUrl = TB_WebUrl.Text.Trim();
            bMSupplierInfo.ZipCode = TB_ZipCode.Text.Trim();
            bMSupplierInfo.SubcontractProfessional = TB_SubcontractProfessional.Text.Trim();

            string strAccessoriesPath = UploadAttach1();
            if (strAccessoriesPath.Equals("0"))
            {
            }
            else if (strAccessoriesPath.Equals("1"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
                return;
            }
            else if (strAccessoriesPath.Equals("2"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
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


            try
            {
                bMSupplierInfoBLL.UpdateBMSupplierInfo(bMSupplierInfo, bMSupplierInfo.ID);

                LoadBMSupplierLink(bMSupplierInfo.Code.Trim());
                LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
        else//新增
        {
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
            bMSupplierInfo.PassWord = string.IsNullOrEmpty(TB_PassWord.Text.Trim()) || TB_PassWord.Text.Trim() == "" ? "12345678" : TB_PassWord.Text.Trim();
            bMSupplierInfo.PhoneNum = TB_PhoneNum.Text.Trim();
            bMSupplierInfo.Qualification = TB_Qualification.Text.Trim();
            bMSupplierInfo.Remark = "";
            bMSupplierInfo.Status = DL_Status.SelectedValue.Trim();
            bMSupplierInfo.SupplyScope = TB_SupplyScope.Text.Trim();
            bMSupplierInfo.WebUrl = TB_WebUrl.Text.Trim();
            bMSupplierInfo.ZipCode = TB_ZipCode.Text.Trim();
            bMSupplierInfo.ReviewDate = DateTime.Now;
            bMSupplierInfo.Point = 0;
            bMSupplierInfo.SubcontractProfessional = TB_SubcontractProfessional.Text.Trim();

            string strAccessoriesPath = UploadAttach1();
            if (strAccessoriesPath.Equals("0"))
            {
                bMSupplierInfo.AccessoriesPath = "";
            }
            else if (strAccessoriesPath.Equals("1"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
                return;
            }
            else if (strAccessoriesPath.Equals("2"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
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

            try
            {
                bMSupplierInfoBLL.AddBMSupplierInfo(bMSupplierInfo);
                LB_SupplierInfoID.Text = GetBMSupplierInfoID(bMSupplierInfo.Code.Trim()).ToString();

                DL_Status.SelectedValue = "New";

                LoadBMSupplierLink(bMSupplierInfo.Code.Trim());
                LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = " from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = '" + strID + "' ";

                BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
                lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);
                BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];

                lbl_CerID.Text = bMSupplierCertification.ID.ToString();
                TB_CertificateName.Text = bMSupplierCertification.CertificateName.Trim();
                TB_CertificateNum.Text = bMSupplierCertification.CertificateNum.Trim();
                TB_LicenseAgency.Text = bMSupplierCertification.LicenseAgency.Trim();
                DLC_ReleaseTime.Text = bMSupplierCertification.ReleaseTime.ToString("yyyy-MM-dd");

                //Button1.Enabled = true;
                //Button2.Enabled = true;
                //Button3.Enabled = true;
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCer','false') ", true);
            }


            if (e.CommandName == "Delete")
            {
                DeleteLink();

            }
        }
    }

    protected void BT_CreateLink_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);
    }

    protected void BT_NewLink_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text;

        if (strID == "")
        {
            AddLink();
        }
        else
        {
            UpdateLink();
        }
    }

    protected void AddLink()
    {
        if (string.IsNullOrEmpty(LB_SupplierInfoID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGYSXXXZLXRXXSB").ToString().Trim() + "')", true);
            return;
        }
        //if (DL_Status.SelectedValue.Trim().Equals("Qualified"))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSHDGRXXWFCZXZLXRXXSB").ToString().Trim()+"')", true);
        //    return;
        //}
        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        BMSupplierLink bMSupplierLink = new BMSupplierLink();
        bMSupplierLink.Email = TB_EmailLink.Text.Trim();
        bMSupplierLink.Gender = DL_Gender.SelectedValue.Trim();
        bMSupplierLink.MobileNum = TB_MobileNum.Text.Trim();
        bMSupplierLink.Name = TB_NameLink.Text.Trim();
        bMSupplierLink.OfficePhone = TB_OfficePhone.Text.Trim();
        bMSupplierLink.Position = TB_Position.Text.Trim();
        bMSupplierLink.SupplierCode = GetBMSupplierInfoCode(LB_SupplierInfoID.Text.Trim());
        bMSupplierLink.Code = "";

        try
        {
            bMSupplierLinkBLL.AddBMSupplierLink(bMSupplierLink);
            LB_ID.Text = GetBMSupplierLinkID(bMSupplierLink.SupplierCode.Trim()).ToString();

            UpdateBMSullierLink(LB_ID.Text.Trim());

            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            LoadBMSupplierLink(bMSupplierLink.SupplierCode.Trim());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSXZSBLXRZDBMGZWYBMBHSFZDGCJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);
        }
    }

    protected void UpdateBMSullierLink(string strID)
    {
        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        string strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.ID = '" + strID + "' ";
        IList lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[0];
            bMSupplierLink.Code = bMSupplierLink.SupplierCode.Trim() + "-" + strID;
            bMSupplierLinkBLL.UpdateBMSupplierLink(bMSupplierLink, bMSupplierLink.ID);
        }
    }
    /// <summary>
    /// 取得供应商联系人当前对应的ID号
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected int GetBMSupplierLinkID(string strCode)
    {
        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        string strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.SupplierCode = '" + strCode + "' Order by ID Desc ";
        IList lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);

        BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[0];

        return bMSupplierLink.ID;
    }

    protected void UpdateLink()
    {
        string strHQL;
        IList lst;
        //if (DL_Status.SelectedValue.Trim().Equals("Qualified"))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSHDGRXXWFCZXGLXRXXSB").ToString().Trim()+"')", true);
        //    return;
        //}

        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.ID = '" + LB_ID.Text.Trim() + "' ";
        lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
        BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[0];

        bMSupplierLink.Email = TB_EmailLink.Text.Trim();
        bMSupplierLink.Gender = DL_Gender.SelectedValue.Trim();
        bMSupplierLink.MobileNum = TB_MobileNum.Text.Trim();
        bMSupplierLink.Name = TB_NameLink.Text.Trim();
        bMSupplierLink.OfficePhone = TB_OfficePhone.Text.Trim();
        bMSupplierLink.Position = TB_Position.Text.Trim();

        try
        {
            bMSupplierLinkBLL.UpdateBMSupplierLink(bMSupplierLink, bMSupplierLink.ID);

            UpdateBMSullierLink(LB_ID.Text.Trim());

            LoadBMSupplierLink(bMSupplierLink.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);

        }
    }

    protected void DeleteLink()
    {
        string strHQL;
        //if (DL_Status.SelectedValue.Trim().Equals("Qualified"))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSHDGRXXWFCZSCLXRXXSB").ToString().Trim()+"')", true);
        //    return;
        //}
        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.ID = '" + LB_ID.Text.Trim() + "' ";
        IList lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
        BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[0];

        strHQL = "delete from T_BMSupplierLink where ID = '" + LB_ID.Text.Trim() + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete from T_ProjectMember where UserCode='" + bMSupplierLink.Code.Trim() + "' ";
            ShareClass.RunSqlCommand(strHQL);

            //BT_Update.Enabled = false;
            //BT_Delete.Enabled = false;

            LoadBMSupplierLink(bMSupplierLink.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
        }
    }

    protected void LoadBMSupplierLink(string strCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.SupplierCode = '" + strCode + "' Order by bMSupplierLink.ID Desc ";
        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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

    protected void BT_CreateCer_Click(object sender, EventArgs e)
    {
        lbl_CerID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCer','false') ", true);
    }

    protected void BT_NewCer_Click(object sender, EventArgs e)
    {
        string strID;

        strID = lbl_CerID.Text;

        if (strID == "")
        {
            AddCer();
        }
        else
        {
            UpdateCer();
        }
    }

    protected void AddCer()
    {
        if (string.IsNullOrEmpty(LB_SupplierInfoID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGYSXXXZZZZSXXSB").ToString().Trim() + "')", true);
            return;
        }
        //if (DL_Status.SelectedValue.Trim().Equals("Qualified"))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSHDGRXXWFCZXZZZZSXXSB").ToString().Trim()+"')", true);
        //    return;
        //}

        string strAttach = UploadAttach();

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
        {
            bMSupplierCertification.Attach = strAttach;
        }
        try
        {
            bMSupplierCertificationBLL.AddBMSupplierCertification(bMSupplierCertification);
            lbl_CerID.Text = GetBMSupplierCertificationID(bMSupplierCertification.SupplierCode.Trim()).ToString();
            //Button2.Enabled = true;
            //Button3.Enabled = true;
            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCer','false') ", true);

        }
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

    protected void UpdateCer()

    {
        string strHQL;
        IList lst;
        //if (DL_Status.SelectedValue.Trim().Equals("Qualified"))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSHDGRXXWFCZXGZZZSXXSB").ToString().Trim()+"')", true);
        //    return;
        //}

        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        strHQL = "from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = '" + lbl_CerID.Text.Trim() + "' ";
        lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);
        BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];

        string strAttach = UploadAttach();
        if (strAttach.Equals("0"))
        {
        }
        else if (strAttach.Equals("1"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "')", true);
            return;
        }
        else if (strAttach.Equals("2"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            return;
        }
        else
        {
            bMSupplierCertification.Attach = strAttach;
        }
        bMSupplierCertification.CertificateName = TB_CertificateName.Text.Trim();
        bMSupplierCertification.CertificateNum = TB_CertificateNum.Text.Trim();
        bMSupplierCertification.LicenseAgency = TB_LicenseAgency.Text.Trim();
        bMSupplierCertification.ReleaseTime = DateTime.Parse(DLC_ReleaseTime.Text.Trim());

        try
        {
            bMSupplierCertificationBLL.UpdateBMSupplierCertification(bMSupplierCertification, bMSupplierCertification.ID);

            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCer','false') ", true);

        }
    }

    protected void DeleteCer()
    {
        string strHQL;
        //if (DL_Status.SelectedValue.Trim().Equals("Qualified"))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYSHDGRXXWFCZSCZZZSXXSB").ToString().Trim()+"')", true);
        //    return;
        //}
        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        strHQL = "from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = '" + lbl_CerID.Text.Trim() + "' ";
        IList lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);
        BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];

        strHQL = "delete from T_BMSupplierCertification where ID = '" + lbl_CerID.Text.Trim() + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            //Button2.Enabled = false;
            //Button3.Enabled = false;

            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
        }
    }

    protected string UploadAttach()
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
    }

}