using ProjectMgt.BLL;
using ProjectMgt.Model;

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;

public partial class TTVendorInfoImport : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strDepartCode, strDepartName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(ShareClass.GetDepartCodeFromUserCode(strUserCode));

        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "查看所有供应商", strUserCode);

        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "from Vendor as vendor ";
            strHQL += " Where vendor.CreatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
            strHQL += " Order by vendor.CreateDate DESC";

            VendorBLL vendorBLL = new VendorBLL();
            lst = vendorBLL.GetAllVendors(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LoadIndustryType();
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strIndustryType, strVendorName, strVendorCode;

        string strDepartString = LB_DepartString.Text.Trim();

        strIndustryType = "%" + TB_IndustryTypeFind.Text.Trim() + "%";
        strVendorName = "%" + TB_CustName.Text.Trim() + "%";
        strVendorCode = "%" + TB_CustCode.Text.Trim() + "%";

        strHQL = "from Vendor as vendor where ";
        strHQL += " vendor.Type like  " + "'" + strIndustryType + "'";
        strHQL += " and vendor.VendorName like  " + "'" + strVendorName + "'";
        strHQL += " and vendor.VendorCode like " + "'" + strVendorCode + "'";
        strHQL += " and vendor.CreatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        strHQL += " Order by vendor.CreateDate DESC";
        VendorBLL vendorBLL = new VendorBLL();
        lst = vendorBLL.GetAllVendors(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DL_IndustryTypeFind_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType = DL_IndustryTypeFind.SelectedValue.Trim();

        TB_IndustryTypeFind.Text = strType;
    }

    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        if (ExcelToDBTest() == -1)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZDRSBEXECLBLDSJYCJC").ToString().Trim() ;
            return;
        }
        else
        {
            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim() ;
                return;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() ;
                return;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim() ;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim() ;
                }
                else
                {
                    VendorBLL vendorBLL = new VendorBLL();
                    Vendor vendor = new Vendor();

                    for (int i = 0; i < dr.Length; i++)
                    {
                        if (dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim() != "")
                        {
                            string strVendorCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();

                            try
                            {
                                vendor.VendorCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();
                                vendor.VendorName = dr[i][LanguageHandle.GetWord("MingChen").ToString().Trim()].ToString().Trim();
                                vendor.Type = dr[i][LanguageHandle.GetWord("HangYeLeiXing").ToString().Trim()].ToString().Trim();
                                vendor.ContactName = dr[i][LanguageHandle.GetWord("LianJiRen").ToString().Trim()].ToString().Trim();
                                vendor.Tel1 = dr[i][LanguageHandle.GetWord("DianHua").ToString().Trim()].ToString().Trim();
                                vendor.EmailAddress = dr[i]["EMail"].ToString().Trim();
                                vendor.RegistrationAddressCN = dr[i][LanguageHandle.GetWord("ZhongWenDeZhi").ToString().Trim()].ToString().Trim();
                                vendor.RegistrationAddressEN = dr[i][LanguageHandle.GetWord("YingWenDeZhi").ToString().Trim()].ToString().Trim();
                                vendor.Bank = dr[i][LanguageHandle.GetWord("JieSuanYinHang").ToString().Trim()].ToString().Trim();
                                vendor.BankAccount = dr[i][LanguageHandle.GetWord("YinHangZhangHao").ToString().Trim()].ToString().Trim();
                                vendor.Currency = dr[i][LanguageHandle.GetWord("JieSuanBiBie").ToString().Trim()].ToString().Trim();
                                try
                                {
                                    vendor.DeviceName = dr[i][LanguageHandle.GetWord("SheBeiMingChen").ToString().Trim()].ToString().Trim();
                                }
                                catch
                                {
                                    vendor.DeviceName = "";
                                }

                                vendor.CreateDate = DateTime.Now;
                                vendor.SalesPerson = strUserName;

                                vendor.CreatorCode = strUserCode;

                                vendor.VendorEnglishName = "";
                                vendor.VendorEnglishName = "";

                                vendor.SalesPerson = "";
                                vendor.InvoiceAddress = "";


                                vendor.Tel2 = "";
                                vendor.Fax = "";

                                vendor.WebSite = "";
                                vendor.ZP = "";
                                vendor.Country = "";
                                vendor.State = "";
                                vendor.City = "";

                                vendor.CreditRate = 1;
                                vendor.Discount = 0;
                                vendor.ClearingForm = "";

                                vendor.BelongDepartCode = strDepartCode;
                                vendor.BelongDepartName = strDepartName;

                                vendor.Comment = "";

                                vendorBLL.AddVendor(vendor);
                            }
                            catch (Exception err)
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strVendorCode + " : " + err.Message.ToString() + "<br/>"; ;

                                LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strVendorCode + " : " + err.Message.ToString());
                            }
                        }
                    }

                    LoadVendorList(strUserCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected int ExcelToDBTest()
    {
        string strHQL;

        string strVendorCode;

        int j = 0;

        IList lst;

        try
        {

            if (FileUpload_Training.HasFile == false)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
                j = -1;
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
            if (IsXls != ".xls" & IsXls != ".xlsx")
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim() ;
                j = -1;
            }
            string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
            string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
            string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
            FileInfo fi = new FileInfo(strDocSavePath + newfilename);
            if (fi.Exists)
            {
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
                j = -1;
            }
            else
            {
                FileUpload_Training.MoveTo(strDocSavePath + newfilename, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
                string strpath = strDocSavePath + newfilename;

                //DataSet ds = ExcelToDataSet(strpath, filename);
                //DataRow[] dr = ds.Tables[0].Select();
                //DataRow[] dr = ds.Tables[0].Select();//定义一个DataRow数组
                //int rowsnum = ds.Tables[0].Rows.Count;

                DataTable dt = MSExcelHandler.ReadExcelToDataTable(strpath, filename);
                DataRow[] dr = dt.Select();                        //定义一个DataRow数组
                int rowsnum = dt.Rows.Count;
                if (rowsnum == 0)
                {
                    j = -1;
                }
                else
                {
                    VendorBLL vendorBLL = new VendorBLL();
                    Vendor vendor = new Vendor();

                    for (int i = 0; i < dr.Length; i++)
                    {
                        strVendorCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();

                        if (strVendorCode != "")
                        {
                            strHQL = "From Vendor as vendor Where vendor.VendorCode = " + "'" + strVendorCode + "'";
                            lst = vendorBLL.GetAllVendors(strHQL);
                            if (lst != null && lst.Count > 0)//存在，则不操作
                            {
                                LB_ErrorText.Text += dr[i][LanguageHandle.GetWord("MingChen").ToString().Trim()].ToString().Trim() + LanguageHandle.GetWord("ZZYCZDRSBQJC").ToString().Trim();
                                j = -1;
                            }
                            else//新增
                            {
                                vendor.VendorCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();
                                vendor.VendorName = dr[i][LanguageHandle.GetWord("MingChen").ToString().Trim()].ToString().Trim();

                                if (CheckIndustryType(dr[i][LanguageHandle.GetWord("HangYeLeiXing").ToString().Trim()].ToString().Trim()))
                                {
                                    vendor.Type = dr[i][LanguageHandle.GetWord("HangYeLeiXing").ToString().Trim()].ToString().Trim();
                                }
                                else
                                {
                                    LB_ErrorText.Text += dr[i][LanguageHandle.GetWord("HangYeLeiXing").ToString().Trim()].ToString().Trim() + LanguageHandle.GetWord("HangYeLeiXingBuCunZaiQingZaiCa").ToString().Trim();
                                    j = -1;
                                }

                                if (CheckCurrencyType(dr[i][LanguageHandle.GetWord("JieSuanBiBie").ToString().Trim()].ToString().Trim()))
                                {
                                    vendor.Currency = dr[i][LanguageHandle.GetWord("JieSuanBiBie").ToString().Trim()].ToString().Trim();
                                }
                                else
                                {
                                    LB_ErrorText.Text += dr[i][LanguageHandle.GetWord("JieSuanBiBie").ToString().Trim()].ToString().Trim() + LanguageHandle.GetWord("ZZBBBCZQZCSSZMKSZ").ToString().Trim();
                                    j = -1;
                                }
                            }
                        }
                        
                    }
                }
            }
        }
        catch (Exception err)
        {
            LB_ErrorText.Text += err.Message.ToString() + "<br/>"; ;

            j = -1;
        }

        return j;
    }


    protected bool CheckIndustryType(string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "From IndustryType as industryType Where industryType.Type = '" + strType + "'";
        IndustryTypeBLL industryTypeBLL = new IndustryTypeBLL();
        lst = industryTypeBLL.GetAllIndustryTypes(strHQL);

        if (lst.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool CheckCurrencyType(string strCurrencyType)
    {
        string strHQL;
        IList lst;

        strHQL = "From CurrencyType as currencyType Where currencyType.Type = '" + strCurrencyType + "'";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        if (lst.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void LoadIndustryType()
    {
        string strHQL;
        IList lst;

        strHQL = "From IndustryType as industryType Order By industryType.SortNumber ASC";
        IndustryTypeBLL industryTypeBLL = new IndustryTypeBLL();
        lst = industryTypeBLL.GetAllIndustryTypes(strHQL);

        DL_IndustryTypeFind.DataSource = lst;
        DL_IndustryTypeFind.DataBind();
    }

    protected void LoadVendorList(string strUserCode)
    {
        string strHQL;
        IList lst;

        string strDepartString = LB_DepartString.Text;

        strHQL = "from Vendor as vendor ";
        strHQL += " Where (vendor.CreatorCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + "))";
        strHQL += " or (vendor.VendorCode in (Select vendorRelatedUser.VendorCode from VendorRelatedUser as vendorRelatedUser where vendorRelatedUser.UserCode = " + "'" + strUserCode + "'" + "))";
        strHQL += " Order by vendor.VendorCode DESC";

        VendorBLL vendorBLL = new VendorBLL();
        lst = vendorBLL.GetAllVendors(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


}
