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

public partial class TTBMSupplierInfoReview : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "承包商审查", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_EnterDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ReleaseTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            TB_EnterPer.Text = strUserCode;
            LB_ApplicantName.Text = strUserName;

            LoadBMSupplierInfo();
            LoadBMSupplierLink("");
            LoadBMSupplierCertification("");
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        string strID;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_SupplierID")).Text.Trim();
            string strCommandArgument = e.CommandArgument.ToString().Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = " + strID;
            BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
            lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

            TB_Code.Text = bMSupplierInfo.Code.Trim();
            TB_Name.Text = bMSupplierInfo.Name.Trim();
            LB_SupplierInfoID.Text = bMSupplierInfo.ID.ToString();

            LoadBMSupplierLink(bMSupplierInfo.Code.Trim());
            LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());

            if (e.CommandName == "Update")
            {
                if (strCommandArgument != "Archived")
                {
  
                    TB_EnterPer.Text = bMSupplierInfo.EnterPer.Trim();
                    LB_ApplicantName.Text = ShareClass.GetUserName(bMSupplierInfo.EnterPer.Trim());
                    DL_CompanyType.SelectedValue = bMSupplierInfo.CompanyType.Trim();
                    DLC_EnterDate.Text = bMSupplierInfo.EnterDate.ToString("yyyy-MM-dd");
                    DL_Status.SelectedValue = bMSupplierInfo.Status.Trim();
                    TB_Address.Text = bMSupplierInfo.Address.Trim();
                    TB_Bank.Text = bMSupplierInfo.Bank.Trim();
                    TB_BankNo.Text = bMSupplierInfo.BankNo.Trim();
                 
                    TB_CompanyFor.Text = bMSupplierInfo.CompanyFor.Trim();
                    TB_EinNo.Text = bMSupplierInfo.EinNo.Trim();
                    TB_Email.Text = bMSupplierInfo.Email.Trim();
                    TB_Fax.Text = bMSupplierInfo.Fax.Trim();
                  
                    TB_PassWord.Text = bMSupplierInfo.PassWord.Trim();
                    TB_PhoneNum.Text = bMSupplierInfo.PhoneNum.Trim();
                    TB_Qualification.Text = bMSupplierInfo.Qualification.Trim();
                    TB_SupplyScope.Text = bMSupplierInfo.SupplyScope.Trim();
                    TB_WebUrl.Text = bMSupplierInfo.WebUrl.Trim();
                    TB_ZipCode.Text = bMSupplierInfo.ZipCode.Trim();
                    TB_Remark.Text = bMSupplierInfo.Remark.Trim();
                    TB_Reviewer.Text = string.IsNullOrEmpty(bMSupplierInfo.Reviewer) ? ShareClass.GetUserName(strUserCode) : bMSupplierInfo.Reviewer.Trim();
                    DLC_ReviewDate.Text = string.IsNullOrEmpty(bMSupplierInfo.ReviewDate.ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : bMSupplierInfo.ReviewDate.ToString("yyyy-MM-dd");
                    NB_Point.Amount = bMSupplierInfo.Point;
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

                    LoadSupplierInfoAttach(bMSupplierInfo.ID.ToString());


                    if (bMSupplierInfo.Status.Trim() == "New")
                    {
                        HL_BMSupplierAnaly.Enabled = false;
                    }
                    else
                    {
                        HL_BMSupplierAnaly.Enabled = true;
                        HL_BMSupplierAnaly.NavigateUrl = "TTBMSupplierAnaly.aspx?SupplierID=" + bMSupplierInfo.ID.ToString();
                    }

                    TB_Code.Enabled = false;

                    //if (bMSupplierInfo.Status.Trim() == "Archived")
                    //{
                    //    TB_Code.Text = "";
                    //    TB_Code.Enabled = true;
                    //    LoadBMSupplierLink("");
                    //    LoadBMSupplierCertification("");
                    //}
                    //else
                    //{
                    //    TB_Code.Enabled = false;
                    //    LoadBMSupplierLink(bMSupplierInfo.Code.Trim());
                    //    LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());
                    //}

                    //BT_New.Enabled = true;
                    //BT_Update.Enabled = false;

                    BT_NewAA.Enabled = true;
                    BT_UpdateAA.Enabled = true;
                    BT_Archive.Enabled = true;

                    BT_DeleteAA.Enabled = true;

                    //Button1.Enabled = true;
                    //Button2.Enabled = false;
                }
                else
                {
                    strHQL = "from BMSupplierInfoHistory as bMSupplierInfoHistory where bMSupplierInfoHistory.ID = " + strID;
                    BMSupplierInfoHistoryBLL bMSupplierInfoHistoryBLL = new BMSupplierInfoHistoryBLL();
                    lst = bMSupplierInfoHistoryBLL.GetAllBMSupplierInfoHistorys(strHQL);
                    BMSupplierInfoHistory bMSupplierInfoHistory = (BMSupplierInfoHistory)lst[0];

                    LB_SupplierInfoID.Text = bMSupplierInfoHistory.ID.ToString();
                    TB_EnterPer.Text = bMSupplierInfoHistory.EnterPer.Trim();
                    LB_ApplicantName.Text = ShareClass.GetUserName(bMSupplierInfoHistory.EnterPer.Trim());
                    DL_CompanyType.SelectedValue = bMSupplierInfoHistory.CompanyType.Trim();
                    DLC_EnterDate.Text = bMSupplierInfoHistory.EnterDate.ToString("yyyy-MM-dd");
                    DL_Status.SelectedValue = bMSupplierInfoHistory.Status.Trim();
                    TB_Address.Text = bMSupplierInfoHistory.Address.Trim();
                    TB_Bank.Text = bMSupplierInfoHistory.Bank.Trim();
                    TB_BankNo.Text = bMSupplierInfoHistory.BankNo.Trim();
                    TB_Code.Text = bMSupplierInfoHistory.Code.Trim();
                    TB_CompanyFor.Text = bMSupplierInfoHistory.CompanyFor.Trim();
                    TB_EinNo.Text = bMSupplierInfoHistory.EinNo.Trim();
                    TB_Email.Text = bMSupplierInfoHistory.Email.Trim();
                    TB_Fax.Text = bMSupplierInfoHistory.Fax.Trim();
                    TB_Name.Text = bMSupplierInfoHistory.Name.Trim();
                    TB_PassWord.Text = bMSupplierInfoHistory.PassWord;
                    TB_PhoneNum.Text = bMSupplierInfoHistory.PhoneNum.Trim();
                    TB_Qualification.Text = bMSupplierInfoHistory.Qualification.Trim();
                    TB_SupplyScope.Text = bMSupplierInfoHistory.SupplyScope.Trim();
                    TB_WebUrl.Text = bMSupplierInfoHistory.WebUrl.Trim();
                    TB_ZipCode.Text = bMSupplierInfoHistory.ZipCode.Trim();
                    TB_Remark.Text = bMSupplierInfoHistory.Remark.Trim();
                    TB_Reviewer.Text = string.IsNullOrEmpty(bMSupplierInfoHistory.Reviewer) ? ShareClass.GetUserName(strUserCode) : bMSupplierInfoHistory.Reviewer.Trim();
                    DLC_ReviewDate.Text = string.IsNullOrEmpty(bMSupplierInfoHistory.ReviewDate.ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : bMSupplierInfoHistory.ReviewDate.ToString("yyyy-MM-dd");
                    NB_Point.Amount = bMSupplierInfoHistory.Point;
                    TB_SubcontractProfessional.Text = bMSupplierInfoHistory.SubcontractProfessional.Trim();

                    TB_BusinessLicense.Text = bMSupplierInfoHistory.BusinessLicense.Trim();
                    TB_EmployeesNum.Text = bMSupplierInfoHistory.EmployeesNum.ToString();
                    DL_IsLand.SelectedValue = bMSupplierInfoHistory.IsLand.Trim();
                    TB_ITNumber.Text = bMSupplierInfoHistory.ITNumber.ToString();
                    TB_LastFinalistsNumber.Text = bMSupplierInfoHistory.LastFinalistsNumber.Trim();
                    TB_LegalRepresentative.Text = bMSupplierInfoHistory.LegalRepresentative.Trim();
                    TB_LegalTel.Text = bMSupplierInfoHistory.LegalTel.Trim();
                    TB_MNumber.Text = bMSupplierInfoHistory.MNumber.ToString();
                    TB_PMNumber.Text = bMSupplierInfoHistory.PMNumber.ToString();
                    TB_PTNumber.Text = bMSupplierInfoHistory.PTNumber.ToString();
                    TB_QualificationCertificate.Text = bMSupplierInfoHistory.QualificationCertificate.Trim();
                    TB_RecommendedUnits.Text = bMSupplierInfoHistory.RecommendedUnits.Trim();
                    TB_RegisteredCapital.Text = bMSupplierInfoHistory.RegisteredCapital.ToString();
                    DLC_SetUpTime.Text = bMSupplierInfoHistory.SetUpTime.ToString("yyyy-MM-dd");
                    TB_STNumber.Text = bMSupplierInfoHistory.STNumber.ToString();
                    TB_TechnicalDirector.Text = bMSupplierInfoHistory.TechnicalDirector.Trim();
                    TB_TechnicalTel.Text = bMSupplierInfoHistory.TechnicalTel.Trim();
                    TB_TechnicalTitles.Text = bMSupplierInfoHistory.TechnicalTitles.Trim();
                    TB_TechnicalTitles_T.Text = bMSupplierInfoHistory.TechnicalTitles_T.Trim();

                    


                    LoadSupplierHistoryInfoAttach(bMSupplierInfoHistory.ID.ToString());

                    //BT_New.Enabled = true;
                    //BT_Update.Enabled = false;

                    BT_NewAA.Enabled = true;
                    BT_UpdateAA.Enabled = true;
                    BT_Archive.Enabled = true;

                    BT_DeleteAA.Enabled = true;

                    //Button1.Enabled = true;
                    //Button2.Enabled = false;


                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }


            if (e.CommandName == "Link")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);
            }
            if (e.CommandName == "Certification")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','false') ", true);
            }
        }
    }

    protected void BT_NewClaim_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMYMCDBNWKJC").ToString().Trim() + "')", true);
            return;
        }
        if (TB_Code.Text.Trim().Contains("-") || TB_Code.Text.Trim().Contains(","))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMGZYWBNBHDJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            return;
        }
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + LB_SupplierInfoID.Text.Trim() + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

        bMSupplierInfo.Address = TB_Address.Text.Trim();
        bMSupplierInfo.Bank = TB_Bank.Text.Trim();
        bMSupplierInfo.BankNo = TB_BankNo.Text.Trim();
        bMSupplierInfo.CompanyFor = TB_CompanyFor.Text.Trim();
        bMSupplierInfo.CompanyType = DL_CompanyType.SelectedValue.Trim();
        bMSupplierInfo.EinNo = TB_EinNo.Text.Trim();
        bMSupplierInfo.Email = TB_Email.Text.Trim();
        bMSupplierInfo.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        bMSupplierInfo.EnterPer = TB_EnterPer.Text.Trim();
        bMSupplierInfo.Fax = TB_Fax.Text.Trim();
        bMSupplierInfo.Name = TB_Name.Text.Trim();
        bMSupplierInfo.PhoneNum = TB_PhoneNum.Text.Trim();
        bMSupplierInfo.Qualification = TB_Qualification.Text.Trim();
        bMSupplierInfo.SupplyScope = TB_SupplyScope.Text.Trim();
        bMSupplierInfo.WebUrl = TB_WebUrl.Text.Trim();
        bMSupplierInfo.ZipCode = TB_ZipCode.Text.Trim();
        bMSupplierInfo.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());
        bMSupplierInfo.Reviewer = TB_Reviewer.Text.Trim();
        bMSupplierInfo.Remark = TB_Remark.Text.Trim();
        bMSupplierInfo.Point = NB_Point.Amount;
        bMSupplierInfo.SubcontractProfessional = TB_SubcontractProfessional.Text.Trim();


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

        if (bMSupplierInfo.Status.Trim() == "Qualified")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWGCBSYBSHHGWXZSJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            return;
        }
        if (bMSupplierInfo.Status.Trim() == "Archived")
        {
            if (IsBMSupplierInfo(TB_Code.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMYCZJC").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
                return;
            }

            bMSupplierInfo.Status = "Qualified";

            try
            {
                BMSupplierInfo bMSupplierInfo1 = new BMSupplierInfo();
                bMSupplierInfo1.AccessoriesPath = bMSupplierInfo.AccessoriesPath.Trim();
                bMSupplierInfo1.Address = bMSupplierInfo.Address.Trim();
                bMSupplierInfo1.Bank = bMSupplierInfo.Bank.Trim();
                bMSupplierInfo1.BankNo = bMSupplierInfo.BankNo.Trim();
                bMSupplierInfo1.BusinessLicense = bMSupplierInfo.BusinessLicense.Trim();
                bMSupplierInfo1.Code = TB_Code.Text.Trim();
                bMSupplierInfo1.CompanyFor = bMSupplierInfo.CompanyFor.Trim();
                bMSupplierInfo1.CompanyType = bMSupplierInfo.CompanyType.Trim();
                bMSupplierInfo1.EinNo = bMSupplierInfo.EinNo.Trim();
                bMSupplierInfo1.Email = bMSupplierInfo.Email.Trim();
                bMSupplierInfo1.EmployeesNum = bMSupplierInfo.EmployeesNum;
                bMSupplierInfo1.EnterDate = DateTime.Now;
                bMSupplierInfo1.EnterPer = strUserCode.Trim();
                bMSupplierInfo1.Fax = bMSupplierInfo.Fax.Trim();
                bMSupplierInfo1.IsLand = bMSupplierInfo.IsLand.Trim();
                bMSupplierInfo1.ITNumber = bMSupplierInfo.ITNumber;
                bMSupplierInfo1.LastFinalistsNumber = bMSupplierInfo.LastFinalistsNumber.Trim();
                bMSupplierInfo1.LegalRepresentative = bMSupplierInfo.LegalRepresentative.Trim();
                bMSupplierInfo1.LegalTel = bMSupplierInfo.LegalTel.Trim();
                bMSupplierInfo1.MNumber = bMSupplierInfo.MNumber;
                bMSupplierInfo1.Name = bMSupplierInfo.Name.Trim();
                bMSupplierInfo1.PassWord = "12345678";
                bMSupplierInfo1.PhoneNum = bMSupplierInfo.PhoneNum.Trim();
                bMSupplierInfo1.PMNumber = bMSupplierInfo.PMNumber;
                bMSupplierInfo1.Point = bMSupplierInfo.Point;
                bMSupplierInfo1.PTNumber = bMSupplierInfo.PTNumber;
                bMSupplierInfo1.Qualification = bMSupplierInfo.Qualification.Trim();
                bMSupplierInfo1.QualificationCertificate = bMSupplierInfo.QualificationCertificate.Trim();
                bMSupplierInfo1.RecommendedUnits = bMSupplierInfo.RecommendedUnits.Trim();
                bMSupplierInfo1.RegisteredCapital = bMSupplierInfo.RegisteredCapital;
                bMSupplierInfo1.Remark = "";
                bMSupplierInfo1.ReviewDate = DateTime.Now;
                bMSupplierInfo1.Reviewer = ShareClass.GetUserName(strUserCode.Trim());
                bMSupplierInfo1.SetUpTime = bMSupplierInfo.SetUpTime;
                bMSupplierInfo1.Status = bMSupplierInfo.Status.Trim();
                bMSupplierInfo1.STNumber = bMSupplierInfo.STNumber;
                bMSupplierInfo1.SubcontractProfessional = bMSupplierInfo.SubcontractProfessional.Trim();
                bMSupplierInfo1.SupplyScope = bMSupplierInfo.SupplyScope.Trim();
                bMSupplierInfo1.TechnicalDirector = bMSupplierInfo.TechnicalDirector.Trim();
                bMSupplierInfo1.TechnicalTel = bMSupplierInfo.TechnicalTel.Trim();
                bMSupplierInfo1.TechnicalTitles = bMSupplierInfo.TechnicalTitles.Trim();
                bMSupplierInfo1.TechnicalTitles_T = bMSupplierInfo.TechnicalTitles_T.Trim();
                bMSupplierInfo1.WebUrl = bMSupplierInfo.WebUrl.Trim();
                bMSupplierInfo1.ZipCode = bMSupplierInfo.ZipCode.Trim();

                bMSupplierInfoBLL.AddBMSupplierInfo(bMSupplierInfo1);


                //修改部门下面的人员能登录
                string strUpdateProjectMemberSQL = string.Format(@"update T_ProjectMember
                                set Status = 'Employed'
                                where UserCode like '{0}%'", bMSupplierInfo.Code);
                ShareClass.RunSqlCommand(strUpdateProjectMemberSQL);


                LB_SupplierInfoID.Text = GetBMSupplierInfoID(bMSupplierInfo1.Code.Trim()).ToString();

                LoadSupplierInfoAttach(LB_SupplierInfoID.Text.Trim());

                //BT_New.Enabled = true;
                //BT_Update.Enabled = false;

                //Button1.Enabled = true;
                //Button2.Enabled = false;

                BT_UpdateAA.Enabled = true;
                BT_Archive.Enabled = true;
                TB_Code.Enabled = false;

                LoadBMSupplierInfo();
                LoadBMSupplierLink(bMSupplierInfo1.Code.Trim());
                LoadBMSupplierCertification(bMSupplierInfo1.Code.Trim());

                DL_Status.SelectedValue = "Qualified";

                HL_BMSupplierAnaly.Enabled = true;
                HL_BMSupplierAnaly.NavigateUrl = "TTBMSupplierAnaly.aspx?SupplierID=" + LB_SupplierInfoID.Text.Trim();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHHGCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHHGSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }
        }
        else
        {
            if (!TB_Code.Text.Trim().Equals(bMSupplierInfo.Code.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMBNGGJC").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
                return;
            }

            bMSupplierInfo.Status = "Qualified";

            try
            {
                bMSupplierInfoBLL.UpdateBMSupplierInfo(bMSupplierInfo, bMSupplierInfo.ID);

                //修改部门下面的人员能登录
                string strUpdateProjectMemberSQL = string.Format(@"update T_ProjectMember
                                set Status = 'Employed'
                                where UserCode like '{0}%'", bMSupplierInfo.Code);
                ShareClass.RunSqlCommand(strUpdateProjectMemberSQL);


                LoadSupplierInfoAttach(bMSupplierInfo.ID.ToString());

                //BT_New.Enabled = true;
                //BT_Update.Enabled = false;

                //Button1.Enabled = true;
                //Button2.Enabled = false;

                BT_UpdateAA.Enabled = true;
                BT_Archive.Enabled = true;

                TB_Code.Enabled = false;

                LoadBMSupplierInfo();
                LoadBMSupplierLink(bMSupplierInfo.Code.Trim());
                LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());

                DL_Status.SelectedValue = "Qualified";

                HL_BMSupplierAnaly.Enabled = true;
                HL_BMSupplierAnaly.NavigateUrl = "TTBMSupplierAnaly.aspx?SupplierID=" + bMSupplierInfo.ID.ToString();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHHGCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSHHGSBJC").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }
        }
    }

    protected void BT_UpdateClaim_Click(object sender, EventArgs e)
    {
        string strID = LB_SupplierInfoID.Text.Trim();

        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMYMCDBNWKJC").ToString().Trim() + "')", true);
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
        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
        if (bMSupplierInfo.Status.Trim() == "Unqualified")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWGCBSYBBHWXZBHJC").ToString().Trim() + "')", true);
            return;
        }

        bMSupplierInfo.Address = TB_Address.Text.Trim();
        bMSupplierInfo.Bank = TB_Bank.Text.Trim();
        bMSupplierInfo.BankNo = TB_BankNo.Text.Trim();
        bMSupplierInfo.CompanyFor = TB_CompanyFor.Text.Trim();
        bMSupplierInfo.CompanyType = DL_CompanyType.SelectedValue.Trim();
        bMSupplierInfo.EinNo = TB_EinNo.Text.Trim();
        bMSupplierInfo.Email = TB_Email.Text.Trim();
        bMSupplierInfo.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        bMSupplierInfo.EnterPer = TB_EnterPer.Text.Trim();
        bMSupplierInfo.Fax = TB_Fax.Text.Trim();
        bMSupplierInfo.Name = TB_Name.Text.Trim();
        bMSupplierInfo.PhoneNum = TB_PhoneNum.Text.Trim();
        bMSupplierInfo.Qualification = TB_Qualification.Text.Trim();
        bMSupplierInfo.SupplyScope = TB_SupplyScope.Text.Trim();
        bMSupplierInfo.WebUrl = TB_WebUrl.Text.Trim();
        bMSupplierInfo.ZipCode = TB_ZipCode.Text.Trim();
        bMSupplierInfo.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());
        bMSupplierInfo.Reviewer = TB_Reviewer.Text.Trim();
        bMSupplierInfo.Remark = TB_Remark.Text.Trim();
        bMSupplierInfo.Point = NB_Point.Amount;
        bMSupplierInfo.SubcontractProfessional = TB_SubcontractProfessional.Text.Trim();

       
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

      
        if (!TB_Code.Text.Trim().Equals(bMSupplierInfo.Code.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMBNGGJC").ToString().Trim() + "')", true);
            return;
        }
        bMSupplierInfo.Status = "Unqualified";
        try
        {
            bMSupplierInfoBLL.UpdateBMSupplierInfo(bMSupplierInfo, bMSupplierInfo.ID);

            //修改部门下面的人员不能登录
            string strUpdateProjectMemberSQL = string.Format(@"update T_ProjectMember
                                set Status = 'Stop'
                                where UserCode like '{0}%'", bMSupplierInfo.Code);
            ShareClass.RunSqlCommand(strUpdateProjectMemberSQL);

            LoadSupplierInfoAttach(bMSupplierInfo.ID.ToString());

            LoadBMSupplierInfo();
            LoadBMSupplierLink(bMSupplierInfo.Code.Trim());
            LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());
            DL_Status.SelectedValue = "Unqualified";

            TB_Code.Enabled = false;
            HL_BMSupplierAnaly.Enabled = true;
            HL_BMSupplierAnaly.NavigateUrl = "TTBMSupplierAnaly.aspx?SupplierID=" + bMSupplierInfo.ID.ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBHCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBHSBJC").ToString().Trim() + "')", true);
        }
     
    }

    protected void BT_Archive_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Code.Text.Trim()) || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMYMCDBNWKJC").ToString().Trim() + "')", true);
            return;
        }
        if (TB_Code.Text.Trim().Contains("-") || TB_Code.Text.Trim().Contains(","))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMGZYWBNBHDJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            return;
        }
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + LB_SupplierInfoID.Text.Trim() + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
        if (bMSupplierInfo.Status.Trim() == "Archived")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWGCBSYBGDWXZGDJC").ToString().Trim() + "')", true);
            return;
        }
        if (!TB_Code.Text.Trim().Equals(bMSupplierInfo.Code.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYBMBNGGJC").ToString().Trim() + "')", true);
            return;
        }
        bMSupplierInfo.Address = TB_Address.Text.Trim();
        bMSupplierInfo.Bank = TB_Bank.Text.Trim();
        bMSupplierInfo.BankNo = TB_BankNo.Text.Trim();
        bMSupplierInfo.CompanyFor = TB_CompanyFor.Text.Trim();
        bMSupplierInfo.CompanyType = DL_CompanyType.SelectedValue.Trim();
        bMSupplierInfo.EinNo = TB_EinNo.Text.Trim();
        bMSupplierInfo.Email = TB_Email.Text.Trim();
        bMSupplierInfo.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        bMSupplierInfo.EnterPer = TB_EnterPer.Text.Trim();
        bMSupplierInfo.Fax = TB_Fax.Text.Trim();
        bMSupplierInfo.Name = TB_Name.Text.Trim();
        bMSupplierInfo.PhoneNum = TB_PhoneNum.Text.Trim();
        bMSupplierInfo.Qualification = TB_Qualification.Text.Trim();

        bMSupplierInfo.SupplyScope = TB_SupplyScope.Text.Trim();
        bMSupplierInfo.WebUrl = TB_WebUrl.Text.Trim();
        bMSupplierInfo.ZipCode = TB_ZipCode.Text.Trim();
        bMSupplierInfo.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());
        bMSupplierInfo.Reviewer = TB_Reviewer.Text.Trim();
        bMSupplierInfo.Remark = TB_Remark.Text.Trim();
        bMSupplierInfo.Point = NB_Point.Amount;
        bMSupplierInfo.SubcontractProfessional = TB_SubcontractProfessional.Text.Trim();

        string strAccessoriesPath = UploadAttachCertification();
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
            //bMSupplierInfo.Status = "Archived";
            bMSupplierInfoBLL.UpdateBMSupplierInfo(bMSupplierInfo, bMSupplierInfo.ID);

            //归档，产生一条新的数据，编码为+1
            //            string strNewSelectSupplierSQL = string.Format(@"select COUNT(1)+1 as RowNumber from T_BMSupplierInfo
            //                                where Code like '{0}%'", bMSupplierInfo.Code);
            //            DataTable dtNewSelectSupplier = ShareClass.GetDataSetFromSql(strNewSelectSupplierSQL, "NewSupplier").Tables[0];
            //            int intRowNumber = 0;
            //            int.TryParse(ShareClass.ObjectToString(dtNewSelectSupplier.Rows[0]["RowNumber"]), out intRowNumber);

            //bMSupplierInfo.Code = bMSupplierInfo.Code + "G" + intRowNumber.ToString();

            BMSupplierInfoHistoryBLL bMSupplierInfoHistoryBLL = new BMSupplierInfoHistoryBLL();

            BMSupplierInfoHistory bMSupplierInfoHistory = new BMSupplierInfoHistory();

            bMSupplierInfoHistory.Code = bMSupplierInfo.Code + "G" + DateTime.Now.ToString("yyyyMM");
            bMSupplierInfoHistory.Address = bMSupplierInfo.Address;
            bMSupplierInfoHistory.Bank = bMSupplierInfo.Bank;
            bMSupplierInfoHistory.BankNo = bMSupplierInfo.BankNo;
            bMSupplierInfoHistory.CompanyFor = bMSupplierInfo.CompanyFor;
            bMSupplierInfoHistory.CompanyType = bMSupplierInfo.CompanyType;
            bMSupplierInfoHistory.EinNo = bMSupplierInfo.EinNo;
            bMSupplierInfoHistory.Email = bMSupplierInfo.Email;
            bMSupplierInfoHistory.EnterDate = bMSupplierInfo.EnterDate;
            bMSupplierInfoHistory.EnterPer = bMSupplierInfo.EnterPer;
            bMSupplierInfoHistory.Fax = bMSupplierInfo.Fax;
            bMSupplierInfoHistory.Name = bMSupplierInfo.Name;
            bMSupplierInfoHistory.PhoneNum = bMSupplierInfo.PhoneNum;
            bMSupplierInfoHistory.Qualification = bMSupplierInfo.Qualification;
            bMSupplierInfoHistory.SupplyScope = bMSupplierInfo.SupplyScope;
            bMSupplierInfoHistory.WebUrl = bMSupplierInfo.WebUrl;
            bMSupplierInfoHistory.ZipCode = bMSupplierInfo.ZipCode;
            bMSupplierInfoHistory.ReviewDate = bMSupplierInfo.ReviewDate;
            bMSupplierInfoHistory.Reviewer = bMSupplierInfo.Reviewer;
            bMSupplierInfoHistory.Remark = bMSupplierInfo.Remark;
            bMSupplierInfoHistory.Point = bMSupplierInfo.Point;
            bMSupplierInfoHistory.SubcontractProfessional = bMSupplierInfo.SubcontractProfessional;
            bMSupplierInfoHistory.AccessoriesPath = bMSupplierInfo.AccessoriesPath;
            bMSupplierInfoHistory.BusinessLicense = bMSupplierInfo.BusinessLicense;
            bMSupplierInfoHistory.EmployeesNum = bMSupplierInfo.EmployeesNum;
            bMSupplierInfoHistory.IsLand = bMSupplierInfo.IsLand;
            bMSupplierInfoHistory.ITNumber = bMSupplierInfo.ITNumber;
            bMSupplierInfoHistory.LastFinalistsNumber = bMSupplierInfo.LastFinalistsNumber;
            bMSupplierInfoHistory.LegalRepresentative = bMSupplierInfo.LegalRepresentative;
            bMSupplierInfoHistory.LegalTel = bMSupplierInfo.LegalTel;
            bMSupplierInfoHistory.MNumber = bMSupplierInfo.MNumber;
            bMSupplierInfoHistory.PMNumber = bMSupplierInfo.PMNumber;
            bMSupplierInfoHistory.PTNumber = bMSupplierInfo.PTNumber;
            bMSupplierInfoHistory.QualificationCertificate = bMSupplierInfo.QualificationCertificate;
            bMSupplierInfoHistory.RecommendedUnits = bMSupplierInfo.RecommendedUnits;
            bMSupplierInfoHistory.RegisteredCapital = bMSupplierInfo.RegisteredCapital;
            bMSupplierInfoHistory.SetUpTime = bMSupplierInfo.SetUpTime;
            bMSupplierInfoHistory.STNumber = bMSupplierInfo.STNumber;
            bMSupplierInfoHistory.TechnicalDirector = bMSupplierInfo.TechnicalDirector;
            bMSupplierInfoHistory.TechnicalTel = bMSupplierInfo.TechnicalTel;
            bMSupplierInfoHistory.TechnicalTitles = bMSupplierInfo.TechnicalTitles;
            bMSupplierInfoHistory.TechnicalTitles_T = bMSupplierInfo.TechnicalTitles_T;
            bMSupplierInfoHistory.Status = "Archived";

            //新增一条归档数据
            bMSupplierInfoHistoryBLL.AddBMSupplierInfoHistory(bMSupplierInfoHistory);

            LoadSupplierInfoAttach(bMSupplierInfo.ID.ToString());

            //BT_New.Enabled = true;
            //BT_Update.Enabled = false;

            //Button1.Enabled = true;
            //Button2.Enabled = false;

            BT_UpdateAA.Enabled = true;
            BT_NewAA.Enabled = true;

            LoadBMSupplierInfo();
            LoadBMSupplierLink(bMSupplierInfo.Code.Trim());
            LoadBMSupplierCertification(bMSupplierInfo.Code.Trim());

            DL_Status.SelectedValue = "Archived";

            HL_BMSupplierAnaly.Enabled = true;
            HL_BMSupplierAnaly.NavigateUrl = "TTBMSupplierAnaly.aspx?SupplierID=" + bMSupplierInfo.ID.ToString();

            TB_Code.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGDSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void BT_DeleteAA_Click(object sender, EventArgs e)
    {
        try
        {
            string strID = LB_SupplierInfoID.Text.Trim();

            if (string.IsNullOrEmpty(strID))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYSCDCBSLB").ToString().Trim() + "')", true);
                return;
            }


            BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
            string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strID + "' ";
            IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
            if (lst != null && lst.Count > 0)
            {
                BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];

                bMSupplierInfoBLL.DeleteBMSupplierInfo(bMSupplierInfo);


                string strDeleteBMSupplierLinkSQL = "delete T_BMSupplierLink where SupplierCode = '" + bMSupplierInfo.Code + "'";
                ShareClass.RunSqlCommand(strDeleteBMSupplierLinkSQL);


                LoadBMSupplierInfo();

                LoadBMSupplierLink("");

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }



    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
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

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','true','popwindowLinkDetail') ", true);
                }
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','true','popwindowLinkDetail') ", true);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCBSXXXZLXRXXSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','true','popwindowLinkDetail') ", true);
            return;
        }

        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        BMSupplierLink bMSupplierLink = new BMSupplierLink();
        bMSupplierLink.Email = TB_EmailLink.Text.Trim();
        bMSupplierLink.Gender = DL_Gender.SelectedValue.Trim();
        bMSupplierLink.MobileNum = TB_MobileNum.Text.Trim();
        bMSupplierLink.Name = TB_NameLink.Text.Trim();
        bMSupplierLink.OfficePhone = TB_OfficePhone.Text.Trim();
        bMSupplierLink.Position = TB_Position.Text.Trim();
        bMSupplierLink.SupplierCode = GetBMSupplierInfoCode(LB_SupplierInfoID.Text.Trim());

        try
        {
            bMSupplierLinkBLL.AddBMSupplierLink(bMSupplierLink);
            LB_ID.Text = GetBMSupplierLinkID(bMSupplierLink.SupplierCode.Trim()).ToString();

            UpdateBMSullierLink(LB_ID.Text.Trim());

            //BT_Update.Enabled = true;

            LoadBMSupplierLink(bMSupplierLink.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLXRXZCG").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZLXRXZSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','true','popwindowLinkDetail') ", true);
        }
   
    }


    protected void UpdateLink()
    {
        string strHQL;
        IList lst;

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
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','true','popwindowLinkDetail') ", true);
        }
    }


    protected void DeleteLink()
    {
        string strHQL;
        IList lst;

        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.ID = '" + LB_ID.Text.Trim() + "' ";
        lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);

        if (lst != null && lst.Count > 0)
        {
            BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[0];

            try
            {
                bMSupplierLinkBLL.DeleteBMSupplierLink(bMSupplierLink);

                LoadBMSupplierLink(bMSupplierLink.SupplierCode.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYSCDLXRLB").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLink','false') ", true);
    }


    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
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

                        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','true','popwindowCertificationDetail') ", true);
                    }

                    if (e.CommandName == "Delete")
                    {
                        DeleteCertification();

                    }
                }
            }
        }
    }


    protected void BT_CreateCertification_Click(object sender, EventArgs e)
    {
        lbl_CerID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','true','popwindowCertificationDetail') ", true);
    }

    protected void BT_NewCertification_Click(object sender, EventArgs e)
    {
        string strID;

        strID = lbl_CerID.Text;

        if (strID == "")
        {
            AddCertification();
        }
        else
        {
            UpdateCertification();
        }
    }

    protected void AddCertification()
    {
        if (string.IsNullOrEmpty(LB_SupplierInfoID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCBSXXXZLXRXXSB").ToString().Trim() + "')", true);
            return;
        }

   
        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        BMSupplierCertification bMSupplierCertification = new BMSupplierCertification();
        bMSupplierCertification.CertificateName = TB_CertificateName.Text.Trim();
        bMSupplierCertification.CertificateNum = TB_CertificateNum.Text.Trim();
        bMSupplierCertification.LicenseAgency = TB_LicenseAgency.Text.Trim();
        bMSupplierCertification.ReleaseTime = DateTime.Parse(DLC_ReleaseTime.Text.Trim());
        bMSupplierCertification.SupplierCode = GetBMSupplierInfoCode(LB_SupplierInfoID.Text.Trim());
        bMSupplierCertification.Attach = HL_Certification.NavigateUrl;
        try
        {
            bMSupplierCertificationBLL.AddBMSupplierCertification(bMSupplierCertification);
            lbl_CerID.Text = GetBMSupplierCertificationID(bMSupplierCertification.SupplierCode.Trim()).ToString();
            //Button2.Enabled = true;
            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZSXZCG").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','false') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZSXZSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','true','popwindowCertificationDetail') ", true);
        }
    }


    protected void UpdateCertification()
    {
        string strHQL;
        IList lst;

        BMSupplierCertificationBLL bMSupplierCertificationBLL = new BMSupplierCertificationBLL();
        strHQL = "from BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = '" + lbl_CerID.Text.Trim() + "' ";
        lst = bMSupplierCertificationBLL.GetAllBMSupplierCertifications(strHQL);
        BMSupplierCertification bMSupplierCertification = (BMSupplierCertification)lst[0];


        bMSupplierCertification.Attach = HL_Certification.NavigateUrl;

        bMSupplierCertification.CertificateName = TB_CertificateName.Text.Trim();
        bMSupplierCertification.CertificateNum = TB_CertificateNum.Text.Trim();
        bMSupplierCertification.LicenseAgency = TB_LicenseAgency.Text.Trim();
        bMSupplierCertification.ReleaseTime = DateTime.Parse(DLC_ReleaseTime.Text.Trim());

        try
        {
            bMSupplierCertificationBLL.UpdateBMSupplierCertification(bMSupplierCertification, bMSupplierCertification.ID);

            LoadBMSupplierCertification(bMSupplierCertification.SupplierCode.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','false') ", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSB").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','true','popwindowCertificationDetail') ", true);
        }
    }

    protected void DeleteCertification()
    {
        string strHQL;

        strHQL = "Delete from T_BMSupplierCertification as bMSupplierCertification where bMSupplierCertification.ID = " + lbl_CerID.Text.Trim();
        ShareClass.RunSqlCommand(strHQL);

        LoadBMSupplierCertification(TB_Code.Text.Trim());

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','false') ", true);
    }

    protected void BT_UploadCertification_Click(object sender, EventArgs e)
    {
        try
        {
            HL_Certification.NavigateUrl= UploadAttachCertification();
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowCertification','true','popwindowCertificationDetail') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
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


    protected void LoadSupplierHistoryInfoAttach(string strId)
    {
        //绑定附件列表
        string strHQL = "from BMSupplierInfoHistory as bMSupplierInfoHistory where bMSupplierInfoHistory.ID = '" + strId + "' ";
        BMSupplierInfoHistoryBLL bMSupplierInfoHistoryBLL = new BMSupplierInfoHistoryBLL();
        IList lst = bMSupplierInfoHistoryBLL.GetAllBMSupplierInfoHistorys(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierInfoHistory bMSupplierInfoHistory = (BMSupplierInfoHistory)lst[0];
            if (!string.IsNullOrEmpty(bMSupplierInfoHistory.AccessoriesPath) && bMSupplierInfoHistory.AccessoriesPath.Trim() != "")
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

    protected void LoadBMSupplierInfo()
    {
        string strHQL;
        strHQL = "Select *,(case when Point>=90 then '优秀' when Point>=80 and Point<90 then '良好' when Point>=60 and Point<80 then 'Qualified' else 'Unqualified' end) EvalueDegree From T_BMSupplierInfo Where 1=1 "; 
        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and (Code like '%" + txt_SupplierInfo.Text.Trim() + "%' or Name like '%" + txt_SupplierInfo.Text.Trim() + "%' or CompanyFor like '%" + txt_SupplierInfo.Text.Trim() + "%' or Status like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
                "or SupplyScope like '%" + txt_SupplierInfo.Text.Trim() + "%' or Qualification like '%" + txt_SupplierInfo.Text.Trim() + "%' or Remark like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-ReviewDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-ReviewDate::date>=0 ";
        }
        string strStatus = DropDownList1.SelectedValue.Trim();
        if (strStatus != "0")
        {
            strHQL += " and Status = '" + DropDownList1.SelectedValue.Trim() + "' ";
        }

        if (strStatus == "Archived")
        {
            strHQL += " union all select *,'Archived' as EvalueDegree from T_BMSupplierInfoHistory";
        }
        else
        {
            strHQL += " Order by ID DESC";
        }



        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
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


    protected string UploadAttachCertification()
    {
        //上传附件
        if (AttachFile.HasFile)
        {
            string strFileName1, strExtendName;

            string strUserCode = Session["UserCode"].ToString();

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


    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMSupplierInfo();
    }

  

    /// <summary>
    /// 判断编码在后台用户及供应商信息中是否存在
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected bool IsBMSupplierInfo(string strCode)
    {
        bool flag1 = true;
        bool flag2 = true;
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        string strHQL = "from BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.Code = '" + strCode + "' ";
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag1 = true;
        }
        else
            flag1 = false;
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = '" + strCode + "' ";
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag2 = true;
        }
        else
            flag2 = false;

        if (flag1 || flag2)
            return true;
        else
            return false;
    }

    protected void UpdateBMSullierLink(string strID)
    {
        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
        string strHQL = "from BMSupplierLink as bMSupplierLink where bMSupplierLink.ID = '" + strID + "' ";
        IList lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[0];
            //bMSupplierLink.Code = bMSupplierLink.SupplierCode.Trim() + "-" + strID;
            bMSupplierLink.Code = bMSupplierLink.SupplierCode.Trim() + strID;
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
