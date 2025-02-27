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

public partial class TTWZCompactDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            if (Request.QueryString["CompactCode"] != null)
            {
                string strCompactCode = Request.QueryString["CompactCode"];

                HF_CompactCode.Value = strCompactCode;

                DataPurchaseBinder();

                DataCompactDetailBinder(strCompactCode);

                DG_PurchaseDetail.DataSource = "";
                DG_PurchaseDetail.DataBind();

                LoadObjectTree();
                DataObjectBinder("", "", "");
            }
        }
    }

    private void DataCompactDetailBinder(string strCompactCode)
    {
        string strWZCompactDetailHQL = string.Format(@"select d.*,o.ObjectName,o.Model,o.Grade,o.Criterion,p.PurchaseCode,l.PlanCode,s.UnitName from T_WZCompactDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                            left join T_WZPurchaseDetail p on d.PurchaseDetailID = p.ID
                            left join T_WZPickingPlanDetail l on d.PlanDetailID = l.ID
                            left join T_WZSpan s on o.Unit = s.ID
                            where d.CompactCode = '{0}'
                            order by o.DLCode,o.ObjectName,o.Model", strCompactCode);
        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strWZCompactDetailHQL, "CompactDetail").Tables[0];

        DG_CompactDetail.DataSource = dtCompactDetail;
        DG_CompactDetail.DataBind();

        LB_CompactDetailSql.Text = strWZCompactDetailHQL;

        LB_RowNumber.Text = dtCompactDetail.Rows.Count.ToString();
    }

    protected void DG_CompactDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_CompactDetail.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_CompactDetailSql.Text.Trim();
        DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strHQL, "CompactDetail").Tables[0];

        DG_CompactDetail.DataSource = dtCompactDetail;
        DG_CompactDetail.DataBind();
    }

    private void DataPurchaseBinder()
    {
        string strCompactCode = HF_CompactCode.Value;
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            WZCompactBLL wZCompactBLL = new WZCompactBLL();
            string strWZCompactHQL = "from WZCompact as wZCompact where CompactCode = '" + strCompactCode + "'";
            IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);
            if (listCompact != null && listCompact.Count > 0)
            {
                WZCompact wZCompact = (WZCompact)listCompact[0];

                WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                string strWZPurchaseHQL = string.Format(@"from WZPurchase as wZPurchase 
                        where PurchaseEngineer = '{0}'
                        and Progress in ('询价','评标','上报','决策')
                        and ProjectCode = '{1}' order by MarkTime desc ", wZCompact.PurchaseEngineer, wZCompact.ProjectCode); 
                IList listWZPurchase = wZPurchaseBLL.GetAllWZPurchases(strWZPurchaseHQL);

                HF_SupplierCode.Value = wZCompact.SupplierCode;

                LB_Purchase.DataSource = listWZPurchase;
                LB_Purchase.DataBind();
            }
        }
    }

    private void DataPurchaseDetailBinder()
    {
        string strPurchaseCode = LB_Purchase.SelectedValue;
        if (!string.IsNullOrEmpty(strPurchaseCode))
        {
            string strSupplierCode = HF_SupplierCode.Value;

            string strWZPurchaseDetailHQL = string.Format(@"select d.*,o.ObjectName,o.Model,o.Grade,o.Criterion,l.PlanCode,s.UnitName from T_WZPurchaseDetail d
                                    left join T_WZObject o on d.ObjectCode = o.ObjectCode
                                    left join T_WZPickingPlanDetail l on d.PlanDetailID = l.ID
                                    left join T_WZSpan s on o.Unit = s.ID
                                    where d.PurchaseCode= '{0}'
                               
                                    order by o.DLCode,o.ObjectName,o.Model ", strPurchaseCode, strSupplierCode);
            DataTable dtPurchaseDetail = ShareClass.GetDataSetFromSql(strWZPurchaseDetailHQL, "PurchaseDetail").Tables[0];

            DG_PurchaseDetail.DataSource = dtPurchaseDetail;
            DG_PurchaseDetail.DataBind();
        }
    }

    protected void DG_PurchaseDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "add")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrArges = cmdArges.Split('|');                //PlanDetailID|ObjectCode|StandardCode|Factory|Remark|PurchaseNumber|ApplyMoney|ID
                string strCompactCode = HF_CompactCode.Value;
                if (!string.IsNullOrEmpty(strCompactCode))
                {
                    //判断当前合同明细中是否添加过采购清单
                    string strCheckCompactHQL = string.Format(@"select * from T_WZCompactDetail 
                        where CompactCode = '{0}'
                        and PurchaseDetailID = {1}", strCompactCode, arrArges[7]);
                    DataTable dtCheckCompact = ShareClass.GetDataSetFromSql(strCheckCompactHQL, "CheckCompact").Tables[0];
                    if (dtCheckCompact != null && dtCheckCompact.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGDZDHTMXZYJCZ").ToString().Trim() + "')", true);
                        return;
                    }

                    //验证采购明细与合同是否是同一个供应商
                    string strCompactSupplierSQL = string.Format(@"select SupplierCode from T_WZCompact
                                where CompactCode = '{0}'", strCompactCode);
                    DataTable dtCompactSupplier = ShareClass.GetDataSetFromSql(strCompactSupplierSQL, "CompactSupplier").Tables[0];
                    if (dtCompactSupplier != null && dtCompactSupplier.Rows.Count > 0)
                    {
                        string strCompactSupplierCode = ShareClass.ObjectToString(dtCompactSupplier.Rows[0]["SupplierCode"]);
                        if (!string.IsNullOrEmpty(strCompactSupplierCode))
                        {
                            string strPurchaseDetailSupplierSQL = string.Format(@"select SupplierCode from T_WZPurchaseDetail
                                                    where ID = {0}", arrArges[7]);
                            DataTable dtPurchaseDetailSupplier = ShareClass.GetDataSetFromSql(strPurchaseDetailSupplierSQL, "PurchaseDetailSupplier").Tables[0];
                            if (dtPurchaseDetailSupplier != null && dtPurchaseDetailSupplier.Rows.Count > 0)
                            {
                                string strPurchaseDetailSupplierCode = ShareClass.ObjectToString(dtPurchaseDetailSupplier.Rows[0]["SupplierCode"]);
                                if (strCompactSupplierCode.Trim() != strPurchaseDetailSupplierCode.Trim())
                                {
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTLMDGYSYCGDGYSBYZ").ToString().Trim() + "')", true);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCGDHWZGYS").ToString().Trim() + "')", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHTBXXZHGYS").ToString().Trim() + "')", true);
                            return;
                        }
                    }

                    string strHQL;
                    IList lst;
                    strHQL = "From WZObject as wZObject Where wZObject.ObjectCode = " + "'" + arrArges[1] + "'";
                    WZObjectBLL wZObjectBLL = new WZObjectBLL();
                    lst = wZObjectBLL.GetAllWZObjects(strHQL);
                    WZObject wZObject = (WZObject)lst[0];


                    WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                    WZCompactDetail wZCompactDetail = new WZCompactDetail();
                    wZCompactDetail.CompactCode = strCompactCode;       //合同编号
                    int intPlanDetailID = 0;
                    int.TryParse(arrArges[0], out intPlanDetailID);
                    wZCompactDetail.PlanDetailID = intPlanDetailID;          //计划编号
                    wZCompactDetail.ObjectCode = arrArges[1];        //物资代码

                    wZCompactDetail.ObjectName = wZObject.ObjectName;
                    wZCompactDetail.Model = wZObject.Model;
                    wZCompactDetail.Criterion = wZObject.Criterion;
                    wZCompactDetail.Grade = wZObject.Grade;
                    wZCompactDetail.Unit = wZObject.Unit;

                    wZCompactDetail.StandardCode = arrArges[2];      //规格书编号
                    wZCompactDetail.Factory = arrArges[3];           //生产厂家
                    wZCompactDetail.Remark = arrArges[4];            //备注

                    decimal decimalCompactNumber = 0;
                    decimal.TryParse(arrArges[5], out decimalCompactNumber);
                    wZCompactDetail.CompactNumber = decimalCompactNumber;      //合同数量
                    decimal decimalCompactPrice = 0;
                    decimal.TryParse(arrArges[6], out decimalCompactPrice);
                    wZCompactDetail.CompactPrice = decimalCompactPrice;       //合同单价
                    wZCompactDetail.CompactMoney = decimalCompactNumber * decimalCompactPrice;       //合同金额
                    wZCompactDetail.CollectNumber = 0;      //收料数量
                    wZCompactDetail.CollectMoney = 0;       //收料金额
                    wZCompactDetail.CheckCode = "";         //检号
                    int intPurchaseDetailID = 0;
                    int.TryParse(arrArges[7], out intPurchaseDetailID);
                    wZCompactDetail.PurchaseDetailID = intPurchaseDetailID;

                    wZCompactDetailBLL.AddWZCompactDetail(wZCompactDetail);

                    //修改全同表的合同金额，使用标记
                    string strSelectCompactDetailHQL = "select SUM(CompactMoney) as CompactMoney,count(1) as RowNumber from T_WZCompactDetail where CompactCode = '" + strCompactCode + "'";
                    DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strSelectCompactDetailHQL, "strSelectCompactDetailHQL").Tables[0];
                    string strCompactMoney = ShareClass.ObjectToString(dtCompactDetail.Rows[0]["CompactMoney"]);
                    string strRowNumber = ShareClass.ObjectToString(dtCompactDetail.Rows[0]["RowNumber"]);
                    int intRowNumber = 0;
                    int.TryParse(strRowNumber, out intRowNumber);
                    decimal decimalCompactMoney = 0;
                    decimal.TryParse(strCompactMoney, out decimalCompactMoney);
                    string strUpdateCompactHQL = "update T_WZCompact set CompactMoney = " + decimalCompactMoney + ",RowNumber=" + intRowNumber + ",IsMark=-1 where CompactCode = '" + strCompactCode + "'";
                    ShareClass.RunSqlCommand(strUpdateCompactHQL);
                    //修改采购清单的进度，使用标记
                    string strUpdatePurchaseDetailHQL = "update T_WZPurchaseDetail set IsMark = -1,Progress='合同' where ID = " + intPurchaseDetailID; 
                    ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                    //修改领料计划明细进度
                    string strUpdatePickingDetailHQL = "update T_WZPickingPlanDetail set Progress = '合同' where ID = " + intPlanDetailID; 
                    ShareClass.RunSqlCommand(strUpdatePickingDetailHQL);

                    //重新加载合同明细列表
                    DataCompactDetailBinder(strCompactCode);
                    DataPurchaseDetailBinder();
                }
            }
        }
    }

    protected void DG_ObjectList_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "add")
        {
            string cmdArges = e.CommandArgument.ToString();
            string[] arrArges = cmdArges.Split('|');                //ObjectCode|Model|Grade|Unit|ConvertUnit|ConvertRatio
            string strCompactCode = HF_CompactCode.Value;
            if (!string.IsNullOrEmpty(strCompactCode))
            {
                //判断当前合同明细中是否添加过当前物资代码
                string strCheckCompactHQL = string.Format(@"select * from T_WZCompactDetail 
                        where CompactCode = '{0}'
                        and ObjectCode = '{1}'", strCompactCode, arrArges[0]);
                DataTable dtCheckCompact = ShareClass.GetDataSetFromSql(strCheckCompactHQL, "CheckCompact").Tables[0];
                if (dtCheckCompact != null && dtCheckCompact.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZWZDMZDHTMXZYJCZ").ToString().Trim() + "')", true);
                    return;
                }

                WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
                WZCompactDetail wZCompactDetail = new WZCompactDetail();
                wZCompactDetail.CompactCode = strCompactCode;       //合同编号
                wZCompactDetail.PlanDetailID = 0;          //计划编号
                wZCompactDetail.ObjectCode = arrArges[0];        //物资代码
                wZCompactDetail.StandardCode = arrArges[1];      //规格书编号
                wZCompactDetail.Factory = "";           //生产厂家
                wZCompactDetail.Remark = "";            //备注
                wZCompactDetail.CompactNumber = 0;      //合同数量
                wZCompactDetail.CompactPrice = 0;       //合同单价
                wZCompactDetail.CompactMoney = 0;       //合同金额
                wZCompactDetail.CollectNumber = 0;      //收料数量
                wZCompactDetail.CollectMoney = 0;       //收料金额
                wZCompactDetail.CheckCode = "";         //检号
                wZCompactDetail.PurchaseDetailID = 0;

                wZCompactDetailBLL.AddWZCompactDetail(wZCompactDetail);
            }
        }
    }

    protected void DG_CompactDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        string strCompactCode = HF_CompactCode.Value;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();

            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail 
                        where ID = {0}", cmdArges);
            IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
            if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
            {
                WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[0];
                if (wZCompactDetail.IsMark != 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSYBJBW0SBYXSC").ToString().Trim() + "')", true);
                    return;
                }

                wZCompactDetailBLL.DeleteWZCompactDetail(wZCompactDetail);

                //修改全同表的合同金额，使用标记
                string strSelectCompactDetailHQL = "select SUM(CompactMoney) as CompactMoney,count(1) as RowNumber from T_WZCompactDetail where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                DataTable dtCompactDetail = ShareClass.GetDataSetFromSql(strSelectCompactDetailHQL, "strSelectCompactDetailHQL").Tables[0];
                if (dtCompactDetail != null && dtCompactDetail.Rows.Count > 0)
                {
                    string strCompactMoney = ShareClass.ObjectToString(dtCompactDetail.Rows[0]["CompactMoney"]);
                    string strRowNumber = ShareClass.ObjectToString(dtCompactDetail.Rows[0]["RowNumber"]);
                    int intRowNumber = 0;
                    int.TryParse(strRowNumber, out intRowNumber);
                    if (intRowNumber == 0)
                    {
                        decimal decimalCompactMoney = 0;
                        decimal.TryParse(strCompactMoney, out decimalCompactMoney);
                        string strUpdateCompactHQL = "update T_WZCompact set CompactMoney = " + decimalCompactMoney + ",RowNumber=" + intRowNumber + ",IsMark=0 where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                        ShareClass.RunSqlCommand(strUpdateCompactHQL);
                    }
                    else
                    {
                        decimal decimalCompactMoney = 0;
                        decimal.TryParse(strCompactMoney, out decimalCompactMoney);
                        string strUpdateCompactHQL = "update T_WZCompact set CompactMoney = " + decimalCompactMoney + ",RowNumber=" + intRowNumber + " where CompactCode = '" + wZCompactDetail.CompactCode + "'";
                        ShareClass.RunSqlCommand(strUpdateCompactHQL);
                    }
                }
                //修改采购清单的进度，使用标记
                string strUpdatePurchaseDetailHQL = "update T_WZPurchaseDetail set IsMark = 0 where ID = " + wZCompactDetail.PurchaseDetailID;
                ShareClass.RunSqlCommand(strUpdatePurchaseDetailHQL);

                //重新加载合同明细列表
                DG_CompactDetail.CurrentPageIndex = 0;
                DataCompactDetailBinder(strCompactCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
        }
        else if (cmdName == "edit")
        {
            for (int i = 0; i < DG_CompactDetail.Items.Count; i++)
            {
                DG_CompactDetail.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdArges = e.CommandArgument.ToString();                             //ID|StandardCode|Factory|Remark
            string[] arrArges = cmdArges.Split('|');

            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail 
                        where ID = {0}", arrArges[0]);
            IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
            if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
            {
                WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[0];
                if (wZCompactDetail.IsMark != 0)
                {
                    TXT_StandardCode.ReadOnly = true;
                    TXT_Factory.ReadOnly = true;
                    TXT_Remark.ReadOnly = true;

                    TXT_CompactNumber.ReadOnly = true;
                    TXT_CompactMoney.ReadOnly = true;
                    LB_CompactPrice.Text = wZCompactDetail.CompactPrice.ToString();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSYBJBW0SBYXXG").ToString().Trim() + "')", true);
                    return;
                }
                else
                {
                    TXT_StandardCode.ReadOnly = false;
                    TXT_Factory.ReadOnly = false;
                    TXT_Remark.ReadOnly = false;

                    HF_CompactDetailID.Value = wZCompactDetail.ID.ToString();
                    TXT_StandardCode.Text = wZCompactDetail.StandardCode;
                    TXT_Factory.Text = wZCompactDetail.Factory;
                    TXT_Remark.Text = wZCompactDetail.Remark;

                    TXT_CompactNumber.Text = wZCompactDetail.CompactNumber.ToString();
                    LB_CompactPrice.Text = wZCompactDetail.CompactPrice.ToString();
                    TXT_CompactMoney.Text = wZCompactDetail.CompactMoney.ToString();
                }
            }
        }
    }

    protected void LB_Purchase_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_Purchase.SelectedValue))
        {
            DataPurchaseDetailBinder();
        }
    }

    protected void BT_Calculate_Click(object sender, EventArgs e)
    {
        decimal deCompactNumber, deCompactPrice;

        try
        {
            deCompactNumber = decimal.Parse(TXT_CompactNumber.Text.Trim());
            deCompactPrice = decimal.Parse(LB_CompactPrice.Text.Trim());

            TXT_CompactMoney.Text = (deCompactNumber * deCompactPrice).ToString();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBZBNBHFFZF").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strCompactDetailID = HF_CompactDetailID.Value;
        string strCompactCode = HF_CompactCode.Value;
        if (!string.IsNullOrEmpty(strCompactDetailID))
        {
            string strStandardCode = TXT_StandardCode.Text.Trim();
            string strFactory = TXT_Factory.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();
            string strCompactNumber = TXT_CompactNumber.Text.Trim();
            string strCompactMoney = TXT_CompactMoney.Text.Trim();
            string strCompactPrice = LB_CompactPrice.Text.Trim();

            if (!ShareClass.CheckStringRight(strStandardCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBHBNBHFFZF").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFactory))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCJBNBHFFZF").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemark))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBZBNBHFFZF").ToString().Trim() + "')", true);
                return;
            }

            try
            {
                decimal.Parse(strCompactNumber);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBXWSZX").ToString().Trim() + "')", true);
                return;
            }

            if (decimal.Parse(strCompactNumber) <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBIXUDAYULING").ToString().Trim() + "')", true);
                return;
            }

            try
            {
                decimal.Parse(strCompactMoney);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBXWSZX").ToString().Trim() + "')", true);
                return;
            }

            if (decimal.Parse(strCompactMoney) <= 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBIXUDAYULING").ToString().Trim() + "')", true);
                return;
            }



            int intCompactDetailID = 0;
            int.TryParse(strCompactDetailID, out intCompactDetailID);
            WZCompactDetailBLL wZCompactDetailBLL = new WZCompactDetailBLL();
            string strWZCompactDetailHQL = string.Format(@"from WZCompactDetail as wZCompactDetail 
                        where ID = {0}", intCompactDetailID);
            IList listWZCompactDetail = wZCompactDetailBLL.GetAllWZCompactDetails(strWZCompactDetailHQL);
            if (listWZCompactDetail != null && listWZCompactDetail.Count > 0)
            {
                WZCompactDetail wZCompactDetail = (WZCompactDetail)listWZCompactDetail[0];

                wZCompactDetail.StandardCode = strStandardCode;
                wZCompactDetail.Factory = strFactory;
                wZCompactDetail.Remark = strRemark;

                wZCompactDetail.CompactNumber = decimal.Parse(strCompactNumber);
                wZCompactDetail.CompactMoney = decimal.Parse(strCompactNumber) * decimal.Parse(strCompactPrice);

                wZCompactDetailBLL.UpdateWZCompactDetail(wZCompactDetail, wZCompactDetail.ID);

                //重新加载合同明细
                DataCompactDetailBinder(strCompactCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYXGDHTMX").ToString().Trim() + "')", true);
            return;
        }
    }

    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_CompactDetail.Items.Count; i++)
        {
            DG_CompactDetail.Items[i].ForeColor = Color.Black;
        }

        HF_CompactDetailID.Value = "";
        TXT_StandardCode.Text = "";
        TXT_Factory.Text = "";
        TXT_Remark.Text = "";
    }

    private void LoadObjectTree()
    {
        TV_Type.Nodes.Clear();
        TreeNode Node = new TreeNode();
        Node.Text = LanguageHandle.GetWord("QuanBuCaiLiao").ToString().Trim();
        Node.Value = "all|0|0|0";
        string strDLSQL = "select * from T_WZMaterialDL";
        DataTable dtDL = ShareClass.GetDataSetFromSql(strDLSQL, "DL").Tables[0];
        if (dtDL != null && dtDL.Rows.Count > 0)
        {
            foreach (DataRow drDL in dtDL.Rows)
            {
                TreeNode DLNode = new TreeNode();
                DLNode.Text = drDL["DLName"].ToString();
                string strDLCode = drDL["DLCode"].ToString();
                DLNode.Value = strDLCode + "|0|0|1";
                string strZLSQL = string.Format("select * from T_WZMaterialZL where DLCode = '{0}'", strDLCode);
                DataTable dtZL = ShareClass.GetDataSetFromSql(strZLSQL, "ZL").Tables[0];
                if (dtZL != null && dtZL.Rows.Count > 0)
                {
                    foreach (DataRow drZL in dtZL.Rows)
                    {
                        TreeNode ZLNode = new TreeNode();
                        ZLNode.Text = drZL["ZLName"].ToString();
                        string strZLCode = drZL["ZLCode"].ToString();
                        ZLNode.Value = strDLCode + "|" + strZLCode + "|0|2";
                        string strXLSQL = string.Format("select * from T_WZMaterialXL where DLCode = '{0}' and ZLCode = '{1}'", strDLCode, strZLCode);
                        DataTable dtXL = ShareClass.GetDataSetFromSql(strXLSQL, "XL").Tables[0];
                        if (dtXL != null && dtXL.Rows.Count > 0)
                        {
                            foreach (DataRow drXL in dtXL.Rows)
                            {
                                TreeNode XLNode = new TreeNode();
                                XLNode.Text = drXL["XLName"].ToString();
                                XLNode.Value = strDLCode + "|" + strZLCode + "|" + drXL["XLCode"].ToString() + "|3";
                                ZLNode.ChildNodes.Add(XLNode);
                            }
                        }
                        DLNode.CollapseAll();
                        DLNode.ChildNodes.Add(ZLNode);
                    }
                }
                Node.ChildNodes.Add(DLNode);
            }
        }
        //Node.ExpandAll();
        TV_Type.Nodes.Add(Node);
    }

    private void DataObjectBinder(string strDLCode, string strZLCode, string strXLCode)
    {
        WZObjectBLL wZObjectBLL = new WZObjectBLL();
        string strObjectSQL = "from WZObject as wZObject where 1=1 ";
        strObjectSQL += " and DLCode = '" + strDLCode + "'";
        strObjectSQL += " and ZLCode = '" + strZLCode + "'";
        strObjectSQL += " and XLCode = '" + strXLCode + "'";
        IList listObject = wZObjectBLL.GetAllWZObjects(strObjectSQL);

        DG_ObjectList.DataSource = listObject;
        DG_ObjectList.DataBind();
    }


    protected void TV_Type_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (TV_Type.SelectedNode != null)
        {
            string strSelectedValue = TV_Type.SelectedNode.Value;
            string[] arrSelectedValue = strSelectedValue.Split('|');
            if (arrSelectedValue[0] != "all")
            {
                DataObjectBinder(arrSelectedValue[0], arrSelectedValue[1], arrSelectedValue[2]);

                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>clickObject();</script>");
            }
        }
    }


    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataCompactDetailBinder(HF_CompactCode.Value);

    }

}
