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

public partial class TTWZSupplierReview : System.Web.UI.Page
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
        DG_List.CurrentPageIndex = 0;

        string strSupplierHQL = string.Format(@"select s.*,m.UserName as AuditorName,q.UserName as QualityEngineerName,p.UserName as PushPersonName,
                    a.UserName as CompetentMaterialsName,
                    c.UserName as ContractWhoseName,
                    l.UserName as CompetentLeadershipName
                    from T_WZSupplier s
                    left join T_ProjectMember m on s.Auditor = m.UserCode 
                    left join T_ProjectMember p on s.PushPerson = p.UserCode 
                    left join T_ProjectMember q on s.QualityEngineer = q.UserCode
                    left join T_ProjectMember a on s.CompetentMaterials = a.UserCode
                    left join T_ProjectMember c on s.ContractWhose = c.UserCode
                    left join T_ProjectMember l on s.CompetentLeadership = l.UserCode
                    where s.Progress in('Approved','Registration','Review')
                    and s.Auditor = '{0}'
                    and now()::date- s.ReviewDate::timestamp::date <= {1}", strUserCode, 0);   //ChineseWord

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strSupplierHQL += " and s.Progress = '" + strProgress + "'";
        }

        strSupplierHQL += " order by s.ReviewDate desc";

        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierHQL, "Supplier").Tables[0];

        DG_List.DataSource = dtSupplier;
        DG_List.DataBind();

        LB_Sql.Text = strSupplierHQL;

        LB_Record.Text = dtSupplier.Rows.Count.ToString();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_List.Items.Count; i++)
            {
                DG_List.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditID = arrOperate[0].Trim();
                string strProgress = arrOperate[1].Trim();
                string strAuditor = arrOperate[2].Trim();

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "','" + strAuditor + "','" + strUserCode + "');", true);
                ControlStatusChange(strProgress, strAuditor, strUserCode);

                HF_NewID.Value = strEditID;
                HF_NewProgress.Value = strProgress;
                HF_NewAuditor.Value = strAuditor;
            //}
            //else if (cmdName == "approve")
            //{
            //    string cmdArges = e.CommandArgument.ToString();
                WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
                string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strEditID;
                IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
                if (supplierList != null && supplierList.Count == 1)
                {
                    WZSupplier wZSupplier = (WZSupplier)supplierList[0];

                    //if (wZSupplier.Progress != LanguageHandle.GetWord("DengJi").ToString().Trim())
                    //{
                    //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGYSJDBWPZBNFS").ToString().Trim()+"')", true);
                    //    return;
                    //}

                    HF_ID.Value = wZSupplier.ID.ToString();
                    HF_SupplierCode.Value = wZSupplier.SupplierCode;
                    TXT_SupplierNumber.Text = wZSupplier.SupplierNumber;
                    TXT_SupplierName.Text = wZSupplier.SupplierName;
                    DDL_ReviewResult.SelectedValue = wZSupplier.ReviewResult;

                    DDL_Progress.SelectedValue = wZSupplier.Progress;

                    string strReviewDocument = wZSupplier.ReviewDocument;
                    string strReviewDocumentURL = wZSupplier.ReviewDocumentURL;
                    HF_ReviewDocument.Value = strReviewDocument;
                    HF_ReviewDocumentURL.Value = strReviewDocumentURL;
                    LT_ReviewDocument.Text = "<a href=\"" + strReviewDocumentURL + "\" class=\"notTab\" target=\"_blank\">" + strReviewDocument + "</a>";

                    LoadRelatedWL("SupplierManagement", "Supplier", int.Parse(strEditID));
                }
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strHQL, "Supplier").Tables[0];

        DG_List.DataSource = dtSupplier;
        DG_List.DataBind();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;
        if (!string.IsNullOrEmpty(strID))
        {
            string strReviewResult = DDL_ReviewResult.SelectedValue;
            string strReviewDocument = HF_ReviewDocument.Value;
            string strReviewDocumentURL = HF_ReviewDocumentURL.Value;

            string strNewProgress = HF_NewProgress.Value;
            string strNewAuditor = HF_NewAuditor.Value;

            if (string.IsNullOrEmpty(strReviewDocument) || string.IsNullOrEmpty(strReviewDocumentURL))
            {
                //string strProgress = HF_Progress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('复审资料不能为空！');", true);   //ChineseWord
                return;
            }

            if (string.IsNullOrEmpty(strReviewResult))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择复审结论！');", true);   //ChineseWord
                return;
            }

            WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
            string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
            IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
            if (supplierList != null && supplierList.Count == 1)
            {
                WZSupplier wZSupplier = (WZSupplier)supplierList[0];


                string strGrade = wZSupplier.Grade;
                if (strGrade == LanguageHandle.GetWord("LinShi").ToString().Trim() && strReviewResult == LanguageHandle.GetWord("BaoChi").ToString().Trim())
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('临时供应商资格复审，不允许保持！');", true);   //ChineseWord
                    return;
                }

                wZSupplier.ReviewResult = strReviewResult;
                wZSupplier.ReviewDocument = strReviewDocument;
                wZSupplier.ReviewDocumentURL = strReviewDocumentURL;

                //wZSupplier.Progress = LanguageHandle.GetWord("FuShen").ToString().Trim();

                wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

                //重新加载列表
                DataBinder();

                ControlStatusCloseChange();

                //HF_ID.Value = "";
                //HF_SupplierCode.Value = "";
                //TXT_SupplierNumber.Text = "";
                //TXT_SupplierName.Text = "";
                //DDL_ReviewResult.SelectedValue = "";
                //LT_ReviewDocument.Text = "";
                //HF_ReviewDocument.Value = "";
                //HF_ReviewDocumentURL.Value = "";

                //DDL_ReviewResult.BackColor = Color.White;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);   //ChineseWord
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
                ControlStatusCloseChange();
            }
        }
        else
        {
            //string strNewProgress = HF_NewProgress.Value;
            //string strNewAuditor = HF_NewAuditor.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要复审的供应商！');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_ReviewDocument_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;
        if (!string.IsNullOrEmpty(strID))
        {
            try
            {
                string strReviewDocument = FUP_ReviewDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strReviewDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strReviewDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strReviewDocument);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    //string strNewProgress = HF_NewProgress.Value;
                    //string strNewAuditor = HF_NewAuditor.Value;

                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"');</script>");
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_ReviewDocument.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_ReviewDocument.Text = "<a href=\"" + "Doc\\" + strUrl + "\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_ReviewDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_ReviewDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
                    string strSupplierHQL = "from WZSupplier as wZSupplier where ID = " + strID;
                    IList listSupplier = wZSupplierBLL.GetAllWZSuppliers(strSupplierHQL);
                    if (listSupplier != null && listSupplier.Count > 0)
                    {
                        WZSupplier wZSupplier = (WZSupplier)listSupplier[0];
                        wZSupplier.ReviewDocument = Path.GetFileNameWithoutExtension(strFileName2);
                        wZSupplier.ReviewDocumentURL = strUrl;

                        wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);
                    }

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传复审文件成功！');", true);   //ChineseWord
                }
                else
                {
                    //string strNewProgress = HF_NewProgress.Value;
                    //string strNewAuditor = HF_NewAuditor.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);   //ChineseWord
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            try
            {
                string strReviewDocument = FUP_ReviewDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strReviewDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strReviewDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strReviewDocument);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    //string strNewProgress = HF_NewProgress.Value;
                    //string strNewAuditor = HF_NewAuditor.Value;

                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"');</script>");
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_ReviewDocument.SaveAs(strDocSavePath + strFileName3);

                    LT_ReviewDocument.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_ReviewDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_ReviewDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //重新加载报价文件列表

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传复审文件成功！');", true);   //ChineseWord
                }
                else
                {
                    //string strNewProgress = HF_NewProgress.Value;
                    //string strNewAuditor = HF_NewAuditor.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true);   //ChineseWord
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }

    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        HF_SupplierCode.Value = "";
        TXT_SupplierNumber.Text = "";
        TXT_SupplierName.Text = "";
        DDL_ReviewResult.SelectedValue = "";
        LT_ReviewDocument.Text = "";
        HF_ReviewDocument.Value = "";
        HF_ReviewDocumentURL.Value = "";

        DDL_ReviewResult.BackColor = Color.White;

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }

    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strEditID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            HF_ID.Value = wZSupplier.ID.ToString();
            HF_SupplierCode.Value = wZSupplier.SupplierCode;
            TXT_SupplierNumber.Text = wZSupplier.SupplierNumber;
            TXT_SupplierName.Text = wZSupplier.SupplierName;
            DDL_ReviewResult.SelectedValue = wZSupplier.ReviewResult;

            string strReviewDocument = wZSupplier.ReviewDocument;
            string strReviewDocumentURL = wZSupplier.ReviewDocumentURL;
            HF_ReviewDocument.Value = strReviewDocument;
            HF_ReviewDocumentURL.Value = strReviewDocumentURL;
            LT_ReviewDocument.Text = "<a href=\"" + strReviewDocumentURL + "\" class=\"notTab\" target=\"_blank\">" + strReviewDocument + "</a>";


            DDL_ReviewResult.BackColor = Color.CornflowerBlue;

            //string strNewProgress = HF_NewProgress.Value;
            //string strNewAuditor = HF_NewAuditor.Value;
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewAuditor + "','" + strUserCode + "');", true);
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
    }



    protected void BT_NewReview_Click(object sender, EventArgs e)
    {
        //复审
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        string strReviewResult = DDL_ReviewResult.SelectedValue.Trim();
        if(strReviewResult == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSJRBNWKQJC").ToString().Trim() + "')", true);
            return;
        }

        string strReviewDocument = HF_ReviewDocument.Value;
        string strReviewDocumentURL = HF_ReviewDocumentURL.Value;
        string strSupplierName = TXT_SupplierName.Text.Trim();
        if (string.IsNullOrEmpty(strReviewDocument) |  strSupplierName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSXXDBNWKBQXBC").ToString().Trim() + "')", true);
            return;
        }

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strEditID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = LanguageHandle.GetWord("FuShen").ToString().Trim();

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('复审成功！');", true);   //ChineseWord
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
    }


    protected void BT_NewReviewReturn_Click(object sender, EventArgs e)
    {
        //复审退回
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strEditID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = LanguageHandle.GetWord("DengJi").ToString().Trim();

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('复审退回成功！');", true);   //ChineseWord
        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
            ControlStatusCloseChange();
        }
    }




    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //浏览
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        //string strNewProgress = HF_NewProgress.Value;
        //string strNewAuditor = HF_NewAuditor.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZSupplierBrowse.aspx?id=" + strEditID + "');", true);
    }


    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }

    protected int LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType In (" + "'" + strRelatedType + "','Other')" + " and workFlow.RelatedID = " + intRelatedID.ToString();
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        return lst.Count;
    }

    private void ControlStatusChange(string objProgress, string objAuditor, string objUserCode)
    {
        BT_NewBrowse.Enabled = true;

        if (objProgress == LanguageHandle.GetWord("DengJi").ToString().Trim() && objAuditor == objUserCode)
        {
            BT_NewEdit.Enabled = true;
            BT_NewReview.Enabled = true;
            BT_NewReviewReturn.Enabled = false;

        }
        else if (objProgress == LanguageHandle.GetWord("FuShen").ToString().Trim() && objAuditor == objUserCode)
        {
            BT_NewEdit.Enabled = false;
            BT_NewReview.Enabled = false;
            BT_NewReviewReturn.Enabled = true;

        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewReview.Enabled = false;
            BT_NewReviewReturn.Enabled = false;
            BT_NewBrowse.Enabled = false;

        }
    }



    private void ControlStatusCloseChange()
    {
        BT_NewEdit.Enabled = false;
        BT_NewReview.Enabled = false;
        BT_NewReviewReturn.Enabled = false;
        BT_NewBrowse.Enabled = false;

    }

    
}
