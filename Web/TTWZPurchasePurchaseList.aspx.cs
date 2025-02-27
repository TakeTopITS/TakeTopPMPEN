using System; using System.Resources;
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
using System.Text;

public partial class TTWZPurchasePurchaseList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();

        }
    }

    private void DataBinder()
    {
        string strPurchaseHQL = string.Format(@"select p.*,e.UserName as PurchaseEngineerName,
                    u.UserName as UpLeaderName,
                    m.UserName as PurchaseManagerName
                    from T_WZPurchase p
                    left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                    left join T_ProjectMember u on p.UpLeader = u.UserCode
                    left join T_ProjectMember m on p.PurchaseManager = m.UserCode 
                    where p.Progress in ('Approved','询价','评标','报价') 
                    and p.PurchaseEngineer = '{0}' 
                    order by p.MarkTime desc", strUserCode); 
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();
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
            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
            string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + cmdArges + "'";
            IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
            if (listPurchase != null && listPurchase.Count == 1)
            {
                WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                if (cmdName == "edit")
                {
                    string strPurchaseHQL = string.Format(@"select p.*,m.UserName as UpLeaderName from T_WZPurchase p
                            left join T_ProjectMember m on p.UpLeader = m.UserCode
                             where p.PurchaseCode = '{0}'", cmdArges);

                    DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
                    if (dtPurchase != null && dtPurchase.Rows.Count > 0)
                    {
                        DataRow drPurchase = dtPurchase.Rows[0];


                        //加载报价文件
                        HF_PurchaseCode.Value = cmdArges;
                        TXT_PurchaseCode.Text = cmdArges;

                        DataPurchaseDocumentBinder(cmdArges);
                    }
                }
                else if (cmdName == "assess")
                {
                    //评标
                    if (wZPurchase.Progress != LanguageHandle.GetWord("XunJia").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJJDBSJBNPB").ToString().Trim()+"')", true);
                        return;
                    }


                    wZPurchase.Progress = LanguageHandle.GetWord("PingBiao").ToString().Trim();

                    wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                    DataBinder();


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPBCG").ToString().Trim()+"')", true);
                }
                else if (cmdName == "apply")
                {
                    //报价完成
                    //if (wZPurchase.Progress != LanguageHandle.GetWord("PingBiao").ToString().Trim())
                    //{
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJJDBSPBBNBJ").ToString().Trim()+"')", true);
                    //    return;
                    //}


                    string strCheckDecisionHQL = string.Format(@"select * from T_WZPurchaseDecision where PurchaseCode = '{0}'", wZPurchase.PurchaseCode);
                    DataTable dtCheckDecision = ShareClass.GetDataSetFromSql(strCheckDecisionHQL, "CheckDecision").Tables[0];
                    if (dtCheckDecision == null || dtCheckDecision.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWZGYSXGJZJYJZGYS").ToString().Trim()+"')", true);
                        return;
                    }

                    wZPurchase.Progress = LanguageHandle.GetWord("BaoJia").ToString().Trim();

                    wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJCG").ToString().Trim()+"')", true);
                }
            }
        }
    }



    protected void BT_Upload_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value.Trim();
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                //判断是否已经报价了
                WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
                if (listPurchase != null && listPurchase.Count == 1)
                {
                    WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                    if (wZPurchase.Progress.Trim() != LanguageHandle.GetWord("PingBiao").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJYJBJWCBNZSCBJWJ").ToString().Trim()+"')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBDCGWJLXGLY").ToString().Trim()+"')", true);
                    return;
                }

                string strPurchaseOfferDocument = FUP_PurchaseOfferDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
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

                    FUP_PurchaseOfferDocument.SaveAs(strDocSavePath + strFileName3);

                    //写入到采购报价文件表中
                    WZPurchaseDocumentBLL wZPurchaseDocumentBLL = new WZPurchaseDocumentBLL();
                    WZPurchaseDocument wZPurchaseDocument = new WZPurchaseDocument();
                    wZPurchaseDocument.PurchaseCode = strPurchaseCode;
                    wZPurchaseDocument.DocumentName = strFileName2;
                    wZPurchaseDocument.DocumentURL = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    wZPurchaseDocumentBLL.AddWZPurchaseDocument(wZPurchaseDocument);

                    //加载报价文件
                    DataPurchaseDocumentBinder(strPurchaseCode);

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCBJWJCG").ToString().Trim()+"')", true);
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXBCCGWJZBJBJWJ").ToString().Trim()+"')", true);
            return;
        }
    }


    protected void RPT_PurchaseDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value.Trim();

        //判断是否已经报价了
        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            if (wZPurchase.Progress.Trim() != LanguageHandle.GetWord("PingBiao").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJYJBJWCBNZSCBJWJ").ToString().Trim()+"')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBDCGWJLXGLY").ToString().Trim()+"')", true);
            return;
        }

        string strCommandName = e.CommandName;
        if (strCommandName.Equals("del"))
        {
            string strCommandArgument = e.CommandArgument.ToString();

            int intPurchaseDocumentID = 0;
            int.TryParse(strCommandArgument, out intPurchaseDocumentID);

            WZPurchaseDocumentBLL wZPurchaseDocumentBLL = new WZPurchaseDocumentBLL();
            string strPurchaseDocumentHQL = "from WZPurchaseDocument as wZPurchaseDocument where ID = " + intPurchaseDocumentID;
            IList lstPurchaseDocument = wZPurchaseDocumentBLL.GetAllWZPurchaseDocuments(strPurchaseDocumentHQL);
            if (lstPurchaseDocument != null && lstPurchaseDocument.Count > 0)
            {
                WZPurchaseDocument wZPurchaseDocument = (WZPurchaseDocument)lstPurchaseDocument[0];

                wZPurchaseDocumentBLL.DeleteWZPurchaseDocument(wZPurchaseDocument);

                DataPurchaseDocumentBinder(HF_PurchaseCode.Value);
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

    protected void BT_Supplier_Click(object sender, EventArgs e)
    {
        string strSupplier = HF_Supplier.Value;
        if (!string.IsNullOrEmpty(strSupplier))
        {
            string[] arrSupplier = strSupplier.Split(',');
            string strPurchaseCode = HF_PurchaseCode.Value.Trim();

            //判断是否已经报价了
            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
            string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
            IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
            if (listPurchase != null && listPurchase.Count == 1)
            {
                WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                if (wZPurchase.Progress.Trim() != LanguageHandle.GetWord("PingBiao").ToString().Trim())
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCGWJYJBJWCBNZGYS").ToString().Trim()+"')", true);
                    return;
                }
            }
            else {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBDCGWJLXGLY").ToString().Trim()+"')", true);
                return;
            }

            string strCheckPurchaseDecisionHQL = string.Format(@"select * from T_WZPurchaseDecision where PurchaseCode = '{0}'", strPurchaseCode);
            DataTable dtPurchaseDecision = ShareClass.GetDataSetFromSql(strCheckPurchaseDecisionHQL, "PurchaseDecision").Tables[0];
            if (dtPurchaseDecision != null && dtPurchaseDecision.Rows.Count > 0)
            {
                string strID = ShareClass.ObjectToString(dtPurchaseDecision.Rows[0]["ID"]);
                //修改
                string strUpdatePurchaseDecisionHQL = string.Format("update T_WZPurchaseDecision set SupplierCode = '{0}' where ID = {1}", arrSupplier[0], strID);

                ShareClass.RunSqlCommand(strUpdatePurchaseDecisionHQL);
            }
            else { 
                //增加
                string strInsertPurchaseDecisionHQL = string.Format(@"insert into T_WZPurchaseDecision(PurchaseCode,SupplierCode)
                                                values('{0}','{1}')", strPurchaseCode, arrSupplier[0]);
                ShareClass.RunSqlCommand(strInsertPurchaseDecisionHQL);
            }

            //修改采购清单里面的供应商编号
            string strUpdatePurchaseDetailHQL = string.Format(@"update T_WZPurchaseDetail
                                                set SupplierCode = '{1}'
                                                where PurchaseCode = '{0}'", strPurchaseCode, arrSupplier[0]);
            ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

            TXT_SupplierName.Text = arrSupplier[1];

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZGYSCG").ToString().Trim()+"')", true);
        }
    }
}
