using System; using System.Resources;
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

public partial class TTWZPickingPlanDetailReplace : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (!IsPostBack)
        {
            DataPickingPlanDetailBinder("");
            DataPickingPlanBander();
            LoadObjectTree();
            DataObjectBinder("", "", "");
        }
    }


    private void DataPickingPlanDetailBinder(string strPlanCode)
    {
        DG_PickPlanDetailList.CurrentPageIndex = 0;

        string strWZPickingPlanDetailHQL = string.Format(@"select d.*,o.ObjectName from T_WZPickingPlanDetail d
                                        left join T_WZObject o on d.ObjectCode = o.ObjectCode 
                                        where PlanCode = '{0}'", strPlanCode);
        DataTable dtWZPickingPlanDetail = ShareClass.GetDataSetFromSql(strWZPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];

        DG_PickPlanDetailList.DataSource = dtWZPickingPlanDetail;
        DG_PickPlanDetailList.DataBind();

        LB_PickPlanDetailSql.Text = strWZPickingPlanDetailHQL;
        #region 注释
        //WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
        //string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where PlanCode = '" + strPlanCode +"'";
        //IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);

        //DG_PickPlanDetailList.DataSource = listWZPickingPlanDetail;
        //DG_PickPlanDetailList.DataBind();
        #endregion
    }

    protected void DG_PickPlanDetailList_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_PickPlanDetailList.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_PickPlanDetailSql.Text.Trim();
        DataTable dtWZPickingPlanDetail = ShareClass.GetDataSetFromSql(strHQL, "PickingPlanDetail").Tables[0];

        DG_PickPlanDetailList.DataSource = dtWZPickingPlanDetail;
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

        #region 注释
        //WZObjectBLL wZObjectBLL = new WZObjectBLL();
        //string strObjectSQL = "from WZObject as wZObject where 1=1 ";
        //strObjectSQL += " and DLCode = '" + strDLCode + "'";
        //strObjectSQL += " and ZLCode = '" + strZLCode + "'";
        //strObjectSQL += " and XLCode = '" + strXLCode + "'";
        //IList listObject = wZObjectBLL.GetAllWZObjects(strObjectSQL);

        //DG_ObjectList.DataSource = listObject;
        //DG_ObjectList.DataBind();
        #endregion
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
                TXT_OldObjectCode.Text = arrArges[1];
            }
        }
    }


    protected void DG_ObjectList_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_ObjectList.Items.Count; i++)
            {
                DG_ObjectList.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();
                TXT_ObjectCode.Text = cmdArges;
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


    protected void BT_Replace_Click(object sender, EventArgs e)
    {
        try
        {
            string strObjectCode = TXT_ObjectCode.Text;
            string strOldObjectCode = TXT_OldObjectCode.Text;
            string strPickingPlanDetailID = HF_PickingPlanDetailID.Value;

            if (!string.IsNullOrEmpty(strObjectCode) && !string.IsNullOrEmpty(strOldObjectCode) && !string.IsNullOrEmpty(strPickingPlanDetailID))
            {
                int intPickingPlanDetailID = 0;
                int.TryParse(strPickingPlanDetailID, out intPickingPlanDetailID);

                //查询 计划明细，使用标记是否为0
                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + intPickingPlanDetailID;
                IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                {
                    WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
                    if (wZPickingPlanDetail.IsMark == 0)
                    {
                        wZPickingPlanDetail.ObjectCode = strObjectCode;

                        wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, intPickingPlanDetailID);

                        //重新加载计划明细列表
                        LB_PickingPlan_SelectedIndexChanged(null, null);

                        TXT_OldObjectCode.Text = strObjectCode;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHMXDSYBJBW0BNTH").ToString().Trim()+"')", true);
                        return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZWZDMHJHMX").ToString().Trim()+"')", true);
                return;
            }
        }
        catch (Exception ex) { }
    }
}