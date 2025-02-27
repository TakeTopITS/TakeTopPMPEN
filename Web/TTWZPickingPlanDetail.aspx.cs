using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZPickingPlanDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataPickingPlanDetailBinder("");
            DataPickingPlanBander();
            LoadObjectTree();
            DataObjectBinder("", "", "");
        }
    }


    private void DataPickingPlanDetailBinder(string strPlanCode)
    {
        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
        string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + strPlanCode +"'";
        IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);

        DG_PickPlanDetailList.DataSource = listWZPickingPlanDetail;
        DG_PickPlanDetailList.DataBind();
    }


    private void DataObjectBinder(string strDLCode, string strZLCode, string strXLCode)
    {
        string strObjectSQL = string.Format(@"select o.*,u.UnitName as UnitName,c.UnitName as ConvertUnitName from T_WZObject o
                            left join T_WZSpan u on o.Unit = u.ID
                            left join T_WZSpan c on o.ConvertUnit = c.ID 
                            where 1=1 
                            and o.DLCode = '{0}'
                            and o.ZLCode = '{1}'
                            and o.XLCode = '{2}'", strDLCode, strZLCode, strXLCode);
        DataTable dtObject = ShareClass.GetDataSetFromSql(strObjectSQL, "Object").Tables[0];

        DG_ObjectList.DataSource = dtObject;
        DG_ObjectList.DataBind();
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

    private void DataPickingPlanBander()
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
        string strWZPickingPlanHQL = "from WZPickingPlan as wZPickingPlan where PlanMarker = '" + strUserCode + "' order by MarkerTime desc";
        IList listWZPickingPlan = wZPickingPlanBLL.GetAllWZPickingPlans(strWZPickingPlanHQL);

        LB_PickingPlan.DataSource = listWZPickingPlan;
        LB_PickingPlan.DataBind();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(HF_PickingPlanDetailID.Value))
            {
                int intPickingPlanDetailID = 0;
                int.TryParse(HF_PickingPlanDetailID.Value, out intPickingPlanDetailID);

                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + intPickingPlanDetailID;
                IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                {
                    WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
                    decimal decimalPlanNumber = 0;
                    decimal.TryParse(TXT_PlanNumber.Text.Trim(), out decimalPlanNumber);
                    decimal decimalConvertNumber = 0;
                    decimal.TryParse(TXT_ConvertPlanNumber.Text.Trim(), out decimalConvertNumber);
                    string strRemark = TXT_Remark.Text.Trim();
                    decimal decimalMarket = 0;
                    decimal.TryParse(HF_Market.Value, out decimalMarket);

                    wZPickingPlanDetail.PlanNumber = decimalPlanNumber;
                    wZPickingPlanDetail.ConvertNumber = decimalConvertNumber;
                    wZPickingPlanDetail.Remark = strRemark;
                    wZPickingPlanDetail.PlanCost = decimalPlanNumber * decimalMarket;

                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, intPickingPlanDetailID);


                    LB_PickingPlan_SelectedIndexChanged(null, null);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
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
            if (cmdName == "edit")
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
                    if (HF_ConvertRatio.Value != "0")
                    {
                        TXT_ConvertPlanNumber.Text = (decimal.Parse(TXT_PlanNumber.Text) / decimal.Parse(HF_ConvertRatio.Value)).ToString("#0.00");
                    }
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
                    string strPickingPlanNumberHQL = "update T_WZPickingPlan set DetailCount = DetailCount -1 where PlanCode = '" + wZPickingPlanDetail.PlanCode + "'";
                    ShareClass.RunSqlCommand(strPickingPlanNumberHQL);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
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
                if (!string.IsNullOrEmpty(LB_PickingPlan.SelectedValue))
                {
                    string cmdArges = e.CommandArgument.ToString();
                    string[] arrArges = cmdArges.Split('|');
                    WZPickingPlanBLL wZPickingPlanBLL = new WZPickingPlanBLL();
                    WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                    string strPickingPlan = LB_PickingPlan.SelectedValue;

                    //判断材料是否已经添加在计划明细里面
                    string strCheckPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + strPickingPlan + "' and ObjectCode = '" + arrArges[0] + "'";
                    IList lstCheckPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strCheckPlanDetailHQL);
                    if (lstCheckPlanDetail != null && lstCheckPlanDetail.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGCLZJHMXZYJCZBNZTJ").ToString().Trim()+"')", true);
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
                        wZPickingPlanDetail.OldCode = arrArges[0];
                        wZPickingPlanDetail.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        wZPickingPlanDetail.IsMark = 0;

                        //增加物资到领料计划明细
                        wZPickingPlanDetailBLL.AddWZPickingPlanDetail(wZPickingPlanDetail);
                        //修改领料计划的条数
                        string strPickingPlanNumberHQL = "update T_WZPickingPlan set DetailCount = DetailCount +1 where PlanCode = '" + wZPickingPlan.PlanCode + "'";
                        ShareClass.RunSqlCommand(strPickingPlanNumberHQL);
                        //修改物资代码的使用标记
                        string strOjbectHQL = "update T_WZObject set IsMark = -1 where ObjectCode = '" + arrArges[0] + "'";
                        ShareClass.RunSqlCommand(strOjbectHQL);
                        //重新加载领料计划明细
                        DataPickingPlanDetailBinder(wZPickingPlan.PlanCode);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZJH").ToString().Trim()+"')", true);
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
    }



    
    protected void LB_PickingPlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPickPlanID = LB_PickingPlan.SelectedValue;
        if (!string.IsNullOrEmpty(strPickPlanID))
        {
            DataPickingPlanDetailBinder(strPickPlanID);
        }
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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXTXJHSL").ToString().Trim()+"')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHSLBXWXSHZZS").ToString().Trim()+"')", true);
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
            else {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZJHMX").ToString().Trim()+"')", true);
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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXTXJHSL").ToString().Trim()+"')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strConvertPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHSLBXWXSHZZS").ToString().Trim()+"')", true);
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZJHMX").ToString().Trim()+"')", true);
                return;
            }
        }
        catch (Exception ex)
        { }
    }
}