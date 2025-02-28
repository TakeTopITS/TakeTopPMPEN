using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProjectMgt.BLL;
using ProjectMgt.Model;

using System.Text;

public partial class TTRCJProjectAdjustPrice : System.Web.UI.Page
{
    private string UserCode = "";
    private int ProjectId = 0;
    private int ItemType = 0;
    private int itemno = 0;
    private string ItemName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ProjectId = Convert.ToInt32(Request.QueryString["ProjectID"]);
        UserCode = Convert.ToString(Session["UserCode"]);
        ItemType = Convert.ToInt32(Request.QueryString["itemtype"]);
        itemno = Convert.ToInt32(Request.QueryString["itemno"]);
        ItemName = Request.QueryString["itemname"].ToString();

        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
                ViewState["UrlReferrer"] = Request.UrlReferrer.ToString();

            try
            {
                //获得基准信息类型列表
                RCJShareClass.InitPerformanceType(DDL_PerformanceType, this.lb_ShowMessage);
                DDL_PerformanceType.Items.FindByValue(ItemType.ToString()).Selected = true;

                TB_ItemNo.Text = itemno.ToString();
                TB_ItemName.Text = ItemName;

                InitDataList();

                InitDataLog();
            }
            catch (Exception exp)
            {
                lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
                lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiChaXunCaoZuoChuXianY").ToString().Trim() + exp.Message;
            }
        }
    }

    private bool IsInputOK()
    {
        if (false == RCJShareClass.isNumberString(TB_ItemNum.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiShuLiangShuRuYouWuYi").ToString().Trim();
            return false;
        } 
        if (false == RCJShareClass.isNumberString(TB_DeviceAdjust.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiSheBeiDiaoZhengChanJ").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_MainAdjust.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiZhuCaiDiaoZhengChanJ").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_WageAdjust.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiRenGongDiaoZhengChan").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_MartAdjust.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiCaiLiaoDiaoZhengChan").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_MachineAdjust.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiJiXieDiaoZhengChanJi").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemTaxesBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiDiaoZhengShuiJinGeJi").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(tb_ProjectBCWS.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiYuSuanGeJiGeJiaBCWSS").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceDeviceBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiYuSuanSheBeiGeJiaShu").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceMainMaterialBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiZhuCaiYuSuanGeJiaShu").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemPriceWageBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiRenGongYuSuanGeJiaSh").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(tb_ProjectMaterialBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiCaiLiaoYuSuanGeJiaSh").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(tb_ProjectMachineBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiJiXieYuSuanGeJiaShuR").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(tb_ItemPricePurchaseFee.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiGuanLiFeiJiLiRunFeiS").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(tb_ItemPricePurchaseFeeBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiFeiLvCuoShiFeiShuRuY").ToString().Trim();
            return false;
        }
        if (false == RCJShareClass.isNumberString(TB_ItemComprehensiveFeeBudget.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiGuiFeiShuRuYouWuYing").ToString().Trim();
            return false;
        }

        if (TB_MachineAdjust.Text.Trim().Length == 0 ||
            TB_DeviceAdjust.Text.Trim().Length == 0 ||
            TB_MartAdjust.Text.Trim().Length == 0 ||
            TB_WageAdjust.Text.Trim().Length == 0 ||
            TB_MainAdjust.Text.Trim().Length == 0 ||
            TB_ItemTaxesBudget.Text.Trim().Length == 0 ||
            tb_ProjectBCWS.Text.Trim().Length == 0||
            TB_ItemNum.Text.Trim().Length == 0||
            TB_ItemPriceDeviceBudget.Text.Trim().Length == 0 ||
            TB_ItemPriceMainMaterialBudget.Text.Trim().Length == 0||
            TB_ItemPriceWageBudget.Text.Trim().Length == 0||
            tb_ProjectMaterialBudget.Text.Trim().Length == 0||
            tb_ProjectMachineBudget.Text.Trim().Length == 0||
            tb_ItemPricePurchaseFee.Text.Trim().Length == 0||
            tb_ItemPricePurchaseFeeBudget.Text.Trim().Length == 0||
            TB_ItemComprehensiveFeeBudget.Text.Trim().Length == 0
            )
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiDiaoZhengJiaGeBuNeng").ToString().Trim();
            return false;
        }

        return true;
    }

    private void InitDataList()
    {
        try
        {
            StringBuilder strSql = new StringBuilder("select * from T_RCJProjectAdjustPriceList where projectid=");
            strSql.Append(ProjectId.ToString());
            strSql.Append(" and itemtype=");
            strSql.Append(DDL_PerformanceType.SelectedValue);
            strSql.Append(" and itemno=");
            strSql.Append(itemno.ToString());
            strSql.Append(" order by AdjustID");

            DataSet ds = ShareClass.GetDataSetFromSql(strSql.ToString(), "T_RCJProjectAdjustPriceList");
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiChaXunJiaGeDiaoZheng").ToString().Trim() + exp.Message;
        }
    }

    protected void btnSavePriceAdjust_Click(object sender, EventArgs e)
    {
        if (false == IsInputOK())
            return;

        try
        {
            if(false == SaveDataList(0, 0 , LanguageHandle.GetWord("ZengJiaDiaoZhengJia").ToString().Trim() ))
                return;

            InitDataLog(); 
            
            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiJiaGeDiaoZhengXinZe").ToString().Trim();

            InitDataList();

        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiJiaGeDiaoZhengXinZen").ToString().Trim() + exp.Message;
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearInputText();

        TB_AdjustID.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text;
        TB_ItemNo.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[3].Text;
        TB_ItemName.Text = ItemName;
        TB_ItemNum.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[4].Text.Replace(",", ""); 
        TB_DeviceAdjust.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[5].Text.Replace(",", ""); 
        TB_MainAdjust.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[6].Text.Replace(",", ""); 
        TB_WageAdjust.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[7].Text.Replace(",", ""); 
        TB_MartAdjust.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[8].Text.Replace(",", ""); 
        TB_MachineAdjust.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[9].Text.Replace(",", ""); 
        tb_ProjectBCWS.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[10].Text.Replace(",", "");
        TB_ItemTaxesBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[11].Text.Replace(",", "");
        TB_ItemPriceDeviceBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[12].Text.Replace(",", "");
        TB_ItemPriceMainMaterialBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[13].Text.Replace(",", "");
        TB_ItemPriceWageBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[14].Text.Replace(",", "");
        tb_ProjectMaterialBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[15].Text.Replace(",", "");
        tb_ProjectMachineBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[16].Text.Replace(",", "");
        tb_ItemPricePurchaseFee.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[17].Text.Replace(",", "");
        tb_ItemPricePurchaseFeeBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[18].Text.Replace(",", "");
        TB_ItemComprehensiveFeeBudget.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[19].Text.Replace(",", "");
        TB_Memo.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[20].Text;
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["UrlReferrer"].ToString());
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.SelectedIndex = -1;
        InitDataList();
    }

    private void ClearInputText()
    {
        TB_ItemNo.Text = "";
        TB_ItemName.Text = "";
        TB_DeviceAdjust.Text = "";
        TB_MainAdjust.Text = "";
        TB_WageAdjust.Text = "";
        TB_MartAdjust.Text = "";
        TB_MachineAdjust.Text = "";
        TB_ItemTaxesBudget.Text = "";
        tb_ProjectBCWS.Text = "";
        TB_ItemTaxesBudget.Text = "";
        TB_ItemPriceWageBudget.Text = "";
        TB_ItemPriceDeviceBudget.Text = "";
        TB_ItemPriceMainMaterialBudget.Text = "";
        tb_ProjectMaterialBudget.Text = "";
        tb_ProjectMachineBudget.Text = "";
        tb_ItemPricePurchaseFee.Text = "";
        tb_ItemPricePurchaseFeeBudget.Text = "";
        TB_ItemComprehensiveFeeBudget.Text = "";

        lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
        lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiMo").ToString().Trim();
    }

    protected void DDL_PerformanceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitDataList();

        ClearInputText();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标经过时，行背景色变 
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#A9A9A9'");
            //鼠标移出时，行背景色变 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
        }
    }

    protected void btnEditPriceAdjust_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedIndex == -1)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeYiHangYao").ToString().Trim();
            lb_ShowMessage.Text = strMsg;
            return;
        }

        //第一个是原定价，不可以修改
        if (GridView1.Rows.Count <= 1 || GridView1.SelectedIndex == 0)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiBuNengXiuGaiYuanDin").ToString().Trim();
            return;
        }

        //修改时如果BCWP不为零，则数量不能小于已经录入的工作量
        int adjustid = Convert.ToInt32(GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text);
        if (RCJShareClass.CheckTheItemNumber(ProjectId, Convert.ToInt32(DDL_PerformanceType.SelectedValue), Convert.ToInt32(TB_ItemNo.Text), adjustid, TB_ItemNum.Text) == false)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = LanguageHandle.GetWord("XiaoXiDiShiGaiXiangBCWPBuWeiLi").ToString().Trim();
            lb_ShowMessage.Text = strMsg;
            return;
        }
        
        if (false == IsInputOK())
            return;

        try
        {
            int selectId = Convert.ToInt32(GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text);

            if (false == SaveDataList(1, selectId, "ModifyAdjustedPrice"))   //ChineseWord
                return;

            InitDataLog();

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiJiaGeDiaoZhengXiuGa").ToString().Trim();

            InitDataList();
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiJiaGeDiaoZhengXiuGai").ToString().Trim() + exp.Message;
        }
    }

    protected void btnDelPriceAdjust_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedIndex == -1)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            string strMsg = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeYiHangYao").ToString().Trim();
            lb_ShowMessage.Text = strMsg;
            return;
        }

        //第一个是原定价，不可以删除
        if (GridView1.Rows.Count <= 1 || GridView1.SelectedIndex == 0)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiBuNengShanChuYuanDi").ToString().Trim();
            return;
        }

        //查询是否有该记录,有记录则删除
        try
        {
            int selectId = Convert.ToInt32(GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text);
            if (false == SaveDataList(2, selectId, "DeleteAdjustedPrice"))   //ChineseWord
                return; 
            
            InitDataLog();

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiZhiDingJiLuShanChuC").ToString().Trim();

            InitDataList();


        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiShanChuCaoZuoChuXia").ToString().Trim() + exp.Message;
        }
    }

    private bool SaveDataList(int bAdd , int selectId,string demo)
    {
        try
        {
            StringBuilder sql = new StringBuilder("exec Pro_WorkAdjustPriceOperation ");
            sql.Append(selectId.ToString());
            sql.Append(",");
            sql.Append(bAdd.ToString());
            sql.Append(",");
            sql.Append(ProjectId.ToString());
            sql.Append(",");
            sql.Append(DDL_PerformanceType.SelectedValue);
            sql.Append(",");
            sql.Append(itemno.ToString());
            sql.Append(",");
            sql.Append(Convert.ToDouble(TB_DeviceAdjust.Text));
            sql.Append(",");
            sql.Append( Convert.ToDouble(TB_MainAdjust.Text));
            sql.Append(",");
            sql.Append(Convert.ToDouble(TB_WageAdjust.Text));
            sql.Append(",");
            sql.Append(Convert.ToDouble(TB_MartAdjust.Text));
            sql.Append(",");
            sql.Append(Convert.ToDouble(TB_MachineAdjust.Text));
            sql.Append(",");
            sql.Append(Convert.ToDouble(tb_ProjectBCWS.Text));
            sql.Append(",");
            sql.Append(TB_ItemNum.Text);
            sql.Append(",'");
            sql.Append(TB_Memo.Text);
            sql.Append("','");
            sql.Append(UserCode);
            sql.Append("','");
            sql.Append(DateTime.Now.ToString());
            sql.Append("','");
            sql.Append(demo);
            sql.Append("',");
            sql.Append(TB_ItemPriceDeviceBudget.Text);
            sql.Append(",");
            sql.Append(TB_ItemPriceMainMaterialBudget.Text);
            sql.Append(",");
            sql.Append(TB_ItemPriceWageBudget.Text);
            sql.Append(",");
            sql.Append(tb_ProjectMaterialBudget.Text);
            sql.Append(",");
            sql.Append(tb_ProjectMachineBudget.Text);
            sql.Append(",");
            sql.Append(tb_ItemPricePurchaseFee.Text);
            sql.Append(",");
            sql.Append(tb_ItemPricePurchaseFeeBudget.Text);
            sql.Append(",");
            sql.Append(TB_ItemComprehensiveFeeBudget.Text);
            sql.Append(",");
            sql.Append(TB_ItemTaxesBudget.Text);

            ShareClass.RunSqlCommand(sql.ToString());

        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShi").ToString().Trim() + exp.Message;
            return false;
        }

        return true;
    }

    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.SelectedIndex = -1;

        InitDataLog();
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标经过时，行背景色变 
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#A9A9A9'");
            //鼠标移出时，行背景色变 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
        }
    }

    private void InitDataLog()
    {
        try
        {
            StringBuilder strSql = new StringBuilder("select * from T_RCJProjectAdjustPriceLog where projectid=");
            strSql.Append(ProjectId.ToString());
            strSql.Append(" and itemtype=");
            strSql.Append(DDL_PerformanceType.SelectedValue);
            strSql.Append(" and itemno=");
            strSql.Append(itemno.ToString());
            strSql.Append(" order by ID desc");

            DataSet ds = ShareClass.GetDataSetFromSql(strSql.ToString(), "T_RCJProjectAdjustPriceLog");
            GridView2.DataSource = ds;
            GridView2.DataBind();
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiChaXunRiZhiShuJuCao").ToString().Trim() + exp.Message;
        }
    }
}
