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

public partial class TTWZSupplierEdit : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString().Trim() : "";
        strUserName = Session["UserName"] != null ? Session["UserName"].ToString().Trim() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "", true); if (!IsPostBack)
        {
            HF_SupplierCode.Value = strUserCode;
            TXT_SupplierName.Text = strUserName;

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindSupplierData(intID);
            }
            else
            {
                TXT_SupplierNumber.Text = CreateNewSupplierNumber();
                TXT_InTime.Text = DateTime.Now.ToString();

                BindSupplierData(strUserCode);
            }


            SetControlState();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();

            string strSupplierCode = HF_SupplierCode.Value;
            string strSupplierNumber = TXT_SupplierNumber.Text.Trim();
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
            string strMainSupplier = TXT_MainSupplier.Text.Trim();
            string strInDocument = HF_InDocument.Value;                                          //上传的附件路径
            string strInDocumentURL = HF_InDocumentURL.Value;                                          //上传的附件路径
            string strPushUnit = TXT_PushUnit.Text.Trim();
            string strPushPerson = HF_PushPerson.Value;

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
            if (string.IsNullOrEmpty(strPushUnit))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJDWZDKTX").ToString().Trim()+"')", true);
                return;
            }
            if (strPushUnit.Length > 22)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJDWCDBNCG22GZFC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPushUnit))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJDWBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPushPerson))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJRZDKTX").ToString().Trim()+"')", true);
                return;
            }

            if (string.IsNullOrEmpty(strInDocument))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRWWJBNWK").ToString().Trim() + "')", true);
                return;
            }

            //if (string.IsNullOrEmpty(strMainSupplier))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZGWZBNWKQJC").ToString().Trim() + "')", true);
            //    return;
            //}

            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //修改
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);
                string strSupplierHQL = "from WZSupplier as wZSupplier where ID = " + intID;
                IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierHQL);
                if (supplierList != null && supplierList.Count > 0)
                {
                    WZSupplier wZSupplier = (WZSupplier)supplierList[0];

                    //if (wZSupplier.Progress != LanguageHandle.GetWord("ShenQing").ToString().Trim())
                    //{
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWSBNXG").ToString().Trim()+"')", true);
                    //    return;
                    //}

                    wZSupplier.SupplierCode = strSupplierCode;
                    wZSupplier.SupplierNumber = strSupplierNumber;
                    wZSupplier.SupplierName = strSupplierName;
                    wZSupplier.OpeningBank = strOpeningBank;
                    wZSupplier.AccountNumber = strAccountNumber;
                    wZSupplier.RateNumber = strRateNumber;
                    wZSupplier.UnitAddress = strUnitAddress;
                    wZSupplier.ZipCode = strZipCode;
                    wZSupplier.UnitPhone = strUnitPhone;
                    wZSupplier.PersonDelegate = strPersonDelegate;
                    wZSupplier.DelegateAgent = strDelegateAgent;
                    wZSupplier.ContactPhone = strContactPhone;
                    wZSupplier.Mobile = strMobile;
                    wZSupplier.QQ = strQQ;
                    wZSupplier.Email = strEmail;
                    wZSupplier.MainSupplier = strMainSupplier;
                    wZSupplier.InDocument = strInDocument;
                    wZSupplier.InDocumentURL = strInDocumentURL;
                    wZSupplier.PushUnit = strPushUnit;
                    wZSupplier.PushPerson = strPushPerson;

                    wZSupplierBLL.UpdateWZSupplier(wZSupplier, intID);
                }
            }
            else
            {
                //增加
                WZSupplier wZSupplier = new WZSupplier();
                wZSupplier.SupplierCode = strSupplierCode;
                wZSupplier.SupplierNumber = strSupplierNumber;
                wZSupplier.SupplierName = strSupplierName;
                wZSupplier.OpeningBank = strOpeningBank;
                wZSupplier.AccountNumber = strAccountNumber;
                wZSupplier.RateNumber = strRateNumber;
                wZSupplier.UnitAddress = strUnitAddress;
                wZSupplier.ZipCode = strZipCode;
                wZSupplier.UnitPhone = strUnitPhone;
                wZSupplier.PersonDelegate = strPersonDelegate;
                wZSupplier.DelegateAgent = strDelegateAgent;
                wZSupplier.ContactPhone = strContactPhone;
                wZSupplier.Mobile = strMobile;
                wZSupplier.QQ = strQQ;
                wZSupplier.Email = strEmail;
                wZSupplier.MainSupplier = strMainSupplier;
                wZSupplier.InDocument = strInDocument;
                wZSupplier.InDocumentURL = strInDocumentURL;
                wZSupplier.PushUnit = strPushUnit;
                wZSupplier.PushPerson = strPushPerson;

                wZSupplier.InTime = DateTime.Now;
                wZSupplier.Progress = LanguageHandle.GetWord("ShenQing").ToString().Trim();
                //wZSupplier.ApproveTime = DateTime.Now;
                //wZSupplier.CancelTime = DateTime.Now;

                wZSupplierBLL.AddWZSupplier(wZSupplier);
            }

            //Response.Redirect("TTWZSupplierList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    public void SetControlState()
    {
        TXT_SupplierName.BackColor = Color.CornflowerBlue;
        TXT_OpeningBank.BackColor = Color.CornflowerBlue;
        TXT_PersonDelegate.BackColor = Color.CornflowerBlue;
        TXT_AccountNumber.BackColor = Color.CornflowerBlue;
        TXT_UnitPhone.BackColor = Color.CornflowerBlue;
        TXT_RateNumber.BackColor = Color.CornflowerBlue;
        TXT_UnitAddress.BackColor = Color.CornflowerBlue;
        TXT_ZipCode.BackColor = Color.CornflowerBlue;
        FUP_InDocument.BackColor = Color.CornflowerBlue;
        TXT_DelegateAgent.BackColor = Color.CornflowerBlue;
        TXT_ContactPhone.BackColor = Color.CornflowerBlue;
        TXT_Mobile.BackColor = Color.CornflowerBlue;
        TXT_QQ.BackColor = Color.CornflowerBlue;
        TXT_Email.BackColor = Color.CornflowerBlue;
        TXT_PushUnit.BackColor = Color.CornflowerBlue;
        TXT_PushPerson.BackColor = Color.CornflowerBlue;
        TXT_MainSupplier.BackColor = Color.CornflowerBlue;
    }


    protected void BT_InDocument_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;
        if (!string.IsNullOrEmpty(strID))
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


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_InDocument.Text = "<a href=\"" + strUrl + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_InDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_InDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
                    string strSupplierHQL = "from WZSupplier as wZSupplier where ID = " + strID;
                    IList listSupplier = wZSupplierBLL.GetAllWZSuppliers(strSupplierHQL);
                    if (listSupplier != null && listSupplier.Count > 0)
                    {
                        WZSupplier wZSupplier = (WZSupplier)listSupplier[0];
                        wZSupplier.InDocument = Path.GetFileNameWithoutExtension(strFileName2);
                        wZSupplier.InDocumentURL = strUrl;

                        wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);
                    }

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
        else
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
    }

    private void BindSupplierData(int ID)
    {
        string strWZSupplierSql = string.Format(@"select s.*,m.UserName as PushPersonName from T_WZSupplier s
                        left join T_ProjectMember m on s.PushPerson = m.UserCode
                        where s.ID = {0}", ID);
        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strWZSupplierSql, "Supplier").Tables[0];
        if (dtSupplier != null && dtSupplier.Rows.Count > 0)
        {
            DataRow drSupplier = dtSupplier.Rows[0];

            HF_SupplierCode.Value = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
            TXT_SupplierNumber.Text = ShareClass.ObjectToString(drSupplier["SupplierNumber"]);
            TXT_SupplierName.Text = ShareClass.ObjectToString(drSupplier["SupplierName"]);
            TXT_OpeningBank.Text =ShareClass.ObjectToString(drSupplier["OpeningBank"]);
            TXT_AccountNumber.Text = ShareClass.ObjectToString(drSupplier["AccountNumber"]);
            TXT_RateNumber.Text = ShareClass.ObjectToString(drSupplier["RateNumber"]);
            TXT_UnitAddress.Text =ShareClass.ObjectToString(drSupplier["UnitAddress"]);
            TXT_ZipCode.Text = ShareClass.ObjectToString(drSupplier["ZipCode"]);
            TXT_UnitPhone.Text = ShareClass.ObjectToString(drSupplier["UnitPhone"]);
            TXT_PersonDelegate.Text = ShareClass.ObjectToString(drSupplier["PersonDelegate"]);
            TXT_DelegateAgent.Text = ShareClass.ObjectToString(drSupplier["DelegateAgent"]);
            TXT_ContactPhone.Text = ShareClass.ObjectToString(drSupplier["ContactPhone"]);
            TXT_Mobile.Text = ShareClass.ObjectToString(drSupplier["Mobile"]);
            TXT_QQ.Text = ShareClass.ObjectToString(drSupplier["QQ"]);
            TXT_Email.Text = ShareClass.ObjectToString(drSupplier["Email"]);
            TXT_MainSupplier.Text = ShareClass.ObjectToString(drSupplier["MainSupplier"]);

            TXT_PushUnit.Text = ShareClass.ObjectToString(drSupplier["PushUnit"]);
            HF_PushPerson.Value = ShareClass.ObjectToString(drSupplier["PushPerson"]);
            TXT_PushPerson.Text = ShareClass.ObjectToString(drSupplier["PushPersonName"]);

            //附件列表
            string strInDocument = ShareClass.ObjectToString(drSupplier["InDocument"]);
            string strInDocumentURL = ShareClass.ObjectToString(drSupplier["InDocumentURL"]);

            LT_InDocument.Text = "<a href=\"" + strInDocumentURL + "\" class=\"notTab\" target=\"_blank\">" + strInDocument + "</a>";
            HF_InDocument.Value = strInDocument;
            HF_InDocumentURL.Value = strInDocumentURL;

            TXT_InTime.Text = ShareClass.ObjectToString(drSupplier["InTime"]);

            string strProgress = ShareClass.ObjectToString(drSupplier["Progress"]);

            //if (strProgress.Trim() != LanguageHandle.GetWord("ShenQing").ToString().Trim())
            //{
            //    TXT_SupplierNumber.ReadOnly = true;
            //    TXT_SupplierName.ReadOnly = true;
            //    TXT_OpeningBank.ReadOnly = true;
            //    TXT_AccountNumber.ReadOnly = true;
            //    TXT_RateNumber.ReadOnly = true;
            //    TXT_UnitAddress.ReadOnly = true;
            //    TXT_ZipCode.ReadOnly = true;
            //    TXT_UnitPhone.ReadOnly = true;
            //    TXT_PersonDelegate.ReadOnly = true;
            //    TXT_DelegateAgent.ReadOnly = true;
            //    TXT_ContactPhone.ReadOnly = true;
            //    TXT_Mobile.ReadOnly = true;
            //    TXT_QQ.ReadOnly = true;
            //    TXT_Email.ReadOnly = true;
            //    TXT_MainSupplier.ReadOnly = true;

            //    TXT_PushUnit.ReadOnly = true;
            //    TXT_PushPerson.ReadOnly = true;
            //    BT_InDocument.Enabled = false;
            //    FUP_InDocument.Enabled = false;
            //}
        }
    }


    private void BindSupplierData(string strUserCode)
    {
        string strWZSupplierSql = string.Format(@"select s.*,m.UserName as PushPersonName from T_WZSupplier s
                        left join T_ProjectMember m on s.PushPerson = m.UserCode 
                        where s.SupplierCode = '{0}'", strUserCode);
        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strWZSupplierSql, "Supplier").Tables[0];
        if (dtSupplier != null && dtSupplier.Rows.Count > 0)
        {
            DataRow drSupplier = dtSupplier.Rows[0];
            HF_SupplierCode.Value = ShareClass.ObjectToString(drSupplier["SupplierCode"]);
            TXT_SupplierNumber.Text = ShareClass.ObjectToString(drSupplier["SupplierNumber"]);
            TXT_SupplierName.Text = ShareClass.ObjectToString(drSupplier["SupplierName"]);
            TXT_OpeningBank.Text = ShareClass.ObjectToString(drSupplier["OpeningBank"]);
            TXT_AccountNumber.Text = ShareClass.ObjectToString(drSupplier["AccountNumber"]);
            TXT_RateNumber.Text = ShareClass.ObjectToString(drSupplier["RateNumber"]);
            TXT_UnitAddress.Text = ShareClass.ObjectToString(drSupplier["UnitAddress"]);
            TXT_ZipCode.Text = ShareClass.ObjectToString(drSupplier["ZipCode"]);
            TXT_UnitPhone.Text = ShareClass.ObjectToString(drSupplier["UnitPhone"]);
            TXT_PersonDelegate.Text = ShareClass.ObjectToString(drSupplier["PersonDelegate"]);
            TXT_DelegateAgent.Text = ShareClass.ObjectToString(drSupplier["DelegateAgent"]);
            TXT_ContactPhone.Text = ShareClass.ObjectToString(drSupplier["ContactPhone"]);
            TXT_Mobile.Text = ShareClass.ObjectToString(drSupplier["Mobile"]);
            TXT_QQ.Text = ShareClass.ObjectToString(drSupplier["QQ"]);
            TXT_Email.Text = ShareClass.ObjectToString(drSupplier["Email"]);
            TXT_MainSupplier.Text = ShareClass.ObjectToString(drSupplier["MainSupplier"]);

            TXT_PushUnit.Text = ShareClass.ObjectToString(drSupplier["PushUnit"]);
            HF_PushPerson.Value = ShareClass.ObjectToString(drSupplier["PushPerson"]);
            TXT_PushPerson.Text = ShareClass.ObjectToString(drSupplier["PushPersonName"]);

            //附件列表
            string strInDocument = ShareClass.ObjectToString(drSupplier["InDocument"]);
            string strInDocumentURL = ShareClass.ObjectToString(drSupplier["InDocumentURL"]);

            LT_InDocument.Text = "<a href=\"" + strInDocumentURL + "\" class=\"notTab\" target=\"_blank\">" + strInDocument + "</a>";
            HF_InDocument.Value = strInDocument;
            HF_InDocumentURL.Value = strInDocumentURL;

            HF_ID.Value = ShareClass.ObjectToString(drSupplier["ID"]);

            TXT_InTime.Text = ShareClass.ObjectToString(drSupplier["InTime"]);

            string strProgress = ShareClass.ObjectToString(drSupplier["Progress"]);

            //if (strProgress.Trim() != LanguageHandle.GetWord("ShenQing").ToString().Trim())
            //{
            //    TXT_SupplierNumber.ReadOnly = true;
            //    TXT_SupplierName.ReadOnly = true;
            //    TXT_OpeningBank.ReadOnly = true;
            //    TXT_AccountNumber.ReadOnly = true;
            //    TXT_RateNumber.ReadOnly = true;
            //    TXT_UnitAddress.ReadOnly = true;
            //    TXT_ZipCode.ReadOnly = true;
            //    TXT_UnitPhone.ReadOnly = true;
            //    TXT_PersonDelegate.ReadOnly = true;
            //    TXT_DelegateAgent.ReadOnly = true;
            //    TXT_ContactPhone.ReadOnly = true;
            //    TXT_Mobile.ReadOnly = true;
            //    TXT_QQ.ReadOnly = true;
            //    TXT_Email.ReadOnly = true;
            //    TXT_MainSupplier.ReadOnly = true;

            //    TXT_PushUnit.ReadOnly = true;
            //    TXT_PushPerson.ReadOnly = true;
            //    BT_InDocument.Enabled = false;
            //    FUP_InDocument.Enabled = false;
            //    btnSave.Enabled = false;
            //}
        }
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