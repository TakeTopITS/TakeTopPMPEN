using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZSupplierReferrerList : System.Web.UI.Page
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

        string strSupplierHQL = string.Format(@"select distinct s.*,m.UserName as AuditorName,p.UserName as PushPersonName,
                    a.UserName as CompetentMaterialsName,
                    c.UserName as ContractWhoseName,
                    l.UserName as CompetentLeadershipName
                    from T_WZSupplier s
                    left join T_ProjectMember m on s.Auditor = m.UserCode 
                    left join T_ProjectMember p on s.PushPerson = p.UserCode 
                    left join T_ProjectMember a on s.CompetentMaterials = a.UserCode
                    left join T_ProjectMember c on s.ContractWhose = c.UserCode
                    left join T_ProjectMember l on s.CompetentLeadership = l.UserCode
                    where s.PushPerson = '{0}'", strUserCode);


        string strProgress = DDL_Progress.SelectedValue;
        if (!string.IsNullOrEmpty(strProgress))
        {
            strSupplierHQL += " and s.Progress = '" + strProgress + "'";
        }
        else
        {
            strSupplierHQL += " and  s.Progress in('提交','提交1')";
        }

        strSupplierHQL += " order by SupplierName";

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


                string strHQL;
                string strSupplierName, strPushUnit;
                strHQL = "Select SupplierCode,SupplierName,PushUnit From T_WZSupplier Where ID = " + strEditID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZSupplier");
                strSupplierName = ds.Tables[0].Rows[0][1].ToString().Trim();
                strPushUnit = ds.Tables[0].Rows[0][2].ToString().Trim();

                if (strProgress == "提交")
                {
                    BT_ApprovalDocument.Enabled = true;
                }
                else
                {
                    BT_ApprovalDocument.Enabled = false;
                }

                LoadRelatedWL("供应商管理", "供应商", int.Parse(strEditID));

                if (GetRelatedWLNumberByStatus("供应商管理", "供应商", int.Parse(strEditID), "通过") > 0 | GetRelatedWLNumberByStatus("供应商管理", "供应商", int.Parse(strEditID), "结案") > 0 | strPushUnit == "上级部门")
                {
                    BT_DirectPush.Enabled = true;
                }
                else
                {
                    BT_DirectPush.Enabled = false;
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
            string strApprovalDocument = HF_ApprovalDocument.Value;
            string strApprovalDocumentURL = HF_ApprovalDocumentURL.Value;

            string strCompetentMaterials = HF_CompetentMaterials.Value;

            //if (string.IsNullOrEmpty(strApprovalDocument) || string.IsNullOrEmpty(strApprovalDocumentURL))
            //{
            //    //string strProgress = HF_Progress.Value;
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('审批文件不能为空！');", true);
            //    return;
            //}

            //if (string.IsNullOrEmpty(strCompetentMaterials))
            //{
            //    //string strProgress = HF_Progress.Value;
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择物资主管！');", true);
            //    return;
            //}

            string strContractWhose = HF_ContractWhose.Value;
            if (string.IsNullOrEmpty(strContractWhose))
            {
                //string strProgress = HF_Progress.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择合同监审！');", true);
                return;
            }

            string strCompetentLeadership = HF_CompetentLeadership.Value;
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

                wZSupplier.ContractWhose = strContractWhose;

                wZSupplier.CompetentMaterials = strCompetentMaterials;
                wZSupplier.CompetentLeadership = strCompetentLeadership;

                wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

                //重新加载列表
                DataBinder();

                ControlStatusCloseChange();

                //HF_ID.Value = "";
                //HF_ApprovalDocument.Value = "";
                //HF_ApprovalDocumentURL.Value = "";
                //LT_ApprovalDocument.Text = "";
                //TXT_CompetentMaterials.Text = "";
                //HF_CompetentMaterials.Value = "";

                //TXT_CompetentMaterials.BackColor = Color.White;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');", true);
            }
        }
        else
        {
            string strProgress = HF_Progress.Value;
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
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + Resources.lang.ZZCZTMWJSCSBGMHZSC + "');</script>");
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
                    string strProgress = HF_Progress.Value;
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
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + Resources.lang.ZZCZTMWJSCSBGMHZSC + "');</script>");
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
        TXT_CompetentMaterials.Text = "";
        HF_CompetentMaterials.Value = "";
        HF_ContractWhose.Value = "";
        HF_CompetentLeadership.Value = "";

        TXT_CompetentMaterials.BackColor = Color.White;

        ControlStatusCloseChange();
    }


    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYCZDGYSLB + "')", true);
            return;
        }

        string strWZSupplierSql = string.Format(@"select s.*,p.UserName as CompetentMaterialsName from T_WZSupplier s
                    left join T_ProjectMember p on s.CompetentMaterials = p.UserCode 
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

            HF_CompetentMaterials.Value = ShareClass.ObjectToString(drSupplier["CompetentMaterials"]);
            TXT_CompetentMaterials.Text = ShareClass.ObjectToString(drSupplier["CompetentMaterialsName"]);

            HF_CompetentLeadership.Value = ShareClass.ObjectToString(drSupplier["CompetentLeadership"]);
            TXT_CompetentLeadership.Text = ShareClass.GetUserName(ShareClass.ObjectToString(drSupplier["CompetentLeadership"]));

            HF_ContractWhose.Value = ShareClass.ObjectToString(drSupplier["ContractWhose"]);
            TXT_ContractWhose.Text = ShareClass.GetUserName(ShareClass.ObjectToString(drSupplier["ContractWhose"]));

            TXT_CompetentMaterials.BackColor = Color.CornflowerBlue;
            TXT_CompetentLeadership.BackColor = Color.CornflowerBlue;
            TXT_ContractWhose.BackColor = Color.CornflowerBlue;


            //string strProgress = HF_Progress.Value;
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);
        }
    }

    protected void BT_DirectPush_Click(object sender, EventArgs e)
    {
        //直接推荐
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYCZDGYSLB + "')", true);
            return;
        }

        //string strApprovalDocument = HF_CompetentMaterials.Value;
        //string strCompetentMaterials = HF_CompetentMaterials.Value;
        //if (string.IsNullOrEmpty(strApprovalDocument) | string.IsNullOrEmpty(strCompetentMaterials))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYSXXDBNWKBQXBC + "')", true);
        //    return;
        //}

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            //判断是否已经选择好物资主管

            wZSupplier.Progress = "批准";

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('推荐成功！');", true);
        }
    }


    protected void BT_NewPush_Click(object sender, EventArgs e)
    {
        //推荐
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYCZDGYSLB + "')", true);
            return;
        }

        //string strApprovalDocument = HF_CompetentMaterials.Value;
        //string strCompetentMaterials = HF_CompetentMaterials.Value;
        //if (string.IsNullOrEmpty(strApprovalDocument) | string.IsNullOrEmpty(strCompetentMaterials))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZYSXXDBNWKBQXBC + "')", true);
        //    return;
        //}

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            //判断是否已经选择好物资主管

            wZSupplier.Progress = "提交1";

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('推荐成功！');", true);
        }
    }

    protected void BT_NewPushReturn_Click(object sender, EventArgs e)
    {
        //推荐退回
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYCZDGYSLB + "')", true);
            return;
        }

        WZSupplierBLL wZSupplierBLL = new WZSupplierBLL();
        string strSupplierSql = "from WZSupplier as wZSupplier where id = " + strID;
        IList supplierList = wZSupplierBLL.GetAllWZSuppliers(strSupplierSql);
        if (supplierList != null && supplierList.Count == 1)
        {
            WZSupplier wZSupplier = (WZSupplier)supplierList[0];

            wZSupplier.Progress = "提交";

            wZSupplierBLL.UpdateWZSupplier(wZSupplier, wZSupplier.ID);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('推荐退回成功！');", true);
        }
    }

    protected void BT_NewBrowse_Click(object sender, EventArgs e)
    {
        //浏览
        string strID = HF_ID.Value;
        if (string.IsNullOrEmpty(strID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYCZDGYSLB + "')", true);
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

        string strProcess;

        strProcess = DDL_Progress.SelectedValue.Trim();
        if (strProcess == "提交")
        {
            BT_ApprovalDocument.Enabled = true;
        }
        else
        {
            BT_ApprovalDocument.Enabled = false;
        }
    }


    private void ControlStatusChange(string objProgress)
    {
        BT_NewBrowse.Enabled = true;

        if (objProgress == "提交")
        {
            BT_NewEdit.Enabled = true;
            BT_NewPush.Enabled = true;
            BT_DirectPush.Enabled = true;
            BT_NewPushReturn.Enabled = false;

        }
        else if (objProgress == "提交1")
        {
            BT_NewEdit.Enabled = false;
            BT_NewPush.Enabled = false;
            BT_DirectPush.Enabled = true;
            BT_NewPushReturn.Enabled = true;

        }
        else if (objProgress == "批准")
        {
            BT_NewEdit.Enabled = false;
            BT_NewPush.Enabled = false;
            BT_DirectPush.Enabled = false;
            BT_NewPushReturn.Enabled = true;

        }
        else
        {
            BT_NewEdit.Enabled = false;
            BT_NewPush.Enabled = false;
            BT_DirectPush.Enabled = false;
            BT_NewPushReturn.Enabled = false;
        }
    }

    private void ControlStatusCloseChange()
    {
        BT_NewEdit.Enabled = false;
        BT_NewPushReturn.Enabled = false;
        BT_NewBrowse.Enabled = false;

        BT_NewPush.Enabled = true;
        BT_DirectPush.Enabled = true;

    }

    protected int LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType In (" + "'" + strRelatedType + "','其它')" + " and workFlow.RelatedID = " + intRelatedID.ToString();
        strHQL += " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        return lst.Count;
    }


    protected int GetRelatedWLNumberByStatus(string strWLType, string strRelatedType, int intRelatedID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType In (" + "'" + strRelatedType + "','其它')" + "  and workFlow.RelatedID = " + intRelatedID.ToString();
        strHQL += " and workFlow.Status = " + "'" + strStatus + "'";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }
}