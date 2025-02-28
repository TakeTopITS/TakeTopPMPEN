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
            string strInDocument = HF_InDocument.Value;                                          //�ϴ��ĸ���·��
            string strInDocumentURL = HF_InDocumentURL.Value;                                          //�ϴ��ĸ���·��
            string strPushPerson = TXT_PushPerson.Text;

            //��֤�Ƿ�Ƿ��ַ�
            if (string.IsNullOrEmpty(strSupplierName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGYSMCZDKTX+"')", true);
                return;
            }
            if (strSupplierName.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGYSMCCDBCG30GZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSupplierName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGYSMCBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strOpeningBank))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKHXZDKTX+"')", true);
                return;
            }
            if (strOpeningBank.Length > 26)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKHXCDBNCG26GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOpeningBank))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKHXBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPersonDelegate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRDBZDKTX+"')", true);
                return;
            }
            if (strPersonDelegate.Length > 20)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRDBCDBNCG4GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPersonDelegate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRDBBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strAccountNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZHZDKTX+"')", true);
                return;
            }
            if (strAccountNumber.Length > 40)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZHCDBNCG40GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAccountNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZHBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strUnitPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDWDHZDKTX+"')", true);
                return;
            }
            if (strUnitPhone.Length > 40)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDWDHCDBNCG14GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strUnitPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDWDHBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strRateNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHZDKTX+"')", true);
                return;
            }
            if (strRateNumber.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHCDBNCG30GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRateNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSHBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strZipCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYBZDKTX+"')", true);
                return;
            }
            if (strZipCode.Length > 6)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYBCDBNCG6GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strZipCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYBBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strUnitAddress))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDWDZZDKTX+"')", true);
                return;
            }
            if (strUnitAddress.Length > 26)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDWDZCDBNCG26GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strUnitAddress))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDWDZBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strDelegateAgent))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWTDLRZDKTX+"')", true);
                return;
            }
            if (strDelegateAgent.Length > 4)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWTDLRCDBNCG4GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDelegateAgent))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZWTDLRBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strContactPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXDHZDKTX+"')", true);
                return;
            }
            if (strContactPhone.Length > 14)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXDHCDBNCG14GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strContactPhone))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXDHBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strMobile))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSJZDKTX+"')", true);
                return;
            }
            if (strMobile.Length > 11)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSJCDBNCG11GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMobile))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSJBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strEmail))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZEMAILZDKTX+"')", true);
                return;
            }
            if (strEmail.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZEMAILCDBNCG30GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strEmail))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZEMAILBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strQQ))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZQQZDKTX+"')", true);
                return;
            }
            if (strQQ.Length > 12)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZQQCDBNCG12GZFC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strQQ))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZQQBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPushPerson))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTJRZDKTX+"')", true);
                return;
            }

            //����
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
            wZSupplierRegister.Progress = "¼��";

            wZSupplierRegisterBLL.AddWZSupplierRegister(wZSupplierRegister);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ע��ɹ����Ⱥ��̨��ˣ�');", true);
        }
        catch (Exception ex)
        { }
    }



    protected void BT_InDocument_Click(object sender, EventArgs e)
    {
        try
        {
            string strPurchaseOfferDocument = FUP_InDocument.PostedFile.FileName;   //��ȡ�ϴ��ļ����ļ���,������׺
            if (!string.IsNullOrEmpty(strPurchaseOfferDocument))
            {
                string strExtendName = System.IO.Path.GetExtension(strPurchaseOfferDocument);//��ȡ��չ��

                DateTime dtUploadNow = DateTime.Now; //��ȡϵͳʱ��
                string strFileName2 = System.IO.Path.GetFileName(strPurchaseOfferDocument);
                string strExtName = Path.GetExtension(strFileName2);

                string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                if (fi.Exists)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+Resources.lang.ZZCZTMWJSCSBGMHZSC+"');</script>");
                }

                if (Directory.Exists(strDocSavePath) == false)
                {
                    //��������ھʹ���file�ļ���{
                    Directory.CreateDirectory(strDocSavePath);
                }

                FUP_InDocument.SaveAs(strDocSavePath + strFileName3);

                LT_InDocument.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\" class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                HF_InDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                HF_InDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                //���¼��ر����ļ��б�
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCRWWJCG+"')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZYSCDWJ+"')", true);
                return;
            }
        }
        catch (Exception ex) { }
    }


    /// <summary>
    ///  ���ɹ�Ӧ�̱��
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

                    //��֤�µĹ�Ӧ�̱���Ƿ����
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