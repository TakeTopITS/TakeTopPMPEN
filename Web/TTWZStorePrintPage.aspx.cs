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

public partial class TTWZStorePrintPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         string strUserCode = Session["UserCode"].ToString();if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["StockCode"]) || !string.IsNullOrEmpty(Request.QueryString["Year"]) || !string.IsNullOrEmpty(Request.QueryString["Month"]))
            {
                string strStockCode = Request.QueryString["StockCode"].ToString();
                string strYear = Request.QueryString["Year"].ToString();
                string strMonth = Request.QueryString["Month"].ToString();

                DataBinder(strStockCode, strYear, strMonth);

                LT_StockCode.Text = strStockCode;
                LT_YearMonth.Text = strYear +"-" + strMonth;
                LT_CurrentTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
    }

    private void DataBinder(string strStockCode, string strYear,string strMonth)
    {
        int intYear = 0;
        int.TryParse(strYear, out intYear);
        int intMonth = 0;
        int.TryParse(strMonth, out intMonth);

        int intPreviousMonth = intMonth - 1;

        string strPreviousMonth = string.Empty;
        string strPreviousYear = string.Empty;
        if (intPreviousMonth <= 0)
        {
            strPreviousMonth = "12";
            strPreviousYear = (intYear - 1).ToString();
        }
        else
        {
            strPreviousMonth = intPreviousMonth.ToString();
            strPreviousYear = intYear.ToString();
        }

        int intNextMonth = intMonth + 1;

        string strNextMonth = string.Empty;
        string strNextYear = string.Empty;

        if (intNextMonth >= 13)
        {
            strNextMonth = "1";
            strNextYear = (intYear + 1).ToString();
        }
        else
        {
            strNextMonth = intNextMonth.ToString();
            strNextYear = intYear.ToString();
        }

        string strHQL = string.Format(@"select 
                        y.DLCode,
                        l.DLName,
                        ���ڽ�� = y.���ڽ��,
                        �����ն� = y.�����ն�,
                        �ۼ��ն� = y.�ۼ��ն�,
                        ���ڷ��� = y.���ڷ���,
                        �ۼƷ��� = y.�ۼƷ���,
                        ���ڽ�� = y.���ڽ��,
                        ƽ��ռ�� = (y.���ڽ�� + y.���ڽ��) / 2
                        from
                        (
                        select 
                        DLCode,
                        ���ڽ�� = x.���ڽ��,
                        �����ն� = x.�����ն�,
                        �ۼ��ն� = x.�ۼ��ն�,
                        ���ڷ��� = x.���ڷ���,
                        �ۼƷ��� = x.�ۼƷ���,
                        ���ڽ�� = x.���ڽ�� + x.�����ն� - x.���ڷ���
                        from
                        (
                        select 
                        DLCode,
                        ���ڽ�� = t.��ʼ���+t.����ĩ֮ǰ��Ч���ϵ���ʵ������ܺ�-t.����ĩ֮ǰ��Ч���ϵ��ļƻ�����ܺ�,
                        �����ն� = t.������Ч���ϵ���ʵ������ܺ�,
                        �ۼ��ն� = t.�������Ч���ϵ���ʵ������ܺ�,
                        ���ڷ��� = t.������Ч���ϵ��ļƻ�����ܺ�,
                        �ۼƷ��� = t.�������Ч���ϵ��ļƻ�����ܺ�
                        from
                        (
                        select a.DLCode,
                        COALESCE(SumYearMoney,0) as ��ʼ���, 
                        COALESCE(b.ActualMoney,0) as ����ĩ֮ǰ��Ч���ϵ���ʵ������ܺ�,
                        COALESCE(c.PlanMoney,0) as ����ĩ֮ǰ��Ч���ϵ��ļƻ�����ܺ�,
                        COALESCE(d.ActualMoney,0) as ������Ч���ϵ���ʵ������ܺ�,
                        COALESCE(e.ActualMoney,0) as �������Ч���ϵ���ʵ������ܺ�,
                        COALESCE(f.PlanMoney,0) as ������Ч���ϵ��ļƻ�����ܺ�,
                        COALESCE(g.PlanMoney,0) as �������Ч���ϵ��ļƻ�����ܺ�
                        from
                        (
                        select SUBSTRING(ObjectCode, 0, 3) as DLCode,
                        SumYearMoney = SUM(YearMoney)
                        from T_WZStore
                        where StockCode like '%{0}%'
                        group by SUBSTRING(ObjectCode, 0, 3)
                        ) a
                        left join
                        (
                        select SUBSTRING(ObjectCode,0,3) as DLCode,
                        SUM(ActualMoney) as ActualMoney
                        from T_WZCollect
                        where IsMark = -1
                        and to_char( CollectTime::timestamp, 'yyyy-mm-dd' ) < cast( {1} as varchar(4))||'-'||cast({2} as varchar(2))||'-01'
                        group by SUBSTRING(ObjectCode,0,3)
                        ) b
                        on a.DLCode = b.DLCode
                        left join
                        (
                        select SUBSTRING(ObjectCode,0,3) as DLCode,
                        SUM(PlanMoney) as PlanMoney
                        from T_WZSend
                        where IsMark = -1
                        and to_char( SendTime::timestamp, 'yyyy-mm-dd' ) < cast({1} as varchar(4))||'-'||cast({2} as varchar(2))||'-01'
                        group by SUBSTRING(ObjectCode,0,3)
                        ) c
                        on a.DLCode = c.DLCode
                        left join
                        (
                        select SUBSTRING(ObjectCode,0,3) as DLCode,
                        SUM(ActualMoney) as ActualMoney
                        from T_WZCollect
                        where IsMark = -1
                        and to_char( CollectTime::timestamp, 'yyyy-mm-dd' ) > cast({5} as varchar(4))||'-'||cast( {3} as varchar(2))||'-01'
                        and to_char( CollectTime::timestamp, 'yyyy-mm-dd' ) < cast({6} as varchar(4))||'-'||cast( {4} as varchar(2))||'-01'
                        group by SUBSTRING(ObjectCode,0,3)
                        ) d
                        on a.DLCode = d.DLCode
                        left join
                        (
                        select SUBSTRING(ObjectCode,0,3) as DLCode,
                        SUM(ActualMoney) as ActualMoney
                        from T_WZCollect
                        where IsMark = -1
                        and extract(year from CollectTime::timestamp) =  {1}
                        group by SUBSTRING(ObjectCode,0,3)
                        ) e
                        on a.DLCode = e.DLCode
                        left join
                        (
                        select SUBSTRING(ObjectCode,0,3) as DLCode,
                        SUM(PlanMoney) as PlanMoney
                        from T_WZSend
                        where IsMark = -1
                        and to_char( SendTime::timestamp, 'yyyy-mm-dd' ) > cast({5} as varchar(4))||'-'||cast({3} as varchar(2))||'-01'
                        and to_char( SendTime::timestamp, 'yyyy-mm-dd' ) < cast({6} as varchar(4))||'-'||cast({4} as varchar(2))||'-01'
                        group by SUBSTRING(ObjectCode,0,3)
                        ) f
                        on a.DLCode = f.DLCode
                        left join
                        (
                        select SUBSTRING(ObjectCode,0,3) as DLCode,
                        SUM(PlanMoney) as PlanMoney
                        from T_WZSend
                        where IsMark = -1
                        and extract(year from SendTime::timestamp) = {1}
                        group by SUBSTRING(ObjectCode,0,3)
                        ) g
                        on a.DLCode = g.DLCode
                        ) t 
                        ) x
                        ) y
                        left join T_WZMaterialDL l on y.DLCode = l.DLCode", strStockCode, strYear, strPreviousMonth, strMonth, strNextMonth, strPreviousYear, strNextYear);

        DataTable dtStore = ShareClass.GetDataSetFromSql(strHQL, "Store").Tables[0];

        decimal decimalPreviousMoney = 0;               //���ڽ��
        decimal decimalCurrentCollectMoney = 0;         //�����ն�
        decimal decimalTotalCollectMoney = 0;           //�ۼ��ն�
        decimal decimalCurrentSendMoney = 0;            //���ڷ���
        decimal decimalTotalSendMoney = 0;              //�ۼƷ���
        decimal decimalCurrentMoney = 0;                //���ڽ��
        decimal decimalAvgMoney = 0;                         //ƽ��ռ��

        foreach (DataRow drStore in dtStore.Rows)
        {
            decimal decimalStorePreviousMoney = 0;
            decimal.TryParse(ShareClass.ObjectToString(drStore["���ڽ��"]), out decimalStorePreviousMoney);
            decimalPreviousMoney += decimalStorePreviousMoney;

            decimal decimalStoreCurrentCollectMoney = 0;
            decimal.TryParse(ShareClass.ObjectToString(drStore["�����ն�"]), out decimalStoreCurrentCollectMoney);
            decimalCurrentCollectMoney += decimalStoreCurrentCollectMoney;

            decimal decimalStoreTotalCollectMoney = 0;
            decimal.TryParse(ShareClass.ObjectToString(drStore["�ۼ��ն�"]), out decimalStoreTotalCollectMoney);
            decimalTotalCollectMoney += decimalStoreTotalCollectMoney;

            decimal decimalStoreCurrentSendMoney = 0;
            decimal.TryParse(ShareClass.ObjectToString(drStore["���ڷ���"]), out decimalStoreCurrentSendMoney);
            decimalCurrentSendMoney += decimalStoreCurrentSendMoney;

            decimal decimalStoreTotalSendMoney = 0;
            decimal.TryParse(ShareClass.ObjectToString(drStore["�ۼƷ���"]), out decimalStoreTotalSendMoney);
            decimalTotalSendMoney += decimalStoreTotalSendMoney;

            decimal decimalStoreCurrentMoney = 0;
            decimal.TryParse(ShareClass.ObjectToString(drStore["���ڽ��"]), out decimalStoreCurrentMoney);
            decimalCurrentMoney += decimalStoreCurrentMoney;
        }

        decimalAvgMoney = (decimalPreviousMoney + decimalCurrentMoney) / 2;


        LT_PreviousMoney.Text = decimalPreviousMoney.ToString() +"";
        LT_CurrentCollectMoney.Text = decimalCurrentCollectMoney.ToString() + "";
        LT_TotalCollectMoney.Text = decimalTotalCollectMoney.ToString() + "";
        LT_CurrentSendMoney.Text = decimalCurrentSendMoney.ToString();
        LT_TotalSendMoney.Text = decimalTotalSendMoney.ToString();
        LT_CurrentMoney.Text = decimalCurrentMoney.ToString();
        LT_AvgMoney.Text = decimalAvgMoney.ToString();


        RT_List.DataSource = dtStore;
        RT_List.DataBind();
    }

    protected void RT_List_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
}