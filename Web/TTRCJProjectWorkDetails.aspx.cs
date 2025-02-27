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

public partial class T_RCJProjectWorkDetails : System.Web.UI.Page
{
    private string UserCode = "";
    private int ProjectId = 0;
    private int ItemType = 0;
    private int itemno = 0;
    private int AdjustId = 0;
    private double TotalBudget = 0;
    private double itemnum = 0;
    private string ItemName = string.Empty;
    private string ItemContent = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ProjectId = Convert.ToInt32(Request.QueryString["ProjectID"]);
        UserCode = Convert.ToString(Session["UserCode"]);
        ItemType = Convert.ToInt32(Request.QueryString["itemtype"]);
        itemno = Convert.ToInt32(Request.QueryString["itemno"]);
        AdjustId = Convert.ToInt32(Request.QueryString["adjustid"]);
        TotalBudget = Convert.ToDouble(Request.QueryString["Budget"]);
        itemnum = Convert.ToDouble(Request.QueryString["itemnum"]);
        ItemName = Convert.ToString(Request.QueryString["itemname"]);
        ItemContent = Convert.ToString(Request.QueryString["itemContent"]);

        if (!IsPostBack)
        {
            try
            {
                Session["UrlReferrer_Details"] = Request.Url.ToString();

                //��û�׼��Ϣ�����б�
                RCJShareClass.InitPerformanceType(DDL_PerformanceType,this.lb_ShowMessage);                
                DDL_PerformanceType.Items.FindByValue(ItemType.ToString()).Selected = true;

                //��ʼ������Id�б�
                TB_ItemNo.Text = itemno.ToString();

                TB_ItemName.Text = ItemName;
                TB_ItemContent.Text = ItemContent;


                //��ʼ�������
                GetAllYearsList(ProjectId, DDL_PerformanceType.SelectedValue, itemno);
                DDL_YearList.SelectedIndex = 0;
                GetAllMonthsList(ProjectId, DDL_PerformanceType.SelectedValue, itemno, DDL_YearList.SelectedValue);

                InitAllDataList(sender , e ,0,0);

            }
            catch (Exception exp)
            {
                lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiCaoZuoChuXianYiChang").ToString().Trim() + exp.Message;
            }
        }
    }

    private void GetAllYearsList(int pid, string tid, int iid)
    {
        try
        {
            StringBuilder sql = new StringBuilder(" select distinct ProjectYear from T_RCJProjectCostPerformanceBenchmar where ProjectID =");
            sql.Append(pid.ToString());
            sql.Append(" and itemtype=");
            sql.Append(tid);
            sql.Append(" and itemno=");
            sql.Append(iid);

            DataSet ds = ShareClass.GetDataSetFromSql(sql.ToString(), "T_RCJProjectCostPerformanceBenchmar");

            DDL_YearList.DataSource = ds;
            DDL_YearList.DataBind();

            DDL_YearListConfirm.DataSource = ds;
            DDL_YearListConfirm.DataBind();

            DDL_YearListMoney.DataSource = ds;
            DDL_YearListMoney.DataBind();
        }
        catch (Exception exp)
        {
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiHuoQuNianLieBiaoCaoZ").ToString().Trim() + exp.Message;
        }
    }

    private void GetAllMonthsList(int pid, string tid, int iid, string year)
    {
        try
        {
            StringBuilder sql = new StringBuilder(" select ProjectMonth from T_RCJProjectCostPerformanceBenchmar where ProjectID =");
            sql.Append(pid.ToString());
            sql.Append(" and itemtype=");
            sql.Append(tid);
            sql.Append(" and itemno=");
            sql.Append(iid);
            sql.Append(" and ProjectYear=");
            sql.Append(year);

            DataSet ds = ShareClass.GetDataSetFromSql(sql.ToString(), "T_RCJProjectCostPerformanceBenchmar");

            DDL_MonthList.DataSource = ds;
            DDL_MonthList.DataBind();

            DDL_MonthListConfirm.DataSource = ds;
            DDL_MonthListConfirm.DataBind();

            DDL_MonthListMoney.DataSource = ds;
            DDL_MonthListMoney.DataBind();
         }
        catch (Exception exp)
        {
            lb_ShowMessage.Text = LanguageHandle.GetWord("CuoWuDiShiHuoQuYueLieBiaoCaoZu").ToString().Trim() + exp.Message;
        }   
    }

    private void InitAllDataList(object sender, EventArgs e, int detailsSelectedid, int confirmSelectedid)
    {
        ClearInputText();

        InitDataDetailsList();

        gvWorkDetails.SelectedIndex = -1;
        if (gvWorkDetails.Rows.Count > 0)
        {
            gvWorkDetails.SelectedIndex = detailsSelectedid;
            //detailsSelectedID = 0;
            gvWorkDetails_SelectedIndexChanged(sender, e);
            int confirmID = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
            InitDataConfirmList(itemno, confirmID);

            gvWorkConfirm.SelectedIndex = -1;
            if (gvWorkConfirm.Rows.Count > 0)
            {
                gvWorkConfirm.SelectedIndex = confirmSelectedid;
                //confirmSelectedID = 0;
                int iid = Convert.ToInt32(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text);
                InitDataListMoney(itemno, confirmID, iid);
            }
            else
            {
                InitDataListMoney(itemno, -1, -1);
            }
        }
        else
        {
            //���GridView��������ʾ
            InitDataListMoney(itemno, -1, -1);

            InitDataListMoney(itemno, -1, -1);
        }
    }

    private void InitDataDetailsList()
    {
        StringBuilder sql = new StringBuilder(" select * from V_RCJProjectWorkDetails where ProjectID =");
        sql.Append(ProjectId.ToString());
        sql.Append(" and itemtype=");
        sql.Append(DDL_PerformanceType.SelectedValue);
        sql.Append(" and itemno=");
        sql.Append(itemno.ToString());
        sql.Append(" and AdjustId=");
        sql.Append(AdjustId.ToString());

        DataSet ds = ShareClass.GetDataSetFromSql(sql.ToString(), "V_RCJProjectWorkDetails");

        gvWorkDetails.DataSource = ds;
        gvWorkDetails.DataBind();

    }

    private void InitDataConfirmList(int itemno, int confirmID)
    {
        StringBuilder sql = new StringBuilder(" select * from V_RCJProjectWorkConfirm where ProjectID =");
        sql.Append(ProjectId.ToString());
        sql.Append(" and itemtype=");
        sql.Append(DDL_PerformanceType.SelectedValue);
        sql.Append(" and itemno=");
        sql.Append(itemno.ToString());
        sql.Append(" and WorkConfirmID=");
        sql.Append(confirmID.ToString());

        DataSet ds = ShareClass.GetDataSetFromSql(sql.ToString(), "V_RCJProjectWorkConfirm");

        gvWorkConfirm.DataSource = ds;
        gvWorkConfirm.DataBind();

    }

    protected void gvWorkDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearInputText();

            TB_ID.Text = gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[1].Text;
            TB_ItemNo.Text = gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[3].Text;

            DDL_YearList.SelectedValue = gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[5].Text;

            DDL_MonthList.SelectedValue = gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[6].Text;
            TB_WorkNumDetails.Text = gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[7].Text;

            int confirmID = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
            InitDataConfirmList(itemno, confirmID);
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiCaoZuoShiBai").ToString().Trim() + exp.Message;
        }
    }


    private bool IfExistsMonthWork(int id, int itemno, string month, bool bAdd)
    {
        try
        {
            //��ȡ�ɱ�����������Ϣ�б� 
            StringBuilder sql = new StringBuilder("From RCJProjectWorkDetails as rCJProjectWorkDetails where projectid=");
            sql.Append(ProjectId.ToString());
            sql.Append(" and WorkMonth='");
            sql.Append(month);
            sql.Append("' and itemno=");
            sql.Append(itemno.ToString());
            sql.Append(" and itemtype=");
            sql.Append(DDL_PerformanceType.SelectedValue);

            if (bAdd == false) //���޸ĵĻ�����Ҫ�ж��Ƿ�id
            {
                sql.Append(" and ID !=");
                sql.Append(id.ToString());
            }

            RCJProjectWorkDetailsBLL pwdBll = new RCJProjectWorkDetailsBLL();
            IList list;
            list = pwdBll.GetAllRCJProjectWorkDetails(sql.ToString());

            if (list.Count > 0)
                return true;
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiChaXunCaoZuoShiBai").ToString().Trim() + exp.Message;

            return true;
        }
        return false;
    }

    private bool SaveDataDetailsList(int bAdd, int selectId, int confirmID,string demo)
    {
        try
        {
            StringBuilder sql = new StringBuilder("exec Pro_WorkDetailsOperation ");
            sql.Append(selectId.ToString());
            sql.Append(",");
            sql.Append(bAdd.ToString());
            sql.Append(",");
            sql.Append(confirmID.ToString());
            sql.Append(",");
            sql.Append(ProjectId.ToString());
            sql.Append(",");
            sql.Append(DDL_PerformanceType.SelectedValue);
            sql.Append(",");
            sql.Append(itemno.ToString());
            sql.Append(",");
            sql.Append(AdjustId.ToString());
            sql.Append(",");
            sql.Append(DDL_YearList.Text);
            sql.Append(",");
            sql.Append(DDL_MonthList.Text);
            sql.Append(",");
            sql.Append(TB_WorkNumDetails.Text);
            sql.Append(",'");
            sql.Append(UserCode);
            sql.Append("','");
            sql.Append(DateTime.Now.ToString());
            sql.Append("','");
            sql.Append(demo);
            sql.Append("'");

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

    private int getMaxid()
    {
        int maxid = 0;
        StringBuilder strSql = new StringBuilder("select max(COALESCE(WorkConfirmID,0)) as maxid from T_RCJProjectWorkDetails where projectid=");
        strSql.Append(ProjectId.ToString());
        strSql.Append(" and itemtype=");
        strSql.Append(DDL_PerformanceType.SelectedValue);
        strSql.Append(" and itemno=");
        strSql.Append(itemno.ToString());
        DataSet ds = ShareClass.GetDataSetFromSql(strSql.ToString(), "T_RCJProjectWorkDetails");
        if (ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0 && !Convert.IsDBNull(ds.Tables[0].Rows[0]["maxid"]))
            maxid = Convert.ToInt32(ds.Tables[0].Rows[0]["maxid"]);

        return maxid;
    }

    //����µ�ConfirmID����ʽ������+��Ч���+01
    private int getNewConfirmID()
    {
        int maxid = getMaxid();

        string str = string.Format("{0}{1:00}{2:000}", int.Parse(DDL_PerformanceType.SelectedValue), int.Parse(itemno.ToString()), 1);

        int newid = int.Parse(str);

        if (newid <= maxid)
            newid = maxid + 1;

        return newid;
    }

    public double getHasInputDetailsWorkNum(GridView gv,int np , int IID)
    {
        double maxWorkNum = 0;

        for (int i = 0; i < gv.Rows.Count ; i++)
        {
            int index = Convert.ToInt32(gv.Rows[i].Cells[1].Text);
            if (IID > 0 && IID == index)
                continue;

            maxWorkNum += Convert.ToDouble(gv.Rows[i].Cells[np].Text);
        }

        return maxWorkNum;
    }

    //����ʵ�ʹ�����¼
    protected void BT_SaveWorkDetails_Click(object sender, EventArgs e)
    {
        if (TB_WorkNumDetails.Text.Trim().Length == 0 || false == ShareClass.CheckIsNumber(TB_WorkNumDetails.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuSh").ToString().Trim();
            return;
        }

        //�ж�Ŀǰ����Ĺ������Ƿ���ʵ�ʹ����Ĺ�������Χ
        double maxHasInputWorkNum = getHasInputDetailsWorkNum(gvWorkDetails,7,1);
        double currentInputWorkNum = Convert.ToDouble(TB_WorkNumDetails.Text);
        double theRestWorkNum = 0;
        if (RCJShareClass.FloatIsEqual(itemnum, maxHasInputWorkNum) == false)
        {
            theRestWorkNum =  itemnum - maxHasInputWorkNum;
        }

        if (RCJShareClass.FloatGT( currentInputWorkNum , theRestWorkNum) )
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = string.Format(LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuGo").ToString().Trim(), theRestWorkNum);
            return;
        }

        try
        {
            int confirmId = getNewConfirmID();
            if (false == SaveDataDetailsList(0, 0, confirmId, LanguageHandle.GetWord("ZengJiaShiJiGongZuoJiLu").ToString().Trim()))
                return;

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXinZengShiJiGongZuo").ToString().Trim();

            InitAllDataList(sender, e, gvWorkDetails.Rows.Count , 0);

        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXinZengShiJiGongZuo").ToString().Trim() + exp.Message;
        }
    }

    //�޸�ʵ�ʹ�����
    protected void BT_EditWorkDetails_Click(object sender, EventArgs e)
    {
        if (gvWorkDetails.SelectedIndex == -1)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeXuYaoXiuG").ToString().Trim();
            return;
        }

        if (TB_WorkNumDetails.Text.Trim().Length == 0 || false == ShareClass.CheckIsNumber(TB_WorkNumDetails.Text))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuSh").ToString().Trim();
            return;
        }

        //�ж�Ŀǰ����Ĺ������Ƿ���ʵ�ʹ����Ĺ�������Χ
        int si = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[1].Text);
        double maxHasInputWorkNum = getHasInputDetailsWorkNum(gvWorkDetails,7,si);
        double currentInputWorkNum = Convert.ToDouble(TB_WorkNumDetails.Text);
        double theRestWorkNum = 0;
        if (RCJShareClass.FloatIsEqual(itemnum, maxHasInputWorkNum) == false)
        {
            theRestWorkNum = itemnum - maxHasInputWorkNum;
        }

        if (RCJShareClass.FloatGT(currentInputWorkNum, theRestWorkNum))
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = string.Format(LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuGo").ToString().Trim(), theRestWorkNum);
            return;
        }

        try
        {
            int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
            if (false == SaveDataDetailsList(1, Convert.ToInt32(TB_ID.Text), confirmId, LanguageHandle.GetWord("XiuGaiShiJiGongZuoJiLu").ToString().Trim()))
                return;

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXiuGaiShiJiGongZuoL").ToString().Trim();

            InitAllDataList(sender, e, gvWorkDetails.SelectedIndex , 0);

        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXiuGaiShiJiGongZuoL").ToString().Trim() + exp.Message;
        }
    }

    //ɾ��ʵ�ʹ�����
    protected void BT_DelWorkDetails_Click(object sender, EventArgs e)
    {
        if (gvWorkDetails.SelectedIndex == -1)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeYiHangJin").ToString().Trim();
            return;
        }

        try
        {
            int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
            if (false == SaveDataDetailsList(2, Convert.ToInt32(TB_ID.Text), confirmId, LanguageHandle.GetWord("ShanChuShiJiGongZuoJiLu").ToString().Trim()))
                return; 

            lb_ShowMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiShanChuShiJiGongZuo").ToString().Trim();

            InitAllDataList(sender, e,0,0);
        }
        
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiShanChuShiJiGongZuo").ToString().Trim() + exp.Message;
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        StringBuilder url = new StringBuilder("TTRCJProjectCost.aspx?ProjectID=");
        url.Append(ProjectId.ToString());
        Response.Redirect(url.ToString());
    }

    protected void gvConfirmWork_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvWorkDetails.PageIndex = e.NewPageIndex;
        gvWorkDetails.SelectedIndex = -1;
        int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
        InitDataConfirmList(itemno, confirmId);
    }

    protected void DDL_PerformanceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitAllDataList(sender, e , 0 , 0);

    }

    private void ClearInputText()
    {
        TB_ID.Text = "";
        TB_WorkNumDetails.Text = "";
        TB_WorkNumConfirm.Text = "";
        TB_WorkNumMoney.Text = "";

        lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiMo").ToString().Trim();
        lb_ShowMessage.ForeColor = System.Drawing.Color.Red;

        lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiMo").ToString().Trim();
        lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;

        lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiMo").ToString().Trim();
        lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
    }

    private void ClearInputText2()
    {
        TB_ID.Text = "";
        TB_WorkNumConfirm.Text = "";
        TB_WorkNumMoney.Text = "";

        lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiMo").ToString().Trim();
        lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;

        lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiMo").ToString().Trim();
        lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
    }

    private void ClearInputText3()
    {
        TB_ID.Text = "";
        TB_WorkNumMoney.Text = "";

        lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiMo").ToString().Trim();
        lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
    }

    protected void gvWorkDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //��꾭��ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#A9A9A9'");
            //����Ƴ�ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
        }
    }

    protected void gvWorkConfirm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearInputText2();

            TB_ID.Text = gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text;
            TB_ItemName.Text = ItemName;
            TB_ItemContent.Text = ItemContent;
            DDL_YearListConfirm.SelectedValue = gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[4].Text;
            DDL_MonthListConfirm.SelectedValue = gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[5].Text;
            TB_WorkNumConfirm.Text = gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[6].Text;

            int confirmID = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
            int iid = Convert.ToInt32(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text);
            InitDataListMoney(itemno, confirmID,iid);
        }
        catch (Exception exp)
        {
            lb_ShowMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiCaoZuoShiBai").ToString().Trim() + exp.Message;
        }
    }

    protected void gvWorkConfirm_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (gvWorkConfirm.Rows.Count <= 0)
            return;

        gvWorkConfirm.PageIndex = e.NewPageIndex;
        gvWorkConfirm.SelectedIndex = -1;
        int confirmID = Convert.ToInt32( gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);

        InitAllDataList(sender, e, gvWorkDetails.SelectedIndex, 0);
    }

    protected void gvWorkConfirm_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //��꾭��ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#A9A9A9'");
            //����Ƴ�ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
        }
    }

    protected void BT_SaveWorkConfirm_Click(object sender, EventArgs e)
    {
        if (TB_WorkNumConfirm.Text.Trim().Length == 0 || false == ShareClass.CheckIsNumber(TB_WorkNumConfirm.Text))
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuGo").ToString().Trim();
            return;
        }
        //�ж�Ŀǰ����Ĺ������Ƿ���ʵ�ʹ����Ĺ�������Χ
        int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
        double maxHasInputWorkNum = getHasInputDetailsWorkNum(gvWorkConfirm, 6, 1);
        //(0, confirmId);
        double currentInputWorkNum = Convert.ToDouble(TB_WorkNumConfirm.Text);
        double workNumConfirm = Convert.ToDouble(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[7].Text);
        double theRestWorkNum = 0;
        if (RCJShareClass.Equals(workNumConfirm, maxHasInputWorkNum) == false)
        {
            theRestWorkNum = workNumConfirm - maxHasInputWorkNum;
        }

        if (RCJShareClass.FloatGT(currentInputWorkNum , theRestWorkNum))
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = string.Format(LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuGo").ToString().Trim(), theRestWorkNum);
            return;
        }

        try
        {
            if (false == SaveDataConfirmList(0, 0, confirmId))
                return;

            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXinZengGongZuoQueRe").ToString().Trim();

            InitAllDataList(sender, e, gvWorkDetails.SelectedIndex, gvWorkConfirm.Rows.Count);

        }
        catch (Exception exp)
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXinZengGongZuoQueRe").ToString().Trim() + exp.Message;
        }
    }

    protected void BT_EditWorkConfirm_Click(object sender, EventArgs e)
    {
        if (gvWorkConfirm.SelectedIndex == -1)
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeXuYaoXiuG").ToString().Trim();
            return;
        }

        if (TB_WorkNumConfirm.Text.Trim().Length == 0 || false == ShareClass.CheckIsNumber(TB_WorkNumConfirm.Text))
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuGo").ToString().Trim();
            return;
        }

        //�ж�Ŀǰ����Ĺ������Ƿ���ʵ�ʹ����Ĺ�������Χ
        int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
        int si = Convert.ToInt32(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text);
        double maxHasInputWorkNum = getHasInputDetailsWorkNum(gvWorkConfirm, 6, si);
        double currentInputWorkNum = Convert.ToDouble(TB_WorkNumConfirm.Text);
        double workNumConfirm = Convert.ToDouble(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[7].Text);
        double theRestWorkNum = 0;
        if (RCJShareClass.Equals(workNumConfirm, maxHasInputWorkNum) == false)
        {
            theRestWorkNum = workNumConfirm - maxHasInputWorkNum;
        }

        if (RCJShareClass.FloatGT(currentInputWorkNum , theRestWorkNum))
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = string.Format(LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuGo").ToString().Trim(), theRestWorkNum);
            return;
        }

        try
        {
            if (false == SaveDataConfirmList(1, Convert.ToInt32(TB_ID.Text),confirmId))
                return;

            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXiuGaiGongZuoQueRen").ToString().Trim();

            InitAllDataList(sender, e, gvWorkDetails.SelectedIndex, gvWorkConfirm.SelectedIndex);
        }
        catch (Exception exp)
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXiuGaiGongZuoQueRen").ToString().Trim() + exp.Message;
        }
    }
    protected void BT_DelWorkConfirm_Click(object sender, EventArgs e)
    {
        if (gvWorkConfirm.SelectedIndex == -1)
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeYiHangJin").ToString().Trim();
            return;
        }

        try
        {
            int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
            SaveDataConfirmList(2, Convert.ToInt32(TB_ID.Text), confirmId);

            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiShanChuGongZuoQueRe").ToString().Trim();

            InitAllDataList(sender, e , 0,0);

        }
        catch (Exception exp)
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiShanChuGongZuoQueRe").ToString().Trim() + exp.Message;
        }
    }

    private bool SaveDataConfirmList(int bAdd, int selectId, int confirmID)
    {
        try
        {
            StringBuilder sql = new StringBuilder("exec Pro_WorkConfirmOperation ");
            sql.Append(selectId.ToString());
            sql.Append(",");
            sql.Append(bAdd.ToString());
            sql.Append(",");
            sql.Append(confirmID.ToString());
            sql.Append(",");
            sql.Append(ProjectId.ToString());
            sql.Append(",");
            sql.Append(DDL_PerformanceType.SelectedValue);
            sql.Append(",");
            sql.Append(itemno.ToString());
            sql.Append(",");
            sql.Append(AdjustId.ToString());
            sql.Append(",");
            sql.Append(DDL_YearListConfirm.Text);
            sql.Append(",");
            sql.Append(DDL_MonthListConfirm.Text);
            sql.Append(",");
            sql.Append(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[7].Text); 
            sql.Append(",");
            sql.Append(TB_WorkNumConfirm.Text);
            sql.Append(",'");
            sql.Append(UserCode);
            sql.Append("','");
            sql.Append(DateTime.Now.ToString());
            sql.Append("'");

            ShareClass.RunSqlCommand(sql.ToString());

        }
        catch (Exception exp)
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("XiaoXiDiShi").ToString().Trim() + exp.Message;
            return false;
        }

        return true;
    }

    protected void gvWorkDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (gvWorkDetails.Rows.Count <= 0)
            return;
        gvWorkDetails.PageIndex = e.NewPageIndex;
        gvWorkDetails.SelectedIndex = -1;

        InitAllDataList(sender, e,0,0);
    }

    private void InitDataListMoney(int itemno, int confirmID,int iid)
    {
        StringBuilder sql = new StringBuilder(" select * from V_RCJProjectWorkMoney where ProjectID =");
        sql.Append(ProjectId.ToString());
        sql.Append(" and itemtype=");
        sql.Append(DDL_PerformanceType.SelectedValue);
        sql.Append(" and itemno=");
        sql.Append(itemno.ToString());
        sql.Append(" and WorkConfirmID=");
        sql.Append(confirmID.ToString());
        sql.Append(" and ApproveID=");
        sql.Append(iid.ToString());

        DataSet ds = ShareClass.GetDataSetFromSql(sql.ToString(), "V_RCJProjectWorkMoney");

        gvMoneyConfirm.DataSource = ds;
        gvMoneyConfirm.DataBind();

        gvMoneyConfirm.SelectedIndex = 0;
    }

    private bool SaveDataMoneyList(int bAdd, int selectId, int confirmID, int idd, string demo)
    {
        try
        {
            StringBuilder sql = new StringBuilder("exec Pro_WorkMoneyOperation ");
            sql.Append(selectId.ToString());
            sql.Append(",");
            sql.Append(bAdd.ToString());
            sql.Append(",");
            sql.Append(confirmID.ToString());
            sql.Append(",");
            sql.Append(ProjectId.ToString());
            sql.Append(",");
            sql.Append(DDL_PerformanceType.SelectedValue);
            sql.Append(",");
            sql.Append(itemno.ToString());
            sql.Append(",");
            sql.Append(DDL_YearListMoney.Text);
            sql.Append(",");
            sql.Append(DDL_MonthListMoney.Text);
            sql.Append(",");
            sql.Append(TB_WorkNumMoney.Text);
            sql.Append(",");
            sql.Append(idd); //TODO:�˴�ΪConfirm��Ӧ��ID������confirmID
            sql.Append(",");
            sql.Append(AdjustId.ToString());
            sql.Append(",'");
            sql.Append(UserCode);
            sql.Append("','");
            sql.Append(DateTime.Now.ToString());
            sql.Append("','");
            sql.Append(demo);
            sql.Append("'");

            ShareClass.RunSqlCommand(sql.ToString());

        }
        catch (Exception exp)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShi").ToString().Trim() + exp.Message;
            return false;
        }

        return true;
    }

    protected void BT_SaveWorkMoney_Click(object sender, EventArgs e)
    {
        if (TB_WorkNumMoney.Text.Trim().Length == 0 || false == ShareClass.CheckIsNumber(TB_WorkNumMoney.Text))
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuSh").ToString().Trim();
            return;
        }
        //�ж�Ŀǰ������տ���Ƿ���ʵ�ʹ����ķ�Χ
        int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
        double hasInputMoney = getHasInputDetailsWorkNum(gvMoneyConfirm, 6, 0);
        double currentInputMoney = Convert.ToDouble(TB_WorkNumMoney.Text);
        double workNum = Convert.ToDouble(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[6].Text);
        double totalMoney = Convert.ToDouble(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[14].Text);
        double theRestMoney = 0;
        if(RCJShareClass.Equals(totalMoney,hasInputMoney) == false)
            theRestMoney = totalMoney - hasInputMoney;

        if (RCJShareClass.FloatGT(currentInputMoney , theRestMoney))
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = string.Format(LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuSh").ToString().Trim(), theRestMoney);
            return;
        }

        try
        {
            int iid = Convert.ToInt32(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text);
            if (false == SaveDataMoneyList(0, 0, confirmId, iid,LanguageHandle.GetWord("ZengJiaShouKuanQueRenJiLu").ToString().Trim()))
                return;

            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXinZengShouKuanQueR").ToString().Trim();

            InitAllDataList(sender, e, gvWorkDetails.SelectedIndex, gvWorkConfirm.SelectedIndex);
        }
        catch (Exception exp)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXinZengShouKuanQueR").ToString().Trim() + exp.Message;
        }
    }
    protected void BT_EditWorkMoney_Click(object sender, EventArgs e)
    {
        if (gvMoneyConfirm.SelectedIndex == -1)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeXuYaoXiuG").ToString().Trim();
            return;
        }

        if (TB_WorkNumMoney.Text.Trim().Length == 0 || false == ShareClass.CheckIsNumber(TB_WorkNumMoney.Text))
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuSh").ToString().Trim();
            return;
        }
        //�ж�Ŀǰ����Ĺ������Ƿ���ʵ�ʹ����Ĺ�������Χ
        int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
        int si = Convert.ToInt32(gvMoneyConfirm.Rows[gvMoneyConfirm.SelectedIndex].Cells[1].Text);
        double hasInputMoney = getHasInputDetailsWorkNum(gvMoneyConfirm, 6, si);
        double currentInputMoney = Convert.ToDouble(TB_WorkNumMoney.Text);
        double workNum = Convert.ToDouble(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[6].Text);
        double totalMoney = Convert.ToDouble(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[14].Text);
        double theRestMoney = 0;
        if (RCJShareClass.Equals(totalMoney, hasInputMoney) == false)
            theRestMoney = totalMoney - hasInputMoney;

        if (RCJShareClass.FloatGT(currentInputMoney, theRestMoney))
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = string.Format(LanguageHandle.GetWord("XiaoXiDiShiQingZhengQueShuRuGo").ToString().Trim(), theRestMoney);
            return;
        }

        try
        {
            int iid = Convert.ToInt32(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text);
            if (false == SaveDataMoneyList(1, Convert.ToInt32(TB_ID.Text), confirmId,iid, LanguageHandle.GetWord("XiuGaiShouKuanQueRenJiLu").ToString().Trim()))
                return;

            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXiuGaiShouKuanQueRe").ToString().Trim();

            InitAllDataList(sender, e, gvWorkDetails.SelectedIndex, gvWorkConfirm.SelectedIndex);
        }
        catch (Exception exp)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiXiuGaiShouKuanQueRe").ToString().Trim() + exp.Message;
        }
    }
    protected void BT_DelWorkMoney_Click(object sender, EventArgs e)
    {
        if (gvMoneyConfirm.SelectedIndex == -1)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiQingShuaZeYiHangJin").ToString().Trim();
            return;
        }

        try
        {
            int iid = Convert.ToInt32(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text);
            int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
            if (false == SaveDataMoneyList(2, Convert.ToInt32(TB_ID.Text), confirmId,iid, LanguageHandle.GetWord("ShanChuShouKuanQueRenJiLu").ToString().Trim()))
                return;

            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Green;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiShanChuShouKuanQueR").ToString().Trim();

            InitAllDataList(sender, e, 0 ,0);
        }
        catch (Exception exp)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiShanChuShouKuanQueR").ToString().Trim() + exp.Message;
        }
    }

    protected void gvMoneyConfirm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearInputText3();

            TB_ID.Text = gvMoneyConfirm.Rows[gvMoneyConfirm.SelectedIndex].Cells[1].Text;
            DDL_YearListMoney.SelectedValue = gvMoneyConfirm.Rows[gvMoneyConfirm.SelectedIndex].Cells[4].Text;
            DDL_MonthListMoney.SelectedValue = gvMoneyConfirm.Rows[gvMoneyConfirm.SelectedIndex].Cells[5].Text;
            TB_WorkNumMoney.Text = Convert.ToDouble( gvMoneyConfirm.Rows[gvMoneyConfirm.SelectedIndex].Cells[6].Text ).ToString();
        }
        catch (Exception exp)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("XiaoXiDiShiCaoZuoShiBai").ToString().Trim() + exp.Message;
        }
    }

    protected void gvMoneyConfirm_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (gvMoneyConfirm.Rows.Count <= 0)
            return;
        gvMoneyConfirm.PageIndex = e.NewPageIndex;
        gvMoneyConfirm.SelectedIndex = -1;
        int confirmId = Convert.ToInt32(gvWorkDetails.Rows[gvWorkDetails.SelectedIndex].Cells[2].Text);
        int iid = Convert.ToInt32(gvWorkConfirm.Rows[gvWorkConfirm.SelectedIndex].Cells[1].Text);

        InitAllDataList(sender, e, gvWorkDetails.SelectedIndex, gvWorkConfirm.SelectedIndex);
    }


    protected void gvMoneyConfirm_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //��꾭��ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#A9A9A9'");
            //����Ƴ�ʱ���б���ɫ�� 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
        }
    }

    protected void LB_QueryWorkMoneyLog_Click(object sender, EventArgs e)
    {
        if (gvWorkConfirm.SelectedIndex < 0)
        {
            lb_ShowMoneyMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowMoneyMessage.Text = LanguageHandle.GetWord("WeiShuaZeDuiYingDeQueRenGongZu").ToString().Trim();
            return;
        }

        StringBuilder sb = new StringBuilder("TTRCJProjectWorkMoney.aspx?projectid=");
        sb.Append(ProjectId.ToString());
        sb.Append("&itemtype=");
        sb.Append(DDL_PerformanceType.SelectedValue);
        sb.Append("&itemno=");
        sb.Append(itemno);
        sb.Append("&adjustid=");
        sb.Append(AdjustId);

        Response.Redirect(sb.ToString());
    }
    protected void LB_WorkConfirmLog_Click(object sender, EventArgs e)
    {
        if (gvWorkDetails.SelectedIndex < 0)
        {
            lb_ShowConfirmMessage.ForeColor = System.Drawing.Color.Red;
            lb_ShowConfirmMessage.Text = LanguageHandle.GetWord("WeiShuaZeDuiYingDeQueRenGongZu").ToString().Trim();
            return;
        } 
        
        StringBuilder sb = new StringBuilder("TTRCJProjectWorkConfirm.aspx?projectid=");
        sb.Append(ProjectId.ToString());
        sb.Append("&itemtype=");
        sb.Append(DDL_PerformanceType.SelectedValue);
        sb.Append("&itemno=");
        sb.Append(itemno);
        sb.Append("&adjustid=");
        sb.Append(AdjustId);

        Response.Redirect(sb.ToString());
    }
    protected void LB_WorkDetailsLog_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder("TTRCJProjectWorkDetailsLog.aspx?projectid=");
        sb.Append(ProjectId.ToString());
        sb.Append("&itemtype=");
        sb.Append(DDL_PerformanceType.SelectedValue);
        sb.Append("&itemno=");
        sb.Append(itemno);
        sb.Append("&adjustid=");
        sb.Append(AdjustId);

        Response.Redirect(sb.ToString());
    }
    protected void DDL_YearList_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetAllMonthsList(ProjectId, DDL_PerformanceType.SelectedValue, itemno, DDL_YearList.SelectedValue);
    }
}