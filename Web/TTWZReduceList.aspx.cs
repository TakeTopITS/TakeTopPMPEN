using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZReduceList : System.Web.UI.Page
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

            DataReduceBinder();

            DG_Store.DataSource = "";
            DG_Store.DataBind();
        }
    }

    private void DataReduceBinder()
    {
        DG_List.CurrentPageIndex = 0;

        string strWZReduceHQL = string.Format(@"select r.*,m.UserName as MainLeaderName,p.UserName as MarkerName from T_WZReduce r
                        left join T_ProjectMember m on r.MainLeader = m.UserCode
                        left join T_ProjectMember p on r.Marker = p.UserCode 
                        where r.Marker = '{0}' 
                        order by r.PlanTime desc", strUserCode);
        DataTable dtReduce = ShareClass.GetDataSetFromSql(strWZReduceHQL, "Reduce").Tables[0];

        DG_List.DataSource = dtReduce;
        DG_List.DataBind();

        LB_ReduceSql.Text = strWZReduceHQL;
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

    private void DataStoreBinder(string strReduceCode)
    {
        string strStoreHQL = string.Format(@"select s.*,o.ObjectName from T_WZStore s
                       left join T_WZObject o on s.ObjectCode = o.ObjectCode
                       where s.DownCode = '{0}'", strReduceCode);
        DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];

        DG_Store.DataSource = dtStore;
        DG_Store.DataBind();
    }

    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "click")
            {
                //操作
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                string[] arrOperate = cmdArges.Split('|');

                string strEditReduceCode = arrOperate[0];
                string strProgress = arrOperate[1];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);

                HF_NewReduceCode.Value = strEditReduceCode;
                HF_NewProcess.Value = strProgress;
            }
            else if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + cmdArges + "'";
                IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
                if (listWZReduce != null && listWZReduce.Count == 1)
                {
                    WZReduce wZReduce = (WZReduce)listWZReduce[0];

                    if (wZReduce.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWBZZTBYXSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZReduceBLL.DeleteWZReduce(wZReduce);

                    //重新加载列表
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }

            }
            else if (cmdName == "edit")
            {
                //修改
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                string strWZReduceHQL = string.Format(@"select r.*,p.UserName as MainLeaderName from T_WZReduce r
                                    left join T_ProjectMember p on r.MainLeader = p.UserCode 
                                    where r.ReduceCode = '{0}'", cmdArges);
                DataTable dtReduce = ShareClass.GetDataSetFromSql(strWZReduceHQL, "Reduce").Tables[0];
                if (dtReduce != null && dtReduce.Rows.Count == 1)
                {
                    DataRow drReduce = dtReduce.Rows[0];

                    string strReduceCode = ShareClass.ObjectToString(drReduce["ReduceCode"]);
                    TXT_ReduceCode.Text = strReduceCode;
                    DDL_StoreRoom.SelectedValue = ShareClass.ObjectToString(drReduce["StoreRoom"]);
                    TXT_PlanMoney.Text = ShareClass.ObjectToString(drReduce["PlanMoney"]);
                    TXT_Remark.Text = ShareClass.ObjectToString(drReduce["Remark"]);
                    HF_MainLeader.Value = ShareClass.ObjectToString(drReduce["MainLeader"]);
                    TXT_MainLeader.Text = ShareClass.ObjectToString(drReduce["MainLeaderName"]);

                    //加载库存
                    DataStoreBinder(strReduceCode);
                }
                #region 注释
                //string cmdArges = e.CommandArgument.ToString();
                //WZReduceBLL wZReduceBLL = new WZReduceBLL();
                //string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + cmdArges + "'";
                //IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
                //if (listWZReduce != null && listWZReduce.Count == 1)
                //{
                //    WZReduce wZReduce = (WZReduce)listWZReduce[0];

                //    TXT_ReduceCode.Text = wZReduce.ReduceCode;
                //    DDL_StoreRoom.SelectedValue = wZReduce.StoreRoom;
                //    TXT_PlanMoney.Text = wZReduce.PlanMoney.ToString();
                //    TXT_Remark.Text = wZReduce.Remark;
                //    TXT_MainLeader.Text = wZReduce.MainLeader;

                //    //加载库存
                //    DataStoreBinder(wZReduce.ReduceCode);
                //}
                #endregion
            }
            else if (cmdName == "submit")
            {
                //提交
                string cmdArges = e.CommandArgument.ToString();
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + cmdArges + "'";
                IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
                if (listWZReduce != null && listWZReduce.Count == 1)
                {
                    WZReduce wZReduce = (WZReduce)listWZReduce[0];

                    if (wZReduce.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWBZZTBYXBP").ToString().Trim()+"')", true);
                        return;
                    }

                    wZReduce.Process = LanguageHandle.GetWord("BaoPi").ToString().Trim();

                    wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

                    //重新加载列表
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJCG").ToString().Trim()+"')", true);
                }
            }
            else if (cmdName == "check")
            {
                //检查
                string cmdArges = e.CommandArgument.ToString();
                string strStoreHQL = string.Format(@"select * from T_WZStore
                       where DownCode = '{0}'", cmdArges);
                DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                string strMessage = string.Empty;
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow drStore in dtStore.Rows)
                    {
                        decimal decimalInNumber = 0;    //入库数量
                        decimal decimalInMoney = 0;     //入库金额
                        decimal decimalOutNumber = 0;   //出库数量
                        decimal decimalOutPrice = 0;    //出库金额
                        decimal.TryParse(ShareClass.ObjectToString(drStore["InNumber"]), out decimalInNumber);
                        decimal.TryParse(ShareClass.ObjectToString(drStore["InMoney"]), out decimalInMoney);
                        decimal.TryParse(ShareClass.ObjectToString(drStore["OutNumber"]), out decimalOutNumber);
                        decimal.TryParse(ShareClass.ObjectToString(drStore["OutPrice"]), out decimalOutPrice);

                        if (decimalInNumber != 0 || decimalInMoney != 0
                            || decimalOutNumber != 0 || decimalOutPrice != 0)
                        {
                            strMessage += LanguageHandle.GetWord("KuCunID").ToString().Trim() + ShareClass.ObjectToString(drStore["ID"]) + LanguageHandle.GetWord("KuCunBuWei0t").ToString().Trim();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(strMessage))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJCDKCBWKDSTRMESSAGE").ToString().Trim()+"')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJCCGKYZX").ToString().Trim()+"')", true);
                    return;
                }
            }
            else if (cmdName == "execute")
            {
                //执行
                string cmdArges = e.CommandArgument.ToString();
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + cmdArges + "'";
                IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
                if (listWZReduce != null && listWZReduce.Count == 1)
                {
                    WZReduce wZReduce = (WZReduce)listWZReduce[0];

                    string strReduceStoreHQL = string.Format(@"select * from T_WZStore
                                            where DownCode = '{0}'", wZReduce.ReduceCode);
                    DataTable dtReduceStore = ShareClass.GetDataSetFromSql(strReduceStoreHQL, "ReduceStore").Tables[0];
                    decimal decimalStoreTotalMoney = 0;          //库存总额
                    int intStoreDetailNumber = 0;                 //明细条数
                    decimal decimalStoreDownMoney = 0;          //减值金额
                    decimal decimalStoreCleanMoney = 0;              //净额
                    //减值计划〈库存总额〉＝∑库存〈库存金额〉												
                    //减值计划〈明细条数〉＝库存记录条数 合计												
                    //减值计划〈减值金额〉＝∑库存〈减值金额〉												
                    //减值计划〈净额〉＝∑库存〈净额〉												
                    //减值计划〈计划减值〉＝减值计划〈减值金额〉÷减值计划〈库存总额〉												
                    //减值计划〈执行日期〉＝系统日期												
                    //减值计划〈进度〉＝“生效”		
                    if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
                    {
                        foreach (DataRow drStore in dtReduceStore.Rows)
                        {
                            string strUpdateStoreHQL = string.Format(@"update T_WZStore 
                                set DownDesc = -1,
                                WearyDesc=0 ,
                                WearyCode = '-'
                                where ID = {0}", ShareClass.ObjectToString(drStore["ID"]));
                            ShareClass.RunSqlCommand(strUpdateStoreHQL);

                            decimal decimalStoreMoney = 0;                          //库存<库存金额>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                            decimal decimalDownMoney = 0;                           //库存<减值金额>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                            decimal decimalCleanMoney = 0;                          //库存<净额>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["CleanMoney"]), out decimalCleanMoney);

                            decimalStoreTotalMoney += decimalStoreMoney;
                            intStoreDetailNumber++;
                            decimalStoreDownMoney += decimalDownMoney;
                            decimalStoreCleanMoney += decimalCleanMoney;
                        }
                    }
                    wZReduce.StoreTotalMoney = decimalStoreTotalMoney;
                    wZReduce.DetailNumber = intStoreDetailNumber;
                    wZReduce.StoreDownMoney = decimalStoreDownMoney;
                    wZReduce.CleanMoney = decimalStoreCleanMoney;
                    if (decimalStoreTotalMoney != 0)
                    {
                        wZReduce.PlanMoney = decimalStoreDownMoney / decimalStoreTotalMoney;
                    }
                    wZReduce.ExcuteTime = DateTime.Now.ToString("yyyy-MM-dd");
                    wZReduce.Process = LanguageHandle.GetWord("ShengXiao").ToString().Trim();

                    wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

                    //重新加载
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZXCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
                }
            }
            else if (cmdName == "calc")
            {
                //计算
                string cmdArges = e.CommandArgument.ToString();
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + cmdArges + "'";
                IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
                if (listWZReduce != null && listWZReduce.Count == 1)
                {
                    WZReduce wZReduce = (WZReduce)listWZReduce[0];
                    if (wZReduce.Process != LanguageHandle.GetWord("ShengXiao").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWSXZTBYXJS").ToString().Trim()+"')", true);
                        return;
                    }

                    string strReduceStoreHQL = string.Format(@"select * from T_WZStore
                                            where DownCode = '{0}'", wZReduce.ReduceCode);
                    DataTable dtReduceStore = ShareClass.GetDataSetFromSql(strReduceStoreHQL, "ReduceStore").Tables[0];
                    decimal decimalStoreTotalMoney = 0;          //库存金额
                    int intStoreDetailNumber = 0;                 //明细条数
                    decimal decimalStoreDownMoney = 0;          //减值金额
                    decimal decimalStoreCleanMoney = 0;              //净额
                    //减值计划〈统计库存〉＝∑库存〈库存金额〉												
                    //减值计划〈统计条数〉＝∑库存记录条数												
                    //减值计划〈统计减值〉＝∑库存〈减值金额〉												
                    //减值计划〈统计净额〉＝∑库存〈净额〉												
                    //减值计划〈统计比例〉＝减值计划〈统计减值〉÷减值计划〈统计库存〉												
                    //“计算”按钮可重复使用，直到该减值计划〈统计库存〉＝“0”时关闭，同步使减值计划〈进度〉＝“完成”												

                    if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
                    {
                        foreach (DataRow drStore in dtReduceStore.Rows)
                        {
                            decimal decimalStoreMoney = 0;                          //库存<库存金额>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                            decimal decimalDownMoney = 0;                           //库存<减值金额>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                            decimal decimalCleanMoney = 0;                          //库存<净额>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["CleanMoney"]), out decimalCleanMoney);

                            decimalStoreTotalMoney += decimalStoreMoney;
                            intStoreDetailNumber++;
                            decimalStoreDownMoney += decimalDownMoney;
                            decimalStoreCleanMoney += decimalCleanMoney;
                        }
                    }
                    wZReduce.TotalStore = decimalStoreTotalMoney;
                    wZReduce.TotalNumber = intStoreDetailNumber;
                    wZReduce.TotalDownMoney = decimalStoreDownMoney;
                    wZReduce.TotalCleanMoney = decimalStoreCleanMoney;
                    if (decimalStoreTotalMoney != 0)
                    {
                        wZReduce.TotalRatio = decimalStoreDownMoney / decimalStoreTotalMoney;
                    }
                    else
                    {
                        wZReduce.Process = "Completed";
                    }

                    wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

                    //重新加载
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJSCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
                }
            }
            else if (cmdName == "store")
            {
                //减值库存
                string cmdArges = e.CommandArgument.ToString();

                //加载库存
                DataStoreBinder(cmdArges);
            }
        }
    }


    protected void DG_Store_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            //库存点击
            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                string strReduceCode = TXT_ReduceCode.Text;
                if (!string.IsNullOrEmpty(strReduceCode))
                {
                    string cmdArges = e.CommandArgument.ToString();

                    WZStoreBLL wZStoreBLL = new WZStoreBLL();
                    string strWZStoreHQL = "from WZStore as wZStore where ID = " + cmdArges;
                    IList lstStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                    if (lstStore != null && lstStore.Count > 0)
                    {
                        WZStore wZStore = (WZStore)lstStore[0];

                        TXT_ID.Text = wZStore.ID.ToString();
                        TXT_DownRatio.Text = wZStore.DownRatio.ToString();

                        for (int i = 0; i < DG_Store.Items.Count; i++)
                        {
                            DG_Store.Items[i].ForeColor = Color.Black;
                        }

                        e.Item.ForeColor = Color.Red;
                    }
                }
                else
                {
                    string strNewProgress = HF_NewProcess.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先点击减值计划！');ControlStatusChange('" + strNewProgress + "');", true); 
                    return;
                }
            }
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strPlanMoney = TXT_PlanMoney.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();
            string strMainLeader = HF_MainLeader.Value; //TXT_MainLeader.Text.Trim();

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择库别！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('计划减值只能为小数，或者整数！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择主管领导！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);

            string strReduceCode = TXT_ReduceCode.Text;
            if (!string.IsNullOrEmpty(strReduceCode))
            {
                //修改
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strReduceCode + "'";
                IList lstReduce = wZReduceBLL.GetAllWZReduces(strReduceHQL);
                if (lstReduce != null && lstReduce.Count > 0)
                {
                    WZReduce wZReduce = (WZReduce)lstReduce[0];


                    if (wZReduce.Process == LanguageHandle.GetWord("BianZhi").ToString().Trim())
                    {
                        string strNewProgress = HF_NewProcess.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('进度不是编制，不能修改！');ControlStatusChange('" + strNewProgress + "');", true); 
                        return;
                    }

                    wZReduce.StoreRoom = strStoreRoom;
                    wZReduce.PlanMoney = decimalPlanMoney;
                    wZReduce.Remark = strRemark;
                    wZReduce.MainLeader = strMainLeader;

                    wZReduceBLL.UpdateWZReduce(wZReduce, strReduceCode);
                }

            }
            else
            {
                //增加
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                WZReduce wZReduce = new WZReduce();
                wZReduce.ReduceCode = CreateNewReduceCode();                //生成新的减值ID
                wZReduce.StoreRoom = strStoreRoom;
                wZReduce.PlanTime = DateTime.Now;
                wZReduce.PlanMoney = decimalPlanMoney;
                wZReduce.Remark = strRemark;
                wZReduce.Process = LanguageHandle.GetWord("BianZhi").ToString().Trim();
                wZReduce.MainLeader = strMainLeader;
                wZReduce.Marker = strUserCode;

                wZReduceBLL.AddWZReduce(wZReduce);
            }

            //重新加载列表
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true); 
        }
        catch (Exception ex)
        { }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        try
        {
            string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strPlanMoney = TXT_PlanMoney.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();
            string strMainLeader = HF_MainLeader.Value; //TXT_MainLeader.Text.Trim();

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择库别！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('计划减值只能为小数，或者整数！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择主管领导！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);


            //增加
            WZReduceBLL wZReduceBLL = new WZReduceBLL();
            WZReduce wZReduce = new WZReduce();
            wZReduce.ReduceCode = CreateNewReduceCode();                //生成新的减值ID
            wZReduce.StoreRoom = strStoreRoom;
            wZReduce.PlanTime = DateTime.Now;
            wZReduce.PlanMoney = decimalPlanMoney;
            wZReduce.Remark = strRemark;
            wZReduce.Process = LanguageHandle.GetWord("BianZhi").ToString().Trim();
            wZReduce.MainLeader = strMainLeader;
            wZReduce.Marker = strUserCode;

            wZReduceBLL.AddWZReduce(wZReduce);


            //重新加载列表
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true); 
        }
        catch (Exception ex)
        { }
    }



    protected void BT_ReduceEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string strStoreRoom = DDL_StoreRoom.SelectedValue;
            string strPlanMoney = TXT_PlanMoney.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();
            string strMainLeader = HF_MainLeader.Value; //TXT_MainLeader.Text.Trim();

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择库别！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('计划减值只能为小数，或者整数！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请选择主管领导！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);

            string strReduceCode = TXT_ReduceCode.Text;
            if (!string.IsNullOrEmpty(strReduceCode))
            {
                //修改
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strReduceCode + "'";
                IList lstReduce = wZReduceBLL.GetAllWZReduces(strReduceHQL);
                if (lstReduce != null && lstReduce.Count > 0)
                {
                    WZReduce wZReduce = (WZReduce)lstReduce[0];


                    if (wZReduce.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
                    {
                        string strNewProgress = HF_NewProcess.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('进度不是编制，不能修改！');ControlStatusChange('" + strNewProgress + "');", true); 
                        return;
                    }

                    wZReduce.StoreRoom = strStoreRoom;
                    wZReduce.PlanMoney = decimalPlanMoney;
                    wZReduce.Remark = strRemark;
                    wZReduce.MainLeader = strMainLeader;

                    wZReduceBLL.UpdateWZReduce(wZReduce, strReduceCode);
                }

            }
            else
            {
                //增加
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要修改的减值列表！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            //重新加载列表
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('保存成功！');ControlStatusCloseChange();", true); 
        }
        catch (Exception ex)
        { }
    }


    protected void BT_Cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        TXT_ReduceCode.Text = "";
        DDL_StoreRoom.SelectedValue = "";
        TXT_PlanMoney.Text = "0";
        TXT_Remark.Text = "";
        TXT_MainLeader.Text = "";

        TXT_ID.Text = "";
        TXT_DownRatio.Text = "0";

        for (int i = 0; i < DG_Store.Items.Count; i++)
        {
            DG_Store.Items[i].ForeColor = Color.Black;
        }
    }


    protected void BT_Total_Click(object sender, EventArgs e)
    {
        string strReduceCode = HF_TotalReduceCode.Value; //TXT_ReduceCode.Text;
        if (!string.IsNullOrEmpty(strReduceCode))
        {
            string strYear = HF_TotalYear.Value.Trim();//TXT_Year.Text.Trim();
            if (!ShareClass.CheckIsNumber(strYear))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('统计年份必须为整数！');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            WZReduceBLL wZReduceBLL = new WZReduceBLL();
            string strReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strReduceCode + "'";
            IList lstReduce = wZReduceBLL.GetAllWZReduces(strReduceHQL);
            if (lstReduce != null && lstReduce.Count > 0)
            {
                WZReduce wZReduce = (WZReduce)lstReduce[0];

                string strStoreHQL = string.Format(@"select * from T_WZStore
                where DownDesc = 0
                and StockCode = '{0}'
                and StoreMoney > 0
                and extract(year from EndOutTime) <= (extract(year from now())-{1})", wZReduce.StoreRoom, strYear);
                DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                decimal decimalStoreTotalMoney = 0;          //库存总额
                int intStoreDetailNumber = 0;                 //明细条数
                decimal decimalStoreDownMoney = 0;          //减值金额
                decimal decimalStoreCleanMoney = 0;              //净额
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow drStore in dtStore.Rows)
                    {
                        decimal decimalDownRatio = wZReduce.PlanMoney;          //库存<减值比例>
                        decimal decimalStoreMoney = 0;                          //库存<库存金额>
                        decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);

                        decimal decimalDownMoney = decimalStoreMoney * decimalDownRatio;//库存<减值金额>
                        decimal decimalCleanMoney = decimalStoreMoney - decimalDownMoney;//库存<净额>

                        string strUpdateStoreHQL = string.Format(@"update T_WZStore 
                                set DownCode = '{0}',
                                DownRatio = {1},
                                DownMoney = {2},
                                CleanMoney = {3}
                                where ID = {4}", wZReduce.ReduceCode, decimalDownRatio, decimalDownMoney, decimalCleanMoney, ShareClass.ObjectToString(drStore["ID"]));
                        ShareClass.RunSqlCommand(strUpdateStoreHQL);

                        decimalStoreTotalMoney += decimalStoreMoney;
                        intStoreDetailNumber++;
                        decimalStoreDownMoney += decimalDownMoney;
                        decimalStoreCleanMoney += decimalCleanMoney;
                    }
                }

                wZReduce.StoreTotalMoney = decimalStoreTotalMoney;
                wZReduce.DetailNumber = intStoreDetailNumber;
                wZReduce.StoreDownMoney = decimalStoreDownMoney;
                wZReduce.CleanMoney = decimalStoreCleanMoney;

                wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

                //重新加载列表
                DataReduceBinder();
                //加载库存
                DataStoreBinder(wZReduce.ReduceCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('统计完成！');ControlStatusCloseChange();", true); 
            }
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择减值单！');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }

    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        //库存<减值比例>
        string strReduceCode = TXT_ReduceCode.Text;
        if (!string.IsNullOrEmpty(strReduceCode))
        {
            WZReduceBLL wZReduceBLL = new WZReduceBLL();
            string strReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strReduceCode + "'";
            IList lstReduce = wZReduceBLL.GetAllWZReduces(strReduceHQL);
            if (lstReduce != null && lstReduce.Count > 0)
            {
                WZReduce wZReduce = (WZReduce)lstReduce[0];

                string strID = TXT_ID.Text;
                string strDownRatio = TXT_DownRatio.Text.Trim();

                if (!string.IsNullOrEmpty(strID))
                {
                    if (!ShareClass.CheckIsNumber(strDownRatio))
                    {
                        string strNewProgress = HF_NewProcess.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('减值比例只能为小数，或者整数！');ControlStatusChange('" + strNewProgress + "');", true); 
                        return;
                    }
                    decimal decimalNewDownRatio = 0;
                    decimal.TryParse(strDownRatio, out decimalNewDownRatio);

                    WZStoreBLL wZStoreBLL = new WZStoreBLL();
                    string strWZStoreHQL = "from WZStore as wZStore where ID = " + strID;
                    IList lstWZStore = wZStoreBLL.GetAllWZStores(strWZStoreHQL);
                    if (lstWZStore != null && lstWZStore.Count > 0)
                    {
                        WZStore wZStore = (WZStore)lstWZStore[0];

                        //库存〈减值金额〉＝库存〈库存金额〉×新录入的减值比例												
                        //库存〈净额〉＝库存〈库存金额〉-库存〈减值金额〉												
                        decimal decimalStoreMoney = wZStore.StoreMoney; //库存金额
                        decimal decimalDownMoney = decimalStoreMoney * decimalNewDownRatio;//减值金额
                        wZStore.DownMoney = decimalDownMoney;
                        wZStore.CleanMoney = decimalStoreMoney - decimalDownMoney;

                        wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                        //减值计划〈减值金额〉＝∑库存〈减值金额〉												
                        //减值计划〈净额〉＝∑库存〈净额〉												
                        //减值计划〈计划减值〉＝减值计划〈减值金额〉÷减值计划〈库存总额〉												
                        string strReduceStoreHQL = string.Format(@"select * from T_WZStore
                                            where DownCode = '{0}'", wZReduce.ReduceCode);
                        DataTable dtReduceStore = ShareClass.GetDataSetFromSql(strReduceStoreHQL, "ReduceStore").Tables[0];

                        decimal decimalStoreDownMoney = 0;          //减值金额
                        decimal decimalStoreCleanMoney = 0;         //净额
                        if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
                        {
                            foreach (DataRow drStore in dtReduceStore.Rows)
                            {
                                decimal decimalDownMoney2 = 0;                          //库存<减值金额>
                                decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney2);
                                decimal decimalCleanMoney2 = 0;                         //库存<净额>
                                decimal.TryParse(ShareClass.ObjectToString(drStore["CleanMoney"]), out decimalCleanMoney2);

                                decimalStoreDownMoney += decimalDownMoney2;
                                decimalStoreCleanMoney += decimalCleanMoney2;
                            }
                        }
                        decimal decimalStoreTotalMoney = wZReduce.StoreTotalMoney;//减值计划<库存总额>
                        wZReduce.StoreDownMoney = decimalStoreDownMoney;
                        wZReduce.CleanMoney = decimalStoreCleanMoney;
                        if (decimalStoreTotalMoney != 0)
                        {
                            wZReduce.PlanMoney = decimalStoreDownMoney / decimalStoreTotalMoney;
                        }
                        wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

                        //重新加载库存，减值计划
                        DataStoreBinder(wZReduce.ReduceCode);
                        DataReduceBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('修改成功！');ControlStatusCloseChange();", true); 
                    }
                }
                else
                {
                    string strNewProgress = HF_NewProcess.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择库存！');ControlStatusChange('" + strNewProgress + "');", true); 
                    return;
                }
            }
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择减值单！');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }



    protected void BT_ExportStoreEffect_Click(object sender, EventArgs e)
    {
        //导出条件：减值计划〈进度〉＝“生效”
        string strReduceCode = Request.Form["cb_ReduceCode"];
        if (!string.IsNullOrEmpty(strReduceCode))
        {
            string[] arrReduceCode = strReduceCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereReduceCode = string.Empty;
            for (int i = 0; i < arrReduceCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrReduceCode[i]))
                {
                    strWhereReduceCode += "'" + arrReduceCode[i] + "',";
                }
            }
            strWhereReduceCode = strWhereReduceCode.EndsWith(",") ? strWhereReduceCode.TrimEnd(',') : strWhereReduceCode;

            //查询减值计划
            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
                        and Process = '生效'", strWhereReduceCode); 
            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //文件名：《〈减值编号〉＋号减值计划》
            Export3Excel(dtSelectReduce, "进度为生效的减值计划"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要导出的减值列表！');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }

    protected void BT_ExportStore_Click(object sender, EventArgs e)
    {
        //导出条件：减值计划〈进度〉＝“生效”，库存〈减值编号〉＝减值计划〈减值编号〉
        string strReduceCode = Request.Form["cb_ReduceCode"];
        if (!string.IsNullOrEmpty(strReduceCode))
        {
            string[] arrReduceCode = strReduceCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereReduceCode = string.Empty;
            for (int i = 0; i < arrReduceCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrReduceCode[i]))
                {
                    strWhereReduceCode += "'" + arrReduceCode[i] + "',";
                }
            }
            strWhereReduceCode = strWhereReduceCode.EndsWith(",") ? strWhereReduceCode.TrimEnd(',') : strWhereReduceCode;

            //查询减值计划
            //            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
            //                        and Process = '生效'", strWhereReduceCode); 
            //            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //查询减值计划明细（库存)
            string strSelectReduceDetailHQL = string.Format(@"select s.* from T_WZReduce r
                        left join T_WZStore s on r.ReduceCode = s.DownCode
                        where r.ReduceCode in ({0})
                        and r.Process = '生效'", strWhereReduceCode); 
            DataTable dtSelectReduceDetail = ShareClass.GetDataSetFromSql(strSelectReduceDetailHQL, "SelectReduceDetail").Tables[0];

            //文件名：《〈减值编号〉＋号减值计划》、《〈减值编号〉＋号减值计划明细》
            Export3Excel(dtSelectReduceDetail, "减值计划明细"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要导出的减值列表！');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }



    protected void BT_ExportStoreComplete_Click(object sender, EventArgs e)
    {
        //导出条件：减值计划〈进度〉＝“完成”
        string strReduceCode = Request.Form["cb_ReduceCode"];
        if (!string.IsNullOrEmpty(strReduceCode))
        {
            string[] arrReduceCode = strReduceCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereReduceCode = string.Empty;
            for (int i = 0; i < arrReduceCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrReduceCode[i]))
                {
                    strWhereReduceCode += "'" + arrReduceCode[i] + "',";
                }
            }
            strWhereReduceCode = strWhereReduceCode.EndsWith(",") ? strWhereReduceCode.TrimEnd(',') : strWhereReduceCode;

            //查询减值计划
            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
                        and Process = 'Completed'", strWhereReduceCode);
            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //文件名：《〈减值编号〉＋号减值计划》、《〈减值编号〉＋号减值计划发料单》
            Export3Excel(dtSelectReduce, "进度为完成的减值计划"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要导出的积压列表！');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }

    protected void BT_ExportSend_Click(object sender, EventArgs e)
    {
        //导出条件：减值计划〈进度〉＝“完成”，发料单〈减值编号〉＝减值计划〈减值编号〉
        string strReduceCode = Request.Form["cb_ReduceCode"];
        if (!string.IsNullOrEmpty(strReduceCode))
        {
            string[] arrReduceCode = strReduceCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereReduceCode = string.Empty;
            for (int i = 0; i < arrReduceCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrReduceCode[i]))
                {
                    strWhereReduceCode += "'" + arrReduceCode[i] + "',";
                }
            }
            strWhereReduceCode = strWhereReduceCode.EndsWith(",") ? strWhereReduceCode.TrimEnd(',') : strWhereReduceCode;

            //查询减值计划
            //            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
            //                        and Process = 'Completed'", strWhereReduceCode);
            //            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //查询发料单<减值编号>
            string strSelectSendHQL = string.Format(@"select s.* from T_WZReduce r
                        left join T_WZSend s on r.ReduceCode = s.ReduceCode
                        where r.ReduceCode in ({0})
                        and r.Process = 'Completed'", strWhereReduceCode);
            DataTable dtSelectSend = ShareClass.GetDataSetFromSql(strSelectSendHQL, "SelectSend").Tables[0];

            //文件名：《〈减值编号〉＋号减值计划》、《〈减值编号〉＋号减值计划发料单》
            Export3Excel(dtSelectSend, "减值计划发料单"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('请先选择要导出的积压列表！');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_ReduceSql.Text.Trim();
        DataTable dtReduce = ShareClass.GetDataSetFromSql(strHQL, "Reduce").Tables[0];

        DG_List.DataSource = dtReduce;
        DG_List.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }

    /// <summary>
    ///  生成减值ID
    /// </summary>
    private string CreateNewReduceCode()
    {
        string strNewReduceCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strReduceCodeHQL = string.Format("select count(1) as RowNumber from T_WZReduce where to_char( PlanTime, 'yyyy-mm-dd' ) like '{0}%'", DateTime.Now.ToString("yyyy-MM"));
                DataTable dtReduceCode = ShareClass.GetDataSetFromSql(strReduceCodeHQL, "ReduceCode").Tables[0];
                int intReduceCodeNumber = int.Parse(dtReduceCode.Rows[0]["RowNumber"].ToString());
                intReduceCodeNumber = intReduceCodeNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbReduceCode = new StringBuilder();
                    for (int j = 3 - intReduceCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbReduceCode.Append(" ");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewReduceCode = strYear + "" + strMonth + "-" + sbReduceCode.ToString() + intReduceCodeNumber.ToString();

                    //验证新的减值ID是否存在
                    string strCheckNewReduceCodeHQL = "select count(1) as RowNumber from T_WZReduce where ReduceCode = '" + strNewReduceCode + "'";
                    DataTable dtCheckNewReduceCode = ShareClass.GetDataSetFromSql(strCheckNewReduceCodeHQL, "CheckNewReduceCode").Tables[0];
                    int intCheckNewReduceCode = int.Parse(dtCheckNewReduceCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewReduceCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intReduceCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewReduceCode;
    }




    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //编辑
        string strEditReduceCode = HF_NewReduceCode.Value;
        if (string.IsNullOrEmpty(strEditReduceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDJZLB").ToString().Trim()+"')", true);
            return;
        }

        string strWZReduceHQL = string.Format(@"select r.*,p.UserName as MainLeaderName from T_WZReduce r
                                    left join T_ProjectMember p on r.MainLeader = p.UserCode 
                                    where r.ReduceCode = '{0}'", strEditReduceCode);
        DataTable dtReduce = ShareClass.GetDataSetFromSql(strWZReduceHQL, "Reduce").Tables[0];
        if (dtReduce != null && dtReduce.Rows.Count == 1)
        {
            DataRow drReduce = dtReduce.Rows[0];

            string strReduceCode = ShareClass.ObjectToString(drReduce["ReduceCode"]);
            TXT_ReduceCode.Text = strReduceCode;
            DDL_StoreRoom.SelectedValue = ShareClass.ObjectToString(drReduce["StoreRoom"]);
            TXT_PlanMoney.Text = ShareClass.ObjectToString(drReduce["PlanMoney"]);
            TXT_Remark.Text = ShareClass.ObjectToString(drReduce["Remark"]);
            HF_MainLeader.Value = ShareClass.ObjectToString(drReduce["MainLeader"]);
            TXT_MainLeader.Text = ShareClass.ObjectToString(drReduce["MainLeaderName"]);

            //加载库存
            DataStoreBinder(strReduceCode);
        }
    }


    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //删除
        string strEditReduceCode = HF_NewReduceCode.Value;
        if (string.IsNullOrEmpty(strEditReduceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDJZLB").ToString().Trim()+"')", true);
            return;
        }

        WZReduceBLL wZReduceBLL = new WZReduceBLL();
        string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strEditReduceCode + "'";
        IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
        if (listWZReduce != null && listWZReduce.Count == 1)
        {
            WZReduce wZReduce = (WZReduce)listWZReduce[0];

            if (wZReduce.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWBZZTBYXSC").ToString().Trim()+"')", true);
                return;
            }

            wZReduceBLL.DeleteWZReduce(wZReduce);

            //重新加载列表
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
    }


    protected void BT_NewTotal_Click(object sender, EventArgs e)
    {
        //统计
        string strEditReduceCode = HF_NewReduceCode.Value;
        if (string.IsNullOrEmpty(strEditReduceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDJZLB").ToString().Trim()+"')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickTotal('" + strEditReduceCode + "')", true);
        return;
    }


    protected void BT_NewSubmit_Click(object sender, EventArgs e)
    {
        //提交
        string strEditReduceCode = HF_NewReduceCode.Value;
        if (string.IsNullOrEmpty(strEditReduceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDJZLB").ToString().Trim()+"')", true);
            return;
        }

        WZReduceBLL wZReduceBLL = new WZReduceBLL();
        string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strEditReduceCode + "'";
        IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
        if (listWZReduce != null && listWZReduce.Count == 1)
        {
            WZReduce wZReduce = (WZReduce)listWZReduce[0];

            if (wZReduce.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWBZZTBYXBP").ToString().Trim()+"')", true);
                return;
            }

            wZReduce.Process = LanguageHandle.GetWord("BaoPi").ToString().Trim();

            wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

            //重新加载列表
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJCG").ToString().Trim()+"')", true);
        }
    }


    protected void BT_NewCheck_Click(object sender, EventArgs e)
    {
        //检测
        string strEditReduceCode = HF_NewReduceCode.Value;
        if (string.IsNullOrEmpty(strEditReduceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDJZLB").ToString().Trim()+"')", true);
            return;
        }

        string strStoreHQL = string.Format(@"select * from T_WZStore
                       where DownCode = '{0}'", strEditReduceCode);
        DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
        string strMessage = string.Empty;
        if (dtStore != null && dtStore.Rows.Count > 0)
        {
            foreach (DataRow drStore in dtStore.Rows)
            {
                decimal decimalInNumber = 0;    //入库数量
                decimal decimalInMoney = 0;     //入库金额
                decimal decimalOutNumber = 0;   //出库数量
                decimal decimalOutPrice = 0;    //出库金额
                decimal.TryParse(ShareClass.ObjectToString(drStore["InNumber"]), out decimalInNumber);
                decimal.TryParse(ShareClass.ObjectToString(drStore["InMoney"]), out decimalInMoney);
                decimal.TryParse(ShareClass.ObjectToString(drStore["OutNumber"]), out decimalOutNumber);
                decimal.TryParse(ShareClass.ObjectToString(drStore["OutPrice"]), out decimalOutPrice);

                if (decimalInNumber != 0 || decimalInMoney != 0
                    || decimalOutNumber != 0 || decimalOutPrice != 0)
                {
                    strMessage += LanguageHandle.GetWord("KuCunID").ToString().Trim() + ShareClass.ObjectToString(drStore["ID"]) + LanguageHandle.GetWord("KuCunBuWei0t").ToString().Trim();
                }
            }
        }

        if (!string.IsNullOrEmpty(strMessage))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJCDKCBWKDSTRMESSAGE").ToString().Trim()+"')", true);
            return;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJCCGKYZX").ToString().Trim()+"')", true);
            return;
        }
    }

    protected void BT_NewExecute_Click(object sender, EventArgs e)
    {
        //执行
        string strEditReduceCode = HF_NewReduceCode.Value;
        if (string.IsNullOrEmpty(strEditReduceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDJZLB").ToString().Trim()+"')", true);
            return;
        }

        WZReduceBLL wZReduceBLL = new WZReduceBLL();
        string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strEditReduceCode + "'";
        IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
        if (listWZReduce != null && listWZReduce.Count == 1)
        {
            WZReduce wZReduce = (WZReduce)listWZReduce[0];

            string strReduceStoreHQL = string.Format(@"select * from T_WZStore
                                            where DownCode = '{0}'", wZReduce.ReduceCode);
            DataTable dtReduceStore = ShareClass.GetDataSetFromSql(strReduceStoreHQL, "ReduceStore").Tables[0];
            decimal decimalStoreTotalMoney = 0;          //库存总额
            int intStoreDetailNumber = 0;                 //明细条数
            decimal decimalStoreDownMoney = 0;          //减值金额
            decimal decimalStoreCleanMoney = 0;              //净额
            //减值计划〈库存总额〉＝∑库存〈库存金额〉												
            //减值计划〈明细条数〉＝库存记录条数 合计												
            //减值计划〈减值金额〉＝∑库存〈减值金额〉												
            //减值计划〈净额〉＝∑库存〈净额〉												
            //减值计划〈计划减值〉＝减值计划〈减值金额〉÷减值计划〈库存总额〉												
            //减值计划〈执行日期〉＝系统日期												
            //减值计划〈进度〉＝“生效”		
            if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
            {
                foreach (DataRow drStore in dtReduceStore.Rows)
                {
                    string strUpdateStoreHQL = string.Format(@"update T_WZStore 
                                set DownDesc = -1,
                                WearyDesc=0 ,
                                WearyCode = '-'
                                where ID = {0}", ShareClass.ObjectToString(drStore["ID"]));
                    ShareClass.RunSqlCommand(strUpdateStoreHQL);

                    decimal decimalStoreMoney = 0;                          //库存<库存金额>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                    decimal decimalDownMoney = 0;                           //库存<减值金额>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                    decimal decimalCleanMoney = 0;                          //库存<净额>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["CleanMoney"]), out decimalCleanMoney);

                    decimalStoreTotalMoney += decimalStoreMoney;
                    intStoreDetailNumber++;
                    decimalStoreDownMoney += decimalDownMoney;
                    decimalStoreCleanMoney += decimalCleanMoney;
                }
            }
            wZReduce.StoreTotalMoney = decimalStoreTotalMoney;
            wZReduce.DetailNumber = intStoreDetailNumber;
            wZReduce.StoreDownMoney = decimalStoreDownMoney;
            wZReduce.CleanMoney = decimalStoreCleanMoney;
            if (decimalStoreTotalMoney != 0)
            {
                wZReduce.PlanMoney = decimalStoreDownMoney / decimalStoreTotalMoney;
            }
            wZReduce.ExcuteTime = DateTime.Now.ToString("yyyy-MM-dd");
            wZReduce.Process = LanguageHandle.GetWord("ShengXiao").ToString().Trim();

            wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

            //重新加载
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZXCG").ToString().Trim()+"')", true);
        }
    }


    protected void BT_NewCalc_Click(object sender, EventArgs e)
    {
        //计算
        string strEditReduceCode = HF_NewReduceCode.Value;
        if (string.IsNullOrEmpty(strEditReduceCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXDJYCZDJZLB").ToString().Trim()+"')", true);
            return;
        }

        WZReduceBLL wZReduceBLL = new WZReduceBLL();
        string strWZReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strEditReduceCode + "'";
        IList listWZReduce = wZReduceBLL.GetAllWZReduces(strWZReduceHQL);
        if (listWZReduce != null && listWZReduce.Count == 1)
        {
            WZReduce wZReduce = (WZReduce)listWZReduce[0];
            if (wZReduce.Process != LanguageHandle.GetWord("ShengXiao").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJDBWSXZTBYXJS").ToString().Trim()+"')", true);
                return;
            }

            string strReduceStoreHQL = string.Format(@"select * from T_WZStore
                                            where DownCode = '{0}'", wZReduce.ReduceCode);
            DataTable dtReduceStore = ShareClass.GetDataSetFromSql(strReduceStoreHQL, "ReduceStore").Tables[0];
            decimal decimalStoreTotalMoney = 0;          //库存金额
            int intStoreDetailNumber = 0;                 //明细条数
            decimal decimalStoreDownMoney = 0;          //减值金额
            decimal decimalStoreCleanMoney = 0;              //净额
            //减值计划〈统计库存〉＝∑库存〈库存金额〉												
            //减值计划〈统计条数〉＝∑库存记录条数												
            //减值计划〈统计减值〉＝∑库存〈减值金额〉												
            //减值计划〈统计净额〉＝∑库存〈净额〉												
            //减值计划〈统计比例〉＝减值计划〈统计减值〉÷减值计划〈统计库存〉												
            //“计算”按钮可重复使用，直到该减值计划〈统计库存〉＝“0”时关闭，同步使减值计划〈进度〉＝“完成”												

            if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
            {
                foreach (DataRow drStore in dtReduceStore.Rows)
                {
                    decimal decimalStoreMoney = 0;                          //库存<库存金额>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                    decimal decimalDownMoney = 0;                           //库存<减值金额>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                    decimal decimalCleanMoney = 0;                          //库存<净额>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["CleanMoney"]), out decimalCleanMoney);

                    decimalStoreTotalMoney += decimalStoreMoney;
                    intStoreDetailNumber++;
                    decimalStoreDownMoney += decimalDownMoney;
                    decimalStoreCleanMoney += decimalCleanMoney;
                }
            }
            wZReduce.TotalStore = decimalStoreTotalMoney;
            wZReduce.TotalNumber = intStoreDetailNumber;
            wZReduce.TotalDownMoney = decimalStoreDownMoney;
            wZReduce.TotalCleanMoney = decimalStoreCleanMoney;
            if (decimalStoreTotalMoney != 0)
            {
                wZReduce.TotalRatio = decimalStoreDownMoney / decimalStoreTotalMoney;
            }
            else
            {
                wZReduce.Process = "Completed";
            }

            wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

            //重新加载
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJSCG").ToString().Trim()+"')", true);
        }
    }


    public void Export3Excel(DataTable dtData, string strFileName)
    {
        DataGrid dgControl = new DataGrid();
        dgControl.DataSource = dtData;
        dgControl.DataBind();


        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dgControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();

    }
}
