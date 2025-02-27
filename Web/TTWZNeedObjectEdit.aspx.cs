using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZNeedObjectEdit : System.Web.UI.Page
{
    public string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString().Trim() : "";
        strUserName = Session["UserName"] != null ? Session["UserName"].ToString().Trim() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindDivideData(intID);
            }
            else {
                TXT_PurchaseEngineer.Text = strUserName;
                HF_PurchaseEngineer.Value = strUserCode;

                BindDivideDataByUserCode(strUserCode);
            }

            SetControlState();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strID = HF_ID.Value;

            string strVendee = TXT_Vendee.Text.Trim();                                              //买受人
            string strPersonDelegate = TXT_PersonDelegate.Text.Trim();                              //法人代表
            string strOpeningBank = TXT_OpeningBank.Text.Trim();                                    //开户银行
            string strAccountNumber = TXT_AccountNumber.Text.Trim();                                //帐号
            string strRateNumber = TXT_RateNumber.Text.Trim();                                      //税号
            string strUnitAddress = TXT_UnitAddress.Text.Trim();                                    //单位地址
            string strZipCode = TXT_ZipCode.Text.Trim();                                            //邮编
            string strAccountPhone = TXT_AccountPhone.Text.Trim();                                  //财务电话
            string strInternetUrl = TXT_InternetUrl.Text.Trim();                                    //网址
            string strFax = TXT_Fax.Text.Trim();                                                    //传真
            string strContactPhone = TXT_ContactPhone.Text.Trim();                                  //联系电话
            string strMobile = TXT_Mobile.Text.Trim();                                              //手机
            string strEmail = TXT_Email.Text.Trim();                                                //E-mail
            string strQQ = TXT_QQ.Text.Trim();                                                      //QQ

            if (!ShareClass.CheckStringRight(strVendee))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMSRBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPersonDelegate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFRDBBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOpeningBank))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZKHYXBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAccountNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRateNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strUnitAddress))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWDZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strZipCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYBBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strInternetUrl))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            //if (string.IsNullOrEmpty(strFax))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZBNWKBC").ToString().Trim()+"')", true);
            //    return;
            //}
            if (!ShareClass.CheckStringRight(strFax))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            WZNeedObjectBLL wZNeedObjectBLL = new WZNeedObjectBLL();

            if (!string.IsNullOrEmpty(strID))
            {
                //修改
                int intID = 0;
                int.TryParse(strID, out intID);
                string strNeedObjectHQL = "from WZNeedObject as wZNeedObject where ID = " + intID;
                IList needObjectList = wZNeedObjectBLL.GetAllWZNeedObjects(strNeedObjectHQL);
                if (needObjectList != null && needObjectList.Count > 0)
                {
                    WZNeedObject wZNeedObject = (WZNeedObject)needObjectList[0];

                    wZNeedObject.NeedCode = LB_NeedCode.Text.Trim();
                    wZNeedObject.Vendee = strVendee;
                    wZNeedObject.PersonDelegate = strPersonDelegate;
                    wZNeedObject.OpeningBank = strOpeningBank;
                    wZNeedObject.AccountNumber = strAccountNumber;
                    wZNeedObject.RateNumber = strRateNumber;
                    wZNeedObject.UnitAddress = strUnitAddress;
                    wZNeedObject.ZipCode = strZipCode;
                    wZNeedObject.AccountPhone = strAccountPhone;
                    wZNeedObject.InternetUrl = strInternetUrl;
                    //wZNeedObject.PurchaseEngineer = TXT_PurchaseEngineer.Text.Trim();
                    wZNeedObject.Fax = strFax;
                    wZNeedObject.ContactPhone = strContactPhone;
                    wZNeedObject.Mobile = strMobile;
                    wZNeedObject.Email = strEmail;
                    wZNeedObject.QQ = strQQ;

                    wZNeedObjectBLL.UpdateWZNeedObject(wZNeedObject, intID);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
            }
            else
            {
                //判断是否已经存在了采购工程师
                string strCheckNeedObjectHQL = "select count(1) as RowNumber from T_WZNeedObject where PurchaseEngineer = '" + strUserCode + "'";
                DataTable dtCheckNeedObject = ShareClass.GetDataSetFromSql(strCheckNeedObjectHQL, "NeedObject").Tables[0];
                int intRowNumber = int.Parse(dtCheckNeedObject.Rows[0]["RowNumber"].ToString());
                if (intRowNumber > 0 && string.IsNullOrEmpty(strID))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNYCZWZF").ToString().Trim()+"')", true);
                    return;
                }

                //增加
                WZNeedObject wZNeedObject = new WZNeedObject();

                wZNeedObject.NeedCode = CreateNewNeedCode();//LB_NeedCode.Text.Trim(); 生成需方编码
                wZNeedObject.Vendee = strVendee;
                wZNeedObject.PersonDelegate = strPersonDelegate;
                wZNeedObject.OpeningBank = strOpeningBank;
                wZNeedObject.AccountNumber = strAccountNumber;
                wZNeedObject.RateNumber = strRateNumber;
                wZNeedObject.UnitAddress = strUnitAddress;
                wZNeedObject.ZipCode = strZipCode;
                wZNeedObject.AccountPhone = strAccountPhone;
                wZNeedObject.InternetUrl = strInternetUrl;
                wZNeedObject.PurchaseEngineer = strUserCode;//TXT_PurchaseEngineer.Text.Trim();
                wZNeedObject.Fax = strFax;
                wZNeedObject.ContactPhone = strContactPhone;
                wZNeedObject.Mobile = strMobile;
                wZNeedObject.Email = strEmail;
                wZNeedObject.QQ = strQQ;
                wZNeedObjectBLL.AddWZNeedObject(wZNeedObject);
            }

            //Response.Redirect("TTWZNeedObjectList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    public void SetControlState()
    {
        TXT_Vendee.BackColor = Color.CornflowerBlue;
        TXT_PersonDelegate.BackColor = Color.CornflowerBlue;
        TXT_UnitAddress.BackColor = Color.CornflowerBlue;
        TXT_ZipCode.BackColor = Color.CornflowerBlue;
        TXT_OpeningBank.BackColor = Color.CornflowerBlue;
        TXT_AccountNumber.BackColor = Color.CornflowerBlue;
        TXT_AccountPhone.BackColor = Color.CornflowerBlue;
        TXT_RateNumber.BackColor = Color.CornflowerBlue;
        TXT_InternetUrl.BackColor = Color.CornflowerBlue;

        TXT_Fax.BackColor = Color.CornflowerBlue;
        TXT_ContactPhone.BackColor = Color.CornflowerBlue;
        TXT_Mobile.BackColor = Color.CornflowerBlue;
        TXT_QQ.BackColor = Color.CornflowerBlue;
        TXT_Email.BackColor = Color.CornflowerBlue;
    }


    private void BindDivideData(int ID)
    {
        string strWZNeedObjectSql = string.Format(@"select o.*,p.UserName as PurchaseEngineerName from T_WZNeedObject o
                        left join T_ProjectMember p on o.PurchaseEngineer = p.UserCode 
                        where o.ID = {0}", ID);
        DataTable dtNeedObject = ShareClass.GetDataSetFromSql(strWZNeedObjectSql, "NeedObject").Tables[0];
        if (dtNeedObject != null && dtNeedObject.Rows.Count > 0)
        {
            DataRow drNeedObject = dtNeedObject.Rows[0];

            LB_NeedCode.Text = ShareClass.ObjectToString(drNeedObject["NeedCode"]);
            TXT_Vendee.Text = ShareClass.ObjectToString(drNeedObject["Vendee"]);
            TXT_PersonDelegate.Text = ShareClass.ObjectToString(drNeedObject["PersonDelegate"]);
            TXT_OpeningBank.Text = ShareClass.ObjectToString(drNeedObject["OpeningBank"]);
            TXT_AccountNumber.Text = ShareClass.ObjectToString(drNeedObject["AccountNumber"]);
            TXT_RateNumber.Text = ShareClass.ObjectToString(drNeedObject["RateNumber"]);
            TXT_UnitAddress.Text = ShareClass.ObjectToString(drNeedObject["UnitAddress"]);
            TXT_ZipCode.Text = ShareClass.ObjectToString(drNeedObject["ZipCode"]);
            TXT_AccountPhone.Text = ShareClass.ObjectToString(drNeedObject["AccountPhone"]);
            TXT_InternetUrl.Text = ShareClass.ObjectToString(drNeedObject["InternetUrl"]);
            TXT_PurchaseEngineer.Text = ShareClass.ObjectToString(drNeedObject["PurchaseEngineerName"]);
            HF_PurchaseEngineer.Value = ShareClass.ObjectToString(drNeedObject["PurchaseEngineer"]);
            TXT_Fax.Text = ShareClass.ObjectToString(drNeedObject["Fax"]);
            TXT_ContactPhone.Text = ShareClass.ObjectToString(drNeedObject["ContactPhone"]);
            TXT_Mobile.Text = ShareClass.ObjectToString(drNeedObject["Mobile"]);
            TXT_Email.Text = ShareClass.ObjectToString(drNeedObject["Email"]);
            TXT_QQ.Text = ShareClass.ObjectToString(drNeedObject["QQ"]);
        }
    }



    private void BindDivideDataByUserCode(string strPurchaseEngineer)
    {
        string strWZNeedObjectSql = string.Format(@"select o.*,p.UserName as PurchaseEngineerName from T_WZNeedObject o
                        left join T_ProjectMember p on o.PurchaseEngineer = p.UserCode 
                        where o.PurchaseEngineer = '{0}'", strPurchaseEngineer);
        DataTable dtNeedObject = ShareClass.GetDataSetFromSql(strWZNeedObjectSql, "NeedObject").Tables[0];
        if (dtNeedObject != null && dtNeedObject.Rows.Count > 0)
        {
            DataRow drNeedObject = dtNeedObject.Rows[0];

            LB_NeedCode.Text = ShareClass.ObjectToString(drNeedObject["NeedCode"]);
            TXT_Vendee.Text = ShareClass.ObjectToString(drNeedObject["Vendee"]);
            TXT_PersonDelegate.Text = ShareClass.ObjectToString(drNeedObject["PersonDelegate"]);
            TXT_OpeningBank.Text = ShareClass.ObjectToString(drNeedObject["OpeningBank"]);
            TXT_AccountNumber.Text = ShareClass.ObjectToString(drNeedObject["AccountNumber"]);
            TXT_RateNumber.Text = ShareClass.ObjectToString(drNeedObject["RateNumber"]);
            TXT_UnitAddress.Text = ShareClass.ObjectToString(drNeedObject["UnitAddress"]);
            TXT_ZipCode.Text = ShareClass.ObjectToString(drNeedObject["ZipCode"]);
            TXT_AccountPhone.Text = ShareClass.ObjectToString(drNeedObject["AccountPhone"]);
            TXT_InternetUrl.Text = ShareClass.ObjectToString(drNeedObject["InternetUrl"]);
            TXT_PurchaseEngineer.Text = ShareClass.ObjectToString(drNeedObject["PurchaseEngineerName"]);
            HF_PurchaseEngineer.Value = ShareClass.ObjectToString(drNeedObject["PurchaseEngineer"]);
            TXT_Fax.Text = ShareClass.ObjectToString(drNeedObject["Fax"]);
            TXT_ContactPhone.Text = ShareClass.ObjectToString(drNeedObject["ContactPhone"]);
            TXT_Mobile.Text = ShareClass.ObjectToString(drNeedObject["Mobile"]);
            TXT_Email.Text = ShareClass.ObjectToString(drNeedObject["Email"]);
            TXT_QQ.Text = ShareClass.ObjectToString(drNeedObject["QQ"]);

            HF_ID.Value = ShareClass.ObjectToString(drNeedObject["ID"]);
        }
    }


    /// <summary>
    ///  生成需方Code
    /// </summary>
    private string CreateNewNeedCode()
    {
        string strNewNeedCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                int intNeedCodeNumber = 1;
                do
                {
                    StringBuilder sbNeedCode = new StringBuilder();
                    for (int j = 4 - intNeedCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbNeedCode.Append("0");
                    }
                    strNewNeedCode = sbNeedCode.ToString() + "" + intNeedCodeNumber.ToString();

                    //验证新的需方Code是否存在
                    string strCheckNewNeedCodeHQL = "select count(1) as RowNumber from T_WZNeedObject where NeedCode = '" + strNewNeedCode + "'";
                    DataTable dtCheckNewNeedCode = ShareClass.GetDataSetFromSql(strCheckNewNeedCodeHQL, "NewNeedCode").Tables[0];
                    int intCheckNewNeedCode = int.Parse(dtCheckNewNeedCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewNeedCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intNeedCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewNeedCode;
        #region 注释原来的
        //string strNewNeedCode = string.Empty;
        //try
        //{
        //    lock (this)
        //    {
        //        bool isExist = true;
        //        string strNeedCodeHQL = "select count(1) as RowNumber from T_WZNeedObject";
        //        DataTable dtNeedCode = ShareClass.GetDataSetFromSql(strNeedCodeHQL, "NeedCode").Tables[0];
        //        int intNeedCodeNumber = int.Parse(dtNeedCode.Rows[0]["RowNumber"].ToString());
        //        intNeedCodeNumber = intNeedCodeNumber + 1;
        //        do
        //        {
        //            StringBuilder sbNeedCode = new StringBuilder();
        //            for (int j = 3 - intNeedCodeNumber.ToString().Length; j > 0; j--)
        //            {
        //                sbNeedCode.Append("0");
        //            }
        //            strNewNeedCode = sbNeedCode.ToString() + "" + intNeedCodeNumber.ToString();

        //            //验证新的需方Code是否存在
        //            string strCheckNewNeedCodeHQL = "select count(1) as RowNumber from T_WZNeedObject where NeedCode = '" + strNewNeedCode + "'";
        //            DataTable dtCheckNewNeedCode = ShareClass.GetDataSetFromSql(strCheckNewNeedCodeHQL, "NewNeedCode").Tables[0];
        //            int intCheckNewNeedCode = int.Parse(dtCheckNewNeedCode.Rows[0]["RowNumber"].ToString());
        //            if (intCheckNewNeedCode == 0)
        //            {
        //                isExist = false;
        //            }
        //            else
        //            {
        //                intNeedCodeNumber++;
        //            }
        //        } while (isExist);
        //    }
        //}
        //catch (Exception ex) { }
        //return strNewNeedCode;
        #endregion
    }
}