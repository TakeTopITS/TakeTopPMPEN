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
using System.Data;
using System.Drawing;

public partial class TTWZPlanDetail : System.Web.UI.Page
{
    public string strPlanCode
    {
        get;
        set;
    }
    public string strUserCode
    {
        get;
        set;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        if (!string.IsNullOrEmpty(Request.QueryString["planCode"]))
        {
            strPlanCode = Request.QueryString["planCode"].ToString();
        }
        else
        {
            strPlanCode = "";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataPickingPlanDetailBinder(strPlanCode);
            //DataPickingPlanBander();
            LoadObjectTree();
            DataObjectBinder("", "", "");
        }
    }


    private void DataPickingPlanDetailBinder(string strPlanCode)
    {
        DG_PickPlanDetailList.CurrentPageIndex = 0;

        string strWZPickingPlanDetailHQL = string.Format(@"select d.*,o.ObjectName,o.Model,o.Grade,o.Criterion,(Select UnitName From T_WZSpan Where ID = o.Unit) as PlanUnit,(Select UnitName From T_WZSpan Where ID = o.ConvertUnit) as ConvertUnit from T_WZPickingPlanDetail d
                                        left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                                        where d.PlanCode = '{0}'
                                        order by o.DLCode,o.ObjectName,o.Model", strPlanCode);
        DataTable dtWZPickingPlanDetail = ShareClass.GetDataSetFromSql(strWZPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];

        DG_PickPlanDetailList.DataSource = dtWZPickingPlanDetail;
        DG_PickPlanDetailList.DataBind();

        LB_PickPlanDetailSql.Text = strWZPickingPlanDetailHQL;

        LB_ShowRecordCount.Text = dtWZPickingPlanDetail.Rows.Count.ToString();
    }


    protected void DG_PickPlanDetailList_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_PickPlanDetailList.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_PickPlanDetailSql.Text.Trim();
        DataTable dtWZPickingPlanDetail = ShareClass.GetDataSetFromSql(strHQL, "PickingPlanDetail").Tables[0];

        DG_PickPlanDetailList.DataSource = dtWZPickingPlanDetail;
        DG_PickPlanDetailList.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    private void DataObjectBinder(string strDLCode, string strZLCode, string strXLCode)
    {
        string strObjectSQL = @"select o.*,u.UnitName,c.UnitName as ConvertUnitName from T_WZObject o
                        left join T_WZSpan u on o.Unit = u.ID
                        left join T_WZSpan c on o.ConvertUnit = c.ID
                        where 1=1 ";
        strObjectSQL += " and o.DLCode = '" + strDLCode + "'";
        strObjectSQL += " and o.ZLCode = '" + strZLCode + "'";
        strObjectSQL += " and o.XLCode = '" + strXLCode + "'";
        DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

        DG_ObjectList.DataSource = dtObject;
        DG_ObjectList.DataBind();
    }


    private void LoadObjectTree()
    {
        TV_Type.Nodes.Clear();
        TreeNode Node = new TreeNode();
        Node.Text = "全部材料";
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


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(HF_PickingPlanDetailID.Value))
            {
                string strPlanNumber = TXT_PlanNumber.Text.Trim();
                string strConvertPlanNumber = TXT_ConvertPlanNumber.Text.Trim();
                string strRemark = TXT_Remark.Text.Trim();

                if (!ShareClass.CheckIsNumber(strPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSLBXSXSHZZS + "')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strConvertPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHSSLBXSXSHZZS + "')", true);
                    return;
                }
                if (!ShareClass.CheckStringRight(strRemark))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBZBNWFFZFC + "')", true);
                    return;
                }

                int intPickingPlanDetailID = 0;
                int.TryParse(HF_PickingPlanDetailID.Value, out intPickingPlanDetailID);

                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + intPickingPlanDetailID;
                IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                {
                    WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
                    decimal decimalPlanNumber = 0;
                    decimal.TryParse(strPlanNumber, out decimalPlanNumber);
                    decimal decimalConvertNumber = 0;
                    decimal.TryParse(strConvertPlanNumber, out decimalConvertNumber);
                    decimal decimalMarket = 0;
                    decimal.TryParse(HF_Market.Value, out decimalMarket);

                    wZPickingPlanDetail.PlanNumber = decimalPlanNumber;
                    wZPickingPlanDetail.ConvertNumber = decimalConvertNumber;
                    wZPickingPlanDetail.Remark = strRemark;
                    wZPickingPlanDetail.PlanCost = decimalPlanNumber * decimalMarket;

                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, intPickingPlanDetailID);

                    //计划数量、预计费用写入计划里面
                    string strTotalPickingPlanDetailHQL = string.Format(@"select COUNT(1) as RowNumber,SUM(PlanCost) as TotalPlanCost from T_WZPickingPlanDetail
                                    where PlanCode = '{0}'
                                    group by PlanCode", wZPickingPlanDetail.PlanCode);
                    DataTable dtTotalPlan = ShareClass.GetDataSetFromSql(strTotalPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];
                    int intDetailCount = 0;
                    decimal decimalPlanCost = 0;
                    if (dtTotalPlan != null && dtTotalPlan.Rows.Count > 0)
                    {
                        int.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["RowNumber"]), out intDetailCount);
                        decimal.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["TotalPlanCost"]), out decimalPlanCost);
                    }


                    int intIsMark = 0;
                    if (intDetailCount > 0)
                    {
                        intIsMark = -1;
                    }
                    string strUpdatePickPlanHQL = string.Format(@"update T_WZPickingPlan 
                                        set DetailCount = {0},
                                            PlanCost={1},
                                            IsMark = {3}
                                        where PlanCode='{2}'", intDetailCount, decimalPlanCost, wZPickingPlanDetail.PlanCode,intIsMark);
                    ShareClass.RunSqlCommand(strUpdatePickPlanHQL);


                    //重新加载计划明细列表
                    DataPickingPlanDetailBinder(strPlanCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void DG_PickPlanDetailList_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_PickPlanDetailList.Items.Count; i++)
            {
                DG_PickPlanDetailList.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrArges = cmdArges.Split('|');

                string strNewID = arrArges[0];
                string strNewProgress = arrArges[1];
                string strNewIsMark = arrArges[2];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "','" + strNewIsMark + "');", true);

                HF_NewID.Value = strNewID;
                HF_NewProgress.Value = strNewProgress;
                HF_NewIsMark.Value = strNewIsMark;
            }
            else if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrArges = cmdArges.Split('|');

                HF_PickingPlanDetailID.Value = arrArges[0];

                WZObjectBLL wZObjectBLL = new WZObjectBLL();
                string strObjectSQL = "from WZObject as wZObject where ObjectCode = '" + arrArges[1] + "'";
                IList listObject = wZObjectBLL.GetAllWZObjects(strObjectSQL);
                if (listObject != null && listObject.Count > 0)
                {
                    WZObject wZObject = (WZObject)listObject[0];
                    HF_ConvertRatio.Value = wZObject.ConvertRatio.ToString();//换算系数
                    HF_Market.Value = wZObject.Market.ToString();//市场行情
                    HF_PickingPlanDetailID.Value = arrArges[0];

                    TXT_PlanNumber.Text = arrArges[2];
                    decimal decimalResult = 0;
                    if (HF_ConvertRatio.Value != "0" && HF_ConvertRatio.Value != "0.00")
                    {
                        decimalResult = decimal.Parse(TXT_PlanNumber.Text) / decimal.Parse(HF_ConvertRatio.Value);
                    }

                    TXT_ConvertPlanNumber.Text = decimalResult.ToString("#0.00");
                    TXT_Remark.Text = arrArges[3];
                }
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                int intPickingPlanDetailID = 0;
                int.TryParse(cmdArges, out intPickingPlanDetailID);

                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + intPickingPlanDetailID;
                IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                {
                    WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
                    wZPickingPlanDetailBLL.DeleteWZPickingPlanDetail(wZPickingPlanDetail);

                    //重新加载列表
                    DataPickingPlanDetailBinder(wZPickingPlanDetail.PlanCode);

                    //修改领料计划的条数
                    //string strPickingPlanNumberHQL = "update T_WZPickingPlan set DetailCount = DetailCount -1 where PlanCode = '" + wZPickingPlanDetail.PlanCode + "'";
                    //ShareClass.RunSqlCommand(strPickingPlanNumberHQL);
                    //计划数量、预计费用写入计划里面
                    string strTotalPickingPlanDetailHQL = string.Format(@"select COUNT(1) as RowNumber,SUM(PlanCost) as TotalPlanCost from T_WZPickingPlanDetail
                                    where PlanCode = '{0}'
                                    group by PlanCode", wZPickingPlanDetail.PlanCode);
                    DataTable dtTotalPlan = ShareClass.GetDataSetFromSql(strTotalPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];
                    int intDetailCount = 0;
                    decimal decimalPlanCost = 0;
                    if (dtTotalPlan != null && dtTotalPlan.Rows.Count > 0)
                    {
                        int.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["RowNumber"]), out intDetailCount);
                        decimal.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["TotalPlanCost"]), out decimalPlanCost);
                    }
                    string strUpdatePickPlanHQL = string.Format(@"update T_WZPickingPlan 
                                        set DetailCount = {0},
                                            PlanCost={1} 
                                        where PlanCode='{2}'", intDetailCount, decimalPlanCost, wZPickingPlanDetail.PlanCode);
                    ShareClass.RunSqlCommand(strUpdatePickPlanHQL);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "')", true);
                }
            }
        }
    }


    protected void DG_ObjectList_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "add")
            {
                string cmdArges = e.CommandArgument.ToString();
                string[] arrArges = cmdArges.Split('|');
                WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();

                string strPickingPlan = strPlanCode;

                //判断材料是否已经添加在计划明细里面
                string strCheckPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + strPickingPlan + "' and ObjectCode = '" + arrArges[0] + "'";
                IList lstCheckPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strCheckPlanDetailHQL);
                if (lstCheckPlanDetail != null && lstCheckPlanDetail.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZGCLZJHMXZYJCZBNZTJ + "')", true);
                    return;
                }

                string strPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanCode = '" + strPickingPlan + "'";
                IList listPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strPickingPlanHQL);
                if (listPickingPlan != null && listPickingPlan.Count > 0)
                {
                    WZPickingPlan wZPickingPlan = (WZPickingPlan)listPickingPlan[0];

                    WZPickingPlanDetail wZPickingPlanDetail = new WZPickingPlanDetail();
                    wZPickingPlanDetail.PlanCode = wZPickingPlan.PlanCode;
                    wZPickingPlanDetail.ObjectCode = arrArges[0];
                    wZPickingPlanDetail.OldCode = "";
                    wZPickingPlanDetail.Progress = "录入";
                    wZPickingPlanDetail.IsMark = 0;
                    

                    //增加物资到领料计划明细
                    wZPickingPlanDetailBLL.AddWZPickingPlanDetail(wZPickingPlanDetail);

                    //修改领料计划的条数
                    string strPickingPlanNumberHQL = "update T_WZPickingPlan set DetailCount = DetailCount +1,IsMark = -1 where PlanCode = '" + wZPickingPlan.PlanCode + "'";
                    ShareClass.RunSqlCommand(strPickingPlanNumberHQL);

                    //修改物资代码的使用标记
                    string strOjbectHQL = "update T_WZObject set IsMark = -1 where ObjectCode = '" + arrArges[0] + "'";
                    ShareClass.RunSqlCommand(strOjbectHQL);

                    //重新加载领料计划明细
                    DataPickingPlanDetailBinder(wZPickingPlan.PlanCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('添加成功！');ControlStatusCloseChange();", true);
                }
            }
        }
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
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }
    

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strObjectSQL = @"select o.*,u.UnitName,c.UnitName as ConvertUnitName from T_WZObject o
                        left join T_WZSpan u on o.Unit = u.ID
                        left join T_WZSpan c on o.ConvertUnit = c.ID
                        where 1=1 ";
        strObjectSQL += " and o.ObjectCode Like '%" + TB_WZCode.Text.Trim() + "%'";
        strObjectSQL += " and o.ObjectName Like '%" + TB_WZName.Text.Trim() + "%'";
        strObjectSQL += " and o.Grade Like '%" + TB_WZGrade.Text.Trim() + "'";
        strObjectSQL += " and o.Criterion Like '%" + TB_WZCriterion.Text.Trim() + "%'";
        strObjectSQL += " and o.Model Like '%" + TB_WZModel.Text.Trim() + "%'";

        //Label1.Text = strObjectSQL;

        DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

        DG_ObjectList.DataSource = dtObject;
        DG_ObjectList.DataBind();
    }

    protected void btnPlan_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(HF_PickingPlanDetailID.Value))
            {
                string strPlanNumber = TXT_PlanNumber.Text.Trim();
                if (string.IsNullOrEmpty(strPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXTXJHSL + "')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSLBXWXSHZZS + "')", true);
                    return;
                }

                decimal deciamlPlanNumber = 0;
                decimal.TryParse(strPlanNumber, out deciamlPlanNumber);

                decimal decimalConvertRatio = 0;
                decimal.TryParse(HF_ConvertRatio.Value, out decimalConvertRatio);

                decimal decimalResult = 0;
                if (decimalConvertRatio != 0)
                {
                    decimalResult = deciamlPlanNumber / decimalConvertRatio;
                }
                TXT_ConvertPlanNumber.Text = decimalResult.ToString("#0.00");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZJHMX + "')", true);
                return;
            }
        }
        catch (Exception ex)
        { }
    }

    protected void btnConvert_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(HF_PickingPlanDetailID.Value))
            {
                string strConvertPlanNumber = TXT_ConvertPlanNumber.Text.Trim();
                if (string.IsNullOrEmpty(strConvertPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXTXJHSL + "')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strConvertPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSLBXWXSHZZS + "')", true);
                    return;
                }

                decimal deciamlConvertPlanNumber = 0;
                decimal.TryParse(strConvertPlanNumber, out deciamlConvertPlanNumber);

                decimal decimalConvertRatio = 0;
                decimal.TryParse(HF_ConvertRatio.Value, out decimalConvertRatio);

                decimal decimalResult = deciamlConvertPlanNumber * decimalConvertRatio;
                TXT_PlanNumber.Text = decimalResult.ToString("#0.00");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZJHMX + "')", true);
                return;
            }
        }
        catch (Exception ex)
        { }
    }

    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJHMX + "')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewIsMark = HF_NewIsMark.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPlanDetailEdit.aspx?id=" + strEditID + "');ControlStatusChange('" + strNewProgress + "','" + strNewIsMark + "');", true);
        return;
    }

    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditID = HF_NewID.Value;
        if (string.IsNullOrEmpty(strEditID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXDJYCZDJHMX + "')", true);
            return;
        }

        string strNewProgress = HF_NewProgress.Value;
        string strNewIsMark = HF_NewIsMark.Value;

        int intPickingPlanDetailID = 0;
        int.TryParse(strEditID, out intPickingPlanDetailID);

        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
        string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + intPickingPlanDetailID;
        IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
        if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
        {
            WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
            wZPickingPlanDetailBLL.DeleteWZPickingPlanDetail(wZPickingPlanDetail);

            //重新加载列表
            DataPickingPlanDetailBinder(wZPickingPlanDetail.PlanCode);

            //修改领料计划的条数
            //string strPickingPlanNumberHQL = "update T_WZPickingPlan set DetailCount = DetailCount -1 where PlanCode = '" + wZPickingPlanDetail.PlanCode + "'";
            //ShareClass.RunSqlCommand(strPickingPlanNumberHQL);
            //计划数量、预计费用写入计划里面
            string strTotalPickingPlanDetailHQL = string.Format(@"select COUNT(1) as RowNumber,SUM(PlanCost) as TotalPlanCost from T_WZPickingPlanDetail
                                    where PlanCode = '{0}'
                                    group by PlanCode", wZPickingPlanDetail.PlanCode);
            DataTable dtTotalPlan = ShareClass.GetDataSetFromSql(strTotalPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];
            int intDetailCount = 0;
            decimal decimalPlanCost = 0;
            if (dtTotalPlan != null && dtTotalPlan.Rows.Count > 0)
            {
                int.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["RowNumber"]), out intDetailCount);
                decimal.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["TotalPlanCost"]), out decimalPlanCost);
            }

            int intIsMark = 0;
            if (intDetailCount > 0)
            {
                intIsMark = -1;
            }
            string strUpdatePickPlanHQL = string.Format(@"update T_WZPickingPlan 
                                        set DetailCount = {0},
                                            PlanCost={1},
                                            IsMark = {3}
                                        where PlanCode='{2}'", intDetailCount, decimalPlanCost, wZPickingPlanDetail.PlanCode, intIsMark);
            ShareClass.RunSqlCommand(strUpdatePickPlanHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCCG + "');ControlStatusCloseChange();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
        }
    }


    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        //重新加载计划明细列表
        DataPickingPlanDetailBinder(strPlanCode);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


}