using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using System.Data.SqlClient;

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakeProjectAssetPO : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strRelatedTypeWF;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strReviewType;

        string strUserName;
        string strUserCode = Session["UserCode"].ToString();

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        strRelatedTypeWF = strRelatedType;

        if (strRelatedType == "Project")
        {
            //strRelatedType = "Project";
            //this.Title = strRelatedType + ":" + strRelatedID + "AssetProcurement";
        }

        if (strRelatedType == "Other")
        {
            //strRelatedType = "Other";
            //this.Title = "AssetProcurement";
        }


        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_ArrivalTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_PurTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ShareClass.InitialProjectMemberTree(TreeView1, strRelatedID);


            strHQL = "from JNUnit as jnUnit order by jnUnit.SortNumber ASC";
            JNUnitBLL jnUnitBLL = new JNUnitBLL();
            lst = jnUnitBLL.GetAllJNUnits(strHQL);
            DL_Unit.DataSource = lst;
            DL_Unit.DataBind();

            strHQL = "from AssetType as assetType Order by assetType.SortNumber ASC";
            AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
            lst = assetTypeBLL.GetAllAssetTypes(strHQL);
            DL_Type.DataSource = lst;
            DL_Type.DataBind();

            ShareClass.LoadCurrencyType(DL_CurrencyType);
            ShareClass.LoadVendorList(DL_VendorList, strUserCode);

            strReviewType = "AssetProcurement";
            strReviewType = "%" + strReviewType + "%";

            WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
            strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName in ";
            strHQL += "(Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = " + "'" + strRelatedTypeWF + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + ")";
            strHQL += " and workFlowTemplate.Type like " + "'" + strReviewType + "'";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
       
            lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();

            //LoadRelatedWL("ProjectReview", "Project", int.Parse(strRelatedID));

            TB_PurManCode.Text = strUserCode;
            LB_PurManName.Text = strUserName;

            LoadAssetPurchaseOrder(strUserCode, strRelatedType, strRelatedID);
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID;
        string strUserCode, strUserName;


        strID = TreeView1.SelectedNode.Target.Trim();

        try
        {
            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ID = " + strID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ProRelatedUser proRelatedUser = (ProRelatedUser)lst[0];

                strUserCode = proRelatedUser.UserCode.Trim();
                strUserName = proRelatedUser.UserName.Trim();

                TB_ApplicantCode.Text = strUserCode;
                LB_ApplicantName.Text = strUserName;
            }
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        TB_ApplicantCode.Text = strUserCode;
        LB_ApplicantName.Text = ShareClass.GetUserName(strUserCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPOID;
            IList lst;
            int intWLNumber;

            strPOID = e.Item.Cells[3].Text.Trim();

            intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
            if (intWLNumber > 0)
            {
                BT_NewMain.Visible = false;
                BT_NewDetail.Visible = false;
                BT_SubmitApply.Enabled = false;
            }
            else
            {
                BT_NewMain.Visible = true;
                BT_NewDetail.Visible = true;
                BT_SubmitApply.Enabled = true;
            }

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid5.Items.Count; i++)
                {
                    DataGrid5.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
                AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
                lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);
                AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

                LB_POID.Text = assetPurchaseOrder.POID.ToString();
                TB_POName.Text = assetPurchaseOrder.POName.Trim();
                DLC_PurTime.Text = assetPurchaseOrder.PurTime.ToString("yyyy-MM-dd");
                DLC_ArrivalTime.Text = assetPurchaseOrder.ArrivalTime.ToString("yyyy-MM-dd");
                NB_Amount.Amount = assetPurchaseOrder.Amount;
                TB_Comment.Text = assetPurchaseOrder.Comment.Trim();
                DL_POStatus.SelectedValue = assetPurchaseOrder.Status.Trim();
                TB_PurManCode.Text = assetPurchaseOrder.OperatorCode.Trim();
                LB_PurManName.Text = assetPurchaseOrder.OperatorName.Trim();
                DL_CurrencyType.SelectedValue = assetPurchaseOrder.CurrencyType;

                LoadAssetPurchaseOrderDetail(strPOID);


                TB_WLName.Text = LanguageHandle.GetWord("GouMai").ToString().Trim()  + assetPurchaseOrder.POName.Trim() + LanguageHandle.GetWord("ShenQing").ToString().Trim();

                LoadRelatedWL("AssetProcurement", "Assets", assetPurchaseOrder.POID);

                BT_SubmitApply.Enabled = true;

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Assign")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                strUserCode = LB_UserCode.Text.Trim();

                intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
                if (intWLNumber > 0)
                {
                    return;
                }

                strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
                AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
                lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

                AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

                try
                {
                    assetPurchaseOrderBLL.DeleteAssetPurchaseOrder(assetPurchaseOrder);


                    LoadAssetPurchaseOrder(strUserCode, strRelatedType, strRelatedID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCKNCZMXJLJC").ToString().Trim() + "')", true);
                }
            }
        
        }
    }
    protected void BT_CreateMain_Click(object sender, EventArgs e)
    {
        LB_POID.Text = "";

        LoadAssetPurchaseOrderDetail("0");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_NewMain_Click(object sender, EventArgs e)
    {
        string strPOID;

        strPOID = LB_POID.Text.Trim();

        if (strPOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }
    }

    protected void AddMain()
    {
        string strPOID, strPOName, strPurManCode, strPurManName, strOperatorCode, strOperatorName, strComment;
        DateTime dtPurTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;

        strPOName = TB_POName.Text.Trim();
        strPurManCode = TB_PurManCode.Text.Trim();
        strPurManName = LB_PurManName.Text.Trim();
        strOperatorCode = LB_UserCode.Text.Trim();
        strOperatorName = LB_UserName.Text.Trim();
        strComment = TB_Comment.Text.Trim();
        dtPurTime = DateTime.Parse(DLC_PurTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);
        deAmount = NB_Amount.Amount;
        strStatus = DL_POStatus.SelectedValue.Trim();
        strComment = TB_Comment.Text.Trim();

        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        AssetPurchaseOrder assetPurchaseOrder = new AssetPurchaseOrder();

        assetPurchaseOrder.POName = strPOName;
        assetPurchaseOrder.PurManCode = strPurManCode;
        try
        {
            assetPurchaseOrder.PurManName = ShareClass.GetUserName(strPurManCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCGRDMBZCWCRJC").ToString().Trim() + "')", true);
            return;
        }
        assetPurchaseOrder.OperatorCode = strOperatorCode;
        assetPurchaseOrder.OperatorName = strOperatorName;
        assetPurchaseOrder.PurTime = dtPurTime;
        assetPurchaseOrder.ArrivalTime = dtArrivalTime;
        assetPurchaseOrder.Amount = 0;
        assetPurchaseOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        assetPurchaseOrder.Comment = strComment;
        assetPurchaseOrder.Status = "New";
        assetPurchaseOrder.RelatedType = "Project";
        assetPurchaseOrder.RelatedID = int.Parse(strRelatedID);

        try
        {
            assetPurchaseOrderBLL.AddAssetPurchaseOrder(assetPurchaseOrder);

            strPOID = ShareClass.GetMyCreatedMaxAssetPurchaseOrderID(strOperatorCode);
            LB_POID.Text = strPOID;


            NB_Amount.Amount = 0;

            TB_WLName.Text = LanguageHandle.GetWord("GouMai").ToString().Trim()  + strPOName + LanguageHandle.GetWord("ShenQing").ToString().Trim();

            LoadAssetPurchaseOrder(strOperatorCode, strRelatedType, strRelatedID);
            LoadAssetPurchaseOrderDetail(strPOID);

            LoadRelatedWL("AssetProcurement", "Assets", assetPurchaseOrder.POID);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMain()
    {
        string strHQL;
        IList lst;

        string strPOID, strUserCode, strPOName, strPurManCode, strPurManName, strComment;
        DateTime dtPurTime, dtArrivalTime;
        decimal deAmount;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();

        strPOID = LB_POID.Text.Trim();
        strPOName = TB_POName.Text.Trim();
        strPurManCode = TB_PurManCode.Text.Trim();
        strPurManName = LB_PurManName.Text.Trim();
        dtPurTime = DateTime.Parse(DLC_PurTime.Text);
        dtArrivalTime = DateTime.Parse(DLC_ArrivalTime.Text);
        deAmount = NB_Amount.Amount;
        strComment = TB_Comment.Text.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        assetPurchaseOrder.POName = strPOName;

        assetPurchaseOrder.PurManCode = strPurManCode;
        try
        {
            assetPurchaseOrder.PurManName = ShareClass.GetUserName(strPurManCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWCGRDMBZCWCRJC").ToString().Trim() + "')", true);
            return;
        }

        assetPurchaseOrder.PurTime = dtPurTime;
        assetPurchaseOrder.ArrivalTime = dtArrivalTime;
        assetPurchaseOrder.Amount = deAmount;
        assetPurchaseOrder.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        assetPurchaseOrder.Comment = strComment;
        assetPurchaseOrder.Status = strStatus;
        assetPurchaseOrder.RelatedType = "Project";
        assetPurchaseOrder.RelatedID = int.Parse(strRelatedID);


        try
        {
            assetPurchaseOrderBLL.UpdateAssetPurchaseOrder(assetPurchaseOrder, int.Parse(strPOID));
            LoadAssetPurchaseOrder(strUserCode, strRelatedType, strRelatedID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        IList lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void DL_VendorList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strVendorCode, strVendorPhone;

        strVendorCode = DL_VendorList.SelectedValue.Trim();
        strHQL = "Select COALESCE(Tel1,'') From T_Vendor Where VendorCode = " + "'" + strVendorCode + "'";

        try
        {
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Vendor");
            if (ds.Tables[0].Rows.Count == 0)
            {
                strHQL = "Select COALESCE(PhoneNum,'') From T_BMSupplierInfo Where Code = " + "'" + strVendorCode + "'";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
            }

            strVendorPhone = ds.Tables[0].Rows[0][0].ToString();
            TB_SupplierPhone.Text = strVendorPhone;
            TB_Supplier.Text = DL_VendorList.SelectedItem.Text;
        }
        catch
        {
            TB_SupplierPhone.Text = "";
            TB_Supplier.Text = "";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserCode = LB_UserCode.Text;
            string strHQL, strPOID;
            IList lst;


            strPOID = LB_POID.Text.Trim();

            int intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
            if (intWLNumber > 0)
            {
                BT_NewMain.Visible = false;
                BT_NewDetail.Visible = false;
                BT_SubmitApply.Enabled = false;
            }
            else
            {
                BT_NewMain.Visible = true;
                BT_NewDetail.Visible = true;
                BT_SubmitApply.Enabled = true;
            }

            string strID = e.Item.Cells[2].Text.Trim();
            LB_ID.Text = strID;

            if (e.CommandName == "Update" | e.CommandName == "Assign")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;

                AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
                lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
                AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

                TB_AssetCode.Text = assetPurRecord.AssetCode;
                TB_AssetName.Text = assetPurRecord.AssetName;
                TB_ModelNumber.Text = assetPurRecord.ModelNumber;
                TB_Spec.Text = assetPurRecord.Spec;
                TB_PurReason.Text = assetPurRecord.PurReason;
                NB_Price.Amount = assetPurRecord.Price;
                TB_ApplicantCode.Text = assetPurRecord.ApplicantCode;
                TB_Supplier.Text = assetPurRecord.Supplier;
                TB_SupplierPhone.Text = assetPurRecord.SupplierPhone;
                LB_ApplicantName.Text = ShareClass.GetUserName(assetPurRecord.ApplicantCode);
                DL_Type.SelectedValue = assetPurRecord.Type;
                DL_Unit.SelectedValue = assetPurRecord.Unit;
                NB_Number.Amount = assetPurRecord.Number;
                DL_Status.SelectedValue = assetPurRecord.Status.Trim();               

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            }

            if (e.CommandName == "Delete")
            {
                intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
                if (intWLNumber > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                    return;
                }

                AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
                strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
                lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
                AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

                try
                {
                    assetPurRecordBLL.DeleteAssetPurRecord(assetPurRecord);

                    LoadAssetPurchaseOrderDetail(strPOID);

                    NB_Amount.Amount = SumPurchaseOrderAmount(strPOID);
                    UpdatePurchaseOrderAmount(strPOID, NB_Amount.Amount);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);


            }
        }
    }


    protected void BT_CreateDetail_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false','popDetailWindow') ", true);
    }

    protected void BT_NewDetail_Click(object sender, EventArgs e)
    {
        string strPOID;

        strPOID = LB_POID.Text.Trim();

        if (strPOID == "")
        {
            AddMain();
        }
        else
        {
            UpdateMain();
        }

        strPOID = LB_POID.Text.Trim();
        int intWLNumber = GetRelatedWorkFlowNumber("AssetProcurement", "Assets", strPOID);
        if (intWLNumber > 0)
        {
            BT_SubmitApply.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCZGLDGZLJLBNSCJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);
            return;
        }
        else
        {
            BT_SubmitApply.Enabled = true;
        }

        string strDetailID;

        strDetailID = LB_ID.Text.Trim();

        if (strDetailID == "")
        {
            AddDetail();
        }
        else
        {
            UpdateDetail();
        }
    }


    protected void AddDetail()
    {
        string strRecordID, strPOID, strType, strAssetCode, strAssetName, strModelNumber, strSpec, strStatus;
        string strSupplier, strSupplierPhone, strUnitName;
        decimal decNumber;
        DateTime dtBuyTime;
        decimal dePrice;
        string strApplicantCode, strPurReason;


        strPOID = LB_POID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();
        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        decNumber = NB_Number.Amount;
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPurReason = TB_PurReason.Text.Trim();
        dePrice = NB_Price.Amount;
        strApplicantCode = TB_ApplicantCode.Text.Trim();
        dtBuyTime = DateTime.Now;
        strSupplier = TB_Supplier.Text.Trim();
        strSupplierPhone = TB_SupplierPhone.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        if (strType == "" | strAssetName == "" | strSpec == "" | strApplicantCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
            AssetPurRecord assetPurRecord = new AssetPurRecord();

            assetPurRecord.POID = int.Parse(strPOID);
            assetPurRecord.Type = strType;
            assetPurRecord.AssetCode = strAssetCode;
            assetPurRecord.AssetName = strAssetName;
            assetPurRecord.Number = decNumber;
            assetPurRecord.Unit = strUnitName;
            assetPurRecord.Number = decNumber;
            assetPurRecord.Price = dePrice;
            assetPurRecord.ModelNumber = strModelNumber;
            assetPurRecord.Spec = strSpec;
            assetPurRecord.PurReason = strPurReason;
            assetPurRecord.PurTime = dtBuyTime;
            assetPurRecord.Status = strStatus;
            assetPurRecord.RelatedType = strRelatedType;
            assetPurRecord.RelatedID = int.Parse(strRelatedID);

            assetPurRecord.ApplicantCode = strApplicantCode;
            try
            {
                assetPurRecord.ApplicantName = ShareClass.GetUserName(strApplicantCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSRDMBZCWCRJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

                return;
            }

            assetPurRecord.Supplier = TB_Supplier.Text.Trim();
            assetPurRecord.SupplierPhone = TB_SupplierPhone.Text.Trim();

            try
            {
                assetPurRecordBLL.AddAssetPurRecord(assetPurRecord);

                strRecordID = ShareClass.GetMyCreatedMaxAssetPurRecordID(strPOID);
                LB_ID.Text = strRecordID;

                LoadAssetPurchaseOrderDetail(strPOID);

                NB_Amount.Amount = SumPurchaseOrderAmount(strPOID);
                UpdatePurchaseOrderAmount(strPOID, NB_Amount.Amount);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            }
        }
    }

    protected void UpdateDetail()
    {
        string strType, strAssetCode, strAssetName, strModelNumber, strSpec, strStatus;
        string strSupplier, strSupplierPhone, strPurReason, strUnitName;
        DateTime dtBuyTime;
        decimal dePrice, deNumber;
        string strApplicantCode;

        string strID, strPOID, strPurManCode, strPurManName;
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text;

        strID = LB_ID.Text.Trim();

        strPOID = LB_POID.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();

        strAssetCode = TB_AssetCode.Text.Trim();
        strAssetName = TB_AssetName.Text.Trim();
        strUnitName = DL_Unit.SelectedValue.Trim();
        strModelNumber = TB_ModelNumber.Text.Trim();
        strSpec = TB_Spec.Text.Trim();
        strPurReason = TB_PurReason.Text.Trim();
        dePrice = NB_Price.Amount;
        deNumber = NB_Number.Amount;
        strApplicantCode = TB_ApplicantCode.Text.Trim();
        dtBuyTime = DateTime.Now;
        strSupplier = TB_Supplier.Text.Trim();
        strSupplierPhone = TB_SupplierPhone.Text.Trim();
        strPurManCode = TB_PurManCode.Text.Trim();
        strPurManName = LB_PurManName.Text.Trim();
        strStatus = DL_Status.SelectedValue;

        if (strType == "" | strAssetName == "" | strSpec == "" | strApplicantCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);
        }
        else
        {
            AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
            strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
            lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);
            AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

            assetPurRecord.POID = int.Parse(strPOID);
            assetPurRecord.Type = strType;
            assetPurRecord.AssetCode = strAssetCode;
            assetPurRecord.AssetName = strAssetName;
            assetPurRecord.Number = deNumber;
            assetPurRecord.Unit = strUnitName;
            assetPurRecord.Price = dePrice;
            assetPurRecord.ModelNumber = strModelNumber;
            assetPurRecord.Spec = strSpec;
            assetPurRecord.PurReason = strPurReason;
            assetPurRecord.PurTime = dtBuyTime;
            assetPurRecord.Status = strStatus;
            assetPurRecord.RelatedType = strRelatedType;
            assetPurRecord.RelatedID = int.Parse(strRelatedID);

            assetPurRecord.ApplicantCode = strApplicantCode;
            try
            {
                assetPurRecord.ApplicantName = ShareClass.GetUserName(strApplicantCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSZDMBZCWCRJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

                return;
            }

            assetPurRecord.Supplier = TB_Supplier.Text.Trim();
            assetPurRecord.SupplierPhone = TB_SupplierPhone.Text.Trim();

            try
            {
                assetPurRecordBLL.UpdateAssetPurRecord(assetPurRecord, int.Parse(strID));

                LoadAssetPurchaseOrderDetail(strPOID);

                NB_Amount.Amount = SumPurchaseOrderAmount(strPOID);
                UpdatePurchaseOrderAmount(strPOID, NB_Amount.Amount);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

            }
        }
    }

    protected void BT_AllPurAsset_Click(object sender, EventArgs e)
    {
        //LB_AssetOwner.Text = LanguageHandle.GetWord("SYCGZCLB").ToString().Trim() + ": ";
        //LB_AssetOwner.Visible = true;

        string strUserCode = LB_UserCode.Text.Trim();

        LoadAssetPurchaseOrder(strUserCode, strRelatedType, strRelatedID);
    }

    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText, strPOID;

        string strWLID, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strPOID = LB_POID.Text.Trim();

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = ShareClass.GetUserName(strCreatorCode);

        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim() + "');</script>");

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

            return "0";
        }

        strXMLFileName = strWLType + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        WorkFlow workFlow = new WorkFlow();

        workFlow.WLName = strWLName;
        workFlow.WLType = strWLType;
        workFlow.XMLFile = strXMLFile2;
        workFlow.TemName = strTemName;
        workFlow.Description = strDescription;
        workFlow.CreatorCode = strCreatorCode;
        workFlow.CreatorName = strCreatorName;
        workFlow.CreateTime = DateTime.Now;
        workFlow.RelatedType = "Assets";
        workFlow.Status = "New";
        workFlow.RelatedID = int.Parse(strPOID);
        workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

        if (CB_SMS.Checked == true)
        {
            workFlow.ReceiveSMS = "YES";
        }
        else
        {
            workFlow.ReceiveSMS = "NO";
        }

        if (CB_Mail.Checked == true)
        {
            workFlow.ReceiveEMail = "YES";
        }
        else
        {
            workFlow.ReceiveEMail = "NO";
        }

        try
        {
            workFlowBLL.AddWorkFlow(workFlow);
            strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

            LoadRelatedWL(strWLType, "Assets", int.Parse(strPOID));

            UpdateAssetPurchaseStatus(strPOID, "InProgress");
            DL_POStatus.SelectedValue = "InProgress";

            strCmdText = "select * from T_AssetPurchaseOrder where POID = " + strPOID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_AssetPurchaseOrder", strXMLFile2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCCGSSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            strWLID = "0";

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCCGSSCSB").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }

    protected void BT_Refrash_Click(object sender, EventArgs e)
    {
        string strHQL, strKeyWord;
        IList lst;

        strKeyWord = TB_KeyWord.Text.Trim();
        strKeyWord = "%" + strKeyWord + "%";


        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName in (Select relatedWorkFlowTemplate.WFTemplateName from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = " + "'" + strRelatedTypeWF + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + ")";
        strHQL += " and workFlowTemplate.TemName like " + "'" + strKeyWord + "'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
     
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popAssignWindow','true') ", true);

    }

    protected void DL_POStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPOID, strStatus, strUserCode;

        strPOID = LB_POID.Text.Trim();
        strStatus = DL_POStatus.SelectedValue.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strPOID != "")
        {
            UpdateAssetPurchaseStatus(strPOID, strStatus);
            LoadAssetPurchaseOrder(strUserCode, strRelatedType, strRelatedID);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strStatus;

        strID = LB_ID.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        if (strID != "")
        {
            UpdateAssetPurchaseOrderDetailStatus(strID, strStatus);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true','popDetailWindow') ", true);

    }

    protected void UpdateAssetPurchaseStatus(string strPOID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        assetPurchaseOrder.Status = strStatus;

        try
        {
            assetPurchaseOrderBLL.UpdateAssetPurchaseOrder(assetPurchaseOrder, int.Parse(strPOID));
        }
        catch
        {
        }
    }

    protected void UpdateAssetPurchaseOrderDetailStatus(string strID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.ID = " + strID;
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);

        AssetPurRecord assetPurRecord = (AssetPurRecord)lst[0];

        assetPurRecord.Status = strStatus;

        try
        {
            assetPurRecordBLL.UpdateAssetPurRecord(assetPurRecord, int.Parse(strID));
        }
        catch
        {
        }
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void LoadAssetPurchaseOrder(string strOperatorCode, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.RelatedType = " + "'" + strRelatedType + "'" + " and assetPurchaseOrder.RelatedID = " + strRelatedID + " and  assetPurchaseOrder.OperatorCode = " + "'" + strOperatorCode + "'" + " Order by assetPurchaseOrder.POID DESC";
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void LoadAssetPurchaseOrderDetail(string strPOID)
    {
        string strHQL = "Select * from T_AssetPurRecord where POID = " + strPOID + " Order by ID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AssetPurRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }
    
    protected int GetWLID()
    {
        int intWLID;
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where WLID in (select max(workFlow1.MLID) from WorkFlow as workFlow)";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        WorkFlow workFlow = (WorkFlow)lst[0];

        intWLID = workFlow.WLID;

        return intWLID;
    }



    protected decimal SumPurchaseOrderAmount(string strPOID)
    {
        string strHQL;
        IList lst;

        decimal deAmount = 0;

        strHQL = "from AssetPurRecord as assetPurRecord where assetPurRecord.POID = " + strPOID;
        AssetPurRecordBLL assetPurRecordBLL = new AssetPurRecordBLL();
        lst = assetPurRecordBLL.GetAllAssetPurRecords(strHQL);

        AssetPurRecord assetPurRecord = new AssetPurRecord();

        for (int i = 0; i < lst.Count; i++)
        {
            assetPurRecord = (AssetPurRecord)lst[i];
            deAmount += assetPurRecord.Number * assetPurRecord.Price;
        }

        return deAmount;
    }

    protected void UpdatePurchaseOrderAmount(string strPOID, decimal deAmount)
    {
        string strHQL;
        IList lst;

        strHQL = "from AssetPurchaseOrder as assetPurchaseOrder where assetPurchaseOrder.POID = " + strPOID;
        AssetPurchaseOrderBLL assetPurchaseOrderBLL = new AssetPurchaseOrderBLL();
        lst = assetPurchaseOrderBLL.GetAllAssetPurchaseOrders(strHQL);

        AssetPurchaseOrder assetPurchaseOrder = (AssetPurchaseOrder)lst[0];

        assetPurchaseOrder.Amount = deAmount;

        try
        {
            assetPurchaseOrderBLL.UpdateAssetPurchaseOrder(assetPurchaseOrder, int.Parse(strPOID));
        }
        catch
        {
        }
    }

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }


}
