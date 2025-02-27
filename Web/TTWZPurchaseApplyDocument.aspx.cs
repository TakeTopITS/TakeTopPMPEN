using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Drawing;
using System.Data;
using System.IO;


public partial class TTWZPurchaseApplyDocument : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["PurchaseCode"]))
            {
                string strPurchaseCode = Request.QueryString["PurchaseCode"].ToString();

                HF_NewPurchaseCode.Value = strPurchaseCode;

                DataBinder(strPurchaseCode);
            }
        }
    }

    private void DataBinder(string strPurchaseCode)
    {

        WZPurchaseSupplierBLL wZPurchaseSupplierBLL = new WZPurchaseSupplierBLL();
        string strPurchaseSupplierHQL = "from WZPurchaseSupplier as wZPurchaseSupplier where PurchaseCode = '" + strPurchaseCode + "' and SupplierCode = '" + strUserCode + "'";
        IList listPurchaseSupplier = wZPurchaseSupplierBLL.GetAllWZPurchaseSuppliers(strPurchaseSupplierHQL);
        if (listPurchaseSupplier != null && listPurchaseSupplier.Count > 0)
        {
            WZPurchaseSupplier wZPurchaseSupplier = (WZPurchaseSupplier)listPurchaseSupplier[0];

            LT_PurchaseDocument.Text = "<a href='" + ShareClass.ObjectToString(wZPurchaseSupplier.DocumentURL) + "' class=\"notTab\" target=\"_blank\">" + ShareClass.ObjectToString(wZPurchaseSupplier.DocumentName) + "</a>";
        }
    }

    protected void BT_PurchaseFile_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_NewPurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                string strPurchaseOfferDocument = FUP_PurchaseDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
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
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_PurchaseDocument.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;//DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_PurchaseDocument.Text = "<a href=\"" + strUrl + "\"  class=\"notTab\" target=\"_blank\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    WZPurchaseSupplierBLL wZPurchaseSupplierBLL = new WZPurchaseSupplierBLL();
                    string strPurchaseSupplierHQL = "from WZPurchaseSupplier as wZPurchaseSupplier where PurchaseCode = '" + strPurchaseCode + "' and SupplierCode = '" + strUserCode + "'";
                    IList listPurchaseSupplier = wZPurchaseSupplierBLL.GetAllWZPurchaseSuppliers(strPurchaseSupplierHQL);
                    if (listPurchaseSupplier != null && listPurchaseSupplier.Count > 0)
                    {
                        WZPurchaseSupplier wZPurchaseSupplier = (WZPurchaseSupplier)listPurchaseSupplier[0];
                        wZPurchaseSupplier.DocumentName = Path.GetFileNameWithoutExtension(strFileName2);
                        wZPurchaseSupplier.DocumentURL = strUrl;

                        wZPurchaseSupplierBLL.UpdateWZPurchaseSupplier(wZPurchaseSupplier, wZPurchaseSupplier.ID);

                        //更新采购文件中的投标文件
                        UpdateWZPurchaserTrendDocument(strPurchaseCode, strUserCode, Path.GetFileNameWithoutExtension(strFileName2), strUrl);
                    }


                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCBJWJCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCGWJ").ToString().Trim() + "')", true);
            return;
        }
    }


    protected void UpdateWZPurchaserTrendDocument(string strPurchaseCode,string strSupplierCode,string strDocumentName,string strDocumentURL)
    {
        string strHQL, strUpdateHQL;

        DataSet ds;

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and SupplierCode1 = " + "'" + strSupplierCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if(ds.Tables [0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument1 = " + "'" + strDocumentName + "'" + ",TenderDocumentURL1 = " + "'" + strDocumentURL + "'" + " Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and SupplierCode2 = " + "'" + strSupplierCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument2 = " + "'" + strDocumentName + "'" + ",TenderDocumentURL2 = " + "'" + strDocumentURL + "'" + " Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and SupplierCode3 = " + "'" + strSupplierCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument3 = " + "'" + strDocumentName + "'" + ",TenderDocumentURL3 = " + "'" + strDocumentURL + "'" + " Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and SupplierCode4 = " + "'" + strSupplierCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument4 = " + "'" + strDocumentName + "'" + ",TenderDocumentURL4 = " + "'" + strDocumentURL + "'" + " Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and SupplierCode5 = " + "'" + strSupplierCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument5 = " + "'" + strDocumentName + "'" + ",TenderDocumentURL5 = " + "'" + strDocumentURL + "'" + " Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and SupplierCode6 = " + "'" + strSupplierCode + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument6 = " + "'" + strDocumentName + "'" + ",TenderDocumentURL6 = " + "'" + strDocumentURL + "'" + " Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }
        
    }
}