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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ڳ����ݵ���", strUserCode);
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
                                and s.Progress = '¼��' 
                                order by s.TicketTime desc", strUserCode);
        DataTable dtSend = ShareClass.GetDataSetFromSql(strSendHQL, "Send").Tables[0];

        DG_Send.DataSource = dtSend;
        DG_Send.DataBind();

        LB_SendSql.Text = strSendHQL;
        #region ע��
        //WZSendBLL wZSendBLL = new WZSendBLL();
        //string strSendHQL = string.Format("from WZSend as wZSend where PurchaseEngineer ='{0}' and Progress = '¼��' order by TicketTime desc", strUserCode);
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
        Node.Text = "ȫ������";
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

                //����<��λ���>�����ϵ�λ������<�����쵼>
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
                    if (wZSend.Progress != "¼��" || wZSend.PurchaseEngineer != strUserCode)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJDBWLRYJHTYBWDDLYHSBYXSC+"')", true);
                        return;
                    }

                    wZSendBLL.DeleteWZSend(wZSend);

                    //���¼����б�
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
                    DDL_SendMethod.SelectedValue = wZSend.SendMethod;    //���Ϸ�ʽ
                    DDL_Project.SelectedValue = wZSend.ProjectCode;          //��Ŀ����
                    DDL_StoreRoom.SelectedValue = wZSend.StoreRoom;          //���
                    //TXT_UnitCode.Text = wZSend.UnitCode;              //��λ���
                    DDL_PickingUnit.SelectedValue = wZSend.UnitCode;              //��λ���
                    TXT_PickingUnit.Text = wZSend.PickingUnit;        //���ϵ�λ
                    TXT_Checker.Text = wZSend.Checker;                //�ļ�Ա
                    TXT_Safekeeper.Text = wZSend.Safekeeper;          //����Ա
                    TXT_UpLeader.Text = wZSend.UpLeader;              //�����쵼
                    TXT_ActualNumber.Text = wZSend.ActualNumber.ToString();  //ʵ������
                    TXT_PlanPrice.Text = wZSend.PlanPrice.ToString();        //�ƻ�����
                    TXT_PlanMoney.Text = wZSend.PlanMoney.ToString();        //�ƻ����
                    TXT_DownMoney.Text = wZSend.DownMoney.ToString();        //��ֵ���
                    TXT_CleanMoney.Text = wZSend.CleanMoney.ToString();      //����
                    TXT_ReduceCode.Text = wZSend.ReduceCode;      //��ֵ���
                    TXT_WearyCode.Text = wZSend.WearyCode;        //��ѹ���
                    TXT_CheckCode.Text = wZSend.CheckCode;        //���
                    TXT_GoodsCode.Text = wZSend.GoodsCode;        //��λ��
                    TXT_ManageRate.Text = wZSend.ManageRate.ToString();      //�������

                    HF_StoreRoom.Value = wZSend.StoreRoom;

                }
            }
            else if (cmdName == "submit")
            {
                //�ύ
                string cmdArges = e.CommandArgument.ToString();
                WZSendBLL wZSendBLL = new WZSendBLL();
                string strSendHQL = "from WZSend as wZSend where SendCode = '" + cmdArges + "'";
                IList listSend = wZSendBLL.GetAllWZSends(strSendHQL);
                if (listSend != null && listSend.Count == 1)
                {
                    WZSend wZSend = (WZSend)listSend[0];
                    if (wZSend.Progress == "¼��")
                    {
                        string strCheckCode = wZSend.CheckCode;

                        //�ļ�
                        wZSend.Progress = "�ļ�";


                        wZSendBLL.UpdateWZSend(wZSend, wZSend.SendCode);

                        //���¼������ϵ��б�
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
            //�����
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
                        //���ϵ����ƻ����ۡ�����桴��浥�ۡ�												
                        //���ϵ����ƻ�������ʵ�������������ƻ����ۡ�												
                        //���ϵ�����ֵ������桴��ֵ�����������ƻ���												
                        //���ϵ�����������ƻ���������ֵ��												
                        //���ϵ�����ֵ��š�����桴��ֵ��š�												
                        //���ϵ�����ѹ��š�����桴��ѹ��š�												
                        //���ϵ�����š�����桴��š�												
                        //���ϵ�����λ�š�����桴��λ�š�												
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


                        //���ӷ��ϵ�
                        WZSend wZSend = new WZSend();
                        wZSend.SendCode = CreateNewSendCode();          //���ϱ��
                        wZSend.TicketTime = DateTime.Now;
                        wZSend.PurchaseEngineer = strUserCode;
                        wZSend.Progress = "¼��";
                        wZSend.IsMark = 0;
                        wZSend.SendMethod = "��Ʊ";
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

                        //���ϵ����ļ����ڡ����������ڡ�����-��
                        wZSend.CheckTime = "-";//DateTime.Now;
                        wZSend.SendTime = "-"; //DateTime.Now;

                        WZSendBLL wZSendBLL = new WZSendBLL();
                        wZSendBLL.AddWZSend(wZSend);

                        for (int i = 0; i < DG_Store.Items.Count; i++)
                        {
                            DG_Store.Items[i].ForeColor = Color.Black;
                        }

                        e.Item.ForeColor = Color.Red;


                        //���¼��ط��ϵ��б�
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
        string strSendMethod = DDL_SendMethod.SelectedValue;        //���Ϸ�ʽ
        string strProjectCode = DDL_Project.SelectedValue;          //��Ŀ����
        string strStoreRoom = DDL_StoreRoom.SelectedValue;          //���
        //string strUnitCode = TXT_UnitCode.Text.Trim();              //��λ���
        string strUnitCode = DDL_PickingUnit.SelectedValue;              //��λ���
        string strPickingUnit = TXT_PickingUnit.Text.Trim();        //���ϵ�λ
        string strChecker = TXT_Checker.Text.Trim();                //�ļ�Ա
        string strSafekeeper = TXT_Safekeeper.Text.Trim();          //����Ա
        string strUpLeader = TXT_UpLeader.Text.Trim();              //�����쵼


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
            //�޸�
            string strActualNumber = TXT_ActualNumber.Text.Trim();  //ʵ������
            string strPlanPrice = TXT_PlanPrice.Text.Trim();        //�ƻ�����
            string strPlanMoney = TXT_PlanMoney.Text.Trim();        //�ƻ����
            string strDownMoney = TXT_DownMoney.Text.Trim();        //��ֵ���
            string strCleanMoney = TXT_CleanMoney.Text.Trim();      //����
            string strReduceCode = TXT_ReduceCode.Text.Trim();      //��ֵ���
            string strWearyCode = TXT_WearyCode.Text.Trim();        //��ѹ���
            string strCheckCode = TXT_CheckCode.Text.Trim();        //���
            string strGoodsCode = TXT_GoodsCode.Text.Trim();        //��λ��
            string strManageRate = TXT_ManageRate.Text.Trim();      //�������


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

                //���¼��ط��ϵ��б�
                DataSendBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
            }
        }
        else
        {
            //����
            WZSendBLL wZSendBLL = new WZSendBLL();
            //���ӷ��ϵ�
            WZSend wZSend = new WZSend();
            wZSend.SendCode = CreateNewSendCode();          //���ϱ��
            wZSend.TicketTime = DateTime.Now;
            wZSend.PurchaseEngineer = strUserCode;
            wZSend.Progress = "¼��";
            wZSend.IsMark = 0;
            wZSend.SendMethod = strSendMethod;
            wZSend.ProjectCode = strProjectCode;
            wZSend.StoreRoom = strStoreRoom;
            wZSend.UnitCode = strUnitCode;
            wZSend.PickingUnit = strPickingUnit;
            wZSend.Checker = strChecker;
            wZSend.Safekeeper = strSafekeeper;
            wZSend.UpLeader = strUpLeader;

            wZSend.PlanDetaiID = 0;         //�޼ƻ����
            wZSend.CheckTime = "-"; //DateTime.Now;
            wZSend.SendTime = "-"; //DateTime.Now;

            wZSendBLL.AddWZSend(wZSend);

            //���¼��ط��ϵ��б�
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

        DDL_SendMethod.SelectedValue = "";           //���Ϸ�ʽ
        TXT_ActualNumber.Text = "0";                 //ʵ������
        TXT_PlanPrice.Text = "0";                     //�ƻ�����
        TXT_PlanMoney.Text = "0";                    //�ƻ����
        TXT_DownMoney.Text = "0";                 //��ֵ���
        TXT_CleanMoney.Text = "0";               //����
        TXT_ReduceCode.Text = "";                //��ֵ���
        TXT_WearyCode.Text = "";                  //��ѹ���
        TXT_CheckCode.Text = "";                  //���
        TXT_GoodsCode.Text = "";                  //��λ��
        TXT_ManageRate.Text = "0";                //�������
    }


    /// <summary>
    ///  ���ɷ��ϵ�Code
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

                    //��֤�µķ��ϵ�Code�Ƿ����
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
    /// ������ʴ���Ļ���ϵ��
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