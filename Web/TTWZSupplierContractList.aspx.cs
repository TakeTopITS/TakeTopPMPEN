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

public partial class TTWZSupplierContractList : System.Web.UI.Page
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

        string strSupplierHQL = string.Format(@"select s.*,m.UserName as AuditorName,p.UserName as PushPersonName,
                    a.UserName as CompetentMaterialsName,
                    c.UserName as ContractWhoseName,
                    l.UserName as CompetentLeadershipName
                    from T_WZSupplier s
                    left join T_ProjectMember m on s.Auditor = m.UserCode 
                    left join T_ProjectMember p on s.PushPerson = p.UserCode 
                    left join T_ProjectMember a on s.CompetentMaterials = a.UserCode
                    left join T_ProjectMember c on s.ContractWhose = c.UserCode
                    left join T_ProjectMember l on s.CompetentLeadership = l.UserCode
                    where s.Progress in('提交2','提交3')"); 
                    //and s.ContractWhose = '{0}'", strUserCode);

        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strSupplierHQL += " and s.Progress = '" + strProgress + "'";
        }

        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strSupplierHQL, "Supplier").Tables[0];

        DG_List.DataSource = dtSupplier;
        DG_List.DataBind();

        LB_Sql.Text = strSupplierHQL;
        LB_Record.Text = dtSupplier.Rows.Count + "";
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

                string strEditID = arrOperate[0];
                string strProgress = arrOperate[1];

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);
                ControlStatusChange(strProgress);

                HF_ID.Value = strEditID;
                HF_Progress.Value = strProgress;
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

            string strApprovalDocument = HF_ApprovalDocument.Value;
            string strApprovalDocumentURL = HF_ApprovalDocumentURL.Value;

            string strCompetentLeadership = HF_CompetentLeadership.Value;

            if (string.IsNullOrEmpty(strApprovalDocument) || string.IsNullOrEmpty(strApprovalDocumentURL))
            {
                //string strProgress = HF_Progress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('审批文件不能为空！');", true); 
                return;
            }

            if (string.IsNullOrEmpty(strCompetentLeadership))
            {
                string strProgress = HF_Progress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择主管领导！');ControlStatusChange('" + strProgress + "');", true); 
                return;
            }

            WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
            string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
            IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
            if (supplierList != null && supplierList.Count == 1)
            {
                WZSupplier wZSupplier = (WZSupplier)supplierList[0];


                wZSupplier.ApprovalDocument = strApprovalDocument;
                wZSupplier.ApprovalDocumentURL = strApprovalDocumentURL;

                wZSupplier.CompetentLeadership = strCompetentLeadership;


                wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

                //重新加载列表
                DataBinder();

                ControlStatusCloseChange();

                //HF_ID.Value = "";
                //HF_ApprovalDocument.Value = "";
                //HF_ApprovalDocumentURL.Value = "";
                //LT_ApprovalDocument.Text = "";
                //TXT_CompetentLeadership.Text = "";
                //HF_CompetentLeadership.Value = "";

                //TXT_CompetentLeadership.BackColor = Color.White;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true); 
            }
        }
        else
        {
            //string strProgress = HF_Progress.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要操作的供应商！');", true); 
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
                string strApprovalDocument = FUP_ApprovalDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strApprovalDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strApprovalDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strApprovalDocument);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    //string strProgress = HF_Progress.Value;
                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"');</script>");
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_ApprovalDocument.SaveAs(strDocSavePath + strFileName3);


                    string strUrl = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;
                    LT_ApprovalDocument.Text = "<a href=\"" + "Doc\\" + strUrl + "\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_ApprovalDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_ApprovalDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
                    string strSupplierHQL = "from WZSupplier as wZSupplier where ID = " + strID;
                    IList listSupplier = wZSupplierBLL.GetAllWZSuppliers(strSupplierHQL);
                    if (listSupplier != null && listSupplier.Count > 0)
                    {
                        WZSupplier wZSupplier = (WZSupplier)listSupplier[0];
                        wZSupplier.ApprovalDocument = Path.GetFileNameWithoutExtension(strFileName2);
                        wZSupplier.ApprovalDocumentURL = strUrl;

                        wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);
                    }

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传审批文件成功！');", true); 
                }
                else
                {
                    //string strProgress = HF_Progress.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true); 
                    return;
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            try
            {
                string strApprovalDocument = FUP_ApprovalDocument.PostedFile.FileName;   //获取上传文件的文件名,包括后缀
                if (!string.IsNullOrEmpty(strApprovalDocument))
                {
                    string strExtendName = System.IO.Path.GetExtension(strApprovalDocument);//获取扩展名

                    DateTime dtUploadNow = DateTime.Now; //获取系统时间
                    string strFileName2 = System.IO.Path.GetFileName(strApprovalDocument);
                    string strExtName = Path.GetExtension(strFileName2);

                    string strFileName3 = Path.GetFileNameWithoutExtension(strFileName2) + DateTime.Now.ToString("yyyyMMddHHMMssff") + strExtendName;

                    string strDocSavePath = Server.MapPath("Doc") + "\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\";


                    FileInfo fi = new FileInfo(strDocSavePath + strFileName3);

                    //string strProgress = HF_Progress.Value;
                    if (fi.Exists)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('"+LanguageHandle.GetWord("ZZCZTMWJSCSBGMHZSC").ToString().Trim()+"');</script>");
                    }

                    if (Directory.Exists(strDocSavePath) == false)
                    {
                        //如果不存在就创建file文件夹{
                        Directory.CreateDirectory(strDocSavePath);
                    }

                    FUP_ApprovalDocument.SaveAs(strDocSavePath + strFileName3);

                    LT_ApprovalDocument.Text = "<a href=\"" + "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3 + "\">" + Path.GetFileNameWithoutExtension(strFileName2) + "</a>";

                    HF_ApprovalDocument.Value = Path.GetFileNameWithoutExtension(strFileName2);
                    HF_ApprovalDocumentURL.Value = "Doc\\" + DateTime.Now.ToString("yyyyMM") + "\\" + strUserCode + "\\Doc\\" + strFileName3;

                    //重新加载报价文件列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上传审批文件成功！');", true); 
                }
                else
                {
                    string strProgress = HF_Progress.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择要上传的文件！');", true); 
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
        HF_ApprovalDocument.Value = "";
        HF_ApprovalDocumentURL.Value = "";
        LT_ApprovalDocument.Text = "";
        TXT_CompetentLeadership.Text = "";
        HF_CompetentLeadership.Value = "";

        TXT_CompetentLeadership.BackColor = Color.White;

        ControlStatusCloseChange();
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        string strWZSupplierSql = string.Format(@"select s.*,p.UserName as CompetentLeadershipName from T_WZSupplier s
                    left join T_ProjectMember p on s.CompetentLeadership = p.UserCode 
                    where s.ID = {0}", strID);
        DataTable dtSupplier = ShareClass.GetDataSetFromSql(strWZSupplierSql, "Supplier").Tables[0];
        if (dtSupplier != null && dtSupplier.Rows.Count > 0)
        {
            DataRow drSupplier = dtSupplier.Rows[0];


            //附件列表
            string strApprovalDocument = ShareClass.ObjectToString(drSupplier["ApprovalDocument"]);
            string strApprovalDocumentURL = ShareClass.ObjectToString(drSupplier["ApprovalDocumentURL"]);

            LT_ApprovalDocument.Text = "<a href=\"" + strApprovalDocumentURL + "\" class=\"notTab\" target=\"_blank\">" + strApprovalDocument + "</a>";
            HF_ApprovalDocument.Value = strApprovalDocument;
            HF_ApprovalDocumentURL.Value = strApprovalDocumentURL;

            HF_CompetentLeadership.Value = ShareClass.ObjectToString(drSupplier["CompetentLeadership"]);
            TXT_CompetentLeadership.Text = ShareClass.ObjectToString(drSupplier["CompetentLeadershipName"]);

            TXT_CompetentLeadership.BackColor = Color.CornflowerBlue;

            //string strProgress = HF_Progress.Value;
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);
        }
    }



    protected void BT_NewReport_Click(object sender, EventArgs e)
    {
        //报批
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }


        string strApprovalDocument = HF_ApprovalDocument.Value;
        string strCompetentLeadership = HF_CompetentLeadership.Value;
        if (string.IsNullOrEmpty(strApprovalDocument) | string.IsNullOrEmpty(strCompetentLeadership))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSXXDBNWKBQXBC").ToString().Trim() + "')", true);
            return;
        }


        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = LanguageHandle.GetWord("DiJiao3").ToString().Trim();

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('报批成功！');", true); 
        }
    }



    protected void BT_NewReportReturn_Click(object sender, EventArgs e)
    {
        //报批退回
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = LanguageHandle.GetWord("DiJiao2").ToString().Trim();

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('报批退回成功！');", true); 
        }
    }

    protected void BT_NewSubmitReturn_Click(object sender, EventArgs e)
    {
        //提交退回
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = LanguageHandle.GetWord("DiJiao1").ToString().Trim();

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('提交退回成功！');", true); 
        }
    }





    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //浏览
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYCZDGYSLB").ToString().Trim()+"')", true);
            return;
        }

        //string strProgress = HF_Progress.Value;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZSupplierBrowse.aspx?id=" + strID + "');", true);
    }


    protected void DDL_Progress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        ControlStatusCloseChange();
    }








    private void ControlStatusChange(string objProgress)
    {

        BT_NewBrowse.Enabled = true;

        if (objProgress == LanguageHandle.GetWord("DiJiao2").ToString().Trim())
        {
            BT_NewEdit.Enabled = true;
            BT_NewReport.Enabled = true;
            BT_NewReportReturn.Enabled = false;
            BT_NewSubmitReturn.Enabled = true;


        }
        else if (objProgress == LanguageHandle.GetWord("DiJiao3").ToString().Trim())
        {

            BT_NewEdit.Enabled = false;
            BT_NewReport.Enabled = false;
            BT_NewReportReturn.Enabled = true;
            BT_NewSubmitReturn.Enabled = false;

        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewReport.Enabled = false;
            BT_NewReportReturn.Enabled = false;
            BT_NewSubmitReturn.Enabled = false;

        }

    }



    private void ControlStatusCloseChange()
    {
        BT_NewEdit.Enabled = false;
        BT_NewReport.Enabled = false;
        BT_NewReportReturn.Enabled = false;
        BT_NewSubmitReturn.Enabled = false;

        BT_NewBrowse.Enabled = true;
    }















}
