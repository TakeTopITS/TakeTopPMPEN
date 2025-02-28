using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Data;
using System.Drawing;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPlanDetailDetail : System.Web.UI.Page
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

            LoadObjectTree();
            DataObjectBinder("", "", "");
        }
    }


    private void DataPickingPlanDetailBinder(string strPlanCode)
    {
        DG_PickPlanDetailList.CurrentPageIndex = 0;

        string strWZPickingPlanDetailHQL = string.Format(@"select d.*,o.ObjectName,o.Model,o.Criterion,o.Grade,s.UnitName from T_WZPickingPlanDetail d
                                        left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                                        left join T_WZSpan s on o.Unit = s.ID
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
        string strObjectSQL = @"select o.*,u.UnitName,c.UnitName as ConvertUnitName,p.UserName as CreaterName
                        from T_WZObject o
                        left join T_WZSpan u on o.Unit = u.ID
                        left join T_WZSpan c on o.ConvertUnit = c.ID
                        left join T_ProjectMember p on o.Creater = p.UserCode
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

                string strID = arrArges[0];
                string strObjectCode = arrArges[1].Trim();

                TXT_ObjectCode.Text = strObjectCode;

                HF_PickingPlanDetailID.Value = strID;
                HF_ObjectCode.Value = strObjectCode;
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
                for (int i = 0; i < DG_ObjectList.Items.Count; i++)
                {
                    DG_ObjectList.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();

                TXT_NewObjectCode.Text = cmdArges;
                HF_NewObjectCode.Value = cmdArges;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strObjectCode = TXT_ObjectCode.Text.Trim();
            string strNewObjectCode = TXT_NewObjectCode.Text.Trim();

            if (string.IsNullOrEmpty(strObjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZYJHMXLB + "')", true);
                return;
            }
            if (string.IsNullOrEmpty(strNewObjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZZWZLBXDWZDM + "')", true);
                return;
            }

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

                    wZPickingPlanDetail.ObjectCode = strNewObjectCode;

                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, intPickingPlanDetailID);

                    //修改物资代码的使用标记
                    string strOjbectHQL = "update T_WZObject set IsMark = -1 where ObjectCode = '" + strNewObjectCode + "'";
                    ShareClass.RunSqlCommand(strOjbectHQL);

                    //重新加载计划明细列表
                    DataPickingPlanDetailBinder(strPlanCode);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZTHCG + "')", true);
                }
            }
        }
        catch (Exception ex) { }
    }



    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        //取消
        for (int i = 0; i < DG_PickPlanDetailList.Items.Count; i++)
        {
            DG_PickPlanDetailList.Items[i].ForeColor = Color.Black;
        }

        for (int i = 0; i < DG_ObjectList.Items.Count; i++)
        {
            DG_ObjectList.Items[i].ForeColor = Color.Black;
        }


        TXT_ObjectCode.Text = "";
        TXT_NewObjectCode.Text = "";
    }
}