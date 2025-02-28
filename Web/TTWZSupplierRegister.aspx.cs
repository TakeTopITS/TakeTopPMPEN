using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZSupplierRegister : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataTemplateFileBinder();
        }
    }



    private void DataTemplateFileBinder()
    {
        //string strSupplierTemplateHQL = @"select * from T_WZSupplierTemplateFile";
        //DataTable dtSupplierTemplate = ShareClass.GetDataSetFromSql(strSupplierTemplateHQL, "SupplierTemplate").Tables[0];

        //DG_SupplierTemplateFile.DataSource = dtSupplierTemplate;
        //DG_SupplierTemplateFile.DataBind();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WZSupplierRegisterBLL wZSupplierRegisterBLL = new WZSupplierRegisterBLL();

            string strSupplierName = TXT_SupplierName.Text.Trim();
            string strOpeningBank = TXT_OpeningBank.Text.Trim();
            string strAccountNumber = TXT_AccountNumber.Text.Trim();
            string strRateNumber = TXT_RateNumber.Text.Trim();
            string strUnitAddress = TXT_UnitAddress.Text.Trim();
            string strZipCode = TXT_ZipCode.Text.Trim();
            string strUnitPhone = TXT_UnitPhone.Text.Trim();
            string strPersonDelegate = TXT_PersonDelegate.Text.Trim();
            string strDelegateAgent = TXT_DelegateAgent.Text.Trim();
            string strContactPhone = TXT_ContactPhone.Text.Trim();
            string strMobile = TXT_Mobile.Text.Trim();
            string strQQ = TXT_QQ.Text.Trim();
            string strEmail = TXT_Email.Text.Trim();
            string strInDocument = HF_InDocument.Value;                                          //上传的附件路径
            string strInDocumentURL = HF_InDocumentURL.Value;                                          //上传的附件路径
            string strPushPerson = TXT_PushPerson.Text;

            //验证是否非法字符
            if (string.IsNullOrEmpty(strSupplierName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGYSMCZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strSupplierName.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGYSMCCDBCG30GZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSupplierName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGYSMCBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strOpeningBank))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKHXZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strOpeningBank.Length > 26)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKHXCDBNCG26GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOpeningBank))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKHXBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPersonDelegate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFRDBZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strPersonDelegate.Length > 20)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFRDBCDBNCG4GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPersonDelegate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFRDBBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strAccountNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZHZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strAccountNumber.Length > 40)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZHCDBNCG40GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAccountNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strUnitPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWDHZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strUnitPhone.Length > 40)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWDHCDBNCG14GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strUnitPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWDHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strRateNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strRateNumber.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHCDBNCG30GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRateNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strZipCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYBZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strZipCode.Length > 6)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYBCDBNCG6GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strZipCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYBBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strUnitAddress))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWDZZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strUnitAddress.Length > 26)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWDZCDBNCG26GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strUnitAddress))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWDZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strDelegateAgent))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWTDLRZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strDelegateAgent.Length > 4)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWTDLRCDBNCG4GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDelegateAgent))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWTDLRBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strContactPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLXDHZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strContactPhone.Length > 14)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLXDHCDBNCG14GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strContactPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZLXDHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strMobile))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSJZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strMobile.Length > 11)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSJCDBNCG11GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMobile))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSJBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strEmail))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEMAILZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strEmail.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEMAILCDBNCG30GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strEmail))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZEMAILBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strQQ))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZQQZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strQQ.Length > 12)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZQQCDBNCG12GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strQQ))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZQQBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPushPerson))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJRZDKTX").ToString().Trim()+"')", true);
                return;
            }

            //增加
            WZSupplierRegister wZSupplierRegister = new WZSupplierRegister();
            string strSupplierNumber = CreateNewSupplierNumber();
            wZSupplierRegister.SupplierNumber = strSupplierNumber;
            wZSupplierRegister.SupplierName = strSupplierName;
            wZSupplierRegister.OpeningBank = strOpeningBank;
            wZSupplierRegister.AccountNumber = strAccountNumber;
            wZSupplierRegister.RateNumber = strRateNumber;
            wZSupplierRegister.UnitAddress = strUnitAddress;
            wZSupplierRegister.ZipCode = strZipCode;
            wZSupplierRegister.UnitPhone = strUnitPhone;
            wZSupplierRegister.PersonDelegate = strPersonDelegate;
            wZSupplierRegister.DelegateAgent = strDelegateAgent;
            wZSupplierRegister.ContactPhone = strContactPhone;
            wZSupplierRegister.Mobile = strMobile;
            wZSupplierRegister.QQ = strQQ;
            wZSupplierRegister.Email = strEmail;
            wZSupplierRegister.InDocument = strInDocument;
            wZSupplierRegister.InDocumentURL = strInDocumentURL;
            wZSupplierRegister.PushPerson = strPushPerson;

            wZSupplierRegister.InTime = DateTime.Now;
            wZSupplierRegister.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

            wZSupplierRegisterBLL.AddWZSupplierRegister(wZSupplierRegister);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('注册成功，等候后台审核！');", true);   //ChineseWord
        }
        catch (Exception ex)
        { }
    }



    protected void BT_InDocument_Click(object sender, EventArgs e)
    {
        try
        {
            string strPurchaseOfferDocument = FUP_InDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
            if (!string.IsNullOrEmpty(strPurchaseOfferDocument))
            {
                string strExtendName = System.IO.Path.GetExtension(strPurchaseOfferDocument);//获取扩展名

                DateTime dtUploadNow = DateTime.Now; //获取系统时间
                string strFileName2 = System.IO.Path.GetFileName(strPurchaseOfferDocument);
                string strExtName = Path.GetExtension(strFileName2);

                string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                if (fi.Exists)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"');</script>");
                }

                if (Directory.Exists(strDocSavePath) == false)
                {
                    //如果不存在就创建file文件夹{
                    Directory.CreateDirectory(strDocSavePath);
                }

                FUP_InDocument.SaveAs(strDocSavePath + strFileName3);

                LT_InDocument.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                HF_InDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                HF_InDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                //重新加载报价文件列表
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCRWWJCG").ToString().Trim()+"')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim()+"')", true);
                return;
            }
        }
        catch (Exception ex) { }
    }


    /// <summary>
    ///  生成供应商编号
    /// </summary>
    private string CreateNewSupplierNumber()
    {
        string strNewSupplierNumber = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                int intSupplierNumber = 1;
                do
                {
                    StringBuilder sbSupplierNumber = new StringBuilder();
                    for (int j = 4 - intSupplierNumber.ToString().Length; j > 0; j--)
                    {
                        sbSupplierNumber.Append("0");
                    }
                    strNewSupplierNumber = sbSupplierNumber.ToString() + "" + intSupplierNumber.ToString();

                    //验证新的供应商编号是否存在
                    string strCheckNewSupplierNumberHQL = "select count(1) as RowNumber from T_WZSupplier where SupplierNumber = '" + strNewSupplierNumber + "'";
                    DataTable dtCheckNewSupplierNumber = ShareClass.GetDataSetFromSql(strCheckNewSupplierNumberHQL, "SupplierNumber").Tables[0];
                    int intCheckNewSupplierNumber = int.Parse(dtCheckNewSupplierNumber.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewSupplierNumber == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intSupplierNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewSupplierNumber;
    }
}
