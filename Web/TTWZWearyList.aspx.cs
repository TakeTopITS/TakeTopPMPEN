using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZWearyList : System.Web.UI.Page
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

            DataWearyBinder();
        }
    }

    private void DataWearyBinder()
    {
        DG_List.CurrentPageIndex = 0;


        string strWZWearyHQL = string.Format(@"select w.*,m.UserName as MainLeaderName,r.UserName as MarkerName from T_WZWeary w
                    left join T_ProjectMember m on w.MainLeader = m.UserCode
                    left join T_ProjectMember r on w.Marker = r.UserCode 
                    where w.Marker = '{0}' 
                    order by w.PlanTime desc", strUserCode);
        DataTable dtWeary = ShareClass.GetDataSetFromSql(strWZWearyHQL, "Weary").Tables[0];

        DG_List.DataSource = dtWeary;
        DG_List.DataBind();

        LB_Sql.Text = strWZWearyHQL;
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text.Trim();
        DataTable dtWeary = ShareClass.GetDataSetFromSql(strHQL, "Weary").Tables[0];

        DG_List.DataSource = dtWeary;
        DG_List.DataBind();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
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

                string strEditWearyCode = arrOperate[0];
                string strProgress = arrOperate[1];

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);

                HF_NewWearyCode.Value = strEditWearyCode;
                HF_NewProcess.Value = strProgress;
            }
            else if (cmdName == "del")
            {
                //ɾ��
                string cmdArges = e.CommandArgument.ToString();
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + cmdArges + "'";
                IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
                if (listWZWeary != null && listWZWeary.Count == 1)
                {
                    WZWeary wZWeary = (WZWeary)listWZWeary[0];

                    if (wZWeary.Process == LanguageHandle.GetWord("ShengXiao").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDWSXZTBYXSC").ToString().Trim() + "')", true);
                        return;
                    }

                    wZWearyBLL.DeleteWZWeary(wZWeary);

                    //���¼����б�
                    DataWearyBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
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
                string strWZWearyHQL = string.Format(@"select w.*,p.UserName as MainLeaderName from T_WZWeary w
                                    left join T_ProjectMember p on w.MainLeader = p.UserCode 
                                    where w.WearyCode = '{0}'", cmdArges);
                DataTable dtWeary = ShareClass.GetDataSetFromSql(strWZWearyHQL, "Weary").Tables[0];
                if (dtWeary != null && dtWeary.Rows.Count == 1)
                {
                    DataRow drWeary = dtWeary.Rows[0];

                    HF_WearyCode.Value = ShareClass.ObjectToString(drWeary["WearyCode"]);
                    TXT_WearyCode.Text = ShareClass.ObjectToString(drWeary["WearyCode"]);
                    DDL_StoreRoom.SelectedValue = ShareClass.ObjectToString(drWeary["StoreRoom"]);
                    TXT_Remark.Text = ShareClass.ObjectToString(drWeary["Remark"]);
                    HF_MainLeader.Value = ShareClass.ObjectToString(drWeary["MainLeader"]);
                    TXT_MainLeader.Text = ShareClass.ObjectToString(drWeary["MainLeaderName"]);
                }
                #region ע��
                //string cmdArges = e.CommandArgument.ToString();
                //WZWearyBLL wZWearyBLL = new WZWearyBLL();
                //string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + cmdArges + "'";
                //IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
                //if (listWZWeary != null && listWZWeary.Count == 1)
                //{
                //    WZWeary wZWeary = (WZWeary)listWZWeary[0];

                //    HF_WearyCode.Value = wZWeary.WearyCode;
                //    TXT_WearyCode.Text = wZWeary.WearyCode;
                //    DDL_StoreRoom.SelectedValue = wZWeary.StoreRoom;
                //    TXT_Remark.Text = wZWeary.Remark;
                //    TXT_MainLeader.Text = wZWeary.MainLeader;
                //}
                #endregion
            }
            else if (cmdName == "submit")
            {
                //�ύ
                string cmdArges = e.CommandArgument.ToString();
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + cmdArges + "'";
                IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
                if (listWZWeary != null && listWZWeary.Count == 1)
                {
                    WZWeary wZWeary = (WZWeary)listWZWeary[0];

                    if (wZWeary.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWBZZTBYXBP").ToString().Trim() + "')", true);
                        return;
                    }

                    wZWeary.Process = LanguageHandle.GetWord("BaoPi").ToString().Trim();

                    wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

                    //���¼����б�
                    DataWearyBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "')", true);
                }
            }
            else if (cmdName == "execute")
            {
                //ִ��
                string cmdArges = e.CommandArgument.ToString();
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + cmdArges + "'";
                IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
                if (listWZWeary != null && listWZWeary.Count == 1)
                {
                    WZWeary wZWeary = (WZWeary)listWZWeary[0];

                    if (wZWeary.Process != "Approved")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWPZZTBYXZX").ToString().Trim() + "')", true);
                        return;
                    }

                    string strStoreHQL = string.Format(@"select * from T_WZStore
                    where DownDesc = 0
                    and WearyDesc = 0
                    and StockCode = '{0}'
                    and StoreMoney > 0
                    and extract(year from EndOutTime) <= (extract(year from now())-{1})", wZWeary.StoreRoom, wZWeary.TotalYear);
                    DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                    decimal decimalTotalMoney = 0;          //�ܼ�ֵ
                    int intTotalNumber = 0;                 //������
                    if (dtStore != null && dtStore.Rows.Count > 0)
                    {
                        foreach (DataRow drStore in dtStore.Rows)
                        {
                            decimal decimalStoreMoney = 0;
                            decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                            decimalTotalMoney += decimalStoreMoney;
                            intTotalNumber++;

                            string strUpdateStoreHQL = string.Format(@"update T_WZStore 
                                set WearyCode = '{0}',
                                WearyDesc=-1 
                                where ID = {1}", wZWeary.WearyCode, ShareClass.ObjectToString(drStore["ID"]));
                            ShareClass.RunSqlCommand(strUpdateStoreHQL);
                        }
                    }

                    wZWeary.WearyTotalMoney = decimalTotalMoney;
                    wZWeary.WearyBalance = decimalTotalMoney;
                    wZWeary.RowNumber = intTotalNumber;
                    wZWeary.OverNumber = intTotalNumber;
                    wZWeary.Process = LanguageHandle.GetWord("ShengXiao").ToString().Trim();

                    wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

                    //���¼����б�
                    DataWearyBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZXCG").ToString().Trim() + "')", true);
                }
            }
            else if (cmdName == "calc")
            {
                //����
                string cmdArges = e.CommandArgument.ToString();
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + cmdArges + "'";
                IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
                if (listWZWeary != null && listWZWeary.Count == 1)
                {
                    WZWeary wZWeary = (WZWeary)listWZWeary[0];

                    if (wZWeary.Process != LanguageHandle.GetWord("ShengXiao").ToString().Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWSXZTBYXJS").ToString().Trim() + "')", true);
                        return;
                    }

                    string strStoreHQL = string.Format(@"select WearyCode,SUM(StoreMoney) as TotalStoreMoney,COUNT(1) as RowNumber from T_WZStore
                            where WearyCode= '{0}'
                            group by WearyCode", wZWeary.WearyCode);
                    DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                    decimal decimalTotalMoney = 0;          //�ܼ�ֵ
                    int intTotalNumber = 0;                 //������
                    if (dtStore != null && dtStore.Rows.Count > 0)
                    {
                        decimal.TryParse(ShareClass.ObjectToString(dtStore.Rows[0]["TotalStoreMoney"]), out decimalTotalMoney);
                        int.TryParse(ShareClass.ObjectToString(dtStore.Rows[0]["RowNumber"]), out intTotalNumber);
                    }

                    wZWeary.WearyBalance = decimalTotalMoney;
                    wZWeary.OverNumber = intTotalNumber;
                    if (decimalTotalMoney == 0)
                    {
                        wZWeary.Process = "Completed";
                    }

                    wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

                    //���¼����б�
                    DataWearyBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSCG").ToString().Trim() + "')", true);
                }
            }
            else if (cmdName == "total")
            {
                //ͳ��
                string cmdArges = e.CommandArgument.ToString();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickTotal('" + cmdArges + "')", true);
                return;
            }
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strStoreRoom = DDL_StoreRoom.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();
            string strMainLeader = HF_MainLeader.Value; //TXT_MainLeader.Text.Trim();

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ����');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ�������쵼��');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }

            string strWearyCode = TXT_WearyCode.Text;

            if (!string.IsNullOrEmpty(strWearyCode))
            {
                //�޸�
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                string strWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + strWearyCode + "'";
                IList lstWeary = wZWearyBLL.GetAllWZWearys(strWearyHQL);
                if (lstWeary != null && lstWeary.Count > 0)
                {
                    WZWeary wZWeary = (WZWeary)lstWeary[0];

                    if (wZWeary.Process == LanguageHandle.GetWord("ShengXiao").ToString().Trim())
                    {
                        string strNewProgress = HF_NewProcess.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����Ϊ��Ч״̬���������޸ģ�');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                        return;
                    }

                    wZWeary.StoreRoom = strStoreRoom;
                    wZWeary.Remark = strRemark;
                    wZWeary.MainLeader = strMainLeader;

                    wZWearyBLL.UpdateWZWeary(wZWeary, strWearyCode);
                }
            }
            else
            {
                //����
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                WZWeary wZWeary = new WZWeary();
                wZWeary.WearyCode = CreateNewWearyCode();               //�����µĻ�ѹID
                wZWeary.StoreRoom = strStoreRoom;
                wZWeary.PlanTime = DateTime.Now;
                wZWeary.WearyTotalMoney = 0;
                wZWeary.RowNumber = 0;
                wZWeary.WearyBalance = 0;
                wZWeary.OverNumber = 0;
                wZWeary.Remark = strRemark;
                wZWeary.Process = LanguageHandle.GetWord("BianZhi").ToString().Trim();
                wZWeary.MainLeader = strMainLeader;
                wZWeary.Marker = strUserCode;

                //����ͳ�����
                wZWeary.TotalYear = 0;

                wZWearyBLL.AddWZWeary(wZWeary);
            }

            //���¼����б�
            DataWearyBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true);   //ChineseWord
        }
        catch (Exception ex)
        { }
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        try
        {
            string strStoreRoom = DDL_StoreRoom.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();
            string strMainLeader = HF_MainLeader.Value; //TXT_MainLeader.Text.Trim();

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ����');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ�������쵼��');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }

            //����
            WZWearyBLL wZWearyBLL = new WZWearyBLL();
            WZWeary wZWeary = new WZWeary();
            wZWeary.WearyCode = CreateNewWearyCode();               //�����µĻ�ѹID
            wZWeary.StoreRoom = strStoreRoom;
            wZWeary.PlanTime = DateTime.Now;
            wZWeary.WearyTotalMoney = 0;
            wZWeary.RowNumber = 0;
            wZWeary.WearyBalance = 0;
            wZWeary.OverNumber = 0;
            wZWeary.Remark = strRemark;
            wZWeary.Process = LanguageHandle.GetWord("BianZhi").ToString().Trim();
            wZWeary.MainLeader = strMainLeader;
            wZWeary.Marker = strUserCode;

            //����ͳ�����
            wZWeary.TotalYear = 0;

            wZWearyBLL.AddWZWeary(wZWeary);

            //���¼����б�
            DataWearyBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true);   //ChineseWord
        }
        catch (Exception ex)
        { }
    }

    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        try
        {
            string strStoreRoom = DDL_StoreRoom.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();
            string strMainLeader = HF_MainLeader.Value; //TXT_MainLeader.Text.Trim();

            if (string.IsNullOrEmpty(strStoreRoom))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ����');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }
            if (string.IsNullOrEmpty(strMainLeader))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('��ѡ�������쵼��');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }

            string strWearyCode = TXT_WearyCode.Text;

            if (!string.IsNullOrEmpty(strWearyCode))
            {
                //�޸�
                WZWearyBLL wZWearyBLL = new WZWearyBLL();
                string strWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + strWearyCode + "'";
                IList lstWeary = wZWearyBLL.GetAllWZWearys(strWearyHQL);
                if (lstWeary != null && lstWeary.Count > 0)
                {
                    WZWeary wZWeary = (WZWeary)lstWeary[0];

                    if (wZWeary.Process == LanguageHandle.GetWord("ShengXiao").ToString().Trim())
                    {
                        string strNewProgress = HF_NewProcess.Value;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����Ϊ��Ч״̬���������޸ģ�');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                        return;
                    }

                    wZWeary.StoreRoom = strStoreRoom;
                    wZWeary.Remark = strRemark;
                    wZWeary.MainLeader = strMainLeader;

                    wZWearyBLL.UpdateWZWeary(wZWeary, strWearyCode);
                }
            }
            else
            {
                //����
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�޸ĵĻ�ѹ�б�');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }

            //���¼����б�
            DataWearyBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ɹ���');ControlStatusCloseChange();", true);   //ChineseWord
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

        HF_WearyCode.Value = "";
        TXT_WearyCode.Text = "";
        DDL_StoreRoom.SelectedValue = "";
        TXT_Remark.Text = "";
        TXT_MainLeader.Text = "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusCloseChange();", true);
    }


    protected void BT_Total_Click(object sender, EventArgs e)
    {
        string strWearyCode = HF_TotalWearyCode.Value; //TXT_WearyCode.Text;
        if (!string.IsNullOrEmpty(strWearyCode))
        {
            string strYear = HF_TotalYear.Value.Trim(); //TXT_Year.Text.Trim();
            if (!ShareClass.CheckIsNumber(strYear))
            {
                string strNewProgress = HF_NewProcess.Value;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ͳ����ݱ���Ϊ������');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
                return;
            }

            int intYear = 0;
            int.TryParse(strYear, out intYear);

            WZWearyBLL wZWearyBLL = new WZWearyBLL();
            string strWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + strWearyCode + "'";
            IList lstWeary = wZWearyBLL.GetAllWZWearys(strWearyHQL);
            if (lstWeary != null && lstWeary.Count > 0)
            {
                WZWeary wZWeary = (WZWeary)lstWeary[0];

                string strStoreHQL = string.Format(@"select * from T_WZStore
                    where DownDesc = 0
                    and WearyDesc = 0
                    and StockCode = '{0}'
                    and StoreMoney > 0
                    and extract(year from EndOutTime) <= (extract(year from now())-{1})", wZWeary.StoreRoom, intYear);
                DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
                decimal decimalTotalMoney = 0;          //�ܼ�ֵ
                int intTotalNumber = 0;                 //������
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow drStore in dtStore.Rows)
                    {
                        decimal decimalStoreMoney = 0;
                        decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                        decimalTotalMoney += decimalStoreMoney;
                        intTotalNumber++;
                    }
                }
                wZWeary.WearyTotalMoney = decimalTotalMoney;
                wZWeary.WearyBalance = decimalTotalMoney;
                wZWeary.RowNumber = intTotalNumber;
                wZWeary.OverNumber = intTotalNumber;

                //����ͳ�����
                wZWeary.TotalYear = intYear;

                wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

                //���¼����б�
                DataWearyBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('ͳ����ɣ�');ControlStatusCloseChange();", true);   //ChineseWord
            }
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ���ѹ����');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_ExportWearyEffect_Click(object sender, EventArgs e)
    {
        //������ѹ����ѹ��ϸ  ������������ѹ�ƻ������ȡ�������Ч��
        string strWearyCode = Request.Form["cb_WearyCode"];
        if (!string.IsNullOrEmpty(strWearyCode))
        {
            string[] arrWearyCode = strWearyCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereWearyCode = string.Empty;
            for (int i = 0; i < arrWearyCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrWearyCode[i]))
                {
                    strWhereWearyCode += "'" + arrWearyCode[i] + "',";
                }
            }
            strWhereWearyCode = strWhereWearyCode.EndsWith(",") ? strWhereWearyCode.TrimEnd(',') : strWhereWearyCode;

            //��ѯ��ѹ�ƻ�
            string strSelectWearyHQL = string.Format(@"select * from T_WZWeary where WearyCode in ({0})
                        and Process = '��Ч'", strWhereWearyCode);   //ChineseWord
            DataTable dtSelectWeary = ShareClass.GetDataSetFromSql(strSelectWearyHQL, "SelectWeary").Tables[0];

            //�ļ�����������ѹ��š����Ż�ѹ�ƻ���

            Export3Excel(dtSelectWeary, "Status Is Effective Backlog Plan");   //ChineseWord

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);

        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����Ļ�ѹ�б�');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_ExportWeary_Click(object sender, EventArgs e)
    {
        //������ѹ����ѹ��ϸ  ������������ѹ�ƻ������ȡ�������Ч������桴��ѹ��š�����ѹ�ƻ�����ѹ��š�
        string strWearyCode = Request.Form["cb_WearyCode"];
        if (!string.IsNullOrEmpty(strWearyCode))
        {
            string[] arrWearyCode = strWearyCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereWearyCode = string.Empty;
            for (int i = 0; i < arrWearyCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrWearyCode[i]))
                {
                    strWhereWearyCode += "'" + arrWearyCode[i] + "',";
                }
            }
            strWhereWearyCode = strWhereWearyCode.EndsWith(",") ? strWhereWearyCode.TrimEnd(',') : strWhereWearyCode;

            //��ѯ��ѹ�ƻ�
            //            string strSelectWearyHQL = string.Format(@"select * from T_WZWeary where WearyCode in ({0})
            //                        and Process = '��Ч'", strWhereWearyCode);   //ChineseWord
            //            DataTable dtSelectWeary = ShareClass.GetDataSetFromSql(strSelectWearyHQL, "SelectWeary").Tables[0];


            //��ѯ��ѹ�ƻ���ϸ
            string strSelectWearyDetailHQL = string.Format(@"select s.* from T_WZWeary w
                        left join T_WZStore s on w.WearyCode = s.WearyCode
                        where w.WearyCode in ({0})
                        and w.Process = '��Ч'", strWhereWearyCode);   //ChineseWord
            DataTable dtSelectWearyDetail = ShareClass.GetDataSetFromSql(strSelectWearyDetailHQL, "SelectWearyDetail").Tables[0];

            //�ļ�����������ѹ��š����Ż�ѹ�ƻ�����������ѹ��š����Ż�ѹ�ƻ���ϸ
            //ExcelUtils.Export2Excel(dtSelectWeary, "��ѹ,�б�,����", "sheet1", "������ѹ��š����Ż�ѹ�ƻ���");


            //Export3Excel(dtSelectWeary, "Status Is Effective Backlog Plan");

            Export3Excel(dtSelectWearyDetail, "Backlog Plan Details");   //ChineseWord

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����Ļ�ѹ�б�');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
            return;
        }
    }



    protected void BT_ExportWearyComplete_Click(object sender, EventArgs e)
    {
        //������ѹ�����ϵ�  ������������ѹ�ƻ������ȡ�������ɡ�
        string strWearyCode = Request.Form["cb_WearyCode"];
        if (!string.IsNullOrEmpty(strWearyCode))
        {
            string[] arrWearyCode = strWearyCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereWearyCode = string.Empty;
            for (int i = 0; i < arrWearyCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrWearyCode[i]))
                {
                    strWhereWearyCode += "'" + arrWearyCode[i] + "',";
                }
            }
            strWhereWearyCode = strWhereWearyCode.EndsWith(",") ? strWhereWearyCode.TrimEnd(',') : strWhereWearyCode;

            //��ѯ��ѹ�ƻ�
            string strSelectWearyHQL = string.Format(@"select * from T_WZWeary where WearyCode in ({0})
                        and Process = 'Completed'", strWhereWearyCode);
            DataTable dtSelectWeary = ShareClass.GetDataSetFromSql(strSelectWearyHQL, "SelectWeary").Tables[0];

            //�ļ�����������ѹ��š����Ż�ѹ�ƻ���
            Export3Excel(dtSelectWeary, "Status Is Completed Backlog Plan");   //ChineseWord

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����Ļ�ѹ�б�');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
            return;
        }
    }


    protected void BT_ExportSend_Click(object sender, EventArgs e)
    {
        //������ѹ�����ϵ�  ������������ѹ�ƻ������ȡ�������ɡ������ϵ�����ѹ��š�����ѹ�ƻ�����ѹ��š�
        string strWearyCode = Request.Form["cb_WearyCode"];
        if (!string.IsNullOrEmpty(strWearyCode))
        {
            string[] arrWearyCode = strWearyCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strWhereWearyCode = string.Empty;
            for (int i = 0; i < arrWearyCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrWearyCode[i]))
                {
                    strWhereWearyCode += "'" + arrWearyCode[i] + "',";
                }
            }
            strWhereWearyCode = strWhereWearyCode.EndsWith(",") ? strWhereWearyCode.TrimEnd(',') : strWhereWearyCode;

            //��ѯ��ѹ�ƻ�
            //            string strSelectWearyHQL = string.Format(@"select * from T_WZWeary where WearyCode in ({0})
            //                        and Process = 'Completed'", strWhereWearyCode);
            //            DataTable dtSelectWeary = ShareClass.GetDataSetFromSql(strSelectWearyHQL, "SelectWeary").Tables[0];

            //��ѯ���ϵ�<��ѹ���>
            string strSelectSendHQL = string.Format(@"select s.* from T_WZWeary w
                        left join T_WZSend s on w.WearyCode = s.WearyCode
                        where w.WearyCode in ({0})
                        and w.Process = 'Completed'", strWhereWearyCode);
            DataTable dtSelectSend = ShareClass.GetDataSetFromSql(strSelectSendHQL, "SelectSend").Tables[0];

            //�ļ�����������ѹ��š����Ż�ѹ�ƻ�����������ѹ��š����Ż�ѹ�ƻ����ϵ���
            //Export3Excel(dtSelectWeary, "Status Is Completed Backlog Plan");

            Export3Excel(dtSelectSend, "Backlog Plan Material Order");   //ChineseWord

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('����ѡ��Ҫ�����Ļ�ѹ�б�');ControlStatusChange('" + strNewProgress + "');", true);   //ChineseWord
            return;
        }
    }

    /// <summary>
    ///  ���ɻ�ѹID
    /// </summary>
    private string CreateNewWearyCode()
    {
        string strNewWearyCode = string.Empty;
        try
        {
            lock (this)
            {
                bool isExist = true;
                string strWearyCodeHQL = string.Format("select count(1) as RowNumber from T_WZWeary where to_char( PlanTime, 'yyyy-mm-dd' ) like '%{0}%'", DateTime.Now.ToString("yyyy-MM"));
                DataTable dtWearyCode = ShareClass.GetDataSetFromSql(strWearyCodeHQL, "SendCode").Tables[0];
                int intWearyCodeNumber = int.Parse(dtWearyCode.Rows[0]["RowNumber"].ToString());
                intWearyCodeNumber = intWearyCodeNumber + 1;
                string strYear = DateTime.Now.Year.ToString();
                string strMonth = DateTime.Now.Month.ToString();
                do
                {
                    StringBuilder sbWearyCode = new StringBuilder();
                    for (int j = 3 - intWearyCodeNumber.ToString().Length; j > 0; j--)
                    {
                        sbWearyCode.Append(" ");
                    }
                    if (strMonth.Length == 1)
                    {
                        strMonth = "0" + strMonth;
                    }
                    strNewWearyCode = strYear + "" + strMonth + "-" + sbWearyCode.ToString() + intWearyCodeNumber.ToString();

                    //��֤�µĻ�ѹID�Ƿ����
                    string strCheckNewWearyCodeHQL = "select count(1) as RowNumber from T_WZWeary where WearyCode = '" + strNewWearyCode + "'";
                    DataTable dtCheckNewWearyCode = ShareClass.GetDataSetFromSql(strCheckNewWearyCodeHQL, "CheckNewWearyCode").Tables[0];
                    int intCheckNewWearyCode = int.Parse(dtCheckNewWearyCode.Rows[0]["RowNumber"].ToString());
                    if (intCheckNewWearyCode == 0)
                    {
                        isExist = false;
                    }
                    else
                    {
                        intWearyCodeNumber++;
                    }
                } while (isExist);
            }
        }
        catch (Exception ex) { }
        return strNewWearyCode;
    }




    protected void BT_NewEdit_Click(object sender, EventArgs e)
    {
        //�༭
        string strEditWearyCode = HF_NewWearyCode.Value;
        if (string.IsNullOrEmpty(strEditWearyCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJYLB").ToString().Trim() + "')", true);
            return;
        }

        string strWZWearyHQL = string.Format(@"select w.*,p.UserName as MainLeaderName from T_WZWeary w
                                    left join T_ProjectMember p on w.MainLeader = p.UserCode 
                                    where w.WearyCode = '{0}'", strEditWearyCode);
        DataTable dtWeary = ShareClass.GetDataSetFromSql(strWZWearyHQL, "Weary").Tables[0];
        if (dtWeary != null && dtWeary.Rows.Count == 1)
        {
            DataRow drWeary = dtWeary.Rows[0];

            HF_WearyCode.Value = ShareClass.ObjectToString(drWeary["WearyCode"]);
            TXT_WearyCode.Text = ShareClass.ObjectToString(drWeary["WearyCode"]);
            DDL_StoreRoom.SelectedValue = ShareClass.ObjectToString(drWeary["StoreRoom"]);
            TXT_Remark.Text = ShareClass.ObjectToString(drWeary["Remark"]);
            HF_MainLeader.Value = ShareClass.ObjectToString(drWeary["MainLeader"]);
            TXT_MainLeader.Text = ShareClass.ObjectToString(drWeary["MainLeaderName"]);

        }

        string strNewProgress = HF_NewProcess.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
    }


    protected void BT_NewDelete_Click(object sender, EventArgs e)
    {
        //ɾ��
        string strEditWearyCode = HF_NewWearyCode.Value;
        if (string.IsNullOrEmpty(strEditWearyCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJYLB").ToString().Trim() + "')", true);
            return;
        }

        WZWearyBLL wZWearyBLL = new WZWearyBLL();
        string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + strEditWearyCode + "'";
        IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
        if (listWZWeary != null && listWZWeary.Count == 1)
        {
            WZWeary wZWeary = (WZWeary)listWZWeary[0];

            if (wZWeary.Process == LanguageHandle.GetWord("ShengXiao").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDWSXZTBYXSC").ToString().Trim() + "')", true);
                return;
            }

            wZWearyBLL.DeleteWZWeary(wZWeary);

            //���¼����б�
            DataWearyBinder();

            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "');ControlStatusCloseChange();", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
    }


    protected void BT_NewTotal_Click(object sender, EventArgs e)
    {
        //ͳ��
        string strEditWearyCode = HF_NewWearyCode.Value;
        if (string.IsNullOrEmpty(strEditWearyCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJYLB").ToString().Trim() + "')", true);
            return;
        }

        string strNewProgress = HF_NewProcess.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickTotal('" + strEditWearyCode + "');ControlStatusChange('" + strNewProgress + "');", true);
        return;
    }


    protected void BT_NewSubmit_Click(object sender, EventArgs e)
    {
        //�ύ
        string strEditWearyCode = HF_NewWearyCode.Value;
        if (string.IsNullOrEmpty(strEditWearyCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJYLB").ToString().Trim() + "')", true);
            return;
        }

        WZWearyBLL wZWearyBLL = new WZWearyBLL();
        string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + strEditWearyCode + "'";
        IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
        if (listWZWeary != null && listWZWeary.Count == 1)
        {
            WZWeary wZWeary = (WZWeary)listWZWeary[0];

            string strNewProgress = HF_NewProcess.Value;
            if (wZWeary.Process != LanguageHandle.GetWord("BianZhi").ToString().Trim())
            {

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWBZZTBYXBP").ToString().Trim() + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            wZWeary.Process = LanguageHandle.GetWord("BaoPi").ToString().Trim();

            wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

            //���¼����б�
            DataWearyBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJCG").ToString().Trim() + "');ControlStatusCloseChange();", true);
        }
    }


    protected void BT_NewExecute_Click(object sender, EventArgs e)
    {
        //ִ��
        string strEditWearyCode = HF_NewWearyCode.Value;
        if (string.IsNullOrEmpty(strEditWearyCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJYLB").ToString().Trim() + "')", true);
            return;
        }

        WZWearyBLL wZWearyBLL = new WZWearyBLL();
        string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + strEditWearyCode + "'";
        IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
        if (listWZWeary != null && listWZWeary.Count == 1)
        {
            WZWeary wZWeary = (WZWeary)listWZWeary[0];

            string strNewProgress = HF_NewProcess.Value;
            if (wZWeary.Process != "Approved")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWPZZTBYXZX").ToString().Trim() + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            string strStoreHQL = string.Format(@"select * from T_WZStore
                    where DownDesc = 0
                    and WearyDesc = 0
                    and StockCode = '{0}'
                    and StoreMoney > 0
                    and extract(year from EndOutTime) <= (extract(year from now())-{1})", wZWeary.StoreRoom, wZWeary.TotalYear);
            DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
            decimal decimalTotalMoney = 0;          //�ܼ�ֵ
            int intTotalNumber = 0;                 //������
            if (dtStore != null && dtStore.Rows.Count > 0)
            {
                foreach (DataRow drStore in dtStore.Rows)
                {
                    decimal decimalStoreMoney = 0;
                    decimal.TryParse(ShareClass.ObjectToString(drStore["StoreMoney"]), out decimalStoreMoney);
                    decimalTotalMoney += decimalStoreMoney;
                    intTotalNumber++;

                    string strUpdateStoreHQL = string.Format(@"update T_WZStore 
                                set WearyCode = '{0}',
                                WearyDesc=-1 
                                where ID = {1}", wZWeary.WearyCode, ShareClass.ObjectToString(drStore["ID"]));
                    ShareClass.RunSqlCommand(strUpdateStoreHQL);
                }
            }

            wZWeary.WearyTotalMoney = decimalTotalMoney;
            wZWeary.WearyBalance = decimalTotalMoney;
            wZWeary.RowNumber = intTotalNumber;
            wZWeary.OverNumber = intTotalNumber;
            wZWeary.Process = LanguageHandle.GetWord("ShengXiao").ToString().Trim();

            wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

            //���¼����б�
            DataWearyBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZXCG").ToString().Trim() + "');ControlStatusCloseChange();", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
        }
    }


    protected void BT_NewCalc_Click(object sender, EventArgs e)
    {
        //����
        string strEditWearyCode = HF_NewWearyCode.Value;
        if (string.IsNullOrEmpty(strEditWearyCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDJYLB").ToString().Trim() + "')", true);
            return;
        }

        WZWearyBLL wZWearyBLL = new WZWearyBLL();
        string strWZWearyHQL = "from WZWeary as wZWeary where WearyCode = '" + strEditWearyCode + "'";
        IList listWZWeary = wZWearyBLL.GetAllWZWearys(strWZWearyHQL);
        if (listWZWeary != null && listWZWeary.Count == 1)
        {
            WZWeary wZWeary = (WZWeary)listWZWeary[0];

            string strNewProgress = HF_NewProcess.Value;
            if (wZWeary.Process != LanguageHandle.GetWord("ShengXiao").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJDBWSXZTBYXJS").ToString().Trim() + "');ControlStatusChange('" + strNewProgress + "');", true);
                return;
            }

            string strStoreHQL = string.Format(@"select WearyCode,SUM(StoreMoney) as TotalStoreMoney,COUNT(1) as RowNumber from T_WZStore
                            where WearyCode= '{0}'
                            group by WearyCode", wZWeary.WearyCode);
            DataTable dtStore = ShareClass.GetDataSetFromSql(strStoreHQL, "Store").Tables[0];
            decimal decimalTotalMoney = 0;          //�ܼ�ֵ
            int intTotalNumber = 0;                 //������
            if (dtStore != null && dtStore.Rows.Count > 0)
            {
                decimal.TryParse(ShareClass.ObjectToString(dtStore.Rows[0]["TotalStoreMoney"]), out decimalTotalMoney);
                int.TryParse(ShareClass.ObjectToString(dtStore.Rows[0]["RowNumber"]), out intTotalNumber);
            }

            wZWeary.WearyBalance = decimalTotalMoney;
            wZWeary.OverNumber = intTotalNumber;
            if (decimalTotalMoney == 0)
            {
                wZWeary.Process = "Completed";
            }

            wZWearyBLL.UpdateWZWeary(wZWeary, wZWeary.WearyCode);

            //���¼����б�
            DataWearyBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSCG").ToString().Trim() + "');ControlStatusCloseChange();", true);
        }
        else
        {
            string strNewProgress = HF_NewProcess.Value;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strNewProgress + "');", true);
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
