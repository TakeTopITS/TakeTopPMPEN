using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPurchaseEdit : System.Web.UI.Page
{
    string strUserCode;


    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataProjectBinder();

            if (!string.IsNullOrEmpty(Request.QueryString["PurchaseCode"]))
            {
                string strPurchaseCode = Request.QueryString["PurchaseCode"];
                HF_PurchaseCode.Value = strPurchaseCode;

                DataBinder(strPurchaseCode);
            }
        }
    }

    private void DataBinder(string strPurchaseCode)
    {
        string strPurchaseHQL = string.Format(@"select p.*,m.UserName as UpLeaderName from T_WZPurchase p
                            left join T_ProjectMember m on p.UpLeader = m.UserCode
                             where p.PurchaseCode = '{0}'", strPurchaseCode);

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];
        if (dtPurchase != null && dtPurchase.Rows.Count > 0)
        {
            DataRow drPurchase = dtPurchase.Rows[0];

            TXT_PurchaseName.Text = ShareClass.ObjectToString(drPurchase["PurchaseName"]);
            DDL_Project.SelectedValue = ShareClass.ObjectToString(drPurchase["ProjectCode"]);
            DDL_PurchaseMethod.SelectedValue = ShareClass.ObjectToString(drPurchase["PurchaseMethod"]);
            TXT_PurchaseEndTime.Text = ShareClass.ObjectToString(drPurchase["PurchaseEndTime"]);
            LT_PurchaseDocument.Text = "<a href='" + ShareClass.ObjectToString(drPurchase["PurchaseDocumentURL"]) + "'>" + ShareClass.ObjectToString(drPurchase["PurchaseDocument"]) + "</a>";
            TXT_UpLeader.Text = ShareClass.ObjectToString(drPurchase["UpLeaderName"]);
            HF_UpLeader.Value = ShareClass.ObjectToString(drPurchase["UpLeader"]);

            //加载供应商
            //加载专家组
            //加载报价文件
            LB_PurchaseCode.Text = strPurchaseCode;

            DataPurchaseDocumentBinder(strPurchaseCode);
            DataPurchaseSupplierBinder(strPurchaseCode);
            DataPurchaseExpertBinder(strPurchaseCode);
        }
        #region 注释
        //string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
        //WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        //IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
        //if (listPurchase != null && listPurchase.Count > 0)
        //{
        //    WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
        //    TXT_PurchaseName.Text = wZPurchase.PurchaseName;
        //    DDL_Project.SelectedValue = wZPurchase.ProjectCode;
        //    DDL_PurchaseMethod.SelectedValue = wZPurchase.PurchaseMethod;
        //    TXT_PurchaseEndTime.Text = wZPurchase.PurchaseEndTime.ToString();
        //    LT_PurchaseDocument.Text = "<a href='" + wZPurchase.PurchaseDocumentURL + "'>" + wZPurchase.PurchaseDocument + "</a>";
        //    TXT_UpLeader.Text = wZPurchase.UpLeader;

        //    //加载供应商
        //    //加载专家组
        //    //加载报价文件
        //    DataPurchaseDocumentBinder(strPurchaseCode);
        //    DataPurchaseSupplierBinder(strPurchaseCode);
        //    DataPurchaseExpertBinder(strPurchaseCode);
        //}
        #endregion
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strPurchaseName = TXT_PurchaseName.Text.Trim();
            string strProjectCode = DDL_Project.SelectedValue;
            string strPurchaseMethod = DDL_PurchaseMethod.SelectedValue;
            string strPurchaseEndTime = TXT_PurchaseEndTime.Text.Trim();
            DateTime dtPurchaseEndTime = DateTime.Now;
            DateTime.TryParse(strPurchaseEndTime, out dtPurchaseEndTime);
            string strUpLeader = HF_UpLeader.Value; //TXT_UpLeader.Text.Trim();

            if (string.IsNullOrEmpty(strPurchaseName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWJMCBNWKBC").ToString().Trim() + "')", true);
                return;
            }
            //if (string.IsNullOrEmpty(strProjectCode))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMBMBNWKBC").ToString().Trim()+"')", true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(strPurchaseMethod))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGYFSBNWKBC").ToString().Trim()+"')", true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(strPurchaseEndTime))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJJZRBNWKBC").ToString().Trim()+"')", true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(strUpLeader))
            //{
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSJLDBNWKBC").ToString().Trim()+"')", true);
            //    return;
            //}

            WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();

            string strPurchaseDocumentURL = HF_PurchaseDocumentURL.Value;
            string strPurchaseDocumentName = HF_PurchaseDocument.Value;

            WZPurchase wZPurchase = new WZPurchase();
            string strPurchaseManager = HF_PurchaseManager.Value;
            string strPurchaseEngineer = HF_PurchaseEngineer.Value;
            string strDecision = HF_Decision.Value;

            if (!string.IsNullOrEmpty(HF_PurchaseCode.Value))
            {
                string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + HF_PurchaseCode.Value + "'";
                IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                if (listPurchase != null && listPurchase.Count > 0)
                {
                    wZPurchase = (WZPurchase)listPurchase[0];

                    //修改
                    wZPurchase.PurchaseName = strPurchaseName;
                    wZPurchase.ProjectCode = strProjectCode;
                    wZPurchase.PurchaseMethod = strPurchaseMethod;
                    wZPurchase.PurchaseEndTime = dtPurchaseEndTime.ToString();
                    if (!string.IsNullOrEmpty(strPurchaseDocumentName) && !string.IsNullOrEmpty(strPurchaseDocumentURL))
                    {
                        wZPurchase.PurchaseDocument = strPurchaseDocumentName;
                        wZPurchase.PurchaseDocumentURL = strPurchaseDocumentURL;
                    }
                    wZPurchase.UpLeader = strUpLeader;

                    wZPurchaseBLL.UpdateWZPurchase(wZPurchase, HF_PurchaseCode.Value);

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
            }
            else
            {
                //增加
                //采购编号
                string strNewPurchaseCode = CreateNewPurchaseCode();

                wZPurchase.PurchaseCode = strNewPurchaseCode;
                wZPurchase.PurchaseName = strPurchaseName;
                wZPurchase.ProjectCode = strProjectCode;
                wZPurchase.PurchaseMethod = strPurchaseMethod;
                wZPurchase.PurchaseEndTime = dtPurchaseEndTime.ToString();
                wZPurchase.PurchaseDocument = strPurchaseDocumentName;
                wZPurchase.PurchaseDocumentURL = strPurchaseDocumentURL;
                wZPurchase.MarkTime = DateTime.Now;
                wZPurchase.PurchaseEngineer = strPurchaseEngineer;
                wZPurchase.UpLeader = strUpLeader;
                wZPurchase.PurchaseManager = strPurchaseManager;
                //wZPurchase.PurchaseStartTime = DateTime.Now;
                wZPurchase.Decision = strDecision;
                //wZPurchase.DecisionTime = DateTime.Now;
                wZPurchase.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();

                wZPurchaseBLL.AddWZPurchase(wZPurchase);

                HF_PurchaseCode.Value = strNewPurchaseCode;
                LB_PurchaseCode.Text = strNewPurchaseCode;
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click1", "LoadParentLit();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    private void DataProjectBinder()
    {
        string strProjectHQL = string.Format(@"select p.*,
                    pp.UserName as ProjectManagerName,
                    pd.UserName as DelegateAgentName,
                    pm.UserName as PurchaseManagerName,
                    pe.UserName as PurchaseEngineerName,
                    pc.UserName as ContracterName,
                    pk.UserName as CheckerName,
                    ps.UserName as SafekeepName,
                    pa.UserName as MarkerName,
                    pu.UserName as SupplementEditorName
                    from T_WZProject p
                    left join T_ProjectMember pp on p.ProjectManager = pp.UserCode
                    left join T_ProjectMember pd on p.DelegateAgent = pd.UserCode
                    left join T_ProjectMember pm on p.PurchaseManager = pm.UserCode
                    left join T_ProjectMember pe on p.PurchaseEngineer = pe.UserCode
                    left join T_ProjectMember pc on p.Contracter = pc.UserCode
                    left join T_ProjectMember pk on p.Checker = pk.UserCode
                    left join T_ProjectMember ps on p.Safekeep = ps.UserCode
                    left join T_ProjectMember pa on p.Marker = pa.UserCode
                    left join T_ProjectMember pu on p.SupplementEditor = pu.UserCode
                    where p.Progress = '开工'                                     
                
                    and (p.PurchaseEngineer = '{0}' or p.PurchaseEngineer = '-')
                    and ProjectCode not in 
                    (
                    select ProjectCode from T_Project
                    where Status in ('Deleted')
                    )", strUserCode); 


        strProjectHQL += " order by p.MarkTime desc";

        DataTable dtProject = ShareClass.GetDataSetFromSql(strProjectHQL, "Project").Tables[0];

        DDL_Project.DataSource = dtProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void DDL_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectSelectedValue = DDL_Project.SelectedItem.Text;
        if (!string.IsNullOrEmpty(strProjectSelectedValue))
        {
            WZProjectBLL wZProjectBLL = new WZProjectBLL();
            string strProjectHQL = "from WZProject as wZProject where ProjectCode = '" + strProjectSelectedValue + "'";
            IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
            if (listProject != null && listProject.Count > 0)
            {
                WZProject wZProject = (WZProject)listProject[0];

                HF_PurchaseManager.Value = wZProject.PurchaseManager;
                HF_PurchaseEngineer.Value = wZProject.PurchaseEngineer;
                HF_Decision.Value = wZProject.DelegateAgent;
            }
        }
    }

    protected void BT_PurchaseFile_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
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


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_PurchaseDocument.Text = "<a href=\"" + strUrl + "\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                    string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                    IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                    if (listPurchase != null && listPurchase.Count > 0)
                    {
                        WZPurchase wZPurchase = (WZPurchase)listPurchase[0];
                        wZPurchase.PurchaseDocument = Path.GetFileNameWithoutExtension(strFileName2);
                        wZPurchase.PurchaseDocumentURL = strUrl;

                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);
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

                    LT_PurchaseDocument.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_PurchaseDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_PurchaseDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCGWJCG").ToString().Trim() + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZYSCDWJ").ToString().Trim() + "')", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }

    protected void BT_Upload_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
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
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim() + "');</script>");
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXBCCGWJZBJBJWJ").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void RPT_PurchaseDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
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



    protected void RPT_PurchaseSupplier_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string strCommandName = e.CommandName;
        if (strCommandName.Equals("del"))
        {
            string strCommandArgument = e.CommandArgument.ToString();

            int intPurchaseSupplierID = 0;
            int.TryParse(strCommandArgument, out intPurchaseSupplierID);

            WZPurchaseSupplierBLL wZPurchaseSupplierBLL = new WZPurchaseSupplierBLL();
            string strPurchaseSupplierHQL = "from WZPurchaseSupplier as wZPurchaseSupplier where ID = " + intPurchaseSupplierID;
            IList lstPurchaseSupplier = wZPurchaseSupplierBLL.GetAllWZPurchaseSuppliers(strPurchaseSupplierHQL);
            if (lstPurchaseSupplier != null && lstPurchaseSupplier.Count > 0)
            {
                WZPurchaseSupplier wZPurchaseSupplier = (WZPurchaseSupplier)lstPurchaseSupplier[0];

                wZPurchaseSupplierBLL.DeleteWZPurchaseSupplier(wZPurchaseSupplier);
            }
        }
    }


    private void DataPurchaseSupplierBinder(string strPurchaseCode)
    {
        WZPurchaseSupplierBLL wZPurchaseSupplierBLL = new WZPurchaseSupplierBLL();
        string strPurchaseSupplierHQL = "from WZPurchaseSupplier as wZPurchaseSupplier where PurchaseCode = '" + strPurchaseCode + "'";
        IList lstPurchaseSupplier = wZPurchaseSupplierBLL.GetAllWZPurchaseSuppliers(strPurchaseSupplierHQL);

        RPT_PurchaseSupplier.DataSource = lstPurchaseSupplier;
        RPT_PurchaseSupplier.DataBind();
    }



    protected void RPT_PurchaseExpert_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string strCommandName = e.CommandName;
        if (strCommandName.Equals("del"))
        {
            string strCommandArgument = e.CommandArgument.ToString();

            int intPurchaseExpertID = 0;
            int.TryParse(strCommandArgument, out intPurchaseExpertID);

            WZPurchaseExpertBLL wZPurchaseExpertBLL = new WZPurchaseExpertBLL();
            string strPurchaseExpertHQL = "from WZPurchaseExpert as wZPurchaseExpert where ID = " + intPurchaseExpertID;
            IList lstPurchaseExpert = wZPurchaseExpertBLL.GetAllWZPurchaseExperts(strPurchaseExpertHQL);
            if (lstPurchaseExpert != null && lstPurchaseExpert.Count > 0)
            {
                WZPurchaseExpert wZPurchaseExpert = (WZPurchaseExpert)lstPurchaseExpert[0];

                wZPurchaseExpertBLL.DeleteWZPurchaseExpert(wZPurchaseExpert);
            }
        }
    }


    private void DataPurchaseExpertBinder(string strPurchaseCode)
    {
        WZPurchaseExpertBLL wZPurchaseExpertBLL = new WZPurchaseExpertBLL();
        string strPurchaseExpertHQL = "from WZPurchaseExpert as wZPurchaseExpert where PurchaseCode = '" + strPurchaseCode + "'";
        IList lstPurchaseExpert = wZPurchaseExpertBLL.GetAllWZPurchaseExperts(strPurchaseExpertHQL);

        RPT_PurchaseExpert.DataSource = lstPurchaseExpert;
        RPT_PurchaseExpert.DataBind();
    }

    protected void BT_SuppierSystem_Click(object sender, EventArgs e)
    {
        //自动选择供应商
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                //查询出采购清单里面的大类代码数组
                string strPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode = '" + strPurchaseCode + "'";
                WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                IList lstPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strPurchaseDetailHQL);
                if (lstPurchaseDetail != null && lstPurchaseDetail.Count > 0)
                {
                    string strPurchaseDetails = string.Empty;
                    for (int i = 0; i < lstPurchaseDetail.Count; i++)
                    {
                        WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)lstPurchaseDetail[0];
                        string strObjectCode = wZPurchaseDetail.ObjectCode;
                        strPurchaseDetails += " and MainSupplier like '%" + strObjectCode.Substring(0, 2) + "%'";
                    }


                    string strSupplierHQL = string.Format(@"select *  from T_WZSupplier 
                                where Grade in ('Qualified','临时')
                                and ReviewDate::timestamp  > now()  {0} order by random() limit 6", strPurchaseDetails); 
                    DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierHQL, "Supplier").Tables[0];
                    if (dtSupplier != null && dtSupplier.Rows.Count > 0)
                    {
                        WZPurchaseSupplierBLL wZPurchaseSupplierBLL = new WZPurchaseSupplierBLL();
                        //写入采购文件供应商
                        foreach (DataRow dr in dtSupplier.Rows)
                        {
                            WZPurchaseSupplier wZPurchaseSupplier = new WZPurchaseSupplier();
                            wZPurchaseSupplier.PurchaseCode = strPurchaseCode;
                            string strSupplierCode = dr["SupplierCode"].ToString();
                            wZPurchaseSupplier.SupplierCode = strSupplierCode;
                            wZPurchaseSupplier.SupplierName = dr["SupplierName"].ToString();

                            wZPurchaseSupplierBLL.AddWZPurchaseSupplier(wZPurchaseSupplier);

                            //修改供应商使用标记
                            string strUpdateSupplierHQL = "update T_WZSupplier set IsMark = -1 where SupplierCode = '" + strSupplierCode + "'";
                            ShareClass.RunSqlCommand(strUpdateSupplierHQL);
                        }

                        //加载供应商列表
                        DataPurchaseSupplierBinder(strPurchaseCode);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWZDHSDGYS").ToString().Trim() + "')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXBJCGDZZGYS").ToString().Trim() + "')", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXBCCGWJZBJGYS").ToString().Trim() + "')", true);
            return;
        }
    }


    protected void BT_ExpertSystem_Click(object sender, EventArgs e)
    {
        //自动选择专家
        string strPurchaseCode = HF_PurchaseCode.Value;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            try
            {
                //查询出采购清单里面的专业类别
                string strPurchaseHQL = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
                WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                IList lstPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseHQL);
                if (lstPurchase != null && lstPurchase.Count > 0)
                {
                    WZPurchase wZPurchase = (WZPurchase)lstPurchase[0];

                    string strPurchaseDetailHQL = "from WZPurchaseDetail as wZPurchaseDetail where PurchaseCode = '" + strPurchaseCode + "'";
                    WZPurchaseDetailBLL wZPurchaseDetailBLL = new WZPurchaseDetailBLL();
                    IList lstPurchaseDetail = wZPurchaseDetailBLL.GetAllWZPurchaseDetails(strPurchaseDetailHQL);
                    if (lstPurchaseDetail != null && lstPurchaseDetail.Count > 0)
                    {
                        string strTotalHQL = string.Empty;
                        //价格监审
                        string strMoneyLookHQL = "select * from T_WZExpertDatabase where ExpertType = '价格监审' order by random() limit 1"; 
                        //判断是否需要纪检监督
                        string strInspectHQL = string.Empty;
                        //技术类专家
                        string strComputeHQL = string.Empty;
                        string strTechnologyHQL = string.Empty;
                        //选择专业范围相同的专家
                        string strPurchaseDetails = string.Empty;
                        ArrayList arrayMajor = new ArrayList();
                        for (int i = 0; i < lstPurchaseDetail.Count; i++)
                        {
                            WZPurchaseDetail wZPurchaseDetail = (WZPurchaseDetail)lstPurchaseDetail[0];
                            string strMajorType = wZPurchaseDetail.MajorType;
                            if (!arrayMajor.Contains(strMajorType))
                            {
                                arrayMajor.Add(strMajorType);
                            }

                            strPurchaseDetails += " or ExpertType like '" + strMajorType + "'%";
                        }

                        if (wZPurchase.PlanMoney >= 300000)
                        {
                            strInspectHQL = "select * from T_WZExpertDatabase where ExpertType = '纪检监督' order by random() limit 1"; 
                            if (wZPurchase.PurchaseMethod == LanguageHandle.GetWord("QiaoBiao").ToString().Trim() || wZPurchase.PurchaseMethod == LanguageHandle.GetWord("KuangJia").ToString().Trim())
                            {
                                //最多抽取4个
                                string strExpertCodeS = string.Empty;
                                int intRowNumber = 0;

                                strTechnologyHQL = string.Format("select * from T_WZExpertDatabase where 1=1 and ({1}) order by random() limit 100", strPurchaseDetails);
                                DataTable dtTechnology = ShareClass.GetDataSetFromSql(strTechnologyHQL, "Technology").Tables[0];
                                if (dtTechnology != null && dtTechnology.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtTechnology.Rows)
                                    {
                                        string strExpertExpertType = ShareClass.ObjectToString(dr["ExpertType"]);
                                        ArrayList arrayMajorCopy = new ArrayList();
                                        arrayMajorCopy = (ArrayList)arrayMajor.Clone();
                                        int intChildRowNumber = 0;
                                        for (int j = 0; j < arrayMajorCopy.Count; j++)
                                        {
                                            if (strExpertExpertType.Contains(arrayMajorCopy[j].ToString()))
                                            {
                                                string strCodes = ShareClass.ObjectToString(dr["ExpertCode"]);
                                                if (!strExpertCodeS.Contains(strCodes))
                                                {
                                                    strExpertCodeS += "'" + strCodes + "',";
                                                }
                                                intChildRowNumber++;
                                            }
                                        }
                                        if (intChildRowNumber > 0)
                                        {
                                            intRowNumber++;
                                        }
                                    }
                                }

                                if (intRowNumber > 4)
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGDDZYLBGD6GZJWFMZBDZYLB").ToString().Trim() + "')", true);
                                    return;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(strExpertCodeS))
                                    {
                                        strExpertCodeS = strExpertCodeS.EndsWith(",") ? strExpertCodeS.TrimEnd(',') : strExpertCodeS;
                                        strComputeHQL = string.Format("select * from T_WZExpertDatabase where 1=1 and ExpertCode in ({0})", strExpertCodeS);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //最多抽取5个
                            if (wZPurchase.PurchaseMethod == LanguageHandle.GetWord("QiaoBiao").ToString().Trim() || wZPurchase.PurchaseMethod == LanguageHandle.GetWord("KuangJia").ToString().Trim())
                            {
                                //最多抽取4个
                                string strExpertCodeS = string.Empty;
                                int intRowNumber = 0;

                                strTechnologyHQL = string.Format("select * from T_WZExpertDatabase where 1=1 and ({1}) order by random() limit 100", strPurchaseDetails);
                                DataTable dtTechnology = ShareClass.GetDataSetFromSql(strTechnologyHQL, "Technology").Tables[0];
                                if (dtTechnology != null && dtTechnology.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtTechnology.Rows)
                                    {
                                        string strExpertExpertType = ShareClass.ObjectToString(dr["ExpertType"]);
                                        ArrayList arrayMajorCopy = new ArrayList();
                                        arrayMajorCopy = (ArrayList)arrayMajor.Clone();
                                        int intChildRowNumber = 0;
                                        for (int j = 0; j < arrayMajorCopy.Count; j++)
                                        {
                                            if (strExpertExpertType.Contains(arrayMajorCopy[j].ToString()))
                                            {
                                                string strCodes = ShareClass.ObjectToString(dr["ExpertCode"]);
                                                if (!strExpertCodeS.Contains(strCodes))
                                                {
                                                    strExpertCodeS += "'" + strCodes + "',";
                                                }
                                                intChildRowNumber++;
                                            }
                                        }
                                        if (intChildRowNumber > 0)
                                        {
                                            intRowNumber++;
                                        }
                                    }
                                }

                                if (intRowNumber > 5)
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGDDZYLBGD6GZJWFMZBDZYLB").ToString().Trim() + "')", true);
                                    return;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(strExpertCodeS))
                                    {
                                        strExpertCodeS = strExpertCodeS.EndsWith(",") ? strExpertCodeS.TrimEnd(',') : strExpertCodeS;
                                        strComputeHQL = string.Format("select * from T_WZExpertDatabase where 1=1 and ExpertCode in ({0})", strExpertCodeS);
                                    }
                                }
                            }
                        }

                        string strExpertHQL = strMoneyLookHQL;
                        if (!string.IsNullOrEmpty(strInspectHQL))
                        {
                            strExpertHQL += " union all " + strInspectHQL;
                        }
                        strExpertHQL += " union all " + strComputeHQL;
                        DataTable dtExpert = ShareClass.GetDataSetFromSql(strExpertHQL, "Expert").Tables[0];
                        if (dtExpert != null && dtExpert.Rows.Count > 0)
                        {
                            WZPurchaseExpertBLL wZPurchaseExpertBLL = new WZPurchaseExpertBLL();
                            //写入采购文件供应商
                            foreach (DataRow dr in dtExpert.Rows)
                            {
                                WZPurchaseExpert wZPurchaseExpert = new WZPurchaseExpert();
                                wZPurchaseExpert.PurchaseCode = strPurchaseCode;
                                string strExpertCode = ShareClass.ObjectToString(dr["ExpertCode"]);
                                wZPurchaseExpert.ExpertCode = strExpertCode;
                                wZPurchaseExpert.ExpertName = ShareClass.ObjectToString(dr["Name"]);

                                wZPurchaseExpertBLL.AddWZPurchaseExpert(wZPurchaseExpert);

                            }

                            //加载专家列表
                            DataPurchaseExpertBinder(strPurchaseCode);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWZDHSDGYS").ToString().Trim() + "')", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXBJCGDZZGYS").ToString().Trim() + "')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGBHBCZ").ToString().Trim() + "')", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXBCCGWJZBJCGWJ").ToString().Trim() + "')", true);
            return;
        }
    }


    /// <summary>
    ///  生成采购单Code
    /// </summary>
    private string CreateNewPurchaseCode()
    {
        string strNewPurchaseCode = string.Empty;
        string strYearMonthString = DateTime.Now.ToString("yyyyMM");
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strPurchaseCodeHQL = "select count(1) as RowNumber from T_WZPurchase  Where PurchaseCode Like " + "'" + strYearMonthString + "%" + "'";
                DataTable dtPurchaseCode = ShareClass.GetDataSetFromSql(strPurchaseCodeHQL, "PurchaseCode").Tables[0];
                int intPurchaseCodeNumber = int.Parse(dtPurchaseCode.Rows[0]["RowNumber"].ToString());
                intPurchaseCodeNumber = intPurchaseCodeNumber + 1;
                do
                {
                    StringBuilder sbPurchaseCode = new StringBuilder();
                    for (int j = 4 - intPurchaseCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbPurchaseCode.Append("0");
                    }
                    strNewPurchaseCode = strYearMonthString + sbPurchaseCode.ToString() + intPurchaseCodeNumber.ToString();

                    //验证新的采购编号是否存在
                    string strCheckNewPurchaseCodeHQL = "select count(1) as RowNumber from T_WZPurchase where PurchaseCode = '" + strNewPurchaseCode + "'";
                    DataTable dtCheckNewPurchaseCode = ShareClass.GetDataSetFromSql(strCheckNewPurchaseCodeHQL, "CheckNewPurchaseCode").Tables[0];
                    int intCheckNewPurchaseCode = int.Parse(dtCheckNewPurchaseCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewPurchaseCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intPurchaseCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewPurchaseCode;
    }
}
