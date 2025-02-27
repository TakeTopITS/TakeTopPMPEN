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
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "�ڳ����ݵ���", strUserCode);
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
                //����
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

                    //���¼����б�
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }

            }
            else if (cmdName == "edit")
            {
                //�޸�
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

                    //���ؿ��
                    DataStoreBinder(strReduceCode);
                }
                #region ע��
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

                //    //���ؿ��
                //    DataStoreBinder(wZReduce.ReduceCode);
                //}
                #endregion
            }
            else if (cmdName == "submit")
            {
                //�ύ
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

                    //���¼����б�
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJCG").ToString().Trim()+"')", true);
                }
            }
            else if (cmdName == "check")
            {
                //���
                string cmdArges = e.CommandArgument.ToString();
                string strStoreHQL = string.Format(@"select * from T_WZStore
                       where DownCode = '{0}'", cmdArges);
                DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                string strMessage = string.Empty;
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow drStore in dtStore.Rows)
                    {
                        decimal decimalInNumber = 0;    //�������
                        decimal decimalInMoney = 0;     //�����
                        decimal decimalOutNumber = 0;   //��������
                        decimal decimalOutPrice = 0;    //������
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
                //ִ��
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
                    decimal decimalStoreTotalMoney = 0;          //����ܶ�
                    int intStoreDetailNumber = 0;                 //��ϸ����
                    decimal decimalStoreDownMoney = 0;          //��ֵ���
                    decimal decimalStoreCleanMoney = 0;              //����
                    //��ֵ�ƻ�������ܶ���ƿ�桴����												
                    //��ֵ�ƻ�����ϸ������������¼���� �ϼ�												
                    //��ֵ�ƻ�����ֵ�����ƿ�桴��ֵ��												
                    //��ֵ�ƻ���������ƿ�桴���												
                    //��ֵ�ƻ����ƻ���ֵ������ֵ�ƻ�����ֵ���¼�ֵ�ƻ�������ܶ												
                    //��ֵ�ƻ���ִ�����ڡ���ϵͳ����												
                    //��ֵ�ƻ������ȡ�������Ч��		
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

                            decimal decimalStoreMoney = 0;                          //���<�����>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                            decimal decimalDownMoney = 0;                           //���<��ֵ���>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                            decimal decimalCleanMoney = 0;                          //���<����>
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

                    //���¼���
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZXCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
                }
            }
            else if (cmdName == "calc")
            {
                //����
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
                    decimal decimalStoreTotalMoney = 0;          //�����
                    int intStoreDetailNumber = 0;                 //��ϸ����
                    decimal decimalStoreDownMoney = 0;          //��ֵ���
                    decimal decimalStoreCleanMoney = 0;              //����
                    //��ֵ�ƻ���ͳ�ƿ�桵���ƿ�桴����												
                    //��ֵ�ƻ���ͳ�����������ƿ���¼����												
                    //��ֵ�ƻ���ͳ�Ƽ�ֵ�����ƿ�桴��ֵ��												
                    //��ֵ�ƻ���ͳ�ƾ�����ƿ�桴���												
                    //��ֵ�ƻ���ͳ�Ʊ���������ֵ�ƻ���ͳ�Ƽ�ֵ���¼�ֵ�ƻ���ͳ�ƿ�桵												
                    //�����㡱��ť���ظ�ʹ�ã�ֱ���ü�ֵ�ƻ���ͳ�ƿ�桵����0��ʱ�رգ�ͬ��ʹ��ֵ�ƻ������ȡ�������ɡ�												

                    if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
                    {
                        foreach (DataRow drStore in dtReduceStore.Rows)
                        {
                            decimal decimalStoreMoney = 0;                          //���<�����>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                            decimal decimalDownMoney = 0;                           //���<��ֵ���>
                            decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                            decimal decimalCleanMoney = 0;                          //���<����>
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

                    //���¼���
                    DataReduceBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJSCG").ToString().Trim()+"');ControlStatusCloseChange();", true);
                }
            }
            else if (cmdName == "store")
            {
                //��ֵ���
                string cmdArges = e.CommandArgument.ToString();

                //���ؿ��
                DataStoreBinder(cmdArges);
            }
        }
    }


    protected void DG_Store_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            //�����
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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���ȵ����ֵ�ƻ���');ControlStatusChange('" + strNewProgress + "');", true); 
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ����');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�ƻ���ֵֻ��ΪС��������������');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ�������쵼��');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);

            string strReduceCode = TXT_ReduceCode.Text;
            if (!string.IsNullOrEmpty(strReduceCode))
            {
                //�޸�
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strReduceCode + "'";
                IList lstReduce = wZReduceBLL.GetAllWZReduces(strReduceHQL);
                if (lstReduce != null && lstReduce.Count > 0)
                {
                    WZReduce wZReduce = (WZReduce)lstReduce[0];


                    if (wZReduce.Process == LanguageHandle.GetWord("BianZhi").ToString().Trim())
                    {
                        string strNewProgress = HF_NewProcess.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���Ȳ��Ǳ��ƣ������޸ģ�');ControlStatusChange('" + strNewProgress + "');", true); 
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
                //����
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                WZReduce wZReduce = new WZReduce();
                wZReduce.ReduceCode = CreateNewReduceCode();                //�����µļ�ֵID
                wZReduce.StoreRoom = strStoreRoom;
                wZReduce.PlanTime = DateTime.Now;
                wZReduce.PlanMoney = decimalPlanMoney;
                wZReduce.Remark = strRemark;
                wZReduce.Process = LanguageHandle.GetWord("BianZhi").ToString().Trim();
                wZReduce.MainLeader = strMainLeader;
                wZReduce.Marker = strUserCode;

                wZReduceBLL.AddWZReduce(wZReduce);
            }

            //���¼����б�
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true); 
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ����');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�ƻ���ֵֻ��ΪС��������������');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ�������쵼��');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);


            //����
            WZReduceBLL wZReduceBLL = new WZReduceBLL();
            WZReduce wZReduce = new WZReduce();
            wZReduce.ReduceCode = CreateNewReduceCode();                //�����µļ�ֵID
            wZReduce.StoreRoom = strStoreRoom;
            wZReduce.PlanTime = DateTime.Now;
            wZReduce.PlanMoney = decimalPlanMoney;
            wZReduce.Remark = strRemark;
            wZReduce.Process = LanguageHandle.GetWord("BianZhi").ToString().Trim();
            wZReduce.MainLeader = strMainLeader;
            wZReduce.Marker = strUserCode;

            wZReduceBLL.AddWZReduce(wZReduce);


            //���¼����б�
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true); 
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ����');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (!ShareClass.CheckIsNumber(strPlanMoney))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�ƻ���ֵֻ��ΪС��������������');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ�������쵼��');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            decimal decimalPlanMoney = 0;
            decimal.TryParse(strPlanMoney, out decimalPlanMoney);

            string strReduceCode = TXT_ReduceCode.Text;
            if (!string.IsNullOrEmpty(strReduceCode))
            {
                //�޸�
                WZReduceBLL wZReduceBLL = new WZReduceBLL();
                string strReduceHQL = "from WZReduce as wZReduce where ReduceCode = '" + strReduceCode + "'";
                IList lstReduce = wZReduceBLL.GetAllWZReduces(strReduceHQL);
                if (lstReduce != null && lstReduce.Count > 0)
                {
                    WZReduce wZReduce = (WZReduce)lstReduce[0];


                    if (wZReduce.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
                    {
                        string strNewProgress = HF_NewProcess.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('���Ȳ��Ǳ��ƣ������޸ģ�');ControlStatusChange('" + strNewProgress + "');", true); 
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
                //����
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�޸ĵļ�ֵ�б�');ControlStatusChange('" + strNewProgress + "');", true); 
                return;
            }

            //���¼����б�
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true); 
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ͳ����ݱ���Ϊ������');ControlStatusChange('" + strNewProgress + "');", true); 
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
                decimal decimalStoreTotalMoney = 0;          //����ܶ�
                int intStoreDetailNumber = 0;                 //��ϸ����
                decimal decimalStoreDownMoney = 0;          //��ֵ���
                decimal decimalStoreCleanMoney = 0;              //����
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow drStore in dtStore.Rows)
                    {
                        decimal decimalDownRatio = wZReduce.PlanMoney;          //���<��ֵ����>
                        decimal decimalStoreMoney = 0;                          //���<�����>
                        decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);

                        decimal decimalDownMoney = decimalStoreMoney * decimalDownRatio;//���<��ֵ���>
                        decimal decimalCleanMoney = decimalStoreMoney - decimalDownMoney;//���<����>

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

                //���¼����б�
                DataReduceBinder();
                //���ؿ��
                DataStoreBinder(wZReduce.ReduceCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ͳ����ɣ�');ControlStatusCloseChange();", true); 
            }
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ���ֵ����');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }

    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        //���<��ֵ����>
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
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ֵ����ֻ��ΪС��������������');ControlStatusChange('" + strNewProgress + "');", true); 
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

                        //��桴��ֵ������桴��������¼��ļ�ֵ����												
                        //��桴�������桴����-��桴��ֵ��												
                        decimal decimalStoreMoney = wZStore.StoreMoney; //�����
                        decimal decimalDownMoney = decimalStoreMoney * decimalNewDownRatio;//��ֵ���
                        wZStore.DownMoney = decimalDownMoney;
                        wZStore.CleanMoney = decimalStoreMoney - decimalDownMoney;

                        wZStoreBLL.UpdateWZStore(wZStore, wZStore.ID);

                        //��ֵ�ƻ�����ֵ�����ƿ�桴��ֵ��												
                        //��ֵ�ƻ���������ƿ�桴���												
                        //��ֵ�ƻ����ƻ���ֵ������ֵ�ƻ�����ֵ���¼�ֵ�ƻ�������ܶ												
                        string strReduceStoreHQL = string.Format(@"select * from T_WZStore
                                            where DownCode = '{0}'", wZReduce.ReduceCode);
                        DataTable dtReduceStore = ShareClass.GetDataSetFromSql(strReduceStoreHQL, "ReduceStore").Tables[0];

                        decimal decimalStoreDownMoney = 0;          //��ֵ���
                        decimal decimalStoreCleanMoney = 0;         //����
                        if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
                        {
                            foreach (DataRow drStore in dtReduceStore.Rows)
                            {
                                decimal decimalDownMoney2 = 0;                          //���<��ֵ���>
                                decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney2);
                                decimal decimalCleanMoney2 = 0;                         //���<����>
                                decimal.TryParse(ShareClass.ObjectToString(drStore["CleanMoney"]), out decimalCleanMoney2);

                                decimalStoreDownMoney += decimalDownMoney2;
                                decimalStoreCleanMoney += decimalCleanMoney2;
                            }
                        }
                        decimal decimalStoreTotalMoney = wZReduce.StoreTotalMoney;//��ֵ�ƻ�<����ܶ�>
                        wZReduce.StoreDownMoney = decimalStoreDownMoney;
                        wZReduce.CleanMoney = decimalStoreCleanMoney;
                        if (decimalStoreTotalMoney != 0)
                        {
                            wZReduce.PlanMoney = decimalStoreDownMoney / decimalStoreTotalMoney;
                        }
                        wZReduceBLL.UpdateWZReduce(wZReduce, wZReduce.ReduceCode);

                        //���¼��ؿ�棬��ֵ�ƻ�
                        DataStoreBinder(wZReduce.ReduceCode);
                        DataReduceBinder();

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('�޸ĳɹ���');ControlStatusCloseChange();", true); 
                    }
                }
                else
                {
                    string strNewProgress = HF_NewProcess.Value;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ���棡');ControlStatusChange('" + strNewProgress + "');", true); 
                    return;
                }
            }
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ���ֵ����');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }



    protected void BT_ExportStoreEffect_Click(object sender, EventArgs e)
    {
        //������������ֵ�ƻ������ȡ�������Ч��
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

            //��ѯ��ֵ�ƻ�
            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
                        and Process = '��Ч'", strWhereReduceCode); 
            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //�ļ�����������ֵ��š����ż�ֵ�ƻ���
            Export3Excel(dtSelectReduce, "����Ϊ��Ч�ļ�ֵ�ƻ�"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����ļ�ֵ�б�');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }

    protected void BT_ExportStore_Click(object sender, EventArgs e)
    {
        //������������ֵ�ƻ������ȡ�������Ч������桴��ֵ��š�����ֵ�ƻ�����ֵ��š�
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

            //��ѯ��ֵ�ƻ�
            //            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
            //                        and Process = '��Ч'", strWhereReduceCode); 
            //            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //��ѯ��ֵ�ƻ���ϸ�����)
            string strSelectReduceDetailHQL = string.Format(@"select s.* from T_WZReduce r
                        left join T_WZStore s on r.ReduceCode = s.DownCode
                        where r.ReduceCode in ({0})
                        and r.Process = '��Ч'", strWhereReduceCode); 
            DataTable dtSelectReduceDetail = ShareClass.GetDataSetFromSql(strSelectReduceDetailHQL, "SelectReduceDetail").Tables[0];

            //�ļ�����������ֵ��š����ż�ֵ�ƻ�����������ֵ��š����ż�ֵ�ƻ���ϸ��
            Export3Excel(dtSelectReduceDetail, "��ֵ�ƻ���ϸ"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����ļ�ֵ�б�');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }



    protected void BT_ExportStoreComplete_Click(object sender, EventArgs e)
    {
        //������������ֵ�ƻ������ȡ�������ɡ�
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

            //��ѯ��ֵ�ƻ�
            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
                        and Process = 'Completed'", strWhereReduceCode);
            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //�ļ�����������ֵ��š����ż�ֵ�ƻ�����������ֵ��š����ż�ֵ�ƻ����ϵ���
            Export3Excel(dtSelectReduce, "����Ϊ��ɵļ�ֵ�ƻ�"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����Ļ�ѹ�б�');ControlStatusChange('" + strNewProgress + "');", true); 
            return;
        }
    }

    protected void BT_ExportSend_Click(object sender, EventArgs e)
    {
        //������������ֵ�ƻ������ȡ�������ɡ������ϵ�����ֵ��š�����ֵ�ƻ�����ֵ��š�
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

            //��ѯ��ֵ�ƻ�
            //            string strSelectReduceHQL = string.Format(@"select * from T_WZReduce where ReduceCode in ({0})
            //                        and Process = 'Completed'", strWhereReduceCode);
            //            DataTable dtSelectReduce = ShareClass.GetDataSetFromSql(strSelectReduceHQL, "SelectReduce").Tables[0];

            //��ѯ���ϵ�<��ֵ���>
            string strSelectSendHQL = string.Format(@"select s.* from T_WZReduce r
                        left join T_WZSend s on r.ReduceCode = s.ReduceCode
                        where r.ReduceCode in ({0})
                        and r.Process = 'Completed'", strWhereReduceCode);
            DataTable dtSelectSend = ShareClass.GetDataSetFromSql(strSelectSendHQL, "SelectSend").Tables[0];

            //�ļ�����������ֵ��š����ż�ֵ�ƻ�����������ֵ��š����ż�ֵ�ƻ����ϵ���
            Export3Excel(dtSelectSend, "��ֵ�ƻ����ϵ�"); 

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����Ļ�ѹ�б�');ControlStatusChange('" + strNewProgress + "');", true); 
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
    ///  ���ɼ�ֵID
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

                    //��֤�µļ�ֵID�Ƿ����
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
        //�༭
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

            //���ؿ��
            DataStoreBinder(strReduceCode);
        }
    }


    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //ɾ��
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

            //���¼����б�
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
    }


    protected void BT_NewTotal_Click(object sender, EventArgs e)
    {
        //ͳ��
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
        //�ύ
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

            //���¼����б�
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJCG").ToString().Trim()+"')", true);
        }
    }


    protected void BT_NewCheck_Click(object sender, EventArgs e)
    {
        //���
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
                decimal decimalInNumber = 0;    //�������
                decimal decimalInMoney = 0;     //�����
                decimal decimalOutNumber = 0;   //��������
                decimal decimalOutPrice = 0;    //������
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
        //ִ��
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
            decimal decimalStoreTotalMoney = 0;          //����ܶ�
            int intStoreDetailNumber = 0;                 //��ϸ����
            decimal decimalStoreDownMoney = 0;          //��ֵ���
            decimal decimalStoreCleanMoney = 0;              //����
            //��ֵ�ƻ�������ܶ���ƿ�桴����												
            //��ֵ�ƻ�����ϸ������������¼���� �ϼ�												
            //��ֵ�ƻ�����ֵ�����ƿ�桴��ֵ��												
            //��ֵ�ƻ���������ƿ�桴���												
            //��ֵ�ƻ����ƻ���ֵ������ֵ�ƻ�����ֵ���¼�ֵ�ƻ�������ܶ												
            //��ֵ�ƻ���ִ�����ڡ���ϵͳ����												
            //��ֵ�ƻ������ȡ�������Ч��		
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

                    decimal decimalStoreMoney = 0;                          //���<�����>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                    decimal decimalDownMoney = 0;                           //���<��ֵ���>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                    decimal decimalCleanMoney = 0;                          //���<����>
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

            //���¼���
            DataReduceBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZXCG").ToString().Trim()+"')", true);
        }
    }


    protected void BT_NewCalc_Click(object sender, EventArgs e)
    {
        //����
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
            decimal decimalStoreTotalMoney = 0;          //�����
            int intStoreDetailNumber = 0;                 //��ϸ����
            decimal decimalStoreDownMoney = 0;          //��ֵ���
            decimal decimalStoreCleanMoney = 0;              //����
            //��ֵ�ƻ���ͳ�ƿ�桵���ƿ�桴����												
            //��ֵ�ƻ���ͳ�����������ƿ���¼����												
            //��ֵ�ƻ���ͳ�Ƽ�ֵ�����ƿ�桴��ֵ��												
            //��ֵ�ƻ���ͳ�ƾ�����ƿ�桴���												
            //��ֵ�ƻ���ͳ�Ʊ���������ֵ�ƻ���ͳ�Ƽ�ֵ���¼�ֵ�ƻ���ͳ�ƿ�桵												
            //�����㡱��ť���ظ�ʹ�ã�ֱ���ü�ֵ�ƻ���ͳ�ƿ�桵����0��ʱ�رգ�ͬ��ʹ��ֵ�ƻ������ȡ�������ɡ�												

            if (dtReduceStore != null && dtReduceStore.Rows.Count > 0)
            {
                foreach (DataRow drStore in dtReduceStore.Rows)
                {
                    decimal decimalStoreMoney = 0;                          //���<�����>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                    decimal decimalDownMoney = 0;                           //���<��ֵ���>
                    decimal.TryParse(ShareClass.ObjectToString(drStore["DownMoney"]), out decimalDownMoney);
                    decimal decimalCleanMoney = 0;                          //���<����>
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

            //���¼���
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
