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
using System.Text;
using System.Drawing;

public partial class TTWZSendMaterialList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            BindStockData();
            DataProjectBinder();
            DataSendBinder();
            LoadObjectTree();

            DG_Store.DataSource = "";
            DG_Store.DataBind();
        }
    }

    private void DataSendBinder()
    {
        DG_Send.CurrentPageIndex = 0;

        string strSendHQL = string.Format(@"select s.*,d.PlanCode,o.ObjectName,
                                c.UserName as CheckerName,
                                f.UserName as SafekeeperName,
                                p.UserName as PurchaseEngineerName
                                from T_WZSend s
                                left join T_WZPickingPlanDetail d on s.PlanDetaiID = d.ID
                                left join T_WZObject o on s.ObjectCode = o.ObjectCode
                                left join T_ProjectMember c on s.Checker = c.UserCode
                                left join T_ProjectMember f on s.Safekeeper = f.UserCode
                                left join T_ProjectMember p on s.PurchaseEngineer = p.UserCode
                                and s.Progress = '录入' 
                                order by s.TicketTime desc", strUserCode);
        DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];

        DG_Send.DataSource = dtSend;
        DG_Send.DataBind();

        LB_SendSql.Text = strSendHQL;
        #region 注释
        //WZSendBLL wZSendBLL = new WZSendBLL();
        //string strSendHQL = string.Format("from WZSend as wZSend where PurchaseEngineer ='{0}' and Progress = '录入' order by TicketTime desc", strUserCode);
        //IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
        //DG_Send.DataSource = listSend;
        //DG_Send.DataBind();

        //LB_SendSql.Text = strSendHQL;
        #endregion
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
        TV_Type.Nodes.Add(Node);
    }



    private void DataStoreBinder(string strStoreRoom, string strXLCode)
    {
        string strStoreHQL = string.Format(@"select s.*,
            o.ObjectName,o.Model,o.Criterion,o.Grade,o.DLCode,o.ZLCode,o.XLCode from T_WZObject o
            left join T_WZStore s on s.ObjectCode = o.ObjectCode
            where s.StockCode = '{0}'
            and o.XLCode = '{1}'", strStoreRoom, strXLCode);
        DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];

        DG_Store.DataSource = dtStore;
        DG_Store.DataBind();
    }


    private void DataProjectBinder()
    {
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject order by MarkTime desc";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        DDL_Project.DataSource = listProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }

    private void BindStockData()
    {
        WZStockBLL wZStockBLL = new WZStockBLL();
        string strStockHQL = "from WZStock as wZStock";
        IList lstStock = wZStockBLL.GetAllWZStocks(strStockHQL);

        DDL_StoreRoom.DataSource = lstStock;
        DDL_StoreRoom.DataBind();

        DDL_StoreRoom.Items.Insert(0, new ListItem("-", ""));
    }


    protected void TV_Type_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strSendCode = HF_SendCode.Value;
        string strStoreRoom = HF_StoreRoom.Value;
        if (!string.IsNullOrEmpty(strSendCode))
        {
            if (TV_Type.SelectedNode != null)
            {
                string strSelectedValue = TV_Type.SelectedNode.Value;
                string[] arrSelectedValue = strSelectedValue.Split('|');
                if (arrSelectedValue[0] != "all")
                {
                    DataStoreBinder(strStoreRoom, arrSelectedValue[2]);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJFLD+"')", true);
            return;
        }
    }


    protected void DDL_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectSelectedValue = DDL_Project.SelectedValue;
        if (!string.IsNullOrEmpty(strProjectSelectedValue))
        {
            WZProjectBLL wZProjectBLL = new WZProjectBLL();
            string strProjectHQL = "from WZProject as wZProject where ProjectCode = '" + strProjectSelectedValue + "'";
            IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);
            if (listProject != null && listProject.Count > 0)
            {
                WZProject wZProject = (WZProject)listProject[0];
                DDL_StoreRoom.SelectedValue = wZProject.StoreRoom;
                //TXT_UnitCode.Text = wZProject.UnitCode;
                //TXT_PickingUnit.Text = wZProject.PickingUnit;
                TXT_Checker.Text = wZProject.Checker;
                TXT_Safekeeper.Text = wZProject.Safekeep;

                //依据<单位编号>从领料单位表单带入<主管领导>
                //string strGetUnitHQL = "from WZGetUnit as wZGetUnit where UnitCode = '" + wZProject.UnitCode + "'";
                //WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
                //IList listGetUnit = wZGetUnitBLL.GetAllWZGetUnits(strGetUnitHQL);
                //if (listGetUnit != null && listGetUnit.Count > 0)
                //{
                //    WZGetUnit wZGetUnit = (WZGetUnit)listGetUnit[0];
                //    TXT_UpLeader.Text = wZGetUnit.Leader;
                //}
            }
        }
    }


    protected void DDL_PickingUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strUnitSelectedValue = DDL_PickingUnit.SelectedValue;
        if (!string.IsNullOrEmpty(strUnitSelectedValue))
        {
            string strGetUnitHQL = "from WZGetUnit as wZGetUnit where UnitCode = '" + strUnitSelectedValue + "'";
            WZGetUnitBLL wZGetUnitBLL = new WZGetUnitBLL();
            IList listGetUnit = wZGetUnitBLL.GetAllWZGetUnits(strGetUnitHQL);
            if (listGetUnit != null && listGetUnit.Count > 0)
            {
                WZGetUnit wZGetUnit = (WZGetUnit)listGetUnit[0];
                TXT_UpLeader.Text = wZGetUnit.Leader;
            }
        }
    }


    protected void DG_Send_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress != "录入" || wZSend.PurchaseEngineer != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBWLRYJHTYBWDDLYHSBYXSC+"')", true);
                        return;
                    }

                    wZSendBLL.DeleteWZSend(wZSend);

                    //重新加载列表
                    DataSendBinder();
                }

            }
            else if (cmdName == "edit")
            {
                for (int i = 0; i < DG_Send.Items.Count; i++)
                {
                    DG_Send.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];

                    HF_SendCode.Value = wZSend.SendCode;
                    TXT_SendCode.Text = wZSend.SendCode;

                    TXT_ObjectCode.Text = wZSend.ObjectCode;
                    TXT_TicketTime.Text = wZSend.TicketTime.ToString("yyyy-MM-dd");
                    DDL_SendMethod.SelectedValue = wZSend.SendMethod;    //发料方式
                    DDL_Project.SelectedValue = wZSend.ProjectCode;          //项目编码
                    DDL_StoreRoom.SelectedValue = wZSend.StoreRoom;          //库别
                    //TXT_UnitCode.Text = wZSend.UnitCode;              //单位编号
                    DDL_PickingUnit.SelectedValue = wZSend.UnitCode;              //单位编号
                    TXT_PickingUnit.Text = wZSend.PickingUnit;        //领料单位
                    TXT_Checker.Text = wZSend.Checker;                //材检员
                    TXT_Safekeeper.Text = wZSend.Safekeeper;          //保管员
                    TXT_UpLeader.Text = wZSend.UpLeader;              //主管领导
                    TXT_ActualNumber.Text = wZSend.ActualNumber.ToString();  //实发数量
                    TXT_PlanPrice.Text = wZSend.PlanPrice.ToString();        //计划单价
                    TXT_PlanMoney.Text = wZSend.PlanMoney.ToString();        //计划金额
                    TXT_DownMoney.Text = wZSend.DownMoney.ToString();        //减值金额
                    TXT_CleanMoney.Text = wZSend.CleanMoney.ToString();      //净额
                    TXT_ReduceCode.Text = wZSend.ReduceCode;      //减值编号
                    TXT_WearyCode.Text = wZSend.WearyCode;        //积压编号
                    TXT_CheckCode.Text = wZSend.CheckCode;        //检号
                    TXT_GoodsCode.Text = wZSend.GoodsCode;        //货位号
                    TXT_ManageRate.Text = wZSend.ManageRate.ToString();      //管理费率

                    HF_StoreRoom.Value = wZSend.StoreRoom;

                }
            }
            else if (cmdName == "submit")
            {
                //提交
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress == "录入")
                    {
                        string strCheckCode = wZSend.CheckCode;

                        //材检
                        wZSend.Progress = "材检";


                        wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                        //重新加载收料单列表
                        DataSendBinder();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBWLRBNTJ+"')", true);
                        return;
                    }
                }
            }
        }
    }


    protected void DG_Store_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            //库存点击
            string cmdName = e.CommandName;
            if (cmdName == "add")
            {
                string strSendCode = HF_SendCode.Value;
                if (!string.IsNullOrEmpty(strSendCode))
                {
                    string cmdArges = e.CommandArgument.ToString();


                    WZStoreBLL wZStoreBLL = new WZStoreBLL();
                    string strWZStoreHQL = "from WZStore as wZStore where ID = " + cmdArges;
                    IList lstStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                    if (lstStore != null && lstStore.Count > 0)
                    {
                        WZStore wZStore = (WZStore)lstStore[0];
                        //发料单〈计划单价〉＝库存〈库存单价〉												
                        //发料单〈计划金额〉＝〈实发数量〉×〈计划单价〉												
                        //发料单〈减值金额〉＝库存〈减值比例〉×〈计划金额〉												
                        //发料单〈净额〉＝〈计划金额〉－〈减值金额〉												
                        //发料单〈减值编号〉＝库存〈减值编号〉												
                        //发料单〈积压编号〉＝库存〈积压编号〉												
                        //发料单〈检号〉＝库存〈检号〉												
                        //发料单〈货位号〉＝库存〈货位号〉												
                        //TXT_PlanPrice.Text = wZStore.StorePrice.ToString();
                        //decimal decimalActualNumber = 0;
                        //decimal.TryParse(TXT_ActualNumber.Text.Trim(), out decimalActualNumber);
                        //decimal decimalPlanMoney = decimalActualNumber * wZStore.StorePrice;
                        //TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.00");
                        //HF_DownRatio.Value = wZStore.DownRatio.ToString();
                        //decimal decimalDownMoney = wZStore.DownRatio * decimalPlanMoney;
                        //TXT_DownMoney.Text = decimalDownMoney.ToString("#0,00");
                        //TXT_CleanMoney.Text = (decimalPlanMoney - decimalDownMoney).ToString();
                        //TXT_ReduceCode.Text = wZStore.DownCode;
                        //TXT_WearyCode.Text = wZStore.WearyCode;
                        //TXT_CheckCode.Text = wZStore.CheckCode;
                        //TXT_GoodsCode.Text = wZStore.GoodsCode;


                        //增加发料单
                        WZSend wZSend = new WZSend();
                        wZSend.SendCode = CreateNewSendCode();          //发料编号
                        wZSend.TicketTime = DateTime.Now;
                        wZSend.PurchaseEngineer = strUserCode;
                        wZSend.Progress = "录入";
                        wZSend.IsMark = 0;
                        wZSend.SendMethod = "蓝票";
                        wZSend.PlanDetaiID = 0;

                        //wZSend.StoreRoom = wZStore.StockCode;
                        wZSend.ObjectCode = wZStore.ObjectCode;
                        wZSend.PlanNumber = wZStore.StoreNumber;
                        wZSend.ActualNumber = wZSend.PlanNumber;
                        wZSend.PlanPrice = wZSend.PlanPrice;
                        wZSend.PlanMoney = wZSend.ActualNumber * wZSend.PlanPrice;
                        wZSend.DownMoney = wZStore.DownRatio * wZSend.PlanMoney;
                        wZSend.CleanMoney = wZSend.PlanMoney - wZStore.DownMoney;
                        wZSend.ReduceCode = wZStore.DownCode;
                        wZSend.WearyCode = wZStore.WearyCode;
                        wZSend.CheckCode = wZStore.CheckCode;
                        wZSend.GoodsCode = wZStore.GoodsCode;

                        //发料单〈材检日期〉〈发料日期〉＝“-”
                        wZSend.CheckTime = "-";//DateTime.Now;
                        wZSend.SendTime = "-"; //DateTime.Now;

                        WZSendBLL wZSendBLL = new WZSendBLL();
                        wZSendBLL.AddWZSend(wZSend);

                        for (int i = 0; i < DG_Store.Items.Count; i++)
                        {
                            DG_Store.Items[i].ForeColor = Color.Black;
                        }

                        e.Item.ForeColor = Color.Red;


                        //重新加载发料单列表
                        DataSendBinder();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXDJFLD+"')", true);
                    return;
                }
            }
        }
    }

    protected void BT_Calc_Click(object sender, EventArgs e)
    {
        string strActualNumber = TXT_ActualNumber.Text.Trim();
        if (!string.IsNullOrEmpty(strActualNumber))
        {
            if (!ShareClass.CheckIsNumber(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSFSLBXWXSHZZS+"')", true);
                return;
            }
            else
            {
                decimal decimalActualNumber = 0;
                decimal.TryParse(strActualNumber, out decimalActualNumber);
                decimal decimalPlanPrice = 0;
                decimal.TryParse(TXT_PlanPrice.Text.Trim(), out decimalPlanPrice);
                decimal decimalPlanMoney = decimalActualNumber * decimalPlanPrice;
                TXT_PlanMoney.Text = decimalPlanMoney.ToString("#0.00");
                decimal decimalDownRatio = 0;
                decimal.TryParse(HF_DownRatio.Value, out decimalDownRatio);
                decimal decimalDownMoney = decimalDownRatio * decimalPlanMoney;
                TXT_DownMoney.Text = decimalDownMoney.ToString("#0.00");
                TXT_CleanMoney.Text = (decimalPlanMoney - decimalDownMoney).ToString();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSFSLBNWK+"')", true);
            return;
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strSendMethod = DDL_SendMethod.SelectedValue;        //发料方式
        string strProjectCode = DDL_Project.SelectedValue;          //项目编码
        string strStoreRoom = DDL_StoreRoom.SelectedValue;          //库别
        //string strUnitCode = TXT_UnitCode.Text.Trim();              //单位编号
        string strUnitCode = DDL_PickingUnit.SelectedValue;              //单位编号
        string strPickingUnit = TXT_PickingUnit.Text.Trim();        //领料单位
        string strChecker = TXT_Checker.Text.Trim();                //材检员
        string strSafekeeper = TXT_Safekeeper.Text.Trim();          //保管员
        string strUpLeader = TXT_UpLeader.Text.Trim();              //主管领导


        if (string.IsNullOrEmpty(strSendMethod))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFLFSBNWKBC+"')", true);
            return;
        }
        if (string.IsNullOrEmpty(strProjectCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZZXMBM+"')", true);
            return;
        }

        

        string strSendCode = HF_SendCode.Value;
        if (!string.IsNullOrEmpty(strSendCode))
        {
            //修改
            string strActualNumber = TXT_ActualNumber.Text.Trim();  //实发数量
            string strPlanPrice = TXT_PlanPrice.Text.Trim();        //计划单价
            string strPlanMoney = TXT_PlanMoney.Text.Trim();        //计划金额
            string strDownMoney = TXT_DownMoney.Text.Trim();        //减值金额
            string strCleanMoney = TXT_CleanMoney.Text.Trim();      //净额
            string strReduceCode = TXT_ReduceCode.Text.Trim();      //减值编号
            string strWearyCode = TXT_WearyCode.Text.Trim();        //积压编号
            string strCheckCode = TXT_CheckCode.Text.Trim();        //检号
            string strGoodsCode = TXT_GoodsCode.Text.Trim();        //货位号
            string strManageRate = TXT_ManageRate.Text.Trim();      //管理费率


            if (string.IsNullOrEmpty(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSSSLBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSSSLBXWXSHZZS+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strManageRate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGLFLBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strManageRate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGLFLBXWXSHZZS+"')", true);
                return;
            }
            
            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal deciamlManageRate = 0;
            decimal.TryParse(strManageRate, out deciamlManageRate);

            WZSendBLL wZSendBLL = new WZSendBLL();
            string strWZSendHQL = "from WZSend as wZSend where SendCode = '" + strSendCode + "'";
            IList listWZSend = wZSendBLL.GetAllWZSends(strWZSendHQL);
            if (listWZSend != null && listWZSend.Count == 1)
            {
                WZSend wZSend = (WZSend)listWZSend[0];

                wZSend.SendMethod = strSendMethod;
                wZSend.ProjectCode = strProjectCode;
                wZSend.StoreRoom = strStoreRoom;
                wZSend.UnitCode = strUnitCode;
                wZSend.PickingUnit = strPickingUnit;
                wZSend.Checker = strChecker;
                wZSend.Safekeeper = strSafekeeper;
                wZSend.UpLeader = strUpLeader;

                wZSend.ActualNumber = decimalActualNumber;
                wZSend.PlanPrice = decimal.Parse(strPlanPrice);
                wZSend.PlanMoney = decimal.Parse(strPlanMoney);
                wZSend.DownMoney = decimal.Parse(strDownMoney);
                wZSend.CleanMoney = decimal.Parse(strCleanMoney);
                wZSend.ReduceCode = strReduceCode;
                wZSend.WearyCode = strWearyCode;
                wZSend.CheckCode = strCheckCode;
                wZSend.GoodsCode = strGoodsCode;

                wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                //重新加载发料单列表
                DataSendBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
            }
        }
        else
        {
            //新增
            WZSendBLL wZSendBLL = new WZSendBLL();
            //增加发料单
            WZSend wZSend = new WZSend();
            wZSend.SendCode = CreateNewSendCode();          //发料编号
            wZSend.TicketTime = DateTime.Now;
            wZSend.PurchaseEngineer = strUserCode;
            wZSend.Progress = "录入";
            wZSend.IsMark = 0;
            wZSend.SendMethod = strSendMethod;
            wZSend.ProjectCode = strProjectCode;
            wZSend.StoreRoom = strStoreRoom;
            wZSend.UnitCode = strUnitCode;
            wZSend.PickingUnit = strPickingUnit;
            wZSend.Checker = strChecker;
            wZSend.Safekeeper = strSafekeeper;
            wZSend.UpLeader = strUpLeader;

            wZSend.PlanDetaiID = 0;         //无计划编号
            wZSend.CheckTime = "-"; //DateTime.Now;
            wZSend.SendTime = "-"; //DateTime.Now;

            wZSendBLL.AddWZSend(wZSend);

            //重新加载发料单列表
            DataSendBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);

        }
    }

    protected void BT_Reset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_Send.Items.Count; i++)
        {
            DG_Send.Items[i].ForeColor = Color.Black;
        }

        for (int i = 0; i < DG_Store.Items.Count; i++)
        {
            DG_Store.Items[i].ForeColor = Color.Black;
        }

        DDL_SendMethod.SelectedValue = "";           //发料方式
        TXT_ActualNumber.Text = "0";                 //实发数量
        TXT_PlanPrice.Text = "0";                     //计划单价
        TXT_PlanMoney.Text = "0";                    //计划金额
        TXT_DownMoney.Text = "0";                 //减值金额
        TXT_CleanMoney.Text = "0";               //净额
        TXT_ReduceCode.Text = "";                //减值编号
        TXT_WearyCode.Text = "";                  //积压编号
        TXT_CheckCode.Text = "";                  //检号
        TXT_GoodsCode.Text = "";                  //货位号
        TXT_ManageRate.Text = "0";                //管理费率
    }


    /// <summary>
    ///  生成发料单Code
    /// </summary>
    private string CreateNewSendCode()
    {
        string strNewSendCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strSendCodeHQL = string.Format("select count(1) as RowNumber from T_WZSend where to_char( TicketTime, 'yyyy-mm-dd' ) like '{0}%'", DateTime.Now.ToString("yyyy-MM"));
                DataTable dtSendCode = ShareClass.GetDataSetFromSql(strSendCodeHQL, "SendCode").Tables[0];
                int intSendCodeNumber = int.Parse(dtSendCode.Rows[0]["RowNumber"].ToString());
                intSendCodeNumber = intSendCodeNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbSendCode = new StringBuilder();
                    for (int j = 3 - intSendCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbSendCode.Append(" ");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewSendCode = strYear + "" + strMonth + "-" + sbSendCode.ToString() + intSendCodeNumber.ToString();

                    //验证新的发料单Code是否存在
                    string strCheckNewSendCodeHQL = "select count(1) as RowNumber from T_WZSend where SendCode = '" + strNewSendCode + "'";
                    DataTable dtCheckNewSendCode = ShareClass.GetDataSetFromSql(strCheckNewSendCodeHQL, "CheckNewSendCode").Tables[0];
                    int intCheckNewSendCode = int.Parse(dtCheckNewSendCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewSendCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intSendCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewSendCode;
    }

    /// <summary>
    /// 获得物资代码的换算系数
    /// </summary>
    private decimal GetConvertRatioByObjectCode(string strObjectCode)
    {
        decimal decimalResult = 0;
        string strObjectConvertRatioHQL = string.Format("select ConvertRatio from T_WZObject where ObjectCode = '{0}'", strObjectCode);
        DataTable dtObjectConvertRatio = ShareClass.GetDataSetFromSql(strObjectConvertRatioHQL, "Market").Tables[0];
        if (dtObjectConvertRatio != null && dtObjectConvertRatio.Rows.Count > 0)
        {
            decimal.TryParse(ShareClass.ObjectToString(dtObjectConvertRatio.Rows[0]["ConvertRatio"]), out decimalResult);
        }
        return decimalResult;
    }

    protected void DG_Send_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DG_Send.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SendSql.Text;

        DataTable dtSend = ShareClass.GetDataSetFromSql(strHQL, "Send").Tables[0];

        DG_Send.DataSource = dtSend;
        DG_Send.DataBind();
    }
}