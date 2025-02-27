using System;
using System.Resources;
using System.IO;
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
using System.Data.OleDb;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTCustomerRelatedUserImport : System.Web.UI.Page
{
    string strUserCode, strUserName;
    string strDepartCode, strDepartName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(ShareClass.GetDepartCodeFromUserCode(strUserCode));

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "客户可视用户导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {

        }
    }


    protected void btn_ExcelToDataTraining_Click(object sender, EventArgs e)
    {
        if (FileUpload_Training.HasFile == false)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGNZEXCELWJ").ToString().Trim();
            return;
        }
        string IsXls = System.IO.Path.GetExtension(FileUpload_Training.FileName).ToString().ToLower();
        if (IsXls != ".xls" & IsXls != ".xlsx")
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
            return;
        }
        string filename = FileUpload_Training.FileName.ToString();  //获取Execle文件名
        string newfilename = System.IO.Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmssff") + IsXls;//新文件名称，带后缀
        string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode.Trim() + "\\Doc\\";
        FileInfo fi = new FileInfo(strDocSavePath + newfilename);
        if (fi.Exists)
        {
            LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRSB").ToString().Trim();
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
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
            }
            else
            {
                CustomerRelatedUserBLL customerRelatedUserBLL = new CustomerRelatedUserBLL();
                CustomerRelatedUser customerRelatedUser = new CustomerRelatedUser();

                for (int i = 0; i < dr.Length; i++)
                {
                    if (dr[i][LanguageHandle.GetWord("KeHuDaiMa").ToString().Trim()].ToString().Trim() != "")
                    {
                        string strCustomerCode = dr[i][LanguageHandle.GetWord("KeHuDaiMa").ToString().Trim()].ToString().Trim();
                        string strRelatedUserNameList = dr[i][LanguageHandle.GetWord("KeShiRenYuan").ToString().Trim()].ToString().Trim();
                        string[] strRelatedUserArray = strRelatedUserNameList.Split(',');

                        if (CheckCustomerIsExisted(strCustomerCode) > 0)
                        {
                            try
                            {
                                if (CB_IsClearAll.Checked == true)
                                {
                                    //删除客户的可视用户
                                    DeleteAllRelatedCustomerUser(strCustomerCode);
                                }

                                for (int j = 0; j < strRelatedUserArray.Length; j++)
                                {
                                    string strRelatedUserName = strRelatedUserArray[j];
                                    string strRelatedUserCode = ShareClass.GetUserCodeByUserName(strRelatedUserArray[j]);

                                    if (strRelatedUserCode != "" & CheckCustomerRelatedUserIsExisted(strCustomerCode, strRelatedUserCode) == 0)
                                    {
                                        customerRelatedUser.CustomerCode = strCustomerCode;
                                        customerRelatedUser.UserCode = strRelatedUserCode;
                                        customerRelatedUser.UserName = strRelatedUserName; ;

                                        customerRelatedUserBLL.AddCustomerRelatedUser(customerRelatedUser);
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strCustomerCode + " : " + err.Message.ToString() + "<br/>";

                                LogClass.WriteLogFile(this.GetType().BaseType.Name + "：" + LanguageHandle.GetWord("ZZJGDRSBJC").ToString().Trim() + " : " + LanguageHandle.GetWord("HangHao").ToString().Trim() + ": " + (i + 2).ToString() + " , " + LanguageHandle.GetWord("DaiMa").ToString().Trim() + ": " + strCustomerCode + " : " + err.Message.ToString());
                            }
                        }
                    }
                }

                LB_ErrorText.Text += LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click41", "alert('" + LanguageHandle.GetWord("ZZEXCLEBDRCG").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click21", "displayWaitingImage('none');", true);
            }
        }
    }

    //删除客户的可视用户
    protected void DeleteAllRelatedCustomerUser(string strCustomerCode)
    {
        string strHQL;

        strHQL = "Delete From T_CustomerRelatedUser Where CustomerCode = '" + strCustomerCode + "'";
        ShareClass.RunSqlCommand(strHQL);
    }

    //取得客户相关可视人员的数量
    protected int CheckCustomerRelatedUserIsExisted(string strCustomerCode, string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_CustomerRelatedUser Where CustomerCode = '" + strCustomerCode + "' and UserCode = '" + strUserCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerRelatedUser");
        return ds.Tables[0].Rows.Count;
    }

    //取得客户的数量
    protected int CheckCustomerIsExisted(string strCustomerCode)
    {
        string strHQL;

        strHQL = "Select * From T_Customer Where CustomerCode = '" + strCustomerCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Customer");
        return ds.Tables[0].Rows.Count;
    }

    protected int ExcelToDBTest()
    {
        string strHQL;

        string strCustomerCode;

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
                LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGZKYZEXCELWJ").ToString().Trim();
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
                    LB_ErrorText.Text += LanguageHandle.GetWord("ZZJGEXCELBWKBWSJ").ToString().Trim();
                    j = -1;
                }
                else
                {
                    CustomerBLL customerBLL = new CustomerBLL();
                    Customer customer = new Customer();

                    for (int i = 0; i < dr.Length; i++)
                    {
                        strCustomerCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();

                        if (strCustomerCode != "")
                        {
                            strHQL = "From Customer as customer Where customer.CustomerCode = " + "'" + strCustomerCode + "'";
                            lst = customerBLL.GetAllCustomers(strHQL);
                            if (lst != null && lst.Count > 0)//存在，则不操作
                            {
                                LB_ErrorText.Text += dr[i][LanguageHandle.GetWord("MingChen").ToString().Trim()].ToString().Trim() + LanguageHandle.GetWord("ZZYCZDRSBQJC").ToString().Trim();
                                j = -1;
                            }
                            else//新增
                            {
                                customer.CustomerCode = dr[i][LanguageHandle.GetWord("DaiMa").ToString().Trim()].ToString().Trim();
                                customer.CustomerName = dr[i][LanguageHandle.GetWord("MingChen").ToString().Trim()].ToString().Trim();
                            }

                            continue;
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
}
