using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Drawing;
using System.Data;
using System.IO;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPurchaseApply : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strPurchaseHQL = string.Format(@"select distinct p.*,s.DocumentName as TenderDocument,s.DocumentURL as TenderDocumentURL,s.IsConfirm,s.SupplierCode,
                        pe.UserName as PurchaseEngineerName,
                        pm.UserName as TenderCompetentName
                        from T_WZPurchase p
                        left join T_WZPurchaseSupplier s on p.PurchaseCode = s.PurchaseCode
                        left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                        left join T_ProjectMember pm on p.TenderCompetent = pm.UserCode
                        where s.SupplierCode = '{0}'
                        and p.Progress in('询价','报价')
                        order by p.MarkTime desc", strUserCode); 


        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_RowCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;

            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdArges = e.CommandArgument.ToString();
            if (cmdName == "click")
            {
                string strPurchaseHQL = string.Format(@"select distinct p.*,s.DocumentName,s.DocumentURL,s.IsConfirm,s.SupplierCode
                        from T_WZPurchase p
                        left join T_WZPurchaseSupplier s on p.PurchaseCode = s.PurchaseCode
                        where p.PurchaseCode = '{0}'
                        and s.SupplierCode = '{1}'", cmdArges, strUserCode);

                DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
                if (dtPurchase != null && dtPurchase.Rows.Count > 0)
                {
                    DataRow drPurchase = dtPurchase.Rows[0];

                    ControlStatusChange(ShareClass.ObjectToString(drPurchase["SupplierCode"]).Trim(), ShareClass.ObjectToString(drPurchase["Progress"]).Trim(), ShareClass.ObjectToString(drPurchase["PurchaseEndTime"]).Trim(), ShareClass.ObjectToString(drPurchase["PurchaseCode"]));

                    HF_NewPurchaseCode.Value = ShareClass.ObjectToString(drPurchase["PurchaseCode"]);

                    string strPurchaseCode = ShareClass.ObjectToString(drPurchase["PurchaseCode"]);
                    string strSupplierCode = strUserCode;// ShareClass.ObjectToString(drPurchase["SupplierCode"]);
                    string strUpdatePurchaseSupplierSQL = string.Format("Select * From T_WZPurchaseOfferRecord Where Progress = '报价' and PurchaseCode = '{0}' and SupplierCode = '{1}'", strPurchaseCode, strSupplierCode); 
                    DataSet ds = ShareClass.GetDataSetFromSql(strUpdatePurchaseSupplierSQL, "T_WZPurchaseOfferRecord");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        BT_NewSubmit.Enabled = false;
                        BT_NewSubmitReturn.Enabled = true;
                    }
                }
            }
            else if (cmdName == "edit")
            {
                string strPurchaseHQL = string.Format(@"select distinct p.*,s.DocumentName,s.DocumentURL,s.IsConfirm from T_WZPurchase p
                        left join T_WZPurchaseSupplier s on p.PurchaseCode = s.PurchaseCode
                        where s.SupplierCode = '{1}'
                        and p.PurchaseCode = '{0}'", cmdArges, strUserCode);

                DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
                if (dtPurchase != null && dtPurchase.Rows.Count > 0)
                {
                    DataRow drPurchase = dtPurchase.Rows[0];

                    TXT_PurchaseCode.Text = ShareClass.ObjectToString(drPurchase["PurchaseCode"]);
                    TXT_PurchaseName.Text = ShareClass.ObjectToString(drPurchase["PurchaseName"]);

                    LT_PurchaseDocument.Text = "<a href='" + ShareClass.ObjectToString(drPurchase["DocumentURL"]) + "' class=\"notTab\" target=\"_blank\">" + ShareClass.ObjectToString(drPurchase["DocumentName"]) + "</a>";
                }

            }
            else if (cmdName == "submit")
            {
                //提交
                string strPurchaseHQL = string.Format(@"select distinct p.*,s.DocumentName,s.DocumentURL,s.IsConfirm,s.ID as PurchaseSupplierID from T_WZPurchase p
                        left join T_WZPurchaseSupplier s on p.PurchaseCode = s.PurchaseCode
                        where s.SupplierCode = '{1}'
                        and p.PurchaseCode = '{0}'", cmdArges, strUserCode);

                DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
                if (dtPurchase != null && dtPurchase.Rows.Count > 0)
                {
                    DataRow drPurchase = dtPurchase.Rows[0];

                    DateTime dtPurchaseEndTime = DateTime.Now;
                    DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseEndTime"]), out dtPurchaseEndTime);
                    if (dtPurchaseEndTime < DateTime.Now)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJJZRYGBNZTJ").ToString().Trim() + "')", true);
                        return;
                    }
                    DateTime dtPurchaseStartTime = DateTime.Now;
                    DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseStartTime"]), out dtPurchaseStartTime);
                    if (dtPurchaseStartTime > DateTime.Now)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJKSRWDBNTJ").ToString().Trim() + "')", true);
                        return;
                    }
                    if (ShareClass.ObjectToString(drPurchase["Progress"]) != LanguageHandle.GetWord("XunJia").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSJBNZTJ").ToString().Trim() + "')", true);
                        return;
                    }

                    if (string.IsNullOrEmpty(ShareClass.ObjectToString(drPurchase["DocumentName"])) || string.IsNullOrEmpty(ShareClass.ObjectToString(drPurchase["DocumentURL"])))
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXSCBJWJZTJ").ToString().Trim() + "')", true);
                        return;
                    }

                    string strSupplierApplyDetailSQL = string.Format(@"select * from T_WZSupplierApplyDetail
                                where SupplierCode = '{0}'
                                and PurchaseCode = '{1}'", strUserCode, cmdArges);
                    DataTable dtSupplierApplyDetail = ShareClass.GetDataSetFromSql(strSupplierApplyDetailSQL, "SupplierApplyDetail").Tables[0];
                    if (dtSupplierApplyDetail == null || dtSupplierApplyDetail.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXTXBJMXZTJ").ToString().Trim() + "')", true);
                        return;
                    }

                    string strPurchaseSupplierID = ShareClass.ObjectToString(drPurchase["PurchaseSupplierID"]);
                    string strUpdatePurchaseSupplierSQL = string.Format("update T_WZPurchaseSupplier set IsConfirm = '1' where ID = {0}", strPurchaseSupplierID);

                    ShareClass.RunSqlCommand(strUpdatePurchaseSupplierSQL);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);
                }
            }
            else if (cmdName == "cancel")
            {
                //退回
                string strPurchaseHQL = string.Format(@"select distinct p.*,s.DocumentName,s.DocumentURL,s.IsConfirm,s.ID as PurchaseSupplierID from T_WZPurchase p
                        left join T_WZPurchaseSupplier s on p.PurchaseCode = s.PurchaseCode
                        where s.SupplierCode = '{1}'
                        and p.PurchaseCode = '{0}'", cmdArges, strUserCode);

                DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
                if (dtPurchase != null && dtPurchase.Rows.Count > 0)
                {
                    DataRow drPurchase = dtPurchase.Rows[0];

                    DateTime dtPurchaseEndTime = DateTime.Now;
                    DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseEndTime"]), out dtPurchaseEndTime);
                    if (dtPurchaseEndTime < DateTime.Now)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJJZRYGBNZTH").ToString().Trim() + "')", true);
                        return;
                    }
                    DateTime dtPurchaseStartTime = DateTime.Now;
                    DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseStartTime"]), out dtPurchaseStartTime);
                    if (dtPurchaseStartTime > DateTime.Now)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJKSRWDBNTH").ToString().Trim() + "')", true);
                        return;
                    }
                    if (ShareClass.ObjectToString(drPurchase["Progress"]) != LanguageHandle.GetWord("XunJia").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSJBNZTH").ToString().Trim() + "')", true);
                        return;
                    }


                    string strPurchaseSupplierID = ShareClass.ObjectToString(drPurchase["PurchaseSupplierID"]);
                    string strUpdatePurchaseSupplierSQL = string.Format("update T_WZPurchaseSupplier set IsConfirm = '0' where ID = {0}", strPurchaseSupplierID);

                    ShareClass.RunSqlCommand(strUpdatePurchaseSupplierSQL);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTHCG").ToString().Trim() + "')", true);
                }
            }
            else if (cmdName == "detail")
            {
                //明细
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickToDetail('" + cmdArges + "')", true);
            }
        }
    }


    private void DataPurchaseDocumentBinder(string strPurchaseCode)
    {
        WZPurchaseDocumentBLL wZPurchaseDocumentBLL = new WZPurchaseDocumentBLL();
        string strPurchaseDocumentHQL = "from WZPurchaseDocument as wZPurchaseDocument where PurchaseCode = '" + strPurchaseCode + "'";
        IList lstPurchaseDocument = wZPurchaseDocumentBLL.GetAllWZPurchaseDocuments(strPurchaseDocumentHQL);

        RPT_PurchaseDocument.DataSource = lstPurchaseDocument;
        RPT_PurchaseDocument.DataBind();
    }

    protected void BT_PurchaseFile_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = TXT_PurchaseCode.Text;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {

                WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
                if (listPurchase != null && listPurchase.Count == 1)
                {
                    WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                    DateTime dtEditPurchaseEndTime = DateTime.Now;
                    DateTime.TryParse(wZPurchase.PurchaseEndTime, out dtEditPurchaseEndTime);
                    if (dtEditPurchaseEndTime < DateTime.Now)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJJZRYGBNZBJ").ToString().Trim() + "')", true);
                        return;
                    }

                    DateTime dtPurchaseStartTime = DateTime.Now;
                    DateTime.TryParse(wZPurchase.PurchaseStartTime, out dtPurchaseStartTime);
                    if (dtPurchaseStartTime > DateTime.Now)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJKSRWDBNBJ").ToString().Trim() + "')", true);
                        return;
                    }
                    if (wZPurchase.Progress != LanguageHandle.GetWord("XunJia").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSJBNZBJ").ToString().Trim() + "')", true);
                        return;
                    }
                }

                //判断是否已经提交
                string strCheckPurchaseSupplierSQL = string.Format(@"select * from T_WZPurchaseSupplier
                                where SupplierCode = '{0}'
                                and PurchaseCode = '{1}'", strUserCode, strPurchaseCode);
                DataTable dtCheckPurchaseSupplier = ShareClass.GetDataSetFromSql(strCheckPurchaseSupplierSQL, "CheckPurchaseSupplier").Tables[0];
                if (dtCheckPurchaseSupplier != null && dtCheckPurchaseSupplier.Rows.Count > 0)
                {
                    string strIsConfirm = ShareClass.ObjectToString(dtCheckPurchaseSupplier.Rows[0]["IsConfirm"]);
                    if (strIsConfirm == "1")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJYJTJBNSCBJWJXTH").ToString().Trim() + "')", true);
                        return;
                    }
                }

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
                    }

                    DataBinder();

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

    protected void BT_NewTenderDocument_Click(object sender, EventArgs e)
    {
        //招标文件下载
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }


        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (File.Exists(Server.MapPath(wZPurchase.PurchaseDocumentURL)))
            {
                try
                {
                    Response.Redirect(wZPurchase.PurchaseDocumentURL, false);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message.ToString());
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoWenJianBuCunZaiQingJi").ToString().Trim()+"')", true);

            }
        }
    }

    protected void BT_NewThrowDocument_Click(object sender, EventArgs e)
    {
        //投标文件上传
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseApplyDocument.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
        return;
    }


    protected void BT_NewThrowDocumentDel_Click(object sender, EventArgs e)
    {
        //投标文件删除
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        WZPurchaseSupplierBLL wZPurchaseSupplierBLL = new WZPurchaseSupplierBLL();
        string strPurchaseSupplierHQL = "from WZPurchaseSupplier as wZPurchaseSupplier where PurchaseCode = '" + strEditPurchaseCode + "' and SupplierCode = '" + strUserCode + "'";
        IList listPurchaseSupplier = wZPurchaseSupplierBLL.GetAllWZPurchaseSuppliers(strPurchaseSupplierHQL);
        if (listPurchaseSupplier != null && listPurchaseSupplier.Count > 0)
        {
            WZPurchaseSupplier wZPurchaseSupplier = (WZPurchaseSupplier)listPurchaseSupplier[0];

            string strPurchaseCode = wZPurchaseSupplier.PurchaseCode.Trim();
            string strDocumentURL = wZPurchaseSupplier.DocumentURL;

            //删除采购文件相应的文档
            DeleteWZPurchaserTrendDocument(strPurchaseCode, strDocumentURL);

            wZPurchaseSupplier.DocumentName = "";
            wZPurchaseSupplier.DocumentURL = "";

            wZPurchaseSupplierBLL.UpdateWZPurchaseSupplier(wZPurchaseSupplier, wZPurchaseSupplier.ID);


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
    }

    protected void DeleteWZPurchaserTrendDocument(string strPurchaseCode, string strDocumentURL)
    {
        string strHQL, strUpdateHQL;

        DataSet ds;

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and TenderDocumentURL1 = " + "'" + strDocumentURL + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument1 = ''" + ",TenderDocumentURL1 = '' Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and TenderDocumentURL2 = " + "'" + strDocumentURL + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument2 = ''" + ",TenderDocumentURL2 = '' Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and TenderDocumentURL3 = " + "'" + strDocumentURL + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument3 = ''" + ",TenderDocumentURL3 = '' Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and TenderDocumentURL4 = " + "'" + strDocumentURL + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument4 = ''" + ",TenderDocumentURL4 = '' Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and TenderDocumentURL5 = " + "'" + strDocumentURL + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument5 = ''" + ",TenderDocumentURL5 = '' Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

        strHQL = "Select * From T_WZPurchase Where PurchaseCode = '" + strPurchaseCode + "'" + " and TenderDocumentURL6 = " + "'" + strDocumentURL + "'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZPurchase");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpdateHQL = "Update T_WZPurchase Set TenderDocument6 = ''" + ",TenderDocumentURL6 = '' Where PurchaseCode = '" + strPurchaseCode + "'";
            ShareClass.RunSqlCommand(strUpdateHQL);

            return;
        }

    }

    protected void BT_NewApplyDetail_Click(object sender, EventArgs e)
    {
        //填写报价单
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseApplyDetail.aspx?PurchaseCode=" + strEditPurchaseCode + "')", true);
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickToDetail('" + strEditPurchaseCode + "')", true);
    }


    protected void BT_NewSubmit_Click(object sender, EventArgs e)
    {
        //提交报价
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        string strPurchaseHQL = string.Format(@"select distinct p.*,s.DocumentName,s.DocumentURL,s.IsConfirm,s.ID as PurchaseSupplierID from T_WZPurchase p
                        left join T_WZPurchaseSupplier s on p.PurchaseCode = s.PurchaseCode
                        where s.SupplierCode = '{1}'
                        and p.PurchaseCode = '{0}'", strEditPurchaseCode, strUserCode);

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
        if (dtPurchase != null && dtPurchase.Rows.Count > 0)
        {
            DataRow drPurchase = dtPurchase.Rows[0];

            DateTime dtPurchaseEndTime = DateTime.Now;
            //DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseEndTime"]), out dtPurchaseEndTime);
            //if (dtPurchaseEndTime < DateTime.Now)
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJJZRYGBNZTJ").ToString().Trim() + "')", true);
            //    return;
            //}
            DateTime dtPurchaseStartTime = DateTime.Now;
            DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseStartTime"]), out dtPurchaseStartTime);
            //if (dtPurchaseStartTime > DateTime.Now)
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJKSRWDBNTJ").ToString().Trim() + "')", true);
            //    return;
            //}
            if (ShareClass.ObjectToString(drPurchase["Progress"]) != LanguageHandle.GetWord("XunJia").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSJBNZTJ").ToString().Trim() + "')", true);
                return;
            }

            if (string.IsNullOrEmpty(ShareClass.ObjectToString(drPurchase["DocumentName"])) || string.IsNullOrEmpty(ShareClass.ObjectToString(drPurchase["DocumentURL"])))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXSCBJWJZTJ").ToString().Trim() + "')", true);
                return;
            }

            string strSupplierApplyDetailSQL = string.Format(@"select * from T_WZPurchaseOfferRecord
                                where SupplierCode = '{0}'
                                and PurchaseCode = '{1}'", strUserCode, strEditPurchaseCode);
            DataTable dtSupplierApplyDetail = ShareClass.GetDataSetFromSql(strSupplierApplyDetailSQL, "PurchaseOfferRecord").Tables[0];
            if (dtSupplierApplyDetail != null && dtSupplierApplyDetail.Rows.Count > 0)
            {

                string strTendersFirst = "", strTenders;
                int intSortNumber = 0, j = 0, k = 0;

                foreach (DataRow drPurchaseOfferRecord in dtSupplierApplyDetail.Rows)
                {
                    decimal decimalApplyMoney = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drPurchaseOfferRecord["ApplyMoney"]), out decimalApplyMoney);

                    if (intSortNumber == 0)
                    {
                        strTendersFirst = ShareClass.ObjectToString(drPurchaseOfferRecord["Tenders"]);
                    }

                    intSortNumber += 1;

                    strTenders = ShareClass.ObjectToString(drPurchaseOfferRecord["Tenders"]);

                    if (strTendersFirst != strTenders)
                    {
                        j = 1;
                    }

                    if (decimalApplyMoney < 0)
                    {
                        k = 1;
                    }
                }

                if (j == 1)
                {
                    if (k == 1)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCaiGouWenJianBaoJiaMingXiBiX").ToString().Trim()+"')", true);
                        return;
                    }
                }
                else
                {
                    if (k == 1)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCaiGouWenJianBaoJiaMingXiBiX").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJWBJMXBNTJ").ToString().Trim() + "')", true);
                return;
            }

            string strPurchaseCode = ShareClass.ObjectToString(drPurchase["PurchaseCode"]);
            string strSupplierCode = strUserCode;//ShareClass.ObjectToString(drPurchase["SupplierCode"]);
            string strUpdatePurchaseSupplierSQL = string.Format("update T_WZPurchaseOfferRecord set Progress = '报价' where PurchaseCode = '{0}' and SupplierCode = '{1}'", strPurchaseCode, strSupplierCode); 

            ShareClass.RunSqlCommand(strUpdatePurchaseSupplierSQL);

            BT_NewSubmit.Enabled = false;
            BT_NewSubmitReturn.Enabled = true;

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);
        }
    }


    protected void BT_NewSubmitReturn_Click(object sender, EventArgs e)
    {
        //提交退回
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        string strPurchaseHQL = string.Format(@"select distinct p.*,s.DocumentName,s.DocumentURL,s.IsConfirm,s.ID as PurchaseSupplierID from T_WZPurchase p
                        left join T_WZPurchaseSupplier s on p.PurchaseCode = s.PurchaseCode
                        where s.SupplierCode = '{1}'
                        and p.PurchaseCode = '{0}'", strEditPurchaseCode, strUserCode);

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
        if (dtPurchase != null && dtPurchase.Rows.Count > 0)
        {
            DataRow drPurchase = dtPurchase.Rows[0];

            DateTime dtPurchaseEndTime = DateTime.Now;
            DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseEndTime"]), out dtPurchaseEndTime);
            //if (dtPurchaseEndTime < DateTime.Now)
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJJZRYGBNZTH").ToString().Trim() + "')", true);
            //    return;
            //}
            DateTime dtPurchaseStartTime = DateTime.Now;
            DateTime.TryParse(ShareClass.ObjectToString(drPurchase["PurchaseStartTime"]), out dtPurchaseStartTime);
            //if (dtPurchaseStartTime > DateTime.Now)
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBJKSRWDBNTH").ToString().Trim() + "')", true);
            //    return;
            //}
            if (ShareClass.ObjectToString(drPurchase["Progress"]) != LanguageHandle.GetWord("XunJia").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGWJJDBSJBNZTH").ToString().Trim() + "')", true);
                return;
            }

            string strPurchaseCode = ShareClass.ObjectToString(drPurchase["PurchaseCode"]);
            string strSupplierCode = strUserCode;// ShareClass.ObjectToString(drPurchase["SupplierCode"]);
            string strUpdatePurchaseSupplierSQL = string.Format("update T_WZPurchaseOfferRecord set Progress = '询价' where PurchaseCode = '{0}' and SupplierCode = '{1}'", strPurchaseCode, strSupplierCode); 

            ShareClass.RunSqlCommand(strUpdatePurchaseSupplierSQL);


            BT_NewSubmit.Enabled = true;
            BT_NewSubmitReturn.Enabled = false;

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTHCG").ToString().Trim() + "')", true);
        }
    }

    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        Response.Redirect("TTWZPurchaseApply.aspx");
    }

    private void ControlStatusChange(string objSupplierCode, string objProgress, string strPurchaseEndTime, string strPurchaseCode)
    {
        DateTime dtPurchaseEndTime = DateTime.Now;
        DateTime.TryParse(strPurchaseEndTime, out dtPurchaseEndTime);

        if (objSupplierCode.ToUpper() == strUserCode.ToUpper() && objProgress == LanguageHandle.GetWord("XunJia").ToString().Trim() && int.Parse(dtPurchaseEndTime.ToString("yyyyMMdd")) <= int.Parse(DateTime.Now.ToString("yyyyMMdd")))
        {
            BT_NewTenderDocument.Enabled = true;
            BT_NewThrowDocument.Enabled = true;
            BT_NewThrowDocumentDel.Enabled = true;
            BT_NewApplyDetail.Enabled = true;
        }
        else
        {
            BT_NewTenderDocument.Enabled = false;
            BT_NewThrowDocument.Enabled = false;
            BT_NewThrowDocumentDel.Enabled = false;
            BT_NewApplyDetail.Enabled = false;
        }

        if (objSupplierCode.ToUpper() == strUserCode.ToUpper() && objProgress == LanguageHandle.GetWord("XunJia").ToString().Trim())
        {
            BT_NewTenderDocument.Enabled = true;
            BT_NewSubmit.Enabled = true;
        }
        else
        {
            BT_NewSubmit.Enabled = false;
            BT_NewTenderDocument.Enabled = false;
        }


        //报价单〈采购编号〉＝选定采购文件〈采购编号〉												
        //报价单〈供方编号〉＝登录用户的供方〈供方编号〉												
        //报价单所有记录的〈进度〉＝“报价”												

        string strSupplierApplyDetailSQL = string.Format(@"select * from T_WZPurchaseOfferRecord
                                where SupplierCode = '{0}'
                                and PurchaseCode = '{1}'", strUserCode, strPurchaseCode);
        DataTable dtSupplierApplyDetail = ShareClass.GetDataSetFromSql(strSupplierApplyDetailSQL, "PurchaseOfferRecord").Tables[0];

        bool isBool = true;

        if (dtSupplierApplyDetail != null && dtSupplierApplyDetail.Rows.Count > 0)
        {
            foreach (DataRow drPurchaseOfferRecord in dtSupplierApplyDetail.Rows)
            {
                if (ShareClass.ObjectToString(drPurchaseOfferRecord["Progress"]) != LanguageHandle.GetWord("BaoJia").ToString().Trim())
                {
                    isBool = false;
                }

            }
        }

        if (objSupplierCode.ToUpper() == strUserCode.ToUpper() && isBool)
        {
            BT_NewSubmitReturn.Enabled = true;
        }
        else
        {
            BT_NewSubmitReturn.Enabled = false;
        }
    }

    private void ControlStatusCloseChange()
    {
        BT_NewTenderDocument.Enabled = false;
        BT_NewThrowDocument.Enabled = false;
        BT_NewThrowDocumentDel.Enabled = false;
        BT_NewApplyDetail.Enabled = false;
        BT_NewSubmit.Enabled = false;
    }
}
